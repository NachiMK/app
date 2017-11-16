using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DataSeeder.Export.Library;
using Export.Common.Library;

namespace DataSeeder.Export.UI
{
    public partial class ExportDataForm : Form
    {
        public ExportDataForm()
        {
            InitializeComponent();
        }

        private void GetTablesButton_Click(object sender, EventArgs e)
        {
            GetTables();
        }

        private void ExportDataForm_Load(object sender, EventArgs e)
        {
            InitializeForm();
        }

        private void InitializeForm()
        {

            //ResetComboBox_Tables();
            //this.ConditionTxtBox.Text = "";
            //this.OptKeepRowsRadioBtn.Checked = true;
            //this.OptDeleteRowsRadioBtn.Checked = false;
            this.ServerNameTxtBox.Text = "Server=localhost;Database=Project;Trusted_Connection=True;";
            this.StagePathTxtBox.Text = @"C:\SQL\SQL_ETL\Stage\";
            this.DestPathTxtBox.Text = @"\\lpvmdata\WebDevData\Data\NachiM";
            this.ResultsTextBox.Text = "";
            this.ExportTableRadioButton.Select();

            ResetOptions();
        }

        private void ResetOptions()
        {
            ResetComboBox_Encoding();
            ResetComboBox_FileExtension();
            ResetComboBox_DateFormat();
            ResetComboBox_ColSeparator();
            ResetComboBox_RowSeparator();
            this.TextQualifierTxtBox.Text = "\"";
            this.AppendDateCheckBox.Checked = true;
            this.AppendHeaderCheckBox.Checked = true;
        }

        private void GetTables(bool RefreshTableList = true)
        {
            Exception outEx;
            string strSQLToGetTables = "SELECT TableName = TABLE_NAME" +
                                       ", [Export?] = CONVERT(BIT, 0)" +
                                       ", Filter = CONVERT(NVARCHAR(MAX), NULL)" +
                                       ", [Prefix Table Name?] = CONVERT(BIT, 0)" +
                                       "  FROM INFORMATION_SCHEMA.TABLES ORDER BY TABLE_NAME;";
            DataTable exportTable = SQLHelper.ExecuteDataTable(this.ServerNameTxtBox.Text, strSQLToGetTables, out outEx);
            AddResults(outEx?.ToString());
            AddResults(outEx?.InnerException?.ToString());

            if ((outEx == null) && (exportTable != null))
            {
                int width = this.TablesDataGridView.Columns["Filter"].Width;
                this.TablesDataGridView.Columns.Clear();
                this.TablesDataGridView.Rows.Clear();
                this.TablesDataGridView.DataSource = null;
                this.TablesDataGridView.DataBindings.Clear();

                BindingSource SBind = new BindingSource();
                SBind.DataSource = exportTable;
                this.TablesDataGridView.AutoGenerateColumns = true;
                //this.TablesDataGridView.DataSource = exportTable;
                this.TablesDataGridView.DataSource = SBind;
                this.TablesDataGridView.AutoResizeColumns();
                this.TablesDataGridView.Columns["Filter"].Width = width;
                this.TablesDataGridView.AutoResizeRows();
                this.TablesDataGridView.Refresh();
            }
        }

        private void ResetComboBox_RowSeparator()
        {
            Dictionary<int, string> DictRowDelimiters = EnumHelper.EnumDictionary<RowDelimiters>();
            this.RowSepComboBox.DataSource = new BindingSource(DictRowDelimiters, null);
            this.RowSepComboBox.DisplayMember = "Value";
            this.RowSepComboBox.ValueMember = "Key";
            this.RowSepComboBox.SelectedIndex = (int)RowDelimiters.CarriageReturnLineFeed;
        }

        private void ResetComboBox_ColSeparator()
        {
            Dictionary<int, string> DictColDelimiters = EnumHelper.EnumDictionary<ColumnDelimiter>();
            this.ColSepComboBox.DataSource = new BindingSource(DictColDelimiters, null);
            this.ColSepComboBox.DisplayMember = "Value";
            this.ColSepComboBox.ValueMember = "Key";
            this.ColSepComboBox.SelectedIndex = (int)ColumnDelimiter.Comma;
        }

        private void ResetComboBox_DateFormat()
        {
            Dictionary<int, string> DictDateFormats = EnumHelper.EnumDictionary<DateFormat>();
            this.DateFormatComboBox.DataSource = new BindingSource(DictDateFormats, null);
            this.DateFormatComboBox.DisplayMember = "Value";
            this.DateFormatComboBox.ValueMember = "Key";
            this.DateFormatComboBox.SelectedIndex = (int)DateFormat.yyyyMMdd_HHmmssfff;
        }

        private void ResetComboBox_FileExtension()
        {
            Dictionary<int, string> DictFileExt = EnumHelper.EnumDictionary<ExportToFileTypeEnum>();
            this.FileExtComboBox.DataSource = new BindingSource(DictFileExt, null);
            this.FileExtComboBox.DisplayMember = "Value";
            this.FileExtComboBox.ValueMember = "Key";
            this.FileExtComboBox.SelectedIndex = (int)ExportToFileTypeEnum.csv;
        }

        private void ResetComboBox_Encoding()
        {
            SortedDictionary<int, string> EncodingDictionary = ExportToFileEnumHelper.FileEncodingTypeStrings();
            this.FileEncodingComboBox.DataSource = new BindingSource(EncodingDictionary, null);
            this.FileEncodingComboBox.DisplayMember = "Value";
            this.FileEncodingComboBox.ValueMember = "Key";
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            InitializeForm();
        }

        private void SubmitBtn_Click(object sender, EventArgs e)
        {
            ExportTables(GetTablesToExport());
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ExportTableRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (ExportTableRadioButton.Checked)
                this.ExportTabControl.SelectTab(0);
        }

        private void ExportQueryRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (ExportQueryRadioButton.Checked)
                this.ExportTabControl.SelectTab(1);
        }

        private void AddResults(string msg)
        {
            this.ResultsTextBox.Text = $"{msg}{System.Environment.NewLine}{this.ResultsTextBox.Text}";
        }

        private ExportTableList GetTablesToExport()
        {
            ExportTableList exportList = new ExportTableList();
            foreach(DataGridViewRow row in this.TablesDataGridView.Rows)
            {
                DataGridViewCell cellExport = row.Cells["Export?"];
                DataGridViewCell cellFilter = row.Cells["Filter"];
                DataGridViewCell cellTableName = row.Cells["TableName"];
                DataGridViewCell cellPrefix = row.Cells["Prefix Table Name?"];
                bool.TryParse(cellExport.Value.ToString(), out bool TableSelected);
                if (TableSelected)
                    exportList.Add(new ExportTable(cellTableName.Value.ToString(), cellFilter?.Value?.ToString(), cellTableName.Value.ToString()));
            }

            return exportList;
        }

        private void ExportTables(ExportTableList exportTableList)
        {
            ExportToFile exportToFile = ExportToFile.NewExportToFile(ServerNameTxtBox.Text, DestPathTxtBox.Text, StagePathTxtBox.Text);
            ExportFileOptions fileOptions = GetExportFileOptions();
            ExportDataResults expResults = exportToFile.ExportTables(exportTableList, fileOptions);
            DisplayResults(expResults);
        }

        private ExportFileOptions GetExportFileOptions()
        {
            ExportFileOptions fileOptions = new ExportFileOptions();
            fileOptions.ExportToFileType = (ExportToFileTypeEnum)(int)this.FileExtComboBox.SelectedValue;
            fileOptions.FileEncoding = Encoding.GetEncoding((int)this.FileEncodingComboBox.SelectedValue);
            if (fileOptions.FileEncoding == Encoding.UTF8)
            {
                fileOptions.FileEncoding = new UTF8Encoding(false, false);
            }

            fileOptions.AppendDateFormat = this.AppendDateCheckBox.Checked && this.DateFormatComboBox.SelectedIndex > 0;
            fileOptions.IncludeHeaders = this.AppendHeaderCheckBox.Checked;
            fileOptions.FileDateFormat = (DateFormat)(int)this.DateFormatComboBox.SelectedValue;
            fileOptions.ColDelimiter = (ColumnDelimiter)(int)this.ColSepComboBox.SelectedValue;
            fileOptions.RowDelimiter = (RowDelimiters)(int)this.RowSepComboBox.SelectedValue;
            fileOptions.MaxRowsPerFile = (int)RowsInFileNumUpDown.Value;
            fileOptions.IncludeTextQualifiers = TextQualifierTxtBox.Text.Length > 0;
            fileOptions.TextQualifier = TextQualifierTxtBox.Text[0];
            fileOptions.CompressionType = FileCompressionTypeEnum.None;
            return fileOptions;
        }

        private void DisplayResults(ExportDataResults expResults)
        {
            AddResults(expResults.ToString());
        }
    }
}