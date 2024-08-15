(() => {
    $.namespace('Administrativo.Seguridad.Recorridos');
    Recorridos = function () {
        //#region Selectores
        let minutaID = 0;
        const selectCentroCosto = $('#selectCentroCosto');
        const selectArea = $('#selectArea');
        const inputMes = $('#inputMes');
        const inputRealizador = $('#inputRealizador');
        const botonBuscar = $('#botonBuscar');
        const botonNuevoRecorrido = $('#botonNuevoRecorrido');
        const tablaGenerales = $('#tablaGenerales');
        const tablaSeguimiento = $('#tablaSeguimiento');
        const modalNuevoRecorrido = $('#modalNuevoRecorrido');
        const selectCentroCostoNuevo = $('#selectCentroCostoNuevo');
        const selectAreaNuevo = $('#selectAreaNuevo');
        const inputFechaNuevo = $('#inputFechaNuevo');
        const inputRealizadorNuevo = $('#inputRealizadorNuevo');
        const textAreaDeteccionNuevo = $('#textAreaDeteccionNuevo');
        const textAreaRecomendacionNuevo = $('#textAreaRecomendacionNuevo');
        const selectClasificacionNuevo = $('#selectClasificacionNuevo');
        const divLideresArea = $('#divLideresArea');
        const inputLideresNuevo = $('#inputLideresNuevo');
        const botonAgregarImagenRecorrido = $('#botonAgregarImagenRecorrido');
        const botonAgregarHallazgo = $('#botonAgregarHallazgo');
        const tablaHallazgo = $('#tablaHallazgo');
        const botonGuardarNuevoRecorrido = $('#botonGuardarNuevoRecorrido');
        const botonGuardarSeguimiento = $('#botonGuardarSeguimiento');
        const tablaActos = $('#tablaActos');
        const tablaCondiciones = $('#tablaCondiciones');
        const tablaAcciones = $('#tablaAcciones');
        const graficaActos = $('#graficaActos');
        const graficaCondiciones = $('#graficaCondiciones');
        const report = $("#report");
        const slMCorreos = $('#slMCorreos');
        const btnSendMail = $('#btnSendMail');
        const modalCorreos = $('#modalCorreos');
        //#endregion

        let recorridoID = 0;

        let dtGenerales;
        let dtSeguimiento;
        let dtHallazgo;
        let dtActos;
        let dtCondiciones;
        let dtAcciones;

        const ESTATUS = {
            NUEVO: 0,
            EDITAR: 1
        };

        //#region Variables Date
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        //#endregion

        _privilegioUsuario = 0;

        (function init() {
            revisarPrivilegio();
            $('.select2').select2({ language: { noResults: () => { return "No hay resultados" }, searching: () => { return "Buscando..." } } });

            initTablaGenerales();
            initTablaSeguimiento();
            initTablaHallazgo();
            initTablaActos();
            initTablaCondiciones();
            initTablaAcciones();

            initMonthPicker(inputMes);
            inputFechaNuevo.datepicker({ dateFormat, maxDate: fechaActual, showAnim, beforeShow: function (input, inst) { inst.dpDiv.removeClass('month_year_datepicker'); } }).datepicker("setDate", fechaActual);

            botonNuevoRecorrido.click(() => {
                botonGuardarNuevoRecorrido.data().estatus = ESTATUS.NUEVO;
                botonGuardarNuevoRecorrido.data().id = 0;
                limpiarModalNuevoRecorrido();
                modalNuevoRecorrido.modal('show');
            });
            botonAgregarHallazgo.click(agregarHallazgo);

            inputLideresNuevo.autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: 'GetUsuariosAutocomplete',
                        dataType: 'json',
                        data: { term: request.term, porClave: false },
                        success: function (data) {
                            response(data);
                        }
                    });
                },
                minLength: 0,
                select: liderSeleccionado
            }).autocomplete("instance")._renderItem = function (ul, item) {
                var t = item.label.replace(new RegExp('(' + this.term + ')', 'gi'), "<b>$1</b>");

                return $("<li>").data("item.autocomplete", item).append("<div>" + t + "</div>").appendTo(ul);
            };
            inputLideresNuevo.click(() => {
                inputLideresNuevo.focus();

                if (inputLideresNuevo.val().length > 0) {
                    inputLideresNuevo.autocomplete('search');
                }
            });

            divLideresArea.on('click', '.userDelete', (e) => { $(e.currentTarget).closest('.divUser').remove(); });
            botonBuscar.click(buscarRecorridos);
            botonGuardarNuevoRecorrido.click(guardarRecorrido);
            botonGuardarSeguimiento.click(guardarSeguimiento);

            inputRealizador.getAutocompleteValid(setDatosRealizador, verificarRealizador, { porClave: false }, '/Administrativo/CapacitacionSeguridad/GetUsuariosAutocomplete');
            inputRealizadorNuevo.getAutocompleteValid(setDatosRealizadorNuevo, verificarRealizadorNuevo, { porClave: false }, '/Administrativo/CapacitacionSeguridad/GetUsuariosAutocomplete');

            setCombos();

            selectCentroCosto.change(cargarAreasCC);
            selectCentroCostoNuevo.change(cargarAreasCCNuevo);
        })();

        function fncEnviarCorreos(_IdUsuario) {
            slMCorreos.fillCombo('llenarCorreos', { _IdUsuario: _IdUsuario }, false, "Todos");
            convertToMultiselect("#slMCorreos");
            modalCorreos.modal("show");
        }

        btnSendMail.click(function () {
            if (slMCorreos.val() == 0) {
                AlertaGeneral("Alerta", "¡No se han seleccionado lideres!");
            }
            else {
                fnEnviarCorreos();

            }

        });

        function fnEnviarCorreos() {
            $("#modalCorreos .close").click();
            $.ajax({
                datatype: "json",
                type: "POST",

                url: `/Reportes/Vista.aspx?idReporte=212&recorridoID=${btnSendMail.attr("data-id")}&enviarCorreoRecorrido=1&inMemory=true&listaCorreos=${getValoresMultiples("#slMCorreos")}`,
                success: function (response) {
                    if (response != null) {
                        AlertaGeneral("Confirmación", "Correos enviados correctamente");
                        $.unblockUI();
                    }
                    else {
                        AlertaGeneral("Alerta", "¡Ocurrio un problema al convertir la minuta a PDF para ser enviada!");
                        $.unblockUI();
                    }
                },
                error: function () {
                    $.unblockUI();
                },

                error: function () {
                    $.unblockUI();
                }
            });
            $.unblockUI();
        }

        function setCombos() {
            axios.get('ObtenerComboCCAmbasEmpresas').then(response => {
                let { success, items, message } = response.data;

                if (success) {
                    selectCentroCosto.append('<option value="Todos">Todos</option>');
                    selectCentroCostoNuevo.append('<option value="">--Seleccione--</option>');

                    items.forEach(x => {
                        let groupOption = `<optgroup label="${x.label}">`;

                        x.options.forEach(y => {
                            groupOption += `<option value="${y.Value}" empresa="${x.label == 'CONSTRUPLAN' ? 1 : x.label == 'ARRENDADORA' ? 2 : 0}">${y.Text}</option>`;
                        });

                        groupOption += `</optgroup>`;

                        selectCentroCosto.append(groupOption);
                        selectCentroCostoNuevo.append(groupOption);
                    });
                } else {
                    AlertaGeneral(`Alerta`, message);
                }

                convertToMultiselect('#selectCentroCosto');
            }).catch(error => AlertaGeneral(`Alerta`, error.message));

            // axios.get('GetDepartamentosCombo').then(response => {
            //     let { success, items, message } = response.data;

            //     if (success) {
            //         selectArea.append('<option value="Todos">Todos</option>');
            //         selectAreaNuevo.append('<option value="">--Seleccione--</option>');

            //         items.forEach(x => {
            //             let groupOption = `<optgroup label="${x.label}">`;

            //             x.options.forEach(y => {
            //                 groupOption += `<option value="${y.Value}" empresa="${y.Prefijo == 'CONSTRUPLAN' ? 1 : y.Prefijo == 'ARRENDADORA' ? 2 : 0}" cc="${y.Id}">${y.Text}</option>`;
            //             });

            //             groupOption += `</optgroup>`;

            //             selectArea.append(groupOption);
            //             selectAreaNuevo.append(groupOption);
            //         });

            convertToMultiselect('#selectArea');
            convertToMultiselect('#selectAreaNuevo');
            //     } else {
            //         AlertaGeneral(`Alerta`, message);
            //     }
            // }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }



        function initTablaGenerales() {
            dtGenerales = tablaGenerales.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                initComplete: function (settings, json) {
                    tablaGenerales.on('click', '.botonConsultarRecorrido', function () {
                        let rowData = tablaGenerales.DataTable().row($(this).closest('tr')).data();

                        cargarSeguimientoRecorrido(rowData.id);
                    });

                    tablaGenerales.on('click', '.botonReporteRecorrido', function () {
                        let rowData = tablaGenerales.DataTable().row($(this).closest('tr')).data();

                        imprimirRecorrido(rowData.id);
                    });

                    tablaGenerales.on('click', '.botonEnviarCorreo', function () {
                        let rowData = tablaGenerales.DataTable().row($(this).closest('tr')).data();
                        _recorridoID = rowData.id;
                        console.log(_recorridoID);
                        fncEnviarCorreos(_recorridoID);
                        btnSendMail.attr("data-id", _recorridoID);

                        //Alert2AccionConfirmar('', '¿Desea enviar el correo con el recorrido?', 'Enviar', 'Cancelar', enviarCorreo, 'warning');
                    });

                    tablaGenerales.on('click', '.botonEditarRecorrido', function () {
                        let rowData = tablaGenerales.DataTable().row($(this).closest('tr')).data();

                        botonGuardarNuevoRecorrido.data().estatus = ESTATUS.EDITAR;
                        botonGuardarNuevoRecorrido.data().id = rowData.id;

                        cargarRecorrido(rowData.id);
                    });
                },
                columns: [

                    { data: 'areaDesc', title: 'ÁREA' },
                    { data: 'fechaString', title: 'FECHA' },
                    { data: 'realizadorDesc', title: 'REALIZADOR' },
                    {
                        title: 'CONSULTAR', render: function (data, type, row, meta) {
                            return `
                                <button class="btn btn-sm btn-primary botonConsultarRecorrido"><i class="fa fa-search"></i></button>
                                <button class="btn btn-sm btn-primary botonReporteRecorrido" ${(_privilegioUsuario == 2) ? 'disabled' : ''}><i class="fa fa-file"></i></button>
                                <button class="btn btn-sm btn-primary botonEnviarCorreo" ${(_privilegioUsuario == 2 || _privilegioUsuario == 3) ? 'disabled' : ''}><i class="fa fa-envelope"></i></button>
                                ${!row.cerrado ? `<button class="btn btn-sm btn-warning botonEditarRecorrido" ${_privilegioUsuario == 3 ? 'disabled' : ''}><i class="fa fa-edit"></i></button>` : ``}
                            `;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        //" ${(_privilegioUsuario == 2) ? 'disabled' : ''}

        function initTablaSeguimiento() {
            dtSeguimiento = tablaSeguimiento.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                initComplete: function (settings, json) {
                    tablaSeguimiento.on('click', '.radioBtn a', function () {
                        let rowData = dtSeguimiento.row($(this).closest('tr')).data();
                        let div = $(this).closest('div');
                        let seleccion = $(this).attr('solventada');

                        div.find(`a[data-toggle="radioAprobado${rowData.id}"]`).not(`[solventada="${seleccion}"]`).removeClass('active').addClass('notActive');
                        div.find(`a[data-toggle="radioAprobado${rowData.id}"][solventada="${seleccion}"]`).removeClass('notActive').addClass('active');
                    });
                },

                createdRow: function (row, rowData) {
                    if (rowData.solventada > 0) {
                        $(row).addClass('renglonAprobado');

                    } else {
                        //$(row).addClass('renglonNoAprobado');
                    }
                },

                columns: [
                    { data: 'deteccion', title: 'Detección' },
                    { data: 'recomendacion', title: 'Recomendación' },
                    { data: 'clasificacionDesc', title: 'Clasificación' },
                    { data: 'lideresString', title: 'Líderes Área' },
                    {
                        data: "solventada", title: 'Solventada', render: function (data, type, row, meta) {
                            let html = ``;
                            if (row.solventada == 0) {

                                html = `
                            <div class="radioBtn btn-group">
                                <a class="btn btn-success notActive" data-toggle="radioAprobado${row.id}" solventada="1"><i class="fa fa-check"></i>&nbsp;SÍ</a>
                                <a class="btn btn-danger notActive" data-toggle="radioAprobado${row.id}" solventada="0"><i class="fa fa-times"></i>&nbsp;NO</a>
                                <a class="btn btn-primary active" data-toggle="radioAprobado${row.id}" solventada="2"><i class="fa fa-square"></i>&nbsp;En espera</a>
                            </div>`;
                            }


                            return html;
                        }
                    }
                ],
            });
        }

        function initTablaHallazgo() {
            dtHallazgo = tablaHallazgo.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                // scrollX: 'auto',
                initComplete: function (settings, json) {
                    tablaHallazgo.on('change', 'input[type=file]', function () {
                        let row = $(this).closest('tr');
                        let inputEvidencia = $(row).find(`.inputEvidencia`);
                        let botonEvidencia = $(row).find(`.botonEvidencia`);
                        let iconoBoton = botonEvidencia.find('i');
                        let labelArchivo = $(row).find(`.labelArchivo`);

                        if (inputEvidencia[0].files.length > 0) {
                            let textoLabel = inputEvidencia[0].files[0].name;

                            if (textoLabel.length > 35) {
                                textoLabel = textoLabel.substr(0, 31) + '...';
                            }

                            labelArchivo.text(textoLabel);
                            botonEvidencia.addClass('btn-success');
                            botonEvidencia.removeClass('btn-default');
                            iconoBoton.addClass('fa-check');
                            iconoBoton.removeClass('fa-upload');
                        } else {
                            labelArchivo.text('');
                            botonEvidencia.addClass('btn-default');
                            botonEvidencia.removeClass('btn-success');
                            iconoBoton.addClass('fa-upload');
                            iconoBoton.removeClass('fa-check');
                        }
                    });

                    tablaHallazgo.on('click', '.botonQuitarHallazgo', function () {
                        let row = $(this).closest('tr');

                        dtHallazgo.row(row).remove().draw();

                        let cuerpo = tablaHallazgo.find('tbody');

                        if (cuerpo.find("tr").length == 0) {
                            dtHallazgo.draw();
                        }
                    });
                },
                columns: [
                    { data: 'deteccion', title: 'Detección' },
                    { data: 'recomendacion', title: 'Recomendación' },
                    { data: 'clasificacionDesc', title: 'Clasificación' },
                    { data: 'lideresString', title: 'Líderes Área' },
                    {
                        title: 'Evidencia', render: function (data, type, row, meta) {
                            return row.rutaEvidencia == undefined ? `
                                <div class="text-center">
                                    <label id="botonEvidencia_${meta.row}" for="inputEvidencia_${meta.row}" class="btn btn-xs btn-default botonEvidencia"><i class="fa fa-upload"></i></label>
                                    <label id="labelArchivoEvidencia_${meta.row}" class="labelArchivo"></label>
                                    <input id="inputEvidencia_${meta.row}" type="file" class="inputEvidencia inputEvidencia_${meta.row}" accept="application/pdf, image/*">
                                </div>`: `<button class="btn btn-xs btn-success botonEvidenciaCargada" disabled><i class="fa fa-check"></i></button>`;
                        }
                    },
                    {
                        title: '', render: function (data, type, row, meta) {
                            return row.rutaEvidencia == undefined ? `<button class="btn btn-xs btn-danger botonQuitarHallazgo"><i class="fa fa-times"></i></button>` : ``;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '30%', targets: [0, 1] },
                    { width: '20%', targets: [3] },
                    { width: '10%', targets: [2, 4] }
                ]
            });
        }

        function initTablaActos() {
            dtActos = tablaActos.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'detectados', title: '# ACTOS DETECTADOS' },
                    { data: 'solventados', title: '# DE ACTOS SOLVENTADOS' },
                    {
                        data: 'porcentajeCumplimiento', title: '% CUMPLIMIENTO', render: function (data, type, row, meta) {
                            return data + '%';
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaCondiciones() {
            dtCondiciones = tablaCondiciones.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'detectados', title: '# CONDICIONES DETECTADAS' },
                    { data: 'solventados', title: '# CONDICIONES SOLVENTADAS' },
                    {
                        data: 'porcentajeCumplimiento', title: '% CUMPLIMIENTO', render: function (data, type, row, meta) {
                            return data + '%';
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaAcciones() {
            dtAcciones = tablaAcciones.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'acciones', title: '# ACCIONES EFICIENTES Y SEGURAS' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function imprimirRecorrido(recorridoID) {
            if (recorridoID == 0) {
                AlertaGeneral(`Alerta`, `No se ha seleccionado un recorrido.`);
                return;
            }

            $.blockUI({ message: 'Generando imprimible...' });
            var path = `/Reportes/Vista.aspx?idReporte=212&recorridoID=${recorridoID}&enviarCorreo=0&`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function enviarCorreo() {
            if (_recorridoID == 0) {
                AlertaGeneral(`Alerta`, `No se ha seleccionado un recorrido.`);
                return;
            }

            $.blockUI({ message: 'Enviando correo...' });
            var path = `/Reportes/Vista.aspx?inMemory=1&idReporte=212&recorridoID=${_recorridoID}&enviarCorreo=1`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                // openCRModal();
                Alert2Exito('Se ha enviado el correo.');
                buscarRecorridos();
            };
        }

        function cargarSeguimientoRecorrido(recorridoID) {

            axios.get('GetRecorridoByID', { params: { recorridoID } })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        $('#botonTabSeguimiento').click();
                        AddRows(tablaSeguimiento, datos.listaHallazgos);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function agregarHallazgo() {
            let deteccion = textAreaDeteccionNuevo.val();
            let recomendacion = textAreaRecomendacionNuevo.val();
            let clasificacion = selectClasificacionNuevo.val();
            let clasificacionDesc = selectClasificacionNuevo.find('option:selected').text();
            let listaLideres = [];

            divLideresArea.find('.userComponent').each(function (index, element) {
                let claveEmpleado = +$(element).attr('data-userid');
                let nombreEmpleado = $(element).attr('data-user');

                listaLideres.push({ claveEmpleado, nombreEmpleado });
            });

            if (deteccion.length == 0 || recomendacion.length == 0 || clasificacion == '' || listaLideres.length == 0) {
                Alert2Warning('Debe capturar todos los campos.');
                return;
            }

            dtHallazgo.row.add({
                deteccion,
                recomendacion,
                clasificacion,
                clasificacionDesc,
                listaLideres,
                lideresString: listaLideres.map((x) => x.nombreEmpleado).join(', ')
            }).draw();

            limpiarSeccionHallazgo();
        }

        function limpiarModalNuevoRecorrido() {
            selectCentroCostoNuevo.val('');
            selectAreaNuevo.empty();
            selectAreaNuevo.multiselect('rebuild');
            inputFechaNuevo.val('');
            inputRealizadorNuevo.data('id', 0);
            inputRealizadorNuevo.val('');
            textAreaDeteccionNuevo.val('');
            textAreaRecomendacionNuevo.val('');
            selectClasificacionNuevo.val('');
            divLideresArea.find('.divUser').remove();
            inputLideresNuevo.val('');
            dtHallazgo.clear().draw();
        }

        function limpiarSeccionHallazgo() {
            textAreaDeteccionNuevo.val('');
            textAreaRecomendacionNuevo.val('');
            selectClasificacionNuevo.val('');
            divLideresArea.find('.divUser').remove();
            inputLideresNuevo.val('');
        }

        function liderSeleccionado(event, ui) {
            let html = `
                <div class="divUser">
                    <div class="userContainer">
                        <span class="userFill">&nbsp;</span>
                        <span class="userComponent" data-user="${ui.item.value}" data-userid="${ui.item.id}">${ui.item.value}</span>
                        <button type="button" class="userDelete">&nbsp;X</button>
                    </div>
                </div>`;

            $(html).insertBefore(inputLideresNuevo);
            inputLideresNuevo.focus();
            ui.item.value = "";  // it will clear field 
            inputLideresNuevo.autocomplete('close').val('');

            return false;
        }

        function buscarRecorridos() {
            let listaCC = getValoresMultiples('#selectCentroCosto');

            let listaAreas = [];

            getValoresMultiplesCustom('#selectArea').forEach(x => {
                listaAreas.push({
                    cc: x.cc,
                    area: +x.departamento,
                    empresa: +x.empresa
                });
            });

            let mes = inputMes.val();
            let listaStringMes = mes.split('/');
            let fecha = '01' + '/' + listaStringMes[0] + '/' + listaStringMes[1];

            let realizador = (inputRealizador.data('id') === undefined) ? 0 : +inputRealizador.data('id');

            axios.post('GetRegistrosRecorridos', { listaCC, listaAreas, fecha, realizador })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        $('#divTab').find('.tab-pane').removeClass('hide');
                        AddRows(tablaGenerales, datos);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));

            //Se carga la información del Dashboard.
            axios.post('CargarDashboardRecorridos', { listaCC, listaAreas, fecha, realizador })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaActos, response.data.listaActos);
                        AddRows(tablaCondiciones, response.data.listaCondiciones);
                        AddRows(tablaAcciones, response.data.listaAcciones);
                        initGraficaActos(response.data.graficaActos);
                        initGraficaCondiciones(response.data.graficaCondiciones);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function guardarSeguimiento() {
            let listaSeguimiento = [];

            tablaSeguimiento.find('tbody tr').each(function (index, row) {
                let rowData = dtSeguimiento.row(row).data();
                let solventada = +($(row).find(`.radioBtn a.active[data-toggle=radioAprobado${rowData.id}]`).attr('solventada'));
                console.log(solventada);

                if (solventada > 0) {
                    listaSeguimiento.push({
                        id: rowData.id,
                        solventada: solventada == 1,
                        evaluador: 0,
                        estatus: true
                    });
                }

            });
            if (listaSeguimiento.length > 0) {
                axios.post('GuardarSeguimientoRecorrido', { listaSeguimiento })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            dtSeguimiento.clear().draw();
                            Alert2Exito('Se ha guardado la información.');
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }

        }

        function setDatosRealizador(e, ui) {
            inputRealizador.data('id', ui.item.id);
            inputRealizador.val(ui.item.nombreEmpleado);
        }

        function verificarRealizador(e, ui) {
            if (ui.item == null) {
                inputRealizador.data('id', 0);
                inputRealizador.val('');
            }
        }

        function setDatosRealizadorNuevo(e, ui) {
            inputRealizadorNuevo.data('id', ui.item.id);
            inputRealizadorNuevo.val(ui.item.nombreEmpleado);
        }

        function verificarRealizadorNuevo(e, ui) {
            if (ui.item == null) {
                inputRealizadorNuevo.data('id', 0);
                inputRealizadorNuevo.val('');
            }
        }

        function initGraficaActos(datos) {
            Highcharts.chart('graficaActos', {
                chart: { type: 'column' },
                lang: highChartsDicEsp,
                title: { text: 'ACTOS' },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: { text: '' },
                    labels: { format: '{value}' },
                    allowDecimals: false
                },
                // tooltip: {
                //     headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                //     pointFormat: `
                //         <tr>
                //             <td style="color:{series.color};padding:0">{series.name}: </td>
                //             <td><b>{point.y}</b></td>
                //         </tr>`,
                //     footerFormat: '</table>',
                //     shared: true,
                //     useHTML: true
                // },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    },
                    series: {
                        colorByPoint: true,
                        colors: ['rgb(237, 125, 49)', 'rgb(0, 176, 80)']
                    }
                },
                series: [
                    { name: datos.serie1Descripcion, data: datos.serie1 }
                ],
                credits: { enabled: false },
                legend: { enabled: false }
            });

            $('.highcharts-title').css("display", "none");
        }

        function initGraficaCondiciones(datos) {
            Highcharts.chart('graficaCondiciones', {
                chart: { type: 'column' },
                lang: highChartsDicEsp,
                title: { text: 'CONDICIONES' },
                xAxis: {
                    categories: datos.categorias,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: { text: '' },
                    labels: { format: '{value}' },
                    allowDecimals: false
                },
                // tooltip: {
                //     headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                //     pointFormat: `
                //         <tr>
                //             <td style="color:{series.color};padding:0">{series.name}: </td>
                //             <td><b>{point.y}</b></td>
                //         </tr>`,
                //     footerFormat: '</table>',
                //     shared: true,
                //     useHTML: true
                // },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    },
                    series: {
                        colorByPoint: true,
                        colors: ['rgb(237, 125, 49)', 'rgb(0, 176, 80)']
                    }
                },
                series: [
                    { name: datos.serie1Descripcion, data: datos.serie1 }
                ],
                credits: { enabled: false },
                legend: { enabled: false }
            });

            $('.highcharts-title').css("display", "none");
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }

        function getValoresMultiplesCustom(selector) {
            var _tempObj = $(selector + ' option:selected').map(function (a, item) {
                return { value: item.value, empresa: $(item).attr('empresa'), departamento: item.value, cc: $(item).attr('cc') };
            });
            var _tempArrObj = new Array();
            $.each(_tempObj, function (i, e) {
                _tempArrObj.push(e);
            });
            return _tempArrObj;
        }

        function initMonthPicker(input) {
            $(input).datepicker({
                dateFormat: "mm/yy",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                maxDate: fechaActual,
                showAnim: showAnim,
                closeText: "Aceptar",
                onClose: function (dateText, inst) {
                    function isDonePressed() {
                        return ($('#ui-datepicker-div').html().indexOf('ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all ui-state-hover') > -1);
                    }

                    if (isDonePressed()) {
                        var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                        var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                        $(this).datepicker('setDate', new Date(year, month, 1)).trigger('change');

                        $('.date-picker').focusout()//Added to remove focus from datepicker input box on selecting date
                    }
                },
                beforeShow: function (input, inst) {
                    inst.dpDiv.addClass('month_year_datepicker')

                    if ((datestr = $(this).val()).length > 0) {
                        year = datestr.substring(datestr.length - 4, datestr.length);
                        month = datestr.substring(0, 2);
                        $(this).datepicker('option', 'defaultDate', new Date(year, month - 1, 1));
                        $(this).datepicker('setDate', new Date(year, month - 1, 1));
                        $(".ui-datepicker-calendar").hide();
                    }
                }
            }).datepicker("setDate", fechaActual);
        }

        function revisarPrivilegio() {
            axios.get('privilegioCapacitacion')
                .then(response => {
                    if (response.data == 0) {
                        AlertaGeneral(`Alerta`, `No tiene permisos para visualizar este módulo.`);
                    } else {
                        _privilegioUsuario = response.data;

                        if (_privilegioUsuario == 2) {
                            botonGuardarSeguimiento.attr('disabled', true);
                        }
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarAreasCC() {
            let listaCentrosCosto = [];

            getValoresMultiplesCustom('#selectCentroCosto').forEach(x => {
                listaCentrosCosto.push({
                    cc: x.value,
                    empresa: +(x.empresa)
                });
            });

            if (listaCentrosCosto.length == 0) {
                selectArea.empty();
                convertToMultiselect('#selectArea');
                return;
            }

            let ccsCplan = listaCentrosCosto.filter((x) => { return x.empresa == 1; }).map(function (x) { return x.cc; });
            let ccsArr = listaCentrosCosto.filter((x) => { return x.empresa == 2; }).map(function (x) { return x.cc; });

            $.post('/Administrativo/CapacitacionSeguridad/ObtenerAreasPorCC', { ccsCplan, ccsArr })
                .then(response => {
                    selectArea.empty();
                    if (response.success) {
                        // Operación exitosa.
                        // const todosOption = `<option value="Todos">Todos</option>`;
                        const option = `<option value="Todos">Todos</option>`;
                        selectArea.append(option);

                        response.items.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}"></optgroup>`;
                            x.options.forEach(y => {
                                groupOption += `<option value="${y.Value}" cc="${y.Id}" empresa="${y.Prefijo}">${y.Text}</option>`;
                            });
                            selectArea.append(groupOption);
                        });

                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }

                    convertToMultiselect('#selectArea');
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function cargarAreasCCNuevo() {
            let listaCentrosCosto = [];

            getValoresMultiplesCustom('#selectCentroCostoNuevo').forEach(x => {
                listaCentrosCosto.push({
                    cc: x.value,
                    empresa: +(x.empresa)
                });
            });

            if (listaCentrosCosto.filter((x) => x.cc != '').length == 0) {
                selectAreaNuevo.empty();
                return;
            }

            let ccsCplan = listaCentrosCosto.filter((x) => { return x.empresa == 1; }).map(function (x) { return x.cc; });
            let ccsArr = listaCentrosCosto.filter((x) => { return x.empresa == 2; }).map(function (x) { return x.cc; });

            $.post('/Administrativo/CapacitacionSeguridad/ObtenerAreasPorCC', { ccsCplan, ccsArr })
                .then(response => {
                    selectAreaNuevo.empty();
                    if (response.success) {
                        // Operación exitosa.
                        // const todosOption = `<option value="Todos">Todos</option>`;
                        const option = `<option value="Todos">Todos</option>`;
                        selectAreaNuevo.append(option);

                        response.items.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}"></optgroup>`;
                            x.options.forEach(y => {
                                groupOption += `<option value="${y.Value}" cc="${y.Id}" empresa="${y.Prefijo}">${y.Text}</option>`;
                            });
                            selectAreaNuevo.append(groupOption);
                        });

                        convertToMultiselect('#selectAreaNuevo');
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

        function guardarRecorrido() {
            let estatus = botonGuardarNuevoRecorrido.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    guardarNuevoRecorrido();
                    break;
                case ESTATUS.EDITAR:
                    editarRecorrido();
                    break;
            }
        }

        function guardarNuevoRecorrido() {
            let data = getInformacionRecorrido(false);

            axios.post('GuardarNuevoRecorrido', data, { headers: { 'Content-Type': 'multipart/form-data' } })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha guardado la información.');
                        modalNuevoRecorrido.modal('hide');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function editarRecorrido() {
            let data = getInformacionRecorrido(true);

            axios.post('EditarRecorrido', data, { headers: { 'Content-Type': 'multipart/form-data' } })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha guardado la información.');
                        modalNuevoRecorrido.modal('hide');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function getInformacionRecorrido(editar) {
            const data = new FormData();

            let recorrido = {
                id: botonGuardarNuevoRecorrido.data().id,
                cc: selectCentroCostoNuevo.val(),
                area: 0,
                empresa: 0,
                fecha: inputFechaNuevo.val(),
                realizador: inputRealizadorNuevo.data('id')
            }

            let listaHallazgos = [];

            tablaHallazgo.find('tbody tr').each(function (index, row) {
                let rowData = dtHallazgo.row(row).data();

                if (rowData.rutaEvidencia == undefined) {
                    let inputEvidencia = $(row).find(`.inputEvidencia`);
                    let evidencia = inputEvidencia[0].files.length > 0 ? inputEvidencia[0].files[0] : null;

                    if (evidencia != null) {
                        data.append('evidencias', evidencia);
                    }

                    listaHallazgos.push({
                        deteccion: rowData.deteccion,
                        recomendacion: rowData.recomendacion,
                        clasificacion: rowData.clasificacion,
                        listaLideres: rowData.listaLideres.map((x) => x.claveEmpleado)
                    });
                }
            });

            let listaAreas = [];

            getValoresMultiplesCustom('#selectAreaNuevo').forEach(x => {
                listaAreas.push({
                    cc: x.cc,
                    area: +x.departamento,
                    empresa: +x.empresa
                });
            });

            data.append('recorrido', JSON.stringify(recorrido));
            data.append('listaHallazgos', JSON.stringify(listaHallazgos));
            data.append('listaAreas', JSON.stringify(listaAreas));

            return data;
        }

        function cargarRecorrido(id) {
            axios.post('GetRecorridoByID', { recorridoID: id })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        limpiarModalNuevoRecorrido();
                        modalNuevoRecorrido.modal('show');
                        llenarInformacionRecorrido(response.data.datos);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function llenarInformacionRecorrido(data) {
            selectCentroCostoNuevo.val(data.cc).trigger('change.select2');

            //#region Evento Change de selectCentroCostoNuevo
            let listaCentrosCosto = [];

            getValoresMultiplesCustom('#selectCentroCostoNuevo').forEach(x => {
                listaCentrosCosto.push({
                    cc: x.value,
                    empresa: +(x.empresa)
                });
            });

            let ccsCplan = listaCentrosCosto.filter((x) => { return x.empresa == 1; }).map(function (x) { return x.cc; });
            let ccsArr = listaCentrosCosto.filter((x) => { return x.empresa == 2; }).map(function (x) { return x.cc; });

            $.post('/Administrativo/CapacitacionSeguridad/ObtenerAreasPorCC', { ccsCplan, ccsArr })
                .then(response => {
                    selectAreaNuevo.empty();
                    if (response.success) {
                        const option = `<option value="">--Seleccione--</option>`;
                        selectAreaNuevo.append(option);

                        response.items.forEach(x => {
                            let groupOption = `<optgroup label="${x.label}"></optgroup>`;
                            x.options.forEach(y => {
                                let flagSeleccionado = false;

                                if (data.listaAreas.some((x) => x.cc == y.Id && x.area == y.Value && x.empresa == y.Prefijo)) {
                                    flagSeleccionado = true;
                                }

                                groupOption += `<option value="${y.Value}" cc="${y.Id}" empresa="${y.Prefijo}" ${flagSeleccionado ? 'selected' : ''}>${y.Text}</option>`;
                            });
                            selectAreaNuevo.append(groupOption);
                        });

                        convertToMultiselect('#selectAreaNuevo');
                    } else {
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
            //#endregion

            inputFechaNuevo.val(data.fechaString);
            inputRealizadorNuevo.val(data.realizadorDesc);

            AddRows(tablaHallazgo, data.listaHallazgos);
        }
    }
    $(document).ready(() => Administrativo.Seguridad.Recorridos = new Recorridos())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...', baseZ: '2000' }))
    // .ajaxStop($.unblockUI);
})();