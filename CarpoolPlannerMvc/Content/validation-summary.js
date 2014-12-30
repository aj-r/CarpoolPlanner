(function() {
  var app = angular.module('val-summary', []);

  app.controller('valSummaryCtrl', ['$scope', function($scope) {
    var validationMessages = {
      required: '$1 is required.',
      $default: '$1 is invalid.'
    };
    $scope.validateForm = function(form) {
      $scope.validationErrors = [];
      if (form.$valid) {
        return true;
      } else {
        // Display validation summary
        for (var validator in form.$error) {
          for (var i in form.$error[validator]) {
            var field = form.$error[validator][i];
            var message = field.valMessage || validationMessages[validator] || validationMessages.$default;
            var name = field.valFriendlyName || field.$name || 'Unnamed field';
            message = message.replace(/\$1(?![0-9])/g, name);
            $scope.validationErrors.push(message);
          }
        }
        // Move focus to the first invalid control (NOTE: only works if form has a name)
        if (form.$name) {
          var selector = "[name=" + form.$name + "] .ng-invalid";
          var elem = jQuery ? jQuery(selector)[0] : document.querySelector(selector);
          if (elem)
            elem.focus();
        }
        return false;
      }
    };
  }]);

  app.directive('valSummary', function() {
    return {
      restrict: 'E',
      template: '<div class="text-danger" ng-repeat="error in validationErrors track by $index">{{ error }}</div>'
        + '<div ng-show="validationErrors.length > 0">&nbsp;</div>'
    };
  });

  app.directive('valFriendlyName', function() {
    return {
      require: 'ngModel',
      restrict: 'A',
      link: function(scope, elm, attrs, ctrl) {
        ctrl.valFriendlyName = attrs.valFriendlyName;
        attrs.$observe('valFriendlyName', function(newValue) {
          ctrl.valFriendlyName = newValue;
        });
      }
    };
  });

  app.directive('valMessage', function() {
    return {
      require: 'ngModel',
      restrict: 'A',
      link: function(scope, elm, attrs, ctrl) {
        ctrl.valMessage = attrs.valMessage;
        attrs.$observe('valMessage', function(newValue) {
          ctrl.valMessage = newValue;
        });
      }
    };
  });

  app.directive('valEquals', function() {
    return {
      require: 'ngModel',
      restrict: 'A',
      link: function(scope, elm, attrs, ctrl) {
        var targetPath = attrs.valEquals;
        var modelPath = attrs.ngModel;
        attrs.$observe('valEquals', function(newValue) {
          targetPath = newValue;
        });
        attrs.$observe('ngModel', function(newValue) {
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

})();