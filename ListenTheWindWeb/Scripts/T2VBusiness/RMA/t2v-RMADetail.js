
soNgModule.controller("QuoteDetailCtrl", ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {
    /* <Initialization> */
    $scope.quote = JSON.parse(t2v_Quote.quoteInfo);
    $scope.currentItemIndex = 0;


    $scope.currentConfigIndex = 0;
    $scope.currentGroupIndex = 0;
    $scope.currentProductIndex = 0;


    if (!$scope.quote.ConfigLine) {
        $scope.quote.ConfigLine = [];
        $scope.quote.ConfigLine.push(createConfigLine());
    };

    /* </Initialization> */

    $scope.fromJson = function (str) {
        return angular.fromJson(str);
    };

    $scope.previousConfig = function () {
        if ($scope.currentConfigIndex <= 0)
            $scope.currentConfigIndex = 0;
        else {
            --$scope.currentConfigIndex;
        }

        $scope.currentGroupIndex = 0;
        $scope.currentProductIndex = 0;
        t2v_Quote.GetCurrentInfo($scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products[$scope.currentProductIndex].Product);
    };

    $scope.nextConfig = function () {
        if ($scope.currentConfigIndex == $scope.quote.ConfigLine.length - 1) {
        }
        else {
            ++$scope.currentConfigIndex;
        }

        $scope.currentGroupIndex = 0;
        $scope.currentProductIndex = 0;
        t2v_Quote.GetCurrentInfo($scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products[$scope.currentProductIndex].Product);
    };

    $scope.currentConfig = function () {
        return $scope.quote.ConfigLine[$scope.currentConfigIndex];
    };

    $scope.showMsgArea = function () {
        $("#MyPasteZone").html("<span id='spPlaceHolder' style='color:Gray'>Enter a message here. Only support PASTE image under chrome.</span>");
        $("#divTarea").toggle();
    };

    $scope.createMessage = function () {
        if ($("#MyPasteZone").find("#spPlaceHolder").length > 0 || $("#MyPasteZone").find("#imgLoading").length > 0) {
            return;
        }

        var msgs = $("#MyPasteZone").html();
        if ($.trim(msgs) != "") {
            $("#MyPasteZone").html("<span id='spPlaceHolder' style='color:Gray'>Enter a message here. Only support PASTE image under chrome.</span>");
            $("#divTarea").toggle();

            //get send to user id
            var res = {
                CreateUserName: $.cookie('UserName'),
                CreateUserId: $.cookie('UserID'),
                CreateTime: new Date(),
                MessageInfo: msgs,
                MessageType: "Quote",
                OID: $scope.quote.QuoteID,
                IsNewAdd: 1,
                SendTo: $("#hidSendToUserId").val(),
                IsRead: $("#hidSendToUserId").val() == "" ? "" : 0,
                InfoForPreview: msgs.replace('<div>', '').replace('</div>', '').replace('<br>', '').replace('<br />', '')
            };
            $scope.quote.Messages.splice(0, 0, res);

            if ($scope.quote.QuoteID != 0) {
                //send message to backend.
                $http.post("/Quote/SaveQuoteMessage", { message: res }).success(function (data, status, headers, config) {

                }).error(function (data, status, headers, config) { t2v_angular.alertDebugError(d, s, h, c); });
            }
        }
    };

    $scope.previousGroup = function () {
        if ($scope.currentGroupIndex <= 0)
            $scope.currentGroupIndex = 0;
        else {
            --$scope.currentGroupIndex;
        }
        $scope.currentProductIndex = 0;
        t2v_Quote.GetCurrentInfo($scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products[$scope.currentProductIndex].Product);
    };

    $scope.nextGroup = function () {
        if ($scope.currentGroupIndex == $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine.length - 1) {
        }
        else {
            ++$scope.currentGroupIndex;
        }
        $scope.currentProductIndex = 0;
        t2v_Quote.GetCurrentInfo($scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products[$scope.currentProductIndex].Product);
    };
    $scope.currentGroup = function () {
        return $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex];
    };

    $scope.previousProduct = function () {
        if ($scope.currentProductIndex <= 0)
            $scope.currentProductIndex = 0;
        else {
            --$scope.currentProductIndex;
        }
        t2v_Quote.GetCurrentInfo($scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products[$scope.currentProductIndex].Product);
    }
    $scope.nextProduct = function () {
        if ($scope.currentProductIndex == $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products.length - 1) {
        }
        else {
            ++$scope.currentProductIndex;
        }
        t2v_Quote.GetCurrentInfo($scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products[$scope.currentProductIndex].Product);
    }
    $scope.currentProduct = function () {
        return $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products[$scope.currentProductIndex];
    };

    /*
       $scope.currentItem = function () {
        return $scope.quote.QuoteLine[$scope.currentItemIndex];
    };
    $scope.nextItem = function () {
        if ($scope.currentItemIndex >= $scope.quote.QuoteLine.length - 1)
            $scope.currentItemIndex = $scope.quote.QuoteLine.length - 1;
        else {
            ++$scope.currentItemIndex;
            $scope.currentLineIndex = 0;
        }
        t2v_Quote.DetailChangeCalcStatus();
    };

    $scope.previousItem = function () {
        if ($scope.currentItemIndex <= 0)
            $scope.currentItemIndex = 0;
        else {
            --$scope.currentItemIndex;
            $scope.currentLineIndex = 0;
        }
        t2v_Quote.DetailChangeCalcStatus();
    };
    */

    $scope.editQuote = function () {
        $.ajax({
            url: SiteRoot + "/Quote/Edit",
            type: 'post',
            data: { id: $scope.quote.QuoteID },
            beforeSend: function () {
                t2v_Quote._showLoading();
            },
            success: function (data, status, jqXhr) {
                t2v_Quote._stuffPage(data);
                t2v_Quote._hideLoading();
            }
        });
    };

    $scope.deleteQuote = function () {

        t2v_util.t2vconfirm.showConfirm("Are you sure you want to delete ?", function () {
            $.ajax({
                url: SiteRoot + "/Quote/DeleteQuote",
                type: 'post',
                data: { quoteID: $scope.quote.QuoteID },
                beforeSend: function () {
                    t2v_Quote._showLoading();
                },
                success: function (data, status, jqXhr) {
                    //t2v_Quote._hideLoading();
                    t2v_util.t2valert.showAlert("Delete successful .", "HDS", function () { window.location.href = "/Quote"; });
                }
            });
        }, function () { }, "HDS");
    };
    $scope.cancelQuote = function () {
        t2v_util.t2vconfirm.showConfirm("Are you sure you want to cancel this quote ?", function () {
            $.ajax({
                url: SiteRoot + "/Quote/CancelQuote",
                type: 'post',
                data: { ID: $scope.quote.QuoteID },
                beforeSend: function () {
                    t2v_Quote._showLoading();
                },
                success: function (data, status, jqXhr) {
                    //t2v_Quote._hideLoading();
                    var timestamp = (new Date()).valueOf();
                    window.location.href = "/Quote#/Detail/" + $scope.quote.QuoteID + "/t/" + timestamp;
                }
            });
        }, function () { }, "HDS");
    };
    $scope.downLoadFile = function (fileGuidName, attachmentFileName) {
        location.href = SiteRoot + '/Quote/DownLoad?fileGuidName=' + fileGuidName + '&attachmentFileName=' + attachmentFileName;
    };

    $scope.attachemnt = function () {
        $("#divUpload").css("left", screen.width / 2 - 60).css("height", "400px").css("width", "800px");
        $('#divUpload').modal('show');
        $scope.Upload();
    };

    $scope.Upload = function () {
        $scope.InitAttachType();
        $("#fine-uploader-left").fineUploader({
            request: {
                endpoint: SiteRoot + '/Quote/Upload'
            },
            params: {},
            multiple: false
        }).on('complete', function (event, Id, fileName, jsonResult) {
            if (jsonResult.success) {
                var ConfigProduct = "";
                if ($("#selAttachType").val() == "SPA") {
                    ConfigProduct = $("#selConfigID").find("option:selected").text();
                }
                var res = {
                    CreateUserName: $.cookie('UserName'),
                    CreateTime: new Date(),
                    AttachmentFileName: jsonResult.fileName,
                    LocalFileName: jsonResult.fileGuidName,
                    AttachmentType: $("#selAttachType").val(),
                    ConfigID: $("#selConfigID").val(),
                    Config: ConfigProduct,
                    QuoteMailAttachmentID: jsonResult.ID
                };
                $scope.quote.QuoteAttachments.splice(0, 0, res);
                $scope.$digest();

                //$scope.InitAttachType();
                //Try to update document check list.
                if (res.AttachmentType == "Request for SPA")
                    $("#imgRequestforquote").attr("src", "../../Content/img/UploadType_ProofTrue.png");
                else if (res.AttachmentType == "SPA") {
                    $("#imgQuote").attr("src", "../../Content/img/UploadType_ProofTrue.png");
                    var tempflag = true;
                    for (var i = 0; i < $scope.quote.ConfigLine.length; i++) {
                        if ($scope.quote.ConfigLine[i].ID == $("#selConfigID").val()) {
                            $scope.quote.ConfigLine[i].Status = "Requested-Response";
                        }
                        else {
                            if ($scope.quote.ConfigLine[i].Status != "Requested-Response")
                            {
                                tempflag = false;
                            }
                        }
                    }
                    if (tempflag) {
                        $scope.quote.Status = "Requested-Response";
                    }
                    $scope.$apply();
                }
                else if (res.AttachmentType == "Request For Credit")
                    $("#imgRequestforCredit").attr("src", "../../Content/img/UploadType_ProofTrue.png");
                else if (res.AttachmentType == "Credit Note")
                    $("#imgCreditNote").attr("src", "../../Content/img/UploadType_ProofTrue.png");
                else if (res.AttachmentType == "Other")
                    $("#imgOther").attr("src", "../../Content/img/UploadType_ProofTrue.png");
                jsonResult.success = false;
                $scope.Upload();

            }

        });
        $("#fine-uploader-left").fineUploader('setParams', { 'ProjectID': $scope.quote.QuoteID, 'AttachType': $("#selAttachType").val(), 'ConfigID': $("#selConfigID").val() });
        jsonResult.success = false;
    };

    $scope.InitAttachType = function () {
        var quoteFlag = false;
        for (var i = 0; i < $scope.quote.ConfigLine.length; i++) {

            if ($scope.quote.ConfigLine[i].Status == "Requested" || $scope.quote.ConfigLine[i].Status == "Requested-Resubmit") {
                quoteFlag = true;
                break;
            }
        }
        //if ($scope.quote.Status == "Requested" || $scope.quote.Status == "Requested-Resubmit") {
        if (quoteFlag) {
            $("#selAttachType").empty().append("<option value='SPA'>SPA</option><option value='Request For Credit'>Request For Credit</option><option value='Credit Note'>Credit Note</option><option value='Other'>Other</option>");
        }
        else {
            $("#selAttachType").empty().append("<option value='Request For Credit'>Request For Credit</option><option value='Credit Note'>Credit Note</option><option value='Other'>Other</option>");
        }
        var configs = "";
        for (var i = 0; i < $scope.quote.ConfigLine.length; i++)
        {
            
            var configflag = true;
            if ($scope.quote.ConfigLine[i].Status == "Requested" || $scope.quote.ConfigLine[i].Status == "Requested-Resubmit") {
                //for (var j = 0; j < $scope.quote.QuoteAttachments.length; j++)
                //{
                //    if ($scope.quote.QuoteAttachments[j].AttachmentType == "SPA" && $scope.quote.QuoteAttachments[j].ConfigID == $scope.quote.ConfigLine[i].ID)
                //    {
                //        configflag = false;
                //        break;
                //    }
                //}
                //if (configflag) {
                configs += "<option value='" + $scope.quote.ConfigLine[i].ID + "'>" + $scope.quote.ConfigLine[i].Product + "</option>";
                //}
            }
        }
        $("#selConfigID").empty().append(configs);
        if (configs == "")
        {
            $("#selAttachType option[value='SPA']").remove();
        }
        //for (var i = 0; i < $scope.quote.QuoteAttachments.length; i++) {
        //    if ($scope.quote.QuoteAttachments[i].AttachmentType == "SPA") {
        //        $("#selAttachType option[value='SPA']").remove();
        //    }
        //}
    };

    $scope.deleteFile = function (fileGuidName, attachmentType) {
        if (attachmentType == "Request for SPA") {
            return;
        }
        if (attachmentType == "SPA") {
            return;
        }

        $.ajax({
            url: SiteRoot + "/Quote/DeleteFile",
            type: 'post',
            async: false,
            data: { fileGuidName: fileGuidName },
            success: function (data, status, jqXhr) {
                for (var i = 0; i < $scope.quote.QuoteAttachments.length; i++) {
                    if ($scope.quote.QuoteAttachments[i].LocalFileName.toString() == fileGuidName.toString()) {
                        var AttachType = $scope.quote.QuoteAttachments[i].AttachmentType;
                        $scope.quote.QuoteAttachments.splice(i, 1);
                        var bFlag = false;
                        for (var j = 0; j < $scope.quote.QuoteAttachments.length; j++) {
                            if (AttachType == $scope.quote.QuoteAttachments[j].AttachmentType) {
                                bFlag = true;
                                break;
                            }
                        }
                        if (!bFlag) {
                            if (AttachType == "Request for SPA")
                                $("#imgRequestforquote").attr("src", "../../Content/img/UploadType_ProofFalse.png");
                            else if (AttachType == "SPA")
                                $("#imgQuote").attr("src", "../../Content/img/UploadType_ProofFalse.png");
                            else if (AttachType == "Request For Credit")
                                $("#imgRequestforCredit").attr("src", "../../Content/img/UploadType_ProofFalse.png");
                            else if (AttachType == "Credit Note")
                                $("#imgCreditNote").attr("src", "../../Content/img/UploadType_ProofFalse.png");
                            else if (AttachType == "Other")
                                $("#imgOther").attr("src", "../../Content/img/UploadType_ProofFalse.png");
                        }
                        break;
                    }
                }
                $scope.InitAttachType();
                $("#fine-uploader-left").fineUploader('setParams', { 'ProjectID': $scope.quote.QuoteID, 'AttachType': $("#selAttachType").val(), 'ConfigID': $("#selConfigID").val() });
            }
        });

    };

    $scope.showNotes = function (obj) {
        $scope.noteSeat = obj;
        $('#dialogNote').modal('show');
    };

    $scope.SetCurrentLinePosition = function () {
        //var currentHref = window.location.href;
        //var arrayGuid = currentHref.substring(currentHref.lastIndexOf('/') + 1, currentHref.length - 1).split(',');

        var bomLineGuid = t2v_Quote.QuoteItemGuid;
        var approveLineGuid = t2v_Quote.QuoteItemLineGuid;

        var bomIndex = 0;
        var lineIndex = 0;
        for (i = 0; i < $scope.quote.QuoteLine.length; i++) {
            if ($scope.quote.QuoteLine[i].QuoteItemID == bomLineGuid) {
                bomIndex = i;
            }
            for (j = 0; j < $scope.quote.QuoteLine[i].QuoteItemLines.length; j++) {
                if ($scope.quote.QuoteLine[i].QuoteItemLines[j].QuoteItemLineID == approveLineGuid) {
                    lineIndex = j;
                }
            }
        }

        $scope.currentItemIndex = bomIndex;
        $scope.currentLineIndex = lineIndex;
    };

    $scope.ChangeLinePage = function (bomlineno, index, quoteItemGuid, quoteItemLineGuid) {


        t2v_Quote.QuoteItemGuid = quoteItemGuid;
        t2v_Quote.QuoteItemLineGuid = quoteItemLineGuid;

        $scope.currentItemIndex = bomlineno;
        $scope.currentLineIndex = index;
    };

}]);

