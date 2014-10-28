var t2v_AdminSet = {

    SaveParameter: function () {
        var ParameterType = $("#selExceed").val();
        var ParameterValue = $("#txtValue").val();

        $.ajax({ url: "/Admin/SaveParameter", type: "get", data: { strParameterType: ParameterType, strParameterValue: ParameterValue },
            beforeSend: function () {
                $("#divMainPageDiv").css("padding", "100px 0 0 400px");
                $("#divMainPageDiv").html("<img src=/Content/images/LoadingContent.gif />");
            },
            success: function (data) {
                $("#divMainPageDiv").css("padding", "0");
                $("#divMainPageDiv").html(data);
                alert("save successful.");
            },
            error: function (status) {
                if (status.status != 401) {
                    alert(status);
                }
            }
        });
    },
    getSelectValue: function () {
        var selectdVal = $("#selExceed").val();

        if (selectdVal != "" && selectdVal != undefined && selectdVal != null) {
            $.ajax({ url: "/Admin/GetParameterByType", type: "post", data: { strParameterType: selectdVal },
                success: function (data) {
                    $("#txtValue").val(data.result);
                },
                error: function (status) {
                    if (status.status != 401) {
                        alert(status);
                    }
                }
            });
        }
    },
    updateFirstMonthBalance: function () {
        $.ajax({ url: "/Admin/UpdateLastCurrentMonth", type: "post",
            beforeSend: function () {
                $("#divMainPageDiv").css("padding", "100px 0 0 400px");
                $("#divMainPageDiv").html("<img src=/Content/images/LoadingContent.gif />");
            },
            success: function (data) {
                $("#divMainPageDiv").css("padding", "0");
                $("#divMainPageDiv").html(data);
                alert(data);
            },
            error: function (status) {
                if (status.status != 401) {
                    alert(status);
                }
            }
        });
    },
    GetExploreTimeZone: function () {
        var munites = new Date().getTimezoneOffset();
        var hour = parseInt(munites / 60);

        var munite = munites % 60;
        var prefix = "-";
        if (hour < 0 || munite < 0) {
            prefix = "+";
            hour = -hour;
            if (munite < 0) {
                munite = -munite;
            }
        }
        hour = hour + "";
        munite = munite + "";
        if (hour.length == 1) {
            hour = "0" + hour;
        }
        if (munite.length == 1) {
            munite = "0" + munite;
        }
        return prefix + hour;
    },

    GetConifigData: function () {
        var colInfo = new Array();

        colInfo.push({ disName_en: 'Link', columnName: 'UserName', Width: '20', disType: 'a', isFilterControl: 'false', operatorType: 'like', onclickFunction: t2v_AdminSet.ShowUserEdit, linkparam: 'ID' });

        colInfo.push({ disName_en: 'User Name', columnName: 'UserName', Width: '50', disType: 'text' });
        colInfo.push({ disName_en: 'Email', columnName: 'Email', Width: '50', disType: 'text' });

        colInfo.push({ disName_en: 'Time Zone', columnName: 'TimeZone', Width: '50', disType: 'text' });

        colInfo.push({ disName_en: 'ID', columnName: 'ID', Width: '50', disType: 'text', isHidden: 'true' });

        return colInfo;
    },
    GetUserProfileList: function () {
        var colInfo = t2v_AdminSet.GetConifigData();

        FetureV7Grid.myParam(colInfo, "divUserList", 20, "/Admin/GetUsersByPermission", "", "UserName");
        FetureV7Grid.showGrid();
    },

    ShowUserEdit: function (paramValue) {

        var paramArray = paramValue.split(",");
        $.ajax({
            url: "/Admin/UserEdit",
            type: "POST",
            data: { userID: paramArray[0] },
            beforeSend: function () {
                $("#divMainPageDiv").css("padding", "100px 0 0 400px");
                $("#divMainPageDiv").html("<img src=/Content/images/LoadingContent.gif />");
            },
            success: function (data) {
                $("#divMainPageDiv").css("padding", "0");
                $("#divMainPageDiv").html(data);
            },
            error: function (status) {
                if (status.status != 401) {
                    alert(status);
                }
            }
        });
    },
    AddMutileSelect: function () {

        //$("#selReportTo").multiselect({
        //    header: true,
        //    selectedList: 5,
        //    close: function () {
        //    },
        //    lateTriggering: true
        //});

    },
    ChangeTimeZone: function (obj) {

        $("#TimeZone").val(obj.val());
    },
    ChangeCheck: function (obj, flag) {
        if (flag == "activeflag") {
            $("#hidActiveFlag").val(obj.attr("checked") == "checked" ? true : false);
        }
        else if (flag == "deleteflag") {
            $("#hidDeleteFlag").val(obj.attr("checked") == "checked" ? true : false);
        }
    },
    ChangeReportTo: function (obj) {

        var resultID = "";
        var resultName = "";

        var idNameArray = obj.val().toLocaleString().split(',');

        for (i = 0; i < idNameArray.length; i++) {

            var myarray = idNameArray[i].split('|');
            resultID += $.trim(myarray[0]) + "," //id
            resultName += $.trim(myarray[1]) + ","; //name
        }

        $("#hidReportTo").val("," + $.trim(resultID));
        $("#hidReportToName").val("," + $.trim(resultName));
    },

    SaveUserProfile: function () {

        var emailFlag = $("#hidEmailFlag").val();

        if (emailFlag == "1") {
            $("#form0").submit();
        }
        else {
            var email = $("#txtEmail").val();
            $.ajax({
                url: "/Admin/CheckEmailExists",
                type: "POST",
                data: { strEmail: email, userID: $("#hidID").val() },
                beforeSend: function () {

                },
                success: function (data) {
                    if (data.resultValue == 0) {
                        t2v_util.t2valert.showAlert("Email already exists.");
                        $("#txtEmail").val("");
                    }
                    else {
                        $("#form0").submit();
                    }
                },
                error: function (status) {
                    if (status.status != 401) {
                        alert(status);
                    }
                }
            });
        }

    },
    CreateNewUser: function () {

        $.ajax({
            url: "/Admin/UserEdit",
            type: "POST",
            data: { userID: 0 },
            beforeSend: function () {
                $("#divMainPageDiv").css("padding", "100px 0 0 400px");
                $("#divMainPageDiv").html("<img src=/Content/images/LoadingContent.gif />");
            },
            success: function (data) {
                $("#divMainPageDiv").css("padding", "0");
                $("#divMainPageDiv").html(data);
            },
            error: function (status) {
                if (status.status != 401) {
                    alert(status);
                }
            }
        });

    },
    CheckEmailExists: function (obj) {
        if ($.trim(obj.val()) == "") {
            return;
        }
        $.ajax({
            url: "/Admin/CheckEmailExists",
            type: "POST",
            data: { strEmail: obj.val(), userID: $("#hidID").val() },
            beforeSend: function () {

            },
            success: function (data) {
                if (data.resultValue == 0) {
                    t2v_util.t2valert.showAlert("Email already exists.");
                    $("#txtEmail").val("");
                    $("#hidEmailFlag").val("1");
                }
            },
            error: function (status) {
                if (status.status != 401) {
                    alert(status);
                }
            }
        });
    },
    searchenter: function (event) {
        if (event.keyCode == 13) {
            t2v_AdminSet.fullsearch();
        }
    },
    //fullsearch: function () {
    //    var colInfo = t2v_AdminSet.GetConifigData();
    //    //colInfo.push({ isShowExportExcel: 'true', Handler: t2v_Authorization.ExportExcel });
    //    var param = "[";
    //    param += '{"columnName":"fullsearch"'
    //                                    + ',"columnValue":"' + t2v_Authorization.ConvertToJson($("#fullSearchBox").val()) + '","columnOperator":"cn"}]';

    //    FetureV7Grid.myParam(colInfo, "divUserList", 20, "/Admin/GetUsersByPermission", param, "UserName");
    //    FetureV7Grid.showGrid($("#fullSearchBox").val());
    //},
    Fullsearch: function (fullSearchString, pageIndex) {
        if (fullSearchString == undefined || fullSearchString == null || fullSearchString == "")
            fullSearchString = t2v_util.valid.ConvertToJson($("#fullSearchBox").val());
        else
            fullSearchString = t2v_util.valid.ConvertToJson(fullSearchString);

        if (pageIndex == undefined || pageIndex == null || pageIndex == "")
            pageIndex = 1;

        var colInfo = t2v_AdminSet.GetConifigData();
        var param = "[";
        param += '{"columnName":"fullsearch"'
                    + ',"columnValue":"' + fullSearchString
                    + '","columnOperator":"cn"}]';
        FetureV7Grid.myParam(colInfo, "divUserList", 20, "/Admin/GetUsersByPermission", param, "UserName");
        FetureV7Grid.showGrid($("#fullSearchBox").val());
    },
    CheckPassword: function () {

        if ($.trim($("#txtPassword").val()) != $.trim($("#txtConfirmPassword").val())) {

            t2v_util.t2valert.showAlert("The confirm password doesn't match, Please check and input again.");
            $("#txtPassword").val("");
            $("#txtConfirmPassword").val("");
        }
    },
    CheckUserNameExists: function (obj) {
        if ($.trim(obj.val()) == "") {
            return;
        }

        $.ajax({
            url: "/Admin/CheckUserNameExists",
            type: "POST",
            data: { strUserName: obj.val(), userID: $("#hidID").val() },
            beforeSend: function () {

            },
            success: function (data) {
                if (data.resultValue == 0) {
                    t2v_util.t2valert.showAlert("User name already exists.");
                    $("#txtUserName").val("");

                }
            },
            error: function (status) {
                if (status.status != 401) {
                    alert(status);
                }
            }
        });

    }
}