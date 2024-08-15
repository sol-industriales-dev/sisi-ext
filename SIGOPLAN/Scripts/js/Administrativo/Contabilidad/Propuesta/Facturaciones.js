(() => {
    $.namespace('Administrativo.Contabilidad.Propuesta.Facturaciones');
    Facturaciones = function () {
        const lblAuth = $('#lblAuth');
        const botonAuth = $('#botonAuth');
        const inputFecha = $('#inputFecha');
        const botonExportar = $('#botonExportar');
        const tablaFacturaciones = $('#tablaFacturaciones');
        let dtTablaFacturaciones;
        let startDate;
        let endDate;
        function init() {
            initDatepicker();
            cargarFacturas();
            botonExportar.hide();
            botonAuth.hide();
            agregarListeners();
        }
        function agregarListeners() {
            botonAuth.click(AlertaAuth);
            botonExportar.click((() => exportUrlToFile('/Administrativo/Propuesta/exportEstimacionesResumen')));
        }
        function AlertaAuth() {
            AlertaAceptarRechazar("Aviso", `Se autorizará la el resumen de estimaciones al ${inputFecha.val()}. ¿Desea continuar?`, ejectAuthResuemEstimacion, null)
                .then(boton => {
                    ejectAuthResuemEstimacion();
                });
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
            setSemanaEstimacion();
        }

        function selectCurrentWeek() {
            window.setTimeout(function () {
                inputFecha.find('.ui-datepicker-current-day a').addClass('ui-state-active');
            }, 1);
        }

        function setSemanaEstimacion() {
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
                        initTablaFacturas(response.facturas);
                        setAuth(response);
                    } else {
                        AlertaGeneral('Error', 'No se pudo completar la operación.');
                        botonExportar.hide(1000);
                        botonAuth.hide(1000);
                        lblAuth.text("");
                    }
                }, () => AlertaGeneral('Error', 'No se pudo completar la operación.'));
        }
        function setAuth({ auth, stAuth }) {
            lblAuth.text(`Estado: ${stAuth}`);
            botonAuth.show(1000);
            if (auth.stAuth === 1) {
                botonAuth.addClass("esAutorizada");
                botonAuth.removeClass("esCancelada");
                botonAuth.html(`<i class="fas fa-ban"></i></i> Cancelar`);
            } else {
                botonAuth.addClass("esCancelada");
                botonAuth.removeClass("esAutorizada");
                botonAuth.html(`<i class="fas fa-user"></i> Autorizar`);
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
                    { data: 'estimacion', sortable: false, createdCell: (td, data) => setTdDinero(td, data) },
                    { data: 'anticipo', sortable: false, createdCell: (td, data) => setTdDinero(td, data) },
                    { data: 'vencido', sortable: false, createdCell: (td, data) => setTdDinero(td, data) },
                    { data: 'no', sortable: false },
                    { data: 'pronostico', sortable: false, createdCell: (td, data) => setTdDinero(td, data) },
                    { data: 'cobrado', sortable: false, createdCell: (td, data) => setTdDinero(td, data) },
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