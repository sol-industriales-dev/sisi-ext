function displayLogo(add, remove) {
    $("#imgLogo").removeClass(remove);
    $("#imgLogo").addClass(add);
    $("#imgLogo").parent().toggleClass('logo-aling');
}

function initMenu() {
    $('#menu ul').addClass("inactivo");
    $('#menu li a').attr("data-option", "off");

    $("#btnMenuPpal").on('click', function () {
        $("#sidebarWrapper").children().addClass('wrapper-hidden');
    });

    $("#sidebarWrapper").on('mouseenter', function () {
        $("#sidebarWrapper").children().removeClass('wrapper-toggled');
        displayLogo("logfix", "logfixmin");
        $('#menu li.child a.Selected').parents('li.parent').children('ul').slideDown(100,
           function () {
               $('#menu li a').attr("data-option", "on");
           });
    });

    $("#sidebarWrapper").on('mouseleave', function () {
        $("#sidebarWrapper").children().addClass('wrapper-toggled');
        displayLogo("logfixmin", "logfix");
        $('#menu li a').parent("li").children("ul").slideUp(100,
          function () {
              $('#menu li a').attr("data-option", "off");
          });
    });
    $("#sidebar-wrapper").on('mouseleave', function () {
        $("#this2").removeClass("hidden");
        $("#this1").addClass("hidden");
        $("#sidebar-wrapper").removeClass("hasHover");
        $('#menu li a').parent("li").children("ul").slideUp(100,
            function () {
                $('#menu li a').attr("data-option", "off");
            });
    });

    $("#sidebar-wrapper").on('mouseenter', function () {
        $("#sidebar-wrapper").toggleClass("hasHover");
        $("#this1").removeClass("hidden");
        $("#this2").addClass("hidden");
        $('#menu li.child a.Selected').parents('li.parent').children('ul').slideDown(100,
            function () {
                $('#menu li a').attr("data-option", "on");
            });
    });

    $("#listaMenu").on('mouseleave', function () {
        $("#btnMenuPpal").trigger("click");
        $(this).on('hidden.bs.dropdown', function () {
            $("#sidebarWrapper").children().toggleClass('wrapper-hidden');
        });
    });

    $(".dropdown").on('hidden.bs.dropdown', function () {
        $("#sidebarWrapper").children().toggleClass('wrapper-hidden');
    });
};

$(function () {
    initMenu();


    $('#menu li a').on('click', function () {
        obj = $(this);

        item = obj.find("ul").parent("li").children("a");
        obj.attr("data-option", "off");

        var a = $(this);
       // a.children('span.caret').removeClass('caret-up');
        if (true) {
            a.parent().parent().find("a[data-option='on']").parent("li").children("ul").slideUp(100 / 1.2,
                function () {
                    a.parent("li").children("a").attr("data-option", "off");
                    a.children('span.caret').removeClass('caret-up');
                })
        }
        if (a.attr("data-option") == "off") {
            a.parent("li").children("ul").slideDown(100,
                function () {
                    a.attr("data-option", "on");
                    a.children('span.caret').addClass('caret-up');
                });
        }
        if (a.attr("data-option") == "on") {
            a.attr("data-option", "off");
            a.children('span.caret').removeClass('caret-up');
            a.parent("li").children("ul").slideUp(100);
        }

    });

    $('#menu li.child a').on('click', function () {
        obj = $(this);
        $('#menu li.child a').removeClass("Selected");
        obj.attr("data-option", "off");
        obj.addClass('Selected');
        obj.attr("data-option", "on");

    });

    $("#menu-toggle").on('click', function (e) {
        e.preventDefault();
        $("#wrapper").toggleClass("toggled");
        if ($("#wrapper").hasClass('toggled')) {
            $("#sidebar-wrapper").toggleClass("hasHover");
        } else {
            $("#sidebar-wrapper").removeClass("hasHover");
        }
    });
    $("#menu-toggle-2").on('click', function (e) {
        e.preventDefault();
        if ($("#wrapper").hasClass('toggled-3')) {
            $("#sidebar-wrapper").toggleClass("hasHover");
        } else {
            $("#sidebar-wrapper").removeClass("hasHover");
        }
        $("#wrapper").toggleClass("toggled-3");
        $("#menu ul").removeClass('activo');
        $('#menu ul').addClass("inactivo");
    });

});
