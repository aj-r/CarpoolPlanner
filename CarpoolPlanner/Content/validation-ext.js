(function() {
  var app = angular.module('validation-ext', []);

  app.factory('ValidationSummary', ['$rootScope', function($rootScope) {
    var validationMessages = {
      required: '$1 is required.',
      phone: '$1 is invalid: $2',
      $default: '$1 is invalid.'
    };
    var validate = function(form) {
      var errors = [];
      if (!form.$valid) {
        // Display validation summary
        for (var validator in form.$error) {
          for (var i in form.$error[validator]) {
            var field = form.$error[validator][i];
            var message = field.valMessage || this.validationMessages[validator] || this.validationMessages.$default;
            if (message){
              var name = field.valFriendlyName || field.$name || 'Unnamed field';
              var validatorMessage = field[validator + 'Error'] || '';
              message = message.replace(/\$1(?![0-9])/g, name).replace(/\$2(?![0-9])/g, validatorMessage);
              errors.push(message);
            }
          }
        }
      }
      $rootScope.$broadcast('ValidationSummary.validate', { form: form, errors: errors });
      return form.$valid;
    }
    return {
      validationMessages: validationMessages,
      validate: validate
    };
  }]);

  // When this directive is applied to a form, it sets the focus to the first invalid control
  // when ValidationSummary.validate() is called.
  app.directive('focusFirstInvalid', function() {
    return {
      restrict: 'A',
      require: 'form',
      link: function(scope, elem, attr, form) {
        scope.$on('ValidationSummary.validate', function(e, args) {
          // Ignore validation event for other forms
          if (args.form != form || form.$valid)
            return;
          angular.element(elem).find('.ng-invalid').first().focus();
        });
      }
    };
  });

  app.directive('valSummary', ['ValidationSummary', function(ValidationSummary) {
    var result = {
      restrict: 'E',
      require: '?^form',
      scopre: {
        errors: []
      },
      link: function(scope, elem, attr, form) {
        if (scope.form == null)
          scope.form = form;
        scope.$on('ValidationSummary.validate', function(e, args) {
          // Ignore validation event for other forms
          if (scope.form && args.form != scope.form)
            return;
          scope.errors = args.errors;
        });
      }
    };
    if (ValidationSummary.templateUrl)
      result.templateUrl = ValidationSummary.templateUrl;
    else if (ValidationSummary.template)
      result.template = ValidationSummary.templateUrl;
    else
      result.template = '<p ng-show="errors.length > 0">'
        + '<span ng-repeat="error in errors track by $index"><br ng-hide="$first" />{{ error }}</span>'
        + '</p>';
    return result;
  }]);

  app.directive('valFriendlyName', function() {
    return {
      require: 'ngModel',
      restrict: 'A',
      link: function(scope, elm, attr, ctrl) {
        ctrl.valFriendlyName = attr.valFriendlyName;
        attr.$observe('valFriendlyName', function(newValue) {
          ctrl.valFriendlyName = newValue;
        });
      }
    };
  });

  app.directive('valMessage', function() {
    return {
      require: 'ngModel',
      restrict: 'A',
      link: function(scope, elm, attr, ctrl) {
        ctrl.valMessage = attr.valMessage;
        attr.$observe('valMessage', function(newValue) {
          ctrl.valMessage = newValue;
        });
      }
    };
  });

  app.directive('valEquals', function() {
    return {
      require: 'ngModel',
      restrict: 'A',
      link: function(scope, elm, attr, ctrl) {
        var targetPath = attr.valEquals;
        var modelPath = attr.ngModel;
        attr.$observe('valEquals', function(newValue) {
          targetPath = newValue;
        });
        attr.$observe('ngModel', function(newValue) {
          modelPath = newValue;
        });
        scope.$watch(function() {
          return scope.$eval(targetPath);
        }, validator);
        scope.$watch(function() {
          return scope.$eval(modelPath);
        }, validator);

        function validator() {
          var valid = scope.$eval(modelPath) == scope.$eval(targetPath);
          ctrl.$setValidity('equals', valid);
        }
      }
    };
  });

  app.directive('valNumber', function() {
    return {
      restrict: 'A',
      require: 'ngModel',
      link: function(scope, element, attr, ctrl) {
        ctrl.$parsers.unshift(function(viewValue) {
          if (ctrl.$isEmpty(viewValue)) {
            return null;
          }
          var num = parseFloat(viewValue);
          return num;
        });
        ctrl.$validators.number = function(modelValue, viewValue) {
          // consider empty values to be valid
          if (ctrl.$isEmpty(viewValue)) {
            return true;
          }
          var num = parseFloat(viewValue);
          return !isNaN(num);
        };
      }
    };
  });

  app.directive('valInteger', function() {
    return {
      restrict: 'A',
      require: 'ngModel',
      link: function(scope, element, attr, ctrl) {
        ctrl.$parsers.unshift(function(viewValue) {
          if (ctrl.$isEmpty(viewValue)) {
            return null;
          }
          var num = parseInt(viewValue, 10);
          return num;
        });
        ctrl.$validators.integer = function(modelValue, viewValue) {
          // consider empty values to be valid
          if (ctrl.$isEmpty(viewValue)) {
            return true;
          }
          var num = parseInt(viewValue, 10);
          return !isNaN(num);
        };
      }
    };
  });

  app.directive('valPhone', function() {
    return {
      restrict: 'A',
      require: 'ngModel',
      link: function(scope, element, attr, ctrl) {
        var options = scope.$eval(attr.valPhone);
        ctrl.$validators.phone = function(modelValue, viewValue) {
          // consider empty values to be valid
          ctrl.phoneError = null;
          if (ctrl.$isEmpty(viewValue)) {
            return true;
          }
          // Ignore all characters that are not numbers, '+', or 'x'
          // ('+' indicates country code, 'x' indicates extension)
          var normalizedPhone = viewValue.replace(/[^0-9+x]/g, '');
          // Ignore the extension - we will just assume that it is valid.
          var index = normalizedPhone.indexOf('x');
          if (index != -1)
            normalizedPhone = normalizedPhone.substr(0, index);
          // If there is a '+', it must be at the beginning.
          var index = normalizedPhone.indexOf('+');
          var countryCodeRequired = false;
          if (index == 0) {
            normalizedPhone = normalizedPhone.substr(1);
            // If the user specified a '+' sign, then they must also specify the country code.
            countryCodeRequired = true;
          }
          else if (index != -1) {
            ctrl.phoneError = 'unexpected + sign';
            return false;
          }
          // At this point, the normalizedPhone should contain only digits.
          // Validate based on the length.
          if (normalizedPhone.length == 11)
            return true;
          if (!countryCodeRequired && (!options || !options.countryCodeRequired)) {
            if (normalizedPhone.length == 10 || ((!options || options.allow9Digits != false) && normalizedPhone.length == 9))
              return true;
            if (!options || !options.areaCodeRequired) {
              if (normalizedPhone.length == 7)
                return true;
            } else {
              ctrl.phoneError = ' (area code is required)';
            }
          } else {
            ctrl.phoneError = ' (country code is required)';
          }
          ctrl.phoneError = 'incorrect number of digits' + (ctrl.phoneError || '');
          return false;
        };
      }
    };
  });

  app.directive('valDatetime', ['$window', function($window) {
    return {
      require: 'ngModel',
      restrict: 'A',
      link: function(scope, elm, attrs, ctrl) {
        var moment = $window.moment;
        if (!moment)
          throw new Error('val-datetime requires moment.js');
        var options = scope.$eval(attrs.valDatetime);
        var format = options ? options.format : null;

        ctrl.$formatters.unshift(function(modelValue) {
          if (!modelValue)
            return "";
          if (!format)
            return modelValue.toString();
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
          var moDate = null;
          if (format)
            moDate = moment(viewValue, format);
          // TODO: use a number of preset moment parsers instead of default date parsing because it is inconsistent between browsers
          if (!moDate || !moDate.isValid())
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
          var moDate = null;
          if (format)
            moDate = moment(viewValue, format);
          // TODO: use a number of preset moment parsers instead of default date parsing because it is inconsistent between browsers
          if (!moDate || !moDate.isValid())
            moDate = moment(viewValue);
          return moDate && moDate.isValid();
        };
      }
    };
  }]);

})();