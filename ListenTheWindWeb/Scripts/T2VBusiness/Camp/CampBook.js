soNgModule.controller("CampBookCtrl", ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {
    $scope.selectedDate = "";
    if (t2v_CampDetail.CampInfo) {
        $scope.camp = JSON.parse(t2v_CampDetail.CampInfo);
    }
    $http.post(SiteRoot + "/../Account/getClientInfo", { id: 1 }).success(function (data, status, headers, config) {
        //re order the config
        $scope.userInfo = data;
    }).error(function (data, status, headers, config) {
        alert('error');
    });
    //Get reserved date for Pile
    $http.post(SiteRoot + "/getReservedDateForPile", { PileId: t2v_CampDetail.PileId }).success(function (data, status, headers, config) {
        //re order the config
        $("#calendarpick").kalendae({
            direction: 'future',
            months: 3,
            mode: 'multiple',
            blackout: data,
            selected: function () { alert(this.getSelected()) },
            subscribe: {
                'change': function (date, action) {
                    $scope.selectedDate = this.getSelected();
                }
            },
        });
    }).error(function (data, status, headers, config) {
        alert('error');
    });
    
    
    $scope.bookcamp = function () {
        var picked = $scope.selectedDate;
        var selectedItem = [];
        angular.forEach($scope.camp.ModelListcampprice, function(v){
            if(v.Checked)
                selectedItem.push(v.CampPriceID);
        });
        if (picked == "")
        {
            alert("请选择日期");
            return false;
        }
        //t2v_util.t2vconfirm.showConfirm("确定预定选择的项目?", function () {
        $http.post(SiteRoot + "/SaveReserve", { SelectedDate: picked.split(','), SelectedItemId: selectedItem, CampID: $scope.camp.CampID, PileID: t2v_CampDetail.PileId }).success(function (data, status, headers, config) {
            alert(data);
            }).error(function (data, status, headers, config) {
                alert('error');
            });
        //}, function () { }, "HDS");
    };
}]);