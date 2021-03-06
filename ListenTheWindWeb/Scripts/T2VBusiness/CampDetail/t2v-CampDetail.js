﻿var SiteRoot = t2v_util.history.GetRootPath();
var t2v_CampDetail = {

    CampInfo: "",
    PileId: 0,
    BasicData: "",
    DefaultDate: "",
};
var t2v_CampBook = new function() {
    var self = this;
    self.GetTotalCampBookPrice = function(){
        var scope = angular.element($("#ng-value")[0]).scope();
        scope.getTotalCampPrice();
    };
};
var t2v_CampOrders = {
    showUnpaid: function () {
        $.ajax({
            url: "/Home/UnpaidView",
            type: 'post',
            data: {},
            beforeSend: function () {
            },
            success: function (data, status, jqXhr) {
                $("#unpiadlink").addClass("crr");
                $("#copletedlink").removeClass("crr");
                $("#paidlink").removeClass("crr");
                $("#divContent").html(data);
            }
        });
    },
    showPaid: function () {
        $.ajax({
            url: "/Home/PaidView",
            type: 'post',
            data: {},
            beforeSend: function () {
            },
            success: function (data, status, jqXhr) {
                $("#paidlink").addClass("crr");
                $("#unpiadlink").removeClass("crr");
                $("#copletedlink").removeClass("crr");
                $("#divContent").html(data);
            }
        });
    },
    showCompleted: function () {
        $.ajax({
            url: "/Home/CompletedView",
            type: 'post',
            data: {},
            beforeSend: function () {
            },
            success: function (data, status, jqXhr) {
                $("#copletedlink").addClass("crr");
                $("#unpiadlink").removeClass("crr");
                $("#paidlink").removeClass("crr");
                $("#divContent").html(data);
            }
        });
    },
    removeSelectedTabClass: function () {
        $(".info-nav").find(".current").removeClass("current");

    },
    showUserCollect: function () {
        t2v_CampOrders.removeSelectedTabClass();
        $.ajax({
            url: "/User/UserCollection",
            type: 'post',
            data: {},
            beforeSend: function () {
            },
            success: function (data, status, jqXhr) {
                $("#divUserContent").html(data);
            }
        });
    },

    showUserUserIntegral: function () {
        t2v_CampOrders.removeSelectedTabClass();
        $("#nav3").addClass("current");
        $.ajax({
            url: "/User/UserIntegral",
            type: 'post',
            data: {},
            beforeSend: function () {
            },
            success: function (data, status, jqXhr) {
                $("#divUserContent").html(data);
            }
        });
    },

    showUserHeaderImg: function () {
        t2v_CampOrders.removeSelectedTabClass();
        $.ajax({
            url: "/User/UserHeaderImg",
            type: 'post',
            data: {},
            beforeSend: function () {
            },
            success: function (data, status, jqXhr) {
                $("#divUserContent").html(data);
            }
        });
    },
    showUserInfo: function () {
        t2v_CampOrders.removeSelectedTabClass();
        $("#nav2").addClass("current");
        $.ajax({
            url: "/User/UserInfo",
            type: 'post',
            data: {},
            beforeSend: function () {
            },
            success: function (data, status, jqXhr) {
                $("#divUserContent").html(data);
            }
        });
    }
    
}