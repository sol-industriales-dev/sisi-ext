(function () {
    $.namespace('Enkontrol.Almacen.Resguardo.Asignacion');
    Asignacion = function () {
        //#region Selectores
        const selectCC = $('#selectCC');
        const selectAlmacen = $('#selectAlmacen');
        const selectAlmacenResguardo = $('#selectAlmacenResguardo');
        const inputFechaResguardo = $('#inputFechaResguardo');
        const report = $("#report");
        const btnGuardarAsignacion = $('#btnGuardarAsignacion');
        const btnVerReporte = $('#btnVerReporte');
        const inputEntregoNum = $('#inputEntregoNum');
        const inputEntregoDesc = $('#inputEntregoDesc');
        const inputAutorizoNum = $('#inputAutorizoNum');
        const inputAutorizoDesc = $('#inputAutorizoDesc');
        const inputAutorizoNumMdlCambio = $('#inputAutorizoNumMdlCambio');
        const inputAutorizoDescMdlCambio = $('#inputAutorizoDescMdlCambio');
        const inputEstatus = $('#inputEstatus');
        const inputRecibio = $('#inputRecibio');
        const inputCondiciones = $('#inputCondiciones');
        const btnAgregarInsumo = $('#btnAgregarInsumo');
        const btnInfoEnter = $('#btnInfoEnter');
        const btnQuitarInsumo = $('#btnQuitarInsumo');
        const tblInsumos = $('#tblInsumos');
        const inputFolio = $('#inputFolio');
        const inputClaveEmpleado = $('#inputClaveEmpleado');
        const inputClaveEmpleadoDesc = $('#inputClaveEmpleadoDesc');

        const fieldsetPanelDerecho = $('#fieldsetPanelDerecho');
        const inputMarca = $('#inputMarca');
        const inputModelo = $('#inputModelo');
        const inputColor = $('#inputColor');
        const inputNumSerie = $('#inputNumSerie');
        const inputValorActivo = $('#inputValorActivo');
        const inputCostoProm = $('#inputCostoProm');
        const textAreaComentarios = $('#textAreaComentarios');
        const formRadioBtnCondiciones = $('#formRadioBtnCondiciones');
        const inputEmpleadoCustodiaNum = $('#inputEmpleadoCustodiaNum');
        const inputEmpleadoCustodiaDesc = $('#inputEmpleadoCustodiaDesc');
        const fieldsetInfo = $('#fieldsetInfo');
        const inputFechaDevolucion = $('#inputFechaDevolucion');

        const btnRegresar = $('#btnRegresar');
        const mdlDevolucionResguardo = $('#mdlDevolucionResguardo');
        const formRadioBtnCondicionesDevolucion = $('#formRadioBtnCondicionesDevolucion');
        const tblInsumosDevolucion = $('#tblInsumosDevolucion');
        const labelFechaDevolución = $('#labelFechaDevolución');
        const inputRecibioDevolucionNum = $('#inputRecibioDevolucionNum');
        const inputRecibioDevolucionDesc = $('#inputRecibioDevolucionDesc');
        const btnGuardarDevolucion = $('#btnGuardarDevolucion');

        const mdlEmpleados = $('#mdlEmpleados');
        const tblEmpleados = $('#tblEmpleados');
        const btnMostrarEmpleados = $('#btnMostrarEmpleados');
        const inputFiltroEmpleado = $('#inputFiltroEmpleado');
        const inputFiltroNombre = $('#inputFiltroNombre');
        const btnQuitarFiltros = $('#btnQuitarFiltros');

        const btnNuevoEmpleado = $('#btnNuevoEmpleado');
        const mdlNuevoEmpleado = $('#mdlNuevoEmpleado');
        const inputEmpleadoNombre = $('#inputEmpleadoNombre');
        const inputEmpleadoPaterno = $('#inputEmpleadoPaterno');
        const inputEmpleadoMaterno = $('#inputEmpleadoMaterno');
        const inputEmpleadoFechaNacimiento = $('#inputEmpleadoFechaNacimiento');
        const inputEmpleadoRFC = $('#inputEmpleadoRFC');
        const btnGuardarNuevoEmpleado = $('#btnGuardarNuevoEmpleado');
        const mdlUbicacionDetalle = $('#mdlUbicacionDetalle');
        const tblUbicacion = $('#tblUbicacion');
        const btnEscogerUbicacion = $('#btnEscogerUbicacion');
        const btnCambio = $('#btnCambio');
        const modalCambio = $('#modalCambio');
        const tituloCambio = $('#tituloCambio');
        const btnGuardarCambio = $('#btnGuardarCambio');
        const selectCCCambio = $('#selectCCCambio');
        const btnMostrarEmpleadosCambio = $('#btnMostrarEmpleadosCambio');
        const btnNuevoEmpleadoCambios = $('#btnNuevoEmpleadoCambios');
        const inputFolioCambio = $('#inputFolioCambio');
        const inputEmpleadoCustodiaNumCambio = $('#inputEmpleadoCustodiaNumCambio');
        const inputEmpleadoCustodiaDescCambio = $('#inputEmpleadoCustodiaDescCambio');
        const inputClaveEmpleadoCambio = $('#inputClaveEmpleadoCambio');
        const inputClaveEmpleadoDescCambio = $('#inputClaveEmpleadoDescCambio');
        // const btnUsuariosQueNoCoinciden = $('#btnUsuariosQueNoCoinciden');
        const btnCrearRFC = $('#btnCrearRFC');
        const btnMostrarEmpleadosAutorizo = $('#btnMostrarEmpleadosAutorizo');
        const btnMostrarEmpleadosCambioClave = $('#btnMostrarEmpleadosCambioClave');
        const btnMostrarEmpleadosCambioAutorizo = $('#btnMostrarEmpleadosCambioAutorizo');
        const btnMostrarEmpleadosClaveEmpleadoDescActivo = $('#btnMostrarEmpleadosClaveEmpleadoDescActivo');

        const inputEmpresaActual = $('#inputEmpresaActual');
        //#endregion

        _filaInsumo = null;

        let flagCrearRow = true;
        let inputNum = null;
        let inputNom = null;
        let tabIndexTblInsumos = 50;
        let colIndex = 0;
        let dataResguardo = [];
        let newRow = false;

        function init() {
            initForm();
            initTblInsumos();
            initTblInsumosDevolucion();
            initTblEmpleados();
            initTableUbicacion();
            fncSelect2();

            labelFechaDevolución.text(`Fecha Devolución: ${getFecha()}`);
            // inputEmpleadoFechaNacimiento.datepicker().datepicker();
            inputEmpleadoFechaNacimiento.datepicker({
                changeMonth: true,
                changeYear: true,
                yearRange: "1900:2030"
            }).datepicker("setDate", new Date());

            btnGuardarAsignacion.click(guardarAsignacion);
            btnGuardarDevolucion.click(guardarDevolucion);
            btnQuitarFiltros.click(limpiarFiltros);
            btnGuardarNuevoEmpleado.click(guardarNuevoEmpleado);
            btnCambio.click(() => {
                tituloCambio.text(' - ' + inputEmpleadoCustodiaDesc.val() + ' (' + selectCC.val() + '/' + selectAlmacen.val() + '/' + inputFolio.val() + ')');
                modalCambio.modal('show');
            });

            inputClaveEmpleadoCambio.on('change', function () {
                setEmpleadoDesc($(this), inputClaveEmpleadoDescCambio);
            });

            inputClaveEmpleado.on('change', function () {
                setEmpleadoDesc($(this), inputClaveEmpleadoDesc);
            });

            //inputClaveEmpleado.change(setEmpleadoDesc());
            inputClaveEmpleadoDesc.getAutocomplete(setEmpleadoNum, null, '/Enkontrol/Resguardo/GetEmpleadosAutoComplete');
            inputClaveEmpleadoDescCambio.getAutocomplete(setEmpleadoNumCambio, null, '/Enkontrol/Resguardo/GetEmpleadosAutoComplete');
            // selectCC.change(verificarPresupuestoCC);

            // btnUsuariosQueNoCoinciden.click(descargarExcelUsuariosQueNoCoinciden);

            if (+inputEmpresaActual.val() == 6) {
                $('.elementoMexico').css('display', 'none');
            }
        }

        btnInfoEnter.click(function () {
            Alert2Info(`Para agregar una partida nueva puedes dar clic al botón azul con el símbolo "+" o presionar la tecla "Enter" después de capturar el insumo y la cantidad, 
                        posicionados en el campo nivel. 
                        Todas las partidas deben tener un insumo válido y una cantidad mayor a cero.`);
        });

        btnGuardarCambio.on('click', function () {

            // numEmpleado: inputEmpleadoCustodiaNumCambio.val(),
            //         claveEmpleado: inputClaveEmpleadoCambio.val(),
            //         ccNuevo: selectCCCambio.val(),

            if (inputEmpresaActual.val() != 6) {
                if (
                    inputEmpleadoCustodiaNumCambio.val() != null &&
                    inputEmpleadoCustodiaNumCambio.val() != undefined &&
                    inputEmpleadoCustodiaNumCambio.val() != '' &&
                    inputClaveEmpleadoCambio.val() != null &&
                    inputClaveEmpleadoCambio.val() != undefined &&
                    inputClaveEmpleadoCambio.val() != '' &&
                    selectCCCambio.val() != null &&
                    selectCCCambio.val() != undefined &&
                    selectCCCambio.val() != ''
                ) {
                    guardarDevolucionCambio();
                }
            } else {
                if (
                    inputEmpleadoCustodiaNumCambio.val() != null &&
                    inputEmpleadoCustodiaNumCambio.val() != undefined &&
                    inputEmpleadoCustodiaNumCambio.val() != '' &&
                    selectCCCambio.val() != null &&
                    selectCCCambio.val() != undefined &&
                    selectCCCambio.val() != ''
                ) {
                    guardarDevolucionCambio();
                }
            }
            // if (selectCCCambio.val() != null && selectCCCambio.val() != undefined) {

            // }
        });

        inputAutorizoNum.on('change', function () {
            const valor = $(this).val();

            if (valor != '' && !isNaN(valor)) {
                getEmpleadoEnkontrolByID(valor, false);
            } else {
                inputAutorizoDesc.val('');
                inputAutorizoNum.val('');
            }
        });

        inputAutorizoNumMdlCambio.on('change', function () {
            const valor = $(this).val();

            if (valor != '' && !isNaN(valor)) {
                getEmpleadoEnkontrolByID(valor, true);
            } else {
                inputAutorizoDescMdlCambio.val('');
                inputAutorizoNumMdlCambio.val('');
            }
        });

        inputEmpleadoCustodiaNum.on('change', function () {
            const valor = $(this).val();

            if (valor != '' && !isNaN(valor)) {
                getEmpleadoCustodiaEnkontrolByID(valor);
            } else {
                inputEmpleadoCustodiaNum.val('');
                inputEmpleadoCustodiaDesc.val('');
            }
        });

        inputEmpleadoCustodiaNumCambio.on('change', function () {
            const valor = $(this).val();

            if (valor != '' && !isNaN(valor)) {
                getEmpleadoCustodiaEnkontrolByIDCambio(valor);
            } else {
                inputEmpleadoCustodiaNumCambio.val('');
                inputEmpleadoCustodiaDescCambio.val('');
            }
        });

        btnVerReporte.on('click', function () {
            if (selectCC.val() != '' && inputFolio.val() != '') {
                if (!isNaN(inputFolio.val())) {
                    verReporteResguardo(selectCC.val(), inputFolio.val());
                } else {
                    AlertaGeneral(`Alerta`, `Ingrese un número de Folio válido.`);
                }
            } else {
                AlertaGeneral(`Alerta`, `Seleccione un Centro de Costos y un número de Folio.`);
            }
        });

        fieldsetPanelDerecho.on('change', 'input, textarea', function () {
            let rowActive = tblInsumos.find('tbody tr.active');

            if (rowActive.length > 0) {
                asignarPanelDerecho();
            } else {
                AlertaGeneral(`Alerta`, `Seleccione un insumo.`);
                limpiarPanelDerecho();
            }
        });

        btnAgregarInsumo.on('click', function () {
            fncCrearRowInsumo();
        });

        btnQuitarInsumo.on('click', function () {
            tblInsumos.DataTable().row(tblInsumos.find("tr.active")).remove().draw();

            let cuerpo = tblInsumos.find('tbody');

            if (cuerpo.find("tr").length == 0) {
                tblInsumos.DataTable().draw();
            } else {
                tblInsumos.find('tbody tr').each(function (idx, row) {
                    let rowData = tblInsumos.DataTable().row(row).data();

                    if (rowData != undefined) {
                        tblInsumos.DataTable().row(row).data(rowData).draw();
                    }
                });
            }
        });

        fieldsetInfo.on('change', '#selectCC, #selectAlmacen', function () {
            let cc = selectCC.val();
            let alm_salida = selectAlmacen.val();

            limpiarPantalla();

            if (cc != '' && alm_salida != '') {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Resguardo/GetUltimoFolio', { cc: cc, alm_salida: alm_salida }).always($.unblockUI).then(response => {
                    if (response.success) {
                        inputFolio.val(response.folio);
                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información del último folio.`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
            }
        });

        inputFolio.on('change', function () {
            let cc = selectCC.val();
            let alm_salida = selectAlmacen.val();
            let folio = inputFolio.val();

            if (cc != '' && alm_salida != '' && (folio != '' && !isNaN(folio))) {
                limpiarPantalla();
                cargarResguardo();
            } else {
                limpiarPantalla();
            }
        });

        btnRegresar.on('click', function () {
            let data = tblInsumos.DataTable().rows().data();
            AddRows(tblInsumosDevolucion, data);
            mdlDevolucionResguardo.modal('show');

        });

        btnMostrarEmpleadosCambio.on('click', function () {
            inputNum = inputEmpleadoCustodiaNumCambio;
            inputNom = inputEmpleadoCustodiaDescCambio;
            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/Resguardo/GetEmpleados')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tblEmpleados, response.data);
                        mdlEmpleados.css('z-index', 1051);
                        mdlEmpleados.modal('show');
                        tblEmpleados.DataTable().columns.adjust().draw();
                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
        });

        btnMostrarEmpleadosCambioClave.on("click", function () {
            inputNum = inputClaveEmpleadoCambio;
            inputNom = inputClaveEmpleadoDescCambio;
            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/Resguardo/GetEmpleados')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tblEmpleados, response.data);
                        mdlEmpleados.css('z-index', 1051);
                        mdlEmpleados.modal('show');
                        tblEmpleados.DataTable().columns.adjust().draw();
                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
        });

        btnMostrarEmpleadosCambioAutorizo.on("click", function () {
            inputNum = inputAutorizoNumMdlCambio;
            inputNom = inputAutorizoDescMdlCambio;
            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/Resguardo/GetEmpleados')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tblEmpleados, response.data);
                        mdlEmpleados.css('z-index', 1051);
                        mdlEmpleados.modal('show');
                        tblEmpleados.DataTable().columns.adjust().draw();
                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
        });

        btnMostrarEmpleadosClaveEmpleadoDescActivo.on("click", function () {
            inputNum = inputClaveEmpleado;
            inputNom = inputClaveEmpleadoDesc;
            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/Resguardo/GetEmpleados')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tblEmpleados, response.data);
                        mdlEmpleados.css('z-index', 1051);
                        mdlEmpleados.modal('show');
                        tblEmpleados.DataTable().columns.adjust().draw();
                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
        });

        btnMostrarEmpleados.on('click', function () {
            inputNum = inputEmpleadoCustodiaNum;
            inputNom = inputEmpleadoCustodiaDesc;
            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/Resguardo/GetEmpleados')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tblEmpleados, response.data);
                        mdlEmpleados.modal('show');
                        tblEmpleados.DataTable().columns.adjust().draw();
                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        });

        btnMostrarEmpleadosAutorizo.on('click', function () {
            inputNum = inputAutorizoNum;
            inputNom = inputAutorizoDesc;
            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/Resguardo/GetEmpleados')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tblEmpleados, response.data);
                        mdlEmpleados.modal('show');
                        tblEmpleados.DataTable().columns.adjust().draw();
                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        });

        mdlEmpleados.on('keyup change', 'input', function () {
            tblEmpleados.DataTable().search('');
            tblEmpleados.DataTable().column($(this).data('columnIndex')).search(this.value).draw();
        });

        btnNuevoEmpleadoCambios.on('click', function () {
            inputNum = inputEmpleadoCustodiaNumCambio;
            inputNom = inputEmpleadoCustodiaDescCambio;

            mdlNuevoEmpleado.css('z-index', 1051);

            limpiarEmpleado();
            mdlNuevoEmpleado.modal('show');
        });

        btnNuevoEmpleado.on('click', function () {
            inputNum = inputEmpleadoCustodiaNum;
            inputNom = inputEmpleadoCustodiaDesc;

            limpiarEmpleado();

            mdlNuevoEmpleado.modal('show');
        });

        btnCrearRFC.click(function (e) {
            fncGenerarRFC();
        });

        $('.tg').bind('keypress', function (e) {
            if (e.keyCode == 13) {
                var TabIndexActual = $(this).attr('tabindex');
                var TabIndexSiguiente = parseInt(TabIndexActual) + 1;
                if (TabIndexSiguiente === 8) {
                    TabIndexSiguiente = 1;
                }
                var CampoSiguiente = $('[tabindex=' + TabIndexSiguiente + ']');

                if (CampoSiguiente.length > 0) {
                    CampoSiguiente.focus();
                    return false;
                } else {
                    return false;
                }
            }
        });

        //#region GENERACIÓN DE RFC
        inputEmpleadoNombre.on("keyup", function (e) {
            if (inputEmpleadoNombre.val() != "" && inputEmpleadoPaterno.val() != "" && inputEmpleadoMaterno.val() != "" && inputEmpleadoFechaNacimiento.val() != "") {
                fncGenerarRFC();
            }
        });

        inputEmpleadoPaterno.on("keyup", function (e) {
            if (inputEmpleadoNombre.val() != "" && inputEmpleadoPaterno.val() != "" && inputEmpleadoMaterno.val() != "" && inputEmpleadoFechaNacimiento.val() != "") {
                fncGenerarRFC();
            }
        });

        inputEmpleadoMaterno.on("keyup", function (e) {
            if (inputEmpleadoNombre.val() != "" && inputEmpleadoPaterno.val() != "" && inputEmpleadoMaterno.val() != "" && inputEmpleadoFechaNacimiento.val() != "") {
                fncGenerarRFC();
            }
        });

        inputEmpleadoFechaNacimiento.on("change", function (e) {
            if (inputEmpleadoNombre.val() != "" && inputEmpleadoPaterno.val() != "" && inputEmpleadoMaterno.val() != "" && inputEmpleadoFechaNacimiento.val() != "") {
                fncGenerarRFC();
            }
        });
        //#endregion

        function fncSelect2() {
            selectCC.select2();
            selectCCCambio.select2();
            selectAlmacen.select2();

            selectCC.select2({ width: "resolve" });
            // selectCCCambio.select2({ width: "resolve" });
            selectAlmacen.select2({ width: "resolve" });
            // selectCCCambio.css("width", "100%");
        }

        function initForm() {
            selectCC.fillCombo('/Enkontrol/Resguardo/FillComboCcTodosExistentes', null, false, null); // selectCC.fillCombo('/Enkontrol/Requisicion/FillComboCcAsigReq', null, false, null);
            selectAlmacen.fillCombo('/Enkontrol/Resguardo/FillComboAlmacenVirtual', null, false, null);
            selectAlmacenResguardo.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtirTodos', null, false, null);

            if (inputEmpresaActual.val() != 6) {
                selectAlmacenResguardo.val('997');
            } else {
                selectAlmacenResguardo.val('97');
            }

            inputFechaResguardo.datepicker().datepicker("setDate", new Date().toLocaleDateString());
            selectCCCambio.fillCombo('/Enkontrol/Resguardo/FillComboCcTodosExistentes', null, false, null);
        }

        function getEmpleadoEnkontrolByID(numEmpleado, esCambio) {
            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/OrdenCompra/GetEmpleadoUsuarioEK', { numEmpleado }).done(response => {
                if (response != '') {
                    if (esCambio) {
                        inputAutorizoDescMdlCambio.val(response);
                    } else {
                        inputAutorizoDesc.val(response);
                    }
                } else {
                    if (esCambio) {
                        inputAutorizoDescMdlCambio.val('');
                        inputAutorizoNum.val('');
                    } else {
                        inputAutorizoDesc.val('');
                        inputAutorizoNum.val('');
                    }
                }
            }).always($.unblockUI);
        }

        function getEmpleadoCustodiaEnkontrolByID(numEmpleado) {
            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/OrdenCompra/GetEmpleadoUsuarioEK', { numEmpleado }).done(response => {
                if (response != '') {
                    inputEmpleadoCustodiaDesc.val(response);
                } else {
                    inputEmpleadoCustodiaNum.val('');
                    inputEmpleadoCustodiaDesc.val('');
                }
            }).always($.unblockUI);
        }

        function getEmpleadoCustodiaEnkontrolByIDCambio(numEmpleado) {
            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/OrdenCompra/GetEmpleadoUsuarioEK', { numEmpleado }).done(response => {
                if (response != '') {
                    inputEmpleadoCustodiaDescCambio.val(response);
                } else {
                    inputEmpleadoCustodiaNumCambio.val('');
                    inputEmpleadoCustodiaDescCambio.val('');
                }
            }).always($.unblockUI);
        }

        function guardarAsignacion() {
            btnGuardarAsignacion.attr("disabled", true);

            let claveEmpleado = inputClaveEmpleado.val();

            if ((isNaN(claveEmpleado) || claveEmpleado == 0) && inputEmpresaActual.val() != 6) {
                AlertaGeneral(`Alerta`, `Debe capturar la clave del empleado.`);
                btnGuardarAsignacion.attr("disabled", false);
                return;
            }

            let resguardos = getResguardos();

            if (resguardos.length > 0) {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Resguardo/GuardarAsignacion', { resguardos }).always($.unblockUI).then(response => {
                    if (response.success) {
                        if (!response.esActualizar) {
                            // AlertaGeneral(`Alerta`, `Se ha guardado la información`);
                            Alert2Exito("Se ha registrado con éxito el resguardo.")
                            verReporteResguardo(response.data[0].cc, response.data[0].folio);
                            limpiarPantalla();
                            selectCC.val('');
                            selectAlmacen.val('');
                            inputFolio.val('');
                            inputClaveEmpleado.val('');
                            inputClaveEmpleadoDesc.val('');
                            btnGuardarAsignacion.attr("disabled", true);
                        } else {
                            Alert2Exito("Se ha actualizado con éxito el resguardo.");
                            limpiarPantalla();
                            selectCC.val('');
                            selectAlmacen.val('');
                            inputFolio.val('');
                            inputClaveEmpleado.val('');
                            inputClaveEmpleadoDesc.val('');
                            btnGuardarAsignacion.attr("disabled", true);
                        }
                    } else {
                        AlertaGeneral(`Alerta`, `${response.message.length > 0 ? response.message : ``}`);
                        btnGuardarAsignacion.attr("disabled", false);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    btnGuardarAsignacion.attr("disabled", false);
                }
                );
            } else {
                AlertaGeneral(`Alerta`, `Error al guardar la información. No se capturó correctamente la información de los insumos.`);
                btnGuardarAsignacion.attr("disabled", false);
            }
        }

        function guardarDevolucionCambio() {
            let resguardos = getResguardosDevolucionPorCambio();

            if (resguardos.length > 0) {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Resguardo/cambiarCCoEmpleado',
                    {
                        numEmpleado: +inputEmpleadoCustodiaNumCambio.val(),
                        claveEmpleado: +inputClaveEmpleadoCambio.val(),
                        ccNuevo: selectCCCambio.val(),
                        resguardos
                    }).always($.unblockUI).then(response => {
                        if (response.success) {
                            AlertaGeneral(`Alerta`, `Se ha realizado el cambio, el folio es: ${response.folio}`);
                            modalCambio.modal('hide');
                            limpiarPantalla();
                            selectCC.val('');
                            selectAlmacen.val('');
                            inputFolio.val('');
                            inputClaveEmpleado.val('');
                            inputClaveEmpleadoDesc.val('');
                        } else {
                            AlertaGeneral(`Alerta`, `${response.message.length > 0 ? response.message : ``}`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            } else {
                AlertaGeneral(`Alerta`, `Error al guardar la información. No se capturó correctamente la información de los insumos.`);
            }
        }

        function guardarDevolucion() {
            btnGuardarDevolucion.attr('disabled', true);

            let resguardos = getResguardosDevolucion();

            if (resguardos.length > 0) {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Resguardo/GuardarDevolucion', { resguardos }).always($.unblockUI).then(response => {
                    if (response.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información` + `: ${response.estatusFinal == 'P' ? 'DEVOLUCIÓN PARCIAL' : response.estatusFinal == 'D' ? 'DEVOLUCIÓN COMPLETA' : ''}`);
                        mdlDevolucionResguardo.modal('hide');
                        limpiarPantalla();
                        selectCC.val('');
                        selectAlmacen.val('');
                        inputFolio.val('');
                        inputClaveEmpleado.val('');
                        inputClaveEmpleadoDesc.val('');
                    } else {
                        AlertaGeneral(`Alerta`, `${response.message.length > 0 ? response.message : ``}`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
            } else {
                AlertaGeneral(`Alerta`, `Error al guardar la información. No se capturó correctamente la información de los insumos.`);
            }
        }

        function getResguardos() {
            let lstResguardos = [];

            tblInsumos.find('tbody tr').each(function (idx, row) {
                let rowData = tblInsumos.DataTable().row($(row)).data();

                if (rowData != undefined && rowData.id_activo > 0 && rowData.cantidad_resguardo > 0) {
                    let resguardo = {
                        cc: selectCC.val(),
                        folio: inputFolio.val(),
                        id_activo: rowData.id_activo,
                        id_tipo_activo: rowData.color_resguardo > 0 ? rowData.color_resguardo : 1,
                        marca: rowData.marca,
                        modelo: rowData.modelo,
                        color: rowData.color,
                        num_serie: rowData.num_serie,
                        valor_activo: rowData.valor_activo,
                        compania: null,
                        plan_desc: rowData.plan_desc,
                        condiciones: rowData.condiciones,
                        numpro: null,
                        factura: null,
                        fec_factura: null,
                        empleado: inputEmpleadoCustodiaNum.val(),
                        claveEmpleado: inputClaveEmpleado.val(),
                        licencia: 'S',
                        tipo: null,
                        fec_licencia: null,
                        observaciones: null,
                        fec_resguardo: null,
                        foto: null,
                        estatus: 'V',
                        entrega: inputEntregoNum.val(),
                        autoriza: inputAutorizoNum.val(),

                        //Campos para Devolución
                        recibio: null,
                        condiciones_ret: null,
                        fec_devolucion: null,
                        //

                        cantidad_resguardo: rowData.cantidad_resguardo,
                        alm_salida: selectAlmacen.val(),
                        alm_entrada: selectAlmacenResguardo.val(),
                        foto_2: null,
                        foto_3: null,
                        foto_4: null,
                        foto_5: null,
                        costo_promedio: rowData.costo_promedio,

                        area_alm: rowData.area_alm,
                        lado_alm: rowData.lado_alm,
                        estante_alm: rowData.estante_alm,
                        nivel_alm: rowData.nivel_alm,

                        //Campo para Devolución
                        resguardo_parcial: null
                        //
                    };

                    lstResguardos.push(resguardo);
                }
            });

            return lstResguardos;
        }

        function getResguardosDevolucionPorCambio() {
            let resguardos = [];

            dataResguardo.forEach((element, index, array) => {
                if (element.cantidad_resguardo > 0) {
                    resguardos.push({
                        cc: selectCC.val(),
                        folio: inputFolio.val(),
                        id_activo: element.id_activo,
                        id_tipo_activo: element.id_tipo_activo,
                        insumo: element.id_activo,
                        insumoDesc: element.insumoDesc,
                        cantidad_resguardo: element.cantidad_resguardo,
                        parcial: false,
                        recibio: inputRecibioDevolucionNum.val(),
                        condiciones: 'B',
                        alm_entrada: element.alm_entrada,
                        alm_salida: element.alm_salida,
                        area_alm: element.area_alm,
                        lado_alm: element.lado_alm,
                        estante_alm: element.estante_alm,
                        nivel_alm: element.nivel_alm,

                        marca: element.marca,
                        modelo: element.modelo,
                        color: element.color,
                        num_serie: element.num_serie,
                        valor_activo: element.valor_activo,
                        plan_desc: element.plan_desc,

                        entrega: inputEntregoNum.val(),
                        // autoriza: inputAutorizoNum.val(),
                        autoriza: inputAutorizoNumMdlCambio.val(),
                    });
                }
            });

            return resguardos;
        }

        function getResguardosDevolucion() {
            let resguardos = [];

            tblInsumosDevolucion.find('tbody tr').each(function (id, row) {
                let rowData = tblInsumosDevolucion.DataTable().row(row).data();
                let devolver = $(row).find('.checkBoxParcial').prop('checked');
                let cantidadDevolver = +($(row).find('.inputCantidad').val());

                if (devolver && cantidadDevolver > 0) {
                    let parcial = rowData.cantidad_resguardo > cantidadDevolver ? true : false;

                    resguardos.push({
                        cc: selectCC.val(),
                        folio: inputFolio.val(),
                        id_activo: rowData.id_activo,
                        insumo: rowData.id_activo,
                        insumoDesc: rowData.insumoDesc,
                        cantidad_resguardo: cantidadDevolver, //cantidad_resguardo: rowData.cantidad_resguardo,
                        parcial: parcial,
                        recibio: inputRecibioDevolucionNum.val(),
                        condiciones:
                            formRadioBtnCondicionesDevolucion.find('input[type="radio"]:checked').length > 0 ?
                                formRadioBtnCondicionesDevolucion.find('input[type="radio"]:checked').val() : 'B',
                        alm_entrada: rowData.alm_entrada,
                        alm_salida: rowData.alm_salida,
                        area_alm: rowData.area_alm,
                        lado_alm: rowData.lado_alm,
                        estante_alm: rowData.estante_alm,
                        nivel_alm: rowData.nivel_alm
                    });
                }
            });

            return resguardos;
        }

        function verReporteResguardo(cc, folio) {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });

            report.attr("src", '/Reportes/Vista.aspx?idReporte=111' + '&cc=' + cc + '&folio=' + folio);
            report.on('load', function () {
                $.unblockUI();
                openCRModal();
            });
        }

        function fncCrearRowInsumo() {
            fncVerificarCamposTblInsumos();
            if (selectCC.val() != "") {
                if (flagCrearRow) {
                    let datos = tblInsumos.DataTable().rows().data();
                    datos.push({
                        insumo: '',
                        insumoDesc: '',
                        cantidad: 0,
                        areaAlmacen: '',
                        ladoAlmacen: '',
                        ladoEstante: '',
                        nivelEstante: '',

                        color_resguardo: 0,
                        marca: '',
                        modelo: '',
                        color: '',
                        num_serie: '',
                        valor_activo: '',
                        costo_promedio: '',
                        plan_desc: '',
                        condiciones: 'B'
                    });
                    tblInsumos.DataTable().clear();
                    tblInsumos.DataTable().rows.add(datos).draw();
                    btnQuitarInsumo.prop("disabled", false);
                }
            } else {
                Alert2Warning("Es necesario seleccionar un CC.");
            }
        }

        function initTblInsumos() {
            tblInsumos.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                scrollCollapse: true,
                bInfo: false,
                searching: false,
                initComplete: function (settings, json) {
                    tblInsumos.on('click', 'tr', function () {
                        let rowData = tblInsumos.DataTable().row($(this).closest('tr')).data();
                        let $row = $(this).closest('tr');
                        let selected = $row.hasClass("active");

                        tblInsumos.find("tr").removeClass("active");

                        if (!selected) {
                            $row.not("th").addClass("active");
                            // habilitarPanelDerecho();
                        } else {
                            // deshabilitarPanelDerecho();
                        }

                        llenarPanelDerecho(rowData);

                        switch (rowData.condiciones_ret) {
                            case 'B':
                                inputCondiciones.val('BUENAS');
                                break;
                            case 'R':
                                inputCondiciones.val('REGULARES');
                                break;
                            case 'M':
                                inputCondiciones.val('MALAS');
                                break;
                            default:
                                inputCondiciones.val('');
                                break;
                        }

                        inputRecibio.val(rowData.recibioNombre);
                        inputFechaDevolucion.val(rowData.fec_devolucionString);
                    });

                    tblInsumos.on('change', 'input', function () {
                        let row = $(this).closest('tr');
                        let rowActive = tblInsumos.find('tbody tr.active');
                        let rowData = tblInsumos.DataTable().row(row).data();

                        // let rowIndex = row[0].rowIndex;
                        colIndex = $(this).closest("td").parent().children().index($(this).closest("td"));
                        let nextInput = $(this).closest("td").next("td").find("input");

                        let insumo = unmaskNumero(row.find('.inputInsumo').val());
                        let insumoDesc = row.find('.inputInsumoDesc').val();
                        let cantidad = unmaskNumero(row.find('.inputCantidad').val());
                        let ultimaCompra = unmaskNumero(row.find('.inputUltimoPrecio').val());
                        let area_alm = row.find('.inputAreaAlmacen').val();
                        let lado_alm = row.find('.inputLadoAlmacen').val();
                        let estante_alm = row.find('.inputEstanteAlmacen').val();
                        let nivel_alm = row.find('.inputNivelAlmacen').val();

                        rowData.ultimaCompra = ultimaCompra;
                        rowData.id_activo = insumo;
                        rowData.insumoDesc = insumoDesc;
                        rowData.cantidad_resguardo = cantidad;
                        rowData.area_alm = area_alm;
                        rowData.lado_alm = lado_alm;
                        rowData.estante_alm = estante_alm;
                        rowData.nivel_alm = nivel_alm;

                        tblInsumos.DataTable().row(row).data(rowData).draw();
                        reInicializarCamposRenglon(row, rowData);

                        if (rowActive.length > 0) {
                            rowActive.addClass("active");
                            asignarPanelDerecho();
                        }

                        if (nextInput.attr("disabled")) {
                            row.find(`td:eq(${colIndex + 2})`).find("input").trigger("focus");
                        } else {
                            row.find(`td:eq(${colIndex + 1})`).find("input").trigger("focus");
                        }

                        //#region NEW ROW
                        if (newRow) {
                            let datos = tblInsumos.DataTable().rows().data();
                            datos.push({
                                insumo: '',
                                insumoDesc: '',
                                cantidad: 0,
                                areaAlmacen: '',
                                ladoAlmacen: '',
                                ladoEstante: '',
                                nivelEstante: '',
                                color_resguardo: 0,
                                marca: '',
                                modelo: '',
                                color: '',
                                num_serie: '',
                                valor_activo: '',
                                costo_promedio: '',
                                plan_desc: '',
                                condiciones: 'B'
                            });
                            tblInsumos.DataTable().clear();
                            tblInsumos.DataTable().rows.add(datos).draw();
                            tblInsumos.find('.inputInsumo').last().focus();
                            newRow = false;
                        }
                        //#endregion
                    });

                    // tblInsumos.on("keypress", "input", function (e) {
                    tblInsumos.on("keypress", ".inputInsumo, .inputInsumoDesc, .inputCantidad, .inputAreaAlmacen, .inputLadoAlmacen, .inputEstanteAlmacen", function (e) {
                        if (e.keyCode === 13) {
                            let nextInput = $(this).closest("td").next("td").find("input");
                            if (nextInput.attr("disabled")) {
                                nextInput.closest("td").next("td").find("input").focus();
                            } else {
                                nextInput.focus();
                            }
                            colIndex = $(this).closest("td").parent().children().index($(this).closest("td"));
                        }
                    });

                    tblInsumos.on("keypress", ".inputNivelAlmacen", function (e) {
                        if (e.keyCode === 13) {
                            fncVerificarCamposTblInsumos();
                            if (flagCrearRow) {
                                // fncCrearRowInsumo();

                                // let datos = tblInsumos.DataTable().rows().data();
                                // datos.push({
                                //     insumo: '',
                                //     insumoDesc: '',
                                //     cantidad: 0,
                                //     areaAlmacen: '',
                                //     ladoAlmacen: '',
                                //     ladoEstante: '',
                                //     nivelEstante: '',
                                //     color_resguardo: 0,
                                //     marca: '',
                                //     modelo: '',
                                //     color: '',
                                //     num_serie: '',
                                //     valor_activo: '',
                                //     costo_promedio: '',
                                //     plan_desc: '',
                                //     condiciones: 'B'
                                // });
                                // tblInsumos.DataTable().clear();
                                // tblInsumos.DataTable().rows.add(datos).draw();
                                // btnQuitarInsumo.prop("disabled", false);
                                newRow = true;

                                tblInsumos.find('.inputInsumo').last().focus();
                            }
                        }
                    });

                    tblInsumos.on('click', '.btnUbicacionDetalle', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblInsumos.DataTable().row(row).data();

                        _filaInsumo = row;

                        let cc = selectCC.val();
                        let almacenID = selectAlmacen.val();
                        let insumo = rowData.id_activo;

                        $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                        $.post('/Enkontrol/Requisicion/GetUbicacionDetalle', { cc, almacenID, insumo })
                            .always($.unblockUI)
                            .then(response => {
                                if (response.success) {
                                    if (response.data != null) {
                                        AddRows(tblUbicacion, response.data);

                                        if ($(this).data('listUbicacionMovimiento') != undefined && $(this).data('listUbicacionMovimiento') != null) {
                                            let listUbicacionMovimiento = $(this).data('listUbicacionMovimiento');
                                            let tablaData = tblUbicacion.DataTable().rows().data();

                                            listUbicacionMovimiento.forEach(item => {
                                                let renglonData = tablaData.toArray().find(x => {
                                                    return x.area_alm == item.area_alm &&
                                                        x.lado_alm == item.lado_alm &&
                                                        x.estante_alm == item.estante_alm &&
                                                        x.nivel_alm == item.nivel_alm
                                                });

                                                if (renglonData != undefined) {
                                                    renglonData.cantidadMovimiento = item.cantidadMovimiento;
                                                }
                                            });

                                            tblUbicacion.DataTable().clear();
                                            tblUbicacion.DataTable().rows.add(tablaData).draw();
                                        }

                                        mdlUbicacionDetalle.modal('show');
                                    }
                                } else {
                                    AlertaGeneral(`Alerta`, `Error al recuperar la información.`);
                                }
                            }, error => {
                                AlertaGeneral(
                                    `Operación fallida`,
                                    `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`
                                );
                            }
                            );
                    });

                    tblInsumos.on('keyup', '.inputAreaAlmacen, .inputLadoAlmacen, .inputEstanteAlmacen, .inputNivelAlmacen', function () {
                        $(this).val($(this).val().toUpperCase());
                    });
                },
                createdRow: function (row, rowData) {
                    reInicializarCamposRenglon(row, rowData);
                },
                columns: [
                    {
                        data: 'id_activo', title: 'Insumo', render: function (data, type, row, meta) {
                            let input = document.createElement('input');
                            input.classList.add('form-control', 'text-center', 'inputInsumo', 'tgInsumo');
                            $(input).attr('value', data != 0 ? data : '');
                            // $(input).attr("tabindex", tabIndexTblInsumos++);
                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'insumoDesc', title: 'Nombre Insumo', render: function (data, type, row, meta) {
                            let input = document.createElement('input');
                            input.classList.add('form-control', 'text-center', 'inputInsumoDesc', 'tgInsumo');
                            $(input).attr('value', data);
                            $(input).attr("type", "text");
                            // $(input).attr("tabindex", tabIndexTblInsumos++);
                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'cantidad_resguardo', title: 'Cantidad', render: function (data, type, row, meta) {
                            let input = document.createElement('input');
                            input.classList.add('form-control', 'text-center', 'inputCantidad', 'tgInsumo');
                            $(input).attr('value', !isNaN(data) && data > 0 ? data : '');
                            // $(input).attr("tabindex", tabIndexTblInsumos++);
                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'ultimaCompra', title: 'Último precio', render: function (data, type, row, meta) {
                            let input = document.createElement('input');
                            input.classList.add('form-control', 'text-center', 'inputUltimoPrecio');
                            $(input).attr('value', !isNaN(data) && data > 0 ? data : 0);
                            // $(input).attr("tabindex", tabIndexTblInsumos++);
                            $(input).attr("disabled", true);
                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'area_alm', title: 'Área', render: function (data, type, row, meta) {
                            let input = document.createElement('input');
                            input.classList.add('form-control', 'text-center', 'inputAreaAlmacen', 'tgInsumo');
                            $(input).attr('value', data);
                            // $(input).attr("tabindex", tabIndexTblInsumos++);
                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'lado_alm', title: 'Lado', render: function (data, type, row, meta) {
                            let input = document.createElement('input');
                            input.classList.add('form-control', 'text-center', 'inputLadoAlmacen', 'tgInsumo');
                            $(input).attr('value', data);
                            // $(input).attr("tabindex", tabIndexTblInsumos++);
                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'estante_alm', title: 'Estante', render: function (data, type, row, meta) {
                            let input = document.createElement('input');
                            input.classList.add('form-control', 'text-center', 'inputEstanteAlmacen', 'tgInsumo');
                            $(input).attr('value', data);
                            // $(input).attr("tabindex", tabIndexTblInsumos++);
                            return input.outerHTML;
                        }
                    },
                    {
                        data: 'nivel_alm', title: 'Nivel', render: function (data, type, row, meta) {
                            let input = document.createElement('input');
                            input.classList.add('form-control', 'text-center', 'inputNivelAlmacen', 'tgInsumo');
                            $(input).attr('value', data);
                            // $(input).attr("tabindex", tabIndexTblInsumos++);
                            return input.outerHTML;
                        }
                    },
                    {
                        sortable: false,
                        render: (data, type, row, meta) => {
                            let valor = data != '' && data != undefined ? data : 0;

                            return `<div class="input-group">
                                        <span class="input-group-btn">
                                            <button class="btn btn-xs btn-default btnUbicacionDetalle">
                                                <i class="fa fa-align-justify"></i>
                                            </button>
                                        </span>
                                    </div>`;
                        },
                        title: 'Escoger Ubicación'
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '5%', targets: [7] }
                ]
            });

            if (inputEmpresaActual.val() == 6) {
                // tblInsumos.DataTable().column(8).visible(false);
            }
        }

        function fncVerificarCamposTblInsumos() {
            tblInsumos.find("tbody tr").each(function (index) {
                let rowIndex = index;
                let rowData = tblInsumos.DataTable().row(rowIndex).data();
                let tdInsumo = 0;
                let tdCantidad = 0;
                if (rowData != undefined) {
                    tdInsumo = rowData.id_activo;
                    tdCantidad = rowData.cantidad_resguardo;
                    if ((tdInsumo <= 0 || tdCantidad <= 0) || (tdInsumo == undefined || tdCantidad == undefined)) {
                        flagCrearRow = false;
                    } else {
                        flagCrearRow = true;
                    }
                } else {
                    flagCrearRow = true;
                }
            });
        }

        function initTblInsumosDevolucion() {
            tblInsumosDevolucion.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                scrollCollapse: true,
                bInfo: false,
                searching: false,
                initComplete: function (settings, json) {
                    tblInsumosDevolucion.on('click', '.checkBoxParcial', function () {
                        let row = $(this).closest('tr');

                        row.find('.inputCantidad').attr('disabled', !($(this).prop('checked')));
                    });
                },
                createdRow: function (row, rowData) {
                },
                columns: [
                    { data: 'id_activo', title: 'Insumo' },
                    { data: 'insumoDesc', title: 'Nombre Insumo' },
                    {
                        data: 'cantidad_resguardo', title: 'Cantidad', render: function (data, type, row, meta) {
                            let input = document.createElement('input');

                            input.classList.add('form-control', 'text-center', 'inputCantidad');

                            $(input).attr('value', !isNaN(data) && data > 0 ? data : '');
                            $(input).attr('disabled', true);

                            return input.outerHTML;
                        }
                    },
                    { data: 'area_alm', title: 'Área' },
                    { data: 'lado_alm', title: 'Lado' },
                    { data: 'estante_alm', title: 'Estante' },
                    { data: 'nivel_alm', title: 'Nivel' },
                    {
                        title: 'Devolver', render: function (data, type, row, meta) {
                            let div = document.createElement('div');

                            let checkbox = document.createElement('input');
                            checkbox.id = 'checkboxParcial_' + meta.row;
                            checkbox.setAttribute('type', 'checkbox');
                            checkbox.classList.add('form-control');
                            checkbox.classList.add('regular-checkbox');
                            checkbox.classList.add('checkBoxParcial');
                            checkbox.style.height = '25px';

                            let label = document.createElement('label');
                            label.setAttribute('for', checkbox.id);

                            $(div).append(checkbox);
                            $(div).append(label);

                            return div.outerHTML;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTblEmpleados() {
            tblEmpleados.DataTable({
                retrieve: true,
                bLengthChange: false,
                deferRender: true,
                // searching: false,
                dom: 'lrtp',
                language: dtDicEsp,
                bInfo: false,
                ordering: false,
                initComplete: function (settings, json) {
                    tblEmpleados.on('click', '.btnSeleccionarEmpleado', function () {
                        let rowData = tblEmpleados.DataTable().row($(this).closest('tr')).data();

                        // inputEmpleadoCustodiaNum.val(rowData.empleado);
                        // inputEmpleadoCustodiaDesc.val(rowData.descripcion);
                        inputNum.val(rowData.empleado);
                        inputNom.val(rowData.descripcion);

                        mdlEmpleados.modal('hide');
                    });
                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'empleado', title: 'Número' },
                    { data: 'descripcion', title: 'Empleado' },
                    { data: 'puesto', title: 'Puesto' },
                    { data: 'puestoDesc', title: 'Descripción del Puesto' },
                    { data: 'telefono', title: 'Teléfono' },
                    {
                        data: 'monto_inicial', title: 'Monto Inicial', render: function (data, type, row, meta) {
                            if (data != null) {
                                return formatMoney(data);
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'monto', title: 'Monto Final', render: function (data, type, row, meta) {
                            if (data != null) {
                                return formatMoney(data);
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'vobo_monto_inicial', title: 'VoBo Monto Inicial', render: function (data, type, row, meta) {
                            if (data != null) {
                                return formatMoney(data);
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'vobo_monto_final', title: 'VoBo Monto Final', render: function (data, type, row, meta) {
                            if (data != null) {
                                return formatMoney(data);
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'vobo', title: 'VoBo', render: function (data, type, row, meta) {
                            if (data == 'S') {
                                return 'Sí';
                            } else {
                                return 'No';
                            }
                        }
                    },
                    {
                        title: 'Seleccionar', render: function (data, type, row, meta) {
                            return `<button class="btn btn-sm btn-default btnSeleccionarEmpleado"><i class="fa fa-arrow-right"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });

            if (+inputEmpresaActual.val() == 6) {
                tblEmpleados.DataTable().column(2).visible(false);
                tblEmpleados.DataTable().column(3).visible(false);
                tblEmpleados.DataTable().column(4).visible(false);
                tblEmpleados.DataTable().column(5).visible(false);
                tblEmpleados.DataTable().column(6).visible(false);
                tblEmpleados.DataTable().column(7).visible(false);
                tblEmpleados.DataTable().column(8).visible(false);
                tblEmpleados.DataTable().column(9).visible(false);
            }
        }

        function initTableUbicacion() {
            tblUbicacion.DataTable({
                retrieve: true,
                paging: false,
                deferRender: true,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblUbicacion.on('click', '.btnCantidadTotalSalida', function () {
                        let rowData = tblUbicacion.DataTable().row($(this).closest('tr')).data();
                        let $row = $(this).closest('tr');

                        if (_filaInsumo != null) {
                            let dataFilaInsumo = tblInsumos.DataTable().row(_filaInsumo).data();

                            dataFilaInsumo.area_alm = rowData.area_alm;
                            dataFilaInsumo.lado_alm = rowData.lado_alm;
                            dataFilaInsumo.estante_alm = rowData.estante_alm;
                            dataFilaInsumo.nivel_alm = rowData.nivel_alm;

                            tblInsumos.DataTable().row(_filaInsumo).data(dataFilaInsumo).draw();

                            _filaInsumo.find('.inputAreaAlmacen').val(rowData.area_alm);
                            _filaInsumo.find('.inputLadoAlmacen').val(rowData.lado_alm);
                            _filaInsumo.find('.inputEstanteAlmacen').val(rowData.estante_alm);
                            _filaInsumo.find('.inputNivelAlmacen').val(rowData.nivel_alm);

                            _filaInsumo.find('.inputCantidad').change(); //Trigger evento change para asignar ubicación a la data de datatable.

                            mdlUbicacionDetalle.modal('hide');
                        }
                    });
                },
                columns: [
                    { data: 'insumoDesc', title: 'Insumo' },
                    { data: 'cantidad', title: 'Cantidad' },
                    { data: 'area_alm', title: 'Área' },
                    { data: 'lado_alm', title: 'Lado' },
                    { data: 'estante_alm', title: 'Estante' },
                    { data: 'nivel_alm', title: 'Nivel' },
                    {
                        sortable: false,
                        data: 'cantidadMovimiento',
                        render: (data, type, row, meta) => {
                            let valor = data != undefined ? data : '';

                            return `<div class="input-group">
                                        <span class="input-group-btn">
                                            <button class="btn btn-xs btn-default btnCantidadTotalSalida" type="button">
                                                <i class="fa fa-arrow-right"></i>
                                            </button>
                                        </span>
                                    </div>`;
                        },
                        title: 'Escoger'
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '50%', targets: [0] }
                ]
            });
        }

        function reInicializarCamposRenglon(row, rowData) {
            let inputInsumo = $(row).find('.inputInsumo');
            let inputInsumoDesc = $(row).find('.inputInsumoDesc');

            if (selectCC.val() != "") {
                inputInsumo.getAutocompleteValid(setInsumoDesc, validarInsumo, { cc: selectCC.val() }, '/Enkontrol/Requisicion/getInsumos');
                inputInsumoDesc.getAutocompleteValid(setInsumoBusqPorDesc, validarInsumo, { cc: selectCC.val() }, '/Enkontrol/Requisicion/getInsumosDesc');
            } else {
                Alert2Warning("Es necesario seleccionar un CC.");
            }
        }

        function setInsumoDesc(e, ui) {
            let row = $(this).closest('tr');

            row.find('.inputInsumoDesc').val(ui.item.id);

            row.find('.inputUltimoPrecio').val(ui.item.ultimaCompra);

            let rowData = tblInsumos.DataTable().row(row).data();
            rowData.color_resguardo = ui.item.color_resguardo;
        }

        function setInsumoBusqPorDesc(e, ui) {
            let row = $(this).closest('tr');

            row.find('.inputInsumo').val(ui.item.id);
            row.find('.inputInsumoDesc').val(ui.item.value);

            row.find('.inputUltimoPrecio').val(ui.item.ultimaCompra);

            let rowData = tblInsumos.DataTable().row(row).data();

            rowData.color_resguardo = ui.item.color_resguardo;
        }

        function validarInsumo(e, ul) {
            if (ul.item == null) {
                let row = $(this).closest('tr');

                row.find('.inputInsumo').val('');
                row.find('.inputInsumoDesc').val('');

                let rowData = tblInsumos.DataTable().row(row).data();

                if (rowData != undefined) {
                    rowData.color_resguardo = 0;
                }
            }
        }

        function asignarPanelDerecho() {
            let rowActive = tblInsumos.find('tbody tr.active');

            if (rowActive.length > 0) {
                let rowData = tblInsumos.DataTable().row(rowActive).data();

                rowData.marca = inputMarca.val();
                rowData.modelo = inputModelo.val();
                rowData.color = inputColor.val();
                rowData.num_serie = inputNumSerie.val();
                rowData.valor_activo = inputValorActivo.val();
                rowData.costo_promedio = inputCostoProm.val();
                rowData.plan_desc = textAreaComentarios.val();
                rowData.condiciones =
                    formRadioBtnCondiciones.find('input[type="radio"]:checked').length > 0 ?
                        formRadioBtnCondiciones.find('input[type="radio"]:checked').val() : 'B';

                // tblInsumos.DataTable().row(rowActive).data(rowData).draw();
                tblInsumos.DataTable().row(rowActive).data(rowData);

                reInicializarCamposRenglon(rowActive, rowData);

                rowActive.addClass("active")
            }
        }

        function habilitarPanelDerecho() {
            inputMarca.attr('disabled', false);
            inputModelo.attr('disabled', false);
            inputColor.attr('disabled', false);
            inputNumSerie.attr('disabled', false);
            inputValorActivo.attr('disabled', false);
            inputCostoProm.attr('disabled', false);
            textAreaComentarios.attr('disabled', false);
        }

        function deshabilitarPanelDerecho() {
            inputMarca.attr('disabled', true);
            inputModelo.attr('disabled', true);
            inputColor.attr('disabled', true);
            inputNumSerie.attr('disabled', true);
            inputValorActivo.attr('disabled', true);
            inputCostoProm.attr('disabled', true);
            textAreaComentarios.attr('disabled', true);
        }

        function llenarPanelDerecho(rowData) {
            if (rowData != undefined && rowData != null) {
                inputMarca.val(rowData.marca);
                inputModelo.val(rowData.modelo);
                inputColor.val(rowData.color);
                inputNumSerie.val(rowData.num_serie);
                inputValorActivo.val(rowData.valor_activo);
                inputCostoProm.val(rowData.costo_promedio);
                textAreaComentarios.val(rowData.plan_desc);
                formRadioBtnCondiciones.find('input[value="' + rowData.condiciones + '"]').attr('checked', true);
                formRadioBtnCondiciones.find('input[value="' + rowData.condiciones + '"]').click();
            }
        }

        function limpiarPantalla() {
            limpiarPanelIzquierdo();
            limpiarPanelDerecho();

            inputEmpleadoCustodiaNum.val('');
            inputEmpleadoCustodiaDesc.val('');
            iContador = 0;

            tblInsumos.DataTable().clear().draw();
        }

        function limpiarPanelIzquierdo() {
            inputAutorizoNum.val('');
            inputAutorizoDesc.val('');
            inputEstatus.val('');
            inputRecibio.val('');
            inputFechaDevolucion.val('');
            inputCondiciones.val('');
        }

        function limpiarPanelDerecho() {
            inputMarca.val('');
            inputModelo.val('');
            inputColor.val('');
            inputNumSerie.val('');
            inputValorActivo.val('');
            inputCostoProm.val('');
            textAreaComentarios.val('');
            formRadioBtnCondiciones.find('input[type="radio"]').attr('checked', false);
        }

        function cargarResguardo() {
            dataResguardo = [];
            btnRegresar.attr('disabled', true);

            axios.post('/Enkontrol/Resguardo/GetResguardo', { cc: selectCC.val(), almacen: selectAlmacen.val(), folio: inputFolio.val() }).then(response => {
                let { success, data, message } = response.data;

                if (success) {
                    btnCambio.attr('disabled', !(data.length > 0 && data[0].estatus != 'D'));

                    if (data.length > 0) {
                        data.forEach((element, index, array) => {
                            dataResguardo.push({
                                cc: selectCC.val(),
                                folio: inputFolio.val(),
                                id_activo: element.id_activo,
                                id_tipo_activo: element.id_tipo_activo,
                                insumo: element.id_activo,
                                insumoDesc: element.insumoDesc,
                                cantidad_resguardo: element.cantidad_resguardo,
                                parcial: false,
                                recibio: inputRecibioDevolucionNum.val(),
                                condiciones: 'B',
                                alm_entrada: element.alm_entrada,
                                alm_salida: element.alm_salida,
                                area_alm: element.area_alm,
                                lado_alm: element.lado_alm,
                                estante_alm: element.estante_alm,
                                nivel_alm: element.nivel_alm,

                                marca: element.marca,
                                modelo: element.modelo,
                                color: element.color,
                                num_serie: element.num_serie,
                                valor_activo: element.valor_activo,
                                plan_desc: element.plan_desc,

                                entrega: inputEntregoNum.val(),
                                autoriza: inputAutorizoNum.val(),
                            });
                        });

                        llenarInformacion(data);

                        let flagVigentes = data.some(function (element) {
                            return element.estatus != 'D';
                        });

                        if (flagVigentes) {
                            btnRegresar.attr('disabled', false);
                        }
                    }
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }

        function agregarTooltip(elemento, mensaje) {
            $(elemento).attr('data-toggle', 'tooltip');
            $(elemento).attr('data-placement', 'top');

            if (mensaje != "") {
                $(elemento).attr('title', mensaje);
            }

            $('[data-toggle="tooltip"]').tooltip({
                position: {
                    my: "center bottom-20",
                    at: "center top+8",
                    using: function (position, feedback) {
                        $(this).css(position);
                        $("<div>")
                            .addClass("arrow")
                            .addClass(feedback.vertical)
                            .addClass(feedback.horizontal)
                            .appendTo(this);
                    }
                }
            });
        }

        function quitarTooltip(elemento) {
            $(elemento).removeAttr('data-toggle');
            $(elemento).removeAttr('data-placement');
            $(elemento).removeAttr('title');
        }

        function llenarInformacion(data) {
            AddRows(tblInsumos, data);

            let informacion = data[0];

            inputEntregoNum.val(informacion.entrega);
            inputEntregoDesc.val(informacion.entregaNombre);
            inputAutorizoNum.val(informacion.autoriza);
            inputAutorizoDesc.val(informacion.autorizaNombre);

            //#region Estatus general del resguardo
            let resguardoVigente = data.every(function (x) { return x.estatus == 'V' });
            let resguardoParcial = data.some(function (x) { return x.estatus == 'P' });
            let resguardoDevuelto = data.every(function (x) { return x.estatus == 'D' });
            let resguardoCancelado = data.every(function (x) { return x.estatus == 'C' });

            if (resguardoVigente) {
                inputEstatus.val('VIGENTE');
            } else if (resguardoParcial) {
                inputEstatus.val('VIGENTE PARCIAL')
            } else if (resguardoDevuelto) {
                inputEstatus.val('DEVUELTO');
            } else if (resguardoCancelado) {
                inputEstatus.val('CANCELADO');
            }
            //#endregion

            inputRecibio.val(informacion.recibioNombre);
            inputFechaDevolucion.val(
                informacion.fec_devolucion != null ? $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(informacion.fec_devolucion.substr(6)))) : ''
            );

            inputCondiciones.val(formRadioBtnCondiciones.find('input[value="' + informacion.condiciones + '"]').attr('texto'));
            inputMarca.val(informacion.marca);
            inputModelo.val(informacion.modelo);
            inputColor.val(informacion.color);
            inputNumSerie.val(informacion.num_serie);
            inputValorActivo.val(informacion.valor_activo == 0 || informacion.valor_activo == null ? maskNumero(0) : maskNumero(informacion.valor_activo));
            inputCostoProm.val(informacion.costo_promedio != null ? informacion.costo_promedio.toFixed(2) : '');
            textAreaComentarios.val(informacion.observaciones);

            formRadioBtnCondiciones.find('input[value="' + informacion.condiciones + '"]').attr('checked', true);
            formRadioBtnCondiciones.find('input[value="' + informacion.condiciones + '"]').click();

            inputEmpleadoCustodiaNum.val(informacion.empleado);
            inputEmpleadoCustodiaDesc.val(informacion.empleadoNombre);
            inputFechaResguardo.val(informacion.fec_resguardo != null ? $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(informacion.fec_resguardo.substr(6)))) : '');
        }

        function limpiarFiltros() {
            inputFiltroEmpleado.val('');
            inputFiltroNombre.val('');

            tblEmpleados.DataTable().columns().search('').draw();
        }

        function limpiarEmpleado() {
            inputEmpleadoNombre.val('');
            inputEmpleadoPaterno.val('');
            inputEmpleadoMaterno.val('');
            inputEmpleadoFechaNacimiento.val('');
            inputEmpleadoRFC.val('');
        }

        function guardarNuevoEmpleado() {
            let empleado = {
                nom_empleado: inputEmpleadoNombre.val(),
                ap_paterno_empleado: inputEmpleadoPaterno.val(),
                ap_materno_empleado: inputEmpleadoMaterno.val(),
                rfc_empleado: inputEmpleadoRFC.val()
            }

            if (empleado.rfc_empleado != '' || inputEmpresaActual.val() == 6) {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Resguardo/GuardarNuevoEmpleado', { empleado })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            limpiarEmpleado();
                            mdlNuevoEmpleado.modal('hide');

                            inputNum.val(response.data.empleado);
                            inputNom.val(response.data.descripcion);
                        } else {
                            AlertaGeneral(`Alerta`, `Error al guardar la información.`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            } else {
                AlertaGeneral(`Alerta`, `Debe capturar un RFC válido.`);
            }
        }

        function setEmpleadoDesc(inputClave, inputNombre) {
            let claveEmpleado = inputClave.val();

            if (!isNaN(claveEmpleado) && claveEmpleado > 0) {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Resguardo/GetEmpleadoNomina', { claveEmpleado })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            if (response.empleado != null) {
                                let nombre = response.empleado.nombre;
                                let apellidoPaterno = response.empleado.ape_paterno;
                                let apellidoMaterno = response.empleado.ape_materno;

                                inputNombre.val(`${nombre} ${apellidoPaterno} ${apellidoMaterno}`);
                            } else {
                                inputClave.val('');
                                inputNombre.val('');

                                AlertaGeneral(`Alerta`, `No existe el número de empleado.`);
                            }
                        } else {
                            AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            }
        }

        function setEmpleadoNum(event, ui) {
            inputClaveEmpleado.val(ui.item.id);
        }

        function setEmpleadoNumCambio(event, ui) {
            inputClaveEmpleadoCambio.val(ui.item.id);
        }

        function verificarPresupuestoCC() {
            let presupuesto = selectCC.find('option:selected').attr('data-prefijo');

            btnGuardarAsignacion.attr('disabled', presupuesto == 'T');
            btnGuardarDevolucion.attr('disabled', presupuesto == 'T');
        }

        function descargarExcelUsuariosQueNoCoinciden() {
            location.href = `DescargarExcelUsuariosEnkontrolNoCoinciden`;
        }

        String.prototype.parseDate = function () {
            return new Date(parseInt(this.replace('/Date(', '')));
        }

        Date.prototype.parseDate = function () {
            return this;
        }

        Date.prototype.addDays = function (days) {
            var date = new Date(this.valueOf());
            date.setDate(date.getDate() + days);
            return date;
        }

        $.fn.commasFormat = function () {
            this.each(function (i) {
                $(this).change(function (e) {
                    if (isNaN(parseFloat(this.value))) return;
                    this.value = parseFloat(this.value).toFixed(6);
                });
            });

            return this;
        }

        function formatMoney(amount, decimalCount = 6, decimal = ".", thousands = ",") {
            try {
                decimalCount = Math.abs(decimalCount);
                decimalCount = isNaN(decimalCount) ? 2 : decimalCount;

                const negativeSign = amount < 0 ? "-" : "";

                let i = parseInt(amount = Math.abs(Number(amount) || 0).toFixed(decimalCount)).toString();
                let j = (i.length > 3) ? i.length % 3 : 0;

                return negativeSign + (j ? i.substr(0, j) + thousands : '') + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousands) + (decimalCount ? decimal + Math.abs(amount - i).toFixed(decimalCount).slice(2) : "");
            } catch (e) {
                console.log(e)
            }
        }

        function fncGenerarRFC() {
            var CURP = [];

            fechaNac = Number($("#inputEmpleadoFechaNacimiento").val().slice(6, 10));
            CURP[0] = $("#inputEmpleadoPaterno").val().charAt(0).toUpperCase();
            CURP[1] = $("#inputEmpleadoPaterno").val().slice(1).replace(/\a\e\i\o\u/gi, "").charAt(0).toUpperCase();
            CURP[2] = $("#inputEmpleadoMaterno").val().charAt(0).toUpperCase();
            CURP[3] = $("#inputEmpleadoNombre").val().charAt(0).toUpperCase();
            CURP[4] = fechaNac.toString().slice(2);
            CURP[5] = $("#inputEmpleadoFechaNacimiento").val().slice(3, 5);
            CURP[6] = $("#inputEmpleadoFechaNacimiento").val().slice(0, 2);
            $("#inputEmpleadoRFC").val(CURP.join(""));
        }

        init();
    }

    $(document).ready(function () {
        Enkontrol.Almacen.Resguardo.Asignacion = new Asignacion();
    })
        // .ajaxStart(function () { $.blockUI({ message: 'Procesando...' }); })
        // .ajaxStop(function () { $.unblockUI(); })
        ;
})();