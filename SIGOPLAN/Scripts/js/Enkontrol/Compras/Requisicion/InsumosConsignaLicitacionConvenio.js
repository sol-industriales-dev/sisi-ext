(() => {
    $.namespace('Enkontrol.Requisicion.InsumosConsignaLicitacionConvenio');
    InsumosConsignaLicitacionConvenio = function () {
        //#region Selectores
        const tablaInsumosConsigna = $('#tablaInsumosConsigna');
        const botonExcelConsigna = $('#botonExcelConsigna');
        const botonDescargarExcelConsigna = $('#botonDescargarExcelConsigna');
        const botonAgregarConsigna = $('#botonAgregarConsigna');
        const tablaInsumosLicitacion = $('#tablaInsumosLicitacion');
        const botonExcelLicitacion = $('#botonExcelLicitacion');
        const botonDescargarExcelLicitacion = $('#botonDescargarExcelLicitacion');
        const botonAgregarLicitacion = $('#botonAgregarLicitacion');
        const tablaInsumosConvenio = $('#tablaInsumosConvenio');
        const botonExcelConvenio = $('#botonExcelConvenio');
        const botonDescargarExcelConvenio = $('#botonDescargarExcelConvenio');
        const botonAgregarConvenio = $('#botonAgregarConvenio');
        const modalInsumo = $('#modalInsumo');
        const inputInsumo = $('#inputInsumo');
        const inputInsumoDesc = $('#inputInsumoDesc');
        const inputProveedor = $('#inputProveedor');
        const inputProveedorDesc = $('#inputProveedorDesc');
        const inputArticulo = $('#inputArticulo');
        const inputUnidad = $('#inputUnidad');
        const inputPrecio = $('#inputPrecio');
        const botonGuardar = $('#botonGuardar');
        const modalCargarExcel = $('#modalCargarExcel');
        const botonGuardarExcel = $('#botonGuardarExcel');
        const labelFormato = $('#labelFormato');
        //#endregion

        let dtInsumosConsigna;
        let dtInsumosLicitacion;
        let dtInsumosConvenio;
        const ESTATUS = { NUEVO: 0, EDITAR: 1 };
        const TIPO_INSUMO = { CONSIGNA: 1, LICITACION: 2, CONVENIO: 4 };

        (function init() {
            initTablaInsumosConsigna();
            initTablaInsumosLicitacion();
            initTablaInsumosConvenio();
            agregarListeners();
            // cargarInsumosConsigna();
            // cargarInsumosLicitacion();
            // cargarInsumosConvenio();
        })();

        modalInsumo.on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        modalInsumo.find('input').on('focus', function () {
            $(this).select();
        });

        inputInsumo.on('change', function () {
            let insumo = +$(this).val();

            if (insumo > 0) {
                axios.post('/Enkontrol/Requisicion/GetInsumoInformacion', { insumo })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            inputInsumoDesc.val(response.data.data.id);
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        });

        inputProveedor.on('change', function () {
            let proveedor = +$(this).val();

            if (proveedor > 0) {
                axios.post('/Enkontrol/OrdenCompra/GetProveedorInfo', { num: proveedor })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        inputProveedorDesc.val(response.data.id);
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        });

        botonGuardarExcel.on('click', function () {
            try {
                var request = new XMLHttpRequest();

                $.blockUI({ message: 'Procesando...', baseZ: 2000 });

                let url = '';

                switch (botonGuardarExcel.data().tipo_insumo) {
                    case TIPO_INSUMO.CONSIGNA:
                        url = "/Enkontrol/Requisicion/CargarExcelInsumosConsigna";
                        break;
                    case TIPO_INSUMO.LICITACION:
                        url = "/Enkontrol/Requisicion/CargarExcelInsumosLicitacion";
                        break;
                    case TIPO_INSUMO.CONVENIO:
                        url = "/Enkontrol/Requisicion/CargarExcelInsumosConvenio";
                        break;
                }

                request.open("POST", url);
                request.send(formData());
                request.onload = function (response) {
                    if (request.status == 200) {
                        $.unblockUI();

                        let respuesta = JSON.parse(request.response);

                        if (respuesta.success) {
                            modalCargarExcel.modal('hide');
                            initTablaInsumosConsigna();
                            initTablaInsumosLicitacion();
                            initTablaInsumosConvenio();
                            // cargarInsumosConsigna();
                            // cargarInsumosLicitacion();
                            // cargarInsumosConvenio();
                            $('#inputFileExcel').val('');
                            Alert2Exito('Se ha guardado la información.');
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

        function formData() {
            let formData = new FormData();

            $.each(document.getElementById("inputFileExcel").files, function (i, file) {
                formData.append("files[]", file);
            });

            return formData;
        }

        function initTablaInsumosConsigna() {
            if ($.fn.DataTable.isDataTable('#tablaInsumosConsigna')) {
                tablaInsumosConsigna.DataTable().clear().destroy();
            }

            dtInsumosConsigna = tablaInsumosConsigna.DataTable({
                destroy: true,
                bLengthChange: false,
                deferRender: true,
                ordering: false,

                processing: true,
                serverSide: true,
                bServerSide: true,
                sAjaxSource: '/Enkontrol/Requisicion/GetInsumosConsigna',
                fnServerData: function (sSource, aoData, fnCallback) {
                    $.ajax({ type: "Post", data: aoData, url: sSource, success: fnCallback });
                },

                retrieve: true,
                language: dtDicEsp,
                bInfo: false,
                dom: 'frtip',
                // buttons: [{ extend: 'excelHtml5', exportOptions: { columns: [0, 1, 2, 4, 5, 6] }, className: 'btn btn-sm btn-default', text: '', title: '' }],
                initComplete: function (settings, json) {
                    tablaInsumosConsigna.on('click', '.btn-editar', function () {
                        let rowData = dtInsumosConsigna.row($(this).closest('tr')).data();

                        limpiarModal();
                        llenarCamposModal(rowData);
                        inputArticulo.val('');
                        inputUnidad.val('');
                        $('#divArticulo').hide();
                        $('#divUnidad').hide();
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;
                        botonGuardar.data().tipo_insumo = TIPO_INSUMO.CONSIGNA;
                        modalInsumo.modal('show');
                    });

                    tablaInsumosConsigna.on('click', '.btn-eliminar', function () {
                        let rowData = dtInsumosConsigna.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar el insumo?`,
                            () => eliminarInsumo(rowData.id, TIPO_INSUMO.CONSIGNA))
                    });
                },
                columns: [
                    { data: 'insumo', title: 'Insumo' },
                    { data: 'insumoDesc', title: 'Descripción' },
                    { data: 'proveedor', title: 'Proveedor', visible: false },
                    { data: 'proveedorDesc', title: 'Proveedor' },
                    {
                        data: 'precio', title: 'Precio', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return maskNumero6DCompras(data);
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        title: 'Activo', render: function (data, type, row, meta) {
                            return 1;
                        }, visible: false
                    },
                    {
                        title: 'Tipo', render: function (data, type, row, meta) {
                            return 'Consigna';
                        }, visible: false
                    },
                    {
                        render: function (data, type, row, meta) {
                            return `
                            <button title="Editar" class="btn-editar btn btn-xs btn-warning">
                                <i class="fas fa-pencil-alt"></i>
                            </button>
                            &nbsp;
                            <button title="Eliminar" class="btn-eliminar btn btn-xs btn-danger">
                                <i class="fas fa-trash"></i>
                            </button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '8%', targets: [7] }
                ]
            });

            // dtInsumosConsigna.buttons().container().find('.buttons-excel').appendTo('#divBotonesConsigna');
            // $('#divBotonesConsigna .buttons-excel').append('<i class="fa fa-file"></i> Descargar Excel');

            $("#tablaInsumosConsigna_filter input").unbind();
            $("#tablaInsumosConsigna_filter input").keyup(function (e) {
                if (e.keyCode == 13) {
                    dtInsumosConsigna.search(this.value).draw();
                }
            });
        }

        function initTablaInsumosLicitacion() {
            if ($.fn.DataTable.isDataTable('#tablaInsumosLicitacion')) {
                tablaInsumosLicitacion.DataTable().clear().destroy();
            }

            dtInsumosLicitacion = tablaInsumosLicitacion.DataTable({
                destroy: true,
                bLengthChange: false,
                deferRender: true,
                ordering: false,

                processing: true,
                serverSide: true,
                bServerSide: true,
                sAjaxSource: '/Enkontrol/Requisicion/GetInsumosLicitacion',
                fnServerData: function (sSource, aoData, fnCallback) {
                    $.ajax({ type: "Post", data: aoData, url: sSource, success: fnCallback });
                },

                retrieve: true,
                language: dtDicEsp,
                bInfo: false,
                dom: 'frtip',
                initComplete: function (settings, json) {
                    tablaInsumosLicitacion.on('click', '.btn-editar', function () {
                        let rowData = dtInsumosLicitacion.row($(this).closest('tr')).data();

                        limpiarModal();
                        llenarCamposModal(rowData);
                        $('#divArticulo').show();
                        $('#divUnidad').show();
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;
                        botonGuardar.data().tipo_insumo = TIPO_INSUMO.LICITACION;
                        modalInsumo.modal('show');
                    });

                    tablaInsumosLicitacion.on('click', '.btn-eliminar', function () {
                        let rowData = dtInsumosLicitacion.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar el insumo?`,
                            () => eliminarInsumo(rowData.id, TIPO_INSUMO.LICITACION))
                    });
                },
                columns: [
                    { data: 'insumo', title: 'Insumo' },
                    { data: 'insumoDesc', title: 'Descripción' },
                    { data: 'proveedor', title: 'Proveedor', visible: false },
                    { data: 'proveedorDesc', title: 'Proveedor' },
                    { data: 'articulo', title: 'Artículo' },
                    { data: 'unidad', title: 'Unidad' },
                    {
                        data: 'precio', title: 'Precio', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return maskNumero6DCompras(data);
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        title: 'Activo', render: function (data, type, row, meta) {
                            return 1;
                        }, visible: false
                    },
                    {
                        title: 'Tipo', render: function (data, type, row, meta) {
                            return 'Licitación';
                        }, visible: false
                    },
                    {
                        render: function (data, type, row, meta) {
                            return `
                            <button title="Editar" class="btn-editar btn btn-xs btn-warning">
                                <i class="fas fa-pencil-alt"></i>
                            </button>
                            &nbsp;
                            <button title="Eliminar" class="btn-eliminar btn btn-xs btn-danger">
                                <i class="fas fa-trash"></i>
                            </button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '8%', targets: [9] }
                ]
            });

            // dtInsumosLicitacion.buttons().container().find('.buttons-excel').appendTo('#divBotonesLicitacion');
            // $('#divBotonesLicitacion .buttons-excel').append('<i class="fa fa-file"></i> Descargar Excel');

            $("#tablaInsumosLicitacion_filter input").unbind();
            $("#tablaInsumosLicitacion_filter input").keyup(function (e) {
                if (e.keyCode == 13) {
                    dtInsumosLicitacion.search(this.value).draw();
                }
            });
        }

        function initTablaInsumosConvenio() {
            if ($.fn.DataTable.isDataTable('#tablaInsumosConvenio')) {
                tablaInsumosConvenio.DataTable().clear().destroy();
            }

            dtInsumosConvenio = tablaInsumosConvenio.DataTable({
                destroy: true,
                bLengthChange: false,
                deferRender: true,
                ordering: false,

                processing: true,
                serverSide: true,
                bServerSide: true,
                sAjaxSource: '/Enkontrol/Requisicion/GetInsumosConvenio',
                fnServerData: function (sSource, aoData, fnCallback) {
                    $.ajax({ type: "Post", data: aoData, url: sSource, success: fnCallback });
                },

                retrieve: true,
                language: dtDicEsp,
                bInfo: false,
                dom: 'frtip',
                initComplete: function (settings, json) {
                    tablaInsumosConvenio.on('click', '.btn-editar', function () {
                        let rowData = dtInsumosConvenio.row($(this).closest('tr')).data();

                        limpiarModal();
                        llenarCamposModal(rowData);
                        inputArticulo.val('');
                        inputUnidad.val('');
                        $('#divArticulo').hide();
                        $('#divUnidad').hide();
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;
                        botonGuardar.data().tipo_insumo = TIPO_INSUMO.CONVENIO;
                        modalInsumo.modal('show');
                    });

                    tablaInsumosConvenio.on('click', '.btn-eliminar', function () {
                        let rowData = dtInsumosConvenio.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar el insumo?`,
                            () => eliminarInsumo(rowData.id, TIPO_INSUMO.CONVENIO))
                    });
                },
                columns: [
                    { data: 'insumo', title: 'Insumo' },
                    { data: 'insumoDesc', title: 'Descripción' },
                    { data: 'proveedor', title: 'Proveedor', visible: false },
                    { data: 'proveedorDesc', title: 'Proveedor' },
                    // { data: 'articulo', title: 'Artículo' },
                    // { data: 'unidad', title: 'Unidad' },
                    {
                        data: 'precio', title: 'Precio', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return maskNumero6DCompras(data);
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        title: 'Activo', render: function (data, type, row, meta) {
                            return 1;
                        }, visible: false
                    },
                    {
                        title: 'Tipo', render: function (data, type, row, meta) {
                            return 'Convenio';
                        }, visible: false
                    },
                    {
                        render: function (data, type, row, meta) {
                            return `
                            <button title="Editar" class="btn-editar btn btn-xs btn-warning">
                                <i class="fas fa-pencil-alt"></i>
                            </button>
                            &nbsp;
                            <button title="Eliminar" class="btn-eliminar btn btn-xs btn-danger">
                                <i class="fas fa-trash"></i>
                            </button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '8%', targets: [7] }
                ]
            });

            // dtInsumosConvenio.buttons().container().find('.buttons-excel').appendTo('#divBotonesConvenio');
            // $('#divBotonesConvenio .buttons-excel').append('<i class="fa fa-file"></i> Descargar Excel');

            $("#tablaInsumosConvenio_filter input").unbind();
            $("#tablaInsumosConvenio_filter input").keyup(function (e) {
                if (e.keyCode == 13) {
                    dtInsumosConvenio.search(this.value).draw();
                }
            });
        }

        function cargarInsumosConsigna() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Enkontrol/Requisicion/GetInsumosConsigna')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tablaInsumosConsigna, response.data);
                        $.fn.dataTable.tables({ api: true }).columns.adjust();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function cargarInsumosLicitacion() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Enkontrol/Requisicion/GetInsumosLicitacion')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tablaInsumosLicitacion, response.data);
                        $.fn.dataTable.tables({ api: true }).columns.adjust();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function cargarInsumosConvenio() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Enkontrol/Requisicion/GetInsumosConvenio')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tablaInsumosConvenio, response.data);
                        $.fn.dataTable.tables({ api: true }).columns.adjust();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function agregarListeners() {
            botonAgregarConsigna.click(() => {
                limpiarModal();
                $('#divArticulo').hide();
                $('#divUnidad').hide();
                botonGuardar.data().estatus = ESTATUS.NUEVO;
                botonGuardar.data().id = 0;
                botonGuardar.data().tipo_insumo = TIPO_INSUMO.CONSIGNA;
                modalInsumo.modal('show');
            });
            botonAgregarLicitacion.click(() => {
                limpiarModal();
                $('#divArticulo').show();
                $('#divUnidad').show();
                botonGuardar.data().estatus = ESTATUS.NUEVO;
                botonGuardar.data().id = 0;
                botonGuardar.data().tipo_insumo = TIPO_INSUMO.LICITACION;
                modalInsumo.modal('show');
            });
            botonAgregarConvenio.click(() => {
                limpiarModal();
                $('#divArticulo').hide();
                $('#divUnidad').hide();
                botonGuardar.data().estatus = ESTATUS.NUEVO;
                botonGuardar.data().id = 0;
                botonGuardar.data().tipo_insumo = TIPO_INSUMO.CONVENIO;
                modalInsumo.modal('show');
            });
            botonGuardar.click(guardarInsumo);
            botonExcelConsigna.click(() => {
                modalCargarExcel.modal('show');
                labelFormato.text('Formato: Insumo | Proveedor | Precio | Activo')
                botonGuardarExcel.data().tipo_insumo = TIPO_INSUMO.CONSIGNA;
            });
            botonDescargarExcelConsigna.click(() => {
                location.href = 'DescargarExcelInsumosConsigna';
            });
            botonExcelLicitacion.click(() => {
                modalCargarExcel.modal('show');
                labelFormato.text('Formato: Insumo | Proveedor | Artículo | Unidad | Precio | Activo');
                botonGuardarExcel.data().tipo_insumo = TIPO_INSUMO.LICITACION;
            });
            botonDescargarExcelLicitacion.click(() => {
                location.href = 'DescargarExcelInsumosLicitacion';
            });
            botonExcelConvenio.click(() => {
                modalCargarExcel.modal('show');
                labelFormato.text('Formato: Insumo | Proveedor | Precio | Activo')
                botonGuardarExcel.data().tipo_insumo = TIPO_INSUMO.CONVENIO;
            });
            botonDescargarExcelConvenio.click(() => {
                location.href = 'DescargarExcelInsumosConvenio';
            });
        }

        function limpiarModal() {
            inputInsumo.val('');
            inputInsumoDesc.val('');
            inputProveedor.val('');
            inputProveedorDesc.val('');
            inputArticulo.val('');
            inputUnidad.val('');
            inputPrecio.val('');
        }

        function llenarCamposModal(data) {
            inputInsumo.val(data.insumo);
            inputInsumo.change();
            inputProveedor.val(data.proveedor);
            inputProveedor.change();
            inputArticulo.val(data.articulo);
            inputUnidad.val(data.unidad);
            inputPrecio.val(maskNumero6DCompras(data.precio));
        }

        function guardarInsumo() {
            let estatus = botonGuardar.data().estatus;
            let tipo_insumo = botonGuardar.data().tipo_insumo;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    nuevoInsumo(tipo_insumo);
                    break;
                case ESTATUS.EDITAR:
                    editarInsumo(tipo_insumo);
                    break;
            }
        }

        function nuevoInsumo(tipo_insumo) {
            let insumo = getInformacionInsumo();
            let url = '';

            switch (tipo_insumo) {
                case TIPO_INSUMO.CONSIGNA:
                    url = '/Enkontrol/Requisicion/GuardarNuevoInsumoConsigna';
                    break;
                case TIPO_INSUMO.LICITACION:
                    url = '/Enkontrol/Requisicion/GuardarNuevoInsumoLicitacion';
                    break;
                case TIPO_INSUMO.CONVENIO:
                    url = '/Enkontrol/Requisicion/GuardarNuevoInsumoConvenio';
                    break;
            }

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post(url, { insumo }).always($.unblockUI).then(response => {
                if (response.success) {
                    modalInsumo.modal('hide');
                    Alert2Exito('Se ha guardado la información.');
                    initTablaInsumosConsigna();
                    initTablaInsumosLicitacion();
                    initTablaInsumosConvenio();
                    // cargarInsumosConsigna();
                    // cargarInsumosLicitacion();
                    // cargarInsumosConvenio();
                } else {
                    AlertaGeneral(`Alerta`, response.message);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }

        function editarInsumo(tipo_insumo) {
            let insumo = getInformacionInsumo();
            let url = '';

            switch (tipo_insumo) {
                case TIPO_INSUMO.CONSIGNA:
                    url = '/Enkontrol/Requisicion/EditarInsumoConsigna';
                    break;
                case TIPO_INSUMO.LICITACION:
                    url = '/Enkontrol/Requisicion/EditarInsumoLicitacion';
                    break;
                case TIPO_INSUMO.CONVENIO:
                    url = '/Enkontrol/Requisicion/EditarInsumoConvenio';
                    break;
            }

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post(url, { insumo }).always($.unblockUI).then(response => {
                if (response.success) {
                    modalInsumo.modal('hide');
                    Alert2Exito('Se ha guardado la información.');
                    initTablaInsumosConsigna();
                    initTablaInsumosLicitacion();
                    initTablaInsumosConvenio();
                    // cargarInsumosConsigna();
                    // cargarInsumosLicitacion();
                    // cargarInsumosConvenio();
                } else {
                    AlertaGeneral(`Alerta`, response.message);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }

        function eliminarInsumo(id, tipo_insumo) {
            let insumo = { id };
            let url = '';

            switch (tipo_insumo) {
                case TIPO_INSUMO.CONSIGNA:
                    url = '/Enkontrol/Requisicion/EliminarInsumoConsigna';
                    break;
                case TIPO_INSUMO.LICITACION:
                    url = '/Enkontrol/Requisicion/EliminarInsumoLicitacion';
                    break;
                case TIPO_INSUMO.CONVENIO:
                    url = '/Enkontrol/Requisicion/EliminarInsumoConvenio';
                    break;
            }

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post(url, { insumo }).always($.unblockUI).then(response => {
                if (response.success) {
                    Alert2Exito('Se ha quitado la información.');
                    initTablaInsumosConsigna();
                    initTablaInsumosLicitacion();
                    initTablaInsumosConvenio();
                    // cargarInsumosConsigna();
                    // cargarInsumosLicitacion();
                    // cargarInsumosConvenio();
                } else {
                    AlertaGeneral(`Alerta`, response.message);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }

        function getInformacionInsumo() {
            return {
                id: botonGuardar.data().id,
                insumo: +inputInsumo.val(),
                proveedor: +inputProveedor.val(),
                articulo: inputArticulo.val(),
                unidad: inputUnidad.val(),
                precio: unmaskNumero6DCompras(inputPrecio.val()),
                registroActivo: true
            };
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Enkontrol.Requisicion.InsumosConsignaLicitacionConvenio = new InsumosConsignaLicitacionConvenio())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();