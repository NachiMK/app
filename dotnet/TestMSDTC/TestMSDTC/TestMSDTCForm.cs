using System;
using System.Transactions;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TestMSDTC
{
    public partial class TestMSDTCForm : Form
    {
        public TestMSDTCForm()
        {
            InitializeComponent();
        }

        private void TestBtn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    using (SqlConnection SqlConn1 = new SqlConnection(this.ConnStrTxtBox1.Text))
                    {
                        string insertCmdSql = this.QueryTxt.Text;
                        SqlCommand insertCmd = new SqlCommand(insertCmdSql, SqlConn1);

                        // This command runs properly.
                        insertCmd.Connection.Open();
                        insertCmd.ExecuteNonQuery();

                        FormWriteLine("Conn1 successfully.");
                    }

                    using (SqlConnection SqlConn2 = new SqlConnection(this.ConnStrTxtBox2.Text))
                    {
                        string exceptionCausingCmdSQL = this.QueryTxt.Text;
                        SqlCommand exceptionCausingCmd = new SqlCommand(exceptionCausingCmdSQL, SqlConn2);

                        // This command results in an exception, which automatically rolls back
                        // the first command (the insertCmd command).
                        exceptionCausingCmd.Connection.Open();
                        int cmdResult = exceptionCausingCmd.ExecuteNonQuery();

                        FormWriteLine("Conn2 successfully.");
                    }

                    FormWriteLine("Test successful.");
                }

            }
            catch (Exception ex)
            {
                FormWriteLine(ex.Message);
                if (ex.InnerException != null)
                    FormWriteLine(ex.InnerException.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void InitForm()
        {
            this.ConnStrTxtBox1.Text = "Server=prod1_dbtest2;database=CartEasy;Trusted_Connection='True';";
            this.ConnStrTxtBox2.Text = "Server=prod1_dbtest2;database=UserProfile;Trusted_Connection='True';";
            this.QueryTxt.Text = "SELECT GETDATE() AS dt, DB_NAME() as dbName;";
            this.ResultsTxtBox.Text = string.Empty;
        }

        private void TestMSDTCForm_Load(object sender, EventArgs e)
        {
            this.InitForm();
        }

        private void FormWriteLine(string msg)
        {
            this.ResultsTxtBox.Text = msg + System.Environment.NewLine + this.ResultsTxtBox.Text;
        }
    }
}
