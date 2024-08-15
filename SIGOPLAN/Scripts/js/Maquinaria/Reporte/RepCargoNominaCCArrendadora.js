(function () {
    $.namespace('maquinaria.reporte.repCargoNomCCArre');
    repCargoNomCCArre = function () {
        inputPeriodoInicial = $('#inputPeriodoInicial');
        inputPeriodoFinal = $('#inputPeriodoFinal');
        btnAplicarFiltros = $('#btnAplicarFiltros');
        ireport = $("#report");
        $('#inputNominaSemanal').on('change', function () {
            $(this).val(maskNumero(unmaskNumero($(this).val())));
        });
        $('#selectPeriodo').on('change', function () {
            if ($(this).val() != "") {
                var arr = $(this).find('option:selected').text().split(" - ");
                $('#inputPeriodoInicial').val(arr[0]);
                $('#inputPeriodoFinal').val(arr[1]);
            } else {
                $('#inputPeriodoInicial').val("");
                $('#inputPeriodoFinal').val("");
            }
        });
        $('#multiSelectProyecto').on('change', function () {
            var arrProyectos = getValoresMultiples('#multiSelectProyecto');
            if (arrProyectos.length > 0) {
                $.ajax({
                    type: "POST",
                    url: '/RepCargoNominaCCArrendadora/GetCCNomina',
                    dataType: 'json',
                    data: { arrProyectos: arrProyectos },
                    success: function (response) {
                        var data = response.data;
                        $('#inputAreaCuentaNomina').val(data);
                    },
                    error: function (error) {
                    },
                    always: function (response) {
                        $.unblockUI();
                    },
                });
            } else {
                $('#inputAreaCuentaNomina').val("");
            }
        });

        function checkPeriodoCapturado(proyecto, periodoInicial, periodoFinal) {
            return $.post("/RepCargoNominaCCArrendadora/CheckPeriodoCapturado", { proyecto, periodoInicial, periodoFinal });
        }

        function init() {
            initCbo();
            var hoy = new Date();
            var inicioMes = new Date(hoy.getFullYear(), hoy.getMonth(), 1);
            inputPeriodoInicial.datepicker({ dateFormat: 'dd/mm/yy' });
            inputPeriodoFinal.datepicker({ dateFormat: 'dd/mm/yy' });
            btnAplicarFiltros.click(fnBuscar);
            $('#btnImprimir').click(GuardarReporte);
            $('#btnGuardar').click(GuardarReporte);
        }
        function fnBuscar() {
            checkPeriodoCapturado($("#multiSelectProyecto").val(), $('#inputPeriodoInicial').val(), $('#inputPeriodoFinal').val()).done(function (response) {
                if (response.success) {
                    if (!response.capturado) {
                        var arrProyectos = getValoresMultiples('#multiSelectProyecto');
                        var nominaSemanal = (unmaskNumero($('#inputNominaSemanal').val())).toString();
                        $.ajax({
                            type: "POST",
                            url: '/RepCargoNominaCCArrendadora/GetEconomicos',
                            data: {
                                Proyectos: $("#multiSelectProyecto").val(),
                                periodoInicial: $('#inputPeriodoInicial').val(),
                                periodoFinal: $('#inputPeriodoFinal').val(),
                                nominaSemanal: nominaSemanal
                            },
                            dataType: 'json',
                            success: function (response) {
                                limpiarTabla();
                                if (response.data !== undefined && response.data.length > 0) {
                                    if (response.isPeriodoActivo) {
                                        AlertaGeneral("Aviso", "Hay cargo en el periodo seleccionado.");
                                        $('#btnImprimir').css('display', 'none');
                                        $('#btnGuardar').css('display', 'none');
                                        //$('#multiSelectProyecto').val(response.arrProyectos);
                                        // $("#multiSelectProyecto").multiselect("refresh");
                                        $('#multiSelectProyecto').change();
                                    }
                                    else {
                                        $('#btnImprimir').css('display', 'inline-block');
                                        $('#btnGuardar').css('display', 'inline-block');
                                    }
                                    fillTabla(response.data, response.sumaHHPeriodo);
                                }
                                else {
                                    AlertaGeneral("Aviso", "No hay Maquinas disponibles.");
                                }
                            },
                            error: function (error) {
                                AlertaGeneral("Alerta", error);
                            }
                        });
                    } else {
                        AlertaGeneral('Alerta', 'Ya se ha capturado la información para ese proyecto en el periodo seleccionado.');
                        limpiarTabla();
                        $('#btnImprimir').css('display', 'none');
                        $('#btnGuardar').css('display', 'none');
                    }
                } else {
                    AlertaGeneral('Alerta', 'Error al consultar la información.');
                }
            });
        }
        function limpiarTabla() {
            $('#tblData tbody tr').remove();
            $('#tblData tfoot tr').remove();
        }
        function fillTabla(data, sumaHHPeriodo) {
            var totalCargoMaquina = 0;
            for (i = 0; i < data.length; i++) {
                var fila = document.createElement('tr');

                var celdaEconomico = document.createElement('td');
                var celdaDescripcion = document.createElement('td');
                var celdaAreaCuenta = document.createElement('td');
                var celdaHH = document.createElement('td');
                var celdaPorcentajeCargo = document.createElement('td');
                celdaPorcentajeCargo.classList.add('celdaNumerica');
                var celdaCargoMaquina = document.createElement('td');
                celdaCargoMaquina.classList.add('celdaNumerica');

                celdaEconomico.textContent = data[i].noEconomico;
                celdaDescripcion.textContent = data[i].descripcion;
                celdaAreaCuenta.textContent = data[i].cc;
                celdaHH.textContent = data[i].hhPeriodo;

                var porcentajeCargo = sumaHHPeriodo != 0 ? (data[i].hhPeriodo / sumaHHPeriodo) * 100 : 0;

                var porcentajeCargoStringConDosDecimales = (Math.round(100 * porcentajeCargo) / 100).toString() + '%';
                var porcentajeCargoString = porcentajeCargo.toString() + '%';

                celdaPorcentajeCargo.textContent = porcentajeCargoStringConDosDecimales;
                celdaCargoMaquina.textContent = maskNumero((unmaskNumero(porcentajeCargoString) / 100) * unmaskNumero($('#inputNominaSemanal').val()));

                $(fila).append(celdaEconomico);
                $(fila).append(celdaDescripcion);
                $(fila).append(celdaAreaCuenta);
                $(fila).append(celdaHH);
                $(fila).append(celdaPorcentajeCargo);
                $(fila).append(celdaCargoMaquina);




                $('#tblData tbody').append(fila);

                totalCargoMaquina += (unmaskNumero(porcentajeCargoString) / 100) * unmaskNumero($('#inputNominaSemanal').val())
            }

            var filaFooter = document.createElement('tr');

            var celdaBlank1 = document.createElement('td');
            var celdaBlank2 = document.createElement('td');

            var celdaLabelTotales = document.createElement('td');
            celdaLabelTotales.classList.add('celdaNumerica');
            celdaLabelTotales.style.fontWeight = 'bold';

            var celdaTotalHH = document.createElement('td');
            celdaTotalHH.style.borderTop = '1px solid #ddd'
            celdaTotalHH.style.borderBottom = '1px solid #999'
            celdaTotalHH.style.borderLeft = '1px solid black'
            celdaTotalHH.style.borderRight = '1px solid black';
            var celdaBlank3 = document.createElement('td');

            var celdaTotalCargo = document.createElement('td');
            celdaTotalCargo.classList.add('celdaNumerica');
            celdaTotalCargo.style.borderTop = '1px solid #ddd'
            celdaTotalCargo.style.borderBottom = '1px solid #999'
            celdaTotalCargo.style.borderLeft = '1px solid black'
            celdaTotalCargo.style.borderRight = '1px solid #999';

            celdaLabelTotales.textContent = 'Totales'
            celdaTotalHH.textContent = sumaHHPeriodo;

            celdaTotalCargo.textContent = maskNumero(totalCargoMaquina);

            $(filaFooter).append(celdaBlank1);
            $(filaFooter).append(celdaBlank2);
            $(filaFooter).append(celdaLabelTotales);
            $(filaFooter).append(celdaTotalHH);
            $(filaFooter).append(celdaBlank3);
            $(filaFooter).append(celdaTotalCargo);

            $('#tblData tfoot').append(filaFooter);
        }

        function initCbo() {
            $('#multiSelectProyecto').fillCombo('/CatObra/cboCentroCostosUsuarios', null, true);
            var exists = 0 != $.grep($('#multiSelectProyecto option'), function( a ) {
                return a.value == '1018';
            }).length;
            $('#multiSelectProyecto').find('option').remove().end();

            if(exists) $('#multiSelectProyecto').append('<option value="1018" name="undefined" data-prefijo="TOV" data-comboid="null">1018 - TALLER OVERHAUL (VIRTUAL)</option>');

            /// convertToMultiselect('#multiSelectProyecto');
            $('#selectPeriodo').fillCombo('/CatInventario/FillCboSemanas', null, false);
        }
        function GuardarReporte(e, selector, data) {
            $('#btnImprimir').css('display', 'none');
            $('#btnGuardar').css('display', 'none');
            let isReport = $(this).data().isreport;
            ireport.attr("src", `/Reportes/Vista.aspx?idReporte=86&inMemory=1&idNomina=0`);
            document.getElementById('report').onload = function () {
                if (isReport)
                    openCRModal();
                $.ajax({
                    type: "POST",
                    url: '/RepCargoNominaCCArrendadora/GuardarCargoNominaCC',
                    dataType: 'json',
                    succes: function (response) {
                        AlertaGeneral("Alerta", "Datos guardados. Se a enviado notificación a Nóminas.");
                    },
                    error: function (error) {
                    }
                });
                $.unblockUI();
                AlertaGeneral("Alerta", "Reporte Guardado");
            };
            e.preventDefault();
        }

        init();
    };

    $(document).ready(function () {
        maquinaria.reporte.repCargoNomCCArre = new repCargoNomCCArre();
    })
        .ajaxStart(function () {
            $.blockUI({
                message: 'Procesando...',
                baseZ: 2000
            });
        })
        .ajaxStop(function () { $.unblockUI(); });
})();