var app = angular.module('CargoApp', []);


app.service('cargoService', function ($http) {

    //this.post = function (Cargo) {

    //    var request = $http({
    //        method: "post",
    //        url: "/Job/AddCargoDescription",
    //        data: Cargo
    //    });
    //    return request;
    //}

    this.getJobId = function () {
        var res = $http.get("/Job/getJobID/");
        return res;
    };


});



app.controller('cargoController', function ($scope, $http, cargoService) {

    //$scope.JobTypeID = '';
    //$scope.JobCode = '';
    //$scope.RevenueTypeID = '';
    //$scope.ProvisionExR = '';
    //$scope.ProexChangeRate = '';
    //$scope.ProvisionRate = '';

    BindCharges();


    BindCargoDes();
    BindAudits();
    BindBills();
    BindContainer();
    BindRevenueType();
    BindProvisionCurrency();

    $scope.getJobPrefix = function () {
        //alert($scope.JobTypeID);
        //LoadJobPrefix($scope.JobTypeID);

        $http({
            url: '/Job/GetJobPrefix/' + $scope.JobTypeID,
            method: 'GET'
        }).success(function (data, status, headers, config) {

            $scope.JobCode = data;
            vactualJObcode = data;

        });
    };

    $scope.getSuppliers = function () {

        debugger;
        $http({
            url: '/Job/GetSupplierOfRevID/' + $scope.RevenueTypeID,
            method: 'GET'
        }).success(function (data, status, headers, config) {
           
            $scope.Sup = data;

        });
    };

    $scope.getSupplierID = function () {
        //alert($scope.Supplier2);
    };

    $scope.getProvisionCurEx = function () {
        $http({
            url: '/Job/GetExchangeRte/' + $scope.ProvisionExR,
            method: 'GET'
        }).success(function (data, status, headers, config) {

            $scope.ProexChangeRate = data;

            if ($scope.ProexChangeRate != '') {

                $scope.ProvisionDomestic = $scope.ProexChangeRate * $scope.ProvisionForeign;

                $scope.LocalCur = $scope.SalesDomestic - $scope.ProvisionDomestic;
            }
            else {
                $scope.ProexChangeRate = '0.00';
            }

        });

    };

    $scope.getSalesCurEx = function () {

        $http({
            url: '/Job/GetExchangeRte/' + $scope.SalesExR,
            method: 'GET'
        }).success(function (data, status, headers, config) {

            $scope.SaleexChangeRate = data;

            if ($scope.SaleexChangeRate != '') {

                $scope.SalesDomestic = $scope.SaleexChangeRate * $scope.SalesForeign;

                $scope.LocalCur = $scope.SalesDomestic - $scope.ProvisionDomestic;
            }
            else {
                $scope.SaleexChangeRate = '0.00';
            }

        });
    };

    $scope.CalcProvForeign = function () {
        // alert($scope.ExR);
        if ($scope.ProvisionRate != '') {
            $scope.ProvisionForeign = $scope.Quantity * $scope.ProvisionRate;

            if ($scope.ProexChangeRate != '' && $scope.ProvisionForeign != '') {
                $scope.ProvisionDomestic = $scope.ProexChangeRate * $scope.ProvisionForeign;

                $scope.LocalCur = $scope.SalesDomestic - $scope.ProvisionDomestic;
            }
        }
        else {
            $scope.ProvisionForeign = '0.00';
        }
    };

    $scope.CalcSaleForeign = function () {
        // alert($scope.ExR);
        if ($scope.SaleRate != '') {
            $scope.SalesForeign = $scope.Quantity * $scope.SaleRate;

            if ($scope.SaleexChangeRate != '' && $scope.SalesForeign != '') {
                $scope.SalesDomestic = $scope.SaleexChangeRate * $scope.SalesForeign;

                $scope.LocalCur = $scope.SalesDomestic - $scope.ProvisionDomestic;
            }
        }
        else {
            $scope.SalesForeign = '0.00';
        }
    };

    $scope.addCharges = function () {
      

        var Charges = {
            InvoiceID: $scope.InvoiceID,
            RevenueTypeID: $scope.RevenueTypeID,
            SupplierID: $scope.Supplier2,
            Quantity: $scope.Quantity,
            Description: $scope.Description,

            ProvisionRate: $scope.ProvisionRate,
            ProvisionCurrencyID: $scope.ProvisionExR,
            ProvisionExchangeRate: $scope.ProexChangeRate,
            ProvisionHome: $scope.ProvisionDomestic,
            ProvisionForeign: $scope.ProvisionForeign,
            SalesRate: $scope.SaleRate,
            SalesCurrencyID: $scope.SalesExR,
            SalesExchangeRate: $scope.SaleexChangeRate,
            SalesHome: $scope.SalesDomestic,
            SalesForeign: $scope.SalesForeign,
            Cost: $scope.LocalCur,
            InvoiceStatus: "1",
            CostUpdationStatus:"1"

        };
      

        $http({
            url: '/Job/AddChargesDetails/',
            method: 'POST',
            data: JSON.stringify(Charges),
            dataType: "json"
        }).success(function (data, status, headers, config) {
            $scope.InvoiceID = 0;
            $scope.RevenueTypeID = '';
            $scope.Supplier2 = '';
            $scope.Description = '';
            $scope.Quantity = '';
            $scope.ProvisionRate = '';
            $scope.ProvisionExR = '';
            $scope.ProexChangeRate = '';
            $scope.ProvisionDomestic = '';
            $scope.ProvisionForeign = '';
            $scope.SaleRate = '';
            $scope.SalesExR = '';
            $scope.SaleexChangeRate = '';
            $scope.SalesDomestic = '';
            $scope.SalesForeign = '';
            $scope.LocalCur = '';

            $scope.RevenueType2 = '';
            $scope.ProvisionCurrencyID = '';
            $scope.SupplierID = '';
            $scope.SalesCurrencyID = '';
            BindCharges();


        });
    };



   

    function BindCharges() {
        $http({
            url: '/Job/GetChargesByJobIdandUserID/',
            method: 'GET'
        }).success(function (data, status, headers, config) {

            $scope.Charges = data;

            $("#AddCharges").show();
        });
    };

    $scope.addAuditDet = function () {

        var Audit = {
            TransDate: $scope.TransDate,
            Remarks: $scope.Remarks
        }

        $http({
            url: '/Job/AddALog/',
            method: 'POST',
            data: JSON.stringify(Audit),
            dataType: "json"
        }).success(function (data, status, headers, config) {

            $scope.TransDate = '';
            $scope.Remarks = '';
            BindAudits();

        });

    };

    $scope.addBillOfEntry = function () {


        var Bill = {
            BIllOfEntry: $scope.BIllOfEntry,
            BillofEntryDate: $scope.BillofEntryDate,
            ShippingAgentID: $scope.ShippingAgentID
        };


        $http({
            url: '/Job/AddBill/',
            method: 'POST',
            data: JSON.stringify(Bill),
            dataType: "json"
        }).success(function (data, status, headers, config) {
            $scope.BIllOfEntry = '';
            $scope.BillofEntryDate = '';
            $scope.ShippingAgentID = '';
            //alert(data);
            BindBills();
        });

    };

    $scope.addContainerDetails = function () {

        var Container = {
            ContainerTypeID: $scope.ContainerTypeID,
            ContainerNo: $scope.ContainerNo,
            SealNo: $scope.CSealNo,
                Description: $scope.CDescription
        };


        $http({
            url: '/Job/AddContainerDet/',
            method: 'POST',
            data: JSON.stringify(Container),
            dataType: "json"
        }).success(function (data, status, headers, config) {
            //alert(data);
            $scope.ContainerTypeID = '';
            $scope.ContainerNo = '';
            $scope.CSealNo = '';
            $scope.CDescription = '';
            BindContainer();
        });

    };

    $scope.addCargoDesc = function () {


        var CargoDes = {
            Mark: $scope.Mark,
            Description: $scope.CarDescription,
            weight: $scope.weight,
            volume: $scope.volume,
            Packages: $scope.Packages,
            GrossWeight: $scope.GrossWeight

        };


        $http({
            url: '/Job/AddCargoDescription/',
            method: 'POST',
            data: JSON.stringify(CargoDes),
            dataType: "json"
        }).success(function (data, status, headers, config) {
            //alert(data);
            $scope.Mark = '';
            $scope.CarDescription = '';
            $scope.weight = '';
            $scope.volume = '';
            $scope.Packages = '';
            $scope.GrossWeight = '';

            BindCargoDes();
        });

    };

    function BindCargoDes() {

        $http({
            url: '/Job/GetCargoByJobIdandUserID/',
            method: 'GET'
        }).success(function (data, status, headers, config) {

            $scope.CargoDescription = data;
            $("#AddCargo").show();

        });
    };

    function BindAudits() {


        $http({
            url: '/Job/GetAuditByJobIdandUserID/',
            method: 'GET'
        }).success(function (data, status, headers, config) {

            //alert(ToJavaScriptDate('/Date(1461868200000)/'));
            //alert(data);
            $scope.Auditing = data;


            angular.forEach(data, function (value, key) {

                var date = new Date(parseInt($scope.Auditing[key].TransDate.substr(6)));
                $scope.Auditing[key].TransDate = date;

            });

        });
    };

    function BindBills() {


        $http({
            url: '/Job/GetBillByJobIdandUserID/',
            method: 'GET'
        }).success(function (data, status, headers, config) {

            $scope.Billing = data;


            $("#AddBill").show();
            angular.forEach(data, function (value, key) {

                var date = new Date(parseInt($scope.Billing[key].BillofEntryDate.substr(6)));
                $scope.Billing[key].BillofEntryDate = date;

            });

        });
    };

    function BindContainer() {


        $http({
            url: '/Job/GetContainerByJobIdandUserID/',
            method: 'GET'
        }).success(function (data, status, headers, config) {

            $scope.Containers = data;
            $("#AddContainer").show();
        });
    };


    function BindRevenueType() {


        $http({
            url: '/Job/GetRevenueTypeList/',
            method: 'GET'
        }).success(function (data, status, headers, config) {

            $scope.RevenueTypeList = data;

        });
    }

    function BindProvisionCurrency() {


        $http({
            url: '/Job/GetCurrencyList/',
            method: 'GET'
        }).success(function (data, status, headers, config) {

            $scope.CurrencyList = data;
          

        });
    }




    $scope.deleteContainer = function (id) {

        //alert(id);
        var val = $scope.JContainerDetailID;
        //alert(val);
        var Container = {
            JContainerDetailID: id
        };


        $http({
            url: '/Job/DeleteContainerJobIdandUserID/',
            method: 'POST',
            data: JSON.stringify(Container),
            dataType: "json"
        }).success(function (data, status, headers, config) {
            BindContainer();
        });

    };

    $scope.deleteBills = function (id) {

        var Bill = {
            BIllOfEntryID: id
        };


        $http({
            url: '/Job/DeleteBillsByID/',
            method: 'POST',
            data: JSON.stringify(Bill),
            dataType: "json"
        }).success(function (data, status, headers, config) {
            BindBills();
        });

    };

    $scope.deleteAuditLogs = function (id) {

        var Aud = {
            JAuditLogID: id
        };


        $http({
            url: '/Job/DeleteAuditLogsByID/',
            method: 'POST',
            data: JSON.stringify(Aud),
            dataType: "json"
        }).success(function (data, status, headers, config) {
            BindAudits();
        });

    };

    $scope.deleteInvoices = function (id) {

        var Inv = {
            InvoiceID: id
        };

        $http({
            url: '/Job/DeleteInvoicesByID/',
            method: 'POST',
            data: JSON.stringify(Inv),
            dataType: "json"
        }).success(function (data, status, headers, config) {
            BindCharges();
        });

    };

    $scope.EditInvoices = function (Charge) {
        debugger;
        $scope.InvoiceID = Charge.InvoiceID;// Charge.InvoiceID;
        $scope.Quantity = Charge.Quantity;

        $scope.Description = Charge.Description;
        $scope.RevenueTypeID = Charge.RevenueTypeID;
        $scope.RevenueTypeID1 = Charge.RevenueTypeID1;
        
        //$scope.RevenueTypeID = Charge.RevenueTypeID
       // $scope.RevenueType2 = Charge.RevenueType; 
        $scope.Supplier2 = Charge.SupplierID;
        $scope.ProvisionRate = Charge.ProvisionRate;
        $scope.ProvisionExR = Charge.ProvisionCurrencyID;
        $scope.ProexChangeRate = Charge.ProvisionExchangeRate;
        $scope.ProvisionDomestic = Charge.ProvisionHome;
        $scope.ProvisionForeign = Charge.ProvisionForeign;
        $scope.SaleRate = Charge.SalesRate;
        $scope.SaleexChangeRate = Charge.SalesExchangeRate;
        $scope.SalesDomestic = Charge.SalesHome;
        $scope.SalesForeign = Charge.SalesForeign;
        $scope.LocalCur = Charge.Cost;
        $scope.ProvisionCurrencyID = Charge.ProvisionCurrencyID;
        $scope.ProvisionExR = Charge.ProvisionCurrencyID;        
        $scope.ProvisionCurrencyID1 = Charge.ProvisionCurrencyID;
        $scope.SupplierID = Charge.SupplierID;
        $scope.SalesCurrencyID = Charge.SalesCurrencyID;
        $scope.SalesCurrencyID1 = Charge.SalesCurrencyID;
        $scope.SalesExR = Charge.SalesCurrencyID;
        //$("#RevenueType").val($scope.RevenueTypeID);
        //$("#ProvisionCurrency").val(Charge.ProvisionCurrencyID);
        //$("#SalesCurrency").val(Charge.SalesCurrencyID);


        $scope.getSuppliers();

    };



    $scope.deleteCargoDescription = function (id) {

        var CargoDes = {
            CargoDescriptionID: id
        };

        $http({
            url: '/Job/DeleteCargoDescriptionByID/',
            method: 'POST',
            data: JSON.stringify(CargoDes),
            dataType: "json"
        }).success(function (data, status, headers, config) {
            BindCargoDes();
        });

    };



});


