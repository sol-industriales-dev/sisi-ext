(function () {
    $.namespace('administrativo.facultamiento.Historico');
    Historico = function () {

        txtFechaI = $("#txtFechaI"),
            txtFechaF = $("#txtFechaF"),
            selCC = $("#selCC");
        tblCuadro = $("#tblCuadro");
        tblAutorizador = $("#tblAutorizador");
        lblCC = $("#lblCC");
        lblEstatus = $("#lblEstatus");
        mdlAutorizacion = $("#mdlAutorizacion");
        function init() {


            initForm();
            initCuadro();
            initAutorizacion();
            selCC.change(getCCCompleto);

            var now = new Date(),
                year = now.getYear() + 1900;
            txtFechaI.datepicker().datepicker("setDate", "01/01/" + year);
            txtFechaF.datepicker().datepicker("setDate", new Date());


            txtFechaI.change((getCCCompleto))
            txtFechaF.change((getCCCompleto))


        }
        function initForm() {
            selCC.fillCombo('/Administrativo/Facultamiento/getComboCC', null, false, "Todos");
            convertToMultiselect("#selCC");
        }

        function datePicker() {
            var now = new Date(),
                year = now.getYear() + 1900;
            $.datepicker.setDefaults($.datepicker.regional["es"]);
            var dateFormat = "dd/mm/yy",
                from = $("#txtFechaI")
                    .datepicker({
                        changeMonth: true,
                        changeYear: true,
                        numberOfMonths: 1,
                        defaultDate: new Date(year, 00, 01),
                        maxDate: new Date(year, 11, 31),

                        onChangeMonthYear: function (y, m, i) {
                            var d = i.selectedDay;
                            $(this).datepicker('setDate', new Date(y, m - 1, d));
                            $(this).trigger('change');
                        }

                    })
                    .on("change", function () {
                        to.datepicker("option", "minDate", getDate(this));
                    }),
                to = $("#txtFechaF").datepicker({
                    changeMonth: true,
                    changeYear: true,
                    numberOfMonths: 1,
                    defaultDate: new Date(),
                    maxDate: new Date(year, 11, 31),
                    onChangeMonthYear: function (y, m, i) {
                        var d = i.selectedDay;
                        $(this).datepicker('setDate', new Date(y, m - 1, d));
                        $(this).trigger('change');
                    }
                })
                    .on("change", function () {
                        from.datepicker("option", "maxDate", getDate(this));
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

        function initCuadro() {

            var now = new Date(),
                year = now.getYear() + 1900;
            txtFechaI.datepicker().datepicker("setDate", "01/01/" + year);
            txtFechaF.datepicker().datepicker("setDate", new Date());

            dtCuadro = tblCuadro.DataTable({
                language: dtDicEsp,
                columns: [
                    { data: 'cc' },
                    { data: 'obra' },
                    { data: 'fecha' },
                    { data: 'registro' },
                    {
                        data: 'estatus',
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.comentario && rowData.comentario.length > 0) {
                                $(td).append(
                                    ` <button class="btn btn-danger btn-sm" onclick="AlertaGeneral('Razón de rechazo','${rowData.comentario}')" >
                                        <i class="far fa-comment"></i>
                                    </button>`);
                            }
                        }
                    },
                    {
                        data: 'id',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<button><i></i> Reporte</button>");
                            $(td).find("button").addClass('btn btn-primary report');
                            $(td).find("i").addClass('fa fa-print');
                        }
                    },
                    {
                        data: 'id',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html("<button><i></i> Autorizaciones</button>");
                            $(td).find("button").addClass('btn btn-default auto');
                            $(td).find("i").addClass('fa fa-user');
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblCuadro.on('click', '.report', function () {
                        verReporte(dtCuadro.row($(this).parents('tr')).data().id);
                    });
                    /*tblCuadro.on('click', '.editar', function () {
                       // selCC.val(dtCuadro.row($(this).parents('tr')).data().cc).change();
                        mdlCaptura.modal('toggle');
                    });*/
                    tblCuadro.on('click', '.auto', function () {
                        let data = dtCuadro.row($(this).parents('tr')).data();
                        lblCC.text(data.obra);
                        getLstAutorizacion(data.id);
                        lblEstatus.text(data.estatus);
                        mdlAutorizacion.modal('toggle');
                    });
                }
            });
        }
        function initAutorizacion() {
            dtAuto = tblAutorizador.DataTable({
                language: dtDicEsp,
                destroy: true,
                searching: false,
                paging: false,
                iDisplayLength: -1,
                info: false,
                deferRender: true,
                sort: false,
                columns: [
                    {
                        data: 'orden',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(cellData == 0 ? "Autorizante" : `Vobo ${cellData}`);
                        }
                    },
                    { data: 'nombre' },
                    { data: 'auth' },
                ]
            });
        }
        function verReporte(id) {
            var path = `/Reportes/Vista.aspx?idReporte=78&id=${id}&isCRModal=${true}`;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                openCRModal();
            };
        }
        function getCCCompleto() {
            let res = $.post("/Administrativo/facultamiento/getCCCompleto", { cc: getValoresMultiples("#selCC"), fechaInicio: txtFechaI.val(), fechaFin: txtFechaF.val() });
            res.done(function (response) {
                if (response.success) {
                    dt = tblCuadro.DataTable();
                    dt.clear().draw();
                    AddRows(tblCuadro, response.lstGestion);



                }
            });
        }
        function getLstAutorizacion(id) {
            let res = $.post("/Administrativo/facultamiento/getHistAutorizacion",
                { id: id });
            res.done(function (response) {
                if (response.success) {
                    if (response.lstAuth.length == 0)
                        dtAuto.clear().draw();
                    else
                        AddRows(tblAutorizador, response.lstAuth);
                }
            });
        }
        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            for (let i in lst)
                if (lst.hasOwnProperty(i))
                    AddRow(dt, lst[i]);
        }
        function AddRow(dt, obj) {
            dt.row.add(obj).draw(false);
        }
        init();
    }
    $(document).ready(function () {
        administrativo.facultamiento.Historico = new Historico();
    })
        .ajaxStart(function () { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(function () { $.unblockUI(); });
})();