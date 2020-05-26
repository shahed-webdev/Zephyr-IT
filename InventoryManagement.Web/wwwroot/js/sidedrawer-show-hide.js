
jQuery(function ($) {
    //sidebar event
    $('.js-show-sidedrawer').on('click', showSidedrawer);
    $('.js-hide-sidedrawer').on('click', hideSidedrawer);  
});

//selector
 var bodyEl = $('body');
 var sidedrawerEl = $('#sidedrawer');

 //menu click
function showSidedrawer() {
    // show overlay
    var options = {
        onclose: function () {
            sidedrawerEl.removeClass('active').appendTo(document.body);
        }
    };

    var overlayEl = $(mui.overlay('on', options));

    // show element
    sidedrawerEl.appendTo(overlayEl);
    setTimeout(function () {
        sidedrawerEl.addClass('active');
    },20);
}

 function hideSidedrawer() {
    bodyEl.toggleClass('hide-sidedrawer');
}
