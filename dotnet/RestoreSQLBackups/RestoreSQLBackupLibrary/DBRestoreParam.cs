using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Text;

namespace RestoreSQLBackupLibrary
{
    public class TargetServerConfig
    {
        public string ConnString { get; set; }
        public string SQLDataPath { get; set; }
        public string SQLLogPath { get; set; }
        public string LocalBackupPath { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(10);
            sb.AppendLine(string.Format("ConnString:{0}", ConnString));
            sb.AppendLine(string.Format("SQLDataPath:{0}", SQLDataPath));
            sb.AppendLine(string.Format("SQLLogPath:{0}", SQLLogPath));
            sb.AppendLine(string.Format("LocalBackupPath:{0}", LocalBackupPath));

            return sb.ToString();
        }
    }

    public class PostRestoreOptions
    {
        public bool CleanupTables { get; set; }
        public string CleanupProcName { get; set; }
        public bool ShrinkDB { get; set; }
        public bool RestoreUserAccess { get; set; }
        public List<string> UsersToRestore { get; set; }
        public bool DeleteLocalCopy { get; set; }

        public PostRestoreOptions()
        {
            this.CleanupTables = false;
            this.ShrinkDB = false;
            this.RestoreUserAccess = false;
            this.UsersToRestore = new List<string>();
            this.DeleteLocalCopy = true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(10);
            sb.AppendLine(string.Format("CleanupTables:{0}", CleanupTables));
            sb.AppendLine(string.Format("CleanupProcName:{0}", CleanupProcName));
            sb.AppendLine(string.Format("ShrinkDB:{0}", ShrinkDB));
            sb.AppendLine(string.Format("RestoreUserAccess:{0}", RestoreUserAccess));
            sb.AppendLine(string.Format("UsersToRestore:{0}", UsersToRestore.ToString()));
            sb.AppendLine(string.Format("DeleteLocalCopy:{0}", DeleteLocalCopy));

            return sb.ToString();
        }

        public string[] GetUsers(string dbName)
        {
            string[] retVal = null;

            if (!string.IsNullOrEmpty(dbName))
            {
                foreach(string str in this.UsersToRestore)
                {
                    string[] dbAndUsers = str.Split(':');
                    if ((dbAndUsers != null) && (dbAndUsers.Length == 2))
                    {
                        if (dbAndUsers[0].Equals(dbName))
                        {
                            retVal = dbAndUsers[1].Split(',');
                            break;
                        }
                    }
                }
            }

            if ((retVal == null) || (retVal.Length == 0))
                retVal = new string[] { "ALL_SQL_USERS" };

            return retVal;
        }
    }

    /// <summary>
    /// Parameter class for restoring DB
    /// </summary>
    public class DBRestoreParam
    {
        public string RestoreBackupsFromPath { get; set; }
        public List<string> DatabasesToRestore { get; set; }
        public TargetServerConfig TargetServer { get; set; }
        public PostRestoreOptions PostRestore { get; set; }

        private DBRestoreParam()
        {
            this.RestoreBackupsFromPath = string.Empty;
            this.DatabasesToRestore = new List<string>(1);
            this.TargetServer = new TargetServerConfig();
            this.PostRestore = new PostRestoreOptions();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(10);
            sb.AppendLine(string.Format("RestoreBackupsFromPath:{0}", RestoreBackupsFromPath));
            sb.AppendLine(string.Format("DatabaseToRestore:{0}", DatabasesToRestore.ToString()));
            sb.AppendLine(string.Format("TargetServer:{0}", TargetServer.ToString()));
            sb.AppendLine(string.Format("PostRestore:{0}", PostRestore.ToString()));
            return sb.ToString();
        }

        public static DBRestoreParam NewDBRestoreParam(string jsonFilePath)
        {
            DBRestoreParam dBRestoreParam = null;

            if (File.Exists(jsonFilePath))
            {
                // get info from the file and populate properties
                dBRestoreParam = JsonConvert.DeserializeObject<DBRestoreParam>(File.ReadAllText(jsonFilePath));

                if (dBRestoreParam != null)
                {
                    dBRestoreParam.RestoreBackupsFromPath = CleanupPath(dBRestoreParam.RestoreBackupsFromPath);
                    dBRestoreParam.TargetServer.SQLDataPath = CleanupPath(dBRestoreParam.TargetServer.SQLDataPath);
                    dBRestoreParam.TargetServer.SQLLogPath = CleanupPath(dBRestoreParam.TargetServer.SQLLogPath);
                    dBRestoreParam.TargetServer.LocalBackupPath = CleanupPath(dBRestoreParam.TargetServer.LocalBackupPath);

                    if (!IsValidDBConnString(dBRestoreParam.TargetServer.ConnString))
                        dBRestoreParam.TargetServer.ConnString = string.Empty;

                    if (dBRestoreParam.PostRestore == null)
                        dBRestoreParam.PostRestore = new PostRestoreOptions();
                }
            }
            return dBRestoreParam;
        }

        public static DBRestoreParam NewDBRestoreParam(string networkPath
                                                    , string dataPath
                                                    , string logPath
                                                    , string backupPath
                                                    , string[] databasesToRestore
                                                    , string connString)
        {
            DBRestoreParam dBRestoreParam = new DBRestoreParam();
            dBRestoreParam.RestoreBackupsFromPath = networkPath;
            dBRestoreParam.TargetServer.SQLDataPath = dataPath;
            dBRestoreParam.TargetServer.SQLLogPath = logPath;
            dBRestoreParam.TargetServer.LocalBackupPath = backupPath;
            dBRestoreParam.DatabasesToRestore = new List<string>(databasesToRestore);
            dBRestoreParam.TargetServer.ConnString = connString;

            return dBRestoreParam;
        }

        public bool IsValid(out string outputMessage)
        {
            outputMessage = string.Empty;
            StringBuilder sb = new StringBuilder(10);

            if (!IsValidDirectoryPath(this.RestoreBackupsFromPath))
                sb.AppendLine(string.Format("BackupNetworkPath:{0} is invalid. Please set proper path.", this.RestoreBackupsFromPath));
            if (!IsValidDBConnString(this.TargetServer.ConnString))
                sb.AppendLine(string.Format("TargetServer.ConnString:{0} is invalid. Please set proper path.", this.TargetServer.ConnString));
            if (this.DatabasesToRestore.Count == 0)
                sb.AppendLine(string.Format("No Databases where selected for restore:{0}.", this.DatabasesToRestore.ToString()));

            if (!IsValidDirectoryPath(this.TargetServer.SQLDataPath))
                sb.AppendLine(string.Format("TargetServer.SQLDataPath:{0} is invalid. Please set proper path.", this.TargetServer.SQLDataPath));
            if (!IsValidDirectoryPath(this.TargetServer.SQLLogPath))
                sb.AppendLine(string.Format("TargetServer.SQLLogPath:{0} is invalid. Please set proper path.", this.TargetServer.SQLLogPath));
            if (!IsValidDirectoryPath(this.TargetServer.LocalBackupPath))
                sb.AppendLine(string.Format("TargetServer.LocalBackupPath:{0} is invalid. Please set proper path.", this.TargetServer.LocalBackupPath));

            if ((this.PostRestore != null) && this.PostRestore.CleanupTables && (string.IsNullOrEmpty(this.PostRestore.CleanupProcName)))
                sb.AppendLine(string.Format("PostRestore.CleanupProcName:{0} is invalid. Please set proc name.", this.PostRestore.CleanupProcName));

            outputMessage = sb.ToString();
            return string.IsNullOrEmpty(outputMessage);
        }

        private static string CleanupPath(string dirPath)
        {
            if (IsValidDirectoryPath(dirPath))
                return dirPath;
            else
                return string.Empty;
        }

        private static bool IsValidDirectoryPath(string dirPath)
        {
            DirectoryInfo di = new DirectoryInfo(dirPath);
            bool blnRetval = false;

            if ((di != null) && (di.Exists))
                blnRetval = true;

            return blnRetval;
        }

        private static bool IsValidDBConnString(string conn)
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
                catch
                {
                    blnRetval = false;
                }
            }
            return blnRetval;
        }
    }
}
