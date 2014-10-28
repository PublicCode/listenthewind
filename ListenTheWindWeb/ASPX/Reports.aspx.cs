using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DWQ;
using DWQ.Builder;
using DWQ.Subject;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Web.ASPX
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Count>0)
            {
                controlLeftDiv.Visible = false;
                controlBarDiv.Visible = false;
            }
            if (!Page.IsPostBack)
            {
                InitDropDownList();
                this.btnSearch.Enabled = false;
                this.btnClear.Enabled = false;
                hfSearchItem.Value = ddlItem.SelectedValue;
            }
            if (ddlItem.SelectedValue != "0")
            {
                SearchFormBuilder.Build(this.tbSearchForm, ddlItem.SelectedValue, GetInitValue());
                this.btnSearch.Enabled = true;
                this.btnClear.Enabled = true;
            }

        }

        private void InitDropDownList()
        {
            DataTable dt = SubjectManager.GetSubjectInfoData().Copy();
            this.ddlItem.AutoPostBack = true;
            this.ddlItem.DataSource = dt;
            this.ddlItem.DataTextField = "Subject_Title";
            this.ddlItem.DataValueField = "Subject_Id";
            this.ddlItem.DataBind();
            //ListItem li = new ListItem("--select--", "0");
            //ddlItem.Items.Insert(0, li);
            this.ddlItem.SelectedValue = "quotelog";
        }

        private Dictionary<string, string> GetInitValue()
        {
            Dictionary<string, string> initValue = new Dictionary<string, string>();
            List<SubjectSearchFormInfo> list = SubjectManager.GetSearchFormInfos(ddlItem.SelectedValue);
            foreach (SubjectSearchFormInfo info in list)
            {
                initValue.Add(info.ControlId, string.Empty);
            }
            return initValue;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected void gvSearchResult_PreRender(object sender, EventArgs e)
        {
            if (this.gvSearchResult.Rows.Count > 0)
            {
                this.gvSearchResult.TopPagerRow.Visible = true;
            }
        }
        protected void odsSearch_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Dictionary<string, string> ctrlValue = SearchFormBuilder.GetHiddenValue(this.tbSearchForm, ddlItem.SelectedValue);
            //gvSearchResult.PageIndex = _pageIndex;
            e.InputParameters["isReset"] = false;
            e.InputParameters["subjectId"] = ddlItem.SelectedValue;
            e.InputParameters["ctrlValue"] = ctrlValue;
            e.InputParameters["isFullTextSearch"] = false; ;
            e.InputParameters["fullTextData"] = string.Empty;
        }
        protected void gvSearchResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            divNoData.Visible = false;
            this.gvSearchResult.PageIndex = 0;
            GridViewBuilder.Build(this.gvSearchResult, ddlItem.SelectedValue);
            SearchFormBuilder.SetHiddenValue(this.tbSearchForm, ddlItem.SelectedValue);
            Dictionary<string, string> ctrlValue = SearchFormBuilder.GetHiddenValue(this.tbSearchForm, ddlItem.SelectedValue);
            DataTable dt = DWQSearch.Search(ddlItem.SelectedValue,ctrlValue,false,false,string.Empty,1000000,0);
            gvSearchResult.DataSource = dt;
            this.gvSearchResult.DataBind();
            hfCtrlData.Value = string.Join(";", ctrlValue.Select(c => c.Key + ":" + Convert.ToBase64String(Encoding.Default.GetBytes(c.Value))));
        }
        
        protected void btnClear_Click(object sender, EventArgs e)
        {
            SearchFormBuilder.ReSetHiddenValue(this.tbSearchForm, ddlItem.SelectedValue);
            GridViewBuilder.Build(this.gvSearchResult, ddlItem.SelectedValue);
            SearchFormBuilder.Build(this.tbSearchForm, ddlItem.SelectedValue, GetInitValue());
            this.gvSearchResult.DataBind();
            this.gvSearchResult.PageIndex = 0;
        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlItem.SelectedValue != "0")
            {
                GridViewBuilder.Build(this.gvSearchResult, ddlItem.SelectedValue);
                SearchFormBuilder.ReSetHiddenValue(this.tbSearchForm, ddlItem.SelectedValue);
                //SearchFormBuilder.Build(this.tbSearchForm, ddlItem.SelectedValue, GetInitValue());
                this.gvSearchResult.DataSource = null;
                //this.gvSearchResult = null;
                this.gvSearchResult.DataBind();
            }
            hfSearchItem.Value = ddlItem.SelectedValue;
            Dictionary<string, string> ctrlValue = SearchFormBuilder.GetHiddenValue(this.tbSearchForm, ddlItem.SelectedValue);
            hfCtrlData.Value = string.Join(";", ctrlValue.Select(c => c.Key + ":" + c.Value));
        }


        protected void gvSearchResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSearchResult.PageIndex = e.NewPageIndex;
            GridViewBuilder.Build(this.gvSearchResult, ddlItem.SelectedValue);
            SearchFormBuilder.SetHiddenValue(this.tbSearchForm, ddlItem.SelectedValue);
            Dictionary<string, string> ctrlValue = SearchFormBuilder.GetHiddenValue(this.tbSearchForm, ddlItem.SelectedValue);
            DataTable dt = DWQSearch.Search(ddlItem.SelectedValue, ctrlValue, false, false, string.Empty, 1000000, 0);
            gvSearchResult.DataSource = dt;
            this.gvSearchResult.DataBind();
        }

    }
}