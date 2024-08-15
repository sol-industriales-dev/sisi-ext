(function () {

    $.namespace('maquinaria.inventario.Calidad');

    Calidad = function () {
        cboTipoControl = $("#cboTipoControl");
        tblEquiposPendientes = $("#tblEquiposPendientes");
        tbGrupoMaquinariaModal = $("#tbGrupoMaquinariaModal");
        tbTipoMaquinariaModal = $("#tbTipoMaquinariaModal");
        tbModeloMaquinariaModal = $("#tbModeloMaquinariaModal");
        tbHorasModal = $("#tbHorasModal");
        tblEconomicosNoAsignados = $("#tblEconomicosNoAsignados");
        cboFiltroEquipos = $("#cboFiltroEquipos");
        modalListaEquiposAsignados = $("#modalListaEquiposAsignados");
        ireport = $("#report");

        mensajes = {
            NOMBRE: 'Control Envio y Recepcion',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        //#region FILTRO CONSTS

        const fechaFiltroInicio = $('#fechaFiltroInicio');
        const fechaFiltroFin = $('#fechaFiltroFin');
        const cboFiltroCC = $('#cboFiltroCC');
        const cboFiltroEconomico = $('#cboFiltroEconomico');
        const btnFiltroBuscar = $('#btnFiltroBuscar');

        //#endregion


        function init() {

            var id = $.urlParam('Tipo');
            if (id == "E") {
                cboTipoControl.val(1);
            }
            else {
                cboTipoControl.val(2);
            }

            // cboTipoControl.change(fillInfo);
            // cboFiltroEquipos.change(fillInfo);
            // fillInfo();

            //#region FILL COMBO
            $(".select2").select2();
            $(".select2").select2({ width: "100%" });
            cboFiltroCC.fillCombo('/ControlCalidad/GetCCs', {}, false);
            cboFiltroEconomico.fillCombo('/ControlCalidad/GetEconomicos', {}, false);
            //#endregion

            btnFiltroBuscar.on("click", function () {
                fillInfo();
            });
        }

        function bootG(url, TipoControl, FiltroControl, fechaInicioVal, fechaFinVal, ccVal, numEconomicoVal) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: url,
                type: "POST",
                datatype: "json",
                data: { obj: TipoControl, tipoFiltro: FiltroControl, fechaInicio: fechaInicioVal, fechaFin: fechaFinVal, cc: ccVal, numEconomico: numEconomicoVal },
                success: function (response) {
                    $.unblockUI();
                    var data = response.EquiposPendientes;
                    tblEquiposPendientes.bootgrid("clear");
                    tblEquiposPendientes.bootgrid("append", data);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function fillInfo() {
            cboTipoControl.attr('data-tipo', cboTipoControl.val());

            if (fechaFiltroInicio.val() > fechaFiltroFin.val()) {
                Alert2Warning("Ingrese una fecha valida");
                return "";
            }

            if (fechaFiltroInicio.val() == "" && fechaFiltroFin.val() != "") {
                Alert2Warning("Ingrese una fecha de inicio");
                return "";
            }

            if (fechaFiltroFin.val() == "" && fechaFiltroInicio.val() != "") {
                Alert2Warning("Ingrese una fecha de fin");
                return "";
            }

            bootG('/ControlCalidad/GetMaquinariasPendientesEnvios', cboTipoControl.val(), cboFiltroEquipos.val(), fechaFiltroInicio.val(), fechaFiltroFin.val(), cboFiltroCC.val(), cboFiltroEconomico.val());

        }

        function GetEconomicosNoAsignados(idAsignacion) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/MovimientoMaquinaria/GetEconomicosNoAsignados",
                type: "POST",
                datatype: "json",
                data: { idAsignacion: idAsignacion },
                success: function (response) {
                    $.unblockUI();
                    var ListaEconomicos = response.ListaEconomicos;
                    //Agregar las referencias
                    tbGrupoMaquinariaModal.val(response.GrupoMaquinaria)
                    tbTipoMaquinariaModal.val(response.TipoMaquinaria);
                    tbModeloMaquinariaModal.val(response.ModeloMaquinaria);
                    tbHorasModal.val(response.Horas);

                    if (ListaEconomicos != null) {

                        tblEconomicosNoAsignados.bootgrid("clear");
                        tblEconomicosNoAsignados.bootgrid("append", ListaEconomicos);
                        modalListaEquiposAsignados.modal('show');

                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function iniciarGrid() {
            tblEquiposPendientes.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                formatters: {

                    "VerControlCalidad": function (column, row) {
                        if (row.estatus == cboTipoControl.attr('data-tipo') || row.estatus - 1 == cboTipoControl.attr('data-tipo')) {
                            return "<button type='button' class='btn btn-primary VerControlCalidad' data-reporte='" + row.reporte + "' data-idAsigacion='" + row.idAsigancion + "' data-idEconomico='" + row.Economico + "'>" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                " </button>";
                        }
                        if (row.estatus == 3 && row.isrenta == true) {
                            return "<button type='button' class='btn btn-primary VerControlCalidad' data-reporte='" + row.reporte + "' data-idAsigacion='" + row.idAsigancion + "' data-idEconomico='" + row.Economico + "'>" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                " </button>";
                        }
                        if (row.estatus == 1 && row.isrenta == true) {
                            return "<button type='button' class='btn btn-primary VerControlCalidad' data-reporte='" + row.reporte + "' data-idAsigacion='" + row.idAsigancion + "' data-idEconomico='" + row.Economico + "'>" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                " </button>";
                        }
                        if (row.estatus == 7) {
                            return "<button type='button' class='btn btn-primary VerControlCalidad' data-reporte='" + row.reporte + "' data-idAsigacion='" + row.idAsigancion + "' data-idEconomico='" + row.Economico + "'>" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                " </button>";
                        }
                        if (row.estatus == 9) {
                            return "<button type='button' class='btn btn-danger AlertaResguardo'>" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                " </button>";
                        }

                    },
                    "VerReporte": function (column, row) {
                        //if (row.reporte != 0) {
                        if (row.reporte != 0 && "2" == cboFiltroEquipos.val()) {
                            return "<button type='button' class='btn btn-primary VerReporte' data-reporte='" + row.reporte + "' data-idAsigacion='" + row.idAsigancion + "'>" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                " </button>";
                        }

                    },
                    "VerControl": function (column, row) {
                        //if (row.reporte != 0) {
                        if (row.reporte != 0 && "2" == cboFiltroEquipos.val()) {
                            return "<button type='button' class='btn btn-primary VerControl' data-idAsigacion='" + row.idAsigancion + "' data-solicitudID='" + row.id + "'>" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                " </button>";
                        }
                    },
                    "DescargarArchivos": function (column, row) {
                        if (row.reporte != 0 && cboFiltroEquipos.val() == '2') {
                            return `
                                <button type='button' class='btn btn-primary DescargarArchivos' data-reporte='${row.reporte}' data-idAsigacion='${row.idAsigancion}' data-solicitudID='${row.id}' ${!row.contieneArchivos ? 'disabled' : ''}>
                                    <span class='glyphicon glyphicon-download-alt'></span>
                                </button>`;
                        }
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {

                //ajuste raguilar 07/012020
                if ("1" == cboFiltroEquipos.val()) {
                    $('#tblEquiposPendientes th:nth-child(' + 6 + ')').css('display', 'none');
                    $('#tblEquiposPendientes th:nth-child(' + 7 + ')').css('display', 'none');
                    $('#tblEquiposPendientes th:nth-child(' + 8 + ')').css('display', 'none');
                    $('#tblEquiposPendientes th:nth-child(' + 9 + ')').css('display', 'none');
                    var visible = $('#tblEquiposPendientes th[data-column-id="VerControlCalidad"]').data('visible');
                    if (visible == undefined || !visible) {
                        $('#tblEquiposPendientes th[data-column-id="VerReporte"]').css('display', 'none');
                    }
                    $('#tblEquiposPendientes tr').find("td:last").remove();
                    $('#tblEquiposPendientes tr').find("td:last").remove();
                    $('#tblEquiposPendientes tr').find("td:last").remove();
                } else {
                    $('#tblEquiposPendientes th[data-column-id="VerReporte"]').css('display', 'table-cell');
                    $('#tblEquiposPendientes th:nth-child(' + 6 + ')').show();
                    $('#tblEquiposPendientes th:nth-child(' + 7 + ')').show();
                    $('#tblEquiposPendientes th:nth-child(' + 8 + ')').show();
                    $('#tblEquiposPendientes th:nth-child(' + 9 + ')').show();
                }

                tblEquiposPendientes.find(".VerControlCalidad").on('click', function (e) {
                    var idAsignacion = $(this).attr('data-idAsigacion');
                    var CCal = $(this).attr('data-reporte');

                    if ($(this).attr('data-idEconomico') != 0) {
                        if ($("#cboFiltroEquipos").val() == 1) {
                            window.location = "/ControlCalidad/Preguntas?obj=" + idAsignacion + "&Tipo=" + cboTipoControl.attr('data-tipo') + "&CCal=" + CCal + "&N";
                        } else {
                            window.location = "/ControlCalidad/Preguntas?obj=" + idAsignacion + "&Tipo=" + cboTipoControl.attr('data-tipo') + "&CCal=" + CCal + "";
                        }

                    }

                    else {

                        GetEconomicosNoAsignados(idAsignacion);
                    }


                });

                tblEquiposPendientes.find(".VerReporte").on('click', function (e) {

                    var idAsignacion = $(this).attr('data-idAsigacion');

                    Imprimir(idAsignacion, cboTipoControl.attr('data-tipo'));

                });

                tblEquiposPendientes.find(".VerControl").on('click', function (e) {

                    var idAsignacion = $(this).attr('data-idAsigacion');
                    var solicitudID = $(this).attr('data-solicitudID');
                    GetDataIdReporte(idAsignacion, cboTipoControl.attr('data-tipo'), solicitudID);

                });

                tblEquiposPendientes.find(".DescargarArchivos").on('click', function (e) {
                    var idAsignacion = $(this).attr('data-idAsigacion');
                    var solicitudID = $(this).attr('data-solicitudID');
                    descargarArchivos(idAsignacion, solicitudID);
                });

                tblEquiposPendientes.find(".AlertaResguardo").on('click', function (e) {

                    AlertaGeneral("Alerta", "¡El equipo tiene un resguardo activo!<br/>Se debe liberar el equipo para poder hacer un control de envio");

                });


            });
            tblEconomicosNoAsignados.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                formatters: {
                    "Asignar": function (column, row) {

                        return "<button type='button' class='btn btn-success verSolicitud' data-idEconomico=" + row.idEconomico + " data-idAsignacion=" + row.idAsignacion + ">" +
                            "<span class='glyphicon glyphicon-plus'></span> " +
                            " </button>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblEconomicosNoAsignados.find(".verSolicitud").on('click', function (e) {
                    var idEconomico = $(this).attr("data-idEconomico");
                    var idAsignacion = $(this).attr('data-idAsignacion')

                    UpdateAsignacion(idEconomico, idAsignacion);
                });
            });
        }

        function GetDataIdReporte(idAsignacion, TipoControl, solicitudID) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/ControlCalidad/GetIDControl',
                type: 'POST',
                dataType: 'json',
                data: { asignacionID: idAsignacion, TipoControl: TipoControl, solicitudID: solicitudID },
                success: function (response) {
                    var id = response.ControlID;
                    var tipoControl = TipoControl;
                    var path = "/Reportes/Vista.aspx?idReporte=13&pidRegistro=" + id + "&ptipoControl=" + tipoControl;

                    ireport.attr("src", path);
                    document.getElementById('report').onload = function () {
                        $.unblockUI();
                        openCRModal();
                    };
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function descargarArchivos(idAsignacion, solicitudID) {
            location.href = `/ControlCalidad/DescargarArchivos?idAsignacion=${idAsignacion}&solicitudID=${solicitudID}`;
        }

        function UpdateAsignacion(idEconomico, idAsignacion) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/MovimientoMaquinaria/UpdateAsignacion',
                type: 'POST',
                dataType: 'json',
                data: { idEconomico: idEconomico, idAsignacion: idAsignacion },
                success: function (response) {


                    var idAsignacion = response.idAsignacion;
                    var CCal = 0;
                    modalListaEquiposAsignados.modal('hide');
                    ConfirmacionGeneral("Confirmacion", "Se asigno correctamente el economico al control de calidad.");
                    fillInfo();

                    // window.location = "/ControlCalidad/Preguntas?obj=" + idAsignacion + "&Tipo=" + cboTipoControl.attr('data-tipo') + "&CCal=" + CCal + "";

                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function Imprimir(asignacion, tipo) {
            verReporte(22, "idAsignacion=" + asignacion + "&" + "TipoControl=" + tipo);
        }

        function verReporte(idReporte, parametros) {

            $.blockUI({ message: mensajes.PROCESANDO });

            var idReporte = idReporte;

            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&" + parametros;
            ireport.attr("src", path);

            document.getElementById('report').onload = function () {

                $.unblockUI();
                openCRModal();

            };
        }

        iniciarGrid();
        init();
    };

    $(document).ready(function () {
        maquinaria.inventario.Calidad = new Calidad();
    });
})();
