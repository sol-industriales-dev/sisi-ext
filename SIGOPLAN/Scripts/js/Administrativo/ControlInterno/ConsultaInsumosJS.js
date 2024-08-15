(() => {
    $.namespace('administrativo.ConsultaInsumos');
    ConsultaInsumos = function () {
        //#region Selectores
        let btnSeleccionar = $('#btnSeleccionar');
        let tblInsumosAlmacen = $('#tblInsumosAlmacen');
        let btnConsultar = $('#btnConsultar');
        let btnExportar = $('#btnExportar');
        let btnBuscar = $('#btnBuscar');
        let lblModalTitulo = $('#lblModalTitulo');
        let cboTipoInsumo = $('#cboTipoInsumo');
        let cboGrupoInsumo = $('#cboGrupoInsumo');
        let tbModalInsumo = $('#tbModalInsumo');
        let tblInsumosBusqueda = $('#tblInsumosBusqueda');
        let tblInsumosBusquedaFinal = $('#tblInsumosBusquedaFinal');
        let modalBusquedaInsumos = $('#modalBusquedaInsumos');
        let tbDescripcionInsumo = $('#tbDescripcionInsumo');
        let tbNoInsumo = $('#tbNoInsumo');
        let tbAlmacenConstruplan = $('#tbAlmacenConstruplan');
        let tbAlmacenArrendadora = $('#tbAlmacenArrendadora');
        let tbDescripcionInsumoA = $('#tbDescripcionInsumoA');
        let tbTotalStock = $('#tbTotalStock');
        let btnDesplegarBusquedaConstruplan = $('#btnDesplegarBusquedaConstruplan');
        let btnDesplegarBusquedaArrendadora = $('#btnDesplegarBusquedaArrendadora');
        let mdlExistencias = $('#mdlExistencias');
        let tblExistencias = $('#tblExistencias');
        let btnByIDConstruplan = $("#btnByIDConstruplan");
        let btnByIDArrendadora = $("#btnByIDArrendadora");
        let tbNoInsumoArrendadora = $("#tbNoInsumoArrendadora");
        //#endregion

        let _Insumos = new Array();
        let mensajes = { PROCESANDO: 'Procesando...' };
        let ireport = $('#report');
        let _tipoSistema = 0;

        $('#modalBusquedaInsumos').on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        (function init() {
            cboTipoInsumo.fillCombo('/Administrativo/RepTraspaso/fillCboTipoInsumos', { sistema: _tipoSistema }, false, '--Todos--');
            cboGrupoInsumo.fillCombo('/Administrativo/RepTraspaso/fillCboGrupoInsumos', { tipo: 0, sistema: _tipoSistema }, false, '--Todos--');

            tbModalInsumo.keypress(autoComplete);
            cboTipoInsumo.change(loadGruposInsumos);

            btnByIDArrendadora.click(loadInsumosClick);
            btnByIDConstruplan.click(loadInsumosClick);
            btnDesplegarBusquedaConstruplan.click(modalShow);
            btnDesplegarBusquedaArrendadora.click(modalShow);
            btnBuscar.click(getListaInsumos);
            btnExportar.click(verRptInsumos);
            btnConsultar.click(fnConsultar);
            btnSeleccionar.click(fnSeleccionarTodo);

            initTable();
            initTableExistencias();
        })();

        function initTable() {
            tblInsumosBusqueda = $("#tblInsumosBusqueda").DataTable({
                scrollY: "200px",
                scrollCollapse: true,
                paging: false,
                bInfo: false,
                ajax: {
                    url: '/Administrativo/RepTraspaso/GetListaInsumosBusqueda',
                    dataSrc: 'dataSet',
                    data: function (d) {
                        d.TipoInsumo = (cboTipoInsumo.val() == "--Todos--" ? 0 : cboTipoInsumo.val()),
                            d.GrupoInsumo = (cboGrupoInsumo.val() == "--Todos--" ? 0 : cboGrupoInsumo.val()),
                            d.term = tbModalInsumo.val(),
                            d.dataInsumo = tbModalInsumo.attr("data-numinsumo"),
                            d.Sistema = _tipoSistema
                    }
                },
                columns: [
                    { data: 'insumo' },
                    { data: 'descripcion' }
                ],
                drawCallback: function (settings) {
                    var insumos = $(".clsInsumo");

                    $.each(insumos, function (i, e) {
                        $(e).dblclick(function () {
                            var insumo = $(this).data("insumo");
                            var descripcion = $(this).data("descripcion");
                            var insumosBusqueda = $(".clsInsumoBusqueda");
                            var lista = new Array();
                            $.each(insumosBusqueda, function (i2, e2) {
                                lista.push($(e2).data("insumo"));
                            });
                            if (!lista.includes(insumo)) {
                                tblInsumosBusquedaFinal.row.add({
                                    'insumo': "<span data-insumo='" + insumo + "' data-descripcion='" + descripcion + "' class='clsInsumoBusqueda'>" + insumo + "</span>",
                                    'descripcion': descripcion,
                                    'eliminar': "<button class='btn btn-xs btn-danger clsEliminar'><i class='glyphicon glyphicon-remove'></i></button>"
                                }).draw(false);
                            }
                        });
                    });
                }
            });
            tblInsumosBusquedaFinal = $("#tblInsumosBusquedaFinal").DataTable({
                scrollY: '200px',
                scrollCollapse: true,
                paging: false,
                bInfo: false,
                columns: [
                    { data: 'insumo' },
                    { data: 'descripcion' },
                    { data: 'eliminar' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": [2] }
                ]
            });
            tblInsumosAlmacen.DataTable({
                columns: [
                    { data: 'insumo' },
                    { data: 'descripcion' },
                    { data: 'cantidad' }
                ]
            });
            $('#tblInsumosBusquedaFinal tbody').on('click', '.clsEliminar', function () {
                tblInsumosBusquedaFinal.row($(this).parents('tr')).remove().draw();
            });
        }

        function initTableExistencias() {
            tblExistencias.DataTable({
                retrieve: true,
                deferRender: true,
                language: dtDicEsp,
                paging: false,
                lengthChange: false,
                bInfo: false,
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'insumoDesc', title: 'Insumo' },
                    { data: 'cantidad', title: 'Cantidad' },
                    { data: 'ultimoConsumoString', title: 'Últ. Consumo' },
                    { data: 'ultimaCompraString', title: 'Últ. Compra' },
                    { data: 'area_alm', title: 'Área' },
                    { data: 'lado_alm', title: 'Lado' },
                    { data: 'estante_alm', title: 'Estante' },
                    { data: 'nivel_alm', title: 'Nivel' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function fnSeleccionarTodo() {
            var insumos = $(".clsInsumo");
            var insumosBusqueda = $(".clsInsumoBusqueda");
            var lista = new Array();
            $.each(insumosBusqueda, function (i2, e2) {
                lista.push($(e2).data("insumo"));
            });
            $.each(insumos, function (i2, e2) {
                var insumo = $(e2).data("insumo");
                var descripcion = $(e2).data("descripcion");
                if (!lista.includes(insumo)) {
                    tblInsumosBusquedaFinal.row.add({
                        'insumo': "<span data-insumo='" + insumo + "' data-descripcion='" + descripcion + "' class='clsInsumoBusqueda'>" + insumo + "</span>",
                        'descripcion': descripcion,
                        'eliminar': "<button class='btn btn-xs btn-danger clsEliminar'><i class='glyphicon glyphicon-remove'></i></button>"
                    }).draw(false);
                }
            });
        }

        function fnConsultar() {
            var insumosBusqueda = $(".clsInsumoBusqueda");
            var lista = new Array();
            $.each(insumosBusqueda, function (i2, e2) {
                lista.push($(e2).data("insumo"));
                _Insumos.push($(e2).data("insumo"));
            });
            _Insumos = lista;
            var insumos = _Insumos.join("\n");
            if (_tipoSistema == 1) {
                tbNoInsumo.val(insumos);
            }
            else {
                tbNoInsumoArrendadora.val(insumos);
            }
            loadInsumosMultiple(_tipoSistema);
        }

        function loadInsumosClick() {
            _tipoSistema = Number($(this).attr('data-tipo'));

            loadInsumosMultiple(_tipoSistema);
        }

        function autoComplete() {
            tbModalInsumo.getAutocomplete(SelectInsumo, getFiltrosInsumos(), '/Administrativo/RepTraspaso/getInsumos');
        }

        function SelectInsumo(event, ui) {
            tbModalInsumo.text(ui.item.value);
            tbModalInsumo.attr('data-numInsumo', ui.item.id);
        }

        function getFiltrosInsumos() {
            var obj = {};

            obj.TipoInsumo = cboTipoInsumo.val() == "--Todos--" ? 0 : cboTipoInsumo.val();
            obj.GrupoInsumo = cboGrupoInsumo.val() == "--Todos--" ? 0 : cboGrupoInsumo.val();
            obj.term = tbModalInsumo.val();
            obj.dataInsumo = tbModalInsumo.attr("data-numinsumo");
            obj.Sistema = _tipoSistema;

            return obj;
        }

        function getListaInsumos() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/Administrativo/RepTraspaso/GetListaInsumos',
                data: getFiltrosInsumos(),
                async: false,
                success: function (response) {
                    loadInsumosGrid(response.dataSet);
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function verRptInsumos() {
            $.blockUI({ message: mensajes.PROCESANDO });
            var path = "/Reportes/Vista.aspx?idReporte=" + 84 + "&pInsumoC=" + tbNoInsumo.val() + "&pDescripcionC=" + tbDescripcionInsumo.val() + "&pDescripcionA=" + tbDescripcionInsumoA.val() + "&pCantidadConstruplan=" + tbAlmacenConstruplan.val() + "&pCantidadArrendadora=" + tbAlmacenArrendadora.val();
            ireport.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function loadInsumosGrid(dataSet) {
            tblInsumosBusqueda.ajax.reload(null, false);
        }

        function modalShow() {
            tipo = Number($(this).attr('data-tipo'));

            if (tipo == 1) {
                lblModalTitulo.text('Consulta de Insumos CONSTRUPLAN');
                _tipoSistema = 1;
            } else {
                lblModalTitulo.text('Consulta de Insumos ARRENDADORA');
                _tipoSistema = 2;
            }

            tbModalInsumo.val("");
            tblInsumosBusqueda.clear().draw();
            tblInsumosBusquedaFinal.clear().draw();
            cboTipoInsumo.fillCombo('/Administrativo/RepTraspaso/fillCboTipoInsumos', { sistema: _tipoSistema }, false, "--Todos--");
            cboGrupoInsumo.fillCombo('/Administrativo/RepTraspaso/fillCboGrupoInsumos', { tipo: 0, sistema: _tipoSistema }, false, "--Todos--");

            var array = new Array();

            loadInsumosGrid(array);

            modalBusquedaInsumos.modal('show');
        }

        function loadGruposInsumos() {
            if (cboTipoInsumo.val() != null && cboTipoInsumo.val() != "--Todos--") {
                cboGrupoInsumo.fillCombo('/Administrativo/RepTraspaso/fillCboGrupoInsumos', { tipo: cboTipoInsumo.val(), sistema: _tipoSistema }, false, "--Todos--");
            } else {
                cboGrupoInsumo.clearCombo();
                cboGrupoInsumo.fillCombo('/Administrativo/RepTraspaso/fillCboGrupoInsumos', { tipo: 0, sistema: _tipoSistema }, false, "--Todos--");
            }
        }

        function loadInsumosMultiple(tipoSistema) {
            $.blockUI({ message: mensajes.PROCESANDO });

            var lines = [];
            var src = tipoSistema == 1 ? "tbNoInsumo" : "tbNoInsumoArrendadora";

            $.each($('#' + src).val().split(/\n/), function (i, line) {
                if (line) {
                    lines.push(line);
                }
            });

            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/Administrativo/RepTraspaso/CargarInsumoMultiple',
                data: { insumos: lines, sistema: tipoSistema },
                success: function (response) {
                    tbAlmacenConstruplan.val(response.TotalConstruplan);
                    tbAlmacenArrendadora.val(response.TotalArrendadora);
                    tbTotalStock.val(response.TotalArrendadora + response.TotalConstruplan);
                    tblConstruplanLoad(response.dataConstruplan);
                    tblArrendadoraLoad(response.dataArrendadora);

                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function tblConstruplanLoad(dataSet) {
            $('#tblConstruplan').DataTable().clear().destroy();

            tblConstruplanGrid = $("#tblConstruplan").DataTable({
                scrollX: true,
                data: dataSet,
                initComplete: function (settings, json) {
                    $("#tblConstruplan").on('click', '.btnUbicaciones', function () {
                        let rowData = $("#tblConstruplan").DataTable().row($(this).closest('tr')).data();

                        cargarExistencias(1, rowData);
                    });
                },
                columns: [
                    { data: "insumoNumero", type: "text" },
                    { data: "insumoDescripcion" },
                    { data: "insumoCantidad" },
                    { data: "almacenNombre" },
                    {
                        render: function (data, type, row, meta) {
                            if (row.insumoCantidad > 0) {
                                return `<button class="btn btn-xs btn-default btnUbicaciones"><i class="glyphicon glyphicon-th-list"></i></button>`
                            } else {
                                return '';
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": [4] }
                ],
                paging: false,
                info: false,
                dom: 'Bfrtip',
                buttons: parametrosImpresion("Existencia de insumos CONSTRUPLAN", "<center><h3>Existencia de insumos CONSTRUPLAN</h3></center>"),
                buttons: [{ extend: 'excel' }]
            });
        }

        function tblArrendadoraLoad(dataSet) {
            $('#tblArrendadora').DataTable().clear().destroy();

            tblArrendadoraGrid = $("#tblArrendadora").DataTable({
                scrollX: true,
                data: dataSet,
                initComplete: function (settings, json) {
                    $("#tblArrendadora").on('click', '.btnUbicaciones', function () {
                        let rowData = $("#tblArrendadora").DataTable().row($(this).closest('tr')).data();

                        cargarExistencias(2, rowData);
                    });
                },
                columns: [
                    { data: "insumoNumero", type: "text" },
                    { data: "insumoDescripcion" },
                    { data: "insumoCantidad" },
                    { data: "almacenNombre" },
                    {
                        render: function (data, type, row, meta) {
                            if (row.insumoCantidad > 0) {
                                return `<button class="btn btn-xs btn-default btnUbicaciones"><i class="glyphicon glyphicon-th-list"></i></button>`
                            } else {
                                return '';
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": [4] }
                ],
                paging: false,
                info: false,
                dom: 'Bfrtip',
                buttons: parametrosImpresion("Existencia de insumos Arrendadora", "<center><h3>Existencia de insumos CONSTRUPLAN</h3></center>"),
                buttons: [{ extend: 'excel' }]
            });
        }

        function cargarExistencias(empresa, rowData) {
            let insumo = rowData.insumoNumero;
            let almacen = rowData.almacenNumero;

            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/Almacen/GetExistencias', { empresa, insumo, almacen })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tblExistencias, response.data);

                        mdlExistencias.modal('show');
                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información.`);
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
    }
    $(document).ready(() => administrativo.ConsultaInsumos = new ConsultaInsumos())
})();