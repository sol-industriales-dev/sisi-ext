(() => {
    $.namespace('controlObra.PlantillaInformeEditor');
    PlantillaInformeEditor = function () {
        let plantillas = [];
        let variables;

        // Se comprueba si hay variables en url.
        const getUrlParams = function (url) {
            let params = {};
            let parser = document.createElement('a');
            parser.href = url;
            let query = parser.search.substring(1);
            let vars = query.split('&');

            for (let i = 0; i < vars.length; i++) {
                let pair = vars[i].split('=');
                params[pair[0]] = decodeURIComponent(pair[1]);
            }

            return params;
        };

        const getPlantilla = (plantilla_id) => { return $.post('/ControlObra/ControlObra/GetPlantillaInformeDetalle', { plantilla_id }) };

        //#region METODOS 
        function createTabsWizard(plantilla) {
            let activeClass = plantilla.ordenDiapositiva == 1 ? "active" : "disabled"

            let div = `
                        <li role="presentation" class="${activeClass} ordenWizard" data-orden="${plantilla.ordenDiapositiva}">
                                <a href="#step${plantilla.ordenDiapositiva}" data-toggle="tab" aria-controls="step${plantilla.ordenDiapositiva}" role="tab" title="${plantilla.tituloDiapositiva}">
                                    <span class="round-tab">
                                        <i>${plantilla.ordenDiapositiva}</i>
                                    </span>
                                </a>
                        </li>
                    `;
            return div;
        }

        function createTabNuevo() {
            const numeroNuevaBolita = $("li.ordenWizard").length + 1;
            const activeClass = numeroNuevaBolita == 1 ? "active" : "disabled"

            let div = `
                        <li role="presentation" class="${activeClass} ordenWizard" data-orden="${numeroNuevaBolita}">
                                <a href="#step${numeroNuevaBolita}" data-toggle="tab" aria-controls="step${numeroNuevaBolita}" role="tab" title="">
                                    <span class="round-tab">
                                        <i>${numeroNuevaBolita}</i>
                                    </span>
                                </a>
                        </li>
                    `;
            return div;
        }

        function createBotonesHeader() {
            const botonAgregar = '<li><button type="button" class="btn btn-sm btn-primary addDiapositiva">Agregar Diapositiva</button></li>'
            const botonPreview = '<li><button type="button" class="btn btn-sm btn-info vistaPrevia">Vista Previa</button></li>'
            const botonGuardar = '<li><button type="button" class="btn btn-sm btn-success guardarPlantilla">Guardar plantilla</button></li>'

            let div = `
                        <ul class="list-inline pull-right">
                            ${botonAgregar}
                            ${botonPreview}
                            ${ botonGuardar}
                        </ul>
                    `;
            return div;
        }

        function createTabsContent(plantilla) {
            const activeClass = plantilla.ordenDiapositiva == 1 ? "active" : "disabled"

            let div = `
                        <div class="tab-pane ${activeClass} panelContenido editor" role="tabpanel" id="step${plantilla.ordenDiapositiva}" data-orden="${plantilla.ordenDiapositiva}">
                            <div class="row">
                                <div class="col-sm-1 col-md-1 col-lg-1"></div>
                                <div class="col-sm-10 col-md-10 col-lg-10 text-center">
                                    <div class="row">
                                        <div class="col-sm-3 col-md-3 col-lg-3">
                                        </div>
                                        <div class="col-md-6 text-center">
                                            <h2 class="text-center">${plantilla.tituloDiapositiva}</h2>
                                        </div>

                                        <div class="col-md-1 pull-right">
                                            <button class="btn btn-sm btn-danger  removeDiapositiva" data-orden="${plantilla.ordenDiapositiva}">Eliminar diapositiva</button>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-1 col-md-1 col-lg-1 ">
                                </div>
                            </div>
                            
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div id="editor${plantilla.tituloDiapositiva.split(' ').join('')}" class="editorContenido orden" data-orden="${plantilla.ordenDiapositiva}"> </div>
                                    </div>
                                </div>
                            </div>

                            
                        </div>
                    `;
            return div;
        }

        function createTabsContentNuevo(orden, titulo) {
            const activeClass = orden == 1 ? "active" : "disabled"

            let div = `
                        <div class="tab-pane ${activeClass} panelNuevo editor" role="tabpanel" id="step${orden}" data-orden="${orden}">

                            <div class="row">
                                <div class="col-sm-1 col-md-1 col-lg-1"></div>
                                <div class="col-sm-10 col-md-10 col-lg-10 text-center">
                                    <div class="row">
                                        <div class="col-sm-3 col-md-3 col-lg-3">
                                        </div>
                                        <div class="col-md-6 text-center">
                                            <h2 class="text-center">${titulo}</h2>
                                        </div>

                                        <div class="col-md-1 pull-right">
                                            <button class="btn btn-sm btn-danger removeDiapositiva" data-orden="${orden}">Eliminar diapositiva</button>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-1 col-md-1 col-lg-1 ">
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div id="editor${titulo.split(' ').join('')}" class="editorContenido orden" data-orden="${orden}"> </div>
                                    </div>
                                </div>
                            </div>

                            
                        </div>
                    `;
            return div;
        }

        function nextTab(elem) {
            $(elem).next().find('a[data-toggle="tab"]').click();
        }

        function prevTab(elem) {
            $(elem).prev().find('a[data-toggle="tab"]').click();
        }

        function guardar(plantilla_contenido) {

            $.blockUI({ message: "Preparando información" });
            $.post('/ControlObra/ControlObra/GuardarPlantillaContenido', { plantilla_id: variables.plantilla, plantilla_contenido: plantilla_contenido })
                .done(response => {
                    if (response.success) {

                        Swal.fire({
                            title: 'El contenido ha sido guardado, Se redirigirá al menu principal',
                            type: 'success',
                            showConfirmButton: true,
                            confirmButtonText: 'Aceptar',
                            width: 700
                        }).then((result) => {
                            if (result.value) {
                                const getUrl = window.location;
                                const baseUrl = getUrl.protocol + "//" + getUrl.host;
                                const urlPlantilla = baseUrl + `/ControlObra/ControlObra/PlantillaInforme`;

                                window.location.href = urlPlantilla;
                            }
                        })
                        $.unblockUI();
                    } else {
                        Swal.fire({
                            type: 'error',
                            title: 'Oops...',
                            text: response.error
                            // footer: '<a href>Why do I have this issue?</a>'
                        });
                        $.unblockUI();
                    }
                });

        }

        function setContenidoPreview(contenido, id, dataOrden) {
            let div = `
                        <div class="sectionPresentation" id="View${id}" data-orden=${dataOrden}>
                            <div class="top_line"></div>
                            <div class="presentationContent" style="height: 500px; overflow-y:scroll;">
                                <br>
                                ${contenido}
                            </div>
                        </div>
                        `;
            return div;
        }

        function guardarDiapositiva() {
            //validar que no este vacio y todo eso
            $('#modalDiapositiva').modal('hide');

            insertarNumero();
            insertarContenido($('#selectOrdenDiapositiva').val(), $('#txtTituloDiapositiva').val());
            setNewEdtitor($('#txtTituloDiapositiva').val())

            reordenar($('#selectOrdenDiapositiva').val(), 1);

            const $active = $('.wizard .nav-tabs li.active');
            if ($active.length > 0) {
                $active.removeClass('active');
                $active.next().removeClass('disabled');
                nextTab($active);
            } else {
                prevTab($('.wizard .nav-tabs li').last());
            }
        }

        function insertarNumero() {
            $('#ulTabs').append(createTabNuevo());
        }

        function insertarContenido(orden, titulo) {
            $('#tabDetalles').append(createTabsContentNuevo(orden, titulo));
        }

        function setNewEdtitor(editorName) {
            new FroalaEditor('div#editor' + $.escapeSelector(editorName.split(' ').join('')), {
                language: 'es',
                imageStyles: {
                    class1: 'Class 1',
                    class2: 'Class 2'
                },
                imageEditButtons: ['imageReplace', 'imageAlign', 'imageRemove', '|', 'imageLink', 'linkOpen', 'linkEdit', 'linkRemove', '-', 'imageDisplay', 'imageStyle', 'imageAlt', 'imageSize'],
                toolbarButtons: {
                    moreText: {
                        // List of buttons used in the  group.
                        buttons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', 'textColor', 'backgroundColor', 'inlineClass', 'inlineStyle', 'clearFormatting'],

                        // Alignment of the group in the toolbar.
                        align: 'left',

                        // By default, 3 buttons are shown in the main toolbar. The rest of them are available when using the more button.
                        buttonsVisible: 3
                    },

                    moreParagraph: {
                        buttons: ['alignLeft', 'alignCenter', 'formatOLSimple', 'alignRight', 'alignJustify', 'formatOL', 'formatUL', 'paragraphFormat', 'paragraphStyle', 'lineHeight', 'outdent', 'indent', 'quote'],
                        align: 'left',
                        buttonsVisible: 3
                    },

                    moreRich: {
                        buttons: ['insertLink', 'insertImage', 'insertVideo', 'insertTable', 'emoticons', 'fontAwesome', 'specialCharacters', 'embedly', 'insertFile', 'insertHR'],
                        align: 'left',
                        buttonsVisible: 3
                    },
                    moreMisc: {
                        buttons: ['undo', 'redo', 'fullscreen', 'spellChecker', 'selectAll', 'help'],
                        align: 'right',
                        buttonsVisible: 2
                    }
                }
            })

            setTimeout(() => {
                let editor = new FroalaEditor('div#editor' + $.escapeSelector(editorName.split(' ').join('')), {}, function () { })
                editor.html.set('')
            }, 100)
        }

        function eliminar(ordenEliminado) {
            eliminarOrdenWizard();
            eliminarContenido(ordenEliminado);
            reordenar(ordenEliminado, -1);

            const $active = $('.wizard .nav-tabs li.active');
            if ($active.length > 0) {
                $active.removeClass('active');
                $active.next().removeClass('disabled');
                nextTab($active);
                //prevTab(active);
            } else {
                prevTab($('.wizard .nav-tabs li').last());
            }
        }

        function eliminarOrdenWizard() {
            // elimina la ultima, igual no importa el orden
            $("li.ordenWizard").last().remove()
        }

        function eliminarContenido(orden) {
            $(`.editor[data-orden="${orden}"]`).remove();
        }

        // tipo indica si agrega o elimina
        // 1: Agregaron
        // -1: Eliminaron
        function reordenar(orden, tipo) {
            $('.panelContenido').each(function () {
                const ordenOld = $(this).attr('data-orden');
                if (ordenOld >= orden) {
                    $(this).attr('data-orden', +ordenOld + tipo)
                    $(this).attr('id', 'step' + (+ordenOld + tipo))
                    $(this).find('.removeDiapositiva').attr('data-orden', (+ordenOld + tipo));
                    $(this).find('.editorContenido').attr('data-orden', (+ordenOld + tipo));
                }
            });
        }

        function nextKey() {
            var $active = $('.wizard .nav-tabs li.active');
            $active.next().removeClass('disabled');
            nextTab($active)
        }

        function backKey() {
            var $active = $('.wizard .nav-tabs li.active');
            prevTab($active);
        }
        //#endregion

        //#region CLICK EVENTS
        $(document).on('click', '.vistaPrevia', function () {

            if ($(this).val() == 'Vista editor') {
                $(this).val('Vista previa');
                $(this).text('Vista previa');
                $('#tabsHeader').show();
                $('.removeDiapositiva').show();

                const divContent = $('div.active');
                const dataOrden = divContent.attr('data-orden');
                const previewID = $('div.sectionPresentation[data-orden=' + dataOrden + ']').attr('id');

                $('div.editorContenido[data-orden=' + dataOrden + ']').parent().parent().children().show()
                $('div.sectionPresentation[data-orden=' + dataOrden + ']').remove();
            } else {
                $(this).val('Vista editor');
                $(this).text('Vista editor');
                $('#tabsHeader').hide();
                $('.removeDiapositiva').hide();

                const divContent = $('div.active');
                const dataOrden = divContent.attr('data-orden');
                const editorId = $('div.editorContenido[data-orden=' + dataOrden + ']').attr('id');

                const editor = new FroalaEditor('div#' + $.escapeSelector(editorId), {}, function () { })
                const contenido = editor.html.get();

                $('div.editorContenido[data-orden=' + dataOrden + ']').parent().parent().children().hide()
                $('div.editorContenido[data-orden=' + dataOrden + ']').parent().parent().append(setContenidoPreview(contenido, editorId, dataOrden))
            }
        });

        $(document).on('click', '.addDiapositiva', function () {
            $('#txtTituloDiapositiva').val('')
            let items = [];

            $('.ordenWizard').each(function () {
                const orden = $(this).attr('data-orden');
                items.push({ Value: orden, Text: orden })
            });

            items.push({ Value: items.length + 1, Text: items.length + 1 })

            const lista = { items: items };

            $('#selectOrdenDiapositiva').fillCombo(lista, null, false, null);
            $('#modalDiapositiva').modal('show');

        });

        $(document).on('click', '.removeDiapositiva', function () {
            const orden = $(this).attr('data-orden');

            Swal.fire({
                title: '¿Esta seguro quiere eliminar esta diapositiva?',
                type: 'warning',
                showConfirmButton: true,
                confirmButtonText: 'Aceptar',
                width: 700
            }).then((result) => {
                if (result.value) {

                    eliminar(orden);
                }
            })

        })

        $(document).on('click', '#btnGuardarDiapositiva', function () {
            guardarDiapositiva();
        });

        $(document).on('click', ".guardarPlantilla", function (e) {

            const swalWithBootstrapButtons = Swal.mixin({
                customClass: {
                    confirmButton: 'btn btn-sm btn-success space',
                    cancelButton: 'btn btn-sm btn-danger',
                },
                buttonsStyling: false,
            })

            swalWithBootstrapButtons.fire({
                title: '¿Estas seguro de guardar?',
                text: "",
                type: 'warning',
                width: 500,
                showCancelButton: true,
                confirmButtonText: 'Guardar',
                cancelButtonText: 'Cancelar',
                reverseButtons: true
            }).then((result) => {
                if (result.value) {
                    let plantilla_contenido = [];

                    $('.editor').each(function () {
                        let contenido = '';

                        let editor = new FroalaEditor('div#' + $.escapeSelector($(this).find('.editorContenido').attr('id')), {}, function () { })
                        contenido = editor.html.get();

                        plantilla_contenido.push({
                            plantilla_id: variables.plantilla,
                            ordenDiapositiva: parseInt($(this).attr('data-orden')),
                            tituloDiapositiva: $(this).find('h2.text-center').text(),
                            contenido: contenido
                        });
                    });

                    guardar(plantilla_contenido)
                } else if (result.dismiss === Swal.DismissReason.cancel) {

                }

                return 4;
            })
        });

        document.onkeydown = checkKey;
        function checkKey(e) {

            e = e || window.event;

            if (e.keyCode == '38') {
                // up arrow
            }
            else if (e.keyCode == '40') {
                // down arrow
            }
            else if (e.keyCode == '37') {
                // left arrow
                backKey();
            }
            else if (e.keyCode == '39') {
                // right arrow
                nextKey()
            }

        }
        //#endregion

        (function init() {
            //#region FUNCIONES WIZARD 
            $('.nav-tabs > li a[title]').tooltip();

            $(document).on('show.bs.tab', 'a[data-toggle="tab"]', function (e) {
                var $target = $(e.target);

                if ($target.parent().hasClass('disabled')) {
                    return false;
                }
            });

            $(document).on('click', ".next-step", function (e) {
                nextKey();
            });

            $(document).on('click', ".prev-step", function (e) {
                backKey();
            })
            //#endregion

            //#region CARGA PLANTILLA
            variables = getUrlParams(window.location.href);
            if (variables && variables.plantilla) {

                getPlantilla(variables.plantilla).done(function (response) {
                    if (response.success) {
                        plantillas = response.plantilla;

                        plantillas.forEach((detalle, index, arr) => {
                            $('#ulTabs').append(createTabsWizard(detalle))

                            $('#tabDetalles').append(createTabsContent(detalle, Object.is(arr.length - 1, index)))

                            new FroalaEditor('div#editor' + $.escapeSelector(detalle.tituloDiapositiva.split(' ').join('')), {
                                language: 'es',
                                imageStyles: {
                                    class1: 'Class 1',
                                    class2: 'Class 2'
                                },
                                imageEditButtons: ['imageReplace', 'imageAlign', 'imageRemove', '|', 'imageLink', 'linkOpen', 'linkEdit', 'linkRemove', '-', 'imageDisplay', 'imageStyle', 'imageAlt', 'imageSize'],
                                toolbarButtons: {
                                    moreText: {
                                        // List of buttons used in the  group.
                                        buttons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', 'textColor', 'backgroundColor', 'inlineClass', 'inlineStyle', 'clearFormatting'],

                                        // Alignment of the group in the toolbar.
                                        align: 'left',

                                        // By default, 3 buttons are shown in the main toolbar. The rest of them are available when using the more button.
                                        buttonsVisible: 3
                                    },

                                    moreParagraph: {
                                        buttons: ['alignLeft', 'alignCenter', 'formatOLSimple', 'alignRight', 'alignJustify', 'formatOL', 'formatUL', 'paragraphFormat', 'paragraphStyle', 'lineHeight', 'outdent', 'indent', 'quote'],
                                        align: 'left',
                                        buttonsVisible: 3
                                    },

                                    moreRich: {
                                        buttons: ['insertLink', 'insertImage', 'insertVideo', 'insertTable', 'emoticons', 'fontAwesome', 'specialCharacters', 'embedly', 'insertFile', 'insertHR'],
                                        align: 'left',
                                        buttonsVisible: 3
                                    },
                                    moreMisc: {
                                        buttons: ['undo', 'redo', 'fullscreen', 'spellChecker', 'selectAll', 'help'],
                                        align: 'right',
                                        buttonsVisible: 2
                                    }
                                }
                            })

                            setTimeout(() => {

                                let editor = new FroalaEditor('div#editor' + $.escapeSelector(detalle.tituloDiapositiva.split(' ').join('')), {}, function () { })
                                editor.html.set(detalle.contenido)
                            }, 100)
                        });



                        $('#divBotonesHeader').append(createBotonesHeader())
                        //$('#tabDetalles').append('<div class="clearfix"></div>')
                    }
                });
            }
            //#endregion


        })();

    }
    $(document).ready(() => controlObra.PlantillaInformeEditor = new PlantillaInformeEditor())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();