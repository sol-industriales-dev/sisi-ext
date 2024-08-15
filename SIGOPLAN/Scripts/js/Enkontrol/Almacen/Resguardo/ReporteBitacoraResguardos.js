(() => {
    $.namespace('Resguardo.ReporteBitacoraResguardos');
    ReporteBitacoraResguardos = function () {
        //#region Selectores
        const inputCentroCostoInicio = $('#inputCentroCostoInicio');
        const inputCentroCostoInicioDesc = $('#inputCentroCostoInicioDesc');
        const inputCentroCostoFin = $('#inputCentroCostoFin');
        const inputCentroCostoFinDesc = $('#inputCentroCostoFinDesc');
        const inputEmpleadoInicio = $('#inputEmpleadoInicio');
        const inputEmpleadoInicioDesc = $('#inputEmpleadoInicioDesc');
        const inputEmpleadoFin = $('#inputEmpleadoFin');
        const inputEmpleadoFinDesc = $('#inputEmpleadoFinDesc');
        const selectEstatus = $('#selectEstatus');
        const inputNumeroSerie = $('#inputNumeroSerie');
        const botonReporte = $('#botonReporte');
        const btnReporte = $('#btnReporte');
        const report = $('#report')
        //#endregion

        const tblCentrosCostos = $('#tblCentrosCostos');
        const tblPersonal = $('#tblPersonal');
        const tblCentrosCostosFin = $('#tblCentrosCostosFin');
        const tblPersonalFin = $('#tblPersonalFin');
        let dtCentros;
        let dtPersonal;
        let dtCentrosfin;
        let dtPersonalfin;

        (function init() {
            botonReporte.click(descargarReporteExcel);
            fncEvents();
            initTablaCC();
            initTablaPersonal();
            initTablaCCFin();
            initTablaPersonalFin();

            convertToMultiselect('#selectEstatus');
        })();

        inputCentroCostoInicio.on('change', function () {
            let cc = inputCentroCostoInicio.val();

            if (cc != '') {
                axios.get('/Enkontrol/Almacen/GetCentroCosto', { params: { cc } })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            inputCentroCostoInicioDesc.val(response.data.ccDesc);
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            } else {
                inputCentroCostoInicio.val('');
                inputCentroCostoInicioDesc.val('');
            }
        });

        inputCentroCostoFin.on('change', function () {
            let cc = inputCentroCostoFin.val();

            if (cc != '') {
                axios.get('/Enkontrol/Almacen/GetCentroCosto', { params: { cc } })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            inputCentroCostoFinDesc.val(response.data.ccDesc);
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            } else {
                inputCentroCostoFin.val('');
                inputCentroCostoFinDesc.val('');
            }
        });

        inputEmpleadoInicio.on('change', function () {
            let empleado = inputEmpleadoInicio.val();

            if (empleado != '') {
                axios.get('/Enkontrol/Almacen/GetUsuarioEnkontrolByID', { params: { empleado } })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            inputEmpleadoInicioDesc.val(response.data.empleadoDesc);
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            } else {
                inputEmpleadoInicio.val('');
                inputEmpleadoInicioDesc.val('');
            }
        });

        inputEmpleadoFin.on('change', function () {
            let empleado = inputEmpleadoFin.val();

            if (empleado != '') {
                axios.get('/Enkontrol/Almacen/GetUsuarioEnkontrolByID', { params: { empleado } })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            inputEmpleadoFinDesc.val(response.data.empleadoDesc);
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            } else {
                inputEmpleadoFin.val('');
                inputEmpleadoFinDesc.val('');
            }
        });

        function initTablaCC() {
            dtCentros = tblCentrosCostos.DataTable({
                destroy: true
                , language: dtDicEsp
                , paging: true
                , ordering: false
                , bFilter: true
                , info: false
                , columns: [
                    { data: 'cc', title: 'Centro Costo' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'cc', render: (data, type, row) => `<button class='btn btn-xs btn-primary seleccionar' data-idCC='${row.cc}'><i class='fa fa-arrow-right'></i></button>` },
                ]
                , initComplete: function (settings, json) {
                    tblCentrosCostos.on("click", ".seleccionar", function (e) {
                        $('#inputCentroCostoInicio').val($(this).attr("data-idCC"))
                        // $('#inputCentroCostoFin').val($(this).attr("data-idCC"))
                        let cc = $(this).attr("data-idCC");
                        axios.get('/Enkontrol/Almacen/GetCentroCosto', { params: { cc } })
                            .then(response => {
                                let { success, datos, message } = response.data;

                                if (success) {
                                    inputCentroCostoInicioDesc.val(response.data.ccDesc);
                                } else {
                                    AlertaGeneral(`Alerta`, message);
                                }
                            }).catch(error => AlertaGeneral(`Alerta`, error.message));
                        $('#mdlCC').modal('hide');

                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaPersonal() {
            dtPersonal = tblPersonal.DataTable({
                destroy: true
                , language: dtDicEsp
                , paging: true
                , ordering: false
                , bFilter: true
                , info: false
                , columns: [
                    { data: 'empleado', title: 'Empleado' },
                    { data: 'descripcion', title: 'Nombre' },
                    { data: 'descripcion', render: (data, type, row) => `<button class='btn btn-xs btn-primary seleccionar' data-idempleado='${row.empleado}'><i class='fa fa-arrow-right'></i></button>` },
                ]
                , initComplete: function (settings, json) {
                    tblPersonal.on("click", ".seleccionar", function (e) {

                        $('#inputEmpleadoInicio').val($(this).attr("data-idempleado"))
                        let empleado = $(this).attr("data-idempleado");
                        axios.get('/Enkontrol/Almacen/GetUsuarioEnkontrolByID', { params: { empleado } })
                            .then(response => {
                                let { success, datos, message } = response.data;

                                if (success) {
                                    inputEmpleadoInicioDesc.val(response.data.empleadoDesc);
                                } else {
                                    AlertaGeneral(`Alerta`, message);
                                }
                            }).catch(error => AlertaGeneral(`Alerta`, error.message));

                        $('#mdlPersonal').modal('hide');

                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaCCFin() {
            dtCentrosfin = tblCentrosCostosFin.DataTable({
                destroy: true
                , language: dtDicEsp
                , paging: true
                , ordering: false
                , bFilter: true
                , info: false
                , columns: [
                    { data: 'cc', title: 'Centro Costo' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'cc', render: (data, type, row) => `<button class='btn btn-xs btn-primary seleccionar' data-idCC='${row.cc}'><i class='fa fa-arrow-right'></i></button>` },
                ]
                , initComplete: function (settings, json) {
                    tblCentrosCostosFin.on("click", ".seleccionar", function (e) {
                        // $('#inputCentroCostoInicio').val($(this).attr("data-idCC"))
                        $('#inputCentroCostoFin').val($(this).attr("data-idCC"))
                        let cc = $(this).attr("data-idCC");
                        axios.get('/Enkontrol/Almacen/GetCentroCosto', { params: { cc } })
                            .then(response => {
                                let { success, datos, message } = response.data;

                                if (success) {
                                    inputCentroCostoFinDesc.val(response.data.ccDesc);
                                } else {
                                    AlertaGeneral(`Alerta`, message);
                                }
                            }).catch(error => AlertaGeneral(`Alerta`, error.message));

                        $('#mdlCCfin').modal('hide');

                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaPersonalFin() {
            dtPersonalfin = tblPersonalFin.DataTable({
                destroy: true
                , language: dtDicEsp
                , paging: true
                , ordering: false
                , bFilter: true
                , info: false
                , columns: [
                    { data: 'empleado', title: 'Empleado' },
                    { data: 'descripcion', title: 'Nombre' },
                    { data: 'descripcion', render: (data, type, row) => `<button class='btn btn-xs btn-primary seleccionar' data-idempleado='${row.empleado}'><i class='fa fa-arrow-right'></i></button>` },
                ]
                , initComplete: function (settings, json) {
                    tblPersonalFin.on("click", ".seleccionar", function (e) {

                        // $('#inputEmpleadoInicio').val($(this).attr("data-idempleado"))
                        $('#inputEmpleadoFin').val($(this).attr("data-idempleado"))

                        let empleado = $(this).attr("data-idempleado");
                        axios.get('/Enkontrol/Almacen/GetUsuarioEnkontrolByID', { params: { empleado } })
                            .then(response => {
                                let { success, datos, message } = response.data;

                                if (success) {
                                    inputEmpleadoFinDesc.val(response.data.empleadoDesc);
                                } else {
                                    AlertaGeneral(`Alerta`, message);
                                }
                            }).catch(error => AlertaGeneral(`Alerta`, error.message));

                        $('#mdlPersonalfin').modal('hide');

                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function postObtenerCC() {

            axios.post('getCentrosCostos', {})
                .catch(o_O => AlertaGeneral(o_O.message))
                .then(response => {
                    console.log(response)
                    let { success, data } = response.data;
                    if (success) {
                        console.log(data)
                        AddRows(tblCentrosCostos, data)
                    }
                });
        }

        function postObtenerPersonal() {

            axios.post('GetEmpleados', {})
                .catch(o_O => AlertaGeneral(o_O.message))
                .then(response => {
                    let { success, data } = response.data;
                    if (success) {
                        AddRows(tblPersonal, data)
                    }
                });
        }

        function postObtenerCCfin() {

            axios.post('getCentrosCostos', {})
                .catch(o_O => AlertaGeneral(o_O.message))
                .then(response => {
                    console.log(response)
                    let { success, data } = response.data;
                    if (success) {
                        console.log(data)
                        AddRows(tblCentrosCostosFin, data)
                    }
                });
        }

        function postObtenerPersonalfin() {

            axios.post('GetEmpleados', {})
                .catch(o_O => AlertaGeneral(o_O.message))
                .then(response => {
                    let { success, data } = response.data;
                    if (success) {
                        AddRows(tblPersonalFin, data)
                    }
                });
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.draw();
            dt.rows.add(lst).draw(false);
        }

        function fncEvents() {
            $('#btnCCInicio').click(function () {
                postObtenerCC();
            })
            $('#btnCCFinal').click(function () {
                postObtenerCCfin();
            })

            $('#btnPersonalInicio').click(function () {
                postObtenerPersonal();
            })
            $('#btnPersonalFinal').click(function () {
                postObtenerPersonalfin();
            })
            btnReporte.click(function () {
                descargarReporte();
            })

        }

        function descargarReporte() {
            let centroCostoInicio = inputCentroCostoInicio.val();
            let centroCostoFin = inputCentroCostoFin.val();
            let empleadoInicio = inputEmpleadoInicio.val();
            let empleadoFin = inputEmpleadoFin.val();
            let listaNumeroSerie = inputNumeroSerie.val() != '' ? inputNumeroSerie.val().split(',').map(x => x.trim()) : [];
            let listaEstatus = [];

            getValoresMultiplesCustom('#selectEstatus').forEach(x => {
                listaEstatus.push(x.estatus);
            });

            axios.post('cargarSesionReporteBitacoraResguardosCrystal', { centroCostoInicio, centroCostoFin, empleadoInicio, empleadoFin, listaEstatus, listaNumeroSerie })
                .then(response => {
                    let { success, datos, message } = response.data;
                    // if (success) {
                    var path = `/Reportes/Vista.aspx?idReporte=217`;
                    report.attr("src", path);
                    document.getElementById('report').onload = function () {
                        $.unblockUI();
                        openCRModal();
                    };

                    limpiarVista();
                    // } else {
                    //     AlertaGeneral(`Alerta`, message);
                    // }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }



        function descargarReporteExcel() {
            let centroCostoInicio = inputCentroCostoInicio.val();
            let centroCostoFin = inputCentroCostoFin.val();
            let empleadoInicio = inputEmpleadoInicio.val();
            let empleadoFin = inputEmpleadoFin.val();
            let listaNumeroSerie = inputNumeroSerie.val() != '' ? inputNumeroSerie.val().split(',').map(x => x.trim()) : [];
            let listaEstatus = [];

            getValoresMultiplesCustom('#selectEstatus').forEach(x => {
                listaEstatus.push(x.estatus);
            });

            axios.post('CargarSesionReporteBitacoraResguardos', { centroCostoInicio, centroCostoFin, empleadoInicio, empleadoFin, listaEstatus, listaNumeroSerie })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        $.blockUI({ message: 'Generando Excel...' });

                        $(this).download = `/Enkontrol/Resguardo/CrearExcelReporteBitacoraResguardos`;
                        $(this).href = `/Enkontrol/Resguardo/CrearExcelReporteBitacoraResguardos`;

                        location.href = `/Enkontrol/Resguardo/CrearExcelReporteBitacoraResguardos`;

                        $.unblockUI();

                        limpiarVista();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function limpiarVista() {
            inputCentroCostoInicio.val('');
            inputCentroCostoInicioDesc.val('');
            inputCentroCostoFin.val('');
            inputCentroCostoFinDesc.val('');

            inputEmpleadoInicio.val('');
            inputEmpleadoInicioDesc.val('');
            inputEmpleadoFin.val('');
            inputEmpleadoFinDesc.val('');

            inputNumeroSerie.val('');
            selectEstatus.val(0);
            convertToMultiselect('#selectEstatus');
        }

        function getValoresMultiplesCustom(selector) {
            var _tempObj = $(selector + ' option:selected').map(function (a, item) {
                return { value: item.value, estatus: $(item).attr('estatus') };
            });
            var _tempArrObj = new Array();
            $.each(_tempObj, function (i, e) {
                _tempArrObj.push(e);
            });
            return _tempArrObj;
        }
    }
    $(document).ready(() => Resguardo.ReporteBitacoraResguardos = new ReporteBitacoraResguardos())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();