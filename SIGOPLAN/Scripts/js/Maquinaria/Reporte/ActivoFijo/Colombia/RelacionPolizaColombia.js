(() => {
    $.namespace('Maquinaria.ActivoFijo.Colombia.RelacionPolizaColombia');
    RelacionPolizaColombia = function () {

        //#region selectores
        const cboxTipoActivo = $('#cboxTipoActivo');
        const tblActivos = $('#tblActivos');

        const modalRelaciones = $('#modalRelaciones');
        const tblPolizas = $('#tblPolizas');
        const btnGuardarRelacion = $('#btnGuardarRelacion');
        //#endregion

        //#region eventos
        cboxTipoActivo.on('change', function() {
            if ($(this).val()) {
                getActivosColombia($(this).val(), $(this).find('option:selected').data().prefijo);
            }
            else {
                tblActivos.DataTable().clear().draw();
            }
        });
        //#endregion

        //#region tablas
        function addRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function initTablas(){
            tblActivos.DataTable({
                order: [[0, 'asc']],
                ordering: true,
                searching: true,
                info: true,
                language: dtDicEsp,
                paging: true,
                scrollX: false,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],
                // fixedColumns: {
                //     leftColumns: 3
                // },

                columns: [
                    { data: null, title: 'Opciones' },
                    { data: 'numeroEconomico', title: '# ecónomico' },
                    { data: 'descripcion', title: 'Descripción' }
                ],

                columnDefs: [
                    {
                        targets: [0],
                        render: function(data, type, row) {
                            const btnVerRelacion = '<button class="btn btn-primary btnVerRelacion" title="Ver pólias"><i class="far fa-eye"></i></button>';
                            return btnVerRelacion;
                        }
                    },
                    {
                        targets: [1],
                        className: 'text-center'
                    }
                ],

                initComplete: function(settings, json) {
                    tblActivos.on('click', '.btnVerRelacion', function() {
                        const rowData = tblActivos.DataTable().row($(this).closest('tr')).data();

                        getRelacionPoliza(rowData.idActivo, rowData.esMaquina);
                    });
                },
                createdRow: function(row, data, dataIndex) {},
                drawCallback: function(settings) {},
                headerCallback: function(thead, data, start, end, display) {},
                footerCallback: function(tfoot, data, start, end, display) {}
            });

            tblPolizas.DataTable({
                order: [[0, 'asc']],
                ordering: false,
                searching: false,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollX: false,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],
                // fixedColumns: {
                //     leftColumns: 3
                // },

                columns: [
                    { data: null, title: 'Opciones' },
                    { data: 'tm', title: 'TM' },
                    { data: 'factura', title: 'Factura' },
                    { data: 'concepto', title: 'Concepto' },
                    { data: 'poliza', title: 'Póliza' },
                    { data: 'cuenta', title: 'Cuenta' },
                    { data: 'monto', title: 'Monto' },
                    { data: 'fechaMovimiento', title: 'Fecha movimiento' },
                    { data: 'fechaInicioDep', title: 'Inicio de dep' },
                    { data: 'porcentajeDep', title: '% dep' },
                    { data: 'mesesDep', title: 'Meses a dep' },
                    { data: 'polizaRelacion', title: 'Póliza relacionada' }
                ],

                columnDefs: [
                ],

                initComplete: function(settings, json) {},
                createdRow: function(row, data, dataIndex) {},
                drawCallback: function(settings) {},
                headerCallback: function(thead, data, start, end, display) {},
                footerCallback: function(tfoot, data, start, end, display) {}
            });
        }
        //#endregion

        //#region backend
        function initCombos() {
            cboxTipoActivo.fillCombo('/ActivoFijo/TipoActivoColombiaCBox', null, false, null, null);
        }

        function getActivosColombia(tipoActivo, esMaquina) {
            $.get('/ActivoFijo/GetActivosColombia',
            {
                tipoActivo,
                esMaquina
            }).then(response => {
                if (response.success) {
                    addRows(tblActivos, response.items);
                }
                else {
                    swal('Alerta!', response.message, 'warning');
                }
            }, error => {
                swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
            });
        }

        function getRelacionPoliza(idActivo, esMaquina) {
            $.get('/ActivoFijo/GetRelacionPoliza',
            {
                idActivo,
                esMaquina
            }).then(response => {
                if (response.success) {
                    modalRelaciones.modal('show');
                    addRows(tblPolizas, response.items);
                }
                else {
                    swal('Alerta!', response.message, 'warning');
                }
            }, error => {
                swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
            });
        }
        //#endregion

        (function init() {
            initCombos();
            initTablas();
        })();
    }
    $(document).ready(() => Maquinaria.ActivoFijo.Colombia.RelacionPolizaColombia = new RelacionPolizaColombia())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();