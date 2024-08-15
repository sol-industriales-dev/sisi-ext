
(function () {

    $.namespace('maquinaria.Inventario.SustitucionMaquinaria');

    SustitucionMaquinaria = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };

        var ListaEconomicosCargados = [];
        var ListaEconomicosParaModificar = [];

        BtnCancelar = $("#BtnCancelar"),
        DivComentario = $("#DivComentario"),
        DivsendInfo = $("#DivsendInfo"),
        tbSingleDate = $("#tbSingleDate"),
        tbDescripcionCC = $("#tbDescripcionCC"),
        BntGuardarDocumento = $("#BntGuardarDocumento");
        modalDivtamaño = $("#modalDivtamaño"),
        btnGuardarSolicitud = $("#btnGuardarSolicitud"),
        ModalUploadFile = $("#ModalUploadFile"),
        btnRegresar = $("#btnRegresar"),
        tbFolioSolicitud = $("#tbFolioSolicitud");
        btnFolioSolicitud = $("#btnFolioSolicitud"),
        btnSiguiente1 = $("#btnSiguiente1"),
        btnGuardarComentarios = $("#btnGuardarComentarios"),
        modalListaMaquinaria = $("#modalListaMaquinaria"),
        btnEquipoAsignado = $("#btnEquipoAsignado"),
        tblListaEquiposRemplazo = $("#tblListaEquiposRemplazo"),
        tblEconomicosAsignados = $("#tblEconomicosAsignados"),
        tbDescripcion = $("#tbDescripcion"),
        ireport = $("#report")
        tbCC = $("#tbCC");

        mensajes = {
            NOMBRE: 'Asignacion de Equipo',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        function init() {

            tbSingleDate.datepicker().datepicker("setDate", new Date());
            tbCC.change(ChangeData);
            btnEquipoAsignado.click(LoadOpenModal);
            btnGuardarComentarios.click(updateComentarios);

            btnGuardarSolicitud.click(openmodal);
            btnSiguiente1.click(setSolicitud);
            btnRegresar.click(back);
            BntGuardarDocumento.click(SendGuardarInfo);


        }
        function back() {
            $('a[href^="#Step1"]').trigger('click');
        }

        function ChangeData() {
            getName(tbCC.val());
            fillTable();
        }

        function SendGuardarInfo() {
            BntGuardarDocumento.prop('disabled',true);
            var SolicitudRemplazo = GetInfoSet();
            listInput = $(".archivos");
            var estatus = true;
            $.each(listInput, function (index, value) {
                var files = document.getElementById($(value).attr('id')).files;
                validarCampo($(value));
                if (files.length == 0) {
                    estatus = false;
                }
            })

            if (estatus) {
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: '/SolicitudEquipo/SaveSolicitudReemplazo',
                    data: { obj: SolicitudRemplazo },
                    success: function (response) {
                        var idSolicitud = response.idSolicitud;
                        var Folio = response.folioSolicitud;
                        GuardarFiles(idSolicitud,Folio);
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });

            }
            else {

                AlertaGeneral('Alerta', 'Se debe tener un documento de respaldo para hacer la sustitucion.')
            }

        }

        function limpiarCampos() {
            tblEconomicosAsignados.bootgrid('clear')
            $('a[href^="#Step1"]').trigger('click');
            $('a[href^="#Step2"]').attr("disabled", "disabled");
            tbFolioSolicitud.val('');
            tbDescripcionCC.val('');
            tbCC.val('');
            tblListaEquiposRemplazo.bootgrid('clear');

            tbSingleDate.datepicker().datepicker("setDate", new Date());

            if (centro_costos != 0) {
                tbCC.val(centro_costos).prop('disabled', true);
                getName(tbCC.val());
                fillTable();
            }

        }

        function GetRowTabla(idAsignacio) {
            var ListaEquiposRemplazo = tblListaEquiposRemplazo.bootgrid('getCurrentRows');
            var Obj = $.grep(ListaEquiposRemplazo,
             function (o, i) { return o.id != Number(idAsignacio); },
            true);
            return Obj;
        }

        function GuardarFiles(idSolicitud, Folio) {
            var arrys = [];
            listInput = $(".archivos");
            var countData = listInput.length;
            $.each(listInput, function (index, value) {
                $.blockUI({ message: mensajes.PROCESANDO });
                var formData = new FormData();
                var id = $(value).attr('data-id');
                RowSolicitud = GetRowTabla(id);

                formData.append("idSolicitud", JSON.stringify(idSolicitud));
                formData.append("CC", JSON.stringify(tbCC.val()));
                formData.append("RowSolicitud", JSON.stringify(RowSolicitud));

                var files = document.getElementById($(value).attr('id')).files;

                $.each(files, function (i, file) {
                    formData.append('fupAdjunto[]', file);
                });

                $.ajax({
                    type: "POST",
                    url: '/SolicitudEquipo/GuardarDetalleSolicitudReemplazo',
                    data: formData,
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        countData -= 1;

                        if (countData == 0) {
                            BntGuardarDocumento.prop('disabled',false);
                            ConfirmacionGeneralAccion("Confirmación", "La solicitud Fue Creada Correctamente. Folio " + Folio);
                            $.unblockUI();
                        }
                    },
                    error: function (error) {
                        BntGuardarDocumento.prop('disabled',false);
                        $.unblockUI();
                        AlertaGeneral("Alerta", error);
                    }
                });
            });


        }
        function SendAlerta() {

        }

        function GetListaEconomicos() {
            var ListaEquiposRemplazo = tblListaEquiposRemplazo.bootgrid('getCurrentRows');
            return ListaEquiposRemplazo;

        }

        function GetInfoSet() {
            return {
                id: 0,
                folio: $("#tbFolioSolicitud").val(),
                CC: $("#tbCC").val(),
                descripcion: $("#tbComentario").val(),

            }
        }

        function getName(obj) {
            $.ajax({
                url: '/SolicitudEquipo/GetInfoSolicitud',
                type: 'POST',
                data: { obj: obj },
                success: function (response) {
                    if (response.success != 'False') {
                        if (response.descripcionCC != '') {
                            tbDescripcionCC.val(response.descripcionCC);
                            tbFolioSolicitud.val(response.folioReemplazo);
                        }
                        else {
                            tbDescripcionCC.val();
                            tbFolioSolicitud.val();
                        }
                    }
                    else if (response.message != '') {
                        AlertaGeneral('Error', response.message);
                    }
                },
                error: function (response) {
                }
            });
        };
        function openmodal() {
            //SetInfoToSave();
            //ModalUploadFile.modal('show');

            modalDivtamaño.addClass('modal-lg-large');
            DivComentario.addClass('hide');
            DivsendInfo.removeClass('hide');
            $('th[data-column-id="Archivo"]').removeClass('hide');
            $('.archivos').parent('td').removeClass('hide');
            modalListaMaquinaria.modal('show');

        }
        function setSolicitud() {
            var ListaEquiposRemplazo = tblListaEquiposRemplazo.bootgrid('getCurrentRows');
            if (ListaEquiposRemplazo.length == 0) {
                $('a[href^="#Step1"]').trigger('click');
                $('a[href^="#Step2"]').attr("disabled", "disabled");
                AlertaGeneral('Alerta', "Debe seleccionar por lo menos un equipo para poder continuar");
            }
            else {
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: '/SolicitudEquipo/SetRptSustitucion',
                    data: { obj: ListaEquiposRemplazo, CC: tbCC.val(), Folio: tbFolioSolicitud.val()},
                    success: function (response) {
                        $.unblockUI();
                        var idReporte = "34";
                        var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&pCC=" + tbCC.val();

                        ireport.attr("src", path);
                        document.getElementById('report').onload = function () {
                            $.unblockUI();
                        };
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
        }
        function LoadOpenModal() {
            modalDivtamaño.removeClass('modal-lg-large');
            DivComentario.removeClass('hide');
            DivsendInfo.addClass('hide');
            $('th[data-column-id="Archivo"]').addClass('hide');
            $('.archivos').parent('td').addClass('hide');
            modalListaMaquinaria.modal('show');
        }

        function fillTable() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/SolicitudEquipo/GetListaEquiposAsignados',
                data: { obj:tbCC.val() },
                success: function (response) {
                    $.unblockUI();
                    var data = response.tbllist;
                    ListaEconomicosCargados = data;
                    tblEconomicosAsignados.bootgrid("clear");
                    tblEconomicosAsignados.bootgrid("append", data);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function initGrid() {
            tblEconomicosAsignados.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: "",
                    footer: ""
                },
                formatters: {
                    "FechaInicio": function (column, row) {
                        return "<input type='text' class='form-control tbFechaInicio'  data-id='" + row.id + "' value='" + row.FechaInicio + "'>"
                    },
                    "FechaFin": function (column, row) {
                        return "<input type='text' class='form-control tbFechaFin' data-id='" + row.id + "' value='" + row.FechaFin + "'>"
                    },
                    "Remplazar": function (column, row) {

                        return "<button type='button' class='btn btn-success ChangeMaquina' data-id='" + row.id + "'>" +
                                                  "<span class='glyphicon glyphicon-plus'></span> " +
                                                         " </button>";
                    }

                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                $(".tbFechaInicio").datepicker();
                $(".tbFechaFin").datepicker();
                tblEconomicosAsignados.find(".ChangeMaquina").on("click", function (e) {

                    var idAsignacio = $(this).attr('data-id');
                    var row = $(this).parent().parent();
                    var fechaInicio = row.find('.tbFechaInicio');
                    var fechaFin = row.find('.tbFechaFin');
                    var Lista = tblEconomicosAsignados.bootgrid('getCurrentRows');
                    var ListaAgregados = tblListaEquiposRemplazo.bootgrid('getCurrentRows');

                    var Obj = $.grep(Lista,
                     function (o, i) { return o.id != Number(idAsignacio); },
                    true);

                    var ListaTemp = $.grep(Lista,
                        function (o, i) { return o.id == Number(idAsignacio); },
                    true);
                    Obj[0].FechaInicio = fechaInicio.val();
                    Obj[0].FechaFin = fechaFin.val();
                    ListaAgregados.push(Obj[0]);
                    //Grid Del Modal
                    tblListaEquiposRemplazo.bootgrid("clear");
                    tblListaEquiposRemplazo.bootgrid("append", ListaAgregados);

                    //Grid de la pantalla
                    tblEconomicosAsignados.bootgrid("clear");
                    tblEconomicosAsignados.bootgrid("append", ListaTemp);
                    ConfirmacionGeneral("Confirmación", "El economico seleccionado esta listo para su sustitución.");

                });

            });

            tblListaEquiposRemplazo.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',

                templates: {
                    header: ""
                },
                formatters: {
                    "RemoveEquipo": function (column, row) {

                        return "<button type='button' class='btn btn-danger RemoveEquipo' data-id='" + row.id + "'>" +
                                                  "<span class='glyphicon glyphicon-remove'></span> " +
                                                         " </button>";
                    },
                    "AddComentario": function (column, row) {
                        return "<input type=\"text\" class=\"ComentarioEquipo form-control\" value='" + (row.Comentario != undefined ? row.Comentario : "") + "' data-id='" + row.id + "' >";
                    },
                    "Archivo": function (column, row) {

                        return "<input type='file' class='archivos' data-id='" + row.id + "'  id='" + row.Economico + "' />";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblListaEquiposRemplazo.find(".RemoveEquipo").on("click", function (e) {

                    var idAsignacio = $(this).attr('data-id');

                    var Lista = tblEconomicosAsignados.bootgrid('getCurrentRows');
                    var ListaAgregados = tblListaEquiposRemplazo.bootgrid('getCurrentRows');

                    var Obj = $.grep(ListaAgregados,
                     function (o, i) { return o.id != Number(idAsignacio); },
                    true);

                    var ListaTemp = $.grep(ListaAgregados,
                        function (o, i) { return o.id == Number(idAsignacio); },
                    true);

                    Lista.push(Obj[0]);
                    //Grid Del Modal
                    tblListaEquiposRemplazo.bootgrid("clear");
                    tblListaEquiposRemplazo.bootgrid("append", ListaTemp);

                    //Grid de la pantalla
                    tblEconomicosAsignados.bootgrid("clear");
                    tblEconomicosAsignados.bootgrid("append", Lista);

                    ConfirmacionGeneral("Confirmación", "Se ha removido el economico deseado de esta lista.");
                });

                tblListaEquiposRemplazo.find(".ComentarioEquipo").on("change", function (e) {

                    var idAsignacio = $(this).attr('data-id');

                    var listaComentarios = $('.ComentarioEquipo');
                    var ListaEquiposRemplazo = tblListaEquiposRemplazo.bootgrid('getCurrentRows');
                    $.each(listaComentarios, function (index, value) {
                        $.each(ListaEquiposRemplazo, function (i, v) {
                            if ($(value).attr('data-id') == v.id) {
                                v.Comentario = $(value).val();
                            }
                        });
                    });
                });

            });
        }

        function updateComentarios() {
            var listaComentarios = $('.ComentarioEquipo');
            var ListaEquiposRemplazo = tblListaEquiposRemplazo.bootgrid('getCurrentRows');
            $.each(listaComentarios, function (index, value) {
                $.each(ListaEquiposRemplazo, function (i, v) {
                    if (Number($(value).attr('data-id')) == v.id) {
                        v.Comentario = $(value).val();
                    }
                });
            });

            ConfirmacionGeneral("Confirmación", "los comentarios fueron añadidos correctamente.");
        }

        function editarFolio() {
            if (tbFolioSolicitud.is(':enabled')) {
                tbFolioSolicitud.prop("disabled", true);
                cboFolio.addClass('hide');
                tbFolioSolicitud.removeClass('hide');
                var Folio = tbFolioSolicitud.val();
                var patt = new RegExp("[0-9]+-[-0-9]*");

                if (tbFolioSolicitud != "" && patt.test(Folio)) {
                    LOADDataSolicitud();
                }
            }
            else {
                if (tbDescripcionCC.val() != "") {
                    tbFolioSolicitud.prop("disabled", false);
                    tbFolioSolicitud.addClass('hide');
                    cboFolio.removeClass('hide');
                    cboFolio.fillCombo('/SolicitudEquipo/FillCboSolicitudesCambio', { CC: tbCC.val() });
                }
                else {
                    ConfirmacionGeneral("Alerta", "Debe seleccionar un centro de costos primero");
                }
            }
        }
        initGrid();
        init();
    };

    $(document).ready(function () {
        maquinaria.Inventario.SustitucionMaquinaria = new SustitucionMaquinaria();
    });
})();

