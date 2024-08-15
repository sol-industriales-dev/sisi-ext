(() => {
    $.namespace('Barrenacion.PiezasBarrenadora');
    PiezasBarrenadora = function () {

        let _barrenadoraID;
        const cbPiezasTotales = $("#cbPiezasTotales");
        const comboAC = $('#comboAC');
        const comboEstatus = $('#comboEstatus');
        const botonBuscar = $('#botonBuscar');
        const tablaEquipos = $('#tablaEquipos');
        const modalPiezas = $('#modalPiezas');
        const inputNoEconomico = $('#inputNoEconomico');
        const inputNombreEquipo = $('#inputNombreEquipo');
        const botonGuardarPiezas = $('#botonGuardarPiezas');

        const divRadioBroca = $('#divRadioBroca');
        const divRadioMartillo = $('#divRadioMartillo');
        const divRadioBarra = $('#divRadioBarra');
        const divRadioBarraSegunda = $('#divRadioBarraSegunda');
        const divRadioCulata = $('#divRadioCulata');
        const divRadioCilindro = $('#divRadioCilindro');

        const comboInsumosBroca = $('#comboInsumosBroca');
        const comboInsumosMartillo = $('#comboInsumosMartillo');
        const inputSerieMartillo = $('#inputSerieMartillo');
        const inputDesechoMartillo = $('#inputDesechoMartillo');
        const comboInsumosBarra = $('#comboInsumosBarra');
        const comboInsumosBarraSegunda = $('#comboInsumosBarraSegunda');
        const inputDesechoBarra = $('#inputDesechoBarra');
        const inputDesechoBarraSegunda = $('#inputDesechoBarraSegunda');
        const comboInsumosCulata = $('#comboInsumosCulata');
        const comboInsumosCilindro = $('#comboInsumosCilindro');
        const spnReparadoMartillo = $('#spnReparadoMartillo');
        const comboPiezasCulata = $('#comboPiezasCulata');
        let dtTablaEquipos;
        const comboPiezasBroca = $('#comboPiezasBroca');
        const comboPiezasMartillo = $('#comboPiezasMartillo');
        const comboPiezasBarra = $('#comboPiezasBarra');
        const comboPiezasBarraSegunda = $('#comboPiezasBarraSegunda');
        const comboPiezasCilindro = $('#comboPiezasCilindro');
        const comboPiezasZanco = $('#comboPiezasZanco');
        const comboInsumosZanco = $("#comboInsumosZanco");
        const divRadioZanco = $('#divRadioZanco');
        const inputPzasCompletas = $('#inputPzasCompletas');


        // Agregar barrenadora
        const botonModalBarrenadora = $('#botonModalBarrenadora');
        const modalAgregarBarrenadora = $('#modalAgregarBarrenadora');
        const inputNoEconomicoBarrenadora = $('#inputNoEconomicoBarrenadora');
        const botonAgregarBarrenadora = $('#botonAgregarBarrenadora');

        const ObtenerSerieMartilloReparadoNoAsignado = new URL(window.location.origin + '/Barrenacion/ObtenerSerieMartilloReparadoNoAsignado');

        //#region 30012021 se añadio codigo nuevo para sustituir la antigua forma de guardado de la información
        const modalGestionPiezas = $('#modalGestionPiezas');
        const divCulata = $("#divCulata");
        const divCilindro = $('#divCilindro');
        const botonEditCulata = $("#botonEditCulata");
        const botonEditCilindro = $("#botonEditCilindro");
        const divMartillo = $("#divMartillo");
        let listaInsumosTempEdit = [];

        let _listaPiezas = [];
        let listaInsumosSalida = [];
        const btnGuardarPiezas = $("#btnGuardarPiezas");

        //#endregion

        const btnEditInsumo = $(".btnEditInsumo");
        (function init() {

            comboAC.fillCombo('/Barrenacion/ObtenerAC', null, false, null);
            // cargarEquipos();
            agregarListeners();
            cargarTablaEquipos();
            divRadioBroca.hide();
            divRadioMartillo.hide();
            divRadioBarra.hide();
            divRadioBarraSegunda.hide();
            divRadioCulata.hide();
            divRadioCilindro.hide();
            divRadioZanco.hide();

            inputDesechoMartillo.hide();
            inputDesechoBarra.hide();
            inputDesechoBarraSegunda.hide();

            initEventListener();

        })();


        function initEventListener() {
            btnEditInsumo.click(editInsumo);
            $("input:checkbox").on('click', clickCheckBox);
            btnGuardarPiezas.click(fnGuardarPiezas);
        }

        $('.inputPrecio').on('focus', function () {
            $(this).select();
        });

        $('.inputPrecio').on('change', function () {
            let valor = unmaskNumero($(this).val());
            $(this).val(maskNumero(valor));
        });

        $('.comboInsumo').on('change', function () {
            let insumo = $(this).val();
            let divPieza = $(this).closest('.divPieza');

            $.post('/Barrenacion/GetPiezaNueva', { insumo, areaCuenta: comboAC.val() }).then(response => {
                if (response.success) {
                    divPieza.find('.inputNoSerie').val(response.noSerie);
                    divPieza.find('.inputPrecio').val($(this).find('option:selected').attr('data-precioPza'));
                } else {
                    AlertaGeneral(`Alerta`, `Error al consultar la información de la pieza nueva.`);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            });
        });

        $('.comboPieza').on('change', function () {
            let piezaObj = $(this).val();
            let div = $(this).parents('div.seccion');
            let insumo = div.find('.comboInsumo').val();
            limpiarDiv(div);
            if (piezaObj == "0") {

                $.post('/Barrenacion/GetPiezaNueva', { insumo: insumo, areaCuenta: comboAC.val() }).then(response => {
                    if (response.success) {
                        div.find('.inputNoSerie').val(response.noSerie);
                        div.find('.inputPrecio').val(response.precioInsumo);
                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información de la pieza nueva.`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
            }
            else {
                $.get('/Barrenacion/GetPiezaID', { piezaID: piezaObj }).then(response => {
                    if (response.success) {
                        let pieza = response.pieza
                        div.find('.inputNoSerie').val(pieza.noSerie.trim(' '));
                        div.find('.inputNoSerie').attr('disabled', false);
                        div.find('.inputSerie').val(pieza.serialExcel).attr('disabled', false);
                        div.find('.inputNoSerie').data().piezaID = pieza.id;
                        div.find('select').attr('disabled', false).val(pieza.insumo);
                        div.find('.inputPrecio').val(maskNumero(pieza.precio));
                        div.find('.inputPrecio').attr('disabled', false);
                        div.find('.comboPieza').val(pieza.id).attr('disabled', false);
                        div.find('.spanSizeRadio').children('input').attr('disabled', false);
                        div.find('.comboInsumo').val(pieza.insumo);

                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información de la pieza nueva.`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });

            }
        });

        function limpiarDiv(div) {
            div.find('.inputNoSerie').val('');
            div.find('.inputNoSerie').attr('disabled', false);
            div.find('.inputSerie').val('');
            div.find('.inputNoSerie').data().piezaID = 0;
            div.find('.inputPrecio').val('');
        }

        $('.comboPiezas').on('change', function () {
            let piezaID = $(this).val();
            let divPieza = $(this).closest('.divPieza');
            if (piezaID != 0) {
                divPieza.find('.inputNoSerie').val($(this).find('option:selected').text().trim());
                divPieza.find('.inputNoSerie').data().piezaID = $(this).val();
                divPieza.find('.inputPrecio').val($(this).find('option:selected').attr('data-preciopza'));
            } else {
                divPieza.find('.inputNoSerie').data().piezaID = 0;
                divPieza.find('.inputNoSerie').val('');
                divPieza.find('.inputPrecio').val('');
                divPieza.find('.comboInsumo').val('');
            }
        });

        function fnGuardarPiezas() {

            $.blockUI({ message: 'Guardando...' });
            $.post('/Barrenacion/SaveOrUpdatePiezasBarrenadora', { listaPzas: _listaPiezas, pzasCompletas: cbPiezasTotales.prop("checked") }).always($.unblockUI).then(response => {
                if (response.success) {
                    AlertaGeneral('Operacion Exitosa', 'La operacion fue exitosa');
                    modalGestionPiezas.modal('hide');
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            });
        }

        function cargarEquipos() {
            llenarCombos();
            const areaCuenta = comboAC.val();
            const estatusPiezas = comboEstatus.val();
            $.blockUI({ message: 'Cargando Perforadoras...' });
            $.get('/Barrenacion/ObtenerBarrenadorasPiezas', { areaCuenta, estatusPiezas })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        cargarTablaEquipos(response.listaBarrenadoras);
                    } else {
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
        }


        tablaEquipos.on('click', '.botonPiezas', function () {
            _listaPiezas = [];
            const barrenadora = dtTablaEquipos.row($(this).parents('tr')).data();
            // llenarCombos();
            obtenerPiezasBarrenadora(barrenadora);

        })

        function cargarTablaEquipos(data) {
            dtTablaEquipos = tablaEquipos.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                data,
                columns: [
                    { data: 'noEconomico', title: 'No. Económico' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'noSerie', title: 'No. Serie' },
                    { data: 'estatus', title: 'Estatus' },
                    { data: 'id', render: (data, type, row) => `<button class="btn btn-primary botonPiezas"><i class="fas fa-tools"></i> Piezas</button>` }
                ],
                columnDefs: [{ className: "dt-center", "targets": "_all" }],
                drawCallback: function (settings) {

                }
            });
        }

        function agregarListeners() {

            botonBuscar.click(cargarEquipos);
            botonGuardarPiezas.click(guardarPiezas);
            /* modalPiezas.on("hide.bs.modal", limpiarCamposModal);
             modalPiezas.find('.btn-danger').click(e => {
                 const div = $(e.currentTarget).hide(200).closest('div.row');
                 const tipoPieza = div.attr('tipoPieza');
                 switch (+(tipoPieza)) {
                     case 1:
                         divRadioBroca.show(200);
                         $('#divBroca').find('.inputPrecio').attr('disabled', false);
                         $('#divBroca').find('.inputNoSerieManual').attr('disabled', false);
                         break;
                     case 2:
                         divRadioMartillo.show(200);
                         inputSerieMartillo.attr('disabled', false);
                         inputDesechoMartillo.show(200);
                         $('#divMartillo').find('.inputPrecio').attr('disabled', false);
                         $('#divMartillo').find('.inputNoSerieManual').attr('disabled', false);
                         //Quitar culata y cilindro.
                         divRadioCulata.show(200);
                         $('#divCulata').find('.inputPrecio').attr('disabled', false);
                         $('#divCulata').find('input.inputNoSerie').toArray().forEach(inputSerie => $(inputSerie).data().piezaID = 0);
                         $('#divCulata').find('input.inputNoSerie').val('');
                         $('#divCulata').find('select').attr('disabled', false).val(0);
                         $('#divCulata').find('.inputNoSerieManual').attr('disabled', false);
                         divRadioCilindro.show(200);
                         $('#divCilindro').find('.inputPrecio').attr('disabled', false);
                         $('#divCilindro').find('input.inputNoSerie').toArray().forEach(inputSerie => $(inputSerie).data().piezaID = 0);
                         $('#divCilindro').find('input.inputNoSerie').val('');
                         $('#divCilindro').find('select').attr('disabled', false).val(0);
                         $('#divCilindro').find('.inputNoSerieManual').attr('disabled', false);
                         break;
                     case 3:
                         divRadioBarra.show(200);
                         inputDesechoBarra.show(200);
                         $('#divBarra').find('.inputPrecio').attr('disabled', false);
                         $('#divBarra').find('.inputNoSerieManual').attr('disabled', false);
                         break;
                     case 4:
                         divRadioCulata.show(200);
                         $('#divCulata').find('.inputPrecio').attr('disabled', false);
                         $('#divCulata').find('.inputNoSerieManual').attr('disabled', false);
                         break;
                     case 6:
                         divRadioCilindro.show(200);
                         $('#divCilindro').find('.inputPrecio').attr('disabled', false);
                         break;
                     case 7:
                         divRadioBarraSegunda.show(200);
                         inputDesechoBarraSegunda.show(200);
                         $('#divBarraSegunda').find('.inputPrecio').attr('disabled', false);
                         break;
                 }
 
                 div.find('input.inputNoSerie').toArray().forEach(inputSerie => $(inputSerie).data().piezaID = 0);
                 div.find('input.inputNoSerie').val('');
                 div.find('select').attr('disabled', false).val(0);
                 div.find('.inputNoSerieManual').attr('disabled', false).val(0);
             });*/

            inputSerieMartillo.getAutocompleteValid(setSerieMartillo, validarSerieMartillo, null, ObtenerSerieMartilloReparadoNoAsignado);
            inputNoEconomicoBarrenadora.getAutocompleteValid(setEquipo, validarEquipo, { porDesc: false }, '/Barrenacion/ObtenerBarrenadorasAutocomplete');
            botonModalBarrenadora.click(() => {
                modalAgregarBarrenadora.modal('show');
            });

            modalAgregarBarrenadora.on("hide.bs.modal", () => {
                inputNoEconomicoBarrenadora.val('');
                inputNoEconomicoBarrenadora.data().maquinaID = null;
            });

            botonAgregarBarrenadora.click(() => {

                const maquinaID = inputNoEconomicoBarrenadora.data().maquinaID;
                if (maquinaID == null || maquinaID == 0) {
                    AlertaGeneral(`Aviso`, `Debe ingresar un número económico válido para poder dar de alta una barrenadora.`);
                    return;
                }
                modalAgregarBarrenadora.modal('hide');

                $.blockUI({ message: 'Dando de alta Perforadora...' });
                $.post('/Barrenacion/GuardarNuevaBarrenadora', { maquinaID })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            AlertaGeneral(`Alerta`, `Operación exitosa.`);
                            cargarEquipos();
                        } else {
                            // Operación no completada.
                            AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        }
                    }, error => {
                        // Error al lanzar la petición.
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            });
        }

        function setSerieMartillo(e, ui) {
            inputSerieMartillo.val(ui.item.value);
            inputSerieMartillo.data().piezaID = ui.item.id;
            spnReparadoMartillo.removeClass("hidden");
        }

        function validarSerieMartillo(e, ui) {
            if (ui.item == null) {
                spnReparadoMartillo.addClass("hidden");
            }
        }

        function setEquipo(e, ui) {
            inputNoEconomicoBarrenadora.val(ui.item.value);
            inputNoEconomicoBarrenadora.data().maquinaID = ui.item.id;
        }

        function validarEquipo(e, ui) {
            if (ui.item == null) {
                inputNoEconomicoBarrenadora.val('');
                inputNoEconomicoBarrenadora.data().maquinaID = null;
            }
        }

        function obtenerPiezasBarrenadora(barrenadora) {
            llenarCombos();
            $.blockUI({ message: 'Cargando piezas...' });
            $.get('/Barrenacion/ObtenerPiezasBarrenadora', { barrenadoraID: barrenadora.id, areaCuenta: comboAC.val() })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        _listaPiezas = [];
                        _barrenadoraID = barrenadora.id;
                        fnEnable();
                        if (response.items != null) {
                            _listaPiezas = response.items;
                            if (response.items.length <= 3) {
                                divCulata.hide();
                                divMartillo.hide();
                                divCilindro.hide();
                                $('#divBarra').hide();
                                $('#divZanco').show();
                                cargarPiezasModalHidro(response.items);

                            } else {
                                $('#divBarra').show();
                                divCulata.show();
                                divMartillo.show();
                                divCilindro.show();
                                $('#divZanco').hide();
                                cargarPiezasModal(response.items);
                            }
                            cbPiezasTotales.prop("checked", response.piezasAsignadas);
                        }
                        modalGestionPiezas.modal('show');
                    } else {
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }
        // function obtenerPiezasBarrenadoraHidro(barrenadora) {
        //     $.blockUI({ message: 'Cargando piezas...' });
        //     $.get('/Barrenacion/ObtenerPiezasBarrenadora', { barrenadoraID: barrenadora.id, areaCuenta: comboAC.val() })
        //         .always($.unblockUI)
        //         .then(response => {
        //             if (response.success) {
        //                 _listaPiezas = [];
        //                 _barrenadoraID = barrenadora.id;
        //                 fnEnable();
        //                 if (response.items != null) {
        //                     _listaPiezas = response.items;
        //                     cargarPiezasModalHidro(response.items);
        //                     cbPiezasTotales.prop("checked", response.piezasAsignadas);
        //                 }
        //                 modalGestionPiezas.modal('show');
        //             } else {
        //                 AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
        //             }
        //         }, error => {
        //             AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
        //         }
        //         );
        // }

        function fnEnable() {
            $('.btnEditInsumo').val('').addClass('btnAceptarInsumo');
            $('.btnEditInsumo').val('').removeClass('btnEditInsumo');
            $('.comboPieza').val('');
            $('.inputNoSerie').val('').prop('disabled', false);
            $('.inputSerie').val('').prop('disabled', false);
            $('.inputPrecio').val('').prop('disabled', false);
            $('.quitarPieza').val('');
            $('.inputNoSerie').data().piezaID = 0;
            $('.spanSizeRadio').children('input').attr('disabled', false).prop('checked', false);
            $('.comboPieza').attr('disabled', false);
            $('.comboInsumo').attr('disabled', false);
        }

        function cargarPiezasModal(piezas) {
            const arrayDivsInputs = ['Broca', 'Martillo', 'Barra', 'Culata', 'Cilindro', 'BarraSegunda', 'Zanco'];
            arrayDivsInputs.forEach(divID => {
                const div = $(`#div${divID}`);
                const tipoPieza = +(div.attr('tipoPieza'));
                let pieza = null;
                if (tipoPieza == 3 || tipoPieza == 7) {
                    if (divID == 'Barra') {
                        pieza = piezas.find(item => (item.tipoPieza == 3) && !item.barraSegunda);
                    } else if (divID == 'BarraSegunda') {
                        pieza = piezas.find(item => (item.tipoPieza == 7) && item.barraSegunda);
                    }
                } else {
                    pieza = piezas.find(item => item.tipoPieza == tipoPieza);
                }
                setPiezaExiste(div, pieza)
                div.find('.form-control').prop('disabled', true);
            });
        }
        function cargarPiezasModalHidro(piezas) {
            const arrayDivsInputs = ['Broca', 'BarraSegunda', 'Zanco'];
            arrayDivsInputs.forEach(divID => {
                const div = $(`#div${divID}`);
                const tipoPieza = +(div.attr('tipoPieza'));
                let pieza = null;
                if (tipoPieza == 3 || tipoPieza == 7) {
                    if (divID == 'Barra') {
                        pieza = piezas.find(item => (item.tipoPieza == 3) && !item.barraSegunda);
                    } else if (divID == 'BarraSegunda') {
                        pieza = piezas.find(item => (item.tipoPieza == 7) && item.barraSegunda);
                    }
                } else {
                    pieza = piezas.find(item => item.tipoPieza == tipoPieza);

                }
                setPiezaExiste(div, pieza)
                div.find('.form-control').prop('disabled', true);
            });
        }

        function setPiezaExiste(div, pieza) {
            if (pieza) {
                div.find('.inputNoSerie').val(pieza.noSerie.trim(' '));
                div.find('.inputNoSerie').attr('disabled', true);
                div.find('.inputSerie').val(pieza.serialExcel).attr('disabled', true);
                div.find('.inputNoSerie').data().piezaID = pieza.id;
                div.find('select').attr('disabled', true).val(pieza.insumo);
                div.find('.inputPrecio').val(maskNumero(pieza.precio));
                div.find('.inputPrecio').attr('disabled', true);
                div.find('.comboPieza').val(pieza.id).attr('disabled', true);
                div.find('.spanSizeRadio').children('input').attr('disabled', true);
                div.find('.comboInsumo').val(pieza.insumo);
                div.find('.btn-xs').removeClass('btnAceptarInsumo');
                div.find('.btn-xs').addClass('btnEditInsumo');
            }
        }

        function guardarPiezas() {
            const noSerieMartillo = inputSerieMartillo.val();
            if (comboInsumosMartillo.val() != 0) {
                if (noSerieMartillo == null || noSerieMartillo.trim().length == 0) {
                    AlertaGeneral(`Aviso`, `Debe introducir el número de serie del martillo.`);
                    return;
                }
            }
            const desechoBarra = inputDesechoBarra.val();
            const desechoMartillo = inputDesechoMartillo.val();
            const listaPiezas = obtenerPiezas();
            const pzasCompletas = inputPzasCompletas.prop('checked');

            $.blockUI({ message: 'Guardando piezas...' });
            $.post('/Barrenacion/GuardarPiezasBarrenadora', { listaPiezas, desechoMartillo, desechoBarra, pzasCompletas })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Se actualizaron las piezas correctamente.`);
                        modalPiezas.modal('hide');
                        cargarEquipos();
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
        }

        function obtenerPiezas() {
            const arrayDivsInputs = ['Broca', 'Martillo', 'Barra', 'Culata', 'Cilindro', 'BarraSegunda', 'Zanco'];
            const barrenadoraID = botonGuardarPiezas.data().barrenadoraID;

            return arrayDivsInputs.map(divID => {
                const div = $(`#div${divID}`);
                const insumo = +(div.find('select').val());
                const tipoPieza = +(div.attr('tipoPieza'));
                const estadoRadio = +($(`input[name=radio${divID}]:checked`).val() == undefined ? 3 : $(`input[name=radio${divID}]:checked`).val());
                return {
                    id: div.find('.inputNoSerie').data().piezaID ?? 0,
                    noSerie: div.find('.inputNoSerie').val(),
                    insumo: insumo,
                    tipoPieza: tipoPieza == 7 ? 3 : tipoPieza,
                    tipoBroca: 0,
                    barraSegunda: tipoPieza == 7 ? true : false,
                    horasTrabajadas: 0,
                    horasAcumuladas: 0,
                    reparando: estadoRadio == 1 ? true : false,
                    cantidadReparaciones: 0,
                    precio: +(unmaskNumero(div.find('.inputPrecio').val())),
                    activa: estadoRadio == 0 ? false : true,
                    montada: estadoRadio == 3 ? true : false,
                    culataID: 0,
                    cilindroID: 0,
                    barrenadoraID: barrenadoraID,
                    serialExcel: div.find('.inputNoSerieManual').val(),
                    areaCuenta: comboAC.val()
                };
            });
        }

        function limpiarCamposModal() {

            inputPzasCompletas.prop('checked', true);
            divRadioBroca.hide();
            divRadioMartillo.hide();
            divRadioBarra.hide();
            divRadioBarraSegunda.hide();
            divRadioCulata.hide();
            divRadioCilindro.hide();
            divRadioZanco.hide();

            modalPiezas.find('input[type="text"]').val('');
            modalPiezas.find('.btn-danger').prop('disabled', false).show();
            modalPiezas.find('.inputNoSerie').toArray().forEach(inputSerie => $(inputSerie).data().piezaID = 0);

            divRadioBroca.find('input[name=radioBroca]')[0].checked = false;
            divRadioBroca.find('input[name=radioBroca]')[1].checked = false;

            divRadioMartillo.find('input[name=radioMartillo]')[0].checked = false;
            divRadioMartillo.find('input[name=radioMartillo]')[1].checked = false;
            divRadioMartillo.find('input[name=radioMartillo]')[2].checked = false;

            divRadioBarra.find('input[name=radioBarra]')[0].checked = false;
            divRadioBarra.find('input[name=radioBarra]')[1].checked = false;

            divRadioBarraSegunda.find('input[name=radioBarraSegunda]')[0].checked = false;
            divRadioBarraSegunda.find('input[name=radioBarraSegunda]')[1].checked = false;

            divRadioCulata.find('input[name=radioCulata]')[0].checked = false;
            divRadioCulata.find('input[name=radioCulata]')[1].checked = false;

            divRadioCilindro.find('input[name=radioCilindro]')[0].checked = false;
            divRadioCilindro.find('input[name=radioCilindro]')[1].checked = false;

            divRadioZanco.find('input[name=radioZanco]')[0].checked = false;
            divRadioZanco.find('input[name=radioZanco]')[1].checked = false;

            modalPiezas.find('select').attr('disabled', false).val(0);
            $('.inputNoSerieManual').attr('disabled', false).val(0);

            inputSerieMartillo.attr('disabled', false);

            inputDesechoMartillo.hide();
            inputDesechoBarra.hide();
            inputDesechoBarraSegunda.hide();

            spnReparadoMartillo.addClass("hidden");
        }

        function llenarCombos() {

            limpiarCombos();
            fnLimpiarComboPiezas();

            if (comboAC.val() != "") {
                $.blockUI({ message: 'Cargando insumos...' });
                $.get('/Barrenacion/ObtenerInsumosPorPiezaConPrecio', { areaCuenta: comboAC.val() })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            cargarCombosInsumos(response);
                            if (response.BrocaBarrenadora ?? false)
                                CargarPiezasBarrenadora(response.BrocaBarrenadora, comboPiezasBroca);
                            if (response.MartilloBarrenadora ?? false)
                                CargarPiezasBarrenadora(response.MartilloBarrenadora, comboPiezasMartillo);
                            if (response.BarraBarrenadora ?? false)
                                CargarPiezasBarrenadora(response.BarraBarrenadora, comboPiezasBarra);
                            if (response.BarraSegunda ?? false)
                                CargarPiezasBarrenadora(response.BarraSegunda, comboPiezasBarraSegunda);
                            if (response.CulataBarrenadora ?? false)
                                CargarPiezasBarrenadora(response.CulataBarrenadora, comboPiezasCulata);
                            if (response.CilindroBarrenadora ?? false)
                                CargarPiezasBarrenadora(response.CilindroBarrenadora, comboPiezasCilindro);
                            if (response.ZancoBarrenadora ?? false)
                                CargarPiezasBarrenadora(response.ZancoBarrenadora, comboPiezasZanco);
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

        }


        function limpiarCombos() {
            comboInsumosBroca.empty();
            comboInsumosBroca.append(`<option value="0" >Seleccione Insumo</option>`);
            comboInsumosMartillo.empty();
            comboInsumosMartillo.append(`<option value="0" >Seleccione Insumo</option>`);
            comboInsumosBarra.empty();
            comboInsumosBarra.append(`<option value="0" >Seleccione Insumo</option>`);
            comboInsumosBarraSegunda.empty();
            comboInsumosBarraSegunda.append(`<option value="0" >Seleccione Insumo</option>`);
            comboInsumosCulata.empty();
            comboInsumosCulata.append(`<option value="0" >Seleccione Insumo</option>`);
            comboInsumosCilindro.empty();
            comboInsumosCilindro.append(`<option value="0" >Seleccione Insumo</option>`);
            comboInsumosZanco.empty();
            comboInsumosZanco.append(`<option value="0" >Seleccione Insumo</option>`);
        }

        function fnLimpiarComboPiezas() {
            comboPiezasBroca.empty();
            comboPiezasBroca.append(`<option selected value="0">Nueva Pieza</option>`);
            comboPiezasMartillo.empty();
            comboPiezasMartillo.append(`<option value="0" >Nueva Pieza</option>`);
            comboPiezasBarra.empty();
            comboPiezasBarra.append(`<option value="0" >Nueva Pieza</option>`);
            comboPiezasBarraSegunda.empty();
            comboPiezasBarraSegunda.append(`<option value="0" >Nueva Pieza</option>`);
            comboPiezasCulata.empty();
            comboPiezasCulata.append(`<option value="0" >Nueva Pieza</option>`);
            comboPiezasCilindro.empty();
            comboPiezasCilindro.append(`<option value="0" >Nueva Pieza</option>`);
            comboPiezasZanco.empty();
            comboPiezasZanco.append(`<option value="0" >Nueva Pieza</option>`);
        }

        function cargarCombosInsumos(listaInsumos) {
            if (listaInsumos.Broca && listaInsumos.Broca.length > 0) {
                listaInsumos.Broca.forEach(insumoPieza => {
                    comboInsumosBroca.append(`<option value="${insumoPieza.insumo}" data-precioPza="${insumoPieza.precioPieza}">${insumoPieza.descripcion}</option>`
                    );
                });
            }

            if (listaInsumos.Martillo && listaInsumos.Martillo.length > 0) {
                listaInsumos.Martillo.forEach(insumo => {
                    comboInsumosMartillo.append(`<option value="${insumo.insumo}" data-precioPza="${insumo.precioPieza}">${insumo.descripcion}</option>`
                    );
                });
            }

            if (listaInsumos.Barra && listaInsumos.Barra.length > 0) {
                listaInsumos.Barra.forEach(insumo => {
                    comboInsumosBarra.append(`<option value="${insumo.insumo}" data-precioPza="${insumo.precioPieza}">${insumo.descripcion}</option>`
                    );
                });
            }

            if (listaInsumos.Barra && listaInsumos.Barra.length > 0) {
                listaInsumos.Barra.forEach(insumo => {
                    comboInsumosBarraSegunda.append(`<option value="${insumo.insumo}" data-precioPza="${insumo.precioPieza}">${insumo.descripcion}</option>`
                    );
                });
            }

            if (listaInsumos.Culata && listaInsumos.Culata.length > 0) {
                listaInsumos.Culata.forEach(insumo => {
                    comboInsumosCulata.append(`<option value="${insumo.insumo}" data-precioPza="${insumo.precioPieza}">${insumo.descripcion}</option>`
                    );
                });
            }

            if (listaInsumos.Cilindro && listaInsumos.Cilindro.length > 0) {
                listaInsumos.Cilindro.forEach(insumo => {
                    comboInsumosCilindro.append(`<option value="${insumo.insumo}" data-precioPza="${insumo.precioPieza}">${insumo.descripcion}</option>`
                    );
                });
            }

            if (listaInsumos.Zanco && listaInsumos.Zanco.length > 0) {
                listaInsumos.Zanco.forEach(insumo => {
                    comboInsumosZanco.append(`<option value="${insumo.insumo}" data-precioPza="${insumo.precioPieza}">${insumo.descripcion}</option>`
                    );
                });
            }
        }

        function CargarPiezasBarrenadora(listaPiezas, cboPiezas) {
            if (listaPiezas.length > 0) {
                listaPiezas.forEach(piezaBarrenadora => {
                    $(cboPiezas).append(`<option value="${piezaBarrenadora.id}" 
                                         data-precioPza="${piezaBarrenadora.precio}"
                                         data-horasAcum="${piezaBarrenadora.horasAcumuladas}">
                    ${piezaBarrenadora.serieAutomatica}
                    </option>`
                    );
                });
            }
        }

        //#region Nuevo Guardado de Piezas

        function clickCheckBox() {
            var $box = $(this);
            if ($box.is(":checked")) {
                var group = "input:checkbox[name='" + $box.attr("name") + "']";
                $(group).prop("checked", false);
                $box.prop("checked", true);
                setInfoEdit($box);
            } else {
                $box.prop("checked", false);
                rollbackPieza($box);
            }
        }

        function setInfoEdit(box) {
            let activa = $(box).val();
            let divID = $(box).attr('name').split('-')[1];
            let div = $(`#div${divID}`);
            if (div.find('.quitarPieza').val() == '') {
                if (div.attr('id') == "divMartillo") {
                    addPiezaTempEdit(box, divMartillo);
                    boxCilindro = box.val() == "1" ? $(divCilindro.find('input:checkbox')[0]).prop('checked', true) : $(divCilindro.find('input:checkbox')[1]).prop('checked', true);
                    addPiezaTempEdit(boxCilindro, divCilindro);
                    boxCulata = box.val() == "1" ? $(divCulata.find('input:checkbox')[0]).prop('checked', true) : $(divCulata.find('input:checkbox')[1]).prop('checked', true);
                    addPiezaTempEdit(boxCulata, divCulata);
                }
                else {
                    addPiezaTempEdit(box, div);
                }
            }
            else {
                let piezaEdit = div.find('.quitarPieza').data().piezaEditada;
                switch (activa) {
                    case 1: //Desmontar
                        piezaEdit.activa = true;
                        piezaEdit.montada = false;
                        break;
                    case 2: //Deshecho
                        piezaEdit.activa = false;
                        piezaEdit.montada = false;
                        break;
                    case 3: //Reparacion
                        piezaEdit.activa = true;
                        piezaEdit.montada = false;
                        piezaEdit.reparando = true;
                        break;
                    default:
                        break;
                }
                div.find('.quitarPieza').data().piezaEditada = piezaEdit;
            }
        }

        function addPiezaTempEdit(box, div) {
            let activa = +$(box).val();
            let piezaID = div.find('.inputNoSerie').data().piezaID;

            if (piezaID != 0) {
                let piezaEdit = _listaPiezas.filter(function (piezas) {
                    return piezas.id == piezaID;
                })[0];
                if (piezaEdit != undefined) {
                    switch (activa) {
                        case 1: //Desmontar
                            piezaEdit.activa = true;
                            piezaEdit.montada = false;
                            break;
                        case 2: //Deshecho
                            piezaEdit.activa = false;
                            piezaEdit.montada = false;
                            break;
                        case 3: //Reparacion
                            piezaEdit.activa = true;
                            piezaEdit.montada = false;
                            piezaEdit.reparando = true;
                            break;
                        default:
                            break;
                    }
                    div.find('.quitarPieza').val(div.find('.inputNoSerie').val());
                    div.find('.quitarPieza').data().piezaEditada = piezaEdit;
                }
                else {
                    $(box).prop('checked', false);
                }
                div.find('.inputNoSerie').val('');
                div.find('.inputNoSerie').attr('disabled', false);
                div.find('.inputNoSerie').data().piezaID = 0;
                div.find('select').attr('disabled', false).val(0);
                div.find('.inputPrecio').attr('disabled', false);
                div.find('.inputPrecio').val('');
                div.find('.inputSerie').val('');
                div.find('.inputSerie').attr('disabled', false);
                div.find('.comboPieza').val('');
            }
            else {
                $(box).prop('checked', false);
            }
        }

        function rollbackPieza(box) {
            let divID = $(box).attr('name').split('-')[1];
            let div = $(`#div${divID}`);
            if (div.attr('id') == "divMartillo") {
                unSetPieza(div);
                unSetPieza(divCilindro);
                unSetPieza(divCulata);
            }
            else {
                unSetPieza(div);
            }
        }

        function unSetPieza(div) {
            let piezaTemp = div.find('.quitarPieza').data().piezaEditada;

            if (piezaTemp != undefined) {
                _listaPiezas = _listaPiezas.filter(function (pieza) {
                    if (div.find('.inputNoSerie') != pieza.noSerie) {
                        return pieza;
                    }
                });

                _listaPiezas.forEach(pieza => {
                    if (pieza.id == piezaTemp.id) {
                        pieza.activa = true;
                        pieza.montada = true;
                        pieza.reparando = false;
                    }
                });

                let objTemp = _listaPiezas.find(pieza => {
                    if (pieza.id == piezaTemp.id) {
                        return pieza;
                    }
                });
                setPieza(div, objTemp);
            }
        }

        function setPieza(div, pieza) {
            div.find('.inputNoSerie').val(pieza.noSerie);
            div.find('.inputNoSerie').attr('disabled', false);
            div.find('.inputNoSerie').data().piezaID = pieza.id;
            div.find('select').attr('disabled', false).val(pieza.insumo);
            div.find('.inputPrecio').attr('disabled', false);
            div.find('.inputPrecio').val(pieza.precio);
            div.find('.inputSerie').val(pieza.serialExcel);
            div.find('.inputSerie').attr('disabled', false);
            div.find('.comboPieza').val(pieza.id);
            div.find('.quitarPieza').data().piezaEditada = null;
            div.find('.quitarPieza').val('');
            div.find('input:checkbox').attr('disabled', false).prop('checked', false);
        }

        function editInsumo() {
            if ($(this).hasClass('btnEditInsumo')) {
                $(this).removeClass('btnEditInsumo').addClass('btnAceptarInsumo');
                $(this).parents('div.seccion').find('.divEditForm').removeClass('hide');
                if ($(this).parents('div.seccion').attr('id') == "divMartillo") {
                    divCulata.find('.divEditForm').removeClass('hide');
                    divCilindro.find('.divEditForm').removeClass('hide');
                    botonEditCulata.removeClass('btnEditInsumo').addClass('btnAceptarInsumo');//.prop('disabled', true);
                    botonEditCilindro.removeClass('btnEditInsumo').addClass('btnAceptarInsumo');//.prop('disabled', true);
                }
                fnDisabledEdit($(this));
            }
            else {
                if (getValidar($(this))) {
                    $(this).addClass('btnEditInsumo').removeClass('btnAceptarInsumo');
                    let divID = $(this).parents('div.seccion').attr('id');
                    disabledInputInsumo(divID);
                }
                else {
                    AlertaGeneral('Alerta:', `Es necesaria Tener la informacíon del Pieza: ${$(this).parent().find('label').text()}`);
                }
            }
        }

        function fnDisabledEdit(btnThis) {
            $btnThis = $(btnThis);
            let divID = getTipoPieza($btnThis); // Nombre de la pieza.
            let div = $(btnThis).parents('.seccion');
            div.find('.inputNoSerie').attr('disabled', false);
            div.find('.inputSerie').attr('disabled', false);
            div.find('select').attr('disabled', false);
            div.find('.inputPrecio').attr('disabled', false);
            div.find('.comboPieza').attr('disabled', false);
            div.find('.spanSizeRadio').children('input').attr('disabled', false);
        }

        function getTipoPieza(div) {
            const arrayDivsInputs = ['Broca', 'Martillo', 'Barra', 'Culata', 'Portabit', 'Cilindro', 'BarraSegunda', 'Zanco'];
            const tipoPieza = div.attr('tipoPieza') - 1;

            return arrayDivsInputs[tipoPieza];
        }

        function getValidar(elemento) {
            let divID = $(elemento).parents('div.seccion').attr('id');

            switch (divID) {
                case "divBarraSegunda":
                    return true;
                case "divBroca":
                case "divBarra":
                case "divZanco":
                    return validarInsumo($(`#${divID}`));
                case "divCulata":
                case "divCilindro":
                case "divMartillo":
                    if (!validarInsumo(divMartillo))
                        return false;
                    if (!validarInsumo(divCulata))
                        return false;
                    if (!validarInsumo(divCilindro))
                        return false;
                    else
                        return true;
                default:
                    break;
            }
        }

        function validarInsumo(div) {
            let inputNoSerie = $(div.find('.inputNoSerie'));
            let comboInsumo = $(div.find('comboInsumo'));
            let inputPrecio = $(div.find('inputPrecio'));

            if (inputNoSerie.val().trim(' ') == '' && inputNoSerie.val().trim(' ').length == 0)
                return false;
            if (comboInsumo.val() == 0)
                return false
            if (inputPrecio.val() == 0)
                return false
            else
                return true;
        }

        function disabledInputInsumo(divID) {

            let div = $(`#${divID}`);
            switch (divID) {
                case "divBarraSegunda":
                case "divBroca":
                case "divBarra":
                case "divZanco":
                    disabledInputs(div);
                    break;
                case "divCulata":
                case "divCilindro":
                case "divMartillo":
                    div = $("#divMartillo");
                    botonEditCulata.removeClass('btnAceptarInsumo').addClass('btnEditInsumo');
                    disabledInputs(div);
                    div = $("#divCulata");
                    botonEditCilindro.removeClass('btnAceptarInsumo').addClass('btnEditInsumo');
                    disabledInputs(div);
                    div = $("#divCilindro");
                    disabledInputs(div);
                    break;
                default:
                    break;
            }
        }

        function disabledInputs(div) {
            let tipoPieza = div.attr('tipoPieza');
            let valorInput = div.find('input:checked').val() ?? "1";

            var existe = _listaPiezas.filter(function (pieza) {
                if (pieza.noSerie == div.find('.inputNoSerie').val()) {
                    return pieza;
                }
            });
            if (existe.length == 0) {
                piezaEditada = {
                    id: div.find('.inputNoSerie').data().piezaID ?? 0,
                    noSerie: div.find('.inputNoSerie').val(),
                    insumo: div.find('.comboInsumo').val(),
                    tipoPieza: tipoPieza,
                    tipoBroca: tipoPieza == 3 ? 1 : tipoPieza == 7 ? 2 : 0,
                    barraSegunda: tipoPieza == 7 ? true : false,
                    horasTrabajadas: 0,
                    horasAcumuladas: 0,
                    reparando: false,
                    activa: true,
                    montada: true,
                    cilindroID: getInfoMartillo(tipoPieza).cilindroID,
                    culataID: getInfoMartillo(tipoPieza).culataID,
                    serialExcel: div.find('.inputSerie').val(),
                    precio: div.find('.inputPrecio').val(),
                    areaCuenta: comboAC.val(),
                    barrenadoraID: _barrenadoraID
                };

                switch (valorInput) {
                    case "1":
                        piezaEditada.reparando = false;
                        piezaEditada.activa = true;
                        piezaEditada.montada = true;
                        break;
                    case "2":
                        piezaEditada.reparando = false;
                        piezaEditada.activa = false;
                        piezaEditada.montada = false;
                        break;
                    case "3":
                        piezaEditada.reparando = false;
                        piezaEditada.activa = true;
                        piezaEditada.montada = true;
                        break;
                    default:
                        break;
                }
                _listaPiezas.push(piezaEditada);
            }

            div.find('.inputNoSerie').attr('disabled', true);
            div.find('.inputSerie').attr('disabled', true);
            div.find('select').attr('disabled', true);
            div.find('.inputPrecio').attr('disabled', true);
            div.find('.comboPieza').attr('disabled', true);
            div.find('.spanSizeRadio').children('input').attr('disabled', true);
        }

        function getInfoMartillo(tipoPieza) {
            var obj = {};
            if (tipoPieza == 2) {
                obj.cilindroID = divCilindro.find('.inputNoSerie').data().piezaID;
                obj.culataID = divCulata.find('.inputNoSerie').data().piezaID;
            }
            else {
                obj.cilindroID = 0;
                obj.culataID = 0;
            }

            return obj;
        }
        //#endregion
    }

    $(document).ready(() => Barrenacion.PiezasBarrenadora = new PiezasBarrenadora())
})();