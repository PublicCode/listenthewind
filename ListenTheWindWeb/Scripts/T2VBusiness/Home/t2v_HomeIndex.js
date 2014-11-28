var SiteRoot = t2v_util.history.GetRootPath();
var t2v_HomeIndex = new function () {
    var self = this;
    self.GoesToPage = function (page) {
        var scope = angular.element($("#ng-value")[0]).scope();
        scope.GoesToPageAng(page);
    };
};