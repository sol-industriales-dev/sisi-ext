(() => {
    $.namespace('Maquinaria.Rentabilidad.Rentabilidad');
    Rentabilidad = function () {
        id = $.urlParam('id');
        // Variables.
        //// Filtros.
        const comboAC = $('#comboAC');
        const comboTipo = $('#comboTipo');
        const comboGrupo = $('#comboGrupo');
        const comboModelo = $('#comboModelo');
        const comboCC = $('#comboCC');
        const inputMesInicial = $('#inputMesInicial');
        const inputMesFinal = $('#inputMesFinal');
        const comboTipoReporte = $('#comboTipoReporte');
        const lblLegend = $('#lblLegend');
        const botonBuscar = $('#botonBuscar');
        const getLstRentabilidad = new URL(window.location.origin + '/Rentabilidad/getLstRentabilidad');

        const getLstRentabilidadDetalle = new URL(window.location.origin + '/Rentabilidad/getLstRentabilidadDetalle');

        //// Tabla Rentabilidad
        const tablaRentabilidad = $('#tablaRentabilidad');
        let dtTablaRentabilidad;

        //// Detalles
        const modalDetalles = $('#modalDetalles');
        //const inputNombreDetalle = $('#inputNombreDetalle');
        //const botonSubdetalle = $('#botonSubdetalle');

        const divTablaNivelUno = $('#divTablaNivelUno');
        const tablaDetalles = $('#tablaDetalles');
        let dtTablaDetalles;
        const botonNombreNivelCero = $("#botonNombreNivelCero");

        const divTablaNivelDos = $('#divTablaNivelDos');
        const tablaSubdetalles = $('#tablaSubdetalles');
        let dtTablaSubdetalles;
        const botonNombreNivelUno = $("#botonNombreNivelUno");

        const divTablaNivelCero = $('#divTablaNivelCero');
        const tablaSctaDetalles = $('#tablaSctaDetalles');
        let dtTablaSctaDetalles;
        const botonNombreNivelDos = $("#botonNombreNivelDos");

        const chbCostoHora = $("#chbCostoHora"); 
        
        let auxDatos = [];

        (function init() {
            // Lógica de inicialización.
            initDatepickers();
            fillCombos();
            agregarListeners();

            initTablaSctaDetalles();
            initTablaDetalles();
            initTablaSubdetalles();

            if(id == 3) { divTablaNivelUno.hide(); botonNombreNivelUno.hide(); botonNombreNivelDos.hide(); }
            else { divTablaNivelCero.hide(); botonNombreNivelCero.hide(); botonNombreNivelDos.hide(); }
            
            if(id == 4) { chbCostoHora.parent().css("visibility", "visible"); }

            divTablaNivelDos.hide();
            //botonSubdetalle.hide();
            cambiarTexto();
            
        })();

        // Métodos.
        function agregarListeners() {
            comboAC.change(cargarMaquinas);
            comboTipo.change(cargarGrupos);
            comboGrupo.change(cargarModelos)
            comboModelo.change(cargarMaquinas);
            botonBuscar.click(setLstRentabilidad);

            modalDetalles.on("hide.bs.modal", () => {
                dtTablaSctaDetalles.clear().draw();
                dtTablaDetalles.clear().draw();
                dtTablaSubdetalles.clear().draw();
                if(id == 3) {
                    divTablaNivelCero.show();
                    botonNombreNivelCero.show();
                    divTablaNivelUno.hide();
                    botonNombreNivelUno.hide();
                }    
                else {
                    divTablaNivelUno.show();
                    botonNombreNivelUno.show();
                    divTablaNivelCero.hide();
                    botonNombreNivelCero.hide();
                }
                divTablaNivelDos.hide();
                botonNombreNivelDos.hide();
                //inputNombreDetalle.val('');
                //botonSubdetalle.html('');
                //botonSubdetalle.hide();
            });
            
            chbCostoHora.change(() => {
                if(auxDatos.length > 0) {
                    if(!chbCostoHora.prop('checked')) {
                        let data = JSON.parse(JSON.stringify(auxDatos));
                        for(var i = 0; i < auxDatos.length; i++) {
                            var horasTrabajadas = data[i].horasTrabajadas;
                            if(horasTrabajadas != 0) {
                                data[i].materialesLubricacion = data[i].materialesLubricacion / horasTrabajadas;
                                data[i].refacciones = data[i].refacciones / horasTrabajadas;
                                data[i].herramientas = data[i].herramientas / horasTrabajadas;
                                data[i].combustibles = data[i].combustibles / horasTrabajadas;
                                data[i].talleresExternos = data[i].talleresExternos / horasTrabajadas;
                                data[i].servicios = data[i].servicios / horasTrabajadas;
                                data[i].subcontratos = data[i].subcontratos / horasTrabajadas;
                                data[i].fletes = data[i].fletes / horasTrabajadas;
                                data[i].traspasoMM = data[i].traspasoMM / horasTrabajadas;
                                data[i].rentaMaquinaria = data[i].rentaMaquinaria / horasTrabajadas;                                
                                data[i].serviciosAdministrativos = data[i].serviciosAdministrativos / horasTrabajadas;
                                data[i].intereses = data[i].intereses / horasTrabajadas;                                
                                //data[i].detalles.forEach(function(item) {
                                //    item.importe = item.importe / horasTrabajadas
                                //});
                            }
                        }
                        dtTablaRentabilidad.clear();
                        dtTablaRentabilidad.rows.add(data);
                        dtTablaRentabilidad.draw();                        
                    }
                    else {
                        dtTablaRentabilidad.clear();
                        dtTablaRentabilidad.rows.add(auxDatos);
                        dtTablaRentabilidad.draw();
                    }
                }
            });

            botonNombreNivelUno.click(() => {
                if(!divTablaNivelUno.is(":visible")) {
                    divTablaNivelDos.hide(500);
                    divTablaNivelUno.show(500);
                    //inputNombreDetalle.val(tituloNivelUno);
                    botonNombreNivelDos.hide();
                }                 
            });
            botonNombreNivelCero.click(() => {
                if(!divTablaNivelCero.is(":visible")) {
                    divTablaNivelUno.hide(500);
                    divTablaNivelDos.hide(500);
                    divTablaNivelCero.show(500);
                    //inputNombreDetalle.val(tituloNivelCero);
                    botonNombreNivelUno.hide();
                    botonNombreNivelDos.hide();
                }                 
            });
        }

        function setLstRentabilidad() {
            let busq = getBusquedaDTO();
            if (busq.tipoReporte == 0) {
                AlertaGeneral(`Aviso`, `Debe seleccionar el tipo de Rentabilidad`);
                return;
            }
            $.post(getLstRentabilidad, { busq })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        cambiarTexto();
                        auxDatos = response.lst;
                        cargarDatosTablaRentabilidad(response.lst);
                        if(!chbCostoHora.prop('checked')) { chbCostoHora.change(); }
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
        function setLstRentabilidadDetalle(busq, nombreColumna, total, horasTrabajadas) {
            //let busq = getBusquedaDetalleDTO(cta, scta);
            if (busq.tipoReporte == 0) {
                AlertaGeneral(`Aviso`, `Debe seleccionar el tipo de Rentabilidad`);
                return;
            }
            $.post(getLstRentabilidadDetalle, { busq })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        cambiarTexto();
                        var detalles = response.lst[0].detalles;
                        if(!chbCostoHora.prop('checked')) {
                            detalles.forEach(function(item) {
                                item.importe = item.importe / horasTrabajadas
                            });
                        }
                        cargarDetalles(detalles, nombreColumna, total); 
                        //cargarDatosTablaRentabilidad(response.lst);
                        if(!chbCostoHora.prop('checked')) { chbCostoHora.change(); }
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
        function setLstRentabilidadDetalleUtilidad(busq, nombreColumna, total, horasTrabajadas) {
            //let busq = getBusquedaDetalleDTO(cta, scta);
            if (busq.tipoReporte == 0) {
                AlertaGeneral(`Aviso`, `Debe seleccionar el tipo de Rentabilidad`);
                return;
            }
            $.post(getLstRentabilidadDetalle, { busq })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        cambiarTexto();
                        var detalles = response.lst[0].detalles;
                        cargarSubCuenta(detalles, nombreColumna, total); 
                        //cargarDatosTablaRentabilidad(response.lst);
                        if(!chbCostoHora.prop('checked')) { chbCostoHora.change(); }
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
        function textoToOracion(texto) {
            let arr = texto.split(",")
            str = '';
            arr.forEach(function (i, index) {
                str += i;
                if (index == (arr.length - 1)) {
                    str += '.';
                }
                else {
                    if (index == (arr.length - 2)) {
                        str += ' y ';
                    }
                    else {
                        if (index != (arr.length - 2)) {
                            str += ',';
                        }
                    }
                }
            });
            return str;
        }
        function cargarGrupos() {
            const busq = getBusquedaDTO();
            if (busq.tipo == 0) {
                comboGrupo.multiselect('deselectAll');
                comboGrupo.multiselect('disable');
                comboModelo.multiselect('deselectAll');
                comboModelo.multiselect('disable');
            } else {
                $.blockUI({ message: 'Cargando grupos...' });
                comboGrupo.fillComboAsync('cboGrupo', { busq }, false, 'Todos', () => {
                    comboGrupo.multiselect('destroy');
                    convertToMultiselect('#comboGrupo');
                    $.unblockUI();
                });
            }
        }

        function cargarModelos() {
            let busq = getBusquedaDTO();

            $.blockUI({ message: 'Cargando modelos...' });
            comboModelo.fillComboAsync('cboModelo', { busq }, false, 'Todos', () => {
                comboModelo.multiselect('destroy');
                convertToMultiselect('#comboModelo');
                $.unblockUI();
            });
        }

        function cargarMaquinas() {
            let busq = getBusquedaDTO();
            $.blockUI({ message: 'Cargando modelos...' });
            comboCC.fillComboAsync('cboMaquina', { busq }, false, 'Todos', () => {
                comboCC.multiselect('destroy');
                convertToMultiselect('#comboCC');
                $.unblockUI();
            });

        }

        function cambiarTexto() {                
            let texto = $("#comboTipoReporte option:eq(" + id + ")").text();
            $("#tituloVista").text("Reporte de " + texto + "  ");
            $("#tituloVista").append("<i class='fas fa-hand-holding-usd'></i>");
            let tipo = id
                , titulo = tipo == 0 ? "Resultado" : texto;
            lblLegend.text(titulo);
        }

        function getBusquedaDTO() {
            return {
                tipoReporte: id
                , obra: comboAC.val()
                , tipo: comboTipo.val()
                , lstGrupo: getValoresMultiples('#comboGrupo')
                , lstModelo: getValoresMultiples('#comboModelo')
                , lstMaquina: getValoresMultiples('#comboCC')
                , min: inputMesInicial.val()
                , max: inputMesFinal.val()
            };
        }

        function fillCombos() {
            comboAC.fillCombo('/CatObra/cboCentroCostosUsuarios', null, false, "TODOS");
            comboTipo.fillCombo('cboTipo', null, false, "TODOS");
            comboTipoReporte.fillCombo('cboTipoReporte', null, false, null);
            comboGrupo.multiselect();
            comboGrupo.multiselect('disable');

            comboModelo.multiselect();
            comboModelo.multiselect('disable');

            comboCC.multiselect();
            comboCC.multiselect('disable');
        }

        function initDatepickers() {
            let yearActual = new Date().getFullYear();
            inputMesInicial.datepicker({
                Button: false,
                dateFormat: "d MM yy", 
                //i18n: mpDicEsp,
            });
            inputMesFinal.datepicker({
                Button: false,
                dateFormat: "d MM yy", 
                //i18n: mpDicEsp,
            });
            inputMesInicial.datepicker("setDate", new Date(yearActual, 0, 1));
            inputMesFinal.datepicker("setDate", new Date());
            //inputMesInicial.MonthPicker({
            //    Button: false,
            //    MonthFormat: 'MM, yy',
            //    i18n: mpDicEsp,
            //    OnAfterChooseMonth: function () {
            //        let date = inputMesInicial.MonthPicker('GetSelectedDate');
            //        inputMesInicial.val(`${date.toLocaleDateString("es-ES", { month: 'long' }).Capitalize()}, ${date.getFullYear()}`);
            //    }
            //});
            //inputMesFinal.MonthPicker({
            //    Button: false,
            //    MonthFormat: 'MM, yy',
            //    i18n: mpDicEsp,
            //    OnAfterChooseMonth: function () {
            //        let date = inputMesFinal.MonthPicker('GetSelectedDate');
            //        inputMesFinal.val(`${date.toLocaleDateString("es-ES", { month: 'long' }).Capitalize()}, ${date.getFullYear()}`);
            //    }
            //});
            //seMonthPickerValor(inputMesInicial, new Date(fechaActual.getFullYear(), 1, 1));
            //seMonthPickerValor(inputMesFinal, new Date(fechaActual.getFullYear(), fechaActual.getMonth() + 1, 1));
        }

        function cargarDatosTablaRentabilidad(data) {

            if (dtTablaRentabilidad != null) {
                dtTablaRentabilidad.destroy();
                tablaRentabilidad.empty();
                tablaRentabilidad.append(`<thead class="bg-table-header"></thead>`);
            }

            dtTablaRentabilidad = tablaRentabilidad.DataTable({
                //language: dtDicEsp,
                language: {
                    sInfo: "Mostrando _TOTAL_ registro(s)",
                    sSearch: "Filtrar:",
                    sZeroRecords: "No se encontraron resultados",
                    sEmptyTable: "Ningún dato disponible en esta tabla",
                },
                destroy: true,
                paging: false,
                autoWidth: false,
                searching: true,
                dom: '<Bif<t>>',
                //lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
                columns: getColumns(),
                data,
                buttons: [ { extend: 'excel', text: 'Exportar', title: 'Reporte Costo Hora', footer: true, }, ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                drawCallback: getOnDrawCallback(),
                
                fnInitComplete: function(oSettings, json) {
                    $('div#tablaRentabilidad_filter input').addClass("form-control input-sm");
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var api = this.api(), data;
                    var intVal = function ( i ) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '')*1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    const tipoReporte = getTipoReporte();
                    if(tipoReporte == 1) {
                        totalLubricacion = api.column(2).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalRefacciones = api.column(3).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalHerramientas = api.column(4).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalCombustibles = api.column(5).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalTalleres = api.column(6).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalServicios = api.column(7).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalAdministrativos = api.column(8).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalSubcontratos = api.column(9).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalFletes = api.column(10).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalTraspasos = api.column(11).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalRenta = api.column(12).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalIntereses = api.column(13).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        total = api.column(14).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        $(api.column(1).footer()).html('TOTAL');
                        $(api.column(2).footer()).html('$' + parseFloat(totalLubricacion, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(3).footer()).html('$' + parseFloat(totalRefacciones, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(4).footer()).html('$' + parseFloat(totalHerramientas, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(5).footer()).html('$' + parseFloat(totalCombustibles, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(6).footer()).html('$' + parseFloat(totalTalleres, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(7).footer()).html('$' + parseFloat(totalServicios, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(8).footer()).html('$' + parseFloat(totalAdministrativos, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(9).footer()).html('$' + parseFloat(totalSubcontratos, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(10).footer()).html('$' + parseFloat(totalFletes, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(11).footer()).html('$' + parseFloat(totalTraspasos, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(12).footer()).html('$' + parseFloat(totalRenta, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(13).footer()).html('$' + parseFloat(totalIntereses, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(14).footer()).html('$' + parseFloat(total, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                    }
                    if(tipoReporte == 2) {
                        totalRenta = api.column(2).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        //totalMtto = api.column(3).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalFletes = api.column(3).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalDanos = api.column(4).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalNeumaticos = api.column(5).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalOverhaul = api.column(6).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalLento = api.column(7).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        total = api.column(8).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        $(api.column(1).footer()).html('TOTAL');
                        $(api.column(2).footer()).html('$' + parseFloat(totalRenta, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        //$(api.column(3).footer()).html('$' + parseFloat(totalMtto, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(3).footer()).html('$' + parseFloat(totalFletes, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(4).footer()).html('$' + parseFloat(totalDanos, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(5).footer()).html('$' + parseFloat(totalNeumaticos, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(6).footer()).html('$' + parseFloat(totalOverhaul, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(7).footer()).html('$' + parseFloat(totalLento, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(8).footer()).html('$' + parseFloat(total, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                    }

                    if(tipoReporte == 3) {                        
                        totalAbono = api.column(2).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalCargo = api.column(3).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        //totalCostoHora = api.column(4).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        total = api.column(4).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        $(api.column(1).footer()).html('TOTAL');                        
                        $(api.column(2).footer()).html('$' + parseFloat(totalAbono, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(3).footer()).html('$' + parseFloat(totalCargo, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        //$(api.column(4).footer()).html('$' + parseFloat(totalCostoHora, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(4).footer()).html('$' + parseFloat(total, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                    }
                    if(tipoReporte == 4) {
                        totalLubricacion = api.column(3).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalRefacciones = api.column(4).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalHerramientas = api.column(5).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalCombustibles = api.column(6).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalTalleres = api.column(7).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalServicios = api.column(8).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalAdministrativos = api.column(9).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalSubcontratos = api.column(10).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalFletes = api.column(11).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalTraspasos = api.column(12).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalRenta = api.column(13).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalIntereses = api.column(14).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        total = api.column(15).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalCostoHorario = api.column(16).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );                        
                        $(api.column(2).footer()).html('TOTAL');
                        $(api.column(3).footer()).html('$' + parseFloat(totalLubricacion, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(4).footer()).html('$' + parseFloat(totalRefacciones, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(5).footer()).html('$' + parseFloat(totalHerramientas, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(6).footer()).html('$' + parseFloat(totalCombustibles, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(7).footer()).html('$' + parseFloat(totalTalleres, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(8).footer()).html('$' + parseFloat(totalServicios, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(9).footer()).html('$' + parseFloat(totalAdministrativos, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(10).footer()).html('$' + parseFloat(totalSubcontratos, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(11).footer()).html('$' + parseFloat(totalFletes, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(12).footer()).html('$' + parseFloat(totalTraspasos, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(13).footer()).html('$' + parseFloat(totalRenta, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(14).footer()).html('$' + parseFloat(totalIntereses, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(15).footer()).html('$' + parseFloat(total, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(16).footer()).html('$' + parseFloat(totalCostoHorario, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());                        
                    }
                    if(tipoReporte == 5) {
                        totalCosto = api.column(2).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalHrs = api.column(3).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalCargo = api.column(4).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        totalAbono = api.column(5).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        total = api.column(6).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                        $(api.column(1).footer()).html('TOTAL');
                        $(api.column(2).footer()).html('$' + parseFloat(totalCosto, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(3).footer()).html(parseFloat(totalHrs, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(4).footer()).html('$' + parseFloat(totalCargo, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(5).footer()).html('$' + parseFloat(totalAbono, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(api.column(6).footer()).html('$' + parseFloat(total, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                    }
                }
            });
            $('[data-toggle="tooltip"]').tooltip();
        }

        function getTipoReporte() {
            return +id;
        }

        function getColumns() {
            const tipoReporte = getTipoReporte();
            switch (tipoReporte) {
                case 1:
                    tablaRentabilidad.append("<tfoot><tr><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td>"
                        + "<td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td>"
                        + "<td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td></tr></tfoot>");
                    return getCostosColumns();
                case 2:
                    tablaRentabilidad.append("<tfoot><tr><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td>"
                        + "<td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td></tr></tfoot>");
                    return getIngresosColumns();
                    break;
                case 3:
                    tablaRentabilidad.append("<tfoot><tr><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td>"
                        + "</tr></tfoot>");
                    return getUtilidadColumns();                    
                case 4:
                    tablaRentabilidad.append("<tfoot><tr><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td>"
                        + "<td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td>"
                        + "<td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td>"
                        + "</tr></tfoot>");
                    return getCostosHoraColumns();
                case 5:
                    tablaRentabilidad.append("<tfoot><tr><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td><td class='footerSticky'></td>"
                        + "<td class='footerSticky'></td><td class='footerSticky'></td></tr></tfoot>");
                    return getPptoMaqColumns();
                default:
                    break;
            }

        }

        function getCostosColumns() {
            var auxColumnas = [
                { data: 'noEconomico', title: 'No.Econ.' },
                { data: 'modelo', title: 'Modelo' },
                { data: 'materialesLubricacion', title: '<span data-toggle="tooltip" title="5000-1" data-placement="bottom">Materiales de Lubricación</span>', render: data => getRowHTML(data) },
                { data: 'refacciones', title: '<span data-toggle="tooltip" title="5000-2" data-placement="bottom">Refacciones</span>', render: data => getRowHTML(data) },
                { data: 'herramientas', title: '<span data-toggle="tooltip" title="5000-3" data-placement="bottom">Herramientas</span>', render: data => getRowHTML(data) },
                { data: 'combustibles', title: '<span data-toggle="tooltip" title="5000-4" data-placement="bottom">Combustibles</span>', render: data => getRowHTML(data) },
                { data: 'talleresExternos', title: '<span data-toggle="tooltip" title="5000-5" data-placement="bottom">Talleres Externos</span>', render: data => getRowHTML(data) },
                { data: 'servicios', title: '<span data-toggle="tooltip" title="5000-6" data-placement="bottom">Servicios</span>', render: data => getRowHTML(data) },
                { data: 'serviciosAdministrativos', title: '<span data-toggle="tooltip" title="5000-7" data-placement="bottom">Servicios Administrativos</span>', render: data => getRowHTML(data) },
                { data: 'subcontratos', title: '<span data-toggle="tooltip" title="5000-8" data-placement="bottom">Subcontratos</span>', render: data => getRowHTML(data) },
                { data: 'fletes', title: '<span data-toggle="tooltip" title="5000-9" data-placement="bottom">Fletes</span>', render: data => getRowHTML(data) },
                { data: 'traspasoMM', title: '<span data-toggle="tooltip" title="5000-10" data-placement="bottom">Traspasos de M y M</span>', render: data => getRowHTML(data) },
                { data: 'rentaMaquinaria', title: '<span data-toggle="tooltip" title="5000-11" data-placement="bottom">Renta de Maquinaria y Equipos</span>', render: data => getRowHTML(data) },
                { data: 'intereses', title: '<span data-toggle="tooltip" title="5900-3" data-placement="bottom">Intereses</span>', render: data => getRowHTML(data) },
                { data: 'total', title: 'Total', render: data => `<p>${maskNumero(data)}</p>` }
            ];

            return auxColumnas;
        }

        function getIngresosColumns() {
            return [
                { data: 'noEconomico', title: 'Económico' },
                { data: 'modelo', title: 'Modelo' },
                { data: 'rentaEquipos', title: '<span data-toggle="tooltip" title="4000-1" data-placement="bottom">Renta de Equipos</span>', render: data => getRowHTML(data) },
                { data: 'reservaOverhaul', title: '<span data-toggle="tooltip" title="4000-2" data-placement="bottom">Reserva de Overhaul</span>', render: data => getRowHTML(data) },
                //{ data: 'mttoEquipos', title: '<span data-toggle="tooltip" title="4000-3">Materiales</span>Mantenimiento de Equipos', render: data => getRowHTML(data) },    
                { data: 'cobroDanioEquipos', title: '<span data-toggle="tooltip" title="4000-4" data-placement="bottom">Cobro de Daños a Equipos</span>', render: data => getRowHTML(data) },
                { data: 'reparacionNeumaticos', title: '<span data-toggle="tooltip" title="4000-5" data-placement="bottom">Reparación de Neumáticos</span>', render: data => getRowHTML(data) },
                { data: 'cobroFletes', title: '<span data-toggle="tooltip" title="4000-6" data-placement="bottom">Cobro de Fletes de Equipos</span>', render: data => getRowHTML(data) },                
                { data: 'lentoMovimiento', title: '<span data-toggle="tooltip" title="4000-7" data-placement="bottom">Refacciones Lento Movimiento</span>', render: data => getRowHTML(data) },
                { data: 'total', title: 'Total', render: data => `<p>${maskNumero(data)}</p>` }
            ];
        }


        function getUtilidadColumns() {
            return [
                { data: 'noEconomico', title: 'Económico' }
                , { data: 'modelo', title: 'Modelo' }
                , { data: 'abono', title: '<span data-toggle="tooltip" title="Ingreso" data-placement="bottom">Ingreso</span>', render: (data, type, row) => getRowHTML(data) }
                , { data: 'cargo', title: '<span data-toggle="tooltip" title="Costo" data-placement="bottom">Costo</span>', render: (data, type, row) => getRowHTML(data) }
                //, { data: 'costoHorario', title: 'Costo Hora Total', render: (data, type, row) => getNumberHTML(data) }
                , { data: 'total', title: 'Utilidad/Perdida', render: (data, type, row) => getNumberHTML(data) }
            ];
        }

        function getCostosHoraColumns() {
            return [
                { data: 'noEconomico', title: 'Económico' },
                { data: 'modelo', title: 'Modelo' },
                { data: 'horasTrabajadas', title: 'Horas Trabajadas' },
                { data: 'materialesLubricacion', title: '<span data-toggle="tooltip" title="5000-1" data-placement="bottom">Materiales de Lubricación</span>', render: (data, type, row) => getRowHTML(data) },
                { data: 'refacciones', title: '<span data-toggle="tooltip" title="5000-2" data-placement="bottom">Refacciones</span>', render: (data, type, row) => getRowHTML(data) },
                { data: 'herramientas', title: '<span data-toggle="tooltip" title="5000-3" data-placement="bottom">Herramientas</span>', render: (data, type, row) => getRowHTML(data) },
                { data: 'combustibles', title: '<span data-toggle="tooltip" title="5000-4" data-placement="bottom">Combustibles</span>', render: (data, type, row) => getRowHTML(data) },
                { data: 'talleresExternos', title: '<span data-toggle="tooltip" title="5000-5" data-placement="bottom">Talleres Externos</span>', render: (data, type, row) => getRowHTML(data) },
                { data: 'servicios', title: '<span data-toggle="tooltip" title="5000-6" data-placement="bottom">Servicios</span>', render: (data, type, row) => getRowHTML(data) },
                { data: 'serviciosAdministrativos', title: '<span data-toggle="tooltip" title="5000-7" data-placement="bottom">Servicios Administrativos</span>', render: (data, type, row) => getRowHTML(data) },
                { data: 'subcontratos', title: '<span data-toggle="tooltip" title="5000-8" data-placement="bottom">Subcontratos</span>', render: (data, type, row) => getRowHTML(data) },
                { data: 'fletes', title: '<span data-toggle="tooltip" title="5000-9" data-placement="bottom">Fletes</span>', render: (data, type, row) => getRowHTML(data) },
                //{ data: 'traspasoMM', title: '<span data-toggle="tooltip" title="5000-10" data-placement="bottom">Traspasos de M y M</span>', render: (data, type, row) => getRowHTML(data) },
                { data: 'traspasoMM', title: '<span data-toggle="tooltip" title="5000-10" data-placement="bottom">Traspasos de M y M</span>', render: data => getRowHTML(data) },
                { data: 'rentaMaquinaria', title: '<span data-toggle="tooltip" title="5000-11" data-placement="bottom">Renta de Maquinaria y Equipos</span>', render: (data, type, row) => getRowHTML(data) },
                { data: 'intereses', title: '<span data-toggle="tooltip" title="5900-3" data-placement="bottom">Intereses</span>', render: (data, type, row) => getRowHTML(data) },
                { data: 'total', title: 'Total', render: (data, type, row) => getNumberHTML(data) },
                { data: 'totalCostoHorario', title: 'Total Costo Horario', render: (data, type, row) => getNumberHTML(data) },
            ];
        }

        function getPptoMaqColumns() {
            return [
                { data: 'noEconomico', title: 'Económico' },
                { data: 'modelo', title: 'Modelo' },
                { data: 'costoHorario', title: 'Costo Horario', render: (data, type, row) => getNumberHTML(data) },
                { data: 'horasTrabajadas', title: 'Horas Trabajadas' },
                { data: 'bolsaPresupuesto', title: 'Bolsa de Presupuesto', render: (data, type, row) => getNumberHTML(data) },
                { data: 'total', title: 'CostoReal', render: (data, type, row) => getRowHTML(data) },
                { data: 'diferencia', title: 'Diferencia', render: (data, type, row) => getNumberHTML(data) }
            ];
        }

        function getRowHTML(value) {
            return `<p ${value != 0 ? 'class="desplegable"' : ''}>${maskNumero(value)}</p>`;
        }

        function getNumberHTML(value) {
            return `<p class="${value != 0 ? 'noDesplegable' : ''} ${value < 0 ? 'Danger' : ''}" >${maskNumero(value)}</p>`;
        }

        function getOnDrawCallback() {    
            const tipoReporte = getTipoReporte();
            switch (tipoReporte) {
                case 1:
                    return getCostosDrawCallback;
                case 2:
                    return getIngresosDrawCallback;
                case 3:
                    return getUtilidadesDrawCallback;
                case 4:
                    return getCostosHorasDrawCallback;
                case 5:
                    return getPptoMaquinaDrawCallback;
                default:
                    break;
            }
        }

        function getCostosDrawCallback() {
            tablaRentabilidad.find('p.desplegable').unbind().click(function () {
                const p = $(this);
                const rowData = dtTablaRentabilidad.row(p.parents('tr')).data();
                const td = p.parents("td");
                const nombreColumna = $('#tablaRentabilidad thead tr th').eq(td.index()).html().trim();                
                let detallesFiltrados;
                var busq = {
                    tipoReporte: id
                    , obra: comboAC.val()
                    , tipo: comboTipo.val()
                    , lstGrupo: getValoresMultiples('#comboGrupo')
                    , lstModelo: getValoresMultiples('#comboModelo')
                    , lstMaquina: [rowData.noEconomico]
                    , min: inputMesInicial.val()
                    , max: inputMesFinal.val()
                    , cta: 5000
                    , scta: 0
                }
                switch (td.index()) {
                    case 2: // Materiales de lubricación
                        busq.scta = 1;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), 1);
                        break;
                    case 3: // Refacciones
                        busq.scta = 2;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), 1);
                        break;
                    case 4: // Herramientas
                        busq.scta = 3;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), 1);
                        break;
                    case 5: // Combustibles
                        busq.scta = 4;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), 1);
                        break;
                    case 6: // Talleres Externos
                        busq.scta = 5;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), 1);
                        break;
                    case 7: // Servicios
                        busq.scta = 6;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), 1);
                        break;
                    case 8: // Servicios Administrativos
                        busq.scta = 7;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), 1);
                        break;
                    case 9: // Subcontratos
                        busq.scta = 8;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), 1);
                        break;
                    case 10: // Fletes
                        busq.scta = 9;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), 1);
                        break;
                    case 11: // Traspasos M y M
                        busq.scta = 10;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), 1);
                        break;
                    case 12: // Renta de Maquinaria y Equipos
                        busq.scta = 11;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), 1);
                        break;
                    case 13: // Intereses
                        busq.cta = 5900; 
                        busq.scta = 3;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), 1);
                        break;
                    default:
                        return;
                }
                //cargarDetalles(detallesFiltrados, nombreColumna, p.html());                
            });
        }
        function getCostosHorasDrawCallback() {
            tablaRentabilidad.find('p.desplegable').unbind().click(function () {
                const p = $(this);
                const rowData = dtTablaRentabilidad.row(p.parents('tr')).data();
                const td = p.parents("td");
                const nombreColumna = $('#tablaRentabilidad thead tr th').eq(td.index()).html().trim();
                const horasTrabajadas = rowData.horasTrabajadas;
                let detallesFiltrados;
                var busq = {
                    tipoReporte: id
                    , obra: comboAC.val()
                    , tipo: comboTipo.val()
                    , lstGrupo: getValoresMultiples('#comboGrupo')
                    , lstModelo: getValoresMultiples('#comboModelo')
                    , lstMaquina: [rowData.noEconomico]
                    , min: inputMesInicial.val()
                    , max: inputMesFinal.val()
                    , cta: 5000
                    , scta: 0
                }
                switch (td.index() - 1) {
                    case 2: // Materiales de lubricación
                        busq.scta = 1;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), horasTrabajadas);
                        break;
                    case 3: // Refacciones
                        busq.scta = 2;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), horasTrabajadas);
                        break;
                    case 4: // Herramientas
                        busq.scta = 3;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), horasTrabajadas);
                        break;
                    case 5: // Combustibles
                        busq.scta = 4;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), horasTrabajadas);
                        break;
                    case 6: // Talleres Externos
                        busq.scta = 5;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), horasTrabajadas);
                        break;
                    case 7: // Servicios
                        busq.scta = 6;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), horasTrabajadas);
                        break;
                    case 8: // Servicios Administrativos
                        busq.scta = 7;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), horasTrabajadas);
                        break;
                    case 9: // Subcontratos
                        busq.scta = 8;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), horasTrabajadas);
                        break;
                    case 10: // Fletes
                        busq.scta = 9;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), horasTrabajadas);
                        break;
                    case 11: // Traspasos M y M
                        busq.scta = 10;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), horasTrabajadas);
                        break;
                    case 12: // Renta de Maquinaria y Equipos
                        busq.scta = 11;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), horasTrabajadas);
                        break;
                    case 13: // Intereses
                        busq.cta = 5900; 
                        busq.scta = 3;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), horasTrabajadas);
                        break;
                    default:
                        return;
                }

                //cargarDetalles(detallesFiltrados, nombreColumna, p.html());
            });
        }
        function getPptoMaquinaDrawCallback() {
            tablaRentabilidad.find('p.desplegable').unbind().click(function () {                
                const p = $(this);
                const rowData = dtTablaRentabilidad.row(p.parents('tr')).data();
                const td = p.parents("td");
                const nombreColumna = $('#tablaRentabilidad thead tr th').eq(td.index()).html().trim();
                let detallesFiltrados;
                var busq = {
                    tipoReporte: id
                    , obra: comboAC.val()
                    , tipo: comboTipo.val()
                    , lstGrupo: getValoresMultiples('#comboGrupo')
                    , lstModelo: getValoresMultiples('#comboModelo')
                    , lstMaquina: [rowData.noEconomico]
                    , min: inputMesInicial.val()
                    , max: inputMesFinal.val()
                    , cta: 5000
                    , scta: 0
                }
                switch (td.index()) {
                    case 5: 
                        busq.scta = 1;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), horasTrabajadas);
                        break;
                    default:
                        return;
                }
                cargarDetalles(detallesFiltrados, nombreColumna, p.html());
            });
        }
        

        function getIngresosDrawCallback() {
            tablaRentabilidad.find('p.desplegable').unbind().click(function () {
                const p = $(this);
                const rowData = dtTablaRentabilidad.row(p.parents('tr')).data();
                const td = p.parents("td");
                const nombreColumna = $('#tablaRentabilidad thead tr th').eq(td.index()).html().trim();
                var busq = {
                    tipoReporte: id
                    , obra: comboAC.val()
                    , tipo: comboTipo.val()
                    , lstGrupo: getValoresMultiples('#comboGrupo')
                    , lstModelo: getValoresMultiples('#comboModelo')
                    , lstMaquina: [rowData.noEconomico]
                    , min: inputMesInicial.val()
                    , max: inputMesFinal.val()
                    , cta: 4000
                    , scta: 0
                }
                let detallesFiltrados;
                switch (td.index()) {
                    case 2: // Renta de Equipos
                        busq.scta = 1;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), 1);
                        break;
                    case 3: // Cobro de Fletes
                        busq.scta = 2;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), 1);
                        break;
                    //case 4: // Cobro de Daños a Equipos
                        //busq.scta = 3;
                        //setLstRentabilidadDetalle(busq, nombreColumna, p.html(), 1);
                    //    break;
                    case 4: // Venta Activo Flujo
                        busq.scta = 4;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), 1);
                        break;
                    case 5: // Reparación Neumáticos
                        busq.scta = 5;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), 1);
                        break;
                    case 6: // Reserva Overhaul
                        busq.scta = 6;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), 1);
                        break;
                    case 7: // Reserva Overhaul
                        busq.scta = 7;
                        setLstRentabilidadDetalle(busq, nombreColumna, p.html(), 1);
                        break;
                    default:
                        return;
                }

                //cargarDetalles(detallesFiltrados, nombreColumna, p.html());
            });
        }
        function  getUtilidadesDrawCallback() {
            tablaRentabilidad.find('p.desplegable').unbind().click(function () {
                const p = $(this);
                const rowData = dtTablaRentabilidad.row(p.parents('tr')).data();
                const td = p.parents("td");
                const nombreColumna = $('#tablaRentabilidad thead tr th').eq(td.index()).html().trim();
                var busq = {
                    tipoReporte: id
                    , obra: comboAC.val()
                    , tipo: comboTipo.val()
                    , lstGrupo: getValoresMultiples('#comboGrupo')
                    , lstModelo: getValoresMultiples('#comboModelo')
                    , lstMaquina: [rowData.noEconomico]
                    , min: inputMesInicial.val()
                    , max: inputMesFinal.val()
                    , cta: 0
                    , scta: 0
                    , tm: []
                }
                let detallesFiltrados;

                switch (td.index()) {
                    case 2: // Ingreso
                        busq.tm = [2, 4];
                        setLstRentabilidadDetalleUtilidad(busq, nombreColumna, p.html(), 1);
                        //detallesFiltrados = rowData.detalles.filter(x => x.tipo_mov == 2 || x.tipo_mov == 4);
                        break;
                    case 3: // Costo
                        busq.tm = [1, 3];
                        setLstRentabilidadDetalleUtilidad(busq, nombreColumna, p.html(), 1);
                        //detallesFiltrados = rowData.detalles.filter(x => x.tipo_mov == 1 || x.tipo_mov == 3);
                        break;
                    default:
                        return;
                }
                //cargarSubCuenta(detallesFiltrados, nombreColumna, p.html());
            });
        }
        
        function cargarDetalles(detalles, nombreColumna, total) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral(`Aviso`, `Ocurrió un error al ver los detalles de este registros.`);
                return;
            }

            const grouped = groupBy(detalles, detalle => detalle.grupoInsumo);

            dtTablaDetalles.clear();
            Array.from(grouped, ([key, value]) => {
                var auxCuenta = value[0].grupoInsumo.split('-');
                const cta = value[0].cta;
                const scta = parseInt(auxCuenta[1]);
                const sscta = parseInt(auxCuenta[2]);
                const id = value[0].grupoInsumo;
                //const fecha = new Date(value.fecha);
                const descripcion = value[0].grupoInsumo_Desc;

                const importe = value.map(x => x.importe).reduce((total, importe) => total + importe);

                const grupo = { cta, scta, sscta, id, descripcion, importe, detalles: value };

                dtTablaDetalles.row.add(grupo);
            });

            dtTablaDetalles.draw();
            //tituloNivelUno = `${nombreColumna} Total - : ${total}`;
            $("#botonNombreNivelUno strong").text($(nombreColumna).text().toUpperCase());
            //inputNombreDetalle.val(tituloNivelUno);

            modalDetalles.modal('show');
        }

        function cargarSubCuenta(detalles, nombreColumna, total) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral(`Aviso`, `Ocurrió un error al ver los detalles de este registros.`);
                return;
            }

            const grouped = groupBy(detalles, detalle => detalle.tipoInsumo);

            dtTablaSctaDetalles.clear();
            Array.from(grouped, ([key, value]) => {
                var auxCuenta = value[0].grupoInsumo.split('-');
                const cta = value[0].cta;
                const scta = parseInt(auxCuenta[1]);
                const id = value[0].tipoInsumo;
                const descripcion = value[0].tipoInsumo_Desc;

                const importe = value.map(x => x.importe).reduce((total, importe) => total + importe);

                const grupo = { cta, scta, id, descripcion, importe, detalles: value };

                dtTablaSctaDetalles.row.add(grupo);
            });

            dtTablaSctaDetalles.draw();
            //tituloNivelCero = `${nombreColumna} Total - : ${total}`;
            $("#botonNombreNivelCero strong").text($(nombreColumna).text().toUpperCase());
            //inputNombreDetalle.val(tituloNivelCero);

            modalDetalles.modal('show');
        }
        function cargarDetScta(detalles, nombreColumna, total) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral(`Aviso`, `Ocurrió un error al ver los detalles de este registros.`);
                return;
            }

            const grouped = groupBy(detalles, detalle => detalle.grupoInsumo);

            dtTablaDetalles.clear();
            Array.from(grouped, ([key, value]) => {
                var auxCuenta = value[0].grupoInsumo.split('-');
                const cta = value[0].cta;
                const scta = parseInt(auxCuenta[1]);
                const sscta = parseInt(auxCuenta[2]);
                const id = value[0].grupoInsumo;
                //const fecha = new Date(value.fecha);
                const descripcion = value[0].grupoInsumo_Desc;

                const importe = value.map(x => x.importe).reduce((total, importe) => total + importe);

                const grupo = { cta, scta, sscta, id, descripcion, importe, detalles: value };

                dtTablaDetalles.row.add(grupo);
            });
            //tituloNivelUno = `${nombreColumna} Total - : ${total}`;
            $("#botonNombreNivelUno strong").text(nombreColumna.toUpperCase());
            //inputNombreDetalle.val(tituloNivelUno);

            dtTablaDetalles.draw();
            divTablaNivelCero.hide(500);
            divTablaNivelUno.show(500);
        }

        function groupBy(list, keyGetter) {
            const map = new Map();
            list.forEach(item => {
                const key = keyGetter(item);
                const collection = map.get(key);
                if (!collection) {
                    map.set(key, [item]);
                } else {
                    collection.push(item);
                }
            });
            return map;
        }

        function initTablaDetalles() {

            dtTablaDetalles = tablaDetalles.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                columns: [
                    //{ data: 'fecha', title: 'Fecha Póliza' },
                    { data: 'cta', title: 'CuentaID', visible: false },
                    { data: 'scta', title: 'SubCuentaID', visible: false },
                    { data: 'sscta', title: 'SubSubCuentaID', visible: false },
                    { data: 'id', title: 'Cuenta' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'importe', title: 'Importe', render: (data, type, row) => getRowHTML(data) }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                order: [[0, 'asc'], [1, 'asc'], [2, 'asc']],
                drawCallback: function () {
                    tablaDetalles.find('p.desplegable').click(function () {

                        const rowData = dtTablaDetalles.row($(this).parents('tr')).data();

                        mostrarSubdetalles(rowData.detalles, rowData.descripcion, rowData.importe);
                        botonNombreNivelDos.show();
                    });
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var api = this.api(), data;
                    var intVal = function ( i ) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '')*1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    total = api.column(5).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    $(api.column(4).footer()).html('TOTAL');
                    $(api.column(5).footer()).html('$' + parseFloat(total, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                }
            });
        }

        function initTablaSctaDetalles() {

            dtTablaSctaDetalles = tablaSctaDetalles.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                autoWidth: true,
                columns: [
                    { data: 'cta', title: 'CuentaID', visible: false },
                    { data: 'scta', title: 'SubCuentaID', visible: false },
                    { data: 'id', title: 'Cuenta' },
                    { data: 'descripcion', title: 'Descripcion' },
                    { data: 'importe', title: 'Importe', render: (data, type, row) => getRowHTML(data) }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                order: [[0, 'asc'], [1, 'asc']],
                drawCallback: function () {
                    tablaSctaDetalles.find('p.desplegable').click(function () {

                        const rowData = dtTablaSctaDetalles.row($(this).parents('tr')).data();

                        cargarDetScta(rowData.detalles, rowData.descripcion, $(this).html());
                        botonNombreNivelUno.show();
                        //mostrarSubdetalles(rowData.detalles, rowData.descripcion, rowData.importe);
                    });
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var api = this.api(), data;
                    var intVal = function ( i ) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '')*1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    total = api.column(4).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    $(api.column(3).footer()).html('TOTAL');
                    $(api.column(4).footer()).html('$' + parseFloat(total, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                }
            });
        }

        function mostrarSubdetalles(subdetalles, descripcion, total) {

            subdetalles = subdetalles.map(x => {
                return {
                    fecha: moment(x.fecha).toDate().toLocaleDateString().Capitalize(),
                    descripcion: x.insumo_Desc,
                    importe: x.importe
                };
            });

            //botonSubdetalle.html(`${descripcion} - Total: ${maskNumero(total)}`);
            //tituloNivelDos = `${descripcion} Total - : ${maskNumero(total)}`;
            $("#botonNombreNivelDos strong").text(descripcion.toUpperCase());
            //inputNombreDetalle.val(tituloNivelDos);

            dtTablaSubdetalles.clear().rows.add(subdetalles).draw();

            divTablaNivelUno.hide(500);

            divTablaNivelDos.show(500);
            //botonSubdetalle.show(500);
        }

        function initTablaSubdetalles() {

            dtTablaSubdetalles = tablaSubdetalles.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                columns: [
                    { data: 'fecha', title: 'Fecha Póliza' },
                    { data: 'descripcion', title: 'Descripcion' },
                    { data: 'importe', title: 'Importe', render: (data, type, row) => `<p>${maskNumero(data)}</p>` }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                footerCallback: function ( row, data, start, end, display ) {                    
                    var api = this.api(), data;
                    var intVal = function ( i ) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '')*1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    total = api.column(2).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    $(api.column(1).footer()).html('TOTAL');
                    $(api.column(2).footer()).html('$' + parseFloat(total, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                }
            });
        }

        //René
        function distinct(array){
            return $.grep(array,function(el,index){
                return index == $.inArray(el,array);
            });
        }



    }

    $(() => Maquinaria.Rentabilidad.Rentabilidad = new Rentabilidad())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();