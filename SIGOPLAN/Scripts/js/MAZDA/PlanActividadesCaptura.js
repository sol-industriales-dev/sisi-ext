(function () {

    $.namespace('planActividades.captura');

    captura = function () {

        btnCargar_PlanMes = $('#btnCargar_PlanMes');
        btnAgregarEquipoArea = $('#btnAgregarEquipoArea');
        btnGuardarPlanMes = $('#btnGuardarPlanMes');

        $("#selectCuadrilla").change(function () {
            if ($(this).val() != "") {
                var arr = new Array();
                arr.push(parseInt($(this).val()));

                $("#selectArea").fillCombo('/MAZDA/PlanActividades/GetAreasList', { cuadrillasID: arr }, false);

                $("#selectPeriodo").fillCombo('/MAZDA/PlanActividades/GetPeriodosList', { cuadrillasID: arr }, false);
                $('#selectPeriodo').change();
            } else {
                $("#selectArea").fillCombo('/MAZDA/PlanActividades/GetAreasList', { cuadrillasID: -1 }, false);

                $("#selectPeriodo").fillCombo('/MAZDA/PlanActividades/GetPeriodosList', null, false);
                $('#selectPeriodo').change();
            }

            if ($('#selectCuadrilla').val() != "" && $('#selectPeriodo').val() != "" && $('#selectMes').val() != "") {
                fnCargarPlanMes();
            }
        });

        $("#selectPeriodo").change(function () {
            if ($(this).val() != "") {
                $("#selectMes").fillCombo('/MAZDA/PlanActividades/GetMesesList', { periodo: $(this).val() }, false);
            } else {
                $("#selectMes").fillCombo('/MAZDA/PlanActividades/GetMesesList', { periodo: 0 }, false);
            }

            if ($('#selectCuadrilla').val() != "" && $('#selectPeriodo').val() != "" && $('#selectMes').val() != "") {
                fnCargarPlanMes();
            }
        });

        $("#selectMes").change(function () {
            if ($('#selectCuadrilla').val() != "" && $('#selectArea').val() != "" && $('#selectPeriodo').val() != "" && $('#selectMes').val() != "") {
                fnCargarPlanMes();
            }
        });

        $('#tblPlanMes tbody').on('click', '.celdaCheck', function () {
            if (!($(this).hasClass('diaDomingoCheck'))) {
                if ($(this).attr('checked') != 'checked') {
                    var span = document.createElement('span');
                    //span.classList.add('badge');
                    //span.textContent = '>';
                    span.classList.add('glyphicon');
                    span.classList.add('glyphicon-arrow-right');

                    $(this).append(span);
                    //$(this).css('padding', '0px');
                    //$(this).css('vertical-align', 'middle');

                    $(this).attr('checked', true);
                    $(this).css('width', '28px');
                } else {
                    $(this).find('span').remove();
                    $(this).attr('checked', false);
                    $(this).css('width', '28px');
                }

            }
        });

        $('#tblPlanMes tbody').on('click', '.elimEquipoArea', function () {
            if ($('#tblPlanMes tbody tr').length == 1) {
                cargarBody();
            } else {
                var row = $(this).closest('tr');

                row.remove();
            }
        });

        function init() {
            var dt = new Date();
            $('#txtEncabezado').text('Captura Plan Maestro ' + dt.getFullYear());

            initCbo();
            initTable();

            btnCargar_PlanMes.click(fnCargarPlanMes);
            btnAgregarEquipoArea.click(fnAgregarEquipoArea);
            btnGuardarPlanMes.click(fnGuardarPlanMes);
        }

        function initCbo() {
            $("#selectCuadrilla").fillCombo('/MAZDA/PlanActividades/GetCuadrillasList', null, false);
            $("#selectPeriodo").fillCombo('/MAZDA/PlanActividades/GetPeriodosList', null, false);
            $("#selectMes").fillCombo('/MAZDA/PlanActividades/GetMesesList', { periodo: 0 }, false);
        }

        function fnCargarPlanMes() {
            if ($("#selectCuadrilla").val() != "" && $('#selectArea').val() != "" && $("#selectPeriodo").val() != "" && $("#selectMes").val() != "") {
                $.ajax({
                    url: '/MAZDA/PlanActividades/GetPlanMes',
                    datatype: "json",
                    type: "POST",
                    data: {
                        cuadrillaID: parseInt($("#selectCuadrilla").val()),
                        periodo: parseInt($("#selectPeriodo").val()),
                        mes: parseInt($("#selectMes").val())
                    },
                    success: function (response) {
                        var data = response.data;

                        if (data != null) {
                            fnFillPlan(data);
                        } else {
                            fnPlanNuevo();
                        }
                    }
                });

                $('#btnAgregarEquipoArea').css('display', 'inline-block');
                $('#btnGuardarPlanMes').css('display', 'inline-block');
            } else {
                initTable();

                $('#btnAgregarEquipoArea').css('display', 'none');
                $('#btnGuardarPlanMes').css('display', 'none');
            }
        }

        function initTable() {
            $('#tblPlanMes thead tr').remove();
            $('#tblPlanMes tbody tr').remove();

            var fechaHoy = new Date();

            $.ajax({
                url: '/MAZDA/PlanActividades/GetDiasMes',
                datatype: "json",
                type: "POST",
                data: { year: fechaHoy.getFullYear(), month: fechaHoy.getMonth() + 1 },
                success: function (response) {
                    var data = response.items;

                    var filaHeader1 = document.createElement('tr');
                    $('#tblPlanMes thead').append(filaHeader1);
                    var filaHeader2 = document.createElement('tr');
                    $('#tblPlanMes thead').append(filaHeader2);

                    var celdaElim = document.createElement('th');
                    celdaElim.setAttribute('rowspan', '2');
                    $('#tblPlanMes thead tr:eq(0)').append(celdaElim);

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

                    var fila = document.createElement('tr');
                    $('#tblPlanMes tbody').append(fila);

                    var celdaElimBody = document.createElement('td');
                    celdaElimBody.classList.add('text-center');
                    celdaElimBody.style.padding = '3px';
                    celdaElimBody.style.verticalAlign = 'middle';

                    $('#tblPlanMes tbody  tr').append(celdaElimBody);

                    var btnElim = document.createElement('button');
                    btnElim.classList.add('btn');
                    btnElim.classList.add('btn-danger');
                    btnElim.classList.add('btn-xs');
                    btnElim.classList.add('elimEquipoArea');
                    btnElim.setAttribute('tabIndex', '-1');

                    var spanElim = document.createElement('span');
                    spanElim.classList.add('glyphicon');
                    spanElim.classList.add('glyphicon-remove');

                    btnElim.disabled = true;

                    btnElim.appendChild(spanElim);
                    celdaElimBody.appendChild(btnElim);

                    celdaElimBody.style.width = '50px';

                    var celdaEquipoAreaBody = document.createElement('td');
                    celdaEquipoAreaBody.style.padding = '3px';

                    var selectEquipoAreaBody = document.createElement('select');
                    selectEquipoAreaBody.classList.add('form-control');
                    selectEquipoAreaBody.classList.add('selectEquipoArea');
                    selectEquipoAreaBody.disabled = true;

                    celdaEquipoAreaBody.appendChild(selectEquipoAreaBody);

                    $('#tblPlanMes tbody tr').append(celdaEquipoAreaBody);

                    for (i = 0; i < data.length; i++) {
                        var celdaCheck = document.createElement('td');
                        celdaCheck.classList.add('text-center');
                        celdaCheck.setAttribute('diaNumero', data[i].Value);

                        if (data[i].Text == 'D') {
                            celdaCheck.classList.add('diaDomingoCheck');
                            //celdaCheck.disabled = true;
                        }

                        celdaCheck.style.width = '28px';

                        $('#tblPlanMes tbody tr').append(celdaCheck);
                    }
                }
            });
        }

        function fnFillPlan(plan) {
            var dt = new Date();

            $.ajax({
                url: '/MAZDA/PlanActividades/GetDiasMes',
                datatype: "json",
                type: "POST",
                data: { year: dt.getFullYear(), month: $('#selectMes').val() },
                success: function (response) {
                    var data = response.items;

                    $('#tblPlanMes thead tr').remove();
                    $('#tblPlanMes tbody tr').remove();

                    var filaHeader1 = document.createElement('tr');
                    $('#tblPlanMes thead').append(filaHeader1);
                    var filaHeader2 = document.createElement('tr');
                    $('#tblPlanMes thead').append(filaHeader2);

                    var celdaElim = document.createElement('th');
                    celdaElim.setAttribute('rowspan', '2');
                    $('#tblPlanMes thead tr:eq(0)').append(celdaElim);

                    var celdaEquipo = document.createElement('th');
                    celdaEquipo.innerText = 'EQUIPO';
                    celdaEquipo.setAttribute('rowspan', '2');
                    celdaEquipo.classList.add('text-center');
                    celdaEquipo.style.verticalAlign = 'middle';
                    $('#tblPlanMes thead tr:eq(0)').append(celdaEquipo);

                    debugger;
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
                        $('#tblPlanMes tbody').append(fila);

                        var celdaElimBody = document.createElement('td');
                        celdaElimBody.classList.add('text-center');
                        celdaElimBody.style.padding = '3px';
                        celdaElimBody.style.verticalAlign = 'middle';

                        $('#tblPlanMes tbody  tr:eq(' + j + ')').append(celdaElimBody);
                        var btnElim = document.createElement('button');
                        btnElim.classList.add('btn');
                        btnElim.classList.add('btn-danger');
                        btnElim.classList.add('btn-xs');
                        btnElim.classList.add('elimEquipoArea');
                        btnElim.setAttribute('tabIndex', '-1');

                        var spanElim = document.createElement('span');
                        spanElim.classList.add('glyphicon');
                        spanElim.classList.add('glyphicon-remove');

                        btnElim.disabled = true;

                        btnElim.appendChild(spanElim);
                        celdaElimBody.appendChild(btnElim);

                        celdaElimBody.style.width = '50px';

                        var celdaEquipoBody = document.createElement('td');
                        celdaEquipoBody.style.padding = '3px';

                        var selectEquipoBody = document.createElement('select');
                        selectEquipoBody.classList.add('form-control');
                        selectEquipoBody.classList.add('selectEquipoBody');

                        celdaEquipoBody.appendChild(selectEquipoBody);

                        $(selectEquipoBody).fillCombo('/MAZDA/PlanActividades/GetEquiposList', { arrCuadrillas: $('#selectCuadrilla').val(), arrAreas: $('#selectArea').val() }, false);
                        $(selectEquipoBody).val(plan.detalle[j].equipoID);

                        $('#tblPlanMes tbody tr:eq(' + j + ')').append(celdaEquipoBody);

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

                            if (plan.detalle[j].dias.toString().includes(data[k].Value)) {
                                var span = document.createElement('span');
                                span.classList.add('glyphicon');
                                span.classList.add('glyphicon-arrow-right');
                                $(celdaCheck).append(span);
                                $(celdaCheck).attr('checked', true);
                                $(celdaCheck).css('width', '28px');
                            }

                            $('#tblPlanMes tbody tr:eq(' + j + ')').append(celdaCheck);
                        }
                    }

                    $('.elimEquipoArea').attr('disabled', false);
                }
            });
        }

        function fnPlanNuevo() {
            var dt = new Date();

            $.ajax({
                url: '/MAZDA/PlanActividades/GetDiasMes',
                datatype: "json",
                type: "POST",
                data: { year: dt.getFullYear(), month: $('#selectMes').val() },
                success: function (response) {
                    var data = response.items;

                    $('#tblPlanMes thead tr').remove();
                    $('#tblPlanMes tbody tr').remove();

                    var filaHeader1 = document.createElement('tr');
                    $('#tblPlanMes thead').append(filaHeader1);
                    var filaHeader2 = document.createElement('tr');
                    $('#tblPlanMes thead').append(filaHeader2);

                    var celdaElim = document.createElement('th');
                    celdaElim.setAttribute('rowspan', '2');
                    $('#tblPlanMes thead tr:eq(0)').append(celdaElim);

                    var celdaEquipo = document.createElement('th');
                    celdaEquipo.innerText = 'EQUIPO';
                    celdaEquipo.setAttribute('rowspan', '2');
                    celdaEquipo.classList.add('text-center');
                    celdaEquipo.style.verticalAlign = 'middle';
                    $('#tblPlanMes thead tr:eq(0)').append(celdaEquipo);

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

                    var fila = document.createElement('tr');
                    $('#tblPlanMes tbody').append(fila);

                    var celdaElimBody = document.createElement('td');
                    celdaElimBody.classList.add('text-center');
                    celdaElimBody.style.padding = '3px';
                    celdaElimBody.style.verticalAlign = 'middle';

                    $('#tblPlanMes tbody  tr').append(celdaElimBody);
                    var btnElim = document.createElement('button');
                    btnElim.classList.add('btn');
                    btnElim.classList.add('btn-danger');
                    btnElim.classList.add('btn-xs');
                    btnElim.classList.add('elimEquipoArea');
                    btnElim.setAttribute('tabIndex', '-1');

                    var spanElim = document.createElement('span');
                    spanElim.classList.add('glyphicon');
                    spanElim.classList.add('glyphicon-remove');

                    btnElim.disabled = true;

                    btnElim.appendChild(spanElim);
                    celdaElimBody.appendChild(btnElim);

                    celdaElimBody.style.width = '50px';

                    var celdaEquipoBody = document.createElement('td');
                    celdaEquipoBody.style.padding = '3px';

                    var selectEquipoBody = document.createElement('select');
                    selectEquipoBody.classList.add('form-control');
                    selectEquipoBody.classList.add('selectEquipoBody');

                    celdaEquipoBody.appendChild(selectEquipoBody);

                    debugger;
                    $(selectEquipoBody).fillCombo('/MAZDA/PlanActividades/GetEquiposList', { arrCuadrillas: $('#selectCuadrilla').val(), arrAreas: $('#selectArea').val() }, false);

                    $('#tblPlanMes tbody tr').append(celdaEquipoBody);

                    for (i = 0; i < data.length; i++) {
                        var celdaCheck = document.createElement('td');
                        celdaCheck.classList.add('text-center');
                        celdaCheck.style.padding = '0px';
                        celdaCheck.style.verticalAlign = 'middle';
                        celdaCheck.classList.add('celdaCheck');
                        celdaCheck.setAttribute('diaNumero', data[i].Value);

                        if (data[i].Text == 'D') {
                            celdaCheck.classList.add('diaDomingoCheck');
                        }

                        celdaCheck.style.width = '28px';

                        $('#tblPlanMes tbody tr').append(celdaCheck);
                    }
                }
            });
        }

        function fnAgregarEquipoArea() {
            var dt = new Date();

            $.ajax({
                url: '/MAZDA/PlanActividades/GetDiasMes',
                datatype: "json",
                type: "POST",
                data: { year: dt.getFullYear(), month: $('#selectMes').val() },
                success: function (response) {
                    var data = response.items;

                    var fila = document.createElement('tr');
                    $('#tblPlanMes tbody').append(fila);

                    var celdaElimBody = document.createElement('td');
                    celdaElimBody.classList.add('text-center');
                    celdaElimBody.style.padding = '3px';
                    celdaElimBody.style.verticalAlign = 'middle';

                    $('#tblPlanMes tbody  tr:last').append(celdaElimBody);
                    var btnElim = document.createElement('button');
                    btnElim.classList.add('btn');
                    btnElim.classList.add('btn-danger');
                    btnElim.classList.add('btn-xs');
                    btnElim.classList.add('elimEquipoArea');
                    btnElim.setAttribute('tabIndex', '-1');

                    var spanElim = document.createElement('span');
                    spanElim.classList.add('glyphicon');
                    spanElim.classList.add('glyphicon-remove');

                    $('.elimEquipoArea').attr('disabled', false);

                    btnElim.appendChild(spanElim);
                    celdaElimBody.appendChild(btnElim);

                    celdaElimBody.style.width = '50px';

                    var celdaEquipoBody = document.createElement('td');
                    celdaEquipoBody.style.padding = '3px';

                    var selectEquipoBody = document.createElement('select');
                    selectEquipoBody.classList.add('form-control');
                    selectEquipoBody.classList.add('selectEquipoArea');

                    celdaEquipoBody.appendChild(selectEquipoBody);

                    $(selectEquipoBody).fillCombo('/MAZDA/PlanActividades/GetEquiposList', { arrCuadrillas: $('#selectCuadrilla').val(), arrAreas: $('#selectArea').val() }, false);

                    $('#tblPlanMes tbody tr:last').append(celdaEquipoBody);

                    for (i = 0; i < data.length; i++) {
                        var celdaCheck = document.createElement('td');
                        celdaCheck.classList.add('text-center');
                        celdaCheck.style.padding = '0px';
                        celdaCheck.style.verticalAlign = 'middle';
                        celdaCheck.classList.add('celdaCheck');
                        celdaCheck.setAttribute('diaNumero', data[i].Value);

                        if (data[i].Text == 'D') {
                            celdaCheck.classList.add('diaDomingoCheck');
                        }

                        celdaCheck.style.width = '28px';

                        $('#tblPlanMes tbody tr:last').append(celdaCheck);
                    }
                }
            });
        }

        function cargarBody() {
            $('#tblPlanMes tbody tr').remove();

            var dt = new Date();

            $.ajax({
                url: '/MAZDA/PlanActividades/GetDiasMes',
                datatype: "json",
                type: "POST",
                data: { year: dt.getFullYear(), month: $('#selectMes').val() },
                success: function (response) {
                    var data = response.items;

                    var fila = document.createElement('tr');
                    $('#tblPlanMes tbody').append(fila);

                    var celdaElimBody = document.createElement('td');
                    celdaElimBody.classList.add('text-center');
                    celdaElimBody.style.padding = '3px';
                    celdaElimBody.style.verticalAlign = 'middle';

                    $('#tblPlanMes tbody  tr').append(celdaElimBody);
                    var btnElim = document.createElement('button');
                    btnElim.classList.add('btn');
                    btnElim.classList.add('btn-danger');
                    btnElim.classList.add('btn-xs');
                    btnElim.classList.add('elimEquipoArea');
                    btnElim.setAttribute('tabIndex', '-1');

                    var spanElim = document.createElement('span');
                    spanElim.classList.add('glyphicon');
                    spanElim.classList.add('glyphicon-remove');

                    btnElim.disabled = true;

                    btnElim.appendChild(spanElim);
                    celdaElimBody.appendChild(btnElim);

                    celdaElimBody.style.width = '50px';

                    var celdaEquipoBody = document.createElement('td');
                    celdaEquipoBody.style.padding = '3px';

                    var selectEquipoBody = document.createElement('select');
                    selectEquipoBody.classList.add('form-control');
                    selectEquipoBody.classList.add('selectEquipoArea');

                    celdaEquipoBody.appendChild(selectEquipoBody);

                    $(selectEquipoBody).fillCombo('/MAZDA/PlanActividades/GetEquiposList', { arrCuadrillas: $('#selectCuadrilla').val(), arrAreas: $('#selectArea').val() }, false);


                    $('#tblPlanMes tbody tr').append(selectEquipoBody);

                    for (i = 0; i < data.length; i++) {
                        var celdaCheck = document.createElement('td');
                        celdaCheck.classList.add('text-center');
                        celdaCheck.style.padding = '0px';
                        celdaCheck.style.verticalAlign = 'middle';
                        celdaCheck.classList.add('celdaCheck');
                        celdaCheck.setAttribute('diaNumero', data[i].Value);

                        if (data[i].Text == 'D') {
                            celdaCheck.classList.add('diaDomingoCheck');
                        }

                        celdaCheck.style.width = '28px';

                        $('#tblPlanMes tbody tr').append(celdaCheck);
                    }
                }
            });
        }

        function cargarHeader(year, month) {
            $.ajax({
                url: '/MAZDA/PlanActividades/GetDiasMes',
                datatype: "json",
                type: "POST",
                data: { year: year, month: month },
                success: function (response) {
                    var data = response.items;

                    $('#tblPlanMes thead tr').remove();
                    $('#tblPlanMes tbody tr').remove();

                    var filaHeader1 = document.createElement('tr');
                    $('#tblPlanMes thead').append(filaHeader1);
                    var filaHeader2 = document.createElement('tr');
                    $('#tblPlanMes thead').append(filaHeader2);

                    var celdaElim = document.createElement('th');
                    celdaElim.setAttribute('rowspan', '2');
                    $('#tblPlanMes thead tr:eq(0)').append(celdaElim);

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

                        $('#tblPlanMes thead tr:eq(0)').append(letra);
                        $('#tblPlanMes thead tr:eq(1)').append(numero);
                    }
                }
            });
        }

        function fnGuardarPlanMes() {
            var detalle = new Array();
            var flagSelectEquipoArea = false;

            $('#tblPlanMes tbody tr').each(function (index) {
                var row = $(this);

                var celdasDia = row.find('td[checked="checked"]');
                var arrDias = new Array();

                $(celdasDia).each(function (index) {
                    arrDias.push(parseInt($(this).attr('dianumero')));
                });

                var obj = {
                    equipoID: parseInt(row.find('.selectEquipoBody').val()),
                    //equipoAreaDesc: row.find('.selectEquipoBody').find('option:selected').text().trim(),
                    tipo: parseInt(row.find('.selectEquipoBody').find('option:selected').attr('data-prefijo')),
                    dias: arrDias
                };

                if (row.find('.selectEquipoBody').val() == "") {
                    flagSelectEquipoArea = true;
                }

                detalle.push(obj);
            });

            var dt = new Date();

            var plan = {
                cuadrillaID: $('#selectCuadrilla').val(),
                periodo: $('#selectPeriodo').val(),
                mes: $('#selectMes').val(),
                anio: dt.getFullYear(),
                detalle: detalle
            }

            if ($('#selectCuadrilla').val() != "" && $('#selectArea').val() != "" && $('#selectPeriodo').val() != "" && $('#selectMes').val() != "") {
                if (flagSelectEquipoArea != true || detalle.length == 1) {
                    $.ajax({
                        url: '/MAZDA/PlanActividades/GuardarPlanMes',
                        datatype: "json",
                        type: "POST",
                        data: {
                            plan: plan
                        },
                        success: function (response) {
                            AlertaGeneral('Información Guardada', 'Se ha guardado la información del plan.');
                            fnCargarPlanMes();
                        }
                    });
                } else {
                    AlertaGeneral('Alerta', 'Seleccione todos los equipos.');
                }
            } else {
                AlertaGeneral('Alerta', 'Seleccione la cuadrilla, el periodo y  el mes del plan')
            }
        }

        init();
    };

    $(document).ready(function () {
        planActividades.captura = new captura();
    })

        .ajaxStart(function () {
            $.blockUI({
                message: 'Procesando...',
                baseZ: 2000
            });
        })
        .ajaxStop(function () { $.unblockUI(); });
})();