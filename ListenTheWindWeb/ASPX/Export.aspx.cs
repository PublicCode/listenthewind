using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DWQ.Subject;
using DWQ;
using System.Data;

namespace HDS.QMS.ASPX
{
    public partial class Export : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindExportFieldTable();
        }

        private void BindExportFieldTable()
        {
            string _reportItem = Request.QueryString["hfSearchItem"];
            hfSearchItem.Value = _reportItem;

            string _ctrlData = Request.QueryString["hfCtrlData"];
            hfCtrlData.Value = _ctrlData;

            this.divExportField.InnerHtml = string.Empty;

            string strHtml = string.Empty;
            strHtml += "<table border='0' cellpadding='0' cellspacing='0' class='DWQ_Export_table' width='100%'>";
            strHtml += "<tr>";
            strHtml += "<th style='width:25px;'><input type='checkbox' onclick=\"javascript:SelectAllFields(this)\" /></th>";
            strHtml += "<th style='width:275px;'>Field Name</th>";
            strHtml += "</tr>";
            SubjectInfo subject = new SubjectInfo(_reportItem);
            for (int i = 0; i < subject.Details.Count; i++)
            {
                SubjectDetailInfo detail = subject.Details[i];
                if (detail.IsGridShow)
                {
                    string headText = detail.GridHeadText;

                    strHtml += "<tr FieldName='" + detail.FieldName + "'>";
                    strHtml += "<td style='text-align:center;'><input type='checkbox' /></td>";
                    strHtml += "<td>" + headText + "</td>";
                    strHtml += "</tr>";
                }
            }
            strHtml += "</table>";

            this.divExportField.InnerHtml = strHtml;
        }

    }
}