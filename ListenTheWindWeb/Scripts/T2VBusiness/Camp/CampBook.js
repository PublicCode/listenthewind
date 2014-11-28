soNgModule.controller("CampBookCtrl", ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {

    if (t2v_CampDetail.CampInfo) {
        $scope.camp = JSON.parse(t2v_CampDetail.CampInfo);
    }
    $scope.selectedDate = "";
    $("#calendarpick").kalendae({
        direction: 'future',
        months: 3,
        mode: 'multiple',
        blackout: ["11/27/2014", "12/2/2014"],
        selected: function () { alert(this.getSelected()) },
        subscribe: {
            'change': function (date, action) {
                    $scope.selectedDate = this.getSelected();
            }
        },
    });
    $scope.bookcamp = function () {
        var picked = $scope.selectedDate;
        alert(picked);
    };
}]);