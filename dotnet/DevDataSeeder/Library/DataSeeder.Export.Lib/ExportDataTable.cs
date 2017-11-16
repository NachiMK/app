using System;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;

namespace DataSeeder.Export.Library
{
    public class ExportDataTable
    {

        #region private members

        private DataTable _myDataTable = null;
        private string _header;

        /// <summary>
        /// Text qualifies the fields and headers
        /// </summary>
        /// <param name="FieldSeparator"></param>
        /// <param name="IncludeTextQualifier"></param>
        /// <param name="TheColumn"></param>
        /// <returns></returns>
        private string ColumnToString(string FieldSeparator, bool IncludeTextQualifier, object TheColumn)
        {
            string strTextQual = IncludeTextQualifier ? "\"" : "";
            string strColValue = string.Empty;

            if (TheColumn == null)
                strColValue = "";
            else if (TheColumn is INullable && ((INullable)TheColumn).IsNull)
                strColValue = "";
            else if (TheColumn is DateTime)
            {
                if (((DateTime)TheColumn).TimeOfDay.TotalSeconds == 0)
                    strColValue = ((DateTime)TheColumn).ToString("yyyy-MM-dd");
                strColValue = ((DateTime)TheColumn).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
                strColValue = TheColumn.ToString();

            if (strTextQual.Length == 0)
                return strColValue + FieldSeparator;

            return strTextQual + strColValue.Replace("\"", "\"\"") + strTextQual + FieldSeparator;
        }

        private DataTable GetDataTableRange(int StartRow, int NoOfRows)
        {
            //var rows = dt.AsEnumerable().Skip(Fetch_FromRow - 1).Take(Fetch_ToRow);
            return _myDataTable.AsEnumerable().Skip(StartRow - 1).Take(NoOfRows).CopyToDataTable();
            //return dt.AsEnumerable().Where(r => dt.Rows.IndexOf(r) >= Fetch_FromRow && dt.Rows.IndexOf(r) <= Fetch_ToRow).AsDataView().ToTable();
        }

        #endregion

        #region constructor

        #endregion

        #region Properties

        public DataTable Table
        {
            get
            {
                return _myDataTable;
            }
            set
            {
                _myDataTable = value;
            }
        }

        #endregion

        #region public functions

        /// <summary>
        /// Returns the Header of given Data Table.
        /// </summary>
        /// <param name="FieldSeparator"></param>
        /// <param name="IncludeTextQualifier"></param>
        /// <returns></returns>
        public string Header(string FieldSeparator, bool IncludeTextQualifier)
        {
            StringBuilder sbData = new StringBuilder(1000);

            if (string.IsNullOrEmpty(_header) && (_myDataTable != null))
            {
                foreach (var col in _myDataTable.Columns)
                {
                    // Text Qualify and Escape double quotes
                    sbData.Append(ColumnToString(FieldSeparator, IncludeTextQualifier, col));
                }

                // Replace last Comma with New Line
                sbData.Replace(FieldSeparator.ToString(), System.Environment.NewLine, sbData.Length - 1, 1);

                // store header
                _header = sbData.ToString();
            }

            return _header;
        }

        /// <summary>
        /// Returns all rows in data table as one big string.
        /// </summary>
        /// <param name="ColDelimiter"></param>
        /// <param name="IncludeTextQualifier"></param>
        /// <returns></returns>
        internal string RowsToString(string ColDelimiter, bool IncludeTextQualifier, bool IncludeHeaders)
        {
            string strRowDelimiter = System.Environment.NewLine;
            return RowsToString(ColDelimiter, IncludeTextQualifier, _myDataTable.Rows.Count, _myDataTable.Rows.Count, IncludeHeaders, strRowDelimiter);
        }

        internal string RowsToString(string ColDelimiter, bool IncludeTextQualifier, int StartRow, int NoOfRows, bool IncludeHeaders)
        {
            string strRowDelimiter = System.Environment.NewLine;
            return RowsToString(ColDelimiter, IncludeTextQualifier, StartRow, NoOfRows, IncludeHeaders, strRowDelimiter);
        }

        /// <summary>
        /// Returns some rows in data table as one big string.
        /// If Start and NOOfRows are same then all rows are returned.
        /// 
        /// If NoOfRows is greater then # of rows in data table then all
        /// rows in data table from start is returned
        /// </summary>
        /// <param name="ColDelimiter"></param>
        /// <param name="IncludeTextQualifier"></param>
        /// <param name="StartRow"></param>
        /// <param name="NoOfRows"></param>
        /// <returns></returns>
        internal string RowsToString(string ColDelimiter, bool IncludeTextQualifier, int StartRow, int NoOfRows, bool IncludeHeaders, string Rowdelimiter)
        {
            StringBuilder sbData = new StringBuilder(1000);
            DataTable miniDT = null;

            // Get Rows from Data Table only if it is for a chunck of records.
            if (StartRow >= NoOfRows)
                miniDT = _myDataTable;
            else
                miniDT = GetDataTableRange(StartRow, NoOfRows);

            if (miniDT != null)
            {
                // Convert column data into CSV/Txt rows.
                foreach (DataRow dr in miniDT.Rows)
                {
                    foreach (var column in dr.ItemArray)
                    {
                        sbData.Append(ColumnToString(ColDelimiter, IncludeTextQualifier, column));
                    }
                    if ((ColDelimiter.Length > 0) && (Rowdelimiter.Length > 0))
                        sbData.Replace(ColDelimiter, Rowdelimiter, sbData.Length - 1, 1);
                }
            }

            //return
            return sbData.ToString();
        }

        public MemoryStream RowsToStream(int startRow, int maxRowsInMemory, ExportFileOptions exportFileOptions)
        {
            string ColDelimiter = exportFileOptions.ColDelimiterString;
            bool IncludeTextQualifier = exportFileOptions.IncludeTextQualifiers;
            int StartRow = startRow;
            int NoOfRows = maxRowsInMemory;
            Encoding FileEncoding = exportFileOptions.FileEncoding;
            bool IncludeHeaders = exportFileOptions.IncludeHeaders;
            string Rowdelimiter = exportFileOptions.RowDelimiterString;

            DataTable miniDT = null;
            MemoryStream memStream = null;
            StringBuilder sbData = null;

            // get row count in data table
            int intTotalRowCount = ((this.Table != null) && (this.Table.Rows != null)) ? this.Table.Rows.Count : 0;

            // Get Rows from Data Table only if it is for a chunck of records.
            if (intTotalRowCount <= NoOfRows)
                miniDT = this.Table;
            else
                miniDT = GetDataTableRange(StartRow, NoOfRows);

            if (miniDT != null)
            {
                memStream = new MemoryStream(1000);
                StreamWriter streamWriter = new StreamWriter(memStream, FileEncoding);
                try
                {
                    // write header if needed
                    if (IncludeHeaders)
                    {
                        streamWriter.Write(this.Header(ColDelimiter, IncludeTextQualifier));
                    }

                    // Convert column data into CSV/Txt rows.
                    foreach (DataRow dr in miniDT.Rows)
                    {
                        sbData = new StringBuilder(100);
                        foreach (var column in dr.ItemArray)
                        {
                            sbData.Append(ColumnToString(ColDelimiter, IncludeTextQualifier, column));
                        }
                        if ((ColDelimiter.Length > 0) && (Rowdelimiter.Length > 0))
                            sbData.Replace(ColDelimiter, Rowdelimiter, sbData.Length - 1, 1);

                        // write to the stream
                        streamWriter.Write(sbData.ToString());
                    }

                    // flush
                    streamWriter.Flush();
                }
                catch (Exception ex)
                {
                    // dispose memory stream
                    memStream.Dispose();
                    // dispose stream
                    streamWriter.Dispose();
                    throw ex;
                }
            }

            // return
            return memStream;
        }

        #endregion
    }
}
