soNgModule.controller("QuoteEditCtrl", ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {
    var createItemLine = function () {
        var res = {
            CompanyID: '',
            IsDeleted: false,
            VendorID: '',
            MPN: '',
            ProjectItemID: '',
            ProjectItemLineID: '',
            Vendor: {
                CompanyID: '',
                IsDeleted: false,
                MFRCode: '',
                VendorCode: '',
                VendorID: '',
                VendorName: ''
            }
        }
        return res;
    };
    var createItem = function () {
        var res = {
            CompanyID: '',
            CPN: '',
            Description: '',
            IsDeleted: false,
            Per: 0,
            ProjectID: '',
            ProjectItemID: '',
            ProjectItemLines: [createItemLine()],
            ProjectItemNo: ''
        };
        return res;
    };

    if (t2v_Project.projectInfo) {
        $scope.project = JSON.parse(t2v_Project.projectInfo);
    }
    else {
        var now = new Date();
        $scope.project = {
            CompanyID: '',
            CreatedBy: null,
            CreatedDate: now,
            ModifiedDate: now,
            CheckListOfTemplate: false,
            CheckListOfOrigCustBOM: false,
            CheckListOthers: false,
            CustomerID: '',
            IsDeleted: false,
            ModifiedBy: null,
            ProjectID: '',
            ProjectItems: [createItem()],
            ProjectName: '',
            Messages: [],
            Attachments: [],
        };
    }
    /* <Initialization> */

    $scope.fromJson = function (str) {
        return angular.fromJson(str);
    };

    $scope.currentItemIndex = 0;
    $scope.currentLineIndex = 0;

    if (!$scope.project.ProjectItems) {
        $scope.project.ProjectItems = [];
    }

    angular.forEach($scope.project.ProjectItems, function (v) {
        if (!v.ProjectItemLines) {
            v.ProjectItemLines = [];
        }
    });

    /* </Initialization> */
    $scope.currentItem = function () {
        return $scope.project.ProjectItems[$scope.currentItemIndex];
    };

    $scope.nextItem = function () {
        if ($scope.currentItemIndex == $scope.project.ProjectItems.length - 1) {
            $scope.project.ProjectItems.push(createItem());
            ++$scope.currentItemIndex;
            $scope.currentLineIndex = 0;
        }
        else {
            ++$scope.currentItemIndex;
            $scope.currentLineIndex = 0;
        }

        t2v_Project.getAvlPossibleMatch($scope.currentLine().AVLMfr);
        t2v_Project.getMPNPossibleMatch($scope.currentLine().MPN);
    };

    $scope.previousItem = function () {
        if ($scope.currentItemIndex <= 0)
            $scope.currentItemIndex = 0;
        else {
            --$scope.currentItemIndex;
            $scope.currentLineIndex = 0;
        }

        t2v_Project.getAvlPossibleMatch($scope.currentLine().AVLMfr);
        t2v_Project.getMPNPossibleMatch($scope.currentLine().MPN);
    };

    $scope.deleteItem = function () {
        $scope.project.ProjectItems.splice($scope.currentItemIndex, 1);
        if ($scope.currentItemIndex == $scope.project.ProjectItems.length && $scope.currentItemIndex > 0) {
            --$scope.currentItemIndex;
        }

        t2v_Project.getAvlPossibleMatch($scope.currentLine().AVLMfr);
        t2v_Project.getMPNPossibleMatch($scope.currentLine().MPN);
    };

    $scope.currentLine = function () {
        var currentItem = $scope.currentItem();
        return currentItem.ProjectItemLines[$scope.currentLineIndex];
    };

    $scope.nextLine = function () {
        var currentItem = $scope.currentItem();
        if ($scope.currentLineIndex == currentItem.ProjectItemLines.length - 1) {
            currentItem.ProjectItemLines.push(createItemLine());
            ++$scope.currentLineIndex;
        }
        else {
            ++$scope.currentLineIndex;
        }

        t2v_Project.getAvlPossibleMatch($scope.currentLine().AVLMfr);
        t2v_Project.getMPNPossibleMatch($scope.currentLine().MPN);
    };

    $scope.previousLine = function () {
        if ($scope.currentLineIndex <= 0)
            $scope.currentLineIndex = 0;
        else {
            --$scope.currentLineIndex;
        }
    };

    $scope.deleteItemLine = function () {
        var currentItem = $scope.currentItem();
        currentItem.ProjectItemLines.splice($scope.currentLineIndex, 1);
        if ($scope.currentLineIndex == currentItem.ProjectItemLines.length && $scope.currentLineIndex > 0) {
            --$scope.currentLineIndex;
        }
    };

    $scope.saveProject = function () {
        if ($scope.projectForm.$valid) {
            t2v_Project._showLoadingForDetail();
            $scope.project.ProjectStatus = "Submit";
            $http.post(SiteRoot + "/Project/Save", $scope.project)
            .success(function (d, s, h, c) {
                t2v_Project._hideLoadingForDetail();
                this.location.href = "/Project#/Detail/" + d.data.projectId;
                //t2v_Project._detail(d.data.projectId);
            })
            .error(function (d, s, h, c) {
                t2v_angular.alertDebugError(d, s, h, c);
            });
        }
        else {
            t2v_Project._showLoadingForDetail();
            $scope.project.ProjectStatus = "Draft";
            $http.post(SiteRoot + "/Project/Save", $scope.project)
            .success(function (d, s, h, c) {
                this.location.href = "/Project#/Detail/" + d.data.projectId;
                //t2v_Project._detail(d.data.projectId);
            })
            .error(function (d, s, h, c) {
                t2v_angular.alertDebugError(d, s, h, c);
            });
        }
    };

    $scope.Delete = function () {

        t2v_util.t2vconfirm.showConfirm("Are you sure you want to delete ?", function () {
            $.ajax({
                url: SiteRoot + "/Quote/DeleteQuote",
                type: 'post',
                data: { projectID: $scope.project.ProjectID },
                beforeSend: function () {
                    t2v_Project._showLoading();
                },
                success: function (data, status, jqXhr) {
                    //t2v_Project._hideLoading();

                    t2v_util.t2valert.showAlert("Delete successful .", "Vario", 1, function () { window.location.href = SiteRoot + "/Project"; });

                }
            });
        }, function () { }, "Vario");
    };

    $scope.Cancel = function () {
        if ($scope.project.ProjectID == "") {
            t2v_Project.index();
        } else {
            window.location.href = "/Project#/Detail/" + $scope.project.ProjectID;
        }
    };

    $scope.SetCurrentLinePosition = function () {

        var bomLineGuid = t2v_Project.ProjectItemGuid;;
        var approveLineGuid = t2v_Project.ProjectItemLineGuid;;

        var bomIndex = 0;
        var lineIndex = 0;
        for (i = 0; i < $scope.project.ProjectItems.length; i++) {
            if ($scope.project.ProjectItems[i].ProjectItemID == bomLineGuid) {
                bomIndex = i;
            }
            for (j = 0; j < $scope.project.ProjectItems[i].ProjectItemLines.length; j++) {
                if ($scope.project.ProjectItems[i].ProjectItemLines[j].ProjectItemLineID == approveLineGuid) {
                    lineIndex = j;
                }
            }
        }

        $scope.currentItemIndex = bomIndex;
        $scope.currentLineIndex = lineIndex;
    };

    $scope.ChangeLinePage = function (bomlineno, index, projectItemGuid, projectItemLineGuid) {

        t2v_Project.ProjectItemGuid = projectItemGuid;
        t2v_Project.ProjectItemLineGuid = projectItemLineGuid;

        $scope.currentItemIndex = bomlineno;
        $scope.currentLineIndex = index;
    };

}]);