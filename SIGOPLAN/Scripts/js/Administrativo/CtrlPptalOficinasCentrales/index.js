(() => {
    $.namespace('CtrlPptalOfCE.index');

    //#region CONST
    const cboFiltroCC = $('#cboFiltroCC');
    const cboFiltroAnio = $('#cboFiltroAnio');
    const btnFiltroReportePlanMaestro = $('#btnFiltroReportePlanMaestro');
    const tblPlanMaestro = $('#tblPlanMaestro');
    const mdlPlanMaestro = $('#mdlPlanMaestro');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const tblCapturas = $('#tblCapturas');
    const tblAditivas = $('#tblAditivas');
    const mdlAditivas = $('#mdlAditivas');
    const btnRegresar = $('#btnRegresar');
    const modalMesContable = $('#modalMesContable');
    const tablaMesContable = $('#tablaMesContable');
    const modalTotalContable = $('#modalTotalContable');
    const tablaTotalContable = $('#tablaTotalContable');
    let dtAditivas;
    let dtCapturas;
    let dtPlanMaestro;
    //#endregion

    //#region CONST DETALLE CAPTURA
    const mdlDetCaptura = $('#mdlDetCaptura');
    const tblDetCapturas = $('#tblDetCapturas');
    let dtDetCapturas;
    //#endregion

    //#region CONST DETALLE CAPTURA POR MES
    const mdlDetCapturaMes = $('#mdlDetCapturaMes');
    const tblDetCapturasMes = $('#tblDetCapturasMes');
    const lblTitlePorMes = $('#lblTitlePorMes');
    let dtDetCapturasMes;
    let dtMesContable;
    let dtTotalContable;
    //#endregion

    //#region CONST PLAN MAESTRO
    const cboCE_PM_Anio = $("#cboCE_PM_Anio");
    const cboCE_PM_CC = $("#cboCE_PM_CC");
    const txtCE_PM_MisionArea = $("#txtCE_PM_MisionArea");
    const txtCE_PM_ObjetivoEspecificoMedible = $("#txtCE_PM_ObjetivoEspecificoMedible");
    const txtCE_PM_Meta = $('#txtCE_PM_Meta');
    const tblMedicionesIndicadores = $('#tblMedicionesIndicadores');
    const tblAgrupacionesConceptos = $('#tblAgrupacionesConceptos');
    let dtMedicionesIndicadores;
    let dtAgrupacionesConceptos;
    //#endregion

    index = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT DATATABLES
            initTblCapturas();
            initTblDetCapturas();
            initTblDetCapturasMes();
            initTablaMesContable();
            initTablaTotalContable();
            initTblAditivas();
            initTblMedicionesIndicadores();
            initTblAgrupacionesConceptos();
            //#endregion

            //#region EVENTOS INDEX
            btnFiltroBuscar.on("click", function () {
                fncGetSumaCapturas();
            });

            btnFiltroReportePlanMaestro.on("click", function () {
                fncGetPlanMaestro();
            });

            cboFiltroAnio.on("change", function () {
                if ($(this).val() > 0) {
                    cboFiltroCC.fillCombo("FillUsuarioRelCCPptosAutorizados", { anio: $(this).val() }, false);
                }
            });

            cboCE_PM_Anio.on("change", function () {
                if ($(this).val() > 0) {
                    cboCE_PM_CC.fillCombo("FillUsuarioRelCCPptosAutorizados", { anio: $(this).val() }, false);
                }
            });
            //#endregion

            //#region FILL COMBOS
            cboFiltroAnio.fillCombo("FillAnios", {}, false);
            cboCE_PM_Anio.fillCombo("FillAnios", {}, false);

            $(".select2").select2();
            $(".select2").select2({ width: "100%" });
            //#endregion
        }

        //#region INDEX
        function initTblCapturas() {
            dtCapturas = tblCapturas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                scrollY: '45vh',
                scrollCollapse: true,
                columns: [
                    {
                        data: 'concepto', title: 'Concepto',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        }
                    },
                    {
                        data: 'importeEnero', title: 'Enero',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeEnero" mes="1">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    // {
                    //     data: 'importeContEnero', title: 'cont',
                    //     createdCell: function (td, tr, cellData, rowData, row, col) {
                    //         let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                    //         $(td).css('background-color', color);
                    //     },
                    //     render: function (data, type, row) {
                    //         return `<a class="importeContable importeContEnero" mes="1">${maskNumero2DCompras(data)}</a>`
                    //     }
                    // },
                    {
                        data: 'importeFebrero', title: 'Febrero',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeFebrero" mes="2">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    // {
                    //     data: 'importeContFebrero', title: 'cont',
                    //     createdCell: function (td, tr, cellData, rowData, row, col) {
                    //         let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                    //         $(td).css('background-color', color);
                    //     },
                    //     render: function (data, type, row) {
                    //         return `<a class="importeContable importeContFebrero" mes="2">${maskNumero2DCompras(data)}</a>`
                    //     }
                    // },
                    {
                        data: 'importeMarzo', title: 'Marzo',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeMarzo" mes="3">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    // {
                    //     data: 'importeContMarzo', title: 'cont',
                    //     createdCell: function (td, tr, cellData, rowData, row, col) {
                    //         let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                    //         $(td).css('background-color', color);
                    //     },
                    //     render: function (data, type, row) {
                    //         return `<a class="importeContable importeContMarzo" mes="3">${maskNumero2DCompras(data)}</a>`
                    //     }
                    // },
                    {
                        data: 'importeAbril', title: 'Abril',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeAbril" mes="4">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    // {
                    //     data: 'importeContAbril', title: 'cont',
                    //     createdCell: function (td, tr, cellData, rowData, row, col) {
                    //         let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                    //         $(td).css('background-color', color);
                    //     },
                    //     render: function (data, type, row) {
                    //         return `<a class="importeContable importeContAbril" mes="4">${maskNumero2DCompras(data)}</a>`
                    //     }
                    // },
                    {
                        data: 'importeMayo', title: 'Mayo',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeMayo" mes="5">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    // {
                    //     data: 'importeContMayo', title: 'cont',
                    //     createdCell: function (td, tr, cellData, rowData, row, col) {
                    //         let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                    //         $(td).css('background-color', color);
                    //     },
                    //     render: function (data, type, row) {
                    //         return `<a class="importeContable importeContMayo" mes="5">${maskNumero2DCompras(data)}</a>`
                    //     }
                    // },
                    {
                        data: 'importeJunio', title: 'Junio',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeJunio" mes="6">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    // {
                    //     data: 'importeContJunio', title: 'cont',
                    //     createdCell: function (td, tr, cellData, rowData, row, col) {
                    //         let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                    //         $(td).css('background-color', color);
                    //     },
                    //     render: function (data, type, row) {
                    //         return `<a class="importeContable importeContJunio" mes="6">${maskNumero2DCompras(data)}</a>`
                    //     }
                    // },
                    {
                        data: 'importeJulio', title: 'Julio',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeJulio" mes="7">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    // {
                    //     data: 'importeContJulio', title: 'cont',
                    //     createdCell: function (td, tr, cellData, rowData, row, col) {
                    //         let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                    //         $(td).css('background-color', color);
                    //     },
                    //     render: function (data, type, row) {
                    //         return `<a class="importeContable importeContJulio" mes="7">${maskNumero2DCompras(data)}</a>`
                    //     }
                    // },
                    {
                        data: 'importeAgosto', title: 'Agosto',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeAgosto" mes="8">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    // {
                    //     data: 'importeContAgosto', title: 'cont',
                    //     createdCell: function (td, tr, cellData, rowData, row, col) {
                    //         let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                    //         $(td).css('background-color', color);
                    //     },
                    //     render: function (data, type, row) {
                    //         return `<a class="importeContable importeContAgosto" mes="8">${maskNumero2DCompras(data)}</a>`
                    //     }
                    // },
                    {
                        data: 'importeSeptiembre', title: 'Septiembre',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeSeptiembre" mes="9">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    // {
                    //     data: 'importeContSeptiembre', title: 'cont',
                    //     createdCell: function (td, tr, cellData, rowData, row, col) {
                    //         let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                    //         $(td).css('background-color', color);
                    //     },
                    //     render: function (data, type, row) {
                    //         return `<a class="importeContable importeContSeptiembre" mes="9">${maskNumero2DCompras(data)}</a>`
                    //     }
                    // },
                    {
                        data: 'importeOctubre', title: 'Octubre',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeOctubre" mes="10">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    // {
                    //     data: 'importeContOctubre', title: 'cont',
                    //     createdCell: function (td, tr, cellData, rowData, row, col) {
                    //         let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                    //         $(td).css('background-color', color);
                    //     },
                    //     render: function (data, type, row) {
                    //         return `<a class="importeContable importeContOctubre" mes="10">${maskNumero2DCompras(data)}</a>`
                    //     }
                    // },
                    {
                        data: 'importeNoviembre', title: 'Noviembre',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeNoviembre" mes="11">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    // {
                    //     data: 'importeContNoviembre', title: 'cont',
                    //     createdCell: function (td, tr, cellData, rowData, row, col) {
                    //         let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                    //         $(td).css('background-color', color);
                    //     },
                    //     render: function (data, type, row) {
                    //         return `<a class="importeContable importeContNoviembre" mes="11">${maskNumero2DCompras(data)}</a>`
                    //     }
                    // },
                    {
                        data: 'importeDiciembre', title: 'Diciembre',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importePresupuesto importeDiciembre" mes="12">${maskNumero2DCompras(data)}</a>`
                        }
                    },
                    // {
                    //     data: 'importeContDiciembre', title: 'cont',
                    //     createdCell: function (td, tr, cellData, rowData, row, col) {
                    //         let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                    //         $(td).css('background-color', color);
                    //     },
                    //     render: function (data, type, row) {
                    //         return `<a class="importeContable importeContDiciembre" mes="12">${maskNumero2DCompras(data)}</a>`
                    //     }
                    // },
                    {
                        data: 'importeTotalConcepto', title: "Total",
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<a class="importeTotalConcepto">${maskNumero2DCompras(data)}</a>`;
                        }
                    },
                    // {
                    //     data: 'importeContTotalConcepto', title: "cont",
                    //     createdCell: function (td, tr, cellData, rowData, row, col) {
                    //         let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                    //         $(td).css('background-color', color);
                    //     },
                    //     render: function (data, type, row) {
                    //         return `<a class="importeContTotalConcepto">${maskNumero2DCompras(data)}</a>`;
                    //     }
                    // },
                    // {
                    //     render: function (data, type, row, meta) {
                    //         if (row.concepto != "TOTAL") {
                    //             return `<button class="btn btn-xs btn-primary detalle" title="Detalle del concepto."><i class="fas fa-list-ul"></i></button>&nbsp;`;
                    //         } else {
                    //             return "";
                    //         }
                    //     },
                    // },
                    { data: 'cc', visible: false },
                    { data: 'idAgrupacion', visible: false },
                    { data: 'idConcepto', visible: false },
                    { data: 'agrupacion', title: 'Agrupación', visible: false },
                    { data: 'esAgrupacion', visible: false },
                    { data: 'id', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblCapturas.on('click', '.detalle', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        fncGetDetCapturas(rowData.idAgrupacion, rowData.idConcepto);
                    });

                    tblCapturas.on('click', '.importePresupuesto', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        let mes = $(this).attr('mes');
                        btnRegresar.attr("mes", mes);

                        if (rowData.concepto != "TOTAL") {
                            if (rowData.esAgrupacion) {
                                fncGetDetCapturasMes(rowData.idAgrupacion, 0, mes);

                            } else {
                                fncGetDetCapturasMes(rowData.idAgrupacion, rowData.idConcepto, mes);
                            }
                        }
                    });

                    tblCapturas.on('click', '.importeContable', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        let mes = $(this).attr('mes');

                        if (rowData.concepto != "TOTAL") {
                            fncGetDetCapturasMesContable(rowData.idAgrupacion, rowData.idConcepto, mes);
                        }
                    });

                    tblCapturas.on('click', '.importeTotalConcepto', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        if (rowData.concepto != "TOTAL") {
                            fncGetDetCapturas(rowData.idAgrupacion, rowData.idConcepto);
                        }
                    });

                    tblCapturas.on('click', '.importeContTotalConcepto', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        if (rowData.concepto != "TOTAL") {
                            fncGetDetCapturasContable(rowData.idAgrupacion, rowData.idConcepto);
                        }
                    });
                },
                columnDefs: [
                    { targets: [0], className: 'dt-body-center' },
                    { targets: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13], className: 'dt-body-right' },
                    { width: "15%", targets: [0] },
                    { width: "6%", targets: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13] },
                ],
            });
        }

        function fncGetSumaCapturas() {
            if (cboFiltroCC.val() > -1 && cboFiltroAnio.val() > 0) {
                let obj = new Object();
                obj = {
                    idCC: +cboFiltroCC.val(),
                    anio: cboFiltroAnio.val()
                }
                axios.post("GetSumaCapturas", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtCapturas.clear();
                        dtCapturas.rows.add(response.data.lstSumaCapturas);
                        dtCapturas.draw();
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                let strMensajeError = "";
                cboFiltroAnio.val() <= 0 ? strMensajeError += "Es necesario indicar un año." : "";
                cboFiltroCC.val() <= 0 ? strMensajeError += "<br>Es necesario indicar un CC." : "";
                Alert2Warning(strMensajeError);
            }
        }

        function fncGetPlanMaestro() {
            if (cboFiltroCC.val() > -1 && cboFiltroAnio.val() > 0) {
                let obj = new Object();
                obj = {
                    idCC: +cboFiltroCC.val(),
                    anio: cboFiltroAnio.val()
                }
                axios.post("GetPlanMaestroRelReportePpto", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        cboCE_PM_Anio.val(response.data.objPlanMaestro.anio);
                        cboCE_PM_Anio.trigger("change");
                        cboCE_PM_CC.val(response.data.objPlanMaestro.idCC);
                        cboCE_PM_CC.trigger("change");
                        txtCE_PM_MisionArea.val(response.data.objPlanMaestro.misionArea);
                        txtCE_PM_ObjetivoEspecificoMedible.val(response.data.objPlanMaestro.objEspecificoMedible);
                        txtCE_PM_Meta.val(response.data.objPlanMaestro.meta);

                        dtMedicionesIndicadores.clear();
                        dtMedicionesIndicadores.rows.add(response.data.lstMedicionesIndicadores);
                        dtMedicionesIndicadores.draw();

                        dtAgrupacionesConceptos.clear();
                        dtAgrupacionesConceptos.rows.add(response.data.lstRNAgrupacionesRNConceptos);
                        dtAgrupacionesConceptos.draw();

                        mdlPlanMaestro.modal("show");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                let strMensajeError = "";
                cboFiltroAnio.val() <= 0 ? strMensajeError += "Es necesario indicar un año." : "";
                cboFiltroCC.val() <= 0 ? strMensajeError += "<br>Es necesario indicar un CC." : "";
                Alert2Warning(strMensajeError);
            }
        }

        function initTblMedicionesIndicadores() {
            dtMedicionesIndicadores = tblMedicionesIndicadores.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'indicador', title: 'INDICADOR' },
                    { data: 'fuenteDatos', title: 'FUENTE DE DATOS' },
                    { data: 'usuarioResponsable', title: 'RESPONSABLE' },
                    { data: 'meta', title: 'META' },
                    { data: 'idPlanMaestro', title: 'idPlanMaestro', visible: false },
                    { data: 'id', title: 'id', visible: false },
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                    { className: 'dt-body-center', targets: "_all" },
                    { className: 'dt-body-right', targets: "_all" },
                ],
            });
        }

        function initTblAgrupacionesConceptos() {
            dtAgrupacionesConceptos = tblAgrupacionesConceptos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    {
                        data: 'concepto', title: 'AGRUPACIÓN',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        }
                    },
                    {
                        data: 'total', title: 'TOTAL',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    { data: 'esAgrupacion', title: 'esAgrupacion', visible: false },
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                    function initTblNombre() {
                        dtNombre = tblNombre.DataTable({
                            language: dtDicEsp,
                            destroy: false,
                            paging: false,
                            ordering: false,
                            searching: false,
                            bFilter: false,
                            info: false,
                            columns: [
                                {
                                    render: function (data, type, row, meta) {
                                        let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                                        let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                                        return btnEditar + btnEliminar;
                                    },
                                }
                            ],
                            initComplete: function (settings, json) {
                                tblNombre.on('click', '.editarRegistro', function () {
                                    let rowData = dtNombre.row($(this).closest('tr')).data();
                                    fncGetDatosActualizarCaptura(rowData.id);
                                });
                                tblNombre.on('click', '.eliminarRegistro', function () {
                                    let rowData = dtNombre.row($(this).closest('tr')).data();
                                    Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarCaptura(rowData.id));
                                });
                            },
                            columnDefs: [
                                { className: 'dt-center', 'targets': '_all' },
                                //{ className: 'dt-body-center', targets: [0] },
                                //{ className: 'dt-body-right', targets: [0] },
                                //{ width: '5%', targets: [0] }
                            ],
                        });
                    }
                ],
            });
        }
        //#endregion

        //#region DETALLE
        function initTblDetCapturas() {
            dtDetCapturas = tblDetCapturas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'agrupacion', title: 'Agrupación' },
                    { data: 'concepto', title: 'Concepto' },
                    {
                        data: 'importeEnero', title: 'Enero',
                        render: function (data, type, row) {
                            console.log(data);
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeFebrero', title: 'Febrero',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeMarzo', title: 'Marzo',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeAbril', title: 'Abril',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeMayo', title: 'Mayo',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeJunio', title: 'Junio',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeJulio', title: 'Julio',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeAgosto', title: 'Agosto',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeSeptiembre', title: 'Septiembre',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeOctubre', title: 'Octubre',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeNoviembre', title: 'Noviembre',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeDiciembre', title: 'Diciembre',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    { data: 'anio', title: 'Año' },
                    { data: 'responsable', title: 'Responsable' }
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                    { targets: [0, 1, 14, 15], className: 'dt-body-center' },
                    { targets: [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13], className: 'dt-body-right' }
                ],
            });
        }

        function fncGetDetCapturas(idAgrupacion, idConcepto) {
            if (cboFiltroCC.val() > -1) {
                let obj = new Object();
                obj = {
                    cc: cboFiltroCC.val(),
                    idAgrupacion: idAgrupacion,
                    idConcepto: idConcepto,
                    anio: cboFiltroAnio.val()
                }
                axios.post("GetCapturas", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtDetCapturas.clear();
                        dtDetCapturas.rows.add(response.data.lstCapPptos);
                        dtDetCapturas.draw();
                        mdlDetCaptura.modal("show");
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Es necesario indicar un CC.");
            }
        }

        function fncGetDetCapturasContable(idAgrupacion, idConcepto) {
            if (cboFiltroCC.val() > -1) {
                let obj = new Object();
                obj = {
                    idCC: cboFiltroCC.val(),
                    idAgrupacion: idAgrupacion,
                    idConcepto: idConcepto,
                    anio: cboFiltroAnio.val()
                }
                axios.post("GetCapturasContable", obj).then(response => {
                    let { success, data, message } = response.data;
                    if (success) {
                        AddRows(tablaMesContable, data);
                        modalMesContable.modal('show');
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Es necesario indicar un CC.");
            }
        }
        //#endregion

        //#region DETALLE POR MES
        function initTblDetCapturasMes() {
            dtDetCapturasMes = tblDetCapturasMes.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'agrupacion', title: 'Agrupación' },
                    { data: 'concepto', title: 'Concepto' },
                    {
                        data: 'importe', title: 'Importe', render: function (data, type, row, meta) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        title: 'Aditiva',
                        render: function (data, type, row, meta) {
                            return `<button class="btn btn-primary btn-xs verAditivas" title="Aditivas."><i class="fas fa-plus"></i></button>&nbsp;`;
                        },
                    },
                    { data: 'anio', title: 'Año' },
                    { data: 'responsable', title: 'Responsable' },
                    { data: 'id', visible: false },
                ],
                initComplete: function (settings, json) {
                    tblDetCapturasMes.on('click', '.verAditivas', function () {
                        let rowData = dtDetCapturasMes.row($(this).closest('tr')).data();
                        fncGetAditivas(rowData.id, btnRegresar.attr("mes"));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function initTablaMesContable() {
            dtMesContable = tablaMesContable.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    {
                        title: 'Póliza', render: function (data, type, row, meta) {
                            return row.year + '-' + row.mes + '-' + row.poliza + '-' + row.tp;
                        }
                    },
                    { data: 'linea', title: 'Linea' },
                    { data: 'cta', title: 'cta' },
                    { data: 'scta', title: 'scta' },
                    { data: 'sscta', title: 'sscta' },
                    { data: 'concepto', title: 'Concepto' },
                    {
                        data: 'monto', title: 'Monto', render: function (data, type, row, meta) {
                            return maskNumero2DCompras(data);
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function initTablaTotalContable() {
            dtTotalContable = tablaTotalContable.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    {
                        data: 'importeEnero', title: 'Enero',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeFebrero', title: 'Febrero',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeMarzo', title: 'Marzo',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeAbril', title: 'Abril',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeMayo', title: 'Mayo',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeJunio', title: 'Junio',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeJulio', title: 'Julio',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeAgosto', title: 'Agosto',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeSeptiembre', title: 'Septiembre',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeOctubre', title: 'Octubre',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeNoviembre', title: 'Noviembre',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    },
                    {
                        data: 'importeDiciembre', title: 'Diciembre',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    }
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                    { targets: [0, 1], className: 'dt-body-center' }
                ],
            });
        }

        function fncGetDetCapturasMes(idAgrupacion, idConcepto, idMes) {
            if (cboFiltroCC.val() > -1) {
                let obj = new Object();
                obj = {
                    cc: cboFiltroCC.val(),
                    idAgrupacion: idAgrupacion,
                    idConcepto: idConcepto,
                    idMes: idMes,
                    anio: cboFiltroAnio.val()
                }
                axios.post("GetCapturasPorMes", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtDetCapturasMes.clear();
                        dtDetCapturasMes.rows.add(response.data.lstCapPptos);
                        dtDetCapturasMes.draw();
                        switch (idMes) {
                            case 1:
                                lblTitlePorMes.html(" - ENERO");
                                break;
                            case 2:
                                lblTitlePorMes.html(" - FEBRERO");
                                break;
                            case 3:
                                lblTitlePorMes.html(" - MARZO");
                                break;
                            case 4:
                                lblTitlePorMes.html(" - ABRIL");
                                break;
                            case 5:
                                lblTitlePorMes.html(" - MAYO");
                                break;
                            case 6:
                                lblTitlePorMes.html(" - JUNIO");
                                break;
                            case 7:
                                lblTitlePorMes.html(" - JULIO");
                                break;
                            case 8:
                                lblTitlePorMes.html(" - AGOSTO");
                                break;
                            case 9:
                                lblTitlePorMes.html(" - SEPTIEMBRE");
                                break;
                            case 10:
                                lblTitlePorMes.html(" - OCTUBRE");
                                break;
                            case 11:
                                lblTitlePorMes.html(" - NOVIEMBRE");
                                break;
                            case 12:
                                lblTitlePorMes.html(" - DICIEMBRE");
                                break;
                            default:
                                break;
                        }
                        mdlDetCapturaMes.modal("show");
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Es necesario indicar un CC.");
            }
        }

        function fncGetDetCapturasMesContable(idAgrupacion, idConcepto, idMes) {
            if (cboFiltroCC.val() > -1) {
                let obj = new Object();
                obj = {
                    idCC: cboFiltroCC.val(),
                    idAgrupacion: idAgrupacion,
                    idConcepto: idConcepto,
                    idMes: idMes,
                    anio: cboFiltroAnio.val()
                }
                axios.post("GetCapturasPorMesContable", obj).then(response => {
                    let { success, data, message } = response.data;
                    if (success) {
                        AddRows(tablaMesContable, data);
                        modalMesContable.modal('show');
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Es necesario indicar un CC.");
            }
        }

        function initTblAditivas() {
            dtAditivas = tblAditivas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'comentario', title: 'Comentario' },
                    {
                        data: 'importe', title: 'Importe',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(data);
                        }
                    }
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetAditivas(idCapPpto, idMes) {
            let obj = new Object();
            obj = {
                id: idCapPpto,
                idMes: idMes
            }
            axios.post("GetAditivasPorCaptura", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtAditivas.clear();
                    dtAditivas.rows.add(response.data.lstAditivas);
                    dtAditivas.draw();
                    mdlAditivas.modal("show");
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }

    $(document).ready(() => {
        CtrlPptalOfCE.index = new index();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();