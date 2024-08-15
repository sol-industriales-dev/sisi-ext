(() => {
    $.namespace('Administrativo.Contabilidad.Propuesta.PolizaNomina');
    PolizaNomina = function () {
        // Variables
        let dtPolizas, valoresCombo, listaPolizas;
        const fechaInicial = $('#fechaInicial');
        const fechaFinal = $('#fechaFinal');
        const botonBuscar = $('#botonBuscar');
        const tablaPolizas = $('#tablaPolizas');
        const botonGuardar = $('#botonGuardar');
        const CargarTiposNomina = originURL('/Administrativo/Propuesta/CargarTiposNomina');
        const guardarNominaPoliza = originURL('/Administrativo/Propuesta/guardarNominaPoliza');
        const getLstPolizasNominas = originURL('/Administrativo/Propuesta/getLstPolizasNominas');
        const setNominaPolizaSession = originURL('/Administrativo/Propuesta/setNominaPolizaSession');
        function init() {
            initDates();
            agregarListeners();
            initTablaPolizas();
            CargarCboTiposNomina();
            botonGuardar.hide();
        }
        function initTablaPolizas() {
            dtPolizas = tablaPolizas.DataTable({
                info: false,
                paging: false,
                sortable: false,
                searching: false,
                language: dtDicEsp,
                iDisplayLength: -1,
                drawCallback: function (settings) {
                    var api = this.api(),
                        rows = api.rows({ page: 'current' }).nodes(),
                        last = null;
                    api.column({ page: 'current' }).data().each(function (group, i, dtable) {
                        const data = dtable.data()[i];
                        if (last !== data.poliza) {
                            $(rows).eq(i).before(`<tr class="${getClaseEmpresa(data.tipoCuentaNombre.toUpperCase())}"><td>${data.fechapol}</td><td>${data.concepto}</td><td>${data.tipoCuentaNombre}</td><td>${data.poliza}</td><td colspan = "2">${getValoresCombo(data)}</td></tr>`);
                            last = data.poliza;
                        }
                    });
                },
                columns: [
                    { data: 'fechapol', visible: false },
                    { data: 'concepto', visible: false },
                    { data: 'poliza', visible: false },
                    { data: 'tipoCuentaNombre', visible: false },
                    { data: 'cc', sortable: false, render: (data, type, row) => `<p poliza=${row.poliza} class="text-center">${(row.cc)}</p>` },
                    { data: 'ccDesc', sortable: false, render: (data, type, row) => `<p poliza=${row.poliza} class="text-center">${(row.ccDesc)}</p>` },
                    { data: 'cargo', sortable: false, render: (data, type, row) => { return `<p poliza=${row.poliza} class="text-center">${maskNumero(row.cargo - row.abono)}</p>` } },
                    { data: 'iva', sortable: false, render: (data, type, row) => `<p poliza=${row.poliza} class="text-center">${maskNumero(data)}</p>` },
                    { data: 'retencion', sortable: false, render: (data, type, row) => `<p poliza=${row.poliza} class="text-center">${maskNumero(data)}</p>` },
                    { data: 'iva', sortable: false, render: (data, type, row) => { return `<p poliza=${row.poliza} class="text-center">${maskNumero(row.cargo - row.abono - row.retencion + data)}</p>` } }
                ]
            });
        }

        function getClaseEmpresa(tipoCuentaNombre) {
            let nombreClase;
            switch (tipoCuentaNombre) {
                case 'ARRENDADORA':
                    nombreClase = 'ARRENDADORA';
                    break;
                case 'SERVICIOS ADMINISTRATIVOS COMPLEMENTARIA':
                    nombreClase = 'CONSTRUPLAN';
                    break;
                case 'CONSTRUCTORA RAVELIO':
                    nombreClase = 'RAVELIO';
                    break;
                case 'SONMONT':
                    nombreClase = 'SONMONT';
                    break;
                case 'REGFORTE':
                    nombreClase = 'REGFORTE';
                    break;
                case 'TRONSET':
                    nombreClase = 'TRONSET';
                    break;
                default:
                    nombreClase = 'CONSTRUPLAN';
                    break;
            }
            return nombreClase;
        }

        function initDates() {
            let ultimoDia = new Date(),
                iniciaDia = new Date(ultimoDia.getFullYear(), ultimoDia.getMonth(), 1);
            fechaInicial.datepicker({ firstDay: 0 }).datepicker("setDate", iniciaDia);
            fechaFinal.datepicker({ firstDay: 0 }).datepicker("setDate", ultimoDia);
        }

        function agregarListeners() {
            botonBuscar.click(() => {
                const fechaInicio = fechaInicial.val();
                const fechaFin = fechaFinal.val();
                if (fechaInicio && fechaFin) {
                    cargarPolizas(fechaInicio, fechaFin);
                } else { AlertaGeneral("Aviso", "Debe seleccionar una fecha inicial y una fecha final válida."); }
            });
            botonGuardar.click(guardarPolizas);
        }

        function cargarPolizas(fecha_inicial, fecha_final) {
            dtPolizas.clear().draw();
            botonGuardar.hide();
            axios.post(getLstPolizasNominas, { fecha_inicial, fecha_final })
                .then(response => {
                    if (response.data.success) {
                        dtPolizas.rows.add(response.data.polizas).draw();
                        listaPolizas = response.data.polizas;
                        botonGuardar.show(1000);
                    } else {
                        AlertaGeneral("Aviso", "No se encontraron registros.");
                        listaPolizas = null;
                    }
                }).catch(() => AlertaGeneral("Aviso", "Error al consultar la información."));
        }

        function getValoresCombo({ poliza, tipoNomina }) {
            const container = $('<div></div>');
            const select = $(`<select poliza=${poliza} class="form-control"></select>`);
            valoresCombo.forEach(tipoNomina => select.append(`<option value='${tipoNomina.Value}'>${tipoNomina.Text}</option>`));
            select.val(tipoNomina);
            select.find(`option[value="${tipoNomina}"]`).attr("selected", true);
            container.append(select);
            return container.html();
        }

        function CargarCboTiposNomina() {
            axios.post(CargarTiposNomina)
                .then(response => valoresCombo = response.data)
                .catch(() => valoresCombo = "Error");
        }

        function guardarPolizas() {
            const listaNominaPoliza = [];
            $('#tablaPolizas select').toArray().forEach(select => {
                let numPoliza = +$(select).attr('poliza')
                    , tipoNomina = +(select.value);
                if (tipoNomina > 0) {
                    listaPolizas.filter(x => x.poliza === numPoliza).forEach(x => {
                        listaNominaPoliza.push({
                            year: x.year,
                            mes: x.mes,
                            poliza: x.poliza,
                            fecha: x.fechapol,
                            tipoNomina: tipoNomina,
                            tipoCuenta: x.tipoCuenta,
                            retencion: x.retencion,
                            cc: x.cc,
                            cargo: x.cargo.toFixed(4),
                            abono: x.abono.toFixed(4),
                            iva: x.iva,
                        });
                    });
                }
            });
            $.LoadInMemoryThenSave(setNominaPolizaSession, guardarNominaPoliza, listaNominaPoliza, null, 70, thenGuardarNominaPoliza);
        }
        function thenGuardarNominaPoliza() {
            AlertaGeneral("Aviso", "Cuentas conciliadas con éxito.");
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.Contabilidad.Propuesta.PolizaNomina = new PolizaNomina();
    });
})();