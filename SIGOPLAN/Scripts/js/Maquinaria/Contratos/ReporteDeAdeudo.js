(() => {
    $.namespace('Maquinaria.DocumentosPorPagar.ReporteDeAdeudo');

    ReporteDeAdeudo = function () {

        // Variables Principales.
        const comboInstituciones = $('#comboInstituciones');
        const comboTipoMoneda = $('#comboTipoMoneda');
        const btnBusqueda = $('#btnBusqueda');
        const btnPrintReporte = $('#btnPrintReporte');
        const tblReporteAdedudo = $('#tblReporteAdedudo');
        const comboAnios = $('#comboAnios');
        const lblAnio = $('#lblAnioAnt');
        const report = $("#report");
        const comboTipoArrendamiento = $("#comboTipoArrendamiento")

        const inputTipoCambio = $("#inputTipoCambio");
        const _ArrayTablaName = ["", "STOD A", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre", "STOD A "];

        let dtTablaReporteAdedudo;
        let dtTblAdeudoDlls;

        const tblAdeudoDlls = $("#tblAdeudoDlls");
        (function init() {
            // Lógica de inicialización.
            fnLoadComboInstituciones();
            fnLlenarAños();
            initTablaAdeudo();
            initTablaAdeudoDLLS();
            btnBusqueda.click(fnBuscar);
            btnPrintReporte.click(fnLoadReporte);
            convertToMultiselect("#comboTipoMoneda");
            inputTipoCambio.change(fnSetTipoCambio);
        })();

        function fnSetTipoCambio() {

            adeudoDLLS = dtTblAdeudoDlls.data().toArray();
            dtTblAdeudoDlls.clear().draw();
            dtTblAdeudoDlls.rows.add(adeudoDLLS);
            dtTblAdeudoDlls.draw();
        }

        function fnLoadReporte() {

            let dtPesos = dtTablaReporteAdedudo.data().toArray();
            let dtDlls = dtTblAdeudoDlls.data().toArray();

            $.blockUI({ message: 'Cargando Información' });
            $.post('/Contratos/setRptAdeudosGeneral/', { rptPesos: dtPesos, rptDlls: dtDlls })
                .then(response => {
                    if (response.Success) {
                        // Operación exitosa.
                        var idReporte = "197";
                        var path = "/Reportes/Vista.aspx?idReporte=" + idReporte;
                        report.attr("src", path);
                        document.getElementById('report').onload = function () {
                            openCRModal();
                            $.unblockUI();
                        };
                        $.unblockUI();
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }).then(r => {

                });

        }

        // Métodos.
        function fnBuscar() {
            if (comboAnios.val().length > 0) {
                if (comboInstituciones.val().length > 0) {
                    $.blockUI({ message: 'Cargando Información' });
                    $.post('/Contratos/getRptAdeudosGeneral', {
                        tipoMoneda: comboTipoMoneda.val(),
                        instituciones: comboInstituciones.val(),
                        anio: comboAnios.val(),
                        tipoArrendamiento: comboTipoArrendamiento.val() == "1" ? true : false
                    }).always($.unblockUI)
                        .then(response => {
                            if (response.success) {
                                // Operación exitosa.
                                dtTablaReporteAdedudo.clear();
                                dtTablaReporteAdedudo.rows.add(response.reporte).draw();
                                dtTblAdeudoDlls.clear();
                                dtTblAdeudoDlls.rows.add(response.adeudoDLLS).draw();

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
                else {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Se deben seleccionar por lo menos una institución`);
                }
            }
            else {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Se deben seleccionar los años`);
            }
        }

        function fnLoadComboInstituciones() {
            comboInstituciones.fillCombo('/Contratos/ObtenerInstituciones', null, false, null);
            convertToMultiselect("#comboInstituciones");
        }

        function fnLlenarAños() {
            var nowY = new Date().getFullYear() + 8,
                options = "";

            for (var Y = nowY; Y >= 2010; Y--) {
                options += `<option value='${Y}'> ${Y}  </option>`;
            }

            comboAnios.append(options);
            convertToMultiselect("#comboAnios");
        }

        function initTablaAdeudo() {
            var groupColumn = 0;
            dtTablaReporteAdedudo = tblReporteAdedudo.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                scrollY: '52vh',
                scrollCollapse: true,
                scrollX: true,
                "bLengthChange": false,
                columns: [
                    { data: 'anio', tittle: 'Anio' },
                    { data: 'descripcionInstitucion', title: 'Institución' },
                    {
                        data: 'anioAnterior', title: 'Inicial', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    {
                        data: 'enero', title: 'Ene ', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    {
                        data: 'febrero', title: 'Feb ', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    {
                        data: 'marzo', title: 'Mar ', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    {
                        data: 'abril', title: 'Abr ', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    {
                        data: 'mayo', title: 'May ', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    {
                        data: 'junio', title: 'Jun ', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    {
                        data: 'julio', title: 'Jul ', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    {
                        data: 'agosto', title: 'Ago ', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    {
                        data: 'septiembre', title: 'Sep ', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    {
                        data: 'octubre', title: 'Oct ', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    {
                        data: 'noviembre', title: 'Nov ', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    {
                        data: 'diciembre', title: 'Dic ', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                    {
                        data: 'anioActual', title: 'Final', render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    },
                ],
                columnDefs: [
                    { "visible": false, "targets": groupColumn },
                    { className: "dt-center", "targets": "_all" },
                ],
                "drawCallback": function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;
                    api.column(groupColumn, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            if (last != null) {
                                $(rows).eq(i).before(
                                    `<tr class="group totalAño">
                                    <td colspan="2">Total:</td>
                                    ${getTotalColumna(last)}
                                    </tr>`
                                );
                                if (last != null) {
                                    $(rows).eq(i).before(
                                        '<tr class="group groupTr"><td colspan="1">' + group + '</td></tr>'
                                    );
                                }
                            }
                            else {
                                $(rows).eq(i).before(
                                    '<tr class="group groupTr"><td colspan="1">' + group + '</td></tr>'
                                );
                            }
                            last = group;
                        }
                        else {
                            if ((api.rows().nodes().length - 1) == i) {
                                $(rows).eq(i).after(
                                    `<tr class="group totalAño">
                                    <td colspan="2">Total:</td>
                                    ${getTotalColumna(last)}
                                    </tr>`
                                );
                            }
                        }
                    });

                },
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;
                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    for (let index = 3; index <= 14; index++) {
                        pageTotalAbono = api
                            .column(index, { page: 'current' })
                            .data()
                            .reduce(function (a, b) {
                                return intVal(a) + intVal(b);
                            }, 0);

                        // Update footer
                        $(api.column(index).footer()).html(
                            maskNumero(pageTotalAbono)
                        );
                    }
                }
            });
        }

        function initTablaAdeudoDLLS() {
            var groupColumn = 0;
            dtTblAdeudoDlls = tblAdeudoDlls.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                scrollY: '52vh',
                scrollCollapse: true,
                scrollX: true,
                "bLengthChange": false,
                columns: [
                    { data: 'anio', tittle: 'Anio' },
                    { data: 'descripcionInstitucion', title: 'Institución' },
                    {
                        data: 'anioAnterior', title: 'Inicial', render: (data, type, row) => {
                            if (inputTipoCambio.val() > 0) {
                                return maskNumero(data.toFixed(2) * inputTipoCambio.val());
                            }
                            else {
                                return maskNumero(data.toFixed(2));
                            }

                        }
                    },
                    {
                        data: 'enero', title: 'Ene ', render: (data, type, row) => {
                            if (inputTipoCambio.val() > 0) {
                                return maskNumero(data.toFixed(2) * inputTipoCambio.val());
                            }
                            else {
                                return maskNumero(data.toFixed(2));
                            }

                        }
                    },
                    {
                        data: 'febrero', title: 'Feb ', render: (data, type, row) => {
                            if (inputTipoCambio.val() > 0) {
                                return maskNumero(data.toFixed(2) * inputTipoCambio.val());
                            }
                            else {
                                return maskNumero(data.toFixed(2));
                            }

                        }
                    },
                    {
                        data: 'marzo', title: 'Mar ', render: (data, type, row) => {
                            if (inputTipoCambio.val() > 0) {
                                return maskNumero(data.toFixed(2) * inputTipoCambio.val());
                            }
                            else {
                                return maskNumero(data.toFixed(2));
                            }

                        }
                    },
                    {
                        data: 'abril', title: 'Abr ', render: (data, type, row) => {
                            if (inputTipoCambio.val() > 0) {
                                return maskNumero(data.toFixed(2) * inputTipoCambio.val());
                            }
                            else {
                                return maskNumero(data.toFixed(2));
                            }

                        }
                    },
                    {
                        data: 'mayo', title: 'May ', render: (data, type, row) => {
                            if (inputTipoCambio.val() > 0) {
                                return maskNumero(data.toFixed(2) * inputTipoCambio.val());
                            }
                            else {
                                return maskNumero(data.toFixed(2));
                            }

                        }
                    },
                    {
                        data: 'junio', title: 'Jun ', render: (data, type, row) => {
                            if (inputTipoCambio.val() > 0) {
                                return maskNumero(data.toFixed(2) * inputTipoCambio.val());
                            }
                            else {
                                return maskNumero(data.toFixed(2));
                            }

                        }
                    },
                    {
                        data: 'julio', title: 'Jul ', render: (data, type, row) => {
                            if (inputTipoCambio.val() > 0) {
                                return maskNumero(data.toFixed(2) * inputTipoCambio.val());
                            }
                            else {
                                return maskNumero(data.toFixed(2));
                            }

                        }
                    },
                    {
                        data: 'agosto', title: 'Ago ', render: (data, type, row) => {
                            if (inputTipoCambio.val() > 0) {
                                return maskNumero(data.toFixed(2) * inputTipoCambio.val());
                            }
                            else {
                                return maskNumero(data.toFixed(2));
                            }

                        }
                    },
                    {
                        data: 'septiembre', title: 'Sep ', render: (data, type, row) => {
                            if (inputTipoCambio.val() > 0) {
                                return maskNumero(data.toFixed(2) * inputTipoCambio.val());
                            }
                            else {
                                return maskNumero(data.toFixed(2));
                            }

                        }
                    },
                    {
                        data: 'octubre', title: 'Oct ', render: (data, type, row) => {
                            if (inputTipoCambio.val() > 0) {
                                return maskNumero(data.toFixed(2) * inputTipoCambio.val());
                            }
                            else {
                                return maskNumero(data.toFixed(2));
                            }

                        }
                    },
                    {
                        data: 'noviembre', title: 'Nov ', render: (data, type, row) => {
                            if (inputTipoCambio.val() > 0) {
                                return maskNumero(data.toFixed(2) * inputTipoCambio.val());
                            }
                            else {
                                return maskNumero(data.toFixed(2));
                            }

                        }
                    },
                    {
                        data: 'diciembre', title: 'Dic ', render: (data, type, row) => {
                            if (inputTipoCambio.val() > 0) {
                                return maskNumero(data.toFixed(2) * inputTipoCambio.val());
                            }
                            else {
                                return maskNumero(data.toFixed(2));
                            }

                        }
                    },
                    {
                        data: 'anioActual', title: 'Final', render: (data, type, row) => {
                            if (inputTipoCambio.val() > 0) {
                                return maskNumero(data.toFixed(2) * inputTipoCambio.val());
                            }
                            else {
                                return maskNumero(data.toFixed(2));
                            }

                        }
                    },
                ],
                columnDefs: [
                    { "visible": false, "targets": groupColumn },
                    { className: "dt-center", "targets": "_all" },
                ],
                "drawCallback": function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;
                    api.column(groupColumn, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            if (last != null) {
                                $(rows).eq(i).before(
                                    `<tr class="group totalAño">
                                    <td colspan="2">Total:</td>
                                    ${getTotalColumnaDLLS((+inputTipoCambio.val()) > 0 ? last * inputTipoCambio.val() : last)}
                                    </tr>`
                                );
                                if (last != null) {
                                    $(rows).eq(i).before(
                                        '<tr class="group groupTr"><td colspan="1">' + group + '</td></tr>'
                                    );
                                }
                            }
                            else {
                                $(rows).eq(i).before(
                                    '<tr class="group groupTr"><td colspan="1">' + group + '</td></tr>'
                                );
                            }
                            last = group;
                        }
                        else {
                            if ((api.rows().nodes().length - 1) == i) {
                                $(rows).eq(i).after(
                                    `<tr class="group totalAño">
                                    <td colspan="2">Total:</td>
                                    ${getTotalColumnaDLLS((+inputTipoCambio.val()) > 0 ? last * inputTipoCambio.val() : last)}
                                    </tr>`
                                );
                            }
                        }
                    });

                },
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;
                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    for (let index = 3; index <= 14; index++) {
                        pageTotalAbono = api
                            .column(index, { page: 'current' })
                            .data()
                            .reduce(function (a, b) {
                                return intVal(a) + intVal(b);
                            }, 0);

                        // Update footer
                        if (inputTipoCambio.val() > 0) {
                            $(api.column(index).footer()).html(
                                maskNumero(pageTotalAbono * inputTipoCambio.val())
                            );
                        }
                        else {
                            $(api.column(index).footer()).html(
                                maskNumero(pageTotalAbono)
                            );
                        }
                    }
                }
            });
        }

        function getTotalColumna(last) {

            let td = '';
            for (let index = 1; index <= 12; index++) {
                td += `<td colspan="1">${maskNumero(getSumMes(index, last))}</td>`
            }
            return td;

        }

        function getTotalColumnaDLLS(last) {
            let td = '';
            for (let index = 1; index <= 12; index++) {
                td += `<td colspan="1">${maskNumero(getSumMesDLLS(index, last))}</td>`
            }
            return td;
        }

        function getSumMes(mes, grupo) {
            switch (mes) {
                case 1:
                    return dtTablaReporteAdedudo.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.enero }, 0);
                case 2:
                    return dtTablaReporteAdedudo.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.febrero }, 0);
                case 3:
                    return dtTablaReporteAdedudo.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.marzo }, 0);
                case 4:
                    return dtTablaReporteAdedudo.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.abril }, 0);
                case 5:
                    return dtTablaReporteAdedudo.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.mayo }, 0);
                case 6:
                    return dtTablaReporteAdedudo.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.junio }, 0);
                case 7:
                    return dtTablaReporteAdedudo.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.julio }, 0);
                case 8:
                    return dtTablaReporteAdedudo.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.agosto }, 0);
                case 9:
                    return dtTablaReporteAdedudo.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.septiembre }, 0);
                case 10:
                    return dtTablaReporteAdedudo.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.octubre }, 0);
                case 11:
                    return dtTablaReporteAdedudo.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.noviembre }, 0);
                case 12:
                    return dtTablaReporteAdedudo.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.diciembre }, 0);
                default:
                    return 0;
            }
        }


        function getSumMesDLLS(mes, grupo) {
            switch (mes) {
                case 1:
                    return dtTblAdeudoDlls.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.eneroDlls }, 0);
                case 2:
                    return dtTblAdeudoDlls.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.febreroDlls }, 0);
                case 3:
                    return dtTblAdeudoDlls.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.marzoDlls }, 0);
                case 4:
                    return dtTblAdeudoDlls.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.abrilDlls }, 0);
                case 5:
                    return dtTblAdeudoDlls.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.mayoDlls }, 0);
                case 6:
                    return dtTblAdeudoDlls.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.junioDlls }, 0);
                case 7:
                    return dtTblAdeudoDlls.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.julioDlls }, 0);
                case 8:
                    return dtTblAdeudoDlls.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.agostoDlls }, 0);
                case 9:
                    return dtTblAdeudoDlls.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.septiembreDlls }, 0);
                case 10:
                    return dtTblAdeudoDlls.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.octubreDlls }, 0);
                case 11:
                    return dtTblAdeudoDlls.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.noviembreDlls }, 0);
                case 12:
                    return dtTblAdeudoDlls.data().toArray().filter(r => r.anio == grupo).reduce((acc, mes) => { return acc + mes.diciembreDlls }, 0);
                default:
                    return 0;
            }
        }

    }
    $(() => Maquinaria.DocumentosPorPagar.ReporteDeAdeudo = new ReporteDeAdeudo())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();