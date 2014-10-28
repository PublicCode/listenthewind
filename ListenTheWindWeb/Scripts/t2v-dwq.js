
function Show_DWQ() {
    window.open("DWQ/DWQSearch.aspx", 'DWQ', 'height=700, width=860, top=0 , left=0, toolbar=yes, menubar=yes, scrollbars=yes, resizable=yes,location=yes, status=yes');
}

function selectAllCheckboxes(spanChk, strChildSubId) {
    var oItem = spanChk.children;
    var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0];
    var xState = theBox.checked;
    var elm = theBox.form.elements;
    var i = 0;
    for (i = 0; i < elm.length; i++) {
        if (elm[i].type == "checkbox" && elm[i].id != theBox.id && elm[i].id.indexOf(strChildSubId) >= 0) {
            if (elm[i].disabled == "") {
                if (elm[i].checked != xState) {
                    elm[i].click();
                    elm[i].checked = xState;
                }
            }
        }
    }
}
function OpenExportPopWindow(url, searchItem, ctrlData) {

    var hfSearchItem;
    var hfCtrlData;
    if (searchItem != undefined) {
        hfSearchItem = searchItem;
        hfCtrlData = ctrlData;
    }
    else {
        hfSearchItem = document.getElementById("CPHMainContent_hfSearchItem").value;
        hfCtrlData = document.getElementById("CPHMainContent_hfCtrlData").value;
    }

    url = url + "&hfSearchItem=" + hfSearchItem;
    url = url + "&hfCtrlData=" + hfCtrlData;
    var LeftPos = Math.ceil((window.screen.width - 460) / 2);
    var TopPos = Math.ceil((window.screen.height - 380) / 2);
    window.open(url, 'DWQExport', 'height=380, width=460, top=' + TopPos + ', left=' + LeftPos + ', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no');
}
/************************************************************AJAXGroupSelect*********************************************************************/
function SelectOnChang(url, control) {
    var subjectIdData = $("#hfCurrentSubjectId").val();
    var controlValues = "[";
    var searchForm = document.getElementById("DWQSearchForm_tbSearchForm") != null ? document.getElementById("DWQSearchForm_tbSearchForm") : document.getElementById("tbSearchForm");
    var controls = control.tagName == "SELECT" ? searchForm.getElementsByTagName("select") : searchForm.getElementsByTagName("input");

    var groupName = $(control).attr("DDlGroupName");
    $.each(controls, function(i, n) {
        if ($(n).attr("DDlGroupName") == groupName) {
            controlValues += '{' + n.id + ':"' + n.value + '"},'
        }
    })
    controlValues = controlValues.substring(0, controlValues.length - 1);
    controlValues += "]";

    $.ajax({
        type: "POST",
        url: "../Service/DWQManagerService.asmx/GetDropDownList",
        data: { subjectId: subjectIdData, controlValuesData: controlValues },
        //contentType: "application/json; charset=utf-8",
        beforeSend: function(xhr) {
            //xhr.setRequestHeader("Content-type", "application/json; charset=utf-8");
        },
        dataType: "xml",
        success: function(response) {
            if (control.tagName == "select") { SetSlectCallBackBySlt(response.text); }
            else { SetSelectCallBackByTxt(response.text); }
        },
        error: function(xml, status) {
            alert(xml.responseText);
        }

    });
}
function SetSelectCallBackByTxt(json) {

    var datas = eval('(' + json + ')');
    //        alert(json.length);
    if (datas.length > 0) {
        $.each(datas, function(i, n) {
            var table = document.getElementById(n.Id + "_Cbl");
            if (table != null)
                SetMultipleSelect(table, datas[i].Sub);
        });
    }
}
var cloneRow;
function SetMultipleSelect(table, data) {
    var rowLength = table.rows.length;
    if (!cloneRow) cloneRow = table.rows[table.rows.length - 1].cloneNode(true);
    //        $(row.firstChild.lastChild).removeAttr("for");
    for (var i = 0; i < rowLength; i++) table.deleteRow(0);
    $.each(data, function(i, n) {
        var tempRow = $(cloneRow).clone(true);
        $(tempRow).find("input").eq(0).attr("checked", "false").attr("id", Math.random())
        $(tempRow).find("label").eq(0).attr("for", $(tempRow).find("input").eq(0).attr("id")).text(eval("n.Value"));
        if ($(tempRow).find("label").eq(0).attr("for", $(tempRow).find("input").eq(0).attr("id")).text() != "") {
            $(tempRow).appendTo($(table));
        }
        //table.tBodies[0].appendChild(tempRow);
    })

}
function SetSlectCallBackBySlt(json) {
    //alert(json);
    var datas = eval('(' + json + ')');
    //alert(datas.length);
    if (datas.length > 0) {
        if (datas[0].length > 0) {
            $.each(datas, function(i, n) {
                var slt = document.getElementById(n[0].ControlId);
                slt.length = 0;
                option = new Option("", "");
                slt.options.add(option);
                $.each(datas[i], function(i, m) {
                    option = new Option(eval("n[" + i + "].CodeText"), eval("n[" + i + "].CodeValue"));
                    slt.options.add(option);
                })
            });
        }
    }
}

/************************************************************AJAXGroupSelect*********************************************************************/

/************************************************************MultipleSelect*****************************************************************************/
var oldtextid;
function checkboxtext(text, divid) {
    var div = document.getElementById(divid);
    var text = document.getElementById(text.id);

    if (document.getElementById("firstDiv")) document.getElementById("firstDiv").parentNode.removeChild(document.getElementById("firstDiv"));
    if (document.getElementById("secondDiv")) document.getElementById("secondDiv").parentNode.removeChild(document.getElementById("secondDiv"));
    if (oldtextid) document.getElementById(oldtextid).disabled = false;
    var firstDiv = document.createElement("DIV");
    firstDiv.id = "firstDiv";
    var secondDiv = document.createElement("DIV");
    secondDiv.id = "secondDiv";
    text.disabled = true;
    oldtextid = text.id;
    var textTop = text.offsetTop;
    var textHeight = text.clientHeight;
    var textLeft = text.offsetLeft;
    var tempText = text;
    while (tempText = tempText.offsetParent) { textTop += tempText.offsetTop;textLeft += tempText.offsetLeft; }

    // firstDiv style
    firstDiv.style.visibility = "";
    firstDiv.style.position = "absolute";
    firstDiv.style.left = textLeft - 100 + "px";
    firstDiv.style.top = textTop + textHeight+5;
    firstDiv.style.width = 0; //text.offsetWidth + "px";
    firstDiv.style.height = 0; //parseInt(div.style.height) + "px";
    firstDiv.style.border = 0;
//    firstDiv.style.border="black 1px solid"; 
//    firstDiv.style.backgroundColor = "white";
    // frame
    var L = document.createElement("IFRAME");
    L.name = "completionFrame";
    L.width = firstDiv.style.width;
    L.height = firstDiv.style.height;
    firstDiv.appendChild(L);
    text.parentNode.appendChild(firstDiv);
    // secondDiv style
    secondDiv.style.visibility = "";
    secondDiv.zIndex = 1;
    secondDiv.style.position = "absolute";
    //secondDiv.style.position="relative"; 
    secondDiv.style.left = textLeft - 100 + "px";
    secondDiv.style.top = textTop + textHeight+5;
    secondDiv.style.width = text.offsetWidth + 100 + "px";
    secondDiv.style.height = parseInt(div.style.height) + 120 + "px";
    secondDiv.style.border = "#EEEEEE solid 3px ";
    secondDiv.style.padding = 6 + "px";
//    secondDiv.style.border = "#dbdbdb 1px solid";
    secondDiv.style.backgroundColor = "#CBCBCB";
    
    // show message

    var tempDiv = div.cloneNode(true);
    $(tempDiv).prepend("<div><input type='checkbox' value='select all' title='select all' onclick='setMultipleSelectcheckbox(this)'/>select all</div>");
    tempDiv.style.display = "";
    tempDiv.style.height = parseInt(div.style.height) + 95 + "px";
    tempDiv.style.width = text.offsetWidth + 100 + "px";
    tempDiv.id = "tempDiv";
    tempDiv.style.backgroundColor = "#CBCBCB";
    var cks = div.getElementsByTagName("input");
    $.each($(tempDiv.childNodes[1]).find("input[type='checkbox']"), function(i, n) {
        $(n).removeAttr("checked");
    })
    if (text.value != "----Select----") {
        var tempcks = tempDiv.childNodes[1].getElementsByTagName("input");
        for (var i = 0; i < cks.length; i++) {
            if (cks[i].type == "checkbox" && cks[i].checked) {
                tempcks[i].outerHTML = tempcks[i].outerHTML.substring(0, tempcks[i].outerHTML.length - 1) + " CHECKED >";
            }
        }
    }
    var ThreeDiv = document.createElement("DIV");

    ThreeDiv.style.border = 0;
    ThreeDiv.style.paddingTop = 4 + "px";
    ThreeDiv.style.backgroundColor = "#CBCBCB";
    ThreeDiv.style.width = text.offsetWidth + 90 + "px";
    ThreeDiv.style.textAlign = "center";

    $(ThreeDiv).prepend("<input type='button' value='clear' class='btn05' onclick='clearboxvalue(\"" + div.id + "\",\"" + tempDiv.id + "\",\"" + text.id + "\")'/>");
    $(ThreeDiv).prepend("<input type='button' value='select' class='btn05' onclick='checkboxselect(\"" + div.id + "\",\"" + tempDiv.id + "\",\"" + text.id + "\")'/>");


    $(secondDiv).html(tempDiv.outerHTML + ThreeDiv.outerHTML);
    text.parentNode.appendChild(secondDiv);
}
function clearboxvalue(div, tempDiv, text) {
    //debugger;
    var tempDIVObj = document.getElementById(tempDiv);
    $(tempDIVObj).find("input[type=checkbox]").
    each(function () {
        //debugger;
        //alert(cb);
        $(this).attr("checked", false)
    });
    var text = document.getElementById(text);
    document.getElementById("firstDiv").parentNode.removeChild(document.getElementById("firstDiv"));
    document.getElementById("secondDiv").parentNode.removeChild(document.getElementById("secondDiv"));
    text.disabled = false;
}   
function setMultipleSelectcheckbox(cb) {
    $(cb).parent().parent().parent().find("input[type=checkbox]").
    each(function () {
        //debugger;
        var bSelectedAll = $(cb).attr("checked");
        //alert(cb);
        $(this).attr("checked", $(cb).is(":checked"))
    });
}
function clearMultipleDivs() {
    document.getElementById("firstDiv").parentNode.removeChild(document.getElementById("firstDiv"));
    document.getElementById("secondDiv").parentNode.removeChild(document.getElementById("secondDiv"));
}
function checkboxselect(divid, tempDiv, textid) {
    var div = document.getElementById(divid);
    var tempDiv = document.getElementById(tempDiv).childNodes[1];
    var text = document.getElementById(textid);
    if (!tempDiv) {
        document.getElementById("firstDiv").parentNode.removeChild(document.getElementById("firstDiv"));
        document.getElementById("secondDiv").parentNode.removeChild(document.getElementById("secondDiv"));
        text.disabled = false;
        return false;
    }
    var tempchks = tempDiv.getElementsByTagName("input");
    var chks = div.getElementsByTagName("input");

    text.value = "";
    for (var i = 0; i < tempchks.length; i++) {
        chks[i].checked = false;
        if (tempchks[i].type == "checkbox" && tempchks[i].checked) {
            chks[i].checked = true;
            text.value += $(tempchks[i].parentNode).text() + ",";
        }
    }
    text.value = text.value.substring(0, text.value.length - 1);

    text.disabled = false;
    text.parentNode.removeChild(document.getElementById("firstDiv"));
    text.parentNode.removeChild(document.getElementById("secondDiv"));
}
function SelectAllFields(chk) {
    var checked = $(chk).attr("checked");
    var tbl = $(chk).parent().parent().parent().parent().parent();
    $(tbl).find("input[type=checkbox]").each(function () {
        $(this).attr("checked", checked);
    });
}
function Export(divId) {
    var reportItem = $("#hfSearchItem").val();
    var exportType = $("#selExportType").val();
    var configData = $("#hfCtrlData").val();
    var exportFields = "";
    $("#" + divId).find("tr").each(function (idx) {
        if (idx > 0) {
            if ($(this).find("input[type=checkbox]").attr("checked")) {
                exportFields += "," + $(this).attr("FieldName");
            }
        }
    });
    if (exportFields == "") {
        alert("Field Name is required.");
        return;
    }
    $.ajax({
        type: "POST",
        url: "../Handler/DWQManagerService.asmx/BuildExtractFile?" + Math.random(),
        data: { ReportItem: reportItem, ExportType: exportType, ConfigData: configData, ExportFields: exportFields.substring(1) },
        async: false,
        beforeSend: function (xhr) {
            $("#divLoading").show();
        },
        success: function (response) {
            if ($("string", response).text().indexOf("Your result set is") == 0) {
                alert($("string", response).text());
            } else {
                var elemIF = document.createElement("iframe");
                elemIF.style.display = "none";
                elemIF.src = $("string", response).text();
                document.body.appendChild(elemIF);
            }
        },
        error: function (xml, status) {
            alert(xml.responseText);
        },
        complete: function () {
            $("#divLoading").hide();
        }
    });
}