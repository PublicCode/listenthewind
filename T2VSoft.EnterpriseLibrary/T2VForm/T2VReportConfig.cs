using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace T2VSoft.EnterpriseLibrary.T2VForm
{
    public class T2VReportConfig
    {
        public string ReportItem { get; set; }

        public List<SearchFieldConfig> SearchFieldConfig { get; set; }

        public T2VReportConfig()
        {
            this.SearchFieldConfig = new List<SearchFieldConfig>();
        }
    }

    public class SearchFieldConfig
    {
        public string DataField { get; set; }

        public string ControlId { get; set; }

        public int ControlType { get; set; }

        public SearchFieldConfig(string dataField, string controlId, int controlType)
        {
            this.DataField = dataField;
            this.ControlId = controlId;
            this.ControlType = controlType;
        }
    }

    public class SearchFieldConfigData
    {
        public string DataField { get; set; }

        public string DataValue { get; set; }

        public SearchFieldConfigData(string dataField, string dataValue)
        {
            this.DataField = dataField;
            this.DataValue = dataValue;
        }
    }

    public class T2VReportConfigData
    {
        public List<SearchFieldConfigData> SearchFieldConfigData { get; set; }

        public T2VReportConfigData()
        {
            this.SearchFieldConfigData = new List<SearchFieldConfigData>();
        }
    }
}
