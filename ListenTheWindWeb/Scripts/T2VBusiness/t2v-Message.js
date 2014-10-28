
var t2v_Message = {

    initGrid: function () {
        var colInfo = t2v_Message.GetConifigData();
        var tempColInfo = new Array();
        colInfo.push({ isShowExportExcel: 'true', Handler: t2v_Message.ExportExcel });
        FetureV7Grid.myParam(colInfo, "divList", 20, "/Message/GetMessageList", "", "");
        FetureV7Grid.showGrid();
    },
    GetConifigData: function () {
        var colInfo = new Array();
        colInfo.push({ disName_en: 'Message', columnName: 'InfoForPreview', Width: '50', disType: 'text' });
        colInfo.push({ disName_en: 'Type', columnName: 'MessageType', Width: '50', disType: 'text' });
        colInfo.push({ disName_en: 'Object Number', columnName: 'ONumber', Width: '100', disType: 'text' });
        //InfoForPreview
        colInfo.push({ disName_en: 'User Name', columnName: 'CreateUserName', Width: '100', disType: 'text' });
        colInfo.push({ disName_en: 'Create Time', columnName: 'CreatedOnString', Width: '100', disType: 'text' });
        return colInfo;
    },
    Fullsearch: function () {
        var colInfo = t2v_Message.GetConifigData();
        colInfo.push({ isShowExportExcel: 'true', Handler: t2v_Message.ExportExcel });
        var param = "[";
        param += '{"columnName":"fullsearch"'
                                        + ',"columnValue":"' + t2v_Message.ConvertToJson($("#fullSearchBox").val()) + '","columnOperator":"cn"}]';
        FetureV7Grid.myParam(colInfo, "divLoaderStatusList", 20, "/Message/GetMessageList", param, "CreateTime");
        FetureV7Grid.showGrid($("#fullSearchBox").val());
    },
    Searchenter: function (event) {
        if (event.keyCode == 13) {
            t2v_Message.Fullsearch();
        }
    },

    initSendToMessage: function () {

        var thisObj = $("#MyPasteZone");
        T2VForm.Post("/Message/GetUserAndGroupList", {}
                        , function (serverResponse) {
                            strResult = serverResponse.resultValue;
                            var userList = eval(strResult);

                            $("#MyPasteZone").autocomplete({
                                delay: 200,
                                source: function (request, response) {
                                    var searchContent = request.term;
                                    var alredayExists = false;

                                    for (i = 0; i < userList.length; i++) {
                                        if (thisObj.text().indexOf(userList[i].value) != -1) {
                                            alredayExists = true;
                                        }
                                    }

                                    if (searchContent.indexOf("#") != -1) {
                                        if (alredayExists == true) {
                                            response(null);
                                        }
                                        else {
                                            response(userList);
                                        }
                                    }
                                },
                                select: function (event, ui) {

                                    $("#MyPasteZone").text(ui.item.value);
                                    //for special user
                                    if (ui.item.msgtype == "3") {
                                        $("#hidSendToUserId").val(ui.item.usergroupid);
                                    }
                                }
                            }).focus(function () {
                                //$(this).data("autocomplete").search("d");
                            });
                        });
    }
}