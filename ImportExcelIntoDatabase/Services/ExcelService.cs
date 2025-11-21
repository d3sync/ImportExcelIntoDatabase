using ClosedXML.Excel;
using ImportExcelIntoDatabase.Models;

namespace ImportExcelIntoDatabase.Services
{
    public class ExcelService
    {
        public ExcelData LoadExcelFile(string filePath)
        {
            var excelData = new ExcelData();
            
            using var workbook = new XLWorkbook(filePath);
            var worksheet = workbook.Worksheet(1);
            
            // Auto-detect if first row has headers
            var firstRow = worksheet.Row(1);
            excelData.HasHeaders = DetectHeaders(worksheet);
            excelData.StartRow = excelData.HasHeaders ? 2 : 1;
            
            // Determine the maximum column count
            var maxColumnCount = worksheet.LastColumnUsed()?.ColumnNumber() ?? 0;
            
            // Get headers
            if (excelData.HasHeaders)
            {
                var headerRow = worksheet.Row(1);
                // Read all columns, not just used ones
                for (int colNum = 1; colNum <= maxColumnCount; colNum++)
                {
                    var cell = headerRow.Cell(colNum);
                    var headerValue = GetCellValueAsString(cell);
                    excelData.Headers.Add(headerValue ?? $"Column{colNum}");
                }
            }
            else
            {
                // Generate default column names for all columns
                for (int i = 1; i <= maxColumnCount; i++)
                {
                    excelData.Headers.Add($"Column{i}");
                }
            }
            
            // Load preview data (first 10 rows)
            int rowsToLoad = Math.Min(10, worksheet.LastRowUsed()?.RowNumber() ?? 0);
            for (int i = excelData.StartRow; i <= rowsToLoad; i++)
            {
                var row = worksheet.Row(i);
                var rowData = new List<object?>();
                
                // Read ALL columns consistently, filling empty cells with null
                for (int colNum = 1; colNum <= maxColumnCount; colNum++)
                {
                    var cell = row.Cell(colNum);
                    rowData.Add(GetCellValueAsObject(cell));
                }
                
                // Only skip if ALL cells are null/empty
                if (rowData.Any(cell => cell != null))
                {
                    excelData.Rows.Add(rowData!);
                }
            }
            
            return excelData;
        }
        
        public async Task<List<List<object>>> GetAllDataRows(string filePath, int startRow)
        {
            return await Task.Run(() =>
            {
                var rows = new List<List<object>>();
                
                using var workbook = new XLWorkbook(filePath);
                var worksheet = workbook.Worksheet(1);
                
                var lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 0;
                
                // Determine the maximum column count from the worksheet
                var maxColumnCount = worksheet.LastColumnUsed()?.ColumnNumber() ?? 0;
                
                for (int i = startRow; i <= lastRow; i++)
                {
                    var row = worksheet.Row(i);
                    var rowData = new List<object?>();
                    
                    // Check if row has any data at all
                    if (!row.CellsUsed().Any())
                        continue;
                    
                    // Read ALL columns up to maxColumnCount, not just used cells
                    for (int colNum = 1; colNum <= maxColumnCount; colNum++)
                    {
                        var cell = row.Cell(colNum);
                        rowData.Add(GetCellValueAsObject(cell));
                    }
                    
                    rows.Add(rowData!);
                }
                
                return rows;
            });
        }
        
        private string? GetCellValueAsString(IXLCell cell)
        {
            try
            {
                if (cell.IsEmpty())
                    return null;
                
                // Get the actual value based on data type
                return cell.DataType switch
                {
                    XLDataType.Text => cell.GetString(),
                    XLDataType.Number => cell.GetDouble().ToString(),
                    XLDataType.Boolean => cell.GetBoolean().ToString(),
                    XLDataType.DateTime => cell.GetDateTime().ToString("yyyy-MM-dd HH:mm:ss"),
                    XLDataType.TimeSpan => cell.GetTimeSpan().ToString(),
                    _ => cell.GetString()
                };
            }
            catch
            {
                return cell.GetString();
            }
        }
        
        private object? GetCellValueAsObject(IXLCell cell)
        {
            try
            {
                if (cell.IsEmpty())
                    return null; // Return null instead of DBNull.Value
                
                // Return the actual typed value based on data type
                return cell.DataType switch
                {
                    XLDataType.Text => string.IsNullOrWhiteSpace(cell.GetString()) ? null : cell.GetString(),
                    XLDataType.Number => cell.GetDouble(),
                    XLDataType.Boolean => cell.GetBoolean(),
                    XLDataType.DateTime => cell.GetDateTime(),
                    XLDataType.TimeSpan => cell.GetTimeSpan().ToString(),
                    _ => string.IsNullOrWhiteSpace(cell.GetString()) ? null : cell.GetString()
                };
            }
            catch
            {
                // Fallback to string
                try
                {
                    var stringValue = cell.GetString();
                    return string.IsNullOrWhiteSpace(stringValue) ? null : stringValue;
                }
                catch
                {
                    return null; // Return null instead of DBNull.Value
                }
            }
        }
        
        private bool DetectHeaders(IXLWorksheet worksheet)
        {
            var firstRow = worksheet.Row(1);
            var secondRow = worksheet.Row(2);
            
            if (!secondRow.CellsUsed().Any())
                return false;
            
            // Check if first row contains mostly strings and second row has different types
            var firstRowCells = firstRow.CellsUsed().ToList();
            var secondRowCells = secondRow.CellsUsed().ToList();
            
            if (firstRowCells.Count == 0)
                return false;
            
            int stringCount = 0;
            foreach (var cell in firstRowCells)
            {
                if (cell.DataType == XLDataType.Text)
                {
                    stringCount++;
                }
            }
            
            // If more than 70% are strings, likely headers
            return (double)stringCount / firstRowCells.Count > 0.7;
        }
    }
}
