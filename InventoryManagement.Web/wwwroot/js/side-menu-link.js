
jQuery(function ($) {
    //sidebar event
    $('.js-show-sidedrawer').on('click', showSidedrawer);
    $('.js-hide-sidedrawer').on('click', hideSidedrawer);  
});

//selector
 var $bodyEl = $('body');
 var $sidedrawerEl = $('#sidedrawer');

 //menu click
function showSidedrawer() {
    // show overlay
    var options = {
        onclose: function () {
            $sidedrawerEl.removeClass('active').appendTo(document.body);
        }
    };

    var $overlayEl = $(mui.overlay('on', options));

    // show element
    $sidedrawerEl.appendTo($overlayEl);
    setTimeout(function () {
        $sidedrawerEl.addClass('active');
    },20);
}

 function hideSidedrawer() {
    $bodyEl.toggleClass('hide-sidedrawer');
}

 //create links
 function appendLinks(data) {
    const menuItem = $("#menuItem");
    let menu = `<li><a href="/Dashboard/Index"><strong> <span class="fas fa-tachometer-alt"></span> Dashboard</strong></a></li><li><a href="/Dashboard/UserProfile"><strong> <span class="fas fa-user-circle"></span> Profile</strong></a></li>`;

    $.each(data, (i, item) => {
        menu += `<strong><span class="${item.IconClass}"></span> ${item.Category} <i class="fas fa-caret-right"></i></strong><ul class="sub-manu">${menuLink(item.links)}</ul>`;
    });

    menuItem.html(menu);
}

 function menuLink(data) {
    let link = "";
    $.each(data, (i, item) => {
        link += `<li><a class="links" href="/${item.Controller}/${item.Action}">${item.Title}</a></li>`;
    });
    return link;
}