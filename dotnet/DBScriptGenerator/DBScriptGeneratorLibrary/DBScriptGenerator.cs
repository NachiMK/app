using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.SqlServer.Management.Smo;


namespace DBScriptGeneratorLibrary
{

    public class DBScriptGenerator : IDisposable
    {

        public static List<string> AllowedListOfSQLObjectsForScripting()
        {
            List<string> retList = new List<string>(10);
            retList.Add("Database Only");
            retList.Add("Assemblies");
            retList.Add("Schemas");
            retList.Add("Types");
            retList.Add("Tables");
            retList.Add("Synonym & Views");
            retList.Add("Procedures");
            retList.Add("Functions");
            retList.Add("Users & Permissions");
            retList.Add("Server Logins");
            retList.Add("Linked Servers");

            return retList;
        }

        public string ServerName { get; private set; }
        public List<string> ListOfObjectsToScript { get; private set;}

        public Server DBServer
        {
            get
            {
                if (_dbServer == null)
                {
                    _dbServer = new Server(this.ServerName);
                    _dbServer.ConnectionContext.LoginSecure = true;
                }
                return _dbServer;
            }
            private set
            {
                _dbServer = value;
            }
        }

        private DBScriptGenerator(string TheServerName, List<string> ObjectsToScriptList = null)
        {
            this.ServerName = TheServerName;
            this.ListOfObjectsToScript = ObjectsToScriptList == null ? AllowedListOfSQLObjectsForScripting() : ObjectsToScriptList;
        }

        public static DBScriptGenerator NewDBScriptGenerator(string TheServerName, List<string> ObjectsToScriptList = null)
        {
            DBScriptGenerator val = new DBScriptGenerator(TheServerName, ObjectsToScriptList);
            return val;
        }

        public string GenerateTransferScript(string DBName, string OutputFilePath = "")
        {
            StringBuilder RetStringBuffer = new StringBuilder(10);
            string strRetVal = string.Empty;

            try
            {
                if (this.IsValidServer(false))
                {

                    Database db = this.DBServer.Databases[DBName];

                    RetStringBuffer.AppendLine("Database Script Result:" + ScriptDatbase(db, DBName, "10", OutputFilePath));
                    RetStringBuffer.AppendLine("Table Script Result:" + ScriptTablesOnly(db, DBName, "20", OutputFilePath));
                    RetStringBuffer.AppendLine("Other Objects Script Result:" + ScriptNonTableObjectsOnly(db, DBName, "30", OutputFilePath));

                    if ((!this.IncludeCreateDatabaseScript) && this.IncludeUserPermissions)
                        RetStringBuffer.AppendLine("SQLUser DB Permissions:" + ScriptDBUserPermission(db, OutputFilePath));

                    RetStringBuffer.AppendLine("Successfully Scripted for DB:" + DBName);
                }
            }
            catch(Exception ex)
            {
                strRetVal = "Error generating transfer script for DB:" + DBName + System.Environment.NewLine + "Exception Message :" + ex.Message;
                if (ex.InnerException != null)
                    strRetVal = ex.InnerException.Message + System.Environment.NewLine + strRetVal;
                RetStringBuffer.AppendLine(strRetVal);
            }
            finally
            {
                this.DisconnectServer();
            }

            return RetStringBuffer.ToString();
        }

        private string GenerateDBScript(string DBName, string OutputFilePath = "")
        {
            StringBuilder logMessages = new StringBuilder(100);

            try
            {
                if (this.IsValidServer(false))
                {
                    Database db = this.DBServer.Databases[DBName];

                    Scripter scr = new Scripter(DBServer);
                    GetSystemObjectsProperty();

                    ScriptingOptions options = GetDBScriptingOptions();
                    options.FileName = GetFileName(DBName, OutputFilePath);
                    scr.Options = options;

                    ScriptDB(db, scr);
                    options.AppendToFile = true;

                    ScriptSchemas(db, scr);
                    ScriptTypes(db, scr);
                    ScriptTables(db, scr);
                    ScriptSequences(db, scr);
                    ScriptViews(db, scr);
                    ScriptProcs(db, scr);
                    ScriptFunctions(db, scr);
                    ScriptDatabaseUsers(db, scr);

                    logMessages.Append("Successfully Processed DB:" + DBName);
                }
            }
            catch(Exception ex)
            {
                logMessages.Append("-------------");
                logMessages.AppendLine("Error Processing DB:" + DBName);
                logMessages.AppendLine(ex.Message);
                if (ex.InnerException != null)
                    logMessages.AppendLine(ex.InnerException.Message);
                logMessages.AppendLine("-------------");
            }
            finally
            {
                this.DisconnectServer();
            }

            return logMessages.ToString();
        }

        public List<string> GenerateScript(List<string> DatabaseList, string OutputFilePath = "")
        {
            List<string> logMessages = new List<string>(DatabaseList.Count);

            if (IncludeLogins)
                logMessages.Add(GenerateServerLogins(OutputFilePath));

            if (IncludeLinkedServers)
                logMessages.Add(ScriptLinkedServers(OutputFilePath, "02"));

            foreach (string strdbName in DatabaseList)
            {
                string strMsg = GenerateTransferScript(strdbName, OutputFilePath);
                logMessages.Add(strMsg);
            }
            return logMessages;
        }

        public string GenerateServerLogins(string OutputFilePath)
        {
            string strRetVal = "Logins were not included in the scripts.";
            string strFileName = GetFileName(this.DBServer.Name + "_Logins", OutputFilePath, "01");

            try
            {
                if (this.IsValidServer(false))
                {
                    DBServer.SetDefaultInitFields(typeof(Login), "IsSystemObject");
                    List<Login> LoginList = new List<Login>(1);

                    foreach (Login l in DBServer.Logins)
                    {
                        if ((!l.IsSystemObject) && (!l.IsDisabled) && (l.HasAccess) && (l.LoginType == LoginType.SqlLogin))
                        {
                            Login loginCopy = new Login(this.DBServer, l.Name);
                            loginCopy.DefaultDatabase = l.DefaultDatabase;
                            loginCopy.DenyWindowsLogin = l.DenyWindowsLogin;
                            loginCopy.Language = l.Language;
                            loginCopy.LoginType = l.LoginType;
                            try
                            {
                                loginCopy.PasswordExpirationEnabled = l.PasswordExpirationEnabled;
                                loginCopy.PasswordPolicyEnforced = l.PasswordPolicyEnforced;
                                loginCopy.Credential = l.Credential;
                            }
                            catch
                            { }
                            LoginList.Add(loginCopy);
                        }
                    }

                    Scripter scr = new Scripter(this.DBServer);
                    ScriptingOptions options = new ScriptingOptions();
                    options.IncludeHeaders = false;
                    options.AppendToFile = false;
                    options.ToFileOnly = true;
                    options.FileName = strFileName;
                    options.IncludeIfNotExists = true;
                    scr.Options = options;

                    scr.Script(LoginList.ToArray());
                    EnableAndChangePassword(strFileName);

                    strRetVal = "Scripts for logins generated successfully and saved to file:." + strFileName;
                }
            }
            catch (Exception ex)
            {
                strRetVal = "Error generating Login scripts:" + System.Environment.NewLine + ex.Message;
                if (ex.InnerException != null)
                    strRetVal = strRetVal + System.Environment.NewLine + ex.InnerException.Message;
            }
            finally
            {
                this.DisconnectServer();
            }

            return strRetVal;
        }

        public List<string> GetAllDatabases(bool IncludeSystemDB = false)
        {
            List<string> retList = new List<string>(10);
            if (this.IsValidServer(false))
            {
                foreach (Database db in this.DBServer.Databases)
                {
                    if (!db.IsSystemObject)
                        retList.Add(db.Name);
                }

                this.DisconnectServer();
            }
            return retList;
        }

        public void Dispose()
        {
            this.DisconnectServer();
        }

        private void GetSystemObjectsProperty()
        {
            DBServer.SetDefaultInitFields(typeof(Table), "IsSystemObject");
            DBServer.SetDefaultInitFields(typeof(View), "IsSystemObject");
            DBServer.SetDefaultInitFields(typeof(StoredProcedure), "IsSystemObject");
            DBServer.SetDefaultInitFields(typeof(UserDefinedFunction), "IsSystemObject");
        }

        private string ScriptNonTableObjectsOnly(Database db, string dBName, string filePrefix, string outputFilePath)
        {
            string strRetVal = "";

            if (!this.IncludeNonTableObjects)
                return "Procs, Functions, Views, and Type scripts were not included.";

            try
            {
                Transfer transfer = new Transfer(this.DBServer.Databases[dBName]);
                SetTransferOptions(transfer, dBName, false);
                transfer.Options = GetDBScriptingOptions(false);
                transfer.Options.FileName = GetFileName(dBName, outputFilePath, filePrefix);
                AppendDatabaseContext(transfer.Options.FileName, dBName);
                transfer.Options.AppendToFile = true;
                transfer.ScriptTransfer();
                strRetVal = "Susccessfully generated non-Table scripts for DB:" + dBName + " and saved to file:" + transfer.Options.FileName;
            }
            catch (Exception ex)
            {
                strRetVal = "Error generating non-Table scripts for DB:" + dBName + " Error: " + ex.Message;
                if (ex.InnerException != null)
                    strRetVal = ex.InnerException.Message + System.Environment.NewLine + strRetVal;
            }

            return strRetVal;
        }

        private string ScriptTablesOnly(Database db, string dBName, string filePrefix, string outputFilePath)
        {
            string strRetVal = "";

            if (!this.IncludeTables)
                return "Table scripts were not included.";

            try
            {
                Transfer transfer = new Transfer(this.DBServer.Databases[dBName]);
                SetTransferOptions(transfer, dBName, true);
                transfer.Options = GetDBScriptingOptions(true);
                transfer.Options.WithDependencies = true;
                transfer.Options.FileName = GetFileName(dBName, outputFilePath, filePrefix);
                AppendDatabaseContext(transfer.Options.FileName, dBName);
                transfer.Options.AppendToFile = true;
                transfer.ScriptTransfer();
                strRetVal = "Susccessfully generated Table scripts for DB:" + dBName + " and saved to file:" + transfer.Options.FileName;
            }
            catch (Exception ex)
            {
                strRetVal = "Error generating Table scripts for DB:" + dBName + " Error: " + ex.Message;
                if (ex.InnerException != null)
                    strRetVal = ex.InnerException.Message + System.Environment.NewLine + strRetVal;
            }

            return strRetVal;
        }

        private string ScriptLinkedServers(string outputFilePath, string filePrefix)
        {
            string strRetVal = "";

            if (!this.IncludeLinkedServers)
            {
                strRetVal = "Linked server scripts were not included.";
                return strRetVal;
            }

            try
            {
                Scripter scr = new Scripter(DBServer);
                GetSystemObjectsProperty();

                LinkedServer[] linkedServers = new LinkedServer[this.DBServer.LinkedServers.Count];
                this.DBServer.LinkedServers.CopyTo(linkedServers, 0);
                ScriptingOptions options = GetDBScriptingOptions(false);
                options.FileName = GetFileName(this.DBServer.Name, outputFilePath, filePrefix);
                scr.Options = options;
                scr.Script(linkedServers);

                strRetVal = "Susccessfully generated Linked server script for Server:" + this.DBServer.Name + " and saved to file:" + options.FileName;
            }
            catch (Exception ex)
            {
                strRetVal = "Error generating Linked server script for server:" + this.DBServer.Name + " Error: " + ex.Message;
                if (ex.InnerException != null)
                    strRetVal = ex.InnerException.Message + System.Environment.NewLine + strRetVal;
            }

            return strRetVal;
        }

        private string ScriptDatbase(Database db, string dBName, string filePrefix, string outputFilePath, bool includeUser = true)
        {
            string strRetVal = "";

            if (!IncludeCreateDatabaseScript)
                return "Database script was not selected. So no script for creating DB is included.";

            try
            {
                Scripter scr = new Scripter(DBServer);
                GetSystemObjectsProperty();

                ScriptingOptions options = GetDBScriptingOptions(false);
                options.FileName = GetFileName(dBName, outputFilePath, filePrefix);
                scr.Options = options;

                ScriptDB(db, scr);
                if (includeUser)
                {
                    ScriptDBUserPermission(db, outputFilePath);
                }

                strRetVal = "Susccessfully generated DB create script for DB:" + dBName + " and saved to file:" + options.FileName;
            }
            catch(Exception ex)
            {
                strRetVal = "Error generating DB script for DB:" + dBName + " Error: " + ex.Message;
                if (ex.InnerException != null)
                    strRetVal = ex.InnerException.Message + System.Environment.NewLine + strRetVal;
            }

            return strRetVal;
        }

        private void AppendDatabaseContext(string FilePath, string DBName)
        {
            WriteStringToFile(DBName, FilePath, string.Empty, true);
        }

        private void SetTransferOptions(Transfer transfer, string DBName, bool ScriptTablesOnly = false)
        {
            transfer.CopyAllDatabaseTriggers = false;
            transfer.CopyAllDefaults = true;
            transfer.CopyAllLogins = false;
            transfer.CopyAllObjects = false;
            transfer.CopyAllPartitionFunctions = false;
            transfer.CopyAllPartitionSchemes = false;
            transfer.CopyAllPlanGuides = false;
            transfer.CopyAllRoles = false;
            transfer.CopyAllSchemas = this.IncludeSchemas;
            transfer.CopyAllUserDefinedTypes = this.IncludeTypes;
            transfer.CopyAllUserDefinedDataTypes = this.IncludeTypes;
            transfer.CopyAllUserDefinedTableTypes = this.IncludeTypes;

            if (ScriptTablesOnly)
            {
                transfer.CopyAllSequences = this.IncludeTables;
                transfer.CopyAllTables = this.IncludeTables;

            }
            else
            {
                transfer.CopyAllRules = this.IncludeRules;
                transfer.CopyAllSqlAssemblies = this.IncludeAssemblies;
                transfer.CopyAllStoredProcedures = this.IncludeProcedures;
                transfer.CopyAllSynonyms = this.IncludeViews;
                transfer.CopyAllUserDefinedFunctions = this.IncludeFunctions;
                transfer.CopyAllViews = this.IncludeViews;
            }

            transfer.CopyAllUsers = false;
            transfer.CopyData = false;
        }

        private void ScriptDB(Database db, Scripter scr)
        {
            //Server srv = new Server("localhost");
            Database dbNew = new Database(this.DBServer, db.Name);
            dbNew.RecoveryModel = RecoveryModel.Simple;
            dbNew.LocalCursorsDefault = true;
            CopyDBProperties(db, dbNew);

            scr.Script(new Database[] { dbNew });
        }

        private static void CopyDBProperties(Database db, Database dbNew)
        {
            dbNew.CompatibilityLevel = db.CompatibilityLevel;
            dbNew.Collation = db.Collation;
            dbNew.ContainmentType = ContainmentType.None;
            dbNew.AnsiNullDefault = db.AnsiNullDefault;
            dbNew.AnsiNullsEnabled = db.AnsiNullsEnabled;
            dbNew.AnsiPaddingEnabled = db.AnsiPaddingEnabled;
            dbNew.AnsiWarningsEnabled = db.AnsiWarningsEnabled;
            dbNew.ArithmeticAbortEnabled = db.ArithmeticAbortEnabled;
            dbNew.AutoClose = db.AutoClose;
            dbNew.AutoShrink = db.AutoShrink;
            dbNew.AutoUpdateStatisticsEnabled = db.AutoUpdateStatisticsEnabled;
            dbNew.CloseCursorsOnCommitEnabled = db.CloseCursorsOnCommitEnabled;
            dbNew.ConcatenateNullYieldsNull = db.ConcatenateNullYieldsNull;
            dbNew.NumericRoundAbortEnabled = db.NumericRoundAbortEnabled;
            dbNew.QuotedIdentifiersEnabled = db.QuotedIdentifiersEnabled;
            dbNew.RecursiveTriggersEnabled = db.RecursiveTriggersEnabled;
            dbNew.BrokerEnabled = db.BrokerEnabled;
            dbNew.AutoUpdateStatisticsAsync = db.AutoUpdateStatisticsAsync;
            dbNew.DateCorrelationOptimization = db.DateCorrelationOptimization;
            dbNew.Trustworthy = db.Trustworthy;
            dbNew.IsParameterizationForced = db.IsParameterizationForced;
            dbNew.IsReadCommittedSnapshotOn = db.IsReadCommittedSnapshotOn;
            dbNew.HonorBrokerPriority = db.HonorBrokerPriority;
            dbNew.UserAccess = DatabaseUserAccess.Multiple;
            dbNew.PageVerify = db.PageVerify;
            dbNew.DatabaseOwnershipChaining = db.DatabaseOwnershipChaining;
            dbNew.TargetRecoveryTime = db.TargetRecoveryTime;
            dbNew.DelayedDurability = db.DelayedDurability;
            //dbNew.SetSnapshotIsolation(db.IsDatabaseSnapshot);
            dbNew.FilestreamNonTransactedAccess = db.FilestreamNonTransactedAccess;
            dbNew.ReadOnly = db.ReadOnly;
            dbNew.IsFullTextEnabled = db.IsFullTextEnabled;
        }

        private void ScriptSchemas(Database db, Scripter scr)
        {
            if (!IncludeSchemas)
                return;

            List<Schema> lstSchema = new List<Schema>(1);
            foreach (Schema smo in db.Schemas)
            {
                if (smo.Name == "sys" ||
                    smo.Name == "dbo" ||
                    smo.Name == "db_accessadmin" ||
                    smo.Name == "db_backupoperator" ||
                    smo.Name == "db_datareader" ||
                    smo.Name == "db_datawriter" ||
                    smo.Name == "db_ddladmin" ||
                    smo.Name == "db_denydatawriter" ||
                    smo.Name == "db_denydatareader" ||
                    smo.Name == "db_owner" ||
                    smo.Name == "db_securityadmin" ||
                    smo.Name == "INFORMATION_SCHEMA" ||
                    smo.Name == "guest") continue;

                {
                    lstSchema.Add(smo);
                }
            }
            scr.Script(lstSchema.ToArray());
        }

        private static void EnableAndChangePassword(string fileName)
        {
            File.WriteAllText(fileName, System.Text.RegularExpressions.Regex.Replace(File.ReadAllText(fileName), "WITH PASSWORD=N'.*'", "WITH PASSWORD=N'T3st1235'"));
            File.WriteAllText(fileName, System.Text.RegularExpressions.Regex.Replace(File.ReadAllText(fileName), "] DISABLE", "] ENABLE"));
        }

        private void ScriptTypes(Database db, Scripter scr)
        {
            if (!IncludeTypes)
                return;

            List<UserDefinedDataType> lstObjects = new List<UserDefinedDataType>(10);
            foreach (UserDefinedDataType obj in db.UserDefinedDataTypes)
                    lstObjects.Add(obj);
            scr.Script(lstObjects.ToArray());

            List<UserDefinedTableType> lstUserDefTableTypes = new List<UserDefinedTableType>(10);
            foreach (UserDefinedTableType obj in db.UserDefinedTableTypes)
                lstUserDefTableTypes.Add(obj);
            scr.Script(lstUserDefTableTypes.ToArray());

            List<UserDefinedType> lstUserDefinedTypes = new List<UserDefinedType>(10);
            foreach (UserDefinedType obj in db.UserDefinedTypes)
                lstUserDefinedTypes.Add(obj);
            scr.Script(lstUserDefinedTypes.ToArray());
        }

        private void ScriptTables(Database db, Scripter scr)
        {
            if (!IncludeTables)
                return;

            List<Table> lstObjects = new List<Table>(10);
            foreach (Table tbl in db.Tables)
                if (!tbl.IsSystemObject)
                    lstObjects.Add(tbl);
            scr.Script(lstObjects.ToArray());
        }

        private void ScriptSequences(Database db, Scripter scr)
        {
            if (!IncludeTables)
                return;

            if (db.Sequences != null)
            {
                Sequence[] arraySeq = new Sequence[db.Sequences.Count];
                db.Sequences.CopyTo(arraySeq, 0);
                scr.Script(arraySeq);
            }
        }

        private void ScriptViews(Database db, Scripter scr)
        {
            if (!IncludeViews)
                return;

            View[] view = new View[1];
            for (int idx = 0; idx < db.Views.Count; idx++)
            {
                if (!db.Views[idx].IsSystemObject)
                {
                    view[0] = db.Views[idx];
                    scr.Script(view);
                }
            }

            Synonym[]  synonyms = new Synonym[db.Synonyms.Count];
            db.Synonyms.CopyTo(synonyms, 0);
            scr.Script(synonyms);
        }

        private void ScriptProcs(Database db, Scripter scr)
        {
            if (!IncludeProcedures)
                return;

            List<StoredProcedure> lstProcs = new List<StoredProcedure>(10);
            foreach (StoredProcedure sp in db.StoredProcedures)
                if (!sp.IsSystemObject)
                    lstProcs.Add(sp);
            scr.Script(lstProcs.ToArray());
        }

        private void ScriptFunctions(Database db, Scripter scr)
        {
            if (!IncludeFunctions)
                return;

            List<UserDefinedFunction> lstObjects = new List<UserDefinedFunction>(10);
            foreach (UserDefinedFunction udf in db.UserDefinedFunctions)
                if (!udf.IsSystemObject)
                    lstObjects.Add(udf);
            scr.Script(lstObjects.ToArray());
        }

        private void ScriptDatabaseUsers(Database db, Scripter scr)
        {
            if (!IncludeUsers)
                return;

            List<User> lstUsers = GetSQLUsers(db);
            scr.Script(lstUsers.ToArray());
        }

        private void ScriptDBRoles(Database db, Scripter scr)
        {
            DatabaseRole[] dbRoles = new DatabaseRole[db.Roles.Count];
            db.Roles.CopyTo(dbRoles, 0);
            scr.Script(dbRoles);
        }

        private static List<User> GetSQLUsers(Database db)
        {
            List<User> lstUsers = new List<User>(1);
            foreach (User u in db.Users)
            {
                if ((!u.IsSystemObject) && (u.HasDBAccess) && (u.LoginType == LoginType.SqlLogin))
                    lstUsers.Add(u);
            }

            return lstUsers;
        }

        private string ScriptDBUserPermission(Database db, string outputFilePath, bool includeUser = true)
        {
            string strRetVal = "";
            string strFileName = GetFileName(db.Name, outputFilePath, "40"); 
            StringBuilder strUserPermissions = new StringBuilder(100);

            if (!this.IncludeUserPermissions)
                return "User Permissions are not required to be scripted!! So Skipped it!!";

            try
            {
                if (includeUser)
                {
                    Scripter scr = new Scripter(DBServer);
                    GetSystemObjectsProperty();

                    ScriptingOptions options = GetDBScriptingOptions(false);
                    options.FileName = strFileName;
                    scr.Options = options;
                    
                    ScriptDatabaseUsers(db, scr);
                    scr.Options.AppendToFile = true;
                    ScriptDBRoles(db, scr);
                }

                List<User> lstUsers = GetSQLUsers(db);
                strUserPermissions.AppendLine("");
                foreach (User u in lstUsers)
                {
                    strUserPermissions.AppendLine(ScriptUserRoles(u));
                    strUserPermissions.Append(ScriptUserPermissions(u, db));
                    strUserPermissions.AppendLine(ScriptObjectLevelPermissions(u, db));
                }

                strUserPermissions.AppendLine("");
                WriteStringToFile(db.Name, strFileName, strUserPermissions.ToString());

                strRetVal = "Successfully scripted DB User permissions for DB:" + db.Name;
            }
            catch(Exception ex)
            {
                strRetVal = "Error scripting DB permissions:" + ex.Message + " Inner Exception:" + ex.InnerException.Message;
            }

            return strRetVal;
        }

        private string ScriptObjectLevelPermissions(User u, Database db)
        {
            StringBuilder sbObjPerm = new StringBuilder(100);
            sbObjPerm.AppendLine("-- [-- OBJECT LEVEL PERMISSIONS --] --");
            foreach (ObjectPermissionInfo objectPerm in db.EnumObjectPermissions(u.Name))
            {
                string strobjectPer = objectPerm.PermissionState.ToString() 
                                    + " " + objectPerm.PermissionType.ToString() 
                                    + " ON " + QuoteName(objectPerm.ObjectSchema, true)
                                    + QuoteName(objectPerm.ObjectName) + " TO " + QuoteName(objectPerm.Grantee) + ";";
                sbObjPerm.AppendLine(strobjectPer);
            }
            return sbObjPerm.ToString();
        }

        private string ScriptUserPermissions(User u, Database db)
        {
            StringBuilder sbDBPermissions = new StringBuilder(100);
            sbDBPermissions.AppendLine("-- [--DB LEVEL PERMISSIONS --] --");
            foreach (DatabasePermissionInfo dbpermission in db.EnumDatabasePermissions(u.Name))
            {
                string strperm = dbpermission.PermissionState.ToString() + " " + dbpermission.PermissionType.ToString() + " TO " + QuoteName(dbpermission.Grantee) + ";";
                sbDBPermissions.AppendLine(strperm);
            }

            return sbDBPermissions.ToString();
        }

        private string ScriptUserRoles(User u)
        {
            string strRoleScript = "EXEC sp_addrolemember @rolename = '<@role>', @membername = '<@User>';";
            StringBuilder sbRoleScript = new StringBuilder(100);
            sbRoleScript.AppendLine("-- [-- DB ROLES --] -- for User:" + u.Name);
            foreach (string strUserRole in u.EnumRoles())
            {
                string strRole2 = strRoleScript.Replace("<@role>", strUserRole);
                strRole2 = strRole2.Replace("<@User>", u.Name);
                sbRoleScript.AppendLine(strRole2);
            }
            return sbRoleScript.ToString();
        }

        private string QuoteName(string strToQuote, bool includeObjectSeparator = false)
        {
            string strRetval = strToQuote;
            if (!string.IsNullOrEmpty(strToQuote))
            {
                strRetval = "[" + strToQuote + "]" + (includeObjectSeparator ? "." : "");
            }
            return strRetval;
        }

        private void WriteStringToFile(string dbName, string outputFilePath, string script, bool IncludeDBContext = true)
        {
            string path = outputFilePath;
            StringBuilder appendText = new StringBuilder(10);

            if (IncludeDBContext)
            {
                appendText.AppendLine("GO");
                appendText.AppendLine("USE [" + dbName + "];");
                appendText.AppendLine("GO");
            }
            if (!string.IsNullOrEmpty(script))
                appendText.AppendLine(script);

            if (File.Exists(path))
                File.AppendAllText(path, appendText.ToString(), Encoding.UTF8);
            else
                File.WriteAllText(path, appendText.ToString(), Encoding.UTF8);
        }

        private ScriptingOptions GetDBScriptingOptions(bool IncludeConstraintsAndIndexes = false)
        {
            ScriptingOptions options = new ScriptingOptions();

            if (IncludeConstraintsAndIndexes)
            {
                options.DriAll = true;
                options.ClusteredIndexes = true;
                options.Default = true;
                options.Indexes = true;
                options.DriForeignKeys = true;
                options.DriDefaults = true;
            }

            options.IncludeHeaders = false;
            options.IncludeIfNotExists = true;
            options.ToFileOnly = true;
            options.AppendToFile = false;
            options.IncludeDatabaseContext = true;
            options.ScriptData = false;
            options.Encoding = Encoding.UTF8;
            return options;
        }

        private string GetFileName(string dBName, string filePath, string filePrefix = "")
        {
            string strRetVal = string.Empty;
            string strFilePrefix = string.Empty;

            if (!string.IsNullOrEmpty(filePrefix))
            {
                if (!filePrefix.EndsWith("_"))
                    strFilePrefix = RemoveInvalidChars(filePrefix + "_");
                else
                    strFilePrefix = RemoveInvalidChars(filePrefix);
            }

            string filename  = strFilePrefix + RemoveInvalidChars(dBName) + "_" + DateTime.Now.ToString("MMddyyyy_HHmss") + ".sql";
            if (System.IO.Directory.Exists(filePath))
                strRetVal = System.IO.Path.Combine(filePath, filename);
            return strRetVal;
        }

        private string RemoveInvalidChars(string stringToClean)
        {
            string strCleaned = stringToClean;
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                strCleaned = strCleaned.Replace(c, '_');
            }
            return strCleaned;
        }

        private void DisconnectServer()
        {
            if ((this.DBServer != null) && (this.DBServer.ConnectionContext != null) && this.DBServer.ConnectionContext.IsOpen)
                this.DBServer.ConnectionContext.Disconnect();
        }

        private bool IsValidServer(bool disconnect = true)
        {
            bool retVal = false;

            try
            {
                string str = this.DBServer.Information.Version.ToString();
                if (disconnect)
                    this.DBServer.ConnectionContext.Disconnect();
                retVal = true;
            }
            catch
            {
                this._dbServer = null;
                retVal = false;
            }
            return retVal;
        }
        private bool IncludeCreateDatabaseScript
        {
            get
            {
                return IncludeObjectInScript("Database Only");
            }
        }

        private bool IncludeNonTableObjects
        {
            get
            {
                bool blnRetVal = this.IncludeRules ||
                                this.IncludeAssemblies ||
                                this.IncludeProcedures ||
                                this.IncludeViews ||
                                this.IncludeFunctions ||
                                this.IncludeViews;
                return blnRetVal;
            }
        }

        private bool IncludeSchemas
        {
            get
            {
                return IncludeObjectInScript("Schemas");
            }
        }
        private bool IncludeRules
        {
            get
            {
                return IncludeObjectInScript("Rules");
            }
        }
        private bool IncludeAssemblies
        {
            get
            {
                return IncludeObjectInScript("Assemblies");
            }
        }

        private bool IncludeTables
        {
            get
            {
                return IncludeObjectInScript("Tables");
            }
        }
        private bool IncludeProcedures
        {
            get
            {
                return IncludeObjectInScript("Procedures");
            }
        }
        private bool IncludeFunctions
        {
            get
            {
                return IncludeObjectInScript("Functions");
            }
        }
        private bool IncludeUsers
        {
            get
            {
                return IncludeObjectInScript("Users & Permissions");
            }
        }

        private bool IncludeUserPermissions
        {
            get
            {
                return IncludeObjectInScript("Users & Permissions");
            }
        }

        private bool IncludeTypes
        {
            get
            {
                return IncludeObjectInScript("Types");
            }
        }

        private bool IncludeViews
        {
            get
            {
                return IncludeObjectInScript("Views");
            }
        }

        private bool IncludeLinkedServers
        {
            get
            {
                return IncludeObjectInScript("Linked Servers");
            }
        }

        private bool IncludeLogins
        {
            get
            {
                return IncludeObjectInScript("Logins");
            }
        }
        private bool IncludeObjectInScript(string ObjectTypeDesc)
        {
            bool blnRetVal = false;

            if ((this.ListOfObjectsToScript != null) && (this.ListOfObjectsToScript.Count > 0) && !string.IsNullOrEmpty(ObjectTypeDesc))
            {
                if (this.ListOfObjectsToScript.Exists(se => se.ToUpper().Contains(ObjectTypeDesc.ToUpper())))
                    blnRetVal = true;
            }

            return blnRetVal;
        }

        private Server _dbServer;
    }
}
