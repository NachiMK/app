namespace RestoreSQLBackups
{
    partial class RestoreProdToDBTestForm
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
            this.EnterConfigGroupBox = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.UsersToRestoreTxtBox = new System.Windows.Forms.RichTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.DBToRestoreTxtBox = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.BackupPathTxtBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.LogPathTextBox = new System.Windows.Forms.TextBox();
            this.DataPathTextBox = new System.Windows.Forms.TextBox();
            this.NetworkPathTextBox = new System.Windows.Forms.TextBox();
            this.RestBtn = new System.Windows.Forms.Button();
            this.SubmitBtn = new System.Windows.Forms.Button();
            this.CloseBtn = new System.Windows.Forms.Button();
            this.ConfigFileGroupBox = new System.Windows.Forms.GroupBox();
            this.FileOpenBtn = new System.Windows.Forms.Button();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.ConfigFileTextBox = new System.Windows.Forms.TextBox();
            this.DevResetBtn = new System.Windows.Forms.Button();
            this.RestoreOptionGroupBox = new System.Windows.Forms.GroupBox();
            this.ConfigFileRadioBtn = new System.Windows.Forms.RadioButton();
            this.EnterConfigRadioBtn = new System.Windows.Forms.RadioButton();
            this.ResultTxtBox = new System.Windows.Forms.RichTextBox();
            this.ShrinkDBCheckBox = new System.Windows.Forms.CheckBox();
            this.DeleteBackupCheckBox = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ServerNameTxtBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.CleanUpProcName = new System.Windows.Forms.TextBox();
            this.CleanupTablesCheckbox = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.EnterConfigGroupBox.SuspendLayout();
            this.ConfigFileGroupBox.SuspendLayout();
            this.RestoreOptionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // EnterConfigGroupBox
            // 
            this.EnterConfigGroupBox.Controls.Add(this.label11);
            this.EnterConfigGroupBox.Controls.Add(this.CleanupTablesCheckbox);
            this.EnterConfigGroupBox.Controls.Add(this.label10);
            this.EnterConfigGroupBox.Controls.Add(this.CleanUpProcName);
            this.EnterConfigGroupBox.Controls.Add(this.label6);
            this.EnterConfigGroupBox.Controls.Add(this.ServerNameTxtBox);
            this.EnterConfigGroupBox.Controls.Add(this.DeleteBackupCheckBox);
            this.EnterConfigGroupBox.Controls.Add(this.ShrinkDBCheckBox);
            this.EnterConfigGroupBox.Controls.Add(this.label8);
            this.EnterConfigGroupBox.Controls.Add(this.UsersToRestoreTxtBox);
            this.EnterConfigGroupBox.Controls.Add(this.label7);
            this.EnterConfigGroupBox.Controls.Add(this.DBToRestoreTxtBox);
            this.EnterConfigGroupBox.Controls.Add(this.label5);
            this.EnterConfigGroupBox.Controls.Add(this.BackupPathTxtBox);
            this.EnterConfigGroupBox.Controls.Add(this.label4);
            this.EnterConfigGroupBox.Controls.Add(this.label3);
            this.EnterConfigGroupBox.Controls.Add(this.label2);
            this.EnterConfigGroupBox.Controls.Add(this.label1);
            this.EnterConfigGroupBox.Controls.Add(this.LogPathTextBox);
            this.EnterConfigGroupBox.Controls.Add(this.DataPathTextBox);
            this.EnterConfigGroupBox.Controls.Add(this.NetworkPathTextBox);
            this.EnterConfigGroupBox.Location = new System.Drawing.Point(3, 92);
            this.EnterConfigGroupBox.Name = "EnterConfigGroupBox";
            this.EnterConfigGroupBox.Size = new System.Drawing.Size(560, 323);
            this.EnterConfigGroupBox.TabIndex = 0;
            this.EnterConfigGroupBox.TabStop = false;
            this.EnterConfigGroupBox.Text = "Enter Config";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(11, 229);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 17);
            this.label8.TabIndex = 47;
            this.label8.Text = "To Restore";
            // 
            // UsersToRestoreTxtBox
            // 
            this.UsersToRestoreTxtBox.Location = new System.Drawing.Point(116, 212);
            this.UsersToRestoreTxtBox.MaxLength = 4000;
            this.UsersToRestoreTxtBox.Name = "UsersToRestoreTxtBox";
            this.UsersToRestoreTxtBox.Size = new System.Drawing.Size(439, 53);
            this.UsersToRestoreTxtBox.TabIndex = 46;
            this.UsersToRestoreTxtBox.Text = "";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(10, 212);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 17);
            this.label7.TabIndex = 45;
            this.label7.Text = "Users";
            // 
            // DBToRestoreTxtBox
            // 
            this.DBToRestoreTxtBox.Location = new System.Drawing.Point(116, 142);
            this.DBToRestoreTxtBox.MaxLength = 4000;
            this.DBToRestoreTxtBox.Name = "DBToRestoreTxtBox";
            this.DBToRestoreTxtBox.Size = new System.Drawing.Size(439, 53);
            this.DBToRestoreTxtBox.TabIndex = 44;
            this.DBToRestoreTxtBox.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(11, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 17);
            this.label5.TabIndex = 36;
            this.label5.Text = "Backup Path";
            // 
            // BackupPathTxtBox
            // 
            this.BackupPathTxtBox.Location = new System.Drawing.Point(116, 64);
            this.BackupPathTxtBox.Name = "BackupPathTxtBox";
            this.BackupPathTxtBox.Size = new System.Drawing.Size(439, 20);
            this.BackupPathTxtBox.TabIndex = 35;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(11, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 17);
            this.label4.TabIndex = 32;
            this.label4.Text = "Restore DBs";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(11, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 17);
            this.label3.TabIndex = 31;
            this.label3.Text = "Data Path";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(11, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 17);
            this.label2.TabIndex = 30;
            this.label2.Text = "Log Path";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 17);
            this.label1.TabIndex = 29;
            this.label1.Text = "Network Path";
            // 
            // LogPathTextBox
            // 
            this.LogPathTextBox.Location = new System.Drawing.Point(116, 115);
            this.LogPathTextBox.Name = "LogPathTextBox";
            this.LogPathTextBox.Size = new System.Drawing.Size(439, 20);
            this.LogPathTextBox.TabIndex = 28;
            // 
            // DataPathTextBox
            // 
            this.DataPathTextBox.Location = new System.Drawing.Point(116, 89);
            this.DataPathTextBox.Name = "DataPathTextBox";
            this.DataPathTextBox.Size = new System.Drawing.Size(439, 20);
            this.DataPathTextBox.TabIndex = 27;
            // 
            // NetworkPathTextBox
            // 
            this.NetworkPathTextBox.Location = new System.Drawing.Point(116, 13);
            this.NetworkPathTextBox.Name = "NetworkPathTextBox";
            this.NetworkPathTextBox.Size = new System.Drawing.Size(439, 20);
            this.NetworkPathTextBox.TabIndex = 26;
            // 
            // RestBtn
            // 
            this.RestBtn.Location = new System.Drawing.Point(357, 618);
            this.RestBtn.Name = "RestBtn";
            this.RestBtn.Size = new System.Drawing.Size(69, 23);
            this.RestBtn.TabIndex = 40;
            this.RestBtn.Text = "Reset";
            this.RestBtn.UseVisualStyleBackColor = true;
            this.RestBtn.Click += new System.EventHandler(this.RestBtn_Click);
            // 
            // SubmitBtn
            // 
            this.SubmitBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.SubmitBtn.Location = new System.Drawing.Point(432, 618);
            this.SubmitBtn.Name = "SubmitBtn";
            this.SubmitBtn.Size = new System.Drawing.Size(62, 23);
            this.SubmitBtn.TabIndex = 36;
            this.SubmitBtn.Text = "Restore";
            this.SubmitBtn.UseVisualStyleBackColor = true;
            this.SubmitBtn.Click += new System.EventHandler(this.SubmitBtn_Click);
            // 
            // CloseBtn
            // 
            this.CloseBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseBtn.Location = new System.Drawing.Point(500, 618);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(63, 23);
            this.CloseBtn.TabIndex = 35;
            this.CloseBtn.Text = "Close";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ConfigFileGroupBox
            // 
            this.ConfigFileGroupBox.Controls.Add(this.FileOpenBtn);
            this.ConfigFileGroupBox.Controls.Add(this.lblFilePath);
            this.ConfigFileGroupBox.Controls.Add(this.ConfigFileTextBox);
            this.ConfigFileGroupBox.Location = new System.Drawing.Point(3, 43);
            this.ConfigFileGroupBox.Name = "ConfigFileGroupBox";
            this.ConfigFileGroupBox.Size = new System.Drawing.Size(560, 44);
            this.ConfigFileGroupBox.TabIndex = 37;
            this.ConfigFileGroupBox.TabStop = false;
            this.ConfigFileGroupBox.Text = "Config File";
            // 
            // FileOpenBtn
            // 
            this.FileOpenBtn.Location = new System.Drawing.Point(534, 20);
            this.FileOpenBtn.Name = "FileOpenBtn";
            this.FileOpenBtn.Size = new System.Drawing.Size(21, 19);
            this.FileOpenBtn.TabIndex = 32;
            this.FileOpenBtn.Text = "...";
            this.FileOpenBtn.UseVisualStyleBackColor = true;
            this.FileOpenBtn.Click += new System.EventHandler(this.FileOpenBtn_Click);
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilePath.Location = new System.Drawing.Point(10, 19);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(72, 17);
            this.lblFilePath.TabIndex = 31;
            this.lblFilePath.Text = "File Path";
            // 
            // ConfigFileTextBox
            // 
            this.ConfigFileTextBox.Location = new System.Drawing.Point(88, 19);
            this.ConfigFileTextBox.Name = "ConfigFileTextBox";
            this.ConfigFileTextBox.Size = new System.Drawing.Size(439, 20);
            this.ConfigFileTextBox.TabIndex = 30;
            // 
            // DevResetBtn
            // 
            this.DevResetBtn.Location = new System.Drawing.Point(282, 618);
            this.DevResetBtn.Name = "DevResetBtn";
            this.DevResetBtn.Size = new System.Drawing.Size(69, 23);
            this.DevResetBtn.TabIndex = 41;
            this.DevResetBtn.Text = "Dev Reset";
            this.DevResetBtn.UseVisualStyleBackColor = true;
            this.DevResetBtn.Click += new System.EventHandler(this.DevResetBtn_Click);
            // 
            // RestoreOptionGroupBox
            // 
            this.RestoreOptionGroupBox.Controls.Add(this.EnterConfigRadioBtn);
            this.RestoreOptionGroupBox.Controls.Add(this.ConfigFileRadioBtn);
            this.RestoreOptionGroupBox.Location = new System.Drawing.Point(3, 5);
            this.RestoreOptionGroupBox.Name = "RestoreOptionGroupBox";
            this.RestoreOptionGroupBox.Size = new System.Drawing.Size(560, 33);
            this.RestoreOptionGroupBox.TabIndex = 42;
            this.RestoreOptionGroupBox.TabStop = false;
            this.RestoreOptionGroupBox.Text = "Restore Option";
            // 
            // ConfigFileRadioBtn
            // 
            this.ConfigFileRadioBtn.AutoSize = true;
            this.ConfigFileRadioBtn.Location = new System.Drawing.Point(6, 15);
            this.ConfigFileRadioBtn.Name = "ConfigFileRadioBtn";
            this.ConfigFileRadioBtn.Size = new System.Drawing.Size(96, 17);
            this.ConfigFileRadioBtn.TabIndex = 0;
            this.ConfigFileRadioBtn.TabStop = true;
            this.ConfigFileRadioBtn.Text = "Use Config File";
            this.ConfigFileRadioBtn.UseVisualStyleBackColor = true;
            this.ConfigFileRadioBtn.CheckedChanged += new System.EventHandler(this.ConfigFileRadioBtn_CheckedChanged);
            // 
            // EnterConfigRadioBtn
            // 
            this.EnterConfigRadioBtn.AutoSize = true;
            this.EnterConfigRadioBtn.Location = new System.Drawing.Point(108, 15);
            this.EnterConfigRadioBtn.Name = "EnterConfigRadioBtn";
            this.EnterConfigRadioBtn.Size = new System.Drawing.Size(83, 17);
            this.EnterConfigRadioBtn.TabIndex = 1;
            this.EnterConfigRadioBtn.TabStop = true;
            this.EnterConfigRadioBtn.Text = "Enter Config";
            this.EnterConfigRadioBtn.UseVisualStyleBackColor = true;
            // 
            // ResultTxtBox
            // 
            this.ResultTxtBox.Location = new System.Drawing.Point(3, 421);
            this.ResultTxtBox.Name = "ResultTxtBox";
            this.ResultTxtBox.Size = new System.Drawing.Size(560, 191);
            this.ResultTxtBox.TabIndex = 43;
            this.ResultTxtBox.Text = "";
            // 
            // ShrinkDBCheckBox
            // 
            this.ShrinkDBCheckBox.AutoSize = true;
            this.ShrinkDBCheckBox.Checked = true;
            this.ShrinkDBCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShrinkDBCheckBox.Location = new System.Drawing.Point(117, 271);
            this.ShrinkDBCheckBox.Name = "ShrinkDBCheckBox";
            this.ShrinkDBCheckBox.Size = new System.Drawing.Size(110, 17);
            this.ShrinkDBCheckBox.TabIndex = 50;
            this.ShrinkDBCheckBox.Text = "Shrink Databases";
            this.ShrinkDBCheckBox.UseVisualStyleBackColor = true;
            // 
            // DeleteBackupCheckBox
            // 
            this.DeleteBackupCheckBox.AutoSize = true;
            this.DeleteBackupCheckBox.Checked = true;
            this.DeleteBackupCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DeleteBackupCheckBox.Location = new System.Drawing.Point(233, 271);
            this.DeleteBackupCheckBox.Name = "DeleteBackupCheckBox";
            this.DeleteBackupCheckBox.Size = new System.Drawing.Size(102, 17);
            this.DeleteBackupCheckBox.TabIndex = 51;
            this.DeleteBackupCheckBox.Text = "Delete Backups";
            this.DeleteBackupCheckBox.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(11, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 17);
            this.label6.TabIndex = 53;
            this.label6.Text = "Connection:";
            // 
            // ServerNameTxtBox
            // 
            this.ServerNameTxtBox.Location = new System.Drawing.Point(116, 39);
            this.ServerNameTxtBox.Name = "ServerNameTxtBox";
            this.ServerNameTxtBox.Size = new System.Drawing.Size(439, 20);
            this.ServerNameTxtBox.TabIndex = 52;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(12, 294);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(87, 17);
            this.label10.TabIndex = 55;
            this.label10.Text = "Proc Name";
            // 
            // CleanUpProcName
            // 
            this.CleanUpProcName.Location = new System.Drawing.Point(116, 294);
            this.CleanUpProcName.Name = "CleanUpProcName";
            this.CleanUpProcName.Size = new System.Drawing.Size(439, 20);
            this.CleanUpProcName.TabIndex = 54;
            // 
            // CleanupTablesCheckbox
            // 
            this.CleanupTablesCheckbox.AutoSize = true;
            this.CleanupTablesCheckbox.Location = new System.Drawing.Point(341, 271);
            this.CleanupTablesCheckbox.Name = "CleanupTablesCheckbox";
            this.CleanupTablesCheckbox.Size = new System.Drawing.Size(100, 17);
            this.CleanupTablesCheckbox.TabIndex = 56;
            this.CleanupTablesCheckbox.Text = "Cleanup Tables";
            this.CleanupTablesCheckbox.UseVisualStyleBackColor = true;
            this.CleanupTablesCheckbox.CheckedChanged += new System.EventHandler(this.CleanupTablesCheckbox_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(10, 269);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(102, 17);
            this.label11.TabIndex = 57;
            this.label11.Text = "Post Restore";
            // 
            // RestoreProdToDBTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 649);
            this.ControlBox = false;
            this.Controls.Add(this.ResultTxtBox);
            this.Controls.Add(this.RestoreOptionGroupBox);
            this.Controls.Add(this.DevResetBtn);
            this.Controls.Add(this.ConfigFileGroupBox);
            this.Controls.Add(this.SubmitBtn);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.EnterConfigGroupBox);
            this.Controls.Add(this.RestBtn);
            this.MaximizeBox = false;
            this.Name = "RestoreProdToDBTestForm";
            this.Text = "Restore SQL Backups";
            this.Load += new System.EventHandler(this.RestoreProdToDBTestForm_Load);
            this.EnterConfigGroupBox.ResumeLayout(false);
            this.EnterConfigGroupBox.PerformLayout();
            this.ConfigFileGroupBox.ResumeLayout(false);
            this.ConfigFileGroupBox.PerformLayout();
            this.RestoreOptionGroupBox.ResumeLayout(false);
            this.RestoreOptionGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox EnterConfigGroupBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RichTextBox UsersToRestoreTxtBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RichTextBox DBToRestoreTxtBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox BackupPathTxtBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox LogPathTextBox;
        private System.Windows.Forms.TextBox DataPathTextBox;
        private System.Windows.Forms.TextBox NetworkPathTextBox;
        private System.Windows.Forms.Button RestBtn;
        private System.Windows.Forms.Button SubmitBtn;
        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.GroupBox ConfigFileGroupBox;
        private System.Windows.Forms.Button FileOpenBtn;
        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.TextBox ConfigFileTextBox;
        private System.Windows.Forms.Button DevResetBtn;
        private System.Windows.Forms.GroupBox RestoreOptionGroupBox;
        private System.Windows.Forms.RadioButton EnterConfigRadioBtn;
        private System.Windows.Forms.RadioButton ConfigFileRadioBtn;
        private System.Windows.Forms.RichTextBox ResultTxtBox;
        private System.Windows.Forms.CheckBox DeleteBackupCheckBox;
        private System.Windows.Forms.CheckBox ShrinkDBCheckBox;
        private System.Windows.Forms.CheckBox CleanupTablesCheckbox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox CleanUpProcName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ServerNameTxtBox;
        private System.Windows.Forms.Label label11;
    }
}

