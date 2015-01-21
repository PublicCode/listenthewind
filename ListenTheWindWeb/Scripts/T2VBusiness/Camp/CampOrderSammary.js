soNgModule.controller("CampSammaryCtrl", ['$scope', '$routeParams', '$http', '$filter', function ($scope, $routeParams, $http, $filter) {
    $scope.pagefor = "";
    $scope.booked = [];
    $scope.paid = [];
    $scope.completed = [];
    $scope.totalAmt = 0;
    $scope.checkall = '';
    $scope.currentreserve = {};
    $scope.searchpara = {};
    $scope.items = [];
    if ($scope.bdUL.HeadPhoto == null || $scope.bdUL.HeadPhoto == '') {
        //$scope.bdUL.HeadPhoto = 'avt1.jpg';
        $("#userheaderimg").attr("src", "../../UploadImg/User/avt1.jpg");
    }
    else {
        $("#userheaderimg").attr("src", "../../UploadImg/User/" + $scope.bdUL.HeadPhoto);
    }
    $("#IDNumberTrue").removeClass("crr");
    $("#IDNumberFlase").removeClass("crr");
    if ($scope.bdUL.IDNumberFlag == 1) {
        $("#IDNumberTrue").addClass("crr");
    }
    else {
        $("#IDNumberFlase").addClass("crr");
    }
    $scope.getAllBooked = function () {
        $http({
            method: 'post',
            url: '/Home/GetAllOrders',
            data: { statusId: 1, page:1, limit:1000000 }
        }).success(function (data) {
            if (data == "\"SessionOut\"") {
                alert("Session失效，将返回首页");
                window.location.href = "/";
            }
            else {
                $scope.booked = data.rows;
                $scope.pagefor = "Unpaid";
            }
        }).error(function (d, s, h, c) {
            alert("error");
            // TODO: Show something like "Username or password invalid."
        });
    };
    $scope.getAllPaid = function (from, to) {
        $http({
            method: 'post',
            url: '/Home/GetAllOrders',
            data: { statusId: 2, from: from, to: to, page: $scope.currentPage, limit: $scope.itemsPerPage }
        }).success(function (data) {
            if (data == "\"SessionOut\"") {
                alert("Session失效，将返回首页");
                window.location.href = "/";
            }
            else {
                $scope.paid = data.rows;
                $scope.pagefor = "Paid";
                $scope.currentPage = data.page;
                $scope.totalPages = data.total;
                $scope.totalRecord = data.records;
            }
        }).error(function (d, s, h, c) {
            alert("error");
            // TODO: Show something like "Username or password invalid."
        });
    };
    $scope.getAllCompleted = function (from, to) {
        $http({
            method: 'post',
            url: '/Home/GetAllOrders',
            data: { statusId: 3, from: from, to: to, page: $scope.currentPage, limit: $scope.itemsPerPage }
        }).success(function (data) {
            if (data == "\"SessionOut\"") {
                alert("Session失效，将返回首页");
                window.location.href = "/";
            }
            else {
                $scope.completed = data.rows;
                $scope.pagefor = "Completed";
                $scope.currentPage = data.page;
                $scope.totalPages = data.total;
                $scope.totalRecord = data.records;
            }
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
        $("#divOrderDetail").css("left", screen.width / 2 - 60).css("height", "500px").css("width", "800px");
        $('#divOrderDetail').modal('show');
    };
    $scope.searchpassed = function (from, to) {
        if ($scope.pagefor == "Paid")
            $scope.getAllPaid(from, to);
        if ($scope.pagefor == "Completed")
            $scope.getAllCompleted(from, to);
    };
    $scope.askRefund = function (CampReserveID,index) {
        //$('#send_suc').modal('show');
        $http({
            method: 'post',
            url: '/Home/CancelRequest',
            data: { CampReserveID: CampReserveID }
        }).success(function (data) {
            if (data == "\"SessionOut\"") {
                alert("Session失效，将返回首页");
                window.location.href = "/";
            }
            else {
                //$scope.getAllPaid($scope.searchpara.From, $scope.searchpara.To);
                $scope.paid.splice(index, 1);
                $('#send_suc').modal('show');
            }
        }).error(function (d, s, h, c) {
            alert("error");
            // TODO: Show something like "Username or password invalid."
        });
    };
    $scope.deleteReserve = function (CampReserveID) {
        $http({
            method: 'post',
            url: '/Home/CancelOrder',
            data: { CampReserveID: CampReserveID }
        }).success(function (data) {
            if (data == "\"SessionOut\"") {
                alert("Session失效，将返回首页");
                window.location.href = "/";
            }
            else {
                alert("取消成功！");
                $scope.getAllBooked();
            }
        }).error(function (d, s, h, c) {
            alert("error");
            // TODO: Show something like "Username or password invalid."
        });
    };
    /***************************
    Paging funcitons
    *****************************************/
   // $scope.sortingOrder = sortingOrder;
    $scope.reverse = false;
    $scope.itemsPerPage = 4;
    $scope.pagedItems = [];
    $scope.currentPage = 1;
    $scope.totalPages = 0;
    $scope.totalRecord = 0;
    

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
        }
        if ($scope.pagefor == "Paid")
            $scope.getAllPaid($scope.searchpara.From, $scope.searchpara.To);
        else if ($scope.pagefor == "Completed")
            $scope.getAllCompleted($scope.searchpara.From, $scope.searchpara.To);

    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.totalPages) {
            $scope.currentPage++;
        }
        if ($scope.pagefor == "Paid")
            $scope.getAllPaid($scope.searchpara.From, $scope.searchpara.To);
        else if ($scope.pagefor == "Completed")
            $scope.getAllCompleted($scope.searchpara.From, $scope.searchpara.To);
    };
}]);