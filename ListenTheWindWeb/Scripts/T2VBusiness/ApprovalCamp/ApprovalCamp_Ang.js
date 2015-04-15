soNgModule.controller("ApprovalCampList", ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {
    if (ApprovalCamp.lstInfo) {
        $scope.apCamp = ApprovalCamp.lstInfo;
    }
    $scope.lastPage = function () {
        if ($scope.apCamp.page == 1)
            return false;
        else {
            $scope.apCamp.page -= 1;
            $scope.GetApprovalCampInfo();
        }
    };
    $scope.nextPage = function () {
        if ($scope.apCamp.page >= $scope.apCamp.total)
            return false;
        else {
            $scope.apCamp.page += 1;
            $scope.GetApprovalCampInfo();
        }
    };
    $scope.GetApprovalCampInfo = function () {
        $http({
            method: 'post',
            url: '/CampApproval/GetApprovalCampLstModel',
            data: { page: $scope.apCamp.page, limit: 12 }
        }).success(function (data) {
            $scope.apCamp = data.data;
            $scope.$apply();
        }).error(function (d, s, h, c) {
            alert("error");
        });
    };
}]);

soNgModule.controller("ApprovalCampDetailCtrl", ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {
    if (ApprovalCamp.CampInfo) {
        $scope.camp = JSON.parse(ApprovalCamp.CampInfo);
    }
    $scope.InitMap = function () {
        //map.addControl(new BMap.MapTypeControl());   //添加地图类型控件
        var map = new BMap.Map("allmap");
        var point = new BMap.Point($scope.camp.Latitude, $scope.camp.Longitude);
        map.centerAndZoom(point, 12);
        var marker = new BMap.Marker(point);  // 创建标注
        map.addOverlay(marker);
        map.enableScrollWheelZoom(true);
    }
    $scope.InitMap();
    //Not sure why check user stasus function didn't finish
    $scope.ShowPileScreen = function () {
        if ($scope.checkUserStatus()) {
            $("#divSelPile").css("left", screen.width / 2 - 60).css("height", "400px").css("width", "800px");
            $('#divSelPile').modal('show');
        }
    };

    $scope.ShowMoreCommenScreen = function () {
        $("#divMoreCommenScreen").css("left", screen.width / 2 - 60).css("height", "400px").css("width", "800px");
        $('#divMoreCommenScreen').modal('show');
    };
}]);