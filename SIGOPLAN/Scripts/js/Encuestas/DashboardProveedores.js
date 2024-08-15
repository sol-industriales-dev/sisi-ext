(function () {

    $.namespace('EncuestasProveedor.Dashboard');

    Dashboard = function () {

        mensajes = {
            PROCESANDO: 'Procesando...'
        };
        idEmpresa = $('#idEmpresa')
        _requisicion = 0
        _encuestaID = 0
        _tipoProveedor = null
        asunto = $(".asunto")
        cboTipoEncuesta = $("#cboTipoEncuesta")
        cboEncuestas = $("#cboEncuestas")
        txtFechaInicio = $("#txtFechaInicio")
        txtFechaFin = $("#txtFechaFin")
        btnBuscar = $("#btnBuscar")
        tabGeneral = $("#tabGeneral")
        btnEditar = $("#btnEditar")
        btnOpenEnviar = $("#btnOpenEnviar")
        btnExportarTodos = $("#btnExportarTodos")
        divDatos = $("#divDatos")
        btnExportar = $("#btnExportar")
        tblDashboard = $("#tblDashboard")
        dialogVerEncuesta = $("#dialogVerEncuesta")
        divImprimir = $("#divImprimir")
        txtDepartamento = $("#txtDepartamento")
        txtTitulo = $("#txtTitulo")
        txtDescripcion = $("#txtDescripcion")
        txtEnvio = $("#txtEnvio")
        txtFechaEnvio = $("#txtFechaEnvio")
        txtRespondio = $("#txtRespondio")
        txtFechaRespondio = $("#txtFechaRespondio")
        divContinuaProveedor = $("#divContinuaProveedor")
        divProveedoresServicio = $("#divProveedoresServicio")
        tblContinuaProveedor = $("#tblContinuaProveedor")
        tblProveedoresServicio = $("#tblProveedoresServicio")
        cboListaProveedoresEvaluados = $("#cboListaProveedoresEvaluados")
        tblProveedoresEvaluados = $("#tblProveedoresEvaluados")

        //#region Evaluacion Encuestas
        tblProveedoresCalificaciones = $("#tblProveedoresCalificaciones")
        btnDescargarEvaluacionProveedores = $("#btnDescargarEvaluacionProveedores")
        btnDescargarEvaluacionProveedores2 = $("#btnDescargarEvaluacionProveedores2")
        cboEncuestasRespuestas = $("#cboEncuestasRespuestas")
        cboTipoEncuestaRespuestas = $("#cboTipoEncuestaRespuestas")
        tbOrdenCompraFiltro = $("#tbOrdenCompraFiltro")
        cboProveedorTop20 = $('#cboProveedorTop20');
        tbRequisicionFiltro = $("#tbRequisicionFiltro")
        btnBuscarResponder = $("#btnBuscarResponder")
        spanTipoDato = $("#spanTipoDato")
        divAreaRespuesta = $("#divAreaRespuesta")
        tbProveedor = $("#tbProveedor")
        tbEvaluacion = $("#tbEvaluacion")
        tbTipoMoneda = $("#tbTipoMoneda")
        tbAntiguedadProveedor = $("#tbAntiguedadProveedor")
        tbUbicacionProveedor = $("#tbUbicacionProveedor")
        tbEvaluador = $("#tbEvaluador")
        tbProveedorRequisiciones = $("#tbProveedorRequisiciones")
        tbEvaluadorRequisiciones = $("#tbEvaluadorRequisiciones")
        tbTituloPregunta = $("#tbTituloPregunta")
        txtDescripcionPregunta = $("#txtDescripcionPregunta")
        txtAsunto = $("#txtAsunto")
        txtComentario = $("#txtComentario")
        btnEnviar = $("#btnEnviar")
        Preguntas = $(".Preguntas")
        encabezadoServicios = $("#encabezadoServicios")
        encabezadoProveedores = $("#encabezadoProveedores")
        tblProveedoresCalificacionesGrid = $("#tblProveedoresCalificaciones")
        //#endregion

        //#region Evaluaciones Contestadas
        cboCompradores = $("#cboCompradores")
        /* Graficas de Evaluacion*/
        cboCompradoresGraficas = $("#cboCompradoresGraficas")
        cboTipoEncuestaGraficas = $("#cboTipoEncuestaGraficas")
        cboEncuestaGraficas = $('#cboEncuestaGraficas');
        cboEncuestaEstrellas = $('#cboEncuestaEstrellas');
        txtFechaInicioGraficas = $("#txtFechaInicioGraficas")
        txtFechaFinGraficas = $("#txtFechaFinGraficas")
        btnCargarGraficas = $("#btnCargarGraficas")
        myChart1 = null
        myChart2 = null
        /******/
        tabCalificacion = $("#tabCalificacion")
        txtComentario = $("#txtComentario")
        //#endregion

        //#region EVALUACION POR ESTRELLAS
        // ** FILTROS */
        cboTipoEncuestaEstrellas = $("#cboTipoEncuestaEstrellas")
        txtFechaInicioEstrellas = $("#txtFechaInicioEstrellas")
        txtFechaFinEstrellas = $("#txtFechaFinEstrellas")
        cboCompradoresEstrellas = $("#cboCompradoresEstrellas")
        btnCargarGraficasEstrellas = $("#btnCargarGraficasEstrellas")


        //** GRAFICAS  */
        chartEvaluacionesEstrellas = null
        chartCompradoresEstrellas = null
        btnDescargarEvaluacionProveedoresEstrellas = $("#btnDescargarEvaluacionProveedoresEstrellas")
        cboListaProveedoresEvaluadosEstrellas = $("#cboListaProveedoresEvaluadosEstrellas")
        tblProveedoresEstrellas = $('#tblProveedoresEstrellas')
        tblProveedoresEvaluadosEstrellas = $("#tblProveedoresEvaluadosEstrellas")

        modalDetalleProv = $('#modalDetalleProv');
        modalTitulo = $('#modalTitulo');
        containerHCProv = $('#containerHCProv');
        //#endregion
        const urlCboTipoEncuesta = new URL(window.location.origin + '/Encuestas/EncuestasProveedor/cboTipoEncuesta');

        const realizarEncuestaTop20PorCompras = $('#realizarEncuestaTop20PorCompras');

        function init() {
            tblProveedoresServicioGrid = $("#tblProveedoresServicio").DataTable({});
            seCboTipoEnciesta();
            tbProveedorRequisiciones.getAutocomplete(SelectProveedor, null, '/Encuestas/EncuestasProveedor/getProveedores');

            txtFechaInicio.datepicker().datepicker("setDate", new Date());
            txtFechaFin.datepicker().datepicker("setDate", new Date());
            txtFechaInicioGraficas.datepicker().datepicker("setDate", new Date());
            txtFechaFinGraficas.datepicker().datepicker("setDate", new Date());



            // cboEncuestas.fillCombo('/EncuestasProveedor/cboEncuestas', { tipoEncuesta: cboTipoEncuesta.val() }, false);
            btnBuscar.click(fnBuscar);
            cboTipoEncuesta.change(tipoEncuestas);
            cboTipoEncuestaGraficas.change(tipoEncuestasGraficas);
            cboTipoEncuestaEstrellas.change(tipoEncuestasEstrella);
            tblContinuaProveedorGrid = $("#tblContinuaProveedor").DataTable({});
            // cboEncuestasRespuestas.fillCombo('/EncuestasProveedor/cboEncuestas', { tipoEncuesta: cboTipoEncuestaRespuestas.val() }, false);

            /*Acciones Evaluacion Proveedores*/
            btnBuscarResponder.click(BuscarOrdenCompra);
            cboTipoEncuestaRespuestas.on('change', function () {
                vaciarCampos();
                cboProveedorTop20.empty();
                cambioEncuesta(cboEncuestasRespuestas, $(this));
            });
            cboEncuestasRespuestas.on('change', function () {
                vaciarCampos();
                let disabled = $(this).val() != undefined && $(this).val() != null && $(this).val() != '' ? false : true;
                btnBuscarResponder.attr('disabled', disabled);
            });
            cboProveedorTop20.on('change', function () {
                vaciarCampos();
            });

            btnEnviar.click(fnEnviar);
            /*Fin Evaluacion Proveedores*/

            /*Acciones Evaluacion Contestadas*/

            cboCompradores.fillCombo('/Encuestas/EncuestasProveedor/FillCboCompradores', { est: true }, false, "Todos");
            convertToMultiselect("#cboCompradores");

            cboCompradoresGraficas.fillCombo('/Encuestas/EncuestasProveedor/FillCboCompradores', { est: true }, false, "Todos");
            convertToMultiselect("#cboCompradoresGraficas");
            /**/

            /*Acciones de Evaluacion de graficas*/

            btnCargarGraficas.click(cargarGraficas);
            /**/
            btnDescargarEvaluacionProveedores.click(saveCanvas1);
            btnDescargarEvaluacionProveedores2.click(saveCanvas2);



            //#region EVALUACIONES ESTRELLAS
            txtFechaInicioEstrellas.datepicker().datepicker("setDate", new Date());
            txtFechaFinEstrellas.datepicker().datepicker("setDate", new Date());

            cboCompradoresEstrellas.fillCombo('/Encuestas/EncuestasProveedor/FillCboCompradores', { est: true }, false, "Todos");
            convertToMultiselect("#cboCompradoresEstrellas");

            btnCargarGraficasEstrellas.click(cargarGraficasEstrellas);
            btnDescargarEvaluacionProveedoresEstrellas.click(saveCanvasEvaluacionEstrellas);
            //#endregion

            const parametrosUrl = new URLSearchParams(window.location.search);
            let realizarEncuestas = parametrosUrl.has('realizar') ? parametrosUrl.get('realizar') : false;

            if (realizarEncuestas || realizarEncuestaTop20PorCompras.val()) {
                AlertaGeneral('Alerta', 'Tienes que evaluar a todos los proveedores correspondientes al top20 del mes anterior antes de continuar con tus actividades');

                var clean_uri = location.protocol + "//" + location.host + location.pathname;
                window.history.replaceState({}, document.title, clean_uri);
            }
        }

        $('#tabCalificacion').on('shown.bs.tab', function (event) {
            tblProveedoresCalificaciones.draw();
        });

        $('#tabEstrellasCalificacion').on('shown.bs.tab', function (event) {
            tblProveedoresEstrellas.draw();
        });

        $('#tabListaProveedores').on('shown.bs.tab', function (event) {
            tblProveedoresEvaluados.draw();
        });

        $('#tabEstrellaListaProveedor').on('shown.bs.tab', function (event) {
            tblProveedoresEvaluadosEstrellas.draw();
        });

        $('#tblProveedoresEvaluadosEstrellas').on('click', '.detallesProv', function () {
            modalTitulo.text($(this).data('nomprov'));
            setGraficaPreguntasProv($(this).data('numprov'));
        });

        $('#tblProveedoresEvaluadosEstrellas').on('click', '.detallesProvMensual', function () {
            modalTitulo.text($(this).data('nomprov'));
            setGraficaMensualProv($(this).data('numprov'));
        });

        async function seCboTipoEnciesta() {
            try {
                response = await ejectFetchJson(urlCboTipoEncuesta);
                let { success, items } = response;
                if (success) {
                    cboTipoEncuesta.fillComboItems(items, undefined, (idEmpresa.val() == 6 ? 4 : 1));
                    cboTipoEncuestaRespuestas.fillComboItems(items, undefined, (idEmpresa.val() == 6 ? 4 : 1));
                    cboTipoEncuestaGraficas.fillComboItems(items, undefined, (idEmpresa.val() == 6 ? 4 : 1));
                    cboTipoEncuestaEstrellas.fillComboItems(items, undefined, (idEmpresa.val() == 6 ? 4 : 1));
                    tipoEncuestas();
                    cambioEncuesta(cboEncuestasRespuestas, cboTipoEncuestaRespuestas);
                    cboTipoEncuestaGraficas.trigger('change');
                    cboTipoEncuestaEstrellas.trigger('change');
                }
            } catch (o_O) { AlertaGeneral('Avisoooo', o_O.message) }
        }
        function saveCanvas1() {
            var canvas = $('#LineWithLine1').get(0);
            canvas.toBlob(function (blob) {
                saveAs(blob, "EvaluacionProveedores.png");
            });

        }
        function saveCanvas2() {
            var canvas = $('#LineWithLine2').get(0);
            canvas.toBlob(function (blob) {
                saveAs(blob, "Compradores.png");
            });

        }

        var backgroundColor = 'white';
        Chart.plugins.register({
            beforeDraw: function (c) {
                var ctx = c.chart.ctx;
                ctx.fillStyle = backgroundColor;
                ctx.fillRect(0, 0, c.chart.width, c.chart.height);
            }
        });


        function BuscarOrdenCompra() {
            $(".Preguntas").empty();
            if (cboEncuestasRespuestas.val() != "" && cboEncuestasRespuestas.val() != undefined && cboEncuestasRespuestas.val() != null) {

                if (cboProveedorTop20.val() == undefined || cboProveedorTop20.val() == null || cboProveedorTop20.val() == '') {
                    AlertaGeneral("Alerta", "¡Es necesario seleccionar un proveedor!");
                } else {
                    let dependencia = +cboTipoEncuestaRespuestas.find(":selected").data().prefijo
                    switch (dependencia) {
                        case 1:
                            //Requisiciones();
                            cargarEvaluacionTop20();
                            break;
                        case 2:
                            cargarEvaluacionTop20();
                            break;
                        default:
                            break;
                    }
                }
            }
            else {
                AlertaGeneral("Alerta", "¡Debe seleccionar una encuesta!");
            }
        }

        function Requisiciones() {
            var requisiciones = tbRequisicionFiltro.val();
            var numOC;
            var centrocostos;

            var id = cboEncuestasRespuestas.val();
            var requisicion;
            var centrocostos;

            if (requisiciones.includes("-")) {

                centrocostos = requisiciones.split('-')[0];
                requisicion = requisiciones.split('-')[1];

                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/Encuestas/EncuestasProveedor/ResponderEncuestaRequisicion",
                    data: { encuestaID: id, noRequisicion: requisicion, centrocostos: centrocostos },
                    asyn: false,
                    success: function (response) {
                        var obj = response.obj;
                        var blob = $.urlParam('encuesta');

                        if (response.RespuestaEncuesta) {
                            ConfirmacionGeneral("Alerta", "Esta encuesta ya fue contestada.");
                            vaciarCampos();
                        }
                        else {
                            divAreaRespuesta.removeClass('hide');
                            encabezadoServicios.removeClass('hide');
                            encabezadoProveedores.addClass('hide');
                            moment.locale();
                            _encuestaID = response.id;
                            _requisicion = requisicion;
                            var FechaActual = new Date();



                            tbEvaluadorRequisiciones.val(response.evaluador);
                            tbProveedorRequisiciones.attr('data-centrocostos', centrocostos);
                            tbProveedorRequisiciones.attr('data-FechaOC', response.getDatosProveedor.fechaRequisicion);
                            tbTituloPregunta.val(response.titulo);
                            txtDescripcionPregunta.val(response.descripcion);
                            //  txtAsunto.val(response.tipoEncuesta);
                            asunto.show();

                            $.ajax({
                                url: "/Encuestas/Encuesta/getEstrellas",
                                asyn: false,
                                success: function (respuestaEstrellas) {
                                    var estrellas = respuestaEstrellas.data;

                                    $.each(response.preguntas, function (i, e) {
                                        var pregunta = fnAddPregunta(e.pregunta, e.id, e.ponderacion);
                                        $(pregunta).appendTo(Preguntas);
                                        $('.starrr').starrr({
                                            rating: 0,
                                            change: function (e, value) {
                                                var id = $(e.currentTarget).data("id");
                                                $(e.currentTarget).attr("data-calificacion", value);
                                                if (value <= 2) {
                                                    $('[data-id="txtRespuesta' + id + '"]').show();
                                                }
                                                if (value > 2) {
                                                    $('[data-id="txtRespuesta' + id + '"]').hide();
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
                        }
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
        }

        function SelectProveedor(event, ui) {
            tbProveedorRequisiciones.text(ui.item.value);
            tbProveedorRequisiciones.attr('data-NumProveedor', ui.item.id)
            //  SetInfoProvee
        }

        function cargarEvaluacionTop20() {
            _tipoProveedor = null;

            $.get('/Encuestas/EncuestasProveedor/ResponderEncuestaTop20', {
                tipoEncuesta: cboTipoEncuestaRespuestas.val(),
                encuestaID: cboEncuestasRespuestas.val(),
                numeroProveedor: (idEmpresa.val() == 6 ? 0 : cboProveedorTop20.val()),
                numeroProveedorPeru: cboProveedorTop20.val()
            }).always($.blockUI({})).then(response => {
                if (response.Success) {
                    divAreaRespuesta.removeClass('hide');
                    encabezadoProveedores.removeClass('hide');
                    encabezadoServicios.addClass('hide');

                    let proveedor = response.Value.proveedor;
                    let preguntas = response.Value.preguntas;

                    _encuestaID = cboEncuestasRespuestas.val();
                    _numeroOC = null;
                    _requisicion = null;
                    _tipoProveedor = proveedor.Tipo;

                    let fechaActual = new Date();

                    tbProveedor.val(proveedor.Nombre);
                    tbProveedor.attr('data-idProveedor', proveedor.Numero);

                    tbEvaluacion.val(moment(fechaActual).format('DD/MM/YYYY'));

                    tbAntiguedadProveedor.val(moment(proveedor.FechaAntiguedad).format('DD/MM/YYYY'));
                    tbUbicacionProveedor.val(proveedor.Ubicacion);
                    tbEvaluador.val(response.Value.evaluador);
                    tbTituloPregunta.val(response.Value.encuesta.titulo);
                    txtDescripcionPregunta.val(response.Value.encuesta.descripcion);
                    tbTipoMoneda.val(proveedor.TipoMoneda == 1 ? 'PESOS' : 'DOLARES');

                    $.get('/Encuestas/Encuesta/getEstrellas', {})
                        .then(response => {
                            if (response.success) {
                                let estrellas = response.data;

                                $.each(preguntas, function (i, e) {
                                    let pregunta = fnAddPregunta(e.pregunta, e.id, e.ponderacion, e.descripcionTipo);
                                    $(pregunta).appendTo(Preguntas);

                                    $('.starrr').starrr({
                                        rating: 0,

                                        change: function (e, value) {
                                            let id = $(e.currentTarget).data('id');

                                            if ($('.starrr[data-id="' + id + '"]').find('.far').length == 5) {
                                                value = 0;
                                            }

                                            $(e.currentTarget).attr('data-calificacion', value);

                                            if (value <= 2) {
                                                $('[data-id="txtRespuesta' + id + '"]').show();
                                            }
                                            if (value > 2 || value == 0) {
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

                                    let etiqueta = document.createElement('label');
                                    etiqueta.style.marginLeft = '10px';
                                    $('.starrr:last').append(etiqueta);
                                });
                            } else {
                                AlertaGeneral('Alerta', response.message);
                            }
                        }, error => {
                            AlertaGeneral('Alerta', null);
                        });

                } else {
                    AlertaGeneral('Alerta', response.message);
                }
            }, error => {
                AlertaGeneral('Alerta', null);
            });
            $.unblockUI({});
        }

        function cargarEvaluacion() {

            var OrdenCompra = tbOrdenCompraFiltro.val();
            var numOC;
            var centrocostos;
            if (OrdenCompra.includes("-")) {

                centrocostos = OrdenCompra.split('-')[0];
                numOC = OrdenCompra.split('-')[1];
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/Encuestas/EncuestasProveedor/ResponderEncuesta",
                    data: { encuestaID: cboEncuestasRespuestas.val(), numeroOC: numOC, centrocostos: centrocostos },
                    asyn: false,
                    success: function (response) {
                        var obj = response.obj;
                        var blob = $.urlParam('encuesta');

                        if (!response.ExisteOC) {
                            ConfirmacionGeneral("Alerta", "Esta OC no se encuentra disponible.");
                            vaciarCampos();
                        } else {
                            if (response.RespuestaEncuesta) {
                                ConfirmacionGeneral("Alerta", "Esta encuesta ya fue contestada.");
                                vaciarCampos();
                            }
                            else {
                                divAreaRespuesta.removeClass('hide');
                                encabezadoProveedores.removeClass('hide');
                                encabezadoServicios.addClass('hide');
                                moment.locale();
                                var proveedoresData = response.getDatosProveedor;
                                _encuestaID = response.id;
                                _numeroOC = numOC;
                                //  _encuestaUsuarioID = obj.encuestaUsuarioID;
                                var FechaActual = new Date();

                                var fechaAnt = "";
                                var fechaAntiguedad = "";

                                if (proveedoresData.fechaAntiguedad != null) {
                                    fechaAnt = Number(proveedoresData.fechaAntiguedad.split('(')[1].split(')')[0]);
                                    fechaAntiguedad = moment(fechaAnt).format('DD/MM/YYYY');
                                }

                                tbProveedor.val(proveedoresData.nombreProveedor);
                                tbProveedor.attr('data-idProveedor', proveedoresData.numProveedor);
                                tbProveedor.attr('data-cc', proveedoresData.centrocostos);

                                tbEvaluacion.val(moment(FechaActual).format('DD/MM/YYYY'));

                                tbAntiguedadProveedor.val(fechaAntiguedad);
                                tbUbicacionProveedor.val(proveedoresData.ubicacionProveedor);
                                tbEvaluador.val(response.evaluador);
                                tbTituloPregunta.val(response.titulo);
                                txtDescripcionPregunta.val(response.descripcion);
                                tbTipoMoneda.val(proveedoresData.tipoMoneda);

                                // txtAsunto.val(response.tipoEncuesta);
                                asunto.show();

                                $.ajax({
                                    url: "/Encuestas/Encuesta/getEstrellas",
                                    asyn: false,
                                    success: function (respuestaEstrellas) {
                                        var estrellas = respuestaEstrellas.data;

                                        $.each(response.preguntas, function (i, e) {
                                            var pregunta = fnAddPregunta(e.pregunta, e.id, e.ponderacion);
                                            $(pregunta).appendTo(Preguntas);
                                            $('.starrr').starrr({
                                                rating: 0,
                                                change: function (e, value) {
                                                    var id = $(e.currentTarget).data("id");
                                                    $(e.currentTarget).attr("data-calificacion", value);
                                                    if (value <= 2) {
                                                        $('[data-id="txtRespuesta' + id + '"]').show();
                                                    }
                                                    if (value > 2) {
                                                        $('[data-id="txtRespuesta' + id + '"]').hide();
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
                            }
                        }
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
        }

        function cambioEncuesta(cboxEncuesta, cboxTipoEncuesta) {
            let idTipoEncuesta = cboxTipoEncuesta.val();

            cboxEncuesta.fillCombo('/EncuestasProveedor/cboEncuestas', { tipoEncuesta: idTipoEncuesta }, false);

            cboxEncuesta.trigger('change');

            let tipoDependencia = +cboxTipoEncuesta.find(":selected").data().prefijo;

            cboProveedorTop20.fillCombo('/Encuestas/EncuestasProveedor/GetProveedoresTop20', { tipoEncuestaId: cboTipoEncuestaRespuestas.val() }, false, null);

            spanTipoDato.text('Proveedores');
            tbRequisicionFiltro.addClass('hide');
            cboProveedorTop20.removeClass('hide');

            // switch (tipoDependencia) {
            //     case 1:
            //         spanTipoDato.text('Requisicion');
            //         //tbOrdenCompraFiltro.addClass('hide');
            //         cboProveedorTop20.addClass('hide');
            //         tbRequisicionFiltro.removeClass('hide');
            //         break;
            //     case 2:
            //         //spanTipoDato.text('Orden Compra')
            //         spanTipoDato.text('Proveedores');
            //         tbRequisicionFiltro.addClass('hide');
            //         cboProveedorTop20.removeClass('hide');
            //         //tbOrdenCompraFiltro.removeClass('hide');
            //         break;
            //     default:
            //         break;
            // }
        }

        function tipoEncuestas() {
            cboEncuestas.clearCombo();
            cboEncuestas.fillCombo('/EncuestasProveedor/cboEncuestasConsultas', { tipoEncuesta: cboTipoEncuesta.val() }, false);
            let tipoDependencia = +cboTipoEncuesta.find(":selected").data().prefijo;
            switch (tipoDependencia) {
                case 1:
                    divProveedoresServicio.removeClass('hide');
                    divContinuaProveedor.addClass('hide');
                    tblContinuaProveedorGrid.data().clear().draw();
                    tblProveedoresServicioGrid.data().clear().draw();
                    break;
                case 2:
                    divProveedoresServicio.addClass('hide');
                    divContinuaProveedor.removeClass('hide');
                    tblContinuaProveedorGrid.data().clear().draw();
                    tblProveedoresServicioGrid.data().clear().draw();
                    break;
                default:
                    break;
            }
        }

        function tipoEncuestasGraficas() {
            cboEncuestaGraficas.clearCombo();
            cboEncuestaGraficas.fillCombo('/EncuestasProveedor/cboEncuestasConsultas', { tipoEncuesta: cboTipoEncuestaGraficas.val() }, false);
        }

        function tipoEncuestasEstrella() {
            cboEncuestaEstrellas.clearCombo();
            cboEncuestaEstrellas.fillCombo('/EncuestasProveedor/cboEncuestasConsultas', { tipoEncuesta: cboTipoEncuestaEstrellas.val() }, false);
        }

        function getFiltrosObject() {
            return {
                id: cboEncuestas.val(),
                fechaInicio: txtFechaInicio.val(),
                fechaFin: txtFechaFin.val(),
                estatus: cboTipoEncuesta.val()
            }
        }

        var backgroundColor = 'white';
        Chart.plugins.register({
            beforeDraw: function (c) {
                var ctx = c.chart.ctx;
                ctx.fillStyle = backgroundColor;
                ctx.fillRect(0, 0, c.chart.width, c.chart.height);
            }
        });


        function fnBuscar() {
            let url = "",
                tipoDependencia = +cboTipoEncuesta.find(":selected").data().prefijo;
            switch (tipoDependencia) {
                case 1:
                    url = '/Encuestas/EncuestasProveedor/LoadEncuestasProveedoresRequisiciones';
                    break;
                case 2:
                    url = '/Encuestas/EncuestasProveedor/LoadEncuestasProveedoresOC';
                    break;
                default:
                    break;
            }
            if (getValoresMultiples("#cboCompradores").length > 0) {
                $.blockUI({ message: 'procesando...' });
                $.ajax({
                    url: url,
                    type: 'POST',
                    dataType: 'json',
                    data: { estatus: cboTipoEncuesta.val(), FechaInicio: txtFechaInicio.val(), FechaFin: txtFechaFin.val(), compradores: getValoresMultiples("#cboCompradores") },
                    success: function (response) {
                        if (response.success) {
                            switch (tipoDependencia) {
                                case 1:
                                    LoadtblProveedoresServicio(response.objListProveedores);
                                    break;
                                case 2:
                                    LoadtblContinuaProveedor(response.objListProveedores);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else {
                            alert(response.message);
                        }

                        $.unblockUI();
                    },
                    error: function (response) {
                        $.unblockUI();
                        alert(response.message);
                    }
                });
            }
            else {
            }
        }

        function cargarGraficas() {
            url = '/Encuestas/EncuestasProveedor/SetGraficasEvaluaciones';

            $.blockUI({ message: 'procesando...' });
            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'json',
                data: { fechainicio: txtFechaInicioGraficas.val(), fechafin: txtFechaFinGraficas.val(), encuesta: cboTipoEncuestaGraficas.val(), tipoEncuesta: cboEncuestaGraficas.val(), compradores: getValoresMultiples("#cboCompradoresGraficas") },
                success: function (response) {

                    if (response.success) {
                        setGraficaEvaluaciones(response.resultGraphBueno, response.resultGraphRegular, response.resultGraphMalo);
                        SetGraficaCompradores(response.dtCompradores);
                        LoadRevisionProveedores(response.dtResultado);
                        LoadProveedoresLista(response.ResultListaProveedores);

                        cboListaProveedoresEvaluados.fillCombo('/Encuestas/EncuestasProveedor/loadCboProveedores', null, false, "Todos");
                        convertToMultiselect("#cboListaProveedoresEvaluados");
                    } else {
                        AlertaGeneral('Alerta', response.message);
                    }
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    alert(response.message);
                }
            });
        }

        function LoadProveedoresLista(dataSet) {
            tblProveedoresEvaluados = $("#tblProveedoresEvaluados").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
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
                responsive: true,
                "bFilter": true,
                "order": false,
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [
                    {
                        data: "proveedorName"
                    },
                    {
                        data: "cantidadBuenos"
                    },
                    {
                        data: "cantidadRegulares"

                    },
                    {
                        data: "cantidadMalos"

                    },
                    {
                        data: "porcentaje"
                    }
                ],
                columnDefs: [
                    {
                        targets: [4],
                        render: function (data, type, row) {
                            let numero = maskNumero(data);
                            return numero.substring(1) + '%';
                        }
                    }
                ],
                "paging": true,
                "info": false,
                dom: 'Bfrtip',
                "drawCallback": function (settings) {
                    $.unblockUI();
                },
                buttons: parametrosImpresion("Reporte de actividades", "")

            });
        }

        function setGraficaEvaluaciones(bueno, regular, malo) {

            if (myChart1 != null) {
                myChart1.destroy();
            }

            var ctx1 = document.getElementById("LineWithLine1").getContext("2d");
            var data = {
                labels: ["Bueno", "Regular", "Malo"],
                datasets: [{
                    label: "Evaluaciones",
                    backgroundColor: ["rgba(245, 176, 65, 0.5)", "rgba(130, 224, 170, 0.5)", "rgba(236, 112, 99, 0.5)"],
                    borderColor: ["rgba(211, 84, 0  )", "rgba(39, 174, 96)", "rgba(231, 76, 60)"],
                    borderWidth: 1,
                    data: [bueno, regular, malo],
                    fill: true
                }
                ]
            };
            dtLabel = ["Bueno", "Regular", "Malo"];

            myChart1 = new Chart(ctx1, {
                type: 'bar',
                data: data,
                options:
                {
                    responsive: false,
                    elements: {
                        line: {
                            fill: false
                        }
                    },
                    scales: {
                        xAxes: [{
                            display: true,
                            gridLines: {
                                display: false
                            },
                            labels: {
                                show: true,
                            }, ticks: {
                                autoSkip: false,

                            }
                        }],
                    },
                    animation: {
                        duration: 1,
                        onComplete: function () {
                            var chartInstance = this.chart,
                                ctx = chartInstance.ctx;

                            if (ctx.canvas.id == 'LineWithLine1') {
                                ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                                ctx.fillStyle = "#000000";
                                ctx.textAlign = 'center';
                                ctx.textBaseline = 'bottom';

                                this.data.datasets.forEach(function (dataset, i) {
                                    var meta = chartInstance.controller.getDatasetMeta(i);
                                    if (dataset.type != 'line') {
                                        meta.data.forEach(function (bar, index) {

                                            var dato = Number(dataset.data[index]);
                                            data = dato;
                                            ctx.fillText(data, bar._model.x, bar._model.y - 5);
                                        });
                                    }

                                });
                            }
                        }
                    }

                }
            });

        }

        function SetGraficaCompradores(Compradores) {
            if (myChart2 != null) {
                myChart2.destroy();
            }

            var dt = Compradores;

            var dtSetInfo = new Array();
            var dtLabel = new Array();
            var dbColores = new Array();


            for (var i = 0; i < dt.length; i++) {
                var colorName = GetColor();
                dtSetInfo.push(dt[i].CantidadEvaluaciones);
                dtLabel.push(dt[i].nombreComprador);
                dbColores.push(colorName);
            }

            var ctx2 = document.getElementById("LineWithLine2").getContext("2d");

            var data = {
                labels: dtLabel,
                datasets: [{
                    label: "Compradores",
                    backgroundColor: dbColores,
                    borderColor: dbColores,
                    borderWidth: 1,
                    data: dtSetInfo,
                    fill: false,
                }]
            };

            myChart2 = new Chart(ctx2, {
                type: 'bar',
                data: data,
                options:
                {
                    responsive: false,
                    tooltips: {
                        mode: 'label'
                    },
                    elements: {
                        line: {
                            fill: false
                        }
                    },
                    scales: {
                        xAxes: [{
                            ticks: {
                                autoSkip: false,

                            }
                        }],
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    },
                    animation: {
                        duration: 1,
                        onComplete: function () {
                            var chartInstance = this.chart,
                                ctx = chartInstance.ctx;

                            if (ctx.canvas.id == 'LineWithLine2') {
                                ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                                ctx.fillStyle = "#000000";
                                ctx.textAlign = 'center';
                                ctx.textBaseline = 'bottom';

                                this.data.datasets.forEach(function (dataset, i) {
                                    var meta = chartInstance.controller.getDatasetMeta(i);
                                    if (dataset.type != 'line') {
                                        meta.data.forEach(function (bar, index) {

                                            var dato = Number(dataset.data[index]);
                                            data = dato;
                                            ctx.fillText(data, bar._model.x, bar._model.y - 5);
                                        });
                                    }

                                });
                            }
                        }
                    }
                }
            });
        }

        function GetColor(idColor) {
            function GetColor() {
                hexadecimal = new Array("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F")
                color_aleatorio = "#";
                for (i = 0; i < 6; i++) {
                    posarray = aleatorio(0, hexadecimal.length)
                    color_aleatorio += hexadecimal[posarray]
                }
                return color_aleatorio
            }
        }

        //Se encarga de hacer blind de la informacíon de las requisiciones.
        function LoadtblProveedoresServicio(dataSet) {
            tblProveedoresServicioGrid = $('#tblProveedoresServicio').DataTable({
                language: dtDicEsp,
                responsive: true,
                bFilter: true,
                order: true,
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                paging: true,
                info: false,
                columns: [
                    { data: 'numeroProveedor' },
                    { data: 'nombreProveedor' },
                    { data: 'monedaProveedor' },
                    { data: 'nombreEvaluador' },
                    { data: 'fechaEvaluacion' },
                    { data: 'calificacion' },
                    { data: 'comentario' },
                    { data: null }
                ],
                columnDefs: [
                    {
                        targets: [0, 2, 4, 5, 7],
                        className: 'text-center'
                    },
                    {
                        targets: [4],
                        render: function (data, type, row) {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    },
                    {
                        targets: [7],
                        render: function (data, type, row) {
                            return '<button type="button" class="btn btn-default btn-block btn-sm" onclick="verReporte(' + row.id + ',2)" >Detalle</button>'
                        }
                    }
                ],
                headerCallback: function (thead, data, start, end, display) {
                    $(thead).children().addClass('text-center');
                }
            });
        }

        //Se encarga de hacer bind la informacion de las OC terminadas.
        function LoadtblContinuaProveedor(dataSet) {
            tblContinuaProveedorGrid = $("#tblContinuaProveedor").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
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
                responsive: true,
                "bFilter": true,
                "order": false,
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [
                    {
                        data: "centrocostos"
                    },
                    {
                        data: "FechaOC"
                    },
                    {
                        data: "numeroOC"
                    },

                    {
                        data: "NombreProveedor"
                    },
                    {
                        data: "Comentarios"
                    },
                    {
                        data: "ponderacion",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (cellData >= .75 && cellData <= 1) {
                                $(td).text('BUENO');
                            }
                            else if (cellData >= .45 && cellData <= .74) {
                                $(td).text('REGULAR');
                            } else if (cellData >= 0 && cellData <= .44) {
                                $(td).text('MALO');
                            }
                        }
                    },
                    {
                        data: "btn",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (rowData.estadoEncuesta) {
                                $(td).append('<button type="button" class="btn btn-default btn-block btn-sm" onclick="verReporte(' + rowData.id + ',1)" >Detalle</button>');
                            }
                            else {

                            }
                        }
                    }
                ],
                "paging": true,
                "info": false

            });
        }
        function LoadRevisionProveedores(dataSet) {
            tblProveedoresCalificaciones = $("#tblProveedoresCalificaciones").DataTable({
                language: dtDicEsp,
                responsive: true,
                "bFilter": true,
                "order": false,
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [
                    {
                        data: "proveedorName"
                    },
                    {
                        data: "tipoProveedor"
                    },
                    {
                        data: "comentario"

                    }
                ],
                "paging": true,
                "info": false,
                dom: 'Bfrtip',
                "drawCallback": function (settings) {
                    $.unblockUI();
                },
                buttons: parametrosImpresion("Reporte de actividades", "")

            });
        }

        function fnAddPregunta(text, id, poderacion, descripcionTipo) {
            var html = '<div class="row Pregunta">';
            html += '    <div class="col-lg-12">';
            html += '        <div class="row">';
            html += '            <div class="col-lg-12">';
            html += '                <div class="input-group">';
            html += '                    <span class="input-group-addon">Pregunta(' + descripcionTipo + '):</span>';
            html += '                    <div style="border:1px dotted gray;height: 32px;">';
            html += '                        <div class="starrr" data-id="' + id + '" data-calificacion="0" data-ponderacion="' + poderacion + '"></div>';
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


        function getObjDetalle() {
            return {
                numProveedor: tbProveedor.attr('data-idProveedor'),
                nombreProveedor: tbProveedor.val(),
                tipoProveedor: _tipoProveedor,
                fechaAntiguedad: tbAntiguedadProveedor.val(),
                ubicacionProveedor: tbUbicacionProveedor.val(),
                comentarios: txtComentario.val(),
                tipoMoneda: tbTipoMoneda.val(),
                encuestaID: _encuestaID
            };
        }

        function fnEnviar() {
            var tipoDependencia = +cboTipoEncuestaRespuestas.find(":selected").data().prefijo;
            var preguntas = $(".starrr");
            var preguntasList = new Array();
            var validacion = false;
            var totalPonderacion = 0;
            $.each(preguntas, function (i, e) {
                var o = {};
                o.encuestaID = _encuestaID;
                //  o.encuestaUsuarioID = _encuestaUsuarioID;
                o.preguntaID = $(e).data("id");
                o.calificacion = $(e).attr('data-calificacion');
                o.respuesta = $('[data-id="txtRespuesta' + o.preguntaID + '"]').val();
                o.ponderacion = $(e).attr('data-ponderacion');
                if (o.calificacion == 0) {
                    ConfirmacionGeneral("Confirmación", "¡No todas las preguntas han sido contestadas! Favor de responderlas.");
                    validacion = true;
                }
                else if (o.calificacion < 3 && (o.respuesta == "" || o.respuesta == null || o.respuesta == undefined)) {
                    ConfirmacionGeneral("Confirmación", "¡No todas las preguntas han sido contestadas! Favor de responderlas.");
                    validacion = true;
                }
                if (o.calificacion > 3) {
                    totalPonderacion += Number(o.ponderacion);
                }

                preguntasList.push(o);
            });

            if (totalPonderacion <= 0.74) {
                if (txtComentario.val().trim('') == "") {
                    ConfirmacionGeneral("Confirmación", "¡Calificaciones Regulares y Malos deben llevar un Comentario Obligatorio.");
                    validacion = true;
                }

            }

            if (validacion == false) {
                $.blockUI({});
                switch (tipoDependencia) {
                    case 1:
                        $.ajax({
                            datatype: "json",
                            type: "POST",
                            url: "/Encuestas/EncuestasProveedor/saveEncuestaResultReq",
                            data: { obj: preguntasList, objSingle: getObjRequisiciones(), encuestaID: _encuestaID, tipoEncuesta: cboTipoEncuestaRespuestas.val() },
                            asyn: false,
                            success: function (response) {
                                if (response.Success) {
                                    ConfirmacionGeneral("Confirmación", "¡Encuesta contestada correctamente!");
                                    vaciarCampos();
                                    cboTipoEncuestaRespuestas.trigger('change');
                                } else {
                                    AlertaGeneral('Alerta', response.Message);
                                }
                            },
                            error: function () {
                                AlertaGeneral('Alerta', null);
                                $.unblockUI();
                            }
                        });
                        break;
                    case 2:
                        $.ajax({
                            datatype: "json",
                            type: "POST",
                            url: "/Encuestas/EncuestasProveedor/saveEncuestaResult",
                            data: { obj: preguntasList, objSingle: getObjDetalle(), encuestaID: _encuestaID, tipoEncuesta: cboTipoEncuestaRespuestas.val() },
                            asyn: false,
                            success: function (response) {
                                if (response.Success) {
                                    ConfirmacionGeneral("Confirmación", "¡Encuesta contestada correctamente!");
                                    vaciarCampos();
                                    cboTipoEncuestaRespuestas.trigger('change');
                                } else {
                                    AlertaGeneral('Alerta', response.Message);
                                }
                            },
                            error: function () {
                                AlertaGeneral('Alerta', null);
                                $.unblockUI();
                            }
                        });
                        break;
                    default:
                        break;
                }

                $.unblockUI({});
            }
        }

        function getObjRequisiciones() {
            return {
                id: 0,
                centroCostos: null,
                numeroRequisicion: 0,
                nombreProveedor: tbProveedor.val(),
                numProveedor: tbProveedor.attr('data-idProveedor'),
                comentarios: txtComentario.val(),
                fechaRequisicion: null,
                tipoEncuesta: _encuestaID
            };
        }

        function vaciarCampos() {
            tbOrdenCompraFiltro.val('');
            tbProveedor.val('');
            tbProveedor.removeAttr('data-idproveedor');
            tbProveedor.removeAttr('data-cc');
            tbEvaluacion.val('');
            tbTipoMoneda.val('');
            tbAntiguedadProveedor.val('');
            tbUbicacionProveedor.val('');
            tbEvaluador.val('');
            tbTituloPregunta.val('');
            $(".Preguntas").empty();
            txtComentario.val('');
            txtDescripcionPregunta.val('');
            divAreaRespuesta.addClass('hide');

            _encuestaID = null;
            _tipoProveedor = null;
        }

        //#region EVALUACION ESTRELLAS
        function cargarGraficasEstrellas() {
            $.blockUI({ message: 'mensaje' });
            url = '/Encuestas/EncuestasProveedor/SetGraficasEvaluacionesEstrellas';

            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'json',
                data: { fechaInicio: txtFechaInicioEstrellas.val(), fechaFin: txtFechaFinEstrellas.val(), encuesta: cboTipoEncuestaEstrellas.val(), tipoEncuesta: cboEncuestaEstrellas.val(), compradores: getValoresMultiples("#cboCompradoresEstrellas") },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        setGraficaEvaluacionesEstrellas(response.resultPesimo, response.resultGraphMalo, response.resultGraphRegular, response.resultGraphAceptable, response.resultGraphEstupendo);
                        SetGraficaCompradoresEstrellas(response.dtCompradores);
                        LoadRevisionProveedoresEstrellas(response.dtEncuestasEstrellas);
                        LoadProveedoresListaEstrellas(response.ResultListaProveedores);

                        cboListaProveedoresEvaluadosEstrellas.fillCombo('/Encuestas/EncuestasProveedor/loadCboProveedores', null, false, "Todos");
                        convertToMultiselect("#cboListaProveedoresEvaluadosEstrellas");

                        let encuestaTitulo = cboEncuestaEstrellas.find('option:selected').text();
                        setGraficaPreguntas(response.preguntas, response.calificaciones, 'containerHC', 'Resultado por preguntas', '');
                        setGraficaPreguntas(response.provMasEvaluados, response.calificacionesMasEvaluados, 'containerHCMasEvaluados', 'Proveedores más evaluados', '');
                        setGraficaPreguntas(response.provPeorEvaluados, response.calificacionesPeorEvaluados, 'containerHCPeores', 'Top 5 proveedores con baja calificación', '');
                        setGraficaPreguntas(response.provBest, response.calificacionesBest, 'containerHCBest', 'Top 10 mejores evaluados', '');
                    } else {
                        AlertaGeneral('Alerta', response.message);
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    alert(response.message);
                }
            });
        }

        function setGraficaEvaluacionesEstrellas(pesimo, malo, regular, aceptable, estupendo) {

            if (chartEvaluacionesEstrellas != null) {
                chartEvaluacionesEstrellas.destroy();
            }

            var ctx1 = document.getElementById("canvasEstrellasGeneral").getContext("2d");
            var data = {
                labels: ["Pésimo", "Malo", "Regular", "Aceptable", "Estupendo"],
                datasets: [{
                    label: "Evaluaciones",
                    backgroundColor: ["rgba(178,34,34, 0.5)", "rgba(255, 255, 0, 0.5)", "rgba(255, 128, 0, 0.5)", "rgba(0,153,0,0.5)", "rgba(0,0,204,0.5)"],
                    borderColor: ["rgba(178,34,34, 0.5)", "rgba(255, 255, 0, 0.5)", "rgba(255, 128, 0, 0.5)", "rgba(0,153,0,0.5)", "rgba(0,153,0,0.5)"],
                    borderWidth: 1,
                    data: [pesimo, malo, regular, aceptable, estupendo],
                    fill: true
                }
                ]
            };
            dtLabel = ["Pésimo", "Malo", "Regular", "Aceptable", "Estupendo"];

            chartEvaluacionesEstrellas = new Chart(ctx1, {
                type: 'bar',
                data: data,
                options:
                {
                    responsive: false,
                    elements: {
                        line: {
                            fill: false
                        }
                    },
                    scales: {
                        xAxes: [{
                            display: true,
                            gridLines: {
                                display: false
                            },
                            labels: {
                                show: true,
                            }, ticks: {
                                autoSkip: false,

                            }
                        }],
                    },
                    animation: {
                        duration: 1,
                        onComplete: function () {
                            var chartInstance = this.chart,
                                ctx = chartInstance.ctx;

                            if (ctx.canvas.id == 'canvasEstrellasGeneral') {
                                ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                                ctx.fillStyle = "#000000";
                                ctx.textAlign = 'center';
                                ctx.textBaseline = 'bottom';

                                this.data.datasets.forEach(function (dataset, i) {
                                    var meta = chartInstance.controller.getDatasetMeta(i);
                                    if (dataset.type != 'line') {
                                        meta.data.forEach(function (bar, index) {

                                            var dato = Number(dataset.data[index]);
                                            data = dato;
                                            ctx.fillText(data, bar._model.x, bar._model.y - 5);
                                        });
                                    }

                                });
                            }
                        }
                    }

                }
            });

        }

        function saveCanvasEvaluacionEstrellas() {
            var canvas = $('#canvasEstrellasGeneral').get(0);
            canvas.toBlob(function (blob) {
                saveAs(blob, "EvaluacionProveedores.png");
            });

        }

        function SetGraficaCompradoresEstrellas(Compradores) {
            if (chartCompradoresEstrellas != null) {
                chartCompradoresEstrellas.destroy();
            }

            var dt = Compradores;

            var dtSetInfo = new Array();
            var dtLabel = new Array();
            var dbColores = new Array();


            for (var i = 0; i < dt.length; i++) {
                var colorName = GetColor();
                dtSetInfo.push(dt[i].CantidadEvaluaciones);
                dtLabel.push(dt[i].nombreComprador);
                dbColores.push(colorName);
            }

            var ctx2 = document.getElementById("canvasEstrellasCompradores").getContext("2d");

            var data = {
                labels: dtLabel,
                datasets: [{
                    label: "Compradores",
                    backgroundColor: dbColores,
                    borderColor: dbColores,
                    borderWidth: 1,
                    data: dtSetInfo,
                    fill: false,
                }]
            };

            chartCompradoresEstrellas = new Chart(ctx2, {
                type: 'bar',
                data: data,
                options:
                {
                    responsive: false,
                    tooltips: {
                        mode: 'label'
                    },
                    elements: {
                        line: {
                            fill: false
                        }
                    },
                    scales: {
                        xAxes: [{
                            ticks: {
                                autoSkip: false,

                            }
                        }],
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    },
                    animation: {
                        duration: 1,
                        onComplete: function () {
                            var chartInstance = this.chart,
                                ctx = chartInstance.ctx;

                            if (ctx.canvas.id == 'canvasEstrellasCompradores') {
                                ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                                ctx.fillStyle = "#000000";
                                ctx.textAlign = 'center';
                                ctx.textBaseline = 'bottom';

                                this.data.datasets.forEach(function (dataset, i) {
                                    var meta = chartInstance.controller.getDatasetMeta(i);
                                    if (dataset.type != 'line') {
                                        meta.data.forEach(function (bar, index) {

                                            var dato = Number(dataset.data[index]);
                                            data = dato;
                                            ctx.fillText(data, bar._model.x, bar._model.y - 5);
                                        });
                                    }

                                });
                            }
                        }
                    }
                }
            });
        }

        setGraficaPreguntas = (preguntas, calificaciones, idGrafica, titulo, subtitulo) => {
            var chart = Highcharts.chart(idGrafica, {
                // chart: {
                //     scrollablePlotArea: {
                //         minWidth: 1300,
                //         scrollPositionX: 1,
                //         opacity: 1
                //     }
                // },
                title: {
                    text: titulo
                },

                subtitle: {
                    text: cboEncuestaEstrellas.find('option:selected').text() + subtitulo,
                    useHTML: true
                },

                xAxis: {
                    categories: preguntas,
                    labels: {
                        useHTML: true,
                        format: '<div style="text-align:center"><span>{value}</span></div>',
                        style: {
                            fontSize: '9px'
                        },
                        //rotation: (idGrafica == 'containerHCMasEvaluados' ? -90 : 0)
                        // reserveSpace: false,
                        // align: 'left',
                        // y: -5
                    }
                },
                yAxis: {
                    title: { text: '' },
                    min: 0,
                    max: 100,
                    startOnTick: true,
                    endOnTick: true
                },
                series: [{
                    type: 'column',
                    colorByPoint: true,
                    data: calificaciones,
                    showInLegend: false,
                    name: 'Calificación',
                    dataLabels: {
                        enabled: true
                    }
                }],
                exporting: {
                    // sourceWidth: 1300,
                    // sourceHeight: 400,
                    // chartOptions: {
                    //     //subtitle: null
                    // }
                },
                credits: {
                    enabled: false
                }
            });
        }

        function setGraficaPreguntasProv(numProv) {
            $.blockUI({ message: 'procesando...' });
            $.post('/Encuestas/EncuestasProveedor/setGraficaPreguntasProv',
                {
                    numProv: numProv,
                    tipoEncuesta: cboTipoEncuestaEstrellas.val(),
                    encuestaID: cboEncuestaEstrellas.val(),
                    fechaIni: txtFechaInicioEstrellas.val(),
                    fechaFin: txtFechaFinEstrellas.val(),
                    listaUsuario: getValoresMultiples("#cboCompradoresEstrellas")
                }).then(response => {
                    $.unblockUI();
                    if (response.Success) {
                        setGraficaPreguntas(response.Value.preguntas, response.Value.calificaciones, 'containerHCProv', 'Resultado preguntas', ': <strong>' + modalTitulo.text() + '</strong>');
                        modalDetalleProv.modal('show');
                    } else {
                        AlertaGeneral(`Alerta`, response.Message);
                    }
                }, error => {
                    $.unblockUI();
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function setGraficaMensualProv(numProv) {
            $.blockUI({ message: 'procesando...' });
            $.post('/Encuestas/EncuestasProveedor/setGraficaMensualProv',
                {
                    numProv: numProv,
                    tipoEncuesta: cboTipoEncuestaEstrellas.val(),
                    encuestaID: cboEncuestaEstrellas.val(),
                    listaUsuario: getValoresMultiples("#cboCompradoresEstrellas")
                }).then(response => {
                    $.unblockUI();
                    if (response.Success) {
                        setGraficaPreguntas(response.Value.preguntas, response.Value.calificaciones, 'containerHCProv', 'Resultado mensual', ': <strong>' + modalTitulo.text() + '</strong>');
                        modalDetalleProv.modal('show');
                    } else {
                        AlertaGeneral(`Alerta`, response.Message);
                    }
                }, error => {
                    $.unblockUI();
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function LoadRevisionProveedoresEstrellas(dataSet) {
            tblProveedoresEstrellas = $('#tblProveedoresEstrellas').DataTable({
                language: dtDicEsp,
                responsive: true,
                bFilter: true,
                order: false,
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [
                    { data: 'proveedorID' },
                    { data: 'proveedorName' },
                    { data: 'tipoMoneda' },
                    { data: 'tipoProveedor' },
                    { data: 'nombreEvaluador' },
                    { data: 'fechaEvaluacion' },
                    { data: 'comentario' }
                ],
                paging: true,
                info: false,
                dom: 'Bfrtip',
                columnDefs: [
                    {
                        targets: [1],
                        className: 'text-center'
                    },
                    {
                        targets: [5],
                        render: function (data, type, row) {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    }
                ],
                headerCallback: function (thead, data, start, end, display) {
                    $(thead).children().addClass('text-center');
                },
                drawCallback: function (settings) {
                    $.unblockUI();
                },
                buttons: parametrosImpresion('Reporte de actividades', '')
            });
        }

        function LoadProveedoresListaEstrellas(dataSet) {
            tblProveedoresEvaluadosEstrellas = $('#tblProveedoresEvaluadosEstrellas').DataTable({
                language: dtDicEsp,
                responsive: true,
                bFilter: true,
                order: false,
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [
                    { data: 'proveedorName' },
                    { data: 'tipoMoneda' },
                    { data: 'cantidadPesimos' },
                    { data: 'cantidadMalos' },
                    { data: 'cantidadRegulares' },
                    { data: 'cantidadAceptables' },
                    { data: 'cantidadEstupendos' },
                    { data: 'porcentaje' },
                    { data: 'proveedorID' }
                ],
                columnDefs: [
                    {
                        targets: [7],
                        render: function (data, type, row) {
                            return maskNumero(data).substring(1) + '%';
                        }
                    },
                    {
                        targets: [8],
                        render: function (data, type, row) {
                            return '<button class="btn btn-success detallesProv" data-numprov="' + row.proveedorID + '" data-nomprov="' + row.proveedorName + '"><i class="fas fa-bars"></i></button>'
                        }
                    },
                    {
                        targets: [9],
                        render: function (data, type, row) {
                            return '<button class="btn btn-success detallesProvMensual" data-numprov="' + row.proveedorID + '" data-nomprov="' + row.proveedorName + '"><i class="fas fa-bars"></i></button>';
                        }
                    }
                ],
                paging: true,
                info: false,
                dom: 'Bfrtip',
                drawCallback: function (settings) {
                    $.unblockUI();
                },
                buttons: parametrosImpresion("Reporte de actividades", "")
            });
        }
        //#endregion

        init();
    };

    $(document).ready(function () {

        EncuestasProveedor.Dashboard = new Dashboard();
    });

})();

function responderEncuesta(cc, numOC) {

    if ($("#cboEncuestas").val() != "") {
        encuestaID = $("#cboEncuestas").val();

        window.location.href = "/Encuestas/EncuestasProveedor/Responder/?encuesta=" + encuestaID + "&CC=" + cc + "&numOC=" + numOC + "&tipoEncuesta=" + 1;
    }
    else {
        AlertaGeneral("Alerta", "Se debe seleccionar una encuesta antes de poder continuar.");
    }
}

function responderEncuestaRequisicion(cc, numRequisicion) {
    if ($("#cboEncuestas").val() != "") {
        encuestaID = $("#cboEncuestas").val();
        window.location.href = "/Encuestas/EncuestasProveedor/Responder/?encuesta=" + encuestaID + "&CC=" + cc + "&requisicion=" + numRequisicion + "&tipoEncuesta=" + 2;
    }
    else {
        AlertaGeneral("Alerta", "Se debe seleccionar una encuesta antes de poder continuar.");
    }
}

function verReporte(proveedoresID, tipo) {

    var reporte = 0;
    if (tipo == 1) {
        reporte = 74;
        $.blockUI({ message: mensajes.PROCESANDO });
        var path = "/Reportes/Vista.aspx?idReporte=" + reporte + "&encuestaProveedorDet=" + proveedoresID + "&tipo=" + tipo;
        $("#report").attr("src", path);
        document.getElementById('report').onload = function () {
            $.unblockUI();
            openCRModal();
        };
    }
    else {
        reporte = 75;
        $.blockUI({ message: mensajes.PROCESANDO });
        var path = "/Reportes/Vista.aspx?idReporte=" + reporte + "&encuestaProveedorDet=" + proveedoresID + "&tipo=" + tipo;
        $("#report").attr("src", path);
        document.getElementById('report').onload = function () {
            $.unblockUI();
            openCRModal();
        };
    }


}