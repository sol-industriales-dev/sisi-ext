(() => {
    $.namespace('CH.Responsables');
    Responsables = function () {
        //#region Selectores
        const tblRH_Vacaciones_Vacaciones = $('#tblRH_Vacaciones_Vacaciones');
        const btnFiltroNuevo = $('#btnFiltroNuevo');
        const btnFiltroBuscar = $('#btnFiltroBuscar');
        const cboVacacionCC = $('#cboVacacionCC');
        const mdlVacacion = $('#mdlVacacion');
        const titleCEVacacion = $('#titleCEVacacion');
        const txtCEVacacionClaveEmp = $('#txtCEVacacionClaveEmp');
        const txtCEVacacionNombreEmp = $('#txtCEVacacionNombreEmp');
        const txtCEVacacionClaveResponsable = $('#txtCEVacacionClaveResponsable');
        const txtCEVacacionNombreResponsable = $('#txtCEVacacionNombreResponsable');
        const btnCEVacacionActualizar = $('#btnCEVacacionActualizar');
        const btnTxtCEVacacion = $('#btnTxtCEVacacion');
        const cboCEVacacionPeriodo = $('#cboCEVacacionPeriodo');
        const inputDias = $('#inputDias');
        const modalHistorial = $('#modalHistorial');
        const tablaHistorial = $('#tablaHistorial');
        const btnDiasSub = $('#btnDiasSub');
        const btnDiasSum = $('#btnDiasSum');
        // const btnMenosDiasPaternidad = $("#btnMenosDiasPaternidad");
        // const txtCantDiasVacacionesPaternidad = $("#txtCantDiasVacacionesPaternidad");
        // const btnMasDiasPaternidad = $("#btnMasDiasPaternidad");
        // const btnMenosDiasMatrimonio = $("#btnMenosDiasMatrimonio");
        // const txtCantDiasVacacionesMatrimonio = $("#txtCantDiasVacacionesMatrimonio");
        // const btnMasDiasMatrimonio = $("#btnMasDiasMatrimonio");
        //#endregion

        let dtRH_Vacaciones_Vacaciones;
        let dtHistorial;

        (function init() {
            initTblRH_Vacaciones_Vacaciones();
            initTablaHistorial();
            getResponsables();
            fncListeners();
        })();

        function fncListeners() {
            cboCEVacacionPeriodo.fillCombo('/Administrativo/Vacaciones/FillComboPeriodos', {}, false);
            cboVacacionCC.fillCombo('/Administrativo/Vacaciones/FillComboCC', {}, false);

            btnFiltroBuscar.click(getResponsables);
            btnCEVacacionActualizar.click(fncCrearEditarResponsable);

            btnFiltroNuevo.on("click", function () {
                fncLimpiarModal();
                mdlVacacion.modal("show");
                titleCEVacacion.text("VACACIONES EMPLEADO");
                btnTxtCEVacacion.html("<i class='fa fa-save'></i>&nbsp;Guardar");
            });


            txtCEVacacionNombreResponsable.getAutocomplete(funGetGerente, null, '/Administrativo/FormatoCambio/getCatEmpleadosReclutamientos');
            txtCEVacacionNombreEmp.getAutocomplete(funGetEmpleado, null, '/Administrativo/FormatoCambio/getCatEmpleadosReclutamientos');

            txtCEVacacionClaveEmp.on("change", function () {
                fncGetDatosPersonal(txtCEVacacionClaveEmp.val(), txtCEVacacionNombreResponsable.val(), false);
            });

            txtCEVacacionClaveResponsable.on("change", function () {
                fncGetDatosPersonal(txtCEVacacionClaveResponsable.val(), txtCEVacacionNombreResponsable.val(), true);
            });

            // FUNCIONES PARA AUMENTAR/REDUCIR CANTIDAD DIAS DE VACACIONES GENERALES
            btnDiasSub.on("click", function () {
                inputDias.val(Number(inputDias.val()) - 1);
            });

            btnDiasSum.on("click", function () {
                inputDias.val(Number(inputDias.val()) + 1);
            });
            // END

            // FUNCIONES PARA AUMENTAR/REDUCIR CANTIDAD DIAS DE VACACIONES PATERNIDAD
            // btnMenosDiasPaternidad.on("click", function () {
            //     txtCantDiasVacacionesPaternidad.val(Number(txtCantDiasVacacionesPaternidad.val()) - 1);
            // });

            // btnMasDiasPaternidad.on("click", function () {
            //     txtCantDiasVacacionesPaternidad.val(Number(txtCantDiasVacacionesPaternidad.val()) + 1);
            // });
            // END

            // FUNCIONES PARA AUMENTAR/REDUCIR CANTIDAD DIAS DE VACACIONES MATERNIDAD
            // btnMenosDiasMatrimonio.on("click", function () {
            //     txtCantDiasVacacionesMatrimonio.val(Number(txtCantDiasVacacionesMatrimonio.val()) - 1);
            // });

            // btnMasDiasMatrimonio.on("click", function () {
            //     txtCantDiasVacacionesMatrimonio.val(Number(txtCantDiasVacacionesMatrimonio.val()) + 1);
            // });
            // END
        }

        function initTblRH_Vacaciones_Vacaciones() {
            dtRH_Vacaciones_Vacaciones = tblRH_Vacaciones_Vacaciones.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreEmpleado', title: 'Empleado' },
                    {
                        title: 'Fecha de Ingreso',
                        render: function (data, type, row, meta) {
                            return moment(row.fecha_ingreso).format("DD/MM/YYYY");
                        }
                    },
                    { data: 'ccDesc', title: 'Centro de Costos' },
                    { data: 'nombreResponsable', title: 'Responsable', visible: false },
                    {
                        data: 'diasIniciales', title: 'Días Iniciales', render: function (data, type, row, meta) {
                            if (data > 0) {
                                return `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Ver Historial"><span>${data}</span>&nbsp;<button class="btn btn-xs btn-default botonHistorial"><i class="fa fa-align-justify"></i></button></span>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    { data: 'diasTomados', title: 'Días Tomados' },
                    { data: 'diasPendientes', title: 'Días Pendientes' },
                    // { data: 'diasPaternidad', title: 'Días paternidad' },
                    // { data: 'diasMatrimonio', title: 'Días matrimonio' },
                    {
                        render: function (data, type, row) {
                            return `
                                <button title="Actualizar" class="btn btn-warning botonActualizar btn-xs"><i class="far fa-edit"></i></button>
                                <button title="Eliminar" class="btn btn-danger botonEliminar btn-xs"><i class="far fa-trash-alt"></i></button>
                            `;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblRH_Vacaciones_Vacaciones.on('click', '.botonActualizar', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();
                        fncLimpiarModal();
                        titleCEVacacion.text("VACACIONES EMPLEADO");
                        btnTxtCEVacacion.html(`<i class='fa fa-save'></i>&nbsp;Actualizar`);

                        btnCEVacacionActualizar.attr("data-id", rowData.id);
                        txtCEVacacionNombreEmp.val(rowData.nombreEmpleado);
                        txtCEVacacionClaveEmp.val(rowData.clave_empleado);
                        txtCEVacacionNombreResponsable.val(rowData.nombreResponsable);
                        txtCEVacacionClaveResponsable.val(rowData.clave_responsable);

                        if (!rowData.esDiasIniciales) {
                            btnDiasSum.attr("disabled", true);
                            btnDiasSub.attr("disabled", true);
                            // btnMasDiasPaternidad.attr("disabled", true);
                            // btnMenosDiasPaternidad.attr("disabled", true);
                            // btnMasDiasMatrimonio.attr("disabled", true);
                            // btnMenosDiasMatrimonio.attr("disabled", true);
                        } else {
                            btnDiasSum.attr("disabled", false);
                            btnDiasSub.attr("disabled", false);
                            // btnMasDiasPaternidad.attr("disabled", false);
                            // btnMenosDiasPaternidad.attr("disabled", false);
                            // btnMasDiasMatrimonio.attr("disabled", false);
                            // btnMenosDiasMatrimonio.attr("disabled", false);
                        }

                        inputDias.val(rowData.diasIniciales);
                        // txtCantDiasVacacionesPaternidad.val(rowData.diasPaternidad);
                        // txtCantDiasVacacionesMatrimonio.val(rowData.diasMatrimonio);

                        mdlVacacion.modal("show");
                    });

                    tblRH_Vacaciones_Vacaciones.on('click', '.botonEliminar', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el responsable seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarResponsable(rowData.id));
                    });

                    tblRH_Vacaciones_Vacaciones.on('click', '.botonHistorial', function () {
                        let rowData = dtRH_Vacaciones_Vacaciones.row($(this).closest('tr')).data();

                        cargarHistorial(rowData);
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    { width: '20%', targets: [0] },
                    { width: '10%', targets: [1] },
                    { width: '5%', targets: [4, 5, 6, 7] }
                ],
            });
        }

        function initTablaHistorial() {
            dtHistorial = tablaHistorial.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                ordering: false,
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'dias', title: 'Días' },
                    { data: 'nombreUsuario', title: 'Usuario' },
                    { data: 'fechaString', title: 'Fecha' }
                ],
                columnDefs: [
                    { className: 'dt-center', targets: '_all' }
                ]
            });
        }

        function cargarHistorial(rowData) {
            axios.post('GetHistorialDias', { id: rowData.clave_empleado })
                .then(response => {
                    let { success, data, message } = response.data;

                    if (success) {
                        AddRows(tablaHistorial, data);
                        modalHistorial.modal('show');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function getResponsables() {
            axios.post('GetResponsables', { cc: cboVacacionCC.val(), claveEmpleado: 0 })
                .then(response => {
                    let { success, data, message } = response.data;

                    if (success) {
                        AddRows(tblRH_Vacaciones_Vacaciones, data);
                        fncDefaultBorder();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function fncCrearEditarResponsable() {
            if (+txtCEVacacionClaveEmp.val() >= 0) {
                let obj = new Object();
                obj.claveEmpleado = +txtCEVacacionClaveEmp.val();
                obj.dias = +inputDias.val();
                // obj.diasPaternidad = +txtCantDiasVacacionesPaternidad.val();
                // obj.diasMatrimonio = +txtCantDiasVacacionesMatrimonio.val();

                axios.post("CrearEditarResponsable", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        getResponsables();
                        Alert2Exito(message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Es necesario indicar la clave del empleado.");
            }
        }

        function fncEliminarResponsable(id) {
            axios.post("EliminarResponsable", { id }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se ha eliminado la información.");
                    getResponsables();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetDatosPersonal(claveUsuario, nombreUsuario, esGerente) {
            axios.post("GetDatosPersona", { claveEmpleado: claveUsuario, nombre: nombreUsuario }).then(response => {
                let { success, items, message } = response.data;

                if (success) {
                    if (esGerente) {
                        txtCEVacacionNombreResponsable.val(response.data.objDatosPersona.nombreCompleto);
                    } else {
                        txtCEVacacionNombreEmp.val(response.data.objDatosPersona.nombreCompleto);
                    }
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncLimpiarModal() {
            txtCEVacacionNombreEmp.val("");
            txtCEVacacionClaveEmp.val("");
            txtCEVacacionClaveResponsable.val("");
            txtCEVacacionNombreResponsable.val("");
            inputDias.val('');
            btnCEVacacionActualizar.attr("data-id", 0);
        }

        function GetReponsableParametros() {
            let strMessage = "";
            if (txtCEVacacionClaveEmp.val() == "") { txtCEVacacionClaveEmp.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }
            // if (txtCEVacacionClaveResponsable.val() == "") { txtCEVacacionClaveResponsable.css("border", "2px solid red"); strMessage = "Es necesario llenar los campos obligatorios."; }

            if (strMessage != "") {
                Alert2Warning(strMessage)
                return "";
            } else {
                let obj = {
                    id: btnCEVacacionActualizar.attr("data-id"),
                    clave_empleado: txtCEVacacionClaveEmp.val()
                    // clave_responsable: txtCEVacacionClaveResponsable.val(),
                }
                return obj;
            }
        }

        function fncDefaultBorder() {
            txtCEVacacionClaveEmp.css("border", "1px solid #CCC");
            txtCEVacacionClaveResponsable.css("border", "1px solid #CCC");

        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
        function funGetGerente(event, ui) {
            txtCEVacacionClaveResponsable.val(ui.item.id);
            txtCEVacacionNombreResponsable.val(ui.item.value);

        }
        function funGetEmpleado(event, ui) {
            txtCEVacacionClaveEmp.val(ui.item.id);
            txtCEVacacionNombreEmp.val(ui.item.value);
            txtCEVacacionClaveEmp.change();
        }
    }
    $(document).ready(() => CH.Responsables = new Responsables())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();