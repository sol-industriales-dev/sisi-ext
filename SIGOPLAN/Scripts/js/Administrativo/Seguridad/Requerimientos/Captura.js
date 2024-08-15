(() => {
    $.namespace('Administrativo.Requerimientos.Captura');
    Captura = function () {
        //#region Selectores
        const selectCentroCosto = $('#selectCentroCosto');
        const selectClasificacion = $('#selectClasificacion');
        const selectRequerimiento = $('#selectRequerimiento');
        const selectEstatus = $('#selectEstatus');
        const selectResponsable = $('#selectResponsable');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const botonBuscar = $('#botonBuscar');
        const botonGuardar = $('#botonGuardar');
        const tablaCapturas = $('#tablaCapturas');
        const botonCargaMasiva = $('#botonCargaMasiva');
        const modalCargaMasiva = $('#modalCargaMasiva');
        const botonGuardarCargaMasiva = $('#botonGuardarCargaMasiva');
        const inputFechaCapturaMasiva = $('#inputFechaCapturaMasiva');
        //#endregion

        let dtCapturas;

        // Datepicker variables.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        const fechaInicioMesAnterior = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
        fechaInicioMesAnterior.setMonth(fechaInicioMesAnterior.getMonth() - 1);
        const fechaFinMesAnterior = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
        fechaFinMesAnterior.setDate(fechaFinMesAnterior.getDate() - 1);

        (function init() {
            agregarListeners();
            initTablaCapturas();
            $('.select2').select2();
            initMonthPicker(inputFechaInicio);
            initMonthPicker(inputFechaFin);
            initMonthPicker(inputFechaCapturaMasiva);

            // inputFechaInicio.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaInicioMesAnterior);
            // inputFechaFin.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaFinMesAnterior);

            selectCentroCosto.fillComboSeguridad(false);
            selectClasificacion.fillCombo('/Administrativo/Requerimientos/GetClasificacionCombo', null, false, null);
            selectRequerimiento.fillCombo('/Administrativo/Requerimientos/GetRequerimientosCombo', null, false, null);
            selectResponsable.fillCombo('/Administrativo/Requerimientos/GetResponsableCombo', null, false, null);
        })();

        function agregarListeners() {
            botonBuscar.click(cargarAsignacionCaptura);
            botonGuardar.click(guardarEvidencia);
            botonCargaMasiva.click(() => { modalCargaMasiva.modal('show') });
            botonGuardarCargaMasiva.click(guardarCargaMasiva);
        }

        function guardarEvidencia() {
            let captura = getInformacionEvidencia();

            $.ajax({
                url: '/Administrativo/Requerimientos/GuardarEvidencia',
                data: captura,
                async: false,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST'
            }).then(response => {
                // cargarAsignacionCaptura();
                tablaCapturas.DataTable().clear().draw();

                if (response.success) {
                    AlertaGeneral(`Éxito`, `Se ha guardado la información.`);
                } else {
                    AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                // cargarAsignacionCaptura();
                tablaCapturas.DataTable().clear().draw();
            }
            );
        }

        function getInformacionEvidencia() {
            const data = new FormData();
            let captura = [];

            tablaCapturas.find('tbody tr').each(function (index, row) {
                let rowData = dtCapturas.row(row).data();

                if (!rowData.evidenciaCapturada) {
                    let fechaPunto = $(row).find('.inputFechaPunto').val();
                    let listaStringFecha = fechaPunto.split('/');
                    let formatoFechaPunto = '06' + '/' + listaStringFecha[0] + '/' + listaStringFecha[1];
                    let inputEvidencia = $(row).find(`.inputEvidencia_${rowData.asignacionID}`);
                    let evidencia = inputEvidencia[0].files.length > 0 ? inputEvidencia[0].files[0] : null;

                    if (evidencia != null) {
                        captura.push({
                            cc: "",
                            idEmpresa: rowData.idEmpresa,
                            idAgrupacion: rowData.idAgrupacion,
                            requerimientoID: rowData.requerimientoID,
                            puntoID: rowData.id,
                            asignacionID: rowData.asignacionID,
                            fechaPunto: formatoFechaPunto,
                            periodicidad: rowData.periodicidad,
                            fechaInicioEvaluacion: rowData.fechaInicioEvaluacion
                        });

                        data.append('evidencias', evidencia);
                    }
                }
            });

            data.append('captura', JSON.stringify(captura));

            return data;
        }

        function initTablaCapturas() {
            dtCapturas = tablaCapturas.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                scrollX: true,
                scrollY: '45vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tablaCapturas.on('click', '.btn-ver-evidencia', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();

                        mostrarArchivo(rowData.evidenciaID);
                    });

                    tablaCapturas.on('click', '.btn-descargar-evidencia', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();

                        descargarArchivo(rowData.evidenciaID);
                    });

                    tablaCapturas.on('change', 'input[type=file]', function () {
                        let row = $(this).closest('tr');
                        let rowData = dtCapturas.row(row).data();
                        let inputEvidencia = $(row).find(`.inputEvidencia_${rowData.asignacionID}`);
                        let botonEvidencia = $(row).find(`#botonEvidencia_${rowData.asignacionID}`);
                        let iconoBoton = botonEvidencia.find('i');
                        let labelArchivo = $(row).find(`#labelArchivoEvidencia_${rowData.asignacionID}`);

                        if (inputEvidencia[0].files.length > 0) {
                            let textoLabel = inputEvidencia[0].files[0].name;

                            if (textoLabel.length > 35) {
                                textoLabel = textoLabel.substr(0, 31) + '...';
                            }

                            labelArchivo.text(textoLabel);
                            botonEvidencia.addClass('custom-file-upload-subido');
                            botonEvidencia.removeClass('custom-file-upload');
                            iconoBoton.addClass('fa-check');
                            iconoBoton.removeClass('fa-file-upload');
                        } else {
                            labelArchivo.text('');
                            botonEvidencia.addClass('custom-file-upload');
                            botonEvidencia.removeClass('custom-file-upload-subido');
                            iconoBoton.addClass('fa-file-upload');
                            iconoBoton.removeClass('fa-check');
                        }
                    });
                },
                createdRow: function (row, rowData) {
                    let inputFechaPunto = $(row).find('.inputFechaPunto');
                    // inputFechaPunto.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);
                    initMonthPicker(inputFechaPunto);

                    if (rowData.usuarioEvaluadorID > 0) {
                        if (rowData.aprobado) {
                            $(row).addClass('renglonAprobado');
                        } else {
                            $(row).addClass('renglonNoAprobado');
                        }
                    }
                },
                columns: [
                    { data: 'proyecto', title: 'Proyecto' },
                    { data: 'requerimientoDesc', title: 'Requerimiento' },
                    { data: 'puntoDesc', title: 'Punto' },
                    { data: 'codigo', title: 'Código' },
                    { data: 'periodicidadDesc', title: 'Periodicidad' },
                    { data: 'responsableDesc', title: 'Responsable' },
                    {
                        title: 'Mes', render: function (data, type, row, meta) {
                            if (row.evidenciaCapturada) {
                                let listaStringFechaEvidencia = row.fechaEvidenciaString.split('/');
                                return listaStringFechaEvidencia[1] + '/' + listaStringFechaEvidencia[2];
                            } else {
                                return `<input class="form-control text-center inputFechaPunto date-picker">`;
                            }
                        }
                    },
                    {
                        title: 'Estatus', render: function (data, type, row, meta) {
                            let estatus = '';

                            if (row.evidenciaCapturada) {
                                if (row.usuarioEvaluadorID > 0) {
                                    estatus = 'EVALUADO';
                                } else {
                                    estatus = 'COMPLETO';
                                }
                            } else {
                                estatus = 'PENDIENTE';
                            }

                            return estatus;
                        }
                    },
                    {
                        title: 'Evidencia', render: function (data, type, row, meta) {
                            let html = ``;

                            if (row.evidenciaCapturada) {
                                html += `
                                <button class="btn-ver-evidencia btn btn-sm btn-primary"><i class="fas fa-eye"></i></button>
                            &nbsp; <button class="btn-descargar-evidencia btn btn-sm btn-primary"><i class="fas fa-file-download"></i></button>`;
                            } else {
                                html += `
                                    <div class="text-center"><label id="botonEvidencia_${row.asignacionID}"for="inputEvidencia_${row.asignacionID}" class="custom-file-upload"><i class="fa fa-file-upload"></i></label>
                                    <label id="labelArchivoEvidencia_${row.asignacionID}" class="labelArchivo"></label>
                                    <input id="inputEvidencia_${row.asignacionID}" type="file" class="inputEvidencia_${row.asignacionID}" accept="application/pdf, image/*"></div>`;
                            }

                            return html;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '10%', targets: [6, 7, 8] }
                ]
            });
        }

        function cargarAsignacionCaptura() {
            let idEmpresa = $(selectCentroCosto).getEmpresa();
            let strAgrupacion = $(selectCentroCosto).getAgrupador();
            let idAgrupacion;
            if (idEmpresa == 1000) {
                idAgrupacion = strAgrupacion.replace("c_", "");
            } else if (idEmpresa == 2000) {
                idAgrupacion = strAgrupacion.replace("a_", "");
            } else {
                idAgrupacion = strAgrupacion;
            }

            let filtros = {
                idEmpresa: idEmpresa,
                idAgrupacion: idAgrupacion,
                clasificacion: +(selectClasificacion.val()),
                requerimientoID: +(selectRequerimiento.val()),
                estatus: +(selectEstatus.val()),
                responsable: +(selectResponsable.val())
                // fechaInicio: inputFechaInicio.val(),
                // fechaFin: inputFechaFin.val()
            };

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/GetAsignacionCaptura', { filtros })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tablaCapturas, response.data);
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function mostrarArchivo(evidenciaID) {
            $.post('/Administrativo/Requerimientos/CargarDatosArchivoEvidencia', { evidenciaID })
                .then(response => {
                    if (response.success) {
                        $('#myModal').data().ruta = null;
                        $('#myModal').modal('show');
                    } else {
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function descargarArchivo(evidenciaID) {
            location.href = `/Administrativo/Requerimientos/DescargarArchivoEvidencia?evidenciaID=${evidenciaID}`;
        }

        function guardarCargaMasiva() {
            var request = new XMLHttpRequest();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            request.open("POST", "/Administrativo/Requerimientos/GuardarEvidenciaCargaMasiva");
            request.send(formDataCargaMasiva());

            request.onload = function (response) {
                $.unblockUI();

                if (request.status == 200) {
                    let respuesta = JSON.parse(request.response);

                    modalCargaMasiva.modal('hide');
                    // cargarAsignacionCaptura();
                    tablaCapturas.DataTable().clear().draw();
                    $('#inputFile').val('');

                    if (respuesta.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                    } else {
                        AlertaGeneral(`Alerta`, `${respuesta.message}`);
                    }

                } else {
                    AlertaGeneral(`Alerta`, `Error al guardar la información.`);
                }
            };
        }

        function formDataCargaMasiva() {
            let formData = new FormData();
            let listaStringFechaCapturaMasiva = inputFechaCapturaMasiva.val().split('/');

            formData.append('mes', +(listaStringFechaCapturaMasiva[0]));
            formData.append('anio', +(listaStringFechaCapturaMasiva[1]));

            $.each(document.getElementById("inputFile").files, function (i, file) {
                formData.append("files[]", file);
            });

            return formData;
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
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
    }
    $(document).ready(() => Administrativo.Requerimientos.Captura = new Captura())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();