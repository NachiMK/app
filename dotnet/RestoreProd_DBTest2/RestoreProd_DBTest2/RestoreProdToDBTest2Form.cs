using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RestoreProd_DBTest2
{
    public partial class RestoreProdToDBTestForm : Form
    {
        public RestoreProdToDBTestForm()
        {
            InitializeComponent();
        }

        private void InitializetheForm()
        {
            this.NetworkPathTextBox.Text = @"\\676179-DEVSQL\PROD_DB_Backup\739781-SQLCLUS\";
            this.BackupPathTxtBox.Text = @"D:\SQL_BACKUP";
            this.DataPathTextBox.Text = @"D:\SQL_DATA";
            this.LogPathTextBox.Text = @"D:\SQL_LOG";
            this.ServerNameTxtBox.Text = "Prod1_DBTest2";
            this.ResultTxtBox.Text = "";
            this.ResultTxtBox.ReadOnly = true;
            AddDBAndSetState(true);
        }

        private void AddDBAndSetState(bool onOrOff, bool addOptionalDBs = true)
        {
            this.RestoreDBListBox.Items.Clear();
            this.RestoreDBListBox.Items.AddRange(DBRestoreHelper.DatabaseToRestore().ToArray());
            for (int intIdx = 0; intIdx < this.RestoreDBListBox.Items.Count; intIdx++)
                this.RestoreDBListBox.SetItemChecked(intIdx, onOrOff);
            if (addOptionalDBs)
                this.RestoreDBListBox.Items.AddRange(DBRestoreHelper.OptionalDBToRestore().ToArray());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SubmitBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                foreach (object chekedItem in RestoreDBListBox.CheckedItems)
                {
                    List<string> strList = DBRestoreHelper.RestoreCleanupShrinkDB(this.NetworkPathTextBox.Text
                                                                                , this.DataPathTextBox.Text
                                                                                , this.LogPathTextBox.Text
                                                                                , this.BackupPathTxtBox.Text
                                                                                , chekedItem.ToString()
                                                                                , this.ServerNameTxtBox.Text);
                    AddToResults(strList);

                    if (ConfirmChkBox.Checked)
                    {
                        DialogResult dr = MessageBox.Show("Continue with Restore of Remaining Databases?", "Continue?"
                            , MessageBoxButtons.YesNo
                            , MessageBoxIcon.Question
                            , MessageBoxDefaultButton.Button1);

                        if (dr == DialogResult.Yes)
                            continue;
                        else
                            break;
                    }

                }
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void CheckAllBtn_Click(object sender, EventArgs e)
        {
            AddDBAndSetState(true, true);
        }

        private void UnCheckAllBtn_Click(object sender, EventArgs e)
        {
            AddDBAndSetState(false);
        }

        private void DevResetBtn_Click(object sender, EventArgs e)
        {
            this.NetworkPathTextBox.Text = @"\\lpvmdata\WebDevData\Data\NachiM";
            this.BackupPathTxtBox.Text = @"C:\SQL\SQL_BACKUP";
            this.DataPathTextBox.Text = @"C:\SQL\SQL_DATA";
            this.LogPathTextBox.Text = @"C:\SQL\SQL_LOG";
            this.ServerNameTxtBox.Text = "localhost";
            AddDBAndSetState(false, false);
        }

        private void RestBtn_Click(object sender, EventArgs e)
        {
            this.InitializetheForm();
        }
    }
}
