(() => {
    $.namespace('Administrativo.Contabilidad.Propuesta.ProgramacionPagos');
    ProgramacionPagos = function () {
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
        const tblProgramacionPagos = $("#tblProgramacionPagos");
        const btnTodo = $('#btnTodo');

        let dtProgrPagos;

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
        }
        let dlldia = 0;
        const getDolarDelDia = new URL(window.location.origin + '/Administrativo/Poliza/getDolarDelDia');
        const CargarTblProgrPagos = new URL(window.location.origin + '/Administrativo/Propuesta/CargarTblProgrPagos');
        const getLimitNoProveedores = new URL(window.location.origin + '/Administrativo/Propuesta/getLimitNoProveedores');
        const GuardarMontosProgrPagos = new URL(window.location.origin + '/Administrativo/Propuesta/GuardarMontosProgrPagos');
        function IniciarForm() {
            getLimitProv();
            if (_gpEmpresa == 1 || _gpEmpresa == 3) {
                cboCC.fillCombo('/Administrativo/Poliza/getCC', {}, true, null);
                convertToMultiselect(cboCC);
            }
            else {
                $(".cboCC").hide();
                convertToMultiselect(cboCC);
            }

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
            dtProgrPagos.rows().every(function (i) {
                let node = $(this.node()),
                    data = this.data();
                if (data.tipoMoneda == 'DLL') {
                    //node.find('input:checkbox').prop('checked', true);
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
            dtProgrPagos = tblProgramacionPagos.DataTable({
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
                columns: [
                    {
                        sortable: false,
                        data: 'pdf', createdCell: function (td, data, rowData, row, col) {
                            if (data == null) $(td).html("");
                            else if (data.length > 0) {
                                var url = rowData.pdf.replace("C:\\", "\\\\10.1.0.125\\");
                                var html = "<button type='button' class='btn btn-primary btn-sm descargar-pdf' data-index='" + rowData.id + "' data-url='" + url + "'  >" +
                                    "<span class='fa fa-file-pdf'></span></button>";
                                $(td).html(html);
                            }
                        }
                    },
                    {
                        sortable: false,
                        data: 'xml', createdCell: function (td, data, rowData, row, col) {
                            if (data == null) $(td).html("");
                            else if (data.length > 0) {
                                var url = rowData.xml.replace("C:\\", "\\\\10.1.0.125\\");
                                var html = "<button type='button' class='btn btn-primary btn-sm descargar-xml' data-index='" + rowData.id + "' data-url='" + url + "'  >" +
                                    "<span class='fa fa-file-code'></span></button>";
                                $(td).html(html);
                            }
                        }
                    },
                    { data: 'factura' },
                    {
                        data: 'vence', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(moment(data).format("DD/MM/YYYY"));
                        }
                    },

                    { data: 'tm' },
                    { data: 'cc' },
                    {
                        data: 'ac', visible: (_gpEmpresa == 2 ? true : false), createdCell: function (td, data, rowData, row, col) {
                            $(td).html(data);
                            $(td).prop("title", rowData.acDesc);
                        }
                    },
                    { data: 'oc' },
                    { data: 'concepto' },
                    {
                        data: 'monto', width: "100px", createdCell: function (td, data, rowData, row, col) {
                            $(td).html(maskNumero(data + rowData.iva)).addClass('text-right');
                        }

                    },
                    //{ data: 'tipoMoneda' },
                    {
                        data: 'tipoMoneda', createdCell: function (td, data, rowData, row, col) {
                            if (_gpEmpresa == 6 && data == 'MXN') $(td).html("S/.");
                            else $(td).html(data);
                        }
                    },
                    {
                        data: 'saldo', width: "150px", createdCell: function (td, data, rowData, row, col) {
                            if (_gpGuardar == 1) {
                                //var html = `<div class='input-group'><input type="text" class="form-control monto text-right ${rowData.esPagado ? 'desactivado' : ''}" data-id="${rowData.id}" data-saldo="${rowData.monto}" data-aplicado="0" value="${maskNumero(rowData.monto_plan - data)}" ${rowData.esPagado ? "disabled" : ""}>` + (rowData.esPagado ? '' : '<span class="input-group-btn"><button type="button" class="btn btn-default btn-sm eraser" style="height: 26px;"><i class="fa fa-eraser" aria-hidden="true"></i></button></span></div>');
                                var html = `<div class="input-group"><input type="text" class="form-control monto text-right ${rowData.esPagado ? 'desactivado' : ''} data-id="${rowData.id}" data-saldo="${rowData.monto}" data-aplicado="0" value="${maskNumero(rowData.monto_plan - data)}" readonly /><span class="input-group-addon"><input type="checkbox" /></span></div>`;
                            }
                            else {
                                var html = `<input type="text" class="form-control monto text-right ${rowData.esPagado ? 'desactivado' : ''}" data-id="${rowData.id}" data-saldo="${rowData.monto}" data-aplicado="0" value="${maskNumero(rowData.monto_plan - data)}" readonly>`;
                            }
                            $(td).html(html);
                        }
                    },
                    { data: 'tmbDescripcion' },
                    { data: 'tmpDescripcion' },

                ],
                drawCallback: function (settings) {
                    tblProgramacionPagos.on('click', '.descargar-pdf', function () {
                        AbrirArchivo($(this).attr("data-url"));

                    });
                    tblProgramacionPagos.on('click', '.descargar-xml', function () {
                        AbrirArchivo($(this).attr("data-url"));
                        $("#report").attr("src", $(this).attr("data-url"));
                    });
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
                                $(rows).eq(i).before(`<tr class="prov${dataBefore.proveedorID}">
                                    <td colspan="9" class="fondoTotal">${dataBefore.proveedorID} - ${dataBefore.proveedor}<span style="float:right">Saldo del proveedor</span></td>
                                    <td class="text-right fondoTotal">${maskNumero(sumaProv)}</td>
                                    <td class="fondoTotal">${dataBefore.tipoMoneda}</td>
                                    <td class="text-right fondoTotal">${maskNumero(suma)}</td>
                                    <td colspan="2"><button type="button" class="btn btn-default btn-sm btnProv" data-proveedor="${dataBefore.proveedorID}"><i class="fas fa-hand-holding-usd"></i> Pagar Todo</button></td>
                                </tr>`);
                            }
                            if (i == dtable.length - 1) {
                                let lstProv = dtable.data().filter(prov => data.proveedorID === prov.proveedorID);
                                let suma = lstProv.reduce((suma, current) => suma + (current.esPagado ? (current.monto_plan) : 0), 0);
                                let sumaProv = lstProv.reduce((suma, current) => suma + current.monto + current.iva, 0);
                                $(rows).eq(i).after(`<tr class="prov${data.proveedorID}">
                                    <td colspan="9" class="fondoTotal">${data.proveedorID} - ${data.proveedor}<span style="float:right">Saldo del proveedor</span></td>
                                    <td class="text-right fondoTotal">${maskNumero(sumaProv)}</td>
                                    <td class="fondoTotal">${data.tipoMoneda}</td>
                                    <td class="text-right fondoTotal">${maskNumero(suma)}</td>
                                    <td colspan="2"><button type="button" class="btn btn-default btn-sm btnProv" data-proveedor="${data.proveedorID}"><i class="fas fa-hand-holding-usd"></i> Pagar Todo</button></td>
                                </tr>`);
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
                    // tblProgramacionPagos.find('tbody').on('click', 'tr', function () {
                    //     let data = dtProgrPagos.row(this).data();
                    //     if (_gpGuardar == 1) {

                    //         if (data !== undefined) {
                    //             let inp = $(this).closest('tr').find('input')
                    //                 , val = unmaskNumero(inp.val());
                    //             if (!inp.hasClass("desactivado")) {

                    //                 if (val === 0 || val > data.monto) {
                    //                     val = data.monto;
                    //                 }
                    //                 inp.val(maskNumero(val));
                    //                 data.saldo = val;
                    //                 setTotalProv(data);
                    //                 setSaldosTotales();
                    //             }
                    //         }
                    //     }
                    //     setFooter(data);
                    // });



                    tblProgramacionPagos.on('change', 'input', function () {
                        if (_gpGuardar == 1) {
                            let row = $(this).closest('tr')
                                , data = dtProgrPagos.row(row).data();
                            if (data !== undefined) {
                                let inp = row.find('input')
                                    , val = unmaskNumero(inp.val());
                                if (val === 0 || val > data.monto) {
                                    val = data.monto;
                                }
                                //inp.val(maskNumero(val));
                                data.saldo = val;
                                setTotalProv(data);
                                setSaldosTotales();
                            }
                        }
                    });
                    tblProgramacionPagos.on('click', '.btnProv', function () {
                        let row = $(this).closest('tr')
                            , prev = row.prev()[0]
                            , data = dtProgrPagos.rows(prev).data()[0];

                        dtProgrPagos.rows().every(function (i) {
                            let node = $(this.node())
                                , current = this.data();
                            if (data.proveedorID == current.proveedorID) {
                                var aplicado = node.find('input').data("aplicado");

                                if (!node.find('input').hasClass("desactivado") && aplicado == "0") {
                                    //node.find('input').val(maskNumero(current.monto));
                                    node.find('input').data("aplicado", "1");
                                    node.find('input:checkbox').prop('checked', true);
                                }
                                else if (!node.find('input').hasClass("desactivado") && aplicado == "1") {
                                    //node.find('input').val(maskNumero(0));
                                    node.find('input').data("aplicado", "0");
                                    node.find('input:checkbox').prop('checked', false);
                                }
                            }
                        });
                        setTotalProv(data);
                        setSaldosTotales();
                    });
                    // tblProgramacionPagos.on('click', '.eraser', function () {
                    //     let row = $(this).closest('tr')
                    //         , data = dtProgrPagos.row(row).data();
                    //     if (data !== undefined) {
                    //         let inp = row.find('input')
                    //             , val = unmaskNumero(inp.val());

                    //         inp.val(maskNumero(0));
                    //         data.saldo = 0;
                    //         setTotalProv(data);
                    //         setSaldosTotales();
                    //     }
                    // });
                }
            });
        }

        function AbrirArchivo(url) {
            window.location.href = "/Administrativo/Propuesta/fnDownloadFile?descargar=" + url;
        }
        function setTotalProv({ proveedorID }) {
            let sumGroup = 0;
            //setDolarDelDia();
            dtProgrPagos.rows().every(function (i) {
                let node = $(this.node())
                    , current = this.data();
                if (proveedorID === current.proveedorID) {
                    if (node.find('input:checkbox').prop('checked')) {
                        sumGroup += unmaskNumero(node.find('input').val());
                    }
                }
            });
            tblProgramacionPagos.find(`tr.prov${proveedorID} td:eq(3)`).text(maskNumero(sumGroup));
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
                dtProgrPagos.clear().draw();
                txtPropHSel.text(maskNumero(0));
                txtPropHTotal.text(maskNumero(0));
                txtPropHSelDll.text(maskNumero(0));
                txtPropHTotalDll.text(maskNumero(0));
                response = await ejectFetchJson(CargarTblProgrPagos, { min: txtProvMin.val(), max: txtProvMax.val(), cc: cboCC.val(), fecha: txtFechaCorte.val() });
                if (response.success) {
                    if (response.lst.length > 0) {
                        let ivaTotal = response.total + response.iva;
                        let ivaTotalDlls = response.totalDll + response.ivaDLLS;
                        // let ivaTotal = response.total;
                        // let ivaTotalDlls = response.totalDll;
                        dtProgrPagos.rows.add(response.lst).draw();
                        // txtPropHTotal.text(maskNumero(response.total));
                        txtPropHTotal.text(maskNumero(ivaTotal));
                        // txtPropHTotalDll.text(maskNumero(response.totalDll));
                        txtPropHTotalDll.text(maskNumero(ivaTotalDlls));
                        tblProgramacionPagos.find('.btnProv').each(function () {
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
                dtProgrPagos.clear().draw();
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

            dtProgrPagos.rows().every(function (i) {
                let node = $(this.node());
                let current = this.data();

                var aplicado = node.find('input').data('aplicado');

                if ((prov == '0' || prov == current.proveedorID)) {
                    if (!node.find('input').hasClass("desactivado") && aplicado == "0") {
                        //node.find('input').val(maskNumero(current.monto));
                        node.find('input').data("aplicado", "1");
                        node.find('input:checkbox').prop('checked', true);
                        sum += unmaskNumero(node.find('input').val());
                    }
                    else if (!node.find('input').hasClass("desactivado") && aplicado == "1") {
                        //node.find('input').val(maskNumero(0));
                        node.find('input').data("aplicado", "0");
                        node.find('input:checkbox').prop('checked', false);
                        sum = 0;
                    }

                    prov = current.proveedorID;
                } else {
                    // prov = 0;
                    tblProgramacionPagos.find(`tr.prov${prov} td:eq(3)`).text(maskNumero(sum));

                    sum = 0;

                    if (!node.find('input').hasClass("desactivado") && aplicado == "0") {
                        //node.find('input').val(maskNumero(current.monto));
                        node.find('input').data("aplicado", "1");
                        node.find('input:checkbox').prop('checked', true);
                        sum += unmaskNumero(node.find('input').val());
                    }
                    else if (!node.find('input').hasClass("desactivado") && aplicado == "1") {
                        //node.find('input').val(maskNumero(0));
                        node.find('input').data("aplicado", "0");
                        node.find('input:checkbox').prop('checked', false);
                        sum = 0;
                    }

                    prov = current.proveedorID;
                }


                if (i == dtProgrPagos.rows().data().length - 1) {
                    tblProgramacionPagos.find(`tr.prov${current.proveedorID} td:eq(3)`).text(maskNumero(sum));
                }

                // console.log(node.index);
                // console.log(current.proveedorID);
                // console.log(current);
                // let sumGroup = 0;
                // //setDolarDelDia();
                // dtProgrPagos.rows().every(function (i) {
                //     let node = $(this.node())
                //         , current = this.data();
                //     if (proveedorID === current.proveedorID) {
                //         if (node.find('input:checkbox').prop('checked')) {
                //             sumGroup += unmaskNumero(node.find('input').val());
                //         }
                //     }
                // });
                // tblProgramacionPagos.find(`tr.prov${proveedorID} td:eq(3)`).text(maskNumero(sumGroup));
            });

            setSaldosTotales();
            //     let row = $(this).closest('tr')
            //     , prev = row.prev()[0]
            //     , data = dtProgrPagos.rows(prev).data()[0];

            // dtProgrPagos.rows().every(function (i) {
            //     let node = $(this.node())
            //         , current = this.data();
            //     if (data.proveedorID == current.proveedorID) {
            //         var aplicado = node.find('input').data("aplicado");

            //         if (!node.find('input').hasClass("desactivado") && aplicado == "0") {
            //             //node.find('input').val(maskNumero(current.monto));
            //             node.find('input').data("aplicado", "1");
            //             node.find('input:checkbox').prop('checked', true);
            //         }
            //         else if (!node.find('input').hasClass("desactivado") && aplicado == "1") {
            //             //node.find('input').val(maskNumero(0));
            //             node.find('input').data("aplicado", "0");
            //             node.find('input:checkbox').prop('checked', false);
            //         }
            //     }
            // });
            // setTotalProv(data);
            // setSaldosTotales();
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
            dtProgrPagos.rows().every(function (i) {
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
        init();
    }
    $(document).ready(() => {
        Administrativo.Contabilidad.Propuesta.ProgramacionPagos = new ProgramacionPagos();
    }).ajaxStart(() => {
        $.blockUI({
            baseZ: 100000,
            message: 'Procesando...'
        });
    }).ajaxStop(() => { $.unblockUI(); });
})();   