soNgModule.controller("CampDetailCtrl", ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {
    
    if (t2v_CampDetail.CampInfo) {
        $scope.camp = JSON.parse(t2v_CampDetail.CampInfo);
    }

    $scope.InitMap = function ()
    {
        //map.addControl(new BMap.MapTypeControl());   //添加地图类型控件
        var map = new BMap.Map("allmap");
        var point = new BMap.Point($scope.camp.Latitude, $scope.camp.Longitude);
        map.centerAndZoom(point, 12);
        var marker = new BMap.Marker(point);  // 创建标注
        map.addOverlay(marker);
        map.enableScrollWheelZoom(true);
    }
    $scope.InitMap();

    $scope.ShowPileScreen = function () {
        //if ($scope.checkUserStatus()) {
            $("#divSelPile").css("left", screen.width / 2 - 60).css("height", "400px").css("width", "800px");
            $('#divSelPile').modal('show');
        //}
    };

    $scope.ShowMoreCommenScreen = function () {
        $("#divMoreCommenScreen").css("left", screen.width / 2 - 60).css("height", "400px").css("width", "800px");
        $('#divMoreCommenScreen').modal('show');
    };

    $scope.LinkToCampBook = function (PileID) {
        window.location.href = "/Home/CampBook?PileID=" + PileID + "&CampID=" + $scope.camp.CampID + "&BookDate=" + $scope.camp.paramDate;
    };
    $scope.SaveFlag = false;
    $scope.commentcon = "";
    $scope.SaveComment = function () {
        if ($scope.commentform.$valid == false) {
            $scope.SaveFlag = true;
            return;
        }

        $http({
            method: 'post',
            url: '/Home/SaveComment',
            data: { CampID: $scope.camp.CampID, CommentCon: $scope.commentcon }
        }).success(function (data) {
            alert(data.content);
            $scope.commentcon = '';
            if (data.campcomments != undefined)
            {
                $scope.camp.ModelListcampcomment = data.campcomments;
            }
        }).error(function (d, s, h, c) {
            alert("error");
            // TODO: Show something like "Username or password invalid."
        });
    };
    

    $scope.CampCollect = function () {
        if ($scope.checkUserStatus()) {
            $http({
                method: 'post',
                url: '/Home/CampCollect',
                data: { CampID: $scope.camp.CampID }
            }).success(function (data) {
                alert(data);
            }).error(function (d, s, h, c) {
                alert("error");
                // TODO: Show something like "Username or password invalid."
            });
        }
    };
    
}]);