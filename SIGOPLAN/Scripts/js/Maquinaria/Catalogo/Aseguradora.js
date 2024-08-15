(function () {

    $.namespace('maquinaria.catalogo.aseguradora');

    aseguradora = function () {
        idAseguradora = 0,
        Actualizacion = 1,
        ruta = '/CatAseguradoras/FillGrid_Aseguradora';
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Aseguradora',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        //Selectores de modal
        formAseguradora = $("#frmAseguradora"),
        txtModaldescripcion = $("#txtModaldescripcionAseguradora"),
        cboModalEstatusAseguradora = $("#cboModalEstatusAseguradora"),
        //Selectores en pantalla principal
        txtDescripcionAseguradora = $("#txtFiltroDescripcion"),
        cboFiltroEstatus = $("#cboFiltroEstatusAseguradora"),
        btnBuscar = $("#btnBuscar_Aseguradora"),
        btnNuevo = $("#btnNuevo_Aseguradora"),
        gridFiltros = $("#grid_Aseguradora"),
        btnGuardar = $("#btnModalGuardar_Aseguradora"),
        btnCancelar = $("#btnModalCancelar_Aseguradora"),
        modalAlta = $("#modalAseguradora"),
        tituloModal = $("#title-modal");

        $(document).on('click', "#btnModalEliminar", function () {
            beforeSaveOrUpdate();
            reset();
        });

        function init() {

            txtModaldescripcion.addClass('required').attr('maxlength', 100);
            txtDescripcionAseguradora.attr('maxlength', 100);

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
            tituloModal.text("Alta Aseguradora");
            cboModalEstatusAseguradora.prop('disabled', true);
            modalAlta.modal('show');
        }
        function update() {
            tituloModal.text("Actualizar Aseguradora");
            cboModalEstatusAseguradora.prop('disabled', false);
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
                id: idAseguradora,
                descripcion: txtModaldescripcion.val().trim(),
                Estatus: cboModalEstatusAseguradora.val() == estatus.ACTIVO ? true : false
            }
        }

        function filtrarGrid() {
            loadGrid(getFiltrosObject(), ruta, gridFiltros);
        }

        function getFiltrosObject() {
            return {
                Id: 0,
                descripcion: txtDescripcionAseguradora.val().trim(),
                Estatus: cboFiltroEstatus.val() == estatus.ACTIVO ? true : false
            }
        }

        function valid() {
            var state = true;
            if (!txtModaldescripcion.valid()) { state = false; }
            if (!cboModalEstatusAseguradora.valid()) { state = false; }
            return state;
        }

        function saveOrUpdate(obj) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/CatAseguradoras/SaveOrUpdate_Aseguradora',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({obj:obj,Actualizacion:Actualizacion}),
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
                    
                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        function resetFiltros()
        {
            cboFiltroEstatus.val('1');
            gridFiltros.bootgrid('clear');
            txtDescripcionAseguradora.val('');
        }

        function initGrid() {
            gridFiltros.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "update": function (column, row) {
                        return "<button type='button' class='btn btn-warning modificar' data-index='" + row.id + "'" + "data-descripcion='" + row.descripcion + "'data-estatus='" + row.estatus + "'>" +
                            "<span class='glyphicon glyphicon-edit '></span> " +
                                   " </button>"
                        ;
                    },
                    "delete": function (column, row) {
                        return "<button type='button' class='btn btn-danger eliminar'  data-index='" + row.id + "'" + "data-descripcion='" + row.descripcion + "' data-estatus='" + row.estatus + "'>" +
                            "<span class='glyphicon glyphicon-remove'></span> " +
                                  " </button>"
                        ;
                    },
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                gridFiltros.find(".modificar").on("click", function (e) {
                    idAseguradora = $(this).attr("data-index");
                    Actualizacion = 2;
                    txtModaldescripcion.val($(this).attr("data-descripcion"));
                    cboModalEstatusAseguradora.val($(this).attr("data-estatus")=="ACTIVO"?"1":"0");
                    update();

                });
                gridFiltros.find(".eliminar").on("click", function (e) {
                    var estado = $(this).attr("data-estatus");
                    if (estado == "ACTIVO") {
                        Actualizacion = 3;
                        idAseguradora = $(this).attr("data-index");
                        txtModaldescripcion.val($(this).attr("data-descripcion"));
                        cboModalEstatusAseguradora.val("0");
                        ConfirmacionEliminacion("Dar baja registro", "¿Esta seguro que desea dar de baja este registro?" + $(this).attr("data-descripcion"));
                    }
                    else {
                        reset();
                    }

                });
            });
        }

        function reset() {
            idAseguradora = 0;
            Actualizacion = 1;
            txtModaldescripcion.val('');
            cboModalEstatusAseguradora.val('1');
            formAseguradora.validate().resetForm();
        }
        //filtrarGrid();
        init();

    };

    $(document).ready(function () {
        maquinaria.catalogo.aseguradora = new aseguradora();
    });
})();

