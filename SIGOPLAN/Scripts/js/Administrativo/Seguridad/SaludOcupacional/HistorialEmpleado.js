(() => {
    $.namespace('SaludOcupacional.HistorialEmpleado');
    HistorialEmpleado = function () {
        //#region Selectores
        const inputClaveEmpleado = $('#inputClaveEmpleado');
        const inputNombreEmpleado = $('#inputNombreEmpleado');
        const botonBuscar = $('#botonBuscar');
        const tablaHistorial = $('#tablaHistorial');
        const botonCertificado = $('#botonCertificado');
        const modalCertificado = $('#modalCertificado');
        const inputClaveEmpleadoCertificado = $('#inputClaveEmpleadoCertificado');
        const inputNombreEmpleadoCertificado = $('#inputNombreEmpleadoCertificado');
        const inputEdad = $('#inputEdad');
        const inputTA = $('#inputTA');
        const inputFC = $('#inputFC');
        const inputFR = $('#inputFR');
        const inputSPO = $('#inputSPO');
        const inputTemperatura = $('#inputTemperatura');
        const inputTalla = $('#inputTalla');
        const inputPeso = $('#inputPeso');
        const inputImpresionDiagnostica = $('#inputImpresionDiagnostica');
        const botonGenerarCertificado = $('#botonGenerarCertificado');
        const report = $("#report");
        //#endregion

        let dtHistorial;

        (function init() {
            initTablaHistorial();

            botonBuscar.click(cargarHistorialEmpleado);
            inputClaveEmpleado.change(cargarInformacionEmpleado);
            inputClaveEmpleadoCertificado.change(cargarInformacionEmpleadoCertificado);
            botonCertificado.click(() => { modalCertificado.modal('show'); })
            botonGenerarCertificado.click(generarCertificado);
        })();

        function initTablaHistorial() {
            dtHistorial = tablaHistorial.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tablaHistorial.on('click', '.botonReporte', function () {
                        let rowData = dtHistorial.row($(this).closest('tr')).data();

                        switch (rowData.tipo) {
                            case 1: //Historial Clínico
                                imprimirHistorialClinico(rowData.id);
                                break;
                            case 2: //Atención Médica
                                imprimirAtencionMedica(rowData.id);
                                break;
                        }
                    });
                },
                columns: [
                    { data: 'claveEmpleado', title: 'Clave' },
                    { data: 'empleadoDesc', title: 'Empleado' },
                    { data: 'ccDesc', title: 'Proyecto' },
                    { data: 'areaDesc', title: 'Área' },
                    { data: 'tipoDesc', title: 'Tipo' },
                    { data: 'fechaString', title: 'Fecha' },
                    {
                        title: '', render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-default botonReporte"><i class="fa fa-file"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarHistorialEmpleado() {
            let claveEmpleado = +inputClaveEmpleado.val();

            if (isNaN(claveEmpleado) || claveEmpleado <= 0) {
                Alert2Warning('Debe colocar una clave de empleado válida.');
                return;
            }

            axios.post('/Administrativo/SaludOcupacional/CargarHistorialEmpleado', { claveEmpleado })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaHistorial, response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarInformacionEmpleado() {
            let claveEmpleado = +inputClaveEmpleado.val();

            if (claveEmpleado <= 0 || isNaN(claveEmpleado)) {
                Alert2Warning('Debe seleccionar una clave de empleado válida.');
                return;
            }

            axios.post('/Administrativo/SaludOcupacional/CargarInformacionEmpleado', { claveEmpleado })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        inputNombreEmpleado.val(response.data.data.nombre);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarInformacionEmpleadoCertificado() {
            let claveEmpleado = +inputClaveEmpleadoCertificado.val();

            if (claveEmpleado <= 0 || isNaN(claveEmpleado)) {
                Alert2Warning('Debe seleccionar una clave de empleado válida.');
                return;
            }

            axios.post('/Administrativo/SaludOcupacional/CargarInformacionEmpleado', { claveEmpleado })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        inputNombreEmpleadoCertificado.val(response.data.data.nombre);
                        inputEdad.val(response.data.data.edad);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function imprimirHistorialClinico(id) {
            if (id == 0) {
                AlertaGeneral(`Alerta`, `No se ha seleccionado un historial clínico.`);
                return;
            }

            $.blockUI({ message: 'Generando imprimible...' });
            var path = `/Reportes/Vista.aspx?idReporte=244&historialClinico=${id}`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function imprimirAtencionMedica(id) {
            if (id == 0) {
                AlertaGeneral(`Alerta`, `No se ha seleccionado una atención médica.`);
                return;
            }

            $.blockUI({ message: 'Generando imprimible...' });
            var path = `/Reportes/Vista.aspx?idReporte=246&atencionMedica_id=${id}`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function generarCertificado() {
            let empleado = {
                claveEmpleado: +inputClaveEmpleadoCertificado.val(),
                nombre: inputNombreEmpleadoCertificado.val(),
                edad: inputEdad.val(),
                TA: inputTA.val(),
                FC: inputFC.val(),
                FR: inputFR.val(),
                SPO: inputSPO.val(),
                temperatura: inputTemperatura.val(),
                talla: inputTalla.val(),
                peso: inputPeso.val(),
                impresionDiagnostica: inputImpresionDiagnostica.val()
            };

            axios.post('/Administrativo/SaludOcupacional/GenerarCertificadoSesion', { empleado })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        crearReporteCertificado();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function crearReporteCertificado() {
            $.blockUI({ message: 'Generando certificado...' });
            var path = `/Reportes/Vista.aspx?idReporte=245`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }
        

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => SaludOcupacional.HistorialEmpleado = new HistorialEmpleado())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();