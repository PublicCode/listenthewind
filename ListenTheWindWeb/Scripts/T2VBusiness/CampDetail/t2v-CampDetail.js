var SiteRoot = t2v_util.history.GetRootPath();
var t2v_CampDetail = {

    CampInfo: "",
    PileId: 0,
    BasicData: "",
    DefaultDate: "",
};
var t2v_CampBook = new function() {
    var self = this;
    self.GetTotalCampBookPrice = function(){
        var scope = angular.element($("#ng-value")[0]).scope();
        scope.getTotalCampPrice();
    };
};