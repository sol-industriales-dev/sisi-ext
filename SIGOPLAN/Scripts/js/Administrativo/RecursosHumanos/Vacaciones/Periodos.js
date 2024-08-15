(() => {
    $.namespace('CH.Periodos');




    //******CONTROL DE AUSENCIAS*********




    //#region CONST PERIODOS

    const tblRH_Vacaciones_Periodos = $('#tblRH_Vacaciones_Periodos');
    const btnFiltroNuevo = $('#btnFiltroNuevo');

    let dttblRH_Vacaciones_Periodos;

    //#endregion

    //#region CONST MDLCREAREDITAR

    const mdlCEPeriodo = $('#mdlCEPeriodo');
    const titleCEPeriodo = $('#titleCEPeriodo');
    const txtCEPeriodoDesc = $('#txtCEPeriodoDesc');
    const txtCEPeriodoFechaini = $('#txtCEPeriodoFechaini');
    const txtCEPeriodoFechafin = $('#txtCEPeriodoFechafin');
    const btnCEPeriodoActualizar = $('#btnCEPeriodoActualizar');
    const btnTxtCEPeriodo = $('#btnTxtCEPeriodo');
    const chkCEPeriodoEstado = $('#chkCEPeriodoEstado');
    //#endregion

    Periodos = function () {
        (function init() {
            initTblRH_Vacaciones_Periodos();
            fncGetPeriodos();
            fncListeners();
        })();

        function fncListeners() {

            btnFiltroNuevo.on("click", function () {
                fncDefaultBorder();
                fncEmptyFields();
                mdlCEPeriodo.modal("show");
                btnCEPeriodoActualizar.attr('data-id', 0);
                btnTxtCEPeriodo.text("Guardar");
                titleCEPeriodo.text("CREAR PERIODO");
            });

            btnCEPeriodoActualizar.on("click", function () {
                fncCrearEditarPeriodo();
            });
        }

        //#region TBLS

        function initTblRH_Vacaciones_Periodos() {
            dttblRH_Vacaciones_Periodos = tblRH_Vacaciones_Periodos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                drawCallback: function (row, data) {
                    tblRH_Vacaciones_Periodos.find('input.chkTblEstado').bootstrapToggle();
                },
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'periodoDesc', title: 'Descripcion' },
                    {
                        title: 'Fecha de Inicio',
                        render: function (data, type, row) {
                            return moment(row.fechaInicio).format("DD/MM/YYYY");
                        }
                    },
                    {
                        title: 'Fecha de Fin',
                        render: function (data, type, row) {
                            return moment(row.fechaFinal).format("DD/MM/YYYY");
                        }
                    },
                    {
                        render: function (data, type, row) {
                            return `<input id="chkTblEstado" class="chkTblEstado" data-size="mini" type="checkbox" data-toggle="toggle" data-on="Disponible" 
                            data-off="NO Disponible" data-onstyle="success" data-offstyle="danger" data-width="150" ${row.estado ? "checked" : ""} disabled>`;
                        }
                    },
                    {

                        render: function (data, type, row) {
                            let btnActualizar = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Editar Periodo"><button class="btn btn-warning actualizarPeriodo btn-xs"><i class="far fa-edit"></i></button>&nbsp;</span>`;
                            let btnEliminar = `<span class="d-inline-block" tabindex="0" data-toggle="tooltip" title="Eliminar Periodo"><button class="btn btn-danger eliminarPeriodo btn-xs"><i class="far fa-trash-alt"></i></button></span>`;
                            let btns = btnActualizar + btnEliminar;
                            return btns;
                        }
                    },
                    { data: 'id', title: 'id', visible: false },
                ],
                initComplete: function (settings, json) {
                    tblRH_Vacaciones_Periodos.on('click', '.actualizarPeriodo', function () {
                        let rowData = dttblRH_Vacaciones_Periodos.row($(this).closest('tr')).data();
                        mdlCEPeriodo.modal("show");
                        btnCEPeriodoActualizar.attr('data-id', rowData.id);
                        btnTxtCEPeriodo.text("Actualizar");
                        titleCEPeriodo.text("ACTUALIZAR PERIODO");
                        txtCEPeriodoDesc.val(rowData.periodoDesc);
                        chkCEPeriodoEstado.prop("checked", rowData.estado);
                        chkCEPeriodoEstado.change();
                        txtCEPeriodoFechaini.val(moment(rowData.fechaInicio).format("YYYY-MM-DD"));
                        txtCEPeriodoFechafin.val(moment(rowData.fechaFinal).format("YYYY-MM-DD"));
                    });
                    tblRH_Vacaciones_Periodos.on('click', '.eliminarPeriodo', function () {
                        let rowData = dttblRH_Vacaciones_Periodos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarPeriodo(rowData.id));
                    });
                    tblRH_Vacaciones_Periodos.on('change', '.chkTblEstado', function () {
                        let rowData = dttblRH_Vacaciones_Periodos.row($(this).closest('tr')).data();
                        tblRH_Vacaciones_Periodos.find('input.chkTblEstado').bootstrapToggle(`${rowData.estado ? "on" : "off"}`);
                    });

                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        //#endregion

        //#region BACK END

        function fncGetPeriodos() {
            axios.post("GetPeriodos",).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    console.log(items);
                    dttblRH_Vacaciones_Periodos.clear();
                    dttblRH_Vacaciones_Periodos.rows.add(items);
                    dttblRH_Vacaciones_Periodos.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearEditarPeriodo() {
            let objPeriodo = fncCrearEditarPeriodoParams();
            if (objPeriodo != "") {
                axios.post("CrearEditarPeriodo", objPeriodo).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        if (objPeriodo.id > 0) {
                            Alert2Exito("Periodo Actualizado.");
                        } else {
                            Alert2Exito("Periodo Creado.");
                        }

                        fncGetPeriodos();

                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncEliminarPeriodo(idReg) {
            axios.post("EliminarPeriodo", { id: idReg }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Registro Elminado.");
                    fncGetPeriodos();
                }
            }).catch(error => Alert2Error(error.message));
        }

        //#endregion

        //#region FNC GRALES

        function fncCrearEditarPeriodoParams() {
            fncDefaultBorder();
            let strMensajeError = "";

            if (txtCEPeriodoDesc.val() == "") { txtCEPeriodoDesc.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCEPeriodoFechaini.val() == "") { txtCEPeriodoFechaini.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCEPeriodoFechafin.val() == "") { txtCEPeriodoFechafin.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (moment(txtCEPeriodoFechafin.val())._d < moment(txtCEPeriodoFechaini.val())) { strMensajeError = "Fecha invalida"; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let objPeriodo = new Object();

                objPeriodo = {
                    id: btnCEPeriodoActualizar.attr("data-id"),
                    estado: chkCEPeriodoEstado.prop("checked"),
                    periodoDesc: txtCEPeriodoDesc.val(),
                    fechaInicio: txtCEPeriodoFechaini.val(),
                    fechaFinal: txtCEPeriodoFechafin.val()
                }
                return objPeriodo;
            }

        }

        function fncDefaultBorder() {

            txtCEPeriodoDesc.css('border', '1px solid #CCC');
            txtCEPeriodoFechaini.css('border', '1px solid #CCC');
            txtCEPeriodoFechafin.css('border', '1px solid #CCC');

        }

        function fncEmptyFields() {
            txtCEPeriodoDesc.val("");
            txtCEPeriodoFechafin.val("");
            txtCEPeriodoFechaini.val("");
        }

        //#endregion
    }

    $(document).ready(() => {
        CH.Periodos = new Periodos();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();