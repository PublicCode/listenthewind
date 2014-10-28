var ngDateFormat = "yyyy-MM-dd";
Function.prototype.clone = function () {
    var that = this;
    var temp = function temporary() { return that.apply(this, arguments); };
    for (key in this) {
        temp[key] = this[key];
    }
    return temp;
};

var soNgModule = angular.module('SO', ['ngSanitize']);

soNgModule.directive('watermark', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs, ngModelCtrl) {
            $(function () {
                element.addClass('watermark');
                element.watermark(attrs.watermark);
            });
        }
    }
});
soNgModule.filter('iif', function () {
    return function (input, trueValue, falseValue) {
        return input ? trueValue : falseValue;
    };
});
soNgModule.directive('datepicker', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            $(function () {
                element.addClass('datefield');
                element.datepicker({
                    dateFormat: 'yy-mm-dd',
                    onSelect: function (date) {
                        ngModelCtrl.$setViewValue(date);
                        scope.$apply();
                    }
                });
            });
        }
    }
});

soNgModule.directive('datepickerMaxdate', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            scope.$watch(attrs.datepickerMaxdate, function () {
                var maxDate = scope.$eval(attrs.datepickerMaxdate);
                if (maxDate)
                    element.datepicker("option", "maxDate", maxDate);
            })
        }
    }
});
soNgModule.directive('datepickerMindate', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            scope.$watch(attrs.datepickerMindate, function () {
                var minDate = scope.$eval(attrs.datepickerMindate);
                if (minDate)
                    element.datepicker("option", "minDate", minDate);
            });
        }
    }
});

soNgModule.directive('positiveInt', function () {
    var regex = /^\+?\d+$/;
    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {
            ctrl.$parsers.unshift(function (viewValue) {

                if (regex.test(viewValue)) {
                    // it is valid
                    ctrl.$setValidity('positiveInt', true);
                    return viewValue;
                } else {
                    // it is invalid, return undefined (no model update)
                    ctrl.$setValidity('positiveInt', false);
                    return undefined;
                }
            });
            ctrl.$formatters.unshift(function (value) {
                // validate.
                ctrl.$setValidity('positiveInt', regex.test(value));

                // return the value or nothing will be written to the DOM.
                return value;
            });

        }
    };
});

soNgModule.directive('positiveExclusiveInt', function () {
    var regex = /^\+?0*[1-9]\d*$/;
    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {
            ctrl.$parsers.unshift(function (viewValue) {

                if (regex.test(viewValue)) {
                    // it is valid
                    ctrl.$setValidity('positiveInt', true);
                    return viewValue;
                } else {
                    // it is invalid, return undefined (no model update)
                    ctrl.$setValidity('positiveInt', false);
                    return undefined;
                }
            });
            ctrl.$formatters.unshift(function (value) {
                // validate.
                ctrl.$setValidity('positiveInt', regex.test(value));

                // return the value or nothing will be written to the DOM.
                return value;
            });

        }
    };
});

soNgModule.directive('positiveFloat', function () {
    var regex = /^\+?\d+(\.\d*)?$/;
    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {
            ctrl.$parsers.unshift(function (viewValue) {
                if (regex.test(viewValue)) {
                    // it is valid
                    ctrl.$setValidity('positiveFloat', true);
                    return viewValue;
                } else {
                    // it is invalid, return undefined (no model update)
                    ctrl.$setValidity('positiveFloat', false);
                    return undefined;
                }
            });
            ctrl.$formatters.unshift(function (value) {
                // validate.
                ctrl.$setValidity('positiveFloat', regex.test(value));

                // return the value or nothing will be written to the DOM.
                return value;
            });
        }
    };
});

soNgModule.directive('positiveExclusiveFloat', function () {
    var regex = /^\+?\d+(\.\d*)?$/;
    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {
            ctrl.$parsers.unshift(function (viewValue) {
                if (regex.test(viewValue)) {
                    // it is valid
                    if (parseFloat(viewValue) > 0) {
                        ctrl.$setValidity('positiveExclusiveFloat', true);
                        return viewValue;
                    }
                }
                // it is invalid, return undefined (no model update)
                ctrl.$setValidity('positiveExclusiveFloat', false);
                return undefined;
            });
            ctrl.$formatters.unshift(function (value) {
                // validate.
                if (regex.test(value)) {
                    // it is valid
                    if (parseFloat(value) > 0) {
                        ctrl.$setValidity('positiveExclusiveFloat', true);
                        return value;
                    }
                }
                ctrl.$setValidity('positiveExclusiveFloat', false);
                // return the value or nothing will be written to the DOM.
                return value;
            });
        }
    };
});

soNgModule.directive('minValue', function () {
    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {
            ctrl.$parsers.unshift(function (viewValue) {
                if (viewValue == '' || viewValue == undefined) {
                    ctrl.$setValidity('minValue', true);
                    return undefined;
                }
                var fltValue = parseFloat(viewValue);
                var minValue = parseFloat(scope.$eval(attrs.minValue));
                if (fltValue >= minValue) {
                    ctrl.$setValidity('minValue', true);
                    return viewValue;
                }
                // it is invalid, return undefined (no model update)
                ctrl.$setValidity('minValue', false);
                return undefined;
            });
            ctrl.$formatters.unshift(function (value) {
                // validate.
                if (value == null || value == undefined) {
                    ctrl.$setValidity('minValue', true);
                    return value;
                }
                var fltValue = parseFloat(value);
                var minValue = parseFloat(scope.$eval(attrs.minValue));
                if (fltValue >= minValue) {
                    // it is valid
                    ctrl.$setValidity('minValue', true);
                    return value;
                }
                ctrl.$setValidity('minValue', false);
                // return the value or nothing will be written to the DOM.
                return value;
            });
        }
    };
});

soNgModule.directive('maxValue', function () {
    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {
            ctrl.$parsers.unshift(function (viewValue) {
                if (viewValue == '' || viewValue == undefined) {
                    ctrl.$setValidity('maxValue', true);
                    return undefined;
                }
                var fltValue = parseFloat(viewValue);
                var maxValue = parseFloat(scope.$eval(attrs.maxValue));
                if (fltValue <= maxValue) {
                    ctrl.$setValidity('maxValue', true);
                    return viewValue;
                }
                // it is invalid, return undefined (no model update)
                ctrl.$setValidity('maxValue', false);
                return undefined;
            });
            ctrl.$formatters.unshift(function (value) {
                // validate.
                if (value == null || value == undefined) {
                    ctrl.$setValidity('maxValue', true);
                    return value;
                }
                var fltValue = parseFloat(value);
                var maxValue = parseFloat(scope.$eval(attrs.maxValue));
                if (fltValue <= maxValue) {
                    // it is valid
                    ctrl.$setValidity('maxValue', true);
                    return value;
                }
                ctrl.$setValidity('maxValue', false);
                // return the value or nothing will be written to the DOM.
                return value;
            });
        }
    };
});

soNgModule.directive('allocationTotalQty', function () {
    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            var validator =
                ctrl.$parsers.unshift(function (viewValue) {
                    var orderedQty = parseInt(viewValue);
                    var origVal = scope.$eval(attrs.ngModel);
                    //attrs.$set('ngModel', orderedQty);

                    if (scope.workingLine.OrderedQty > 0) {
                        if (!scope.workingLine.Deliveries || scope.workingLine.Deliveries.length == 0) {
                            ctrl.$setValidity('allocationTotalQty', true);
                            return orderedQty;
                        } else {
                            var totalOrderedQty = scope.workingLine.Deliveries.reduce(function (p, c) {
                                if (c.OrderedQty == origVal) {
                                    return p;
                                }
                                return p + (c.OrderedQty == undefined || c.OrderedQty == null || c.OrderedQty == '' ? 0 : parseInt(c.OrderedQty));
                            }, 0);
                            totalOrderedQty += orderedQty;
                            if (totalOrderedQty == scope.workingLine.OrderedQty) {
                                ctrl.$setValidity('allocationTotalQty', true);
                                return orderedQty;
                            } else {
                                ctrl.$setValidity('allocationTotalQty', false);
                                return orderedQty;
                            }

                        }
                    } else {
                        ctrl.$setValidity('allocationTotalQty', true);
                        return orderedQty;
                    }
                });
            ctrl.$formatters.unshift(function (value) {
                var orderedQty = parseInt(viewValue);
                //attrs.$set('ngModel', orderedQty);

                if (scope.workingLine.OrderedQty > 0) {
                    if (!scope.workingLine.Deliveries || scope.workingLine.Deliveries.length == 0) {
                        ctrl.$setValidity('allocationTotalQty', true);
                        return value;
                    } else {
                        var totalOrderedQty = scope.workingLine.Deliveries.reduce(function (p, c) {
                            return p + (c.OrderedQty == undefined || c.OrderedQty == null || c.OrderedQty == '' ? 0 : parseInt(c.OrderedQty));
                        }, 0);
                        if (totalOrderedQty == orderedQty) {
                            ctrl.$setValidity('allocationTotalQty', true);
                            return value;
                        }
                    }
                } else {
                    ctrl.$setValidity('allocationTotalQty', true);
                    return value;
                }
            });
        }
    }
});

soNgModule.directive('allocationRule', function () {
    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {
            var validator =
            ctrl.$parsers.unshift(function (viewValue) {
                var orderedQty = scope.solc ? scope.solc.OrderedQty : undefined;
                if (viewValue == '' || orderedQty == undefined) {
                    ctrl.$setValidity('allocationRule', true);
                    return 0;
                }

                var allocQty = parseInt(viewValue);

                if (isNaN(allocQty)) {
                    ctrl.$setValidity('allocationRule', false);
                    return undefined;
                }

                else if (allocQty < 0 || allocQty > orderedQty) {
                    ctrl.$setValidity('allocationRule', false);
                    return undefined;
                }
                else {
                    ctrl.$setValidity('allocationRule', true);
                    return allocQty;
                }
            });
            ctrl.$formatters.unshift(function (value) {
                var orderedQty = scope.solc ? scope.solc.OrderedQty : undefined;
                if (orderedQty == undefined) {
                    ctrl.$setValidity('allocationRule', true);
                    return 0;
                }
                if (isNaN(value)) {
                    ctrl.$setValidity('allocationRule', false);
                    return undefined;
                }
                else if (value < 0 || value > orderedQty) {
                    ctrl.$setValidity('allocationRule', false);
                    return value;
                } else {
                    ctrl.$setValidity('allocationRule', true);
                    return value;
                }

                // validate.
                ctrl.$setValidity('allocationRule', regex.test(value));

                // return the value or nothing will be written to the DOM.
                return value;
            });

        }
    };
});

soNgModule.filter('fromNow', function () {
    return function (date) {
        return jQuery.timeago(date);
    }
})
.filter('ellipsis', function () {
    return function (str) {
        var splitted = str.split(' ');
        var res;
        if (splitted.length > 3) {
            res = splitted.slice(0, 3).join(' ') + '...';
        } else {
            res = splitted.join(' ');
        }
        if (res.length > 20) {
            res = res.substring(0, 17) + '...';
        }
        return res;
    }
})
.filter('getByProperty', function() {
        return function(propertyName, propertyValue, collection) {
            var i=0, len=collection.length;
            for (; i<len; i++) {
                if (collection[i][propertyName] == +propertyValue) {
                    return collection[i];
                }
            }
            return null;
        }
})
.filter('trimToLenOrSpc', function () {
    return function (str, len) {
        if (!str) {
            return '';
        }
        if (str.length > len) {
            var pres = str.slice(0, len);
            pres += "...";
            return pres;
        }
        else
            return str;
    }
})
.filter('decimaldigits', function () {
        return function (str, len) {
            if (!str) {
                return '';
            }
            else {
                return parseFloat(str).toFixed(len);
            }
        }
})
.filter('ellipsis15', function () {
    return function (str) {
        if (!str)
            return str;

        var splitted = str.split(' ');
        var res;
        if (splitted.length > 30) {
            res = splitted.slice(0, 29).join(' ') + '...';
        } else {
            res = splitted.join(' ');
        }
        if (res.length > 150) {
            res = res.substring(0, 147) + '...';
        }
        return res;
    }
})
.filter('nl2p', function () {
    return function (text) {
        text = String(text).trim();
        return (text.length > 0 ? '<p>' + text.replace(/[\r\n]+/g, '</p><p>') + '</p>' : null);
    }
});

var t2v_angular = new function () {
    var self = this;
    this.alertDebugError = function (d, s, h, c) {
        var msg = "Sorry, an error has occurred: \n";
        msg += "Data: " + d + "\n";
        msg += "Status: " + s + "\n";
        alert(msg);
    }
};
