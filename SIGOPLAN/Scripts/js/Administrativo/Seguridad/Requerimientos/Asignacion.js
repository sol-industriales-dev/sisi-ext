(() => {
    $.namespace('Administrativo.Requerimientos.Asignacion');
    Asignacion = function () {
        //#region Selectores
        const selectCentroCosto = $('#selectCentroCosto');
        const tablaPuntos = $('#tablaPuntos');
        const botonBuscar = $('#botonBuscar');
        const botonAgregar = $('#botonAgregar');
        const modalAgregarPunto = $('#modalAgregarPunto');
        const selectClasificacionAgregar = $('#selectClasificacionAgregar');
        const selectCentroCostoAgregar = $('#selectCentroCostoAgregar');
        const selectRequerimiento = $('#selectRequerimiento');
        const inputFechaInicioEvaluacion = $('#inputFechaInicioEvaluacion');
        const botonGuardar = $('#botonGuardar');
        const selectActividad = $('#selectActividad');
        const selectCondicionante = $('#selectCondicionante');
        const selectSeccion = $('#selectSeccion');
        //#endregion

        let dtPuntos;

        // Datepicker variables.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();

        (function init() {
            agregarListeners();
            initTablaPuntos();
            initMonthPicker(inputFechaInicioEvaluacion);
            $('.select2').select2();

            // inputFechaInicioEvaluacion.datepicker({ dateFormat, minDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);
            selectCentroCosto.fillComboSeguridad(false);

            selectCentroCostoAgregar.fillComboSeguridad(false);
            convertToMultiselect('#selectCentroCostoAgregar');
            selectClasificacionAgregar.fillCombo('/Administrativo/Requerimientos/GetClasificacionCombo', null, false, 'Todos');
            convertToMultiselect('#selectClasificacionAgregar');
            selectRequerimiento.fillCombo('/Administrativo/Requerimientos/GetRequerimientosCombo', null, false, 'Todos');
            convertToMultiselect('#selectRequerimiento');
            selectActividad.fillCombo('/Administrativo/Requerimientos/GetActividadesCombo', null, false, 'Todos');
            convertToMultiselect('#selectActividad');
            selectCondicionante.fillCombo('/Administrativo/Requerimientos/GetCondicionantesCombo', null, false, 'Todos');
            convertToMultiselect('#selectCondicionante');
            selectSeccion.fillCombo('/Administrativo/Requerimientos/GetSeccionesCombo', null, false, 'Todos');
            convertToMultiselect('#selectSeccion');
        })();

        selectCentroCosto.on('change', function () {
            selectCentroCostoAgregar.val(selectCentroCosto.val());
            selectCentroCostoAgregar.select2().trigger('change');
        });

        selectClasificacionAgregar.on('change', function () {
            let clasificaciones = getValoresMultiples('#selectClasificacionAgregar');

            selectRequerimiento.fillCombo('/Administrativo/Requerimientos/GetRequerimientosAsignacionCombo', { clasificaciones }, false, 'Todos');
            convertToMultiselect('#selectRequerimiento');
            selectActividad.fillCombo('/Administrativo/Requerimientos/GetActividadesAsignacionCombo', null, false, 'Todos');
            convertToMultiselect('#selectActividad');
            selectCondicionante.fillCombo('/Administrativo/Requerimientos/GetCondicionantesAsignacionCombo', null, false, 'Todos');
            convertToMultiselect('#selectCondicionante');
            selectSeccion.fillCombo('/Administrativo/Requerimientos/GetSeccionesAsignacionCombo', null, false, 'Todos');
            convertToMultiselect('#selectSeccion');
        });

        selectRequerimiento.on('change', function () {
            let requerimientos = getValoresMultiples('#selectRequerimiento');

            selectActividad.fillCombo('/Administrativo/Requerimientos/GetActividadesAsignacionCombo', { requerimientos }, false, 'Todos');
            convertToMultiselect('#selectActividad');
            selectCondicionante.fillCombo('/Administrativo/Requerimientos/GetCondicionantesAsignacionCombo', null, false, 'Todos');
            convertToMultiselect('#selectCondicionante');
            selectSeccion.fillCombo('/Administrativo/Requerimientos/GetSeccionesAsignacionCombo', null, false, 'Todos');
            convertToMultiselect('#selectSeccion');
        });

        selectActividad.on('change', function () {
            let requerimientos = getValoresMultiples('#selectRequerimiento');
            let actividades = getValoresMultiples('#selectActividad');

            selectCondicionante.fillCombo('/Administrativo/Requerimientos/GetCondicionantesAsignacionCombo', { requerimientos, actividades }, false, 'Todos');
            convertToMultiselect('#selectCondicionante');
            selectSeccion.fillCombo('/Administrativo/Requerimientos/GetSeccionesAsignacionCombo', null, false, 'Todos');
            convertToMultiselect('#selectSeccion');
        });

        selectCondicionante.on('change', function () {
            let requerimientos = getValoresMultiples('#selectRequerimiento');
            let actividades = getValoresMultiples('#selectActividad');
            let condicionantes = getValoresMultiples('#selectCondicionante');

            selectSeccion.fillCombo('/Administrativo/Requerimientos/GetSeccionesAsignacionCombo', { requerimientos, actividades, condicionantes }, false, 'Todos');
            convertToMultiselect('#selectSeccion');
        });

        function agregarListeners() {
            botonBuscar.click(cargarAsignacion);
            botonGuardar.click(guardarAsignacion);
            botonAgregar.click(() => {
                inputFechaInicioEvaluacion.datepicker({ dateFormat, minDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);
                modalAgregarPunto.modal('show');
            });
        }

        function initTablaPuntos() {
            dtPuntos = tablaPuntos.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '45vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tablaPuntos.on('click', '.btn-eliminar', function () {
                        let rowData = dtPuntos.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de quitar la asignación del punto "${rowData.indice} - ${rowData.descripcion}"?`,
                            () => eliminarAsignacionPunto(rowData.id))
                    });
                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'requerimiento', title: 'Requerimiento' },
                    { data: 'requerimientoDesc', title: 'Título' },
                    { data: 'indice', title: 'Punto' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'codigo', title: 'Código' },
                    { data: 'fechaInicioEvaluacionString', title: 'Fecha Inicio Evaluación' },
                    {
                        title: 'Quitar', render: function (data, type, row, meta) {
                            return `<button class="btn-eliminar btn btn-sm btn-danger"><i class="fas fa-times"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '30%', targets: [1, 3] }
                ]
            });
        }

        function cargarAsignacion() {
            let idEmpresa = $(selectCentroCosto).getEmpresa();
            let strAgrupacion = $(selectCentroCosto).getAgrupador();
            let idAgrupacion;
            if (idEmpresa == 1000){
                idAgrupacion = strAgrupacion.replace("c_", "");
            } else if (idEmpresa == 2000) {
                idAgrupacion = strAgrupacion.replace("a_", "");
            } else {
                idAgrupacion = strAgrupacion;
            }

            if (idAgrupacion != '') {
                $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                $.post('/Administrativo/Requerimientos/GetAsignacion', { idEmpresa, idAgrupacion })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            if (response.data.length > 0) {
                                AddRows(tablaPuntos, response.data);
                            } else {
                                AlertaGeneral(`Alerta`, `El centro de costo no tiene puntos asignados.`);
                                dtPuntos.clear().draw();
                            }
                        } else {
                            AlertaGeneral(`Alerta`, response.message);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            }
        }

        function guardarAsignacion() {
            
            //#region SE ELIMINA LOS PARAMETROS ADICIONALES DEL VALUE EN CASO DE SER CONTRATISTA O AGRUPACION DE CONTRATISTAS
            let objGrupos = new Object();
            let arrGrupos = [];
            for (let i = 0; i < $("#selectCentroCostoAgregar").getMultiSeg().length; i++) {
                let str = $("#selectCentroCostoAgregar").getMultiSeg()[i].idAgrupacion;
                let idEmpresa = $("#selectCentroCostoAgregar").getMultiSeg()[i].idEmpresa;
                if (parseFloat(idEmpresa) == 1000) {
                    let idAgrupacion = str.replace("c_", "");
                    objGrupos = {
                        idEmpresa: idEmpresa,
                        idAgrupacion: parseFloat(idAgrupacion)
                    };
                    arrGrupos.push(objGrupos);
                } else if (parseFloat(idEmpresa) == 2000) {
                    let idAgrupacion = str.replace("a_", "");
                    objGrupos = {
                        idEmpresa: idEmpresa,
                        idAgrupacion: parseFloat(idAgrupacion)
                    };
                    arrGrupos.push(objGrupos);
                } else {
                    objGrupos = {
                        idEmpresa: $("#selectCentroCostoAgregar").getMultiSeg()[i].idEmpresa,
                        idAgrupacion: $("#selectCentroCostoAgregar").getMultiSeg()[i].idAgrupacion
                    };
                    arrGrupos.push(objGrupos);
                }
            }
            //#endregion

            let asignacion = {
                clasificaciones: getValoresMultiples('#selectClasificacionAgregar'),
                // arrGrupos: $("#selectCentroCostoAgregar").getMultiSeg(),
                arrGrupos: arrGrupos,
                fechaInicioEvaluacion: '01/' + inputFechaInicioEvaluacion.val(),
                requerimientos: getValoresMultiples('#selectRequerimiento'),
                actividades: getValoresMultiples('#selectActividad'),
                condicionantes: getValoresMultiples('#selectCondicionante'),
                secciones: getValoresMultiples('#selectSeccion')
            };

            if (asignacion.clasificaciones.length == 0 ||
                asignacion.arrGrupos.length == 0 ||
                asignacion.fechaInicioEvaluacion == '' ||
                asignacion.requerimientos.length == 0 ||
                asignacion.actividades.length == 0 ||
                asignacion.condicionantes.length == 0 ||
                asignacion.secciones.length == 0) {
                AlertaGeneral(`Alerta`, `Llene todos los campos de información.`);
                return;
            }

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/GuardarAsignacion', { asignacion })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalAgregarPunto.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarAsignacion();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function eliminarAsignacionPunto(id) {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/EliminarAsignacionPunto', { asignacionID: id })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarAsignacion();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
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

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Administrativo.Requerimientos.Asignacion = new Asignacion())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();