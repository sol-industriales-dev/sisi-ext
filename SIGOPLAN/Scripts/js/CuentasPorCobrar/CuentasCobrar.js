(() => {
    $.namespace('Administrativo.Contabilidad.Propuesta.Facturaciones');
    Facturaciones = function () {
        const lblAuth = $('#lblAuth');
        const botonAuth = $('#botonAuth');
        const inputFecha = $('#inputFecha');
        const botonExportar = $('#botonExportar');
        const tablaFacturaciones = $('#tablaFacturaciones');
        const textBtnAutorizar = $('#textBtnAutorizar');
        const btnCancelar = $('#btnCancelar');
        const inputFechaTemp = $('#inputFechaTemp');

        let dtTablaFacturaciones;
        let startDate;
        let endDate;
        let facturasSave = [];

        function init() {
            initDatepicker();
            cargarFacturas();
            botonExportar.hide();
            // botonAuth.hide();
            agregarListeners();
        }
        function agregarListeners() {
            botonAuth.click(AlertaAuth);
            botonExportar.click((() => exportUrlToFile('/Administrativo/Propuesta/exportEstimacionesResumen')));

            btnCancelar.on("click", function () {
                Alert2AccionConfirmar('Aviso', `Se cancelara la el resumen de estimaciones al ${inputFecha.val()}. ¿Desea continuar?`, 'Confirmar', 'Cancelar', () => fncCancelarCXC());

            });
        }
        function AlertaAuth() {
            // AlertaAceptarRechazar("Aviso", `Se autorizará la el resumen de estimaciones al ${inputFecha.val()}. ¿Desea continuar?`, fncGuardarCXC(), null);
            Alert2AccionConfirmar('Aviso', `Se autorizará la el resumen de estimaciones al ${inputFecha.val()}. ¿Desea continuar?`, 'Confirmar', 'Cancelar', () => fncGuardarCXC());
        }

        async function ejectAuthResuemEstimacion() {
            try {
                let authResumenEstimacion = new URL(window.location.origin + '/Administrativo/Propuesta/authResumenEstimacion');
                response = await ejectFetchJson(authResumenEstimacion, { fecha: inputFecha.val() });
                if (response.success) {
                    AlertaGeneral("Aviso", `Autorización realizada con éxito.`);
                }
                else { AlertaGeneral("Aviso", "No se logró autorizar correctamente. Intentelo más tarde."); }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
        }
        function initDatepicker() {
            inputFecha.datepicker({
                firstDay: 1,
                onSelect: function (dateText, inst) {
                    cargarFacturas();
                    setSemanaEstimacion();
                },
                beforeShowDay: function (date) {
                    var cssClass = '';
                    if (date >= startDate && date <= endDate)
                        cssClass = 'ui-datepicker-current-day';
                    return [true, cssClass];
                },
                onChangeMonthYear: function (year, month, inst) {
                    selectCurrentWeek();
                },
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 9999);
                    }, 0);
                }
            }).datepicker().datepicker("setDate", new Date());

            inputFechaTemp.datepicker().datepicker("setDate", new Date());

            setSemanaEstimacion();
        }

        function selectCurrentWeek() {
            window.setTimeout(function () {
                inputFecha.find('.ui-datepicker-current-day a').addClass('ui-state-active');
            }, 1);
        }

        function setSemanaEstimacion() {
            let tempDate = inputFecha.datepicker('getDate');

            inputFechaTemp.datepicker("setDate", tempDate);

            let date = inputFecha.datepicker('getDate'),
                prevDom = date.getDate() - (date.getDay() + 7) % 7,
                startDate = new Date(date.getFullYear(), date.getMonth(), prevDom),
                endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 7),
                diaNombre = endDate.toLocaleDateString("es-MX", { weekday: 'long' }).toUpperCase(),
                diaNumero = endDate.getDate(),
                mesNombre = endDate.toLocaleDateString("es-MX", { month: 'long' }).toUpperCase(),
                anio = endDate.getFullYear();

            inputFecha.val(`${diaNombre}, ${diaNumero} DE ${mesNombre} DE ${anio}`);

            selectCurrentWeek();
        }

        function getFechas() {
            let date = inputFecha.datepicker('getDate'),
                prevDom = date.getDate() - (date.getDay() + 7) % 7,
                startDate = new Date(date.getFullYear(), date.getMonth(), prevDom),
                endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 7);


            // console.log(inputFecha.datepicker('getDate'));
            // console.log(startDate.toLocaleDateString());
            // console.log(endDate.toLocaleDateString());

            return {
                min: startDate.toLocaleDateString(),
                max: endDate.toLocaleDateString()
            };
        }

        function getFechasTemp() {
            let date = inputFechaTemp.datepicker('getDate'),
                prevDom = date.getDate() - (date.getDay() + 7) % 7,
                startDate = new Date(date.getFullYear(), date.getMonth(), prevDom),
                endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 7);


            // console.log(inputFecha.datepicker('getDate'));
            // console.log(startDate.toLocaleDateString());
            // console.log(endDate.toLocaleDateString());

            return {
                min: startDate.toLocaleDateString(),
                max: endDate.toLocaleDateString()
            };
        }

        function cargarFacturas() {
            const fechas = getFechas();
            const fechaInicial = fechas.min;
            const fechaFinal = fechas.max;
            $.post('/Administrativo/Propuesta/GetAnaliticoClientesCXC', { fechaInicial, fechaFinal })
                .then(response => {
                    if (response.success) {
                        botonExportar.show(1000);
                        facturasSave = response.lstFacturas;
                        console.log(facturasSave);
                        // console.log(facturasSave);
                        initTablaFacturas(response.facturas);
                        setAuth(response);
                    } else {
                        AlertaGeneral('Error', 'No se pudo completar la operación.');
                        botonExportar.hide(1000);
                        // botonAuth.hide(1000);
                        lblAuth.text("");
                    }
                }, () => AlertaGeneral('Error', 'No se pudo completar la operación.'));
        }

        function fncGuardarCXC() {
            const fechas = getFechasTemp();

            for (const item of facturasSave) {
                let momFecha = moment(item.fecha);
                let momFechavenc = moment(item.fechavenc);
                let momFechaflujo = moment(item.fechavenc);

                let dayOW = momFechaflujo.day();
                while (dayOW != 3) {
                    if (dayOW > 3) {
                        momFechaflujo = momFechaflujo.add(-1, 'days');
                        dayOW = momFechaflujo.day();
                    } else {
                        momFechaflujo = momFechaflujo.add(1, 'days');
                        dayOW = momFechaflujo.day();
                    }
                }

                item.fecha = momFecha._d;
                item.fechavenc = momFechavenc._d;
                item.fechaFlujo = momFechaflujo._d;

            }

            axios.post("GuardarCXC", { lstFacturas: facturasSave, fechaInicial: fechas.min, fechaFinal: fechas.max }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    Alert2Exito("Autorizado con exito");

                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCancelarCXC() {
            const fechas = getFechasTemp();
            axios.post("CancelarCXC", { fechaInicial: fechas.min }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    Alert2Exito("Cancelado con exito");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function setAuth({ auth, stAuth }) {
            lblAuth.text(`Estado: ${stAuth}`);
            // botonAuth.show(1000);
            if (auth) {
                botonAuth.hide();
                btnCancelar.show();
                // botonAuth.addClass("esAutorizada");
                // botonAuth.removeClass("esCancelada");
                // textBtnAutorizar.html(`<i class="fas fa-ban"></i></i> Cancelar`);
            } else {
                botonAuth.show();
                btnCancelar.hide();
                botonAuth.addClass("esCancelada");
                botonAuth.removeClass("esAutorizada");
                textBtnAutorizar.html(`<i class="fas fa-user"></i> Autorizar`);
            }

        }
        function initTablaFacturas(data) {
            if (dtTablaFacturaciones != null) {
                dtTablaFacturaciones.destroy();
            }
            dtTablaFacturaciones = tablaFacturaciones.DataTable({
                info: false,
                paging: false,
                sortable: false,
                searching: false,
                destroy: true,
                order: [],
                language: dtDicEsp,
                iDisplayLength: -1,
                data,
                columns: [
                    { data: 'no', sortable: false },
                    { data: 'descripcion', sortable: false },
                    { data: 'vencido', sortable: false, createdCell: (td, data) => setTdDinero(td, data) },
                    { data: 'pronostico', sortable: false, createdCell: (td, data) => setTdDinero(td, data) },
                ],
                columnDefs: [
                    { width: '25%', targets: 2 },
                    { width: '25%', targets: 3 }

                ],
                createdRow: (tr, data) => $(tr).addClass(data.clase),
            });
        }
        function setTdDinero(td, dinero) {
            return $(td).html(maskNumero(dinero));
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.Contabilidad.Propuesta.Facturaciones = new Facturaciones();
    }).ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop(() => $.unblockUI());
})();