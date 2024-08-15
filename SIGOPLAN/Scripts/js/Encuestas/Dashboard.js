var iframeDownload = $("#iframeDownload");
(function () {

    $.namespace('encuestas.dashboard');

    dashboard = function () {

        mensajes = {
            PROCESANDO: 'Procesando...'
        },
            _objPapel = {};
        _tipoGuardado = 1,
        _encuestaID = 0,
        _adminPermisosBotones = false,
        btnBuscar = $("#btnBuscar"),
        btnOpenEnviar = $("#btnOpenEnviar"),
        btnEnviar = $("#btnEnviar"),
        btnOpenTelefonica = $("#btnOpenTelefono"),
        btnOpenPapel = $('#btnOpenPapel'),
        btnEditar = $("#btnEditar"),
        btnVer = $("#btnVer"),
        cboEncuestas = $("#cboEncuestas"),
        txtFechaInicio = $("#txtFechaInicio"),
        txtFechaFin = $("#txtFechaFin"),
        usersInProyect = $(".usersInProyect"),
        btnDisplayUsers = $("#btnDisplayUsers"),
        newUser = $("#newUser"),
        tblData = $("#tblData"),
        btnExportar = $("#btnExportar"),
        btnExportarTodos = $("#btnExportarTodos"),

        divDatos = $("#divDatos"),
        btnImprimir = $("#btnImprimir"),
        txtAsunto = $("#txtAsunto"),
        divGrafica = $("#divGrafica"),
        fieldExp = $("#fieldExp"),

        selectMes = $("#selectMes"),
        selectMesYear = $("#selectMesYear"),
        selectTrimestre = $("#selectTrimestre"),
        selectTrimestreYear = $("#selectTrimestreYear"),
        selectSemestre = $("#selectSemestre"),
        selectSemestreYear = $("#selectSemestreYear"),
        selectYear = $("#selectYear"),

        btnExportarMes = $("#btnExportarMes"),
        btnExportarTri = $("#btnExportarTri"),
        btnExportarSem = $("#btnExportarSem"),
        btnExportarYear = $("#btnExportarYear"),
        cboDepartamentos = $("#cboDepartamentos");

        txtEmpresa = $("#txtEmpresa");
        txtCliente = $("#txtCliente");
        txtTelefonicaAsunto = $("#txtTelefonicaAsunto");

        btnAgregarUsuario = $("#btnAgregarUsuario");
        btnCancelarUsuario = $("#btnCancelarUsuario");

        btnGuardar = $("#btnGuardar");

        $("#dialogTelefonica").on({
            change: function () {
                $("#txtCliente").fillCombo('/Encuestas/Encuesta/FillClientes', { empresa: txtEmpresa.find('option:selected').text() }, false);
            }
        }, '#txtEmpresa');

        $("#dialogPapel").on({
            change: function () {
                $("#txtClientePapel").fillCombo('/Encuestas/Encuesta/FillClientes', { empresa: $('#txtEmpresaPapel').find('option:selected').text() }, false);
            }
        }, '#txtEmpresaPapel');


        /*Sugerencia de mejora.*/
        modalTituloEncuesta = $("#modalTituloEncuesta"),
            btnOpenCapEncuesta = $("#btnOpenCapEncuesta"),
            btnExportarRptCerteza = $("#btnExportarRptCerteza"),
            cboEncuestasRpt = $("#cboEncuestasRpt"),
            fechaIniRpt = $("#fechaIniRpt"),
            fechaFinRpt = $("#fechaFinRpt");

        function init() {

            btnExportarRptCerteza.click(fnLoadReporteCerteza);
            LoadFechas();
            btnOpenCapEncuesta.click(fnOpenCapturaEnc);
            fnDisableItems();

            txtEmpresa.fillCombo('/Encuestas/Encuesta/FillEmpresas', null, false);
            txtCliente.fillCombo('/Encuestas/Encuesta/FillClientes', { empresa: txtEmpresa.find('option:selected').text() }, false);

            $('#txtEmpresaPapel').fillCombo('/Encuestas/Encuesta/FillEmpresas', null, false);
            $('#txtClientePapel').fillCombo('/Encuestas/Encuesta/FillClientes', { empresa: $('#txtEmpresaPapel').find('option:selected').text() }, false);

            //fnCheckTelefonica();
            loadCboDepartamentos();
            loadCboEncuestas();
            loadCboEncuestasRpt();
            txtFechaInicio.datepicker().datepicker("setDate", new Date());
            txtFechaFin.datepicker().datepicker("setDate", new Date());
            btnBuscar.click(fnBuscar);
            btnOpenEnviar.click(fnOpenEnviar);
            btnOpenTelefonica.click(fnOpenCapTelefonica);
            btnOpenPapel.click(fnOpenCapPapel);
            btnEditar.click(fnEditar);
            btnEnviar.click(fnEnviar);

            btnAgregarUsuario.click(fnNuevoCliente);
            btnCancelarUsuario.click(fnCancelarCliente);

            $('#btnAgregarUsuarioPapel').click(fnNuevoClientePapel);
            $('#btnCancelarUsuarioPapel').click(fnCancelarClientePapel);

            btnGuardar.click(fnResponderEncuestaTelefonica);
            $('#btnGuardarPapel').click(fnResponderEncuestaPapel);

            cboEncuestas.change(function () {
                if ($(this).val() == '') {
                    fnDisableItems();
                    _encuestaID = 0;
                } else {
                    _encuestaID = $(this).val();
                    fnEnableItems();
                    // fnCheckTelefonica();
                }

                permisosUsuario();
            });

            usersInProyect.on("mouseenter mouseleave", ".userComponent", hoverUserFromProject);
            usersInProyect.on("mouseenter mouseleave", ".userDelete", hoverdelUserFromProject);
            usersInProyect.on("click", ".userComponent", getUserFromProject);
            usersInProyect.on("click", ".userDelete", delUserFromProject);
            newUser.click(newUserFromProjectInit);
            btnDisplayUsers.click(newUserFromProjectInit);
            newUser.autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '/Administrativo/FormatoCambio/getUsuarioSelectConCorreo',
                        dataType: 'json',
                        data: {
                            term: request.term,
                            encuesta: _encuestaID
                        },
                        success: function (data) {
                            response(data);
                        }
                    });
                },
                minLength: 0,
                select: newUserSearch
            }).autocomplete("instance")._renderItem = function (ul, item) {
                var t = item.label.replace(new RegExp('(' + this.term + ')', 'gi'), "<b>$1</b>");
                return $("<li>")
                    .data("item.autocomplete", item)
                    .append("<div>" + t + "</div>")
                    .appendTo(ul);
            };
            btnExportar.click(fbExportar);
            btnExportarTodos.click(fbExportarTodos);
            btnExportarMes.click(fbExportarMes);
            btnExportarTri.click(fbExportarTri);
            btnExportarSem.click(fbExportarSem);
            btnExportarYear.click(fbExportarYear);
            $("#printChart").click(function () {
                $("#myChart").get(0).toBlob(function (blob) {
                    saveAs(blob, "Grafica");
                });
            });

            $('#selectMes').val(new Date().getMonth() + 1);

            setAutoComplete();
            //loadCboEncuestasRpt();
        }

        const checkPermisosUsuario = (encuestaID) => $.post('/Encuestas/Encuesta/CheckPermisosUsuario', { encuestaID: encuestaID });

        function permisosUsuario() {
            if (cboEncuestas.val() != '' && cboEncuestas.val() != 'todos') {
                if (_adminPermisosBotones) {
                    $('#btnEditar').css('display', 'table-cell');
                    $('#btnOpenEnviar').css('display', 'table-cell');
                    $('#btnOpenTelefono').css('display', 'table-cell');
                    $('#btnOpenPapel').css('display', 'table-cell');
                } else {
                    checkPermisosUsuario(cboEncuestas.val()).done(function (response) {
                        if (response.success) {
                            let permisos = response.permisos;

                            if (permisos != null) {
                                if (permisos.editar) {
                                    $('#btnEditar').css('display', 'table-cell');
                                } else {
                                    $('#btnEditar').css('display', 'none');
                                }
                                if (permisos.enviar) {
                                    $('#btnOpenEnviar').css('display', 'table-cell');
                                } else {
                                    $('#btnOpenEnviar').css('display', 'none');
                                }
                                if (permisos.contestaTelefonica) {
                                    $('#btnOpenTelefono').css('display', 'table-cell');
                                } else {
                                    $('#btnOpenTelefono').css('display', 'none');
                                }
                                if (permisos.contestaPapel) {
                                    $('#btnOpenPapel').css('display', 'table-cell');
                                } else {
                                    $('#btnOpenPapel').css('display', 'none');
                                }
                            } else {
                                $('#btnEditar').css('display', 'none');
                                $('#btnOpenEnviar').css('display', 'none');
                                $('#btnOpenTelefono').css('display', 'none');
                                $('#btnOpenPapel').css('display', 'none');
                            }
                        }
                    });
                }
            } else {
                $('#btnEditar').css('display', 'none');
                $('#btnOpenEnviar').css('display', 'none');
                $('#btnOpenTelefono').css('display', 'none');
                $('#btnOpenPapel').css('display', 'none');
            }
        }

        function fnLoadReporteCerteza() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Encuestas/Encuesta/getRptCerteza2",
                data: { listaEncuestas: getValoresMultiples("#cboEncuestasRpt"), fechaInicio: fechaIniRpt.val(), fechaFinal: fechaFinRpt.val() },
                asyn: false,
                success: function (response) {

                    var url = '/Encuestas/Encuesta/getRptCertezaDownload';
                    download(url);
                },
                error: function () {
                    $.unblockUI();
                }
            });

        }

        function LoadFechas() {
            var now = new Date(),
                year = now.getYear() + 1900;
            month = now.getMonth() + 1;
            fechaIniRpt.datepicker().datepicker("setDate", "01/0" + month + "/" + year);
            fechaFinRpt.datepicker().datepicker("setDate", new Date());
        }

        function datePicker() {
            var now = new Date(),
                year = now.getYear() + 1900;
            $.datepicker.setDefaults($.datepicker.regional["es"]);
            var dateFormat = "dd/mm/yy",
                from = $("#fechaIni")
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
                to = $("#fechaFin").datepicker({
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

        function fnOpenCapTelefonica() {
            modalTituloEncuesta.text('Responder Encuesta Telefonica');
            _tipoGuardado = 1;
            fnOpenTelefonica();
        }

        function fnOpenCapPapel() {
            modalTituloEncuesta.text('Responder Encuesta Papel');
            _tipoGuardado = 3;
            fnOpenPapel();
        }

        function fnOpenCapturaEnc() {

            modalTituloEncuesta.text('Responder Encuesta');
            _tipoGuardado = 2;
            fnOpenTelefonica();

        }

        function fnCheckTelefonica() {

            if (_encuestaID.trim('') != "todos") {
                $.ajax({
                    url: "/Encuestas/Encuesta/checkTelefonica",
                    data: { encuestaID: _encuestaID },
                    success: function (response) {
                        if (response.flag == true) {
                            btnOpenTelefonica.css("display", "table-cell");
                        } else {
                            btnOpenTelefonica.css("display", "none");
                        }
                    }
                });
            }

        }

        function fnResponderEncuestaTelefonica() {
            if (txtEmpresa.val() != "" || $("#txtEmpresaCliente").length) {
                if ($("#txtCliente").length) {
                    if (!$("#txtNombreCliente").length && $("#txtCliente").val() != "") {
                        if (txtTelefonicaAsunto.val() != "") {
                            fnEnviarTelefonica(txtTelefonicaAsunto.val());
                        } else {
                            AlertaGeneral("Alerta", "Especifique un asunto para la encuesta.");
                        }
                    } else {
                        AlertaGeneral("Alerta", "Seleccione un cliente o cree uno nuevo.");
                    }
                } else {
                    if (!$("#txtCliente").length && $("#txtNombreCliente").val() != "" && $("#txtApePaternoCliente").val() != "" && $("#txtApeMaternoCliente").val() != "" && $("#txtCorreoCliente").val() != "" && $("#txtEmpresaCliente").val() != "") {
                        if (txtTelefonicaAsunto.val() != "") {
                            var nuevo = {
                                nombre: $("#txtNombreCliente").val(),
                                apellidoPaterno: $("#txtApePaternoCliente").val(),
                                apellidoMaterno: $("#txtApeMaternoCliente").val(),
                                correo: $("#txtCorreoCliente").val(),
                                //empresa: txtEmpresa.find('option:selected').text()
                                empresa: $("#txtEmpresaCliente").val()
                            };
                            fnEnviarTelefonica(txtTelefonicaAsunto.val(), nuevo);
                        } else {
                            AlertaGeneral("Alerta", "Especifique un asunto para la encuesta.");
                        }
                    } else {
                        AlertaGeneral("Alerta", "Llene todos los campos del cliente nuevo.");
                    }
                }
            } else {
                AlertaGeneral("Alerta", "Debe seleccionar una empresa.");
            }
        }

        function fnResponderEncuestaPapel() {
                if (!$('#txtEmpresaPapel').attr("disabled")) {
                    if ($("#txtClientePapel").val() != "") {
                        if ($('#txtTelefonicaAsuntoPapel').val() != "") {
                            fnEnviarPapel($('#txtTelefonicaAsuntoPapel').val());
                        } else {
                            AlertaGeneral("Alerta", "Especifique un asunto para la encuesta.");
                        }
                    } else {
                        AlertaGeneral("Alerta", "Seleccione un cliente o cree uno nuevo.");
                    }
                } else {
                    if (
                        $("#txtNombreClientePapel").val() != "" &&
                        $("#txtApePaternoClientePapel").val() != "" &&
                        $("#txtApeMaternoClientePapel").val() != "" &&
                        $("#txtCorreoClientePapel").val() != "" &&
                        $("#txtEmpresaClientePapel").val() != "") {
                        if ($('#txtTelefonicaAsuntoPapel').val() != "") {
                            var nuevo = {
                                nombre: $("#txtNombreClientePapel").val(),
                                apellidoPaterno: $("#txtApePaternoClientePapel").val(),
                                apellidoMaterno: $("#txtApeMaternoClientePapel").val(),
                                correo: $("#txtCorreoClientePapel").val(),
                                empresa: $("#txtEmpresaClientePapel").val()
                            };
                            fnEnviarPapel($('#txtTelefonicaAsuntoPapel').val(), nuevo);
                        } else {
                            AlertaGeneral("Alerta", "Especifique un asunto para la encuesta.");
                        }
                    } else {
                        AlertaGeneral("Alerta", "Llene todos los campos del cliente nuevo.");
                    }
                }

        }

        function fnEnviarTelefonica(asunto, nuevo) {
            var preguntas = $("#dialogTelefonica .starrr");
            var preguntasList = new Array();
            var validacion = false;
            $.each(preguntas, function (i, e) {
                var o = {};
                o.encuestaID = _encuestaID;
                //o.encuestaUsuarioID = _encuestaUsuarioID;
                o.usuarioRespondioID = $("#txtCliente").val() != "" ? $("#txtCliente").val() : 0;
                o.preguntaID = $(e).data("id");
                o.calificacion = $(e).attr("data-calificacion");
                o.respuesta = $('#dialogTelefonica').find('[data-id="txtRespuesta' + o.preguntaID + '"]').val()
                if (o.calificacion == 0) {
                    ConfirmacionGeneral("Confirmación", "¡No todas las preguntas han sido contestadas! Favor de responderlas.");
                    validacion = true;
                }
                else if (o.calificacion <= 3 && (o.respuesta == "" || o.respuesta == null || o.respuesta == undefined)) {
                    ConfirmacionGeneral("Confirmación", "¡No todas las preguntas han sido contestadas! Favor de responderlas.");
                    validacion = true;
                }
                preguntasList.push(o);
            });



            var file1 = document.getElementById("fupdateEvidencia").files[0];

            hadFile = (file1 != undefined ? false : true);

            if (hadFile) {
                sendSaveFile(file1);
                validacion = _objPapel.valida;
            }
            else {
                if (_tipoGuardado == 2) {
                    validacion = true;
                    ConfirmacionGeneral("Confirmación", "¡Subir archivo es requerido.");
                }
            }


            if (validacion == false) {
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/Encuestas/Encuesta/saveEncuestaTelefonica",
                    data: { obj: preguntasList, encuestaID: _encuestaID, comentario: $("#txtTelefonicaComentario").val(), asunto: asunto, nuevoCliente: nuevo },
                    asyn: false,
                    success: function (response) {
                        if (response.success) {
                            ConfirmacionGeneral("Confirmación", "¡Encuesta contestada correctamente!");

                            setInterval(function () {
                                var blob = $.urlParam('blob');
                                if (blob == null) {
                                    window.location.href = "/Encuestas/Encuesta/Dashboard";
                                }
                                else {
                                    window.location.href = "/Usuario/Login";
                                }

                            }, 2000);
                        } else {
                            AlertaGeneral('Alerta', response.message);
                        }
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }

        }

        function fnEnviarPapel(asunto, nuevo) {
            var preguntas = $("#dialogPapel .starrr");
            var preguntasList = new Array();
            var validacion = false;
            $.each(preguntas, function (i, e) {
                var o = {};
                o.encuestaID = _encuestaID;
                o.usuarioRespondioID = $("#txtClientePapel").val() != "" ? $("#txtClientePapel").val() : 0;
                o.preguntaID = $(e).data("id");
                o.calificacion = $(e).attr("data-calificacion");
                o.respuesta = $('#dialogPapel').find('[data-id="txtRespuesta' + o.preguntaID + '"]').val()
                if (o.calificacion == 0) {
                    ConfirmacionGeneral("Confirmación", "¡No todas las preguntas han sido contestadas! Favor de responderlas.");
                    validacion = true;
                }
                else if (o.calificacion <= 3 && (o.respuesta == "" || o.respuesta == null || o.respuesta == undefined)) {
                    ConfirmacionGeneral("Confirmación", "¡No todas las preguntas han sido contestadas! Favor de responderlas.");
                    validacion = true;
                }
                preguntasList.push(o);
            });

            var file1 = document.getElementById("fupdateEvidenciaPapel").files[0];

            hadFile = (file1 == undefined ? false : true);

            if (hadFile) {
                sendSaveFile(file1);
                validacion = _objPapel.valida;
            }
            else {
                if (_tipoGuardado == 3) {
                    validacion = true;
                    ConfirmacionGeneral("Confirmación", "¡Subir archivo es requerido.");
                }
            }

            if (validacion == false) {
                $.blockUI({
                    message: mensajes.PROCESANDO,
                    baseZ: 2000
                });
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/Encuestas/Encuesta/saveEncuestaPapel",
                    data: { obj: preguntasList, encuestaID: _encuestaID, comentario: $("#txtTelefonicaComentarioPapel").val(), asunto: asunto, nuevoCliente: nuevo,rutaArchivo:_objPapel.ruta },
                    asyn: false,
                    success: function (response) {
                        if (response.success) {
                            ConfirmacionGeneral("Confirmación", "¡Encuesta contestada correctamente!");

                            setInterval(function () {
                                var blob = $.urlParam('blob');
                                if (blob == null) {
                                    window.location.href = "/Encuestas/Encuesta/Dashboard";
                                }
                                else {
                                    window.location.href = "/Usuario/Login";
                                }

                            }, 2000);
                        } else {
                            AlertaGeneral('Alerta', response.message);
                        }
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }

        }

        function sendSaveFile(file1) {
            $.blockUI({
                message: 'Cargando archivo... ¡Espere un momento!',
                baseZ: 2000
            });
            const data = new FormData();
            data.append("fArchivoEvidencia", file1);

            $.ajax({
                type: "POST",
                url: '/Encuestas/Encuesta/SubirArchivoEvidencia',
                data: data,
                dataType: 'json',
                contentType: false,
                processData: false,
                async: false,
                success: function (response) {
                    if(response.success)
                    {
                        ConfirmacionGeneral("Error", "Ha ocurrido un error al intentar guardar el archivo. " + response.message);
                    }
                    else
                    {
                        _objPapel.ruta = response.ruta;
                    }
                    _objPapel.valida = response.success;                    
                    $.unblockUI();
                },
                error: function (error) {
                    _objPapel.valida = response.success;
                    ConfirmacionGeneral("Error", "Ha ocurrido un error al intentar guardar el archivo. " + response.message);
                    $.unblockUI();
                }
            });

        }

        function setAutoComplete() {
            //txtCliente.getAutocomplete(setIdEmpleado, null, '/Encuestas/Encuesta/getClientes?empresa=' + txtEmpresa.find('option:selected').text());
        }

        function setIdEmpleado(event, ui) {
            //txtCliente.data("usuarioID", ui.item.id);
        }

        function fnNuevoCliente() {
            txtEmpresa.attr("disabled", true);

            var row = $("#rowCliente");
            $(row).remove();

            var html = "";
            html += '<div id="rowCliente" class="row">';
            //html += '   <fieldset class="col-lg-12">';
            html += '       <div id="divTxtCliente" class="col-lg-9" style="padding-right: 0px; margin-top: 5px; margin-bottom: 5px;">';
            //html += '           <div class="input-group">';
            //html += '               <span class="input-group-addon">Nombre:</span>';
            html += '               <input id="txtNombreCliente" class="form-control" placeholder="Nombre" />'
            //html += '           </div>';
            html += '       </div>'
            html += '       <div id="divBotones" class="col-lg-3">';
            html += '           <div class="input-group pull-right" style="width: 100%;">';
            html += '               <button id="btnAgregarUsuario" class="btn btn-md btn-primary btn-nuevo" style="display: none;">Nuevo Cliente</button>';
            html += '               <button id="btnCancelarUsuario" class="btn btn-md btn-primary btn-cancelar">Cancelar</button>';
            html += '           </div>';
            html += '       </div>';
            html += '       <div class="col-lg-9" style="padding-right: 0px; margin-bottom: 5px;">';
            //html += '           <div class="input-group">';
            //html += '               <span class="input-group-addon">Apellido Paterno:</span>';
            html += '               <input id="txtApePaternoCliente" class="form-control" placeholder="Apellido Paterno" />'
            //html += '           </div>';
            html += '       </div>';
            html += '       <div class="col-lg-3">';
            html += '       </div>';
            html += '       <div class="col-lg-9" style="padding-right: 0px; margin-bottom: 5px;">';
            //html += '           <div class="input-group">';
            //html += '               <span class="input-group-addon">Apellido Materno:</span>';
            html += '               <input id="txtApeMaternoCliente" class="form-control" placeholder="Apellido Materno" />'
            //html += '           </div>';
            html += '       </div>';
            html += '       <div class="col-lg-3">';
            html += '       </div>';
            html += '       <div class="col-lg-9" style="padding-right: 0px; margin-bottom: 5px;">';
            //html += '           <div class="input-group">';
            //html += '               <span class="input-group-addon">Correo:</span>';
            html += '               <input id="txtCorreoCliente" class="form-control" placeholder="Correo" />'
            //html += '           </div>';
            html += '       </div>';
            html += '       <div class="col-lg-3">';
            html += '       </div>';

            if (txtEmpresa.val() != "") {
                html += '       <div class="col-lg-9" style="padding-right: 0px; margin-bottom: 5px;">';
                //html += '           <div class="input-group">';
                //html += '               <span class="input-group-addon">Empresa:</span>';
                html += '               <input id="txtEmpresaCliente" class="form-control" placeholder="Empresa" value="' + txtEmpresa.find('option:selected').text() + '" />'
                //html += '           </div>';
                html += '       </div>';
            } else {
                html += '       <div class="col-lg-9" style="padding-right: 0px; margin-bottom: 5px;">';
                //html += '           <div class="input-group">';
                //html += '               <span class="input-group-addon">Empresa:</span>';
                html += '               <input id="txtEmpresaCliente" class="form-control" placeholder="Empresa" />'
                //html += '           </div>';
                html += '       </div>';
            }

            html += '       <div class="col-lg-3">';
            html += '       </div>';
            //html += '   </fieldset>';
            html += '</div>';

            $("#rowAsunto").before(html);

            $("#btnCancelarUsuario").click(fnCancelarCliente);
        }

        function fnNuevoClientePapel() {
            $('#txtEmpresaPapel').attr("disabled", true);

            var row = $("#rowClientePapel");
            $(row).remove();

            var html = "";

            html += '<div id="rowClientePapel" class="row">';
            html += '       <div id="divTxtClientePapel" class="col-lg-9" style="padding-right: 0px; margin-top: 5px; margin-bottom: 5px;">';
            html += '               <input id="txtNombreClientePapel" class="form-control" placeholder="Nombre" />';
            html += '       </div>'
            html += '       <div id="divBotonesPapel" class="col-lg-3">';
            html += '           <div class="input-group pull-right" style="width: 100%;">';
            html += '               <button id="btnAgregarUsuarioPapel" class="btn btn-md btn-primary btn-nuevo" style="display: none;">Nuevo Cliente</button>';
            html += '               <button id="btnCancelarUsuarioPapel" class="btn btn-md btn-primary btn-cancelar">Cancelar</button>';
            html += '           </div>';
            html += '       </div>';
            html += '       <div class="col-lg-9" style="padding-right: 0px; margin-bottom: 5px;">';
            html += '               <input id="txtApePaternoClientePapel" class="form-control" placeholder="Apellido Paterno" />';
            html += '       </div>';
            html += '       <div class="col-lg-3">';
            html += '       </div>';
            html += '       <div class="col-lg-9" style="padding-right: 0px; margin-bottom: 5px;">';
            html += '               <input id="txtApeMaternoClientePapel" class="form-control" placeholder="Apellido Materno" />'
            html += '       </div>';
            html += '       <div class="col-lg-3">';
            html += '       </div>';
            html += '       <div class="col-lg-9" style="padding-right: 0px; margin-bottom: 5px;">';
            html += '               <input id="txtCorreoClientePapel" class="form-control" placeholder="Correo" />';
            html += '       </div>';
            html += '       <div class="col-lg-3">';
            html += '       </div>';

            if (txtEmpresa.val() != "") {
                html += '       <div class="col-lg-9" style="padding-right: 0px; margin-bottom: 5px;">';
                html += '               <input id="txtEmpresaClientePapel" class="form-control" placeholder="Empresa" value="' + txtEmpresa.find('option:selected').text() + '" />'
                html += '       </div>';
            } else {
                html += '       <div class="col-lg-9" style="padding-right: 0px; margin-bottom: 5px;">';
                html += '               <input id="txtEmpresaClientePapel" class="form-control" placeholder="Empresa" />'
                html += '       </div>';
            }

            html += '       <div class="col-lg-3">';
            html += '       </div>';
            html += '</div>';

            $("#rowAsuntoPapel").before(html);

            $("#btnCancelarUsuarioPapel").click(fnCancelarClientePapel);
        }

        function fnCancelarCliente() {
            txtEmpresa.attr("disabled", false);

            var row = $("#rowCliente");
            $(row).remove();

            var html = "";
            html += '<div id="rowCliente" class="row">';
            html += '   <div id="divTxtCliente" class="col-lg-9" style="padding-right: 0px; margin-top: 5px; margin-bottom: 5px;">';
            html += '       <select id="txtCliente" class="form-control"></select>';
            html += '   </div>';
            html += '   <div id="divBotones" class="col-lg-3">';
            html += '       <div class="input-group pull-right" style="width: 100%;">';
            html += '           <button id="btnAgregarUsuario" class="btn btn-md btn-primary btn-nuevo">Nuevo Cliente</button>';
            html += '           <button id="btnCancelarUsuario" class="btn btn-md btn-primary btn-cancelar" style="display: none;">Cancelar</button>';
            html += '       </div>';
            html += '   </div>';
            html += '</div>';

            $("#rowAsunto").before(html);

            $("#btnAgregarUsuario").click(fnNuevoCliente);

            txtEmpresa.change();
        }

        function fnCancelarClientePapel() {
            $('#txtEmpresaPapel').attr("disabled", false);

            var row = $("#rowClientePapel");
            $(row).remove();

            var html = "";
            html += '<div id="rowClientePapel" class="row">';
            html += '   <div id="divTxtClientePapel" class="col-lg-9" style="padding-right: 0px; margin-top: 5px; margin-bottom: 5px;">';
            html += '       <select id="txtClientePapel" class="form-control"></select>';
            html += '   </div>';
            html += '   <div id="divBotonesPapel" class="col-lg-3">';
            html += '       <div class="input-group pull-right" style="width: 100%;">';
            html += '           <button id="btnAgregarUsuarioPapel" class="btn btn-md btn-primary btn-nuevo">Nuevo Cliente</button>';
            html += '           <button id="btnCancelarUsuarioPapel" class="btn btn-md btn-primary btn-cancelar" style="display: none;">Cancelar</button>';
            html += '       </div>';
            html += '   </div>';
            html += '</div>';

            $("#rowAsuntoPapel").before(html);

            $("#btnAgregarUsuarioPapel").click(fnNuevoClientePapel);

            $('#txtEmpresaPapel').change();
        }

        function fnEnviar() {
            $.blockUI({ message: mensajes.PROCESANDO });
            var encuestados = getUserFromProject();
            if (txtAsunto.val() != '') {
                if (encuestados.length > 0) {
                    $.ajax({
                        datatype: "json",
                        type: "POST",
                        url: "/Encuestas/Encuesta/sendEncuesta",
                        data: { asunto: txtAsunto.val(), listaEnviar: encuestados },
                        asyn: false,
                        success: function (response) {
                            $("#dialogEnviar .close").click();
                            ConfirmacionGeneral("Confirmación", "¡Encuesta enviada correctamente!");
                            $.unblockUI();
                        },
                        error: function () {
                            $.unblockUI();
                        }
                    });

                }
                else {
                    $.unblockUI();
                    ConfirmacionGeneral("Alerta", "¡Debe agregar almenos un usuario!");
                }
            }
            else {
                $.unblockUI();
                ConfirmacionGeneral("Alerta", "¡Debe poner un asunto del envio de encuesta!");
            }
        }

        function fnTelefonica() {
            $.blockUI({ message: mensajes.PROCESANDO });
            var encuestados = getUserFromProject();
            if (txtAsunto.val() != '') {
                if (encuestados.length > 0) {
                    //$.ajax({
                    //    datatype: "json",
                    //    type: "POST",
                    //    url: "/Encuestas/Encuesta/sendEncuesta",
                    //    data: { asunto: txtAsunto.val(), listaEnviar: encuestados },
                    //    asyn: false,
                    //    success: function (response) {
                    //        $("#dialogEnviar .close").click();
                    //        ConfirmacionGeneral("Confirmación", "¡Encuesta enviada correctamente!");
                    //        $.unblockUI();
                    //    },
                    //    error: function () {
                    //        $.unblockUI();
                    //    }
                    //});
                }
                else {
                    $.unblockUI();
                    ConfirmacionGeneral("Alerta", "¡Debe agregar almenos un usuario!");
                }
            }
            else {
                $.unblockUI();
                ConfirmacionGeneral("Alerta", "¡Debe poner un asunto del envio de encuesta!");
            }
        }

        function hoverUserFromProject(e) {
            var _this = $(this);

            if (e.type === "mouseenter") {
                _this.parent().find(".userFill").css({ 'background-color': '#d3d3d3' });
                _this.parent().find(".userDelete").css({ 'background-color': '#d3d3d3' });
            }
            else if (e.type === "mouseleave") {
                _this.parent().find(".userFill").css({ 'background-color': '#e0e0e0' });
                _this.parent().find(".userDelete").css({ 'background-color': '#e0e0e0' });
            }
        }
        function hoverdelUserFromProject(e) {
            var _this = $(this);

            if (e.type === "mouseenter") {
                _this.parent().find(".userDelete").css({ 'background-color': '#bdbdbd' });
            }
            else if (e.type === "mouseleave") {
                _this.parent().find(".userDelete").css({ 'background-color': '#e0e0e0' });
            }
        }
        function getUserFromProject() {
            var obj = new Array();
            usersInProyect.find(".userComponent").each(function (i, e) {
                var o = {};
                o.encuestaID = _encuestaID;
                o.encuestaNombre = $("#cboEncuestas option:selected").text();
                o.encuestadoID = $(e).data("userid");
                obj.push(o);
            });
            return obj;
        }

        function delUserFromProject() {
            var _this = $(this);
            var u = _this.parent().find(".userComponent");
            _this.parent().parent().remove();
        }

        function newUserFromProjectInit() {
            newUser.focus();
            newUser.autocomplete("search");
        }
        function newUserSearch(event, ui) {
            var html = '<div class="divUser">';
            html += '    <div class="userContainer">';
            html += '        <span class="userFill">&nbsp;</span>';
            html += '        <span class="userComponent" data-user="' + ui.item.value + '" data-userid="' + ui.item.id + '">';
            html += ui.item.value;
            html += '        </span>';
            html += '        <button type="button" class="userDelete">&nbsp;X</button>';
            html += '    </div>';
            html += '</div>';
            $(html).insertBefore(newUser);
            newUser.focus();

            ui.item.value = "";  // it will clear field 
            newUser.autocomplete('close').val('');
            return false;
        }
        function fnBuscar() {
            if (cboEncuestas.val() != '') {
                if (cboEncuestas.val() == 'todos') {
                    fieldExp.show();
                    btnExportar.hide();
                    divDatos.hide();
                    divGrafica.hide();
                }
                else {
                    loadGrid(getFiltrosObject(), '/Encuestas/Encuesta/getEncuestaResults', tblData);

                    loadGraph(getFiltrosObject(), '/Encuestas/Encuesta/getEncuestaGrafica', "myChart");
                    $("#printChart").show();
                    btnExportar.show();

                    fieldExp.hide();
                    divDatos.show();
                    divGrafica.show();
                }
            }
            else {
                divDatos.show();
                divGrafica.show();
                $("#printChart").hide();
                btnExportar.hide();

                fieldExp.hide();
                ConfirmacionGeneral("Alerta", "¡Debe seleccionar una encuesta!", "bg-red");
            }
        }
        function getFiltrosObject() {
            return {
                id: cboEncuestas.val(),
                fechaInicio: txtFechaInicio.val(),
                fechaFin: txtFechaFin.val()
            }
        }
        function fnEditar() {
            var soloLectura = cboEncuestas.find(':selected').data('sololectura');
            if (soloLectura == "1") {
                AlertaGeneral("Alerta", "¡Esta encuesta esta marcada como solo lectura!");
            }
            else {
                window.location.href = "/Encuestas/Encuesta/Crear/?encuesta=" + cboEncuestas.val();
            }
        }
        function fnOpenEnviar() {

            var soloLectura = cboEncuestas.find(':selected').data('sololectura');
            if (soloLectura == "1") {
                AlertaGeneral("Alerta", "¡Esta encuesta esta marcada como solo lectura!");
            }
            else {
                $(".divUser").remove();
                $("#dialogEnviar").modal("show");
                newUser.focus();
            }
        }
        function fnOpenTelefonica() {
            $(".divUser").remove();
            $("#dialogTelefonica").modal("show");

            txtEmpresa.val("");
            $("#txtCliente").val("");
            $("#txtTelefonicaAsunto").val("");
            $("#txtTelefonicaComentario").val("");

            fnCancelarCliente();

            getEncuestaTelefonica();
            newUser.focus();
        }
        function fnOpenPapel() {
            $(".divUser").remove();
            $("#dialogPapel").modal("show");

            $('#txtEmpresaPapel').val("");
            $("#txtClientePapel").val("");
            $("#txtTelefonicaAsuntoPapel").val("");
            $("#txtTelefonicaComentarioPapel").val("");

            fnCancelarClientePapel();

            getEncuestaTelefonicaPapel();
            newUser.focus();
        }

        function getEncuestaTelefonica() {
            cargarEncuestaTelefonica(!isNaN(unmaskNumero(cboEncuestas.val())) ? unmaskNumero(cboEncuestas.val()) : null);
        }

        function getEncuestaTelefonicaPapel() {
            cargarEncuestaTelefonicaPapel(!isNaN(unmaskNumero(cboEncuestas.val())) ? unmaskNumero(cboEncuestas.val()) : null);
        }

        function cargarEncuestaTelefonica(id) {
            if (id != null) {
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/Encuestas/Encuesta/getEncuestaTelefonica",
                    data: { id: id },
                    success: function (response) {
                        var obj = response.obj;
                        _encuestaID = obj.id;

                        $("#txtTelefonicaTitulo").val(obj.titulo);
                        $("#txtTelefonicaDescripcion").val(obj.descripcion);
                        $(".Preguntas").empty();

                        $.ajax({
                            url: "/Encuestas/Encuesta/getEstrellas",
                            asyn: false,
                            success: function (respuestaEstrellas) {
                                var estrellas = respuestaEstrellas.data;

                                $.each(obj.preguntas, function (i, e) {
                                    var pregunta = fnAddPreguntaTelefonica(e.pregunta, e.id);
                                    $(pregunta).appendTo($(".Preguntas"));
                                    $('.starrr').starrr({
                                        rating: 0,
                                        change: function (e, value) {
                                            var id = $(e.currentTarget).data("id");
                                            if ($('#dialogTelefonica .starrr[data-id="'+id+'"]').find('.far').length == 5) {
                                                value = 0;
                                            }
                                            $(e.currentTarget).attr("data-calificacion", value);
                                            if (value <= 3) {
                                                $('[data-id="txtRespuesta' + id + '"]').show();
                                            }
                                            if (value > 3 || value == 0) {
                                                $('[data-id="txtRespuesta' + id + '"]').hide();
                                                $('[data-id="txtRespuesta' + id + '"]').val('');
                                            }

                                            let etiqueta = $(e.currentTarget).find('label');
                                            if (value > 0) {
                                                $.each(estrellas, function (index, est) {
                                                    if (est.estrellas == value) {
                                                        etiqueta.text(est.descripcion);
                                                    }
                                                });
                                            } else {
                                                etiqueta.text('');
                                            }
                                        }
                                    });

                                    var etiqueta = document.createElement('label');
                                    etiqueta.style.marginLeft = '10px';
                                    $('.starrr:last').append(etiqueta);
                                });
                            }
                        });
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
        }

        function cargarEncuestaTelefonicaPapel(id) {
            if (id != null) {
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/Encuestas/Encuesta/getEncuestaTelefonica",
                    data: { id: id },
                    success: function (response) {
                        var obj = response.obj;
                        _encuestaID = obj.id;

                        $("#txtTelefonicaTituloPapel").val(obj.titulo);
                        $("#txtTelefonicaDescripcionPapel").val(obj.descripcion);
                        $(".Preguntas").empty();

                        $.ajax({
                            url: "/Encuestas/Encuesta/getEstrellas",
                            asyn: false,
                            success: function (respuestaEstrellas) {
                                var estrellas = respuestaEstrellas.data;

                                $.each(obj.preguntas, function (i, e) {
                                    var pregunta = fnAddPreguntaTelefonica(e.pregunta, e.id);
                                    $(pregunta).appendTo($(".Preguntas"));
                                    $('.starrr').starrr({
                                        rating: 0,
                                        change: function (e, value) {
                                            var id = $(e.currentTarget).data("id");
                                            if ($('#dialogPapel .starrr[data-id="'+id+'"]').find('.far').length == 5) {
                                                value = 0;
                                            }
                                            $(e.currentTarget).attr("data-calificacion", value);
                                            if (value <= 3) {
                                                $('[data-id="txtRespuesta' + id + '"]').show();
                                            }
                                            if (value > 3 || value == 0) {
                                                $('[data-id="txtRespuesta' + id + '"]').hide();
                                                $('[data-id="txtRespuesta' + id + '"]').val('');
                                            }

                                            let etiqueta = $(e.currentTarget).find('label');
                                            if (value > 0) {
                                                $.each(estrellas, function (index, est) {
                                                    if (est.estrellas == value) {
                                                        etiqueta.text(est.descripcion);
                                                    }
                                                });
                                            } else {
                                                etiqueta.text('');
                                            }
                                        }
                                    });

                                    var etiqueta = document.createElement('label');
                                    etiqueta.style.marginLeft = '10px';
                                    $('.starrr:last').append(etiqueta);
                                });
                            }
                        });
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
        }

        function fnAddPreguntaTelefonica(text, id) {
            var html = '<div class="row Pregunta">';
            html += '    <div class="col-lg-12">';
            html += '        <div class="row">';
            html += '            <div class="col-lg-12">';
            html += '                <div class="input-group">';
            html += '                    <span class="input-group-addon">Pregunta:</span>';
            html += '                    <div style="border:1px dotted gray;height: 32px;">';
            html += '                        <div class="starrr" data-id="' + id + '" data-calificacion="0"></div>';
            html += '                    </div>';
            html += '                </div>';
            html += '            </div>';
            html += '        </div>';
            html += '        <div class="row">';
            html += '            <div class="col-lg-12">';
            html += '                <textarea class="form-control txtPregunta" placeholder="Pregunta" data-calificacion="" data-id="' + id + '" disabled>' + text + '</textarea>';
            html += '                <textarea class="form-control txtPregunta" placeholder="Explica tu respuesta" style="display:none;" data-respuesta="" data-id="txtRespuesta' + id + '"></textarea>';
            html += '            </div>';
            html += '        </div>';
            html += '    </div>';
            html += '</div>';

            return html;
        }

        function datePicker() {
            var now = new Date(),
                year = now.getYear() + 1900;
            $.datepicker.setDefaults($.datepicker.regional["es"]);
            var dateFormat = "dd/mm/yy",
                from = $("#fechaIni")
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
                to = $("#fechaFin").datepicker({
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
        function fnDisableItems() {
            btnEditar.attr("disabled", true);
            btnOpenEnviar.attr("disabled", true);
            btnOpenTelefonica.attr("disabled", true);
            btnBuscar.prop("disabled", true);
            txtFechaInicio.prop("disabled", true);
            txtFechaFin.prop("disabled", true);
        }
        function fnEnableItems() {
            btnEditar.attr("disabled", false);
            btnOpenEnviar.attr("disabled", false);
            btnOpenTelefonica.attr("disabled", false);
            btnBuscar.prop("disabled", false);
            txtFechaInicio.prop("disabled", false);
            txtFechaFin.prop("disabled", false);
        }
        function loadGraph(objetoCarga, controller, divChart) {
            $.ajax({
                url: controller,
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify(objetoCarga),
                success: function (response) {
                    if (response.success) {
                        var pregunta = [];
                        var calificacion = [];

                        $.each(response.obj, function (i, e) {
                            pregunta.push(e.preguntaDescripcion);
                            calificacion.push(e.calificacion);
                        });

                        BarChart(pregunta, calificacion, divChart);
                    }
                    else {

                    }
                },
                error: function (response) {
                    alert(response.message);
                }
            });
        }
        var myChart;
        function BarChart(meses, importes, divChart) {

            var maximo = Math.max.apply(null, importes);
            maximo = (maximo * .2) + maximo;
            var barChartData = {
                labels: meses,
                datasets: [
                    {
                        backgroundColor: 'rgba(255, 130, 35, 1)',
                        hoverBackgroundColor: 'rgba(255, 130, 35,0.5)',
                        borderColor: 'rgba(255,131,15,1)',
                        borderWidth: 1,
                        data: importes
                    }
                ]
            }


            if (myChart != null) {
                myChart.destroy();
            }

            var ctx = document.getElementById(divChart);
            var ctxx = ctx.getContext("2d");
            ctxx.fillStyle = "blue";
            ctxx.fillRect(0, 0, ctx.width, ctx.height);
            myChart = new Chart(ctx, {
                type: 'bar',
                data: barChartData,
                options: {
                    responsive: true,
                    legend: {
                        display: false
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                callback: function (value, index, values) {
                                    return value.toFixed(1);
                                },
                                stepSize: Math.trunc(maximo / meses.length)
                            }
                        }],
                        xAxes: [{
                            ticks: {
                                autoSkip: false
                            }
                        }]
                    }
                    ,
                    hover: {
                        animationDuration: 0
                    },
                    animation: {
                        duration: 1,
                        onComplete: function () {
                            var chartInstance = this.chart,
                                ctx = chartInstance.ctx;
                            ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                            ctx.fillStyle = "#000000";
                            ctx.textAlign = 'center';
                            ctx.textBaseline = 'bottom';

                            this.data.datasets.forEach(function (dataset, i) {
                                var meta = chartInstance.controller.getDatasetMeta(i);
                                meta.data.forEach(function (bar, index) {
                                    data = dataset.data[index].toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + "%";
                                    ctx.fillText(data, bar._model.x, bar._model.y - 5);
                                });
                            });
                        }
                    }
                }
            });
            ctxx.fill();
            function clickHandler(evt, element) {
                if (element.length) {
                    let data = meses[element[0]._index]
                    if (getIfMeses()) {
                        modalTitle.text("Detalle por Año " + data);
                    }
                    else {
                        modalTitle.text("Detalle por mes " + data);
                    }
                    $("#tituloModalMaquina").text($("#cboFiltroGrupo option:selected").text() + " " + $("#cboFiltroNoEconomico option:selected").text());
                    cargarInicio();
                    loadTabla(getFiltrosObject(data), ruta, gridFiltros, true);
                }
            }

            //inicializarCanvas();
            //addEventListener("resize", inicializarCanvas);
        }
        function getFile() {
            var url = '/Encuestas/Encuesta/getFileDownload/?encuesta=' + $("#cboEncuestas option:selected").text() + '&fechainicio=' + txtFechaInicio.val() + '&fechafin=' + txtFechaFin.val();
            download(url)
            //$.unblockUI();
        }
        function fbExportar() {
            getFile();
        }

        function getFileTodos() {
            //$.blockUI({ message: mensajes.PROCESANDO });

            var url = '/Encuestas/Encuesta/getFileTodosDownload/?&fechainicio=' + txtFechaInicio.val() + '&fechafin=' + txtFechaFin.val();

            download(url)
            //$.unblockUI();
        }

        function getFileMes() {
            //$.blockUI({ message: mensajes.PROCESANDO });

            if (cboDepartamentos.val() == "todos") {
                var url = '/Encuestas/Encuesta/getEncuestasTodosMes/?&mes=' + selectMes.val() + '&year=' + selectMesYear.val();
                download(url)
            } else {
                var url = '/Encuestas/Encuesta/getEncuestaIndividualMes/?&mes=' + selectMes.val() + '&year=' + selectMesYear.val() + '&departamentoID=' + cboDepartamentos.val() + '&departamento=' + cboDepartamentos.find('option:selected').text();
                download(url)
            }

            //$.unblockUI();
        }

        function getFileTri() {
            //$.blockUI({ message: mensajes.PROCESANDO });

            if (cboDepartamentos.val() == "todos") {
                var url = '/Encuestas/Encuesta/getEncuestasTodosTri/?&trimestre=' + selectTrimestre.val() + '&year=' + selectTrimestreYear.val();
                download(url)
            } else {
                var url = '/Encuestas/Encuesta/getEncuestaIndividualTri/?&trimestre=' + selectTrimestre.val() + '&year=' + selectTrimestreYear.val() + '&departamentoID=' + cboDepartamentos.val() + '&departamento=' + cboDepartamentos.find('option:selected').text();
                download(url)
            }

            //$.unblockUI();
        }

        function getFileSem() {
            //$.blockUI({ message: mensajes.PROCESANDO });

            if (cboDepartamentos.val() == "todos") {
                var url = '/Encuestas/Encuesta/getEncuestasTodosSem/?&sem=' + selectSemestre.val() + '&year=' + selectSemestreYear.val();
                download(url)
            } else {
                var url = '/Encuestas/Encuesta/getEncuestaIndividualSem/?&sem=' + selectSemestre.val() + '&year=' + selectSemestreYear.val() + '&departamentoID=' + cboDepartamentos.val() + '&departamento=' + cboDepartamentos.find('option:selected').text();
                download(url)
            }

            //$.unblockUI();
        }

        function getFileYear() {
            //$.blockUI({ message: mensajes.PROCESANDO });
            if (cboDepartamentos.val() == "todos") {
                var url = '/Encuestas/Encuesta/getEncuestasTodosYear/?&year=' + selectYear.val();
                download(url)
            } else {
                var url = '/Encuestas/Encuesta/getEncuestaIndividualYear/?&year=' + selectYear.val() + '&departamentoID=' + cboDepartamentos.val() + '&departamento=' + cboDepartamentos.find('option:selected').text();
                download(url)
            }

            //$.unblockUI();
        }

        function fbExportarTodos() {
            getFileTodos();
        }

        function fbExportarMes() {
            getFileMes();
        }

        function fbExportarTri() {
            getFileTri();
        }

        function fbExportarSem() {
            getFileSem();
        }

        function fbExportarYear() {
            getFileYear();
        }

        function loadCboEncuestas() {
            $.ajax({
                datatype: "json",
                type: "POST",
                // url: "/Encuestas/Encuesta/FillEncuestasByDepto",
                url: "/Encuestas/Encuesta/FillEncuestasPorPermisosCheck",
                success: function (response) {
                    var o = response.items;
                    var g = response.Group;

                    _adminPermisosBotones = response.adminPermisosBotones;

                    if (g.length > 1) {
                        var todos = '<option value="todos">Todos</option>';
                        cboEncuestas.append(todos);
                        $.each(g, function (j, f) {
                            var html = "";
                            html += '<optgroup label="' + f.Text + '">';
                            $.each(o, function (i, e) {
                                if (f.Value == e.deptId) {
                                    html += '<option ' + (e.soloLectura ? 'title="Encuesta marcada como solo lectura"' : '') + ' style="color:' + (e.soloLectura ? 'red; ' : 'none;') + '" value="' + e.Value + '" data-sololectura="' + e.soloLectura + '">' + (e.soloLectura ? '&#xf05e; ' : '') + e.Text + '</option>';
                                }
                            });
                            html += '</optgroup>';
                            var count = $("#cboEncuestas optgroup[label='" + f.Text + "']");
                            if (count.length == 0)
                                cboEncuestas.append(html);
                        });
                    }
                    else {
                        $.each(o, function (i, e) {
                            var html = "";
                            html = '<option ' + (e.soloLectura ? 'title="Encuesta marcada como solo lectura"' : '') + ' style="color:' + (e.soloLectura ? 'red; ' : 'none;') + '" value="' + e.Value + '" data-sololectura="' + e.soloLectura + '">' + (e.soloLectura ? '&#xf05e; ' : '') + e.Text + '</option>';
                            cboEncuestas.append(html);
                        });
                    }
                    //cboEncuestas.html(html);
                    cboEncuestas.trigger("change");
                },
                error: function (error) { }
            });
        }

        function loadCboEncuestasRpt() {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Encuestas/Encuesta/FillEncuestasByDepto",
                success: function (response) {
                    var o = response.items;
                    var g = response.Group;
                    if (g.length > 1) {
                        var todos = '<option value="todos">Todos</option>';
                        //  cboEncuestasRpt.append(todos);
                        $.each(g, function (j, f) {
                            var html = "";
                            html += '<optgroup label="' + f.Text + '">';
                            $.each(o, function (i, e) {
                                if (f.Value == e.deptId) {
                                    html += '<option ' + (e.soloLectura ? 'title="Encuesta marcada como solo lectura"' : '') + ' style="color:' + (e.soloLectura ? 'red; ' : 'none;') + '" value="' + e.Value + '" data-sololectura="' + e.soloLectura + '">' + (e.soloLectura ? '&#xf05e; ' : '') + e.Text + '</option>';
                                }
                            });
                            html += '</optgroup>';
                            var count = $("#cboEncuestasRpt optgroup[label='" + f.Text + "']");
                            if (count.length == 0)
                                cboEncuestasRpt.append(html);
                        });
                    }
                    else {
                        $.each(o, function (i, e) {
                            var html = "";
                            html = '<option ' + (e.soloLectura ? 'title="Encuesta marcada como solo lectura"' : '') + ' style="color:' + (e.soloLectura ? 'red; ' : 'none;') + '" value="' + e.Value + '" data-sololectura="' + e.soloLectura + '">' + (e.soloLectura ? '&#xf05e; ' : '') + e.Text + '</option>';
                            cboEncuestasRpt.append(html);
                        });
                    }
                    //cboEncuestas.html(html);
                    // cboEncuestasRpt.trigger("change");+

                    convertToMultiselect('#cboEncuestasRpt');
                },
                error: function (error) { }
            });
        }

        function loadCboDepartamentos() {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Encuestas/Encuesta/FillEncuestasByDepto",
                success: function (response) {
                    var o = response.items;
                    var g = response.Group;
                    if (g.length > 1) {
                        var todos = '<option value="todos" style="font-weight: bold;">Todos</option>';
                        cboDepartamentos.append(todos);
                        $.each(g, function (j, f) {
                            var html = "";
                            html += '<option  value="' + f.Value + '" label="' + f.Text + '">' + f.Text + '';
                            //$.each(o, function (i, e) {
                            //    if (f.Value == e.deptId) {
                            //        html += '<option value="' + e.Value + '">' + e.Text + '</option>';
                            //    }
                            //});
                            html += '</option>';
                            var count = $("#cboDepartamentos option[label='" + f.Text + "']");
                            if (count.length == 0)
                                cboDepartamentos.append(html);
                        });
                    }
                    else {
                        //$.each(g, function (i, e) {
                        var html = "";
                        html = '<option value="' + g.Value + '">' + g.Text + '</option>';
                        cboDepartamentos.append(html);
                        //});
                    }
                    //cboEncuestas.html(html);
                    cboDepartamentos.trigger("change");
                },
                error: function (error) { }
            });
        }

        init();
    };

    $(document).ready(function () {

        encuestas.dashboard = new dashboard();
    });

})();
function download(url) {
    $.blockUI({ message: "Preparando archivo a descargar" });
    iframe = document.getElementById('iframeDownload');
    iframe.src = url;

    var timer = setInterval(function () {

        var iframeDoc = iframe.contentDocument || iframe.contentWindow.document;
        // Check if loading is complete
        if (iframeDoc.readyState == 'complete' || iframeDoc.readyState == 'interactive') {
            setTimeout(function () {
                $.unblockUI();
            }, 5000);

            clearInterval(timer);
            return;
        }
    }, 1000);
}
function fnVerEncuesta(id, userID) {
    cargarEncuesta(id, userID);
    $("#dialogVerEncuesta").dialog({
        resizable: false,
        height: 600,
        width: "1000px",
        modal: true,
        buttons: {
            "Imprimir": function () {
                fnImprimir();
            },
            "Cerrar": function () {
                $(this).dialog("close");
            }
        }
    });
}
function fnImprimir() {
    var contents = $(".divImprimir").html();
    var frame1 = $('<iframe />');
    frame1[0].name = "frame1";
    frame1.css({ "position": "absolute", "top": "-1000000px" });
    $("body").append(frame1);
    var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
    frameDoc.document.open();
    //Create a new HTML document.
    frameDoc.document.write('<html><head>');
    frameDoc.document.write('<style type="text/css" media="print">@page { size:portrait; } .Pregunta, .Comentario { page-break-inside: avoid; } </style>');
    frameDoc.document.write('</head><body>');
    //Append the external CSS file.

    frameDoc.document.write('<link href="/Content/bootstrap.css" rel="stylesheet" type="text/css" media="all"/>');
    frameDoc.document.write('<link href="/Content/css/themes/default/easyui.css" rel="stylesheet" type="text/css" />');
    frameDoc.document.write('<link href="/Content/Print.css" rel="stylesheet" type="text/css"  media="print"/>');
    //Append the DIV contents.
    frameDoc.document.write(contents);
    frameDoc.document.write('</body></html>');
    frameDoc.document.close();
    setTimeout(function () {
        window.frames["frame1"].focus();
        window.frames["frame1"].print();
        frame1.remove();
    }, 500);
}
function cargarEncuesta(id) {
    if (id != null) {
        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/Encuestas/Encuesta/getEncuestaResult",
            data: { id: id },
            asyn: false,
            success: function (response) {
                var obj = response.obj;
                _encuestaID = obj.id;

                $("#txtEnvio").html(obj.envio);
                $("#txtFechaEnvio").html(obj.fechaEnvio);
                $("#txtRespondio").html(obj.respondio);
                $("#txtFechaRespondio").html(obj.fechaRespondio);
                $("#txtDepartamento").html(obj.departamento);
                $("#txtTitulo").html(obj.titulo);
                $("#txtDescripcion").html(obj.descripcion);
                $(".Preguntas").empty();

                $.ajax({
                    url: "/Encuestas/Encuesta/getEstrellas",
                    asyn: false,
                    success: function (respuestaEstrellas) {
                        var estrellas = respuestaEstrellas.data;

                        $.each(obj.preguntas, function (i, e) {
                            let estrellaDescripcion = '';
                            $.each(estrellas, function (index, est) {
                                if (est.estrellas == e.calificacion) {
                                    estrellaDescripcion = est.descripcion;
                                }
                            });
                            var pregunta = fnAddPregunta(e.pregunta, e.id, e.respuesta, e.calificacion, estrellaDescripcion);
                            $(pregunta).appendTo($(".Preguntas"));
                        });
                    }
                });

                $("#txtComentario").html(obj.comentario);
            },
            error: function () {
                $.unblockUI();
            }
        });
    }
}
function fnDownloadFileEncuesta (descargar)
{
    window.location.href = "/Encuestas/Encuesta/fnDownloadFileEncuesta/?descargar="+descargar;
}
function fnAddPregunta(text, id, respuesta, calificacion, estrellaDescripcion) {
    var html = '<div class="row Pregunta">';
    html += '    <div class="col-lg-12">';
    html += '        <div class="row">';
    html += '            <div class="col-lg-12">';
    html += '                <div class="input-group">';
    html += '                    <span class="input-group-addon">Pregunta:</span>';
    html += '                    <div style="border:1px dotted gray;height: 37px;">';
    //html += '                        <div class="starrr" data-id="' + id + '" data-calificacion="0"></div>';
    html += '' + stars(calificacion) + '<span style="font-weight: bold; position: relative; top: -15px; left: 5px;">' + estrellaDescripcion + '</span>';
    html += '                    </div>';
    html += '                </div>';
    html += '            </div>';
    html += '        </div>';

    html += '        <div class="row">';
    html += '            <div class="col-lg-12">';
    html += '                <span class="form-control txtPregunta" style="height:50px;" placeholder="Pregunta" data-calificacion="' + calificacion + '" data-id="' + id + '" disabled>' + text + '</span>';
    if (calificacion <= 3) {
        html += '                <span class="form-control" style="border:3px dotted red;height: auto;" disabled>' + respuesta + '</span>';
    }
    html += '            </div>';
    html += '        </div>';

    html += '    </div>';
    html += '</div>';
    return html;
}

function stars(num) {
    var s = "";
    for (var i = 1; i <= num; i++) {
        s = s + '★';
    }
    var n = 5 - num;
    if (n > 0) {
        for (var i = 1; i <= n; i++) {
            s = s + '☆';
        }
    }
    return "<span style='color: #da6a1a; font-size: 34px; position: relative; top: -11px;'>" + s + "</span>";
}