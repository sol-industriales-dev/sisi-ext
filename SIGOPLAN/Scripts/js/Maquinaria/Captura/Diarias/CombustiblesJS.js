(function () {

    $.namespace('maquinaria.captura.diaria.CapturaCombustible');

    CapturaCombustible = function () {

        mensajes = {
            NOMBRE: 'Combustibles',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        var fecha = getFecha();
        var PermisoAuditor = false;
        txtCentroCostosNombre = $("#txtCentroCostosNombre");
        url = '/Combustibles/getDataTable';
        btnGuardar = $("#btnGuardar"),
        gridResultado = $("#gridResultado"),
        txtFecha = $("#txtFecha"),
        txtCentroCostos = $("#txtCentroCostos"),
        cboTipo = $("#cboTipo"),
        cboTurno = $("#cboTurno"),
        litrosTotal = $("#litrosTotal"),
        precioTotal = $("#precioTotal"),
        precioLitro = $("#txtprecioLitro"),
        btnEditPrecio = $("#btnEditPrecio"),
        chkCostoGasolina = $("#chkCostoGasolina"),
        txtTotales = $("#txtTotales"),
        btnAsignar = $("#btnAsignar"),
        cboEconomicoPipa = $("#cboEconomicoPipa");

        function ini() {
            ValidarPermisoAuditor();
            txtCentroCostos.fillCombo('/CatObra/cboCentroCostosUsuarios', null, true);
            datePicker();
            cboTipo.fillCombo('/CatMaquina/FillCboTipo_Maquina', { estatus: true });
            cboEconomicoPipa.change(changeCbosPipa);
            btnGuardar.click(sendInfo);
            precioLitro.prop("disabled", true);
            btnEditPrecio.click(changePrecio);
            getPrecio();
            cboTurno.change(fillTableCbo);

            txtFecha.datepicker().datepicker("setDate", new Date());
            txtFecha.datepicker("setDate", -1);


            cboTipo.change(fillTable);
            txtFecha.change(fillTable);
            fillTable();
            txtCentroCostos.change(fillTable);

        }


        function ValidarPermisoAuditor() {

            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/administrador/usuarios/getUsuariosPermisos',
                success: function (response) {

                    PermisoAuditor = response.Autorizador;


                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        txtCentroCostos.keypress(function (e) {
            if (e.which == 13) {
                fillTable();
            }
        });

        function DateServerSend() {
            var dateTypeVar = txtFecha.datepicker('getDate');
            return $.datepicker.formatDate('mm-dd-yy', dateTypeVar);
        }

        initGrid();

        function fillTableCbo() {
            bootG(url, txtCentroCostos.val(), cboTurno.val());
        }

        function changePrecio() {
            if (precioLitro.is(':enabled')) {
                precioLitro.prop("disabled", true);
            }
            else {
                precioLitro.prop("disabled", false);
            }

        }

        function fillTable() {
            if (txtCentroCostos.val() != "") {
                getDataCentroCostos(txtCentroCostos.val());
                cboEconomicoPipa.fillCombo('/Combustibles/FillCboPipa', { obj: txtCentroCostos.val() });
                bootG(url, txtCentroCostos.val(), cboTurno.val());
            }

        }

        function getDataCentroCostos(obj) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/Combustibles/getCentroCostos',
                data: { obj: obj },
                success: function (response) {
                    $.unblockUI();
                    var nomb = response.centroCostos;
                    txtCentroCostosNombre.val(nomb);
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        ini();

        function changeCbosPipa() {
            $('select.itemPipa').val($(this).val());
        }

        function sendInfo() {
            var datos = [];

            var tbl = $('table#gridResultado tr').get().map(function (row) {
                return $(row).find('td').get().map(function (cell) {
                    if ($(cell).children().prop('tagName') == 'DIV') {
                        var Carga = {};
                        var volumen = $(cell).children().find('input').val();
                        var pipa = $(cell).children().find('select').val();
                        Carga.Volumen = volumen;
                        Carga.pipa = pipa;
                        return Carga;
                    }
                    if ($(cell).children().prop('tagName') == 'LABEL') {

                        var objeto = {};
                        var economico = $(cell).children().text();
                        var id = $(cell).children().attr('data-id');
                        objeto.value = economico;
                        objeto.id = id;
                        return objeto;
                    }
                });
            });
            var array = [];
            var valida = false;
            $.each(tbl, function (index, value) {
                // do your stuff here
                if (index != 0) {

                    var json = {};

                    json.Economico = value[0].value;//.replace('*', '');
                    json.id = value[0].id;
                    json.CC = txtCentroCostos.val();
                    if (value[1].Volumen == 0 || value[1].pipa == "0") {
                        json.Carga1 = 0;
                        json.Pipa1 = 0;
                    }
                    else {
                        json.Carga1 = value[1].Volumen;
                        json.Pipa1 = value[1].pipa;
                    }

                    if (value[2].Volumen == 0 || value[2].pipa == "0") {
                        json.Carga2 = 0;
                        json.Pipa2 = 0;
                    }
                    else {
                        json.Carga2 = value[2].Volumen;
                        json.Pipa2 = value[2].pipa;
                    }

                    if (value[3].Volumen == 0 || value[3].pipa == "0") {
                        json.Carga3 = 0;
                        json.Pipa3 = 0;
                    }
                    else {
                        json.Carga3 = value[3].Volumen;
                        json.Pipa3 = value[3].pipa;
                    }

                    if (value[3].Volumen == 0 || value[3].pipa == "0") {
                        json.Carga4 = 0;
                        json.Pipa4 = 0;
                    }
                    else {
                        json.Carga4 = value[4].Volumen;
                        json.Pipa4 = value[4].pipa;
                    }

                    json.turno = cboTurno.val();
                    json.volumne_carga = value[5].value;
                    json.fecha = txtFecha.val();
                    json.surtidor = "";

                    var importe = 0;

                    importe = value[6].value.split('$')[1].trim();
                    importe = importe.replace(",", "");

                    json.PrecioTotal = Number(importe);
                    json.PrecioLitro = precioLitro.val();
                    json.aplicarCosto = chkCostoGasolina.is(":checked");
                    json.FechaCaptura = getFecha();

                    if (json.Carga4 != 0 || json.Carga3 != 0 || json.Carga2 != 0 || json.Carga1 != 0) {
                        array.push(json);
                        if (value[5] == null) {
                            valida = true;
                            return;
                        }
                    }
                }
            });
            if (array.length > 0 && valida == false) {
                saveOrUpdate(array);
            }
            else {
                AlertaGeneral("Alerta", "Falta Pipa o Capturar volumen de carga", 'bg-red');
            }
        }

        function saveOrUpdate(array) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Combustibles/SaveOrUpdate_Combustible',
                type: 'POST',
                dataType: 'json',
                data: { array: array },
                success: function (response) {
                    ConfirmacionGeneral("Confirmación", response.message, "bg-green");
                    bootG(url, txtCentroCostos.val(), cboTurno.val());
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function getSumaDatos() {
            var tbl = $('table#gridResultado tr td').get().map(function (row) {
                return $(row).find('td').get().map(function (cell) {
                    if ($(cell).children().prop('tagName') == 'LABEL') {
                        return $(cell).text();
                    }
                });
            });

            var SumaTotalLitros = 0;
            var SumaTotalCostos = 0;
            var litros = $(".sumaLitros");
            var costos = $(".sumaPrecio");
            $.each(litros, function (i, e) {
                var _this = $(this);
                SumaTotalLitros += Number(_this.html());
            });
            $.each(costos, function (i, e) {
                var _this = $(this);
                var importe = 0;
                importe = _this.html().split('$')[1].trim();
                importe = importe.replace(",", "");
                SumaTotalCostos += Number(parseFloat(importe));
            });
            
            SumaTotalCostos = SumaTotalCostos.toFixed(2);
            litrosTotal.text(formatNumber.new(SumaTotalLitros));//+ "    " + formatNumber.new(SumaTotalCostos, '$'));
            precioTotal.text(formatNumber.new(SumaTotalCostos, '$'));
        }

        var ErrorExcede = "El limite de carga es: ";
        function initGrid() {

            gridResultado.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: "",
                    footer: ""
                },
                formatters: {
                    "economico": function (column, row) {
                        var indicador = "";
                        if (row.capacidadCarga == 0)
                        { indicador = "<label class='indicador'>*</lable>" }

                        return "<label data-val='" + row.Economico + "'  data-id='" + row.id + "' class='eco'>" + row.Economico + "</label>" + indicador;
                    },
                    "loadPipa": function (column, row) {


                        if (row.Carga1 > 0 && row.Carga2 > 0 && row.Carga3 > 0 && row.Carga4 > 0) {

                            return
                        } else {
                            return comboData;// cboTablaPipas; //"<select class='form-control itemPipa'></select>";
                        }

                    },
                    "editChange1": function (column, row) {
                        if (row.Carga1 > 0) {
                            return "<div class=\"col-lg-12 removePadding carga1 \" >" +
                                    "<div class=\"col-lg-6\">" +
                                    "<input type=\"number\" class=\"capacidad1 form-control\" data-total='" + row.capacidadCarga + "' min='0' value='" + row.Carga1 + "' disabled > </div>" +
                                   "<div class=\"col-lg-6 removePadding\">" +
                                    "<select class='form-control' disabled> <option value='" + row.Pipa1 + "'>" + row.Pipa1 + "</option> </select> </div>" +
                                "</div>";
                        }
                        else {
                            return "<div class=\"col-lg-12 removePadding carga1\">" +
                                   "<div class=\"col-lg-6\">" +
                                  "<input type=\"number\" class=\"capacidad1 form-control\" data-total='" + row.capacidadCarga + "' min='0' value='" + row.Carga1 + "' > </div>" +
                                  "<div class=\"col-lg-6 removePadding\">" +
                                   comboData + " </div>" +
                               "</div>";
                        }
                    },
                    "editChange2": function (column, row) {
                        if (row.Carga2 > 0) {
                            return "<div class=\"col-lg-12 removePadding carga2\">" +
                                  "<div class=\"col-lg-6\">" +
                                  "<input type=\"number\" class=\"capacidad2 form-control\" data-total='" + row.capacidadCarga + "' min='0' value='" + row.Carga2 + "' disabled> </div>" +
                                 "<div class=\"col-lg-6 removePadding\">" +
                                  "<select class='form-control' disabled> <option value='" + row.Pipa2 + "'>" + row.Pipa2 + "</option> </select> </div>" +
                              "</div>";
                            return
                        } else {
                            return "<div class=\"col-lg-12 removePadding carga2\">" +
                                "<div class=\"col-lg-6 \">" +
                               "<input type=\"number\" class=\"capacidad2 form-control\" data-total='" + row.capacidadCarga + "' min='0' value='" + row.Carga2 + "'  > </div>" +
                               "<div class=\"col-lg-6 removePadding\">" +
                                comboData + " </div>" +
                            "</div>";

                        }
                    },
                    "editChange3": function (column, row) {
                        if (row.Carga3 > 0) {
                            return "<div class=\"col-lg-12 removePadding carga2\">" +
                                 "<div class=\"col-lg-6\">" +
                                 "<input type=\"number\" class=\"capacidad3 form-control\" data-total='" + row.capacidadCarga + "' min='0'value='" + row.Carga3 + "' disabled> </div>" +
                                "<div class=\"col-lg-6 removePadding\">" +
                                 "<select class='form-control' disabled> <option value='" + row.Pipa3 + "'>" + row.Pipa3 + "</option> </select> </div>" +
                             "</div>";
                        }
                        else {
                            return "<div class=\"col-lg-12 removePadding carga3\">" +
                             "<div class=\"col-lg-6\">" +
                            "<input type=\"number\" class=\"capacidad3 form-control\" data-total='" + row.capacidadCarga + "' min='0'value='" + row.Carga3 + "'> </div>" +
                            "<div class=\"col-lg-6 removePadding\">" +
                             comboData + " </div>" +
                         "</div>";
                        }
                    },
                    "editChange4": function (column, row) {
                        if (row.Carga4 > 0) {
                            return "<div class=\"col-lg-12 removePadding carga2\">" +
                                "<div class=\"col-lg-6\">" +
                                "<input type=\"number\" class=\"capacidad3 form-control\" data-total='" + row.capacidadCarga + "' min='0'value='" + row.Carga4 + "' disabled> </div>" +
                               "<div class=\"col-lg-6 removePadding\">" +
                                "<select class='form-control' disabled> <option value='" + row.Pipa4 + "'>" + row.Pipa4 + "</option> </select> </div>" +
                            "</div>";
                        }
                        else {
                            return "<div class=\"col-lg-12 removePadding carga4\">" +
                           "<div class=\"col-lg-6\">" +
                          "<input type=\"number\" class=\"capacidad4 form-control\" data-total='" + row.capacidadCarga + "' min='0' value='" + row.Carga4 + "' ></div>" +
                          "<div class=\"col-lg-6 removePadding\">" +
                           comboData + " </div>" +
                       "</div>";
                        }
                    },
                    "totalLitros": function (column, row) {
                        return "<label class=\"sumaLitros\"> " + row.volumne_carga + " </label>";
                    },
                    "totalprecio": function (column, row) {
                        return "<label class=\"sumaPrecio\">" + row.PrecioTotal + " </label>";
                    }
                }

            }).on("loaded.rs.jquery.bootgrid", function (e) {
                /* Executes after data is loaded and rendered */

                if (PermisoAuditor) {
                    btnEditPrecio.prop('disabled', 'disabled');
                    btnGuardar.prop('disabled', 'disabled');
                    $(".capacidad1").prop('disabled', 'disabled');
                    $(".capacidad2").prop('disabled', 'disabled');
                    $(".capacidad3").prop('disabled', 'disabled');
                    $(".capacidad4").prop('disabled', 'disabled');
                    $(".itemPipa").prop('disabled', 'disabled');
                    cboEconomicoPipa.prop('disabled', 'disabled');
                }

                var rows = gridResultado.bootgrid('getCurrentRows');
                fillComboTable(rows);
                gridResultado.find('.id').parent('td').addClass('hidden');
                getSumaDatos();
                if (cboEconomicoPipa.val() != "") {
                    $('select.itemPipa').val(cboEconomicoPipa.val());
                }
                else {
                    $('select.itemPipa').val("0");
                }

                gridResultado.find(".capacidad1").on("change", function (e) {
                    var capacidadTanque = $(this).attr('data-total');
                    var carga = $(this).val();
                    var capacidad1 = $(this).parents('tr').find(".capacidad1").val() == "" ? 0 : $(this).parents('tr').find(".capacidad1").val();
                    var capacidad2 = $(this).parents('tr').find(".capacidad2").val() == "" ? 0 : $(this).parents('tr').find(".capacidad2").val();
                    var capacidad3 = $(this).parents('tr').find(".capacidad3").val() == "" ? 0 : $(this).parents('tr').find(".capacidad3").val();
                    var capacidad4 = $(this).parents('tr').find(".capacidad4").val() == "" ? 0 : $(this).parents('tr').find(".capacidad4").val();
                    var Total = parseFloat(capacidad1) + parseFloat(capacidad2) + parseFloat(capacidad3) + parseFloat(capacidad4);


                    if (parseFloat(carga) <= parseFloat(capacidadTanque) || capacidadTanque == 0) {

                        var resultado = parseFloat(capacidad1) + parseFloat(capacidad2) + parseFloat(capacidad3) + parseFloat(capacidad4);
                        var resultadoPrecio = parseFloat(resultado) * parseFloat(precioLitro.val());
                        $(this).parents('tr').find(".sumaLitros").text(resultado);
                        $(this).parents('tr').find(".sumaPrecio").text("$" + resultadoPrecio.toFixed(2));
                        getSumaDatos();

                    }
                    else {
                        $(this).val(0);
                        AlertaGeneral("Alerta", ErrorExcede + capacidadTanque);
                        var capacidad1 = $(this).parents('tr').find(".capacidad1").val() == "" ? 0 : $(this).parents('tr').find(".capacidad1").val();
                        var capacidad2 = $(this).parents('tr').find(".capacidad2").val() == "" ? 0 : $(this).parents('tr').find(".capacidad2").val();
                        var capacidad3 = $(this).parents('tr').find(".capacidad3").val() == "" ? 0 : $(this).parents('tr').find(".capacidad3").val();
                        var capacidad4 = $(this).parents('tr').find(".capacidad4").val() == "" ? 0 : $(this).parents('tr').find(".capacidad4").val();

                        var resultado = parseFloat(capacidad1) + parseFloat(capacidad2) + parseFloat(capacidad3) + parseFloat(capacidad4);
                        var resultadoPrecio = parseFloat(resultado) * parseFloat(precioLitro.val());
                        $(this).parents('tr').find(".sumaLitros").text(resultado);
                        $(this).parents('tr').find(".sumaPrecio").text(formatNumber.new(resultadoPrecio.toFixed(2), '$'));
                        getSumaDatos();
                    }
                });
                gridResultado.find(".capacidad2").on("change", function (e) {
                    var capacidadTanque = $(this).attr('data-total');
                    var carga = $(this).val();
                    var capacidad1 = $(this).parents('tr').find(".capacidad1").val() == "" ? 0 : $(this).parents('tr').find(".capacidad1").val();
                    var capacidad2 = $(this).parents('tr').find(".capacidad2").val() == "" ? 0 : $(this).parents('tr').find(".capacidad2").val();
                    var capacidad3 = $(this).parents('tr').find(".capacidad3").val() == "" ? 0 : $(this).parents('tr').find(".capacidad3").val();
                    var capacidad4 = $(this).parents('tr').find(".capacidad4").val() == "" ? 0 : $(this).parents('tr').find(".capacidad4").val();
                    var Total = parseFloat(capacidad1) + parseFloat(capacidad2) + parseFloat(capacidad3) + parseFloat(capacidad4);

                    if (parseFloat(carga) <= parseFloat(capacidadTanque) || capacidadTanque == 0) {

                        var resultado = parseFloat(capacidad1) + parseFloat(capacidad2) + parseFloat(capacidad3) + parseFloat(capacidad4);
                        var resultadoPrecio = parseFloat(resultado) * parseFloat(precioLitro.val());
                        $(this).parents('tr').find(".sumaLitros").text(resultado);
                        $(this).parents('tr').find(".sumaPrecio").text(formatNumber.new(resultadoPrecio.toFixed(2), '$'));
                        getSumaDatos();

                    }
                    else {
                        $(this).val(0);
                        AlertaGeneral("Alerta", ErrorExcede + capacidadTanque);
                        var capacidad1 = $(this).parents('tr').find(".capacidad1").val() == "" ? 0 : $(this).parents('tr').find(".capacidad1").val();
                        var capacidad2 = $(this).parents('tr').find(".capacidad2").val() == "" ? 0 : $(this).parents('tr').find(".capacidad2").val();
                        var capacidad3 = $(this).parents('tr').find(".capacidad3").val() == "" ? 0 : $(this).parents('tr').find(".capacidad3").val();
                        var capacidad4 = $(this).parents('tr').find(".capacidad4").val() == "" ? 0 : $(this).parents('tr').find(".capacidad4").val();

                        var resultado = parseFloat(capacidad1) + parseFloat(capacidad2) + parseFloat(capacidad3) + parseFloat(capacidad4);
                        var resultadoPrecio = parseFloat(resultado) * parseFloat(precioLitro.val());
                        $(this).parents('tr').find(".sumaLitros").text(resultado);
                        $(this).parents('tr').find(".sumaPrecio").text(formatNumber.new(resultadoPrecio.toFixed(2), '$'));
                        getSumaDatos();
                    }
                });
                gridResultado.find(".capacidad3").on("change", function (e) {
                    var capacidadTanque = $(this).attr('data-total');
                    var carga = $(this).val();
                    var capacidad1 = $(this).parents('tr').find(".capacidad1").val() == "" ? 0 : $(this).parents('tr').find(".capacidad1").val();
                    var capacidad2 = $(this).parents('tr').find(".capacidad2").val() == "" ? 0 : $(this).parents('tr').find(".capacidad2").val();
                    var capacidad3 = $(this).parents('tr').find(".capacidad3").val() == "" ? 0 : $(this).parents('tr').find(".capacidad3").val();
                    var capacidad4 = $(this).parents('tr').find(".capacidad4").val() == "" ? 0 : $(this).parents('tr').find(".capacidad4").val();
                    var Total = parseFloat(capacidad1) + parseFloat(capacidad2) + parseFloat(capacidad3) + parseFloat(capacidad4);

                    if (parseFloat(carga) <= parseFloat(capacidadTanque) || capacidadTanque == 0) {

                        var resultado = parseFloat(capacidad1) + parseFloat(capacidad2) + parseFloat(capacidad3) + parseFloat(capacidad4);
                        var resultadoPrecio = parseFloat(resultado) * parseFloat(precioLitro.val());
                        $(this).parents('tr').find(".sumaLitros").text(resultado);
                        $(this).parents('tr').find(".sumaPrecio").text(formatNumber.new(resultadoPrecio.toFixed(2), '$'));
                        getSumaDatos();

                    }
                    else {
                        $(this).val(0);
                        AlertaGeneral("Alerta", ErrorExcede + capacidadTanque);
                        var capacidad1 = $(this).parents('tr').find(".capacidad1").val() == "" ? 0 : $(this).parents('tr').find(".capacidad1").val();
                        var capacidad2 = $(this).parents('tr').find(".capacidad2").val() == "" ? 0 : $(this).parents('tr').find(".capacidad2").val();
                        var capacidad3 = $(this).parents('tr').find(".capacidad3").val() == "" ? 0 : $(this).parents('tr').find(".capacidad3").val();
                        var capacidad4 = $(this).parents('tr').find(".capacidad4").val() == "" ? 0 : $(this).parents('tr').find(".capacidad4").val();

                        var resultado = parseFloat(capacidad1) + parseFloat(capacidad2) + parseFloat(capacidad3) + parseFloat(capacidad4);
                        var resultadoPrecio = parseFloat(resultado) * parseFloat(precioLitro.val());
                        $(this).parents('tr').find(".sumaLitros").text(resultado);
                        $(this).parents('tr').find(".sumaPrecio").text(formatNumber.new(resultadoPrecio.toFixed(2), '$'));
                        getSumaDatos();
                    }
                });
                gridResultado.find(".capacidad4").on("change", function (e) {
                    var capacidadTanque = $(this).attr('data-total');
                    var carga = $(this).val();
                    var capacidad1 = $(this).parents('tr').find(".capacidad1").val() == "" ? 0 : $(this).parents('tr').find(".capacidad1").val();
                    var capacidad2 = $(this).parents('tr').find(".capacidad2").val() == "" ? 0 : $(this).parents('tr').find(".capacidad2").val();
                    var capacidad3 = $(this).parents('tr').find(".capacidad3").val() == "" ? 0 : $(this).parents('tr').find(".capacidad3").val();
                    var capacidad4 = $(this).parents('tr').find(".capacidad4").val() == "" ? 0 : $(this).parents('tr').find(".capacidad4").val();
                    var Total = parseFloat(capacidad1) + parseFloat(capacidad2) + parseFloat(capacidad3) + parseFloat(capacidad4);

                    if (parseFloat(carga) <= parseFloat(capacidadTanque) || capacidadTanque == 0) {

                        var resultado = parseFloat(capacidad1) + parseFloat(capacidad2) + parseFloat(capacidad3) + parseFloat(capacidad4);
                        var resultadoPrecio = parseFloat(resultado) * parseFloat(precioLitro.val());
                        $(this).parents('tr').find(".sumaLitros").text(resultado);
                        $(this).parents('tr').find(".sumaPrecio").text(formatNumber.new(resultadoPrecio.toFixed(2), '$'));
                        getSumaDatos();
                    }
                    else {
                        AlertaGeneral("Alerta", ErrorExcede + capacidadTanque);
                        $(this).val(0);
                        var capacidad1 = $(this).parents('tr').find(".capacidad1").val() == "" ? 0 : $(this).parents('tr').find(".capacidad1").val();
                        var capacidad2 = $(this).parents('tr').find(".capacidad2").val() == "" ? 0 : $(this).parents('tr').find(".capacidad2").val();
                        var capacidad3 = $(this).parents('tr').find(".capacidad3").val() == "" ? 0 : $(this).parents('tr').find(".capacidad3").val();
                        var capacidad4 = $(this).parents('tr').find(".capacidad4").val() == "" ? 0 : $(this).parents('tr').find(".capacidad4").val();

                        var resultado = parseFloat(capacidad1) + parseFloat(capacidad2) + parseFloat(capacidad3) + parseFloat(capacidad4);
                        var resultadoPrecio = parseFloat(resultado) * parseFloat(precioLitro.val());
                        $(this).parents('tr').find(".sumaLitros").text(resultado);
                        $(this).parents('tr').find(".sumaPrecio").text(formatNumber.new(resultadoPrecio.toFixed(2), '$'));
                        getSumaDatos();
                    }
                });
            });


        }

        var comboData = "";
        function bootG(url, obj, turno) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: url,
                data: { obj: obj, turno: turno, fecha: txtFecha.val(), idTipo: cboTipo.val() == "" ? 0 : cboTipo.val() },
                success: function (response) {
                    $.unblockUI();
                    var data = response.economico;
                    gridResultado.bootgrid("clear");
                    comboData = response.combo;
                    gridResultado.bootgrid("append", data);
                    gridResultado.addClass('fix-tableGrid');
                    var columna = (gridResultado.find('th')[8]);

                    $(columna).addClass('hidden');
                    txtTotales.removeClass('hidden');

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function fillComboTable(data) {
            if (data.length > 0) {
                $("#gridResultado tbody tr").each(function (index) {
                    if (data[index].id != 0) {
                        $(this).find('select.itemPipa').val(data[index].surtidor);
                    } else {
                        $("#gridResultado").find('select.itemPipa').val('0');
                    }

                });
            }
        }

        var cboTablaPipas;

        function getCombos(obj) {

            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/Combustibles/tablaCboPipa',
                data: { obj: obj },
                success: function (response) {
                    $.unblockUI();
                    cboTablaPipas = response.table;
                    return cboTablaPipas;

                },
                error: function () {
                    $.unblockUI();
                }

            });



        }

        function getPrecio() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/Combustibles/getPrecioDiesel',
                data: { obj: 1 },
                success: function (response) {
                    $.unblockUI();
                    var precio = response.Precio;
                    precioLitro.val(precio);

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        var formatNumber = {
            separador: ",", // separador para los miles
            sepDecimal: '.', // separador para los decimales
            formatear: function (num) {
                num += '';
                var splitStr = num.split('.');
                var splitLeft = splitStr[0];
                var splitRight = splitStr.length > 1 ? this.sepDecimal + splitStr[1] : '';
                var regx = /(\d+)(\d{3})/;
                while (regx.test(splitLeft)) {
                    splitLeft = splitLeft.replace(regx, '$1' + this.separador + '$2');
                }
                return this.simbol + splitLeft + splitRight;
            },
            new: function (num, simbol) {
                this.simbol = simbol || '';
                return this.formatear(num);
            }
        }

        function datePicker() {
            var now = new Date(),
            year = now.getYear() + 1900;
            mes = now.getMonth();
            day = now.getDate();
            $.datepicker.setDefaults($.datepicker.regional["es"]);
            var dateFormat = "dd/mm/yy",
              from = $("#txtFecha")
                .datepicker({

                    changeMonth: true,
                    changeYear: true,
                    numberOfMonths: 1,
                    defaultDate: new Date(year, 00, 01),
                    maxDate: new Date(year, mes, day),
                    //minDate: new Date(year, mes, day - 5),
                    onSelect: function () {
                        $(this).trigger('change');
                    }
                })
                .on("change", function () {

                    //var date = $(this).val();
                    //var array = new Array();
                    //array = date.split('/');

                    //$(this).datepicker('setDate', new Date(array[2], array[1] - 1, 1));
                    //fechaFin.datepicker('setDate', new Date(array[2], array[1], 0));
                    //$(this).blur();

                });

            function getDate(element) {
                var date;
                try {
                    date = $.datepicker.parseDate(dateFormat, element.value);
                } catch (error) {
                    date = null;
                }

                return date;
            }
        }
    }
    $(document).ready(function () {
        maquinaria.captura.diaria.CapturaCombustible = new CapturaCombustible();
    });
})();