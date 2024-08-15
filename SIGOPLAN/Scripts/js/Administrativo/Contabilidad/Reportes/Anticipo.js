(function () {
    $.namespace('Administrativo.Reportes.Anticipo');
    Anticipo = function () {
        const selProv = $("#selProv");
        const selBusqProv = $("#selBusqProv");
        const selCC = $("#selCC");
        const dtEmision = $("#dtEmision");
        const dtVencimiento = $("#dtVencimiento");
        const txtAnticipo = $("#txtAnticipo");
        const radioBtn = $('.radioBtn a');
        const txtConcepto = $("#txtConcepto");
        const selFactoraje = $("#selFactoraje");
        const selBanco = $("#selBanco");
        const btnGuardar = $("#btnGuardar");
        const valida = $(".valida");
        const tblAnticipo = $("#tblAnticipo");
        const btnBuscar = $("#btnBuscar");
        const mdlCaptura = $("#mdlCaptura");
        function init() {
            initForm();
            initTable();
            buscar();
            btnGuardar.click(valGuardado);
            btnBuscar.click(buscar);
            radioBtn.on('click', function () { aClick(this); });
            valida.change(validar);
            txtAnticipo.change(setAnticipo);
            mdlCaptura.on("hidden.bs.modal", function () { 
                setForm({
                    numProveedor: "",
                    centro_costos: "",
                    cif: "",
                    factoraje: "V",
                    fecha: new Date(),
                    fechaVencimiento: new Date(),
                    anticipo: 0,
                    estatus: false,
                    concepto: ""
                })
            });
        }
        function buscar() {
            let res = $.post("/Administrativo/Reportes/getLstAnticipo", {
                numProveedor: selBusqProv.val()
            });
            res.done(function (response) {
                if (response.success)
                    AddRows(response.lstAnticipos);
            });
        }
        function AddRows(lst) {
            dt = tblAnticipo.DataTable();
            dt.clear().draw(false);
            for (let i in lst)
                if (lst.hasOwnProperty(i))
                    AddRow(dt, lst[i]);
        }
        function AddRow(dt, obj) {
            dt.row.add(obj).draw(false);
            dt.order([0, 'asc']).draw();
            dt.page('last').draw(false);
        }
        function guardar() {
            let anticipo = unmaskDinero(txtAnticipo.val()),
                iva = anticipo / 16;
            let res = $.post("/Administrativo/Reportes/GuardarAnticipo", {
                obj: {
                    id: btnGuardar.val(),
                    proveedor: selProv.find(":selected").text(),
                    numProveedor: selProv.val(),
                    numNafin: selProv.find(":selected").data().prefijo,
                    anticipo: anticipo,
                    IVA: iva,
                    tipoCambio: 1,
                    monto: anticipo - iva,
                    concepto: txtConcepto.val(),
                    factoraje: selFactoraje.val(),
                    cif: selBanco.val(),
                    banco: selBanco.find(":selected").text(),
                    fecha: dtEmision.val(),
                    fechaVencimiento: dtVencimiento.val(),
                    estatus: getRadioValue("radEstatus"),
                    tipoMoneda: selProv.val() < 9000 ? 1 : 2,
                    centro_costos: selCC.val(),
                    nombCC: selCC.find(":selected").text(),
                }
            });
            res.done(function (response) {
                if (response.success) {
                    AlertaGeneral("Aviso", "Anticipo guardado");
                }
            });
        }
        function valGuardado() {
            let ban = true;
            $.each(valida, function (i, e) {
                if (!validarCampo($(this))) ban = false;
            });
            if (ban) guardar();
        }
        function validar() {
            validarCampo($(this));
        }
        function validarCampo(_this) {
            var r = false;
            if (_this.val().length == 0 || _this.val() == "$0.00") {
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
        function setAnticipo() {
            this.value = maskDinero(unmaskDinero(this.value));
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
        function getRadioValue(tog) {
            return $('a.active[data-toggle="' + tog + '"]').data('title');
        }
        function initTable() {
            dtAnticipo = tblAnticipo.DataTable({
                destroy: true,
                language: dtDicEsp,
                columns: [
                    { data: 'numProveedor' },
                    { data: 'proveedor' },
                    {
                        data: 'factoraje',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(`${cellData == "N" ? "Normal" : "Vencido"}`);
                        }
                    },
                    { data: 'banco' },
                    {
                        data: 'tipoMoneda',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(`${cellData == 1 ? "Pesos" : "Dólares"}`);
                        }
                    },
                    {
                        data: 'anticipo',
                        className: "text-right",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(maskDinero(cellData));
                        }
                    },
                    {
                        data: 'fecha',
                        className: "text-right",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(setDate(cellData).toLocaleDateString());
                        }
                    },
                    {
                        data: 'fechaVencimiento',
                        className: "text-right",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(setDate(cellData).toLocaleDateString());
                        }
                    },
                    {
                        data: 'id',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<button><i></i> Editar</button>");
                            $(td).find("button").val(cellData);
                            $(td).find("button").addClass('btn btn-primary edit');
                            $(td).find("i").addClass('fa fa-edit');
                        }
                    },
                    {
                        data: 'id',
                        width: "100px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(`<div data-id='${cellData}'><a data-toggle='radEstatus${cellData}' data-title=${false}><i></i></a><a data-toggle='radEstatus${cellData}' data-title=${true}><i></i></a></div>`);
                            $(td).find("div").addClass('radioBtn btn-group');
                            $(td).find("a").addClass('btn btn-primary');
                            $($(td).find("i")[0]).addClass('fa fa-check');
                            $($(td).find("i")[1]).addClass('fa fa-times');
                            if (rowData.estatus) {
                                $($(td).find("a")[0]).addClass('notActive');
                                $($(td).find("a")[1]).addClass('active');
                            } else {
                                $($(td).find("a")[1]).addClass('notActive');
                                $($(td).find("a")[0]).addClass('active');
                            }
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblAnticipo.on('click', '.edit', function () {
                        let res = $.post("/Administrativo/Reportes/getObjAnticipo", {
                            id: this.value
                        });
                        res.done(function (response) {
                            if (response.success){
                                setForm(response.obj);
                                mdlCaptura.modal("toggle");
                            }                                
                        });
                    });
                    tblAnticipo.on('click', '.radioBtn a', function () {
                        let sel = $(this).data('title'),
                            tog = $(this).data('toggle');
                        $('#' + tog).prop('value', sel);
                        $('a[data-toggle="' + tog + '"]').not('[data-title="' + sel + '"]').removeClass('active').addClass('notActive');
                        $('a[data-toggle="' + tog + '"][data-title="' + sel + '"]').removeClass('notActive').addClass('active');
                        let res = $.post("/Administrativo/Reportes/updateEstatus", {
                            estatus: sel,
                            id: tog.replace('radEstatus', '')
                        });
                    });
                }
            });
        }
        function setForm(obj) {
            $.each(valida, function (i, e) {
                $(this).removeClass("errorClass");
            });
            selProv.val(obj.numProveedor);
            selCC.val(obj.centro_costos);
            selBanco.val(obj.cif);
            selFactoraje.val(obj.factoraje);
            dtEmision.datepicker("setDate", obj.fecha);
            dtVencimiento.datepicker("setDate", obj.fechaVencimiento);
            txtAnticipo.val(maskDinero(obj.anticipo));
            setRadioValue("radEstatus", obj.estatus);
            txtConcepto.val(obj.concepto);
        }
        function initForm() {
            selProv.fillCombo('/Administrativo/Reportes/FillComboProv', null, false, null);
            selProv.select2();
            selBusqProv.fillCombo('/Administrativo/Reportes/FillComboProv', null, false, null);
            selBusqProv.select2();
            selCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', null, false, null);
            selCC.select2();
            dtEmision.datepicker({
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 99999999999);
                    }, 0);
                }
            }).datepicker("setDate", new Date());
            dtVencimiento.datepicker({
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 99999999999);
                    }, 0);
                }
            }).datepicker("setDate", new Date());
            txtAnticipo.val(maskDinero(0));
            btnGuardar.val(0);
        }
        function unmaskDinero(dinero) {
            return Number(dinero.replace(/[^0-9\.]+/g, ""));
        }
        function maskDinero(numero) {
            return "$" + parseFloat(numero).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        }
        function setDate(jsonDate) {
            return new Date(parseInt(jsonDate.substr(6)));
        }
        init();
    }
    $(document).ready(function () {
        Administrativo.Reportes.Anticipo = new Anticipo();
    })
        .ajaxStart(function () { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(function () { $.unblockUI(); });
})();