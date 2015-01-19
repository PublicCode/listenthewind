soNgModule.controller("UserCollectionCtrl", ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {

    if (UserCollect.campcollectobj) {
        $scope.campcollectobj = JSON.parse(UserCollect.campcollectobj);
    }

    $scope.LogOutTran = function () {
        if (!$scope.campcollectobj.loginflag) {
            window.location.href = "/Home/Index";
        }
    };
    $scope.LogOutTran();

    $scope.predicate = '+CampItemSort';

    $scope.DeleteCampCollect = function (index) {
        if (!confirm("确认删除营地收藏？")) {
            return;
        }
        $http({
            method: 'post',
            url: '/User/DeleteCampCollect',
            data: { CampID: $scope.campcollectobj.campcollect[index].CampID }
        }).success(function (data) {
            if (data == "-1") {
                alert("session失效，请重新登陆！")
                window.location.href = "/Home/Index";
            }
            else if (data == "1") {
                alert("删除成功！")
                $scope.campcollectobj.campcollect.splice(index, 1);
            }
            else {
                alert(data);
            }
        }).error(function (d, s, h, c) {
            alert("error");
            // TODO: Show something like "Username or password invalid."
        });
    };
}]);

soNgModule.controller("UserHeaderCtrl", ['$scope', '$routeParams', '$http', '$location', function ($scope, $routeParams, $http, $location) {
    $scope.photoname = '';

    $scope.fileUpload = function () {
        $("#fine-uploader-left").fineUploader({
            request: {
                endpoint: SiteRoot + '/UserFileUpload'
            },
            params: {},
            multiple: false
        }).on('validate', function (id, fileName) {
        }).on('complete', function (event, id, fileName, responseJSON) {
            if (fileName != false.toString()) {
                $scope.photoname = responseJSON.fileName;
                $("#fine-uploader-left").html("");
                $scope.fileUpload();
                $scope.$apply();
            }
            else
                alert("附件上传失败！");
        });
    };
    $scope.fileUpload();

    $scope.SaveUserPhoto = function () {
        if ($scope.photoname == "")
            return;
        $http({
            method: 'post',
            url: SiteRoot + '/SaveUserPhotos',
            data: { tmpFileName: $scope.photoname }
        }).success(function (data) {
            if (data == "True") {
                alert("保存成功！");
                $scope.photoname = ""
            }
        }).error(function (d, s, h, c) {
            alert("error");
        });
    };
    //if (UserCollect.campcollectobj) {
    //    $scope.campcollectobj = JSON.parse(UserCollect.campcollectobj);
    //}

    //$scope.LogOutTran = function () {
    //    if (!$scope.campcollectobj.loginflag) {
    //        window.location.href = "/Home/Index";
    //    }
    //};
    //$scope.LogOutTran();

    //$scope.predicate = '+CampItemSort';

    //$scope.DeleteCampCollect = function (index) {
    //    if (!confirm("确认删除营地收藏？")) {
    //        return;
    //    }
    //    $http({
    //        method: 'post',
    //        url: '/User/DeleteCampCollect',
    //        data: { CampID: $scope.campcollectobj.campcollect[index].CampID }
    //    }).success(function (data) {
    //        if (data == "-1") {
    //            alert("session失效，请重新登陆！")
    //            window.location.href = "/Home/Index";
    //        }
    //        else if (data == "1") {
    //            alert("删除成功！")
    //            $scope.campcollectobj.campcollect.splice(index, 1);
    //        }
    //        else {
    //            alert(data);
    //        }
    //    }).error(function (d, s, h, c) {
    //        alert("error");
    //        // TODO: Show something like "Username or password invalid."
    //    });
    //};
}]);

soNgModule.controller("UserIntegralCtrl", ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {

    if (UserCollect.IntegralList) {
        $scope.IntegralList = JSON.parse(UserCollect.IntegralList);
    }

    $scope.LogOutTran = function () {
        if (!$scope.IntegralList.loginflag) {
            window.location.href = "/Home/Index";
        }
    };

    $scope.UsedIntegralSum = UserCollect.UsedIntegralSum;
    $scope.LogOutTran();

    $scope.prePage = function () {
        if ($scope.IntegralList.page == 1) {
            return;
        }
        var page = $scope.IntegralList.page - 1;
        $scope.getPageRecords(page);
    };

    $scope.nextPage = function () {
        if ($scope.IntegralList.page == $scope.IntegralList.total) {
            return;
        }
        var page = $scope.IntegralList.page + 1;
        $scope.getPageRecords(page);
    };

    $scope.getPageRecords = function (page) {
        $http({
            method: 'post',
            url: '/User/GetIntegralList',
            data: { page: page, limit: 20 }
        }).success(function (data) {
            $scope.IntegralList = data;
            $scope.LogOutTran();
        }).error(function (d, s, h, c) {
            alert("error");
            // TODO: Show something like "Username or password invalid."
        });
    };

}]);




