using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DWQ;
using DWQ.Subject;
using IDataAccessLayer;
using Newtonsoft.Json;

namespace DataAccessLayer
{
    public class ReportManager:IReportManager
    {
        public object GetReport(string strSubject,Dictionary<string, string> ctrlValue, string username, string permissions, int currentPageIndex, int pageSize)
        {
            return this.GetReport(strSubject,ctrlValue, currentPageIndex, pageSize);
        }

        private object GetReport(string strSubject,Dictionary<string, string> ctrlValue, int currentPageIndex, int pageSize)
        {
            var data = this.GetGetReportData(strSubject,ctrlValue);

            DataTable newDT = data.Copy();
            newDT.Clear();

            for (int i = (currentPageIndex - 1) * pageSize; i <= (currentPageIndex - 1) * pageSize +pageSize-1; i++)
            {
                DataRow newdr = newDT.NewRow();
                if (i >= data.Rows.Count)
                {
                    break;
                }
                DataRow dr = data.Rows[i];
                foreach (DataColumn column in data.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newDT.Rows.Add(newdr);
            }
            
            //var res = data.Skip((currentPageIndex - 1) * pageSize).Take(pageSize).ToList();

            int count = data.Rows.Count;
            return new
            {
                total = pageSize > 0 ? Math.Ceiling((double)count / pageSize) : 1,
                page = currentPageIndex,
                records = count,
                rows = ConverDtToRows(newDT)
            };
        }

        private string ConverDtToRows(DataTable newDT)
        {
            StringBuilder strJsonBuilder = new StringBuilder();
            int k = 0;
            strJsonBuilder.Append("[");
            foreach (DataRow dr in newDT.Rows)
            {
                strJsonBuilder.Append("{");
                int j = 0;
                foreach (DataColumn dc in newDT.Columns)
                {
                    var textObj = dr[dc.ToString()];
                    if (textObj != null)
                    {
                        textObj = textObj.ToString().Replace("\"", "\\\"");
                    }
                    strJsonBuilder.Append(dc.ToString() + ":\"" + textObj + "\"");
                    if (j != newDT.Columns.Count - 1)
                    {
                        strJsonBuilder.Append(",");
                    }
                    j++;
                }
                strJsonBuilder.Append("}");
                if (k != newDT.Rows.Count - 1)
                {
                    strJsonBuilder.Append(",");
                }
                k++;
            }
            strJsonBuilder.Append("]");
            return strJsonBuilder.ToString();
        }

        private DataTable GetGetReportData(string strSubject,Dictionary<string, string> ctrlValue)
        {
            DataTable dt = DWQSearch.Search(strSubject,ctrlValue, false, false, string.Empty, 1000000, 0);
            return dt;
        }

        public string GetSubjectAndSearchFormData()
        {
            StringBuilder buildJson = new StringBuilder();

            //compose subject
            buildJson.Append("{subject:");
            buildJson.Append("[");
            string strSubjectID = "";
            DataTable dtSubject = SubjectManager.GetSubjectInfoData();
            int i = 0;

            if (dtSubject.Rows.Count > 0)
            {
                strSubjectID = dtSubject.Rows[0]["Subject_Id"].ToString();
            }
            foreach (DataRow dr in dtSubject.Rows)
            {
                DataTable dtDetail = SubjectManager.GetSubjectDetailInfoData(dr["Subject_Id"].ToString());
                string strDetail ="";
                int k=0;
                
                foreach(DataRow drDetailRow in dtDetail.Rows)
                {
                    strDetail += "{text:\""+drDetailRow["Field_Name"].ToString()+"\",value:\""+drDetailRow["Grid_Head_Text"].ToString()+"\"}"; ;
                    if(k != dtDetail.Rows.Count -1)
                    {
                        strDetail+= ",";
                    }
                    k++;
                }

                buildJson.Append("{text:\"" + dr["Subject_Title"] + "\",value:\"" + dr["Subject_Id"] + "\",detail:[" + strDetail + "]}");
                
                if (i != dtSubject.Rows.Count - 1)
                {
                    buildJson.Append(",");
                }
                i++;
            }
            buildJson.Append("],");

            //compose search form data

            List<SubjectSearchFormInfo> formInfos = SubjectManager.GetSearchFormInfos();

            for (int j = 0; j < formInfos.Count; j++)
            {
                switch (formInfos[j].CurrentControlType)
                {
                    case (ControlType.DropDownList):
                        {
                            DataTable dt = new DataTable();
                            dt = DWQSearch.GetCodeTable(formInfos[j]);
                            List<forSelect> listForSelect = new List<forSelect>();
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (formInfos[j].CodeTextFieldName != string.Empty)
                                {
                                    forSelect f = new forSelect();
                                    f.text = dr[formInfos[j].CodeTextFieldName].ToString();
                                    f.value = dr[formInfos[j].CodeValueFieldName].ToString();
                                    listForSelect.Add(f);
                                }
                                else
                                {
                                    forSelect f = new forSelect();
                                    f.text = dr["Code_Name"].ToString();
                                    f.value = dr["Code_Name"].ToString();
                                    listForSelect.Add(f);
                                }
                            }

                            formInfos[j].listTextValue = listForSelect;
                        }
                        break;
                    case ControlType.MultipleSelect:
                        {
                            DataTable dt = new DataTable();
                            dt = DWQSearch.GetCodeTable(formInfos[j]);
                            List<forSelect> listForSelect = new List<forSelect>();
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (formInfos[j].CodeTextFieldName != string.Empty)
                                {
                                    forSelect f = new forSelect();
                                    f.text = dr[formInfos[j].CodeTextFieldName].ToString();
                                    f.value = dr[formInfos[j].CodeValueFieldName].ToString();
                                    listForSelect.Add(f);
                                }
                                else
                                {
                                    forSelect f = new forSelect();
                                    f.text = dr["Code_Name"].ToString();
                                    f.value = dr["Code_Name"].ToString();
                                    listForSelect.Add(f);
                                }
                            }

                            formInfos[j].listTextValue = listForSelect;
                        }
                        break;
                    default: 
                    break;
                }
            }
            
            string strJson = JsonConvert.SerializeObject(formInfos);
            buildJson.Append("searchfield:"+strJson+"}");
            return buildJson.ToString();

        }
             
    }
}
