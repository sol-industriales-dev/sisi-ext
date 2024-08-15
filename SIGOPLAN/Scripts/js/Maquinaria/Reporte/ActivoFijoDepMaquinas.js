(() => {
    $.namespace('Maquinaria.ActivoFijo.DepreciacionMaquinas');
    DepreciacionMaquinas = function () {

        //Como enums
        const TipoResultadoCuentasMaquinaria = {
            CboxMaquinaria: 1
        };

        //Objetos tablas
        const tblDepMaquinas = $('#tblDepMaquinas');
        const tblPolizasCC = $('#tblPolizasCC');
        const tblInsumosSinCapturar = $('#tblInsumosSinCapturar');
        const tblNuevaPoliza = $('#tblNuevaPoliza');

        //Objetos filtro
        const cboFiltroMaquinaria = $('#cboFiltroMaquinaria');
        const cboFiltroEstatusMaquina = $('#cboFiltroEstatusMaquina');
        const cboFiltroTipoCaptura = $('#cboFiltroTipoCaptura');
        const btnActualizacionAutomatica = $('#btnActualizacionAutomatica');

        //Objetos formulario registro/modificación de depreciación
        const btnRegistrar = $('#btnRegistrar');

        // const txtAgregarPoliza_Año = $('#txtAgregarPoliza_Año');
        // const txtAgregarPoliza_Mes = $('#txtAgregarPoliza_Mes');
        // const txtAgregarPoliza_Poliza = $('#txtAgregarPoliza_Poliza');
        // const txtAgregarPoliza_Tp = $('#txtAgregarPoliza_Tp');
        // const txtAgregarPoliza_Linea = $('#txtAgregarPoliza_Linea');

        // const btnAgregarPoliza = $('#btnAgregarPoliza');
        const btnPolizasNuevas = $('#btnPolizasNuevas');
        const btnModificar = $('#btnModificar');
        const btnEliminar = $('#btnEliminar');

        //Modales
        const modalRegistrarDep = $('#modalRegistrarDep');
        const modalTitulo = $('#modalTitulo');
        const modalInsumosSinCapturar = $('#modalInsumosSinCapturar');
        const modalCapturaPoliza = $('#modalCapturaPoliza');

        const leyendaTipoMovimiento = $('#leyendaTipoMovimiento');

        const btnGenerarPolizaCosto = $('#btnGenerarPolizaCosto');
        const btnRegistrarPoliza = $('#btnRegistrarPoliza');

        //Eventos
        modalInsumosSinCapturar.on('shown.bs.modal', function () {
            tblInsumosSinCapturar.DataTable().columns.adjust().draw();
        });

        modalCapturaPoliza.on('shown.bs.modal', function () {
            tblNuevaPoliza.DataTable().columns.adjust().draw();
        });

        btnGenerarPolizaCosto.on('click', function () {
            cambiarModal(modalInsumosSinCapturar, modalCapturaPoliza);
        });

        btnRegistrarPoliza.on('click', function () {
            RegistrarPolizaCosto();
        })

        btnActualizacionAutomatica.on('click', function () {
            RelacionAutomaticaPolizas();
        });

        $('table').on('click', '.btnIniciarRegistro', function () {
            btnModificar.hide();
            btnEliminar.hide();
            btnPolizasNuevas.hide();
            btnRegistrar.data('id_cat_maq', $(this).data('id_cat_maq'));
            btnRegistrar.show();

            modalTitulo.text('Registro de depreciación de # Económico/CC: ' + $(this).data('no_economico') + ' / ' + $(this).data('cc'));

            ObtenerPolizasCC($(this).data('no_economico'),);
        });

        $('table').on('click', '.btnIniciarModificacion', function () {
            btnModificar.show();
            btnEliminar.hide();
            btnPolizasNuevas.show();
            btnPolizasNuevas.prop('disabled', false);
            btnPolizasNuevas.data('id_cat_maq', $(this).data('id_cat_maq'));
            btnModificar.data('id_cat_maq', $(this).data('id_cat_maq'));
            btnModificar.data('id_dep_maq', $(this).data('id_dep_maq'));
            btnRegistrar.hide();

            modalTitulo.text('Modificar depreciación de # Económico/CC: ' + $(this).data('no_economico') + ' / ' + $(this).data('cc'));

            ObtenerDepMaquina($(this).data('id_cat_maq'));
        });

        $('table').on('click', '.btnIniciarEliminacion', function () {
            AlertaAceptarRechazarNormal(
                'Eliminar depreciación del # económico/cc: ' + $(this).data('no_economico') + ' / ' + $(this).data('cc'),
                '¿Está seguro de eliminar la información de depreciación?',
                () => EliminarDepMaquina($(this).data('id_cat_maq')));
        })

        $('.inputFecha').on('change', function () {
            if (!moment($(this).val(), 'DD/MM/YYYY').isValid()) {
                $(this).val('');
            }
        });

        $('#tblPolizasCC').on('keypress', '.txtPorcentajeDepreciacion', function (event) {
            aceptaSoloNumeroXDIntMax($(this), event, 3);
        });

        $('#tblPolizasCC').on('paste', '.txtPorcentajeDepreciacion', function (event) {
            permitePegarSoloNumeroXDIntMax($(this), event, 3);
        });

        $('#tblPolizasCC').on('keypress', '.txtMesesDeDepreciacion', function (event) {
            aceptaSoloNumeroXDIntMax($(this), event, 3);
        });

        $('#tblPolizasCC').on('paste', '.txtMesesDeDepreciacion', function (event) {
            permitePegarSoloNumeroXDIntMax($(this), event, 3);
        });

        $('#tblPolizasCC').on('click', '.quitarPoliza', function () {
            tblPolizasCC.DataTable().row($(this).closest('tr')).remove().draw(true);
            // table.row('.selected').remove().draw( false );
        });

        $('#tblPolizasCC').on('click', '.btnCostoInsumo', function () {
            console.log('btn');
            let id = tblPolizasCC.DataTable().row($(this).closest('tr')).data();
            GenerarPolizaCostoPorInsumo(id.IdCatMaquinaDepreciacion);
        });

        $('#tblPolizasCC').on('click', '.polizaCopy', function () {

        });

        btnRegistrar.on('click', function () {
            if (ValidarCampos()) {
                var objDepMaq = InitObjetoDepMaquina($(this).data('id_cat_maq'));
                if (!objDepMaq) {
                    return;
                }
                else {
                    RegistrarDepMaquina(objDepMaq);
                }
            }
            else {
                return;
            }
        });

        btnPolizasNuevas.on('click', function () {
            AgregarPolizas($(this).data('id_cat_maq'));
        });

        // btnAgregarPoliza.on('click', function() {
        //     var objPoliza = {
        //         Año: txtAgregarPoliza_Año.val(),
        //         Mes: txtAgregarPoliza_Mes.val(),
        //         Poliza: txtAgregarPoliza_Poliza.val(),
        //         TP: txtAgregarPoliza_Tp.val(),
        //         Linea: txtAgregarPoliza_Linea.val()
        //     }
        //     console.log(objPoliza);

        //     AgregarPoliza(objPoliza);
        // });

        btnModificar.on('click', function () {
            if (ValidarCampos()) {
                var objDepMaq = InitObjetoDepMaquina($(this).data('id_cat_maq'));
                if (!objDepMaq) {
                    return;
                }
                else {
                    ModificarDepMaquina(objDepMaq);
                }
            }
            else {
                return;
            }
        });

        modalRegistrarDep.on('hidden.bs.modal', function () {
            LimpiarModalRegistro(tblPolizasCC);
        });

        cboFiltroMaquinaria.on('change', function () {
            LimpiarModalRegistro(tblDepMaquinas);

            if ($(this).val() != '') {

                GetMaquinas($(this).val(), cboFiltroEstatusMaquina.val(), cboFiltroTipoCaptura.val());
            }
        });

        cboFiltroEstatusMaquina.on('change', function () {
            LimpiarModalRegistro(tblDepMaquinas);

            if (cboFiltroMaquinaria.val() != '') {
                GetMaquinas(cboFiltroMaquinaria.val(), $(this).val(), cboFiltroTipoCaptura.val());
            }
        });

        cboFiltroTipoCaptura.on('change', function () {
            LimpiarModalRegistro(tblDepMaquinas);

            if (cboFiltroMaquinaria.val() != '') {
                GetMaquinas(cboFiltroMaquinaria.val(), cboFiltroEstatusMaquina.val(), $(this).val());
            }
        });

        $('#tblDepMaquinas').on('click', '.btnFactura', function () {
            //btnAddSingle.data('data-idEconomico', idEconomico);
            menuConfig.parametros = {
                id: $(this).data('id')
            }
            mostrarMenu();
            // if (id == 0) {
            //     openModalAlta(tipoArchivo);
            // }
            // else {
            //     menuConfig.parametros = {
            //         id
            //     };
            //     mostrarMenu();
            //     // downloadURI(id);
            // }
        });

        //Variables
        let tiposMovimientos;

        const report = $('#report');

        const TiposMovimiento = {
            Cargo: 1,
            Abono: 2
        };

        //Inicializador
        (function init() {
            checkJS_VERSION();
            initComboBoxCuentasMaquinaria();
            InitFechas();
            InitTableDepMaquinas();
            iniTblInsumosSinCapturar();
            iniTblDetallePoliza();
            InitTableFacturasCC();
            GetTiposMovimiento();
            //GetMaquinas();
        })();

        //Inits
        function checkJS_VERSION() {
            let JS_VERSION = 2;
            if ($('#JS_VERSION').val() != JS_VERSION) {
                alert('ELIMINAR CACHE');
            }
        }

        menuConfig = {
            lstOptions: [
                { text: `<i class="fa fa-download"></i> Descargar`, action: "descargar", fn: parametros => { downloadURI(parametros.id) } }
                , { text: `<i class="fa fa-file"></i> Visor`, action: "visor", fn: parametros => { setVisor(parametros.id) } }
            ]
        }

        //Cbox
        function initComboBoxCuentasMaquinaria() {
            cboFiltroMaquinaria.fillCombo('/ActivoFijo/GetCuentas', { tipoResultado: TipoResultadoCuentasMaquinaria.CboxMaquinaria }, false, null);
        }

        //Fechas
        function InitFechas() {
        }

        //Tablas
        function InitTableDepMaquinas() {
            tblDepMaquinas.DataTable({
                order: [[8, 'desc'], [6, 'desc']],
                searching: true,
                paging: true,
                ordering: true,
                info: true,
                language: dtDicEsp,

                columns: [
                    { data: null, title: 'Opciones' },
                    { data: 'Factura', title: 'Factura' },
                    { data: 'CC', title: 'Centro de costo' },
                    { data: 'NoEconomico', title: 'Número económico', },
                    { data: 'Descripcion', title: 'Descripción' },
                    { data: 'AreaCuenta', title: 'Área cuenta' },
                    { data: 'DepreciacionCapturada', title: 'Depreciación capturada' },
                    { data: 'DepreciacionCapturada' },
                    { data: 'CapturaAutomatica', title: 'Actualizadas por usuario' }
                ],

                columnDefs: [
                    {
                        targets: [0],
                        render: function (data, type, row) {
                            var opcionRegistrar = '';
                            var opcionModificar = '';
                            var opcionEliminar = '';

                            if (!data.DepreciacionCapturada) {
                                var opcionRegistrar = '<button type="button" class="btn btn-success btnIniciarRegistro" data-id_cat_maq="' + data.Id + '" data-cc="' + data.CC + '" data-no_economico="' + data.NoEconomico + '"><i class="glyphicon glyphicon-plus"></i></button>';

                                return opcionRegistrar;
                            }
                            else {
                                var opcionModificar = '<button type="button" class="btn btn-primary btnIniciarModificacion" data-id_cat_maq="' + data.Id + '" data-cc="' + data.CC + '" data-no_economico="' + data.NoEconomico + '"><i class="glyphicon glyphicon-pencil"></i></button>';
                                var opcionEliminar = '<button type="button" class="btn btn-danger btnIniciarEliminacion" data-id_cat_maq="' + data.Id + '" data-cc="' + data.CC + '" data-no_economico="' + data.NoEconomico + '"><i class="glyphicon glyphicon-remove"></i></button>';

                                return opcionModificar + ' ' + opcionEliminar;
                            }
                        }
                    },
                    {
                        targets: [1],
                        render: function (data, type, row) {
                            if (data != null) {
                                return '<button class="btn btn-success btnFactura" data-tipo_archivo="1" data-id="' + data.id + '" data-id_economico="' + data.noEconomicoID + '")"><i class="glyphicon glyphicon-file"></i></button>';
                            }
                            else {
                                return '';
                            }
                        }
                    },
                    {
                        targets: [6],
                        render: function (data, type, row) {
                            if (type === 'display') {
                                if (data) {
                                    return '<i class="far fa-check-circle fa-2x"></i>';
                                }
                                else {
                                    return '<i class="far fa-times-circle fa-2x"></i>';
                                }
                            }
                            else {
                                return data;
                            }
                        }
                    },
                    {
                        targets: [7],
                        visible: false
                    },
                    {
                        targets: [8],
                        render: function (data, type, row) {
                            if (type === 'display') {
                                if (!row.DepreciacionCapturada) {
                                    return '<i class="far fa-times-circle fa-2x"></i>';
                                }
                                if (data) {
                                    return '<i class="far fa-times-circle fa-2x"></i>';
                                }
                                else {
                                    return '<i class="far fa-check-circle fa-2x"></i>';
                                }
                            }
                            else {
                                return data;
                                // if(!row.DepreciacionCapturada) {
                                //     return !data;
                                // }else{
                                //     return data;
                                // }
                            }
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) {
                    $(row).find('td').eq(0).addClass('text-center');
                    $(row).find('td').eq(1).addClass('text-center');
                    $(row).find('td').eq(2).addClass('text-center');
                    $(row).find('td').eq(3).addClass('text-center');
                    $(row).find('td').eq(5).addClass('text-left');
                    $(row).find('td').eq(6).addClass('text-center');
                    $(row).find('td').eq(7).addClass('text-center');
                    // if (data.COLUMNA) {
                    //     $(row).addClass('');
                    // }
                },

                drawCallback: function (settings) {
                },

                headerCallback: function (thead, data, start, end, display) {
                    $(thead).addClass('bg-table-header');
                    $(thead).find('th').addClass('text-center');
                    $(thead).find('th').eq(0).addClass('thOpciones');
                    $(thead).find('th').eq(1).addClass('thFactura');
                    $(thead).find('th').eq(2).addClass('thCC');
                    $(thead).find('th').eq(3).addClass('thNumEco');
                    $(thead).find('th').eq(6).addClass('thDepCapturada');
                    $(thead).find('th').eq(7).addClass('thCapSistema');
                    // $(thead).find('th').eq('4').html('');
                },

                footerCallback: function (tfoot, data, start, end, display) {
                }
            });
        }

        function InitTableFacturasCC() {
            tblPolizasCC.DataTable({
                searching: false,
                paging: false,
                ordering: false,
                info: false,
                language: dtDicEsp,

                columns: [
                    { data: 'TM', title: 'TM', className: 'dt-body-center' },
                    { data: null, title: 'Tipo del activo', className: 'dt-body-center' },
                    { data: 'Factura', title: 'Factura', className: 'dt-body-center' },
                    { data: null, title: 'Póliza', className: 'dt-body-nowrap dt-body-center' },
                    { data: null, title: 'Cuenta', className: 'dt-body-center dt-body-nowrap' },
                    { data: 'Monto', title: 'Monto', className: 'dt-body-right' },
                    { data: null, title: 'Tipo', className: 'dt-body-center' },
                    { data: 'FechaFactura', title: 'Fecha movimiento', className: 'dt-body-center' },
                    { data: null, title: 'Inicio depreciación', className: 'dt-body-center' },
                    { data: null, title: '% Depreciación', className: 'dt-body-center' },
                    { data: null, title: 'Meses a depreciar' },
                    { data: null, title: 'Poliza Alta' },
                    { data: 'Concepto', title: 'Concepto' },
                    { data: 'AreaCuenta', title: 'Área cuenta' }
                ],

                columnDefs: [
                    {
                        targets: [0],
                        render: function (data, type, row) {
                            let iconoNuevo = '';
                            if (row.CapturaPorSistema) {
                                iconoNuevo = '<span class="label label-info">Nuevo</span> ';
                            }
                            if (data === 1) {
                                if (row.TipoActivo == 3) {
                                    iconoNuevo = iconoNuevo + '<span class="label label-default">Unir <i class="far fa-trash-alt quitarPoliza"></i></span>';
                                } else if (row.TipoActivo == 4) {
                                    iconoNuevo = iconoNuevo + '<span class="label label-default">Quitar <i class="far fa-trash-alt quitarPoliza"></i></span>';
                                } else {
                                    iconoNuevo = iconoNuevo + '<span class="label label-success">Alta <i class="far fa-trash-alt quitarPoliza"></i></span>';
                                }
                                if (row.botonCosto) {
                                    return iconoNuevo + ' <span class="label label-default">Costo <i class="fas fa-caret-down btnCostoInsumo"></i></span>';
                                } else {
                                    return iconoNuevo;
                                }
                            }
                            if (data === 2) {
                                if (row.TipoActivo == 3) {
                                    iconoNuevo = iconoNuevo + '<span class="label label-default">Unir <i class="far fa-trash-alt quitarPoliza"></i></span>';
                                } else if (row.TipoActivo == 4) {
                                    iconoNuevo = iconoNuevo + '<span class="label label-default">Quitar <i class="far fa-trash-alt quitarPoliza"></i></span>';
                                } else {
                                    iconoNuevo = iconoNuevo + '<span class="label label-warning">Baja <i class="far fa-trash-alt quitarPoliza"></i></span>';
                                }
                                if (row.botonCosto) {
                                    return iconoNuevo + ' <span class="label label-default">Costo <i class="fas fa-caret-down btnCostoInsumo"></i></span>';
                                } else {
                                    return iconoNuevo;
                                }
                            }
                            if (data === 3) {
                                if (row.TipoActivo == 3) {
                                    iconoNuevo = iconoNuevo + '<span class="label label-default">Unir <i class="far fa-trash-alt quitarPoliza"></i></span>';
                                } else if (row.TipoActivo == 4) {
                                    iconoNuevo = iconoNuevo + '<span class="label label-default">Quitar <i class="far fa-trash-alt quitarPoliza"></i></span>';
                                } else {
                                    iconoNuevo = iconoNuevo + '<span class="label label-danger">Cancelación <i class="far fa-trash-alt quitarPoliza"></i></span>';
                                }
                                if (row.botonCosto) {
                                    return iconoNuevo + ' <span class="label label-default">Costo <i class="fas fa-caret-down btnCostoInsumo"></i></span>';
                                } else {
                                    return iconoNuevo;
                                }
                            }
                        }
                    },
                    {
                        targets: [1],
                        render: function (data, type, row) {
                            switch (data.TipoActivo) {
                                case 0:
                                    {
                                        var combo =
                                            '<select class="cboxTipoActivo form-control">' +
                                            '<option value="0" selected>--Seleccionar--</option>' +
                                            '<option value="1">Maquina</option>' +
                                            '<option value="2">Complemento</option>' +
                                            '<option value="3">Unir</option>' +
                                            '<option value="4">Quitar</option>' +
                                            '</select>';

                                        return combo;
                                    }
                                case 1:
                                    {
                                        var combo =
                                            '<select class="cboxTipoActivo form-control">' +
                                            '<option value="0">--Seleccionar--</option>' +
                                            '<option value="1" selected>Maquina</option>' +
                                            '<option value="2">Complemento</option>' +
                                            '<option value="3">Unir</option>' +
                                            '<option value="4">Quitar</option>' +
                                            '</select>';

                                        return combo;
                                    }
                                    break;
                                case 2:
                                    {
                                        var combo =
                                            '<select class="cboxTipoActivo form-control">' +
                                            '<option value="0">--Seleccionar--</option>' +
                                            '<option value="1">Maquina</option>' +
                                            '<option value="2" selected>Complemento</option>' +
                                            '<option value="3">Unir</option>' +
                                            '<option value="4">Quitar</option>' +
                                            '</select>';

                                        return combo;
                                    }
                                    break;
                                case 3:
                                    {
                                        var combo =
                                            '<select class="cboxTipoActivo form-control">' +
                                            '<option value="0">--Seleccionar--</option>' +
                                            '<option value="1" selected>Maquina</option>' +
                                            '<option value="2">Complemento</option>' +
                                            '<option value="3" selected>Unir</option>' +
                                            '<option value="4">Quitar</option>' +
                                            '</select>';

                                        return combo;
                                    }
                                    break;
                                default:
                                    {
                                        var combo =
                                            '<select class="cboxTipoActivo form-control">' +
                                            '<option value="0" selected>--Seleccionar--</option>' +
                                            '<option value="1">Maquina</option>' +
                                            '<option value="2">Complemento</option>' +
                                            '<option value="3">Unir</option>' +
                                            '<option value="4">Quitar</option>' +
                                            '</select>';

                                        return combo;
                                    }
                                    break;
                            }
                            // if (data.TM === 1) {
                            //     if (data.TipoActivo == 0 || data.TipoActivo == 1) {
                            //         return '<select class="cboxTipoActivo form-control">' +
                            //             '<option value="1" selected>Maquina</option>' +
                            //             '<option value="2">Complemento</option>' +
                            //             '</select>';
                            //     }
                            //     else {
                            //         return '<select class="cboxTipoActivo form-control">' +
                            //             '<option value="1">Maquina</option>' +
                            //             '<option value="2" selected>Complemento</option>' +
                            //             '</select>';
                            //     }
                            // }
                            // else {
                            //     return '';
                            // }
                        }
                    },
                    {
                        targets: [2],
                        render: function (data, type, row) {
                            let fac = data != null ? data : '';
                            return '<input type="text" class="form-control txtFactura" value="' + fac + '" />';
                        }
                    },
                    {
                        targets: [3],
                        render: function (data, type, row) {
                            return '<span class="polizaCopy">' + data.Año + '-' + data.Mes + '-' + data.Poliza + '-' + data.TP + '-' + data.Linea + '</span>';
                        }
                    },
                    {
                        targets: [4],
                        render: function (data, type, row) {
                            return data.Cuenta + '-' + data.Subcuenta + '-' + data.SubSubcuenta;
                        }
                    },
                    {
                        targets: [5],
                        render: function (data, type, row) {
                            if (data < 0) {
                                return '<span class="numeroRojo">' + maskNumero(data) + '</span>'
                            }
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [6],
                        render: function (data, type, row) {
                            return '<select class="form-control cboxTipoDelMovimiento">' +
                                '<option value="1" ' + ((data.TipoDelMovimiento == 1 || data.TipoDelMovimiento == 0) ? 'selected' : "") + '>A</option>' +
                                '<option value="2" ' + ((data.TipoDelMovimiento == 2) ? 'selected' : "") + '>B</option>' +
                                '<option value="3" ' + ((data.TipoDelMovimiento == 3) ? 'selected' : "") + '>C</option>' +
                                '<option value="4" ' + ((data.TipoDelMovimiento == 4) ? 'selected' : "") + '>F</option>' +
                                '<option value="5" ' + ((data.TipoDelMovimiento == 5) ? 'selected' : "") + '>O</option>' +
                                '</select>';
                        }
                    },
                    {
                        targets: [7],
                        render: function (data, type, row) {
                            var inputFechaPol = '';
                            if (moment(data).isValid()) {
                                inputFechaPol = '<input type="text" class="form-control txtFechaPol" value="' + moment(data).format('DD/MM/YYYY') + '" />';
                            } else {
                                inputFechaPol = '<input type="text" class="form-control txtFechaPol" value="' + moment(row.FechaPol).format('DD/MM/YYYY') + '" />';
                            }
                            return inputFechaPol;
                        }
                    },
                    {
                        targets: [8],
                        render: function (data, type, row) {
                            var inputFechaInicioDepreciacion = '';
                            inputFechaInicioDepreciacion = '<input type="text" class="form-control txtFechaInitDep" value="' + moment(Date.now()).format('DD/MM/YYYY') + '" />';

                            if (moment(row.FechaInicioDepreciacion, 'DD/MM/YYYY').isValid()) {
                                inputFechaInicioDepreciacion = '<input type="text" class="form-control txtFechaInitDep" value="' + moment(row.FechaInicioDepreciacion).format('DD/MM/YYYY') + '" />';
                            }

                            if (data.TM == 1 && data.TipoActivo != 3) {
                                return inputFechaInicioDepreciacion;
                            }
                            else {
                                return '<input type="text" class="form-control txtFechaInitDep" disabled />';
                            }
                        }
                    },
                    {
                        targets: [9],
                        render: function (data, type, row) {
                            if ((data.TM == 1 || data.TM == 3) && data.TipoActivo != 3) {
                                return '<input type="text" class="form-control txtPorcentajeDepreciacion" value="' + (data.PorcentajeDepreciacion * 100) + '" />'
                            }
                            else {
                                return '<input type="text" class="form-control txtPorcentajeDepreciacion" disabled />';
                            }
                        }
                    },
                    {
                        targets: [10],
                        render: function (data, type, row) {
                            if ((data.TM == 1 || data.TM == 3) && data.TipoActivo != 3) {
                                return '<input type="text" class="form-control txtMesesDeDepreciacion" value="' + (data.MesesTotalesDepreciacion == null ? 0 : data.MesesTotalesDepreciacion) + '" />';
                            }
                            else {
                                return '<input type="text" class="form-control txtMesesDeDepreciacion" disabled />';
                            }
                        }
                    },
                    {
                        targets: [11],
                        render: function (data, type, row) {
                            if (data.TM == 2 || data.TM == 3 || data.TipoActivo == 3) {
                                if (data.PolizaRefAlta != null) {
                                    return '<input type="text" class="form-control txtPolizaRelAlta" value="' + data.PolizaRefAlta + '" />';
                                }
                                else {
                                    return '<input type="text" class="form-control txtPolizaRelAlta" value="" />';
                                }
                            }
                            else {
                                return '<input type="text" class="form-control txtPolizaRelAlta" disabled />';
                            }
                        }
                    },
                    {
                        targets: [13],
                        render: function (data, type, row) {
                            if (data == '' || data == null) {
                                return '0-0';
                            }
                            else {
                                return data;
                            }
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) {
                    // if (data.COLUMNA) {
                    //     $(row).addClass('');
                    // }
                },

                drawCallback: function (settings) {
                    $('.txtFechaPol').datepicker({}).datepicker();
                    $('.txtFechaInitDep').datepicker({}).datepicker();

                    let spans = document.querySelectorAll('.polizaCopy');
                    for (const span of spans) {
                        span.onclick = function () {
                            document.execCommand('copy');
                        }

                        span.addEventListener('copy', function (event) {
                            event.preventDefault();
                            if (event.clipboardData) {
                                event.clipboardData.setData('text/plain', span.textContent);
                                console.log(event.clipboardData.getData('text'));
                            }
                        });
                    }
                },

                headerCallback: function (thead, data, start, end, display) {
                    $(thead).addClass('bg-table-header');
                    $(thead).find('th').addClass('text-center');
                    // $(thead).find('th').eq('4').html('');
                },

                footerCallback: function (tfoot, data, start, end, display) {
                    let totalMonto = 0;
                    data.forEach(function (element, index, array) {
                        totalMonto += element.Monto;
                    });

                    $(tfoot).find('th').eq(1).text(maskNumero(totalMonto));
                }
            });
        }

        function iniTblInsumosSinCapturar() {
            tblInsumosSinCapturar.DataTable({
                ordering: false,
                searching: true,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollY: '400px',
                scrollCollapse: true,
                scrollX: false,
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        text: 'Descargar en excel',
                        exportOptions: {
                            modifier: {
                                page: '_all'
                            }
                        }
                    }
                ],

                columns: [
                    { data: 'numEconomico', title: '# economico' },
                    { data: 'Poliza', title: 'Póliza' },
                    { data: 'Cuenta', title: 'Cuenta' },
                    { data: 'Subcuenta', title: 'Subcuenta' },
                    { data: 'SubSubcuenta', title: 'SubSubcuenta' },
                    { data: 'Monto', title: 'Monto' },
                    { data: 'Concepto', title: 'Concepto' }
                ],

                columnDefs: [
                    {
                        targets: [0, 1, 2, 3, 4],
                        className: 'dt-body-center'
                    },
                    {
                        targets: [5],
                        className: 'dt-body-right',
                        render: function (data, type, row) {
                            return maskNumero(row.Monto);
                        }
                    },
                    {
                        targets: [6],
                        className: 'dt-body-left'
                    }
                ],

                createdRow: function (row, data, dataIndex) {
                    // if (data.aCosto) {
                    //     $(row).css('background-color', 'yellow');
                    //   }
                },
                drawCallback: function (settings) { },
                headerCallback: function (thead, data, start, end, display) { },

                footerCallback: function (tfoot, data, start, end, display) {
                    let totalCargos = 0.0;

                    data.forEach(function (value, index, array) {
                        totalCargos += value.Monto;
                    });

                    $(tfoot).find('th').eq(1).text(maskNumero(totalCargos));
                },

                initComplete: function (settings, json) { }
            });
        }

        function iniTblDetallePoliza() {
            tblNuevaPoliza.DataTable({
                ordering: true,
                searching: true,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollY: '400px',
                scrollCollapse: true,
                scrollX: false,
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        text: 'Descargar en excel',
                        exportOptions: {
                            modifier: {
                                page: '_all'
                            }
                        }
                    }
                ],

                columns: [
                    { data: 'Linea', title: 'Línea' },
                    { data: 'Cta', title: 'Cuenta' },
                    { data: 'Scta', title: 'Subcuenta' },
                    { data: 'Sscta', title: 'SubSubcuenta' },
                    { data: 'CC', title: 'CC' },
                    { data: 'Referencia', title: 'Referencia' },
                    { data: 'Concepto', title: 'Concepto' },
                    { data: 'TM', title: 'Tipo del movimiento' },
                    { data: 'Cargos', title: 'Cargos' },
                    { data: 'Abonos', title: 'Abonos' },
                    { data: null, title: 'Área cuenta' }
                ],

                columnDefs: [
                    {
                        targets: [0, 1, 2, 3, 4, 5, 7, 10],
                        className: 'dt-body-center'
                    },
                    {
                        targets: [8],
                        className: 'dt-body-right',
                        render: function (data, type, row) {
                            if (row.TM == TiposMovimiento.Cargo) {
                                return maskNumero(row.Monto);
                            }
                            else {
                                return maskNumero(0.0);
                            }
                        }
                    },
                    {
                        targets: [9],
                        className: 'dt-body-right',
                        render: function (data, type, row) {
                            if (row.TM == TiposMovimiento.Abono) {
                                return maskNumero(row.Monto);
                            }
                            else {
                                return maskNumero(0.0);
                            }
                        }
                    },
                    {
                        targets: [10],
                        render: function (data, type, row) {
                            return row.Area + '-' + row.Cuenta_OC;
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) { },
                drawCallback: function (settings) { },
                headerCallback: function (thead, data, start, end, display) { },

                footerCallback: function (tfoot, data, start, end, display) {
                    let totalCargos = 0.0;
                    let totalAbonos = 0.0;

                    data.forEach(function (value, index, array) {
                        totalCargos += value.TM == TiposMovimiento.Cargo ? value.Monto : 0.0;
                        totalAbonos += value.TM == TiposMovimiento.Abono ? value.Monto : 0.0;
                    });

                    $(tfoot).find('th').eq(1).text(maskNumero(totalCargos));
                    $(tfoot).find('th').eq(2).text(maskNumero(totalAbonos));
                },

                initComplete: function (settings, json) { }
            });
        }

        function AddRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function AddRowsWithoutClear(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.rows.add(datos).draw();
        }

        function ValidarCampos() {
            var valido = true;

            var datosDT = $('#tblPolizasCC').DataTable().rows().data();
            $('#tblPolizasCC').find('tbody').find('tr').each(function (index, value) {
                switch (datosDT[index].TM) {
                    case 1:
                        if (!validarCampo($(this).find('.txtFechaInitDep')) && !$(this).find('.txtPolizaRelAlta').prop('disabled')) { valido = false; }
                        if (!validarCampo($(this).find('.txtPorcentajeDepreciacion')) && !$(this).find('.txtPolizaRelAlta').prop('disabled')) { valido = false; }
                        if (!validarCampo($(this).find('.txtMesesDeDepreciacion')) && !$(this).find('.txtPolizaRelAlta').prop('disabled')) { valido = false; }
                    case 2:
                        if (!validarCampo($(this).find('.txtPolizaRelAlta')) && !$(this).find('.txtPolizaRelAlta').prop('disabled')) { valido = false; }
                    case 3:
                    // if (!validarCampo($(this).find('.txtPolizaRelAlta')) && !$(this).find('.txtPolizaRelAlta').prop('disabled')) { valido = false; }
                    default:
                        if (!validarCampo($(this).find('.txtFechaPol'))) { valido = false; }
                        break;
                }
            });
            return valido;
        }

        function InitInputsDepMaquina(informacion) {
        }

        function downloadURI(elemento) {
            var link = document.createElement("button");
            link.download = '/CatInventario/getFileDownload?id=' + elemento;
            link.href = '/CatInventario/getFileDownload?id=' + elemento;
            link.click();
            location.href = '/CatInventario/getFileDownload?id=' + elemento;
        }

        const getFileRuta = new URL(window.location.origin + '/CatInventario/getFileRuta');
        async function setVisor(id) {
            try {
                response = await ejectFetchJson(getFileRuta, { id });
                if (response.success) {
                    $('#myModal').data().ruta = response.ruta;
                    $('#myModal').modal('show');
                }
            } catch (o_O) { }
        }

        function reporteCaptura(reporte) {
            $.blockUI({ message: 'Procesando...' });
            var path = '/Reportes/Vista.aspx?' +
                'esDescargaVisor=' + false +
                '&esVisor=' + true +
                '&idReporte=' + 64 + // 188 pruebas, 64 producción
                '&isResumen=' + reporte.ReporteResumido +
                '&isCC=' + reporte.EsCC +
                '&isPorHoja=' + reporte.PolizaPorHoja +
                '&isFirma=' + reporte.IncluirFirmas +
                '&Estatus=' + reporte.Estatus +
                '&icc=' + reporte.CCInicial +
                '&fcc=' + reporte.CCFinal +
                '&iPol=' + reporte.PolizaInicial +
                '&fPol=' + reporte.PolizaFinal +
                '&iPer=' + moment(reporte.PeriodoInicial).format('MM/YYYY') +
                '&fPer=' + moment(reporte.PeriodoFinal).format('MM/YYYY') +
                '&iTp=' + reporte.TipoPolizaInicial +
                '&fTp=' + reporte.TipoPolizaFinal +
                '&firma1=' + reporte.Reviso +
                '&firma2=' + reporte.Autorizo;
            report.attr('src', path);
            document.getElementById('report').onload = function () {
                $('#myModal').modal('show');
                //$.unblockUI();
                //openCRModal();
            };
        }

        function cambiarModal(modalCerrar, modalAbrir) {
            modalCerrar.on('hidden.bs.modal', function () {
                modalAbrir.modal('show');
            });

            modalCerrar.modal('hide');
        }

        //Crear objetos
        function InitObjetoDepMaquina(idMaquina) {
            objInfoDep = new Array();
            var datosDT = $('#tblPolizasCC').DataTable().rows().data();
            $('#tblPolizasCC').find('tbody').find('tr').each(function (index, value) {
                objDepMaquina = {
                    IdCatMaquinaDepreciacion: datosDT[index].IdCatMaquinaDepreciacion,
                    Año: datosDT[index].Año,
                    Mes: datosDT[index].Mes,
                    Poliza: datosDT[index].Poliza,
                    TP: datosDT[index].TP,
                    Linea: datosDT[index].Linea,
                    TM: datosDT[index].TM,
                    Cuenta: datosDT[index].Cuenta,
                    Factura: $(value).find('.txtFactura').val(),
                    FechaMovimiento: moment($(value).find('.txtFechaPol').val(), 'DD/MM/YYYY').toISOString(true),
                    TipoActivo: $(value).find('.cboxTipoActivo').val(),
                    IdCatMaquina: idMaquina,
                    FechaInicioDepreciacion: datosDT[index].TM === 1 && datosDT[index].TipoActivo != 3 ? moment($(value).find('.txtFechaInitDep').val(), 'DD/MM/YYYY').toISOString(true) : null,
                    PorcentajeDepreciacion: (datosDT[index].TM === 1 || datosDT[index].TM === 3) && datosDT[index].TipoActivo != 3 ? $(value).find('.txtPorcentajeDepreciacion').val() : null,
                    MesesTotalesDepreciacion: (datosDT[index].TM === 1 || datosDT[index].TM === 3) && datosDT[index].TipoActivo != 3 ? $(value).find('.txtMesesDeDepreciacion').val() : null,
                    TipoDelMovimiento: $(value).find('.cboxTipoDelMovimiento').val(),
                    PolizaRefAlta: (datosDT[index].TM === 2 || datosDT[index].TM === 3) || (datosDT[index].TM === 1 && datosDT[index].TipoActivo == 3) ? $(value).find('.txtPolizaRelAlta').val() : null,
                    Concepto: datosDT[index].Concepto,
                    Costo: datosDT[index].botonCosto
                }

                objInfoDep.push(objDepMaquina);
            });

            for (let index = 0; index < objInfoDep.length; index++) {
                let existe = false;
                if ((objInfoDep[index].TM == 2 || objInfoDep[index].TM == 3) && objInfoDep[index].TipoActivo != 3) {
                    for (let index2 = 0; index2 < objInfoDep.length; index2++) {
                        if (objInfoDep[index].PolizaRefAlta ==
                            (
                                objInfoDep[index2].Año + '-' + objInfoDep[index2].Mes + '-' + objInfoDep[index2].Poliza + '-' +
                                objInfoDep[index2].TP + '-' + objInfoDep[index2].Linea) && objInfoDep[index2].TM == 1
                        ) {
                            existe = true;
                        }
                    }
                    // if (!existe) {
                    //     AlertaGeneral('Alerta', 'No hay una poliza de referencia de Alta para la poliza: ' + objInfoDep[index].Año + '-' + objInfoDep[index].Mes + '-' + objInfoDep[index].Poliza + '-' + objInfoDep[index].TP + '-' + objInfoDep[index].Linea);
                    //     return false;
                    // }
                }
            }

            for (let index = 0; index < objInfoDep.length; index++) {
                let existe = false;
                if (objInfoDep[index].TipoActivo == 3) {
                    for (let index2 = 0; index2 < objInfoDep.length; index2++) {
                        if (objInfoDep[index].PolizaRefAlta ==
                            (
                                objInfoDep[index2].Año + '-' + objInfoDep[index2].Mes + '-' + objInfoDep[index2].Poliza + '-' +
                                objInfoDep[index2].TP + '-' + objInfoDep[index2].Linea)
                        ) {
                            existe = true;
                        }
                    }
                    if (!existe) {
                        AlertaGeneral('Alerta', 'No hay una poliza de referencia de unión para la poliza: ' + objInfoDep[index].Año + '-' + objInfoDep[index].Mes + '-' + objInfoDep[index].Poliza + '-' + objInfoDep[index].TP + '-' + objInfoDep[index].Linea);
                        return false;
                    }
                }
            }

            let CostosConBaja = false;
            for (let index = 0; index < objInfoDep.length; index++) {
                if (objInfoDep[index].Costo) {
                    CostosConBaja = true;
                    for (let index2 = 0; index2 < objInfoDep.length; index2++) {
                        if (objInfoDep[index2].PolizaRefAlta == (objInfoDep[index].Año + '-' + objInfoDep[index].Mes + '-' + objInfoDep[index].Poliza + '-' + objInfoDep[index].TP + '-' + objInfoDep[index].Linea)) {
                            CostosConBaja = false;
                            break;
                        }
                    }
                }
            }

            if (CostosConBaja) {
                AlertaGeneral('Alerta', 'Necesita mandar a costo los equipos indicados como "COSTO" y relacionar la baja con el insumo que se fue a costo');
                return false;
            }

            return objInfoDep;
        }

        function crearLeyendaTipoMovimiento() {
            tiposMovimientos.forEach(element => {
                let text = element.Text;
                let descripcion = element.Prefijo;

                leyendaTipoMovimiento.append('<li><span class="leyendaId">' + text + '</span> ' + descripcion + '</li>');
            });
        }

        function LimpiarModalRegistro(tabla) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
        }

        //Llamadas al servidor
        function GetMaquinas(idCuenta, estatusMaquina, tipoCaptura) {
            $.get('/ActivoFijo/GetMaquinas', {
                idCuenta: idCuenta,
                estatusMaquina: estatusMaquina,
                tipoCaptura: tipoCaptura
            }).always().then(response => {
                if (response.success) {
                    AddRows(tblDepMaquinas, response.items);
                }
                else {
                    alert('ERROR: ' + response.message);
                }
            }, error => {
                alert('ERROR DEL SERVIDOR: ' + response.message);
            });
        }

        function RegistrarDepMaquina(objDepMaquina, idCuenta) {
            $.post('/ActivoFijo/RegistrarDepMaquina', {
                depMaquina: objDepMaquina
            }).always().then(response => {
                if (response.success) {
                    $(modalRegistrarDep).modal('hide');

                    // cboFiltroMaquinaria.on('change', function() {
                    //     LimpiarModalRegistro(tblDepMaquinas);

                    //     if($(this).val() != '') {
                    //         GetMaquinas($(this).val());
                    //     }
                    // });

                    GetMaquinas(cboFiltroMaquinaria.val(), cboFiltroEstatusMaquina.val(), cboFiltroTipoCaptura.val());
                }
                else {
                    alert('ERROR: ' + response.message);
                }
            }, error => {
                alert('ERROR DEL SERVIDOR: ' + response.message);
            });
        }

        function ObtenerDepMaquina(idDepMaquina) {
            $.get('/ActivoFijo/ObtenerDepMaquina', {
                idDepMaquina: idDepMaquina
            }).always().then(response => {
                if (response.success) {
                    AddRows(tblPolizasCC, response.items);
                    $(modalRegistrarDep).modal('show');
                }
                else {
                    alert('ERROR: ' + response.message);
                }
            }, error => {
                alert('ERROR DEL SERVIDOR: ' + response.message);
            });
        }

        // function AgregarPoliza(infoPoliza) {
        //     $.get('/ActivoFijo/AgregarPoliza', {
        //         infoPoliza: infoPoliza
        //     }).always().then(response => {
        //         if(response.success){
        //             AddRowsWithoutClear(tblPolizasCC, response.items);
        //         }
        //         else {
        //             alert(response.message);
        //         }
        //     },error => {
        //         alert('ERROR DEL SERVIDOR: ' + error.statusText);
        //     });
        // }

        function AgregarPolizas(idMaquina) {
            $.get('/ActivoFijo/AgregarPolizas', {
                idMaquina: idMaquina
            }).always().then(response => {
                if (response.success) {
                    btnPolizasNuevas.prop('disabled', true);
                    AddRowsWithoutClear(tblPolizasCC, response.items);

                    let rowData = tblPolizasCC.DataTable().rows().data();

                    $.each(response.idsACosto, function (index, valueCosto) {
                        $.each(rowData, function (index, valueTbl) {
                            if (valueCosto == valueTbl.IdCatMaquinaDepreciacion) {
                                valueTbl.botonCosto = true;
                                console.log(valueTbl);
                            }
                        });
                    });

                    AddRows(tblPolizasCC, rowData);

                    console.log(response.idsACosto);
                }
                else {
                    alert('ERROR: ' + response.message);
                }
            }, error => {
                alert('ERROR DEL SERVIDOR: ' + response.message);
            });
        }

        function ModificarDepMaquina(objDepMaquina) {
            $.post('/ActivoFijo/ModificarDepMaquina', {
                depMaquina: objDepMaquina
            }).always().then(response => {
                if (response.success) {
                    $(modalRegistrarDep).modal('hide');
                    GetMaquinas(cboFiltroMaquinaria.val(), cboFiltroEstatusMaquina.val(), cboFiltroTipoCaptura.val());
                }
                else {
                    alert('ERROR: ' + response.message);
                }
            }, error => {
                alert('ERROR DEL SERVIDOR: ' + response.message);
            });
        }

        function EliminarDepMaquina(idDepMaquina) {
            $.post('/ActivoFijo/EliminarDepMaquina', {
                idDepMaquina: idDepMaquina
            }).always().then(response => {
                if (response.success) {
                    GetMaquinas(cboFiltroMaquinaria.val(), cboFiltroEstatusMaquina.val(), cboFiltroTipoCaptura.val());
                }
                else {
                    alert('ERROR: ' + response.message);
                }
            }, error => {
                alert('ERROR DEL SERVIDOR: ' + response.message);
            });
        }

        function RelacionAutomaticaPolizas() {
            $.post('/ActivoFijo/RelacionAutomaticaPolizas').always().then(response => {
                if (response.success) {
                    if (response.seGeneroPolizaCosto) {
                        AddRows(tblInsumosSinCapturar, response.lstRegistrosConDuplicados);
                        AddRows(tblNuevaPoliza, response.polizaGeneradaCostos.Detalle);

                        modalInsumosSinCapturar.modal('show');
                        //modalCapturaPoliza.modal('show');
                    } else {
                        ConfirmacionGeneral('Confirmación', '¡Operación realizada con exito!');
                    }
                }
                else {
                    alert(response.message);
                }
            }, error => {
                alert('ERROR DEL SERVIDOR: ' + error.statusText);
            });
        }

        function ObtenerPolizasCC(cc) {
            $.get('/ActivoFijo/ObtenerPolizasCC', {
                Cc: cc
            }).always().then(response => {
                if (response.success) {
                    AddRows(tblPolizasCC, response.items);
                    modalRegistrarDep.modal('show');
                }
                else {
                    // @Html.Raw(ViewBag.ViewerCSS)
                    // @Html.Raw(ViewBag.ViewerScripts)
                    AlertaGeneral('Alerta', response.message);
                }
            }, error => {
                alert('ERROR DEL SERVIDOR: ' + response.message);
            });
        }

        function GetTiposMovimiento() {
            $.post('/ActivoFijo/GetTiposMovimiento', {
            }).always().then(response => {
                if (response.success) {
                    tiposMovimientos = response.items;
                    crearLeyendaTipoMovimiento();
                }
                else {
                    alert('ERROR: ', response.message);
                }
            }, error => {
                alert('ERROR DEL SERVIDOR: ' + error.statusText);
            });
        }

        function GenerarPolizaCostoPorInsumo(idCatMaqDepreciacion) {
            console.log(idCatMaqDepreciacion);
            $.get('/ActivoFijo/GenerarPolizaCostoPorInsumo',
                {
                    idCatMaqDepreciacion: idCatMaqDepreciacion
                }).then(response => {
                    if (response.Success) {
                        AddRows(tblNuevaPoliza, response.Value.Detalle);
                        modalCapturaPoliza.modal('show');
                    } else {
                        AlertaGeneral('Alerta', response.Message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
        }

        function RegistrarPolizaCosto() {
            $.post('/ActivoFijo/RegistrarPolizaCosto').then(response => {
                if (response.success) {
                    modalCapturaPoliza.modal('hide');
                    if (response.esIndividual) {
                        AddRowsWithoutClear(tblPolizasCC, response.nuevosRegistros);
                    }
                    AlertaAceptarRechazarNormal(
                        'Confirmación',
                        '!Se registró correctamente la póliza: ' + response.Value.Poliza + ' ¿Desea ver la póliza?',
                        () => reporteCaptura(response.Value)
                    );
                } else {
                    AlertaGeneral('Alerta', response.message);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            });
        }
    }

    $(document).ready(() => {
        Maquinaria.ActivoFijo = new DepreciacionMaquinas();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }) })
        .ajaxStop(() => { $.unblockUI(); });
})();