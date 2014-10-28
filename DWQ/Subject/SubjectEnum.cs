
namespace DWQ.Subject
{
    public enum SubjectInitType
    {
        SubjectInfo = 0,
        SubjectInfoAndDetail = 1
    }

    public enum CompareType
    {
        //EqualTo = 1,
        //Like = 2,
        //GreaterThan = 3,
        //LessThan = 4,
        //Contains = 5,
        //FullText = 6,
        //Empty = 10,
        //MultipleLike = 11
        EqualTo = 1,
        Contains = 2,
        LikeLeft = 3,
        LikeRight = 4,
        GreaterThan = 5,
        GreaterThanEqualTo = 6,
        LessThan = 7,
        LessThanEqualTo = 8,
        MultipleEqualTo = 9,
        Empty = 10,
        DoubleFiledsCompared = 11,
        MultipleContains = 12,
        ContainsOrMultipleEqualTo = 13,
        DoubleFiledsComparedFirst = 14,
        MultipleNotEqualTo = 15,
        FiledAGreaterThanFiledB = 16,
        FiledALessThanFiledB = 17,
        MultipleLeftContains = 18,
        CustomType = 20
   }

    public enum ControlType
    {
        //HtmlCell = 1,
        //Label = 2,
        //TextBox = 3,
        //DropDownList = 4,
        //DateTime = 5,
        //CheckBox = 6,
        //HyperLink = 7,
        //HiddenField = 8,
        //RadioButton = 9,
        //MultipleSelect=10
        HtmlCell = 1,
        Label = 2,
        TextBox = 3,
        DateTime = 4,
        CheckBox = 5,
        RadioButton = 6,
        DropDownList = 7,
        MustDropDownList = 8,
        MultipleSelect = 9,
        HiddenField = 10,
        HyperLink = 11,
        DoubleFiledsCompared = 12,
        SAPSystemStatus = 13,
        CustomType = 20
    }

    public enum BoundFieldType
    {
        BoundField = 1,
        HyperLinkField = 2,
        BoundFieldRight = 3
    }
}
