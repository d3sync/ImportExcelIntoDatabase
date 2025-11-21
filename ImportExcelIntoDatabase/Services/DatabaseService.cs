using Dapper;
using ImportExcelIntoDatabase.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace ImportExcelIntoDatabase.Services
{
    public class DatabaseService
    {
        public async Task<bool> TestConnection(string connectionString)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public async Task<List<string>> GetDatabases(string serverName, string username, string password, bool useWindowsAuth)
        {
            var connectionString = BuildConnectionString(serverName, "master", username, password, useWindowsAuth);
            
            using var connection = new SqlConnection(connectionString);
            var databases = await connection.QueryAsync<string>(
                "SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb') ORDER BY name"
            );
            
            return databases.ToList();
        }
        
        public async Task<List<string>> GetTables(string serverName, string databaseName, string username, string password, bool useWindowsAuth)
        {
            var connectionString = BuildConnectionString(serverName, databaseName, username, password, useWindowsAuth);
            
            using var connection = new SqlConnection(connectionString);
            var tables = await connection.QueryAsync<string>(
                "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME"
            );
            
            return tables.ToList();
        }
        
        public async Task<List<(string ColumnName, string DataType)>> GetTableColumns(
            string serverName, string databaseName, string tableName, 
            string username, string password, bool useWindowsAuth)
        {
            var connectionString = BuildConnectionString(serverName, databaseName, username, password, useWindowsAuth);
            
            using var connection = new SqlConnection(connectionString);
            var columns = await connection.QueryAsync<(string, string)>(
                @"SELECT COLUMN_NAME, DATA_TYPE 
                  FROM INFORMATION_SCHEMA.COLUMNS 
                  WHERE TABLE_NAME = @TableName 
                  ORDER BY ORDINAL_POSITION",
                new { TableName = tableName }
            );
            
            return columns.ToList();
        }
        
        public async Task<ImportResult> ImportDataWithErrorHandling(
            string serverName, string databaseName, string tableName,
            string username, string password, bool useWindowsAuth,
            List<ColumnMapping> mappings, List<List<object>> rows,
            ErrorHandlingStrategy errorStrategy,
            IProgress<(int Current, int Total, string Message)> progress)
        {
            var result = new ImportResult
            {
                TotalRows = rows.Count,
                StartTime = DateTime.Now
            };
            
            var connectionString = BuildConnectionString(serverName, databaseName, username, password, useWindowsAuth);
            
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            
            // Get selected mappings
            var selectedMappings = mappings.Where(m => m.IsSelected).ToList();
            
            if (selectedMappings.Count == 0)
                throw new InvalidOperationException("No columns selected for import");
            
            // ? PERFORMANCE OPTIMIZATION: Use SqlBulkCopy for large datasets (1000+ rows)
            // This is 10-100x faster than row-by-row inserts
            if (rows.Count >= 1000 && errorStrategy == ErrorHandlingStrategy.UseTransaction)
            {
                return await ImportWithBulkCopy(
                    connection, tableName, mappings, selectedMappings, rows, result, progress);
            }
            
            // Build INSERT statement for smaller datasets or error handling strategies
            var columnNames = string.Join(", ", selectedMappings.Select(m => $"[{m.SqlColumn}]"));
            var parameterNames = string.Join(", ", selectedMappings.Select((m, i) => $"@p{i}"));
            var insertSql = $"INSERT INTO [{tableName}] ({columnNames}) VALUES ({parameterNames})";
            
            switch (errorStrategy)
            {
                case ErrorHandlingStrategy.UseTransaction:
                    await ImportWithTransaction(connection, insertSql, mappings, selectedMappings, rows, result, progress);
                    break;
                    
                case ErrorHandlingStrategy.StopOnFirstError:
                    await ImportStopOnError(connection, insertSql, mappings, selectedMappings, rows, result, progress);
                    break;
                    
                case ErrorHandlingStrategy.SkipErrorsAndContinue:
                    await ImportSkipErrors(connection, insertSql, mappings, selectedMappings, rows, result, progress);
                    break;
            }
            
            result.EndTime = DateTime.Now;
            return result;
        }
        
        /// <summary>
        /// ? HIGH PERFORMANCE: Uses SqlBulkCopy for large datasets (10-100x faster than INSERT)
        /// Best for: 1,000+ rows with clean data
        /// </summary>
        private async Task<ImportResult> ImportWithBulkCopy(
            SqlConnection connection, string tableName,
            List<ColumnMapping> allMappings, List<ColumnMapping> selectedMappings,
            List<List<object>> rows, ImportResult result,
            IProgress<(int Current, int Total, string Message)> progress)
        {
            try
            {
                progress?.Report((0, result.TotalRows, 
                    $"?? Using high-performance bulk import for {result.TotalRows} rows..."));
                
                // Create DataTable with proper schema
                var dataTable = new DataTable();
                
                // Add columns with appropriate data types
                foreach (var mapping in selectedMappings)
                {
                    var columnType = InferClrType(mapping.DataType);
                    dataTable.Columns.Add(mapping.SqlColumn, columnType);
                }
                
                // Populate DataTable
                int currentRow = 0;
                foreach (var row in rows)
                {
                    currentRow++;
                    
                    var dataRow = dataTable.NewRow();
                    
                    for (int i = 0; i < selectedMappings.Count; i++)
                    {
                        var mapping = selectedMappings[i];
                        var excelColumnIndex = allMappings.FindIndex(m => m.ExcelColumn == mapping.ExcelColumn);
                        
                        object? value = null;
                        if (excelColumnIndex >= 0 && excelColumnIndex < row.Count)
                        {
                            value = row[excelColumnIndex];
                            
                            // Handle empty or whitespace strings
                            if (value is string str && string.IsNullOrWhiteSpace(str))
                            {
                                value = DBNull.Value;
                            }
                            else if (value == null)
                            {
                                value = DBNull.Value;
                            }
                            else
                            {
                                // Convert to appropriate type
                                value = ConvertToClrType(value, mapping.DataType);
                            }
                        }
                        else
                        {
                            value = DBNull.Value;
                        }
                        
                        dataRow[mapping.SqlColumn] = value;
                    }
                    
                    dataTable.Rows.Add(dataRow);
                    
                    if (currentRow % 500 == 0)
                    {
                        progress?.Report((currentRow, result.TotalRows, 
                            $"Preparing data for bulk import... ({currentRow} of {result.TotalRows})"));
                    }
                }
                
                // Configure SqlBulkCopy for optimal performance
                using var bulkCopy = new SqlBulkCopy(connection)
                {
                    DestinationTableName = $"[{tableName}]",
                    BatchSize = 5000, // Process in batches of 5000 rows
                    BulkCopyTimeout = 600, // 10 minute timeout for very large datasets
                    EnableStreaming = true, // Memory efficient for large datasets
                    NotifyAfter = 1000 // Progress notification every 1000 rows
                };
                
                // Map columns
                foreach (var mapping in selectedMappings)
                {
                    bulkCopy.ColumnMappings.Add(mapping.SqlColumn, mapping.SqlColumn);
                }
                
                // Progress reporting
                bulkCopy.SqlRowsCopied += (sender, e) =>
                {
                    progress?.Report(((int)e.RowsCopied, result.TotalRows, 
                        $"? Bulk importing... {e.RowsCopied:N0} of {result.TotalRows:N0} rows ({(e.RowsCopied * 100 / result.TotalRows)}%)"));
                };
                
                // Perform the bulk copy
                await bulkCopy.WriteToServerAsync(dataTable);
                
                result.SuccessfulRows = result.TotalRows;
                progress?.Report((result.TotalRows, result.TotalRows, 
                    $"? Bulk import completed! {result.TotalRows:N0} rows imported successfully in {(DateTime.Now - result.StartTime).TotalSeconds:F1} seconds"));
            }
            catch (Exception ex)
            {
                result.FailedRows = result.TotalRows;
                result.Errors.Add(new ImportError
                {
                    RowNumber = 0,
                    ErrorMessage = $"Bulk import failed: {ex.Message}",
                    Exception = ex
                });
                
                throw new InvalidOperationException(
                    $"Bulk import failed. This usually means data validation issues.\n\n" +
                    $"?? Suggestions:\n" +
                    $"   • Use 'Validate Data' first to check for issues\n" +
                    $"   • Try 'Skip Errors' mode for problematic data\n" +
                    $"   • Check data types match between Excel and SQL\n\n" +
                    $"Original error: {ex.Message}", ex);
            }
            
            result.EndTime = DateTime.Now;
            return result;
        }
        
        /// <summary>
        /// Infers CLR type from SQL data type for DataTable columns
        /// </summary>
        private Type InferClrType(string sqlDataType)
        {
            if (string.IsNullOrEmpty(sqlDataType))
                return typeof(string);
            
            var lowerType = sqlDataType.ToLower();
            
            // Remove size specifications
            if (lowerType.Contains('('))
                lowerType = lowerType.Substring(0, lowerType.IndexOf('('));
            
            return lowerType switch
            {
                "int" or "smallint" => typeof(int),
                "bigint" => typeof(long),
                "tinyint" => typeof(byte),
                "decimal" or "numeric" or "money" or "smallmoney" => typeof(decimal),
                "float" => typeof(double),
                "real" => typeof(float),
                "bit" => typeof(bool),
                "date" or "datetime" or "datetime2" or "smalldatetime" => typeof(DateTime),
                "uniqueidentifier" => typeof(Guid),
                _ => typeof(string) // Default to string for all text types
            };
        }
        
        /// <summary>
        /// Converts Excel value to appropriate CLR type
        /// </summary>
        private object? ConvertToClrType(object value, string sqlDataType)
        {
            if (value == null || value == DBNull.Value)
                return DBNull.Value;
            
            var stringValue = value.ToString();
            if (string.IsNullOrWhiteSpace(stringValue))
                return DBNull.Value;
            
            var lowerType = sqlDataType.ToLower();
            if (lowerType.Contains('('))
                lowerType = lowerType.Substring(0, lowerType.IndexOf('('));
            
            try
            {
                return lowerType switch
                {
                    "int" or "smallint" => int.TryParse(stringValue, out var i) ? i : value,
                    "bigint" => long.TryParse(stringValue, out var l) ? l : value,
                    "tinyint" => byte.TryParse(stringValue, out var b) ? b : value,
                    "decimal" or "numeric" or "money" or "smallmoney" => 
                        decimal.TryParse(stringValue, out var d) ? d : value,
                    "float" => double.TryParse(stringValue, out var dbl) ? dbl : value,
                    "real" => float.TryParse(stringValue, out var f) ? f : value,
                    "bit" => ConvertToBool(stringValue),
                    "date" or "datetime" or "datetime2" or "smalldatetime" => 
                        value is DateTime dt ? dt : 
                        DateTime.TryParse(stringValue, out var date) ? date : value,
                    _ => value // Keep as-is for string types
                };
            }
            catch
            {
                // If conversion fails, return original value
                return value;
            }
        }
        
        private object ConvertToBool(string value)
        {
            var lower = value.ToLower().Trim();
            if (lower == "1" || lower == "true" || lower == "yes" || lower == "y")
                return true;
            if (lower == "0" || lower == "false" || lower == "no" || lower == "n")
                return false;
            return bool.TryParse(value, out var b) ? b : value;
        }
        
        private async Task ImportWithTransaction(
            SqlConnection connection, string insertSql,
            List<ColumnMapping> mappings, List<ColumnMapping> selectedMappings,
            List<List<object>> rows, ImportResult result,
            IProgress<(int Current, int Total, string Message)> progress)
        {
            using var transaction = connection.BeginTransaction();
            
            try
            {
                int current = 0;
                
                foreach (var row in rows)
                {
                    current++;
                    
                    try
                    {
                        var parameters = BuildParameters(selectedMappings, mappings, row);
                        await connection.ExecuteAsync(insertSql, parameters, transaction);
                        result.SuccessfulRows++;
                        
                        if (current % 10 == 0 || current == result.TotalRows)
                        {
                            progress?.Report((current, result.TotalRows, 
                                $"Importing row {current} of {result.TotalRows}... ({result.SuccessfulRows} successful)"));
                        }
                    }
                    catch (Exception ex)
                    {
                        // On any error, rollback and throw
                        result.FailedRows++;
                        
                        var friendlyError = TranslateErrorMessage(ex, current, row, selectedMappings);
                        
                        result.Errors.Add(new ImportError
                        {
                            RowNumber = current,
                            ErrorMessage = friendlyError,
                            RowData = row,
                            Exception = ex
                        });
                        
                        await transaction.RollbackAsync();
                        throw new InvalidOperationException(
                            $"Import failed at row {current}. All changes have been rolled back.\n\n" +
                            $"{friendlyError}\n\n" +
                            $"Original error: {ex.Message}", ex);
                    }
                }
                
                // If we got here, commit all changes
                await transaction.CommitAsync();
                progress?.Report((result.TotalRows, result.TotalRows, 
                    $"Transaction committed! {result.SuccessfulRows} rows imported successfully."));
            }
            catch
            {
                // Transaction already rolled back in the catch block above
                throw;
            }
        }
        
        private async Task ImportStopOnError(
            SqlConnection connection, string insertSql,
            List<ColumnMapping> mappings, List<ColumnMapping> selectedMappings,
            List<List<object>> rows, ImportResult result,
            IProgress<(int Current, int Total, string Message)> progress)
        {
            int current = 0;
            
            foreach (var row in rows)
            {
                current++;
                
                try
                {
                    var parameters = BuildParameters(selectedMappings, mappings, row);
                    await connection.ExecuteAsync(insertSql, parameters);
                    result.SuccessfulRows++;
                    
                    if (current % 10 == 0 || current == result.TotalRows)
                    {
                        progress?.Report((current, result.TotalRows, 
                            $"Importing row {current} of {result.TotalRows}... ({result.SuccessfulRows} successful)"));
                    }
                }
                catch (Exception ex)
                {
                    result.FailedRows++;
                    
                    var friendlyError = TranslateErrorMessage(ex, current, row, selectedMappings);
                    
                    result.Errors.Add(new ImportError
                    {
                        RowNumber = current,
                        ErrorMessage = friendlyError,
                        RowData = row,
                        Exception = ex
                    });
                    
                    throw new InvalidOperationException(
                        $"Import stopped at row {current}. {result.SuccessfulRows} rows were imported before the error.\n\n" +
                        $"{friendlyError}\n\n" +
                        $"Original error: {ex.Message}", ex);
                }
            }
        }
        
        private async Task ImportSkipErrors(
            SqlConnection connection, string insertSql,
            List<ColumnMapping> mappings, List<ColumnMapping> selectedMappings,
            List<List<object>> rows, ImportResult result,
            IProgress<(int Current, int Total, string Message)> progress)
        {
            int current = 0;
            
            foreach (var row in rows)
            {
                current++;
                
                try
                {
                    var parameters = BuildParameters(selectedMappings, mappings, row);
                    await connection.ExecuteAsync(insertSql, parameters);
                    result.SuccessfulRows++;
                }
                catch (Exception ex)
                {
                    result.FailedRows++;
                    
                    var friendlyError = TranslateErrorMessage(ex, current, row, selectedMappings);
                    
                    result.Errors.Add(new ImportError
                    {
                        RowNumber = current,
                        ErrorMessage = friendlyError,
                        RowData = row,
                        Exception = ex
                    });
                }
                
                if (current % 10 == 0 || current == result.TotalRows)
                {
                    progress?.Report((current, result.TotalRows, 
                        $"Processing row {current} of {result.TotalRows}... ({result.SuccessfulRows} successful, {result.FailedRows} failed)"));
                }
            }
        }
        
        private DynamicParameters BuildParameters(
            List<ColumnMapping> selectedMappings,
            List<ColumnMapping> allMappings,
            List<object> row)
        {
            var parameters = new DynamicParameters();
            
            for (int i = 0; i < selectedMappings.Count; i++)
            {
                var mapping = selectedMappings[i];
                var excelColumnIndex = allMappings.FindIndex(m => m.ExcelColumn == mapping.ExcelColumn);
                
                object? value = null;
                if (excelColumnIndex >= 0 && excelColumnIndex < row.Count)
                {
                    value = row[excelColumnIndex];
                    
                    // Handle empty or whitespace strings
                    if (value is string str && string.IsNullOrWhiteSpace(str))
                    {
                        value = null;
                    }
                }
                
                // Dapper handles null values appropriately
                // NULL will be sent to the database for nullable columns
                // Non-nullable columns will cause a SQL error with our friendly message
                parameters.Add($"@p{i}", value);
            }
            
            return parameters;
        }
        
        // Keep old method for backward compatibility
        public async Task<int> ImportData(
            string serverName, string databaseName, string tableName,
            string username, string password, bool useWindowsAuth,
            List<ColumnMapping> mappings, List<List<object>> rows,
            IProgress<(int Current, int Total, string Message)> progress)
        {
            var result = await ImportDataWithErrorHandling(
                serverName, databaseName, tableName,
                username, password, useWindowsAuth,
                mappings, rows,
                ErrorHandlingStrategy.SkipErrorsAndContinue,
                progress);
            
            return result.SuccessfulRows;
        }
        
        public async Task CreateTable(
            string serverName, string databaseName, string tableName,
            string username, string password, bool useWindowsAuth,
            List<string> columnNames, List<string> dataTypes)
        {
            var connectionString = BuildConnectionString(serverName, databaseName, username, password, useWindowsAuth);
            
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            
            var sql = new StringBuilder();
            sql.AppendLine($"CREATE TABLE [{tableName}] ("
            );
            sql.AppendLine($"    [ID] INT IDENTITY(1,1) PRIMARY KEY,"
            );
            
            for (int i = 0; i < columnNames.Count; i++)
            {
                var columnName = columnNames[i];
                var dataType = dataTypes[i];
                
                // Add column definition
                sql.Append($"    [{columnName}] {dataType}");
                
                // Add comma if not last column
                if (i < columnNames.Count - 1)
                {
                    sql.AppendLine(","
                    );
                }
                else
                {
                    sql.AppendLine();
                }
            }
            
            sql.AppendLine(");"
            );
            
            await connection.ExecuteAsync(sql.ToString()
            );
        }
        
        public string InferSqlDataType(List<object> sampleData)
        {
            if (sampleData == null || sampleData.Count == 0)
                return "NVARCHAR(255)";
            
            // Check for numeric types
            bool allIntegers = true;
            bool allDecimals = true;
            bool allDates = true;
            bool allBooleans = true;
            int maxLength = 0;
            
            foreach (var value in sampleData)
            {
                if (value == null || value.ToString() == string.Empty)
                    continue;
                
                var stringValue = value.ToString()!;
                maxLength = Math.Max(maxLength, stringValue.Length);
                
                // Check integer
                if (allIntegers && !int.TryParse(stringValue, out _))
                {
                    allIntegers = false;
                }
                
                // Check decimal
                if (allDecimals && !decimal.TryParse(stringValue, out _))
                {
                    allDecimals = false;
                }
                
                // Check date
                if (allDates && !DateTime.TryParse(stringValue, out _))
                {
                    allDates = false;
                }
                
                // Check boolean
                if (allBooleans && !bool.TryParse(stringValue, out _) && 
                    stringValue != "1" && stringValue != "0")
                {
                    allBooleans = false;
                }
            }
            
            // Return appropriate type
            if (allBooleans)
                return "BIT";
            
            if (allIntegers)
                return "INT";
            
            if (allDecimals)
                return "DECIMAL(18,2)";
            
            if (allDates)
                return "DATE";
            
            // Default to NVARCHAR with appropriate length
            int suggestedLength = Math.Max(50, Math.Min(maxLength * 2, 4000));
            return $"NVARCHAR({suggestedLength})";
        }
        
        public string BuildConnectionString(string serverName, string databaseName, 
            string username, string password, bool useWindowsAuth)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = serverName,
                InitialCatalog = databaseName,
                TrustServerCertificate = true
            };
            
            if (useWindowsAuth)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.UserID = username;
                builder.Password = password;
            }
            
            return builder.ConnectionString;
        }
        
        public async Task<ValidationResult> ValidateDataBeforeImport(
            string serverName, string databaseName, string tableName,
            string username, string password, bool useWindowsAuth,
            List<ColumnMapping> mappings, List<List<object>> rows,
            IProgress<(int Current, int Total, string Message)> progress)
        {
            var result = new ValidationResult
            {
                TotalRowsValidated = rows.Count
            };
            
            var connectionString = BuildConnectionString(serverName, databaseName, username, password, useWindowsAuth);
            
            // Get table column information
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            
            var selectedMappings = mappings.Where(m => m.IsSelected).ToList();
            
            // Get detailed column information including nullable, max length, etc.
            var columnInfo = await GetDetailedColumnInfo(connection, tableName);
            
            int currentRow = 0;
            foreach (var row in rows)
            {
                currentRow++;
                
                if (currentRow % 100 == 0)
                {
                    progress?.Report((currentRow, rows.Count, 
                        $"Validating row {currentRow} of {rows.Count}... ({result.ErrorCount} errors, {result.WarningCount} warnings found)"));
                }
                
                // Validate each mapped column
                for (int i = 0; i < selectedMappings.Count; i++)
                {
                    var mapping = selectedMappings[i];
                    var excelColumnIndex = mappings.FindIndex(m => m.ExcelColumn == mapping.ExcelColumn);
                    
                    if (excelColumnIndex < 0 || excelColumnIndex >= row.Count)
                        continue;
                    
                    var value = row[excelColumnIndex];
                    var colInfo = columnInfo.FirstOrDefault(c => 
                        c.COLUMN_NAME.Equals(mapping.SqlColumn, StringComparison.OrdinalIgnoreCase));
                    
                    if (colInfo.COLUMN_NAME == null)
                        continue;
                    
                    // Validate this cell
                    var validationErrors = ValidateCell(value, colInfo, currentRow, mapping.ExcelColumn);
                    
                    foreach (var error in validationErrors)
                    {
                        if (error.Severity == ValidationSeverity.Error)
                            result.Errors.Add(error);
                        else
                            result.Warnings.Add(error);
                    }
                }
            }
            
            progress?.Report((rows.Count, rows.Count, 
                $"Validation complete: {result.ErrorCount} errors, {result.WarningCount} warnings"));
            
            return result;
        }
        
        private async Task<List<ColumnInfo>> GetDetailedColumnInfo(SqlConnection connection, string tableName)
        {
            var sql = @"
                SELECT 
                    c.COLUMN_NAME,
                    c.DATA_TYPE,
                    c.IS_NULLABLE,
                    c.CHARACTER_MAXIMUM_LENGTH,
                    c.NUMERIC_PRECISION,
                    c.NUMERIC_SCALE
                FROM INFORMATION_SCHEMA.COLUMNS c
                WHERE c.TABLE_NAME = @TableName
                ORDER BY c.ORDINAL_POSITION";
            
            var columns = await connection.QueryAsync<ColumnInfo>(sql, new { TableName = tableName });
            return columns.ToList();
        }
        
        private List<ValidationError> ValidateCell(object? value, ColumnInfo columnInfo, int rowNumber, string excelColumnName)
        {
            var errors = new List<ValidationError>();
            
            // Check for NULL in non-nullable column
            if (value == null && columnInfo.IS_NULLABLE == "NO")
            {
                errors.Add(new ValidationError
                {
                    RowNumber = rowNumber,
                    ColumnName = excelColumnName,
                    ExcelValue = "NULL/Empty",
                    ExpectedType = columnInfo.DATA_TYPE,
                    ErrorMessage = $"Column '{columnInfo.COLUMN_NAME}' does not allow NULL values",
                    Severity = ValidationSeverity.Error
                });
                return errors;
            }
            
            if (value == null)
                return errors; // NULL is OK for nullable columns
            
            var stringValue = value.ToString() ?? "";
            
            // Validate based on SQL data type
            switch (columnInfo.DATA_TYPE.ToLower())
            {
                case "int":
                case "smallint":
                case "tinyint":
                case "bigint":
                    if (!int.TryParse(stringValue, out _) && !long.TryParse(stringValue, out _))
                    {
                        errors.Add(new ValidationError
                        {
                            RowNumber = rowNumber,
                            ColumnName = excelColumnName,
                            ExcelValue = stringValue,
                            ExpectedType = columnInfo.DATA_TYPE,
                            ErrorMessage = $"Value '{stringValue}' is not a valid integer",
                            Severity = ValidationSeverity.Error
                        });
                    }
                    break;
                
                case "decimal":
                case "numeric":
                case "money":
                case "smallmoney":
                case "float":
                case "real":
                    if (!decimal.TryParse(stringValue, out _))
                    {
                        errors.Add(new ValidationError
                        {
                            RowNumber = rowNumber,
                            ColumnName = excelColumnName,
                            ExcelValue = stringValue,
                            ExpectedType = columnInfo.DATA_TYPE,
                            ErrorMessage = $"Value '{stringValue}' is not a valid number",
                            Severity = ValidationSeverity.Error
                        });
                    }
                    break;
                
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    if (value is not DateTime && !DateTime.TryParse(stringValue, out _))
                    {
                        errors.Add(new ValidationError
                        {
                            RowNumber = rowNumber,
                            ColumnName = excelColumnName,
                            ExcelValue = stringValue,
                            ExpectedType = columnInfo.DATA_TYPE,
                            ErrorMessage = $"Value '{stringValue}' is not a valid date/time",
                            Severity = ValidationSeverity.Error
                        });
                    }
                    break;
                
                case "bit":
                    var lowerValue = stringValue.ToLower();
                    if (lowerValue != "0" && lowerValue != "1" && 
                        lowerValue != "true" && lowerValue != "false" &&
                        !bool.TryParse(stringValue, out _))
                    {
                        errors.Add(new ValidationError
                        {
                            RowNumber = rowNumber,
                            ColumnName = excelColumnName,
                            ExcelValue = stringValue,
                            ExpectedType = columnInfo.DATA_TYPE,
                            ErrorMessage = $"Value '{stringValue}' is not a valid boolean (expected: 0, 1, true, or false)",
                            Severity = ValidationSeverity.Error
                        });
                    }
                    break;
                
                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                    if (columnInfo.CHARACTER_MAXIMUM_LENGTH.HasValue && 
                        columnInfo.CHARACTER_MAXIMUM_LENGTH.Value > 0)
                    {
                        if (stringValue.Length > columnInfo.CHARACTER_MAXIMUM_LENGTH.Value)
                        {
                            errors.Add(new ValidationError
                            {
                                RowNumber = rowNumber,
                                ColumnName = excelColumnName,
                                ExcelValue = stringValue.Length > 50 ? stringValue.Substring(0, 50) + "..." : stringValue,
                                ExpectedType = $"{columnInfo.DATA_TYPE}({columnInfo.CHARACTER_MAXIMUM_LENGTH})",
                                ErrorMessage = $"Text length ({stringValue.Length}) exceeds maximum ({columnInfo.CHARACTER_MAXIMUM_LENGTH})",
                                Severity = ValidationSeverity.Error
                            });
                        }
                        else if (stringValue.Length > columnInfo.CHARACTER_MAXIMUM_LENGTH.Value * 0.9)
                        {
                            // Warning if close to limit
                            errors.Add(new ValidationError
                            {
                                RowNumber = rowNumber,
                                ColumnName = excelColumnName,
                                ExcelValue = stringValue.Length > 50 ? stringValue.Substring(0, 50) + "..." : stringValue,
                                ExpectedType = $"{columnInfo.DATA_TYPE}({columnInfo.CHARACTER_MAXIMUM_LENGTH})",
                                ErrorMessage = $"Text length ({stringValue.Length}) is close to maximum ({columnInfo.CHARACTER_MAXIMUM_LENGTH})",
                                Severity = ValidationSeverity.Warning
                            });
                        }
                    }
                    break;
            }
            
            return errors;
        }
        
        // Helper class for column information
        private class ColumnInfo
        {
            public string COLUMN_NAME { get; set; } = string.Empty;
            public string DATA_TYPE { get; set; } = string.Empty;
            public string IS_NULLABLE { get; set; } = "YES";
            public int? CHARACTER_MAXIMUM_LENGTH { get; set; }
            public byte? NUMERIC_PRECISION { get; set; }
            public int? NUMERIC_SCALE { get; set; }
        }
        
        private string TranslateErrorMessage(Exception ex, int rowNumber, List<object> rowData, List<ColumnMapping> selectedMappings)
        {
            var message = ex.Message;
            
            // Translate common SQL Server errors to user-friendly messages
            if (message.Contains("String or binary data would be truncated"))
            {
                return $"? Data Too Long: One or more text values exceed the column size limits.\n" +
                       $"?? Solution: Check your table column sizes or shorten the data in this row.";
            }
            
            if (message.Contains("Cannot insert the value NULL") || message.Contains("does not allow nulls"))
            {
                var columnMatch = System.Text.RegularExpressions.Regex.Match(message, @"column '(\w+)'");
                var columnName = columnMatch.Success ? columnMatch.Groups[1].Value : "unknown";
                
                return $"? Missing Required Data: Column '{columnName}' cannot be empty (NULL).\n" +
                       $"?? Row {rowNumber} has an empty cell that maps to a required database column.\n" +
                       $"?? Solutions:\n" +
                       $"   • Fill in the missing data in your Excel file for this row\n" +
                       $"   • Make the column nullable in your database: ALTER TABLE ... ALTER COLUMN [{columnName}] ... NULL\n" +
                       $"   • Use 'Skip Errors' mode to import other rows and fix this one later";
            }
            
            if (message.Contains("Violation of PRIMARY KEY constraint") || message.Contains("duplicate key"))
            {
                return $"? Duplicate Value: A row with this key already exists in the database.\n" +
                       $"?? Solution: This row has duplicate data in a unique column (like ID or Email). Remove duplicate or skip this row.";
            }
            
            if (message.Contains("Violation of FOREIGN KEY constraint"))
            {
                var constraintMatch = System.Text.RegularExpressions.Regex.Match(message, @"constraint ""(\w+)""");
                var constraintName = constraintMatch.Success ? constraintMatch.Groups[1].Value : "unknown";
                
                return $"? Invalid Reference: The data references a non-existent record in another table (Foreign Key: {constraintName}).\n" +
                       $"?? Solution: Ensure referenced data exists in the related table first, or remove the constraint temporarily.";
            }
            
            if (message.Contains("Conversion failed") || message.Contains("Error converting"))
            {
                // Check if it's a string-to-number conversion issue
                if (message.Contains("nvarchar") && (message.Contains("int") || message.Contains("smallint") || message.Contains("bigint")))
                {
                    return $"? Wrong Column Type: Trying to import text into a number column.\n" +
                           $"?? Row {rowNumber} contains text value that can't be converted to a number.\n" +
                           $"?? Common Cause: **Column Mapping Error** - You may have mapped a text column (like ProductCode) to a numeric column (like ProductID).\n" +
                           $"?? Solutions:\n" +
                           $"   • Go to Column Mapping tab and check which Excel column is mapped to this SQL column\n" +
                           $"   • Re-map to the correct NVARCHAR column\n" +
                           $"   • OR use 'Validate Data' to see exactly which columns have type mismatches\n" +
                           $"   • Row data: {GetRowDataPreview(rowData, selectedMappings)}";
                }
                
                return $"? Data Type Mismatch: The data format doesn't match the column type (e.g., text in a number column).\n" +
                       $"?? Solution: Check that dates are valid, numbers don't contain text, etc. in row {rowNumber}.";
            }
            
            if (message.Contains("Arithmetic overflow"))
            {
                return $"? Number Too Large: A numeric value exceeds the maximum size for its column.\n" +
                       $"?? Solution: Use a larger numeric type (e.g., BIGINT instead of INT) or reduce the value.";
            }
            
            if (message.Contains("The member p") && message.Contains("of type ClosedXML.Excel.XLCellValue cannot be used as a parameter value"))
            {
                return $"? Excel Data Format Error: Unable to read cell value from Excel.\n" +
                       $"?? Solution: This is likely a formula or special Excel format. Try:\n" +
                       $"   • Copy the Excel data and paste as values (Paste Special ? Values)\n" +
                       $"   • Save Excel as a new file\n" +
                       $"   • Check for merged cells or complex formulas in row {rowNumber}";
            }
            
            if (message.Contains("of type System.DBNull cannot be used as a parameter value"))
            {
                return $"? Empty Cell Issue: One or more cells are empty and the database column doesn't accept NULL values.\n" +
                       $"?? Row {rowNumber} has empty cells that need data.\n" +
                       $"?? Solutions:\n" +
                       $"   • Fill in all required cells in your Excel file\n" +
                       $"   • Make database columns nullable (if appropriate)\n" +
                       $"   • Check column mapping - you might be mapping empty Excel columns";
            }
            
            // Return a generic but helpful message for unknown errors
            return $"? Database Error: {message}\n" +
                   $"?? Row {rowNumber} data preview: {GetRowDataPreview(rowData, selectedMappings)}\n" +
                   $"?? Suggestion: Check if the data format matches the database column types.";
        }
        
        private string GetRowDataPreview(List<object> rowData, List<ColumnMapping> selectedMappings)
        {
            var preview = new List<string>();
            for (int i = 0; i < Math.Min(3, Math.Min(rowData.Count, selectedMappings.Count)); i++)
            {
                var value = rowData[i]?.ToString() ?? "NULL";
                if (value.Length > 20)
                    value = value.Substring(0, 20) + "...";
                preview.Add($"{selectedMappings[i].SqlColumn}='{value}'");
            }
            
            if (rowData.Count > 3)
                preview.Add("...");
            
            return string.Join(", ", preview);
        }
    }
}
