var myapp = angular.module('MyApp', []);

myapp.controller('MyController', function ($scope, $http) {

    $scope.printDiv = function (divName) {
       
        var printContents = document.getElementById(divName).innerHTML;
        var popupWin = window.open('', '_blank', 'width=300,height=300');
        popupWin.document.open();
        popupWin.document.write('<html><head><link rel="stylesheet" type="text/css" href="style.css" /></head><body onload="window.print()">' + printContents + '</body></html>');
        popupWin.document.close();
    }

    $scope.LoadData = function (CustomerID) {
       
        var divName='Customerledger';

        $http({
            url: '/Reports/PrintCustomerLedger/' + CustomerID,
            method: 'GET'
        }).success(function (data, status, headers, config) {

            $scope.CusLedgers = data;

            angular.forEach(data, function (value, key) {

                var date = new Date(parseInt($scope.CusLedgers[key].DATE.substr(6)));
                $scope.CusLedgers[key].DATE = date;

            });

            $http({
                url: '/Reports/GetCustomerDetailsByID/' + CustomerID,
                method: 'GET'
            }).success(function (data, status, headers, config) {

                $scope.CustDetails = data;

            });


            alert("Report has been loaded successfully..click on print button now.");
        });

      
    }

    $scope.printDiv2 = function () {

        var divName = 'Customerledger';


            var printContents = document.getElementById(divName).innerHTML;
            var popupWin = window.open('', '_blank', 'width=300,height=300');
            popupWin.document.open();
            popupWin.document.write('<html><head><link rel="stylesheet" type="text/css" href="style.css" /></head><body onload="window.print()">' + printContents + '</body></html>');
            popupWin.document.close();

    }

    $scope.BindCustomerGrid = function () {
     
        $http({
            url: '/Reports/GetAllCustomers/' ,
            method: 'GET'
        }).success(function (data, status, headers, config) {

            $scope.AllCustomers = data;

        });

    }

});