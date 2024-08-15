(function () {

    $.namespace('maquinaria.overhaul.talleroverhaul');

    talleroverhaul = function () {

        mensajes = {
            NOMBRE: 'Componentes',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        let configEditor = {
            codeview: {
                enabled: false,
            }
        };
        let meses = ['ENERO', 'FEBRERO', 'MARZO', 'ABRIL', 'MAYO', 'JUNIO', 'JULIO', 'AGOSTO', 'SEPTIEMBRE', 'OCTUBRE', 'NOVIEMBRE', 'DICIEMBRE'];
        let componentes = [];    
        let idFallaGlobal = 0;
        
        ulNuevo = $("#ulNuevo"),
        // Estatus
        tabEstatus = $("#tabEstatus"),
        fechaFinParo= $("#fechaFinParo"),
        modalFechaFin = $("#modalFechaFin"),
        tabRefaccionamiento = $("#tabRefaccionamiento"),
        tabReportes = $("#tabReportes"),
        estatus = $("#estatus"),
        btnBuscarEstatus = $("#btnBuscarEstatus"),
        gridEstatusTaller = $("#gridEstatusTaller"),
        modalDetallesEstatus = $("#modalDetallesEstatus"),
        titlemodalEstatus = $("#titlemodalEstatus"),
        cboModalEconomicoEstatus = $("#cboModalEconomicoEstatus"),
        txtModalComponenteEstatus = $("#txtModalComponenteEstatus"),
        btnBuscarModalEstatus = $("#btnBuscarModalEstatus"),
        modalGuardarComentario = $("#modalGuardarComentario"),
        btnModalGuardarComentario = $("#btnModalGuardarComentario"),
        tbComentario = $("#tbComentario"),
        btnAgregarOverhaulEstatus = $("#btnAgregarOverhaulEstatus"),
        modalNuevoOverhaul = $("#modalNuevoOverhaul"),
        cboGrupoModalNuevo = $("#cboGrupoModalNuevo");
        cboModeloModalNuevo = $("#cboModeloModalNuevo"),
        cboEconomicoModalNuevo = $("#cboEconomicoModalNuevo"),
        gridModalGantt = $("#gridModalGantt"),
        btnModalGuardarNuevo = $("#btnModalGuardarNuevo"),
        cboCalendarioEstatus = $("#cboCalendarioEstatus"),
        titleModalNuevo = $("#title-modal-nuevo"),
        btnModalIniciarNuevo = $("#btnModalIniciarNuevo"),
        modalActividadesOverhaul = $("#modalActividadesOverhaul"),
        gridActividadesOverhaul = $("#gridActividadesOverhaul"),
        btnModalPortada = $("#btnModalPortada"),
        btnModalEvidencia = $("#btnModalEvidencia"),
        btnModalCancelarActividades = $("#btnModalCancelarActividades"),
        btnModalTerminarOverhaul = $("#btnModalTerminarOverhaul"),
        btnTerminarOverhaul = $("#btnTerminarOverhaul"),
        modalGuardarArchivo = $("#modalGuardarArchivo"),
        btncargarArchivo = $("#btncargarArchivo"),
        inCargarArchivo = $("#inCargarArchivo"),
        gridArchivos = $("#gridArchivos"),
        modalOverhaulFalla = $("#modalOverhaulFalla"),
        gridOHFalla = $("#gridOHFalla"),
        cboEconomicoFalla = $("#cboEconomicoFalla"),
        btnModalOverhaulFalla = $("#btnModalOverhaulFalla"),
        btnModalGuardarComentarioReporte = $("#btnModalGuardarComentarioReporte"),
        //tbComentarioReporte = $("#tbComentarioReporte"),
        modalGuardarArchivoFL = $("#modalGuardarArchivoFL"),
        gridArchivosFL = $("#gridArchivosFL"),
        btncargarArchivoFL = $("#btncargarArchivoFL"),
        inCargarArchivoFL = $("#inCargarArchivoFL"),
        btnEnviarLiberacion = $("#btnEnviarLiberacion"),
        modalGuardarPortada = $("#modalGuardarPortada"),
        btncargarPortada = $("#btncargarPortada"),
        inCargarPortada = $("#inCargarPortada"),
        tblPortada = $("#tblPortada"),
        //tbComentarioPortada = $("#tbComentarioPortada"),
        btnModalGuardarComentarioPortada = $("#btnModalGuardarComentarioPortada"),
        modalGuardarEvidencia = $("#modalGuardarEvidencia"),
        btncargarEvidencia = $("#btncargarEvidencia"),
        inCargarEvidencia = $("#inCargarEvidencia"),
        tblEvidencia = $("#tblEvidencia"),
        btnModalGuardarEvidencia = $("#btnModalGuardarEvidencia"),
        ireport = $("#report"),
        tblCorreos = $("#tblCorreos"),
        modalCorreos = $("#modalCorreos"),
        agregarCorreo = $("#agregarCorreo"),
        btnEnviarCorreo = $("#btnEnviarCorreo"),
        fechaInicioParo = $("#fechaInicioParo"),
        //Diagrama de Gantt
        modalAgregarAct = $("#modalAgregarAct"),
        tblAgregAct = $("#tblAgregAct"),
        tblDiagrama = $("#tblDiagrama"),
        btnCargarAgrAct = $("#btnCargarAgrAct");
        txtHorasTrabajadas = $("#txtHorasTrabajadas");

        const cboTipoFalla = $('#cboTipoFalla');
        let idObra = 0;
        let idEvento = 0;
        let idCalendario = -1;
        let actividadesID = [];
        let anio = 0;
        let tbComentarioPortada = textboxio.replace('#tbComentarioPortada', configEditor);
        let tbComentarioReporte = textboxio.replace('#tbComentarioReporte', configEditor);
        let tipoUsuario = 6;
        let tipoCorreo = 0;
        let selectedOpts = '';
        let diaActualDG = 0;
        let restaHoras = 0;
        let dtGridActividadesOverhaul;


        //Catálogo de actividades
        const btnCatActividades = $("#btnCatActividades");
        const modalCatActividades = $("#modalCatActividades");
        const descripcionCatAct = $("#descripcionCatAct");
        const horasCatActUpdate = $("#horasCatActUpdate");
        const cbModeloCatAct = $("#cbModeloCatAct");
        const cbEstatusCatAct = $("#cbEstatusCatAct");
        const btnBuscarCatAct = $("#btnBuscarCatAct");
        const btnAgregarCatAct = $("#btnAgregarCatAct");        
        const tblCatActividades = $("#tblCatActividades");
        let dtTblCatActividades;

        const modalCatActUpdate = $("#modalCatActUpdate");
        const lblCatActUpdate = $("#lblCatActUpdate");
        const descripcionCatActUpdate = $("#descripcionCatActUpdate");
        const cbModeloCatActUpdate = $("#cbModeloCatActUpdate");
        const cbEstatusCatActUpdate = $("#cbEstatusCatActUpdate");
        const ckbRECatActUpdate = $("#ckbRECatActUpdate");
        const btnGuardarCatActUpdate = $("#btnGuardarCatActUpdate");

        const btnAgregarAct = $("#btnAgregarAct");

        //
        const cboEstatusTaller = $("#cboEstatusTaller");
        const cboTipoParo = $("#cboTipoParo");        

        function init() {
            PermisosBotones();
            // Estatus
            IniciarTablas();      
            cboTipoFalla.val("3");   
            btnBuscarEstatus.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                CargarGridEstatus();
                
            });
            btnModalGuardarComentario.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                GuardarComentario();
            });
            btnModalGuardarComentarioReporte.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                GuardarComentarioReporte();
            });
            btnAgregarOverhaulEstatus.click(OpenModalNuevo);
            btnModalGuardarNuevo.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                guardarGantt();
            });
            //btnModalIniciarNuevo.click(IniciarOverhaul);
            btnTerminarOverhaul.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                TerminarOverhaul();
            });
            btnModalTerminarOverhaul.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 3 || tipoUsuario == 7) {
                    btnTerminarOverhaul.css("display", "block");
                    btnModalGuardarComentario.css("display", "block");
                    tbComentario.prop("disabled", false);
                }
                else
                {
                    btnTerminarOverhaul.css("display", "none");
                    btnModalGuardarComentario.css("display", "none");
                    tbComentario.prop("disabled", true);
                }                        
                btnModalGuardarNuevo.attr("data-estatus", $(this).attr("data-estatus"));
                btnTerminarOverhaul.attr("data-index", $(this).attr("data-index"));
                btnTerminarOverhaul.attr("data-economico", $(this).attr("data-economico"));
                btnTerminarOverhaul.attr("data-tipo", $(this).attr("data-tipo"));
                btnTerminarOverhaul.attr("data-estatus", $(this).attr("data-estatus"));
           
                $("#modalFechaFin").modal("show")
                
            });
            btnModalPortada.click(OpenModalPortada);
            btnModalEvidencia.click(OpenModalEvidencia);
            btnModalGuardarComentarioPortada.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                GuardarComentarioPortada();
            });
            cboCalendarioEstatus.fillCombo("/Overhaul/CargarCalendariosGuardadosTaller");
            cboEconomicoFalla.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                CargarGridOHFalla();
            });
            btnModalOverhaulFalla.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                cboTipoFalla.val("3");
                var tipo = btnModalOverhaulFalla.attr("data-tipo");
                if (tipo == 1) {
                    var index = btnModalOverhaulFalla.attr("data-planeacionID");
                    componentes = [];
                    $("input.agregar").each(function () {
                        componentes.push({
                            Value: "0",
                            componenteID: $(this).attr('data-componenteID'),
                            nombre: $(this).attr('data-nombre'),
                            descripcion: $(this).attr('data-descripcion'),
                            posicion: $(this).attr('data-posicion'),
                            Tipo: $(this).attr('data-tipo'),
                            horasCiclo: $(this).attr('data-horasCiclo'),
                            target: $(this).attr('data-target'),
                            tipoOverhaul: cboTipoFalla.val(),
                            falla: cboTipoFalla.val() == 3? true:false,
                        });
                    });
                    $.blockUI({ message: "Procesando..." });
                    $.ajax({
                        url: '/Overhaul/UpdateOHFallaTaller',
                        datatype: "json",
                        type: "POST",
                        data: {
                            id: index,
                            componentes: componentes,
                            calendarioID: idCalendario
                        },
                        success: function (response) {
                            $.unblockUI();
                            if (response.success) {
                                if (response.exito) {
                                    CargarGridEstatus();
                                    AlertaGeneral("Éxito", "Se guardó Overhaul correctamente");
                                    modalOverhaulFalla.modal("hide");
                                }
                                else {
                                    AlertaGeneral("Alerta", "No es posible guardar el paro porque un componente de la lista pertenece a un paro abierto");
                                }
                            }
                            else { AlertaGeneral("Alerta", "Se encontró un error al guardar los datos"); }
                        },
                        error: function (response) {
                            $.unblockUI();
                            AlertaGeneral("Alerta", "Se encontró un error al guardar los datos");
                        }
                    });
                } else {
                    GuardarOHFalla();
                }                
            });
            agregarCorreo.click(AgregarCorreo);
            ReacomodoTablas();
            btnEnviarCorreo.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                if (tipoCorreo == 0) { IniciarOverhaul(); }
                else { EnviarLiberacion(); }
            });
            $(document).on('click', "#btnModalEliminar", function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                if (btnModalGuardarNuevo.attr("data-estatus") < 2) {
                    tipoCorreo = 0;
                    $("#lblCorreos").text("Inicio de Overhaul");
                    $("#txtCorreo").val('');
                    var listaCorreos = [1, 1, 0, 0, 1, 1];
                    CargartablaCorreos(tblCorreos, listaCorreos);
                    modalCorreos.modal("show");
                }
                if (btnModalGuardarNuevo.attr("data-estatus") == 2) TerminarOverhaul();
            });
            //Boton de confirmacion que agrega paro a un paro existente
            $(document).on('click', "#btnModalBoton1", function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                $.blockUI({ message: "Procesando..." });
                $.ajax({
                    url: '/Overhaul/UpdateOHFallaTaller',
                    datatype: "json",
                    type: "POST",
                    data: {
                        id: idFallaGlobal,
                        componentes: componentes,
                        calendarioID: idCalendario
                    },
                    success: function (response) {
                        $.unblockUI();
                        if (response.success) {
                            if (response.exito) {
                                CargarGridEstatus();
                                AlertaGeneral("Éxito", "Se guardó Overhaul correctamente");
                                modalOverhaulFalla.modal("hide");
                                idFallaGlobal = 0;
                            }
                            else {
                                AlertaGeneral("Alerta", "No es posible guardar el paro porque un componente de la lista pertenece a un paro abierto");
                            }                            
                        }
                        else { AlertaGeneral("Alerta", "Se encontró un error al guardar los datos"); }
                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", "Se encontró un error al guardar los datos");
                    }
                });
            });
            //Boton de confirmacion que agrega un paro nuevo
            $(document).on('click', "#btnModalBoton2", function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                $.blockUI({ message: "Procesando..." });
                $.ajax({
                    url: '/Overhaul/GuardarOHFallaTaller',
                    datatype: "json",
                    type: "POST",
                    data: {
                        idMaquina: cboEconomicoFalla.val(),
                        componentes: componentes,
                        mes: btnModalOverhaulFalla.attr("data-mes"),
                        anio: anio,
                        calendarioID: idCalendario,
                        tipo: cboTipoFalla.val()
                    },
                    success: function (response) {
                        $.unblockUI();
                        if (response.success) {
                            CargarGridEstatus();
                            AlertaGeneral("Éxito", "Se guardó Overhaul correctamente");
                            modalOverhaulFalla.modal("hide");
                        }
                        else { AlertaGeneral("Alerta", "Se encontró un error al guardar los datos"); }
                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", "Se encontró un error al guardar los datos");
                    }
                });
            });

            btnModalIniciarNuevo.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                tipoCorreo = 0;
                $("#lblCorreos").text("Inicio de Overhaul");
                $("#txtCorreo").val('');
                var listaCorreos = [1, 1, 0, 0, 1, 1];
                CargartablaCorreos(tblCorreos, listaCorreos);
                modalCorreos.modal("show");
            });
            btnEnviarLiberacion.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                tipoCorreo = 1;
                $("#lblCorreos").text("Liberación de Equipo");
                $("#txtCorreo").val('');
                var listaCorreos = [1, 1, 0, 0, 1, 0, 1];
                CargartablaCorreos(tblCorreos, listaCorreos);
                modalCorreos.modal("show");
            });
            btncargarArchivo.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                inCargarArchivo.click();      
            });
            inCargarArchivo.change(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                SubirArchivoActividad(e, 0, "inCargarArchivo");
                CargarGridArchivos(btncargarArchivo.attr("data-index"), btnTerminarOverhaul.attr("data-index"), btnModalGuardarComentarioReporte.attr("data-numDia"));
            });
            btncargarPortada.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                inCargarPortada.click();
            });
            inCargarPortada.change(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                SubirArchivoActividad(e, 2, "inCargarPortada");
                CargarTblPortada(btncargarPortada.attr("data-index"));
            });
            btncargarEvidencia.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                inCargarEvidencia.click();
            });
            inCargarEvidencia.change(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                SubirArchivoActividad(e, 3, "inCargarEvidencia");
                CargarTblPortada(btncargarEvidencia.attr("data-index"));
            });
            btncargarArchivoFL.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                inCargarArchivoFL.click();
            });
            inCargarArchivoFL.change(function (e) {
                SubirArchivoActividad(e, 1, "inCargarArchivoFL");
                CargarGridArchivosFL(-1, btncargarArchivoFL.attr("data-index"));
            });
            gridEstatusTaller.on('click', 'td.details-control', function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                var tr = $(this).closest('tr');
                var row = gridEstatusTaller.row(tr);
                if (row.child.isShown()) {
                    row.child.hide();
                    tr.removeClass('shown');
                }
                else {
                    row.child(format(row.data())).show();
                    tr.addClass('shown');
                }
            });
            modalActividadesOverhaul.on('hidden.bs.modal', function () {
                btnModalPortada.css("display", "none");
                btnModalEvidencia.css("display", "none");
                //gridActividadesOverhaul.column(7).visible(false);
            });

            //Diagrama de Gantt
            btnCargarAgrAct.click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                let rows = tblAgregAct.bootgrid().data('.rs.jquery.bootgrid').rows;
                AgregarATblDiagrama(rows, parseFloat(txtHorasTrabajadas.val()));
            });
            
            $("#txtHorasTrabajadas").on("change", function (e) {
                e.preventDefault();
            });

            $("#txtHorasTrabajadas").on("keyup", function (e) {
                e.preventDefault();
                RecargarTblDiagrama();
            });
            
            $('#btnRight').click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                selectedOpts = $('#lstBox1 option:selected');
                if (selectedOpts.length == 0) {
                    e.preventDefault();
                }
                mostrarModalAgrAct();
            });
            $('#btnLeft').click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                let auxNumRows = tblDiagrama.rows().data().length;
                if (auxNumRows > 0) {
                    last_row = tblDiagrama.row(':last').data();
                    let actividadIDRem = last_row.id;
                    while (last_row != null && last_row.id == actividadIDRem) {
                        tblDiagrama.row(':last').remove();
                        last_row = tblDiagrama.row(':last').data();
                    }
                    let dia_last_row = 1;
                    let sumatoriaHoras = 0;
                    if (last_row != null) {
                        dia_last_row = last_row.dia;
                        let dias_rows = tblDiagrama.rows().data().flatten().filter(function (value, index) { return value.dia == dia_last_row ? true : false; });
                        for (let i = 0; i < dias_rows.length; i++) { sumatoriaHoras += dias_rows[i].duracion; }
                    }
                    diaActualDG = dia_last_row;
                    restaHoras = parseFloat(txtHorasTrabajadas.val()) - sumatoriaHoras;
                    tblDiagrama.draw();
                }
            });
            inicioDatePickers();

            //Catálogo de actividades
            btnCatActividades.click(cargarCatAct);
            initTblCatAct();
            cbModeloCatAct.select2({ dropdownParent: $("#modalCatActividades") });
            cbModeloCatAct.fillCombo('/CatComponentes/FillCboModelo_Componente');

            cbModeloCatActUpdate.select2({ dropdownParent: $("#modalCatActUpdate") });
            cbModeloCatActUpdate.fillCombo('/CatComponentes/FillCboModelo_Componente');

            btnBuscarCatAct.click(cargarTblCatAct);

            btnAgregarCatAct.click(AltaCatAct);

            btnGuardarCatActUpdate.click(GuardarCatAct);
            modalCatActUpdate.on('hidden.bs.modal', function () {
                $('body').addClass('modal-open');
            });

            btnAgregarAct.click(AltaCatActPorModelo);

        }

        function PermisosBotones() {
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: "/Overhaul/PermisosBotonesAdminComp",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                success: function (response) {
                    $.unblockUI();
                    tipoUsuario = response.tipoUsuario;
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function inicioDatePickers()
        {
            fechaInicioParo.datepicker().datepicker("setDate", new Date());
            fechaFinParo.datepicker().datepicker("setDate", new Date());
        }

        function IniciarTablas() {
            IniciarGridEstatus();
            InitGridGantt();
            InitGridActividades();
            IniciarGridArchivos();
            IniciarGridArchivosFL();
            IniciarGridOHFalla();
            IniciarTblPortada();
            IniciarTblEvidencia();
            initTblCorreos();
            initTblAgrAct();
            IniciarTblDiagrama();
        }

        function ReacomodoTablas()
        {
            $('#modalNuevoOverhaul').on('shown.bs.modal', function () {
                gridModalGantt.columns.adjust();
            });
            $('#modalActividadesOverhaul').on('shown.bs.modal', function () {
                dtGridActividadesOverhaul.columns.adjust();
            });
            $('#modalGuardarArchivo').on('shown.bs.modal', function () {
                gridArchivos.columns.adjust();
            });
            $('#modalOverhaulFalla').on('shown.bs.modal', function () {
                gridOHFalla.columns.adjust();
            });
            $('#modalGuardarArchivoFL').on('shown.bs.modal', function () {
                gridArchivosFL.columns.adjust();
            });
            $('#modalGuardarPortada').on('shown.bs.modal', function () {
                tblPortada.columns.adjust();
            });
            $('#modalGuardarPortada').on('shown.bs.modal', function () {
                tblPortada.columns.adjust();
            });
            $('#modalGuardarEvidencia').on('shown.bs.modal', function () {
                tblEvidencia.columns.adjust();
            });
        }

        function IniciarGridEstatus() {            
            gridEstatusTaller = $("#gridEstatusTaller").DataTable({
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
                scrollY: '70vh',
                scrollCollapse: true,
                autoWidth: false,
                dom: '<<"fg-toolbar"><t>>',
                //ordering: false,
                select: {
                    style: 'os',
                    selector: 'td:first-child'
                },
                "rowCallback": function (row, data, index) {
                    $('td', row).css('text-align', 'center');
                    switch (data.tipoOverhaul) {
                        case 0:
                            $('td', row).eq(0).css('background-color', '#5cb85c');
                            $('td', row).eq(0).css('color', 'white');
                            break;
                        case 1:
                            $('td', row).eq(0).css('background-color', '#204d74');
                            $('td', row).eq(0).css('color', 'white');
                            break;
                        case 3:
                            $('td', row).eq(0).css('background-color', '#ff1919');
                            $('td', row).eq(0).css('color', 'white');
                            break;
                    }
                },
                "drawCallback": function (settings, json) {
                    
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;
                    var d = new Date();
                    var n = d.getMonth();
                    $("div.fg-toolbar:eq(0)").html('<button type="button" data-index="' + idCalendario + '" data-mes="' + (n + 1) + '" class="btn-agregar btn btn-sm btn-warning glyphicon glyphicon-plus pull-right" id="btnBuscar" ' + (((tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 2 || tipoUsuario == 3 || tipoUsuario == 7) && cboEstatusTaller.val() == "1") ? '' : 'style="display:none;"') + '></button>');

                    api.column(0, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(
                                '<tr class="group" style="background: rgb(68,133,51) !important; background: linear-gradient(40deg, #45cafc, #303f9f) !important;color:white;text-align:center;"><td colspan="14" style="">' +
                                '<b style="font-size: 18px;">' + meses[group - 1] + '</b>' + (group == n + 1 ? ('<button type="button" data-index="' + idCalendario + '" data-mes="' + group +
                                '" class="btn-agregar btn btn-sm btn-warning glyphicon glyphicon-plus pull-right" id="btnBuscar" ' + (((tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 2 || tipoUsuario == 3 || tipoUsuario == 7) && cboEstatusTaller.val() == "1") ? '' : 'style="display:none;"') + '></button>') : '') + '</td></tr>'
                            );
                            last = group;
                        }
                    });
                    $(".btn-agregar").click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 2 || tipoUsuario == 3 || tipoUsuario == 7) {
                            cboEconomicoFalla.prop("disabled", false);
                            cboEconomicoFalla.val("");
                            event.stopImmediatePropagation();
                            OpenModalFalla();
                            btnModalOverhaulFalla.attr("data-mes", $(this).attr("data-mes"));
                            btnModalOverhaulFalla.attr("data-tipo", 0);
                        }
                    });
                    gridEstatusTaller.on('click', '.btn-agregar', function (e) {
                        if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 2 || tipoUsuario == 3 || tipoUsuario == 7) {
                            cboEconomicoFalla.prop("disabled", false);
                            cboEconomicoFalla.val("");
                            e.preventDefault();
                            e.stopPropagation();
                            e.stopImmediatePropagation();
                            OpenModalFalla();
                            btnModalOverhaulFalla.attr("data-mes", $(this).attr("data-mes"));
                            btnModalOverhaulFalla.attr("data-tipo", 0);
                        }
                    });
                    gridEstatusTaller.on('click', '.btn-agregar-componente', function (e) {
                        if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 2 || tipoUsuario == 3 || tipoUsuario == 7) {
                            cboEconomicoFalla.prop("disabled", true);
                            cboEconomicoFalla.val($(this).attr("data-maquinaID"));
                            cboEconomicoFalla.click();
                            e.preventDefault();
                            e.stopPropagation();
                            e.stopImmediatePropagation();
                            OpenModalFalla();
                            btnModalOverhaulFalla.attr("data-mes", $(this).attr("data-mes"));
                            btnModalOverhaulFalla.attr("data-planeacionID", $(this).attr("data-planeacionID"));
                            btnModalOverhaulFalla.attr("data-tipo", 1);
                        }
                    });
                    gridEstatusTaller.on('click', '.btn-dg', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        btnAgregarAct.attr("data-modeloID", $(this).attr("data-modeloID"));
                        btnAgregarAct.attr("data-eventoID", $(this).attr("data-index"));
                        if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 3 || tipoUsuario == 7) {
                            btnModalGuardarNuevo.css("display", "block");
                        }
                        else {
                            btnModalGuardarNuevo.css("display", "none");
                        }
                        btnModalIniciarNuevo.css("display", "none");
                        btnModalGuardarNuevo.html("<span class='glyphicon glyphicon-floppy-disk'></span> Guardar");
                        titleModalNuevo.text("Crear Diagrama de Gantt para " + $(this).attr("data-economico") + " (" + $(this).attr("data-fecha") + ")");
                        if ($(this).attr("data-estatus") == 0 && $(this).attr("data-tipo") == 0) { btnModalGuardarNuevo.prop("disabled", true); }
                        else { btnModalGuardarNuevo.prop("disabled", false); }
                        btnModalGuardarNuevo.attr("data-index", $(this).attr("data-index"));
                        btnModalGuardarNuevo.attr("data-estatus", $(this).attr("data-index"));
                        CargarGridGantt($(this).attr("data-modeloID"), $(this).attr("data-index"));
                        $('#lstBox1').fillCombo("/Overhaul/FillCboDiagramaGantt", { idModelo: $(this).attr("data-modeloID"), idEvento: $(this).attr("data-index") }, true);
                        OpenModalNuevo();
                    });
                    gridEstatusTaller.on('click', '.btn-inicio', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        btnAgregarAct.attr("data-modeloID", $(this).attr("data-modeloID"));
                        btnAgregarAct.attr("data-eventoID", $(this).attr("data-index"));
                        btnModalGuardarNuevo.attr("data-index", $(this).attr("data-index"));
                        btnModalGuardarNuevo.attr("data-estatus", $(this).attr("data-estatus"));
                        btnModalGuardarNuevo.attr("data-tipo", $(this).attr("data-tipo"));
                        if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 3 || tipoUsuario == 7) {
                            btnModalGuardarNuevo.css("display", "block");
                            btnModalIniciarNuevo.css("display", "block");
                        }
                        else {
                            btnModalGuardarNuevo.css("display", "none");
                            btnModalIniciarNuevo.css("display", "none");
                        }
                        btnModalGuardarNuevo.html("<span class='glyphicon glyphicon-edit'></span> Modificar");
                        if ($(this).attr("data-estatus") == 1) {
                            CargarGridGantt($(this).attr("data-modeloID"), $(this).attr("data-index"));
                            $('#lstBox1').fillCombo("/Overhaul/FillCboDiagramaGantt", { idModelo: $(this).attr("data-modeloID"), idEvento: $(this).attr("data-index") }, true);
                            OpenModalNuevo();
                        }
                        else { ConfirmacionEliminacion("Iniciar Overhaul", "¿Desea iniciar Overhaul para " + $(this).attr("data-economico") + "? "); }                        
                    });

                    gridEstatusTaller.on('click', '.btn-terminar', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 3 || tipoUsuario == 7) {
                            //btnModalPortada.css("display", "block");
                            //btnModalEvidencia.css("display", "block");
                            btnTerminarOverhaul.css("display", "block");
                            btnModalGuardarComentario.css("display", "block");
                            tbComentario.prop("disabled", false);
                        }
                        else
                        {
                            //btnModalPortada.css("display", "none");
                            //btnModalEvidencia.css("display", "none");
                            btnTerminarOverhaul.css("display", "none");
                            btnModalGuardarComentario.css("display", "none");
                            tbComentario.prop("disabled", true);
                        }

                        
                        btnModalGuardarNuevo.attr("data-estatus", $(this).attr("data-estatus"));
                        btnTerminarOverhaul.attr("data-index", $(this).attr("data-index"));
                        btnTerminarOverhaul.attr("data-economico", $(this).attr("data-economico"));
                        btnTerminarOverhaul.attr("data-tipo", $(this).attr("data-tipo"));
                        btnTerminarOverhaul.attr("data-estatus", $(this).attr("data-estatus"));
                        if ($(this).attr("data-tipo") == 0) {

                            CargarGridActividades($(this).attr("data-index"), $(this).attr("data-estatus"));
                            OpenModalActividades($(this).attr("data-estatus"));
                        }
                        else { $("#modalFechaFin").modal("show");
                                
                                // ConfirmacionEliminacion("Iniciar Overhaul", "¿Desea terminar Overhaul para " + $(this).attr("data-economico") + "? ");
                    }

                    });

                    gridEstatusTaller.on('click', '.btn-dgt', function (e) { 
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 3 || tipoUsuario == 7) {
                            //btnModalPortada.css("display", "block");
                            //btnModalEvidencia.css("display", "block");
                            btnTerminarOverhaul.css("display", "block");
                            btnModalGuardarComentario.css("display", "block");
                            tbComentario.prop("disabled", false);
                        }
                        else {
                            //btnModalPortada.css("display", "none");
                            //btnModalEvidencia.css("display", "none");
                            btnTerminarOverhaul.css("display", "none");
                            btnModalGuardarComentario.css("display", "none");
                            tbComentario.prop("disabled", true);
                        }
                        btnTerminarOverhaul.attr("data-index", $(this).attr("data-index"));
                        btnTerminarOverhaul.attr("data-economico", $(this).attr("data-economico"));
                        btnTerminarOverhaul.attr("data-tipo", $(this).attr("data-tipo"));
                        btnTerminarOverhaul.attr("data-estatus", $(this).attr("data-estatus"));
                        if ($(this).attr("data-tipo") == 0) {

                            CargarGridActividades($(this).attr("data-index"), $(this).attr("data-estatus"));
                            OpenModalActividades($(this).attr("data-estatus"));
                        }
                        var estatus = parseInt($(this).attr("data-estatus"));
                        btnTerminarOverhaul.text("Cerrar DG");
                        if (estatus > 3) { btnTerminarOverhaul.css("display", "none"); }
                        else { btnTerminarOverhaul.css("display", "block"); }
                        
                    });
                    gridEstatusTaller.on('click', '.btn-rep', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        btnModalGuardarComentario.css("display", "none");
                        tbComentario.prop("disabled", true);
                        if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 3 || tipoUsuario == 7) {
                            btnModalPortada.css("display", "block");
                            btnModalEvidencia.css("display", "block");
                            btnTerminarOverhaul.css("display", "block");                            
                        }
                        else {
                            btnTerminarOverhaul.css("display", "none");
                        }
                        btnTerminarOverhaul.attr("data-index", $(this).attr("data-index"));
                        btnTerminarOverhaul.attr("data-economico", $(this).attr("data-economico"));
                        btnTerminarOverhaul.attr("data-tipo", $(this).attr("data-tipo"));
                        btnTerminarOverhaul.attr("data-estatus", $(this).attr("data-estatus"));
                        if ($(this).attr("data-tipo") == 0) {

                            CargarGridActividades($(this).attr("data-index"), $(this).attr("data-estatus"));
                            OpenModalActividades($(this).attr("data-estatus"));
                        }
                        var estatus = parseInt($(this).attr("data-estatus"));
                        btnTerminarOverhaul.text("Cerrar RE");
                        if (estatus > 4) { btnTerminarOverhaul.css("display", "none"); }
                        else { btnTerminarOverhaul.css("display", "block"); }

                    });
                    
                    gridEstatusTaller.on('click', '.btn-rdg', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        AbrirReporteDG($(this).attr("data-index"));
                    });
                    gridEstatusTaller.on('click', '.btn-re', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        AbrirReporteEjecutivo($(this).attr("data-index"));
                    });
                    gridEstatusTaller.on('click', '.btn-fl', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        btncargarArchivoFL.parent().parent().css("display", "block");
                        btnEnviarLiberacion.parent().css("display", "block");
                        gridArchivosFL.column(3).visible(true);

                        CargarGridArchivosFL(-1, $(this).attr("data-index"));
                        btncargarArchivoFL.attr("data-index", $(this).attr("data-index"));
                        btnEnviarLiberacion.attr("data-index", $(this).attr("data-index"));
                        btnEnviarLiberacion.attr("data-tipo", $(this).attr("data-tipo"));
                        OpenModalArchivosFL();
                    });
                    gridEstatusTaller.on('click', '.btn-fl-fin', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        btncargarArchivoFL.parent().parent().css("display", "none");
                        btnEnviarLiberacion.parent().css("display", "none");

                        gridArchivosFL.column(3).visible(false);

                        CargarGridArchivosFL(-1, $(this).attr("data-index"));
                        btncargarArchivoFL.attr("data-index", $(this).attr("data-index"));
                        btnEnviarLiberacion.attr("data-index", $(this).attr("data-index"));
                        btnEnviarLiberacion.attr("data-tipo", $(this).attr("data-tipo"));
                        OpenModalArchivosFL();
                    });
                },
                columns: [
                    { data: 'mes', title: 'MES' },

                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            return row.economico;
                        },
                        title: "Económico"
                    },
                    {
                        "orderable": false,
                        "data": null,
                        "defaultContent": '',
                        title: " ",
                        "render": function (data, type, row, meta) {
                            return '<button type="button" data-index="' + idCalendario + '" data-mes="' + row.mes + '" data-maquinaID="' + row.maquinaID + '" data-planeacionID="' + row.id + '" class="btn-agregar-componente btn btn-sm btn-warning glyphicon glyphicon-plus" id="btnBuscar" ' + (((tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 2 || tipoUsuario == 3 || tipoUsuario == 7) && cboEstatusTaller.val() == "1") ? '' : 'style="display:none;"') + '></button>';
                        },
                    },
                    { data: 'componente', title: 'Componente' },
                    { data: 'subconjunto', title: 'Subconjunto' },
                    {
                        data: 'horasComponente',
                        //.toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")
                        render: function (data, type, row, meta) {
                            return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                        },
                        title: 'Horas<br>Componente'
                    },
                    {
                        data: 'target',
                        render: function (data, type, row, meta) {
                            return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                        },
                        title: 'Target'
                    },
                    {
                        data: 'falla',
                        render: function (data, type, row, meta) {
                            return data ? '<span class="glyphicon glyphicon-remove" style="color: rgb(255, 25, 25)"></span>' : '<span class="glyphicon glyphicon-ok" style="color: rgb(92, 184, 92)"></span>';
                        },
                        title: 'Programado'
                    },
                    //{
                    //    sortable: false,
                    //    "render": function (data, type, row, meta) {
                    //        var html = "";
                    //        switch (row.tipoOverhaul)
                    //        {
                    //            case 0:
                    //                html += "OVERHAUL GENERAL";
                    //                break;
                    //            case 1:
                    //                html += "CAMBIO DE MOTOR";
                    //                break;
                    //            case 2:
                    //                html += "COMPONENTE DESFASADO";
                    //                break;
                    //            default:
                    //                html += "FALLO"
                    //                break;
                    //        }
                    //        return html;
                    //    },
                    //    title: "Descripción"
                    //},

                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';
                            if (row.tipoOverhaul == 0) {
                                if (row.estatusOverhaul == 0) { html += '<button class="btn-dg btn btn-sm btn-warning glyphicon glyphicon-arrow-right" type="button" data-index="' + row.id + '" data-economico="' + row.economico + '" data-fecha="' + row.fecha + '" data-modeloID="' + row.modeloID + '" data-estatus="' + row.estatusOverhaul + '" data-tipo="' + row.tipoOverhaul + '" style=""></button>'; }
                                else { html += row.diasProgramados; }
                            }
                            return html;
                        },
                        title: "Días<br>DG"
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';
                            if (row.estatusOverhaul in [0, 1]) {
                                if (row.estatusOverhaul == 0 && row.tipoOverhaul == 0) {  }
                                else {
                                    if (row.estatusOverhaul < 2) {
                                        html += '<button class="btn-inicio btn btn-sm btn-warning glyphicon glyphicon-arrow-right" type="button" data-index="' +
                                        row.id + '" data-economico="' + row.economico + '" data-fecha="' + row.fecha + '" data-modeloID="' + row.modeloID + '" data-estatus="' + row.estatusOverhaul + '" data-tipo="' + row.tipoOverhaul + '" style=""></button>';
                                    }
                                }
                            }
                            else { html += row.fechaInicio; }
                            return html;
                        },
                        title: "Fecha<br>Inicio"
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            if (row.fechaInicio != "") {                                
                                var startDay = new Date(row.fechaInicioFix);                                
                                var endDay = row.estatusOverhaul < 3 ? new Date() : new Date(row.fechaFinFix);
                                var millisecondsPerDay = 1000 * 60 * 60 * 24;
                                var millisBetween = endDay.getTime() - startDay.getTime();
                                var days = millisBetween / millisecondsPerDay;
                                return row.estatusOverhaul < 3 ? Math.floor(days) : Math.ceil(days);
                            }
                            return "";
                        },
                        title: "Días<br>Real"
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = "";
                            if (row.estatusOverhaul == 2) {
                                var icono = "";
                                //if (row.tipoOverhaul == 0) { icono = "glyphicon glyphicon-tasks"; }
                                //else { icono = "glyphicon glyphicon-stop" }
                                icono = "glyphicon glyphicon-arrow-right";
                                html += '<button class="btn-terminar btn btn-sm btn-warning ' + icono + '" type="button" data-index="' +
                                row.id + '" data-economico="' + row.economico + '" data-fecha="' + row.fecha + '" data-modeloID="' + row.modeloID + '" data-estatus="' +
                                row.estatusOverhaul + '" data-tipo="' + row.tipoOverhaul + '" style=""></button>';
                            }
                            else
                            {
                                if (row.estatusOverhaul > 2)
                                {
                                    html = row.fechaFin;
                                }
                            }
                            return html;
                        },
                        title: 'Fecha<br>Fin'
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = "";
                            if (row.tipoOverhaul == 0 && row.estatusOverhaul >= 3) {
                                if (row.estatusOverhaul == 3) {
                                    html += '<button class="btn-dgt btn btn-sm btn-warning glyphicon glyphicon-arrow-right" type="button" data-index="' +
                                    row.id + '" data-economico="' + row.economico + '" data-fecha="' + row.fecha + '" data-modeloID="' + row.modeloID + '" data-estatus="' +
                                    row.estatusOverhaul + '" data-tipo="' + row.tipoOverhaul + '" style=""></button>';
                                }
                                else {
                                    html += '<button class="btn-rdg btn btn-sm btn-warning glyphicon glyphicon-file" type="button" data-index="' +
                                    row.id + '" data-economico="' + row.economico + '" data-fecha="' + row.fecha + '" data-modeloID="' + row.modeloID + '" data-estatus="' +
                                    row.estatusOverhaul + '" data-tipo="' + row.tipoOverhaul + '" style=""></button>';
                                }
                            }
                            return html;
                        },
                        title: 'DG'
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = "";
                            if (row.tipoOverhaul == 0) {
                                if (row.estatusOverhaul > 3)
                                {
                                    if (row.estatusOverhaul == 4) {
                                        html += '<button class="btn-rep btn btn-sm btn-warning glyphicon glyphicon-arrow-right" type="button" data-index="' +
                                        row.id + '" data-economico="' + row.economico + '" data-fecha="' + row.fecha + '" data-modeloID="' + row.modeloID + '" data-estatus="' +
                                        row.estatusOverhaul + '" data-tipo="' + row.tipoOverhaul + '" style=""></button>&nbsp;&nbsp;&nbsp;';
                                    }
                                    html += '<button class="btn-re btn btn-sm btn-warning glyphicon glyphicon-file" type="button" data-index="' +
                                    row.id + '" data-economico="' + row.economico + '" data-fecha="' + row.fecha + '" data-modeloID="' + row.modeloID + '" data-estatus="' +
                                    row.estatusOverhaul + '" data-tipo="' + row.tipoOverhaul + '" style=""></button>';
                                }
                            }
                            return html;
                        },
                        title: 'Rep<br>Ejec'
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = "";
                            if (row.tipoOverhaul == 0) {
                                if (row.estatusOverhaul == 5) {
                                    html += '<button class="btn-fl btn btn-sm btn-warning glyphicon glyphicon-arrow-right" type="button" data-index="' +
                                    row.id + '" data-economico="' + row.economico + '" data-fecha="' + row.fecha + '" data-modeloID="' + row.modeloID + '" data-estatus="' +
                                    row.estatusOverhaul + '" data-tipo="' + row.tipoOverhaul + '" style=""></button>';
                                }
                            }
                            else
                            {
                                if (row.estatusOverhaul > 2 && row.estatusOverhaul < 6) {
                                    html += '<button class="btn-fl btn btn-sm btn-warning glyphicon glyphicon-arrow-right" type="button" data-index="' +
                                    row.id + '" data-economico="' + row.economico + '" data-fecha="' + row.fecha + '" data-modeloID="' + row.modeloID + '" data-estatus="' +
                                    row.estatusOverhaul + '" data-tipo="' + row.tipoOverhaul + '" style=""></button>';
                                }
                            }
                            if (row.estatusOverhaul == 6) {
                                html += '<button class="btn-fl-fin btn btn-sm btn-warning glyphicon glyphicon-file" type="button" data-index="' +
                                row.id + '" data-economico="' + row.economico + '" data-fecha="' + row.fecha + '" data-modeloID="' + row.modeloID + '" data-estatus="' +
                                row.estatusOverhaul + '" data-tipo="' + row.tipoOverhaul + '" style=""></button>';
                            }
                            return html;
                        },
                        title: 'Form<br>Lib'
                    },
                    { data: 'tipoOverhaul', title: 'Tipo', visible: false },

                ],
                "columnDefs": [
                    { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
                    { "visible": false, "targets": 0 },
                    { "orderable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15] }
                ],
                "order": [0, 'asc'],
            });
        }
      
        function CargarGridEstatus()
        {         
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/CargarGridTallerEstatus',
                datatype: "json",
                type: "POST",
                data: {
                    idCalendario: cboCalendarioEstatus.val(),
                    estatus: cboEstatusTaller.val(), 
                    tipo: cboTipoParo.val()
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        cboEconomicoFalla.fillCombo("/Overhaul/FillCboEconomicosByObraID", { obra: response.obra });
                        idObra = response.obra;
                        anio = response.anio;
                        gridEstatusTaller.clear();
                        gridEstatusTaller.rows.add(response.data);
                        gridEstatusTaller.draw();
                        $("#lgTablaTaller").html("<b>Calendario:</b> " + $("#cboCalendarioEstatus option:selected").text() + " / <b>Tipo:</b> "
                            + $("#cboTipoParo option:selected").text() + " / <b>Estatus:</b> " + $("#cboEstatusTaller option:selected").text());
                        MergeGridCells();
                    }                    
                }
            });
            idCalendario = cboCalendarioEstatus.val();
            gridEstatusTaller.columns.adjust();
        }

        function MergeGridCells() {
            var dimension_cells = new Array();
            var dimension_col = 13;
            var columnCount = $("#gridEstatusTaller tr:first th").length;
            // first_instance holds the first instance of identical td
            var first_instance = null;
            var first_instance_tipo = null;
            var first_instance_tipo2 = null;
            var first_instance_tipo3 = null;
            var first_instance_tipo4 = null;
            var first_instance_tipo5 = null;
            var first_instance_tipo6 = null;
            var first_instance_tipo7 = null;
            var first_instance_tipo8 = null;
            var first_instance_tipo9 = null;
            var first_instance_tipo10 = null;
            var rowspan = 1;
            var rowspan_tipo = 1;
            var rowspan_tipo2 = 1;
            var rowspan_tipo3 = 1;
            var rowspan_tipo4 = 1;
            var rowspan_tipo5 = 1;
            var rowspan_tipo6 = 1;
            var rowspan_tipo7 = 1;
            var rowspan_tipo8 = 1;
            var rowspan_tipo9 = 1;
            var rowspan_tipo10 = 1;
            // iterate through rows
            $("#gridEstatusTaller").find('tr').each(function () {
                // find the td of the correct column (determined by the dimension_col set above)
                var dimension_td = $(this).find('td:nth-child(1)');
                var dimension_td_tipo = $(this).find('td:nth-child(2)');
                var dimension_td_tipo2 = $(this).find('td:nth-child(7)');
                var dimension_td_tipo3 = $(this).find('td:nth-child(8)');
                var dimension_td_tipo4 = $(this).find('td:nth-child(9)');
                var dimension_td_tipo5 = $(this).find('td:nth-child(10)');
                var dimension_td_tipo6 = $(this).find('td:nth-child(11)');
                var dimension_td_tipo7 = $(this).find('td:nth-child(12)');
                var dimension_td_tipo8 = $(this).find('td:nth-child(13)');
                var dimension_td_tipo9 = $(this).find('td:nth-child(14)');
                var dimension_td_tipo10 = $(this).find('td:nth-child(1)').css("background-color");
                if (first_instance == null) {
                    // must be the first row
                    first_instance = dimension_td;
                    first_instance_tipo = dimension_td_tipo;
                    first_instance_tipo2 = dimension_td_tipo2;
                    first_instance_tipo3 = dimension_td_tipo3;
                    first_instance_tipo4 = dimension_td_tipo4;
                    first_instance_tipo5 = dimension_td_tipo5;
                    first_instance_tipo6 = dimension_td_tipo6;
                    first_instance_tipo7 = dimension_td_tipo7;
                    first_instance_tipo8 = dimension_td_tipo8;
                    first_instance_tipo9 = dimension_td_tipo9;
                    first_instance_tipo10 = dimension_td.css("background-color");
                }
                else {
                    if (dimension_td.text() == first_instance.text() && dimension_td_tipo10 == first_instance_tipo10) {
                        // the current td is identical to the previous
                        // remove the current td
                        dimension_td.remove();
                        ++rowspan;
                        // increment the rowspan attribute of the first instance
                        first_instance.attr('rowspan', rowspan);
                        if (dimension_td_tipo.text() == first_instance_tipo.text()) {
                            // the current td is identical to the previous
                            // remove the current td
                            dimension_td_tipo.remove();
                            ++rowspan_tipo;
                            // increment the rowspan attribute of the first instance
                            first_instance_tipo.attr('rowspan', rowspan_tipo);
                        }
                        else {
                            // this cell is different from the last
                            first_instance_tipo = dimension_td_tipo;
                            rowspan_tipo = 1;
                        }
                        //if (dimension_td_tipo2.text() == first_instance_tipo2.text()) {
                        //    // the current td is identical to the previous
                        //    // remove the current td
                        //    dimension_td_tipo2.remove();
                        //    ++rowspan_tipo2;
                        //    // increment the rowspan attribute of the first instance
                        //    first_instance_tipo2.attr('rowspan', rowspan_tipo2);
                        //}
                        //else {
                        //    // this cell is different from the last
                        //    first_instance_tipo2 = dimension_td_tipo2;
                        //    rowspan_tipo2 = 1;
                        //}
                        if (dimension_td_tipo3.text() == first_instance_tipo3.text()) {
                            // the current td is identical to the previous
                            // remove the current td
                            dimension_td_tipo3.remove();
                            ++rowspan_tipo3;
                            // increment the rowspan attribute of the first instance
                            first_instance_tipo3.attr('rowspan', rowspan_tipo3);
                        }
                        else {
                            // this cell is different from the last
                            first_instance_tipo3 = dimension_td_tipo3;
                            rowspan_tipo3 = 1;
                        }
                        if (dimension_td_tipo4.text() == first_instance_tipo4.text()) {
                            // the current td is identical to the previous
                            // remove the current td
                            dimension_td_tipo4.remove();
                            ++rowspan_tipo4;
                            // increment the rowspan attribute of the first instance
                            first_instance_tipo4.attr('rowspan', rowspan_tipo4);
                        }
                        else {
                            // this cell is different from the last
                            first_instance_tipo4 = dimension_td_tipo4;
                            rowspan_tipo4 = 1;
                        }
                        if (dimension_td_tipo5.text() == first_instance_tipo5.text()) {
                            // the current td is identical to the previous
                            // remove the current td
                            dimension_td_tipo5.remove();
                            ++rowspan_tipo5;
                            // increment the rowspan attribute of the first instance
                            first_instance_tipo5.attr('rowspan', rowspan_tipo5);
                        }
                        else {
                            // this cell is different from the last
                            first_instance_tipo5 = dimension_td_tipo5;
                            rowspan_tipo5 = 1;
                        }
                        if (dimension_td_tipo6.text() == first_instance_tipo6.text()) {
                            // the current td is identical to the previous
                            // remove the current td
                            dimension_td_tipo6.remove();
                            ++rowspan_tipo6;
                            // increment the rowspan attribute of the first instance
                            first_instance_tipo6.attr('rowspan', rowspan_tipo6);
                        }
                        else {
                            // this cell is different from the last
                            first_instance_tipo6 = dimension_td_tipo6;
                            rowspan_tipo6 = 1;
                        }
                        if (dimension_td_tipo7.text() == first_instance_tipo7.text()) {
                            // the current td is identical to the previous
                            // remove the current td
                            dimension_td_tipo7.remove();
                            ++rowspan_tipo7;
                            // increment the rowspan attribute of the first instance
                            first_instance_tipo7.attr('rowspan', rowspan_tipo7);
                        }
                        else {
                            // this cell is different from the last
                            first_instance_tipo7 = dimension_td_tipo7;
                            rowspan_tipo7 = 1;
                        }
                        if (dimension_td_tipo8.text() == first_instance_tipo8.text()) {
                            // the current td is identical to the previous
                            // remove the current td
                            dimension_td_tipo8.remove();
                            ++rowspan_tipo8;
                            // increment the rowspan attribute of the first instance
                            first_instance_tipo8.attr('rowspan', rowspan_tipo8);
                        }
                        else {
                            // this cell is different from the last
                            first_instance_tipo8 = dimension_td_tipo8;
                            rowspan_tipo8 = 1;
                        }
                        if (dimension_td_tipo9.text() == first_instance_tipo9.text()) {
                            // the current td is identical to the previous
                            // remove the current td
                            dimension_td_tipo9.remove();
                            ++rowspan_tipo9;
                            // increment the rowspan attribute of the first instance
                            first_instance_tipo9.attr('rowspan', rowspan_tipo9);
                        }
                        else {
                            // this cell is different from the last
                            first_instance_tipo9 = dimension_td_tipo9;
                            rowspan_tipo9 = 1;
                        }
                        if (dimension_td_tipo10 == first_instance_tipo10) {
                            ++rowspan_tipo10;
                        }
                        else {
                            // this cell is different from the last
                            first_instance_tipo10 = dimension_td_tipo10;
                            rowspan_tipo10 = 1;
                        }
                    }
                    else {
                        $(this).find('td').css("border-top-width", "thick");
                        // this cell is different from the last
                        first_instance = dimension_td;
                        rowspan = 1;
                        first_instance_tipo = dimension_td_tipo;
                        rowspan_tipo = 1;
                        first_instance_tipo2 = dimension_td_tipo2;
                        rowspan_tipo2 = 1;
                        first_instance_tipo3 = dimension_td_tipo3;
                        rowspan_tipo3 = 1;
                        first_instance_tipo4 = dimension_td_tipo4;
                        rowspan_tipo4 = 1;
                        first_instance_tipo5 = dimension_td_tipo5;
                        rowspan_tipo5 = 1;
                        first_instance_tipo6 = dimension_td_tipo6;
                        rowspan_tipo6 = 1;
                        first_instance_tipo7 = dimension_td_tipo7;
                        rowspan_tipo7 = 1;
                        first_instance_tipo8 = dimension_td_tipo8;
                        rowspan_tipo8 = 1;
                        first_instance_tipo9 = dimension_td_tipo9;
                        rowspan_tipo9 = 1;
                        first_instance_tipo10 = dimension_td_tipo10;
                        rowspan_tipo10 = 1;
                    }
                }
            });
        }

        function format(d) {
            let html = '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;" class="table-condensed table-hover table-striped text-center bootgrid-table">';
            html += '<thead class="bg-table-header">';
            html += '<tr><th><b>Serie Componente</b></th><th><b>Descripción</b></th><th><b>Target</b></th><th><b>Horas Ciclo</b></th></tr>'
            html += '</thead>';
            html += '<tbody>';
            for(let i = 0 ; i < d.componentes.length; i++){
                html += '<tr>' +
                    '<td>' + d.componentesID[i] + '</td>' +
                    '<td>' + d.componentes[i].toUpperCase() + '</td>' +
                    '<td>' + d.componentestarget[i] + '</td>' +
                    '<td>' + d.componentesHoras[i] + '</td>' +
                '</tr>';
            }
            html += '</tbody>';
            html += '</table>';
            return html
        }


        function CargarComentario(ActividadID, EventoID, tipo, numDia) {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarComentarioActividadOverhaul",
                type: 'POST',
                async: false,
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ actividadID: ActividadID, eventoID: EventoID, tipo: tipo, numDia: numDia }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        if (tipo == 0) {
                            if (response.comentario != null) tbComentario.val(response.comentario);
                            else { tbComentario.val(""); }
                            if (response.fecha != null) $("#txtfechaComentario").val("Última modificacion: " + response.fecha);
                            else $("#txtfechaComentario").val("");
                        }
                        else
                        {                            
                            if (tipo == 5) {
                                if (response.comentario != null) tbComentarioPortada.content.set(response.comentario);
                                else tbComentarioPortada.content.set("");
                                if (response.fecha != null) $("#txtfechaComentarioPortada").val("Última modificacion: " + response.fecha);
                                else $("#txtfechaComentarioPortada").val("");
                            }
                            else {
                                if (response.comentario != null) tbComentarioReporte.content.set(response.comentario);
                                else tbComentarioReporte.content.set("");
                                if (response.fecha != null) $("#txtfechaComentarioReporte").val("Última modificacion: " + response.fecha);
                                else $("#txtfechaComentarioReporte").val("");
                            }
                        }
                    }
                    else {
                        AlertaGeneral("Alerta", "Se produjo un error al cargar el comentario");
                        if (tipo == 0) {
                            tbComentario.text("");
                            $("#txtfechaComentario").text("");
                        }
                        else {
                            tbComentarioReporte.content.set("");
                            $("#txtfechaComentarioReporte").text("");
                        }
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function OpenModalNuevo()
        {
            diaActualDG = 1;
            txtHorasTrabajadas.val('');
            CargarTblDiagrama();
            modalNuevoOverhaul.modal("show");
        }

        function CargarGridGantt(modeloID, index)
        {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/CargarDatosDiagramaGantt',
                datatype: "json",
                type: "POST",
                data: {
                    idModelo: modeloID,
                    idEvento: index
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {                        
                        txtHorasTrabajadas.val(response.data.horasTrabajadas);
                        diaActualDG = response.data.diaActual;
                        restaHoras = response.data.horasTrabajadas - response.data.restaHoras;
                        if (response.data.diasTrabajados != null) {
                            if (response.data.diasTrabajados[0] > 0) { $("#ckhorasLunes").prop("checked", true); }
                            else { $("#ckhorasLunes").prop("checked", false); }
                            if (response.data.diasTrabajados[1] > 0) { $("#ckhorasMartes").prop("checked", true); }
                            else { $("#ckhorasMartes").prop("checked", false); }
                            if (response.data.diasTrabajados[2] > 0) { $("#ckhorasMiercoles").prop("checked", true); }
                            else { $("#ckhorasMiercoles").prop("checked", false); }
                            if (response.data.diasTrabajados[3] > 0) { $("#ckhorasJueves").prop("checked", true); }
                            else { $("#ckhorasJueves").prop("checked", false); }
                            if (response.data.diasTrabajados[4] > 0) { $("#ckhorasViernes").prop("checked", true); }
                            else { $("#ckhorasViernes").prop("checked", false); }
                            if (response.data.diasTrabajados[5] > 0) { $("#ckhorasSabado").prop("checked", true); }
                            else { $("#ckhorasSabado").prop("checked", false); }
                            if (response.data.diasTrabajados[6] > 0) { $("#ckhorasDomingo").prop("checked", true); }
                            else { $("#ckhorasDomingo").prop("checked", false); }
                        }
                        tblDiagrama.clear();
                        tblDiagrama.rows.add(response.data.actividades);
                        tblDiagrama.draw();
                        let numRows = tblDiagrama.rows().data().length;
                        ActivarGuardado(numRows);
                    }
                }
            });
        }

        function InitGridGantt()
        {
            gridModalGantt = $("#gridModalGantt").DataTable({
                "language": {
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
                //retrieve: true,
                "searching": false,
                paging: false,
                scrollY: '35vh',
                scrollCollapse: true,
                select: {
                    style: 'os',
                    selector: 'td:first-child'
                },
                //ajax: {
                //    url: "/Overhaul/CargarDatosDiagramaGantt",
                //    data: function (data) {
                //        data.idModelo = modeloID
                //    }
                //},
                "drawCallback": function (settings) {
                    //gridModalGantt.on('click', '.btn-inicio', function () {
                    //    //ConfirmacionEliminacion("Iniciar Overhaul", "¿Desea iniciar Overhaul " + + "? " + $(this).attr("data-noComponente"));
                    //});
                    $("#ckSelectAll").change(function () {
                        $("input.checkbox-lista").prop('checked', $(this).prop("checked"));
                        $('input.number-horas-actividad').prop("disabled", !$(this).prop("checked"));
                    });

                    $("input.checkbox-actividad").change(function () {
                        var idDesactivar = $(this).attr("data-index");
                        var inputDesactivar = $('input.number-horas').filter(function () { return $(this).attr("data-index") == idDesactivar; });
                        if ($(this).prop("checked")) { $(inputDesactivar[0]).prop("disabled", false); }
                        else { $(inputDesactivar[0]).prop("disabled", true); }

                        var elementosVacios = $('input.number-horas').filter(function () {
                            return this.value == '' && $(this).prop("disabled") == false;
                        });
                        var elementosActivados = $('input.number-horas').filter(function () {
                            return $(this).prop("disabled") == false;
                        });
                        if (elementosVacios.length == 0 && elementosActivados.length > 0) { btnModalGuardarNuevo.attr("disabled", false); }
                        else { btnModalGuardarNuevo.attr("disabled", true); }
                    });

                    $("input.number-horas").keyup(function () {
                        var elementosVacios = $('input.number-horas').filter(function () {
                            return this.value == '' && $(this).prop("disabled") == false;
                        });
                        var elementosActivados = $('input.number-horas').filter(function () {
                            return $(this).prop("disabled") == false;
                        });
                        if (elementosVacios.length == 0 && elementosActivados.length > 0) { btnModalGuardarNuevo.attr("disabled", false); }
                        else { btnModalGuardarNuevo.attr("disabled", true); }
                    });
                },
                columns: [
                    { data: 'id', title: '<input id="ckSelectAll" class="checkbox-select-all checkbox-actividad" value="1" type="checkbox" checked>' },
                    { data: 'descripcion', title: 'ACTIVIDAD' },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            if (actividadesID != null) {
                                var actividad = actividadesID.filter(function (actividad) { return actividad.id == row.id; });
                                var html = "<input class='form-control number-horas number-horas-actividad' data-index='" + row.id + "' type='number' maxlength='10' " + (actividad.length > 0 ? "value='" + actividad[0].horasDuracion + "'" : "disabled") + ">";
                            }
                            else { var html = "<input  class='form-control number-horas number-horas-actividad' data-index='" + row.id + "' type='number' maxlength='10'>"; }
                            return html;
                        },
                        title: "HORAS"
                    },
                ],
                "columnDefs": [
                    { "className": "dt-center", "targets": [0, 1] },
                    { "orderable": false, "targets": [0, 1, 2] },
                    {
                        "targets": 0,
                        "data": null,
                        "defaultContent": '',
                        "orderable": false,
                        'render': function (data, type, row, meta) {
                            if (actividadesID != null) {
                                var actividad = actividadesID.filter(function (actividad) { return actividad.id == row.id; });
                                return '<input class="checkbox-actividad checkbox-lista" type="checkbox" data-index = "' + row.id + '" ' + (actividad.length > 0 ? 'checked' : "") + '>';
                            }
                            else { return '<input class="checkbox-actividad checkbox-lista" type="checkbox" data-index = "' + row.id + '" checked>'; }
                        }
                    }
                ],
                "order": [[0, 'asc']],                
            });
        }

        function guardarGantt() {
            if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 3 || tipoUsuario == 7) {
                var actividades = [];
                //var actividadesGuardar = $('input.number-horas-actividad').filter(function () {
                //    return $(this).prop("disabled") == false;
                //});
                var actividadesGuardar = tblDiagrama.rows().data();
                for (let i = 0; i < actividadesGuardar.length; i++) {
                    actividades.push({ id: i + 1, idAct: parseInt(actividadesGuardar[i].id), horasDuracion: parseFloat(actividadesGuardar[i].duracion), estatus: 0, numDia: parseInt(actividadesGuardar[i].dia) });
                }
                var horasTrabajadas = [];
                $(".checkbox-horas-trabajadas").each(function () {
                    if ($(this).prop("checked")) { horasTrabajadas.push(parseFloat(txtHorasTrabajadas.val())); }
                    else { horasTrabajadas.push(0); }
                });
                $.blockUI({ message: "Procesando..." });
                $.ajax({
                    url: "/Overhaul/GuardarDiagramaGantt",
                    type: 'POST',
                    dataType: 'json',
                    //contentType: 'application/json',
                    data: { idEvento: btnModalGuardarNuevo.attr("data-index"), actividades: actividades, horasTrabajadas: horasTrabajadas },
                        //JSON.stringify({ idEvento: btnModalGuardarNuevo.attr("data-index"), actividades: actividades, horasTrabajadas: horasTrabajadas }),
                    success: function (response) {
                        $.unblockUI();
                        if (response.success) {
                            if (response.exito) {
                                CargarGridEstatus();
                                modalNuevoOverhaul.modal("hide");
                                AlertaGeneral("Alerta", "Se ha guardado el diagrama con éxito");
                            }
                            else { AlertaGeneral("Alerta", "Ha ocurrido un error, no se ha guardado el diagrama"); }
                        }
                        else { AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados"); }
                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", response.message);
                    }
                });
            }
        }



        function IniciarOverhaul()
        {
            if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 3 || tipoUsuario == 7) {
                var fechaInicio = fechaInicioParo.val();
                var tipo = btnModalGuardarNuevo.attr("data-tipo");
                if (tipo == "0") guardarGantt();
                $.blockUI({ message: "Procesando..." });
                $.ajax({
                    url: "/Overhaul/IniciarOverhaulTaller",
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json',
                    data: JSON.stringify({ idEvento: btnModalGuardarNuevo.attr("data-index"), fechaInicio: fechaInicio, tipoOverhaul: btnModalGuardarNuevo.attr("data-tipo") }),
                    success: function (response) {
                        $.unblockUI();
                        if (response.success) {
                            if (response.exito) {
                                CargarGridEstatus();
                                modalNuevoOverhaul.modal("hide");
                                modalCorreos.modal("hide");
                                if (tipo == "0") {
                                    var idReporte = "154";
                                    var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&index=" + btnModalGuardarNuevo.attr("data-index") + "&tipo=0&inMemory=1";
                                    ireport.attr("src", path);
                                    document.getElementById('report').onload = function () {
                                        var fechaInicio = new Date();
                                        let correos = [];
                                        $('#tblCorreos tbody tr').each(function () {
                                            correos.push($(this).find('td:eq(0)').text());
                                        });
                                        EnviarCorreoLiberacion(response.economico, response.obra, correos, btnModalGuardarNuevo.attr("data-tipo"), btnModalGuardarNuevo.attr("data-index"));
                                    };
                                } else {
                                    let correos = [];
                                    $('#tblCorreos tbody tr').each(function () {
                                        correos.push($(this).find('td:eq(0)').text());
                                    });
                                    EnviarCorreoLiberacion(response.economico, response.obra, correos, btnModalGuardarNuevo.attr("data-tipo"), btnModalGuardarNuevo.attr("data-index"));
                                }
                                AlertaGeneral("Alerta", "Se ha iniciado Overhaul con éxito");                                
                            }
                            else { AlertaGeneral("Alerta", "Ha ocurrido un error, no se ha iniciado Overhaul"); }
                        }
                        else { AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados"); }
                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", response.message);
                    }
                });
            }
        }
        function EnviarCorreoLiberacion(economico, obra, correos, tipoOH, eventoID)
        {
            $.blockUI({ message: "Enviando correo electrónico" });
            $.ajax({
                url: "/Overhaul/EnviarCorreoInicioOH",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ economico: economico, obra: obra, correos: correos, tipo: tipoOH, eventoID: eventoID }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) { }
                    else { AlertaGeneral("Alerta", "No se puede enviar correo electrónico"); }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function InitGridActividades()
        {
            dtGridActividadesOverhaul = $("#gridActividadesOverhaul").DataTable({
                "language": {
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
                "searching": false,
                paging: false,
                scrollY: '50vh',
                scrollCollapse: true,
                select: {
                    style: 'os',
                    selector: 'td:first-child'
                },
                "drawCallback": function (settings) {
                    gridActividadesOverhaul.off("click").on('click', '.btn-iniciar-act', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 3 || tipoUsuario == 7) {
                            IniciarActividad($(this).attr("data-index"), $(this).attr("data-id"));
                        }
                    });
                    gridActividadesOverhaul.on('click', '.btn-fin-act', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 3 || tipoUsuario == 7) {
                            FinalizarActividad($(this).attr("data-index"), $(this).attr("data-id"));
                        }
                    });
                    gridActividadesOverhaul.on('click', '.btn-cdg', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        btnModalGuardarComentario.attr("data-index", $(this).attr("data-index"));
                        btnModalGuardarComentario.attr("data-numDia", $(this).attr("data-numDia"));
                        btnModalGuardarComentario.attr("data-tipo", 0);
                        CargarComentario($(this).attr("data-index"), btnTerminarOverhaul.attr("data-index"), 0, $(this).attr("data-numDia"));
                        OpenModalComentario($(this).attr("data-num-act"), 0);
                    });
                    //gridActividadesOverhaul.on('click', '.btn-cre', function () {
                    //    btnModalGuardarComentario.attr("data-index", $(this).attr("data-index"));
                    //    btnModalGuardarComentario.attr("data-tipo", 1);
                    //    CargarComentario($(this).attr("data-index"), btnModalTerminarOverhaul.attr("data-index"), 1);
                    //    OpenModalComentario($(this).attr("data-num-act"), 1);
                    //});
                    gridActividadesOverhaul.on('click', '.btn-are', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 3 || tipoUsuario == 7) {
                            btncargarArchivo.attr("data-index", $(this).attr("data-index"));
                            btncargarArchivo.attr("data-num-act", $(this).attr("data-num-act"));
                            btnModalGuardarComentarioReporte.attr("data-tipo", 1);
                            btnModalGuardarComentarioReporte.attr("data-numDia", $(this).attr("data-numDia"));
                            //tbComentarioReporte.val("");
                            CargarComentario($(this).attr("data-index"), btnTerminarOverhaul.attr("data-index"), 1, $(this).attr("data-numDia"));
                            CargarGridArchivos($(this).attr("data-index"), btnTerminarOverhaul.attr("data-index"), btnModalGuardarComentarioReporte.attr("data-numDia"));
                            OpenModalArchivos($(this).attr("data-num-act"), 1);
                        }
                    });
                },
                columns: [
                    { data: 'id', title: 'ID' },
                    { data: 'idAct', title: 'IDACT' },
                    { data: 'descripcion', title: 'ACTIVIDAD' },
                    { data: 'fechaInicioP', title: 'FECHA<br>INICIO<br>PROG.' },
                    { data: 'fechaFinP', title: 'FECHA<br>FIN<br>PROG.' },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = "";
                            if (row.estatus == 0) {
                                html += '<button class="btn-iniciar-act btn btn-sm btn-success glyphicon glyphicon-play" type="button" data-index="' + row.idAct + '" data-estatus="' +
                                    row.estatus + '" data-id="' + row.id + '" style="" ' + (row.estatus == 0 ? ((tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 3 || tipoUsuario == 7) ? "" : "disabled") : "disabled") + '></button>';
                            }
                            else { html += row.fechaInicio; }                            
                            return html;
                        }, 
                        title: 'FECHA<br>INICIO'
                    },
                    {
                        "render": function (data, type, row, meta) {
                            var html = "";
                            if (row.estatus < 2) {
                                html += '<button class="btn-fin-act btn btn-sm btn-success glyphicon glyphicon-stop" type="button" data-index="' + row.idAct + '" data-estatus="' +
                                    row.estatus + '" data-id="' + row.id + '" style="" ' + (row.estatus == 1 ? ((tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 3 || tipoUsuario == 7) ? "" : "disabled") : "disabled") + '></button>';
                            }
                            else { html += row.fechaFin; }
                            return html;
                        },
                        title: 'FECHA<br>FIN'
                    },
                    { 
                        "render": function (data, type, row, meta) {
                            var html = "";
                            html += '<button class="btn-cdg btn btn-sm btn-primary glyphicon glyphicon-comment" type="button" data-index="' + row.idAct + '" data-estatus="' +
                                row.estatus + '" data-num-act="' + meta.row + '"  data-numDia="' + row.numDia + '" style=""></button>';
                            return html;
                        },
                        title: 'COMENTARIO<br>DIAGRAMA<br>DE GANTT'
                    },
                    //{ 
                    //    "render": function (data, type, row, meta) {
                    //        var html = "";
                    //        html += '<button class="btn-cre btn btn-sm btn-primary glyphicon glyphicon-comment" type="button" data-index="' + row.idAct + '" data-estatus="' + row.estatus + '" data-num-act = "' + meta.row + '" style=""> ' + "" + '</button>';
                    //        return html;
                    //    }, 
                    //    title: 'CRE' 
                    //},
                    { 
                        "render": function (data, type, row, meta) {
                            var html = "";
                            html += '<button class="btn-are btn btn-sm btn-primary" type="button" data-index="' + row.idAct + '" data-estatus="' + row.estatus + '" data-numDia="' + row.numDia + '" data-num-act="' + 
                                meta.row + '" style="" ' + ((row.reporteEjecutivo && (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 3 || tipoUsuario == 7)) ? "" : "disabled") + '><span class=" glyphicon glyphicon-picture"></span></button>';
                            return html;
                        }, 
                        title: 'ARCHIVOS<br>REPORTE<br>EJECUTIVO'
                    }
                ],
                "columnDefs": [
                    { "className": "dt-center", "targets": [0, 1, 3, 4, 5, 6, 7, 8] },
                    { "visible": false, "targets": [0, 1] },
                    { "orderable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8] }

                ],
                "order": [[0, 'asc'], [3, 'asc']],
            });
        }
        function CargarGridActividades(index, estatus) {
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: '/Overhaul/CargarDatosActividadesTaller',
                datatype: "json",
                type: "POST",
                data: {
                    idEvento: index
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        var curr_pos = { 'top': $(dtGridActividadesOverhaul.settings()[0].nScrollBody).scrollTop(), 'left': $(dtGridActividadesOverhaul.settings()[0].nScrollBody).scrollLeft() };


                        dtGridActividadesOverhaul.clear();
                        dtGridActividadesOverhaul.rows.add(response.data);
                        dtGridActividadesOverhaul.draw();

                        $(dtGridActividadesOverhaul.settings()[0].nScrollBody).scrollTop(curr_pos.top);
                        $(dtGridActividadesOverhaul.settings()[0].nScrollBody).scrollLeft(curr_pos.left);
                        if (estatus < 3)
                        {
                            if (response.terminado)
                            {
                                btnTerminarOverhaul.prop("disabled", false);
                            }
                            else { btnTerminarOverhaul.prop("disabled", true); }
                        }
                        else { btnTerminarOverhaul.prop("disabled", false); }
                    }
                }
            });
        }

        function OpenModalActividades(estatus)
        {
            if (estatus < 4)
                btnTerminarOverhaul.prop("disabled", true);
            else
                btnTerminarOverhaul.prop("disabled", false);
            modalActividadesOverhaul.modal("show");
        }
        function OpenModalComentario(numAct, tipo) {
            $("#title-modal-comentario").text("Comentario " + (tipo == 0 ? "DG" : "RE") + " / Actividad " + (parseInt(numAct) + 1) + " / " + btnTerminarOverhaul.attr("data-economico"));
            modalGuardarComentario.modal("show");
        }
        function OpenModalArchivos(numAct, tipo) {
            $("#title-modal-archivo").text("Archivos Actividad " + (parseInt(numAct) + 1) + " / " + btnTerminarOverhaul.attr("data-economico"));            
            modalGuardarArchivo.modal("show");
        }
        function OpenModalFalla()
        {
            $("#title-modal-falla").text("Alta de Overhaul por falla");
            gridOHFalla.clear();
            gridOHFalla.draw();
            cboTipoFalla.val("3");
            modalOverhaulFalla.modal("show");
        } 
        function OpenModalArchivosFL() {
            $("#title-modal-fl").text("Archivos Formato Liberación ");
            modalGuardarArchivoFL.modal("show");
        }
        function OpenModalPortada() {
            if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 3 || tipoUsuario == 7) {
                tbComentarioPortada.content.set("");
                CargarComentario(-1, btnTerminarOverhaul.attr("data-index"), 5, -1);
                btncargarPortada.attr("data-index", btnTerminarOverhaul.attr("data-index"));
                CargarTblPortada(btnTerminarOverhaul.attr("data-index"));
                $("#title-modal-portada").text("Archivos Portada");
                modalGuardarPortada.modal("show");
            }
        }

        function OpenModalEvidencia()
        {
            if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 3 || tipoUsuario == 7) {
                btncargarEvidencia.attr("data-index", btnTerminarOverhaul.attr("data-index"));
                CargarTblEvidencia(btnTerminarOverhaul.attr("data-index"));
                $("#title-modal-evidencia").text("Archivos Evidencia");
                modalGuardarEvidencia.modal("show");
            }
        }


        function IniciarActividad(idActividad, id)
        {
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: '/Overhaul/IniciarActividadOHTaller',
                datatype: "json",
                type: "POST",
                data: {
                    idEvento: btnTerminarOverhaul.attr("data-index"),
                    idActividad: idActividad,
                    id: id
                },
                success: function (response) {
                    //$.unblockUI();
                    if (response.success) {                        
                        CargarGridActividades(btnTerminarOverhaul.attr("data-index"), 2);
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        function FinalizarActividad(idActividad, id) {
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: '/Overhaul/FinalizarActividadOHTaller',
                datatype: "json",
                type: "POST",
                data: {
                    idEvento: btnTerminarOverhaul.attr("data-index"),
                    idActividad: idActividad,
                    id: id
                },
                success: function (response) {
                    //$.unblockUI();
                    if (response.success) {
                        CargarGridActividades(btnTerminarOverhaul.attr("data-index"), 2);
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function GuardarComentario() {
            if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 3 || tipoUsuario == 7) {
                $.blockUI({ message: "Procesando..." });
                $.ajax({
                    url: "/Overhaul/GuardarComentarioActividadOverhaul",
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json',
                    data: JSON.stringify({ 
                        actividadID: btnModalGuardarComentario.attr("data-index"), 
                        eventoID: btnTerminarOverhaul.attr("data-index"), 
                        comentario: tbComentario.val().trim(), 
                        tipo: btnModalGuardarComentario.attr("data-tipo"),
                        numDia: btnModalGuardarComentario.attr("data-numDia")
                    }),
                    success: function (response) {
                        $.unblockUI();
                        if (response.success) {
                            modalGuardarComentario.modal("hide");
                        }
                        else {
                            AlertaGeneral("Alerta", "Error al realizar la consulta");
                        }
                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", response.message);
                    }
                });
            }
        }

        function GuardarComentarioPortada() {
            $.blockUI({ message: "Procesando..." });
            let comentarioActual = tbComentarioPortada.content.get().replace(/\<br \/>/g, '<br>');
            $.ajax({
                url: "/Overhaul/GuardarComentarioActividadOverhaul",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ actividadID: -1, eventoID: btnTerminarOverhaul.attr("data-index"), comentario: comentarioActual, tipo: 5, numDia: -1 }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        modalGuardarPortada.modal("hide");
                    }
                    else {
                        AlertaGeneral("Alerta", "Error al realizar la consulta");
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function GuardarComentarioReporte() {
            $.blockUI({ message: "Procesando..." });
            let comentarioActual = tbComentarioReporte.content.get().replace(/\<br \/>/g, '<br>');
            comentarioActual = comentarioActual.replace(/\u00a0/g, ' ');
            $.ajax({
                url: "/Overhaul/GuardarComentarioActividadOverhaul",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    actividadID: btncargarArchivo.attr("data-index"),
                    eventoID: btnTerminarOverhaul.attr("data-index"),
                    comentario: comentarioActual,
                    tipo: 1,
                    numDia: btnModalGuardarComentarioReporte.attr("data-numDia")
                }),
                success: function (response) {
                    $.unblockUI();
                    if (response.exito) {
                        modalGuardarArchivo.modal("hide");
                    }
                    else {
                        AlertaGeneral("Alerta", "Error al realizar la consulta");
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        

        //function CargarComentario(idActividad, idEvento, tipo)
        //{
        //    $.blockUI({ message: "Procesando..." });
        //    $.ajax({
        //        url: '/Overhaul/CargarComentarioActividadOverhaul',
        //        datatype: "json",
        //        type: "POST",
        //        data: {
        //            eventoID: idEvento,
        //            actividadID: idActividad,
        //            tipo: tipo
        //        },
        //        success: function (response) {
        //            $.unblockUI();
        //            if (response.success) {
        //                if (response.comentario != null) tbComentario.text(response.comentario);
        //                else tbComentario.text("");
        //                if (response.fecha != null) $("#txtfechaComentario").text("Última modificacion: " + response.fecha);
        //                else $("#txtfechaComentario").text("");
        //            }
        //        }
        //    });
        //}

        function SubirArchivoActividad(e, tipo, input)
        {
            e.preventDefault();
            if (document.getElementById(input).files[0] != null)  {
                var auxExt = document.getElementById(input).files[0].name.split('.');
                var ext = auxExt[auxExt.length - 1];
                ext = ext.toLowerCase();
                if (tipo == 1 ? ext == "pdf" : ($.inArray(ext, ["jpg", "jpeg", "png"]) != -1)) {
                    size = document.getElementById(input).files[0].size;
                    if (size > 52428800) {
                        AlertaGeneral("Alerta", "Archivo sobrepasa los 50MB");
                    }
                    else {
                        if (size <= 0) {
                            AlertaGeneral("Alerta", "Archivo vacío");
                        }
                        else {
                            guardarArchivoActividad(e, tipo, input);
                        }
                    }
                }
                else {
                    if (tipo == 1) { AlertaGeneral("Alerta", "Sólo se aceptan archivos PDF"); }
                    else { AlertaGeneral("Alerta", "Sólo se aceptan archivos tipo imagen"); }
                }
            }
        }

        function guardarArchivoActividad(e, tipo, input) {
            e.preventDefault();
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            var formData = new FormData();
            var request = new XMLHttpRequest();
            var file = document.getElementById(input).files[0];
            formData.append("archivoActividad", file);
            switch (tipo) {
                case 0:
                    formData.append("idEvento", btnTerminarOverhaul.attr("data-index"));
                    formData.append("numActividad", btncargarArchivo.attr("data-num-act"));
                    formData.append("idActividad", btncargarArchivo.attr("data-index"));
                    formData.append("tipo", 2);
                    formData.append("numDia", btnModalGuardarComentarioReporte.attr("data-numDia"));
                    break;
                case 1:
                    formData.append("idEvento", btncargarArchivoFL.attr("data-index"));
                    formData.append("numActividad", -1);
                    formData.append("idActividad", -1);
                    formData.append("tipo", 3);
                    formData.append("numDia", -1);
                    break;
                case 2:
                    formData.append("idEvento", btncargarPortada.attr("data-index"));
                    formData.append("numActividad", -1);
                    formData.append("idActividad", -1);
                    formData.append("tipo", 4);
                    formData.append("numDia", -1);
                    break;
                case 3:
                    formData.append("idEvento", btncargarEvidencia.attr("data-index"));
                    formData.append("numActividad", -1);
                    formData.append("idActividad", -1);
                    formData.append("tipo", 6);
                    formData.append("numDia", -1);
                    break;
                default:
                    break;
            }

            if (file != undefined) {
                $.blockUI({
                    message: 'Cargando archivo... Espere un momento',
                    baseZ: 2000
                });
            }
            $.ajax({
                type: "POST",
                url: '/Overhaul/GuardarArchivoActividad',
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (response) {
                    $.unblockUI();
                    switch (tipo) {
                        case 0:
                            CargarGridArchivos(btncargarArchivo.attr("data-index"), btnTerminarOverhaul.attr("data-index"), btnModalGuardarComentarioReporte.attr("data-numDia"));
                            break;
                        case 1:
                            CargarGridArchivosFL(-1, btnTerminarOverhaul.attr("data-index"));
                            break;
                        case 2:
                            CargarTblPortada(btnTerminarOverhaul.attr("data-index"));
                            break;
                        case 3:
                            CargarTblEvidencia(btnTerminarOverhaul.attr("data-index"));
                            break;
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", "Se encontró un error al tratar de guardar el archivo");
                }
            });
        }


        function IniciarGridArchivos()
        {
            gridArchivos = $("#gridArchivos").DataTable({
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
                drawCallback: function (settings) {
                    gridArchivos.on('click', '.btn-eliminar-archivo', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        EliminarArchivoActividad($(this).attr("data-index"));
                        CargarGridArchivos(btncargarArchivo.attr("data-index"), btnTerminarOverhaul.attr("data-index"), btnModalGuardarComentarioReporte.attr("data-numDia"));
                    });
                    gridArchivos.on('click', '.btn-descargar-archivo', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        DescargarArchivoActividad($(this).attr("data-index"));
                    });
                },
                columns: [

                    { data: 'id', title: 'ID' },
                    { data: 'fecha', title: 'FECHA' },
                    { data: 'nombre', title: 'NOMBRE' },
                    {
                        //sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = "";
                            html += '<button class="btn-eliminar-archivo btn btn-sm btn-danger glyphicon glyphicon-remove" type="button" data-index="' + row.id + '" style=""></button>';
                            return html;
                        },
                        title: '<span class="glyphicon glyphicon-remove" style="right:0;"></span>'
                    },
                    {
                        "render": function (data, type, row, meta) {
                            var html = "";
                            html += '<button class="btn-descargar-archivo btn btn-sm btn-primary glyphicon glyphicon-download-alt" type="button" data-index="' + row.id + '" style=""></button>';
                            return html;
                        },
                        title: '<span class="glyphicon glyphicon-download-alt" style="display:inline-block;right:0;"></span>'
                    },
                ],
                columnDefs: [
                    { "className": "dt-center", "targets": [0, 1, 2, 3, 4] },
                    { "visible": false, "targets": 0 },
                    { "orderable": false, "targets": [0, 1, 2, 3, 4] },
                    { "width": "20%", "targets": [1] },
                    { "width": "60%", "targets": [2] },
                    { "width": "10%", "targets": [3, 4] }
                ],
                order: [[0, 'asc']],
            });
        }

        function IniciarGridArchivosFL()
        {
            gridArchivosFL = $("#gridArchivosFL").DataTable({
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
                drawCallback: function (settings) {
                    gridArchivosFL.on('click', '.btn-eliminar-archivo', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        EliminarArchivoActividad($(this).attr("data-index"));
                        CargarGridArchivosFL(-1, btncargarArchivoFL.attr("data-index"));
                    });
                    gridArchivosFL.on('click', '.btn-descargar-archivo', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        DescargarArchivoActividad($(this).attr("data-index"));
                    });
                },
                columns: [

                    { data: 'id', title: 'ID' },
                    { data: 'fecha', title: 'FECHA' },
                    { data: 'nombre', title: 'NOMBRE' },
                    {
                        //sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = "";
                            html += '<button class="btn-eliminar-archivo btn btn-sm btn-danger glyphicon glyphicon-remove" type="button" data-index="' + row.id + '" style=""></button>';
                            return html;
                        },
                        title: '<span class="glyphicon glyphicon-remove" style="right:0;"></span>'
                    },
                    {
                        "render": function (data, type, row, meta) {
                            var html = "";
                            html += '<button class="btn-descargar-archivo btn btn-sm btn-primary glyphicon glyphicon-download-alt" type="button" data-index="' + row.id + '" style=""></button>';
                            return html;
                        },
                        title: '<span class="glyphicon glyphicon-download-alt" style="display:inline-block;right:0;"></span>'
                    },
                ],
                columnDefs: [
                    { "className": "dt-center", "targets": [0, 1, 2, 3, 4] },
                    { "visible": false, "targets": 0 },
                    { "orderable": false, "targets": [0, 1, 2, 3, 4] },
                    { "width": "20%", "targets": [1] },
                    { "width": "60%", "targets": [2] },
                    { "width": "10%", "targets": [3, 4] }
                ],
                order: [[0, 'asc']],
            });
        }

        function CargarGridArchivos(idActividad, idEvento, numDia) {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/cargarGridArchivosActividad',
                datatype: "json",
                type: "POST",
                data: {
                    idEvento: idEvento,
                    idActividad: idActividad,
                    tipo: 2,
                    numDia: numDia
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        gridArchivos.clear();
                        gridArchivos.rows.add(response.data);
                        gridArchivos.draw();
                    }
                }
            });
        }

        function CargarGridArchivosFL(idActividad, idEvento) {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/cargarGridArchivosActividad',
                datatype: "json",
                type: "POST",
                data: {
                    idEvento: idEvento,
                    idActividad: idActividad,
                    tipo: 3,
                    numDia : -1
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        gridArchivosFL.clear();
                        gridArchivosFL.rows.add(response.data);
                        gridArchivosFL.draw();
                    }
                }
            });
        }

        function DescargarArchivoActividad(idComentario) {
            window.location.href = "/Overhaul/DescargarArchivoActividad?idComentario=" + idComentario;
        }

        function EliminarArchivoActividad (idComentario) {
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: "/Overhaul/DeleteArchivoActividad",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({ idComentario: idComentario }),
                success: function (response) {                    
                    $.unblockUI();
                    ConfirmacionGeneral("Éxito", "Se eliminó el archivo.", "bg-green");
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", "Se encontró un error al tratar de eliminar el archivo");
                }
            });
        }
        function AbrirReporteDG(index)
        {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/getReporteEjecutivo',
                type: 'POST',
                dataType: 'json',
                async: false,
                contentType: 'application/json',
                data: JSON.stringify({ idEvento: index }),
                success: function (response) {
                    ireport.attr("src", response.html);
                    document.getElementById('report').onload = function () {
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

        function AbrirReporteEjecutivo(index) {

            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/getReporteEjecutivoImprimible',
                type: 'POST',
                dataType: 'json',
                async: false,
                contentType: 'application/json',
                data: JSON.stringify({ idEvento: index }),
                success: function (response) {
                    ireport.attr("src", response.html);

                    ireport.attr("src", response.html);

                    document.getElementById('report').onload = function () {
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

        function TerminarOverhaul()
        {            
            if (tipoUsuario == 0 || tipoUsuario == 1 || tipoUsuario == 3 || tipoUsuario == 7) {
             var fechafin = fechaFinParo.val();
                let estatus = btnTerminarOverhaul.attr("data-estatus");
                $.blockUI({ message: "Procesando..." });
                $.ajax({
                    url: '/Overhaul/TerminarOverhaulTaller',
                    datatype: "json",
                    type: "POST",
                    data: {
                        idEvento: btnTerminarOverhaul.attr("data-index"), 
                        tipoOverhaul: btnTerminarOverhaul.attr("data-tipo"), 
                        estatus: estatus,
                        fechafin: fechaFinParo.val()
                    },
                    success: function (response) {
                        $.unblockUI();
                        if (response.success) {
                            if (response.exito) {
                                CargarGridEstatus();
                                modalFechaFin.modal("hide");
                                if (estatus == "2") { AlertaGeneral("Alerta", "Se ha finalizado Overhaul con éxito"); }
                                else {
                                    if (estatus == "3") { AlertaGeneral("Alerta", "Se ha cerrado Diagrama de Gantt con éxito"); }
                                    else {
                                        if (estatus == "4") { AlertaGeneral("Alerta", "Se ha cerrado Reporte Ejecutivo con éxito"); }
                                    }                                    
                                }
                            }
                            else { AlertaGeneral("Alerta", "Ha ocurrido un error, no se ha finalizado"); }
                        }
                        else { AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados"); }
                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", "Se encontró un error al tratar de finalizar Overhaul");
                    }
                });
            }
        }

        function IniciarGridOHFalla() {
            gridOHFalla = $("#gridOHFalla").DataTable({
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
                drawCallback: function (settings) {
                    $("#ckSelectAllFalla").change(function () {
                        $("input.checkbox-lista-componentes").prop('checked', $(this).prop("checked"));
                        if ($(this).prop("checked")) { $("input.checkbox-lista-componentes").addClass("agregar"); }
                        else { $("input.checkbox-lista-componentes").removeClass("agregar"); }
                        if ($('.agregar').length > 0) { btnModalOverhaulFalla.prop("disabled", false); }
                        else { btnModalOverhaulFalla.prop("disabled", true); }
                    });
                    $("input.checkbox-lista-componentes").change(function () {
                        if ($(this).prop("checked")) { $(this).addClass("agregar"); }
                        else { $(this).removeClass("agregar"); }
                        if ($('.agregar').length > 0) { btnModalOverhaulFalla.prop("disabled", false); }
                        else { btnModalOverhaulFalla.prop("disabled", true); }
                    });
                },
                columns: [
                    {
                        data: 'id',
                        title: '<input id="ckSelectAllFalla" class="checkbox-select-all checkbox-componentes" value="1" type="checkbox">'
                    },
                    { data: 'nombre', title: 'COMPONENTE' },
                    {
                        "render": function (data, type, row, meta) { return row.descripcion + " " + row.posicion; },
                        title: 'DESCRIPCIÓN'
                    }
                ],
                columnDefs: [
                    {
                        "targets": 0,
                        "data": null,
                        "defaultContent": '',
                        "orderable": false,
                        'render': function (data, type, row, meta) {
                            return '<input class="checkbox-componentes checkbox-lista-componentes" type="checkbox" data-componenteID = "' + row.componenteID + '" data-nombre = "' + row.nombre +
                                '" data-descripcion = "' + row.descripcion + '" data-posicion = "' + row.posicion + '" data-Tipo = "' + row.Tipo + '" data-horasCiclo = "' + row.horasCiclo +
                                '" data-target = "' + row.target + '">';
                        }
                    },
                ],
                order: [[0, 'asc']],
            });
        }

        function CargarGridOHFalla()
        {
            if (cboEconomicoFalla.val() != "") {
                $.blockUI({ message: "Procesando..." });
                $.ajax({
                    url: '/Overhaul/CargarGridOHFallaTaller',
                    datatype: "json",
                    type: "POST",
                    data: {
                        idMaquina: cboEconomicoFalla.val()
                    },
                    success: function (response) {
                        $.unblockUI();
                        if (response.success) {
                            gridOHFalla.clear();
                            gridOHFalla.rows.add(response.data);
                            gridOHFalla.draw();
                        }
                        else { AlertaGeneral("Alerta", "Se encontró un error al cargar los datos"); }
                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", "Se encontró un error al cargar los datos");
                    }
                });
            }
            else { gridOHFalla.clear(); gridOHFalla.draw(); }
        }

        function GuardarOHFalla()
        {
            componentes = [];
            $("input.agregar").each(function () {
                componentes.push({
                    Value: "0",
                    componenteID: $(this).attr('data-componenteID'),
                    nombre: $(this).attr('data-nombre'),
                    descripcion: $(this).attr('data-descripcion'),
                    posicion: $(this).attr('data-posicion'),
                    Tipo: $(this).attr('data-tipo'),
                    horasCiclo: $(this).attr('data-horasCiclo'),
                    target: $(this).attr('data-target'),
                    tipoOverhaul: cboTipoFalla.val(),
                    falla: cboTipoFalla.val() == 3 ? true: false,
                                
                });
            });
            var existeParo = VerificarParo();

            if (existeParo != null)
            {
                idFallaGlobal = existeParo.id;
                ConfirmacionEliminacionCustom("Confirmación", "Ya existe un paro abierto de ese economico el dia:" + existeParo.fecha + ", ¿desea agregar el fallo a este paro?", "Agregar", "Crear Nuevo Paro");
            }
            else {
                $.blockUI({ message: "Procesando..." });
                $.ajax({
                    url: '/Overhaul/GuardarOHFallaTaller',
                    datatype: "json",
                    type: "POST",
                    data: {
                        idMaquina: cboEconomicoFalla.val(),
                        componentes: componentes,
                        mes: btnModalOverhaulFalla.attr("data-mes"),
                        anio: anio,
                        calendarioID: idCalendario,
                        tipo: cboTipoFalla.val()
                    },
                    success: function (response) {
                        $.unblockUI();
                        if (response.success) {
                            CargarGridEstatus();
                            AlertaGeneral("Éxito", "Se guardó Overhaul correctamente");
                            modalOverhaulFalla.modal("hide");
                        }
                        else { AlertaGeneral("Alerta", "Se encontró un error al guardar los datos"); }
                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", "Se encontró un error al guardar los datos");
                    }
                });
            }
        }

        function VerificarParo() {
            var data = [];
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/VerificarOHFallaTaller',
                datatype: "json",
                type: "POST",
                async: false,
                data: {
                    idMaquina: cboEconomicoFalla.val(),
                    mes: btnModalOverhaulFalla.attr("data-mes"),
                    anio: anio,
                    calendarioID: idCalendario
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        data = response.data;
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", "Se encontró un error al guardar los datos");
                }
            });
            return data;
        }

        function EnviarLiberacion() {
            var path = "/Reportes/Vista.aspx?idReporte=154&index=" + btncargarArchivoFL.attr("data-index") + "&tipo=1&inMemory=1";
            let correos = [];
            $('#tblCorreos tbody tr').each(function () {
                correos.push($(this).find('td:eq(0)').text());
            });
            ireport.attr("src", path);
            document.getElementById('report').onload = function () {
                var path = "/Reportes/Vista.aspx?idReporte=155&index=" + btncargarArchivoFL.attr("data-index") + "&inMemory=1";
                ireport.attr("src", path);
                document.getElementById('report').onload = function () {
                    $.ajax({
                        datatype: "json",
                        type: "POST",
                        url: '/Overhaul/EnviarLiberacionTaller',
                        data: { idEvento: btncargarArchivoFL.attr("data-index"), tipoOverhaul: btnEnviarLiberacion.attr("data-tipo"), correos: correos },
                        success: function (response) {
                            CargarGridEstatus();
                            modalGuardarArchivoFL.modal("hide");
                            modalCorreos.modal("hide");
                        },
                        error: function () {
                            $.unblockUI();
                        }
                    });
                };
            };
        }

        function IniciarTblPortada() {
            tblPortada = $("#tblPortada").DataTable({
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
                drawCallback: function (settings) {
                    tblPortada.on('click', '.btn-eliminar-archivo', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        EliminarArchivoActividad($(this).attr("data-index"));
                        CargarTblPortada(btnTerminarOverhaul.attr("data-index"));
                    });
                    tblPortada.on('click', '.btn-descargar-archivo', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        DescargarArchivoActividad($(this).attr("data-index"));
                    });
                },
                columns: [

                    { data: 'id', title: 'ID' },
                    { data: 'fecha', title: 'FECHA' },
                    { data: 'nombre', title: 'NOMBRE' },
                    {
                        //sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = "";
                            html += '<button class="btn-eliminar-archivo btn btn-sm btn-danger glyphicon glyphicon-remove" type="button" data-index="' + row.id + '" style=""></button>';
                            return html;
                        },
                        title: '<span class="glyphicon glyphicon-remove" style="right:0;"></span>'
                    },
                    {
                        "render": function (data, type, row, meta) {
                            var html = "";
                            html += '<button class="btn-descargar-archivo btn btn-sm btn-primary glyphicon glyphicon-download-alt" type="button" data-index="' + row.id + '" style=""></button>';
                            return html;
                        },
                        title: '<span class="glyphicon glyphicon-download-alt" style="display:inline-block;right:0;"></span>'
                    },
                ],
                columnDefs: [
                    { "className": "dt-center", "targets": [0, 1, 2, 3, 4] },
                    { "visible": false, "targets": 0 },
                    { "orderable": false, "targets": [0, 1, 2, 3, 4] },
                    { "width": "20%", "targets": [1] },
                    { "width": "60%", "targets": [2] },
                    { "width": "10%", "targets": [3, 4] }
                ],
                order: [[0, 'asc']],
            });
        }
        function CargarTblPortada(idEvento) {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/cargarGridArchivosActividad',
                datatype: "json",
                type: "POST",
                data: {
                    idEvento: idEvento,
                    idActividad: -1,
                    tipo: 4,
                    numDia: -1
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        tblPortada.clear();
                        tblPortada.rows.add(response.data);
                        tblPortada.draw();
                    }
                }
            });
        }

        function IniciarTblEvidencia() {
            tblEvidencia = $("#tblEvidencia").DataTable({
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
                drawCallback: function (settings) {
                    tblEvidencia.on('click', '.btn-eliminar-archivo', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        EliminarArchivoActividad($(this).attr("data-index"));
                        CargarTblEvidencia(btnTerminarOverhaul.attr("data-index"));
                    });
                    tblEvidencia.on('click', '.btn-descargar-archivo', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        DescargarArchivoActividad($(this).attr("data-index"));
                    });
                },
                columns: [

                    { data: 'id', title: 'ID' },
                    { data: 'fecha', title: 'FECHA' },
                    { data: 'nombre', title: 'NOMBRE' },
                    {
                        //sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = "";
                            html += '<button class="btn-eliminar-archivo btn btn-sm btn-danger glyphicon glyphicon-remove" type="button" data-index="' + row.id + '" style=""></button>';
                            return html;
                        },
                        title: '<span class="glyphicon glyphicon-remove" style="right:0;"></span>'
                    },
                    {
                        "render": function (data, type, row, meta) {
                            var html = "";
                            html += '<button class="btn-descargar-archivo btn btn-sm btn-primary glyphicon glyphicon-download-alt" type="button" data-index="' + row.id + '" style=""></button>';
                            return html;
                        },
                        title: '<span class="glyphicon glyphicon-download-alt" style="display:inline-block;right:0;"></span>'
                    },
                ],
                columnDefs: [
                    { "className": "dt-center", "targets": [0, 1, 2, 3, 4] },
                    { "visible": false, "targets": 0 },
                    { "orderable": false, "targets": [0, 1, 2, 3, 4] },
                    { "width": "20%", "targets": [1] },
                    { "width": "60%", "targets": [2] },
                    { "width": "10%", "targets": [3, 4] }
                ],
                order: [[0, 'asc']],
            });
        }
        function CargarTblEvidencia(idEvento) {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/cargarGridArchivosActividad',
                datatype: "json",
                type: "POST",
                data: {
                    idEvento: idEvento,
                    idActividad: -1,
                    tipo: 6,
                    numDia: -1
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        tblEvidencia.clear();
                        tblEvidencia.rows.add(response.data);
                        tblEvidencia.draw();
                    }
                }
            });
        }

        function CargartablaCorreos(tabla, listaCorreos) {
            $.ajax({
                url: '/Overhaul/GetCorreosOverhaul',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({

                }),
                success: function (response) {
                    tabla.bootgrid("clear");
                    var JSONINFO = [{ "id": 0, "correo": response.correosPpales[0] }];
                    tabla.bootgrid("append", JSONINFO);
                    let j = 1;
                    for (i = 0; i < listaCorreos.length; i++, j++) {
                        if (listaCorreos[i] == 1) {
                            var JSONINFO = [{ "id": (i + 1), "correo": response.correosPpales[(i + 1)] }];
                            tabla.bootgrid("append", JSONINFO);
                        }
                    }                    
                    for (i = 0; i < response.correosLocacion.length; i++, j++) {
                        var JSONINFO = [{ "id": j, "correo": response.correosLocacion[i] }];
                        tabla.bootgrid("append", JSONINFO);

                    }                    
                    ultimoIDCorreo = j;
                    tabla.bootgrid('reload');
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function initTblCorreos() {
            ultimoIDCorreo = 0;
            tblCorreos.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: { header: "" },
                sorting: false,
                formatters: { "eliminar": function (column, row) { return "<button type='button' class='btn btn-sm btn-danger eliminar'><span class='glyphicon glyphicon-remove'></span></button>"; } }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblCorreos.find(".eliminar").parent().css("text-align", "center");
                tblCorreos.find(".eliminar").parent().css("width", "3%");
                tblCorreos.find(".eliminar").on("click", function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    var rowID = parseInt($(this).parent().parent().attr('data-row-id'));
                    tblCorreos.bootgrid("remove", [rowID]);
                });
            });
        }

        function AgregarCorreo() {
            let correo = $("#txtCorreo").val().trim();
            if (correo != "" && validateEmail(correo)) {
                var JSONINFO = [{ "id": ultimoIDCorreo, "correo": correo }];
                tblCorreos.bootgrid("append", JSONINFO);
                ultimoIDCorreo++;
            }
            else { AlertaGeneral("Alerta", "No se ha proporcionado un correo electrónico válido"); }
        }

        function validateEmail(email) {
            var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
            if (!emailReg.test(email)) {
                return false;
            } else {
                return true;
            }
        }

        //Creación Diagrama Gantt

        function mostrarModalAgrAct() {

            tblAgregAct.bootgrid("clear");
            for (let i = 0; i < selectedOpts.length; i++)
            {                
                var JSONINFO = [{ "id": $(selectedOpts[i]).val(), "actividad": $(selectedOpts[i]).text(), "hrsDuracion": $(selectedOpts[i]).attr("data-Prefijo") }];
                    tblAgregAct.bootgrid("append", JSONINFO);
            }
            tblAgregAct.bootgrid('reload');
            modalAgregarAct.modal("show");
        }

        function initTblAgrAct() {
            ultimoIDCorreo = 0;
            tblAgregAct.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: { header: "" },
                sorting: false,
                formatters: {
                    "duracion": function (column, row) {
                        return '<input type="number" min="0" value="' + row.hrsDuracion + '" class="form-control horas-agract" data-index="' + row.id + '">';
                    }
                }
            });
        }

        function AgregarATblDiagrama(rows, horasTrabajadas)
        {
            if (horasTrabajadas > 0) {
                //restaHoras = horasTrabajadas;
                for (let i = 0; i < rows.length; i++) {
                    let input = parseFloat($("input.horas-agract[data-index='" + rows[i].id + "']").val().trim().toUpperCase());
                    if (input > restaHoras) {
                        if (restaHoras > 0) {
                            tblDiagrama.row.add({ "dia": diaActualDG, "id": rows[i].id, "actividad": rows[i].actividad, "duracion": restaHoras }).draw();
                            input -= restaHoras;
                        }
                        diaActualDG++;
                        restaHoras = horasTrabajadas;
                    }
                    while (input > restaHoras) {
                        tblDiagrama.row.add({ "dia": diaActualDG, "id": rows[i].id, "actividad": rows[i].actividad, "duracion": restaHoras }).draw();
                        input -= restaHoras;
                        diaActualDG++;
                    }
                    if (input > 0) {
                        tblDiagrama.row.add({ "dia": diaActualDG, "id": rows[i].id, "actividad": rows[i].actividad, "duracion": input }).draw();
                        restaHoras -= input;
                    }
                }
                ActivarGuardado(rows.length);
                modalAgregarAct.modal("hide");
            }
        }

        function IniciarTblDiagrama()
        {
            tblDiagrama = $("#tblDiagrama").DataTable({
                language: { "sProcessing": "Procesando...", "sLengthMenu": "Mostrar _MENU_ registros", "sZeroRecords": "No se encontraron resultados", "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "", "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)", "sInfoPostFix": "", "sSearch": "Buscar:", "sUrl": "", "sInfoThousands": ",", "sLoadingRecords": "Cargando...",
                    "oPaginate": { "sFirst": "Primero", "sLast": "Último", "sNext": "Siguiente", "sPrevious": "Anterior" },
                    "oAria": { "sSortAscending": ": Activar para ordenar la columna de manera ascendente", "sSortDescending": ": Activar para ordenar la columna de manera descendente" }
                },
                retrieve: true, searching: false, paging: false, scrollY: '420px', scrollCollapse: true, autoWidth: false, select: { style: 'os', selector: 'td:first-child' },
                drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;
                    api.column(0, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(
                                '<tr class="group" style="background: rgb(68,133,51) !important; background: linear-gradient(40deg, #45cafc, #303f9f) !important;color:white;text-align:center;"><td colspan="10" style="">' +
                                '<b style="font-size: 18px;">Día ' + group + '</b></td></tr>'
                            );
                            last = group;
                        }
                    });
                },
                columns: [
                    { data: 'dia', title: 'DIA' },
                    { data: 'id', title: 'ID' },                    
                    { data: 'actividad', title: 'ACTIVIDAD' },
                    { data: 'duracion', title: 'HRS' },
                ],
                columnDefs: [
                    { "className": "dt-center", "targets": [0, 1, 2, 3] },
                    { "visible": false, "targets": [0, 1] },
                    { "orderable": false, "targets": [0, 1, 2, 3] },
                    { "width": "10%", "targets": [3] },
                    { "width": "90%", "targets": [2] }
                ],
                order: [[0, 'asc']],
            });
        }

        function CargarTblDiagrama()
        {
            tblDiagrama.clear().draw();
        }

        function RecargarTblDiagrama()
        {
            if (txtHorasTrabajadas.val() != "" && parseFloat(txtHorasTrabajadas.val()) > 0) {
                diaActualDG = 1;
                restaHoras = parseFloat(txtHorasTrabajadas.val());
                let rowsBack = tblDiagrama.rows().data();
                let auxRows = [];
                let auxActividadDuracion = 0;
                let auxActividadID = 0;
                let auxActividad = "";
                for (let i = 0; i < rowsBack.length; i++) {
                    auxActividadID = rowsBack[i].id;
                    auxActividad = rowsBack[i].actividad;
                    auxActividadDuracion = 0;
                    while (i < rowsBack.length && rowsBack[i].id == auxActividadID) {
                        auxActividadDuracion += rowsBack[i].duracion;
                        i++;
                    }
                    auxRows.push({ id: auxActividadID, actividad: auxActividad, duracion: auxActividadDuracion });
                    i--;
                }
                tblDiagrama.clear();
                AgregarATblDiagrama(auxRows, parseFloat(txtHorasTrabajadas.val()));
                let numRows = tblDiagrama.rows().data().length;
                ActivarGuardado(numRows);
                tblDiagrama.draw();
            }
        }

        function ActivarGuardado(numRows)
        {
            if (numRows > 0) { btnModalGuardarNuevo.prop("disabled", false); }
            else { btnModalGuardarNuevo.prop("disabled", true); }
        }

        //Catálogo de actividades
        function cargarCatAct()
        {
            modalCatActividades.modal("show");
            dtTblCatActividades.clear();
            descripcionCatAct.val("");
            cbModeloCatAct.val("");
            cbModeloCatAct.change();
            cbEstatusCatAct.val("1");
            cbEstatusCatAct.change();
        }
        
        function initTblCatAct() {
            dtTblCatActividades = tblCatActividades.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                dom: '<<t>p>',
                columns: [
                    { data: 'id', title: 'ActividadID', visible: false },
                    { data: 'modeloID', title: 'modeloID', visible: false },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'hrsDuracion', title: 'Horas<br>Recom.' },                    
                    { data: 'modelo', title: 'Modelo' },
                    { data: 'reporteEjecutivo', title: 'RE', render: function (data, type, row) { return '<span class="reporteEjecutivo ' + (row.reporteEjecutivo ? 'glyphicon glyphicon-ok' : 'glyphicon glyphicon-remove') + '"> </span>'; } },
                    { title: 'Modificar', render: function (data, type, row) { return '<button class="btn modificar btn-sm btn-warning" data-id="' + row.id + '"><i class="fas fa-edit"></i></button>'; } }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                order: [[0, 'asc'], [1, 'asc'], [2, 'asc']],
                drawCallback: function () {
                    tblCatActividades.find('button.modificar').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        var actividadID = $(this).attr("data-id");
                        UpdateCatAct(actividadID)
                    });
                }               
            });
        }

        function cargarTblCatAct() {
            $.blockUI({ message: "Procesando...", baseZ: 2000 });
            $.ajax({
                url: '/Overhaul/CargarCatActTaller',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    idModelo: cbModeloCatAct.val(),
                    descripcion: descripcionCatAct.val().trim(),
                    estatus: cbEstatusCatAct.val() == "1"
                }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        // Operación exitosa.
                        dtTblCatActividades.clear();
                        dtTblCatActividades.rows.add(response.actividades);
                        dtTblCatActividades.draw();
                    } else {
                        // Operación no completada.
                        AlertaGeneral("Operación fallida", "No se pudo completar la operación: " + response.message);
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Operación fallida", "Ocurrió un error al lanzar la petición al servidor: Error " + response.status + " - " + response.statusText + ".");
                }
            });
        }

        function AltaCatAct()
        {
            limpiarCatAct();
            modalCatActUpdate.modal("show");
        }

        function AltaCatActPorModelo() {
            limpiarCatAct();
            cbModeloCatActUpdate.val(btnAgregarAct.attr("data-modeloID"));
            cbModeloCatActUpdate.change();
            cbModeloCatActUpdate.prop('disabled', true);
            modalCatActUpdate.modal("show");
        }

        function UpdateCatAct(actividadID)
        {
            limpiarCatAct();
            setUpdateCatAct(actividadID);
            modalCatActUpdate.modal("show");
        }

        function limpiarCatAct()
        {
            cbModeloCatActUpdate.prop('disabled', false);
            lblCatActUpdate.text("Alta de Actividad")
            descripcionCatActUpdate.val("");
            cbModeloCatActUpdate.val("");
            cbModeloCatActUpdate.change();
            cbEstatusCatActUpdate.val("1");
            cbEstatusCatActUpdate.change();
            ckbRECatActUpdate.prop("checked", true);
            horasCatActUpdate.val("");
            btnGuardarCatActUpdate.attr("data-id", "0");
        }

        function setUpdateCatAct(actividadID)
        {
            $.blockUI({ message: "Procesando...", baseZ: 2000 });
            $.ajax({
                url: '/Overhaul/CargarActividadTaller',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    actividadID: actividadID
                }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        // Operación exitosa.
                        const actividad = response.actividad;
                        lblCatActUpdate.text("Edición de Actividad")
                        descripcionCatActUpdate.val(actividad.descripcion);
                        cbModeloCatActUpdate.val(actividad.modeloID);
                        cbModeloCatActUpdate.change();
                        cbEstatusCatActUpdate.val(actividad.estatus ? "1" : "0");
                        cbEstatusCatActUpdate.change();
                        ckbRECatActUpdate.prop("checked", actividad.reporteEjecutivo);
                        horasCatActUpdate.val(actividad.horasDuracion);
                        btnGuardarCatActUpdate.attr("data-id", actividad.id);
                    } else {
                        // Operación no completada.
                        AlertaGeneral("Operación fallida", "No se pudo completar la operación: " + response.message);
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Operación fallida", "Ocurrió un error al lanzar la petición al servidor: Error " + response.status + " - " + response.statusText + ".");
                }
            });
        }

        function GuardarCatAct()
        {
            if (validarCarAct()) {
                $.blockUI({ message: "Procesando...", baseZ: 2000 });
                $.ajax({
                    url: '/Overhaul/GuardarActividadTaller',
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json',
                    data: JSON.stringify({
                        actividadID: btnGuardarCatActUpdate.attr("data-id"),
                        descripcion: descripcionCatActUpdate.val().trim(),
                        modeloID: cbModeloCatActUpdate.val(),
                        estatus: cbEstatusCatActUpdate.val() == "1",
                        reporteEjecutivo: ckbRECatActUpdate.prop("checked"),
                        horasDuracion: horasCatActUpdate.val()
                    }),
                    success: function (response) {
                        $.unblockUI();
                        if (response.success) {
                            // Operación exitosa.
                            modalCatActUpdate.modal("hide");
                            btnBuscarCatAct.click();
                            if (modalNuevoOverhaul.hasClass('in'))
                            {
                                $('#lstBox1').fillCombo("/Overhaul/FillCboDiagramaGantt", { idModelo: btnAgregarAct.attr("data-modeloID"), idEvento: btnAgregarAct.attr("data-eventoID") }, true);
                            }
                            AlertaGeneral("Operación exitosa", "Se han guardado los cambios");
                        } else {
                            // Operación no completada.
                            AlertaGeneral("Operación fallida", "No se pudo completar la operación: " + response.message);
                        }
                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Operación fallida", "Ocurrió un error al lanzar la petición al servidor: Error " + response.status + " - " + response.statusText + ".");
                    }
                });
            }
            else
            {
                AlertaGeneral("Operación fallida", "Se requieren todos los campos");
            }
        }

        function validarCarAct()
        {
            var estado = true;
            descripcionCatActUpdate.css("background-color", "#fff");
            cbModeloCatActUpdate.parent().find(".select2-selection").css("background-color", "#fff");
            horasCatActUpdate.css("background-color", "#fff");
            
            if (descripcionCatActUpdate.val().trim() == "") { descripcionCatActUpdate.css("background-color", "pink"); estado = false; }
            if (cbModeloCatActUpdate.val() == "") { cbModeloCatActUpdate.parent().find(".select2-selection").css("background-color", "pink"); estado = false; }
            if (horasCatActUpdate.val() == "") { horasCatActUpdate.css("background-color", "pink"); estado = false; }
            return estado;
        }

        init();
    };

    $(document).ready(function () {
        maquinaria.overhaul.talleroverhaul = new talleroverhaul();
    });
})();


