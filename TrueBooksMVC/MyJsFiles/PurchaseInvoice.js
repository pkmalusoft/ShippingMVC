﻿$(document).ready(function () {

    $("#add_charges").click(function () {

        var product = $("#Service").val();
        var Description = $("#Description").val();
        var Quantity = $("#Quantity").val();
        var Unit = $("#Unit").val();
        var RateType = $("#RateType").val();
        var RateLC = $("#RateLC").val();
        var RateFC = $("#RateFC").val();
        var ValueLC = $("#ValueLC").val();
        var ValueFc = $("#ValueFc").val();
        var Tax = $("#Tax").val();
        var netvalues = $("#netvalue").val();
        var job = $("#job").val();

        $('#displayArea').append(
            "<tr><td>" + Description + "</td><td>" + Quantity + "</td><td>" + Unit + "</td><td>"
            + RateType + "</td><td>" + RateLC + "</td></td>" + RateFC + "</td><td>" + ValueLC + "</td><td>"
            + ValueFc + "</td><td>" + Tax + "</td><td>" + netvalues + "</td><td>" + job  + "</td><td>"
            + product + "</td></tr>"




        );



    });
});