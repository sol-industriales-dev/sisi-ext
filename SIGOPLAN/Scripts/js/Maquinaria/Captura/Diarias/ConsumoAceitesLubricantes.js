(function () {
    $.namespace('maquinaria.Capturas.Diarias.ConsumoAceitesLubricantes');
    ConsumoAceitesLubricantes = function () {
        mensajes = { PROCESANDO: 'Procesando...' };



        PermisoAuditor = false;
        btnReporteLubricantes = $("#btnReporteLubricantes"),
            modalLubricantes = $("#modalLubricantes"),
            cboTiposLubricantes = $("#cboTiposLubricantes"),
            dpBusqFecha = $("#dpBusqFecha");
        cboBusqCC = $("#cboBusqCC");
        txtCCNombre = $("#txtCCNombre");
        cboBusqTurno = $("#cboBusqTurno");
        cboBusqTipo = $("#cboBusqTipo");
        cboBusqOrquesta = $("#cboBusqOrquesta");
        tblConsumo = $("#tblConsumo");
        divTblConsumo = $("#divTblConsumo");
        btnGaurdar = $("#btnGaurdar");
        const btnExistencia = $('#btnExistencia');
        const mdlExistenciaDetalle = $('#mdlExistenciaDetalle');
        const tblExistencias = $('#tblExistencias');

        //btnImprimir = $("#btnImprimir");
        ireport = $("#report");
        var selectCboGridPipas;

        function init() {
            ValidarPermisoAuditor();
            initElementos();
            initTabla();
            initExistencias();
            btnGaurdar.click(SaveMaqAceiteLubricante);
            //btnImprimir.click(verReporte);
            //cboBusqCC.change(setCC);
            cboBusqCC.change(tblConsumoMaqAceiteLubricante);
            cboBusqTipo.change(tblConsumoMaqAceiteLubricante);
            dpBusqFecha.change(tblConsumoMaqAceiteLubricante);
            cboBusqTurno.change(tblConsumoMaqAceiteLubricante);
            cboBusqOrquesta.change(changeCbosPipa);
            btnExistencia.attr("disabled", true);

        }

        function ValidarPermisoAuditor() {

            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/administrador/usuarios/getUsuariosPermisos',
                success: function (response) {

                    PermisoAuditor = response.Autorizador;
                    if (PermisoAuditor) {
                        btnGaurdar.prop('disabled', true);
                        cboBusqOrquesta.prop('disabled', true);
                    }

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        $("#cboBusqCC").change(function (e) {
            btnExistencia.attr("disabled", false);
        });

        btnExistencia.click(function (e) {
            let almacen = $("#cboBusqCC").val();
            let numeroAlmacen;

            //Si son centros de costo de Perú se coloca el almacén 01.
            if ($('#cboBusqCC').find('option:selected').attr('data-prefijo') == 'CPA' || $('#cboBusqCC').find('option:selected').attr('data-prefijo') == 'FVT') {
                numeroAlmacen = "01";
                mdlExistenciaDetalle.modal('show');
            } else {
                switch (almacen) {
                    case "1-9":
                        numeroAlmacen = "602";
                        mdlExistenciaDetalle.modal('show');

                        break;
                    case "1-10":
                        numeroAlmacen = "603";
                        mdlExistenciaDetalle.modal('show');

                        break;
                    case "2-1":
                        numeroAlmacen = "603";
                        mdlExistenciaDetalle.modal('show');

                        break;
                    case "3-1":
                        numeroAlmacen = "604";
                        mdlExistenciaDetalle.modal('show');

                        break;
                    case "5-4":
                        numeroAlmacen = "605";
                        mdlExistenciaDetalle.modal('show');

                        break;
                    case "5-6":
                        numeroAlmacen = "607";
                        mdlExistenciaDetalle.modal('show');

                        break;
                    case "5-7":
                        numeroAlmacen = "608";
                        mdlExistenciaDetalle.modal('show');

                        break;
                    case "7-15":
                        numeroAlmacen = "609";
                        mdlExistenciaDetalle.modal('show');

                        break;
                    case "17-1":
                        numeroAlmacen = "610";
                        mdlExistenciaDetalle.modal('show');

                        break;
                    case "18-1":
                        numeroAlmacen = "610";
                        mdlExistenciaDetalle.modal('show');

                        break;
                    default:
                        alert("No se encontro almacen");
                        mdlExistenciaDetalle.modal('hide');
                        break;
                }
            }

            getExistenciaDetalle(numeroAlmacen);
        });

        function getExistenciaDetalle(almacen) {
            $.ajax({
                url: "/AceitesLubricantes/ExistenciaLubricante",
                datatype: "json",
                type: "POST",
                async: false,
                data: {
                    almacen: almacen
                },
                success: function (response) {
                    AddRows(tblExistencias, response)
                }
            });
        }

        function initExistencias() {
            dtExistencias = tblExistencias.DataTable({
                language: dtDicEsp,
                destroy: true,
                ordering: false,
                paging: true,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'almacen', title: 'almacen' },
                    { data: 'insumo', title: 'insumo' },
                    { data: 'descripcion', title: 'descripcion' },
                    { data: 'existencia', title: 'existencias' },
                ],
            });
        }
        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }

        function initElementos() {
            dpBusqFecha.datepicker().datepicker("setDate", new Date());
            cboBusqCC.fillCombo('/CatObra/cboCentroCostosUsuarios', null, false, null);
            cboBusqTipo.fillCombo('/CatMaquina/FillCboTipo_Maquina', { estatus: true }, true);
            cboBusqOrquesta.fillCombo('/AceitesLubricantes/FillDlOrquestas', null, true, null);

        }
        function changeCbosPipa() {
            $('select.cboAddConsumo').val($(this).val());
            GETDATA();
        }

        function initTabla() {
            tblConsumo.bootgrid({
                rowCount: -1,
                templates: {
                    header: "",
                    footer: ""
                },
                formatters: {

                    "ECONOMICO": function (column, row) {
                        var html = '<div hidden><input class="hddnAddid" value="' + row.id + '" /><input class="hddnAddturno" value="' + row.Turno + '" /><input class="hddnAddfecha" value="' + row.Fecha + '" /><input class="hddnAddCC" value="' + row.CC + '" /></div>';
                        return html + '<label class="txtAddEconomico">' + row.Economico + '</label>';
                    },
                    "CONSUMO": function (column, row) {
                        var desabilitado = row.id == 0 ? "" : "disabled";

                        return $('<div>').append(selectCboGridPipas).html();
                    },
                    "ANTIFREEZE": function (column, row) {
                        var desabilitado = row.id == 0 ? "" : "disabled";

                        var desabilitado = (row.id != 0 && row.Antifreeze != "0") ? "disabled" : "";
                        if (row.Antifreeze != "0") {
                            desabilitado = "disabled";
                        }

                        var html = '<input type="number" class="txtAddAntiFreeze" style="width: 100%; height: 50px;" value="' + row.Antifreeze + '" ' + desabilitado + ' />'
                        return html;
                    },
                    "MOTOR": function (column, row) {

                        var desabilitado = (row.id != 0 && row.MotorVal != "0") ? "disabled" : "";
                        if (row.CboMotor == "") {
                            desabilitado = "disabled";
                        }
                        var leer = "";
                        var html = '<input type="number" class="txtAddMotor" style="width: 100%;" value="' + row.MotorVal + '" ' + desabilitado + ' ' + leer + ' />'


                        return $('<div>').append(row.CboMotor).html() + html;

                    },
                    "TRANSMISION": function (column, row) {

                        var desabilitado = (row.id != 0 && row.TransmisionVal != "0") ? "disabled" : "";
                        if (row.CboTransmision == "") {
                            desabilitado = "disabled";
                        }
                        var leer = "";
                        var html = '<input type="number" class="txtAddTransmision" style="width: 100%;" value="' + row.TransmisionVal + '" ' + desabilitado + ' ' + leer + ' />'

                        return $('<div>').append(row.CboTransmision).html() + html;

                    },
                    "HIDRAULICO": function (column, row) {

                        var desabilitado = (row.id != 0 && row.HidraulicoVal != "0") ? "disabled" : "";
                        if (row.CboHidraulico == "") {
                            desabilitado = "disabled";
                        }
                        var leer = "";
                        var html = '<input type="number" class="txtAddHidraulico" style="width: 100%;" value="' + row.HidraulicoVal + '" ' + desabilitado + ' ' + leer + ' />'
                        return $('<div>').append(row.CboHidraulico).html() + html;

                    },
                    "DIFERENCIAL": function (column, row) {
                        var desabilitado = (row.id != 0 && row.DiferencialVal != "0") ? "disabled" : "";
                        if (row.CboDiferencial == "") {
                            desabilitado = "disabled";
                        }
                        var leer = "";
                        var html = '<input type="number" class="txtAddDiferencial" style="width: 100%;" value="' + row.DiferencialVal + '" ' + desabilitado + ' ' + leer + ' />'
                        return $('<div>').append(row.CboDiferencial).html() + html;
                    },
                    "MFT": function (column, row) {
                        var desabilitado = (row.id != 0 && row.MFTIzqVal != "0") ? "disabled" : "";
                        if (row.CboMandoFinal == "") {
                            desabilitado = "disabled";
                        }
                        var leer = "";
                        var html = '<input type="number" class="txtAddMFTIzq" style="width: 100%;" value="' + row.MFTIzqVal + '" ' + desabilitado + ' ' + leer + ' />'
                        return $('<div>').append(row.CboMandoFinal).html() + html;

                    },
                    "MD": function (column, row) {
                        var desabilitado = (row.id != 0 && row.MDIzqVal != "0") ? "disabled" : "";
                        if (row.CboMandoFinal == "") {
                            desabilitado = "disabled";
                        }
                        var leer = "";
                        var html = '<input type="number" class="txtAddMDIzq" style="width: 50%;" value="' + row.MDIzqVal + '" ' + desabilitado + ' ' + leer + ' />'
                        return $('<div>').append(row.CboMandoFinal).html() + html;
                    },
                    "DIR": function (column, row) {
                        var desabilitado = (row.id != 0 && row.DirVal != "0") ? "disabled" : "";
                        if (row.CboDireccion == "") {
                            desabilitado = "disabled";
                        }
                        var leer = "";
                        var html = '<input type="number" class="txtAddDir" style="width: 100%;" value="' + row.DirVal + '" ' + desabilitado + ' ' + leer + ' />';
                        return $('<div>').append(row.CboDireccion).html() + html;
                    },
                    "GRASA": function (column, row) {
                        var desabilitado = (row.id != 0 && row.DirVal != "0") ? "disabled" : "";

                        if (row.CboGrasa == "") {
                            desabilitado = "disabled";
                        }

                        var html = '<input type="number" class="txtAddGrasa" style="width: 100%;" value="' + row.Grasa + '" ' + desabilitado + '>';
                        return $('<div>').append(row.CboGrasa).html() + html;
                    },
                    "OTROS1": function (column, row) {
                        var desabilitado = row.otros1 == 0 ? "" : "disabled";
                        if (row.CboOtros1 == "") {
                            desabilitado = "disabled";
                        }
                        var leer = "";
                        var html = '<input type="number" class="tbOtros1" style="width: 100%;" value="' + row.otros1 + '" ' + desabilitado + ' ' + leer + ' />';
                        return $('<div>').append(row.CboOtros1).html() + html;

                    },
                    "OTROS2": function (column, row) {
                        var desabilitado = row.otros2 == 0 ? "" : "disabled";
                        if (row.CboOtros2 == "") {
                            desabilitado = "disabled";
                        }
                        var leer = "";
                        var html = '<input type="number" class="tbOtros2" style="width: 100%;" value="' + row.otros2 + '" ' + desabilitado + ' ' + leer + ' />';
                        return $('<div>').append(row.CboOtros2).html() + html;

                    },
                    "OTROS3": function (column, row) {
                        var desabilitado = row.otros3 == 0 ? "" : "disabled";
                        if (row.CboOtros3 == "") {
                            desabilitado = "disabled";
                        } var leer = "";
                        var html = '<input type="number" class="tbOtros3" style="width: 100%;" value="' + row.otros3 + '" ' + desabilitado + ' ' + leer + ' />';
                        return $('<div>').append(row.CboOtros3).html() + html;

                    },
                    "OTROS4": function (column, row) {
                        var desabilitado = row.otros4 == 0 ? "" : "disabled";
                        if (row.CboOtros4 == "") {
                            desabilitado = "disabled";
                        } var leer = "";
                        var html = '<input type="number" class="tbOtros4" style="width: 100%;" value="' + row.otros4 + '" ' + desabilitado + ' ' + leer + ' />';
                        return $('<div>').append(row.CboOtros4).html() + html;

                    },
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                var rows = tblConsumo.bootgrid('getCurrentRows');
                fillComboTable(rows);
                if (PermisoAuditor) {
                    btnGaurdar.prop('disabled', 'disabled');
                    $('table').children().find('input').prop('disabled', true);
                    $('table').children().find('select').prop('disabled', true);
                    cboEconomicoPipa.prop('disabled', 'disabled');
                }
            });
        }

        function fillComboTable(data) {
            if (data.length > 0) {
                $("#tblConsumo tbody tr").each(function (index) {
                    if (data[index].MotorVal != 0) {
                        $(this).find('select.cboAddMotor').val(data[index].MotorId).prop('disabled', true);
                    } else {
                        $(this).find('select.cboAddMotor').prop('disabled', false);

                    }

                    if (data[index].DiferencialVal != 0) {
                        $(this).find('select.cboAddDiferencial').val(data[index].DiferencialId).prop('disabled', true);
                    } else {
                        $(this).find('select.cboAddDiferencial').prop('disabled', false);
                    }

                    if (data[index].HidraulicoVal != 0) {
                        $(this).find('select.cboAddHidraulico').val(data[index].HidraulicoID).prop('disabled', true);
                    } else {
                        $(this).find('select.cboAddHidraulico').prop('disabled', false);
                    }

                    if (data[index].TransmisionVal != 0) {
                        //$(this).find('select.CboTransmision').val(data[index].TransmisionID).prop('disabled', true); bug raguilar 27/12/19 
                        $(this).find('select.cboAddTransmision').val(data[index].TransmisionID).prop('disabled', true);
                    } else {
                        //$(this).find('select.CboTransmision').prop('disabled', false);
                        $(this).find('select.cboAddTransmision').prop('disabled', false);
                    }

                    if (data[index].MFTIzqVal != 0) {
                        //$(this).find('select.CboMandoFinal').val(data[index].MFTIzqId).prop('disabled', true);
                        $(this).find('select.cboAddMFTIzq').val(data[index].MFTIzqId).prop('disabled', true);
                    } else {
                        //$(this).find('select.CboMandoFinal').prop('disabled', false);
                        $(this).find('select.cboAddMFTIzq').prop('disabled', false);
                    }

                    if (data[index].otros1 != 0) {
                        $(this).find('select.CboOtros1').val(data[index].otroId1).prop('disabled', true);
                    } else {
                        $(this).find('select.CboOtros1').prop('disabled', false);
                    }

                    if (data[index].otros2 != 0) {
                        $(this).find('select.CboOtros2').val(data[index].otroId2).prop('disabled', true);
                    } else {
                        $(this).find('select.CboOtros2').prop('disabled', false);
                    }

                    if (data[index].otros3 != 0) {
                        $(this).find('select.CboOtros3').val(data[index].otroId3).prop('disabled', true);
                    } else {
                        $(this).find('select.CboOtros3').prop('disabled', false);

                    }
                    if (data[index].otros4 != 0) {
                        $(this).find('select.CboOtros4').val(data[index].otroId4).prop('disabled', true);
                    } else {
                        $(this).find('select.CboOtros4').prop('disabled', false);
                    }

                    $(this).find('select.cboAddConsumo').val(data[index].Firma);

                });
            }
        }
        function setCC() {
            var cc = $("#cboBusqCC option:selected").text()
            if (cc == "--Seleccione--") {
                txtCCNombre.val("");
            }
            else {
                txtCCNombre.val(cboBusqCC.val());
            }
        }

        function getConsumoFromTbl() {
            var Arr = [];
            tblConsumo.find("tr").each(function (idx, row) {
                if (idx >= 1) {
                    var JsonData = {};
                    var bandera = false;
                    JsonData.Id = $(this).find("td:eq(0) input.hddnAddid", row).val();
                    JsonData.Turno = $(this).find("td:eq(0) input.hddnAddturno", row).val();
                    JsonData.Fecha = dpBusqFecha.val();//new Date(+($(this).find("td:eq(0) input.hddnAddfecha", row).val().replace(/^\D+|\D+$/g, ""))).toLocaleDateString();
                    JsonData.CC = $(this).find("td:eq(0) input.hddnAddCC", row).val();
                    JsonData.Economico = $(this).find("td:eq(0) label.txtAddEconomico", row).text();
                    JsonData.Horometro = $(this).find("td:eq(1) label.txtAddHorometro", row).text();
                    JsonData.Firma = $(this).find(".cboAddConsumo", row).val();
                    JsonData.Antifreeze = $(this).find(".txtAddAntiFreeze", row).val();

                    JsonData.MotorId = $(this).find(".cboAddMotor").val();
                    JsonData.MotorVal = $(this).find(".txtAddMotor").val();


                    if (JsonData.Antifreeze != "0") {
                        bandera = true;
                    }

                    if (JsonData.MotorVal != "0") {

                        bandera = true;

                    }

                    JsonData.TransmisionID = $(this).find(".cboAddTransmision").val();
                    JsonData.TransmisionVal = $(this).find(".txtAddTransmision").val();

                    if (JsonData.TransmisionVal != "0") {
                        bandera = true;
                    }

                    JsonData.HidraulicoID = $(this).find(".cboAddHidraulico").val();
                    JsonData.HidraulicoVal = $(this).find(".txtAddHidraulico").val();

                    if (JsonData.HidraulicoVal != "0") {
                        bandera = true;
                    }

                    JsonData.DiferencialId = $(this).find(".cboAddDiferencial").val();
                    JsonData.DiferencialVal = $(this).find(".txtAddDiferencial").val();

                    if (JsonData.DiferencialVal != "0") {
                        bandera = true;
                    }

                    JsonData.MFTIzqId = $(this).find(".cboAddMFTIzq").val();
                    JsonData.MFTIzqVal = $(this).find(".txtAddMFTIzq").val();
                    if (JsonData.MFTIzqVal != "0") {
                        bandera = true;

                    }

                    JsonData.DirVal = $(this).find(".txtAddDir").val();
                    JsonData.DirId = $(this).find(".cboAddDir").val();

                    if (JsonData.DirVal != "0") {
                        bandera = true;
                    }

                    JsonData.DirVal = $(this).find(".txtAddDir").val();
                    JsonData.DirId = $(this).find(".cboAddDir").val();

                    if (JsonData.DirVal != "0") {
                        bandera = true;
                    }

                    JsonData.GrasaVal = $(this).find(".txtAddGrasa").val();
                    JsonData.GrasaId = $(this).find(".cboAddGrasa").val();

                    if (JsonData.GrasaVal != "0") {
                        bandera = true;
                    }

                    JsonData.otroId1 = $(this).find(".CboOtros1").val();
                    JsonData.otros1 = $(this).find(".tbOtros1").val();

                    if (JsonData.otros1 != "0") {

                        bandera = true;

                    }

                    JsonData.otroId2 = $(this).find(".CboOtros2").val();
                    JsonData.otros2 = $(this).find(".tbOtros2").val();

                    if (JsonData.otros2 != "0") {
                        bandera = true;
                    }
                    JsonData.otroId3 = $(this).find(".CboOtros3").val();
                    JsonData.otros3 = $(this).find(".tbOtros3").val();

                    if (JsonData.otros3 != "0") {
                        bandera = true;
                    }
                    JsonData.otroId4 = $(this).find(".CboOtros4").val();
                    JsonData.otros4 = $(this).find(".tbOtros4").val();

                    if (JsonData.otros4 != "0") {
                        bandera = true;
                    }
                    JsonData.MFTDerId = 0;//$(this).find(".cboAddMFTDer").val(); //No se utilizan
                    JsonData.MFTDerVal = 0; //$(this).find(".txtAddMFTDer").val(); //No se utilizan
                    JsonData.MDIzqID = 0;// $(this).find("td:eq(13) select.cboAddMDIzq", row).val(); //No se utilizan
                    JsonData.MDDerID = 0;//$(this).find(".cboAddMDDer").val(); //No se utilizan
                    JsonData.MDIzqVal = 0; //$(this).find(".txtAddMDIzq").val(); //No se utilizan
                    JsonData.MDDerVal = 0;//$(this).find(".txtAddMDDer").val();//No se utilizan
                    JsonData.Grasa = 0;// $(this).find(".txtAddGrasa").val();

                    if (bandera) {
                        Arr.push(JsonData);
                    }
                }
            });
            return Arr;
        }



        function validaObj(obj) {
            var bandera = false;
            if (obj.Rotacion || obj.Sopleteo || obj.Ak || obj.Lubricacion) bandera = true;
            if (validaPareja(obj.MotorId, obj.MotorVal)) bandera = true;
            if (validaPareja(obj.TransmisionID, obj.TransmisionVal)) bandera = true;
            if (validaPareja(obj.HidraulicoID, obj.HidraulicoVal)) bandera = true;
            if (validaPareja(obj.MotorDiferencialIdId, obj.DiferencialVal)) bandera = true;
            if (validaPareja(obj.MFTIzqId, obj.MFTIzqVal)) bandera = true;
            if (validaPareja(obj.MFTDerId, obj.MFTDerVal)) bandera = true;
            if (validaPareja(obj.MDIzqID, obj.MDIzqVal)) bandera = true;
            if (validaPareja(obj.MDDerID, obj.MDDerVal)) bandera = true;
            if (validaPareja(obj.DirId, obj.DirVal)) bandera = true;
            if (validaPareja(0, obj.Grasa)) bandera = true;
            return bandera;
        }

        function validaPareja(id, valor) {
            return valor == "0" || valor == "" ? false : true;
        }

        function tblConsumoMaqAceiteLubricante() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/AceitesLubricantes/tblConsumoMaqAceiteLubricante",
                type: "POST",
                datatype: "json",
                data: { cc: cboBusqCC.val(), consumo: cboBusqOrquesta.val(), turno: cboBusqTurno.val(), fecha: dpBusqFecha.val(), tipo: (cboBusqTipo.val() == "" ? 0 : cboBusqTipo.val()) },
                success: function (response) {
                    if (response.success) {
                        tblConsumo.bootgrid("clear");
                        if (response.lstMaq.length > 0) {
                            selectCboGridPipas = $('<select class="form-control cboAddConsumo" style="width: 100px;"></select>');
                            selectCboGridPipas.fillCombo('/AceitesLubricantes/FillDlOrquestas', { tipoComponente: 0, economico: 0 }, true, null);
                            tblConsumo.bootgrid("append", response.lstMaq);
                            //btnImprimir.removeClass("hidden");

                        } else {
                            //btnImprimir.addClass("hidden")
                        }
                    }
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function SaveMaqAceiteLubricante() {
            btnGaurdar.prop('disabled', true);
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/AceitesLubricantes/SaveMaqAceiteLubricante",
                type: "POST",
                datatype: "json",
                data: { lst: getConsumoFromTbl() },
                success: function (response) {
                    if (response.success) {
                        AlertaGeneral("Alerta", "El consumo de las máquinas fue guardado con éxito.");
                        tblConsumoMaqAceiteLubricante();
                    } else
                        AlertaGeneral("Alerta", "El consumo de las máquinas no fue guardado, revise sus datos.");
                    $.unblockUI();
                    btnGaurdar.prop('disabled', false);
                },
                error: function () {
                    $.unblockUI();
                    btnGaurdar.prop('disabled', false);
                }
            });
        }

        function GETDATA() {
            getConsumoFromTbl();
        }

        function verReporte() {
            $.blockUI({ message: mensajes.PROCESANDO });
            var idReporte = "49";
            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte;
            ireport.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        init();
    };
    $(document).ready(function () {
        maquinaria.Capturas.Diarias.ConsumoAceitesLubricantes = new ConsumoAceitesLubricantes();
    });
})();


