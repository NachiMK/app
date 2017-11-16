using System;
using System.Collections.Generic;
using System.Text;

namespace DataSeeder.Export.Library
{
    public class ExportDataResult
    {
        public ExportQueryBase ExportedQuery { get; private set; }
        public ExportFileInfoList ExportedFiles { get; internal set; }
        public Exception ExportError { get; set; }

        private ExportDataResult() { }

        public ExportDataResult(ExportQueryBase exportQueryBase, ExportFileInfoList exportFileInfoList = null)
        {
            this.ExportedQuery = exportQueryBase ?? throw new ArgumentNullException("exportQueryBase", "Please provide a Query/Table that was exported.");
            this.ExportedFiles = exportFileInfoList;
        }

        public ExportDataResult(ExportTable exportTable, ExportFileInfoList exportFileInfoList = null)
        {
            this.ExportedQuery = exportTable ?? throw new ArgumentNullException("exportTable", "Please provide a Table that was exported.");
            this.ExportedFiles = exportFileInfoList;
        }

        public ExportDataResult(ExportQuery exportQuery, ExportFileInfoList exportFileInfoList = null)
        {
            this.ExportedQuery = exportQuery ?? throw new ArgumentNullException("exportQuery", "Please provide a Query that was exported.");
            this.ExportedFiles = exportFileInfoList;
        }

        public override string ToString()
        {
            return $"ExportedQuery : {this.ExportedQuery?.ToString()} ExportedFiles : {this.ExportedFiles?.ToString()}";
        }
    }

    public class ExportDataResults : List<ExportDataResult>
    {
        public ExportDataResults() : base() { }
        public ExportDataResults(int capacity) : base(capacity) { }
        public ExportDataResults(IEnumerable<ExportDataResult> collection) : base(collection) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(100);
            sb.AppendLine($"Export Data List, No of items:{this.Count.ToString()}");
            // loop throug and add string
            foreach (var obj in this)
                sb.AppendLine($"Export Data Result:{obj.ToString()}");

            return sb.ToString();
        }
    }
}
