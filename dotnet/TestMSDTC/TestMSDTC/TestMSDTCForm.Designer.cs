namespace TestMSDTC
{
    partial class TestMSDTCForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ConnStrTxtBox1 = new System.Windows.Forms.TextBox();
            this.ConnStrTxtBox2 = new System.Windows.Forms.TextBox();
            this.ResultsTxtBox = new System.Windows.Forms.RichTextBox();
            this.TestBtn = new System.Windows.Forms.Button();
            this.QueryTxt = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(2, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Connection String 1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(2, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Connection String 2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(4, 187);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Results";
            // 
            // ConnStrTxtBox1
            // 
            this.ConnStrTxtBox1.Location = new System.Drawing.Point(166, 6);
            this.ConnStrTxtBox1.Name = "ConnStrTxtBox1";
            this.ConnStrTxtBox1.Size = new System.Drawing.Size(388, 20);
            this.ConnStrTxtBox1.TabIndex = 3;
            // 
            // ConnStrTxtBox2
            // 
            this.ConnStrTxtBox2.Location = new System.Drawing.Point(166, 35);
            this.ConnStrTxtBox2.Name = "ConnStrTxtBox2";
            this.ConnStrTxtBox2.Size = new System.Drawing.Size(388, 20);
            this.ConnStrTxtBox2.TabIndex = 4;
            // 
            // ResultsTxtBox
            // 
            this.ResultsTxtBox.Location = new System.Drawing.Point(5, 208);
            this.ResultsTxtBox.Name = "ResultsTxtBox";
            this.ResultsTxtBox.Size = new System.Drawing.Size(549, 137);
            this.ResultsTxtBox.TabIndex = 5;
            this.ResultsTxtBox.Text = "";
            // 
            // TestBtn
            // 
            this.TestBtn.Location = new System.Drawing.Point(474, 353);
            this.TestBtn.Name = "TestBtn";
            this.TestBtn.Size = new System.Drawing.Size(75, 23);
            this.TestBtn.TabIndex = 6;
            this.TestBtn.Text = "Test";
            this.TestBtn.UseVisualStyleBackColor = true;
            this.TestBtn.Click += new System.EventHandler(this.TestBtn_Click);
            // 
            // QueryTxt
            // 
            this.QueryTxt.Location = new System.Drawing.Point(5, 86);
            this.QueryTxt.Name = "QueryTxt";
            this.QueryTxt.Size = new System.Drawing.Size(549, 98);
            this.QueryTxt.TabIndex = 8;
            this.QueryTxt.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(4, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "Query";
            // 
            // TestMSDTCForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 388);
            this.Controls.Add(this.QueryTxt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TestBtn);
            this.Controls.Add(this.ResultsTxtBox);
            this.Controls.Add(this.ConnStrTxtBox2);
            this.Controls.Add(this.ConnStrTxtBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "TestMSDTCForm";
            this.Text = "TestMSDTCForm";
            this.Load += new System.EventHandler(this.TestMSDTCForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ConnStrTxtBox1;
        private System.Windows.Forms.TextBox ConnStrTxtBox2;
        private System.Windows.Forms.RichTextBox ResultsTxtBox;
        private System.Windows.Forms.Button TestBtn;
        private System.Windows.Forms.RichTextBox QueryTxt;
        private System.Windows.Forms.Label label4;
    }
}

