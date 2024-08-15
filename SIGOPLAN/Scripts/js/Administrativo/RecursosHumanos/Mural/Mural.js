(() => {
    $.namespace('RecursosHumanos.Mural');
    Mural = function () {

        /* SELECTORES */
        const addPostIt = $('#addPostIt');
        const addBloque = $('#addBloque');
        const cleanMural = $('#cleanMural');
        const zoomIn = $('#zoomIn');
        const zoom0 = $('#zoom0');
        const zoomOut = $('#zoomOut');
        const saveMural = $('#saveMural');
        const cboMural = $('#cboMural');
        const newMural = $('#newMural');

        const mural = $('#mural');

        const modalNewMural = $('#modalNewMural');
        const txtNombreMural = $('#txtNombreMural');
        const btnSaveNombreMural = $('#btnSaveNombreMural');

        /* GLOBALES */
        const objetoMural = {
            id: 0,
            color: '#ffffff',
            titulo: ''
        }

        const postIt = {
            id: 0,
            idMural: objetoMural.id,
            idSeccion: null,
            texto: '',
            posicionX: $(document).scrollTop() + 200,
            posicionY: $(document).scrollLeft() + 200,
            colorFondo: '',
            altura: 200,
            ancho: 200,
            estatus: true
        }

        const bloque = {
            id: 0,
            idMural: objetoMural.id,
            posicionX: $(document).scrollTop() + 200,
            posicionY: $(document).scrollLeft() + 200,
            colorFondo: '#ffffff',
            altura: 300,
            ancho: 250,
            estatus: true
        }

        let timer;
        let timeout = 1000;

        /* EVENTOS */
        cboMural.on('change', function() {
            if ($(this).val() != null && $(this).val() != undefined && $(this).val() != '') {
                obtenerMural();
            }
            else {
                mural.empty();
            }
        });
        
        addPostIt.on('click', function() {
            let nuevoPostIt = Object.create(postIt);

            nuevoPostIt.texto = '';
            nuevoPostIt.colorFondo = '#ffffcc';
            nuevoPostIt.idMural = objetoMural.id;

            agregarPostIt(nuevoPostIt);
        });

        addBloque.on('click', function() {
            let nuevoBloque = Object.create(bloque);

            nuevoBloque.texto = ''
            nuevoBloque.colorFondo = '#ffffff';
            nuevoBloque.idMural = objetoMural.id;

            agregarBloque(nuevoBloque);
        });

        mural.on('resize', '.postIt', function() {
            let valHeight = $(this).css('height');
            let valWidth = $(this).css('width');
            let valSinPX = valHeight.replace('px', '');
            let valWidthSinPX = valWidth.replace('px', '');
            $(this).find('.note-editor').find('.note-editing-area').find('.note-editable').css('height', (valSinPX) + 'px');
            $(this).find('.note-editor').find('.note-editing-area').find('.note-editable').css('width', valWidthSinPX + 'px');

            if (timer) {
                clearTimeout(timer);
            }
            let sendPostIt = $(this).closest('.postIt');
            $(this).data('data-info').ancho = valWidthSinPX;
            $(this).data('data-info').altura = valSinPX;
            timer = setTimeout(function() {
                guardarPostIt(sendPostIt)
            }, timeout);
        });

        mural.on('mouseenter', '.postIt', function() {
            $(this).find('.header-postIt').find('.closePostIt').show();
            $(this).find('.ui-resizable-handle').show();
            //$(this).find('.note-editor').find('.note-toolbar').show();
            

            $.each($('.postIt'), function(index, element) {
                $(element).css('z-index', 999);
            });

            $(this).css('z-index', 1000);
        });

        mural.on('dblclick', '.note-editable', function() {
            $(this).focus();
            $('.note-toolbar').hide();
            $(this).closest('.note-editing-area').closest('.note-editor').find('.note-toolbar').show();
            $(this).closest('.note-editing-area').closest('.note-editor').parent().draggable('option', 'disabled', true);
            $(this).closest('.note-editing-area').closest('.note-editor').parent().parent().draggable('option', 'disabled', true);
        });

        mural.on('mousedown', '.header-postIt', function() {
            $(this).closest('.postIt').draggable('option', 'disabled', false);
        });

        mural.on('mouseleave', '.postIt', function() {
            $(this).find('.header-postIt').find('.closePostIt').hide();
            //$(this).find('.note-editor').find('.note-toolbar').hide();
            $(this).find('.ui-resizable-handle').hide();
        });

        mural.on('blur', '.postIt', function(event) {
            // console.log($(event));
            if ($(event)[0].relatedTarget == null) {
                $('.note-toolbar').hide();
                $(this).closest('.postIt').draggable('option', 'disabled', false);
                $(this).closest('.postIt').parent().draggable('option', 'disabled', false);
            }
            //$(this).closest('.note-editing-area').prev('.note-toolbar').hide();
            //$(this).find('.note-editor').find('.note-toolbar').hide();
        });

        mural.on('mouseup', '.postIt', function() {
            $(this).data('data-info').posicionX = $(this).position().left;
            $(this).data('data-info').posicionY = $(this).position().top;
        });

        mural.on('click', '.closePostIt', function() {
            eliminarPostIt($(this).closest('.postIt').data('data-info').id);
            $(this).closest('.postIt').remove();
        });

        mural.on('resize', '.bloque', function(event) {
            if (!$(event)[0].target.className.includes('postIt')) {
                let valHeight = $(this).css('height');
                let valWidth = $(this).css('width');
                let valSinPX = valHeight.replace('px', '');
                let valWidthSinPX = valWidth.replace('px', '');

                if (timer) {
                    clearTimeout(timer);
                }
                let sendBloque = $(this).closest('.bloque');
                $(this).data('data-info').ancho = valWidthSinPX;
                $(this).data('data-info').altura = valSinPX;
                timer = setTimeout(function () {
                    guardarBloque(sendBloque);
                }, timeout);
            }
        });

        mural.on('mouseenter', '.bloque', function() {
            $(this).find('.bloque-header').find('.closeBloque').show();
            $(this).find('.ui-resizable-handle').show();

            $.each($('.bloque'), function(index, element) {
                $(element).css('z-index', 995);
            });

            $(this).css('z-index', 996);
            $(this).find('.btnColorPickerBloque').show();
            $(this).find('.colorPickerBloque').show();
        });

        mural.on('mouseleave', '.bloque', function() {
            $(this).find('.bloque-header').find('.closeBloque').hide();
            $(this).find('.ui-resizable-handle').hide();
            $(this).find('.btnColorPickerBloque').hide();
            $(this).find('.colorPickerBloque').hide();
        });

        mural.on('mouseup', '.bloque', function() {
            $(this).data('data-info').posicionX = $(this).position().left;
            $(this).data('data-info').posicionY = $(this).position().top;
        });

        mural.on('click', '.closeBloque', function() {
            eliminarSeccion($(this).closest('.bloque').data('data-info').id);
            $(this).closest('.bloque').remove();
        });

        cleanMural.on('click', function() {
            objetoMural.id = 0;
            objetoMural.color = '#ffffff';
            objetoMural.titulo = '';

            cboMural.val('');

            mural.empty();
        });

        zoomIn.on('click', function() {
            let muralZoom = mural.css('zoom');
            mural.animate({
                zoom: muralZoom * 1.2
            }, 400);
        });

        zoom0.on('click', function() {
            mural.animate({
                zoom: 1
            }, 400);
        });

        zoomOut.on('click', function() {
            let muralZoom = mural.css('zoom');
            mural.animate({
                zoom: muralZoom - 0.2
            }, 400);
        });

        newMural.on('click', function() {
            modalNewMural.modal('show');
        });

        btnSaveNombreMural.on('click', function() {
            objetoMural.id = 0;
            objetoMural.color = '#ffffff';
            objetoMural.titulo = txtNombreMural.val();

            crearMural();
        });

        saveMural.on('click', function() {
            guardarMural();
        });

        mural.on('click', '.btnColorPickerPostIt', function() {
            $(this).prev('.colorPickerPostIt').trigger('click');
        });

        mural.on('change', '.colorPickerPostIt', function() {
            $(this).closest('.note-editor').find('.note-editing-area').find('.note-editable').css('background-color', $(this).val());
            
            if (timer) {
                clearTimeout(timer);
            }
            let sendPostIt = $(this).closest('.postIt');
            $(this).closest('.postIt').data('data-info').colorFondo = $(this).val();
            timer = setTimeout(function() {
                guardarPostIt(sendPostIt)
            }, timeout);
        });

        mural.on('click', '.btnColorPickerBloque', function() {
            $(this).prev('.colorPickerBloque').trigger('click');
        });

        mural.on('change', '.colorPickerBloque', function() {
            $(this).closest('.bloque-header').closest('.bloque').css('background-color', $(this).val());
            
            if (timer) {
                clearTimeout(timer);
            }
            let sendBloque = $(this).closest('.bloque');
            $(this).closest('.bloque').data('data-info').colorFondo = $(this).val();
            timer = setTimeout(function() {
                guardarBloque(sendBloque)
            }, timeout);
        });

        /* FUNCIONES POST ITS */
        function agregarPostIt(postIt) {
            // console.log(postIt.altura);
            let nuevoPostItHTML = '' +
                '<div class="postIt nuevo-postIt" ' +
                    'style="position: absolute; ' +
                    'z-index: 1000; ' + 
                    'top: ' + postIt.posicionY + 'px;' +
                    'left: ' + postIt.posicionX + 'px; ' +
                    'height; ' + postIt.altura + 'px; ' +
                    'width: ' + postIt.ancho + 'px;">' +
                    '<div class="header-postIt" ' +
                        'style="position: absolute; ' +
                        'z-index: 999; ' +
                        // 'top: -35px; ' +
                        'left: -2px; ' +
                        'height: 17px; ' +
                        'width: 20px;">' +
                        '<i class="fas fa-times pull-right closePostIt"></i>' +
                    '</div>' +
                    '<div class="postIt-texto">' +
                    postIt.texto +
                    '</div>' +
                '</div>';

            if (postIt.idSeccion == null) {
                mural.append(nuevoPostItHTML);
            }
            else {
                let bloqueDondeInsertar;
                $.each($('.bloque'), function(index, element) {
                    let data = $(element).data('data-info');
                    if (data.id == postIt.idSeccion) {
                        bloqueDondeInsertar = element;
                    }
                });
                $(bloqueDondeInsertar).append(nuevoPostItHTML);
            }

            $('.nuevo-postIt').find('.postIt-texto').summernote({
                containment: 'parent',
                disableDragAndDrop: true,
                toolbar: [
                    ['style', ['bold']],
                    ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['para', ['paragraph']]
                ],
                callbacks: {
                    onChange: function(contents) {
                        if (timer) {
                            clearTimeout(timer);
                        }
                        let sendPostIt = $(this).closest('.postIt');
                        $(this).closest('.postIt').data('data-info').texto = contents;
                        timer = setTimeout(function() {
                            guardarPostIt(sendPostIt)
                        }, timeout);
                    }
                }
            });
            $('.note-btn').removeAttr('title');
            $('.nuevo-postIt').find('.note-editor').find('.note-toolbar').hide();
            $('.nuevo-postIt').find('.note-editor').css('margin-bottom', '0px');
            $('.nuevo-postIt').find('.note-editor').find('.note-editing-area').find('.note-editable').css('height', postIt.altura + 'px');
            //$('.nuevo-postIt').find('.note-editor').find('.note-editing-area').find('.note-editable').css('padding-top', 15 + 'px');
            $('.nuevo-postIt').find('.note-editor').find('.note-toolbar').css('position', 'absolute');
            $('.nuevo-postIt').find('.note-editor').find('.note-toolbar').css('z-index', '2000');
            $('.nuevo-postIt').find('.note-editor').find('.note-toolbar').css('left', '10px');
            $('.nuevo-postIt').find('.note-editor').find('.note-toolbar').css('top', '-45px');
            $('.nuevo-postIt').find('.note-editor').find('.note-toolbar').css('background-color', 'white');
            $('.nuevo-postIt').find('.note-editor').find('.note-toolbar').css('border', '0px');
            //
            // $('.nuevo-postIt').find('.note-editor').find('.note-editing-area').find('.note-editable').css('width', postIt.ancho + 'px');
            // $('.nuevo-postIt').find('.note-editor').find('.note-editing-area').find('.note-editable').css('height', postIt.altura + 'px');
            let pointerY;
            let pointerX;
            $('.nuevo-postIt').draggable({
                containment: '#mural',
                //handle: '.header-postIt',
                snap: true,
                snapTolerance: 5,
                start: function(event, ui) {
                    pointerY = (event.pageY - mural.offset().top) / mural.css('zoom') - parseInt($(event.target).css('top'));
                    pointerX = (event.pageX - mural.offset().left) / mural.css('zoom') - parseInt($(event.target).css('left'));
                },
                drag: function(event, ui) {
                    if (Math.round((event.pageX - mural.offset().left) / mural.css('zoom') - pointerX) > (mural.offset().left / mural.css('zoom'))) {
                        ui.position.left = Math.round((event.pageX - mural.offset().left) / mural.css('zoom') - pointerX);
                    }
                    if (Math.round((event.pageY - mural.offset().top) / mural.css('zoom') - pointerY) > (mural.offset().top / mural.css('zoom'))) {
                        ui.position.top = Math.round((event.pageY - mural.offset().top) / mural.css('zoom') - pointerY);
                    }
                    console.log('******DRAG******');
                    console.log('offsetX: ' + ui.offset.left);
                    console.log('offsetY: ' + ui.offset.top);
                    console.log('positionX: ' + ui.position.left);
                    console.log('positionY: ' + ui.position.top);
                    console.log('cssX: ' + ui.helper.css('left'));
                    console.log('cssY: ' + ui.helper.css('top'));
                },
                stop: function(event, ui) {
                    console.log('******STOP******')
                    $(this).data('data-info').posicionX = ui.helper.css('left').replace('px', '');
                    $(this).data('data-info').posicionY = ui.helper.css('top').replace('px', '');
                    console.log('offsetX: ' + ui.offset.left);
                    console.log('offsetY: ' + ui.offset.top);
                    console.log('positionX: ' + ui.position.left);
                    console.log('positionY: ' + ui.position.top);
                    console.log('cssX: ' + ui.helper.css('left'));
                    console.log('cssY: ' + ui.helper.css('top'));
                    guardarPostIt($(this));
                }
            });
            $('.nuevo-postIt').resizable();
            $('.nuevo-postIt').find('.postIt-texto').summernote('focus');
            $('.nuevo-postIt').find('.ui-resizable-handle').hide();
            $('.nuevo-postIt').data('data-info', postIt);
            $('.nuevo-postIt').find('.note-editor').find('.note-editing-area').find('.note-editable').css('background-color', postIt.colorFondo);
            $('.nuevo-postIt').find('.note-editor').find('.note-resizebar').hide();
            let colorPicker = '' +
                '<div class="note-btn-group btn-group note-style"> ' +
                    '<input type="color" class="colorPickerPostIt" ' +
                        'style="display: block; ' +
                        'position: absolute; ' +
                        'width: 5px; "' +
                        'title="Selector de color"> ' +
                    '<button type="button" ' +
                        'class="note-btn btn btn-default btn-sm note-btn-bold btnColorPickerPostIt" ' +
                        'tabindex="-1"> ' +
                            '<i class="fas fa-paint-brush"></i>' +
                    '</button>' +
                '</div>';
            $('.nuevo-postIt').find('.note-editor').find('.note-toolbar').append(colorPicker);
            let newPostIt = $('.nuevo-postIt');
            $('.nuevo-postIt').removeClass('nuevo-postIt');

            if (postIt.id == 0) {
                guardarPostIt(newPostIt);
            }
        }

        function agregarBloque(bloque) {
            let nuevoBloqueHTML = '' +
                '<div class="bloque bloque-nuevo" ' +
                    'style="background-color: ' + bloque.colorFondo + '; ' +
                    'height: ' + bloque.altura + 'px; ' +
                    'width: ' + bloque.ancho + 'px; ' +
                    'border: 1px solid black; ' +
                    'position: absolute; ' +
                    'left: ' + bloque.posicionX + 'px; ' +
                    'top: ' + bloque.posicionY + 'px;">' +
                    '<div class="bloque-header" ' +
                        'style="z-index: 1000; ' +
                        'position: absolute; ' +
                        'left: 0px; ' +
                        'top: -35px;">' +
                        '<input type="color" class="colorPickerBloque pull-left" ' +
                            'style="display: block; ' +
                            'position: absolute; ' +
                            'width: 5px; ' +
                            'z-index: -1; "' +
                            'title="Selector de color"> ' +
                        '<button type="button" ' +
                            'class="note-btn btn btn-default btn-sm note-btn-bold btnColorPickerBloque pull-left"> ' +
                                '<i class="fas fa-paint-brush"></i>' +
                        '</button>' +
                        '<i class="fas fa-times pull-right closeBloque"></i>' +
                    '</div>' +
                '</div>';

            mural.append(nuevoBloqueHTML);
            // $('.bloque-nuevo').find('.bloque-header').hide();
            $('.bloque-nuevo').find('.btnColorPickerBloque').hide();
            $('.bloque-nuevo').find('.colorPickerBloque').hide();

            let pointerY;
            let pointerX;
            $('.bloque-nuevo').draggable({
                containment: '#mural',
                //handle: '.bloque-header',
                // start: function(event, ui) {
                //     pointerY = (event.pageY - mural.offset().top) / mural.css('zoom') - parseInt($(event.target).css('top'));
                //     pointerX = (event.pageX - mural.offset().left) / mural.css('zoom') - parseInt($(event.target).css('left'));
                // },
                // drag: function(event, ui) {
                //     if (Math.round((event.pageX - mural.offset().left) / mural.css('zoom') - pointerX) > (mural.offset().left / mural.css('zoom'))) {
                //         ui.position.left = Math.round((event.pageX - mural.offset().left) / mural.css('zoom') - pointerX);
                //     }
                //     if (Math.round((event.pageY - mural.offset().top) / mural.css('zoom') - pointerY) > (mural.offset().top / mural.css('zoom'))) {
                //         ui.position.top = Math.round((event.pageY - mural.offset().top) / mural.css('zoom') - pointerY);
                //     }
                // },
                stop: function(event, ui) {
                    // console.log('***STOPBLOQUE***');
                    $(this).data('data-info').posicionX = ui.helper.css('left').replace('px', '');
                    $(this).data('data-info').posicionY = ui.helper.css('top').replace('px', '');
                    // console.log('offsetX: ' + ui.offset.left);
                    // console.log('offsetY: ' + ui.offset.top);
                    // console.log('positionX: ' + ui.position.left);
                    // console.log('positionY: ' + ui.position.top);
                    // console.log('cssX: ' + ui.helper.css('left'));
                    // console.log('cssY: ' + ui.helper.css('top'));
                    guardarBloque($(this));
                }
            });

            $('.bloque-nuevo').droppable({
                accept: '.postIt',
                drop: function(event, ui) {
                    $(ui)[0].draggable.data('data-info').idSeccion = $(this).data('data-info').id;
                    // if (mural.css('zoom') == 1)
                    // {
                    //     ui.draggable.detach().appendTo($(this));
                    //     $(ui)[0].draggable.css('left', Math.round($(ui)[0].offset.left - $(this).offset().left) + 'px');
                    //     $(ui)[0].draggable.css('top', Math.round($(ui)[0].offset.top) - $(this).offset().top + 'px');
                    // }
                    // else
                    // {
                    //     if ($(ui)[0].draggable.hasClass('enBloque'))
                    //     {
                    //         $(ui)[0].draggable.css('left', Math.round($(ui)[0].position.left) + 'px');
                    //         $(ui)[0].draggable.css('top', Math.round($(ui)[0].position.top) + 'px');
                    //         ui.draggable.detach().appendTo($(this));   
                    //     }
                    //     else
                    //     {
                    //         ui.draggable.detach().appendTo($(this));
                    //         $(ui)[0].draggable.css('left', Math.round($(ui)[0].position.left - $(this).offset().left) + 'px');
                    //         $(ui)[0].draggable.css('top', Math.round($(ui)[0].position.top) - $(this).offset().top + 'px');
                    //         $(ui)[0].draggable.addClass('enBloque');
                    //     }
                    // }
                    
                    // console.log('******DROP******');
                    ui.draggable.detach().appendTo($(this));

                    // ui.offset.left = ui.offset.left - $(this).offset().left;
                    // ui.offset.top = ui.offset.top - $(this).offset().top;
                    // ui.position.left = ui.position.left - $(this).offset().left;
                    // ui.position.top = ui.position.top - $(this).offset().top;
                    $(ui)[0].draggable.css('left', ui.offset.left - ($(this).offset().left + 1) + 'px');
                    $(ui)[0].draggable.css('top', ui.offset.top - ($(this).offset().top + 1) + 'px');
                    // console.log('newOffX: ' + ui.offset.left);
                    // console.log('newOffY: ' + ui.offset.top);
                    // console.log('newPositionX: ' + ui.position.left);
                    // console.log('newPositionY: ' + ui.position.top);
                    // console.log('draggableX: ' + $(ui)[0].draggable.css('left'));
                    // console.log('draggableY: ' + $(ui)[0].draggable.css('top'));
                },
                out: function(event, ui) {
                    ui.draggable.data('data-info').idSeccion = null;
                }
            });
            $('.bloque-nuevo').resizable();
            $('.bloque-nuevo').find('.ui-resizable-handle').hide();
            $('.bloque-nuevo').data('data-info', bloque);
            let newBloque = $('.bloque-nuevo');
            $('.bloque-nuevo').removeClass('bloque-nuevo');

            if (bloque.id == 0) {
                guardarBloque(newBloque);
            }
        }

        /* AJAX */
        function eliminarSeccion(idSeccion) {
            $.post('/Administrativo/Mural/EliminarSeccion',
            {
                idSeccion: idSeccion
            }).then(response => {
                if (response.Success) {
                }
                else {
                    AlertaGeneral(`Alerta`, response.Message);
                }
            }, error => {
                AlertaGeneral('Operación fallida', `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            });
        }

        function guardarBloque(bloque) {
            let info = $(bloque).data('data-info');
            $.post('/Administrativo/Mural/GuardarSeccion',
            {
                seccion: info
            }).then(response => {
                if (response.Success) {
                    $(bloque).data('data-info').id = response.Value;
                }
                else {
                    AlertaGeneral(`Alerta`, response.Message);
                }
            }, error => {
                AlertaGeneral('Operación fallida', `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            });
        }

        function eliminarPostIt(idPostIt) {
            $.post('/Administrativo/Mural/EliminarPostIt',
            {
                idPostIt: idPostIt
            }).then(response => {
                if (response.Success) {
                }
                else {
                    AlertaGeneral(`Alerta`, response.Message);
                }
            }, error => {
                AlertaGeneral('Operación fallida', `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            });
        }

        function guardarPostIt(postIt) {
            let info = $(postIt).data('data-info');
            $.post('/Administrativo/Mural/SavePostIt',
            {
                postIt: info
            }).then(response => {
                if (response.Success) {
                    $(postIt).data('data-info').id = response.Value;
                }
                else {
                    AlertaGeneral(`Alerta`, response.Message);
                }
            }, error => {
                AlertaGeneral('Operación fallida', `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            });
        }

        function crearMural() {
            $.post('/Administrativo/Mural/CrearMural',
                {
                    mural: objetoMural
                }).then(response => {
                    if (response.Success) {
                        mural.empty();
                        modalNewMural.modal('hide');

                        cboMural.fillCombo('/Administrativo/Mural/CboxMural', null, null, null, null);
                        cboMural.val(response.Value);
                        cboMural.trigger('change');

                        objetoMural.id = response.Value;
                        objetoMural.titulo = cboMural.find('option:selected').text();
                    }
                    else {
                        AlertaGeneral(`Alerta`, response.Message);
                    }
                }, error => {
                    AlertaGeneral('Operación fallida', `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
        }

        function guardarMural() {
            let postItList = new Array();

            $.each($('.postIt'), function(index, element) {
                $(element).data('data-info').texto = $(element).find('textarea').val();
                postItList.push($(element).data('data-info'));
            });

            $.post('/Administrativo/Mural/SaveMural',
                {
                    mural: objetoMural,
                    postIt: postItList
                }).then(response => {
                    if (response.Success) {
                        ConfirmacionGeneral('Operación correcta', 'Se guardó correctamente el mural');
                    }
                    else {
                        AlertaGeneral(`Alerta`, response.Message);
                    }
                }, error => {
                    AlertaGeneral('Operación fallida', `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
        }

        function obtenerMural() {
            $.get('/Administrativo/Mural/GetMural',
            {
                idMural: cboMural.val()
            }).then(response => {
                if (response.Success) {
                    mural.empty();

                    $.each(response.Value.PostIt, function(index, element) {

                        if (element.IdSeccion == null) {
                            let nuevoPostIt = Object.create(postIt);

                            nuevoPostIt.id = element.Id;
                            nuevoPostIt.idMural = response.Value.Mural.Id;
                            nuevoPostIt.texto = element.Texto;
                            nuevoPostIt.posicionX = element.PosicionX;
                            nuevoPostIt.posicionY = element.PosicionY;
                            nuevoPostIt.colorFondo = element.Color;
                            nuevoPostIt.altura = element.Altura;
                            nuevoPostIt.ancho = element.Ancho;

                            agregarPostIt(nuevoPostIt);
                        }
                    });

                    $.each(response.Value.Seccion, function(index, element) {

                        let nuevoBloque = Object.create(bloque);

                        nuevoBloque.id = element.Id;
                        nuevoBloque.idMural = response.Value.Mural.Id;
                        nuevoBloque.posicionX = element.PosicionX;
                        nuevoBloque.posicionY = element.PosicionY;
                        nuevoBloque.colorFondo = element.Color;
                        nuevoBloque.altura = element.Altura;
                        nuevoBloque.ancho = element.Ancho;

                        agregarBloque(nuevoBloque);
                        
                        $.each(response.Value.PostIt, function(index, element) {

                            if (element.IdSeccion != null) {
                                let nuevoPostIt = Object.create(postIt);
    
                                nuevoPostIt.id = element.Id;
                                nuevoPostIt.idMural = response.Value.Mural.Id;
                                nuevoPostIt.idSeccion = element.IdSeccion;
                                // console.log(nuevoPostIt.idSeccion);
                                nuevoPostIt.texto = element.Texto;
                                nuevoPostIt.posicionX = element.PosicionX;
                                nuevoPostIt.posicionY = element.PosicionY;
                                nuevoPostIt.colorFondo = element.Color;
                                nuevoPostIt.altura = element.Altura;
                                nuevoPostIt.ancho = element.Ancho;
    
                                agregarPostIt(nuevoPostIt);
                            }
                        });
                    });

                    objetoMural.id = response.Value.Mural.Id;
                    objetoMural.color = response.Value.Mural.Color;
                    objetoMural.titulo = response.Value.Mural.Titulo;
                }
                else {
                    AlertaGeneral('Alerta', response.Message);
                }
            }, error => {
                AlertaGeneral('Operación fallida', `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            });
        }

        (function init() {
            mural.droppable({
                accept: '.postIt',
                drop: function(event, ui) {
                    // if ($(ui)[0].draggable.parent().attr('id') == 'mural')
                    // {
                    //     $(ui)[0].draggable.removeClass('enBloque');
                    // }
                    // else
                    // {
                    //     if( mural.css('zoom') != 1)
                    //     {
                    //         $(ui)[0].draggable.css('left', $(ui)[0].draggable.parent().offset().left + $(ui)[0].position.left + 'px');
                    //         $(ui)[0].draggable.css('top', $(ui)[0].draggable.parent().offset().top + $(ui)[0].position.top + 'px');
                    //     }else{
                    //         $(ui)[0].draggable.css('left', Math.round($(ui)[0].offset.left) + 'px');
                    //         $(ui)[0].draggable.css('top', Math.round($(ui)[0].offset.top) + 'px');
                    //     }
                        
                    // }
                    ui.draggable.detach().appendTo($(this));
                    console.log('******MURAL******')
                    console.log(ui)
                    if (mural.css('zoom') != 1) {
                        $(ui)[0].draggable.css('left', ui.position.left + 'px');
                        $(ui)[0].draggable.css('top', ui.position.top + 'px');
                    }else {
                        $(ui)[0].draggable.css('left', ui.offset.left + 'px');
                        $(ui)[0].draggable.css('top', ui.offset.top + 'px');
                    }
                    console.log($(ui)[0].draggable.css('left'));
                    console.log($(ui)[0].draggable.css('top'));

                    // start: function(event, ui) {
                    //     pointerY = (event.pageY - mural.offset().top) / mural.css('zoom') - parseInt($(event.target).css('top'));
                    //     pointerX = (event.pageX - mural.offset().left) / mural.css('zoom') - parseInt($(event.target).css('left'));
                    // },
                    // drag: function(event, ui) {
                    //     if (Math.round((event.pageX - mural.offset().left) / mural.css('zoom') - pointerX) > (mural.offset().left / mural.css('zoom'))) {
                    //         ui.position.left = Math.round((event.pageX - mural.offset().left) / mural.css('zoom') - pointerX);
                    //     }
                    //     if (Math.round((event.pageY - mural.offset().top) / mural.css('zoom') - pointerY) > (mural.offset().top / mural.css('zoom'))) {
                    //         ui.position.top = Math.round((event.pageY - mural.offset().top) / mural.css('zoom') - pointerY);
                    //     }
                    //     console.log('******DRAG******');
                    //     console.log('offsetX: ' + ui.offset.left);
                    //     console.log('offsetY: ' + ui.offset.top);
                    //     console.log('positionX: ' + ui.position.left);
                    //     console.log('positionY: ' + ui.position.top);
                    //     console.log('cssX: ' + ui.helper.css('left'));
                    //     console.log('cssY: ' + ui.helper.css('top'));
                    // },
                    // stop: function(event, ui) {
                    //     console.log('******STOP******')
                    //     $(this).data('data-info').posicionX = ui.helper.css('left').replace('px', '');
                    //     $(this).data('data-info').posicionY = ui.helper.css('top').replace('px', '');
                    //     console.log('offsetX: ' + ui.offset.left);
                    //     console.log('offsetY: ' + ui.offset.top);
                    //     console.log('positionX: ' + ui.position.left);
                    //     console.log('positionY: ' + ui.position.top);
                    //     console.log('cssX: ' + ui.helper.css('left'));
                    //     console.log('cssY: ' + ui.helper.css('top'));
                    //     guardarPostIt($(this));
                }
            });

            cboMural.fillCombo('/Administrativo/Mural/CboxMural', null, null, null);
        })();

    }
    $(document).ready(() => RecursosHumanos.Mural = new Mural())
        // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        // .ajaxStop($.unblockUI);
})();