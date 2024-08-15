(() => {
    $.namespace('CH.CatNotificantes');

    //#region CONSTS FILTROS
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnFiltroNuevo = $('#btnFiltroNuevo');
    //#endregion

    //#region CONSTS TBL
    const tblNotificantes = $('#tblNotificantes');
    let dtNotificantes;
    //#endregion

    //#region CONSTS MDL CE NOTIFICANTE
    const mdlCENotificante = $('#mdlCENotificante');
    const btnCENotificante = $('#btnCENotificante');
    const btnCEAddNotificante = $('#btnCEAddNotificante');
    const cboCEUsuario = $('#cboCEUsuario');
    const cboCECC = $('#cboCECC');
    const cboCEConcepto = $('#cboCEConcepto');

    const tblNotificantesDet = $('#tblNotificantesDet');
    let dtNotificantesDet;
    //#endregion

    RelPuestoFases = function () {
        (function init() {
            initTblNotificantes();
            initTblNotificantesDet();

            fncListeners();

        })();

        function fncListeners() {

            //#region FILLCOMBO
            cboCEUsuario.fillCombo("FillCboUsuarios", {}, false);
            cboCECC.fillCombo("FillCboCC", {}, false);
            cboCEConcepto.fillCombo("FillCboConceptos", {}, false);
            $(".select2").select2({ width: "100%" });
            //#endregion

            btnFiltroBuscar.on("click", function () {
                fncGetNotificantes();
            });

            btnFiltroNuevo.on("click", function () {
                cboCECC.val("");
                cboCECC.trigger("change");

                cboCEConcepto.val("");
                cboCEConcepto.trigger("change");

                cboCEUsuario.val("");
                cboCEUsuario.trigger("change");

                mdlCENotificante.modal("show");
                dtNotificantesDet.clear().draw();
            });

            cboCECC.on('change', function (event, noUpdateTblNotisDet) {
                if (cboCEConcepto.val() != "" && $(this).val() != "") {
                    fncGetNotificantesDet($(this).val(), cboCEConcepto.val());
                } else {
                    dtNotificantesDet.clear().draw();
                }
            });

            cboCEConcepto.on('change', function (event, noUpdateTblNotisDet) {
                if (cboCECC.val() != "" && $(this).val() != "") {
                    fncGetNotificantesDet(cboCECC.val(), $(this).val());
                } else {
                    dtNotificantesDet.clear().draw();
                }
            });

            btnCEAddNotificante.on("click", function () {

                if (cboCECC.val() != "" && cboCEConcepto.val() != "" && cboCEUsuario.val() != "") {
                    let objRow = {
                        usrNomCompleto: $("#cboCEUsuario option:selected").text(),
                        idUsuario: cboCEUsuario.val(),
                        esNuevoNoti: true
                    }

                    let lstNotis = new Array();
                    lstNotis.push(objRow);

                    dtNotificantesDet.rows.add(lstNotis);
                    dtNotificantesDet.draw();

                    cboCEUsuario.val("");
                    cboCEUsuario.trigger("change");
                }
            });

            btnCENotificante.on("click", function () {
                fncCrearEditarNotificantes();
            });
        }
    }

    //#region TBL
    function initTblNotificantes() {
        dtNotificantes = tblNotificantes.DataTable({
            language: dtDicEsp,
            destroy: false,
            paging: false,
            ordering: false,
            searching: true,
            bFilter: false,
            info: false,
            scrollX: true,
            scrollCollapse: true,
            scrollY: '500px',
            columns: [
                //render: function (data, type, row) { }
                { data: 'ccDesc', title: 'CC' },
                {
                    data: 'nombresTaller', title: 'LIB. TALLER',
                    render: function (data, type, row) {
                        let htmlNombres = "";

                        for (const item of data) {
                            htmlNombres += `<span class="labelNombre label label-primary" style="display: block; margin-bottom: 1px;">${item}</span>`;
                        }

                        return htmlNombres;
                    }
                },
                {
                    data: 'nombresAlmacen', title: 'LIB. ALMACEN',
                    render: function (data, type, row) {
                        let htmlNombres = "";

                        for (const item of data) {
                            htmlNombres += `<span class="labelNombre label label-primary" style="display: block; margin-bottom: 1px;">${item}</span>`;
                        }

                        return htmlNombres;
                    }
                },
                {
                    data: 'nombresConta', title: 'LIB. CONTABILIDAD',
                    render: function (data, type, row) {
                        let htmlNombres = "";

                        for (const item of data) {
                            htmlNombres += `<span class="labelNombre label label-primary" style="display: block; margin-bottom: 1px;">${item}</span>`;
                        }

                        return htmlNombres;
                    }
                },
                {
                    data: 'nombresNominas', title: 'LIB. NOMINAS',
                    render: function (data, type, row) {
                        let htmlNombres = "";

                        for (const item of data) {
                            htmlNombres += `<span class="labelNombre label label-primary" style="display: block; margin-bottom: 1px;">${item}</span>`;
                        }

                        return htmlNombres;
                    }
                },
                {
                    data: 'nombresResponsableCC', title: 'RESPONSABLE CC',
                    render: function (data, type, row) {
                        let htmlNombres = "";

                        for (const item of data) {
                            htmlNombres += `<span class="labelNombre label label-primary" style="display: block; margin-bottom: 1px;">${item}</span>`;
                        }

                        return htmlNombres;
                    }
                },
                {
                    data: 'nombresAltas', title: 'ALTAS',
                    render: function (data, type, row) {
                        let htmlNombres = "";

                        for (const item of data) {
                            htmlNombres += `<span class="labelNombre label label-primary" style="display: block; margin-bottom: 1px;">${item}</span>`;
                        }

                        return htmlNombres;
                    }
                },
                {
                    data: 'nombresBajas', title: 'BAJAS',
                    render: function (data, type, row) {
                        let htmlNombres = "";

                        for (const item of data) {
                            htmlNombres += `<span class="labelNombre label label-primary" style="display: block; margin-bottom: 1px;">${item}</span>`;
                        }

                        return htmlNombres;
                    }
                },
                {
                    data: 'nombresCH', title: 'CH',
                    render: function (data, type, row) {
                        let htmlNombres = "";

                        for (const item of data) {
                            htmlNombres += `<span class="labelNombre label label-primary" style="display: block; margin-bottom: 1px;">${item}</span>`;
                        }

                        return htmlNombres;
                    }
                },
                {
                    data: 'nombresIncapacidades', title: 'INCAPACIDADES',
                    render: function (data, type, row) {
                        let htmlNombres = "";

                        for (const item of data) {
                            htmlNombres += `<span class="labelNombre label label-primary" style="display: block; margin-bottom: 1px;">${item}</span>`;
                        }

                        return htmlNombres;
                    }
                },
            ],
            initComplete: function (settings, json) {
                tblNotificantes.on('click', '.labelNombre', function () {
                    let rowData = dtNotificantes.row($(this).closest('tr')).data();
                    let column = dtNotificantes.column($(this).closest('tr td'));
                    let columnIdx = column.index();

                    btnCENotificante.data("cc", rowData.cc);
                    btnCENotificante.data("idConcepto", columnIdx);

                    cboCECC.val(rowData.cc);
                    cboCECC.trigger("change");

                    cboCEConcepto.val(columnIdx);
                    cboCEConcepto.trigger("change");

                    cboCEUsuario.val("");
                    cboCEUsuario.trigger("change");

                    mdlCENotificante.modal("show");
                });
                tblNotificantes.on('click', '.classBtn', function () {
                    let rowData = dtNotificantes.row($(this).closest('tr')).data();
                    //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                });
            },
            columnDefs: [
                { className: 'dt-center', 'targets': '_all' }
            ],
        });
    }

    function initTblNotificantesDet() {
        dtNotificantesDet = tblNotificantesDet.DataTable({
            language: dtDicEsp,
            destroy: false,
            paging: false,
            ordering: false,
            searching: false,
            bFilter: false,
            info: false,
            columns: [
                //render: function (data, type, row) { }
                { data: 'usrNomCompleto', title: 'NOMBRE' },
                { render: (data, type, row, meta) => { return `<button title="Eliminar notificante" class="btn btn-sm btn-danger eliminarNoti btn-xs"><i class="far fa-trash-alt"></i></button>` } },
            ],
            initComplete: function (settings, json) {
                tblNotificantesDet.on('click', '.classBtn', function () {
                    let rowData = dtNotificantesDet.row($(this).closest('tr')).data();
                });
                tblNotificantesDet.on('click', '.eliminarNoti', function () {
                    let rowData = dtNotificantesDet.row($(this).closest('tr')).data();
                    let row = dtNotificantesDet.row($(this).closest('tr')).index();

                    if (rowData.esNuevoNoti != undefined && rowData.esNuevoNoti != null && rowData.esNuevoNoti) {
                        dtNotificantesDet.row(row).remove().draw();
                    } else {
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarNotificantes(rowData.idRelNoti));
                    }
                });
            },
            columnDefs: [
                { className: 'dt-center', 'targets': '_all' }
            ],
        });
    }
    //#endregion

    //#region BACKEND
    function fncGetNotificantes() {
        axios.post("GetNotificantes").then(response => {
            let { success, items, message } = response.data;
            if (success) {
                dtNotificantes.clear();
                dtNotificantes.rows.add(items);
                dtNotificantes.draw();
            }
        }).catch(error => Alert2Error(error.message));
    }

    function fncGetNotificantesDet(cc, idConcepto) {
        let obj = {
            cc,
            idConcepto,
        }

        axios.post("GetNotificantesDet", obj).then(response => {
            let { success, items, message } = response.data;
            if (success) {
                dtNotificantesDet.clear();
                dtNotificantesDet.rows.add(items);
                dtNotificantesDet.draw();
            }
        }).catch(error => Alert2Error(error.message));
    }

    function fncCrearEditarNotificantes() {
        let lstNotis = fncGetObjNotificantes();

        if (lstNotis.length > 0 && cboCECC.val() != "" && cboCEConcepto.val() != null) {

            let obj = {
                cc: cboCECC.val(),
                idConcepto: cboCEConcepto.val(),
                lstUsuariosNuevos: lstNotis
            }

            axios.post("CrearEditarNotificantes", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Registro agregado con exito");
                    fncGetNotificantesDet(cboCECC.val(), cboCEConcepto.val());
                    fncGetNotificantes();

                }
            }).catch(error => Alert2Error(error.message));
        }
    }

    function fncEliminarNotificantes(idRelNoti) {
        axios.post("RemoveNotificante", { idRelNoti }).then(response => {
            let { success, items, message } = response.data;
            if (success) {
                Alert2Exito("Registro eliminado con exito");
                fncGetNotificantesDet(cboCECC.val(), cboCEConcepto.val());
                fncGetNotificantes();

            }
        }).catch(error => Alert2Error(error.message));
    }

    //#endregion

    //#region GRALES
    function fncGetObjNotificantes() {
        let rowData = tblNotificantesDet.DataTable().rows().data().toArray();

        let lstIdsUsuarioNotis = new Array();

        for (let index = 0; index <= rowData.length; index++) {
            if (rowData[index - 1] != undefined) {
                if (rowData[index - 1].esNuevoNoti) {
                    // let objTabulador = {
                    //     tabulador: +txtNumeroTabulador.val(),
                    //     puesto: +txtCECompaniaPuestoDescripcion.data('puesto'),
                    //     salario_base: +rowData[index - 1].salario_base,
                    //     complemento: +rowData[index - 1].complemento,
                    //     bono_de_zona: +rowData[index - 1].bono_de_zona,
                    //     year: moment(rowData[index - 1].fechaRealNomina).format('YYYY'),
                    //     motivoCambio: +rowData[index - 1].motivoCambio,
                    //     fechaAplicaCambio: moment(rowData[index - 1].fechaAplicaCambio, 'DD/MM/YYYY')
                    // }

                    // return objTabulador;
                    lstIdsUsuarioNotis.push(rowData[index - 1].idUsuario);
                }
            }
        }

        return lstIdsUsuarioNotis;
    }
    //#endregion

    $(document).ready(() => {
        CH.RelPuestoFases = new RelPuestoFases();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();