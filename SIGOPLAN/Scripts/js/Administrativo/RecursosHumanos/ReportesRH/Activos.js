$(function () {

    $.namespace('recursoshumanos.reportesrh.repactivos');

    repactivos = function () {
        mensajes = {
            NOMBRE: 'Reporte Captura Horometro',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        },
            cboCC = $("#cboCC"),
            btnBuscar = $("#btnBuscar"),
            btnImprimir = $("#btnImprimir"),
            tblData = $("#tblData");
        ireport = $("#report");
        modalReportes = $("#modalReportes");
        const tblActivos = $('#tblActivos');
        const cboColumnas = $('#cboColumnas');
        let dtActivos;

        //#region MDL HIJXS
        const tblHijxs = $('#tblHijxs');
        let dtHijxs;
        const mdlHijxs = $('#mdlHijxs');
        //#endregion

        const idEmpresa = $('#idEmpresa');

        const _ESTATUS = []
        _ESTATUS[0] = "";
        _ESTATUS[1] = "PADRE";
        _ESTATUS[2] = "MADRE";
        _ESTATUS[3] = "CONYUGE";
        _ESTATUS[4] = "HIJO";
        _ESTATUS[5] = "HERMANO";
        _ESTATUS[11] = "OTRO";

        function init() {
            cboCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false, "Todos");
            convertToMultiselect("#cboCC");
            convertToMultiselect("#cboColumnas");

            initTblActivos();
            initTblHijxs();

            btnBuscar.click(clickBuscar);
            btnImprimir.click(verReporte);

            cboColumnas.on("change", function () {
                let columns = getValoresMultiples("#cboColumnas");
                let numColumns = $('#cboColumnas option').length + 7;

                for (let i = 9; i <= numColumns; i++) {
                    if (columns.includes(i.toString())) {
                        dtActivos.column(i).visible(true);

                    } else {
                        dtActivos.column(i).visible(false);

                    }
                }
            });

        }
        function clickBuscar() {
            if (validateBuscar()) {
                filtrarGrid();
            }
        }
        function filtrarGrid() {
            // loadGrid(getFiltrosObject(), '/Administrativo/ReportesRH/FillGridRepActivos', tblData);
            fncGetRptActivos();
            loadGraph(getFiltrosObject(), '/Administrativo/ReportesRH/FillGridRepActivos', "myChart");
            btnImprimir.show();
        }
        function getFiltrosObject() {
            return {
                cc: getValoresMultiples("#cboCC")
            }
        }

        function validateBuscar() {
            var state = true;
            if (!validarCampo(cboCC)) { state = false; }
            if (!state) AlertaGeneral("Alerta", "Seleccione al menos un centro de costo");
            return state;
        }

        function loadGraph(objetoCarga, controller, divChart) {

            $.ajax({
                url: controller,
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify(objetoCarga),
                success: function (response) {
                    if (response.success) {
                        var cc = [];
                        var cantidad = [];
                        var Unicos = Enumerable.From(response.rows).Select(function (x) { return x.cC }).Distinct().ToArray();
                        $.each(Unicos, function (i, e) {
                            cc.push(e);
                            var cant = Enumerable.From(response.rows).Where(function (x) { return x.cC == e }).Count();
                            cantidad.push(cant);
                        });

                        BarChart(cc, cantidad, divChart);
                    }
                    else {

                    }
                },
                error: function (response) {
                    alert(response.message);
                }
            });
        }
        var myChart;
        function BarChart(meses, importes, divChart) {

            var maximo = Math.max.apply(null, importes);
            maximo = (maximo * .2) + maximo;
            var barChartData = {
                labels: meses,
                datasets: [
                    {
                        backgroundColor: 'rgba(255, 130, 35, 1)',
                        hoverBackgroundColor: 'rgba(255, 130, 35,0.5)',
                        borderColor: 'rgba(255,131,15,1)',
                        borderWidth: 1,
                        data: importes
                    }
                ]
            }

            if (myChart != null) {
                myChart.destroy();
            }

            var ctx = document.getElementById(divChart);
            myChart = new Chart(ctx, {
                type: 'bar',
                data: barChartData,
                options: {
                    //onClick: clickHandler,
                    responsive: true,
                    legend: {
                        display: false
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                callback: function (value, index, values) {
                                    return value.toFixed(1);
                                },
                                stepSize: Math.trunc(maximo / meses.length)
                            }
                        }],
                        xAxes: [{
                            ticks: {
                                autoSkip: false
                            }
                        }]
                    }
                    ,
                    hover: {
                        animationDuration: 0
                    },
                    animation: {
                        duration: 1,
                        onComplete: function () {
                            var chartInstance = this.chart,
                                ctx = chartInstance.ctx;
                            ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                            ctx.fillStyle = "#000000";
                            ctx.textAlign = 'center';
                            ctx.textBaseline = 'bottom';

                            this.data.datasets.forEach(function (dataset, i) {
                                var meta = chartInstance.controller.getDatasetMeta(i);
                                meta.data.forEach(function (bar, index) {
                                    data = dataset.data[index].toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                                    ctx.fillText(data, bar._model.x, bar._model.y - 5);
                                });
                            });
                        }
                    }
                }
            });
            function clickHandler(evt, element) {
                if (element.length) {
                    let data = meses[element[0]._index]
                    if (getIfMeses()) {
                        modalTitle.text("Detalle por Año " + data);
                    }
                    else {
                        modalTitle.text("Detalle por mes " + data);
                    }
                    $("#tituloModalMaquina").text($("#cboFiltroGrupo option:selected").text() + " " + $("#cboFiltroNoEconomico option:selected").text());
                    cargarInicio();
                    loadTabla(getFiltrosObject(data), ruta, gridFiltros, true);
                }
            }

            //inicializarCanvas();
            //addEventListener("resize", inicializarCanvas);
        }

        function verReporte(e) {

            $.blockUI({ message: mensajes.PROCESANDO });

            var idReporte = "20";

            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte;

            ireport.attr("src", path);

            document.getElementById('report').onload = function () {

                $.unblockUI();
                openCRModal();

            };
            e.preventDefault();

        }

        function initTblActivos() {
            dtActivos = tblActivos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: true,
                dom: 'Bfrtip',
                columns: [
                    //render: function (data, type, row) { }
                    { data: "empleadoID", title: "NO. EMPLEADO" },
                    { data: "empleado", title: "NOMBRE" },
                    {
                        data: "puesto", title: "PUESTO", render: function (data, type, row) {
                            return data != null ? ("[" + row.idPuesto + "] " + data) : " "
                        }
                    },
                    { data: 'descCategoria', title: 'CATEGORIA', render: function (data, type, row) { return data ?? "S/N" } },
                    { data: "cC", title: "CC" },
                    { data: "departamento", title: "DEPARTAMENTO" },
                    { data: "fechaAltaStr", title: "FECHA PRIMER INGRESO" },
                    { data: "fechaAltaRe", title: "FECHA ALTA" },
                    { data: "tipo_nomina", title: "TIPO NOMINA" },
                    { data: "requisicion", title: "REQUISICION", visible: false },
                    {
                        data: "nss", title: idEmpresa.val() == 6 ? 'DNI' : 'NSS', visible: false, render: function (data, type, row) {

                            if (idEmpresa.val() == 6) {
                                return row.dni;
                            } else {
                                return data
                            }
                        }
                    },
                    { data: "jefeInmediato", title: "RESPONSABLE DE CC", visible: false },
                    { data: "salario_base", title: "SALARIO BASE", render: function (data, type, row) { return maskNumero(data) }, visible: false },
                    { data: "complemento", title: "COMPLEMENTO", render: function (data, type, row) { return maskNumero(data) }, visible: false },
                    { data: "bono_zona", title: "BONO EN SISTEMA", render: function (data, type, row) { return maskNumero(data) }, visible: false },
                    { data: "total_nominal", title: "TOTAL NOMINAL", render: function (data, type, row) { return maskNumero(data) }, visible: false },
                    { data: "total_mensual", title: "TOTAL MENSUAL", render: function (data, type, row) { return maskNumero(data) }, visible: false },
                    { data: "regpat", title: "REG. PATRONAL", visible: false },
                    { data: "domicilio", title: "DOMICILIO", visible: false },
                    { data: "nombre_estado_nac", title: "ESTADO NAC", visible: false },
                    { data: "nombre_ciudad_nac", title: "CUIDAD NAC", visible: false },
                    {
                        data: "fecha_nac", title: "FECHA NAC", visible: false,
                        render: function (data, type, row) {
                            return moment(data, 'DD-MM-YYYY').format('DD/MM/YYYY');
                        }
                    },
                    { data: "email", title: "EMAIL", visible: false },
                    { data: "tel_cel", title: "TEL. CELULAR", visible: false },
                    { data: "tel_casa", title: "TEL. CONTACTO 1", visible: false },
                    { data: "en_accidente_nombre", title: "CONTACTO EMERGENCIA", visible: false },
                    { data: "en_accidente_telefono", title: "TELEFONO EMERGENCIA", visible: false },
                    { data: "sexo", title: "SEXO", visible: false },
                    { data: "rfc", title: "RFC", visible: false },
                    {
                        data: "curp", title: idEmpresa.val() == 6 ? 'C.U.S.P.P' : "CURP", visible: false, render: function (data, type, row) {

                            if (idEmpresa.val() == 6) {
                                return row.cuspp;
                            } else {
                                return data
                            }
                        }
                    },
                    { data: "estado_civil", title: "ESTADO CIVIL", visible: false },
                    { data: "tipoSangre", title: "TIPO SANGRE", visible: false },
                    { data: "alergias", title: "ALERGIAS", visible: false },
                    { data: "tipoCasa", title: "TIPO CASA", visible: false },
                    { data: "ocupacion", title: "OCUPACION", visible: false },
                    {
                        data: "numHijos", title: "NUM HIJOS", visible: false,
                        render: function (data, type, row) {
                            return `<button class="btn  btn-xs btn-primary btn-sm getHijos">Hijos: ${data}</button>`;
                        }
                    },
                    { data: "nombreConyuge", title: "CONYUGE", visible: false },
                    { data: "contratoDesc", title: "TIPO CONTRATO", visible: false },
                    { data: "camisa", title: "TALLA CAMISA", visible: false },
                    { data: "pantalon", title: "TALLA PANTALON", visible: false },
                    { data: "calzado", title: "TALLA CALZADO", visible: false },
                    { data: 'ciudadContacto', title: 'CIUDAD CONTACTO', visible: false }
                    // { data: "hijos", title: "", visible: false },                    
                    // { data: "fechaAlta", title: "", visible: false },
                    // { data: "fechaAltaRe", title: "", visible: false },
                ],
                initComplete: function (settings, json) {
                    tblActivos.on('click', '.getHijos', function () {
                        let rowData = dtActivos.row($(this).closest('tr')).data();
                        mdlHijxs.modal("show");
                        dtHijxs.clear();
                        dtHijxs.rows.add(rowData.hijxs);
                        dtHijxs.draw();
                        // console.log(rowData.hijxs);
                    });
                    tblActivos.on('click', '.classBtn', function () {
                        let rowData = dtActivos.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        exportOptions: {
                            columns: ':visible'
                            // columns: [1, 2, 3, 4, 5, 6, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61]
                        }

                    }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }

                ],
                order: [[1, 'asc']]
            });
        }

        function fncGetRptActivos() {
            obj = getFiltrosObject();
            axios.post("GetRptActivos", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    // if (!response.data.empresa) {
                    //     dtActivos.Columns[3].ColumnName = "AC";
                    // }
                    dtActivos.clear();
                    dtActivos.rows.add(items);
                    dtActivos.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTblHijxs() {
            dtHijxs = tblHijxs.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                dom: 'Bfrtip',
                columns: [
                    //render: function (data, type, row) { }
                    {
                        title: "NOMBRE",
                        render: function (data, type, row) {
                            return row.apellido_paterno + " " + row.apellido_materno + " " + row.nombre;
                        }
                    },
                    {
                        data: "fecha_de_nacimiento", title: "FECHA NAC",
                        render: function (data, type, row) {
                            return data != null ? moment(data).format("DD/MM/YYYY") : "";
                        }
                    },
                    {
                        data: "parentesco", title: "PARENTESCO",
                        render: function (data, type, row) {
                            return _ESTATUS[data ?? 0];
                        }
                    },
                    { data: "grado_de_estudios", title: "ESTUDIOS", },
                    { data: "estado_civil", title: "ESTADO CIVIL", },
                    { data: "estudia", title: "ESTUDIA", },
                    { data: "genero", title: "GENERO", },
                    { data: "trabaja", title: "TRABAJA", },
                    { data: "vive", title: "VIVE", },
                ],
                initComplete: function (settings, json) {
                    tblHijxs.on('click', '.classBtn', function () {
                        let rowData = dtHijxs.row($(this).closest('tr')).data();
                    });
                    tblHijxs.on('click', '.classBtn', function () {
                        let rowData = dtHijxs.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        exportOptions: {
                            columns: ':visible'
                            // columns: [1, 2, 3, 4, 5, 6, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61]
                        }

                    }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }
        init();
    }

    $(document).ready(function () {
        recursoshumanos.reportesrh.repactivos = new repactivos();
    });
});



