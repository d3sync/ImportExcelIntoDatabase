using ImportExcelIntoDatabase.Models;
using ImportExcelIntoDatabase.Services;
using System.Data;

namespace ImportExcelIntoDatabase
{
    public partial class Form1 : Form
    {
        private readonly ExcelService _excelService;
        private readonly DatabaseService _databaseService;
        private ExcelData? _excelData;
        private string _excelFilePath = string.Empty;
        private List<ColumnMapping> _columnMappings = new();

        public Form1()
        {
            InitializeComponent();
            _excelService = new ExcelService();
            _databaseService = new DatabaseService();
            
            // Load application icon
            LoadApplicationIcon();
        }
        
        private void LoadApplicationIcon()
        {
            try
            {
                var iconPath = Path.Combine(Application.StartupPath, "Resources", "app.ico");
                if (File.Exists(iconPath))
                {
                    this.Icon = new Icon(iconPath);
                }
            }
            catch
            {
                // If icon loading fails, just continue without it
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chkWindowsAuth.Checked = true;
            UpdateNavigationButtons();
            UpdateImportSummary();
        }

        private async void btnBrowseExcel_Click(object sender, EventArgs e)
        {
            using var openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files (*.xlsx;*.xls)|*.xlsx;*.xls|All Files (*.*)|*.*",
                Title = "Select Excel File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    _excelFilePath = openFileDialog.FileName;
                    txtExcelPath.Text = _excelFilePath;

                    // Load Excel file
                    _excelData = _excelService.LoadExcelFile(_excelFilePath);

                    // Update UI
                    chkHasHeaders.Checked = _excelData.HasHeaders;
                    numStartRow.Value = _excelData.StartRow;

                    // Display preview
                    DisplayExcelPreview();

                    MessageBox.Show("Excel file loaded successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading Excel file: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void DisplayExcelPreview()
        {
            if (_excelData == null) return;

            dataGridViewPreview.Columns.Clear();
            dataGridViewPreview.Rows.Clear();

            // Add columns
            foreach (var header in _excelData.Headers)
            {
                dataGridViewPreview.Columns.Add(header, header);
            }

            // Add rows
            foreach (var row in _excelData.Rows)
            {
                var rowIndex = dataGridViewPreview.Rows.Add();
                for (int i = 0; i < row.Count && i < _excelData.Headers.Count; i++)
                {
                    dataGridViewPreview.Rows[rowIndex].Cells[i].Value = row[i];
                }
            }
        }

        private void chkHasHeaders_CheckedChanged(object sender, EventArgs e)
        {
            if (_excelData != null)
            {
                _excelData.HasHeaders = chkHasHeaders.Checked;
                _excelData.StartRow = chkHasHeaders.Checked ? 2 : 1;
                numStartRow.Value = _excelData.StartRow;

                if (!string.IsNullOrEmpty(_excelFilePath))
                {
                    try
                    {
                        _excelData = _excelService.LoadExcelFile(_excelFilePath);
                        _excelData.HasHeaders = chkHasHeaders.Checked;
                        _excelData.StartRow = (int)numStartRow.Value;
                        DisplayExcelPreview();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error reloading Excel file: {ex.Message}", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void chkWindowsAuth_CheckedChanged(object sender, EventArgs e)
        {
            txtUsername.Enabled = !chkWindowsAuth.Checked;
            txtPassword.Enabled = !chkWindowsAuth.Checked;
        }

        private async void btnTestConnection_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtServer.Text))
            {
                MessageBox.Show("Please enter a server name.", "Validation", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!chkWindowsAuth.Checked && string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Please enter a username.", "Validation", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                btnTestConnection.Enabled = false;

                var connectionString = _databaseService.BuildConnectionString(
                    txtServer.Text, "master", txtUsername.Text, 
                    txtPassword.Text, chkWindowsAuth.Checked);

                var success = await _databaseService.TestConnection(connectionString);

                if (success)
                {
                    MessageBox.Show("Connection successful!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Load databases
                    await LoadDatabases();
                }
                else
                {
                    MessageBox.Show("Connection failed. Please check your settings.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnTestConnection.Enabled = true;
            }
        }

        private async Task LoadDatabases()
        {
            try
            {
                var databases = await _databaseService.GetDatabases(
                    txtServer.Text, txtUsername.Text, 
                    txtPassword.Text, chkWindowsAuth.Checked);

                cmbDatabases.Items.Clear();
                foreach (var db in databases)
                {
                    cmbDatabases.Items.Add(db);
                }

                if (cmbDatabases.Items.Count > 0)
                {
                    cmbDatabases.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading databases: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void cmbDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            await LoadTablesForSelectedDatabase();
        }

        private async Task LoadTablesForSelectedDatabase()
        {
            if (cmbDatabases.SelectedItem == null) return;

            try
            {
                Cursor = Cursors.WaitCursor;

                var tables = await _databaseService.GetTables(
                    txtServer.Text, cmbDatabases.SelectedItem.ToString()!,
                    txtUsername.Text, txtPassword.Text, chkWindowsAuth.Checked);

                cmbTables.Items.Clear();
                foreach (var table in tables)
                {
                    cmbTables.Items.Add(table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tables: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex > 0)
            {
                tabControl.SelectedIndex--;
                UpdateNavigationButtons();
            }
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            // Validation before moving to next step
            if (tabControl.SelectedIndex == 0) // Excel tab
            {
                if (string.IsNullOrEmpty(_excelFilePath) || _excelData == null)
                {
                    MessageBox.Show("Please select an Excel file first.", "Validation", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else if (tabControl.SelectedIndex == 1) // Database tab
            {
                if (string.IsNullOrWhiteSpace(txtServer.Text))
                {
                    MessageBox.Show("Please configure database connection.", "Validation", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if table is selected or entered
                if (string.IsNullOrWhiteSpace(cmbTables.Text))
                {
                    MessageBox.Show("Please select or enter a target table name.", "Validation", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Load column mappings
                await LoadColumnMappings();
            }
            else if (tabControl.SelectedIndex == 2) // Mapping tab
            {
                if (!_columnMappings.Any(m => m.IsSelected))
                {
                    MessageBox.Show("Please select at least one column to import.", "Validation", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UpdateImportSummary();
            }

            if (tabControl.SelectedIndex < tabControl.TabCount - 1)
            {
                tabControl.SelectedIndex++;
                UpdateNavigationButtons();
            }
        }

        private void UpdateNavigationButtons()
        {
            btnPrevious.Enabled = tabControl.SelectedIndex > 0;
            btnNext.Enabled = tabControl.SelectedIndex < tabControl.TabCount - 1;
        }

        private async Task LoadColumnMappings()
        {
            if (_excelData == null || string.IsNullOrWhiteSpace(cmbTables.Text)) return;

            try
            {
                Cursor = Cursors.WaitCursor;

                // Get SQL columns - use the text from combo box (could be typed or selected)
                var tableName = cmbTables.Text;
                var sqlColumns = await _databaseService.GetTableColumns(
                    txtServer.Text, cmbDatabases.SelectedItem.ToString()!,
                    tableName,
                    txtUsername.Text, txtPassword.Text, chkWindowsAuth.Checked);

                // Create mappings
                _columnMappings.Clear();
                dataGridViewMapping.Rows.Clear();

                // Update SQL column dropdown
                colSqlColumn.Items.Clear();
                foreach (var col in sqlColumns)
                {
                    colSqlColumn.Items.Add(col.ColumnName);
                }

                foreach (var excelColumn in _excelData.Headers)
                {
                    var mapping = new ColumnMapping
                    {
                        ExcelColumn = excelColumn,
                        IsSelected = true
                    };

                    // Try to auto-map by name
                    var matchingColumn = sqlColumns.FirstOrDefault(c => 
                        c.ColumnName.Equals(excelColumn, StringComparison.OrdinalIgnoreCase));

                    if (matchingColumn.ColumnName != null)
                    {
                        mapping.SqlColumn = matchingColumn.ColumnName;
                        mapping.DataType = matchingColumn.DataType;
                    }

                    _columnMappings.Add(mapping);

                    var rowIndex = dataGridViewMapping.Rows.Add();
                    dataGridViewMapping.Rows[rowIndex].Cells[0].Value = mapping.IsSelected;
                    dataGridViewMapping.Rows[rowIndex].Cells[1].Value = mapping.ExcelColumn;
                    dataGridViewMapping.Rows[rowIndex].Cells[2].Value = mapping.SqlColumn;
                    dataGridViewMapping.Rows[rowIndex].Cells[3].Value = mapping.DataType;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading column mappings: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewMapping.Rows)
            {
                row.Cells[0].Value = true;
            }
            UpdateMappingsFromGrid();
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewMapping.Rows)
            {
                row.Cells[0].Value = false;
            }
            UpdateMappingsFromGrid();
        }

        private void UpdateMappingsFromGrid()
        {
            for (int i = 0; i < dataGridViewMapping.Rows.Count && i < _columnMappings.Count; i++)
            {
                var row = dataGridViewMapping.Rows[i];
                _columnMappings[i].IsSelected = row.Cells[0].Value is bool b && b;
                _columnMappings[i].SqlColumn = row.Cells[2].Value?.ToString() ?? string.Empty;
            }
        }

        private void UpdateImportSummary()
        {
            UpdateMappingsFromGrid();

            var summary = new System.Text.StringBuilder();
            summary.AppendLine("=== Import Summary ===");
            summary.AppendLine();
            summary.AppendLine($"Excel File: {Path.GetFileName(_excelFilePath)}");
            summary.AppendLine($"Server: {txtServer.Text}");
            summary.AppendLine($"Database: {cmbDatabases.SelectedItem}");
            summary.AppendLine($"Table: {cmbTables.Text}");
            summary.AppendLine();
            summary.AppendLine("Columns to Import:");
            
            var selectedMappings = _columnMappings.Where(m => m.IsSelected && !string.IsNullOrEmpty(m.SqlColumn)).ToList();
            foreach (var mapping in selectedMappings)
            {
                summary.AppendLine($"  • {mapping.ExcelColumn} → {mapping.SqlColumn} ({mapping.DataType})");
            }

            summary.AppendLine();
            summary.AppendLine($"Total Columns: {selectedMappings.Count}");

            txtSummary.Text = summary.ToString();
        }

        private async void btnStartImport_Click(object sender, EventArgs e)
        {
            if (_excelData == null || string.IsNullOrEmpty(_excelFilePath))
            {
                MessageBox.Show("Please select an Excel file first.", "Validation", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UpdateMappingsFromGrid();

            var selectedMappings = _columnMappings.Where(m => m.IsSelected && !string.IsNullOrEmpty(m.SqlColumn)).ToList();
            if (selectedMappings.Count == 0)
            {
                MessageBox.Show("Please select at least one column mapping.", "Validation", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Determine error handling strategy
            ErrorHandlingStrategy strategy;
            if (rbUseTransaction.Checked)
            {
                strategy = ErrorHandlingStrategy.UseTransaction;
            }
            else if (rbSkipErrors.Checked)
            {
                strategy = ErrorHandlingStrategy.SkipErrorsAndContinue;
            }
            else
            {
                strategy = ErrorHandlingStrategy.StopOnFirstError;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to start the import?\n\n" +
                $"Strategy: {GetStrategyDescription(strategy)}\n" +
                $"This will insert data into the database.",
                "Confirm Import",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                btnStartImport.Enabled = false;
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
                Cursor = Cursors.WaitCursor;

                lblStatus.Text = "Loading data from Excel...";
                lblProgress.Text = "Progress: 0 / 0";
                progressBar.Value = 0;
                Application.DoEvents();

                // Load all data from Excel
                var allRows = await _excelService.GetAllDataRows(_excelFilePath, _excelData.StartRow);

                lblStatus.Text = $"Importing {allRows.Count} rows using {GetStrategyDescription(strategy)}...";
                lblProgress.Text = $"Progress: 0 / {allRows.Count}";
                progressBar.Maximum = allRows.Count;
                Application.DoEvents();

                var progress = new Progress<(int Current, int Total, string Message)>(p =>
                {
                    progressBar.Value = Math.Min(p.Current, progressBar.Maximum);
                    lblProgress.Text = $"Progress: {p.Current} / {p.Total}";
                    lblStatus.Text = p.Message;
                    Application.DoEvents();
                });

                var importResult = await _databaseService.ImportDataWithErrorHandling(
                    txtServer.Text,
                    cmbDatabases.SelectedItem.ToString()!,
                    cmbTables.Text,
                    txtUsername.Text,
                    txtPassword.Text,
                    chkWindowsAuth.Checked,
                    _columnMappings,
                    allRows,
                    strategy,
                    progress);

                // Show results
                if (importResult.IsSuccessful)
                {
                    lblStatus.Text = $"✓ Import completed successfully! {importResult.SuccessfulRows} rows imported.";
                    lblProgress.Text = $"Progress: {importResult.TotalRows} / {importResult.TotalRows} (100%)";
                    lblStatus.ForeColor = Color.Green;
                    progressBar.Value = progressBar.Maximum;

                    MessageBox.Show(
                        $"Import completed successfully!\n\n" +
                        $"Total Rows: {importResult.TotalRows}\n" +
                        $"Successful: {importResult.SuccessfulRows}\n" +
                        $"Duration: {importResult.Duration.TotalSeconds:F2} seconds",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    lblStatus.Text = $"⚠ Import completed with {importResult.FailedRows} error(s). {importResult.SuccessfulRows} rows imported.";
                    lblProgress.Text = $"Progress: {importResult.TotalRows} / {importResult.TotalRows} ({importResult.SuccessfulRows} successful, {importResult.FailedRows} failed)";
                    lblStatus.ForeColor = Color.DarkOrange;

                    // Show detailed results dialog
                    using var resultDialog = new ImportResultDialog(importResult);
                    resultDialog.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "❌ Import failed!";
                lblStatus.ForeColor = Color.Red;
                
                MessageBox.Show($"Error during import:\n\n{ex.Message}\n\n" +
                    $"Check the error details and try again.", 
                    "Import Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnStartImport.Enabled = true;
                btnNext.Enabled = true;
                btnPrevious.Enabled = true;
                Cursor = Cursors.Default;
            }
        }
        
        private async void btnCreateNewTable_Click(object sender, EventArgs e)
        {
            if (_excelData == null)
            {
                MessageBox.Show("Please load an Excel file first.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (cmbDatabases.SelectedItem == null)
            {
                MessageBox.Show("Please select a database first.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            try
            {
                Cursor = Cursors.WaitCursor;
                
                // Infer data types from Excel data
                var suggestedDataTypes = new List<string>();
                foreach (var header in _excelData.Headers)
                {
                    var columnIndex = _excelData.Headers.IndexOf(header);
                    var columnData = new List<object>();
                    
                    foreach (var row in _excelData.Rows)
                    {
                        if (columnIndex < row.Count)
                        {
                            columnData.Add(row[columnIndex]);
                        }
                    }
                    
                    var dataType = _databaseService.InferSqlDataType(columnData);
                    suggestedDataTypes.Add(dataType);
                }
                
                // Show create table dialog
                using var dialog = new CreateTableDialog(_excelData.Headers, suggestedDataTypes);
                
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // Create the table
                    var columnNames = dialog.Columns.Select(c => c.ColumnName).ToList();
                    var dataTypes = dialog.Columns.Select(c => c.DataType).ToList();
                    
                    await _databaseService.CreateTable(
                        txtServer.Text,
                        cmbDatabases.SelectedItem.ToString()!,
                        dialog.TableName,
                        txtUsername.Text,
                        txtPassword.Text,
                        chkWindowsAuth.Checked,
                        columnNames,
                        dataTypes);
                    
                    MessageBox.Show($"Table '{dialog.TableName}' created successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Refresh tables list
                    await LoadTablesForSelectedDatabase();
                    
                    // Select the newly created table
                    cmbTables.Text = dialog.TableName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating table: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        
        private string GetStrategyDescription(ErrorHandlingStrategy strategy)
        {
            return strategy switch
            {
                ErrorHandlingStrategy.UseTransaction => "Transaction (All or Nothing)",
                ErrorHandlingStrategy.SkipErrorsAndContinue => "Skip Errors and Continue",
                ErrorHandlingStrategy.StopOnFirstError => "Stop on First Error",
                _ => "Unknown"
            };
        }
        
        private async void btnValidateData_Click(object sender, EventArgs e)
        {
            if (_excelData == null || string.IsNullOrEmpty(_excelFilePath))
            {
                MessageBox.Show("Please select an Excel file first.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UpdateMappingsFromGrid();

            var selectedMappings = _columnMappings.Where(m => m.IsSelected && !string.IsNullOrEmpty(m.SqlColumn)).ToList();
            if (selectedMappings.Count == 0)
            {
                MessageBox.Show("Please select at least one column mapping.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnValidateData.Enabled = false;
                btnStartImport.Enabled = false;
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
                Cursor = Cursors.WaitCursor;

                lblStatus.Text = "Loading data from Excel for validation...";
                lblProgress.Text = "Progress: 0 / 0";
                progressBar.Value = 0;
                Application.DoEvents();

                // Load all data from Excel
                var allRows = await _excelService.GetAllDataRows(_excelFilePath, _excelData.StartRow);

                lblStatus.Text = $"Validating {allRows.Count} rows...";
                lblProgress.Text = $"Progress: 0 / {allRows.Count}";
                progressBar.Maximum = allRows.Count;
                Application.DoEvents();

                var progress = new Progress<(int Current, int Total, string Message)>(p =>
                {
                    progressBar.Value = Math.Min(p.Current, progressBar.Maximum);
                    lblProgress.Text = $"Progress: {p.Current} / {p.Total}";
                    lblStatus.Text = p.Message;
                    Application.DoEvents();
                });

                var validationResult = await _databaseService.ValidateDataBeforeImport(
                    txtServer.Text,
                    cmbDatabases.SelectedItem.ToString()!,
                    cmbTables.Text,
                    txtUsername.Text,
                    txtPassword.Text,
                    chkWindowsAuth.Checked,
                    _columnMappings,
                    allRows,
                    progress);

                // Show validation results
                if (validationResult.IsValid && validationResult.WarningCount == 0)
                {
                    lblStatus.Text = $"✓ Validation passed! All {validationResult.TotalRowsValidated} rows are valid.";
                    lblStatus.ForeColor = Color.Green;
                    progressBar.Value = progressBar.Maximum;

                    MessageBox.Show(
                        $"✓ Validation Successful!\n\n" +
                        $"All {validationResult.TotalRowsValidated} rows passed validation.\n" +
                        $"You can safely proceed with the import.",
                        "Validation Complete",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    lblStatus.Text = $"⚠ Validation found {validationResult.ErrorCount} error(s) and {validationResult.WarningCount} warning(s).";
                    lblStatus.ForeColor = validationResult.IsValid ? Color.DarkOrange : Color.Red;

                    // Show validation results dialog
                    using var resultDialog = new ValidationResultDialog(validationResult);
                    var dialogResult = resultDialog.ShowDialog();

                    // If user chose to continue anyway despite errors
                    if (dialogResult == DialogResult.OK && resultDialog.ContinueWithImport)
                    {
                        // Suggest using Skip Errors mode
                        var suggestion = MessageBox.Show(
                            "It's highly recommended to use 'Skip Errors and Continue' mode.\n\n" +
                            "Would you like to switch to this mode now?",
                            "Recommendation",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (suggestion == DialogResult.Yes)
                        {
                            rbSkipErrors.Checked = true;
                        }

                        lblStatus.Text = "Ready to import (validation completed with errors).";
                        lblStatus.ForeColor = Color.DarkOrange;
                    }
                    else
                    {
                        lblStatus.Text = "Validation completed. Please fix errors before importing.";
                        lblStatus.ForeColor = Color.DarkOrange;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "❌ Validation failed!";
                lblStatus.ForeColor = Color.Red;

                MessageBox.Show($"Error during validation:\n\n{ex.Message}",
                    "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnValidateData.Enabled = true;
                btnStartImport.Enabled = true;
                btnNext.Enabled = true;
                btnPrevious.Enabled = true;
                Cursor = Cursors.Default;
            }
        }
    }
}
