(() => {
    $.namespace('Administrativo.FlujoEfectivo.PlaneacionDetalle');
    PlaneacionDetalle = function () {
        // Selectores.
        const divForm = $('.divForm');
        const divSelDetCC = $('#divSelDetCC');
        const selDetCC = $('#selDetCC');
        const btnDetLimpiar = $('#btnDetLimpiar');
        const txtDirInicioObra = $('#txtDirInicioObra');
        const tablaConceptos = $('#tablaConceptos');
        const lblCaptura = $('#lblCaptura');
        const inputDescripcion = $('#inputDescripcion');
        const inputMonto = $('#inputMonto');
        const tablaConceptoDetalle = $('#tablaConceptoDetalle');
        const botonAgregar = $('#botonAgregar');
        const comboCentroCostos = $('#comboCentroCostos');
        const divCapturaDetalle = $('#divCapturaDetalle');
        const mpDirSemana = $('#mpDirSemana');
        const labelCostosProyecto = $('#labelCostosProyecto');
        const labelGastosOperativos = $('#labelGastosOperativos');
        const labelEfectivoRecibido = $('#labelEfectivoRecibido');
        const selNomina = $('#selNomina');
        const divNomina = $('#divNomina');
        const modalContratos = $("#modalContratos");
        const btnAgregarContratos = $('#btnAgregarContratos');

        //Variables;
        let semanaInicial = null;
        let dtTablaConceptos;
        let dtTablaConceptoDetalles;
        let dtTablaGastosProv;
        let dtTablaGastosOperativos;
        let dtTablaEfectivoRecibido;
        let dtTablaProgramacion;

        const tablaProgramacion = $("#tablaProgramacion");


        let d = new Date();
        let detalleInfo = 0, idDetProyGemelo = 0;
        let centroCostos, startDate, endDate, conceptoID, noSemana, anio;
        const getCorte = new URL(window.location.origin + '/Administrativo/FlujoEfectivo/getCorte');
        const getCC = new URL(window.location.origin + '/Administrativo/FlujoEfectivo/getCboCCActivosSigoplan');
        const getCbotSoloPeriodoNomina = new URL(window.location.origin + '/Administrativo/Propuesta/getCbotSoloPeriodoNomina');
        //modal Costos Por proyecto
        const modalGastosProyecto = $('#modalGastosProyecto');
        const tablaGastosProv = $('#tablaGastosProv');
        const btnGuardarCostosProyecto = $('#btnGuardarCostosProyecto');

        //modal Costos Operativos.active
        const modalGastosOperativos = $('#modalGastosOperativos');
        const tablaGastosOperativos = $('#tablaGastosOperativos');
        const btnGuardarGastosOperativos = $('#btnGuardarGastosOperativos');

        //modal Efectivo Recibido
        const tablaEfectivoRecibido = $('#tablaEfectivoRecibido');
        const modalEfectivoRecibido = $('#modalEfectivoRecibido');
        const btnGuardarEfectivoRecibido = $('#btnGuardarEfectivoRecibido');

        //#region CodeInicial
        (function init() {
            // Lógica de inicialización.
            addListener();
            initTablaPropuesta();
            initTablaConceptos();
            initTablaConceptoDetalles();
            initTablaGastosProv();
            initTablaGastosOperativos();
            initTablaEfectivoRecibido();
            setMaxCorte();
            limpiarInfo();
        })();

        function addListener() {
            comboCentroCostos.fillCombo(getCC, null, false, "TODOS");
            comboCentroCostos.select2();
            selDetCC.fillCombo(getCC, null, false);
            selNomina.fillCombo(getCbotSoloPeriodoNomina, null, false, "Todos");
            convertToMultiselect('#selNomina');
            centroCostos = comboCentroCostos.val();
            let iniTodos = "ENERO / 2020";
            txtDirInicioObra.val(iniTodos);
            comboCentroCostos.change(setInicioObra);
            mpDirSemana.change(setInicioObra);
            selNomina.change(getCargaNominas);
            botonAgregar.click(guardarDetalle);
            btnGuardarCostosProyecto.click(setCostosProyecto);
            btnGuardarGastosOperativos.click(setGastosProyecto);
            btnGuardarEfectivoRecibido.click(setEfectivoRecibido);
            btnDetLimpiar.click(limpiarInfo);
            inputMonto.change(setMonedaMonto);
            inputMonto.focus(function () {
                $(this).select();
            });
            btnAgregarContratos.click(fnGuardarContratos);
        }

        function setDetForm(det) {
            detalleInfo = det.id;
            idDetProyGemelo = det.idDetProyGemelo;
            inputDescripcion.val(det.descripcion);
            inputMonto.val(maskNumero(det.monto));
            selDetCC.val(det.ccDetProyGemelo);
        }

        function setMonedaMonto() {
            let monto = unmaskNumero(inputMonto.val()),
                cpto = dtTablaConceptos.data().toArray().find(cpto => cpto.conceptoID == conceptoID);
            if (cpto.operador === "-" && monto < 0) {
                monto *= -1;
            }
            if (conceptoID == 17 && monto < 0) {
                monto *= -1;
            }
            inputMonto.val(maskNumero(monto));
        }

        function limpiarInfo() {
            detalleInfo = 0;
            idDetProyGemelo = 0;
            inputDescripcion.val('');
            inputMonto.val(maskNumero(0));
            selDetCC.val("");
        }

        function setInicioObra() {
            cargarConceptos(comboCentroCostos.val(), noSemana, anio);
            divCapturaDetalle.addClass('hide');
        }

        function selectCurrentWeek() {
            window.setTimeout(function () {
                mpDirSemana.find('.ui-datepicker-current-day a').addClass('ui-state-active');
            }, 1);
        }

        function setSemanaSelecionada() {
            date = mpDirSemana.datepicker('getDate');
            prevDom = date.getDate() - (date.getDay() + 14) % 7;
            startDate = new Date(date.getFullYear(), date.getMonth(), prevDom);
            endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 6);
            diaSemana = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 3);
            noSemana = diaSemana.noSemana();
            anio = endDate.getFullYear();
            mpDirSemana.val(`Semana ${noSemana} - ${startDate.toLocaleDateString()} - ${endDate.toLocaleDateString()}`);
            selectCurrentWeek();

            cargarConceptos(comboCentroCostos.val(), noSemana);
            divCapturaDetalle.addClass('hide');

            if (semanaInicial == null)
                semanaInicial = noSemana;
        }
        //#endregion

        //#region Fix de Tablas en modal.active
        modalEfectivoRecibido.on('shown.bs.modal', function () {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });
        modalGastosOperativos.on('shown.bs.modal', function () {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });
        modalGastosProyecto.on('shown.bs.modal', function () {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });
        modalContratos.on('shown.bs.modal', function () {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });
        //#endregion

        //#region inicializadores de tabla.
        function initTablaEfectivoRecibido() {
            dtTablaEfectivoRecibido = tablaEfectivoRecibido.DataTable({
                language: dtDicEsp,
                "scrollY": "400px",
                "scrollCollapse": true,
                destroy: true,
                paging: false,
                searching: false,
                order: [],
                columns: [
                    { data: 'numcte', title: '# Cliente' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'cc', title: 'Centro Costo' },
                    { data: 'factura', title: 'Factura' },
                    { data: 'fechafac', title: 'Fecha Factura' },
                    { data: 'total', title: 'Monto', createdCell: (td, data, rowdata) => $(td).addClass("text-right").html(maskNumero(data)) },
                    { data: 'id', render: (data, type, row) => `<input type="checkbox" class='form-control agregarMonto' checked >` }

                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaConceptos() {
            dtTablaConceptos = tablaConceptos.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                columns: [
                    { data: 'concepto', width: '40%', title: 'Concepto' },
                    {
                        data: 'total', title: 'Monto Total', createdCell: (td, data, rowData, row, col) => {
                            let botonera = renderButtons(rowData);
                            $(td).html(botonera);
                        }
                    },
                ],
                drawCallback: function (settings) {
                    tablaConceptos.find('.editar').click(function () {
                        limpiarInfo();
                        let data = dtTablaConceptos.row($(this).parents('tr')).data();
                        conceptoID = data.conceptoID;
                        let Concepto = data.concepto;
                        lblCaptura.html(`<i class="fas fa-table"></i> ${Concepto}`);
                        divCapturaDetalle.removeClass('hide');
                        cargarDetalles({
                            concepto: conceptoID,
                            cc: comboCentroCostos.val(),
                            semana: noSemana,
                            amio: anio
                        });
                    });

                    tablaConceptos.find('.cargar').click(function () {
                        let data = dtTablaConceptos.row($(this).parents('tr')).data();
                        switch (data.conceptoID) {
                            case 6:
                                labelCostosProyecto.text(data.concepto);
                                getCostosProyecto(startDate.toLocaleDateString(), endDate.toLocaleDateString(), comboCentroCostos.val(), noSemana, anio);
                                break;
                            case 7:
                                divNomina.addClass("hidden");
                                labelGastosOperativos.text(data.concepto);
                                getGastosOperativos(startDate.toLocaleDateString(), endDate.toLocaleDateString(), comboCentroCostos.val(), noSemana, anio);
                                break;
                            case 17:
                                labelEfectivoRecibido.text(data.concepto);
                                getEfectivoRecibido(startDate.toLocaleDateString(), endDate.toLocaleDateString(), comboCentroCostos.val(), noSemana, anio);
                                break;
                            case 13:
                                getProgramacionPagos();
                                break;
                            default:
                                break;
                        }
                    });

                    tablaConceptos.find('.cargarExtras').click(function () {
                        let data = dtTablaConceptos.row($(this).parents('tr')).data();
                        switch (data.conceptoID) {
                            case 7:
                                divNomina.removeClass("hidden");
                                getCargaNominas()
                                break;
                            case 6:
                                getCadenasProductivas(startDate.toLocaleDateString(), endDate.toLocaleDateString(), comboCentroCostos.val(), noSemana, anio)
                                break;
                            default:
                                break;
                        }
                    });
                }
            });
        }

        //Inicializador Tabla Costos Proyecto.
        function initTablaGastosProv() {
            dtTablaGastosProv = tablaGastosProv.DataTable({
                language: dtDicEsp,
                "scrollY": "400px",
                "scrollCollapse": true,
                destroy: true,
                paging: false,
                searching: false,
                order: [],
                columns: [
                    { data: 'numprov', title: 'Numero Proveedor' },
                    { data: 'descripcion', title: 'Concepto' },
                    { data: 'cc', title: 'Centro Costos' },
                    { data: 'factura', title: 'Factura' },
                    { data: 'fechaFactura', title: 'Fecha Factura' },
                    { data: 'monto', title: 'Monto', createdCell: (td, data, rowdata) => $(td).addClass("text-right").html(maskNumero(data)) },
                    {
                        data: 'id', render: (data, type, row) => `<input type="checkbox" class='form-control agregarMonto' checked >`
                    },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        //function initTablaConceptoDetalles() {
        //    dtTablaConceptoDetalles = tablaConceptoDetalle.DataTable({
        //        language: dtDicEsp,
        //        "scrollY": "400px",
        //        "scrollCollapse": true,
        //        destroy: true,
        //        paging: false,
        //        columns: [
        //            { data: 'descripcion', title: 'Concepto', width: '45%' },
        //            { data: 'monto', title: 'Monto', createdCell: (td, data, rowdata) => $(td).addClass("text-right").html(maskNumero(data)) },
        //            { data: 'factura', title: 'Factura' },
        //            { data: 'fechaFactura', title: 'Fecha', createdCell: (td, data, rowdata) => $(td).html(data) },
        //            { data: 'ccDetProyGemelo', title: 'CC', createdCell: (td, data, rowData, row, col) => $(td).html(data === "N/A" ? rowData.cc : data) },
        //            { data: 'id', width: '2%', render: (data, type, row) => setBotonTablaConceptos(row) }
        //        ],
        //        createdRow: (tr, data) => $(tr).addClass(data.tipo == 1 ? "detClick" : ""),
        //        initComplete: function (settings, json) {
        //            tablaConceptoDetalle.on('click', '.detClick', function (event) {
        //                let row = $(this).closest("tr");
        //                data = dtTablaConceptoDetalles.row(row).data();
        //                tablaConceptoDetalle.find("tbody tr").removeClass("selected");
        //                row.addClass("selected");
        //                setDetForm(data);
        //            });
        //            tablaConceptoDetalle.find('.quitar').click(function () {
        //                let row = $(this).closest("tr");
        //                let data = dtTablaConceptoDetalles.row(row).data();
        //                let id = $(this).attr('data-id');
        //                let nuevo = [{
        //                    id: id,
        //                    idDetProyGemelo: data.idDetProyGemelo,
        //                    concepto: data.concepto,
        //                    descripcion: data.descripcion,
        //                    monto: data.monto,
        //                    cc: data.cc,
        //                    semana: data.semana,
        //                    año: data.año,
        //                    estatus: false,
        //                    fechaCaptura: data.fechaCaptura,
        //                    usuarioCaptura: data.usuarioCaptura
        //                }];
        //                if ($(this).attr('data-editable')) {
        //                    saveOrUpdateDetalle(nuevo, true, row[0]._DT_RowIndex);
        //                }
        //                else {
        //                    NotificacionGeneral('Alerta', 'Este valor no puede ser modificado porque ya paso su fecha de corte.');
        //                }
        //            });
        //        },

        //    });
        //}


        function initTablaConceptoDetalles() {
            dtTablaConceptoDetalles = tablaConceptoDetalle.DataTable({
                language: dtDicEsp,
                "scrollY": "400px",
                "scrollCollapse": true,
                destroy: true,
                paging: false,
                columns: [
                    { data: 'descripcion', title: 'Concepto', width: '45%' },
                    { data: 'monto', title: 'Monto', createdCell: (td, data, rowdata) => $(td).addClass("text-right").html(maskNumero(data)) },
                    { data: 'factura', title: 'Factura' },
                    { data: 'fechaFactura', title: 'Fecha', createdCell: (td, data, rowdata) => $(td).html(data) },
                    { data: 'ccDetProyGemelo', title: 'CC', createdCell: (td, data, rowData, row, col) => $(td).html(data === "N/A" ? rowData.cc : data) },
                    { data: 'id', width: '2%', render: (data, type, row) => setBotonTablaConceptos(row) }
                ],
                createdRow: (tr, data) => $(tr).addClass(data.tipo == 1 ? "detClick" : ""),
                initComplete: function (settings, json) {
                    tablaConceptoDetalle.on('click', '.detClick', function (event) {
                        let row = $(this).closest("tr");
                        data = dtTablaConceptoDetalles.row(row).data();
                        tablaConceptoDetalle.find("tbody tr").removeClass("selected");
                        row.addClass("selected");
                        setDetForm(data);
                    });
                    tablaConceptoDetalle.on('click', '.quitar', function (event) {
                        let row = $(this).closest("tr");
                        let data = dtTablaConceptoDetalles.row(row).data();
                        let id = $(this).attr('data-id');
                        let nuevo = [{
                            id: id,
                            idDetProyGemelo: data.idDetProyGemelo,
                            concepto: data.concepto,
                            descripcion: data.descripcion,
                            monto: data.monto,
                            cc: data.cc,
                            semana: data.semana,
                            año: data.año,
                            estatus: false,
                            fechaCaptura: data.fechaCaptura,
                            usuarioCaptura: data.usuarioCaptura
                        }];
                        if ($(this).attr('data-editable')) {
                            saveOrUpdateDetalle(nuevo, true, row[0]._DT_RowIndex);
                        }
                        else {
                            NotificacionGeneral('Alerta', 'Este valor no puede ser modificado porque ya paso su fecha de corte.');
                        }
                    });
                },

            });
        }

        function initTablaGastosOperativos() {
            dtTablaGastosOperativos = tablaGastosOperativos.DataTable({
                language: dtDicEsp,
                "scrollY": "400px",
                "scrollCollapse": true,
                destroy: true,
                paging: false,
                searching: false,
                order: [],
                columns: [
                    { data: 'numprov', title: '# Proveedor', createdCell: (td, data, rowdata) => data == 0 ? $(td).html('----') : $(td).html(data) },
                    { data: 'descripcion', title: 'Concepto' },
                    { data: 'cc', title: 'Centro Costos' },
                    { data: 'factura', title: 'Factura', createdCell: (td, data, rowdata) => data == null ? $(td).html('----') : $(td).html(data) },
                    { data: 'fechaFactura', title: 'Fecha Factura', createdCell: (td, data, rowdata) => rowdata.factura == null ? $(td).html('----') : $(td).html(data) },
                    { data: 'monto', title: 'Monto', createdCell: (td, data, rowdata) => $(td).addClass("text-right").html(maskNumero(data)) },
                    {
                        data: 'id', render: (data, type, row) => `<input type="checkbox" class='form-control agregarMonto' checked >`
                    },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function renderButtons(row) {
            let opcion = row.conceptoID;
            let lst = [],
                div = $("<span>", {
                    class: "input-group-btn"
                });
            lstOpciones = [13];

            if(lstOpciones.indexOf(opcion) < 0) {
                lst.push($(`<button>`, {
                    type: "button",
                    class: "btn btn-xs btn-success editar",
                    text: maskNumero(row.total),
                }));
            }
            else{
                lst.push($(`<button>`, {
                    type: "button",
                    class: "btn btn-xs btn-success disabled",
                    text: maskNumero(row.total),
                }));
            }
            switch (opcion) {
                case 6: //Costos de Proyecto
                    {
                        let btnGastosProveedor = $(`<button>`, {
                            type: "button",
                            class: "btn btn-xs btn-primary cargar",
                            text: "Gastos Proyecto"
                        });
                        btnGastosProveedor.data().conceptoID = conceptoID;
                        lst.push(btnGastosProveedor);
                        let btnCadenaProductiva = $(`<button>`, {
                            type: "button",
                            class: "btn btn-xs btn-primary cargarExtras",
                            text: "Cadenas Productivas"
                        });
                        btnCadenaProductiva.data().conceptoID = conceptoID;
                        lst.push(btnCadenaProductiva);
                        break;
                    }
                case 7: // Gastos Operativos
                    {
                        let btnGastosOperativos = $(`<button>`, {
                            type: "button",
                            class: "btn btn-xs btn-primary cargar",
                            text: "Gastos Operativos"
                        });
                        btnGastosOperativos.data().conceptoID = conceptoID;
                        lst.push(btnGastosOperativos);
                        let btnCargarNomina = $(`<button>`, {
                            type: "button",
                            class: "btn btn-xs btn-primary cargarExtras",
                            text: "Cargar Nomina"
                        });
                        btnCargarNomina.data().conceptoID = conceptoID;
                        lst.push(btnCargarNomina);
                        break;
                    }
                case 17: // Efectivo Recibido
                    {
                        let btnEfectivoRecibido = $(`<button>`, {
                            type: "button",
                            class: "btn btn-xs btn-primary cargar",
                            text: "Efectivo Recibido"
                        });
                        btnEfectivoRecibido.data().conceptoID = conceptoID;
                        lst.push(btnEfectivoRecibido);
                        break;
                    }
                case 13: {
                    let btnContratos = $(`<button>`, {
                        type: "button",
                        class: `btn btn-xs btn-primary cargar `,
                        text: "Contratos"
                    });
                    btnContratos.data().conceptoID = conceptoID;
                    lst.push(btnContratos);
                    break;
                }
                default: break;
            }
            lst.forEach(btn => div.append(btn));
            return div;
        }

        function setBotonTablaConceptos(row) {
            return `<button class="btn btn-xs btn-danger quitar" data-id="${row.id}" data-editable='${row.editable}' >
                        <i class="fas fa-times"></i>
                    </button>`
        }
        function setBtnForm() {
            dtTablaConceptoDetalles.column(4).visible(true);
            dtTablaConceptoDetalles.column(5).visible(true);
            dtTablaEfectivoRecibido.column(6).visible(true);
            dtTablaGastosOperativos.column(6).visible(true);
            dtTablaGastosProv.column(6).visible(true);
            divSelDetCC.removeClass("hidden");
            botonAgregar.removeClass("hidden");
            btnDetLimpiar.removeClass("hidden");
            btnGuardarCostosProyecto.removeClass("hidden");
            btnGuardarEfectivoRecibido.removeClass("hidden");
            btnGuardarGastosOperativos.removeClass("hidden");
            divForm.removeClass("hidden");
            if (centroCostos !== "TODOS") {
                dtTablaConceptoDetalles.column(4).visible(false);
                dtTablaConceptoDetalles.column(5).visible(false);
                dtTablaEfectivoRecibido.column(6).visible(false);
                dtTablaGastosOperativos.column(6).visible(false);
                dtTablaGastosProv.column(6).visible(false);
                divSelDetCC.addClass("hidden");
                botonAgregar.addClass("hidden");
                btnDetLimpiar.addClass("hidden");
                btnGuardarCostosProyecto.addClass("hidden");
                btnGuardarEfectivoRecibido.addClass("hidden");
                btnGuardarGastosOperativos.addClass("hidden");
                divForm.addClass("hidden");
            }
        }
        //#endregion

        //#region Llenar Informacion de tablas.
        function cargarConceptos(cc, noSemana) {
            try {
                $.get('/Administrativo/FlujoEfectivo/ObtenerInfoConceptos/', { centroCostos: cc, semana: noSemana, anio: anio })
                    .then(response => {
                        if (response.success) {
                            if (dtTablaConceptos != null) {
                                dtTablaConceptos.clear().draw();
                                dtTablaConceptos.rows.add(response.listaConceptos).draw();
                                centroCostos = cc;
                                setBtnForm();
                            }
                        }
                        else
                            // Operación no completada.
                            NotificacionGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    },
                        error => {
                            NotificacionGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        });
            } catch (e) { NotificacionGeneral(`Operación fallida`, e.message) }
        }

        function getEfectivoRecibido(fechaInicio, fechaFin, cc, semana) {
            try {
                $.get('/Administrativo/FlujoEfectivo/getEfectivoRecibido/', { fechaInicio, fechaFin, cc, semana, anio })
                    .then(response => {
                        if (response.success) {
                            if (dtTablaEfectivoRecibido != null) {
                                dtTablaEfectivoRecibido.clear().draw();
                                dtTablaEfectivoRecibido.rows.add(response.listaPlaneacion).draw();
                                modalEfectivoRecibido.modal('show');
                            }
                        }
                        else
                            // Operación no completada.
                            NotificacionGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    },
                        error => {
                            NotificacionGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        });
            } catch (e) { NotificacionGeneral(`Operación fallida`, e.message) }
        }

        function getGastosOperativos(fechaInicio, fechaFin, cc, semana) {
            try {
                $.get('/Administrativo/FlujoEfectivo/getGastosOperativos/', { fechaInicio, fechaFin, cc, semana, anio })
                    .then(response => {
                        if (response.success) {
                            if (dtTablaGastosOperativos != null) {
                                dtTablaGastosOperativos.clear().draw();
                                dtTablaGastosOperativos.rows.add(response.listaPlaneacion).draw();
                                modalGastosOperativos.modal('show');
                            }
                        }
                        else
                            // Operación no completada.
                            NotificacionGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    },
                        error => {
                            NotificacionGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        });
            } catch (e) { NotificacionGeneral(`Operación fallida`, e.message) }
        }

        function getCostosProyecto(fechaInicio, fechaFin, cc, semana) {
            $.get('/Administrativo/FlujoEfectivo/getGastosProv/', { fechaInicio, fechaFin, cc, semana, anio })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        if (dtTablaGastosProv != null) {
                            dtTablaGastosProv.clear().draw();
                            dtTablaGastosProv.rows.add(response.listaPlaneacion).draw();
                            modalGastosProyecto.modal('show');
                        }
                        else {
                            dtTablaGastosProv.clear().draw();
                            NotificacionGeneral('Alerta', 'No se encontró información con los valores seleccionados.');
                        }
                    } else {
                        // Operación no completada.
                        NotificacionGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    NotificacionGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }
        function cargarDetalles({ concepto, cc, semana }) {
            try {
                $.get('/Administrativo/FlujoEfectivo/getDetallePlaneacion/', { concepto, cc, semana, anio, tipo: 0 })
                    .then(response => {
                        if (response.success) {
                            if (dtTablaConceptoDetalles != null) {
                                dtTablaConceptoDetalles.clear().draw();
                                dtTablaConceptoDetalles.rows.add(response.listaConceptosDetalle).draw();
                                centroCostos = cc;
                            }
                        }
                        else {
                            // Operación no completada.
                            NotificacionGeneral(`Operación fallida`, `No se pudo completar la operación ${response.message}`);
                        }
                    },
                        error => {
                            NotificacionGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        });
            } catch (e) { NotificacionGeneral(`Operación fallida`, e.message) }
        }
        async function setMaxCorte() {
            try {
                response = await ejectFetchJson(getCorte);
                if (response.success) {
                    let arrUltimaFecha = $.toDate(response.maxCorte).split('/')
                        , ultimaFecha = new Date(arrUltimaFecha[2], +arrUltimaFecha[1] - 1, +arrUltimaFecha[0] + 14);
                    mpDirSemana.datepicker({
                        firstDay: 0,
                        showOtherMonths: true,
                        selectOtherMonths: true,
                        onSelect: function (dateText, inst) {
                            setSemanaSelecionada();
                        },
                        beforeShowDay: function (date) {
                            var cssClass = '';
                            if (date >= startDate && date <= endDate)
                                cssClass = 'ui-datepicker-current-day';
                            return [true, cssClass];
                        },
                        onChangeMonthYear: function (year, month, inst) {
                            selectCurrentWeek();
                        }
                    }).datepicker("setDate", ultimaFecha);
                    setSemanaSelecionada();
                }
            } catch (o_O) { AlertaGeneral('Aviso', o_O.message) }
        }

        function getCargaNominas() {
            modalGastosOperativos.modal('show');
            let periodos = selNomina.val();
            if (periodos.length > 0) {
                let lstPeriodo = periodos.map(option => {
                    let valores = option.split('-');
                    return {
                        fecha_inicial: valores[0],
                        fecha_final: valores[1],
                        tipo_nomina: valores[2]
                    }
                });
                $.post('/Administrativo/FlujoEfectivo/getCargaNomina/', {
                    lstPeriodo,
                    cc: comboCentroCostos.val(),
                    semana: noSemana,
                    anio: anio
                }).then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        if (dtTablaGastosOperativos != null) {
                            dtTablaGastosOperativos.clear().draw();
                            dtTablaGastosOperativos.rows.add(response.listaPlaneacion).draw();
                        }
                    } else {
                        // Operación no completada.
                        NotificacionGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    NotificacionGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
            }
        }

        function getCadenasProductivas(fechaInicio, fechaFin, cc, semana) {
            $.get('/Administrativo/FlujoEfectivo/getCadenasProductivas/', { fechaInicio, fechaFin, cc, semana, anio })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        if (dtTablaGastosProv != null) {
                            dtTablaGastosProv.clear().draw();
                            dtTablaGastosProv.rows.add(response.listaPlaneacion).draw();
                            modalGastosProyecto.modal('show');
                        }
                    } else {
                        // Operación no completada.
                        NotificacionGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    NotificacionGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }
        //#endregion

        //#region  Guardado Información
        function guardarDetalle() {
            if (selDetCC.val() == '') {
                NotificacionGeneral(`Operación fallida`, `Debe seleccionar un CC`);
            }
            else {
                let nuevo = getInfoDetalle();
                saveOrUpdateDetalle(nuevo, false);
            }

        }

        function getInfoDetalle() {
            let monto = unmaskNumero(inputMonto.val()),
                cpto = dtTablaConceptos.data().toArray().find(cpto => cpto.conceptoID == conceptoID),
                cc = selDetCC.val(),
                ccDet = selDetCC.val();
            if (cpto.operador === "-" && monto > 0) {
                monto *= -1;
            }
            let lst = [{
                id: detalleInfo,
                concepto: conceptoID,
                descripcion: inputDescripcion.val(),
                monto: monto,
                cc: comboCentroCostos.val(),
                semana: noSemana,
                estatus: true,
                fechaCaptura: `${d.getDate()}/${d.getMonth()}/${d.getFullYear()}`,
                año: anio,
                usuarioCaptura: 0,
                numcte: 0,
                numprov: 0,
                fechaFactura: "",
                idDetProyGemelo: idDetProyGemelo,
                ccDetProyGemelo: ccDet
            }];
            if (centroCostos === "TODOS" && cc !== "") {
                lst.push({
                    id: idDetProyGemelo,
                    concepto: conceptoID,
                    descripcion: inputDescripcion.val(),
                    monto: monto,
                    cc: cc,
                    semana: noSemana,
                    estatus: true,
                    fechaCaptura: `${d.getDate()}/${d.getMonth()}/${d.getFullYear()}`,
                    año: anio,
                    usuarioCaptura: 0,
                    numcte: 0,
                    numprov: 0,
                    fechaFactura: "",
                    idDetProyGemelo: detalleInfo,
                });
            }
            return lst;
        }
        function saveOrUpdateDetalle(nuevo, esEliminado, index) {
            axios.post("saveOrUpdateDetalle", { nuevo }).then(response => {
                if (response.data.success) {
                    // Operación exitosa.
                    if (esEliminado) {
                        dtTablaConceptoDetalles.row(index).remove().draw();
                        NotificacionGeneral('Operacion Exitosa', 'El registro se eliminó correctamente');
                    } else {
                        let nuevo = response.data.nuevo[0];
                        // setManualDetPlaneacion(nuevo);
                        cargarDetalles(nuevo);
                        NotificacionGeneral('Operacion Exitosa', 'El registro se actualizo correctamente');
                    }
                    limpiarInfo();
                    cargarConceptos(comboCentroCostos.val(), noSemana, anio);
                }
                else
                {
                    // Operación no completada.
                    NotificacionGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.data.message}`);
                }
            }).catch(error => Alert2Error(error.message));           
        }
        //async function saveOrUpdateDetalle(nuevo, esEliminado, index) {
        //    try {
        //        response = await ejectFetchJson('/Administrativo/FlujoEfectivo/saveOrUpdateDetalle/', { nuevo });
        //        if (response.success) {
        //            // Operación exitosa.
        //            if (esEliminado) {
        //                dtTablaConceptoDetalles.row(index).remove().draw();
        //                NotificacionGeneral('Operacion Exitosa', 'El registro se eliminó correctamente');
        //            } else {
        //                let nuevo = response.nuevo[0];
        //                // setManualDetPlaneacion(nuevo);
        //                cargarDetalles(nuevo);
        //                NotificacionGeneral('Operacion Exitosa', 'El registro se actualizo correctamente');
        //            }
        //            limpiarInfo();
        //            cargarConceptos(comboCentroCostos.val(), noSemana, anio);
        //        }
        //        else
        //        {
        //            // Operación no completada.
        //            NotificacionGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
        //        }
        //    } catch (o_O) {
        //        // Error al lanzar la petición.
        //        NotificacionGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${o_O.status} - ${o_O.statusText}.`);
        //    }
        //}
        function setManualDetPlaneacion(detalle) {
            let lstDet = dtTablaConceptoDetalles.data().toArray(),
                data = lstDet.find(det => detalle.id === det.id);
            if (data !== null) {
                let index = dtTablaConceptoDetalles.row(data).node().rowIndex;
                dtTablaConceptoDetalles.row(index).remove().draw();
            }
            dtTablaConceptoDetalles.row.add(detalle).draw();

            // let lstDet = dtTablaConceptoDetalles.data().toArray(),
            //     data = lstDet.find(det => detalle.id === det.id);
            // if (data !== null) {
            //     let row = $(dtTablaConceptoDetalles.row(data).node());
            //     row.find('td:eq(0)').text(detalle.descripcion === null ? '' : detalle.descripcion);
            //     row.find('td:eq(1)').text(maskNumero(detalle.monto));
            //     row.find('td:eq(2)').text(detalle.factura === null ? '' : detalle.factura);
            //     row.find('td:eq(3)').text(detalle.fechaFactura === null ? '' : detalle.fechaFactura);
            //     row.find('td:eq(4)').text(detalle.ccDetProyGemelo === "N/A" ? detalle.cc : detalle.ccDetProyGemelo);
            //     data.monto = detalle.monto;
            //     data.descripcion = detalle.descripcion;
            //     data.factura = detalle.factura;
            //     data.fechaFactura = detalle.fechaFactura;
            //     data.ccDetProyGemelo = detalle.ccDetProyGemelo;
            //     data.idDetProyGemelo = detalle.idDetProyGemelo;
            // } else {
            //     dtTablaConceptoDetalles.row.add(detalle).draw();
            // }
        }
        function setGastosProyecto() {
            modalGastosOperativos.modal('hide');
            idConceptoDir = 7;
            let arrayDtTable = getArrayInfo(tablaGastosOperativos, dtTablaGastosOperativos);
            guardarCostosProyecto(arrayDtTable, idConceptoDir);
        }

        function getArrayEfectivoRecibido(SelectorTabla, dtTabla, idConceptoDir) {
            const datosCaptura = SelectorTabla.find('tbody tr[role="row"]').toArray()
                .map(row => {
                    let $row = $(row);
                    const esCapturaEspecial = $row.find('input[type="checkbox"].agregarMonto')[0].checked;
                    if (esCapturaEspecial) {
                        let captura = dtTabla.row(row).data();
                        let objData = {
                            id: 0,
                            concepto: idConceptoDir,
                            descripcion: captura.descripcion,
                            monto: captura.total,
                            semana: noSemana,
                            año: anio,
                            cc: captura.cc,
                            estatus: true,
                            fechaCaptura: `${d.getDate()}/${d.getMonth()}/${d.getFullYear()}`,
                            usuarioCaptura: 0,
                            sp_gastos_provID: 0,
                            factura: captura.factura,
                            numcte: captura.numcte,
                            fechaFactura: captura.fechafac
                        };
                        return objData;
                    }
                }).filter(x => x != undefined);
            return datosCaptura;
        }

        function getArrayInfo(SelectorTabla, dtTabla) {
            const datosCaptura = SelectorTabla.find('tbody tr[role="row"]').toArray()
                .map(row => {
                    let $row = $(row);
                    const esCapturaEspecial = $row.find('input[type="checkbox"].agregarMonto')[0].checked;
                    if (esCapturaEspecial) {
                        let captura = dtTabla.row(row).data();
                        return captura;
                    }
                }).filter(x => x != undefined);
            return datosCaptura;
        }

        function setCostosProyecto() {
            modalGastosProyecto.modal('hide');
            let arrayDtTable = getArrayInfo(tablaGastosProv, dtTablaGastosProv);
            idConceptoDir = 6;
            guardarCostosProyecto(arrayDtTable, idConceptoDir);
        }

        function setEfectivoRecibido() {
            modalEfectivoRecibido.modal('hide');
            idConceptoDir = 17;
            let arrayDtTable = getArrayEfectivoRecibido(tablaEfectivoRecibido, dtTablaEfectivoRecibido, idConceptoDir);
            guardarCostosProyecto(arrayDtTable, idConceptoDir);
        }

        function guardarCostosProyecto(arrayDtTable, idConceptoDir) {
            semana = noSemana;
            cc = comboCentroCostos.val();
            $.post('/Administrativo/FlujoEfectivo/saveDetallesMasivos', { lista: arrayDtTable, idConceptoDir: idConceptoDir, anio: anio, semana: semana, cc })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        NotificacionGeneral('Operacion Exitosa', 'El registro se actualizo correctamente');
                        cargarDetalles(response);
                        limpiarInfo();
                        cargarConceptos(comboCentroCostos.val(), noSemana, anio);
                    } else {
                        // Operación no completada.
                        NotificacionGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    NotificacionGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
        }
        //#endregion


        function getProgramacionPagos() {

            $.get('/Contratos/LoadContratosProgramadosCplan/', {
                pInicio: startDate.toLocaleDateString(),
                pFinal: endDate.toLocaleDateString(),
                empresa: 2,
                cc: comboCentroCostos.val()
            }).then(response => {
                if (response.success) {
                    // Operación exitosa.
                    dtTablaProgramacion.clear().draw();
                    dtTablaProgramacion.rows.add(response.contratoLista).draw();

                } else {
                    // Operación no completada.
                    AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                }
            }, error => {
                // Error al lanzar la petición.
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            });
            modalContratos.modal('show');
        }

        function fnGuardarContratos() {
            modalContratos.modal('hide');
            idConceptoDir = 13;
            
            let arrayDtTable = [];
            let arrayProgramacionID = [];
            $.each($(".agregarMonto:checked"),function(i,e){
                let contratos = dtTablaProgramacion.row($(e).closest('tr')).data();
                arrayProgramacionID.push({ id: contratos.id });
                let objData = {
                    id: 0,
                    concepto: idConceptoDir,
                    descripcion: `${contratos.noEconomico} PAR ${contratos.mensualidad}`,
                    monto: contratos.total * (-1),
                    semana: noSemana,
                    año: anio,
                    cc: contratos.cc,
                    ac: contratos.ac,
                    estatus: true,
                    fechaCaptura: `${d.getDate()}/${d.getMonth()}/${d.getFullYear()}`,
                    usuarioCaptura: 0,
                    sp_gastos_provID: 0,
                    numcte: 0,
                    categoriaTipo: 6
                };
                    arrayDtTable.push(objData);
            });
            //let arrayDtTable = dtTablaProgramacion.data().toArray().filter(r => r.programado == true).map(element => {
            //    let objData = {
            //        id: 0,
            //        concepto: idConceptoDir,
            //        descripcion: `${element.noEconomico} PAR ${element.mensualidad}`,
            //        monto: element.total * (-1),
            //        semana: noSemana,
            //        año: anio,
            //        cc: element.cc,
            //        ac: element.ac,
            //        estatus: true,
            //        fechaCaptura: `${d.getDate()}/${d.getMonth()}/${d.getFullYear()}`,
            //        usuarioCaptura: 0,
            //        sp_gastos_provID: 0,
            //        numcte: 0,
            //        categoriaTipo: 6
            //    };
            //    return objData;
            //});

            //let arrayProgramacionID = dtTablaProgramacion.data().toArray().filter(r => r.programado == true).map(element => {
            //    let objLista = { id: element.id };
            //    return objLista;
            //});

            acutalizarContratos(arrayProgramacionID, arrayDtTable, idConceptoDir)
            //  guardarCostosProyecto(arrayDtTable, idConceptoDir);
        }

        //tablaProgramacion.on('change', '.agregarMonto', function () {
        //    let contratos = dtTablaProgramacion.row($(this).closest('tr')).data();
        //    contratos.programado = $(this).prop('checked');
        //    let contratoDTO = dtTablaProgramacion.data().toArray();
        //    dtTablaProgramacion.clear();
        //    dtTablaProgramacion.rows.add(contratoDTO).draw();
        //});

        function initTablaPropuesta() {

            dtTablaProgramacion = tablaProgramacion.DataTable({
                paging: false
                , destroy: true
                , ordering: false
                , language: dtDicEsp
                , scrollY: '52vh',
                scrollCollapse: true,
                scrollX: true
                , "bLengthChange": false
                , "searching": false
                , "bFilter": true
                , "bInfo": true
                , columns: [
                    { data: 'noEconomico', title: 'No Eco' },
                    { data: 'mensualidad', title: 'Par', width: "25px" },
                    { data: 'financiamiento', title: 'Financiera' },
                    {
                        data: 'fechaVencimiento', title: 'Vencimiento',
                        render: (data, type, row) => {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    },
                    { data: 'contrato', title: 'Contrato' },
                    {
                        data: 'capital', title: 'Capital', width: "65px",
                        render: (data, type, row) => {
                            let disabled = 'disabled';
                            return `<input type='text' class='form-control inputCapital inputFontS input-sm' value='${maskNumero(data)}' ${disabled}/>`;
                        }
                    },
                    {
                        data: 'ivaInteres', title: 'iva Int.s', width: "65px",
                        render: (data, type, row) => {
                            let disabled = 'disabled';
                            return `<input type='text' class='form-control inputIvaIntereses inputFontS input-sm' value='${maskNumero(data)}' ${disabled}/>`;
                        }
                    },
                    {
                        data: 'intereses', title: 'Intereses', width: "65px",
                        render: (data, type, row) => {
                            let disabled = 'disabled';
                            return `<input type='text' class='form-control inputIntereses inputFontS input-sm' value='${maskNumero(data)}' ${disabled}/>`;
                        }
                    },
                    {
                        data: 'iva', title: 'IVA', width: "65px",
                        render: (data, type, row) => {
                            let disabled = 'disabled';
                            return `<input type='text' class='form-control inputIVA inputFontS input-sm' value='${maskNumero(data)}' ${disabled}/>`;
                        }
                    },
                    {
                        data: 'importe', title: 'Importe',
                        render: (data, type, row) => {
                            if (data != 0)
                                return maskNumero(data);
                            else
                                return '';
                        }
                    },
                    {
                        data: 'importeDLLS', title: 'Imp. DLLS',
                        render: (data, type, row) => {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: 'tipoCambio', title: 'T.C.',
                        render: (data, type, row) => {
                            return maskNumero(data);
                        }
                    },
                    {
                        data: 'total', title: 'Total',
                        render: (data, type, row) => {
                            return maskNumero((row.total));
                        }
                    }, {
                        data: 'programado', title: 'Programar', render: (data, type, row) => {
                            let checked = data ? 'checked' : '';
                            return `<input type='checkbox' class='form-control agregarMonto' ${checked}/>`;
                        }
                    }
                ],
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;
                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    // Total over this page
                    totalDLLS = api.data().toArray().reduce((acc, c) => acc + c.importeDLLS, 0);

                    // Update footer
                    $(api.column(10).footer()).html(
                        maskNumero(totalDLLS)
                    );

                    total = api.data().toArray().reduce((acc, c) => acc + c.total, 0);

                    // Update footer
                    $(api.column(12).footer()).html(
                        maskNumero(total)
                    );
                }
            });
        }


        function acutalizarContratos(arrayProgramacionID, arrayDtTable, idConceptoDir) {
            $.post('/Contratos/ActualizarContratos/', { arrayProgramacionID: arrayProgramacionID })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        getProgramacionPagos();
                        setInicioObra();
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                ).then(guardarCostosProyecto(arrayDtTable, idConceptoDir));
        }
    }
    $(() => Administrativo.FlujoEfectivo.PlaneacionDetalle = new PlaneacionDetalle())
        .ajaxStart(() => $.blockUI({
            message: 'Procesando...',
            baseZ: 9999
        }))
        .ajaxStop($.unblockUI);
})();