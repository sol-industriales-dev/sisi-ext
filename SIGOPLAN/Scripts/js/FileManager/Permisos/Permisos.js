(() => {

    $.namespace('FileManager.Permisos');

    Permisos = function () {

        // Variables.
        const inputUsuario = $('#inputUsuario');
        const botonBuscar = $('#botonBuscar');
        const spanNumeroArchivos = $('#spanNumeroArchivos');
        const treeGridDiv = $('#treeGridDiv');
        const botonGuardar = $('#botonGuardar');
        const divBotonGuardar = $('#divBotonGuardar');
        const botonModalGuardado = $("#botonModalGuardado");
        const botonColapsar = $('#botonColapsar');
        const botonExpandir = $('#botonExpandir');

        //// Opciones avanzadas
        const botonOpcionesAvanzadas = $('#botonOpcionesAvanzadas');
        const modalOpcionesAvanzadas = $('#modalOpcionesAvanzadas');
        const divRadioOpciones = $('#divRadioOpciones');
        const divPermisoEspecial = $('#divPermisoEspecial');
        const divEstructura = $('#divEstructura');

        // Crear estructura
        const comboDivision = $('#comboDivision');
        const comboCC = $('#comboCC');
        const botonAgregarEstructura = $('#botonAgregarEstructura');

        // Permisos especiales
        const divRadioOpcionesEspeciales = $('#divRadioOpcionesEspeciales');
        const divNivelDivision = $('#divNivelDivision');
        const divNivelSubdivision = $('#divNivelSubdivision');
        const divNivelObra = $('#divNivelObra');
        const comboDivisionNivelDivision = $('#comboDivisionNivelDivision');
        const comboSubdivision = $('#comboSubdivision');
        const comboDivisionNivelObra = $('#comboDivisionNivelObra');
        const botonCargarCCPorDivision = $('#botonCargarCCPorDivision');
        const divComboObraNivelObra = $('#divComboObraNivelObra');
        const comboObraNivelObra = $('#comboObraNivelObra');
        const inputUsuarioEspecial = $('#inputUsuarioEspecial');
        const botonGuardarPermisosEspeciales = $('#botonGuardarPermisosEspeciales');

        // Privilegios permiso especial
        const checkboxEspecialSubir = $('#checkboxEspecialSubir');
        const checkboxEspecialCrear = $('#checkboxEspecialCrear');
        const checkboxEspecialDescargarCarpeta = $('#checkboxEspecialDescargarCarpeta');
        const checkboxEspecialEliminar = $('#checkboxEspecialEliminar');
        const checkboxEspecialActualizar = $('#checkboxEspecialActualizar');
        const checkboxEspecialDescargarArchivo = $('#checkboxEspecialDescargarArchivo');

        // Permiso seguridad
        const divPermisoSeguridad = $('#divPermisoSeguridad');
        const inputUsuarioSeguridad = $('#inputUsuarioSeguridad');
        const comboTipoPermisoSeguridad = $('#comboTipoPermisoSeguridad');
        const botonGuardarPermisoSeguridad = $('#botonGuardarPermisoSeguridad');

        // Eliminar permisos
        const divEliminarPermisos = $('#divEliminarPermisos');
        const comboUsuarioPermisoEspecial = $('#comboUsuarioPermisoEspecial');
        const botonEliminarPermisoEspecial = $('#botonEliminarPermisoEspecial');

        // Modal aviso
        const modalAviso = $('#modalAviso');
        const botonModalAviso = $('#botonModalAviso');

        function init() {
            inputUsuario.getAutocompleteValid(setUsuarioID, validarUsuarioID, null, '/FileManager/FileManager/ObtenerUsuariosAutocompletado');
            inputUsuarioEspecial.getAutocompleteValid(setUsuarioID, validarUsuarioID, null, '/FileManager/FileManager/ObtenerUsuariosAutocompletado');
            inputUsuarioSeguridad.getAutocompleteValid(setUsuarioID, validarUsuarioID, null, '/FileManager/FileManager/ObtenerUsuariosAutocompletado');
            llenarCombos();
            agregarListeners();
            ocultarElementos();
        }

        function ocultarElementos() {
            treeGridDiv.hide();
            divBotonGuardar.hide();
            divPermisoEspecial.hide();
            divNivelSubdivision.hide();
            divNivelObra.hide();
            divComboObraNivelObra.hide();
            divPermisoSeguridad.hide();
            divEliminarPermisos.hide();
        }

        function llenarCombos() {
            // Combos de modal Opciones Avanzadas

            // Agregar estructura
            // comboDivision.fillCombo('ObtenerDivisiones', null, false, "Seleccione una división");
            // comboCC.fillCombo('ObtenerObras', null, false, 'Seleccione una obra');

            // Permisos especiales
            // comboDivisionNivelDivision.fillCombo('ObtenerDivisiones', null, false, "Todas");
            // convertToMultiselect("#comboDivisionNivelDivision");

            // comboSubdivision.fillCombo('ObtenerSubdivisionesPermiso', null, false, 'Todas');
            // convertToMultiselect("#comboSubdivision");

            // comboDivisionNivelObra.fillCombo('ObtenerDivisiones', null, false, "Todas");
            // convertToMultiselect("#comboDivisionNivelObra");

            // comboUsuarioPermisoEspecial.fillCombo('ObtenerUsuariosPermisosEspeciales', null, false);
        }

        function agregarListeners() {

            // Modal aviso
            modalAviso.on('shown.bs.modal', () => {
                setTimeout(() => botonModalAviso.focus(), 100);
            });

            botonBuscar.click(() => obtenerEstructuraPermisos(inputUsuario.data().usuarioID));

            botonOpcionesAvanzadas.click(() => modalOpcionesAvanzadas.modal('show'));
            divRadioOpciones.find('a').click(toggleRadioButton);
            divRadioOpcionesEspeciales.find('a').click(toggleRadioButtonPermisos)

            botonGuardar.click(guardarPermisos);

            botonModalGuardado.click(() => {
                $.ui.fancytree.getTree().clear();
                treeGridDiv.hide(1000);
                divBotonGuardar.hide(1000);
                spanNumeroArchivos.html('');
                inputUsuario.val('');
                $("#treeGridArea").data().usuarioID = 0;
            });

            botonAgregarEstructura.click(agregarEstructura);

            botonCargarCCPorDivision.click(ObtenerObrasPorDivision);

            botonGuardarPermisosEspeciales.click(guardarPermisosEspeciales);
            botonGuardarPermisoSeguridad.click(guardarPermisoSeguridad);

            modalOpcionesAvanzadas.on("hide.bs.modal", limpiarCamposModalOpcionesAvanzadas);

            botonColapsar.click(() => {
                const rootNode = $("#treeGridArea").fancytree("getRootNode").getFirstChild();
                if (rootNode) {
                    rootNode.findAll(node => node.setExpanded(false));
                }
            });

            botonExpandir.click(() => {
                const rootNode = $("#treeGridArea").fancytree("getRootNode").getFirstChild();
                if (rootNode) {
                    rootNode.findAll(node => node.setExpanded(true));
                }
            });

            botonEliminarPermisoEspecial.click(eliminarPermisoEspecial);
        }

        function eliminarPermisoEspecial() {
            const permisoID = comboUsuarioPermisoEspecial.val();

            if (permisoID == "") {
                mostrarModal('Aviso', "Debe seleccionar un permiso a eliminar.");
                return;
            }

            $.blockUI({ message: 'Eliminando permiso especial...' });
            $.post('EliminarPermisoEspecial', { permisoID })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        modalOpcionesAvanzadas.modal('hide');
                        mostrarModal('Aviso', "Permiso eliminado correctamente.");
                        // comboUsuarioPermisoEspecial.fillCombo('ObtenerUsuariosPermisosEspeciales', null, false);
                        return;
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function limpiarCamposModalOpcionesAvanzadas() {

            // Modal Opciones Avanzadas
            divRadioOpciones.find('a').removeClass('active');
            divRadioOpciones.find('a').first().addClass('active');
            divPermisoEspecial.hide();
            divPermisoSeguridad.hide();
            divEliminarPermisos.hide();
            divEstructura.show();

            // Cargar estructura
            comboCC.val('Seleccione una obra');
            comboDivision.val('Seleccione una división');

            // Permisos especiales
            divRadioOpcionesEspeciales.find('a').removeClass('active');
            divRadioOpcionesEspeciales.find('a').first().addClass('active');
            inputUsuarioEspecial.val('');
            divNivelSubdivision.hide();
            divNivelObra.hide();
            divNivelDivision.show();
            divComboObraNivelObra.hide();
            limpiarMultiselect('#comboDivisionNivelDivision');
            limpiarMultiselect('#comboDivisionNivelObra');
            limpiarMultiselect('#comboSubdivision');
            limpiarMultiselect('#comboObraNivelObra');

            // Permisos seguridad
            inputUsuarioSeguridad.val('');
            comboTipoPermisoSeguridad.val(3);

            // Checkboxes permiso especial
            checkboxEspecialSubir[0].checked = false;
            checkboxEspecialCrear[0].checked = false;
            checkboxEspecialDescargarCarpeta[0].checked = false;
            checkboxEspecialEliminar[0].checked = false;
            checkboxEspecialActualizar[0].checked = false;
            checkboxEspecialDescargarArchivo[0].checked = false;

            // Eliminar permiso
            comboUsuarioPermisoEspecial[0].selectedIndex = 0;
        }

        function ObtenerObrasPorDivision() {
            const listaDivisionesIDs = getValoresMultiples('#comboDivisionNivelObra');
            if (listaDivisionesIDs.length > 0) {
                $.blockUI({ message: 'Filtrando...' });
                // comboObraNivelObra.fillComboAsync('ObtenerObrasPorDivision', { listaDivisionesIDs }, false, 'Todos', () => {
                //     $.unblockUI();
                //     divComboObraNivelObra.show(500);
                //     convertToMultiselect("#comboObraNivelObra");
                // });
            } else {
                AlertaGeneral('Aviso', 'Debe seleccionar por lo menos una división para cargar sus centros de costos correspondientes.');
                divComboObraNivelObra.hide(200);
            }
        }

        function toggleRadioButton(e) {
            const option = $(e.currentTarget).addClass('active').attr('option');
            divRadioOpciones.find(`a[option!="${option}"]`).removeClass('active');
            alternarDivOpcionesEspeciales();
        }

        function toggleRadioButtonPermisos(e) {
            const option = $(e.currentTarget).addClass('active').attr('specialOption');
            divRadioOpcionesEspeciales.find(`a[specialOption!="${option}"]`).removeClass('active');
            alternarDivOpcionesPermisosEspeciales();
        }

        function alternarDivOpcionesEspeciales() {
            const option = $('#divRadioOpciones a.active').attr('option');
            if (option === "1") {
                divEstructura.show(500);
                divPermisoEspecial.hide(500);
                divPermisoSeguridad.hide(500);
                divEliminarPermisos.hide(500);
            } else if (option === "2") {
                divPermisoEspecial.show(500);
                divEstructura.hide(500);
                divPermisoSeguridad.hide(500);
                divEliminarPermisos.hide(500);
            } else if (option === "3") {
                divPermisoSeguridad.show(500);
                divEstructura.hide(500);
                divPermisoEspecial.hide(500);
                divEliminarPermisos.hide(500);
            } else {
                divEliminarPermisos.show(500);
                divPermisoEspecial.hide(500);
                divEstructura.hide(500);
                divPermisoSeguridad.hide(500);
            }
        }

        function alternarDivOpcionesPermisosEspeciales() {
            const option = divRadioOpcionesEspeciales.find('a.active').attr('specialOption');
            if (option === "1") {
                divNivelSubdivision.hide(500);
                divNivelObra.hide(500);
                divNivelDivision.show(500);
            } else if (option === "2") {
                divNivelDivision.hide(500);
                divNivelObra.hide(500);
                divNivelSubdivision.show(500);
            }
            else {
                divNivelDivision.hide(500);
                divNivelSubdivision.hide(500);
                divNivelObra.show(500);
            }
        }

        function agregarEstructura() {
            if (comboDivision.val() === 'Seleccione una división') {
                mostrarModal('Aviso', "Debe seleccionar una división.");
                return;
            } else if (comboCC.val() === 'Seleccione una obra') {
                mostrarModal('Aviso', "Debe seleccionar una obra.");
                return;
            }

            const divisionID = comboDivision.val();
            const ccID = comboCC.val();

            $.blockUI({ message: 'Creando estructura. Esta operación puede tardar un poco.' });
            $.post('CrearEstructuraObra', { divisionID, ccID })
                .then(response => {
                    if (response.success) {
                        mostrarModal('Éxito', 'Estructura creada exitosamente.');
                        // comboCC.fillCombo('ObtenerObras', null, false, 'Seleccione una obra');
                        comboCC.val('Seleccione una obra');
                        comboDivision.val('Seleccione una división');
                        modalOpcionesAvanzadas.modal('hide');
                    } else {
                        mostrarModal('Error', `Ocurrió un error: ${response.message}`);
                    }
                }, () => mostrarModal('Error', 'Ocurrió un error al intentar crear la estructura de la obra.'))
                .then($.unblockUI);
        }

        function guardarPermisosEspeciales() {

            const usuarioID = inputUsuarioEspecial.data().usuarioID;

            let permisosEspeciales = [];

            const puedeSubir = checkboxEspecialSubir[0].checked;
            const puedeCrear = checkboxEspecialCrear[0].checked;
            const puedeDescargarCarpeta = checkboxEspecialDescargarCarpeta[0].checked;
            const puedeEliminar = checkboxEspecialEliminar[0].checked;
            const puedeActualizar = checkboxEspecialActualizar[0].checked;
            const puedeDescargarArchivo = checkboxEspecialDescargarArchivo[0].checked;

            const checkboxes = [puedeSubir, puedeCrear, puedeDescargarCarpeta, puedeEliminar, puedeActualizar, puedeDescargarArchivo];

            if (usuarioID == null) {
                mostrarModal('Aviso', 'Debe seleccionar el usuario al cual le dará permiso especial.');
                return;
            } else if (checkboxes.filter(x => x).length === 0) {
                mostrarModal('Aviso', 'Debe seleccionar por lo menos un privilegio para guardar el permiso especial.');
                return;
            }

            // Validación dependiendo del tipo de permiso.
            const nivelPermiso = divRadioOpcionesEspeciales.find('a.active').attr('specialOption');

            if (nivelPermiso === "1") {
                const listaDivisionesIDs = getValoresMultiples('#comboDivisionNivelDivision');
                if (listaDivisionesIDs.length > 0) {
                    const tipoPermiso = 1;
                    permisosEspeciales = listaDivisionesIDs.map(entidadID => {
                        return {
                            usuarioID,
                            tipoPermiso,
                            entidadID,
                            puedeSubir,
                            puedeEliminar,
                            puedeDescargarArchivo,
                            puedeDescargarCarpeta,
                            puedeActualizar,
                            puedeCrear
                        }
                    });
                } else {
                    mostrarModal('Aviso', 'Debe seleccionar por lo menos una división.');
                    return;
                }
            } else if (nivelPermiso === "2") {
                const listaSubdivisionesIDs = getValoresMultiples('#comboSubdivision');
                if (listaSubdivisionesIDs.length > 0) {
                    const tipoPermiso = 6;
                    permisosEspeciales = listaSubdivisionesIDs.map(entidadID => {
                        return {
                            usuarioID,
                            tipoPermiso,
                            entidadID,
                            puedeSubir,
                            puedeEliminar,
                            puedeDescargarArchivo,
                            puedeDescargarCarpeta,
                            puedeActualizar,
                            puedeCrear
                        }
                    });
                } else {
                    mostrarModal('Aviso', 'Debe seleccionar por lo menos una subdivisión.');
                    return;
                }
            } else {
                const listaObrasIDs = getValoresMultiples('#comboObraNivelObra');
                if (listaObrasIDs.length > 0) {
                    const tipoPermiso = 2;
                    permisosEspeciales = listaObrasIDs.map(entidadID => {
                        return {
                            usuarioID,
                            tipoPermiso,
                            entidadID,
                            puedeSubir,
                            puedeEliminar,
                            puedeDescargarArchivo,
                            puedeDescargarCarpeta,
                            puedeActualizar,
                            puedeCrear
                        }
                    });
                } else {
                    mostrarModal('Aviso', 'Debe seleccionar por lo menos un centro de costos.');
                    return;
                }
            }
            $.blockUI({ message: 'Guardando permiso especial. Esta operación puede tardar un poco.' });
            $.post('GuardarPermisosEspeciales', { permisosEspeciales })
                .always($.unblockUI)
                .done(response => {
                    if (response.success) {
                        mostrarModal('Aviso', 'Permiso especial guardado con éxito.');
                        modalOpcionesAvanzadas.modal('hide');
                        // comboUsuarioPermisoEspecial.fillCombo('ObtenerUsuariosPermisosEspeciales', null, false);
                    } else {
                        mostrarModal('Aviso', response.message);
                    }
                })
                .fail(() => mostrarModal('Aviso', 'Ocurrió un error al intentar guardar el permiso especial.'));
        }

        function guardarPermisoSeguridad() {

            const usuarioID = inputUsuarioSeguridad.data().usuarioID;

            if (usuarioID == null) {
                mostrarModal('Aviso', 'Debe seleccionar el usuario al cual le dará permiso de seguridad.');
                return;
            }

            const permisosEspeciales = [{
                usuarioID,
                tipoPermiso: comboTipoPermisoSeguridad.val()
            }];
            $.blockUI({ message: 'Guardando permiso de seguridad. Esta operación puede tardar un poco.' });
            $.post('GuardarPermisosEspeciales', { permisosEspeciales })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        mostrarModal('Aviso', 'Permiso de seguridad guardado con éxito.');
                        modalOpcionesAvanzadas.modal('hide');
                        // comboUsuarioPermisoEspecial.fillCombo('ObtenerUsuariosPermisosEspeciales', null, false);
                    } else {
                        // Operación no completada.
                        mostrarModal(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    mostrarModal(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function mostrarModal(titulo, cuerpo) {
            $("#modalAvisoTitutlo").html(titulo);
            $("#modalAvisoCuerpo").text(cuerpo);
            modalAviso.modal('show');
        }

        function mostrarModalGuardado(titulo, cuerpo) {
            const modal = $("#modalGuardado");
            $("#modalGuardadoTitutlo").html(titulo);
            $("#modalGuardadoCuerpo").text(cuerpo);
            modal.modal({
                backdrop: 'static',
                keyboard: false
            });
        }

        function mostrarArchivos(archivos) {

            $("#treeGridArea").fancytree({
                checkbox: true,
                quicksearch: true,
                source: archivos,
                extensions: ["table"],
                table: {
                    indentation: 20,      // indent 20px per node level
                    nodeColumnIdx: 0,     // render the node title into the 1st column
                },
                selectMode: 3,
                renderColumns: (event, data) => {

                    const $tdList = $(data.node.tr).find(">td");
                    const { permisos } = data.node.data;

                    if (data.node.folder) {
                        $tdList.eq(1).html(`<input tipoPermiso='subir' ${data.node.data.partsel || data.node.selected ? '' : 'class="disabled"'} type='checkbox' ${permisos.puedeSubir ? 'checked' : ''}>`);
                        $tdList.eq(2).html(`<input tipoPermiso='crear' ${data.node.data.partsel || data.node.selected ? '' : 'class="disabled"'} type='checkbox' ${permisos.puedeCrear ? 'checked' : ''}>`);
                        $tdList.eq(3).html(`<input tipoPermiso='descargarCarpeta' ${data.node.data.partsel || data.node.selected ? '' : 'class="disabled"'} type='checkbox' ${permisos.puedeDescargarCarpeta ? 'checked' : ''}>`);
                        $tdList.eq(5).html(``);
                        $tdList.eq(6).html(``);
                    } else {
                        $tdList.eq(1).html(``);
                        $tdList.eq(2).html(``);
                        $tdList.eq(3).html(``);
                        $tdList.eq(5).html(`<input tipoPermiso='actualizar' ${data.node.data.partsel || data.node.selected ? '' : 'class="disabled"'} type='checkbox' ${permisos.puedeActualizar ? 'checked' : ''}>`);
                        $tdList.eq(6).html(`<input tipoPermiso='descargarArchivo' ${data.node.data.partsel || data.node.selected ? '' : 'class="disabled"'} type='checkbox' ${permisos.puedeDescargarArchivo ? 'checked' : ''}>`);
                    }
                    $tdList.eq(4).html(`<input tipoPermiso='eliminar' ${data.node.data.partsel || data.node.selected ? '' : 'class="disabled"'} type='checkbox' ${permisos.puedeEliminar ? 'checked' : ''}>`);
                },
                select: (e, data) => {
                    data.node.setActive();
                    enableCheckboxes();
                    $('#treeGridArea th:not(#thArchivos)')
                        .toArray()
                        .forEach(th => {
                            const tipoPermiso = $(th).attr('tipoPermiso');
                            updateColumnHeaderClass(tipoPermiso);
                        });
                },
                dblclick: (e, data) => {
                    data.node.toggleSelected();
                },
                keydown: (e, data) => {
                    if (e.which === 32) {
                        data.node.toggleSelected();
                        return false;
                    }
                },
                lazyLoad: function (event, data) {

                    var node = data.node;
                    // Load child nodes via Ajax GET /getTreeData?mode=children&parent=1234
                    data.result = {
                        url: "ObtenerEstructuraCarpetaPermisos",
                        data: { userID: inputUsuario.data().usuarioID, folderID: node.key },
                        cache: false
                    };
                },
                renderNode: function (event, data) {
                    updateTreeFileCounter();
                    setCheckboxesListener();
                    setHeadersListeners();
                }
            });

            let rootNode = $("#treeGridArea").fancytree("getRootNode");
            rootNode.removeChildren();
            rootNode.addChildren(archivos);
            $("#treeGridArea").data().usuarioID = inputUsuario.data().usuarioID;
            updateTreeFileCounter();
            treeGridDiv.show(1000);
            divBotonGuardar.show(1000);
            setCheckboxesListener();
            setHeadersListeners();
        }

        function setHeadersListeners() {
            $('#treeGridArea th:not(#thArchivos)')
                .toArray()
                .forEach(th => {
                    const tipoPermiso = $(th).attr('tipoPermiso');
                    updateColumnHeaderClass(tipoPermiso);
                });
        }

        function updateTreeFileCounter() {
            spanNumeroArchivos.html($.ui.fancytree.getTree().count());
        }

        function updateColumnHeaderClass(tipoPermiso) {
            const checkboxesVisibles = $(`#treeGridArea input[tipoPermiso="${tipoPermiso}"]`).toArray().filter(input => !($(input).hasClass('disabled')));
            const todosActivos = checkboxesVisibles.length === 0 ? false : checkboxesVisibles.length === checkboxesVisibles.filter(checkbox => checkbox.checked).length;
            const column = $(`#treeGridArea th[tipoPermiso="${tipoPermiso}"]`);
            todosActivos ? column.addClass('thSelected') : column.removeClass('thSelected');
        }

        function setCheckboxesListener() {
            $('#treeGridArea input[type="checkbox"]').click(e => {
                const node = $.ui.fancytree.getNode(e);
                const tipoPermiso = $(e.currentTarget).attr('tipoPermiso');
                actualizarPermisosNodo(tipoPermiso, node, e.currentTarget.checked);
                updateColumnHeaderClass(tipoPermiso);
            });

            // Click a la columna
            $('#treeGridArea th:not(#thArchivos)').unbind().click(e => {

                const th = $(e.currentTarget);
                // Toggles the class
                th.hasClass('thSelected') ? th.removeClass('thSelected') : th.addClass('thSelected');
                // Checks if it should select or deselect all the checkboxes.
                const isSelectingAll = th.hasClass('thSelected');
                const tipoPermiso = $(e.currentTarget).attr('tipoPermiso');
                $(`#treeGridArea input[tipoPermiso="${tipoPermiso}"]`)
                    .toArray()
                    .filter(input => !($(input).hasClass('disabled')))
                    .forEach(checkbox => {
                        const node = $.ui.fancytree.getNode(checkbox);
                        checkbox.checked = isSelectingAll;
                        actualizarPermisosNodo(tipoPermiso, node, isSelectingAll);
                    });
            });
        }

        function actualizarPermisosNodo(tipoPermiso, node, checked) {
            const { permisos } = node.data;
            switch (tipoPermiso) {
                case 'subir':
                    permisos.puedeSubir = checked;
                    break;
                case 'crear':
                    permisos.puedeCrear = checked;
                    break;
                case 'descargarCarpeta':
                    permisos.puedeDescargarCarpeta = checked;
                    break;
                case 'eliminar':
                    permisos.puedeEliminar = checked;
                    break;
                case 'actualizar':
                    permisos.puedeActualizar = checked;
                    break;
                case 'descargarArchivo':
                    permisos.puedeDescargarArchivo = checked;
                    break;
            }
        }

        function enableCheckboxes() {
            const rootNode = $("#treeGridArea").fancytree("getRootNode");

            rootNode.findAll(node => {
                if (node.selected && node.partsel) {
                    updateCheckboxArray(node, input => $(input).removeClass('disabled'));
                } else {
                    updateCheckboxArray(node, input => {
                        input.checked = false;
                        $(input).addClass('disabled')
                    });
                }
            });
        }

        function updateCheckboxArray(node, callback) {
            if (callback && (typeof callback === 'function')) {
                $(node.tr).find('input[type="checkbox"]').toArray().forEach(callback);
            }
        }

        function obtenerEstructuraPermisos(userID) {
            if (userID == null) {
                mostrarModal('Aviso', 'Debe seleccionar a un usuario.');
                return;
            }
            $.blockUI({ message: 'Cargando estructura de datos. Esta operación puede tardar un poco.' });
            $.get('ObtenerEstructuraPermisos', { userID })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        let carpetaBase = response.contenedor;
                        mostrarArchivos(carpetaBase);
                        enableCheckboxes();
                    } else {
                        spanNumeroArchivos.html("");
                        mostrarModal("Error", `Error al cargar la estructura de carpetas: ${response.message}`);
                    }
                }, () => mostrarModal("Error", "Error al cargar la estructura de carpetas."));
        }

        function guardarPermisos() {

            const tree = $.ui.fancytree.getTree();
            const lstArchivos = tree.findAll('').map(node => {
                // if (node.selected) {
                if (node.partsel || node.selected) {
                    return {
                        partsel: node.partsel,
                        selected: node.selected,
                        key: node.key,
                        permisos: node.data.permisos
                    }
                }
                // }
            });

            //#region VERSION ANTERIOR
            // const archivos = tree.findAll('').map(node => {
            //     if (node.partsel || node.selected) {
            //         return {
            //             partsel: node.partsel,
            //             selected: node.selected,
            //             key: node.key,
            //             permisos: node.data.permisos
            //         }
            //     } else {
            //         return {
            //             partsel: false,
            //             selected: false,
            //             key: node.key,
            //             permisos: {
            //                 puedeSubir: false,
            //                 puedeEliminar: false,
            //                 puedeDescargarArchivo: false,
            //                 puedeDescargarCarpeta: false,
            //                 puedeCrear: false,
            //                 puedeActualizar: false
            //             }
            //         }
            //     }
            // });
            //#endregion

            let obj = {};
            let archivos = [];
            lstArchivos.forEach(element => {
                if (element != undefined) {
                    obj = {};
                    obj.partsel = element.partsel;
                    obj.selected = element.selected;
                    obj.key = element.key;
                    obj.permisos = element.permisos;
                    archivos.push(obj);
                }
            });

            const usuarioID = $("#treeGridArea").data().usuarioID
            $.blockUI({ message: 'Guardando cambios. Esta operación puede tardar un poco.' });
            $.post('GuardarPermisos', { usuarioID, archivos }).then(response => {
                if (response.success) {
                    mostrarModalGuardado("Éxito", "Los cambios se guardaron correctamente.");
                } else {
                    mostrarModal("Error", `Ocurrió un error al intentar guardar los cambios: ${response.message}`);
                }
            }, () => mostrarModal("Error", "Ocurrió un error al intentar guardar los cambios.")).always(() => $.unblockUI());
        }

        function setUsuarioID(e, ul) {
            $(this).data().usuarioID = ul.item.id;
        }

        function validarUsuarioID(e, ul) {
            if (ul.item == null) {
                $(this).val('');
                $(this).data().usuarioID = null;
            }
        }

        init();

    };
    $(document).ready(() => FileManager.Permisos = new Permisos());
})();