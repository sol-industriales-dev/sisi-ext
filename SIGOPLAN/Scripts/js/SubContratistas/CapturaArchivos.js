(() => {
    $.namespace('SubContratistas.CapturaArchivos');
    CapturaArchivos = function () {

        //#region Variables Date
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const defaultDate = new Date();
        //#endregion

        let documentos;
        const divDocumentos = $("#divDocumentos");
        const modalJustificacion = $("#modalJustificacion");
        const txtComentarioJustificacion = $("#txtComentarioJustificacion");
        const btnGuardarJustificacion = $("#btnGuardarJustificacion");

        (function init() {

            CargarArchivosFijos();
        })();

        function InicializarListeners() {
            $('input.botonDocumento').change(function () {
                //labelRegistroEspecializacion.text($(this)[0].files[0].name);

                if ($(this)[0].files.length > 0) {
                    $(this).removeClass('btn-primary');
                    $(this).addClass('btn-success');
                    $(this).find('i').removeClass('fa-folder-open');
                    $(this).find('i').addClass('fa-check');

                    let archivo = $(this)[0].files.length > 0 ? $(this)[0].files[0] : null;
                    let tipoArchivo = $(this).attr("data-index");
                    let fechaVencimiento = $(this).parent().find('input.inputFechaVencimiento').val()

                    guardarCaptura(archivo, tipoArchivo, fechaVencimiento);
                } else {
                    $(this).addClass('btn-primary');
                    $(this).removeClass('btn-success');
                    $(this).find('i').addClass('fa-folder-open');
                    $(this).find('i').removeClass('fa-check');
                }
            });
            $('label.inputs').click(function () {
                var clickable = $(this).parent().find('input.botonDocumento').attr('data-validacion');
                if (clickable == 2) {
                    $(this).parent().find('input.botonDocumento').click();
                } else {
                    let archivoID = +$(this).parent().find('input.botonDocumento').attr('archivocargadoid')
                    downloadURI(archivoID);
                }
            });

            $('input.checkAplica').change(function () {
                if (!$(this).prop('checked')) {
                    txtComentarioJustificacion.val('');
                    btnGuardarJustificacion.attr("data-index", $(this).attr("data-index"));
                    btnGuardarJustificacion.attr("data-fechaVencimiento", $(this).parent().parent().parent().find('input.inputFechaVencimiento').val());
                    modalJustificacion.modal("show");
                }
            });

            modalJustificacion.on('hidden.bs.modal', function () {
                $('input.checkAplica').prop('checked', true);
            });

            btnGuardarJustificacion.click(function () {
                var documentacionID = $(this).attr("data-index");
                var fechaVencimiento = $(this).attr("data-fechaVencimiento");
                guardarCapturaNoAplica(documentacionID, fechaVencimiento)
            });

            $('.inputEditarArchivo').change(function () {
                let archivoCargadoID = +$(this).attr('archivocargadoid');
                let archivo = $(this)[0].files.length > 0 ? $(this)[0].files[0] : null;

                editarArchivo(archivoCargadoID, archivo);
            });

            $('.inputRenovarArchivo').change(function () {
                let archivoCargadoID = +$(this).attr('archivocargadoid');
                let archivo = $(this)[0].files.length > 0 ? $(this)[0].files[0] : null;

                renovarArchivo(archivoCargadoID, archivo);
            });

            $('input.inputFechaVencimiento').datepicker({ dateFormat, showAnim, defaultDate });
        }

        function downloadURI(elemento) {
            var link = document.createElement("button");
            link.download = '/SubContratistas/SubContratistas/getFileDownload?id=' + elemento;
            link.href = '/SubContratistas/SubContratistas/getFileDownload?id=' + elemento;
            link.click();
            location.href = '/SubContratistas/SubContratistas/getFileDownload?id=' + elemento;
        }

        function guardarCaptura(archivo, documentacionID, fechaVencimiento) {
            if (archivo != null) {
                const data = new FormData();

                data.append('archivo', archivo);
                data.append('subcontratista', JSON.stringify({ id: 1 }));
                data.append('documentacionID', documentacionID);
                data.append('justificacion', "");
                data.append('fechaVencimiento', fechaVencimiento);

                guardarArchivo(data);
            }
        }

        function guardarCapturaNoAplica(documentacionID, fechaVencimiento) {
            const data = new FormData();

            data.append('archivo', null);
            data.append('subcontratista', JSON.stringify({ id: 1 }));
            data.append('documentacionID', documentacionID);
            data.append('justificacion', txtComentarioJustificacion.val());
            data.append('fechaVencimiento', fechaVencimiento);

            guardarArchivo(data);
        }

        function guardarArchivo(data) {
            let captura = data;

            $.ajax({
                url: 'GuardarArchivoSubcontratista',
                data: captura,
                async: false,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST'
            }).then(response => {
                if (response.success) {
                    Alert2Exito(`Se ha guardado la información.`);
                    CargarArchivosFijos();
                } else {
                    AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }

        function CargarArchivosFijos() {
            $.ajax({
                url: 'CargarArchivosFijos',
                data: null,
                async: false,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST'
            }).then(response => {
                if (response.success) {
                    documentos = response.documentos;
                    divDocumentos.empty();
                    html = "";
                    for (var i = 0; i < documentos.length; i++) {
                        if (i % 4 == 0) html += '<div class="col-sm-12" style="margin-top: 5px;">'
                        html += `
                            <div class="col-lg-3">
                                <div class="panel-group">
                                    <div class="panel panel-primary">
                                        <div class="panel-heading">
                                            <div class="panel-title" style="font-size: 15px;">
                                                ${(documentos[i].opcional ? '<input type="checkbox" class="checkAplica pull-left" style="width: 20px; height: 20px; margin-top: 1px;" data-index="' + documentos[i].id + '" checked ' + (documentos[i].validacion == 2 ? '' : 'disabled') + ' >  ' : '') + documentos[i].nombre}
                                                ${documentos[i].validacion == 0 ? agregarBotonEditarArchivo(documentos[i].archivoCargadoID) : documentos[i].validacion == 1 ? agregarBotonRenovarArchivo(documentos[i].archivoCargadoID) : ''}
                                            </div>
                                        </div>
                                        <div class="panel-collapse collapse in" aria-expanded="true" style="">
                                            <div class="panel-body ${(documentos[i].validacion == 0 ? 'p-warning' : (documentos[i].validacion == 1 ? 'p-success' : 'p-primary'))} ">
                                                <div class="row text-center">
                                                    <label class="inputs pointer btn" style="margin-top: 10px; border-radius: 20px;">
                                                        ${(documentos[i].validacion == 0 ? '<i class="glyphicon glyphicon-hourglass fa-6x"></i>' : (documentos[i].validacion == 1 ? '<i class="fa fa-check fa-6x"></i>' : '<i class="fa fa-file-alt fa-6x"></i>'))}
                                                    </label>
                                                    <input class="botonDocumento" archivocargadoid="${documentos[i].archivoCargadoID}" data-index="${documentos[i].id}" data-validacion="${documentos[i].validacion}" ${(documentos[i].validacion == 2 ? 'type="file"' : '')} style="display:none;">
                                                    <label class="label" style="display: block;"> ${(documentos[i].validacion == 0 ? 'Pendiente Autorización' : (documentos[i].validacion == 1 ? 'Autorizado' : 'Ningún Archivo Seleccionado'))}
                                                    </label>
                                                    ${(documentos[i].aplicaFechaVencimiento ? (documentos[i].validacion == 2 ? '<input class="form-control text-center inputFechaVencimiento" data-index=' + documentos[i].id + ' placeholder="Fecha de Vencimiento" ' + (documentos[i].validacion == 2 ? '' : 'disabled') + '>' : '<label>Fecha Vencimiento: ' + documentos[i].fechaVencimiento + '</label>') : '')}
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>`;
                        if (i % 4 == 3) html += '</div>'
                    }
                    if (documentos.length % 4 != 0) html += '</div>'
                    divDocumentos.append(html);
                    InicializarListeners();
                } else {
                    AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );

        }

        function agregarBotonEditarArchivo(archivoCargadoID) {
            return `
                <div class="text-center" style="display: inline-block;">
                    <label id="botonArchivo_${archivoCargadoID}" for="inputArchivo_${archivoCargadoID}" class="btn btn-xs btn-warning botonEditarArchivo"><i class="fa fa-edit"></i></label>
                    <input id="inputArchivo_${archivoCargadoID}" type="file" archivocargadoid="${archivoCargadoID}" class="inputEditarArchivo inputArchivo_${archivoCargadoID}" accept="application/pdf, image/*" style="display: none;">
                </div>
            `;
        }

        function agregarBotonRenovarArchivo(archivoCargadoID) {
            return `
                <div class="text-center" style="display: inline-block;">
                    <label id="botonArchivo_${archivoCargadoID}" for="inputArchivo_${archivoCargadoID}" class="btn btn-xs btn-success botonRenovarArchivo"><i class="fa fa-redo-alt"></i></label>
                    <input id="inputArchivo_${archivoCargadoID}" type="file" archivocargadoid="${archivoCargadoID}" class="inputRenovarArchivo inputArchivo_${archivoCargadoID}" accept="application/pdf, image/*" style="display: none;">
                </div>
            `;
        }

        function editarArchivo(archivoCargadoID, archivo) {
            const data = new FormData();

            data.append('archivo', archivo);
            data.append('subcontratista', JSON.stringify({ id: 1 }));
            data.append('archivoCargadoID', archivoCargadoID);

            guardarArchivoEditado(data);
        }

        function renovarArchivo(archivoCargadoID, archivo) {
            const data = new FormData();

            data.append('archivo', archivo);
            data.append('subcontratista', JSON.stringify({ id: 1 }));
            data.append('archivoCargadoID', archivoCargadoID);

            guardarArchivoRenovado(data);
        }

        function guardarArchivoEditado(data) {
            let captura = data;

            $.ajax({
                url: 'GuardarArchivoEditadoSubcontratista',
                data: captura,
                async: false,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST'
            }).then(response => {
                if (response.success) {
                    Alert2Exito(`Se ha guardado la información.`);
                    CargarArchivosFijos();
                } else {
                    AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }

        function guardarArchivoRenovado(data) {
            let captura = data;

            $.ajax({
                url: 'GuardarArchivoRenovadoSubcontratista',
                data: captura,
                async: false,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST'
            }).then(response => {
                if (response.success) {
                    Alert2Exito(`Se ha guardado la información.`);
                    CargarArchivosFijos();
                } else {
                    AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }
    }
    $(document).ready(() => SubContratistas.CapturaArchivos = new CapturaArchivos())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();