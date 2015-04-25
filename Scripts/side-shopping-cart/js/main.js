jQuery(document).ready(function ($) {
    //if you change this breakpoint in the style.css file (or _layout.scss if you use SASS), don't forget to update this value as well

    initialCart($);

});

function test() {
    alert("HEre waer are");
}
function initialCart($) {
    var $L = 1200,
		$menu_navigation = $('#main-nav'),
		$cart_trigger = $('#cd-cart-trigger'),
		$hamburger_icon = $('#cd-hamburger-menu'),
		$lateral_cart = $('#cd-cart'),
		$shadow_layer = $('#cd-shadow-layer'),
        $checkout_trigger = $('#checkout_trigger'),
        $checkout_trigger2 = $('#checkout_trigger2'),
       /* $remove = $('#removeFromCart'),*/
        $checkoutBox = $('#msform');
    /*
    $remove.on('click', function (event) {
        event.preventDefault();
        $.getJSON("Buyer/OrderNow",null,function(cart){
        });
    });
    */
    var current_fs, next_fs, previous_fs; //fieldsets
    var left, opacity, scale; //fieldset properties which we will animate
    var animating; //flag to prevent quick multi-click glitches
    /*
    $remove.on('click', function (event) {
        alert("I am clicked");
    
        var form = $(this).parent("form");

        $.ajax({
            type: "POST",
            url: form.attr('action'),
            data: form.serialize()
        })
            .success(function (html) {
                

                $("#mainCartPart").replaceWith(html);

            })
            .error(function () {
                
            });

        return false;
    }); 
    */

    $checkout_trigger.on('click', function (event) {
        $checkoutBox.addClass('speed-in');
        $lateral_cart.removeClass('speed-in');
    });

    $checkout_trigger2.on('click', function (event) {
        event.preventDefault();
        toggle_panel_visibility($checkoutBox, $shadow_layer, $('body'));

    });
    //open lateral menu on mobile

    $hamburger_icon.on('click', function (event) {
        event.preventDefault();
        //close cart panel (if it's open)
        $lateral_cart.removeClass('speed-in');
        toggle_panel_visibility($menu_navigation, $shadow_layer, $('body'));
    });

    //open cart
    $cart_trigger.on('click', function (event) {
        event.preventDefault();
        //close lateral menu (if it's open)
        if ($checkoutBox.hasClass('speed-in')) {
            $checkoutBox.removeClass('speed-in').on('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend', function () {
                $('body').removeClass('overflow-hidden');
            });

        }

        $menu_navigation.removeClass('speed-in');
        toggle_panel_visibility($lateral_cart, $shadow_layer, $('body'));
    });

    //close lateral cart or lateral menu
    $shadow_layer.on('click', function () {
        $shadow_layer.removeClass('is-visible');
        // firefox transitions break when parent overflow is changed, so we need to wait for the end of the trasition to give the body an overflow hidden
        if ($lateral_cart.hasClass('speed-in')) {
            $lateral_cart.removeClass('speed-in').on('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend', function () {
                $('body').removeClass('overflow-hidden');
            });
            $menu_navigation.removeClass('speed-in');
        } else {
            $checkoutBox.removeClass('speed-in').on('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend', function () {
                $('body').removeClass('overflow-hidden');
            });
            $lateral_cart.removeClass('speed-in');
        }
    });

    //move #main-navigation inside header on laptop
    //insert #main-navigation after header on mobile
    move_navigation($menu_navigation, $L);
    $(window).on('resize', function () {
        move_navigation($menu_navigation, $L);

        if ($(window).width() >= $L && $menu_navigation.hasClass('speed-in')) {
            $menu_navigation.removeClass('speed-in');
            $shadow_layer.removeClass('is-visible');
            $('body').removeClass('overflow-hidden');
        }

    });





    $(".next").on('click', function () {
        if (animating) return false;
        animating = true;

        current_fs = $(this).parent();
        next_fs = $(this).parent().next();

        //activate next step on progressbar using the index of next_fs
        $("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");

        //show the next fieldset
        next_fs.show();
        //hide the current fieldset with style
        current_fs.animate({ opacity: 0 }, {
            step: function (now, mx) {
                //as the opacity of current_fs reduces to 0 - stored in "now"
                //1. scale current_fs down to 80%
                scale = 1 - (1 - now) * 0.2;
                //2. bring next_fs from the right(50%)
                left = (now * 50) + "%";
                //3. increase opacity of next_fs to 1 as it moves in
                opacity = 1 - now;
                current_fs.css({ 'transform': 'scale(' + scale + ')' });
                next_fs.css({ 'left': left, 'opacity': opacity });
            },
            duration: 800,
            complete: function () {
                current_fs.hide();
                animating = false;
            },
            //this comes from the custom easing plugin
            easing: 'easeInOutBack'
        });
    });

    $(".previous").click(function () {
        if (animating) return false;
        animating = true;

        current_fs = $(this).parent();
        previous_fs = $(this).parent().prev();

        //de-activate current step on progressbar
        $("#progressbar li").eq($("fieldset").index(current_fs)).removeClass("active");

        //show the previous fieldset
        previous_fs.show();
        //hide the current fieldset with style
        current_fs.animate({ opacity: 0 }, {
            step: function (now, mx) {
                //as the opacity of current_fs reduces to 0 - stored in "now"
                //1. scale previous_fs from 80% to 100%
                scale = 0.8 + (1 - now) * 0.2;
                //2. take current_fs to the right(50%) - from 0%
                left = ((1 - now) * 50) + "%";
                //3. increase opacity of previous_fs to 1 as it moves in
                opacity = 1 - now;
                current_fs.css({ 'left': left });
                previous_fs.css({ 'transform': 'scale(' + scale + ')', 'opacity': opacity });
            },
            duration: 800,
            complete: function () {
                current_fs.hide();
                animating = false;
            },
            //this comes from the custom easing plugin
            easing: 'easeInOutBack'
        });
    });

    $(".submit").click(function (event) {
        event.preventDefault();
        //close lateral menu (if it's open)
        toggle_panel_visibility($checkoutBox, $shadow_layer, $('body'));
        return true;
    });

}
function toggle_panel_visibility($lateral_panel, $background_layer, $body) {
    if ($lateral_panel.hasClass('speed-in')) {
        // firefox transitions break when parent overflow is changed, so we need to wait for the end of the trasition to give the body an overflow hidden
        $lateral_panel.removeClass('speed-in').one('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend', function () {
            $body.removeClass('overflow-hidden');
        });
        $background_layer.removeClass('is-visible');

    } else {
        $lateral_panel.addClass('speed-in').one('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend', function () {
            $body.addClass('overflow-hidden');
        });
        $background_layer.addClass('is-visible');
    }
}

function move_navigation($navigation, $MQ) {
    if ($(window).width() >= $MQ) {
        $navigation.detach();
        $navigation.appendTo('header');
    } else {
        $navigation.detach();
        $navigation.insertAfter('header');
    }
}