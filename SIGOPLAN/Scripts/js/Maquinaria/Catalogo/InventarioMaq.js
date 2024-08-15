(function () {

    $.namespace('maquinaria.catalogo.InventarioMaq');

    InventarioMaq = function () {

        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'

        };

        mensajes = {
            NOMBRE: 'Inventario',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        var ruta = "/CatInventario/FillGridInventario";
        btnExport = $("#btnExport"),
            cboSemanas = $("#cboSemanas"),
            ModalListaInventario = $("#ModalListaInventario"),
            btnHistorial = $("#btnHistorial"),
            btnImprimir = $("#btnImprimir"),
            tbEconomico = $("#tbEconomico"),
            cboFiltroTipoMaquinaria = $("#cboFiltroTipoMaquinaria"),
            cboFiltroEstatus = $("#cboFiltroEstatus"),
            txtFiltroCentroCostos = $("#txtFiltroCentroCostos"),
            btnBuscar = $("#btnBuscar"),
            tituloModal = $("#title-modal"),
            btnEnvioCorreo = $("#btnEnvioCorreo"),
            cboCC = $("#cboCC"),
            ireport = $("#report"),
            envioCorreosModal = $("#envioCorreosModal"),
            gridResultado = $("#grid_Inventario");
        tblCxC = $('#grid_Inventario').DataTable();
        btnSabado = $("#btnSabado");
        btnMartes = $("#btnMartes");
        btnStandBy = $('#btnStandBy');


        function init() {
            cboFiltroTipoMaquinaria.fillCombo('/CatGrupos/FillCboTipoMaquinaria', { estatus: true });
            //initGrid();
            btnBuscar.click(LoadTabla);
            cboCC.fillCombo('/CatInventario/FillComboCC', { est: true }, false, "Todos");
            convertToMultiselect("#cboCC");
            btnImprimir.click(verReporte);
            btnHistorial.click(verHistorial);
            cboSemanas.fillCombo('/CatInventario/FillCboSemanas');
            cboSemanas.change(LoadTablaHist);
            btnExport.click(getFile);
            btnEnvioCorreo.click(verCorreos);
            btnSabado.attr('data-inventario', 2);
            btnMartes.attr('data-inventario', 1);
            btnStandBy.attr('data-inventario', 1);

            btnMartes.click(EnviarCorreo);
            btnStandBy.click(EnviarCorreoStandBy);
        }

        function EnviarCorreo() {
            var tipoInventario = $(this).attr('data-inventario');

            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/CatInventario/EnviarInventario",
                type: "POST",
                datatype: "json",
                data: { obj: tipoInventario },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        btnMartes.hide();
                    }
                    else {
                        AlertaGeneral('Alerta', response.message);
                    }
                    //dataSet = response.rows;
                    // iniciarGridInv(dataSet);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function EnviarCorreoStandBy() {
            var tipoInve = $(this).attr('data-inventario');

            $.blockUI({ message: mensajes.PROCESANDO });
            $.post('/StandByNuevo/EnviarExcelCorteStandBy', {
                inventario: tipoInve
            }).always().then(response => {
                $.unblockUI();
                if (response.Success) {
                    ConfirmacionGeneral('Confirmación', '!Se envió el inventario');
                }else{
                    AlertaGeneral('Alerta', response.Message);
                }
            }, error => {
                $.unblockUI();
                AlertaGeneral('Error', null);
            });
        }

        function verCorreos() {
            corteExiste();
        }

        function getFile() {
            var url = '/CatInventario/ExportData';
            download(url);
        }

        function corteExiste(){
            var tipoMaq = cboFiltroTipoMaquinaria.val();
            tipoMaq = tipoMaq == '' || tipoMaq == undefined ? null : tipoMaq;
            $.get('/CatInventario/CorteInventarioEnviado', {
                tipo: tipoMaq
            }).always().then(response => {
                if(response.success) {
                    if (!response.Corte) {
                        btnMartes.show();
                    }
                    else {
                        btnMartes.hide();
                        // AlertaGeneral('Alerta', 'El corte ya se envió');
                    }
                }
                else {
                    btnMartes.hide();
                    AlertaGeneral('Alerta', response.message);
                }
                envioCorreosModal.modal('show');
            }, error => {
                btnMartes.hide();
                AlertaGeneral('Error', null);
            });
        }

        function download(url) {
            $.blockUI({ message: "Preparando archivo a descargar" });
            iframe = document.getElementById('iframeDownload');
            iframe.src = url;

            var timer = setInterval(function () {

                var iframeDoc = iframe.contentDocument || iframe.contentWindow.document;
                // Check if loading is complete
                if (iframeDoc.readyState == 'complete' || iframeDoc.readyState == 'interactive') {
                    setTimeout(function () {
                        $.unblockUI();
                    }, 5000);

                    clearInterval(timer);
                    return;
                }
            }, 1000);
        }

        function verHistorial() {
            ModalListaInventario.modal('show');
        }

        function verReporte() {

            $.blockUI({ message: mensajes.PROCESANDO });

            var idReporte = "43";

            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte;

            ireport.attr("src", path);

            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function LoadTabla() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/CatInventario/FillGridInventario",
                type: "POST",
                datatype: "json",
                data: { obj: getFiltrosObject() },
                success: function (response) {
                    $.unblockUI();

                    dataSet = response.rows;
                    iniciarGridInv(dataSet);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }


        function LoadTablaHist() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/CatInventario/GetInfoHistorial",
                type: "POST",
                datatype: "json",
                data: { idFecha: cboSemanas.val() },
                success: function (response) {
                    $.unblockUI();

                    dataSet = response.rows;
                    iniciarGridInv1(dataSet);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function iniciarGridInv1(dataSet) {
            tableDet2 = $('#tblHistorial').DataTable({
                //"dom": '<"top"i>rt<"bottom"flp><"clear">',
                "order": false,
                destroy: true,
                data: dataSet,
                columns: [
                    { data: "Economico", width: "80px" },
                    { data: "Tipo" /*width: "70px"*/ },
                    { data: "Descripcion" /*width: "100px"*/ },
                    { data: "Marca" /*width: "70px"*/ },
                    { data: "Modelo" /*width: "70px"*/ },
                    { data: "NumeroSerie" /*width: "100px"*/ },
                    { data: "Anio", width: "80px" },
                    { data: "HorometroAcumulado", width: "80px" },
                    { data: "Ubicacion" /*width: "150px"*/ },
                    { data: "Redireccion" /*width: "150px"*/ },
                    { data: "CargoObra" /*width: "150px"*/ },
                    { data: "Estatus" /*width: "150px"*/ }
                ],
                initComplete: function () {
                    this.api().columns().every(function () {
                        var column = this;
                        var select = $('<select class="form-control" style="color:black;width:100% !important;margin-left:auto;margin-right:auto;position:relative;text-align-last: center;"><option value="" selected>N/A</option></select>')
                            .appendTo($(column.header()).empty())
                            .on('change', function () {
                                var val = $.fn.dataTable.util.escapeRegex(
                                    $(this).val()
                                );

                                column
                                    .search(val ? '^' + val + '$' : '', true, false)
                                    .draw();
                            });
                        column.data().unique().sort().each(function (d, j) {
                            select.append('<option value="' + d + '">' + d + '</option>');
                        });
                    });
                },
                'bProcessing': true,
                "bPaginate": false,
                "sScrollX": "100%",
                "sScrollXInner": "130%",
                "bScrollCollapse": true,
                scrollY: '65vh',
                scrollCollapse: true,
                "bLengthChange": false,
                "searching": true,
                "bFilter": true,
                "bInfo": false,
                "bAutoWidth": false,
                columnDefs:
                    [
                        { orderable: true, targets: '_all' }
                    ]
            });
            tableDet2.on('draw', function () {
                tableDet2.columns().indexes().each(function (idx) {
                    var select = $(tableDet2.column(idx).header()).find('select');

                    if (select.val() === '') {
                        select
                            .empty()
                            .append('<option value="" selected>N/A</option>');

                        tableDet2.column(idx, { search: 'applied' }).data().unique().sort().each(function (d, j) {
                            select.append('<option value="' + d + '">' + d + '</option>');
                        });
                    }
                });
            });

        }

        function iniciarGridInv(dataSet) {
            tableDet = $('#grid_Inventario').DataTable({
                //"dom": '<"top"i>rt<"bottom"flp><"clear">',
                "order": false,
                destroy: true,
                data: dataSet,
                columns: [
                    { data: "Economico", width: "80px" },
                    { data: "Tipo" /*width: "70px"*/ },
                    { data: "Descripcion" /*width: "100px"*/ },
                    { data: "Marca" /*width: "70px"*/ },
                    { data: "Modelo" /*width: "70px"*/ },
                    { data: "NumeroSerie" /*width: "100px"*/ },
                    { data: "Anio", width: "80px" },
                    { data: "HorometroAcumulado", width: "80px" },
                    { data: "Ubicacion" /*width: "150px"*/ },
                    { data: "Redireccion" /*width: "150px"*/ },
                    { data: "CargoObra" /*width: "150px"*/ },
                    { data: "empresa" /*width: "150px"*/ },
                    { data: "Estatus" /*width: "150px"*/ }
                ],
                initComplete: function () {
                    this.api().columns().every(function () {
                        var column = this;
                        var select = $('<select class="form-control" style="color:black;width:100% !important;margin-left:auto;margin-right:auto;position:relative;text-align-last: center;"><option value="" selected>N/A</option></select>')
                            .appendTo($(column.header()).empty())
                            .on('change', function () {
                                var val = $.fn.dataTable.util.escapeRegex(
                                    $(this).val()
                                );

                                column
                                    .search(val ? '^' + val + '$' : '', true, false)
                                    .draw();
                            });
                        column.data().unique().sort().each(function (d, j) {
                            select.append('<option value="' + d + '">' + d + '</option>');
                        });
                    });
                },
                'bProcessing': true,
                "bPaginate": false,
                "sScrollX": "100%",
                "sScrollXInner": "130%",
                "bScrollCollapse": true,
                scrollY: '65vh',
                scrollCollapse: true,
                "bLengthChange": false,
                "searching": true,
                "bFilter": true,
                "bInfo": true,
                "bAutoWidth": false,
                columnDefs:
                    [
                        { orderable: true, targets: '_all' }
                    ]
            });
            tableDet.on('draw', function () {
                tableDet.columns().indexes().each(function (idx) {
                    var select = $(tableDet.column(idx).header()).find('select');

                    if (select.val() === '') {
                        select
                            .empty()
                            .append('<option value="" selected>N/A</option>');

                        tableDet.column(idx, { search: 'applied' }).data().unique().sort().each(function (d, j) {
                            select.append('<option value="' + d + '">' + d + '</option>');
                        });
                    }
                });
            });

        }

        function filtrarGrid() {

            loadGrid(getFiltrosObject(), ruta, gridResultado);
        }

        function getFiltrosObject() {
            return {
                idTipo: cboFiltroTipoMaquinaria.val().trim(),
                ListCC: cboCC.val(),
                noEconomico: tbEconomico.val()
            }
        }

        function initGrid() {
            gridResultado.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "verFichaTecnica": function (column, row) {
                        return "<button type='button' class='btn btn-primary verFicha'  data-idEconomico='" + row.idEconomico + "' data-Economico='" + row.Economico + "'>" +
                            "<span class='glyphicon glyphicon-share-alt'></span> " +
                            " </button>"
                            ;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                gridResultado.find(".verFicha").on("click", function (e) {
                    var idEconomico = $(this).attr('data-idEconomico')
                    var Economico = $(this).attr('data-Economico')
                    if ($(this).attr('data-idEconomico') != 0) {
                        window.location = "/Detalle/Index?idEconomico=" + idEconomico;
                    }
                });
            });
        }

        init();

    };

    $(document).ready(function () {
        maquinaria.catalogo.InventarioMaq = new InventarioMaq();
    });
})();


