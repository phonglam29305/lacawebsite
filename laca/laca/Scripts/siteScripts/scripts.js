//jQuery(document).ready(function () {
//    jQuery('.extra-main-product').each(function () { jQuery(this).find('.products-grid:last').addClass('last'); });
//});
//jQuery(document).ready(function () {
//    jQuery('.home_new').each(function () {
//        jQuery(this).find('.products-grid:last').addClass('last');
//    });
//});
//jQuery(function () {
//    jQuery('.quick-access .block-content form ').jqTransform({ imgPath: 'jqtransformplugin/img/' });
//});
//jQuery(document).ready(function () {
//    jQuery('.footer ul li a ').hover(function () {
//        jQuery(this).stop().animate({ paddingLeft: '0', marginLeft: '5px' }, 100);
//    }, function () { jQuery(this).stop().animate({ paddingLeft: '0', marginLeft: '0px' }, 100); });
//});
jQuery(document).ready(function () {
    jQuery('.list-1 li').prepend('<span></span>').find('>a').wrap('<div></div>').hover(function () {
        jQuery(this).parent().prev().animate({ height: 0, marginTop: 10, backgroundColor: '#cccccc' }, 100, function () {
            j
            Query(this).animate({ height: 19, marginTop: 0, backgroundColor: '#cccccc' }, 100)
        })
    }, function () {
        jQuery(this).parent().prev().stop().animate({ height: 0, marginTop: 10, backgroundColor: '#cccccc' }, 100, function () {
            jQuery(this).animate({ height: 19, marginTop: 0, backgroundColor: '#fff' }, 100)
        })
    })
});
jQuery(document).ready(function () {
    //alert(jQuery("#back-top"));
    jQuery("#back-top").hide();
    jQuery(function () {
        jQuery(window).scroll(function () {
            if (jQuery(this).scrollTop() > 100) { jQuery('#back-top').fadeIn(); } else { jQuery('#back-top').fadeOut(); }
        }); jQuery('#back-top a').click(function () { jQuery('body,html').animate({ scrollTop: 0 }, 800); return false; });
    });
});

//jQuery(document).ready(function () {
//    var fl = false, fl2 = false; jQuery('.block-cart-header .cart-content').hide();
//    jQuery('.block-cart-header  .amount a, .block-cart-header .cart-content').hover(function () {
//        jQuery('.block-cart-header .cart-content').stop(true, true).slideDown(400);
//    }, function () { jQuery('.block-cart-header .cart-content').stop(true, true).delay(400).slideUp(300); })
//});
//jQuery(document).ready(function () {
//    jQuery('.itemSubMenu').each(function () {
//        jQuery(this).find('.itemMenu a:last').addClass('last');
//    });
//});
//jQuery(document).ready(function () {
//    jQuery('.itemSubMenu .itemMenu  a  span').hover(function () {
//        jQuery(this).stop().animate({ paddingLeft: '0', marginLeft: '5px' }, 100);
//    }, function () { jQuery(this).stop().animate({ paddingLeft: '0', marginLeft: '0px' }, 100); });
//});
//jQuery(document).ready(function () {
//    jQuery("a[data-gal^='prettyPhoto']").prettyPhoto({ animationSpeed: 'normal', padding: 40, opacity: 0.35, showTitle: true, counter_separator_label: '0', allowresize: true, counter_separator_label: '/', theme: 'light_rounded' });
//});