(function () {
    $.namespace('planActividades.revisionCuadrilla');
    revisionCuadrilla = function () {

        tblRevisionActividades = $('#tblRevisionActividades');

        //AC
        inputObs = $("#inputObs");
        divGaleria = $("#divGaleria");
        btnShowMdl = $("#btnShowMdl");
        btnGuardar = $('#btnGuardar');
        btnLimpiar = $("#btnLimpiar");
        selectArea = $("#selectArea");
        selTecnico = $("#selTecnico");
        selAyudante = $("#selAyudante");
        mdlEvidencia = $("#mdlEvidencia");
        selectEquipo = $("#selectEquipo");
        selectPeriodo = $("#selectPeriodo");
        inputTonelaje = $("#inputTonelaje");
        tblEvaporador = $("#tblEvaporador");
        tblCondensador = $("#tblCondensador");
        inputEvidencias = $("#inputEvidencias");
        //AC//

        //Cuadrillas
        selectCuadrilla = $('#selectCuadrilla');
        inputObsCua = $('#inputObsCua');
        btnShowMdlCua = $('#btnShowMdlCua');
        mdlEvidenciaCua = $('#mdlEvidenciaCua');
        btnGuardarCua = $('#btnGuardarCua');
        divGaleriaCua = $('#divGaleriaCua');
        inputEvidenciasCua = $('#inputEvidenciasCua');
        selectMes = $('#selectMes');
        selTecnicoCua = $('#selTecnicoCua');
        selAyudanteCua = $('#selAyudanteCua');
        //Cuadrillas//

        $('#selectCuadrilla').on('change', function () {
            if ($(this).val() != "") {
                if ($(this).find('option:selected').text().trim() != "AIRES ACONDICIONADOS") {
                    $('#divCuadrillaGeneral').css('display', 'block');
                    $('#divCuadrillaAC').css('display', 'none');

                    fnPlanMaestroIndividual();

                    $('#divObservaciones').css('display', 'block');
                } else {
                    $('#divCuadrillaAC').css('display', 'block');
                    $('#divCuadrillaGeneral').css('display', 'none');
                }
            } else {
                $('#tblRevisionActividades tbody tr').remove();
                $('#divObservaciones').css('display', 'none');

                $('#divCuadrillaGeneral').css('display', 'none');
                $('#divCuadrillaAC').css('display', 'none');
            }
        });

        tblRevisionActividades.on('click', 'a', function () {
            if ($(this).hasClass('notActive')) {
                $(this).removeClass('notActive').addClass('active');
                $(this).siblings().removeClass('active').addClass('notActive');
            }
        });

        function init() {
            initCbo();

            //AC
            initForm();
            btnLimpiar.click(setDefault);
            btnShowMdl.click(showModal);
            //btnGuardar.click(GuardarRevision);
            inputEvidencias.change(setGalery);
            selectEquipo.change(setEquipoAC);
            fillTablas();
            //AC//

            //Cuadrillas
            btnShowMdlCua.click(showModalCua);
            inputEvidenciasCua.change(setGaleryCua);
            //btnGuardarCua.click(GuardarRevisionCua);
            //Cuadrillas//
        }

        function initCbo() {
            $('#selectCuadrilla').fillCombo('/MAZDA/PlanActividades/GetCuadrillasList', null, false);

            //AC
            selectEquipo.fillCombo('/MAZDA/PlanActividades/GetEquiposList', null, false);
            selectArea.fillCombo('/MAZDA/PlanActividades/GetAreasList', { cuadrillasID: 1 }, false);
            selectPeriodo.fillCombo('/MAZDA/PlanActividades/GetPeriodosList', null, false);
            selTecnico.fillCombo('/MAZDA/PlanActividades/GetEmpleadoList', null, false);

            $("#selAyudante").fillCombo('/MAZDA/PlanActividades/GetEmpleadoList', { est: true }, false, "Todos");
            convertToMultiselect('#selAyudante');
            //AC//

            //Cuadrillas
            selTecnicoCua.fillCombo('/MAZDA/PlanActividades/GetEmpleadoList', null, false);
            $('#selAyudanteCua').fillCombo('/MAZDA/PlanActividades/GetEmpleadoList', { est: true }, false, "Todos");
            convertToMultiselect('#selAyudanteCua');
            //Cuadrillas//
        }

        function fnPlanMaestroIndividual() {
            var arr = new Array();
            arr.push($('#selectCuadrilla').val());

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

            cuadrillaAnterior = $('.cua_' + cuadrillaID);
            cuadrillaAnterior.attr('rowspan', cuadrillaCount);

            periodoAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo);
            periodoAnterior.attr('rowspan', periodoCount);

            periodoMesesAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo).closest('tr').find('.mes');
            periodoMesesAnterior.attr('rowspan', periodoCount);

            areaAnterior = $('.cua_' + cuadrillaID + '_per_' + periodo + '_area_' + areaID);
            areaAnterior.attr('rowspan', areaCount);

            actividadAnterior = $('#tblRevisionActividades tbody td[class*="' + 'cua_' + cuadrillaIDAnt + '_per_' + periodoAnt + '"]' + ':contains(' + actividad.trim() + ')');

            actividadAnteriorFiltrado = actividadAnterior.filter(function () {
                return $(this).text().trim() === actividad.trim();
            });

            actividadAnteriorFiltrado.attr('rowspan', actividadCount);

            //$('#tblRevisionActividades tbody .cuadrillaText').remove();
            //$('#tblRevisionActividades tbody .mes').remove();
        }

        //AC
        tblCondensador.on('click', 'a', function () {
            if ($(this).hasClass('notActive')) {
                $(this).removeClass('notActive').addClass('active');
                $(this).siblings().removeClass('active').addClass('notActive');
            }
        });
        tblEvaporador.on('click', 'a', function () {
            if ($(this).hasClass('notActive')) {
                $(this).removeClass('notActive').addClass('active');
                $(this).siblings().removeClass('active').addClass('notActive');
            }
        });
        function setEquipoAC() {
            let equipoID = $(this).val();
            if (equipoID.length != 0) {
                GetEquipoAC(equipoID).done(function (response) {
                    if (response.success) {
                        let data = response.data;
                        let areaID = selectArea.find('option[name="' + data.area + '"]').val();
                        inputTonelaje.val(data.tonelaje);
                        selectArea.val(areaID);
                        selectPeriodo.val(data.periodo);
                    }
                });
            } else {
                inputTonelaje.val(0);
                selectArea.val("");
                selectPeriodo.val("");
            }
        }
        function GuardarRevision() {
            var request = new XMLHttpRequest();
            request.open("POST", "/MAZDA/PlanActividades/GuardarRevision");
            request.send(formData());
            request.onload = function (response) {
                if (request.status == 200) {
                    AlertaGeneral("Aviso", "Revision guardada correctamente.");
                    mdlEvidencia.modal("hide");

                    $('#mdlRevisionAC').modal('hide');
                    $('#dialogPlanMes').modal('hide');

                    setDefault();
                    $('#tblCondensador tbody tr').remove();
                    $('#tblEvaporador tbody tr').remove();

                    fillTablas();

                    inputEvidencias.val("");
                    divGaleria.empty();
                    inputObs.val("");
                }
            };
        }
        function GetAllActividadesAC() {
            return $.post("/MAZDA/PlanActividades/GetAllActividadesAC");
        }
        function GetEquipoAC(equipoID) {
            return $.post("/MAZDA/PlanActividades/GetEquipoAC", { equipoID: equipoID });
        }
        function fillTablas() {
            GetAllActividadesAC().done(function (response) {
                if (response.success) {
                    setRows("tblCondensador", response.lstCondensador);
                    setRows("tblEvaporador", response.lstEvaporador);
                }
            });
        }
        function validaGuardado() {
            let ban = true;
            $.each($("#fieldBusquedaAC > input, #fieldBusquedaAC > select"), function (i, e) {
                if (this.value.length == 0 || this.value == 0)
                    ban = false;
            });
            return ban;
        }
        function showModal() {
            if (validaGuardado())
                //mdlEvidencia.modal("show");
                GuardarRevision();
        }
        function formData() {
            let formData = new FormData();
            formData.append("rev", JSON.stringify(getRevision()));
            formData.append("ayu[]", getAyudantes());
            formData.append("det[]", getDetalles());
            $.each(document.getElementById("inputEvidencias").files, function (i, file) {
                formData.append("files[]", file);
            });
            return formData;
        }
        function getDetalles() {
            let det = [];
            tblCondensador.find("tbody tr").each(function (idx, row) {
                let obs = $(this).find('input').val();
                obs = obs.length == 0 ? "" : obs;
                det.push(JSON.stringify({
                    id: 0,
                    revisionID: 0,
                    tipo: 1,
                    actividadID: $(this).data().idActividad,
                    realizo: getRadioValue('radRealizo' + idx, 1),
                    observaciones: obs,
                    estatus: true
                }));
            });
            tblEvaporador.find("tbody tr").each(function (idx, row) {
                let obs = $(this).find('input').val();
                obs = obs.length == 0 ? "" : obs;
                det.push(JSON.stringify({
                    id: 0,
                    revisionID: 0,
                    tipo: 2,
                    actividadID: $(this).data().idActividad,
                    realizo: getRadioValue('radRealizo' + idx, 2),
                    observaciones: obs,
                    estatus: true
                }));
            });
            
            return "[" + det + "]";
        }
        function getAyudantes() {
            let ayu = [];
            
            $(getValoresMultiples('#selAyudante')).each(function (idx, numero) {
                ayu.push(JSON.stringify({
                    id: 0,
                    idPersonal: parseInt(numero),
                    revisionID: 0,
                    estatus: true
                }));
            });
            
            return "[" + ayu + "]";
        }
        function getRevision() {
            return {
                equipoID: selectEquipo.val(),
                tonelaje: inputTonelaje.val(),
                area: selectArea.val(),
                periodo: selectPeriodo.val(),
                tecnico: selTecnico.val() != "" ? selTecnico.val() : 0,
                observaciones: inputObs.val(),
                estatus: true,

                planMesDetalleID: $('#planMesDetalleID').val() || 0
            };
        }
        function setDefault() {
            $.each($("#divCuadrillaAC input, #divCuadrillaAC select:not(#selAyudante)"), function (i, e) {
                this.value = "";
            });
            inputTonelaje.val(0);

            $("#selAyudante").fillCombo('/MAZDA/PlanActividades/GetEmpleadoList', { est: true }, false, "Todos");
            convertToMultiselect('#selAyudante');
        }
        function initForm() {
            initCbo();
            setDefault();
        }
        function getRadioValue(tog, tipoAC) {
            switch (tipoAC) {
                case 1:
                    return $('#divCuadrillaAC #tblCondensador a[data-toggle="' + tog + '"]').closest('td').find('.active').data('title');
                    break;
                case 2:
                    return $('#divCuadrillaAC #tblEvaporador a[data-toggle="' + tog + '"]').closest('td').find('.active').data('title');
                    break;
            }
        }
        function setGalery() {
            let lstFotos = $(this)[0].files;
            $.each(lstFotos, function (i, e) {
                readURL(this);
            });
        }
        var readURL = function (input) {
            var reader = new FileReader();
            let item = $(document.createElement('div'));
            reader.onload = function (e) {
                item.addClass("mkr_SldItem");
                item.append(document.createElement('div'));
                item.find("div").addClass("thumbHolder");
                item.find(".thumbHolder").append(document.createElement("img"));
                item.find("img").attr("src", e.target.result);
                item.find("img").attr("width", "125px");
            }
            reader.readAsDataURL(input);
            divGaleria.append(item);
        }
        function setRows(tbl, data) {
            for (i = 0; i < data.length; i++) {
                var row = document.createElement('tr');
                var celdaDesc = document.createElement('td');
                celdaDesc.textContent = data[i].descripcion;
                var celdaRealizo = document.createElement('td');
                celdaRealizo.classList.add('text-center');
                var divRadio = document.createElement('div');
                divRadio.classList.add('radioBtn');
                divRadio.classList.add('btn-group');
                $(celdaRealizo).append(divRadio);
                var a1 = document.createElement('a');
                a1.classList.add('btn');
                a1.classList.add('btn-success');
                a1.classList.add('notActive');
                a1.setAttribute('data-toggle', 'radRealizo' + i);
                a1.setAttribute('data-title', 'true');
                var i1 = document.createElement('i');
                i1.classList.add('fa');
                i1.classList.add('fa-check');
                $(a1).append(i1);
                $(divRadio).append(a1);
                var a2 = document.createElement('a');
                a2.classList.add('btn');
                a2.classList.add('btn-danger');
                a2.classList.add('active');
                a2.setAttribute('data-toggle', 'radRealizo' + i);
                a2.setAttribute('data-title', 'false');
                var i2 = document.createElement('i');
                i2.classList.add('fa');
                i2.classList.add('fa-close');
                $(a2).append(i2);
                $(divRadio).append(a2);
                var celdaObs = document.createElement('td');
                var inputObs = document.createElement('input');
                inputObs.classList.add('form-control');
                $(celdaObs).append(inputObs);
                $(row).data().idActividad = data[i].id;
                $(row).append(celdaDesc);
                $(row).append(celdaRealizo);
                $(row).append(celdaObs);
                $('#' + tbl + ' tbody').append(row);
            }
        }
        //AC//

        //Cuadrillas
        function showModalCua() {
            if (selectMes.val() != "")
                //mdlEvidenciaCua.modal("show");
                GuardarRevisionCua();
        }
        function setGaleryCua() {
            let lstFotos = $(this)[0].files;
            $.each(lstFotos, function (i, e) {
                readURLCua(this);
            });
        }
        var readURLCua = function (input) {
            var reader = new FileReader();
            let item = $(document.createElement('div'));
            reader.onload = function (e) {
                item.addClass("mkr_SldItem");
                item.append(document.createElement('div'));
                item.find("div").addClass("thumbHolder");
                item.find(".thumbHolder").append(document.createElement("img"));
                item.find("img").attr("src", e.target.result);
                item.find("img").attr("width", "125px");
            }
            reader.readAsDataURL(input);
            divGaleriaCua.append(item);
        }
        function GuardarRevisionCua() {
            var request = new XMLHttpRequest();
            request.open("POST", "/MAZDA/PlanActividades/GuardarRevisionCuadrilla");
            request.send(formDataCua());
            request.onload = function (response) {
                if (request.status == 200) {
                    AlertaGeneral("Aviso", "Revision guardada correctamente.");
                    mdlEvidenciaCua.modal("hide");

                    $('#mdlRevisionCua').modal('hide');
                    $('#dialogPlanMes').modal('hide');

                    selectMes.val("");
                    selectCuadrilla.change();
                    inputObsCua.val("");

                    selTecnicoCua.val("");
                    $('#selAyudanteCua').fillCombo('/MAZDA/PlanActividades/GetEmpleadoList', { est: true }, false, "Todos");
                    convertToMultiselect('#selAyudanteCua');

                    //Limpiar input tipo file en navegadores viejos
                    //inputEvidenciasCua.wrap('<form>').closest('form').get(0).reset();
                    //inputEvidenciasCua.unwrap();

                    inputEvidenciasCua.val("");
                    divGaleriaCua.empty();
                }
            };
        }
        function formDataCua() {
            let formData = new FormData();
            formData.append("rev", JSON.stringify(getRevisionCua()));
            formData.append("ayu[]", getAyudantesCua());
            formData.append("det[]", getDetallesCua());
            $.each(document.getElementById("inputEvidenciasCua").files, function (i, file) {
                formData.append("files[]", file);
            });
            return formData;
        }
        function getRevisionCua() {
            return {
                cuadrillaID: parseInt($('#selectCuadrilla').val()),
                mes: selectMes.val(),
                tecnico: selTecnicoCua.val() != "" ? selTecnicoCua.val() : 0,
                observaciones: inputObsCua.val(),
                estatus: true,

                planMesDetalleID: $('#planMesDetalleID').val() || 0
            };
        }
        function getAyudantesCua() {
            let ayu = [];

            $(getValoresMultiples('#selAyudanteCua')).each(function (idx, numero) {
                ayu.push(JSON.stringify({
                    id: 0,
                    idPersonal: parseInt(numero),
                    revisionID: 0,
                    estatus: true
                }));
            });

            return "[" + ayu + "]";
        }
        function getDetallesCua() {
            let det = [];
            tblRevisionActividades.find("tbody tr").each(function (idx, row) {
                let est = $(this).find('input').val();
                est = est.length == 0 ? "" : est;
                det.push(JSON.stringify({
                    id: 0,
                    actividadID: $(this).attr('data-actividadID'),
                    realizo: getRadioValueCua('radRealizo' + idx),
                    estadoString: est,
                    revisionID: 0,
                    estatus: true
                }));
            });
            return "[" + det + "]";
        }
        function getRadioValueCua(tog) {
            return $('#divCuadrillaGeneral #tblRevisionActividades a[data-toggle="' + tog + '"]').closest('td').find('.active').data('title');
        }
        //Cuadrillas//

        init();
    };

    $(document).ready(function () {
        planActividades.revisionCuadrilla = new revisionCuadrilla();
    })
    .ajaxStart(function () {
        $.blockUI({
            message: 'Procesando...',
            baseZ: 2000
        });
    })
    .ajaxStop(function () { $.unblockUI(); });
})();