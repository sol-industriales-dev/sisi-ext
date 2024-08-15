(() => {
    $.namespace('Administrativo.RecursosHumanos.MontoAdministrativo');
    MontoAdministrativo = function () {
        //#region SELECTORES
        const buscar = $('.buscar');
        const comboNomina = $("#comboNomina");
        const comboCC = $("#comboCC");
        const comboEstado = $("#comboEstado");
        const botonExcelDetalle = $("#botonExcelDetalle");
        const tablaGestion = $('#tablaGestion');
        const divPrenominas = $('#divPrenominas');
        const divFiltros = $('#divFiltros');
        const botonAtras = $('#botonAtras');
        const tabComparacion = $("#tabComparacion");
        const tabReporte = $("#tabReporte");
        const divAutorizacion = $('#divAutorizacion');
        const fieldsetReporte = $("#fieldsetReporte");
        const fieldsetAutorizacion = $("#fieldsetAutorizacion");
        const divAutorizantes = $('#divAutorizantes');
        const legendHome = $("#legendHome");
        const report = $("#report");
        const report2 = $("#report2");
        const textAreaRechazo = $('#textAreaRechazo');
        const modalAutorizar = $('#modalAutorizar');
        const modalRechazar = $('#modalRechazar');
        const modalAutorizarbtnRechazar = $('#modalAutorizarbtnRechazar');
        const modalAutorizarbtnAutorizar = $('#modalAutorizarbtnAutorizar');
        const botonSolicitudCheque = $('#botonSolicitudCheque');
        const modalSolicitudCheque = $('#modalSolicitudCheque');
        const selectBanco = $('#selectBanco');
        const botonGenerarSolicitudCheque = $('#botonGenerarSolicitudCheque');
        //#endregion

        const GetCbotPeriodoNomina = originURL('/Administrativo/Nomina/GetCbotPeriodoNomina');
        const FillCboCC = originURL('/Administrativo/Nomina/GetCCsIncidencias');
        const GetEstadosAutotizacion = new URL(window.location.origin + '/Administrativo/Nomina/GetEstadosAutotizacion');
        const GetLstGestionPrenomina = new URL(window.location.origin + '/Administrativo/Nomina/GetLstGestionPrenomina');
        const cboTipoNomina = $('#cboTipoNomina');


        itemPeriodo = null;

        let init = () => {
            buscar.change(setLstGestionPrenomina);
            initForm();
            LlenarCombos();
            comboNomina.change();
            botonSolicitudCheque.click(() => { modalSolicitudCheque.modal('show'); });
            botonGenerarSolicitudCheque.click(imprimirSolicitudCheque);
            setLstGestionPrenomina();
        }

        function LlenarCombos() {
            comboNomina.fillComboGroup(GetCbotPeriodoNomina, { tipoNomina: cboTipoNomina.val() }, false, undefined, function () {
                comboNomina.change();
            });
            comboNomina.change(function (e) {
                const dataPeriodo = comboNomina.find('option:selected').data('prefijo').split('-');
                tipoPeriodo = cboTipoNomina.val();
                periodo = comboNomina.val();
                anio = dataPeriodo[3];
                comboCC.fillComboGroupSelectable(FillCboCC, { periodo: periodo, tipoNomina: tipoPeriodo, anio: anio }, false, "--Seleccione--");
            });
            comboCC.change(function (e) {
                $("#select2-comboCC-container").removeClass('validada');
                $("#select2-comboCC-container").removeClass('no-validada');
                $("#select2-comboCC-container").addClass($("#comboCC option:selected")[0].className);
                $(".select2-selection--single").removeClass('validada');
                $(".select2-selection--single").removeClass('no-validada');
                $(".select2-selection--single").addClass($("#comboCC option:selected")[0].className);
            });
            comboEstado.fillCombo(GetEstadosAutotizacion, null, false, "Todos");
            cboTipoNomina.select2();
            cboTipoNomina.change(function (e)
            {
                comboNomina.fillComboGroup(GetCbotPeriodoNomina, { tipoNomina: cboTipoNomina.val() }, false, undefined, function () {
                    //comboNomina.prop("selectedIndex", 1);
                    comboNomina.change();
                });
            });
        }


        function setLstGestionPrenomina()
        {
            $.blockUI({ message: 'Cargando datos...' });
            var tipoPeriodo = 0;
            var periodo = 0;
            var anio = 0;
            if (comboNomina.val()) {
                const dataPeriodo = comboNomina.find('option:selected').data('prefijo').split('-');
                tipoPeriodo = cboTipoNomina.val();
                periodo = comboNomina.val();
                anio = dataPeriodo[3];
            }
            dtTablaGestion.clear().draw();
            $.get('/Administrativo/Nomina/GetLstGestionPrenomina', { 
                CC: comboCC.val() == null ? "" : comboCC.val(),
                periodo: periodo,
                tipoNomina: tipoPeriodo,
                anio: anio,
                estatus: comboEstado.val() 
            })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        dtTablaGestion.clear().draw();
                        dtTablaGestion.rows.add(response.lst).draw();

                    } else {
                        // Operación no completada.
                        dtTablaGestion.clear().draw();
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al buscar las prenominas.`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
            );
        }

        //async function setLstGestionPrenomina() {
        //    try {
        //        var tipoPeriodo = 0;
        //        var periodo = 0;
        //        var anio = 0;
        //        if (comboNomina.val()) {
        //            const dataPeriodo = comboNomina.find('option:selected').data('prefijo').split('-');
        //            tipoPeriodo = cboTipoNomina.val();
        //            periodo = comboNomina.val();
        //            anio = dataPeriodo[3];
        //        }
        //        dtTablaGestion.clear().draw();
        //        response = await ejectFetchJson(GetLstGestionPrenomina, {
        //            CC: comboCC.val() == null ? "" : comboCC.val(),
        //            periodo: periodo,
        //            tipoNomina: tipoPeriodo,
        //            anio: anio,
        //            estatus: comboEstado.val()
        //        });
        //        if (response.success) {
        //            dtTablaGestion.clear().draw();
        //            dtTablaGestion.rows.add(response.lst).draw();
        //        } else {
        //            dtTablaGestion.clear().draw();
        //            AlertaGeneral(`Error`, `Ocurrió un error al buscar las prenominas.`);
        //        }
        //    } catch (o_O) { AlertaGeneral("Aviso", o_O.message); }
        //}

        function initTablaGestion() {
            dtTablaGestion = tablaGestion.DataTable({
                destroy: true,
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                ordering: false,
                columnDefs: [
                    { className: "dt-center", targets: [0, 1, 2, 3, 4, 5, 6, 7] },
                ],
                columns: [
                    { title: "Centro<br>Costos", data: 'ccNombre', width: "35%" }
                    , { title: "Tipo<br>Nómina", data: 'tipoNominaDesc', width: "10%" }
                    , { title: "Periodo", data: 'periodo', width: "10%" }
                    , { title: "Año", data: 'anio', width: "10%" }
                    , { title: "Captura", data: 'fechaCaptura', width: "10%" }
                    , { title: "Capturó", data: 'usuarioCapturoNombre', width: "20%" }
                    , { title: "Estado<br>Prenomina", data: 'estatusDesc', width: "6%" }
                    , { title: "Autorización<br>Usuario", data: 'estadoUsuarioDesc', width: "6%" }
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
                ]
                , initComplete: function (settings, json) {
                    tablaGestion.on("click", ".det", function () {
                        let data = dtTablaGestion.row($(this).closest("tr")).data();

                        obtenerListaAutorizantes(data.id);
                    });
                }
            });
        }

        function obtenerListaAutorizantes(prenominaID) {
            $.blockUI({ message: 'Cargando datos...' });
            $.get('/Administrativo/Nomina/GetListaAutorizantes', { prenominaID })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        verReporte(prenominaID);
                        establecerPaneles(response.autorizantes, prenominaID);

                        //dtTablaPlantillaActual.clear();
                        //dtTablaPlantillaNueva.clear();

                        //if (response.comparacion) {
                        //    dtTablaPlantillaActual.rows.add(response.comparacion.Item1).draw();
                        //    dtTablaPlantillaNueva.rows.add(response.comparacion.Item2).draw();
                        //}

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

            fieldsetAutorizacion.data().prenominaID = null;
            // fieldsetReporte.hide(500);
            divAutorizantes.empty();

            //dtTablaPlantillaActual.clear().draw();
            //dtTablaPlantillaNueva.clear().draw();

            divPrenominas.prop('hidden', false);
            divPrenominas.show(500);

            divFiltros.prop('hidden', false);
            divFiltros.show(500);
        }

        function establecerPaneles(listaAutorizantes, prenominaID) {

            divPrenominas.hide(500);
            divFiltros.hide(500);
            fieldsetReporte.show(500);
            fieldsetAutorizacion.show(500);

            divAutorizacion.show(500);

            fieldsetAutorizacion.data().prenominaID = prenominaID;

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

        function autorizarPrenomina() {

            const prenominaID = fieldsetAutorizacion.data().prenominaID;
            const registroID = fieldsetAutorizacion.data().idRegistro;

            const auth = {
                idPadre: prenominaID,
                idRegistro: registroID
            };

            if (prenominaID && prenominaID > 0) {

                modalAutorizar.modal('hide');

                $.blockUI({ message: 'Autorizando...' });
                $.post('/Administrativo/Nomina/AutorizarPrenomina', { auth })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            AlertaGeneral(`Aviso`, `Prenomina autorizada.`);
                            ocultarPaneles();
                            setLstGestionPrenomina();
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

        function rechazarPrenomina() {

            const comentario = textAreaRechazo.val().trim();

            if (comentario == null || comentario.trim().length <= 10) {
                AlertaGeneral("Aviso", "El mensaje de rechazo debe tener un mínimo de 10 caracteres.");
                return;
            }

            const prenominaID = fieldsetAutorizacion.data().prenominaID;
            const registroID = fieldsetAutorizacion.data().idRegistro;

            const auth = {
                idPadre: prenominaID,
                idRegistro: registroID,
                comentario
            };

            modalRechazar.modal('hide');
            $.blockUI({ message: 'Rechazando paquete...' });
            $.post('/Administrativo/Nomina/RechazarPrenomina', { auth })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AlertaGeneral(`Aviso`, `Prenomina rechazada.`);
                        ocultarPaneles();
                        setLstGestionPrenomina();
                    } else {
                        AlertaGeneral(`Aviso`, `Ocurrió un error al intentar rechazar la prenomina`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function verReporte(prenominaID) {
            $.blockUI({ message: 'Cargando reporte...' });
            var path = `/Reportes/Vista.aspx?idReporte=221&fId=${prenominaID}&inMemory=1&isCRModal=${false}`;
            report2.attr("src", path);
            report2[0].onload = () => {
                $.unblockUI();
            }
        }

        function imprimirSolicitudCheque() {
            let periodo = comboNomina.val();
            let dataPeriodo = comboNomina.find('option:selected').data('prefijo').split('-');
            let tipoNomina = dataPeriodo[2];
            let year = dataPeriodo[3];
            let banco = selectBanco.val();

            if (periodo <= 0 || isNaN(periodo)) {
                AlertaGeneral(`Alerta`, `Debe seleccionar un periodo.`);
                return;
            }

            $.blockUI({ message: 'Generando solicitud de cheque...' });
            var path = `/Reportes/Vista.aspx?idReporte=248&year=${year}&periodo=${periodo}&tipoNomina=${tipoNomina}&banco=${banco}`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
                modalSolicitudCheque.modal('hide');
            };
        }

        //function initTablaPlantillaActual() {
        //    dtTablaPlantillaActual = tablaPlantillaActual.DataTable({
        //        language: dtDicEsp,
        //        destroy: true,
        //        paging: false,
        //        searching: false,
        //        columns: [
        //            { data: 'puesto', title: 'Puesto' },
        //            { data: 'periocidadDesc', title: 'Periocidad' },
        //            { data: 'monto', title: 'Monto', render: data => `${maskNumero(data)}` },
        //        ],
        //        createdRow: function (row, data) {
        //            $(row).addClass(data.clase);
        //        },
        //        columnDefs: [
        //            { className: "dt-center", "targets": "_all" }
        //        ]
        //    });
        //}

        //function initTablaPlantillaNueva() {
        //    dtTablaPlantillaNueva = tablaPlantillaNueva.DataTable({
        //        language: dtDicEsp,
        //        destroy: true,
        //        paging: false,
        //        searching: false,
        //        columns: [
        //            { data: 'puesto', title: 'Puesto' },
        //            { data: 'periocidadDesc', title: 'Periocidad' },
        //            { data: 'monto', title: 'Monto', render: data => `${maskNumero(data)}` },
        //        ],
        //        createdRow: function (row, data) {
        //            $(row).addClass(data.clase);
        //        },
        //        columnDefs: [
        //            { className: "dt-center", "targets": "_all" }
        //        ]
        //    });
        //}
        function mostrarAutorizaciones() {
            $("a[href='#tabComparacion']").on('show.bs.tab', function (e) {
                fieldsetAutorizacion.hide();
            });
            $("a[href='#tabReporte']").on('show.bs.tab', function (e) {
                fieldsetAutorizacion.show();
            });
        }

        function DescargaExcelDetalle() {
            var prenominaID = fieldsetAutorizacion.data().prenominaID;
            if (prenominaID > 0) {
                $(this).download = '/Nomina/CrearExcelPrenomina?prenominaID=' + prenominaID;
                $(this).href = '/Nomina/CrearExcelPrenomina?prenominaID=' + prenominaID;
                location.href = '/Nomina/CrearExcelPrenomina?prenominaID=' + prenominaID;
            }
            else {
                swal('Alerta!', "No se ha cargado prenomina", 'warning');
            }
        }

        function initForm() {
            //selCC.fillCombo('/Administrativo/Bono/getTblP_CC', null, false, "Todos");
            //selEstado.fillCombo('/Administrativo/Bono/getcboAuthEstado', null, false, "Todos");
            //selEstado.val(0);
            initTablaGestion();
            setLstGestionPrenomina();

            legendHome.click(ocultarPaneles);
            botonAtras.click(ocultarPaneles);
            botonExcelDetalle.click(DescargaExcelDetalle);

            modalAutorizarbtnAutorizar.unbind().click(autorizarPrenomina);
            modalAutorizarbtnRechazar.unbind().click(rechazarPrenomina);

            //initTablaPlantillaActual();
            //initTablaPlantillaNueva();

            divAutorizacion.hide();
            mostrarAutorizaciones();
            comboCC.select2({
                templateResult: function (data, container) {
                    if (data.element) {
                        $(container).addClass($(data.element).attr("class"));
                    }
                    return data.text;
                }
            });
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.RecursosHumanos.MontoAdministrativo = new MontoAdministrativo();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();
