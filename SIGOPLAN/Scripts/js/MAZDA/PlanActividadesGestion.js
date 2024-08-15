(function () {

    $.namespace('planActividades.gestion');

    gestion = function () {

        _flagPlanMesGeneral = false;
        const fecha = new Date();
        const monthNames = ["ENERO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO", "JULIO", "AGOSTO", "SEPTIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE"];

        btnFiltrarPlanMaestro = $("#btnFiltrarPlanMaestro");
        btnPrint = $("#btnPrint");
        btnPrintMes = $("#btnPrintMes");
        report = $("#report");

        mdlRevisionAC = $('#mdlRevisionAC');
        mdlRevisionCua = $('#mdlRevisionCua');

        cuadrillaID = 0;
        areaID = 0;
        periodoID = 0;
        area = '';
        btnPrintExcel = $('#btnPrintExcel');
        let plan = [];
        let revision = [];


        /*$('#tblPlanMaestro tbody').on('click', '.mesChecked', function () {
            var celda = $(this);

            $('#selectMes').val(celda.attr('data-mes'));

            $.ajax({
                url: '/MAZDA/PlanActividades/GetPlanMes',
                datatype: "json",
                type: "POST",
                data: { cuadrillaID: parseInt(celda.attr('data-cuadrillaID')), periodo: parseInt(celda.attr('data-periodo')), mes: parseInt(celda.attr('data-mes')) },
                success: function (response) {
                    var data = response.data;

                    if (data != null) {
                        _flagPlanMesGeneral = false;

                        $("#dialogPlanMes").modal('show');
                        $('#dialogPlanMes').attr('data-cuadrillaID', celda.attr('data-cuadrillaID'));

                        $('#labelDialogPlanMes').text('Plan Mes ' + response.info.mes + ' - ' + response.info.cua + (response.info.per != "" ? (' (' + response.info.per + ')') : ''))

                        fillPlanMes(data, parseInt(celda.attr('data-mes')), parseInt(celda.attr('data-periodo')), parseInt(celda.attr('data-cuadrillaID')));
                    }
                }
            });
        });*/

        $('#tblPlanMaestro tbody').on('click', '.areaText', function () {
            const celda = $(this);
            $.ajax({
                url: '/MAZDA/PlanActividades/GetEquiposCatalogo',
                type: 'POST',
                data: { arrCuadrillas: parseInt(celda.attr('data-cuadrillaID')), arrAreas: parseInt(celda.attr('data-areaID')) },
                success: function (response) {
                    const data = response.data;

                    if (data != null) {
                        cuadrillaID = celda.attr('data-cuadrillaID');
                        areaID = celda.attr('data-areaID');
                        periodoID = celda.attr('data-periodo');
                        area = celda.prop('outerText');

                        $("#mdlEquiposArea").modal('show');
                        $('#lblEquiposArea').text('Equipos Area - ' + celda.prop('outerText'));

                        $('#tblEquiposAreaDias tbody tr').remove();
                        $('#tblEquiposAreaDias thead tr').remove();

                        fillEquiposArea(data, parseInt(celda.attr('data-cuadrillaID')), parseInt(celda.attr('data-periodo')));
                    }
                    else {
                        $('#tblEquiposAreaDias tbody tr').remove();
                    }

                }
            });

        });

        $('#tblPlanMaestro thead').on('click', '.mesHeader', function () {
            var celda = $(this);
            var mes = parseInt(celda.attr('data-numeromes'));

            $.ajax({
                url: '/MAZDA/PlanActividades/GetPlanMesGeneral',
                datatype: "json",
                type: "POST",
                data: { mes: mes },
                success: function (response) {
                    var data = response.data;

                    if (data != null) {
                        _flagPlanMesGeneral = true;

                        $("#dialogPlanMes").modal('show');

                        $('#labelDialogPlanMes').text('Plan Mes ' + response.info.mes)

                        fillPlanMesGeneral(data, mes);
                    }
                }
            });
        });

        $('#tblPlanMes tbody').on('click', '.celdaEquipoAreaBody', function () {
            var cuadrillaID = 0;
            if (!_flagPlanMesGeneral) {
                cuadrillaID = parseInt($('#dialogPlanMes').attr('data-cuadrillaID'));
            } else {
                cuadrillaID = parseInt($(this).attr('data-cuadrillaid'))
            }

            var planMesDetalleID = $(this).attr('planmesdetalleid');

            $('#planMesDetalleID').val(planMesDetalleID);

            if (cuadrillaID == 1) {
                mdlRevisionAC.modal('show');
            } else if (cuadrillaID > 1) {
                mdlRevisionCua.modal('show');
                fnPlanMaestroIndividual(cuadrillaID);
                $('#selectCuadrilla').val(cuadrillaID);
            }
        });

        $('#tblPlanMes tbody').on('click', '.celdaCheckRevision', function () {

            var cuadrillaID = 0;
            if (!_flagPlanMesGeneral) {
                cuadrillaID = parseInt($('#dialogPlanMes').attr('data-cuadrillaid'));
            } else {
                cuadrillaID = parseInt($(this).attr('data-cuadrillaid'))
            }

            var revisionID = parseInt($(this).closest('tr').attr('data-revisionid'));

            if (cuadrillaID != 1) {
                report.attr("src", '/Reportes/Vista.aspx?idReporte=91&cuadrillaID=' + cuadrillaID + '&revisionID=' + revisionID);
            } else {
                report.attr("src", '/Reportes/Vista.aspx?idReporte=90&cuadrillaID=' + cuadrillaID + '&revisionID=' + revisionID);
            }

            document.getElementById('report').onload = function () {
                openCRModal();
            };
        });

        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        function init() {
            initCbo();
            btnPrint.prop("disabled", true);
            btnFiltrarPlanMaestro.click(fnPlanMaestro);
            btnPrint.click(verReporte);
            btnPrintMes.click(verReporteMensual);
            $("#multiSelectCua").change(fnMultiCuaChange);
            $("#multiSelectPer").change(fnMultiPerChange);
            $("#multiSelectArea").change(fnMultiAreaChange);
            $('#multiSelectAct').change(fnMultiActChange);

            fnPlanMaestro();
            //mdn document;
        }

        btnPrintExcel.click(function (e) {

            $.blockUI({ message: "Preparando archivo a descargar" });
            $(this).download = '/MAZDA/PlanActividades/GenerarPlanExcel';
            $(this).href = '/MAZDA/PlanActividades/GenerarPlanExcel';
            location.href = '/MAZDA/PlanActividades/GenerarPlanExcel';
            $.unblockUI();
        });


        function daysInMonth(year, month) {
            return new Date(year, month, 0).getDate();
        }

        function verReporte() {
            report.attr("src", '/Reportes/Vista.aspx?idReporte=88');
            document.getElementById('report').onload = function () {
                openCRModal();
            };
        }

        function verReporteMensual() {
            if (!_flagPlanMesGeneral) {
                report.attr("src", '/Reportes/Vista.aspx?idReporte=89');
                document.getElementById('report').onload = function () {
                    openCRModal();
                };
            } else {
                report.attr("src", '/Reportes/Vista.aspx?idReporte=95');
                document.getElementById('report').onload = function () {
                    openCRModal();
                };
            }
        }

        function initCbo() {
            $("#multiSelectCua").fillCombo('/MAZDA/PlanActividades/GetCuadrillasList', { est: true }, false, "Todos");
            convertToMultiselect('#multiSelectCua');

            $("#multiSelectPer").fillCombo('/MAZDA/PlanActividades/GetPeriodosList', { est: true }, false, "Todos");
            convertToMultiselect('#multiSelectPer');

            $("#multiSelectArea").fillCombo('/MAZDA/PlanActividades/GetAreasList', { est: true }, false, "Todos");
            convertToMultiselect('#multiSelectArea');

            $("#multiSelectAct").fillCombo('/MAZDA/PlanActividades/GetActividadesList', { est: true }, false, "Todos");
            convertToMultiselect('#multiSelectAct');

            $("#multiSelectMes").fillCombo('/MAZDA/PlanActividades/GetMesesList', { periodo: 0 }, false, "Todos");
            convertToMultiselect('#multiSelectMes');

            $("#selAyudante").fillCombo('/MAZDA/PlanActividades/GetEmpleadoList', { est: true }, false, "Todos");
            convertToMultiselect('#selAyudante');
        }

        function fnMultiCuaChange() {
            $("#multiSelectPer").fillCombo('/MAZDA/PlanActividades/GetPeriodosList', { cuadrillasID: getValoresMultiples('#multiSelectCua') }, false, "Todos");
            convertToMultiselect('#multiSelectPer');

            $("#multiSelectArea").fillCombo('/MAZDA/PlanActividades/GetAreasList', { cuadrillasID: getValoresMultiples('#multiSelectCua'), periodos: getValoresMultiples('#multiSelectPer') }, false, "Todos");
            convertToMultiselect('#multiSelectArea');

            $("#multiSelectAct").fillCombo('/MAZDA/PlanActividades/GetActividadesList', { cuadrillasID: getValoresMultiples('#multiSelectCua'), periodos: getValoresMultiples('#multiSelectPer'), areas: getTextosMultiples('#multiSelectArea') }, false, "Todos");
            convertToMultiselect('#multiSelectAct');

            $("#multiSelectMes").fillCombo('/MAZDA/PlanActividades/GetMesesList', { periodo: 0 }, false, "Todos");
            convertToMultiselect('#multiSelectMes');
        }

        function fnMultiPerChange() {
            $("#multiSelectArea").fillCombo('/MAZDA/PlanActividades/GetAreasList', { cuadrillasID: getValoresMultiples('#multiSelectCua'), periodos: getValoresMultiples('#multiSelectPer') }, false, "Todos");
            convertToMultiselect('#multiSelectArea');

            $("#multiSelectAct").fillCombo('/MAZDA/PlanActividades/GetActividadesList', { cuadrillasID: getValoresMultiples('#multiSelectCua'), periodos: getValoresMultiples('#multiSelectPer'), areas: getTextosMultiples('#multiSelectArea') }, false, "Todos");
            convertToMultiselect('#multiSelectAct');

            $("#multiSelectMes").fillCombo('/MAZDA/PlanActividades/GetMesesList', { periodo: 0 }, false, "Todos");
            convertToMultiselect('#multiSelectMes');
        }

        function fnMultiAreaChange() {
            $("#multiSelectAct").fillCombo('/MAZDA/PlanActividades/GetActividadesList', { cuadrillasID: getValoresMultiples('#multiSelectCua'), periodos: getValoresMultiples('#multiSelectPer'), areas: getTextosMultiples('#multiSelectArea') }, false, "Todos");
            convertToMultiselect('#multiSelectAct');

            $("#multiSelectMes").fillCombo('/MAZDA/PlanActividades/GetMesesList', { periodo: 0 }, false, "Todos");
            convertToMultiselect('#multiSelectMes');
        }

        function fnMultiActChange() {
            $("#multiSelectMes").fillCombo('/MAZDA/PlanActividades/GetMesesList', { periodo: 0 }, false, "Todos");
            convertToMultiselect('#multiSelectMes');
        }

        function recargarTodo() {
            initCbo();
        }

        function fnPlanMaestro() {
            var arrCuadrillas = getValoresMultiples('#multiSelectCua');
            var arrPeriodos = getValoresMultiples('#multiSelectPer');
            var arrAreas = getTextosMultiples('#multiSelectArea');
            var arrActividades = getTextosMultiples('#multiSelectAct');
            var arrMeses = getValoresMultiples('#multiSelectMes');

            $.ajax({
                url: '/MAZDA/PlanActividades/GetPlanMaestro',
                datatype: "json",
                type: "POST",
                data: {
                    arrCuadrillas: arrCuadrillas,
                    arrPeriodos: arrPeriodos,
                    arrAreas: arrAreas,
                    arrActividades: arrActividades,
                    arrMeses: arrMeses
                },
                success: function (response) {
                    var data = response.data;
                    fillPlanMaestroOrdenado(data);
                    btnPrint.prop("disabled", data.length == 0);
                }
            });
        }

        function fillPlanMaestroOrdenado(data) {
            $('#tblPlanMaestro tbody tr').remove();

            var cuadrillaID = 0;
            var cuadrillaCount = 1;
            var cuadrillaIDAnt = 0;

            var periodo = 0;
            var periodoCount = 1;
            var periodoAnt = 0;

            var areaID = 0;
            var areaCount = 1;

            var actividad = "";
            var actividadCount = 1;

            for (i = 0; i < data.length; i++) {
                var html = '';

                html += '<tr>';

                if (data[i].cuadrillaID != cuadrillaID) {
                    html += '   <td class="text-center cuadrillaText cua_' + data[i].cuadrillaID + '" style="vertical-align: middle;">';
                    html += '       ' + data[i].cuadrilla;
                    html += '   </td>';

                    var cuadrillaAnterior = $('.cua_' + cuadrillaID);
                    cuadrillaAnterior.attr('rowspan', cuadrillaCount);

                    var periodoAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo);
                    periodoAnterior.attr('rowspan', periodoCount);

                    var periodoMesesAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo).closest('tr').find('.mes');
                    periodoMesesAnterior.attr('rowspan', periodoCount);

                    var areaAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo + '_area_' + areaID);
                    areaAnterior.attr('rowspan', areaCount);

                    cuadrillaID = data[i].cuadrillaID;
                    cuadrillaCount = 1;
                } else {
                    cuadrillaCount++;
                }

                if ((data[i].periodo != periodo) || (data[i].cuadrillaID != cuadrillaIDAnt)) {
                    html += '   <td class="text-center periodoText cua_' + data[i].cuadrillaID + '_per_' + data[i].periodo + '" style="vertical-align: middle;">';
                    html += '       ' + data[i].periodoDesc;
                    html += '   </td>';

                    periodoAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo);
                    periodoAnterior.attr('rowspan', periodoCount);

                    periodoMesesAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo).closest('tr').find('.mes');
                    periodoMesesAnterior.attr('rowspan', periodoCount);

                    areaAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo + '_area_' + areaID);
                    areaAnterior.attr('rowspan', areaCount);

                    periodo = data[i].periodo;
                    periodoCount = 1;
                } else {
                    periodoCount++;
                }

                if ((data[i].areaID != areaID) || (data[i].periodo != periodoAnt)) {
                    html += '   <td class="text-center areaText cua_' + data[i].cuadrillaID + '_per_' + data[i].periodo + '_area_' + data[i].areaID + '" style="vertical-align: middle;" data-cuadrillaID="' + data[i].cuadrillaID + '" data-areaID="' + data[i].areaID + '" data-periodo="' + data[i].periodo + '">';
                    html += '       ' + data[i].area;
                    html += '   </td>';

                    areaAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo + '_area_' + areaID);
                    areaAnterior.attr('rowspan', areaCount);

                    areaID = data[i].areaID;
                    areaCount = 1;
                } else {
                    areaCount++;
                }

                //if ((data[i].descripcion.trim() != actividad.trim()) || (data[i].periodo != periodoAnt)) {
                if (data[i].descripcion.trim() != actividad.trim()) {
                    html += '   <td class="text-left cua_' + data[i].cuadrillaID + '_per_' + data[i].periodo + '_area_' + data[i].areaID + '_act_' + data[i].id + '" style="vertical-align: middle;">';
                    html += '       ' + data[i].descripcion;
                    html += '   </td>';

                    var actividadAnterior = $('#tblPlanMaestro tbody td[class*="' + 'cua_' + cuadrillaIDAnt + '_per_' + periodoAnt + '"]' + ':contains(' + actividad.trim() + ')');

                    var actividadAnteriorFiltrado = actividadAnterior.filter(function () {
                        return $(this).text().trim() === actividad.trim();
                    });

                    actividadAnteriorFiltrado.attr('rowspan', actividadCount);

                    actividad = data[i].descripcion.trim();
                    actividadCount = 1;
                } else {
                    actividadCount++;
                }

                if ((data[i].periodo != periodoAnt) || (data[i].cuadrillaID != cuadrillaIDAnt)) {
                    var periodoClase = '';

                    switch (data[i].periodo) {
                        case 1:
                            periodoClase = 'mensual';
                            break;
                        case 2:
                            periodoClase = 'bimestral';
                            break;
                        case 3:
                            periodoClase = 'trimestral';
                            break;
                        case 4:
                            periodoClase = 'semestral';
                            break;
                        case 5:
                            periodoClase = 'anual';
                            break;
                    }

                    var encabezadoMeses = $('#tblPlanMaestro thead .mesHeader');
                    $.each(encabezadoMeses, function (index, th) {
                        let numero = parseInt($(th).attr('data-numeromes'));

                        if (data[i].mesesFiltro == null || data[i].mesesFiltro.includes(numero)) {
                            $(th).css('display', 'table-cell');
                        } else {
                            $(th).css('display', 'none');
                        }
                    });

                    if (data[i].mesesFiltro == null || data[i].mesesFiltro.includes(4)) {
                        html += '   <td class="text-center mes ' + (data[i].periodoFiltro.includes(4) ? periodoClase : '') + ' ' + (data[i].mes4 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="4">';
                        // if (data[i].mes4 == true) {
                        //     html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                        // }
                        html += '   </td>';
                    }

                    if (data[i].mesesFiltro == null || data[i].mesesFiltro.includes(5)) {
                        html += '   <td class="text-center mes ' + (data[i].periodoFiltro.includes(5) ? periodoClase : '') + ' ' + (data[i].mes5 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="5">';
                        // if (data[i].mes5 == true) {
                        //     html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                        // }
                        html += '   </td>';
                    }

                    if (data[i].mesesFiltro == null || data[i].mesesFiltro.includes(6)) {
                        html += '   <td class="text-center mes ' + (data[i].periodoFiltro.includes(6) ? periodoClase : '') + ' ' + (data[i].mes6 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="6">';
                        // if (data[i].mes6 == true) {
                        //     html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                        // }
                        html += '   </td>';
                    }

                    if (data[i].mesesFiltro == null || data[i].mesesFiltro.includes(7)) {
                        html += '   <td class="text-center mes ' + (data[i].periodoFiltro.includes(7) ? periodoClase : '') + ' ' + (data[i].mes7 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="7">';
                        // if (data[i].mes7 == true) {
                        //     html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                        // }
                        html += '   </td>';
                    }

                    if (data[i].mesesFiltro == null || data[i].mesesFiltro.includes(8)) {
                        html += '   <td class="text-center mes ' + (data[i].periodoFiltro.includes(8) ? periodoClase : '') + ' ' + (data[i].mes8 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="8">';
                        // if (data[i].mes8 == true) {
                        //     html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                        // }
                        html += '   </td>';
                    }

                    if (data[i].mesesFiltro == null || data[i].mesesFiltro.includes(9)) {
                        html += '   <td class="text-center mes ' + (data[i].periodoFiltro.includes(9) ? periodoClase : '') + ' ' + (data[i].mes9 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="9">';
                        // if (data[i].mes9 == true) {
                        //     html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                        // }
                        html += '   </td>';
                    }

                    if (data[i].mesesFiltro == null || data[i].mesesFiltro.includes(10)) {
                        html += '   <td class="text-center mes ' + (data[i].periodoFiltro.includes(10) ? periodoClase : '') + ' ' + (data[i].mes10 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="10">';
                        // if (data[i].mes10 == true) {
                        //     html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                        // }
                        html += '   </td>';
                    }

                    if (data[i].mesesFiltro == null || data[i].mesesFiltro.includes(11)) {
                        html += '   <td class="text-center mes ' + (data[i].periodoFiltro.includes(11) ? periodoClase : '') + ' ' + (data[i].mes11 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="11">';
                        // if (data[i].mes11 == true) {
                        //     html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                        // }
                        html += '   </td>';
                    }

                    if (data[i].mesesFiltro == null || data[i].mesesFiltro.includes(12)) {
                        html += '   <td class="text-center mes ' + (data[i].periodoFiltro.includes(12) ? periodoClase : '') + ' ' + (data[i].mes12 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="12">';
                        // if (data[i].mes12 == true) {
                        //     html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                        // }
                        html += '   </td>';
                    }

                    if (data[i].mesesFiltro == null || data[i].mesesFiltro.includes(1)) {
                        html += '   <td class="text-center mes ' + (data[i].periodoFiltro.includes(1) ? periodoClase : '') + ' ' + (data[i].mes1 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="1">';
                        // if (data[i].mes1 == true) {
                        //     html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                        // }
                        html += '   </td>';
                    }

                    if (data[i].mesesFiltro == null || data[i].mesesFiltro.includes(2)) {
                        html += '   <td class="text-center mes ' + (data[i].periodoFiltro.includes(2) ? periodoClase : '') + ' ' + (data[i].mes2 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="2">';
                        // if (data[i].mes2 == true) {
                        //     html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                        // }
                        html += '   </td>';
                    }

                    if (data[i].mesesFiltro == null || data[i].mesesFiltro.includes(3)) {
                        html += '   <td class="text-center mes ' + (data[i].periodoFiltro.includes(3) ? periodoClase : '') + ' ' + (data[i].mes3 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="3">';
                        // if (data[i].mes3 == true) {
                        //     html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                        // }
                        html += '   </td>';
                    }
                }

                cuadrillaIDAnt = data[i].cuadrillaID;
                periodoAnt = data[i].periodo;

                html += '</tr>';

                $(html).appendTo($("#tblPlanMaestro tbody"));
            }

            cuadrillaAnterior = $('.cua_' + cuadrillaID);
            cuadrillaAnterior.attr('rowspan', cuadrillaCount);

            periodoAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo);
            periodoAnterior.attr('rowspan', periodoCount);

            periodoMesesAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo).closest('tr').find('.mes');
            periodoMesesAnterior.attr('rowspan', periodoCount);

            areaAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo + '_area_' + areaID);
            areaAnterior.attr('rowspan', areaCount);

            actividadAnterior = $('#tblPlanMaestro tbody td[class*="' + 'cua_' + cuadrillaIDAnt + '_per_' + periodoAnt + '"]' + ':contains(' + actividad.trim() + ')');

            actividadAnteriorFiltrado = actividadAnterior.filter(function () {
                return $(this).text().trim() === actividad.trim();
            });

            actividadAnteriorFiltrado.attr('rowspan', actividadCount);
        }

        function fillPlanMaestroOrdenadoAnterior(data) {
            $('#tblPlanMaestro tbody tr').remove();

            var cuadrillaID = 0;
            var cuadrillaCount = 1;
            var cuadrillaIDAnt = 0;

            var periodo = 0;
            var periodoCount = 1;
            var periodoAnt = 0;

            var areaID = 0;
            var areaCount = 1;

            var actividad = "";
            var actividadCount = 1;

            for (i = 0; i < data.length; i++) {
                var html = '';

                html += '<tr>';

                if (data[i].cuadrillaID != cuadrillaID) {
                    html += '   <td class="text-center cuadrillaText cua_' + data[i].cuadrillaID + '" style="vertical-align: middle;">';
                    html += '       ' + data[i].cuadrilla;
                    html += '   </td>';

                    var cuadrillaAnterior = $('.cua_' + cuadrillaID);
                    cuadrillaAnterior.attr('rowspan', cuadrillaCount);

                    var periodoAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo);
                    periodoAnterior.attr('rowspan', periodoCount);

                    var periodoMesesAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo).closest('tr').find('.mes');
                    periodoMesesAnterior.attr('rowspan', periodoCount);

                    var areaAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo + '_area_' + areaID);
                    areaAnterior.attr('rowspan', areaCount);

                    cuadrillaID = data[i].cuadrillaID;
                    cuadrillaCount = 1;
                } else {
                    cuadrillaCount++;
                }

                if ((data[i].periodo != periodo) || (data[i].cuadrillaID != cuadrillaIDAnt)) {
                    html += '   <td class="text-center periodoText cua_' + data[i].cuadrillaID + '_per_' + data[i].periodo + '" style="vertical-align: middle;">';
                    html += '       ' + data[i].periodoDesc;
                    html += '   </td>';

                    periodoAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo);
                    periodoAnterior.attr('rowspan', periodoCount);

                    periodoMesesAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo).closest('tr').find('.mes');
                    periodoMesesAnterior.attr('rowspan', periodoCount);

                    areaAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo + '_area_' + areaID);
                    areaAnterior.attr('rowspan', areaCount);

                    periodo = data[i].periodo;
                    periodoCount = 1;
                } else {
                    periodoCount++;
                }

                if ((data[i].areaID != areaID) || (data[i].periodo != periodoAnt)) {
                    html += '   <td class="text-center areaText cua_' + data[i].cuadrillaID + '_per_' + data[i].periodo + '_area_' + data[i].areaID + '" style="vertical-align: middle;">';
                    html += '       ' + data[i].area;
                    html += '   </td>';

                    areaAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo + '_area_' + areaID);
                    areaAnterior.attr('rowspan', areaCount);

                    areaID = data[i].areaID;
                    areaCount = 1;
                } else {
                    areaCount++;
                }

                //if ((data[i].descripcion.trim() != actividad.trim()) || (data[i].periodo != periodoAnt)) {
                if (data[i].descripcion.trim() != actividad.trim()) {
                    html += '   <td class="text-left cua_' + data[i].cuadrillaID + '_per_' + data[i].periodo + '_area_' + data[i].areaID + '_act_' + data[i].id + '" style="vertical-align: middle;">';
                    html += '       ' + data[i].descripcion;
                    html += '   </td>';

                    var actividadAnterior = $('#tblPlanMaestro tbody td[class*="' + 'cua_' + cuadrillaIDAnt + '_per_' + periodoAnt + '"]' + ':contains(' + actividad.trim() + ')');

                    var actividadAnteriorFiltrado = actividadAnterior.filter(function () {
                        return $(this).text().trim() === actividad.trim();
                    });

                    actividadAnteriorFiltrado.attr('rowspan', actividadCount);

                    actividad = data[i].descripcion.trim();
                    actividadCount = 1;
                } else {
                    actividadCount++;
                }

                if ((data[i].periodo != periodoAnt) || (data[i].cuadrillaID != cuadrillaIDAnt)) {
                    switch (data[i].periodo) {
                        case 1:
                            html += '   <td class="text-center mes mensual ' + (data[i].mes4 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="4">';
                            if (data[i].mes4 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes mensual ' + (data[i].mes5 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="5">';
                            if (data[i].mes5 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes mensual ' + (data[i].mes6 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="6">';
                            if (data[i].mes6 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes mensual ' + (data[i].mes7 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="7">';
                            if (data[i].mes7 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes mensual ' + (data[i].mes8 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="8">';
                            if (data[i].mes8 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes mensual ' + (data[i].mes9 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="9">';
                            if (data[i].mes9 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes mensual ' + (data[i].mes10 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="10">';
                            if (data[i].mes10 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes mensual ' + (data[i].mes11 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="11">';
                            if (data[i].mes11 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes mensual ' + (data[i].mes12 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="12">';
                            if (data[i].mes12 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes mensual ' + (data[i].mes1 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="1">';
                            if (data[i].mes1 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes mensual ' + (data[i].mes2 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="2">';
                            if (data[i].mes2 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes mensual ' + (data[i].mes3 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="3">';
                            if (data[i].mes3 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            break;
                        case 2:
                            html += '   <td class="text-center mes ' + (data[i].mes4 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="4">';
                            if (data[i].mes4 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes bimestral ' + (data[i].mes5 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="5">';
                            if (data[i].mes5 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes6 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="6">';
                            if (data[i].mes6 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes bimestral ' + (data[i].mes7 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="7">';
                            if (data[i].mes7 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes8 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="8">';
                            if (data[i].mes8 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes bimestral ' + (data[i].mes9 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="9">';
                            if (data[i].mes9 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes10 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="10">';
                            if (data[i].mes10 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes bimestral ' + (data[i].mes11 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="11">';
                            if (data[i].mes11 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes12 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="12">';
                            if (data[i].mes12 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes bimestral ' + (data[i].mes1 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="1">';
                            if (data[i].mes1 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes2 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="2">';
                            if (data[i].mes2 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes bimestral ' + (data[i].mes3 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="3">';
                            if (data[i].mes3 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            break;
                        case 3:
                            html += '   <td class="text-center mes ' + (data[i].mes4 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="4">';
                            if (data[i].mes4 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes5 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="5">';
                            if (data[i].mes5 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes trimestral ' + (data[i].mes6 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="6">';
                            if (data[i].mes6 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes7 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="7">';
                            if (data[i].mes7 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes8 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="8">';
                            if (data[i].mes8 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes trimestral ' + (data[i].mes9 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="9">';
                            if (data[i].mes9 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes10 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="10">';
                            if (data[i].mes10 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes11 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="11">';
                            if (data[i].mes11 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes trimestral ' + (data[i].mes12 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="12">';
                            if (data[i].mes12 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes1 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="1">';
                            if (data[i].mes1 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes2 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="2">';
                            if (data[i].mes2 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes trimestral ' + (data[i].mes3 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="3">';
                            if (data[i].mes3 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            break;
                        case 4:
                            html += '   <td class="text-center mes semestral ' + (data[i].mes4 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="4">';
                            if (data[i].mes4 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes5 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="5">';
                            if (data[i].mes5 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes6 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="6">';
                            if (data[i].mes6 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes7 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="7">';
                            if (data[i].mes7 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes8 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="8">';
                            if (data[i].mes8 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes9 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="9">';
                            if (data[i].mes9 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes semestral ' + (data[i].mes10 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="10">';
                            if (data[i].mes10 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes11 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="11">';
                            if (data[i].mes11 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes12 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="12">';
                            if (data[i].mes12 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes1 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="1">';
                            if (data[i].mes1 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes2 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="2">';
                            if (data[i].mes2 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes3 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="3">';
                            if (data[i].mes3 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            break;
                        case 5:
                            html += '   <td class="text-center mes ' + (data[i].mes4 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="4">';
                            if (data[i].mes4 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes5 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="5">';
                            if (data[i].mes5 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes6 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="6">';
                            if (data[i].mes6 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes7 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="7">';
                            if (data[i].mes7 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes8 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="8">';
                            if (data[i].mes8 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes anual ' + (data[i].mes9 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="9">';
                            if (data[i].mes9 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes10 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="10">';
                            if (data[i].mes10 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes11 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="11">';
                            if (data[i].mes11 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes12 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="12">';
                            if (data[i].mes12 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes1 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="1">';
                            if (data[i].mes1 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes2 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="2">';
                            if (data[i].mes2 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            html += '   <td class="text-center mes ' + (data[i].mes3 == true ? 'mesChecked' : '') + '" data-cuadrillaID="' + data[i].cuadrillaID + '" data-periodo="' + data[i].periodo + '" data-mes="3">';
                            if (data[i].mes3 == true) {
                                html += '<span class="glyphicon glyphicon-arrow-right"></span>';
                            }
                            html += '   </td>';

                            break;
                    }
                }

                cuadrillaIDAnt = data[i].cuadrillaID;
                periodoAnt = data[i].periodo;

                html += '</tr>';

                $(html).appendTo($("#tblPlanMaestro tbody"));
            }

            cuadrillaAnterior = $('.cua_' + cuadrillaID);
            cuadrillaAnterior.attr('rowspan', cuadrillaCount);

            periodoAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo);
            periodoAnterior.attr('rowspan', periodoCount);

            periodoMesesAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo).closest('tr').find('.mes');
            periodoMesesAnterior.attr('rowspan', periodoCount);

            areaAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo + '_area_' + areaID);
            areaAnterior.attr('rowspan', areaCount);

            actividadAnterior = $('#tblPlanMaestro tbody td[class*="' + 'cua_' + cuadrillaIDAnt + '_per_' + periodoAnt + '"]' + ':contains(' + actividad.trim() + ')');

            actividadAnteriorFiltrado = actividadAnterior.filter(function () {
                return $(this).text().trim() === actividad.trim();
            });

            actividadAnteriorFiltrado.attr('rowspan', actividadCount);
        }

        function createElement(tagName, id, classes, attrs, events) {
            attrs = Object.assign(attrs || {}, { id: id });
            events = Object.assign(events || {});

            var el = document.createElement(tagName);

            classes.forEach(function (value) {
                if (value !== undefined && value != '') {
                    el.classList.add(value);
                }
            });

            Object.keys(attrs).forEach(function (key) {
                if (attrs[key] !== undefined) {
                    el.setAttribute(key.toString(), attrs[key]);
                }
            });

            Object.keys(events).forEach(function (key) {
                if (typeof events[key] === 'function') {
                    el.addEventListener(key, events[key]);
                }
            });

            return el;
        }

        function fillPlanMes(plan, mes, periodo, cuadrilla) {
            //Fecha nueva para agarrar el año actual
            var dt = new Date();

            $.ajax({
                url: '/MAZDA/PlanActividades/GetDiasMes',
                datatype: "json",
                type: "POST",
                data: { year: dt.getFullYear(), month: mes },
                success: function (response) {
                    var data = response.items;

                    $('#tblPlanMes thead tr').remove();
                    $('#tblPlanMes tbody tr').remove();

                    var filaHeader1 = document.createElement('tr');
                    $('#tblPlanMes thead').append(filaHeader1);
                    var filaHeader2 = document.createElement('tr');
                    $('#tblPlanMes thead').append(filaHeader2);

                    var celdaEquipoArea = document.createElement('th');
                    celdaEquipoArea.innerText = 'EQUIPO/AREA';
                    celdaEquipoArea.setAttribute('rowspan', '2');
                    celdaEquipoArea.classList.add('text-center');
                    celdaEquipoArea.style.verticalAlign = 'middle';

                    $('#tblPlanMes thead tr:eq(0)').append(celdaEquipoArea);

                    for (i = 0; i < data.length; i++) {
                        var letra = document.createElement('th');
                        letra.innerText = data[i].Text;
                        letra.classList.add('diaLetra');
                        letra.classList.add('text-center');

                        var numero = document.createElement('th');
                        numero.innerText = data[i].Value;
                        numero.classList.add('diaNumero');
                        numero.classList.add('text-center');

                        if (data[i].Text == 'D') {
                            letra.classList.add('diaDomingoLetra');
                            numero.classList.add('diaDomingoNumero');
                        }

                        $('#tblPlanMes thead tr:eq(0)').append(letra);
                        $('#tblPlanMes thead tr:eq(1)').append(numero);
                    }

                    for (j = 0; j < plan.detalle.length; j++) {
                        var fila = document.createElement('tr');

                        $(fila).attr('data-revisionid', plan.detalle[j].revisionID);

                        $('#tblPlanMes tbody').append(fila);

                        var celdaEquipoAreaBody = document.createElement('td');
                        $(celdaEquipoAreaBody).attr('planmesdetalleid', plan.detalle[j].id);
                        $(celdaEquipoAreaBody).attr('data-tipo', plan.detalle[j].tipo);
                        $(celdaEquipoAreaBody).addClass('celdaEquipoAreaBody');

                        var inputEquipoAreaBody = document.createElement('input');
                        inputEquipoAreaBody.classList.add('form-control');
                        inputEquipoAreaBody.classList.add('inputEquipoArea');
                        inputEquipoAreaBody.disabled = true;

                        $(celdaEquipoAreaBody).append(document.createElement('div'));

                        $(celdaEquipoAreaBody).find('div').append(document.createElement('a'));
                        $(celdaEquipoAreaBody).find('a').text(plan.detalle[j].equipoAreaDesc);
                        $(celdaEquipoAreaBody).find('a').attr('href', '#');

                        //celdaEquipoAreaBody.textContent = plan.detalle[j].equipoAreaDesc;

                        $(inputEquipoAreaBody).val(plan.detalle[j].equipoAreaDesc);

                        $('#tblPlanMes tbody tr:eq(' + j + ')').append(celdaEquipoAreaBody);

                        for (k = 0; k < data.length; k++) {
                            var celdaCheck = document.createElement('td');
                            celdaCheck.classList.add('text-center');
                            celdaCheck.style.padding = '0px';
                            celdaCheck.style.verticalAlign = 'middle';
                            celdaCheck.classList.add('celdaCheck');
                            celdaCheck.setAttribute('diaNumero', data[k].Value);

                            if (data[k].Text == 'D') {
                                celdaCheck.classList.add('diaDomingoCheck');
                            }

                            celdaCheck.style.width = '28px';

                            if (plan.detalle[j].dias.includes(data[k].Value)) {
                                var span = document.createElement('span');
                                span.classList.add('glyphicon');
                                span.classList.add('glyphicon-arrow-right');
                                $(celdaCheck).append(span);
                                $(celdaCheck).attr('checked', true);
                                $(celdaCheck).css('width', '28px');

                                if (plan.detalle[j].checkRev) {
                                    $(celdaCheck).addClass('celdaCheckRevision');
                                }
                            }

                            $('#tblPlanMes tbody tr:eq(' + j + ')').append(celdaCheck);
                        }
                    }
                }
            });
        }

        function fillPlanMesGeneral(planes, mes) {
            var dt = new Date();

            $.ajax({
                url: '/MAZDA/PlanActividades/GetDiasMes',
                datatype: "json",
                type: "POST",
                data: { year: dt.getFullYear(), month: mes },
                success: function (response) {
                    var data = response.items;

                    $('#tblPlanMes tr').remove();

                    $('#tblPlanMes thead').append(document.createElement('tr'));
                    $('#tblPlanMes thead').append(document.createElement('tr'));

                    var celdaCuadrilla = document.createElement('th');
                    celdaCuadrilla.innerText = 'CUADRILLA';
                    celdaCuadrilla.setAttribute('rowspan', '2');
                    celdaCuadrilla.classList.add('text-center');
                    celdaCuadrilla.style.verticalAlign = 'middle';

                    $('#tblPlanMes thead tr:eq(0)').append(celdaCuadrilla);

                    var celdaPeriodo = document.createElement('th');
                    celdaPeriodo.innerText = 'PERIODO';
                    celdaPeriodo.setAttribute('rowspan', '2');
                    celdaPeriodo.classList.add('text-center');
                    celdaPeriodo.style.verticalAlign = 'middle';

                    $('#tblPlanMes thead tr:eq(0)').append(celdaPeriodo);

                    var celdaEquipoArea = document.createElement('th');
                    celdaEquipoArea.innerText = 'EQUIPO/AREA';
                    celdaEquipoArea.setAttribute('rowspan', '2');
                    celdaEquipoArea.classList.add('text-center');
                    celdaEquipoArea.style.verticalAlign = 'middle';

                    $('#tblPlanMes thead tr:eq(0)').append(celdaEquipoArea);

                    for (i = 0; i < data.length; i++) {
                        var letra = document.createElement('th');
                        letra.innerText = data[i].Text;
                        letra.classList.add('diaLetra');
                        letra.classList.add('text-center');

                        var numero = document.createElement('th');
                        numero.innerText = data[i].Value;
                        numero.classList.add('diaNumero');
                        numero.classList.add('text-center');

                        if (data[i].Text == 'D') {
                            letra.classList.add('diaDomingoLetra');
                            numero.classList.add('diaDomingoNumero');
                        }

                        $('#tblPlanMes thead tr:eq(0)').append(letra);
                        $('#tblPlanMes thead tr:eq(1)').append(numero);
                    }

                    var cuadrillaID = 0;
                    var cuadrillaCount = 1;
                    var cuadrillaIDAnt = 0;

                    var periodo = 0;
                    var periodoCount = 1;
                    var periodoAnt = 0;

                    for (g = 0; g < planes.length; g++) {
                        for (j = 0; j < planes[g].detalle.length; j++) {
                            var fila = document.createElement('tr');

                            $(fila).attr('data-revisionid', planes[g].detalle[j].revisionID);

                            $('#tblPlanMes tbody').append(fila);

                            if (planes[g].cuadrillaID != cuadrillaID) {
                                var tdCuadrilla = createElement('td', '', ['text-center', 'cuadrillaText', ('cua_' + planes[g].cuadrillaID)], { 'data-cuadrillaid': planes[g].cuadrillaID }, '')
                                tdCuadrilla.textContent = planes[g].cuadrilla;
                                tdCuadrilla.style.verticalAlign = 'middle';

                                $('#tblPlanMes tbody tr:last').append(tdCuadrilla);

                                var cuadrillaAnterior = $('#tblPlanMes .cua_' + cuadrillaID);
                                cuadrillaAnterior.attr('rowspan', cuadrillaCount);

                                var periodoAnterior = $('#tblPlanMes .cua_' + cuadrillaID + '_per_' + periodo);
                                periodoAnterior.attr('rowspan', periodoCount);

                                cuadrillaID = planes[g].cuadrillaID;
                                cuadrillaCount = 1;
                            } else {
                                cuadrillaCount++;
                            }

                            if ((planes[g].periodo != periodo) || (planes[g].cuadrillaID != cuadrillaIDAnt)) {
                                var tdPeriodo = createElement('td', '', ['text-center', 'periodoText', ('cua_' + planes[g].cuadrillaID + '_per_' + planes[g].periodo)], '', '')
                                tdPeriodo.textContent = planes[g].periodoDesc;
                                tdPeriodo.style.verticalAlign = 'middle';

                                $('#tblPlanMes tbody tr:last').append(tdPeriodo);

                                periodoAnterior = $('#tblPlanMes .cua_' + cuadrillaID + '_per_' + periodo);
                                periodoAnterior.attr('rowspan', periodoCount);

                                periodo = planes[g].periodo;
                                periodoCount = 1;
                            } else {
                                periodoCount++;
                            }

                            var celdaEquipoAreaBody = document.createElement('td');
                            $(celdaEquipoAreaBody).attr('planmesdetalleid', planes[g].detalle[j].id);
                            $(celdaEquipoAreaBody).attr('data-tipo', planes[g].detalle[j].tipo);
                            $(celdaEquipoAreaBody).attr('data-cuadrillaid', planes[g].cuadrillaID);
                            $(celdaEquipoAreaBody).addClass('celdaEquipoAreaBody');

                            var inputEquipoAreaBody = document.createElement('input');
                            inputEquipoAreaBody.classList.add('form-control');
                            inputEquipoAreaBody.classList.add('inputEquipoArea');
                            inputEquipoAreaBody.disabled = true;

                            $(celdaEquipoAreaBody).append(document.createElement('div'));

                            $(celdaEquipoAreaBody).find('div').append(document.createElement('a'));
                            $(celdaEquipoAreaBody).find('a').text(planes[g].detalle[j].equipoAreaDesc);
                            $(celdaEquipoAreaBody).find('a').attr('href', '#');

                            $(inputEquipoAreaBody).val(planes[g].detalle[j].equipoAreaDesc);

                            $('#tblPlanMes tbody tr:last').append(celdaEquipoAreaBody);

                            for (k = 0; k < data.length; k++) {
                                var celdaCheck = document.createElement('td');
                                celdaCheck.classList.add('text-center');
                                celdaCheck.style.padding = '0px';
                                celdaCheck.style.verticalAlign = 'middle';
                                celdaCheck.classList.add('celdaCheck');
                                celdaCheck.setAttribute('diaNumero', data[k].Value);
                                celdaCheck.setAttribute('data-cuadrillaid', planes[g].cuadrillaID);

                                if (data[k].Text == 'D') {
                                    celdaCheck.classList.add('diaDomingoCheck');
                                }

                                celdaCheck.style.width = '28px';

                                if (planes[g].detalle[j].dias.includes(data[k].Value)) {
                                    var span = document.createElement('span');
                                    span.classList.add('glyphicon');
                                    span.classList.add('glyphicon-arrow-right');
                                    $(celdaCheck).append(span);
                                    $(celdaCheck).attr('checked', true);
                                    $(celdaCheck).css('width', '28px');

                                    if (planes[g].detalle[j].checkRev) {
                                        $(celdaCheck).addClass('celdaCheckRevision');
                                    }
                                }

                                $('#tblPlanMes tbody tr:last').append(celdaCheck);
                            }

                            cuadrillaIDAnt = planes[g].cuadrillaID;
                            periodoAnt = planes[g].periodo;
                        }

                        cuadrillaAnterior = $('#tblPlanMes .cua_' + cuadrillaID);
                        cuadrillaAnterior.attr('rowspan', cuadrillaCount);

                        periodoAnterior = $('#tblPlanMes .cua_' + cuadrillaID + '_per_' + periodo);
                        periodoAnterior.attr('rowspan', periodoCount);
                    }
                }
            });
        }

        function fnPlanMaestroIndividual(cuadrillaID) {
            var arr = new Array();
            arr.push(cuadrillaID);

            $.ajax({
                url: '/MAZDA/PlanActividades/GetPlanMaestro',
                datatype: "json",
                type: "POST",
                data: { arrCuadrillas: arr },
                success: function (response) {
                    var data = response.data;

                    fillPlanMaestroIndividual(data);
                }
            });
        }

        function fillPlanMaestroIndividual(data) {
            $('#tblRevisionActividades tbody tr').remove();

            var cuadrillaID = 0;
            var cuadrillaCount = 1;
            var cuadrillaIDAnt = 0;

            var periodo = 0;
            var periodoCount = 1;
            var periodoAnt = 0;

            var areaID = 0;
            var areaCount = 1;

            var actividad = "";
            var actividadCount = 1;

            for (i = 0; i < data.length; i++) {
                var html = '';

                html += '<tr  data-actividadID="' + data[i].id + '">';

                if (data[i].cuadrillaID != cuadrillaID) {
                    var cuadrillaAnterior = $('#tblRevisionActividades .cua_' + cuadrillaID);
                    cuadrillaAnterior.attr('rowspan', cuadrillaCount);

                    var periodoAnterior = $('#tblRevisionActividades .cua_' + cuadrillaID + '_per_' + periodo);
                    periodoAnterior.attr('rowspan', periodoCount);

                    var periodoMesesAnterior = $('#tblRevisionActividades .cua_' + cuadrillaID + '_per_' + periodo).closest('tr').find('.mes');
                    periodoMesesAnterior.attr('rowspan', periodoCount);

                    var areaAnterior = $('#tblRevisionActividades .cua_' + cuadrillaID + '_per_' + periodo + '_area_' + areaID);
                    areaAnterior.attr('rowspan', areaCount);

                    cuadrillaID = data[i].cuadrillaID;
                    cuadrillaCount = 1;
                } else {
                    cuadrillaCount++;
                }

                if ((data[i].periodo != periodo) || (data[i].cuadrillaID != cuadrillaIDAnt)) {
                    html += '   <td class="text-center periodoText cua_' + data[i].cuadrillaID + '_per_' + data[i].periodo + '" style="vertical-align: middle;">';
                    html += '       ' + data[i].periodoDesc;
                    html += '   </td>';

                    periodoAnterior = $('#tblRevisionActividades .cua_' + cuadrillaID + '_per_' + periodo);
                    periodoAnterior.attr('rowspan', periodoCount);

                    periodoMesesAnterior = $('#tblRevisionActividades .cua_' + cuadrillaID + '_per_' + periodo).closest('tr').find('.mes');
                    periodoMesesAnterior.attr('rowspan', periodoCount);

                    areaAnterior = $('#tblRevisionActividades .cua_' + cuadrillaID + '_per_' + periodo + '_area_' + areaID);
                    areaAnterior.attr('rowspan', areaCount);

                    periodo = data[i].periodo;
                    periodoCount = 1;
                } else {
                    periodoCount++;
                }

                if ((data[i].areaID != areaID) || (data[i].periodo != periodoAnt)) {
                    html += '   <td class="text-center areaText cua_' + data[i].cuadrillaID + '_per_' + data[i].periodo + '_area_' + data[i].areaID + '" style="vertical-align: middle;">';
                    html += '       ' + data[i].area;
                    html += '   </td>';

                    areaAnterior = $('#tblRevisionActividades .cua_' + cuadrillaID + '_per_' + periodo + '_area_' + areaID);
                    areaAnterior.attr('rowspan', areaCount);

                    areaID = data[i].areaID;
                    areaCount = 1;
                } else {
                    areaCount++;
                }

                if (data[i].descripcion.trim() != actividad.trim()) {
                    html += '   <td class="text-left cua_' + data[i].cuadrillaID + '_per_' + data[i].periodo + '_area_' + data[i].areaID + '_act_' + data[i].id + '" style="vertical-align: middle;">';
                    html += '       ' + data[i].descripcion;
                    html += '   </td>';

                    var actividadAnterior = $('#tblRevisionActividades tbody td[class*="' + 'cua_' + cuadrillaIDAnt + '_per_' + periodoAnt + '"]' + ':contains(' + actividad.trim() + ')');

                    var actividadAnteriorFiltrado = actividadAnterior.filter(function () {
                        return $(this).text().trim() === actividad.trim();
                    });

                    actividadAnteriorFiltrado.attr('rowspan', actividadCount);

                    actividad = data[i].descripcion.trim();
                    actividadCount = 1;
                } else {
                    actividadCount++;
                }

                html += '<td class="text-center">';
                html += '   <div class="radioBtn btn-group">';
                html += '       <a class="btn btn-success notActive" data-toggle="radRealizo' + i + '" data-title="true">';
                html += '           <i class="fa fa-check"></i>';
                html += '       </a>';
                html += '       <a class="btn btn-danger active" data-toggle="radRealizo' + i + '" data-title="false">';
                html += '           <i class="fa fa-close"></i>';
                html += '       </a>';
                html += '   </div>';
                html += '</td>';

                html += '<td>';
                html += '   <input class="form-control" />';
                html += '</td>';

                cuadrillaIDAnt = data[i].cuadrillaID;
                periodoAnt = data[i].periodo;

                html += '</tr>';

                $(html).appendTo($("#tblRevisionActividades tbody"));
            }

            cuadrillaAnterior = $('#tblRevisionActividades .cua_' + cuadrillaID);
            cuadrillaAnterior.attr('rowspan', cuadrillaCount);

            periodoAnterior = $('#tblRevisionActividades .cua_' + cuadrillaID + '_per_' + periodo);
            periodoAnterior.attr('rowspan', periodoCount);

            periodoMesesAnterior = $('#tblRevisionActividades .cua_' + cuadrillaID + '_per_' + periodo).closest('tr').find('.mes');
            periodoMesesAnterior.attr('rowspan', periodoCount);

            areaAnterior = $('#tblRevisionActividades .cua_' + cuadrillaID + '_per_' + periodo + '_area_' + areaID);
            areaAnterior.attr('rowspan', areaCount);

            actividadAnterior = $('#tblRevisionActividades tbody td[class*="' + 'cua_' + cuadrillaIDAnt + '_per_' + periodoAnt + '"]' + ':contains(' + actividad.trim() + ')');

            actividadAnteriorFiltrado = actividadAnterior.filter(function () {
                return $(this).text().trim() === actividad.trim();
            });

            actividadAnteriorFiltrado.attr('rowspan', actividadCount);
        }

        function fillEquiposArea(data, cuadrillaID, periodoID) {

            $('#tblEquiposArea thead tr').remove();
            $('#tblEquiposArea tbody tr').remove();

            const filaHeader1 = document.createElement('tr');
            $('#tblEquiposArea thead').append(filaHeader1);

            const filaHeader2 = document.createElement('tr');
            $('#tblEquiposAreaDias thead').append(filaHeader2);

            let celdaEquipo = document.createElement('th');
            celdaEquipo.innerText = 'Equipo';
            celdaEquipo.setAttribute('rowspan', '3');
            celdaEquipo.classList.add('text-center');
            celdaEquipo.style.verticalAlign = 'middle';
            celdaEquipo.style.minWidth = "250px";

            let celdaCantidad = document.createElement('th');
            celdaCantidad.innerText = 'Cantidad';
            celdaCantidad.setAttribute('rowspan', '3');
            celdaCantidad.classList.add('text-center');
            celdaCantidad.style.verticalAlign = 'middle';
            celdaCantidad.style.minWidth = "50px";

            let celdaTipo = document.createElement('th');
            celdaTipo.innerText = 'Tipo/Caracteristicas';
            celdaTipo.setAttribute('rowspan', '3');
            celdaTipo.classList.add('text-center');
            celdaTipo.style.verticalAlign = 'middle';
            celdaTipo.style.minWidth = "250px";

            let celdaSubarea = document.createElement('th');
            celdaSubarea.innerText = 'Subárea';
            celdaSubarea.setAttribute('rowspan', '3');
            celdaSubarea.classList.add('text-center');
            celdaSubarea.style.verticalAlign = 'middle';
            celdaSubarea.style.minWidth = "250px";

            $('#tblEquiposArea thead tr:eq(0)').append(celdaEquipo);
            $('#tblEquiposArea thead tr:eq(0)').append(celdaCantidad);
            $('#tblEquiposArea thead tr:eq(0)').append(celdaTipo);
            $('#tblEquiposArea thead tr:eq(0)').append(celdaSubarea);

            // MESES CABECERA
            for (i = 0; i < monthNames.length; i++) {
                let celdaMes = document.createElement('th');
                celdaMes.innerText = monthNames[i];
                celdaMes.setAttribute("data-mes", i + 1);
                celdaMes.setAttribute("data-cuadrillaID", cuadrillaID);
                celdaMes.setAttribute("data-periodoID", periodoID);
                celdaMes.setAttribute("colspan", daysInMonth(fecha.getFullYear(), i + 1));
                celdaMes.classList.add('text-center');
                celdaMes.style.border = "1px solid black";

                //$('#tblEquiposArea thead tr:eq(0)').append(celdaMes);
                $('#tblEquiposAreaDias thead tr:eq(0)').append(celdaMes);
            }

            // EQUIPO DETALLE
            for (j = 0; j < data.length; j++) {
                let fila = document.createElement('tr');
                fila.classList.add('border_bottom');
                $('#tblEquiposArea tbody').append(fila);

                let celdaEquipoBody = document.createElement('td');
                $(celdaEquipoBody).append(document.createElement('div'));
                $(celdaEquipoBody).find('div').append(document.createElement('label')).attr('style', 'text-align: left');
                $(celdaEquipoBody).find('label').text(data[j].descripcion).attr('style', 'font-weight: normal !important');
                celdaEquipoBody.setAttribute("data-equipoID", data[j].id);

                let celdaCantidadBody = document.createElement('td');
                $(celdaCantidadBody).append(document.createElement('div'));
                $(celdaCantidadBody).find('div').append(document.createElement('label')).attr('style', 'text-align: center');
                $(celdaCantidadBody).find('label').text(data[j].cantidad).attr('style', 'font-weight: normal !important; text-align: center');

                let celdaTipoBody = document.createElement('td');
                $(celdaTipoBody).append(document.createElement('div'));
                $(celdaTipoBody).find('div').append(document.createElement('label'));
                $(celdaTipoBody).find('label').text(data[j].caracteristicas).attr('style', 'font-weight: normal !important');

                let celdaSubareaBody = document.createElement('td');
                $(celdaSubareaBody).append(document.createElement('div'));
                $(celdaSubareaBody).find('div').append(document.createElement('label'));
                $(celdaSubareaBody).find('label').text(data[j].subArea).attr('style', 'font-weight: normal !important');

                let celdafalsa = document.createElement('td');
                celdafalsa.style.borderLeftStyle = "hidden";
                $(celdafalsa).append(document.createElement('div'));
                $(celdafalsa).find('div').append(document.createElement('label'));

                $('#tblEquiposArea tbody tr:eq(' + j + ')').append(celdaEquipoBody);
                $('#tblEquiposArea tbody tr:eq(' + j + ')').append(celdaCantidadBody);
                $('#tblEquiposArea tbody tr:eq(' + j + ')').append(celdaTipoBody);
                $('#tblEquiposArea tbody tr:eq(' + j + ')').append(celdaSubareaBody);
                $('#tblEquiposArea tbody tr:eq(' + j + ')').append(celdafalsa);

            }

            getDays();
        }

        function columnasFalsas() {
            const filaHeader1 = document.createElement('tr');
            $('#tblEquiposArea thead').append(filaHeader1);
            const filaHeader2 = document.createElement('tr');
            $('#tblEquiposArea thead').append(filaHeader2);
            const filaHeader3 = document.createElement('tr');
            $('#tblEquiposArea thead').append(filaHeader3);

            let thE = document.createElement('th');
            thE.style.height = "31px";
            thE.classList.add('noBorder');
            $('#tblEquiposArea thead tr:eq(0)').append(thE);

            let thE2 = document.createElement('th');
            thE2.style.height = "35px";
            thE2.classList.add('noBorder');
            $('#tblEquiposArea thead tr:eq(1)').append(thE2);

            let thE3 = document.createElement('th');
            thE3.style.height = "35px";
            thE3.classList.add('noBorderTop');
            $('#tblEquiposArea thead tr:eq(2)').append(thE3);
        }

        function getDays() {
            $.ajax({
                url: '/MAZDA/PlanActividades/GetAllDays',
                datatype: "json",
                type: "POST",
                data: { year: fecha.getFullYear() },
                success: function (response) {
                    const diasMes = response.diasMes

                    columnasFalsas();

                    const filaDias1 = document.createElement('tr');
                    $('#tblEquiposAreaDias thead').append(filaDias1);
                    const filaDias2 = document.createElement('tr');
                    $('#tblEquiposAreaDias thead').append(filaDias2);
                    const filaDias3 = document.createElement('tr');
                    $('#tblEquiposAreaDias thead').append(filaDias3);


                    for (i = 0; i < diasMes.length; i++) {
                        const letra = document.createElement('th');
                        letra.innerText = diasMes[i].Text;
                        letra.classList.add('diaLetra');
                        letra.classList.add('text-center');

                        const numero = document.createElement('th');
                        numero.innerText = diasMes[i].Value;
                        numero.classList.add('diaNumero');
                        numero.classList.add('text-center');

                        if (diasMes[i].Value == '1') {
                            letra.style.borderLeft = "1px solid black";

                            numero.style.borderLeft = "1px solid black";
                        }

                        if (diasMes[i].Text == 'D') {
                            letra.classList.add('diaDomingoLetra');
                            numero.classList.add('diaDomingoNumero');
                        }

                        $('#tblEquiposAreaDias thead tr:eq(1)').append(letra);
                        $('#tblEquiposAreaDias thead tr:eq(2)').append(numero);
                    }

                    getPlanDetalle(diasMes);

                }
            });
        }

        function getPlanDetalle(diasMes) {
            //const meses = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
            let equiposIDarr = [];
            let cuadrillaID = '';
            let periodoID = '';

            $('#tblEquiposArea > tbody  > tr').each(function (index) {
                equiposIDarr.push($('#tblEquiposArea > tbody  > tr:eq(' + index + ') > td:eq(0)').attr('data-equipoID'));
                cuadrillaID = $('#tblEquiposAreaDias > thead > tr > th:eq(4) ').attr('data-cuadrillaID');
                periodoID = $('#tblEquiposAreaDias > thead > tr > th:eq(4) ').attr('data-periodoID');
            });

            $.post('/MAZDA/PlanActividades/GetPlanMesEquipo', { cuadrillaID: cuadrillaID, periodo: periodoID, equipoID: equiposIDarr })
                .done(response => {
                    plan = response.data;

                    $.post('/MAZDA/PlanActividades/getRevisionActividadEquipo', { equipoID: equiposIDarr, cuadrillaID: cuadrillaID, areaID: areaID })
                        .done(response => {
                            debugger;
                            revision = response.revision;

                            makeTablePlanRevision(diasMes, plan, revision, equiposIDarr);
                        });
                });
        }

        function makeTablePlanRevision(diasMes, plan, revision, equiposIDarr) {
            debugger;
            let startRowDetalle = 0;
            let startRowMerge = 0;

            equiposIDarr.forEach(function callback(equipoID, index, array) {
                startRowMerge = startRowDetalle + 2;

                const filaDias1 = document.createElement('tr');
                $('#tblEquiposAreaDias tbody').append(filaDias1);
                const filaDias2 = document.createElement('tr');
                $('#tblEquiposAreaDias tbody').append(filaDias2);
                const filaDias3 = document.createElement('tr');
                $('#tblEquiposAreaDias tbody').append(filaDias3);

                for (let k = 0; k < diasMes.length; k++) {

                    var celdaPlan = document.createElement('td');
                    celdaPlan.classList.add('text-center');
                    celdaPlan.style.height = "5px";
                    celdaPlan.style.width = "5px";
                    celdaPlan.style.verticalAlign = 'middle';
                    celdaPlan.classList.add('celdaPlan');
                    celdaPlan.setAttribute('diaNumero', diasMes[k].Value);

                    var celdaRevision = document.createElement('td');
                    celdaRevision.classList.add('text-center');
                    celdaRevision.style.height = "5px";
                    celdaRevision.style.width = "5px";
                    celdaRevision.style.verticalAlign = 'middle';
                    celdaRevision.classList.add('celdaRevision');

                    var celdaReprogramacion = document.createElement('td');
                    celdaReprogramacion.classList.add('text-center');
                    celdaReprogramacion.style.height = "5px";
                    celdaReprogramacion.style.width = "5px";
                    celdaReprogramacion.style.verticalAlign = 'middle';
                    celdaReprogramacion.classList.add('celdaReprogramacion');


                    //COMPARA LOS DIAS DEL PLAN MAESTRO DETALLE
                    if (plan != null) {

                        for (let j = 0; j < plan.length; j++) {

                            if (plan[j].mes.toString() == diasMes[k].Prefijo) {

                                for (let i = 0; i < plan[j].detalle.length; i++) {

                                    if (plan[j].detalle[i].equipoID == equipoID) {

                                        if (plan[j].detalle[i].dias.toString().includes(diasMes[k].Value)) {

                                            switch (periodoID) {
                                                case "1":
                                                    $(celdaPlan).attr('style', 'background-color: #00b0f0');
                                                    break;
                                                case "2":
                                                    $(celdaPlan).attr('style', 'background-color: #7030a0');
                                                    break;
                                                case "3":
                                                    $(celdaPlan).attr('style', 'background-color: #808080');
                                                    break;
                                                case "4":
                                                    $(celdaPlan).attr('style', 'background-color: #ed7d31');
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }

                    //COMPARA LOS DIAS DE REVISION
                    if (revision != null) {
                        for (let j = 0; j < revision.length; j++) {
                            debugger;
                            if (revision[j].monthFechaCaptura.toString() == diasMes[k].Prefijo) {

                                if (revision[j].dayFechaCaptura.toString() == diasMes[k].Value) {

                                    for (let i = 0; i < revision[j].equiposID.length; i++) {
                                        debugger;
                                        if (revision[j].equiposID[i] == equipoID) {
                                            $(celdaRevision).attr('style', 'background-color: #0b6623');
                                        }
                                    }
                                }
                            }

                            if (revision[j].fechaReprogramacion != null && revision[j].fechaReprogramacion != "") {

                                if (revision[j].monthFechaReprogramacion.toString() == diasMes[k].Prefijo) {

                                    if (revision[j].dayFechaReprogramacion.toString() == diasMes[k].Value) {

                                        for (let i = 0; i < revision[j].equiposID.length; i++) {
                                            debugger;
                                            if (revision[j].equiposID[i] == equipoID) {
                                                $(celdaReprogramacion).attr('style', 'background-color: #ffff00');
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    celdaPlan.classList.add('border_width');
                    celdaRevision.classList.add('border_width');
                    celdaReprogramacion.classList.add('border_width');

                    if (diasMes[k].Value == "1" && diasMes[k].Prefijo == "1") {
                        celdaPlan.style.borderLeftStyle = "solid";
                        celdaRevision.style.borderLeftStyle = "solid";
                        celdaReprogramacion.style.borderLeftStyle = "solid";
                    } else {
                        celdaPlan.style.borderLeftStyle = "Dotted";
                        celdaRevision.style.borderLeftStyle = "Dotted";
                        celdaReprogramacion.style.borderLeftStyle = "Dotted";
                    }

                    if (diasMes[k].Text == "D") {
                        celdaPlan.style.borderRightStyle = "solid";
                        celdaRevision.style.borderRightStyle = "solid";
                        celdaReprogramacion.style.borderRightStyle = "solid";
                    } else {
                        if (diasMes[k].Value == "31" && diasMes[k].Prefijo == "12") {
                            celdaPlan.style.borderRightStyle = "solid";
                            celdaRevision.style.borderRightStyle = "solid";
                            celdaReprogramacion.style.borderRightStyle = "solid";
                        }
                        else {
                            celdaPlan.style.borderRightStyle = "Dotted";
                            celdaRevision.style.borderRightStyle = "Dotted";
                            celdaReprogramacion.style.borderRightStyle = "Dotted";
                        }
                    }

                    celdaPlan.style.borderTopStyle = "Dotted";
                    celdaRevision.style.borderTopStyle = "Dotted";
                    celdaReprogramacion.style.borderTopStyle = "Dotted";

                    celdaPlan.style.borderBottomStyle = "Dotted";
                    celdaRevision.style.borderBottomStyle = "Dotted";
                    celdaReprogramacion.style.borderBottomStyle = "Dotted";

                    celdaReprogramacion.style.borderBottomStyle = "solid";

                    $('#tblEquiposAreaDias tbody tr:eq(' + (startRowDetalle) + ')').append(celdaPlan);
                    $('#tblEquiposAreaDias tbody tr:eq(' + (startRowDetalle + 1) + ')').append(celdaRevision);
                    $('#tblEquiposAreaDias tbody tr:eq(' + (startRowDetalle + 2) + ')').append(celdaReprogramacion);
                }

                startRowDetalle = startRowMerge + 1;
            });

        }

        init();
    };

    $(document).ready(function () {
        planActividades.gestion = new gestion();
    })
        .ajaxStart(function () {
            $.blockUI({
                message: 'Procesando...',
                baseZ: 2000
            });
        })
        .ajaxStop(function () { $.unblockUI(); });
})();