(() => {
    $.namespace('Administrativo.Requerimientos.CentroCostoDivision');

    CentroCostoDivision = function () {
        //#region Selectores
        const tablaCentroCosto = $('#tablaCentroCosto');
        //#endregion

        let dtCentroCosto;
        let _pageScrollPos = 0;

        (function init() {
            initTablaCentroCosto();
        })();

        async function initTablaCentroCosto() {
            let listaDivisiones = [];
            let listaLineasNegocio = [];

            await new Promise(function (resolve) {
                resolve(axios.post('/Administrativo/Requerimientos/GetDivisionesCombo'));
            }).then((response) => {
                listaDivisiones = response.data.items;
            });

            await new Promise(function (resolve) {
                resolve(axios.post('/Administrativo/Requerimientos/GetLineaNegocioCombo', { division: 0 }));
            }).then((response) => {
                listaLineasNegocio = response.data.items;
            });

            dtCentroCosto = tablaCentroCosto.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '50vh',
                scrollCollapse: true,
                preDrawCallback: function (settings) {
                    _pageScrollPos = $('div.dataTables_scrollBody').scrollTop();
                },
                drawCallback: function (settings) {
                    $('div.dataTables_scrollBody').scrollTop(_pageScrollPos);
                },
                initComplete: function (settings, json) {
                    tablaCentroCosto.on('change', '.selectDivision, .selectLineaNegocio', function () {
                        let row = $(this).closest('tr');
                        let rowData = dtCentroCosto.row(row).data();
                        let grupo = rowData.grupo;
                        let empresa = rowData.empresa;
                        let division = +($(row).find('.selectDivision').val());
                        let lineaNegocio_id = +($(row).find('.selectLineaNegocio').val());

                        guardarRelacionCentroCostoDivision(grupo, empresa, division, lineaNegocio_id);
                    });
                },
                createdRow: function (row, rowData) {
                    let selectDivision = $(row).find('.selectDivision');
                    let division = rowData.division > 0 ? rowData.division : '';

                    selectDivision.fillCombo({ items: listaDivisiones }, null, false, null); // selectDivision.fillCombo('/Administrativo/Requerimientos/GetDivisionesCombo', null, false, null);
                    selectDivision.find('option[value="' + division + '"]').attr('selected', true);

                    let selectLineaNegocio = $(row).find('.selectLineaNegocio');
                    let lineaNegocio_id = rowData.lineaNegocio_id > 0 ? rowData.lineaNegocio_id : '';

                    selectLineaNegocio.fillCombo({ items: listaLineasNegocio.filter(x => +x.Prefijo == +division) }, null, false, null); // selectLineaNegocio.fillCombo('/Administrativo/Requerimientos/GetLineaNegocioCombo', { division: +division }, false, null);
                    selectLineaNegocio.find('option[value="' + lineaNegocio_id + '"]').attr('selected', true);
                },
                columns: [
                    {
                        title: 'Centro de Costo', render: function (data, type, row, meta) {
                            return row.descripcion;
                        }
                    },
                    {
                        title: 'División', render: function (data, type, row, meta) {
                            return `<select class="form-control selectDivision"></select>`;
                        }
                    },
                    {
                        title: 'Linea de Negocio', render: function (data, type, row, meta) {
                            return `<select class="form-control selectLineaNegocio"></select>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });

            cargarRelacionCentroCostoDivision();
        }

        function cargarRelacionCentroCostoDivision() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/GetRelacionCentroCostoDivision').always($.unblockUI).then(response => {
                if (response.success) {
                    AddRows(tablaCentroCosto, response.data);
                } else {
                    AlertaGeneral(`Alerta`, response.message);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }

        function guardarRelacionCentroCostoDivision(grupo, empresa, division, lineaNegocio_id) {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/GuardarRelacionCentroCostoDivision', { grupo, empresa, division, lineaNegocio_id }).always($.unblockUI).then(response => {
                if (response.success) {
                    Alert2Exito('Se ha guardado la información.');
                    cargarRelacionCentroCostoDivision();
                } else {
                    AlertaGeneral(`Alerta`, response.message);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }

        function AddRows(tbl, lst) {
            tbl.DataTable().clear().rows.add(lst).draw(false);
        }
    }

    $(document).ready(() => Administrativo.Requerimientos.CentroCostoDivision = new CentroCostoDivision())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();