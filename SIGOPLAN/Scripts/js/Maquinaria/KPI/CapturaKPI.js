
(() => {
    $.namespace('maquinaria.KPI.CapturaKpi');

    CapturaKpi = function () {

        let _CapturaDiaria;
        let _hoorasObra;
        // Variables de Filtros;
        const cboGrupoEquipo = $('#cboGrupoEquipo');
        const cboModeloEquipo = $('#cboModeloEquipo');
        const cboCC = $('#cboCC');
        const inputFecha = $('#inputFecha');
        const cboTurno = $('#cboTurno');

        //Tablas:
        const theadID = $('#theadID');
        const tbodyID = $('#tbodyID');

        //Botones
        const btnBuscar = $('#btnBuscar');
        const btnGuardar = $('#btnGuardar');

        //#region DATEPICKER VARIABLES.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        let horasDia = 24;
        //Generar Semana

        (function init() {
            // Lógica de inicialización.
            initCargas();
            initListeners();
            initObjects();
            // initTablaKpiGenerados();
        })();

        function initObjects() {

            inputFecha.datepicker({
                "dateFormat": "dd/mm/yy",
                "maxDate": new Date()
            }).datepicker("option", "showAnim", "slide")
                .datepicker("setDate", new Date());
        }

        function initCargas() {
            cboCC.fillCombo('/CatObra/cboCentroCostosUsuarios', null, false);
        }

        function initListeners() {
            btnBuscar.click(CargarCapturaDiaria);
            cboCC.change(LoadGrupos);
            cboGrupoEquipo.change(LoadModelos);
            btnGuardar.click(saveOrUpdateCapturaDiaria);
        }

        function LoadModelos() {
            cboModeloEquipo.fillCombo('/KPI/CboModeloEquipos', { grupoID: cboGrupoEquipo.val() });
        }

        function LoadGrupos() {
            $.post('/KPI/getHorasDia', { ac: cboCC.val() }).then(response => {horasDia = response.horasDia});
            cboGrupoEquipo.fillCombo('/KPI/CboGrupoEquipos', { areaCuenta: cboCC.val() });
        }

        // Métodos.
        function getBusqKpiDiaria() {
            return {
                ac: cboCC.val(),
                idModelo: cboModeloEquipo.val() ?? 0,
                idGrupo: cboGrupoEquipo.val() ?? 0,
                fecha: inputFecha.val(),
                turno: cboTurno.val()
            };
        }

        function CargarCapturaDiaria() {
            $.post('/KPI/GetCapturaDiaria', { busq: getBusqKpiDiaria() })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa
                        _CapturaDiaria = response.capturaDiaria;
            _hoorasObra = response.horasObra;
                        // builtTables(response.tblDescripciones, response.listaEconomicos, response.capturaDiaria, response.capturaHorometros);
                        builtTables(response.tblDescripciones, response.listaEconomicos, response.capturaDiaria);
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

        function porcentajes() {

            let ar = new Array();

            /* 
             Tipos Tiempos
             Tiempo Total Capturado
             Tiempo programado = 1;
             Tiempo Disponible = 2;
             Tiempo Operacion = 3;
             Tiempo Trabajo = 4;
             Disponibilidad  Fisica = 5;
             Uso Disponibilidad = 6;
             Utilizacion = 7;            
            */
            ar.push({ texto: 'Tiempo Programado', valor: horasDia, tipoGrupo: [0], tipos_tiempos: 1 });
            ar.push({ texto: 'Tiempo Disponible', valor: 0, tipoGrupo: [4, 5], tipos_tiempos: 2 }); // 4 MP , 5 MNP 
            ar.push({ texto: 'Tiempo Operación', valor: 0, tipoGrupo: [3], tipos_tiempos: 3 });
            ar.push({ texto: 'Tiempo Trabajo', valor: 0, tipoGrupo: [2], tipos_tiempos: 4 });

            ar.push({ texto: 'Disponibilidad Fisica', valor: 0, tipoGrupo: [0], tipos_tiempos: 5 });
            ar.push({ texto: 'Uso de Disponibilidad', valor: 0, tipoGrupo: [0], tipos_tiempos: 6 });
            ar.push({ texto: 'Utilización', valor: 0, tipoGrupo: [0], tipos_tiempos: 7 });
            ar.push({ texto: 'Tiempo Total Capturado', valor: 0, tipoGrupo: [0], tipos_tiempos: 8 });

            return ar;
        }

        function builtTables(tbl1, tbl2, tbl3/*, tbl4*/) {
            let tr;
            $('#theadID').empty();
            $('#tbodyID').empty();

            let pFilas = tbl1;
            let pColumnas = tbl2;
            let titulos = true;
            let last = null;

            for (let index = 0; index < pFilas.length; index++) {
                if (pFilas[index].cod == '' && pFilas[index].tipoGrupo == 0) { //Crea los encabezados de la tabla tanto como las columnas.

                    tr = createTR(theadID);
                    tr.append(`<th colspan='2' class='clsEncabezado fix' >${pFilas[index].descripcion}</td>`);
                    //bindeo de grupos
                    if (index == 0)
                        tr.append(`<th colspan='${pColumnas.length}' class='clsEncabezadoColumnas '>${$("#cboGrupoEquipo option:selected").text()}</td > `);
                    else {
                        for (let index2 = 0; index2 < pColumnas.length; index2++) {

                            if (pFilas[index].descripcion == "Horometros") {
                                let newTD = document.createElement("td");
                                let newInput = document.createElement("input");
                                tr.append(newTD);
                                $(newTD).append(newInput);
                                $(newInput).addClass('form-control').addClass('inputHorometro');
                                $(newInput).data().economico = pColumnas[index2].economico;

                                //const rebels = tbl4.filter(economico => economico.Economico == pColumnas[index2].economico);
                                if (pColumnas[index2].tieneRegistros) {
                                    $(newInput).prop('disabled', true);
                                    //let capHorometro = rebels.filter(r => r.turno == cboTurno.val() && moment(r.Fecha).format('DD/MM/YYYY') == inputFecha.val())[0];
                                    //if (capHorometro != null) {
                                    //    $(newInput).val(capHorometro.HorasTrabajo);
                                    //}
                                    if (pColumnas[index2].horometro != null) {
                                        $(newInput).val(pColumnas[index2].horometro);
                                    }
                                }

                            }
                            else {
                                if (index == 1)
                                    tr.append(`<th class='clsEncabezadoColumnas' style="width: 350px;"> ${pColumnas[index2].modelo}</td> `);
                                else
                                    tr.append(`<th class='clsEncabezadoColumnas' style="width: 350px;"> ${pColumnas[index2].economico}</td> `);
                            }
                        }
                    }
                }
                else {
                    tr = createTR(tbodyID);
                    if (titulos) {
                        //Columnas de los codigos.
                        tr.append(`<th class='clsEncabezado fix'>COD</td > `);
                        tr.append(`<th class='clsEncabezado fix'>Descripción</td > `);
                        tr.append(`<td colspan = '${pColumnas.length}' class='clsEncabezado'> </td > `);
                        titulos = false;
                    }
                }
            }
            let dataBind = tbl1.filter(function (cods) {
                return cods.tipoGrupo != 0;
            });
            let rest = false;
            for (let index = 0; index < dataBind.length; index++) {
                tr = createTR(tbodyID);
                if (last !== dataBind[index].tipoGrupo) {
                    //Este es el separador de los tipos de codigo
                    tr.append(`<th colspan='2' class='clsEncabezadoCodigos fix'> ${dataBind[index].descripcionGrupo}</td >`);
                    tr.append(`<td colspan='${pColumnas.length}' class='tdBlanco'></td >`);
                    reset = true;
                    last = dataBind[index].tipoGrupo;
                    if (reset) {
                        index = index - 1;
                        reset = false;
                    }
                }
                else {
                    if (dataBind[index].cod == '') {
                        //Esta es la seccion de los totales.
                        tr.append((`<th colspan='2' class='clsTotales '> ${dataBind[index].descripcion}</td >`));
                        for (let index2 = 0; index2 < pColumnas.length; index2++) {
                            let valorTotal = totalesColumnas(dataBind[index].tipoGrupo, pColumnas[index2].economicoID);
                            tr.append(`<td> <input class="form-control clsTotales totalValue" data-tipo='${dataBind[index].tipoGrupo}' name='${pColumnas[index2].economico}' value='${valorTotal}' disabled/></td >`);
                        }
                    }
                    else {
                        //Esta primera parte es la descripcion y codigo del valor.
                        tr.append(`<th class='fix'> ${dataBind[index].cod}</td > `);
                        tr.append(`<th class='fix'> ${dataBind[index].descripcion}</td > `);
                        //Se encarga de rellenar los valores
                        for (let index2 = 0; index2 < pColumnas.length; index2++) {
                            let newTD = document.createElement("td");
                            let newInput = document.createElement("input");
                            tr.append(newTD);
                            $(newTD).append(newInput);
                            $(newInput).addClass('form-control').addClass('inputCaptura');
                            const rebels = tbl3.filter(economico => economico.idEconomico == pColumnas[index2].economicoID && dataBind[index].coidID == economico.idParo && economico.turno == cboTurno.val());
                            $(newInput).data().economico = pColumnas[index2].economico;
                            $(newInput).data().economicoID = pColumnas[index2].economicoID;
                            $(newInput).data().codigoID = dataBind[index].coidID;
                            $(newInput).data().codigo = dataBind[index].cod;
                            $(newInput).data().modeloID = pColumnas[index2].modeloID;
                            $(newInput).data().grupoID = pColumnas[index2].grupoID;
                            $(newInput).data().id = pColumnas[index2].id;
                            $(newInput).data().tipoGrupo = dataBind[index].tipoGrupo;

                            if (rebels.length > 0) {
                                $(newInput).val(rebels[0].valor);
                                $(newInput).data().idCaptura = (rebels[0].id);
                            }
                        }
                    }
                }
            }

            let infoTotales = porcentajes();

            for (let index = 0; index < infoTotales.length; index++) {
                tr = createTR(tbodyID);
                tr.append((`<th colspan='2'> ${infoTotales[index].texto}</td >`));
                for (let index2 = 0; index2 < pColumnas.length; index2++) {
                    let newTD = document.createElement("td");
                    let newInput = document.createElement("input");
                    $(newInput).addClass('form-control').addClass('inputConTotales').prop('disabled', true);

                    let tipos_tiempos = infoTotales[index].tipos_tiempos;
                    $(newInput).data().economico = pColumnas[index2].economico;
                    $(newInput).data().economicoID = pColumnas[index2].economicoID;
                    $(newInput).data().tipos_tiempos = tipos_tiempos;

                    tr.append(newTD);
                    $(newTD).append(newInput);

                    switch (tipos_tiempos) {
                        case 8: //Tiempo Capturado
                            {
                                $(newInput).addClass('inputTotalCapturado');
                                $(newInput).val(infoTotales[index].valor);
                                fnSetTotalCapturado(pColumnas[index2].economicoID)
                            }
                            break;
                        case 1: //Tiempo Programado
                            $(newInput).val(infoTotales[index].valor);
                            break;
                        case 2: // Tiempo Disponible.
                            {
                                let arrayTiposGrupos = infoTotales[index].tipoGrupo;
                                let totalMTTOS = 0; //MP Mantenimiento Programado  y no Programado Suma;
                                let inputConTotalesV = 0;

                                arrayTiposGrupos.forEach(tipoGrupo => {
                                    totalMTTOS += totalesColumnas(tipoGrupo, pColumnas[index2].economicoID)
                                });
                                $('.inputConTotales').toArray().forEach(element => {
                                    if (pColumnas[index2].economicoID == $(element).data().economicoID && 1 == $(element).data().tipos_tiempos) {
                                        inputConTotalesV = (+$(element).val());
                                    }
                                });

                                $(newInput).val((inputConTotalesV - totalMTTOS).toFixed(2));

                            }
                            break;
                        case 3: /// Tiempo Operacion
                            {
                                let objtTipoGrupo = infoTotales[index].tipoGrupo[0];
                                let totalesOperacion = totalesColumnas(objtTipoGrupo, pColumnas[index2].economicoID);
                                let totalesDisponible = 0;
                                $('.inputConTotales').toArray().forEach(element => {
                                    if (pColumnas[index2].economicoID == $(element).data().economicoID && 2 == $(element).data().tipos_tiempos) {
                                        totalesDisponible = (+$(element).val());
                                    }
                                });
                                $(newInput).val((totalesDisponible - totalesOperacion).toFixed(2));
                            }
                            break;

                        case 4: // Tiempo de Trabajo
                            {
                                let objtTipoGrupo = infoTotales[index].tipoGrupo[0];
                                let sustraendo = totalesColumnas(objtTipoGrupo, pColumnas[index2].economicoID);
                                let minuendo = 0;
                                $('.inputConTotales').toArray().forEach(element => {
                                    if (pColumnas[index2].economicoID == $(element).data().economicoID && 3 == $(element).data().tipos_tiempos) {
                                        minuendo = (+$(element).val());
                                    }
                                });
                                $(newInput).val((minuendo - sustraendo).toFixed(2));
                            }
                            break;
                        case 5: // Disponibilidad Fisica
                            {
                                let programado = 0;
                                let disponible = 0;
                                $('.inputConTotales').toArray().forEach(element => {
                                    if (pColumnas[index2].economicoID == $(element).data().economicoID && 1 == $(element).data().tipos_tiempos) {
                                        programado = (+$(element).val());
                                    }
                                });
                                $('.inputConTotales').toArray().forEach(element => {
                                    if (pColumnas[index2].economicoID == $(element).data().economicoID && 2 == $(element).data().tipos_tiempos) {
                                        disponible = (+$(element).val());
                                    }
                                });
                                let resultado = (disponible / programado) * 100;
                                $(newInput).val(resultado.toFixed(2));
                            }
                            break;
                        case 6: // Uso de la disponibilidad
                            {
                                let mtto = 0;
                         
                                $('.inputConTotales').toArray().forEach(element => {
                                    if (columna == $(element).data().economicoID && 4 == $(element).data().tipos_tiempos) {
                                        mtto = (+$(element).val());
                                    }
                                });

                                let resultado = ((_horasObra - mtto) / _horasObra) * 100;
                                $(element).val(resultado.toFixed(2));
                            }
                            break;
                        case 7: // Utillizacion
                            {
                                let trabajo = 0;
                                let disponible = 0;
                                $('.inputConTotales').toArray().forEach(element => {
                                    if (pColumnas[index2].economicoID == $(element).data().economicoID && 4 == $(element).data().tipos_tiempos) {
                                        trabajo = (+$(element).val());
                                    }
                                });
                                let mtto = 0;
                         
                                $('.inputConTotales').toArray().forEach(element => {
                                    if (columna == $(element).data().economicoID && 4 == $(element).data().tipos_tiempos) {
                                        mtto = (+$(element).val());
                                    }
                                });

                                let resultado = (trabajo / (_horasObra - mtto)) * 100;
                                $(newInput).val(resultado.toFixed(2));
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            //    $('.inputCaptura').trigger('change');
        }

        function fnSetTotalCapturado(economicoID) {

            let totalActual = $('.inputCaptura').toArray().filter(e => $(e).data().economicoID == economicoID).map(dato => +$(dato).val()).reduce((acum, valor) => acum + valor, 0);
            let inputTotalCapturado = $('.inputTotalCapturado').toArray().filter(element => $(element).data().economicoID == economicoID);
            let totalRestante = _CapturaDiaria.filter(r => r.idEconomico == economicoID && r.turno != cboTurno.val()).reduce((acc, e) => acc + e.valor, 0);
            $(inputTotalCapturado).val((totalActual + totalRestante).toFixed(2));
            return totalActual + totalRestante;

        }

        function totalesColumnas(tipoGrupo, economicoID) {
            let total = 0;
            $('.inputCaptura').toArray().forEach(element => {
                if (economicoID == $(element).data().economicoID && tipoGrupo == $(element).data().tipoGrupo) {
                    total += (+$(element).val());
                }
            });
            return total;
        }

        $('#tlbNew').on('change', '.inputCaptura', function () {
            let columna = $(this).data().economicoID;
            let tipo = $(this).data().tipoGrupo;
            let economico = $(this).data().economico;
            let total = 0;
            let totalValida = 0;
            // totalValida += totalDia;
            let validHorasTrabajadas = (+$('.inputHorometro').toArray().filter(e => $(e).data().economico == economico).map(dato => $(dato).val())[0]);
            totalValida = fnSetTotalCapturado(columna);

            $('.inputCaptura').toArray().forEach(element => {
                if (columna == $(element).data().economicoID && tipo == $(element).data().tipoGrupo) {
                    total += +$(element).val();
                }
            });

            $('input[name ="' + economico + '"]').toArray().forEach(element => {
                if (tipo == $(element).attr('data-tipo')) {
                    $(element).val(total.toFixed(2));
                }
            });
            let infoTotales = new Array();

            /* 
             Tipos Tiempos
             Tiempo programado = 1;
             Tiempo Disponible = 2;
             Tiempo Operacion = 3;
             Tiempo Trabajo = 4;
             Disponibilidad  Fisica = 5;
             Uso Disponibilidad = 6;
             Utilizacion = 7;            
            */

            infoTotales.push({ texto: 'Tiempo Programado', valor: horasDia, tipoGrupo: [0], tipos_tiempos: 1 });
            infoTotales.push({ texto: 'Tiempo Disponible', valor: 0, tipoGrupo: [4, 5], tipos_tiempos: 2 }); // 4 MP , 5 MNP 
            infoTotales.push({ texto: 'Tiempo Operación', valor: 0, tipoGrupo: [3], tipos_tiempos: 3 });
            infoTotales.push({ texto: 'Tiempo Trabajo', valor: 0, tipoGrupo: [2], tipos_tiempos: 4 });

            infoTotales.push({ texto: 'Disponibilidad Fisica', valor: 0, tipoGrupo: [0], tipos_tiempos: 5 });
            infoTotales.push({ texto: 'Uso de Disponibilidad', valor: 0, tipoGrupo: [0], tipos_tiempos: 6 });
            infoTotales.push({ texto: 'Utilización', valor: 0, tipoGrupo: [0], tipos_tiempos: 7 });


            // let infoTotales = porcentajes();
            fnOperacionesTotales(columna);

            if (tipo == 1) {
                if (validHorasTrabajadas == 0) {
                    $(this).val(0);
                    let element = $('input[name ="' + economico + '"]').toArray().filter(element => { return $(element).attr('data-tipo') == tipo }).map(f => { return $(f) })[0];
                    $(element).val(totalesColumnas(tipo, columna));
                    fnOperacionesTotales(columna);
                    return AlertaGeneral('Alerta', `Las horas trabajadas del equipo ${economico} son 0`);
                }
                if ($(this).val() > validHorasTrabajadas) {
                    $(this).val(0);
                    let element = $('input[name ="' + economico + '"]').toArray().filter(element => { return $(element).attr('data-tipo') == tipo }).map(f => { return $(f) })[0];
                    $(element).val(totalesColumnas(tipo, columna));
                    fnOperacionesTotales(columna);
                    return AlertaGeneral('Alerta', 'Las horas sobrepasan las horas de trabajo capturadas.')
                }
            }

                if (totalValida > horasDia) {
                $(this).val(0);
                let element = $('input[name ="' + economico + '"]').toArray().filter(element => { return $(element).attr('data-tipo') == tipo }).map(f => { return $(f) })[0];
                $(element).val(totalesColumnas(tipo, columna));
                fnOperacionesTotales(columna);
                fnSetTotalCapturado(columna, $('.inputCaptura').toArray().filter(e => $(e).data().economico == economico).map(dato => +$(dato).val()).reduce((acum, valor) => acum + valor, 0));
                return AlertaGeneral('Alerta', 'La captura no puede ser Mayor a '+horasDia);
            }
        });

        function fnOperacionesTotales(columna) {

            let infoTotales = porcentajes();

            let inputConTotalesList = $('.inputConTotales').toArray().filter(function (economico) {
                if ($(economico).data().economicoID === columna)
                    return economico;
            })

            inputConTotalesList.forEach(element => {
                let tipos_tiempos = $(element).data().tipos_tiempos;

                switch (tipos_tiempos) {
                    case 2: // Tiempo Disponible.
                        {
                            let arrayTiposGrupos = infoTotales[1].tipoGrupo;
                            let totalMTTOS = 0; //MP Mantenimiento Programado  y no Programado Suma;
                            let inputConTotalesV = 0;

                            arrayTiposGrupos.forEach(tipoGrupo => {
                                totalMTTOS += totalesColumnas(tipoGrupo, columna)
                            });

                            $('.inputConTotales').toArray().forEach(element => {
                                if (columna == $(element).data().economicoID && 1 == $(element).data().tipos_tiempos) {
                                    inputConTotalesV = (+$(element).val());
                                }
                            });

                            $(element).val((inputConTotalesV - totalMTTOS).toFixed(2));
                        }

                        break;
                    case 3: /// Tiempo Operacion
                        {
                            let objtTipoGrupo = infoTotales[2].tipoGrupo[0];
                            let totalesOperacion = totalesColumnas(objtTipoGrupo, columna);
                            let totalesDisponible = 0;
                            $('.inputConTotales').toArray().forEach(element => {
                                if (columna == $(element).data().economicoID && 2 == $(element).data().tipos_tiempos) {
                                    totalesDisponible = (+$(element).val());
                                }
                            });
                            $(element).val((totalesDisponible - totalesOperacion).toFixed(2));
                        }
                        break;

                    case 4: // Tiempo de Trabajo
                        {
                            let objtTipoGrupo = infoTotales[3].tipoGrupo[0];
                            let sustraendo = totalesColumnas(objtTipoGrupo, columna);
                            let minuendo = 0;
                            $('.inputConTotales').toArray().forEach(element => {
                                if (columna == $(element).data().economicoID && 3 == $(element).data().tipos_tiempos) {
                                    minuendo = (+$(element).val());
                                }
                            });
                            $(element).val((minuendo - sustraendo).toFixed(2));
                        }
                        break;
                    case 5:
                        {
                            let programado = 0;
                            let disponible = 0;
                            $('.inputConTotales').toArray().forEach(element => {
                                if (columna == $(element).data().economicoID && 1 == $(element).data().tipos_tiempos) {
                                    programado = (+$(element).val());
                                }
                            });
                            $('.inputConTotales').toArray().forEach(element => {
                                if (columna == $(element).data().economicoID && 2 == $(element).data().tipos_tiempos) {
                                    disponible = (+$(element).val());
                                }
                            });
                            let resultado = (disponible / programado) * 100;
                            $(element).val(resultado.toFixed(2));
                        }
                        break;
                    case 6:
                        {
                            let mtto = 0;
                         
                            $('.inputConTotales').toArray().forEach(element => {
                                if (columna == $(element).data().economicoID && 4 == $(element).data().tipos_tiempos) {
                                    mtto = (+$(element).val());
                                }
                            });

                        let resultado = ((_horasObra - mtto) / _horasObra) * 100;
                            $(element).val(resultado.toFixed(2));
                        }
                        break;
                    case 7:
                        {
                                let trabajo = 0;
     
                                $('.inputConTotales').toArray().forEach(element => {
                                    if (pColumnas[index2].economicoID == $(element).data().economicoID && 4 == $(element).data().tipos_tiempos) {
                                        trabajo = (+$(element).val());
                                    }
                                });
                                let mtto = 0;
                         
                                $('.inputConTotales').toArray().forEach(element => {
                                    if (columna == $(element).data().economicoID && 4 == $(element).data().tipos_tiempos) {
                                        mtto = (+$(element).val());
                                    }
                                });

                                let resultado = (trabajo / (_horasObra - mtto)) * 100;
                                $(newInput).val(resultado.toFixed(2));
                            }
                        break;
                    default:
                        break;
                }
            });
        }

        $('#tlbNew').on('change', '.inputHorometro', function () {
            let horometro = +$(this).val();
            if (horometro > horasDia) {
                AlertaGeneral('Alerta', 'No se puede hacer una captura mayor a '+horasDia+' hrs');
                $(this).val(0);
            }
        });

        function saveOrUpdateCapturaDiaria() {
            let captura = getInputCaptura();
            let validacionDeCaptura = fnValidacionCaptura(captura);
            btnGuardar.prop('disabled', true);
            if (validacionDeCaptura.valido) {
                $.post('/KPI/saveOrUpdateCapturaDiaria', { captura: captura.kpi, horometros: captura.horometro })
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            CargarCapturaDiaria();
                            AlertaGeneral(`Confirmacion`, `Guardado Finalizado`);
                            btnGuardar.prop('disabled', false);
                        } else {
                            // Operación no completada.
                            AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message} `);
                            btnGuardar.prop('disabled', false);
                            CargarCapturaDiaria();
                        }
                    }, error => {
                        // Error al lanzar la petición.
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        btnGuardar.prop('disabled', false);
                        CargarCapturaDiaria();
                    }
                    );
            }
            else {
                AlertaGeneral(`Operación fallida`, `Error ${validacionDeCaptura.statusText}.`);
                btnGuardar.prop('disabled', false);
            }
        }

        function fnValidacionCaptura(groupBy) {

            var objCapturas = $('.inputCaptura').toArray().map(element => {
                return {
                    id: $(element).data().idCaptura,
                    fecha: $(inputFecha).val(),
                    turno: $(cboTurno).val(),
                    ac: $(cboCC).val(),
                    codigoParo: $(element).data().codigo,
                    valor: $(element).val(),
                    idParo: $(element).data().codigoID,
                    idGrupo: $(element).data().grupoID,
                    idModelo: $(element).data().modeloID,
                    idEconomico: $(element).data().economicoID,
                    economico: $(element).data().economico,
                    año: 0,
                    semana: 0,
                    idTipoParo: $(element).data().tipoGrupo
                }
            });

            const groupByEconomicos = fnGroupBy("economico", objCapturas);
            let grupoEconomicos = groupByEconomicos(groupBy);
            let msj = "";
            let count = 0;
            let mensaje = "Los Siguientes Economicos :"
            for (var economicoPpal in grupoEconomicos) {

                let totalTrabajo = grupoEconomicos[economicoPpal].reduce((acc, economico) => acc + (+economico.valor), 0);
                let inputHorometro = $('.inputHorometro').toArray().filter(economico => $(economico).data().economico == economicoPpal);
                let valorHorometro = +$(inputHorometro).val();
                let resta = valorHorometro - totalTrabajo.toFixed(2);
                if (totalTrabajo < valorHorometro) {
                    msj += `${economicoPpal},`;
                    count++;
                }
            }
            mensaje += msj + " Faltan Horas de completar en Tiempos de Trabajo";

            return { valido: count == 0 ? true : false, statusText: mensaje }
        }

        function fnGroupBy(key, array) {
            return function group(array) {
                return array.kpi.reduce((acc, obj) => {
                    const property = obj[key];
                    acc[property] = acc[property] || [];
                    acc[property].push(obj);
                    return acc;
                }, {});
            };
        }

        function getInputCaptura() {
            let objCapturas = {};

            objCapturas.kpi = $('.inputCaptura').toArray().map(element => {
                return {
                    id: $(element).data().idCaptura,
                    fecha: $(inputFecha).val(),
                    turno: $(cboTurno).val(),
                    ac: $(cboCC).val(),
                    codigoParo: $(element).data().codigo,
                    valor: $(element).val(),
                    idParo: $(element).data().codigoID,
                    idGrupo: $(element).data().grupoID,
                    idModelo: $(element).data().modeloID,
                    idEconomico: $(element).data().economicoID,
                    economico: $(element).data().economico,
                    año: 0,
                    semana: 0,
                    idTipoParo: $(element).data().tipoGrupo
                }
            });

            objCapturas.horometro = $('.inputHorometro').toArray().map(element => {
                return {
                    CC: $(cboCC).val(),
                    Economico: $(element).data().economico,
                    HorasTrabajo: $(element).val(),
                    Horometro: 0,
                    HorometroAcumulado: 0,
                    Desfase: 0,
                    Fecha: $(inputFecha).val(),
                    Turno: $(cboTurno).val(),
                    Ritmo: $(element).prop('disabled')
                }
            });

            return objCapturas;
        }

        function createTR(element) {
            let newTR = document.createElement("tr");
            element.append(newTR);
            return $(newTR);
        }

    }
    $(() => maquinaria.KPI.CapturaKpi = new CapturaKpi())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();


