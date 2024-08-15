(() => {
    $.namespace('gestion.GestionOrden');

    //#region CONST
    const btnBuscar = $('#btnBuscar');
    const btnValidarFirma = $('#btnValidarFirma');


    const tblGestionDeFirmas = $('#tblGestionDeFirmas');
    let dtGestionDeFirmas;
    const cboProyecto = $('#cboProyecto');
    const cboEstatus = $('#cboEstatus');
    const tblFirmas = $('#tblFirmas');
    let dtFirmas;
    const mdlFirmas = $('#mdlFirmas');

    const inpTotalAlcance = $('#inpTotalAlcance');
    const inpTotalAutorizada = $('#inpTotalAutorizada');
    const inpTotalRechazada = $('#inpTotalRechazada');

    let _cFirmaFull = document.getElementById('canvasFirmaFull');
    let _ordenCambioID = 0;
    var signaturePadFull = new SignaturePad(_cFirmaFull, {
        backgroundColor: 'rgba(255, 255, 255, 0)',
        penColor: 'black'
    });

    const divFirmaFull = $('#divFirmaFull');
    const claveDelQueFirma = $('#claveDelQueFirma');
    const nombreDelQueFirma = $('#nombreDelQueFirma');
    const btnGuardarFirma = $('#btnGuardarFirma');
    const btnCancelarFirma = $('#btnCancelarFirma');
    const report = $('#report');
    const mdlRevision = $('#mdlRevision');
    const txtArchivoAntecedentes = $('#txtArchivoAntecedentes');
    // const txtArchivoFirmado = $('#txtArchivoFirmado');
    // const lblFileNameArchivoFirmado = $('#lblFileNameArchivoFirmado');

    const fileAlcancesNuevos = $('#fileAlcancesNuevos');
    const fileModificaciones = $('#fileModificaciones');
    const fileRequerimientos = $('#fileRequerimientos');
    const fileAjusteDeVolumenes = $('#fileAjusteDeVolumenes');
    const fileServiYSuministros = $('#fileServiYSuministros');

    const inpIdOrden = $('#inpIdOrden');

    const mdlFirmasComentario = $('#mdlFirmasComentario');
    const txtFirmasComentario = $('#txtFirmasComentario');
    const modalVoboPMO = $('#modalVoboPMO');
    const botonGuardarVoboPMO = $('#botonGuardarVoboPMO');
    //#endregion

    $(document).on('change', '.txtArchivoFirmado', function () {
        subirArchivosFirmados($(this).data('id'), $(this));
    });

    GestionOrden = function () {
        let init = () => {
            obtenertipoYID();
            eventListeners();
            initTblGestionFirmas();
            initTblFirmas();
            initSignaturePad();
        }
        init();
        ControlObra();
    }
    function obtenerPermisos() {
        axios.post('obtenerPermisos')
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    console.log(response)
                    if (items != null) {
                        items.forEach(x => {

                            if (x.permiso = "ADMINISTRADOR") {

                            } else if (x.permiso = "GESTOR OC") {

                            } else if (x.permiso = "VISOR") {

                            } else if (x.permiso = "INTERESADOS") {

                            }
                        });
                    }
                }
            });
    }
    function fncGetEstatusGlobalOrdenesCambio() {
        axios.post('GetEstatusGlobalOrdenesCambio', { cc: cboProyecto.val() }).then(response => {
            let { success, items, message } = response.data;
            if (success) {
                inpTotalAlcance.val(response.data.cantPendientes)
                inpTotalAutorizada.val(response.data.cantAutorizadas)
                inpTotalRechazada.val(response.data.cantRechazadas)
            } else {
                Alert2Error(message);
            }
        }).catch(error => Alert2Error(error.message));
    }

    function obtenertipoYID() {
        axios.post('obteniendoTipoYIDDeUsuario')
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    if (items.tipo == 3) {
                        cboProyecto.data('tipo', items.tipo);
                        cboProyecto.data('id', items.id);
                        cboProyecto.attr('disabled', true);
                    }
                }
            });
    }

    function initSignaturePad() {
        function resizeCanvasFull() {
            _cFirmaFull.width = divFirmaFull.width();
            _cFirmaFull.height = divFirmaFull.height();
            signaturePadFull.clear();
        }
        window.onresize = resizeCanvasFull;
        resizeCanvasFull();
    }

    function eventListeners() {
        $(".select2").select2()

        cboProyecto.change(function () {
            var table = tblGestionDeFirmas.DataTable();
            if (cboProyecto.val() != '') {
                table.columns([0]).visible(false, false);
            } else {
                table.columns([0]).visible(true, true);
            }
            fncGetEstatusGlobalOrdenesCambio();
            obtenerResultados();
        });

        cboProyecto.fillCombo('getProyectoOrdenCambio', null, false, null);

        btnBuscar.click(function () {
            fncGetEstatusGlobalOrdenesCambio();
            obtenerResultados();
        });

        btnCancelarFirma.click(function () {
            divFirmaFull.hide();
        });

        btnGuardarFirma.click(function () {
            let dataURL = signaturePadFull.toDataURL('image/png');
            Autorizar(btnGuardarFirma.attr('data-idOrdenDeCambio'), dataURL)
        });



        botonGuardarVoboPMO.click(guardarVobo);
    }

    function Autorizar(idOrdenDeCambio, firma) {
        axios.post('Autorizar', { idOrdenDeCambio: idOrdenDeCambio, firma: firma })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    if (items.success) {
                        AlertaGeneral('', items.items);
                        signaturePadFull.clear();
                        divFirmaFull.hide();
                    }
                }
            });
    }

    function obtenerResultados() {
        axios.post('obtenerOrdenesDeCambiabosPorAutorizar', {
            cc: cboProyecto.val(),
            estatus: cboEstatus.val(),
            idUsuario: cboProyecto.data('id')
        }).then(response => {
            let { success, items } = response.data;
            if (success) {
                AddRows(tblGestionDeFirmas, items.items)
            }
        });
    }

    function AddRows(tbl, lst) {
        dtGestionDeFirmas = tbl.DataTable();
        dtGestionDeFirmas.clear().draw();
        dtGestionDeFirmas.rows.add(lst).draw(false);
    }

    function guardarVobo() {
        let lstMontos = [];
        let tabla = $('#cboOrdenDeCambio').find('option:selected').attr('data-prefijo');
        let valor = tabla.split('-')[1];
        let formData = new FormData();

        if (document.getElementById("txtArchivoAntecedentes").files != null) {
            var file1 = document.getElementById("txtArchivoAntecedentes").files[0];
            if (file1 != undefined) {
                formData.append("AntecedentesArchivos", file1);
            }
        }

        let tr = tblAlcances.find('tr');
        for (let d = 1; d < tr.length; d++) {
            let input = $(tr[d]).find('input');
            let select = $(tr[d]).find('select');
            if ($(tblAlcances.find('tr')[1]).find('input')[0] != undefined) {

                if ($(input[1]).val() != '' && $(input[2]).val() != '' && $(input[3]).val() != '' && $(input[4]).val() != '' && $(select[0]).find('option:selected').val() != '') {
                    let item = {
                        id: $(input[0]).attr('data-id'),
                        tipoSoportes: 1,
                        no: $(input[0]).val(),
                        descripcion: $(input[1]).val(),
                        unidad: $(select[0]).find('option:selected').val(),
                        cantidad: $(input[2]).val(),
                        PrecioUnitario: unmaskNumero($(input[3]).val()),
                        importe: unmaskNumero($(input[4]).val()),
                        idOrdenDeCambio: 0,
                    }
                    lstMontos.push(item);
                }
            }
        }
        let trMod = tblModificaciones.find('tr');
        for (let d = 1; d < trMod.length; d++) {
            let input = $(trMod[d]).find('input');
            let select = $(tr[d]).find('select');
            if ($(tblModificaciones.find('tr')[1]).find('input')[0] != undefined) {

                if ($(input[1]).val() != '' && $(input[2]).val() != '' && $(input[3]).val() != '' && $(input[4]).val() != '' && $(select[0]).find('option:selected').val() != '') {
                    let item = {
                        id: $(input[0]).attr('data-id'),
                        tipoSoportes: 2,
                        no: $(input[0]).val(),
                        descripcion: $(input[1]).val(),
                        unidad: $(select[0]).find('option:selected').val(),
                        cantidad: $(input[2]).val(),
                        PrecioUnitario: unmaskNumero($(input[3]).val()),
                        importe: unmaskNumero($(input[4]).val()),
                        idOrdenDeCambio: 0,
                    }
                    lstMontos.push(item);
                }
            }
        }
        let trRe = tblReqCampo.find('tr');
        for (let d = 1; d < trRe.length; d++) {
            let input = $(trRe[d]).find('input');
            let select = $(tr[d]).find('select');
            if ($(tblReqCampo.find('tr')[1]).find('input')[0] != undefined) {

                if ($(input[1]).val() != '' && $(input[2]).val() != '' && $(input[3]).val() != '' && $(input[4]).val() != '' && $(select[0]).find('option:selected').val() != '') {
                    let item = {
                        id: $(input[0]).attr('data-id'),
                        tipoSoportes: 3,
                        no: $(input[0]).val(),
                        descripcion: $(input[1]).val(),
                        unidad: $(select[0]).find('option:selected').val(),
                        cantidad: $(input[2]).val(),
                        PrecioUnitario: unmaskNumero($(input[3]).val()),
                        importe: unmaskNumero($(input[4]).val()),
                        idOrdenDeCambio: 0,
                    }
                    lstMontos.push(item);
                }
            }
        }
        let trAju = tblAjusteDeVolumenes.find('tr');
        for (let d = 1; d < trAju.length; d++) {
            let input = $(trAju[d]).find('input');
            let select = $(tr[d]).find('select');
            if ($(tblAjusteDeVolumenes.find('tr')[1]).find('input')[0] != undefined) {

                if ($(input[1]).val() != '' && $(input[2]).val() != '' && $(input[3]).val() != '' && $(input[4]).val() != '' && $(select[0]).find('option:selected').val() != '') {
                    let item = {
                        id: $(input[0]).attr('data-id'),
                        tipoSoportes: 4,
                        no: $(input[0]).val(),
                        descripcion: $(input[1]).val(),
                        unidad: $(select[0]).find('option:selected').val(),
                        cantidad: $(input[2]).val(),
                        PrecioUnitario: unmaskNumero($(input[3]).val()),
                        importe: unmaskNumero($(input[4]).val()),
                        idOrdenDeCambio: 0,
                    }
                    lstMontos.push(item);
                }
            }
        }
        let trServ = tblServiySum.find('tr');
        for (let d = 1; d < trServ.length; d++) {
            let input = $(trServ[d]).find('input');
            let select = $(tr[d]).find('select');
            if ($(tblServiySum.find('tr')[1]).find('input')[0] != undefined) {

                if ($(input[1]).val() != '' && $(input[2]).val() != '' && $(input[3]).val() != '' && $(input[4]).val() != '' && $(select[0]).find('option:selected').val() != '') {
                    let item = {
                        id: $(input[0]).attr('data-id'),
                        tipoSoportes: 5,
                        no: $(input[0]).val(),
                        descripcion: $(input[1]).val(),
                        unidad: $(select[0]).find('option:selected').val(),
                        cantidad: $(input[2]).val(),
                        PrecioUnitario: unmaskNumero($(input[3]).val()),
                        importe: unmaskNumero($(input[4]).val()),
                        idOrdenDeCambio: 0,
                    }
                    lstMontos.push(item);
                }
            }
        }

        formData.append("editar", true);
        formData.append("id", _ordenCambioID);
        formData.append("proyecto", inpProyecto.val());
        formData.append("NoOrden", cboOrdenDeCambio.val());
        formData.append("CLiente", inpCliente.val());
        formData.append("idSubContratista", inpIdContratista.val());
        formData.append("Contratista", inpContratista.val());
        formData.append("cc", inpcc.val());
        formData.append("Estado", inpEstado.val());
        formData.append("Municipio", inpMunicipio.val());
        formData.append("Direccion", inpDireccion.val());
        formData.append("esCobrable", inpCobrable.prop('checked'));
        formData.append("Antecedentes", txtAntecedentes.val());
        formData.append("FechaAmpliacion", inpFechaAmpliacion.val());
        formData.append("ubicacionProyecto", inpUbicacion.val());
        formData.append("fechaEfectiva", dtFechaEfectiva.val());
        formData.append("idContrato", valor);
        formData.append("nombreDelArchivo", inpNombreDelArchivo.val());
        formData.append("representanteLegal", inpRespresentanteLegal.val());
        formData.append("cveEmpleados", JSON.stringify(traermeLasClaves()));
        formData.append("nombreEmpleados", fncGetNombreNotificantes());
        formData.append("lstMontos", JSON.stringify(lstMontos));
        formData.append("lstSoportesEvidencia", JSON.stringify({
            id: 0,
            idOrdenDeCambio: 0,
            alcancesNuevos: unmaskNumero(inpAlcances.val()),
            modificacionesPorCambio: unmaskNumero(inpModificaciones.val()),
            requerimientosDeCampo: unmaskNumero(inpReqCampo.val()),
            ajusteDeVolumenes: unmaskNumero(inpAjusteVol.val()),
            serviciosYSuministros: unmaskNumero(inpServiySum.val()),
            fechaInicial: inpTiempo.val(),
            FechaFinal: inpTiempoFinal.val(),
            MontoContratoOriginalSuma: inpOrdenDeCambioSum.val(),
        }));

        formData.append("voboPMO", true);

        $.ajax({
            datatype: "json",
            type: "POST",
            contentType: false,
            processData: false,
            url: "/ControlObra/ControlObra/nuevoEditarOrdenesDeCambio",
            data: formData,
            success: function (response) {
                if (response.success) {
                    Alert2Exito(response.items.items);
                    btnBuscar.click();
                } else {
                    Alert2Error(response.message);
                }
            },
            error: function () {
                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            }
        });
    }

    function initTblGestionFirmas() {
        dtGestionDeFirmas = tblGestionDeFirmas.DataTable({
            language: dtDicEsp,
            destroy: false,
            paging: true,
            ordering: true,
            searching: true,
            bFilter: false,
            info: false,
            columns: [
                { data: 'cc', title: 'CC' },
                { data: 'Proyecto', title: 'Proyecto' },
                { data: 'noContrato', title: 'No. Contrato' },
                { data: 'NoOrden', title: 'Orden cambio' },
                { data: 'Contratista', title: 'Contratista' },
                {
                    data: 'fechaEfectiva', title: 'Fecha efectiva',
                    render: (data, type, row, meta) => {
                        return moment(data).format("DD/MM/YYYY");
                    }
                },
                {
                    data: 'status', title: 'Estatus',
                    render: (data, type, row, meta) => {
                        let btnAutorizar = `<button title='Imprimir reporte orden de cambio' class="btn btn-primary btn-xs AutorizarOrdenesDeCambio"><i class="fas fa-print"></i></button>`;
                        return `${btnAutorizar}`
                    }
                },
                {
                    data: 'archivo', title: 'Descargar antecedente',
                    render: (data, type, row, meta) => {
                        return `<button title='Descargar Antecedente' class="btn btn-success btn-xs descargarArchivo"><span class="glyphicon glyphicon-file"></span></button>`;
                    }
                },
                {
                    title: 'VoBo PMO', render: (data, type, row, meta) => {
                        return !row.voboPMO ? `<button title='VoBo PMO' class="btn btn-success btn-xs botonVobo"><span class="glyphicon glyphicon-check"></span></button>` : ``;
                    }
                },
                {
                    data: 'status', title: 'Autorizar',
                    render: (data, type, row, meta) => {
                        if (cboEstatus.val() == 3 && row.voboPMO) {
                            return `<button title='Autorizar orden de cambio' class="btn btn-success btn-xs mdlAutorizar"><span class="glyphicon glyphicon-check"></span></button>`;
                        } else {
                            return ``;
                        }
                    }
                }
            ],
            initComplete: function (settings, json) {
                tblGestionDeFirmas.on("click", ".mdlAutorizar", function () {
                    const rowData = dtGestionDeFirmas.row($(this).closest("tr")).data()
                    mdlFirmas.data().filtroCC = rowData.filtroCC;
                    mdlFirmas.data().idOrdenCambio = rowData.id;
                    obtenerFirmas();

                    // if (rowData.esValidada) {
                    //     dtFirmas.column(6).visible(false);
                    //     btnValidarFirma.hide();
                    // }else{
                    //     dtFirmas.column(6).visible(true);
                    //     btnValidarFirma.show();
                    // }
                })

                tblGestionDeFirmas.on("click", ".RevisionArchivos", function () {
                    const rowData = dtGestionDeFirmas.row($(this).closest("tr")).data();
                    mdlRevision.modal('show')
                    inpIdOrden.val(rowData.id)
                    obtenerArchivos(rowData.id)
                });

                tblGestionDeFirmas.on("click", ".AutorizandoConFirma", function () {
                    const rowData = dtGestionDeFirmas.row($(this).closest("tr")).data();
                    btnGuardarFirma.attr('data-idOrdenDeCambio', rowData.id);
                    fncClick(rowData.turno);
                });

                tblGestionDeFirmas.on("click", ".AutorizoRapidoDig", function () {
                    const rowData = dtGestionDeFirmas.row($(this).closest("tr")).data();
                    Swal.fire({
                        title: '¡Alerta!',
                        text: "Desea Autorizar esta orden de cambio?",
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'si!'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            autorizarRapido(rowData.id, '');
                        }
                    })
                });

                tblGestionDeFirmas.on("click", ".RechazarRapidoDig", function () {
                    const rowData = dtGestionDeFirmas.row($(this).closest("tr")).data();
                    Swal.fire({
                        title: '¡Alerta!',
                        text: "Desea Rechazar esta orden de cambio?",
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'si!'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            rechazarRapido(rowData.id, '');
                        }
                    })
                });

                tblGestionDeFirmas.on("click", ".AutorizarOrdenesDeCambio", function () {
                    const rowData = dtGestionDeFirmas.row($(this).closest("tr")).data();
                    obtenerReporte(rowData.id);
                });
                tblGestionDeFirmas.on("click", ".descargarArchivo", function () {
                    const rowData = dtGestionDeFirmas.row($(this).closest("tr")).data();
                    DescargarArchivos(rowData.id, 1)
                });
                tblGestionDeFirmas.on("click", ".descargarArchivoFirmado", function () {
                    const rowData = dtGestionDeFirmas.row($(this).closest("tr")).data();
                    var idOrdenCambio = rowData.id;
                    DescargarArchivos(idOrdenCambio, 2);
                });

                tblGestionDeFirmas.on("click", ".subirArchivofirmado", function () {
                    const rowData = dtGestionDeFirmas.row($(this).closest("tr")).data();
                    $(this).closest("tr").find(".txtArchivoFirmado").trigger("click");
                    $(this).closest("tr").find(".txtArchivoFirmado").data('id', rowData.id);
                });
                tblGestionDeFirmas.on("click", ".autorizarArchivoFirmado", function () {
                    const rowData = dtGestionDeFirmas.row($(this).closest("tr")).data();
                    swal({
                        title: 'Alerta!',
                        text: '¿Desea autorizar el archivo firmado?.',
                        icon: 'warning',
                        buttons: true,
                        dangerMode: false,
                        buttons: ['Cerrar', 'Aceptar']
                    })
                        .then((aceptar) => {
                            if (aceptar) {
                                fncAutorizarArchivoFirmado(rowData.id);
                            }
                        });

                });

                tblGestionDeFirmas.on('click', '.botonVobo', async function () {
                    let rowData = dtGestionDeFirmas.row($(this).closest("tr")).data();

                    _ordenCambioID = rowData.id;

                    cboFiltroCC.val(rowData.filtroCC.trim());
                    cboFiltroCC.select2().change();
                    await new Promise(res => setTimeout(res, 2000)); //Esperar dos segundos para que se termine de llenar el combo de contratos/órdenes de cambio. Cambiar esto.
                    cboOrdenDeCambio.val(rowData.id);
                    cboOrdenDeCambio.select2().change();

                    modalVoboPMO.modal('show');
                });
            },
            columnDefs: [
                { className: 'dt-center', 'targets': '_all' },
                { className: 'dt-body-center', 'targets': '_all' },
                { width: '30%', targets: [0, 1, 4] }
            ],
        });
    }

    function fncAutorizarArchivoFirmado(id) {
        let obj = {};
        obj.id = id
        obj.esValidada = true;
        axios.post('autorizarArchivoFirmado', obj).then(response => {
            let { success, items, message } = response.data;
            if (success) {
                obtenerResultados();
            } else {
                Alert2Error(message)
            }
        })
    }

    function subirArchivosFirmados(id, input) {
        // var file1 = input.files[0];
        let fileName = "";
        fileName = $(input)[0].files[0].name;

        var idOrdenCambio = id;
        let formData = new FormData();
        formData.append("ArchivoFirmado", $(input)[0].files[0]);

        formData.append("idOrdenDeCambio", idOrdenCambio);
        $.ajax({
            datatype: "json",
            type: "POST",
            contentType: false,
            processData: false,
            url: "/ControlObra/ControlObra/GuardarDocumentoFirmado",
            data: formData,
            success: function (response) {
                if (response.success) {
                    btnBuscar.click();
                    Alert2Exito(response.items.items);
                } else {
                    Alert2Error(response.message);
                }
            },
            error: function () {
                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            },
            complete: function () {
            }
        });
    }

    function enviarCorre(idOrdenDeCambio, tipoMail) {
        axios.post('EnviarCorreo', { idOrdenDeCambio: idOrdenDeCambio, tipoCorreo: tipoMail })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                }
            });
    }

    function obtenerReporte(idOrdenDeCambio) {
        report.attr(`src`, `/Reportes/Vista.aspx?idReporte=242&idOrdenDeCambio=${idOrdenDeCambio}`);
        document.getElementById('report').onload = function () {
            $.unblockUI();
            openCRModal();
        }
    }
    function autorizarRapido(idOrdenDeCambio, firma) {
        axios.post('Autorizar', { idOrdenDeCambio: idOrdenDeCambio, firma: firma })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    if (items.success) {
                        AlertaGeneral('', items.items);
                        enviarCorre(idOrdenDeCambio, 1);
                        obtenerResultados();
                    }
                }
            });
    }

    function rechazarRapido(idOrdenDeCambio, firma) {
        axios.post('Rechazar', { idOrdenDeCambio: idOrdenDeCambio, firma: firma })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    if (items.success) {
                        AlertaGeneral('', items.items);
                        obtenerResultados();
                    }
                }
            });
    }

    function fncClick(botonFirma) {
        btnGuardarFirma.attr('data-Firma', botonFirma)
        divFirmaFull.show();
        _cFirmaFull.width = divFirmaFull.width();
        _cFirmaFull.height = document.body.clientHeight;

        signaturePadFull.clear();
    }

    function obtenerFirmas() {
        if (mdlFirmas.data().filtroCC != null && mdlFirmas.data().idOrdenCambio > 0) {
            let obj = {}
            obj.filtroCC = mdlFirmas.data().filtroCC
            obj.filtroOrdenCambioID = mdlFirmas.data().idOrdenCambio
            axios.post('obtenerTodasLasFirmas', obj).then(response => {
                let { success, items } = response.data;
                if (success) {
                    dtFirmas.clear()
                    dtFirmas.rows.add(response.data.lstFirmasDTO)
                    dtFirmas.draw()
                    mdlFirmas.modal('show');
                    // if(response.data.ultimoFirmante == true)
                    // {
                    //     btnValidarFirma.show();
                    // }
                    // else
                    // {
                    //     btnValidarFirma.hide();
                    // }

                } else {
                    Alert2Error(message)
                }
            })
        } else {
            let mensajeError = "Ocurrió un error al obtener el listado de autorizantes."
            if (mdlFirmas.data().filtroCC == null) { Alert2Error(mensajeError) }
            if (mdlFirmas.data().idOrdenCambio <= 0) { Alert2Error(mensajeError) }
        }
    }

    function initTblFirmas() {
        dtFirmas = tblFirmas.DataTable({
            language: dtDicEsp,
            destroy: false,
            paging: false,
            ordering: false,
            searching: false,
            bFilter: false,
            info: false,
            columns: [
                { data: 'puesto', title: 'Puesto' },
                { data: 'nombre_completo', title: 'Nombre' },
                {
                    data: 'fechaAutorizacion', title: 'Fecha autorización',
                    render: (data, type, row, meta) => {
                        if (data != null) {
                            return moment(row.fechaAutorizacion).format("DD/MM/YYYY");
                        } else {
                            return '';
                        }
                    }
                },
                { data: 'firmaDigital', title: 'Firma', visible: false },
                { data: 'strEstatusFirma', title: 'Estado' },
                {
                    title: "Autorizar",
                    render: (data, type, row, meta) => {
                        if (row.puedeAutorizar) {
                            if (row.estatusFirma == 3) {
                                return "<button class='btn btn-sm btn-secondary verComentario' title='Ver comentarios de factura removida'><i class='far fa-comments'></i></button>";

                            } else {
                                let btnAutorizar = `<button class="btn btn-xs btn-success autorizar" title="Autorizar orden de cambio."><i class="fas fa-check-circle"></i></button>`
                                let btnRechazar = `<button class="btn btn-xs btn-danger rechazar" title="Rechazar orden de cambio."><i class="fas fa-times-circle"></i></button>`
                                return `${btnAutorizar} ${btnRechazar}`
                            }

                        } else {
                            if (row.estatusFirma == 3) {
                                return "<button class='btn btn-sm btn-secondary verComentario' title='Ver comentarios de factura removida'><i class='far fa-comments'></i></button>";

                            } else {
                                return `-`;

                            }
                        }
                    }
                },
            ],
            initComplete: function (settings, json) {
                tblFirmas.on('click', '.autorizar', function () {
                    Alert2AccionConfirmar('Autorizar', '¿Desea autorizar el orden de cambio?', 'Confirmar', 'Cancelar', () => fncAutorizarRechazarOrdenCambio(true, mdlFirmas.data().idOrdenCambio))
                });

                tblFirmas.on('click', '.rechazar', function () {
                    Swal.fire({
                        position: "center",
                        icon: "warning",
                        title: "Rechazar",
                        input: 'textarea',
                        width: '50%',
                        showCancelButton: true,
                        html: "<h3>¿Desea rechazar el orden de cambio?</h3><br><h3>Comentario:</h3>",
                        confirmButtonText: "Confirmar",
                        confirmButtonColor: "#449d44",
                        cancelButtonText: "Cancelar",
                        cancelButtonColor: "#5c636a",
                        showCloseButton: true
                    }).then((result) => {
                        if (result.isConfirmed) {
                            fncAutorizarRechazarOrdenCambio(false, mdlFirmas.data().idOrdenCambio, $('.swal2-textarea').val());
                        } else {

                        }
                    });
                    // Alert2AccionConfirmar('Rechazar','¿Desea rechazar el orden de cambio?','Confirmar','Cancelar', () => fncAutorizarRechazarOrdenCambio(false, mdlFirmas.data().idOrdenCambio))
                });

                tblFirmas.on('click', '.verComentario', function () {
                    const rowData = dtFirmas.row($(this).closest("tr")).data();

                    txtFirmasComentario.val(rowData.comentarioRechazo);
                    mdlFirmasComentario.modal("show");
                });
                // tblFirmas.on("click", ".subirArchivofirmado", function () {  

                // const rowData = dtFirmas.row($(this).closest("tr")).data();

                // $(this).closest('tr').find('.txtArchivoFirmado').trigger("click");

                //     $(this).closest('tr').find('.txtArchivoFirmado').on("change", function () {
                //         let fileName = "";
                //             fileName = $(this)[0].files[0].name;
                //             $('.lblFileNameArchivoFirmado').text(fileName);
                //     });                 
                // });
                // tblFirmas.on("click", ".guardarDocumentoFirmado", function () {   
                //     const rowData = dtFirmas.row($(this).closest("tr")).data();                     
                //     var idOrdenCambio=mdlFirmas.data().idOrdenCambio;
                //     let formData = new FormData();
                //     if (document.getElementById("txtArchivoFirmado") != null) {
                //         var file1 = document.getElementById("txtArchivoFirmado").files[0];
                //         if (file1 != undefined) {
                //             formData.append("ArchivoFirmado", file1);
                //         }
                //     }                   

                //     formData.append("idOrdenDeCambio", idOrdenCambio);                    
                //     $.ajax({
                //         datatype: "json",
                //         type: "POST",
                //         contentType: false,
                //         processData: false,
                //         url: "/ControlObra/ControlObra/GuardarDocumentoFirmado",
                //         data: formData,                
                //         success: function (response) {
                //             if (response.success) {
                //                 Alert2Exito(response.items.items);  
                //                 obtenerFirmas();    

                //             } else {
                //                 Alert2Error(response.message);
                //             }
                //         },
                //         error: function () {
                //             AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                //         },
                //         complete: function () {
                //         }
                //     });                   

                // });
                // tblFirmas.on("click", ".descargarArchivoFirmado", function () {                    
                //     const rowData = dtFirmas.row($(this).closest("tr")).data();  
                //     var idOrdenCambio=mdlFirmas.data().idOrdenCambio;                   
                //     DescargarArchivos(idOrdenCambio,2) 
                // });
            },
            columnDefs: [
                { className: 'dt-center', 'targets': '_all' },
                { className: 'dt-body-center', 'targets': '_all' }
            ]
        });
    }
    function alert2Exito(mensaje) {
        Swal.fire({
            position: 'top-end',
            icon: 'success',
            title: mensaje,
            showConfirmButton: false,
            timer: 1500
        });
    }
    function fncAutorizarRechazarOrdenCambio(esAutorizar, idOrdenCambio, comentarioRechazo) {
        if (idOrdenCambio > 0) {
            let obj = {}
            obj.esAutorizar = esAutorizar;
            obj.idOrdenCambio = idOrdenCambio;
            obj.comentarioRechazo = comentarioRechazo;

            axios.post('AutorizarRechazarOrdenCambio', obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    obtenerFirmas();
                    fncGetEstatusGlobalOrdenesCambio();
                    obtenerResultados();
                    Alert2Exito(message);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
    }

    function visualizarArchivoGuardado(elemento, archivos, tipoAnexo) {
        $(elemento).closest('.panel-body').removeClass('p-primary');
        $(elemento).closest('.panel-body').addClass('p-success');
        $(elemento).closest('.panel-body').find('i').removeClass('fa-file-alt');
        $(elemento).closest('.panel-body').find('i').addClass('fa-check');

        let rutaSeccionada = archivos.split('\\');

        $(elemento).closest('.panel-body').find('.labelNombre').text(rutaSeccionada[rutaSeccionada.length - 1]);
    }
    function visualizarArchivoNoCargado(elemento) {
        $(elemento).closest('.panel-body').addClass('p-primary');
        $(elemento).closest('.panel-body').removeClass('p-success');
        $(elemento).closest('.panel-body').find('i').addClass('fa-file-alt');
        $(elemento).closest('.panel-body').find('i').removeClass('fa-check');

        $(elemento).closest('.panel').find('.panel-title').find('.botonVerArchivos').remove();
    }
    function obtenerArchivos(idOrdenDeCambio) {
        axios.post('obtenerArchivos', { id: idOrdenDeCambio })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    items.items.antecedentesArchivos != "" ? visualizarArchivoGuardado(txtArchivoAntecedentes, items.items.antecedentesArchivos) : visualizarArchivoNoCargado(txtArchivoAntecedentes);

                }
            });
    }
    function btnActions() {
        txtArchivoAntecedentes.click(function () {
            DescargarArchivos(inpIdOrden.val(), 1)
        });
        // fileModificaciones.click(function () {
        //    DescargarArchivos(inpIdOrden.val(),2) 
        // });
        // fileRequerimientos.click(function () {
        //    DescargarArchivos(inpIdOrden.val(),3) 
        // });
        // fileAjusteDeVolumenes.click(function () {
        //    DescargarArchivos(inpIdOrden.val(),4) 
        // });
        // fileServiYSuministros.click(function () {
        //    DescargarArchivos(inpIdOrden.val(),5) 
        // });
    }
    function DescargarArchivos(idDet, tipo) {
        location.href = `DescargarArchivos?idDet=${idDet}&Tipo=${tipo}`;
    }

    function limpiarSoportes() {
        dtAlcances.clear();
        dtAlcances.rows.add(lstDetalle);
        dtAlcances.draw();

        lstDetalle = [{
            id: 0,
            no: '',
            descripcion: '',
            cantidad: '',
            unidad: '',
            PrecioUnitario: '',
            importe: '',
            nuevo: true
        }];

        dtModificaciones.clear();
        dtModificaciones.rows.add(lstDetalleMod);
        dtModificaciones.draw();
        lstDetalleMod = [{
            id: 0,
            no: '',
            descripcion: '',
            cantidad: '',
            unidad: '',
            PrecioUnitario: '',
            importe: '',
            nuevo: true
        }];

        dtReqCampo.clear();
        dtReqCampo.rows.add(lstDetalleReq);
        dtReqCampo.draw();

        lstDetalleReq = [{
            id: 0,
            no: '',
            descripcion: '',
            cantidad: '',
            unidad: '',
            PrecioUnitario: '',
            importe: '',
            nuevo: true
        }];

        dtAjusteDeVolumenes.clear();
        dtAjusteDeVolumenes.rows.add(lstDetalleAjust);
        dtAjusteDeVolumenes.draw();

        lstDetalleAjust = [{
            id: 0,
            no: '',
            descripcion: '',
            cantidad: '',
            unidad: '',
            PrecioUnitario: '',
            importe: '',
            nuevo: true
        }];

        dtServiySum.clear();
        dtServiySum.rows.add(lstDetalleServ);
        dtServiySum.draw();
        lstDetalleServ = [{
            id: 0,
            no: '',
            descripcion: '',
            cantidad: '',
            unidad: '',
            PrecioUnitario: '',
            importe: '',
            nuevo: true
        }];


    };

    //#region Script Captura
    const tblOrdenesDeCambio = $('#tblOrdenesDeCambio');
    let dtOrdenDeCambios;
    const btnNuevo = $('#btnNuevo');
    const btnEditar = $('#btnEditar');
    const btnGestionar = $('#btnGestionar');
    const btnImprimir = $('#btnImprimir');



    // const cboProyecto = $('#cboProyecto');
    const lbltitle = $('#lbltitle');
    const mdlgMontosSoportes = $('#mdlgMontosSoportes');
    // const inpDias = $('#inpDias');
    // const inpDias2 = $('#inpDias2');
    // const inpDias3 = $('#inpDias3');
    const btnArchivoAntecedentes = $('#btnArchivoAntecedentes');
    const lblFileName = $('#lblFileName');
    const btnArchivoAlcancesNuevos = $("#btnArchivoAlcancesNuevos");
    const lblFileNameAlcancesNuevos = $("#lblFileNameAlcancesNuevos");

    const cboFiltroCC = $('#cboFiltroCC');
    const cboOrdenDeCambio = $('#cboOrdenDeCambio');
    const cboContratista = $('#cboContratista');

    const tblAlcances = $('#tblAlcances');
    let dtAlcances;
    let lstDetalle = [{
        id: 0,
        no: '',
        descripcion: '',
        cantidad: '',
        unidad: '',
        PrecioUnitario: '',
        importe: '',
        nuevo: true
    }];
    let lstDetalleAgregar = [{
        id: 0,
        no: '',
        descripcion: '',
        cantidad: '',
        unidad: '',
        PrecioUnitario: '',
        importe: '',
        nuevo: true
    }];

    const tblModificaciones = $('#tblModificaciones ');
    let dtModificaciones;
    let lstDetalleMod = [{
        id: 0,
        no: '',
        descripcion: '',
        cantidad: '',
        unidad: '',
        PrecioUnitario: '',
        importe: '',
        nuevo: true
    }];
    let lstDetalleAgregarMod = [{
        id: 0,
        no: '',
        descripcion: '',
        cantidad: '',
        unidad: '',
        PrecioUnitario: '',
        importe: '',
        nuevo: true
    }];

    const tblReqCampo = $('#tblReqCampo ');
    let dtReqCampo;
    let lstDetalleReq = [{
        id: 0,
        no: '',
        descripcion: '',
        cantidad: '',
        unidad: '',
        PrecioUnitario: '',
        importe: '',
        nuevo: true
    }];
    let lstDetalleAgregarReq = [{
        id: 0,
        no: '',
        descripcion: '',
        cantidad: '',
        unidad: '',
        PrecioUnitario: '',
        importe: '',
        nuevo: true
    }];

    const tblAjusteDeVolumenes = $('#tblAjusteDeVolumenes ');
    let dtAjusteDeVolumenes;
    let lstDetalleAjust = [{
        id: 0,
        no: '',
        descripcion: '',
        cantidad: '',
        unidad: '',
        PrecioUnitario: '',
        importe: '',
        nuevo: true
    }];
    let lstDetalleAgregarAjust = [{
        id: 0,
        no: '',
        descripcion: '',
        cantidad: '',
        unidad: '',
        PrecioUnitario: '',
        importe: '',
        nuevo: true
    }];

    const tblServiySum = $('#tblServiySum ');
    let dtServiySum;
    let lstDetalleServ = [{
        id: 0,
        no: '',
        descripcion: '',
        cantidad: '',
        unidad: '',
        PrecioUnitario: '',
        importe: '',
        nuevo: true
    }];
    let lstDetalleAgregarServ = [{
        id: 0,
        no: '',
        descripcion: '',
        cantidad: '',
        unidad: '',
        PrecioUnitario: '',
        importe: '',
        nuevo: true
    }];

    let valorInputSumaTotalModificaciones = 0;
    let valorInputOrdenCambioActual = 0;

    const contenidoNotificantes = $('#contenidoNotificantes');

    const btnMasModificaciones = $('#btnMasModificaciones');
    const btnMasReqCampo = $('#btnMasReqCampo');
    const btnMasAjusteDeVolumenes = $('#btnMasAjusteDeVolumenes');
    const btnMasServiySum = $('#btnMasServiySum');

    const inpAlcancesDescripcion = $('#inpAlcancesDescripcion');
    const inpModificacionesDescripcion = $('#inpModificacionesDescripcion');
    const inpReqCampoDescripcion = $('#inpReqCampoDescripcion');
    const inpAjusteVolDescripcion = $('#inpAjusteVolDescripcion');
    const inpServiySumDescripcion = $('#inpServiySumDescripcion');
    const inpfechaDescripcion = $('#inpfechaDescripcion');

    const btnMas = $('#btnMas');
    // const btnMenos = $('#btnMenos');
    // const cboOrdenDeCambio = $('#cboOrdenDeCambio');

    //#region Division Generales

    const inpProyecto = $('#inpProyecto');
    const inpCliente = $('#inpCliente');
    const inpEstado = $('#inpEstado');
    const inpMunicipio = $('#inpMunicipio');
    const inpCobrable = $('#inpCobrable');
    const inpDireccion = $('#inpDireccion');

    const inpOrden = $('#inpOrden');
    const inpUbicacion = $('#inpUbicacion');
    const inpNoContrato = $('#inpNoContrato');
    const inpIdContratista = $('#inpIdContratista');
    const inpContratista = $('#inpContratista');
    const inpcc = $('#inpcc');
    const dtFechaEfectiva = $('#dtFechaEfectiva');

    //#endregion

    //#region Division Variacion Plazos

    const inpFechaFinalContrato = $('#inpFechaFinalContrato');
    const inpFechaInicioContrato = $('#inpFechaInicioContrato');
    const inpDias = $('#inpDias');

    const inpFechaFinalPrevias = $('#inpFechaFinalPrevias');
    const inpDiasPrevias = $('#inpDiasPrevias');

    const inpFechaAmpliacion = $('#inpFechaAmpliacion');
    const inpDiasAmpliacion = $('#inpDiasAmpliacion');

    //#endregion


    //#region Montos
    const inpMontocontractual = $('#inpMontocontractual');
    const inpMontoTotalOrdenPrevia = $('#inpMontoTotalOrdenPrevia');
    const inpTotalMontoActual = $('#inpTotalMontoActual');
    const inpSumaTotalModificaciones = $('#inpSumaTotalModificaciones');
    const inpTotalMontoOrdenCambioActual = $('#inpTotalMontoOrdenCambioActual');
    //#endregion


    // const inpProyecto = $('#inpProyecto');
    // const inpOrden = $('#inpOrden');
    // const inpCliente = $('#inpCliente');
    // const inpCobrable = $('#inpCobrable');
    // const inpUbicacion = $('#inpUbicacion');
    // const inpNoContrato = $('#inpNoContrato');
    // const inpContratista = $('#inpContratista');
    // const inpcc = $('#inpcc');
    // const inpDireccion = $('#inpDireccion');
    // const dtFechaEfectiva = $('#dtFechaEfectiva');

    const inpAlcances = $('#inpAlcances');
    const inpModificaciones = $('#inpModificaciones');
    const inpReqCampo = $('#inpReqCampo');
    const inpAjusteVol = $('#inpAjusteVol');
    const inpServiySum = $('#inpServiySum');
    const inpTiempo = $('#inpTiempo');
    const inpTiempoFinal = $('#inpTiempoFinal');
    const txtAntecedentes = $('#txtAntecedentes');
    // const txtCondicions = $('#txtCondicions');

    const inpOriginal = $('#inpOriginal');
    const inpOrdenDeCambioSum = $('#inpOrdenDeCambioSum');
    const mdlFormularioDeFirmas = $('#mdlFormularioDeFirmas');

    var signaturePadFull = new SignaturePad(_cFirmaFull, {
        backgroundColor: 'rgba(255, 255, 255, 0)',
        penColor: 'black'
    });

    const Firma1 = $('#Firma1');
    const Firma2 = $('#Firma2');
    const Firma3 = $('#Firma3');
    const Firma4 = $('#Firma4');
    const Firma5 = $('#Firma5');

    const imgFirma1 = $('#imgFirma1');
    const imgFirma2 = $('#imgFirma2');
    const imgFirma3 = $('#imgFirma3');
    const imgFirma4 = $('#imgFirma4');
    const imgFirma5 = $('#imgFirma5');

    const btnlimpiar = $('#btnlimpiar');

    const btnGuardarFirmas = $('#btnGuardarFirmas');

    const tabAlcances = $('#tabAlcances');
    const dateFormat = "dd/mm/yy";
    const showAnim = "slide";
    const fechaActual = new Date();

    const selAutorizanteSubContratista1 = $('#selAutorizanteSubContratista1');
    const selAutorizanteSubContratista2 = $('#selAutorizanteSubContratista2');
    const selAutorizante1 = $('#selAutorizante1');
    const selAutorizante2 = $('#selAutorizante2');
    const selAutorizante3 = $('#selAutorizante3');
    //#region Firmas
    const inpRespresentanteLegal = $('#inpRespresentanteLegal');
    const inpDirectorTecnico = $('#inpDirectorTecnico');
    const inpGerentePMO = $('#inpGerentePMO');
    const inpGerenteArea = $('#inpGerenteArea');
    const inpGerenteProyecto = $('#inpGerenteProyecto');
    const inpDirectorDivision = $('#inpDirectorDivision');

    const inpIdDirectorTecnico = $('#inpIdDirectorTecnico');
    const inpIdGerentePMO = $('#inpIdGerentePMO');
    const inpIdGerenteArea = $('#inpIdGerenteArea');
    const inpIdGerenteProyecto = $('#inpIdGerenteProyecto');
    const inpIdDirectorDivision = $('#inpIdDirectorDivision');

    //#endregion
    $('a.accordion-toggle').on('click', function () {
        $(this).parent().parent().toggleClass('inactivo')
    });

    // $('#accordion').on('show.bs.collapse', function (evt) {
    //     //evt.target es el elemento al que se le ha hecho click
    //     $(evt.target).prev().removeClass('inactivo')
    // })
    // //Detectando cuando se cierra el collapse se agrega el fondo rojo
    // $('#accordion').on('hide.bs.collapse', function (evt) {
    //     //evt.target es el elemento al que se le ha hecho click
    //     $(evt.target).prev().addClass('inactivo')
    // })


    const DtGenerales = $('#DtGenerales');
    const Antecedentes = $('#Antecedentes');
    const Soporte = $('#Soporte');
    const otrasCondiciones = $('#otrasCondiciones');
    const firmas = $('#firmas');
    const tabDatosGenerales = $('#tabDatosGenerales');
    const tabAntecedentes = $('#tabAntecedentes');
    const tabSoporte = $('#tabSoporte');
    const tabCondiciones = $('#tabCondiciones');
    const tabFirmas = $('#tabFirmas');
    const tabNotificantes = $('#tabNotificantes');
    const Notificantes = $('#Notificantes');

    const fileAntecedentes = $('#fileAntecedentes');
    const inpNombreDelArchivo = $('#inpNombreDelArchivo');




    //FLAG NOTIFICANTES
    let tieneNotificantes = false;

    ControlObra = function () {
        let init = () => {
            // reporte();
            // btnNuevo.prop("disabled", true);
            // obtenerPermisos();
            // initDataTblOrdenDeCambios();
            initDatatblAlcances();
            inittblModificaciones();
            inittblReqCampo();
            inittblServ();
            inittblAjust();

            // inittblsSoportes();


            // obtenerDatos();
            EventListener();
            initSignaturePad();
            // inpTiempoFinal.datepicker({ dateFormat, maxDate: fechaActual, showAnim, beforeShow: function (input, inst) { inst.dpDiv.removeClass('month_year_datepicker'); } }).datepicker("setDate", fechaActual);

            // inpTiempoFinal.datepicker({
            //     "dateFormat": "dd/mm/yy",
            // }).datepicker("option", "showAnim", "slide")

            // inpFechaAmpliacion.datepicker({
            //     "dateFormat": "dd-mm-yy",
            // }).datepicker("option", "showAnim", "slide")
            // // dtFechaEfectiva.datepicker({ dateFormat: 'dd/mm/yy' });
            inpTiempo.datepicker({ dateFormat: 'yy-mm-dd' });




            inpTiempoFinal.datepicker({
                "dateFormat": "yy-mm-dd",
            }).datepicker("option", "showAnim", "slide")

            // inpFechaAmpliacion.datepicker({
            //     "dateFormat": "dd/mm/yy",
            // }).datepicker("option", "showAnim", "slide")

            inpFechaAmpliacion.datepicker({
                "dateFormat": "yy-mm-dd",
            });
            dtFechaEfectiva.datepicker({
                "dateFormat": "yy-mm-dd",
            });
            // inpTiempo.datepicker({ dateFormat: 'dd/mm/yy' });

            // inpTiempoFinal.datepicker({ dateFormat: 'dd/mm/yy' });
            // inittblsSoportes();
            // initButtons();
            inpFechaAmpliacion.on("change", function () {
                var fechaInicio = new Date(inpFechaInicioContrato.val());
                var fechaAmpliacion = new Date(inpFechaAmpliacion.val());
                var difMili = fechaAmpliacion - fechaInicio;
                var difAmp = difMili / (1000 * 60 * 60 * 24);
                inpDiasAmpliacion.val(difAmp);

            });

            btnArchivoAntecedentes.on("click", function () {
                txtArchivoAntecedentes.trigger("click");
            });

            txtArchivoAntecedentes.on("change", function () {
                let fileName = "";
                if ($(this).val() != "") {
                    fileName = $(this)[0].files[0].name;
                    lblFileName.text(fileName);
                } else {
                    lblFileName.text(fileName);
                }
            });


            // btnArchivoAlcancesNuevos.on("click", function () {
            //     fileAlcancesNuevos.trigger("click");
            // });

            // fileAlcancesNuevos.on("change", function () {
            //     let fileName = "";
            //     if ($(this).val() != "") {
            //         fileName = $(this)[0].files[0].name;
            //         lblFileNameAlcancesNuevos.text(fileName);
            //     } else {
            //         lblFileNameAlcancesNuevos.text(fileName);
            //     }
            // });


        }
        init();
    }
    function obtenerPermisos() {
        axios.post('obtenerPermisos')
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    if (items.permiso = "ADMINISTRADOR") {
                        btnGestionar.prop("disabled", false);
                    } else if (items.permiso = "GESTOR OC") {
                        btnGestionar.prop("disabled", false);
                    } else if (items.permiso = "VISOR") {
                        btnGestionar.prop("disabled", true);
                    } else if (items.permiso = "INTERESADOS") {
                        btnGestionar.prop("disabled", true);
                    }

                }
            });
    }
    function initAutoCompletes() {
        selAutorizanteSubContratista1.getAutocompleteValidSubCon(fnSelRevisa, fnSelNull, null, null, 'getUsuarioSelectWithEitemsceptionSubContratista');
        selAutorizanteSubContratista2.getAutocompleteValidSubCon(fnSelRevisa, fnSelNull, null, cboFiltroCC.val().split('-')[0].trim(), 'getUsuarioSelectWithExceptionConstruplan');
        selAutorizante1.getAutocompleteValidSubCon(fnSelRevisa, fnSelNull, null, cboFiltroCC.val().split('-')[0].trim(), 'getUsuarioSelectWithExceptionConstruplan');
        selAutorizante2.getAutocompleteValidSubCon(fnSelRevisa, fnSelNull, null, cboFiltroCC.val().split('-')[0].trim(), 'getUsuarioSelectWithExceptionConstruplan');
        selAutorizante3.getAutocompleteValidSubCon(fnSelRevisa, fnSelNull, null, cboFiltroCC.val().split('-')[0].trim(), 'getUsuarioSelectWithExceptionConstruplan');
    }
    function fnSelRevisa(event, ui) {
        $(this).data("id", ui.item.id);
        $(this).data("nombre", ui.item.value);
    }
    function fnSelNull(event, ui) {
        if (ui.item === null && $(this).val() != '') {
            $(this).val("");
            $(this).data("id", "");
            $(this).data("nombre", "");
            AlertaGeneral("Alerta", "Solo puede seleccionar un usuario de la lista, si no aparece en la lista de autocompletado favor de solicitar al personal de TI");
        }
    }
    function EventListener() {
        $(".select2").select2()
        // cboFiltroCC.fillCombo('obtenerCC', null, false, null);
        cboFiltroCC.fillCombo('getProyecto', null, false, null);
        // cboOrdenDeCambio.fillCombo('obtenerContratos', null, false, null);
        cboFiltroCC.on("change", function () {
            limpiarCampos(false);
            cboOrdenDeCambio.fillCombo('comboObtenerContratosyUltimasOrdenesDeCambio', { filtroCC: getValoresMultiples('#cboFiltroCC') }, false, null);
        });
        cboOrdenDeCambio.change(function () {
            limpiarCampos(false, false);
            let id = cboOrdenDeCambio.val();
            let tabla = $('#cboOrdenDeCambio').find('option:selected').attr('data-prefijo');
            if (cboOrdenDeCambio.val() != null && tabla != undefined) {
                let valor = (tabla.split('-')[0]);
                llenarCamposVacios(id, valor)
            }
        });
        inpSumaTotalModificaciones.on("change", function () {
            let valor1 = inpTotalMontoActual.val()
            let valor2 = inpSumaTotalModificaciones.val()
            let valor3 = 0

            if (valor1 != "" && valor2 != "") {
                valor3 = valor1 + valor2
                inpSumaTotalModificaciones.val(valor3)
            }

            // inpTotalMontoOrdenCambioActual.val(Number(inpTotalMontoActual.val()) + Number(inpSumaTotalModificaciones.val()));
        });
        // cboOrdenDeCambio.on("change", function () {
        //     cboContratista.fillCombo('obtenerContratistas', { filtroContrato: getValoresMultiples('#cboOrdenDeCambio') }, false, null);
        //     convertToMultiselect('#cboContratista');

        //     llenarCamposVacios();

        // });

        inpModificaciones.change(function () {
            console.log("1");
            let valor1 = $(this).val()
            let valor2 = valor1
            inpModificaciones.val(valor2)
        })
        $('#btnLimpiarMods').on("click", function () {
            limpiarSoportes();
        });



        imgFirma1.attr("src", '');
        imgFirma2.attr("src", '');
        imgFirma3.attr("src", '');
        imgFirma4.attr("src", '');
        imgFirma5.attr("src", '');
        Firma1.attr('data-id', 0);
        Firma2.attr('data-id', 0);
        Firma3.attr('data-id', 0);
        Firma4.attr('data-id', 0);
        Firma5.attr('data-id', 0);

        // inpTiempo.prop('disabled', true);
        // inpFechaFinalContrato.prop('disabled', true);
        // inpFechaInicioContrato.prop('disabled', true);
        // cboOrdenDeCambio.prop('disabled', false);
        // inpProyecto.prop('disabled', true);
        // inpOrden.prop('disabled', true);
        // inpCliente.prop('disabled', true);
        // inpNoContrato.prop('disabled', true);
        // inpContratista.prop('disabled', true);
        // inpcc.prop('disabled', true);
        // inpDireccion.prop('disabled', true);
        // inpOriginal.prop('disabled', true);
        // inpOrdenDeCambioSum.prop('disabled', true);
        $('#inpCobrable').bootstrapToggle('off')

        // btnNuevo.click(function () {
        //     mdlgMontosSoportes.modal('show');
        //     $('#tabNotificantes').css('display', 'none');
        //     $('#tabFirmas').css('display', 'block');
        //     limpiarCampos();
        lbltitle.text('NUEVA ORDEN DE CAMBIO');
        //     btnEditar.text('Siguiente');
        // btnGestionar.text('Nuevo');
        btnGestionar.attr('data-editar', false)
        btnGestionar.attr('data-id', 0);
        //     selAutorizanteSubContratista1.data('id', '');
        //     selAutorizanteSubContratista1.val('');
        //     selAutorizanteSubContratista2.data('id', '');
        //     selAutorizanteSubContratista2.val('');
        //     selAutorizante1.data('id', '');
        //     selAutorizante1.val('');
        //     selAutorizante2.data('id', '');
        //     selAutorizante2.val('');
        //     selAutorizante3.data('id', '');
        //     selAutorizante3.val('');
        //     DtGenerales.addClass('fade in active');
        //     Antecedentes.removeClass();
        //     Antecedentes.addClass('tab-pane')
        //     Soporte.removeClass();
        //     Soporte.addClass('tab-pane')
        //     otrasCondiciones.removeClass();
        //     otrasCondiciones.addClass('tab-pane')
        //     firmas.removeClass();
        //     firmas.addClass('tab-pane')
        //     tabDatosGenerales.parent().addClass('active')
        //     tabAntecedentes.parent().removeClass()
        //     tabSoporte.parent().removeClass()
        //     tabCondiciones.parent().removeClass()
        //     tabFirmas.parent().removeClass()
        //     tabNotificantes.parent().removeClass()

        //     lstDetalle = [{
        //         id: 0,
        //         no: '',
        //         descripcion: '',
        //         cantidad: '',
        //         unidad: '',
        //         PrecioUnitario: '',
        //         importe: '',
        //         nuevo: true
        //     }];

        //     lstDetalleMod = [{
        //         id: 0,
        //         no: '',
        //         descripcion: '',
        //         cantidad: '',
        //         unidad: '',
        //         PrecioUnitario: '',
        //         importe: '',
        //         nuevo: true
        //     }];
        //     lstDetalleReq = [{
        //         id: 0,
        //         no: '',
        //         descripcion: '',
        //         cantidad: '',
        //         unidad: '',
        //         PrecioUnitario: '',
        //         importe: '',
        //         nuevo: true
        //     }];
        //     lstDetalleAjust = [{
        //         id: 0,
        //         no: '',
        //         descripcion: '',
        //         cantidad: '',
        //         unidad: '',
        //         PrecioUnitario: '',
        //         importe: '',
        //         nuevo: true
        //     }];
        //     lstDetalleServ = [{
        //         id: 0,
        //         no: '',
        //         descripcion: '',
        //         cantidad: '',
        //         unidad: '',
        //         PrecioUnitario: '',
        //         importe: '',
        //         nuevo: true
        //     }];

        //     dtAlcances.clear();
        //     dtAlcances.rows.add(lstDetalle);
        //     dtAlcances.draw();

        //     dtModificaciones.clear();
        //     dtModificaciones.rows.add(lstDetalleMod);
        //     dtModificaciones.draw();

        //     dtAjusteDeVolumenes.clear();
        //     dtAjusteDeVolumenes.rows.add(lstDetalleAjust);
        //     dtAjusteDeVolumenes.draw()

        //     dtReqCampo.clear();
        //     dtReqCampo.rows.add(lstDetalleReq);
        //     dtReqCampo.draw()

        //     dtServiySum.clear();
        //     dtServiySum.rows.add(lstDetalleServ);
        //     dtServiySum.draw()
        //     // cboOrdenDeCambio.fillCombo('comboObtenerContratosyUltimasOrdenesDeCambio', null, false, null);
        // });
        // btnBuscar.click(function () {
        //     obtenerDatos();
        //     tieneNotificantes = false;
        // });


        // cboFiltroCC.fillCombo('getProyecto', null, false, null);


        // cboFiltroCC.change(function () {
        //     var table = tblOrdenesDeCambio.DataTable();
        //     if (cboFiltroCC.val() != '') {
        //         table.columns([0]).visible(false, false);
        //     } else {
        //         table.columns([0]).visible(true, true);
        //     }
        //     obtenerDatos();
        // });


        btnMas.click(function () {
            let lstDetalleAgregar = [{
                id: 0,
                no: '',
                descripcion: '',
                cantidad: '',
                unidad: '',
                PrecioUnitario: '',
                importe: '',
                nuevo: true
            }];
            let lstDetalle = [{
                id: 0,
                no: '',
                descripcion: '',
                cantidad: '',
                unidad: '',
                PrecioUnitario: '',
                importe: '',
                nuevo: true
            }];
            if (lstDetalle.length != 0) {
                // dtAlcances.clear();
                dtAlcances.rows.add(lstDetalleAgregar);
                dtAlcances.draw();
            } else {
                dtAlcances.clear();
                dtAlcances.rows.add(lstDetalle);
                dtAlcances.draw();
            }
        });

        btnMasModificaciones.click(function () {
            let lstDetalleMod = [{
                id: 0,
                no: '',
                descripcion: '',
                cantidad: '',
                unidad: '',
                PrecioUnitario: '',
                importe: '',
                nuevo: true
            }];
            let lstDetalleAgregarMod = [{
                id: 0,
                no: '',
                descripcion: '',
                cantidad: '',
                unidad: '',
                PrecioUnitario: '',
                importe: '',
                nuevo: true
            }];
            if (lstDetalleMod.length != 0) {
                dtModificaciones.rows.add(lstDetalleAgregarMod);
                dtModificaciones.draw();
            } else {
                dtModificaciones.rows.add(lstDetalleMod);
                dtModificaciones.draw();
            }
        });
        btnMasReqCampo.click(function () {
            let lstDetalleReq = [{
                id: 0,
                no: '',
                descripcion: '',
                cantidad: '',
                unidad: '',
                PrecioUnitario: '',
                importe: '',
                nuevo: true
            }];
            let lstDetalleAgregarReq = [{
                id: 0,
                no: '',
                descripcion: '',
                cantidad: '',
                unidad: '',
                PrecioUnitario: '',
                importe: '',
                nuevo: true
            }];
            if (lstDetalleReq.length != 0) {
                dtReqCampo.rows.add(lstDetalleAgregarReq);
                dtReqCampo.draw();
            } else {
                dtReqCampo.rows.add(lstDetalleReq);
                dtReqCampo.draw();
            }
        });
        btnMasAjusteDeVolumenes.click(function () {
            let lstDetalleAjust = [{
                id: 0,
                no: '',
                descripcion: '',
                cantidad: '',
                unidad: '',
                PrecioUnitario: '',
                importe: '',
                nuevo: true
            }];
            let lstDetalleAgregarAjust = [{
                id: 0,
                no: '',
                descripcion: '',
                cantidad: '',
                unidad: '',
                PrecioUnitario: '',
                importe: '',
                nuevo: true
            }];
            if (lstDetalleAjust.length != 0) {
                dtAjusteDeVolumenes.rows.add(lstDetalleAgregarAjust);
                dtAjusteDeVolumenes.draw();
            } else {
                dtAjusteDeVolumenes.rows.add(lstDetalleAjust);
                dtAjusteDeVolumenes.draw();
            }
        });
        btnMasServiySum.click(function () {
            let lstDetalleServ = [{
                id: 0,
                no: '',
                descripcion: '',
                cantidad: '',
                unidad: '',
                PrecioUnitario: '',
                importe: '',
                nuevo: true
            }];
            let lstDetalleAgregarServ = [{
                id: 0,
                no: '',
                descripcion: '',
                cantidad: '',
                unidad: '',
                PrecioUnitario: '',
                importe: '',
                nuevo: true
            }];
            if (lstDetalleServ.length != 0) {
                dtServiySum.rows.add(lstDetalleAgregarServ);
                dtServiySum.draw();
            } else {
                dtServiySum.rows.add(lstDetalleServ);
                dtServiySum.draw();
            }
        });
        btnGestionar.click(function () {
            GuardarEditarOrdenDeCambio(btnGestionar.attr('data-editar'));
        });
        btnImprimir.click(function () {
            obtenerReporte(inpOrden.val());
        });

        btnlimpiar.click(function () {
            Swal.fire({
                title: '¡Alerta!',
                text: "Desea eliminar este soporte?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, delete it!'
            }).then((result) => {
                if (result.isConfirmed) {

                    switch (btnlimpiar.attr('data-idLimpiar')) {
                        case "1":
                            let file1 = document.getElementById('imgFirma1');
                            file1['src'] = 'http://localhost:3676/ControlObra/ControlObra/Index';
                            break;
                        case "2":
                            let file2 = document.getElementById('imgFirma2');
                            file2['src'] = 'http://localhost:3676/ControlObra/ControlObra/Index';
                            break;
                        case "3":
                            let file3 = document.getElementById('imgFirma3');
                            file3['src'] = 'http://localhost:3676/ControlObra/ControlObra/Index';
                            break;
                        case "4":
                            let file4 = document.getElementById('imgFirma4');
                            file4['src'] = 'http://localhost:3676/ControlObra/ControlObra/Index';
                            break;
                        case "5":
                            let file5 = document.getElementById('imgFirma5');
                            file5['src'] = 'http://localhost:3676/ControlObra/ControlObra/Index';
                            break;

                    }
                }
            })

        })

        function MASK(form, n, mask, format) {
            if (format == "undefined") format = false;
            if (format || NUM(n)) {
                dec = 0, point = 0;
                x = mask.indexOf(".") + 1;
                if (x) { dec = mask.length - x; }

                if (dec) {
                    n = NUM(n, dec) + "";
                    x = n.indexOf(".") + 1;
                    if (x) { point = n.length - x; } else { n += "."; }
                } else {
                    n = NUM(n, 0) + "";
                }
                for (var x = point; x < dec; x++) {
                    n += "0";
                }
                x = n.length, y = mask.length, XMASK = "";
                while (x || y) {
                    if (x) {
                        while (y && "#0.".indexOf(mask.charAt(y - 1)) == -1) {
                            if (n.charAt(x - 1) != "-")
                                XMASK = mask.charAt(y - 1) + XMASK;
                            y--;
                        }
                        XMASK = n.charAt(x - 1) + XMASK, x--;
                    } else if (y && "$0".indexOf(mask.charAt(y - 1)) + 1) {
                        XMASK = mask.charAt(y - 1) + XMASK;
                    }
                    if (y) { y-- }
                }
            } else {
                XMASK = "";
            }
            if (form) {
                form.value = XMASK;
                if (NUM(n) < 0) {
                    form.style.color = "#FF0000";
                } else {
                    form.style.color = "#000000";
                }
            }
            return XMASK;
        }
        function NUM(s, dec) {
            for (var s = s + "", num = "", x = 0; x < s.length; x++) {
                c = s.charAt(x);
                if (".-+/*".indexOf(c) + 1 || c != " " && !isNaN(c)) { num += c; }
            }
            if (isNaN(num)) { num = eval(num); }
            if (num == "") { num = 0; } else { num = parseFloat(num); }
            if (dec != undefined) {
                r = .5; if (num < 0) r = -r;
                e = Math.pow(10, (dec > 0) ? dec : 0);
                return parseInt(num * e + r) / e;
            } else {
                return num;
            }
        }

        Firma1.click(function () {
            btnlimpiar.attr('data-idLimpiar', 1)
            let file1 = document.getElementById('imgFirma1');
            if (file1['src'] == 'http://localhost:3676/ControlObra/ControlObra/Index') {
                fncClick(1);
            } else {
                imgFirma1.css('display', 'block');
                imgFirma2.css('display', 'none');
                imgFirma3.css('display', 'none');
                imgFirma4.css('display', 'none');
                imgFirma5.css('display', 'none');

            }
        });
        Firma2.click(function () {
            btnlimpiar.attr('data-idLimpiar', 2)
            let file2 = document.getElementById('imgFirma2');
            if (file2['src'] == 'http://localhost:3676/ControlObra/ControlObra/Index') {
                fncClick(2);
            } else {
                imgFirma1.css('display', 'none');
                imgFirma2.css('display', 'block');
                imgFirma3.css('display', 'none');
                imgFirma4.css('display', 'none');
                imgFirma5.css('display', 'none');

            }
        });
        Firma3.click(function () {
            btnlimpiar.attr('data-idLimpiar', 3)
            let file3 = document.getElementById('imgFirma3');
            if (file3['src'] == 'http://localhost:3676/ControlObra/ControlObra/Index') {
                fncClick(3);
            } else {
                imgFirma1.css('display', 'none');
                imgFirma2.css('display', 'none');
                imgFirma3.css('display', 'block');
                imgFirma4.css('display', 'none');
                imgFirma5.css('display', 'none');

            }
        });
        Firma4.click(function () {
            btnlimpiar.attr('data-idLimpiar', 4)
            let file4 = document.getElementById('imgFirma4');
            if (file4['src'] == 'http://localhost:3676/ControlObra/ControlObra/Index') {
                fncClick(4);
            } else {
                imgFirma1.css('display', 'none');
                imgFirma2.css('display', 'none');
                imgFirma3.css('display', 'none');
                imgFirma4.css('display', 'block');
                imgFirma5.css('display', 'none');

            }
        });
        Firma5.click(function () {
            btnlimpiar.attr('data-idLimpiar', 5)
            let file5 = document.getElementById('imgFirma5');
            if (file5['src'] == 'http://localhost:3676/ControlObra/ControlObra/Index') {
                fncClick(5);
            } else {
                imgFirma1.css('display', 'none');
                imgFirma2.css('display', 'none');
                imgFirma3.css('display', 'none');
                imgFirma4.css('display', 'none');
                imgFirma5.css('display', 'block');

            }
        });

        btnCancelarFirma.click(function () {
            divFirmaFull.hide();
        });
        btnGuardarFirma.click(function () {
            let dataURL = signaturePadFull.toDataURL('image/png');
            signaturePadFull.clear();
            divFirmaFull.hide();
            switch (btnGuardarFirma.attr('data-firma')) {
                case "1":
                    imgFirma1.attr("src", dataURL)
                    imgFirma1.css('display', 'block')
                    imgFirma2.css('display', 'none')
                    imgFirma3.css('display', 'none')
                    imgFirma4.css('display', 'none')
                    imgFirma5.css('display', 'none')
                    break;
                case "2":
                    imgFirma2.attr("src", dataURL)
                    imgFirma2.css('display', 'block')
                    imgFirma1.css('display', 'none')
                    imgFirma3.css('display', 'none')
                    imgFirma4.css('display', 'none')
                    imgFirma5.css('display', 'none')
                    break;

                case "3":
                    imgFirma3.attr("src", dataURL)
                    imgFirma3.css('display', 'block')
                    imgFirma2.css('display', 'none')
                    imgFirma1.css('display', 'none')
                    imgFirma4.css('display', 'none')
                    imgFirma5.css('display', 'none')
                    break;

                case "4":
                    imgFirma4.attr("src", dataURL)
                    imgFirma4.css('display', 'block')
                    imgFirma2.css('display', 'none')
                    imgFirma3.css('display', 'none')
                    imgFirma1.css('display', 'none')
                    imgFirma5.css('display', 'none')
                    break;

                case "5":
                    imgFirma5.attr("src", dataURL)
                    imgFirma5.css('display', 'block')
                    imgFirma2.css('display', 'none')
                    imgFirma3.css('display', 'none')
                    imgFirma4.css('display', 'none')
                    imgFirma1.css('display', 'none')
                    break;

            }
        });
        btnGuardarFirmas.click(function () {
            fncGuardarFirmas();
        });
        inpTiempoFinal.change(function () {
            getDaysLeft(inpTiempoFinal.val(), inpTiempo.val());

        })
        obtenerFuncion();
    }

    function fncClick(botonFirma) {
        btnGuardarFirma.attr('data-Firma', botonFirma)
        divFirmaFull.show();
        _cFirmaFull.width = divFirmaFull.width();
        _cFirmaFull.height = document.body.clientHeight;

        signaturePadFull.clear();
    }
    function limpiarCampos(limpiarcc, limpiarcontrato) {
        $('#inpCobrable').bootstrapToggle('off')
        inpFechaInicioContrato.val('');
        inpFechaFinalContrato.val('');
        if (limpiarcc) {
            cboFiltroCC.val('');
            cboFiltroCC.trigger("change");
        }
        // cboOrdenDeCambio.prop('disabled', false);
        if (limpiarcontrato) {
            inpIdContratista.val('');
            cboOrdenDeCambio.val('');
            cboOrdenDeCambio.trigger('change');
        }
        inpProyecto.val('');
        inpOrden.val('');
        inpCliente.val('');
        inpCobrable.val('');
        // inpUbicacion.val('');
        inpMunicipio.val('');
        inpEstado.val('');
        inpNoContrato.val('');
        inpContratista.val('');
        inpcc.val('');
        inpDireccion.val('');
        inpRespresentanteLegal.val('');
        inpAlcances.val('');
        inpModificaciones.val('');
        inpReqCampo.val('');
        inpAjusteVol.val('');
        inpServiySum.val('');
        inpTiempo.val('');
        inpTiempoFinal.val('');
        txtAntecedentes.val('');
        inpTiempo.val('');
        inpTiempoFinal.val('');
        inpMontocontractual.val('');
        inpMontoTotalOrdenPrevia.val('');
        inpTotalMontoActual.val('');
        inpSumaTotalModificaciones.val('');
        inpTotalMontoOrdenCambioActual.val('');

        inpAlcancesDescripcion.val('')
        inpReqCampoDescripcion.val('')
        inpModificacionesDescripcion.val('')
        inpAjusteVolDescripcion.val('')
        inpServiySumDescripcion.val('')
        inpNombreDelArchivo.val('')

        inpDirectorDivision.val('');
        inpDirectorTecnico.val('');
        inpGerenteArea.val('');
        inpGerentePMO.val('');
        inpGerenteProyecto.val('');

        inpIdGerenteProyecto.val('');
        inpIdDirectorDivision.val('');
        inpIdDirectorTecnico.val('');
        inpIdGerentePMO.val('');
        inpIdGerenteArea.val('');

        inpTiempo.val('');
        inpTiempoFinal.val('');
        inpFechaInicioContrato.val('');
        inpFechaFinalContrato.val('');
        inpDias.val('');
        inpFechaFinalPrevias.val('');
        inpDiasPrevias.val('');
        inpFechaAmpliacion.val('');
        inpDiasAmpliacion.val('');
        limpiarSoportes();
        $("#porcentaje1").val('');
        $("#porcentaje2").val('');
    }
    function obtenerDatos() {
        let cc = cboFiltroCC.val();
        axios.post('obtenerOrdenesDeCambio', { cc: cc })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    AddRows(tblOrdenesDeCambio, items);
                }
            });
    }

    function obtenerReporte(idOrdenDeCambio) {
        report.attr(`src`, `/Reportes/Vista.aspx?idReporte=242&idOrdenDeCambio=${idOrdenDeCambio}`);
        document.getElementById('report').onload = function () {
            $.unblockUI();
            openCRModal();
        }
    }

    function enviarCorre(idOrdenDeCambio, tipoMail) {
        axios.post('EnviarCorreo', { idOrdenDeCambio: idOrdenDeCambio, tipoCorreo: tipoMail })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    if (tipoMail == 2) {
                        // report.attr(`src`, `/Reportes/Vista.aspx?idReporte=242&idOrdenDeCambio=${idOrdenDeCambio}&isCRModal=${false}&inMemory=1`); 
                        // document.getElementById('report').onload = function () {
                        // $.unblockUI();
                        // openCRModal();
                        // }                        
                    }
                }
            });
    }
    function AprobarOrdenC(id) {
        axios.post('AutorizarOrdenDeCambio', { id: id })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    if (items.success) {
                        if (items.items == "Autorizado Vobo 1 con exito") {
                            alert2Exito(items.items)
                            obtenerDatos();
                        } else {
                            alert2Exito(items.items)
                            enviarCorre(id, 2);
                            obtenerDatos();
                        }
                    } else {
                        AlertaGeneral('Error!', items.items)
                    }
                }
            });
    }
    function RechazarOrdenC(id) {
        axios.post('RechazarOrdenDeCambio', { id: id })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    if (items.success) {
                        alert2Exito(items.items)
                        obtenerDatos();
                    } else {
                        AlertaGeneral('Error!', items.items)
                    }
                }
            });
    }

    function obtenerReporte(idOrdenDeCambio) {
        report.attr(`src`, `/Reportes/Vista.aspx?idReporte=242&idOrdenDeCambio=${idOrdenDeCambio}&isCRModal=${true}`);
        document.getElementById('report').onload = function () {
            $.unblockUI();
            openCRModal();
        }
    }
    // function llenarCamposVaciosEditar(id, tabla, idcbo) {
    //     let parametros = {
    //         id: id,
    //         tabla: tabla,
    //     }
    //     axios.post('obtenerCamposDeOrdenDeCambio', { parametros: parametros })
    //         .catch(o_O => AlertaGeneral(o_O.message))
    //         .then(response => {
    //             let { success, items } = response.data;
    //             if (success) {
    //                 cboOrdenDeCambio.prop('disabled', true);
    //                 cboOrdenDeCambio.val(idcbo);
    //                 cboOrdenDeCambio.trigger('change');
    //                 inpProyecto.val(items.items.Proyecto);
    //                 inpOrden.val(items.items.NoOrden);
    //                 inpCliente.val(items.items.CLiente);
    //                 inpCobrable.prop('checked', items.items.esCobrable);
    //                 if (items.items.esCobrable == true) {
    //                     $('#inpCobrable').bootstrapToggle('on')
    //                 } else {
    //                     $('#inpCobrable').bootstrapToggle('off')
    //                 }
    //                 inpUbicacion.val(items.items.ubicacionProyecto);
    //                 inpNoContrato.val(items.items.numeroDeContrato);
    //                 inpContratista.val(items.items.Contratista);
    //                 inpContratista.attr('data-idSubcontratista', items.items.idSubContratista)
    //                 inpcc.val(items.items.cc);
    //                 inpDireccion.val(items.items.Direccion);
    //                 dtFechaEfectiva.val(moment(items.items.fechaEfectiva).format("DD/MM/YYYY"));
    //                 txtAntecedentes.val(items.items.Antecedentes)
    //                 txtCondicions.val(items.items.otrasCondicioes)

    //                 inpFechaFinalContrato.val(moment(items.items.fechaExpiracion).format("DD/MM/YYYY"));
    //                 inpFechaInicioContrato.val(moment(items.items.fechaSuscripcion).format("DD/MM/YYYY"));

    //                 inpNombreDelArchivo.val(items.items.nombreDelArchivo);

    //                 if (items.items.lstSoportesEvidencia != null) {
    //                     inpOriginal.val(items.items.lstSoportesEvidencia.MontoContratoOriginal);
    //                     inpOrdenDeCambioSum.val(items.items.lstSoportesEvidencia.MontoContratoOriginalSuma);
    //                     inpAlcances.val(items.items.lstSoportesEvidencia.alcancesNuevos);
    //                     inpModificaciones.val(items.items.lstSoportesEvidencia.modificacionesPorCambio);
    //                     inpReqCampo.val(items.items.lstSoportesEvidencia.requerimientosDeCampo);
    //                     inpAjusteVol.val(items.items.lstSoportesEvidencia.ajusteDeVolumenes);
    //                     inpServiySum.val(items.items.lstSoportesEvidencia.serviciosYSuministros);
    //                     inpTiempo.val(moment(items.items.fechaExpiracion).format("DD/MM/YYYY"));
    //                     inpTiempoFinal.val(moment(items.items.lstSoportesEvidencia.FechaFinal).format("DD/MM/YYYY"));
    //                     getDaysLeft(inpTiempoFinal.val(), inpTiempo.val());

    //                     inpAlcancesDescripcion.val(items.items.lstSoportesEvidencia.alcancesNuevosDescripcion);
    //                     inpModificacionesDescripcion.val(items.items.lstSoportesEvidencia.modificacionesPorCambioDescripcion);
    //                     inpReqCampoDescripcion.val(items.items.lstSoportesEvidencia.requerimientosDeCampoDescripcion);
    //                     inpAjusteVolDescripcion.val(items.items.lstSoportesEvidencia.ajusteDeVolumenesDescripcion);
    //                     inpServiySumDescripcion.val(items.items.lstSoportesEvidencia.serviciosYSuministrosDescripcion);
    //                     inpfechaDescripcion.val(items.items.lstSoportesEvidencia.fechaDescripcion);

    //                     items.items.lstSoportesEvidencia.AlcancesNuevosArchivos != "" ? visualizarArchivoGuardado(fileAlcancesNuevos, items.items.lstSoportesEvidencia.AlcancesNuevosArchivos) : visualizarArchivoNoCargado(fileAlcancesNuevos);
    //                     items.items.lstSoportesEvidencia.modificacionArchvios != "" ? visualizarArchivoGuardado(fileModificaciones, items.items.lstSoportesEvidencia.modificacionArchvios) : visualizarArchivoNoCargado(fileModificaciones);
    //                     items.items.lstSoportesEvidencia.requerimientosArchivos != "" ? visualizarArchivoGuardado(fileRequerimientos, items.items.lstSoportesEvidencia.requerimientosArchivos) : visualizarArchivoNoCargado(fileRequerimientos);
    //                     items.items.lstSoportesEvidencia.ajusteDeVolumenesArchivos != "" ? visualizarArchivoGuardado(fileAjusteDeVolumenes, items.items.lstSoportesEvidencia.ajusteDeVolumenesArchivos) : visualizarArchivoNoCargado(fileAjusteDeVolumenes);
    //                     items.items.lstSoportesEvidencia.serviciosYSuministrosArchivos != "" ? visualizarArchivoGuardado(fileServiYSuministros, items.items.lstSoportesEvidencia.serviciosYSuministrosArchivos) : visualizarArchivoNoCargado(fileServiYSuministros);

    //                 }
    //                 if (items.items.lstFirmas != null) {
    //                     Firma1.attr('data-id', items.items.lstFirmas[0].id);
    //                     let file1 = document.getElementById('imgFirma1');
    //                     file1['src'] = items.items.lstFirmas[0].firma;
    //                     imgFirma1.css('display', 'block');
    //                     for (let i = 0; i < items.items.lstFirmas.length; i++) {
    //                         if (i == 0) {
    //                             Firma1.attr('data-id', items.items.lstFirmas[i].id);
    //                             let file1 = document.getElementById('imgFirma1');
    //                             file1['src'] = items.items.lstFirmas[i].firma;
    //                         } else if (i == 1) {
    //                             Firma2.attr('data-id', items.items.lstFirmas[i].id);
    //                             let file2 = document.getElementById('imgFirma2');
    //                             file2['src'] = items.items.lstFirmas[i].firma;
    //                         } else if (i == 2) {
    //                             Firma3.attr('data-id', items.items.lstFirmas[i].id);
    //                             let file3 = document.getElementById('imgFirma3');
    //                             file3['src'] = items.items.lstFirmas[i].firma;
    //                         } else if (i == 3) {
    //                             Firma4.attr('data-id', items.items.lstFirmas[i].id);
    //                             let file4 = document.getElementById('imgFirma4');
    //                             file4['src'] = items.items.lstFirmas[i].firma;
    //                         } else if (i == 4) {
    //                             Firma5.attr('data-id', items.items.lstFirmas[i].id);
    //                             let file5 = document.getElementById('imgFirma5');
    //                             file5['src'] = items.items.lstFirmas[i].firma;
    //                         }
    //                     }
    //                 } else {
    //                     Firma1.attr('data-id', 0);
    //                     let file1 = document.getElementById('imgFirma1');
    //                     file1['src'] = 'http://localhost:3676/ControlObra/ControlObra/Index';
    //                     Firma2.attr('data-id', 0);
    //                     let file2 = document.getElementById('imgFirma2');
    //                     file2['src'] = 'http://localhost:3676/ControlObra/ControlObra/Index';
    //                     Firma3.attr('data-id', 0);
    //                     let file3 = document.getElementById('imgFirma3');
    //                     file3['src'] = 'http://localhost:3676/ControlObra/ControlObra/Index';
    //                     Firma4.attr('data-id', 0);
    //                     let file4 = document.getElementById('imgFirma4');
    //                     file4['src'] = 'http://localhost:3676/ControlObra/ControlObra/Index';
    //                     Firma5.attr('data-id', 0);
    //                     let file5 = document.getElementById('imgFirma5');
    //                     file5['src'] = 'http://localhost:3676/ControlObra/ControlObra/Index';
    //                 }


    //                 // AddRows(tblAlcances,);
    //             }
    //         });
    // }

    function llenarCamposVacios(id, tabla) {
        axios.post('obtenerCamposDeOrdenDeCambio', { parametros: { id: id, tabla: tabla, } }).catch(o_O => AlertaGeneral(o_O.message)).then(response => {
            let { success, items } = response.data;

            if (success) {
                if (items.items != null) {
                    // btnEditar.attr('data-id', items.items.id);
                    inpContratista.val(items.items.subcontratistaNombre);
                    inpIdContratista.val(items.items.idSubContratista);
                    inpProyecto.val(items.items.Proyecto);
                    inpCliente.val(items.items.CLiente);
                    inpEstado.val(items.items.estado);
                    inpMunicipio.val(items.items.municipio);
                    inpDireccion.val(items.items.Direccion);
                    inpCobrable.prop('checked', items.items.esCobrable);
                    inpUbicacion.val(items.items.ubicacionProyecto);
                    inpNoContrato.val(items.items.numeroDeContrato);
                    inpcc.val(items.items.cc);
                    inpOrden.val(items.items.NoOrden);
                    dtFechaEfectiva.val(moment(items.items.fechaEfectiva).format("DD/MM/YYYY"));
                    txtAntecedentes.val(items.items.Antecedentes);

                    //inpSumaTotalModificaciones.val("$" + "0");
                    inpMontocontractual.val("$" + items.items.montoContractual.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                    let valor = inpMontocontractual.val();
                    inpMontocontractual.val(valor);

                    for (const item of items.items.listaAutorizantes) {
                        switch (item.puesto) {
                            case "REPRESENTANTE LEGAL SUB":
                                // inpIdDirectorTecnico.val(item.id)
                                // inpDirectorTecnico.val(item.nombre)
                                inpRespresentanteLegal.val(item.nombre)
                                break;
                            case "Gerente de Proyecto":
                                inpIdGerenteProyecto.val(item.id)
                                inpGerenteProyecto.val(item.nombre)
                                break;
                            case "Gerente de Área":
                                inpIdGerenteArea.val(item.id)
                                inpGerenteArea.val(item.nombre)
                                break;
                            case "Director de División":
                                inpIdDirectorDivision.val(item.id)
                                inpDirectorDivision.val(item.nombre)
                                break;
                            case "Gerente de Administración de Proyectos":
                                inpIdGerentePMO.val(item.id)
                                inpGerentePMO.val(item.nombre)
                                break;
                            case "Director General":
                                inpIdDirectorTecnico.val(item.id)
                                inpDirectorTecnico.val(item.nombre)
                                break;
                            default:
                                break;
                        }
                    }

                    if (items.items.lstSoportesEvidencia != null) {
                        inpMontoTotalOrdenPrevia.val("$" + items.items.lstSoportesEvidencia.MontoContratoOriginalSuma.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        let valor = inpMontoTotalOrdenPrevia.val()
                        inpMontoTotalOrdenPrevia.val(valor)

                        let valorInputMontoActual = Number(inpMontocontractual.val().replace(/[^0-9.-]+/g, "")) + Number(inpMontoTotalOrdenPrevia.val().replace(/[^0-9.-]+/g, ""))
                        inpTotalMontoActual.val("$" + valorInputMontoActual.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        let valor2 = items.items.montoContractual + items.items.lstSoportesEvidencia.MontoContratoOriginalSuma
                        inpTotalMontoActual.val("$" + valor2.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"))

                        let montoOriginal = Number(inpMontocontractual.val().replace(/[^0-9.-]+/g, ""));
                        let porcentajeIncrementoContractual = (Number(valor2) / (montoOriginal == 0 ? 1 : montoOriginal)) * 100;

                        $("#porcentaje1").val(porcentajeIncrementoContractual.toFixed(4));

                        //inpAlcances.val("$0.00");
                        //inpModificaciones.val("$0.00");
                        //inpReqCampo.val("$0.00");
                        //inpAjusteVol.val("$0.00");
                        //inpServiySum.val("$0.00");

                        inpTiempo.val(moment(items.items.lstSoportesEvidencia.fechaInicial).format("DD/MM/YYYY"));
                        inpTiempoFinal.val(moment(items.items.lstSoportesEvidencia.FechaFinal).format("DD/MM/YYYY"));
                    }

                    var click = false;
                    AddRows(tblAlcances, items.items.lstMontos.filter(x => x.tipoSoportes == 1));
                    if (items.items.lstMontos.filter(x => x.tipoSoportes == 1).length > 0) {
                        $('#checkAlcance').prop('checked', true);
                        click = true;
                    }
                    AddRows(tblModificaciones, items.items.lstMontos.filter(x => x.tipoSoportes == 2));
                    if (items.items.lstMontos.filter(x => x.tipoSoportes == 2).length > 0) {
                        $('#checkCambioIngenieria').prop('checked', true);
                        click = true;
                    }
                    AddRows(tblReqCampo, items.items.lstMontos.filter(x => x.tipoSoportes == 3));
                    if (items.items.lstMontos.filter(x => x.tipoSoportes == 3).length > 0) {
                        $('#checkRequerimientosCampo').prop('checked', true);
                        click = true;
                    }
                    AddRows(tblAjusteDeVolumenes, items.items.lstMontos.filter(x => x.tipoSoportes == 4));
                    if (items.items.lstMontos.filter(x => x.tipoSoportes == 4).length > 0) {
                        $('#checkAjustesVol').prop('checked', true);
                        click = true;
                    }
                    AddRows(tblServiySum, items.items.lstMontos.filter(x => x.tipoSoportes == 5));
                    if (items.items.lstMontos.filter(x => x.tipoSoportes == 5).length > 0) {
                        $('#checkServiciosSum').prop('checked', true);
                        click = true;
                    }

                    showContent();

                    totalizadorAlcances(1);
                    totalizadorAlcances(2);
                    totalizadorAlcances(3);
                    totalizadorAlcances(4);
                    totalizadorAlcances(5);

                    tblAlcances.find('tbody tr').toArray().forEach(row => {
                        $(row).find('.inputPrecioInsumo').val($(row).find('.inputPrecioInsumo').val());
                        $(row).find('.importe').val(maskNumero($(row).find('.importe').val()));
                    });

                    tblModificaciones.find('tbody tr').toArray().forEach(row => {
                        $(row).find('.inputPrecioInsumo').val($(row).find('.inputPrecioInsumo').val());
                        $(row).find('.importe').val(maskNumero($(row).find('.importe').val()));
                    });

                    tblReqCampo.find('tbody tr').toArray().forEach(row => {
                        $(row).find('.inputPrecioInsumo').val($(row).find('.inputPrecioInsumo').val());
                        $(row).find('.importe').val(maskNumero($(row).find('.importe').val()));
                    });

                    tblAjusteDeVolumenes.find('tbody tr').toArray().forEach(row => {
                        $(row).find('.inputPrecioInsumo').val($(row).find('.inputPrecioInsumo').val());
                        $(row).find('.importe').val(maskNumero($(row).find('.importe').val()));
                    });

                    tblServiySum.find('tbody tr').toArray().forEach(row => {
                        $(row).find('.inputPrecioInsumo').val($(row).find('.inputPrecioInsumo').val());
                        $(row).find('.importe').val(maskNumero($(row).find('.importe').val()));
                    });

                    if (items.items.fechaInicial == null || items.items.fechaFinal == null) {
                        inpFechaInicioContrato.val('');
                        inpFechaFinalContrato.val('');
                    } else {
                        inpFechaInicioContrato.val(moment(items.items.fechaInicial).format("YYYY-MM-DD"));
                        inpFechaFinalContrato.val(moment(items.items.fechaFinal).format("YYYY-MM-DD"));
                        var fechaI = new Date(inpFechaInicioContrato.val());
                        var fechaF = new Date(inpFechaFinalContrato.val());
                        var difM = fechaF - fechaI // diferencia en milisegundos  
                        var difD = difM / (1000 * 60 * 60 * 24) // diferencia en dias
                        inpDias.val(difD);
                    }

                    if (items.items.fechaAmpliacionAcumulada == null) {
                        inpFechaFinalPrevias.val('');
                        inpDiasPrevias.val(0);
                    } else {
                        inpFechaFinalPrevias.val(moment(items.items.fechaAmpliacionAcumulada).format("YYYY-MM-DD"));
                        var fechaIPrev = new Date(inpFechaInicioContrato.val());
                        var fechaPrevia = new Date(inpFechaFinalPrevias.val());
                        var difMPrevia = fechaPrevia - fechaIPrev
                        var difPrev = difMPrevia / (1000 * 60 * 60 * 24)
                        inpDiasPrevias.val(difPrev);
                    }
                }
            }
        });
    }

    function initDatatblAlcances() {
        dtAlcances = tblAlcances.DataTable({
            destroy: true
            , paging: false
            , ordering: false
            , searching: false
            , bFilter: false
            , info: false
            , language: dtDicEsp
            , columns: [
                {
                    data: 'no', title: 'No.', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class='form-control inputNo inputNumeroInsumo'></input>`;
                        } else {
                            html = `<input class='form-control inputNo inputNumeroInsumo' data-id='${row.id}' value='${data}'></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'descripcion', title: 'descripcion', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class="form-control inputDescripcionInsumo" >`;
                        } else {
                            html = `<input class='form-control inputDescripcionInsumo'  value='${data}'></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'unidad', title: 'Unidad', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `
                            <select class="form-control motivoRehabilitacion">
                                <option value='Metros'>Metros</option>
                                <option value='KM'>KM</option>
                                <option value='Lote'>Lote</option>
                                <option value='Persona'>Persona</option>
                                <option value='M2'>M2</option>
                                <option value='M3'>M3</option>
                                <option value='M3/KM'>M3/KM</option>
                            </select>
                            `;
                        } else {
                            if (data == 'Metros') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'KM') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='KM'>KM</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'Lote') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='Lote'>Lote</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'Persona') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='Persona'>Persona</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'M2') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='M2'>M2</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'M3') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='M3'>M3</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'M3/KM') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='M3/KM'>M3/KM</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                </select>
                                `;
                            }
                        }
                        return html;
                    }
                },
                {
                    data: 'cantidad', title: 'Cantidad', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class='form-control cantidad' type='number'></input>`;
                        } else {
                            html = `<input class='form-control cantidad' type='number' value='${data}'></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'PrecioUnitario', title: 'Precio Unitario.', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class='form-control inputNo inputPrecioInsumo'></input>`;
                        } else {
                            html = `<input class='form-control inputNo inputPrecioInsumo' value='${data}'></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'importe', title: 'Importe', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class='form-control importe' disabled></input>`;
                        } else {
                            html = `<input class='form-control importe' value='${data}' disabled></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'id', title: 'Opciones', render: (data, type, row, meta) => {
                        let html = `<button type="" class="btn btn-danger btnMenos" style="width: 24%;margin-left: 68px;margin-right: -51px;height: auto;padding: 6px;"><span class="glyphicon glyphicon-trash"></span></button>`;
                        return html;
                    }
                },
            ]
            , initComplete: function (settings, json) {
                tblAlcances.on("click", ".btnMenos", function () {
                    let rowData = dtAlcances.row($(this).closest("tr")).data();
                    dtAlcances.row(rowData).remove().draw();
                    totalizadorAlcances(1);
                });
            },
            createdRow: function (row, rowData) {
                $(row).find('.inputNumeroInsumo').getAutocompleteValid(
                    (e, ui) => {
                        $(row).find('.inputNumeroInsumo').val(ui.item.id);
                        $(row).find('.inputDescripcionInsumo').val(ui.item.value);
                        $(row).find('.inputPrecioInsumo').val(ui.item.precio);
                        $(row).find('.cantidad').change();
                        e.preventDefault();
                    },
                    (e, ui) => {
                        if (ui.item == null) {
                            $(row).find('.inputNumeroInsumo').val('');
                        }
                    },
                    {
                        busquedaPorNumero: true,
                        cc: cboFiltroCC.val()
                    },
                    'GetInsumosSISUNAutocomplete'
                );
                $(row).find('.inputDescripcionInsumo').getAutocompleteValid(
                    (e, ui) => {
                        $(row).find('.inputNumeroInsumo').val(ui.item.id);
                        $(row).find('.inputDescripcionInsumo').val(ui.item.value);
                        $(row).find('.inputPrecioInsumo').val(ui.item.precio);
                        $(row).find('.cantidad').change();
                    },
                    (e, ui) => {
                        if (ui.item == null) {
                            $(row).find('.inputNumeroInsumo').val('');
                        }
                    },
                    {
                        busquedaPorNumero: false,
                        cc: cboFiltroCC.val()
                    },
                    'GetInsumosSISUNAutocomplete'
                );
            },
            drawCallback: function (settings) {
                tblAlcances.find('input.cantidad, input.inputPrecioInsumo').on('change', function (e) {
                    let numeroFila = ($(this).closest("tr")[0].rowIndex) - 1;
                    let row = $(this).closest('tr');
                    let cantidad = +$(row).find('.cantidad').val();
                    let precio = +$(row).find('.inputPrecioInsumo').val();
                    let importe = cantidad * precio;

                    dtAlcances.row(numeroFila).data().cantidad = cantidad;
                    dtAlcances.row(numeroFila).data().inputNo = precio;
                    dtAlcances.row(numeroFila).data().importe = importe;

                    $(row).find('.importe').val(maskNumero(importe));
                    totalizadorAlcances(1);
                });
            }
        });
    }
    function totalizadorAlcances(tipo) {
        let montoOriginal = Number(inpMontocontractual.val().replace(/[^0-9.-]+/g, ""));

        porcentajeIncrementoFinal = 0;
        switch (tipo) {
            case 1:
                let tr = tblAlcances.find('tr');
                let con = 0;
                for (let b = 1; b < tr.length; b++) {
                    let td = $(tr[b]).find('input')
                    if ($(td[4]).val()) {
                        con += parseFloat($(td[4]).val().replace(/[^0-9.-]+/g, ""));
                    } else {
                        con += 0;
                    }
                }

                if (con > 0) {
                    inpAlcances.val(maskNumero(con ?? 0));
                } else {
                    inpAlcances.val(maskNumero(0));
                }

                let valor1 = inpAlcances.val()
                if (valor1 != "") {
                    let valor2 = valor1
                    inpAlcances.val((valor2))
                }

                //
                let valorInputSumaTotalModificaciones01 = Number(inpAlcances.val().replace(/[^0-9.-]+/g, "")) + Number(inpModificaciones.val().replace(/[^0-9.-]+/g, "")) + Number(inpReqCampo.val().replace(/[^0-9.-]+/g, "")) + Number(inpAjusteVol.val().replace(/[^0-9.-]+/g, "")) + Number(inpServiySum.val().replace(/[^0-9.-]+/g, ""));
                inpSumaTotalModificaciones.val("$" + valorInputSumaTotalModificaciones01.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                valorInputOrdenCambioActual = Number(inpTotalMontoActual.val().replace(/[^0-9.-]+/g, "")) + Number(inpSumaTotalModificaciones.val().replace(/[^0-9.-]+/g, ""));
                inpTotalMontoOrdenCambioActual.val("$" + valorInputOrdenCambioActual.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

                porcentajeIncrementoFinal = (Number(inpTotalMontoOrdenCambioActual.val().replace(/[^0-9.-]+/g, "")) / (montoOriginal == 0 ? 1 : montoOriginal)) * 100;
                $("#porcentaje2").val(porcentajeIncrementoFinal.toFixed(4));
                //

                // inpSumaTotalModificaciones.val("$" + con.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                // valor1 = inpSumaTotalModificaciones.val()
                // if (valor1 != "") {
                //     let valor2 = valor1
                //     inpSumaTotalModificaciones.val(valor2.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"))
                // }

                //
                // inpSumaTotalModificaciones.val(con);
                // valor1 = inpSumaTotalModificaciones.val()
                // if (valor1 != "") {
                //     let valor2 = valor1
                //     inpSumaTotalModificaciones.val(valor2)
                // }

                // inpTotalMontoOrdenCambioActual.val(Number(inpTotalMontoActual.val()) + Number(inpSumaTotalModificaciones.val()));
                // if (inpTotalMontoActual.val() != "" && inpSumaTotalModificaciones.val() != "") {
                //     let valor1 = +inpTotalMontoActual.val().replace(/[^0-9.-]+/g, "");
                //     let valor2 = +inpSumaTotalModificaciones.val().replace(/[^0-9.-]+/g, "");
                //     let valor3 = valor1 + valor2;
                //     valor3 = valor3;
                //     inpTotalMontoOrdenCambioActual.val("$" + valor3.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));


                // }

                // porcentajeIncrementoFinal = (Number(valor3) / (montoOriginal == 0 ? 1 : montoOriginal)) * 100;
                // $("#porcentaje2").val(porcentajeIncrementoFinal.toFixed(4));
                // //

                break;
            case 2:
                let trMod = tblModificaciones.find('tr');
                let conMod = 0;
                for (let b = 1; b < trMod.length; b++) {
                    let td = $(trMod[b]).find('input')
                    if ($(td[4]).val()) {
                        conMod += parseFloat($(td[4]).val().replace(/[^0-9.-]+/g, ""));
                    } else {
                        conMod += 0;
                    }
                }

                if (conMod > 0) {
                    inpModificaciones.val(maskNumero(conMod));
                } else {
                    inpModificaciones.val(maskNumero(0));
                }

                let valorInputSumaTotalModificaciones1 = Number(inpAlcances.val().replace(/[^0-9.-]+/g, "")) + Number(inpModificaciones.val().replace(/[^0-9.-]+/g, "")) + Number(inpReqCampo.val().replace(/[^0-9.-]+/g, "")) + Number(inpAjusteVol.val().replace(/[^0-9.-]+/g, "")) + Number(inpServiySum.val().replace(/[^0-9.-]+/g, ""));
                inpSumaTotalModificaciones.val("$" + valorInputSumaTotalModificaciones1.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                valorInputOrdenCambioActual = Number(inpTotalMontoActual.val().replace(/[^0-9.-]+/g, "")) + Number(inpSumaTotalModificaciones.val().replace(/[^0-9.-]+/g, ""));
                inpTotalMontoOrdenCambioActual.val("$" + valorInputOrdenCambioActual.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

                porcentajeIncrementoFinal = (Number(inpTotalMontoOrdenCambioActual.val().replace(/[^0-9.-]+/g, "")) / (montoOriginal == 0 ? 1 : montoOriginal)) * 100;
                $("#porcentaje2").val(porcentajeIncrementoFinal.toFixed(4));
                break;
            case 3:
                let trRe = tblReqCampo.find('tr');
                let conRe = 0;
                for (let b = 1; b < trRe.length; b++) {
                    let td = $(trRe[b]).find('input')
                    if ($(td[4]).val()) {
                        conRe += parseFloat($(td[4]).val().replace(/[^0-9.-]+/g, ""));
                    } else {
                        conRe += 0;
                    }
                }

                if (conRe > 0) {
                    inpReqCampo.val(maskNumero(conRe));
                } else {
                    inpReqCampo.val(maskNumero(0));
                }

                let valorInputSumaTotalModificaciones2 = Number(inpAlcances.val().replace(/[^0-9.-]+/g, "")) + Number(inpModificaciones.val().replace(/[^0-9.-]+/g, "")) + Number(inpReqCampo.val().replace(/[^0-9.-]+/g, "")) + Number(inpAjusteVol.val().replace(/[^0-9.-]+/g, "")) + Number(inpServiySum.val().replace(/[^0-9.-]+/g, ""));
                inpSumaTotalModificaciones.val("$" + valorInputSumaTotalModificaciones2.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                valorInputOrdenCambioActual = Number(inpTotalMontoActual.val().replace(/[^0-9.-]+/g, "")) + Number(inpSumaTotalModificaciones.val().replace(/[^0-9.-]+/g, ""));
                inpTotalMontoOrdenCambioActual.val("$" + valorInputOrdenCambioActual.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

                porcentajeIncrementoFinal = (Number(inpTotalMontoOrdenCambioActual.val().replace(/[^0-9.-]+/g, "")) / (montoOriginal == 0 ? 1 : montoOriginal)) * 100;
                $("#porcentaje2").val(porcentajeIncrementoFinal.toFixed(4));
                break;
            case 4:
                let trAju = tblAjusteDeVolumenes.find('tr');
                let conAju = 0;
                for (let b = 1; b < trAju.length; b++) {
                    let td = $(trAju[b]).find('input')
                    if ($(td[4]).val()) {
                        conAju += parseFloat($(td[4]).val().replace(/[^0-9.-]+/g, ""));
                    } else {
                        conAju += 0;
                    }
                }

                if (conAju > 0) {
                    inpAjusteVol.val(maskNumero(conAju));
                } else {
                    inpAjusteVol.val(maskNumero(0));
                }

                let valorInputSumaTotalModificaciones3 = Number(inpAlcances.val().replace(/[^0-9.-]+/g, "")) + Number(inpModificaciones.val().replace(/[^0-9.-]+/g, "")) + Number(inpReqCampo.val().replace(/[^0-9.-]+/g, "")) + Number(inpAjusteVol.val().replace(/[^0-9.-]+/g, "")) + Number(inpServiySum.val().replace(/[^0-9.-]+/g, ""));
                inpSumaTotalModificaciones.val("$" + valorInputSumaTotalModificaciones3.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                valorInputOrdenCambioActual = Number(inpTotalMontoActual.val().replace(/[^0-9.-]+/g, "")) + Number(inpSumaTotalModificaciones.val().replace(/[^0-9.-]+/g, ""));
                inpTotalMontoOrdenCambioActual.val("$" + valorInputOrdenCambioActual.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

                porcentajeIncrementoFinal = (Number(inpTotalMontoOrdenCambioActual.val().replace(/[^0-9.-]+/g, "")) / (montoOriginal == 0 ? 1 : montoOriginal)) * 100;
                $("#porcentaje2").val(porcentajeIncrementoFinal.toFixed(4));
                break;
            case 5:
                let trSer = tblServiySum.find('tr');
                let conServ = 0;
                for (let b = 1; b < trSer.length; b++) {
                    let td = $(trSer[b]).find('input')
                    if ($(td[4]).val()) {
                        conServ += parseFloat($(td[4]).val().replace(/[^0-9.-]+/g, ""));
                    } else {
                        conServ += 0;
                    }
                }

                if (conServ > 0) {
                    inpServiySum.val(maskNumero(conServ));
                } else {
                    inpServiySum.val(maskNumero(0));
                }

                let valorInputSumaTotalModificaciones4 = Number(inpAlcances.val().replace(/[^0-9.-]+/g, "")) + Number(inpModificaciones.val().replace(/[^0-9.-]+/g, "")) + Number(inpReqCampo.val().replace(/[^0-9.-]+/g, "")) + Number(inpAjusteVol.val().replace(/[^0-9.-]+/g, "")) + Number(inpServiySum.val().replace(/[^0-9.-]+/g, ""));
                inpSumaTotalModificaciones.val("$" + valorInputSumaTotalModificaciones4.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                valorInputOrdenCambioActual = Number(inpTotalMontoActual.val().replace(/[^0-9.-]+/g, "")) + Number(inpSumaTotalModificaciones.val().replace(/[^0-9.-]+/g, ""));
                inpTotalMontoOrdenCambioActual.val("$" + valorInputOrdenCambioActual.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

                porcentajeIncrementoFinal = (Number(inpTotalMontoOrdenCambioActual.val().replace(/[^0-9.-]+/g, "")) / (montoOriginal == 0 ? 1 : montoOriginal)) * 100;
                $("#porcentaje2").val(porcentajeIncrementoFinal.toFixed(4));

                break;
        }
    }





    function fncEliminar(rowData) {
        Swal.fire({
            title: '¡Alerta!',
            text: "Desea eliminar este soporte?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                for (let i = 0; i < lstDetalle.length; i++) {
                    if (rowData.id == lstDetalle[i].id) {
                        lstDetalle.splice(i, 1);
                        dtAlcances.clear();
                        dtAlcances.rows.add(lstDetalle);
                        dtAlcances.draw();
                        if (lstDetalle.length == 0) {
                            lstDetalle = [{
                                id: 0,
                                no: '',
                                descripcion: '',
                                cantidad: '',
                                unidad: '',
                                PrecioUnitario: '',
                                importe: '',
                                nuevo: true
                            }];
                        }
                    }

                }
                Swal.fire(
                    'Deleted!',
                    'Your file has been deleted.',
                    'success'
                )
            }
        })
    }
    function AddRows(tbl, lst) {
        dtOrdenDeCambios = tbl.DataTable();
        dtOrdenDeCambios.clear().draw();
        dtOrdenDeCambios.rows.add(lst).draw(false);
    }
    function reporte() {
        report.attr(`src`, `/Reportes/Vista.aspx?idReporte=1&inMemory=1`);
        document.getElementById('report').onload = function () {
            $.unblockUI();
            openCRModal();
        }
    }
    function GetParameters(editar) {
        let lstMontos = [];
        let tabla = $('#cboOrdenDeCambio').find('option:selected').attr('data-prefijo');
        let valor = tabla.split('-')[1];

        let formData = new FormData();

        if (document.getElementById("txtArchivoAntecedentes") != null) {
            var file1 = document.getElementById("txtArchivoAntecedentes").files[0];
            if (file1 != undefined) {
                formData.append("AntecedentesArchivos", file1);
            }
        }


        formData.append("editar", editar);

        let tr = tblAlcances.find('tr');
        for (let d = 1; d < tr.length; d++) {
            let input = $(tr[d]).find('input');
            let select = $(tr[d]).find('select');
            if ($(tblAlcances.find('tr')[1]).find('input')[0] != undefined) {

                if ($(input[1]).val() != '' && $(input[2]).val() != '' && $(input[3]).val() != '' && $(input[4]).val() != '' && $(select[0]).find('option:selected').val() != '') {
                    let item = {
                        id: editar == true ? $(input[0]).attr('data-id') : 0,
                        tipoSoportes: 1,
                        no: $(input[0]).val(),
                        descripcion: $(input[1]).val(),
                        unidad: $(select[0]).find('option:selected').val(),
                        cantidad: $(input[2]).val(),
                        PrecioUnitario: unmaskNumero($(input[3]).val()),
                        importe: unmaskNumero($(input[4]).val()),
                        idOrdenDeCambio: 0,
                    }
                    lstMontos.push(item);
                }
            }
        }
        let trMod = tblModificaciones.find('tr');
        for (let d = 1; d < trMod.length; d++) {
            let input = $(trMod[d]).find('input');
            let select = $(tr[d]).find('select');
            if ($(tblModificaciones.find('tr')[1]).find('input')[0] != undefined) {

                if ($(input[1]).val() != '' && $(input[2]).val() != '' && $(input[3]).val() != '' && $(input[4]).val() != '' && $(select[0]).find('option:selected').val() != '') {
                    let item = {
                        id: editar == true ? $(input[0]).attr('data-id') : 0,
                        tipoSoportes: 2,
                        no: $(input[0]).val(),
                        descripcion: $(input[1]).val(),
                        unidad: $(select[0]).find('option:selected').val(),
                        cantidad: $(input[2]).val(),
                        PrecioUnitario: unmaskNumero($(input[3]).val()),
                        importe: unmaskNumero($(input[4]).val()),
                        idOrdenDeCambio: 0,
                    }
                    lstMontos.push(item);
                }
            }
        }
        let trRe = tblReqCampo.find('tr');
        for (let d = 1; d < trRe.length; d++) {
            let input = $(trRe[d]).find('input');
            let select = $(tr[d]).find('select');
            if ($(tblReqCampo.find('tr')[1]).find('input')[0] != undefined) {

                if ($(input[1]).val() != '' && $(input[2]).val() != '' && $(input[3]).val() != '' && $(input[4]).val() != '' && $(select[0]).find('option:selected').val() != '') {
                    let item = {
                        id: editar == true ? $(input[0]).attr('data-id') : 0,
                        tipoSoportes: 3,
                        no: $(input[0]).val(),
                        descripcion: $(input[1]).val(),
                        unidad: $(select[0]).find('option:selected').val(),
                        cantidad: $(input[2]).val(),
                        PrecioUnitario: unmaskNumero($(input[3]).val()),
                        importe: unmaskNumero($(input[4]).val()),
                        idOrdenDeCambio: 0,
                    }
                    lstMontos.push(item);
                }
            }
        }
        let trAju = tblAjusteDeVolumenes.find('tr');
        for (let d = 1; d < trAju.length; d++) {
            let input = $(trAju[d]).find('input');
            let select = $(tr[d]).find('select');
            if ($(tblAjusteDeVolumenes.find('tr')[1]).find('input')[0] != undefined) {

                if ($(input[1]).val() != '' && $(input[2]).val() != '' && $(input[3]).val() != '' && $(input[4]).val() != '' && $(select[0]).find('option:selected').val() != '') {
                    let item = {
                        id: editar == true ? $(input[0]).attr('data-id') : 0,
                        tipoSoportes: 4,
                        no: $(input[0]).val(),
                        descripcion: $(input[1]).val(),
                        unidad: $(select[0]).find('option:selected').val(),
                        cantidad: $(input[2]).val(),
                        PrecioUnitario: unmaskNumero($(input[3]).val()),
                        importe: unmaskNumero($(input[4]).val()),
                        idOrdenDeCambio: 0,
                    }
                    lstMontos.push(item);
                }
            }
        }
        let trServ = tblServiySum.find('tr');
        for (let d = 1; d < trServ.length; d++) {
            let input = $(trServ[d]).find('input');
            let select = $(tr[d]).find('select');
            if ($(tblServiySum.find('tr')[1]).find('input')[0] != undefined) {

                if ($(input[1]).val() != '' && $(input[2]).val() != '' && $(input[3]).val() != '' && $(input[4]).val() != '' && $(select[0]).find('option:selected').val() != '') {
                    let item = {
                        id: editar == true ? $(input[0]).attr('data-id') : 0,
                        tipoSoportes: 5,
                        no: $(input[0]).val(),
                        descripcion: $(input[1]).val(),
                        unidad: $(select[0]).find('option:selected').val(),
                        cantidad: $(input[2]).val(),
                        PrecioUnitario: unmaskNumero($(input[3]).val()),
                        importe: unmaskNumero($(input[4]).val()),
                        idOrdenDeCambio: 0,
                    }
                    lstMontos.push(item);
                }
            }
        }
        let lstFirmas = [
            {
                idRow: 1,
                idFirma: inpIdGerenteProyecto.val(),
                nombre: inpGerenteProyecto.val()
            }, {
                idRow: 2,
                idFirma: inpIdGerenteArea.val(),
                nombre: inpGerenteArea.val()
            }, {
                idRow: 3,
                idFirma: inpIdDirectorDivision.val(),
                nombre: inpDirectorDivision.val()
            }, {
                idRow: 4,
                idFirma: inpIdGerentePMO.val(),
                nombre: inpGerentePMO.val()
            }, {
                idRow: 5,
                idFirma: inpIdDirectorTecnico.val(),
                nombre: inpDirectorTecnico.val()
            }
        ];

        formData.append("id", btnGestionar.attr('data-id'));
        formData.append("proyecto", inpProyecto.val());
        formData.append("NoOrden", cboOrdenDeCambio.val());
        formData.append("CLiente", inpCliente.val());
        formData.append("idSubContratista", inpIdContratista.val());
        formData.append("Contratista", inpContratista.val());
        formData.append("cc", inpcc.val());
        formData.append("Estado", inpEstado.val());
        formData.append("Municipio", inpMunicipio.val());
        formData.append("Direccion", inpDireccion.val());
        formData.append("esCobrable", inpCobrable.prop('checked'));
        formData.append("Antecedentes", txtAntecedentes.val());
        formData.append("FechaAmpliacion", inpFechaAmpliacion.val());

        formData.append("ubicacionProyecto", inpUbicacion.val());
        formData.append("fechaEfectiva", dtFechaEfectiva.val());
        // formData.append("otrasCondicioes", txtCondicions.val());
        formData.append("idContrato", valor);
        // formData.append("fechaDescripcion", inpfechaDescripcion.val());
        // formData.append("alcancesNuevosDescripcion", inpAlcancesDescripcion.val());
        // formData.append("modificacionesPorCambioDescripcion", inpModificacionesDescripcion.val());
        // formData.append("requerimientosDeCampoDescripcion", inpReqCampoDescripcion.val());
        // formData.append("ajusteDeVolumenesDescripcion", inpAjusteVolDescripcion.val());
        // formData.append("serviciosYSuministrosDescripcion", inpServiySumDescripcion.val());
        formData.append("nombreDelArchivo", inpNombreDelArchivo.val());
        // formData.append("respresentanteLegal", inpRespresentanteLegal.val());
        let lstSoportesEvidencia = {
            id: 0,
            idOrdenDeCambio: 0,
            alcancesNuevos: inpAlcances.val(),
            modificacionesPorCambio: inpModificaciones.val(),
            requerimientosDeCampo: inpReqCampo.val(),
            ajusteDeVolumenes: inpAjusteVol.val(),
            serviciosYSuministros: inpServiySum.val(),
            fechaInicial: inpTiempo.val(),
            FechaFinal: inpTiempoFinal.val(),
            // MontoContratoOriginal: inpOriginal.val(),
            MontoContratoOriginalSuma: inpOrdenDeCambioSum.val(),
        };
        if (editar === "false") {
            formData.append("lstFirmas", JSON.stringify(lstFirmas));
            // formData.append("lstFirmas", lstFirmas);
        }

        formData.append("cveEmpleados", JSON.stringify(traermeLasClaves()));
        formData.append("nombreEmpleados", fncGetNombreNotificantes());
        formData.append("lstMontos", JSON.stringify(lstMontos));
        formData.append("lstSoportesEvidencia", JSON.stringify(lstSoportesEvidencia));
        formData.append("representanteLegal", inpRespresentanteLegal.val());
        // lstMontos:lstMontos,
        // lstFirmas:lstFirmas
        return formData;
    }
    function GuardarEditarOrdenDeCambio(editar) {
        // if (editar === "false") {
        //     if (btnGestionar.attr('data-estatus') == 7) {
        //         tieneNotificantes = true;
        //     }
        // }
        // if (inpFechaInicioContrato.val() == '') {
        //     AlertaGeneral('Alerta!', 'Este contrato no tiene fecha de inicio contractual')
        //     return;
        // }
        // if (inpFechaFinalContrato.val() == '') {
        //     AlertaGeneral('Alerta!', 'Este contrato no tiene fecha final contractual')
        //     return;
        // }
        // if (inpRespresentanteLegal.val() == '') {
        //     AlertaGeneral('Alerta!', 'Es necesario capturar el nombre del representante legal.')
        //     return;
        // }

        if (inpDirectorDivision.val() != '' || inpDirectorTecnico.val() != '' || inpGerenteArea.val() != '' || inpGerentePMO.val() != '' || inpGerenteProyecto.val() != '') {

            AddEdit(editar);
            limpiarCampos(true, true);
        } else {
            // if (
            //     inpRespresentanteLegal.val() != '' &&
            //     inpDirectorDivision.val() != '' &&
            //     inpDirectorTecnico.val() != '' &&
            //     inpGerenteArea.val() != '' &&
            //     inpGerentePMO.val() != '' &&
            //     inpGerenteProyecto.val() != ''
            // ) {
            //     AddEdit(editar);

            // } else {
            AlertaGeneral('Alerta!', 'Para gestionar se necesitan al menos un autorizante..')
            // }
        }
    }
    const AddEdit = function (editar) {
        let data = GetParameters(editar);
        $.ajax({
            datatype: "json",
            type: "POST",
            contentType: false,
            processData: false,
            url: "/ControlObra/ControlObra/nuevoEditarOrdenesDeCambio",
            data: data,

            success: function (response) {
                if (response.success) {
                    // mdlgMontosSoportes.modal('hide');
                    // obtenerDatos();
                    alert2Exito(response.items.items);
                } else {
                    Alert2Error(response.message);
                }
            },
            error: function () {
                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            },
            complete: function () {
            }
        });
    }
    function alert2Exito(mensaje) {
        Swal.fire({
            position: 'top-end',
            icon: 'success',
            title: mensaje,
            showConfirmButton: false,
            timer: 1500
        });
    }
    function initSignaturePad() {
        function resizeCanvasFull() {
            _cFirmaFull.width = divFirmaFull.width();
            _cFirmaFull.height = divFirmaFull.height();
            signaturePadFull.clear();
        }

        window.onresize = resizeCanvasFull;
        resizeCanvasFull();
    }
    function fncGuardarFirmas() {

        let lstFirmas = [
            {
                id: Firma1.attr('data-id'),
                idOrdenDeCambio: btnGuardarFirmas.attr('data-id'),
                rutaFirma: document.getElementById('imgFirma1').src,
                idUsuarioFirma: claveDelQueFirma.val() == null ? 0 : claveDelQueFirma.val(),
            },
            {
                id: Firma2.attr('data-id'),
                idOrdenDeCambio: btnGuardarFirmas.attr('data-id'),
                rutaFirma: document.getElementById('imgFirma2').src,
                idUsuarioFirma: claveDelQueFirma.val() == null ? 0 : claveDelQueFirma.val(),
            },
            {
                id: Firma3.attr('data-id'),
                idOrdenDeCambio: btnGuardarFirmas.attr('data-id'),
                rutaFirma: document.getElementById('imgFirma3').src,
                idUsuarioFirma: claveDelQueFirma.val() == null ? 0 : claveDelQueFirma.val(),
            },
            {
                id: Firma4.attr('data-id'),
                idOrdenDeCambio: btnGuardarFirmas.attr('data-id'),
                rutaFirma: document.getElementById('imgFirma4').src,
                idUsuarioFirma: claveDelQueFirma.val() == null ? 0 : claveDelQueFirma.val(),
            },
            {
                id: Firma5.attr('data-id'),
                idOrdenDeCambio: btnGuardarFirmas.attr('data-id'),
                rutaFirma: document.getElementById('imgFirma5').src,
                idUsuarioFirma: claveDelQueFirma.val() == null ? 0 : claveDelQueFirma.val(),
            },
        ]
        axios.post('GenerandoFirmas', { lstFirmas: lstFirmas })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    alert2Exito(items.items)
                    // mdlFormularioDeFirmas.modal('hide');
                    obtenerDatos();
                }
            });
    }


    function inittblModificaciones() {
        dtModificaciones = tblModificaciones.DataTable({
            destroy: true
            , paging: false
            , ordering: false
            , searching: false
            , bFilter: false
            , info: false
            , language: dtDicEsp
            , columns: [
                {
                    data: 'no', title: 'No.', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class='form-control inputNo inputNumeroInsumo'></input>`;
                        } else {
                            html = `<input class='form-control inputNo inputNumeroInsumo' data-id='${row.id}' value='${data}'></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'descripcion', title: 'descripcion', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class="form-control inputDescripcionInsumo" >`;
                        } else {
                            html = `<input class='form-control inputDescripcionInsumo'  value='${data}'></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'unidad', title: 'Unidad', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `
                            <select class="form-control motivoRehabilitacion">
                                <option value='Metros'>Metros</option>
                                <option value='KM'>KM</option>
                                <option value='Lote'>Lote</option>
                                <option value='Persona'>Persona</option>
                                <option value='M2'>M2</option>
                                <option value='M3'>M3</option>
                                <option value='M3/KM'>M3/KM</option>
                            </select>
                        `;
                        } else {
                            if (data == 'Metros') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'KM') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='KM'>KM</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'Lote') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='Lote'>Lote</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'Persona') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='Persona'>Persona</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'M2') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='M2'>M2</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'M3') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='M3'>M3</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'M3/KM') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='M3/KM'>M3/KM</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                </select>
                                `;
                            }
                        }
                        return html;
                    }
                },
                {
                    data: 'cantidad', title: 'Cantidad', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class='form-control cantidad' type='number'></input>`;
                        } else {
                            html = `<input class='form-control cantidad' type='number' value='${data}'></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'PrecioUnitario', title: 'Precio Unitario.', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class='form-control inputNo inputPrecioInsumo' ></input>`;
                        } else {
                            html = `<input class='form-control inputNo inputPrecioInsumo' value='${data}'></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'importe', title: 'Importe', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class='form-control importe' disabled></input>`;
                        } else {
                            html = `<input class='form-control importe' value='${data}' disabled></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'id', title: 'Opciones', render: (data, type, row, meta) => {
                        let html = `<button type="" class="btn btn-danger EliminarSoporte" style="width: 24%;margin-left: 68px;margin-right: -51px;height: auto;padding: 6px;"><span class="glyphicon glyphicon-trash"></span></button>`;
                        return html;
                    }
                },
            ],
            initComplete: function (settings, json) {
                tblModificaciones.on("click", ".EliminarSoporte", function () {
                    // const rowData = dtModificaciones.row($(this).closest("tr")).data();
                    // fncEliminarMod(rowData);
                    // $(this).closest('tr').remove();
                    let $tr = $(this).closest('tr');
                    dtModificaciones.row($tr).remove().draw(false);
                    totalizadorAlcances(2);
                });
            },
            createdRow: function (row, rowData) {
                $(row).find('.inputNumeroInsumo').getAutocompleteValid(
                    (e, ui) => {
                        $(row).find('.inputNumeroInsumo').val(ui.item.id);
                        $(row).find('.inputDescripcionInsumo').val(ui.item.value);
                        $(row).find('.inputPrecioInsumo').val(ui.item.precio);
                        $(row).find('.cantidad').change();
                        e.preventDefault();
                    },
                    (e, ui) => {
                        if (ui.item == null) {
                            $(row).find('.inputNumeroInsumo').val('');
                        }
                    },
                    {
                        busquedaPorNumero: true,
                        cc: cboFiltroCC.val()
                    },
                    'GetInsumosSISUNAutocomplete'
                );
                $(row).find('.inputDescripcionInsumo').getAutocompleteValid(
                    (e, ui) => {
                        $(row).find('.inputNumeroInsumo').val(ui.item.id);
                        $(row).find('.inputDescripcionInsumo').val(ui.item.value);
                        $(row).find('.inputPrecioInsumo').val(ui.item.precio);
                        $(row).find('.cantidad').change();
                    },
                    (e, ui) => {
                        if (ui.item == null) {
                            $(row).find('.inputNumeroInsumo').val('');
                        }
                    },
                    {
                        busquedaPorNumero: false,
                        cc: cboFiltroCC.val()
                    },
                    'GetInsumosSISUNAutocomplete'
                );
            },
            drawCallback: function (settings) {
                tblModificaciones.find('input.cantidad, input.inputPrecioInsumo').on('change', function (e) {
                    let numeroFila = ($(this).closest("tr")[0].rowIndex) - 1;
                    let row = $(this).closest('tr');
                    let cantidad = +$(row).find('.cantidad').val();
                    let precio = +$(row).find('.inputPrecioInsumo').val();
                    let importe = cantidad * precio;

                    dtModificaciones.row(numeroFila).data().cantidad = cantidad;
                    dtModificaciones.row(numeroFila).data().inputNo = precio;
                    dtModificaciones.row(numeroFila).data().importe = importe;

                    $(row).find('.importe').val(maskNumero(importe));
                    totalizadorAlcances(2);
                });
            }
        });
    }
    function fncEliminarMod(rowData) {
        Swal.fire({
            title: '¡Alerta!',
            text: "Desea eliminar este soporte?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                for (let i = 0; i < lstDetalleMod.length; i++) {
                    if (rowData.id == lstDetalleMod[i].id) {
                        lstDetalleMod.splice(i, 1);
                        dtModificaciones.clear();
                        dtModificaciones.rows.add(lstDetalleMod);
                        dtModificaciones.draw();
                        if (lstDetalleMod.length == 0) {
                            lstDetalleMod = [{
                                id: 0,
                                no: '',
                                descripcion: '',
                                cantidad: '',
                                unidad: '',
                                PrecioUnitario: '',
                                importe: '',
                                nuevo: true
                            }];
                        }
                    }

                }
                Swal.fire(
                    'Deleted!',
                    'Your file has been deleted.',
                    'success'
                )
            }
        })
    }
    function inittblReqCampo() {
        dtReqCampo = tblReqCampo.DataTable({
            destroy: true
            , paging: false
            , ordering: false
            , searching: false
            , bFilter: false
            , info: false
            , language: dtDicEsp
            , columns: [
                {
                    data: 'no', title: 'No.', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class='form-control inputNo inputNumeroInsumo'></input>`;
                        } else {
                            html = `<input class='form-control inputNo inputNumeroInsumo' data-id='${row.id}' value='${data}'></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'descripcion', title: 'descripcion', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class="form-control inputDescripcionInsumo" >`;
                        } else {
                            html = `<input class='form-control inputDescripcionInsumo'  value='${data}'></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'unidad', title: 'Unidad', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `
                            <select class="form-control motivoRehabilitacion">
                                <option value='Metros'>Metros</option>
                                <option value='KM'>KM</option>
                                <option value='Lote'>Lote</option>
                                <option value='Persona'>Persona</option>
                                <option value='M2'>M2</option>
                                <option value='M3'>M3</option>
                                <option value='M3/KM'>M3/KM</option>
                            </select>                            
                            `;
                        } else {
                            if (data == 'Metros') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'KM') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='KM'>KM</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'Lote') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='Lote'>Lote</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'Persona') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='Persona'>Persona</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'M2') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='M2'>M2</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'M3') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='M3'>M3</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'M3/KM') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='M3/KM'>M3/KM</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                </select>
                                `;
                            }
                        }
                        return html;
                    }
                },
                {
                    data: 'cantidad', title: 'Cantidad', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class='form-control cantidad' type='number'></input>`;
                        } else {
                            html = `<input class='form-control cantidad' type='number' value='${data}'></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'PrecioUnitario', title: 'Precio Unitario.', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class='form-control inputNo inputPrecioInsumo'></input>`;
                        } else {
                            html = `<input class='form-control inputNo inputPrecioInsumo' value='${data}'></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'importe', title: 'Importe', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class='form-control importe' disabled></input>`;
                        } else {
                            html = `<input class='form-control importe' value='${data}' disabled></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'id', title: 'Opciones', render: (data, type, row, meta) => {
                        let html = `<button type="" class="btn btn-danger EliminarSoporte" style="width: 24%;margin-left: 68px;margin-right: -51px;height: auto;padding: 6px;"><span class="glyphicon glyphicon-trash"></span></button>`;
                        return html;
                    }
                },
            ],
            initComplete: function (settings, json) {
                tblReqCampo.on("click", ".EliminarSoporte", function () {
                    // const rowData = dtReqCampo.row($(this).closest("tr")).data();
                    // fncEliminarReq(rowData);
                    // $(this).closest('tr').remove();
                    let $tr = $(this).closest('tr');
                    dtReqCampo.row($tr).remove().draw(false);
                    totalizadorAlcances(3);
                });
            },
            createdRow: function (row, rowData) {
                $(row).find('.inputNumeroInsumo').getAutocompleteValid(
                    (e, ui) => {
                        $(row).find('.inputNumeroInsumo').val(ui.item.id);
                        $(row).find('.inputDescripcionInsumo').val(ui.item.value);
                        $(row).find('.inputPrecioInsumo').val(ui.item.precio);
                        $(row).find('.cantidad').change();
                        e.preventDefault();
                    },
                    (e, ui) => {
                        if (ui.item == null) {
                            $(row).find('.inputNumeroInsumo').val('');
                        }
                    },
                    {
                        busquedaPorNumero: true,
                        cc: cboFiltroCC.val()
                    },
                    'GetInsumosSISUNAutocomplete'
                );
                $(row).find('.inputDescripcionInsumo').getAutocompleteValid(
                    (e, ui) => {
                        $(row).find('.inputNumeroInsumo').val(ui.item.id);
                        $(row).find('.inputDescripcionInsumo').val(ui.item.value);
                        $(row).find('.inputPrecioInsumo').val(ui.item.precio);
                        $(row).find('.cantidad').change();
                    },
                    (e, ui) => {
                        if (ui.item == null) {
                            $(row).find('.inputNumeroInsumo').val('');
                        }
                    },
                    {
                        busquedaPorNumero: false,
                        cc: cboFiltroCC.val()
                    },
                    'GetInsumosSISUNAutocomplete'
                );
            },
            drawCallback: function (settings) {
                tblReqCampo.find('input.cantidad, input.inputPrecioInsumo').on('change', function (e) {
                    let numeroFila = ($(this).closest("tr")[0].rowIndex) - 1;
                    let row = $(this).closest('tr');
                    let cantidad = +$(row).find('.cantidad').val();
                    let precio = +$(row).find('.inputPrecioInsumo').val();
                    let importe = cantidad * precio;

                    dtReqCampo.row(numeroFila).data().cantidad = cantidad;
                    dtReqCampo.row(numeroFila).data().inputNo = precio;
                    dtReqCampo.row(numeroFila).data().importe = importe;

                    $(row).find('.importe').val(maskNumero(importe));
                    totalizadorAlcances(3);
                });
            }
        });
    }
    function fncEliminarReq(rowData) {
        Swal.fire({
            title: '¡Alerta!',
            text: "Desea eliminar este soporte?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                for (let i = 0; i < lstDetalleReq.length; i++) {
                    if (rowData.id == lstDetalleReq[i].id) {
                        lstDetalleReq.splice(i, 1);
                        dtReqCampo.clear();
                        dtReqCampo.rows.add(lstDetalleReq);
                        dtReqCampo.draw();
                        if (lstDetalleReq.length == 0) {
                            lstDetalleReq = [{
                                id: 0,
                                no: '',
                                descripcion: '',
                                cantidad: '',
                                unidad: '',
                                PrecioUnitario: '',
                                importe: '',
                                nuevo: true
                            }];
                        }
                    }

                }
                Swal.fire(
                    'Deleted!',
                    'Your file has been deleted.',
                    'success'
                )
            }
        })
    }
    function inittblAjust() {
        dtAjusteDeVolumenes = tblAjusteDeVolumenes.DataTable({
            destroy: true
            , paging: false
            , ordering: false
            , searching: false
            , bFilter: false
            , info: false
            , language: dtDicEsp
            , columns: [
                {
                    data: 'no', title: 'No.', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class='form-control inputNo inputNumeroInsumo'></input>`;
                        } else {
                            html = `<input class='form-control inputNo inputNumeroInsumo' data-id='${row.id}' value='${data}'></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'descripcion', title: 'descripcion', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class="form-control inputDescripcionInsumo" >`;
                        } else {
                            html = `<input class='form-control inputDescripcionInsumo'  value='${data}'></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'unidad', title: 'Unidad', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `
                            <select class="form-control motivoRehabilitacion">
                                <option value='Metros'>Metros</option>
                                <option value='KM'>KM</option>
                                <option value='Lote'>Lote</option>
                                <option value='Persona'>Persona</option>
                                <option value='M2'>M2</option>
                                <option value='M3'>M3</option>
                                <option value='M3/KM'>M3/KM</option>
                            </select>
                            `;
                        } else {
                            if (data == 'Metros') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'KM') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='KM'>KM</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'Lote') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='Lote'>Lote</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'Persona') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='Persona'>Persona</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'M2') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='M2'>M2</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'M3') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='M3'>M3</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'M3/KM') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='M3/KM'>M3/KM</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                </select>
                                `;
                            }
                        }
                        return html;
                    }
                },
                {
                    data: 'cantidad', title: 'Cantidad', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class='form-control cantidad' type='number'></input>`;
                        } else {
                            html = `<input class='form-control cantidad' type='number' value='${data}'></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'PrecioUnitario', title: 'Precio Unitario.', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class='form-control inputNo inputPrecioInsumo'></input>`;
                        } else {
                            html = `<input class='form-control inputNo inputPrecioInsumo' value='${data}'></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'importe', title: 'Importe', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class='form-control importe' disabled></input>`;
                        } else {
                            html = `<input class='form-control importe' value='${data}' disabled></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'id', title: 'Opciones', render: (data, type, row, meta) => {
                        let html = `<button type="" class="btn btn-danger EliminarSoporte" style="width: 24%;margin-left: 68px;margin-right: -51px;height: auto;padding: 6px; text-center"><span class="glyphicon glyphicon-trash"></span></button>`;
                        return html;
                    }
                },
            ]
            , initComplete: function (settings, json) {
                tblAjusteDeVolumenes.on("click", ".EliminarSoporte", function () {
                    // const rowData = dtAjusteDeVolumenes.row($(this).closest("tr")).data();
                    // fncEliminarAjust(rowData);
                    // $(this).closest('tr').remove();
                    let $tr = $(this).closest('tr');
                    dtAjusteDeVolumenes.row($tr).remove().draw(false);
                    totalizadorAlcances(4);
                });
            },
            createdRow: function (row, rowData) {
                $(row).find('.inputNumeroInsumo').getAutocompleteValid(
                    (e, ui) => {
                        $(row).find('.inputNumeroInsumo').val(ui.item.id);
                        $(row).find('.inputDescripcionInsumo').val(ui.item.value);
                        $(row).find('.inputPrecioInsumo').val(ui.item.precio);
                        $(row).find('.cantidad').change();
                        e.preventDefault();
                    },
                    (e, ui) => {
                        if (ui.item == null) {
                            $(row).find('.inputNumeroInsumo').val('');
                        }
                    },
                    {
                        busquedaPorNumero: true,
                        cc: cboFiltroCC.val()
                    },
                    'GetInsumosSISUNAutocomplete'
                );
                $(row).find('.inputDescripcionInsumo').getAutocompleteValid(
                    (e, ui) => {
                        $(row).find('.inputNumeroInsumo').val(ui.item.id);
                        $(row).find('.inputDescripcionInsumo').val(ui.item.value);
                        $(row).find('.inputPrecioInsumo').val(ui.item.precio);
                        $(row).find('.cantidad').change();
                    },
                    (e, ui) => {
                        if (ui.item == null) {
                            $(row).find('.inputNumeroInsumo').val('');
                        }
                    },
                    {
                        busquedaPorNumero: false,
                        cc: cboFiltroCC.val()
                    },
                    'GetInsumosSISUNAutocomplete'
                );
            },
            drawCallback: function (settings) {
                tblAjusteDeVolumenes.find('input.cantidad, input.inputPrecioInsumo').on('change', function (e) {
                    let numeroFila = ($(this).closest("tr")[0].rowIndex) - 1;
                    let row = $(this).closest('tr');
                    let cantidad = +$(row).find('.cantidad').val();
                    let precio = +$(row).find('.inputPrecioInsumo').val();
                    let importe = cantidad * precio;

                    dtAjusteDeVolumenes.row(numeroFila).data().cantidad = cantidad;
                    dtAjusteDeVolumenes.row(numeroFila).data().inputNo = precio;
                    dtAjusteDeVolumenes.row(numeroFila).data().importe = importe;

                    $(row).find('.importe').val(maskNumero(importe));
                    totalizadorAlcances(4);
                });
            }
        });
    }
    function fncEliminarAjust(rowData) {
        Swal.fire({
            title: '¡Alerta!',
            text: "Desea eliminar este soporte?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                for (let i = 0; i < lstDetalleAjust.length; i++) {
                    if (rowData.id == lstDetalleAjust[i].id) {
                        lstDetalleAjust.splice(i, 1);
                        dtAjusteDeVolumenes.clear();
                        dtAjusteDeVolumenes.rows.add(lstDetalleAjust);
                        dtAjusteDeVolumenes.draw();
                        if (lstDetalleAjust.length == 0) {
                            lstDetalleAjust = [{
                                id: 0,
                                no: '',
                                descripcion: '',
                                cantidad: '',
                                unidad: '',
                                PrecioUnitario: '',
                                importe: '',
                                nuevo: true
                            }];
                        }
                    }

                }
                Swal.fire(
                    'Deleted!',
                    'Your file has been deleted.',
                    'success'
                )
            }
        })
    }
    function inittblServ() {
        dtServiySum = tblServiySum.DataTable({
            destroy: true
            , paging: false
            , ordering: false
            , searching: false
            , bFilter: false
            , info: false
            , language: dtDicEsp
            , columns: [
                {
                    data: 'no', title: 'No.', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class='form-control inputNo inputNumeroInsumo'></input>`;
                        } else {
                            html = `<input class='form-control inputNo inputNumeroInsumo' data-id='${row.id}' value='${data}'></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'descripcion', title: 'descripcion', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class="form-control inputDescripcionInsumo" >`;
                        } else {
                            html = `<input class='form-control inputDescripcionInsumo'  value='${data}'></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'unidad', title: 'Unidad', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `
                            <select class="form-control motivoRehabilitacion">
                                <option value='Metros'>Metros</option>
                                <option value='KM'>KM</option>
                                <option value='Lote'>Lote</option>
                                <option value='Persona'>Persona</option>
                                <option value='M2'>M2</option>
                                <option value='M3'>M3</option>
                                <option value='M3/KM'>M3/KM</option>
                            </select>                            
                            `;
                        } else {
                            if (data == 'Metros') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'KM') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='KM'>KM</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'Lote') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='Lote'>Lote</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'Persona') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='Persona'>Persona</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'M2') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='M2'>M2</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M3'>M3</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'M3') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='M3'>M3</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3/KM'>M3/KM</option>
                                </select>
                                `;
                            } else if (data == 'M3/KM') {
                                html = `
                                <select class="form-control motivoRehabilitacion">
                                    <option value='M3/KM'>M3/KM</option>
                                    <option value='Metros'>Metros</option>
                                    <option value='KM'>KM</option>
                                    <option value='Lote'>Lote</option>
                                    <option value='Persona'>Persona</option>
                                    <option value='M2'>M2</option>
                                    <option value='M3'>M3</option>
                                </select>
                                `;
                            }
                        }
                        return html;
                    }
                },
                {
                    data: 'cantidad', title: 'Cantidad', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class='form-control cantidad' type='number'></input>`;
                        } else {
                            html = `<input class='form-control cantidad' type='number' value='${data}'></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'PrecioUnitario', title: 'Precio Unitario.', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class='form-control inputNo inputPrecioInsumo'></input>`;
                        } else {
                            html = `<input class='form-control inputNo inputPrecioInsumo' value='${data}'></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'importe', title: 'Importe', render: (data, type, row, meta) => {
                        let html = '';
                        if (data == '') {
                            html = `<input class='form-control importe' disabled></input>`;
                        } else {
                            html = `<input class='form-control importe' value='${data}' disabled></input>`;
                        }
                        return html;
                    }
                },
                {
                    data: 'id', title: 'Opciones', render: (data, type, row, meta) => {
                        let html = `<button type="" class="btn btn-danger EliminarSoporte" style="width: 24%;margin-left: 68px;margin-right: -51px;height: auto;padding: 6px;"><span class="glyphicon glyphicon-trash"></span></button>`;
                        return html;
                    }
                },
            ],
            initComplete: function (settings, json) {
                tblServiySum.on("click", ".EliminarSoporte", function () {
                    // const rowData = dtServiySum.row($(this).closest("tr")).data();

                    // $(this).parents("tr").remove().draw(false);
                    let $tr = $(this).closest('tr');
                    dtServiySum.row($tr).remove().draw(false);
                    // Le pedimos al DataTable que borre la fila
                    totalizadorAlcances(5);

                    // dtServiySum.rows.add(lstDetalleServ);
                });
            },
            createdRow: function (row, rowData) {
                $(row).find('.inputNumeroInsumo').getAutocompleteValid(
                    (e, ui) => {
                        $(row).find('.inputNumeroInsumo').val(ui.item.id);
                        $(row).find('.inputDescripcionInsumo').val(ui.item.value);
                        $(row).find('.inputPrecioInsumo').val(ui.item.precio);
                        $(row).find('.cantidad').change();
                        e.preventDefault();
                    },
                    (e, ui) => {
                        if (ui.item == null) {
                            $(row).find('.inputNumeroInsumo').val('');
                        }
                    },
                    {
                        busquedaPorNumero: true,
                        cc: cboFiltroCC.val()
                    },
                    'GetInsumosSISUNAutocomplete'
                );
                $(row).find('.inputDescripcionInsumo').getAutocompleteValid(
                    (e, ui) => {
                        $(row).find('.inputNumeroInsumo').val(ui.item.id);
                        $(row).find('.inputDescripcionInsumo').val(ui.item.value);
                        $(row).find('.inputPrecioInsumo').val(ui.item.precio);
                        $(row).find('.cantidad').change();
                    },
                    (e, ui) => {
                        if (ui.item == null) {
                            $(row).find('.inputNumeroInsumo').val('');
                        }
                    },
                    {
                        busquedaPorNumero: false,
                        cc: cboFiltroCC.val()
                    },
                    'GetInsumosSISUNAutocomplete'
                );
            },
            drawCallback: function (settings) {
                tblServiySum.find('input.cantidad, input.inputPrecioInsumo').on('change', function (e) {
                    let numeroFila = ($(this).closest("tr")[0].rowIndex) - 1;
                    let row = $(this).closest('tr');
                    let cantidad = +$(row).find('.cantidad').val();
                    let precio = +$(row).find('.inputPrecioInsumo').val();
                    let importe = cantidad * precio;

                    dtServiySum.row(numeroFila).data().cantidad = cantidad;
                    dtServiySum.row(numeroFila).data().inputNo = precio;
                    dtServiySum.row(numeroFila).data().importe = importe;

                    $(row).find('.importe').val(maskNumero(importe));
                    totalizadorAlcances(5);
                });
            }
        });
    }
    function fncEliminarServ(rowData) {
        Swal.fire({
            title: '¡Alerta!',
            text: "Desea eliminar este soporte?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {

                for (let i = 0; i < lstDetalleServ.length; i++) {
                    if (rowData.id == lstDetalleServ[i].id) {
                        lstDetalleServ.splice(i, 1);
                        dtServiySum.clear();
                        dtServiySum.rows.add(lstDetalleServ);
                        dtServiySum.draw();
                        if (lstDetalleServ.length == 0) {
                            lstDetalleServ = [{
                                id: 0,
                                no: '',
                                descripcion: '',
                                cantidad: '',
                                unidad: '',
                                PrecioUnitario: '',
                                importe: '',
                                nuevo: true
                            }];
                        }
                    }

                }
                Swal.fire(
                    'Deleted!',
                    'Your file has been deleted.',
                    'success'
                )
            }
        })
    }
    function totalizadorEditar(tipo, lst) {
        let con = 0;
        switch (tipo) {
            case 1:
                for (let b = 0; b < lst.length; b++) {
                    con += lst[b].cantidad * lst[b].PrecioUnitario;
                }
                inpAlcances.val(con);
                break;
            case 2:
                for (let b = 0; b < lst.length; b++) {
                    con += lst[b].cantidad * lst[b].PrecioUnitario;
                }
                inpModificaciones.val(con);
                break;
            case 3:
                for (let b = 0; b < lst.length; b++) {
                    con += lst[b].cantidad * lst[b].PrecioUnitario;
                }
                inpReqCampo.val(con);
                break;
            case 4:
                for (let b = 0; b < lst.length; b++) {
                    con += lst[b].cantidad * lst[b].PrecioUnitario;
                }
                inpAjusteVol.val(con);
                break;
            case 5:
                for (let b = 0; b < lst.length; b++) {
                    con += lst[b].cantidad * lst[b].PrecioUnitario;
                }
                inpServiySum.val(con);
                break;
        }
    }
    function limpiarTodosLosCampos() {
        inpProyecto.val('');
        inpOrden.val('');
        inpCliente.val('');
        inpCobrable.val('');
        inpEstado.val('');
        inpMunicipio.val('');
        inpContratista.val('');
        inpcc.val('');
        inpDireccion.val('');
        dtFechaEfectiva.val('');
        inpAlcances.val('');
        inpModificaciones.val('');
        inpReqCampo.val('');
        inpAjusteVol.val('');
        inpServiySum.val('');
        inpTiempo.val('');
        inpTiempoFinal.val('');
        txtAntecedentes.val('');
        txtCondicions.val('');
        inpOriginal.val('');
        inpOrdenDeCambioSum.val('');
        lstDetalle = [{
            id: 0,
            no: '',
            descripcion: '',
            cantidad: '',
            unidad: '',
            PrecioUnitario: '',
            importe: '',
            nuevo: true
        }];
        lstDetalleMod = [{
            id: 0,
            no: '',
            descripcion: '',
            cantidad: '',
            unidad: '',
            PrecioUnitario: '',
            importe: '',
            nuevo: true
        }];
        lstDetalleReq = [{
            id: 0,
            no: '',
            descripcion: '',
            cantidad: '',
            unidad: '',
            PrecioUnitario: '',
            importe: '',
            nuevo: true
        }];
        lstDetalleAjust = [{
            id: 0,
            no: '',
            descripcion: '',
            cantidad: '',
            unidad: '',
            PrecioUnitario: '',
            importe: '',
            nuevo: true
        }];
        lstDetalleServ = [{
            id: 0,
            no: '',
            descripcion: '',
            cantidad: '',
            unidad: '',
            PrecioUnitario: '',
            importe: '',
            nuevo: true
        }];
        limpiarCampos();
    }

    function obtenerFuncion() {
        $('input.botonDocumento').change(function () {
            $(this).attr('archivosCambiados', 1);

            if ($(this)[0].files.length > 0) {
                visualizarArchivoCargado(this);

                if ($(this)[0].files.length > 1) {
                    $(this).closest('.panel-body').find('.labelNombre').text($(this)[0].files.length + ' archivos cargados.');
                } else {
                    $(this).closest('.panel-body').find('.labelNombre').text($(this)[0].files[0].name);
                }
            } else {
                visualizarArchivoNoCargado(this);

                $(this).closest('.panel-body').find('.labelNombre').text('Ningún Archivo Seleccionado');
            }
        });
    }
    function visualizarArchivoCargado(elemento) {
        $(elemento).closest('.panel-body').removeClass('p-primary');
        $(elemento).closest('.panel-body').addClass('p-success');
        $(elemento).closest('.panel-body').find('i').removeClass('fa-file-alt');
        $(elemento).closest('.panel-body').find('i').addClass('fa-check');

        $(elemento).closest('.panel').find('.panel-title').find('.botonVerArchivos').remove();
    }
    function visualizarArchivoNoCargado(elemento) {
        $(elemento).closest('.panel-body').addClass('p-primary');
        $(elemento).closest('.panel-body').removeClass('p-success');
        $(elemento).closest('.panel-body').find('i').addClass('fa-file-alt');
        $(elemento).closest('.panel-body').find('i').removeClass('fa-check');

        $(elemento).closest('.panel').find('.panel-title').find('.botonVerArchivos').remove();
    }
    function visualizarArchivoGuardado(elemento, archivos, tipoAnexo) {
        $(elemento).closest('.panel-body').removeClass('p-primary');
        $(elemento).closest('.panel-body').addClass('p-success');
        $(elemento).closest('.panel-body').find('i').removeClass('fa-file-alt');
        $(elemento).closest('.panel-body').find('i').addClass('fa-check');


        let rutaSeccionada = archivos.split('\\');

        $(elemento).closest('.panel-body').find('.labelNombre').text(rutaSeccionada[rutaSeccionada.length - 1]);
    }

    function agregarANotificantes() {
        axios.post('obtenerPuestos')
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {

                    $('#notifi').remove();

                    let html = `
                        <div class="card" id='notifi'>
                            <ul class="list-group list-group-flush">
                    `;
                    let contador = 0;
                    items.forEach(x => {
                        contador++;
                        html += `
                                <div class='col-md-6'>
                                    <li class="list-group-item">
                                        <input id='inpFc${contador}' class='form-control'  type="checkbox" data-toggle="toggle" data-text = '${x.Text}' data-id='${x.Value}'> ${x.Text}</input>
                                    </li>
                                </div>
                         `;

                    });
                    html += `
                            </ul>
                        </div>
                    `;

                    contenidoNotificantes.append(html);
                    let con = 0;
                    items.forEach(x => {
                        con++;
                        if (x.Value == 21782 ||
                            x.Value == 25806 ||
                            x.Value == 173 ||
                            x.Value == 20396 ||
                            x.Value == 51 ||
                            x.Value == 351 ||
                            x.Value == 667 ||
                            x.Value == 21889) {
                            $(`#inpFc${con}`).bootstrapToggle('on');
                        } else {
                            $(`#inpFc${con}`).bootstrapToggle('off');
                        }
                    });
                }
            });
    }
    function traermeLasClaves() {
        let input = $('#contenidoNotificantes').find('input');
        let lstClave = [];
        for (let i = 0; i < input.length; i++) {
            if ($(input[i]).prop('checked') == true) {
                let item = {
                    cveEmpleados: $(input[i]).attr('data-id')
                }
                lstClave.push(item);
            }
        }
        return lstClave;
    }

    function fncGetNombreNotificantes() {
        let input = $('#contenidoNotificantes').find('input');
        let lstClave = [];
        for (let i = 0; i < input.length; i++) {
            if ($(input[i]).prop('checked') == true) {
                lstClave.push($(input[i]).attr('data-text'));
            }
        }
        return lstClave;
    }
    //#endregion

    $(document).ready(() => {
        gestion.GestionOrden = new GestionOrden();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();