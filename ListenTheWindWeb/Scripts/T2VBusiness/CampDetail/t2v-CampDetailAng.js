soNgModule.controller("RMADetailCtrl", ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {

    if (t2v_CampDetail.CampInfo) {
        $scope.camp = JSON.parse(t2v_CampDetail.CampInfo);
    }

}]);