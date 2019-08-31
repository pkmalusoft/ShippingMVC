var myapp = angular.module('MyApp', []);

myapp.service('ngservice', function ($http) {

    //this.getSuppliers = function (ID) {

    //    var res = $http.get("/CostUpdation/BindSupplierByJobCode/" + ID);
    //    return res;
    //};

    //this.getGridData = function (ID) {

    //    var res = $http.get("/CostUpdation/BindCostGrid/" + ID);
    //    return res;
    //};

});

myapp.controller('MyCostUpdation', function ($scope, $http) {
    $scope.JobID = '';
    if (vSelectedJobID != undefined)
        $scope.JobID = parseInt(vSelectedJobID);


    $scope.SupplierID = '';
    if (vSupplierId != undefined)
        $scope.SupplierID = parseInt(vSupplierId);



    $scope.costs = [];

    //$scope.BindSuppliers = function () {
    //    //alert('Mayur');
    //    //alert($scope.JobID);

    //    $http({
    //        url: '/CostUpdation/BindSupplierByJobCode/' + $scope.JobID,
    //        method: 'GET'
    //    }).success(function (data, status, headers, config) {

    //        $scope.Sup = data;
    //        // alert(data);
    //    });


    //};

    //$scope.BindJobCode = function () {
    //    //alert('Mayur');
    //    //alert($scope.JobID);
    //    debugger;
    //    $http({
    //        url: '/CostUpdation/BindJobCodeBySupplier/' + $scope.SupplierID,
    //        method: 'GET'
    //    }).success(function (data, status, headers, config) {

    //        $scope.jobid = data;
    //        // alert(data);
    //    });


    //};

    //$scope.BindGridCost = function () {
    //    loadCosts($scope.SupplierID);
    //};
    //$scope.SaveData = function (costs) {
    //    alert(costs.length);
    //}; 

    //function loadCosts(ID) { 
    //    $http({
    //        url: '/CostUpdation/BindCostGrid/',
    //        data: { ID: $('#SupplierID').val(), JobID: $("#JobID").val() },
    //        method: 'GET'
    //    }).success(function (data, status, headers, config) { 
    //        $scope.costs = data;
    //        alert($scope.costs.length);
    //        // alert(data);
    //    });
    //};

});