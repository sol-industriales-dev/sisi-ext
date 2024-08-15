(() => {
    $.namespace('Maquinaria.Reportes.ActivoFijo.AltasSubCuentas');
    AltasSubCuentas = function () {
        //#region Selectores
        const tblCuentas = $('#tblCuentas');
        const tblSubCuentas = $('#tblSubCuentas');
        const modalSubCuentas = $('#modalSubCuentas');
        const guardarSubCuentas = $('#guardarSubCuentas');
        //#endregion

        (function init() {
            initTableCuentas();
            initTableSubCuentas();

            cargarCuentas();
        })();

        function initTableCuentas() {
            tblCuentas.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                scrollCollapse: true,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblCuentas.on('click', '.btnSubCuentas', function () {
                        let rowData = tblCuentas.DataTable().row($(this).closest('tr')).data();

                        $.post('GetSubCuentas', { cuenta: rowData.Cuenta }).then(response => {
                            if (response.success) {
                                AddRows(tblSubCuentas, response.data);
                            } else {
                                AlertaGeneral(`Alerta`, `Error al consultar la información de las sub cuentas.`);
                            }
                        }, error => {
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        }
                        );

                        modalSubCuentas.modal('show');
                    });
                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'Cuenta', title: 'Cuenta' },
                    { data: 'Descripcion', title: 'Descripción' },
                    { data: 'MesesDeDepreciacion', title: 'Meses Depreciación' },
                    { data: 'FechaCreacion', title: 'Fecha Creación' },
                    {
                        title: 'Sub-Cuentas', render: (data, type, row, meta) => {
                            let button = document.createElement('button');
                            let i = document.createElement('i');

                            button.classList.add('btn', 'btn-primary', 'btnSubCuentas');
                            i.classList.add('fa', 'fa-align-justify');

                            $(button).append(i);

                            return button.outerHTML;
                        }
                    },
                ],
                columnDefs: [
                    {
                        "render": function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                            }
                        },
                        "targets": [3]
                    },
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTableSubCuentas() {
            tblSubCuentas.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                scrollCollapse: true,
                bInfo: false,
                initComplete: function (settings, json) {

                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'Anio', title: 'Año' },
                    {
                        data: 'Subcuenta', title: 'Sub Cuenta', function(data, type, row, meta) {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputSubCuenta');
                            $(input).attr('value', data);

                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'SubSubcuenta', title: 'Sub Sub Cuenta', function(data, type, row, meta) {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputSubSubCuenta');
                            $(input).attr('value', data);

                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'EsOverhaul', title: 'Overhaul', function(data, type, row, meta) {
                            let select = `
                                        <select class="form-control selectOverhaul">
                                            <option value="1">Sí</option>
                                            <option value="0">No</option>
                                        </select>`;
                            $(select).val(data == 1);

                            return select;
                        }
                    },
                    {
                        data: 'EsCuentaDepreciacion', title: 'Cuenta Depreciación', function(data, type, row, meta) {
                            let html = `
                                        <select class="form-control selectCuentaDepreciacion" style="width: 25%;">
                                            <option value="1">Sí</option>
                                            <option value="0">No/option>
                                        </select>
                                        <input class="form-control inputCuentaDepreciacion" style="width: 70%;">`;

                            $(html).find('select').val(data == 1);

                            return html;
                        }
                    },
                    {
                        data: 'PorcentajeDepreciacion', title: 'Porcentaje Depreciación', function(data, type, row, meta) {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputPorcentajeDepreciacion');
                            $(input).attr('value', data);

                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'MesesMaximoDepreciacion', title: 'Meses Máximo', function(data, type, row, meta) {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputMesesMaximo');
                            $(input).attr('value', data);

                            return input.outerHTML;
                        }
                    },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarCuentas() {
            $.post('/ActivoFijo/GetCuentas').then(response => {
                if (response.success) {
                    AddRows(tblCuentas, response.data);
                } else {
                    AlertaGeneral(`Alerta`, `Error al consultar la información de las cuentas. ${response.message}`);
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
    $(document).ready(() => Maquinaria.Reportes.ActivoFijo.AltasSubCuentas = new AltasSubCuentas())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();