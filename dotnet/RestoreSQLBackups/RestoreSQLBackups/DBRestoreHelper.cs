using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;

namespace RestoreSQLBackups
{
    internal class TODELETEClass
    {
        public static List<string> DatabaseToRestore()
        {
            List<string> RetList = new List<string>(10);

            var requiredDatabasesToRestore = ConfigurationManager.GetSection("RequiredDatabasesToRestore") as NameValueCollection;
            if (requiredDatabasesToRestore != null)
            {
                foreach (var dbkey in requiredDatabasesToRestore.AllKeys)
                {
                    if (requiredDatabasesToRestore.GetValues(dbkey).Length > 0)
                    {
                        string dbvalue = requiredDatabasesToRestore.GetValues(dbkey)[0];
                        RetList.Add(dbvalue);
                    }
                }
            }
            return RetList;
        }

        public static List<string> OptionalDBToRestore()
        {
            List<string> RetList = new List<string>(10);

            var optionalDatabasesToRestore = ConfigurationManager.GetSection("OptionalDatabasesToRestore") as NameValueCollection;
            if (optionalDatabasesToRestore != null)
            {
                foreach (var dbkey in optionalDatabasesToRestore.AllKeys)
                {
                    if (optionalDatabasesToRestore.GetValues(dbkey).Length > 0)
                    {
                        string dbname = optionalDatabasesToRestore.GetValues(dbkey)[0];
                        RetList.Add(dbname);
                    }
                }
            }

            return RetList;
        }

        public static List<string> RestoreCleanupShrinkDB(string networkPath
                                    , string dataPath
                                    , string logPath
                                    , string backupPath
                                    , string [] databasesToRestore
                                    , string serverName = "")
        {
            List<string> RetList = new List<string>(5);
            string sqlBackupfile = string.Empty;
            foreach (string dbName in databasesToRestore)
            {
                RetList.Add("Working on DB:" + dbName);
                RetList.Add(CopyBackupFromNetwork(networkPath, backupPath, dbName, out sqlBackupfile));
                RetList.Add(RestoreBackup(sqlBackupfile, dataPath, logPath, dbName, serverName));
                RetList.Add(CleanupTables(dbName, serverName));
                RetList.Add(ShrinkDatabase(dbName, serverName));
                RetList.Add(FixDbUserLogin(dbName, serverName));
                RetList.Add(DeleteBackupFile(sqlBackupfile));
                RetList.Add("Done with DB:" + dbName);
            }

            return RetList;
        }

        public static List<string> RestoreCleanupShrinkDB(string networkPath
            , string dataPath
            , string logPath
            , string backupPath
            , string DBName
            , string serverName = "")
        {
            List<string> RetList = new List<string>(5);
            string sqlBackupfile = string.Empty;
            RetList.Add(CopyBackupFromNetwork(networkPath, backupPath, DBName, out sqlBackupfile));
            RetList.Add(RestoreBackup(sqlBackupfile, dataPath, logPath, DBName, serverName));
            RetList.Add(CleanupTables(DBName, serverName));
            RetList.Add(ShrinkDatabase(DBName, serverName));
            RetList.Add(FixDbUserLogin(DBName, serverName));
            RetList.Add(DeleteBackupFile(sqlBackupfile));
            return RetList;
        }

        private static string CopyBackupFromNetwork(string networkPath, string backupPath, string DBName, out string copiedBackupFile)
        {
            string strRetVal = string.Empty;
            copiedBackupFile = "";
            try
            {
                string filename = "*" + DBName + "*.bak";
                string strNetworkPath = Path.Combine(networkPath, DBName, "FULL");
                string[] strFiles =  Directory.GetFiles(strNetworkPath, filename);

                if (strFiles.Length > 0)
                {
                    strNetworkPath = Path.Combine(networkPath, strFiles[0]);
                    FileInfo fi = new FileInfo(strNetworkPath);
                    DirectoryInfo backupDirInfo = new DirectoryInfo(Path.Combine(backupPath, DBName));

                    if (!backupDirInfo.Exists)
                        Directory.CreateDirectory(backupDirInfo.FullName);

                    copiedBackupFile = Path.Combine(backupDirInfo.FullName, fi.Name);

                    if (!File.Exists(copiedBackupFile))
                        File.Copy(strNetworkPath, copiedBackupFile);

                    strRetVal = "Successfully copied file from Network for DB:" + DBName;
                    strRetVal = strRetVal + System.Environment.NewLine + "Source Path: " + strNetworkPath;
                    strRetVal = strRetVal + System.Environment.NewLine + "Target Path: " + copiedBackupFile;

                    if (File.Exists(copiedBackupFile))
                        strRetVal = strRetVal + System.Environment.NewLine + "File Exists in Target: " + copiedBackupFile;
                    else
                        strRetVal = strRetVal + System.Environment.NewLine + "File DOES NOT Exists in Target: " + copiedBackupFile;
                }
            }
            catch(Exception ex)
            {
                strRetVal = "Error in copying Backup files from network for DB:" + DBName;
                strRetVal = strRetVal + System.Environment.NewLine + ex.Message;
                throw ex;
            }

            return strRetVal;
        }

        private static string RestoreBackup(string sqlBackupfile, string dataPath, string logPath, string DBName, string serverName = "")
        {
            string strRetVal = string.Empty;
            FileInfo fi = new FileInfo(sqlBackupfile);
            string sqlBackuplocation = fi.DirectoryName;

            try
            {
                string strConn = TODELETEClass.DBAToolConnectionString(serverName);
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    conn.Open();

                    // 1.  create a command object identifying the stored procedure
                    SqlCommand cmd = new SqlCommand("dbo.sp_RestoreFromAllFilesInDirectory", conn);

                    // 2. set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    // 3. add parameter to command, which will be passed to the stored procedure
                    cmd.Parameters.Add(new SqlParameter("@SourceDirBackupFiles", sqlBackuplocation));
                    cmd.Parameters.Add(new SqlParameter("@DestDirDbFiles", dataPath));
                    cmd.Parameters.Add(new SqlParameter("@DestDirLogFiles", logPath));
                    cmd.Parameters.Add(new SqlParameter("@OnlyPrintCommands", false));

                    // execute the command
                    cmd.ExecuteNonQuery();
                }

                strRetVal = "Successfully Restored backup :" + sqlBackupfile;
            }
            catch (Exception ex)
            {
                strRetVal = "Error in restoring Backup for DB:" + sqlBackupfile;
                strRetVal = strRetVal + System.Environment.NewLine + ex.Message;
                throw ex;
            }

            return strRetVal;
        }

        private static string CleanupTables(string DBName, string serverName = "")
        {
            string strRetVal = string.Empty;

            try
            {
                string strConn = TODELETEClass.DBAToolConnectionString(serverName);
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    conn.Open();

                    // 1.  create a command object identifying the stored procedure
                    SqlCommand cmd = new SqlCommand("dbo.usp_TruncateTableDBTest2_PostRestore", conn);

                    // 2. set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    // 3. add parameter to command, which will be passed to the stored procedure
                    cmd.Parameters.Add(new SqlParameter("@DBToTruncate", DBName));
                    cmd.Parameters.Add(new SqlParameter("@Debug", false));

                    // execute the command
                    cmd.ExecuteNonQuery();
                }

                strRetVal = "Successfully cleanedup tables for DB:" + DBName;
            }
            catch (Exception ex)
            {
                strRetVal = "Error in clearing tables from network for DB:" + DBName;
                strRetVal = strRetVal + System.Environment.NewLine + ex.Message;
            }

            return strRetVal;
        }

        private static string ShrinkDatabase(string DBName, string serverName = "")
        {
            string strRetVal = string.Empty;

            try
            {
                string strConn = TODELETEClass.DBAToolConnectionString(serverName);
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    conn.Open();

                    // 1.  create a command object identifying the stored procedure
                    SqlCommand cmd = new SqlCommand("dbo.usp_ShrinkDatabase", conn);

                    // 2. set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    // 3. add parameter to command, which will be passed to the stored procedure
                    cmd.Parameters.Add(new SqlParameter("@DBName", DBName));

                    // execute the command
                    cmd.ExecuteNonQuery();
                }
                strRetVal = "Successfully Shrunk DB:" + DBName;
            }
            catch (Exception ex)
            {
                strRetVal = "Error shrinking DB:" + DBName;
                strRetVal = strRetVal + System.Environment.NewLine + ex.Message;
            }

            return strRetVal;
        }

        private static string FixDbUserLogin(string DBName, string Username = "lpsqlrw1", string serverName = "")
        {
            string strRetVal = string.Empty;

            try
            {
                string strConn = TODELETEClass.DBAToolConnectionString(serverName);
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    conn.Open();

                    // 1.  create a command object identifying the stored procedure
                    SqlCommand cmd = new SqlCommand("dbo.usp_ReSyncDBUser_Login", conn);

                    // 2. set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    // 3. add parameter to command, which will be passed to the stored procedure
                    cmd.Parameters.Add(new SqlParameter("@DBName", DBName));
                    cmd.Parameters.Add(new SqlParameter("@UserName", Username));

                    // execute the command
                    cmd.ExecuteNonQuery();
                }
                strRetVal = "Successfully fixed user:" + Username + " in DB:" + DBName;
            }
            catch (Exception ex)
            {
                strRetVal = "Error fixing user:" + Username + " in DB:" + DBName;
                strRetVal = strRetVal + System.Environment.NewLine + ex.Message;
            }

            return strRetVal;
        }

        private static string DeleteBackupFile(string sqlBackupfile)
        {
            string strRetVal = string.Empty;

            try
            {
                if (File.Exists(sqlBackupfile))
                    File.Delete(sqlBackupfile);

                if (!File.Exists(sqlBackupfile))
                    strRetVal = "Successfully delete backup file :" + sqlBackupfile;
                else
                    strRetVal = "Deleting backup file :" + sqlBackupfile + " completed. But, file is STILL IN LOCATION!!!";
            }
            catch (Exception ex)
            {
                strRetVal = "Error deleting backup file :" + sqlBackupfile;
                strRetVal = strRetVal + System.Environment.NewLine + ex.Message;
            }

            return strRetVal;
        }

        private static string ExecuteSQLCommand(string SQLCommand)
        {
            throw new NotImplementedException();
        }

        public static string DBAToolConnectionString(string serverName = "")
        {
            string strRetVal = ConfigurationManager.ConnectionStrings["DBATools"].ConnectionString;
            if (!string.IsNullOrEmpty(serverName))
                strRetVal = "Server='" + serverName + "';Database='DBATools';Trusted_Connection='True';";
            return strRetVal;
        }

    }
}
