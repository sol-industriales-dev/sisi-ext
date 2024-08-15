(function () {

    $.namespace('maquinaria.overhaul.locaciones');

    locaciones = function () {
        var idLocacion = 0;
        var saveOrUpdate = 0;
        var ultimoIDCorreo = 0;
        gridLocaciones = $("#grid_Locaciones"),
        modalAcciones = $("#modalLocaciones"),
        tituloModal = $("#title-modal"),
        btnNuevo = $("#btnNueva_Locacion"),
        btnGuardar = $("#btnModalGuardar_Locacion"),
        txtDescripcion = $("#txtDescripcion"),
        cboModalTipoLocacion = $("#cboModalTipoLocacion"),
        txtFiltroDescripcionLocacion = $("#txtFiltroDescripcionLocacion"),
        cboFiltroEstatusLocacion = $("#cboFiltroEstatusLocacion"),
        btnBuscarLocaciones = $("#btnBuscar_Locaciones"),
        cboModalCC = $("#cboModalCC"),
        btnAdminCorreos = $("#btnAdminCorreos"),
        lbCorreos = $("#lbCorreos"),
        modalCorreos = $("#modalCorreos"),
        tblCorreos = $("#tblCorreos"),
        btnAgregarCorreo = $("#btnAgregarCorreo");
        mensajes = {
            NOMBRE: 'Componentes',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        function init() {
            initGrid();
            cargarGrid();
            btnNuevo.click(openModal);
            btnGuardar.click(guardarLocacion);
            btnBuscarLocaciones.click(cargarGrid);
            txtFiltroDescripcionLocacion.change(cargarGrid);
            cboFiltroEstatusLocacion.change(cargarGrid);
            cboModalTipoLocacion.change(habilitarTipoAlmacen);
            cboModalCC.fillCombo('/Overhaul/FillCbo_CentroCostos');
            $(document).on('click', "#btnModalEliminar", function () {
                eliminarLocacion();
            });
            IniciarGridCorreos();
            btnAgregarCorreo.click(AgregarCorreo);
        }

        function habilitarTipoAlmacen()
        {
            if (cboModalTipoLocacion.val() == "1") {
                cboModalCC.parent().css("display", "inline");
                lbCorreos.text("Asministrar correos facilitadores");
            }
            else {
                cboModalCC.parent().css("display", "none");
                lbCorreos.text("Asministrar correos proveedor");
            }            
        }

        function initGrid() {
            gridLocaciones.bootgrid({
                headerCssClass: '.bg-table-header',
                rowCount: -1,
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "update": function (column, row) {
                        return "<button type='button' class='btn btn-warning modificar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "' data-tipolocacion='" + row.tipoLocacion + "' data-cc='" + row.areaCuenta + "'>" +
                        "<span class='glyphicon glyphicon-edit '></span> </button>";
                    },
                    "delete": function (column, row) {
                        return "<button type='button' class='btn btn-danger eliminar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'>" +
                        "<span class='glyphicon glyphicon-remove'></span>  </button>";
                    },
                    "tipoLocacion": function (column, row) {
                        return "<span class='tipoLocacion'>" + (row.tipoLocacion == 1 ? "ALMACEN" : "PROVEEDOR") + "</span>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                gridLocaciones.find(".modificar").parent().css("width", "3%");
                gridLocaciones.find(".eliminar").parent().css("width", "3%");
                gridLocaciones.find(".modificar").parent().css("text-align", "center");
                gridLocaciones.find(".eliminar").parent().css("text-align", "center");
                gridLocaciones.find(".modificar").on("click", function (e) {
                    idLocacion = $(this).attr("data-index");
                    txtDescripcion.val($(this).attr("data-descrip"));
                    cboModalTipoLocacion.val($(this).attr("data-tipolocacion"));
                    cboModalTipoLocacion.change();
                    cboModalCC.val($(this).attr("data-cc"));
                    cboModalCC.change();
                    tituloModal.text("Actualizar Tipo de Maquinaria");
                    $("#txtCorreo").val('');
                    saveOrUpdate = 1;
                    CargartablaCorreos(tblCorreos);
                    modalAcciones.modal('show');
                });

                gridLocaciones.find(".eliminar").on("click", function (e) {
                    idLocacion = $(this).attr("data-index");
                    ConfirmacionEliminacion("Eliminar Locación", "¿Esta seguro que desea dar de baja la locación \"" + $(this).attr("data-descrip") + "\"?");                    
                });
            });
        }

        function cargarGrid()
        {
            $.blockUI({ message: "Procesando..." });
            estatus = cboFiltroEstatusLocacion.val() == "1" ? true : false;
            descripcion = txtFiltroDescripcionLocacion.val().trim();
            $.ajax({
                url: "/Overhaul/CargarLocaciones",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ estatus: estatus, descripcion: descripcion }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        gridLocaciones.bootgrid({
                            templates: {
                                header: ""
                            }
                        });
                        gridLocaciones.bootgrid("clear");
                        gridLocaciones.bootgrid("append", response.rows);
                        gridLocaciones.bootgrid('reload');
                    }
                    else {
                        AlertaGeneral("Alerta", "no se obtuvieron registros con los filtros seleccionados");
                    }                    
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                    
                }
            });
        }

        function openModal() {
            tituloModal.text("Alta Locación");
            reset();
            modalAcciones.modal('show');
        }

        function guardarLocacion()
        {
            if (valid()) {
                altaUpdateLocacion(getElementosLocacion());
                saveOrUpdate = 0;
                idLocacion = 0;
            }
            else { AlertaGeneral("Alerta", "Uno o más datos son inválidos"); }
        }

        function getElementosLocacion()
        {
            let correos = [];
            var numCorreos = $("#tblCorreos").bootgrid("getTotalRowCount");
            if (numCorreos > 0) {
                $('#tblCorreos tbody tr').each(function () { correos.push($(this).find('td:eq(0)').text()); });
            }
            return {
                id: idLocacion,
                tipoLocacion: cboModalTipoLocacion.val(),
                descripcion: txtDescripcion.val().trim(),
                estatus: cboFiltroEstatusLocacion.val() == "1" ? true : false,
                areaCuenta: cboModalTipoLocacion.val() == 1 ? cboModalCC.val() : "",
                JsonCorreos: JSON.stringify(correos)
            }
           
        }

        function valid()
        {
            var state = true;
            if (txtDescripcion.val() == "" || !txtDescripcion.valid()) { state = false; }
            if (cboModalTipoLocacion.val() == "") { state = false; }
            if (cboModalTipoLocacion.val() == "1" && cboModalCC.val() == "") { state = false; }
            return state;
        }

        function altaUpdateLocacion(obj1) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/overhaul/AltaUpdateLocacion',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ locacion: obj1 }),
                success: function (response) {
                    $.unblockUI();
                    modalAcciones.modal('hide');
                    reset();
                    ConfirmacionGeneral("Confirmación", "Se guardo la Locación", "bg-green");                  
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);                    
                }
            });
        }

        function updateLocacion(obj1)
        {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/overhaul/AltaUpdateLocacion',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ locacion: obj1 }),
                success: function (response) {
                    modalAcciones.modal('hide');
                    ConfirmacionGeneral("Confirmación", "Se actualizó la Locación", "bg-green");
                    reset();
                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                    $.unblockUI();
                }
            });
        
        }

        function eliminarLocacion()
        {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/overhaul/BajaLocacion',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ idLocacion: idLocacion }),
                success: function (response) {
                    modalAcciones.modal('hide');
                    ConfirmacionGeneral("Confirmación", "Se eliminó la Locación", "bg-green");
                    reset();
                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                    $.unblockUI();
                }
            });
        }

        function reset()
        {
            idLocacion = 0;
            tblCorreos.bootgrid("clear");
            tituloModal.val(""); 
            txtDescripcion.val(""),
            cboModalTipoLocacion.val("2");
            cboModalTipoLocacion.change();
            cboModalCC.val("");
            $("#txtCorreo").val('');
            cboModalCC.change();
            cargarGrid();
        }

        function IniciarGridCorreos() {
            ultimoIDCorreo = 0;
            tblCorreos.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: { header: "" },
                sorting: false,
                formatters: { "eliminar": function (column, row) { return "<button type='button' class='btn btn-danger eliminar'><span class='glyphicon glyphicon-remove'></span></button>"; } }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblCorreos.find(".eliminar").parent().css("text-align", "center");
                tblCorreos.find(".eliminar").parent().css("width", "3%");
                tblCorreos.find(".eliminar").on("click", function (e) {
                    var rowID = parseInt($(this).parent().parent().attr('data-row-id'));
                    tblCorreos.bootgrid("remove", [rowID]);
                });
            });
        }

        function CargartablaCorreos(tabla) {
            $.ajax({
                type: "POST",
                url: '/Overhaul/GetCorreosLocacionOverhaul',
                async: false,
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ idLocacion: idLocacion }),
                success: function (response) {
                    tabla.bootgrid("clear");
                    console.log(response.correos);
                    var correos = response.correos;
                    for (let i = 0; i < correos.length; i++) {
                        var JSONINFO = [{ "id": i, "correo": response.correos[i] }];
                        tabla.bootgrid("append", JSONINFO);
                    }
                    ultimoIDCorreo = correos.length;
                    tabla.bootgrid('reload');
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function AgregarCorreo() {
            let correo = $("#txtCorreo").val().trim();
            if (correo != "" && validateEmail(correo)) {
                var JSONINFO = [{ "id": ultimoIDCorreo, "correo": correo }];
                tblCorreos.bootgrid("append", JSONINFO);
                ultimoIDCorreo++;
                $("#txtCorreo").val('');
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

        init();

    };

    $(document).ready(function () {
        maquinaria.overhaul.locaciones = new locaciones();
    });
})();


