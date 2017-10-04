using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Configuration;
using System.Collections.Specialized;

namespace RestoreSQLBackupLibrary
{
    /// <summary>
    /// Helper class to restore DB
    /// </summary>
    public class DBRestoreHelper
    {
        private DBRestoreHelper()
        {
        }

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


        public static List<string> RestoreCleanupShrinkDB(string jsonFilePath)
        {
            List<string> RetList = new List<string>(5);
            DBRestoreParam dbparam = DBRestoreParam.NewDBRestoreParam(jsonFilePath);
            string outMessage = string.Empty;

            if ((dbparam != null) && (dbparam.IsValid(out outMessage)))
            {
                string sqlBackupfile = string.Empty;

                foreach (string dbName in dbparam.DatabasesToRestore)
                {
                    RetList.Add("Working on DB:" + dbName);
                    RetList.AddRange(RestoreCleanupShrinkDB(dbparam, dbName, false));
                    RetList.Add("Done with DB:" + dbName);
                }
            }
            else
            {
                RetList.Add(outMessage);
            }
            return RetList;
        }

        public static List<string> RestoreCleanupShrinkDB(DBRestoreParam dBRestoreParam)
        {
            List<string> RetList = new List<string>(5);
            string outMessage = string.Empty;
            if ((dBRestoreParam == null) || (!dBRestoreParam.IsValid(out outMessage)))
                RetList.Add(outMessage);
            else
            {
                string[] databasesToRestore = dBRestoreParam.DatabasesToRestore.ToArray();
                string sqlBackupfile = string.Empty;
                foreach (string dbName in databasesToRestore)
                {
                    RetList.Add("Working on DB:" + dbName);
                    RetList.AddRange(RestoreCleanupShrinkDB(dBRestoreParam, dbName, false));
                    RetList.Add("Done with DB:" + dbName);
                }
            }

            return RetList;
        }

        public static List<string> RestoreCleanupShrinkDB(DBRestoreParam dBRestoreParam, string DBName, bool ValidateParam = true)
        {
            List<string> RetList = new List<string>(5);
            string sqlBackupfile = string.Empty;
            string outMessage = string.Empty;

            if (ValidateParam)
            {
                if ((dBRestoreParam == null) || (!dBRestoreParam.IsValid(out outMessage)))
                {
                    RetList.Add(outMessage);
                    return RetList;
                }
            }

            DbConnectionStringBuilder dbConnBuilder = new DbConnectionStringBuilder();
            dbConnBuilder.ConnectionString = dBRestoreParam.TargetServer.ConnString;

            RetList.Add(CopyBackupFromNetwork(dBRestoreParam, DBName, out sqlBackupfile));
            RetList.Add(RestoreBackup(sqlBackupfile, dBRestoreParam, DBName));
            RetList.Add(CleanupTables(DBName, dBRestoreParam));
            RetList.Add(ShrinkDatabase(DBName, dBRestoreParam));
            RetList.Add(FixDbUserLogin(DBName, dBRestoreParam));

            if (dBRestoreParam.PostRestore.DeleteLocalCopy)
                RetList.Add(DeleteBackupFile(sqlBackupfile));

            return RetList;
        }

        private static string CopyBackupFromNetwork(DBRestoreParam dBRestoreParam, string DBName, out string copiedBackupFile)
        {
            return CopyBackupFromNetwork(dBRestoreParam.RestoreBackupsFromPath, dBRestoreParam.TargetServer.LocalBackupPath
                                        , DBName, out copiedBackupFile);
        }

        private static string CopyBackupFromNetwork(string networkPath, string backupPath, string DBName, out string copiedBackupFile)
        {
            string strRetVal = string.Empty;
            copiedBackupFile = "";
            try
            {
                string filename = "*" + DBName + "*.bak";
                string strNetworkPath = Path.Combine(networkPath, DBName, "FULL");
                string[] strFiles = Directory.GetFiles(strNetworkPath, filename);

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
            catch (Exception ex)
            {
                strRetVal = "Error in copying Backup files from network for DB:" + DBName;
                strRetVal = strRetVal + System.Environment.NewLine + ex.Message;
                throw ex;
            }

            return strRetVal;
        }

        private static string RestoreBackup(string sqlBackupfile, DBRestoreParam dBRestoreParam, string DBName)
        {
            DbConnectionStringBuilder dbConnBuilder = new DbConnectionStringBuilder();
            dbConnBuilder.ConnectionString = dBRestoreParam.TargetServer.ConnString;
            return RestoreBackup(sqlBackupfile, dBRestoreParam.TargetServer.SQLDataPath, dBRestoreParam.TargetServer.SQLLogPath, DBName, dbConnBuilder);
        }

        private static string RestoreBackup(string sqlBackupfile, string dataPath, string logPath, string DBName, DbConnectionStringBuilder dbConnBuilder)
        {
            string strRetVal = string.Empty;
            FileInfo fi = new FileInfo(sqlBackupfile);
            string sqlBackuplocation = fi.DirectoryName;

            try
            {
                string strConn = dbConnBuilder.ConnectionString;
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

        private static string CleanupTables(string DBName, DBRestoreParam dBRestoreParam)
        {

            string ProcName = dBRestoreParam.PostRestore.CleanupProcName;
            DbConnectionStringBuilder dbConnBuilder = new DbConnectionStringBuilder();
            dbConnBuilder.ConnectionString = dBRestoreParam.TargetServer.ConnString;

            if (dBRestoreParam.PostRestore.CleanupTables)
                return CleanupTables(DBName, dbConnBuilder, ProcName);

            return string.Format("Cleanup Tables was not run for Database:{0}", DBName);
        }

        private static string CleanupTables(string DBName, DbConnectionStringBuilder dbConnBuilder, string ProcName = "")
        {
            string strRetVal = string.Empty;

            try
            {
                string strConn = dbConnBuilder.ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    conn.Open();

                    // 1.  create a command object identifying the stored procedure
                    ProcName = string.IsNullOrEmpty(ProcName) ? "dbo.usp_TruncateTableDBTest2_PostRestore" : ProcName;
                    SqlCommand cmd = new SqlCommand(ProcName, conn);

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

        private static string ShrinkDatabase(string DBName, DBRestoreParam dBRestoreParam)
        {
            DbConnectionStringBuilder dbConnBuilder = new DbConnectionStringBuilder();
            dbConnBuilder.ConnectionString = dBRestoreParam.TargetServer.ConnString;

            if (dBRestoreParam.PostRestore.ShrinkDB)
                return ShrinkDatabase(DBName, dbConnBuilder);

            return string.Format("Shrink DB was not run for Database:{0}", DBName);
        }
        private static string ShrinkDatabase(string DBName, DbConnectionStringBuilder dbConnBuilder)
        {
            string strRetVal = string.Empty;

            try
            {
                string strConn = dbConnBuilder.ConnectionString;
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

        private static string FixDbUserLogin(string DBName, DBRestoreParam dBRestoreParam)
        {
            DbConnectionStringBuilder dbConnBuilder = new DbConnectionStringBuilder();
            dbConnBuilder.ConnectionString = dBRestoreParam.TargetServer.ConnString;
            string[] usersToFix = dBRestoreParam.PostRestore.GetUsers(DBName);

            if (dBRestoreParam.PostRestore.RestoreUserAccess)
                return FixDbUserLogin(DBName, dbConnBuilder, usersToFix);

            return string.Format("DB User Login was not run for Database:{0}", DBName);
        }

        private static string FixDbUserLogin(string DBName, DbConnectionStringBuilder dbConnBuilder, string[] usersToFix)
        {
            string strRetVal = string.Empty;
            if ((usersToFix == null) || (usersToFix.Length == 0))
                usersToFix = new string [] { "ALL_SQL_USERS" };

            try
            {
                string strConn = dbConnBuilder.ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    conn.Open();

                    // 1.  create a command object identifying the stored procedure
                    SqlCommand cmd = new SqlCommand("dbo.usp_ReSyncDBUser_Login", conn);
                    // 2. set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    foreach (string username in usersToFix)
                    {
                        // 3. add parameter to command, which will be passed to the stored procedure
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new SqlParameter("@DBName", DBName));
                        cmd.Parameters.Add(new SqlParameter("@UserName", username));
                        // execute the command
                        cmd.ExecuteNonQuery();
                        strRetVal += "Successfully fixed user:" + username + " in DB:" + DBName + System.Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                strRetVal = "Error fixing user in DB:" + DBName;
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

    }
}
