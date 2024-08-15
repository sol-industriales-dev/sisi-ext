(function () {
    $.namespace('planActividades.reporteActividades');
    reporteActividades = function () {

        report = $("#report");
        btnPrint = $('#btnPrint');

        tblReporteActividades = $('#tblReporteActividades');
        dtPckrFechaReporte = $('#dtPckrFechaReporte');

        headerFecha = $('#headerFecha');
        mdlEvidencia = $('#mdlEvidencia');
        divGaleria = $('#divGaleria');

        imagenZoom = $('#imagenZoom');
        mdlImagen = $('#mdlImagen');

        mdlRevisionAC = $('#mdlRevisionAC');
        mdlRevisionCua = $('#mdlRevisionCua');

        mdlReferencias = $('#mdlReferencias');
        divGaleriaReferencias = $('#divGaleriaReferencias');

        inputEvidencias = $('#inputEvidencias');

        mdlEvidenciaNueva = $('#mdlEvidenciaNueva');
        divGaleriaNueva = $('#divGaleriaNueva');
        inputEvidenciaNueva = $('#inputEvidenciaNueva');
        btnGuardarEvidenciaNueva = $('#btnGuardarEvidenciaNueva');

        dtPckrFechaReporte.on('change', function () {
            fnCargar();
        });

        $('#tblReporteActividades').on('click', '.buttonEvidencia', function () {
            mdlEvidencia.modal('show');

            var tipoRevision = $(this).attr('data-tiporevision');
            var revisionID = $(this).attr('data-revisionid');
            var revisionDetalleID = $(this).closest('tr').attr('data-revdetid');

            $.ajax({
                url: '/MAZDA/PlanActividades/GetEvidenciasReporte',
                datatype: "json",
                type: "POST",
                data: { tipoRevision: tipoRevision, revisionID: revisionID, revDetID: revisionDetalleID },
                success: function (response) {
                    var data = response.data;

                    //$('#imagenSrc').attr('src', 'data:image/png;base64, ' + data[0]);
                    setGalery(data);
                }
            });
        });

        $('#tblReporteActividades').on('click', '.buttonReferencias', function () {
            mdlReferencias.modal('show');

            var row = $(this).closest('tr');

            var _tempObj = row.find('select option:selected').map(function (a, item) { return item.value; });
            var equiposID = new Array();
            $.each(_tempObj, function (i, e) {
                equiposID.push(e);
            });

            var equipoID = $(this).attr('data-equipoid');

            $.ajax({
                url: '/MAZDA/PlanActividades/GetReferencias',
                datatype: "json",
                type: "POST",
                data: { equiposID: equiposID },
                success: function (response) {
                    var data = response.data;

                    setGaleryReferencias(data);
                }
            });
        });

        $('#tblReporteActividades').on('click', '.buttonDetalle', function () {
            var tipoRevision = $(this).attr('data-tiporevision');
            var revisionID = $(this).attr('data-revisionid');
            var cuadrillaID = parseInt($(this).closest('tr').attr('data-cuadrillaid'));
            var cuadrilla = $(this).closest('tr').find('td').first().text().trim();

            switch (parseInt(tipoRevision)) {
                case 1:
                    $('#labelTituloRevisionAC').text(" Revisión " + cuadrilla);
                    mdlRevisionAC.modal('show');
                    fillTablasAC(revisionID);
                    break;
                case 2:
                    $('#labelTituloRevisionCua').text(" Revisión " + cuadrilla);
                    mdlRevisionCua.modal('show');
                    getTablaCuadrilla(cuadrillaID, revisionID);
                    break;
            }

            //$.ajax({
            //    url: '/MAZDA/PlanActividades/GetDetalleRevision',
            //    datatype: "json",
            //    type: "POST",
            //    data: { tipoRevision: tipoRevision, revisionID: revisionID },
            //    success: function (response) {
            //        var data = response.data;
            //    }
            //});
        });

        $('#divGaleria').on('click', 'img', function () {
            var imagen = $(this).attr('src');

            imagenZoom.attr('src', imagen);
            mdlImagen.modal('show');
        });

        $('#divGaleriaReferencias').on('click', 'img', function () {
            var imagen = $(this).attr('src');

            imagenZoom.attr('src', imagen);
            mdlImagen.modal('show');
        });

        $('#tblReporteActividades').on('click', '.buttonGuardar', function () {
            var row = $(this).closest('tr');
            var revDetID = parseInt(row.attr('data-revdetid'));
            var cuadrillaID = parseInt(row.attr('data-cuadrillaid'));

            debugger;
            var obj = {
                revisionTipo: cuadrillaID != 1 ? 2 : 1,
                revisionDetalleID: revDetID,
                ultMant: row.find('.ultMant').val(),
                sigMant: row.find('.sigMant').val(),
                descripcionActividad: row.find('textarea').val(),
                semaforo: row.find('.circuloActual').attr('circulotipo'),
                reprogramacion: row.find('.reprogramacion').val(),
                estatusInfo: row.find('.estatusInfo').val(),
                fechaReprogramacion: row.find('.fechaReprogramacion').val(),
                estatus: true
            };

            var equipos = getValoresMultiples('#' + row.find('select').attr('id'));
            let eq = [];
            for (i = 0; i < equipos.length; i++) {
                eq.push(JSON.stringify({
                    reporteActividadesID: 0,
                    equipoID: equipos[i],
                    estatus: true
                }));
            }

            let formData = new FormData();
            formData.append("info", JSON.stringify(obj));
            formData.append("eq[]", ("[" + eq + "]"));
            $.each(document.getElementById("inputEvidenciaNueva").files, function (i, file) {
                formData.append("files[]", file);
            });

            var request = new XMLHttpRequest();
            request.open("POST", "/MAZDA/PlanActividades/GuardarInfoRevDet");
            request.send(formData);
            request.onload = function (response) {
                if (request.status == 200) {
                    AlertaGeneral("Aviso", "Reporte guardado correctamente.");

                    iniciarTabla();

                    var inputs = $('#tblReporteActividades tbody input');

                    $.each(inputs, function (index, element) {
                        if (element.offsetWidth < element.scrollWidth) {
                            fnAgregarTooltip($(element), $(element).val());
                        } else {
                            fnQuitarTooltip($(element));
                        }
                    });
                }
            };

            //$.ajax({
            //    url: '/MAZDA/PlanActividades/GuardarInfoRevDet',
            //    datatype: "json",
            //    type: "POST",
            //    data: { cuadrillaID: cuadrillaID, revDetID: revDetID, equipos: equipos, ultMant: ultMant, sigMant: sigMant, descripcionAct: descripcionAct, semaforo: semaforo, reprog: reprog, estatus: estatus },
            //    success: function (response) {
            //        var inputs = $('#tblReporteActividades tbody input');

            //        $.each(inputs, function (index, element) {
            //            if (element.offsetWidth < element.scrollWidth) {
            //                fnAgregarTooltip($(element), $(element).val());
            //            } else {
            //                fnQuitarTooltip($(element));
            //            }
            //        });
            //    }
            //});
        });

        $('#tblReporteActividades').on('click', '.circSelect', function () {
            var seleccionado = $(this);
            const fecha = $(seleccionado).closest('td').parent().find('.fechaReprogramacion');

            var tipo = parseInt(seleccionado.attr('circulotipo'));
            var circulo = seleccionado.closest('.circuloActual');
            let boolFecha = false;

            let html = '';
            switch (tipo) {
                case 1:
                    circulo.attr('circulotipo', 1);
                    circulo.empty();


                    html = '<a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false"></a>\
                            <ul class="dropdown-menu listaSemaforo">\
                                <ul class="list-inline">\
                                    <li><div class="circulo circSelect" circulotipo="2"></div></li>\
                                    <li><div class="circulo circSelect" circulotipo="3"></div></li>\
                                </ul>\
                            </ul>';
                    boolFecha = true;
                    break;
                case 2:
                    circulo.attr('circulotipo', 2);
                    circulo.empty();

                    html = '<a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false"></a>\
                            <ul class="dropdown-menu listaSemaforo">\
                                <ul class="list-inline">\
                                    <li><div class="circulo circSelect" circulotipo="1"></div></li>\
                                    <li><div class="circulo circSelect" circulotipo="3"></div></li>\
                                </ul>\
                            </ul>';
                    boolFecha = false;
                    break;
                default:
                    circulo.attr('circulotipo', 3);
                    circulo.empty();

                    html = '<a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false"></a>\
                            <ul class="dropdown-menu listaSemaforo">\
                                <ul class="list-inline">\
                                    <li><div class="circulo circSelect" circulotipo="1"></div></li>\
                                    <li><div class="circulo circSelect" circulotipo="2"></div></li>\
                                </ul>\
                            </ul>';
                    boolFecha = true;
                    break;
            }

            circulo.append(html);
            circulo.change();
            $(fecha).val('');
            $(fecha).prop("disabled", boolFecha);

        });

        $('#tblReporteActividades').on('click', '.buttonEvidenciaNueva', function () {
            mdlEvidenciaNueva.modal('show');
        });

        function init() {
            initCbo();

            var hoy = new Date();
            dtPckrFechaReporte.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);

            iniciarTabla();

            inputEvidenciaNueva.change(setGaleryNuevo);
            btnGuardarEvidenciaNueva.click(function () {
                mdlEvidenciaNueva.modal('hide');
            });
            btnPrint.click(verReporte);
        }

        function verReporte() {
            report.attr("src", '/Reportes/Vista.aspx?idReporte=93&fecha=' + dtPckrFechaReporte.val());

            document.getElementById('report').onload = function () {
                openCRModal();
            };
        }

        function initCbo() {

        }

        function fnCargar() {
            iniciarTabla();
        }

        function iniciarTabla() {
            $.ajax({
                url: '/MAZDA/PlanActividades/GetReporteDiario',
                datatype: "json",
                type: "POST",
                data: { fecha: dtPckrFechaReporte.val() },
                success: function (response) {
                    var data = response.data;

                    headerFecha.val(response.fechaConDia != null && response.fechaConDia != undefined ? response.fechaConDia : "");
                    headerFecha.text(response.fechaConDia != null && response.fechaConDia != undefined ? response.fechaConDia : "");

                    fillTabla(data);
                }
            });
        }

        function fillTabla(data) {
            $("#tblReporteActividades tbody tr").remove();
            if (data != void 0) {
                btnPrint.css('display', 'inline-block');
                for (i = 0; i < data.length; i++) {
                    var tr = document.createElement('tr');
                    tr.setAttribute('data-cuadrillaid', data[i].cuadrillaID);
                    tr.setAttribute('data-revDetID', data[i].revisionDetID);

                    var td1 = document.createElement('td');
                    td1.textContent = data[i].cuadrilla;
                    td1.classList.add('dt-center');
                    var td2 = document.createElement('td');
                    td2.textContent = data[i].actividad;
                    td2.classList.add('dt-center');

                    var td3 = document.createElement('td');
                    //td3.textContent = data[i].equipo;
                    td3.classList.add('dt-center');
                    $(td3).append(document.createElement('select'));
                    $(td3).find('select').attr('id', 'multiSelectEquipo' + i);
                    $(td3).find('select').attr('multiple', 'multiple');
                    $(td3).find('select').addClass('form-control');
                    $(td3).find('select').fillCombo('/MAZDA/PlanActividades/GetEquiposList', { arrAreas: data[i].areaID }, false, "Todos");

                    debugger;
                    if ($(td3).find('select option').length <= 1) {
                        $(td3).find('select').remove();
                    } else {
                        if (data[i].revisionTipo == 1 && data[i].equiposID == null) {
                            $(td3).find('select').val(data[i].equiposID);
                        } else {
                            if (data[i].equipoID != 0 && data[i].equiposID == null) {
                                $(td3).find('select').val(data[i].equipoID);
                            }
                        }
                    }

                    var td4 = document.createElement('td');
                    //td4.textContent = data[i].ultMantenimiento;
                    td4.classList.add('dt-center');
                    $(td4).append(document.createElement('input'));
                    $(td4).find('input').addClass('ultMant');
                    $(td4).find('input').addClass('form-control');
                    $(td4).find('input').val(data[i].ultMantenimiento)

                    var td5 = document.createElement('td');
                    //td5.textContent = data[i].sigMantenimiento;
                    td5.classList.add('dt-center');
                    $(td5).append(document.createElement('input'));
                    $(td5).find('input').addClass('sigMant');
                    $(td5).find('input').addClass('form-control');
                    $(td5).find('input').val(data[i].sigMantenimiento);

                    var td6 = document.createElement('td');
                    td6.textContent = data[i].areaEjecucion;
                    td6.classList.add('dt-center');

                    var td7 = document.createElement('td');
                    td7.textContent = data[i].descripcionActividad;

                    $(td7).append($(document.createElement('textarea')).addClass('form-control'));

                    var td8 = document.createElement('td');

                    var divCirculo = document.createElement('div');
                    divCirculo.classList.add('circulo');
                    divCirculo.classList.add('circuloActual')
                    divCirculo.classList.add('dropdown');

                    var html = '';
                    let boolFecha = false;

                    switch (data[i].semaforo) {
                        case 1:
                            divCirculo.setAttribute('circulotipo', 1);

                            html = '<a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false"></a>\
                                    <ul class="dropdown-menu listaSemaforo">\
                                        <ul class="list-inline">\
                                            <li><div class="circulo circSelect" circulotipo="2"></div></li>\
                                            <li><div class="circulo circSelect" circulotipo="3"></div></li>\
                                        </ul>\
                                    </ul>';
                            boolFecha = true;
                            break;
                        case 2:
                            divCirculo.setAttribute('circulotipo', 2);

                            html = '<a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false"></a>\
                                    <ul class="dropdown-menu listaSemaforo">\
                                        <ul class="list-inline">\
                                            <li><div class="circulo circSelect" circulotipo="1"></div></li>\
                                            <li><div class="circulo circSelect" circulotipo="3"></div></li>\
                                        </ul>\
                                    </ul>';
                            boolFecha = false;
                            break;
                        default:
                            divCirculo.setAttribute('circulotipo', 3);

                            html = '<a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false"></a>\
                                    <ul class="dropdown-menu listaSemaforo">\
                                        <ul class="list-inline">\
                                            <li><div class="circulo circSelect" circulotipo="1"></div></li>\
                                            <li><div class="circulo circSelect" circulotipo="2"></div></li>\
                                        </ul>\
                                    </ul>';
                            boolFecha = true;
                            break;
                    }

                    $(divCirculo).append(html);

                    $(td8).append(divCirculo);

                    td8.classList.add('dt-center');
                    var td9 = document.createElement('td');
                    //td9.textContent = data[i].reprogramacion;
                    td9.classList.add('dt-center');
                    $(td9).append(document.createElement('input'));
                    $(td9).find('input').addClass('reprogramacion');
                    $(td9).find('input').addClass('form-control');
                    $(td9).find('input').val(data[i].reprogramacion);

                    var td11 = document.createElement('td');
                    var d = new Date(data[i].fechaReprogramacion);
                    td11.classList.add('dt-center');
                    $(td11).append(document.createElement('input'));
                    $(td11).find('input').addClass('fechaReprogramacion');
                    $(td11).find('input').addClass('form-control');
                    $(td11).find('input').datepicker({ dateFormat: 'dd/mm/yy' })
                    if (!boolFecha) $(td11).find('input').datepicker("setDate", d);
                    $(td11).find('input').prop("disabled", boolFecha);
                    td11.id = 'dtpickerFechaProgramacion';

                    var td10 = document.createElement('td');
                    //td10.textContent = data[i].estatus;
                    td10.classList.add('dt-center');
                    $(td10).append(document.createElement('input'));
                    $(td10).find('input').addClass('estatusInfo');
                    $(td10).find('input').addClass('form-control');
                    $(td10).find('input').val(data[i].estatus);

                    var tdBotones = document.createElement('td');
                    tdBotones.classList.add('dt-center');
                    tdBotones.style.whiteSpace = 'nowrap';

                    var buttonDetalle = document.createElement('button');
                    //buttonDetalle.style.marginLeft = '5px';
                    buttonDetalle.classList.add('btn');
                    buttonDetalle.classList.add('btn-info');
                    buttonDetalle.classList.add('btn-xs');
                    buttonDetalle.classList.add('buttonDetalle');
                    buttonDetalle.setAttribute('data-tiporevision', data[i].revisionTipo);
                    buttonDetalle.setAttribute('data-revisionID', data[i].revisionID);
                    var spanButtonDetalle = document.createElement('span');
                    spanButtonDetalle.classList.add('glyphicon');
                    spanButtonDetalle.classList.add('glyphicon-list-alt');
                    $(buttonDetalle).append(spanButtonDetalle);

                    var buttonEvidencias = document.createElement('button');
                    buttonEvidencias.style.marginLeft = '5px';
                    buttonEvidencias.classList.add('btn');
                    buttonEvidencias.classList.add('btn-primary');
                    buttonEvidencias.classList.add('btn-xs');
                    buttonEvidencias.classList.add('buttonEvidencia');
                    buttonEvidencias.setAttribute('data-tiporevision', data[i].revisionTipo);
                    buttonEvidencias.setAttribute('data-revisionID', data[i].revisionID);
                    var spanButtonEvidencias = document.createElement('span');
                    spanButtonEvidencias.classList.add('glyphicon');
                    spanButtonEvidencias.classList.add('glyphicon-eye-open');
                    $(buttonEvidencias).append(spanButtonEvidencias);

                    var buttonEvidenciaNueva = document.createElement('button');
                    buttonEvidenciaNueva.style.marginLeft = '5px';
                    buttonEvidenciaNueva.classList.add('btn');
                    buttonEvidenciaNueva.classList.add('btn-success');
                    buttonEvidenciaNueva.classList.add('btn-xs');
                    buttonEvidenciaNueva.classList.add('buttonEvidenciaNueva');
                    buttonEvidenciaNueva.setAttribute('data-tiporevision', data[i].revisionTipo);
                    buttonEvidenciaNueva.setAttribute('data-revisionID', data[i].revisionID);
                    var spanButtonEvidenciaNueva = document.createElement('span');
                    spanButtonEvidenciaNueva.classList.add('glyphicon');
                    spanButtonEvidenciaNueva.classList.add('glyphicon-plus');
                    $(buttonEvidenciaNueva).append(spanButtonEvidenciaNueva);

                    var buttonReferencias = document.createElement('button');
                    buttonReferencias.style.marginLeft = '5px';
                    buttonReferencias.classList.add('btn');
                    buttonReferencias.classList.add('btn-warning');
                    buttonReferencias.classList.add('btn-xs');
                    buttonReferencias.classList.add('buttonReferencias');
                    buttonReferencias.setAttribute('data-equipoid', data[i].equipoID);
                    buttonReferencias.setAttribute('data-revisionID', data[i].revisionID);
                    var spanButtonReferencias = document.createElement('span');
                    spanButtonReferencias.classList.add('glyphicon');
                    spanButtonReferencias.classList.add('glyphicon-eye-open');
                    $(buttonReferencias).append(spanButtonReferencias);

                    var buttonGuardar = document.createElement('button');
                    buttonGuardar.style.marginLeft = '5px';
                    buttonGuardar.classList.add('btn');
                    buttonGuardar.classList.add('btn-success');
                    buttonGuardar.classList.add('btn-xs');
                    buttonGuardar.classList.add('buttonGuardar');
                    buttonGuardar.setAttribute('data-tiporevision', data[i].revisionTipo);
                    buttonGuardar.setAttribute('data-revisionID', data[i].revisionID);
                    var spanButtonGuardar = document.createElement('span');
                    spanButtonGuardar.classList.add('glyphicon');
                    spanButtonGuardar.classList.add('glyphicon-floppy-save');
                    $(buttonGuardar).append(spanButtonGuardar);

                    var divBotones = document.createElement('div');

                    $(tdBotones).append(divBotones);

                    $(divBotones).append(buttonDetalle);
                    $(divBotones).append(buttonEvidencias);
                    $(divBotones).append(buttonEvidenciaNueva);
                    $(divBotones).append(buttonReferencias);
                    $(divBotones).append(buttonGuardar);

                    buttonDetalle.id = 'buttonDetalle_' + i;
                    buttonEvidencias.id = 'buttonEvidencia_' + i;
                    buttonEvidenciaNueva.id = 'buttonEvidenciaNueva_' + i;
                    buttonReferencias.id = 'buttonReferencias_' + i;
                    buttonGuardar.id = 'buttonGuardar_' + i;

                    $(tr).append(tdBotones);
                    $(tr).append(td1);
                    $(tr).append(td2);
                    $(tr).append(td3);
                    $(tr).append(td4);
                    $(tr).append(td5);
                    $(tr).append(td6);
                    $(tr).append(td7);
                    $(tr).append(td8);
                    $(tr).append(td9);
                    $(tr).append(td11);
                    $(tr).append(td10);

                    $('#tblReporteActividades tbody').append(tr);

                    if (data[i].equiposID != null) {
                        seleccionarMultiple(document.getElementById('multiSelectEquipo' + i), data[i].equiposID);
                    }

                    var idString = '#multiSelectEquipo' + i;
                    convertToMultiselect(idString);

                    if ($(td3).find('select option').length > 1 && data[i].revisionTipo != 1 && data[i].equipoID == 0 && data[i].equiposID == null) {
                        $(td3).find('select').val("");
                        $(td3).find('select option:selected').prop("selected", false);

                        $(idString).multiselect('selectAll', false);
                        $(idString).multiselect('deselect', "Todos");
                        $(idString).multiselect('updateButtonText');
                    }

                    fnAgregarTooltip($(document.getElementById(buttonDetalle.id)), 'Ver Revisión');
                    fnAgregarTooltip($(document.getElementById(buttonEvidencias.id)), 'Ver Evidencias');
                    fnAgregarTooltip($(document.getElementById(buttonEvidenciaNueva.id)), 'Agregar Evidencias');
                    fnAgregarTooltip($(document.getElementById(buttonReferencias.id)), 'Ver Referencias');
                    fnAgregarTooltip($(document.getElementById(buttonGuardar.id)), 'Guardar Información');
                }

                //$('#tblReporteActividades input').first().change();
            } else {
                btnPrint.css('display', 'none');

                var tr = document.createElement('tr');

                var tdVacio = document.createElement('td');
                tdVacio.textContent = "No hay capturas este día.";
                tdVacio.classList.add('tdVacio');
                tdVacio.setAttribute('colspan', '14');

                $(tr).append(tdVacio);
                $('#tblReporteActividades tbody').append(tr);
            }
        }

        function seleccionarMultiple(selectObj, valores) {
            for (var i = 0; i < selectObj.length; i++) {
                if (valores.includes(parseInt(selectObj.options[i].value))) {
                    selectObj.options[i].selected = true;
                } else {
                    selectObj.options[i].selected = false;
                }
            }
        }

        function setGalery(archivos) {
            divGaleria.empty();

            let lstFotos = archivos;
            $.each(lstFotos, function (i, e) {
                readURL(this);
            });
        }

        function setGaleryNuevo() {
            let lstFotos = $(this)[0].files;
            $.each(lstFotos, function (i, e) {
                readURLNuevo(this);
            });
        }

        function setGaleryReferencias(archivos) {
            divGaleriaReferencias.empty();

            let lstFotos = archivos;
            $.each(lstFotos, function (i, e) {
                readURLReferencias(this);
            });
        }

        var readURL = function (base64) {
            let item = $(document.createElement('div'));
            item.addClass("mkr_SldItem");
            item.append(document.createElement('div'));
            item.find("div").addClass("thumbHolder");
            item.find(".thumbHolder").append(document.createElement("img"));
            item.find("img").attr("src", base64);
            item.find("img").attr("width", "125px");

            divGaleria.append(item);
        }
        var readURLNuevo = function (input) {
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
            divGaleriaNueva.append(item);
        }

        var readURLReferencias = function (base64) {
            let item = $(document.createElement('div'));
            item.addClass("mkr_SldItem");
            item.append(document.createElement('div'));
            item.find("div").addClass("thumbHolder");
            item.find(".thumbHolder").append(document.createElement("img"));
            item.find("img").attr("src", base64);
            item.find("img").attr("width", "125px");

            divGaleriaReferencias.append(item);
        }

        function getTablaCuadrilla(cuadrillaID, revisionID) {
            var arr = new Array();
            arr.push(cuadrillaID);

            $.ajax({
                url: '/MAZDA/PlanActividades/GetRevisionCuadrilla',
                datatype: "json",
                type: "POST",
                data: { arrCuadrillas: arr, revisionID: revisionID },
                success: function (response) {
                    var data = response.data;
                    var revision = response.revision;

                    llenarInfoCua(revision);
                    fillTablaCuadrilla(data, revision.detalle);
                }
            });
        }
        function fillTablaCuadrilla(data, revDetalle) {
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
                var actividadRev = revDetalle.find(function (act) {
                    return act.actividadID == data[i].id;
                });

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

                if (actividadRev.realizo == true) {
                    html += '<td class="text-center">';
                    html += '   <div class="radioBtn btn-group">';
                    html += '       <a class="btn btn-success active" data-toggle="radRealizo' + i + '" data-title="true">';
                    html += '           <i class="fa fa-check"></i>';
                    html += '       </a>';
                    html += '       <a class="btn btn-danger notActive" data-toggle="radRealizo' + i + '" data-title="false">';
                    html += '           <i class="fa fa-close"></i>';
                    html += '       </a>';
                    html += '   </div>';
                    html += '</td>';
                } else {
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
                }

                html += '<td>';
                html += '   <input class="form-control" value="' + actividadRev.estadoString + '" readonly />';
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
        }
        function llenarInfoCua(revision) {
            $('#selectMes').val(revision.mesDesc);
            $('#selTecnicoCua').val(revision.tecnico);
            $('#selAyudanteCua').val(revision.ayudantes);

            var selAyudanteCua = document.getElementById('selAyudanteCua');
            var $selAyudanteCua = $(selAyudanteCua);
            if (selAyudanteCua.offsetWidth < selAyudanteCua.scrollWidth) {
                fnAgregarTooltip($selAyudanteCua, $selAyudanteCua.val())
            } else {
                fnQuitarTooltip($selAyudanteCua);
            }

            $('#inputObsCua').val(revision.observaciones);
        }

        function fillTablasAC(revisionID) {
            GetAllActividadesAC(revisionID).done(function (response) {
                if (response.success) {
                    llenarInfoAC(response.revision);

                    setRows("tblCondensador", response.lstCondensador, response.revision.detalle);
                    setRows("tblEvaporador", response.lstEvaporador, response.revision.detalle);
                }
            });
        }
        function GetAllActividadesAC(revisionID) {
            return $.post("/MAZDA/PlanActividades/GetAllActividadesACRevision/" + "?revisionID=" + revisionID);
        }
        function setRows(tbl, data, revDetalle) {
            $('#' + tbl + ' tbody tr').remove();

            for (i = 0; i < data.length; i++) {
                var actividad = revDetalle.find(function (act) {
                    return act.actividadID == data[i].id;
                });

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

                if (actividad.realizo == true) {
                    a1.classList.add('active');
                } else {
                    a1.classList.add('notActive');
                }

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

                if (actividad.realizo == true) {
                    a2.classList.add('notActive');
                } else {
                    a2.classList.add('active');
                }

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
                inputObs.readOnly = true;
                $(inputObs).val(actividad.observaciones);

                $(celdaObs).append(inputObs);
                $(row).data().idActividad = data[i].id;
                $(row).append(celdaDesc);
                $(row).append(celdaRealizo);
                $(row).append(celdaObs);
                $('#' + tbl + ' tbody').append(row);
            }
        }
        function llenarInfoAC(revision) {
            $('#selectEquipo').val(revision.equipo);
            $('#inputTonelaje').val(revision.tonelaje);
            $('#selectArea').val(revision.area);
            $('#selectPeriodo').val(revision.periodo);
            $('#selTecnico').val(revision.tecnico);
            $('#selAyudante').val(revision.ayudantes);
            //$('#selAyudante').attr('title', revision.ayudantes);

            var selAyudante = document.getElementById('selAyudante');
            var $selAyudante = $(selAyudante);
            if (selAyudante.offsetWidth < selAyudante.scrollWidth) {
                fnAgregarTooltip($selAyudante, $selAyudante.val())
            } else {
                fnQuitarTooltip($selAyudante);
            }

            $('#inputObs').val(revision.observaciones);
        }

        function fnAgregarTooltip(elemento, mensaje) {
            $(elemento).attr('data-toggle', 'tooltip');
            $(elemento).attr('data-placement', 'top');
            //Agregar text-overflow ellipsis
            $(elemento).addClass('long-value-input');

            if (mensaje != "") {
                $(elemento).attr('title', mensaje);
            }

            $('[data-toggle="tooltip"]').tooltip({
                position: {
                    my: "center bottom-20",
                    at: "center top",
                    using: function (position, feedback) {
                        $(this).css(position);
                        $("<div>")
                            .addClass("arrow")
                            .addClass(feedback.vertical)
                            .addClass(feedback.horizontal)
                            .appendTo(this);
                    }
                }
            });
        }
        function fnQuitarTooltip(elemento) {
            $(elemento).removeAttr('data-toggle');
            $(elemento).removeAttr('data-placement');
            $(elemento).removeAttr('title');
            //Quitar text-overflow ellipsis
            $(elemento).removeClass('long-value-input');
        }

        init();
    };

    $(document).ready(function () {
        planActividades.reporteActividades = new reporteActividades();
    })
        .ajaxStart(function () {
            $.blockUI({
                message: 'Procesando...',
                baseZ: 2000
            });
        })
        .ajaxStop(function () { $.unblockUI(); });
})();