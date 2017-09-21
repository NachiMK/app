namespace RestoreProd_DBTest2
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
            this.btnClose = new System.Windows.Forms.Button();
            this.NetworkPathTextBox = new System.Windows.Forms.TextBox();
            this.DataPathTextBox = new System.Windows.Forms.TextBox();
            this.LogPathTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ResultTxtBox = new System.Windows.Forms.RichTextBox();
            this.SubmitBtn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.BackupPathTxtBox = new System.Windows.Forms.TextBox();
            this.RestoreDBListBox = new System.Windows.Forms.CheckedListBox();
            this.CheckAllBtn = new System.Windows.Forms.Button();
            this.UnCheckAllBtn = new System.Windows.Forms.Button();
            this.DevResetBtn = new System.Windows.Forms.Button();
            this.RestBtn = new System.Windows.Forms.Button();
            this.ConfirmChkBox = new System.Windows.Forms.CheckBox();
            this.ServerNameTxtBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(470, 429);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // NetworkPathTextBox
            // 
            this.NetworkPathTextBox.Location = new System.Drawing.Point(106, 12);
            this.NetworkPathTextBox.Name = "NetworkPathTextBox";
            this.NetworkPathTextBox.Size = new System.Drawing.Size(439, 20);
            this.NetworkPathTextBox.TabIndex = 1;
            // 
            // DataPathTextBox
            // 
            this.DataPathTextBox.Location = new System.Drawing.Point(106, 61);
            this.DataPathTextBox.Name = "DataPathTextBox";
            this.DataPathTextBox.Size = new System.Drawing.Size(439, 20);
            this.DataPathTextBox.TabIndex = 2;
            // 
            // LogPathTextBox
            // 
            this.LogPathTextBox.Location = new System.Drawing.Point(106, 87);
            this.LogPathTextBox.Name = "LogPathTextBox";
            this.LogPathTextBox.Size = new System.Drawing.Size(439, 20);
            this.LogPathTextBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Network Path";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Log Path";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Data Path";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(1, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Restore DBs";
            // 
            // ResultTxtBox
            // 
            this.ResultTxtBox.Location = new System.Drawing.Point(4, 243);
            this.ResultTxtBox.Name = "ResultTxtBox";
            this.ResultTxtBox.Size = new System.Drawing.Size(541, 182);
            this.ResultTxtBox.TabIndex = 9;
            this.ResultTxtBox.Text = "";
            // 
            // SubmitBtn
            // 
            this.SubmitBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.SubmitBtn.Location = new System.Drawing.Point(389, 429);
            this.SubmitBtn.Name = "SubmitBtn";
            this.SubmitBtn.Size = new System.Drawing.Size(75, 23);
            this.SubmitBtn.TabIndex = 10;
            this.SubmitBtn.Text = "Restore";
            this.SubmitBtn.UseVisualStyleBackColor = true;
            this.SubmitBtn.Click += new System.EventHandler(this.SubmitBtn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(1, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 17);
            this.label5.TabIndex = 12;
            this.label5.Text = "Backup Path";
            // 
            // BackupPathTxtBox
            // 
            this.BackupPathTxtBox.Location = new System.Drawing.Point(106, 36);
            this.BackupPathTxtBox.Name = "BackupPathTxtBox";
            this.BackupPathTxtBox.Size = new System.Drawing.Size(439, 20);
            this.BackupPathTxtBox.TabIndex = 11;
            // 
            // RestoreDBListBox
            // 
            this.RestoreDBListBox.CheckOnClick = true;
            this.RestoreDBListBox.FormattingEnabled = true;
            this.RestoreDBListBox.Location = new System.Drawing.Point(106, 114);
            this.RestoreDBListBox.Name = "RestoreDBListBox";
            this.RestoreDBListBox.Size = new System.Drawing.Size(339, 124);
            this.RestoreDBListBox.TabIndex = 13;
            // 
            // CheckAllBtn
            // 
            this.CheckAllBtn.Location = new System.Drawing.Point(452, 114);
            this.CheckAllBtn.Name = "CheckAllBtn";
            this.CheckAllBtn.Size = new System.Drawing.Size(75, 23);
            this.CheckAllBtn.TabIndex = 14;
            this.CheckAllBtn.Text = "Check All";
            this.CheckAllBtn.UseVisualStyleBackColor = true;
            this.CheckAllBtn.Click += new System.EventHandler(this.CheckAllBtn_Click);
            // 
            // UnCheckAllBtn
            // 
            this.UnCheckAllBtn.Location = new System.Drawing.Point(452, 144);
            this.UnCheckAllBtn.Name = "UnCheckAllBtn";
            this.UnCheckAllBtn.Size = new System.Drawing.Size(75, 23);
            this.UnCheckAllBtn.TabIndex = 15;
            this.UnCheckAllBtn.Text = "UnCheck All";
            this.UnCheckAllBtn.UseVisualStyleBackColor = true;
            this.UnCheckAllBtn.Click += new System.EventHandler(this.UnCheckAllBtn_Click);
            // 
            // DevResetBtn
            // 
            this.DevResetBtn.Location = new System.Drawing.Point(452, 174);
            this.DevResetBtn.Name = "DevResetBtn";
            this.DevResetBtn.Size = new System.Drawing.Size(75, 23);
            this.DevResetBtn.TabIndex = 16;
            this.DevResetBtn.Text = "Dev Reset";
            this.DevResetBtn.UseVisualStyleBackColor = true;
            this.DevResetBtn.Click += new System.EventHandler(this.DevResetBtn_Click);
            // 
            // RestBtn
            // 
            this.RestBtn.Location = new System.Drawing.Point(452, 204);
            this.RestBtn.Name = "RestBtn";
            this.RestBtn.Size = new System.Drawing.Size(75, 23);
            this.RestBtn.TabIndex = 17;
            this.RestBtn.Text = "Reset";
            this.RestBtn.UseVisualStyleBackColor = true;
            this.RestBtn.Click += new System.EventHandler(this.RestBtn_Click);
            // 
            // ConfirmChkBox
            // 
            this.ConfirmChkBox.AutoSize = true;
            this.ConfirmChkBox.Location = new System.Drawing.Point(234, 433);
            this.ConfirmChkBox.Name = "ConfirmChkBox";
            this.ConfirmChkBox.Size = new System.Drawing.Size(149, 17);
            this.ConfirmChkBox.TabIndex = 18;
            this.ConfirmChkBox.Text = "Confirm for each Restore?";
            this.ConfirmChkBox.UseVisualStyleBackColor = true;
            // 
            // ServerNameTxtBox
            // 
            this.ServerNameTxtBox.Location = new System.Drawing.Point(106, 429);
            this.ServerNameTxtBox.Name = "ServerNameTxtBox";
            this.ServerNameTxtBox.Size = new System.Drawing.Size(122, 20);
            this.ServerNameTxtBox.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(1, 429);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 17);
            this.label6.TabIndex = 20;
            this.label6.Text = "DB Server:";
            // 
            // RestoreProdToDBTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(550, 455);
            this.ControlBox = false;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ServerNameTxtBox);
            this.Controls.Add(this.ConfirmChkBox);
            this.Controls.Add(this.RestBtn);
            this.Controls.Add(this.DevResetBtn);
            this.Controls.Add(this.UnCheckAllBtn);
            this.Controls.Add(this.CheckAllBtn);
            this.Controls.Add(this.RestoreDBListBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.BackupPathTxtBox);
            this.Controls.Add(this.SubmitBtn);
            this.Controls.Add(this.ResultTxtBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LogPathTextBox);
            this.Controls.Add(this.DataPathTextBox);
            this.Controls.Add(this.NetworkPathTextBox);
            this.Controls.Add(this.btnClose);
            this.MaximizeBox = false;
            this.Name = "RestoreProdToDBTestForm";
            this.Text = "Restore Prod to DBTest2";
            this.Load += new System.EventHandler(this.RestoreProdToDBTestForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox NetworkPathTextBox;
        private System.Windows.Forms.TextBox DataPathTextBox;
        private System.Windows.Forms.TextBox LogPathTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox ResultTxtBox;
        private System.Windows.Forms.Button SubmitBtn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox BackupPathTxtBox;
        private System.Windows.Forms.CheckedListBox RestoreDBListBox;
        private System.Windows.Forms.Button CheckAllBtn;
        private System.Windows.Forms.Button UnCheckAllBtn;
        private System.Windows.Forms.Button DevResetBtn;
        private System.Windows.Forms.Button RestBtn;
        private System.Windows.Forms.CheckBox ConfirmChkBox;
        private System.Windows.Forms.TextBox ServerNameTxtBox;
        private System.Windows.Forms.Label label6;
    }
}

