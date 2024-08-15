(() => {
    $.namespace('SubContratistas.ConsultaArchivos');
    ConsultaArchivos = function () {
        //#region Selectores
        const tablaSubcontratistas = $('#tablaSubcontratistas');
        const modalSubcontratista = $('#modalSubcontratista');
        const inputNumeroProveedor = $('#inputNumeroProveedor');
        const inputNombre = $('#inputNombre');
        const inputDireccion = $('#inputDireccion');
        const inputNombreCorto = $('#inputNombreCorto');
        const inputCodigoPostal = $('#inputCodigoPostal');
        const inputCorreo = $('#inputCorreo');
        const inputRFC = $('#inputRFC');
        const radioFisica = $('#radioFisica');
        const radioMoral = $('#radioMoral');
        const selectFiltroCarga = $('#selectFiltroCarga');
        const modalValidacion = $('#modalValidacion');
        const botonGuardarValidacion = $('#botonGuardarValidacion');
        const tablaValidacion = $('#tablaValidacion');
        const modalHistorial = $('#modalHistorial');
        const tablaHistorial = $('#tablaHistorial');
        const modalJustificacion = $('#modalJustificacion');
        const txtComentarioJustificacion = $('#txtComentarioJustificacion');

        //#endregion

        let dtSubcontratistas;
        let dtValidacion;
        let dtHistorial;

        (function init() {
            initTablaSubcontratistas();
            initTablaValidacion();
            initTablaHistorial();
            agregarListeners();
            cargarSubcontratistas();

            selectFiltroCarga.change(cargarSubcontratistas);
            botonGuardarValidacion.click(guardarValidacion);
        })();

        function initTablaSubcontratistas() {
            dtSubcontratistas = tablaSubcontratistas.DataTable({
                retrieve: true,
                // paging: false,
                language: dtDicEsp,
                bInfo: false,
                // scrollY: '50vh',
                // scrollCollapse: true,
                initComplete: function (settings, json) {
                    tablaSubcontratistas.on('click', '.botonActaConstitutiva, .botonPoderRL, .botonINE, .botonCedulaFiscal, .botonRegistroPatronal, .botonObjetoSocialVigente, .botonRegistroEspecializacion, .botonComprobanteDomicilio', function () {
                        let rowData = dtSubcontratistas.row($(this).closest('tr')).data();

                        clickAnexo(rowData, +$(this).attr('archivofijo'));
                    });

                    tablaSubcontratistas.on('click', '.botonNoAplicaActaConstitutiva, .botonNoAplicaPoderRL, .botonNoAplicaINE, .botonNoAplicaCedulaFiscal, .botonNoAplicaRegistroPatronal, .botonNoAplicaObjetoSocialVigente, .botonNoAplicaRegistroEspecializacion, .botonNoAplicaComprobanteDomicilio', function () {
                        let rowData = dtSubcontratistas.row($(this).closest('tr')).data();
                        let archivoID = 0;

                        switch (+$(this).attr('archivofijo')) {
                            case 1:
                                archivoID = rowData.actaConstitutivaID;
                                break;
                            case 2:
                                archivoID = rowData.poderRLID;
                                break;
                            case 3:
                                archivoID = rowData.INEID;
                                break;
                            case 4:
                                archivoID = rowData.cedulaFiscalID;
                                break;
                            case 5:
                                archivoID = rowData.registroPatronalID;
                                break;
                            case 6:
                                archivoID = rowData.objetoSocialVigenteID;
                                break;
                            case 7:
                                archivoID = rowData.registroEspecializacionID;
                                break;
                            case 8:
                                archivoID = rowData.comprobanteDomicilioID;
                                break;
                        }

                        cargarJustificacionOpcional(archivoID);
                    });

                    tablaSubcontratistas.on('click', '.botonDetalles', function () {
                        let rowData = dtSubcontratistas.row($(this).closest('tr')).data();

                        cargarDetallesSubcontratista(rowData.id);
                    });

                    tablaSubcontratistas.on('click', '.botonValidacion', function () {
                        let rowData = dtSubcontratistas.row($(this).closest('tr')).data();

                        cargarDocumentacionPendiente(rowData);
                    });

                    tablaSubcontratistas.on('click', '.botonHistorial', function () {
                        let rowData = dtSubcontratistas.row($(this).closest('tr')).data();

                        cargarHistorial(rowData);
                    });
                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'numeroProveedor', title: '# Proveedor' },
                    { data: 'nombre', title: 'Nombre' },
                    {
                        title: 'Acta Constitutiva', render: function (data, type, row, meta) {
                            return row.validadoActaConstitutiva ? row.aplicaActaConstitutiva ?
                                `<button class="btn btn-xs btn-success botonRedondo botonActaConstitutiva" archivofijo="1" id="${row.actaConstitutivaID}"><i class="fa fa-check"></i></button>` :
                                `<button class="btn btn-xs btn-info botonRedondo botonNoAplicaActaConstitutiva" archivofijo="1" id="${row.actaConstitutivaID}"><i class="fa fa-check"></i></button>` :
                                `<button class="btn btn-xs btn-warning botonRedondo" disabled><i class="fa fa-times"></i></button>`;
                        }
                    },
                    {
                        title: 'Poder del RL', render: function (data, type, row, meta) {
                            return row.validadoPoderRL ? row.aplicaPoderRL ?
                                `<button class="btn btn-xs btn-success botonRedondo botonPoderRL" archivofijo="2" id="${row.poderRLID}"><i class="fa fa-check"></i></button>` :
                                `<button class="btn btn-xs btn-info botonRedondo botonNoAplicaPoderRL" archivofijo="2" id="${row.poderRLID}"><i class="fa fa-check"></i></button>` :
                                `<button class="btn btn-xs btn-warning botonRedondo" disabled><i class="fa fa-times"></i></button>`;
                        }
                    },
                    {
                        title: 'INE', render: function (data, type, row, meta) {
                            return row.validadoINE ? row.aplicaINE ?
                                `<button class="btn btn-xs btn-success botonRedondo botonINE" archivofijo="3" id="${row.INEID}"><i class="fa fa-check"></i></button>` :
                                `<button class="btn btn-xs btn-info botonRedondo botonNoAplicaINE" archivofijo="3" id="${row.INEID}"><i class="fa fa-check"></i></button>` :
                                `<button class="btn btn-xs btn-warning botonRedondo" disabled><i class="fa fa-times"></i></button>`;
                        }
                    },
                    {
                        title: 'Cédula Fiscal', render: function (data, type, row, meta) {
                            return row.validadoCedulaFiscal ? row.aplicaCedulaFiscal ?
                                `<button class="btn btn-xs btn-success botonRedondo botonCedulaFiscal" archivofijo="4" id="${row.cedulaFiscalID}"><i class="fa fa-check"></i></button>` :
                                `<button class="btn btn-xs btn-info botonRedondo botonNoAplicaCedulaFiscal" archivofijo="4" id="${row.cedulaFiscalID}"><i class="fa fa-check"></i></button>` :
                                `<button class="btn btn-xs btn-warning botonRedondo" disabled><i class="fa fa-times"></i></button>`;
                        }
                    },
                    {
                        title: 'Registro Patronal', render: function (data, type, row, meta) {
                            return row.validadoRegistroPatronal ? row.aplicaRegistroPatronal ?
                                `<button class="btn btn-xs btn-success botonRedondo botonRegistroPatronal" archivofijo="5" id="${row.registroPatronalID}"><i class="fa fa-check"></i></button>` :
                                `<button class="btn btn-xs btn-info botonRedondo botonNoAplicaRegistroPatronal" archivofijo="5" id="${row.registroPatronalID}"><i class="fa fa-check"></i></button>` :
                                `<button class="btn btn-xs btn-warning botonRedondo" disabled><i class="fa fa-times"></i></button>`;
                        }
                    },
                    {
                        title: 'Objeto Social Vigente', render: function (data, type, row, meta) {
                            return row.validadoObjetoSocialVigente ? row.aplicaObjetoSocialVigente ?
                                `<button class="btn btn-xs btn-success botonRedondo botonObjetoSocialVigente" archivofijo="6" id="${row.objetoSocialVigenteID}"><i class="fa fa-check"></i></button>` :
                                `<button class="btn btn-xs btn-info botonRedondo botonNoAplicaObjetoSocialVigente" archivofijo="6" id="${row.objetoSocialVigenteID}"><i class="fa fa-check"></i></button>` :
                                `<button class="btn btn-xs btn-warning botonRedondo" disabled><i class="fa fa-times"></i></button>`;
                        }
                    },
                    {
                        title: 'Registro Especialización', render: function (data, type, row, meta) {
                            return row.validadoRegistroEspecializacion ? row.aplicaRegistroEspecializacion ?
                                `<button class="btn btn-xs btn-success botonRedondo botonRegistroEspecializacion" archivofijo="7" id="${row.registroEspecializacionID}"><i class="fa fa-check"></i></button>` :
                                `<button class="btn btn-xs btn-info botonRedondo botonNoAplicaRegistroEspecializacion" archivofijo="7" id="${row.registroEspecializacionID}"><i class="fa fa-check"></i></button>` :
                                `<button class="btn btn-xs btn-warning botonRedondo" disabled><i class="fa fa-times"></i></button>`;
                        }
                    },
                    {
                        title: 'Comprobante Domicilio', render: function (data, type, row, meta) {
                            return row.validadoComprobanteDomicilio ? row.aplicaComprobanteDomicilio ?
                                `<button class="btn btn-xs btn-success botonRedondo botonComprobanteDomicilio" archivofijo="8" id="${row.comprobanteDomicilioID}"><i class="fa fa-check"></i></button>` :
                                `<button class="btn btn-xs btn-info botonRedondo botonNoAplicaComprobanteDomicilio" archivofijo="8" id="${row.comprobanteDomicilioID}"><i class="fa fa-check"></i></button>` :
                                `<button class="btn btn-xs btn-warning botonRedondo" disabled><i class="fa fa-times"></i></button>`;
                        }
                    },
                    {
                        title: 'Detalles', render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-primary botonDetalles"><i class="fa fa-align-justify"></i></button>`;
                        }
                    },
                    {
                        title: 'Validación', render: function (data, type, row, meta) {
                            return row.pendienteValidacion ? `<button class="btn btn-xs btn-danger botonValidacion" style="padding-left: 8px; padding-right: 8px;"><i class="fa fa-exclamation"></i></button>` : ``;
                        }
                    },
                    {
                        title: 'Historial', render: function (data, type, row, meta) {
                            return row.tieneHistorial ? `<button class="btn btn-xs btn-default botonHistorial"><i class="fa fa-align-justify"></i></button>` : ``;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '50%', targets: [1] }
                ]
            });
        }

        function initTablaValidacion() {
            dtValidacion = tablaValidacion.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                // scrollX: 'auto',
                initComplete: function (settings, json) {
                    tablaValidacion.on('change', 'input[type=file]', function () {
                        let row = $(this).closest('tr');
                        let index = $(row).index();
                        let inputArchivo = $(row).find(`.inputArchivo`);
                        let botonArchivo = $(row).find(`.botonArchivo`);
                        let iconoBoton = botonArchivo.find('i');
                        let labelArchivo = $(row).find(`.labelArchivo`);

                        if (inputArchivo[0].files.length > 0) {
                            let textoLabel = inputArchivo[0].files[0].name;

                            if (textoLabel.length > 35) {
                                textoLabel = textoLabel.substr(0, 31) + '...';
                            }

                            labelArchivo.text(textoLabel);
                            botonArchivo.addClass('btn-success');
                            botonArchivo.removeClass('btn-default');
                            iconoBoton.addClass('fa-check');
                            iconoBoton.removeClass('fa-upload');
                        } else {
                            labelArchivo.text('');
                            botonArchivo.addClass('btn-default');
                            botonArchivo.removeClass('btn-success');
                            iconoBoton.addClass('fa-upload');
                            iconoBoton.removeClass('fa-check');
                        }
                    });

                    tablaValidacion.on('click', '.radioBtn a', function () {
                        let row = $(this).closest('tr');
                        let rowData = dtValidacion.row(row).data();
                        let div = $(this).closest('div');
                        let seleccion = $(this).attr('validado');

                        div.find(`a[data-toggle="radioValidado${rowData.id}"]`).not(`[validado="${seleccion}"]`).removeClass('active').addClass('notActive');
                        div.find(`a[data-toggle="radioValidado${rowData.id}"][validado="${seleccion}"]`).removeClass('notActive').addClass('active');

                        row.find('.inputJustificacion').css('display', seleccion == 2 ? 'block' : 'none');
                        row.find('.divArchivoJustificacion').css('display', seleccion == 2 ? 'block' : 'none');
                    });

                    tablaValidacion.on('click', '.botonDescargarArchivo', function () {
                        let rowData = dtValidacion.row($(this).closest('tr')).data();

                        downloadURI(rowData.id);
                    });

                    tablaValidacion.on('click', '.botonVisorArchivo', function () {
                        let rowData = dtValidacion.row($(this).closest('tr')).data();

                        setVisor(rowData.id);
                    });
                },
                columns: [
                    { data: 'nombreDocumentacionFija', title: 'Documentación' },
                    {
                        title: 'Archivo', render: function (data, type, row, meta) {
                            return row.aplica ? `
                                <button class="btn btn-primary botonDescargarArchivo"><i class="fa fa-file"></i></button>
                                <button class="btn btn-primary botonVisorArchivo"><i class="fa fa-eye"></i></button>
                            ` : `<button class="btn btn-xs btn-default" disabled><i class="fa fa-exclamation"></i>&nbsp;NO APLICA</button>`;
                        }
                    },
                    {
                        title: 'Validación', render: function (data, type, row, meta) {
                            return `
                                <div class="radioBtn btn-group">
                                    <a class="btn btn-success notActive" data-toggle="radioValidado${row.id}" validado="1"><i class="fa fa-check"></i>&nbsp;VALIDADO</a>
                                    <a class="btn btn-danger notActive" data-toggle="radioValidado${row.id}" validado="2"><i class="fa fa-times"></i>&nbsp;RECHAZADO</a>
                                </div>`;
                        }
                    },
                    {
                        title: 'Comentario Justificación', render: function (data, type, row, meta) {
                            return `<input class="form-control inputJustificacion" style="display: none;">`;
                        }
                    },
                    {
                        title: 'Archivo Justificación', render: function (data, type, row, meta) {
                            return `
                                <div class="text-center divArchivoJustificacion" style="display: none;">
                                    <label id="botonArchivo_${meta.row}" for="inputArchivo_${meta.row}" class="btn btn-xs btn-default botonArchivo"><i class="fa fa-upload"></i></label>
                                    <label id="labelArchivo_${meta.row}" class="labelArchivo"></label>
                                    <input id="inputArchivo_${meta.row}" type="file" class="inputArchivo inputArchivo_${meta.row}" accept="application/pdf, image/*">
                                </div>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaHistorial() {
            dtHistorial = tablaHistorial.DataTable({
                retrieve: true,
                paging: true,
                language: dtDicEsp,
                dom: 'tp',
                initComplete: function (settings, json) {
                    tablaHistorial.on('click', '.botonDescargarArchivo', function () {
                        let rowData = dtHistorial.row($(this).closest('tr')).data();

                        downloadURI(rowData.id);
                    });

                    tablaHistorial.on('click', '.botonVisorArchivo', function () {
                        let rowData = dtHistorial.row($(this).closest('tr')).data();

                        setVisor(rowData.id);
                    });
                },
                columns: [
                    { data: 'nombreDocumentacionFija', title: 'Documentación' },
                    {
                        title: 'Archivo', render: function (data, type, row, meta) {
                            return row.aplica ? `
                                <button class="btn btn-xs btn-primary botonDescargarArchivo"><i class="fa fa-file"></i></button>
                                <button class="btn btn-xs btn-primary botonVisorArchivo"><i class="fa fa-eye"></i></button>
                            ` : `<button class="btn btn-xs btn-default" disabled><i class="fa fa-exclamation"></i>&nbsp;NO APLICA</button>`;
                        }
                    },
                    { data: 'justificacionValidacion', title: 'Comentario Rechazo' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarDocumentacionPendiente(rowData) {
            axios.post('GetDocumentacionPendiente', { subcontratistaID: rowData.id })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaValidacion, response.data.data);
                        modalValidacion.modal('show');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarHistorial(rowData) {
            axios.post('GetHistorialRechazado', { subcontratistaID: rowData.id })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaHistorial, response.data.data);
                        modalHistorial.modal('show');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarJustificacionOpcional(archivoID) {
            axios.post('GetJustificacionOpcional', { archivoID: archivoID })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        txtComentarioJustificacion.val(response.data.data);
                        modalJustificacion.modal('show');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarSubcontratistas() {
            axios.post('GetSubcontratistasArchivos', { filtroCarga: +selectFiltroCarga.val() })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaSubcontratistas, response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function agregarListeners() {

        }

        function clickAnexo(rowData, archivoFijo) {
            let id = 0;

            switch (archivoFijo) {
                case 1:
                    id = rowData.actaConstitutivaID;
                    break;
                case 2:
                    id = rowData.poderRLID;
                    break;
                case 3:
                    id = rowData.INEID;
                    break;
                case 4:
                    id = rowData.cedulaFiscalID;
                    break;
                case 5:
                    id = rowData.registroPatronalID;
                    break;
                case 6:
                    id = rowData.objetoSocialVigenteID;
                    break;
                case 7:
                    id = rowData.registroEspecializacionID;
                    break;
                case 8:
                    id = rowData.comprobanteDomicilioID;
                    break;
            }

            menuConfig = {
                lstOptions: [
                    { text: `<i class="fa fa-download"></i> Descargar`, action: "descargar", fn: parametros => { downloadURI(id) } },
                    { text: `<i class="fa fa-file"></i> Visor`, action: "visor", fn: parametros => { setVisor(id) } }
                ]
            }

            mostrarMenu();
        }

        function downloadURI(elemento) {
            var link = document.createElement("button");
            link.download = '/SubContratistas/SubContratistas/getFileDownload?id=' + elemento;
            link.href = '/SubContratistas/SubContratistas/getFileDownload?id=' + elemento;
            link.click();
            location.href = '/SubContratistas/SubContratistas/getFileDownload?id=' + elemento;
        }

        async function setVisor(id) {
            try {
                response = await ejectFetchJson(new URL(window.location.origin + '/SubContratistas/SubContratistas/getFileRuta'), { id });
                if (response.success) {
                    $('#myModal').data().ruta = response.ruta;
                    $('#myModal').modal('show');
                }
            } catch (o_O) { }
        }

        function cargarDetallesSubcontratista(id) {
            axios.post('GetSubcontratistaByID', { id })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        limpiarModal();
                        modalSubcontratista.modal('show');
                        llenarCamposModal(response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function limpiarModal() {
            inputNumeroProveedor.val('');
            inputNombre.val('');
            inputDireccion.val('');
            inputNombreCorto.val('');
            inputCodigoPostal.val('');
            inputCorreo.val('');
            inputRFC.val('');
            radioMoral.click();
        }

        function llenarCamposModal(data) {
            inputNumeroProveedor.val(data.numeroProveedor);
            inputNombre.val(data.nombre);
            inputDireccion.val(data.direccion);
            inputNombreCorto.val(data.nombreCorto);
            inputCodigoPostal.val(data.codigoPostal);
            inputCorreo.val(data.correo);
            inputRFC.val(data.rfc);

            if (data.fisica) {
                radioFisica.click();
            } else {
                radioMoral.click();
            }
        }

        function guardarValidacion() {
            const data = new FormData();

            let listaValidacion = [];
            let flagJustificacion = false;

            tablaValidacion.find('tbody tr').each(function (index, row) {
                let rowData = dtValidacion.row(row).data();
                let inputArchivo = $(row).find(`.inputArchivo`);
                let justificacion = $(row).find('.inputJustificacion').val();
                let archivo = inputArchivo[0].files.length > 0 ? inputArchivo[0].files[0] : null;
                let validacion = +($(row).find(`.radioBtn a.active[data-toggle=radioValidado${rowData.id}]`).attr('validado'));

                if (validacion == 2 && (archivo == null || justificacion == '')) {
                    flagJustificacion = true;
                    return;
                }

                if (archivo != null) {
                    data.append('archivos', archivo);
                }

                listaValidacion.push({
                    id: rowData.id,
                    validacion: validacion,
                    justificacionValidacion: justificacion,
                    subContratistaID: rowData.subcontratistaID
                });
            });

            if (flagJustificacion) {
                AlertaGeneral(`Alerta`, `Debe capturar la justificación del rechazo y su archivo.`);
                return;
            }

            data.append('listaValidacion', JSON.stringify(listaValidacion));

            axios.post('GuardarValidacion', data, { headers: { 'Content-Type': 'multipart/form-data' } })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha guardado la información.');
                        modalValidacion.modal('hide');
                        cargarSubcontratistas();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => SubContratistas.ConsultaArchivos = new ConsultaArchivos())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();