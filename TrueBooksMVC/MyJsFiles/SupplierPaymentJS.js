var myapp = angular.module('MyApp', []);

myapp.service('ngservice', function ($http) {
    //The function to read all Orders
    //this.getOrders = function (ID) {
    //    //alert(ID);
    //    var res = $http.get("/SupplierPayment/GetCostOfSupplier/" + ID);
    //    return res;
    //};

});

myapp.controller('MyController', function ($scope, $http, ngservice) {
    $scope.ExR = '';
    $scope.Amt = '';
    $scope.SupplierID = '';
    $scope.Orders = [];
    $scope.Order = null;
    
    $scope.ck = true;

    $scope.disabledCash = true;

    $scope.CashDiv = true;
    $scope.BankDiv = false;

    $scope.Cash = true;
    $scope.Bank = false;
 

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
        loadOrders($scope.SupplierID);
    };
    $scope.getTradeInvoice = function () {
        loadTradeOrders($scope.SupplierID);
    };
    $scope.getselectval = function () {
        // alert($scope.ExR);
        if ($scope.ExR != '') {
           
            $http({
                url: '/SupplierPayment/GetExchangeRateByCurID/' + $scope.ExR,
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

            loadOrders($scope.SupplierID);
        }
        else {
            $scope.TotalAmt = '0.00';
            $scope.leftAmt = $scope.FinalAmount;
        }
        //alert($scope.TotalAmt);
    };

    function loadOrders(ID) {
        debugger;
        $http({
            url: '/SupplierPayment/GetCostOfSupplier/' + ID,
            method: 'GET'
        }).success(function (data, status, headers, config) {
           
            $scope.Orders = data;
            $('#tbl1 tr:gt(0)').remove()
           // var FAmt = 0;

            //angular.forEach(data, function (value, key) {

            //    var date = new Date(parseInt($scope.Orders[key].InvoiceDate.substr(6)));

            //    $scope.Orders[key].InvoiceDate = date;

                
            //        FAmt = FAmt + $scope.Orders[key].AmountToBePaid;
               

            //    if ($scope.TotalAmt >= $scope.Orders[key].AmountToBePaid) {


            //        $scope.Orders[key].Amount = $scope.Orders[key].AmountToBePaid;

            //    }
            //    else {
            //        $scope.Orders[key].Amount = '0.0';
            //    }


            //});

           // $scope.FinalAmount = FAmt;
            for (var i = 0; i < data.length; i++) {
                var date = new Date(parseInt(data[i].InvoiceDate.substr(6)));
                var tempdate = new Date(date).getDate() + '/' + (new Date(date).getMonth() + 1) + '/' + new Date(date).getFullYear();

                $('#tbl1').append('<tr>' +
                    '<td>' + data[i].JobID + ' <input id="" name="CustomerRcieptChildVM[' + i + '].JobID" value=' + data[i].JobID + ' type="hidden"><input id="" name="CustomerRcieptChildVM[' + i + '].InvoiceID" value=' + data[i].InvoiceID + ' type="hidden"></td>' +
                    '<td>' + tempdate + '<input id="" name="CustomerRcieptChildVM[' + i + '].InvoiceDate" value=' + tempdate + ' type="hidden"></td>' +
                    '<td>' + data[i].AmountToBePaid + '<input id="" name="CustomerRcieptChildVM[' + i + '].AmountToBePaid" value=' + data[i].AmountToBePaid + ' type="hidden"></td>' +
                    '<td>' + data[i].AmtPaidTillDate + '<input id="" name="CustomerRcieptChildVM[' + i + '].AmtPaidTillDate" value=' + data[i].AmtPaidTillDate + ' type="hidden" class="AmountPaidTillDate"></td>' +
                    '<td>' + data[i].Balance + '<input id="" name="CustomerRcieptChildVM[' + i + '].Balance" value=' + data[i].Balance + ' type="hidden" class = "BalanceAmount"><input id="" name="CustomerRcieptChildVM[' + i + '].InvoiceNo" value=' + data[i].InvoiceNo + ' type="hidden"></td>' +
                    '<td> <input type="text"  onBlur="CheckAmt(this)"  class="amt txtNum text-right AmountAllocated" name=CustomerRcieptChildVM[' + i + '].Amount>' +

                    '</tr>');

            }
         
            

            

        });
    };
    $scope.getvalueofamount = function () {
        // alert($scope.ExR);
        if ($scope.Amt != '') {

            $scope.TotalAmt = $scope.Amt * $scope.exChangeRate;
            $scope.leftAmt = $scope.FinalAmount - $scope.TotalAmt;
            $scope.AdjustAmt = $scope.AdjustAmt;
            $scope.leftAmt = $scope.leftAmt - $scope.AdjustAmt;
            loadTradeOrders($scope.SupplierID);
        }
        else {
            $scope.TotalAmt = '0.00';
            $scope.leftAmt = $scope.FinalAmount;
            $scope.AdjustAmt = '0.00';
        }
    };

    function loadTradeOrders(ID) {
        debugger;
        $http({
            url: '/SupplierPayment/GetCostOfTradeSupplier/' + ID,
            method: 'GET'
        }).success(function (data, status, headers, config) {

            $scope.Orders = data;
            $('#tbl1 tr:gt(0)').remove()            

            for (var i = 0; i < data.length; i++) {
               $('#tbl1').append('<tr>' +
                    '<td>' + data[i].InvoiceNo + ' <input id="" name="CustomerRcieptChildVM[' + i + '].JobID" value=' + data[i].JobID + ' type="hidden"><input id="" name="CustomerRcieptChildVM[' + i + '].InvoiceID" value=' + data[i].PurchaseInvoiceDetailId + ' type="hidden"></td>' +
                    //'<td>' + data[i].JobCode + '<input id="" name="CustomerRcieptChildVM[' + i + '].JobCode" value=' + data[i].JobCode + ' type="hidden"></td>' +
                   '<td>' + data[i].DateTime + '<input id="" name="CustomerRcieptChildVM[' + i + '].InvoiceDate" value=' + data[i].DateTime + ' type="hidden"></td>' +
                   '<td>' + parseFloat(data[i].InvoiceAmount).toFixed(2) + '<input id="" name="CustomerRcieptChildVM[' + i + '].AmountToBePaid" value=' + data[i].InvoiceAmount + ' type="hidden"></td>' +
                   '<td>' + parseFloat(data[i].AmountPaidTillDate).toFixed(2) + '<input id="" name="CustomerRcieptChildVM[' + i + '].AmtPaidTillDate" value=' + data[i].AmountPaidTillDate + ' type="hidden" class="AmountPaidTillDate"></td>' +
                   '<td>' + parseFloat(data[i].Balance).toFixed(2) + '<input id="" name="CustomerRcieptChildVM[' + i + '].Balance" value=' + data[i].Balance + ' type="hidden" class = "BalanceAmount"><input id="" name="CustomerRcieptChildVM[' + i + '].InvoiceNo" value=' + data[i].InvoiceNo + ' type="hidden"></td>' +
                    //'<td>' + data[i].Amount + '<input id="" name="customerRcieptVM[' + i + '].Amount" value=' + data[i].Amount + ' type="hidden"></td>' +
                    '<td> <input type="text"  onBlur="CheckAmt(this)"  class="amt txtNum text-right AmountAllocated" name=CustomerRcieptChildVM[' + i + '].Amount>' +
                    '<td> <input type="text" onBlur="CheckAmt1(this)"  class="amt1 txtNum text-right AdjustmentAmount" name=CustomerRcieptChildVM[' + i + '].AdjustmentAmount>' +

                    //'<td>' + data[i].InvoiceDate + '<input id="" name="customerRcieptVM[' + i + '].InvoiceDate" value=' + data[i].InvoiceDate + ' type="hidden"></td>'+

                    '</tr>');


            }




        });
    };

});