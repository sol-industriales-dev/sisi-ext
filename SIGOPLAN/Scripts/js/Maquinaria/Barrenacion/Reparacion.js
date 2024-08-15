(() => {
    $.namespace('Barrenacion.Reparacion');
    Reparacion = function () {
        const tablaPiezas = $('#tablaPiezas');
        const mdlAceptarRechazar = $('#mdlAceptarRechazar');
        let dtTablaPiezas, idRepara, idRechaza;
        const setAceptarPieza = new URL(window.location.origin + '/Barrenacion/setAceptarPieza');
        const setRechazoPieza = new URL(window.location.origin + '/Barrenacion/setRechazoPieza');
        const ObtenerPiezasPorReparar = new URL(window.location.origin + '/Barrenacion/ObtenerPiezasPorReparar');
        (function init() {
            setIdDefault();
            initTablaPiezas();
            cargarPiezasPorReparar();
            mdlAceptarRechazar.on('hidden.bs.modal', () => setIdDefault());
        })();
        function initTablaPiezas() {
            dtTablaPiezas = tablaPiezas.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                columns: [
                    { data: 'noSerie', title: 'Número de Serie' },
                    { data: 'economico', title: 'No. Económico' },
                    { data: 'descTipoPieza', title: 'Pieza' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'cc', title: 'AC' },
                    { data: 'metrosLineales', title: 'Metros Lineales' },
                    { data: 'id', render: (data, type, row) => `<button class="btn btn-success reparar"><i class="fas fa-tools"></i> Reparar</button>` },
                    { data: 'id', render: (data, type, row) => `<button class="btn btn-danger deshechar"><i class="fas fa-trash"></i> Deshechar</button>` }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                drawCallback: function (settings) {
                    tablaPiezas.find('.reparar').click(function () {
                        let data = dtTablaPiezas.row($(this).parents('tr')).data();
                        idRepara = data.id;
                        AlertaAceptarRechazarNormal("Aviso", `¿Desea reparar la pieza ${data.descTipoPieza} de la barrenadora ${data.cc}-${data.economico}?`, aceptaReparacionPieza, setIdDefault);
                    });
                    tablaPiezas.find('.deshechar').click(function () {
                        let data = dtTablaPiezas.row($(this).parents('tr')).data();
                        idRechaza = data.id;
                        AlertaAceptarRechazarNormal("Aviso", `<p>¿Desea dar de baja a la pieza ${data.descTipoPieza} de la barrenadora ${data.cc}-${data.economico}?</p><textarea rows="4" cols="70" class="form-control comentarioRechazo"></textarea>`, aceptaRechazoPieza, setIdDefault);
                    });
                }
            });
        }
        async function aceptaReparacionPieza() {
            try {
                if (idRepara > 0) {
                    response = await ejectFetchJson(setAceptarPieza, { idRepara });
                    if (response.success) {
                        cargarPiezasPorReparar();
                        AlertaGeneral("Aviso", "Pieza reparada con éxito.");
                    }
                    else { AlertaGeneral("Aviso", "No fue posible reparar la pieza. Contacte con el departamento de TI."); }
                }
            } catch (o_O) { AlertaGeneral("Aviso", "No fue posible reparar la pieza. Contacte con el departamento de TI."); }
        }
        async function aceptaRechazoPieza() {
            try {
                let comentario = $(".comentarioRechazo").val()
                    , esvalido = idRechaza > 0 && comentario.length > 3;
                if (esvalido) {
                    response = await ejectFetchJson(setRechazoPieza, { idRechaza, comentario });
                    if (response.success) {
                        cargarPiezasPorReparar();
                        AlertaGeneral("Aviso", "La pieza ah sido deshechada.");
                    }
                    else { AlertaGeneral("Aviso", "No fue posible reparar la pieza. Contacte con el departamento de TI."); }
                } else { AlertaGeneral("Aviso", "Escriba un motivo de rechazo, porfavor."); }
            } catch (o_O) { AlertaGeneral("Aviso", "No fue posible reparar la pieza. Contacte con el departamento de TI."); }
        }
        async function cargarPiezasPorReparar() {
            try {
                dtTablaPiezas.clear().draw();
                response = await ejectFetchJson(ObtenerPiezasPorReparar);
                if (response.success) {
                    dtTablaPiezas.rows.add(response.items).draw();
                } else {
                    AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                }
            } catch (o_O) { AlertaGeneral(`Operación fallida`, o_O.message) }
        }
        function setIdDefault() {
            idRepara = 0;
            idRechaza = 0;
        }
    }

    $(() => Barrenacion.Reparacion = new Reparacion())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();