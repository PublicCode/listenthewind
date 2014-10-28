var hasProp = Object.prototype.hasOwnProperty;

var util = {
    // Takes an array of objects and creates a flexigrid-friendly object
    flexFormat: function (data) {
        if (!(data instanceof Array)) {
            // Return the param because it isn't an array
            return data;
        }
        // Iterate through the data array 
        var rows = [];
        for (var i = 0; i < data.length; i++) {
            var item = data[i];
            var values = {};
            // Iterate through an element's properties
            for (key in item) {
                // Don't add properties that are nested in the prototype chain
                if (!hasProp.call(item, key)) continue;
                values[key] = item[key];
            }
            rows.push({ cell: values });
        }
        // Flexigrid-friendly return object
        return {
            total: data.length,
            page: 1,
            rows: rows
        };
    },
    jqGridFormattedCell: function (cell, dateCellIndices) {
        // turn an element's properties into an array
        var values = [];
        for (key in cell) {
            // Don't add properties that are nested in the prototype chain
            if (!hasProp.call(cell, key)) continue;
            values.push(cell[key]);
        }
        // Iterate through the cells that need extra processing
        if (dateCellIndices && dateCellIndices.length) {
            for (var i = 0; i < dateCellIndices.length; i++) {
                if (!values[dateCellIndices[i]]) continue;
                values[dateCellIndices[i]] = util.aspDateJSONToDateString(values[dateCellIndices[i]].toString());
            }
        }

        return values;
    },
    // note: http://stackoverflow.com/questions/726334/asp-net-mvc-jsonresult-date-format
    aspDateJSONToDateString: function (value) {
        if (value == undefined || value == null || value == '')
            return '';
        return new Date(parseInt(value.substr(6))).toDCMFormatString();
    }
};

Date.prototype.toDCMFormatString = function () {
    var year = this.getFullYear().toLeftPaddedString(4, '0');
    var month = (this.getMonth() + 1).toLeftPaddedString(2, '0');
    var date = this.getDate().toLeftPaddedString(2, '0');
    var hours = this.getHours().toLeftPaddedString(2, '0');
    var mins = this.getMinutes().toLeftPaddedString(2, '0');
    var secs = this.getSeconds().toLeftPaddedString(2, '0');
    return [year, month, date].join('-') + '  ' + [hours, mins, secs].join(':');
};

Number.prototype.toLeftPaddedString = function (length, character) {
    var str = this.toString();
    while (str.length < length) {
        str = character + str;
    }
    return str;
};

var urlCodeUtil = {
    char2asc: function (ch) {
        return ch.charCodeAt(0).toString(16);
    },
    asc2char: function (asc) {
        return String.fromCharCode("0x" + asc);
    },
    encode: function (str) {
        var ret = "";
        var strSpecial = "!\"#$%&'()*+,/:;<=>?[]^`{|}~%";
        for (var i = 0; i < str.length; i++) {
            var chr = str.charAt(i);
            var c = urlCodeUtil.char2asc(chr);
            if (parseInt("0x" + c) > 0x7f) {
                ret += "%" + c.slice(0, 2) + "%" + c.slice(-2);
            } else {
                if (chr == " ")
                    ret += "%20";
                else if (strSpecial.indexOf(chr) != -1)
                    ret += "%" + c.toString(16);
                else
                    ret += chr;
            }
        }
        return ret;
    },
    decode: function (str) {
        var ret = "";
        for (var i = 0; i < str.length; i++) {
            var chr = str.charAt(i);
            if (chr == "+") {
                ret += " ";
            } else if (chr == "%") {
                var asc = str.substring(i + 1, i + 3);
                if (parseInt("0x" + asc) > 0x7f) {
                    ret += urlCodeUtil.asc2char(asc + str.substring(i + 4, i + 6));
                    i += 5;
                } else {
                    ret += urlCodeUtil.asc2char(asc);
                    i += 2;
                }
            } else {
                ret += chr;
            }
        }
        return ret;
    }
};

var jqgridUtil = {
    addResetButton: function (grid, pagerId) {
        grid.jqGrid('navButtonAdd', '#' + pagerId, {
            caption: "Reset",
            buttonicon: "ui-icon-newwin",
            onClickButton: function () {
                jqgridUtil.resetSearchFilter(grid);
            },
            position: "last",
            title: "Reset",
            cursor: "pointer"
        });
    },
    resetSearchFilter: function (grid) {
        grid.jqGrid('setGridParam', { search: false });

        var postData = grid.jqGrid('getGridParam', 'postData');
        $.extend(postData, { searchField: "", searchString: "", searchOper: "" });
        // for singe search you should replace the line with 
        // $.extend(postData,{searchField:"",searchString:"",searchOper:""}); 

        grid.trigger("reloadGrid", [{ page: 1}]);
    }
};

var dialogUtil = {
    dialog: function (elem) {
        elem.dialog();
    },

    modalDialog: function (elem) {
        elem.dialog({ modal: true, overlay: { opacity: 0.5, background: "black"} });
    },

    ajaxDialog: function (elem, url, data, isCached, width, height, onSuccess) {
        $.ajax({
            type: "GET",
            url: url,
            data: data,
            cache: isCached == null ? true : isCached,
            beforeSend: function (xhr) {
                // TODO: 
            },
            success: function (data, textStatus) {
                elem.html(data);
                elem.dialog({ modal: true, overlay: { opacity: 0.5, background: "black" }, width: width, height: height });
                if (typeof (onSuccess) === 'function')
                    onSuccess();
            }
        });
    },

    closeDialog: function (elem) {
        elem.dialog("close");
    }

};

var browserUtil = {
    addFavorite: function (url, title) {
        if (window.sidebar || window.opera) return true;
        try {
            window.external.AddFavorite(url, title);
        } catch (e) {
            alert("Failed to bookmark this page. Your browser may not be supported. Please bookmark this page manually.");
        }
        return false;
    }
};

var staticData = {
    parts: {
        loading: function() { return "<img src='/Content/Vendor/loading.gif' alt='' />"; }
    }
};

var ajaxUtil = {
    ajax: function (elem, url, data, cache, loading) {
        var _cache = !!cache;
        $.ajax({
            type: "GET",
            url: url,
            data: data,
            cache: _cache,
            beforeSend: function (xmlHttpRequest) {
                if (loading == true) {
                    elem.html(staticData.parts.loading());
                } else if (loading == false) {

                } else {
                    elem.html(loading);
                }
            },
            success: function (data, textStatus) {
                elem.html(data);
            }
        });
    }
};

var tipBoxUtil = {
    tipBox: function (elem, content, width, height, outerColor, innerColor) {
        elem.each(function () {
            var jThis = $(this);
            if (!this.isTipBoxized) {
                jThis.hide();
                var imgTipBoxCloser = "/Content/images/tipbox-closer.png";
                var imgTipBoxCloserHovered = "/Content/images/tipbox-closer-hovered.png";
                var imgTipBoxCloserActivated = "/Content/images/tipbox-closer-activated.png";
                jThis.css({ 'display': 'none', 'float': 'left', 'max-width': '500px', 'padding': '4px 2px 4px 2px', 'border-width': '2px', 'border-style': 'solid', 'border-radius': '4px' });
                if (content) {
                    jThis.html("<div style=' padding:0 10px 10px 10px;'>" + content + "</div>");
                } else {
                    jThis.html("<div style=' padding:0 10px 10px 10px;'>" + jThis.html() + "</div>");
                }
                var closerBar = $("<div style='width: 100%; height: 100%; text-align: right; '></div>").prependTo(jThis);
                //var closer = $("<div style='border-width:2px; border-style: solid; display: inline'></div>").appendTo(closerBar);
                var closerImg = $("<img src='" + imgTipBoxCloser + "' />").appendTo(closerBar);

                // TODO: Fix the float & clear problem.
                if (this.separator == undefined)
                    this.separator = $("<div style='display:none; width:100%;height:" + (jThis.height() + 20) + "px'></div>").insertAfter(jThis);


                closerImg.css('cursor', 'pointer');
                closerImg.mouseover(function () {
                    jThis.attr("src", imgTipBoxCloserHovered);
                });
                closerImg.mouseout(function () {
                    jThis.attr("src", imgTipBoxCloser);
                });
                closerImg.mousedown(function () {
                    jThis.attr("src", imgTipBoxCloserActivated);
                });
                closerImg.mouseup(function () {
                    jThis.attr("src", imgTipBoxCloser);
                });
                closerImg.click(function () {
                    $.ajax({
                        url: "/Tip/SetRead",
                        data: { 'tipName': jThis.attr('tipName') },
                        cache: false,
                        success: function (response, status, xhr) {
                            // Done
                        }
                    });
                    tipBoxUtil.hide(jThis);
                });

                if (width != null)
                    jThis.width(width);
                if (height != null)
                    jThis.height(height);
                if (outerColor != null) {
                    jThis.css('border-color', outerColor);
                } else {
                    jThis.css('border-color', 'burlywood');
                }
                if (innerColor != null) {
                    jThis.css('background-color', innerColor);
                } else {
                    jThis.css('background-color', 'blanchedalmond');
                }
                this.isTipBoxized = true;
            }
        });
    },
    show: function (elem, speed) {
        elem.each(function () {
            if (this.isTipBoxized) {
                $(this.separator).show();
                if (speed == null)
                    $(this).show(120);
                else {
                    $(this).show(speed);
                }
            }
        });
    },
    hide: function (elem, speed) {
        elem.each(function () {
            if (this.isTipBoxized) {
                if (speed == null) {
                    $(this).hide(120, function () {
                        // TODO: Fix the float & clear problem.
                        $(this.separator).hide();
                    });
                } else {
                    $(this).hide(speed);
                    // TODO: Fix the float & clear problem.
                    $(this.separator).hide();
                }

            }
        });
    }
};

var mappingUtil = {
    drawMapping: function (obj) {
        var rp = {
            ConnectRecords: [],
            CheckFlag: 0,
            CheckPoints: [],
            Do: function (source, target, type) {
                if (type == 0) {
                    var cons = jsPlumb.getConnections({ source: source, target: target });
                    for (c in cons) {
                        jsPlumb.detach(cons[c]);
                    }
                } else {
                    rp.Connect(source, target);
                }
                if (rp.CheckPoints.length >= 30) {
                    var t = rp.CheckPoints.shift();
                    for (var i = 0; i < t; ++i) {
                        rp.ConnectRecords.shift();
                    }
                }
                rp.CheckPoints.push(rp.CheckFlag);
                rp.CheckFlag = 0;
            },
            Undo: function () {
                if (rp.ConnectRecords.length == 0) {
                    alert("Sorry. Cannot undo.");
                    return 0;
                }
                var record, flag = rp.CheckPoints.pop();
                while ((flag--) > 0 && (record = rp.ConnectRecords.pop())) {
                    if (record.type != 0) {
                        var cons = jsPlumb.getConnections({ source: record.source, target: record.target });
                        for (c in cons) {
                            jsPlumb.detach(cons[c], { fireEvent: false });
                        }
                    } else {
                        rp.Connect(record.source, record.target, { fireEvent: false });
                    }
                }
                return rp.ConnectRecords.length;
            },
            Connect: function (source, target, params) {
                var fireEvent = params == null ? true : params.fireEvent == null ? true : params.fireEvent;
                jsPlumb.detachAllConnections(source, { fireEvent: fireEvent });
                jsPlumb.detachAllConnections(target, { fireEvent: fireEvent });

                var con = jsPlumb.connect({
                    source: source,
                    target: target,
                    anchors: ["LeftMiddle", "RightMiddle"],
                    connector: 'Straight',
                    endpoint: ['Dot', { radius: 3}],
                    paintStyle: { strokeStyle: "#000099", lineWidth: 2 },
                    hoverPaintStyle: { strokeStyle: "#990000", lineWidth: 4 },
                    deleteEndpointsOnDetach: true,
                    maxConnections: 1,
                    fireEvent: fireEvent
                    //            overlays: [
                    //                ["Arrow", { width: 10, length: 30, location:'1', id: "arrow"}]
                    //            ]
                });
                con.setDetachable(false);

                con.bind("mouseenter", function (c) {
                    $("body").css("cursor", "pointer");
                });

                con.bind("mouseexit", function (c) {
                    $("body").css("cursor", "default");
                });

                con.bind('click', function (c) {
                    rp.Do(c.sourceId, c.targetId, 0);

                    // After detaching the cursor stays `pointer` till it enters another connection. Force a reset of cursor.
                    $("body").css("cursor", "default");
                });

                return con;
            }
        };
        var draggableSel = obj.draggable,
            droppableSel = obj.droppable,
            containmentSel = obj.containment,
            undoSel = obj.undoBtn,
            saveSel = obj.saveBtn,
            onSave = obj.onSave,
            preMappingData = obj.preMappingData,
            beforePreMapping = obj.beforePreMapping;
        $(obj.draggable).each(function () {
            $(this).attr('id', 'u_' + urlCodeUtil.encode($(this).text()).replace(/\%/g, '_'));
        });
        $(obj.droppable).each(function () {
            $(this).attr('id', 's_' + $(this).text());
        });

        $(draggableSel).draggable({
            containment: containmentSel,
            revert: 'true',
            opacity: 0.7,
            helper: 'clone',
            scroll: true
            //cursorAt: { left: -5, top: -5 }
        });
        $(droppableSel).droppable({
            accept: draggableSel,
            hoverClass: 'upload_time_mapping_target_item:hover',
            drop: function (ev, ui) {
                if (jsPlumb.getConnections({ source: ui.draggable.attr('id'), target: $(this).attr('id') }).length == 0) {
                    //jsPlumb.ready(function() {
                    rp.Do(ui.draggable.attr('id'), $(this).attr('id'), 1);
                }
                //         });
            }
        });
        if (undoSel) {
            $(undoSel).button();
            $(undoSel).button('disable');
            $(undoSel).click(function () {
                if (rp.Undo(rp) <= 0)
                    $(undoSel).button('disable');
            });
        }
        if (saveSel && onSave) {
            $(saveSel).button();
            $(saveSel).click(function () {
                var mappings = [];
                var connections = jsPlumb.getConnections();
                for (var i = 0; i < connections.length; ++i) {
                    mappings.push({ source: urlCodeUtil.decode(connections[i].sourceId.substring(2).replace(/\_/g, '%')), target: connections[i].targetId.substring(2) });
                }
                onSave(mappings);
                jsPlumb.reset();
            });
        }
        if (beforePreMapping) {
            beforePreMapping();
        }
        if (preMappingData) {
            var pm = JSON.parse(preMappingData);
            if (pm) {
                for (var i = 0; i < pm.length; ++i) {
                    if (pm[i] && pm[i].source && pm[i].target) {
                        var sourceId = 'u_' + urlCodeUtil.encode(pm[i].source).replace(/\%/g, '%');
                        var targetId = 's_' + pm[i].target;
                        if (jsPlumb.getConnections({ target: targetId }).length == 0) {
                            rp.Connect(sourceId, targetId);
                        }
                    }
                }
            }
        }
        jsPlumb.bind('jsPlumbConnection', function (c) {
            rp.ConnectRecords.push({ source: c.sourceId, target: c.targetId, type: 1 });
            ++rp.CheckFlag;
            if ($(undoSel).is(':disabled')) {
                $(undoSel).button('enable');
            }
        });

        jsPlumb.bind('jsPlumbConnectionDetached', function (c) {
            rp.ConnectRecords.push({ source: c.sourceId, target: c.targetId, type: 0 });
            ++rp.CheckFlag;
            if ($(undoSel).is(':disabled')) {
                $(undoSel).button('enable');
            }
        });
    }
};

var jsExt = {
    customUnobValBehavior: function ($form, options) {
        if (!options.onError) {
            throw "Missing function on error.";
        }
        if (!options.onSuccess) {
            throw "Missing function on success.";
        }
        var onError = options.onError;
        var onSuccess = options.onSuccess;
        var keepOldBehavior = options.keepOldBehavior == undefined ? true : !!options.keepOldBehavior;
        var settings = $form.data('validator').settings;
        if (keepOldBehavior) {
            var oldErrorFunction = settings.errorPlacement;
            var oldSuccessFunction = settings.success;
        }
        settings.errorPlacement = function (error, inputElement) {
            onError(error, inputElement);
            if (keepOldBehavior)
                oldErrorFunction(error, inputElement);
        };
        settings.success = function (error, inputElement) {
            onSuccess(error, inputElement);
            if (keepOldBehavior)
                oldSuccessFunction(error);
        };

    },
    ajaxEnSuccessFin: function (response) {
        if (response) {
            if (response.redirectTo == "")
                location.reload();
            else if (response.redirectTo) {
                location.href = response.redirectTo;
            }
        }
    },
    submitForm: function (obj, options) {
        var form = $('<form></form>');
        form.css('display', 'none');
        if (typeof (options) == "string") {
            form.attr('action', options);
            form.attr('method', 'post');
        } else {
            if (options.url == undefined || options.url == null)
                throw "Target url is not defined. Make sure you have `url` property in the options.";

            form.attr('action', options.url == '' ? location.href : options.url);
            form.attr('method', options.method == undefined || options.method == null || options.method == '' ? 'post' : options.method);
        }
        for (var i in obj) {
            if (typeof (obj[i]) != 'string' && typeof (obj[i].length) != 'undefined') {
                for (var j = 0; j < obj[i].length; ++j) {
                    var el = $('<input type="hidden" />');
                    el.attr('name', i);
                    el.attr('value', obj[i][j]);
                    form.append(el);
                }
            } else {
                var el = $('<input type="hidden" />');
                el.attr('name', i);
                el.attr('value', obj[i]);
                form.append(el);
            }
        }
        //form.append($('<input type="submit" value="Sub" />'));
        //form.submit();
        form.appendTo('body');
        //form.submit();
        //setTimeout(function () {
        form.ajaxForm({
            success: options.success ? options.success : null
        });
        form.submit();
        //},0);
    },
    //    ajaxSubmitForm: function (obj, options, onsuccess) {
    //        var $form = $('<form></form>');
    //        if (typeof (options) == "string") {
    //            $form.attr('action', options);
    //            $form.attr('method', 'post');
    //        } else {
    //            if (options.url == undefined || options.url == null)
    //                throw "Target url is not defined. Make sure you have `url` property in the options.";

    //            $form.attr('action', options.url == '' ? location.href : options.url);
    //            $form.attr('method', options.method == undefined || options.method == null || options.method == '' ? 'post' : options.method);
    //        }
    //        $form.ajaxForm({
    //            data: obj,
    //            success: onsuccess ? onsuccess : null
    //        });
    //        $form.submit();
    //    },
    ajaxValidateForm: function ($form, validationUrl) {
        $form.ajaxForm({
            url: validationUrl,
            success: function (response, status) {
                if (response && !$.isEmptyObject(response.msg)) {
                    for (var i in response.msg) {
                        var $inner = $("<span class='' for='" + i + "' generated='true'></span>");
                        $inner.text(response.msg[i]);
                        $('[data-valmsg-for="' + i + '"]', $form).append($inner).removeClass('field-validation-valid').addClass('field-validation-error');
                    }
                }
                return false;
            }
        });
    }

};

var jsUiExt = {
    tooltip: function ($obj, text, time, placement, animation) {
        if (!time)
            time = 1000;
        if (!placement)
            placement = 'right';
        if (!animation)
            animation = true;
        $obj.tooltip({
            animation: animation,
            placement: placement,
            title: text,
            trigger: 'manual'
        });
        $obj.tooltip("show");
        window.setTimeout(function() {
            $obj.tooltip("hide");
        }, time);
    },
    blink: function ($obj, color, pulse, times) {
        if (!$obj)
            throw "Selector is required.";
        var _color = $obj.css('background-color');
        if (!color)
            color = "#FF8";
        if (!pulse)
            pulse = 'normal';
        if (!times)
            times = 1;

        function __anim__($obj, index) {
            if (index == 0)
                return;
            $obj.animate({ 'background-color': color }, pulse, 'swing', function () {
                $obj.animate({ 'background-color': _color }, pulse, 'swing', function () {
                    __anim__($obj, index - 1);
                });
            });
        }

        __anim__($obj, times);
    }
}

