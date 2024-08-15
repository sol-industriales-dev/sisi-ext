(() => {
    $.namespace('Maquinaria.ActivoFijo.EnviarCosto');
    EnviarCosto = function () {
        //-->Elementos
        const tblEnvioCosto = $('#tblEnvioCosto');
        const modalCapturaPoliza = $('#modalCapturaPoliza');
        //<--

        //--> Listeners
        modalCapturaPoliza.on('shown.bs.modal', function () {
            tblNuevaPoliza.DataTable().columns.adjust().draw();
        });
        //<--

        //--> Declaracion Variables
        let dtEnvioCosto;
        //<--

        //--> VISOR
        const report = $('#report');

        menuConfig = {
            lstOptions: [
                { text: `<i class="fa fa-download"></i> Descargar póliza` ,action:"descargarPoliza" ,fn: parametros => { reportePoliza(true, parametros.esCC, false, false, parametros.estatus, parametros.poliza, parametros.fechaPolizaCosto, true)} },
                { text: `<i class="fa fa-file"></i> Ver póliza` ,action:"visorPoliza" ,fn: parametros => { reportePoliza(true, parametros.esCC, false, false, parametros.estatus, parametros.poliza, parametros.fechaPolizaCosto, false)} },
                { text: `<i class="fa fa-download"></i> Descargar póliza detalle` ,action:"descargarPolizaDetalle" ,fn: parametros => { reportePoliza(false, parametros.esCC, true, false, parametros.estatus, parametros.poliza, parametros.fechaPolizaCosto, true)} },
                { text: `<i class="fa fa-file"></i> Ver póliza detalle` ,action:"visorPolizaDetalle" ,fn: parametros => { reportePoliza(false, parametros.esCC, true, false, parametros.estatus, parametros.poliza, parametros.fechaPolizaCosto, false)} },
                { text: `<i class="fa fa-download"></i> Descargar póliza firma` ,action:"descargarPolizaFirma" ,fn: parametros => { reportePoliza(false, parametros.esCC, true, true, parametros.estatus, parametros.poliza, parametros.fechaPolizaCosto, true)} },
                { text: `<i class="fa fa-file"></i> Ver póliza firma` ,action:"visorPolizaFirma" ,fn: parametros => { reportePoliza(false, parametros.esCC, true, true, parametros.estatus, parametros.poliza, parametros.fechaPolizaCosto, false)} },
            ]
        };
        //<--

        //--> Inicializacion
        (function init() {
            initTableEnvioCosto();
            CargarTablaEnvioCosto();
        })();
        //<--

        //--> Metodos
        function reportePoliza(esResumen, esCC, esPorHoja, esFirma, estatus, poliza, fecha, esVisor) {
            if(!esVisor){$.blockUI({ message: 'Procesando...' });}
            var path = '/Reportes/Vista.aspx?' +
                       'esDescargaVisor=' + esVisor +
                       '&esVisor=' + true +
                       '&idReporte=' + 64 + // 188 pruebas, 64 producción
                       '&isResumen=' + esResumen +
                       '&isCC=' + esCC +
                       '&isPorHoja=' + esPorHoja +
                       '&isFirma=' + esFirma +
                       '&Estatus=' + estatus +
                       '&icc=' + '001' +
                       '&fcc=' + 'S01' +
                       '&iPol=' + poliza +
                       '&fPol=' + poliza +
                       '&iPer=' + moment(fecha).format('MM/YYYY') +
                       '&fPer=' + moment(fecha).format('MM/YYYY') +
                       '&iTp=' + '03' +
                       '&fTp=' + '03' +
                       '&firma1=' + 'CP. Jessica Galdean' +
                       '&firma2=' + 'CP. Arturo Sánchez';
            report.attr('src', path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                $('#myModal').modal('show');
            };
        }

        function initTableEnvioCosto() {
            dtEnvioCosto = tblEnvioCosto.DataTable({
                ordering: false,
                searching: true,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollX: true,
                scrollY: '45vh',
                scrollCollapse: true,
                columns: [
                    { data: null, title: 'Opciones', className: 'dt-body-center'},
                    { data: 'numEconomico', title: '#Economico', className: 'dt-body-center' },
                    { data: 'depActual', title: 'Depreciación Actual', className: 'dt-body-right' },
                    { data: 'depFaltante', title: 'Depreciación Faltante', className: 'dt-body-right' },
                    { data: 'descripcion', title: 'Descripción', className: 'dt-body-center' },
                    { data: 'polizaBaja', title: 'Póliza Baja', className: 'dt-body-center' },
                    { data: 'polizaCosto', title: 'Póliza Costo', className: 'dt-body-center' }
                ],
                columnDefs: [
                    {
                        targets: [2, 3],
                        render: function(data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [0],
                        render: function(data, type, row) {
                            let dtBtnVerReportePoliza = '<button class="btn btn-success btnVisor" data-tipo_archivo="1"><i class="far fa-eye"></i> Poliza</button>';
                            return dtBtnVerReportePoliza;
                        }
                    }
                ],
                createdRow: function (row, data, dataIndex) {
                },
                drawCallback: function (settings) {
                },
                headerCallback: function (thead, data, start, end, display) {
                },
                footerCallback: function (tfoot, data, start, end, display) {
                },
                initComplete: function (settings, json) {
                    tblEnvioCosto.on('click', '.btnVisor', function() {
                        let dataRow = dtEnvioCosto.row($(this).closest('tr')).data();

                        menuConfig.parametros = {
                            esCC: true,
                            estatus: dataRow.status,
                            poliza: dataRow.poliza,
                            fecha: dataRow.fechaPolizaCosto
                        }
                        mostrarMenu();
                    });
                }
            });
        }

        function CargarTablaEnvioCosto() {
            $.get('/ActivoFijo/CargarTablaEnvioCosto', {
                enviado: true
            }).always().then(response => {
                if (response.success) {
                    AddRows(tblEnvioCosto, response.items);
                } else {
                    AlertaGeneral('Alerta', response.message);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            });
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }
    }
    //<--

    $(document).ready(() => {
        Maquinaria.ActivoFijo.EnviarCosto = new EnviarCosto();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();