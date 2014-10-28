using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using T2VSoft.EnterpriseLibrary.Common;
using T2VSoft.EnterpriseLibrary.Data;

namespace T2VSoft.EnterpriseLibrary.FlexiGrid
{
    public class FilterSqlBuilder
    {
        public virtual string GetFilterWhereSql(Dictionary<string, ColumnFilter> colFilters)
        {

            StringBuilder sbSql = new StringBuilder();

            foreach (KeyValuePair<string, ColumnFilter> filter in colFilters)
            {
                ColumnFilter colFilter = filter.Value;

                string sqlColName = colFilter.SqlColName;
                if (colFilter.SqlColName == "")
                {
                    sqlColName = colFilter.ColKey;
                }
                colFilter.SqlColName = sqlColName;

                if (colFilter.Values.Count <= 0 && colFilter.FilterType == ColFilterType.TextSearch)
                {
                    continue;
                }
                else
                {
                    if (colFilter.FilterType == ColFilterType.TextSearch && colFilter.Values[0].Trim() == string.Empty)
                    {
                        colFilter.Values.Clear();
                        continue;
                    }
                }


                if (sbSql.Length > 0 && colFilter.Values.Count > 0)
                {
                    sbSql.Append(" AND ");
                }

                sbSql.Append(this.GetOneColSql(colFilter));


            }

            return sbSql.ToString();
        }

        public virtual string GetAutoCompleteData(string colKey, Dictionary<string, ColumnFilter> colFilters)
        {
            ColumnFilter colFilter = colFilters[colKey];

            if (colFilter.SqlColName == string.Empty) colFilter.SqlColName = colFilter.ColKey;
            if (colFilter.WhereColName == string.Empty) colFilter.WhereColName = colFilter.ColKey;
            string sql = "";
            switch (colFilter.FilterType)
            {
                case ColFilterType.TextSearch:
                    sql = string.Format("select distinct {0} from `{1}` where {0} like '%{2}%'", colFilter.WhereColName, colFilter.TableName, CommonUtil.MySqlEncode(colFilter.Values[0]));
                    if (colFilter.SqlFrom != string.Empty)
                    {
                        sql = string.Format("select distinct {0}  `{1}` where {0} like '%{2}%'", colFilter.WhereColName, colFilter.SqlFrom, CommonUtil.MySqlEncode(colFilter.Values[0]));
                    }
                    break;
                case ColFilterType.CheckBoxList:
                    sql = string.Format("select distinct {0} from `{1}`", colFilter.WhereColName, colFilter.TableName);
                    if (colFilter.SqlFrom != string.Empty)
                    {
                        sql = string.Format("select distinct {0}  `{1}`", colFilter.WhereColName, colFilter.SqlFrom);
                    }
                    break;
                default:
                    break;
            }
            colFilters.Remove(colKey);
            string filterWhereSql = this.GetFilterWhereSql(colFilters);
            if (filterWhereSql != string.Empty)
            {
                if (colFilter.Values.Count == 0 && colFilter.FilterType == ColFilterType.CheckBoxList)
                {
                    sql += " where " + this.GetFilterWhereSql(colFilters);
                }
                else
                {
                    sql += " And " + this.GetFilterWhereSql(colFilters);
                }
            }
            Database db = DatabaseFactory.CreateDatabase();
            DataTable dtData = new DataTable();
            using (DbCommand command = db.GetSqlStringCommand(sql))
            {
                dtData = db.ExecuteDataSet(command).Tables[0];
                return ToAutoCompleteString(dtData, colFilter.SqlColName);
            }
        }



        public string ToAutoCompleteString(DataTable dtData, string trueCol)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dtData.Rows)
            {
                if (sb.Length > 0)
                    sb.Append("\n");
                sb.Append(dr[trueCol].ToString());
            }
            return sb.ToString();
        }


        public virtual string GetOneColSql(ColumnFilter colFilter)
        {
            StringBuilder sbSql = new StringBuilder();

            switch (colFilter.FilterType)
            {
                case ColFilterType.TextSearch:
                    sbSql.Append(colFilter.SqlColName);
                    sbSql.Append(" Like '%");
                    sbSql.Append(CommonUtil.MySqlEncode(colFilter.Values[0]));
                    sbSql.Append("%'");
                    break;
                case ColFilterType.CheckBoxList:
                    if (!colFilter.CheckBoxListIsSelectAll)
                    {
                        if (colFilter.Values.Count > 0)
                        {
                            sbSql.Append(colFilter.SqlColName);
                            sbSql.Append(" in(");
                            for (int i = 0; i < colFilter.Values.Count; i++)
                            {
                                sbSql.Append("'" + CommonUtil.MySqlEncode(colFilter.Values[i]) + "',");
                            }
                            sbSql.Remove(sbSql.Length - 1, 1);
                            sbSql.Append(")");
                        }
                        else
                        {
                            sbSql.Append(" 1=2");
                        }
                    }
                    break;
                default:
                    break;
            }
            return sbSql.ToString();
        }

        private string JoinString(List<string> list)
        {
            string str = string.Empty;
            for (int i = 0; i < list.Count; i++)
            {
                str += "'" + list[i] + "',";
            }
            if (str != string.Empty)
            {
                str = str.Substring(0, str.Length - 1);
            }
            return str;
        }

    }
}
