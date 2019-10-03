$(document).ready(function () {
/* Sidebar */
// Menu Controls
var parentLink = 'a.nav-parent';
if ($(parentLink).length) {
    $('a.nav-parent').on('click', function (e) {
        debugger;
        var clickedLink = $(this);
        var dynamicDuration = 150;
        var dynamicDelay = 'linear';

        if (clickedLink.closest('li').hasClass('open')) {
            clickedLink.closest('li').removeClass('open');
            clickedLink.siblings('ul.nav').slideUp(dynamicDuration, dynamicDelay, function (elements) {
                // callback here
                // Close all open children sub-menus
                clickedLink.closest('li').find('li').removeClass('open');
                clickedLink.closest('li').find('ul.nav').removeAttr('style');
            });
        } else {
            // Opens its sub-menu
            clickedLink.closest('li').addClass('open');
            clickedLink.siblings('ul.nav').slideDown(dynamicDuration, dynamicDelay, function (elements) {
                // callback here
                // Close all open children sub-menus
               // clickedLink.closest('li').find('li').removeClass('open');
               // clickedLink.closest('li').find('ul.nav').removeAttr('style');
            });
          
            // Closes the sub-menus' and children sub-menus of other menu items in the same ul parent
            clickedLink.closest('li').siblings('li.nav-item.open').find('ul.nav').slideUp(dynamicDuration, dynamicDelay, function (elements) {
                        // callback here
                        $(this).removeAttr('style');
                        $(this).closest('li').removeClass('open');
                    
            });

            // Closes the sub-menus' and children sub-menus of other menu items in other ul parents
            clickedLink.closest('ul').siblings('ul.nav').find('ul.nav').slideUp(dynamicDuration,dynamicDelay,function (elements) {
                        // callback here
                        $(this).closest('li').removeClass('open');
                        $(this).closest('li').removeClass('open');
            });
        }
        e.preventDefault();
    });
    }

});