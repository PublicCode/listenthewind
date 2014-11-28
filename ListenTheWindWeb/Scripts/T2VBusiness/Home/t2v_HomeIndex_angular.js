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
    $scope.obj = JSON.parse(t2v_HomeIndex.lstInfo);
    $scope.cp = $scope.obj.rows;

    $scope.updatePage = function () {
        var strHtml = "<a href='#' onclick='t2v_HomeIndex.GoesToPage(10086)' class='n'>上一页</a>";
        if ($scope.obj.page == 1)
            strHtml = "";
        for (var i = 1; i <= $scope.obj.total; i++)
        {
            strHtml += $scope.obj.page == i ? "<strong>" : "<a href='#' onclick='t2v_HomeIndex.GoesToPage(" + i + ")'>";
            strHtml += "<span class='pc'>" + i + "</span>";
            strHtml += $scope.obj.page == i ? "</strong>" : "</a>";
        }
        if ($scope.obj.page < $scope.obj.total)
            strHtml += "<a href='#' onclick='t2v_HomeIndex.GoesToPage(10087)' class='n'>下一页</span></a>"
        $("#page").html(strHtml);
    };
    $scope.updatePage();
    $scope.GoesToPageAng = function (page) {
        if (page == 10087)
            page = $("#page").find("strong").text() * 1 + 1;
        else if (page == 10086)
            page = $("#page").find("strong").text() * 1 - 1;
        $.ajax({
            url: SiteRoot + "/AjaxCampList",
            type: 'POST',
            data: { locId: 1, dateTime: "2014-01-01", page: page, limit: 12 },
            async: true,
            success: function (data) {
                $scope.obj = data;
                $scope.cp = $scope.obj.rows;
                $scope.$apply();
                $scope.updatePage();
            }
        });
    };
}]); 