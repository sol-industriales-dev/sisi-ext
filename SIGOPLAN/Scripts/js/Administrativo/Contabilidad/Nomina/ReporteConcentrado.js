(() => {
    $.namespace('Nominas.GetReporteEmpleadoCC');

    //#region Componentes
    //Variables jquery

    const inMinDate = $('#min'),
        inMaxDate = $('#max'),
        btnSetRango = $('#btnSetRango'),
        btnExportar = $('#btnExportar'),
        tblReporteEmpleadoCC = $('#tblReporteEmpleadoCC'),
        spanEmpleadosNum = $('#empleadosNum'),
        cboxCentroCostos = $('#cboxCentroCostos'),
        cboxEmpleado = $('#cboxEmpleado');
    const inputMes = $('#inputMes');
    const cboFiltroTipoRaya = $('#cboFiltroTipoRaya');
    const numHeadEmpleados = $('#numHeadEmpleados');
    const cboFiltroTipoNomina = $('#cboFiltroTipoNomina');
    //#endregion

    //#region Variables locales

    let dtReporteEmpleadoCC,
        dateNow = new Date(),
        añoActual = dateNow.getFullYear(),
        mesActual = dateNow.getMonth(),
        ultimoDiaDelMes = moment(new Date(añoActual, mesActual, 1)).endOf('month').format('DD'),
        lstDetalleAnual = [{
            data: '',
            title: ''
        }];
    let months = ["enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre"];

    let dataArray = [];

    let btmDate, topDate, btmObjDate, topObjDate;

    //Calcular cual es la diferencia entre el rango de meses de los inputs para obtener el numero de columnas
    let range = 0;
    let footerData = [],
        numEmpleados = 0;

    let formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
    });

    let consultado = false;

    //#endregion


    GetReporteEmpleadoCC = function () {
        (function init() {
            //#region Date Picker
            inputMes.datepicker({
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                dateFormat: "mm/yy",
                yearRange: '2018:c',
                onClose: function (dateText, inst) {
                    $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
                }
            });
            //#endregion

            //#region SELECT2

            cboxCentroCostos.select2({ width: 'resolve' });
            cboxEmpleado.select2({ width: 'resolve' });
            cboFiltroTipoRaya.select2({ width: 'resolve' });

            //#endregion

            fncListeners();
            FillCombos();
        })();


        //#region Listeners
        function fncListeners() {

            //Boton de busqueda
            btnSetRango.on('click', function () {

                let strMensajeError = "";

                // if (inMinDate.val() == "") {/* $("#select2-cboCrearEditarCC-container").css('border', '2px solid red');*/ strMensajeError = "Es necesario llenar los campos obligatorios."; }
                // if (inMaxDate.val() == "") { /*$("#select2-cboCrearEditarCC-container").css('border', '2px solid red');*/ strMensajeError = "Es necesario llenar los campos obligatorios."; }
                if (inputMes.val() == "") { /*$("#select2-cboCrearEditarCC-container").css('border', '2px solid red');*/ strMensajeError = "Es necesario llenar los campos obligatorios."; }


                // console.log(cboxCentroCostos.val());

                if (strMensajeError != "") {
                    Alert2Warning(strMensajeError);
                    btnExportar.prop('disabled', true);
                    return "";
                } else {

                    consultado = true;
                    btnExportar.prop('disabled', false);

                    btmDate = inputMes.val().split("/");
                    topDate = inputMes.val().split("/");

                    let ultimoDiaDelMes = moment(new Date(btmDate[1], btmDate[0] - 1, 1)).endOf('month').format('DD');

                    btmObjDate = new Date(parseInt(btmDate[2]), parseInt(btmDate[1]), parseInt(btmDate[0]));
                    topObjDate = new Date(parseInt(topDate[2]), parseInt(topDate[1]), parseInt(topDate[0]));

                    range = monthDiff(btmObjDate, topObjDate);
                    fncGetReporteConcentrado(`${btmDate[1]}-${btmDate[0]}-1`, `${topDate[1]}-${topDate[0]}-${ultimoDiaDelMes} `);
                }
            });

            // btnExportar.on('click', function () {

            //     let strMensajeError = "Vuelva a ingresar el rango";
            //     btmDate = inputMes.val().split("/");
            //     ultimoDiaDelMes = moment(new Date(btmDate[1], btmDate[0] - 1, 1)).endOf('month').format('DD');

            //     if (!consultado) {
            //         Alert2Warning(strMensajeError);
            //         return "";
            //     } else {
            //         if (cboxCentroCostos.val() != "" && cboxEmpleado.val() == "") {
            //             saveData(`${btmDate[2]}-${btmDate[0]}-${btmDate[1]}`, `${topDate[2]}-${topDate[0]}-${topDate[1]}`, cboxCentroCostos.val(), null);
            //         } else if (cboxEmpleado.val() != "" && cboxCentroCostos.val() == "") {
            //             saveData(`${btmDate[1]}-1-${btmDate[0]}`, `${topDate[1]}-${ultimoDiaDelMes}-${topDate[0]}`, null, cboxEmpleado.val());
            //         } else {
            //             saveData(`${btmDate[1]}-1-${btmDate[0]}`, `${topDate[1]}-${ultimoDiaDelMes}-${topDate[0]}`, cboxCentroCostos.val(), cboxEmpleado.val());
            //         }
            //     }
            // });

        }
        //#endregion

        //#region Inizalizacion de la tabla
        function initTblReporteAnualNomina(data, headers) {

            if (dtReporteEmpleadoCC != null) {
                dtReporteEmpleadoCC.destroy();
                tblReporteEmpleadoCC.empty();
                tblReporteEmpleadoCC.append('<thead class="bg-table-header"></thead>');

            }
            //Reinicializar las columnas
            dataArray = [];

            let temp = "";

            dtReporteEmpleadoCC = tblReporteEmpleadoCC.DataTable({
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
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excelHtml5',
                        exportOptions: {
                            // columns: [':visible', 21]
                        }
                    }
                ],
                columns: [
                    //render: function (data, type, row) { }
                    // { data: 'anioNom', title: 'Año' },
                    { data: 'mesNom', title: 'Mes' },
                    { data: 'numEmpleados', title: 'numEmpleados' },
                    // { data: 'tipoNominaNom', title: 'tipoNominaNom', visible: false },
                    // { data: 'tipoNominaDesc', title: 'Tipo Nomina' },
                    { data: 'periodoNom', title: 'Periodo' },
                    // { data: 'fechaInicio', title: 'Fecha Inicio' },
                    // { data: 'fechaFin', title: 'Fecha Fin' },
                    // { data: 'ccNom', title: 'CC' },
                    // { data: 'ccDesc', title: 'Descripcion CC' },
                    // { data: 'numeroEmpleado', title: 'Numero empleado' },
                    // { data: 'nombreCompleto', title: 'Nombre' },
                    { data: 'sueldoImporte', title: 'Sueldo Importe' },
                    { data: 'septimoDiaImporte', title: 'Septimo Día Importe' },
                    { data: 'horasExtraDoblesImporte', title: 'Horas Extra Doblres', visible: false },
                    { data: 'horasExtrasTriplesImporte', title: 'Horas Extras Triples', visible: false },
                    { data: 'primaDominicalImporte', title: 'Prima Dominical', visible: false },
                    { data: 'ansoFestivoTrabajadoImporte', title: 'Anso Festivo Trabajado', visible: false },
                    { data: 'incapacidadEnfermedad3DiasImporte', title: 'Incapacidad Enfermedad 3 Dias', visible: false },
                    { data: 'premiosAsistenciaImporte', title: 'Premios Asistencia', visible: false },
                    { data: 'bonoPuntualidadImporte', title: 'Bono Puntualidad', visible: false },
                    { data: 'permisosConGoceImporte', title: 'Permisos Con Goce', visible: false },
                    { data: 'despensa2Importe', title: 'Despensa 2', visible: false },
                    { data: 'fondoAhorroEmpresaImporte', title: 'Fondo Ahorro Empresa', visible: false },
                    { data: 'compensacionIsptImssImporte', title: 'Compensacion Ispt Imss', visible: false },
                    { data: 'vacacionTrabajadaImporte', title: 'Vacacion Trabajada', visible: false },
                    { data: 'vacacionFiniquitoImporte', title: 'Vacacion Finiquito', visible: false },
                    { data: 'vacacionDisfrutadaImporte', title: 'Vacacion Disfrutada', visible: false },
                    { data: 'primaVacacionalImporte', title: 'Prima Vacacional', visible: false },
                    { data: 'primaVacacional2Importe', title: 'Prima Vacacional 2', visible: false },
                    { data: 'aguinaldoGravadoImporte', title: 'Aguinaldo Gravado', visible: false },
                    { data: 'aguinaldoExentoImporte', title: 'Aguinaldo Exento', visible: false },
                    { data: 'primaAntiguedadImporte', title: 'Prima Antiguedad', visible: false },
                    { data: 'indemnizacion3MesesImporte', title: 'Indemnizacion 3 Meses', visible: false },
                    { data: 'indemnizacion20DiasImporte', title: 'Indemnizacion 20 Dias', visible: false },
                    { data: 'indemnizacionOtrosImporte', title: 'Indemnizacion Otros', visible: false },
                    { data: 'repartoUtilidadGraImporte', title: 'Reparto Utilidad Gra', visible: false },
                    { data: 'repartoUtilidadExeImporte', title: 'Reparto Utilidad Exe', visible: false },
                    { data: 'prestamoAhorroImporte', title: 'Prestamo Ahorro', visible: false },
                    { data: 'fondoAhorroEmpresa2Importe', title: 'Fondo Ahorro Empresa2', visible: false },
                    { data: 'fondoAhorroEmpleadoImporte', title: 'Fondo Ahorro Empleado', visible: false },
                    { data: 'interesPorFondoAhorroImporte', title: 'Interes Por Fondo Ahorro', visible: false },
                    { data: 'diferenciaCategoriaImporte', title: 'Diferencia Categoria', visible: false },
                    { data: 'bonosImporte', title: 'Bonos', visible: false },
                    { data: 'retroactivoDiversoImporte', title: 'Retroactivo Diverso', visible: false },
                    { data: 'comisionesSueldoImporte', title: 'Comisiones Sueldo', visible: false },
                    { data: 'incentivoProductivoImporte', title: 'Incentivo Productivo', visible: false },
                    { data: 'gratificacionEspecialImporte', title: 'Gratificacion Especial', visible: false },
                    { data: 'prevSocialImporte', title: 'Prev Social', visible: false },
                    { data: 'prevSocialAlimentosImporte', title: 'Prev Social Alimentos', visible: false },
                    { data: 'prevSocialHabitacionImporte', title: 'Prev Social Habitacion', visible: false },
                    { data: 'otrasPercepcionesImporte', title: 'Otras Percepciones', visible: false },
                    { data: 'compensacionUnicaExtraordinariaImporte', title: 'Compensacion Unica Extraordinaria', visible: false },
                    { data: 'bonoProduccionImporte', title: 'Bono Produccion', visible: false },
                    { data: 'despensaImporte', title: 'Despensa', visible: false },
                    { data: 'previsionSocialImporte', title: 'Prevision Social', visible: false },
                    { data: 'altoCostoVidaImporte', title: 'Alto Costo Vida', visible: false },
                    { data: 'despensa3Importe', title: 'Despensa 3', visible: false },
                    { data: 'prevSocialAlimentos2Importe', title: 'Prev Social Alimentos 2', visible: false },
                    { data: 'altoCostoVida2Importe', title: 'Alto Costo Vida 2', visible: false },
                    { data: 'subsidioEmpleoImporte', title: 'Subsidio Empleo', visible: false },
                    { data: 'totalPercepciones', title: 'Total Percepciones', visible: false },
                    { data: 'isrImporte', title: 'ISR', visible: false },
                    { data: 'imssImporte', title: 'IMSS', visible: false },
                    { data: 'isptCompImporte', title: 'ISPT Comp', visible: false },
                    { data: 'cuotaSindicalImporte', title: 'Cuota Sindical', visible: false },
                    { data: 'cuotaSindicalExtraImporte', title: 'Cuota Sindical Extra', visible: false },
                    { data: 'infonavitImporte', title: 'Infonavit', visible: false },
                    { data: 'fonacotImporte', title: 'Fonacot', visible: false },
                    { data: 'pensionAlimenticiaImporte', title: 'Pension Alimenticia', visible: false },
                    { data: 'vejezSarImporte', title: 'Vejez Sar', visible: false },
                    { data: 'descuentosAlimentosImporte', title: 'Descuentos Alimentos', visible: false },
                    { data: 'descuentosAlimentosPendImporte', title: 'Descuentos Alimentos Pend', visible: false },
                    { data: 'adeudoEmpresaImporte', title: 'Adeudo Empresa', visible: false },
                    { data: 'interesesPrestamoImporte', title: 'Intereses Prestamo', visible: false },
                    { data: 'fondoAhorroEmpleado2Importe', title: 'Fondo Ahorro Empleado 2', visible: false },
                    { data: 'fondoAhorroEmpresa3Importe', title: 'Fondo Ahorro Empresa 3', visible: false },
                    { data: 'infonavitMesAnteriorImporte', title: 'Infonavit Mes Anterior', visible: false },
                    { data: 'anticiposDiversosImporte', title: 'Anticipos Diversos', visible: false },
                    { data: 'isptSaldoAnteriorImporte', title: 'ISPT Saldo Anterior', visible: false },
                    { data: 'descuentosImporte', title: 'Descuentos', visible: false },
                    { data: 'famsaImporte', title: 'Famsa', visible: false },
                    { data: 'prestamosImporte', title: 'Prestamos', visible: false },
                    { data: 'axaImporte', title: 'AXA', visible: false },
                    { data: 'ajusteNominaImporte', title: 'Ajuste Nomina', visible: false },
                    { data: 'totalDeducciones', title: 'Total Deducciones', visible: false },
                    { data: 'netoPagar', title: 'Neto a Pagar', visible: false },
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [

                    { className: 'dt-center', 'targets': '_all' },
                ],
                headerCallback: function (thead, data, start, end, display) {
                    $(thead).parent().children().addClass('dtHeader');
                    $(thead).children().addClass('text-center');
                },
                createdRow: function (row, data, dataIndex) {

                    // if (temp != data.numeroEmpleado && temp != "") {
                    //     $(row).find('td').addClass('tdBorde');
                    // }
                    // temp = data.numeroEmpleado;
                }

            });
        }
        //#endregion


        //#region BACKEND
        function fncGetReporteEmpleadoCC(btm, tp, centroC, idEmpleado) {
            axios.post('GetReporteEmpleadoCC', { bottom: btm, top: tp, cc: centroC, empleado: idEmpleado, tipoRaya: cboFiltroTipoRaya.val() }).then(response => {
                let { success, items, message } = response.data;
                // console.log(response)
                if (success) {
                    // console.log(items)
                    initTblReporteAnualNomina(items);

                    addRows(tblReporteEmpleadoCC, items);
                    numHeadEmpleados.text(response.data.numEmpleados ?? 0);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetReporteConcentrado(fechaInicial, fechaFinal) {
            let obj = {
                fechaInicial: moment(fechaInicial),
                fechaFinal: moment(fechaFinal),
                tipoRaya: cboFiltroTipoRaya.val(),
                tipoNomina: cboFiltroTipoNomina.val()
            };
            axios.post("GetReporteConcentrado", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    initTblReporteAnualNomina(items);

                    addRows(tblReporteEmpleadoCC, response.data.lstConcetrado);
                    // numHeadEmpleados.text( (response.data.numEmpleados ?? 0));

                    response.data.numEmpleados.forEach(e => {
                        numHeadEmpleados.text(`${months[e.mesNom - 1]}: ${e.numEmpleados}`);

                    });
                }
            }).catch(error => Alert2Error(error.message));
        }

        function FillCombos() {
            cboxCentroCostos.fillCombo('/Nomina/GetCentroCostos', {}, false);
            cboxEmpleado.fillCombo('/Nomina/GetListaEmpleados', {}, false);
        }
        //#endregion

        //#region UTILS

        function addRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function monthDiff(d1, d2) {
            var months;
            months = (d2.getFullYear() - d1.getFullYear()) * 12;
            months -= d1.getMonth();
            months += d2.getMonth();
            return months <= 0 ? 0 : months;
        }
        function getMonth(date) {
            let nombreDelMes = "";

            nombreDelMes = months[date - 1];

            return nombreDelMes;
        }
        //#endregion

    }
    $(document).ready(() => {
        Nominas.GetReporteEmpleadoCC = new GetReporteEmpleadoCC();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();