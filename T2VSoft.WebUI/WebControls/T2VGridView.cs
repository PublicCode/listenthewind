using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace T2VSoft.WebUI.WebControls
{
    public class T2VGridView : GridView
    {
        public T2VGridView() { }

        protected override void OnInit(EventArgs e)
        {
            this.PagerSettings.Position = PagerPosition.Top;
            base.OnInit(e);
        }

        protected override void OnRowCommand(GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Previous":
                    PageIndex--;
                    break;
                case "Next":
                    PageIndex++;
                    break;
            }

            base.OnRowCommand(e);
        }

        protected override void OnRowCreated(GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                Table tblPager = e.Row.Cells[0].Controls[0] as Table;
                tblPager.Rows[0].Cells.AddAt(0, GetCellForPageCount());
                tblPager.Rows[0].Cells.AddAt(1, GetCellForPreButton());
                tblPager.Rows[0].Cells.Add(GetCellForNextButton());

                TableCell tcBlank = new TableCell();
                tcBlank.Width = 50;
                tblPager.Rows[0].Cells.Add(tcBlank);
                tblPager.Rows[0].Cells.Add(GetCellForExportLink());
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < Columns.Count; i++)
                {
                    string[] text = e.Row.Cells[i].Text.Split('?');
                    e.Row.Cells[i].Text = text[0];
                    e.Row.Cells[i].ToolTip = text[0];
                }
            }
            base.OnRowCreated(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (this.Rows.Count > 0)
            {
                TopPagerRow.Visible = true;
            }

            base.OnPreRender(e);
        }

        private TableCell GetCellForPreButton()
        {
            TableCell tc = new TableCell();
            LinkButton linkBtPre = new LinkButton();
            linkBtPre.CommandName = "Previous";
            linkBtPre.Text = "Previous";
            tc.Controls.Add(linkBtPre);
            linkBtPre.Enabled = true;
            if (this.PageIndex == 0)
            {
                linkBtPre.Enabled = false;
            }
            return tc;
        }

        private TableCell GetCellForExportLink()
        {
            TableCell tc = new TableCell();
            HyperLink hylExport = new HyperLink();
            hylExport.Text = "Extract";
            hylExport.ID = "hylExport";
            hylExport.NavigateUrl = "javascript:OpenExportPopWindow('ExportNew.aspx?a='+Math.random())";
            tc.Controls.Add(hylExport);

            return tc;
        }

        private TableCell GetCellForPageCount()
        {
            TableCell tc = new TableCell();
            Label lblPageCount = new Label();
            lblPageCount.Text = "Page: " + (this.PageIndex + 1).ToString() + "/" + this.PageCount.ToString();
            Literal litBlankPageCountPre = new Literal();
            litBlankPageCountPre.Text = "&nbsp&nbsp&nbsp";
            tc.Controls.Add(litBlankPageCountPre);

            tc.Controls.Add(lblPageCount);

            Literal litBlankPageCountLast = new Literal();
            litBlankPageCountLast.Text = "&nbsp&nbsp&nbsp";
            tc.Controls.Add(litBlankPageCountLast);
            return tc;
        }

        private TableCell GetCellForNextButton()
        {
            TableCell tc = new TableCell();

            LinkButton linkBtNext = new LinkButton();
            linkBtNext.CommandName = "Next";
            linkBtNext.Text = "Next";
            tc.Controls.Add(linkBtNext);

            linkBtNext.Enabled = true;

            if (this.PageIndex == this.PageCount - 1)
            {
                linkBtNext.Enabled = false;
            }
            return tc;
        }
    }
}
