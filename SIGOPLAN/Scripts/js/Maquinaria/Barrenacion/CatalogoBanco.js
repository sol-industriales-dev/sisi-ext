(
    () => {
        $.namespace('Barrenacion.CatalogoBanco');
        CatalogoBanco = function () {
            //Variables.
            const comboAC = $("#comboAC");
            const tablaBancos = $("#tablaBancos");
            const modalNuevoBanco = $("#modalNuevoBanco");
            const inputBanco = $("#inputBanco");
            const inputDescripcion = $("#inputDescripcion");
            const botonAgregarBanco = $("#botonAgregarBanco");

            const modalEliminarBanco = $("#modalEliminarBanco");
            const botonEliminarBanco = $("#botonEliminarBanco");
            const botonNuevoBanco = $("#botonNuevoBanco");
            const comboAreaCuenta = $('#comboAreaCuenta');

            let d = new Date();

            let dtTablaBancos;
            let idBanco;

            //Inicializaciones
            (function int() {
                SetIdDefault();
                initTablaBanco();
                cargarBancos();
                addListeners();
            })();

            function addListeners() {
                modalNuevoBanco.on("hide.bs.modal", limpiarModal);
                botonNuevoBanco.click(() => modalNuevoBanco.modal('show'));
                botonAgregarBanco.click(() => GuardarBanco(true));
                botonEliminarBanco.click(() => GuardarBanco(false));
                comboAC.fillCombo('/Barrenacion/ObtenerAC', null, false);
                comboAreaCuenta.fillCombo('/Barrenacion/ObtenerAC', null, false);
                comboAreaCuenta.change(cargarBancos);
            }


            function limpiarModal() {
                inputDescripcion.val('');
                inputBanco.val('');
            }

            function initTablaBanco() {
                dtTablaBancos = tablaBancos.DataTable({
                    language: dtDicEsp,
                    destroy: true,
                    paging: true,
                    searching: false,
                    columns: [
                        { data: 'banco', title: 'Banco' },
                        { data: 'descripcion', title: 'Descripcion' },
                        { data: 'areaCuenta', title: 'Área Cuenta' },
                        { data: 'id', render: (data, type, row) => `<button class="btn btn-success editar"><i class="fas fa-tools"></i> editar</button>` },
                        { data: 'id', render: (data, type, row) => `<button class="btn btn-danger eliminar"><i class="fas fa-trash"></i> eliminar</button>` }
                    ],
                    columnDefs: [
                        { className: "dt-center", "targets": "_all" }
                    ],
                    drawCallback: function (settings) {

                        tablaBancos.find('.editar').click(function () {
                            let data = dtTablaBancos.row($(this).parents('tr')).data();
                            idBanco = data.id;
                            inputBanco.val(data.banco);
                            inputDescripcion.val(data.descripcion);
                            comboAC.val(data.areaCuenta);
                            modalNuevoBanco.modal('show');
                        });

                        tablaBancos.find('.eliminar').click(function () {
                            let data = dtTablaBancos.row($(this).parents('tr')).data();
                            idBanco = data.id;
                            modalEliminarBanco.modal('show');
                        });

                    }
                });
            }
            async function cargarBancos() {
                try {

                    $.get('/Barrenacion/ObtenerBancos', { areaCuenta: comboAreaCuenta.val() })
                        .then(response => {
                            if (response.success) {
                                if (dtTablaBancos != null) {
                                    dtTablaBancos.clear().draw();
                                    dtTablaBancos.rows.add(response.listaBancos).draw();
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

            function GuardarBanco(estatus) {
                const nuevoBanco = ItemBanco(estatus);
                $.blockUI({ message: 'Guardando Banco...' });
                $.post('/Barrenacion/AgregarBanco', { nuevoBanco })
                    .always($.unblockUI)
                    .then(r => {
                        if (r.success) {
                            CheckModalShown();
                            AlertaGeneral(`Éxito`, `El banco ha sido actualizado con exito.`);
                            cargarBancos();
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
            function ItemBanco(estatus) {
                return {
                    id: idBanco,
                    banco: inputBanco.val(),
                    descripcion: inputDescripcion.val(),
                    areaCuenta: comboAC.val(),
                    estatus: estatus,
                    fechaCreacion: `${d.getDate()}/${d.getMonth()}/${d.getFullYear()}`,
                    usuarioCreadorID: 0
                };
            }
            function SetIdDefault() {
                idBanco = 0;
            }

            function CheckModalShown() {
                if ((modalNuevoBanco.data('bs.modal') || {}).isShown)
                    modalNuevoBanco.modal('hide');
                if ((modalEliminarBanco.data('bs.modal') || {}).isShown)
                    modalEliminarBanco.modal('hide');
            }

        }
        $(() => Barrenacion.CatalogoBanco = new CatalogoBanco())
            .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
            .ajaxStop($.unblockUI);
    })();