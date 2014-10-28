<%@ Page Title="" Language="C#" MasterPageFile="~/ASPX/ReportMasterPage.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="Reports.aspx.cs" Inherits="Web.ASPX.Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <script type="text/javascript" language="javascript">
        $(function () {
            var para = t2v_main.request('fromModule');
            if (para == 1) {
                $('#divReportLeft').css('width', $(window).width() - 310 + "px");
                $(window).resize(function () { $('#divReportLeft').css('width', $(window).width() - 310 + "px"); });
                $(".QuoteMenu").css("display", "none");
            } else {
                $('#divReportLeft').css('width', $(window).width() - 510 + "px");
                $(window).resize(function () { $('#divReportLeft').css('width', $(window).width() - 510 + "px"); });
            }
            $(".datefield").datepicker({ changeYear: true, changeMonth: true, showButtonPanel: true,
                onClose: function (e) {
                    var ev = window.event;
                    if (ev.srcElement.innerHTML == 'Clear')
                        this.value = "";
                },
                closeText: 'Clear',
                buttonText: ''
            });
            
            $(".multipleselect").multiselect({
                header: true,
                selectedList: 5,
                close: function () {
                    
                },
                lateTriggering: true
            });
        });
    </script>
</asp:Content>
<asp:Content ID = "LeftContent" ContentPlaceHolderID ="CPHLeftContent" runat  ="server">
             <td id="controlLeftDiv" runat ="server" style="width: 200px; border-bottom: solid 1px #dbdbdb; vertical-align: top; table-layout:fixed;" class="left_nav">
               <% Web.ASPX.WebFormMvcUtil.RenderPartial("~/Views/Shared/LeftMenu.cshtml", new Dictionary<string, object> { { "controller", "Report" }, { "action", "Report" } }); %>
                </td>
                <td id="controlBarDiv" runat = "server" style="width: 8px; border: 1px solid #dbdbdb; border-width: 1px 1px 1px 0;">
                    <div id="mainBar" class="mainBarLeft" onclick="t2v_main.ControlDiv();">
                        &nbsp;
                    </div>
                </td>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHMainContent" runat="server">
    <form id="form2" runat="server" defaultbutton="btnSearch">
    <%--<%
        
        var user = Web.Energizer.User.UserHelper.CurrentUser;
        var userId = user.ID;
        int userGroup = user.GroupID.Value;
        var userSubGroup = user.SubRoleName;

        if (userGroup == 2 || (userSubGroup != "23" && userSubGroup != "24"))
        {
            Response.Redirect("/Queue/Index");
        }
         %>--%>
    <table cellpadding="0" cellspacing="0" id="obody" class="divLog" style="width: 100%;">
        <tr>
            <td style="text-align: left; vertical-align: top; border: 1px solid #dddddd; border-right-width: 0;">
                <div id="divReportLeft" style="overflow: auto; height: 460px;">
                    <div id ="divNoData" runat="server" style="text-align: center; height: 450px; padding-top: 250px;">
                                <span style="font-family: Verdana; font-size: 20px; font-weight: bold; color: #666">
                                    No Result Found</span></div>
                    <asp:GridView ID="gvSearchResult" runat="server" PageSize="20"
                        AutoGenerateColumns="False" OnPreRender="gvSearchResult_PreRender" CssClass="DWQ_Results_table"
                        Item="Other" AllowPaging="true" 
                        OnRowDataBound="gvSearchResult_RowDataBound" 
                        onpageindexchanging="gvSearchResult_PageIndexChanging">
                        <EmptyDataTemplate>
                            <div style="text-align: center; height: 450px; padding-top: 250px;">
                                <span style="font-family: Verdana; font-size: 20px; font-weight: bold; color: #666">
                                    No Result Found</span></div>
                        </EmptyDataTemplate>
                        <PagerTemplate>
                            <table cellpadding="0" cellspacing="0" class="DWQ_Results_pager_table" style="text-align: left;">
                                <tr>
                                    <td>
                                        Page:<asp:Label ID="lblPageIndex" runat="server" Text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1  %>' />
                                        /<asp:Label ID="lblPageCount" runat="server" Text='<%# ((GridView)Container.Parent.Parent).PageCount  %>' />
                                        <asp:LinkButton ID="cmdFirstPage" runat="server" CommandName="Page" CommandArgument="First"
                                            Enabled="<%# ((GridView)Container.Parent.Parent).PageIndex!=0 %>">First</asp:LinkButton>
                                        <asp:LinkButton ID="cmdPreview" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled="<%# ((GridView)Container.Parent.Parent).PageIndex!=0 %>">Prev</asp:LinkButton>
                                        <asp:LinkButton ID="cmdNext" runat="server" CommandName="Page" CommandArgument="Next"
                                            Enabled="<%# ((GridView)Container.Parent.Parent).PageIndex!=((GridView)Container.Parent.Parent).PageCount-1 %>">Next</asp:LinkButton>
                                        <asp:LinkButton ID="cmdLastPage" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled="<%# ((GridView)Container.Parent.Parent).PageIndex!=((GridView)Container.Parent.Parent).PageCount-1 %>">Last</asp:LinkButton>&nbsp;&nbsp;
                                        <a href="javascript:void(0)" onclick="OpenExportPopWindow('Export.aspx?a='+Math.random());">Extract</a>
                                    </td>
                                </tr>
                            </table>
                        </PagerTemplate>
                        <PagerSettings Position="Top" />
                        <RowStyle Wrap="false" />
                    </asp:GridView>
<%--                    <asp:ObjectDataSource ID="odsSearch" runat="server" EnablePaging="True" TypeName="DWQ.DWQSearch"
                        SelectCountMethod="SearchCount" SelectMethod="Search" OnSelecting="odsSearch_Selecting">
                        <SelectParameters>
                            <asp:Parameter Name="subjectId" Type="String" />
                            <asp:Parameter Name="ctrlValue" Type="Object" />
                            <asp:Parameter Name="isReset" Type="Boolean" />
                            <asp:Parameter Name="isFullTextSearch" Type="Boolean" />
                            <asp:Parameter Name="fullTextData" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>--%>
                </div>
            </td>
            <td id="tdRight" style="width:240px; vertical-align: top; text-align: center; border: 1px solid #dddddd; padding:0 4px;">
                <div style="border-bottom: 2px dashed #999; padding:7px 0; text-align:left">
                    <b>Reports:</b>
                    <asp:DropDownList ID="ddlItem" runat="server" Width="160px" CssClass="ddlReportItem" onselectedindexchanged="ddlItem_SelectedIndexChanged" ></asp:DropDownList>
                </div>
                <br />
                <asp:Table ID="tbSearchForm" runat="server" CssClass="FilterArea" CellPadding="0" CellSpacing="0" Width="240px">
                </asp:Table>
                <br />
                <br />
                <div style="text-align: center;">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="DWQ_Button_02"
                        OnClick="btnSearch_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="DWQ_Button_02" OnClick="btnClear_Click" />
                </div>
                <br />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hfSearchItem" runat="server" />
    <asp:HiddenField ID="hfCtrlData" runat="server" />
    </form>
</asp:Content>
