(() => {
    $.namespace('Barrenacion.CatalogoPieza');

    CatalogoPieza = function () {

        // Variables.
        const itemsBroca = $('#itemsBroca');
        const spanBroca = $('#spanBroca');
        const itemsMartillo = $('#itemsMartillo');
        const spanMartillo = $('#spanMartillo');
        const itemsBarra = $('#itemsBarra');
        const spanBarra = $('#spanBarra');
        const itemsCulata = $('#itemsCulata');
        const spanCulata = $('#spanCulata');
        const itemsPortabit = $('#itemsPortabit');
        const spanPortabit = $('#spanPortabit');
        const itemsCilindro = $('#itemsCilindro');
        const spanCilindro = $('#spanCilindro');
        const itemsBarraSegunda = $("#itemsBarraSegunda");
        const spanBarraSegunda = $("#spanBarraSegunda");

        const itemsZanco = $('#itemsZanco');
        const spanZanco = $('#spanZanco');

        // Modal nuevo insumo
        const modalNuevoInsumo = $('#modalNuevoInsumo');
        const botonAgregarInsumo = $('#botonAgregarInsumo');
        const inputInsumo = $('#inputInsumo');
        const inputDescripcion = $('#inputDescripcion');
        const divTipoBroca = $('#divTipoBroca');
        const comboTipoBroca = $('#comboTipoBroca');

        // Modal eliminar insumo
        const modalEliminarInsumo = $('#modalEliminarInsumo');
        const botonEliminarInsumo = $('#botonEliminarInsumo');

        const comboAC = $('#comboAC');

        (function init() {
            // Lógica de inicialización.
            comboAC.fillCombo('/Barrenacion/ObtenerAC', null, false, null);
            obtenerInsumosPieza();
            agregarListeners();
            mostrarPaneles();
            cargarTiposBroca();
            divTipoBroca.hide();


        })();

        function cargarTiposBroca() {
            $.post('/Barrenacion/ObtenerTiposBroca')
                .then(response => {
                    if (response) {
                        // Operación exitosa.
                        response.reverse().pop();
                        const tiposBrocaHTML = response.map(item => `<option value=${item.Value} >${item.Text}</option>`).join('');
                        comboTipoBroca.append(tiposBrocaHTML);
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

        function mostrarPaneles() {
            $('div.container div.panel.panel-primary').hide();
            let counter = 0;
            $('div.container div.panel.panel-primary').toArray().forEach(panel => {
                setTimeout(() => {
                    $(panel).show(500);
                }, counter);
                counter += 500;
            });
        }

        // Métodos.
        function obtenerInsumosPieza() {
            limpiarItems();
            $.blockUI({ message: 'Cargando insumos...' });
            $.get('/Barrenacion/ObtenerInsumosPorPieza', { areaCuenta: comboAC.val() })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        mostrarInsumosPieza(response);
                        addOnClickEvent();
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
        }

        function mostrarInsumosPieza(listaInsumosPieza) {

            if (listaInsumosPieza.Broca && listaInsumosPieza.Broca.length > 0) {
                listaInsumosPieza.Broca.forEach(insumo => {
                    itemsBroca.append(`
                        <button data-id=${insumo.id} type="button" class="list-group-item">${insumo.descripcion} Medida:${insumo.tipoBroca}
                            <i class="fas fa-minus-circle pull-right"></i>
                        </button>`
                    );
                });
                spanBroca.text(listaInsumosPieza.Broca.length);
            }

            if (listaInsumosPieza.Martillo && listaInsumosPieza.Martillo.length > 0) {
                listaInsumosPieza.Martillo.forEach(insumo => {
                    itemsMartillo.append(`
                        <button data-id=${insumo.id} type="button" class="list-group-item">${insumo.descripcion}
                            <i class="fas fa-minus-circle pull-right"></i>
                        </button>`
                    );
                });
                spanMartillo.text(listaInsumosPieza.Martillo.length);
            }

            if (listaInsumosPieza.Barra && listaInsumosPieza.Barra.length > 0) {
                listaInsumosPieza.Barra.forEach(insumo => {
                    itemsBarra.append(`
                        <button data-id=${insumo.id} type="button" class="list-group-item">${insumo.descripcion}
                            <i class="fas fa-minus-circle pull-right"></i>
                        </button>`
                    );
                });
                spanBarra.text(listaInsumosPieza.Barra.length);
            }

            if (listaInsumosPieza.Barra_Segunda && listaInsumosPieza.Barra_Segunda.length > 0) {
                listaInsumosPieza.Barra_Segunda.forEach(insumo => {
                    itemsBarraSegunda.append(`
                        <button data-id=${insumo.id} type="button" class="list-group-item">${insumo.descripcion}
                            <i class="fas fa-minus-circle pull-right"></i>
                        </button>`
                    );
                });
                spanBarraSegunda.text(listaInsumosPieza.Barra.length);
            }

            if (listaInsumosPieza.Culata && listaInsumosPieza.Culata.length > 0) {
                listaInsumosPieza.Culata.forEach(insumo => {
                    itemsCulata.append(`
                        <button data-id=${insumo.id} type="button" class="list-group-item">${insumo.descripcion}
                            <i class="fas fa-minus-circle pull-right"></i>
                        </button>`
                    );
                });
                spanCulata.text(listaInsumosPieza.Culata.length);
            }

            if (listaInsumosPieza.Portabit && listaInsumosPieza.Portabit.length > 0) {
                listaInsumosPieza.Portabit.forEach(insumo => {
                    itemsPortabit.append(`
                        <button data-id=${insumo.id} type="button" class="list-group-item">${insumo.descripcion}
                            <i class="fas fa-minus-circle pull-right"></i>
                        </button>`
                    );
                });
                spanPortabit.text(listaInsumosPieza.Portabit.length);
            }

            if (listaInsumosPieza.Cilindro && listaInsumosPieza.Cilindro.length > 0) {
                listaInsumosPieza.Cilindro.forEach(insumo => {
                    itemsCilindro.append(`
                        <button data-id=${insumo.id} type="button" class="list-group-item">${insumo.descripcion}
                            <i class="fas fa-minus-circle pull-right"></i>
                        </button>`
                    );
                });
                spanCilindro.text(listaInsumosPieza.Cilindro.length);
            }

            if (listaInsumosPieza.Zanco && listaInsumosPieza.Zanco.length > 0) {
                listaInsumosPieza.Zanco.forEach(insumo => {
                    itemsZanco.append(`
                        <button data-id=${insumo.id} type="button" class="list-group-item">${insumo.descripcion}
                            <i class="fas fa-minus-circle pull-right"></i>
                        </button>`
                    );
                });
                spanZanco.text(listaInsumosPieza.Zanco.length);
            }
        }

        function agregarListeners() {

            $('button[tipoPieza]').click(e => {
                const boton = $(e.currentTarget);
                modalNuevoInsumo.modal('show');
                const tipoPieza = boton.attr('tipoPieza');
                botonAgregarInsumo.data().tipoPieza = tipoPieza;
                if (tipoPieza == 1) {
                    divTipoBroca.show();
                }
            });
            // Autocomplete número de Insumo
            inputInsumo.getAutocompleteValid(setInsumoDesc, validarInsumo, { porDesc: false }, '/Barrenacion/getInsumo');
            // Autocomplete descripción de insumo
            inputDescripcion.getAutocompleteValid(setInsumoBusqPorDesc, validarInsumo, { porDesc: true }, '/Barrenacion/getInsumo');
            botonAgregarInsumo.click(agregarNuevoInsumo);
            modalNuevoInsumo.on("hide.bs.modal", limpiarCamposModal);
            botonEliminarInsumo.click(eliminarInsumoPieza);
            comboAC.change(obtenerInsumosPieza);

        }

        function addOnClickEvent() {
            $('button.list-group-item').unbind().click(e => {
                modalEliminarInsumo.modal('show');
                botonEliminarInsumo.data().id = $(e.currentTarget).data().id;
            });
        }

        function limpiarCamposModal() {
            inputInsumo.val('');
            inputDescripcion.val('');
            botonAgregarInsumo.data().tipoPieza = null;
            divTipoBroca.hide();
            comboTipoBroca[0].selectedIndex = 0;
        }

        function setInsumoDesc(e, ui) {
            inputInsumo.val(ui.item.value);
            inputDescripcion.val(ui.item.id);
        }

        function setInsumoBusqPorDesc(e, ui) {
            inputInsumo.val(ui.item.id);
            inputDescripcion.val(ui.item.value);
        }

        function validarInsumo(e, ul) {
            if (ul.item == null) {
                inputInsumo.val('');
                inputDescripcion.val('');
            }
        }

        function agregarNuevoInsumo() {

            const insumo = inputInsumo.val();
            const descripcion = inputDescripcion.val();

            if (!insumo || insumo.length === 0) {
                AlertaGeneral(`Aviso`, `Él número de insumo no puede estar vacío`);
                return;
            }

            const tipoPieza = botonAgregarInsumo.data().tipoPieza;

            const nuevoInsumoPieza = {
                tipoPieza,
                insumo,
                descripcion,
                activo: true,
                tipoBroca: tipoPieza == 1 ? comboTipoBroca.val() : 0,
                areaCuenta: comboAC.val()
            }

            $.blockUI({ message: 'Agregando insumo...' });
            $.post('/Barrenacion/AgregarInsumoPieza', { nuevoInsumoPieza })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        modalNuevoInsumo.modal('hide');
                        AlertaGeneral(`Éxito`, `Insumo agregado correctamente.`);
                        nuevoInsumoPieza.id = response.id;
                        nuevoInsumoPieza.tipoBroca = response.tipoBroca;
                        agregarInsumo(nuevoInsumoPieza);
                        addOnClickEvent();
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

        function agregarInsumo(nuevoInsumo) {
            let numeroItems;
            switch (nuevoInsumo.tipoPieza) {
                case "1":
                    itemsBroca.append(`
                        <button data-id=${nuevoInsumo.id} type="button" class="list-group-item">${nuevoInsumo.descripcion} Medida:${nuevoInsumo.tipoBroca}
                            <i class="fas fa-minus-circle pull-right"></i>
                        </button>`
                    );
                    numeroItems = parseInt(spanBroca.text());
                    spanBroca.text(numeroItems + 1);
                    break;
                case "2":
                    itemsMartillo.append(`
                        <button data-id=${nuevoInsumo.id} type="button" class="list-group-item">${nuevoInsumo.descripcion}
                            <i class="fas fa-minus-circle pull-right"></i>
                        </button>`
                    );
                    numeroItems = parseInt(spanMartillo.text());
                    spanMartillo.text(numeroItems + 1);
                    break;
                case "3":
                    itemsBarra.append(`
                        <button data-id=${nuevoInsumo.id} type="button" class="list-group-item">${nuevoInsumo.descripcion}
                            <i class="fas fa-minus-circle pull-right"></i>
                        </button>`
                    );
                    numeroItems = parseInt(spanBarra.text());
                    spanBarra.text(numeroItems + 1);
                    break;
                case "4":
                    itemsCulata.append(`
                        <button data-id=${nuevoInsumo.id} type="button" class="list-group-item">${nuevoInsumo.descripcion}
                            <i class="fas fa-minus-circle pull-right"></i>
                        </button>`
                    );
                    numeroItems = parseInt(spanCulata.text());
                    spanCulata.text(numeroItems + 1);
                    break;
                case "5":
                    itemsPortabit.append(`
                        <button data-id=${nuevoInsumo.id} type="button" class="list-group-item">${nuevoInsumo.descripcion}
                            <i class="fas fa-minus-circle pull-right"></i>
                        </button>`
                    );
                    numeroItems = parseInt(spanPortabit.text());
                    spanPortabit.text(numeroItems + 1);
                    break;
                case "6":
                    itemsCilindro.append(`
                        <button data-id=${nuevoInsumo.id} type="button" class="list-group-item">${nuevoInsumo.descripcion}
                            <i class="fas fa-minus-circle pull-right"></i>
                        </button>`
                    );
                    numeroItems = parseInt(spanCilindro.text());
                    spanCilindro.text(numeroItems + 1);
                    break;
                case "7":
                    itemsBarraSegunda.append(`
                            <button data-id=${nuevoInsumo.id} type="button" class="list-group-item">${nuevoInsumo.descripcion}
                                <i class="fas fa-minus-circle pull-right"></i>
                            </button>`
                    );
                    numeroItems = parseInt(spanBarraSegunda.text());
                    spanCilindro.text(numeroItems + 1);
                    break;
                case "8":
                    itemsZanco.append(`
                                <button data-id=${nuevoInsumo.id} type="button" class="list-group-item">${nuevoInsumo.descripcion}
                                    <i class="fas fa-minus-circle pull-right"></i>
                                </button>`
                    );
                    numeroItems = parseInt(spanZanco.text());
                    spanZanco.text(numeroItems + 1);
                    break;
            }
        }

        function eliminarInsumoPieza() {
            const id = botonEliminarInsumo.data().id;
            if (!id || id == 0) {
                AlertaGeneral(`Aviso`, `No se encontró al insumo por eliminar.`);
                return;
            }
            $.blockUI({ message: 'Eliminando insumo...' });
            $.post('/Barrenacion/EliminarInsumoPieza', { id })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        modalEliminarInsumo.modal('hide');
                        AlertaGeneral(`Éxito`, `Insumo eliminado correctamente.`);
                        obtenerInsumosPieza();
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

        function limpiarItems() {
            itemsBroca.empty();
            spanBroca.text("0");
            itemsMartillo.empty();
            spanMartillo.text("0");
            itemsBarra.empty();
            spanBarra.text("0");
            itemsCulata.empty();
            spanCulata.text("0");
            itemsPortabit.empty();
            spanPortabit.text("0");
            itemsCilindro.empty();
            spanCilindro.text("0");
            itemsBarraSegunda.empty();
            spanBarraSegunda.text("0");
            itemsZanco.empty();
            spanZanco.text("0");
        }

    }

    $(() => Barrenacion.CatalogoPieza = new CatalogoPieza());
})();