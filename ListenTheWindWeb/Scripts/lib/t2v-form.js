(function($) {
    var oldHTML = $.fn.html;

    $.fn.formhtml = function() {
        if (arguments.length) return oldHTML.apply(this, arguments);
        $("input,textarea,button", this).each(function() {
            this.setAttribute('value', this.value);
        });
        $(":radio,:checkbox", this).each(function() {
            if (this.checked) this.setAttribute('checked', 'checked');
            else this.removeAttribute('checked');
        });
        $("option", this).each(function() {
            if (this.selected) this.setAttribute('selected', 'selected');
            else this.removeAttribute('selected');
        });
        return oldHTML.apply(this);
    };
})(jQuery);

(function($) {
    $.fn.T2VForm = function(p) 
    {
        aDiv = $(this);
        op = $.extend({
            operationUrl: "",
            autoLoadType: "None",
            errorContainer: "errorContainer"
        }, p);


        var t2vFormBase = {
            SaveT2VFormConfig: function() {
                var T2VFormConfigData = eval('(' + $("[DataField=T2VFormConfigData]", t2vFormBase.aDiv).val() + ')');
                t2vFormBase.T2VFormConfigData = T2VFormConfigData;
            },

            ChangeLanguage: function() {
                if (t2vFormBase.OperationType == "Edit") {
                    t2vFormBase.aDiv.T2VFormShowEdit(t2vFormBase["OperationArgs1"], t2vFormBase["OperationArgs2"]);
                }
                if (t2vFormBase.OperationType == "Detail") {
                    t2vFormBase.aDiv.T2VFormShowDetail(t2vFormBase["OperationArgs1"], t2vFormBase["OperationArgs2"]);
                }
            },
            SetLanguage: function(languageData) {
                for (var i = 0; i < languageData.length; i++) {
                    var enUrl = languageData[i].enUrl;
                    var frUrl = languageData[i].frUrl;
                    var divs = new Array();
                    for (var index = 0; index < languageData[i].divIDs.length; index++) {
                        divs.push($("#" + languageData[i].divIDs[index]));
                    }

                    t2v_util.culture.ChangeMulti(divs, enUrl, frUrl);
                }
            },
            SubmitData: function(afterSave, beforeSave, otherData) {
                var data = T2VForm.GetJsonDataByConfig(t2vFormBase.T2VFormConfigData);
                data["OperationType"] = "Submit";
                data["r"] = Math.random();
                if (otherData != undefined) {
                    $.each(otherData, function(key, value) {
                        data[key] = value;
                    });
                }
                if (beforeSave != undefined)
                    beforeSave(data);
                var t2vFormConfig = t2vFormBase.T2VFormConfigData[0];
                var groupName = t2vFormConfig.GroupName
                var dataStr = "{";
                $.each(data[groupName], function(key, value) {
                    if (dataStr != "{")
                        dataStr += ",";
                    dataStr += "\"" + key + "\" : \"" + value + "\"";
                });
                dataStr += "}"
                data[groupName] = dataStr;
                $.ajax({
                    type: "POST",
                    url: t2vFormBase.op.operationUrl,
                    data: data,
                    async: true,
                    beforeSend: function(xhr) {
                        $("#divLoading").show();
                    },
                    success: function(response) {
                        var valid = true;
                        if (response.indexOf("T2VFormSaveError") >= 0) {
                            valid = false;
                            $("#" + t2vFormBase.op.errorContainer).html(response);
                            $("#" + t2vFormBase.op.errorContainer).show();
                            t2v_util.staticflag.EditFlag = true;
                        }
                        else {
                            t2vFormBase.aDiv.html(response);
                            t2v_util.staticflag.EditFlag = false;
                        }
                        if (afterSave != undefined)
                            afterSave(response, valid);
                    },
                    error: function(xml, status) {
                        //alert(xml.responseText);
                    },
                    complete: function() {
                        $("#divLoading").hide();
                    }
                });
                return true;
            },
            SaveData: function(afterSave, beforeSave, otherData) {
                var data = T2VForm.GetJsonDataByConfig(t2vFormBase.T2VFormConfigData);
                data["OperationType"] = "Save";
                data["r"] = Math.random();
                if (otherData != undefined) {
                    $.each(otherData, function(key, value) {
                        data[key] = value;
                    });
                }
                if (beforeSave != undefined)
                    beforeSave(data);
                var t2vFormConfig = t2vFormBase.T2VFormConfigData[0];
                var groupName = t2vFormConfig.GroupName
                var dataStr = "{";
                $.each(data[groupName], function(key, value) {
                    if (dataStr != "{")
                        dataStr += ",";
                    dataStr += "\"" + key + "\" : \"" + value + "\"";
                });
                dataStr += "}"
                data[groupName] = dataStr;
                $.ajax({
                    type: "POST",
                    url: t2vFormBase.op.operationUrl,
                    data: data,
                    async: true,
                    beforeSend: function(xhr) {
                        $("#divLoading").show();
                    },
                    success: function(response) {
                        var valid = true;
                        if (response.indexOf("T2VFormSaveError") >= 0) {
                            valid = false;
                            $("#" + t2vFormBase.op.errorContainer).html(response);
                            $("#" + t2vFormBase.op.errorContainer).show();
                            t2v_util.staticflag.EditFlag = true;
                        }
                        else {
                            t2vFormBase.aDiv.html(response);
                            t2v_util.staticflag.EditFlag = false;
                        }
                        if (afterSave != undefined)
                            afterSave(response, valid);
                    },
                    error: function(xml, status) {
                        alert(xml.responseText);
                    },
                    complete: function() {
                        $("#divLoading").hide();
                    }
                });
                return true;
            }
        };

        t2vFormBase.aDiv = $(this);
        t2vFormBase.loadType = "None";
        t2vFormBase.op = op;
        $(this).data("t2vFormBase", t2vFormBase);
    }

    $.fn.T2VFormSubmitData = function(afterSave, beforeSave, otherData) {
        var t2vFormBase = $(this).data("t2vFormBase");
        $("#" + t2vFormBase.op.errorContainer).empty();
        $("#" + t2vFormBase.op.errorContainer).hide();
        var saveResult = t2vFormBase.SubmitData(afterSave, beforeSave, otherData);
        return saveResult;
    }

    $.fn.T2VFormSaveData = function(afterSave, beforeSave, otherData) {
        var t2vFormBase = $(this).data("t2vFormBase");
        $("#" + t2vFormBase.op.errorContainer).empty();
        $("#" + t2vFormBase.op.errorContainer).hide();
        var saveResult = t2vFormBase.SaveData(afterSave, beforeSave, otherData);
        return saveResult;
    }

    //Public Show Detail Function
    $.fn.T2VFormShowDetail = function(data, callback) {
        var originalData = data;

        var t2vFormBase = $(this).data("t2vFormBase");
        $("#" + t2vFormBase.op.errorContainer).empty();
        $("#" + t2vFormBase.op.errorContainer).hide();
        data["OperationType"] = "Detail";
        data["r"] = Math.random();
        t2vFormBase["OperationType"] = "Detail";
        t2vFormBase["OperationArgs1"] = data;
        t2vFormBase["OperationArgs2"] = callback;
        $.ajax({
            type: "Post",
            url: t2vFormBase.op.operationUrl,
            data: data,
            async: true,
            beforeSend: function(xhr) {
                $("#divLoading").show();
            },
            success: function(response) {
                t2vFormBase.aDiv.html(response);
                t2vFormBase.SaveT2VFormConfig();
                if (callback != undefined)
                    callback();
            },
            error: function(xml, status) {
                t2vFormBase.aDiv.html(xml.responseText);
            },
            complete: function() {
                $("#divLoading").hide();
            }
        });

        t2v_util.staticflag.EditFlag = false;
    }

    $.fn.T2VFormShowEdit = function(data, callback) {
        var originalData = data;
        var t2vFormBase = $(this).data("t2vFormBase");
        $("#" + t2vFormBase.op.errorContainer).empty();
        $("#" + t2vFormBase.op.errorContainer).hide();
        data["OperationType"] = "Edit";
        data["r"] = Math.random();
        t2vFormBase["OperationType"] = "Edit";
        t2vFormBase["OperationArgs1"] = data;
        t2vFormBase["OperationArgs2"] = callback;

        $.ajax({
            type: "Post",
            url: t2vFormBase.op.operationUrl,
            data: data,
            async: true,
            beforeSend: function(xhr) {
                $("#divLoading").show();
            },
            success: function(response) {
                t2vFormBase.aDiv.html(response);
                t2vFormBase.SaveT2VFormConfig();
                if (callback != undefined)
                    callback();
            },
            error: function(xml, status) {
                t2vFormBase.aDiv.html(xml.responseText);
            },
            complete: function() {
                $("#divLoading").hide();
            }
        });

        t2v_util.staticflag.EditFlag = true;
    }

    $.fn.T2VFormPost = function(url, data, callback) {
        aDiv = $(this);
        data["r"] = Math.random();
        $.ajax({
            type: "Post",
            url: url,
            data: data,
            async: true,
            beforeSend: function(xhr) {
                $("#divLoading").show();
            },
            success: function(response) {
                aDiv.html(response);
                if (callback != undefined)
                    callback();
            },
            error: function(xml, status) {
                aDiv.html(xml.responseText);
            },
            complete: function() {
                $("#divLoading").hide();
            }
        });
    }

    $.fn.T2VFormChangeLanguage = function() {
        var t2vFormBase = $(this).data("t2vFormBase");
        t2vFormBase.ChangeLanguage();
    }

})(jQuery);

var T2VForm = {
    PostModelPage: function(url, div, data) {
        div.html("Loading...").dialog("open").empty();
        T2VForm.PostPage(url, div, data);
    },
    PostAsync: function(url, data, callback) {
        if (data == undefined)
            data = {};
        $.ajax({
            type: "Post",
            url: url,
            data: data,
            async: false,
            beforeSend: function(xhr) {
                $("#divLoading").show();
            },
            success: function(response) {
                if (callback != undefined) {
                    callback(response);
                }
            },
            error: function(xml, status) {
                //alert(xml.responseText);
            },
            complete: function() {
                $("#divLoading").hide();
            }
        });
    },
    Post: function(url, data, callback) {
        if (data == undefined)
            data = {};
        $.ajax({
            type: "Post",
            url: url,
            data: data,
            async: true,
            beforeSend: function(xhr) {
                $("#divLoading").show();
            },
            success: function(response) {
                if (callback != undefined) {
                    callback(response);
                }
            },
            error: function(xml, status) {
                //alert(xml.responseText);
            },
            complete: function() {
                $("#divLoading").hide();
            }
        });
    },
    MergeJsonData: function(data1, data2) {

        $.each(data2, function(key, value) {

            data1[key] = value;
        });
    },
    SetDivFormData: function(div, jsonData) {
        div.find("[DataField]").each(function(i) {
            var dataField = $(this).attr("DataField");
            if (dataField != undefined) {
                T2VForm.SetControlData($(this), jsonData[dataField]);
            }
        });
    },
    GetDivFormData: function(div) {
        var data = {};
        div.find("[DataField]").each(function(i) {
            data[$(this).attr("DataField")] = T2VForm.GetControlData($(this));
        });
        return data;
    },
    GetJsonDataByConfig: function(T2VFormConfigData) {
        var data = {};
        for (var i = 0; i < T2VFormConfigData.length; i++) {
            var t2vFormConfig = T2VFormConfigData[i];
            if (t2vFormConfig.GroupType == "DIV") {
                data[t2vFormConfig.GroupName] = T2VForm.GetDivFormDataByConfig(t2vFormConfig);
            }
        }
        return data;
    },
    GetDivFormDataByConfig: function(t2vFormConfig) {
        var div = $("#" + t2vFormConfig.ControlID);
        var data = {};
        for (var i = 0; i < t2vFormConfig.DataFields.length; i++) {
            var dataFieldName = t2vFormConfig.DataFields[i].DataFieldName;
            var control = $(div).find("[DataField=" + dataFieldName + "]");
            if (control.length > 0) {
                data[dataFieldName] = T2VForm.GetControlData($(control));
            }
        }
        return data;
    },
    GetInputData: function(control) {
        var data = "";
        var inputData = T2VForm.GetControlData(control);
        inputData = t2v_util.valid.ConvertToUrlAndJson(inputData);
        data += $(control).attr("DataField") + ":'" + inputData + "'";
        return data;
    },
    GetControlData: function(control) {
        var data = "";
        switch ($(control).attr("tagName")) {
            case "INPUT":
                {
                    if ($(control).attr("type") == "checkbox") {
                        data += $(control).attr("checked");
                    }
                    else
                        data += $(control).val();
                }
                break;
            case "SPAN":
                {
                    data += $(control).text();
                }
                break;
            case "SELECT":
                {
                    data += $(control).val();
                }
                break; default:
                data += $(control).val();
                break;
        }
        return t2v_util.valid.ConvertToUrlAndJson(data);
    },
    SetControlData: function(control, data) {
        switch ($(control).attr("tagName")) {
            case "INPUT":
                {
                    data += $(control).val(data);
                }
                break;
            case "SPAN":
                {
                    data += $(control).text(data);
                }
                break;
            default:
                data += $(control).val(data);
                break;
        }

    },
    RequestPage: function(url, div, data, op) {
        var requestData = data;
        if (data != "" || data != undefined) {
            requestData += "&";
        }

        requestData += "ranArg=" + Math.random();
        var responseData = $.ajax({
            type: "GET",
            url: url,
            data: requestData,
            async: false,
            beforeSend: function(xhr) {
                $("#divLoading").show();
            },
            success: function(response) {
                $(div).html(response);
            },
            error: function(xml, status) {
                $(div).html(xml.responseText);
            },
            complete: function() {
                $("#divLoading").hide();
            }
        })
    },
    PostPage: function(url, div, data) {
        if (data == undefined)
            data = {};
        data["r"] = Math.random();
        $.ajax({
            type: "Post",
            url: url,
            data: data,
            async: false,
            beforeSend: function(xhr) {
                $("#divLoading").show();
            },
            complete: function() {
                $("#divLoading").hide();
            },
            success: function(response) {
                $(div).html(response);
            },
            error: function(xml, status) {
                $(div).html(xml.responseText);
            }            
        });
    }
}