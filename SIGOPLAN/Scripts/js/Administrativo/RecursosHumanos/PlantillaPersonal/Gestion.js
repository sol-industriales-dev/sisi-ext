(() => {
    $.namespace('sigoplan.rh.plantillapersonal');
    plantillapersonal = function () {
        _ID = 0;
        const capUser = $("#capUser");
        const cboEstatus = $("#cboEstatus");
        const modalAprobadores = $("#modalAprobadores");
        tblAprobaciones = $("#tblAprobaciones");
        tblData = $("#tblData");
        const AutorizarPlantilla = new URL(window.location.origin + '/Administrativo/PlantillaPersonal/AutorizarPlantilla');
        const EnviarCorreo = new URL(window.location.origin + '/Administrativo/PlantillaPersonal/EnviarCorreo');
        function init() {
            initTable();
            cboEstatus.change(fnCargar);
            if ($.urlParam('autID') != null) {
                fnDetalle($.urlParam('autID'));
            }
        }
        function initTable() {
            tblAprobaciones = $("#tblAprobaciones").DataTable({
                ajax: {
                    url: '/Administrativo/PlantillaPersonal/GetAutorizadores',
                    dataSrc: 'dataMain',
                    data: function (d) { d.plantillaID = _ID }
                },
                columns: [
                    { data: 'id' },
                    { data: 'nombre' },
                    { data: 'tipo' },
                    { data: 'puesto' },
                    { data: 'estatus' },
                    { data: 'firma' }
                ],
                columnDefs: [ { targets: 0, "visible": false } ],
                drawCallback: function (settings) {
                    var autorizar = $(".clsAutorizar");
                    var rechazar = $(".clsRechazar");
                    $.each(autorizar, function (i, e) {
                        $(this).click(clickAutorizar);
                    });
                    $.each(rechazar, function (i, e) {
                        $(this).click(clickRechazar);
                    });
                },
                rowCallback: function (row, data) {
                    capUser.html(data.capturo);
                }
            });
            tblData = $("#tblData").DataTable({
                ajax: {
                    url: '/Administrativo/PlantillaPersonal/GetPlantillas',
                    dataSrc: 'dataMain',
                    data: function (d) {
                        d.cc = "",
                        d.estatus = cboEstatus.val()
                    }
                },
                columns: [
                    { data: 'id' },
                    { data: 'cc' },
                    { data: 'fechaInicio' },
                    { data: 'fechaFin' },
                    { data: 'fechaRegistro' },
                    { data: 'autorizar', width: "50px" },
                    { data: 'reporte', width: "50px" }
                ],
                columnDefs: [ { targets: 0, "visible": false } ],
                drawCallback: function (settings) {
                    var detalle = $(".clsDetalle");
                    var reporte = $(".clsReporte");
                    $.each(detalle,function(i,e){
                        $(this).click(clickDetalle);
                    });
                    $.each(reporte, function (i, e) {
                        $(this).click(clickReporte);
                    });
                }
            });
        }
        function clickDetalle() {
            _ID = $(this).data("id");
            tblAprobaciones.ajax.reload(null, false);
            modalAprobadores.modal("show");
        }
        function fnDetalle(id)
        {
            _ID = id;
            tblAprobaciones.ajax.reload(null, false);
            modalAprobadores.modal("show");
        }
        function clickReporte() {
            $.blockUI({ message: "Cargando información..." });
            var path = `/Reportes/Vista.aspx?idReporte=104&plantillaID=${$(this).data("id")}&pendiente=${cboEstatus.val()==1}`;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }
        function clickAutorizar() {
            var id = $(this).data("id");
            fnSetEstatus(2,id);
        }
        function clickRechazar() {
            var id = $(this).data("id");
            fnSetEstatus(3,id);
        }
        async function fnSetEstatus(estatus, id) {
            try {
                modalAprobadores.block({ message: null }); 
                response = await ejectFetchJson(AutorizarPlantilla, { plantillaID: _ID, autorizacion: id, estatus: estatus });
                if (response.success) {
                    generarReporte().then(async () => {
                        responseCorreo = await ejectFetchJson(EnviarCorreo, { plantillaID: _ID, autorizacion: id, estatus: estatus });
                        if (responseCorreo.success) {
                            tblData.ajax.reload(null, false);                                    
                            $("#modalAprobadores .close").click();
                            modalAprobadores.unblock();
                            AlertaGeneral("Confirmación", "¡Registro actualizado correctamente!");
                        }
                    });                        
                };
            }
            catch (o_O) {
                modalAprobadores.unblock();
                $(".modal-backdrop").remove();
            }
        }
        function generarReporte () {
            return new Promise((resolve, reject) => {
                $("#report").attr("src", `/Reportes/Vista.aspx?idReporte=104&plantillaID=${_ID}&inMemory=1`);
                document.getElementById('report').onload = () => resolve();
              });
        }
        function fnCargar() {
            tblData.ajax.reload(null, false);
        }
        init();
    };
    $(document).ready(function () {
        sigoplan.rh.plantillapersonal = new plantillapersonal();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();
