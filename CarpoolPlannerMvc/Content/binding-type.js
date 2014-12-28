(function() {
  var app = angular.module('binding-type', []);
  app.directive('btNumber', function() {
    return {
      restrict: 'A',
      require: 'ngModel',
      scope: {
        model: '=ngModel',
      },
      link: function(scope, element, attrs) {
        if (scope.model && typeof scope.model == 'string') {
          scope.model = parseFloat(scope.model);
        }
        scope.$watch('model', function(val, old) {
          if (typeof val == 'string') {
            scope.model = parseFloat(val);
          }
        });
      }
    };
  });

  app.directive('btDatetime', ['$window', function($window) {
    return {
      require: 'ngModel',
      restrict: 'A',
      link: function(scope, elm, attrs, ctrl) {
        var moment = $window.moment;
        if (!moment)
          throw new Error('datetime binding requires moment.js');
        var format = attrs.btDatetime;
        attrs.$observe('btDatetime', function(newValue) {
          if (format == newValue || !ctrl.$modelValue)
            return;
          format = newValue;
          ctrl.$modelValue = new Date(ctrl.$setViewValue);
        });

        ctrl.$formatters.unshift(function(modelValue) {
          if (!format || !modelValue)
            return "";
          var retVal = moment(modelValue).format(format);
          return retVal;
        });

        ctrl.$parsers.unshift(function(viewValue) {
          if (viewValue instanceof Date)
            return viewValue;
          if (ctrl.$isEmpty(viewValue)) {
            return '';
          }
          // Try to parse using the expected format. If that fails, use the default date parser.
          var moDate = moment(viewValue, format);
          if (!moDate.isValid())
            moDate = moment(viewValue);
          var date = moDate.toDate();
          return date;
        });

        ctrl.$validators.datetime = function(modelValue, viewValue) {
          // consider empty values to be valid
          if (ctrl.$isEmpty(viewValue)) {
            return true;
          }
          if (viewValue instanceof Date) {
            return true;
          }
          // Try to parse using the expected format. If that fails, use the default date parser.
          var moDate = moment(viewValue, format);
          if (!moDate.isValid())
            moDate = moment(viewValue);
          if (moDate && moDate.isValid() && moDate.year() > 1950) {
            return true;
          }
          return false;
        };
      }
    };
  }]);
})();