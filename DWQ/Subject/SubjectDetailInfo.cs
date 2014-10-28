using System.Data;

namespace DWQ.Subject
{
    public class SubjectDetailInfo
    {
        public string RefSubjectId { get; set; }

        public string FieldName { get; set; }

        public string FieldAlias { get; set; }

        public DbType FieldType { get; set; }

        public BoundFieldType CurrentBoundFieldType { get; set; }

        public int FieldSize { get; set; }

        public string FieldExpression { get; set; }

        public string GridHeadText { get; set; }

        public int GridColSequence { get; set; }

        public bool IsGridShow { get; set; }

        public string CodeKeyFileldName { get; set; }

        public string CodeTableName { get; set; }

        public string CodeValueFieldName { get; set; }

        public string CodeAsName { get; set; }

        public string OrderFieldName { get; set; }

        public string MainTableKeyName { get; set; }

        public string LinkUrl { get; set; }

        public string TableName { get; set; }
    }
}
