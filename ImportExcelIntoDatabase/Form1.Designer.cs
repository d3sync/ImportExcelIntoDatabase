namespace ImportExcelIntoDatabase
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            tabControl = new TabControl();
            tabExcel = new TabPage();
            groupExcelPreview = new GroupBox();
            dataGridViewPreview = new DataGridView();
            groupExcelConfig = new GroupBox();
            numStartRow = new NumericUpDown();
            chkHasHeaders = new CheckBox();
            lblStartRow = new Label();
            btnBrowseExcel = new Button();
            txtExcelPath = new TextBox();
            lblExcelFile = new Label();
            tabDatabase = new TabPage();
            groupTableSelection = new GroupBox();
            btnCreateNewTable = new Button();
            cmbTables = new ComboBox();
            lblTable = new Label();
            cmbDatabases = new ComboBox();
            lblDatabase = new Label();
            groupConnection = new GroupBox();
            btnTestConnection = new Button();
            txtPassword = new TextBox();
            lblPassword = new Label();
            txtUsername = new TextBox();
            lblUsername = new Label();
            chkWindowsAuth = new CheckBox();
            txtServer = new TextBox();
            lblServer = new Label();
            tabMapping = new TabPage();
            dataGridViewMapping = new DataGridView();
            colSelect = new DataGridViewCheckBoxColumn();
            colExcelColumn = new DataGridViewTextBoxColumn();
            colSqlColumn = new DataGridViewComboBoxColumn();
            colDataType = new DataGridViewTextBoxColumn();
            btnDeselectAll = new Button();
            btnSelectAll = new Button();
            tabImport = new TabPage();
            groupImportSummary = new GroupBox();
            txtSummary = new TextBox();
            groupImportStatus = new GroupBox();
            groupErrorHandling = new GroupBox();
            lblErrorHandling = new Label();
            rbUseTransaction = new RadioButton();
            rbSkipErrors = new RadioButton();
            rbStopOnError = new RadioButton();
            lblProgress = new Label();
            btnStartImport = new Button();
            btnValidateData = new Button();
            progressBar = new ProgressBar();
            lblStatus = new Label();
            panelNavigation = new Panel();
            btnNext = new Button();
            btnPrevious = new Button();
            tabControl.SuspendLayout();
            tabExcel.SuspendLayout();
            groupExcelPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPreview).BeginInit();
            groupExcelConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numStartRow).BeginInit();
            tabDatabase.SuspendLayout();
            groupTableSelection.SuspendLayout();
            groupConnection.SuspendLayout();
            tabMapping.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewMapping).BeginInit();
            tabImport.SuspendLayout();
            groupImportSummary.SuspendLayout();
            groupImportStatus.SuspendLayout();
            groupErrorHandling.SuspendLayout();
            panelNavigation.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabExcel);
            tabControl.Controls.Add(tabDatabase);
            tabControl.Controls.Add(tabMapping);
            tabControl.Controls.Add(tabImport);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Font = new Font("Segoe UI", 10F);
            tabControl.Location = new Point(0, 0);
            tabControl.Margin = new Padding(3, 4, 3, 4);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1143, 867);
            tabControl.TabIndex = 0;
            // 
            // tabExcel
            // 
            tabExcel.Controls.Add(groupExcelPreview);
            tabExcel.Controls.Add(groupExcelConfig);
            tabExcel.Location = new Point(4, 32);
            tabExcel.Margin = new Padding(3, 4, 3, 4);
            tabExcel.Name = "tabExcel";
            tabExcel.Padding = new Padding(17, 20, 17, 20);
            tabExcel.Size = new Size(1135, 831);
            tabExcel.TabIndex = 0;
            tabExcel.Text = "1. Excel File";
            tabExcel.UseVisualStyleBackColor = true;
            // 
            // groupExcelPreview
            // 
            groupExcelPreview.Controls.Add(dataGridViewPreview);
            groupExcelPreview.Dock = DockStyle.Fill;
            groupExcelPreview.Location = new Point(17, 220);
            groupExcelPreview.Margin = new Padding(3, 4, 3, 4);
            groupExcelPreview.Name = "groupExcelPreview";
            groupExcelPreview.Padding = new Padding(17, 20, 17, 20);
            groupExcelPreview.Size = new Size(1101, 591);
            groupExcelPreview.TabIndex = 1;
            groupExcelPreview.TabStop = false;
            groupExcelPreview.Text = "Data Preview";
            // 
            // dataGridViewPreview
            // 
            dataGridViewPreview.AllowUserToAddRows = false;
            dataGridViewPreview.AllowUserToDeleteRows = false;
            dataGridViewPreview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewPreview.BackgroundColor = SystemColors.Window;
            dataGridViewPreview.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewPreview.Dock = DockStyle.Fill;
            dataGridViewPreview.Location = new Point(17, 43);
            dataGridViewPreview.Margin = new Padding(3, 4, 3, 4);
            dataGridViewPreview.Name = "dataGridViewPreview";
            dataGridViewPreview.ReadOnly = true;
            dataGridViewPreview.RowHeadersWidth = 51;
            dataGridViewPreview.Size = new Size(1067, 528);
            dataGridViewPreview.TabIndex = 0;
            // 
            // groupExcelConfig
            // 
            groupExcelConfig.Controls.Add(numStartRow);
            groupExcelConfig.Controls.Add(chkHasHeaders);
            groupExcelConfig.Controls.Add(lblStartRow);
            groupExcelConfig.Controls.Add(btnBrowseExcel);
            groupExcelConfig.Controls.Add(txtExcelPath);
            groupExcelConfig.Controls.Add(lblExcelFile);
            groupExcelConfig.Dock = DockStyle.Top;
            groupExcelConfig.Location = new Point(17, 20);
            groupExcelConfig.Margin = new Padding(3, 4, 3, 4);
            groupExcelConfig.Name = "groupExcelConfig";
            groupExcelConfig.Padding = new Padding(17, 20, 17, 20);
            groupExcelConfig.Size = new Size(1101, 200);
            groupExcelConfig.TabIndex = 0;
            groupExcelConfig.TabStop = false;
            groupExcelConfig.Text = "Excel Configuration";
            // 
            // numStartRow
            // 
            numStartRow.Location = new Point(450, 132);
            numStartRow.Margin = new Padding(3, 4, 3, 4);
            numStartRow.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numStartRow.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numStartRow.Name = "numStartRow";
            numStartRow.Size = new Size(91, 30);
            numStartRow.TabIndex = 5;
            numStartRow.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // chkHasHeaders
            // 
            chkHasHeaders.AutoSize = true;
            chkHasHeaders.Location = new Point(17, 133);
            chkHasHeaders.Margin = new Padding(3, 4, 3, 4);
            chkHasHeaders.Name = "chkHasHeaders";
            chkHasHeaders.Size = new Size(230, 27);
            chkHasHeaders.TabIndex = 3;
            chkHasHeaders.Text = "First row contains headers";
            chkHasHeaders.UseVisualStyleBackColor = true;
            chkHasHeaders.CheckedChanged += chkHasHeaders_CheckedChanged;
            // 
            // lblStartRow
            // 
            lblStartRow.AutoSize = true;
            lblStartRow.Location = new Point(286, 135);
            lblStartRow.Name = "lblStartRow";
            lblStartRow.Size = new Size(149, 23);
            lblStartRow.TabIndex = 4;
            lblStartRow.Text = "Data starts at row:";
            // 
            // btnBrowseExcel
            // 
            btnBrowseExcel.Location = new Point(829, 71);
            btnBrowseExcel.Margin = new Padding(3, 4, 3, 4);
            btnBrowseExcel.Name = "btnBrowseExcel";
            btnBrowseExcel.Size = new Size(114, 40);
            btnBrowseExcel.TabIndex = 2;
            btnBrowseExcel.Text = "Browse...";
            btnBrowseExcel.UseVisualStyleBackColor = true;
            btnBrowseExcel.Click += btnBrowseExcel_Click;
            // 
            // txtExcelPath
            // 
            txtExcelPath.Location = new Point(17, 73);
            txtExcelPath.Margin = new Padding(3, 4, 3, 4);
            txtExcelPath.Name = "txtExcelPath";
            txtExcelPath.ReadOnly = true;
            txtExcelPath.Size = new Size(799, 30);
            txtExcelPath.TabIndex = 1;
            // 
            // lblExcelFile
            // 
            lblExcelFile.AutoSize = true;
            lblExcelFile.Location = new Point(17, 40);
            lblExcelFile.Name = "lblExcelFile";
            lblExcelFile.Size = new Size(82, 23);
            lblExcelFile.TabIndex = 0;
            lblExcelFile.Text = "Excel File:";
            // 
            // tabDatabase
            // 
            tabDatabase.Controls.Add(groupTableSelection);
            tabDatabase.Controls.Add(groupConnection);
            tabDatabase.Location = new Point(4, 32);
            tabDatabase.Margin = new Padding(3, 4, 3, 4);
            tabDatabase.Name = "tabDatabase";
            tabDatabase.Padding = new Padding(17, 20, 17, 20);
            tabDatabase.Size = new Size(1135, 831);
            tabDatabase.TabIndex = 1;
            tabDatabase.Text = "2. Database";
            tabDatabase.UseVisualStyleBackColor = true;
            // 
            // groupTableSelection
            // 
            groupTableSelection.Controls.Add(btnCreateNewTable);
            groupTableSelection.Controls.Add(cmbTables);
            groupTableSelection.Controls.Add(lblTable);
            groupTableSelection.Controls.Add(cmbDatabases);
            groupTableSelection.Controls.Add(lblDatabase);
            groupTableSelection.Dock = DockStyle.Top;
            groupTableSelection.Location = new Point(17, 327);
            groupTableSelection.Margin = new Padding(3, 4, 3, 4);
            groupTableSelection.Name = "groupTableSelection";
            groupTableSelection.Padding = new Padding(17, 20, 17, 20);
            groupTableSelection.Size = new Size(1101, 240);
            groupTableSelection.TabIndex = 1;
            groupTableSelection.TabStop = false;
            groupTableSelection.Text = "Target Selection";
            // 
            // btnCreateNewTable
            // 
            btnCreateNewTable.BackColor = Color.FromArgb(0, 122, 204);
            btnCreateNewTable.FlatStyle = FlatStyle.Flat;
            btnCreateNewTable.ForeColor = Color.White;
            btnCreateNewTable.Location = new Point(17, 193);
            btnCreateNewTable.Margin = new Padding(3, 4, 3, 4);
            btnCreateNewTable.Name = "btnCreateNewTable";
            btnCreateNewTable.Size = new Size(206, 40);
            btnCreateNewTable.TabIndex = 4;
            btnCreateNewTable.Text = "➕ Create New Table";
            btnCreateNewTable.UseVisualStyleBackColor = false;
            btnCreateNewTable.Click += btnCreateNewTable_Click;
            // 
            // cmbTables
            // 
            cmbTables.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbTables.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbTables.FormattingEnabled = true;
            cmbTables.Location = new Point(17, 153);
            cmbTables.Margin = new Padding(3, 4, 3, 4);
            cmbTables.Name = "cmbTables";
            cmbTables.Size = new Size(457, 31);
            cmbTables.TabIndex = 3;
            // 
            // lblTable
            // 
            lblTable.AutoSize = true;
            lblTable.Location = new Point(17, 120);
            lblTable.Name = "lblTable";
            lblTable.Size = new Size(245, 23);
            lblTable.TabIndex = 2;
            lblTable.Text = "Table (type to search or select):";
            // 
            // cmbDatabases
            // 
            cmbDatabases.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDatabases.FormattingEnabled = true;
            cmbDatabases.Location = new Point(17, 73);
            cmbDatabases.Margin = new Padding(3, 4, 3, 4);
            cmbDatabases.Name = "cmbDatabases";
            cmbDatabases.Size = new Size(457, 31);
            cmbDatabases.TabIndex = 1;
            cmbDatabases.SelectedIndexChanged += cmbDatabases_SelectedIndexChanged;
            // 
            // lblDatabase
            // 
            lblDatabase.AutoSize = true;
            lblDatabase.Location = new Point(17, 40);
            lblDatabase.Name = "lblDatabase";
            lblDatabase.Size = new Size(85, 23);
            lblDatabase.TabIndex = 0;
            lblDatabase.Text = "Database:";
            // 
            // groupConnection
            // 
            groupConnection.Controls.Add(btnTestConnection);
            groupConnection.Controls.Add(txtPassword);
            groupConnection.Controls.Add(lblPassword);
            groupConnection.Controls.Add(txtUsername);
            groupConnection.Controls.Add(lblUsername);
            groupConnection.Controls.Add(chkWindowsAuth);
            groupConnection.Controls.Add(txtServer);
            groupConnection.Controls.Add(lblServer);
            groupConnection.Dock = DockStyle.Top;
            groupConnection.Location = new Point(17, 20);
            groupConnection.Margin = new Padding(3, 4, 3, 4);
            groupConnection.Name = "groupConnection";
            groupConnection.Padding = new Padding(17, 20, 17, 20);
            groupConnection.Size = new Size(1101, 307);
            groupConnection.TabIndex = 0;
            groupConnection.TabStop = false;
            groupConnection.Text = "Connection Settings";
            // 
            // btnTestConnection
            // 
            btnTestConnection.Location = new Point(17, 253);
            btnTestConnection.Margin = new Padding(3, 4, 3, 4);
            btnTestConnection.Name = "btnTestConnection";
            btnTestConnection.Size = new Size(171, 40);
            btnTestConnection.TabIndex = 7;
            btnTestConnection.Text = "Test Connection";
            btnTestConnection.UseVisualStyleBackColor = true;
            btnTestConnection.Click += btnTestConnection_Click;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(497, 207);
            txtPassword.Margin = new Padding(3, 4, 3, 4);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '●';
            txtPassword.Size = new Size(457, 30);
            txtPassword.TabIndex = 6;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(497, 173);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(84, 23);
            lblPassword.TabIndex = 5;
            lblPassword.Text = "Password:";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(17, 207);
            txtUsername.Margin = new Padding(3, 4, 3, 4);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(457, 30);
            txtUsername.TabIndex = 4;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(17, 173);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(91, 23);
            lblUsername.TabIndex = 3;
            lblUsername.Text = "Username:";
            // 
            // chkWindowsAuth
            // 
            chkWindowsAuth.AutoSize = true;
            chkWindowsAuth.Location = new Point(17, 127);
            chkWindowsAuth.Margin = new Padding(3, 4, 3, 4);
            chkWindowsAuth.Name = "chkWindowsAuth";
            chkWindowsAuth.Size = new Size(252, 27);
            chkWindowsAuth.TabIndex = 2;
            chkWindowsAuth.Text = "Use Windows Authentication";
            chkWindowsAuth.UseVisualStyleBackColor = true;
            chkWindowsAuth.CheckedChanged += chkWindowsAuth_CheckedChanged;
            // 
            // txtServer
            // 
            txtServer.Location = new Point(17, 73);
            txtServer.Margin = new Padding(3, 4, 3, 4);
            txtServer.Name = "txtServer";
            txtServer.Size = new Size(457, 30);
            txtServer.TabIndex = 1;
            // 
            // lblServer
            // 
            lblServer.AutoSize = true;
            lblServer.Location = new Point(17, 40);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(112, 23);
            lblServer.TabIndex = 0;
            lblServer.Text = "Server Name:";
            // 
            // tabMapping
            // 
            tabMapping.Controls.Add(dataGridViewMapping);
            tabMapping.Controls.Add(btnDeselectAll);
            tabMapping.Controls.Add(btnSelectAll);
            tabMapping.Location = new Point(4, 32);
            tabMapping.Margin = new Padding(3, 4, 3, 4);
            tabMapping.Name = "tabMapping";
            tabMapping.Padding = new Padding(17, 20, 17, 20);
            tabMapping.Size = new Size(1135, 831);
            tabMapping.TabIndex = 2;
            tabMapping.Text = "3. Column Mapping";
            tabMapping.UseVisualStyleBackColor = true;
            // 
            // dataGridViewMapping
            // 
            dataGridViewMapping.AllowUserToAddRows = false;
            dataGridViewMapping.AllowUserToDeleteRows = false;
            dataGridViewMapping.BackgroundColor = SystemColors.Window;
            dataGridViewMapping.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewMapping.Columns.AddRange(new DataGridViewColumn[] { colSelect, colExcelColumn, colSqlColumn, colDataType });
            dataGridViewMapping.Dock = DockStyle.Bottom;
            dataGridViewMapping.Location = new Point(17, 80);
            dataGridViewMapping.Margin = new Padding(3, 4, 3, 4);
            dataGridViewMapping.Name = "dataGridViewMapping";
            dataGridViewMapping.RowHeadersWidth = 51;
            dataGridViewMapping.Size = new Size(1101, 731);
            dataGridViewMapping.TabIndex = 2;
            // 
            // colSelect
            // 
            colSelect.HeaderText = "Import";
            colSelect.MinimumWidth = 6;
            colSelect.Name = "colSelect";
            colSelect.Width = 70;
            // 
            // colExcelColumn
            // 
            colExcelColumn.HeaderText = "Excel Column";
            colExcelColumn.MinimumWidth = 6;
            colExcelColumn.Name = "colExcelColumn";
            colExcelColumn.ReadOnly = true;
            colExcelColumn.Width = 200;
            // 
            // colSqlColumn
            // 
            colSqlColumn.HeaderText = "SQL Column";
            colSqlColumn.MinimumWidth = 6;
            colSqlColumn.Name = "colSqlColumn";
            colSqlColumn.Width = 200;
            // 
            // colDataType
            // 
            colDataType.HeaderText = "Data Type";
            colDataType.MinimumWidth = 6;
            colDataType.Name = "colDataType";
            colDataType.ReadOnly = true;
            colDataType.Width = 150;
            // 
            // btnDeselectAll
            // 
            btnDeselectAll.Location = new Point(143, 20);
            btnDeselectAll.Margin = new Padding(3, 4, 3, 4);
            btnDeselectAll.Name = "btnDeselectAll";
            btnDeselectAll.Size = new Size(114, 40);
            btnDeselectAll.TabIndex = 1;
            btnDeselectAll.Text = "Deselect All";
            btnDeselectAll.UseVisualStyleBackColor = true;
            btnDeselectAll.Click += btnDeselectAll_Click;
            // 
            // btnSelectAll
            // 
            btnSelectAll.Location = new Point(17, 20);
            btnSelectAll.Margin = new Padding(3, 4, 3, 4);
            btnSelectAll.Name = "btnSelectAll";
            btnSelectAll.Size = new Size(114, 40);
            btnSelectAll.TabIndex = 0;
            btnSelectAll.Text = "Select All";
            btnSelectAll.UseVisualStyleBackColor = true;
            btnSelectAll.Click += btnSelectAll_Click;
            // 
            // tabImport
            // 
            tabImport.Controls.Add(groupImportSummary);
            tabImport.Controls.Add(groupImportStatus);
            tabImport.Location = new Point(4, 32);
            tabImport.Margin = new Padding(3, 4, 3, 4);
            tabImport.Name = "tabImport";
            tabImport.Padding = new Padding(17, 20, 17, 20);
            tabImport.Size = new Size(1135, 831);
            tabImport.TabIndex = 3;
            tabImport.Text = "4. Import";
            tabImport.UseVisualStyleBackColor = true;
            // 
            // groupImportSummary
            // 
            groupImportSummary.Controls.Add(txtSummary);
            groupImportSummary.Dock = DockStyle.Fill;
            groupImportSummary.Location = new Point(17, 407);
            groupImportSummary.Margin = new Padding(3, 4, 3, 4);
            groupImportSummary.Name = "groupImportSummary";
            groupImportSummary.Padding = new Padding(17, 20, 17, 20);
            groupImportSummary.Size = new Size(1101, 404);
            groupImportSummary.TabIndex = 1;
            groupImportSummary.TabStop = false;
            groupImportSummary.Text = "Summary";
            // 
            // txtSummary
            // 
            txtSummary.BackColor = SystemColors.Window;
            txtSummary.Dock = DockStyle.Fill;
            txtSummary.Font = new Font("Consolas", 9.75F);
            txtSummary.Location = new Point(17, 43);
            txtSummary.Margin = new Padding(3, 4, 3, 4);
            txtSummary.Multiline = true;
            txtSummary.Name = "txtSummary";
            txtSummary.ReadOnly = true;
            txtSummary.ScrollBars = ScrollBars.Vertical;
            txtSummary.Size = new Size(1067, 341);
            txtSummary.TabIndex = 0;
            // 
            // groupImportStatus
            // 
            groupImportStatus.Controls.Add(groupErrorHandling);
            groupImportStatus.Controls.Add(lblProgress);
            groupImportStatus.Controls.Add(btnStartImport);
            groupImportStatus.Controls.Add(btnValidateData);
            groupImportStatus.Controls.Add(progressBar);
            groupImportStatus.Controls.Add(lblStatus);
            groupImportStatus.Dock = DockStyle.Top;
            groupImportStatus.Location = new Point(17, 20);
            groupImportStatus.Margin = new Padding(3, 4, 3, 4);
            groupImportStatus.Name = "groupImportStatus";
            groupImportStatus.Padding = new Padding(17, 20, 17, 20);
            groupImportStatus.Size = new Size(1101, 387);
            groupImportStatus.TabIndex = 0;
            groupImportStatus.TabStop = false;
            groupImportStatus.Text = "Import Configuration & Status";
            // 
            // groupErrorHandling
            // 
            groupErrorHandling.Controls.Add(lblErrorHandling);
            groupErrorHandling.Controls.Add(rbUseTransaction);
            groupErrorHandling.Controls.Add(rbSkipErrors);
            groupErrorHandling.Controls.Add(rbStopOnError);
            groupErrorHandling.Location = new Point(17, 40);
            groupErrorHandling.Margin = new Padding(3, 4, 3, 4);
            groupErrorHandling.Name = "groupErrorHandling";
            groupErrorHandling.Padding = new Padding(3, 4, 3, 4);
            groupErrorHandling.Size = new Size(1065, 173);
            groupErrorHandling.TabIndex = 0;
            groupErrorHandling.TabStop = false;
            groupErrorHandling.Text = "Error Handling Strategy";
            // 
            // lblErrorHandling
            // 
            lblErrorHandling.ForeColor = Color.DarkGray;
            lblErrorHandling.Location = new Point(11, 133);
            lblErrorHandling.Name = "lblErrorHandling";
            lblErrorHandling.Size = new Size(1042, 33);
            lblErrorHandling.TabIndex = 3;
            lblErrorHandling.Text = "Choose how to handle rows that fail to import";
            // 
            // rbUseTransaction
            // 
            rbUseTransaction.AutoSize = true;
            rbUseTransaction.Checked = true;
            rbUseTransaction.Location = new Point(11, 33);
            rbUseTransaction.Margin = new Padding(3, 4, 3, 4);
            rbUseTransaction.Name = "rbUseTransaction";
            rbUseTransaction.Size = new Size(612, 27);
            rbUseTransaction.TabIndex = 0;
            rbUseTransaction.TabStop = true;
            rbUseTransaction.Text = "Use Transaction - All or Nothing (Safest: If any row fails, rollback all changes)";
            rbUseTransaction.UseVisualStyleBackColor = true;
            // 
            // rbSkipErrors
            // 
            rbSkipErrors.AutoSize = true;
            rbSkipErrors.Location = new Point(11, 67);
            rbSkipErrors.Margin = new Padding(3, 4, 3, 4);
            rbSkipErrors.Name = "rbSkipErrors";
            rbSkipErrors.Size = new Size(686, 27);
            rbSkipErrors.TabIndex = 1;
            rbSkipErrors.Text = "Skip Errors and Continue - Import valid rows, skip invalid ones (Best for large datasets)";
            rbSkipErrors.UseVisualStyleBackColor = true;
            // 
            // rbStopOnError
            // 
            rbStopOnError.AutoSize = true;
            rbStopOnError.Location = new Point(11, 100);
            rbStopOnError.Margin = new Padding(3, 4, 3, 4);
            rbStopOnError.Name = "rbStopOnError";
            rbStopOnError.Size = new Size(751, 27);
            rbStopOnError.TabIndex = 2;
            rbStopOnError.Text = "Stop on First Error - Stop immediately when error occurs (Keep successful imports before error)";
            rbStopOnError.UseVisualStyleBackColor = true;
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblProgress.Location = new Point(17, 227);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(111, 20);
            lblProgress.TabIndex = 1;
            lblProgress.Text = "Progress: 0 / 0";
            // 
            // btnStartImport
            // 
            btnStartImport.BackColor = Color.FromArgb(0, 122, 204);
            btnStartImport.FlatStyle = FlatStyle.Flat;
            btnStartImport.ForeColor = Color.White;
            btnStartImport.Location = new Point(17, 333);
            btnStartImport.Margin = new Padding(3, 4, 3, 4);
            btnStartImport.Name = "btnStartImport";
            btnStartImport.Size = new Size(171, 47);
            btnStartImport.TabIndex = 4;
            btnStartImport.Text = "Start Import";
            btnStartImport.UseVisualStyleBackColor = false;
            btnStartImport.Click += btnStartImport_Click;
            // 
            // btnValidateData
            // 
            btnValidateData.BackColor = Color.FromArgb(0, 122, 204);
            btnValidateData.FlatStyle = FlatStyle.Flat;
            btnValidateData.ForeColor = Color.White;
            btnValidateData.Location = new Point(206, 333);
            btnValidateData.Margin = new Padding(3, 4, 3, 4);
            btnValidateData.Name = "btnValidateData";
            btnValidateData.Size = new Size(171, 47);
            btnValidateData.TabIndex = 5;
            btnValidateData.Text = "Validate Data";
            btnValidateData.UseVisualStyleBackColor = false;
            btnValidateData.Click += btnValidateData_Click;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(17, 293);
            progressBar.Margin = new Padding(3, 4, 3, 4);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(1065, 33);
            progressBar.TabIndex = 3;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(17, 260);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(172, 23);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "Ready to start import";
            // 
            // panelNavigation
            // 
            panelNavigation.Controls.Add(btnNext);
            panelNavigation.Controls.Add(btnPrevious);
            panelNavigation.Dock = DockStyle.Bottom;
            panelNavigation.Location = new Point(0, 867);
            panelNavigation.Margin = new Padding(3, 4, 3, 4);
            panelNavigation.Name = "panelNavigation";
            panelNavigation.Size = new Size(1143, 80);
            panelNavigation.TabIndex = 1;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(1006, 20);
            btnNext.Margin = new Padding(3, 4, 3, 4);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(114, 47);
            btnNext.TabIndex = 1;
            btnNext.Text = "Next >";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click;
            // 
            // btnPrevious
            // 
            btnPrevious.Enabled = false;
            btnPrevious.Location = new Point(880, 20);
            btnPrevious.Margin = new Padding(3, 4, 3, 4);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new Size(114, 47);
            btnPrevious.TabIndex = 0;
            btnPrevious.Text = "< Previous";
            btnPrevious.UseVisualStyleBackColor = true;
            btnPrevious.Click += btnPrevious_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1143, 947);
            Controls.Add(tabControl);
            Controls.Add(panelNavigation);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Excel to Database Importer";
            Load += Form1_Load;
            tabControl.ResumeLayout(false);
            tabExcel.ResumeLayout(false);
            groupExcelPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewPreview).EndInit();
            groupExcelConfig.ResumeLayout(false);
            groupExcelConfig.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numStartRow).EndInit();
            tabDatabase.ResumeLayout(false);
            groupTableSelection.ResumeLayout(false);
            groupTableSelection.PerformLayout();
            groupConnection.ResumeLayout(false);
            groupConnection.PerformLayout();
            tabMapping.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewMapping).EndInit();
            tabImport.ResumeLayout(false);
            groupImportSummary.ResumeLayout(false);
            groupImportSummary.PerformLayout();
            groupImportStatus.ResumeLayout(false);
            groupImportStatus.PerformLayout();
            groupErrorHandling.ResumeLayout(false);
            groupErrorHandling.PerformLayout();
            panelNavigation.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabExcel;
        private System.Windows.Forms.TabPage tabDatabase;
        private System.Windows.Forms.TabPage tabMapping;
        private System.Windows.Forms.TabPage tabImport;
        private System.Windows.Forms.GroupBox groupExcelConfig;
        private System.Windows.Forms.Label lblExcelFile;
        private System.Windows.Forms.TextBox txtExcelPath;
        private System.Windows.Forms.Button btnBrowseExcel;
        private System.Windows.Forms.CheckBox chkHasHeaders;
        private System.Windows.Forms.Label lblStartRow;
        private System.Windows.Forms.NumericUpDown numStartRow;
        private System.Windows.Forms.GroupBox groupExcelPreview;
        private System.Windows.Forms.DataGridView dataGridViewPreview;
        private System.Windows.Forms.GroupBox groupConnection;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.CheckBox chkWindowsAuth;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.GroupBox groupTableSelection;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.ComboBox cmbDatabases;
        private System.Windows.Forms.Label lblTable;
        private System.Windows.Forms.ComboBox cmbTables;
        private System.Windows.Forms.Button btnCreateNewTable;
        private System.Windows.Forms.DataGridView dataGridViewMapping;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnDeselectAll;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExcelColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn colSqlColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataType;
        private System.Windows.Forms.GroupBox groupImportStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnStartImport;
        private System.Windows.Forms.Button btnValidateData;
        private System.Windows.Forms.GroupBox groupErrorHandling;
        private System.Windows.Forms.Label lblErrorHandling;
        private System.Windows.Forms.RadioButton rbStopOnError;
        private System.Windows.Forms.RadioButton rbSkipErrors;
        private System.Windows.Forms.RadioButton rbUseTransaction;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.GroupBox groupImportSummary;
        private System.Windows.Forms.TextBox txtSummary;
        private System.Windows.Forms.Panel panelNavigation;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnNext;
    }
}
