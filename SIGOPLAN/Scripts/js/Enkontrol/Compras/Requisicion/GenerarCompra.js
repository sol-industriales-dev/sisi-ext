(() => {
    $.namespace('Enkontrol.Compras.Requisicion.GenerarCompra');
    GenerarCompra = function () {
        selectCC = $('#selectCC');
        dtFin = $('#dtFin');
        txtTc = $('#txtTc');
        selLab = $('#selLab');
        tblReq = $('#tblReq');
        dtFechaCompra = $('#dtFechaCompra');
        dtInicio = $('#dtInicio');
        selMoneda = $('#selMoneda');
        btnBuscar = $('#btnBuscar');
        txtReqFin = $('#txtReqFin');
        txtReqIni = $('#txtReqIni');
        btnGuardar = $('#btnGuardar');
        txtCompEmp = $('#txtCompEmp');
        txtCompNom = $('#txtCompNom');
        tblPartidas = $('#tblPartidas');
        txtDescPartida = $('#txtDescPartida');
        txtRequisicion = $('#txtRequisicion');
        txtFolioOrigen = $('#txtFolioOrigen');
        radioBtn = $('.radioBtn a');

        isSaveTC = true;
        let init = () => {
            initForm();
            btnBuscar.click(clkBusq);
            selMoneda.change(chaMoneda);
            btnGuardar.click(clkGuardar);
            txtDescPartida.change(chaDescPartida);
        }
        /**
         * Consulta empleado comprador
         * @returns {post} empleado comprador
         * */
        const getComprador = () => { return $.post('/Enkontrol/OrdenCompra/getComprador'); };
        /** 
         * Consulta requisiciones según CC/AC
         * @returns {post} comprador
         * */
        const busqReqNum = () => { return $.post('/Enkontrol/OrdenCompra/busqReqNum', { busq: getFiltros() }); };
        /** 
         * Consulta de requisiciones
         * @returns {post} 
         * */
        const busqReq = () => { return $.post('/Enkontrol/OrdenCompra/busqReq', { busq: { cc: "'" + selectCC.val() + "'", numeroRequisicion: txtRequisicion.val() } }); };
        /**
         * Consulta de partidas
         * @param {string} cc CC/AC
         * @param {number} num Número de requisición
         * @returns {post} 
         */
        const getPartidas = (cc, num) => { return $.post('/Enkontrol/OrdenCompra/getPartidas', { cc: cc, num: num, moneda: selMoneda.val() }); };
        /**
         * Consulta si el usuario tiene permiso para agregar el tipo de cambio
         */
        const isUsuarioCambiarTC = () => { return $.post('/Enkontrol/Moneda/isUsuarioCambiarTC'); };
        /**
         * Genera las nuervas ordenes de compra
         * @param {Array[object]} lstOC Lista de ordenes
         */
        const generarOC = (lstOC) => { return $.post('/Enkontrol/OrdenCompra/generarOC', { lstOC }); };
        /**
         * Cálculo de importe
         * @param {Number} col Colocadas
         * @param {Number} precio Precio
         * @param {Number} tc Tipo de cambio
         * @returns {number} Importe
         */
        const calImporte = (col, precio, tc) => { return col * precio * tc };

        $('#tblPartidas').on('click', '.btn-cancelado', function () {
            if ($(this).attr('data-cancelado') == 'false') {
                $(this).removeClass('btn-success');
                $(this).addClass('btn-danger')
                $(this).find('i').removeClass('fa-check');
                $(this).find('i').addClass('fa-times');
                $(this).attr('data-cancelado', 'true');
            } else {
                $(this).removeClass('btn-danger');
                $(this).addClass('btn-success')
                $(this).find('i').removeClass('fa-times');
                $(this).find('i').addClass('fa-check');
                $(this).attr('data-cancelado', 'false');
            }
        });

        function aClick(esto) {
            let sel = $(esto).data('title');
            let tog = $(esto).data('toggle');
            $(`#${tog}`).prop('value', sel);
            $(`a[data-toggle="${tog}"]`).not(`[data-title="${sel}"]`).removeClass('active').addClass('notActive');
            $(`a[data-toggle="${tog}"][data-title="${sel}"]`).removeClass('notActive').addClass('active');
        }

        /**Ejecuta calculo del importe
         * @param {$(tr)} row de tblPartida
         */
        function setImporte(row) {
            col = +(row.find('.col').val()),
                cant = +(row.find('.cantidad').val()),
                pre = unmaskNumero(row.find('.precio').val()),
                camb = +(selMoneda.find(":selected").data().prefijo);
            if (col > cant || col <= 0) {
                col = cant;
                row.find('.col').val(cant);
            }
            let imp = calImporte(col, pre, camb);
            row.find('.importe').val(maskNumero(imp)).data().importa = imp;
            row.find('.precio').val(maskNumero(pre));
            let rowReq = tblReq.find('tbody > tr.active')[0];
            dtReq.row(rowReq.rowIndex - 1).data().lstPartida[row[0].rowIndex - 1].colocada = col;
            dtReq.row(rowReq.rowIndex - 1).data().lstPartida[row[0].rowIndex - 1].cant_ordenada = cant;
            dtReq.row(rowReq.rowIndex - 1).data().lstPartida[row[0].rowIndex - 1].precio = pre;
            dtReq.row(rowReq.rowIndex - 1).data().lstPartida[row[0].rowIndex - 1].tc = camb;
            dtReq.row(rowReq.rowIndex - 1).data().lstPartida[row[0].rowIndex - 1].importe = imp;
        }
        /**Calcula el importe de todas las partidas */
        function setImportes() {
            tblPartidas.find("tbody tr").each(function (idx, row) {
                setImporte($(this).closest('tr'));
            });
        }
        /**
         * Valida y genera las ordenes de compra
         */
        function clkGuardar() {
            let req = dtReq.rows().data().toArray();

            let reqFiltrados = req.filter((x) => {
                return x.lstPartida != null && x.proveedor > 0 && x.proveedor.toString().length > 0 && x.lstPartida.map((y) => {
                    return y.colocada > 0 && y.colocada <= y.cantidad_excedida_ppto && y.precio > 0 && y.tc > 0
                })
            });

            // let reqFiltrados = req.filter((x) => {
            //     return x.lstPartida != null
            // });

            // let reqFiltrados2 = reqFiltrados.filter((x) => {
            //     return x.proveedor > 0 && x.proveedor.toString().length > 0 && x.lstPartida.map((y) => {
            //         return y.colocada > 0 && y.colocada <= y.cantidad_excedida_ppto && y.precio > 0 && y.tc > 0
            //     });
            // });

            generarOC(reqFiltrados).always(response => {
                if (response.success) {
                    AlertaGeneral("Aviso", 'Orden de compra generada.');
                    // btnBuscar.click();
                    $('#mdlCapturarOrdenCompra').modal('hide');
                } else {
                    AlertaGeneral("Aviso", response.mensaje);
                }
            });
        }
        /**Cambio de select de moneda */
        function chaMoneda() {
            val = selMoneda.val(),
                tc = +(selMoneda.find(':selected').data().prefijo),
                txt = selMoneda.find(":selected").text();
            if (tc == 0 && (val != "" || val != 1)) {
                if (isSaveTC) {
                    $('#lblMoneda').val(txt).data().moneda = val;
                    $('#txtTipoCambio').val(unmaskNumero6DCompras(tc));
                    $('#mdlTC').modal('show');
                } else {
                    AlertaGeneral("Aviso", `Nó se podra generar la orden de compra hasta guardar el tipo de cambio ${txt} de hoy. Verifique.`);
                    txtTc.val(unmaskNumero6DCompras(1)).data().tc = 1;
                }
            }
            else {
                dtReq.rows().data().lstPartida.foreach(p => {
                    p.moneda = val;
                    p.tc = tc;
                });
                setImportes();
            }
        }
        /**
         * Cambia la descripción de partida del valor a la data
         */
        function chaDescPartida() {
            txtDescPartida.data().partidaDesc = txtDescPartida.val();
            let rowReq = tblReq.find('tbody > tr.active')[0],
                rowPar = tblPartidas.find('tbody > tr.active')[0];
            dtReq.row(rowReq.rowIndex - 1).data().lstPartida[rowPar.rowIndex - 1].partidaDesc = txtDescPartida.val();
        }
        /**
         * Ejecuta la busqueda de requisiciones
         */
        function clkBusq() {
            busqReq().done(response => {
                if (response.success) {
                    if (response.lstReq.length > 0) {
                        AddRows(tblReq, response.lstReq);
                        txtFolioOrigen.val(response.lstReq[0].folioOrigen);
                        setPartidas(response.lstReq[0].cc, response.lstReq[0].numero);
                    } else {
                        AlertaGeneral('Alerta', 'No se encuentra información.');
                        limpiarVista();
                    }
                } else {
                    AlertaGeneral('Alerta', 'Error al consultar la información.');
                    limpiarVista();
                }
            });
        }

        /** 
         * Muestra las partidas en la tabla tblPartidas
         * @param {String} cc centro de costos o area cuenta
         * @param {Number} numero número de requisición
         */
        function setPartidas(cc, numero) {
            getPartidas(cc, numero).done(response => {
                if (response.success)
                    if (response.lstPartida != 0) {
                        AddRows(tblPartidas, response.lstPartida);

                        let par = response.lstPartida[0];

                        txtDescPartida.val(par.partidaDesc).data({
                            numero: par.numero,
                            partida: par.partida,
                            partidaDesc: par.partidaDesc
                        });

                        tblPartidas.find('tbody > tr:eq(0)').addClass('active');

                        setStGuardar(response.lstPartida.length > 0);
                    }
            });
        }
        /**Habilita o deshabailita a btnGuardar y txtDescPartida
         * @param {boolean} isPartida */
        function setStGuardar(isPartida) {
            $('#btnGuardar, #txtDescPartida').prop("disabled", !isPartida);
        }
        /**Cambia selCC para asignar Requisición máxima y mínima */
        function chaSelCC() {
            busqReqNum()
                .done(response => {
                    if (response.success) {
                        txtReqIni.val(response.min);
                        txtReqFin.val(response.max);
                    }
                });
        }
        /**Verifica si el usuario es comprador*/
        function isComprador() {
            getComprador()
                .done(response => {
                    if (response.success) {
                        txtCompEmp.val(response.comprador.comprador);
                        txtCompNom.val(response.comprador.emplNom);
                    } else {
                        AlertaGeneral("Aviso", "No eres comprador.");
                    }
                })
                .done(response => {
                    // selCC.fillCombo('/Enkontrol/OrdenCompra/FillComboCcReqComprador', { busq: getFiltros() }, true, "Todos");
                    // convertToMultiselect(selCC);

                    selectCC.fillCombo('/Enkontrol/OrdenCompra/FillComboCcReqComprador', { busq: { numComp: txtCompEmp.val() } }, false, null);
                });
        }
        /**Obtiene formulario de filtro
         * @returns {object} Valores del formulario
         */
        function getFiltros() {
            return {
                lstCc: getValoresMultiples("#selCC"),
                ini: dtInicio.val(),
                fin: dtFin.val(),
                min: txtReqIni.val(),
                max: txtReqFin.val(),
                lab: selLab.val(),
                numComp: txtCompEmp.val(),
                nomComp: txtCompNom.val(),
                fechaOC: dtFechaCompra.val()
            }
        }
        /**Inicializa datatable de tblReq */
        function initTblReq() {
            dtReq = tblReq.DataTable({
                destroy: true,
                searching: false,
                paging: false,
                iDisplayLength: -1,
                info: false,
                sort: false,
                deferRender: true,
                scrollY: "150px",
                scrollCollapse: true,
                language: dtDicEsp,
                columns: [
                    { data: 'folio', width: '25px' },
                    { data: 'fecha', width: '25px' },
                    { data: 'tipo', width: '30px' },
                    { data: 'lab', width: '75px' },
                    { data: 'solNom', width: '90px' },
                    { data: 'authNom', width: '90px' },
                    {
                        data: 'proveedor', width: '15px', createdCell: function (td, data, rowData, row, col) {
                            $(td).html("<input>");
                            $(td).find("input").addClass('form-control prov num');
                            $(td).find("input").getAutocomplete(setAutoNumProv, null, '/Enkontrol/OrdenCompra/getProvFromNum')
                            $(td).find("input").val(data);
                        }
                    },
                    {
                        data: 'nombre', createdCell: function (td, data, rowData, row, col) {
                            $(td).html("<input>");
                            $(td).find("input").addClass('form-control prov nom');
                            $(td).find("input").getAutocomplete(setAutoNomProv, null, '/Enkontrol/OrdenCompra/getProvFromNom')
                            $(td).find("input").val(data);
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblReq.on('keydown', '.num', function (event) {
                        if (event != null)
                            limitText(event.currentTarget, 4);
                    });
                    tblReq.on('click', '.prov', function (event) {
                        if (event != null) {
                            if (dtPartidas.rows().count() > 0) {
                                let req = dtReq.row($(this).closest('tr')).data();
                                let par = dtPartidas.row(0).data();

                                if (par.cc != req.cc || par.numero != req.numero) {
                                    setPartidas(req.cc, req.numero);
                                    let $row = $(this).closest('tr');
                                    selected = $row.hasClass("active");
                                    tblReq.find("tr").removeClass("active");
                                    if (!selected)
                                        $row.not("th").addClass("active");
                                }
                            }
                        }
                    });
                }
            });
        }
        /**Inicializa datatable tabla partidas */
        function initTblPartidas() {
            dtPartidas = tblPartidas.DataTable({
                destroy: true,
                searching: false,
                paging: false,
                iDisplayLength: -1,
                info: false,
                deferRender: true,
                sort: false,
                // scrollY: "150px",
                scrollCollapse: true,
                language: dtDicEsp,
                columnDefs: [
                    { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10] }
                ],
                columns: [
                    { data: 'partida', width: '2%', sClass: 'text-right' },
                    { data: 'insumo', width: '4%' },
                    { data: 'insumoDesc' },
                    {
                        data: 'area', width: '9.5%', createdCell: function (td, data, rowData, row, col) {
                            $(td).html("<input>");
                            $(td).find("input").addClass('form-control area').prop("disabled", true);
                            $(td).find("input").val(data == 0 && rowData.cuenta == 0 ? "000-000" : `${data}-${rowData.cuenta}`);
                        }
                    },
                    {
                        data: 'unidad', createdCell: function (td, data, rowData, row, col) {
                            $(td).html("<input>");
                            $(td).find("input").addClass('form-control unidad').prop("disabled", true).val(data);
                        }
                    },
                    {
                        data: 'fecha_requerido', width: '9.5%', createdCell: function (td, data, rowData, row, col) {
                            $(td).html("<input>");
                            $(td).find("input").addClass('form-control req').prop("disabled", true);
                            $(td).find("input").datepicker().datepicker("setDate", data.parseDate());
                        }
                    },
                    {
                        data: 'cantidad', width: '10%', createdCell: function (td, data, rowData, row, col) {
                            $(td).html("<input>");
                            $(td).find("input").addClass('form-control text-center cantidad').prop("disabled", true).val(data - rowData.cant_ordenada);
                        }
                    },
                    {
                        data: 'cantidadSurtir', createdCell: function (td, data, rowData, row, col) {
                            $(td).html("<input>");
                            $(td).find("input").addClass('form-control text-center').prop("disabled", true);
                            $(td).find('input').val(data);
                        }
                    },
                    {
                        data: 'cantidadParaCompra', createdCell: function (td, data, rowData, row, col) {
                            $(td).html("<input>");
                            $(td).find("input").addClass('form-control text-center col').prop("disabled", true);
                            $(td).find('input').val(data);
                        }
                    },
                    // {
                    //     data: 'colocada', createdCell: function (td, data, rowData, row, col) {
                    //         $(td).html("<input>");
                    //         $(td).find("input").addClass('form-control col setImp');
                    //         $(td).find("input").val(rowData.cantidad - rowData.cant_ordenada);
                    //         rowData.colocada = rowData.cantidad - rowData.cant_ordenada;
                    //     }
                    // },
                    {
                        data: 'precio', createdCell: function (td, data, rowData, row, col) {
                            $(td).html("<input>");
                            $(td).find("input").addClass('form-control precio setImp').val(maskNumero(data));
                        }
                    },
                    {
                        data: 'importe', width: '12%', createdCell: function (td, data, rowData, row, col) {
                            $(td).html("<input>");
                            $(td).find("input").addClass('form-control importe text-right');
                            $(td).find("input").val(maskNumero(data)).prop("disabled", true);
                        }
                    },
                    {
                        data: 'cancelado', createdCell: function (td, data, rowData, row, col) {
                            let html = '';

                            if (data == 'A') {
                                html += '<button class="btn btn-success btn-cancelado" data-cancelado="false"><i class="fa fa-check"></i></button>';
                            } else {
                                html += '<button class="btn btn-danger btn-cancelado" data-cancelado="true"><i class="fa fa-times"></i></button>';
                            }

                            $(td).html(html);
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblPartidas.on("click", "tbody tr", function (event) {
                        if (event != null) {
                            tblPartidas.find("tbody tr").each(function (idx, row) {
                                let data = dtPartidas.row(this).data();
                                if (data.partida == txtDescPartida.data().partida)
                                    data.partidaDesc = txtDescPartida.data().partidaDesc;
                            });
                            let row = dtPartidas.row(this).data();
                            txtDescPartida.data({
                                partida: row.partida,
                                partidaDesc: row.partidaDesc
                            });
                            txtDescPartida.val(row.partidaDesc);
                            var selected = $(this).closest('tr').hasClass("active");
                            if (!selected) {
                                tblPartidas.find("tr").removeClass("active");
                                $(this).closest('tr').addClass("active");
                            }
                        }
                    });
                    tblPartidas.on('change', '.setImp', function (event) {
                        if (event != null) {
                            setImporte($(this).closest('tr'));
                        }
                    });
                    tblPartidas.on('click', '.radioBtn a', function () {
                        aClick(this);
                    });
                }
            });
        }
        /**Inicializa los formularios y tablas*/
        function initForm() {
            let hoy = new Date();
            txtReqIni.val(0);
            txtReqFin.val(0);
            dtFechaCompra.val(hoy.toLocaleDateString());
            selLab.fillCombo('/Enkontrol/Requisicion/FillComboLab', null, false, "Todos");
            selMoneda.fillCombo('/Enkontrol/Moneda/FillComboMonedaHoy', null, false, null);
            selMoneda.val(1);
            txtTc.val(unmaskNumero6DCompras(1)).data().tc = 1;
            dtInicio.datepicker().datepicker("setDate", new Date(hoy.getFullYear(), 0, 1));
            dtFin.datepicker().datepicker("setDate", new Date(hoy.getFullYear(), 11, 31));
            isComprador();
            initTblReq();
            initTblPartidas();
            setStGuardar(false);
            isUsuarioCambiarTC().done(response => {
                if (response.success)
                    isSaveTC = response.isUsuarioCambiarTC;
            });
        }
        /**
         * Callback de autocompletado. Selecciona proveddor
         * @param {event} e cambio
         * @param {Element} ui proveedor selecionado
         */
        function setAutoNumProv(e, ui) {
            let row = $(this).closest('tr');
            row.find('.num').val(ui.item.label);
            row.find('.nom').val(ui.item.id);
            tblPartidas.find('tbody>tr.active>td>input.col').select();
            dtReq.row(row[0].rowIndex - 1).data().proveedor = +(ui.item.label);
        }
        /**
         * Callback de autocompletado. Selecciona proveddor
         * @param {event} e cambio
         * @param {Element} ui proveedor selecionado
         */
        function setAutoNomProv(e, ui) {
            let row = $(this).closest('tr');
            row.find('.num').val(ui.item.id);
            row.find('.nom').val(ui.item.label);
            tblPartidas.find('tbody>tr.active>td>input.col').select();
            dtReq.row(row[0].rowIndex - 1).data().proveedor = +(ui.item.id);
        }
        /**
         * Carga datos a tabla datatable
         * @param {table} tbl tabla
         * @param {Array[object]} lst Arreglo de objeto
         */
        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            for (let i in lst)
                if (lst.hasOwnProperty(i))
                    AddRow(dt, lst[i]);
        }
        function limpiarVista() {
            // selectCC.val('');
            // txtRequisicion.val('');
            tblReq.DataTable().clear().draw();
            tblPartidas.DataTable().clear().draw();
            setStGuardar(false);
            txtDescPartida.val('');
            txtDescPartida.removeData();
        }
        /**
         * Carga objeto a tabla 
         * @param {datatable} dt tabla
         * @param {object} obj datos
         */
        function AddRow(dt, obj) {
            dt.row.add(obj).draw(false);
        }
        /**
         * Limita la longitud del texto
         * @param {input} limitField elemento a limitar
         * @param {number} limitNum longitud límite
         */
        function limitText(limitField, limitNum) {
            if (limitField.value.length > limitNum)
                limitField.value = limitField.value.substring(0, limitNum);
        }
        String.prototype.parseDate = function () { return new Date(parseInt(this.replace('/Date(', ''))); }
        Date.prototype.parseDate = function () { return this; }
        init();
    }
    $(document).ready(() => {
        Enkontrol.Compras.Requisicion.GenerarCompra = new GenerarCompra();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();