(function () {

    $.namespace('maquinaria.catalogo.conjunto');

    conjunto = function () {
        idConjunto = 0,
        ruta = '/CatConjuntos/FillGrid_Conjunto';
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'

        };
        Actualizacion = 1;
        mensajes = {
            NOMBRE: 'Conjunto',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        frmModal = $("#frmConjunto"),
        txtModaldescripcion = $("#txtModaldescripcionConjunto"),
        cboModalEstatus = $("#cboModalEstatusConjunto"),
        txtModalPrefijo = $("#txtModalPrefijo"),
        

        cboFiltroEstatus = $("#cboFiltroEstatusConjunto"),
        txtFiltroDescripcion = $("#txtFiltroDescripcionConjunto"),

        btnBuscar = $("#btnBuscar_Conjunto"),
        btnNuevo = $("#btnNuevo_Conjunto"),
        btnGuardar = $("#btnModalGuardar_Conjunto"),
        btnCancelar = $("#btnModalCancelar_Conjunto"),
        modalAcciones = $("#modalConjunto"),
        tituloModal = $("#title-modal"),
        gridResultado = $("#grid_Conjunto");

        $(document).on('click', "#btnModalEliminar", function () {
            beforeSaveOrUpdate();
            reset();
        });

        function init() {
            txtModaldescripcion.addClass('required').attr('maxlength', 100);
            txtModalPrefijo.addClass('required').attr('maxlength', 5);
            txtFiltroDescripcion.attr('maxlength', 100);
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
            tituloModal.text("Alta Conjunto");
            reset();
            cboModalEstatus.prop('disabled', true);
            modalAcciones.modal('show');
        }
        function update() {
            tituloModal.text("Actualizar Tipo de Maquinaria");
            cboModalEstatus.prop('disabled', false);
            modalAcciones.modal('show');
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
                Id: idConjunto,
                descripcion: txtModaldescripcion.val().trim(),
                prefijo: "",// txtModalPrefijo.val(),
                Estatus: cboModalEstatus.val() == estatus.ACTIVO ? true : false
            }
        }

        function filtrarGrid() {
            loadGrid(getFiltrosObject(), ruta, gridResultado);
        }

        function getFiltrosObject() {
            return {
                Id: 0,
                descripcion: txtFiltroDescripcion.val().trim(),
                prefijo: "",
                Estatus: cboFiltroEstatus.val() == estatus.ACTIVO ? true : false
            }
        }

        function valid() {
            var state = true;
            if (!txtModaldescripcion.valid()) { state = false; }
            if (!cboModalEstatus.valid()) { state = false; }
            if (!txtModalPrefijo.valid()) { state = false; }
            return state;
        }

        function saveOrUpdate(obj) {
            $.blockUI({ message: mensajes.PROCESANDO });

            $.ajax({
                url: '/CatConjuntos/SaveOrUpdate_Conjunto',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ obj: obj, Actualizacion: Actualizacion }),
                success: function (response) {
                    modalAcciones.modal('hide');
                    ConfirmacionGeneral("Confirmación", response.message, "bg-green");
                    if (Actualizacion == 1)
                    {
                        resetFiltros();
                    }
                    else {
                        filtrarGrid();
                    }
                    reset();
                    gridResultado.bootgrid('reload');
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
            txtFiltroDescripcion.val('');
            gridResultado.bootgrid('clear');
        }
        
        function initGrid() {
            gridResultado.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "update": function (column, row) {
                        return "<button type='button' class='btn btn-warning modificar' data-prefijo='" + row.prefijo + "' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "'>" +
                                        "<span class='glyphicon glyphicon-edit '></span> " +
                                   " </button>"
                        ;
                    },
                    "delete": function (column, row) {
                        return "<button type='button' class='btn btn-danger eliminar' data-prefijo='" + row.prefijo + "' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "'>" +
                                       "<span class='glyphicon glyphicon-remove'></span> " +
                                  " </button>"
                        ;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                gridResultado.find(".modificar").on("click", function (e) {
                    Actualizacion = 2;
                    idConjunto = $(this).attr("data-index");
                    txtModaldescripcion.val($(this).attr("data-descrip"));
                    txtModalPrefijo.val($(this).attr("data-prefijo"));
                    var estado = $(this).attr("data-estatus") == "ACTIVO" ? "1" : "0";
                    cboModalEstatus.val(estado);
                    update();
                });

                gridResultado.find(".eliminar").on("click", function (e) {
                    var estado = $(this).attr("data-estatus");
                    if (estado == "ACTIVO") {
                        Actualizacion = 3;
                        idConjunto = $(this).attr("data-index");
                        txtModaldescripcion.val($(this).attr("data-descrip"));
                        txtModalPrefijo.val($(this).attr("data-prefijo"));
                        cboModalEstatus.val("0");
                        ConfirmacionEliminacion("Dar baja registro", "¿Esta seguro que desea dar de baja este registro? "+ $(this).attr("data-descrip"));
                    }
                    else {
                        reset();
                    }

                });
            });
        }

        function reset() {
            idConjunto = 0;
            Actualizacion = 1;
            txtModaldescripcion.val('');
            cboModalEstatus.val('1');
            txtModalPrefijo.val('');
            frmModal.validate().resetForm();
        }
        // filtrarGrid();
        init();

    };

    $(document).ready(function () {
        maquinaria.catalogo.conjunto = new conjunto();
    });
})();


