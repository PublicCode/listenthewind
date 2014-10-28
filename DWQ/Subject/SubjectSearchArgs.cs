using System.Collections.Generic;

namespace DWQ.Subject
{
    public class SubjectSearchArgs
    {
        public string SubjectId { get; set; }

        public Dictionary<string,string> CtrlValue { get; set; }

        public bool IsFullTextSearch { get; set; }

        public string FullTextData { get; set; }

        public string SortField { get; set; }

        public List<string> ExportFields { get; set; }

        public List<string> FilterCols { get; set; }

        public int DataCount { get; set; }        
    }
}
