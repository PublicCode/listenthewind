using System.Data;
using System.Web.UI.WebControls;
using DWQ.Subject;
using System.Globalization;

namespace DWQ.Builder
{
    public class GridViewBuilder
    {
        public static void Build(GridView gv, string subjectId)
        {
            SubjectInfo subject = new SubjectInfo(subjectId);
            gv.Columns.Clear();

            for (int i = 0; i < subject.Details.Count; i++)
            {
                SubjectDetailInfo detail = subject.Details[i];
                switch (detail.CurrentBoundFieldType)
                {
                    case BoundFieldType.BoundField:
                        SetBoundField(gv, detail,"0");
                        break;
                    case BoundFieldType.BoundFieldRight:
                        SetBoundField(gv, detail,"1");
                        break;
                    case BoundFieldType.HyperLinkField:
                        SetHyperLinkField(gv, detail);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void SetBoundField(GridView gv, SubjectDetailInfo detail,string alignflag)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalDigits = 0;
            BoundField field = new BoundField();

            field.HeaderText = detail.GridHeadText;

            if (detail.CodeAsName != string.Empty)
            {
                field.DataField = detail.CodeAsName;
                field.SortExpression = detail.CodeAsName;
            }
            else
            {
                field.DataField = detail.FieldName;
                field.SortExpression = detail.FieldName;
            }
            field.Visible = detail.IsGridShow;
            field.ItemStyle.Wrap = false;
            field.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            field.ItemStyle.CssClass = "pad";
            if (alignflag == "1")
            {
                field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            }
            DbType type = detail.FieldType;
            if (type == DbType.DateTime)
            {
                field.HtmlEncode = false;
                field.DataFormatString = "{0:MM/dd/yyyy hh:mm:ss}";
            }
            else if (type == DbType.Date)
            {
                field.HtmlEncode = false;
                field.DataFormatString = "{0:MM/dd/yyyy}";
            } 
            else if (type == DbType.Decimal)
            {
                field.HtmlEncode = false;
                field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            }
            else if (type == DbType.Currency)
            {
                field.HtmlEncode = false;
                field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            }
            gv.Columns.Add(field);
        }

        private static void SetHyperLinkField(GridView gv, SubjectDetailInfo detail)
        { 
            HyperLinkField field = new HyperLinkField();

            field.HeaderText = detail.GridHeadText;
            field.Visible = detail.IsGridShow;
            field.DataTextField = detail.FieldName;
            field.SortExpression = detail.FieldName;
            field.ItemStyle.Wrap = false;
            field.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            field.ItemStyle.CssClass = "pad";

            gv.Columns.Add(field);
        }
    }
}
