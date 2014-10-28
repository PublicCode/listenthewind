var constIndexDiv = "<div class='CreateArea'><input type='button' class='btnExport' style='width: 35px !important; height: 32px !important' value='&nbsp;' title='Export Quotes' onclick='t2v_Quote.ExportQuoteList();' /><a href='/Quote#/Create/0' title='Create New Quote' class='AuthClaim'>&nbsp;</a><div class='search_box'><input type='text' id='fullSearchBox' placeholder='Search String' class='search_input watermark' onkeypress='javascript:t2v_Quote.Searchenter(event)' /><input type='button' class='btnSearch' value=' ' title='Search' onclick='javascript: t2v_Quote.SearchContent()' /></div></div><br /><div class='pageSection'><div id='divList'></div></div>";
t2v_SiteRouteForRMA = new function () {
    this._initRoute = function() {
        var app = $.sammy(function () {
            this.get('/RMA#/Detail/:id={0}', function () {
                var id = this.params['id'];
                t2v_RMA._detail(id);
            });
            this.get('/RMA#/Detail/:id/t/:t', function () {
                var id = this.params['id'];
                t2v_RMA._detail(id);
            });
            this.get('/RMA#/Create/:id={0}', function () {
                var id = this.params['id'];
                t2v_RMA.CreateQuote();
            });
            this.get('/RMA#/Edit/:id={0}', function () {
                $('#divMainPageDiv').css("display", "none");
                var id = this.params['id'];
                t2v_RMA.CreateQuote(id);
            });
            this.get('/RMA#/Edit/:id={0}/t/:t', function () {
                $('#divMainPageDiv').css("display", "none");
                var id = this.params['id'];
                t2v_RMA.CreateQuote(id);
            });
            this.get('/RMA#/List/:page={0}', function () {
                var pageIndex = this.params['page'];
                $('#divMainPageDiv').html(constIndexDiv);
                $('#divMainPageDiv').css("display", "block");
                $("#divMainPageDiv").css("padding", "0");
                t2v_RMA.Fullsearch("", " ", pageIndex);
            });
            this.get('/RMA#/page/:index/search/:content', function () {
                $("#divMainPageDiv").css("padding", "0");
                $("#divMainPageDiv").html(constIndexDiv);
                var search = this.params['content'];
                var pageIndex = this.params['index'];
                $("#fullSearchBox").val(search);
                t2v_RMA.Fullsearch("", search, pageIndex);
            });
            //this.get(/Project#\/page\/[1-9]d*\/(.*)/, function () {
            this.get(/RMA#\/page\/(.*)/, function () {
                $("#divMainPageDiv").css("padding", "0");
                $("#divMainPageDiv").html(constIndexDiv);
                //1/search/MSTBVA2,5/14-G-5,08 (Ord# 1755859)/
                var paramaters = this.params.splat;
                var pageIndex = paramaters[0].substring(0, 1);
                var search = paramaters[0].substring(9);
                $("#fullSearchBox").val(search);
                t2v_RMA.Fullsearch("", search, pageIndex);
            });
        });
        $(function () { app.run('/RMA#/List/1'); });
    }
}