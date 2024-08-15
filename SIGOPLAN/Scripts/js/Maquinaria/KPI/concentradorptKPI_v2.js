(() => {
    $.namespace('kpi.concentradorptKPI');

    concentradorptKPI = function () {

        urlCboCC = '/Conciliacion/getCboCC';
        // Variables.
        const txtDivision = $("#txtDivision");
        const cboAC = $('#cboAC');
        const cboGrupo = $('#cboGrupo');
        const cboModelo = $('#cboModelo');
        const fechaFin = $('#fechaFin');
        const fechaIni = $('#fechaIni');
        const tablaKPI = $('#tablaKPI');
        const btnBusqueda = $('#btnBusqueda');
        const report = $('#report');
        const btnExcel = $("#btnExcel");
        const btnValidar = $("#btnValidar");

        ///
        const btnGenerarKPI = $('#btnGenerarKPI');
        const modalGestion = $('#modalGestion');
        const btnGuardarSemana = $('#btnGuardarSemana');
        const selBusqCC = $("#selBusqCC");
        const inputFecha = $('#inputFecha');
        let dtTablaKpiGenerados;
        let dtContratos;

        (function init() {
            // Lógica de inicialización.
            initTablaMaquinas();
            initCbo();
            initEventListener();
            //initTablaKpiGenerados();

        })();

        // Métodos.
        function initCbo() {
            txtDivision.fillCombo('/Rentabilidad/fillComboDivision', null, false);
            cboAC.fillCombo('/Rentabilidad/cboCentroCostosUsuarios', null, false);
            cboGrupo.fillCombo('/CatMaquina/FillCboGrupo_Maquina', { idTipo: 1 });
            cboGrupo.change(setCboModelos);
            btnExcel.prop('disabled', true);
            selBusqCC.fillCombo(urlCboCC, null, false, null);
            inputFecha.datepicker({
                "dateFormat": "dd/mm/yy",
                "maxDate": new Date()
            }).datepicker("option", "showAnim", "slide")
                .datepicker("setDate", new Date());
        }


        function initEventListener() {
            btnBusqueda.click(getInfoTabla);
            btnExcel.click(getRPT);
            btnGuardarSemana.click(fnGuardarClick);
            btnValidar.click(fnValidar);
        }
        function fnValidar(){
            $.post('/KPI/ValidarConcentrado', { ac: cboAC.val(), fechaInicio: inputFecha.val(), fechaFinal: inputFecha.val(), semana: 0 })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        btnValidar.hide();
                        modalGestion.modal('hide');
                        AlertaGeneral(`Operación Exitosa`, `Validada correctamente.`);
                    } else {
                        // Operación no completada.
                                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
            );
        }
        function guardarSemana() {
            $.post('/KPI/GuardarSemana', { ac: cboAC.val(), fechaInicio:inputFecha.val(), fechaFinal: inputFecha.val(), semana: 0 })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        modalGestion.modal('hide');
                        AlertaGeneral(`Operación Exitosa`, `Guardado Exitoso.`);
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
            );
        }

        function getRPT() {
            $.blockUI({ message: 'Cargando Información' });
            var idReporte = "206";
            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&pModulo=" + 1 + "&pFechaPeriodo=" + inputFecha.val();
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                openCRModal();
                $.unblockUI();
            };
            $.unblockUI();

        }

        function getInfoTabla() {

            if (inputFecha.val() != "") {

                let modelo = cboModelo.val() != null ? cboModelo.val() == '' ? 0 : cboModelo.val() : 0;
                $.get('/KPI/GetConcentradoKPI', { ac: cboAC.val(), grupoID: cboGrupo.val() == '' ? 0 : cboGrupo.val(), modeloID: modelo, fechaInicio: inputFecha.val(), fechaFin: inputFecha.val() })
                    .then(response => {
                        if (response.success) {
                            // Operación ex itosa.
                            AddRows(tablaKPI, response.infoGrupos);
                            if((cboGrupo.val() == '' || cboGrupo.val() == null) && (cboModelo.val() == '' || cboModelo.val() == null) ){
                                if(response.generada)
                                {
                                    btnGuardarSemana.hide();
                                    if(response.validada){
                                        btnValidar.hide();
                                    }
                                    else{
                                        btnValidar.show();
                                    }
                                }
                                else{
                                    btnGuardarSemana.show();
                                }
                            }
                            else{
                                btnGuardarSemana.hide();
                                btnExcel.hide();
                                btnValidar.hide();
                            }
                            //btnExcel.prop('disabled', false);
                        } else {
                            // Operación no completada.
                            AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        }
                    }, error => {
                        // Error al lanzar la petición.
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            }
            else {
                AlertaGeneral(`Operación fallida`, `Es necesario un periodo para poder realizar la busqueda.`);
            }

        }

        function setCboModelos() {
            cboModelo.fillCombo('/OT/cboModelo', { idGrupo: cboGrupo.val() ?? 0 });
        }

        //#region tabla KPI
        function initTablaMaquinas() {
            dtTablaKPI = tablaKPI.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '45vh',
                scrollCollapse: true,
                dom: 'Bfrtip',
                buttons: parametrosImpresion("Reporte de Autorizaciones KPI Homologado", "<center><h3>Reporte de Autorizaciones KPI Homologado</h3></center>"),
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true
                    }
                ],
                initComplete: function (settings, json) {
                },
                columns: [
                    //    { data: null, title: 'Analisis' },
                    { data: 'economico', title: 'Maquina' },
                    { data: 'horasTrabajadas', title: 'H.Trabajadas' },
                    { data: 'horasMMTO', title: 'H. Paro MTTO' },
                    { data: 'horasReserva', title: 'H. Paro Reserva' },
                    { data: 'horasDemora', title: 'H. Paro Demora' },
                    { data: 'disponibilidad', title: '% Disponibilidad' },
                    { data: 'utilizacion', title: '% Utilizacion' }

                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ]
            });
        }
        //#endregion

        function initTablaKpiGenerados() {
            dtContratos = tblContratos.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '45vh',
                scrollCollapse: true,
                scrollX: true,
                initComplete: function (settings, json) {
                },
                columns: [
                    { data: 'semana', title: 'Semana' },
                    { data: 'año', title: 'Año' },
                    { data: 'fechaInicio', title: 'Fecha inicio' },
                    { data: 'fechaFin', title: 'Fecha vencimiento periodo actual' },
                    { data: 'estatus', title: 'Monto Financiado' },
                    { data: 'usuario', title: 'Amortización Capital' },
                    { data: null, title: 'Cancelar' }
                ],
                columnDefs: [
                    {
                        targets: '_all',
                        className: 'dt-center'
                    },
                    {
                        targets: [0],
                        render: function (data, type, row) {
                            return '<button type="button" class="btn btn-primary btnCancelar"><i class="fas fa-align-justify"></i></button>';
                        }
                    }
                ]
            });
        }

        //#region  Config_tabla
        function AddRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }
        //#endregion
        //#region config_Fechas
        function datePicker() {
            var now = new Date(),
                year = now.getYear() + 1900;
            $.datepicker.setDefaults($.datepicker.regional["es"]);
            var dateFormat = "dd/mm/yy",
                from = $("#fechaIni")
                    .datepicker({

                        changeMonth: true,
                        changeYear: true,
                        numberOfMonths: 1,
                        defaultDate: new Date(year, 00, 01),
                        maxDate: new Date(year, 11, 31),
                        onSelect: function () {
                            $(this).trigger('change');
                        }
                    })
                    .on("change", function () {

                        var date = $(this).val();
                        var array = new Array();
                        dateIncio = "";
                        dateFin = "";
                        array = date.split('/');
                        $(this).datepicker('setDate', new Date(array[2], array[1] - 1, 1));
                        fechaFin.datepicker('setDate', new Date(array[2], array[1], 0));
                        $(this).blur();
                        getFechas();

                    });

            function getDate(element) {
                var date;
                try {
                    date = $.datepicker.parseDate(dateFormat, element.value);
                } catch (error) {
                    date = null;
                }

                return date;
            }
        }

        function getFechas() {
            var array2 = new Array();
            dateFinal = fechaFin.val();
            array2 = dateFinal.split('/');
            dateIncio = array2[2] + "/" + array2[1] + "/" + "01";
            dateFin = array2[2] + "/" + array2[1] + "/" + array2[0];
        }
        //#endregion

        function fnGuardarClick() {
            let valid = getValid();
            if (valid.isValid) {
                swal({
                    title: "¿Desea Generar el concentrado de KPI?",
                    text: "Una vez generado pasara a autorización.",
                    type: "warning",
                    buttons: true,
                    dangerMode: true,
                }).then(isConfirmed => {
                    if (isConfirmed) {
                        guardarSemana();
                    }
                });
            }
            else {
                AlertaGeneral('Alerta', valid.message);
            }

        }

        function getValid() {
            let valid = { isValid: true, message: '' }
            if (cboAC.val() === '') {
                valid.isValid = false;
                valid.message = 'No se seleccciono ningun centro de costos';
                return valid;
            }

            if ($('#inputFecha').val() === '') {
                valid.isValid = false;
                valid.message = 'Es necesario una semana para poder generar KPI';
                return valid;
            }

            return valid;
        }
    }

    $(() => kpi.concentradorptKPI = new concentradorptKPI())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();


