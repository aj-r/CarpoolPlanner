var app = angular.module('carpoolApp', ['angular.net', 'val-summary', 'binding-type', 'ui.bootstrap.datetimepicker']);
app.controller('baseCtrl', ['$scope', '$q', '$window', 'AngularNet', function($scope, $q, $window, AngularNet) {
  $scope.RecurrenceType = $window.RecurrenceType;
  $scope.MessageType = $window.MessageType;
  $scope.model = $window.originalModel;
  $scope.trySubmit = function(form, url, modelContainer, modelName, beforesubmit) {
    var scope = this;
    if (this.validateForm && !this.validateForm(form)) {
      var deferred = $q.defer();
      var promise = deferred.promise;
      var msg = 'Form validation failed.';
      deferred.reject(msg);
      promise.success = function(a) { };
      promise.error = function(a) { if (a) a(msg); };
      return promise;
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
      .error(function(result) {
        console.error('Error from server: ' + result);
        scope.errorHandler(model, result);
      });
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
    if (!tr || !tr.start)
      return s;
    var start = moment(tr.start);
    switch (tr.type) {
      case RecurrenceType.Yearly:
        s += start.format('MMMM Do');
        s += ' every ';
        if (tr.every > 1) {
          s += tr.every + $scope.getSuffix(tr.every) + ' ';
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
        if (tr.every > 1) {
          s += tr.eery + $scope.getSuffix(tr.every) + ' ';
        }
        s += tr.type == RecurrenceType.YearlyByDayOfWeek ? 'year' : 'month';
        break;
      case RecurrenceType.Monthly:
        s += 'The ' + start.date() + $scope.getSuffix(start.date()) + ' of every ';
        if (tr.every > 1) {
          s += tr.every + $scope.getSuffix(tr.every) + ' ';
        }
        s += 'month';
        break;
      case RecurrenceType.Weekly:
        s += 'Every ';
        if (tr.every > 1) {
          s += tr.every + $scope.getSuffix(tr.every) + ' ';
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
    var date = moment(ti.date);
    var diff = date.diff(moment(), 'days'); // Difference between ti.Date and today
    var specifier = '';
    if (diff == 0)
      specifier = '(today) ';
    else if (diff == 1)
      specifier = '(tomorrow) ';
    return date.format('dddd, MMM d [' + specifier + 'at] h:mm a');
  };

  $scope.errorHandler = function(model, message) {
    if (model) {
      var displayMessage = "An unexpected error occurred. Please report this error to the website administrator.";
      if (message && message.indexOf('<!DOCTYPE') === -1)
        displayMessage += "\nThe error is: " + message;
      model.message = displayMessage;
      model.messageType = MessageType.Error;
    }
  };
}]);

// Directive for nav links. Adds a 'current-page' class if the link points to the current location.
app.directive('cpNav', ['$window', function($window) {
  function processHref(elem, href) {
    // Determine the normalized path by:
    // - stripping out the query string and hash
    // - removing parts of the path that are equal to the default route values (i.e. 'Home', 'Index')
    var parts = $window.location.pathname.split('/');
    if (parts.length == 3 && parts[2] == 'Index')
      parts.splice(2, 1);
    if (parts.length == 2 && parts[1] == 'Home')
      parts.splice(1, 1);
    var path = parts.join('/') || '/';
    if (path === href) {
      elem.addClass('current-page');
    } else {
      elem.removeClass('current-page');
    }
  }
  return {
    restrict: 'A',
    link: function(scope, elem, attr) {
      processHref(elem, attr.href);
      attr.$observe('href', function(newValue, oldValue) {
        if (newValue === oldValue)
          return;
        processHref(elem, attr.href);
      })
    }
  };
}]);

// Directive for diplaying the message for a certain ViewModel.
app.directive('cpMessage', function() {
  return {
    restrict: 'E',
    template: function(elem, attr) {
      var modelName = attr.model || 'model';
      elem.append(
        '<p ng-class="{ \'text-success\': ' + modelName + '.messageType === MessageType.Success, \'text-danger\': ' + modelName + '.messageType === MessageType.Error }"'
        + 'class="preserve-newlines">{{ ' + modelName + '.message }}</p>');
    }
  };
});

// Directive for building checkbox hierarchies.
// Attribute value should be in the format "parent.level1Child.level2Child"
// When a parent checkbox is checked, all of its children are automatically checked.
// When a parent checkbox is unchecked, all of its children are automatically unchecked.
// When a child checkbox is checked, its parent is automatically checked (but not its siblings).
app.directive('chHierarchy', function($timeout) {
  var tree = {};

  function createNode() {
    return {
      $children: [],
      $suppressParentChange: false
    }
  }

  return {
    restrict: 'A',
    require: 'ngModel',
    link: function(scope, elem, attr, ctrl) {
      var hierarchy = attr.chHierarchy.split('.');
      var parentNode = tree;
      for (var i = 0; i < hierarchy.length - 1; i++) {
        var name = hierarchy[i];
        if (!parentNode[name]) {
          parentNode[name] = createNode();
        }
        parentNode = parentNode[name];
      }
      var name = hierarchy[hierarchy.length - 1];
      var node = parentNode[name] = createNode();
      node.$value = elem.prop('checked'),
      node.$setValue = function(value) {
        if (value == ctrl.$modelValue)
          return;
        ctrl.$setViewValue(value);
        ctrl.$render();
      };
      elem.change(function() {
        var newValue = elem.prop('checked');
        if (newValue == node.$value)
          return;
        node.$value = newValue;
        if (newValue && parentNode.$setValue) {
          parentNode.$suppressParentChange = true;
          parentNode.$setValue(newValue);
          parentNode.$suppressParentChange = false;
        }
        if (!node.$suppressParentChange) {
          for (var i in node.$children) {
            var childNode = node.$children[i];
            if (childNode && childNode.$setValue)
              childNode.$setValue(newValue);
          }
        }
      });
      if (parentNode.$children)
        parentNode.$children.push(node);
    }
  };
});