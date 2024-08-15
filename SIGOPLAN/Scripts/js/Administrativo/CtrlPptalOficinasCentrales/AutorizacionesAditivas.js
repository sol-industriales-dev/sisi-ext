(() => {
    $.namespace('Presupuesto.CtrlPptalOficinasCentrales');
    CtrlPptalOficinasCentrales = function () {
        //#region Selectores
        const comboBoxYear = $('#comboBoxYear');
        const comboBoxCC = $('#comboBoxCC');
        const comboBoxEstatus = $('#comboBoxEstatus');
        const botonBuscar = $('#botonBuscar');
        const tablaAditivas = $('#tablaAditivas');
        const modalSeguimientoFirmas = $('#modalSeguimientoFirmas');
        const modalSeguimientoFirmasTitulo = $('#modalSeguimientoFirmasTitulo');
        const tablaSeguimientoFirmas = $('#tablaSeguimientoFirmas');
        const modalRechazo = $('#modalRechazo');
        const modalRechazoTitulo = $('#modalRechazoTitulo');
        const comentarioRechazo = $('#comentarioRechazo');
        const btnRechazar = $('#btnRechazar');
        const modalJustificacion = $('#modalJustificacion');
        const txtMdlJustificacion = $('#txtMdlJustificacion');
        //#endregion

        let parametro_Anio = 0;
        let parametro_idMes = 0;
        let parametro_idCC = 0;


        const TipoPresupuestoEnum = {
            original: 0,
            aditivaDeductivaAutorizada: 1,
            total: 2,
            aditivaDeductivaPendiente: 3,
            aditivaDeductivaNueva: 4,
            aditivaDeductivaRechazada: 5
        };

        //#region eventos
        botonBuscar.on('click', function () {
            if (/*comboBoxCC.val() == '' ||*/ comboBoxYear.val() == '') {
                let strMensajeError = "";
                //strMensajeError += comboBoxCC.val() == '' ? "Es necesario indicar un CC." : "";
                strMensajeError += comboBoxYear.val() == '' ? "<br>Es necesario indicar un año." : "";

                Alert2Warning(strMensajeError);
                return;
            }

            let estatus = null;

            if (comboBoxEstatus.val()) {
                estatus = comboBoxEstatus.val();
            }

            $.get('GetAditivasAAutorizar',
                {
                    cc: comboBoxCC.val(),
                    year: comboBoxYear.val(),
                    esAutorizacion: estatus
                }).then(response => {
                    if (response.success) {
                        addRows(tablaAditivas, response.items);
                    } else {
                        swal('Aviso', response.message, 'warning');
                    }
                }, error => {
                    swal('Aviso', `Ocurrió un error al lanzar la petición al servidor:\n${error.status} - ${error.statusText}`, 'error');
                });
        });

        btnRechazar.on('click', function () {
            $.post('RechazarAditiva',
                {
                    aditivaId: $(this).data('idaditiva'),
                    comentario: comentarioRechazo.val()
                }).then(response => {
                    if (response.success) {
                        Alert2Exito('Se ha guardado la información.');
                        botonBuscar.trigger('click');
                        modalRechazo.modal("hide");
                    } else {
                        swal('Aviso', response.message, 'warning');
                    }
                }, error => {
                    swal('Aviso', `Ocurrió un error al lanzar la petición al servidor:\n${error.status} - ${error.statusText}`, 'error');
                });
        });

        modalRechazo.on('shown.bs.modal', function () {
            comentarioRechazo.css('width', '100%');
        });
        //#endregion

        (function init() {
            comboBoxYear.fillCombo('FillAnios', {}, false);
            comboBoxYear.select2();

            comboBoxYear.on("change", function () {
                comboBoxCC.fillCombo('FillUsuarioRelCC', { anio: $(this).val() }, false);
                $("#comboBoxCC option[value]:first").text("-- Todos --");
                comboBoxCC.select2();
            });

            comboBoxEstatus.select2();

            initTablaAditivas();
            initTablaSeguimiento();

            obtenerUrlPArams();
        })();

        function initTablaAditivas() {
            tablaAditivas.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                ordering: false,
                scrollY: '45vh',
                scrollX: true,
                scrollCollapse: true,
                columns: [
                    {
                        data: 'ccString',
                        title: 'CC',
                        className: 'dt-head-center',
                        visible: false
                    },
                    {
                        data: 'agrupacion',
                        title: 'AGRUPACIÓN',
                        className: 'dt-head-center'
                    },
                    {
                        data: 'descripcion',
                        title: 'CONCEPTO',
                        className: 'dt-head-center dt-body-left'
                    },
                    {
                        data: 'actividad',
                        title: 'ACTIVIDAD',
                        className: 'dt-head-center'
                    },
                    {
                        data: 'importeEnero',
                        title: 'ENERO',
                        className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            return data >= 0 ? maskNumero2DCompras(data) : '-' + maskNumero2DCompras(data).replace('-', '');
                        }
                    },
                    {
                        data: 'importeFebrero',
                        title: 'FEBRERO',
                        className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            return data >= 0 ? maskNumero2DCompras(data) : '-' + maskNumero2DCompras(data).replace('-', '');
                        }
                    },
                    {
                        data: 'importeMarzo',
                        title: 'MARZO',
                        className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            return data >= 0 ? maskNumero2DCompras(data) : '-' + maskNumero2DCompras(data).replace('-', '');
                        }
                    },
                    {
                        data: 'importeAbril',
                        title: 'ABRIL',
                        className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            return data >= 0 ? maskNumero2DCompras(data) : '-' + maskNumero2DCompras(data).replace('-', '');
                        }
                    },
                    {
                        data: 'importeMayo',
                        title: 'MAYO',
                        className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            return data >= 0 ? maskNumero2DCompras(data) : '-' + maskNumero2DCompras(data).replace('-', '');
                        }
                    },
                    {
                        data: 'importeJunio',
                        title: 'JUNIO',
                        className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            return data >= 0 ? maskNumero2DCompras(data) : '-' + maskNumero2DCompras(data).replace('-', '');
                        }
                    },
                    {
                        data: 'importeJulio',
                        title: 'JULIO',
                        className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            return data >= 0 ? maskNumero2DCompras(data) : '-' + maskNumero2DCompras(data).replace('-', '');
                        }
                    },
                    {
                        data: 'importeAgosto',
                        title: 'AGOSTO',
                        className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            return data >= 0 ? maskNumero2DCompras(data) : '-' + maskNumero2DCompras(data).replace('-', '');
                        }
                    },
                    {
                        data: 'importeSeptiembre',
                        title: 'SEPTIEMBRE',
                        className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            return data >= 0 ? maskNumero2DCompras(data) : '-' + maskNumero2DCompras(data).replace('-', '');
                        }
                    },
                    {
                        data: 'importeOctubre',
                        title: 'OCTUBRE',
                        className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            return data >= 0 ? maskNumero2DCompras(data) : '-' + maskNumero2DCompras(data).replace('-', '');
                        }
                    },
                    {
                        data: 'importeNoviembre',
                        title: 'NOVIEMBRE',
                        className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            return data >= 0 ? maskNumero2DCompras(data) : '-' + maskNumero2DCompras(data).replace('-', '');
                        }
                    },
                    {
                        data: 'importeDiciembre',
                        title: 'DICIEMBRE',
                        className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            return data >= 0 ? maskNumero2DCompras(data) : '-' + maskNumero2DCompras(data).replace('-', '');
                        }
                    },
                    {
                        data: 'importeTotal',
                        title: 'TOTAL',
                        className: 'dt-body-right dt-head-center',
                        render: function (data, type, row) {
                            return data >= 0 ? maskNumero2DCompras(data) : '-' + maskNumero2DCompras(data).replace('-', '');
                        }
                    },
                    {
                        data: 'indicador',
                        title: 'ESTATUS',
                        className: 'dt-center',
                        render: function (data, type, row) {
                            var estatus = '';
                            switch (data) {
                                case TipoPresupuestoEnum.aditivaDeductivaAutorizada:
                                    estatus = 'AUTORIZADA';
                                    break;
                                case TipoPresupuestoEnum.aditivaDeductivaPendiente:
                                    estatus = 'PENDIENTE';
                                    break;
                                case TipoPresupuestoEnum.aditivaDeductivaRechazada:
                                    estatus = 'RECHAZADA';
                                    break;
                            }
                            return estatus;
                        }
                    },
                    {
                        data: null,
                        title: 'OPCIONES',
                        className: 'dt-head-center',
                        render: function (data, type, row) {
                            let opcionSeguimiento = `<button class="btn btn-xs btn-primary dtBtnSeguimientoAutorizacion" title="Consultar estatus autorizaciones">
                                                        <i class="fas fa-list-ol"></i>
                                                </button>`;
                            let opcionJustificacion = `<button class="btn btn-xs btn-primary dtBtnJustificacion" title="Ver justificación">
                                                            <i class="far fa-comment"></i>
                                                        </button>`;
                            let opcionAutorizar = `<button class="btn btn-xs btn-success dtBtnAutorizar" title="Autorizar">
                                                        <i class="fas fa-file-signature"></i>
                                                    </button>`;
                            let opcionRechazar = `<button class="btn btn-xs btn-danger dtBtnRechazar" title="Rechazar">
                                                        <i class="fas fa-ban"></i>
                                                </button>`;

                            if (row.elUsuarioPuedeAutorizar) {
                                return opcionSeguimiento + ' ' + opcionJustificacion + ' ' + opcionAutorizar + ' ' + opcionRechazar;
                            } else {
                                return opcionSeguimiento;
                            }
                        }
                    }
                ],

                drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;

                    api
                        .column(0, { page: 'current' })
                        .data()
                        .each(function (group, i) {
                            if (last !== group) {
                                $(rows)
                                    .eq(i)
                                    .before('<tr class="group"><td colspan="19" style="background-color:lightgray;">' + group + '</td></tr>');

                                last = group;
                            }
                        });
                },

                initComplete: function (settings, json) {
                    tablaAditivas.on('click', '.dtBtnSeguimientoAutorizacion', function () {
                        let rowData = tablaAditivas.DataTable().row($(this).closest('tr')).data();

                        modalSeguimientoFirmasTitulo.text(rowData.descripcion);

                        cargarSeguimientoAutorizacion(rowData.idAditiva);
                    });

                    tablaAditivas.on('click', '.dtBtnAutorizar', function () {
                        let rowData = tablaAditivas.DataTable().row($(this).closest('tr')).data();

                        Alert2AccionConfirmar('Atención',
                            '¿Desea autorizar la aditiva?<br>Justificación:<br>' + rowData.comentario, 'Autorizar', 'Cancelar', () => autorizarAditiva(rowData.idAditiva));
                    });

                    tablaAditivas.on('click', '.dtBtnJustificacion', function () {
                        let rowData = tablaAditivas.DataTable().row($(this).closest('tr')).data();

                        modalJustificacion.modal("show");
                        txtMdlJustificacion.text(rowData.comentario);

                    });

                    tablaAditivas.on('click', '.dtBtnRechazar', function () {
                        let rowData = tablaAditivas.DataTable().row($(this).closest('tr')).data();

                        modalRechazoTitulo.text(`${rowData.descripcion} - Rechazar`);
                        btnRechazar.removeData('idaditiva');
                        btnRechazar.data('idaditiva', rowData.idAditiva);
                        modalRechazo.modal('show');
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
                    // { data: 'tipoAutorizante', title: 'Autorizante' },
                    {
                        data: 'estatus', title: 'Estado', render: function (data, type, row, meta) {
                            if (row.rechazado) {
                                return 'RECHAZADO'
                            } else {
                                switch (data) {
                                    case true:
                                        return 'AUTORIZADO';
                                    case false:
                                        return 'PENDIENTE';
                                }
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function addRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function cargarSeguimientoAutorizacion(aditivaId) {
            $.get('GetDetalleAutorizacionAditiva',
                {
                    aditivaId
                }).then(response => {
                    if (response.success) {
                        addRows(tablaSeguimientoFirmas, response.items);
                        modalSeguimientoFirmas.modal('show');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }, error => {
                    AlertaGeneral(`Alerta`, error.message)
                });
        }

        function autorizarAditiva(aditivaId) {
            axios.post('AutorizarAditiva', { aditivaId })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha guardado la información.');
                        botonBuscar.click();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function obtenerUrlPArams() {
            const variables = getUrlParams(window.location.href);
            if (variables != undefined) {
                let idPlanAccion = variables.idPlanAccion != undefined && variables.idPlanAccion > 0 ? variables.idPlanAccion : 0;
                if (idPlanAccion > 0) {
                    fncGetPlanAccion(0, 0, 0, idPlanAccion);
                }

                let anio = variables.anio != undefined && variables.anio > 0 ? variables.anio : 0;
                let idCC = variables.idCC != undefined && variables.idCC > 0 ? variables.idCC : 0;

                if (anio > 0 && idCC > 0) {
                    parametro_Anio = anio
                    parametro_idCC = idCC;

                    comboBoxYear.val(parametro_Anio);
                    comboBoxYear.trigger("change");

                    comboBoxCC.val(parametro_idCC);
                    comboBoxCC.trigger("change");

                    comboBoxEstatus.val('false');
                    comboBoxEstatus.trigger("change");

                    botonBuscar.click();
                }

                var clean_uri = location.protocol + "//" + location.host + location.pathname;
                window.history.replaceState({}, document.title, clean_uri);
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
    }
    $(document).ready(() => Presupuesto.CtrlPptalOficinasCentrales = new CtrlPptalOficinasCentrales())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();