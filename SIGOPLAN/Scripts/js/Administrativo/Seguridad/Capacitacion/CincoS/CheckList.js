(() => {
    $.namespace('Administrativo.CincoS');
    CincoS = function () {
        //#region ELEMENTOS
        const cboFiltroCC = $('#cboFiltroCC');
        const btnFiltroNuevo = $('#btnFiltroNuevo');
        const btnFiltroBuscar = $('#btnFiltroBuscar');

        const tblCheckList = $('#tblCheckList');

        let dtCheckList;
        //#endregion

        //#region CONSTS MDL CE CHECKLIST

        const mdlCEChecklist = $('#mdlCEChecklist');
        const titleCEChecklist = $('#titleCEChecklist');
        const txtCECheckistNombre = $('#txtCECheckistNombre');
        const cboCEChecklistCC = $('#cboCEChecklistCC');
        const cboCEChecklistArea = $('#cboCEChecklistArea');
        const cboCEChecklistLideres = $('#cboCEChecklistLideres');
        const txtCEChecklistInspecciones = $('#txtCEChecklistInspecciones');
        const chkCECEChecklistClasificar = $('#chkCECEChecklistClasificar');
        const chkCECEChecklistLimpieza = $('#chkCECEChecklistLimpieza');
        const chkCECEChecklistOrden = $('#chkCECEChecklistOrden');
        const chkCECEChecklistEstandarizar = $('#chkCECEChecklistEstandarizar');
        const chkCECEChecklistDisciplina = $('#chkCECEChecklistDisciplina');
        const cboCEChecklistSubArea = $('#cboCEChecklistSubArea');
        const btnCEInspeccion = $('#btnCEInspeccion');
        const tblCEChecklistInspecciones = $('#tblCEChecklistInspecciones');
        const btnCEChecklistGuardar = $('#btnCEChecklistGuardar');
        const txtCEChecklistClasificar = $('#txtCEChecklistClasificar');
        const txtCEChecklistOrden = $('#txtCEChecklistOrden');
        const txtCEChecklistLimpieza = $('#txtCEChecklistLimpieza');
        const txtCEChecklistEstandarizacion = $('#txtCEChecklistEstandarizacion');
        const txtCEChecklistDisciplina = $('#txtCEChecklistDisciplina');
        const txtCEChecklistTotal = $('#txtCEChecklistTotal');

        let dtCEChecklistInspecciones;
        //#endregion

        //#region CONSTS MDL CALENDARIO
        const mdlCalendario = $('#mdlCalendario');
        const txtCalendarioChecklist = $('#txtCalendarioChecklist');
        const cboCalendarioCC = $('#cboCalendarioCC');
        const cboCalendarioFechas = $('#cboCalendarioFechas');
        const btnCalendarioGuardar = $('#btnCalendarioGuardar');
        const fechaCalendario = $('#fechaCalendario');
        const btnCalendarioAgregarFecha = $('#btnCalendarioAgregarFecha');
        //#endregion

        //#region ENUM
        const consultaCCEnum = {
            Todos: 0,
            TodosLosActivos: 1,
            TodosConCheckListCreado: 2,
            LosDelCheckList: 3
        };

        let cincoSEnum = [];
        cincoSEnum[1] = "Clasificar";
        cincoSEnum[2] = "Limpieza";
        cincoSEnum[3] = "Orden";
        cincoSEnum[4] = "Estandarizar";
        cincoSEnum[5] = "Disciplina";

        //#endregion

        //#region EVENTOS
        btnFiltroBuscar.on('click', function () {
            if (cboFiltroCC.val() != "") {
                getCheckLists();
            }
        });

        btnFiltroNuevo.on("click", function () {
            mdlCEChecklist.modal("show");
            fncDefaultModalCECheckList();

            titleCEChecklist.text("CREAR");
            btnCEChecklistGuardar.data("id", 0);
            btnCEChecklistGuardar.html("<i class='fa fa-save'></i> CREAR");
        });

        //#region MDL
        btnCEInspeccion.on("click", function () {
            fncAddInsp();
        });

        btnCEChecklistGuardar.on("click", function () {
            fncCEChecklist();
        });

        btnCalendarioAgregarFecha.on("click", function () {
            if (fechaCalendario.val() != "") {
                if ($(`#cboCalendarioFechas option[value='${moment(fechaCalendario.val()).format("YYYY-MM-DD")}']`).length > 0) {

                } else {
                    var newOption = new Option(moment(fechaCalendario.val()).format("DD/MM/YYYY"), moment(fechaCalendario.val()).format("YYYY-MM-DD"), false, true);
                    $('#cboCalendarioFechas').append(newOption).trigger('change');
                }
            }
        });

        btnCalendarioGuardar.on("click", function () {
            fncGuardarCalendarioCheckList();
        });

        mdlCEChecklist.on('shown.bs.modal', function () {
            tblCEChecklistInspecciones.DataTable().columns.adjust().draw();
        });
        //#endregion

        //#endregion

        (function init() {
            initTblCheckList();
            initTblCEChecklistInspecciones();
            initCbox();
        })();

        //#region CBOX
        function initCbox() {
            cboFiltroCC.fillCombo('GetCCs', { consulta: consultaCCEnum.TodosConCheckListCreado, checkListId: null }, false, null, () => {
                cboFiltroCC.attr('multiple', 'multiple');
                cboFiltroCC.select2();
                cboFiltroCC.find('option:selected').remove();
            });

            cboCEChecklistCC.fillCombo('GetCCs', { consulta: consultaCCEnum.Todos, checkListId: null }, false, null, () => {
                cboCEChecklistCC.attr('multiple', 'multiple');
                cboCEChecklistCC.select2();
                cboCEChecklistCC.find('option:selected').remove();
            });

            cboCEChecklistLideres.fillCombo('GetLideres', null, false, null, () => {
                cboCEChecklistLideres.attr('multiple', 'multiple');
                cboCEChecklistLideres.select2();
                cboCEChecklistLideres.find('option:selected').remove();
            });

            // cboCalendarioCC.fillCombo('GetCCs', { consulta: consultaCCEnum.Todos, checkListId: null }, false, null, () => {
            //     cboCalendarioCC.find('option:eq(0)').remove();
            cboCalendarioCC.attr('multiple', 'multiple');
            cboCalendarioCC.select2();
            // });

            cboCEChecklistArea.fillCombo('GetAreas', null, false, null);
            cboCEChecklistSubArea.fillCombo('GetSubAreas', null, false, null);

            cboCalendarioFechas.attr('multiple', 'multiple');
            cboCalendarioFechas.select2();
        }
        //#endregion

        //#region TABLAS
        function initTblCheckList() {
            dtCheckList = tblCheckList.DataTable({
                order: [[0, 'asc']],
                ordering: false,
                searching: true,
                info: true,
                language: dtDicEsp,
                paging: true,
                scrollX: false,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],
                columns: [
                    {
                        title: 'CCs',
                        className: 'dt-center',
                        render: function (data, type, row) {

                            let cell = "";
                            if (row.ccSinCalendario) {
                                for (const item of row.ccSinCalendario) {
                                    cell += `<span class="label label-default" >${item}</span>&nbsp;`;
                                }
                            }
                            if (row.ccConCalendario) {
                                for (const item of row.ccConCalendario) {
                                    cell += `<span class="label label-primary">${item}</span>&nbsp;`;
                                }
                            }
                            return cell;
                        }
                    },
                    {
                        title: 'NOMBRE AUDITORIA',
                        className: 'dt-center',
                        data: 'nombreAuditoria'
                    },
                    {
                        title: 'ÁREA',
                        className: 'dt-center',
                        data: 'area'
                    },
                    {
                        title: 'LIDERES',
                        data: 'lideres',
                        className: 'dt-center',
                        render: function (data, type, row) {
                            if (data) {
                                let cell = "";
                                for (const item of data) {
                                    cell += `<span class="label label-default" >${item}</span>&nbsp;`;
                                }
                                return cell;
                            } else {
                                return "";
                            }
                        }
                    },
                    {
                        title: 'EDITAR',
                        className: 'dt-center',
                        render: function (data, type, row) {
                            let btnTblCalendario = `<button class="btn btn-primary btnCalendario" title="Calendario"><i class="far fa-calendar-alt"></i></button>`;
                            let btnTblEditar = `<button class="btn btn-warning btnEditar" title="Editar"><i class="fas fa-edit"></i></button>`;
                            let btnTblEliminar = `<button class="btn btn-danger btnEliminar" title="Eliminar"><i class="fas fa-trash-alt"></i></button>`;

                            return btnTblCalendario + ' ' + btnTblEditar + ' ' + btnTblEliminar;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblCheckList.on('click', '.btnCalendario', function () {
                        let rowData = dtCheckList.row($(this).closest('tr')).data();
                        let ccsCal = [];

                        cboCalendarioCC.val("");
                        cboCalendarioCC.trigger("change");
                        cboCalendarioCC.empty();

                        cboCalendarioFechas.val("");
                        cboCalendarioFechas.trigger("change");
                        cboCalendarioFechas.empty("");

                        fncGetCalendarioCheckList(rowData.checkListId);

                        // for (const item of rowData.ccSinCalendario) {
                        //     // ccsCal.push(item.split("-")[0].replaceAll(" ", ""));
                        //     var newOption = new Option(item, item.split("-")[0].replaceAll(" ", ""), false, true);
                        //     $('#cboCalendarioCC').append(newOption).trigger('change');
                        // }

                        // for (const item of rowData.ccConCalendario) {
                        //     // ccsCal.push(item.split("-")[0].replaceAll(" ", ""));
                        //     var newOption = new Option(item, item.split("-")[0].replaceAll(" ", ""), false, true);
                        //     $('#cboCalendarioCC').append(newOption).trigger('change');
                        // }

                        // cboCalendarioCC.val(ccsCal);
                        cboCalendarioCC.trigger("change");

                        btnCEChecklistGuardar.data("id", rowData.checkListId);
                        txtCalendarioChecklist.val(rowData.nombreAuditoria);
                        mdlCalendario.modal("show");
                    });
                    tblCheckList.on('click', '.btnEditar', function () {
                        let rowData = dtCheckList.row($(this).closest('tr')).data();
                        fncGetCheckListById(rowData.checkListId);

                        btnCEChecklistGuardar.data("id", rowData.checkListId);
                        mdlCEChecklist.modal("show");
                    });
                    tblCheckList.on('click', '.btnEliminar', function () {
                        let rowData = dtCheckList.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncRemoveChecklist(rowData.checkListId));
                    });
                },
                columnDefs: []
            });
        }

        function addRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function initTblCEChecklistInspecciones() {
            dtCEChecklistInspecciones = tblCEChecklistInspecciones.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                scrollX: false,
                scrollY: '45vh',
                scrollCollapse: true,
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'inspeccion', title: 'Inspección' },
                    { data: 'areaDesc', title: 'Área' },
                    { data: 'subAreaDesc', title: 'Sub - área' },
                    {
                        data: 'cincoS', title: '5\'S Auditada',
                        render: function (data, type, row) {
                            if (type === 'display') {
                                if (data) {
                                    let cell = "";
                                    for (const item of data) {
                                        cell += `<span class="label label-warning">${cincoSEnum[item]}</span>&nbsp;`;
                                    }

                                    return cell;
                                }
                                else {
                                    return "";
                                }
                            }
                            else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'Quitar', title: 'Quitar',
                        render: function (data, type, row) {
                            return `<button title='Eliminar inspeccion.' class="btn btn-danger btn-xs deleteInspeccion"><i class="fas fa-times"></i></button>&nbsp;`;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblCEChecklistInspecciones.on('click', '.classBtn', function () {
                        let rowData = dtCEChecklistInspecciones.row($(this).closest('tr')).data();
                    });
                    tblCEChecklistInspecciones.on('click', '.deleteInspeccion', function () {
                        let row = dtCEChecklistInspecciones.row($(this).closest('tr'));
                        let rowData = row.data();

                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => {

                            for (const item of rowData.cincoS) {
                                switch (item) {
                                    case 1:

                                        txtCEChecklistClasificar.text(Number(txtCEChecklistClasificar.text()) - 1);
                                        break;
                                    case 2:

                                        txtCEChecklistOrden.text(Number(txtCEChecklistOrden.text()) - 1);
                                        break;
                                    case 3:

                                        txtCEChecklistLimpieza.text(Number(txtCEChecklistLimpieza.text()) - 1);
                                        break;
                                    case 4:

                                        txtCEChecklistEstandarizacion.text(Number(txtCEChecklistEstandarizacion.text()) - 1);
                                        break;
                                    case 5:

                                        txtCEChecklistDisciplina.text(Number(txtCEChecklistDisciplina.text()) - 1);
                                        break;

                                    default:
                                        break;
                                }
                            }

                            txtCEChecklistTotal.text(Number(txtCEChecklistTotal.text()) - rowData.cincoS.length);
                            row.remove().draw();
                        });
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }
        //#endregion

        //#region ENDPOINT
        function getCheckLists() {
            $.post('GetCheckLists',
                {
                    ccs: cboFiltroCC.val()
                }).then(response => {
                    if (response.success) {
                        addRows(tblCheckList, response.items);
                    } else {
                        swal("¡Alerta!", response.message, "warning");
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function fncCEChecklist() {
            let obj = fncGetObjChecklist();

            if (btnCEChecklistGuardar.data("id") == 0) {
                if (obj != "" && dtCEChecklistInspecciones.data().count() > 0) {
                    axios.post("GuardarCheckList", obj).then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            //CODE...
                            Alert2Exito("Checklist guardado");
                            mdlCEChecklist.modal("hide");

                            let seleccionados = cboFiltroCC.val();
                            cboFiltroCC.fillCombo('GetCCs', { consulta: consultaCCEnum.TodosConCheckListCreado, checkListId: null }, false, null, () => {
                                cboFiltroCC.find('option:eq(0)').remove();
                                cboFiltroCC.attr('multiple', 'multiple');
                                cboFiltroCC.select2();
                                cboFiltroCC.val(seleccionados);
                                cboFiltroCC.trigger('change');
                                getCheckLists();
                            });
                        }
                    }).catch(error => Alert2Error(error.message));
                }
            } else {

                obj.id = btnCEChecklistGuardar.data("id");

                if (obj != "" && dtCEChecklistInspecciones.data().count() > 0) {
                    axios.post("EditarCheckList", obj).then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            //CODE...
                            Alert2Exito("Checklist actualizado");
                            mdlCEChecklist.modal("hide");

                            let seleccionados = cboFiltroCC.val();
                            cboFiltroCC.fillCombo('GetCCs', { consulta: consultaCCEnum.TodosConCheckListCreado, checkListId: null }, false, null, () => {
                                cboFiltroCC.find('option:eq(0)').remove();
                                cboFiltroCC.attr('multiple', 'multiple');
                                cboFiltroCC.select2();
                                cboFiltroCC.val(seleccionados);
                                cboFiltroCC.trigger('change');
                                getCheckLists();
                            });
                        }
                    }).catch(error => Alert2Error(error.message));
                }
            }
        }

        function fncGetCheckListById(checkListId) {
            axios.post("GetCheckList", { checkListId }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...

                    fncDefaultModalCECheckList();

                    txtCECheckistNombre.val(items.nombre);
                    cboCEChecklistCC.val(items.ccs);
                    cboCEChecklistCC.trigger("change");
                    cboCEChecklistArea.val(items.area);
                    cboCEChecklistArea.trigger("change");
                    cboCEChecklistLideres.val(items.lideres);
                    cboCEChecklistLideres.trigger("change");
                    txtCEChecklistClasificar.text(items.cincoS_clasificar);
                    txtCEChecklistOrden.text(items.cincoS_orden);
                    txtCEChecklistLimpieza.text(items.cincoS_limpieza);
                    txtCEChecklistEstandarizacion.text(items.cincoS_estandarizacion);
                    txtCEChecklistDisciplina.text(items.cincoS_disciplina);
                    txtCEChecklistTotal.text(items.cincoS_total);

                    dtCEChecklistInspecciones.clear();
                    dtCEChecklistInspecciones.rows.add(items.inspecciones);
                    dtCEChecklistInspecciones.draw();

                    titleCEChecklist.text("ACTUALIZAR");
                    btnCEChecklistGuardar.html("<i class='fa fa-save'></i> ACTUALIZAR");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncRemoveChecklist(checkListId) {
            axios.post("EliminarCheckList", { checkListId }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    Alert2Exito("Registo eliminado con exito");
                    getCheckLists();

                    cboFiltroCC.fillCombo('GetCCs', { consulta: consultaCCEnum.TodosConCheckListCreado, checkListId: null }, false, null, () => {
                        cboFiltroCC.find('option:eq(0)').remove();
                        cboFiltroCC.attr('multiple', 'multiple');
                        cboFiltroCC.select2();
                    });

                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGuardarCalendarioCheckList(checkListId) {

            let lstCCsComboDTO = [];
            let lstFechasComboDTO = [];
            let lstCCs = getValoresMultiples("#cboCalendarioCC");
            let lstFechas = getValoresMultiples("#cboCalendarioFechas");

            for (const item of lstCCs) {
                let obj = {
                    Text: item,
                    Value: item,
                    Prefijo: "seleccionado",
                }

                lstCCsComboDTO.push(obj);
            }

            for (const item of lstFechas) {
                let obj = {
                    Text: item,
                    Value: item,
                    Prefijo: "seleccionado",
                }
                lstFechasComboDTO.push(obj);
            }

            let obj = {
                nombre: txtCalendarioChecklist.val(),
                ccs: lstCCsComboDTO,
                fechas: lstFechasComboDTO,
                checkListId: btnCEChecklistGuardar.data("id"),
            }

            axios.post("GuardarCalendarioCheckList", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    mdlCalendario.modal("hide");
                    Alert2Exito("Calendario guardado con exito");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetCalendarioCheckList(checkListId) {
            axios.post("GetCalendarioCheckList", { checkListId }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    for (const item of items.fechas) {
                        let fecha = item.Text.split("/");
                        var newOption = new Option(item.Text, `${fecha[2]}-${fecha[1]}-${fecha[0]}`, false, true);
                        $('#cboCalendarioFechas').append(newOption).trigger('change');
                    }

                    for (const item of items.ccs) {
                        if (item.Prefijo == "seleccionado") {
                            var newOption = new Option(item.Text, item.Value, false, true);
                            cboCalendarioCC.append(newOption);
                        } else {
                            var newOption = new Option(item.Text, item.Value, false, false);
                            cboCalendarioCC.append(newOption);
                        }
                    }
                    cboCEChecklistArea.trigger('change');
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region GRALES
        function fncGetObjInsp() {
            let rowData = tblCEChecklistInspecciones.DataTable().rows().data().toArray();

            let abonos = new Array();

            for (let index = 0; index < rowData.length; index++) {
                abonos.push(rowData[index]);
            }

            return abonos;
        }

        function fncGetObjChecklist() {
            let strMensajeError = "";

            if (txtCECheckistNombre.val() == "") { txtCECheckistNombre.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (getValoresMultiples("#cboCEChecklistCC").length == 0) { $("#select2-cboCEChecklistCC-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (cboCEChecklistArea.val() == "") { cboCEChecklistArea.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (getValoresMultiples("#cboCEChecklistLideres").length == 0) { $("#select2-cboCEChecklistLideres-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let lstInsp = fncGetObjInsp();

                let obj = {
                    nombre: txtCECheckistNombre.val(),
                    ccs: getValoresMultiples("#cboCEChecklistCC"),
                    areaId: cboCEChecklistArea.val(),
                    lideresId: getValoresMultiples("#cboCEChecklistLideres"),
                    inspecciones: lstInsp,
                }

                return obj;
            }
        }

        function fncAddInsp() {
            let strMensajeError = "";

            if (txtCEChecklistInspecciones.val() == "") { txtCEChecklistInspecciones.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (cboCEChecklistSubArea.val() == "") { cboCEChecklistSubArea.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios"; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let arrCincoS = [];

                let esCheckboxSelect = false;

                if (chkCECEChecklistClasificar.prop("checked")) {
                    txtCEChecklistClasificar.text(Number(txtCEChecklistClasificar.text()) + 1);
                    arrCincoS.push(1);
                    esCheckboxSelect = true;
                }

                if (chkCECEChecklistLimpieza.prop("checked")) {
                    txtCEChecklistOrden.text(Number(txtCEChecklistOrden.text()) + 1);
                    arrCincoS.push(2);

                    esCheckboxSelect = true;
                }

                if (chkCECEChecklistOrden.prop("checked")) {
                    txtCEChecklistLimpieza.text(Number(txtCEChecklistLimpieza.text()) + 1);
                    arrCincoS.push(3);
                    esCheckboxSelect = true;

                }

                if (chkCECEChecklistEstandarizar.prop("checked")) {
                    txtCEChecklistEstandarizacion.text(Number(txtCEChecklistEstandarizacion.text()) + 1);
                    arrCincoS.push(4);
                    esCheckboxSelect = true;

                }

                if (chkCECEChecklistDisciplina.prop("checked")) {
                    txtCEChecklistDisciplina.text(Number(txtCEChecklistDisciplina.text()) + 1);
                    arrCincoS.push(5);
                    esCheckboxSelect = true;

                }

                txtCEChecklistTotal.text(Number(txtCEChecklistTotal.text()) + arrCincoS.length);

                let obj = {
                    inspeccion: txtCEChecklistInspecciones.val(),
                    area: cboCEChecklistArea.val(),
                    areaDesc: $("#cboCEChecklistArea option:selected").text(),
                    subAreaId: cboCEChecklistSubArea.val(),
                    subAreaDesc: $("#cboCEChecklistSubArea option:selected").text(),
                    cincoS: arrCincoS,
                    // esNuevo: true,
                }

                if (esCheckboxSelect) {
                    dtCEChecklistInspecciones.row.add(obj);
                    dtCEChecklistInspecciones.draw();

                    txtCEChecklistInspecciones.val('');
                } else {
                    Alert2Warning("Seleccionar almenos un de las 5 opciones de 5's");

                }

            }
        }

        function fncDefaultModalCECheckList() {
            //BORDES
            txtCECheckistNombre.css('border', '1px solid #CCC');
            cboCEChecklistArea.css('border', '1px solid #CCC');
            cboCEChecklistSubArea.css('border', '1px solid #CCC');
            txtCEChecklistInspecciones.css('border', '1px solid #CCC');

            //INFO
            txtCECheckistNombre.val("");
            cboCEChecklistCC.val("");
            cboCEChecklistCC.trigger("change");
            cboCEChecklistArea.val("");
            cboCEChecklistArea.trigger("change");
            cboCEChecklistLideres.val("");
            cboCEChecklistLideres.trigger("change");
            txtCEChecklistInspecciones.val("");

            txtCEChecklistClasificar.text("0");
            txtCEChecklistOrden.text("0");
            txtCEChecklistLimpieza.text("0");
            txtCEChecklistEstandarizacion.text("0");
            txtCEChecklistDisciplina.text("0");
            txtCEChecklistTotal.text("0");

            chkCECEChecklistClasificar.prop("checked", false);
            chkCECEChecklistLimpieza.prop("checked", false);
            chkCECEChecklistOrden.prop("checked", false);
            chkCECEChecklistEstandarizar.prop("checked", false);
            chkCECEChecklistDisciplina.prop("checked", false);

            cboCEChecklistSubArea.val("");
            cboCEChecklistSubArea.trigger("change");

            dtCEChecklistInspecciones.clear().draw();
        }
        //#endregion
    }

    $(document).ready(() => Administrativo.CincoS = new CincoS())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();
