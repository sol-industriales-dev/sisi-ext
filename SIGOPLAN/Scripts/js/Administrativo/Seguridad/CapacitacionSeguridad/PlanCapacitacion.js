(() => {
    $.namespace('PlanCapacitacion.Capacitacion');
    Capacitacion = function () {
        //#region Selectores
        const selectCentroCosto = $('#selectCentroCosto');
        const selectTematica = $('#selectTematica');
        const botonBuscar = $('#botonBuscar');
        const botonImprimir = $('#botonImprimir');
        const divCalendario = $('#divCalendario');
        //#endregion

        let calendar;

        //#region Variables Date
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        const fechaInicioMes = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
        //#endregion

        (function init() {
            selectCentroCosto.select2();
            initCalendario();
            cargarCombos();

            botonBuscar.click(cargarCalendario);
            botonImprimir.click(imprimirCalendario);
        })();

        function cargarCalendario() {
            let cc = selectCentroCosto.val();
            let listaTematicas = getValoresMultiples('#selectTematica');

            if (cc != '') {
                let empresa = selectCentroCosto.find('option:selected').attr('empresa');
                let mesCalendario = calendar.getDate();

                axios.post('CargarCalendarioPlanCapacitacion', { cc, listaTematicas, empresa, mesCalendario })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            initCalendario(response.data.data);
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            } else {
                AlertaGeneral(`Alerta`, `Debe seleccionar un centro de costo.`);
            }
        }

        function initCalendario(eventos) {
            if (calendar != undefined) {
                calendar.destroy();
            }

            calendar = new FullCalendar.Calendar(document.getElementById('divCalendario'), {
                plugins: ['dayGrid', 'moment', 'interaction'],
                locale: 'es',
                titleFormat: 'MMMM YYYY',
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: ''
                },
                defaultDate: fechaActual,
                height: 650,
                showNonCurrentDates: false,
                allDayDefault: true,
                events: eventos
            });

            calendar.render();
        }

        function cargarCombos() {
            axios.get('ObtenerComboCCAmbasEmpresas').then(response => {
                let { success, items, message } = response.data;

                if (success) {
                    selectCentroCosto.append('<option value="">--Seleccione--</option>');
                    items.forEach(x => {
                        let groupOption = `<optgroup label="${x.label}">`;

                        x.options.forEach(y => {
                            groupOption += `<option value="${y.Value}" empresa="${x.label == 'CONSTRUPLAN' ? 1 : x.label == 'ARRENDADORA' ? 2 : 0}">${y.Text}</option>`;
                        });

                        groupOption += `</optgroup>`;

                        selectCentroCosto.append(groupOption);
                    });
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));

            selectTematica.fillCombo('GetTematicaCombo', null, false, 'Todos');
            convertToMultiselect('#selectTematica');
        }

        function imprimirCalendario() {
            var doc = new html2pdf();
            // var doc = new jsPDF('l','mm','A4');
            var divContenido = $('#divCalendario').get(0)
            var opt = {
                margin: [0.5, 0.5, 0.42, 0.5],
                filename: 'calendario.pdf',
                image: { type: 'jpeg', quality: 1 },
                html2canvas: { scale: 1 },
                jsPDF: { unit: 'in', format: 'letter', orientation: 'landscape' }
            };

            $.blockUI({ message: 'Generando imprimible...' });

            // New Promise-based usage:
            doc.set(opt).from(divContenido).save().then(r => {
                $.unblockUI();
            });
        }
    }
    $(document).ready(() => PlanCapacitacion.Capacitacion = new Capacitacion())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();