
(function () {

    $.namespace('SISTEMA.SISTEMA.MenuPrincipalSistemas');

    MenuPrincipalSistemas = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Pantalla principal',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        const tipoAutorizacion = {
            Requisicion: 1,
            OrdenCompra: 2,
            FormatoCambio: 3,
            AditivaDeductiva: 4,
            PlantillaPersonal: 5
        }

        const isMobile = window.innerWidth < 767;

        const botonAutorizar = $('#botonAutorizar');
        // const botonGoTop = $('#botonGoTop');

        const tablaOC = $('#tablaOC');
        let dtTablaOC;

        const tablaReq = $('#tablaReq');
        let dtTablaReq;

        const modalFacultamientosFacturas = $('#modalFacultamientosFacturas');
        const tablaFacultamientosFacturas = $('#tablaFacultamientosFacturas');
        const labelMensajeFacultamientosFacturas = $('#labelMensajeFacultamientosFacturas');
        const labelModalFacultamientosFacturasTitulo = $('#labelModalFacultamientosFacturasTitulo');
        let dtTablaFacultamientosFacturas;

        let _empresaActual = +$('#inputEmpresaActual').val();

        function init() {

            modalSistemas();

            $("#divMenuAccesoPpal").on("click", "a", fnIniciar);

            initTablaFacultamientosFacturas();

            if (isMobile) {
                $(`a.loadsData`).click(e => {
                    const link = $(e.currentTarget);
                    const tipoAut = link.attr('tipoAutorizacion');

                    if (link.attr('loaded') === "0") {
                        cargarAutorizacionesPendientes(tipoAut);
                    }

                    if (link.attr('active') === "0") {
                        $(`a.loadsData`).attr('active', 0);
                        link.attr('active', 1);
                    } else if (link.attr('active') === "1") {
                        link.attr('active', 0);
                    }
                    alternarBotonAutorizar(+tipoAut);
                });

                $('span.checkAll').click(alternateCheck);

                botonAutorizar.hide();
                // botonGoTop.hide();

                // botonGoTop.click(() => $("html, body").animate({ scrollTop: 0 }, "slow"));

                botonAutorizar.click(autorizarCompras);

                initTablaOC();
                initTablaReq();
            }
        }

        function initTablaFacultamientosFacturas() {
            dtTablaFacultamientosFacturas = tablaFacultamientosFacturas.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                ordering: false,
                scrollY: '45vh',
                // scrollX: true,
                scrollCollapse: true,
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'proveedorDesc', title: 'Proveedor' },
                    { data: 'cfd_folio', title: 'CFDI Folio' },
                    { data: 'ccDesc', title: _empresaActual != 2 ? 'Centro Costo' : 'Área-Cuenta' },
                    { data: 'factura', title: 'Factura' },
                    { data: 'fechaString', title: 'Fecha' }
                ],
                columnDefs: [
                    { className: 'dt-center', targets: '_all' }
                ]
            });
        }

        modalFacultamientosFacturas.on('shown.bs.modal', function () {
            dtTablaFacultamientosFacturas.columns.adjust().draw();
        });

        function fnIniciar(e) {
            $.blockUI({ message: 'procesando...' });
            e.preventDefault();
            var _this = $(this);
            var _systemID = _this.data("sistemaid");
            var _url = _this.data("url");
            $.ajax({
                url: "/Base/setCurrentSystem",
                type: "POST",
                dataType: "json",
                data: { systemID: _systemID },
                success: function (result) {
                    if (result.esVirtual) {
                        var objExt = result.objExt;
                        var a = "http://" + window.location.hostname + _url + '&sistemaID=' + result.sistemaID;
                        window.location.href = a;
                    }
                    else {

                        if (result.externo == true) {
                            var objExt = result.objExt;
                            var a = "http://" + window.location.hostname + _url + '/Login/InitExtDto';
                            var b = "http://" + window.location.hostname + _url + '/Home/Principal';
                            $.ajax({
                                url: a,
                                data: {
                                    ObjUsuarioExt: objExt, remoto: _Remoto, empresa: _GEmpresaID, hostOrigin: "http://" + window.location.host
                                },
                                datatype: 'jsonp',
                                type: 'POST',
                                async: false,
                                xhrFields: {
                                    withCredentials: true,
                                },
                                success: function (data) {
                                    window.location.href = b;
                                }
                            });
                            window.location.href = b;
                        }
                        else {
                            if (result.necesitaIngresarDatosEnKontrol) {
                                window.location.href = result.formularioURL;
                            } else {
                                window.location.href = _url;
                            }
                        }
                    }
                }
            });
        }
        function fnExterno(_systemID, _url) {

            $.ajax({
                url: "/Base/setCurrentSystem",
                type: "POST",
                dataType: "json",
                data: { systemID: _systemID },
                success: function (result) {
                    if (result.externo == true) {
                        var objExt = result.objExt;
                        var a = "http://" + window.location.hostname + _url + '/Login/InitExtDto';
                        var b = "http://" + window.location.hostname + _url + '/Home/Index/?id=1';
                        $.ajax({
                            url: a,
                            data: {
                                ObjUsuarioExt: objExt, remoto: _Remoto, empresa: _GEmpresaID, hostOrigin: "http://" + window.location.host
                            },
                            datatype: 'jsonp',
                            type: 'POST',
                            async: false,
                            xhrFields: {
                                withCredentials: true,
                            },
                            success: function (data) {
                                window.location.href = b;
                            }
                        });
                        window.location.href = b;
                    }
                    else {
                        window.location.href = _url;
                    }
                }
            });
        }
        function modalSistemas() {
            $.ajax({
                url: '/Usuario/getSistemas',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: {},
                success: function (response) {
                    if (response.success === true) {

                        if (response.facultamientoFacturas != null) {
                            if (response.facultamientoFacturas.listaFacturasPendientes.length > 0) {
                                if (response.facultamientoFacturas.bloqueo) {
                                    labelModalFacultamientosFacturasTitulo.text('Bloqueo de Sistema');
                                    labelMensajeFacultamientosFacturas.text('ATENCIÓN: Se ha bloqueado el sistema debido a que tiene facturas pendientes por validar.');
                                } else {
                                    labelModalFacultamientosFacturasTitulo.text('Facturas Pendientes de Autorización');
                                    labelMensajeFacultamientosFacturas.text('ATENCIÓN: Tiene facturas pendientes por autorizar.');
                                }

                                AddRows(tablaFacultamientosFacturas, response.facultamientoFacturas.listaFacturasPendientes);
                                modalFacultamientosFacturas.modal('show');
                            }
                        }

                        if (response.sistemas.length == 1) {
                            fnExterno(response.sistemas[0].id, response.sistemas[0].url);
                            //window.location.href = "http://"+window.location.hostname + response.sistemas[0].url;
                        }
                        else {
                            var html = '';
                            $.each(response.sistemas, function (i, e) {
                                if (e.activo == true) {
                                    html += '<div class="col-xs-6 col-sm-4 col-md-2 col-lg-2 SubSystemsP">';
                                    html += ' <span><a href="#" data-url="' + e.url + '" data-SistemaID="' + e.id + '" class="SubSystemsS" role="button"><img src="' + e.icono + '"> <br />' + e.nombre + '</a></span>';
                                    html += '</div>';
                                }
                                else {
                                    html += '<div class="col-xs-6 col-sm-4 col-md-2">';
                                    html += ' <span class="btnMenudisabled disabled SubSystems"><img src="' + e.icono + '"> <br />' + e.nombre + '</span>';
                                    html += '</div>';
                                }
                            });
                            $("#divMenuAccesoPpal").html(html);
                        }

                    }
                    else {
                    }
                },
                error: function (response) {
                    AlertaGeneral("Error", response.message);
                }
            });
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }

        function cargarAutorizacionesPendientes(tipoAut) {
            switch (+tipoAut) {
                case tipoAutorizacion.Requisicion:
                    cargarCCsUsuario();
                    break;
                case tipoAutorizacion.OrdenCompra:
                    cargarOrdenesCompra(false);
                    break;
                case tipoAutorizacion.FormatoCambio:
                    // cargarOrdenesCompra();
                    botonAutorizar.hide(1000);
                    break;
                case tipoAutorizacion.AditivaDeductiva:
                    // cargarOrdenesCompra();
                    botonAutorizar.hide(1000);
                    break;
                case tipoAutorizacion.PlantillaPersonal:
                    // cargarOrdenesCompra();
                    botonAutorizar.hide(1000);
                    break;
                default:
                    break;
            }
            // botonGoTop.show(1000);
        }

        function initTablaOC() {
            dtTablaOC = tablaOC.DataTable({
                language: dtDicEsp,
                dom: '<t>',
                paging: false,
                columns: [
                    { data: 'cc', title: 'CC' },
                    { data: 'numero', title: '#' },
                    { data: 'total', title: 'Total', render: data => `${maskNumero(data)}` },
                    { data: 'id', title: 'Tipo', render: (data, type, row) => `${(row.voboPendiente && row.flagPuedeDarVobo) ? row.PERU_tipoCompra == 'RS' ? 'V' + '(servicio)' : 'V' : row.PERU_tipoCompra == 'RS' ? 'A' + '(servicio)' : 'A'}` },
                    { data: 'id', title: '', render: (data, type, row) => `<input type="checkbox">` }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaReq() {
            dtTablaReq = tablaReq.DataTable({
                language: dtDicEsp,
                dom: '<t>',
                paging: false,
                columns: [
                    { data: 'cc', title: 'CC' },
                    { data: 'numero', title: '#' },
                    { data: 'cantidadTotal', title: 'Total', render: data => `${maskNumero(data)}` },
                    { data: 'id', title: '', render: (data, type, row) => `<input type="checkbox">` }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarOrdenesCompra(autComplete) {
            const isAuth = false, cc = "", pendientes = false, propias = true;
            $.blockUI({ message: 'Cargando...' });
            $.get(`/Enkontrol/OrdenCompra/GetListaCompras`, { isAuth, cc, pendientes, propias })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        $(`a[tipoAutorizacion=${tipoAutorizacion.OrdenCompra}]`).attr('loaded', 1);

                        // Se filtran los registros
                        response.data = response.data.filter(x => {
                            if (x.voboPendiente && x.flagPuedeDarVobo) {
                                return true;
                            } else if (x.voboPendiente == false && x.flagPuedeAutorizar) {
                                return true;
                            } else {
                                return false;
                            }
                        });

                        dtTablaOC.clear().rows.add(response.data).draw();

                        if (autComplete) {
                            AlertaGeneral(`Éxito`, `Órdenes de Compra Autorizadas.`);
                        }
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

        let ccsUsuarios = null;

        function cargarCCsUsuario() {

            if (ccsUsuarios != null) {
                cargarRequisiciones(false);
                return;
            }

            $.blockUI({ message: 'Cargando...' });
            $.post('/Enkontrol/Requisicion/FillComboCcReq', { isAuth: false })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        ccsUsuarios = response.items.map(x => x.Value);
                        cargarRequisiciones(false);
                    }
                },
                    error => AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`));
        }

        function cargarRequisiciones(autComplete) {

            $.blockUI({ message: 'Cargando...' });

            $.post('/Enkontrol/Requisicion/getEncReq', { cc: ccsUsuarios, isAuth: false })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        $(`a[tipoAutorizacion=${tipoAutorizacion.Requisicion}]`).attr('loaded', 1);

                        dtTablaReq.clear().rows.add(response.lstReq).draw();

                        if (autComplete) {
                            AlertaGeneral(`Éxito`, `Requisiciones Autorizadas.`);
                        }
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

        function alternarBotonAutorizar(tipoAut) {
            switch (tipoAut) {
                case tipoAutorizacion.Requisicion:
                    botonAutorizar.show(1000);
                    break;
                case tipoAutorizacion.OrdenCompra:
                    botonAutorizar.show(1000);
                    break;
                case tipoAutorizacion.FormatoCambio:
                    botonAutorizar.hide(1000);
                    break;
                case tipoAutorizacion.AditivaDeductiva:
                    botonAutorizar.hide(1000);
                    break;
                case tipoAutorizacion.PlantillaPersonal:
                    botonAutorizar.hide(1000);
                    break;
                default:
                    botonAutorizar.hide(1000);
                    break;
            }
        }

        function alternateCheck(e) {
            const span = $(e.currentTarget);
            const tipoAut = span.parent().find('a').attr('tipoAutorizacion');
            const table = $(`table[tipoAutorizacion=${+tipoAut}]`);
            const checkboxArray = table.find(`input[type="checkbox"]`).toArray();
            const allChecked = checkboxArray.every(x => x.checked);
            checkboxArray.forEach(x => x.checked = allChecked == false);
        }

        function autorizarCompras() {
            const tipoAut = $('a.loadsData[active=1]').attr('tipoAutorizacion');
            switch (+tipoAut) {
                case tipoAutorizacion.Requisicion:
                    autorizarRequisiciones();
                    break;
                case tipoAutorizacion.OrdenCompra:
                    autorizarOrdenesCompras();
                    break;
            }
        }

        function autorizarOrdenesCompras() {

            const listaVobos = [];
            const listaAutorizados = [];

            tablaOC.find('tbody tr').toArray()
                .filter(x => $(x).find('input[type="checkbox"]').prop('checked'))
                .forEach(x => {

                    const rowData = dtTablaOC.row($(x)).data();

                    if (rowData.voboPendiente && rowData.flagPuedeDarVobo) {
                        listaVobos.push({
                            cc: rowData.cc,
                            numero: rowData.numero,
                            PERU_tipoCompra: rowData.PERU_tipoCompra ?? ""
                        });
                    } else {
                        listaAutorizados.push({
                            cc: rowData.cc,
                            numero: rowData.numero
                            ,
                            PERU_tipoCompra: rowData.PERU_tipoCompra ?? ""
                        });
                    }

                });

            if (listaVobos.length == 0 && listaAutorizados.length == 0) {
                AlertaGeneral(`Aviso`, `Debe seleccionar al menos un registro.`);
                return;
            }

            $.blockUI({ message: 'Autorizando...' });
            $.post('/Enkontrol/OrdenCompra/AutorizarCompras', { listaVobos, listaAutorizados })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        cargarOrdenesCompra(true);
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

        function autorizarRequisiciones() {

            let lst = [];

            lst = tablaReq.find('tbody tr').toArray()
                .filter(x => $(x).find('input[type="checkbox"]').prop('checked'))
                .map(x => {
                    const rowData = dtTablaReq.row($(x)).data();
                    return {
                        cc: rowData.cc,
                        numero: rowData.numero,
                        stAutoriza: rowData.isAuth,
                        PERU_tipoRequisicion: rowData.PERU_tipoRequisicion ?? ""
                    };
                });

            if (lst.length == 0) {
                AlertaGeneral(`Aviso`, `Debe seleccionar al menos un registro.`);
                return;
            }

            $.blockUI({ message: 'Autorizando...' });
            $.post('/Enkontrol/Requisicion/setAuth', { lst })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        cargarRequisiciones(true);
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

        init();
    };

    $(document).ready(function () {
        SISTEMA.SISTEMA.MenuPrincipalSistemas = new MenuPrincipalSistemas();
    });
})();

