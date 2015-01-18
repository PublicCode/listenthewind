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
    $scope.getAllBooked = function () {
        $http({
            method: 'post',
            url: '/Home/GetAllOrders',
            data: { statusId: 1 }
        }).success(function (data) {
            $scope.booked = data;
            $scope.pagefor = "Unpaid";
        }).error(function (d, s, h, c) {
            alert("error");
            // TODO: Show something like "Username or password invalid."
        });
    };
    $scope.getAllPaid = function (from, to) {
        $http({
            method: 'post',
            url: '/Home/GetAllOrders',
            data: { statusId: 2, from: from, to: to }
        }).success(function (data) {
            $scope.paid = data;
            $scope.items = data;
            $scope.pagefor = "Paid";
            $scope.search();
        }).error(function (d, s, h, c) {
            alert("error");
            // TODO: Show something like "Username or password invalid."
        });
    };
    $scope.getAllCompleted = function (from, to) {
        $http({
            method: 'post',
            url: '/Home/GetAllOrders',
            data: { statusId: 3, from: from, to: to }
        }).success(function (data) {
            $scope.completed = data;
            $scope.pagefor = "Completed";
            $scope.items = data;
            $scope.search();
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
    $scope.searchpassed = function (from, to) {
        if ($scope.pagefor == "Paid")
            $scope.getAllPaid(from, to);
        if ($scope.pagefor == "Completed")
            $scope.getAllCompleted(from, to);
    };
    $scope.askRefund = function (CampReserveID) {
        $http({
            method: 'post',
            url: '/Home/CancelRequest',
            data: { CampReserveID: CampReserveID }
        }).success(function (data) {
            if (data == "\"SessionOut\"") {
                alert("Session失效，请返回");
                window.location.href = "/";
            }
            else {
                $scope.getAllPaid($scope.searchpara.From, $scope.searchpara.To);
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
                alert("Session失效，请返回");
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
    $scope.filteredItems = [];
    $scope.groupedItems = [];
    $scope.itemsPerPage = 4;
    $scope.pagedItems = [];
    $scope.currentPage = 0;

    var searchMatch = function (haystack, needle) {
        if (!needle) {
            return true;
        }
        return haystack.toLowerCase().indexOf(needle.toLowerCase()) !== -1;
    };

    // init the filtered items
    $scope.search = function () {
        $scope.filteredItems = $filter('filter')($scope.items, function (item) {
            for (var attr in item) {
                if (searchMatch(item[attr], $scope.query))
                    return true;
            }
            return false;
        });
        // take care of the sorting order
        if ($scope.sortingOrder !== '') {
            $scope.filteredItems = $filter('orderBy')($scope.filteredItems, $scope.sortingOrder, $scope.reverse);
        }
        $scope.currentPage = 0;
        // now group by pages
        $scope.groupToPages();
    };

    // calculate page in place
    $scope.groupToPages = function () {
        $scope.pagedItems = [];

        for (var i = 0; i < $scope.filteredItems.length; i++) {
            if (i % $scope.itemsPerPage === 0) {
                $scope.pagedItems[Math.floor(i / $scope.itemsPerPage)] = [$scope.filteredItems[i]];
            } else {
                $scope.pagedItems[Math.floor(i / $scope.itemsPerPage)].push($scope.filteredItems[i]);
            }
        }
    };

    $scope.range = function (start, end) {
        var ret = [];
        if (!end) {
            end = start;
            start = 0;
        }
        for (var i = start; i < end; i++) {
            ret.push(i);
        }
        return ret;
    };

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
        }
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pagedItems.length - 1) {
            $scope.currentPage++;
        }
    };

    $scope.setPage = function () {
        $scope.currentPage = this.n;
    };

    // change sorting order
    $scope.sort_by = function (newSortingOrder) {
        if ($scope.sortingOrder == newSortingOrder)
            $scope.reverse = !$scope.reverse;

        $scope.sortingOrder = newSortingOrder;

        // icon setup
        $('th i').each(function () {
            // icon reset
            $(this).removeClass().addClass('icon-sort');
        });
        if ($scope.reverse)
            $('th.' + new_sorting_order + ' i').removeClass().addClass('icon-chevron-up');
        else
            $('th.' + new_sorting_order + ' i').removeClass().addClass('icon-chevron-down');
    };
}]);