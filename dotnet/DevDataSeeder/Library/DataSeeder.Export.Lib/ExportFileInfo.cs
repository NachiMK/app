using System;
using System.Collections.Generic;
using System.Text;
using Export.Common.Library;

namespace DataSeeder.Export.Library
{
    public class ExportFileInfo
    {
        public string FileName { get; set; }
        public int StartRow { get; set; }
        public int EndRow { get; set; }
        public int NoOfRows
        {
            get
            {
                int rowCount = 0;
                if (EndRow > StartRow)
                    rowCount = (EndRow - StartRow) + 1;
                else
                    rowCount = EndRow;
                // return
                return rowCount;
            }
        }
        public bool Created { get; set; }
        public bool Compressed { get; set; }
        public bool Exported { get; set; }
        public string ExportedFileName { get; set; }

        public string OutputFileName
        {
            get
            {
                return (CompressionType == FileCompressionTypeEnum.None) ? this.FileName : 
                    $"{this.FileName}{ExportToFileEnumHelper.CompressionFileTypeStrings()[(int)CompressionType]}";
            }
        }
        public FileCompressionTypeEnum CompressionType { get; set; }
        public bool CompressionRequired
        {
            get
            {
                return (this.CompressionType != FileCompressionTypeEnum.None) ? true : false;
            }
        }
        public Exception FileExportError { get; set; }

        #region Constructor

        internal ExportFileInfo(string fileName, int startRow, int endRow, FileCompressionTypeEnum CompressType)
        {
            NewExportFileInfo(fileName, startRow, endRow, CompressType);
        }

        public ExportFileInfo(string filePrefix, string FileExtension)
        {
            string strFileName = filePrefix + FileExtension;
            NewExportFileInfo(strFileName, -1, -1, FileCompressionTypeEnum.None);
        }

        public ExportFileInfo(string filePrefix, string appendDateFormat, string fileExtension)
        {
            string strFileName = filePrefix + DateTime.Now.ToString(appendDateFormat) + fileExtension;
            NewExportFileInfo(strFileName, -1, -1, FileCompressionTypeEnum.None);
        }

        public ExportFileInfo(string filePrefix, string appendDateFormat, string fileExtension, FileCompressionTypeEnum CompressType)
        {
            // update file name
            string strFileName = filePrefix + DateTime.Now.ToString(appendDateFormat) + fileExtension;
            NewExportFileInfo(strFileName, -1, -1, CompressType);
        }

        #endregion

        #region private members

        private void NewExportFileInfo(string fileName, int startRow, int endRow, FileCompressionTypeEnum CompressType)
        {
            this.FileName = fileName;
            this.StartRow = startRow;
            this.EndRow = endRow;
            //default flags
            this.Compressed = false;
            this.Created = false;
            this.Exported = false;
            this.CompressionType = CompressType;
        }

        private void InitializeExportFile()
        {
            this.FileName = string.Empty;
            this.StartRow = -1;
            this.EndRow = -1;
            this.Compressed = false;
            this.Created = false;
            this.Exported = false;
            this.FileExportError = null;
        }

        #endregion

        #region public overrides

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(100);
            sb.AppendLine($"FileName :{this.FileName}");
            sb.AppendLine($"StartRow:{this.StartRow.ToString()}");
            sb.AppendLine($"EndRow:{this.EndRow.ToString()}");
            sb.AppendLine($"Created:{this.Created}");
            sb.AppendLine($"Compressed:{this.Compressed}");
            sb.AppendLine($"Exported:{this.Exported}");
            sb.AppendLine($"Output File Name:{this.OutputFileName}");
            sb.AppendLine($"Exported File Name:{this.ExportedFileName}"); 
            sb.AppendLine($"FileExport Error :{FileExportError?.ToString()}");
            return sb.ToString();
        }

        #endregion
    }

    public class ExportFileInfoList : SortedDictionary<int, ExportFileInfo>
    {
        #region constructors

        private ExportFileInfoList() : base()
        { }

        public ExportFileInfoList(string stageUri, int totalRows
                                  ,ExportQueryBase exportQueryBase
                                  ,ExportFileOptions exportFileOptions)
        {
            int intKey = 1;
            // should we append date format
            bool AppendDateFlag = exportFileOptions.AppendDateFormat;
            int batchSize = exportFileOptions.MaxRowsPerFile;
            string filePrefix = exportQueryBase.OutputFilePrefix;
            if (totalRows > 0)
            {
                for (int intIdx = 1; intIdx <= totalRows; intIdx = intIdx + batchSize)
                {
                    // get file name
                    string strDate = AppendDateFlag ? FileHelper.GetFormattedDate(exportFileOptions.FileDateFormat.ToString()) : string.Empty;
                    string strFileName = $"{filePrefix}{strDate}.{intKey.ToString()}{exportFileOptions.ExportToFileTypeToString}";
                    strFileName = System.IO.Path.Combine(stageUri, strFileName);
                    // get end row
                    int intEndRow = intIdx + batchSize - 1;
                    // if end row spills more than total rows then reset it to total rows.
                    intEndRow = (totalRows < intEndRow) ? totalRows : intEndRow;
                    // add export file object
                    this.Add(intKey, new ExportFileInfo(strFileName, intIdx, intEndRow, exportFileOptions.CompressionType));
                    intKey++;
                }
            }
        }

         #endregion

        #region public object overriders

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(100);

            sb.AppendLine($"ExportFileList, No of items:{this.Count.ToString()}");
            // loop throug and add string
            foreach (var obj in this)
            {
                sb.AppendLine($"Export File Key:{obj.Key.ToString()}");
                sb.AppendLine(obj.Value.ToString());
            }

            return sb.ToString();
        }

        #endregion
    }

}
