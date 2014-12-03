var ConfirmChangePostion = "Are you sure you want to navigate away from this page?<br>If you leave this page, your changes will not be saved.<br>Press [OK] to continue, or [Cancel] to stay on the current page."

var t2v_main = {
    Init: function () {
        //        t2v_main.RemoveBackSPace();
        //        t2v_main.RefreshLeftMenu();
    },
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
    Login: function () {
        $.ajax({
            type: "Post",
            url: "/Account/LogOn",
            async: true,
            data: { UserName: $("#UserName").val(), Pwd : $("#Pwd").val(), returnUrl: '' },
            beforeSend: function (xhr) {
                $("#divLeftMenu").html("<img src ='App_Themes/Default/Images/loading.gif' alt='loading'  />");
            },
            success: function (response) {
                window.location.reload();
            },
            error: function (xml, status) {
                $("#divLeftMenu").html(xml.responseText);
            }
        });
    },
    GetExplorerType: function () {
        if (navigator.userAgent.indexOf("MSIE") != -1) {
            return "ie";
        }
        else {
            return "notie";
        }
    },
    RefreshLeftMenu: function (id) {
        var url = "../../Share/GetLeftMenu"
        $.ajax({
            type: "Get",
            url: url,
            async: true,
            data: { r: Math.random() },
            beforeSend: function (xhr) {
                //$("#divLeftMenu").html("<img src ='App_Themes/Default/Images/loading.gif' alt='loading'  />");
            },
            success: function (response) {
                $("#tbMenuDistributor").html(response);
                $('#divLiftMenuList').accordion({ alwaysOpen: false, header: "a.head", active: id });
                //                t2v_main.ShowLeftMenu();

                ////Init Default Report
                //t2v_reports.Init($("#ctl00_hfInitReportItem").val(),$("#ctl00_hfInitExportFlag").val());

                //Init home page
                //t2v_main.InitRightContent();
            },
            error: function (xml, status) {
                $("#divLeftMenu").html(xml.responseText);
            }
        });
    },
    ShowLeftMenu: function () {
        $("#controlLeftDiv").css("display", "block");
        $("#mainBar").removeClass("mainBarRight");
        $("#mainBar").addClass("mainBarLeft");
    },
    ControlDiv: function () {
        var leftWidth = 248;
        if ($("#controlLeftDiv").css("display") == "table-cell") {
            $("#controlLeftDiv").css("display", "none");
            $("#mainBar").removeClass("mainBarLeft");
            $("#mainBar").addClass("mainBarRight");

            leftWidth = 48;
        } else {
            $("#controlLeftDiv").css("display", "table-cell");
            $("#mainBar").removeClass("mainBarRight");
            $("#mainBar").addClass("mainBarLeft");
        }

        if ($("#divResults").length == 1) {
            $("#divResults").find("div").css("width", (window.screen.width - leftWidth).toString() + "px");
        }
        if ($("#divOut").length == 1) {
            $("#divOut").css("width", (window.screen.width - leftWidth).toString() + "px");
        }
        t2v_main.SetControlMainDivWidth();
    },
    openwindow: function (url, name, iWidth, iHeight) {
        var url;
        var name;
        var iWidth;
        var iHeight;
        var iTop = (window.screen.availHeight - 30 - iHeight) / 2;
        var iLeft = (window.screen.availWidth - 10 - iWidth) / 2;
        return window.open(url, name, 'height=' + iHeight + ',,innerHeight=' + iHeight + ',width=' + iWidth + ',innerWidth=' + iWidth + ',top=' + iTop + ',left=' + iLeft + ',toolbar=no,menubar=no,scrollbars=auto,resizeable=no,location=no,status=no');
    },
    request: function (strParame) {
        var args = new Object();
        var query = location.search.substring(1);

        var pairs = query.split("&"); // Break at ampersand 
        for (var i = 0; i < pairs.length; i++) {
            var pos = pairs[i].indexOf('=');
            if (pos == -1) continue;
            var argname = pairs[i].substring(0, pos);
            var value = pairs[i].substring(pos + 1);
            value = decodeURIComponent(value);
            args[argname] = value;
        }
        return args[strParame];
    },
    SetControlMainDivWidth: function () {
        var screenWidth = window.screen.width - 27;
        if ($("#controlLeftDiv").css("display") == "" || $("#controlLeftDiv").css("display") == "table-cell") {
            screenWidth = screenWidth - 200;
        }
        if ($("#controlRightDiv").css("display") == "" || $("#controlRightDiv").css("display") == "table-cell") {
            screenWidth = screenWidth - 280;
        }
        if (screenWidth < 840) {
            $("#controlMainDiv").css("width", "830px");
        }
        else {
            $("#controlMainDiv").css("width", screenWidth + "px");
        }
        $("#divHeader").css("width", $("#main").css("width"));
    },
    ControlRightDiv: function () {
        var leftWidth = 248;
        if ($("#tdRightSlide").css("display") == "table-cell") {
            $("#tdRightSlide").css("display", "none");
            $("#mainBarForRight").removeClass("mainBarRight");
            $("#mainBarForRight").addClass("mainBarLeft");

            leftWidth = 48;
        } else {
            $("#tdRightSlide").css("display", "table-cell");
            $("#mainBarForRight").removeClass("mainBarLeft");
            $("#mainBarForRight").addClass("mainBarRight");
        }
        t2v_main.SetControlMainDivWidth();
    },
    ConfirmExportForEdit: function (fn) {
        var msg = "You have not saved your changes. Click Ok to export the last saved data or Cancel to save data.";
        t2v_util.t2vconfirm.showConfirm(msg, function () { fn(); }, function () { });
    },
    /*Peter Sun add water mark js*/
    //    addwatermark: function (obj, watermark) {
    //        if (watermark == undefined || watermark == "") {
    //            obj.each(function () {
    //                var watermarktext = "Enter " + $(this).attr("watermark");
    //                $(this).blur(function () {
    //                    if ($(this).val().length == 0) {
    //                        if ($(this)[0].type == "password") {
    //                            $(this)[0].type = "text";
    //                        }
    //                        $(this).val(watermarktext).addClass('watermark');
    //                    }
    //                }).focus(function () {
    //                    if ($(this).val() == watermarktext) {
    //                        if ($(this).val() == "Enter Password") {
    //                            $(this)[0].type = "password";
    //                        }
    //                        $(this).val('').removeClass('watermark');
    //                    }
    //                }).addClass('watermark');
    //                if ($(this).val() == "") {
    //                    $(this).val(watermarktext)
    //                }
    //            })
    //        }
    //    },
    ShowFeedbackScreen: function () {
        $.ajax({
            url: "/Home/ShowFeedbackView",
            type: "POST",
            success: function (data) {
                $("#divFeedback").css("left", (screen.width / 2)).css("top", (screen.height / 2)).css("height", "200px").css("width", "500px");
                $("#divFeedbackArea").html(data);
                $('#divFeedback').modal("show");
            },
            error: function (status) {
                if (status.status != 401) {
                    alert(status);
                }
            }
        });
    },
    SaveFeedback: function () {

        var mes = "";
        var sendTo = $("#tblUploadFunction").find("#hidUserID").val();

        mes = t2v_Authorization.ConvertToJson($("#txtFeedbackInfo").val());
        if (mes == "") {
            $("#divFeedbackError").html("Can not be empty.");
            return;
        }
        else {
            $("#divFeedbackError").html("");
        }
        mes = t2v_util.valid.ConvertToHtml(mes);
        //save message to db
        T2VForm.Post("/Home/SaveFeedback", { strFeedback: mes }
                    , function (serverResponse) {
                        $("#divFeedback").modal("hide");
                    });
    },
    /*
    showactivity: function (id, flag) {
        var url = "../../Handler/DWQManagerService.asmx/GetActivities";
        try {
            T2VForm.Post(url, { userID: id, flag: flag }
                        , function (serverResponse) {
                            //debugger;
                            var obj = t2v_util.valid.ConvertResponseToObj(serverResponse);
                            var ulContainer = $("#ul_activeLog");
                            ulContainer.empty();
                            for (var i = 0; i < (obj ? obj.length : 0); i++) {
                                var history = obj[i];
                                var msgTitle = history.OperationInfo.length > 24 ? history.OperationInfo.substr(0, 24) + "..." : history.OperationInfo;
                                var msg = "<b>" + history.NickName + "</b>&nbsp;" + msgTitle + "</span><span class='timeago' title='" + history.OperationTime + "'>";

                                var messageBody = history.OperationInfoBody + " </br>at " + history.OperationTime;
                                //var messageBody = history.OperationUserName + " " + history.OperationInfo + " " + history.QuoteNum + " </br>at " + history.OperationTime;
                                if (flag != "OCZ") {
                                    ulContainer.append("<li class = 'tipclass' title = '" + messageBody + "' onclick = 't2v_Queue.ShowQueueInfoByNumber(\"" + history.QuoteNum + "\")'>" + msg + "</span></li>");
                                }
                                else {

                                    messageBody = history.OperationUserName + " " + history.OperationInfo + " " + history.QuoteNum + " </br>at " + history.OperationTime;
                                    if (history.OperationType == "Authorization") {
                                        ulContainer.append("<li class = 'tipclass' title = '" + messageBody + "' onclick='t2v_Authorization.ShowInfoByNumber(\"" + history.QuoteNum + "\");'>" + msg + "</span></li>");
                                    }
                                    else if (history.OperationType == "AuthorizationVIR") {
                                        ulContainer.append("<li class = 'tipclass' title = '" + messageBody + "' onclick='t2v_Authorization_vir.ShowInfoView(\"" + history.QuoteNum + "\");'>" + msg + "</span></li>");

                                    }
                                    else if (history.OperationType == "Claim") {
                                        ulContainer.append("<li class = 'tipclass' title = '" + messageBody + "' onclick='t2v_Claim.ShowInfoView(\"" + history.QuoteNum + "\");'>" + msg + "</span></li>");
                                    }
                                    else if (history.OperationType == "ClaimVIR") {
                                        ulContainer.append("<li class = 'tipclass' title = '" + messageBody + "' onclick='t2v_Claim_vir.ShowInfoView(\"" + history.QuoteNum + "\");'>" + msg + "</span></li>");
                                    }

                                }

                                ulContainer.find('.timeago').timeago();
                            }
                            $('.tipclass').yitip();
                            $('.tipclass').each(function () {
                                var tip = $(this).yitip("api");
                                tip.options.offest = { "left": 30, "top": 0 };
                                tip.setPosition("rightMiddle");
                                tip.setColor("blue");
                                $('.timeago').removeAttr("title");
                                $(this).removeAttr('title');
                            })
                        });
        }
        catch (e) {
        }
    }
    */
    NumericOnKeyUp: function (obj) {
        var price = "";

        var s = "";
        var temp = $(obj).attr("value").Trim();
        for (var i = 0; i < temp.length; i++) {
            s = temp.charAt(i);
            if ((s == ".") || (parseInt(s) >= 0 && parseInt(s) <= 9)) {
                price += s;
            }
        }

        $(obj).attr("value", price);
    },
    NumericOnBlur: function (obj, decimalLength) {
        var price = $(obj).attr("value").Trim();

        if (price == "0") {
            $(obj).attr('value', "");
            return;
        }
        if (isNaN(price)) {
            return;
        }

        if (price != "") {
            var n = 0;
            for (var i = 0; i < price.length; i++) {
                if (price.charAt(i) == "0") {
                    n++;
                } else {
                    break;
                }
            }
            price = price.substring(n);

            var decimalValue = "";
            if (price != "") {
                if (price.indexOf(".") == 0) {
                    price = "0" + price;
                }
                if (price.indexOf(".") > 0) {
                    decimalValue = price.substring(price.indexOf(".") + 1);
                    if (decimalValue.length > decimalLength) {
                        if (decimalLength != 0) {
                            price = price.substring(0, price.indexOf(".") + (decimalLength + 1));
                        }
                        else {
                            price = price.substring(0, price.indexOf("."));
                        }
                    } else {
                        if (decimalLength != 0) {
                            for (var i = 0; i < decimalLength - decimalValue.length; i++) {
                                price = price + "0";
                            }
                        }
                        else {
                            price = price.substring(0, price.indexOf("."));
                        }
                    }
                } else {
                    for (var i = 0; i < decimalLength; i++) {
                        decimalValue += "0";
                    }
                    if (decimalLength != 0) {
                        price = price + "." + decimalValue;
                    }
                    else {
                        price = price;
                    }
                }
            } else {
                for (var i = 0; i < decimalLength; i++) {
                    decimalValue += "0";
                }
                if (decimalLength != 0) {
                    price = "0." + decimalValue;
                }
                else {
                    price = "0";
                }
            }
        }


        $(obj).attr('value', price);
    },
    NumericOnKeyDown: function (obj, e) {
        var errFlag = true;

        e = (e) ? e : ((window.event) ? window.event : "")
        var eventKeyCode = e.keyCode ? e.keyCode : e.which;
        if (eventKeyCode == 8 || eventKeyCode == 9 ||
            eventKeyCode == 16 || eventKeyCode == 17 ||
            eventKeyCode == 35 || eventKeyCode == 36 ||
            eventKeyCode == 37 || eventKeyCode == 39 ||
            eventKeyCode == 46) {
            errFlag = false;
        }
        if ((eventKeyCode >= 48 && eventKeyCode <= 57) || (eventKeyCode >= 96 && eventKeyCode <= 105)) {
            errFlag = false;
        }
        if (eventKeyCode == 110 || eventKeyCode == 190) {
            if ($(obj).attr("value").indexOf(".") == -1) {
                errFlag = false;
            }
        }

        if (errFlag) {
            if (navigator.userAgent.indexOf('Firefox') != -1) {
                return false;
            } else {
                e.keyCode = 35;
            }
        }
    }
};
