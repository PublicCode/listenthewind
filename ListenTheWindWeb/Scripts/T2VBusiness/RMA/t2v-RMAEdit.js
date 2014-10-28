soNgModule.controller("QuoteEditCtrl", ['$scope', '$routeParams', '$http', '$filter', '$compile', function ($scope, $routeParams, $http, $filter, $compile) {
    $scope.currentConfigIndex = 0;
    $scope.currentGroupIndex = 0;
    $scope.currentProductIndex = 0;
    var createConfigLine = function ()
    {
        var config = {
            ID: 0,
            QuoteID: 0,
            PartnerName: "",
            PartnerID:0,
            Product: "",
            ProductDesc: "",
            Status: "Draft",
            ContactName: "",
            ContactPhone: "",
            ContactEmail: "",
            Qty:1,
            TPVRebateTarget: $scope.quote.TVPRebateTarget,
            TPVRebateFinal:0,
            listPartner: $scope.quote.listPartner,
            GroupLine: [],
            PosIndex: 0
        };
        config.GroupLine.push(createGroupLine());
        return config;
    };

    var createGroupLine = function ()
    {
        var group =
            {
                ID: 0,
                ConfigID: 0,
                Product: "",
                ProductDesc: "",
                Qty: 1,
                TPVRebate: 0,
                Products: [],
                PosIndex: 0
            };
        group.Products.push(createProductLine());
        return group;
    };
    var createProductLine = function ()
    {
        var product = {
            ID: 0,
            GroupID: 0,
            Product: "",
            ProductDesc: "",
            Qty:0,
            TPVRebateUnit: 0,
            TPVRebateExt: 0,
            PosIndex: 0
        }
        return product;
    }
    
    $scope.integerval = /[+-]?([0-9]+(\.[0-9]+)?)/;
    $scope.quote = JSON.parse(t2v_Quote.quoteInfo);

    /* <Initialization> */

    $scope.fromJson = function (str) {
        return angular.fromJson(str);
    };


    if (!$scope.quote.ConfigLine) {
        $scope.quote.ConfigLine = [];
        var configLine = createConfigLine();
        
        $scope.quote.ConfigLine.push(configLine);

        $scope.quote.ConfigLine[$scope.currentConfigIndex].PosIndex = $scope.currentConfigIndex + 1;
        $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].PosIndex = 1;
        $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products[$scope.currentProductIndex].PosIndex = 1;
    };

    $scope.previousConfig = function ()
    {
        if ($scope.currentConfigIndex <= 0)
            $scope.currentConfigIndex = 0;
        else {
            --$scope.currentConfigIndex;
        }
        $scope.currentGroupIndex = 0;
        $scope.currentProductIndex = 0;
        t2v_Quote.GetCurrentInfo($scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products[$scope.currentProductIndex].Product);
    };

    $scope.nextConfig = function ()
    {
        if ($scope.currentConfigIndex == $scope.quote.ConfigLine.length - 1) {
            $scope.quote.ConfigLine.push(createConfigLine());
            ++$scope.currentConfigIndex;
        }
        else {
            ++$scope.currentConfigIndex;
        }

        $scope.currentGroupIndex = 0;
        $scope.currentProductIndex = 0;
        
        $scope.quote.ConfigLine[$scope.currentConfigIndex].PosIndex = $scope.currentConfigIndex+1;
        $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].PosIndex = 1;
        $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products[$scope.currentProductIndex].PosIndex = 1;

        $scope.setConfigPartnerInit($scope.quote.ConfigLine[$scope.currentConfigIndex].PartnerId);
        t2v_Quote.GetCurrentInfo($scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products[$scope.currentProductIndex].Product);
    };

    $scope.currentConfig = function () {
        return $scope.quote.ConfigLine[$scope.currentConfigIndex];
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
            $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine.push(createGroupLine());
            ++$scope.currentGroupIndex;
        }
        else {
            ++$scope.currentGroupIndex;
        }
        $scope.currentProductIndex = 0;


        $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].PosIndex = $scope.currentGroupIndex+1;
        $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products[$scope.currentProductIndex].PosIndex = 1;
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
    };

    $scope.nextProduct = function () {
        if ($scope.currentProductIndex == $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products.length - 1) {
            $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products.push(createProductLine());
            ++$scope.currentProductIndex;
        }
        else {
            ++$scope.currentProductIndex;
        }
        t2v_Quote.GetCurrentInfo($scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products[$scope.currentProductIndex].Product);
        $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products[$scope.currentProductIndex].PosIndex = $scope.currentProductIndex + 1;
    };

    $scope.currentProduct = function () {
        return $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products[$scope.currentProductIndex];
    };
    $scope.calculateGroupUnit = function ()
    {
        var currentProduct = $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products[$scope.currentProductIndex];
        if (!isNaN(parseFloat(currentProduct.Qty) && !isNaN(parseFloat(currentProduct.TPVRebateExt)))) {
            currentProduct.TPVRebateUnit = parseFloat(parseFloat(currentProduct.TPVRebateExt / currentProduct.Qty).toFixed(2));
        }

        var sumValue = 0;
        var currentObj = $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products;
        for (i = 0; i < currentObj.length; i++) {
            var tpvRebateExt = currentObj[i].TPVRebateExt;
            if (!isNaN(parseFloat(tpvRebateExt))) {
                sumValue += parseFloat(parseFloat(tpvRebateExt).toFixed(2));;
            }
        }

        $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].TPVRebate = sumValue;

        $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].TPVRebate = sumValue;

        var sumConfig = 0;
        for (i = 0; i < $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine.length; i++)
        {
            var tpvRebate = $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[i].TPVRebate;
            if (!isNaN(parseFloat(tpvRebate))) {
                sumConfig += parseFloat(parseFloat(tpvRebate).toFixed(2));
            }
        }
        
        $scope.quote.ConfigLine[$scope.currentConfigIndex].TPVRebateFinal = sumConfig;

        //set the header's TPVRebateunit
        if ($scope.quote.SelectConfigID != "" && $scope.quote.SelectConfigID != null && ($scope.quote.SelectConfigID == $scope.quote.ConfigLine[$scope.currentConfigIndex].ID))
        {
            $scope.quote.TVPRebateFinal = sumConfig;
        }
    };

    $scope.calculateGroupRebate = function () {

        var currentProduct = $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products[$scope.currentProductIndex];
        if(!isNaN(parseFloat(currentProduct.Qty) && !isNaN(parseFloat(currentProduct.TPVRebateUnit))))
        {
            currentProduct.TPVRebateExt = parseFloat(parseFloat(currentProduct.TPVRebateUnit * currentProduct.Qty).toFixed(2));
        }

        var sumValue = 0;
        var currentObj = $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products;
        for (i = 0; i < currentObj.length; i++)
        {
            var tpvRebateExt = currentObj[i].TPVRebateExt;
            if(!isNaN(parseFloat(tpvRebateExt)))
            {
                sumValue += tpvRebateExt;
            }
        }

        $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].TPVRebate = sumValue;

        var sumConfig = 0;
        for (i = 0; i < $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine.length; i++)
        {
            var tpvRebate = $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[i].TPVRebate;
            if(!isNaN(parseFloat(tpvRebate)))
            {
                sumConfig += tpvRebate;
            }
        }

        $scope.quote.ConfigLine[$scope.currentConfigIndex].TPVRebateFinal = sumConfig;

        //set the header's TPVRebateunit
        if ($scope.quote.SelectConfigID != "" && $scope.quote.SelectConfigID != null && ($scope.quote.SelectConfigID == $scope.quote.ConfigLine[$scope.currentConfigIndex].ID)) {
            $scope.quote.TVPRebateFinal = sumConfig;
        }
    };

    $scope.deleteConfig = function ()
    {
        t2v_util.t2vconfirm.showConfirm("Are you sure you want to delete ?", function () {

            var configLineId = $scope.quote.ConfigLine[$scope.currentConfigIndex].ID
            $http.post(SiteRoot + "/Quote/DeleteConfig", { ConfigId: configLineId }).success(function (data, status, headers, config) {
                //re order the config
                if (data.result == "Requested-Response")
                    $scope.quote.Status = "Requested-Response";
                $scope.quote.ConfigLine.splice($scope.currentConfigIndex, 1);
                $scope.reOrderConfig();
                if ($scope.currentConfigIndex == $scope.quote.ConfigLine.length && $scope.currentConfigIndex > 0)
                {
                    --$scope.currentConfigIndex;
                    $scope.setConfigPartnerInit($scope.quote.ConfigLine[$scope.currentConfigIndex].PartnerId);
                }

                //$http.post(SiteRoot + "/Quote/Save", $scope.quote)
                //.success(function (d, s, h, c) {
                //    //alert('update PosIndex Successful.');
                //})
                //.error(function (d, s, h, c) {
                //    t2v_angular.alertDebugError(d, s, h, c);
                //});
            }).error(function (data, status, headers, config) {
                alert('error');
            });

        }, function () { }, "HDS");
    };

    $scope.deleteGroup = function () {

        t2v_util.t2vconfirm.showConfirm("Are you sure you want to delete ?", function () {
        var groupLineId = $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].ID;
        $http.post(SiteRoot + "/Quote/DeleteGroup", { GroupId: groupLineId }).success(function (data, status, headers, config) {

            //re order the group
            $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine.splice($scope.currentGroupIndex, 1);
            $scope.reOrderGroup();

            if ($scope.currentGroupIndex == $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine.length && $scope.currentGroupIndex > 0) {
                --$scope.currentGroupIndex;
            }

            $scope.calculateGroupRebate();

            $http.post(SiteRoot + "/Quote/Save", $scope.quote)
            .success(function (d, s, h, c) {
               // alert('update PosIndex Successful.');
            })
            .error(function (d, s, h, c) {
                t2v_angular.alertDebugError(d, s, h, c);
            });

        }).error(function (data, status, headers, config) {
            alert('error');
        });
        }, function () { }, "HDS");
    };
    
    $scope.deleteProduct = function ()
    {
        t2v_util.t2vconfirm.showConfirm("Are you sure you want to delete ?", function () {
        var productId = $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products[$scope.currentProductIndex].ID;
        $http.post(SiteRoot + "/Quote/DeleteProduct", { ProductId: productId }).success(function (data, status, headers, config) {

            //re order the group
            $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products.splice($scope.currentProductIndex, 1);
            $scope.reOrderProduct();

            if ($scope.currentProductIndex == $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products.length && $scope.currentProductIndex > 0) {
                --$scope.currentProductIndex;
            }

            $scope.calculateGroupRebate();
            //Change info section.
            t2v_Quote.GetCurrentInfo($scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products[$scope.currentProductIndex].Product);
            $http.post(SiteRoot + "/Quote/Save", $scope.quote)
            .success(function (d, s, h, c) {
                //alert('update PosIndex Successful.');
            }).error(function (d, s, h, c) {
                t2v_angular.alertDebugError(d, s, h, c);
            });
        }).error(function (data, status, headers, config) {
            alert('error');
        });
        }, function () { }, "HDS");
    };


    $scope.reOrderConfig = function ()
    {
        for (i = 0; i < $scope.quote.ConfigLine.length; i++)
        {
            $scope.quote.ConfigLine[i].PosIndex = i + 1;
        }
    };

    $scope.ChangeSelectConfigID = function () {
        
        for (i = 0; i < $scope.quote.ConfigLine.length; i++) {
            if ($scope.quote.ConfigLine[i].ID == $scope.quote.SelectConfigID)
            {
                //$("#txtTVPRebateTarget").val(scope.quote.ConfigLine[i].TPVRebateTarget);
                //$("#txtTVPRebateFinal").val(scope.quote.ConfigLine[i].TPVRebateFinal);
                $scope.quote.TVPRebateTarget = $scope.quote.ConfigLine[i].TPVRebateTarget;
                $scope.quote.TVPRebateFinal = $scope.quote.ConfigLine[i].TPVRebateFinal;
                //scope.$apply();
                //t2v_Quote.CalcConfigHDSGPTarget();
                t2v_Quote.CalcConfigHDSGPFinal();
                break;
            }
        }
    },
    $scope.ChangeTVPRebateTarget = function () {
        
        for (i = 0; i < $scope.quote.ConfigLine.length; i++) {
            $scope.quote.ConfigLine[i].TPVRebateTarget = $scope.quote.TVPRebateTarget;
        }
        t2v_Quote.CalcConfigHDSGPTarget();
    },
    $scope.ChangeSelectType = function () {
        t2v_Quote.SetControlStatus($scope.quote.CustomerTypeName);
        t2v_Quote.CalcHDSGPTarget();
        t2v_Quote.CalcHDSGPFinal();
        t2v_Quote.CalcConfigHDSGPTarget();

    },
    $scope.reOrderGroup = function () {
        for (i = 0; i < $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine.length; i++)
        {
            $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[i].PosIndex = i + 1;
        }
    };
    $scope.reOrderProduct = function () {

        for (i = 0; i < $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products.length; i++) {
            $scope.quote.ConfigLine[$scope.currentConfigIndex].GroupLine[$scope.currentGroupIndex].Products[i].PosIndex = i + 1;
        }
    };

    $scope.NavigateToItem = function (itemIndex) {
        $scope.currentItemIndex = itemIndex
        t2v_Quote.DetailChangeCalcStatus();
        t2v_Quote.ChangeCalcStatus();
        t2v_Quote.GetCurrentLineValue();
    };
    $scope.deleteItem = function ()
    {
        $scope.quote.QuoteLine.splice($scope.currentItemIndex, 1);
        if ($scope.currentItemIndex == $scope.quote.QuoteLine.length && $scope.currentItemIndex > 0) {
            --$scope.currentItemIndex;
        }
        t2v_Quote.DetailChangeCalcStatus();
        t2v_Quote.ChangeCalcStatus();
        
        t2v_Quote.GetCurrentLineValue();
    };

    $http.get(SiteRoot + "/Quote/GetPartners")
        .success(function (data, status, headers, config) {
            $scope.partners = data;
            var found = $filter('getByProperty')('PartnerId', $scope.quote.PartnerId, $scope.partners);
            if (found == null || found == undefined)
                found = $scope.partners[0];
            $scope.partner = found;
            $scope.quote.PartnerId = found.PartnerId;

        }).error(function (data, status, headers, config) {
    });
    $scope.changePartner = function () {
        $scope.quote.PartnerId = $scope.partner.PartnerId;
        $scope.quote.PartnerName = $scope.partner.PartnerName;
    };

    $scope.changeConfigPartner = function () {
        $scope.quote.ConfigLine[$scope.currentConfigIndex].PartnerId = $scope.partner.PartnerId;
        $scope.quote.ConfigLine[$scope.currentConfigIndex].PartnerName = $scope.partner.PartnerName;
    };

    $scope.ValidateRequire = function ()
    {
        /*var isValidated = true;
        var nestedBreak = false;
        for (i = 0; i < $scope.quote.ConfigLine.length; i++)
        {
            if ($scope.quote.QuoteLine[i].Product == "" || $scope.quote.QuoteLine[i].Product == undefined)
            {
                isValidated = false;
                //NavigateToItem should have three parameters 1,conifg index,group index,product index.
                $scope.NavigateToItem(i,0,0);
                break;
            }

            for (j = 0; j < $scope.quote.ConfigLine[i].GroupLine.length; j++)
            {
                var groupProduct = $scope.quote.ConfigLine[i].GroupLine[j].Product;
                if (groupProduct == "" || groupProduct == undefined)
                {
                    isValidated = false;
                    $scope.NavigateToItem(i, j, 0);
                    break;
                }
                for (k = 0; k < $scope.quote.ConfigLine[i].GroupLine[j].Products.length;k++)
                {
                    var product = $scope.quote.ConfigLine[i].GroupLine[j].Products[k].Product;
                    if (product == "" || product == undefined)
                    {
                        isValidated = false;
                        $scope.NavigateToItem(i, j, 0);
                        break;
                    }
                }
            }
        }
        return isValidated;*/
        return true;
    };

    $scope.setCustomerTypeInit = function (typeValue) {
        if (typeValue == "")
        {
            $scope.quote.CustomerTypeName = $scope.quote.listCustomerTypeModel[0];
        }
    };

    $scope.setConfigPartnerInit = function (typeValue) {
        if (typeValue == "" || typeValue == undefined) {
            $scope.quote.ConfigLine[$scope.currentConfigIndex].PartnerId = $scope.quote.ConfigLine[$scope.currentConfigIndex].listPartner[0].PartnerId;
        }
    };
    $scope.saveQuote = function () {
            t2v_Quote._showLoading();
            if ($scope.quote.Status == "" || $scope.quote.Status == undefined)
                $scope.quote.Status = "Draft";
            $scope.quote.sendToEmail = "";
            $http.post(SiteRoot + "/Quote/Save", $scope.quote)
            .success(function (d, s, h, c) {
                var timestamp = (new Date()).valueOf();
                window.location.href = "/Quote#/Detail/" + eval(d.flag) + "/t/" + timestamp;
                //t2v_Quote._detail(d.data.quoteId);
            })
            .error(function (d, s, h, c) {
                t2v_angular.alertDebugError(d, s, h, c);
            });
    };
    $scope.submitQuote = function () {
        if ($scope.quoteForm.$valid && $scope.ValidateRequire() == true) {
            t2v_util.t2vconfirm.showConfirm("Send to Email?<br>Email: <input type = 'text' id = 'txtEmail' class = 'text ui-widget-content ui-corner-all' />", function () {
                var email = $("#txtEmail").val();
                if (email == "") {
                    return;
                }
                t2v_Quote._showLoading();
                $scope.quote.sendToEmail = email;
                if ($scope.quote.Status == "Requested-Modify")
                    $scope.quote.Status = "Requested-Resubmit";
                else
                    $scope.quote.Status = "Requested";

                $http.post(SiteRoot + "/Quote/Save", $scope.quote)
                    .success(function (d, s, h, c) {
                        t2v_Quote._hideLoading();
                        this.location.href = "/Quote#/Detail/" + eval(d.flag);
                        //t2v_Quote._detail(d.data.quoteId);
                    })
                    .error(function (d, s, h, c) {
                        t2v_angular.alertDebugError(d, s, h, c);
                    });
            }, function () { return; }, "HDS");
        }
        else {
            t2v_util.t2valert.showAlert("Please enter mandatory fields then submit.", "HDS", function () { });
        }
        
    };
    $scope.submitConfig = function ()
    {
        if ($scope.quoteForm.$valid) {
            t2v_util.t2vconfirm.showConfirm("Send to Email?<br>Email: <input type = 'text' id = 'txtEmail' class = 'text ui-widget-content ui-corner-all' />", function () {
                var email = $("#txtEmail").val();
                if (email == "") {
                    return;
                }
                t2v_Quote._showLoading();
                $scope.quote.sendToEmail = email;
                
                /*
                if ($scope.quote.Status == "Requested-Modify")
                    $scope.quote.Status = "Requested-Resubmit";
                else
                    $scope.quote.Status = "Requested";
                */

                $scope.quote.OperationAction = "submit";
                if ($scope.currentConfig().Status == "Requested-ReQuote")
                    $scope.currentConfig().Status = "Requested-Resubmit";
                else
                    $scope.currentConfig().Status = "Requested";
                $scope.quote.SubmitConfigPosIndex = $scope.currentConfig().PosIndex;
                $http.post(SiteRoot + "/Quote/Save", $scope.quote)
                    .success(function (d, s, h, c) {
                        t2v_Quote._hideLoading();
                        var result = eval("(" + d.flag + ")");
                        var timestamp = (new Date()).valueOf();
                        window.location.href = "/Quote#/Edit/" + eval(result.QuoteID) + "/t/" + timestamp;;
                        //alert(result.QuoteAttachment);
                        //$scope.quote.Messages = result.Messages;
                        //$scope.quote.Status = result.QuoteStatus;
                        //$scope.quote.QuoteAttachment = result.QuoteAttachment;
                        //$scope.quote.CheckListOfRquestForQuote = result.CheckListOfRquestForQuote;
                    })
                    .error(function (d, s, h, c) {
                        t2v_angular.alertDebugError(d, s, h, c);
                    });
            }, function () { return; }, "HDS");
        }
        else {
            t2v_util.t2valert.showAlert("Please enter mandatory fields then submit.", "HDS", function () { });
        }
    };
    $scope.requoteConfig = function () {
        var config = $scope.quote.ConfigLine[$scope.currentConfigIndex];
        t2v_util.t2vconfirm.showConfirm("Are you sure you want to redo this config? Please confirm.", function () {
            $.post(SiteRoot + "/Quote/ReQuoteConfig", { ConfigID: config.ID },
                function (data) {
                    if (data.result == true) {
                        $scope.quote.ConfigLine[$scope.currentConfigIndex].Status = 'Requested-ReQuote';
                        $scope.quote.Status = 'Requested-Modify';
                        $scope.$apply();
                    }
                });
        }, function () { }, "HDS");
    };
    $scope.requestTPV = function () {
        t2v_util.t2vconfirm.showConfirm("Send to Email?<br>Email: <input type = 'text' id = 'txtEmail' class = 'text ui-widget-content ui-corner-all' />", function () {
            var email = $("#txtEmail").val();
            if (email == "") {
                return;
            }
            t2v_Quote._showLoading();

            $scope.quote.Status = "Requested-TPV";
            $scope.quote.sendToEmail = email;

            $http.post(SiteRoot + "/Quote/Save", $scope.quote)
            .success(function (d, s, h, c) {
                t2v_Quote._hideLoading();
                var timestamp = (new Date()).valueOf();
                this.location.href = "/Quote#/Detail/" + d.flag + "/t/" + timestamp;
                //t2v_Quote._detail(d.data.quoteId);
            })
            .error(function (d, s, h, c) {
                t2v_angular.alertDebugError(d, s, h, c);
            });

        }, function () { return; }, "HDS");
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
                    t2v_util.t2valert.showAlert("Delete successful .", "HDS", function () { window.location.href = '/Quote'; });
                }
            });
        }, function () { }, "HDS");
    };

    $scope.cancelEdit = function () {
        if ($scope.quote.QuoteID == "") {
            t2v_Quote.index();
        } else {
            var timestamp = (new Date()).valueOf();
            window.location.href = "/Quote#/Detail/" + $scope.quote.QuoteID + "/t/" + timestamp;
            //t2v_Quote._detail($scope.quote.QuoteID);
        }
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
                InfoForPreview:msgs.replace('<div>', '').replace('</div>', '').replace('<br>', '').replace('<br />', '')
            };
            $scope.quote.Messages.splice(0, 0, res);

            if ($scope.quote.QuoteID != 0) {
                //send message to backend.
                $http.post("/Quote/SaveQuoteMessage", { message: res }).success(function (data, status, headers, config) {

                }).error(function (data, status, headers, config) { t2v_angular.alertDebugError(d, s, h, c); });
            }
        }
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
                if ($("#selAttachType").val() == "SPA")
                {
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
                            if ($scope.quote.ConfigLine[i].Status != "Requested-Response") {
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
        for (var i = 0; i < $scope.quote.ConfigLine.length; i++) {

            var configflag = true;
            if ($scope.quote.ConfigLine[i].Status == "Requested" || $scope.quote.ConfigLine[i].Status == "Requested-Resubmit") {
                //for (var j = 0; j < $scope.quote.QuoteAttachments.length; j++) {
                //    if ($scope.quote.QuoteAttachments[j].AttachmentType == "SPA" && $scope.quote.QuoteAttachments[j].ConfigID == $scope.quote.ConfigLine[i].ID) {
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
        if (configs == "") {
            $("#selAttachType option[value='SPA']").remove();
        }
        //for (var i = 0; i < $scope.quote.QuoteAttachments.length; i++) {
        //    if ($scope.quote.QuoteAttachments[i].AttachmentType == "SPA") {
        //        $("#selAttachType option[value='SPA']").remove();
        //    }
        //}
    };

    $scope.deleteFile = function (fileGuidName, attachmentType) {
        if (attachmentType == "Request for SPA")
        {
            return;
        }
        if (attachmentType == "SPA") {
            return;
        }

        $.ajax({
            url: SiteRoot + "/Quote/DeleteFile",
            type: 'post',
            async: false,
            data: { fileGuidName: fileGuidName},
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
                jsonResult.success = false;
            }
        });

    };

    $scope.repo = new function () {
        var self = this;
        this.noteTypes = [
            { 'display': 'Sourcing', 'value': 'Sourcing' },
            { 'display': 'Engineering', 'value': 'Engineering' },
            { 'display': 'Other', 'value': 'Other' }
        ];

        this.getRemainingNoteTypes = function (obj) {
            var usedIndices = {};
            var notUsed = [];
            var notes = obj ? obj.Notes : []; // Get its notes.

            // Remove used note types.
            angular.forEach(notes, function (rex, k) {
                if (rex.Type != 'Other') { // Will not remove `other` by all means.
                    var _found = null; // The index of the note type that's been found in the used list.
                    $.each($scope.repo.noteTypes, function (i, val) { // Iterate through the note types 
                        if (val.value == rex.Type) { // Get the one that matches the note
                            usedIndices[i] = true;
                            return false; // Stop looping
                        }
                        return true; // Continue looking up
                    });
                }
            });
            $.each($scope.repo.noteTypes, function (i, val) {
                if (!usedIndices[i]) {
                    notUsed.push(val);
                }
            });

            return notUsed;
        };
    };

    $scope.SetCurrentLinePosition = function () {
        var bomLineGuid = t2v_Quote.QuoteItemGuid;;
        var approveLineGuid = t2v_Quote.QuoteItemLineGuid;;
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
    };

    $scope.ChangeLinePage = function (bomlineno, index, quoteItemGuid, quoteItemLineGuid) {

        t2v_Quote.QuoteItemGuid = quoteItemGuid;

        $scope.currentItemIndex = bomlineno;
    };
    $scope.ChangeManageService = function (applicationTransport) {
        if ($scope.quote.ManageService) {
            $("#txtCustomerName").attr("readonly", true);
            $("#btnCustomerName").attr("disabled", "disabled");
            $scope.quote.AccountName = "Hitachi Data Systems";
            $scope.quote.OpportunityName = "HDS Managed Services Infrastructure project";
        }
        else {
            $("#txtCustomerName").attr("readonly", false);
            $("#btnCustomerName").removeAttr('disabled');
        }
    };

    $scope.AddNewCompetitor = function () {
        var timestamp = (new Date()).valueOf();
        var newCell = "<tr class='NewItem' id = '" + timestamp + "'><td><input style='width:62px' type = 'text' id = 'txtNewMfr" + timestamp + "' /></td>";
        newCell += "<td><input style='width:124px' type = 'text' id = 'txtProduct" + timestamp + "' /></td>";
        newCell += "<td><input style='width:50px' type = 'text' id = 'txtPrice" + timestamp + "' /></td><td></td></tr>";
        newCell += "<tr id='trSubmit'><td colspan='4'><input type = 'button' style='float:right' ng-click='SaveNewCompetitor()' class='SubmitWithRightIcon'/></td></tr>";
        $("#trSubmit").remove();
        var test = $compile(newCell)($scope)
        $("#tbCompetitor").append(test);
    };
    $scope.DeleteCompetitor = function (idx) {
        var competitor = $scope.quote.Competitors[idx];
        $http.post(SiteRoot + "/Quote/DeleteCompetitor", { ID: competitor.ID })
            .success(function (d, s, h, c) {
                // alert('update PosIndex Successful.');
            })
            .error(function (d, s, h, c) {
                t2v_angular.alertDebugError(d, s, h, c);
            });
        $scope.quote.Competitors.splice(idx, 1);
    };
    $scope.AcceptQuote = function () {
        if ($scope.quoteForm.$valid)
        {
            t2v_util.t2vconfirm.showConfirm("Are you sure you want to accept? ", function () {
                t2v_Quote._showLoading();
                $http.post(SiteRoot + "/Quote/AcceptQuote", $scope.quote).success(function (d, s, h, c) {

                    var timestamp = (new Date()).valueOf();
                    window.location.href = "/Quote#/Detail/" + $scope.quote.QuoteID + "/t/" + timestamp;
                }).error(function (d, s, h, c) {
                    t2v_angular.alertDebugError(d, s, h, c);
                });
                //$scope.quote.Competitors.splice(idx, 1);

            }, function () { },"HDS");
        }
    };
    $scope.SaveNewCompetitor = function () {
        $(".NewItem").each(function (k) {
            var id = this.id;
            var competitor = {
                ID: 0,
                QuoteID: $scope.quote.QuoteID,
                Mfr: $("#txtNewMfr"+id).val(),
                ProductOrSolution: $("#txtProduct"+id).val(),
                EndUserPrice: $("#txtPrice" + id).val(),
                Action: 'New',
            };
            if (competitor.Mfr != "" || competitor.ProductOrSolution != "" || competitor.EndUserPrice !="")
                $scope.quote.Competitors.push(competitor);
        });
        $(".NewItem").remove();
        $("#trSubmit").remove();
        $scope.$apply();
    };
}]);