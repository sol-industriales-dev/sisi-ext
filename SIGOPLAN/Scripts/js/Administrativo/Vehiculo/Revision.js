(function () {
    $.namespace('Administrativo.Seguridad.Vehiculo.Revision');
    Revision = function () {
        lstDicEs = {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"
            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            }
        };
        DiccMP = {
            year: 'Año',
            buttonText: 'botón siguiente',
            prevYear: "Año anterior",
            nextYear: "Año siguiente",
            next12Years: 'Siguientes 12 años',
            prev12Years: 'Anteriores 12 años',
            nextLabel: "Siguiente",
            prevLabel: "Anterior",
            jumpYears: "Brincar años",
            backTo: "Selecionado",
            months: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"]
        }
        mesNombre = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Augosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
        selBusCC = $("#selBusCC");
        selBusEco = $("#selBusEco");
        dtBusMes = $("#dtBusMes");
        btnBuscar = $("#btnBuscar");
        tblVehiculo = $("#tblVehiculo");
        selCC = $("#selCC");
        dtFecha = $("#dtFecha");
        selEconomico = $("#selEconomico");
        txtKm = $("#txtKm");
        txtResponsable = $("#txtResponsable");
        selTipoLic = $("#selTipoLic");
        txtNumLic = $("#txtNumLic");
        dtVigenciaLic = $("#dtVigenciaLic");
        tblPartes = $("#tblPartes");
        numero = $(".numero");
        txtAnterior = $("#txtAnterior");
        txtActual = $("#txtActual");
        txtRequerimientos = $("#txtRequerimientos");
        txtNotas = $("#txtNotas");
        mdlCaptura = $("#mdlCaptura");
        btnGuardar = $("#btnGuardar");
        radioBtn = $('.radioBtn a');
        val = $('.val');
        function init() {
            initForm();
            btnGuardar.click(valGuardado);
            radioBtn.on('click', function () { aClick(this); });
            dtFecha.change(setMes);
            selBusCC.change(setBusEco);
            selCC.change(setEco);
            val.change(validaMov);
            selEconomico.change(getObjVehiculo);
            dtFecha.change(getObjVehiculo);
            numero.keydown(soloDigitos);
            mdlCaptura.on('shown.bs.modal', function () { dtPartes.columns.adjust(); });
            mdlCaptura.on('hidden.bs.modal', function () { setDefault(); });
        }
        function getObjVehiculo() {
            let res = $.post("/Administrativo/Vehiculo/getObjVehiculo", {
                cc: selCC.val(),
                eco: selEconomico.val(),
                fecha: dtFecha.val()
            });
            res.done(function (response) {
                if (response.success) {
                    AddRows(tblPartes, response.obs);
                }
            });
        }
        function saveVehiculo() {
            let res = $.post("/Administrativo/Vehiculo/saveVehiculo", {
                obj: getVehiculo(),
                lst: getObservaciones()
            });
            res.done(function (response) {
                if (response.success) {

                }
            });
        }
        function initVehiculo() {
            dtVehiculo = tblVehiculo.DataTable({
                destroy: true,
                language: lstDicEs,
                initComplete: function (settings, json) {
                    tblVehiculo.on('click', 'tr', function () {

                    });
                }
            });
        }
        function initPartes() {
            dtPartes = tblPartes.DataTable({
                destroy: true,
                bFilter: false,
                scrollY: '50vh',
                scrollCollapse: true,
                paging: false,
                order: true,
                language: lstDicEs,
                drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;
                    api.column(0, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(`<tr class="group"><td colspan="4" class="muted">${group}</td></tr>`);
                            last = group;
                        }
                    });
                },
                columns: [
                    { sortable: false, data: 'tipo', visible: false },
                    { sortable: false, data: 'parte' },
                    {
                        sortable: false, data: 'anterior',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(setCboComentario(cellData, 'ant'));
                        }
                    },
                    {
                        sortable: false,
                        data: 'actual',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(setCboComentario(cellData, 'act'));
                        }
                    },
                    {
                        sortable: false,
                        data: 'observaciones',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html($("<input>").addClass('form-control obs').val(cellData));
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblPartes.on('click', 'tr', function () {

                    });
                }
            });
        }
        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear();
            for (let i in lst)
                if (lst.hasOwnProperty(i))
                    AddRow(dt, lst[i]);
        }
        function AddRow(dt, obj) {
            dt.row.add(obj).draw(false);
        }
        function setCboComentario(value, clase) {
            let sel = $("<select></select>");
            sel.fillCombo('/Administrativo/Vehiculo/fillComboComentario', null, false, null);
            sel.addClass(`form-control ${clase}`);
            sel.find(`option[value="${value}"]`).attr('selected', true);
            return sel;
        }
        function setMes() {
            var fecha = dtFecha.datepicker("getDate");
            txtActual.text(mesNombre[fecha.getMonth()]);
            txtAnterior.text(mesNombre[fecha.getMonth() - 1]);
        }
        function soloDigitos(e) {
            // Allow: backspace, delete, tab, escape, enter and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 ||
                // Allow: Ctrl+A, Command+A
                (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                // Allow: home, end, left, right, down, up
                (e.keyCode >= 35 && e.keyCode <= 40))
                // let it happen, don't do anything
                return;
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105))
                e.preventDefault();
        }
        function setDefault() {
            btnGuardar.val(0);
            selCC.val("").change();
            txtKm.val(0);
            txtResponsable.val("");
            selTipoLic.val("");
            txtNumLic.val("");
            dtVigenciaLic.val(new Date().toLocaleDateString());
            setRadioValue("radPreventivo", true);
            dtPartes.clear().draw();
        }
        function aClick(esto) {
            let sel = $(esto).data('title');
            let tog = $(esto).data('toggle');
            $('#' + tog).prop('value', sel);
            $('a[data-toggle="' + tog + '"]').not('[data-title="' + sel + '"]').removeClass('active').addClass('notActive');
            $('a[data-toggle="' + tog + '"][data-title="' + sel + '"]').removeClass('notActive').addClass('active');
        }
        function setRadioValue(tog, sel) {
            $('#' + tog).prop('value', sel);
            $('a[data-toggle="' + tog + '"]').not('[data-title="' + sel + '"]').removeClass('active').addClass('notActive');
            $('a[data-toggle="' + tog + '"][data-title="' + sel + '"]').removeClass('notActive').addClass('active');
        }
        function valGuardado() {
            let ban = true;
            $.each(val, function (i, e) {
                if (!validarCampo($(this))) ban = false;
            });
            if (dtPartes.data().count() == 0) ban = false;
            if (ban) saveVehiculo();
            return ban;
        }
        function validaMov() {
            validarCampo($(this));
        }
        function validarCampo(_this) {
            var r = false;
            if (_this.val().length == 0 || _this.val() == 0) {
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
        function getVehiculo() {
            return {
                id: btnGuardar.val(),
                cc: selCC.val(),
                fecha: dtFecha.val(),
                economico: selEconomico.val(),
                responsable: txtResponsable.val(),
                tipoLicencia: selTipoLic.val(),
                numLicencia: txtNumLic.val(),
                vigenciaLicencia: dtVigenciaLic.val(),
                kilometraje: txtKm.val(),
                preventivo: getRadioValue("radPreventivo"),
                requerimientos: txtRequerimientos.val(),
                notas: txtNotas.val()
            }
        }
        function getObservaciones() {
            let lst = [];
            tblPartes.find("tbody tr").each(function (idx, row) {
                lst.push({
                    id: dtPartes.rows().data()[idx].id,
                    idVehiculo: dtPartes.rows().data()[idx].idVehiculo,
                    idTipo: dtPartes.rows().data()[idx].idTipo,
                    idParte: dtPartes.rows().data()[idx].idParte,
                    anterior: $(this).find(".ant").val(),
                    actual: $(this).find(".act").val(),
                    observaciones: $(this).find(".obs").val()
                });
            });
        }
        function getRadioValue(tog) {
            return $('a.active[data-toggle="' + tog + '"]').data('title');
        }
        function setBusEco() {
            selBusEco.fillCombo('/Administrativo/Vehiculo/fillCboEconomico', { cc: selBusCC.val() }, false, null);
        }
        function setEco() {
            selEconomico.fillCombo('/Administrativo/Vehiculo/fillCboEconomico', { cc: selCC.val() }, false, null);
        }
        function initForm() {
            selBusCC.fillCombo('/Administrativo/Poliza/getCC', null, false, null);
            selCC.fillCombo('/Administrativo/Poliza/getCC', null, false, null);
            selBusEco.fillCombo('/Administrativo/Vehiculo/fillCboEconomico', { cc: selBusCC.val() }, false, null);
            selEconomico.fillCombo('/Administrativo/Vehiculo/fillCboEconomico', { cc: selCC.val() }, false, null);
            selTipoLic.fillCombo('/Administrativo/Vehiculo/fillComboTipoLicencia', null, false, null);
            txtResponsable.getAutocomplete("", null, '/Administrativo/FormatoCambio/getEmpleados');
            dtVigenciaLic.datepicker({
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 99999999999);
                    }, 0);
                }
            }).datepicker("setDate", new Date());
            dtFecha.datepicker({
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 99999999999);
                    }, 0);
                }
            }).datepicker("setDate", new Date());
            dtBusMes.MonthPicker({
                Button: false,
                SelectedMonth: 0,
                i18n: DiccMP
            });
            setMes();
            initVehiculo();
            initPartes();
            setDefault();
        }
        init();
    }
    $(document).ready(function () {
        Administrativo.Seguridad.Vehiculo.Revision = new Revision();
    })
        .ajaxStart(function () {
            $.blockUI({
                message: 'Procesando...',
                baseZ: 2000
            });
        })
        .ajaxStop(function () { $.unblockUI(); });
})();