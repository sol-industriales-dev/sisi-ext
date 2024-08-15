(() => {
    $.namespace('Maquinaria.Captura.StandBy');
    StandBy = function () {
        const cboTipo = $("#cboTipo");
        const btnVoBo = $('#btnVoBo');
        const btnAutorizar = $("#btnAutorizar");
        const btnRechazar = $("#btnRechazar");
        const btnLiberar = $("#btnLiberar");
        const btnCargar = $("#btnCargar");
        const txtFecha = $("#txtFecha");
        const tblData = $("#tblData");
        var dtData;

        const strGetData = new URL(window.location.origin + '/StandByNuevo/getListaByEstatus');
        const strGuardarValidacion = new URL(window.location.origin + '/StandByNuevo/GuardarValidacion');
        const strGuardarLiberacion = new URL(window.location.origin + '/StandByNuevo/GuardarLibracion');

        let puedeDarVobo = false;
        let puedeAutorizar = false;

        let init = () => {
            btnLiberar.hide();
            InitForm();
            btnAutorizar.click(fnAutorizar);
            btnRechazar.click(fnRechazar);
            btnLiberar.click(fnLiberar);
            btnCargar.click(CargarTblData);
            setTimeout(() => {
                btnCargar.trigger("click");
            }, 3000);
            $('#tblData').on({
                change: function () {
                    var _this = $(this).is(":checked");
                    var justificacion = $(this).parent().parent().find(".justificacion");
                    if (_this) {
                        justificacion.prop("disabled", false);
                        justificacion.val("");
                    }
                    else {
                        justificacion.prop("disabled", true);
                        justificacion.val("");
                    }
                }
            }, 'input[type="checkbox"]');

            fncGetUsuarioTipoAutorizacion();

            btnVoBo.click(function () {
                fncIndicarVoBo();
            });
        }
        function fnAutorizar() {
            setGuardar(2);
        }
        function fnRechazar() {
            setGuardar(3);
        }
        function fnLiberar() {
            setGuardar(4);
        }
        async function setGuardar(estatus) {
            try {
                var lst = [];
                var aplica = $(".aplica");
                var completo = true;
                $.each(aplica, function (i, e) {
                    if ($(e).is(":checked")) {
                        var o = {};
                        o.id = $(e).data("id");
                        o.Economico = $(e).data("Economico");
                        o.noEconomicoID = $(e).data("noeconomicoid");
                        o.estatus = estatus;
                        o.cc = $(e).data("cc");
                        o.comentario = $(e).parent().parent().find(".justificacion").val();
                        o.modelo = $(e).data('modelo');
                        o.ccActual = $(e).data('ccActual');
                        o.moiEquipo = $(e).data('moiEquipo');
                        o.moiOverhaul = $(e).data('moiOverhaul');
                        o.dep = $(e).data('dep');
                        o.valorEnLibro = $(e).data('valorEnLibro');
                        // if ((o.comentario == '' || o.comentario == undefined) && estatus == 3) {
                        //     completo = false;
                        // }
                        lst.push(o);
                    }
                });
                if (completo) {
                    if (lst.length > 0) {
                        response = await ejectFetchJson(((estatus == 2 || estatus == 3) ? strGuardarValidacion : strGuardarLiberacion), { lst });
                        if (response.success) {
                            AlertaGeneral("Aviso", `Registros guardados con éxito`);
                            CargarTblData();
                        } else {
                            AlertaGeneral(`Error`, `Ocurrió un error al intentar guardar la información.`);
                        }
                    }
                    else {
                        AlertaGeneral(`Alerta`, `Debe seleccionar almenos un equipo para validar StandBy`);
                    }
                }
                else {
                    AlertaGeneral(`Alerta`, `Es necesario que se escriba la justificación de cada equipo que desee rechazar`);
                }
            }
            catch (o_O) {
                AlertaGeneral("Aviso", o_O.message)
            }
        }
        async function CargarTblData() {
            try {
                let estatus = cboTipo.val();
                var noEconomico = "";
                dtData.clear().draw();
                response = await ejectFetchJson(strGetData, { estatus, noEconomico });
                dtData.rows.add(response.data).draw();
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
        }
        function InitTblData() {
            dtData = tblData.DataTable({
                destroy: true,
                ordering: true,
                language: dtDicEsp,
                columns: [
                    { title: 'Equipo', data: 'Economico', width: '70px' },
                    { title: 'Modelo', data: 'modelo', width: '70px' },
                    { title: 'Obra', data: 'ccActual' },
                    { title: 'Fecha Solicitud', data: 'fechaCaptura', width: '70px' },
                    { title: 'Justificación', data: 'comentarioJustificacion' },
                    { title: 'MOI Equipo', data: 'moiEquipo' },
                    { title: 'Falta Dep Equipo', data: 'valorEnLibroEquipo' },
                    { title: 'Dep Mensual Equipo', data: 'depreciacionMensualEquipo' },
                    { title: 'Falta Dep OH', data: 'valorEnLibroOverhaul' },
                    { title: 'Dep Mensual OH', data: 'depreciacionMensualOverhaul' },
                    {
                        title: 'Observación', data: 'estatus',
                        createdCell: (td, data, rowData, row, col) => {
                            $(td).html("<input type='text'/>");
                            $(td).find(`input[type="text"]`).addClass(`form-control justificacion`);
                            $(td).find(`input[type="text"]`).prop("disabled", true);
                        }
                    },
                    {
                        title: 'Seleccionar', data: 'noEconomicoID', width: '70px',
                        createdCell: (td, data, rowData, row, col) => {
                            if ((!rowData.esVoBo && puedeDarVobo) || (rowData.esVoBo && puedeAutorizar)) {
                                $(td).html("<input type='checkbox' data-id='" + rowData.id + "'/>");
                                $(td).find(`input[type="checkbox"]`).addClass(`form-control aplica`);
                                $(td).find(`input[type="checkbox"]`).data("id", rowData.id);
                                $(td).find(`input[type="checkbox"]`).data("noEconmico", rowData.Economico);
                                $(td).find(`input[type="checkbox"]`).data("noEconmicoID", rowData.noEconomicoID);
                                $(td).find(`input[type="checkbox"]`).data("cc", rowData.ccActual);
                                $(td).find(`input[type="checkbox"]`).data('Economico', rowData.Economico);
                                $(td).find(`input[type="checkbox"]`).data('modelo', rowData.modelo);
                                $(td).find(`input[type="checkbox"]`).data('moiEquipo', rowData.moiEquipo);
                                $(td).find(`input[type="checkbox"]`).data('valorEnLibroEquipo', rowData.valorEnLibroEquipo);
                                $(td).find(`input[type="checkbox"]`).data('depreciacionMensualEquipo', rowData.depreciacionMensualEquipo);
                                $(td).find(`input[type="checkbox"]`).data('valorEnLibroOverhaul', rowData.valorEnLibroOverhaul);
                                $(td).find(`input[type="checkbox"]`).data('depreciacionMensualOverhaul', rowData.depreciacionMensualOverhaul);
                                $(td).find(`input[type="checkbox"]`).data('ccActual', rowData.ccActual);
                            } else if (rowData.estatus == 'Autorizado') {
                                $(td).html("<p>Autorizado.</p>");
                            } else if (rowData.esVoBo && rowData.estatus == 'Pendiente') {
                                $(td).html("<p>Pendiente autorización.</p>");
                            } else if (rowData.esVoBo) {
                                $(td).html("<p>VoBo registrado.</p>");
                            } else {
                                $(td).html("<p>Se requiere VoBo.</p>");
                            }
                        }
                    },
                    { title: 'Tabulador', data: null }
                ],
                columnDefs: [
                    {
                        targets: [5, 6, 7, 8, 9],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [12],
                        render: function (data, type, row) {
                            let btnTabulador = '<button class="btn btn-success btnTabulador btn-xs" title="Tabulador"><i class="fas fa-eye"></i></button>';
                            return btnTabulador;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblData.on('click', '.btnTabulador', function () {
                        let rowData = tblData.DataTable().row($(this).closest('tr')).data();
                        window.open('/ActivoFijo/TabuladorDepreciacion?noEconomico=' + rowData.Economico);
                    });
                }
            });
        }

        function toDateFromJson(src) {
            let strfecha = $.toDate(src).split("/")
                , fecha = new Date(+strfecha[2], +strfecha[1], +strfecha[0]);
            return fecha;
        }

        function InitForm() {
            let ahora = new Date();
            $(".fechaMesAnio").MonthPicker({
                Button: false,
                MonthFormat: 'MM, yy',
                i18n: mpDicEsp
            });

            txtFecha.datepicker().datepicker("setDate", ahora);
            cboTipo.change(function () {
                if ($(this).val() == '1') {
                    btnRechazar.hide();
                    btnLiberar.hide();
                    if (puedeDarVobo) {
                        btnVoBo.show();
                    }
                    if (puedeAutorizar) {
                        btnAutorizar.show();
                    }
                }
                else {
                    btnAutorizar.hide();
                    btnRechazar.hide();
                    btnVoBo.hide();
                    if (puedeAutorizar || puedeDarVobo) {
                        btnLiberar.show();
                    }
                }
                CargarTblData();
            });
            InitTblData();
            //CargarTblData();
        }

        function fncGetUsuarioTipoAutorizacion() {
            axios.post('GetUsuarioTipoAutorizacion').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (response.data.vobo == 1) {
                        btnVoBo.css("display", "inline");
                        btnAutorizar.css("display", "none");
                        puedeDarVobo = true;
                    } else {
                        btnVoBo.css("display", "none");
                        btnAutorizar.css("display", "inline");
                        puedeDarVobo = false;
                        btnVoBo.hide();
                        puedeAutorizar = true;
                    }
                } else {
                    btnVoBo.hide();
                    btnAutorizar.hide();
                    btnRechazar.hide();
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncIndicarVoBo() {
            var lst = [];
            var aplica = $(".aplica");
            $.each(aplica, function (i, e) {
                if ($(e).is(":checked")) {
                    var o = {};
                    o.id = $(e).data("id");
                    o.Economico = $(e).data("Economico");
                    o.noEconomicoID = $(e).data("noeconomicoid");
                    o.estatus = 1;
                    o.cc = $(e).data("cc");
                    o.comentario = $(e).parent().parent().find(".justificacion").val();
                    o.modelo = $(e).data('modelo');
                    o.ccActual = $(e).data('ccActual');
                    o.moiEquipo = $(e).data('moiEquipo');
                    o.moiOverhaul = $(e).data('moiOverhaul');
                    o.dep = $(e).data('dep');
                    o.valorEnLibro = $(e).data('valorEnLibro');
                    lst.push(o);
                }
            });
            axios.post('/StandByNuevo/GuardarVoBo', lst).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito(message);
                    btnCargar.trigger("click");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        init();
    }
    $(document).ready(() => {
        Maquinaria.Captura.StandBy = new StandBy();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();