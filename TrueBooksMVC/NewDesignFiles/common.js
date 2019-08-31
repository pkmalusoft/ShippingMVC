$(document).ready(function () {
    /* to assign border of same height in data table of JOBS and other pages */
    $('.tab-pane').addClass('active');
    $('.panel-collapse').removeClass('collapse');
    $('.panel-collapse').addClass('in');
    $('.data').each(function () {
        var maxHeight = 0;
        $(this).children().each(function () {
            if ($(this).outerHeight() > maxHeight)
                maxHeight = $(this).outerHeight();
        });
        setTimeout(assignHeight(this, maxHeight), 500);
    });
    $('.tab-pane').removeClass('active');
    $('.panel-collapse').addClass('collapse');
    $('.panel-collapse').removeClass('in');
    $($('.tab-pane')[0]).addClass('active');
    
    /* to show first tab "Cargo Description" active in mobile view */
    $("#cargo_desc-collapse").addClass('in');
});
function assignHeight(thisDiv, maxHeight) {
    $(thisDiv).children().each(function () {
        $(this).css('height', maxHeight);
    });
}
    