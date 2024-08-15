let promedioEvaluacionesController = function () {

    const spanPromedioGeneral = $('#spanPromedioGeneral');
    const spanTotalPersonal = $('#spanTotalPersonal');
    const tdInstructivosOperativos = $('#tdInstructivosOperativos');
    const tdPromedioGeneral = $('#tdPromedioGeneral');
    const tdTecnicosOperativos = $('#tdTecnicosOperativos');
    const tdProtocolosFatalidad = $('#tdProtocolosFatalidad');
    const tdNormativos = $('#tdNormativos');
    let dtPersonal1;
    let inputMes = $('#inputMes');
    let cboProyecto = $('#cboProyecto');
    let cboDepartamento = $('#cboDepartamento');

    const btnBuscar = $('#btnBuscar');

    const fechaActual = new Date();
    const dateFormat = "dd/mm/yy";
    const showAnim = "slide";

    const Inicializar = function () {
        $.namespace('PromedioEvaluacion.Promedio');
        Iniciar();
        CargarCombos();
        fncBotones();

        cboProyecto.change(cargarAreasCC);
    }
    const Iniciar = function () {
        Promedio = function () {
            (function init() {
                initMonthPicker(inputMes);
            })();
        }
    }
    const fncBotones = function () {
        btnBuscar.click(function () {
            // console.log('obtener')
            postObtenerTablaPromedioEvaluaciones();
        });
    }
    const CargarCombos = function () {
        axios.get('ObtenerComboCCAmbasEmpresas').then(response => {
            let { success, items, message } = response.data;

            if (success) {
                cboProyecto.append('<option value="Todos">Todos</option>');

                items.forEach(x => {
                    let groupOption = `<optgroup label="${x.label}">`;

                    x.options.forEach(y => {
                        groupOption += `<option value="${y.Value}" empresa="${x.label == 'CONSTRUPLAN' ? 1 : x.label == 'ARRENDADORA' ? 2 : 0}">${y.Text}</option>`;
                    });

                    groupOption += `</optgroup>`;

                    cboProyecto.append(groupOption);
                });
            } else {
                AlertaGeneral(`Alerta`, message);
            }

            convertToMultiselect('#cboProyecto');
        }).catch(error => AlertaGeneral(`Alerta`, error.message));

        // axios.get('GetDepartamentosCombo').then(response => {
        //     let { success, items, message } = response.data;

        //     if (success) {
        //         cboDepartamento.append('<option value="Todos">Todos</option>');

        //         items.forEach(x => {
        //             let groupOption = `<optgroup label="${x.label}">`;

        //             x.options.forEach(y => {
        //                 groupOption += `<option value="${y.Value}" empresa="${y.Prefijo == 'CONSTRUPLAN' ? 1 : y.Prefijo == 'ARRENDADORA' ? 2 : 0}" cc="${y.Id}">${y.Text}</option>`;
        //             });

        //             groupOption += `</optgroup>`;

        //             cboDepartamento.append(groupOption);
        //         });

        convertToMultiselect('#cboDepartamento');
        //     } else {
        //         AlertaGeneral(`Alerta`, message);
        //     }
        // }).catch(error => AlertaGeneral(`Alerta`, error.message));
    }
    const postObtenerTablaPromedioEvaluaciones = function () {
        let listaCC = getValoresMultiples('#cboProyecto');
        let listaAreas = [];
        getValoresMultiplesCustom('#cboDepartamento').forEach(x => {
            listaAreas.push({
                cc: x.cc,
                area: +x.departamento,
                empresa: +x.empresa
            });
        });

        let mes = inputMes.val();
        let listaStringMes = mes.split('/');
        let fecha = '01' + '/' + listaStringMes[0] + '/' + listaStringMes[1];

        axios.post('GetPromedioEvaluaciones', { listaCC, listaAreas, fecha })
            .then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    spanPromedioGeneral.text(response.data.porcentajePromedioGeneral + '%');
                    spanTotalPersonal.text(response.data.totalPersonal);
                    tdInstructivosOperativos.text(response.data.porcentajeInstructivosOperativos + '%');
                    tdPromedioGeneral.text(response.data.porcentajePromedioGeneral + '%');
                    tdTecnicosOperativos.text(response.data.porcentajeTecnicosOperativos + '%');
                    tdProtocolosFatalidad.text(response.data.porcentajeProtocolosFatalidad + '%');
                    tdNormativos.text(response.data.porcentajeNormativos + '%');
                    initGraficaPromedioGeneralAnual(response.data.lstGraficaDTO)
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
    }
    const initMonthPicker = function (input) {
        $(input).datepicker({
            dateFormat: "mm/yy",
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            maxDate: fechaActual,
            showAnim: showAnim,
            closeText: "Aceptar",
            onClose: function (dateText, inst) {
                function isDonePressed() {
                    return ($('#ui-datepicker-div').html().indexOf('ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all ui-state-hover') > -1);
                }

                if (isDonePressed()) {
                    var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                    var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                    $(this).datepicker('setDate', new Date(year, month, 1)).trigger('change');

                    $('.date-picker').focusout()//Added to remove focus from datepicker input box on selecting date
                }
            },
            beforeShow: function (input, inst) {
                inst.dpDiv.addClass('month_year_datepicker')

                if ((datestr = $(this).val()).length > 0) {
                    year = datestr.substring(datestr.length - 4, datestr.length);
                    month = datestr.substring(0, 2);
                    $(this).datepicker('option', 'defaultDate', new Date(year, month - 1, 1));
                    $(this).datepicker('setDate', new Date(year, month - 1, 1));
                    $(".ui-datepicker-calendar").hide();
                }
            }
        }).datepicker("setDate", fechaActual);
    }

    function getValoresMultiplesCustom(selector) {
        var _tempObj = $(selector + ' option:selected').map(function (a, item) {
            return { value: item.value, empresa: $(item).attr('empresa'), departamento: item.value, cc: $(item).attr('cc') };
        });
        var _tempArrObj = new Array();
        $.each(_tempObj, function (i, e) {
            _tempArrObj.push(e);
        });
        return _tempArrObj;
    }

    function cargarAreasCC() {
        let listaCentrosCosto = [];

        getValoresMultiplesCustom('#cboProyecto').forEach(x => {
            listaCentrosCosto.push({
                cc: x.value,
                empresa: +(x.empresa)
            });
        });

        if (listaCentrosCosto.length == 0) {
            cboDepartamento.empty();
            convertToMultiselect('#cboDepartamento');
            return;
        }

        let ccsCplan = listaCentrosCosto.filter((x) => { return x.empresa == 1; }).map(function (x) { return x.cc; });
        let ccsArr = listaCentrosCosto.filter((x) => { return x.empresa == 2; }).map(function (x) { return x.cc; });

        $.post('/Administrativo/Capacitacion/ObtenerAreasPorCC', { ccsCplan, ccsArr })
            .then(response => {
                cboDepartamento.empty();
                if (response.success) {
                    // Operación exitosa.
                    // const todosOption = `<option value="Todos">Todos</option>`;
                    const option = `<option value="Todos">Todos</option>`;
                    cboDepartamento.append(option);

                    response.items.forEach(x => {
                        let groupOption = `<optgroup label="${x.label}"></optgroup>`;
                        x.options.forEach(y => {
                            groupOption += `<option value="${y.Value}" cc="${y.Id}" empresa="${y.Prefijo}">${y.Text}</option>`;
                        });
                        cboDepartamento.append(groupOption);
                    });

                } else {
                    // Operación no completada.
                    AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                }

                convertToMultiselect('#cboDepartamento');
            }, error => {
                // Error al lanzar la petición.
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
    }

    function AddRows(tbl, lst) {
        dt = tbl.DataTable();
        dt.clear().draw();
        dt.rows.add(lst).draw(false);
    }

    function initGraficaPromedioGeneralAnual(datos) {
        let obj = {}
        let arr = []
        datos.forEach(element => {
            obj = {}
            obj.name = element.name
            obj.y = element.y
            obj.drilldown = element.drilldown
            arr.push(obj)
        });

        Highcharts.chart("graficaMeses", {
            chart: {
                type: 'column'
            },
            title: {
                align: 'center',
                text: 'Promedio general por mes'
            },
            accessibility: {
                announceNewData: {
                    enabled: true
                }
            },
            xAxis: {
                type: 'category'
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                series: {
                    borderWidth: 0,
                    dataLabels: {
                        enabled: true,
                        format: '{point.y:.1f}%'
                    }
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b> of total<br/>'
            },
            series: [
                {
                    name: "Promedio",
                    colorByPoint: true,
                    data: arr,
                    color: "#ed7d31"
                }
            ]
        });
    }

    $(document).ready(() => {
        PromedioEvaluacion.Promedio = new Promedio();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...', baseZ: 2000 }); })
        .ajaxStop(() => { $.unblockUI(); });

    return {
        Inicializar: Inicializar
    }
};