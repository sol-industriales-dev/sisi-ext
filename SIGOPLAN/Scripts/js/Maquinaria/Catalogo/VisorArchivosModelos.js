(() => {
    $.namespace('Maquinaria.Catalogo.VisorArchivosModelos');

    VisorArchivosModelos = function () {
        //#region Selectores
        const tblListaArchivosView = $("#tblListaArchivosView");
        const tblVistaDocumentosModelos = $('#tblVistaDocumentosModelos');
        const btnBuscarInfo = $('#btnBuscarInfo');
        const inputModelo = $('#inputModelo');
        const inputGrupo = $('#inputGrupo');
        const selectMarcaEquipo = $('#selectMarcaEquipo');
        const modalVistaArchivo = $('#modalVistaArchivo');
        //#endregion

        (function init() {
            selectMarcaEquipo.fillCombo('/CatModeloEquipo/FillCboMarcaEquipo_ModeloEquipo', { estatus: true });
            btnBuscarInfo.click(filtrarGrid);

            initGrid();
            filtrarGrid();
        })();

        function filtrarGrid() {
            loadGrid({
                id: 0,
                descripcion: inputModelo.val().trim(),
                grupoDesc: inputGrupo.val().trim(),
                marcaEquipoID: selectMarcaEquipo.val(),
                Estatus: true
            }, '/CatModeloEquipo/FillGridVistaDocumentosModelo', tblVistaDocumentosModelos);
        }

        function initGrid() {
            tblVistaDocumentosModelos.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: { header: "" },
                formatters: {
                    "archivos": function (column, row) {
                        if (!row.hasArchivos) {
                            return "<button type='button' class='btn btn-primary verArchivos' data-index='" + row.id + "'>" +
                                "<span class='glyphicon glyphicon-eye-open'></span> " +
                                " </button>";
                        } else {
                            return "";
                        }
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblVistaDocumentosModelos.find(".verArchivos").on("click", function (e) {
                    idModeloMaquina = $(this).attr("data-index");
                    modalVistaArchivo.modal('show');
                    loadArchivos(idModeloMaquina, tblListaArchivosView);
                    LoadGridArchivos();
                });
            });
        }

        function loadArchivos(id, elemento) {
            $.blockUI({ message: 'Procesando...' });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/CatModeloEquipo/GetArchivosModificacion',
                data: { obj: id },
                success: function (response) {
                    $.unblockUI();
                    var data = response.ListaArchivosGrid;
                    elemento.bootgrid("clear");
                    if (data != undefined) {
                        if (data.length > 0) {
                            elemento.bootgrid("append", data);
                            elemento.bootgrid('reload');
                        }
                    } else {
                        if (elemento.attr('id') == "grid_ArchivosModificar") {
                            tblArchivosDiv.addClass('hide');
                        }
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function LoadGridArchivos() {
            tblListaArchivosView.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: { header: "" },
                formatters: {
                    "descarga": function (column, row) {
                        return "<button type='button' class='btn btn-primary descargar'  data-idModelo='" + row.modeloId + "'" + "data-Nombre='" + row.Nombre + "'>" +
                            "<span class='glyphicon glyphicon-save'></span> " +
                            " </button>";
                    },
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblListaArchivosView.find(".descargar").on("click", function (e) {
                    downloadURI($(this).attr('data-idModelo'));
                });
            });
        }

        function downloadURI(elemento) {
            var link = document.createElement("button");
            link.download = '/CatModeloEquipo/getFileDownload?id=' + elemento;
            link.href = '/CatModeloEquipo/getFileDownload?id=' + elemento;
            link.click();
            location.href = '/CatModeloEquipo/getFileDownload?id=' + elemento;
        }
    }

    $(document).ready(() => Maquinaria.Catalogo.VisorArchivosModelos = new VisorArchivosModelos())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();