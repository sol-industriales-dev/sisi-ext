(() => {
    $.namespace('Nominas.AcumuladoMensual');

    //#region Componentes html
    //Variables jquery
    const inputDateMin = $('#inputDateMin'),
        inputDateMax = $('#inputDateMax'),
        btnSetRango = $('#btnSetRango'),
        btnExportar = $('#btnExportar'),
        tblAcomuladoMensual = $('#tblReporteAcomulado'),
        thFooterSemana5 = $('#semana5'),
        spanEmpleadosNum = $('#empleadosNum');
    //#endregion

    //#region Variables locales
    //DT
    let dtAcomuladoMensual,
        range = 0,
        dateNow = new Date(),
        añoActual = dateNow.getFullYear(),
        mesActual = dateNow.getMonth(),
        numEmpleados = 0;

    let consultado = false;

    let months = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];

    //#endregion

    AcomuladoMensual = function () {
        (function init() {
            inputDateMin.datepicker({
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                dateFormat: "mm/yy",
                yearRange: '2018:c',
                onClose: function (dateText, inst) {
                    $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
                }
            });
            // inputDateMax.datepicker({
            //     dateFormat: "dd/mm/yy",
            //     changeYear: true,
            //     changeMonth: true,
            //     showButtonPanel: true,
            //     yearRange: '2018:c',
            //     onClose: function (dateText, inst) {
            //         function isDonePressed() {
            //             return ($('#ui-datepicker-div').html().indexOf('ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all ui-state-hover') > -1);
            //         }

            //         if (isDonePressed()) {
            //             var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
            //             var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
            //             var day = moment(new Date(year, month, 1)).endOf('month').format('DD');
            //             $(this).datepicker('setDate', new Date(year, month, day)).trigger('change');
            //             $(this).focusout()//Added to remove focus from datepicker input box on selecting date
            //         }
            //     }
            // }).datepicker('setDate', new Date(añoActual, mesActual, ultimoDiaDelMes));

            fncListeners();
            //initTblAcomuladoMensual();
        })();

        function fncListeners() {
            //Boton de busqueda
            btnSetRango.on('click', function () {


                let strMensajeError = "";

                if (inputDateMin.val() == "") {/* $("#select2-cboCrearEditarCC-container").css('border', '2px solid red');*/ strMensajeError = "Es necesario llenar los campos obligatorios."; }

                if (strMensajeError != "") {
                    Alert2Warning(strMensajeError);
                    btnExportar.prop('disabled', true);
                    return "";
                } else {

                    consultado = true;
                    btnExportar.prop('disabled', false);

                    btmDate = inputDateMin.val().split("/");
                    //topDate = inputDateMax.val().split("/");

                    let ultimoDiaDelMes = moment(new Date(btmDate[1], btmDate[0] - 1, 1)).endOf('month').format('DD');

                    btmObjDate = new Date(parseInt(btmDate[1]), parseInt(btmDate[0]), 1);
                    topObjDate = new Date(parseInt(btmDate[1]), parseInt(btmDate[0]), ultimoDiaDelMes);

                    //Calcular cual es la diferencia entre el rango de meses de los inputs para obtener el numero de columnas
                    range = monthDiff(btmObjDate, topObjDate);

                    //fncGetReportes(`${btmDate[2]}-${btmDate[0]}-${btmDate[1]}`, `${topDate[2]}-${topDate[0]}-${topDate[1]}`);
                    GetNominaDetalle(`${btmDate[1]}-1-${btmDate[0]}`, `${btmDate[1]}-${ultimoDiaDelMes}-${btmDate[0]}`);
                    //fncGetNumEmpleados(`${btmDate[1]}-1-${btmDate[0]}`, `${btmDate[1]}-${ultimoDiaDelMes}-${btmDate[0]}`);
                }
            });

            btnExportar.on('click', function () {

                let strMensajeError = "Vuelva a ingresar el rango";

                if (!consultado) {
                    Alert2Warning(strMensajeError);
                    return "";
                } else {
                    saveData();
                }
            });
        }

        function initTblAcomuladoMensual(data, headers) {

            let semana5Vis = false;
            let semana5Title = "Semana 5";

            if (dtAcomuladoMensual != null) {
                dtAcomuladoMensual.destroy();
                tblAcomuladoMensual.empty();
                tblAcomuladoMensual.append('<thead class="bg-table-header"></thead>');

                tblAcomuladoMensual.append("<tfoot><tr><th></th><th></th><th></th><th>Totales</th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th></tr></tfoot>");

            }

            if (data[0].semana5 != null) {
                semana5Vis = true;
                semana5Title = `Semana ${headers[4].periodo}`;
            }

            dtAcomuladoMensual = tblAcomuladoMensual.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                scrollX: true,
                scrollY: '45vh',
                scrollCollapse: true,
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'cta1', title: 'Cta', className: 'text-center' },
                    { data: 'cta2', title: 'Sub Cta', className: 'text-center' },
                    { data: 'cta3', title: 'Subsub Cta', className: 'text-center' },
                    { data: 'concepto', title: 'Descripcion' },
                    { data: 'semana1', title: `Semana ${headers[0].periodo}\n` },
                    { data: 'semana2', title: `Semana ${headers[1].periodo}\n` },
                    { data: `semana3`, title: `Semana ${headers[2].periodo}\n` },
                    { data: `semana4`, title: `Semana ${headers[3].periodo}\n` },
                    { data: `semana5`, title: semana5Title, visible: semana5Vis },
                    { data: 'finiquitoSemanal', title: 'Finiquito Semanal' },
                    { data: 'quincena1', title: `Quincena ${headers[5].periodo}\n` },
                    { data: `quincena2`, title: `Quincena ${headers[6].periodo}\n` },
                    { data: 'finiquitoQuincenal', title: 'Finiquito Quincenal' },
                    { data: 'total', title: 'SUMAS' },
                ],
                columnDefs: [
                    {
                        targets: [4, 5, 6, 7, 8, 9, 10, 11, 12, 13],
                        render: function (data, row, type) {
                            return maskNumero(data);
                        }
                    },
                    { className: 'dt-center', 'targets': '_all' }
                ],
                headerCallback: function (thead, data, start, end, display) {
                    $(thead).parent().children().addClass('dtHeader');
                    $(thead).children().addClass('text-center');
                },
                createdRow: function (row, data, dataIndex) {

                    //Agregar break lines a la tabla
                    switch (true) {
                        case data[0] == '5000' && data[1] == '13' && data[2] == '17':
                            $(row).find('td').addClass('tdBorde');
                            break;

                        case data[0] == '5000' && data[1] == '14' && data[2] == '17':
                            $(row).find('td').addClass('tdBorde');
                            break;

                        default:
                            break;
                    }

                    tblAcomuladoMensual.DataTable().columns().every(function (colIdx, tableLoop, colLoop) {
                        if (colIdx > 3) {
                            let total = 0;

                            for (let x = 0; x < this.data().length; x++) {
                                total += this.data()[x];
                            }

                            this.column(colIdx).visible(total != 0);
                        }

                    });

                    let total = 0;
                    let invisibles = 0;

                    tblAcomuladoMensual.DataTable().columns().every(function (colIdx, tableLoop, colLoop) {
                        if (colIdx > 3) {
                            for (let x = 0; x < this.data().length; x++) {
                                total += Number(this.data()[x]);
                            }

                            if (this.column(colIdx).visible()) {
                                if (colIdx < 4) {
                                    $(this.footer()).html(maskNumero(total));
                                }
                                else {
                                    $(this.footer()).html(maskNumero(total));
                                }
                            }
                            else {
                                invisibles++;
                            }
                            total = 0;
                        }
                    });

                },
                drawCallback: function (settings) {

                },
            });


        }

        //#region UTILS
        function monthDiff(d1, d2) {
            var months;
            months = (d2.getFullYear() - d1.getFullYear()) * 12;
            months -= d1.getMonth();
            months += d2.getMonth();
            return months <= 0 ? 0 : months;
        }

        function getYSize(data) {

            let dataSizes = [];

            for (let i = 0; i < data.length; i++) {
                dataSizes[i] = data[i].lstMontos.length;
            }
            return arrayMax(dataSizes);
        }

        function getMonth(date) {
            let nombreDelMes = "";

            nombreDelMes = months[date - 1];

            return nombreDelMes;
        }

        function arrayMax(arr) {
            var len = arr.length, max = -Infinity;
            while (len--) {
                if (Number(arr[len]) > max) {
                    max = Number(arr[len]);
                }
            }
            return max;
        };

        //#endregion

        function addRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function fncGetNumEmpleados(btm, tp) {
            axios.post('GetNumEmpleados', { bottom: btm, top: tp }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    spanEmpleadosNum.text(items);
                    numEmpleados = items;
                }
            }).catch(error => Alert2Error(error.message));
        }

        function GetNominaDetalle(bottom, top) {
            axios.post('GetNominaDetalle', { botomDate: bottom, topDate: top }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    initTblAcomuladoMensual(items.items, items.dataHeaders);

                    addRows(tblAcomuladoMensual, items.items);

                }
            }).catch(error => Alert2Error("Mes vacio"));
        }

        //#region Excel
        function saveData() {
            let dtTabla = dtAcomuladoMensual.data().toArray(),
                dtHeaders = [],
                dtFooters = [];

            dtAcomuladoMensual.columns().every(function (colIdx, tableLoop, colLoop) {
                dtHeaders.push(dtAcomuladoMensual.column(colIdx).header().textContent);
            });

            dtAcomuladoMensual.columns().every(function (colIdx, tableLoop, colLoop) {
                dtFooters.push(dtAcomuladoMensual.column(colIdx).footer().textContent);
            });

            let objHeader = {};
            let objFooter = {};

            if (dtTabla[0].semana5 != null) {
                objHeader = {
                    cta1: dtHeaders[0],
                    cta2: dtHeaders[1],
                    cta3: dtHeaders[2],
                    concepto: dtHeaders[3],
                    semana1: dtHeaders[4],
                    semana2: dtHeaders[5],
                    semana3: dtHeaders[6],
                    semana4: dtHeaders[7],
                    semana5: dtHeaders[8],
                    finiquitoSemanal: dtHeaders[9],
                    quincena1: dtHeaders[10],
                    quincena2: dtHeaders[11],
                    finiquitoQuincenal: dtHeaders[12],
                    total: dtHeaders[13],
                }
                objFooter = {
                    cta1: dtFooters[0],
                    cta2: dtFooters[1],
                    cta3: dtFooters[2],
                    concepto: dtFooters[3],
                    semana1: dtFooters[4],
                    semana2: dtFooters[5],
                    semana3: dtFooters[6],
                    semana4: dtFooters[7],
                    semana5: dtFooters[8],
                    finiquitoSemanal: dtFooters[9],
                    quincena1: dtFooters[10],
                    quincena2: dtFooters[11],
                    finiquitoQuincenal: dtFooters[12],
                    total: dtFooters[13],
                }
            } else {
                objHeader = {
                    cta1: dtHeaders[0],
                    cta2: dtHeaders[1],
                    cta3: dtHeaders[2],
                    concepto: dtHeaders[3],
                    semana1: dtHeaders[4],
                    semana2: dtHeaders[5],
                    semana3: dtHeaders[6],
                    semana4: dtHeaders[7],
                    finiquitoSemanal: dtHeaders[9],
                    quincena1: dtHeaders[10],
                    quincena2: dtHeaders[11],
                    finiquitoQuincenal: dtHeaders[12],
                    total: dtHeaders[13],
                }
                objFooter = {
                    cta1: dtFooters[0],
                    cta2: dtFooters[1],
                    cta3: dtFooters[2],
                    concepto: dtFooters[3],
                    semana1: dtFooters[4],
                    semana2: dtFooters[5],
                    semana3: dtFooters[6],
                    semana4: dtFooters[7],
                    finiquitoSemanal: dtFooters[9],
                    quincena1: dtFooters[10],
                    quincena2: dtFooters[11],
                    finiquitoQuincenal: dtFooters[12],
                    total: dtFooters[13],
                }
            }

            dtTabla.push(objFooter);

            axios.post('creaVariableDeSesionMensual', { lstReporteData: dtTabla, lstReporteHeaders: objHeader, numeroEmpleados: numEmpleados.toString(), periodoDate: `${getMonth(btmDate[0])} ${btmDate[1]}` }).then(response => {
                location.href = '/Administrativo/Nomina/crearReporteMensual';
                let { success, items, message } = response.data;
                if (success) {
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

    }




    $(document).ready(() => {
        Nominas.AcomuladoMensual = new AcomuladoMensual();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();