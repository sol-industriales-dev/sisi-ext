$(function () {
    $.namespace('Kubrix.BaseDatos.CapturaMaq');
    CapturaMaq = function () {
        _Eliminar = 0;
        mensajes = {
            PROCESANDO: 'Procesando...'
        };

        tblDataMaqKubrix = $("#tblDataMaqKubrix");
        fechaIni = $("#fechaIni");
        fechaFin = $("#fechaFin");
        txtCCFiltro = $("#txtCCFiltro");
        txtNombreCC = $("#txtNombreCC");
        btnAplicarFiltros = $("#btnAplicarFiltros");

        btnCapturarMaq = $("#btnCapturarMaq");

        $("#tblDataMaqKubrix").on({
            change: function () {
                var horasProg = $(this);
                var row = horasProg.closest('tr');
                var eficiencia = row.find('.eficiencia');
                var horasTrab = parseInt(row.find('.horasTrab').text().trim());

                if (!isNaN(horasProg.val())) {
                    var efiCalculada = ((horasProg.val() / horasTrab) * 100).toFixed(0);

                    eficiencia.text(efiCalculada + "%");
                }
            }
        }, ".horasProg");

        $("#tblDataMaqKubrix").on({
            change: function () {
                var rendTeo = $(this);
                var row = rendTeo.closest('tr');
                var rendimiento = row.find('.rendimiento');
                var rendReal = parseFloat(row.find('.rendReal').text().trim());

                if (!isNaN(rendTeo.val()) && rendTeo.val() != "" && rendTeo.val() != 0) {
                    var rendCalculado = ((rendReal / rendTeo.val()) * 100).toFixed(1);

                    rendimiento.text(rendCalculado + "%");
                }
            }
        }, ".rendTeorico");

        function init() {
            //llenarTabla();

            datePicker();

            var now = new Date();
            year = now.getYear() + 1900;
            fechaIni.datepicker().datepicker("setDate", now);
            fechaFin.datepicker().datepicker("setDate", "");

            txtCCFiltro.change(getInfoEconomico);
            btnAplicarFiltros.click(llenarTabla);

            btnCapturarMaq.click(fnCapturarMaq);
        }

        function fnCapturarMaq() {
            var arr = new Array();

            $("#tblDataMaqKubrix tbody tr").each(function () {
                var row = $(this);
                var obj = {};
                
                if (row.find('.paroClima').val() != "" && row.find('.hrsMtto').val() != "" && row.find('.horasProg').val() != "" && row.find('.rendTeorico').val() != "") {
                    obj = {
                        ccObra: row.find('.ccObra').text().trim(),
                        fecha: row.find('.fecha').text().trim(),
                        economico: row.find('.economico').text().trim(),
                        turno: row.find('.turno').text().trim(),
                        horoInicial: row.find('.horoInicial').text().trim(),
                        horoFinal: row.find('.horoFinal').text().trim(),
                        paroClima: row.find('.paroClima').val(),
                        hrsMtto: row.find('.hrsMtto').val(),
                        horasTrab: row.find('.horasTrab').text().trim(),
                        horasProg: row.find('.horasProg').val(),
                        horasEfectivas: row.find('.horasEfectivas').text().trim(),
                        eficiencia: unmaskNumero(row.find('.eficiencia').text().trim()),
                        consumo: row.find('.consumo').text().trim(),
                        grupoEquipo: row.find('.grupoEquipo').text().trim(),
                        rendTeorico: row.find('.rendTeorico').val(),
                        rendReal: row.find('.rendReal').text().trim(),
                        rendimiento: unmaskNumero(row.find('.rendimiento').text().trim())
                    }

                    arr.push(obj);
                }
            });

            if (arr.length > 0) {
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: '/Kubrix/BaseDatos/CapturarMaq',
                    data: { arr: arr },
                    success: function (response) {
                        llenarTabla();
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
        }

        function getInfoEconomico() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Horometros/getCentroCostos",
                data: { obj: txtCCFiltro.val() == "" ? 0 : txtCCFiltro.val() },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        var nombreCC = response.centroCostos;

                        if (nombreCC == "") {
                            ConfirmacionGeneral("", "No se Encontro ese centro de costos", "bg-red");
                        }
                        txtNombreCC.val(nombreCC);

                    }
                    else {

                        txtNombreCC.val('');
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function llenarTabla() {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/Kubrix/BaseDatos/lstCapturaMaq',
                data: { cc: txtCCFiltro.val(), fechaInicio: fechaIni.val(), fechaFin: fechaFin.val() },
                success: function (response) {
                    addRow(response.data);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function addRow(obj) {
            $("#tblDataMaqKubrix tbody tr").remove();

            for (i = 0; i < obj.length; i++) {
                for (j = 0; j < obj[i].length; j++) {
                    var html = "";

                    html += '<tr>';
                    html += '   <td>';
                    
                    if (obj[i][j].sem == true) {
                        html += '   ACT';
                    } else {
                        html += '   ANT';
                    }

                    html += '   </td>';
                    html += '   <td class="ccObra">';
                    html += '       ' + obj[i][j].cc;
                    html += '   </td>';
                    html += '   <td class="fecha">';
                    html += '       ' + obj[i][j].fecha;
                    html += '   </td>';
                    html += '   <td class="economico">';
                    html += '       ' + obj[i][j].economico;
                    html += '   </td>';
                    html += '   <td class="turno">';
                    html += '       ' + obj[i][j].turno;
                    html += '   </td>';
                    html += '   <td class="horoInicial">';
                    html += '       ' + obj[i][j].HoroInicial;
                    html += '   </td>';
                    html += '   <td class="horoFinal">';
                    html += '       ' + obj[i][j].HoroFinal;
                    html += '   </td>';
                    html += '   <td>';
                    html += '       <input type="text" class="form-control paroClima" value="0" />';
                    html += '   </td>';
                    html += '   <td>';
                    html += '       <input type="text" class="form-control hrsMtto" value="0" />';
                    html += '   </td>';
                    html += '   <td class="horasTrab">';
                    html += '       ' + obj[i][j].HrsTrab;
                    html += '   </td>';
                    html += '   <td>';
                    html += '       <input type="text" class="form-control horasProg" />';
                    //html += '       ' + obj[i][j].HrsProg;
                    html += '   </td>';
                    html += '   <td class="horasEfectivas">';
                    html += '       ' + obj[i][j].HrsEfect;
                    html += '   </td>';
                    html += '   <td class="eficiencia">';
                    html += '       ' + obj[i][j].Efectivas;
                    html += '   </td>';
                    html += '   <td class="consumo">';
                    html += '       ' + obj[i][j].consumo;
                    html += '   </td>';
                    html += '   <td class="grupoEquipo">';
                    html += '       ' + obj[i][j].gpo;
                    html += '   </td>';
                    html += '   <td>';
                    html += '       <input type="text" class="form-control rendTeorico" />';
                    //html += '       ' + obj[i][j].RendTeorico;
                    html += '   </td>';
                    html += '   <td class="rendReal">';
                    html += '       ' + obj[i][j].RendReal;
                    html += '   </td>';
                    html += '   <td class="rendimiento">';
                    html += '       ' + obj[i][j].Rendimiento;
                    html += '   </td>';
                    html += '</tr>';

                    //html += '<tr>';
                    //html += '   <td>';
                    //html += '       <div class="col-lg-6">';
                    //html += '           <select class="form-control comboConcepto" class="form-control" style="box-shadow: none;"></select>';
                    //html += '       </div>';
                    //html += '       <div class="col-lg-6" style="padding-left: 0px;">';
                    //html += '           <input type="text" class="form-control text-left conceptoInfo" id="" style="box-shadow: none;" />';
                    //html += '       </div>';
                    //html += '   </td>';
                    //html += '   <td>';
                    //html += '       <div class="col-lg-12">';
                    //html += '           <input type="text" class="form-control text-right conceptoDetalle" id="" style="box-shadow: none;" />';
                    //html += '       </div>';
                    //html += '   </td>';
                    //html += '   <td>';
                    //html += '       <div class="col-lg-12">';
                    //html += '           <input type="text" class="form-control text-right resultado dinero" value="$0.00" style="box-shadow: none; border-radius: 0px;" />';
                    //html += '       </div>';
                    //html += '   </td>';
                    //html += '</tr>';

                    $(html).appendTo($("#tblDataMaqKubrix tbody"));
                }
            }
        }

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
                    array = date.split('/');
                    //$(this).datepicker('setDate', new Date(array[2], array[1] - 1, 1));
                    //fechaFin.datepicker('setDate', new Date(array[2], array[1], 0));
                    $(this).blur();

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

        init();
    }

    $(document).ready(function () {
        Kubrix.BaseDatos.CapturaMaq = new CapturaMaq();
    });
});