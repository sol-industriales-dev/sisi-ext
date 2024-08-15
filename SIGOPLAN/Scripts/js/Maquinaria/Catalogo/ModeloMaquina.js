(function () {

    $.namespace('maquinaria.catalogo.marcaEquipo');

    grupoMaquinaria = function () {
        idModeloMaquina = 0,
            noComponente = 1,
            Actualizacion = 1;
        ruta = '/CatModeloEquipo/FillGrid_ModeloEquipo';

        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };

        mensajes = {
            NOMBRE: 'Modelo Equipo',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        cboConjuntoModal = $("#cboConjuntoModal"),
            cboSubConjuntoModal = $("#cboSubConjuntoModal"),
            modalAltaSubConjuntos = $("#modalAltaSubConjuntos"),
            btnAgregarSubConjunto = $("#btnAgregarSubConjunto"),
            tblArchivosDiv = $("#tblArchivosDiv"),
            modalVistaArchivo = $("#modalVistaArchivo"),
            cboModalOverhaul = $("#cboModalOverhaul"),
            grid_ArchivosVER = $("#grid_ArchivosVER"),
            grid_ArchivosModificar = $("#grid_ArchivosModificar"),
            fupAdjunto = $("#fupAdjunto"),
            //Selectores de modal
            frmModeloEquipo = $("#frmModeloEquipo"),
            txtModaldescripcion = $("#txtModaldescripcionModeloEquipo"),
            cboModalMarcas = $("#cboModalMarcas"),
            cboModalEstatusModeloEquipo = $("#cboModalEstatusModeloEquipo"),
            //txtModalNomCorto = $("#txtModalNomCorto"),
            //Selectores en pantalla principal
            cboFiltroMarcas = $("#cboFiltrosMarcaEquipo"),
            cboFiltroGrupoEquipo = $("#cboFiltroGrupoEquipo"),
            txtDescripcionModelosEquipos = $("#txtDescripcionModelosEquipo"),
            cboFiltroEstatus = $("#cboFiltroEstatusModelosEquipo"),
            btnBuscar = $("#btnBuscar_ModeloEquipo"),
            btnNuevo = $("#btnNuevo_ModeloEquipo"),
            gridResultadoFiltros = $("#grid_ModelosEquipo"),
            btnGuardar = $("#btnModalGuardar_ModelosEquipo"),
            btnCancelar = $("#btnModalCancelar_ModelosEquipo"),
            cboModalGrupo = $("#cboModalGrupo"),
            modalAlta = $("#modalModeloEquipo"),
            tituloModal = $("#title-modal"),
            cboConjunto = $("#cboConjunto"),
            cboSubconjunto = $("#cboSubconjunto"),
            btnAgregarSubconjuntoNuevo = $("#btnAgregarSubconjuntoNuevo"),
            gridSubconjuntos = $("#grid_Subconjuntos"),
            txtModalComponentesNumParte = $("#txtModalComponentesNumParte"),
            txtModalPrefijo = $("#txtModalPrefijo"),
            btnAgregarPrefijo = $("#btnAgregarPrefijo"),
            gridPrefijos = $("#gridPrefijos");
        let idPrefijo = 0;

        $(document).on('click', "#btnModalEliminar", function () {
            beforeSaveOrUpdate();
            reset();
        });

        function init() {

            txtModaldescripcion.addClass('required').attr('maxlength', 100);
            txtDescripcionModelosEquipos.attr('maxlength', 100);
            //txtModalNomCorto.addClass('required').attr('maxlength', 5);
            cboFiltroMarcas.fillCombo('/CatModeloEquipo/FillCboMarcaEquipo_ModeloEquipo', { estatus: true });
            cboModalMarcas.fillCombo('/CatModeloEquipo/FillCboMarcaEquipo_ModeloEquipo', { estatus: true });

            //  cboFiltroGrupoEquipo.fillCombo('/CatModeloEquipo/FillCboGrupo_ModeloEquipo', { estatus: true });
            cboModalGrupo.fillCombo('/CatModeloEquipo/FillCboGrupo_ModeloEquipo', { estatus: true });
            iniciarGridSubconjuntos();
            iniciarGridPrefijos();

            btnNuevo.click(openModal);
            btnGuardar.click(guardar);
            btnCancelar.click(reset);
            btnBuscar.click(clickBuscar);

            cboConjunto.change(cargarCboSubConjuntos);
            cboConjunto.change(habilitarAgregar);

            cboSubconjunto.change(habilitarAgregar);

            btnAgregarSubconjuntoNuevo.click(agregarSubconjunto);
            txtModalPrefijo.keyup(habilitarAgregarPrefijo);
            btnAgregarPrefijo.click(AgregarPrefijo);

            initGrid();
        }

        function cargarCboSubConjuntos() {
            if (cboConjunto.val() == "") { cboSubconjunto.prop("disabled", true); cboSubconjunto.empty() }
            else {
                cboSubconjunto.fillCombo('/CatModeloEquipo/FillCboSubConjunto', { idConjunto: cboConjunto.val() });
                cboSubconjunto.prop("disabled", false);
            }
        }

        function habilitarAgregar() {
            txtModalComponentesNumParte.val('');
            if (cboSubconjunto.val() == "" || $('#cboSubconjunto option').length == 0) { btnAgregarSubconjuntoNuevo.prop("disabled", true); }
            else { btnAgregarSubconjuntoNuevo.prop("disabled", false); }
        }

        function habilitarAgregarPrefijo() {
            if (txtModalPrefijo.val().trim() == '') { btnAgregarPrefijo.prop("disabled", true); }
            else { btnAgregarPrefijo.prop("disabled", false); }
        }

        function AgregarPrefijo() {
            var JSONINFO = [{ "id": idPrefijo, "prefijo": txtModalPrefijo.val().trim() }];
            gridPrefijos.bootgrid("append", JSONINFO);
            txtModalPrefijo.val('');
            btnAgregarPrefijo.prop("disabled", true);
            idPrefijo++;
        }
        function agregarSubconjunto() {
            var JSONINFO = [{ "subconjuntoID": parseInt(cboSubconjunto.val()), "conjunto": $("#cboConjunto option:selected").text(), "subconjunto": $("#cboSubconjunto option:selected").text(), "numParte": txtModalComponentesNumParte.val() }];
            gridSubconjuntos.bootgrid("append", JSONINFO);
            cboConjunto.val('');
            cboSubconjunto.val('');
            txtModalComponentesNumParte.val('');
        }

        function clickBuscar() {
            filtrarGrid();
        }

        function openModal() {
            reset();

            tituloModal.text("Alta Modelo Equipo");
            cboModalEstatusModeloEquipo.prop('disabled', true);
            cboConjunto.fillCombo('/CatModeloEquipo/FillCboConjunto');

            modalAlta.modal('show');
        }
        function update() {
            tituloModal.text("Actualizar Modelo Equipo");
            cboModalEstatusModeloEquipo.prop('disabled', false);
            cboConjunto.fillCombo('/CatModeloEquipo/FillCboConjunto');
            modalAlta.modal('show');
        }

        function guardar() {
            beforeSaveOrUpdate();
        }

        function beforeSaveOrUpdate() {
            if (valid()) {
                saveOrUpdate(null, getPlainObject(), getListaSubComponentes());
            }
        }

        function getPlainObject() {
            var prefijos = [];
            $('#gridPrefijos tbody tr').each(function () {
                prefijos.push($(this).find('td:eq(0)').text());
            });
            console.log(prefijos);
            return {
                id: idModeloMaquina,
                descripcion: txtModaldescripcion.val().trim(),
                marcaEquipoID: cboModalMarcas.val(),
                nomCorto: JSON.stringify(prefijos),
                noComponente: noComponente,
                Estatus: cboModalEstatusModeloEquipo.val() == estatus.ACTIVO ? true : false,
                idGrupo: cboModalGrupo.val(),
                overhaul: cboModalOverhaul.val() == estatus.ACTIVO ? true : false,
            }
        }

        function getListaSubComponentes() {
            var subconjuntos = [];
            $('#grid_Subconjuntos tbody tr').each(function () {
                subconjuntos.push([$(this).attr('data-row-id'), $(this).find('td:eq(3)').text()]);
            });
            return subconjuntos;
        }

        function filtrarGrid() {
            loadGrid(getFiltrosObject(), ruta, gridResultadoFiltros);
        }

        function getFiltrosObject() {
            return {
                id: 0,
                descripcion: txtDescripcionModelosEquipos.val().trim(),
                marcaEquipoID: cboFiltroMarcas.val(),
                Estatus: cboFiltroEstatus.val() == estatus.ACTIVO ? true : false
            }
        }

        function valid() {
            var state = true;
            if (!cboModalMarcas.valid()) { state = false; }
            //if (!txtModalNomCorto.valid()) { state = false; }
            if (!txtModaldescripcion.valid()) { state = false; }
            if (!cboModalEstatusModeloEquipo.valid()) { state = false; }
            if (!cboModalGrupo.valid()) { state = false; }
            return state;
        }



        function resetFiltros() {
            cboFiltroEstatus.val('1');
            txtDescripcionModelosEquipos.val('');
            cboFiltroMarcas.val('');
            cboModalGrupo.val('');
            gridResultadoFiltros.bootgrid('clear');
        }

        function initGrid() {
            gridResultadoFiltros.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {

                    "archivos": function (column, row) {
                        if (!row.hasArchivos) {
                            return "<button type='button' class='btn btn-primary verArchivos' data-index='" + row.id + "'>" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                " </button>"
                                ;
                        }
                        else {
                            return "";
                        }

                    },
                    "update": function (column, row) {
                        return "<button type='button' class='btn btn-warning modificar' data-index='" + row.id + "'" + "data-descripcion='" + row.descripcion + "'data-estatus='" + getEstatus(row.estatus) + "'data-idMarca='" + row.idMarca + "'data-nomCorto='" + row.nomCorto + "'data-noComponente='" + row.noComponente + "' data-idGrupo='" + row.idGrupo + "'>" +
                            "<span class='glyphicon glyphicon-edit '></span> " +
                            " </button>"
                            ;
                    },
                    "delete": function (column, row) {
                        return "<button type='button' class='btn btn-danger eliminar'  data-index='" + row.id + "'" + "data-descripcion='" + row.descripcion + "' data-estatus='" + getEstatus(row.estatus) + "'data-idMarca='" + row.idMarca + "'data-nomCorto='" + row.nomCorto + "'data-noComponente='" + row.noComponente + "' data-idGrupo='" + row.idGrupo + "'>" +
                            "<span class='glyphicon glyphicon-remove'></span> " +
                            " </button>"
                            ;
                    },
                    "addSubConjuntos": function (column, row) {

                        return "<button type='button' class='btn btn-success AgregarSubConjunto'  data-index='" + row.id + "'>" +
                            "<span class='glyphicon glyphicon-plus'></span> " +
                            " </button>"
                            ;
                    },

                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                gridResultadoFiltros.find(".modificar").on("click", function (e) {
                    reset();
                    idModeloMaquina = $(this).attr("data-index");
                    noComponente = $(this).attr("data-noComponente");
                    txtModaldescripcion.val($(this).attr("data-descripcion"));
                    cboModalMarcas.val($(this).attr("data-idMarca"));
                    //txtModalNomCorto.val($(this).attr("data-nomCorto"));
                    cboModalEstatusModeloEquipo.val($(this).attr("data-estatus"));
                    cboModalGrupo.val($(this).attr("data-idGrupo"));
                    Actualizacion = 2;
                    loadGridSubconjuntos(idModeloMaquina);
                    loadGridPrefijos($.parseJSON($(this).attr("data-nomCorto")))
                    update();
                    LoadGridArchivos();
                    loadArchivos(idModeloMaquina, grid_ArchivosModificar);

                });
                gridResultadoFiltros.find(".eliminar").on("click", function (e) {
                    var estado = $(this).attr("data-estatus");
                    if (estado == "1") {
                        idModeloMaquina = $(this).attr("data-index");
                        noComponente = $(this).attr("data-noComponente");
                        txtModaldescripcion.val($(this).attr("data-descripcion"));
                        cboModalMarcas.val($(this).attr("data-idMarca"));
                        //txtModalNomCorto.val($(this).attr("data-nomCorto"));
                        cboModalGrupo.val($(this).attr("data-idGrupo"));
                        Actualizacion = 3;
                        cboModalEstatusModeloEquipo.val("0");

                        ConfirmacionEliminacion("Dar baja registro", "¿Esta seguro que desea dar de baja este registro? " + $(this).attr("data-descripcion"));
                    }
                    else {
                        reset();
                    }

                });

                gridResultadoFiltros.find(".AgregarSubConjunto").on("click", function (e) {
                    var idModelo = $(this).attr('data-index');
                    LoadSubConjuntos(idModelo);
                });

                gridResultadoFiltros.find(".verArchivos").on("click", function (e) {
                    idModeloMaquina = $(this).attr("data-index");
                    modalVistaArchivo.modal('show');
                    loadArchivos(idModeloMaquina, grid_ArchivosVER);

                    LoadGridArchivos();
                });
            });
        }


        function LoadSubConjuntos(id) {
            $.ajax({
                url: '/CatModeloEquipo/verSubConjuntos',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ modeloID: id }),
                success: function (response) {

                    cboConjuntoModal.fillCombo('/CatModeloEquipo/FillCboConjunto');

                    modalAltaSubConjuntos.modal('show');


                },
                error: function (response) {
                }
            });
        }

        function loadGridSubconjuntos(idModeloMaquina) {
            $.ajax({
                url: '/CatModeloEquipo/FillGridSubConjunto',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ idModelo: idModeloMaquina }),
                success: function (response) {
                    gridSubconjuntos.bootgrid("append", response.subconjuntos);
                },
                error: function (response) {
                }
            });
        }

        function loadGridPrefijos(prefijos) {
            for (let i = 0; i < prefijos.length; i++) {
                var JSONINFO = [{ "id": idPrefijo, "prefijo": prefijos[i] }];
                gridPrefijos.bootgrid("append", JSONINFO);
                txtModalPrefijo.val('');
                idPrefijo++;
            }
        }

        function loadArchivos(id, elemento) {

            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/CatModeloEquipo/GetArchivosModificacion',
                data: { obj: id },
                success: function (response) {
                    $.unblockUI();
                    var data = response.ListaArchivosGrid;
                    if (data != undefined) {
                        if (data.length > 0) {


                            elemento.bootgrid("clear");
                            elemento.bootgrid("append", data);
                            elemento.bootgrid('reload');

                            var nombreid = elemento.attr('id');

                            if (nombreid == "grid_ArchivosModificar") {
                                tblArchivosDiv.removeClass('hide');
                            }
                        }
                    }
                    else {
                        var nombreid = elemento.attr('id');

                        if (nombreid == "grid_ArchivosModificar") {
                            tblArchivosDiv.addClass('hide');
                        }
                    }

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        var ArchivosEliminados = [];

        function LoadGridArchivos() {

            grid_ArchivosVER.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "descarga": function (column, row) {
                        return "<button type='button' class='btn btn-primary descargar'  data-idModelo='" + row.modeloId + "'" + "data-Nombre='" + row.Nombre + "'>" +
                            "<span class='glyphicon glyphicon-save'></span> " +
                            " </button>";
                    },
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                grid_ArchivosVER.find(".descargar").on("click", function (e) {
                    var elemento = $(this).attr('data-idModelo');
                    downloadURI(elemento);


                });
            });

            grid_ArchivosModificar.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "delete": function (column, row) {
                        return "<button type='button' class='btn btn-danger eliminar'  data-idModelo='" + row.modeloId + "'" + "data-Nombre='" + row.Nombre + "'>" +
                            "<span class='glyphicon glyphicon-remove'></span> " +
                            " </button>"
                            ;
                    },
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                grid_ArchivosModificar.find(".eliminar").on("click", function (e) {

                    ArchivosEliminados.push($(this).attr('data-idModelo'));
                    $(this).parents('tr').remove();
                });
            });
        }

        function reset() {
            idModeloMaquina = 0;
            Actualizacion = 1;
            noComponente = 1;
            txtModaldescripcion.val('');
            cboModalMarcas.val('');
            //txtModalNomCorto.val('');
            cboModalEstatusModeloEquipo.val('1');
            frmModeloEquipo.validate().resetForm();
            cboConjunto.val("");
            cboSubconjunto.prop("disabled", true);
            cboSubconjunto.empty();
            btnAgregarSubconjuntoNuevo.prop("disabled", true);
            btnAgregarPrefijo.prop("disabled", true);
            txtModalPrefijo.val('');
            gridSubconjuntos.bootgrid("clear");
            gridPrefijos.bootgrid("clear");
            $('#tabTitle1').tab('show');
            tblArchivosDiv.addClass('hide');
            idPrefijo = 0;
        }

        function downloadURI(elemento) {
            var link = document.createElement("button");
            link.download = '/CatModeloEquipo/getFileDownload?id=' + elemento;
            link.href = '/CatModeloEquipo/getFileDownload?id=' + elemento;
            link.click();
            location.href = '/CatModeloEquipo/getFileDownload?id=' + elemento;
        }

        function saveOrUpdate(e, obj, lista) {
            if (true) {
                var formData = new FormData();
                var files = document.getElementById("fupAdjunto").files;
                var listaSubConjuntos = [];
                var listaNumParte = [];
                if (lista != null) {
                    lista.forEach(function (v) {
                        listaSubConjuntos.push(v[0]);
                        v[1] != null ? listaNumParte.push(v[1]) : listaNumParte.push("");
                    });
                }
                formData.append("obj", JSON.stringify(obj));
                formData.append("ArchivosEliminados", JSON.stringify(ArchivosEliminados));
                formData.append("listaSubConjuntos", JSON.stringify(listaSubConjuntos));
                formData.append("listaNumParte", JSON.stringify(listaNumParte));

                $.each(files, function (i, file) {
                    formData.append('fupAdjunto[]', file);
                });

                if (files != undefined) {
                    $.blockUI({ message: 'Cargando archivo... ¡Espere un momento!' });
                }
                $.ajax({
                    type: "POST",
                    url: '/CatModeloEquipo/SubirArchivos',
                    data: formData,
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        if (response.success) {
                            fupAdjunto.val("");
                            modalAlta.modal('hide');
                            ConfirmacionGeneral("Confirmación", 'El registro se Actualizó correctamente');
                            reset();
                            filtrarGrid();
                        } else {
                            AlertaGeneral("Alerta", response.message);
                        }

                        $.unblockUI();
                    },
                    error: function (error) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", error);
                    }
                });
            } else {
                e.preventDefault()
            }
        }

        function iniciarGridSubconjuntos() {
            gridSubconjuntos.bootgrid({
                rowCount: -1,
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "quitar": function (column, row) {
                        return "<button type='button' class='btn btn-danger eliminar'  data-rowID='" + row.subconjuntoID + "'>" +
                            "<span class='glyphicon glyphicon-remove'></span>" +
                            "</button>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                gridSubconjuntos.find(".eliminar").on('click', function (e) {
                    var rowID = parseInt($(this).parent().parent().attr('data-row-id'));
                    gridSubconjuntos.bootgrid("remove", [rowID]);
                })
            });
        }

        function iniciarGridPrefijos() {
            gridPrefijos.bootgrid({
                rowCount: -1,
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: { header: "" },
                formatters: {
                    "quitar": function (column, row) {
                        return "<button type='button' class='btn btn-danger eliminar'  data-rowID='" + row.id + "'>" +
                            "<span class='glyphicon glyphicon-remove'></span>" +
                            "</button>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                gridPrefijos.find(".eliminar").on('click', function (e) {
                    var rowID = parseInt($(this).parent().parent().attr('data-row-id'));
                    gridPrefijos.bootgrid("remove", [rowID]);
                })
            });
        }

        init();

    };

    $(document).ready(function () {
        maquinaria.catalogo.grupoMaquinaria = new grupoMaquinaria();
    });
})();

