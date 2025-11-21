namespace ImportExcelIntoDatabase
{
    public partial class CreateTableDialog : Form
    {
        public string TableName { get; private set; } = string.Empty;
        public List<(string ColumnName, string DataType)> Columns { get; private set; } = new();
        
        private DataGridView dataGridViewColumns;
        private TextBox txtTableName;
        private Button btnOK;
        private Button btnCancel;
        private Label lblTableName;
        private Label lblColumns;
        private DataGridViewTextBoxColumn colColumnName;
        private DataGridViewComboBoxColumn colDataType;
        
        public CreateTableDialog(List<string> columnNames, List<string> suggestedDataTypes)
        {
            InitializeComponent();
            
            // Pre-fill columns
            for (int i = 0; i < columnNames.Count; i++)
            {
                var rowIndex = dataGridViewColumns.Rows.Add();
                dataGridViewColumns.Rows[rowIndex].Cells[0].Value = columnNames[i];
                dataGridViewColumns.Rows[rowIndex].Cells[1].Value = i < suggestedDataTypes.Count 
                    ? suggestedDataTypes[i] 
                    : "NVARCHAR(255)";
            }
        }
        
        private void InitializeComponent()
        {
            this.txtTableName = new TextBox();
            this.lblTableName = new Label();
            this.lblColumns = new Label();
            this.dataGridViewColumns = new DataGridView();
            this.colColumnName = new DataGridViewTextBoxColumn();
            this.colDataType = new DataGridViewComboBoxColumn();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewColumns)).BeginInit();
            this.SuspendLayout();
            
            // lblTableName
            this.lblTableName.AutoSize = true;
            this.lblTableName.Font = new Font("Segoe UI", 10F);
            this.lblTableName.Location = new Point(20, 20);
            this.lblTableName.Name = "lblTableName";
            this.lblTableName.Size = new Size(84, 19);
            this.lblTableName.TabIndex = 0;
            this.lblTableName.Text = "Table Name:";
            
            // txtTableName
            this.txtTableName.Font = new Font("Segoe UI", 10F);
            this.txtTableName.Location = new Point(20, 45);
            this.txtTableName.Name = "txtTableName";
            this.txtTableName.Size = new Size(600, 25);
            this.txtTableName.TabIndex = 1;
            
            // lblColumns
            this.lblColumns.AutoSize = true;
            this.lblColumns.Font = new Font("Segoe UI", 10F);
            this.lblColumns.Location = new Point(20, 85);
            this.lblColumns.Name = "lblColumns";
            this.lblColumns.Size = new Size(325, 19);
            this.lblColumns.TabIndex = 2;
            this.lblColumns.Text = "Columns (an ID column will be added automatically):";
            
            // colColumnName
            this.colColumnName.HeaderText = "Column Name";
            this.colColumnName.Name = "colColumnName";
            this.colColumnName.Width = 300;
            
            // colDataType
            this.colDataType.HeaderText = "Data Type";
            this.colDataType.Name = "colDataType";
            this.colDataType.Items.AddRange(new object[] {
                "NVARCHAR(50)",
                "NVARCHAR(100)",
                "NVARCHAR(255)",
                "NVARCHAR(MAX)",
                "INT",
                "BIGINT",
                "DECIMAL(18,2)",
                "FLOAT",
                "BIT",
                "DATE",
                "DATETIME",
                "DATETIME2"
            });
            this.colDataType.Width = 200;
            
            // dataGridViewColumns
            this.dataGridViewColumns.AllowUserToDeleteRows = false;
            this.dataGridViewColumns.BackgroundColor = SystemColors.Window;
            this.dataGridViewColumns.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewColumns.Columns.AddRange(new DataGridViewColumn[] {
                this.colColumnName,
                this.colDataType
            });
            this.dataGridViewColumns.Location = new Point(20, 110);
            this.dataGridViewColumns.Name = "dataGridViewColumns";
            this.dataGridViewColumns.RowHeadersWidth = 51;
            this.dataGridViewColumns.Size = new Size(600, 300);
            this.dataGridViewColumns.TabIndex = 3;
            
            // btnCancel
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Font = new Font("Segoe UI", 10F);
            this.btnCancel.Location = new Point(445, 425);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(80, 35);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            
            // btnOK
            this.btnOK.BackColor = Color.FromArgb(0, 122, 204);
            this.btnOK.FlatStyle = FlatStyle.Flat;
            this.btnOK.Font = new Font("Segoe UI", 10F);
            this.btnOK.ForeColor = Color.White;
            this.btnOK.Location = new Point(540, 425);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(80, 35);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "Create";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            
            // CreateTableDialog
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new Size(640, 480);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dataGridViewColumns);
            this.Controls.Add(this.lblColumns);
            this.Controls.Add(this.txtTableName);
            this.Controls.Add(this.lblTableName);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateTableDialog";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Create New Table";
            
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewColumns)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        
        private void btnOK_Click(object? sender, EventArgs e)
        {
            // Validate table name
            if (string.IsNullOrWhiteSpace(txtTableName.Text))
            {
                MessageBox.Show("Please enter a table name.", "Validation", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Validate table name characters
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtTableName.Text, @"^[a-zA-Z][a-zA-Z0-9_]*$"))
            {
                MessageBox.Show("Table name must start with a letter and contain only letters, numbers, and underscores.", 
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            TableName = txtTableName.Text;
            Columns.Clear();
            
            // Get columns from grid
            foreach (DataGridViewRow row in dataGridViewColumns.Rows)
            {
                if (row.IsNewRow) continue;
                
                var columnName = row.Cells[0].Value?.ToString();
                var dataType = row.Cells[1].Value?.ToString();
                
                if (string.IsNullOrWhiteSpace(columnName))
                {
                    MessageBox.Show("All columns must have a name.", "Validation", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(dataType))
                {
                    MessageBox.Show("All columns must have a data type.", "Validation", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Validate column name
                if (!System.Text.RegularExpressions.Regex.IsMatch(columnName, @"^[a-zA-Z][a-zA-Z0-9_]*$"))
                {
                    MessageBox.Show($"Column name '{columnName}' must start with a letter and contain only letters, numbers, and underscores.", 
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                Columns.Add((columnName, dataType));
            }
            
            if (Columns.Count == 0)
            {
                MessageBox.Show("Please define at least one column.", "Validation", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
