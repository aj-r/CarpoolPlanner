var app = angular.module('carpoolApp', ['angular.net', 'validation-summary', 'ui.bootstrap.datetimepicker']);
app.controller('baseCtrl', ['$scope', '$q', 'AngularNet', function($scope, $q, AngularNet) {
  $scope.RecurrenceType = RecurrenceType;
  $scope.MessageType = MessageType;
  $scope.model = originalModel;
  $scope.trySubmit = function(form, url, modelContainer, modelName, beforesubmit) {
    if (!this.validateForm(form)) {
      return $q(function(resolve, reject) {
        reject('Form validation failed.');
      });
    }
    if (modelContainer == null) {
      modelContainer = $scope;
    }
    if (modelName == null) {
      modelName = "model";
    }
    var state = {
      form: form,
      modelContainer: modelContainer,
      modelName: modelName
    };
    var model = modelContainer[modelName];

    if (beforesubmit)
      beforesubmit();

    return AngularNet.submitModel(model, url, state)
      .success(function(result) {
        if (result.model) {
          modelContainer[modelName] = result.model;
        }
      })
      .error(this.errorHandler);
  };

  $scope.getSuffix = function(num) {
    if (num < 0)
      num *= -1;
    num %= 100;
    if (num >= 11 && num <= 13)
      return 'th';
    switch (num % 10) {
      case 1:
        return 'st';
      case 2:
        return 'nd';
      case 3:
        return 'rd';
      default:
        return 'th';
    }
  }

  $scope.tripRecurrenceToString = function(tr) {
    var s = '';
    if (!tr || !tr.Start)
      return s;
    var start = moment(tr.Start);
    switch (tr.Type) {
      case RecurrenceType.Yearly:
        s += start.format('MMMM Do');
        s += ' every ';
        if (tr.Every > 1) {
          s += tr.Every + $scope.getSuffix(tr.Every) + ' ';
        }
        s += 'year';
        break;
      case RecurrenceType.YearlyByDayOfWeek:
      case RecurrenceType.MonthlyByDayOfWeek:
        var week = Math.floor((start.date() - 1) / 7) + 1;
        s += 'The ' + week + $scope.getSuffix(week) + ' ' + start.format('dddd');
        if (tr.Type == RecurrenceType.YearlyByDayOfWeek)
          s += ' of ' + start.format('MMMM');
        s += ' every ';
        if (tr.Every > 1) {
          s += tr.Every + $scope.getSuffix(tr.Every) + ' ';
        }
        s += tr.Type == RecurrenceType.YearlyByDayOfWeek ? 'year' : 'month';
        break;
      case RecurrenceType.Monthly:
        s += 'The ' + start.date() + $scope.getSuffix(start.date()) + ' of every ';
        if (tr.Every > 1) {
          s += tr.Every + $scope.getSuffix(tr.Every) + ' ';
        }
        s += 'month';
        break;
      case RecurrenceType.Weekly:
        s += 'Every ';
        if (tr.Every > 1) {
          s += tr.Every + $scope.getSuffix(tr.Every) + ' ';
        }
        s += start.format('dddd');
        break;
      case RecurrenceType.Daily:
        s += 'Every day';
        break;
    }
    s += ' at ' + start.format('h:mm a');
    return s;
  };

  $scope.tripInstanceToString = function(ti) {
    var date = moment(ti.Date);
    var diff = date.diff(moment(), 'days'); // Difference between ti.Date and today
    var specifier = '';
    if (diff == 0)
      specifier = '(today) ';
    else if (diff == 1)
      specifier = '(tomorrow) ';
    return date.format('dddd, MMM d [' + specifier + 'at] h:mm a');
  };

  $scope.errorHandler = function(message) {
    model = modelContainer[modelName];
    if (model) {
      var displayMessage = "An unexpected error occurred. Please report this error to the website administrator.";
      if (message && message.indexOf('<!DOCTYPE') === -1)
        displayMessage += "\nThe error is: " + message;
      model.Message = displayMessage;
      model.MessageType = $scope.MessageType.Error;
    }
  };
}]);

app.directive('numericBinding', function() {
  return {
    restrict: 'A',
    require: 'ngModel',
    scope: {
      model: '=ngModel',
    },
    link: function(scope, element, attrs, ngModelCtrl) {
      if (scope.model && typeof scope.model == 'string') {
        scope.model = parseInt(scope.model);
      }
      scope.$watch('model', function(val, old) {
        if (typeof val == 'string') {
          scope.model = parseInt(val);
        }
      });
    }
  };
});

app.directive('datetimeFormat', ['$window', function($window) {
  return {
    require: 'ngModel',
    restrict: 'A',
    link: function(scope, elm, attrs, ctrl) {
      var moment = $window.moment;
      var format = attrs.datetimeFormat;
      attrs.$observe('datetimeFormat', function(newValue) {
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