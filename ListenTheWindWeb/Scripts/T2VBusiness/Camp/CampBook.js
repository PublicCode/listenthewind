soNgModule.controller("CampBookCtrl", ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {
    $scope.selectedDate = "";
    $scope.selectedDateCount = 0;
    if (t2v_CampDetail.CampInfo) {
        $scope.camp = JSON.parse(t2v_CampDetail.CampInfo);
    }
    //$http.post(SiteRoot + "/../Account/getClientInfo", { id: 1 }).success(function (data, status, headers, config) {
    //    //re order the config
    //    $scope.userInfo = data;
    //}).error(function (data, status, headers, config) {
    //    alert('error');
    //});
    //Get reserved date for Pile
    $http.post(SiteRoot + "/getReservedDateForPile", { PileId: t2v_CampDetail.PileId }).success(function (data, status, headers, config) {
        //re order the config
        $("#calendarpick").kalendae({
            direction: 'future',
            months: 3,
            mode: 'multiple',
            blackout: data,
            selected: t2v_CampDetail.DefaultDate,
            subscribe: {
                'change': function (date, action) {
                    $scope.selectedDate = this.getSelected();
                    $scope.selectedDateCount = $scope.selectedDate == "" ? 0 : $scope.selectedDate.split(',').length;
                    $scope.getTotalCampPrice();
                    $scope.$apply();
                }
            },
        });
    }).error(function (data, status, headers, config) {
        alert('error');
    });
    
    $scope.basicdata = JSON.parse(t2v_CampDetail.BasicData);

    $scope.totalCampPrice = 0;
    $scope.getTotalCampPrice = function () {
        $scope.totalCampPrice = 0;
        angular.forEach($scope.camp.ModelListcampprice, function (v) {
            if (v.Qty > 0)
                $scope.totalCampPrice += v.ItemPrice * v.Qty;
        });
        $scope.$apply();
    };

    $scope.bookcamp = function (flag) {
        if (!$scope.checkUserStatus)
            return false;
        var picked = $scope.selectedDate;
        var selectedItem = [];
        angular.forEach($scope.camp.ModelListcampprice, function(v){
            if (v.Qty > 0)
                selectedItem.push({ CampPriceID: v.CampPriceID, Qty: v.Qty });
        });
        if (picked == "")
        {
            alert("请选择日期");
            return false;
        }
        //t2v_util.t2vconfirm.showConfirm("确定预定选择的项目?", function () {
        $http.post(SiteRoot + "/SaveReserve", { SelectedDate: picked.split(','), Camp: $scope.camp, PileID: t2v_CampDetail.PileId }).success(function (data, status, headers, config) {
            alert(data);
            if (flag == "1") {
                window.location.href = "/Home/CampDetail?CampID=" + $scope.camp.CampID;
            }
            else {

            }
            }).error(function (data, status, headers, config) {
                alert('error');
            });
        //}, function () { }, "HDS");
    };
}]);
