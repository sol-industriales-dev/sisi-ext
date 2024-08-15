(() => {
    $.namespace('CH.ReporteSegCandidatos');

    //#region CONST FILTROS
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const cboFiltroCC = $('#cboFiltroCC');
    //#endregion

    //#region CONST
    const tblReporteSegCandidatos = $('#tblReporteSegCandidatos');
    let dtSegCandidatos;
    let cantColumnasDT;
    let arrColumnasDT = [];
    let arrColumnasExcel = [];
    //#endregion

    ReporteSegCandidatos = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT DATATABLES
            fncGetReporteSegCandidatos();
            //#endregion

            //#region EVENTOS FILTROS
            btnFiltroBuscar.on("click", function () {
                fncGetReporteSegCandidatos();
            });

            cboFiltroCC.fillCombo("/Reclutamientos/FillFiltroCboCC", {}, false);
            cboFiltroCC.select2();
            //#endregion
        }

        //#region FUNCIONES REPORTE SEGUIMIENTO DE CANDIDATOS
        function fncGetReporteSegCandidatos() {
            let obj = new Object();
            obj = {
                cc: cboFiltroCC.val()
            }
            axios.post("GetReporteSegCandidatos", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    cantColumnasDT = response.data.lstFasesDTO.length;
                    let inicioColDinamica = 7;
                    for (let i = 0; i < cantColumnasDT; i++) {
                        arrColumnasDT.push(inicioColDinamica);
                        inicioColDinamica++;
                    }

                    let ultimaColDinamica = 0;
                    for (let i = 0; i < arrColumnasDT.length; i++) {
                        ultimaColDinamica = arrColumnasDT[i];
                    }

                    arrColumnasExcel = [0, 1, 2, 3, 4, 5, 6];
                    for (let i = 0; i < cantColumnasDT; i++) {
                        ultimaColDinamica++;
                        arrColumnasExcel.push(ultimaColDinamica);
                    }

                    for (let i = 0; i < 4; i++) {
                        ultimaColDinamica++;
                        arrColumnasExcel.push(ultimaColDinamica);
                    }

                    establecerColumnasCursos(response.data.lstCandidatosDTO, response.data.lstFasesDTO);
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function establecerColumnasCursos(lstCandidatos, columnasCursos) {
            const columnas = [
                { data: 'puesto', title: 'PUESTO' },
                // { data: 'departamento', title: 'DEPARTAMENTO' },
                { data: 'nombre', title: 'NOMBRE' },
                { data: 'edad', title: 'EDAD' },
                {
                    data: 'nss', title: 'NSS',
                    render: function (data, type, row) {
                        if (data != "") {
                            return data + ".";
                        } else {
                            return "";
                        }
                    }
                },
                { data: 'residencia', title: 'RESIDENCIA' },
                {
                    data: 'telefono', title: 'TELEFONO',
                    render: function (data, type, row) {
                        if (data != "") {
                            return data + ".";
                        } else {
                            return "";
                        }
                    }
                },
                {
                    data: 'fechaEntrevista', title: 'FECHA DE <br>ENTREVISTA',
                    render: function (data, type, row) {
                        let fechaNacimiento = moment(data).format("DD/MM/YYYY");
                        if (fechaNacimiento != '01/01/2000') {
                            return fechaNacimiento;
                        } else {
                            return "";
                        }
                    }
                }
            ];

            //#region COLUMNAS DINAMICAS
            let contador = 0;
            columnasCursos.forEach(x =>
                columnas.push({
                    data: x.Item1, title: x.Item2,
                    render: function (data, type, row) {
                        let strSplit = data.split("|");
                        let tamanioCadena = (strSplit.length);
                        contador++;
                        if (contador == tamanioCadena) {
                            contador = 1;
                        }

                        while (contador < tamanioCadena) {
                            let estatus = strSplit[contador];
                            let icono = "";
                            switch (estatus) {
                                case "0":
                                    icono = `<i class="R fas fa-times-circle fa-2x" style="color: #ff0000" title="No aprobado."></i>`;
                                    break;
                                case "1":
                                    icono = `<i class="G fas fa-check-circle fa-2x" style="color: #008000" title="Aprobado."></i>`;
                                    break;
                                case "2":
                                    icono = `<i class="Y fas fa-exclamation-circle fa-2x" style="color: #CDAE00" title="Actividades pendientes."></i>`;
                                    break;
                            }
                            return icono;
                        }
                    }
                }));
            //#endregion

            //#region COLUMNAS DINAMICAS NO VISIBLES
            contador = 0;
            columnasCursos.forEach(x =>
                columnas.push({
                    data: x.Item1, title: x.Item2, visible: false,
                    render: function (data, type, row) {
                        let strSplit = data.split("|");
                        let tamanioCadena = (strSplit.length);
                        contador++;
                        if (contador == tamanioCadena) {
                            contador = 1;
                        }

                        while (contador < tamanioCadena) {
                            let estatus = strSplit[contador];
                            let icono = "";
                            switch (estatus) {
                                case "0":
                                    icono = `NO APROBADO`;
                                    break;
                                case "1":
                                    icono = `APROBADO`;
                                    break;
                                case "2":
                                    icono = `PENDIENTE`;
                                    break;
                            }
                            return icono;
                        }
                    }
                }));
            //#endregion

            columnas.push({
                data: "porcProceso", title: "% PROCESO",
                render: function (data, type, row) {
                    let txtProgressBar = "";
                    let porcent = row.porcProceso.toFixed(2);
                    txtProgressBar +=
                        `<div class="progress">
                            <div class="progress-bar" style="width: 100%; background-color: #008000 !important;" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100">${porcent}%</div>
                        </div>`;
                    return txtProgressBar;
                }
            });
            columnas.push({
                data: "fechaLiberacion", title: "FECHA LIBERACIÓN",
                render: function (data, type, row) {
                    let fechaNacimiento = moment(data).format("DD/MM/YYYY");
                    if (fechaNacimiento != '01/01/2000') {
                        return fechaNacimiento;
                    } else {
                        return "";
                    }
                }
            });
            columnas.push({
                data: "tiempoTranscurrido", title: "TIEMPO TRANSCURRIDO",
                render: function (data, type, row) {
                    let tiempoTranscurrido = parseFloat(data);
                    if (tiempoTranscurrido > 1) {
                        return data + " días.";
                    } else if (tiempoTranscurrido == 1) {
                        return data + " día.";
                    }
                }
            });
            columnas.push({ data: "comentarios", title: "COMENTARIOS" });

            initTablaPersonalActivo(lstCandidatos, columnas);
        }

        function initTablaPersonalActivo(data, columns) {
            if (dtSegCandidatos != null) {
                dtSegCandidatos.clear().destroy();
                $('#tblReporteSegCandidatos').empty();
            }
            dtSegCandidatos = tblReporteSegCandidatos.DataTable({
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excelHtml5',
                        footer: true,
                        exportOptions: {
                            columns: arrColumnasExcel
                        }
                    }
                ],
                destroy: true,
                language: dtDicEsp,
                data,
                paging: false,
                ordering: false,
                searching: true,
                order: [[3, "asc"], [1, "asc"]],
                columns,
                scrollX: true,
                scrollY: '60vh',
                scrollCollapse: true,
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
            // Al agregar columnas estáticas, agregarlas de esta forma para evitar el error: "fixedColumns already initialised on this table"
            new $.fn.dataTable.FixedColumns(dtSegCandidatos, {
                leftColumns: 7
            });
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.ReporteSegCandidatos = new ReporteSegCandidatos();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();