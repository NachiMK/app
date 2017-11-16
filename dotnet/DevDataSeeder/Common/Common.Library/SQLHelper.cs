using System;
using System.Data;
using System.Data.SqlClient;

namespace Export.Common.Library
{
    public class SQLHelper
    {
        public static bool IsValidDBConnString(string conn, bool throwException = false)
        {
            bool blnRetval = false;

            if (!string.IsNullOrEmpty(conn))
            {
                try
                {
                    using (SqlConnection objConn = new SqlConnection(conn))
                    {
                        objConn.Open();
                        if (objConn.State == System.Data.ConnectionState.Open)
                            blnRetval = true;
                    }
                }
                catch (SqlException ex)
                {
                    if (!throwException)
                        blnRetval = false;
                    else
                        throw ex;
                }
            }
            else if (throwException)
                throw new ArgumentException("Connection String is empty. Please pass in a valid connection string.");

            return blnRetval;
        }

        internal static SqlDataReader ExecuteDataReader(string connectionString, string scriptToExecute, out Exception OutputEx)
        {
            SqlDataReader RetSqlDr = null;
            OutputEx = null;

            if (IsValidDBConnString(connectionString))
            {
                try
                {
                    string connString = connectionString;

                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = scriptToExecute;
                        conn.Open();
                        // NOTE CONNECTION IS OPEN --- YUCK
                        RetSqlDr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    }

                }
                catch (Exception ex)
                {
                    OutputEx = ex;
                }
            }
            else
            {
                OutputEx = new ArgumentException("Invalid SQL Connection Information provided");
            }

            return RetSqlDr;
        }

        public static DataTable ExecuteDataTable(string connectionString, string scriptToExecute, out Exception OutputEx)
        {
            DataTable RetDataTable = null;
            OutputEx = null;

            if (IsValidDBConnString(connectionString))
            {
                try
                {
                    string connString = connectionString;

                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = scriptToExecute;
                        conn.Open();
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        RetDataTable = new DataTable("ExportToFileResultSet");
                        sda.Fill(RetDataTable);
                    }
                }
                catch (Exception ex)
                {
                    OutputEx = ex;
                    RetDataTable = null;
                }
            }
            else
            {
                OutputEx = new ArgumentException("Invalid SQL Connection Information provided.");
            }
            return RetDataTable;
        }
    }
}
