var t2v_bussiness = {   
    dashboard: {},
    lighthouse: {},
    language:{},
    ppv:{},
    Employee:{},
    Customer:{},
    Reports:{},
    confirmchangepostion: "Are you sure you want to navigate away from this page?<br>If you leave this page, your changes will not be saved.<br>Press Yes to continue, or No to stay on the current page."
};

t2v_bussiness.lighthouse = {
    RemoveBackSPace: function () {
        window.BackSpace = function () {
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
    Init: function () {
        t2v_bussiness.lighthouse.MainContainer = $("#fragment-All");
        t2v_bussiness.lighthouse.RemoveBackSPace();
        var appName = new Array();
        appName[0] = "Dashboard";
        appName[1] = "Employee";
        appName[2] = "Customer";
        appName[3] = "Reports";
        $('#MainTab').tabs({
            select: function (e, ui) {
                if (t2v_util.staticflag.EditFlag) {
                    jConfirm(t2v_bussiness.confirmchangepostion, "Confirm", function (yes) {
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
        t2v_bussiness.lighthouse.RefreshTabMenu();
        t2v_bussiness.lighthouse.RefreshUserName();
        t2v_util.history.h_list[0] = 0;
        $(".pageLoading").hide();
        t2v_bussiness.ppv.ReceiveMail();

    },
    InitHome: function () {
        if (t2v_util.staticflag.EditFlag) {
            jConfirm(t2v_bussiness.confirmchangepostion, "Confirm", function (yes) {
                if (yes) {
                    t2v_util.staticflag.EditFlag = false;
                    t2v_bussiness.lighthouse.RefreshMainContant("Dashboard");
                    if (!t2v_util.history.h_flag)
                        t2v_util.history.SaveHistory(0);
                }
            });
            return false;
        }
        else {
            t2v_util.staticflag.EditFlag = false;
            t2v_bussiness.lighthouse.RefreshMainContant("Dashboard");
            if (!t2v_util.history.h_flag)
                t2v_util.history.SaveHistory(0);
        }
    },
    InitReports: function () {
        if (t2v_util.staticflag.EditFlag) {
            jConfirm(t2v_bussiness.confirmchangepostion, "Confirm", function (yes) {
                if (yes) {
                    t2v_util.staticflag.EditFlag = false;
                    t2v_bussiness.lighthouse.RefreshMainContant("Reports");
                    if (!t2v_util.history.h_flag)
                        t2v_util.history.SaveHistory(2);
                }
            });
            return false;
        }
        else {
            t2v_util.staticflag.EditFlag = false;
            t2v_bussiness.lighthouse.RefreshMainContant("Reports");
            if (!t2v_util.history.h_flag)
                t2v_util.history.SaveHistory(2);
        }
    },
    RefreshMainContant: function (appName) {
        var appUrl = {
            Reports: t2v_bussiness.reports.Init
        };

        $("#ctl00_ContentPlaceHolderMain_panelContant").empty();
        appUrl[appName]();
    },
    RefreshUserName: function () {
        T2VForm.Post("Service/CustomerManagerService.asmx/GetUserName", {}, function (response) {
            $("#ctl00_lblName").text($("string", response).text());
        });
    },
    RefreshTabMenu: function () {
        var url = "UserControl/Common/TabMenu.uc"
        $.ajax({
            type: "Get",
            url: url,
            data: { r: Math.random() },
            beforeSend: function (xhr) {
                $("#MainTab").html("<img src ='App_Themes/Default/Images/loading.gif' alt='loading'  />");
            },
            success: function (response) {
                $("#MainTab").html(response);
            },
            error: function (xml, status) {
                $("#MainTab").html(xml.responseText);
            }
        });
    },
    SwitchTabClass: function (slt) {
        $(slt).parent().parent().find(".ui-tabs-selected").removeClass("ui-tabs-selected");
        $(slt).parent().addClass("ui-tabs-selected");
    },
    RefreshLeftMenu: function () {
        var url = "UserControl/Common/LeftMenu.uc"
        $.ajax({
            type: "Get",
            url: url,
            data: { r: Math.random() },
            beforeSend: function (xhr) {
                $("#leftMenu").html("<img src ='App_Themes/Default/Images/loading.gif' alt='loading'  />");
            },
            success: function (response) {
                $("#leftMenu").html(response);
                $('#list1a').accordion({ alwaysOpen: false, header: "a.head" });
                t2v_bussiness.dashboard.LeftMenu();
            },
            error: function (xml, status) {
                $("#leftMenu").html(xml.responseText);
            }
        });
    },
    ResizeFutureV7Grid: function () {
        $(window).resize(function () {
            if ($("#divOut").length > 0) {
                var tmpWidth = $(".pageSection").width() - 10;
                $("#divOut").css("width", tmpWidth + "px");
            }
        });
    },
    InsertSwtichSystem: function (loginId, systemName) {
        T2VForm.Post("Service/SwitchSystemManagerService.asmx/InsertSwitchSystem", { loginId: loginId, systemName: systemName }, function (response) { });
    }
};