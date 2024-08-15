(() => {
    $.namespace('Nominas.GetReporteCtroCta');

    //#region Componentes
    //Variables jquery

    const inMinDate = $('#min'),
        inMaxDate = $('#max'),
        btnSetRango = $('#btnSetRango'),
        btnExportar = $('#btnExportar'),
        tblReporteCentroCuenta = $('#tblReporteCentroCuenta'),
        spanEmpleadosNum = $('#empleadosNum');
        
    //#endregion

    //#region Variables locales
    
    let dtReporteCentroCuenta,
        dateNow = new Date(),
        añoActual = dateNow.getFullYear(),
        mesActual = dateNow.getMonth(),
        ultimoDiaDelMes = moment(new Date(añoActual, mesActual, 1)).endOf('month').format('DD'),
        lstDetalleAnual = [{
        data: '',
        title: ''
    }];
    let months = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];

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


    GetReporteCtroCta = function () {
        (function init() {
            //#region Date Picker
            inMinDate.datepicker({
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                dateFormat: "dd/mm/yy",
                yearRange: '2018:c',
                onClose: function (dateText, inst) {
                    $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
                }
            });
            inMaxDate.datepicker({
                dateFormat: "dd/mm/yy",
                changeYear: true,
                changeMonth: true,
                showButtonPanel: true,
                yearRange: '2018:c',
                onClose: function (dateText, inst) {
                    function isDonePressed() {
                        return ($('#ui-datepicker-div').html().indexOf('ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all ui-state-hover') > -1);
                    }

                    if (isDonePressed()) {
                        var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                        var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                        var day = moment(new Date(year, month, 1)).endOf('month').format('DD');
                        $(this).datepicker('setDate', new Date(year, month, day)).trigger('change');
                        $(this).focusout()//Added to remove focus from datepicker input box on selecting date
                    }
                }
            }).datepicker('setDate', new Date(añoActual, mesActual, ultimoDiaDelMes));
            //#endregion

            fncListeners();
        })();


        //#region Listeners
        function fncListeners() {

            //Boton de busqueda
            btnSetRango.on('click', function () {

                let strMensajeError = "";

                if (inMinDate.val() == "") {/* $("#select2-cboCrearEditarCC-container").css('border', '2px solid red');*/ strMensajeError = "Es necesario llenar los campos obligatorios."; }
                if (inMaxDate.val() == "") { /*$("#select2-cboCrearEditarCC-container").css('border', '2px solid red');*/ strMensajeError = "Es necesario llenar los campos obligatorios."; }

                if (strMensajeError != "") {
                    Alert2Warning(strMensajeError);
                    btnExportar.prop('disabled',true);
                    return "";
                } else {

                    consultado = true;
                    btnExportar.prop('disabled',false);

                    btmDate = inMinDate.val().split("/");
                    topDate = inMaxDate.val().split("/");

                    btmObjDate = new Date(parseInt(btmDate[2]), parseInt(btmDate[1]), parseInt(btmDate[0]));
                    topObjDate = new Date(parseInt(topDate[2]), parseInt(topDate[1]), parseInt(topDate[0]));

                    //Calcular cual es la diferencia entre el rango de meses de los inputs para obtener el numero de columnas
                    range = monthDiff(btmObjDate, topObjDate);

                    fncGetReporteCtroCta(`${btmDate[2]}-${btmDate[0]}-${btmDate[1]}`, `${topDate[2]}-${topDate[0]}-${topDate[1]}`);
                    fncGetNumEmpleados(`${btmDate[2]}-${btmDate[0]}-${btmDate[1]}`, `${topDate[2]}-${topDate[0]}-${topDate[1]}`);
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
        //#endregion

        //#region Inizalizacion de la tabla
        function initTblReporteAnualNomina(data, headers) {

            if (dtReporteCentroCuenta != null) {
                dtReporteCentroCuenta.destroy();
                tblReporteCentroCuenta.empty();
                tblReporteCentroCuenta.append('<thead class="bg-table-header"></thead>');

                //tblReporteCentroCuenta.append('<tfoot id = "tblFoot"><tr id = "tblFooter"><th></th><th>Totales</th></tr></tfoot>');
                
            }// }else{
            //     let htmlString = '<tfoot id = "tblFooter"><tr>';
            //     for (let i = 1; i <= data.length+1; i++) {
            //         htmlString += `<th class="text-right" rowspan="1" colspan="1" style="width: 173px;">x</th>`;
            //     }
            //     htmlString += `</tr></tfoot>`;
            //     tblReporteCentroCuenta.append(htmlString);
            // }
            
            //Insertar y llenar footer del tamaño de los headers
            let htmlString = '<tfoot><tr><th></th><th>Totales</th>';
            for (let i = 2; i <= data.length+2; i++) {
                htmlString += `<th>${i}</th>`;
            }
            htmlString += `</tr></tfoot>`;
            tblReporteCentroCuenta.append(htmlString);

            //Reinicializar las columnas
            dataArray = [];
            

            dtReporteCentroCuenta = tblReporteCentroCuenta.DataTable({
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
                columns: range > headers.length ? obtenerColumnar(headers) : obtenerColumnar(headers), //Crear columnas de la variable con respecto al rango : Crear columnas con respecto al numero de items en el query
                initComplete: function (settings, json) {
                },
                columnDefs: [

                    { className: 'dt-center', 'targets': '_all' },
                ],
                
                fixedColumns: {
                    leftColumns: 2,
                },
                headerCallback: function (thead, data, start, end, display) {
                    $(thead).parent().children().addClass('dtHeader');
                    $(thead).children().addClass('text-center');
                },
                createdRow: function (row, data, dataIndex) {
                    
                    let total = 0;
                    let invisibles = 0;

                    tblReporteCentroCuenta.DataTable().columns().every(function(colIdx, tableLoop, colLoop){
                        if (colIdx > 1) {
                            for (let x = 0; x < this.data().length; x++) {
                                total += Number(this.data()[x].split('$')[1].replaceAll(',',''));
                            }

                            if (this.column(colIdx).visible()) {
                                $(this.footer()).html(maskNumero(total));
                            }
                            else {
                                invisibles++;
                            }
                            total = 0;
                        }
                    });
                    
                },
                
            });

            //llenar la tabla
            
        }
        //#endregion

        //#region Crear columnas dinamicas
        function obtenerColumnar(data) {
            //Columnas
            let lst = [];

            //Primera columna: Concepto
            lst.push({ title: 'C.C', className: 'text-left' });

            lst.push({ title: 'Descripción', className: 'text-left' });

            //Crear columnas para el rango porporcionado
            for (let i = 0; i < data.length; i++) {
                let item = {
                    title: `${data[i].cta} ${data[i].concepto}`,
                    className: 'text-right'
                };
                lst.push(item);
                
            }
            lst.push({ title: 'Totales', className: 'text-right' });

            //Regresar columnas
            return lst;
        }
        //#endregion

        //#region Llenado y procesado de datos
        function fncCreadata(data) {
            //Numero de registros, numero de meses
            let registros = getYSize(data),
                dataRange = data.length,
                totalesY = [],
                totalXY = 0,
                iTotales;

            totalesY[0] = "Totales";

            //Si dataArray(variable que se llena solo si el rango de meses supera el de data) tiene valores se la diferencia del rango
            if (dataArray.length != 0) {
                dataRange += (dataArray.length - dataRange);
            }

            dataRange -= 1;
            //Ciclo para el # de registros a llenar dentro del DT
            for (let j = 0; j < registros; j++) {
                //Añadir total de los montos del mes al ultimo registro

                //Registro/Tupla
                let item = [];
                //Indice para recorrer el ciclo segun el rango izq->derecha(Febrero2021-Diciembre2021|Marzo2021-Diciembre2021)
                let iRecorrer = 2,//Iterador para recorer para el segmento ELSE
                    iiRecorrer = 1,//Iterador para recorer para el segmento IF
                    //indice para recorrel el ciclo segun el dataRange
                    ii = 0;//Iterador para recorer para el segmento IF
                //Ciclo para rango fechas/Columnas
                for (let i = 0; i <= dataRange; i++) {
                    //Si. la query tiene datos y el rango es mayor al numero de meses en la query se llena el DT con respecto al rango calculado al principio de metodo.
                    //Si no. Llena el DT con respecto a la data del query.
                    if (data[ii] != null && dataArray.length != 0) {

                        //Si los el mes y el año de la columna coincide con un registro de data -> llena el registro con el valor
                        if (dataArray[i].cta == data[ii].cta ) {

                            //Obtener el indice donde se empieza a llenar el query 
                            if (iTotales == undefined) {
                                iTotales = i;
                            }

                            if (data[ii].lstCentros.length != 0) {
                                //Checa si el concepto del monto existe
                                if (ii == 0 || item[0] == 0 || item[0] == null) {
                                    if (data[ii].lstCentros[j]) {
                                        item[0] = data[ii].lstCentros[j].cc;
                                    } else {
                                        item[0] = 0;
                                    }
                                }

                                //Checa si la descripcion del monto existe
                                if (ii == 0 || item[1] == 0 || item[1] == null) {
                                    if (data[ii].lstCentros[j]) {
                                        item[1] = data[ii].lstCentros[j].nombreCC;
                                    } else {
                                        item[1] = 0;
                                    }
                                }

                                //Llena los montos con su valor por rango o con 0 si no hay monto para dicha cuenta
                                if (data[ii].lstCentros[j]) {
                                    if (iiRecorrer == 1) {
                                        item[i] = "0.00";
                                        item[1 + i] = "0.00";
                                        iiRecorrer++;
                                    }
                                    item[iiRecorrer + i] = data[ii].lstCentros[j].monto;

                                } else {
                                    item[iiRecorrer + i] = "0.00";
                                }
                                ii++;
                            } else {
                                iiRecorrer--;
                            }
                        } else {
                            if (i == 0) {
                                item[0] = "0.00";
                            }
                            item[1 + i] = "0.00";
                            //iiRecorrer--; 
                        }
                    } else {
                        //Checa si la fecha tiene cuentas y montos
                        if (data[i] != undefined) {
                            //Obtener el indice donde se empieza a llenar el query 
                            if (iTotales == undefined) {
                                iTotales = i;
                            }
                            //Checa si el nombre del centro del monto existe
                            if (i == 0 || item[0] == 0 || item[0] == null) {
                                if (data[i].lstCentros[j]) {
                                    item[0] = data[i].lstCentros[j].cc;
                                } else {
                                    item[0] = 0;
                                }
                            }

                            //Checa si la descripcion del monto existe
                            if (i == 0 || item[1] == 0 || item[1] == null) {
                                if (data[i].lstCentros[j]) {
                                    item[1] = data[i].lstCentros[j].nombreCC;
                                } else {
                                    item[1] = 0;
                                }
                            }

                            //Llena los montos con su valor por rango o con 0 si no hay monto para dicha cuenta
                            if (data[i].lstCentros[j]) {
                                item[iRecorrer + i] = data[i].lstCentros[j].monto;

                            } else {
                                item[iRecorrer + i] = "0.00";

                            }

                        } else {
                            iRecorrer--;
                            item[1 + i] = "0.00";
                        }
                    }

                }

                
                let sum = 0;
                for (let i = 2; i <= item.length - 1; i++) {
                    let numVal = 0;
                    if (item[i] != 0) {
                        numVal = Number(item[i]);
                        sum += numVal;
                        
                    }  
                    item[i] = formatter.format(item[i]); 
                }
                item.push(formatter.format(sum));

                //Agregar registros
                dtReporteCentroCuenta.row.add(item).draw(false);

                
            }

            //Se crea el ultimo registro de la tabla. Totales por cuenta.
            let ii = 0;
            for (let i = 0; i < data.length; i++) {

                if (data[ii] != undefined) {
                    totalesY.push(data[ii].total);
                    totalXY += Number(data[ii].total.split('$')[1].replaceAll(',', ''));
                    ii++;
                    
                } else {
                    totalesY.push("$0.00");
                }
                
            }
            totalesY.push(formatter.format(totalXY));
            footerData = totalesY;

            //$('#idfotter').remove();
            // for (let index = 0; index < data.length; index++) {
            //     console.log($('#footer' + index).html());
            //     $('#footer' + index).text(footerData[index]);
            // }
            

            // $('#tblFooter').remove();
            // let htmlString = '<tfoot id = "tblFooter"><tr>';
            // for (let i = 1; i <= data.length+1; i++) {
            //     htmlString += `<th class="text-right" rowspan="1" colspan="1" style="width: 173px;">${footerData[i]}</th>`;
            // }
            // htmlString += `</tr></tfoot>`;
            // $('#tblFooter').append(htmlString);
        }

        //BACK END
        function fncGetReporteCtroCta(btm, tp) {
            axios.post('GetReporteCtroCta', { bottom: btm, top: tp }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //console.log(items);
                    initTblReporteAnualNomina(items.tblData,items.tblHeaders);
                    fncCreadata(items.tblData);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetNumEmpleados(btm, tp){
            axios.post('GetNumEmpleados', { bottom: btm, top: tp }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    spanEmpleadosNum.text(items);
                    numEmpleados = items;
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

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
                dataSizes[i] = data[i].lstCentros.length;
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

        //#region Excel
        function saveData() {
            let dtData = [],
                dtTabla = dtReporteCentroCuenta.data().toArray(),
                dtHeaders = [];

            //Aumentar rango para las columnas de totales y conceptos
            for (let i = 0; i < dtTabla[0].length; i++) {
                dtHeaders[i] = dtReporteCentroCuenta.column(i).header().textContent;
            }

            if (!dtHeaders.includes("Totales")) {
                dtHeaders.push("Totales")
            }

            dtData = dtData.concat([dtHeaders]);
            dtData = dtData.concat(dtTabla);
            if (footerData[1] != "--") {
                footerData.splice(1, 0, "--");
            }
            dtData = dtData.concat([footerData]);

            //Añadir # de empleados al fondo del excel
            //dtData = dtData.concat([["Empleados",numEmpleados.toString()]]);
            
            axios.post('creaVariableDeSesionCentroCuenta', { lstReporte: JSON.stringify(dtData),btm:`${btmDate[0]} de ${getMonth(btmDate[1])} ${btmDate[2]}`, tp:`${topDate[0]} de ${getMonth(topDate[1])} ${topDate[2]}`}).then(response => {
                location.href = '/Administrativo/Nomina/crearReporteCentroCuenta';
                let { success, items, message } = response.data;
                if (success) {
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

    }
    $(document).ready(() => {
        Nominas.GetReporteCtroCta = new GetReporteCtroCta();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();