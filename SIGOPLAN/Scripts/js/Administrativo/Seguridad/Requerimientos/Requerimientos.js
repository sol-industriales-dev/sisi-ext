(() => {
    $.namespace('Administrativo.Requerimientos.Requerimientos');
    Requerimientos = function () {
        //#region Selectores
        const tablaRequerimientos = $('#tablaRequerimientos');
        const botonAgregar = $('#botonAgregar');
        const modalRequerimiento = $('#modalRequerimiento');
        const inputRequerimiento = $('#inputRequerimiento');
        const selectClasificacion = $('#selectClasificacion');
        const inputDescripcion = $('#inputDescripcion');
        const tablaPuntos = $('#tablaPuntos');
        const botonGuardar = $('#botonGuardar');
        const botonAgregarPunto = $('#botonAgregarPunto');
        const botonQuitarPunto = $('#botonQuitarPunto');
        const inputPorcentajeTotal = $('#inputPorcentajeTotal');
        const botonCargaMasiva = $('#botonCargaMasiva');
        const modalCargaMasiva = $('#modalCargaMasiva');
        const inputFileExcel = $('#inputFileExcel');
        const botonGuardarCargaMasiva = $('#botonGuardarCargaMasiva');
        //#endregion

        let dtRequerimientos;
        let dtPuntos;
        const ESTATUS = { NUEVO: 0, EDITAR: 1 };

        (function init() {
            initTablaRequerimientos();
            initTablaPuntos();
            agregarListeners();
            cargarRequerimientos();

            selectClasificacion.fillCombo('/Administrativo/Requerimientos/GetClasificacionCombo', null, false, null);
        })();

        modalRequerimiento.on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
            calcularPorcentajeTotal();
        });

        botonAgregarPunto.on('click', function () {
            let datos = dtPuntos.rows().data();

            $.each(datos, function (idx, data) {
                let row = tablaPuntos.find('tbody tr').eq(idx);

                data.indice = $(row).find('.inputIndicePunto').val();
                data.actividad = $(row).find('.selectActividad').val();
                data.condicionante = $(row).find('.selectCondicionante').val();
                data.seccion = $(row).find('.selectSeccion').val();
                data.codigo = $(row).find('.inputCodigo').val();
                data.descripcion = $(row).find('.inputDescripcionPunto').val();
                data.verificacion = $(row).find('.selectVerificacion').val();
                data.porcentaje = $(row).find('.inputPorcentaje').val();
                data.periodicidad = $(row).find('.selectPeriodicidad').val();
                data.area = $(row).find('.selectArea').val();
            });

            datos.push({
                id: 0,
                indice: '',
                actividad: '',
                condicionante: '',
                seccion: '',
                codigo: '',
                descripcion: '',
                verificacion: '',
                porcentaje: '',
                fechaCreacion: '',
                fechaCreacionString: '',
                periodicidad: '',
                area: ''
            });

            dtPuntos.clear();
            dtPuntos.rows.add(datos).draw();

            calcularPorcentajeTotal();
        });

        botonQuitarPunto.on('click', function () {
            dtPuntos.row(tablaPuntos.find("tr.selected")).remove().draw();

            let cuerpo = tablaPuntos.find('tbody');

            if (cuerpo.find("tr").length == 0) {
                dtPuntos.draw();
            } else {
                tablaPuntos.find('tbody tr').each(function (idx, row) {
                    let rowData = dtPuntos.row(row).data();

                    if (rowData != undefined) {
                        rowData.indice = $(row).find('.inputIndicePunto').val();
                        rowData.actividad = $(row).find('.selectActividad').val();
                        rowData.condicionante = $(row).find('.selectCondicionante').val();
                        rowData.seccion = $(row).find('.selectSeccion').val();
                        rowData.codigo = $(row).find('.inputCodigo').val();
                        rowData.descripcion = $(row).find('.inputDescripcionPunto').val();
                        rowData.verificacion = $(row).find('.selectVerificacion').val();
                        rowData.porcentaje = $(row).find('.inputPorcentaje').val();
                        rowData.periodicidad = $(row).find('.selectPeriodicidad').val();
                        rowData.area = $(row).find('.selectArea').val();

                        dtPuntos.row(row).data(rowData).draw();

                        let selectActividad = $(row).find('.selectActividad');
                        let selectCondicionante = $(row).find('.selectCondicionante');
                        let selectSeccion = $(row).find('.selectSeccion');
                        let selectVerificacion = $(row).find('.selectVerificacion');
                        let selectPeriodicidad = $(row).find('.selectPeriodicidad');
                        let selectArea = $(row).find('.selectArea');

                        selectActividad.fillCombo('/Administrativo/Requerimientos/GetActividadesCombo', null, false, '');
                        selectActividad.find('option[value="' + rowData.actividad + '"]').attr('selected', true);
                        selectCondicionante.fillCombo('/Administrativo/Requerimientos/GetCondicionantesCombo', null, false, '');
                        selectCondicionante.find('option[value="' + rowData.condicionante + '"]').attr('selected', true);
                        selectSeccion.fillCombo('/Administrativo/Requerimientos/GetSeccionesCombo', null, false, '');
                        selectSeccion.find('option[value="' + rowData.seccion + '"]').attr('selected', true);
                        selectVerificacion.fillCombo('/Administrativo/Requerimientos/GetVerificacionCombo', null, false, '');
                        selectVerificacion.find('option[value="' + rowData.verificacion + '"]').attr('selected', true);
                        selectPeriodicidad.fillCombo('/Administrativo/Requerimientos/GetPeriodicidadCombo', null, false, '');
                        selectPeriodicidad.find('option[value="' + rowData.periodicidad + '"]').attr('selected', true);
                        selectArea.fillCombo('/Administrativo/Requerimientos/GetAreaCombo', null, false, '');
                        selectArea.find('option[value="' + rowData.area + '"]').attr('selected', true);
                    }
                });
            }

            calcularPorcentajeTotal();
        });

        function initTablaRequerimientos() {
            dtRequerimientos = tablaRequerimientos.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tablaRequerimientos.on('click', '.btn-editar', function () {
                        let rowData = dtRequerimientos.row($(this).closest('tr')).data();

                        limpiarModal();
                        llenarCamposModal(rowData);
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;
                        cargarPuntos(true, rowData.id);
                        modalRequerimiento.modal('show');
                    });

                    tablaRequerimientos.on('click', '.btn-eliminar', function () {
                        let rowData = dtRequerimientos.row($(this).closest('tr')).data();

                        AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar el requerimiento "${rowData.requerimiento} - ${rowData.descripcion}"?`,
                            () => eliminarRequerimiento(rowData.id))
                    });
                },
                columns: [
                    { data: 'requerimiento', title: 'Requerimiento' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'clasificacionDesc', title: 'Clasificación' },
                    {
                        render: function (data, type, row, meta) {
                            return `
                            <button title="Editar" class="btn-editar btn btn-warning">
                                <i class="fas fa-pencil-alt"></i>
                            </button>
                            &nbsp;
                            <button title="Eliminar" class="btn-eliminar btn btn-danger">
                                <i class="fas fa-trash"></i>
                            </button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaPuntos() {
            dtPuntos = tablaPuntos.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                scrollY: '45vh',
                scrollCollapse: true,
                order: [],
                initComplete: function (settings, json) {
                    tablaPuntos.on('click', 'td', function () {
                        let row = $(this).closest('tr');

                        if (row.hasClass('selected')) {
                            row.removeClass('selected');
                        } else {
                            dtPuntos.$('tr.selected').removeClass('selected');
                            row.addClass('selected');
                        }
                    });

                    tablaPuntos.on('change', '.inputPorcentaje', function () {
                        calcularPorcentajeTotal();
                    });

                    tablaPuntos.on('focus', '.inputPorcentaje', function () {
                        $(this).select();
                    });
                },
                createdRow: function (row, rowData) {
                    let selectActividad = $(row).find('.selectActividad');
                    let selectCondicionante = $(row).find('.selectCondicionante');
                    let selectSeccion = $(row).find('.selectSeccion');
                    let selectVerificacion = $(row).find('.selectVerificacion');
                    let selectPeriodicidad = $(row).find('.selectPeriodicidad');
                    let selectArea = $(row).find('.selectArea');

                    selectActividad.fillCombo('/Administrativo/Requerimientos/GetActividadesCombo', null, false, '');
                    selectActividad.find('option[value="' + rowData.actividad + '"]').attr('selected', true);
                    selectCondicionante.fillCombo('/Administrativo/Requerimientos/GetCondicionantesCombo', null, false, '');
                    selectCondicionante.find('option[value="' + rowData.condicionante + '"]').attr('selected', true);
                    selectSeccion.fillCombo('/Administrativo/Requerimientos/GetSeccionesCombo', null, false, '');
                    selectSeccion.find('option[value="' + rowData.seccion + '"]').attr('selected', true);
                    selectVerificacion.fillCombo('/Administrativo/Requerimientos/GetVerificacionCombo', null, false, '');
                    selectVerificacion.find('option[value="' + rowData.verificacion + '"]').attr('selected', true);
                    selectPeriodicidad.fillCombo('/Administrativo/Requerimientos/GetPeriodicidadCombo', null, false, '');
                    selectPeriodicidad.find('option[value="' + rowData.periodicidad + '"]').attr('selected', true);
                    selectArea.fillCombo('/Administrativo/Requerimientos/GetAreaCombo', null, false, '');
                    selectArea.find('option[value="' + rowData.area + '"]').attr('selected', true);
                },
                columns: [
                    {
                        // title: 'Índice', render: function (data, type, row, meta) {
                        title: 'Punto', render: function (data, type, row, meta) {
                            return `<input class="form-control text-center inputIndicePunto" value="${row.indice}">`;
                        }
                    },
                    {
                        title: 'Actividad', render: function (data, type, row, meta) {
                            return `<select class="form-control selectActividad"></select>`;
                        }
                    },
                    {
                        title: 'Condicionante', render: function (data, type, row, meta) {
                            return `<select class="form-control selectCondicionante"></select>`;
                        }
                    },
                    {
                        title: 'Sección', render: function (data, type, row, meta) {
                            return `<select class="form-control selectSeccion"></select>`;
                        }
                    },
                    {
                        title: 'Código', render: function (data, type, row, meta) {
                            return `<input class="form-control text-center inputCodigo" value="${row.codigo}">`;
                        }
                    },
                    {
                        title: 'Descripción', render: function (data, type, row, meta) {
                            return `<input class="form-control inputDescripcionPunto" value="${row.descripcion}">`;
                        }
                    },
                    {
                        title: 'Verificación', render: function (data, type, row, meta) {
                            return `<select class="form-control selectVerificacion"></select>`;
                        }
                    },
                    {
                        title: 'Porcentaje', render: function (data, type, row, meta) {
                            return `<input type="number" class="form-control text-center inputPorcentaje" value="${row.porcentaje}">`;
                        }
                    },
                    {
                        title: 'Periodicidad', render: function (data, type, row, meta) {
                            return `<select class="form-control selectPeriodicidad"></select>`;
                        }
                    },
                    {
                        title: 'Área', render: function (data, type, row, meta) {
                            return `<select class="form-control selectArea"></select>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '40%', targets: [5] },
                    { width: '7%', targets: [9] }
                ]
            });
        }

        function cargarRequerimientos() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/GetRequerimientos')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AddRows(tablaRequerimientos, response.data);
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function cargarPuntos(editar, requerimientoID) {
            if (editar) {
                $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                $.post('/Administrativo/Requerimientos/GetPuntosRequerimiento', { requerimientoID })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            AddRows(tablaPuntos, response.data);
                        } else {
                            AlertaGeneral(`Alerta`, response.message);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            } else {
                // $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                // $.post('/Administrativo/Requerimientos/GetPuntos')
                //     .always($.unblockUI)
                //     .then(response => {
                //         if (response.success) {
                //             AddRows(tablaPuntos, response.data);
                //         } else {
                //             AlertaGeneral(`Alerta`, response.message);
                //         }
                //     }, error => {
                //         AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                //     }
                //     );
            }
        }

        function agregarListeners() {
            botonAgregar.click(() => {
                limpiarModal();
                botonGuardar.data().estatus = ESTATUS.NUEVO;
                botonGuardar.data().id = 0;
                cargarPuntos(false, 0);
                modalRequerimiento.modal('show');
                calcularPorcentajeTotal();
            });
            botonGuardar.click(guardarRequerimiento);
            botonCargaMasiva.click(() => { modalCargaMasiva.modal('show') });
            botonGuardarCargaMasiva.click(guardarCargaMasiva);
        }

        function limpiarModal() {
            inputRequerimiento.val('');
            inputDescripcion.val('');

            dtPuntos.clear().draw();
        }

        function llenarCamposModal(data) {
            inputRequerimiento.val(data.requerimiento);
            selectClasificacion.val(data.clasificacion);
            inputDescripcion.val(data.descripcion);
        }

        function guardarRequerimiento() {
            //#region Validación 100% de porcentaje total y validación información incompleta.
            let porcentajeTotal = 0;
            let flagInformacionIncompleta = false;

            tablaPuntos.find('tbody tr').each(function (index, row) {
                let porcentaje = +($(row).find('.inputPorcentaje').val());
                porcentajeTotal += !isNaN(porcentaje) ? porcentaje : 0;

                let indice = $(row).find('.inputIndicePunto').val();
                let actividad = $(row).find('.selectActividad').val();
                let condicionante = $(row).find('.selectCondicionante').val();
                let seccion = $(row).find('.selectSeccion').val();
                let codigo = $(row).find('.inputCodigo').val();
                let descripcion = $(row).find('.inputDescripcionPunto').val();
                let verificacion = $(row).find('.selectVerificacion').val();
                let periodicidad = $(row).find('.selectPeriodicidad').val();
                let area = $(row).find('.selectArea').val();

                if (indice == '' || actividad == '' || condicionante == '' || seccion == '' || codigo == '' || descripcion == '' || verificacion == '' || periodicidad == '' || area == '') {
                    flagInformacionIncompleta = true;
                }
            });

            if (Math.round(porcentajeTotal) != 100) {
                AlertaGeneral(`Alerta`, `El porcentaje total de los puntos debe sumar 100%.`);
                return;
            }

            if (flagInformacionIncompleta) {
                AlertaGeneral(`Alerta`, `Debe llenar todos los campos de información.`);
                return;
            }
            //#endregion

            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    nuevaRequerimiento();
                    break;
                case ESTATUS.EDITAR:
                    editarRequerimiento();
                    break;
            }
        }

        function nuevaRequerimiento() {
            let requerimiento = getInformacionRequerimiento();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/GuardarNuevoRequerimiento', { requerimiento, puntos: requerimiento.puntos })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalRequerimiento.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarRequerimientos();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function editarRequerimiento() {
            let requerimiento = getInformacionRequerimiento();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/EditarRequerimiento', { requerimiento, puntos: requerimiento.puntos })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        modalRequerimiento.modal('hide');
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarRequerimientos();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function eliminarRequerimiento(id) {
            let requerimiento = { id };

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Requerimientos/EliminarRequerimiento', { requerimiento })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        cargarRequerimientos();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function getInformacionRequerimiento() {
            let puntos = getRenglonesPuntos();

            return {
                id: botonGuardar.data().id,
                requerimiento: inputRequerimiento.val(),
                descripcion: inputDescripcion.val(),
                clasificacion: +selectClasificacion.val(),
                estatus: true,
                puntos: puntos
            };
        }

        function getRenglonesPuntos() {
            let puntos = [];

            tablaPuntos.find('tbody tr').each(function (index, row) {
                let rowData = dtPuntos.row(row).data();
                let indice = $(row).find('.inputIndicePunto').val();
                let actividad = $(row).find('.selectActividad').val();
                let condicionante = $(row).find('.selectCondicionante').val();
                let seccion = $(row).find('.selectSeccion').val();
                let codigo = $(row).find('.inputCodigo').val();
                let descripcion = $(row).find('.inputDescripcionPunto').val();
                let verificacion = $(row).find('.selectVerificacion').val();
                let porcentaje = $(row).find('.inputPorcentaje').val();
                let periodicidad = $(row).find('.selectPeriodicidad').val();
                let area = $(row).find('.selectArea').val();

                puntos.push({
                    id: rowData.id,
                    indice: indice,
                    actividad: actividad,
                    condicionante: condicionante,
                    seccion: seccion,
                    codigo: codigo,
                    descripcion: descripcion,
                    verificacion: verificacion,
                    porcentaje: porcentaje,
                    periodicidad: periodicidad,
                    area: area
                });
            });

            return puntos;
        }

        function guardarCargaMasiva() {
            var request = new XMLHttpRequest();

            request.open("POST", "/Administrativo/Requerimientos/CargarExcelRequerimientosMasivo");
            request.send(formDataCargaMasiva());

            request.onload = function (response) {
                if (request.status == 200) {
                    $('#inputFileExcel').val('');
                    modalCargaMasiva.modal('hide');

                    cargarRequerimientos();

                    let respuesta = JSON.parse(request.response);

                    if (respuesta.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                    } else {
                        AlertaGeneral(`Alerta`, `${respuesta.message}`);
                    }
                } else {
                    AlertaGeneral(`Alerta`, `Error al guardar la información.`);
                }
            };
        }

        function formDataCargaMasiva() {
            let formData = new FormData();

            $.each(document.getElementById("inputFileExcel").files, function (i, file) {
                formData.append("files[]", file);
            });

            return formData;
        }

        function calcularPorcentajeTotal() {
            let porcentajeTotal = 0;

            tablaPuntos.find('tbody tr').each(function (index, row) {
                let porcentaje = +($(row).find('.inputPorcentaje').val());

                porcentajeTotal += !isNaN(porcentaje) ? porcentaje : 0;
            });

            inputPorcentajeTotal.val(Math.round(porcentajeTotal).toFixed(2) + '%');
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Administrativo.Requerimientos.Requerimientos = new Requerimientos())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();