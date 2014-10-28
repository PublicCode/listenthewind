
using System.Collections.Generic;
namespace DWQ.Subject
{
    public class SubjectSearchFormInfo
    {
        public string RefSubjectId { get; set; }

        public string FieldName { get; set; }

        public string ControlId { get; set; }

        public ControlType CurrentControlType { get; set; }

        public int ControlSequence { get; set; }

        public CompareType CurrentCompareType { get; set; }

        public string ControlTitle { get; set; }

        public string CodeTableName { get; set; }

        public int ControlMaxLength { get; set; }

        public string CodeTextFieldName { get; set; }

        public string CodeTextFieldExpression { get; set; }

        public string CodeValueFieldName { get; set; }

        public bool ControlIsShow { get; set; }

        public string DDlGroupName { get; set; }

        public string DDlClickScript { get; set; }

        public string FilterFor { get; set; }

        public string FilterField { get; set; }

        public List<forSelect> listTextValue { get; set; }
    }

    public class forSelect
    {
        public string text{get;set;}
        public string value{get;set;}
    }

}
