(() => {
    $.namespace('Administrativo.Contabilidad.Propuesta.NominaQuincenalSemanal');
    NominaQuincenalSemanal = function () {
        let lstCtaEmpresas = [], lstDiv = [], dtTablaNominas, dtTablaOtros, listaNominas;
        const tablaOtros = $('#tblNomOtros');
        const comboNomina = $('#comboNomina');
        const botonGuardar = $('#botonGuardar');
        const tablaNominas = $('#tablaNominas');
        const GetNominasOtros = originURL('/Administrativo/Propuesta/GetNominasOtros');
        const getCbotPeriodoNomina = originURL('/Administrativo/Propuesta/getCbotPeriodoNomina');
        const getCbotipoCuentaNomina = originURL('/Administrativo/Propuesta/getCbotipoCuentaNomina');
        const GetNominasQuincenalesSemanales = originURL('/Administrativo/Propuesta/GetNominasQuincenalesSemanales');
        function init() {
            setLstCuentas();
            llenarCombos();
            agregarListeners();
            tablaOtros.addClass("hidden");
        }
        function setLstCuentas() {
            axios.get(getCbotipoCuentaNomina)
                .then(response => {
                    let { success, items, lstDivCC } = response.data;
                    if (success) {
                        lstCtaEmpresas = items;
                        lstDiv = lstDivCC;
                    }
                }).catch(o_O => AlertaGeneral(o_O.message));
        }
        function llenarCombos() {
            comboNomina.fillComboGroup(getCbotPeriodoNomina, null, false, undefined, () => {
                comboNomina.prop("selectedIndex", 1);
                cargarNomina();
            });
        }
        function agregarListeners() {
            comboNomina.change(e => {
                if ($(e.currentTarget).val() !== '--Seleccione periodo--') {
                    cargarNomina();
                    botonGuardar.show(1000);
                } else {
                    listaNominas = null;
                    botonGuardar.hide(1000);
                }
            });
            botonGuardar.click(guardarNominaResumen);
        }
        function cargarNomina() {
            const valores = comboNomina.val().split('-');
            const fechaInicio = valores[0];
            const fechaFin = valores[1];
            const tipoNomina = valores[2];
            const verOtros = valores[3] === 'OTROS';
            const url = verOtros ? GetNominasOtros : GetNominasQuincenalesSemanales;
            axios.post(url, { fechaInicio, fechaFin, tipoNomina })
                .then(response => {
                    if (response.data.success) {
                        if (verOtros) {
                            tablaNominas.addClass("hidden");
                            tablaOtros.removeClass('hidden');
                            initTablaOtros(listaNominas = response.data.listaNominas);
                        } else {
                            tablaNominas.removeClass("hidden");
                            tablaOtros.addClass("hidden");
                            initTablaNominas(listaNominas = response.data.listaNominas);
                        }
                    } else {
                        AlertaGeneral('Error', 'No se pudo completar la operación.');
                        listaNominas = null;
                    }
                }).catch(o_O => AlertaGeneral("Aviso", o_O.message));
        }
        function initTablaNominas(data) {
            if (dtTablaNominas != null) {
                dtTablaNominas.destroy();
            }
            dtTablaNominas = tablaNominas.DataTable({
                info: false,
                paging: false,
                sortable: false,
                searching: false,
                destroy: true,
                language: dtDicEsp,
                iDisplayLength: -1,
                data,
                order: [],
                columns: [
                    {
                        data: 'cc', sortable: false, render: (data, type, row) =>
                            `<p class="text-center ${Contiene(['ZZ', '0-1', '0-2'], row.cc) ? 'hidden' : ''}">${row.cc}</p>`
                    },
                    {
                        data: 'descripcion', sortable: false, render: (data, type, row) =>
                            `<p class="text-center">${row.descripcion}</p>`
                    },
                    {
                        data: 'nomina', sortable: false, render: (data, type, row) => {
                            switch (true) {
                                case Contiene(['encabezadoTabla'], row.clase): return '<p class="text-center">NÓMINA</p>';
                                case Contiene(['encabezadoEmpresa'], row.clase):
                                case Contiene(['vacio'], row.clase):
                                    return "";
                                default: return `<p class="text-center">${maskNumero(data)}</p>`;
                            }
                        }
                    },
                    {
                        data: 'iva', sortable: false, render: (data, type, row) => {
                            switch (true) {
                                case Contiene(['encabezadoTabla'], row.clase): return '<p class="text-center">IVA 16%</p>';
                                case Contiene(['encabezadoEmpresa'], row.clase):
                                case Contiene(['vacio'], row.clase):
                                    return "";
                                default: return `<p class="text-center">${maskNumero(data)}</p>`;
                            }
                        }
                    },
                    {
                        data: 'retencion', sortable: false, render: (data, type, row) => {
                            switch (true) {
                                case Contiene(['encabezadoTabla'], row.clase): return '<p class="text-center">RETENCIÓN</p>';
                                case Contiene(['encabezadoEmpresa'], row.clase):
                                case Contiene(['vacio'], row.clase):
                                    return "";
                                default: return `<p class="text-center">${maskNumero(data)}</p>`;
                            }
                        }
                    },
                    {
                        data: 'total', sortable: false, render: (data, type, row) => {
                            switch (true) {
                                case Contiene(['encabezadoTabla'], row.clase): return '<p class="text-center">TOTAL</p>';
                                case Contiene(['encabezadoEmpresa'], row.clase):
                                case Contiene(['vacio'], row.clase):
                                    return "";
                                default: return `<p class="text-center">${maskNumero(data)}</p>`;
                            }
                        }
                    },
                    {
                        data: 'noEmpleado', sortable: false, render: (data, type, row) => {
                            switch (true) {
                                case Contiene(['encabezadoTabla'], row.clase): return '<p class="text-center">No. EMPLEADO</p>';
                                case Contiene(['encabezadoEmpresa'], row.clase):
                                case Contiene(['vacio'], row.clase):
                                    return "";
                                default: return `<input cc="${row.cc}" class="text-center empleado ${row.tipoCuenta} ${row.division}" type="number" ${row.cc.includes('ZZ') ? 'disabled' : ''} value="${row.noEmpleado}" style="width:100%">`;
                            }
                        }
                    },
                    {
                        data: 'noPracticante', sortable: false, render: (data, type, row) => {
                            switch (true) {
                                case Contiene(['encabezadoTabla'], row.clase): return '<p class="text-center">No. PRACTICANTE</p>';
                                case Contiene(['encabezadoEmpresa'], row.clase):
                                case Contiene(['vacio'], row.clase):
                                    return "";
                                default: return `<input cc="${row.cc}" class="text-center practicante ${row.tipoCuenta} ${row.division}" type="number" ${row.cc.includes('ZZ') ? 'disabled' : ''} value="${row.noPracticante}" style="width:100%">`;
                            }
                        }
                    }
                ],
                createdRow: (tr, data) => $(tr).addClass(data.clase),
                columnDefs: [
                    { "width": "8%", "targets": [0, 2, 3, 4, 5, 6, 7] },
                    { "width": "40%", "targets": 1 }
                ]
                , initComplete: function (settings, json) {
                    tablaNominas.on('change', 'input', function (event) { setSuma(); });
                }
            });
        }
        function setSuma() {
            let lstInpPersona = ["empleado", "practicante"],
                lstInpCaptura = ["normal", "obraIndividual"],
                lstInpSuma = ["totalCuadro", "totalCuenta", "totalGeneral"];
            lstCtaEmpresas.forEach(cta => {
                let rows = tablaNominas.find(`.${cta.Value}`).closest('tr');
                if (rows.length > 0) {
                    let esCplan = rows[0].className.includes("cplan");
                    lstInpPersona.forEach(inpPersona => {
                        lstDiv.forEach(division => {
                            lstInpCaptura.forEach(inpCaptura => {
                                let acum = rows.filter(`.${inpCaptura}`).find(`.${inpPersona}.${division}`).toArray().reduce((previous, current) => previous += +current.value, 0);
                                if (acum > 0) {
                                    rows.filter(esCplan ? `.totalCuadro` : `.totalCuenta`).find(`.${inpPersona}.${division}`).val(acum);
                                }
                            });
                        });
                        lstInpSuma.filter(clsSuma => esCplan ? true : !clsSuma.includes("totalCuadro")).forEach((inpSuma, iSuma) => {
                            if (!esCplan) { iSuma += 1; }
                            let acum = rows.filter(`.${inpSuma}`).find(`.${inpPersona}`).toArray().reduce((previous, current) => previous += +current.value, 0);
                            if (acum > 0) {
                                rows.filter(`.${lstInpSuma[iSuma + 1]}`).find(`.${inpPersona}`).val(acum);
                            }
                        });
                    });
                }
            });
            lstInpPersona.forEach(inpPersona => {
                let acum = tablaNominas.find(`.totalCuenta .${inpPersona}`).toArray().reduce((previous, current) => previous += +current.value, 0);
                tablaNominas.find(`.totalGeneral .${inpPersona}`).val(acum);
            });
        }
        function initTablaOtros(data) {
            if (dtTablaOtros != null) {
                dtTablaOtros.destroy();
            }
            dtTablaOtros = tablaOtros.DataTable({
                info: false,
                paging: false,
                destroy: true,
                sortable: false,
                searching: false,
                order: [],
                data,
                language: dtDicEsp,
                iDisplayLength: -1,
                drawCallback: function (settings) {
                    var api = this.api(),
                        rows = api.rows({ page: 'current' }).nodes(),
                        head = null, foot = null;
                    api.column({ page: 'current' }).data().each((group, i, dtable) => {
                        const data = dtable.data()[i];
                        let descripcionNomina = data.descripcionNomina;
                        let descripcionCuenta = data.descripcionCuenta;
                        if (head !== `${descripcionNomina}-${descripcionCuenta}`) {
                            $(rows).eq(i).before(`<tr class="text-center encabezadoOtros"><td>C.C.</td><td>OBRA</td><td class="conceptoNomina">${descripcionNomina}</td><td>IVA 16%</td><td>RETENCIÓN</td><td>TOTAL</td>/tr>`);
                            head = `${descripcionNomina}-${descripcionCuenta}`;
                        }
                    });
                },
                columns: [
                    { data: 'tipoNomina', visible: false },
                    { data: 'tipoCuenta', visible: false },
                    {
                        data: 'cc', sortable: false, render: (data, type, row) =>
                            `<p class="text-center ${row.cc.toUpperCase().includes('ZZ') ? 'hidden' : ''}">${row.cc}</p>`
                    },
                    {
                        data: 'descripcion', sortable: false, render: (data, type, row) =>
                            `<p class="text-center">${row.descripcion}</p>`
                    },
                    {
                        data: 'nomina', sortable: false, render: (data, type, row) => {
                            if (row.clase === 'encabezadoTabla' || row.clase === 'vacio') {
                                return '';
                            } else {
                                return `<p class="text-center">${maskNumero(row.nomina)}</p>`
                            }
                        }
                    },
                    {
                        data: 'iva', sortable: false, render: (data, type, row) => {
                            if (row.clase === 'encabezadoTabla' || row.clase === 'vacio') {
                                return '';
                            } else {
                                return `<p class="text-center">${maskNumero(row.iva)}</p>`
                            }
                        }
                    },
                    {
                        data: 'retencion', sortable: false, render: (data, type, row) => {
                            if (row.clase === 'encabezadoTabla' || row.clase === 'vacio') {
                                return '';
                            } else {
                                return `<p class="text-center">${maskNumero(row.iva)}</p>`
                            }
                        }
                    },
                    {
                        data: 'total', sortable: false, render: (data, type, row) => {
                            if (row.clase === 'encabezadoTabla' || row.clase === 'vacio') {
                                return '';
                            } else {
                                return `<p class="text-center">${maskNumero(row.total)}</p>`
                            }
                        }
                    },
                ],
                createdRow: function (tr, data) {
                    $(tr).addClass(data.clase);
                },
            });
        }
        function guardarNominaResumen() {
            const otros = comboNomina.val().split('-')[3] === 'OTROS';
            const listaNominaResumen = getNominasResumen(otros);
            axios.post('/Administrativo/Propuesta/guardarNominaResumen', { listaNominaResumen, otros })
                .then(response => {
                    if (response.data.success) {
                        AlertaGeneral('Éxito', 'Cambios guardados correctamente.');
                    } else {
                        AlertaGeneral('Aviso', 'Ocurrió un error al intentar guardar los cambios.')
                    }
                }).catch(o_O => AlertaGeneral("Aviso", o_O.message));
        }
        function getNominasResumen(otros) {
            let lstClasesAGuardar = [`normal`, `obraIndividual`];
            const valores = comboNomina.val().split('-');
            const fechaInicio = valores[0];
            const fechaFin = valores[1];
            let lst = listaNominas
                .filter(nomina => Contiene(lstClasesAGuardar, nomina.clase))
                .map(nomina => {
                    const noEmpleado = otros ? 0 : $(`#tablaNominas input[type="number"][cc="${nomina.cc}"].empleado`).val();
                    const noPracticante = otros ? 0 : $(`#tablaNominas input[type="number"][cc="${nomina.cc}"].practicante`).val();
                    return {
                        tipoNomina: nomina.tipoNomina,
                        fecha_inicial: fechaInicio,
                        fecha_final: fechaFin,
                        tipoCuenta: nomina.tipoCuenta,
                        cc: nomina.cc,
                        nomina: nomina.nomina,
                        iva: nomina.iva,
                        retencion: nomina.retencion,
                        total: nomina.total,
                        noEmpleado: noEmpleado,
                        noPracticante: noPracticante
                    };
                });
            return lst;
        }
        function Contiene(array, busqueda) {
            return array.some(busqueda.includes.bind(busqueda))
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.Contabilidad.Propuesta.NominaQuincenalSemanal = new NominaQuincenalSemanal();
    })
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop(() => $.unblockUI());
})();