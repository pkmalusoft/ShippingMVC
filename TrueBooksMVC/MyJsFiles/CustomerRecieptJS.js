var myapp = angular.module('MyApp', []);

myapp.service('ngservice', function ($http) {
    //The function to read all Orders
    //this.getOrders = function (ID) {
    //    //alert(ID);
    //    var res = $http.get("/CustomerReciept/GetInvoiceOfCustomer/" + ID);
    //    return res;
    //};

});

myapp.controller('MyController', function ($scope, $http, ngservice) {

    $scope.ExR = '';
    $scope.Amt = '';
    $scope.CustomerID = '';
    $scope.Orders = '';
    $scope.Order = null;

    $scope.ck = true;
    $scope.disabledCash = true;

    $scope.CashDiv = true;
    $scope.BankDiv = false;

    $scope.Cash = true;
    $scope.Bank = false;
    $scope.TotalAmt = '0.00';
    $scope.leftAmt = '0.00';

    $scope.ShowHide = function () {

        $scope.CashDiv = true;
        $scope.BankDiv = false;
        $scope.Bank = false;
    }

    $scope.ShowHide2 = function () {
        $scope.ck = false;
        $scope.CashDiv = false;
        $scope.BankDiv = true;
        $scope.Cash = true;
    }

    $scope.getInvoice = function () {
        debugger;
        loadOrders($scope.CustomerID);
    };
    $scope.getTradeInvoice = function () {
        debugger;
        loadTradeOrders($scope.CustomerID);
    };

    $scope.getselectval = function () {
        // alert($scope.ExR);
        if ($scope.ExR != '') {

            $http({
                url: '/CustomerReciept/GetExchangeRateByCurID/' + $scope.ExR,
                method: 'GET'
            }).success(function (data, status, headers, config) {

                angular.forEach(data, function (value, key) {
                    //alert(data[key].ExchangeRate);
                    $scope.exChangeRate = parseFloat(data[key].ExchangeRate).toFixed(2);

                });
            });


        }
        else {
            $scope.exChangeRate = '0.00';
        }
    };



    $scope.getvalueofamount = function () {
        // alert($scope.ExR);
        if ($scope.Amt != '') {
            $scope.TotalAmt = $scope.Amt * $scope.exChangeRate;
            $scope.leftAmt = $scope.FinalAmount - $scope.TotalAmt;

            //loadOrders($scope.CustomerID);
        }
        else {
            $scope.TotalAmt = '0.00';
            $scope.leftAmt = $scope.FinalAmount;
        }
        //alert($scope.TotalAmt);
    };



    function loadOrders(ID) {
        debugger;
        //var promise = ngservice.getOrders(ID);
        //this.getOrders = function (ID) {
        //    //alert(ID);
        //    var res = $http.get("/CustomerReciept/GetInvoiceOfCustomer/" + ID);
        //    return res;
        //};
        //showLoading();
        $http({
            url: '/CustomerReciept/GetInvoiceOfCustomer/' + ID,
            method: 'GET'
        }).success(function (data, status, headers, config) {
            $('#tbl1 tr:gt(0)').remove();
            $scope.Orders = data;
            for (var i = 0; i < data.length; i++) {
                var date = new Date(parseInt(data[i].InvoiceDate.substr(6)));
                var tempdate = new Date(date).getDate() + '/' +( new Date(date).getMonth()+1) + '/' + new Date(date).getFullYear();
               
                $('#tbl1').append('<tr>' +
                    '<td>' + data[i].InvoiceID + ' <input id="" name="CustomerRcieptChildVM[' + i + '].JobID" value=' + data[i].JobID + ' type="hidden"><input id="" name="CustomerRcieptChildVM[' + i + '].InvoiceID" value=' + data[i].InvoiceID + ' type="hidden"></td>' +
                                           '<td>' + data[i].JobCode + '<input id="" name="CustomerRcieptChildVM[' + i + '].JobCode" value=' + data[i].JobCode + ' type="hidden"></td>' +
                                           '<td>' + tempdate + '<input id="" name="CustomerRcieptChildVM[' + i + '].InvoiceDate" value=' + tempdate + ' type="hidden"></td>' +
                    '<td>' + parseFloat(data[i].AmountToBeRecieved).toFixed(2) + '<input id="" name="CustomerRcieptChildVM[' + i + '].AmountToBeRecieved" value=' + data[i].AmountToBeRecieved + ' type="hidden" class="AmountToBeRecieved"></td>' +
                                           '<td>' + parseFloat(data[i].AmtPaidTillDate).toFixed(2) + '<input id="" name="CustomerRcieptChildVM[' + i + '].AmtPaidTillDate" value=' + data[i].AmtPaidTillDate + ' type="hidden"></td>' +
                                           '<td>' + parseFloat(data[i].Balance).toFixed(2) + '<input id="" name="CustomerRcieptChildVM[' + i + '].Balance" value=' + data[i].Balance + ' type="hidden"><input id="" name="CustomerRcieptChildVM[' + i + '].InvoiceNo" value=' + data[i].InvoiceNo + ' type="hidden"></td>' +
                                            //'<td>' + data[i].Amount + '<input id="" name="customerRcieptVM[' + i + '].Amount" value=' + data[i].Amount + ' type="hidden"></td>' +
                                           '<td> <input type="text" onBlur="CheckAmt(this)"  class="amt txtNum text-right AmountReceived" name=CustomerRcieptChildVM[' + i + '].Amount>' +

                                           '<tr>');
                //hideLoading();
            }
            //var FAmt = 0;

            //angular.forEach(data, function (value, key) {

            //    var date = new Date(parseInt($scope.Orders[key].InvoiceDate.substr(6)));
            //    $scope.Orders[key].InvoiceDate = date;


            //        FAmt = FAmt + $scope.Orders[key].AmountToBeRecieved;


            //    if ($scope.TotalAmt >= $scope.Orders[key].AmountToBeRecieved) {

            //        $scope.Orders[key].Amount = $scope.TotalAmt;

            //    }
            //    else {
            //        $scope.Orders[key].Amount = '0.0';
            //    }


            //});

            //$scope.FinalAmount = FAmt;

            // alert(data);



            //promise.then(function (resp) {
            //    debugger;
            //    $scope.Orders = resp;

            //    alert($scope.Orders.length);

            //    $scope.Message = "Call is Completed Successfully";
            //}, function (err) {
            //    $scope.Message = "Call Failed " + err.status;
            //});
        });



        function Test() {
            alert("hii");
        }

    };
    function loadTradeOrders(ID) {
        debugger;
        //var promise = ngservice.getOrders(ID);
        //this.getOrders = function (ID) {
        //    //alert(ID);
        //    var res = $http.get("/CustomerReciept/GetInvoiceOfCustomer/" + ID);
        //    return res;
        //};
        //showLoading();
        $http({
            url: '/CustomerReciept/GetTradeInvoiceOfCustomer/' + ID,
            method: 'GET'
        }).success(function (data, status, headers, config) {
            $('#tbl1 tr:gt(0)').remove();
            $scope.Orders = data;
            for (var i = 0; i < data.length; i++) {
                var date = new Date(data[i].date);
                var tempdate = new Date(date).getDate() + '/' + (new Date(date).getMonth() + 1) + '/' + new Date(date).getFullYear();

                $('#tbl1').append('<tr>' +
                    '<td>' + data[i].InvoiceNo + ' <input id="" name="CustomerRcieptChildVM[' + i + '].JobID" value=' + data[i].JobID + ' type="hidden"><input id="" name="CustomerRcieptChildVM[' + i + '].InvoiceID" value=' + data[i].SalesInvoiceDetailID + ' type="hidden"></td>' +
                    '<td>' + data[i].DateTime + '<input id="" name="CustomerRcieptChildVM[' + i + '].InvoiceDate" value=' + data[i].DateTime + ' type="hidden"></td>' +
                    '<td>' + parseFloat(data[i].InvoiceAmount).toFixed(2) + '<input id="" name="CustomerRcieptChildVM[' + i + '].AmountToBeRecieved" value=' + data[i].InvoiceAmount + ' type="hidden" class="AmountToBeRecieved"></td>' +
                    '<td>' + parseFloat(data[i].AmountReceived).toFixed(2) + '<input id="" name="CustomerRcieptChildVM[' + i + '].AmtPaidTillDate" value=' + data[i].AmountReceived + ' type="hidden"></td>' +
                    '<td>' + parseFloat(data[i].Balance).toFixed(2) + '<input id="" name="CustomerRcieptChildVM[' + i + '].Balance" value=' + data[i].Balance + ' type="hidden"><input id="" name="CustomerRcieptChildVM[' + i + '].InvoiceNo" value=' + data[i].InvoiceNo + ' type="hidden"></td>' +
                    //'<td>' + data[i].Amount + '<input id="" name="customerRcieptVM[' + i + '].Amount" value=' + data[i].Amount + ' type="hidden"></td>' +
                    '<td> <input type="text" onBlur="CheckAmt(this)"  class="amt txtNum text-right AmountReceived" name=CustomerRcieptChildVM[' + i + '].Amount>' +
                    '<td> <input type="text"  onBlur="CheckAmt1(this)" class="amt1 txtNum text-right AdjustmentAmount" name=CustomerRcieptChildVM[' + i + '].AdjustmentAmount>' +

                    '<tr>');
                //hideLoading();
            }
            //var FAmt = 0;

            //angular.forEach(data, function (value, key) {

            //    var date = new Date(parseInt($scope.Orders[key].InvoiceDate.substr(6)));
            //    $scope.Orders[key].InvoiceDate = date;


            //        FAmt = FAmt + $scope.Orders[key].AmountToBeRecieved;


            //    if ($scope.TotalAmt >= $scope.Orders[key].AmountToBeRecieved) {

            //        $scope.Orders[key].Amount = $scope.TotalAmt;

            //    }
            //    else {
            //        $scope.Orders[key].Amount = '0.0';
            //    }


            //});

            //$scope.FinalAmount = FAmt;

            // alert(data);



            //promise.then(function (resp) {
            //    debugger;
            //    $scope.Orders = resp;

            //    alert($scope.Orders.length);

            //    $scope.Message = "Call is Completed Successfully";
            //}, function (err) {
            //    $scope.Message = "Call Failed " + err.status;
            //});
        });



        function Test() {
            alert("hii");
        }

    };

});
