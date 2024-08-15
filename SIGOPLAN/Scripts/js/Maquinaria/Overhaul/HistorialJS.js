(function () {

    $.namespace('maquinaria.overhaul.historial');

    historial = function () {

        const modalEliminarArchivo = $("#modalEliminarArchivo");
        const lblEliminarArchivo = $("#lblEliminarArchivo");
        const btnEliminarArchivo = $("#btnEliminarArchivo");
        const btnEliminarCancelarArchivo = $("#btnEliminarCancelarArchivo");

        const txtPersonal = $("#txtPersonal");

        mensajes = {
            NOMBRE: 'Componentes',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        //Historial
        txtComponente = $("#txtComponente"),
            cboConjunto = $("#cboConjunto"),
            cboSubconjunto = $("#cboSubconjunto"),
            cboTipoLocacion = $("#cboTipoLocacion"),
            txtLocacion = $("#txtLocacion"),
            txtFechaInicio = $("#txtFechaInicio"),
            txtFechaFin = $("#txtFechaFin"),
            cboGrupo = $("#cboGrupo"),
            cboModelo = $("#cboModelo"),
            btnBuscar = $("#btnBuscar"),
            tblHistorial = $("#tblHistorial");
        cboConjunto.select2();
        cboSubconjunto.select2();
        cboGrupo.select2();
        cboModelo.select2();

        //Modal Archivo
        modalArchivo = $("#modalArchivo"),
            btnCargarArchivo = $("#btnCargarArchivo"),
            inCargarArchivo = $("#inCargarArchivo"),
            tblArchivos = $("#tblArchivos");
        //Modal Reporte
        modalReporte = $("#modalReporte"),
            cboEmpresaResponsable = $("#cboEmpresaResponsable"),
            cboEmpresaInstalacion = $("#cboEmpresaInstalacion"),
            cboDestino = $("#cboDestino"),
            txtFecha = $("#txtFecha"),
            txtFechaInstalacionComponente = $("#txtFechaInstalacionComponente"),
            btnEnviarReporte = $("#btnEnviarReporte"),
            txtSerieComponenteInstalado = $("#txtSerieComponenteInstalado");

        function init() {
            cboGrupo.fillCombo('/Overhaul/FillCboGrupoMaquinaComponentes', { obj: 0 });
            cboModelo.fillCombo('/Overhaul/fillCboModelo', { idGrupo: -1 });
            cboGrupo.change(recargarModelo);
            cboConjunto.fillCombo('/CatComponentes/FillCboConjunto_Componente', { idModelo: -1 });
            cboConjunto.change(CargarSubconjuntos);
            IniciarTblHistorial();

            txtComponente.getAutocomplete(SelectComponente, null, '/Overhaul/getNoComponente');
            txtFechaInicio.datepicker().datepicker("setDate", new Date());
            txtFechaFin.datepicker().datepicker("setDate", new Date());
            btnBuscar.click(CargarTblHistorial);
            IniciarTblArchivos();

            //Reporte
            btnCargarArchivo.click(function (e) { e.preventDefault(); inCargarArchivo.click(); });
            inCargarArchivo.change(function (e) { SubirArchivo(e); });
            $("#cboMotivo").change(cargarDestino);
            cboEmpresaResponsable.change(habilitarPersonal);
            $("#txtPersonal").change(habilitarBtnAgregarEmp);
            $("#btnAgregarEmpleado").click(cargarEmpleado);
            iniciarGridPersonal();
            $("#cboGarantia").change(habilitarEmpresaResponsable);
            $("#cboGarantia").change(function (e) {
                txtPersonal.prop("disabled", false);
            });
            $("#inCargarImgRemovido").change(function () {
                leerURL(this, $("#imgRemovido"));
            });
            $("#inCargarImgInstalado").change(function () {
                leerURL(this, $("#imgInstalado"));
            });
            $("#btncargarImgRemovido").click(function () {
                $("#inCargarImgRemovido").click();
            });
            $("#btncargarImgInstalado").click(function () {
                $("#inCargarImgInstalado").click();
            });
            btnEnviarReporte.click(ValidarReporte);

            btnEliminarArchivo.click(function (e) {
                fncEliminarArchivo();
            });

            btnEliminarCancelarArchivo.click(function (e) {
                modalEliminarArchivo.modal("hide");
            });

            txtPersonal.getAutocomplete(funSelEmpleado, null, '/Overhaul/getEmpleadosRemocion');
        }

        function funSelEmpleado(event, ui) {
            txtPersonal.val(ui.item.value);
            txtPersonal.attr("data-index", ui.item.id);
            habilitarBtnAgregarEmp();
        }

        function CargarSubconjuntos() {
            if (cboConjunto.val() != null && cboConjunto.val() != "") {
                cboSubconjunto.fillCombo('/CatComponentes/FillCboSubConjunto_Componente', { idConjunto: cboConjunto.val(), idModelo: -1 });
                cboSubconjunto.attr('disabled', false);
            }
            else {
                cboSubconjunto.clearCombo();
                cboSubconjunto.attr('disabled', true);
            }
        }

        function SelectComponente(event, ui) { txtComponente.text(ui.item.noComponente); }
        function SelectNoComponenteInstalado(event, ui) { txtSerieComponenteInstalado.text(ui.item.noComponente); txtSerieComponenteInstalado.attr("data-index", ui.item.id) }

        function IniciarTblHistorial() {
            tblHistorial = $("#tblHistorial").DataTable({
                language: {
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
                },
                retrieve: true,
                searching: false,
                paging: false,
                scrollY: '50vh',
                scrollCollapse: true,
                autoWidth: false,
                select: {
                    style: 'os',
                    selector: 'td:first-child'
                },
                "drawCallback": function (settings, json) {
                    $("#tblHistorial .parcial").each(function () {
                        $(this).val($(this).attr("data-value"));
                    });

                    tblHistorial.on("click", ".archivo", function (e) {
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        cargarGridArchivosCRC($(this).attr("data-index"), tblArchivos);
                        btnCargarArchivo.attr("data-index", $(this).attr("data-index"));
                        btnCargarArchivo.attr("data-componenteID", $(this).attr("data-componenteID"));
                        modalArchivo.modal("show");
                    });
                    tblHistorial.on("click", ".reporte", function (e) {
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        btnEnviarReporte.attr("data-trackID", $(this).attr("data-index"));
                        LimpiarReporte();
                        cargarDatosRemocion($(this).attr("data-index"), $(this).attr("data-componenteID"), $(this).attr("data-fecha"));
                    });
                    tblHistorial.on("click", ".guardar", function (e) {
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        UpdateTracking($(this).attr("data-index"));
                    });
                },
                columns: [
                    { data: 'componente', title: 'Componente' },
                    { data: 'locacion', title: 'Locación' },
                    { data: 'fecha', title: 'Fecha Movimiento' },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            return '<input class="form-control claveCotizacion" type="text" value="' + row.cotizacion + '" autocomplete="off" data-index="' + row.id + '">';
                        },
                        title: 'Cotización'
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            return '<input class="form-control costo" type="number" value="' + row.costo + '" autocomplete="off" data-index="' + row.id + '">';
                        },
                        title: 'Costo'
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            //console.log(row.parcial);
                            return '<select class="form-control parcial" data-value="' + (row.parcial == "true" ? "1" : (row.parcial == "false" ? "0" : "")) + '" data-index="' + row.id + '" ' + (row.parcial == "" ? "disabled" : "") + '>' +
                                (row.parcial == "" ? "" : '<option value="1">Parcial</option><option value="0">General</option>') +
                                '</select>';
                        },
                        title: 'Parcial/General'
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            return '<input class="form-control folioRequisicion" type="text" value="' + row.requisicion + '" autocomplete="off" data-index="' + row.id + '">';
                        },
                        title: 'Requisición'
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            return '<input class="form-control OC" type="text" value="' + row.oc + '" autocomplete="off" data-index="' + row.id + '">';
                        },
                        title: 'Orden de Compra'
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            return '<input class="form-control factura" type="text" value="' + row.factura + '" autocomplete="off" data-index="' + row.id + '">';
                        },
                        title: 'Factura'
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            return '<button type="button" class="btn btn-primary reporte" data-index="' + row.id + '" data-componenteID="' + row.componenteID + '" data-fecha="' +
                                row.fechaRaw + '" ' + (row.tipo == false ? "disabled" : "") + '><span class="fa fa-file"></span>  </button>';
                        },
                        title: 'Agregar Reporte'
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            return '<button type="button" class="btn btn-primary archivo" data-index="' + row.id + '" ' + (row.tipo == false ? "disabled" : "") +
                                '><span class="fa fa-file-pdf"></span></button>';
                        },
                        title: 'Agregar Archivos'
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            return '<button type="button" align="center" class="btn btn-primary guardar" data-index="' + row.id + '" data-componenteID="' + row.componenteID + '"><span class="fa fa-save"></span>  </button>';
                        },
                        title: 'Guardar Cambios'
                    },
                ],
                "columnDefs": [
                    { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11] },
                    { "width": "5%", "targets": [5, 9, 10, 11] },
                ],
                "order": [[0, 'asc']],
            });
        }

        function CargarTblHistorial() {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/CargarTablaHistorial',
                datatype: "json",
                type: "POST",
                data: {
                    componente: txtComponente.val(),
                    subconjunto: cboSubconjunto.val(),
                    locacion: txtLocacion.val(),
                    fechaInicio: txtFechaInicio.val(),
                    fechaFin: txtFechaFin.val(),
                    grupo: cboGrupo.val(),
                    modelo: cboModelo.val()
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        tblHistorial.clear();
                        tblHistorial.rows.add(response.historial);
                        tblHistorial.draw();
                        tblHistorial.columns.adjust();
                    }
                },
                error: function (response) {
                    console.log(response);
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.MESSAGE);
                }
            });
        }

        function IniciarTblArchivos() {
            tblArchivos.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {
                    "fecha": function (column, row) {
                        var fecha = row.FechaCreacion.substring(0, 2) + "/" + row.FechaCreacion.substring(2, 4) + "/" + row.FechaCreacion.substring(4, 8);
                        return "<span class='estatus' title='" + fecha + ".'> " + fecha + " </span>";
                    },
                    "nombre": function (column, row) {
                        return "<span title='" + row.nombre + ".'> " + row.nombre + " </span>";
                    },
                    "eliminar": function (column, row) {
                        return "<button type='button' class='btn btn-danger eliminar' title='Eliminar archivo.' data-index='" + row.id + "'>" +
                            "<span class='glyphicon glyphicon-remove'></span></button>";
                    },
                    "descargar": function (column, row) {
                        return "<button type='button' class='btn btn-primary descargar' title='Descargar archivo.' data-index='" + row.id + "' >" +
                            "<span class='glyphicon glyphicon-ok'></span></button>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblArchivos.find(".eliminar").parent().css("text-align", "center");
                tblArchivos.find(".eliminar").parent().css("width", "3%");
                tblArchivos.find(".descargar").parent().css("text-align", "center");
                tblArchivos.find(".descargar").parent().css("width", "3%");

                tblArchivos.find(".eliminar").on("click", function (e) {
                    let idArchivo = $(this).attr("data-index");
                    let idComponente = btnCargarArchivo.attr("data-index");
                    let NombreArchivo = $(this).closest("tr").find("td").eq(1).html();

                    lblEliminarArchivo.html(NombreArchivo);
                    btnEliminarArchivo.attr("idArchivo", idArchivo);
                    btnEliminarArchivo.attr("idComponente", idComponente);
                    modalEliminarArchivo.modal("show");
                });
            });
        }

        function fncEliminarArchivo() {
            $.ajax({
                url: "/Overhaul/EliminarArchivoTrackComponentes",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    idArchivo: btnEliminarArchivo.attr("idArchivo"),
                    idComponente: btnEliminarArchivo.attr("idComponente"),
                }),
                success: function (response) {
                    if (!response.success) {
                        AlertaGeneral("Error", "Ocurrió un error al eliminar el archivo selccionado.");
                    }
                    else {
                        cargarGridArchivosCRC(btnCargarArchivo.attr("data-index"), tblArchivos);
                        modalEliminarArchivo.modal("hide");
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }

        function cargarGridArchivosCRC(idTrack, tabla) {
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: "/Overhaul/cargarGridArchivosCRC",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ idTrack: idTrack }),
                success: function (response) {
                    $.unblockUI();
                    tabla.bootgrid("clear");
                    tabla.bootgrid("append", response.archivos);
                    tabla.bootgrid('reload');
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function SubirArchivo(e) {
            e.preventDefault();
            if (document.getElementById("inCargarArchivo").files[0] != null) {
                var ext = document.getElementById("inCargarArchivo").files[0].name.match(/\.(.+)$/)[1];
                ext = ext.toLowerCase();
                if (ext == 'pdf') {
                    size = document.getElementById("inCargarArchivo").files[0].size;
                    if (size > 52428800) { AlertaGeneral("Alerta", "Archivo sobrepasa los 50MB"); }
                    else {
                        if (size <= 0) { AlertaGeneral("Alerta", "Archivo vacío"); }
                        else { guardarArchivo(e); }
                    }
                }
                else { AlertaGeneral("Alerta", "Sólo se aceptan archivos PDF"); }
            }
        }

        function guardarArchivo(e) {
            e.preventDefault();
            $.blockUI({ message: mensajes.PROCESANDO, baseZ: 2000 });
            var formData = new FormData();
            var request = new XMLHttpRequest();
            var file = document.getElementById("inCargarArchivo").files[0];
            formData.append("archivoCRC", file);
            formData.append("idTrack", btnCargarArchivo.attr("data-index"));
            if (file != undefined) { $.blockUI({ message: 'Cargando archivo... Espere un momento', baseZ: 2000 }); }
            $.ajax({
                type: "POST",
                url: '/Overhaul/GuardarArchivoCRC',
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (response) {
                    $.unblockUI();
                    cargarGridArchivosCRC(btnCargarArchivo.attr("data-index"), tblArchivos);
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", "Se encontró un error al tratar de guardar el archivo");
                }
            });
        }

        function UpdateTracking(index) {
            let cotizacion = $("input.claveCotizacion[data-index='" + index + "']").val().trim().toUpperCase();
            let costo = $("input.costo[data-index='" + index + "']").val().trim().toUpperCase();
            let parcial = $("select.parcial[data-index='" + index + "']").val() == null ? "" : $("select.parcial[data-index='" + index + "']").val().trim().toUpperCase();
            let requisicion = $("input.folioRequisicion[data-index='" + index + "']").val().trim().toUpperCase();
            let oc = $("input.OC[data-index='" + index + "']").val().trim().toUpperCase();
            let factura = $("input.factura[data-index='" + index + "']").val().trim().toUpperCase();
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Overhaul/GuardarTracking",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    idTracking: index,
                    cotizacion: cotizacion,
                    costo: costo,
                    parcial: parcial == "1",
                    requisicion: requisicion,
                    oc: oc,
                    factura: factura
                }),
                success: function (response) {
                    $.unblockUI();
                    if (response.exito)
                        AlertaGeneral("Exito", "Se han guardado los cambios con éxito");
                    else
                        AlertaGeneral("Alerta", "No se ha encontrado el registro solicitado");
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        //Reporte Remoción

        function cargarDatosRemocion(trackID, componenteID, fecha) {
            cboEmpresaResponsable.fillCombo('/Overhaul/fillCboLocacion', { tipoLocacion: 2 });
            $('#cboEmpresaResponsable option:first').after($('<option />', { "value": '0', text: 'CONSTRUPLAN' }));
            cboEmpresaInstalacion.fillCombo('/Overhaul/fillCboLocacion', { tipoLocacion: 2 });
            $('#cboEmpresaInstalacion option:first').after($('<option />', { "value": '0', text: 'CONSTRUPLAN' }));
            cboDestino.append("<option value=''>--Seleccione--</option>");

            txtFechaInstalacionComponente.datepicker().datepicker("setDate", new Date(fecha));

            $.blockUI({ message: mensajes.PROCESANDO, baseZ: 2000 });
            $.ajax({
                url: '/Overhaul/cargarDatosRemocionHistorial',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ idComponente: componenteID, trackID: trackID }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        $("#txtDescripcionComponente").val(response.remocion.descripcionComponente);
                        $("#txtNoEconomico").val(response.remocion.noEconomico);
                        $("#txtModelo").val(response.remocion.modelo);
                        $("#txtModelo").attr('data-info', response.remocion.idModelo);
                        $("#txtHoras").val(response.remocion.horas);
                        $("#txtHorasComponenteRemovido").val(response.remocion.horasComponenteRemovido);
                        $("#txtNumParteComponente").val(response.remocion.numParteComponente);
                        $("#txtSerieComponenteRemovido").val(response.remocion.serieComponenteRemovido);
                        $("#txtSerieMaquina").val(response.remocion.serieMaquina);
                        $("#txtCC").val(response.remocion.nombreCC);
                        $("#txtFechaInstalacion").val(response.remocion.fechaInstalacionRemovido);
                        $("#txtFechaInstalacion").attr("data_fechaNum", response.remocion.fechaInstalacionRemovidoRaw);
                        //txtUltimaReparacion.val(response.remocion.fechaUltimaReparacion)
                        $("#cboNoComponenteInstalado").fillCombo('/Overhaul/cargarCboComponenteInstalado', { idModelo: response.remocion.modeloID, idSubconjunto: response.remocion.subconjuntoID });
                        //$("#txtPersonal").fillCombo('/Overhaul/cargartxtPersonal', { cc: response.remocion.cc }); //AUTOCOMPLETAR
                        $("#txtFecha").val(response.remocion.fecha);
                        $("#txtFecha").attr("data_fechaNum", response.remocion.fechaNum);
                        $("#txtDescripcionComponente").attr("data-id", response.remocion.componenteRemovidoID);
                        $("#txtNoEconomico").attr("data-id", response.remocion.maquinaID);
                        $("#txtCC").attr("data_id", response.remocion.cc);
                        $("#txtDescripcionComponente").attr("data-garantia", response.remocion.garantia);
                        response.remocion.motivoID != "-1" ? $("#cboMotivo").val(response.remocion.motivoID) : $("#cboMotivo").val("");
                        cargarDestino();
                        response.remocion.destinoID != "-1" ? $("#cboDestino").val(response.remocion.destinoID) : $("#cboDestino").val("");
                        $("#txtacomentario").val(response.remocion.comentario);


                        $("#cboGarantia").val(response.remocion.garantia != null ? (response.remocion.garantia == true ? "1" : "0") : "");
                        $("#cboGarantia").change();
                        response.remocion.empresaResponsable == -1 ? $("#cboEmpresaResponsable").val("") : $("#cboEmpresaResponsable").val(response.remocion.empresaResponsable);
                        response.remocion.empresaInstala == -1 ? $("#cboEmpresaInstalacion").val("") : $("#cboEmpresaInstalacion").val(response.remocion.empresaInstala);
                        $("#cboEmpresaResponsable").change();
                        $("#imgInstalado").attr("src", response.remocion.imgInstalado);
                        $("#imgRemovido").attr("src", response.remocion.imgRemovido);
                        if (response.remocion.personal != null && response.remocion.personal != "") {
                            var arrPersonal = response.remocion.personal.split(',');
                            for (var i = 0; i < arrPersonal.length - 1; i++) {
                                var JSONINFO = [{ "usuarioID": i, "usuario": arrPersonal[i] }];
                                $("#gridPersonal").bootgrid("append", JSONINFO);
                            }
                        }
                        txtSerieComponenteInstalado.getAutocomplete(SelectNoComponenteInstalado, { modeloID: response.remocion.modeloID, subconjuntoID: response.remocion.subconjuntoID }, '/Overhaul/getNoComponenteReporte');
                        response.remocion.componenteInstaladoID != "-1" ? txtSerieComponenteInstalado.val(response.remocion.componenteInstalado) : txtSerieComponenteInstalado.val("");
                        response.remocion.componenteInstaladoID != "-1" ? txtSerieComponenteInstalado.attr("data-index", response.remocion.componenteInstaladoID) : txtSerieComponenteInstalado.attr("data-index", "");
                        btnEnviarReporte.attr("data-reporteID", response.remocion.folioReporte);
                        if (response.remocion.estatus > 1) {
                            $("#cboGarantia").prop("disabled", true);
                            $("#txtPersonal").prop("disabled", false);
                            $("#cboEmpresaResponsable").prop("disabled", true);
                            $("#btncargarImgRemovido").prop("disabled", true);
                            $("#txtacomentario").prop("disabled", true);
                            $("#cboMotivo").prop("disabled", true);
                            $("#cboDestino").prop("disabled", true);
                            if (response.remocion.estatus > 4) {
                                $("#txtFechaInstalacionComponente").prop("disabled", true);
                                $("#cboEmpresaInstalacion").prop("disabled", true);
                                $("#btncargarImgInstalado").prop("disabled", true);
                                $("#cboNoComponenteInstalado").prop("disabled", true);
                            }
                        }
                        else {
                            $("#cboGarantia").prop("disabled", false);
                            $("#txtPersonal").prop("disabled", true);
                            $("#cboEmpresaResponsable").prop("disabled", false);
                            $("#btncargarImgRemovido").prop("disabled", false);
                            $("#txtacomentario").prop("disabled", false);
                            $("#cboMotivo").prop("disabled", false);
                            $("#cboDestino").prop("disabled", false);
                            if (response.remocion.estatus > 4) {
                                $("#txtFechaInstalacionComponente").prop("disabled", false);
                                $("#cboEmpresaInstalacion").prop("disabled", false);
                                $("#btncargarImgInstalado").prop("disabled", false);
                                $("#cboNoComponenteInstalado").prop("disabled", false);
                            }
                        }
                        btnEnviarReporte.attr("data-estatus", response.remocion.estatus);
                        //$("#gridPersonal").empty();
                        modalReporte.modal("show");
                    }
                    else { AlertaGeneral("Alerta", response.message); }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function LimpiarReporte() {
            cboEmpresaResponsable.clearCombo();
            cboEmpresaInstalacion.clearCombo();
            cboDestino.clearCombo();
            txtFecha.datepicker().datepicker("setDate", new Date());
            txtFechaInstalacionComponente.datepicker().datepicker("setDate", new Date());
            $("#txtDescripcionComponente").val('');
            $("#txtNoEconomico").val('');
            $("#txtModelo").val('');
            $("#txtModelo").attr('data-info', '');
            $("#txtHoras").val('');
            $("#txtHorasComponenteRemovido").val('');
            $("#txtNumParteComponente").val('');
            $("#txtNoComponenteRemovido").val('');
            $("#txtSerieMaquina").val('');
            $("#txtCC").val('');
            $("#txtFechaInstalacion").val('');
            $("#txtFechaInstalacion").attr("data_fechaNum", '');
            $("#cboNoComponenteInstalado").clearCombo();
            $("#txtPersonal").val("");
            $("#txtFecha").val('');
            $("#txtFecha").attr('');
            $("#txtDescripcionComponente").attr("data-id", '');
            $("#txtNoEconomico").attr("data-id", '');
            $("#txtCC").attr("data_id", '');
            $("#cboMotivo").val('');
            $("#cboGarantia").val('');
            $("#cboDestino").val('');
            $("#gridPersonal").bootgrid("clear");
        }

        function cargarDestino() {
            if ($("#cboMotivo").val() == "" || $("#cboMotivo").val() == null) {
                cboDestino.clearCombo();
            }
            else {
                cboDestino.fillCombo('fillCboLocaciones', { idModelo: $("#txtModelo").attr('data-info'), tipoLocacion: $("#cboMotivo").val() });
            }
        }

        function habilitarPersonal() {
            if (cboEmpresaResponsable.val() == "0") {
                $("#txtPersonal").parent().parent().parent().parent().parent().parent().css("display", "block");
            }
            else {
                $("#txtPersonal").parent().parent().parent().parent().parent().parent().css("display", "none");
            }
        }

        function habilitarBtnAgregarEmp() {
            if ($("#txtPersonal").val() != "") {
                $("#btnAgregarEmpleado").prop("disabled", false);
            }
            else {
                $("#btnAgregarEmpleado").prop("disabled", true);
            }
        }

        function cargarEmpleado() {
            var JSONINFO = [{ "usuarioID": parseInt(txtPersonal.attr("data-index")), "usuario": txtPersonal.val() }];
            $("#gridPersonal").bootgrid("append", JSONINFO);
        }

        function iniciarGridPersonal() {
            $("#gridPersonal").bootgrid({
                rowCount: -1,
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "quitar": function (column, row) {
                        return "<button type='button' class='btn btn-danger quitar'  data-index='" + row.usuarioID + "'" + "data-usuario='" + row.usuario + "'>" +
                            "<span class='glyphicon glyphicon-remove'></span>  </button>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                $("#gridPersonal").find(".quitar").on('click', function (e) {
                    var rowID = parseInt($(this).parent().parent().attr('data-row-id'));
                    $("#gridPersonal").bootgrid("remove", [rowID]);
                })
            });;
            $("#gridPersonal").bootgrid("clear");
        }

        function habilitarEmpresaResponsable() {
            cboEmpresaResponsable.val("");
            cboEmpresaResponsable.change();
            if ($("#cboGarantia").val() == "1") {
                cboEmpresaResponsable.prop("disabled", false);
            }
            else {
                cboEmpresaResponsable.prop("disabled", true);
                if ($("#cboGarantia").val() == "0") {
                    cboEmpresaResponsable.val("0");
                    cboEmpresaResponsable.change();
                }
            }
        }
        function leerURL(input, imagen) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) { imagen.attr('src', e.target.result); }
                reader.readAsDataURL(input.files[0]);
            }
        }

        function ValidarReporte() {
            //var estadoReporte = btnEnviarReporte.attr("data-estatus");
            //if (estadoReporte < 3) {
            //var estado = 1;
            if ($("#cboMotivo").val() == null || $("#cboMotivo").val() == "") {
                AlertaGeneral("Error", "Se requiere especificar el motivo de la remoción");
                return 0;
            }
            if ($("#cboGarantia").val() == null || $("#cboGarantia").val() == "") {
                AlertaGeneral("Error", "Se requiere especificar si el componente está en garantía");
                return 0;
            }
            if ($("#cboDestino").val() == null || $("#cboDestino").val() == "") {
                AlertaGeneral("Error", "Se requiere especificar el destino del componente");
                return 0;
            }
            if (txtSerieComponenteInstalado.val() == null || txtSerieComponenteInstalado.val() == "") {
                AlertaGeneral("Error", "Se requiere especificar el componente instalado");
                return 0;
            }
            //}
            guardarReporte(5);
        }

        function guardarReporte(estado) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/guardarReporte',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ reporte: datosReporte(estado) }),
                success: function (response) {

                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);

                }
            });
        }
        function datosReporte(estado) {
            var personal = "";
            $('#gridPersonal tbody tr').each(function () {
                personal += $(this).find('td:eq(1)').text() + ",";
            });
            return {
                id: btnEnviarReporte.attr("data-reporteID"),
                fechaRemocion: txtFecha.val(),
                componenteRemovidoID: $("#txtDescripcionComponente").attr("data-id"),
                componenteInstaladoID: txtSerieComponenteInstalado.attr("data-index") == "" ? "-1" : txtSerieComponenteInstalado.attr("data-index"),
                maquinaID: $("#txtNoEconomico").attr("data-id"),
                areaCuenta: $("#txtCC").attr("data_id"),
                motivoRemocionID: $("#cboMotivo").val(),
                destinoID: $("#cboDestino").val(),
                comentario: $("#txtacomentario").val(),
                garantia: $("#cboGarantia").val() == "1" ? true : ($("#cboGarantia").val() == "0" ? false : null),//txtDescripcionComponente.attr("data-garantia") == null ? false : txtDescripcionComponente.attr("data-garantia"),
                empresaResponsable: $("#cboEmpresaResponsable").val(),
                personal: personal == "," ? "" : personal,
                imgComponenteRemovido: $("#imgRemovido").attr("src"),
                imgComponenteInstalado: $("#imgInstalado").attr("src"),
                empresaInstala: $("#cboEmpresaInstalacion").val() == "" ? "-1" : $("#cboEmpresaInstalacion").val(),
                fechaInstalacionCRemovido: $("#txtFechaInstalacion").val(),
                fechaInstalacionCInstalado: $("#txtFechaInstalacionComponente").val(),
                //fechaUltimaReparacion: txtUltimaReparacion.val() == "N/A" ? "" : txtUltimaReparacion.val(),
                estatus: estado,
                horasComponente: $("#txtHorasComponenteRemovido").val(),
                horasMaquina: $("#txtHoras").val(),
                trackID: btnEnviarReporte.attr("data-trackID")
            }
        }
        function recargarModelo() {
            if (cboGrupo.val() == "") {
                cboModelo.fillCombo('/Overhaul/fillCboModelo', { idGrupo: -1 });
            }
            else {
                cboModelo.fillCombo('/Overhaul/fillCboModelo', { idGrupo: cboGrupo.val() });
            }
        }

        init();
    };

    $(document).ready(function () {
        maquinaria.overhaul.historial = new historial();
    });
})();