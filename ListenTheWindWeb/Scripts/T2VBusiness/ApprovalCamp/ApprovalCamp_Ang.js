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