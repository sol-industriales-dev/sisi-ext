﻿///<reference path="jquery-1.3.2-vsdoc2.js" />
/*
Copyright (c) 2009 Mikael Söderström.
Contact: vimpyboy@msn.com

Feel free to use this script as long as you don't remove this comment.
*/

(function($) {
    var isLoaded;
    var isClosed;

    $.fn.Ribbon = function(ribbonSettings) {
        var settings = $.extend({ theme: 'windows7' }, ribbonSettings || {});

        $('.ribbon a').each(function() {
            if ($(this).attr('accesskey')) {
                $(this).append('<div rel="accesskeyhelper" style="display: none; position: absolute; background-repeat: none; width: 16px; padding: 0px; text-align: center; height: 17px; line-height: 17px; top: ' + $(this).offset().top + 'px; left: ' + ($(this).offset().left + $(this).width() - 15) + 'px;">' + $(this).attr('accesskey') + '</div>');
            }
        });

        $('head').append('<link href="ribbon/themes/' + settings.theme + '/ribbon.css" rel="stylesheet" type="text/css" />" />');

        if (!isLoaded) {
            SetupMenu(settings);
        }

        $(document).keydown(function(e) { ShowAccessKeys(e); });
        $(document).keyup(function(e) { HideAccessKeys(e); });

        function SetupMenu(settings) {
            $('.menu li a:first').addClass('active');
            $('.menu li ul').hide();
            $('.menu li a:first').parent().children('ul:first').show();
            $('.menu li a:first').parent().children('ul:first').addClass('submenu');
            $('.menu li > a').click(function() { ShowSubMenu(this); });
            $('.orb').click(function() { ShowMenu(); });
            $('.orb ul').hide();
            $('.orb ul ul').hide();
            $('.orb li ul li ul').show();
            $('.orb li li ul').each(function() { $(this).prepend('<div style="background-color: #EBF2F7; height: 25px; line-height: 25px; width: 292px; padding-left: 9px; border-bottom: 1px solid #CFDBEB;">' + $(this).parent().children('a:first').text() + '</div>'); });
            $('.orb li li a').each(function() { if ($(this).parent().children('ul').length > 0) { $(this).addClass('arrow') } });

            //$('.ribbon-list div').each(function() { $(this).parent().width($(this).parent().width()); });

            $('.ribbon-list div').click(function(e) {
                var elwidth = $(this).parent().width();
                var insideX = e.pageX > $(this).offset().left && e.pageX < $(this).offset().left + $(this).width();
                var insideY = e.pageY > $(this).offset().top && e.pageY < $(this).offset().top + $(this).height();

                $('.ribbon-list div ul').fadeOut('fast');

                if (insideX && insideY) {
                    $(this).attr('style', 'background-image: ' + $(this).css('background-image'));

                    $(this).parent().width(elwidth);

                    $(this).children('ul').width(elwidth - 4);
                    $(this).children('ul').fadeIn('fast');
                }
            });

//            $('.ribbon-list div').parents().click(function(e) {
//                var outsideX = e.pageX < $('.ribbon-list div ul:visible').parent().offset().left || e.pageX > $('.ribbon-list div ul:visible').parent().offset().left + $('.ribbon-list div ul:visible').parent().width();
//                var outsideY = e.pageY < $('.ribbon-list div ul:visible').parent().offset().top || e.pageY > $('.ribbon-list div ul:visible').parent().offset().top + $('.ribbon-list div ul:visible').parent().height();

//                if (outsideX || outsideY) {
//                    $('.ribbon-list div ul:visible').each(function() {
//                        $(this).fadeOut('fast');
//                    });
//                    $('.ribbon-list div').css('background-image', '');
//                }
//            });

            $('.orb li li a').mouseover(function() { ShowOrbChildren(this); });

            $('.menu li > a').dblclick(function() {
                $('ul .submenu').animate({ height: "hide" });
                $('body').animate({ paddingTop: $(this).parent().parent().parent().parent().height() });
                isClosed = true;
            });
        }

        $('.ribbon').parents().click(function(e) {
            var outsideX = e.pageX < $('.orb ul:first').offset().left || e.pageX > $('.orb ul:first').offset().left + $('.orb ul:first').width();
            var outsideY = e.pageY < $('.orb ul:first img:first').offset().top || e.pageY > $('.orb ul:first').offset().top + $('.orb ul:first').height();

            if (outsideX || outsideY)
                $('.orb ul').fadeOut('fast');
        });

        if (isLoaded) {
            $('.orb li:first ul:first img:first').remove();
            $('.orb li:first ul:first img:last').remove();
            $('.ribbon-list div img[src*="/images/arrow_down.png"]').remove();
        }

        $('.orb li:first ul:first').prepend('<img src="ribbon/themes/' + settings.theme + '/images/menu_top.png" style="margin-left: -10px; margin-top: -22px;" />');
        $('.orb li:first ul:first').append('<img src="ribbon/themes/' + settings.theme + '/images/menu_bottom.png" style="margin-left: -10px; margin-bottom: -22px;" />');

        $('.ribbon-list div').each(function() { if ($(this).children('ul').length > 0) { $(this).append('<img src="ribbon/themes/' + settings.theme + '/images/arrow_down.png" style="float: right; margin-top: 5px;" />') } });

        //Hack for IE 7.
        if (navigator.appVersion.indexOf('MSIE 6.0') > -1 || navigator.appVersion.indexOf('MSIE 7.0') > -1) {
            $('ul.menu li li div').css('width', '90px');
            $('ul.menu').css('width', '500px');
            $('ul.menu').css('float', 'left');
            $('ul.menu .submenu li div.ribbon-list').css('width', '100px');
            $('ul.menu .submenu li div.ribbon-list div').css('width', '100px');
        }

        $('a[href=' + window.location.hash + ']').click();

        isLoaded = true;

        function ResetSubMenu() {
            $('.menu li a').removeClass('active');
            $('.menu ul').removeClass('submenu');
            $('.menu li ul').hide();
        }

        function ShowSubMenu(e) {
            var isActive = $(e).next().css('display') == 'block';
            ResetSubMenu();

            $(e).addClass('active');
            $(e).parent().children('ul:first').addClass('submenu');

            $(e).parent().children('ul:first').show();
            $('body').css('padding-top', '120px');

            isClosed = false;
        }

        function ShowOrbChildren(e) {
            if (($(e).parent().children('ul').css('display') == 'none' || $(e).parent().children('ul').length == 0) && $(e).parent().parent().parent().parent().hasClass('orb')) {
                $('.orb li li ul').fadeOut('fast');
                $(e).parent().children('ul').fadeIn('fast');
            }
        }

        function ShowMenu() {
            $('.orb ul').animate({ opacity: 'toggle' }, 'fast');
        }

        function ShowAccessKeys(e) {
            if (e.altKey) {
                $('div[rel="accesskeyhelper"]').each(function() { $(this).css('top', $(this).parent().offset().top).css('left', $(this).parent().offset().left - 20 + $(this).parent().width()); $(this).show(); });
            }
        }

        function HideAccessKeys(e) {
            $('div[rel="accesskeyhelper"]').hide();
        }
    }
})(jQuery);