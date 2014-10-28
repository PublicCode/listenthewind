var t2v_bussiness = {
    dashboard: {},
    lighthouse: {},
    language: {},
    Employee: {},
    Home: {},
    confirmchangepostion: "Are you sure you want to navigate away from this page?<br>If you leave this page, your changes will not be saved.<br>Press Yes to continue, or No to stay on the current page."
};

t2v_bussiness.lighthouse = {
    RemoveBackSPace: function() {
        window.BackSpace = function() {
            if (event.keyCode == 8) {
                obj = event.srcElement;
                if (obj.tagName.toLowerCase() == 'textarea' || (obj.tagName.toLowerCase() == 'input' && (obj.type.toLowerCase() == 'text' || obj.type.toLowerCase() == 'password'))) {
                    if (!obj.readOnly && !obj.disabled) {
                        return true;
                    }
                }

                event.returnvalue = false;
                return false;
            }
            return true;
        }

        with (window.document) {
            onkeydown = window.BackSpace;
        }
    },
    Init: function() {
        t2v_bussiness.lighthouse.MainContainer = $("#fragment-All");
        t2v_bussiness.lighthouse.RemoveBackSPace();
        var appName = new Array();
//        appName[0] = "Policies";
//        appName[1] = "Employee";
//        appName[2] = "Employee";
        appName[0] = "Employee";
        $('#MainTab').tabs({
            select: function(e, ui) {
                if (t2v_util.staticflag.EditFlag) {
                    jConfirm(t2v_bussiness.confirmchangepostion, "Confirm", function(yes) {
                        if (yes) {
                            t2v_util.staticflag.EditFlag = false;
                            var i = ui.index;
                            t2v_bussiness.lighthouse.RefreshMainContant(appName[i]);
                            $('#MainTab').tabs('select', i);
                            if (!t2v_util.history.h_flag)
                                t2v_util.history.SaveHistory(i);
                            if (i != 0)
                                $("#ctl00_ContentPlaceHolderMain_liHomeTitle").hide();
                            else
                                $("#ctl00_ContentPlaceHolderMain_liHomeTitle").show();
                        }
                    });
                    return false;
                }
                else {
                    t2v_util.staticflag.EditFlag = false;
                    var i = ui.index;
                    t2v_bussiness.lighthouse.RefreshMainContant(appName[i]);
                    if (!t2v_util.history.h_flag)
                        t2v_util.history.SaveHistory(i);
                    if (i != 0)
                        $("#ctl00_ContentPlaceHolderMain_liHomeTitle").hide();
                    else
                        $("#ctl00_ContentPlaceHolderMain_liHomeTitle").show();
                }
            }
        });

        t2v_bussiness.lighthouse.RefreshMainContant(appName[0]);
        t2v_bussiness.lighthouse.RefreshLeftMenu();
        t2v_bussiness.lighthouse.ReceiveMail();
        t2v_util.history.h_list[0] = 0;
        $(".pageLoading").hide();
    },
    ReceiveMail: function() {
        //t2v_bussiness.lighthouse.ReceiveActiveMail();        
    },
    ReceiveActiveMail : function(){
        var loginId = $.Querystring({ id: "LoginID" });
        if (loginId == "null") {
            return false;
        }
        T2VForm.Post("Service/UserRoleManagerService.asmx/ActiveRegister", { loginId: loginId }, function(response) { 
    window.location = "ActivePage.aspx";
  });
    },
    InitHome: function() {
        if (t2v_util.staticflag.EditFlag) {
            jConfirm(t2v_bussiness.confirmchangepostion, "Confirm", function(yes) {
                if (yes) {
                    t2v_util.staticflag.EditFlag = false;
                    t2v_bussiness.lighthouse.RefreshMainContant("Employee");
                    if (!t2v_util.history.h_flag)
                        t2v_util.history.SaveHistory(0);
                }
            });
            return false;
        }
        else {
            t2v_util.staticflag.EditFlag = false;
            t2v_bussiness.lighthouse.RefreshMainContant("Employee");
            if (!t2v_util.history.h_flag)
                t2v_util.history.SaveHistory(0);
        }
    },
    RefreshMainContant: function(appName) {
        var appUrl = {
            Dashboard: t2v_bussiness.lighthouse.InitMainHome,
            Policies: t2v_bussiness.lighthouse.InitPolicies,
            Employee: t2v_bussiness.Employee.InitUser
        };

        $("#ctl00_ContentPlaceHolderMain_panelContant").empty();
        appUrl[appName]();
    },
    InitMainHome: function() {
        T2VForm.RequestPage('UserControl/Common/HomePage.uc', $('#ctl00_ContentPlaceHolderMain_panelContant'));
    },
    InitPolicies: function() {
        T2VForm.RequestPage('UserControl/Common/PoliciesPage.uc', $('#ctl00_ContentPlaceHolderMain_panelContant'));
    },
    RefreshLeftMenu: function() {
        t2v_bussiness.Employee.InitDialog();
        var url = "UserControl/Common/LeftMenu.uc"
        $.ajax({
            type: "Get",
            url: url,
            data: { r: Math.random() },
            beforeSend: function(xhr) {
                $("#leftMenu").html("<img src ='Content/img/loading.gif' alt='loading'  />");
            },
            success: function(response) {
                $("#leftMenu").html(response);
                $('#list1a').accordion({ alwaysOpen: false, header: "a.head" });
                t2v_bussiness.dashboard.LeftMenu();
            },
            error: function(xml, status) {
                $("#leftMenu").html(xml.responseText);
            }
        });
    },
    InsertSwtichSystem: function(loginId, systemName) {
        T2VForm.Post("Service/SwitchSystemManagerService.asmx/InsertSwitchSystem", { loginId: loginId, systemName: systemName }, function(response) { });
    }
};
