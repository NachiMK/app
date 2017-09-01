using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DBScriptGeneratorLibrary;

namespace DBScriptGeneratorUI
{
    public partial class DBScriptGeneratorForm : Form
    {
        public DBScriptGeneratorForm()
        {
            InitializeComponent();
        }

        private void btnGetDBs_Click(object sender, EventArgs e)
        {
            this.InitializeLstBoxDatabases();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            this.GenerateScripts();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            this.ResetForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private List<string> GetDatabases()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                DBScriptGenerator dBScriptGenerator = DBScriptGenerator.NewDBScriptGenerator(this.txtServerName.Text);
                List<string> lstDB = dBScriptGenerator.GetAllDatabases();
                return lstDB;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
}

        private void InitializeLstBoxDatabases()
        {
            chkLstBoxDatabases.Items.Clear();
            List<string> lstDB = GetDatabases();
            foreach (string strDb in lstDB)
            {
                this.chkLstBoxDatabases.Items.Add(strDb, false);
            }
            CheckSelectedDBs(GetRequiredDB());
            LogResult("Databases from Server was refreshed and required DBs were selected.");
        }

        private void InitializeCheckObjectsToScript()
        {
            chkObjectsToScript.Items.Clear();
            List<string> lstDB = DBScriptGenerator.AllowedListOfSQLObjectsForScripting();
            foreach (string strDb in lstDB)
            {
                this.chkObjectsToScript.Items.Add(strDb, true);
            }
            LogResult("Added allowed list of scripting objects and required DBs were selected.");
        }

        private void GenerateScripts()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                List<string> ScriptObjectList = GetSelectedObjectsToScript();
                DBScriptGenerator dBScriptGenerator = DBScriptGenerator.NewDBScriptGenerator(this.txtServerName.Text, ScriptObjectList);
                List<string> lstMsg = dBScriptGenerator.GenerateScript(GetSelectedDatabases(), txtOutputPath.Text);
                LogResult(lstMsg);
                LogResult("Scripts for selected DBs was generated and saved to output folder.");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void GenerateTransferScripts()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string strScript = string.Empty;
                List<string> ScriptObjectList = GetSelectedObjectsToScript();
                DBScriptGenerator dBScriptGenerator = DBScriptGenerator.NewDBScriptGenerator(this.txtServerName.Text, ScriptObjectList);
                string lstMsg = dBScriptGenerator.GenerateTransferScript(GetSelectedDatabases()[0], txtOutputPath.Text);
                LogResult(lstMsg);
                LogResult("Script returned by Transfer Script");
                LogResult(strScript);
                LogResult("Scripts for first DB in selected list was generated and saved to output folder.");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }


        private List<string> GetSelectedDatabases()
        {
            List<string> lstRet = new List<string>();

            foreach (object itemChecked in this.chkLstBoxDatabases.CheckedItems)
            {
                lstRet.Add(itemChecked.ToString());
            }

            return lstRet;
        }

        private List<string> GetSelectedObjectsToScript()
        {
            List<string> lstRet = new List<string>();

            foreach (object itemChecked in this.chkObjectsToScript.CheckedItems)
            {
                lstRet.Add(itemChecked.ToString());
            }

            return lstRet;
        }

        private void ResetForm()
        {
            this.txtOutputPath.Text = string.Empty;
            this.txtServerName.Text = "Prod1_DB";
            this.InitializeLstBoxDatabases();
            this.txtOutputPath.Text = @"C:\Users\NachiM\Documents\DBScripts\";
            this.rtbResults.Text = string.Empty;
            InitializeCheckObjectsToScript();
        }

        private void LogResult(string strMsg, bool append = true)
        {
            if (!append)
                this.rtbResults.Text = strMsg;
            else
                this.rtbResults.Text = strMsg + System.Environment.NewLine + this.rtbResults.Text;
        }

        private void LogResult(List<string> MsgList, bool append = true)
        {
            foreach (string s in MsgList)
                LogResult(s, append);
        }


        private void DBScriptGeneratorForm_Load(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnUnCheckDBList_Click(object sender, EventArgs e)
        {
            CheckUnCheckDBList(false);
            LogResult("All Databases unchecked.");
        }

        private void btnCheckDBList_Click(object sender, EventArgs e)
        {
            CheckUnCheckDBList();
            LogResult("All Databases checked.");
        }

        private void CheckUnCheckDBList(bool Check = true)
        {
            for (int i = 0; i < chkLstBoxDatabases.Items.Count; i++)
            {
                chkLstBoxDatabases.SetItemChecked(i, Check);
            }
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    this.txtOutputPath.Text = fbd.SelectedPath;
                    LogResult("Output Folder Path set to:" + this.txtOutputPath.Text);
                }
            }
        }

        private void btnCheckTest2DB_Click(object sender, EventArgs e)
        {
            List<string> lstTest2DB = new List<string>(10);
            lstTest2DB.Add("Logfile");
            lstTest2DB.Add("ColoServiceBrokerLog");
            lstTest2DB.Add("DBA");
            lstTest2DB.Add("DOMLog");
            lstTest2DB.Add("ASPState");
            lstTest2DB.Add("MiniSoft_LP_Products");
            lstTest2DB.Add("ASPState-PacificCoastLighting");
            lstTest2DB.Add("ASPState-OU");
            lstTest2DB.Add("CentennialDesks");
            lstTest2DB.Add("Imports");
            lstTest2DB.Add("Amazon");
            lstTest2DB.Add("Marketplace");
            lstTest2DB.Add("TempTables");
            lstTest2DB.Add("ContactUs");
            lstTest2DB.Add("Project");

            CheckSelectedDBs(lstTest2DB);
        }

        private void btnGenerateLogins_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                DBScriptGenerator dBScriptGenerator = DBScriptGenerator.NewDBScriptGenerator(this.txtServerName.Text);
                string lstMsg = dBScriptGenerator.GenerateServerLogins(txtOutputPath.Text);
                LogResult(lstMsg);
                LogResult("Login Scripts for selected DBs was generated and saved to output folder.");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnCheckReqDBs_Click(object sender, EventArgs e)
        {
            CheckSelectedDBs(GetRequiredDB());
        }

        private void CheckSelectedDBs(List<string> selectedDBList)
        {
            for (int i = 0; i < chkLstBoxDatabases.Items.Count; i++)
            {
                if (selectedDBList.Exists(se => se.ToUpper().Equals(chkLstBoxDatabases.Items[i].ToString().ToUpper())))
                    chkLstBoxDatabases.SetItemChecked(i, true);
                else
                    chkLstBoxDatabases.SetItemChecked(i, false);
            }
        }

        private List<string> GetRequiredDB()
        {
            List<string> lstReqDB = new List<string>(20);
            lstReqDB.Add("Amazon");
            lstReqDB.Add("ASPNetServices");
            lstReqDB.Add("ASPState");
            lstReqDB.Add("ASPState - PacificCoastLighting");
            lstReqDB.Add("Assets");
            lstReqDB.Add("Carteasy");
            lstReqDB.Add("CentennialDesks");
            lstReqDB.Add("ColoExternalActivation");
            lstReqDB.Add("ColoServiceBrokerLog");
            lstReqDB.Add("CommonDB");
            lstReqDB.Add("ContactUs");
            lstReqDB.Add("ContentManagement");
            lstReqDB.Add("Coupon");
            lstReqDB.Add("Customers");
            lstReqDB.Add("DomExportOrder");
            lstReqDB.Add("DOMLog");
            lstReqDB.Add("Imports");
            lstReqDB.Add("LampsPlusAspServices");
            lstReqDB.Add("Logfile");
            lstReqDB.Add("Marketplace");
            lstReqDB.Add("MiniSoft_LP_Products");
            lstReqDB.Add("OrderArchive");
            lstReqDB.Add("PCL");
            lstReqDB.Add("Private");
            lstReqDB.Add("Products");
            lstReqDB.Add("Project");
            lstReqDB.Add("StockCheck");
            lstReqDB.Add("TempTables");
            lstReqDB.Add("Universal");
            lstReqDB.Add("UserProfile");
            lstReqDB.Add("UserProfile_Universal");
            lstReqDB.Add("ZipCodes");
            return lstReqDB;
        }

        private void btnTransferScript_Click(object sender, EventArgs e)
        {
            //this.GenerateTransferScripts();
            MessageBox.Show("This is same as Generate Script.");
        }
    }
}
