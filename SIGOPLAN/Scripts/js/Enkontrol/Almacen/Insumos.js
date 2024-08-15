(() => {
    $.namespace('Enkontrol.Almacen.Almacen.Insumos');
    Insumos = function () {
        //#region Selectores
        //#region Filtros
        const fieldsetFiltros = $('#fieldsetFiltros');
        const inputFiltroInsumo = $('#inputFiltroInsumo');
        const inputFiltroInsumoDesc = $('#inputFiltroInsumoDesc');
        // const inputFiltroModeloMaquinaria = $('#inputFiltroModeloMaquinaria');
        const inputFiltroModeloMaquinariaDesc = $('#inputFiltroModeloMaquinariaDesc');
        const inputFiltroUnidadDesc = $('#inputFiltroUnidadDesc');
        const inputFiltroTipoInsumo = $('#inputFiltroTipoInsumo');
        const inputFiltroTipoInsumoDesc = $('#inputFiltroTipoInsumoDesc');
        const inputFiltroGrupoInsumo = $('#inputFiltroGrupoInsumo');
        const inputFiltroGrupoInsumoDesc = $('#inputFiltroGrupoInsumoDesc');
        const inputFiltroTolerancia = $('#inputFiltroTolerancia');
        const inputFiltroFechaAlta = $('#inputFiltroFechaAlta');
        const btnQuitarFiltros = $('#btnQuitarFiltros');
        const btnCargarExcel = $('#btnCargarExcel');
        const mdlCargarExcel = $('#mdlCargarExcel');
        const btnGuardarExcel = $('#btnGuardarExcel');
        const inputFileExcel = $('#inputFileExcel');
        const btnBuscarInsumos = $('#btnBuscarInsumos');
        //#endregion

        //#region Información
        const fieldsetInformacion = $('#fieldsetInformacion');
        const inputInsumo = $('#inputInsumo');
        const inputTipoInsumo = $('#inputTipoInsumo');
        const inputTipoInsumoDesc = $('#inputTipoInsumoDesc');
        const selectTipoInsumo = $('#selectTipoInsumo');
        const inputGrupoInsumo = $('#inputGrupoInsumo');
        const inputGrupoInsumoDesc = $('#inputGrupoInsumoDesc');
        // const inputModeloMaquinaria = $('#inputModeloMaquinaria');
        const inputModeloMaquinariaDesc = $('#inputModeloMaquinariaDesc');
        const inputInsumoDesc = $('#inputInsumoDesc');
        const inputUnidad = $('#inputUnidad');
        const radioVigente = $('#radioVigente');
        const radioPassword = $('#radioPassword');
        const radioCancelado = $('#radioCancelado');
        const inputTolerancia = $('#inputTolerancia');
        const checkBoxProductoTerminado = $('#checkBoxProductoTerminado');
        const checkBoxMateriaPrima = $('#checkBoxMateriaPrima');
        const checkBoxSeFactura = $('#checkBoxSeFactura');
        const checkBoxListaPrecios = $('#checkBoxListaPrecios');
        const inputAutorizado = $('#inputAutorizado');
        const selectColorResguardo = $('#selectColorResguardo');
        const textAreaDescripcionAdicional = $('#textAreaDescripcionAdicional');
        const divRadioEstatusGeneral = $('#divRadioEstatusGeneral');
        const labelInsumoDisponible = $('#labelInsumoDisponible');
        const checkBoxComprasRequisiciones = $('#checkBoxComprasRequisiciones');
        //#endregion

        const tblInsumos = $('#tblInsumos');
        const botonGuardarInsumo = $('#botonGuardarInsumo');
        const btnImprimir = $('#btnImprimir');
        const btnNuevoInsumo = $('#btnNuevoInsumo');

        const empresaActual = $('#empresaActual');
        const selectUnidadPeru = $('#selectUnidadPeru');
        const selectEstadoPeru = $('#selectEstadoPeru');
        const inputCodigo2Peru = $('#inputCodigo2Peru');

        const modalInsumo = $('#modalInsumo');
        //#endregion

        _empresaActual = empresaActual.val();

        (function init() {
            // initTableInsumos();

            if (_empresaActual != 6) {
                // cargarInsumos();

                inputUnidad.getAutocomplete(setUnidad, null, '/Enkontrol/Almacen/GetUnidades');
            } else {
                //SOLO PERU
                // btnNuevoInsumo.attr('disabled', true);
                // btnImprimir.attr('disabled', true);
                // btnGuardar.attr('disabled', true);
                $('.elementoPeru').hide();

                // inputTipoInsumo.on('change', function () {
                //     axios.post('/Enkontrol/Almacen/GetTipoInsumoPeru', { tipo: +inputTipoInsumo.val() }).then(response => {
                //         let { success, data, message } = response.data;

                //         if (success) {
                //             inputTipoInsumoDesc.val(response.data.tipoDesc);
                //         } else {
                //             AlertaGeneral(`Alerta`, message);
                //         }
                //     }).catch(error => AlertaGeneral(`Alerta`, error.message));
                // });

                selectTipoInsumo.fillCombo('/Enkontrol/Almacen/FillComboTipoInsumoPeru', null, false, null);
                selectUnidadPeru.fillCombo('/Enkontrol/Almacen/FillComboUnidadPeru', null, false, null);
            }

            btnQuitarFiltros.click(limpiarFiltros);
            botonGuardarInsumo.click(guardarNuevoInsumo);

            agregarTooltip(btnQuitarFiltros, 'Quitar Filtros');
            inputFiltroFechaAlta.datepicker().datepicker();
        })();

        btnNuevoInsumo.on('click', function () {
            let datos = tblInsumos.DataTable().rows().data();
            let dataUltimoInsumo = datos[datos.length - 1];

            if (_empresaActual != 6) {
                if (dataUltimoInsumo.insumo > 0) {
                    // datos.push({
                    //     'insumo': 0,
                    //     'insumoDesc': '',
                    //     // 'modeloMaquinaria': 0,
                    //     'modeloMaquinariaDesc': '',
                    //     'unidadDesc': '',
                    //     'tipo': 0,
                    //     'tipoDesc': '',
                    //     'grupo': 0,
                    //     'grupoDesc': '',
                    //     'tolerancia': 0
                    // });

                    // tblInsumos.DataTable().clear();
                    // tblInsumos.DataTable().rows.add(datos).draw();
                    // tblInsumos.DataTable().page('last').draw('page');

                    // tblInsumos.DataTable().$('tr.selected').removeClass('selected');
                    // tblInsumos.find('tbody tr:last').addClass("selected");

                    limpiarInformacion();

                    radioVigente.attr('checked', true);
                    checkBoxProductoTerminado.prop('checked', true);
                    checkBoxMateriaPrima.prop('checked', true);
                    checkBoxComprasRequisiciones.prop('checked', true);
                } else {
                    // tblInsumos.DataTable().page('last').draw('page');
                    // tblInsumos.DataTable().$('tr.selected').removeClass('selected');
                    // tblInsumos.find('tbody tr:last').addClass("selected");
                }
            } else {
                if (+dataUltimoInsumo.PERU_insumo > 0) {
                    // datos.push({
                    //     'insumo': 0,
                    //     'PERU_insumo': '',
                    //     'insumoDesc': '',
                    //     'modeloMaquinariaDesc': '',
                    //     'unidadDesc': '',
                    //     'tipo': 0,
                    //     'tipoDesc': '',
                    //     'grupo': 0,
                    //     'PERU_grupo': '',
                    //     'grupoDesc': '',
                    //     'tolerancia': 0
                    // });

                    // tblInsumos.DataTable().clear();
                    // tblInsumos.DataTable().rows.add(datos).draw();
                    // tblInsumos.DataTable().page('last').draw('page');

                    // tblInsumos.DataTable().$('tr.selected').removeClass('selected');
                    // tblInsumos.find('tbody tr:last').addClass("selected");

                    limpiarInformacion();

                    radioVigente.attr('checked', true);
                    checkBoxProductoTerminado.prop('checked', true);
                    checkBoxMateriaPrima.prop('checked', true);
                    checkBoxComprasRequisiciones.prop('checked', true);
                } else {
                    // tblInsumos.DataTable().page('last').draw('page');
                    // tblInsumos.DataTable().$('tr.selected').removeClass('selected');
                    // tblInsumos.find('tbody tr:last').addClass("selected");
                }
            }

            modalInsumo.modal('show');
        });

        inputInsumo.on('change', function () {
            let valor = inputInsumo.val().split('-').join('');

            if (_empresaActual != 6) {
                if (!isNaN(valor) && (valor.toString().length <= 7)) {
                    if (valor.length != 2) {
                        let tipo = !isNaN(parseInt(valor.substring(0, 1))) ? parseInt(valor.substring(0, 1)) : 0;
                        let grupo = !isNaN(parseInt(valor.substring(1, 3))) ? parseInt(valor.substring(1, 3)) : 0;

                        inputTipoInsumo.val(tipo > 0 ? tipo : '');
                        inputGrupoInsumo.val(grupo > 0 ? grupo : '');

                        getTipoGrupo(tipo, grupo);
                    } else {
                        inputGrupoInsumo.val('');
                        labelInsumoDisponible.text('Disponible: ');
                    }
                } else {
                    AlertaGeneral(`Alerta`, `Número de Insumo no válido.`);
                }
            } else {
                if (!isNaN(valor) && (valor.length == 6)) {
                    inputGrupoInsumo.val(valor.substring(2));

                    getTipoGrupo(0, +valor);
                } else {
                    AlertaGeneral(`Alerta`, `Número no válido. Debe colocar los primeros seis dígitos del insumo para consultar el siguiente consecutivo disponible.`);
                    selectTipoInsumo.val('');
                    inputGrupoInsumo.val('');
                    inputGrupoInsumoDesc.val('');
                    labelInsumoDisponible.text('Disponible: ');
                }
            }
        });

        inputInsumoDesc.on('keyup', function () {
            $(this).val($(this).val().toUpperCase());
        });

        inputUnidad.on('keyup', function () {
            $(this).val($(this).val().toUpperCase());
        });

        // inputModeloMaquinaria.on('change', function () {

        // });

        fieldsetFiltros.on('keyup change', 'input', function () {
            // tblInsumos.DataTable().search('');
            // tblInsumos.DataTable().column($(this).data('columnIndex')).search(this.value).draw();
        });

        btnCargarExcel.on('click', function () {
            mdlCargarExcel.modal('show');
        });

        btnImprimir.on('click', function () {
            $.blockUI({ message: 'Procesando...' });

            $(this).download = '/Enkontrol/Almacen/CrearExcelInsumos';
            $(this).href = '/Enkontrol/Almacen/CrearExcelInsumos';

            location.href = '/Enkontrol/Almacen/CrearExcelInsumos';

            $.unblockUI();
        });

        btnGuardarExcel.on('click', function () {
            try {
                var request = new XMLHttpRequest();

                $.blockUI({ message: 'Procesando...', baseZ: 2000 });

                request.open("POST", "/Enkontrol/Almacen/CargarExcel");
                request.send(formData());

                request.onload = function (response) {
                    if (request.status == 200) {
                        $.unblockUI();

                        let respuesta = JSON.parse(request.response);

                        if (respuesta.success) {
                            mdlCargarExcel.modal('hide');
                            // cargarInsumos();
                            initTableInsumos();
                            AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        } else {
                            AlertaGeneral(`Alerta`, `No se ha guardado la información. ${respuesta.message}`);
                        }
                    } else {
                        $.unblockUI();
                        AlertaGeneral(`Alerta`, `Error al guardar la información.`);
                    }
                };
            } catch {
                $.unblockUI();
            }
        });

        btnBuscarInsumos.on("click", function () {
            // cargarInsumos();
            initTableInsumos();
        });

        function initTableInsumos() {
            let filtros = {
                insumo: +inputFiltroInsumo.val(),
                insumoDesc: inputFiltroInsumoDesc.val(),
                modeloMaquinariaDesc: inputFiltroModeloMaquinariaDesc.val(),
                unidadDesc: inputFiltroUnidadDesc.val(),
                tipo: +inputFiltroTipoInsumo.val(),
                tipoDesc: inputFiltroTipoInsumoDesc.val(),
                grupo: +inputFiltroGrupoInsumo.val(),
                grupoDesc: inputFiltroGrupoInsumoDesc.val(),
                tolerancia: +inputFiltroTolerancia.val()
            };

            if ($.fn.DataTable.isDataTable('#tblInsumos')) {
                tblInsumos.DataTable().clear().destroy();
            }

            tblInsumos.DataTable({
                destroy: true,
                retrieve: true,
                bLengthChange: false,
                deferRender: true,
                // searching: false,
                // dom: 'lrtp',
                language: dtDicEsp,
                bInfo: false,
                ordering: false,

                processing: true,
                serverSide: true,
                bServerSide: true,
                sAjaxSource: '/Enkontrol/Almacen/GetInsumosCatalogo',
                fnServerData: function (sSource, aoData, fnCallback) {
                    aoData.push({ name: 'sSearchValues', value: JSON.stringify(filtros) });

                    $.ajax({ type: "Post", data: aoData, url: sSource, success: fnCallback });
                },
                initComplete: function (settings, json) {
                    tblInsumos.on('click', '.botonEditarInsumo', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblInsumos.DataTable().row(row).data();

                        // if (row.hasClass('selected')) {
                        //     row.removeClass('selected');
                        // } else {
                        //     tblInsumos.DataTable().$('tr.selected').removeClass('selected');
                        //     row.addClass('selected');
                        // }

                        getInformacionInsumo(row, rowData);
                    });
                },
                columns: [
                    {
                        data: 'insumo', title: 'Insumo', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                if (_empresaActual != 6) {
                                    let tipo = row.tipo.toString();
                                    let grupo = row.grupo.toString().length > 1 ? row.grupo.toString() : '0' + row.grupo.toString();
                                    let numero = row.insumo.toString();

                                    return tipo + '-' + grupo + '-' + (numero.substring(3).length > 0 ? numero.substring(3) : '0000');
                                } else {
                                    return row.PERU_insumo;
                                }

                            } else {
                                return _empresaActual == 6 ? row.PERU_insumo : row.insumo;
                            }
                        }
                    },
                    { data: 'insumoDesc', title: 'Descripción del Insumo' },
                    // {
                    //     data: 'modeloMaquinaria', title: 'Id Mod Maqui', render: function (data, type, row, meta) {
                    //         return data > 0 ? data : '';
                    //     }
                    // },
                    { data: 'modeloMaquinariaDesc', title: 'Modelo Maquinaria' },
                    { data: 'unidadDesc', title: 'Unidad' },
                    { data: 'tipo', title: 'Tipo' },
                    { data: 'tipoDesc', title: 'Descripción del Tipo' },
                    {
                        data: 'grupo', title: 'Grupo', render: function (data, type, row, meta) {
                            return _empresaActual == 6 ? row.PERU_grupo : data;
                        }
                    },
                    { data: 'grupoDesc', title: 'Descripción del Grupo' },
                    { data: 'tolerancia', title: 'Tolerancia' },
                    {
                        data: 'fechaAlta', title: 'Fecha Alta', render: function (data, type, row, meta) {
                            if (data != null) {
                                return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                            } else {
                                return '';
                            }
                        }
                    },
                    {
                        title: '', render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-warning botonEditarInsumo"><i class="fa fa-edit"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '30%', targets: [1, 2, 5, 7] }
                ]
            });

            if (_empresaActual == 6 || _empresaActual == 3) {
                $('#tblInsumos').DataTable().column(2).visible(false);
                $('#tblInsumos').DataTable().column(8).visible(false);
            }
        }

        function cargarInsumos() {
            let filtros = {
                insumo: inputFiltroInsumo.val(),
                insumoDesc: inputFiltroInsumoDesc.val(),
                // modeloMaquinaria: inputFiltroModeloMaquinaria.val(),
                modeloMaquinariaDesc: inputFiltroModeloMaquinariaDesc.val(),
                unidadDesc: inputFiltroUnidadDesc.val(),
                tipo: inputFiltroTipoInsumo.val(),
                tipoDesc: inputFiltroTipoInsumoDesc.val(),
                grupo: inputFiltroGrupoInsumo.val(),
                grupoDesc: inputFiltroGrupoInsumoDesc.val(),
                tolerancia: inputFiltroTolerancia.val()
            };

            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/Almacen/GetInsumosCatalogo', { filtros }).always($.unblockUI).then(response => {
                if (response.success) {
                    if (response.data != null) {
                        AddRows(tblInsumos, response.data);
                    }
                } else {
                    AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }

        function getInformacionInsumo(row, rowData) {
            let insumo = _empresaActual != 6 ? rowData.insumo : +rowData.PERU_insumo;

            limpiarInformacion();

            if (insumo > 0) {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Almacen/GetInformacionInsumoCatalogo', { insumo }).always($.unblockUI).then(response => {
                    if (response.success) {
                        llenarInformacion(response.data);
                        modalInsumo.modal('show');
                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
            }
        }

        function llenarInformacion(data) {
            labelInsumoDisponible.text('Disponible: ');

            inputInsumo.val(data.insumo);

            if (_empresaActual != 6) {
                inputTipoInsumo.val(data.tipo);
                inputTipoInsumoDesc.val(data.tipoDesc);
            } else {
                selectTipoInsumo.val(data.tipo);
            }

            inputGrupoInsumo.val(data.grupo);
            inputGrupoInsumoDesc.val(data.grupoDesc);
            // inputModeloMaquinaria.val(data.modeloMaquinaria);
            inputModeloMaquinariaDesc.val(data.modeloMaquinariaDesc);
            inputInsumoDesc.val(data.insumoDesc);

            if (_empresaActual != 6) {
                inputUnidad.val(data.unidadDesc);
            } else {
                selectUnidadPeru.val(data.unidadDesc);
                selectEstadoPeru.val(data.PERU_estado);
                inputCodigo2Peru.val(data.PERU_codigo2);
            }

            switch (data.cancelado) {
                case 'A':
                    radioVigente.attr('checked', true);
                    break;
                case 'P':
                    radioPassword.attr('checked', true);
                    break;
                case 'C':
                    radioCancelado.attr('checked', true);
                    break;
            }

            inputTolerancia.val(data.tolerancia);

            checkBoxProductoTerminado.attr('checked', data.bit_pt == 'S');
            checkBoxMateriaPrima.attr('checked', data.bit_mp == 'S');
            checkBoxSeFactura.attr('checked', data.bit_factura == 'S');

            checkBoxListaPrecios.attr('checked', data.validar_lista_precios == 'S');

            inputAutorizado.val(data.bit_af == 'S' ? 'Sí Utiliza' : 'No Utiliza');
            selectColorResguardo.val(data.color_resguardo == 0 || data.color_resguardo == undefined ? '' : data.color_resguardo);
            textAreaDescripcionAdicional.val('');

            checkBoxComprasRequisiciones.attr('checked', data.compras_req == 1);
        }

        function limpiarFiltros() {
            inputFiltroInsumo.val('');
            inputFiltroInsumoDesc.val('');
            // inputFiltroModeloMaquinaria.val('');
            inputFiltroModeloMaquinariaDesc.val('');
            inputFiltroUnidadDesc.val('');
            inputFiltroTipoInsumo.val('');
            inputFiltroTipoInsumoDesc.val('');
            inputFiltroGrupoInsumo.val('');
            inputFiltroGrupoInsumoDesc.val('');
            inputFiltroTolerancia.val('');
            inputFiltroFechaAlta.val('');

            // tblInsumos.DataTable().columns().search('').draw();
        }

        function limpiarInformacion() {
            inputInsumo.val('');
            inputTipoInsumo.val('');
            inputTipoInsumoDesc.val('');
            selectTipoInsumo.val('');
            inputGrupoInsumo.val('');
            inputGrupoInsumoDesc.val('');
            // inputModeloMaquinaria.val('');
            inputModeloMaquinariaDesc.val('');
            inputInsumoDesc.val('');
            inputUnidad.val('');
            selectUnidadPeru.val('');
            selectEstadoPeru.val('');
            inputCodigo2Peru.val('');

            radioVigente.attr('checked', false);
            radioPassword.attr('checked', false);
            radioCancelado.attr('checked', false);

            inputTolerancia.val('');

            checkBoxProductoTerminado.attr('checked', false);
            checkBoxMateriaPrima.attr('checked', false);
            checkBoxSeFactura.attr('checked', false);

            checkBoxListaPrecios.attr('checked', false);

            inputAutorizado.val('');
            selectColorResguardo.val('');
            textAreaDescripcionAdicional.val('');

            checkBoxComprasRequisiciones.attr('checked', false);

            labelInsumoDisponible.text('Disponible: ');
        }

        function guardarNuevoInsumo() {
            let insumo = {
                insumo: inputInsumo.val(),
                tipo: _empresaActual != 6 ? !isNaN(parseInt(inputTipoInsumo.val())) ? parseInt(inputTipoInsumo.val()) : 0 : +selectTipoInsumo.val(),
                grupo: !isNaN(parseInt(inputGrupoInsumo.val())) ? parseInt(inputGrupoInsumo.val()) : 0,
                insumoDesc: inputInsumoDesc.val(),
                unidadDesc: _empresaActual != 6 ? inputUnidad.val() : selectUnidadPeru.val(),
                cancelado: divRadioEstatusGeneral.find('input[name=estatusGeneral]:checked').val(),
                validar_lista_precios: checkBoxListaPrecios.prop('checked') ? 'S' : 'N',
                bit_pt: checkBoxProductoTerminado.prop('checked') ? 'S' : 'N',
                bit_mp: checkBoxMateriaPrima.prop('checked') ? 'S' : 'N',
                bit_factura: checkBoxSeFactura.prop('checked') ? 'S' : 'N',
                tolerancia: !isNaN(inputTolerancia.val()) ? parseFloat(inputTolerancia.val()) : 0,
                color_resguardo: selectColorResguardo.val(),
                color_resguardoDesc: selectColorResguardo.find('option:selected').text(),
                // modeloMaquinaria: inputModeloMaquinaria.val(),
                modeloMaquinariaDesc: inputModeloMaquinariaDesc.val(),
                compras_req: checkBoxComprasRequisiciones.prop('checked') ? 1 : 0,
                PERU_estado: +selectEstadoPeru.val() == 1 ? 'V' : 'F',
                PERU_codigo2: inputCodigo2Peru.val(),
            };

            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/Almacen/GuardarNuevoInsumo', { insumo }).always($.unblockUI).then(response => {
                if (response.success) {
                    AlertaGeneral(`Alerta`, `Se ha guardado la información`);
                    limpiarInformacion();
                    // cargarInsumos();
                    initTableInsumos();
                    modalInsumo.modal('hide');
                } else {
                    AlertaGeneral(`Alerta`, `Error al guardar la información.`);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }

        function getTipoGrupo(tipo, grupo) {
            $.post('/Enkontrol/Almacen/GetTipoGrupo', { tipo, grupo }).then(response => {
                if (response.success) {
                    if (_empresaActual != 6) {
                        inputTipoInsumoDesc.val(response.tipoDesc);
                        inputGrupoInsumoDesc.val(response.grupoDesc);

                        if (response.flagTipoGrupoLleno) {
                            AlertaGeneral(`Alerta`, `El tipo-grupo ${tipo}-${grupo} está lleno.`);
                        } else {
                            if (!isNaN(response.siguienteInsumoDisponible) && response.flagMostrarInsumoDisponible) {
                                labelInsumoDisponible.text('Disponible: ');
                                labelInsumoDisponible.text(`Disponible: ${response.siguienteInsumoDisponibleFolio}`);

                                if (inputInsumo.val().length == 3) {
                                    inputInsumo.val(response.siguienteInsumoDisponibleFolio);
                                }
                            } else {
                                labelInsumoDisponible.text('Disponible: ');
                            }
                        }
                    } else {
                        inputGrupoInsumoDesc.val(response.grupoDesc);

                        if (response.flagGrupoLleno) {
                            AlertaGeneral(`Alerta`, `El grupo "0${grupo}" está lleno.`);
                        } else {
                            if (!isNaN(response.siguienteInsumoDisponible) && response.flagMostrarInsumoDisponible) {
                                labelInsumoDisponible.text('Disponible: ');
                                labelInsumoDisponible.text(`Disponible: ${response.siguienteInsumoDisponibleFolio}`);

                                if (inputInsumo.val().length == 6) {
                                    inputInsumo.val(response.siguienteInsumoDisponibleFolio);
                                }
                            } else {
                                labelInsumoDisponible.text('Disponible: ');
                            }
                        }
                    }
                } else {
                    AlertaGeneral(`Alerta`, `Error al consultar la información`);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }

        function agregarTooltip(elemento, mensaje) {
            $(elemento).attr('data-toggle', 'tooltip');
            $(elemento).attr('data-placement', 'top');

            if (mensaje != "") {
                $(elemento).attr('title', mensaje);
            }

            $('[data-toggle="tooltip"]').tooltip({
                position: {
                    my: "center bottom-20",
                    at: "center top+8",
                    using: function (position, feedback) {
                        $(this).css(position);
                        $("<div>")
                            .addClass("arrow")
                            .addClass(feedback.vertical)
                            .addClass(feedback.horizontal)
                            .appendTo(this);
                    }
                }
            });
        }

        function quitarTooltip(elemento) {
            $(elemento).removeAttr('data-toggle');
            $(elemento).removeAttr('data-placement');
            $(elemento).removeAttr('title');
        }

        function setUnidad(e, ui) {
            inputUnidad.val(ui.item.id);
        }

        function validarInsumo(e, ul) {
            if (ul.item == null) {
                inputInsumo.val('');
                inputChange.change();
            }
        }

        function formData() {
            let formData = new FormData();

            $.each(document.getElementById("inputFileExcel").files, function (i, file) {
                formData.append("files[]", file);
            });

            return formData;
        }
    }
    $(document).ready(() => Enkontrol.Almacen.Almacen.Insumos = new Insumos())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();