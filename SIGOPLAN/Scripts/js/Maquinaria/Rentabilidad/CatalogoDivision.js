(
    () => {
        $.namespace('Maquinaria.Rentabilidad.CatalogoDivision');
        CatalogoDivision = function () {
            //Variables.
            let d = new Date();
            let dtTablaDivisiones;
            let divisionID;
            let estatus;

            //#region Selectores HTML
            const comboEstatus = $('#comboEstatus');
            const tablaDivisiones = $('#tablaDivisiones');
            const modalNuevaDivision = $('#modalNuevaDivision');
            const lblDescripcionModal = $('#lblDescripcionModal');
            const inputDivision = $('#inputDivision');
            const comboAC = $('#comboAC');
            const botonAddACDivision = $('#botonAddACDivision');
            const divAreasCuentaADD = $('#divAreasCuentaADD');
            const botonGuardarCambios = $('#botonGuardarCambios');
            const botonAgregarDivision = $('#botonAgregarDivision');

            //Eliminar Modal
            const botonEliminar = $('#botonEliminar');
            const lblEliminar = $('#lblEliminar');
            const modalEliminar = $('#modalEliminar');
            //#endregion
            let empresaActual = 2;

            //Inicializaciones
            (function int() {
                getEmpresaActual();
                comboAC.fillCombo('/Rentabilidad/cboAreaCuenta', null, false);
                comboAC.select2();
                initTablaDivisiones();
                cargarDivisiones();
                addListeners();
                setDefault();

            })();

            function getEmpresaActual(){
                $.post("/Base/getEmpresa").then(function(response) { empresaActual = response; cbConfiguracion.val(response == 2 ? 1 : 0); } );
            }

            function initTablaDivisiones() {
                dtTablaDivisiones = tablaDivisiones.DataTable({
                    language: dtDicEsp,
                    destroy: true,
                    paging: false,
                    searching: false,
                    columns: [
                        { data: 'division', title: 'Descripcion' },
                        {
                            data: 'areaCuenta', title: 'Áreas Cuenta', render: (data, type, row) => {
                                let html = "";
                                row.areaCuenta.forEach(dato => {
                                    console.log(dato)
                                    html += `<button class="btn btn-success displayAreaCuenta"><i class="fab fa-creative-commons-nd"></i> ${dato.descripcion}</button>`;
                                }
                                );
                                return html;
                            }
                        },
                        { data: 'id', render: (data, type, row) => `<button class="btn btn-success editar" data-id="${data}"><i class="fas fa-tools"></i> editar</button>` },
                        { data: 'id', render: (data, type, row) => `<button class="btn btn-danger eliminar" data-id="${data}"><i class="fas fa-trash"></i> eliminar</button>` }
                    ],
                    columnDefs: [
                        { className: "dt-center", "targets": "_all" }
                    ],
                    drawCallback: function (settings) {

                        tablaDivisiones.find('.editar').click(function () {
                            let ID = $(this).attr('data-id');
                            divisionID = ID;
                            $.get('/Rentabilidad/getDivisionByID', { divisionID: ID })
                                .then(response => {
                                    if (response.success) {
                                        // Operación exitosa.
                                        limpiarModal();
                                        comboEstatus.val(response.estatus);
                                        inputDivision.val(response.division);
                                        let listaAC = response.areaCuenta;
                                        comboAC.val(listaAC);
                                        comboAC.trigger('change');
                                        modalNuevaDivision.modal('show');
                                    } else {
                                        // Operación no completada.
                                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                                    }
                                }, error => {
                                    // Error al lanzar la petición.
                                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                                }
                                );
                        });

                        tablaDivisiones.find('.eliminar').click(function () {
                            let ID = $(this).attr('data-id');
                            divisionID = ID;
                            lblEliminar.text('¿Estas seguro que deseas dar de baja la Division?')
                            modalEliminar.modal('show');
                        });

                    }
                });
            }
            function bajaDivision() {

                $.get('/Rentabilidad/bajaDivision', { divisionID: divisionID })
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            setDefault();
                            modalEliminar.modal('hide');
                            AlertaGeneral('Operaciòn Existosa', 'La division fue dada de baja correctamente');
                            cargarDivisiones();
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

            async function cargarDivisiones() {
                try {

                    $.get('/Rentabilidad/GetInfoDivision', { areaCuenta: 1, estatus: true })
                        .then(response => {
                            if (response.success) {
                                if (dtTablaDivisiones != null) {
                                    dtTablaDivisiones.clear().draw();
                                    dtTablaDivisiones.rows.add(response.listaDivisiones).draw();
                                }
                            }
                            else
                                // Operación no completada.
                                AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        },
                            error => {
                                AlertaGeneral(`Operación fallida`, `Ocurrió un error al enviar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                            });
                } catch (e) { AlertaGeneral(`Operación fallida`, e.message) }
            }

            function limpiarModal() {
                inputDivision.val('');
                comboEstatus.val('true');
                comboAC.val(null).trigger('change');

            }
            function addListeners() {
                botonEliminar.click(bajaDivision)
                botonGuardarCambios.click(GuardarDivisiones);
                botonAgregarDivision.click(() => {
                    lblDescripcionModal.text('Alta de Divisiones');
                    limpiarModal();
                    modalNuevaDivision.modal('show');
                });
            }

            function GuardarDivisiones() {

                let objDivisiones = setInfoDivisiones();

                $.post('/Rentabilidad/SaveOrUpdateCatalogoDivisiones', { objDivision: objDivisiones })
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            modalNuevaDivision.modal('hide');
                            limpiarModal();
                            cargarDivisiones();
                            AlertaGeneral(`Operación Exitosa`, `Los registros se han actualizado correctamente`);
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

            function setInfoDivisiones() {

                let division = inputDivision.val();
                let estatus = comboEstatus.val();
                if (inputDivision.val().length == 0)
                    return AlertaGeneral('Alerta', 'No se Capturo la División, Favor de Capturar para poder continuar con el guardado.')
                if (comboAC.val().length == 0)
                    return AlertaGeneral('Alerta', 'No se asignó ningun area cuenta para esta division, favor de agregar por lo menos una division para continuar.');

                let listaAreaCuentas = comboAC.val().map(x => {
                    let divisionDetalle = {
                        id: 0,
                        acID: x,
                        divisionID: divisionID,
                        estatus: true,
                    }
                    return divisionDetalle;
                });

                return {
                    id: divisionID,
                    division: division,
                    estatus: estatus,
                    fechaCreacion: `${d.getDate()}/${d.getMonth()}/${d.getFullYear()}`,
                    usuarioCreadorID: 0,
                    divisionDetalle: listaAreaCuentas
                }
            }

            function setDefault() {
                divisionID = 0;
            }
        }
        $(() => Maquinaria.Rentabilidad.CatalogoDivision = new CatalogoDivision())
            .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
            .ajaxStop($.unblockUI);
    })();