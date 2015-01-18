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
        if (!confirm("确认删除营地收藏？"))
        {
            return;
        }
        $http({
            method: 'post',
            url: '/User/DeleteCampCollect',
            data: { CampID: $scope.campcollectobj.campcollect[index].CampID}
        }).success(function (data) {
            if (data == "-1")
            {
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

soNgModule.controller("UserHeaderCtrl", ['$scope', '$routeParams', '$http', '$location', '$upload', function ($scope, $routeParams, $http, $location, $upload) {
    $scope.photoname = '';
    $scope.onFileSelect = function ($files) {
        var allowedMime = ['image/jpg', 'image/jpeg', 'image/gif'];
        var file = $files[0];//audio/mpeg name size type
        var fileValid = false;
        allowedMime.forEach(function (v) {
            if (file.type === v) {
                fileValid = true;
            }
        });
        if (fileValid) {
            $scope.photo = file;
            $scope.photoFormatFlag = true;
            // Only proceed if the selected file is an image
            if (/^image/.test(file.type)) {
                // Create a new instance of the FileReader
                var reader = new FileReader();
                // Read the local file as a DataURL
                reader.readAsDataURL(file);
                // When loaded, set image data as background of div
                reader.onloadend = function () {
                    $scope.photoname = this.result;
                }

            }
        }
        else {
            $scope.photoFormatFlag = false;
            $scope.photo = {};
        }
    };

    $scope.SaveHeaderImg = function () {
        alert("sdf");
        $upload.upload({
            url: '/User/SaveHeadImg', //upload.php script, node.js route, or servlet url
            method: 'POST',
            data: {
            },
            file: $scope.photo,
        }).progress(function (evt) {
            //$("#progressbarforpic").progressbar({
            //    value: Math.min(100, parseInt(100.0 * evt.loaded / evt.total))
            //});
        }).success(function (data, status, headers, config) {
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




