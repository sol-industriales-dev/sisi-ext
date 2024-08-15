(() => {
    $.namespace('Administrativo.RecursosHumanos.MontoAdministrativo');
    MontoAdministrativo = function () {

        itemPeriodo = null;
        const selCC = $('#selCC');
        const buscar = $('.buscar');
        const tblBonos = $('#tblBonos');
        const selEstado = $('#selEstado');
        const getLstGestionBono = new URL(window.location.origin + '/Administrativo/Bono/getLstGestionBono');
        const BonoAdministrativo = new URL(window.location.origin + '/Administrativo/Bono/BonoAdministrativo');

        const divPlantillas = $('#divPlantillas');

        const botonAtras = $('#botonAtras');

        const tabComparacion = $("#tabComparacion");
        const tabReporte = $("#tabReporte");
        const divAutorizacion = $('#divAutorizacion');
        const tablaPlantillaActual = $('#tablaPlantillaActual');
        let dtTablaPlantillaActual;
        const tablaPlantillaNueva = $('#tablaPlantillaNueva');
        let dtTablaPlantillaNueva;
        const fieldsetReporte = $("#fieldsetReporte");
        const fieldsetAutorizacion = $("#fieldsetAutorizacion");
        const divAutorizantes = $('#divAutorizantes');
        const legendHome = $("#legendHome");
        const report = $("#report");
        const textAreaRechazo = $('#textAreaRechazo');
        const modalAutorizar = $('#modalAutorizar');
        const modalRechazar = $('#modalRechazar');
        const modalAutorizarbtnRechazar = $('#modalAutorizarbtnRechazar');
        const modalAutorizarbtnAutorizar = $('#modalAutorizarbtnAutorizar');


        //     urlLstAuth: '/Administrativo/Bono/getLstAuth',
        //     urlAuth: '/Administrativo/Bono/AutorizarBonoAdministrativo',
        //     urlRech: '/Administrativo/Bono/RechazarBonoAdministrativo',



        let init = () => {
            //buscar.change(setLstGestionBono);
            initForm();
        }

        async function setLstGestionBono() {
            try {
                dtBonos.clear().draw();
                response = await ejectFetchJson(getLstGestionBono, {
                    cc: selCC.val()
                    , st: selEstado.val()
                });
                if (response.success) {
                    dtBonos.rows.add(response.lst).draw();
                } else {
                    AlertaGeneral(`Erro`, `Ocurrió un error al buscar las plantillas de bonos.`);
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message); }
        }

        function initDataTblBonMontos() {
            dtBonos = tblBonos.DataTable({
                destroy: true,
                language: dtDicEsp,
                columns: [
                    { title: "Centro Costos", data: 'ccNombre', width: "35%" }
                    , { title: "Captura", data: 'fechaCaptura', width: "10%" }
                    , { title: "Capturó", data: 'usuarioCapturoNombre', width: "20%" }
                    , { title: "Estado", data: 'estatusDesc', width: "6%" }
                    , {
                        title: "Detalle", data: 'estatus', width: "5%", createdCell: function (td, data, rowData, row, col) {
                            let btn = $("<button>")
                                , ico = $("<i>")
                                , clase = "";
                            switch (data) {
                                case 0: clase = "btn-default"; break;
                                case 1: clase = "btn-success"; break;
                                case 2: clase = "btn-danger"; break;
                                case 3: clase = "btn-primary"; break;
                                default:
                                    break;
                            }
                            btn.addClass(`btn ${clase} det`);
                            ico.addClass("fa fa-print");
                            btn.html(ico);
                            $(td).html(btn);
                        }
                    }
                    , {
                        title: "Editar", data: 'id', width: "5%", createdCell: function (td, data, rowData, row, col) {
                            if(rowData.estatusDesc=="En Espera"){
                                let btn = $("<button>")
                                    , ico = $("<i>");
                                btn.addClass('btn btn-default edit');
                                ico.addClass("fa fa-edit");
                                btn.html(ico);
                                $(td).html(btn);
                            }
                            else{
                                $(td).html("");
                            }
                        }
                    }
                ]
                , initComplete: function (settings, json) {
                    tblBonos.on("click", ".edit", function () {
                        let data = dtBonos.row($(this).closest("tr")).data();
                        localStorage.setItem("cc", data.cc);
                        window.open(BonoAdministrativo, '_self');
                    });
                    tblBonos.on("click", ".det", function () {
                        let data = dtBonos.row($(this).closest("tr")).data();

                        obtenerListaAutorizantes(data.id);
                    });
                }
            });
        }

        function obtenerListaAutorizantes(id) {
            $.blockUI({ message: 'Cargando datos...' });
            $.get('/Administrativo/Bono/getLstAuth', { id })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        verReporte(id);
                        establecerPaneles(response.autorizantes, id);

                        dtTablaPlantillaActual.clear();
                        dtTablaPlantillaNueva.clear();

                        if (response.comparacion) {
                            dtTablaPlantillaActual.rows.add(response.comparacion.Item1).draw();
                            dtTablaPlantillaNueva.rows.add(response.comparacion.Item2).draw();
                        }

                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }


        function ocultarPaneles() {

            // fieldsetAutorizacion.hide(500);

            divAutorizacion.hide(500);

            fieldsetAutorizacion.data().plantillaID = null;
            // fieldsetReporte.hide(500);
            divAutorizantes.empty();

            dtTablaPlantillaActual.clear().draw();
            dtTablaPlantillaNueva.clear().draw();

            divPlantillas.prop('hidden', false);
            divPlantillas.show(500);
        }

        function establecerPaneles(listaAutorizantes, plantillaID) {

            divPlantillas.hide(500);
            fieldsetReporte.show(500);
            fieldsetAutorizacion.show(500);

            divAutorizacion.show(500);

            fieldsetAutorizacion.data().plantillaID = plantillaID;

            listaAutorizantes.forEach((autorizante, index) => {
                const divPanel = $(`
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <div class="panel panel-default text-center">
                            <div class="panel-heading"><label>${autorizante.nombre}</label></div>
                            <div class="panel-body">
                                <p>${autorizante.descripcion}</p>
                                <button ${autorizante.authEstado == 3 ? "id='botonAutorizar'" : ''} class='${autorizante.authEstado == 3 ? '' : 'hidden'} btn btn-success btnPanel'><i class='fa fa-check'></i> Autorizar</button>
                                <button ${autorizante.authEstado == 3 ? "id='botonRechazar'" : ''} class='${autorizante.authEstado == 3 ? '' : 'hidden'} btn btn-danger btnPanel'><i class='fa fa-ban'></i> Rechazar</button>
                            </div>
                            <div class="panel-footer ${autorizante.authEstado == 1 ? "panelAutorizado" : autorizante.authEstado == 2 ? "panelRechazado" : "panelPendiente"}">
                                <p>${autorizante.authEstado == 1 ? 'Autorizado' : autorizante.authEstado == 2 ? "Rechazado" : "Pendiente"}</p>
                                <p>${autorizante.firma ? autorizante.firma : "S/F"}</p>
                            </div>
                        </div>
                    </div>
                </div>
                `);

                divPanel.data().idRegistro = autorizante.idRegistro;

                divAutorizantes.append(divPanel);
            });

            $('#botonAutorizar').unbind().click(e => {
                modalAutorizar.modal('show');
                const registroID = $(e.currentTarget).parents('div.row').data().idRegistro;
                fieldsetAutorizacion.data().idRegistro = registroID;
            });

            $('#botonRechazar').unbind().click(e => {
                modalRechazar.modal('show');

                const registroID = $(e.currentTarget).parents('div.row').data().idRegistro;
                fieldsetAutorizacion.data().idRegistro = registroID;

                textAreaRechazo.change(() =>
                    textAreaRechazo.val(sanitizeString(textAreaRechazo.val()))
                );
            });
        }

        function sanitizeString(str) {
            str = str.replace(/[^a-z0-9áéíóúñü \.,]/gim, "");
            return str.trim();
        }

        function autorizarBonoAdministrativo() {

            const plantillaID = fieldsetAutorizacion.data().plantillaID;
            const registroID = fieldsetAutorizacion.data().idRegistro;

            const auth = {
                idPadre: plantillaID,
                idRegistro: registroID
            };

            if (plantillaID && plantillaID > 0) {

                modalAutorizar.modal('hide');

                $.blockUI({ message: 'Autorizando...' });
                $.post('/Administrativo/Bono/AutorizarBonoAdministrativo', { auth })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            AlertaGeneral(`Aviso`, `Plantilla autorizada.`);
                            ocultarPaneles();
                            setLstGestionBono();
                        } else {
                            // Operación no completada.
                            AlertaGeneral(`Operación fallida`, `No se pudo completar la operación.`);
                        }
                    }, error => {
                        // Error al lanzar la petición.
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            }
        }

        function rechazarControlAsistencia() {

            const comentario = textAreaRechazo.val().trim();

            if (comentario == null || comentario.trim().length <= 10) {
                AlertaGeneral("Aviso", "El mensaje de rechazo debe tener un mínimo de 10 caracteres.");
                return;
            }

            const plantillaID = fieldsetAutorizacion.data().plantillaID;
            const registroID = fieldsetAutorizacion.data().idRegistro;

            const auth = {
                idPadre: plantillaID,
                idRegistro: registroID,
                comentario
            };

            modalRechazar.modal('hide');
            $.blockUI({ message: 'Rechazando paquete...' });
            $.post('/Administrativo/Bono/RechazarBonoAdministrativo', { auth })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AlertaGeneral(`Aviso`, `Plantilla rechazada.`);
                        ocultarPaneles();
                        setLstGestionBono();
                    } else {
                        AlertaGeneral(`Aviso`, `Ocurrió un error al intentar rechazar la plantilla`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function verReporte(plantillaID) {
            $.blockUI({ message: 'Cargando reporte...' });
            var path = `/Reportes/Vista.aspx?idReporte=169&fId=${plantillaID}&inMemory=1&isCRModal=${false}`;
            report.attr("src", path);
            report[0].onload = () => {
                $.unblockUI();
            }
        }

        function initTablaPlantillaActual() {
            dtTablaPlantillaActual = tablaPlantillaActual.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                columns: [
                    { data: 'puesto', title: 'Puesto' },
                    { data: 'periocidadDesc', title: 'Periocidad' },
                    { data: 'monto', title: 'Monto', render: data => `${maskNumero(data)}` },
                ],
                createdRow: function (row, data) {
                    $(row).addClass(data.clase);
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaPlantillaNueva() {
            dtTablaPlantillaNueva = tablaPlantillaNueva.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                columns: [
                    { data: 'puesto', title: 'Puesto' },
                    { data: 'periocidadDesc', title: 'Periocidad' },
                    { data: 'monto', title: 'Monto', render: data => `${maskNumero(data)}` },
                ],
                createdRow: function (row, data) {
                    $(row).addClass(data.clase);
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }
        function mostrarAutorizaciones()
        {
            $("a[href='#tabComparacion']").on('show.bs.tab', function(e) {
                fieldsetAutorizacion.hide();
            });
            $("a[href='#tabReporte']").on('show.bs.tab', function(e) {
                fieldsetAutorizacion.show();
            });
        }

        function initForm() {
            selCC.fillCombo('/Administrativo/Bono/getTblP_CC', null, false, "Todos");
            selEstado.fillCombo('/Administrativo/Bono/getcboAuthEstado', null, false, "Todos");
            selEstado.val(0);
            initDataTblBonMontos();
            setLstGestionBono();

            legendHome.click(ocultarPaneles);
            botonAtras.click(ocultarPaneles);

            modalAutorizarbtnAutorizar.unbind().click(autorizarBonoAdministrativo);
            modalAutorizarbtnRechazar.unbind().click(rechazarControlAsistencia);

            initTablaPlantillaActual();
            initTablaPlantillaNueva();
            
            divAutorizacion.hide();
            mostrarAutorizaciones();
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.RecursosHumanos.MontoAdministrativo = new MontoAdministrativo();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();
