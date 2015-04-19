﻿soNgModule.controller("ApprovalCampList", ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {
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

soNgModule.controller("ApprovalCampDetailCtrl", ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {
    if (ApprovalCamp.CampInfo) {
        $scope.camp = JSON.parse(ApprovalCamp.CampInfo);
    };
    $scope.InitMap = function () {
        //map.addControl(new BMap.MapTypeControl());   //添加地图类型控件
        var map = new BMap.Map("allmap");
        var point = new BMap.Point($scope.camp.Latitude, $scope.camp.Longitude);
        map.centerAndZoom(point, 12);
        var marker = new BMap.Marker(point);  // 创建标注
        map.addOverlay(marker);
        map.enableScrollWheelZoom(true);
    };
    $scope.InitMap();
    $scope.ShowReject = function () {
        $("#divReject").css("left", screen.width / 2 - 60).css("height", "400px").css("width", "800px");
        $('#divReject').modal('show');
    };
    $scope.RejectCamp = function (type) {
        if ($.trim($scope.camp.RejectReason) == "") {
            alert("请输入拒绝原因！");
            return false;
        }
        $http({
            method: 'post',
            url: '/CampApproval/RejectCamp',
            data: { info: $scope.camp }
        }).success(function (data) {
            window.location.href = "/CampApproval/ApprovalDetail?CampID=" + data.campID + "&dt=";
        }).error(function (d, s, h, c) {
            alert("error");
        });
    };
    $scope.RejectHide = function () {
        $scope.camp.RejectReason = "";
        $('#divReject').modal('hide');
    };
    $scope.ApprovalCamp = function (type) {
        $http({
            method: 'post',
            url: '/CampApproval/ApprovalCamp',
            data: { info: $scope.camp, ops: type }
        }).success(function (data) {
            window.location.href = "/CampApproval/ApprovalDetail?CampID=" + data.campID + "&dt=";
        }).error(function (d, s, h, c) {
            alert("error");
        });
    };
    $scope.GoToEdit = function () {
        window.location.href = "/CampApproval/ApprovalEdit?CampID=" + $scope.camp.CampID + "&dt=";
    };
    $scope.ShowPileScreen = function () {
        if ($scope.checkUserStatus()) {
            $("#divSelPile").css("left", screen.width / 2 - 60).css("height", "400px").css("width", "800px");
            $('#divSelPile').modal('show');
        }
    };

    $scope.ShowMoreCommenScreen = function () {
        $("#divMoreCommenScreen").css("left", screen.width / 2 - 60).css("height", "400px").css("width", "800px");
        $('#divMoreCommenScreen').modal('show');
    };
}]);

soNgModule.controller("ApprovalCampEditCtrl", ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {
    if (ApprovalCamp.CampInfo) {
        $scope.camp = JSON.parse(ApprovalCamp.CampInfo);
    };
    $scope.SaveApprovalCamp = function (type) {
        if ($scope.camp.ModelListcampphoto.length > 0) {
            $http({
                method: 'post',
                url: '/CampApproval/SaveApprovalCamp',
                data: { info: $scope.camp, ops: type }
            }).success(function (data) {
                window.location.href = "/CampApproval/ApprovalDetail?CampID=" + data.campID + "&dt=";
            }).error(function (d, s, h, c) {
                alert("error");
            });
        }
        else {
            alert("请上传营地照片.");
        }
    };
    $scope.CancelApprovalCamp = function () {
        if ($scope.camp.CampID == 0) {
            window.location.href = "/CampApproval/Index";
        }
        else {
            window.location.href = "/CampApproval/ApprovalDetail?CampID=" + $scope.camp.CampID + "&dt=";
        }
    };
    $scope.fileUpload = function () {
        $("#fine-uploader-left").fineUploader({
            request: {
                endpoint: SiteRoot + '/CampFileUpload'
            },
            params: {},
            multiple: false
        }).on('validate', function (id, fileName) {
        }).on('complete', function (event, id, fileName, responseJSON) {
            if (responseJSON.success) {
                $scope.camp.ModelListcampphoto.push({ CampPhotoID: 0, CampID: $scope.camp.CampID, CampPhoteFile: responseJSON.fileName });
                $("#fine-uploader-left").html("");
                $scope.fileUpload();
                responseJSON.success = false;
                $scope.$apply();
            }
            else if (responseJSON.fileName == undefined)
                alert("附件上传失败！");
        });
    };
    $scope.fileUpload();
    $scope.fileUpload1 = function () {
        $("#fine-uploader-left1").fineUploader({
            request: {
                endpoint: SiteRoot + '/CampFileUpload1'
            },
            params: {},
            multiple: false
        }).on('validate', function (id, fileName) {
        }).on('complete', function (event, id, fileName, responseJSON) {
            if (responseJSON.success) {
                $scope.camp.CampPic = responseJSON.fileName;
                $("#fine-uploader-left").html("");
                $scope.fileUpload1();
                responseJSON.success = false;
                $scope.$apply();
            }
            else if (responseJSON.fileName == undefined)
                alert("附件上传失败！");
        });
    };
    $scope.fileUpload1();
    $scope.RemoveCampPhoto = function (index) {
        $scope.camp.ModelListcampphoto.splice(index, 1);
    };
    $scope.ShowPileScreen = function () {
        if ($scope.checkUserStatus()) {
            $("#divSelPile").css("left", screen.width / 2 - 60).css("height", "400px").css("width", "800px");
            $('#divSelPile').modal('show');
        }
    };
    $scope.RemoveCampPile = function (index) {
        $scope.camp.ModelListcamppile.splice(index, 1);
    };
    $scope.AddCampPile = function () {
        $scope.camp.ModelListcamppile.push({ PileID: 0, CampID: $scope.camp.CampID, PileNumber: $scope.PileNumber, Active: 0 });
        $scope.PileNumber = "";
    };
    $scope.ShowMoreCommenScreen = function () {
        $("#divMoreCommenScreen").css("left", screen.width / 2 - 60).css("height", "400px").css("width", "800px");
        $('#divMoreCommenScreen').modal('show');
    };
    $scope.deleteComment = function (index) {
        $scope.camp.ModelListcampcomment.splice(index, 1);
    };
    $scope.deleteCommentRes = function (model) {
        model.CommentRes = "";
    };
}]); 