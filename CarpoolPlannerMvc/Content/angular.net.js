(function() {
  var app = angular.module('angular.net', []);

  app.factory('AngularNet', ['$rootScope', '$http', function($rootScope, $http) {
    // Posts the model to the specified URL
    return {
      submitModel: function(model, url, state) {
        $rootScope.$broadcast('angular.net.beforesubmit', { url: url, state: state });
        return $http.post(url, model)
          .success(function(result) {
            if (result.redirectUrl) {
              location.href = result.redirectUrl;
            } else {
              $rootScope.$broadcast('angular.net.success', { url: url, model: result.model, state: state });
            }
          })
          .error(function(result) {
            $rootScope.$broadcast('angular.net.error', { url: url, message: result, state: state });
          });
      }
    };
  }]);
})();