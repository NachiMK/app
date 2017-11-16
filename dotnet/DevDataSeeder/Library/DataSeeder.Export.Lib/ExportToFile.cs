using System;
using Export.Common.Library;
using System.Data;
using System.IO;
using System.Text;

namespace DataSeeder.Export.Library
{
    public enum ExportTypeEnum
    {
        ExportTables,
        ExportQueries
    }

    public class ExportToFile
    {
        public string SQLConnectionString { get; private set; }

        public string StagingUri { get; private set; }

        public string DestinationUri { get; private set; }

        private ExportToFile()
        {

        }

        private ExportToFile(string sqlConnString, string destinationUri, string stagingUri = "")
        {
            this.SQLConnectionString = sqlConnString;
            this.DestinationUri = destinationUri;
            this.StagingUri = stagingUri;
        }

        public static ExportToFile NewExportToFile(string sqlConnString, string destinationUri, string stagingUri = @"")
        {
            SQLHelper.IsValidDBConnString(sqlConnString, true);
            FileHelper.IsValidDirectoryPath(destinationUri);
            if (!string.IsNullOrEmpty(stagingUri))
                FileHelper.IsValidDirectoryPath(stagingUri);

            ExportToFile exportToFile = new ExportToFile(sqlConnString, destinationUri, stagingUri);

            return exportToFile;
        }

        #region public members

        public ExportDataResults ExportTables(ExportTableList exportTableList, ExportFileOptions exportFileOptions)
        {
            ExportDataResults exportResults = new ExportDataResults(exportTableList.Count);
            ExportDataResult exportOutcome = null;
            foreach(var e in exportTableList)
            {
                exportOutcome = new ExportDataResult(e, null);
                try
                {
                    exportOutcome = ExportSingleTable(e, exportFileOptions);
                }
                catch(Exception ex)
                {
                    exportOutcome.ExportError = ex;
                }
                exportResults.Add(exportOutcome);
            }
            return exportResults;
        }

        public ExportDataResult ExportSingleTable(ExportTable export, ExportFileOptions exportFileOptions)
        {
            ExportDataResult exportResult = new ExportDataResult(export, null);

            // Log that validation has passed
            ExportDataTable _ExportDataTable = new ExportDataTable();
            _ExportDataTable.Table = SQLHelper.ExecuteDataTable(this.SQLConnectionString, export.SQLCommandText, out Exception outEx);

            exportResult.ExportError = outEx;
            // throw exception if we got one from SQL layer
            if (outEx == null)
                // create a file and export it
                exportResult.ExportedFiles = CreateFiles(_ExportDataTable, export, exportFileOptions);

            //ExportFileInfoList exportFileInfoList = new ExportFileInfoList(this.StagingUri, totalRows, export, exportFileOptions);
            return exportResult;
        }

        public ExportDataResults ExportQueryOutput(ExportQueryList exportQueryList, ExportFileOptions exportFileOptions)
        {
            ExportDataResults exportResults = new ExportDataResults(exportQueryList.Count);
            ExportDataResult exportOutcome = null;
            foreach (var e in exportQueryList)
            {
                exportOutcome = new ExportDataResult(e, null);
                try
                {
                    exportOutcome = ExportSingleQueryOutput(e, exportFileOptions);
                }
                catch (Exception ex)
                {
                    exportOutcome.ExportError = ex;
                }
                exportResults.Add(exportOutcome);
            }
            return exportResults;
        }

        public ExportDataResult ExportSingleQueryOutput(ExportQuery exportQuery, ExportFileOptions exportFileOptions)
        {
            ExportDataResult exportResult = new ExportDataResult(exportQuery, null);

            // Log that validation has passed
            ExportDataTable _ExportDataTable = new ExportDataTable();
            _ExportDataTable.Table = SQLHelper.ExecuteDataTable(this.SQLConnectionString, exportQuery.SQLCommandText, out Exception outEx);

            exportResult.ExportError = outEx;
            // throw exception if we got one from SQL layer
            if (outEx == null)
                // create a file and export it
                exportResult.ExportedFiles = CreateFiles(_ExportDataTable, exportQuery, exportFileOptions);

            //ExportFileInfoList exportFileInfoList = new ExportFileInfoList(this.StagingUri, totalRows, export, exportFileOptions);
            return exportResult;
        }

        #endregion

        #region private methods

        private ExportFileInfoList CreateFiles(ExportDataTable exportDataTable, ExportQueryBase exportQueryBase, ExportFileOptions exportFileOptions)
        {
            ExportFileInfoList exportFileList = null;

            if ((exportDataTable == null) || (exportDataTable?.Table == null))
                throw new ArgumentNullException("Data Table is missing", $"Data Table to export data is null{exportQueryBase.ToString()}");

            int intTotalRows = exportDataTable.Table.Rows.Count;

            // figure out batch size
            int maxRowsPerFile = exportFileOptions.SplitFiles ? exportFileOptions.MaxRowsPerFile : intTotalRows;
            // Files split by Max Rows per file.
            exportFileList = new ExportFileInfoList(this.StagingUri, intTotalRows, exportQueryBase, exportFileOptions);

            if (exportFileList == null)
                throw new InvalidDataException($"Export File List is null for:{exportQueryBase.ToString()}");

            // FOR EACH FILE
            foreach (var e in exportFileList)
            {
                // Add Encoding information if needed
                // Create a new Stream
                Encoding encoding = exportFileOptions.FileEncoding;
                e.Value.Created = false;
                e.Value.Compressed = false;
                e.Value.Exported = false;

                // Split the file writing into chunks in case it is too big.
                int maxRowsInMemory = exportFileOptions.MaxRowsInMemory;
                if (e.Value.NoOfRows > maxRowsInMemory)
                {
                    for (int intChunkStart = 1; intChunkStart <= e.Value.NoOfRows;)
                    {
                        // Rows to be added for each file.
                        using (MemoryStream memStream = exportDataTable.RowsToStream(intChunkStart, maxRowsInMemory, exportFileOptions))
                        {
                            // write to file.
                            FileHelper.CreateFile(e.Value.FileName, memStream, exportFileOptions.CompressionType.ToString());
                        }

                        // next set
                        intChunkStart += maxRowsInMemory;
                    }
                }
                else
                {
                    // Add Header for each file if needed
                    // Rows to be added for each file.
                    using (MemoryStream memStream = exportDataTable.RowsToStream(e.Value.StartRow, e.Value.NoOfRows, exportFileOptions))
                    {
                        // write to file.
                        FileHelper.CreateFile(e.Value.FileName, memStream, exportFileOptions.CompressionType.ToString());
                    }
                }

                if (FileHelper.CheckFileExists(e.Value.OutputFileName))
                {
                    e.Value.Created = true;
                    e.Value.Compressed = (exportFileOptions.CompressionType != FileCompressionTypeEnum.None);
                }
                else
                {
                    e.Value.FileExportError = new FileNotFoundException($"File didnt get created! {e.Value.FileName}");
                }

                bool CopyResultFlag = false;
                e.Value.ExportedFileName = string.Empty;

                // if file exists then export it
                if ((e.Value.Created) && (this.DestinationUri.Length > 0))
                {
                    try
                    {
                        CopyResultFlag = FileHelper.CopyFileTo(e.Value.OutputFileName, this.DestinationUri);
                    }
                    catch (Exception ex)
                    {
                        e.Value.FileExportError = ex;
                        CopyResultFlag = false;
                    }

                    if (CopyResultFlag)
                    {
                        string strUri = (string.IsNullOrEmpty(this.StagingUri)) ? "" : this.StagingUri;
                        e.Value.ExportedFileName = $"{this.DestinationUri}{e.Value.OutputFileName.Replace(strUri, "")}";
                        e.Value.Exported = CopyResultFlag;
                    }
                }
            }

            // return file list and status
            return exportFileList;

        }

        #endregion

    }
}
