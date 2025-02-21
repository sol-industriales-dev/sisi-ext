﻿(function (){
    $.namespace('Kubrix.Catalogo._tblBal12');
    _tblBal12 = function(){
        tblBal12 = $("#tblBal12");
        function init(){
            initTable();
        }
        function initTable(){
            paginador();
        }
        function paginador(){
            tblBal12.each(function () {
                var currentPage = 0;
                var numPerPage = 10;
                var $table = $(this);
                $table.bind('repaginate', function () {
                    $table.find('tbody tr').hide().slice(currentPage * numPerPage, (currentPage + 1) * numPerPage).show();
                });
                $table.trigger('repaginate');
                var numRows = $table.find('tbody tr').length;
                var numPages = Math.ceil(numRows / numPerPage);
                var $pager = $('<div class="pager"></div>');
                for (var page = 0; page < numPages; page++) {
                    $('<span class="page-number"></span>').text(page + 1).bind('click', {
                        newPage: page
                    }, function (event) {
                        currentPage = event.data['newPage'];
                        $table.trigger('repaginate');
                        $(this).addClass('active').siblings().removeClass('active');
                    }).appendTo($pager).addClass('clickable');
                }
                $pager.insertAfter($table).find('span.page-number:first').addClass('active');
        
        
            });
        }
        init();
    }
    $(document).ready(function () {
        Kubrix.Catalogo._tblBal12 = new _tblBal12();
    });
})();