using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RestoreSQLBackupLibrary;

namespace RestoreSQLBackups
{
    public partial class RestoreProdToDBTestForm : Form
    {
        public RestoreProdToDBTestForm()
        {
            InitializeComponent();
        }

        private void InitializetheForm(bool ForDev = false)
        {

            this.NetworkPathTextBox.Text = ForDev ? @"\\676179-DEVSQL\PROD_DB_Backup\739781-SQLCLUS\" : @"\\lpvmdata\WebDevData\Data\NachiM";
            this.BackupPathTxtBox.Text = ForDev ? @"D:\SQL_BACKUP" : @"C:\SQL\SQL_BACKUP";
            this.DataPathTextBox.Text = ForDev ? @"D:\SQL_DATA" : @"C:\SQL\SQL_DATA";
            this.LogPathTextBox.Text = ForDev ? @"D:\SQL_LOG" : @"C:\SQL\SQL_LOG";
            this.ServerNameTxtBox.Text = ForDev ? "Server=Prod1_DBTest2;Database=DBATools;Trusted_Connection=True;" 
                                                    : "Server=localhost;Database=DBATools;Trusted_Connection=True;";
            this.CleanUpProcName.Text = "dbo.usp_TruncateTableDBTest2_PostRestore";
            this.ResultTxtBox.Text = "";
            this.ResultTxtBox.ReadOnly = true;

            AddDBAndSetState(false);
            this.UsersToRestoreTxtBox.Text = this.DBToRestoreTxtBox.Text.Replace(",", ":lpsqlrw1,") + ":lpsqlrw1";

            this.ShrinkDBCheckBox.Checked = true;
            this.CleanupTablesCheckbox.Checked = true;
            this.DeleteBackupCheckBox.Checked = true;
            this.EnterConfigRadioBtn.Checked = true;

            if (ForDev)
            {
                this.DBToRestoreTxtBox.Text = "Assets,CommonDB,Products";
                this.UsersToRestoreTxtBox.Text = "Assets:ALL_USERS,CommonDB:ALL_SQL_USERS,Products:lpsqlrw1";
            }
        }

        private void AddDBAndSetState(bool onOrOff, bool addOptionalDBs = true)
        {
            this.DBToRestoreTxtBox.Text = string.Empty;
            this.DBToRestoreTxtBox.Text = String.Join(",", DBRestoreHelper.DatabaseToRestore().ToArray());
            if (addOptionalDBs)
                this.DBToRestoreTxtBox.Text += String.Join(",", DBRestoreHelper.OptionalDBToRestore().ToArray());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SubmitBtn_Click(object sender, EventArgs e)
        {
            DBRestoreParam dBRestoreParam = null;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (EnterConfigRadioBtn.Checked)
                {
                    dBRestoreParam = GetDBRestoreParam();
                }
                else if (ConfigFileRadioBtn.Checked)
                {
                    dBRestoreParam = DBRestoreParam.NewDBRestoreParam(this.ConfigFileTextBox.Text);
                }

                List<string> strList = DBRestoreHelper.RestoreCleanupShrinkDB(dBRestoreParam);
                AddToResults(strList);
            }
            catch(Exception ex)
            {
                AddToResults(ex.Message);
                if (ex.InnerException != null)
                    AddToResults(ex.InnerException.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private DBRestoreParam GetDBRestoreParam()
        {
            string[] dbstoRestore = this.DBToRestoreTxtBox.Text.Split(',');
            List<string> usersToRestore = new List<string>(this.UsersToRestoreTxtBox.Text.Split(','));
            bool UseCleanupProc = this.CleanupTablesCheckbox.Checked;
            bool ShrinkDB = this.ShrinkDBCheckBox.Checked;
            bool DeleteBackups = this.DeleteBackupCheckBox.Checked;

            DBRestoreParam dBRestoreParam = DBRestoreParam.NewDBRestoreParam(this.NetworkPathTextBox.Text
                                                                            , this.DataPathTextBox.Text
                                                                            , this.LogPathTextBox.Text
                                                                            , this.BackupPathTxtBox.Text
                                                                            , dbstoRestore
                                                                            , this.ServerNameTxtBox.Text);
            dBRestoreParam.PostRestore.CleanupTables = UseCleanupProc;
            dBRestoreParam.PostRestore.CleanupProcName = this.CleanUpProcName.Text;
            dBRestoreParam.PostRestore.UsersToRestore = usersToRestore;
            dBRestoreParam.PostRestore.ShrinkDB = ShrinkDB;
            dBRestoreParam.PostRestore.DeleteLocalCopy = DeleteBackups;

            return dBRestoreParam;
        }

        private string [] GetCommaSeparatedListOfDBs()
        {
            if(!string.IsNullOrEmpty(DBToRestoreTxtBox.Text))
            {
                return DBToRestoreTxtBox.Text.Split(',');
            }
            return null;
        }

        private void RestoreProdToDBTestForm_Load(object sender, EventArgs e)
        {
            InitializetheForm();
        }

        private void AddToResults(string strMsg)
        {
            ResultTxtBox.Text = strMsg + System.Environment.NewLine + ResultTxtBox.Text;
        }

        private void AddToResults(List<string> listMsg)
        {
            foreach(string strMsg in listMsg)
                ResultTxtBox.Text = strMsg + System.Environment.NewLine + ResultTxtBox.Text;
        }


        private void DevResetBtn_Click(object sender, EventArgs e)
        {
            InitializetheForm(true);
        }

        private void RestBtn_Click(object sender, EventArgs e)
        {
            this.InitializetheForm();
        }

        private void FileOpenBtn_Click(object sender, EventArgs e)
        {
            using (var fbd = new OpenFileDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.FileName))
                {
                    this.ConfigFileTextBox.Text = fbd.FileName;
                    AddToResults("Config File used:" + this.ConfigFileTextBox.Text);
                }
            }
        }

        private void ConfigFileRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (EnterConfigRadioBtn.Checked)
            {
                this.ConfigFileGroupBox.Enabled = false;
                this.EnterConfigGroupBox.Enabled = true;
            }
            else
            {
                this.EnterConfigGroupBox.Enabled = false;
                this.ConfigFileGroupBox.Enabled = true;
            }
        }

        private void CleanupTablesCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (CleanupTablesCheckbox.Checked)
                this.CleanUpProcName.Enabled = true;
            else
                this.CleanUpProcName.Enabled = false;
        }
    }
}
