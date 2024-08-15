(() => {
    $.namespace('SaludOcupacional.AtencionMedica');
    AtencionMedica = function () {
        //#region Selectores
        const inputClaveEmpleadoFiltro = $('#inputClaveEmpleadoFiltro');
        const botonBuscar = $('#botonBuscar');
        const botonAgregar = $('#botonAgregar');
        const tablaAtencionesMedicas = $('#tablaAtencionesMedicas');
        const modalCaptura = $('#modalCaptura');
        const botonGuardar = $('#botonGuardar');
        const inputClaveEmpleadoAgregar = $('#inputClaveEmpleadoAgregar');
        const inputNombreEmpleado = $('#inputNombreEmpleado');
        const inputFechaIngreso = $('#inputFechaIngreso');
        const inputPuesto = $('#inputPuesto');
        const inputEdad = $('#inputEdad');
        const inputSupervisor = $('#inputSupervisor');
        const inputArea = $('#inputArea');
        const selectTipoAtencionMedicaAgregar = $('#selectTipoAtencionMedicaAgregar');
        const tablaRevisiones = $('#tablaRevisiones');
        const textAreaDiagnostico = $('#textAreaDiagnostico');
        const textAreaTratamiento = $('#textAreaTratamiento');
        const textAreaComentarios = $('#textAreaComentarios');
        const checkboxIncapacidad = $('#checkboxIncapacidad');
        const checkboxTerminacion = $('#checkboxTerminacion');
        const inputDiasSiguienteRevision = $('#inputDiasSiguienteRevision');
        const botonDescargarArchivoST7 = $('#botonDescargarArchivoST7');
        const botonDescargarArchivoST2 = $('#botonDescargarArchivoST2');
        const report = $("#report");
        //#endregion

        const ESTATUS = {
            TERMINADO: 0,
            NUEVA_ATENCION: 1,
            NUEVA_REVISION: 2,
            CAPTURA_ARCHIVOS: 3
        };

        let dtAtencionesMedicas;
        let dtRevisiones;

        (function init() {
            initTablaAtencionesMedicas();
            initTablaRevisiones();

            botonBuscar.click(cargarAtencionesMedicas);
            botonAgregar.click(() => {
                limpiarModal();

                $('#rowNuevaRevision').css('display', 'block');
                $('#rowArchivos').css('display', 'block');
                $('#divCargarArchivoST7').css('display', 'block');
                $('#divCargarArchivoST2').css('display', 'block');
                $('#divDescargarArchivoST7').css('display', 'none');
                $('#divDescargarArchivoST2').css('display', 'none');

                botonGuardar.attr('disabled', false);
                botonGuardar.data().estatus = ESTATUS.NUEVA_ATENCION;
                botonGuardar.data().id = 0;
                modalCaptura.modal('show');
            });
            inputClaveEmpleadoAgregar.change(cargarInformacionEmpleado);
            botonGuardar.click(guardarInformacion);
        })();

        $('input[type=file]').on('change', function () {
            let tipoArchivo = $(this).attr('tipo-archivo');
            let inputArchivo = $(`#inputArchivo${tipoArchivo}`);
            let botonArchivo = $(`#botonArchivo${tipoArchivo}`);
            let labelArchivo = $(`#labelArchivo${tipoArchivo}`);
            let iconoBoton = botonArchivo.find('i');

            if (inputArchivo[0].files.length > 0) {
                let textoLabel = inputArchivo[0].files[0].name;

                if (textoLabel.length > 35) {
                    textoLabel = textoLabel.substr(0, 31) + '...';
                }

                labelArchivo.text(textoLabel);
                botonArchivo.addClass('custom-file-upload-subido');
                botonArchivo.removeClass('custom-file-upload');
                iconoBoton.addClass('fa-check');
                iconoBoton.removeClass('fa-file-upload');
            } else {
                labelArchivo.text('');
                botonArchivo.addClass('custom-file-upload');
                botonArchivo.removeClass('custom-file-upload-subido');
                iconoBoton.addClass('fa-file-upload');
                iconoBoton.removeClass('fa-check');
            }
        });

        botonDescargarArchivoST7.on('click', function () {
            let atencionMedica_id = botonDescargarArchivoST7.data().atencionMedica_id;

            if (atencionMedica_id > 0) {
                location.href = `/Administrativo/SaludOcupacional/DescargarArchivoST7?atencionMedica_id=${atencionMedica_id}`;
            }
        });

        botonDescargarArchivoST2.on('click', function () {
            let atencionMedica_id = botonDescargarArchivoST2.data().atencionMedica_id;

            if (atencionMedica_id > 0) {
                location.href = `/Administrativo/SaludOcupacional/DescargarArchivoST2?atencionMedica_id=${atencionMedica_id}`;
            }
        });

        function initTablaAtencionesMedicas() {
            dtAtencionesMedicas = tablaAtencionesMedicas.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tablaAtencionesMedicas.on('click', '.botonDetalle', function () {
                        let rowData = dtAtencionesMedicas.row($(this).closest('tr')).data();

                        cargarAtencionMedicaDetalle(rowData.id);
                    });

                    tablaAtencionesMedicas.on('click', '.botonReporte', function () {
                        let rowData = dtAtencionesMedicas.row($(this).closest('tr')).data();

                        imprimirAtencionMedica(rowData.id);
                    });
                    tablaAtencionesMedicas.on('click', '.botonEliminar', function () {
                        let rowData = dtAtencionesMedicas.row($(this).closest('tr')).data();

                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarAtencionMedica(rowData.id));
                    });
                },
                createdRow: function (row, rowData) {
                    if (rowData.estatus == 'PENDIENTE' || rowData.estatus == 'ARCHIVO PENDIENTE') {
                        $(row).addClass('renglonPendiente')
                    }

                    if (rowData.estatus == 'VENCIDO') {
                        $(row).addClass('renglonVencido')
                    }
                },
                columns: [
                    { data: 'empleadoDesc', title: 'Empleado' },
                    { data: 'fechaString', title: 'Fecha' },
                    { data: 'tipoDesc', title: 'Tipo' },
                    { data: 'estatus', title: 'Estado' },
                    {
                        title: '', render: function (data, type, row, meta) {
                            return `
                                <button class="btn btn-xs btn-primary botonDetalle"><i class="fa fa-align-justify"></i></button>
                                <button class="btn btn-xs btn-default botonReporte"><i class="fa fa-file"></i></button>
                                <button class="btn btn-xs btn-danger botonEliminar"><i class="fas fa-trash"></i></button>`;
                            ;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaRevisiones() {
            dtRevisiones = tablaRevisiones.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'consecutivo', title: '#' },
                    { data: 'diagnostico', title: 'Diagnóstico' },
                    { data: 'tratamiento', title: 'Tratamiento' },
                    { data: 'comentarios', title: 'Comentarios' },
                    {
                        data: 'incapacidad', title: 'Incapacidad', render: function (data, type, row, meta) {
                            return data ? '<h4 style="margin-top: 0px; margin-bottom: 0px;">&#10004;</h4>' : '';
                        }
                    },
                    { data: 'fechaString', title: 'Fecha' }
                ],
                columnDefs: [
                    { className: 'dt-center', targets: '_all' }
                ]
            });
        }

        function cargarAtencionesMedicas() {
            let claveEmpleado = +inputClaveEmpleadoFiltro.val();

            axios.post('/Administrativo/SaludOcupacional/CargarAtencionesMedicas', { claveEmpleado })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaAtencionesMedicas, response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }
        function fncEliminarAtencionMedica(idAtencion) {
            if (idAtencion <= 0) {
                Alert2Warning("Ocurrió un error al eliminar la atención medica seleccionada.");
            } else if (idAtencion > 0) {
                let obj = new Object();
                obj = {
                    idAtencion: idAtencion
                }
                axios.post("EliminarAtencionMedica", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(response.data.message);
                        cargarAtencionesMedicas();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function cargarAtencionMedicaDetalle(atencionMedica_id) {
            axios.post('/Administrativo/SaludOcupacional/CargarAtencionMedicaDetalle', { atencionMedica_id })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        limpiarModal();

                        $('#rowNuevaRevision').css('display', response.data.data.terminacion ? 'none' : 'block');
                        $('#rowArchivos').css('display', response.data.data.terminacion ? 'none' : 'block');

                        $('#divCargarArchivoST7').css('display', response.data.data.rutaArchivoST7 == null ? 'block' : 'none');
                        $('#divCargarArchivoST2').css('display', response.data.data.rutaArchivoST2 == null ? 'block' : 'none');
                        $('#divDescargarArchivoST7').css('display', response.data.data.rutaArchivoST7 == null ? 'none' : 'block');
                        $('#divDescargarArchivoST2').css('display', response.data.data.rutaArchivoST2 == null ? 'none' : 'block');

                        botonDescargarArchivoST7.data().atencionMedica_id = atencionMedica_id;
                        botonDescargarArchivoST2.data().atencionMedica_id = atencionMedica_id;

                        if (!response.data.data.terminacion) {
                            botonGuardar.data().estatus = ESTATUS.NUEVA_REVISION;
                            botonGuardar.attr('disabled', false);
                        } else if (response.data.data.terminacion && !response.data.data.archivoPendiente) {
                            botonGuardar.data().estatus = ESTATUS.TERMINADO;
                            botonGuardar.attr('disabled', true);
                        } else if (response.data.data.terminacion && response.data.data.archivoPendiente) {
                            botonGuardar.data().estatus = ESTATUS.CAPTURA_ARCHIVOS;
                            botonGuardar.attr('disabled', false);
                        }

                        botonGuardar.data().id = atencionMedica_id;
                        llenarModal(response.data.data);
                        modalCaptura.modal('show');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function limpiarModal() {
            inputClaveEmpleadoAgregar.val('');
            inputNombreEmpleado.val('');
            inputFechaIngreso.val('');
            inputPuesto.val('');
            inputEdad.val('');
            inputSupervisor.val('');
            inputArea.val('');
            selectTipoAtencionMedicaAgregar.val('');
            dtRevisiones.clear().draw();
            textAreaDiagnostico.val('');
            textAreaTratamiento.val('');
            textAreaComentarios.val('');
            checkboxIncapacidad.prop('checked', false);
            checkboxTerminacion.prop('checked', false);
            inputDiasSiguienteRevision.val('');

            $('#inputArchivoST7').val('').change();
            $('#inputArchivoST2').val('').change();
        }

        function llenarModal(datos) {
            inputClaveEmpleadoAgregar.val(datos.claveEmpleado);
            inputNombreEmpleado.val(datos.nombre);
            inputFechaIngreso.val(datos.fechaIngresoString);
            inputPuesto.val(datos.puestoDesc);
            inputEdad.val(datos.edad);
            inputSupervisor.val(datos.supervisorDesc);
            inputArea.val(datos.areaDesc);
            selectTipoAtencionMedicaAgregar.val(datos.tipo);

            AddRows(tablaRevisiones, datos.revisiones);

            checkboxIncapacidad.prop('checked', datos.revisiones.some((x) => { x.incapacidad }));
        }

        function cargarInformacionEmpleado() {
            let claveEmpleado = +inputClaveEmpleadoAgregar.val();

            if (claveEmpleado <= 0 || isNaN(claveEmpleado)) {
                Alert2Warning('Debe seleccionar una clave de empleado válida.');
                return;
            }

            axios.post('/Administrativo/SaludOcupacional/CargarInformacionEmpleado', { claveEmpleado })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        llenarInformacionEmpleado(response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function llenarInformacionEmpleado(datos) {
            inputNombreEmpleado.val(datos.nombre);
            inputFechaIngreso.val(datos.fechaIngresoString);
            inputPuesto.val(datos.puestoDesc);
            inputEdad.val(datos.edad);
            inputSupervisor.val(datos.supervisorDesc);
            inputArea.val(datos.areaDesc);
        }

        function guardarInformacion() {
            switch (botonGuardar.data().estatus) {
                case ESTATUS.NUEVA_ATENCION:
                    guardarNuevaAtencionMedica();
                    break;
                case ESTATUS.NUEVA_REVISION:
                    guardarNuevaRevision();
                    break;
                case ESTATUS.CAPTURA_ARCHIVOS:
                    guardarArchivosST7ST2();
                    break;
            }
        }

        function guardarNuevaAtencionMedica() {
            let data = getInformacionAtencionMedica();

            //#region Validaciones
            let atencionMedica = JSON.parse(data.get('atencionMedica'));
            let revision = JSON.parse(data.get('revision'));

            if (atencionMedica.clave_empleado <= 0 || isNaN(atencionMedica.clave_empleado)) {
                Alert2Warning('Debe capturar una clave de empleado válida.');
                return;
            }

            if (atencionMedica.tipo == 0) {
                Alert2Warning('Debe capturar el tipo de atención médica.');
                return;
            }

            if (!revision.terminacion && (revision.diasSiguienteRevision <= 0 || isNaN(revision.diasSiguienteRevision))) {
                Alert2Warning('Debe capturar una cantidad válida de días para la siguiente revisión.');
                return;
            }
            //#endregion

            axios.post('/Administrativo/SaludOcupacional/GuardarNuevaAtencionMedica', data, { headers: { 'Content-Type': 'multipart/form-data' } })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha guardado la información.');
                        modalCaptura.modal('hide');
                        cargarAtencionesMedicas();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function guardarNuevaRevision() {
            let data = getInformacionRevision();

            //#region Validaciones
            let revision = JSON.parse(data.get('revision'));

            if (!revision.terminacion && (revision.diasSiguienteRevision <= 0 || isNaN(revision.diasSiguienteRevision))) {
                Alert2Warning('Debe capturar una cantidad válida de días para la siguiente revisión.');
                return;
            }
            //#endregion

            axios.post('/Administrativo/SaludOcupacional/GuardarNuevaRevision', data, { headers: { 'Content-Type': 'multipart/form-data' } })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha guardado la información.');
                        modalCaptura.modal('hide');
                        cargarAtencionesMedicas();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function guardarArchivosST7ST2() {
            let data = getInformacionArchivos();

            axios.post('/Administrativo/SaludOcupacional/GuardarArchivosST7ST2', data, { headers: { 'Content-Type': 'multipart/form-data' } })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha guardado la información.');
                        modalCaptura.modal('hide');
                        cargarAtencionesMedicas();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function getInformacionAtencionMedica() {
            const data = new FormData();

            let atencionMedica = {
                clave_empleado: +inputClaveEmpleadoAgregar.val(),
                tipo: +selectTipoAtencionMedicaAgregar.val()
            }

            let revision = {
                diagnostico: textAreaDiagnostico.val(),
                tratamiento: textAreaTratamiento.val(),
                comentarios: textAreaComentarios.val(),
                incapacidad: checkboxIncapacidad.prop('checked'),
                terminacion: !checkboxTerminacion.prop('checked'),
                diasSiguienteRevision: +inputDiasSiguienteRevision.val()
            };

            data.append('atencionMedica', JSON.stringify(atencionMedica));
            data.append('revision', JSON.stringify(revision));
            data.append('archivoST7', $(`#inputArchivoST7`)[0].files.length > 0 ? $(`#inputArchivoST7`)[0].files[0] : null);
            data.append('archivoST2', $(`#inputArchivoST2`)[0].files.length > 0 ? $(`#inputArchivoST2`)[0].files[0] : null);

            return data;
        }

        function getInformacionRevision() {
            const data = new FormData();

            let revision = {
                diagnostico: textAreaDiagnostico.val(),
                tratamiento: textAreaTratamiento.val(),
                comentarios: textAreaComentarios.val(),
                incapacidad: checkboxIncapacidad.prop('checked'),
                terminacion: !checkboxTerminacion.prop('checked'),
                diasSiguienteRevision: +inputDiasSiguienteRevision.val(),
                atencionMedica_id: botonGuardar.data().id
            };

            data.append('revision', JSON.stringify(revision));
            data.append('archivoST7', $(`#inputArchivoST7`)[0].files.length > 0 ? $(`#inputArchivoST7`)[0].files[0] : null);
            data.append('archivoST2', $(`#inputArchivoST2`)[0].files.length > 0 ? $(`#inputArchivoST2`)[0].files[0] : null);

            return data;
        }

        function getInformacionArchivos() {
            const data = new FormData();

            data.append('atencionMedica_id', botonGuardar.data().id);
            data.append('archivoST7', $(`#inputArchivoST7`)[0].files.length > 0 ? $(`#inputArchivoST7`)[0].files[0] : null);
            data.append('archivoST2', $(`#inputArchivoST2`)[0].files.length > 0 ? $(`#inputArchivoST2`)[0].files[0] : null);

            return data;
        }

        function imprimirAtencionMedica(atencionMedica_id) {
            if (atencionMedica_id == 0) {
                AlertaGeneral(`Alerta`, `No se ha seleccionado una atención médica.`);
                return;
            }

            $.blockUI({ message: 'Generando imprimible...' });
            var path = `/Reportes/Vista.aspx?idReporte=246&atencionMedica_id=${atencionMedica_id}`;
            report.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => SaludOcupacional.AtencionMedica = new AtencionMedica())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();