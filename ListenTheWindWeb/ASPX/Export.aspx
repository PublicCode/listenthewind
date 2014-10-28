<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Export.aspx.cs" Inherits="HDS.QMS.ASPX.Export" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
<script src="../Scripts/Vendor/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="../Script/Vendor/jquery-ui-1.8.20.min.js" type="text/javascript"></script>
<script src="../Scripts/t2v-dwq.js" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" media="screen" href="../Content/Default.css" />
<link rel="stylesheet" type="text/css" media="screen" href="../Content/DWQ.css" />
</head>
<body">
    <form id="form1" runat="server">
    <table border="0" cellpadding="3" cellspacing="3">
        <tr>
            <td align="left">
                <select id="selExportType" class="DWQ_DropDown_01" style="width: 140px;">
                    <%--<option value="CSV">CSV</option>--%>
                    <option value="CSV COMPRESS">CSV COMPRESS</option>
                    <%--<option value="EXCEL">EXCEL</option>--%>
                </select>
                <input type="button" id="btnExport" value="Export" class="DWQ_Button_01" onclick="Export('divExportField');" />
            </td>
        </tr>
        <tr>
            <td>
                <div id="divExportField" runat="server">
                </div>
            </td>
        </tr>
    </table>
     <asp:HiddenField ID="hfSearchItem" runat="server" />
     <asp:HiddenField ID="hfCtrlData" runat="server" />
    </form>
</body>
</html>
