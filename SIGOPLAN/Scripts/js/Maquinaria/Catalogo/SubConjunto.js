(function () {

    $.namespace('maquinaria.catalogo.subConjunto');

    subConjunto = function () {
        contadorNumParte = 0,
        idSubConjunto = 0,
        Actualizacion = 1,
        ruta = '/CatSubConjunto/FillGrid_SubConjunto';
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'

        };
        mensajes = {
            NOMBRE: 'Sub-Conjunto',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        frmModal = $("#frmSubConjunto"),
        cboModalConjunto = $("#cboModalConjunto"),
        txtModaldescripcion = $("#txtModaldescripcionSubConjunto"),
        cboModalPosiciones = $("#cboModalPosicion"),
        cboModalEstatus = $("#cboModalEstatusSubConjunto"),
        txtModalPrefijo = $("#txtModalPrefijo"),
        cboFiltroConjunto = $("#cboFiltroConjunto"),
        cboFiltroEstatus = $("#cboFiltroSubConjunto"),
        txtFiltroDescripcion = $("#txtFiltroDescripcionSubConjunto"),

        btnBuscar = $("#btnBuscar"),
        btnNuevo = $("#btnNuevo_SubConjunto"),
        btnGuardar = $("#btnModalGuardar_SubConjunto"),
        btnCancelar = $("#btnModalCancelar_SubConjunto"),

        modalAcciones = $("#modalSubConjunto"),
        tituloModal = $("#title-modal"),
        gridResultado = $("#grid_SubConjunto");
        //gridNumParte = $("#gridNumParte"),
        //btnAgregarNumParte = $("#btnAgregarNumParte"),
        //txtModalNumParte = $("#txtModalNumParte");

        $(document).on('click', "#btnModalEliminar", function () {
            beforeSaveOrUpdate();
            reset();
        });

        function init() {
            txtModaldescripcion.addClass('required').attr('maxlength', 100);
            txtModalPrefijo.addClass('required');
            txtModalPrefijo.attr('maxlength', 5);
            cboModalConjunto.addClass('required');

            cboModalPosiciones.fillCombo('/CatSubConjunto/FillCbo_Posiciones', { estatus: true }, true);
            convertToMultiselectSelectAll(cboModalPosiciones);
            cboFiltroConjunto.fillCombo('/CatSubConjunto/FillCbo_Conjunto', { estatus: true });
            cboModalConjunto.fillCombo('/CatSubConjunto/FillCbo_Conjunto', { estatus: true });

            txtFiltroDescripcion.attr('maxlength', 100);
            
            btnNuevo.click(openModal);
            btnGuardar.click(guardar);
            btnCancelar.click(reset);
            btnBuscar.click(clickBuscar);
            //btnAgregarNumParte.click(agregarNumParte);

            initGrid();
            //initGridNumParte();
        }

        function clickBuscar() {
            filtrarGrid();
        }

        function openModal() {
            tituloModal.text("Alta Sub-Conjunto");
            reset();
            cboModalEstatus.prop('disabled', true);
            modalAcciones.modal('show');
        }
        function update() {
            tituloModal.text("Actualizar Sub-Conjunto");
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
            //var tabla = gridNumParte.bootgrid("getCurrentRows");
            //var data = "";
            //for (var i = 0; i < contadorNumParte; i++) {
            //    data += tabla[i]['numParte'] + "/";
            //}

            return {
                id: idSubConjunto,
                descripcion: txtModaldescripcion.val().trim(),
                conjuntoID: cboModalConjunto.val(),
                posicionID: cboModalPosiciones.val().join(','),
                prefijo : txtModalPrefijo.val(),
                hasPosicion: cboModalPosiciones.val() > 0 ? true : false,
                estatus: cboModalEstatus.val() == estatus.ACTIVO ? true : false,
                //numParte: data
            }
        }

        function filtrarGrid() {
            loadGrid(getFiltrosObject(), ruta, gridResultado);
        }

        function getFiltrosObject() {
            return {
                Id: 0,
                descripcion: txtFiltroDescripcion.val().trim(),
                conjuntoID: cboFiltroConjunto.val(),
                posicionID: 0,
                hasPosicion: false,
                prefijo: "",
                Estatus: cboFiltroEstatus.val() == estatus.ACTIVO ? true : false
            }
        }

        function valid() {
            var state = true;
            if (!txtModaldescripcion.valid()) { state = false; }
            if (!cboModalConjunto.valid()) { state = false; }
            if (!cboModalEstatus.valid()) { state = false; }
            return state;
        }

        function saveOrUpdate(obj) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/CatSubConjunto/SaveOrUpdate_SubConjunto',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({obj:obj, Actualizacion: Actualizacion}),
                success: function (response) {
                    modalAcciones.modal('hide');
                    ConfirmacionGeneral("Confirmación", response.message, "bg-green");
                    if (Actualizacion == 1)
                    {
                        resetFiltros()
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

        function resetFiltros() {
            cboFiltroEstatus.val('1');
            txtFiltroDescripcion.val('');
            cboFiltroConjunto.val('');
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
                        return "<button type='button' class='btn btn-warning modificar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' data-conjunto='" + row.conjuntoID + "' data-posicion='" +
                            row.idPosicion + "' data-prefijo='" + row.prefijo + "' >" +
                                        "<span class='glyphicon glyphicon-edit '></span> " +
                                   " </button>"
                        ;
                    },
                    "delete": function (column, row) {
                        return "<button type='button' class='btn btn-danger eliminar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' data-conjunto='" + row.conjuntoID + "' data-posicion='" + row.idPosicion + "' data-prefijo='" + row.prefijo + "' >" +
                                       "<span class='glyphicon glyphicon-remove'></span> " +
                                  " </button>"
                        ;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                gridResultado.find(".modificar").on("click", function (e) {
                    idSubConjunto = $(this).attr("data-index");
                    cboModalConjunto.val($(this).attr("data-conjunto"));
                    txtModaldescripcion.val($(this).attr("data-descrip"));
                    cboModalPosiciones.val($(this).attr("data-posicion"));
                    cboModalEstatus.val($(this).attr("data-estatus") == "ACTIVO" ? 1 : 0);
                    txtModalPrefijo.val($(this).attr("data-prefijo"));
                    Actualizacion = 2;
                    update();
                });

                gridResultado.find(".eliminar").on("click", function (e) {
                    var estado = $(this).attr("data-estatus");

                    if (estado == "ACTIVO") {
                        idSubConjunto = $(this).attr("data-index");
                        txtModaldescripcion.val($(this).attr("data-descrip"));
                        txtModalPrefijo.val($(this).attr("data-prefijo"));
                        cboModalEstatus.val("0");
                        cboModalConjunto.val($(this).attr("data-conjunto"));
                        Actualizacion = 3;
                        ConfirmacionEliminacion("Dar baja registro", "¿Esta seguro que desea dar de baja este registro? " + $(this).attr("data-descrip"));
                    }
                    else {
                        reset();
                    }

                });
            });
        }

        //function initGridNumParte()
        //{
        //    gridNumParte.bootgrid({
        //        headerCssClass: '.bg-table-header',
        //        align: 'center',
        //        templates: {
        //            header: ""
        //        },
        //        rowCount: -1,
        //        //formatters: {

        //        //    "update": function (column, row) {
        //        //        return "<button type='button' class='btn btn-warning modificar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' data-conjunto='" + row.conjuntoID + "' data-posicion='" +
        //        //            row.idPosicion + "' data-prefijo='" + row.prefijo + "' >" +
        //        //                        "<span class='glyphicon glyphicon-edit '></span> " +
        //        //                   " </button>"
        //        //        ;
        //        //    },
        //        //    "delete": function (column, row) {
        //        //        return "<button type='button' class='btn btn-danger eliminar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' data-conjunto='" + row.conjuntoID + "' data-posicion='" + row.idPosicion + "' data-prefijo='" + row.prefijo + "' >" +
        //        //                       "<span class='glyphicon glyphicon-remove'></span> " +
        //        //                  " </button>"
        //        //        ;
        //        //    }
        //        //}
        //    });
        //}

        //function agregarNumParte()
        //{
        //    if (txtModalNumParte.val() != "" && txtModalNumParte.valid()) {
        //        var JSONINFO = [{ "contador": contadorNumParte + 1, "numParte": txtModalNumParte.val() }];
        //        gridNumParte.bootgrid("append", JSONINFO);
        //        contadorNumParte++;
                
        //    }
        //}

        function reset() {
            idSubConjunto = 0;
            Actualizacion = 1;
            cboModalConjunto.val('');
            cboModalPosiciones.val('');
            txtModaldescripcion.val('');
            txtModalPrefijo.val('');
            cboModalEstatus.val('1');
            frmModal.validate().resetForm();
            //gridNumParte.bootgrid("clear");
            //txtModalNumParte.val("");
        }

        //filtrarGrid();
        init();

    };

    $(document).ready(function () {
        maquinaria.catalogo.subConjunto = new subConjunto();
    });
})();


