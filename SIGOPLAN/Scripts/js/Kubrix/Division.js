(function (){
    $.namespace('Kubrix.Analisis.Division');
    Division = function(){
        selDivision = $('#selDivision');
        txtDivision = $("#txtDivision");
        radioBtn = $('.radioBtn a');
        tblResultado = $("#tblResultado");
        tblObras = $('#tblObras');
        var tablaObra,
            lstCcDivision = [
                {div: 1, cc: '139 PRESA DE JALES SAN JULIAN', valor: 0},
                {div: 1, cc: '155 PROYECTO LA YAQUI', valor: 0},
                {div: 1, cc: '158 PRESA SAN JULIAN ETAPA II', valor: 0},
                {div: 1, cc: '157 PRESA DE JALES HERRADURA III', valor: 0},
                {div: 1, cc: '153 TERRACERIAS HERRADURA PLD-2', valor: 0},
                {div: 1, cc: '159 PATIOS NOCHE BUENA VII', valor: 0},
                {div: 1, cc: '160 HERRADURA XIII', valor: 0},
                {div: 2, cc: 'C63 CDI ZONA BAJIO', valor: 0},
                {div: 2, cc: 'C59 WALBRIDGE BAJIO', valor: 0},
                {div: 2, cc: '44 WAREHOUSE GRUPO MODELO', valor: 0},
                {div: 2, cc: '43 SILOS GRUPO MODELO', valor: 0},
                {div: 2, cc: '559 VARIAS INDUSTRIAS DEL NOROESTE', valor: 0},
                {div: 2, cc: 'C60 MAZDA', valor: 0},
                {div: 2, cc: 'C70 CDI AUTOMOTRIZ', valor: 0},
                {div: 2, cc: 'C68 CDI ALIMENTOS', valor: 0},
                {div: 2, cc: '40 CDI CULIACAN', valor: 0},
                {div: 2, cc: 'C72 SECTOR AUTOMOTRIZ NORESTE', valor: 0},
                {div: 2, cc: '560 FORD CCM CHIHUAHUA', valor: 0},
                {div: 2, cc: '542 FORD CCM', valor: 0},
                {div: 3, cc: '218 EDIFICIO C5', valor: 0},
                {div: 3, cc: '214 CANAL DE DESCARGA OHL', valor: 0},
                {div: 3, cc: '221 PAVIMENTACION QUINTERO ARCE', valor: 0},
            ];
        function init(){
            initElementos();
            initTblObra();
            selDivision.change(nombreDivision);
            radioBtn.on('click', function (){aClick(this);});
        }
        function initElementos(){
            selDivision.fillCombo('/Kubrix/Analisis/getCboDivision', null, false, "0");
        }
        function nombreDivision(){
            txtDivision.val(selDivision.find(':selected').data().prefijo);
            initTblObra();
        }
        function aClick(esto){
            let sel = $(esto).data('title');
            let tog = $(esto).data('toggle');
            $('#' + tog).prop('value', sel);
            $('a[data-toggle="' + tog + '"]').not('[data-title="' + sel + '"]').removeClass('active').addClass('notActive');
            $('a[data-toggle="' + tog + '"][data-title="' + sel + '"]').removeClass('notActive').addClass('active');
        }
        function initTblObra(){
        }
        init();
    }
    $(document).ready(function () {
        Kubrix.Analisis.Division = new Division();
    });
})();