(function () {

    $.namespace('maquinaria.catalogo.tipoMaqinaria');

    tipoMaquinaria = function () {
        idTipoMaquinaria = 0,
         Actualizacion = 1,
        ruta = '/CatTipos/FillGrid_TipoMaquinaria';
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'

        };
        mensajes = {
            NOMBRE: 'Tipo Maquinaria',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        formTipoMaquinaria = $("#frmTipoMaquinaria"),

        txtModaldescripcionTipoMaquinaria = $("#txtModaldescripcion"),
        txtDescripcionTipoMaquinaria = $("#txtDescripcion"),
        cboEstatusTipoMaquinaria = $("#cboEstatusMaquinaria"),
        gridTipoMaquinaria = $("#grid_TipoMaquinaria"),
        btnBuscar = $("#btnBuscar_TipoMaquinaria"),
        btnNuevo = $("#btnNuevo_TipoMaquinaria"),
        btnGuardar = $("#btnModalGuardar_TipoMaquinaria"),
        btnCancelar = $("#btnModalCancelar_TipoMaquinaria"),
        modalAlta = $("#modalAltaTipoMaquinaria"),
        tituloModal = $("#title-modal"),
        cboModalEstatusMaquinaria = $("#cboModalEstatusMaquinaria"),
        grid_TipoMaquinaria = $("#grid_TipoMaquinaria");

        $(document).on('click', "#btnModalEliminar", function () {
            beforeSaveOrUpdate();
            reset();
        });

        function init() {
            txtModaldescripcionTipoMaquinaria.addClass('required').attr('maxlength', 100);
            txtDescripcionTipoMaquinaria.attr('maxlength', 100);
            btnNuevo.click(openModal);
            btnGuardar.click(guardar);
            btnCancelar.click(reset);
            btnBuscar.click(clickBuscar);
            initGrid();
        }

        function clickBuscar() {
            filtrarGrid();
        }

        function openModal() {
            reset();
            tituloModal.text("Alta Tipo de Maquinaria");
            cboModalEstatusMaquinaria.prop('disabled', true);
            modalAlta.modal('show');
        }
        function update() {
            
            tituloModal.text("Actualizar Tipo de Maquinaria");
            cboModalEstatusMaquinaria.prop('disabled', false);
            modalAlta.modal('show');
        }

        function guardar() {
            beforeSaveOrUpdate();
        }

        function beforeSaveOrUpdate() {

            if (valid()) {
                saveOrUpdate(getPlainObject());
            }
        }

        function getPlainObject() {
            return {
                Id: idTipoMaquinaria,
                descripcion: txtModaldescripcionTipoMaquinaria.val().trim(),
                Estatus: cboModalEstatusMaquinaria.val() == estatus.ACTIVO ? true : false
            }
        }
        function filtrarGrid() {
            loadGrid(getFiltrosObject(), ruta, grid_TipoMaquinaria);
        }

        function getFiltrosObject() {
            return {
                Id: 0,
                descripcion: txtDescripcionTipoMaquinaria.val().trim(),
                Estatus: cboEstatusTipoMaquinaria.val() == estatus.ACTIVO ? true : false
            }
        }

        function valid() {
            var state = true;
            if (!txtModaldescripcionTipoMaquinaria.valid()) { state = false; }
            if (!cboModalEstatusMaquinaria.valid()) { state = false; }
            return state;
        }

        function saveOrUpdate(obj) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/CatTipos/SaveOrUpdate_TipoMaquinaria',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({obj:obj, Actualizacion: Actualizacion }),
                success: function (response) {
                    modalAlta.modal('hide');
                    ConfirmacionGeneral("Confirmación", response.message, "bg-green");
                    reset();
                    if (Actualizacion == 1)
                    {
                        resetFiltros()
                    }
                    else {
                        filtrarGrid();
                    }
                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        function resetFiltros() {
            cboEstatusTipoMaquinaria.val('1');
            txtDescripcionTipoMaquinaria.val('');
            grid_TipoMaquinaria.bootgrid('clear');
        }

        function initGrid() {
            grid_TipoMaquinaria.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {

                    "update": function (column, row) {
                        return "<button type='button' class='btn btn-warning modificar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                                        "<span class='glyphicon glyphicon-edit '></span> " +
                                   " </button>"
                        ;
                    },
                    "delete": function (column, row) {
                        return "<button type='button' class='btn btn-danger eliminar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                                       "<span class='glyphicon glyphicon-remove'></span> " +
                                  " </button>"
                        ;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                grid_TipoMaquinaria.find(".modificar").on("click", function (e) {
                    Actualizacion = 2;
                    idTipoMaquinaria = $(this).attr("data-index");
                    txtModaldescripcionTipoMaquinaria.val($(this).attr("data-descrip"));
                    cboModalEstatusMaquinaria.val($(this).attr("data-estatus") == "ACTIVO" ? "1" : "0");
                    update();
                });

                grid_TipoMaquinaria.find(".eliminar").on("click", function (e) {
                    var estado = $(this).attr("data-estatus");
                    if (estado == "ACTIVO") {
                        Actualizacion = 3;
                        idTipoMaquinaria = $(this).attr("data-index");
                        txtModaldescripcionTipoMaquinaria.val($(this).attr("data-descrip"));
                        cboModalEstatusMaquinaria.val("0");
                        ConfirmacionEliminacion("Dar baja registro", "¿Esta seguro que desea dar de baja este registro? " + $(this).attr("data-descrip"));
                    }
                    else {
                        reset();
                    }

                });
            });
        }

       
        function reset() {
            idTipoMaquinaria = 0;
            txtModaldescripcionTipoMaquinaria.val('');
            cboModalEstatusMaquinaria.val('1');
            formTipoMaquinaria.validate().resetForm();
        }

       // filtrarGrid();
        init();
    };

    $(document).ready(function () {
        maquinaria.catalogo.tipoMaqinaria = new tipoMaquinaria();
    });
})();


