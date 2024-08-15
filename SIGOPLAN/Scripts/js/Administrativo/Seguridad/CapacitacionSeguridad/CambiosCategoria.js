let GestionControllers = function () {


    const tblGestionCambios = $('#tblGestionCambios');
    const tblGestionCambiosCategoria = $('#tblGestionCambiosCategoria');
    const tblGestionCambiosCategoriaDescargas = $('#tblGestionCambiosCategoriaDescargas');
    let dtGestionCambiosCategoriaDescargas
    let dtGestionCambiosCategoria;
    let dtGestionCambios;
    const inputMes = $('#inputMes');
    let cboCC = $('#cboCC');
    let cboTurno = $('#cboTurno');

    const btnBuscar = $('#btnBuscar');
    const btnModal = $('#btnModal');

    const fechaActual = new Date();
    const dateFormat = "dd/mm/yy";
    const showAnim = "slide";
    const btnGuardarModalArchivos = $('#btnGuardarModalArchivos');

    let cboProyecto = $('#cboProyecto');
    let inputNum = $('#inputNum');
    let cboEstado = $('#cboEstado');
    let cboTipo = $('#cboTipo');
    let txtGato = $('#txtGato');

    let dtFechaModal = $('#dtFechaModal');

    let cboCCDescarga = $('#cboCCDescarga');
    let dtFechaModalDescarga = $('#dtFechaModalDescarga');
    let dataID = 0;
    const btnDescargarExamen = $('#btnDescargarExamen');
    const btnDescargarReporteEvaluacion = $('#btnDescargarReporteEvaluacion');
    const btnDescargarSolicitudCambio = $('#btnDescargarSolicitudCambio');
    const btnDescargarListadoCambio = $('#btnDescargarListadoCambio');




    const Inicializar = function () {
        $.namespace('CambiosCategoria.Promedio');
        Iniciar();
        CargarCombos();
        fncBotones();
        postObtenerTablaCambiosCategoria();
    }
    const Iniciar = function () {
        Promedio = function () {
            (function init() {
                initTablaCambiosCategoria();
                initTablaCambiosCategoriaModalDescargas();
                initMonthPicker(inputMes);
                initfechas();

            })();
        }
    }
    const initfechas = function () {
        dtFechaModal.datepicker({
            "dateFormat": "dd/mm/yy",
            "maxDate": fechaActual
        }).datepicker("option", "showAnim", "slide")
            .datepicker("setDate", fechaActual);
        dtFechaModalDescarga.datepicker({
            "dateFormat": "dd/mm/yy",
            "maxDate": fechaActual
        }).datepicker("option", "showAnim", "slide")
            .datepicker("setDate", fechaActual);
    }
    const fncBotones = function () {
        btnBuscar.click(function () {
            postObtenerTablaCambiosCategoria();
        });
        $('#inputExamen').change(function () {
            $('#lblTexto1').text($(this)[0].files[0].name);
        });
        $('#inputReporteEvaluacion').change(function () {
            $('#lblTexto2').text($(this)[0].files[0].name);
        });
        $('#inputSolicitudDeCambio').change(function () {
            $('#lblTexto3').text($(this)[0].files[0].name);
        });
        $('#inputListadoCambio').change(function () {
            $('#lblTexto4').text($(this)[0].files[0].name);
        });

        btnGuardarModalArchivos.click(function () {
            subiendoArchivo(btnGuardarModalArchivos.attr('data-id'), btnGuardarModalArchivos.attr('data-empresa'));
        });


        btnDescargarExamen.click(function () {
            DescargarArchivo(btnDescargarExamen.attr('data-id'))
        });
        btnDescargarReporteEvaluacion.click(function () {
            DescargarArchivo(btnDescargarReporteEvaluacion.attr('data-id'))
        });
        btnDescargarSolicitudCambio.click(function () {
            DescargarArchivo(btnDescargarSolicitudCambio.attr('data-id'))
        });
        btnDescargarListadoCambio.click(function () {
            DescargarArchivo(btnDescargarListadoCambio.attr('data-id'))
        });


    }

    const CargarCombos = function () {
        axios.get('ObtenerComboCCAmbasEmpresas').then(response => {
            let { success, items, message } = response.data;

            if (success) {
                cboProyecto.append('<option value="Todos">Todos</option>');
                cboCC.append('<option value="Todos">Todos</option>');
                cboCCDescarga.append('<option value="Todos">Todos</option>');
                items.forEach(x => {
                    let groupOption = `<optgroup label="${x.label}">`;

                    x.options.forEach(y => {
                        groupOption += `<option value="${y.Value}" empresa="${x.label == 'CONSTRUPLAN' ? 1 : x.label == 'ARRENDADORA' ? 2 : 0}">${y.Text}</option>`;
                    });

                    groupOption += `</optgroup>`;

                    cboProyecto.append(groupOption);
                    cboCC.append(groupOption);
                    cboCCDescarga.append(groupOption);
                });
            } else {
                AlertaGeneral(`Alerta`, message);
            }

            convertToMultiselect('#cboProyecto');
            convertToMultiselect('#cboCC');
            convertToMultiselect('#cboCCDescarga');
        }).catch(error => AlertaGeneral(`Alerta`, error.message));

        axios.get('GetDepartamentosCombo').then(response => {
            let { success, items, message } = response.data;

            if (success) {
                cboTurno.append('<option value="Todos">Todos</option>');

                items.forEach(x => {
                    let groupOption = `<optgroup label="${x.label}">`;

                    x.options.forEach(y => {
                        groupOption += `<option value="${y.Value}" empresa="${y.Prefijo == 'CONSTRUPLAN' ? 1 : y.Prefijo == 'ARRENDADORA' ? 2 : 0}" cc="${y.Id}">${y.Text}</option>`;
                    });

                    groupOption += `</optgroup>`;

                    cboTurno.append(groupOption);
                });

                convertToMultiselect('#cboTurno');
            } else {
                AlertaGeneral(`Alerta`, message);
            }
        }).catch(error => AlertaGeneral(`Alerta`, error.message));
    }
    const initTablaCambiosCategoria = function () {
        dtGestionCambios = tblGestionCambios.DataTable({
            destroy: true,
            ordering: false,
            language: dtDicEsp,
            searching: false,
            paging: false,
            columns: [
                { title: 'Operacion', data: 'id', width: '6%' },
                { title: 'Empleado', data: 'usuarioCap', width: '7%' },
                { title: 'Nombre', data: 'nomUsuarioCap', width: '7%' },
                { title: 'Centro Costo', data: 'CCAnt', width: '7%' },
                { title: 'Puestos', data: 'PuestoAnt', width: '7%' },
                {
                    title: 'Estado', data: 'Aprobado', width: "2%", createdCell: (td, data, rowData, row, col) => {
                        let txt;
                        if (rowData.Aprobado) {
                            txt = `Aprobado`
                        }
                        else {
                            if (rowData.Rechazado) {
                                txt = `Rechazado`
                            }
                            else {
                                txt = `Pendiente`
                            }
                        }
                        $(td).html(txt);
                    }
                },
                {
                    title: 'Soporte', data: 'btnSubirArchivo', width: '7%',
                    render: function (data, type, row) {
                        let btnEliminar = '';

                        if (data == false) {
                            btnEliminar += `<button class='btn btn-danger subirArchivos' data-esActivo="0" data-id="${row.id}">` +
                                `<i class="fas fa-upload"></i></button>`;
                        } else {
                            btnEliminar += `<button class='btn btn-success descargarArchivos' data-esActivo="1" data-id="${row.id}">` +
                                `<i class="fas fa-eye"></i></button>`;
                        }
                        return btnEliminar;
                    }
                }
            ],
            drawCallback: function (settings) {
                $('.descargarArchivos').on("click", function (e) {
                    ObtenerArchivos($(this).attr("data-id"));

                    $('#modalCambiosCategoriasDescarg').modal('show');
                    $("#titleCurso").empty();
                    $("#titleCurso").append('Gestion de cambios de categoria Descargas');
                });
                $('.subirArchivos').on("click", function (e) {
                    let row = $(this).closest('tr');
                    let rowData = dtGestionCambios.row(row).data();

                    btnGuardarModalArchivos.attr('data-id', $(this).attr("data-id"));
                    btnGuardarModalArchivos.attr('data-empresa', rowData.empresa);

                    $('#modalCambiosCategorias').modal('show');
                    $("#titleCurso").empty();
                    $("#titleCurso").append('Gestion de cambios de categoria Subir Archivos');
                    $('#inputExamen').val('');
                    $('#lblTexto1').text('Ningún archivo seleccionado');
                    $('#inputReporteEvaluacion').val('');
                    $('#lblTexto2').text('Ningún archivo seleccionado');
                    $('#inputSolicitudDeCambio').val('');
                    $('#lblTexto3').text('Ningún archivo seleccionado');
                    $('#inputListadoCambio').val('');
                    $('#lblTexto4').text('Ningún archivo seleccionado');
                });
            }

        });
    }

    const initTablaCambiosCategoriaModalDescargas = function () {
        dtGestionCambiosCategoriaDescargas = tblGestionCambiosCategoriaDescargas.DataTable({
            destroy: true,
            ordering: false,
            language: dtDicEsp,
            searching: false,
            paging: false,
            columns: [
                { title: 'Empleado', data: 'TipoSolicitud', width: '7%' },
                { title: 'Examen', data: 'TipoSolicitud', width: '7%' },
                { title: 'Reporte Evaluacion', data: 'TipoSolicitud', width: '7%' },
                { title: 'Solicitud de Cambio', data: 'TipoSolicitud', width: '7%' },
                { title: 'Listado Cambio', data: 'TipoSolicitud', width: '7%' },
            ],
            initComplete: function (settings, json) {
                tblGestionCambiosCategoriaDescargas.on("click", ".eliminarAgrupacion", function () {

                });


            }

        });
    }
    const postObtenerTablaCambiosCategoria = function () {
        let parametro = CrearParametro();
        axios.post('/CapacitacionSeguridad/TablaFormatosPendientes', { parametros: parametro })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, data } = response.data;
                if (success) {
                    AddRows(tblGestionCambios, response.data.data);
                }
                else {
                    AddRows(tblGestionCambios, response.data.data);
                }
            });

    }



    const postObtenerTablaCambiosCategoriaModalDescargas = function () {
        let parametro = CrearParametroModal();
        axios.post('/CapacitacionSeguridad/postObtenerTablaCambiosCategoriaModalDescargas', { parametro: parametro })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, data } = response.data;
                if (success) {
                    dtGestionCambiosCategoria.clear();
                    dtGestionCambiosCategoria.rows.add(response.data.data);
                    dtGestionCambiosCategoria.draw();
                } else {

                }
            });
    }

    const initMonthPicker = function (input) {
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
    const CrearParametro = function () {
        let lst = "";
        lst = "";
        $('#cboProyecto').val().forEach(x => {
            lst += x + ",";
        });
        let parameter = $('#cboProyecto').val().length == 0 ? '' : lst;
        let Parametros = {
            CC: parameter,
            claveEmpleado: inputNum.val(),
            estado: cboEstado.val(),
            tipo: cboTipo.val() == 1 ? '' : cboTipo.val(),
            numero: txtGato.val() == "" ? 0 : txtGato.val(),
            id: dataID
        };
        return Parametros;
    }
    const CrearParametroModal = function () {
        let Parametros = {
            cc: cboProyecto.val(),
            fecha: dtFechaModal.val(),
            numEmpleado: inputNum.val(),
            puestoSolicitado: inputPuestoSolicitado.val(),
        };
        return Parametros;
    }

    const subiendoArchivo = function (id, empresa) {
        if (validarInputs() == true) {
            var data = formDataCargaMasiva(id, empresa);
            axios.post('postSubirArchivos', data, { headers: { 'Content-Type': 'multipart/form-data' } })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha guardado la información.');
                        $('#modalCambiosCategorias').modal('hide');
                        postObtenerTablaCambiosCategoria();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        } else {
            AlertaGeneral('Alerta', 'Campos vacios favor de seleccionar todos los archivos');
        }
    }

    function formDataCargaMasiva(id, empresa) {
        let formData = new FormData();
        formData.append("id", id);
        formData.append('empresa', empresa);
        $.each(document.getElementById("inputExamen").files, function (i, file) {
            formData.append("archivo", file);
        });
        $.each(document.getElementById("inputReporteEvaluacion").files, function (i, file) {
            formData.append("archivo", file);
        });
        $.each(document.getElementById("inputSolicitudDeCambio").files, function (i, file) {
            formData.append("archivo", file);
        });
        $.each(document.getElementById("inputListadoCambio").files, function (i, file) {
            formData.append("archivo", file);
        });
        return formData;
    }
    const validarInputs = function () {
        let inputVacio = true;
        if ($('#inputExamen').val() == "") {
            return inputVacio = false;
        } else { inputVacio = true; }
        if ($('#inputReporteEvaluacion').val() == "") {
            return inputVacio = false;
        } else { inputVacio = true; }
        if ($('#inputSolicitudDeCambio').val() == "") {
            return inputVacio = false;
        } else { inputVacio = true; }
        if ($('#inputListadoCambio').val() == "") {
            return inputVacio = false;
        } else { inputVacio = true; }

        return inputVacio;
    }


    const ObtenerArchivos = function (idFormato) {
        axios.post('/CapacitacionSeguridad/obtenerArchivoCODescargas', { idFormatoCambio: idFormato })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, data } = response.data;
                if (success) {
                    btnDescargarExamen.attr('data-id', data[0].id)
                    btnDescargarReporteEvaluacion.attr('data-id', data[1].id)
                    btnDescargarSolicitudCambio.attr('data-id', data[2].id)
                    btnDescargarListadoCambio.attr('data-id', data[3].id)
                    $('#pExamen').text(data[0].rutaArchivo.split('\\')[8]);
                    $('#pReporteEvaluacion').text(data[1].rutaArchivo.split('\\')[8]);
                    $('#pSolicitudCambio').text(data[2].rutaArchivo.split('\\')[8]);
                    $('#pLimitadoCambio').text(data[3].rutaArchivo.split('\\')[8]);
                }
            });
    }
    const DescargarArchivo = function (id) {
        location.href = `DescargarArchivoCO?id=${id} `;
    }
    function AddRows(tbl, lst) {
        dtGestionCambios = tbl.DataTable();
        dtGestionCambios.clear().draw();
        dtGestionCambios.rows.add(lst).draw(false);
    }

    $(document).ready(() => {
        CambiosCategoria.Promedio = new Promedio();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...', baseZ: 2000 }); })
        .ajaxStop(() => { $.unblockUI(); });

    return {
        Inicializar: Inicializar
    }
};



