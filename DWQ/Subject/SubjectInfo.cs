using System;
using System.Data;

namespace DWQ.Subject
{
    public class SubjectInfo
    {
        public string SubjectId { get; set; }

        public string SubjectTitle { get; set; }

        public string TableName { get; set; }

        public SubjectDetailCollection Details { get; set; }

        public string DDlTableName { get; set; }

        public SubjectInfo(string subjectId)
        {
            this.SubjectId = subjectId;

            DataTable dtSubject = SubjectManager.GetSubjectInfoData(subjectId);
            DataTable dtSubjectDetail = SubjectManager.GetSubjectDetailInfoData(subjectId);            

            SetSubjectInfo(dtSubject);
            SetSubjectDetailInfo(dtSubjectDetail);
        }

        public SubjectInfo(string subjectId, SubjectInitType initType)
        {
            this.SubjectId = subjectId;
            DataTable dtSubject = null;
            DataTable dtSubjectDetail = null;
            dtSubject = SubjectManager.GetSubjectInfoData(subjectId);
            SetSubjectInfo(dtSubject);

            switch (initType)
            {
                case SubjectInitType.SubjectInfo:

                    break;
                case SubjectInitType.SubjectInfoAndDetail:
                    dtSubjectDetail = SubjectManager.GetSubjectDetailInfoData(subjectId);
                    SetSubjectDetailInfo(dtSubjectDetail);
                    break;
                default:
                    break;
            }
        }

        public SubjectInfo(string subjectId, DataSet dsSubject)
        {
            this.SubjectId = subjectId;

            DataTable dtSubject = dsSubject.Tables[SubjectManager.TABLE_NAME_DWQ_SUBJECT];
            DataTable dtSubjectDetail = dsSubject.Tables[SubjectManager.TABLE_NAME_DWQ_SUBJECT_DETAIL].Clone();

            DataRow[] drSelect = dsSubject.Tables[SubjectManager.TABLE_NAME_DWQ_SUBJECT_DETAIL].Select("Ref_Subject_Id = '" + subjectId + "'", "Grid_Col_Sequence");

            for (int i = 0; i < drSelect.Length; i++)
            {
                dtSubjectDetail.ImportRow(drSelect[i]);
            }

            SetSubjectInfo(dtSubject);
            SetSubjectDetailInfo(dtSubjectDetail);
        }

        private void SetSubjectInfo(DataTable dtSubject)
        {
            DataRow drSubject = dtSubject.Rows[0];
            this.SubjectTitle = drSubject["Subject_Title"].ToString();           
            this.TableName = drSubject["Table_Name"].ToString();
            this.DDlTableName = drSubject["DDL_Table_Name"].ToString();
        }

        private void SetSubjectDetailInfo(DataTable dtSubjectDetail)
        {
            if (dtSubjectDetail.Rows.Count > 0)
            {
                this.Details = new SubjectDetailCollection();
            }

            for (int i = 0; i < dtSubjectDetail.Rows.Count; i++)
            {
                DataRow drSubjectDetail = dtSubjectDetail.Rows[i];
                SubjectDetailInfo detail = new SubjectDetailInfo();
                detail.RefSubjectId = this.SubjectId;
                detail.FieldName = drSubjectDetail["Field_Name"].ToString().Trim();
                detail.FieldType = (DbType)Convert.ToInt32(drSubjectDetail["Field_Type"]);
                detail.FieldAlias = drSubjectDetail["Field_Alias"].ToString().Trim();
                detail.CurrentBoundFieldType = (BoundFieldType)Convert.ToInt32(drSubjectDetail["BoundField_Type"]);

                //detail.FieldSize = Convert.ToInt32(drSubjectDetail["Field_Size"]);
                detail.GridHeadText = drSubjectDetail["Grid_Head_Text"].ToString().Trim();
                detail.GridColSequence = Convert.ToInt32(drSubjectDetail["Grid_Col_Sequence"]);
                detail.IsGridShow = Convert.ToBoolean(drSubjectDetail["Is_GridShow"]);
                detail.FieldExpression = drSubjectDetail["Field_Expression"].ToString().Trim();
                detail.CodeKeyFileldName = drSubjectDetail["Code_Key_Field_Name"].ToString().Trim();
                detail.CodeTableName = drSubjectDetail["Code_Table_Name"].ToString().Trim();
                detail.CodeValueFieldName = drSubjectDetail["Code_Value_Field_Name"].ToString().Trim();
                detail.CodeAsName = drSubjectDetail["Code_As_Name"].ToString().Trim();
                detail.OrderFieldName = drSubjectDetail["Order_Field_Name"].ToString().Trim();
                detail.MainTableKeyName = drSubjectDetail["MainTable_Key_Field_Name"].ToString().Trim();
                detail.LinkUrl = drSubjectDetail["Link_Url"].ToString().Trim();
                detail.TableName = drSubjectDetail["Table_Name"].ToString().Trim();

                this.Details.Add(detail);
            }
        }
    }
}
