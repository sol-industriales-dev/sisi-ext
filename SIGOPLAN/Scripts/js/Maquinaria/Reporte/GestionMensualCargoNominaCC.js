(() => {
    $.namespace('maquinaria.reporte.gestionMensualCargoNominaCC');
    gestionMensualCargoNominaCC = function () {

        //Declaración de variables
        const cboCentroCostos = $('#cboCentroCostos');
        const cboMes = $('#cboMes');
        const cboAño = $('#cboAño');
        const cboStatus = $('#cboStatus');
        const btnBuscar = $('#btnBuscar');
        const btnGuardar = $('#btnGuardar');
        const tablaNominaMensual = $('#tablaNominaMensual');
        const report = $('#report');
        let dtTablaNomina;

        const columnasTabla = [
            {
                data: 'proyecto', title: "Proyecto",
                render: (data, type, row) => `<p class="areaCuenta" areaCuentaID="${row.id}">${data}</p>`
            },
            {
                data: 'horasHombreTotales', title: "Horas Hombre Totales",
                render: data => `<p class="hht">${data}</p>`
            },
            {
                sortable: false, title: "IMSS", render: (data, type, row) => {
                    return `<input value="${maskNumero(row.nominaIMSS)}" type="text" 
                            class="form-control text-center inputDinero inputIMSS" ${row.nominaIMSS > 0 ? 'disabled' : ''}>`;
                }
            },
            {
                sortable: false, title: "Infonavit", render: (data, type, row) => {
                    return `<input value="${maskNumero(row.nominaInfonavit)}" type="text" 
                            class="form-control text-center inputDinero inputInfonavit" ${row.nominaInfonavit > 0 ? 'disabled' : ''}>
                            `;
                }
            },
            {
                sortable: false, title: "ISN", render: (data, type, row) => {
                    return `<input value="${maskNumero(row.ISN)}" type="text" 
                            class="form-control text-center inputDinero inputISN" ${row.ISN > 0 ? 'disabled' : ''}>
                            `;
                }
            },
            {
                sortable: false, title: "ISR", render: (data, type, row) => {
                    return `<input value="${maskNumero(row.ISR)}" type="text" 
                            class="form-control text-center inputDinero inputISR" ${row.ISR > 0 ? 'disabled' : ''}>
                            `;
                }
            },
            {
                data: 'estatus', title: "Estado",
            },
            {
                sortable: false, title: "Reporte PDF",
                render: (data, type, row) => {
                    if ((row && row.nominaIMSS > 0) || (row && row.nominaInfonavit > 0)) {
                        return `<button class="reporte btn btn-primary glyphicon glyphicon-print" type="button" value="${row.nominaID}" style="margin-right: 5px;"></button>`;
                    } else {
                        return '';
                    }
                }
            }
        ];

        function init() {
            llenarCombos();
            agregarListeners();
            seleccionarMesActual();
            seleccionarAñoActual();
        }

        function llenarCombos() {
            cboCentroCostos.fillCombo('/RepCargoNominaCCArrendadora/LlenarComboAC', null, false, "Todos");
            cboMes.fillCombo('/RepCargoNominaCCArrendadora/FillComboMes', null, false);
            cboAño.fillCombo('/RepCargoNominaCCArrendadora/FillComboAño', null, false);
        }

        function agregarListeners() {
            btnBuscar.click(llenarTablaNomina);
            btnGuardar.click(guardarNominaMensual);
        }

        function seleccionarMesActual() {
            cboMes.val(new Date().getMonth() + 1);
        }

        function seleccionarAñoActual() {
            cboAño.val(new Date().getFullYear());
        }

        function llenarTablaNomina() {

            const filtros = obtenerFiltrosBusqueda();

            if (filtros == null) {
                return;
            }

            if (dtTablaNomina != null) {
                dtTablaNomina.destroy();
            }

            dtTablaNomina = tablaNominaMensual.DataTable({
                ajax: {
                    url: '/RepCargoNominaCCArrendadora/ObtenerNominaMensualCC',
                    type: 'POST',
                    dataSrc: 'data',
                    data: data => {
                        data.areaCuentaArray = filtros.areaCuentaArray,
                            data.mes = filtros.mes,
                            data.año = filtros.año,
                            data.estatus = filtros.estatus
                    }
                },
                language: dtDicEsp,
                rowId: 'id',
                scrollX: "100%",
                scrollCollapse: true,
                destroy: true,
                paging: false,
                order: [0, 'asc'],
                initComplete: () => {
                    tablaNominaMensual.on('change', '.inputDinero', e => e.currentTarget.value = maskNumero(unmaskNumero(e.currentTarget.value)));

                    tablaNominaMensual.on('click', '.reporte', e => {
                        var rowData = dtTablaNomina.row($(e.currentTarget).closest('tr')).data();
                        verReporte(rowData.nominaID);
                    });
                },
                columns: columnasTabla,
                columnDefs: [
                    { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 5, 6, 7] }
                ]
            });
        }

        function obtenerFiltrosBusqueda() {
            let areaCuentaArray = [];

            // Si seleccionó 'Todos', se agregan todas las ac al array
            if (cboCentroCostos.val() === 'Todos') {
                areaCuentaArray = $('#cboCentroCostos option')
                    .toArray()
                    .map(ac => $(ac).val())
                    .filter(ac => (ac !== 'Todos') && (ac !== '0'));
            } else {
                areaCuentaArray.push(cboCentroCostos.val());
            }

            const mes = cboMes.val();
            const año = cboAño.val();
            const estatus = cboStatus.val();

            if (mes === "") {
                AlertaGeneral("Aviso", "Debe seleccionar un mes.");
                return null;
            } else if (año === "") {
                AlertaGeneral("Aviso", "Debe seleccionar un año.");
                return null;
            }

            return {
                areaCuentaArray,
                mes: parseInt(mes),
                año: parseInt(año),
                estatus
            }
        }

        function guardarNominaMensual() {

            if ($('#tablaNominaMensual tr .inputDinero').toArray().length === 0) {
                AlertaGeneral("Aviso", "No hay cambios que guardar.");
                return;
            }

            const nominasProyectos = obtenerNominasProyectos();
            const mes = cboMes.val();
            const año = cboAño.val();

            $.post('/RepCargoNominaCCArrendadora/GuardarNominaMensualCC', { nominasProyectos, mes, año })
                .then(guardadoCompleto, guardadoFallido)
                .always(() => {
                    if (dtTablaNomina != null) {
                        dtTablaNomina.clear().draw();
                    }
                });
        }

        function guardadoCompleto(response) {
            if (response.success) {
                AlertaGeneral("Aviso", "Los cambios han sido guardados correctamente.");
            } else {
                guardadoFallido();
            }
        }

        function guardadoFallido() {
            AlertaGeneral("Aviso", "No se pudo completar la operación.");
        }

        function obtenerNominasProyectos() {

            const arrayNominasProyectos = [];

            $('#tablaNominaMensual tr .inputIMSS').toArray().forEach(x => {

                const nominaIMSS = unmaskNumero($(x).val());
                const nominaInfonavit = unmaskNumero($(x).parent().parent().find('.inputInfonavit').val());
                const ISN = unmaskNumero($(x).parent().parent().find('.inputISN').val());
                const ISR = unmaskNumero($(x).parent().parent().find('.inputISR').val());
                const id = $(x).parent().parent().find('.areaCuenta').attr('areaCuentaID');
                const horasHombreTotales = $(x).parent().parent().find('.hht').text();

                arrayNominasProyectos.push({
                    nominaIMSS,
                    nominaInfonavit,
                    ISN,
                    ISR,
                    id,
                    horasHombreTotales
                });
            });

            return arrayNominasProyectos;
        }

        function verReporte(nominaMensualID) {
            $.blockUI({ message: 'Esta operación puede tardar varios minutos, espere por favor...' });
            report.attr("src", `/Reportes/Vista.aspx?idReporte=103&nominaMensualID=${nominaMensualID}`);
            document.getElementById('report').onload = () => {
                $.unblockUI();
                openCRModal();
            };
        }

        init();
    }
    $(document).ready(() => maquinaria.reporte.gestionMensualCargoNominaCC = new gestionMensualCargoNominaCC())
        .ajaxStart(() => $.blockUI({ message: 'Esta operación puede tardar varios minutos, espere por favor...' }))
        .ajaxStop(() => $.unblockUI());
})();