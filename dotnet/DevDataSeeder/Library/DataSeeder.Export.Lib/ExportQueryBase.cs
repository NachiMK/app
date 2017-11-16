using System;
using System.Collections.Generic;
using System.Text;

namespace DataSeeder.Export.Library
{
    public enum ExportCommandTypeEnum
    {
        Table,
        Query
    }

    public enum FilterConfitionTypeEnum
    {
        DoesNotApply,
        KeepOnlyMatchingRows,
        ExcludeMatchingRows
    }

    public class ExportQueryBase
    {
        public ExportCommandTypeEnum ExportCommandType { get; private set; }
        public virtual string SQLCommandText { get; internal set; }
        public string OutputFilePrefix { get; set; }

        internal ExportQueryBase()
        {
            ExportCommandType = ExportCommandTypeEnum.Table;
            SQLCommandText = string.Empty;
            OutputFilePrefix = string.Empty;
        }

        internal ExportQueryBase(ExportCommandTypeEnum exportCommandType, string sqlCmdText)
        {
            this.ExportCommandType = exportCommandType;
            this.SQLCommandText = sqlCmdText;
        }

        public bool ExportingTable => this.ExportCommandType == ExportCommandTypeEnum.Table;
        public bool ExportingQuery => this.ExportCommandType == ExportCommandTypeEnum.Query;

        #region system overrides

        public override string ToString()
        {
            StringBuilder str = new StringBuilder(100);
            str.AppendLine($"ExportCommandType : {this.ExportCommandType}");
            str.AppendLine($"SQLCommandText    : {this.SQLCommandText}");
            str.AppendLine($"OutputFilePrefix  : {this.OutputFilePrefix}");
            return str.ToString();
        }

        public override bool Equals(object obj)
        {
            bool Retval = false;
            if ((obj is ExportQueryBase objToCompare)
                && (this.SQLCommandText.Equals(objToCompare.SQLCommandText, StringComparison.OrdinalIgnoreCase))
                && (this.ExportCommandType == objToCompare.ExportCommandType))
                Retval = true;

            return Retval;
        }

        public override int GetHashCode()
        {
            return (this.ExportCommandType.ToString() +  SQLCommandText).GetHashCode();
        }
        #endregion

    }

    public class ExportTable : ExportQueryBase
    {
        public string FilterCondition { get; private set; }
        public string TableName { get; private set; }


        private ExportTable() : base()
        {
            this.TableName = string.Empty;
            this.FilterCondition = string.Empty;
        }

        public ExportTable(string tableName
                           ,string filterCondition = ""
                           ,string filePrefix = "")
            : base(ExportCommandTypeEnum.Table, "")
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException("tableName", "Please provide a Valid Table Name.");

            this.TableName = tableName;
            this.SQLCommandText = AddSelectClause(TableName);
            this.FilterCondition = filterCondition;

            this.OutputFilePrefix = string.IsNullOrEmpty(filePrefix) ? tableName : filePrefix;
        }

        public override string SQLCommandText
        {
            get
            {
                return $"{base.SQLCommandText} {this.FilterCondition}";
            }
        }

        public override bool Equals(object obj)
        {
            bool Retval = false;
            if ((obj is ExportTable objToCompare) 
                && (this.TableName.Equals(objToCompare.TableName, StringComparison.OrdinalIgnoreCase)))
                Retval = true;

            return Retval;
        }

        public override int GetHashCode()
        {
            return this.TableName.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder(100);
            str.AppendLine($"TableName : {this.TableName}");
            str.AppendLine($"Filter Condition : {this.FilterCondition}");
            str.AppendLine($"Base : {base.ToString()}");

            return str.ToString();
        }

        private string AddSelectClause(string tableName)
        {
            string RetVal = tableName;
            if (!tableName.StartsWith("SELECT * FROM ", StringComparison.OrdinalIgnoreCase))
                RetVal = "SELECT * FROM " + QuoteName(tableName) + " ";

            return RetVal;
        }

        private string QuoteName(string tableName)
        {
            string RetVal = tableName;

            if (!tableName.StartsWith("["))
                RetVal = "[" + tableName;

            if (!RetVal.EndsWith("]"))
                RetVal = RetVal + "]";

            return RetVal;
        }
    }

    public class ExportTableList : List<ExportTable>
    {
        public override string ToString()
        {
            StringBuilder strList = new StringBuilder(300);
            int i = 1;
            strList.AppendLine($"Total # of Items : {this.Count}");
            foreach (ExportTable t in this)
            {
                strList.AppendLine($"Index: {i}, Value: {t.ToString()}");
                i++;
            }
            return base.ToString();
        }
    }


    public class ExportQuery : ExportQueryBase
    {
        private ExportQuery()
            :base()
        {
            this.OutputFilePrefix = "QueryOutput_";
        }

        public ExportQuery(string sqlCmdText, string outputFilePrefix = "") : base(ExportCommandTypeEnum.Query, sqlCmdText)
        {
            if (string.IsNullOrEmpty(sqlCmdText))
                throw new ArgumentNullException("sqlCmdText", "Query for creating file is empty. Please enter a query.");

            this.OutputFilePrefix = string.IsNullOrEmpty(outputFilePrefix) ? "QueryOutput_" : outputFilePrefix;
        }
    }

    public class ExportQueryList : List<ExportQuery>
    {
        public override string ToString()
        {
            StringBuilder strList = new StringBuilder(300);
            int i = 1;
            strList.AppendLine($"Total # of Items : {this.Count}");
            foreach (ExportQuery t in this)
            {
                strList.AppendLine($"Index: {i}, Value: {t.ToString()}");
                i++;
            }
            return base.ToString();
        }
    }

}
