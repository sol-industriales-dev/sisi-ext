(function () {
    $.namespace('sigoplan.rh.plantillapersonal');
    plantillapersonal = function () {
        _ID = 0;
        _Nuevo = true;
        _Count = 1;
        _PuestosCargados = false;
        _Puestos = new Array();
        var cboCC = $("#cboCC");
        const txtFechaInicio = $("#txtFechaInicio");
        const txtFechaFin = $("#txtFechaFin");
        const btnBuscar = $("#btnBuscar");
        const btnContinuar = $("#btnContinuar");
        const btnGuardar = $("#btnGuardar");
        const btnAgregar = $("#btnAgregar");
        const btnEliminar = $("#btnEliminar");
        const dialogCargar = $("#dialogCargar");
        const selAutSolicita = $("#selAutSolicita");
        const selAutVoBo = $("#selAutVoBo");
        const selAutoriza11 = $("#selAutoriza11");
        const selAutoriza12 = $("#selAutoriza12");
        const selAutoriza21 = $("#selAutoriza21");
        const selAutoriza22 = $("#selAutoriza22");
        const modal = $("#modal");
        var tblData = $("#tblData");
        function init() {
            initTable();
            cboCC.fillCombo('/Administrativo/PlantillaPersonal/FillComboCC', { plantilla: false }, false);
            //cboCC.change(fnCargar);
            txtFechaInicio.datepicker().datepicker("setDate", new Date());
            txtFechaFin.datepicker().datepicker("setDate", new Date());
            btnAgregar.click(fnAgregar);

            btnGuardar.click(fnGuardar);
            btnBuscar.click(fnConfirmarCargado);
            btnContinuar.click(fnCargar);
            selAutSolicita.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutVoBo.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutoriza11.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutoriza12.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutoriza21.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutoriza22.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
           /* selAutoriza21.val("GERARDO REINA CECCO");
            selAutoriza21.data("id", 1164);
            selAutoriza21.data("nombre", "GERARDO REINA CECCO");*/

           /* selAutVoBo.val("MANUEL DE JESUS CRUZ GARCIA");
            selAutVoBo.data("id", 1041);
            selAutVoBo.data("nombre", "MANUEL DE JESUS CRUZ GARCIA");*/
        }
        function initTable() {
            tblData = $("#tblData").DataTable({
                columns: [
                    { data: 'id' },
                    { data: 'puestoID', width: "50px" },
                    { data: 'puesto', width: "600px" },
                    { data: 'departamento', width: "80px" },
                    { data: 'nomina', width: "80px" },
                    { data: 'personalNecesario', width: "80px" },
                    { data: 'sueldoBase', width: "100px" },
                    { data: 'complemento', width: "100px" },
                    { data: 'totalNomina', width: "100px" },
                    { data: 'Eliminar', width: "50px" }
                ],
                columnDefs: [{ targets: 0, "visible": false }],
                drawCallback: function (settings) {
                    var puestoID = $(".clsPuestoID");
                    var chkPuesto = $(".clsChkPuesto");
                    var puesto = $(".clsPuesto");
                    var departamento = $(".clsDepartamento");
                    var nomina = $(".clsNomina");
                    var personalNecesario = $(".clsPersonal");
                    var sueldoBase = $(".clsBase");
                    var complemento = $(".clsComplemento");
                    var totalNomina = $(".clsTotal");
                    var Eliminar = $(".clsEliminar");
                    $.each(puestoID, function (i, e) {
                        if ($(this).data("listo") == '0') {
                            $(this).change(function () {
                                var existe = _Puestos.includes($(this).val());
                                var row = $(e).parent().parent().find(".clsPuesto");
                                if (existe) {

                                    row.val($(this).val());
                                    $(e).data("id", $(this).val());
                                    row.change();
                                }
                                else {
                                    row.val("");
                                    row.change();
                                    $(e).data("id", 0);
                                }
                            });
                            $(this).data("listo", 1);
                        }
                    });
                    $.each(chkPuesto, function (i, e) {
                        if ($(this).data("listo") == '0') {
                            $(this).change(function () {
                                var elID = $(e).parent().parent().parent().parent().find(".clsPuestoID");
                                var el = $(e).parent().parent().parent().parent().find(".clsPuesto");

                                if ($(this).is(":checked")) {
                                    $(el[0]).unbind("change");
                                    $(el[0]).replaceWith('<input type="text" class="form-control clsPuesto" placeholder="Nuevo Puesto" data-listo="0">');
                                    $(el[0]).change(function () {
                                        $(this).val(($(this).val().toUpperCase()));
                                    });
                                    $(el[0]).data("listo", 1);
                                    elID.val("0");
                                    elID.data("id", 0);
                                    elID.prop("disabled", true);
                                    var nom = $(this).parent().parent().parent().parent().find(".clsNomina");
                                    nom.val(1);
                                    nom.prop("disabled", false);
                                }
                                else {
                                    $(el[0]).unbind("change");
                                    $(el[0]).replaceWith('<select class="form-control clsPuesto" data-listo="0"></select>');
                                    $(el[0]).data("listo", 0);
                                    elID.val("0");
                                    elID.data("id", 0);
                                    elID.prop("disabled", false);
                                    var nom = $(this).parent().parent().parent().parent().find(".clsNomina");
                                    nom.val(1);
                                    nom.prop("disabled", true);
                                    tblData.draw();
                                }
                                $(this).data("listo", 1);
                            });

                        }
                    });
                    $.each(puesto, function (i, e) {
                        if ($(this).data("listo") == '0') {

                            $(this).fillCombo('/Administrativo/PlantillaPersonal/FillComboPuestos', { est: true }, false);
                            $(this).change(function () {
                                var pre = $(this).find("option:selected").data("prefijo");
                                var elID = $(this).parent().parent().parent().parent().find(".clsPuestoID");
                                var nom = $(this).parent().parent().parent().parent().find(".clsNomina");
                                nom.val(pre);
                                nom.prop("disabled", true);
                                if ($(this).val() == undefined || $(this).val() == '') {
                                    elID.val("0");
                                    elID.data("id", 0);
                                    elID.prop("disabled", true);
                                }
                                else {
                                    elID.val($(this).val());
                                    elID.data("id", $(this).val());
                                    elID.prop("disabled", false);
                                }
                            });
                            $(this).data("listo", 1);
                            if (!_PuestosCargados) {
                                var opciones = $(this).find("option");
                                $.each(opciones, function (i2, e2) {
                                    _Puestos.push($(this).val());
                                });
                                _PuestosCargados = true;
                            }
                        }
                    });
                    $.each(departamento, function (i, e) {
                        if ($(this).data("listo") == '0') {
                            $(this).fillCombo('/Administrativo/PlantillaPersonal/FillComboDepartamentos', { cc: "" }, true);
                            $(this).data("listo", 1);
                        }
                    });
                    $.each(nomina, function (i, e) {
                        if ($(this).data("listo") == '0') {
                            $(this).fillCombo('/Administrativo/PlantillaPersonal/GetTipoNomina', { est: true }, true);
                            $(this).data("listo", 1);
                            $(this).prop("disabled", true);
                        }
                    });
                    $.each(personalNecesario, function (i, e) {
                        if ($(this).data("listo") == '0') {
                            $(this).DecimalFixNS(0);
                            $(this).data("listo", 1);
                        }
                    });
                    $.each(sueldoBase, function (i, e) {
                        if ($(this).data("listo") == '0') {
                            $(this).DecimalFixPs(2);
                            $(this).change(fnCalTotal);
                            $(this).data("listo", 1);
                        }
                    });
                    $.each(complemento, function (i, e) {
                        if ($(this).data("listo") == '0') {
                            $(this).DecimalFixPs(2);
                            $(this).change(fnCalTotal);
                            $(this).data("listo", 1);
                        }
                    });
                    $.each(totalNomina, function (i, e) {
                        if ($(this).data("listo") == '0') {
                            $(this).DecimalFixPs(2);
                            $(this).prop("disabled", true);
                            $(this).data("listo", 1);
                        }
                    });
                    $.each(Eliminar, function (i, e) {
                        if ($(e).data("listo") == '0') {
                            $(e).click(function () {
                                tblData.row($(this).parents('tr')).remove().draw();

                                let totalPersonal = 0;

                                $('#tblData').DataTable().rows().every(function (rowIdx, tableLoop, rowLoop) {
                                    totalPersonal += +$(this.node()).find('.clsPersonal').val();
                                });

                                $('#totalPersonalNecesario').text('Total personal necesario: ' + totalPersonal);
                            });
                            $(e).data("listo", 1);
                        }
                    });

                },
                initComplete: function (settings, json) {
                    $("#tblData").on('change', '.clsPersonal', function () {
                        let totalPersonal = 0;

                        $('#tblData').DataTable().rows().every(function (rowIdx, tableLoop, rowLoop) {
                            totalPersonal += +$(this.node()).find('.clsPersonal').val();
                        });

                        $('#totalPersonalNecesario').text('Total personal necesario: ' + totalPersonal);
                    });
                },
                "bPaginate": false,
                "searching": false,
                "bFilter": false,
                "bInfo": true,
            });

        }
        function fnCalTotal() {
            var row = $(this).parent().parent();
            var base = $(row.find(".clsBase")[0]).getVal(2);
            var complemento = $(row.find(".clsComplemento")[0]).getVal(2);
            $(row.find(".clsTotal")[0]).setVal((base + complemento));
        }
        function fnAgregar() {
            var puestoID = "<input type='text' class='form-control clsPuestoID' data-listo='0' data-id=''/>";
            var puesto = "<div class='row'><div class='col-md-1'><input type='checkbox' style='width:24px;height:24px;' title='Activar para capturar un puesto no existente' class='clsChkPuesto' data-listo='0'/></div><div class='col-md-11'><select class='form-control clsPuesto' data-listo='0'></select></div></div>";
            var departamento = "<select class='form-control clsDepartamento' data-listo='0'></select>";
            var nomina = "<select class='form-control clsNomina' data-listo='0'></select>";
            var personalNecesario = "<input type='text' class='form-control clsPersonal' data-listo='0'/>";
            var sueldoBase = "<input type='text' class='form-control clsBase' data-listo='0'/>";
            var complemento = "<input type='text' class='form-control clsComplemento' data-listo='0'/>";
            var totalNomina = "<input type='text' class='form-control clsTotal' data-listo='0'/>";
            var Eliminar = "<button class='btn btn-primary clsEliminar' data-listo='0'><i class='glyphicon glyphicon-remove' ></i></button>";
            tblData.row.add({
                'id': _Count,
                'puestoID': puestoID,
                'puesto': puesto,
                'departamento': departamento,
                'nomina': nomina,
                'personalNecesario': personalNecesario,
                'sueldoBase': sueldoBase,
                'complemento': complemento,
                'totalNomina': totalNomina,
                'Eliminar': Eliminar
            }
            ).draw(false);
            _Count++;
        }
        function fnGuardar() {
            $.blockUI({ message: "Procesando información..." });
            if (validateData()) {
                var plantilla = {};
                plantilla.id = _ID;
                plantilla.ccID = cboCC.val();
                plantilla.cc = $("#cboCC option:selected").text();
                plantilla.fechaInicio = txtFechaInicio.val();
                plantilla.fechaFin = txtFechaFin.val() == '' || txtFechaFin.val() == undefined ? null : txtFechaFin.val();
                plantilla.estatus = 1;
                plantilla.plantillaEKID = 0;

                var rows = $(".clsPuesto").parent().parent().parent().parent();
                var autorizan = $(".autoriza");
                var dets = new Array();
                $.each(rows, function (i, e) {
                    var o = {};
                    o.id = 0;
                    o.plantillaID = _ID;
                    o.puestoNumero = Number($(e).find(".clsPuestoID").data("id"));
                    if (o.puestoNumero == 0) {
                        o.puesto = $($(e).find(".clsPuesto")).val().toUpperCase();
                    }
                    else {
                        o.puesto = $($(e).find(".clsPuesto option:selected")).text();
                    }
                    o.departamento = $($(e).find(".clsDepartamento option:selected")).text();
                    o.departamentoNumero = $($(e).find(".clsDepartamento")).val();
                    o.tipoNomina = $($(e).find(".clsNomina")).val();
                    o.personalNecesario = $($(e).find(".clsPersonal")).getVal(0);
                    o.sueldoBase = $($(e).find(".clsBase")).getVal(2);
                    o.sueldocomplemento = $($(e).find(".clsComplemento")).getVal(2);
                    o.sueldoTotal = $($(e).find(".clsTotal")).getVal(2);
                    dets.push(o);
                });
                var auts = new Array();
                $.each(autorizan, function (i, e) {
                    var o = {};
                    o.id = 0;
                    o.plantillaID = _ID;
                    o.aprobadorClave = $(e).data("id");
                    o.aprobadorNombre = $(e).data("nombre");
                    o.aprobadorPuesto = $(e).data("puesto");
                    o.tipo = $(e).data("tipo");
                    o.estatus = 1;
                    o.firma = "S/F";
                    o.autorizando = false;
                    o.orden = $(e).data("orden");
                    o.comentario = "";
                    if (o.aprobadorNombre != '') {
                        auts.push(o);
                    }
                });
                $.ajax({
                    type: "POST",
                    url: "/Administrativo/PlantillaPersonal/GuardarPlantilla",
                    contentType: 'application/json',
                    data: JSON.stringify({ plantilla: plantilla, dets: dets, auts: auts }),
                    success: function (response) {
                        if (response.success === true) {
                            var path = "/Reportes/Vista.aspx?idReporte=104&plantillaID=" + response.plantillaID + "&inMemory=1";
                            var folio = response.folio;
                            $("#report").attr("src", path);
                            //var tipo = objEmpleadoCambios.id == 0 ? "nuevo" : "cambio";//raguilar moodificacion 
                            document.getElementById('report').onload = function () {//error sin confirmacion 260118 1205
                                $.ajax({
                                    datatype: "json",
                                    type: "POST",
                                    url: '/Administrativo/PlantillaPersonal/EnviarCorreo',
                                    data: { plantillaID: response.plantillaID, autorizacion: 0, estatus: 1 },
                                    success: function (response) {
                                        $.unblockUI();
                                        limpiar();
                                        AlertaGeneral("Confirmación", "¡Registro guardado correctamente con Folio: " + folio + "!");
                                    },
                                    error: function () {
                                        $.unblockUI();
                                    }
                                });
                            };

                        }
                        else {
                            $.unblockUI();
                            AlertaGeneral("Error", "¡Ocurrio un error al guardar intente de nuevo!");
                        }
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
            else {
                $.unblockUI();
            }
        }
        function fnConfirmarCargado() {
            dialogCargar.modal("show");
        }
        function fnCargar() {

            tblData.clear().draw();
            //tblData.ajax.reload(null, false);
        }
        function validateModal() {
            var state = true;
            //if (!validarCampo(txtCodigo)) { state = false; }
            return state;
        }
        function limpiarCampos() {
            _ID = 0;
            //txtCodigo.val("");
        }
        function fnSelRevisa(event, ui) {
            $(this).data("id", ui.item.id);
            $(this).data("nombre", ui.item.value);
        }
        function fnSelNull(event, ui) {
            if (ui.item === null && $(this).val() != '') {
                $(this).val("");
                $(this).data("id", "");
                $(this).data("nombre", "");
                AlertaGeneral("Alerta", "Solo puede seleccionar un usuario de la lista, si no aparece en la lista de autocompletado favor de solicitar al personal de TI");
            }
        }
        function validateData() {
            var state = true;

            if (!validarCampo(cboCC)) { state = false; }
            if (!validarCampo(txtFechaInicio)) { state = false; }
            if (!validarCampo(txtFechaFin)) { state = false; }
            var rows = $(".clsPuesto").parent().parent().parent().parent();
            $.each(rows, function (i, e) {
                if (!validarCampo($($(e).find(".clsPuesto")))) { state = false; }
                if (!validarCampo($($(e).find(".clsDepartamento")))) { state = false; }
                if (!validarCampo($($(e).find(".clsNomina")))) { state = false; }
                //if (!validarCampo($($(e).find(".clsPersonal")))) { state = false; }
                //if (!validarCampo($($(e).find(".clsBase")))) { state = false; }
                //if (!validarCampo($($(e).find(".clsComplemento")))) { state = false; }
                //if (!validarCampo($($(e).find(".clsTotal")))) { state = false; }
            });
            if (!validarCampo(selAutSolicita)) { state = false; }
            if (!validarCampo(selAutVoBo)) { state = false; }
            if (!validarCampo(selAutoriza11)) { state = false; }

            if (!validarCampo(selAutoriza21)) { state = false; }


            return state;
        }
        function limpiar() {
            cboCC.fillCombo('/Administrativo/PlantillaPersonal/FillComboCC', { plantilla: false }, false);
            tblData.clear().draw();
            txtFechaFin.val("");
            selAutSolicita.val("");
            selAutVoBo.val("");
            selAutoriza11.val("");
            selAutoriza12.val("");
            selAutoriza21.val("");
            selAutoriza22.val("");
        }
        init();
    };
    $(document).ready(function () {
        sigoplan.rh.plantillapersonal = new plantillapersonal();
    });
})();
function clickActualizar(id, codigo, descripcion) {
    _ID = id;
    //txtCodigo.val(codigo);
    btnGuardar.val("Actualizar");
    modal.modal("show");
}
