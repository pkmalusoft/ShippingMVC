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
    debugger;
    var month_names_short = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    BindCharges();
    BindCargoDes();
    BindAudits();
    BindBills();
    BindContainer();
    BindRevenueType();
    BindProvisionCurrency();
    BindContainerType();
    BindUnit();

    function isValidDate(dateWrapper) {
        if (typeof dateWrapper.getMonth === 'function') {
            return true;
        } else {
            return false;
        }
    }

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
        debugger;
        $http({
            url: '/Job/GetExchangeRte/' + $scope.ProvisionExR,
            method: 'GET'
        }).success(function (data, status, headers, config) {

            $scope.ProexChangeRate = data;

            if ($scope.ProexChangeRate !== '') {

                $scope.ProvisionDomestic = $scope.ProexChangeRate * $scope.ProvisionForeign;

               // $scope.LocalCur = $scope.SalesDomestic - $scope.ProvisionDomestic;
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

            if ($scope.SaleexChangeRate !== '') {

                $scope.SalesForeign = parseFloat($scope.SaleexChangeRate * $scope.SalesDomestic).toFixed(2);

            //    $scope.LocalCur = $scope.SalesDomestic - $scope.ProvisionDomestic;
            }
            else {
                $scope.SaleexChangeRate = '0.00';
            }

        });
    };

    $scope.CalcProvForeign = function () {
        // alert($scope.ExR);
        if ($scope.ProvisionRate !== '') {
            $scope.ProvisionForeign = $scope.Quantity * $scope.ProvisionRate;

            if ($scope.ProexChangeRate != '' && $scope.ProvisionForeign != '') {
                $scope.ProvisionDomestic = $scope.ProexChangeRate * $scope.ProvisionForeign;

            //    $scope.LocalCur = $scope.SalesDomestic - $scope.ProvisionDomestic;
            }
        }
        else {
            $scope.ProvisionForeign = '0.00';
        }
    };

    $scope.CalcSaleForeign = function () {
        // alert($scope.ExR);

        $scope.SalesDomestic = parseFloat($scope.SaleRate).toFixed(2);
        if ($scope.SaleRate !== '') {
            var res = $scope.Quantity * $scope.SaleRate;
            res = parseFloat(res).toFixed(2);
            $scope.SalesForeign = res;
          //  if ($scope.SaleexChangeRate != '' && $scope.SalesForeign !== '') {
         //       var res1 = $scope.SaleexChangeRate * $scope.SalesForeign;
         //       res1 = parseFloat(res1).toFixed(2);
          //     $scope.SalesForeign = res1;
         //   }
        }
        else {
            $scope.SalesForeign = '0.00';
        }
    };

    var vCharges = JSON.parse('[]');

    $scope.editCharges = function (index) {
        debugger;
        var objRevenueTypeID = $('#RevenueTypeID_' + index).val();
        $('#RevenueTypeID').val(objRevenueTypeID);
        $('#RevenueTypeID').trigger("change");
       
        $('#QTY').val($('#Quantity_' + index).val());
        $('#UnitID').val($('#ItemUnitID_' + index).val());
        $('#ProRate').val($('#ProvisionRate_' + index).val());
        $('#ProvisionExR').val($('#ProvisionCurrencyId_' + index).val());
        $('#ProvisionExRate').val($('#ProvisionExchangeRate_' + index).val());
        $('#ProvisiDomestic').val($('#ProvisionHome_' + index).val());
        $('#ProvisionForeign').val($('#ProvisionForeign_' + index).val());
        $('#margin').val($('#margin_' + index).val());
        $('#tax').val($('#tax_' + index).val());
        $('#taxamt').val($('#taxamt_' + index).val());
        $('#SaleRate').val($('#SalesRate_' + index).val());
        $('#SalesExR').val($('#SalesCurrencyId_' + index).val());
        $('#SalesExRate').val($('#SalesExchangeRate_' + index).val());
        $('#SalesDomestic').val($('#SalesHome_' + index).val());
        $('#SalesForeign').val($('#SalesForeign_' + index).val());
        $('#charges_update_index').val(index);
        $('#update_charges').show();
        $('#add_charges').hide();
        $('#cancel_edit_charges').show();
        $('#Supplier2').val($('#SupplierID_' + index).val());
    };

    $scope.cancelEditCharges = function () {
        $('#charges_update_index').val('0');
        $('#update_charges').hide();
        $('#add_charges').show();
        $('#cancel_edit_charges').hide();
        ClearChargesTab();
    };

    $scope.updateCharges = function () {
        debugger;
        if ($.isNumeric($('#RevenueTypeID').val()) === false || $('#RevenueTypeID').val() <= 0) {
            alert("Please choose Revenue Type");
            $('#RevenueTypeID').focus();
            return;
        }
        var ChargeObj = {
            InvoiceID: ($scope.InvoiceID === undefined) ? "" : $scope.InvoiceID,
            RevenueTypeID: $('#RevenueTypeID').val() + "",
            RevenueType: $('#RevenueTypeID option:selected').text(),
            RevenueTaxPercentage: $('#RevenueTypeID option:selected').attr('tp'),
            SupplierID: ($scope.Supplier2 === undefined) ? "" : $scope.Supplier2,
            SupplierName: $('#Supplier2 option:selected').text() + "",
            Quantity: ($scope.Quantity === undefined) ? "" : $scope.Quantity,
            ItemUnit: $('#UnitID option:selected').text(),
            ItemUnitID: $('#UnitID').val(),
            Description: ($scope.Description === undefined) ? "" : $scope.Description,
            ProvisionCurrency: $('#ProvisionExR option:selected').text(),
            ProvisionCurrencyId: ($scope.ProvisionExR === undefined) ? "" : $scope.ProvisionExR,
            ProvisionRate: $('#ProRate').val(),
            //ProvisionCurrencyID: $scope.ProvisionExR,
            ProvisionExchangeRate: ($scope.ProexChangeRate === undefined) ? "" : $scope.ProexChangeRate,
            ProvisionHome: $('#ProvisiDomestic').val(),
            ProvisionForeign: $('#ProvisionForeign').val(),
            Margin: $('#margin').val(),
            Tax: $('#tax').val(),
            TaxAmount: $('#taxamt').val(),
            SalesRate: $('#SaleRate').val(),
            SalesCurrencyID: ($scope.SalesExR === undefined) ? "" : $scope.SalesExR,
            SalesCurrency: $('#SalesExR option:selected').text(),
            Currency: ($scope.Currency === undefined) ? "" : $scope.Currency,
            // SalesCurrencyID: $scope.SalesCurrency,
            SalesExchangeRate: ($scope.SaleexChangeRate === undefined) ? "" : $scope.SaleexChangeRate,
            SalesHome: ($scope.SalesDomestic === undefined) ? "" : $scope.SalesDomestic,
            SalesForeign: $('#SalesForeign').val(),
            InvoiceStatus: "1",
            CostUpdationStatus: "1"
        };
        var index = $('#charges_update_index').val();
        $('#RevenueTypeName_' + index).val($('#RevenueTypeID option:selected').text());
        $('#RevenueTypeID_' + index).val($('#RevenueTypeID').val());
        $('#SupplierName_' + index).val($('#Supplier2 option:selected').text() + "");
        $('#SupplierID_' + index).val($('#Supplier2').val());
        $('#Quantity_' + index).val($('#QTY').val());
        $('#ItemUnit_' + index).val($('#UnitID option:selected').text());
        $('#ItemUnitID_' + index).val($('#UnitID').val());
        $('#ProvisionRate_' + index).val($('#ProRate').val());
        $('#ProvisionCurrency_' + index).val($('#ProvisionExR option:selected').text());
        $('#ProvisionCurrencyId_' + index).val($('#ProvisionExR').val());

        $('#ProvisionExchangeRate_' + index).val($('#ProvisionExRate').val());
        $('#ProvisionHome_' + index).val($('#ProvisiDomestic').val());
        $('#ProvisionForeign_' + index).val($('#ProvisionForeign').val());
        $('#margin_' + index).val($('#margin').val());
        $('#tax_' + index).val($('#tax').val());
        $('#taxamt_' + index).val($('#taxamt').val());
        $('#SalesRate_' + index).val($('#SaleRate').val());
        $('#SalesCurrencyName_' + index).val($('#SalesExR option:selected').text());
        $('#SalesCurrencyId_' + index).val($('#SalesExR').val());
        $('#SalesExchangeRate_' + index).val($('#SalesExRate').val());
        $('#SalesHome_' + index).val($('#SalesDomestic').val());
        $('#SalesForeign_' + index).val($('#SalesForeign').val());
        $('#charges_update_index').val('0');
        $('#update_charges').hide();
        $('#add_charges').show();
        $('#cancel_edit_charges').hide();
        ClearChargesTab();
    };

    $scope.addCharges = function () {
        debugger;
        if ($.isNumeric($('#RevenueTypeID').val()) === false || $('#RevenueTypeID').val() <= 0) {
            alert("Please choose Revenue Type");
            $('#RevenueTypeID').focus();
            return;
        }
        var ChargeObj = {
            InvoiceID: ($scope.InvoiceID === undefined) ? "" : $scope.InvoiceID,
            RevenueTypeID: $('#RevenueTypeID').val() + "",
            RevenueType: $('#RevenueTypeID option:selected').text(),
            RevenueTaxPercentage: $('#RevenueTypeID option:selected').attr('tp'),
            SupplierID: ($scope.Supplier2 === undefined) ? "" : $scope.Supplier2,
            SupplierName: $('#Supplier2 option:selected').text() + "",
            Quantity: ($scope.Quantity === undefined) ? "" : $scope.Quantity,
            ItemUnit: $('#UnitID option:selected').text(),
            ItemUnitID: $('#UnitID').val(),
            Description: ($scope.Description === undefined) ? "" : $scope.Description,
            ProvisionCurrency: $('#ProvisionExR option:selected').text(),
            ProvisionCurrencyId: ($scope.ProvisionExR === undefined) ? "" : $scope.ProvisionExR,
            ProvisionRate: $('#ProRate').val(),
            //ProvisionCurrencyID: $scope.ProvisionExR,
            ProvisionExchangeRate: ($scope.ProexChangeRate === undefined) ? "" : $scope.ProexChangeRate,
            ProvisionHome: $('#ProvisiDomestic').val(),
            ProvisionForeign: $('#ProvisionForeign').val(),
            Margin: $('#margin').val(),
            Tax: $('#tax').val(),
            TaxAmount: $('#taxamt').val(),
            SalesRate: $('#SaleRate').val(),
            SalesCurrencyID: ($scope.SalesExR === undefined) ? "" : $scope.SalesExR,
            SalesCurrency: $('#SalesExR option:selected').text(),
            Currency: ($scope.Currency === undefined) ? "" : $scope.Currency,
            // SalesCurrencyID: $scope.SalesCurrency,
            SalesExchangeRate: ($scope.SaleexChangeRate === undefined) ? "" : $scope.SaleexChangeRate,
            SalesHome: ($scope.SalesDomestic === undefined) ? "" : $scope.SalesDomestic,
            SalesForeign: $('#SalesForeign').val(),
            InvoiceStatus: "1",
            CostUpdationStatus: "1"
        };
        var ChargeStr = JSON.stringify(ChargeObj);
        vCharges.push(ChargeStr);
        var tdString = '<tr><td style="width:20%"><div class= "data1" ><input type="text" value="' + ChargeObj.RevenueType + '" title="' + ChargeObj.RevenueType + '" name="RevenueTypeName_' + vCharges.length + '" id="RevenueTypeName_' + vCharges.length + '" style="width:90%;" readonly /><input type="hidden" value="' + ChargeObj.RevenueTypeID + '" name="RevenueTypeID_' + vCharges.length + '" id="RevenueTypeID_' + vCharges.length + '" /><input type="hidden" value="' + ChargeObj.RevenueTaxPercentage + '" name="RevenueTaxPercentage_' + vCharges.length + '" id="RevenueTaxPercentage_' + vCharges.length + '" /></div ></td>';
        tdString = tdString + '<td style="width:20%;><div class="data2" ><input type="text" value="' + ChargeObj.SupplierName + '" title="' + ChargeObj.SupplierName + '" name="SupplierName_' + vCharges.length + '" id="SupplierName_' + vCharges.length + '" style="width:90%;"  readonly/><input type="hidden" value="' + ChargeObj.SupplierID + '" name="SupplierID_' + vCharges.length + '" id="SupplierID_' + vCharges.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data4" ><input type="text" value="' + ChargeObj.Quantity + '" title="' + ChargeObj.Quantity + '" name="Quantity_' + vCharges.length + '" id="Quantity_' + vCharges.length + '" style="width:30px;" readonly/></div></td>';
        tdString = tdString + '<td><div class="data2" ><input type="text" value="' + ChargeObj.ItemUnit + '" title="' + ChargeObj.ItemUnit + '" name="ItemUnit_' + vCharges.length + '" id="ItemUnit_' + vCharges.length + '" style="width:30px;" readonly /><input type="hidden" value="' + ChargeObj.ItemUnitID + '" name="ItemUnitID_' + vCharges.length + '" id="ItemUnitID_' + vCharges.length + '" /></div></td>';
        tdString = tdString + '<td class="hideinsummary smallwindowwidth" id="provin_rate"><div class="data5" ><input type="text" value="' + ChargeObj.ProvisionRate + '" title="' + ChargeObj.ProvisionRate + '" name="ProvisionRate_' + vCharges.length + '" id="ProvisionRate_' + vCharges.length + '" style="width:50px; text-align:right" readonly/></div></td>';
        tdString = tdString + '<td class="hideinsummary smallwindowwidth" id="provin_curr"><div class="data5"><input type="text" value="' + ChargeObj.ProvisionCurrency + '" title="' + ChargeObj.ProvisionCurrency + '" name="ProvisionCurrency_' + vCharges.length + '" id="ProvisionCurrency_' + vCharges.length + '" style="width:50px; text-align:right" /><input type="hidden" value="' + ChargeObj.ProvisionCurrencyId + '" name="ProvisionCurrencyId_' + vCharges.length + '" id="ProvisionCurrencyId_' + vCharges.length + '" readonly /></div></td>';
        tdString = tdString + '<td class="hideinsummary smallwindowwidth" id="provinex_rate"><div class="data6"><input type="text" value="' + ChargeObj.ProvisionExchangeRate + '" title="' + ChargeObj.ProvisionExchangeRate + '" name="ProvisionExchangeRate_' + vCharges.length + '" id="ProvisionExchangeRate_' + vCharges.length + '" style="width:50px; text-align:right" /></div></td>';
        tdString = tdString + '<td class="data3"><div class="data7"><input type="text" value="' + ChargeObj.ProvisionHome + '" title="' + ChargeObj.ProvisionHome + '" name="ProvisionHome_' + vCharges.length + '" id="ProvisionHome_' + vCharges.length + '" style="width:50px; text-align:right" readonly /></div></td>';
        tdString = tdString + '<td id="provin_forgin" class="hideinsummary smallwindowwidth"><div class="data8"><input type="text" value="' + ChargeObj.ProvisionForeign + '" title="' + ChargeObj.ProvisionForeign + '" name="ProvisionForeign_' + vCharges.length + '" id="ProvisionForeign_' + vCharges.length + '" style="width:50px; text-align:right" readonly /></div></td>';
        tdString = tdString + '<td id="sale_rate" class="hideinsummary smallwindowwidth"><div class="data8"><input type="text" value="' + ChargeObj.SalesRate + '" title="' + ChargeObj.SalesRate + '" name="SalesRate_' + vCharges.length + '" id="SalesRate_' + vCharges.length + '" style="width:50px; text-align:right" readonly /></div></td>';
        tdString = tdString + '<td id="sale_curr" class="hideinsummary smallwindowwidth"><div class="data9"><input type="text" value="' + ChargeObj.SalesCurrency + '" title="' + ChargeObj.SalesCurrency + '" name="SalesCurrencyName_' + vCharges.length + '" id="SalesCurrencyName_' + vCharges.length + '" style="width:50px;" readonly /><input type="hidden" value="' + ChargeObj.SalesCurrencyID + '" name="SalesCurrencyId_' + vCharges.length + '" id="SalesCurrencyId_' + vCharges.length + '" /></div></td>';
        tdString = tdString + '<td id="sale_exrate" class="hideinsummary smallwindowwidth"><div class="data10"><input type="text" value="' + ChargeObj.SalesExchangeRate + '" title="' + ChargeObj.SalesExchangeRate + '" name="SalesExchangeRate_' + vCharges.length + '" id="SalesExchangeRate_' + vCharges.length + '" style="width:50px; text-align:right" readonly /></div></td>';
        tdString = tdString + '<td ><div class="data11"><input type="text" value="' + ChargeObj.SalesHome + '" title="' + ChargeObj.SalesHome + '" name="SalesHome_' + vCharges.length + '" id="SalesHome_' + vCharges.length + '" style="width:50px; text-align:right" readonly /></div></td>';
        tdString = tdString + '<td id="sale_for"><div class="data12"><input type="text" value="' + ChargeObj.SalesForeign + '" title="' + ChargeObj.SalesForeign + '" name="SalesForeign_' + vCharges.length + '" id="SalesForeign_' + vCharges.length + '" style="width:50px; text-align:right" readonly /></div></td>';
       // tdString = tdString + '<td><div class="data13"><input type="text" value="' + ChargeObj.Cost + '" title="' + ChargeObj.Cost +'" name="Cost_' + vCharges.length + '" id="Cost_' + vCharges.length + '" style="width:70px;" /></div></td>';
        tdString = tdString + '<td><div class="data13"><input type="text" value="' + ChargeObj.Tax + '" title="' + ChargeObj.Tax + '" name="tax_' + vCharges.length + '" id="tax_' + vCharges.length + '" style="width:30px; text-align:right" readonly /></div></td>';
        tdString = tdString + '<td><div class="data14"><input type="text" value="' + ChargeObj.TaxAmount + '" title="' + ChargeObj.TaxAmount + '" name="taxamt_' + vCharges.length + '" id="taxamt_' + vCharges.length + '" style="width:50px; text-align:right" readonly /></div></td>';
        tdString = tdString + '<td><div class="data15"><input type="text" value="' + ChargeObj.Margin + '" title="' + ChargeObj.Margin + '" name="margin_' + vCharges.length + '" id="margin_' + vCharges.length + '" style="width:50px; text-align:right" readonly /></div></td>';
        tdString = tdString + '<td><a href="javascript:void(0)" onclick="angular.element(this).scope().editCharges(' + vCharges.length + ')"><i class="fa fa-pencil"></i></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:void(0)" onclick="deleteCharge(this)"><i class="fa fa-times-circle"></i></a></td>';
        tdString = tdString + '</tr>';
        $("#charges_table").append(tdString);
        $("#AddCharges").show();
        ClearChargesTab();

    };

    function ClearChargesTab() {
        $scope.InvoiceID = '';
        $('#RevenueTypeID').val('');
        $('#Supplier2').val('');
        $('#QTY').val(''),
        $scope.Supplier2 = '';
        $scope.Quantity = '';
        $('#UnitID').val('');
        $scope.Description = '';
        $('#ProvisionExR').val('');
        $('#ProvisionExRate').val('');
        $('#ProRate').val('');
        $scope.ProvisionExR = ''; $scope.ProexChangeRate = '';
        $('#ProvisiDomestic').val('');
        $('#ProvisionForeign').val('');
        Margin: $('#margin').val('');
        $('#tax').val('');
        $('#taxamt').val('');
        $('#SaleRate').val('');
        $scope.SalesExR = '';
        $('#SalesExR').val('');
        $('#SalesExRate').val('');
        $scope.Currency = '';

        $scope.SaleexChangeRate = '';
        $scope.SalesDomestic = '';
        $('#SalesForeign').val('');
    }

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
                var tdString = '<tr><td style="width:20%"><div class= "data1" ><input type="text" value="' + ChargeObj.RevenueType + '" title="' + ChargeObj.RevenueType + '" name="RevenueTypeName_' + vCharges.length + '" id="RevenueTypeName_' + vCharges.length + '" style="width:90%;"readonly /><input type="hidden" value="' + ChargeObj.RevenueTypeID + '" name="RevenueTypeID_' + vCharges.length + '" id="RevenueTypeID_' + vCharges.length + '" /></div ></td>';
                tdString = tdString + '<td style="width:20%"><div class="data2" ><input type="text" value="' + ChargeObj.SupplierName + '" title="' + ChargeObj.SupplierName + '" name="SupplierName_' + vCharges.length + '" id="SupplierName_' + vCharges.length + '" style="width:90%;" readonly/><input type="hidden" value="' + ChargeObj.SupplierID + '" name="SupplierID_' + vCharges.length + '" id="SupplierID_' + vCharges.length + '" /></div></td>';
                tdString = tdString + '<td><div class="data4" ><input type="text" value="' + ChargeObj.Quantity + '" title="' + ChargeObj.Quantity + '" name="Quantity_' + vCharges.length + '" id="Quantity_' + vCharges.length + '" style="width:30px;" readonly /></div></td>';
                tdString = tdString + '<td><div class="data2" ><input type="text" value="' + (ChargeObj.ItemUnit == null ? '' : ChargeObj.ItemUnit) + '" title="' + (ChargeObj.ItemUnit == null ? '' : ChargeObj.ItemUnit) + '" name="ItemUnit_' + vCharges.length + '" id="ItemUnit_' + vCharges.length + '" style="width:30px;" readonly /><input type="hidden" value="' + ChargeObj.ItemUnitID + '" name="ItemUnitID_' + vCharges.length + '" id="ItemUnitID_' + vCharges.length + '" /></div></td>';
                tdString = tdString + '<td class="hideinsummary smallwindowwidth" id="provin_rate"><div class="data5" ><input type="text" value="' + ChargeObj.ProvisionRate + '" title="' + ChargeObj.ProvisionRate + '" name="ProvisionRate_' + vCharges.length + '" id="ProvisionRate_' + vCharges.length + '" style="width:50px; text-align:right"readonly /></div></td>';
                tdString = tdString + '<td class="hideinsummary smallwindowwidth" id="provin_curr"><div class="data5"><input type="text" value="' + ChargeObj.ProvisionCurrency + '" title="' + ChargeObj.ProvisionCurrency + '" name="ProvisionCurrency_' + vCharges.length + '" id="ProvisionCurrency_' + vCharges.length + '" style="width:50px; text-align:right" readonly /><input type="hidden" value="' + ChargeObj.ProvisionCurrencyId + '" name="ProvisionCurrencyId_' + vCharges.length + '" id="ProvisionCurrencyId_' + vCharges.length + '" /></div></td>';
                tdString = tdString + '<td class="hideinsummary smallwindowwidth" id="provinex_rate"><div class="data6"><input type="text" value="' + ChargeObj.ProvisionExchangeRate + '" title="' + ChargeObj.ProvisionExchangeRate + '" name="ProvisionExchangeRate_' + vCharges.length + '" id="ProvisionExchangeRate_' + vCharges.length + '" style="width:50px; text-align:right" readonly/></div></td>';
                tdString = tdString + '<td  class="data3"><div class="data7"><input type="text" value="' + ChargeObj.ProvisionHome + '" title="' + ChargeObj.ProvisionHome + '" name="ProvisionHome_' + vCharges.length + '" id="ProvisionHome_' + vCharges.length + '" style="width:70px; text-align:right" readonly /></div></td>';
                tdString = tdString + '<td id="provin_forgin" class="hideinsummary smallwindowwidth"><div class="data8"><input type="text" value="' + ChargeObj.ProvisionForeign + '" title="' + ChargeObj.ProvisionForeign + '" name="ProvisionForeign_' + vCharges.length + '" id="ProvisionForeign_' + vCharges.length + '" style="width:50px; text-align:right" readonly/></div></td>';
                tdString = tdString + '<td id="sale_rate" class="hideinsummary smallwindowwidth"><div class="data8"><input type="text" value="' + ChargeObj.SalesRate + '" title="' + ChargeObj.SalesRate + '" name="SalesRate_' + vCharges.length + '" id="SalesRate_' + vCharges.length + '" style="width:70px; text-align:right " readonly /></div></td>';
                tdString = tdString + '<td id="sale_curr" class="hideinsummary smallwindowwidth"><div class="data9"><input type="text" value="' + ChargeObj.SalesCurrency + '" title="' + ChargeObj.SalesCurrency + '" name="SalesCurrencyName_' + vCharges.length + '" id="SalesCurrencyName_' + vCharges.length + '" style="width:50px; text-align:right" readonly/><input type="hidden" value="' + ChargeObj.SalesCurrencyID + '" name="SalesCurrencyId_' + vCharges.length + '" id="SalesCurrencyId_' + vCharges.length + '" /></div></td>';
                tdString = tdString + '<td id="sale_exrate" class="hideinsummary smallwindowwidth"><div class="data10"><input type="text" value="' + ChargeObj.SalesExchangeRate + '" title="' + ChargeObj.SalesExchangeRate + '" name="SalesExchangeRate_' + vCharges.length + '" id="SalesExchangeRate_' + vCharges.length + '" style="width:50px; text-align:right" readonly/></div></td>';
                tdString = tdString + '<td><div class="data11"><input type="text" value="' + ChargeObj.SalesHome + '" title="' + ChargeObj.SalesHome + '" name="SalesHome_' + vCharges.length + '" id="SalesHome_' + vCharges.length + '" style="width:60px;text-align:right"readonly /></div></td>';
                tdString = tdString + '<td id="sale_for"><div class="data12"><input type="text" value="' + ChargeObj.SalesForeign + '" title="' + ChargeObj.SalesForeign + '" name="SalesForeign_' + vCharges.length + '" id="SalesHome_' + vCharges.length + '" style="width:50px;text-align:right" /></div></td>';
              //  tdString = tdString + '<td><div class="data13"><input type="text" value="' + ChargeObj.Cost + '" title="' + ChargeObj.Cost + '" name="Cost_' + vCharges.length + '" id="Cost_' + vCharges.length + '" style="width:70px;" /></div></td>';
                tdString = tdString + '<td><div class="data13"><input type="text" value="' + ChargeObj.Tax + '" title="' + ChargeObj.Tax + '" name="tax_' + vCharges.length + '" id="tax_' + vCharges.length + '" style="width:30px; text-align:right" readonly/></div></td>';
                tdString = tdString + '<td><div class="data14"><input type="text" value="' + ChargeObj.TaxAmount + '" title="' + ChargeObj.TaxAmount + '" name="taxamt_' + vCharges.length + '" id="taxamt_' + vCharges.length + '" style="width:50px; text-align:right" readonly/></div></td>';
                tdString = tdString + '<td><div class="data15"><input type="text" value="' + ChargeObj.Margin + '" title="' + ChargeObj.Margin + '" name="margin_' + vCharges.length + '" id="margin_' + vCharges.length + '" style="width:50px; text-align:right" readonly /></div></td>';
                tdString = tdString + '<td><a href="javascript:void(0)" onclick="angular.element(this).scope().editCharges(' + vCharges.length + ')"><i class="fa fa-pencil"></i></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:void(0)" onclick="deleteCharge(this)"><i class="fa fa-times-circle"></i></a></td>';
                tdString = tdString + '</tr>';
                $("#charges_table").append(tdString);
            }
            $("#AddCharges").show();
        });
    };

    var vAuditDet = JSON.parse('[]');
    function BindAudits() {
        $http({
            url: '/Job/GetAuditByJobIdandUserID/',
            method: 'GET'
        }).success(function (data, status, headers, config) {
            $scope.Auditing = data;
            vAuditDet = JSON.parse('[]');
            for (var i = 0; i < $scope.Auditing.length; i++) {
                var AuditObj = $scope.Auditing[i];
                var TransDate = new Date(parseInt(AuditObj.TransDate.substr(6)));
                var TransDateStr = '';
                if (isValidDate(TransDate)) {
                    TransDateStr = TransDate.getDate() + '-' + month_names_short[TransDate.getMonth()] + '-' + TransDate.getFullYear();
                }
                var AuditObjStr = JSON.stringify(AuditObj);
                vAuditDet.push(AuditObjStr);
                var tdString = '<tr><td><div class= "data1" ><input type="text" style="width:100%;" value="' + TransDateStr + '" title="' + TransDateStr + '" name="AuditTransDate_' + vAuditDet.length + '" id="AuditTransDate_' + vAuditDet.length + '" /></div></td>';
                tdString = tdString + '<td><div class="data2" ><input style="width:100%;" type="text" value="' + AuditObj.Remarks + '" title="' + AuditObj.Remarks + '" name="AuditRemarks_' + vAuditDet.length + '" id="AuditRemarks_' + vAuditDet.length + '" /></div></td>';
                tdString = tdString + '<td><a href="javascript:void(0)" onclick="angular.element(this).scope().editNotification(' + vAuditDet.length + ')"><i class="fa fa-pencil"></i></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:void(0)" onclick="deleteRow(this)"><i class="fa fa-times-circle"></i></a></td>';
                tdString = tdString + '</tr>';
                debugger;
                $("#audit_table").append(tdString);
            }
        });
    };

    $scope.addAuditDet = function () {
        debugger;
        var AuditObj = {
            TransDate: ($scope.TransDate == undefined) ? "" : $scope.TransDate,
            Remarks: ($scope.Remarks == undefined) ? "" : $scope.Remarks
        };
        debugger;
        var AuditObjStr = JSON.stringify(AuditObj);
        vAuditDet.push(AuditObjStr);
        var tdString = '<tr><td><div class= "data1" ><input type="text" style="width:100%;" value="' + AuditObj.TransDate + '" name="AuditTransDate_' + vAuditDet.length + '" id="AuditTransDate_' + vAuditDet.length + '" /></div></td>';
        tdString = tdString + '<td><div class="data2" ><input style="width:100%;" type="text" value="' + AuditObj.Remarks + '" name="AuditRemarks_' + vAuditDet.length + '" id="AuditRemarks_' + vAuditDet.length + '" /></div></td>';
        tdString = tdString + '<td><a href="javascript:void(0)" onclick="angular.element(this).scope().editNotification(' + vAuditDet.length + ')"><i class="fa fa-pencil"></i></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:void(0)" onclick="deleteRow(this)"><i class="fa fa-times-circle"></i></a></td>';
        tdString = tdString + '</tr>';
        debugger;
        $("#audit_table").append(tdString);
        debugger;
        $scope.TransDate = '';
        $scope.Remarks = '';
    };

    $scope.editNotification = function (index) {
        $('#notification_update_index').val(index);
        $('#TransDate').val($('#AuditTransDate_' + index).val());
        $('#NotificationRemarks').val($('#AuditRemarks_' + index).val());
        $('#update_notification').show();
        $('#add_notification').hide();
        $('#cancel_edit_notification').show();
    };

    $scope.updateNotification = function () {
        var index = $('#notification_update_index').val();
        $('#AuditTransDate_' + index).val($('#TransDate').val());
        $('#AuditRemarks_' + index).val($('#NotificationRemarks').val());
        $('#update_notification').hide();
        $('#add_notification').show();
        $('#cancel_edit_notification').hide();
        $('#notification_update_index').val('0');
        ClearNotificationTab();
    };

    $scope.cancelNotification = function () {
        $('#update_notification').hide();
        $('#add_notification').show();
        $('#cancel_edit_notification').hide();
        $('#notification_update_index').val('0');
        ClearNotificationTab();
    };

    function ClearNotificationTab() {
        $('#TransDate').val('');
        $('#NotificationRemarks').val('');
    }

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
        var tdString = '<tr><td><div class= "data1" ><input type="text" style="width:100%;" value="' + BillOfEntryObj.BIllOfEntry + '" name="BIllOfEntry_' + vBillOfEntry.length + '" id="BIllOfEntry_' + vBillOfEntry.length + '" readonly/></div></td>';
        tdString = tdString + '<td><div class="data2" ><input style="width:100%;" type="text" value="' + BillOfEntryObj.BillofEntryDate + '" name="BillofEntryDate_' + vBillOfEntry.length + '" id="BillofEntryDate_' + vBillOfEntry.length + '" readonly /></div></td>';
        tdString = tdString + '<td><div class="data3"><input type="text" style="width:100%;" value="' + BillOfEntryObj.ShippingAgentName + '" name="ShippingAgentName_' + vBillOfEntry.length + '" id="ShippingAgentName_' + vBillOfEntry.length + '" readonly/><input type="hidden" value="' + BillOfEntryObj.ShippingAgentID + '" name="ShippingAgentID_' + vBillOfEntry.length + '" id="ShippingAgentID_' + vBillOfEntry.length + '" /></div></td>';
        tdString = tdString + '<td><a href="javascript:void(0)" onclick="angular.element(this).scope().editBillOfEntry(' + vBillOfEntry.length + ')"><i class="fa fa-pencil"></i></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:void(0)" onclick="deleteRow(this)"><i class="fa fa-times-circle"></i></a></td>';
        tdString = tdString + '</tr>';
        $("#bill_of_entry_table").append(tdString);
        $scope.BIllOfEntry = '';
        $scope.BillofEntryDate = '';
        $scope.ShippingAgentID = '';
        ClearBillOfEntryTab();
    };

    $scope.editBillOfEntry = function (index) {
        $('#billofentry_update_index').val(index);
        $('#BillOfEntry').val($('#BIllOfEntry_' + index).val());
        $('#BillofEntryDate').val($('#BillofEntryDate_' + index).val());
        $('#ShippingAgentID').val($('#ShippingAgentID_' + index).val());
        $('#update_billofentry').show();
        $('#add_bill').hide();
        $('#cancel_edit_billofentry').show();
    };

    function BindBills() {
        $http({
            url: '/Job/GetBillByJobIdandUserID/',
            method: 'GET'
        }).success(function (data, status, headers, config) {
            $scope.Billing = data;
            vBillOfEntry = JSON.parse('[]');
            debugger;
            for (var i = 0; i < $scope.Billing.length; i++) {
                var BillOfEntryObj = $scope.Billing[i];
                var BillOfEntryStr = JSON.stringify(BillOfEntryObj);
                vBillOfEntry.push(BillOfEntryStr);
                var BOEdate = '';
                var BOEdate = new Date(parseInt(BillOfEntryObj.BillofEntryDate.substr(6)));
                var BOEDateStr = '';
                if (isValidDate(BOEdate)) {
                    BOEDateStr = BOEdate.getDate() + '-' + month_names_short[BOEdate.getMonth()] + '-' + BOEdate.getFullYear();
                }
                var ShippingAgentName = $("#ShippingAgentID option[value='" + BillOfEntryObj.ShippingAgentID + "']").text();
                var tdString = '<tr><td><div class= "data1" ><input type="text" style="width:100%;" value="' + BillOfEntryObj.BIllOfEntry + '" title="' + BillOfEntryObj.BIllOfEntry + '" name="BIllOfEntry_' + vBillOfEntry.length + '" id="BIllOfEntry_' + vBillOfEntry.length + '" readonly/></div></td>';
                tdString = tdString + '<td><div class="data2" ><input style="width:100%;" type="text" value="' + BOEDateStr + '" title="' + BOEDateStr + '" name="BillofEntryDate_' + vBillOfEntry.length + '" id="BillofEntryDate_' + vBillOfEntry.length + '" readonly /></div></td>';
                tdString = tdString + '<td><div class="data3"><input type="text" style="width:100%;" value="' + ShippingAgentName + '" title="' + ShippingAgentName + '" name="ShippingAgentName_' + vBillOfEntry.length + '" id="ShippingAgentName_' + vBillOfEntry.length + '" readonly/><input type="hidden" value="' + BillOfEntryObj.ShippingAgentID + '" name="ShippingAgentID_' + vBillOfEntry.length + '" id="ShippingAgentID_' + vBillOfEntry.length + '" /></div></td>';
                tdString = tdString + '<td><a href="javascript:void(0)" onclick="angular.element(this).scope().editBillOfEntry(' + vBillOfEntry.length + ')"><i class="fa fa-pencil"></i></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:void(0)" onclick="deleteRow(this)"><i class="fa fa-times-circle"></i></a></td>';
                tdString = tdString + '</tr>';
                $("#bill_of_entry_table").append(tdString);
            }
            $("#AddBill").show();
        });
    };

    $scope.updateBillOfEntry = function () {
        var index = $('#billofentry_update_index').val();
        $('#BIllOfEntry_' + index).val($('#BillOfEntry').val());
        $('#BillofEntryDate_' + index).val($('#BillofEntryDate').val());
        $('#ShippingAgentID_' + index).val($('#ShippingAgentID').val());
        $('#update_billofentry').hide();
        $('#add_bill').show();
        $('#cancel_edit_billofentry').hide();
        $('#billofentry_update_index').val('0');
        ClearBillOfEntryTab();
    };

    $scope.cancelBillOfEntry = function () {
        $('#update_billofentry').hide();
        $('#add_bill').show();
        $('#cancel_edit_billofentry').hide();
        $('#billofentry_update_index').val('0');
        ClearBillOfEntryTab();
    };

    function ClearBillOfEntryTab() {
        $('#BillOfEntry').val('');
        $('#BillofEntryDate').val('');
        $('#ShippingAgentID').val('');
    }

    var vContainer = JSON.parse('[]');

    function BindContainer() {
        $http({
            url: '/Job/GetContainerByJobIdandUserID/',
            method: 'GET'
        }).success(function (data, status, headers, config) {
            $scope.Containers = data;

            vContainer = JSON.parse('[]');
            for (var i = 0; i < $scope.Containers.length; i++) {
                var ContainerObj = $scope.Containers[i];
                var ContainerStr = JSON.stringify(ContainerObj);
                vContainer.push(ContainerStr);
                var tdString = '<tr><td><div class= "data1" ><input type="text" style="width:100%;" value="' + ContainerObj.ContainerType + '" name="ContainerType_' + vContainer.length + '" id="ContainerType_' + vContainer.length + '" readonly/><input type="hidden" value="' + ContainerObj.ContainerTypeID + '" name="ContainerTypeID_' + vContainer.length + '" id="ContainerTypeID_' + vContainer.length + '" /></div ></td>';
                tdString = tdString + '<td><div class="data2" ><input type="text" style="width:100%;" value="' + ContainerObj.ContainerNo + '" name="ContainerNo_' + vContainer.length + '" id="ContainerNo_' + vContainer.length + '" readonly/></div></td>';
                tdString = tdString + '<td><div class="data3"><input type="text" style="width:100%;" value="' + ContainerObj.SealNo + '" name="SealNo_' + vContainer.length + '" id="SealNo_' + vContainer.length + '" readonly/></div></td>';
                tdString = tdString + '<td><div class="data" ><input type="text" style="width:100%;" value="' + ContainerObj.Description + '" name="ContainerDescription_' + vContainer.length + '" id="ContainerDescription_' + vContainer.length + '" readonly/></div></td>';
                tdString = tdString + '<td><a href="javascript:void(0)" onclick="angular.element(this).scope().editContainerDetails(' + vContainer.length + ')"><i class="fa fa-pencil"></i></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:void(0)" onclick="deleteRow(this)"><i class="fa fa-times-circle"></i></a></td>';
                tdString = tdString + '</tr>';
                $("#container_table").append(tdString);
            }
            $("#AddContainer").show();
        });
    };

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
        var tdString = '<tr><td><div class= "data1" ><input type="text" style="width:100%;" value="' + ContainerObj.ContainerTypeName + '" name="ContainerType_' + vContainer.length + '" id="ContainerType_' + vContainer.length + '" readonly/><input type="hidden" value="' + ContainerObj.ContainerTypeID + '" name="ContainerTypeID_' + vContainer.length + '" id="ContainerTypeID_' + vContainer.length + '" /></div ></td>';
        tdString = tdString + '<td><div class="data2" ><input type="text" style="width:100%;" value="' + ContainerObj.ContainerNo + '" name="ContainerNo_' + vContainer.length + '" id="ContainerNo_' + vContainer.length + '" readonly/></div></td>';
        tdString = tdString + '<td><div class="data3"><input type="text" style="width:100%;" value="' + ContainerObj.SealNo + '" name="SealNo_' + vContainer.length + '" id="SealNo_' + vContainer.length + '" readonly/></div></td>';
        tdString = tdString + '<td><div class="data" ><input type="text" style="width:100%;" value="' + ContainerObj.Description + '" name="ContainerDescription_' + vContainer.length + '" id="ContainerDescription_' + vContainer.length + '" readonly/></div></td>';
        tdString = tdString + '<td><a href="javascript:void(0)" onclick="angular.element(this).scope().editContainerDetails(' + vContainer.length + ')"><i class="fa fa-pencil"></i></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:void(0)" onclick="deleteRow(this)"><i class="fa fa-times-circle"></i></a></td>';
        tdString = tdString + '</tr>';
        $("#container_table").append(tdString);
        //   $scope.Charges = vCharges;
        //  var test = JSON.stringify(vCharges);
        $("#AddContainer").show();
        $scope.ContainerTypeID = '';
        $('#ContainerTypeID').val("0");
        $scope.ContainerNo = '';
        $scope.SealNo = '';
        $scope.ContainerDescription = '';
        ClearContainerDetailsTab();
    };

    $scope.editContainerDetails = function (Newindex) {
        $('#container_update_index').val(Newindex);
        $("#ContainerTypeID").val($("#ContainerTypeID_" + Newindex).val());
        $('#ContainerNo').val($('#ContainerNo_' + Newindex).val());
        $('#SealNo').val($("#SealNo_" + Newindex).val());
        $('#ContainerDescription').val($("#ContainerDescription_" + Newindex).val());
        $('#update_container').show();
        $('#add_container').hide();
        $('#cancel_edit_container').show();
    };

    function ClearContainerDetailsTab() {
        $('#ContainerTypeID').val('0');
        $('#ContainerNo').val('');
        $('#SealNo').val('');
        $('#ContainerDescription').val('');
    }

    $scope.updateContainerDetails = function () {
        debugger;
        var index = $('#container_update_index').val();
        $('#ContainerType_' + index).val($('#ContainerTypeID option:selected').text());
        $('#ContainerTypeID_' + index).val($('#ContainerTypeID').val());
        $('#ContainerNo_' + index).val($('#ContainerNo').val());
        $('#SealNo_' + index).val($('#SealNo').val());
        $('#ContainerDescription_' + index).val($('#ContainerDescription').val());
        ClearContainerDetailsTab();
        $('#update_container').hide();
        $('#add_container').show();
        $('#cancel_edit_container').hide();
    };

    $scope.cancelEditContainerDetails = function () {
        $('#container_update_index').val('0');
        ClearContainerDetailsTab();
        $('#update_container').hide();
        $('#add_container').show();
        $('#cancel_edit_container').hide();
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
        var tdString = '<tr><td><div class= "data1" ><input type="text" style="width:70px;" value="' + CargoDescObj.Mark + '" name="Mark_' + vCargoDesc.length + '" id="Mark_' + vCargoDesc.length + '" readonly/></div ></td>';
        tdString = tdString + '<td><div class="data2" ><input type="text" style="width:95%;" value="' + CargoDescObj.Description + '" name="CarDescription_' + vCargoDesc.length + '" id="CarDescription_' + vCargoDesc.length + '" readonly/></div></td>';
        tdString = tdString + '<td><div class="data3" ><input type="text" style="width:70px;" value="' + CargoDescObj.weight + '" name="weight_' + vCargoDesc.length + '" id="weight_' + vCargoDesc.length + '" readonly/></div></td>';
        tdString = tdString + '<td><div class="data5"><input type="text" style="width:70px;" value="' + CargoDescObj.volume + '" name="volume_' + vCargoDesc.length + '" id="volume_' + vCargoDesc.length + '" readonly/></div></td>';
        tdString = tdString + '<td><div class="data6"><input type="text" style="width:70px;" value="' + CargoDescObj.Packages + '" name="Packages_' + vCargoDesc.length + '" id="Packages_' + vCargoDesc.length + '" readonly/></div></td>';
        tdString = tdString + '<td><div class="data6"><input type="text" style="width:70px;" value="' + CargoDescObj.GrossWeight + '" name="GrossWeight_' + vCargoDesc.length + '" id="GrossWeight_' + vCargoDesc.length + '" readonly/></div></td>';
        tdString = tdString + '<td><a href="javascript:void(0)" onclick="angular.element(this).scope().editCargo(' + vCargoDesc.length + ')"><i class="fa fa-pencil"></i></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:void(0)" onclick="deleteRow(this)"><i class="fa fa-times-circle"></i></a></td>';
        tdString = tdString + '</tr>';
        $("#cargo_table").append(tdString);
        ClearCargoTab();
    };

    $scope.editCargo = function (index) {
        $('#cargo_update_index').val(index);
        $('#Mark').val($('#Mark_' + index).val());
        $('#Description').val($('#CarDescription_' + index).val());
        $('#weight').val($('#weight_' + index).val());
        $('#volume').val($('#volume_' + index).val());
        $('#Packages').val($('#Packages_' + index).val());
        $('#GrossWeight').val($('#GrossWeight_' + index).val());
        $('#update_cargo').show();
        $('#cancel_edit_cargo').show();
        $('#add_cargo').hide();
    };

    $scope.updateCargo = function () {
        var index = $('#cargo_update_index').val();
        $('#Mark_' + index).val($('#Mark').val());
        $('#CarDescription_' + index).val($('#Description').val());
        $('#weight_' + index).val($('#weight').val());
        $('#volume_' + index).val($('#volume').val());
        $('#Packages_' + index).val($('#Packages').val());
        $('#GrossWeight_' + index).val($('#GrossWeight').val());
        $('#update_cargo').hide();
        $('#cancel_edit_cargo').hide();
        $('#add_cargo').show();
        ClearCargoTab();
    };
    $scope.cancelEditCargo = function () {
        ClearCargoTab();
        $('#update_cargo').hide();
        $('#cancel_edit_cargo').hide();
        $('#add_cargo').show();
    };
    function ClearCargoTab() {
        $('#Mark').val('');
        $('#Description').val('');
        $('#weight').val('');
        $('#volume').val('');
        $('#Packages').val('');
        $('#GrossWeight').val('');
        $('#cargo_update_index').val('0');
    }

    function BindCargoDes() {

        $http({
            url: '/Job/GetCargoByJobIdandUserID/',
            method: 'GET'
        }).success(function (data, status, headers, config) {

            $scope.CargoDescription = data;
            vCargoDesc = JSON.parse('[]');
            for (var i = 0; i < $scope.CargoDescription.length; i++) {
                var CargoDescObj = $scope.CargoDescription[i];
                var CargoDescStr = JSON.stringify(CargoDescObj);
                vCargoDesc.push(CargoDescStr);
                var tdString = '<tr><td><div class= "data1" ><input type="text" style="width:70px;" value="' + CargoDescObj.Mark + '" name="Mark_' + vCargoDesc.length + '" id="Mark_' + vCargoDesc.length + '" readonly/></div ></td>';
                tdString = tdString + '<td><div class="data2" ><input type="text" style="width:95%;" value="' + CargoDescObj.Description + '" name="CarDescription_' + vCargoDesc.length + '" id="CarDescription_' + vCargoDesc.length + '" readonly/></div></td>';
                tdString = tdString + '<td><div class="data3" ><input type="text" style="width:70px;" value="' + CargoDescObj.weight + '" name="weight_' + vCargoDesc.length + '" id="weight_' + vCargoDesc.length + '" readonly/></div></td>';
                tdString = tdString + '<td><div class="data5"><input type="text" style="width:70px;" value="' + CargoDescObj.volume + '" name="volume_' + vCargoDesc.length + '" id="volume_' + vCargoDesc.length + '" readonly/></div></td>';
                tdString = tdString + '<td><div class="data6"><input type="text" style="width:70px;" value="' + CargoDescObj.Packages + '" name="Packages_' + vCargoDesc.length + '" id="Packages_' + vCargoDesc.length + '" readonly/></div></td>';
                tdString = tdString + '<td><div class="data6"><input type="text" style="width:70px;" value="' + CargoDescObj.GrossWeight + '" name="GrossWeight_' + vCargoDesc.length + '" id="GrossWeight_' + vCargoDesc.length + '" readonly/></div></td>';
                tdString = tdString + '<td><a href="javascript:void(0)" onclick="angular.element(this).scope().editCargo(' + vCargoDesc.length + ')"><i class="fa fa-pencil"></i></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:void(0)" onclick="deleteRow(this)"><i class="fa fa-times-circle"></i></a></td>';
                tdString = tdString + '</tr>';
                $("#cargo_table").append(tdString);
            }
                $("#AddCargo").show();
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

   function BindUnit() {


        $http({
            url: '/Job/GetUnitList/',
            method: 'GET'
        }).success(function (data, status, headers, config) {
            debugger;
            $scope.UnitList = data;

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

        debugger;
        $http({
            url: '/Job/GetCurrencyList/',
            method: 'GET'
        }).success(function (data, status, headers, config) {
            debugger;
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
        //$scope.LocalCur = Charge.Cost;
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


