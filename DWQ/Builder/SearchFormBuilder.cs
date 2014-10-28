using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using DWQ.Subject;
using System.Web.UI.HtmlControls;
using System;
using System.Text;
using System.Web;
using System.Linq;
using System.Web.UI;

namespace DWQ.Builder
{
    public class SearchFormBuilder
    {
        private const int ONE_ROW_CONTROLS_COUNT = 1;

        public static void Build(Table searchForm, string subjectId, Dictionary<string, string> initValue)
        {
            searchForm.Rows.Clear();
            List<SubjectSearchFormInfo> formInfos = SubjectManager.GetSearchFormInfos(subjectId);
            //List<SubjectSearchFormInfo> formInfosForHidden = SubjectManager.GetSearchFormInfosForHidden(subjectId);

            TableCell firstTc = new TableCell();
            firstTc.ID = "firstTc";
            List<Tuple<string,string,string>> filters = new List<Tuple<string,string,string>>();

            for (int i = 0; i < formInfos.Count; ++i)
            {
                if (!string.IsNullOrEmpty(formInfos[i].FilterFor))
                {
                    var filterFors = formInfos[i].FilterFor.Split(',');
                    var filterFields = formInfos[i].FilterField.Split(',');
                    for (int k = 0; k < Math.Min(filterFors.Length, filterFields.Length); ++k)
                    {
                        filters.Add(new Tuple<string, string, string>(formInfos[i].ControlId, filterFors[k], filterFields[k]));
                    }
                }
            }

            for (int i = 0; i < formInfos.Count; i++)
            {
                ControlType type = formInfos[i].CurrentControlType;
                TableCell tc = new TableCell();
                tc.HorizontalAlign = HorizontalAlign.Left;
                if (i == 0)
                {
                    firstTc = tc;
                }

                switch (type)
                {
                    case ControlType.TextBox:
                        {
                            TextBox tb = new TextBox();
                            tb.ID = formInfos[i].ControlId;
                            tb.Width = Unit.Pixel(140);
                            tb.Attributes.Add("class", "inputText01");
                            tb.MaxLength = formInfos[i].ControlMaxLength;

                            if (initValue.ContainsKey(formInfos[i].ControlId))
                            {
                                tb.Text = initValue[formInfos[i].ControlId];
                            }
                            tc.Controls.Add(tb);
                        }
                        break;
                    case ControlType.DropDownList:
                        {
                            DropDownList ddl = new DropDownList();
                            ddl.ID = formInfos[i].ControlId;
                            ddl.Width = Unit.Pixel(100);
                            ddl.Attributes.Add("class", "inputText01");
                            if (formInfos[i].DDlClickScript != string.Empty)
                                ddl.Attributes.Add("onchange", formInfos[i].DDlClickScript);

                            DataTable dt = new DataTable();
                            dt = DWQSearch.GetCodeTable(formInfos[i]);
                            ddl.DataSource = dt;
                            if (formInfos[i].CodeTextFieldName != string.Empty)
                            {
                                ddl.DataTextField = formInfos[i].CodeTextFieldName;
                                ddl.DataValueField = formInfos[i].CodeValueFieldName;
                            }
                            else
                            {
                                ddl.DataValueField = "Code_Name";
                            }

                            ddl.DataBind();
                            ddl.Items.Insert(0, new ListItem("", ""));
                            if (initValue.ContainsKey(formInfos[i].ControlId))
                            {
                                ddl.SelectedValue = initValue[formInfos[i].ControlId];
                            }
                            foreach (ListItem listItem in ddl.Items)
                            {
                                listItem.Attributes["title"] = listItem.Text;
                            }

                            tc.Controls.Add(ddl);
                        }
                        break;
                    case ControlType.DateTime:
                        {
                            TextBox tb = new TextBox();
                            tb.Width = Unit.Pixel(140);
                            tb.Attributes.Add("class", "datefield");
                            tb.ID = formInfos[i].ControlId;
                            tb.SkinID = "DateTime";
                            tb.Attributes.Add("ReadOnly", "true");
                            //tb.Attributes.Add("onload", "this.datepicker({ changeYear: true, changeMonth: true, showButtonPanel: true })");
                            //$('#txtAuthFromDate').datepicker({ changeYear: true, changeMonth: true, showButtonPanel: true });
                            //tb.Attributes.Add("onClick", " WdatePicker({lang:'en'})");
                            if (initValue.ContainsKey(formInfos[i].ControlId))
                            {
                                tb.Text = initValue[formInfos[i].ControlId];
                            }
                            tc.Controls.Add(tb);
                        }
                        break;
                    case ControlType.CheckBox:
                        {
                            CheckBox cb = new CheckBox();
                            cb.Checked = true;
                            cb.ID = formInfos[i].ControlId;
                            tc.Controls.Add(cb);
                        }
                        break;
                    case ControlType.MultipleSelect:
                        {
                            ListBox multipleSelect = new ListBox();
                            multipleSelect.SelectionMode = ListSelectionMode.Multiple;
                            multipleSelect.ID = formInfos[i].ControlId;
                            multipleSelect.Attributes.Add("class", "multipleselect");

                            
                            if (!string.IsNullOrEmpty(formInfos[i].FilterFor))
                            {
                                multipleSelect.AutoPostBack = true;
                                var index = i;
                                multipleSelect.SelectedIndexChanged += (object sender, EventArgs e) => {
                                    var myFilteredItems = filters.Where(c => c.Item1 == formInfos[index].ControlId);
                                    foreach (var j in myFilteredItems)
                                    {
                                        var targetFormInfo = formInfos.Find(c => c.ControlId == j.Item2);
                                        var allFiltersToThis = filters.Where(c => c.Item2 == j.Item2);
                                        DataTable data = DWQSearch.GetCodeTable(targetFormInfo, allFiltersToThis.Select(c => new[] { c.Item3, GetHtmlControlValue(searchForm.FindControl(c.Item1)) }).ToArray());
                                        dynamic target = tc.FindControl(j.Item2);
                                        target.DataSource = data;
                                        target.DataBind();

                                    }
                                };
                            }

                            DataTable dt = DWQSearch.GetCodeTable(formInfos[i]);
                            multipleSelect.DataSource = dt;
                            if (formInfos[i].CodeTextFieldName != string.Empty)
                            {
                                multipleSelect.DataTextField = formInfos[i].CodeTextFieldName;
                                multipleSelect.DataValueField = formInfos[i].CodeValueFieldName;
                            }
                            else
                            {
                                multipleSelect.DataValueField = "Code_Name";
                            }
                            multipleSelect.DataBind();
                            tc.Controls.Add(multipleSelect);
                        }
                        break;
                    default:
                        break;
                }

                if (i % ONE_ROW_CONTROLS_COUNT == 0)
                {
                    searchForm.Rows.Add(new TableRow());
                }
                TableCell tcTitle = new TableCell();
                tcTitle.HorizontalAlign = HorizontalAlign.Left;
                searchForm.Rows[searchForm.Rows.Count - 1].Cells.Add(tcTitle);
                tcTitle.Text = formInfos[i].ControlTitle;
                searchForm.Rows[searchForm.Rows.Count - 1].Cells.Add(tc);

                HiddenField hf = new HiddenField();
                hf.ID = "hf" + formInfos[i].ControlId + subjectId;
                searchForm.Rows[searchForm.Rows.Count - 1].Cells[searchForm.Rows[searchForm.Rows.Count - 1].Cells.Count - 1].Controls.Add(hf);
            }
            
            //for (int i = 0; i < formInfosForHidden.Count; i++)
            //{
            //    HiddenField hf = new HiddenField();
            //    hf.ID = "hf" + formInfosForHidden[i].ControlId + subjectId;
            //    if (initValue.ContainsKey(formInfosForHidden[i].FieldName))
            //    {
            //        hf.Value = initValue[formInfosForHidden[i].FieldName];
            //    }
            //    firstTc.Controls.Add(hf);
            //}
        }

        private static string GetHtmlControlValue(Control control)
        {
            if (control is TextBox)
            {
                return (control as TextBox).Text;
            }
            else if (control is DropDownList)
            {
                return (control as DropDownList).SelectedValue;
            }
            else if (control is ListBox)
            {
                List<string> values = new List<string>();
                foreach (ListItem j in (control as ListBox).Items)
                {
                    if (j.Selected)
                        values.Add(j.Value);
                }
                return string.Join(";", values);
            }
            else
            {
                return string.Empty;
            }
        }

        public static void ReSetHiddenValue(Table searchForm, string subjectId)
        {
            List<SubjectSearchFormInfo> formInfos = SubjectManager.GetSearchFormInfos(subjectId);

            for (int i = 0; i < formInfos.Count; i++)
            {
                string hfId = "hf" + formInfos[i].ControlId + subjectId;
                HiddenField hf = searchForm.FindControl(hfId) as HiddenField;

                ControlType type = formInfos[i].CurrentControlType;
                switch (type)
                {
                    case ControlType.TextBox:
                        {
                            TextBox tb = searchForm.FindControl(formInfos[i].ControlId) as TextBox;
                            tb.Text = string.Empty;
                            hf.Value = tb.Text.Trim();
                        }
                        break;
                    case ControlType.DropDownList:
                        {
                            DropDownList ddl = searchForm.FindControl(formInfos[i].ControlId) as DropDownList;
                            ddl.SelectedIndex = 0;
                            hf.Value = ddl.SelectedValue;
                        }
                        break;
                    case ControlType.DateTime:
                        {
                            TextBox tb = searchForm.FindControl(formInfos[i].ControlId) as TextBox;
                            tb.Text = string.Empty;
                            hf.Value = tb.Text.Trim();
                        }
                        break;
                    case ControlType.CheckBox:
                        {
                            CheckBox cb = searchForm.FindControl(formInfos[i].ControlId) as CheckBox;
                            cb.Checked = true;
                            if (cb.Checked)
                                hf.Value = bool.TrueString;
                            else
                                hf.Value = bool.FalseString;
                        }
                        break;
                    case ControlType.MultipleSelect:
                        {
                            ListBox multipleSelect = searchForm.FindControl(formInfos[i].ControlId) as ListBox;
                            foreach (ListItem li in multipleSelect.Items)
                            {
                                if (li.Selected == true)
                                {
                                    li.Selected = false;
                                }
                            }              
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        public static void SetHiddenValue(Table searchForm, string subjectId)
        {
            List<SubjectSearchFormInfo> formInfos = SubjectManager.GetSearchFormInfos(subjectId);
           
            for (int i = 0; i < formInfos.Count; i++)
            {
                string hfId = "hf" + formInfos[i].ControlId + subjectId;
                HiddenField hf = searchForm.FindControl(hfId) as HiddenField;

                ControlType type = formInfos[i].CurrentControlType;
                switch (type)
                {
                    case ControlType.TextBox:
                        {
                            TextBox tb = searchForm.FindControl(formInfos[i].ControlId) as TextBox;
                            hf.Value = tb.Text.Trim();
                        }
                        break;
                    case ControlType.DropDownList:
                        {
                            DropDownList ddl = searchForm.FindControl(formInfos[i].ControlId) as DropDownList;
                            hf.Value = ddl.SelectedValue;
                        }
                        break;
                    case ControlType.DateTime:
                        {
                            TextBox tb = searchForm.FindControl(formInfos[i].ControlId) as TextBox;
                            hf.Value = tb.Text.Trim();
                        }
                        break;
                    case ControlType.CheckBox:
                        {
                            CheckBox cb = searchForm.FindControl(formInfos[i].ControlId) as CheckBox;
                            if (cb.Checked)
                                hf.Value = bool.TrueString;
                            else
                                hf.Value = bool.FalseString;
                        }
                        break;
                    case ControlType.MultipleSelect:
                        {
                            hf.Value = "";
                            ListBox multipleSelect = searchForm.FindControl(formInfos[i].ControlId) as ListBox;
                            foreach (ListItem li in multipleSelect.Items)
                            {
                                if (li.Selected == true)
                                {
                                    hf.Value += ";" + HttpUtility.HtmlEncode(li.Value);
                                }
                            }                            
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        public static Dictionary<string, string> GetHiddenValue(Table searchForm, string subjectId)
        {
            List<SubjectSearchFormInfo> formInfos = SubjectManager.GetSearchFormInfos(subjectId);
            //List<SubjectSearchFormInfo> formInfosForHidden = SubjectManager.GetSearchFormInfosForHidden(subjectId);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            //for (int i = 0; i < formInfosForHidden.Count; i++)
            //{
            //    string hfId = "hf" + formInfosForHidden[i].ControlId + subjectId;
            //    HiddenField hf = searchForm.FindControl(hfId) as HiddenField;
            //    dict.Add(formInfosForHidden[i].ControlId, hf.Value);
            //}

            for (int i = 0; i < formInfos.Count; i++)
            {
                string hfId = "hf" + formInfos[i].ControlId + subjectId;
                HiddenField hf = searchForm.FindControl(hfId) as HiddenField;
                dict.Add(formInfos[i].ControlId, hf.Value);
            }
            return dict;
        }

        private static bool isFrom(string controlId)
        {

            if (controlId.Substring(controlId.Length - 4, 4) == "From")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
