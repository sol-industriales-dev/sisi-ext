(function () {
    $.namespace('controlObra.InformeSemanal');

    InformeSemanal = function () {
        let plantillas = [];
        let pdf = [];
        let plantilla_id = 0;
        let cc_id = '';
        let pdfstring = [];
        let informePresentacion_id = 0;

        const getPlantilla = () => { return $.post('/ControlObra/ControlObra/GetPlantillaInformeDetalleCC') };
        const getInformeContenido = (informe_id) => { return $.post('/ControlObra/ControlObra/GetInformeSemanalContenido', { informe_id }) };
        const getUltimoInforme = () => { return $.post('/ControlObra/ControlObra/GetUltimoInforme') };
        const getInforme = (informe_id) => { return $.post('/ControlObra/ControlObra/GetInformeSemanal', { informe_id }) };

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

        //#region PDF VIEWER
        let pdfjsLib = window['pdfjs-dist/build/pdf'];
        let BASE64_MARKER = ';base64,';

        pdfjsLib.GlobalWorkerOptions.workerSrc = '//mozilla.github.io/pdf.js/build/pdf.worker.js';

        let pdfDoc = null,
            pageNum = 1,
            pageRendering = false,
            pageNumPending = null,
            scale = 1,
            canvas = null,
            ctx = null,
            currentTabOrden = null;

        function renderPage(num, orden) {
            pageRendering = true;
            // Using promise to fetch the page
            pdfDoc.getPage(num).then(function (page) {
                var viewport = page.getViewport({ scale: scale });
                canvas.height = viewport.height;
                canvas.width = viewport.width;

                // Render PDF page into canvas context
                var renderContext = {
                    canvasContext: ctx,
                    viewport: viewport
                };
                var renderTask = page.render(renderContext);

                // Wait for rendering to finish
                renderTask.promise.then(function () {
                    pageRendering = false;
                    if (pageNumPending !== null) {
                        // New page rendering is pending
                        renderPage(pageNumPending);
                        pageNumPending = null;
                    }
                });
            });

            // Update page counters
            document.getElementById('page_num' + orden).textContent = num;
        }

        function queueRenderPage(num, orden) {
            if (pageRendering) {
                pageNumPending = num;
            } else {
                renderPage(num, orden);
            }
        }

        function onPrevPage() {
            if (pageNum <= 1) {
                return;
            }
            pageNum--;
            queueRenderPage(pageNum, currentTabOrden);
        }

        function onNextPage() {
            if (pageNum >= pdfDoc.numPages) {
                return;
            }
            pageNum++;
            queueRenderPage(pageNum, currentTabOrden);
        }

        function onzoomIn() {
            scale = scale + 0.25;
            queueRenderPage(pageNum, currentTabOrden);
        }

        function onzoomOut() {
            if (scale <= 0.25) {
                return;
            }
            scale = scale - 0.25;
            queueRenderPage(pageNum, currentTabOrden);
        }

        function convertDataURIToBinary(dataURI) {
            var base64Index = dataURI.indexOf(BASE64_MARKER) + BASE64_MARKER.length;
            var base64 = dataURI.substring(base64Index);
            var raw = window.atob(base64);
            var rawLength = raw.length;
            var array = new Uint8Array(new ArrayBuffer(rawLength));

            for (var i = 0; i < rawLength; i++) {
                array[i] = raw.charCodeAt(i);
            }
            return array;
        }
        //#endregion

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

        function createTabsContent(plantilla, isLast) {
            const activeClass = plantilla.ordenDiapositiva == 1 ? "active" : "disabled"

            const div = `
                        <div class="tab-pane ${activeClass} panelContenido editor" role="tabpanel" id="step${plantilla.ordenDiapositiva}" data-orden="${plantilla.ordenDiapositiva}">
                            <h2 class="text-center">${plantilla.tituloDiapositiva}</h2>
                            
                            <div class="row">
                                <div class="col-md-12">
                                    <input id="inputFile${plantilla.ordenDiapositiva}" type="file" class="archivoContenido ${plantilla.ordenDiapositiva}" data-titulo="${plantilla.tituloDiapositiva.split(' ').join('')}" data-orden="${plantilla.ordenDiapositiva}" accept="application/pdf" />
                                </div>
                            </div>
                            
                            <div class="row">
                                <div class="col-sm-12 col-md-12">
                                    <div class="fr-view">
                                        <div id="editor${plantilla.tituloDiapositiva.split(' ').join('')}" class="editorContenido orden" data-orden="${plantilla.ordenDiapositiva}"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    `;
            return div;
        }

        function createBotonesHeaderInforme() {
            const botonAgregar = '<li><button type="button" class="btn btn-sm btn-primary addDiapositiva">Agregar Diapositiva</button></li>'
            const botonPreview = '<li><button type="button" class="btn btn-sm btn-info vistaPrevia">Vista Previa</button></li>'
            const botonGuardar = '<li><button type="button" class="btn btn-sm btn-success guardarInforme">Guardar Informe</button></li>'

            let div = `
                        <ul class="list-inline pull-right">
                            ${botonAgregar}
                            ${botonPreview}
                            ${botonGuardar}
                        </ul>
                    `;
            return div;
        }

        function createTabPresentacion(informe, isPDF) {
            const activeClass = informe.ordenDiapositiva == 1 ? "active" : "disabled"

            const btbPDF = isPDF ? `<button id="btnShowPdf" class="btn btn-sm btn-primary showPdf" data-orden="${informe.ordenDiapositiva}">Mostrar PDF</button>` : ''
            const contenido = informe.contenido == null ? '' : informe.contenido

            if (isPDF) {
                pdfstring.push({ ordenDiapositiva: informe.ordenDiapositiva, pdf: informe.pdf })
            }

            let div = `
                       
                            <div class="tab-pane ${activeClass}" role="tabpanel" id="step${informe.ordenDiapositiva}">

                                
                                <h2 class="text-center">${informe.tituloDiapositiva}</h2>

                                <ul class="list-inline pull-right" style="padding-right: 30px;">
                                    ${btbPDF}
                                </ul>

                                <br>
                                <br>
                                 <div class="col-sm-12 col-md-12">
                                    <div class="sectionPresentation">
                                        <div class="top_line"></div>
                                        <div class="presentationContent" style="height: 600px; overflow-y:scroll;">
                                            <br>
                                            ${contenido}
                                        </div>
                                    </div>
                                 </div>
                                    
                                    
                            </div>
                        
                    `;
            return div;
        }

        function createBotonesHeaderPresentacion() {
            const btnExpandir = '<li><button type="button" id="btnPantallaCompleta" class="btn btn-sm btn-default">Pantalla Completa</button></li>'
            const menuBack = '<li><button type="button" class="btn btn-sm btn-success menuPrincipal">Regresar al menu principal</button></li>'
            const editarInforme = '<li><button type="button" class="btn btn-sm btn-info editarInforme">Editar Informe</button></li>'

            let div = `
                        <ul class="list-inline pull-right">
                            ${editarInforme}
                            ${btnExpandir}
                            ${ menuBack}
                        </ul>
                    `;
            return div;
        }

        function createBotonesNextBackPresentacion() {
            const btnBack = '<button type="button" class="btn btn-primary prev-step"><</button>'
            const btnNext = '<button type="button" class="btn btn-primary next-step" >></button>'

            let div = `
                        <ul class="list-inline pull-right">
                            ${btnBack}
                            ${btnNext}
                        </ul>
                    `;
            return div;
        }

        function createModalPdf(ordenDiapositiva) {
            //Si se quiere poner encima de todo en el style iria position: absolute; 
            let div = `
                        <div id="divPDF${ordenDiapositiva}" style="right: 1%; top: 15px; padding-bottom:100px;">
                            <span class="close">&times;</span>
                            <div> 
                                <div class="text-center">
                                    <button id="zoominbutton${ordenDiapositiva}">zoom +</button>
                                    <button id="zoomoutbutton${ordenDiapositiva}">zoom -</button>
                                    <button id="prev${ordenDiapositiva}">Anterior</button>
                                    <button id="next${ordenDiapositiva}">Siguiente</button>

                                    &nbsp; &nbsp;
                                    <span>Pagina: <span id="page_num${ordenDiapositiva}"></span> / <span id="page_count${ordenDiapositiva}"></span></span>
                                </div>
                                <div class="canvas-container">
                                    <canvas id="the-canvas${ordenDiapositiva}" style="border:1px  solid black"></canvas>
                                </div>
                            </div>             
                        </div>
                    `
            return div;
        }

        function nextTab(elem) {
            $(elem).next().find('a[data-toggle="tab"]').click();
        }

        function prevTab(elem) {
            $(elem).prev().find('a[data-toggle="tab"]').click();
        }

        var zoom_percent = "100";
        function zoom(zoom_percent) {
            $(".mfp-figure figure").click(function () {
                switch (zoom_percent) {
                    case "100":
                        zoom_percent = "120";
                        break;
                    case "120":
                        zoom_percent = "150";
                        break;
                    case "150":
                        zoom_percent = "200";
                        $(".mfp-figure figure").css("cursor", "zoom-out");
                        break;
                    case "200":
                        zoom_percent = "100";
                        $(".mfp-figure figure").css("cursor", "zoom-in");
                        break;
                }
                $(this).css("zoom", zoom_percent + "%");
            });
        }

        function setGalleryImage() {
            $(document).find('.parent-container').magnificPopup({
                delegate: 'img', // child items selector, by clicking on it popup will open
                type: 'image',
                mainClass: 'mfp-with-zoom',
                gallery: {
                    enabled: true, // set to true to enable gallery

                    preload: [0, 2], // read about this option in next Lazy-loading section

                    navigateByImgClick: true,

                    arrowMarkup: '<button title="%title%" type="button" class="mfp-arrow mfp-arrow-%dir%"></button>', // markup of an arrow button

                    tPrev: 'Anterior', // title for left button
                    tNext: 'Siguiente', // title for right button
                    tCounter: '<span class="mfp-counter">%curr% de %total%</span>' // markup of counter
                },
                callbacks: {
                    elementParse: function (item) { item.src = item.el.attr('src'); },
                    open: function () {
                        $(".mfp-figure figure").css("cursor", "zoom-in");
                        zoom(zoom_percent);
                    },
                    close: function () {
                        // Will fire when popup is closed
                    }
                    // e.t.c.
                },

                // other options
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

        function guardar(informe_detalle) {
            const informe = {
                plantilla_id: plantilla_id,
                cc: cc_id,
                periodo: $('#periodo').text(),
                estatus: true
            }

            $.blockUI({ message: "Preparando información" });
            $.post('/ControlObra/ControlObra/GuardarInforme', { informe, informe_detalle })
                .done(response => {
                    if (response.success) {

                        Swal.fire({
                            title: 'El contenido ha sido guardado.',
                            type: 'success',
                            showConfirmButton: true,
                            confirmButtonText: 'Aceptar',
                            width: 700
                        }).then((result) => {
                            if (result.value) {
                                $.unblockUI();

                                debugger;
                                if (response.informe_id > 0) {
                                    const getUrl = window.location;
                                    const baseUrl = getUrl.protocol + "//" + getUrl.host;
                                    const urlInforme = baseUrl + `/ControlObra/ControlObra/InformeSemanal?informePresentacion=${response.informe_id}`;

                                    window.location.href = urlInforme;
                                } else {
                                    location.reload(true);
                                }
                            }
                        })

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

        function nextKey() {
            var $active = $('.wizard .nav-tabs li.active');
            $active.next().removeClass('disabled');
            nextTab($active)
        }

        function backKey() {
            var $active = $('.wizard .nav-tabs li.active');
            prevTab($active);
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

        function insertarContenido(orden, titulo) {
            $('#tabDetalles').append(createTabsContentNuevo(orden, titulo));
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
                                            <button class="btn btn-sm btn-danger  removeDiapositiva" data-orden="${orden}">Eliminar diapositiva</button>
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

            debugger;
            const $active = $('.wizard .nav-tabs li.active');
            if ($active.length > 0) {
                $active.removeClass('active');
                $active.next().removeClass('disabled');
                //nextTab($active);
                prevTab($('.wizard .nav-tabs li').last());
            } else {
                prevTab($('.wizard .nav-tabs li').last());
            }
        }

        function eliminarOrdenWizard() {
            // elimina la ultima, igual no importa el orden
            $("li.ordenWizard").last().remove()
        }

        function eliminarContenido(orden) {
            $(`.panelNuevo[data-orden="${orden}"]`).remove();
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
        //#endregion

        //#region CLICK EVENTS
        $(document).on('click', '.showPdf', function () {
            if ($('#the-canvas' + $(this).attr('data-orden')).length) {
                $(document).find('#divPDF' + $(this).attr('data-orden')).show(400);
            } else {
                $(this).parent().parent().append(createModalPdf($(this).attr('data-orden')));
                setTimeout(() => {
                    currentTabOrden = $(this).attr('data-orden');
                    document.getElementById('prev' + $(this).attr('data-orden')).addEventListener('click', onPrevPage);
                    document.getElementById('next' + $(this).attr('data-orden')).addEventListener('click', onNextPage);
                    document.getElementById("zoominbutton" + $(this).attr('data-orden')).addEventListener('click', onzoomIn);
                    document.getElementById("zoomoutbutton" + $(this).attr('data-orden')).addEventListener('click', onzoomOut);



                    canvas = document.getElementById('the-canvas' + $(this).attr('data-orden'));
                    ctx = canvas.getContext('2d');

                    var pdfAsDataUri = pdfstring.find(x => x.ordenDiapositiva == $(this).attr('data-orden')).pdf//detalle.pdf; // shortened
                    var pdfAsArray = convertDataURIToBinary(pdfAsDataUri);

                    pdfjsLib.getDocument(pdfAsArray).promise.then(function (pdfDoc_) {
                        pdfDoc = pdfDoc_;
                        document.getElementById('page_count' + currentTabOrden).textContent = pdfDoc.numPages;

                        // Initial/first page rendering
                        renderPage(pageNum);
                    });
                }, 1000)

                //$("body").addClass("viewer");
                //$('.wizard').addClass("viewer");
            }

        })

        $(document).on('click', '.close', function () {
            $(this).parent().hide(400);
        });

        $(document).on('change', 'input[type="file"]', function () {
            //Read File
            debugger;
            const selectedFile = $(this)[0].files// document.getElementById("inputFile1").files;
            const orden = $(this).attr('data-orden')//document.getElementById("inputFile1").className[0];
            const titulo = $(this).attr('data-titulo')//document.getElementById("inputFile1").className[2];


            //Check File is not Empty
            if (selectedFile.length > 0) {
                // Select the very first file from list
                var fileToLoad = selectedFile[0];
                // FileReader function for read the file.
                var fileReader = new FileReader();
                var base64;
                // Onload of file read the file content
                fileReader.onload = function (fileLoadedEvent) {
                    base64 = fileLoadedEvent.target.result;
                    // Print data in console
                    //console.log(base64);

                    pdf.push({
                        ordenDiapositiva: orden,
                        tituloDiapositiva: titulo,
                        contenido: base64 // Convert data to base64
                    })
                };

                // Convert data to base64
                fileReader.readAsDataURL(fileToLoad)
            }
        })

        $(document).on('click', ".guardarInforme", function (e) {

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
                    let informe_detalle = [];

                    $('.editor').each(function () {
                        let contenido = '';
                        let pdfContenido = '';

                        let editor = new FroalaEditor('div#' + $.escapeSelector($(this).find('.editorContenido').attr('id')), {}, function () { })
                        contenido = editor.html.get();

                        if (pdf.find(x => x.ordenDiapositiva === $(this).attr('data-orden')) != undefined) {
                            pdfContenido = pdf.find(x => x.ordenDiapositiva === $(this).attr('data-orden')).contenido
                        }

                        informe_detalle.push({
                            ordenDiapositiva: $(this).attr('data-orden'),
                            tituloDiapositiva: $(this).find('h2.text-center').text(),
                            contenido: contenido,
                            pdf: pdfContenido
                        });
                    });

                    guardar(informe_detalle)

                } else if (result.dismiss === Swal.DismissReason.cancel) {

                }

            })
        });

        $(document).on('click', '#btnPantallaCompleta', function (e) {
            var el = document.getElementsByTagName('html')[0]; el.webkitRequestFullscreen();
        })

        $(document).on('click', '.menuPrincipal', function () {
            const getUrl = window.location;
            const baseUrl = getUrl.protocol + "//" + getUrl.host;
            const urlInforme = baseUrl + `/ControlObra/ControlObra/PlantillaInforme`;

            window.location.href = urlInforme;
        })

        $(document).on('click', '.vistaPrevia', function () {

            if ($(this).val() == 'Vista editor') {
                $(this).val('Vista previa');
                $(this).text('Vista previa');
                $('#tabsHeader').show();
                $('.archivoContenido').show();

                const divContent = $('div.active');
                const dataOrden = divContent.attr('data-orden');
                const previewID = $('div.sectionPresentation[data-orden=' + dataOrden + ']').attr('id');

                $('div.editorContenido[data-orden=' + dataOrden + ']').parent().parent().children().show()
                $('div.sectionPresentation[data-orden=' + dataOrden + ']').remove();
            } else {
                $(this).val('Vista editor');
                $(this).text('Vista editor');
                $('#tabsHeader').hide();
                $('.archivoContenido').hide();

                const divContent = $('div.active');
                const dataOrden = divContent.attr('data-orden');
                const editorId = $('div.editorContenido[data-orden=' + dataOrden + ']').attr('id');

                const editor = new FroalaEditor('div#' + $.escapeSelector(editorId), {}, function () { })
                const contenido = editor.html.get();

                $('div.editorContenido[data-orden=' + dataOrden + ']').parent().parent().children().hide()
                $('div.editorContenido[data-orden=' + dataOrden + ']').parent().parent().append(setContenidoPreview(contenido, editorId, dataOrden))
            }
        });

        $(document).on('click', '.editarInforme', function () {

            const getUrl = window.location;
            const baseUrl = getUrl.protocol + "//" + getUrl.host;
            const urlInforme = baseUrl + `/ControlObra/ControlObra/InformeSemanal?informeEdicion=${informePresentacion_id}`;

            window.location.href = urlInforme;
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

        $(document).on('click', '#btnGuardarDiapositiva', function () {
            guardarDiapositiva();
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


            //#region PARAMETROS URL
            const variables = getUrlParams(window.location.href);

            if (variables && variables.informePresentacion) {
                getInformeContenido(variables.informePresentacion).done(response => {
                    if (response.success) {

                        $('#titulo').text(response.cc_desc)
                        $('#periodo').text(response.periodo)
                        $("#ulTabs").hide();
                        $('#logoCP').show();

                        contenido = response.informe
                        informePresentacion_id = variables.informePresentacion;

                        if (contenido.length > 0) {
                            contenido.forEach((detalle, index, arr) => {

                                $('#ulTabs').append(createTabsWizard(detalle))
                                $('#tabDetalles').append(createTabPresentacion(detalle, (detalle.pdf != null)))
                            });

                            $('#btnBack').hide();
                            $('#btnNext').hide();

                            $('#divBotonesHeader').append(createBotonesHeaderPresentacion());
                            $('#divTituloDerecha').append(createBotonesNextBackPresentacion())
                            $(document).find('img.fr-fic.fr-dii').parent().parent().addClass('parent-container')
                            setGalleryImage();
                        }
                    }
                })

            } else if (variables && variables.informeEdicion) {
                getInforme(variables.informeEdicion).done(function (response) {
                    if (response.success) {
                        detalle = response.informe;
                        cc_id = response.cc
                        plantilla_id = detalle[0].plantilla_id;
                        //informePresentacion_id = variables.informeEdicion;

                        $('#titulo').text('Informe Semanal: ' + response.cc_desc)
                        $('#periodo').text(response.periodo)
                        $('#logoCP').hide();

                        if (detalle.length > 0) {
                            plantillas = detalle;

                            detalle.forEach((det, index, arr) => {
                                $('#ulTabs').append(createTabsWizard(det))
                                $('#tabDetalles').append(createTabsContent(det, Object.is(arr.length - 1, index)))

                                new FroalaEditor('div#editor' + $.escapeSelector(det.tituloDiapositiva.split(' ').join('')), {
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
                                    },
                                    language: 'es',
                                    imageStyles: {
                                        class1: 'Class 1',
                                        class2: 'Class 2'
                                    },
                                    events: {
                                        'image.beforeUpload': function (files) {
                                            const editor = this
                                            if (files.length) {
                                                var reader = new FileReader()
                                                reader.onload = function (e) {
                                                    var result = e.target.result
                                                    editor.image.insert(result, null, null, editor.image.get())
                                                }
                                                reader.readAsDataURL(files[0])
                                            }
                                            return false
                                        }
                                    }
                                });

                                setTimeout(() => {
                                    let editor = new FroalaEditor('div#editor' + $.escapeSelector(det.tituloDiapositiva.split(' ').join('')), {}, function () { })
                                    editor.html.set(det.contenido)
                                }, 150)
                            })

                            $('#divBotonesHeader').append(createBotonesHeaderInforme())
                        }

                    }
                })
            } else {

                //SELECIONA OPCION DE CARGAR PLANTILLA O ULTIMO INFORME
                const swalWithBootstrapButtons = Swal.mixin({
                    customClass: {
                        confirmButton: 'btn btn-sm btn-primary space',
                        cancelButton: 'btn btn-sm btn-primary',
                    },
                    buttonsStyling: false,
                })

                swalWithBootstrapButtons.fire({
                    title: 'Seleccione una opción',
                    text: "",
                    type: 'info',
                    width: 500,
                    showCancelButton: true,
                    confirmButtonText: 'Cargar ultimo Informe',
                    cancelButtonText: 'Cargar Plantilla',
                    reverseButtons: true
                }).then((result) => {

                    if (result.value) {
                        getUltimoInforme().done(function (response) {
                            if (response.success) {
                                detalle = response.informe;
                                cc_id = response.cc
                                plantilla_id = detalle[0].plantilla_id;

                                $('#titulo').text('Informe Semanal: ' + response.cc_desc)
                                $('#periodo').text(response.periodo)
                                $('#logoCP').hide();

                                if (detalle.length > 0) {
                                    plantillas = detalle;

                                    detalle.forEach((det, index, arr) => {
                                        $('#ulTabs').append(createTabsWizard(det))
                                        $('#tabDetalles').append(createTabsContent(det, Object.is(arr.length - 1, index)))

                                        new FroalaEditor('div#editor' + $.escapeSelector(det.tituloDiapositiva.split(' ')).join(''), {
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
                                            },
                                            language: 'es',
                                            imageStyles: {
                                                class1: 'Class 1',
                                                class2: 'Class 2'
                                            },
                                            events: {
                                                'image.beforeUpload': function (files) {
                                                    const editor = this
                                                    if (files.length) {
                                                        var reader = new FileReader()
                                                        reader.onload = function (e) {
                                                            var result = e.target.result
                                                            editor.image.insert(result, null, null, editor.image.get())
                                                        }
                                                        reader.readAsDataURL(files[0])
                                                    }
                                                    return false
                                                }
                                            }
                                        });

                                        setTimeout(() => {
                                            let editor = new FroalaEditor('div#editor' + $.escapeSelector(det.tituloDiapositiva.split(' ').join('')), {}, function () { })
                                            editor.html.set(det.contenido)
                                        }, 150)
                                    })

                                    $('#divBotonesHeader').append(createBotonesHeaderInforme())
                                }

                            }
                        })
                    } else if (result.dismiss === Swal.DismissReason.cancel) {
                        getPlantilla().done(function (response) {
                            if (response.success) {
                                plantillas = response.plantilla;
                                plantilla_id = plantillas[0].plantilla_id
                                cc_id = response.cc

                                $('#titulo').text('Informe Semanal: ' + response.cc_desc)
                                $('#periodo').text(response.periodo)
                                $('#logoCP').hide();

                                if (plantillas.length > 0) {

                                    plantillas.forEach((detalle, index, arr) => {
                                        $('#ulTabs').append(createTabsWizard(detalle))
                                        $('#tabDetalles').append(createTabsContent(detalle, Object.is(arr.length - 1, index)))

                                        new FroalaEditor('div#editor' + $.escapeSelector(detalle.tituloDiapositiva.split(' ').join('')), {
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
                                            },
                                            language: 'es',
                                            imageStyles: {
                                                class1: 'Class 1',
                                                class2: 'Class 2'
                                            },
                                            events: {
                                                'image.beforeUpload': function (files) {
                                                    const editor = this
                                                    if (files.length) {
                                                        var reader = new FileReader()
                                                        reader.onload = function (e) {
                                                            var result = e.target.result
                                                            editor.image.insert(result, null, null, editor.image.get())
                                                        }
                                                        reader.readAsDataURL(files[0])
                                                    }
                                                    return false
                                                }
                                            }
                                        });

                                        setTimeout(() => {
                                            let editor = new FroalaEditor('div#editor' + $.escapeSelector(detalle.tituloDiapositiva.split(' ').join('')), {}, function () { })
                                            editor.html.set(detalle.contenido)
                                        }, 150)
                                    });

                                    $('#divBotonesHeader').append(createBotonesHeaderInforme())

                                } else {
                                    Swal.fire({
                                        type: 'error',
                                        title: 'Oops...',
                                        text: "No existe plantilla para el centro de costo " + response.cc_desc,
                                        customClass: {
                                            popup: 'format-pre'
                                        }
                                        // footer: '<a href>Why do I have this issue?</a>'
                                    });
                                }
                            }
                            else {
                                Swal.fire({
                                    type: 'error',
                                    title: 'Oops...',
                                    text: "No existe plantilla para el centro de costo: " + response.cc_desc,
                                    customClass: {
                                        popup: 'format-pre'
                                    }
                                    // footer: '<a href>Why do I have this issue?</a>'
                                });
                            }
                        });
                    }

                })
            }
            //#endregion

        })();
    }

    $(document).ready(function () {
        controlObra.InformeSemanal = new InformeSemanal();
    });
})();