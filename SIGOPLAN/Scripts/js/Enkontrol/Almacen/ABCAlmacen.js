(() => {
    $.namespace('Enkontrol.Almacen.Almacen.ABCAlmacen');
    ABCAlmacen = function () {
        //#region Selectores
        const fieldsetFiltros = $('#fieldsetFiltros');
        const btnQuitarFiltros = $('#btnQuitarFiltros');
        const inputFiltroAlmacen = $('#inputFiltroAlmacen');
        const inputFiltroAlmacenDesc = $('#inputFiltroAlmacenDesc');
        const inputFiltroDireccion = $('#inputFiltroDireccion');
        const inputFiltroResponsable = $('#inputFiltroResponsable');
        const inputFiltroTelefono = $('#inputFiltroTelefono');
        const inputFiltroValidaAlmacen = $('#inputFiltroValidaAlmacen');
        const inputFiltroPTerminado = $('#inputFiltroPTerminado');
        const inputFiltroMPrima = $('#inputFiltroMPrima');
        const tblAlmacen = $('#tblAlmacen');
        const btnNuevoAlmacen = $('#btnNuevoAlmacen');
        const inputTabDGAlmacen = $('#inputTabDGAlmacen');
        const inputTabDGValidarCC = $('#inputTabDGValidarCC');
        const inputTabDGCC = $('#inputTabDGCC');
        const inputTabDGCCDesc = $('#inputTabDGCCDesc');
        const inputTabDGDescripcion = $('#inputTabDGDescripcion');
        const inputTabDGDireccion = $('#inputTabDGDireccion');
        const inputTabDGResponsable = $('#inputTabDGResponsable');
        const inputTabDGTelefono = $('#inputTabDGTelefono');
        const inputTabDGPTerminado = $('#inputTabDGPTerminado');
        const inputTabDGMPrima = $('#inputTabDGMPrima');
        const inputTabDGPAlmacenVirtual = $('#inputTabDGPAlmacenVirtual');
        const btnGuardarAlmacen = $('#btnGuardarAlmacen');
        //#endregion

        //#region Eventos
        btnNuevoAlmacen.on('click', function () {
            limpiarDatosGenerales();
            HabilitandoInputs();
            let datos = tblAlmacen.DataTable().rows().data();
            let dataUltimoAlmacen = datos[datos.length - 1];

            if (dataUltimoAlmacen.almacen > 0) {
                datos.push({
                    'almacen': 0,
                    'cc': '',
                    'descripcion': '',
                    'direccion': '',
                    'responsable': '',
                    'telefono': '',
                    'valida_almacen_cc': '',
                    'bit_pt': '',
                    'bit_mp': ''
                });

                tblAlmacen.DataTable().clear();
                tblAlmacen.DataTable().rows.add(datos).draw();
                tblAlmacen.DataTable().page('last').draw('page');

                tblAlmacen.DataTable().$('tr.selected').removeClass('selected');
                tblAlmacen.find('tbody tr:last').addClass("selected");

                limpiarDatosGenerales();
                inputTabDGAlmacen.attr('data-id', 0);
                checkBoxProductoTerminado.prop('checked', true);
                checkBoxMateriaPrima.prop('checked', true);
                checkBoxComprasRequisiciones.prop('checked', true);
            } else {
                tblAlmacen.DataTable().page('last').draw('page');
                tblAlmacen.DataTable().$('tr.selected').removeClass('selected');
                tblAlmacen.find('tbody tr:last').addClass("selected");
            }
        });

        btnGuardarAlmacen.on('click', function () {

            if (inputTabDGAlmacen.attr('data-id') == 0) {
                guardarAlmacen(setDatosAlmacen());
            } else {
                EditarAlmacen(setDatosAlmacen());
            }
        });

        fieldsetFiltros.on('keyup change', 'input', function () {
            tblAlmacen.DataTable().search('');
            tblAlmacen.DataTable().column($(this).data('columnIndex')).search(this.value).draw();
        });
        //#endregion

        (function init() {
            inputTabDGAlmacen.attr('data-id', 0);
            initTablas();
            getAlmacenes();

            inputTabDGCC.fillCombo('FillComboCCTodos', null, false);
            $(document).ready(function () {
                $('#inputTabDGCC').select2();
            });
            btnQuitarFiltros.click(limpiarFiltros);
            inputTabDGAlmacen.change(function () {
                ObtenerAlmacenEditaroAgregar();
            });
        })();

        function ObtenerAlmacenEditaroAgregar() {
            let almacen = inputTabDGAlmacen.val();
            axios.post('ObtenerAlmacenEditaroAgregar', { almacen: almacen })
                .catch(o_O => AlertaGeneral(o_O.message))
                .then(response => {
                    let { success, items } = response.data.data;
                    if (success) {
                        llenarInputs(response.data.data.items[0])
                        DesabilitandoInputs();
                        mensajeDesahabilitar();
                    } else {
                        HabilitandoInputs();
                        QuitarValoresInputs();
                    }
                });
        }
        function llenarInputs(params) {
            console.log(params)

            let bitpt = params.bit_pt == 'S' ? true : false;
            let bitmp = params.bit_mp == 'S' ? true : false;
            let valida_almacen_cc = params.valida_almacen_cc == 'S' ? true : false;
            inputTabDGAlmacen.attr('data-id', params.almacen);
            inputTabDGAlmacen.val(params.almacen);
            inputTabDGValidarCC.val(params.valida_almacen_cc);
            inputTabDGCC.val(params.cc);
            inputTabDGCC.trigger("change");

            inputTabDGDescripcion.val(params.descripcion);
            inputTabDGDireccion.val(params.direccion);
            inputTabDGResponsable.val(params.responsable);
            inputTabDGTelefono.val(params.telefono);
            inputTabDGPTerminado.prop('checked', bitpt);
            inputTabDGMPrima.prop('checked', bitmp);
            inputTabDGPAlmacenVirtual.prop('checked', valida_almacen_cc);
        }
        function QuitarValoresInputs() {
            inputTabDGAlmacen.attr('data-id', 0);
            inputTabDGValidarCC.val('');
            inputTabDGCC.val('');
            inputTabDGCC.trigger("change");
            inputTabDGDescripcion.val('');
            inputTabDGDireccion.val('');
            inputTabDGResponsable.val('');
            inputTabDGTelefono.val('');
            inputTabDGPTerminado.prop('checked', false);
            inputTabDGMPrima.prop('checked', false);
            inputTabDGPAlmacenVirtual.prop('checked', false);
        }
        function mensajeDesahabilitar() {
            let strMensaje = "¿este almacen ya esta dado de alta quiere editarlo?";
            Swal.fire({
                position: "center",
                icon: "warning",
                title: "¡Cuidado!",
                width: '35%',
                showCancelButton: true,
                html: `<h3>${strMensaje}</h3>`,
                confirmButtonText: "Confirmar",
                confirmButtonColor: "#5cb85c",
                cancelButtonText: "Cancelar",
                cancelButtonColor: "#d9534f",
                showCloseButton: true
            }).then((result) => {
                if (result.value) {
                    HabilitandoInputs();
                }
            });
        }

        function DesabilitandoInputs() {
            inputTabDGValidarCC.prop('disabled', 'true');
            inputTabDGCC.prop('disabled', 'true');
            inputTabDGDescripcion.prop('disabled', 'true');
            inputTabDGDireccion.prop('disabled', 'true');
            inputTabDGResponsable.prop('disabled', 'true');
            inputTabDGTelefono.prop('disabled', 'true');
        }
        function HabilitandoInputs() {
            inputTabDGValidarCC.prop('disabled', false);
            inputTabDGCC.prop('disabled', false);
            inputTabDGDescripcion.prop('disabled', false);
            inputTabDGDireccion.prop('disabled', false);
            inputTabDGResponsable.prop('disabled', false);
            inputTabDGTelefono.prop('disabled', false);
        }
        //#region Inits
        function initTablas() {
            tblAlmacen.DataTable({
                order: [[0, 'asc']],
                ordering: false,
                searching: true,
                info: true,
                language: dtDicEsp,
                paging: true,
                scrollX: false,
                scrollCollapse: false,
                dom: 'Brtp',
                buttons: [{ extend: 'excelHtml5', exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7] }, className: 'btn btn-default' }],
                bInfo: false,
                bLengthChange: false,
                deferRender: true,
                retrieve: true,
                orderCellsTop: true,
                columns: [
                    { data: 'almacen', title: 'Almacén', className: 'text-right' },
                    { data: 'cc', title: 'cc', visible: false, className: 'text-right' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'direccion', title: 'Dirección' },
                    { data: 'responsable', title: 'Responsable' },
                    { data: 'telefono', title: 'Teléfono' },
                    { data: 'valida_almacen_cc', title: 'Valida Almacén_CC', className: 'text-center' },
                    { data: 'bit_pt', title: 'P. terminado', className: 'text-center' },
                    { data: 'bit_mp', title: 'M. prima', className: 'text-center' },
                    {
                        data: 'botonEliminar', render: (data, type, row, meta) => {
                            let html = ``;
                            if (data == false) {
                                html = `<button class="btn btn-danger Eliminar" data-id="${row.almacen}"><i class="far fa-trash-alt"></i></button>`;
                            } else {
                                html = ``;
                            }
                            return html;
                        }
                    },
                ],
                columnDefs: [
                    {
                        targets: '_all',
                        className: 'text-nowrap'
                    }
                ],
                headerCallback: function (thead, data, start, end, display) {
                    $(thead).parent().children().addClass('bg-table-header');
                    $(thead).children().addClass('text-center');
                }
                , initComplete: function (settings, json) {
                    tblAlmacen.on("click", ".Eliminar", function () {
                        let strMensaje = "¿esta seguro que desea eliminar este almacen?";
                        Swal.fire({
                            position: "center",
                            icon: "warning",
                            title: "¡Cuidado!",
                            width: '35%',
                            showCancelButton: true,
                            html: `<h3>${strMensaje}</h3>`,
                            confirmButtonText: "Confirmar",
                            confirmButtonColor: "#5cb85c",
                            cancelButtonText: "Cancelar",
                            cancelButtonColor: "#d9534f",
                            showCloseButton: true
                        }).then((result) => {
                            if (result.value) {
                                fncEliminar($(this).attr("data-id"));
                            }
                        });
                    });

                    tblAlmacen.on('click', 'td', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblAlmacen.DataTable().row(row).data();

                        if (row.hasClass('selected')) {
                            row.removeClass('selected');
                        } else {
                            tblAlmacen.DataTable().$('tr.selected').removeClass('selected');
                            row.addClass('selected');
                        }

                        getInformacionRow(row, rowData);
                    });

                }

            });

            tblAlmacen.DataTable().buttons().container().appendTo($('#divTablaAlmacen'));
        }
        //#endregion
        function getInformacionRow(row, rowData) {
            HabilitandoInputs();
            console.log(row)
            console.log(rowData)
            let bitpt = rowData.bit_pt == 'S' ? true : false;
            let bitmp = rowData.bit_mp == 'S' ? true : false;
            let valida_almacen_cc = rowData.valida_almacen_cc == 'S' ? true : false;
            $('#inputTabDGAlmacen').prop('disabled', 'true');
            console.log(bitpt)
            console.log(bitmp)
            inputTabDGAlmacen.attr('data-id', rowData.almacen);
            inputTabDGAlmacen.val(rowData.almacen);
            inputTabDGValidarCC.val(rowData.valida_almacen_cc);
            inputTabDGCC.val(rowData.cc);
            inputTabDGCC.trigger("change");

            inputTabDGDescripcion.val(rowData.descripcion);
            inputTabDGDireccion.val(rowData.direccion);
            inputTabDGResponsable.val(rowData.responsable);
            inputTabDGTelefono.val(rowData.telefono);
            inputTabDGPTerminado.prop('checked', bitpt);
            inputTabDGMPrima.prop('checked', bitmp);
            inputTabDGPAlmacenVirtual.prop('checked', valida_almacen_cc);
        }

        function fncEliminar(almacen) {
            axios.post('EliminarAlmacen', { almacen: almacen })
                .catch(o_O => AlertaGeneral(o_O.message))
                .then(response => {
                    let { success, ITEMS } = response.data;
                    if (success) {
                        console.log(response)
                        getAlmacenes();
                    }
                });
        }
        //#region Funciones generales
        function addRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function limpiarDatosGenerales() {
            $('#inputTabDGAlmacen').prop('disabled', false);
            inputTabDGAlmacen.attr('data-id', 0);
            inputTabDGAlmacen.val('');
            inputTabDGValidarCC.val('');
            inputTabDGCC.val('');
            inputTabDGCC.trigger("change");

            inputTabDGCCDesc.val('');
            inputTabDGDescripcion.val('');
            inputTabDGDireccion.val('');
            inputTabDGResponsable.val('');
            inputTabDGTelefono.val('');
            inputTabDGPTerminado.prop('checked', false);
            inputTabDGMPrima.prop('checked', false);
            inputTabDGPAlmacenVirtual.prop('checked', false);
        }

        function setDatosAlmacen() {
            const almacen = {
                almacen: inputTabDGAlmacen.val(),
                descripcion: inputTabDGDescripcion.val(),
                direccion: inputTabDGDireccion.val(),
                responsable: inputTabDGResponsable.val(),
                telefono: inputTabDGTelefono.val(),
                valida_almacen_cc: inputTabDGValidarCC.val(),
                bit_pt: inputTabDGPTerminado.prop('checked') ? 'S' : 'N',
                bit_mp: inputTabDGMPrima.prop('checked') ? 'S' : 'N',
                cc: inputTabDGCC.val(),
                almacen_virtual: inputTabDGPAlmacenVirtual.prop('checked') ? 1 : 0
            }

            return almacen;
        }

        function limpiarFiltros() {
            inputFiltroAlmacen.val('');
            inputFiltroAlmacenDesc.val('');
            inputFiltroDireccion.val('');
            inputFiltroResponsable.val('');
            inputFiltroTelefono.val('');
            inputFiltroValidaAlmacen.val('');
            inputFiltroPTerminado.val('');
            inputFiltroMPrima.val('');

            tblAlmacen.DataTable().columns().search('').draw();
        }
        //#endregion

        //#region Servidor
        function getAlmacenes() {
            $.get('/Enkontrol/Almacen/GetAlmacenes', {}).then(response => {
                if (response.success) {
                    console.log('response');
                    console.log(response);
                    addRows(tblAlmacen, response.items);
                } else {
                    swal("Alerta!", response.message, "warning");
                }
            }, error => {
                swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
            });
        }

        function guardarAlmacen(datos) {
            $.post('/Enkontrol/Almacen/GuardarAlmacen',
                {
                    datos
                }).then(response => {
                    if (response.success) {

                        limpiarDatosGenerales();
                        getAlmacenes();
                    } else {
                        swal("Alerta!", response.message, "warning");
                    }
                }, error => {
                    swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                });
        }

        function EditarAlmacen(datos) {
            $.post('/Enkontrol/Almacen/EditarAlmacen',
                {
                    datos
                }).then(response => {
                    if (response.success) {

                        limpiarDatosGenerales();
                        getAlmacenes();
                    } else {
                        swal("Alerta!", response.message, "warning");
                    }
                }, error => {
                    swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                });
        }


        //#endregion
    }
    $(document).ready(() => Enkontrol.Almacen.Almacen.ABCAlmacen = new ABCAlmacen())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();