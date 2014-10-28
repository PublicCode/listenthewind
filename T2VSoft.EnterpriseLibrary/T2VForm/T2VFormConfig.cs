using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace T2VSoft.EnterpriseLibrary.T2VForm
{
    public class T2VFormConfig
    {
        public string GroupName { get; set; }

        public string ControlID { get; set; }

        public string GroupType { get; set; }

        public List<DataFieldConfig> DataFields { get; set; }

        public T2VFormConfig()
        {
            this.DataFields = new List<DataFieldConfig>();
            this.GroupType = "DIV";
        }

        public T2VFormConfig(string groupName, string controlID)
        {
            this.DataFields = new List<DataFieldConfig>();
            this.GroupName = groupName;
            this.ControlID = controlID;
            this.GroupType = "DIV";
        }
    }
}
