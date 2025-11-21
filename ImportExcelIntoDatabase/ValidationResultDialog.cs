using ImportExcelIntoDatabase.Models;
using System.Text;

namespace ImportExcelIntoDatabase
{
    public partial class ValidationResultDialog : Form
    {
        private ValidationResult _result;
        private TextBox txtSummary;
        private DataGridView dataGridViewErrors;
        private DataGridView dataGridViewWarnings;
        private Button btnExport;
        private Button btnContinueAnyway;
        private Button btnCancel;
        private Label lblSummary;
        private TabControl tabControl;
        private TabPage tabErrors;
        private TabPage tabWarnings;
        private DataGridViewTextBoxColumn colErrorRow;
        private DataGridViewTextBoxColumn colErrorColumn;
        private DataGridViewTextBoxColumn colErrorValue;
        private DataGridViewTextBoxColumn colErrorExpected;
        private DataGridViewTextBoxColumn colErrorMessage;
        private DataGridViewTextBoxColumn colWarnRow;
        private DataGridViewTextBoxColumn colWarnColumn;
        private DataGridViewTextBoxColumn colWarnValue;
        private DataGridViewTextBoxColumn colWarnExpected;
        private DataGridViewTextBoxColumn colWarnMessage;
        
        public bool ContinueWithImport { get; private set; }
        
        public ValidationResultDialog(ValidationResult result)
        {
            _result = result;
            InitializeComponent();
            DisplayResult();
        }
        
        private void InitializeComponent()
        {
            this.tabControl = new TabControl();
            this.tabErrors = new TabPage();
            this.tabWarnings = new TabPage();
            this.lblSummary = new Label();
            this.dataGridViewErrors = new DataGridView();
            this.colErrorRow = new DataGridViewTextBoxColumn();
            this.colErrorColumn = new DataGridViewTextBoxColumn();
            this.colErrorValue = new DataGridViewTextBoxColumn();
            this.colErrorExpected = new DataGridViewTextBoxColumn();
            this.colErrorMessage = new DataGridViewTextBoxColumn();
            this.dataGridViewWarnings = new DataGridView();
            this.colWarnRow = new DataGridViewTextBoxColumn();
            this.colWarnColumn = new DataGridViewTextBoxColumn();
            this.colWarnValue = new DataGridViewTextBoxColumn();
            this.colWarnExpected = new DataGridViewTextBoxColumn();
            this.colWarnMessage = new DataGridViewTextBoxColumn();
            this.btnExport = new Button();
            this.btnContinueAnyway = new Button();
            this.btnCancel = new Button();
            
            this.tabControl.SuspendLayout();
            this.tabErrors.SuspendLayout();
            this.tabWarnings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewErrors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWarnings)).BeginInit();
            this.SuspendLayout();
            
            // tabControl
            this.tabControl.Controls.Add(this.tabErrors);
            this.tabControl.Controls.Add(this.tabWarnings);
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Font = new Font("Segoe UI", 10F);
            this.tabControl.Location = new Point(10, 60);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(980, 450);
            this.tabControl.TabIndex = 0;
            
            // tabErrors
            this.tabErrors.Controls.Add(this.dataGridViewErrors);
            this.tabErrors.Location = new Point(4, 28);
            this.tabErrors.Name = "tabErrors";
            this.tabErrors.Padding = new Padding(10);
            this.tabErrors.Size = new Size(972, 418);
            this.tabErrors.TabIndex = 0;
            this.tabErrors.Text = $"? Errors ({_result?.ErrorCount ?? 0})";
            this.tabErrors.UseVisualStyleBackColor = true;
            
            // tabWarnings
            this.tabWarnings.Controls.Add(this.dataGridViewWarnings);
            this.tabWarnings.Location = new Point(4, 28);
            this.tabWarnings.Name = "tabWarnings";
            this.tabWarnings.Padding = new Padding(10);
            this.tabWarnings.Size = new Size(972, 418);
            this.tabWarnings.TabIndex = 1;
            this.tabWarnings.Text = $"? Warnings ({_result?.WarningCount ?? 0})";
            this.tabWarnings.UseVisualStyleBackColor = true;
            
            // Error columns
            this.colErrorRow.HeaderText = "Row #";
            this.colErrorRow.Name = "colErrorRow";
            this.colErrorRow.ReadOnly = true;
            this.colErrorRow.Width = 70;
            
            this.colErrorColumn.HeaderText = "Column";
            this.colErrorColumn.Name = "colErrorColumn";
            this.colErrorColumn.ReadOnly = true;
            this.colErrorColumn.Width = 120;
            
            this.colErrorValue.HeaderText = "Value";
            this.colErrorValue.Name = "colErrorValue";
            this.colErrorValue.ReadOnly = true;
            this.colErrorValue.Width = 150;
            
            this.colErrorExpected.HeaderText = "Expected Type";
            this.colErrorExpected.Name = "colErrorExpected";
            this.colErrorExpected.ReadOnly = true;
            this.colErrorExpected.Width = 120;
            
            this.colErrorMessage.HeaderText = "Error Message";
            this.colErrorMessage.Name = "colErrorMessage";
            this.colErrorMessage.ReadOnly = true;
            this.colErrorMessage.Width = 400;
            
            // dataGridViewErrors
            this.dataGridViewErrors.AllowUserToAddRows = false;
            this.dataGridViewErrors.AllowUserToDeleteRows = false;
            this.dataGridViewErrors.BackgroundColor = SystemColors.Window;
            this.dataGridViewErrors.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewErrors.Columns.AddRange(new DataGridViewColumn[] {
                this.colErrorRow,
                this.colErrorColumn,
                this.colErrorValue,
                this.colErrorExpected,
                this.colErrorMessage
            });
            this.dataGridViewErrors.Dock = DockStyle.Fill;
            this.dataGridViewErrors.Location = new Point(10, 10);
            this.dataGridViewErrors.Name = "dataGridViewErrors";
            this.dataGridViewErrors.ReadOnly = true;
            this.dataGridViewErrors.RowHeadersWidth = 51;
            this.dataGridViewErrors.Size = new Size(952, 398);
            this.dataGridViewErrors.TabIndex = 0;
            
            // Warning columns (same as error columns)
            this.colWarnRow.HeaderText = "Row #";
            this.colWarnRow.Name = "colWarnRow";
            this.colWarnRow.ReadOnly = true;
            this.colWarnRow.Width = 70;
            
            this.colWarnColumn.HeaderText = "Column";
            this.colWarnColumn.Name = "colWarnColumn";
            this.colWarnColumn.ReadOnly = true;
            this.colWarnColumn.Width = 120;
            
            this.colWarnValue.HeaderText = "Value";
            this.colWarnValue.Name = "colWarnValue";
            this.colWarnValue.ReadOnly = true;
            this.colWarnValue.Width = 150;
            
            this.colWarnExpected.HeaderText = "Expected Type";
            this.colWarnExpected.Name = "colWarnExpected";
            this.colWarnExpected.ReadOnly = true;
            this.colWarnExpected.Width = 120;
            
            this.colWarnMessage.HeaderText = "Warning Message";
            this.colWarnMessage.Name = "colWarnMessage";
            this.colWarnMessage.ReadOnly = true;
            this.colWarnMessage.Width = 400;
            
            // dataGridViewWarnings
            this.dataGridViewWarnings.AllowUserToAddRows = false;
            this.dataGridViewWarnings.AllowUserToDeleteRows = false;
            this.dataGridViewWarnings.BackgroundColor = SystemColors.Window;
            this.dataGridViewWarnings.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewWarnings.Columns.AddRange(new DataGridViewColumn[] {
                this.colWarnRow,
                this.colWarnColumn,
                this.colWarnValue,
                this.colWarnExpected,
                this.colWarnMessage
            });
            this.dataGridViewWarnings.Dock = DockStyle.Fill;
            this.dataGridViewWarnings.Location = new Point(10, 10);
            this.dataGridViewWarnings.Name = "dataGridViewWarnings";
            this.dataGridViewWarnings.ReadOnly = true;
            this.dataGridViewWarnings.RowHeadersWidth = 51;
            this.dataGridViewWarnings.Size = new Size(952, 398);
            this.dataGridViewWarnings.TabIndex = 0;
            
            // lblSummary
            this.lblSummary.AutoSize = true;
            this.lblSummary.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblSummary.Location = new Point(10, 15);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new Size(200, 21);
            this.lblSummary.TabIndex = 1;
            this.lblSummary.Text = "Validation Results";
            
            // btnExport
            this.btnExport.Location = new Point(580, 520);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new Size(120, 35);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "Export Report";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new EventHandler(this.btnExport_Click);
            
            // btnContinueAnyway
            this.btnContinueAnyway.BackColor = Color.FromArgb(255, 140, 0); // DarkOrange
            this.btnContinueAnyway.FlatStyle = FlatStyle.Flat;
            this.btnContinueAnyway.ForeColor = Color.White;
            this.btnContinueAnyway.Location = new Point(710, 520);
            this.btnContinueAnyway.Name = "btnContinueAnyway";
            this.btnContinueAnyway.Size = new Size(140, 35);
            this.btnContinueAnyway.TabIndex = 3;
            this.btnContinueAnyway.Text = "Import Anyway";
            this.btnContinueAnyway.UseVisualStyleBackColor = false;
            this.btnContinueAnyway.Visible = _result?.ErrorCount > 0;
            this.btnContinueAnyway.Click += new EventHandler(this.btnContinueAnyway_Click);
            
            // btnCancel
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(860, 520);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(130, 35);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = _result?.IsValid == true ? "OK" : "Cancel Import";
            this.btnCancel.UseVisualStyleBackColor = true;
            
            // ValidationResultDialog
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1000, 565);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnContinueAnyway);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.lblSummary);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ValidationResultDialog";
            this.Padding = new Padding(10, 60, 10, 10);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Data Validation Results";
            
            this.tabControl.ResumeLayout(false);
            this.tabErrors.ResumeLayout(false);
            this.tabWarnings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewErrors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWarnings)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        
        private void DisplayResult()
        {
            // Update summary label
            if (_result.IsValid)
            {
                lblSummary.Text = $"? Validation Passed - {_result.TotalRowsValidated} rows validated";
                lblSummary.ForeColor = Color.Green;
                if (_result.WarningCount > 0)
                {
                    lblSummary.Text += $" ({_result.WarningCount} warnings)";
                    lblSummary.ForeColor = Color.DarkOrange;
                }
            }
            else
            {
                lblSummary.Text = $"? Validation Failed - {_result.ErrorCount} errors found";
                lblSummary.ForeColor = Color.Red;
            }
            
            // Populate errors grid
            foreach (var error in _result.Errors)
            {
                var rowIndex = dataGridViewErrors.Rows.Add();
                dataGridViewErrors.Rows[rowIndex].Cells[0].Value = error.RowNumber;
                dataGridViewErrors.Rows[rowIndex].Cells[1].Value = error.ColumnName;
                dataGridViewErrors.Rows[rowIndex].Cells[2].Value = error.ExcelValue;
                dataGridViewErrors.Rows[rowIndex].Cells[3].Value = error.ExpectedType;
                dataGridViewErrors.Rows[rowIndex].Cells[4].Value = error.ErrorMessage;
            }
            
            // Populate warnings grid
            foreach (var warning in _result.Warnings)
            {
                var rowIndex = dataGridViewWarnings.Rows.Add();
                dataGridViewWarnings.Rows[rowIndex].Cells[0].Value = warning.RowNumber;
                dataGridViewWarnings.Rows[rowIndex].Cells[1].Value = warning.ColumnName;
                dataGridViewWarnings.Rows[rowIndex].Cells[2].Value = warning.ExcelValue;
                dataGridViewWarnings.Rows[rowIndex].Cells[3].Value = warning.ExpectedType;
                dataGridViewWarnings.Rows[rowIndex].Cells[4].Value = warning.ErrorMessage;
            }
            
            // Select appropriate tab
            if (_result.ErrorCount > 0)
            {
                tabControl.SelectedIndex = 0; // Errors tab
            }
            else if (_result.WarningCount > 0)
            {
                tabControl.SelectedIndex = 1; // Warnings tab
            }
        }
        
        private void btnExport_Click(object? sender, EventArgs e)
        {
            using var saveDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                Title = "Export Validation Report",
                FileName = $"ValidationReport_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };
            
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var csv = new StringBuilder();
                    csv.AppendLine("Severity,Row #,Column,Value,Expected Type,Message");
                    
                    foreach (var error in _result.Errors)
                    {
                        csv.AppendLine($"ERROR,{error.RowNumber},\"{error.ColumnName}\",\"{error.ExcelValue}\",\"{error.ExpectedType}\",\"{error.ErrorMessage}\"");
                    }
                    
                    foreach (var warning in _result.Warnings)
                    {
                        csv.AppendLine($"WARNING,{warning.RowNumber},\"{warning.ColumnName}\",\"{warning.ExcelValue}\",\"{warning.ExpectedType}\",\"{warning.ErrorMessage}\"");
                    }
                    
                    File.WriteAllText(saveDialog.FileName, csv.ToString());
                    
                    MessageBox.Show($"Validation report exported successfully to:\n{saveDialog.FileName}", 
                        "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting report: {ex.Message}", 
                        "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void btnContinueAnyway_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                $"?? WARNING: There are {_result.ErrorCount} validation errors!\n\n" +
                "These rows WILL FAIL during import.\n\n" +
                "Do you want to continue anyway?\n" +
                "(Recommended: Use 'Skip Errors' mode if you proceed)",
                "Confirm Import with Errors",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);
            
            if (result == DialogResult.Yes)
            {
                ContinueWithImport = true;
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
