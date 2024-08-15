(() => {
    $.namespace('Presupuesto.CtrlPptalOficinasCentrales');

    CtrlPptalOficinasCentrales = function () {
        //#region Selectores
        const divVerComentario = $('#divVerComentario');
        const ulComentarios = $('#ulComentarios');
        const txtComentarios = $('#txtComentarios');
        const btnCrearComentario = $('#btnCrearComentario');
        const comboBoxCC = $('#comboBoxCC');
        const comboBoxYear = $('#comboBoxYear');
        const comboBoxEstatus = $('#comboBoxEstatus');
        const botonBuscar = $('#botonBuscar');
        const tablaPresupuestos = $('#tablaPresupuestos');
        const modalSeguimientoFirmas = $('#modalSeguimientoFirmas');
        const modalSeguimientoFirmasTitulo = $('#modalSeguimientoFirmasTitulo');
        const tablaSeguimientoFirmas = $('#tablaSeguimientoFirmas');
        //#endregion

        //#region CONST RECHAZAR PPTO
        const mdlRechazarPpto = $('#mdlRechazarPpto');
        const txtRechazarPpto = $('#txtRechazarPpto');
        const btnRechazarPpto = $('#btnRechazarPpto');
        //#endregion

        //#region CONST DETALLE CAPTURA
        const mdlCaptura = $('#mdlCaptura');
        const tblCapturas = $('#tblCapturas');
        let dtCapturas;
        //#endregion

        botonBuscar.on('click', function () {
            if (comboBoxCC.val() == '' || comboBoxYear.val() == "") {
                let strMensajeError = "";
                comboBoxYear.val() <= 0 ? strMensajeError += "Es necesario indicar un año." : "";
                comboBoxCC.val() <= 0 ? strMensajeError += "<br>Es necesario indicar un CC." : "";
                Alert2Warning(strMensajeError);
                return;
            }

            $.get('GetPresupuestosAEvaluar',
                {
                    cc: comboBoxCC.val(),
                    year: comboBoxYear.val(),
                    estatus: comboBoxEstatus.val()
                }).then(response => {
                    if (response.success) {
                        addRows(tablaPresupuestos, response.items);
                    } else {
                        Alert2Warning(response.message);
                    }
                }, error => {
                    Alert2Error(`Ocurrió un error al lanzar la petición al servidor:\n${error.status} - ${error.statusText}`);
                })
        });

        let _idConcepto = 0;

        (function init() {
            //#region INIT DATATABLES
            initTablaPresupuestos();
            initTablaSeguimiento();
            initTblCapturas();
            //#endregion

            //#region FILL COMBOS
            comboBoxYear.fillCombo("FillAnios", {}, false);
            comboBoxYear.select2();
            comboBoxYear.select2({ width: "100%" });

            comboBoxEstatus.select2();
            comboBoxEstatus.select2({ width: "100%" });
            //#endregion

            comboBoxYear.on("change", function () {
                if ($(this).val() > 0) {
                    comboBoxCC.fillCombo("FillUsuarioRelCC", { anio: $(this).val() }, false);
                    comboBoxCC.select2();
                    comboBoxCC.select2({ width: "100%" });
                }
            });

            btnRechazarPpto.on("click", function () {
                fncRechazarPpto();
            });

            obtenerUrlPArams();
        })();

        function obtenerUrlPArams() {
            const variables = getUrlParams(window.location.href);
            if (variables != undefined) {
                let anio = variables.anio != undefined && variables.anio > 0 ? variables.anio : 0;
                let idCC = variables.idCC != undefined && variables.idCC > 0 ? variables.idCC : 0;

                if (anio > 0 && idCC > 0) {
                    comboBoxYear.val(anio);
                    comboBoxYear.trigger("change");
                    comboBoxCC.val(idCC);
                    comboBoxCC.trigger("change");
                    comboBoxEstatus.val("false");
                    comboBoxEstatus.trigger("change");
                    botonBuscar.trigger("click");
                }
            }
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
        }

        function initTablaPresupuestos() {
            tablaPresupuestos.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                ordering: false,
                // "sScrollY": "0px",
                // "sScrollX": "0px%",
                scrollCollapse: false,
                columns: [
                    {
                        data: 'cc',
                        title: 'CC',
                        className: 'dt-center'
                    },
                    {
                        data: 'year',
                        title: 'AÑO',
                        className: 'dt-center'
                    },
                    {
                        data: 'nombrePresupuesto',
                        title: 'NOMBRE',
                        className: 'dt-head-center'
                    },
                    {
                        data: 'estatus',
                        title: 'ESTATUS',
                        className: 'dt-center',
                        render: function (data, type, row) {
                            switch (data) {
                                case true:
                                    return 'AUTORIZADO';
                                case false:
                                    return 'PENDIENTE';
                            }
                        }
                    },
                    {
                        data: null,
                        title: 'OPCIONES',
                        className: 'dt-head-center',
                        render: function (data, type, row) {
                            let opcionSeguimiento = `<button class="btn btn-xs btn-primary dtBtnSeguimientoAutorizacion" title="Consultar estatus autorizaciones"><i class="fas fa-list-ol"></i></button>`;
                            let opcionAutorizar = `<button class="btn btn-xs btn-success dtBtnAutorizar" title="Autorizar"><i class="fas fa-file-signature"></i></button>`;
                            let opcionDetalles = `<button class="btn btn-xs btn-success detalle" title="Detalles"><i class="fas fa-book-open"></i></button>`;
                            let opcionRechazar = `<button class="btn btn-xs btn-danger rechazar" title="Rechazar"><i class="fas fa-ban"></i></button>`;

                            if (row.autorizado || !row.elUsuarioPuedeAutorizar) {
                                return opcionSeguimiento + ' ' + opcionDetalles;
                            } else {
                                return opcionSeguimiento + ' ' + opcionAutorizar + ' ' + opcionDetalles + ' ' + opcionRechazar;
                            }
                        }
                    }
                ],

                initComplete: function (settings, json) {
                    tablaPresupuestos.on('click', '.dtBtnSeguimientoAutorizacion', function () {
                        let rowData = tablaPresupuestos.DataTable().row($(this).closest('tr')).data();

                        cargarSeguimientoAutorizacion(rowData.id);
                    });

                    tablaPresupuestos.on('click', '.dtBtnAutorizar', function () {
                        let rowData = tablaPresupuestos.DataTable().row($(this).closest('tr')).data();

                        Alert2AccionConfirmar('Atención', '¿Desea autorizar el presupuesto anual?', 'Confirmar', 'Cancelar', () => autorizarPresupuesto(rowData.id));
                    });

                    tablaPresupuestos.on('click', '.detalle', function () {
                        let rowData = tablaPresupuestos.DataTable().row($(this).closest('tr')).data();
                        fncGetSumaCapturas();
                    });

                    tablaPresupuestos.on("click", ".rechazar", function () {
                        let rowData = tablaPresupuestos.DataTable().row($(this).closest('tr')).data();
                        btnRechazarPpto.attr("data-id", rowData.id);
                        mdlRechazarPpto.modal("show");
                        // Alert2AccionConfirmar('Atención', '¿Desea rechazar el presupuesto anual?', 'Confirmar', 'Cancelar', () => fncRechazarPpto(rowData.id));
                    });

                }
            });
        }

        function initTablaSeguimiento() {
            tablaSeguimientoFirmas.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                ordering: false,
                initComplete: function (settings, json) {

                },
                drawCallback: function () {
                    $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                },
                columns: [
                    { data: 'numero', title: '#' },
                    { data: 'nombre', title: 'Nombre' },
                    { data: 'tipoAutorizante', title: 'Autorizante' },
                    {
                        data: 'estatus', title: 'Estado', render: function (data, type, row, meta) {
                            switch (data) {
                                case true:
                                    return 'AUTORIZADO';
                                case false:
                                    return 'PENDIENTE';
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarSeguimientoAutorizacion(presupuestoId) {
            axios.post('GetDetalleAutorizacion', { presupuestoId })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        addRows(tablaSeguimientoFirmas, response.data.items);
                        modalSeguimientoFirmas.modal('show');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function autorizarPresupuesto(presupuestoId) {
            axios.post('Autorizar', { presupuestoId })
                .then(response => {
                    let { success, datos, message } = response.data;
                    if (success) {
                        Alert2Exito('Se ha autorizado con éxito el presupuesto.');
                        comboBoxEstatus.val("true");
                        comboBoxEstatus.trigger("change");
                        botonBuscar.click();
                        modalSeguimientoFirmas.modal('hide');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function addRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function fncRechazarPpto(idPpto) {
            let obj = new Object();
            obj = {
                idPptoAnual: btnRechazarPpto.attr("data-id"),
                comentarioRechazo: txtRechazarPpto.val()
            }
            axios.post("RechazarPpto", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    mdlRechazarPpto.modal("hide");
                    addRows(tablaPresupuestos, "");
                    Alert2Exito(response.data.message);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
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
                columns: [
                    {
                        data: 'concepto', title: 'Concepto',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (data == "TOTAL") {
                                return `<label style="font-weight: bold;">${data}</label>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'importeTotalConcepto', title: "Total",
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            // $(td).css('background-color', "#93d4ff");
                            let color = (cellData.concepto == "TOTAL") ? '#dbdcd9' : '#93d4ff';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            return `<label style="font-weight: bold;">${maskNumero2DCompras(data)}</label>`;
                        }
                    },
                    {
                        data: 'importeEnero', title: 'Enero',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto == "TOTAL") {
                                return `<label style="font-weight: bold;">${maskNumero2DCompras(data)}</label>`;
                            } else {
                                if (!row.esAgrupacion) {
                                    return `<a class="importeEnero">${maskNumero2DCompras(data)}</a>`;
                                } else {
                                    return `<label style="font-weight: normal;">${maskNumero2DCompras(data)}</label>`;
                                }
                            }
                        }
                    },
                    {
                        data: 'importeFebrero', title: 'Febrero',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto == "TOTAL") {
                                return `<label style="font-weight: bold;">${maskNumero2DCompras(data)}</label>`;
                            } else {
                                if (!row.esAgrupacion) {
                                    return `<a class="importeFebrero">${maskNumero2DCompras(data)}</a>`;
                                } else {
                                    return `<label style="font-weight: normal;">${maskNumero2DCompras(data)}</label>`;
                                }
                            }
                        }
                    },
                    {
                        data: 'importeMarzo', title: 'Marzo',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto == "TOTAL") {
                                return `<label style="font-weight: bold;">${maskNumero2DCompras(data)}</label>`;
                            } else {
                                if (!row.esAgrupacion) {
                                    return `<a class="importeMarzo">${maskNumero2DCompras(data)}</a>`;
                                } else {
                                    return `<label style="font-weight: normal;">${maskNumero2DCompras(data)}</label>`;
                                }
                            }
                        }
                    },
                    {
                        data: 'importeAbril', title: 'Abril',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto == "TOTAL") {
                                return `<label style="font-weight: bold;">${maskNumero2DCompras(data)}</label>`;
                            } else {
                                if (!row.esAgrupacion) {
                                    return `<a class="importeAbril">${maskNumero2DCompras(data)}</a>`;
                                } else {
                                    return `<label style="font-weight: normal;">${maskNumero2DCompras(data)}</label>`;
                                }
                            }
                        }
                    },
                    {
                        data: 'importeMayo', title: 'Mayo',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto == "TOTAL") {
                                return `<label style="font-weight: bold;">${maskNumero2DCompras(data)}</label>`;
                            } else {
                                if (!row.esAgrupacion) {
                                    return `<a class="importeMayo">${maskNumero2DCompras(data)}</a>`;
                                } else {
                                    return `<label style="font-weight: normal;">${maskNumero2DCompras(data)}</label>`;
                                }
                            }
                        }
                    },
                    {
                        data: 'importeJunio', title: 'Junio',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto == "TOTAL") {
                                return `<label style="font-weight: bold;">${maskNumero2DCompras(data)}</label>`;
                            } else {
                                if (!row.esAgrupacion) {
                                    return `<a class="importeJunio">${maskNumero2DCompras(data)}</a>`;
                                } else {
                                    return `<label style="font-weight: normal;">${maskNumero2DCompras(data)}</label>`;
                                }
                            }
                        }
                    },
                    {
                        data: 'importeJulio', title: 'Julio',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto == "TOTAL") {
                                return `<label style="font-weight: bold;">${maskNumero2DCompras(data)}</label>`;
                            } else {
                                if (!row.esAgrupacion) {
                                    return `<a class="importeJulio">${maskNumero2DCompras(data)}</a>`;
                                } else {
                                    return `<label style="font-weight: normal;">${maskNumero2DCompras(data)}</label>`;
                                }
                            }
                        }
                    },
                    {
                        data: 'importeAgosto', title: 'Agosto',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto == "TOTAL") {
                                return `<label style="font-weight: bold;">${maskNumero2DCompras(data)}</label>`;
                            } else {
                                if (!row.esAgrupacion) {
                                    return `<a class="importeAgosto">${maskNumero2DCompras(data)}</a>`;
                                } else {
                                    return `<label style="font-weight: normal;">${maskNumero2DCompras(data)}</label>`;
                                }
                            }
                        }
                    },
                    {
                        data: 'importeSeptiembre', title: 'Septiembre',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto == "TOTAL") {
                                return `<label style="font-weight: bold;">${maskNumero2DCompras(data)}</label>`;
                            } else {
                                if (!row.esAgrupacion) {
                                    return `<a class="importeSeptiembre">${maskNumero2DCompras(data)}</a>`;
                                } else {
                                    return `<label style="font-weight: normal;">${maskNumero2DCompras(data)}</label>`;
                                }
                            }
                        }
                    },
                    {
                        data: 'importeOctubre', title: 'Octubre',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto == "TOTAL") {
                                return `<label style="font-weight: bold;">${maskNumero2DCompras(data)}</label>`;
                            } else {
                                if (!row.esAgrupacion) {
                                    return `<a class="importeOctubre">${maskNumero2DCompras(data)}</a>`;
                                } else {
                                    return `<label style="font-weight: normal;">${maskNumero2DCompras(data)}</label>`;
                                }
                            }
                        }
                    },
                    {
                        data: 'importeNoviembre', title: 'Noviembre',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto == "TOTAL") {
                                return `<label style="font-weight: bold;">${maskNumero2DCompras(data)}</label>`;
                            } else {
                                if (!row.esAgrupacion) {
                                    return `<a class="importeNoviembre">${maskNumero2DCompras(data)}</a>`;
                                } else {
                                    return `<label style="font-weight: normal;">${maskNumero2DCompras(data)}</label>`;
                                }
                            }
                        }
                    },
                    {
                        data: 'importeDiciembre', title: 'Diciembre',
                        createdCell: function (td, tr, cellData, rowData, row, col) {
                            let color = (cellData.esAgrupacion) ? '#dbdcd9' : 'white';
                            $(td).css('background-color', color);
                        },
                        render: function (data, type, row) {
                            if (row.concepto == "TOTAL") {
                                return `<label style="font-weight: bold;">${maskNumero2DCompras(data)}</label>`;
                            } else {
                                if (!row.esAgrupacion) {
                                    return `<a class="importeDiciembre">${maskNumero2DCompras(data)}</a>`;
                                } else {
                                    return `<label style="font-weight: normal;">${maskNumero2DCompras(data)}</label>`;
                                }
                            }
                        }
                    },
                    {
                        visible: false,
                        render: function (data, type, row, meta) {
                            if (row.concepto != "TOTAL") {
                                return `<button class="btn btn-xs btn-primary detalle" title="Detalle del concepto."><i class="fas fa-list-ul"></i></button>&nbsp;`;
                            } else {
                                return "";
                            }
                        },
                    },
                    { data: 'cc', visible: false },
                    { data: 'idAgrupacion', visible: false },
                    { data: 'idConcepto', visible: false },
                    { data: 'agrupacion', title: 'Agrupación', visible: false },
                    { data: 'esAgrupacion', visible: false },
                    { data: 'id', visible: false },
                ],
                initComplete: function (settings, json) {
                    //#region COMENTARIOS
                    tblCapturas.on("click", ".importeEnero", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 1);
                        fncGetComentarios(1, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblCapturas.on("click", ".importeFebrero", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 2);
                        fncGetComentarios(2, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblCapturas.on("click", ".importeMarzo", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 3);
                        fncGetComentarios(3, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblCapturas.on("click", ".importeAbril", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 4);
                        fncGetComentarios(4, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblCapturas.on("click", ".importeMayo", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 5);
                        fncGetComentarios(5, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblCapturas.on("click", ".importeJunio", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 6);
                        fncGetComentarios(6, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblCapturas.on("click", ".importeJulio", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 7);
                        fncGetComentarios(7, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblCapturas.on("click", ".importeAgosto", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 8);
                        fncGetComentarios(8, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblCapturas.on("click", ".importeSeptiembre", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 9);
                        fncGetComentarios(9, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblCapturas.on("click", ".importeOctubre", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 10);
                        fncGetComentarios(10, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblCapturas.on("click", ".importeNoviembre", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 11);
                        fncGetComentarios(11, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });

                    tblCapturas.on("click", ".importeDiciembre", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        btnCrearComentario.attr("data-id", 12);
                        fncGetComentarios(12, rowData.idConcepto);
                        _idConcepto = rowData.idConcepto;
                        divVerComentario.modal("show");
                    });
                    //#endregion

                    tblCapturas.on('click', '.detalle', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        fncGetDetCapturas(rowData.idAgrupacion, rowData.idConcepto);
                    });

                    tblCapturas.on('click', '.importeEnero', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        if (rowData.concepto != "TOTAL") {
                            // fncGetDetCapturasMes(rowData.idAgrupacion, rowData.idConcepto, 1);
                        }
                    });

                    tblCapturas.on('click', '.importeFebrero', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        if (rowData.concepto != "TOTAL") {
                            // fncGetDetCapturasMes(rowData.idAgrupacion, rowData.idConcepto, 2);
                        }
                    });

                    tblCapturas.on('click', '.importeMarzo', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        if (rowData.concepto != "TOTAL") {
                            // fncGetDetCapturasMes(rowData.idAgrupacion, rowData.idConcepto, 3);
                        }
                    });

                    tblCapturas.on('click', '.importeAbril', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        if (rowData.concepto != "TOTAL") {
                            // fncGetDetCapturasMes(rowData.idAgrupacion, rowData.idConcepto, 4);
                        }
                    });

                    tblCapturas.on('click', '.importeMayo', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        if (rowData.concepto != "TOTAL") {
                            // fncGetDetCapturasMes(rowData.idAgrupacion, rowData.idConcepto, 5);
                        }
                    });

                    tblCapturas.on('click', '.importeJunio', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        if (rowData.concepto != "TOTAL") {
                            // fncGetDetCapturasMes(rowData.idAgrupacion, rowData.idConcepto, 6);
                        }
                    });

                    tblCapturas.on('click', '.importeJulio', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        if (rowData.concepto != "TOTAL") {
                            // fncGetDetCapturasMes(rowData.idAgrupacion, rowData.idConcepto, 7);
                        }
                    });

                    tblCapturas.on('click', '.importeAgosto', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        if (rowData.concepto != "TOTAL") {
                            // fncGetDetCapturasMes(rowData.idAgrupacion, rowData.idConcepto, 8);
                        }
                    });

                    tblCapturas.on('click', '.importeSeptiembre', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        if (rowData.concepto != "TOTAL") {
                            // fncGetDetCapturasMes(rowData.idAgrupacion, rowData.idConcepto, 9);
                        }
                    });

                    tblCapturas.on('click', '.importeOctubre', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        if (rowData.concepto != "TOTAL") {
                            // fncGetDetCapturasMes(rowData.idAgrupacion, rowData.idConcepto, 10);
                        }
                    });

                    tblCapturas.on('click', '.importeNoviembre', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        if (rowData.concepto != "TOTAL") {
                            // fncGetDetCapturasMes(rowData.idAgrupacion, rowData.idConcepto, 11);
                        }
                    });

                    tblCapturas.on('click', '.importeDiciembre', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        if (rowData.concepto != "TOTAL") {
                            // fncGetDetCapturasMes(rowData.idAgrupacion, rowData.idConcepto, 12);
                        }
                    });
                },
                columnDefs: [
                    { targets: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13], className: 'dt-head-center dt-body-right' },
                    { targets: [0], className: 'dt-center' }
                ],
            });
        }

        function fncGetComentarios(mes, idConcepto) {
            let objDTO = { anio: comboBoxYear.val(), idCC: comboBoxCC.val(), idMes: btnCrearComentario.attr("data-id"), idConcepto };

            axios.post("GetComentarios", objDTO).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    let cantComentarios = response.data.lstComentarios;
                    let arr = new Array();
                    for (let i = 0; i < cantComentarios.length; i++) {
                        let data = new Object();
                        data.fecha = moment(response.data.lstComentarios[i].fechaCreacion).format("DD/MM/YYYY");
                        data.usuarioNombre = response.data.lstComentarios[i].usuarioNombre;
                        data.comentario = response.data.lstComentarios[i].comentario;
                        arr.push(data);
                    }
                    setComentarios(arr);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function setComentarios(data) {
            var htmlComentario = "";
            $.each(data, function (i, e) {
                htmlComentario += "<li class='comentario' data-id='100'>";
                htmlComentario += "    <div class='timeline-item'>";
                htmlComentario += "        <span class='time'><i class='fa fa-clock-o'></i>" + e.fecha + "</span>";
                htmlComentario += "        <h3 class='timeline-header'><a href='#'>" + e.usuarioNombre + "</a></h3>";
                htmlComentario += "        <div class='timeline-body'>";
                htmlComentario += "             " + e.comentario;
                htmlComentario += "        </div>";
                htmlComentario += "    </div>";
                htmlComentario += "</li>";
            });
            ulComentarios.html(htmlComentario);
        }

        function fncGetSumaCapturas() {
            if (comboBoxCC.val() > 0 || comboBoxYear.val() > 0) {
                let obj = new Object();
                obj = {
                    idCC: comboBoxCC.val(),
                    anio: comboBoxYear.val()
                }
                axios.post("GetSumaCapturas", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtCapturas.clear();
                        dtCapturas.rows.add(response.data.lstSumaCapturas);
                        dtCapturas.draw();
                        mdlCaptura.modal("show");
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                let strMensajeError = "";
                comboBoxYear.val() <= 0 ? strMensajeError += "Es necesario indicar un año." : "";
                comboBoxCC.val() <= 0 ? strMensajeError += "<br>Es necesario indicar un CC" : "";
                Alert2Warning(strMensajeError);
            }
        }
        //#endregion
    }

    $(document).ready(() => Presupuesto.CtrlPptalOficinasCentrales = new CtrlPptalOficinasCentrales())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();