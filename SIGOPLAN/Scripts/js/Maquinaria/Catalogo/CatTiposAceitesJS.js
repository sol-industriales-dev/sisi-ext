(function () {

    $.namespace('maquinaria.catalogo.TipoAceites');

    TipoAceites = function () {

        idRegistro = 0;
        Actualizacion = 0;
        ruta = '/AceitesLubricantes/GetListaCatAceites';
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'

        };
        mensajes = {
            NOMBRE: 'Tipos de Aceites',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        tituloModal = $("#tituloModal"),
        tbFiltroDescripcion = $("#tbFiltroDescripcion"),
        cboFiltroEstatus = $("#cboFiltroEstatus"),
        btnAplicarFiltros = $("#btnAplicarFiltros"),
        btnNuevoRegistro = $("#btnNuevoRegistro"),
        tblTiposAceite = $("#tblTiposAceite"),
        modalNuevoRegistro = $("#modalNuevoRegistro"),
        frmDatosFormulario = $("#frmDatosFormulario"),
        tbRegistroDescripcion = $("#tbRegistroDescripcion"),
        cboRegistroEstatus = $("#cboRegistroEstatus"),
        btnModalGuardarRegistro = $("#btnModalGuardarRegistro"),
        btnModalCancelar = $("#btnModalCancelar");

        function init() {
            initGrid();

            tblTiposAceite.bootgrid('clear');
            tblTiposAceite.bootgrid('append', new Array());

            btnNuevoRegistro.click(openModal);
            btnModalGuardarRegistro.click(guardar);
            btnAplicarFiltros.click(clickBuscar);
        }

        function clickBuscar() {
            filtrarGrid();
        }

        function openModal() {
            reset();
            tituloModal.text("Alta Tipo de Aceite");
            cboRegistroEstatus.prop('disabled', true);
            modalNuevoRegistro.modal('show');
        }

        function update() {

            tituloModal.text("Actualizar Tipo de Aceite");
            cboRegistroEstatus.prop('disabled', false);
            modalNuevoRegistro.modal('show');
        }

        function guardar() {
            Actualizacion = 1;
            beforeSaveOrUpdate();
        }

        function beforeSaveOrUpdate() {

            if (valid()) {
                saveOrUpdate(getPlainObject());
            }
        }

        function getPlainObject() {
            return {
                id: idRegistro,
                descripcion: tbRegistroDescripcion.val().trim(),
                estatus: cboRegistroEstatus.val() == estatus.ACTIVO ? true : false
            }
        }

        function filtrarGrid() {
            loadGrid(getFiltrosObject(), ruta, tblTiposAceite);
        }

        function valid() {
            var state = true;
            if (!tbRegistroDescripcion.valid()) { state = false; }
            if (!cboRegistroEstatus.valid()) { state = false; }
            return state;
        }

        function getFiltrosObject() {
            return {
                id: 0,
                descripcion: tbFiltroDescripcion.val().trim(),
                estatus: cboFiltroEstatus.val() == estatus.ACTIVO ? true : false
            }
        }

        function resetFiltros() {
            cboFiltroEstatus.val('1');
            tbFiltroDescripcion.val('');
            tblTiposAceite.bootgrid('clear');
        }

        function saveOrUpdate(obj) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/AceitesLubricantes/SaveOrUpdate',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ obj: obj, Actualizacion: Actualizacion }),
                success: function (response) {
                    modalNuevoRegistro.modal('hide');
                    ConfirmacionGeneral("Confirmación", response.message, "bg-green");
                    reset();
                    if (Actualizacion == 1) {
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

        function initGrid() {
            tblTiposAceite.bootgrid({
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
                tblTiposAceite.find(".modificar").on("click", function (e) {
                    Actualizacion = 2;
                    idRegistro = $(this).attr("data-index");
                    tbRegistroDescripcion.val($(this).attr("data-descrip"));
                    cboRegistroEstatus.val($(this).attr("data-estatus") == "ACTIVO" ? "1" : "0");
                    update();
                });

                tblTiposAceite.find(".eliminar").on("click", function (e) {
                    var estado = $(this).attr("data-estatus");
                    if (estado == "ACTIVO") {
                        Actualizacion = 3;
                        idRegistro = $(this).attr("data-index");
                        tbRegistroDescripcion.val($(this).attr("data-descrip"));
                        cboRegistroEstatus.val("0");
                        ConfirmacionEliminacion("Dar baja registro", "¿Esta seguro que desea dar de baja este registro? " + $(this).attr("data-descrip"));
                    }
                    else {
                        reset();
                    }

                });
            });
        }

        function reset() {
            idRegistro = 0;
            tbRegistroDescripcion.val('');
            cboRegistroEstatus.val('1');
            frmDatosFormulario.validate().resetForm();
        }

        init();
    };

    $(document).ready(function () {
        maquinaria.catalogo.TipoAceites = new TipoAceites();
    });
})();


