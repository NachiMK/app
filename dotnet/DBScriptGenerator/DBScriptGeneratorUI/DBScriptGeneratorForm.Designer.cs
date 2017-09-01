namespace DBScriptGeneratorUI
{
    partial class DBScriptGeneratorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnClose = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.btnGetDBs = new System.Windows.Forms.Button();
            this.rtbResults = new System.Windows.Forms.RichTextBox();
            this.lblDBNames = new System.Windows.Forms.Label();
            this.chkLstBoxDatabases = new System.Windows.Forms.CheckedListBox();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblResults = new System.Windows.Forms.Label();
            this.btnUnCheckDBList = new System.Windows.Forms.Button();
            this.btnCheckDBList = new System.Windows.Forms.Button();
            this.fbDialogOutputFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.btnCheckTest2DB = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkObjectsToScript = new System.Windows.Forms.CheckedListBox();
            this.btnGenerateLogins = new System.Windows.Forms.Button();
            this.btnCheckReqDBs = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(648, 540);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(566, 540);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 15;
            this.btnReset.Text = "&Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(485, 169);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(113, 23);
            this.btnGenerate.TabIndex = 12;
            this.btnGenerate.Text = "&Generate Script";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server Name :";
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(135, 8);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(198, 20);
            this.txtServerName.TabIndex = 1;
            // 
            // btnGetDBs
            // 
            this.btnGetDBs.Location = new System.Drawing.Point(339, 8);
            this.btnGetDBs.Name = "btnGetDBs";
            this.btnGetDBs.Size = new System.Drawing.Size(119, 23);
            this.btnGetDBs.TabIndex = 2;
            this.btnGetDBs.Text = "Get Databases";
            this.btnGetDBs.UseVisualStyleBackColor = true;
            this.btnGetDBs.Click += new System.EventHandler(this.btnGetDBs_Click);
            // 
            // rtbResults
            // 
            this.rtbResults.Location = new System.Drawing.Point(4, 220);
            this.rtbResults.Name = "rtbResults";
            this.rtbResults.ReadOnly = true;
            this.rtbResults.Size = new System.Drawing.Size(719, 314);
            this.rtbResults.TabIndex = 14;
            this.rtbResults.Text = "";
            // 
            // lblDBNames
            // 
            this.lblDBNames.AutoSize = true;
            this.lblDBNames.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDBNames.Location = new System.Drawing.Point(1, 42);
            this.lblDBNames.Name = "lblDBNames";
            this.lblDBNames.Size = new System.Drawing.Size(128, 15);
            this.lblDBNames.TabIndex = 3;
            this.lblDBNames.Text = "Choose Databases";
            // 
            // chkLstBoxDatabases
            // 
            this.chkLstBoxDatabases.FormattingEnabled = true;
            this.chkLstBoxDatabases.Location = new System.Drawing.Point(135, 42);
            this.chkLstBoxDatabases.Name = "chkLstBoxDatabases";
            this.chkLstBoxDatabases.Size = new System.Drawing.Size(198, 124);
            this.chkLstBoxDatabases.TabIndex = 4;
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Location = new System.Drawing.Point(135, 172);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(198, 20);
            this.txtOutputPath.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1, 173);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "Output Path :";
            // 
            // lblResults
            // 
            this.lblResults.AutoSize = true;
            this.lblResults.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResults.Location = new System.Drawing.Point(1, 202);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(59, 15);
            this.lblResults.TabIndex = 13;
            this.lblResults.Text = "Results:";
            // 
            // btnUnCheckDBList
            // 
            this.btnUnCheckDBList.Location = new System.Drawing.Point(340, 42);
            this.btnUnCheckDBList.Name = "btnUnCheckDBList";
            this.btnUnCheckDBList.Size = new System.Drawing.Size(118, 23);
            this.btnUnCheckDBList.TabIndex = 9;
            this.btnUnCheckDBList.Text = "UnCheck All";
            this.btnUnCheckDBList.UseVisualStyleBackColor = true;
            this.btnUnCheckDBList.Click += new System.EventHandler(this.btnUnCheckDBList_Click);
            // 
            // btnCheckDBList
            // 
            this.btnCheckDBList.Location = new System.Drawing.Point(340, 72);
            this.btnCheckDBList.Name = "btnCheckDBList";
            this.btnCheckDBList.Size = new System.Drawing.Size(118, 23);
            this.btnCheckDBList.TabIndex = 10;
            this.btnCheckDBList.Text = "Check All";
            this.btnCheckDBList.UseVisualStyleBackColor = true;
            this.btnCheckDBList.Click += new System.EventHandler(this.btnCheckDBList_Click);
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(339, 169);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(25, 23);
            this.btnSelectFolder.TabIndex = 8;
            this.btnSelectFolder.Text = "...";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // btnCheckTest2DB
            // 
            this.btnCheckTest2DB.Location = new System.Drawing.Point(340, 102);
            this.btnCheckTest2DB.Name = "btnCheckTest2DB";
            this.btnCheckTest2DB.Size = new System.Drawing.Size(118, 23);
            this.btnCheckTest2DB.TabIndex = 11;
            this.btnCheckTest2DB.Text = "Check Test2 DBs Only";
            this.btnCheckTest2DB.UseVisualStyleBackColor = true;
            this.btnCheckTest2DB.Click += new System.EventHandler(this.btnCheckTest2DB_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkObjectsToScript);
            this.groupBox1.Location = new System.Drawing.Point(485, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 154);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Objects to Script";
            // 
            // chkObjectsToScript
            // 
            this.chkObjectsToScript.FormattingEnabled = true;
            this.chkObjectsToScript.Items.AddRange(new object[] {
            "Schemas",
            "Types",
            "Tables",
            "Synonym & Views",
            "Procedures",
            "Functions",
            "Users",
            "Server Logins"});
            this.chkObjectsToScript.Location = new System.Drawing.Point(7, 20);
            this.chkObjectsToScript.Name = "chkObjectsToScript";
            this.chkObjectsToScript.Size = new System.Drawing.Size(219, 124);
            this.chkObjectsToScript.TabIndex = 0;
            // 
            // btnGenerateLogins
            // 
            this.btnGenerateLogins.Location = new System.Drawing.Point(602, 170);
            this.btnGenerateLogins.Name = "btnGenerateLogins";
            this.btnGenerateLogins.Size = new System.Drawing.Size(112, 23);
            this.btnGenerateLogins.TabIndex = 17;
            this.btnGenerateLogins.Text = "Generate Logins";
            this.btnGenerateLogins.UseVisualStyleBackColor = true;
            this.btnGenerateLogins.Click += new System.EventHandler(this.btnGenerateLogins_Click);
            // 
            // btnCheckReqDBs
            // 
            this.btnCheckReqDBs.Location = new System.Drawing.Point(340, 132);
            this.btnCheckReqDBs.Name = "btnCheckReqDBs";
            this.btnCheckReqDBs.Size = new System.Drawing.Size(118, 23);
            this.btnCheckReqDBs.TabIndex = 18;
            this.btnCheckReqDBs.Text = "Required DBs Only";
            this.btnCheckReqDBs.UseVisualStyleBackColor = true;
            this.btnCheckReqDBs.Click += new System.EventHandler(this.btnCheckReqDBs_Click);
            // 
            // DBScriptGeneratorForm
            // 
            this.AcceptButton = this.btnGenerate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(729, 567);
            this.Controls.Add(this.btnCheckReqDBs);
            this.Controls.Add(this.btnGenerateLogins);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCheckTest2DB);
            this.Controls.Add(this.btnSelectFolder);
            this.Controls.Add(this.btnCheckDBList);
            this.Controls.Add(this.btnUnCheckDBList);
            this.Controls.Add(this.lblResults);
            this.Controls.Add(this.txtOutputPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkLstBoxDatabases);
            this.Controls.Add(this.lblDBNames);
            this.Controls.Add(this.rtbResults);
            this.Controls.Add(this.btnGetDBs);
            this.Controls.Add(this.txtServerName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DBScriptGeneratorForm";
            this.ShowIcon = false;
            this.Text = "DB Script Generator";
            this.Load += new System.EventHandler(this.DBScriptGeneratorForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtServerName;
        private System.Windows.Forms.Button btnGetDBs;
        private System.Windows.Forms.RichTextBox rtbResults;
        private System.Windows.Forms.Label lblDBNames;
        private System.Windows.Forms.CheckedListBox chkLstBoxDatabases;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblResults;
        private System.Windows.Forms.Button btnUnCheckDBList;
        private System.Windows.Forms.Button btnCheckDBList;
        private System.Windows.Forms.FolderBrowserDialog fbDialogOutputFolder;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.Button btnCheckTest2DB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox chkObjectsToScript;
        private System.Windows.Forms.Button btnGenerateLogins;
        private System.Windows.Forms.Button btnCheckReqDBs;
    }
}

