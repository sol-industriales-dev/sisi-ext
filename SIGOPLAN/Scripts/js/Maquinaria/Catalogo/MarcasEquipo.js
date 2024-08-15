(function () {

    $.namespace('maquinaria.catalogo.MarcaEquipo');

    marcaEquipo = function () {
        idMarcaEquipo = 0,
            Actualizacion = 1;
        ruta = '/CatMarcasEquipo/FillGrid_MarcaEquipo';
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

        //Modal
        formMarcaEquipo = $("#frmMarcaEquipo"),
            txtModaldescripcionMarcaEquipo = $("#txtModaldescripcionMarcaEquipo"),
            cboModalEstatusMarcaEquipo = $("#cboModalEstatusMarcaEquipo"),
            cboModalGrupo = $("#cboModalGrupo"),

            //Pantalla Principal
            cboFiltroGrupoEquipo = $("#cboFiltroGrupoEquipo"),
            txtFiltroDescripcionMarcaEquipo = $("#txtFiltroDescripcionMarcaEquipo"),
            cboFiltroEstatusMarcaEquipo = $("#cboFiltroEstatusMarcaEquipo"),
            //Acciones
            btnBuscar = $("#btnBuscar_MarcaEquipo"),
            btnNuevo = $("#btnNuevo_MarcaEquipo"),
            btnGuardar = $("#btnModalGuardar_MarcaEquipo"),
            btnCancelar = $("#btnModalCancelar_MarcaEquipo"),
            modalAlta = $("#modalMarcaEquipo"),
            tituloModal = $("#title-modal"),
            gridResultados = $("#grid_MarcaEquipo");

        $(document).on('click', "#btnModalEliminar", function () {
            beforeSaveOrUpdate();
            reset();
        });

        function init() {
            btnNuevo.click(openModal);
            btnGuardar.click(guardar);
            btnCancelar.click(reset);
            btnBuscar.click(clickBuscar);
            cboFiltroGrupoEquipo.fillCombo('/CatMarcasEquipo/FillCboGrupoMaquinaria', { estatus: true });
            cboModalGrupo.fillCombo('/CatMarcasEquipo/FillCboGrupoMaquinaria', { estatus: true });
            initGrid();
        }

        function clickBuscar() {
            filtrarGrid();
        }

        function openModal() {
            reset();
            tituloModal.text("Alta Marcas de Equipos");
            cboModalEstatusMarcaEquipo.prop('disabled', true);
            modalAlta.modal('show');
        }
        function update() {
            tituloModal.text("Actualizar Tipo de Maquinaria");
            cboModalEstatusMarcaEquipo.prop('disabled', false);
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
                id: idMarcaEquipo,
                grupoEquipoID: cboModalGrupo.val(),
                descripcion: txtModaldescripcionMarcaEquipo.val().trim(),
                Estatus: cboModalEstatusMarcaEquipo.val() == estatus.ACTIVO ? true : false
            }
        }

        function filtrarGrid() {
            loadGrid(getFiltrosObject(), ruta, gridResultados);
        }

        function getFiltrosObject() {
            return {
                Id: cboFiltroGrupoEquipo.val(),
                grupoEquipoID: cboFiltroGrupoEquipo.val(),
                descripcion: txtFiltroDescripcionMarcaEquipo.val().trim(),
                Estatus: cboFiltroEstatusMarcaEquipo.val() == estatus.ACTIVO ? true : false
            }
        }

        function valid() {
            var state = true;
            if (!txtModaldescripcionMarcaEquipo.valid()) { state = false; }
            if (!cboModalGrupo.valid()) { state = false; }
            if (!cboModalEstatusMarcaEquipo.valid()) { state = false; }
            return state;
        }

        function saveOrUpdate(obj) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/CatMarcasEquipo/SaveOrUpdate_MarcaEquipo',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ obj: obj, Actualizacion: Actualizacion }),
                success: function (response) {
                    modalAlta.modal('hide');
                    ConfirmacionGeneral("Confirmación", response.message, "bg-green");
                    if (Actualizacion == 1) {
                        resetFiltros();
                    }
                    else {
                        filtrarGrid();
                    }
                    reset();


                    gridResultados.bootgrid('reload');
                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function resetFiltros() {
            cboFiltroEstatusMarcaEquipo.val('1');
            txtFiltroDescripcionMarcaEquipo.val('');
            cboFiltroGrupoEquipo.val('');
            gridResultados.bootgrid('clear');
        }

        function initGrid() {
            gridResultados.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {

                    "update": function (column, row) {
                        return "<button type='button' class='btn btn-warning modificar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' data-idGrupo='" + row.idGrupo + "'>" +
                            "<span class='glyphicon glyphicon-edit '></span> " +
                            " </button>"
                            ;
                    },
                    "delete": function (column, row) {
                        return "<button type='button' class='btn btn-danger eliminar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' data-idGrupo='" + row.idGrupo + "'>" +
                            "<span class='glyphicon glyphicon-remove'></span> " +
                            " </button>"
                            ;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                gridResultados.find(".modificar").on("click", function (e) {
                    idMarcaEquipo = $(this).attr("data-index");
                    txtModaldescripcionMarcaEquipo.val($(this).attr("data-descrip"));
                    cboModalGrupo.val($(this).attr("data-idGrupo"));
                    cboModalEstatusMarcaEquipo.val($(this).attr("data-estatus") == "ACTIVO" ? "1" : "0");
                    Actualizacion = 2;
                    update();
                });

                gridResultados.find(".eliminar").on("click", function (e) {
                    var estado = $(this).attr("data-estatus");
                    if (estado == "ACTIVO") {
                        idMarcaEquipo = $(this).attr("data-index");
                        Actualizacion = 3;
                        txtModaldescripcionMarcaEquipo.val($(this).attr("data-descrip"));
                        cboModalGrupo.val($(this).attr("data-idGrupo"));
                        cboModalEstatusMarcaEquipo.val("0");
                        ConfirmacionEliminacion("Dar baja registro", "¿Esta seguro que desea dar de baja este registro? " + $(this).attr("data-descrip"));
                    }
                    else {
                        reset();
                    }

                });
            });
        }

        function reset() {
            idMarcaEquipo = 0;
            Actualizacion = 1;
            cboModalGrupo.val('');
            txtModaldescripcionMarcaEquipo.val('');
            cboModalEstatusMarcaEquipo.val('1');
            formMarcaEquipo.validate().resetForm();
        }
        //    filtrarGrid();
        init();

    };

    $(document).ready(function () {
        maquinaria.catalogo.marcaEquipo = new marcaEquipo();
    });
})();
