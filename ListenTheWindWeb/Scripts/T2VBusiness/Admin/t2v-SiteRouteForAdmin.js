t2v_SiteRouteForAdmin = new function () {
    this._initRoute = function () {
        var app = $.sammy(function () {
            this.get('/UserProfile#/Detail/:id={0}', function () {
                var id = this.params['id'];
                t2v_AdminSet._detail(id);
            });
            this.get('/UserProfile#/Detail/:id/t/:t', function () {
                var id = this.params['id'];
                t2v_AdminSet._detail(id);
            });
            this.get('/UserProfile#/Create/:id={0}', function () {
                var id = this.params['id'];
                t2v_AdminSet.CreateNewUser();
            });
            this.get('/UserProfile#/Edit/:id={0}', function () {
                $('#divMainPageDiv').css("display", "none");
                var id = this.params['id'];
                t2v_AdminSet.CreateNewUser();
            });
            this.get('/UserProfile#/List/:page={0}', function () {
                var pageIndex = this.params['page'];
                $('#divMainPageDiv').html(constIndexDiv);
                $('#divMainPageDiv').css("display", "block");
                $("#divMainPageDiv").css("padding", "0");
                t2v_AdminSet.Fullsearch(" ", pageIndex);
            });
        });
        $(function () { app.run('/UserProfile#/List/1'); });
    }
}