using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Web;
using DataAccess.Database;
using DWQ.Subject;

namespace DWQ
{
    public class SubjectManager
    {
        private const string SQL_SELECT_DWQ_SUBJECT = "SELECT * FROM dwq_subject ORDER BY Subject_Title";
        private const string SQL_SELECT_DWQ_SUBJECT_DETAIL = "SELECT dwq_subject_detail.*,dwq_subject.table_name FROM dwq_subject_detail,dwq_subject WHERE dwq_subject_detail.ref_subject_id = dwq_subject.subject_id";
        private const string SQL_SELECT_DWQ_SUBJECT_SEARCH_FORM = "SELECT * FROM dwq_subject_search_form";
        private const string SQL_SELECT_DWQ_SUBJECT_FULLTEXT_DETAIL = "SELECT * FROM dwq_subject_fulltext_detail";

        public const string TABLE_NAME_DWQ_SUBJECT = "DWQ_SUBJECT";
        public const string TABLE_NAME_DWQ_SUBJECT_DETAIL = "DWQ_SUBJECT_DETAIL";
        private const string TABLE_NAME_DWQ_SUBJECT_SEARCH_FORM = "DWQ_SUBJECT_SEARCH_FORM";
        private const string TABLE_NAME_DWQ_SUBJECT_FULLTEXT_DETAIL = "DWQ_SUBJECT_FULLTEXT_DETAIL";

        public static DataTable GetSubjectInfoData(string subjectId)
        {
            return GetSubjectInfoDataByTable(TABLE_NAME_DWQ_SUBJECT, subjectId);
        }

        public static DataTable GetSubjectInfoData()
        {
            DataSet dsSubjectInfo = GetAllSubjectInfoData();
            return dsSubjectInfo.Tables[TABLE_NAME_DWQ_SUBJECT];
        }

        public static DataTable GetSubjectDetailInfoData(string subjectId)
        {
            return GetSubjectInfoDataByTable(TABLE_NAME_DWQ_SUBJECT_DETAIL, subjectId);
        }

        public static List<SubjectSearchFormInfo> GetSearchFormInfos(string subjectId="")
        {
            List<SubjectSearchFormInfo> formInfs = new List<SubjectSearchFormInfo>();
            DataTable dtSearchFormInfos = SubjectManager.GetSearchFormInfoData(subjectId);
            for (int i = 0; i < dtSearchFormInfos.Rows.Count; i++)
            {
                DataRow dr = dtSearchFormInfos.Rows[i];
                bool isGridShow = Convert.ToBoolean(Convert.ToInt32(dr["Control_IsShow"].ToString().Trim()));
                if (isGridShow)
                {
                    SubjectSearchFormInfo formInfo = new SubjectSearchFormInfo();
                    if (string.IsNullOrEmpty(subjectId))
                    {
                        formInfo.RefSubjectId = dr["Ref_Subject_Id"].ToString();
                    }
                    else
                    {
                        formInfo.RefSubjectId = subjectId;
                    }
                    formInfo.ControlTitle = dr["Control_Title"].ToString().Trim();
                    formInfo.CodeTableName = dr["Code_Table_Name"].ToString().Trim();
                    formInfo.ControlId = dr["Control_Id"].ToString().Trim();
                    formInfo.ControlIsShow = isGridShow;
                    formInfo.FieldName = dr["Field_Name"].ToString().Trim();
                    formInfo.CurrentControlType = (ControlType)Convert.ToInt32(dr["Control_Type"].ToString().Trim());
                    formInfo.CurrentCompareType = (CompareType)Convert.ToInt32(dr["Compare_Type"].ToString().Trim());
                    //formInfo.ControlMaxLength = Convert.ToInt32(dr["Control_MaxLength"].ToString().Trim());
                    formInfo.CodeTextFieldName = dr["Code_Text_FieldName"].ToString().Trim();
                    formInfo.CodeValueFieldName = dr["Code_Value_FieldName"].ToString().Trim();
                    formInfo.CodeTextFieldExpression = dr["Code_Text_FieldExpression"].ToString().Trim();
                    formInfo.DDlClickScript = dr["DDL_Client_Script"].ToString().Trim();
                    formInfo.DDlGroupName = dr["DDl_Group_Name"].ToString().Trim();
                    formInfo.FilterFor = dr["FilterFor"].ToString(); ;
                    formInfo.FilterField = dr["FilterField"].ToString();

                    formInfs.Add(formInfo);
                }
            }
            return formInfs;
        }

        public static Dictionary<string, SubjectSearchFormInfo> GetDicSearchFormInfos(string subjectId)
        {
            Dictionary<string, SubjectSearchFormInfo> formInfs = new Dictionary<string, SubjectSearchFormInfo>();
            DataTable dtSearchFormInfos = SubjectManager.GetSearchFormInfoData(subjectId);

            for (int i = 0; i < dtSearchFormInfos.Rows.Count; i++)
            {
                DataRow dr = dtSearchFormInfos.Rows[i];
                bool isGridShow = Convert.ToBoolean(Convert.ToInt32(dr["Control_IsShow"].ToString().Trim()));
                if (isGridShow)
                {
                    SubjectSearchFormInfo formInfo = new SubjectSearchFormInfo();
                    formInfo.RefSubjectId = subjectId;
                    formInfo.ControlTitle = dr["Control_Title"].ToString().Trim();
                    formInfo.CodeTableName = dr["Code_Table_Name"].ToString().Trim();
                    formInfo.ControlId = dr["Control_Id"].ToString().Trim();
                    formInfo.ControlIsShow = isGridShow;
                    formInfo.FieldName = dr["Field_Name"].ToString().Trim();
                    formInfo.CurrentControlType = (ControlType)Convert.ToInt32(dr["Control_Type"].ToString().Trim());
                    formInfo.CurrentCompareType = (CompareType)Convert.ToInt32(dr["Compare_Type"].ToString().Trim());
                    formInfo.ControlMaxLength = Convert.ToInt32(dr["Control_MaxLength"].ToString().Trim());
                    formInfo.CodeTextFieldName = dr["Code_Text_FieldName"].ToString().Trim();
                    formInfo.CodeValueFieldName = dr["Code_Value_FieldName"].ToString().Trim();
                    formInfo.CodeTextFieldExpression = dr["Code_Text_FieldExpression"].ToString().Trim();

                    formInfs.Add(dr["Control_Id"].ToString().Trim(), formInfo);
                }
            }
            return formInfs;
        }

        public static List<SubjectFullTextDetailInfo> GetFullTextDetailInfos(string subjectId)
        {
            List<SubjectFullTextDetailInfo> fullTextDetails = new List<SubjectFullTextDetailInfo>();
            DataTable dtFullText = SubjectManager.GetFullTextInfoData(subjectId);

            for (int i = 0; i < dtFullText.Rows.Count; i++)
            {
                DataRow dr = dtFullText.Rows[i];

                SubjectFullTextDetailInfo detail = new SubjectFullTextDetailInfo();
                detail.RefSubjectId = subjectId;
                detail.FieldName = dr["Field_Name"].ToString().Trim();
                detail.FieldAlias = dr["Field_Alias"].ToString().Trim();
                fullTextDetails.Add(detail);
            }
            return fullTextDetails;
        }

        public static DataSet GetAllSubjectInfoData()
        {
            DataSet dsSubject = new DataSet();
            Database db = DatabaseFactory.CreateDatabase();

            string strSql = "SELECT * FROM dwq_subject order by Subject_Sequence";

            using (DbCommand command = db.GetSqlStringCommand(strSql))
            {
                DataTable dt = db.ExecuteDataSet(command).Tables[0];
                DataTable dtDWQSubject = dt.Copy();
                dtDWQSubject.TableName = TABLE_NAME_DWQ_SUBJECT;
                dsSubject.Tables.Add(dtDWQSubject);
            }

            using (DbCommand command = db.GetSqlStringCommand(SQL_SELECT_DWQ_SUBJECT_DETAIL))
            {
                DataTable dt = db.ExecuteDataSet(command).Tables[0];
                DataTable dtDWQSubjectDetail = dt.Copy();
                dtDWQSubjectDetail.TableName = TABLE_NAME_DWQ_SUBJECT_DETAIL;
                dsSubject.Tables.Add(dtDWQSubjectDetail);
            }

            using (DbCommand command = db.GetSqlStringCommand(SQL_SELECT_DWQ_SUBJECT_SEARCH_FORM))
            {
                DataTable dt = db.ExecuteDataSet(command).Tables[0];
                DataTable dtDWQSubjectSearchForm = dt.Copy();
                dtDWQSubjectSearchForm.TableName = TABLE_NAME_DWQ_SUBJECT_SEARCH_FORM;
                dsSubject.Tables.Add(dtDWQSubjectSearchForm);
            }

            using (DbCommand command = db.GetSqlStringCommand(SQL_SELECT_DWQ_SUBJECT_FULLTEXT_DETAIL))
            {
                DataTable dt = db.ExecuteDataSet(command).Tables[0];
                DataTable dtDWQSubjectFullTextDetail = dt.Copy();
                dtDWQSubjectFullTextDetail.TableName = TABLE_NAME_DWQ_SUBJECT_FULLTEXT_DETAIL;
                dsSubject.Tables.Add(dtDWQSubjectFullTextDetail);
            }

            HttpContext.Current.Cache[ConfigurationManager.AppSettings["SubjectInfoCacheName"]] = dsSubject;
            HttpContext.Current.Cache["IsRefreshCache"] = ConfigurationManager.AppSettings["RefreshCache"];

            //}
            return dsSubject;
        }

        private static DataTable GetSearchFormInfoData(string subjectId="")
        {
            return GetSubjectInfoDataByTable(TABLE_NAME_DWQ_SUBJECT_SEARCH_FORM, subjectId);
        }

        private static DataTable GetSubjectInfoDataByTable(string tableName, string subjectId="")
        {
            DataSet dsSubjectInfo = GetAllSubjectInfoData();

            DataTable dtSubjectInfo = dsSubjectInfo.Tables[tableName];

            DataTable dtSelectSubjectInfo = dtSubjectInfo.Clone();
            string subjectIdColName = "Subject_Id";
            if (dtSubjectInfo.Columns.IndexOf(subjectIdColName) < 0)
            {
                subjectIdColName = "Ref_Subject_Id";
            }

            string sortColName = string.Empty;
            switch (tableName)
            {
                case TABLE_NAME_DWQ_SUBJECT:
                    sortColName = "Subject_Sequence";
                    break;
                case TABLE_NAME_DWQ_SUBJECT_DETAIL:
                    sortColName = "Grid_Col_Sequence";
                    break;
                case TABLE_NAME_DWQ_SUBJECT_SEARCH_FORM:
                    sortColName = "Control_Sequence";
                    break;
                default:
                    sortColName = string.Empty;
                    break;
            }

            DataRow[] drSelect;

            if (sortColName == string.Empty)
            {
                if (subjectId != "")
                {
                    drSelect = dtSubjectInfo.Select(subjectIdColName + " = '" + subjectId + "'");
                }
                else
                {
                    drSelect = dtSubjectInfo.Select();
                }
            }
            else
            {
                if (subjectId != "")
                {
                    drSelect = dtSubjectInfo.Select(subjectIdColName + " = '" + subjectId + "'", sortColName);
                }
                else
                {
                    drSelect = dtSubjectInfo.Select("1=1",sortColName);
                }
            }

            for (int i = 0; i < drSelect.Length; i++)
            {
                dtSelectSubjectInfo.ImportRow(drSelect[i]);
            }
            return dtSelectSubjectInfo;
        }



        private static DataTable GetFullTextInfoData(string subjectId)
        {
            return GetSubjectInfoDataByTable(TABLE_NAME_DWQ_SUBJECT_FULLTEXT_DETAIL, subjectId);
        }
    }
}
