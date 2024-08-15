(() => {
    $.namespace('Administrativo.RecursosHumanos.MontoAdministrativo');
    MontoAdministrativo = function () {

        itemPeriodo = null;
        const selCC = $('#selCC');
        const buscar = $('.buscar');
        const tblData = $('#tblData');
        const selEstado = $('#selEstado');
        const getLstGestion = new URL(window.location.origin + '/Administrativo/Bono/getLstGestionEvaluacion');
        const Evaluacion = new URL(window.location.origin + '/Administrativo/Bono/Evaluacion');
        const ccNotificacion = $('#ccNotificacion');

        const divPlantillas = $('#divPlantillas');

        const botonAtras = $('#botonAtras');

        const tabComparacion = $("#tabComparacion");
        const tabReporte = $("#tabReporte");
        const divAutorizacion = $('#divAutorizacion');
        const tablaActual = $('#tablaActual');
        let dttablaActual;
        const tablaNueva = $('#tablaNueva');
        let dttablaNueva;
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
            buscar.change(setLstGestion);
            initForm();
        }

        async function setLstGestion() {
            try {
                dtBonos.clear().draw();
                response = await ejectFetchJson(getLstGestion, {
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
            dtBonos = tblData.DataTable({
                destroy: true,
                language: dtDicEsp,
                columns: [
                    { title: "Centro Costos", data: 'ccNombre' }
                    , { title: "Mes", data: 'mes' }
                    , { title: "Nomina", data: 'nomina' }
                    , { title: "Periodo", data: 'periodo', width: "15%" }
                    , { title: "Fecha Evaluación", data: 'fechaCaptura' }
                    , { title: "Evaluador", data: 'usuarioCapturoNombre' }
                    , { title: "Estado", data: 'estatusDesc' }
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
                            let btn = $("<button>")
                                , ico = $("<i>");
                            btn.addClass('btn btn-default edit');
                            ico.addClass("fa fa-edit");
                            btn.html(ico);
                            $(td).html(btn);
                        }
                    }
                ]
                , initComplete: function (settings, json) {
                    tblData.on("click", ".edit", function () {
                        let data = dtBonos.row($(this).closest("tr")).data();
                        localStorage.setItem("cc", data.cc);
                        localStorage.setItem('nomina', data.tipoNomina);
                        localStorage.setItem('periodo', data.periodoInt);
                        window.open(Evaluacion, '_self');
                    });
                    tblData.on("click", ".det", function () {
                        let data = dtBonos.row($(this).closest("tr")).data();

                        obtenerListaAutorizantes(data.id);
                    });
                }
            });
            setLstGestion();
        }

        function obtenerListaAutorizantes(id) {
            $.blockUI({ message: 'Cargando datos...' });
            $.get('/Administrativo/Bono/getLstAuthEvaluacion', { id })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        verReporte(id);
                        establecerPaneles(response.autorizantes, id);

                        dttablaActual.clear();
                        dttablaNueva.clear();

                        if (response.comparacion) {
                            dttablaActual.rows.add(response.comparacion.Item1).draw();
                            dttablaNueva.rows.add(response.comparacion.Item2).draw();
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

            fieldsetAutorizacion.data().evaluacionID = null;
            // fieldsetReporte.hide(500);
            divAutorizantes.empty();

            dttablaActual.clear().draw();
            dttablaNueva.clear().draw();

            divPlantillas.prop('hidden', false);
            divPlantillas.show(500);
        }

        function establecerPaneles(listaAutorizantes, evaluacionID) {

            divPlantillas.hide(500);
            fieldsetReporte.show(500);
            fieldsetAutorizacion.show(500);

            divAutorizacion.show(500);

            fieldsetAutorizacion.data().evaluacionID = evaluacionID;

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

            const evaluacionID = fieldsetAutorizacion.data().evaluacionID;
            const registroID = fieldsetAutorizacion.data().idRegistro;

            const auth = {
                idPadre: evaluacionID,
                idRegistro: registroID
            };

            if (evaluacionID && evaluacionID > 0) {

                modalAutorizar.modal('hide');

                $.blockUI({ message: 'Autorizando...' });
                $.post('/Administrativo/Bono/AutorizarEvaluacion', { auth })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            AlertaGeneral(`Aviso`, `Evaluación autorizada.`);
                            ocultarPaneles();
                            setLstGestion();
                            verReporteCorreo(evaluacionID);
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

            const evaluacionID = fieldsetAutorizacion.data().evaluacionID;
            const registroID = fieldsetAutorizacion.data().idRegistro;

            const auth = {
                idPadre: evaluacionID,
                idRegistro: registroID,
                comentario: comentario
            };

            modalRechazar.modal('hide');
            $.blockUI({ message: 'Rechazando paquete...' });
            $.post('/Administrativo/Bono/RechazarEvaluacion', { auth })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AlertaGeneral(`Aviso`, `Evaluación rechazada.`);
                        ocultarPaneles();
                        setLstGestion();
                        verReporteCorreo(evaluacionID);
                    } else {
                        AlertaGeneral(`Aviso`, `Ocurrió un error al intentar rechazar la evaluación`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function verReporte(evaluacionID) {
            $.blockUI({ message: 'Cargando reporte...' });
            var path = `/Reportes/Vista.aspx?idReporte=216&fId=${evaluacionID}&isCRModal=${false}`;
            report.attr("src", path);
            report[0].onload = () => {
                $.unblockUI();
            }
        }
        function verReporteCorreo(evaluacionID) {
            $.blockUI({ message: 'Enviando correo...' });
            var path = `/Reportes/Vista.aspx?idReporte=216&fId=${evaluacionID}&inMemory=1`;
            report.attr("src", path);
            report[0].onload = () => {
                $.unblockUI();
            }
        }
        function inittablaActual() {
            dttablaActual = tablaActual.DataTable({
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

        function inittablaNueva() {
            dttablaNueva = tablaNueva.DataTable({
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
        function mostrarAutorizaciones() {
            $("a[href='#tabComparacion']").on('show.bs.tab', function (e) {
                fieldsetAutorizacion.hide();
            });
            $("a[href='#tabReporte']").on('show.bs.tab', function (e) {
                fieldsetAutorizacion.show();
            });
        }

        function initForm() {
            if (ccNotificacion.val()) {
                selCC.fillCombo('/Administrativo/Bono/getTblP_CCconPlantilla', null, false, "Todos", () => {
                    $('#selCC').val(ccNotificacion.val());
                });
            } else {
                selCC.fillCombo('/Administrativo/Bono/getTblP_CCconPlantilla', null, false, "Todos");
            }
            selEstado.fillCombo('/Administrativo/Bono/getcboAuthEstado', null, false, "Todos");
            selEstado.val(0);
            initDataTblBonMontos();


            legendHome.click(ocultarPaneles);
            botonAtras.click(ocultarPaneles);

            modalAutorizarbtnAutorizar.unbind().click(autorizarBonoAdministrativo);
            modalAutorizarbtnRechazar.unbind().click(rechazarControlAsistencia);

            inittablaActual();
            inittablaNueva();

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
