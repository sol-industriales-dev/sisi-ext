(function () {

    $.namespace('maquinaria.overhaul.marcas');

    marcas = function () {
        idLocacion = 0;
        mensajes = {
            NOMBRE: 'Componentes',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        txtFiltroDescripcionMarca = $("#txtFiltroDescripcionMarca"),
        cboFiltroEstatusMarca = $("#cboFiltroEstatusMarca"),
        btnBuscarMarcas = $("#btnBuscarMarcas"),
        btnNuevaMarca = $("#btnNuevaMarca"),
        gridMarcas = $("#gridMarcas"),
        modalMarcas = $("#modalMarcas"),
        titleModal = $("#title-modal"),
        frmMarca = $("#frmMarca"),
        txtDescripcionMarca = $("#txtDescripcionMarca"),
        cboEstatusMarca = $("#cboEstatusMarca"),
        btnModalGuardarMarca = $("#btnModalGuardarMarca"),
        btnModalCancelarMarca = $("#btnModalCancelarMarca");


        function init() {
            initGrid();
            cargarGrid();
            btnNuevaMarca.click(nuevaMarca);
            btnModalGuardarMarca.click(guardarMarca);
            btnBuscarMarcas.click(cargarGrid);
            txtFiltroDescripcionMarca.change(cargarGrid);
            cboFiltroEstatusMarca.change(cargarGrid);

            $(document).on('click', "#btnModalEliminar", function () {
                eliminarLocacion();
                reset();
            });
        }

        function initGrid() {
            gridMarcas.bootgrid({
                headerCssClass: '.bg-table-header',
                rowCount: -1,
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "update": function (column, row) {
                        return "<button type='button' class='btn btn-warning modificar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "' data-estatus='" + row.estatus + "'>" +
                        "<span class='glyphicon glyphicon-edit '></span></button>";
                    },
                    "delete": function (column, row) {
                        return "<button type='button' class='btn btn-danger eliminar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "' data-estatus='" + row.estatus + "'>" +
                        "<span class='glyphicon glyphicon-remove'></span></button>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                gridMarcas.find(".modificar").parent().css("width", "3%");
                gridMarcas.find(".eliminar").parent().css("width", "3%");
                gridMarcas.find(".modificar").parent().css("text-align", "center");
                gridMarcas.find(".eliminar").parent().css("text-align", "center");
                gridMarcas.find(".modificar").on("click", function (e) {
                    idLocacion = $(this).attr("data-index");
                    txtDescripcionMarca.val($(this).attr("data-descrip"));

                    titleModal.text("Actualizar Tipo de Maquinaria");
                    cboEstatusMarca.val($(this).attr("data-estatus") == "true" ? "1" : "0").prop('disabled', false);
                    modalMarcas.modal('show');
                });

                gridMarcas.find(".eliminar").on("click", function (e) {
                    idLocacion = $(this).attr("data-index");
                    ConfirmacionEliminacion("Eliminar Locación", "¿Esta seguro que desea dar de baja la marca \"" + $(this).attr("data-descrip") + "\"?");
                });
            });
        }

        function nuevaMarca() {
            cboEstatusMarca.prop('disabled', true);
            modalMarcas.modal('show');
        }

        function guardarMarca() {
            if (valid()) {
                altaUpdateMarca(getElementosLocacion());
            }
        }

        function valid() {
            var state = true;
            if (txtDescripcionMarca.val() == "" || !txtDescripcionMarca.valid()) { state = false; }
            if (cboEstatusMarca == "") { state = false; }
            return state;
        }

        function altaUpdateMarca(obj1) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/AltaUpdateMarca',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ marca: obj1 }),
                success: function (response) {
                    $.unblockUI();
                    modalMarcas.modal('hide');
                    reset();
                    ConfirmacionGeneral("Confirmación", "Se guardó la Marca", "bg-green");                
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);                    
                }
            });
        }

        function getElementosLocacion() {
            return {
                id: idLocacion,
                descripcion: txtDescripcionMarca.val().trim(),
                estatus: cboEstatusMarca.val() == "1" ? true : false
            }
        }

        function reset() {
            idLocacion = 0;
            titleModal.text("");
            txtDescripcionMarca.text("");
            txtFiltroDescripcionMarca.text("");
            cboEstatusMarca.val("1");
            cboFiltroEstatusMarca.val("1");
            cargarGrid();
        }

        function cargarGrid() {
            $.blockUI({ message: "Procesando..." });
            estatus = cboFiltroEstatusMarca.val() == "1" ? true : false;
            descripcion = txtFiltroDescripcionMarca.val().trim();
            $.ajax({
                url: "/Overhaul/CargarMarcasComponentes",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ estatus: estatus, descripcion: descripcion }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        gridMarcas.bootgrid({
                            templates: {
                                header: ""
                            }
                        });
                        gridMarcas.bootgrid("clear");
                        gridMarcas.bootgrid("append", response.rows);
                        gridMarcas.bootgrid('reload');
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

        function eliminarLocacion() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/BajaMarcaComponente',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ idLocacion: idLocacion }),
                success: function (response) {
                    $.unblockUI();
                    modalAcciones.modal('hide');
                    ConfirmacionGeneral("Confirmación", "Se eliminó la Locación", "bg-green");                  
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);                    
                }
            });
        }

        init();

    };

    $(document).ready(function () {
        maquinaria.overhaul.marcas = new marcas();
    });
})();


