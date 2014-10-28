var SiteRoot = t2v_util.history.GetRootPath();
var t2v_RMA = {
    _showLoading : function () {
        $('#divMainPageDiv').hide();
        $('#loading').show();
    },
    _hideLoading : function () {
        $('#loading').hide();
        $('#divMainPageDiv').show();
    },
    initGrid: function () {
        var colInfo = t2v_RMA.GetConifigData();
        var tempColInfo = new Array();
        //colInfo.push({ isShowExportExcel: 'true', Handler: "" });
        FetureV7Grid.myParam(colInfo, "divList", 20, "/RMA/GetRMAList", "", "");
        FetureV7Grid.showGrid();
    },
    ShowRMAListDiv:function()
    {
        $("#divShowRMASelect").css("left", screen.width / 2 - 60).css("height", "400px").css("width", "800px");
        $('#divShowRMASelect').modal('show');
        t2v_RMA.initRMAGrid();
    },
    initRMAGrid :function()
    {
        var colInfo = t2v_RMA.GetConfigData();
        FetureV7Grid.myParam(colInfo, "divRMASelect", 20, "/RMA/GetRMAList", "", "");
        FetureV7Grid.showGrid();
    },
    GetConifigData: function () {
        var colInfo = new Array();
        //colInfo.push({ Width: '40', disType: 'checkbox', keyColumn: "QuoteID" });
        colInfo.push({ disName_en: 'Link', sortOrder: 0, columnName: 'Msgs', Width: '20', disType: 'a', isFilterControl: 'false', operatorType: 'like', onclickFunction: t2v_Quote.CheckInfo, linkparam: 'QuoteID', gridUsedFor: 'Detail' });
        colInfo.push({ disName_en: 'Request ID', sortOrder: 3, columnName: 'QuoteNumber', Width: '40', disType: 'text' });
        colInfo.push({ disName_en: 'Quote Number', sortOrder: 3, columnName: 'FinalQuoteNumber', Width: '40', disType: 'text' });
        colInfo.push({ disName_en: 'Customer Type', sortOrder: 3, columnName: 'CustomerTypeName', Width: '40', disType: 'text' });
        colInfo.push({ disName_en: 'Customer Name', sortOrder: 3, columnName: 'AccountName', Width: '40', disType: 'text' });
        colInfo.push({ disName_en: 'Status', sortOrder: 3, columnName: 'Status', Width: '40', disType: 'text' });
        //colInfo.push({ disName_en: 'Partner Name', sortOrder: 3, columnName: 'PartnerName', Width: '40', disType: 'text' });

        colInfo.push({ disName_en: 'Quote Amt', sortOrder: 3, columnName: 'QuoteAmt', Width: '40', disType: 'text', align: 'right', decimal: 2 });
        colInfo.push({ disName_en: 'CIP Amt', sortOrder: 3, columnName: 'WIPAmt', Width: '40', disType: 'text', align: 'right', decimal: 2 });
        colInfo.push({ disName_en: 'Paid Amt', sortOrder: 3, columnName: 'PaidAmt', Width: '40', disType: 'text', align: 'right', decimal: 2 });
        colInfo.push({ disName_en: 'Balance', sortOrder: 3, columnName: 'Balance', Width: '40', disType: 'text', align: 'right', decimal: 2 });
        colInfo.push({ disName_en: 'Request Date', sortOrder: 3, columnName: 'CreateDate',dateFormat: 'MM/dd/yyyy', Width: '40', disType: 'date' });
        colInfo.push({ disName_en: 'Quote Date', sortOrder: 3, columnName: 'QuotedDate', dateFormat: 'MM/dd/yyyy', Width: '40', disType: 'date' });
        colInfo.push({ disName_en: 'ExpiryDate', sortOrder: 3, columnName: 'ExpirationDate', dateFormat: 'MM/dd/yyyy', Width: '40', disType: 'date' });
        colInfo.push({ disName_en: 'QuoteID', sortOrder: 1, columnName: 'QuoteID', Width: '40', disType: 'text', align: 'right', isHidden: true });
        return colInfo;
    },
    CheckInfo: function ()
    { },

    ShowArrow:function(divId)
    {
        var boolResult =  $("#" + divId).data('fading');
        if (boolResult == false || boolResult == undefined) {
            $("#" + divId).data('fading', true);
            $("#" + divId).fadeIn('fast', function () { $("#" + divId).data('fading', false) });
            $("#" + divId+"Delete").fadeIn('fast');
        }
    },

    exportSinglePDF:function()
    {
        var quoteId = $("#hidQuoteID").val();

        if (quoteId != undefined || quoteId != 0)
        {
            window.location.href = "/RMA/DownLoadPDF?quoteNumber=" + quoteId;
        }
    },
    HideArrow: function (divId)
    {
        var boolResult = $("#" + divId).data('fading');
        if (boolResult == false || boolResult ==undefined) {
            $("#" + divId).data('fading', true);
            $("#" + divId).fadeOut('fast', function () { $("#" + divId).data('fading', false) });
            $("#" + divId + "Delete").fadeOut('fast');
        }
    },
    quoteInfo: "",
    Searchenter :function (event) {
        if (event.keyCode == 13) {
            t2v_Quote.SearchContent();
        }
    },
    SearchContent : function () {
        var fullSearchString = t2v_util.valid.ConvertToJson($("#fullSearchBox").val());
        if (fullSearchString != "" && fullSearchString != undefined && fullSearchString != null) {
            window.location.href = "/RMA#/page/1/search/" + fullSearchString;
        }
        else
            window.location.href = "/RMA#/List/1";
    },
    Fullsearch : function (isMPNSearch, fullSearchString, pageIndex) {
        if (fullSearchString == undefined || fullSearchString == null || fullSearchString == "")
            fullSearchString = t2v_util.valid.ConvertToJson($("#fullSearchBox").val());
        else
            fullSearchString = t2v_util.valid.ConvertToJson(fullSearchString);

        if (pageIndex == undefined || pageIndex == null || pageIndex == "")
            pageIndex = 1;

        var colInfo = t2v_Quote.GetConifigData();
        var param = "[";
        param += '{"columnName":"fullsearch"'
                    + ',"columnValue":"' + fullSearchString
                    + '","columnOperator":"cn"}]';
        FetureV7Grid.myParam(colInfo, "divList", 20, "/RMA/GetRMAList", param, "CreateTime");
        FetureV7Grid.showGrid();
    },
    ExportRMAList : function () {
        var colInfo = t2v_RMA.GetConifigData();
        var fieldsInfo = "[";
        for (var i = 0; i < colInfo.length; i++) {
            if (colInfo[i].isHidden != 'true')
                fieldsInfo += "{disName_en: '" + colInfo[i].disName_en + "',sortOrder: " + colInfo[i].sortOrder + ", columnName: '" + colInfo[i].columnName + "', disType: '" + colInfo[i].disType + "', isHidden: '" + colInfo[i].isHidden + "'}, ";
        }
        fieldsInfo += "]";
        var fullSearch = $("#fullSearchBox").val();
        var postData = new Array();
        postData.fullSearch = fullSearch;
        postData.exportFileds = fieldsInfo;
        ShowExportInfo(SiteRoot + '/Export/ExportRMAList', postData, 0);
    },
    CreateRMA: function (id) {
        $.ajax({
            url: "/RMA/RMAEdit",
            type: 'post',
            data: {QuoteID:id },
            beforeSend: function () {
                t2v_Quote._showLoading();
            },
            success: function (data, status, jqXhr) {
                t2v_Quote._hideLoading();
                $("#divMainPageDiv").css("padding", "0");
                $("#divMainPageDiv").html(data);
            }
        });
    },

    _detail: function (id) {

        $.ajax({
            url: SiteRoot + "/RMA/Detail",
            type: 'post',
            data: { id: id },
            beforeSend: function () {
                t2v_RMA._showLoading();
            },
            success: function (data, status, jqXhr) {
                t2v_Quote._hideLoading();
                //_stuffPage(data);
                $("#divMainPageDiv").css("padding", "0");
                $("#divMainPageDiv").html(data);
                $("#divShipToInfoArea").html("");
            }
        });
    },
    ChangeCalcStatus: function ()
    {
        var scope = angular.element($("#ng-value")[0]).scope();
        if ($("#chkCostReliefOnly").attr("checked") == "checked") {
        //if (scope.currentItem().CostReliefOnly == "1") {
            $("#txtUnitCost").attr("readonly", true);
            $("#txtUnitCost").val("");
            $("#txtExtCost").val("");
            $("#txtNetUnitCost").val("");
            $("#txtExtNetCost").val("");

            var scope = angular.element($("#ng-value")[0]).scope();
            scope.currentItem().UnitCost = "";
            scope.currentItem().ExtCost = "";
            scope.currentItem().NetUnitCost = "";
            scope.currentItem().ExtNetCost = "";
            scope.$apply();

            //$("#chkCostReliefOnly").attr("checked", "checked");
        }
        else
        {
            $("#txtUnitCost").attr("readonly", false);
            //$("#chkCostReliefOnly").removeAttr("checked");
        }
        t2v_RMA.CalcTotalCost();
    },

    DetailChangeCalcStatus: function () {
        var scope = angular.element($("#ng-value")[0]).scope();
        if (scope.currentItem().CostReliefOnly == "1") {
            

            $("#chkCostReliefOnly").attr("checked", "checked");
        }
        else {
            $("#chkCostReliefOnly").removeAttr("checked");
        }
    },
}
