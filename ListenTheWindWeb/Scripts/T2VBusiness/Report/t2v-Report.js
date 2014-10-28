var t2v_Report = {

    _showLoading: function () {
        $('#divMainPageDiv').hide();
        $('#loading').show();
    },
    _hideLoading: function () {
        $('#loading').hide();
        $('#divMainPageDiv').show();
    },
    objSearchInfo: "",

    initSearchForm:function()
    {
        $.ajax({
            url: "/Report/GetSubject",
            type: 'post',
            data: {},
            beforeSend: function () {
                t2v_Report._showLoading();
            },
            success: function (data, status, jqXhr) {
                t2v_Report._hideLoading();
                var objSearch = eval("(" + data.result + ")");
                t2v_Report.objSearchInfo = objSearch;

                //set report subject dropdownlist value
                for (i = 0; i < objSearch.subject.length; i++)
                {
                    $("#selReportSubject").append("<option value='" + objSearch.subject[i].value + "'>" + objSearch.subject[i].text + "</option>");
                }
                t2v_Report.ChangeSubject();


                //check window.hash if has value it means we need load the data.
                if (window.location.hash != "" && window.location.hash != undefined)
                {
                    var hashObj = eval(decodeURI(window.location.hash).substring(1));
                    for (i = 0; i < hashObj.length; i++)
                    {
                        if (hashObj[i].columnName.indexOf('dll') != -1) {

                            var ddlValue = hashObj[i].columnValue.split(',');
                            for(k=0;k<ddlValue.length;k++)
                            {
                                $("#" + hashObj[i].columnName).multiselect("widget").find(":checkbox[value='" + ddlValue[k] + "']").attr("checked", "checked");

                                $("#" + hashObj[i].columnName + " option[value='" + ddlValue[k] + "']").attr("selected", 1);
                            }
                            $("#" + hashObj[i].columnName).multiselect("refresh");
                        }
                        else {
                            $("#" + hashObj[i].columnName).val(hashObj[i].columnValue);
                        }
                    }
                    
                    t2v_Report.SearchResult();
                }
            }
        });
    },

    ExportReport:function()
    {
        var objSearch = t2v_Report.objSearchInfo;

        var param = "[";

        for (i = 0; i < objSearch.searchfield.length; i++) {
            if (objSearch.searchfield[i].RefSubjectId == $("#selReportSubject").val()) {
                if ($("#" + objSearch.searchfield[i].ControlId).val() == "") {
                    param += '{"columnName":"' + objSearch.searchfield[i].ControlId + '"' + ',"columnValue":" ","columnOperator":"cn"},';
                }
                else {
                    param += '{"columnName":"' + objSearch.searchfield[i].ControlId + '"' + ',"columnValue":"' + $("#" + objSearch.searchfield[i].ControlId).val() + '","columnOperator":"cn"},';
                }
            }
        }
        param = param.substring(0, param.length - 1);

        param += "]";


        $.ajax({
            url: "/Report/EncryptParam",
            type: 'post',
            data: {param:param},
            beforeSend: function () {
                //t2v_Report._showLoading();
            },
            success: function (data, status, jqXhr) {
                //t2v_Report._hideLoading();
                var k = data.result;
                OpenExportPopWindow("../ASPX/Export.aspx?a=" + Math.random(), $("#selReportSubject").val(), data.result);
            }
        });
    },

    SearchResult:function()
    {
        var objSearch =t2v_Report.objSearchInfo;

        var colInfo = new Array();
        var currentPosition = 0;
        for (i = 0; i < objSearch.subject.length; i++)
        {
            if (objSearch.subject[i].val == $("#selReportSubject").val())
            {
                currentPosition = i;
                break;
            }
        }

        for (i = 0; i < objSearch.subject[currentPosition].detail.length; i++) {
            colInfo.push({ disName_en: '' + objSearch.subject[currentPosition].detail[i].value + '', sortOrder: 3, columnName: '' + objSearch.subject[currentPosition].detail[i].text + '', Width: '15', disType: 'text' });
        }
        
        var param = "[";

        param += '{"columnName":"subject","columnValue":"' + $("#selReportSubject").val() + '","columnOperator":"cn"},';
        for (i = 0; i < objSearch.searchfield.length; i++)
        {
            if (objSearch.searchfield[i].RefSubjectId == $("#selReportSubject").val())
            {
                if ($("#" + objSearch.searchfield[i].ControlId).val() == "" || $("#" + objSearch.searchfield[i].ControlId).val()==null || $("#" + objSearch.searchfield[i].ControlId).val() == undefined) {
                    param += '{"columnName":"' + objSearch.searchfield[i].ControlId + '"' + ',"columnValue":"","columnOperator":"cn"},';
                }
                else {
                    param += '{"columnName":"' + objSearch.searchfield[i].ControlId + '"' + ',"columnValue":"' + $("#" + objSearch.searchfield[i].ControlId).val() + '","columnOperator":"cn"},';
                }
            }
        }
        param = param.substring(0, param.length - 1);

        param += "]";

        FetureV7Grid.myParam(colInfo, "divList", 20, "/Report/GetReport", param, "CreateTime",undefined,300);
        FetureV7Grid.showGrid();
        window.location.hash = encodeURI(param);
    },

    ClearData:function()
    {
        var objSearch = t2v_Report.objSearchInfo;
        for (i = 0; i < objSearch.searchfield.length; i++) {
            if (objSearch.searchfield[i].RefSubjectId == $("#selReportSubject").val()) {
                $("#" + objSearch.searchfield[i].ControlId).val("");
            }
        }

        $("#divList").html("<div style='text-align: center; padding-top:200px;'><span style='font-family: Verdana; font-size: 20px; font-weight: bold; color: #666'>No Result Found</span></div>");
    },

    ChangeSubject:function()
    {
        t2v_Report.ClearData();
        var objSearch =t2v_Report.objSearchInfo;
        var tbSearchForm = $("#tbSearchForm");
        tbSearchForm.empty();

        for (i = 0; i < objSearch.searchfield.length; i++) {
            if (objSearch.searchfield[i].RefSubjectId == $("#selReportSubject").val()) {
                var tRow = "<tr>";
                if (objSearch.searchfield[i].ControlIsShow) {
                    if (objSearch.searchfield[i].CurrentControlType == 3)//textbox
                    {
                        var txtControl = "<input type='text' class='inputText01' style='width:140px;' maxlength='" + objSearch.searchfield[i] + "' id='" + objSearch.searchfield[i].ControlId + "' placeholder='" + objSearch.searchfield[i].ControlTitle + "' />";
                        tRow += "<td>" + objSearch.searchfield[i].ControlTitle + "</td><td>" + txtControl + "</td>";
                    }
                    else if (objSearch.searchfield[i].CurrentControlType == 9) //MultipleSelect
                    {
                        var dropDownControl = "<select multiple='multiple' class='multipleselect' id='" + objSearch.searchfield[i].ControlId + "'>";
                        if (objSearch.searchfield[i].listTextValue != null)
                        {
                            for (l = 0; l < objSearch.searchfield[i].listTextValue.length; l++)
                            {
                                dropDownControl += "<option value='" + objSearch.searchfield[i].listTextValue[l].value + "'>" + objSearch.searchfield[i].listTextValue[l].text + "</option>";
                            }
                        }
                        dropDownControl += "</select>";
                        tRow += "<td>" + objSearch.searchfield[i].ControlTitle + "</td><td>" + dropDownControl + "</td>";
                    }
                    else if (objSearch.searchfield[i].CurrentControlType == 4) //DateTime
                    {
                        var txtControl = "<input type='text' class='datefield' style='width:140px;' id='" + objSearch.searchfield[i].ControlId + "' placeholder='" + objSearch.searchfield[i].ControlTitle + "' />";
                        tRow += "<td>" + objSearch.searchfield[i].ControlTitle + "</td><td>" + txtControl + "</td>";
                    }
                }
                tRow += "</tr>";
                $(tRow).appendTo(tbSearchForm);
                //tbSearchForm.append(tRow);
            }
        }

        $(".multipleselect").multiselect({
            header: true,
            selectedList: 5,
            close: function () {

            },
            lateTriggering: true
        });

        //$(".multipleselect").multiselect("uncheckAll");

        $(".datefield").datepicker({
            changeYear: true, changeMonth: true, showButtonPanel: true,
            onClose: function (e) {
                var ev = window.event;
                if (ev.srcElement.innerHTML == 'Clear')
                    this.value = "";
            },
            closeText: 'Clear',
            buttonText: ''
        });
    }
}