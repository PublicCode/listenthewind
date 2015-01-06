soNgModule.controller("BodyCtrl", ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {
    $scope.initUL = { UserID: 0, UserName: "", errUserName: false, Pwd: "", errPwd: false, Mobile: "" };
    $scope.bdUL = JSON.parse(t2v_HomeIndex.ulInfo);
    $scope.bdShowLayout = function (id) {
        $("#bd_userRegister").css("display", "none");
        $("#tm").css("display", "");
        $("#" + id).css("display", "");
    };
    $scope.close_userLogin = function () {
        $("#tm").css("display", "none");
        $("#bd_userLogin").css("display", "none");
        if ($scope.bdUL.UserID == 0)
        {
            $scope.bdUL = angular.copy($scope.initUL);
        }
    };
    $scope.bgActionLogin = function () {
        $.ajax({
            url: SiteRoot + "/Account/UserLogOn",
            type: 'POST',
            data: { strJson: JSON.stringify($scope.bdUL) },
            async: true,
            success: function (data) {
                $scope.bdUL = JSON.parse(data);
                if ($scope.bdUL.UserID > 0) {
                    $scope.close_userLogin();
                }
                $scope.$apply();
            }
        });
    };
    $scope.bdLogOff = function () {
        $.ajax({
            url: SiteRoot + "/Account/UserLogOff",
            type: 'POST',
            data: { },
            async: true,
            success: function (data) {
                if (data == "True") {
                    $scope.bdUL = angular.copy($scope.initUL);
                    $scope.$apply();
                }
                else
                    alert(data);
            }
        });
    };

    $scope.bdULReg = { UserName: '', errUserName: false, Pwd: '', RePwd: '', Email:'', };
}]);

soNgModule.controller("HomeIndexCtrl", ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {
    $scope.hi = JSON.parse(t2v_HomeIndex.hiInfo);
    $scope.ba = JSON.parse(t2v_HomeIndex.baInfo).filter(function (e) { return e.DataType == "camplod"; });
    $scope.myLoc = [];
    $scope.SearchCamp = function () {
        var locId = $scope.myLoc.LocationID;

        var date = $("#datepicker1").val();

        if (locId == undefined) {
            locId = 0;
        }
        var lod = $scope.myLOD == null ? "" : $scope.myLOD.DataName;

        $("#param").val(locId + "/" + date + "/" + lod);
    };
}]);

soNgModule.controller("CampListCtrl", ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {
    $scope.obj = JSON.parse(t2v_HomeIndex.lstInfo);
    $scope.cp = $scope.obj.rows;
    $scope.basicdata = JSON.parse(t2v_HomeIndex.bdInfo);

    $scope.hi = JSON.parse(t2v_HomeIndex.hiInfo);
    $scope.searchInfo = $scope.obj.searchInfo;
    $scope.refreshLocCity = function () {
        angular.forEach($scope.hi, function (v, k) {
            angular.forEach(v.Locations, function (s, p) {
                if (s.LocationID == $scope.searchInfo.LocationID) {
                    $scope.myLoc = s;
                    $scope.myCity = v;
                }
            });
        });
    };
    $scope.refreshLocCity();

    $scope.bindLOD = function () {
        angular.forEach($scope.basicdata, function (v, k) {
            if (v.DataName == $scope.obj.searchInfo.CampLOD)
            {
                $scope.myLOD = v;
            }
        });
    };
    $scope.bindLOD();

    $scope.predicate = '+CampItemSort';

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
            data: { searchInfo: JSON.stringify($scope.obj.searchInfo), page: page, limit: 12 },
            async: true,
            success: function (data) {
                $scope.obj = data;
                $scope.cp = $scope.obj.rows;
                $scope.$apply();
                $scope.updatePage();
            }
        });
    };
    $scope.myLoc = [];
    $scope.Search = function () {
        var locId = $scope.myLoc.LocationID;
        var date = $("#datepicker1").val();
        if (locId == undefined) {
            locId = 0;
        }

        $scope.obj.searchInfo.LocationID = locId;
        $scope.obj.searchInfo.JoinCampDate = date;

        var specialContents = [];
        $(".campitem").each(function (v, k) {
            if ($(k).prop("checked"))
                specialContents.push($(k).attr("selValue"));
        });
        $scope.obj.searchInfo.SpecialContents = specialContents;

        var campTypes = [];
        $(".camptype").each(function (v, k) {
            if ($(k).prop("checked"))
                campTypes.push($(k).attr("selValue"));
        });
        $scope.obj.searchInfo.CampType = campTypes;

        var hostLangs = [];
        $(".hostlang").each(function (v, k) {
            if ($(k).prop("checked"))
                hostLangs.push($(k).attr("selValue"));
        });
        $scope.obj.searchInfo.HostLang = hostLangs;

        $scope.obj.searchInfo.LocationID = $scope.myLoc == undefined ? 0 : $scope.myLoc.LocationID;

        $scope.obj.searchInfo.PriceStart = $("#amount").val().split('-')[0].replace("￥", "") * 1;
        $scope.obj.searchInfo.PriceEnd = $("#amount").val().split('-')[1].replace("￥", "") * 1;
        $scope.obj.searchInfo.CampLOD = $scope.myLOD == null ? "" : $scope.myLOD.DataName;
        $.ajax({
            url: SiteRoot + "/AjaxCampList",
            type: 'POST',
            data: { searchInfo: JSON.stringify($scope.obj.searchInfo), page: 1, limit: 12 },
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