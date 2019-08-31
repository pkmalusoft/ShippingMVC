var myapp = angular.module('MyApp', []);

myapp.service('ngservice', function ($http) {
    //The function to read all Orders
    this.getSesion = function () {
        var res = $http.get("/CostUpdation/getSuccessID/");
        return res;
    };

});

myapp.controller('MyController', function ($scope, $http, ngservice) {

    $scope.SuccessAlert = false;
    //$scope.ErrorAlert = false;

    loadSsion();

    function loadSsion() {

        var promise = ngservice.getSesion();

        promise.then(function (resp) {
            var sID = resp.data;

            if (sID > 0) {
                $scope.SuccessAlert = true;
                //$scope.ErrorAlert = false;
            }
            else {
                $scope.SuccessAlert = false;
                //$scope.ErrorAlert = false;
            }

        });

    };

});