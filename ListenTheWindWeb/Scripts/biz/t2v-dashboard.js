
t2v_bussiness.dashboard = {   
    ShowDiv: function(id) {
        var divId = "#" + id;
        if ($(divId).css("display") == "none") {
            $(divId).css("display", "");
        }
        else {
            $(divId).css("display", "none");
        }
        $("#CollectMainGrid").css("width", (window.screen.width - 330).toString() + "px");
    },
    ControlList: function(val) {
        if (val == "List") {
            if ($('#MainGridDiv').css("display") == "none") {
                $("#MainGridDiv").css("display", "block");
            }
            else {
                $('#MainGridDiv').css("display", "none");
            }
        }
        else {
            $('#MainGridDiv').css("display", "none");
        }
        $("#CollectMainGrid").css("width", (window.screen.width - 330).toString() + "px");
    },
    LeftMenu: function() {
        $("#controlLeftDiv").css("display", "block");
        $("#mainBar").removeClass("mainBarRight");
        $("#mainBar").addClass("mainBarLeft");
    },
    ControlDiv: function() {
        if ($("#controlLeftDiv").css("display") == "block") {
            $("#controlLeftDiv").css("display", "none");
            $("#mainBar").removeClass("mainBarLeft");
            $("#mainBar").addClass("mainBarRight");
        }
        else {
            $("#controlLeftDiv").css("display", "block");
            $("#mainBar").removeClass("mainBarRight");
            $("#mainBar").addClass("mainBarLeft");
        }
        if ($("#ctl00_gvSearchResult").length == 1) {
            t2v_bussiness.reports.InitShowSide();
        }
    },
    ControlPeriod: function() {
        var selectValue = "#" + $("#periodOjbective").val();
        if ($(selectValue).css("display") == "none") {
            $(selectValue).css("display", "block");
            var id = "#" + $(selectValue).attr('value');
            $(id).css("display", "block");
        }
    },
    
    //Project Menu
    OpenProjectUrl: function(slt) {
        var autoGuid = "";
        T2VForm.Post("Service/UserRoleManagerService.asmx/InserGuid", { permissionId: $(slt).attr("permissionId"), url: $(slt).attr("url") }, function(response) {
            if ($("string", response).text() != "") {
                autoGuid = $("string", response).text();
            }
        });
        var url = $(slt).attr("url") + "?AutoLogin=" + autoGuid;
        window.open(url, "_self");
    },

    //ChangePassword
    ChangePassword: function() {
        T2VForm.PostModelPage("UserControl/Common/ChangePassword.uc", $("#ChangePassword"), {});
    },
    CancelChangePassword: function() {
        $("#ChangePassword").dialog("close");
    },
    SavePassword: function() {
        var loginId = $("#ctl00_UserLoginId").val();
        var oldPassword = $.trim($("#txtOldPassword").val());
        var newPassword = $.trim($("#txtNewPassword").val());
        var reNewPassword = $.trim($("#txtReNewPassword").val());

        if (oldPassword == "") {
            jAlert("Old Password is required.", "Alert");
            return;
        } else {
            var errFlag = false;
            T2VForm.Post("Service/UserRoleManagerService.asmx/CheckUserPassword", { loginId:loginId, password: oldPassword }, function(response) {
                if ($("string", response).text() == "false") {
                    jAlert("The password you typed is incorrect.", "Alert");
                    errFlag = true;
                }
            });
            if (errFlag) {
                return;
            }
        }
        if (newPassword == "") {
            jAlert("New Password is required.", "Alert");
            return;
        }
        if (reNewPassword == "") {
            jAlert("Re-enter New Password is required.", "Alert");
            return;
        }
        if (newPassword != reNewPassword) {
            jAlert("The two new passwords should be identical.", "Alert");
            return;
        }

        T2VForm.Post("Service/UserRoleManagerService.asmx/ChangeUserPassword", { loginId:loginId, password: newPassword }, function(response) {
            t2v_bussiness.dashboard.CancelChangePassword();
        });
    }
};