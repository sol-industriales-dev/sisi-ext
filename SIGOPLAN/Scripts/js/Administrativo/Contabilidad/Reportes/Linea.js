(function () {
    $.namespace('Administrativo.Contabilidad.Reportes.Linea');
    Linea = function (){
        selBanco = $("#selBanco");
        selFactoraje = $("#selFactoraje");
        selMoneda = $("#selMoneda");
        txtLinea = $("#txtLinea");
        dpFecha = $("#dpFecha");
        btnGuardar = $("#btnGuardar");
        tblLinea = $("#tblLinea");
        
        function init() {
            initForm();
            btnGuardar.click(setLinea);
            txtLinea.change(setMonto);
        }
        function initForm(){
            selBanco.val("");
            selFactoraje.val("");
            selMoneda.val(0);
            txtLinea.val(maskDinero(0));
            dpFecha.datepicker().datepicker("setDate", new Date());
            initTable();
        }
        function initTable() {
            tabaLinea = tblLinea.DataTable({
                destroy: true,
                searching: true,
                paging: true,
                language: dtDicEsp,
                ajax: { url: '/Administrativo/Reportes/getLstLinea'},
                columnDefs: [
                    { targets: 0, data: 'banco' },
                    { targets: 1, data: 'factoraje' },
                    { targets: 2, data: 'moneda' },
                    { targets: 3, data: 'linea', className: "text-right" },
                    { targets: 4, data: 'fecha', className: "text-right"  }
                ],
                initComplete: function (settings, json) {
                    tabaLinea.on('click', 'tr', function () {
                        let data = tabaLinea.row($(this)).data()
                        selBanco.val(data.banco);
                        selFactoraje.val(data.factoraje[0]);
                        selMoneda.val(data.tipoMoneda);
                        txtLinea.val(data.linea);
                        dpFecha.val(data.fecha);
                    });
                }
            });
        }
        function setLinea(){
            if (selBanco.val() != "") {
                let res = $.post("/Administrativo/Reportes/setLinea",{
                    obj:{
                        banco: selBanco.val(),
                        factoraje: selFactoraje.val(),
                        moneda: selMoneda.val(),
                        linea: unmaskDinero(txtLinea.val()),
                        fecha: dpFecha.val()
                    }
                });
                res.done(function(response){
                    if (response.success) {
                        AlertaGeneral("Aviso", "Línea de banco actualizado");
                        initForm();
                    }
                });   
            }
            else
            AlertaGeneral("Aviso", "Necesita selecionar un baco");   
        }
        function setMonto(){
            let monto = unmaskDinero(txtLinea.val());
            txtLinea.val(maskDinero(monto));
        }
        function maskDinero(numero) {
            return "$" + parseFloat(numero).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        }
        function unmaskDinero(dinero) {
            return Number(dinero.replace(/[^0-9\.]+/g, ""));
        }
        init();
    }
    $(document).ready(function () {
        Administrativo.Contabilidad.Reportes.Linea = new Linea();
    })
    .ajaxStart(function () { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(function () { $.unblockUI(); });
})();