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
    $scope.GoToEdit = function () {
        window.location.href = "/CampApproval/ApprovalEdit?CampID=0&dt=";
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
            window.location.href = "/CampApproval";
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
            if (type == "submitBy3")
                window.location.href = "/CampApproval/";
            else
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
    $scope.ShowPilePriceScreen = function () {
        if ($scope.checkUserStatus()) {
            $("#divSelPilePrice").css("left", screen.width / 2 - 60).css("height", "600px").css("width", "800px");
            $('#divSelPilePrice').modal('show');
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
    $scope.ValidSave = function () {
        var flag = true;
        if ($scope.camp.CampPhoto == "") {
            alert("请设置默认主营地图片");
            flag = false;
        }
        else if ($scope.camp.ModelListcampphoto.length == 0) {
            alert("请上传营地照片.");
            flag = false;
        }
        return flag
    };
    $scope.SaveApprovalCamp = function (type) {
        if ($scope.ValidSave()){
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
                $("#fine-uploader-left1").html("");
                $scope.fileUpload1();
                responseJSON.success = false;
                $scope.$apply();
            }
            else if (responseJSON.fileName == undefined)
                alert("附件上传失败！");
        });
    };
    $scope.fileUpload1();
    $scope.fileUpload2 = function () {
        $("#fine-uploader-left2").fineUploader({
            request: {
                endpoint: SiteRoot + '/CampFileUpload2'
            },
            params: {},
            multiple: false
        }).on('validate', function (id, fileName) {
        }).on('complete', function (event, id, fileName, responseJSON) {
            if (responseJSON.success) {
                $scope.CampPilePrice.ItemImage = responseJSON.fileName;
                //$("#fine-uploader-left2").html("");
                responseJSON.success = false;
                $scope.$apply();
                //$scope.fileUpload2();
            }
            else if (responseJSON.fileName == undefined)
                alert("附件上传失败！");
        });
    };
    $scope.fileUpload2();
    $scope.RemoveCampPhoto = function (index) {
        if ($scope.camp.CampPhoto == $scope.camp.ModelListcampphoto[index].CampPhoteFile) {
            $scope.camp.CampPhoto = "";
        }
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

    $scope.ShowPilePriceScreen = function () {
        if ($scope.checkUserStatus()) {
            $("#divSelPilePrice").css("left", screen.width / 2 - 60).css("height", "600px").css("width", "800px");
            $('#divSelPilePrice').modal('show');
        }
    };
    var res = { CampPriceID: 0, CampID: $scope.camp.CampID, ItemName: '', ItemUnit: '', ItemPrice: 0, ItemImage: '' };
    $scope.CampPilePrice = angular.copy(res);
    $scope.AddCampPilePrice = function () {
        if ($scope.CampPilePrice.ItemName == "") {
            alert("请输入项目名称");
            return false;
        }
        if ($scope.CampPilePrice.ItemUnit == "") {
            alert("请输入单位");
            return false;
        }
        if ($scope.CampPilePrice.ItemPrice == "") {
            alert("请输入单价");
            return false;
        }
        if ($scope.CampPilePrice.ItemImage == "") {
            alert("请输入图片");
            return false;
        }
        $scope.camp.ModelListcampprice.push($scope.CampPilePrice);
        $scope.CampPilePrice = angular.copy(res);
        $scope.$apply();
    };
    $scope.RemoveCampPilePrice = function (index) {
        $scope.camp.ModelListcampprice.splice(index, 1);
    };

    $scope.ShowMoreCommenScreen = function () {
        $("#divMoreCommenScreen").css("left", screen.width / 2 - 60).css("height", "400px").css("width", "800px");
        $('#divMoreCommenScreen').modal('show');
    };
    $scope.deleteComment = function (index) {
        $scope.CommentDeleteRecord($scope.camp.ModelListcampcomment[index]);
        $scope.camp.ModelListcampcomment.splice(index, 1);
    };
    $scope.CommentDeleteRes = function (model) {
        $http({
            method: 'post',
            url: '/CampApproval/UpdateCommentRes',
            data: { id: model.CampCommentID, cres: '', type: 'DeleteRes' }
        }).success(function (data) {
            model.CommentRes = "";
            alert("删除成功");
        }).error(function (d, s, h, c) {
            alert("error");
        });
    };
    $scope.CommentDeleteRecord = function (model) {
        $http({
            method: 'post',
            url: '/CampApproval/UpdateCommentRes',
            data: { id: model.CampCommentID, cres: '', type: 'DeleteRecord' }
        }).success(function (data) {
            alert("删除成功");
        }).error(function (d, s, h, c) {
            alert("error");
        });
    };
    $scope.CommentUpdateRecord = function (model) {
        $http({
            method: 'post',
            url: '/CampApproval/UpdateCommentRes',
            data: { id: model.CampCommentID, cres: model.CommentRes, type: 'UpdateRes' }
        }).success(function (data) {
            alert("更新成功");
        }).error(function (d, s, h, c) {
            alert("error");
        });
    };
}]);
