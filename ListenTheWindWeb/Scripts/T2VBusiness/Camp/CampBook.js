soNgModule.controller("CampBook", ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {

    if (t2v_CampDetail.CampInfo) {
        $scope.camp = JSON.parse(t2v_CampDetail.CampInfo);
    }
    $http.post("/Home/GetClient")
                    .success(function (d, s, h, c) {
                        $scope.client = d;
                    })
                    .error(function (d, s, h, c) {
                        t2v_angular.alertDebugError(d, s, h, c);
                    });

    $scope.bookcamp = function () {
        debugger;
        var picked = $("#calendarpick").val();
        alert(picked);
    };
}]);