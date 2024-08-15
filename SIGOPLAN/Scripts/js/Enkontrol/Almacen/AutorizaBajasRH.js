(() => {
    $.namespace('Enkontrol.Almacen.Almacen.AutorizaBajasRH');
    AutorizaBajasRH = function () {
        //#region Selectores
        const tblEmpleadosPendientes = $('#tblEmpleadosPendientes');
        const btnGuardar = $('#btnGuardar');
        //#endregion

        (function init() {
            initTableEmpleadosPendientes();

            cargarEmpleados();

            btnGuardar.click(guardarBajas);
        })();

        function initTableEmpleadosPendientes() {
            tblEmpleadosPendientes.DataTable({
                retrieve: true,
                paging: false,
                deferRender: true,
                // searching: false,
                language: dtDicEsp,
                bInfo: false,
                order: [0, 'desc'],
                initComplete: function (settings, json) {
                    tblEmpleadosPendientes.on('change', '.checkBoxBaja', function () {
                        tblEmpleadosPendientes.DataTable().row($(this).closest('tr')).data().estatus = $(this).is(':checked');
                    });

                    tblEmpleadosPendientes.on('change', '.inputComentario', function () {
                        tblEmpleadosPendientes.DataTable().row($(this).closest('tr')).data().comentario = $(this).val();
                    });
                },
                columns: [
                    { data: 'clave_empleado', title: 'Empleado' },
                    { data: 'nombreEmpleado', title: 'Nombre Empleado' },
                    { data: 'rfc', title: 'RFC' },
                    { data: 'cc', title: 'CC' },
                    { data: 'ccDesc', title: 'Descripción' },
                    { data: 'puestoDesc', title: 'Puesto' },
                    {
                        title: 'Comentario', render: function (data, type, row, meta) {
                            return `<input class="form-control inputComentario">`;
                        }
                    },
                    {
                        title: 'Baja', render: function (data, type, row, meta) {
                            let div = document.createElement('div');
                            let checkbox = document.createElement('input');

                            checkbox.id = 'checkboxBaja_' + meta.row;
                            checkbox.setAttribute('type', 'checkbox');
                            checkbox.classList.add('form-control');
                            checkbox.classList.add('regular-checkbox');
                            checkbox.classList.add('checkBoxBaja');
                            checkbox.style.height = '25px';

                            let label = document.createElement('label');
                            label.setAttribute('for', checkbox.id);

                            $(div).append(checkbox);
                            $(div).append(label);

                            return div.outerHTML;
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', targets: [0, 2, 3, 7] },
                ]
            });
        }

        function cargarEmpleados() {
            $.post('/Enkontrol/Almacen/GetEmpleadosPendientesLiberacion').then(response => {
                if (response.success) {
                    AddRows(tblEmpleadosPendientes, response.data);
                } else {
                    AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }

        function guardarBajas() {
            let empleados = [];

            tblEmpleadosPendientes.DataTable().rows().data().each(function (rowData) {
                // let rowData = tblEmpleadosPendientes.DataTable().row(row).data();
                // let checkBoxBaja = $(row).find('.checkBoxBaja');

                // if (checkBoxBaja.prop('checked')) {
                if (rowData.estatus) {
                    empleados.push(rowData);
                }
            });

            if (empleados.length > 0) {
                $.post('/Enkontrol/Almacen/GuardarBajas', { empleados }).then(response => {
                    if (response.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarEmpleados();
                    } else {
                        AlertaGeneral(`Alerta`, `Error al guardar la información.`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
            }
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }
    }
    $(document).ready(() => Enkontrol.Almacen.Almacen.AutorizaBajasRH = new AutorizaBajasRH())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();