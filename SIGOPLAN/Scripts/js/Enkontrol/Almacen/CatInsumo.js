(function () {

    $.namespace('compras.catalogo.grupoInsumo');

    grupoInsumo = function () {
        id = 0,
        Actualizacion = 1,
        ruta = '/Enkontrol/Almacen/FillGrid_Insumo';
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'

        };
        mensajes = {
            NOMBRE: 'grupo ',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        form = $("#frm"),
        txtModaltipo = $("#txtModalTipo"),
        txttipo = $("#txtTipo"),
        txtModalgrupo = $("#txtModalGrupo"),
        txtgrupo = $("#txtGrupo"),
        txtModaldescripcion = $("#txtModalDescripcion"),
        txtDescripcion = $("#txtDescripcion"),
        cboEstatus = $("#cboEstatus"),
        grid = $("#grid"),
        btnBuscar = $("#btnBuscar"),
        btnNuevo = $("#btnNuevo"),
        btnGuardar = $("#btnModalGuardar"),
        btnCancelar = $("#btnModalCancelar"),
        modalAlta = $("#modalAlta"),
        tituloModal = $("#title-modal"),
        cboModalEstatus = $("#cboModalEstatus"),
        grid = $("#grid");

        $(document).on('click', "#btnModalEliminar", function () {
            beforeSaveOrUpdate();
            reset();
        });

        function init() {
            txtModaltipo.fillCombo('/Enkontrol/Almacen/FillCboInsumoTipo', { estatus: true });
            txttipo.fillCombo('/Enkontrol/Almacen/FillCboInsumoTipo', { estatus: true });
            txtModalgrupo.fillCombo('/Enkontrol/Almacen/FillCboInsumoGrupo', { estatus: true });
            txtgrupo.fillCombo('/Enkontrol/Almacen/FillCboInsumoGrupo', { estatus: true });
            txtModaldescripcion.addClass('required').attr('maxlength', 100);
            txtDescripcion.attr('maxlength', 100);
            btnNuevo.click(openModal);
            btnGuardar.click(guardar);
            btnCancelar.click(reset);
            btnBuscar.click(clickBuscar);
            initGrid();
            filtrarGrid();
        }

        function clickBuscar() {
            filtrarGrid();
        }

        function openModal() {
            reset();
            tituloModal.text("Alta de Insumos");
            cboModalEstatus.prop('disabled', true);
            modalAlta.modal('show');
        }
        function update() {
            
            tituloModal.text("Actualizar Insumos");
            cboModalEstatus.prop('disabled', false);
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
                Id: id,
                tipo: txtModaltipo.val(),
                grupo: txtModalgrupo.val(),
                descripcion: txtModaldescripcion.val().trim().toUpperCase(),
                Estatus: cboModalEstatus.val() == estatus.ACTIVO ? true : false
            }
        }
        function filtrarGrid() {
            loadGrid(getFiltrosObject(), ruta, grid);
        }

        function getFiltrosObject() {
            return {
                Id: 0,
                tipo: txttipo.val() == '' ? 0 : txttipo.val(),
                grupo: txtgrupo.val() == '' ? 0 : txtgrupo.val(),
                descripcion: txtDescripcion.val().trim(),
                Estatus: cboEstatus.val() == estatus.ACTIVO ? true : false
            }
        }

        function valid() {
            var state = true;
            if (!txtModaldescripcion.valid()) { state = false; }
            if (!cboModalEstatus.valid()) { state = false; }
            return state;
        }

        function saveOrUpdate(obj) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Enkontrol/Almacen/SaveOrUpdate_Insumo',
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
            cboEstatus.val('1');
            // txtDescripcion.val('');
            grid.bootgrid('clear');
        }

        function initGrid() {
            grid.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {

                    "update": function (column, row) {
                        return "<button type='button' class='btn btn-warning modificar' data-index='" + row.id + "'"
                                + " data-descripcion='" + row.descripcion + "'"
                                + " data-estatus='" + row.estatus + "'"
                                + " data-tipo='" + row.tipo + "'"
                                + " data-grupo='" + row.grupo + "' >"
                                + "<span class='glyphicon glyphicon-edit'></span> "
                                + "</button>";
                    },
                    "delete": function (column, row) {
                        return "<button type='button' class='btn btn-danger eliminar' data-index='" + row.id + "'"
                                + " data-descripcion='" + row.descripcion + "'"
                                + " data-estatus='" + row.estatus + "'"
                                + " data-tipo='" + row.tipo + "'"
                                + " data-grupo='" + row.grupo + "' >"
                                + "<span class='glyphicon glyphicon-remove'></span> "
                                + "</button>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                grid.find(".modificar").on("click", function (e) {
                    Actualizacion = 2;
                    id = $(this).attr("data-index");
                    txtModaldescripcion.val($(this).attr("data-descripcion"));
                    txtModaltipo.val($(this).attr("data-tipo"));
                    txtModalgrupo.val($(this).attr("data-grupo"));
                    cboModalEstatus.val($(this).attr("data-estatus") == "ACTIVO" ? "1" : "0");
                    update();
                });

                grid.find(".eliminar").on("click", function (e) {
                    var estado = $(this).attr("data-estatus");
                    if (estado == "ACTIVO") {
                        Actualizacion = 3;
                        id = $(this).attr("data-index");
                        txtModaldescripcion.val($(this).attr("data-descripcion"));
                        cboModalEstatus.val("0");
                        ConfirmacionEliminacion("Dar baja registro", "¿Esta seguro que desea dar de baja este registro? " + $(this).attr("data-descrip"));
                    }
                    else {
                        reset();
                    }

                });
            });
        }

       
        function reset() {
            id = 0;
            txtModaltipo.val('');
            txtModalgrupo.val('');
            txtModaldescripcion.val('');
            cboModalEstatus.val('1');
            form.validate().resetForm();
        }

       // filtrarGrid();
        init();
    };

    $(document).ready(function () {
        compras.catalogo.grupoInsumo = new grupoInsumo();
    });
})();


