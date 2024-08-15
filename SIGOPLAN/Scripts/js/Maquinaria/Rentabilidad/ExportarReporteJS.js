(function () {

    $.namespace('Maquinaria.Rentabilidad.ExportarReporte');

    exportarreporte = function () {
        const cbTipoCorte = $("#cbTipoCorte");
        const inputCorte = $("#inputCorte");
        const cboHoraCorte = $("#cboHoraCorte");
        const comboDivision = $("#comboDivision");
        const comboResponsable = $("#comboResponsable");
        const spanComboAC = $("#spanComboAC");
        const comboAC = $("#comboAC");

        const btnReporteSemanalActual = $("#btnReporteSemanalActual");
        const btnReporteSubcuentaActual = $("#btnReporteSubcuentaActual");
        const btnReporteSemanalAcumulado = $("#btnReporteSemanalAcumulado");
        const btnReporteSubcuentaAcumulado = $("#btnReporteSubcuentaAcumulado");
        const btnReporteSaldosCplan = $("#btnReporteSaldosCplan");
        const btnReporteSaldosArrendadora = $("#btnReporteSaldosArrendadora");
        const btnReporteSaldosColombia = $("#btnReporteSaldosColombia");
        const btnReporteSaldosCplanEici = $("#btnReporteSaldosCplanEici");
        const btnReporteClientes = $('#btnReporteClientes');
        const btnReporteVencimientos = $('#btnReporteVencimientos');
        const btnReporteSaldosCplanIntegradora = $('#btnReporteSaldosCplanIntegradora');

        const btnReporteClientesEici = $("#btnReporteClientesEici");
        const btnGuardarEstimados = $("#btnGuardarEstimados");
        const btnGuardarEstimadosFlujo = $("#btnGuardarEstimadosFlujo");
        const btnGuardarSoloEstimados = $("#btnGuardarSoloEstimados");
        const btnCargarComponentes = $("#btnCargarComponentes");
        const btnReporteCplan = $("#btnReporteCplan");

        const getLstFchasCortes = new URL(window.location.origin + '/Rentabilidad/getLstFechasCortes');
        const guardarEstimadosArr = new URL(window.location.origin + '/Rentabilidad/GuardarCorteFlujo');
        const guardarEstimadosArr2 = new URL(window.location.origin + '/Administrativo/FlujoEfectivoArrendadora/guardarDetProyeccionCierreKubrix');
        const guardarSoloEstimados = new URL(window.location.origin + '/Rentabilidad/GuardarCorteEstimados');
        const cargarComponentesAPresupuesto = new URL(window.location.origin + '/Overhaul/cargarComponentesAPresupuesto');
        const enviarCorreoSemanalCplan = new URL(window.location.origin + '/Rentabilidad/EnviarCorreoSemanalCplan');

        let availableDates = [];

        function init()
        {
            initDatePickers();
            fillCombos();
            agregarListeners();
            updateDatePicker();
        }

        function initDatePickers()
        {
            inputCorte.datepicker({
                Button: false,
                dateFormat: "dd-mm-yy",
                onSelect: function (dateText) {
                    //$("#inputDiaFinalAnalisis").val(dateText);
                    //inputDiaFinal.val(dateText);
                    cargarHorasCorte();
                },
                beforeShowDay: function (date) {
                    var auxArr = false;
                    $.each(availableDates, function (index, value) {
                        if (date.getDate() == value.getDate() && date.getMonth() == value.getMonth() && date.getYear() == value.getYear()) {
                            auxArr = true;
                        }
                    });
                    return [auxArr, ""];
                }
            });
        }

        function cargarHorasCorte() {
            cboHoraCorte.fillCombo('/Rentabilidad/getHorasCorte', { fecha: inputCorte.val(), tipoCorte: cbTipoCorte.val() }, null, "");
            cboHoraCorte.find('option').get(0).remove();
        }

        function fillCombos() {
            comboDivision.fillCombo('/Rentabilidad/fillComboDivision', {}, false, "TODAS");
            comboResponsable.fillCombo('/Rentabilidad/fillComboResponsable', {}, false, "TODOS");

            comboAC.select2({ closeOnSelect: false });
            comboAC.fillCombo('cboObraKubrix', null, false, "TODOS");
            comboAC.find('option').get(0).remove();
        }

        function agregarListeners() {
            cbTipoCorte.change(updateDatePicker);
            comboDivision.change(cargarAC);
            comboResponsable.change(cargarAC);

            //-- comboAC multiselect custom --//
            comboAC.next(".select2-container").css("display", "none");
            $("#spanComboAC").click(function(e){
                comboAC.next(".select2-container").css("display", "block");
                comboAC.siblings("span").find(".select2-selection__rendered")[0].click();
            });
            comboAC.on('select2:close', function (e)
            {
                comboAC.next(".select2-container").css("display", "none");
                var seleccionados = $(this).siblings("span").find(".select2-selection__choice");
                if(seleccionados.length == 0) $("#spanComboAC").text("TODOS");
                else {
                    if (seleccionados.length  == 1) $("#spanComboAC").text($(seleccionados[0]).text().slice(1));
                    else $("#spanComboAC").text(seleccionados.length.toString() + " Seleccionados");
                }                
            });
            comboAC.on("select2:unselect", function (evt) {
                if (!evt.params.originalEvent) { return; }
                evt.params.originalEvent.stopPropagation();
            });
            btnReporteSemanalActual.click(setExcelSemanalActual);
            btnReporteSubcuentaActual.click(setExcelSubcuentasActual);
            btnReporteSemanalAcumulado.click(setExcelSemanalAcumulado);
            btnReporteSubcuentaAcumulado.click(setExcelSubcuentasAcumulado);
            btnReporteSaldosCplan.click(setExcelSaldosCplan);
            btnReporteSaldosColombia.click(setExcelSaldosColombia);
            btnReporteSaldosCplanEici.click(setExcelSaldosCplanEici);
            btnReporteClientes.click(setExcelClientesCplan);
            btnReporteVencimientos.click(setExcelVencimientosCplan);
            btnReporteSaldosCplanIntegradora.click(setExcelSaldosCplanIntegradora);

            btnReporteClientesEici.click(setExcelClientesCplanEici);
            btnGuardarEstimados.click(guardarEstimados);
            btnGuardarEstimadosFlujo.click(guardarEstimadosFlujo);
            btnGuardarSoloEstimados.click(getSoloEstimados);
            btnCargarComponentes.click(getComponentesAPresupuesto);
            btnReporteCplan.click(CorreoSemanalCplan);
        }

        function updateDatePicker() {
            setLstFechaCortes();
            inputCorte.datepicker("destroy");
            inputCorte.datepicker({
                Button: false,
                dateFormat: "dd-mm-yy",
                onSelect: function (dateText) {
                    cargarHorasCorte();
                },
                beforeShowDay: function (date) {
                    var auxArr = false;
                    $.each(availableDates, function (index, value) {
                        if (date.getDate() == value.getDate() && date.getMonth() == value.getMonth() && date.getYear() == value.getYear()) {
                            auxArr = true;
                        }
                    });
                    return [auxArr, ""];
                }
            });
            inputCorte.val("");
            cboHoraCorte.clearCombo();
        }

        function cargarAC() {
            comboAC.fillComboAsync('cboObraKubrix', { divisionID: comboDivision.val(), responsableID: comboResponsable.val() }, false);
            comboAC.find('option').get(0).remove();
            if (comboDivision.val() == "TODOS" && comboResponsable.val() == "TODOS") comboAC.find('option').prop('selected', false).change();
            else comboAC.find('option').prop('selected', true).change();
            comboAC.trigger({ type: 'select2:close' });

            comboAC.next(".select2-container").css("display", "none");
            var seleccionados = $(this).siblings("span").find(".select2-selection__choice");
            if (seleccionados.length == 0) $("#spanComboAC").text("TODOS");
            else {
                if (seleccionados.length == 1) $("#spanComboAC").text($(seleccionados[0]).text().slice(1));
                else $("#spanComboAC").text(seleccionados.length.toString() + " Seleccionados");
            }
        }

        function setLstFechaCortes() {
            $.post(getLstFchasCortes, { tipoCorte: cbTipoCorte.val() })
                .then(function (response) {
                    //$.unblockUI;
                    if (response.success) {
                        // Operación exitosa.
                        availableDates = [];
                        for (let i = 0; i < response.fechas.length; i++) {
                            availableDates.push(new Date(parseInt(response.fechas[i].substr(6))));
                        }
                        if (availableDates.length > 0) {
                            inputCorte.datepicker("setDate", availableDates[availableDates.length - 1]);
                            cargarHorasCorte();
                        }
                    } else {
                        // Operación no completada.
                        AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
                    }
                }, function (error) {
                    //$.unblockUI;
                    // Error al lanzar la petición.
                    AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status + ' - ' + error.statusText + '.');
                }
            );
        }

        function setExcelSemanalActual() {
            var notSelected = $("#comboAC").find('option').not(':selected');
            var array = notSelected.map(function () {
                return this.value;
            }).get();
            var array2 = [];
            var areaCuentaFinal = (comboAC.val().length < 1 && comboDivision.val() == "TODAS" && comboResponsable.val() == "TODOS") ? array2 : (comboAC.val().length < 1 ? array : comboAC.val()),

            areaCuentaFinal = unique(areaCuentaFinal);

            var areaCuentaFinalQry = areaCuentaFinal.map(function (el, idx) {
                return 'areaCuenta[' + idx + ']=' + el;
            }).join('&');

            $.blockUI({ message: 'Procesando...' });

            $(this).download = '/Rentabilidad/crearExcelSemanalKubrix?corteID=' + cboHoraCorte.val() + '&tipo=1';
            $(this).href = '/Rentabilidad/crearExcelSemanalKubrix?corteID=' + cboHoraCorte.val() + '&tipo=1';
            location.href = '/Rentabilidad/crearExcelSemanalKubrix?corteID=' + cboHoraCorte.val() + '&tipo=1';

            $.unblockUI();
        }

        function setExcelSemanalAcumulado() {
            var notSelected = $("#comboAC").find('option').not(':selected');
            var array = notSelected.map(function () {
                return this.value;
            }).get();
            var array2 = [];
            var areaCuentaFinal = (comboAC.val().length < 1 && comboDivision.val() == "TODAS" && comboResponsable.val() == "TODOS") ? array2 : (comboAC.val().length < 1 ? array : comboAC.val()),

            areaCuentaFinal = unique(areaCuentaFinal);

            var areaCuentaFinalQry = areaCuentaFinal.map(function (el, idx) {
                return 'areaCuenta[' + idx + ']=' + el;
            }).join('&');

            $.blockUI({ message: 'Procesando...' });

            $(this).download = '/Rentabilidad/crearExcelSemanalKubrix?corteID=' + cboHoraCorte.val() + '&tipo=2';
            $(this).href = '/Rentabilidad/crearExcelSemanalKubrix?corteID=' + cboHoraCorte.val() + '&tipo=2';
            location.href = '/Rentabilidad/crearExcelSemanalKubrix?corteID=' + cboHoraCorte.val() + '&tipo=2';

            $.unblockUI();
        }

        function setExcelSubcuentasActual() {
            var notSelected = $("#comboAC").find('option').not(':selected');
            var array = notSelected.map(function () {
                return this.value;
            }).get();
            var array2 = [];
            var areaCuentaFinal = (comboAC.val().length < 1 && comboDivision.val() == "TODAS" && comboResponsable.val() == "TODOS") ? array2 : (comboAC.val().length < 1 ? array : comboAC.val()),

            areaCuentaFinal = unique(areaCuentaFinal);

            var areaCuentaFinalQry = areaCuentaFinal.map(function (el, idx) {
                return 'areaCuenta[' + idx + ']=' + el;
            }).join('&');

            $.blockUI({ message: 'Procesando...' });

            $(this).download = '/Rentabilidad/crearExcelPorSubcuentaKubrix?corteID=' + cboHoraCorte.val() + '&tipo=1';
            $(this).href = '/Rentabilidad/crearExcelPorSubcuentaKubrix?corteID=' + cboHoraCorte.val() + '&tipo=1';
            location.href = '/Rentabilidad/crearExcelPorSubcuentaKubrix?corteID=' + cboHoraCorte.val() + '&tipo=1';

            $.unblockUI();
        }

        function setExcelSubcuentasAcumulado() {
            var notSelected = $("#comboAC").find('option').not(':selected');
            var array = notSelected.map(function () {
                return this.value;
            }).get();
            var array2 = [];
            var areaCuentaFinal = (comboAC.val().length < 1 && comboDivision.val() == "TODAS" && comboResponsable.val() == "TODOS") ? array2 : (comboAC.val().length < 1 ? array : comboAC.val()),

            areaCuentaFinal = unique(areaCuentaFinal);

            var areaCuentaFinalQry = areaCuentaFinal.map(function (el, idx) {
                return 'areaCuenta[' + idx + ']=' + el;
            }).join('&');

            $.blockUI({ message: 'Procesando...' });

            $(this).download = '/Rentabilidad/crearExcelPorSubcuentaKubrix?corteID=' + cboHoraCorte.val() + '&tipo=2';
            $(this).href = '/Rentabilidad/crearExcelPorSubcuentaKubrix?corteID=' + cboHoraCorte.val() + '&tipo=2';
            location.href = '/Rentabilidad/crearExcelPorSubcuentaKubrix?corteID=' + cboHoraCorte.val() + '&tipo=2';

            $.unblockUI();
        }

        function exceldeprueba()
        {
            var notSelected = $("#comboAC").find('option').not(':selected');
            var array = notSelected.map(function () {
                return this.value;
            }).get();
            var array2 = [];
            var areaCuentaFinal = (comboAC.val().length < 1 && comboDivision.val() == "" && comboResponsable.val() == "") ? array2 : (comboAC.val().length < 1 ? array : comboAC.val()),

            areaCuentaFinal = unique(areaCuentaFinal);

            var areaCuentaFinalQry = areaCuentaFinal.map(function (el, idx) {
                return 'areaCuenta[' + idx + ']=' + el;
            }).join('&');

            $.blockUI({ message: 'Procesando...' });

            $(this).download = '/Rentabilidad/crearExcelPrueba?corteID=' + cboHoraCorte.val() + '';
            $(this).href = '/Rentabilidad/crearExcelPrueba?corteID=' + cboHoraCorte.val() + '';
            location.href = '/Rentabilidad/crearExcelPrueba?corteID=' + cboHoraCorte.val() + '';

            $.unblockUI();
        }
        

        //Nuevos reportes//

        function excelnuevo1() {
            var notSelected = $("#comboAC").find('option').not(':selected');
            var array = notSelected.map(function () {
                return this.value;
            }).get();
            var array2 = [];
            var areaCuentaFinal = (comboAC.val().length < 1 && comboDivision.val() == "" && comboResponsable.val() == "") ? array2 : (comboAC.val().length < 1 ? array : comboAC.val()),

            areaCuentaFinal = unique(areaCuentaFinal);

            var areaCuentaFinalQry = areaCuentaFinal.map(function (el, idx) {
                return 'areaCuenta[' + idx + ']=' + el;
            }).join('&');

            $.blockUI({ message: 'Procesando...' });

            $(this).download = '/Rentabilidad/crearExcel1?corteID=' + cboHoraCorte.val() + '';
            $(this).href = '/Rentabilidad/crearExcel1?corteID=' + cboHoraCorte.val() + '';
            location.href = '/Rentabilidad/crearExcel1?corteID=' + cboHoraCorte.val() + '';

            $.unblockUI();
        }

        function excelnuevo2() {
            var notSelected = $("#comboAC").find('option').not(':selected');
            var array = notSelected.map(function () {
                return this.value;
            }).get();
            var array2 = [];
            var areaCuentaFinal = (comboAC.val().length < 1 && comboDivision.val() == "" && comboResponsable.val() == "") ? array2 : (comboAC.val().length < 1 ? array : comboAC.val()),

            areaCuentaFinal = unique(areaCuentaFinal);

            var areaCuentaFinalQry = areaCuentaFinal.map(function (el, idx) {
                return 'areaCuenta[' + idx + ']=' + el;
            }).join('&');

            $.blockUI({ message: 'Procesando...' });

            $(this).download = '/Rentabilidad/crearExcel2?corteID=' + cboHoraCorte.val() + '';
            $(this).href = '/Rentabilidad/crearExcel2?corteID=' + cboHoraCorte.val() + '';
            location.href = '/Rentabilidad/crearExcel2?corteID=' + cboHoraCorte.val() + '';

            $.unblockUI();
        }


        function excelnuevo3() {
            var notSelected = $("#comboAC").find('option').not(':selected');
            var array = notSelected.map(function () {
                return this.value;
            }).get();
            var array2 = [];
            var areaCuentaFinal = (comboAC.val().length < 1 && comboDivision.val() == "" && comboResponsable.val() == "") ? array2 : (comboAC.val().length < 1 ? array : comboAC.val()),

            areaCuentaFinal = unique(areaCuentaFinal);

            var areaCuentaFinalQry = areaCuentaFinal.map(function (el, idx) {
                return 'areaCuenta[' + idx + ']=' + el;
            }).join('&');

            $.blockUI({ message: 'Procesando...' });

            $(this).download = '/Rentabilidad/crearExcel3?corteID=' + cboHoraCorte.val() + '';
            $(this).href = '/Rentabilidad/crearExcel3?corteID=' + cboHoraCorte.val() + '';
            location.href = '/Rentabilidad/crearExcel3?corteID=' + cboHoraCorte.val() + '';

            $.unblockUI();
        }


        //fin reportes nuevos//



        function setExcelSaldosCplan() {
            $.blockUI({ message: 'Procesando...' });
            $(this).download = '/Rentabilidad/CrearExcelSaldosCplanKubrix?corteID=' + cboHoraCorte.val();
            $(this).href = '/Rentabilidad/CrearExcelSaldosCplanKubrix?corteID=' + cboHoraCorte.val();
            location.href = '/Rentabilidad/CrearExcelSaldosCplanKubrix?corteID=' + cboHoraCorte.val();
            $.unblockUI();
        }

        function setExcelSaldosColombia() {
            $.blockUI({ message: 'Procesando...' });
            $(this).download = '/Rentabilidad/CrearExcelSaldosColombiaKubrix?corteID=' + cboHoraCorte.val();
            $(this).href = '/Rentabilidad/CrearExcelSaldosColombiaKubrix?corteID=' + cboHoraCorte.val();
            location.href = '/Rentabilidad/CrearExcelSaldosColombiaKubrix?corteID=' + cboHoraCorte.val();
            $.unblockUI();
        }

        function setExcelSaldosCplan() {
            $.blockUI({ message: 'Procesando...' });
            $(this).download = '/Rentabilidad/CrearExcelSaldosCplanKubrix?corteID=' + cboHoraCorte.val();
            $(this).href = '/Rentabilidad/CrearExcelSaldosCplanKubrix?corteID=' + cboHoraCorte.val();
            location.href = '/Rentabilidad/CrearExcelSaldosCplanKubrix?corteID=' + cboHoraCorte.val();
            $.unblockUI();
        }

        function setExcelSaldosCplanEici() {
            $.blockUI({ message: 'Procesando...' });

            $(this).download = '/Rentabilidad/CrearExcelSaldosCplanEiciKubrix?corteID=' + cboHoraCorte.val();
            $(this).href = '/Rentabilidad/CrearExcelSaldosCplanEiciKubrix?corteID=' + cboHoraCorte.val();
            location.href = '/Rentabilidad/CrearExcelSaldosCplanEiciKubrix?corteID=' + cboHoraCorte.val();

            $.unblockUI();

        }

        function setExcelClientesCplan() {
            $.blockUI({ message: 'Procesando...' });

            $(this).download = '/Rentabilidad/CrearExcelClientesCplanKubrix?corteID=' + cboHoraCorte.val();
            $(this).href = '/Rentabilidad/CrearExcelClientesCplanKubrix?corteID=' + cboHoraCorte.val();
            location.href = '/Rentabilidad/CrearExcelClientesCplanKubrix?corteID=' + cboHoraCorte.val();

            $.unblockUI();

        }

        function setExcelClientesCplanEici() {
            $.blockUI({ message: 'Procesando...' });

            $(this).download = '/Rentabilidad/CrearExcelClientesCplanEiciKubrix?corteID=' + cboHoraCorte.val();
            $(this).href = '/Rentabilidad/CrearExcelClientesCplanEiciKubrix?corteID=' + cboHoraCorte.val();
            location.href = '/Rentabilidad/CrearExcelClientesCplanEiciKubrix?corteID=' + cboHoraCorte.val();

            $.unblockUI();

        }

        function setExcelVencimientosCplan() {
            $.blockUI({ message: 'Procesando...' });

            $(this).download = '/Rentabilidad/CrearExcelVencimientosCplanKubrix?corteID=' + cboHoraCorte.val();
            $(this).href = '/Rentabilidad/CrearExcelVencimientosCplanKubrix?corteID=' + cboHoraCorte.val();
            location.href = '/Rentabilidad/CrearExcelVencimientosCplanKubrix?corteID=' + cboHoraCorte.val();

            $.unblockUI();

        }

        function setExcelSaldosCplanIntegradora() {
            $.blockUI({ message: 'Procesando...' });
            $(this).download = '/Rentabilidad/CrearExcelSaldosCplanIntegradoraKubrix?corteID=' + cboHoraCorte.val();
            $(this).href = '/Rentabilidad/CrearExcelSaldosCplanIntegradoraKubrix?corteID=' + cboHoraCorte.val();
            location.href = '/Rentabilidad/CrearExcelSaldosCplanIntegradoraKubrix?corteID=' + cboHoraCorte.val();
            $.unblockUI();
        }

        function unique(array) {
            return array.filter(function (el, index, arr) {
                return index === arr.indexOf(el);
            });
        }

        function guardarEstimados() {
            $.post(guardarEstimadosArr, { })
                .then(function (response) {
                    //$.unblockUI;
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral('Éxito', 'Se guardó correctamente el corte');
                    } else {
                        // Operación no completada.
                        AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
                    }
                }, function (error) {
                    //$.unblockUI;
                    // Error al lanzar la petición.
                    AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status + ' - ' + error.statusText + '.');
                }
            );
        }

        function guardarEstimadosFlujo() {
            $.post(guardarEstimadosArr2, {})
                .then(function (response) {
                    //$.unblockUI;
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral('Éxito', 'Se guardó correctamente el corte');
                    } else {
                        // Operación no completada.
                        AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
                    }
                }, function (error) {
                    //$.unblockUI;
                    // Error al lanzar la petición.
                    AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status + ' - ' + error.statusText + '.');
                }
            );
        }

        
        function getSoloEstimados() {
            $.post(guardarSoloEstimados, {})
                .then(function (response) {
                    //$.unblockUI;
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral('Éxito', 'Se guardó correctamente el corte');
                    } else {
                        // Operación no completada.
                        AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
                    }
                }, function (error) {
                    //$.unblockUI;
                    // Error al lanzar la petición.
                    AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status + ' - ' + error.statusText + '.');
                }
            );
        }

        function getComponentesAPresupuesto() {
            $.post(cargarComponentesAPresupuesto, {})
                .then(function (response) {
                    //$.unblockUI;
                    if (response.success) {
                        // Operación exitosa.
                        console.log(response.data);
                    } else {
                        // Operación no completada.
                        AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
                    }
                }, function (error) {
                    //$.unblockUI;
                    // Error al lanzar la petición.
                    AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status + ' - ' + error.statusText + '.');
                }
            );
        }

        function CorreoSemanalCplan() {
            let tipo = cbTipoCorte.val();

            $.post(enviarCorreoSemanalCplan, { tipo: tipo })
                .then(function (response) {
                    //$.unblockUI;
                    if (response.success) {
                        // Operación exitosa.
                    } else {
                        // Operación no completada.
                        AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
                    }
                }, function (error) {
                    //$.unblockUI;
                    // Error al lanzar la petición.
                    AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status + ' - ' + error.statusText + '.');
                }
            );
        }
        
        init();
    };
    $(document).ready(function () {
        Maquinaria.Rentabilidad.ExportarReporte = new exportarreporte();
    }).ajaxStart(function () { $.blockUI({ baseZ: 2000, message: 'Procesando...' }) }).ajaxStop($.unblockUI);
})();
