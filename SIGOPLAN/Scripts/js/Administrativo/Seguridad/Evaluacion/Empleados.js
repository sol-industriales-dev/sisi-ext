(() => {
    $.namespace('Administrativo.Evaluacion.Empleados');

    Empleados = function () {
        //#region Selectores
        const tablaEmpleados = $('#tablaEmpleados');
        const botonAgregar = $('#botonAgregar');
        const modalEmpleado = $('#modalEmpleado');
        const tablaEvaluadores = $('#tablaEvaluadores');
        const botonGuardar = $('#botonGuardar');
        const inputClaveEmpleado = $('#inputClaveEmpleado');
        const inputNombre = $('#inputNombre');
        const inputApellidoPaterno = $('#inputApellidoPaterno');
        const inputApellidoMaterno = $('#inputApellidoMaterno');
        const selectPuesto = $('#selectPuesto');
        const checkBoxEvaluador = $('#checkBoxEvaluador');
        const selectRol = $('#selectRol');
        const inputFechaInicioRol = $('#inputFechaInicioRol');
        const selectCentroCosto = $('#selectCentroCosto');
        const chkEsContratista = $('#chkEsContratista');
        const lblContratista = $('#lblContratista');
        //#endregion

        let dtEmpleados;
        let dtEvaluadores;
        const ESTATUS = {
            NUEVO: 0,
            EDITAR: 1
        };

        // Datepicker variables.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();

        (function init() {
            $('.select2').select2();
            initTablaEmpleados();
            initTablaEvaluadores();
            agregarListeners();
            cargarEmpleados();
            fncValidarAccesoContratista();

            selectPuesto.fillCombo('/Administrativo/Evaluacion/GetPuestosCombo', null, false);
            selectRol.fillCombo('/Administrativo/Evaluacion/GetRolesCombo', null, false);
            selectCentroCosto.fillComboSeguridad(false);

            inputFechaInicioRol.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);

            chkEsContratista.change(function (e){
                if (inputClaveEmpleado.val() != "") { inputClaveEmpleado.trigger("change"); }
            });
        })();

        modalEmpleado.on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        function initTablaEmpleados() {
            dtEmpleados = tablaEmpleados.DataTable({
                retrieve: true,
                paging: false,
                // searching: false,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tablaEmpleados.on('click', '.btn-editar', function () {
                        let rowData = dtEmpleados.row($(this).closest('tr')).data();

                        limpiarModal();
                        llenarCamposModal(rowData);
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;
                        cargarEvaluadores(true, rowData.id);
                        modalEmpleado.modal('show');
                    });

                    tablaEmpleados.on('click', '.btn-eliminar', function () {
                        let rowData = dtEmpleados.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación',
                            `¿Está seguro de eliminar el empleado "${rowData.nombre} ${rowData.apellidoPaterno} ${rowData.apellidoMaterno}"? ATENCIÓN: Se perderá la información de las evidencias capturas por este usuario y toda su información relacionada.`,
                            () => eliminarEmpleado(rowData.id))
                    });
                },
                columns: [
                    { data: 'claveEmpleado', title: 'Clave Empleado' },
                    {
                        title: 'Nombre', render: function (data, type, row, meta) {
                            return `${row.nombre} ${row.apellidoPaterno} ${row.apellidoMaterno}`;
                        }
                    },
                    { data: 'puestoDesc', title: 'Puesto' },
                    {
                        data: 'evaluador', title: 'Evaluador', render: function (data, type, row, meta) {
                            return data ? 'SÍ' : 'NO';
                        }
                    },
                    {
                        render: function (data, type, row, meta) {
                            return `
                            <button title="Editar" class="btn-editar btn btn-warning">
                                <i class="fas fa-pencil-alt"></i>
                            </button>
                            &nbsp;
                            <button title="Eliminar" class="btn-eliminar btn btn-danger">
                                <i class="fas fa-trash"></i>
                            </button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '10%', targets: [4] }
                ]
            });
        }

        function fncValidarAccesoContratista() {
            axios.post("/Administrativo/IndicadoresSeguridad/ValidarAccesoContratista").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    botonGuardar.css("display", "none");
                    chkEsContratista.css("display", "none");
                    lblContratista.css("display", "none");
                    botonGuardar.attr("data-esContratista", true);
                } else {
                    botonGuardar.attr("data-esContratista", false);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTablaEvaluadores() {
            dtEvaluadores = tablaEvaluadores.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '45vh',
                scrollCollapse: true,
                order: [],
                initComplete: function (settings, json) {
                    tablaEvaluadores.on('click', 'input[type="checkbox"]', function () {
                        let row = $(this).closest('tr');
                        let checked = $(this).prop('checked');
                    });
                },
                createdRow: function (row, rowData) {
                    $(row).find('.checkBoxAplica').prop('checked', rowData.aplica);
                },
                columns: [
                    { data: 'claveEmpleado', title: 'Clave Empleado' },
                    {
                        title: 'Nombre', render: function (data, type, row, meta) {
                            return `${row.nombre} ${row.apellidoPaterno} ${row.apellidoMaterno}`;
                        }
                    },
                    { data: 'puestoDesc', title: 'Puesto' },
                    {
                        title: 'Aplica', render: function (data, type, row, meta) {
                            let div = document.createElement('div');
                            let checkbox = document.createElement('input');

                            checkbox.id = 'checkboxAplica_' + meta.row;
                            checkbox.setAttribute('type', 'checkbox');
                            checkbox.classList.add('form-control');
                            checkbox.classList.add('regular-checkbox');
                            checkbox.classList.add('checkBoxAplica');
                            checkbox.style.height = '25px';

                            let label = document.createElement('label');
                            label.setAttribute('for', checkbox.id);

                            $(div).append(checkbox);
                            $(div).append(label);

                            return div.outerHTML;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarEmpleados() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Evaluacion/GetEmpleados')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tablaEmpleados, response.data);
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function cargarEvaluadores(editar, empleadoID) {
            if (editar) {
                $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                $.post('/Administrativo/Evaluacion/GetEvaluadoresEmpleado', { empleadoID })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            AddRows(tablaEvaluadores, response.data);
                        } else {
                            AlertaGeneral(`Alerta`, response.message);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            } else {
                $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                $.post('/Administrativo/Evaluacion/GetEvaluadores')
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            AddRows(tablaEvaluadores, response.data);
                        } else {
                            AlertaGeneral(`Alerta`, response.message);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            }
        }

        function agregarListeners() {
            botonAgregar.click(() => {
                limpiarModal();
                botonGuardar.data().estatus = ESTATUS.NUEVO;
                botonGuardar.data().id = 0;
                cargarEvaluadores(false, 0);
                modalEmpleado.modal('show');
            });
            botonGuardar.click(guardarEmpleado);
            inputClaveEmpleado.change(cargarEmpleadoPorClave);
            inputClaveEmpleado.click(function (e){
                $(this).select();
            });
        }

        function limpiarModal() {
            inputClaveEmpleado.val('');
            inputNombre.val('');
            inputApellidoPaterno.val('');
            inputApellidoMaterno.val('');
            selectPuesto.val('');
            checkBoxEvaluador.prop('checked', false);
            selectRol.val('');
            inputFechaInicioRol.val($.datepicker.formatDate('dd/mm/yy', fechaActual));
            selectCentroCosto.val('');
            selectCentroCosto.select2().trigger('change');

            dtEvaluadores.clear().draw();
        }

        function llenarCamposModal(data) { //TODO
            let idEmpresa = data.idEmpresa;
            let idAgrupacion = data.idAgrupacion;
            let strAgrupacion;
            if (idEmpresa == 1000) {
                let str = "c_";
                strAgrupacion = str.concat(idAgrupacion);
            } else if (idEmpresa == 2000) {
                let str = "a_";
                strAgrupacion = str.concat(idAgrupacion);
            } else {
                strAgrupacion = idAgrupacion;
            }

            inputClaveEmpleado.val(data.claveEmpleado);
            inputNombre.val(data.nombre);
            inputApellidoPaterno.val(data.apellidoPaterno);
            inputApellidoMaterno.val(data.apellidoMaterno);
            selectPuesto.val(data.puestoEvaluacionID);
            checkBoxEvaluador.prop('checked', data.evaluador);
            selectRol.val(data.rol != 0 ? data.rol : '');
            inputFechaInicioRol.val($.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.fechaInicioRol.substr(6))))); //TODO
            selectCentroCosto.val(strAgrupacion);
            selectCentroCosto.select2().trigger('change');
        }

        function guardarEmpleado() {
            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    nuevoEmpleado();
                    break;
                case ESTATUS.EDITAR:
                    editarEmpleado();
                    break;
            }
        }

        function nuevoEmpleado() {
            let empleado = getInformacionEmpleado();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Evaluacion/GuardarNuevoEmpleado', { empleado, evaluadores: empleado.evaluadores })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalEmpleado.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarEmpleados();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
            );
        }

        function editarEmpleado() {
            let empleado = getInformacionEmpleado();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Evaluacion/EditarEmpleado', { empleado, evaluadores: empleado.evaluadores })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalEmpleado.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarEmpleados();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function eliminarEmpleado(id) {
            let empleado = { id };

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Evaluacion/EliminarEmpleado', { empleado })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarEmpleados();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function getInformacionEmpleado() {
            let evaluadores = getEvaluadoresAplica();

            let idEmpresa = $(selectCentroCosto).getEmpresa();
            let strAgrupacion = $(selectCentroCosto).getAgrupador();
            let idAgrupacion;
            if (idEmpresa == 1000) {
                idAgrupacion = strAgrupacion.replace("c_" , "");
            } else if (idEmpresa == 2000) {
                idAgrupacion = strAgrupacion.replace("a_", "");
            } else {
                idAgrupacion = strAgrupacion;
            }
            console.log(idEmpresa);
            console.log(idAgrupacion);

            return {
                id: botonGuardar.data().id,
                claveEmpleado: inputClaveEmpleado.val(),
                nombre: inputNombre.val(),
                apellidoPaterno: inputApellidoPaterno.val(),
                apellidoMaterno: inputApellidoMaterno.val(),
                puestoEvaluacionID: selectPuesto.val(),
                evaluador: checkBoxEvaluador.prop('checked'),
                rol: selectRol.val(),
                fechaInicioRol: inputFechaInicioRol.val(),
                estatus: true,
                cc: "",
                idEmpresa: idEmpresa,
                idAgrupacion: idAgrupacion,
                evaluadores: evaluadores
            };
        }

        function getEvaluadoresAplica() {
            let evaluadores = [];

            tablaEvaluadores.find('tbody tr').each(function (index, row) {
                let checkbox = $(row).find('.checkBoxAplica');

                if (checkbox.prop('checked')) {
                    let rowData = dtEvaluadores.row(row).data();

                    evaluadores.push({
                        id: rowData.id,
                        claveEmpleado: rowData.claveEmpleado,
                        nombre: rowData.nombre,
                        apellidoPaterno: rowData.apellidoPaterno,
                        apellidoMaterno: rowData.apellidoMaterno,
                        puestoEvaluacionID: rowData.puestoEvaluacionID,
                        evaluador: rowData.evaluador,
                        estatus: rowData.estatus,
                        aplica: true
                    });
                }
            });

            return evaluadores;
        }

        function cargarEmpleadoPorClave() {
            let claveEmpleado = +(inputClaveEmpleado.val());
            
            if (claveEmpleado > 0) {

                let attrEsContratista = botonGuardar.attr("data-esContratista");
                let idEmpresa = 0;
                let esContratista = false;
                if (attrEsContratista == "true") {
                    esContratista = true;
                } else {
                    if (chkEsContratista.prop('checked')) {
                        esContratista = true;
                    } else {
                        esContratista = false;
                    }
                }

                $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                $.post('/Administrativo/Evaluacion/GetEmpleadoPorClave', { 
                    claveEmpleado: claveEmpleado, esContratista: esContratista, idEmpresaContratista: idEmpresa 
                    }).always($.unblockUI).then(response => {
                        if (response.success) {
                            inputNombre.val(response.data.nombre);
                            inputApellidoPaterno.val(response.data.apellidoPaterno);
                            inputApellidoMaterno.val(response.data.apellidoMaterno);
                        } else {
                            AlertaGeneral(`Alerta`, response.message);
                        }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
            }
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }

    $(document).ready(() => Administrativo.Evaluacion.Empleados = new Empleados())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();