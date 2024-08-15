(
    () => {
        $.namespace('Maquinaria.Rentabilidad.CatalogoFletes');
        CatalogoFletes = function () {
            //Variables.

            const tablaFletes = $("#tablaFletes");
            const botonNuevoFlete = $("#botonNuevoFlete");
            const modalNuevoFlete = $("#modalNuevoFlete");
            const comboNoEconomico = $("#comboNoEconomico");
            const inputAreaCuenta = $("#inputAreaCuenta");
            const botonAgregarFlete = $("#botonAgregarFlete");
            const comboEstatus = $('#comboEstatus');

            const modalEliminarFlete = $("#modalEliminarBanco");
            const botonEliminarFlete = $("#botonEliminarFlete");

            let d = new Date();
            let dtTablaFletes;
            let idFlete;

            //Inicializaciones
            (function int() {
                comboNoEconomico.select2();
                SetIdDefault();
                limpiarModal();
                cargarFletes();
                initTablaFletes();
                addListeners();

            })();

            function addListeners() {
                modalNuevoFlete.on("hide.bs.modal", limpiarModal);
                botonNuevoFlete.click(() => modalNuevoFlete.modal('show'));
                botonAgregarFlete.click(GuardarFlete);
                botonEliminarFlete.click(GuardarFlete);
                cargarComboEconomico();
                comboNoEconomico.on('change', () => {
                    inputAreaCuenta.val(comboNoEconomico.select2('data')[0].areaCuenta);
                });
            }

            function cargarComboEconomico() {
                $.get('/Rentabilidad/CboEconomico')
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            comboNoEconomico.select2({
                                data: response.items
                            });
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

            function limpiarModal() {
                inputAreaCuenta.val('');
                comboNoEconomico.val(null);//.trigger('change');
            }

            function initTablaFletes() {
                dtTablaFletes = tablaFletes.DataTable({
                    language: dtDicEsp,
                    destroy: true,
                    paging: false,
                    searching: false,
                    columns: [
                        { data: 'noEconomico', title: 'No Economico' },
                        { data: 'areaCuenta', title: 'Área Cuenta' },
                        { data: 'id', render: (data, type, row) => `<button class="btn btn-success editar"><i class="fas fa-tools"></i> editar</button>` },
                        { data: 'id', render: (data, type, row) => `<button class="btn btn-danger eliminar"><i class="fas fa-trash"></i> eliminar</button>` }
                    ],
                    columnDefs: [
                        { className: "dt-center", "targets": "_all" }
                    ],
                    drawCallback: function (settings) {
                        tablaFletes.find('.editar').click(function () {
                            let data = dtTablaFletes.row($(this).parents('tr')).data();
                            idFlete = data.id;
                            //inputBanco.val(data.banco);
                            comboNoEconomico.val(data.noEconomico);
                            inputAreaCuenta.val(data.inputAreaCuenta);
                            comboEstatus.val(data.estatus);
                            modalNuevoFlete.modal('show');
                        });

                        tablaFletes.find('.eliminar').click(function () {
                            let data = dtTablaFletes.row($(this).parents('tr')).data();
                            idFlete = data.id;
                            modalEliminarFlete.modal('show');
                        });
                    }
                });
            }

            async function cargarFletes() {
                try {

                    $.get('/Rentabilidad/GetInfoFletes')
                        .then(response => {
                            if (response.success) {
                                if (dtTablaFletes != null) {
                                    dtTablaFletes.clear().draw();
                                    dtTablaFletes.rows.add(response.listaFletes).draw();
                                }
                            }
                            else
                                // Operación no completada.
                                AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        },
                            error => {
                                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                            });
                } catch (e) { AlertaGeneral(`Operación fallida`, e.message) }
            }

            function GuardarFlete() {
                const nuevoFlete = ItemFlete();
                $.blockUI({ message: 'Guardando Banco...' });
                $.post('/Rentabilidad/SaveOrUpdateFlete', { nuevo: nuevoFlete })
                    .always($.unblockUI)
                    .then(r => {
                        if (r.success) {

                            AlertaGeneral(`Éxito`, `El Flete ha sido actualizado con exito.`);
                            cargarFletes();
                            SetIdDefault();
                        }
                        else {
                            CheckModalShown();
                            AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        }
                    }, error => {
                        CheckModalShown();
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    });
            }

            function ItemFlete() {
                let economico = comboNoEconomico.select2('data')[0];

                return {
                    id: idFlete,
                    noEconomico: economico.text,
                    economicoID: economico.id,
                    areaCuenta: economico.areaCuenta,
                    estatus: comboEstatus.val(),
                    fechaCreacion: `${d.getDate()}/${d.getMonth()}/${d.getFullYear()}`,
                    usuarioCreadorID: 0
                };
            }
            function SetIdDefault() {
                idFlete = 0;
            }

            function CheckModalShown() {
                if ((modalNuevoFlete.data('bs.modal') || {}).isShown)
                    modalNuevoFlete.modal('hide');
                if ((modalEliminarFlete.data('bs.modal') || {}).isShown)
                    modalEliminarFlete.modal('hide');
            }

        }
        $(() => Maquinaria.Rentabilidad.CatalogoFletes = new CatalogoFletes())
            .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
            .ajaxStop($.unblockUI);
    })();