(() => {
    $.namespace('Administrativo.Contabilidad.Propuesta.ValidacionFacturas');
    ValidacionFacturas = function () {
        const cboCC = $("#cboCC");
        const btnBuscar = $("#btnBuscar");
        const btnGuardar = $("#btnGuardar");
        const txtProvMin = $('#txtProvMin');
        const txtProvMax = $('#txtProvMax');
        const txtPropFOC = $('#txtPropFOC');
        const txtPropHSel = $('#txtPropHSel');
        const txtPropHSelDll = $('#txtPropHSelDll');
        const txtPropFTmb = $('#txtPropFTmb');
        const txtPropFFac = $('#txtPropFFac');
        const txtPropFPag = $('#txtPropFPag');
        const txtPropFMax = $('#txtPropFMax');
        const txtPropFTmp = $('#txtPropFTmp');
        const txtPropFSol = $('#txtPropFSol');
        const txtPropFRec = $('#txtPropFRec');
        const btnProvLimit = $('#btnProvLimit');
        const txtFechaPago = $("#txtFechaPago");
        const txtFechaCorte = $("#txtFechaCorte");
        const txtPropHTotal = $('#txtPropHTotal');
        const txtPropHTotalDll = $('#txtPropHTotalDll');
        const inputGroupBtn = $('.input-group-btn');
        const tblData = $("#tblData");
        const btnTodo = $('#btnTodo');
        const btnFiltroValidar = $('#btnFiltroValidar');
        const btnFiltroRechazar = $('#btnFiltroRechazar');
        const btnModalAutorizar = $('#btnModalAutorizar');

        const inputTipo = $("#inputTipo");

        let dtTblData;

        const report = $('#report');
        menuConfig = {
            lstOptions: [
                { text: '<i class="fa fa-download"></i> Descargar', action: "descargar", fn: parametros => { reporteDocumento(parametros.tipo, parametros.cc, parametros.oc, parametros.url, true) } },
                { text: '<i class="fa fa-file"></i> Visualizar', action: "visor", fn: parametros => { reporteDocumento(parametros.tipo, parametros.cc, parametros.oc, parametros.url, false) } },
            ]
        }

        let init = () => {

            IniciarForm();
            txtFechaPago.change(setDolarDelDia)
            btnBuscar.click(CargarTabla);
            btnProvLimit.click(setDataLimit);
            btnGuardar.click(saveSplitted);
            btnTodo.click(seleccionarTodo);
            inputGroupBtn.click(chngSetAllSelOpt);
            inputGroupBtn.each((i, btn) => $(btn).click());
            setPermisos(_gpGuardar);

            inputTipo.on("change", function () {
                if ($(this).val() != "") {
                    if ($(this).val() == "1") {
                        btnFiltroRechazar.show();
                        btnFiltroValidar.show();
                        btnModalAutorizar.hide();
                    } else {
                        btnFiltroRechazar.hide();
                        btnFiltroValidar.hide();
                        btnModalAutorizar.show();
                    }
                }
            });

            btnFiltroValidar.on("click", function () {
                Alert2AccionConfirmar('¡Cuidado!', '¿Desea validar las facturas seleccionadas?', 'Confirmar', 'Cancelar', () => fncValidarFactura(true));

            });

            btnFiltroRechazar.on("click", function () {
                Alert2AccionConfirmar('¡Cuidado!', '¿Desea rechazar las facturas seleccionadas?', 'Confirmar', 'Cancelar', () => fncValidarFactura(false));
                // fncValidarFactura(false);
            });

            btnModalAutorizar.on("click", function () {
                Alert2AccionConfirmar('¡Cuidado!', '¿Desea autorizar las facturas seleccionadas?', 'Confirmar', 'Cancelar', () => fncAutorizarFactura(true));
            });
        }
        let dlldia = 0;
        const getDolarDelDia = new URL(window.location.origin + '/Administrativo/Poliza/getDolarDelDia');
        const CargarFacturasPendientes = new URL(window.location.origin + '/Administrativo/Propuesta/CargarFacturasPendientes');
        const getLimitNoProveedores = new URL(window.location.origin + '/Administrativo/Propuesta/getLimitNoProveedores');
        const GuardarMontosProgrPagos = new URL(window.location.origin + '/Administrativo/Propuesta/GuardarMontosProgrPagos');
        function IniciarForm() {
            getLimitProv();
            cboCC.fillCombo('/Administrativo/Poliza/getCC', {}, true, null);
            cboCC.select2({
                placeholder: "  TODOS",
                allowClear: true
            });

            txtFechaCorte.datepicker();
            txtFechaPago.datepicker().datepicker("setDate", new Date());
            IniciarTblProgrPagos();
            txtPropHTotal.text(maskNumero(0));
            txtPropHSel.text(maskNumero(0));
            txtPropHTotalDll.text(maskNumero(0));
            txtPropHSelDll.text(maskNumero(0));
            setFooter({ cc: "", oc: "", saldo: 0, tipoMoneda: "", tmb: "", tmbDescripcion: "", tmp: "", tmpDescripcion: "" });


        }
        async function getLimitProv() {
            response = await ejectFetchJson(getLimitNoProveedores);
            if (response.success) {
                txtProvMin.val(response.limit[0]);
                txtProvMin.data().prov = response.limit[0];
                txtProvMax.val(response.limit[1]);
                txtProvMax.data().prov = response.limit[1];
            }
        }
        async function setDolarDelDia() {
            try {
                response = await ejectFetchJson(getDolarDelDia, { fecha: txtFechaPago.val() });
                if (response.success) {
                    dlldia = response.dll;
                }
            } catch (o_O) { dlldia = 1; }
        }
        function setPermisos(permiso) {
            if (permiso == 1) {
                btnGuardar.show();
            }
            else {
                btnGuardar.hide();
            }
        }
        function setDataLimit() {
            txtProvMin.val(txtProvMin.data().prov);
            txtProvMax.val(txtProvMax.data().prov);
        }
        function chngSetAllSelOpt() {
            let estodo = !this.value,
                select = $(this).next().find("select[multiple]");
            this.value = estodo;
            limpiarMultiselect(select);
            if (estodo) {
                let lstValor = $(`#${select.attr("id")}`).find(`option`).toArray().map(option => option.value);
                select.val(lstValor);
                convertToMultiselect(select);
            }
        }
        function setSaldosTotales() {
            let sumaSaldo = 0;
            let sumaSaldoDll = 0;
            dtTblData.rows().every(function (i) {
                let node = $(this.node()),
                    data = this.data();
                if (data.tipoMoneda == 'DLL') {
                    if (node.find('input:checkbox').prop('checked')) {
                        sumaSaldoDll += unmaskNumero(node.find('input').val());
                    }
                }
                else {
                    if (node.find('input:checkbox').prop('checked')) {
                        sumaSaldo += unmaskNumero(node.find('input').val());
                    }
                }
            });
            txtPropHSel.text(maskNumero(sumaSaldo));
            txtPropHSelDll.text(maskNumero(sumaSaldoDll));
        }
        function IniciarTblProgrPagos() {
            dtTblData = tblData.DataTable({
                paging: false,
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                "sScrollX": "100%",
                "sScrollXInner": "100%",
                "bScrollCollapse": true,
                scrollY: '65vh',
                scrollCollapse: true,
                "bLengthChange": false,
                "searching": false,
                "bFilter": true,
                "bInfo": true,
                "bAutoWidth": false,
                columnDefs: [
                    { targets: [0, 1, 2, 3, 4, 5, 9], width: '8px' },
                ],
                columns: [
                    {
                        title: ' ',
                        data: 'id',
                        createdCell: function (td, data, rowData, row, col) {
                            var html = "<input class='form-data checkFactura' type='checkbox'  data-index='" + rowData.id + "'>";
                            $(td).html(html);

                        }
                    },
                    {
                        title: 'Req',
                        sortable: false,
                        data: 'id',
                        createdCell: function (td, data, rowData, row, col) {
                            var html = "<button type='button' class='btn btn-primary btn-sm descargar-req' data-index='" + rowData.id + "'>" +
                                "<span class='fa fa-file'></span></button>";
                            $(td).html(html);

                        }
                    },
                    {
                        title: 'OC',
                        sortable: false,
                        data: 'id',
                        createdCell: function (td, data, rowData, row, col) {
                            var html = "<button type='button' class='btn btn-primary btn-sm descargar-oc' data-cc='" + rowData.cc + "' data-oc='" + rowData.oc + "'>" +
                                "<span class='fa fa-file-pdf'></span></button>";
                            $(td).html(html);

                        }
                    },
                    {
                        title: 'PDF',
                        sortable: false,
                        data: 'pdf', createdCell: function (td, data, rowData, row, col) {
                            if (data == null) $(td).html("");
                            else if (data.length > 0) {
                                var url = rowData.pdf.replace("C:\\", "\\\\10.1.0.100\\");
                                var html = "<button type='button' class='btn btn-primary btn-sm descargar-pdf' data-index='" + rowData.id + "' data-url='" + url + "'  >" +
                                    "<span class='fa fa-file-pdf'></span></button>";
                                $(td).html(html);
                            }
                        }
                    },
                    {
                        title: 'XML',
                        sortable: false,
                        data: 'xml', createdCell: function (td, data, rowData, row, col) {
                            if (data == null) $(td).html("");
                            else if (data.length > 0) {
                                var url = rowData.xml.replace("C:\\", "\\\\10.1.0.100\\");
                                var html = "<button type='button' class='btn btn-primary btn-sm descargar-xml' data-index='" + rowData.id + "' data-url='" + url + "'  >" +
                                    "<span class='fa fa-file-code'></span></button>";
                                $(td).html(html);
                            }
                        }
                    },
                    {
                        title: 'ZIP',
                        sortable: false,
                        data: 'id',
                        createdCell: function (td, data, rowData, row, col) {
                            var html = "<button type='button' class='btn btn-primary btn-sm descargar-zip' data-index='" + rowData.id + "'>" +
                                "<span class='fa fa-file'></span></button>";
                            $(td).html(html);

                        }
                    },
                    { title: 'CC', data: 'cc' },
                    {
                        title: 'AC',
                        data: 'ac', visible: (_gpEmpresa == 2 ? true : false), createdCell: function (td, data, rowData, row, col) {
                            $(td).html(data);
                            $(td).prop("title", rowData.acDesc);
                        }
                    },
                    { title: 'OC', data: 'oc' },
                    {
                        title: 'Vence', data: 'vence', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(moment(data).format("DD/MM/YYYY"));
                        }
                    },
                    {
                        title: '',
                        data: 'tipoMoneda',
                        createdCell: function (td, data, rowData, row, col) {
                            if (_gpEmpresa == 6 && data == 'MXN') $(td).html("S/.");
                            else $(td).html(data);
                        }
                    },
                    {
                        title: 'Monto',
                        data: 'monto', width: "100px",
                        createdCell: function (td, data, rowData, row, col) {
                            $(td).html(maskNumero(data + rowData.iva)).addClass('text-right');
                            // $(td).html(`
                            // <div class='row'>
                            //     <div class='col-sm-12'>
                            //         <input type='text' class='inputRowTotal' value='${maskNumero(data + rowData.iva)}' style='width: 100%; text-align: right;'  ${rowData.estatus == "P" ? "readonly" : ""}/>
                            //     </div >   
                            // </div > `);
                        }

                    },
                    {
                        title: 'TM',
                        data: 'tm',
                        createdCell: function (td, data, rowData, row, col) {
                            // $(td).html(data + ' ' + rowData.tmbDescripcion);
                            $(td).html(`
                            <div class='row'>
                                <div class='col-sm-4'>
                                    <input type='text' class='inputRowTM' value=${data} style='width: 50%' ${rowData.estatus == "P" ? "readonly" : ""}/>
                                </div>    
                                <div class='col-sm-8' style="text-align: center;">
                                    <span class='spanRowDescTM'>${rowData.tmbDescripcion}<span>
                                </div>    
                            </div>`);
                        }
                    },
                    {
                        title: 'Factura', data: 'factura',
                        createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`
                            <div class='row'>
                                <div class='col-sm-2'>
                                </div>    
                                <div class='col-sm-8'>
                                    <input type='text' class='inputRowFactura' value='${data}' style='width: 100%; text-align: center;'  ${rowData.estatus == "P" ? "readonly" : ""}/>
                                </div >
                                <div class='col-sm-2'>
                                </div>    
                            </div > `);
                        }
                    },
                    {
                        title: 'Póliza',
                        data: 'id',
                        createdCell: function (td, data, rowData, row, col) {
                            $(td).html(data);
                        }
                    },

                ],
                drawCallback: function (settings) {
                    // tblData.on('click', '.descargar-xml', function () {
                    //     AbrirArchivo($(this).attr("data-url"));
                    //     $("#report").attr("src", $(this).attr("data-url"));
                    // });
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    api.column({ page: 'current' }).data().each((group, i, dtable) => {
                        const dataBefore = dtable.data()[i - 1];
                        const data = dtable.data()[i];
                        if (data.activo_fijo) {
                            $(rows).eq(i).addClass('activo_fijo');
                        }
                        if (data.numproPeru != null && data.numproPeru != "") {
                            $(rows).eq(i).addClass('peru');
                        }
                        if (i > 0) {
                            if (dataBefore.proveedorID !== data.proveedorID) {
                                let lstProv = dtable.data().filter(prov => dataBefore.proveedorID === prov.proveedorID);
                                let suma = lstProv.reduce((suma, current) => suma + (current.esPagado ? (current.monto_plan) : 0), 0);
                                let sumaProv = lstProv.reduce((suma, current) => suma + current.monto + current.iva, 0);
                                $(rows).eq(i).before(`< tr class= "prov${dataBefore.proveedorID}" >
                            <td colspan="14" class="fondoProveedor">${dataBefore.proveedorID} - ${dataBefore.proveedor}</td>
                                </tr > `);
                            }
                            if (i == dtable.length - 1) {
                                let lstProv = dtable.data().filter(prov => data.proveedorID === prov.proveedorID);
                                let suma = lstProv.reduce((suma, current) => suma + (current.esPagado ? (current.monto_plan) : 0), 0);
                                let sumaProv = lstProv.reduce((suma, current) => suma + current.monto + current.iva, 0);
                                $(rows).eq(i).after(`< tr class= "prov${data.proveedorID}" >
                            <td colspan="14" class="fondoProveedor">${data.proveedorID} - ${data.proveedor}</td>
                                </tr > `);
                            }
                        }
                        if (_gpGuardar == 1) {
                            $(".btnProv").show();
                        }
                        else {
                            $(".btnProv").hide();
                        }
                    });
                }
                , initComplete: function (settings, json) {
                    tblData.on('click', '.descargar-oc', function () {
                        menuConfig.parametros = {
                            tipo: 'oc',
                            cc: $(this).attr("data-cc"),
                            oc: $(this).attr("data-oc"),
                            url: ''
                        }
                        mostrarMenu();

                        //AbrirArchivo($(this).attr("data-url"));

                    });
                    tblData.on('click', '.descargar-pdf', function () {
                        menuConfig.parametros = {
                            tipo: 'pdf',
                            cc: '',
                            oc: '',
                            url: $(this).attr("data-url")
                        }
                        mostrarMenu();

                        //AbrirArchivo($(this).attr("data-url"));

                    });
                    tblData.on('click', '.descargar-xml', function () {

                        let arr = $(this).attr("data-url").split('\\');
                        let nombre = arr[arr.length - 1];
                        location.href = 'GetArchivoPortal?nombreArchivo=' + nombre;
                        //AbrirArchivo($(this).attr("data-url"));

                    });
                    tblData.on('click', '.descargar-req', function () {
                        let rowData = dtTblData.row($(this).closest('tr')).data();

                        location.href = 'GetRutaRequerimiento?cc=' + rowData.cc + "&numero=" + rowData.oc;
                    });
                    tblData.on('change', 'input', function () {
                        if (_gpGuardar == 1) {
                            let row = $(this).closest('tr')
                                , data = dtTblData.row(row).data();
                            if (data !== undefined) {
                                let inp = row.find('input')
                                    , val = unmaskNumero(inp.val());
                                if (val === 0 || val > data.monto) {
                                    val = data.monto;
                                }
                                data.saldo = val;
                                setTotalProv(data);
                                setSaldosTotales();
                            }
                        }
                    });
                    tblData.on('click', '.btnProv', function () {
                        let row = $(this).closest('tr')
                            , prev = row.prev()[0]
                            , data = dtTblData.rows(prev).data()[0];

                        dtTblData.rows().every(function (i) {
                            let node = $(this.node())
                                , current = this.data();
                            if (data.proveedorID == current.proveedorID) {
                                var aplicado = node.find('input').data("aplicado");

                                if (!node.find('input').hasClass("desactivado") && aplicado == "0") {
                                    node.find('input').data("aplicado", "1");
                                    node.find('input:checkbox').prop('checked', true);
                                }
                                else if (!node.find('input').hasClass("desactivado") && aplicado == "1") {
                                    node.find('input').data("aplicado", "0");
                                    node.find('input:checkbox').prop('checked', false);
                                }
                            }
                        });
                        setTotalProv(data);
                        setSaldosTotales();
                    });
                    tblData.on("change", ".inputRowTM", function (event, noRecursar) {
                        let rowData = dtTblData.row($(this).closest('tr')).data();
                        let row = $(this).closest('tr');
                        let spanDescTM = $(row.find("span.spanRowDescTM"));

                        if (!noRecursar) {
                            axios.post("GetDescripcionTM", { tm: $(this).val() }).then(response => {
                                let { success, items, message } = response.data;
                                if (success) {
                                    spanDescTM.text(response.data.desc);
                                } else {
                                    //REGRESAR AL VALOR CONSULTADO
                                    $(this).val(rowData.tm, ["noRecursar"]);
                                    spanDescTM.text(rowData.tmbDescripcion);
                                }
                            }).catch(error => {
                                $(this).val(rowData.tm, ["noRecursar"]);
                                spanDescTM.text(rowData.tmbDescripcion);
                            });
                        }
                    });
                    tblData.on("change", ".inputRowTotal", function (event, noRecursar) {
                        let rowData = dtTblData.row($(this).closest('tr')).data();

                        if (!noRecursar) {
                            // axios.post("GetDescripcionTM", { tm: $(this).val() }).then(response => {
                            //     let { success, items, message } = response.data;
                            //     if (success) {
                            //         spanDescTM.text(response.data.desc);
                            //     } else {
                            //         //REGRESAR AL VALOR CONSULTADO
                            //         $(this).val(rowData.tm, ["noRecursar"]);
                            //         spanDescTM.text(rowData.tmbDescripcion);
                            //     }
                            // }).catch(error => {
                            //     $(this).val(rowData.tm, ["noRecursar"]);
                            //     spanDescTM.text(rowData.tmbDescripcion);
                            // });
                            if ($(this).val() == "") {
                                $(this).val(maskNumero(rowData.monto + rowData.iva), ["noRecursar"]);
                            } else {
                                let newVal = unmaskNumero($(this).val());

                                if (newVal == 0) {
                                    $(this).val(maskNumero(rowData.monto + rowData.iva), ["noRecursar"]);

                                } else {
                                    $(this).val(maskNumero(newVal), ["noRecursar"]);

                                }
                            }
                        }
                    });
                }
            });
        }

        function AbrirArchivo(url) {
            window.location.href = "/Administrativo/Propuesta/fnDownloadFile?descargar=" + url;
        }
        function setTotalProv({ proveedorID }) {
            let sumGroup = 0;
            dtTblData.rows().every(function (i) {
                let node = $(this.node())
                    , current = this.data();
                if (proveedorID === current.proveedorID) {
                    if (node.find('input:checkbox').prop('checked')) {
                        sumGroup += unmaskNumero(node.find('input').val());
                    }
                }
            });
            tblData.find(`tr.prov${proveedorID} td: eq(3)`).text(maskNumero(sumGroup));
        }
        function setFooter({ cc, oc, saldo, tipoMoneda, tmb, tmbDescripcion, tmp, tmpDescripcion }) {
            txtPropFOC.val(`${cc} - ${oc}`);
            txtPropFTmb.val(`${tmb} - ${tmbDescripcion}`);
            txtPropFFac.val(maskNumero(saldo));
            txtPropFPag.val(maskNumero(0));
            txtPropFMax.val(`${maskNumero(saldo)} - ${tipoMoneda}`);
            txtPropFTmp.val(`${tmp} - ${tmpDescripcion}`);
            txtPropFSol.val(maskNumero(saldo));
            txtPropFRec.val(maskNumero(saldo));
        }
        async function CargarTabla() {
            try {
                dtTblData.clear().draw();
                //txtPropHSel.text(maskNumero(0));
                //txtPropHTotal.text(maskNumero(0));
                //txtPropHSelDll.text(maskNumero(0));
                //txtPropHTotalDll.text(maskNumero(0));
                response = await ejectFetchJson(CargarFacturasPendientes, { min: txtProvMin.val(), max: txtProvMax.val(), cc: cboCC.val(), tipo: inputTipo.val() });
                if (response.success) {
                    if (response.lst.length > 0) {
                        let ivaTotal = response.total + response.iva;
                        let ivaTotalDlls = response.totalDll + response.ivaDLLS;
                        dtTblData.rows.add(response.lst).draw();
                        txtPropHTotal.text(maskNumero(ivaTotal));
                        txtPropHTotalDll.text(maskNumero(ivaTotalDlls));
                        tblData.find('.btnProv').each(function () {
                            this.value = 0;
                        });
                    } else {
                        AlertaGeneral("Alerta", "¡No se encontraron facturas pendientes de autorización!");
                    }
                    setSaldosTotales();
                } else {
                    AlertaGeneral("Alerta", "¡No se encontraron facturas pendientes de autorización!");
                }
            } catch (error) {
                AlertaGeneral(`Alerta`, `Error al cargar la tabla.`);
            }
        }
        async function LimpiarTabla() {
            try {
                dtTblData.clear().draw();
                txtPropHSel.text(maskNumero(0));
                txtPropHTotal.text(maskNumero(0));
                txtPropHSelDll.text(maskNumero(0));
                txtPropHTotalDll.text(maskNumero(0));

            } catch (error) {
                AlertaGeneral(`Alerta`, `Error al cargar la tabla.`);
            }
        }
        function saveSplitted() {
            let data = MontoPropPagoDTO();
            if (data.length > 0) {
                var scheme = { lst: new Array(), pago: txtFechaPago.val() };
                $.sm_SplittedSave(GuardarMontosProgrPagos, data, scheme, 20, LimpiarTabla);
            }
            else {
                AlertaGeneral("Alerta", "¡Debe configurar almenos un monto para continuar!");
            }
        }

        function seleccionarTodo() {
            let sum = 0;
            let prov = '0';

            dtTblData.rows().every(function (i) {
                let node = $(this.node());
                let current = this.data();
                var aplicado = node.find('input').data('aplicado');
                if ((prov == '0' || prov == current.proveedorID)) {
                    if (!node.find('input').hasClass("desactivado") && aplicado == "0") {
                        node.find('input').data("aplicado", "1");
                        node.find('input:checkbox').prop('checked', true);
                        sum += unmaskNumero(node.find('input').val());
                    }
                    else if (!node.find('input').hasClass("desactivado") && aplicado == "1") {
                        node.find('input').data("aplicado", "0");
                        node.find('input:checkbox').prop('checked', false);
                        sum = 0;
                    }
                    prov = current.proveedorID;
                } else {
                    // prov = 0;
                    tblData.find(`tr.prov${prov} td: eq(3)`).text(maskNumero(sum));
                    sum = 0;
                    if (!node.find('input').hasClass("desactivado") && aplicado == "0") {
                        node.find('input').data("aplicado", "1");
                        node.find('input:checkbox').prop('checked', true);
                        sum += unmaskNumero(node.find('input').val());
                    }
                    else if (!node.find('input').hasClass("desactivado") && aplicado == "1") {
                        node.find('input').data("aplicado", "0");
                        node.find('input:checkbox').prop('checked', false);
                        sum = 0;
                    }
                    prov = current.proveedorID;
                }

                if (i == dtTblData.rows().data().length - 1) {
                    tblData.find(`tr.prov${current.proveedorID} td: eq(3)`).text(maskNumero(sum));
                }
            });

            setSaldosTotales();

        }

        async function GuardarMontos() {
            try {
                response = await ejectFetchJson(GuardarMontosProgrPagos, { lst: MontoPropPagoDTO(), pago: txtFechaPago.val() });
                if (response.success) {
                    if (response.exito) {
                        CargarTabla();
                        AlertaGeneral("Aviso", "Se han guardado los montos con éxito.");
                    }
                    else {
                        AlertaGeneral("Aviso", "Se ha producido un error. No se han guardado los montos.");
                    }
                }
            } catch (error) { }
        }
        function MontoPropPagoDTO() {
            let lst = [];

            dtTblData.rows().every(function (i) {
                let node = $(this.node())
                    , data = this.data()
                    , valor = unmaskNumero(node.find('input').val());
                if (node.find('input[type=checkbox]').is(":checked")) {
                    if (valor > 0) {
                        data.monto = valor;
                        data.fecha = moment(data.fecha).format("DD/MM/YYYY");
                        data.vence = moment(data.vence).format("DD/MM/YYYY");
                        lst.push(data);
                    }
                }
            });
            return lst;
        }

        // string RutaBasePortalRequisitos = @"\\10.1.0.100\Portal\Requisitos";
        // string RutaBasePortalRequisitosEnkontrol = @"\\10.1.0.125\po\req";
        // string RutaBasePortal = @"\\10.1.0.100\Portal\xml\FileCopyPruebas";

        function reporteDocumento(tipo, cc, oc, url, esVisor) {
            if (!esVisor) { $.blockUI({ message: 'Procesando...' }); }
            var path = "";
            switch (tipo) {
                case 'oc':
                    {
                        path = '/Reportes/Vista.aspx?' +
                            'esDescargaVisor=' + esVisor +
                            '&esVisor=' + true +
                            '&idReporte=' + 201 +
                            '&cc=' + cc +
                            '&numero=' + oc;
                        report.attr('src', path);
                        document.getElementById('report').onload = function () {
                            $.unblockUI();
                            $('#myModal').modal('show');
                        };
                    }

                    break;
                case 'pdf':
                    {
                        let arr = url.split('\\');
                        let nombre = arr[arr.length - 1];
                        if (!esVisor) {
                            $.post('CargarDatosArchivoPortal', { nombreArchivo: nombre })
                                .then(response => {
                                    if (response.success) {
                                        $('#myModal').data().ruta = null;
                                        $('#myModal').modal('show');
                                    } else {
                                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                                    }
                                }, error => {
                                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                                });
                        } else {

                            location.href = 'GetArchivoPortal?nombreArchivo=' + nombre;
                        }

                    }
                    break;
                default:

                    break;
            }
        }

        //#region BACK
        function GetListaFacturasValidar() {
            let lst = [];
            dtTblData.rows().every(function (i) {
                let node = $(this.node())
                    , data = this.data();
                if (node.find('input[type=checkbox]').is(":checked")) {
                    // data.monto = valor;
                    // data.fecha = moment(data.fecha).format("DD/MM/YYYY");
                    // data.vence = moment(data.vence).format("DD/MM/YYYY");
                    var inputTM = node.find(".inputRowTM").val();
                    var inputFactura = node.find(".inputRowFactura").val();
                    // var inputMonto = unmaskNumero(node.find(".inputRowTotal").val());

                    if (inputFactura == "") {
                        Alert2Warning("Favor de capturar la factura para Poliza: " + data.id);
                        return [];
                    }

                    // lst.push({ id: data.id, esAuth: true, tm: inputTM, factura: inputFactura, total: inputMonto });
                    lst.push({ id: data.id, esAuth: true, tm: inputTM, factura: inputFactura });
                }
            });
            return lst;
        }

        function GetListaFacturasRechazar() {
            let lst = [];
            dtTblData.rows().every(function (i) {
                let node = $(this.node())
                    , data = this.data();
                if (node.find('input[type=checkbox]').is(":checked")) {
                    var inputTM = node.find("#inputRowTM").val();

                    lst.push({ id: data.id, esAuth: false, tm: inputTM, factura: 0, cc: data.cc, numero: data.oc });
                }
            });
            return lst;
        }

        function GetListaFacturasAutorizar() {
            let lst = [];
            dtTblData.rows().every(function (i) {
                let node = $(this.node())
                    , data = this.data();
                if (node.find('input[type=checkbox]').is(":checked")) {
                    var inputTM = node.find("#inputRowTM").val();

                    lst.push({ cc: data.cc, numero: data.oc });
                }
            });
            return lst;
        }

        function fncValidarFactura(esAuth) {
            var lstFacturas = [];

            if (esAuth) {
                lstFacturas = GetListaFacturasValidar();
            } else {
                lstFacturas = GetListaFacturasRechazar();
            }

            if (lstFacturas.length > 0) {
                axios.post("ValidarFactura", { lstFiltro: lstFacturas }).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        if (esAuth) {
                            Alert2Exito("Facturas validadas con exito");

                        } else {
                            Alert2Exito("Facturas rechazadas con exito");

                        }
                        CargarTabla();

                    } else {
                        Alert2Warning("Ocurrio algo mal, favor de contactarse con el departamento de sistemas");
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncAutorizarFactura() {
            var lstFacturas = GetListaFacturasAutorizar();

            axios.post("AutorizarFactura", { lstFiltro: lstFacturas }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Facturas autorizadas con exito");
                    CargarTabla();

                } else {
                    Alert2Warning("Ocurrio algo mal, favor de contactarse con el departamento de sistemas");
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        init();
    }
    $(document).ready(() => {
        Administrativo.Contabilidad.Propuesta.ValidacionFacturas = new ValidacionFacturas();
    }).ajaxStart(() => {
        $.blockUI({
            baseZ: 100000,
            message: 'Procesando...'
        });
    }).ajaxStop(() => { $.unblockUI(); });
})();   