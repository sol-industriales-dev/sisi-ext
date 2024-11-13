(function () {
    $.namespace('Enkontrol.Compras.Requisicion.Captura');
    Captura = function () {
        tblInsumos = $("#tblInsumos");
        selTipoReq = $("#selTipoReq");
        selAutorizo = $("#selAutorizo");
        selLab = $("#selLab");
        selCC = $("#selCC");
        selOT = $("#selOT");
        dtFecha = $("#dtFecha");
        txtNum = $("#txtNum");
        inputTipoRequisicionPERU = $("#inputTipoRequisicionPERU");
        txtFolio = $('#txtFolio');
        txtSolicito = $("#txtSolicito");
        txtEmpNum = $("#txtEmpNum");
        txtEmpNom = $("#txtEmpNom");
        txtUsuNom = $("#txtUsuNom");
        txtUsuNum = $("#txtUsuNum");
        txtEstatus = $("#txtEstatus");
        txtComentarios = $("#txtComentarios");
        txtDescPartida = $("#txtDescPartida");
        txtModificacion = $("#txtModificacion");
        btnLimpiar = $("#btnLimpiar");
        btnGuardar = $("#btnGuardar");
        btnAddRenglon = $("#btnAddRenglon");
        btnRemRenglon = $("#btnRemRenglon");
        spanEstatus = $("#spanEstatus");
        spanActivos = $("#spanActivos");
        radioBtn = $('.radioBtn a');
        selectTipoFolio = $('#selectTipoFolio');
        txtFolioOrigen = $('#txtFolioOrigen');
        btnFoliosDisponibles = $('#btnFoliosDisponibles');
        mdlFoliosDisponibles = $('#mdlFoliosDisponibles');
        tblFoliosDisponibles = $('#tblFoliosDisponibles');
        mdlEstatusObservaciones = $('#mdlEstatusObservaciones');
        btnGuardarObservaciones = $('#btnGuardarObservaciones');
        inputCantidadTotal = $('#inputCantidadTotal');
        mdlExistenciaDetalle = $('#mdlExistenciaDetalle');
        ulComentarios = $("#ulComentarios");
        btnAddComentario = $("#btnAddComentario");
        txtObservaciones = $("#txtObservaciones");
        checkboxConsigna = $('#checkboxConsigna');
        checkboxConvenio = $('#checkboxConvenio');
        checkboxCRC = $('#checkboxCRC');
        checkboxLicitacion = $('#checkboxLicitacion');
        checkboxTMC = $('#checkboxTMC');
        selectComprador = $('#selectComprador');
        labelEstatusCompra = $('#labelEstatusCompra');
        btnImprimir = $('#btnImprimir');
        report = $("#report");
        btnBorrarRequisicion = $('#btnBorrarRequisicion');
        dialogConfirmarBorrado = $('#dialogConfirmarBorrado');
        btnInfoEnter = $('#btnInfoEnter');
        labelCompras = $('#labelCompras');
        btnConsultaInsumos = $('#btnConsultaInsumos');
        modalConsultaInsumos = $('#modalConsultaInsumos');
        inputUsuario = $('#inputUsuario');
        inputUso = $('#inputUso');
        botonCalcularExistencias = $('#botonCalcularExistencias');
        selectProveedor = $('#selectProveedor');
        botonTabulador = $('#botonTabulador');
        modalTabulador = $('#modalTabulador');
        tablaTabulador = $('#tablaTabulador');
        botonTabuladorAgregarPartidas = $('#botonTabuladorAgregarPartidas');
        tituloModalTabulador = $('#tituloModalTabulador');
        btnVerOT = $('#btnVerOT');
        modalOT = $('#modalOT');
        //#region CONST GENERAR LINK
        const btnListadoLinks = $('#btnListadoLinks');
        const mdlListadoLinks = $('#mdlListadoLinks');
        const tblCom_ProveedoresLinks = $('#tblCom_ProveedoresLinks');
        const btnNuevoProveedorLink = $('#btnNuevoProveedorLink');
        const btnCENuevoProveedorLink = $('#btnCENuevoProveedorLink');
        const cbo_CEProveedorLink_Proveedor = $("#cbo_CEProveedorLink_Proveedor");
        const txt_CEProveedorLink_NumRequisicion = $("#txt_CEProveedorLink_NumRequisicion");
        const divListadoLinks = $('#divListadoLinks');
        const divCEProveedorLink = $('#divCEProveedorLink');
        const btnCENuevoProveedorLinkCancelar = $('#btnCENuevoProveedorLinkCancelar');
        const btnCENuevoProveedorLinkCerrar = $('#btnCENuevoProveedorLinkCerrar');
        const cbo_CEProveedorLink_FiltroProveedor = $('#cbo_CEProveedorLink_FiltroProveedor');
        let dtProveedorLink;
        //#endregion

        const inputEmpresaActual = $('#inputEmpresaActual');

        _renglonObservaciones = null;
        currenInsumo = 0;
        currentPartida = 0;
        arryComentarios = [];
        _flagPeriodoContable = false;
        _flagEditarRequisicion = false;

        _flagInitTerminado = 0;
        _idBL = 0;

        _vistaActual = $.post("/Base/getVistaActual");


        var url_string = window.location.href;
        var url = new URL(url_string);
        const esServicio = url.searchParams.get("id") == "2";

        const getReq = (cc, num, esServicio) => $.post("/Enkontrol/Requisicion/GetRequisicion", { cc, num, esServicio });

        function init() {
            const variables = getUrlParams(window.location.href);
            if (variables && variables.idBL) {
                _idBL = variables.idBL;

                var clean_uri = location.protocol + "//" + location.host + location.pathname;
                window.history.replaceState({}, document.title, clean_uri);
            }

            initForm(_idBL);

            $('.select2').select2();
            selCC.on('change', function (event, numRequi) {
                if (numRequi) {
                    setNewReq(numRequi);
                } else {
                    setNewReq();
                }
            });
            txtNum.change(setReq);
            selTipoReq.change(setFechaReq);
            radioBtn.on('click', function () { aClick(this); });
            btnGuardar.click(guardarRequisicion);
            btnAddRenglon.click(AddNewRenglon);
            btnRemRenglon.click(RemSelRenglon);
            btnAddComentario.click(insertCommentary);
            btnImprimir.click(verReporte);
            btnBorrarRequisicion.click(borrarRequisicion);
            btnConsultaInsumos.click(() => { modalConsultaInsumos.modal('show') });
            botonCalcularExistencias.click(calcularExistencias);
            botonTabulador.click(cargarTabulador);
            botonTabuladorAgregarPartidas.click(agregarPartidasTabulador);

            setFromStorage();

            agregarTooltip(btnBorrarRequisicion, 'Borrar Requisición');
            agregarTooltip(btnFoliosDisponibles, 'Folios Disponibles');
            agregarTooltip(selLab, 'Dirección de Envío');
            initTblFoliosDisponibles();
            initTablaTabulador();
            // OcultarColumnaAreaCuenta();
            // Se comprueba si hay variables en url.
            if (variables && variables.cc && variables.req) {
                selCC.val(variables.cc);
                txtNum.val(variables.req);
                setReq();
            }

            checkPeriodoContable();

            $("#btnVerOT").click(async function () {
                if (selOT.val() == '') {
                    AlertaGeneral('Alerta', 'Seleccione un OT');
                    return;
                }
            
                var url = 'http://localhost:3000/api/ot-by-id?id=' + selOT.val();
                const data = await fetch(url);
                const ot = await data.json();
            
                // Asignar los valores a los elementos del modal
                $("#otFolio").text(ot.folio || 'N/A');
                $("#otFolioDetalle").val(ot.folio || 'N/A');
                $("#otTipo").val(ot.tipo || 'N/A');
                $("#otDescripcion").val(ot.descripcion || 'N/A');
                $("#otEquipo").val(ot.equipo ? ot.equipo.nombre : 'N/A');
                $("#otModelo").val(ot.modelo ? ot.modelo.nombre : 'N/A');
                $("#otCentroCosto").val(ot.centroCosto ? ot.centroCosto.nombre : 'N/A');
                $("#otRazonSocial").val(ot.razonSocial ? ot.razonSocial.nombre : 'N/A');
            
                let supervisorNombre = ot.supervisor ? `${ot.supervisor.nombre} ${ot.supervisor.apaterno} ${ot.supervisor.amaterno}` : 'N/A';
                $("#otSupervisor").val(supervisorNombre);
                $("#otCliente").val(ot.cliente ? ot.cliente.nombre : 'N/A');
                $("#otUbicacion").val(ot.ubicacion ? ot.ubicacion.nombre : 'N/A');
                $("#otUrgente").val(ot.urgente ? 'Sí' : 'No');
                $("#otFechaInicio").val(ot.it_fechaInicio ? new Date(ot.it_fechaInicio).toLocaleDateString() : 'N/A');
                $("#otNoEconomico").val(ot.noEconomico || 'N/A');
                $("#otEstadoFlujo").val(ot.estadoFlujo || 'N/A');
            
                // Mostrar el modal
                $("#modalOT").modal('show');
            });
        }


        // function OcultarColumnaAreaCuenta() {
        //     if (inputEmpresaActual.val() == 6) {
        //         tblInsumos.DataTable().column(4).visible(false);
        //     } else {
        //         tblInsumos.DataTable().column(4).visible(true);
        //     }
        // }
        const getUrlParams = function (url) {
            let params = {};
            let parser = document.createElement('a');
            parser.href = url;
            let query = parser.search.substring(1);
            let vars = query.split('&');
            for (let i = 0; i < vars.length; i++) {
                let pair = vars[i].split('=');
                params[pair[0]] = decodeURIComponent(pair[1]);
            }
            return params;
        };

        $(document).on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        btnLimpiar.on('click', function () {
            setDefault(false);
        });

        tblInsumos.on("click", ".existencia", function (e) {
            let row = $(this).closest('tr'),
                data = row.data();

            tablaExistenciaDetalle(getExistenciaDetalle(data.insumo));
        });

        tblInsumos.on("click", ".existenciaBoton", function (e) {
            let row = $(this).closest('tr'),
                data = row.data();

            tablaExistenciaDetalle(getExistenciaDetalle(data.insumo));
        });

        tblInsumos.on("click", "tbody tr", function (e) {
            tblInsumos.find("tbody tr").each(function (idx, row) {
                if ($(this).data().partida == txtDescPartida.data().partida) {
                    $(this).data().DescPartida = txtDescPartida.val();
                }
            });

            let row = $(this).closest('tr').data();

            txtDescPartida.data().partida = row.partida;
            txtDescPartida.val(row.DescPartida);

            let target = $(e.target);

            if (!(target.is("input") || target.is("select"))) {
                txtDescPartida.focus();
            }

            var selected = $(this).hasClass("active");

            tblInsumos.find("tr").removeClass("active");
            txtDescPartida.prop('disabled', true);

            if (!selected) {
                $(this).not("th").addClass("active");
                txtDescPartida.prop('disabled', false);
            }
        });

        tblInsumos.on("click", ".radioBtn a", function (e) {
            aClick(this);
        });

        tblInsumos.on("keypress", ".insumo", function (e) {
            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) //solo digitos
                return false;
        });

        tblInsumos.on("keypress", ".cantidad", function (e) {
            if (e.which != 0 && e.which != 8 && e.which != 46 && e.which != 110 && e.which != 190 && (e.which < 48 || e.which > 57)) //solo digitos
                return false;
        });

        tblInsumos.on("change", ".cantidad", function (e) {
            let row = $(this).closest('tr'),
                data = row.data(),
                valor = +(this.value);
            row.find(".cantidad");

            getCantidadTotal();
        });

        selectProveedor.on('change', function () {
            tblInsumos.find('tbody tr .insumo').change(); //Se carga la información de cada partida para ver si tienen registro de insumo consigna/CRC/convenio.
        });

        tblInsumos.on('change', '.insumo', function () {
            let insumo = $(this).val();
            let row = $(this).closest('tr');

            if (inputEmpresaActual.val() != 6) {
                if (insumo.length != 7) {
                    return;
                }
            } else {
                if (insumo.length != 11) {
                    return;
                }
            }

            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/Requisicion/GetInsumoInformacion', { insumo, esServicio }).always($.unblockUI).then(response => {
                if (response.success) {
                    let ins = response.data;
                    let isAreaCueta = ins.isAreaCueta;
                    let row = $(this).closest('tr');

                    row.find('.insumoDesc').val(ins.id);
                    row.find('.unidad').text(ins.unidad);
                    row.find('.existencia').text(getExistencia(ins.value, 400, $("#selLab").val() != '' ? $("#selLab").val() : 0));
                    row.find('.existenciaBoton').removeClass('hidden');

                    agregarTooltip(row.find('.existenciaBoton'), 'Desglose por Almacén');

                    row.data({
                        isAreaCueta: isAreaCueta,
                        insumo: ins.value
                    });

                    // if (isAreaCueta) {
                    //     if (row.find(".areaCuenta").val() == "000-000") {
                    //         row.find(".areaCuenta").val(row.find(".areaCuenta option:eq(1)").val());
                    //     }
                    // }

                    if (ins.cancelado == 'A') {
                        row.find('.btn-estatus-activo').css('display', 'inline-block');
                        row.find('.btn-estatus-inactivo').css('display', 'none');
                        row.find('.btn-estatus-inactivo').attr('data-observaciones', '');
                    } else {
                        row.find('.btn-estatus-activo').css('display', 'none');
                        row.find('.btn-estatus-inactivo').css('display', 'inline-block');
                        row.find('.btn-estatus-inactivo').attr('data-observaciones', '');
                    }

                    if (ins.compras_req == 0) {
                        row.find('input, select, button').attr('disabled', true);
                        row.find('.existenciaBoton').attr('disabled', false);
                        row.addClass('renglonInsumoBloqueado');

                        AlertaGeneral(`Alerta`, `El insumo "${insumo} - ${ins.id}" está bloqueado.`);
                    }

                    checkRenglonInsumoBloqueado();
                } else {
                    AlertaGeneral(`Alerta`, `Error al consultar la información del insumo.`);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );

            let proveedor = +selectProveedor.val();

            if (proveedor > 0) {
                let consigna = checkboxConsigna.prop('checked');
                let convenio = checkboxConvenio.prop('checked');
                let licitacion = checkboxLicitacion.prop('checked');
                let crc = checkboxCRC.prop('checked');

                if (!crc) {
                    axios.post(consigna ? 'GetInsumoProveedorConsigna' : convenio ? 'GetInsumoProveedorConvenio' : licitacion ? 'GetInsumoProveedorLicitacion' : '', { insumo, proveedor }).then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            if (response.data.data != null) {
                                $(row).find('.celdaPrecio').text(maskNumero6DCompras(response.data.data.precio));
                                $(row).find('.cantidad').change();
                            }
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
                }
            }
        });

        selectTipoFolio.on('change', function () {
            txtFolio.fillCombo('/Enkontrol/Requisicion/FillComboTxtFolio', { tipo: selectTipoFolio.val() });
        });

        txtFolio.on('change', function () {
            setReqOrigen(txtFolio.val());
        });

        tblInsumos.on('click', '.btn-estatus-inactivo', function (e) {
            _renglonObservaciones = $(this).closest('tr');
            datos = _renglonObservaciones.data();
            currenInsumo = datos.insumo;
            currentPartida = datos.partida;

            let comentarios = [];

            arryComentarios.forEach(function (currentValue, index, array) {
                if (currentValue.partida == currentPartida) {
                    comentarios.push(currentValue);
                }
            });

            setComentarios(comentarios);

            txtComentarios.val("");
            mdlEstatusObservaciones.modal("show");
        });

        btnFoliosDisponibles.on('click', function () {
            mdlFoliosDisponibles.modal('show');
        });

        btnGuardarObservaciones.on('click', function () {
            mdlEstatusObservaciones.modal('hide');
        });

        tblInsumos.on('focus', 'input', function () {
            $(this).select();
        });

        txtDescPartida.on('change', function () {
            let renglonActivo = tblInsumos.find('tbody tr.active');

            if (renglonActivo.length > 0) {
                $(renglonActivo).data().DescPartida = txtDescPartida.val();
            } else {
                AlertaGeneral(`Alerta`, `Seleccione una partida para capturar la descripción.`);
                txtDescPartida.val('');
            }
        });

        tblInsumos.on('keyup', function (event) {
            if (event.keyCode === 13) {
                // Cancel the default action, if needed
                // event.preventDefault();

                //#region Función "AddNewRenglon" para simular click al botón "btnAddRenglon"
                let cuerpo = tblInsumos.find('tbody');

                if (cuerpo.find("tr td").prop("colspan") == 11) {
                    cuerpo.empty();
                }
                if (validaCamposRenglones()) {
                    let partida = ++cuerpo.find("tr").length;
                    _renglonNuevo(partida, true, false).done(function (_renglonNuevo) {
                        let row = initRenglonInsumo($(_renglonNuevo), partida);

                        cuerpo.append(row);

                        txtDescPartida.prop("disabled", false);
                        btnRemRenglon.prop("disabled", false);
                        window.scrollTo(0, document.body.scrollHeight);
                        $('.select2').select2();
                        tblInsumos.find('.insumo').last().focus();
                    });
                }

                getCantidadTotal();
                //#endregion
            }
        });

        btnInfoEnter.on('click', function () {
            AlertaGeneral(``, `Para agregar una partida nueva puedes dar click al botón azul con el símbolo "+" o 
            presionar la tecla "Enter" después de capturar el insumo y la cantidad. Todas las partidas deben tener un insumo válido y una cantidad mayor a cero.`);
        });

        checkboxConsigna.on('change', function () {
            selectProveedor.fillCombo('/Enkontrol/Requisicion/FillComboProveedoresConsignaLicitacionConvenio', { tipo: $(this).prop('checked') ? 1 : 0 }, false, null);
        });

        checkboxLicitacion.on('change', function () {
            selectProveedor.fillCombo('/Enkontrol/Requisicion/FillComboProveedoresConsignaLicitacionConvenio', { tipo: $(this).prop('checked') ? 2 : 0 }, false, null);
        });

        checkboxCRC.on('change', function () {
            selectProveedor.fillCombo('/Enkontrol/Requisicion/FillComboProveedoresConsignaLicitacionConvenio', { tipo: $(this).prop('checked') ? 3 : 0 }, false, null);
        });

        checkboxConvenio.on('change', function () {
            selectProveedor.fillCombo('/Enkontrol/Requisicion/FillComboProveedoresConsignaLicitacionConvenio', { tipo: $(this).prop('checked') ? 4 : 0 }, false, null);
        });

        //#region CARGA DE INSUMOS QUE VIENEN DE BACKLOGS
        function setNewReqBL(partes) {
            let cc = selCC.val();

            _flagEditarRequisicion = false;

            $.blockUI({ message: 'Cargando...' });
            $.when(
                selAutorizo.fillCombo('/Enkontrol/Requisicion/FillComboResponsablePorCc', { cc: cc }, false, null),
                selectComprador.fillCombo('/Enkontrol/OrdenCompra/FillComboCompradoresCC', { cc: cc }, false, null),
                getNewReq(cc).done(function (response) {
                    txtNum.val(response.numero);
                    txtSolicito.val(response.solicitoNom).data().solicito = response.solicito;
                    txtEstatus.val("No autorizó").data().st_autoriza = false;

                    labelEstatusCompra.css('display', 'none');

                    btnGuardar.attr('disabled', false);
                    tblInsumos.find('tbody').empty();

                    fncSetPartidas(partes);
                }).always($.unblockUI)
            ).then(selTipoReq.find("option:eq(1)").prop("selected", true));
        }

        function fncGetDatosBL(idBL) {
            if (idBL > 0) {
                let obj = new Object();
                obj = {
                    idBL: idBL
                };
                axios.post("/BackLogs/GetDatosBL", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        selCC.val(response.data.objBL.cc);
                        setNewReqBL(response.data.objBL.lstPartes);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        let _contadorPartidasBL = 1;
        function fncSetPartidas(lstPartes) {
            _renglonNuevoBL(_contadorPartidasBL, true, false).done(function (_renglon) {
                let $renglon = setInsumoBL(initRenglonInsumo($(_renglon)), lstPartes[_contadorPartidasBL - 1], _contadorPartidasBL);

                agregarTooltip($renglon.find('.btn-estatus-activo'), 'ACTIVO');
                agregarTooltip($renglon.find('.btn-estatus-inactivo'), 'INACTIVO');

                tblInsumos.find(`tbody`).append($renglon);
                $renglon.find('.existencia').text(getExistencia(lstPartes[_contadorPartidasBL - 1].insumo, 400, $("#selLab").val() != '' ? $("#selLab").val() : 0));
                $renglon.find('.existenciaBoton').removeClass('hidden');

                agregarTooltip($renglon.find('.existenciaBoton'), 'Desglose por Almacén');
                $renglon.data({ insumo: lstPartes[_contadorPartidasBL - 1].insumo });
                $renglon.find('.selectAlmacen').fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtir', null, false, null);
                $('.select2').select2();

                _contadorPartidasBL++;

                if (_contadorPartidasBL - 1 < lstPartes.length) {
                    fncSetPartidas(lstPartes);
                } else {
                    _contadorPartidasBL = 0;
                }
            });
        }

        function _renglonNuevoBL(partida, nuevo, cancelado) {
            return $.post('/Enkontrol/Requisicion/_renglonNuevo', { partida, nuevo, cancelado });
        }

        function setInsumoBL(row, partida, numPartida) {
            console.log(partida)
            if ($("#inputEmpresaActual").val() == 6) {
                row.find('.insumo').val(partida.PERU_insumo).change();
            } else {
                row.find('.insumo').val(partida.insumo).change();
            }
            row.find('.tipoPartida').val(partida.tipoPartida);
            row.find('.tipoPartidaDet').val(partida.tipoPartidaDet);
            row.find('.insumoDesc').val(partida.articulo);
            row.find('.areaCuenta').val("000-000");
            row.find('.cantidad').val(partida.cantidad).change();
            row.find('.unidad').val(partida.unidad);
            row.find('.unidad').text(partida.unidad);

            row.data({
                id: 0,
                idReq: 0,
                partida: numPartida,
                DescPartida: '',
                exceso: 0,
                isAreaCueta: false
            });

            row = setRowRadioValue(row, `radCancel${partida.partida}`, partida.cant_cancelada != 0);

            if (partida.compras_req == 0) {
                row.find('input, select, button').attr('disabled', true);
                row.addClass('renglonInsumoBloqueado');

                AlertaGeneral(`Alerta`, `El insumo "${partida.insumo} - ${partida.insumoDesc}" está bloqueado.`);
            }

            return row;
        }

        function initRenglonInsumo(row, partida) {
            row.find('.insumo').getAutocomplete(setInsumoDesc, { cc: selCC.val() }, '/Enkontrol/Requisicion/getInsumos');
            row.find('.insumoDesc').getAutocomplete(setInsumoBusqPorDesc, { cc: selCC.val() }, '/Enkontrol/Requisicion/getInsumosDesc');
            row.find('.tipoPartida').fillCombo('/Enkontrol/Requisicion/FillComboTipoPartida', { cc: selCC.val() }, true);
            row.find('.tipoPartidaDet').fillCombo('/Enkontrol/Requisicion/FillComboTipoPartidaDet', { tipo: 0 }, true);
            row.find('.tipoPartida').on('change', function () {
                var tipoPartida = $(this).val();
                row.find('.tipoPartidaDet').fillCombo('/Enkontrol/Requisicion/FillComboTipoPartidaDet', { tipo: tipoPartida }, true);
            });
            row.find('.areaCuenta').fillCombo('/Enkontrol/Requisicion/FillComboAreaCuenta', { cc: selCC.val() }, false, "000-000");
            row.find(".cantidad").val(0).commasFormat().change();

            agregarTooltip(row.find('.btn-estatus-activo'), 'ACTIVO');
            agregarTooltip(row.find('.btn-estatus-inactivo'), 'INACTIVO');

            row.data({
                id: 0,
                idReq: 0,
                partida: partida,
                DescPartida: "",
                exceso: 0,
                isAreaCueta: false,
                tipoPartida: 0,
                tipoPartidaDet: 0
            });

            return row;
        }

        function initGetDatosBL() {
            if (_idBL != 0 && _idBL != null && _idBL != '' && _idBL != undefined) {
                fncGetDatosBL(_idBL);
            }
        }

        function initTerminado() {
            _flagInitTerminado += 1;
            if (_flagInitTerminado == 6) {
                setDefault(false);

                btnRemRenglon.prop("disabled", false);

                initGetDatosBL();
            }
        }
        //#endregion

        function initTablaTabulador() {
            tablaTabulador.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                order: ['1', 'asc'],
                bInfo: false,
                scrollY: '45vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tablaTabulador.on('focus', '.inputCantidadArticulo', function () {
                        $(this).select();
                    });

                    tablaTabulador.on('change', '.inputCantidadArticulo', function () {
                        let row = $(this).closest('tr');
                        let rowData = tablaTabulador.DataTable().row(row).data();
                        let celdaImporte = $(row).find('.celdaImporte');
                        let cantidad = +$(this).val();

                        if (cantidad > 0) {
                            let importe = cantidad * rowData.precio;
                            celdaImporte.text(maskNumero6DCompras(importe));
                        } else {
                            celdaImporte.text('');
                        }
                    });
                },
                columns: [
                    { data: 'insumo', title: 'Insumo' },
                    // { data: 'proveedor', title: 'Proveedor' },
                    { data: 'articulo', title: 'Artículo' },
                    { data: 'unidad', title: 'Unidad' },
                    {
                        data: 'precio', title: 'Precio', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return maskNumero6DCompras(data);
                            } else {
                                return data;
                            }
                        }, visible: false
                    },
                    {
                        title: 'Cantidad', render: function (data, type, row, meta) {
                            return `<input class="form-control text-center inputCantidadArticulo">`;
                        }, width: '10%'
                    },
                    {
                        title: 'Importe', className: 'dt-center celdaImporte', render: function (data, type, row, meta) {
                            return ``;
                        }, visible: false
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarTabulador() {
            let licitacion = checkboxLicitacion.prop('checked');
            let proveedor = +selectProveedor.val();

            if (!licitacion) {
                Alert2Warning('Debe seleccionar la casilla de licitación.');
                return;
            }

            if (proveedor == 0 || isNaN(proveedor)) {
                Alert2Warning('Debe seleccionar un proveedor.');
                return;
            }

            axios.post('/Enkontrol/Requisicion/GetArticulosConsignaLicitacionConvenioPorProveedor', { proveedor, tipo: 2 }).then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    let proveedorDesc = selectProveedor.find('option:selected').text();

                    if (licitacion) {
                        tituloModalTabulador.text(`Tabulador Licitación [${proveedorDesc}]`);
                    }

                    tablaTabulador.DataTable().clear().draw();
                    tablaTabulador.DataTable().rows.add(response.data.data).draw(false);

                    modalTabulador.modal('show');
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function agregarPartidasTabulador() {
            $.blockUI({ message: 'Procesando...', baseZ: 9000 });

            let listaArticulos = [];
            let ultimaPartidaAgregada = +tblInsumos.find('tbody tr:last td:first').text();
            let siguientePartida = 1;

            if (ultimaPartidaAgregada > 0) {
                siguientePartida += ultimaPartidaAgregada;
            }

            let importeTotal = 0;

            tablaTabulador.find('tbody tr').each(function (idx, row) {
                let cantidad = +$(row).find('.inputCantidadArticulo').val();

                if (cantidad > 0) {
                    let rowData = tablaTabulador.DataTable().row(row).data();

                    listaArticulos.push({
                        insumo: rowData.insumo,
                        articulo: rowData.articulo,
                        unidad: rowData.unidad,
                        precio: rowData.precio,
                        cantidad: cantidad,
                        partida: siguientePartida++,
                        cancelado: 'A',
                        insumoDesc: rowData.insumoDesc,
                        area: 0,
                        cuenta: 0,
                        observaciones: '',
                        cantidadCapturada: cantidad,
                        partidaDesc: `Artículo: ${rowData.articulo}`,
                        exceso: 0,
                        isAreaCueta: false,
                        id: 0,
                        idReq: 0,
                        cant_cancelada: 0,
                        compras_req: 1
                    });

                    importeTotal += rowData.precio * cantidad;
                }
            });

            if (listaArticulos.length > 0) {
                if (isNaN(ultimaPartidaAgregada) || ultimaPartidaAgregada == 0) {
                    tblInsumos.find('tbody').empty();
                }

                setPartidas(listaArticulos);

                if (isNaN(ultimaPartidaAgregada) || ultimaPartidaAgregada == 0) {
                    txtDescPartida.val(listaArticulos[0].partidaDesc);
                    tblInsumos.find('tbody tr:first').click();
                }
            }

            $.unblockUI();
        }

        function calcularExistencias() {
            let almacen = selLab.val();
            let listaInsumos = [];

            tblInsumos.find('tbody tr').each(function (idx, row) {
                listaInsumos.push(+$(row).find('.insumo').val())
            });

            if (listaInsumos.length > 0) {
                axios.post('/Enkontrol/Requisicion/CalcularExistenciasRequisicion', { almacen, listaInsumos })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            let i = 0;
                            tblInsumos.find('tbody tr').each(function (idx, row) {
                                $(row).find('.celdaExistencia').text(response.data.data[i++]);
                            });
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        }

        function initTblFoliosDisponibles() {
            tblFoliosDisponibles.DataTable({
                paging: false,
                info: false,
                searching: false,
                language: dtDicEsp,
                scrollCollapse: true,
                columns: [
                    { data: 'folio', title: 'Folio' },
                    { data: 'descripcion', title: 'Descripción' },
                    {
                        sortable: false,
                        render: function (data, type, row, meta) {
                            let buttonAgregar = document.createElement('button');
                            let icono = document.createElement('i');
                            $(buttonAgregar).addClass('btn btn-sm btn-primary btn-agregar');
                            $(icono).addClass('far fa-arrow-right');
                            $(buttonAgregar).append(icono);

                            return buttonAgregar.outerHTML;
                        }
                    }
                ],
                columnDefs: [
                    { "className": "dt-center", "targets": "_all" }
                ],
                initComplete: function (settings, json) {

                }
            });
        }

        function getNewReq(cc) {
            labelCompras.text('');
            return $.post("/Enkontrol/Requisicion/getNewReq", { cc: cc });
        }

        function _renglonNuevo(partida, nuevo, cancelado) {
            return $.post('/Enkontrol/Requisicion/_renglonNuevo', { partida, nuevo, cancelado });
        }

        function _renglonVacio() {
            tblInsumos.find('tbody').load('/Enkontrol/Requisicion/_renglonVacio');
        }

        function setNewReq(numRequi) {
            let cc = selCC.val();

            _flagEditarRequisicion = false;

            if (numRequi) {
                $.blockUI({ message: 'Cargando...' });

                selAutorizo.fillCombo('/Enkontrol/Requisicion/FillComboResponsablePorCc', { cc: cc }, false, null);
                selectComprador.fillCombo('/Enkontrol/OrdenCompra/FillComboCompradoresCC', { cc: cc }, false, null);
                selOT.fillCombo('http://66.175.239.161/api/ot-list-by-cc', { cc: cc }, false, null);
                labelEstatusCompra.css('display', 'none');
                btnGuardar.attr('disabled', false);
                tblInsumos.find('tbody').empty();

                txtEstatus.val("No autorizó").data().st_autoriza = false;

                txtNum.val(numRequi);
                txtNum.trigger('change');
            } else {
                $.blockUI({ message: 'Cargando...' });

                $.when(
                    selAutorizo.fillCombo('/Enkontrol/Requisicion/FillComboResponsablePorCc', { cc: cc }, false, null),
                    selectComprador.fillCombo('/Enkontrol/OrdenCompra/FillComboCompradoresCC', { cc: cc }, false, null),
                    selOT.fillCombo('http://66.175.239.161/api/ot-list-by-cc', { cc: cc }, false, null),
                    getNewReq(cc).done(function (response) {
                        txtNum.val(response.numero);
                        labelEstatusCompra.css('display', 'none');

                        btnGuardar.attr('disabled', false);
                        tblInsumos.find('tbody').empty();
                        setDefault(true);

                        txtSolicito.val(response.solicitoNom).data().solicito = response.solicito;
                        txtEstatus.val("No autorizó").data().st_autoriza = false;
                    }).always($.unblockUI)
                ).then(selTipoReq.find("option:eq(1)").prop("selected", true));
            }
        }

        function setReq() {
            btnBorrarRequisicion.attr('disabled', true);
            btnListadoLinks.attr("disabled", true);
            _flagEditarRequisicion = false;

            labelCompras.text('');

            $.blockUI({ message: 'Procesando...' });
            getReq(selCC.val(), txtNum.val(), inputTipoRequisicionPERU.val() == "RS").done(function (response) {
                if (response.success) {
                    if (!response.requisicionNueva) {
                        if (response.req.otroCC) {
                            setDefault(true);
                            selCC.val(response.req.cc);
                            selOT.val(response.req.otID);
                            if (selCC.val()) {
                                selCC.trigger('change', txtNum.val());
                            } else {
                                AlertaGeneral('Alerta', 'El número de requisición ' + txtNum.val() + ' se encuentra en el CC ' + response.req.cc + ' pero no lo tiene asignado');
                            }
                        } else {
                            _flagEditarRequisicion = true;
                            selOT.val(response.req.otID);
                            tblInsumos.find('tbody').empty();
                            setPartidas(response.partidas);
                            setRequisicion(response.req);
                            txtDescPartida.val(response.partidas[0].partidaDesc);
                            btnRemRenglon.prop("disabled", false);
                            labelCompras.text(response.req.comprasString != '' ? 'Compras: ' + response.req.comprasString : '');

                            // if (response.req.st_autoriza == 'S') {
                            btnImprimir.attr('disabled', false);
                            // } else {
                            //     btnImprimir.attr('disabled', true);
                            btnBorrarRequisicion.attr('disabled', false);
                            // }

                            //#region 
                            btnListadoLinks.attr("disabled", false);
                            //#endregion

                            $('#mdlDetalleReq').modal('show');
                        }
                    } else {
                        setDefault(true);
                        txtNum.val(response.ultimaRequisicionNumero);
                        tblInsumos.find('tbody').empty();
                    }
                } else {
                    setDefault(true);
                }
            }).always($.unblockUI);
        }

        function setReqOrigen(folio) {
            $.blockUI({ message: 'Procesando...' });
            let cc = "";
            let numero = "";

            switch (folio) {
                case "2526":
                    cc = "050";
                    numero = 170;
                    break;
                case "2527":
                    cc = "159";
                    numero = 1157;
                    break;
                case "2528":
                    cc = "114";
                    numero = 31484;
                    break;
                case "1324":
                    cc = "011";
                    numero = 369;
                    break;
                case "1325":
                    cc = "012";
                    numero = 532;
                    break;
                case "1326":
                    cc = "111";
                    numero = 5866;
                    break;
                default:
                    setDefault(false);
                    return 0;
                    break;
            }
            getReq(cc, numero).done(function (response) {
                if (response.success) {
                    if (!response.requisicionNueva) {
                        tblInsumos.find('tbody').empty();
                        setPartidas(response.partidas);
                        setRequisicion(response.req);
                        txtDescPartida.val(response.partidas[0].partidaDesc);
                        btnRemRenglon.prop("disabled", false);

                        $('#mdlDetalleReq').modal('show');
                    } else {
                        setDefault(true);
                        txtNum.val(response.ultimaRequisicionNumero);
                        tblInsumos.find('tbody').empty();
                    }
                    selCC.change();
                } else {
                    // AlertaGeneral('Alerta', 'Error al consultar la información.');
                    setDefault(true);
                }
            }).always($.unblockUI);
        }

        function guardarRequisicion() {
            //#region Validaciones
            if (!_flagPeriodoContable) {
                AlertaGeneral(`Alerta`, `El Periodo Contable no está activo.`);
                return;
            }

            if (!validaCamposRenglones()) {
                AlertaGeneral('Alerta', 'Información no válida en las partidas.');
                return;
            }

            if (selLab.val() == '') {
                AlertaGeneral('Alerta', 'Seleccione L.A.B.');
                selLab.addClass('campoInvalido');
                return;
            }

            if (selTipoReq.val() == '') {
                AlertaGeneral('Alerta', 'Seleccione el tipo de Requisición.');
                selTipoReq.addClass('campoInvalido');
                return;
            }

            if (+(txtNum.val()) <= 0) {
                AlertaGeneral(`Alerta`, `Número de requisición no válido.`);
                return;
            }
            //#endregion

            let req = getRequisicion();
            let det = getPartidas();
            let comentarios = arryComentarios;

            //Se asigna el número cero cuando es requisición nueva para que entre correctamente en la validación del backend.
            if (!_flagEditarRequisicion) {
                req.numero = 0;
            }

            let maquinaDesc = selCC.find('option:selected').text();

            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/Requisicion/guardar', { req, det, comentarios })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        setNewReq();

                        //#region backlog
                        if (_idBL != 0) {
                            _idBL = 0;

                            let textBL = '';
                            if (response.flagMaquinaStandBy) {
                                textBL = 'Requisición guardada. Número de requisición: ' + response.numeroRequisicionNueva + '.\nSe quitó el estado "Stand-By" de la máquina ' + maquinaDesc + '.\n' +
                                    'Presione "aceptar" para regresar a la pagina anterior';
                            }
                            else {
                                textBL = 'Operación exitosa. Número de requisición: ' + response.numeroRequisicionNueva + '.\n' +
                                    'Presione "aceptar" para regresar a la pagina anterior';
                            }
                            swal({
                                title: 'Confirmación!',
                                text: textBL,
                                icon: "success",
                                buttons: true,
                                dangerMode: true,
                                buttons: ["Cerrar", "Aceptar"]
                            })
                                .then((aceptar) => {
                                    if (aceptar) {
                                        document.location.href = '/BackLogs/RegistroBackLogsObra';
                                    } else {
                                        location.reload();
                                    }
                                });
                        }
                        //#endregion
                        else {
                            if (response.flagMaquinaStandBy) {
                                AlertaGeneral(`Alerta`, `Requisición guardada. Número de requisición: ${response.numeroRequisicionNueva}. 
                                Se quitó el estado "Stand-By" de la máquina "${maquinaDesc}".`);
                            } else {
                                AlertaGeneral("Requisición", `Operación exitosa. Número de requisición: ${response.numeroRequisicionNueva}.`);
                            }
                        }
                    } else {
                        AlertaGeneral(`Alerta`, `No se guardó la información. ${response.message}`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );

            selTipoReq.removeClass('campoInvalido');
            selLab.removeClass('campoInvalido');
        }

        function AddNewRenglon() {
            let cuerpo = tblInsumos.find('tbody');
            if (cuerpo.find("tr td").prop("colspan") == 12)
                cuerpo.empty();
            if (validaCamposRenglones()) {
                let partida = ++cuerpo.find("tr").length;
                _renglonNuevo(partida, true, false).done(function (_renglonNuevo) {
                    let row = initRenglonInsumo($(_renglonNuevo), partida);

                    cuerpo.append(row);

                    // txtDescPartida.prop("disabled", false);
                    btnRemRenglon.prop("disabled", false);

                    $('.select2').select2();
                });
            }

            getCantidadTotal();
        }

        function RemSelRenglon() {
            tblInsumos.find("tr.active").remove();
            let cuerpo = tblInsumos.find('tbody');
            txtDescPartida.prop('disabled', true);
            if (cuerpo.find("tr").length == 0) {
                _renglonVacio();
                btnRemRenglon.prop("disabled", true);
            } else {
                tblInsumos.find("tbody tr").each(function (idx, row) {
                    $(row).find("td").eq(0).text(idx + 1);
                });
            }

            checkRenglonInsumoBloqueado();
            getCantidadTotal();
        }

        function validaCamposRenglones() {
            let ban = true;

            tblInsumos.find("tbody tr").each(function (idx, row) {
                let $row = $(row);

                if (inputEmpresaActual.val() != 6) {
                    if ($row.find(".insumo").val().length != 7) {
                        ban = false;
                    }
                } else {
                    if ($row.find(".insumo").val().length != 11) {
                        ban = false;
                    }
                }

                if ($row.data().isAreaCueta) {
                    if ($row.find(".insumo").val() == "000-000") {
                        ban = false;
                    }
                }

                if (+($row.find(".cantidad").val()) == 0) {
                    ban = false;
                }
            });

            return ban;
        }

        function initRenglonInsumo(row, partida) {
            row.find('.insumo').getAutocomplete(setInsumoDesc, { cc: selCC.val(), esServicio: esServicio }, '/Enkontrol/Requisicion/getInsumos');
            row.find('.insumoDesc').getAutocomplete(setInsumoBusqPorDesc, { cc: selCC.val(), esServicio: esServicio }, '/Enkontrol/Requisicion/getInsumosDesc');
            row.find('.tipoPartida').fillCombo('/Enkontrol/Requisicion/FillComboTipoPartida', { cc: selCC.val() }, true);
            row.find('.tipoPartidaDet').fillCombo('/Enkontrol/Requisicion/FillComboTipoPartidaDet', { tipo: partida.tipoPartidaDet ?? 0 }, true);
            row.find('.tipoPartida').on('change', function () {
                var tipoPartida = $(this).val();
                row.find('.tipoPartidaDet').fillCombo('/Enkontrol/Requisicion/FillComboTipoPartidaDet', { tipo: tipoPartida }, true);
            });
            row.find('.areaCuenta').fillCombo('/Enkontrol/Requisicion/FillComboAreaCuenta', { cc: selCC.val() }, false, "000-000");
            //row.find('.fechaReq').datepicker().datepicker("setDate", new Date().addDays(selTipoReq.find(":selected").data().prefijo).toLocaleDateString());
            row.find(".cantidad").val(0).commasFormat().change();
            //row.find(".existencia").val(0).commasFormat().change();

            agregarTooltip(row.find('.btn-estatus-activo'), 'ACTIVO');
            agregarTooltip(row.find('.btn-estatus-inactivo'), 'INACTIVO');

            row.data({
                id: 0,
                idReq: 0,
                partida: partida,
                DescPartida: "",
                exceso: 0,
                isAreaCueta: false,
                tipoPartida: 0,
                tipoPartidaDet: 0
            });

            return row;
        }

        function setInsumo(row, partida) {
            row.find('.insumo').val(partida.insumo).change();
            row.find('.insumoDesc').val(partida.insumoDesc);
            row.find('.tipoPartida').val(partida.tipoPartida).change();

            //wait 1 seg and set tipoPartidaDet
            setTimeout(function () {
                row.find('.tipoPartidaDet').val(partida.tipoPartidaDet);
            }, 1000);

            if (inputEmpresaActual.val() != 6 && inputEmpresaActual.val() != 3) {
                if (partida.area == 0) {
                    row.find('.areaCuenta').val("000-000");
                } else {
                    row.find(`.areaCuenta option[value="${partida.area}"][data-prefijo="${partida.cuenta}"]`).prop("selected", true).val(partida.area);
                }
            } else {
                if (partida.noEconomico != '') {
                    row.find(`.areaCuenta option[value="${partida.noEconomico}"]`).prop("selected", true).val(partida.noEconomico);
                } else {
                    row.find('.areaCuenta').val("000-000");
                }
            }

            // row.find('.referencia').val(partida.referencia_1);
            row.find('.cantidad').val(partida.cantidad).change();
            row.find('.unidad').val(partida.unidad);
            row.find('.unidad').text(partida.unidad);
            row.find('.btn-estatus').attr('data-observaciones', partida.observaciones != null ? partida.observaciones : '');
            // row.find('.observaciones').val(partida.observaciones);
            row.find('.cantidadCapturada').val(partida.cantidadCapturada);

            row.data({
                id: partida.id,
                idReq: partida.idReq,
                partida: partida.partida,
                DescPartida: partida.partidaDesc,
                exceso: 0,
                isAreaCueta: partida.isAreaCueta
            });

            row = setRowRadioValue(row, `radCancel${partida.partida}`, partida.cant_cancelada != 0);

            if (partida.compras_req == 0) {
                row.find('input, select, button').attr('disabled', true);
                row.addClass('renglonInsumoBloqueado');

                AlertaGeneral(`Alerta`, `El insumo "${partida.insumo} - ${partida.insumoDesc}" está bloqueado.`);
            }

            return row;
        }

        function setInsumoDesc(e, ui) {
            let isAreaCueta = ui.item.isAreaCueta;
            let row = $(this).closest('tr');
            let valor = row.find(".cantidad").val();

            row.find('.insumoDesc').val(ui.item.descripcion);
            row.find('.unidad').val(ui.item.unidad);
            row.find('.unidad').text(ui.item.unidad);
            row.find('.existencia').text(getExistencia(ui.item.value, 400, $("#selLab").val() != '' ? $("#selLab").val() : 0));
            row.find('.existenciaBoton').removeClass('hidden');

            agregarTooltip(row.find('.existenciaBoton'), 'Desglose por Almacén');

            row.data({
                isAreaCueta: isAreaCueta,
                insumo: ui.item.value
            });

            if (ui.item.cancelado == 'A') {
                row.find('.btn-estatus-activo').css('display', 'inline-block');
                row.find('.btn-estatus-inactivo').css('display', 'none');
                row.find('.btn-estatus-inactivo').attr('data-observaciones', '');
            } else {
                row.find('.btn-estatus-activo').css('display', 'none');
                row.find('.btn-estatus-inactivo').css('display', 'inline-block');
                row.find('.btn-estatus-inactivo').attr('data-observaciones', '');
            }

            if (ui.item.compras_req == 0) {
                row.find('input, select, button').attr('disabled', true);
                row.addClass('renglonInsumoBloqueado');

                AlertaGeneral(`Alerta`, `El insumo "${ui.item.value} - ${ui.item.id}" está bloqueado.`);
            }

            checkRenglonInsumoBloqueado();
        }

        function setInsumoBusqPorDesc(e, ui) {
            let isAreaCueta = ui.item.isAreaCueta;
            let row = $(this).closest('tr');
            let valor = row.find(".cantidad").val();

            row.find('.insumo').val(ui.item.id);
            row.find('.insumoDesc').val(ui.item.descripcion);
            row.find('.unidad').val(ui.item.unidad);
            row.find('.unidad').text(ui.item.unidad);
            row.find('.existencia').text(getExistencia(ui.item.id, 400, $("#selLab").val() != '' ? $("#selLab").val() : 0));
            row.find('.existenciaBoton').removeClass('hidden');

            agregarTooltip(row.find('.existenciaBoton'), 'Desglose por Almacén');

            row.data({
                isAreaCueta: isAreaCueta,
                insumo: ui.item.id
            });

            if (ui.item.cancelado == 'A') {
                row.find('.btn-estatus-activo').css('display', 'inline-block');
                row.find('.btn-estatus-inactivo').css('display', 'none');
                row.find('.btn-estatus-inactivo').attr('data-observaciones', '');
            } else {
                row.find('.btn-estatus-activo').css('display', 'none');
                row.find('.btn-estatus-inactivo').css('display', 'inline-block');
                row.find('.btn-estatus-inactivo').attr('data-observaciones', '');
            }

            if (ui.item.compras_req == 0) {
                row.find('input, select, button').attr('disabled', true);
                row.addClass('renglonInsumoBloqueado');

                AlertaGeneral(`Alerta`, `El insumo "${ui.item.id} - ${ui.item.descripcion}" está bloqueado.`);
            }

            checkRenglonInsumoBloqueado();

            return false;
        }

        function getExistencia(insumo, cc, almacen) {
            let existencia = 0;
            $.ajax({
                url: '/Enkontrol/Requisicion/GetExistenciaInsumo',
                datatype: "json",
                type: "GET",
                async: false,
                data: {
                    insumo: insumo,
                    cc: cc,
                    almacen: almacen
                },
                success: function (response) {
                    if (response.success) {
                        existencia = response.existencia;
                    }
                }
            });

            return existencia;
        }

        function getExistenciaDetalle(insumo) {
            let detalle = [];
            $.ajax({
                url: '/Enkontrol/Requisicion/GetExistenciaInsumoDetalle',
                datatype: "json",
                type: "GET",
                async: false,
                data: {
                    insumo: insumo
                },
                success: function (response) {
                    if (response.success) {
                        detalle = response.existencia;
                    }
                }
            });

            return detalle;
        }

        function tablaExistenciaDetalle(detalle) {
            mdlExistenciaDetalle.find('.modal-body').html('');

            let row = '';
            row = document.createElement('div');
            row.setAttribute('style', 'max-height: 70vh; overflow-y: auto;');

            let div = '';
            div = document.createElement('div');
            div.setAttribute('id', 'divTable');
            div.classList.add('col-xs-12');
            div.classList.add('col-sm-12');
            div.classList.add('col-md-12');
            div.classList.add('col-lg-12');

            let table = crearTabla();
            let body = document.createElement('tbody');

            if (detalle != undefined) {
                detalle.forEach(function (detalle) {
                    body.append(crearRowsDetalle(detalle));
                });
            }

            $(table).append(body);
            $(div).append(table);
            $(row).append(div);

            mdlExistenciaDetalle.find('.modal-body').append(row);
            mdlExistenciaDetalle.modal('show');
        }

        function crearTabla() {
            let table = document.createElement('table');
            table.setAttribute('id', 'tblExistenciaDetalle');
            table.setAttribute('style', 'width: 100%; margin-bottom: 0px;');
            table.classList.add('table');
            table.classList.add('tblExistenciaDetalle');

            let head = document.createElement('thead');

            let tr = document.createElement('tr');

            let thAlmacen = document.createElement('th');
            thAlmacen.textContent = 'Almacen';

            let thEntradas = document.createElement('th');
            thEntradas.textContent = 'Entradas';

            let thSalidas = document.createElement('th');
            thSalidas.textContent = 'Salidas';

            let thExistencias = document.createElement('th');
            thExistencias.textContent = 'Existencias';

            let thReservados = document.createElement('th');
            thReservados.textContent = 'Reservados';

            $(tr).append(thAlmacen);
            $(tr).append(thEntradas);
            $(tr).append(thSalidas);
            $(tr).append(thExistencias);
            $(tr).append(thReservados);

            $(head).append(tr);
            $(table).append(head);

            return table;
        }

        function crearRowsDetalle(detalle) {
            let tr = document.createElement('tr');

            let tdAlmacen = document.createElement('td');
            tdAlmacen.textContent = detalle.almacen;

            let tdEntradas = document.createElement('td');
            tdEntradas.setAttribute('align', 'right');
            tdEntradas.textContent = formatValue(detalle.entradas);

            let tdSalidas = document.createElement('td');
            tdSalidas.setAttribute('align', 'right');
            tdSalidas.textContent = formatValue(detalle.salidas);

            let tdExistencia = document.createElement('td');
            tdExistencia.setAttribute('align', 'right');
            tdExistencia.textContent = formatValue(detalle.existencia);

            let tdReservados = document.createElement('td');
            tdReservados.setAttribute('align', 'right');
            tdReservados.textContent = formatValue(detalle.reservados);

            $(tr).append(tdAlmacen);
            $(tr).append(tdEntradas);
            $(tr).append(tdSalidas);
            $(tr).append(tdExistencia);
            $(tr).append(tdReservados);

            return tr;
        }

        function aClick(esto) {
            let sel = $(esto).data('title');
            let tog = $(esto).data('toggle');
            $(`#${tog}`).prop('value', sel);
            $(`a[data-toggle="${tog}"]`).not(`[data-title="${sel}"]`).removeClass('active').addClass('notActive');
            $(`a[data-toggle="${tog}"][data-title="${sel}"]`).removeClass('notActive').addClass('active');
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
        function setRowRadioValue(row, tog, sel) {
            row.find(`#${tog}`).prop('value', sel);
            row.find(`a[data-toggle="${tog}"]`).not(`[data-title="${sel}"]`).removeClass('active').addClass('notActive');
            row.find(`a[data-toggle="${tog}"][data-title="${sel}"]`).removeClass('notActive').addClass('active');
            return row;
        }
        function setRadioValue(tog, sel) {
            $(`#${tog}`).prop('value', sel);
            $(`a[data-toggle="${tog}"]`).not(`[data-title="${sel}"]`).removeClass('active').addClass('notActive');
            $(`a[data-toggle="${tog}"][data-title="${sel}"]`).removeClass('notActive').addClass('active');
        }
        function setFechaReq() {
            let dtReq = new Date().addDays($(this).find(":selected").data().prefijo).toLocaleDateString();
            tblInsumos.find("tbody tr").each(function (idx, row) {
                //$(row).find(".fechaReq").val(dtReq);
            });
        }
        function setDefault(nuevaRequisicion) {
            // selAutorizo.prop("disabled", true).html("");
            btnRemRenglon.prop("disabled", true);
            txtDescPartida.val('');

            txtDescPartida.data({
                partida: 1
            });

            inputCantidadTotal.val('');
            txtFolio.val('');

            txtModificacion.val('');

            setRequisicion({
                id: 0,
                cc: nuevaRequisicion ? selCC.val() : '',
                numero: nuevaRequisicion ? txtNum.val() : '',
                fecha: new Date(),
                fechaString: getFecha(),
                libre_abordo: "",
                tipo_req_oc: "",
                tmc: 0,
                fecha_autoriza: new Date(),
                fecha_modifica: new Date(),
                hora_modifica: new Date(),
                comentarios: "",
                solicitoNom: "",
                solicito: "",
                autorizo: "",
                vobo: "",
                voboNom: "",
                empleado_modifica: "",
                empModificaNom: "",
                st_estatus: "",
                st_impresa: "",
                st_autoriza: "N",
                autoriza_activos: 0,
                num_vobo: 0,
                usuarioSolicita: 0,
                usuarioSolicitaDesc: '',
                usuarioSolicitaUso: ''
            });

            if (selCC.val() == '') {
                txtNum.val('');
                txtEstatus.val('');
            }

            if (nuevaRequisicion) {
                txtEstatus.val('');
                txtModificacion.val('');

                labelEstatusCompra.css('display', 'none');
            }

            inputUsuario.val('');
            inputUso.val('');

            _renglonVacio();
        }
        function setRequisicion(req) {
            selAutorizo.fillCombo('/Enkontrol/Requisicion/FillComboResponsablePorCc', { cc: req.cc }, false, null);
            selCC.val(req.cc);
            if (req.otID) {
                selOT.val(req.otID);
            }
            txtNum.val(req.numero != '-1' ? req.numero : '');
            dtFecha.val(req.fechaString); // dtFecha.val(req.fecha.parseDate().toLocaleDateString());
            selLab.val(req.libre_abordo);

            if (selTipoReq.find('option[value="' + req.tipo_req_oc + '"]').length) {
                selTipoReq.val(req.tipo_req_oc)
            } else {
                AlertaGeneral('Alerta', 'No se encontró el tipo de requisición.');
                selTipoReq.val('1');
            }

            txtFolioOrigen.val(req.folioOrigen);

            setRadioValue("radTmc", req.tmc == 1);
            txtEstatus.val(req.st_autoriza == "N" ? "No autorizada" : "Autorizada");

            labelEstatusCompra.css('display', 'none');
            btnGuardar.attr('disabled', true);

            if (req.numero > 0 && req.st_autoriza == 'S') {
                labelEstatusCompra.css('display', 'inline');
                labelEstatusCompra.css('color', 'green');
                labelEstatusCompra.text('AUTORIZADA');
            } else if (req.numero > 0 && req.st_autoriza == 'N') {
                labelEstatusCompra.css('display', 'inline');
                labelEstatusCompra.css('color', 'red');
                labelEstatusCompra.text('NO AUTORIZADA');

                btnGuardar.attr('disabled', false);
            }

            let fechaModificacion =
                (req.fecha_modificaString != null ? req.fecha_modificaString : '') + ' ' + (req.hora_modificaString != null ? req.hora_modificaString : '');

            txtModificacion.val(req.numero == 0 ? "" : fechaModificacion);
            txtComentarios.val(req.comentarios);
            txtSolicito.val(req.solicitoNom);
            selAutorizo.val(req.autorizo);
            txtEmpNum.val(req.vobo);
            txtEmpNom.text(req.voboNom);
            txtUsuNum.val(req.empleado_modifica);
            txtUsuNom.text(req.empModificaNom);
            txtNum.data({
                id: req.id,
                solicito: req.solicito,
                st_estatus: req.st_estatus,
                st_impresa: req.st_impresa,
                st_autoriza: req.st_autoriza,
                autoriza_activos: req.autoriza_activos,
                num_vobo: req.num_vobo,
                autoriza: req.fecha_autoriza == null ? "" : req.fecha_autoriza.parseDate()
            });
            setInputEstatus(req.st_estatus);
            req.autoriza_activos == 1 ? spanActivos.removeClass("hidden") : spanActivos.addClass("hidden");
            $('#checkboxConsigna').prop('checked', req.consigna);
            $('#checkboxCRC').prop('checked', req.crc);
            $('#checkboxConvenio').prop('checked', req.convenio);

            if (req.consigna || req.licitacion || req.crc || req.convenio) {
                selectProveedor.empty();

                let tipo = 0;

                if (req.consigna) {
                    tipo = 1;
                } else if (req.licitacion) {
                    tipo = 2;
                } else if (req.crc) {
                    tipo = 3;
                } else if (req.convenio) {
                    tipo = 4;
                }

                axios.post('/Enkontrol/Requisicion/FillComboProveedoresConsignaLicitacionConvenio', { tipo }).then(response => {
                    let { success, items, message } = response.data;

                    if (success) {
                        selectProveedor.append('<option value="">--Seleccione--</option>');

                        items.forEach(x => {
                            selectProveedor.append(`<option value="${x.Value}">${x.Text}</option>`);
                        });

                        selectProveedor.val(req.proveedor > 0 ? req.proveedor : '');
                        selectProveedor.select2().change();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }

            checkboxLicitacion.prop('checked', req.licitacion);

            if (!req.consigna && !req.licitacion && !req.crc && !req.convenio) {
                selectProveedor.fillCombo('/Enkontrol/Requisicion/FillComboProveedoresConsignaLicitacionConvenio', { tipo: 0 }, false, null); //Limpiar el combo de proveedores.
            }

            $('#checkboxTMC').prop('checked', req.tmc);

            selectComprador.val(req.comprador > 0 ? req.comprador : '');

            inputUsuario.data().id = req.usuarioSolicita;
            inputUsuario.data().empresa = req.usuarioSolicitaEmpresa != undefined ? req.usuarioSolicitaEmpresa : 0;
            inputUsuario.val(req.usuarioSolicitaDesc);
            inputUso.val(req.usuarioSolicitaUso);
        }
        function setInputEstatus(estatus) {
            let isEstatus = estatus != "";
            switch (estatus) {
                case "T":
                    spanEstatus.removeClass("hidden").text("Total Comprado");
                    break;
                case "P":
                    spanEstatus.removeClass("hidden").text("Parcial Comprado");
                    break;
                case "C":
                    spanEstatus.removeClass("hidden").text("Total Cancelado");
                    break;
                default:
                    spanEstatus.addClass("hidden").text("");
                    break;
            }
        }
        function getRequisicion() {
            return {
                id: txtNum.data().id,
                cc: selCC.val(),
                numero: txtNum.val(),
                fecha: dtFecha.val(),
                idLibreAbordo: selLab.val() > 0 ? selLab.val() : 1,
                idTipoReqOc: selTipoReq.val(),
                solicito: txtNum.data().solicito,
                vobo: txtNum.data().vobo,
                autorizo: selAutorizo.val(),
                comentarios: txtComentarios.val(),
                stEstatus: txtNum.data().st_estatus,
                stImpresa: getEstatusImpresion(),
                stAutoriza: getEstatusAuth(),
                empAutoriza: txtEmpNum.val(),
                empModifica: txtUsuNum.val(),
                modifica: txtModificacion.val(),
                autoriza: txtNum.data().fecha_autoriza,
                isTmc: checkboxTMC.prop('checked'),
                isActivos: getActivos(),
                numVobo: txtNum.data().num_vobo,
                folioAsignado: txtFolio.val(),
                consigna: checkboxConsigna.prop('checked'),
                licitacion: checkboxLicitacion.prop('checked'),
                crc: checkboxCRC.prop('checked'),
                convenio: checkboxConvenio.prop('checked'),
                proveedor: +selectProveedor.val(),
                comprador: selectComprador.val() > 0 ? selectComprador.val() : 0,
                usuarioSolicita: inputUsuario.data().id,
                usuarioSolicitaUso: inputUso.val().trim(),
                usuarioSolicitaEmpresa: inputUsuario.data().empresa,
                idBL: _idBL,
                otID: selOT.val() != "" ? selOT.val() : null,
                otFolio: selOT.val() != "" ? $("#selOT option:selected").text(): "",
            }
        }
        function getEstatusAuth() {
            return txtNum.data().st_autoriza == "S";
        }
        function getEstatusImpresion() {
            return txtNum.data().st_impresa == "I";
        }
        function getActivos() {
            return txtNum.data().autoriza_activos == 1;
        }
        function getPartidas() {
            let list = [];

            tblInsumos.find("tbody tr").each(function (idx, row) {
                let data = $(this).data();
                let celda = $(row).find("td");
                let partida = Number($(row).find('td:eq(0)').text());

                list.push({
                    id: data.id,
                    idReq: txtNum.data().id,
                    partida: partida, // partida: data.partida,
                    insumo: celda.find(".insumo").val(),
                    requerido: null,
                    cantidad: celda.find(".cantidad").val(),
                    precio: unmaskNumero6DCompras($(row).find('.celdaPrecio').text()),
                    cantOrdenada: 0,
                    ordenada: dtFecha.val(),
                    estatus: txtNum.data().st_estatus,
                    cantCancelada: 0,
                    // referencia: celda.find(".referencia").val(),
                    cantExcedida: 0,
                    area: celda.find(".areaCuenta").val() != '' ? celda.find(".areaCuenta").val() : 0,
                    cuenta: celda.find(".areaCuenta").val() != '' ? celda.find(".areaCuenta option:selected").data().prefijo : 0,
                    noEconomico: celda.find(".areaCuenta").val(),
                    descripcion: data.DescPartida,
                    // observaciones: celda.find(".observaciones").val()
                    observaciones: '',
                    tipoPartida: celda.find(".tipoPartida").val(),
                    tipoPartidaDet: celda.find(".tipoPartidaDet").val()
                });
            });

            return list;
        }
        function setPartidas(partidas) {
            $.ajaxSetup({ async: false });

            for (let i = 0; i < partidas.length; i++) {
                _renglonNuevo(partidas[i].partida, false, partidas[i].cancelado == 'A' ? false : true).done(function (_renglon) {
                    let $renglon = setInsumo(initRenglonInsumo($(_renglon), partidas[i]), partidas[i]);

                    agregarTooltip($renglon.find('.btn-estatus-activo'), 'ACTIVO');
                    agregarTooltip($renglon.find('.btn-estatus-inactivo'), 'INACTIVO');

                    tblInsumos.find(`tbody`).append($renglon);
                    $renglon.find('.tipoPartida').val(partidas[i].tipoPartida);  
                    $renglon.find('.tipoPartidaDet').val(partidas[i].tipoPartidaDet);
                    if (_vistaActual.responseJSON != 7221) { //Vista diferente a la de autorización.
                        $renglon.find('.existencia').text(getExistencia(partidas[i].insumo, 400, $("#selLab").val() != '' ? $("#selLab").val() : 0));
                        $renglon.find('.existenciaBoton').removeClass('hidden');

                        agregarTooltip($renglon.find('.existenciaBoton'), 'Desglose por Almacén');
                    }

                    $renglon.data({
                        insumo: partidas[i].insumo,
                        precio: partidas[i].precio > 0 ? partidas[i].precio : 0,
                        tipoPartida: partidas[i].tipoPartida,
                        tipoPartidaDet: partidas[i].tipoPartidaDet
                    });

                    $renglon.find('.selectAlmacen').fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtir', null, false, null);
                    $('.select2').select2();

                    if (partidas[i].precio > 0) {
                        $renglon.find('.celdaPrecio').text(maskNumero6DCompras(partidas[i].precio));
                    }
                });
            }

            checkRenglonInsumoBloqueado();

            $.ajaxSetup({ async: true });

            getCantidadTotal();
            getPrecioTotal();
        }

        function getPrecioTotal() {
            let precioTotal = 0;

            tblInsumos.find('tbody tr').each(function (idx, row) {
                let cantidad = +$(row).find('.cantidad').val();
                let precio = unmaskNumero6DCompras($(row).find('.celdaPrecio').text());

                precioTotal += cantidad * precio;
            });

            $('#inputPrecioTotal').val(maskNumero6DCompras(precioTotal));
        }

        function initForm(idBL) {
            // selCC.fillCombo('/Enkontrol/Requisicion/FillComboCcTodos', null, false, null);
            selCC.fillCombo('/Enkontrol/Requisicion/FillComboCcAsigReq', null, false, null, initTerminado);
            selOT.fillCombo('http://66.175.239.161/api/ot-list-by-cc', { cc: '999' }, false, null);
            $('#selCCDetReq').fillCombo('/Enkontrol/Requisicion/FillComboCcAsigReq', null, false, null, initTerminado);
            selLab.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtir', null, false, null, initTerminado);
            selTipoReq.fillCombo('/Enkontrol/Requisicion/FillComboTipoReq', null, false, null, initTerminado);
            selectTipoFolio.fillCombo('/Enkontrol/Requisicion/FillComboTipoFolio', null, false, null, initTerminado);
            selectComprador.fillCombo('/Enkontrol/OrdenCompra/FillComboCompradores', null, false, null, initTerminado);
            selectProveedor.fillCombo('/Enkontrol/Requisicion/FillComboProveedoresConsignaLicitacionConvenio', { tipo: 0 }, false, null);

            if (idBL == 0) {
                setDefault(false);
            }

            inputUsuario.getAutocompleteValid(setDatosUsuario, verificarUsuario, null, '/Enkontrol/Requisicion/GetEmpleadoEnkontrolAutocomplete');
        }

        function setDatosUsuario(e, ui) {
            let input = $(e.target);

            input.data().id = ui.item.id;
            input.data().empresa = ui.item.empresa;
            input.val(ui.item.nombreEmpleado);
        }

        function verificarUsuario(e, ui) {
            if (ui.item == null) {
                let input = $(e.target);

                input.data().id = 0;
                input.data().empresa = 0;
                input.val('');
            }
        }

        function setFromStorage() {
            let cc = localStorage.getItem("cc"),
                num = localStorage.getItem("num");
            localStorage.removeItem("cc");
            localStorage.removeItem("num");
            if (cc != null) {
                selCC.val(cc);
                txtNum.val(num).change();
            }
        }
        function getCantidadTotal() {
            let cantidadTotal = 0;
            let inputs = tblInsumos.find('tbody tr .cantidad');

            inputs.each(function (index, elemento) {
                cantidadTotal += parseFloat(elemento.value)
            });

            inputCantidadTotal.val(cantidadTotal).commasFormat();
        }

        function setComentarios(data) {
            var htmlComentario = "";

            data.forEach(function (currentValue, index, array) {
                if (currentValue.partida == currentPartida) {
                    htmlComentario += "<li class='comentario' data-id='" + currentValue.id + "'>";
                    htmlComentario += "    <div class='timeline-item'>";
                    htmlComentario += "        <span class='time'><i class='fa fa-clock-o'></i>" + currentValue.fecha + "</span>";
                    htmlComentario += "        <h3 class='timeline-header'><a href='#'>" + currentValue.usuarioNombre + "</a></h3>";
                    htmlComentario += "        <div class='timeline-body'>";
                    htmlComentario += "             " + currentValue.comentario;
                    htmlComentario += "        </div>";
                    htmlComentario += "    </div>";
                    htmlComentario += "</li>";
                }
            });
            ulComentarios.html(htmlComentario);
        }

        function insertCommentary(e) {
            if (validateComentario()) {
                const obj = getNewCommentary();
                obj.usuarioNombre = '';
                arryComentarios.push(obj);

                $("#a" + currenInsumo).find(".portlet-toggle").each(function () {
                    var icon = $(this);
                    icon.closest(".portlet").find(".comentariesCount").html(arryComentarios.length);
                });

                setComentarios(arryComentarios);
                txtObservaciones.val("");

                //var formData = new FormData();
                //formData.append("obj", JSON.stringify(obj));

                // $.ajax({
                //     type: "POST",
                //     url: "/SeguimientoAcuerdos/guardarComentario",
                //     data: formData,
                //     dataType: 'json',
                //     contentType: false,
                //     processData: false,
                //     success: function (response) {
                //         fupAdjunto.val("");
                //         $.unblockUI();
                //         obj.id = Number(response.obj.id);
                //         obj.adjuntoNombre = response.obj.adjuntoNombre;
                //         var data = Enumerable.From(activitiesObj).Where(function (x) { return x.id === currentActivity }).Select(function (x) { return x }).FirstOrDefault();
                //         data.comentarios.push(obj);
                //         data.comentariosCount = data.comentarios.length;
                //         $("#a" + currentActivity).find(".portlet-toggle").each(function () {
                //             var icon = $(this);
                //             icon.closest(".portlet").find(".comentariesCount").html(data.comentariosCount);
                //         });
                //         setComentarios(data.comentarios);
                //         txtObservaciones.val("");
                //     },
                //     error: function (error) {
                //         $.unblockUI();
                //     }
                // });
            } else {
                e.preventDefault()
            }
        }

        function getNewCommentary() {
            var r = {};
            r.id = 0;
            r.insumo = currenInsumo;
            r.comentario = txtObservaciones.val();
            //r.usuarioNombre = usuarioNombre;
            r.fecha = dtFecha.val();
            r.tipo = 'new';
            r.partida = currentPartida;
            return r;
        }

        function validateComentario() {
            var state = true;
            if (!validarCampoComentario(txtObservaciones)) { state = false; }
            return state;
        }

        function validarCampoComentario(_this) {
            var r = false;
            if (_this.val() == '') {
                if (!_this.hasClass("errorClass")) {
                    _this.addClass("errorClass")
                }
                r = false;
            }
            else {
                if (_this.hasClass("errorClass")) {
                    _this.removeClass("errorClass")
                }
                r = true;
            }
            return r;
        }

        function checkRenglonInsumoBloqueado() {
            let existeRenglonBloqueado = tblInsumos.find('tbody .renglonInsumoBloqueado').length > 0;

            btnGuardar.attr('disabled', existeRenglonBloqueado);
        }

        function verReporte() {
            let cc = selCC.val();
            let numero = txtNum.val();
            let PERU_tipoRequisicion = esServicio ? 'RS' : 'RQ'

            if (cc != '' && !isNaN(numero) && numero > 0) {
                $.blockUI({ message: 'Procesando...', baseZ: 2000 });

                report.attr("src", '/Reportes/Vista.aspx?idReporte=112' + '&cc=' + cc + '&numero=' + numero + '&PERU_tipoRequisicion=' + PERU_tipoRequisicion);
                report.on('load', function () {
                    $.unblockUI();
                    openCRModal();
                });
            }
        }

        function borrarRequisicion() {
            labelCompras.text('');
            // if (_flagPeriodoContable) {
            let cc = selCC.val();
            let numero = txtNum.val();

            dialogConfirmarBorrado.text(`¿Desea borrar la requisición ${cc}-${numero}?`);

            dialogConfirmarBorrado.dialog({
                width: '30%',
                modal: true,
                buttons: {
                    'Borrar': function () {
                        btnBorrarRequisicion.attr('disabled', true);
                        btnListadoLinks.attr("disabled", true);

                        $.blockUI({ message: 'Procesando...' });
                        $.post('/Enkontrol/Requisicion/BorrarRequisicion', { cc: cc, numero: numero })
                            .always($.unblockUI)
                            .then(response => {
                                dialogConfirmarBorrado.dialog('close');

                                if (response.success) {
                                    AlertaGeneral(`Alerta`, `Se ha borrado la requisición ${cc}-${numero}.`);

                                    btnLimpiar.click();
                                } else {
                                    AlertaGeneral(`Alerta`, `No se pudo borrar la requisición. ${response.message.length > 0 ? response.message : ``}`);
                                }
                            }, error => {
                                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                            }
                            );

                        dialogConfirmarBorrado.dialog('close');
                    },
                    'Cancelar': function () {
                        dialogConfirmarBorrado.dialog('close');
                    }
                }
            });
            // } else {
            //     AlertaGeneral(`Alerta`, `El Periodo Contable no está activo.`);
            // }
        }

        function checkPeriodoContable() {
            $.post('/Enkontrol/OrdenCompra/GetPeriodoContable').then(response => {
                if (response.success) {
                    if (response.data != null) {
                        _flagPeriodoContable = true;
                        btnGuardar.attr('disabled', false);
                        btnBorrarRequisicion.css('display', 'inline-block');
                    } else {
                        _flagPeriodoContable = false;
                        btnGuardar.attr('disabled', true);
                        btnBorrarRequisicion.css('display', 'none');
                    }
                } else {
                    AlertaGeneral(`Alerta`, `Error al consultar la información del periodo contable.`);
                    _flagPeriodoContable = false;
                    btnGuardar.attr('disabled', true);
                    btnBorrarRequisicion.css('display', 'none');
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                _flagPeriodoContable = false;
                btnGuardar.attr('disabled', true);
                btnBorrarRequisicion.css('display', 'none');
            }
            );
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

        init();
    }
    $(document).ready(function () {
        Enkontrol.Compras.Requisicion.Captura = new Captura();
    });
})();