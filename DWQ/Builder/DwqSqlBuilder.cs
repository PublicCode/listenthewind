using System;
using System.Collections.Generic;
using System.Text; 
using DWQ.Subject;
using System.Data;
using System.Linq;
using System.Web;
namespace DWQ.Builder
{
    public class DwqSqlBuilder
    {
        public static string GetDwqSqlForPage(string subjectId, Dictionary<string, string> ctrlValue, bool isReset, bool isFullTextSearch, string fullTextData, int maximumRows, int startRowIndex)
        {
            StringBuilder sql = DwqSqlBuilder.GetDwqSqlStringBuilder(subjectId, ctrlValue, isReset, isFullTextSearch, fullTextData, null);
            sql.Append(" Where a.RowNumber BETWEEN ");
            sql.Append(startRowIndex.ToString());
            sql.Append(" AND ");
            sql.Append((startRowIndex + maximumRows).ToString());

            return sql.ToString();
        }
        public static string GetDwqSqlForPage(string subjectId, Dictionary<string, string> ctrlValue, bool isReset, bool isFullTextSearch, string fullTextData)
        {
            StringBuilder sql = DwqSqlBuilder.GetDwqSqlStringBuilder(subjectId, ctrlValue, isReset, isFullTextSearch, fullTextData, null);
            return sql.ToString();
        }

        public static string GetExportSql(string subjectId, Dictionary<string, string> ctrlValue)
        {
            StringBuilder sql = DwqSqlBuilder.GetDwqSqlStringBuilder(subjectId, ctrlValue, false, false, string.Empty, null); ;
            return sql.ToString();
        }

        public static string GetDwqSqlForPageCount(string subjectId, Dictionary<string, string> ctrlValue, bool isReset, bool isFullTextSearch, string fullTextData)
        {
            SubjectInfo subjectInfo = new SubjectInfo(subjectId);
            StringBuilder leftJoinStr = new StringBuilder();

            string sql = "SELECT Count(*) FROM " + subjectInfo.TableName;

            if (isReset)
            {
                sql += " Where 1=2 ";
            }
            else
            {
                sql += BuildWhereSql(subjectId, ctrlValue);
            }
            return sql;
        }

        public static StringBuilder GetDwqSqlForExport(SubjectSearchArgs searchArgs)
        {
            StringBuilder sql = new StringBuilder();
            sql = DwqSqlBuilder.GetDwqSqlExportStringBuilder(searchArgs.SubjectId, searchArgs.CtrlValue, false, searchArgs.IsFullTextSearch, searchArgs.FullTextData, searchArgs.ExportFields);
            return sql;
        }

        public static string GetDwqSqlForExportHead(SubjectSearchArgs searchArgs)
        {
            string sql = "select Grid_Head_Text as HeadText from dwq_subject_detail where Ref_Subject_Id='" + searchArgs.SubjectId + "' and Field_Name in ('" + string.Join("','", searchArgs.ExportFields.ToArray()) + "') order by Grid_Col_Sequence ";
            return sql;
        }

        public static string GetDwqSqlForExportCSV(SubjectSearchArgs searchArgs)
        {
            StringBuilder sql = new StringBuilder();
            sql = DwqSqlBuilder.GetDwqSqlStringBuilderIfNull(searchArgs.SubjectId, searchArgs.CtrlValue, false, searchArgs.IsFullTextSearch, searchArgs.FullTextData, searchArgs.ExportFields);

            return sql.ToString();
        }

        public static string GetDwqSqlForCodeTable(SubjectSearchFormInfo formInfo, params string[][] prereqs)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select distinct " + formInfo.CodeValueFieldName + " from " + formInfo.CodeTableName + " where " + formInfo.CodeValueFieldName + "  is not null and " + formInfo.CodeValueFieldName + " <> ''");
            if (prereqs != null)
            {
                foreach (var j in prereqs)
                {
                    if (string.IsNullOrEmpty(j[1])) 
                        continue;
                    sql.Append(" and ");
                    var splitted = j[1].Replace("'","''").Split(';');
                    if (splitted.Length > 1)
                    {
                        sql.Append(string.Format("{0} in ({1})", j[0], string.Join(",", splitted.Select(c => "'" + c + "'"))));
                    }
                    else
                    {
                        sql.Append(string.Format("{0} = '{1}'", j[0], j[1]));
                    }
                }
            }

            sql.Append(" order by " + formInfo.CodeValueFieldName);
            return sql.ToString();
        }

        private static StringBuilder GetDwqSqlStringBuilderIfNull(string subjectId, Dictionary<string, string> ctrlValue, bool isReset, bool isFullTextSearch, string fullTextData, List<string> fields)
        {
            SubjectInfo subjectInfo = new SubjectInfo(subjectId);
            StringBuilder sql = new StringBuilder();
            string orderBy = string.Empty;

            sql.Append(" SELECT ");

            for (int i = 0; i < subjectInfo.Details.Count; i++)
            {
                SubjectDetailInfo detail = subjectInfo.Details[i];

                if (fields != null)
                {
                    if (!fields.Contains(detail.FieldName))
                    {
                        continue;
                    }
                }
                if (i == 0)
                    orderBy = detail.FieldName;
                sql.Append("IFNULL(");
                sql.Append(subjectInfo.TableName);
                sql.Append(".");
                sql.Append(detail.FieldName);
                sql.Append(",''),");
            }

            if (isReset)
            {
                sql.Append(" Where 1=2 ");
            }
            else
            {
                sql.Append(BuildWhereSql(subjectId, ctrlValue));
            }
            return sql;
        }

        private static StringBuilder GetDwqSqlExportStringBuilder(string subjectId, Dictionary<string, string> ctrlValue, bool isReset, bool isFullTextSearch, string fullTextData, List<string> fields)
        {
            SubjectInfo subjectInfo = new SubjectInfo(subjectId);
            StringBuilder sql = new StringBuilder();
            string orderBy = string.Empty;

            sql.Append(" SELECT ");

            for (int i = 0; i < subjectInfo.Details.Count; i++)
            {
                SubjectDetailInfo detail = subjectInfo.Details[i];

                if (fields != null)
                {
                    if (!fields.Contains(detail.FieldName))
                    {
                        continue;
                    }
                }
                if (i == 0)
                    orderBy = detail.FieldName;
                else
                {
                    if (orderBy == string.Empty)
                        orderBy = detail.FieldName;
                }
                sql.Append(subjectInfo.TableName);
                sql.Append(".");
                sql.Append(detail.FieldName);
                sql.Append(",");
            }
            sql.Remove(sql.Length - 1, 1);

            sql.Append(" FROM ");
            sql.Append(subjectInfo.TableName);

            if (isReset)
            {
                sql.Append(" Where 1=2 ");
            }
            else
            {
                sql.Append(BuildWhereSql(subjectId, ctrlValue));
            }
            //sql.Append(GetWhereRestriction());
            return sql;
        }

        private static StringBuilder GetDwqSqlStringBuilder(string subjectId, Dictionary<string, string> ctrlValue, bool isReset, bool isFullTextSearch, string fullTextData, List<string> fields)
        {
            SubjectInfo subjectInfo = new SubjectInfo(subjectId);
            StringBuilder sql = new StringBuilder();
            string orderBy = string.Empty;

            sql.Append(" SELECT * FROM (SELECT ");

            for (int i = 0; i < subjectInfo.Details.Count; i++)
            {
                SubjectDetailInfo detail = subjectInfo.Details[i];

                if (fields != null)
                {
                    if (!fields.Contains(detail.FieldName))
                    {
                        continue;
                    }
                }

                if (i == 0)
                    orderBy = detail.FieldName;
                else
                {
                    if (orderBy != string.Empty)
                        orderBy = detail.FieldName;
                }
                if (detail.FieldType == DbType.Date)
                {
                    sql.Append("convert(varchar(100), ");
                }
                sql.Append(subjectInfo.TableName);
                sql.Append(".");
                sql.Append(detail.FieldName);
                if (detail.FieldType == DbType.Date)
                {
                    sql.Append(", 101) as " + detail.FieldName);
                }
                sql.Append(",");
            }
            //sql.Remove(sql.Length - 1, 1);

            sql.Append(" ROW_NUMBER() OVER (ORDER BY ID) AS 'RowNumber' FROM ");
            sql.Append(subjectInfo.TableName);

            if (isReset)
            {
                sql.Append(" Where 1=2 ");
            }
            else
            {
                sql.Append(BuildWhereSql(subjectId, ctrlValue));
            }
            sql.Append(" ) a");
            return sql;
        }
        
        public static string BuildWhereSql(string subjectId, Dictionary<string, string> ctrlValue)
        {
            List<SubjectSearchFormInfo> formInfos = SubjectManager.GetSearchFormInfos(subjectId);

            string sql = " WHERE ";

            if (formInfos.Count == 0)
            {
                return string.Empty;
            }

            for (int i = 0; i < formInfos.Count; i++)
            {
                SubjectSearchFormInfo formInfo = formInfos[i];
                string compareSql = string.Empty;
                if (formInfo.ControlId == "ddlAgingType")
                {
                    var readyValue = ctrlValue[formInfo.ControlId];

                    
                    if (readyValue.Trim() == "Open Balance Plus WIP")
                    {
                        compareSql = " (BalIncWIP is not null and BalIncWIP > 0)";
                    }
                    else if (readyValue.Trim() == "Open Balance Only")
                    {
                        compareSql = " (Balance is not null and Balance > 0)";
                    }
                }
                else if (formInfo.ControlId == "chkExclude")
                {
                    var readyValue = ctrlValue[formInfo.ControlId];
                    if (readyValue == "True")
                    {
                        compareSql = "CreateYearMonth <= (select Max([AccMth]) from [Authorization])";
                    }
                    
                }
                else
                {
                    compareSql = GetCompareSql(formInfo, ctrlValue);
                }
                if (compareSql != string.Empty)
                    sql += compareSql + " AND ";
            }

            if (sql != " WHERE ")
            {

                sql = sql.Substring(0, sql.Length - 4);
            }
            if (sql == " WHERE ")
            {
                return string.Empty;
            }
            return sql;
        }

        private static string GetFullTextSql(string subjectId, Dictionary<string, string> ctrlValue, SubjectSearchFormInfo formInfo)
        {
            var readyValue = Encoding.Default.GetString(Convert.FromBase64String(ctrlValue[formInfo.ControlId]));
            string sql = "(";
            SubjectInfo subjectInfo = new SubjectInfo(subjectId);
            for (int j = 0; j < subjectInfo.Details.Count; j++)
            {
                SubjectDetailInfo detail = subjectInfo.Details[j];
                sql += detail.FieldName + " LIKE '%" + readyValue.Trim() + "%' OR ";
            }
            sql = sql.Substring(0, sql.Length - 3);
            sql += ")";
            return sql;
        }
        
        private static string GetCompareSql(SubjectSearchFormInfo formInfo, Dictionary<string, string> ctrlValue)
        {
            string sql = " " + formInfo.FieldName + "";
            var readyValue = ctrlValue[formInfo.ControlId];
            if (string.IsNullOrEmpty(readyValue.Trim()) || readyValue == "null" )
            {
                return string.Empty;
            }

            switch (formInfo.CurrentCompareType)
            {
                case CompareType.EqualTo:
                    {
                        if (readyValue.Trim() == "Null")
                        {
                            sql += " is null ";
                        }
                        else
                        {
                            sql += " = '" + readyValue.Trim() + "' ";
                        }
                    }
                    break;
                case CompareType.GreaterThan:
                    sql += " >= '" + readyValue.Trim() + "' ";
                    break;
                case CompareType.LessThan:
                    string data = string.Empty;
                    if (formInfo.CurrentControlType == ControlType.DateTime)
                    {
                        DateTime dt = Convert.ToDateTime(readyValue);
                        dt = dt.AddDays(1.0);
                        data = dt.ToString("yyyy-MM-dd");
                        sql += " < '" + data + "' ";
                    }
                    else
                    {
                        data = readyValue.Trim();
                        sql += " <= '" + data + "' ";
                    }
                    break;
                case CompareType.Contains:
                    sql += " LIKE '%" + readyValue.Trim() + "%' ";
                    break;
                case CompareType.MultipleContains:
                    {
                        if (readyValue.Trim() == string.Empty)
                            return string.Empty;

                        string[] sqlSplit;
                        if (readyValue.IndexOf(';') != -1)
                        {
                            sqlSplit = readyValue.Trim(';').Split(';');
                        }
                        else if (readyValue.IndexOf(',') != -1)
                        {
                            sqlSplit = readyValue.Split(',');
                        }
                        else
                        {
                            sqlSplit = readyValue.Trim(';').Split(';');
                        }
                        
                        
                        if (sqlSplit.Length > 0)
                        {
                            sql = " (" + formInfo.FieldName + "";
                            if (sqlSplit[0] == "NULL")
                            {
                                sql += " = ' '";
                            }
                            else
                            {
                                sql += " = '" + HttpUtility.HtmlDecode(sqlSplit[0]) + "'";
                                for (int i = 1; i < sqlSplit.Length; i++)
                                {
                                    sql += " or " + formInfo.FieldName + "";
                                    if (sqlSplit[i] == "NULL")
                                    {
                                        sql += " = ' '";
                                    }
                                    else
                                    {
                                        sql += " = '" + HttpUtility.HtmlDecode(sqlSplit[i]) + "'";
                                    }
                                }
                            }
                            sql += ")";
                        }
                        else
                        {
                            sql += " = '" + HttpUtility.HtmlDecode(sqlSplit[0]) + "'";
                        }
                    }
                    break;
                case CompareType.Empty:
                    sql = string.Empty;
                    break;
                default:
                    break;
            }

            return sql;
        }
    }
}
