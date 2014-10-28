using System.Collections.Generic;

namespace T2VSoft.EnterpriseLibrary.FlexiGrid
{
    public enum ColFilterType
    {
        TextSearch,
        CheckBoxList
    }


    public class ColumnFilter
    {
        public string ColKey { get; set; }

        public string SqlColName { get; set; }

        public List<string> Values { get; set; }

        public ColFilterType FilterType { get; set; }

        public bool CheckBoxListIsSelectAll { get; set; }

        public string SqlFrom { get; set; }
        public string TableName { get; set; }
        public string WhereColName { get; set; }

        public string WhereSql { get; set; }


        public ColumnFilter()
        {
            this.Values = new List<string>();
            this.ColKey = "";
            this.FilterType = ColFilterType.TextSearch;
            this.SqlColName = "";
            this.WhereColName = "";
            this.SqlFrom = "";
            this.TableName = "";
            this.WhereSql = "";

        }

        public ColumnFilter(string colKey)
        {
            this.Values = new List<string>();
            this.ColKey = colKey;
            this.FilterType = ColFilterType.TextSearch;
            this.SqlColName = colKey;
            this.WhereColName = colKey;
            this.SqlFrom = "";
            this.TableName = "";
            this.WhereSql = "";
        }

        public ColumnFilter(string colKey, string sqlColName)
        {
            this.Values = new List<string>();
            this.ColKey = colKey;
            this.FilterType = ColFilterType.TextSearch;
            this.SqlColName = sqlColName;
            this.WhereColName = colKey;
            this.SqlFrom = "";
            this.TableName = "";
            this.WhereSql = "";
        }

        public ColumnFilter(string colKey, string sqlColName, string tableName)
        {
            this.Values = new List<string>();
            this.ColKey = colKey;
            this.FilterType = ColFilterType.TextSearch;
            this.SqlColName = sqlColName;
            this.WhereColName = colKey;
            this.SqlFrom = "";
            this.TableName = tableName;
            this.WhereSql = "";
        }

    }
}
