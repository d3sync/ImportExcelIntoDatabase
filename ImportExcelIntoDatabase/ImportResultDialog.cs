using ImportExcelIntoDatabase.Models;
using System.Text;

namespace ImportExcelIntoDatabase
{
    public partial class ImportResultDialog : Form
    {
        private ImportResult _result;
        private TextBox txtSummary;
        private DataGridView dataGridViewErrors;
        private Button btnExportErrors;
        private Button btnClose;
        private Label lblSummary;
        private TabControl tabControl;
        private TabPage tabSummary;
        private TabPage tabErrors;
        private DataGridViewTextBoxColumn colRowNumber;
        private DataGridViewTextBoxColumn colErrorMessage;
        private DataGridViewTextBoxColumn colRowData;
        
        public ImportResultDialog(ImportResult result)
        {
            _result = result;
            InitializeComponent();
            DisplayResult();
        }
        
        private void InitializeComponent()
        {
            this.tabControl = new TabControl();
            this.tabSummary = new TabPage();
            this.tabErrors = new TabPage();
            this.lblSummary = new Label();
            this.txtSummary = new TextBox();
            this.dataGridViewErrors = new DataGridView();
            this.colRowNumber = new DataGridViewTextBoxColumn();
            this.colErrorMessage = new DataGridViewTextBoxColumn();
            this.colRowData = new DataGridViewTextBoxColumn();
            this.btnExportErrors = new Button();
            this.btnClose = new Button();
            
            this.tabControl.SuspendLayout();
            this.tabSummary.SuspendLayout();
            this.tabErrors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewErrors)).BeginInit();
            this.SuspendLayout();
            
            // tabControl
            this.tabControl.Controls.Add(this.tabSummary);
            this.tabControl.Controls.Add(this.tabErrors);
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Font = new Font("Segoe UI", 10F);
            this.tabControl.Location = new Point(10, 60);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(880, 450);
            this.tabControl.TabIndex = 0;
            
            // tabSummary
            this.tabSummary.Controls.Add(this.txtSummary);
            this.tabSummary.Location = new Point(4, 28);
            this.tabSummary.Name = "tabSummary";
            this.tabSummary.Padding = new Padding(10);
            this.tabSummary.Size = new Size(872, 418);
            this.tabSummary.TabIndex = 0;
            this.tabSummary.Text = "Summary";
            this.tabSummary.UseVisualStyleBackColor = true;
            
            // txtSummary
            this.txtSummary.BackColor = SystemColors.Window;
            this.txtSummary.Dock = DockStyle.Fill;
            this.txtSummary.Font = new Font("Consolas", 9.75F);
            this.txtSummary.Location = new Point(10, 10);
            this.txtSummary.Multiline = true;
            this.txtSummary.Name = "txtSummary";
            this.txtSummary.ReadOnly = true;
            this.txtSummary.ScrollBars = ScrollBars.Vertical;
            this.txtSummary.Size = new Size(852, 398);
            this.txtSummary.TabIndex = 0;
            
            // tabErrors
            this.tabErrors.Controls.Add(this.dataGridViewErrors);
            this.tabErrors.Location = new Point(4, 28);
            this.tabErrors.Name = "tabErrors";
            this.tabErrors.Padding = new Padding(10);
            this.tabErrors.Size = new Size(872, 418);
            this.tabErrors.TabIndex = 1;
            this.tabErrors.Text = $"Errors ({_result?.FailedRows ?? 0})";
            this.tabErrors.UseVisualStyleBackColor = true;
            
            // colRowNumber
            this.colRowNumber.HeaderText = "Row #";
            this.colRowNumber.Name = "colRowNumber";
            this.colRowNumber.ReadOnly = true;
            this.colRowNumber.Width = 80;
            
            // colErrorMessage
            this.colErrorMessage.HeaderText = "Error Message";
            this.colErrorMessage.Name = "colErrorMessage";
            this.colErrorMessage.ReadOnly = true;
            this.colErrorMessage.Width = 400;
            
            // colRowData
            this.colRowData.HeaderText = "Row Data (Preview)";
            this.colRowData.Name = "colRowData";
            this.colRowData.ReadOnly = true;
            this.colRowData.Width = 350;
            
            // dataGridViewErrors
            this.dataGridViewErrors.AllowUserToAddRows = false;
            this.dataGridViewErrors.AllowUserToDeleteRows = false;
            this.dataGridViewErrors.BackgroundColor = SystemColors.Window;
            this.dataGridViewErrors.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewErrors.Columns.AddRange(new DataGridViewColumn[] {
                this.colRowNumber,
                this.colErrorMessage,
                this.colRowData
            });
            this.dataGridViewErrors.Dock = DockStyle.Fill;
            this.dataGridViewErrors.Location = new Point(10, 10);
            this.dataGridViewErrors.Name = "dataGridViewErrors";
            this.dataGridViewErrors.ReadOnly = true;
            this.dataGridViewErrors.RowHeadersWidth = 51;
            this.dataGridViewErrors.Size = new Size(852, 398);
            this.dataGridViewErrors.TabIndex = 0;
            
            // lblSummary
            this.lblSummary.AutoSize = true;
            this.lblSummary.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblSummary.Location = new Point(10, 15);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new Size(200, 21);
            this.lblSummary.TabIndex = 1;
            this.lblSummary.Text = "Import Results";
            
            // btnExportErrors
            this.btnExportErrors.Enabled = _result?.FailedRows > 0;
            this.btnExportErrors.Location = new Point(680, 520);
            this.btnExportErrors.Name = "btnExportErrors";
            this.btnExportErrors.Size = new Size(120, 35);
            this.btnExportErrors.TabIndex = 2;
            this.btnExportErrors.Text = "Export Errors";
            this.btnExportErrors.UseVisualStyleBackColor = true;
            this.btnExportErrors.Click += new EventHandler(this.btnExportErrors_Click);
            
            // btnClose
            this.btnClose.DialogResult = DialogResult.OK;
            this.btnClose.Location = new Point(810, 520);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(80, 35);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            
            // ImportResultDialog
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(900, 565);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExportErrors);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.lblSummary);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportResultDialog";
            this.Padding = new Padding(10, 60, 10, 10);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Import Results";
            
            this.tabControl.ResumeLayout(false);
            this.tabSummary.ResumeLayout(false);
            this.tabSummary.PerformLayout();
            this.tabErrors.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewErrors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        
        private void DisplayResult()
        {
            // Update summary label
            lblSummary.Text = _result.IsSuccessful 
                ? "? Import Completed Successfully"
                : $"? Import Completed with {_result.FailedRows} Error(s)";
            
            lblSummary.ForeColor = _result.IsSuccessful ? Color.Green : Color.DarkOrange;
            
            // Build detailed summary
            var summary = new StringBuilder();
            summary.AppendLine("=== IMPORT RESULTS ===");
            summary.AppendLine();
            summary.AppendLine($"Status: {(_result.IsSuccessful ? "SUCCESS" : "COMPLETED WITH ERRORS")}");
            summary.AppendLine($"Total Rows: {_result.TotalRows}");
            summary.AppendLine($"Successful: {_result.SuccessfulRows}");
            summary.AppendLine($"Failed: {_result.FailedRows}");
            summary.AppendLine($"Success Rate: {(_result.TotalRows > 0 ? (double)_result.SuccessfulRows / _result.TotalRows * 100 : 0):F2}%");
            summary.AppendLine();
            summary.AppendLine($"Started: {_result.StartTime:yyyy-MM-dd HH:mm:ss}");
            summary.AppendLine($"Ended: {_result.EndTime:yyyy-MM-dd HH:mm:ss}");
            summary.AppendLine($"Duration: {_result.Duration.TotalSeconds:F2} seconds");
            summary.AppendLine();
            
            if (_result.FailedRows > 0)
            {
                summary.AppendLine("=== ERROR SUMMARY ===");
                summary.AppendLine();
                
                // Group errors by message
                var errorGroups = _result.Errors
                    .GroupBy(e => e.ErrorMessage)
                    .OrderByDescending(g => g.Count());
                
                foreach (var group in errorGroups)
                {
                    summary.AppendLine($"• {group.Key}");
                    summary.AppendLine($"  Occurrences: {group.Count()}");
                    summary.AppendLine($"  Affected Rows: {string.Join(", ", group.Select(e => e.RowNumber).Take(10))}");
                    if (group.Count() > 10)
                    {
                        summary.AppendLine($"  ... and {group.Count() - 10} more");
                    }
                    summary.AppendLine();
                }
            }
            else
            {
                summary.AppendLine("No errors occurred during import.");
            }
            
            txtSummary.Text = summary.ToString();
            
            // Populate errors grid
            if (_result.FailedRows > 0)
            {
                foreach (var error in _result.Errors)
                {
                    var rowData = string.Join(", ", error.RowData.Take(5).Select(d => 
                        d?.ToString()?.Length > 20 ? d.ToString()!.Substring(0, 20) + "..." : d?.ToString() ?? "NULL"));
                    
                    if (error.RowData.Count > 5)
                    {
                        rowData += $", ... ({error.RowData.Count - 5} more)";
                    }
                    
                    var rowIndex = dataGridViewErrors.Rows.Add();
                    dataGridViewErrors.Rows[rowIndex].Cells[0].Value = error.RowNumber;
                    dataGridViewErrors.Rows[rowIndex].Cells[1].Value = error.ErrorMessage;
                    dataGridViewErrors.Rows[rowIndex].Cells[2].Value = rowData;
                }
                
                // Auto-select errors tab if there are errors
                tabControl.SelectedIndex = 1;
            }
        }
        
        private void btnExportErrors_Click(object? sender, EventArgs e)
        {
            using var saveDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                Title = "Export Error Report",
                FileName = $"ImportErrors_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };
            
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var csv = new StringBuilder();
                    csv.AppendLine("Row Number,Error Message,Row Data");
                    
                    foreach (var error in _result.Errors)
                    {
                        var rowData = string.Join("|", error.RowData.Select(d => 
                            d?.ToString()?.Replace(",", ";").Replace("\n", " ").Replace("\r", " ") ?? "NULL"));
                        
                        csv.AppendLine($"{error.RowNumber},\"{error.ErrorMessage.Replace("\"", "\"\"")}\",\"{rowData}\"");
                    }
                    
                    File.WriteAllText(saveDialog.FileName, csv.ToString());
                    
                    MessageBox.Show($"Error report exported successfully to:\n{saveDialog.FileName}", 
                        "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting report: {ex.Message}", 
                        "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
