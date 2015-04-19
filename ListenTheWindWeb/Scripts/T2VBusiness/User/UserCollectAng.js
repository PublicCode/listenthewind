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
                //$scope.$parent.bdUL.HeadPhoto = $scope.photoname;
                $("#userheaderimg").attr("src", "../../UploadImg/User/" + $scope.photoname);
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

    $scope.currentreserve = [];
    $scope.getCampReserve = function (v) {
        $scope.currentreserve = [];

        $.ajax({
            url: "/Home/GetReserveByReserveID",
            type: 'post',
            data: { campReserveId: v.CampReserveID },
            beforeSend: function () {
            },
            success: function (data, status, jqXhr) {
                if (data == "SessionOut") {
                    alert("session失效，请重新登陆！")
                    window.location.href = "/Home/Index";
                }
                $scope.currentreserve = data;
                $scope.$apply();
                $("#divSalesDetail").css("left", screen.width / 2 - 60).css("height", "500px").css("width", "800px");
                $('#divSalesDetail').modal('show');
            }
        });
    };
}]);

soNgModule.controller("UserInfoCtrl", ['$scope', '$routeParams', '$http', '$location', function ($scope, $routeParams, $http, $location) {

    if (UserCollect.UserAllInfoObj) {
        $scope.UserAllInfoObj = JSON.parse(UserCollect.UserAllInfoObj);
        if ($scope.UserAllInfoObj.usermodel.IDNumberImg2 != null)
        {
            $scope.UserAllInfoObj.usermodel.IDNumberImg2 = "User\\" + $scope.UserAllInfoObj.usermodel.IDNumberImg2;
        }
        if ($scope.UserAllInfoObj.usermodel.IDNumberImg1 != null) {
            $scope.UserAllInfoObj.usermodel.IDNumberImg1 = "User\\" + $scope.UserAllInfoObj.usermodel.IDNumberImg1;
        }
    }

    $scope.LogOutTran = function () {
        if (!$scope.UserAllInfoObj.loginflag) {
            window.location.href = "/Home/Index";
        }
    };
    $scope.LogOutTran();
    //$scope.photoname = '';

    $scope.fileUpload = function () {
        $("#fine-uploader-left1").fineUploader({
            request: {
                endpoint: SiteRoot + '/UserIDNumFileUpload'
            },
            params: {},
            multiple: false
        }).on('validate', function (id, fileName) {
        }).on('complete', function (event, id, fileName, responseJSON) {
            if (fileName != false.toString()) {
                $scope.UserAllInfoObj.usermodel.IDNumberImg1 = responseJSON.fileName;
                $("#fine-uploader-left1").html("");
                $scope.fileUpload();
                $scope.$apply();
            }
            else
                alert("附件上传失败！");
        });
    };
    $scope.fileUpload();

    $scope.fileUpload2 = function () {
        $("#fine-uploader-left2").fineUploader({
            request: {
                endpoint: SiteRoot + '/UserIDNumFileUpload'
            },
            params: {},
            multiple: false
        }).on('validate', function (id, fileName) {
        }).on('complete', function (event, id, fileName, responseJSON) {
            if (fileName != false.toString()) {
                $scope.UserAllInfoObj.usermodel.IDNumberImg2 = responseJSON.fileName;
                $("#fine-uploader-left2").html("");
                $scope.fileUpload2();
                $scope.$apply();
            }
            else
                alert("附件上传失败！");
        });
    };
    $scope.fileUpload2();
    $scope.createUser = function () {
        if ($scope.checkUserStatus()) {
            $http({
                method: 'post',
                url: '/User/CreateUser',
                data: $scope.UserAllInfoObj.usermodel,
            }).success(function (data) {
                if (data.Code == "Success") {
                    alert("保存成功！");
                    $scope.UserAllInfoObj.usermodel.UserID = data;
                    $scope.$apply();
                }
                else {
                    alert(data.Message);
                }
            }).error(function (d, s, h, c) {
                alert("error");
            });
        }
    };
    $scope.saveUserBasicInfo = function () {
        
        if ($scope.checkUserStatus()) {
            $http({
                method: 'post',
                url: '/User/saveUserBasicInfo',
                data: $scope.UserAllInfoObj.usermodel,
            }).success(function (data) {
                if (data == "True") {
                    alert("保存成功！");
                    $scope.$apply();
                }
            }).error(function (d, s, h, c) {
                alert("error");
            });
        }
    };

    $scope.saveUserAuthInfo = function () {

        if ($scope.checkUserStatus()) {
            $http({
                method: 'post',
                url: '/User/saveUserAuthInfo',
                data: $scope.UserAllInfoObj.usermodel,
            }).success(function (data) {
                if (data == "True") {
                    alert("保存成功！");
                    $scope.$apply();
                }
            }).error(function (d, s, h, c) {
                alert("error");
            });
        }
    };

    $scope.saveIDNumberInfo = function () {

        if ($scope.checkUserStatus()) {
            $http({
                method: 'post',
                url: '/User/saveIDNumberInfo',
                data: $scope.UserAllInfoObj.usermodel,
            }).success(function (data) {
                if (data == "True") {
                    alert("保存成功！");
                    $scope.$apply();
                }
            }).error(function (d, s, h, c) {
                alert("error");
            });
        }
    };
    
}]);
soNgModule.controller("UserManagementCtrl", ['$scope', '$routeParams', '$http', '$location', function ($scope, $routeParams, $http, $location) {
    $scope.userList = [];
    //$scope.photoname = '';
    $scope.currentUser = {};
    $scope.currentPage = 1;
    $scope.itemsPerPage = 4;
    $scope.totalPages = 0;
    $scope.totalRecord = 0;
    $scope.getAllUsers = function () {
        $http({
            method: 'post',
            url: '/User/GetAllUserList',
            data: { page: $scope.currentPage, limit: $scope.itemsPerPage }
        }).success(function (data) {
            $scope.userList = data.listOfUsers.rows;
            $scope.currentPage = data.listOfUsers.page;
            $scope.totalPages = data.listOfUsers.total;
            $scope.totalRecord = data.listOfUsers.records;
        }).error(function (d, s, h, c) {
            alert("error");
        });
    };
    $scope.getAllUsers();
    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
        }
        $scope.getAllUsers();
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.totalPages) {
            $scope.currentPage++;
        }
        $scope.getAllUsers();
    };

    $scope.validateUser = function ($index) {
        $scope.currentUser = $scope.userList[$index];
        $("#divValidateUser").css("left", screen.width / 2 - 60).css("height", "400px").css("width", "500px");
        $('#divValidateUser').modal('show');
    };
    $scope.deleteUser = function ($index) {
        if (!confirm("确认删除用户？")) {
            return;
        }
        $scope.currentUser = $scope.userList[$index];
        $http({
            method: 'post',
            url: '/User/DeleteUser',
            data: { userId: $scope.currentUser.UserID }
        }).success(function (data) {
            if (data.Code == 'Sucess') {
                $scope.userList.splice($scope.currentUser);
            }
            alert(data.Message);
        }).error(function (d, s, h, c) {
            alert("error");
        });
    };
    $scope.passValidate = function (userId) {
        $http({
            method: 'post',
            url: '/User/PassValidate',
            data: { userId: userId }
        }).success(function (data) {
            if (data == 'true') {
                alert("验证成功");
            }
        }).error(function (d, s, h, c) {
            alert("error");
        });
    };
}]);

soNgModule.controller("AssignManagerForCampCtrl", ['$scope', '$routeParams', '$http', '$location', function ($scope, $routeParams, $http, $location) {
    $scope.campList = [];
    $scope.myManager = '';
    $scope.currentCamp = {};
    $scope.currentPage = 1;
    $scope.itemsPerPage = 4;
    $scope.totalPages = 0;
    $scope.totalRecord = 0;
    $scope.getAllCamps = function () {
        $http({
            method: 'post',
            url: '/User/GetAllCampList',
            data: { page: $scope.currentPage, limit: $scope.itemsPerPage }
        }).success(function (data) {
            $scope.campList = data.listOfCamps.rows;
            $scope.currentPage = data.listOfCamps.page;
            $scope.totalPages = data.listOfCamps.total;
            $scope.totalRecord = data.listOfCamps.records;
        }).error(function (d, s, h, c) {
            alert("error");
        });
    };
    $scope.getAllManager = function () {
        $http({
            method: 'post',
            url: '/User/GetAllManagerList'
        }).success(function (data) {
            $scope.managerList = data.managerList;
        }).error(function (d, s, h, c) {
            alert("error");
        });
    };
    $scope.getAllCamps();
    $scope.getAllManager();
    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
        }
        $scope.getAllCamps();
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.totalPages) {
            $scope.currentPage++;
        }
        $scope.getAllCamps();
    };

    $scope.assignManagerForCamp = function ($index) {
        $scope.currentCamp = $scope.campList[$index];
        $("#divCampAssign").css("left", screen.width / 2 - 60).css("height", "400px").css("width", "500px");
        $('#divCampAssign').modal('show');
    };
    $scope.chooseThisManager = function () {
        if (!confirm("确认选择此用户做为营地管理员？")) {
            return;
        }
        else {
            if ($scope.checkUserStatus()) {
                $scope.currentCamp.ManagedByID = $scope.myManager.UserID;
                $scope.currentCamp.ManagedByName = $scope.myManager.UserName;
                $http({
                    method: 'post',
                    url: '/User/ChooseManager',
                    data: $scope.currentCamp,
                }).success(function (data) {
                    if (data == "True") {
                        alert("保存成功！");
                        $scope.$apply();
                    }
                }).error(function (d, s, h, c) {
                    alert("error");
                });
            }
        }
    };
}]);



