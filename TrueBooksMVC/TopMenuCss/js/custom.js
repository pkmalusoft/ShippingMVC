// MESSAGE BOX FADING SCRIPTS
$(document).ready(function () {
    //$(".close-yellow").live('click', function () {
    $(document).on('click', '.close-yellow', function (e) {
        $("#message-yellow").fadeOut("slow");
    });
    //$(".close-red").live('click', function () {
    $(document).on('click', '.close-red', function (e) {
        $("#message-red").fadeOut("slow");
    });
    //$(".close-blue").live('click', function () {
    $(document).on('click', '.close-blue', function (e) {
        $("#message-blue").fadeOut("slow");
    });
    //$(".close-green").live('click', function () {
    $(document).on('click', '.close-green', function (e) {
        $("#message-green").fadeOut("slow");
    });
});
function OnResponseEnd_CalcCustReceipt(sender, eventArgs)
{
    var tbody = $("#tbl_cust_receipts");
    if (tbody.find("tr:not(:last-child)").length >= 0)
    {
        var total3 = tbody.find("tr:last-child #total3");
        var total4 = tbody.find("tr:last-child #total4");
        var total5 = tbody.find("tr:last-child #total5");
       // var total6 = tbody.find("tr:last-child #total6");
        total3.text('0');
        total4.text('0');
        total5.text('0');
       // total6.text('0');
        tbody.find("tr:not(:last-child)").each(function ()
        {
            var td3 = $.trim($(this).find("td").eq(3).find("> span").text());
            var td4 = $.trim($(this).find("td").eq(4).find("> span").text());
            var td5 = $.trim($(this).find("td").eq(5).text());
            //var td6 = $.trim($(this).find("td").eq(6).find("input[type='text']").val());
            total3.text((parseFloat(total3.text()) + parseFloat(td3)).toFixed(2));
            total4.text((parseFloat(total4.text()) + parseFloat(td4)).toFixed(2));
           
            if (td5 <= 0)
            {
                td5 = 0;
            }
            total5.text((parseFloat(total5.text()) + parseFloat(td5)).toFixed(2));
            //total6.text((parseFloat(total6.text()) + parseFloat(td6)).toFixed(2));

        });
    }
}
// END
