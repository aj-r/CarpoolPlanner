var app = angular.module('carpoolApp', ['ui.bootstrap', 'ngAnimate', 'angular.net', 'validation-ext', 'ui.bootstrap.datetimepicker']);

app.config(function($locationProvider) {
  // Enable HTML5 mode so that $location.search() can read query strings that are not preceded by a hash (#).
  // However, we don't actually want to use the History API, so disable <base> and link rewriting.
  $locationProvider.html5Mode({ enabled: true, requireBase: false, rewriteLinks: false });
});

app.controller('baseCtrl', ['$scope', '$q', '$window', 'AngularNet', 'ValidationSummary', function($scope, $q, $window, AngularNet, ValidationSummary) {
  $scope.RecurrenceType = $window.RecurrenceType;
  $scope.MessageType = $window.MessageType;
  $scope.CommuteMethod = $window.CommuteMethod;
  $scope.model = $window.originalModel;
  $scope.trySubmit = function(form, url, modelContainer, modelName, beforesubmit) {
    var scope = this;
    if (modelContainer == null)
      modelContainer = $scope;
    if (modelName == null)
      modelName = "model";
    var model = modelContainer[modelName];
    if (model)
      model.message = '';

    if (form && !ValidationSummary.validate(form)) {
      // Move focus to the first invalid control (NOTE: only works if form has a name). TODO: move to a directive?
      if (form.$name) {
        $("[name=" + form.$name + "] .ng-invalid").first().focus();
      }
      var deferred = $q.defer();
      var promise = deferred.promise;
      var msg = 'Form validation failed.';
      deferred.reject(msg);
      promise.success = function(a) { };
      promise.error = function(a) { if (a) a(msg); };
      return promise;
    }

    if (beforesubmit)
      beforesubmit();

    return AngularNet.submitModel(model, url)
      .success(function(result) {
        if (result.model) {
          modelContainer[modelName] = result.model;
        }
      })
      .error(function(result) {
        var message;
        if (result && result.model) {
          modelContainer[modelName] = result.model;
          message = result.model.message;
        } else if (typeof (result) == 'string') {
          message = result;
          scope.errorHandler(model, message);
        }
        console.error('Error from server: ' + message);
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
        if (tr.type == RecurrenceType.YearlyByDayOfWeek)
          s += ' of ' + start.format('MMMM');
        s += ' every ';
        if (tr.every > 1) {
          s += tr.every + $scope.getSuffix(tr.every) + ' ';
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

  $scope.errorHandler = function(model, message) {
    if (model) {
      var displayMessage = "An unexpected error occurred. Please report this error to the website administrator.";
      if (message && message.indexOf('<!DOCTYPE') === -1)
        displayMessage += "\nThe error is: " + message;
      model.message = displayMessage;
      model.messageType = $scope.MessageType.Error;
    }
  };
}]);

app.controller('tripInstanceCtrl', ['$scope', function($scope) {

  $scope.userTripInstance = $scope.tripInstance.userTripInstances[$scope.model.userId];

  $scope.tripInstanceToString = function(ti) {
    var date = moment(ti.date);
    // Difference (in days) between ti.Date and today
    var diff = moment(date).startOf('day').diff(moment().startOf('day'), 'days');
    var specifier = '';
    if (diff == 0)
      specifier = '(today) ';
    else if (diff == 1)
      specifier = '(tomorrow) ';
    return date.format('dddd, MMM D [' + specifier + 'at] h:mm a');
  };

  $scope.getAvailableSeats = function() {
    var available = 0;
    angular.forEach($scope.tripInstance.userTripInstances, function(uti) {
      if (uti.attending && (uti.commuteMethod == $scope.CommuteMethod.Driver || uti.canDriveIfNeeded))
        available += uti.seats;
    });
    return available;
  };
  $scope.getRequiredSeats = function() {
    var required = 0;
    angular.forEach($scope.tripInstance.userTripInstances, function(uti) {
      if (uti.attending && uti.commuteMethod != $scope.CommuteMethod.HaveRide)
        required++;
    });
    return required;
  };
  function updateSeats() {
    $scope.availableSeats = $scope.getAvailableSeats();
    $scope.requiredSeats = $scope.getRequiredSeats();

    $scope.drivers = [];
    $scope.passengers = [];
    $scope.waitingList = [];
    $scope.unconfirmed = [];
    angular.forEach($scope.tripInstance.userTripInstances, function(uti) {
      var user = null;
      for (var i in $scope.model.users) {
        if ($scope.model.users[i].id == uti.userId) {
          user = $scope.model.users[i];
          break;
        }
      }
      if (!user)
        return true;
      if (uti.attending == true) {
        if (uti.commuteMethod == $scope.CommuteMethod.Driver) {
          $scope.drivers.push(user);
        } else if (uti.commuteMethod == $scope.CommuteMethod.NeedRide) {
          $scope.passengers.push(user);
        }
      } else if (uti.attending == false && uti.noRoom) {
        $scope.waitingList.push(user);
      } else if (uti.attending == null) {
        $scope.unconfirmed.push(user);
      }
    });
  }
  updateSeats();
  $scope.$watch('userTripInstance', updateSeats, true);
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
      });
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
        + ' ng-show="' + modelName + '.message"'
        + ' class="preserve-newlines">{{ ' + modelName + '.message }}</p>');
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
      node.$setValue = function(value) {
        if (value == ctrl.$modelValue)
          return;
        ctrl.$setViewValue(value);
        ctrl.$render();
      };
      node.$getValue = function() {
        return ctrl.$modelValue;
      };
      elem.change(function() {
        var newValue = elem.prop('checked');
        if (newValue && parentNode.$setValue) {
          parentNode.$setValue(newValue);
        } else if (!newValue && parentNode.$setValue) {
          // If all children were unchecked, uncheck the parent node
          var allUnchecked = true;
          for (var i in parentNode.$children) {
            var childNode = parentNode.$children[i];
            if (childNode && childNode != node && childNode.$getValue && childNode.$getValue()) {
              allUnchecked = false;
              break;
            }
          }
          if (allUnchecked) {
            parentNode.$setValue(false);
          }
        }
        for (var i in node.$children) {
          var childNode = node.$children[i];
          if (childNode && childNode.$setValue)
            childNode.$setValue(newValue);
        }
      });
      if (parentNode.$children)
        parentNode.$children.push(node);
    }
  };
});

// Directive for tri-state checkboxes (using indeterminate state)
app.directive('chTriState', function($timeout) {
  return {
    restrict: 'A',
    require: 'ngModel',
    link: function(scope, elem, attr, ctrl) {
      if (scope.$eval(attr.ngModel) == null)
        elem.prop('indeterminate', true);
      scope.$watch(attr.ngModel, function (newValue, oldValue) {
        if (newValue == oldValue)
          return;
        elem.prop('indeterminate', newValue == null);
      });
      elem.change(function() {
        var indeterminate = elem.prop('indeterminate');
        if (indeterminate) {
          elem.prop('indeterminate', false);
          ctrl.$setViewValue(true);
          ctrl.$render();
        }
      });
    }
  };
});

// Directive for clearing a control's value when it is disabled.
app.directive('cpDisabledValue', ['$window', function($window) {
  return {
    restrict: 'A',
    require: 'ngModel',
    link: function(scope, elem, attr, model) {
      var disabledValue = scope.$eval(attr.cpDisabledValue);
      var storedValue = null;
      if (!attr.ngDisabled)
        return;
      if (scope.$eval(attr.ngDisabled)) {
        storedValue = scope.$eval(attr.ngModel);
        model.$setViewValue(disabledValue || '');
        model.$render();
      }
      scope.$watch(attr.ngDisabled, function(newValue, oldValue) {
        if (newValue == oldValue)
          return;
        if (newValue) {
          storedValue = model.$modelValue;
          model.$setViewValue(disabledValue);
          model.$render();
        } else {
          model.$setViewValue(storedValue);
          storedValue = null;
          model.$render();
        }
      });
    }
  };
}]);

// Add a $submit method to forms to allow programmatic submittion
app.directive('form', function() {
  return {
    require: 'form',
    restrict: 'E',
    link: function(scope, elem, attrs, form) {
      form.$submit = function() {
        form.$setSubmitted();
        scope.$eval(attrs.ngSubmit);
      };
    }
  };
});

// On load, focus the first empty input.
$(document).ready(function() {
  $('input:not([type=image],[type=button],[type=submit],[type=hidden]), textarea, select').each(function() {
    if (this.value == '') {
      this.focus();
      return false;
    }
  });
})