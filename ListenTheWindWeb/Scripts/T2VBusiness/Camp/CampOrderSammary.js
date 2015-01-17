soNgModule.controller("CampSammaryCtrl", ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {
    $scope.showcorrectpage = "yes";
    $scope.booked = [];
    $scope.totalAmt = 0;
    $scope.checkall = '';
    $scope.currentreserve = {};
    $scope.getAllBooked = function () {
        $http({
            method: 'post',
            url: '/Home/GetAllOrders',
            data: { statusId: 1 }
        }).success(function (data) {
            $scope.booked = data;
        }).error(function (d, s, h, c) {
            alert("error");
            // TODO: Show something like "Username or password invalid."
        });
    };
    $scope.checkall = function () {
        if ($scope.checkedall === "crr") {
            $scope.checkedall = "";
            angular.forEach($scope.booked, function (v) {
                v.Choosed = ""
                $scope.totalAmt = 0;
            })
        }
        else {
            $scope.checkedall = "crr";
            angular.forEach($scope.booked, function (v) {
                v.Choosed = "crr"
                $scope.totalAmt += v.TotalAmt;
            })
        }
    };
    $scope.checkmyself = function (reserve) {
        if (reserve.Choosed == "crr") {
            reserve.Choosed = ""
            $scope.totalAmt -= reserve.TotalAmt;
        }
        else {
            reserve.Choosed = "crr"
            $scope.totalAmt += reserve.TotalAmt;
        }
    };
    $scope.showDetail = function (reserve) {
        $scope.currentreserve = reserve;
        $("#divOrderDetail").css("left", screen.width / 2 - 60).css("height", "400px").css("width", "800px");
        $('#divOrderDetail').modal('show');
    };
    $scope.getAllBooked();
}]);