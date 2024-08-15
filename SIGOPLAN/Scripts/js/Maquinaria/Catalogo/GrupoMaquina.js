(function () {

    $.namespace('maquinaria.catalogo.grupoMaquinaria');

    grupoMaquinaria = function () {
        idGrupoMaquinaria = 0,
            ruta = '/CatGrupos/FillGrid_TipoMaquinaria',
            Actualizacion = 1;
        noEconomico = 1,
            estatus = {
                ACTIVO: '1',
                INACTIVO: '0'
            };
        mensajes = {
            NOMBRE: 'Grupo Maquinaria',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        //Selectores de modal
        formTipoMaquinaria = $("#frmGrupoMaquinaria");
        txtModaldescripcion = $("#txtModaldescripcionGrupoMaquinaria");
        cboModalTipoMaquinaria = $("#cboModalTipoMaquinaria");
        cboModalEstatusMaquinaria = $("#cboModalEstatusGrupoMaquinaria");
        txtModalPrefijo = $("#txtModalPrefijo");
        const inputDN = $("#inputDN");
        const inputSOS = $("#inputSOS");
        const inputBitacora = $("#inputBitacora");

        //Selectores en pantalla principal
        cboFiltroTipoMaquinaria = $("#cboFiltroTipoMaquinaria");
        txtDescripcionGrupoMaquinaria = $("#txtDescripcionGrupoMaquinaria");
        cboFiltroEstatus = $("#cboEstatusGrupoMaquinaria");
        btnBuscar = $("#btnBuscar_GrupoMaquinaria");
        btnNuevo = $("#btnNuevo_GrupoMaquinaria");
        gridGrupoMaquinaria = $("#grid_GrupoMaquinaria");
        btnGuardar = $("#btnModalGuardar_GrupoMaquinaria");
        btnCancelar = $("#btnModalCancelar_GrupoMaquinaria");
        modalAlta = $("#modalAltaGrupoMaquina");
        tituloModal = $("#title-modal");

        $(document).on('click', "#btnModalEliminar", function () {
            beforeSaveOrUpdate();
            reset();
        });

        function init() {
            txtModaldescripcion.addClass('required').attr('maxlength', 100);
            txtDescripcionGrupoMaquinaria.attr('maxlength', 100);
            txtModalPrefijo.attr('maxlength', 5);
            cboFiltroTipoMaquinaria.fillCombo('/CatGrupos/FillCboTipoMaquinaria', { estatus: true });
            cboModalTipoMaquinaria.fillCombo('/CatGrupos/FillCboTipoMaquinaria', { estatus: true });
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
            tituloModal.text("Alta Grupo de Maquinaria");
            cboModalEstatusMaquinaria.prop('disabled', true);
            modalAlta.modal('show');
        }
        function update() {
            tituloModal.text("Actualizar Grupo de Maquinaria");
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
                id: idGrupoMaquinaria,
                descripcion: txtModaldescripcion.val().trim(),
                tipoEquipoID: cboModalTipoMaquinaria.val(),
                prefijo: txtModalPrefijo.val(),
                Estatus: cboModalEstatusMaquinaria.val() == estatus.ACTIVO ? true : false,
                noEco: noEconomico,
                dn: inputDN.is(':checked'),
                sos: inputSOS.is(':checked'),
                bitacora: inputBitacora.is(':checked')
            }
        }

        function valid() {
            var state = true;
            if (!txtModalPrefijo.valid()) { state = false; }
            if (!cboModalTipoMaquinaria.valid()) { state = false; }
            if (!txtModaldescripcion.valid()) { state = false; }
            if (!cboModalEstatusMaquinaria.valid()) { state = false; }
            return state;
        }

        function saveOrUpdate(obj) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/CatGrupos/SaveOrUpdate_GrupoMaquinaria',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ obj: obj, Actualizacion: Actualizacion }),
                success: function (response) {
                    modalAlta.modal('hide');
                    if (Actualizacion == 1) {
                        resetFiltros();
                    }
                    else {
                        filtrarGrid();
                    }
                    ConfirmacionGeneral("Confirmación", response.message, "bg-green");
                    reset();
                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function resetFiltros() {
            cboFiltroEstatus.val('1');
            txtDescripcionGrupoMaquinaria.val('');
            cboFiltroTipoMaquinaria.val('');
            gridGrupoMaquinaria.bootgrid('clear');
        }

        function initGrid() {
            gridGrupoMaquinaria.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {

                    "update": function (column, row) {
                        return `<button type='button' class='btn btn-warning modificar' 
                                data-dn='${row.dn}' 
                                data-index='${row.id}'  
                                data-descripcion='${row.descripcion}' 
                                data-estatus='${getEstatus(row.estatus)}' 
                                data-tipo='${row.idTipo}' 
                                data-prefijo='${row.prefijo}' 
                                data-economico='${row.noEco}'
                                data-sos='${row.sos}'
                                data-bitacora='${row.bitacora}' >
                            <span class='glyphicon glyphicon-edit'></span>
                             </button>`;
                    },
                    "delete": function (column, row) {
                        return "<button type='button' class='btn btn-danger eliminar'  data-index='" + row.id + "'" + "data-descripcion='" + row.descripcion + "' data-estatus='" + getEstatus(row.estatus) + "'data-tipo='" + row.idTipo + "' data-prefijo='" + row.prefijo + "' data-economico='" + row.noEco + "' >" +
                            "<span class='glyphicon glyphicon-remove'></span> " +
                            " </button>"
                            ;
                    },
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                gridGrupoMaquinaria.find(".modificar").on("click", function (e) {
                    idGrupoMaquinaria = $(this).attr("data-index");
                    noEconomico = $(this).attr("data-noEco");
                    txtModaldescripcion.val($(this).attr("data-descripcion"));
                    cboModalTipoMaquinaria.val($(this).attr("data-tipo"));
                    cboModalEstatusMaquinaria.val($(this).attr("data-estatus"));
                    txtModalPrefijo.val($(this).attr("data-prefijo"));
                    inputDN.prop('checked', $(this).attr("data-dn") == "false" ? false : true);
                    inputSOS.prop('checked', $(this).attr("data-sos") == "false" ? false : true);
                    inputBitacora.prop('checked', $(this).attr("data-bitacora") == "false" ? false : true);
                    Actualizacion = 2;
                    update();

                });

                gridGrupoMaquinaria.find(".eliminar").on("click", function (e) {
                    var estado = $(this).attr("data-estatus");
                    if (estado == "1") {
                        Actualizacion = 3;
                        idGrupoMaquinaria = $(this).attr("data-index");
                        noEconomico = $(this).attr("data-noEco");
                        txtModaldescripcion.val($(this).attr("data-descripcion"));
                        cboModalTipoMaquinaria.val($(this).attr("data-tipo"));
                        txtModalPrefijo.val($(this).attr("data-prefijo"));
                        cboModalEstatusMaquinaria.val("0");

                        ConfirmacionEliminacion("Dar baja registro", "¿Esta seguro que desea dar de baja este registro? " + $(this).attr("data-descripcion"));
                    }
                    else {
                        reset();
                    }

                });
            });
        }

        function reset() {
            idGrupoMaquinaria = 0;
            Actualizacion = 1;
            noEconomico = 1;
            txtModaldescripcion.val('');
            cboModalTipoMaquinaria.val('');
            cboModalEstatusMaquinaria.val('1');
            txtModalPrefijo.val('');
        }

        function filtrarGrid() {
            loadGrid(getFiltrosObject(), ruta, gridGrupoMaquinaria);
        }

        function getFiltrosObject() {
            return {
                id: 0,
                descripcion: txtDescripcionGrupoMaquinaria.val().trim(),
                tipoEquipoID: cboFiltroTipoMaquinaria.val(),
                prefijo: "",
                Estatus: cboFiltroEstatus.val() == estatus.ACTIVO ? true : false,
                noEco: 0
            }
        }

        // filtrarGrid();
        init();

    };

    $(document).ready(function () {
        maquinaria.catalogo.grupoMaquinaria = new grupoMaquinaria();
    });
})();

