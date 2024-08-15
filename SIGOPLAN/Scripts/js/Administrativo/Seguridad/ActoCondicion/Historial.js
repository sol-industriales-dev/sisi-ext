(() => {
    $.namespace('Administrativo.ActoCondicion.Historial');

    Historial = function () {

        // Variables.
        const report = $('#report');

        //Historial.
        const inputClaveEmpleado = $('#inputClaveEmpleado');
        const inputNombreEmpleado = $('#inputNombreEmpleado');
        const botonBuscarEmpleado = $('#botonBuscarEmpleado');

        const tablaHistorial = $('#tablaHistorial');
        let dtTablaHistorial = null;

        // Matrices.
        const comboCC = $('#comboCC');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        const comboSupervisor = $('#comboSupervisor');
        const comboDepartamento = $('#comboDepartamento');
        const comboSubclasificacion = $('#comboSubclasificacion');
        const botonBuscarMatrices = $('#botonBuscarMatrices');

        const botonExportarActos = $('#botonExportarActos');
        const tablaMatrizActo = $('#tablaMatrizActo');
        let dtTablaMatrizActo = null;

        const botonExportarCondiciones = $('#botonExportarCondiciones');
        const tablaMatrizCondicion = $('#tablaMatrizCondicion');
        let dtTablaMatrizCondicion = null;

        const chkEsContratista = $('#chkEsContratista');
        const divChkEsContratista = $('#divChkEsContratista');

        // Datepicker variables.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        const fechaInicioAnio = new Date(new Date().getFullYear(), 0, 1);

        (function init() {
            // Lógica de inicialización.
            llenarCombos();
            initDatepickers();
            agregarListeners();
            initTablaHistorial();
            initTablaMatrizActo();
            initTablaMatrizCondicion();
            botonExportarActos.hide();
            botonExportarCondiciones.hide();

            // Se comprueba si hay variables en url.
            const variables = getUrlParams(window.location.href);

            if (variables && variables.claveEmpleado) {
                inputClaveEmpleado.val(variables.claveEmpleado);
                inputClaveEmpleado.blur();
                botonBuscarEmpleado.click();
            }

        })();

        // Métodos.
        function llenarCombos() {
            comboCC.fillComboActoCondicion(false);
            convertToMultiselect('#comboCC');
            comboSupervisor.fillCombo('/Administrativo/ActoCondicion/ObtenerSupervisores', null, false, 'Todos');
            comboDepartamento.fillCombo('/Administrativo/ActoCondicion/ObtenerDepartamentos', null, false, 'Todos');
            comboSubclasificacion.fillCombo('/Administrativo/ActoCondicion/FillCboSubclasificaciones', null, false, 'Todos');

            $('.select2').select2();
        }

        function initDatepickers() {
            inputFechaInicio.datepicker({ dateFormat, showAnim }).datepicker("setDate", fechaInicioAnio);
            inputFechaFin.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);
        }

        function agregarListeners() {
            inputClaveEmpleado.on('blur', cargarInfoEmpleado);
            botonBuscarEmpleado.click(cargarHistorial);
            botonBuscarMatrices.click(cargarMatrices);
            botonExportarActos.click(descargarExcelMatrizActos);
            botonExportarCondiciones.click(descargarExcelMatrizCondiciones);
            inputClaveEmpleado.click(function (e) {
                $(this).select();
            })
        }

        function getUrlParams(url) {
            let params = {};
            let parser = document.createElement('a');
            parser.href = url;
            let query = parser.search.substring(1);
            let vars = query.split('&');

            for (let i = 0; i < vars.length; i++) {
                let pair = vars[i].split('=');
                params[pair[0]] = decodeURIComponent(pair[1]);
            }

            return params;
        };

        function cargarInfoEmpleado() {

            const claveEmpleado = $(this).val();

            if (claveEmpleado == "") {
                inputNombreEmpleado.val('');
                return;
            }

            let esContratista = false;
            if (chkEsContratista.prop("checked")) {
                esContratista = true;
            }

            $.post('/Administrativo/IndicadoresSeguridad/GetInfoEmpleado', { claveEmpleado: claveEmpleado, esContratista: esContratista, idEmpresaContratista: 0 })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        inputNombreEmpleado.val(response.empleadoInfo.nombreEmpleado);
                    } else {
                        // Operación no completada.
                        inputClaveEmpleado.val('');
                        inputNombreEmpleado.val('');
                    }
                }, error => AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`));
        }

        function cargarHistorial() {
            const claveEmpleado = inputClaveEmpleado.val();

            if (claveEmpleado == "") {
                AlertaGeneral(`Aviso`, `Debe ingresar a un empleado válido para ver su historial.`);
                return;
            }

            $.get('/Administrativo/ActoCondicion/ObtenerHistorialEmpleado', { claveEmpleado })
                .then(response => {

                    dtTablaHistorial.clear();

                    if (response.success) {
                        // Operación exitosa.
                        dtTablaHistorial.rows.add(response.items).draw();
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }

                    dtTablaHistorial.draw();

                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }


        function cargarMatrices() {

            const filtro = obtenerFiltroBusqueda();

            $.post('/Administrativo/ActoCondicion/ObtenerMatrices', { filtro })
                .then(response => {

                    dtTablaMatrizActo.clear();
                    dtTablaMatrizCondicion.clear();

                    if (response.success) {
                        // Operación exitosa.
                        if (response.actos && response.actos.length > 0) {
                            dtTablaMatrizActo.rows.add(response.actos);
                            botonExportarActos.show();
                        } else {
                            botonExportarActos.hide();
                        }

                        if (response.condiciones && response.condiciones.length > 0) {
                            dtTablaMatrizCondicion.rows.add(response.condiciones);
                            botonExportarCondiciones.show();
                        } else {
                            botonExportarCondiciones.hide();
                        }

                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }

                    dtTablaMatrizActo.draw();
                    dtTablaMatrizCondicion.draw();

                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );

        }

        function obtenerFiltroBusqueda() {
            const claveSupervisor = comboSupervisor.val();
            const departamentoID = comboDepartamento.val();
            const subclasificacion = comboSubclasificacion.val();
            // const arrGrupos = $("#comboCC").getMultiSeg();

            //#region SE ELIMINA LOS PARAMETROS ADICIONALES DEL VALUE EN CASO DE SER CONTRATISTA O AGRUPACION DE CONTRATISTAS
            let objGrupos = new Object();
            let arrGrupos = [];
            for (let i = 0; i < $("#comboCC").getMultiSeg().length; i++) {
                let str = $("#comboCC").getMultiSeg()[i].idAgrupacion;
                let idEmpresa = $("#comboCC").getMultiSeg()[i].idEmpresa;
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
                        idEmpresa: $("#comboCC").getMultiSeg()[i].idEmpresa,
                        idAgrupacion: $("#comboCC").getMultiSeg()[i].idAgrupacion
                    };
                    arrGrupos.push(objGrupos);
                }
            }
            console.log(arrGrupos);
            //#endregion

            return {
                arrGrupos: arrGrupos,
                fechaInicial: inputFechaInicio.val(),
                fechaFinal: inputFechaFin.val(),
                claveSupervisor: claveSupervisor == "Todos" ? 0 : claveSupervisor,
                departamentoID: departamentoID == "Todos" ? 0 : departamentoID,
                subclasificacion: subclasificacion == "Todos" ? 0 : subclasificacion
            };
        }

        function obtenerReporte(id) {
            axios.post('/Administrativo/ActoCondicion/ObtenerReporteActoCondicion',
                {
                    id,
                    tipo: 1 //ACTO
                }).then(response => {
                    let { success, items } = response.data;
                    if (success) {
                        $.blockUI({ message: 'Generando imprimible...' });
                        var path = `/Reportes/Vista.aspx?idReporte=227`;
                        report.attr("src", path);
                        document.getElementById('report').onload = function () {
                            $.unblockUI();
                            openCRModal();
                        };
                    }
                });
        }

        function descargarExcelMatrizActos() {
            location.href = `DescargarExcelMatrizActos`;
        }

        function descargarExcelMatrizCondiciones() {
            location.href = `DescargarExcelMatrizCondiciones`;
        }

        function initTablaHistorial() {
            dtTablaHistorial = tablaHistorial.DataTable({
                paging: false,
                language: dtDicEsp,
                order: [[7, "desc"]],
                searching: false,
                columns: [
                    { data: 'folio', title: 'Folio' },
                    { data: 'proyecto', title: 'Proyecto' },
                    { data: 'tipoActoDesc', title: 'Tipo' },
                    { data: 'accionDesc', title: 'Accion' },
                    { data: 'departamentoDesc', title: 'Departamento' },
                    { data: 'clasificacionDesc', title: 'Clasificación' },
                    { data: 'procedimientoDesc', title: 'Procedimiento' },
                    { data: 'fechaSuceso', title: 'Fecha' },
                    {
                        data: 'id', title: 'Descargar Evidencia', render: (data, type, row) => {
                            const btnEvidencia =
                                `<button title="Descargar archivo evidencia" class="btn-descargar btn btn-primary">
                                    <i class="fas fa-file-download"></i>
                                </button>`;

                            const btnReporte =
                                `<button title="Reporte" class="btn-reporte btn btn-info">
                                    <i class="fas fa-file-pdf"></i>
                                </button>`;

                            if (row.tieneEvidencia) {
                                return btnEvidencia + ' ' + btnReporte;
                            } else {
                                return btnReporte;
                            }
                        }
                    }
                ],
                drawCallback: function (settings, json) {
                    tablaHistorial.find('.btn-descargar').off().click(function () {
                        const rowData = dtTablaHistorial.row($(this).closest('tr')).data();
                        location.href = `DescargarActo?actoID=${rowData.id}`;
                    });

                    tablaHistorial.find('.btn-reporte').off().click(e => {
                        const rowData = dtTablaHistorial.row($(e.currentTarget).closest('tr')).data();

                        obtenerReporte(rowData.id);
                    });
                }
            });
        }

        function initTablaMatrizActo() {
            dtTablaMatrizActo = tablaMatrizActo.DataTable({
                paging: false,
                language: dtDicEsp,
                order: [[2, "desc"]],
                searching: false,
                columns: [
                    { data: 'folio', title: 'Folio' },
                    { data: 'proyecto', title: 'Proyecto' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'fechaSuceso', title: 'Fecha Detección' },
                    { data: 'accionDesc', title: 'Accion' }
                ]
            });
        }

        function initTablaMatrizCondicion() {
            dtTablaMatrizCondicion = tablaMatrizCondicion.DataTable({
                paging: false,
                language: dtDicEsp,
                order: [[2, "desc"]],
                searching: false,
                columns: [
                    { data: 'folio', title: 'Folio' },
                    { data: 'proyecto', title: 'Proyecto' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'fechaSuceso', title: 'Fecha Detección' },
                    { data: 'fechaResolucion', title: 'Fecha Correción' }
                ]
            });
        }

    }

    $(() => Administrativo.ActoCondicion.Historial = new Historial())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();