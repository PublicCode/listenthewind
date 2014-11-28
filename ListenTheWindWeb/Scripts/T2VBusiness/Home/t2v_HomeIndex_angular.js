soNgModule.controller("HomeIndexCtrl", ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {
    $scope.hi = JSON.parse(t2v_HomeIndex.hiInfo);
    $scope.myCity  = $scope.hi[0];
    $scope.locLst = $scope.myCity.Locations;
    $scope.myLoc = $scope.locLst[0];
    $scope.ChangeCity = function () {
        $scope.locLst = $scope.myCity.Locations;
        $scope.myLoc = $scope.locLst[0];
    };
    $scope.SearchCamp = function () {
        var locId = $scope.myLoc.LocationID;
        var date = $("#datepicker1").val();
        if (locId == undefined) {
            alert("请选择景点");
            return false;
        }
        else if (date == "") {
            alert("请选择入营时间");
            return false;
        }
        $("#param").val(locId + "/" + date);
        return true;
    };
}]);

soNgModule.controller("CampListCtrl", ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {
    $scope.cp = JSON.parse(t2v_HomeIndex.lstInfo);
    
}]); 