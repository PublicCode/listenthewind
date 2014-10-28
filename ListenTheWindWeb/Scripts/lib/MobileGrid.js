var MobileGrid = {
    myParam: function (colInfo, divName, pageSize, servicesUrl, searchParam, sortName) {
        this.inCol = colInfo;
        this.divName = divName;
        this.pageSize = pageSize;
        this.columnName = "";
        this.displayName = "";
        this.currentPageIndex = 1;
        this.allCount = 0;
        this.searchParam = searchParam;
        this.isPostBack = false;
        this.pageCount = 0;
        this.servicesUrl = servicesUrl;
        this.sortName = sortName;
        this.sortOrder = "";
        this.postBackParam = "";
        this.lang = "en";
        this.outParam = "";
        this.outParamValue = "";
        window.selectedChk = ",";
        window.selectedChkAll = ",";

        this.multiLanguage = { pageSizeBefore_en: "display", pageSizeBefore_fr: "display1", pageSizeAfter_en: "matches per page",
            pageSizeAfter_fr: "matches per page1", refeshData_en: "Refresh Data", refeshData_fr: "Refresh Data1", resetFilter_en:
        "Reset Filters", resetFilter_fr: "Reset Filters1", firstPage_en: "First", firstPage_fr: "First1", prePage_en: "Prev",
            prePage_fr: "Pre1", nextPage_en: "Next", nextPage_fr: "Next1", lastPage_en: "Last", lastPage_fr: "Last1", countPage_en: "Count",
            countPage_fr: "Count1", ofPage_en: "of", ofPage_fr: "of1", exportExcel_en: "Export To Excel", exportExcel_fr: "Export To Excel1"
        };
    },
    fillDropDown: function (columnName, fillServicesUrl) {
        var str = "";
        $.ajax({
            type: "post",
            url: fillServicesUrl, //get drop down data source
            data: { colName: columnName },
            dataType: "json",
            async: false,
            success: function (filldata) {
                $(filldata).find("mytable").each(function (i) {
                    str += "<option value='" + $(this).find(columnName).text() + "'>" + $(this).find(columnName).text() + "</option>";
                })
            },
            error: function (data, textStatus) {
                //alert("error：" + textStatus);
            }
        });
        return str;
    },

    getSelectValue: function () {
        var selectValues = window.selectedChk;
        if (selectValues != ",") {
            selectValues = selectValues.replace(/(Chk)/g, "");
            return selectValues.substring(1, selectValues.length - 1);
        }
        else {
            return "";
        }
    },
    showGrid: function (fullSearchText) {
        $.mobile.hidePageLoadingMsg();
        var inCol = this.inCol;
        var divName = this.divName;
        var colName = "";
        var tempColName = "";
        var pageSize = this.pageSize;
        var currentPageIndex = this.currentPageIndex;
        var searchParam = this.searchParam;
        var isPostBack = this.isPostBack;
        var servicesUrl = this.servicesUrl;
        var sortName = this.sortName;
        var sortOrder = this.sortOrder;
        var disName = "";
        //the select column
        for (i = 0; i < inCol.length; i++) {
            if (inCol[i].columnName && !inCol[i].isHidden && inCol[i].disType != "checkbox") {
                colName += inCol[i].columnName + ",";
                disName += inCol[i]["disName_" + this.lang] + ",";
            }
            if (inCol[i].columnName && inCol[i].disType != "checkbox") {
                tempColName += inCol[i].columnName + ",";
            }
        }
        tempColName = tempColName.substr(0, tempColName.length - 1);
        colName = colName.substr(0, colName.length - 1);
        disName = disName.substr(0, disName.length - 1);

        this.columnName = colName;
        this.displayName = disName;
        if (sortName == "") {
            sortName = "CreateDate";
        }
        if (sortOrder == "") {
            sortOrder = "desc";
        }
        $.ajax({
            type: "post",
            url: servicesUrl,
            data: { currentPageIndex: currentPageIndex, pageSize: pageSize, sortName: sortName, sortOrder: sortOrder, SearchParam: searchParam },
            dataType: "json",
            async: true,
            beforeSend: function () {
                $.mobile.showPageLoadingMsg();
            },
            success: function (mydata) {
                $.mobile.hidePageLoadingMsg();

                if (!isPostBack) {
                    MobileGrid.initHeadTable(divName);
                }
                MobileGrid.parseXml(mydata, inCol, divName, fullSearchText);
                this.outParam = "";
                this.outParamValue = "";
            },
            error: function (data, textStatus) {
                if (textStatus == "parseerror")
                    window.location.href = "/Account/LogOn";
            }
        });
    },
    couputePage: function (pageDirection) {
        var pageCount;

        pageCount = Math.ceil(this.allCount / this.pageSize);
        this.pageCount = pageCount;
        if (pageDirection == "first") {
            this.currentPageIndex = 1;
        }
        if (pageDirection == "last") {
            this.currentPageIndex = pageCount;
        }
        if (pageDirection == "up") {
            if (this.currentPageIndex <= 1) {
                this.currentPageIndex = 1;
            }
            else {
                this.currentPageIndex--;
            }
        }
        else if (pageDirection == "down") {

            if (this.currentPageIndex >= pageCount) {
                this.currentPageIndex = pageCount;
            }
            else {
                this.currentPageIndex++;
            }
        }

        MobileGrid.showGrid();
    },
    setOutSearchParam: function (searchparam, searchvalue) {
        this.outParam = searchparam;
        this.outParamValue = searchvalue;
        MobileGrid.composeSearchParam();
    },
    composeSearchParam: function () {

        this.postBackParam = "[";
        this.searchParam = "[";
        for (i = 0; i < this.inCol.length; i++) {
            if (this.inCol[i].isFilterControl && this.inCol[i].isFilterControl == 'true') {
                if ($("#control" + this.inCol[i].columnName).val() != "") {
                    this.searchParam += "{columnName:\"" + this.inCol[i].columnName + "\",columnValue:\"" + t2v_util.valid.ConvertToJson($("#control" + this.inCol[i].columnName).val().replace(/'/g, "\'\'")) + "\",";
                    if (this.inCol[i].operatorType) {
                        this.searchParam += "columnOperator:\"" + t2v_util.valid.ConvertToJson(this.inCol[i].operatorType) + "\",isOutParam:\"0\"},";
                    }
                    else {
                        this.searchParam += "columnOperator:\"=\",isOutParam:\"0\"},";
                    }
                    this.postBackParam += "{\"controlName\":\"control" + this.inCol[i].columnName + "\",\"controlValue\":\"" + t2v_util.valid.ConvertToJson($("#control" + this.inCol[i].columnName).val().replace(/'/g, "\'\'")) + "\"},";
                }
            }
        }
        if ($.trim(this.outParam) != "" && $.trim(this.outParamValue) != "") {
            var outpa = this.outParam.split(",");
            for (i = 0; i < outpa.length; i++) {
                this.searchParam += "{columnName:\"" + outpa[i].toString() + "\",columnValue:\"" + t2v_util.valid.ConvertToJson(this.outParamValue.replace(/'/g, "\'\'")) + "\",columnOperator:\"like\",isOutParam:\"1\"},";
            }
        }
        if (this.postBackParam != "[") {
            this.postBackParam = this.postBackParam.substring(0, this.postBackParam.length - 1);
        }
        this.postBackParam += "]";
        this.postBackParam = eval(this.postBackParam);
        this.searchParam += "]";
        this.currentPageIndex = 1;
        MobileGrid.showGrid();
    },
    changePageSize: function (newPageSize) {
        this.pageSize = newPageSize;
        this.currentPageIndex = 1;
        MobileGrid.showGrid();
    },

    refreshGrid: function () {
        this.currentPageIndex = 1;
        MobileGrid.showGrid();
    },
    //init Head Table
    initHeadTable: function (divName) {
    },
    resetFilters: function () {
        this.postBackParam = "";
        this.searchParam = "";
        MobileGrid.showGrid();
    },
    ClickRows: function (obj) {
        obj.parent().find("tr").each(function (k, v) {
            if ($(v).attr("id") != obj.attr("id")) {
                //$(v).removeClass("GridRowClick");
            }
        })
    },
    sortColumn: function (sortColName, sortObj) {
        if (sortColName != "") {
            if (sortColName == this.sortName) {
                if (this.sortOrder == "desc") {
                    this.sortOrder = "asc";
                }
                else {
                    this.sortOrder = "desc";
                }
            }
            else {
                this.sortName = sortColName;
                this.sortOrder = "desc";
            }
        }
        this.currentPageIndex = 1;
        MobileGrid.showGrid();
    },
    processFun: function (fun) {
        fun();
    },
    processFunction: function (obj, fun, params) {
        var ary = params.split(",");
        var strReturn = "";
        for (z = 0; z < ary.length; z++) {

            var objvalue = obj.parent().parent().find("[id=td" + ary[z] + "]");
            if (objvalue.length > 0) {
                if (z != ary.length - 1) {
                    strReturn += t2v_util.valid.RemoveHtml(objvalue.html().replace(/\,/g, "")) + ",";
                }
                else {
                    strReturn += t2v_util.valid.RemoveHtml(objvalue.html().replace(/\,/g, ""));
                }
            }
        }
        fun(strReturn);
    },
    //Get param
    GetParam: function () {
        var myArray = new Array();
        myArray.push(this.searchParam);
        myArray.push(this.sortName);
        myArray.push(this.sortOrder);
        myArray.push(this.columnName);
        myArray.push(this.displayName);
        return myArray;
    },
    parseXml: function (mydata, inCol, divName, fullSearchText) {
        this.isPostBack = true;
        var allCount = mydata.records
        this.allCount = allCount;
        this.pageCount = Math.ceil(this.allCount / this.pageSize);

        var outerTable = document.createElement("table");
        var outerTbody = document.createElement("tbody");
        outerTable.cellPadding = 0;
        outerTable.cellSpecing = 0;
        //outerTable.className = "outertable";
        //outerTable.style.width = "100%";
        var outerTr = document.createElement("tr");
        var outerTd = document.createElement("td");
        //outerTd.style.cssText = "vertical-align:top;height:250px";

        var mainDiv = document.createElement("div");
        mainDiv.id = "divOut";
        var mainTable = document.createElement("table");
        var mainTbody = document.createElement("tbody");
        mainTable.id = "mainTable";
        //mainTable.cellPadding = 0;
        //mainTable.cellSpecing = 0;
        mainTable.setAttribute("data-role", "table");
        mainTable.setAttribute("data-mode", "columntoggle");
        mainTable.setAttribute("class", "ui-responsive table-stroke")

        //mainTable.style.cssText = "border-collapse:collapse;";

        var mianThead = document.createElement("thead");
        var mainTr = document.createElement("tr");

        for (i = 0; i < this.inCol.length; i++) {
            var mainTh = document.createElement("th");
            mainTh.align = "center";

            if (this.inCol[i].dataPriority != undefined) {
                mainTh.setAttribute("data-priority", this.inCol[i].dataPriority);
            }

            mainTh.innerHTML = this.inCol[i]["disName_" + this.lang];
            mainTr.appendChild(mainTh);

        }
        mianThead.appendChild(mainTr);
        mainTable.appendChild(mianThead);

        for (j = 0; j < mydata.rows.length; j++) {

            var textTr = document.createElement("tr");
            textTr.id = "tr" + j;
            $(textTr).click(function () {
                MobileGrid.ClickRows($(this));
            });

            for (i = 0; i < inCol.length; i++) {
                var textTd = document.createElement("td");
                textTd.id = "td" + inCol[i].columnName;
                if (inCol[i].isShowExportExcel && inCol[i].isShowExportExcel == 'true') {
                    continue;
                }
                if (inCol[i].isHidden) {
                    textTd.style.cssText = "display:none";
                }
                else {
                    if (inCol[i].align) {
                        textTd.align = inCol[i].align;
                    }
                    else {
                        textTd.align = "left";
                    }

                    if (inCol[i].BackGroundImg) {

                        textTd.style.cssText = "background:url(" + inCol[i].BackGroundImg + ")";
                    }
                }

                if (inCol[i].disType == "a") {

                    if (inCol[i].isSpecial == "true") {

                        $(textTd).append("<a href='" + mydata.rows[j]["strOnclick"] + "'><img src='/Content/Images/Details_View.png' alt=''></a>");
                    }
                    else {

                        if (inCol[i].gridUsedFor != undefined && inCol[i].gridUsedFor != "") {
                            var arrayList = inCol[i].linkparam.split(',');
                            var dx = inCol[i].columnName;
                            var dxd = mydata.rows[j][arrayList[0]] == null ? "" : mydata.rows[j][arrayList[0]];

                            if (inCol[i].class != undefined && inCol[i].class != "") {

                                $(textTd).append("<a href='#/" + inCol[i].gridUsedFor + "/" + dxd + "' class='" + inCol[i].class + "' onclick='javascript:MobileGrid.processFunction($(this)," + inCol[i].onclickFunction + ",\"" + inCol[i].linkparam + "\");'></a>");
                            }
                            else {
                                $(textTd).append("<a href='#/" + inCol[i].gridUsedFor + "/" + dxd + "' onclick='javascript:MobileGrid.processFunction($(this)," + inCol[i].onclickFunction + ",\"" + inCol[i].linkparam + "\");'></a>");
                            }
                        }
                        else {
                            if (inCol[i].class != undefined && inCol[i].class != "") {
                                $(textTd).append("<a href='#' class='" + inCol[i].class + "' onclick='javascript:MobileGrid.processFunction($(this)," + inCol[i].onclickFunction + ",\"" + inCol[i].linkparam + "\");'></a>");
                            }
                            else {
                                $(textTd).append("<a href='#' onclick='javascript:MobileGrid.processFunction($(this)," + inCol[i].onclickFunction + ",\"" + inCol[i].linkparam + "\");'></a>");
                            }
                        }
                    }

                    textTd.align = "left"
                }
                else if (inCol[i].disType == "checkbox") {
                    var textCheck = document.createElement("input");
                    textCheck.type = "checkbox";
                    var dx = inCol[i].columnName;
                    var dxd = mydata.rows[j][dx] == null ? "" : mydata.rows[j][dx];
                    textCheck.id = "Chk" + dxd;
                    if (window.selectedChk.indexOf("," + textCheck.id + ",") > -1) {
                        $(textCheck).attr("checked", "checked");
                    }
                    $(textCheck).click(function () {
                        if ($(this).attr("checked") == "checked") {
                            window.selectedChk += $(this).attr("id") + ",";
                        }
                        else {
                            if (window.selectedChk.indexOf("," + $(this).attr("id") + ",") > -1) {
                                window.selectedChk = window.selectedChk.replace("," + $(this).attr("id") + ",", ",");
                            }
                        }
                    });

                    textTd.appendChild(textCheck);
                }
                else if (inCol[i].disType == "text") {

                    var dx = inCol[i].columnName;
                    var dxd = mydata.rows[j][dx] == null ? "" : mydata.rows[j][dx];
                    var dxdHtml = "";
                    //Peter Sun change to bold charactor on searching
                    if (fullSearchText != undefined && fullSearchText != "") {
                        var matchString = dxd.toString().match(new RegExp(fullSearchText, 'gi'));
                        if (matchString != null) {
                            for (var k = 0; k < matchString.length; k++) {
                                dxdHtml = dxd.toString().replace(new RegExp(matchString[k], 'g'), "<b>" + matchString[k] + "</b>");
                            }
                        }
                        else {
                            dxdHtml = dxd;
                        }
                    }
                    else
                        dxdHtml = dxd;
                    textTd.innerHTML = dxdHtml;
                }
                else if (inCol[i].disType == "img") {
                    var statusParam = eval(inCol[i].statusParam);
                    for (x = 0; x < statusParam.length; x++) {
                        if ($(this).find(inCol[i].columnName).text() == statusParam[x].status) {
                            var textImg = document.createElement("img");
                            textImg.src = statusParam[x].disImg;
                            textImg.border = 0;
                            textTd.appendChild(textImg);
                        }
                    }
                }
                else if (inCol[i].disType == "aText") {

                    $(textTd).attr("class", inCol[i].class);

                    var dx = inCol[i].columnName;
                    var dxd = mydata.rows[j][dx] == null ? "" : mydata.rows[j][dx];
                    var dxdHtml = "";
                    //Peter Sun change to bold charactor on searching
                    if (fullSearchText != undefined && fullSearchText != "") {
                        var matchString = dxd.toString().match(new RegExp(fullSearchText, 'gi'));
                        if (matchString != null) {
                            for (var k = 0; k < matchString.length; k++) {
                                dxdHtml = dxd.toString().replace(new RegExp(matchString[k], 'g'), "<b>" + matchString[k] + "</b>");
                            }
                        }
                        else {
                            dxdHtml = dxd;
                        }
                    }
                    else {
                        dxdHtml = dxd;
                    }

                    $(textTd).append("<span id='spanText' style='width:100%;cursor: pointer;' onclick='javascript:MobileGrid.processFunction($(this)," + inCol[i].onclickFunction + ",\"" + inCol[i].linkparam + "\");'>" + dxdHtml + "</span>");
                }
                else if (inCol[i].disType == "originalA") {

                    var dx = inCol[i].columnName;
                    var dxd = mydata.rows[j][dx] == null ? "" : mydata.rows[j][dx];
                    var dxdHtml = "";

                    if (fullSearchText != undefined && fullSearchText != "") {
                        var matchString = dxd.toString().match(new RegExp(fullSearchText, 'gi'));
                        if (matchString != null) {
                            for (var k = 0; k < matchString.length; k++) {
                                dxdHtml = dxd.toString().replace(new RegExp(matchString[k], 'g'), "<b>" + matchString[k] + "</b>");
                            }
                        }
                        else {
                            dxdHtml = dxd;
                        }
                    }
                    else {
                        dxdHtml = dxd;
                    }
                    var tdContent = "<a style='background-image:none;cursor:pointer;text-decoration:none;'  href='" + mydata.rows[j]["strOnclick"] + "'>" + dxdHtml + "</a>";
                    tdContent += "<a href='#popupInfo" + j + this.currentPageIndex + "' data-role='button' data-iconpos='notext' data-icon='bars' data-transition='pop' data-rel='popup' data-inline='true'>more..</a>";
                    tdContent += "<div class='ui-content' id='popupInfo" + j + this.currentPageIndex + "' style='max-width: 350px;' data-role='popup' data-theme='e'><p>" + mydata.rows[j]["allContent"] + "</p></div>";
                    $(textTd).append(tdContent);

                }
                else {
                    if (inCol[i].columnName) {
                        textTd.innerHTML = $(this).find(inCol[i].columnName).text();
                    }
                }
                textTr.appendChild(textTd);
                textTd = null;
            }
            mainTbody.appendChild(textTr);
        }
        var icolspan = 0;
        for (i = 0; i < this.inCol.length; i++) {
            if (inCol[i].columnName) {
                icolspan++;
            }
        }

        var pageTable = document.createElement("table");
        var pageBody = document.createElement("tbody");
        //pageTable.className = "Gird_Header";
        var pageTr = document.createElement("tr");
        var pageTd = document.createElement("td");
        pageTd.align = "left";
        //pageTd.style.cssText = "height:20px";
        pageTd.id = "tdfooter";
        $(pageTd).append("<span>&nbsp;&nbsp;</span>");
        tolpagesize = parseInt(this.allCount) % parseInt(this.pageSize) == 0 ? parseInt(this.allCount / this.pageSize) : (parseInt(this.allCount / this.pageSize) + 1);
        $(pageTd).append("<span>Page: " + this.currentPageIndex + "/" + tolpagesize + " &nbsp;</span>");
        if (this.currentPageIndex == 1)
            $(pageTd).append(eval("this.multiLanguage.firstPage_" + this.lang));
        else
            $(pageTd).append("<a class='footerlink' onclick='javascript:MobileGrid.couputePage(\"first\")'>" + eval("this.multiLanguage.firstPage_" + this.lang) + "</a>");
        $(pageTd).append("<span>&nbsp;&nbsp;</span>");
        if (this.currentPageIndex == 1)
            $(pageTd).append(eval("this.multiLanguage.prePage_" + this.lang));
        else
            $(pageTd).append("<a class='footerlink' onclick='javascript:MobileGrid.couputePage(\"up\")'>" + eval("this.multiLanguage.prePage_" + this.lang) + "</a>");
        $(pageTd).append("<span>&nbsp;&nbsp;</span>");
        if (this.currentPageIndex == tolpagesize)
            $(pageTd).append(eval("this.multiLanguage.nextPage_" + this.lang));
        else
            $(pageTd).append("<a class='footerlink' onclick='javascript:MobileGrid.couputePage(\"down\")'>" + eval("this.multiLanguage.nextPage_" + this.lang) + "</a>");
        $(pageTd).append("<span>&nbsp;&nbsp;</span>");
        if (this.currentPageIndex == tolpagesize)
            $(pageTd).append(eval("this.multiLanguage.lastPage_" + this.lang));
        else
            $(pageTd).append("<a class='footerlink' onclick='javascript:MobileGrid.couputePage(\"last\")'>" + eval("this.multiLanguage.lastPage_" + this.lang) + "</a>");
        $(pageTd).append("<span>&nbsp;&nbsp;</span>");
        $(pageTd).append("<input type='hidden' id='currentPage' value='" + this.currentPageIndex + "'/>");

        //$(pageTd).append("<span>" + eval("this.multiLanguage.countPage_" + this.lang) + ": " + parseInt(this.currentPageIndex * this.pageSize - this.pageSize + 1) + "-" + (this.currentPageIndex == this.pageCount ? parseInt(this.allCount) : parseInt(this.currentPageIndex * this.pageSize)) + " &nbsp;" + eval("this.multiLanguage.ofPage_" + this.lang) + "&nbsp;&nbsp;" + this.allCount + "</span>");
        pageTr.appendChild(pageTd);
        pageBody.appendChild(pageTr);
        pageTable.appendChild(pageBody);


        mainTable.appendChild(mainTbody);
        mainDiv.appendChild(mainTable);
        outerTd.appendChild(mainDiv);
        outerTr.appendChild(outerTd);
        outerTbody.appendChild(outerTr);
        outerTable.appendChild(outerTbody);


        $("#" + divName).empty();
        $("#" + divName).append($("<div id='headDiv'>").html(pageTable));

        $("#" + divName).append(outerTable);

        var divHeight;
        divHeight = 430;

        var divWidth = $(window).width() - 240;

        //mainDiv.style.cssText = "overflow-x:auto;overflow-y:auto;height:" + divHeight + "px; width:" + divWidth + "px";

        if ($("#mainTable").find("tr").length == 1) {
            $("#mainTable").parent().parent().html("<div style='text-align: center;'><span style='font-family: Verdana; font-size: 20px; font-weight: bold; color: #666'>No Result Found</span></div>");
            $("#headDiv").html("");
        }
        if (this.postBackParam != "") {
            for (i = 0; i < this.postBackParam.length; i++) {
                $("#" + this.postBackParam[i].controlName).val(this.postBackParam[i].controlValue);
            }
        }

        $("#" + divName).trigger("create");

    }
}