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
            url: "/Home/UnpiadView",
            type: 'post',
            data: {},
            beforeSend: function () {
            },
            success: function (data, status, jqXhr) {
                $("#divContent").html(data);
            }
        });
    },
    showUserCollect: function () {
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
}