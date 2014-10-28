using System.Collections.Generic;

namespace DWQ.ModelUtil
{
    public class DropDownListData
    {
        public string Id { get; set; }

        public List<DropDownListDataSub> Sub { get; set; }
    }

    public class DropDownListDataSub
    {
        public string Value { get; set; }
    }
}
