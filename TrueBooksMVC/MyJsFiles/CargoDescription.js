var app = angular.module('CargoApp', []);

function deleteCharge(obj) {
    $(obj).closest('tr').remove();
}

function deleteRow(obj) {
    debugger;
    $(obj).closest('tr').remove();
}

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

    BindCharges();


    BindCargoDes();
    BindAudits();
    BindBills();
    BindContainer();
    BindRevenueType();
    BindProvisionCurrency();
    BindContainerType();

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

    var vCharges = JSON.parse('[]');

    $scope.addCharges = function () {
        debugger;
        if ($.isNumeric($('#RevenueTypeID').val()) == false || $('#RevenueTypeID').val() <= 0) {
            alert("Please choose Revenue Type");
            $('#RevenueTypeID').focus();
            return;
        }
        var ChargeObj = {
            InvoiceID: ($scope.InvoiceID == undefined) ? "" : $scope.InvoiceID,
            RevenueTypeID: $('#RevenueTypeID').val() + "",
            RevenueTypeName: $('#RevenueTypeID option:selected').text(),
            SupplierID: ($scope.Supplier2 == undefined) ? "" : $scope.Supplier2,
            SupplierName: $('#Supplier2 option:selected').text() + "",
            Quantity: ($scope.Quantity == undefined) ? "" : $scope.Quantity,
            Description: ($scope.Description == undefined) ? "" : $scope.Description,
            ProvisionCurrency: $('#ProvisionExR option:selected').text(),
            ProvisionCurrencyId: ($scope.ProvisionExR == undefined) ? "" : $scope.ProvisionExR,
            ProvisionRate: ($scope.ProvisionRate == undefined) ? "" : $scope.ProvisionRate,
            //ProvisionCurrencyID: $scope.ProvisionExR,
            ProvisionExchangeRate: ($scope.ProexChangeRate == undefined) ? "" : $scope.ProexChangeRate,
            ProvisionHome: ($scope.ProvisionDomestic == undefined) ? "" : $scope.ProvisionDomestic,
            ProvisionForeign: ($scope.ProvisionForeign == undefined) ? "" : $scope.ProvisionForeign,
            SalesRate: ($scope.SaleRate == undefined) ? "" : $scope.SaleRate,
            SalesCurrencyId: ($scope.SalesExR == undefined) ? "" : $scope.SalesExR,
            SalesCurrencyName: $('#SalesExR option:selected').text(),
            Currency: ($scope.Currency == undefined) ? "" : $scope.Currency,
            // SalesCurrencyID: $scope.SalesCurrency,
            SalesExchangeRate: ($scope.SaleexChangeRate == undefined) ? "" : $scope.SaleexChangeRate,
            SalesHome: ($scope.SalesDomestic == undefined) ? "" : $scope.SalesDomestic,
            SalesForeign: ($scope.SalesForeign == undefined) ? "" : $scope.SalesForeign,
            Cost: ($scope.LocalCur == undefined) ? "" : $scope.LocalCur,
            InvoiceStatus: "1",
            CostUpdationStatus: "1"
        };
        var ChargeStr = JSON.stringify(ChargeObj);
        vCharges.push(ChargeStr);
        var tdString = '<tr><td><div class= "data1" ><input type="text" value="' + ChargeObj.RevenueTypeName + '" name="RevenueTypeName_' + vCharges.length + '" id="RevenueTypeName_' + vCharges.length + '" /><input type="hidden" value="' + ChargeObj.RevenueTypeID + '" name="RevenueTypeID_' + vCharges.length + '" id="RevenueTypeID_' + vCharges.length + '" /></div ></td>';
        tdString = tdString + '<td><div class="data2" ><input type="text" value="' + ChargeObj.SupplierName + '" name="SupplierName_' + vCharges.length + '" id="SupplierName_' + vCharges.length + '" /><input type="hidden" value="' + ChargeObj.SupplierID + '" name="SupplierID_' + vCharges.length + '" id="SupplierID_' + vCharges.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data3" ><input type="text" value="' + ChargeObj.Description + '" name="ChargeDescription_' + vCharges.length + '" id="ChargeDescription_' + vCharges.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data4" ><input type="text" value="' + ChargeObj.Quantity + '" name="Quantity_' + vCharges.length + '" id="Quantity_' + vCharges.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data5" ><input type="text" value="' + ChargeObj.ProvisionRate + '" name="ProvisionRate_' + vCharges.length + '" id="ProvisionRate_' + vCharges.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data5"><input type="text" value="' + ChargeObj.ProvisionCurrency + '" name="ProvisionCurrency_' + vCharges.length + '" id="ProvisionCurrency_' + vCharges.length + '" /><input type="hidden" value="' + ChargeObj.ProvisionCurrencyId + '" name="ProvisionCurrencyId_' + vCharges.length + '" id="ProvisionCurrencyId_' + vCharges.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data6"><input type="text" value="' + ChargeObj.ProvisionExchangeRate + '" name="ProvisionExchangeRate_' + vCharges.length + '" id="ProvisionExchangeRate_' + vCharges.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data7"><input type="text" value="' + ChargeObj.ProvisionHome + '" name="ProvisionHome_' + vCharges.length + '" id="ProvisionHome_' + vCharges.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data8"><input type="text" value="' + ChargeObj.ProvisionForeign + '" name="ProvisionForeign_' + vCharges.length + '" id="ProvisionForeign_' + vCharges.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data8"><input type="text" value="' + ChargeObj.SalesRate + '" name="SalesRate_' + vCharges.length + '" id="SalesRate_' + vCharges.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data9"><input type="text" value="' + ChargeObj.SalesCurrencyName + '" name="SalesCurrencyName_' + vCharges.length + '" id="SalesCurrencyName_' + vCharges.length + '" /><input type="hidden" value="' + ChargeObj.SalesCurrencyId + '" name="SalesCurrencyId_' + vCharges.length + '" id="SalesCurrencyId_' + vCharges.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data10"><input type="text" value="' + ChargeObj.SalesExchangeRate + '" name="SalesExchangeRate_' + vCharges.length + '" id="SalesExchangeRate_' + vCharges.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data11"><input type="text" value="' + ChargeObj.SalesHome + '" name="SalesHome_' + vCharges.length + '" id="SalesHome_' + vCharges.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data12"><input type="text" value="' + ChargeObj.SalesForeign + '" name="SalesForeign_' + vCharges.length + '" id="SalesHome_' + vCharges.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data13"><input type="text" value="' + ChargeObj.Cost + '" name="Cost_' + vCharges.length + '" id="Cost_' + vCharges.length + '" /></div></td>';
        tdString = tdString + '<td><a href="javascript:void(0)" onclick="deleteCharge(this)"><i class="fa fa-times-circle"></i></a></td>';
        tdString = tdString + '</tr>';
        $("#charges_table").append(tdString);
        $("#AddCharges").show();
    };

    $scope.addContainer = function () {


        var Containers = {
            JContainerDetailID: $scope.JContainerDetailID,
            JobID: $scope.JobID,
            ContainerTypeID: $scope.ContainerTypeID,
            ContainerNo: $scope.ContainerNo,
            SealNo: $scope.SealNo,
            Description: $scope.Description,
            UserID: $scope.UserID
        };


        $http({
            url: '/Job/AddContainerDetails/',
            method: 'POST',
            data: JSON.stringify(Charges),
            dataType: "json"
        }).success(function (data, status, headers, config) {
            $scope.JContainerDetailID = 0;
            $scope.JobID = 0;
            $scope.ContainerTypeID = 0;
            $scope.ContainerNo = '';
            $scope.SealNo = '';
            $scope.Description = '';
            $scope.UserID = 0;
            BindContainer();
        });
    };



    function BindCharges() {
        $http({
            url: '/Job/GetChargesByJobIdandUserID/',
            method: 'GET'
        }).success(function (data, status, headers, config) {
            debugger;
            $scope.Charges = data;
            vCharges = JSON.parse('[]');
            for (var i = 0; i < $scope.Charges.length; i++) {
                var ChargeObj = $scope.Charges[i];
                var ChargeStr = JSON.stringify(ChargeObj);
                vCharges.push(ChargeStr);
                var tdString = '<tr><td><div class= "data1" ><input type="text" value="' + ChargeObj.RevenueType + '" name="RevenueTypeName_' + vCharges.length + '" id="RevenueTypeName_' + vCharges.length + '" /><input type="hidden" value="' + ChargeObj.RevenueTypeID + '" name="RevenueTypeID_' + vCharges.length + '" id="RevenueTypeID_' + vCharges.length + '" /></div ></td>';
                tdString = tdString + '<td><div class="data2" ><input type="text" value="' + ChargeObj.SupplierName + '" name="SupplierName_' + vCharges.length + '" id="SupplierName_' + vCharges.length + '" /><input type="hidden" value="' + ChargeObj.SupplierID + '" name="SupplierID_' + vCharges.length + '" id="SupplierID_' + vCharges.length + '" /></div></td>';
                tdString = tdString + '<td><div class="data4" ><input type="text" value="' + ChargeObj.Quantity + '" name="Quantity_' + vCharges.length + '" id="Quantity_' + vCharges.length + '" /></div></td>';
                tdString = tdString + '<td><div class="data5" ><input type="text" value="' + ChargeObj.ProvisionRate + '" name="ProvisionRate_' + vCharges.length + '" id="ProvisionRate_' + vCharges.length + '" /></div></td>';
                tdString = tdString + '<td><div class="data5"><input type="text" value="' + ChargeObj.ProvisionCurrency + '" name="ProvisionCurrency_' + vCharges.length + '" id="ProvisionCurrency_' + vCharges.length + '" /><input type="hidden" value="' + ChargeObj.ProvisionCurrencyId + '" name="ProvisionCurrencyId_' + vCharges.length + '" id="ProvisionCurrencyId_' + vCharges.length + '" /></div></td>';
                tdString = tdString + '<td><div class="data6"><input type="text" value="' + ChargeObj.ProvisionExchangeRate + '" name="ProvisionExchangeRate_' + vCharges.length + '" id="ProvisionExchangeRate_' + vCharges.length + '" /></div></td>';
                tdString = tdString + '<td><div class="data7"><input type="text" value="' + ChargeObj.ProvisionHome + '" name="ProvisionHome_' + vCharges.length + '" id="ProvisionHome_' + vCharges.length + '" /></div></td>';
                tdString = tdString + '<td><div class="data8"><input type="text" value="' + ChargeObj.ProvisionForeign + '" name="ProvisionForeign_' + vCharges.length + '" id="ProvisionForeign_' + vCharges.length + '" /></div></td>';
                tdString = tdString + '<td><div class="data8"><input type="text" value="' + ChargeObj.SalesRate + '" name="SalesRate_' + vCharges.length + '" id="SalesRate_' + vCharges.length + '" /></div></td>';
                tdString = tdString + '<td><div class="data9"><input type="text" value="' + ChargeObj.SalesCurrencyName + '" name="SalesCurrencyName_' + vCharges.length + '" id="SalesCurrencyName_' + vCharges.length + '" /><input type="hidden" value="' + ChargeObj.SalesCurrencyId + '" name="SalesCurrencyId_' + vCharges.length + '" id="SalesCurrencyId_' + vCharges.length + '" /></div></td>';
                tdString = tdString + '<td><div class="data10"><input type="text" value="' + ChargeObj.SalesExchangeRate + '" name="SalesExchangeRate_' + vCharges.length + '" id="SalesExchangeRate_' + vCharges.length + '" /></div></td>';
                tdString = tdString + '<td><div class="data11"><input type="text" value="' + ChargeObj.SalesHome + '" name="SalesHome_' + vCharges.length + '" id="SalesHome_' + vCharges.length + '" /></div></td>';
                tdString = tdString + '<td><div class="data12"><input type="text" value="' + ChargeObj.SalesForeign + '" name="SalesForeign_' + vCharges.length + '" id="SalesHome_' + vCharges.length + '" /></div></td>';
                tdString = tdString + '<td><div class="data13"><input type="text" value="' + ChargeObj.Cost + '" name="Cost_' + vCharges.length + '" id="Cost_' + vCharges.length + '" /></div></td>';
                tdString = tdString + '<td><a href="javascript:void(0)" onclick="deleteCharge(this)"><i class="fa fa-times-circle"></i></a></td>';
                tdString = tdString + '</tr>';
                $("#charges_table").append(tdString);
            }
            $("#AddCharges").show();
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



    var vAuditDet = JSON.parse('[]');
    $scope.addAuditDet = function () {

        var AuditObj = {
            TransDate: ($scope.TransDate == undefined) ? "" : $scope.TransDate,
            Remarks: ($scope.Remarks == undefined) ? "" : $scope.Remarks
        };
        var AuditObjStr = JSON.stringify(AuditObj);
        vAuditDet.push(AuditObjStr);
        var tdString = '<tr><td><div class= "data1" ><input type="text" style="width:125px;" value="' + AuditObj.TransDate + '" name="AuditTransDate_' + vAuditDet.length + '" id="AuditTransDate_' + vAuditDet.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data2" ><input style="width:125px;" type="text" value="' + AuditObj.Remarks + '" name="AuditRemarks_' + vAuditDet.length + '" id="AuditRemarks_' + vAuditDet.length + '" /></div></td>';
        tdString = tdString + '<td><a href="javascript:void(0)" onclick="deleteRow(this)"><i class="fa fa-times-circle"></i></a></td>';
        tdString = tdString + '</tr>';
        $("#audit_table").append(tdString);

    };


    var vBillOfEntry = JSON.parse('[]');
    $scope.addBillOfEntry = function () {
        var BillOfEntryObj = {
            BIllOfEntry: ($scope.BIllOfEntry == undefined) ? "" : $scope.BIllOfEntry,
            BillofEntryDate: ($scope.BillofEntryDate == undefined) ? "" : $scope.BillofEntryDate,
            ShippingAgentID: ($scope.ShippingAgentID == undefined) ? "" : $scope.ShippingAgentID,
            ShippingAgentName: $('#ShippingAgentID option:selected').text()
        };
        var BillOfEntryStr = JSON.stringify(BillOfEntryObj);
        vBillOfEntry.push(BillOfEntryStr);
        var tdString = '<tr><td><div class= "data1" ><input type="text" style="width:125px;" value="' + BillOfEntryObj.BIllOfEntry + '" name="BIllOfEntry_' + vBillOfEntry.length + '" id="BIllOfEntry_' + vBillOfEntry.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data2" ><input style="width:125px;" type="text" value="' + BillOfEntryObj.BillofEntryDate + '" name="BillofEntryDate_' + vBillOfEntry.length + '" id="BillofEntryDate_' + vBillOfEntry.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data3"><input type="text" value="' + BillOfEntryObj.ShippingAgentName + '" name="ShippingAgentName_' + vBillOfEntry.length + '" id="ShippingAgentName_' + vBillOfEntry.length + '" /><input type="hidden" value="' + BillOfEntryObj.ShippingAgentID + '" name="ShippingAgentID_' + vBillOfEntry.length + '" id="ShippingAgentID_' + vBillOfEntry.length + '" /></div></td>';
        tdString = tdString + '<td><a href="javascript:void(0)" onclick="deleteRow(this)"><i class="fa fa-times-circle"></i></a></td>';
        tdString = tdString + '</tr>';
        $("#bill_of_entry_table").append(tdString);
    };

    var vContainer = JSON.parse('[]');
    $scope.addContainerDetails = function () {
        var ContainerObj = {
            ContainerTypeID: ($scope.ContainerTypeID == undefined) ? "" : $scope.ContainerTypeID,
            ContainerTypeName: $('#ContainerTypeID option:selected').text(),
            ContainerNo: ($scope.ContainerNo == undefined) ? "" : $scope.ContainerNo,
            SealNo: ($scope.SealNo == undefined) ? "" : $scope.SealNo,
            Description: ($scope.ContainerDescription == undefined) ? "" : $scope.ContainerDescription
        };


        var ContainerStr = JSON.stringify(ContainerObj);
        vContainer.push(ContainerStr);
        var tdString = '<tr><td><div class= "data1" ><input type="text" value="' + ContainerObj.ContainerTypeName + '" name="ContainerType_' + vContainer.length + '" id="ContainerType_' + vContainer.length + '" /><input type="hidden" value="' + ContainerObj.ContainerTypeID + '" name="ContainerTypeID_' + vContainer.length + '" id="ContainerTypeID_' + vContainer.length + '" /></div ></td>';
        tdString = tdString + '<td><div class="data2" ><input type="text" style="width:60px;" value="' + ContainerObj.ContainerNo + '" name="ContainerNo_' + vContainer.length + '" id="ContainerNo_' + vContainer.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data3"><input type="text" style="width:60px;" value="' + ContainerObj.SealNo + '" name="SealNo_' + vContainer.length + '" id="SealNo_' + vContainer.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data" ><input type="text" value="' + ContainerObj.Description + '" name="ContainerDescription_' + vContainer.length + '" id="ContainerDescription_' + vContainer.length + '" /></div></td>';
        tdString = tdString + '<td><a href="javascript:void(0)" onclick="deleteRow(this)"><i class="fa fa-times-circle"></i></a></td>';
        tdString = tdString + '</tr>';
        $("#container_table").append(tdString);
        //   $scope.Charges = vCharges;
        //  var test = JSON.stringify(vCharges);
        $("#AddContainer").show();
    };

    var vCargoDesc = JSON.parse('[]');
    $scope.addCargoDesc = function () {
        var CargoDescObj = {
            Mark: ($scope.Mark == undefined) ? "" : $scope.Mark,
            Description: ($scope.CarDescription == undefined) ? "" : $scope.CarDescription,
            weight: ($scope.weight == undefined) ? "" : $scope.weight,
            volume: ($scope.volume == undefined) ? "" : $scope.volume,
            Packages: ($scope.Packages == undefined) ? "" : $scope.Packages,
            GrossWeight: ($scope.GrossWeight == undefined) ? "" : $scope.GrossWeight
        };

        var CargoDescStr = JSON.stringify(CargoDescObj);
        vCargoDesc.push(CargoDescStr);
        var tdString = '<tr><td><div class= "data1" ><input type="text" style="width:70px;" value="' + CargoDescObj.Mark + '" name="Mark_' + vCargoDesc.length + '" id="Mark_' + vCargoDesc.length + '" /></div ></td>';
        tdString = tdString + '<td><div class="data2" ><input type="text" style="width:180px;" value="' + CargoDescObj.Description + '" name="CarDescription_' + vCargoDesc.length + '" id="CarDescription_' + vCargoDesc.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data3" ><input type="text" style="width:70px;" value="' + CargoDescObj.weight + '" name="weight_' + vCargoDesc.length + '" id="weight_' + vCargoDesc.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data5"><input type="text" style="width:70px;" value="' + CargoDescObj.volume + '" name="volume_' + vCargoDesc.length + '" id="volume_' + vCargoDesc.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data6"><input type="text" style="width:70px;" value="' + CargoDescObj.Packages + '" name="Packages_' + vCargoDesc.length + '" id="Packages_' + vCargoDesc.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data6"><input type="text" style="width:70px;" value="' + CargoDescObj.GrossWeight + '" name="GrossWeight_' + vCargoDesc.length + '" id="GrossWeight_' + vCargoDesc.length + '" /></div></td>';
        tdString = tdString + '<td><a href="javascript:void(0)" onclick="deleteRow(this)"><i class="fa fa-times-circle"></i></a></td>';
        tdString = tdString + '</tr>';
        $("#cargo_table").append(tdString);

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

    function BindContainerType() {


        $http({
            url: '/Job/GetContainerTypeList/',
            method: 'GET'
        }).success(function (data, status, headers, config) {

            $scope.ContainerTypeList = data;

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


