using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSeeder.Export.Library
{
    public class ExportFileOptions
    {

        #region Public Properties

        public int MaxRowsInMemory { get; set; }

        public bool IncludeHeaders { get; set; }
        
        public bool IncludeTextQualifiers { get; set; }
        
        public char TextQualifier { get; set; }

        public ColumnDelimiter ColDelimiter { get; set; }

        public string ColDelimiterString
        {
            get
            {
                return ExportToFileEnumHelper.ColumnDelimiterToString(ColDelimiter);
            }
        }

        public RowDelimiters RowDelimiter { get; set; }

        public string RowDelimiterString
        {
            get
            {
                return ExportToFileEnumHelper.RowDelimiterToString(RowDelimiter);
            }
        }

        public bool SplitFiles { get; set; }

        public int MaxRowsPerFile { get; set; }

        public FileCompressionTypeEnum CompressionType { get; set; }

        public string CompressionTypeToString
        {
            get
            {
                string strRet = string.Empty;
                if (ExportToFileEnumHelper.CompressionFileTypeStrings()[(int)CompressionType] != null)
                    strRet = ExportToFileEnumHelper.CompressionFileTypeStrings()[(int)CompressionType];

                // return finded value
                return strRet;
            }
        }

        public string FilePrefix { get; set; }

        public bool AppendDateFormat { get; set; }

        public DateFormat FileDateFormat { get; set; }

        public ExportToFileTypeEnum ExportToFileType { get; set; }

        public string ExportToFileTypeToString
        {
            get
            {
                // return finded value
                return EnumHelper.GetDescription(ExportToFileType);
            }
        }

        public Encoding FileEncoding { get; set; }

        public string FileEncodingString
        {
            get
            {
                return FileEncoding.EncodingName;
            }
        }

        #endregion

        public ExportFileOptions()
        {
            this.MaxRowsInMemory = 100000;
            this.IncludeHeaders = true;
            this.IncludeTextQualifiers = true;
            this.TextQualifier = '\"';
            this.ColDelimiter = ColumnDelimiter.Comma;
            this.RowDelimiter = RowDelimiters.CarriageReturnLineFeed;
            this.SplitFiles = false;
            this.MaxRowsPerFile = 100000;
            this.CompressionType = FileCompressionTypeEnum.None;
            this.AppendDateFormat = true;
            this.FileDateFormat = DateFormat.yyyyMMdd_HHmmssfff;
            this.ExportToFileType = ExportToFileTypeEnum.csv;
            this.FileEncoding = Encoding.UTF8;
        }

        private string TextQualifyAndEscape(object TheColumn)
        {
            string strTextQual = this.IncludeTextQualifiers ? "\"" : "";
            return strTextQual + TheColumn.ToString().Replace("\"", "\"\"") + strTextQual + this.ColDelimiterString;
        }

        #region System overrides

        public override string ToString()
        {
            StringBuilder str = new StringBuilder(100);

            str.AppendLine($"MaxRowsInFile : {this.MaxRowsInMemory}");
            str.AppendLine($"IncludeHeaders : {this.IncludeHeaders}");
            str.AppendLine($"IncludeTextQualifiers : {this.IncludeTextQualifiers}");
            str.AppendLine($"TextQualifier : {this.TextQualifier}");
            str.AppendLine($"ColDelimiter : {this.ColDelimiter}");
            str.AppendLine($"RowDelimiter : {this.RowDelimiter}");
            str.AppendLine($"SplitFiles : {this.SplitFiles}");
            str.AppendLine($"BatchSize : {this.MaxRowsPerFile}");
            str.AppendLine($"CompressionType : {this.CompressionType}");
            str.AppendLine($"AppendDateFormat : {this.AppendDateFormat}");
            str.AppendLine($"FileDateFormat : {this.FileDateFormat}");
            str.AppendLine($"ExportToFileType : {this.ExportToFileType}");
            str.AppendLine($"FileEncoding : {this.FileEncoding}");

            return str.ToString();
        }
        #endregion 

    }
}
