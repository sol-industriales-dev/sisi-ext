(() => {
    $.namespace('ComparativoExistenciasInventario.Almacen');

    const tblTabla = $('#tblComparativoExistenciasInventario');
    const tblAlmacen = $('#tblAlmacen');
    let dtAlmacen;
    let dtTabla;
    let fechaActualr = new Date;
    const inputFecha = $('#inputFecha');
    const btnBuscar = $('#btnBuscar');
    const inputAlmacenNum = $('#inputAlmacenNum');
    const rdCostoPromedio = $('#rdCostoPromedio');
    const rdUltimoPrecio = $('#rdUltimoPrecio');
    const checkSoloConDiferencia = $('#checkSoloConDiferencia');
    const checkConcentrado = $('#checkConcentrado');
    const inputInsumoInicio = $('#inputInsumoInicio');
    const inputInsumoFin = $('#inputInsumoFin');
    const btnReporte = $('#btnReporte');
    const btnCierreInventario = $('#btnCierreInventario');
    const btnBuscarAlmacen = $('#btnBuscarAlmacen');
    const inputAlmacenDescripcion = $('#inputAlmacenDescripcion');

    const inputInsumoInicioDescripcion = $('#inputInsumoInicioDescripcion');
    const inputInsumoFinDescripcion = $('#inputInsumoFinDescripcion');
    const report = $('#report');

    const difNumero = $('#difNumero');
    const difPorcentaje = $("#difPorcentaje");
    const exactitudNumero = $("#exactitudNumero");
    const exactitudPorcentaje = $("#exactitudPorcentaje");
    const totalNumero = $("#totalNumero");
    const totalPorcentaje = $("#totalPorcentaje");

    const difAbono = $('#difAbono');
    const difCargo = $('#difCargo');
    const difTotal = $('#difTotal');


    Almacen = function () {
        let init = () => {
            initDataTblComparativoExistencia();
            getObtenerFecha();
            botones();
            initTablaAlmacen();
            consultaInicioInsumo(0);
            consultaFinInsumo(0);
        }
        init();
    }

    function botones() {
        btnBuscar.click(function () {
            getComparativoExistenciasinvitentario();
        });
        btnReporte.click(function () {
            ObtenerExistenciaInventarioreporte();
        });
        btnCierreInventario.click(() => {
            Alert2AccionConfirmar('', `¿Desea cerrar el inventario del almacén "${inputAlmacenDescripcion.val()}" al día "${inputFecha.val()}"?`, 'Aceptar', 'Cancelar', cierreInventario, 'warning');
        });
        btnBuscarAlmacen.click(function () {
            getObtenerAlmacenes();
        });
        inputAlmacenNum.change(function () {
            obtenerAlmacenxID();
        })
        inputInsumoInicio.change(function () {
            consultaInicioInsumo(inputInsumoInicio.val());
        });
        inputInsumoFin.change(function () {
            consultaFinInsumo(inputInsumoFin.val());
        });
    }
    function initTablaAlmacen() {
        dtAlmacen = tblAlmacen.DataTable({
            ordering: false
            , paging: true
            , ordering: true
            , searching: false
            , bFilter: true
            , info: false
            , columns: [
                { data: 'almacen', title: 'almacen' },
                { data: 'descripcion', title: 'descripcion' },
                {
                    title: 'Acciones', render: (data, type, row, meta) => {
                        return html = `<button class="btn btn-primary seleccionar"><i class="fas fa-arrow-right"></i></button>`;
                    }
                },
            ]
            , initComplete: function (settings, json) {
                tblAlmacen.on('click', '.seleccionar', function () {
                    const rowData = dtAlmacen.row($(this).closest("tr")).data();
                    $('#inputAlmacenNum').val(rowData.almacen);
                    $('#inputAlmacenDescripcion').val(rowData.descripcion);
                    $('#mdlObtenerAlmacen').modal('hide');
                });

            }
        });
    }

    function initDataTblComparativoExistencia() {
        dtTabla = tblTabla.DataTable({
            ordering: true
            , paging: true
            , searching: false
            , bFilter: true
            , info: false
            , language: dtDicEsp
            // , dom: '<t>Bp'
            , dom: 'Btp'
            , buttons: [{ extend: 'excelHtml5', exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7, 8] }, className: 'btn btn-xs btn-default botonExcelExistencias', title: null }]
            , columns: [
                { data: 'insumo', title: 'Insumo' },
                { data: 'categoria', title: 'Categoría' },
                { data: 'ubicacion', title: 'Ubicación' },
                {
                    data: 'fisica', title: 'Física', render: function (data, type, row, meta) {
                        return parseFloat(data).toFixed(6);
                    }
                },
                {
                    data: 'teorica', title: 'Teórica', render: function (data, type, row, meta) {
                        return parseFloat(data).toFixed(6);
                    }
                },
                {
                    data: 'diferencia', title: 'Diferencia', render: function (data, type, row, meta) {
                        return parseFloat(data).toFixed(6);
                    }
                },
                {
                    data: 'promedioOPrecio', title: 'Costo Promedio', render: function (data, type, row, meta) {
                        if (type === 'display') {
                            return maskNumero6D(data);
                        } else {
                            return +data;
                        }
                    }
                },
                {
                    data: 'cargos', title: 'Cargos', render: function (data, type, row, meta) {
                        if (type === 'display') {
                            return maskNumero6D(data);
                        } else {
                            return +data;
                        }
                    }
                },
                {
                    data: 'abono', title: 'Abonos', render: function (data, type, row, meta) {
                        if (type === 'display') {
                            if (data.includes('-')) {
                                return `<span style="color: red;">${('-' + maskNumero6D(data.replace('-', '')))}</span>`; //Se quita el símbolo negativo y se vuelve a colocar a la izquierda del símbolo de pesos.
                            } else {
                                return maskNumero6D(data);
                            }
                        } else {
                            return +data;
                        }
                    }
                },
            ]
            , initComplete: function (settings, json) {
            },
            columnDefs: [
                { className: "dt-left", "targets": [0, 1, 2] },
                { className: "dt-right", "targets": [3, 4, 5, 6, 7, 8] },
                { width: '40%', targets: [0] }
            ]
        });
    }

    function getObtenerFecha() {
        inputFecha.datepicker({
            "dateFormat": "dd/mm/yy",
            "maxDate": fechaActualr
        }).datepicker("option", "showAnim", "slide")
            .datepicker("setDate", fechaActualr);
    }

    function cierreInventario() {
        let almacen = +inputAlmacenNum.val();
        let fecha = inputFecha.val();

        if (almacen > 0 && fecha.length > 0) {
            axios.post('CerrarInventarioFisico', { almacen, fecha })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha guardado la información.');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }
    }

    function getComparativoExistenciasinvitentario() {
        axios.post('ObtenerExistenciaInventario', {
            almacen: inputAlmacenNum.val(),
            fecha: inputFecha.val(),
            existentes: true,
            ultimoPrecio: true,
            insumoInicio: inputInsumoInicio.val(),
            insumoFin: inputInsumoFin.val(),
            soloConDiferencia: checkSoloConDiferencia.prop('checked')
        })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    AddRows(tblTabla, items);

                    btnCierreInventario.attr('disabled', !items.length > 0);

                    let totalCargos = items.map((x) => +x.cargos).reduce((accumulator, currentValue) => accumulator + currentValue);
                    let totalAbonos = items.map((x) => +x.abono).reduce((accumulator, currentValue) => accumulator + currentValue);
                    let diferenciaMontos = totalCargos - totalAbonos;

                    difNumero.text(parseFloat(response.data.difNumero, 16).toString());
                    difPorcentaje.text(parseFloat(response.data.difPorcentaje, 16).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + '%');
                    exactitudNumero.text(parseFloat(response.data.exactitudNumero, 16).toString());
                    exactitudPorcentaje.text(parseFloat(response.data.exactitudPorcentaje, 16).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + '%');
                    totalNumero.text(parseFloat(response.data.totalNumero, 16).toString());
                    totalPorcentaje.text(parseFloat(response.data.totalPorcentaje, 16).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + '%');

                    difCargo.text('$ ' + parseFloat(response.data.difCargo, 16).toFixed(6).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                    difAbono.text('$ ' + parseFloat(response.data.difAbono, 16).toFixed(6).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                    difTotal.text('$ ' + parseFloat(response.data.difTotal, 16).toFixed(6).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());

                    if (response.data.difCargo < 0) difCargo.addClass('negativo');
                    else difCargo.removeClass('negativo');
                    if (response.data.difAbono < 0) difAbono.addClass('negativo');
                    else difAbono.removeClass('negativo');
                    if (response.data.difTotal < 0) difTotal.addClass('negativo');
                    else difTotal.removeClass('negativo');
                }
            });
    }

    function ObtenerExistenciaInventarioreporte() {

        axios.post('ObtenerExistenciaInventarioreporte', {
            almacen: inputAlmacenNum.val(),
            fecha: inputFecha.val(),
            existentes: true,
            ultimoPrecio: true,
            insumoInicio: inputInsumoInicio.val(),
            insumoFin: inputInsumoFin.val(),
            soloConDiferencia: checkSoloConDiferencia.prop('checked')
        })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    $.blockUI({ message: 'Cargando reporte...' });
                    report.attr("src", `/Reportes/Vista.aspx?idReporte=219`);
                    document.getElementById('report').onload = function () {
                        openCRModal();
                        $.unblockUI();
                    };
                }
            });
    }


    function getObtenerAlmacenes() {
        axios.post('obtenerAlmacenes',)
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    AddRows(tblAlmacen, items);
                }
            });
    }
    function obtenerAlmacenxID() {
        axios.post('ObtenerAlmacenID', { almacen: inputAlmacenNum.val() })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    if (items != null) {
                        inputAlmacenDescripcion.val(items.descripcion)
                    } else {
                        inputAlmacenDescripcion.val('')
                    }
                } else {
                    inputAlmacenDescripcion.val('')
                }
            });
    }




    function AddRows(tbl, lst) {
        dt = tbl.DataTable();
        dt.clear().draw();
        dt.draw();
        dt.rows.add(lst).draw(false);
    }

    function consultaInicioInsumo(insumo) {
        axios.post('consultarPrimerInsumo', { insumo: insumo })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    if (items != null) {
                        inputInsumoInicio.val(items.insumoNumero);
                        inputInsumoInicioDescripcion.val(items.insumoDescripcion);
                    } else {
                        inputInsumoInicioDescripcion.val('');
                    }
                } else {
                    inputInsumoInicioDescripcion.val('');
                }
            });
    }
    function consultaFinInsumo(insumo) {
        axios.post('consultarUltimoInsumo', { insumo: insumo })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    if (items != null) {
                        inputInsumoFin.val(items.insumoNumero);
                        inputInsumoFinDescripcion.val(items.insumoDescripcion);
                    } else {
                        inputInsumoFinDescripcion.val('');
                    }
                } else {
                    inputInsumoFinDescripcion.val('');
                }
            });
    }









    $(document).ready(() => {
        ComparativoExistenciasInventario.Almacen = new Almacen();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();