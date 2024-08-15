var AsignacioncomparativosController = function () {

    const btnBuscar = $('#btnBuscar');
    const btnAutorizar = $('#btnAutorizar');
    const btnAutorizarFinanciera = $('#btnAutorizarFinanciera');
    const tblAutorizacion = $('#tblM_AutorizarAsginacionNoEconomigo');
    const tblAutorizanteAdquisicion = $('#tblM_AutorizanteAdquisicion');
    const tblAutorizantesFinanciera = $('#tblM_AutorizantesFinanciera');
    const tblComparativo = $('#tblM_ComparativoAdquisicionyRenta');
    const tblFinanciera = $('#tblM_ComparativoFinanciera');
    const btnReporteImprimir = $('#btnReporteImprimir');
    const contenidoPRINTadquisicion = $('#contenidoPRINTadquisicion');
    const modalAsignarSolicitud = $('#modalAsignarSolicitud');
    const inputAsignarSolicitud = $('#inputAsignarSolicitud');
    const botonGuardarAsignacion = $('#botonGuardarAsignacion');
    const ireport = $("#report");
    const inputFechaInicio = $('#inputFechaInicio');
    const inputFechaFin = $('#inputFechaFin');
    const botonNuevoCuadro = $('#botonNuevoCuadro');
    const btnLimpiarFinanciero = $('#btnLimpiarFinanciero');
    const btnNuevoFinanciero = $('#btnNuevoFinanciero');
    const btnAgregarColumnaFinanciero = $('#btnAgregarColumnaFinanciero');
    const btnCerrarCuadroComparativoFinanciero = $('#btnCerrarCuadroComparativoFinanciero');
    const dlgFormAdquisicionNuevoCuadro = $('#dlgFormAdquisicionNuevoCuadro');
    const idAsignacionNuevoCuadro = $('#idAsignacionNuevoCuadro');
    const txtObra = $('#txtObra');
    const txtNombreDelEquipo = $('#txtNombreDelEquipo');
    const checkCompra = $('#checkCompra');
    const checkRenta = $('#checkRenta');
    const checkRoc = $('#checkRoc');
    const tblComparativoAdquisicionyRenta = $('#tblComparativoAdquisicionyRenta');
    const printTipoMoneda = $('#printTipoMoneda');
    const selAutSolicita1 = $('#selAutSolicita1');
    const selAutSolicita2 = $('#selAutSolicita2');
    const selAutSolicita3 = $('#selAutSolicita3');
    const selAutSolicita4 = $('#selAutSolicita4');
    const selAutSolicita5 = $('#selAutSolicita5');
    const btnAgregarColumna = $('#btnAgregarColumna');
    const btnEliminarColumna = $('#btnEliminarColumna');
    const btnNuevo = $('#btnNuevo');

    let dtComaparativo;
    let idAsignacion;
    let _idCuadro;
    let idAsignacionp = 0;
    let dtAutorizacion;
    let dtFinanciera;
    let dtAutorizanteAdquisicion;
    let dtAutorizantesFinanciera;
    idDet1 = 0;
    idDet2 = 0;
    idDet3 = 0;
    idDet4 = 0;
    idDet5 = 0;
    idDet6 = 0;
    idDet7 = 0;
    let renta;
    let ObjParametros = {
        idComparativoDetalle: '',
        idAsignacion: ''
    }

    let idCaracteristica1;
    let idCaracteristica2;
    let idCaracteristica3;
    let idCaracteristica4;
    let idCaracteristica5;
    let idCaracteristica6;
    let idCaracteristica7;

    let idCaracteristica21;
    let idCaracteristica22;
    let idCaracteristica23;
    let idCaracteristica24;
    let idCaracteristica25;
    let idCaracteristica26;
    let idCaracteristica27;
    let idCaracteristica31;
    let idCaracteristica32;
    let idCaracteristica33;
    let idCaracteristica34;
    let idCaracteristica35;
    let idCaracteristica36;
    let idCaracteristica37;
    let idCaracteristica41;
    let idCaracteristica42;
    let idCaracteristica43;
    let idCaracteristica44;
    let idCaracteristica45;
    let idCaracteristica46;
    let idCaracteristica47;
    let idCaracteristica51;
    let idCaracteristica52;
    let idCaracteristica53;
    let idCaracteristica54;
    let idCaracteristica55;
    let idCaracteristica56;
    let idCaracteristica57;
    let idCaracteristica61;
    let idCaracteristica62;
    let idCaracteristica63;
    let idCaracteristica64;
    let idCaracteristica65;
    let idCaracteristica66;
    let idCaracteristica67;
    let idCaracteristica71;
    let idCaracteristica72;
    let idCaracteristica73;
    let idCaracteristica74;
    let idCaracteristica75;
    let idCaracteristica76;
    let idCaracteristica77;

    let ObjParametrosFinanciera = {
        idRow: '',
        idAsignacion: ''
    }

    let objColor = {
        id: '',
        color: ''
    }

    _idCuadro = 0;
    let columnasNuevoCuadro = 1;
    let folio = '';

    //#endregion
    var Inicializar = function () {
        moment.locale("es");

        $.namespace('AsignacionComparativo.Asignacion');

        var date = new Date();
        date.setMonth(date.getMonth(), 1);
        inputFechaInicio.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", date);
        inputFechaFin.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", new Date());
        IniciarModal();
        bootGRenta();

        fcnButons();
        fncBotonesmodal();
        fncBtnAutorizar();
        botonGuardarAsignacion.click(guardarAsignacion);
        botonNuevoCuadro.click(() => {
            btnNuevo.data().id = 0;
            mostrarModalCuadro();
        });

        btnAgregarColumna.click(fncAgregarColumnas);
        btnEliminarColumna.click(fncEliminarColumna);

        btnNuevo.click(guardarCuadro);

        $("#dlgFormAdquisicionNuevoCuadro").dialog({
            draggable: false,
            modal: true,
            resizable: true,
            width: "100%",
            height: "100%",
            autoOpen: false,
            position: 'absolute'
        });

        Iniciar();
    }

    function guardarCuadro() {
        let data = getFormDataCuadro();

        $.ajax({
            url: '/CatMaquina/GuardarCuadroIndependiente',
            data: data,
            async: false,
            cache: false,
            contentType: false,
            processData: false,
            method: 'POST'
        }).then(response => {
            if (response.success) {
                if (btnNuevo.data().id == 0) {
                    AlertaGeneral("Folio", "Número de folio:" + response.folio);
                } else if (btnNuevo.data().id > 0) {
                    Alert2Exito('Se ha guardado la información.');
                }

                dlgFormAdquisicionNuevoCuadro.dialog("close");
                btnBuscar.click();
            } else {
                AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
            }
        }, error => {
            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
        });
    }

    function getFormDataCuadro() {
        let data = new FormData();

        //#region Archivos
        for (let i = 1; i < 8; i++) {
            if (document.getElementById(`inputAgregarImagen${i}`) != null) {
                if (document.getElementById(`inputAgregarImagen${i}`).files.length > 0) {
                    data.append('listaArchivos', document.getElementById(`inputAgregarImagen${i}`).files[0]);
                }
            }
        }
        //#endregion

        let comparativo = {
            idCuadro: btnNuevo.data().id,
            idAsignacion: 0,
            obra: txtObra.val(),
            nombreDelEquipo: txtNombreDelEquipo.val(),
            compra: $('#checkCompra').prop('checked'),
            renta: $('#checkRenta').prop('checked'),
            roc: $('#checkRoc').prop('checked')
        };

        data.append('comparativo', JSON.stringify(comparativo));

        //#region Detalle
        let detalle = [];

        for (let i = 1; i < 8; i++) {
            let lstCaracteristicas = [];

            for (let j = 0; j < 7; j++) {
                lstCaracteristicas.push(objCaracteristicas(i, j));
            }

            if ($(`#idMarcaNum${i}Marca`).val() && $(`#idMarcaNum${i}proveedor`).val() && $(`#idMarcaNum${i}precio`).val() &&
                $(`#idMarcaNum${i}Trade`).val() && $(`#idMarcaNum${i}Valores`).val() && $(`#idMarcaNum${i}Precio`).val() &&
                $(`#idMarcaNum${i}PrecioRoc`).val() && $(`#idMarcaNum${i}BaseHoras`).val() && $(`#idMarcaNum${i}Tiempo`).val() &&
                $(`#idMarcaNum${i}Ubicacion`).val() && $(`#idMarcaNum${i}Horas`).val() && $(`#idMarcaNum${i}Seguro`).val() &&
                $(`#idMarcaNum${i}Garantia`).val() && $(`#idMarcaNum${i}Servicios`).val() && $(`#idMarcaNum${i}Capacitacion`).val() &&
                $(`#idMarcaNum${i}Deposito`).val() && $(`#idMarcaNum${i}Lugar`).val() && $(`#idMarcaNum${i}Flete`).val() && $(`#idMarcaNum${i}Condiciones`).val()) {

                detalle.push({
                    idRow: i,
                    idComparativo: 0,
                    marcaModelo: $(`#idMarcaNum${i}Marca`).val(),
                    proveedor: $(`#idMarcaNum${i}proveedor`).val(),
                    precioDeVenta: $(`#idMarcaNum${i}precio`).val(),
                    tradeIn: $(`#idMarcaNum${i}Trade`).val(),
                    valoresDeRecompra: $(`#idMarcaNum${i}Valores`).val(),
                    precioDeRentaPura: $(`#idMarcaNum${i}Precio`).val(),
                    precioDeRentaEnRoc: $(`#idMarcaNum${i}PrecioRoc`).val(),
                    baseHoras: $(`#idMarcaNum${i}BaseHoras`).val(),
                    tiempoDeEntrega: $(`#idMarcaNum${i}Tiempo`).val(),
                    ubicacion: $(`#idMarcaNum${i}Ubicacion`).val(),
                    horas: $(`#idMarcaNum${i}Horas`).val(),
                    seguro: $(`#idMarcaNum${i}Seguro`).val(),
                    garantia: $(`#idMarcaNum${i}Garantia`).val(),
                    serviciosPreventivos: $(`#idMarcaNum${i}Servicios`).val(),
                    capacitacion: $(`#idMarcaNum${i}Capacitacion`).val(),
                    depositoEnGarantia: $(`#idMarcaNum${i}Deposito`).val(),
                    lugarDeEntrega: $(`#idMarcaNum${i}Lugar`).val(),
                    flete: $(`#idMarcaNum${i}Flete`).val(),
                    condicionesDePagoEntrega: $(`#idMarcaNum${i}Condiciones`).val(),
                    caracteristicasDelEquipo1: $(`#Caracteristica1`).val(),
                    caracteristicasDelEquipo2: $(`#Caracteristica2`).val(),
                    caracteristicasDelEquipo3: $(`#Caracteristica3`).val(),
                    caracteristicasDelEquipo4: $(`#Caracteristica4`).val(),
                    caracteristicasDelEquipo5: $(`#Caracteristica5`).val(),
                    caracteristicasDelEquipo6: $(`#Caracteristica6`).val(),
                    caracteristicasDelEquipo7: $(`#Caracteristica7`).val(),
                    tipoMoneda: $(`#printTipoMoneda`).val(),
                    comentarios: $(`#textareaComentarios` + i).val() != undefined ? $(`#textareaComentarios` + i).val() : '',
                    lstCaracteristicas: lstCaracteristicas
                });
            }
        }

        data.append('detalle', JSON.stringify(detalle));
        //#endregion

        //#region Autorizantes
        let listaAutorizantes = [];

        if (selAutSolicita1.data("id") != '') {
            listaAutorizantes.push({
                id: 0,
                idAsignacion: 0,
                autorizanteID: selAutSolicita1.data("id"),
                autorizanteNombre: selAutSolicita1.val(),
                autorizanteStatus: false,
                autorizanteFinal: false,
                orden: 1
            });
        }

        if (selAutSolicita2.data("id") != '') {
            listaAutorizantes.push({
                id: 0,
                idAsignacion: 0,
                autorizanteID: selAutSolicita2.data("id"),
                autorizanteNombre: selAutSolicita2.val(),
                autorizanteStatus: false,
                autorizanteFinal: false,
                orden: 2
            });
        }

        if (selAutSolicita3.data("id") != '') {
            listaAutorizantes.push({
                id: 0,
                idAsignacion: 0,
                autorizanteID: selAutSolicita3.data("id"),
                autorizanteNombre: selAutSolicita3.val(),
                autorizanteStatus: false,
                autorizanteFinal: false,
                orden: 3
            });
        }

        if (selAutSolicita4.data("id") != '') {
            listaAutorizantes.push({
                id: 0,
                idAsignacion: 0,
                autorizanteID: selAutSolicita4.data("id"),
                autorizanteNombre: selAutSolicita4.val(),
                autorizanteStatus: false,
                autorizanteFinal: false,
                orden: 4
            });
        }

        if (selAutSolicita5.data("id") != '') {
            listaAutorizantes.push({
                id: 0,
                idAsignacion: 0,
                autorizanteID: selAutSolicita5.data("id"),
                autorizanteNombre: selAutSolicita5.val(),
                autorizanteStatus: false,
                autorizanteFinal: true,
                orden: 5
            });
        }

        data.append('listaAutorizantes', JSON.stringify(listaAutorizantes));
        //#endregion

        return data;
    }

    function guardarAsignacion() {
        axios.post('/CatMaquina/GuardarAsignacionSolicitud', { idCuadro: _idCuadro, folio: inputAsignarSolicitud.val() })
            .then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    Alert2Exito('Se ha guardado la información.');
                    modalAsignarSolicitud.modal('hide');
                    bootGRenta();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
    }

    function mostrarModalCuadro() {
        let addCaracteristicas = 23;

        //#region Limpiar Valores
        txtObra.val('');
        txtNombreDelEquipo.val('');
        checkCompra.prop('checked', false);
        checkRenta.prop('checked', false);
        checkRoc.prop('checked', false);

        tblComparativoAdquisicionyRenta.find('input').val('');
        tblComparativoAdquisicionyRenta.find('textarea').val('');

        selAutSolicita1.val("");
        selAutSolicita1.data("id", "");
        selAutSolicita1.data("nombre", "");
        selAutSolicita2.val("");
        selAutSolicita2.data("id", "");
        selAutSolicita2.data("nombre", "");
        selAutSolicita3.val("");
        selAutSolicita3.data("id", "");
        selAutSolicita3.data("nombre", "");
        selAutSolicita4.val("");
        selAutSolicita4.data("id", "");
        selAutSolicita4.data("nombre", "");
        selAutSolicita5.val("");
        selAutSolicita5.data("id", "");
        selAutSolicita5.data("nombre", "");

        printTipoMoneda.val('');

        $('#imginputAgregarImagen1').attr('src', '/Content/img/ico/details_open.png');
        $('#imginputAgregarImagen1').css('width', '100%');
        $('#imginputAgregarImagen2').attr('src', '/Content/img/ico/details_open.png');
        $('#imginputAgregarImagen2').css('width', '100%');
        $('#imginputAgregarImagen3').attr('src', '/Content/img/ico/details_open.png');
        $('#imginputAgregarImagen3').css('width', '100%');
        $('#imginputAgregarImagen4').attr('src', '/Content/img/ico/details_open.png');
        $('#imginputAgregarImagen4').css('width', '100%');
        $('#imginputAgregarImagen5').attr('src', '/Content/img/ico/details_open.png');
        $('#imginputAgregarImagen5').css('width', '100%');
        $('#imginputAgregarImagen6').attr('src', '/Content/img/ico/details_open.png');
        $('#imginputAgregarImagen6').css('width', '100%');
        $('#imginputAgregarImagen7').attr('src', '/Content/img/ico/details_open.png');
        $('#imginputAgregarImagen7').css('width', '100%');
        //#endregion

        btnLimpiarFinanciero.css('visibility', 'visible');
        btnNuevoFinanciero.css('visibility', 'visible');
        btnAgregarColumnaFinanciero.css('visibility', 'visible');
        btnCerrarCuadroComparativoFinanciero.css('visibility', 'visible');

        dlgFormAdquisicionNuevoCuadro.dialog("open");
        dlgFormAdquisicionNuevoCuadro.css('min-height', '850px');
        dlgFormAdquisicionNuevoCuadro.css('height', '100%');

        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/CatMaquina/getTablaComparativoAdquisicion",
            data: { objFiltro: { idAsignacion: 0 } },
            success: function (response) {
                if (response.success) {
                    //#region establecerAdquisicion
                    //#region initDataTblM_AdquisicionyRenta
                    tblComparativoAdquisicionyRenta.DataTable({
                        retrieve: true,
                        paging: false,
                        searching: false,
                        language: dtDicEsp,
                        bInfo: false,
                        data: response.items,
                        order: [[3, "asc"], [1, "asc"]],
                        ordering: false,
                        columns: [
                            {
                                data: 'header'
                                , render: (data, type, row) => {
                                    if (data == 'Caracteristica1' || data == 'Caracteristica2' || data == 'Caracteristica3' || data == 'Caracteristica4' || data == 'Caracteristica5' || data == 'Caracteristica6' || data == 'Caracteristica7') {
                                        let html = '';
                                        html += '<input id="' + data + '" type="text" class="form-control " placeholder="">';
                                        return html;
                                    } else if (data == 'Caracteristicas del equipo') {
                                        let html = '';
                                        html += '<div id=' + data + '>' + data + '</div>';
                                        return html;

                                    } else {
                                        let html = '';
                                        if (data != null) {
                                            html += '<div id=' + data + '>' + data + '</div>';
                                        }
                                        return html;

                                    }
                                }
                            },
                            {
                                data: 'txtIdnumero1', render: (data, type, row) => {
                                    if (data == "idMarcaNum1Caracteristicas") {
                                        let html = '';
                                        html += '<button id="' + data + '" type="button" class="btn btn-primary" placeholder=""> Agregar Caracteristicas </button>';
                                        return html;
                                    } else if (data == 'inputAgregarImagen1') {
                                        let html = '';
                                        html += '<div class="image-upload">' +
                                            '<label for=' + data + ' class="' + data + '">' +
                                            '<center><img src="/Content/img/ico/details_open.png" id="img' + data + '"/>' +
                                            '</center></label>' +
                                            '<input id=' + data + ' class="' + data + '" type="file" style="display:none;" />' +
                                            ' </div>';
                                        return html;
                                    } else if (data == 'textareaComentarios1') {
                                        return `<textarea id="${data}" class="form-control" rows="5"></textarea>`;
                                    } else {

                                        let html = '';
                                        html += '<input id="' + data + '" type="text" class="form-control " placeholder="">';
                                        return html;
                                    }
                                }
                            },
                            {
                                data: 'txtIdnumero2', render: (data, type, row) => {

                                    if (data == "idMarcaNum2Caracteristicas") {
                                        let html = '';
                                        html += '';
                                        return html;
                                    } else if (data == 'inputAgregarImagen2') {
                                        let html = '';
                                        html += '<div class="image-upload">' +
                                            '<label for=' + data + '  class="' + data + '">' +
                                            '<center><img src="/Content/img/ico/details_open.png" id="img' + data + '"/>' +
                                            '</center></label>' +
                                            '<input id=' + data + ' class="' + data + '" type="file" style="display:none;" />' +
                                            ' </div>';
                                        return html;
                                    } else if (data == 'textareaComentarios2') {
                                        return `<textarea id="${data}" class="form-control" rows="5"></textarea>`;
                                    } else {
                                        let html = '';
                                        html += '<input id="' + data + '" type="text" class="form-control" placeholder="" >';
                                        return html;
                                    }
                                }
                            },
                            {
                                data: 'txtIdnumero3', render: (data, type, row) => {
                                    if (data == "idMarcaNum3Caracteristicas") {
                                        let html = '';
                                        html += '';
                                        return html;
                                    } else if (data == 'inputAgregarImagen3') {
                                        let html = '';
                                        html += '<div class="image-upload">' +
                                            '<label for=' + data + ' class="' + data + '">' +
                                            '<center><img src="/Content/img/ico/details_open.png" id="img' + data + '"/>' +
                                            '</center></label>' +
                                            '<input id=' + data + ' class="' + data + '" type="file" style="display:none;" />' +
                                            ' </div>';
                                        return html;
                                    } else if (data == 'textareaComentarios3') {
                                        return `<textarea id="${data}" class="form-control" rows="5"></textarea>`;
                                    } else {
                                        let html = '';
                                        html += '<input id="' + data + '" type="text" class="form-control" placeholder="" >';
                                        return html;
                                    }
                                }
                            },
                            {
                                data: 'txtIdnumero4'
                                , render: (data, type, row) => {
                                    if (data == "idMarcaNum4Caracteristicas") {
                                        let html = '';
                                        html += '';
                                        return html;
                                    } else if (data == 'inputAgregarImagen4') {
                                        let html = '';
                                        html += '<div class="image-upload">' +
                                            '<label for=' + data + ' class="' + data + '">' +
                                            '<center><img src="/Content/img/ico/details_open.png" id="img' + data + '"/>' +
                                            '</center></label>' +
                                            '<input id=' + data + ' class="' + data + '" type="file" style="display:none;" />' +
                                            ' </div>';
                                        return html;
                                    } else if (data == 'textareaComentarios4') {
                                        return `<textarea id="${data}" class="form-control" rows="5"></textarea>`;
                                    } else {
                                        let html = '';
                                        html += '<input id="' + data + '" type="text" class="form-control" placeholder="" >';
                                        return html;
                                    }
                                }
                            },
                            {
                                data: 'txtIdnumero5'
                                , render: (data, type, row) => {
                                    if (data == "idMarcaNum5Caracteristicas") {
                                        let html = '';
                                        html += '';
                                        return html;
                                    } else if (data == 'inputAgregarImagen5') {
                                        let html = '';
                                        html += '<div class="image-upload">' +
                                            '<label for=' + data + ' class="' + data + '">' +
                                            '<center><img src="/Content/img/ico/details_open.png" id="img' + data + '"/>' +
                                            '</center></label>' +
                                            '<input id=' + data + ' class="' + data + '" type="file" style="display:none;" />' +
                                            ' </div>';
                                        return html;
                                    } else if (data == 'textareaComentarios5') {
                                        return `<textarea id="${data}" class="form-control" rows="5"></textarea>`;
                                    } else {
                                        let html = '';
                                        html += '<input id="' + data + '" type="text" class="form-control" placeholder="">';
                                        return html;
                                    }
                                }
                            },
                            {
                                data: 'txtIdnumero6'
                                , render: (data, type, row) => {
                                    if (data == "idMarcaNum6Caracteristicas") {
                                        let html = '';
                                        html += '';
                                        return html;
                                    } else if (data == 'inputAgregarImagen6') {
                                        let html = '';
                                        html += '<div class="image-upload">' +
                                            '<label for=' + data + ' class="' + data + '">' +
                                            '<center><img src="/Content/img/ico/details_open.png" id="img' + data + '"/>' +
                                            '</center></label>' +
                                            '<input id=' + data + ' class="' + data + '" type="file" style="display:none;" />' +
                                            ' </div>';
                                        return html;
                                    } else if (data == 'textareaComentarios6') {
                                        return `<textarea id="${data}" class="form-control" rows="5"></textarea>`;
                                    } else {
                                        let html = '';
                                        html += '<input id="' + data + '" type="text" class="form-control" placeholder="">';
                                        return html;
                                    }
                                }
                            },
                            {
                                data: 'txtIdnumero7'
                                , render: (data, type, row) => {
                                    if (data == "idMarcaNum7Caracteristicas") {
                                        let html = '';
                                        html += '';
                                        return html;
                                    } else if (data == 'inputAgregarImagen7') {
                                        let html = '';
                                        html += '<div class="image-upload">' +
                                            '<label for=' + data + ' class="' + data + '">' +
                                            '<center><img src="/Content/img/ico/details_open.png" id="img' + data + '"/>' +
                                            '</center></label>' +
                                            '<input id=' + data + ' class="' + data + '" type="file" style="display:none;" />' +
                                            ' </div>';
                                        return html;
                                    } else if (data == 'textareaComentarios7') {
                                        return `<textarea id="${data}" class="form-control" rows="5"></textarea>`;
                                    } else {
                                        let html = '';
                                        html += '<input id="' + data + '" type="text" class="form-control" placeholder="">';
                                        return html;
                                    }
                                }
                            },
                        ],
                        // scrollX: true,
                        // scrollY: false,
                        // scrollCollapse: true,
                        columnDefs: [
                            { className: "dt-head-left", "targets": "_all" },
                            { className: "dt-body-left", "targets": "_all" }
                        ],
                        drawCallback: function (settings) {
                            $('.inputAgregarImagen1').on("change", function (e) {
                                var fileInput = document.getElementById('inputAgregarImagen1');

                                if (fileInput.files[0]) {
                                    var reader = new FileReader();
                                    reader.readAsDataURL(fileInput.files[0]);
                                    if (document.getElementById('inputAgregarImagen1').files[0].type == "image/jpeg") {
                                        reader.onload = function () {
                                            $('#imginputAgregarImagen1').attr('src', reader.result)
                                            $('#imginputAgregarImagen1').css('width', '30%');
                                        };
                                    } else {
                                        $('#imginputAgregarImagen1').attr('src', '/Content/img/ico/details_open.png')
                                        $('#imginputAgregarImagen1').css('width', '100%');
                                    }
                                } else {
                                    $('#imginputAgregarImagen1').attr('src', '/Content/img/ico/details_open.png')
                                    $('#imginputAgregarImagen1').css('width', '100%');
                                }
                            });
                            $('.inputAgregarImagen2').on("change", function (e) {
                                var fileInput = document.getElementById('inputAgregarImagen2');

                                if (fileInput.files[0]) {
                                    var reader = new FileReader();
                                    reader.readAsDataURL(fileInput.files[0]);
                                    if (document.getElementById('inputAgregarImagen2').files[0].type == "image/jpeg") {
                                        reader.onload = function () {
                                            $('#imginputAgregarImagen2').attr('src', reader.result)
                                            $('#imginputAgregarImagen2').css('width', '30%');
                                        };
                                    } else {
                                        $('#imginputAgregarImagen2').attr('src', '/Content/img/ico/details_open.png')
                                        $('#imginputAgregarImagen2').css('width', '100%');
                                    }
                                } else {
                                    $('#imginputAgregarImagen2').attr('src', '/Content/img/ico/details_open.png')
                                    $('#imginputAgregarImagen2').css('width', '100%');
                                }
                            });
                            $('.inputAgregarImagen3').on("change", function (e) {
                                var fileInput = document.getElementById('inputAgregarImagen3');

                                if (fileInput.files[0]) {
                                    var reader = new FileReader();
                                    reader.readAsDataURL(fileInput.files[0]);
                                    if (document.getElementById('inputAgregarImagen3').files[0].type == "image/jpeg") {
                                        reader.onload = function () {
                                            $('#imginputAgregarImagen3').attr('src', reader.result)
                                            $('#imginputAgregarImagen3').css('width', '30%');
                                        };
                                    } else {
                                        $('#imginputAgregarImagen3').attr('src', '/Content/img/ico/details_open.png')
                                        $('#imginputAgregarImagen3').css('width', '100%');
                                    }
                                } else {
                                    $('#imginputAgregarImagen3').attr('src', '/Content/img/ico/details_open.png')
                                    $('#imginputAgregarImagen3').css('width', '100%');
                                }
                            });
                            $('.inputAgregarImagen4').on("change", function (e) {
                                var fileInput = document.getElementById('inputAgregarImagen4');

                                if (fileInput.files[0]) {
                                    var reader = new FileReader();
                                    reader.readAsDataURL(fileInput.files[0]);
                                    if (document.getElementById('inputAgregarImagen4').files[0].type == "image/jpeg") {
                                        reader.onload = function () {
                                            $('#imginputAgregarImagen4').attr('src', reader.result)
                                            $('#imginputAgregarImagen4').css('width', '30%');
                                        };
                                    } else {
                                        $('#imginputAgregarImagen4').attr('src', '/Content/img/ico/details_open.png')
                                        $('#imginputAgregarImagen4').css('width', '100%');
                                    }
                                } else {
                                    $('#imginputAgregarImagen4').attr('src', '/Content/img/ico/details_open.png')
                                    $('#imginputAgregarImagen4').css('width', '100%');
                                }
                            });
                            $('.inputAgregarImagen5').on("change", function (e) {
                                var fileInput = document.getElementById('inputAgregarImagen5');

                                if (fileInput.files[0]) {
                                    var reader = new FileReader();
                                    reader.readAsDataURL(fileInput.files[0]);
                                    if (document.getElementById('inputAgregarImagen5').files[0].type == "image/jpeg") {
                                        reader.onload = function () {
                                            $('#imginputAgregarImagen5').attr('src', reader.result)
                                            $('#imginputAgregarImagen5').css('width', '30%');
                                        };
                                    } else {
                                        $('#imginputAgregarImagen5').attr('src', '/Content/img/ico/details_open.png')
                                        $('#imginputAgregarImagen5').css('width', '100%');
                                    }
                                } else {
                                    $('#imginputAgregarImagen5').attr('src', '/Content/img/ico/details_open.png')
                                    $('#imginputAgregarImagen5').css('width', '100%');
                                }
                            });
                            $('.inputAgregarImagen6').on("change", function (e) {
                                var fileInput = document.getElementById('inputAgregarImagen6');

                                if (fileInput.files[0]) {
                                    var reader = new FileReader();
                                    reader.readAsDataURL(fileInput.files[0]);
                                    if (document.getElementById('inputAgregarImagen6').files[0].type == "image/jpeg") {
                                        reader.onload = function () {
                                            $('#imginputAgregarImagen6').attr('src', reader.result)
                                            $('#imginputAgregarImagen6').css('width', '30%');
                                        };
                                    } else {
                                        $('#imginputAgregarImagen6').attr('src', '/Content/img/ico/details_open.png')
                                        $('#imginputAgregarImagen6').css('width', '100%');
                                    }
                                } else {
                                    $('#imginputAgregarImagen6').attr('src', '/Content/img/ico/details_open.png')
                                    $('#imginputAgregarImagen6').css('width', '100%');
                                }
                            });
                            $('.inputAgregarImagen7').on("change", function (e) {
                                var fileInput = document.getElementById('inputAgregarImagen7');

                                if (fileInput.files[0]) {
                                    var reader = new FileReader();
                                    reader.readAsDataURL(fileInput.files[0]);
                                    if (document.getElementById('inputAgregarImagen7').files[0].type == "image/jpeg") {
                                        reader.onload = function () {
                                            $('#imginputAgregarImagen7').attr('src', reader.result)
                                            $('#imginputAgregarImagen7').css('width', '30%');
                                        };
                                    } else {
                                        $('#imginputAgregarImagen7').attr('src', '/Content/img/ico/details_open.png')
                                        $('#imginputAgregarImagen7').css('width', '100%');
                                    }
                                } else {
                                    $('#imginputAgregarImagen7').attr('src', '/Content/img/ico/details_open.png')
                                    $('#imginputAgregarImagen7').css('width', '100%');
                                }
                            });
                        },
                        initComplete: function () {
                            tblComparativoAdquisicionyRenta.on('click', '#idMarcaNum1Caracteristicas', function () {
                                let primerCoincidenciaRenglonOculto = tblComparativoAdquisicionyRenta.find('tr').filter(function () { return this.style.visibility == 'hidden' })[0];

                                if (primerCoincidenciaRenglonOculto) {
                                    $(primerCoincidenciaRenglonOculto).css('visibility', 'visible');
                                }
                            });
                        }
                    });
                    //#endregion
                    //#endregion

                    //#region acomadorInput
                    $('#idMarcaNum1Marca').css('width', '210px');
                    $('#idMarcaNum2Marca').css('width', '210px');
                    $('#idMarcaNum3Marca').css('width', '210px');
                    $('#idMarcaNum4Marca').css('width', '210px');
                    $('#idMarcaNum5Marca').css('width', '210px');
                    $('#idMarcaNum6Marca').css('width', '210px');
                    $('#idMarcaNum7Marca').css('width', '210px');
                    $('.dataTables_scrollHead').css('display', 'none');
                    tblComparativoAdquisicionyRenta.find('td').css('padding', 0);

                    let tabla = tblComparativoAdquisicionyRenta.find("tr");

                    for (let i = 0; i < tabla.length; i++) {
                        if (i >= addCaracteristicas) {
                            $(tabla[i]).css('visibility', 'hidden');
                        }
                    }

                    for (let i = 0; i < 8; i++) {
                        if (i > 1) {
                            tblComparativoAdquisicionyRenta.DataTable().columns([i]).visible(false);
                        }
                    }

                    for (let index = 0; index < tabla.length; index++) {
                        $($(tabla[index]).find('td')[0]).css('font-weight', 'bold')
                    }

                    // $('#idMarcaNum1Caracteristicas').on("click", function (e) {
                    //     let primerCoincidenciaRenglonOculto = tblComparativoAdquisicionyRenta.find('tr').filter(function () { return this.style.visibility == 'hidden' })[0];

                    //     if (primerCoincidenciaRenglonOculto) {
                    //         $(primerCoincidenciaRenglonOculto).css('visibility', 'visible');
                    //     }

                    //     // if (addCaracteristicas <= tabla.length) {
                    //     //     for (let i = 0; i < tabla.length; i++) {
                    //     //         if (i == addCaracteristicas) {
                    //     //             $(tabla[i]).css('visibility', 'visible');
                    //     //         }
                    //     //     }

                    //     //     addCaracteristicas++;
                    //     // }
                    // });

                    tblComparativoAdquisicionyRenta.css('margin', '0');
                    tblComparativoAdquisicionyRenta.find('input').prop('disabled', false);
                    //#endregion
                } else {
                    Alert2Error(response.message);
                }
            },
            error: function () {
                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            },
            complete: function () {
                $.unblockUI();
            }
        });

        selAutSolicita1.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
        selAutSolicita2.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
        selAutSolicita3.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
        selAutSolicita4.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
        selAutSolicita5.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
    }

    function mostrarModalCuadroEditar() {
        let addCaracteristicas = 23;

        //#region Limpiar Valores
        txtObra.val('');
        txtNombreDelEquipo.val('');
        checkCompra.prop('checked', false);
        checkRenta.prop('checked', false);
        checkRoc.prop('checked', false);

        tblComparativoAdquisicionyRenta.find('input').val('');
        tblComparativoAdquisicionyRenta.find('textarea').val('');

        selAutSolicita1.val("");
        selAutSolicita1.data("id", "");
        selAutSolicita1.data("nombre", "");
        selAutSolicita2.val("");
        selAutSolicita2.data("id", "");
        selAutSolicita2.data("nombre", "");
        selAutSolicita3.val("");
        selAutSolicita3.data("id", "");
        selAutSolicita3.data("nombre", "");
        selAutSolicita4.val("");
        selAutSolicita4.data("id", "");
        selAutSolicita4.data("nombre", "");
        selAutSolicita5.val("");
        selAutSolicita5.data("id", "");
        selAutSolicita5.data("nombre", "");

        printTipoMoneda.val('');

        $('#imginputAgregarImagen1').attr('src', '/Content/img/ico/details_open.png');
        $('#imginputAgregarImagen1').css('width', '100%');
        $('#imginputAgregarImagen2').attr('src', '/Content/img/ico/details_open.png');
        $('#imginputAgregarImagen2').css('width', '100%');
        $('#imginputAgregarImagen3').attr('src', '/Content/img/ico/details_open.png');
        $('#imginputAgregarImagen3').css('width', '100%');
        $('#imginputAgregarImagen4').attr('src', '/Content/img/ico/details_open.png');
        $('#imginputAgregarImagen4').css('width', '100%');
        $('#imginputAgregarImagen5').attr('src', '/Content/img/ico/details_open.png');
        $('#imginputAgregarImagen5').css('width', '100%');
        $('#imginputAgregarImagen6').attr('src', '/Content/img/ico/details_open.png');
        $('#imginputAgregarImagen6').css('width', '100%');
        $('#imginputAgregarImagen7').attr('src', '/Content/img/ico/details_open.png');
        $('#imginputAgregarImagen7').css('width', '100%');
        //#endregion

        btnLimpiarFinanciero.css('visibility', 'visible');
        btnNuevoFinanciero.css('visibility', 'visible');
        btnAgregarColumnaFinanciero.css('visibility', 'visible');
        btnCerrarCuadroComparativoFinanciero.css('visibility', 'visible');

        dlgFormAdquisicionNuevoCuadro.dialog("open");
        dlgFormAdquisicionNuevoCuadro.css('min-height', '850px');
        dlgFormAdquisicionNuevoCuadro.css('height', '100%');

        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/CatMaquina/getTablaComparativoAdquisicion",
            data: { objFiltro: { idAsignacion: 0 } },
            success: function (response) {
                if (response.success) {
                    //#region establecerAdquisicion
                    //#region initDataTblM_AdquisicionyRenta
                    tblComparativoAdquisicionyRenta.DataTable({
                        retrieve: true,
                        paging: false,
                        searching: false,
                        language: dtDicEsp,
                        bInfo: false,
                        data: response.items,
                        order: [[3, "asc"], [1, "asc"]],
                        ordering: false,
                        columns: [
                            {
                                data: 'header'
                                , render: (data, type, row) => {
                                    if (data == 'Caracteristica1' || data == 'Caracteristica2' || data == 'Caracteristica3' || data == 'Caracteristica4' || data == 'Caracteristica5' || data == 'Caracteristica6' || data == 'Caracteristica7') {
                                        let html = '';
                                        html += '<input id="' + data + '" type="text" class="form-control " placeholder="">';
                                        return html;
                                    } else if (data == 'Caracteristicas del equipo') {
                                        let html = '';
                                        html += '<div id=' + data + '>' + data + '</div>';
                                        return html;

                                    } else {
                                        let html = '';
                                        if (data != null) {
                                            html += '<div id=' + data + '>' + data + '</div>';
                                        }
                                        return html;

                                    }
                                }
                            },
                            {
                                data: 'txtIdnumero1', render: (data, type, row) => {
                                    if (data == "idMarcaNum1Caracteristicas") {
                                        let html = '';
                                        html += '<button id="' + data + '" type="button" class="btn btn-primary" placeholder=""> Agregar Caracteristicas </button>';
                                        return html;
                                    } else if (data == 'inputAgregarImagen1') {
                                        let html = '';
                                        html += '<div class="image-upload">' +
                                            '<label for=' + data + ' class="' + data + '">' +
                                            '<center><img src="/Content/img/ico/details_open.png" id="img' + data + '"/>' +
                                            '</center></label>' +
                                            '<input id=' + data + ' class="' + data + '" type="file" style="display:none;" />' +
                                            ' </div>';
                                        return html;
                                    } else if (data == 'textareaComentarios1') {
                                        return `<textarea id="${data}" class="form-control" rows="5"></textarea>`;
                                    } else {

                                        let html = '';
                                        html += '<input id="' + data + '" type="text" class="form-control " placeholder="">';
                                        return html;
                                    }
                                }
                            },
                            {
                                data: 'txtIdnumero2', render: (data, type, row) => {

                                    if (data == "idMarcaNum2Caracteristicas") {
                                        let html = '';
                                        html += '';
                                        return html;
                                    } else if (data == 'inputAgregarImagen2') {
                                        let html = '';
                                        html += '<div class="image-upload">' +
                                            '<label for=' + data + '  class="' + data + '">' +
                                            '<center><img src="/Content/img/ico/details_open.png" id="img' + data + '"/>' +
                                            '</center></label>' +
                                            '<input id=' + data + ' class="' + data + '" type="file" style="display:none;" />' +
                                            ' </div>';
                                        return html;
                                    } else if (data == 'textareaComentarios2') {
                                        return `<textarea id="${data}" class="form-control" rows="5"></textarea>`;
                                    } else {
                                        let html = '';
                                        html += '<input id="' + data + '" type="text" class="form-control" placeholder="" >';
                                        return html;
                                    }
                                }
                            },
                            {
                                data: 'txtIdnumero3', render: (data, type, row) => {
                                    if (data == "idMarcaNum3Caracteristicas") {
                                        let html = '';
                                        html += '';
                                        return html;
                                    } else if (data == 'inputAgregarImagen3') {
                                        let html = '';
                                        html += '<div class="image-upload">' +
                                            '<label for=' + data + ' class="' + data + '">' +
                                            '<center><img src="/Content/img/ico/details_open.png" id="img' + data + '"/>' +
                                            '</center></label>' +
                                            '<input id=' + data + ' class="' + data + '" type="file" style="display:none;" />' +
                                            ' </div>';
                                        return html;
                                    } else if (data == 'textareaComentarios3') {
                                        return `<textarea id="${data}" class="form-control" rows="5"></textarea>`;
                                    } else {
                                        let html = '';
                                        html += '<input id="' + data + '" type="text" class="form-control" placeholder="" >';
                                        return html;
                                    }
                                }
                            },
                            {
                                data: 'txtIdnumero4'
                                , render: (data, type, row) => {
                                    if (data == "idMarcaNum4Caracteristicas") {
                                        let html = '';
                                        html += '';
                                        return html;
                                    } else if (data == 'inputAgregarImagen4') {
                                        let html = '';
                                        html += '<div class="image-upload">' +
                                            '<label for=' + data + ' class="' + data + '">' +
                                            '<center><img src="/Content/img/ico/details_open.png" id="img' + data + '"/>' +
                                            '</center></label>' +
                                            '<input id=' + data + ' class="' + data + '" type="file" style="display:none;" />' +
                                            ' </div>';
                                        return html;
                                    } else if (data == 'textareaComentarios4') {
                                        return `<textarea id="${data}" class="form-control" rows="5"></textarea>`;
                                    } else {
                                        let html = '';
                                        html += '<input id="' + data + '" type="text" class="form-control" placeholder="" >';
                                        return html;
                                    }
                                }
                            },
                            {
                                data: 'txtIdnumero5'
                                , render: (data, type, row) => {
                                    if (data == "idMarcaNum5Caracteristicas") {
                                        let html = '';
                                        html += '';
                                        return html;
                                    } else if (data == 'inputAgregarImagen5') {
                                        let html = '';
                                        html += '<div class="image-upload">' +
                                            '<label for=' + data + ' class="' + data + '">' +
                                            '<center><img src="/Content/img/ico/details_open.png" id="img' + data + '"/>' +
                                            '</center></label>' +
                                            '<input id=' + data + ' class="' + data + '" type="file" style="display:none;" />' +
                                            ' </div>';
                                        return html;
                                    } else if (data == 'textareaComentarios5') {
                                        return `<textarea id="${data}" class="form-control" rows="5"></textarea>`;
                                    } else {
                                        let html = '';
                                        html += '<input id="' + data + '" type="text" class="form-control" placeholder="">';
                                        return html;
                                    }
                                }
                            },
                            {
                                data: 'txtIdnumero6'
                                , render: (data, type, row) => {
                                    if (data == "idMarcaNum6Caracteristicas") {
                                        let html = '';
                                        html += '';
                                        return html;
                                    } else if (data == 'inputAgregarImagen6') {
                                        let html = '';
                                        html += '<div class="image-upload">' +
                                            '<label for=' + data + ' class="' + data + '">' +
                                            '<center><img src="/Content/img/ico/details_open.png" id="img' + data + '"/>' +
                                            '</center></label>' +
                                            '<input id=' + data + ' class="' + data + '" type="file" style="display:none;" />' +
                                            ' </div>';
                                        return html;
                                    } else if (data == 'textareaComentarios6') {
                                        return `<textarea id="${data}" class="form-control" rows="5"></textarea>`;
                                    } else {
                                        let html = '';
                                        html += '<input id="' + data + '" type="text" class="form-control" placeholder="">';
                                        return html;
                                    }
                                }
                            },
                            {
                                data: 'txtIdnumero7'
                                , render: (data, type, row) => {
                                    if (data == "idMarcaNum7Caracteristicas") {
                                        let html = '';
                                        html += '';
                                        return html;
                                    } else if (data == 'inputAgregarImagen7') {
                                        let html = '';
                                        html += '<div class="image-upload">' +
                                            '<label for=' + data + ' class="' + data + '">' +
                                            '<center><img src="/Content/img/ico/details_open.png" id="img' + data + '"/>' +
                                            '</center></label>' +
                                            '<input id=' + data + ' class="' + data + '" type="file" style="display:none;" />' +
                                            ' </div>';
                                        return html;
                                    } else if (data == 'textareaComentarios7') {
                                        return `<textarea id="${data}" class="form-control" rows="5"></textarea>`;
                                    } else {
                                        let html = '';
                                        html += '<input id="' + data + '" type="text" class="form-control" placeholder="">';
                                        return html;
                                    }
                                }
                            },
                        ],
                        // scrollX: true,
                        // scrollY: false,
                        // scrollCollapse: true,
                        columnDefs: [
                            { className: "dt-head-left", "targets": "_all" },
                            { className: "dt-body-left", "targets": "_all" }
                        ],
                        drawCallback: function (settings) {
                            $('.inputAgregarImagen1').on("change", function (e) {
                                var fileInput = document.getElementById('inputAgregarImagen1');

                                if (fileInput.files[0]) {
                                    var reader = new FileReader();
                                    reader.readAsDataURL(fileInput.files[0]);
                                    if (document.getElementById('inputAgregarImagen1').files[0].type == "image/jpeg") {
                                        reader.onload = function () {
                                            $('#imginputAgregarImagen1').attr('src', reader.result)
                                            $('#imginputAgregarImagen1').css('width', '30%');
                                        };
                                    } else {
                                        $('#imginputAgregarImagen1').attr('src', '/Content/img/ico/details_open.png')
                                        $('#imginputAgregarImagen1').css('width', '100%');
                                    }
                                } else {
                                    $('#imginputAgregarImagen1').attr('src', '/Content/img/ico/details_open.png')
                                    $('#imginputAgregarImagen1').css('width', '100%');
                                }
                            });
                            $('.inputAgregarImagen2').on("change", function (e) {
                                var fileInput = document.getElementById('inputAgregarImagen2');

                                if (fileInput.files[0]) {
                                    var reader = new FileReader();
                                    reader.readAsDataURL(fileInput.files[0]);
                                    if (document.getElementById('inputAgregarImagen2').files[0].type == "image/jpeg") {
                                        reader.onload = function () {
                                            $('#imginputAgregarImagen2').attr('src', reader.result)
                                            $('#imginputAgregarImagen2').css('width', '30%');
                                        };
                                    } else {
                                        $('#imginputAgregarImagen2').attr('src', '/Content/img/ico/details_open.png')
                                        $('#imginputAgregarImagen2').css('width', '100%');
                                    }
                                } else {
                                    $('#imginputAgregarImagen2').attr('src', '/Content/img/ico/details_open.png')
                                    $('#imginputAgregarImagen2').css('width', '100%');
                                }
                            });
                            $('.inputAgregarImagen3').on("change", function (e) {
                                var fileInput = document.getElementById('inputAgregarImagen3');

                                if (fileInput.files[0]) {
                                    var reader = new FileReader();
                                    reader.readAsDataURL(fileInput.files[0]);
                                    if (document.getElementById('inputAgregarImagen3').files[0].type == "image/jpeg") {
                                        reader.onload = function () {
                                            $('#imginputAgregarImagen3').attr('src', reader.result)
                                            $('#imginputAgregarImagen3').css('width', '30%');
                                        };
                                    } else {
                                        $('#imginputAgregarImagen3').attr('src', '/Content/img/ico/details_open.png')
                                        $('#imginputAgregarImagen3').css('width', '100%');
                                    }
                                } else {
                                    $('#imginputAgregarImagen3').attr('src', '/Content/img/ico/details_open.png')
                                    $('#imginputAgregarImagen3').css('width', '100%');
                                }
                            });
                            $('.inputAgregarImagen4').on("change", function (e) {
                                var fileInput = document.getElementById('inputAgregarImagen4');

                                if (fileInput.files[0]) {
                                    var reader = new FileReader();
                                    reader.readAsDataURL(fileInput.files[0]);
                                    if (document.getElementById('inputAgregarImagen4').files[0].type == "image/jpeg") {
                                        reader.onload = function () {
                                            $('#imginputAgregarImagen4').attr('src', reader.result)
                                            $('#imginputAgregarImagen4').css('width', '30%');
                                        };
                                    } else {
                                        $('#imginputAgregarImagen4').attr('src', '/Content/img/ico/details_open.png')
                                        $('#imginputAgregarImagen4').css('width', '100%');
                                    }
                                } else {
                                    $('#imginputAgregarImagen4').attr('src', '/Content/img/ico/details_open.png')
                                    $('#imginputAgregarImagen4').css('width', '100%');
                                }
                            });
                            $('.inputAgregarImagen5').on("change", function (e) {
                                var fileInput = document.getElementById('inputAgregarImagen5');

                                if (fileInput.files[0]) {
                                    var reader = new FileReader();
                                    reader.readAsDataURL(fileInput.files[0]);
                                    if (document.getElementById('inputAgregarImagen5').files[0].type == "image/jpeg") {
                                        reader.onload = function () {
                                            $('#imginputAgregarImagen5').attr('src', reader.result)
                                            $('#imginputAgregarImagen5').css('width', '30%');
                                        };
                                    } else {
                                        $('#imginputAgregarImagen5').attr('src', '/Content/img/ico/details_open.png')
                                        $('#imginputAgregarImagen5').css('width', '100%');
                                    }
                                } else {
                                    $('#imginputAgregarImagen5').attr('src', '/Content/img/ico/details_open.png')
                                    $('#imginputAgregarImagen5').css('width', '100%');
                                }
                            });
                            $('.inputAgregarImagen6').on("change", function (e) {
                                var fileInput = document.getElementById('inputAgregarImagen6');

                                if (fileInput.files[0]) {
                                    var reader = new FileReader();
                                    reader.readAsDataURL(fileInput.files[0]);
                                    if (document.getElementById('inputAgregarImagen6').files[0].type == "image/jpeg") {
                                        reader.onload = function () {
                                            $('#imginputAgregarImagen6').attr('src', reader.result)
                                            $('#imginputAgregarImagen6').css('width', '30%');
                                        };
                                    } else {
                                        $('#imginputAgregarImagen6').attr('src', '/Content/img/ico/details_open.png')
                                        $('#imginputAgregarImagen6').css('width', '100%');
                                    }
                                } else {
                                    $('#imginputAgregarImagen6').attr('src', '/Content/img/ico/details_open.png')
                                    $('#imginputAgregarImagen6').css('width', '100%');
                                }
                            });
                            $('.inputAgregarImagen7').on("change", function (e) {
                                var fileInput = document.getElementById('inputAgregarImagen7');

                                if (fileInput.files[0]) {
                                    var reader = new FileReader();
                                    reader.readAsDataURL(fileInput.files[0]);
                                    if (document.getElementById('inputAgregarImagen7').files[0].type == "image/jpeg") {
                                        reader.onload = function () {
                                            $('#imginputAgregarImagen7').attr('src', reader.result)
                                            $('#imginputAgregarImagen7').css('width', '30%');
                                        };
                                    } else {
                                        $('#imginputAgregarImagen7').attr('src', '/Content/img/ico/details_open.png')
                                        $('#imginputAgregarImagen7').css('width', '100%');
                                    }
                                } else {
                                    $('#imginputAgregarImagen7').attr('src', '/Content/img/ico/details_open.png')
                                    $('#imginputAgregarImagen7').css('width', '100%');
                                }
                            });
                        },
                        initComplete: function () {
                            tblComparativoAdquisicionyRenta.on('click', '#idMarcaNum1Caracteristicas', function () {
                                let primerCoincidenciaRenglonOculto = tblComparativoAdquisicionyRenta.find('tr').filter(function () { return this.style.visibility == 'hidden' })[0];

                                if (primerCoincidenciaRenglonOculto) {
                                    $(primerCoincidenciaRenglonOculto).css('visibility', 'visible');
                                }
                            });
                        }
                    });
                    //#endregion
                    //#endregion

                    //#region acomadorInput
                    $('#idMarcaNum1Marca').css('width', '210px');
                    $('#idMarcaNum2Marca').css('width', '210px');
                    $('#idMarcaNum3Marca').css('width', '210px');
                    $('#idMarcaNum4Marca').css('width', '210px');
                    $('#idMarcaNum5Marca').css('width', '210px');
                    $('#idMarcaNum6Marca').css('width', '210px');
                    $('#idMarcaNum7Marca').css('width', '210px');
                    $('.dataTables_scrollHead').css('display', 'none');
                    tblComparativoAdquisicionyRenta.find('td').css('padding', 0);

                    let tabla = tblComparativoAdquisicionyRenta.find("tr");

                    for (let i = 0; i < tabla.length; i++) {
                        if (i >= addCaracteristicas) {
                            $(tabla[i]).css('visibility', 'hidden');
                        }
                    }

                    for (let i = 0; i < 8; i++) {
                        if (i > 1) {
                            tblComparativoAdquisicionyRenta.DataTable().columns([i]).visible(false);
                        }
                    }

                    for (let index = 0; index < tabla.length; index++) {
                        $($(tabla[index]).find('td')[0]).css('font-weight', 'bold')
                    }

                    // $('#idMarcaNum1Caracteristicas').on("click", function (e) {
                    //     let primerCoincidenciaRenglonOculto = tblComparativoAdquisicionyRenta.find('tr').filter(function () { return this.style.visibility == 'hidden' })[0];

                    //     if (primerCoincidenciaRenglonOculto) {
                    //         $(primerCoincidenciaRenglonOculto).css('visibility', 'visible');
                    //     }

                    //     // if (addCaracteristicas <= tabla.length) {
                    //     //     for (let i = 0; i < tabla.length; i++) {
                    //     //         if (i == addCaracteristicas) {
                    //     //             $(tabla[i]).css('visibility', 'visible');
                    //     //         }
                    //     //     }

                    //     //     addCaracteristicas++;
                    //     // }
                    // });

                    tblComparativoAdquisicionyRenta.css('margin', '0');
                    tblComparativoAdquisicionyRenta.find('input').prop('disabled', false);
                    //#endregion

                    //#region Cargar Información del Cuadro
                    axios.post('/CatMaquina/ObtenerInformacionCuadro', { idCuadro: btnNuevo.data().id })
                        .then(response => {
                            let { success, datos, message } = response.data;

                            if (success) {
                                llenarModalCuadro(response.data);
                            } else {
                                AlertaGeneral(`Alerta`, message);
                            }
                        }).catch(error => AlertaGeneral(`Alerta`, error.message));
                    //#endregion
                } else {
                    Alert2Error(response.message);
                }
            },
            error: function () {
                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            },
            complete: function () {
                $.unblockUI();
            }
        });

        selAutSolicita1.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
        selAutSolicita2.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
        selAutSolicita3.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
        selAutSolicita4.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
        selAutSolicita5.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
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

    function llenarModalCuadro(data) {
        //#region Información General
        txtObra.val(data.cuadro.obra);
        txtNombreDelEquipo.val(data.cuadro.nombreDelEquipo);
        checkCompra.prop('checked', data.cuadro.compra);
        checkRenta.prop('checked', data.cuadro.renta);
        checkRoc.prop('checked', data.cuadro.roc);
        printTipoMoneda.val(data.cuadro.tipoMoneda);
        //#endregion

        //#region Información del Detalle
        for (let i = 1; i < data.listaDetalle.length; i++) {
            btnAgregarColumna.click(); //Se muestran las columnas dependiendo de la cantidad de detalles obtenidos.
        }

        for (let i = 1; i < 8; i++) {
            let caracteristicaNombre = data.listaDetalle[0][`caracteristicasDelEquipo${i}`];

            if (caracteristicaNombre) {
                $('#idMarcaNum1Caracteristicas').click(); //Se muestran los renglones de las características dependiendo de la cantidad guardada que sean diferentes de string vacío y nulos.
                $(`#Caracteristica${i}`).val(caracteristicaNombre);
            }
        }

        for (let i = 0; i < data.listaDetalle.length; i++) {
            let det = data.listaDetalle[i];

            $(`#idMarcaNum${i + 1}Marca`).val(det.marcaModelo);
            $(`#idMarcaNum${i + 1}proveedor`).val(det.proveedor);
            $(`#idMarcaNum${i + 1}precio`).val(det.precioDeVenta);
            $(`#idMarcaNum${i + 1}Trade`).val(det.tradeIn);
            $(`#idMarcaNum${i + 1}Valores`).val(det.valoresDeRecompra);
            $(`#idMarcaNum${i + 1}Precio`).val(det.precioDeRentaPura);
            $(`#idMarcaNum${i + 1}PrecioRoc`).val(det.precioDeRentaEnRoc);
            $(`#idMarcaNum${i + 1}BaseHoras`).val(det.baseHoras);
            $(`#idMarcaNum${i + 1}Tiempo`).val(det.tiempoDeEntrega);
            $(`#idMarcaNum${i + 1}Ubicacion`).val(det.ubicacion);
            $(`#idMarcaNum${i + 1}Horas`).val(det.horas);
            $(`#idMarcaNum${i + 1}Seguro`).val(det.seguro);
            $(`#idMarcaNum${i + 1}Garantia`).val(det.garantia);
            $(`#idMarcaNum${i + 1}Servicios`).val(det.serviciosPreventivos);
            $(`#idMarcaNum${i + 1}Capacitacion`).val(det.capacitacion);
            $(`#idMarcaNum${i + 1}Deposito`).val(det.depositoEnGarantia);
            $(`#idMarcaNum${i + 1}Lugar`).val(det.lugarDeEntrega);
            $(`#idMarcaNum${i + 1}Flete`).val(det.flete);
            $(`#idMarcaNum${i + 1}Condiciones`).val(det.condicionesDePagoEntrega);
            $(`#textareaComentarios${i + 1}`).val(det.comentarios);

            //#region Características
            let listaCaracteristicas = det.lstCaracteristicas;

            for (let j = 0; j < listaCaracteristicas.length; j++) {
                $(`#idMarcaNum${i + 1}Caracteristicas${i + 1}${j + 1}`).val(listaCaracteristicas[j].Descripcion);
            }
            //#endregion
        }
        //#endregion

        //#region Información de los Autorizantes
        for (let i = 1; i < 6; i++) {
            let aut = data.listaAutorizantes[i - 1];

            if (aut) {
                $(`#selAutSolicita${i}`).data('id', aut.autorizanteID);
                $(`#selAutSolicita${i}`).data('nombre', aut.autorizanteNombre);
                $(`#selAutSolicita${i}`).val(aut.autorizanteNombre);
            }
        }
        //#endregion
    }

    function fncAgregarColumnas() {
        columnasNuevoCuadro = $(tblComparativoAdquisicionyRenta.find('tr')[1]).find('td').length - 1;

        if (columnasNuevoCuadro > 7) {
            columnasNuevoCuadro = 7;
        } else {
            columnasNuevoCuadro++;

            tblComparativoAdquisicionyRenta.DataTable().columns([columnasNuevoCuadro]).visible(true);
        }
    }
    function fncEliminarColumna() {
        let td = $(tblComparativoAdquisicionyRenta.find('tr')[1]).find('td');

        columnasNuevoCuadro = td.length;

        if (td.length != 2) {
            if (columnasNuevoCuadro < 1) {
                columnasNuevoCuadro = 1;
            } else {
                columnasNuevoCuadro--;

                $(`#idMarcaNum${columnasNuevoCuadro}Marca`).val('');
                $(`#idMarcaNum${columnasNuevoCuadro}proveedor`).val('');
                $(`#idMarcaNum${columnasNuevoCuadro}precio`).val('');
                $(`#idMarcaNum${columnasNuevoCuadro}Trade`).val('');
                $(`#idMarcaNum${columnasNuevoCuadro}Valores`).val('');
                $(`#idMarcaNum${columnasNuevoCuadro}Precio`).val('');
                $(`#idMarcaNum${columnasNuevoCuadro}PrecioRoc`).val('');
                $(`#idMarcaNum${columnasNuevoCuadro}BaseHoras`).val('');
                $(`#idMarcaNum${columnasNuevoCuadro}Tiempo`).val('');
                $(`#idMarcaNum${columnasNuevoCuadro}Ubicacion`).val('');
                $(`#idMarcaNum${columnasNuevoCuadro}Horas`).val('');
                $(`#idMarcaNum${columnasNuevoCuadro}Seguro`).val('');
                $(`#idMarcaNum${columnasNuevoCuadro}Garantia`).val('');
                $(`#idMarcaNum${columnasNuevoCuadro}Servicios`).val('');
                $(`#idMarcaNum${columnasNuevoCuadro}Capacitacion`).val('');
                $(`#idMarcaNum${columnasNuevoCuadro}Deposito`).val('');
                $(`#idMarcaNum${columnasNuevoCuadro}Lugar`).val('');
                $(`#idMarcaNum${columnasNuevoCuadro}Flete`).val('');
                $(`#idMarcaNum${columnasNuevoCuadro}Condiciones`).val('');

                tblComparativoAdquisicionyRenta.DataTable().columns([columnasNuevoCuadro]).visible(false);
            }
        }
    }

    const CrearLstObjeto = function () {
        var formData = new FormData();

        if ($('#idMarcaNum1Marca').val() != "" &&
            $('#idMarcaNum1proveedor').val() != "" &&
            $('#idMarcaNum1precio').val() != "" &&
            $('#idMarcaNum1Trade').val() != "" &&
            $('#idMarcaNum1Valores').val() != "" &&
            $('#idMarcaNum1Precio').val() != "" &&
            $('#idMarcaNum1PrecioRoc').val() != "" &&
            $('#idMarcaNum1BaseHoras').val() != "" &&
            $('#idMarcaNum1Tiempo').val() != "" &&
            $('#idMarcaNum1Ubicacion').val() != "" &&
            $('#idMarcaNum1Horas').val() != "" &&
            $('#idMarcaNum1Seguro').val() != "" &&
            $('#idMarcaNum1Garantia').val() != "" &&
            $('#idMarcaNum1Servicios').val() != "" &&
            $('#idMarcaNum1Capacitacion').val() != "" &&
            $('#idMarcaNum1Deposito').val() != "" &&
            $('#idMarcaNum1Lugar').val() != "" &&
            $('#idMarcaNum1Flete').val() != "" &&
            $('#idMarcaNum1Condiciones').val() != "") {

            formData.append("objComparativo[0][idRow]", 1);
            formData.append("objComparativo[0][idDet]", idDet1);
            formData.append("objComparativo[0][idComparativo]", $('#idComparativo').val());
            formData.append("objComparativo[0][marcaModelo]", $('#idMarcaNum1Marca').val());
            formData.append("objComparativo[0][proveedor]", $('#idMarcaNum1proveedor').val());
            formData.append("objComparativo[0][precioDeVenta]", $('#idMarcaNum1precio').val());
            formData.append("objComparativo[0][tradeIn]", $('#idMarcaNum1Trade').val());
            formData.append("objComparativo[0][valoresDeRecompra]", $('#idMarcaNum1Valores').val());
            formData.append("objComparativo[0][precioDeRentaPura]", $('#idMarcaNum1Precio').val());
            formData.append("objComparativo[0][precioDeRentaEnRoc]", $('#idMarcaNum1PrecioRoc').val());
            formData.append("objComparativo[0][baseHoras]", $('#idMarcaNum1BaseHoras').val());
            formData.append("objComparativo[0][tiempoDeEntrega]", $('#idMarcaNum1Tiempo').val());
            formData.append("objComparativo[0][ubicacion]", $('#idMarcaNum1Ubicacion').val());
            formData.append("objComparativo[0][horas]", $('#idMarcaNum1Horas').val());
            formData.append("objComparativo[0][seguro]", $('#idMarcaNum1Seguro').val());
            formData.append("objComparativo[0][garantia]", $('#idMarcaNum1Garantia').val());
            formData.append("objComparativo[0][serviciosPreventivos]", $('#idMarcaNum1Servicios').val());
            formData.append("objComparativo[0][capacitacion]", $('#idMarcaNum1Capacitacion').val());
            formData.append("objComparativo[0][depositoEnGarantia]", $('#idMarcaNum1Deposito').val());
            formData.append("objComparativo[0][lugarDeEntrega]", $('#idMarcaNum1Lugar').val());
            formData.append("objComparativo[0][flete]", $('#idMarcaNum1Flete').val());
            formData.append("objComparativo[0][condicionesDePagoEntrega]", $('#idMarcaNum1Condiciones').val());
            formData.append("objComparativo[0][caracteristicasDelEquipo1]", $('#Caracteristica1').val());
            formData.append("objComparativo[0][caracteristicasDelEquipo2]", $('#Caracteristica2').val());
            formData.append("objComparativo[0][caracteristicasDelEquipo3]", $('#Caracteristica3').val());
            formData.append("objComparativo[0][caracteristicasDelEquipo4]", $('#Caracteristica4').val());
            formData.append("objComparativo[0][caracteristicasDelEquipo5]", $('#Caracteristica5').val());
            formData.append("objComparativo[0][caracteristicasDelEquipo6]", $('#Caracteristica6').val());
            formData.append("objComparativo[0][caracteristicasDelEquipo7]", $('#Caracteristica7').val());
            formData.append("objComparativo[0][tipoMoneda]", $('#printTipoMoneda').val())

            let lstCaracteristicas = crearlstCaracteristicas(1);

            let conlst = 0;
            lstCaracteristicas.forEach(x => {
                formData.append("objComparativo[0][lstCaracteristicas][" + conlst + "][idRow]", x.idRow);
                formData.append("objComparativo[0][lstCaracteristicas][" + conlst + "][idComparativoDetalle]", x.idComparativoDetalle);
                formData.append("objComparativo[0][lstCaracteristicas][" + conlst + "][Descripcion]", x.Descripcion);
                conlst++;
            });
            if (document.getElementById("inputAgregarImagen1") != null) {
                var file1 = document.getElementById("inputAgregarImagen1").files[0];
                if (file1 != undefined) {
                    formData.append("file", file1);
                }
            }
        }

        if ($('#idMarcaNum2Marca').val() != "" &&
            $('#idMarcaNum2proveedor').val() != "" &&
            $('#idMarcaNum2precio').val() != "" &&
            $('#idMarcaNum2Trade').val() != "" &&
            $('#idMarcaNum2Valores').val() != "" &&
            $('#idMarcaNum2Precio').val() != "" &&
            $('#idMarcaNum2PrecioRoc').val() != "" &&
            $('#idMarcaNum2BaseHoras').val() != "" &&
            $('#idMarcaNum2Tiempo').val() != "" &&
            $('#idMarcaNum2Ubicacion').val() != "" &&
            $('#idMarcaNum2Horas').val() != "" &&
            $('#idMarcaNum2Seguro').val() != "" &&
            $('#idMarcaNum2Garantia').val() != "" &&
            $('#idMarcaNum2Servicios').val() != "" &&
            $('#idMarcaNum2Capacitacion').val() != "" &&
            $('#idMarcaNum2Deposito').val() != "" &&
            $('#idMarcaNum2Lugar').val() != "" &&
            $('#idMarcaNum2Flete').val() != "" &&
            $('#idMarcaNum2Condiciones').val() != "") {

            formData.append("objComparativo[1][idRow]", 2);
            formData.append("objComparativo[1][idDet]", idDet2);
            formData.append("objComparativo[1][idComparativo]", $('#idComparativo').val());
            formData.append("objComparativo[1][marcaModelo]", $('#idMarcaNum2Marca').val());
            formData.append("objComparativo[1][proveedor]", $('#idMarcaNum2proveedor').val());
            formData.append("objComparativo[1][precioDeVenta]", $('#idMarcaNum2precio').val());
            formData.append("objComparativo[1][tradeIn]", $('#idMarcaNum2Trade').val());
            formData.append("objComparativo[1][valoresDeRecompra]", $('#idMarcaNum2Valores').val());
            formData.append("objComparativo[1][precioDeRentaPura]", $('#idMarcaNum2Precio').val());
            formData.append("objComparativo[1][precioDeRentaEnRoc]", $('#idMarcaNum2PrecioRoc').val());
            formData.append("objComparativo[1][baseHoras]", $('#idMarcaNum2BaseHoras').val());
            formData.append("objComparativo[1][tiempoDeEntrega]", $('#idMarcaNum2Tiempo').val());
            formData.append("objComparativo[1][ubicacion]", $('#idMarcaNum2Ubicacion').val());
            formData.append("objComparativo[1][horas]", $('#idMarcaNum2Horas').val());
            formData.append("objComparativo[1][seguro]", $('#idMarcaNum2Seguro').val());
            formData.append("objComparativo[1][garantia]", $('#idMarcaNum2Garantia').val());
            formData.append("objComparativo[1][serviciosPreventivos]", $('#idMarcaNum2Servicios').val());
            formData.append("objComparativo[1][capacitacion]", $('#idMarcaNum2Capacitacion').val());
            formData.append("objComparativo[1][depositoEnGarantia]", $('#idMarcaNum2Deposito').val());
            formData.append("objComparativo[1][lugarDeEntrega]", $('#idMarcaNum2Lugar').val());
            formData.append("objComparativo[1][flete]", $('#idMarcaNum2Flete').val());
            formData.append("objComparativo[1][condicionesDePagoEntrega]", $('#idMarcaNum2Condiciones').val());
            formData.append("objComparativo[1][caracteristicasDelEquipo1]", $('#Caracteristica1').val());
            formData.append("objComparativo[1][caracteristicasDelEquipo2]", $('#Caracteristica2').val());
            formData.append("objComparativo[1][caracteristicasDelEquipo2]", $('#Caracteristica3').val());
            formData.append("objComparativo[1][caracteristicasDelEquipo2]", $('#Caracteristica4').val());
            formData.append("objComparativo[1][caracteristicasDelEquipo2]", $('#Caracteristica5').val());
            formData.append("objComparativo[1][caracteristicasDelEquipo2]", $('#Caracteristica6').val());
            formData.append("objComparativo[1][caracteristicasDelEquipo7]", $('#Caracteristica7').val());


            let lstCaracteristicas = crearlstCaracteristicas(2);

            let conlst = 0;
            lstCaracteristicas.forEach(x => {
                formData.append("objComparativo[1][lstCaracteristicas][" + conlst + "][idRow]", x.idRow);
                formData.append("objComparativo[1][lstCaracteristicas][" + conlst + "][idComparativoDetalle]", x.idComparativoDetalle);
                formData.append("objComparativo[1][lstCaracteristicas][" + conlst + "][Descripcion]", x.Descripcion);
                conlst++;
            });


            if (document.getElementById("inputAgregarImagen2") != null) {
                var file1 = document.getElementById("inputAgregarImagen2").files[0];
                if (file1 != undefined) {
                    formData.append("file", file1);
                }
            }
        }

        if ($('#idMarcaNum3Marca').val() != "" &&
            $('#idMarcaNum3proveedor').val() != "" &&
            $('#idMarcaNum3precio').val() != "" &&
            $('#idMarcaNum3Trade').val() != "" &&
            $('#idMarcaNum3Valores').val() != "" &&
            $('#idMarcaNum3Precio').val() != "" &&
            $('#idMarcaNum3PrecioRoc').val() != "" &&
            $('#idMarcaNum3BaseHoras').val() != "" &&
            $('#idMarcaNum3Tiempo').val() != "" &&
            $('#idMarcaNum3Ubicacion').val() != "" &&
            $('#idMarcaNum3Horas').val() != "" &&
            $('#idMarcaNum3Seguro').val() != "" &&
            $('#idMarcaNum3Garantia').val() != "" &&
            $('#idMarcaNum3Servicios').val() != "" &&
            $('#idMarcaNum3Capacitacion').val() != "" &&
            $('#idMarcaNum3Deposito').val() != "" &&
            $('#idMarcaNum3Lugar').val() != "" &&
            $('#idMarcaNum3Flete').val() != "" &&
            $('#idMarcaNum3Condiciones').val() != "") {


            formData.append("objComparativo[2][idRow]", 3);
            formData.append("objComparativo[2][idDet]", idDet3);
            formData.append("objComparativo[2][idComparativo]", $('#idComparativo').val());
            formData.append("objComparativo[2][marcaModelo]", $('#idMarcaNum3Marca').val());
            formData.append("objComparativo[2][proveedor]", $('#idMarcaNum3proveedor').val());
            formData.append("objComparativo[2][precioDeVenta]", $('#idMarcaNum3precio').val());
            formData.append("objComparativo[2][tradeIn]", $('#idMarcaNum3Trade').val());
            formData.append("objComparativo[2][valoresDeRecompra]", $('#idMarcaNum3Valores').val());
            formData.append("objComparativo[2][precioDeRentaPura]", $('#idMarcaNum3Precio').val());
            formData.append("objComparativo[2][precioDeRentaEnRoc]", $('#idMarcaNum3PrecioRoc').val());
            formData.append("objComparativo[2][baseHoras]", $('#idMarcaNum3BaseHoras').val());
            formData.append("objComparativo[2][tiempoDeEntrega]", $('#idMarcaNum3Tiempo').val());
            formData.append("objComparativo[2][ubicacion]", $('#idMarcaNum3Ubicacion').val());
            formData.append("objComparativo[2][horas]", $('#idMarcaNum3Horas').val());
            formData.append("objComparativo[2][seguro]", $('#idMarcaNum3Seguro').val());
            formData.append("objComparativo[2][garantia]", $('#idMarcaNum3Garantia').val());
            formData.append("objComparativo[2][serviciosPreventivos]", $('#idMarcaNum3Servicios').val());
            formData.append("objComparativo[2][capacitacion]", $('#idMarcaNum3Capacitacion').val());
            formData.append("objComparativo[2][depositoEnGarantia]", $('#idMarcaNum3Deposito').val());
            formData.append("objComparativo[2][lugarDeEntrega]", $('#idMarcaNum3Lugar').val());
            formData.append("objComparativo[2][flete]", $('#idMarcaNum3Flete').val());
            formData.append("objComparativo[2][condicionesDePagoEntrega]", $('#idMarcaNum3Condiciones').val());
            formData.append("objComparativo[2][caracteristicasDelEquipo1]", $('#Caracteristica1').val());
            formData.append("objComparativo[2][caracteristicasDelEquipo2]", $('#Caracteristica2').val());
            formData.append("objComparativo[2][caracteristicasDelEquipo3]", $('#Caracteristica3').val());
            formData.append("objComparativo[2][caracteristicasDelEquipo4]", $('#Caracteristica4').val());
            formData.append("objComparativo[2][caracteristicasDelEquipo5]", $('#Caracteristica5').val());
            formData.append("objComparativo[2][caracteristicasDelEquipo6]", $('#Caracteristica6').val());
            formData.append("objComparativo[2][caracteristicasDelEquipo7]", $('#Caracteristica7').val());


            let lstCaracteristicas = crearlstCaracteristicas(3);

            let conlst = 0;
            lstCaracteristicas.forEach(x => {
                formData.append("objComparativo[2][lstCaracteristicas][" + conlst + "][idRow]", x.idRow);
                formData.append("objComparativo[2][lstCaracteristicas][" + conlst + "][idComparativoDetalle]", x.idComparativoDetalle);
                formData.append("objComparativo[2][lstCaracteristicas][" + conlst + "][Descripcion]", x.Descripcion);
                conlst++;
            });

            if (document.getElementById("inputAgregarImagen3") != null) {
                var file1 = document.getElementById("inputAgregarImagen3").files[0];
                if (file1 != undefined) {
                    formData.append("file", file1);
                }
            }
        }

        if ($('#idMarcaNum4Marca').val() != "" &&
            $('#idMarcaNum4proveedor').val() != "" &&
            $('#idMarcaNum4precio').val() != "" &&
            $('#idMarcaNum4Trade').val() != "" &&
            $('#idMarcaNum4Valores').val() != "" &&
            $('#idMarcaNum4Precio').val() != "" &&
            $('#idMarcaNum4PrecioRoc').val() != "" &&
            $('#idMarcaNum4BaseHoras').val() != "" &&
            $('#idMarcaNum4Tiempo').val() != "" &&
            $('#idMarcaNum4Ubicacion').val() != "" &&
            $('#idMarcaNum4Horas').val() != "" &&
            $('#idMarcaNum4Seguro').val() != "" &&
            $('#idMarcaNum4Garantia').val() != "" &&
            $('#idMarcaNum4Servicios').val() != "" &&
            $('#idMarcaNum4Capacitacion').val() != "" &&
            $('#idMarcaNum4Deposito').val() != "" &&
            $('#idMarcaNum4Lugar').val() != "" &&
            $('#idMarcaNum4Flete').val() != "" &&
            $('#idMarcaNum4Condiciones').val() != "") {
            formData.append("objComparativo[3][idRow]", 4);
            formData.append("objComparativo[3][idDet]", idDet4);
            formData.append("objComparativo[3][idComparativo]", $('#idComparativo').val());
            formData.append("objComparativo[3][marcaModelo]", $('#idMarcaNum4Marca').val());
            formData.append("objComparativo[3][proveedor]", $('#idMarcaNum4proveedor').val());
            formData.append("objComparativo[3][precioDeVenta]", $('#idMarcaNum4precio').val());
            formData.append("objComparativo[3][tradeIn]", $('#idMarcaNum4Trade').val());
            formData.append("objComparativo[3][valoresDeRecompra]", $('#idMarcaNum4Valores').val());
            formData.append("objComparativo[3][precioDeRentaPura]", $('#idMarcaNum4Precio').val());
            formData.append("objComparativo[3][precioDeRentaEnRoc]", $('#idMarcaNum4PrecioRoc').val());
            formData.append("objComparativo[3][baseHoras]", $('#idMarcaNum4BaseHoras').val());
            formData.append("objComparativo[3][tiempoDeEntrega]", $('#idMarcaNum4Tiempo').val());
            formData.append("objComparativo[3][ubicacion]", $('#idMarcaNum4Ubicacion').val());
            formData.append("objComparativo[3][horas]", $('#idMarcaNum4Horas').val());
            formData.append("objComparativo[3][seguro]", $('#idMarcaNum4Seguro').val());
            formData.append("objComparativo[3][garantia]", $('#idMarcaNum4Garantia').val());
            formData.append("objComparativo[3][serviciosPreventivos]", $('#idMarcaNum4Servicios').val());
            formData.append("objComparativo[3][capacitacion]", $('#idMarcaNum4Capacitacion').val());
            formData.append("objComparativo[3][depositoEnGarantia]", $('#idMarcaNum4Deposito').val());
            formData.append("objComparativo[3][lugarDeEntrega]", $('#idMarcaNum4Lugar').val());
            formData.append("objComparativo[3][flete]", $('#idMarcaNum4Flete').val());
            formData.append("objComparativo[3][condicionesDePagoEntrega]", $('#idMarcaNum4Condiciones').val());
            formData.append("objComparativo[3][caracteristicasDelEquipo1]", $('#Caracteristica1').val());
            formData.append("objComparativo[3][caracteristicasDelEquipo2]", $('#Caracteristica2').val());
            formData.append("objComparativo[3][caracteristicasDelEquipo3]", $('#Caracteristica3').val());
            formData.append("objComparativo[3][caracteristicasDelEquipo4]", $('#Caracteristica4').val());
            formData.append("objComparativo[3][caracteristicasDelEquipo5]", $('#Caracteristica5').val());
            formData.append("objComparativo[3][caracteristicasDelEquipo6]", $('#Caracteristica6').val());
            formData.append("objComparativo[3][caracteristicasDelEquipo7]", $('#Caracteristica7').val());

            let lstCaracteristicas = crearlstCaracteristicas(4);

            let conlst = 0;
            lstCaracteristicas.forEach(x => {
                formData.append("objComparativo[3][lstCaracteristicas][" + conlst + "][idRow]", x.idRow);
                formData.append("objComparativo[3][lstCaracteristicas][" + conlst + "][idComparativoDetalle]", x.idComparativoDetalle);
                formData.append("objComparativo[3][lstCaracteristicas][" + conlst + "][Descripcion]", x.Descripcion);
                conlst++;
            });
            if (document.getElementById("inputAgregarImagen4") != null) {
                var file1 = document.getElementById("inputAgregarImagen4").files[0];
                if (file1 != undefined) {
                    formData.append("file", file1);
                }
            }
        }

        if ($('#idMarcaNum5Marca').val() != "" &&
            $('#idMarcaNum5proveedor').val() != "" &&
            $('#idMarcaNum5precio').val() != "" &&
            $('#idMarcaNum5Trade').val() != "" &&
            $('#idMarcaNum5Valores').val() != "" &&
            $('#idMarcaNum5Precio').val() != "" &&
            $('#idMarcaNum5PrecioRoc').val() != "" &&
            $('#idMarcaNum5BaseHoras').val() != "" &&
            $('#idMarcaNum5Tiempo').val() != "" &&
            $('#idMarcaNum5Ubicacion').val() != "" &&
            $('#idMarcaNum5Horas').val() != "" &&
            $('#idMarcaNum5Seguro').val() != "" &&
            $('#idMarcaNum5Garantia').val() != "" &&
            $('#idMarcaNum5Servicios').val() != "" &&
            $('#idMarcaNum5Capacitacion').val() != "" &&
            $('#idMarcaNum5Deposito').val() != "" &&
            $('#idMarcaNum5Lugar').val() != "" &&
            $('#idMarcaNum5Flete').val() != "" &&
            $('#idMarcaNum5Condiciones').val() != "") {
            formData.append("objComparativo[4][idRow]", 5);
            formData.append("objComparativo[4][idDet]", idDet5);
            formData.append("objComparativo[4][idComparativo]", $('#idComparativo').val());
            formData.append("objComparativo[4][marcaModelo]", $('#idMarcaNum5Marca').val());
            formData.append("objComparativo[4][proveedor]", $('#idMarcaNum5proveedor').val());
            formData.append("objComparativo[4][precioDeVenta]", $('#idMarcaNum5precio').val());
            formData.append("objComparativo[4][tradeIn]", $('#idMarcaNum5Trade').val());
            formData.append("objComparativo[4][valoresDeRecompra]", $('#idMarcaNum5Valores').val());
            formData.append("objComparativo[4][precioDeRentaPura]", $('#idMarcaNum5Precio').val());
            formData.append("objComparativo[4][precioDeRentaEnRoc]", $('#idMarcaNum5PrecioRoc').val());
            formData.append("objComparativo[4][baseHoras]", $('#idMarcaNum5BaseHoras').val());
            formData.append("objComparativo[4][tiempoDeEntrega]", $('#idMarcaNum5Tiempo').val());
            formData.append("objComparativo[4][ubicacion]", $('#idMarcaNum5Ubicacion').val());
            formData.append("objComparativo[4][horas]", $('#idMarcaNum5Horas').val());
            formData.append("objComparativo[4][seguro]", $('#idMarcaNum5Seguro').val());
            formData.append("objComparativo[4][garantia]", $('#idMarcaNum5Garantia').val());
            formData.append("objComparativo[4][serviciosPreventivos]", $('#idMarcaNum5Servicios').val());
            formData.append("objComparativo[4][capacitacion]", $('#idMarcaNum5Capacitacion').val());
            formData.append("objComparativo[4][depositoEnGarantia]", $('#idMarcaNum5Deposito').val());
            formData.append("objComparativo[4][lugarDeEntrega]", $('#idMarcaNum5Lugar').val());
            formData.append("objComparativo[4][flete]", $('#idMarcaNum5Flete').val());
            formData.append("objComparativo[4][condicionesDePagoEntrega]", $('#idMarcaNum5Condiciones').val());
            formData.append("objComparativo[4][caracteristicasDelEquipo1]", $('#Caracteristica1').val());
            formData.append("objComparativo[4][caracteristicasDelEquipo2]", $('#Caracteristica2').val());
            formData.append("objComparativo[4][caracteristicasDelEquipo3]", $('#Caracteristica3').val());
            formData.append("objComparativo[4][caracteristicasDelEquipo4]", $('#Caracteristica4').val());
            formData.append("objComparativo[4][caracteristicasDelEquipo5]", $('#Caracteristica5').val());
            formData.append("objComparativo[4][caracteristicasDelEquipo6]", $('#Caracteristica6').val());
            formData.append("objComparativo[4][caracteristicasDelEquipo7]", $('#Caracteristica7').val());


            let lstCaracteristicas = crearlstCaracteristicas(5);

            let conlst = 0;
            lstCaracteristicas.forEach(x => {
                formData.append("objComparativo[4][lstCaracteristicas][" + conlst + "][idRow]", x.idRow);
                formData.append("objComparativo[4][lstCaracteristicas][" + conlst + "][idComparativoDetalle]", x.idComparativoDetalle);
                formData.append("objComparativo[4][lstCaracteristicas][" + conlst + "][Descripcion]", x.Descripcion);
                conlst++;
            });
            if (document.getElementById("inputAgregarImagen5") != null) {
                var file1 = document.getElementById("inputAgregarImagen5").files[0];
                if (file1 != undefined) {
                    formData.append("file", file1);
                }
            }
        }

        if ($('#idMarcaNum6Marca').val() != "" &&
            $('#idMarcaNum6proveedor').val() != "" &&
            $('#idMarcaNum6precio').val() != "" &&
            $('#idMarcaNum6Trade').val() != "" &&
            $('#idMarcaNum6Valores').val() != "" &&
            $('#idMarcaNum6Precio').val() != "" &&
            $('#idMarcaNum6PrecioRoc').val() != "" &&
            $('#idMarcaNum6BaseHoras').val() != "" &&
            $('#idMarcaNum6Tiempo').val() != "" &&
            $('#idMarcaNum6Ubicacion').val() != "" &&
            $('#idMarcaNum6Horas').val() != "" &&
            $('#idMarcaNum6Seguro').val() != "" &&
            $('#idMarcaNum6Garantia').val() != "" &&
            $('#idMarcaNum6Servicios').val() != "" &&
            $('#idMarcaNum6Capacitacion').val() != "" &&
            $('#idMarcaNum6Deposito').val() != "" &&
            $('#idMarcaNum6Lugar').val() != "" &&
            $('#idMarcaNum6Flete').val() != "" &&
            $('#idMarcaNum6Condiciones').val() != "") {
            formData.append("objComparativo[5][idRow]", 6);
            formData.append("objComparativo[5][idDet]", idDet6);
            formData.append("objComparativo[5][idComparativo]", $('#idComparativo').val());
            formData.append("objComparativo[5][marcaModelo]", $('#idMarcaNum6Marca').val());
            formData.append("objComparativo[5][proveedor]", $('#idMarcaNum6proveedor').val());
            formData.append("objComparativo[5][precioDeVenta]", $('#idMarcaNum6precio').val());
            formData.append("objComparativo[5][tradeIn]", $('#idMarcaNum6Trade').val());
            formData.append("objComparativo[5][valoresDeRecompra]", $('#idMarcaNum6Valores').val());
            formData.append("objComparativo[5][precioDeRentaPura]", $('#idMarcaNum6Precio').val());
            formData.append("objComparativo[5][precioDeRentaEnRoc]", $('#idMarcaNum6PrecioRoc').val());
            formData.append("objComparativo[5][baseHoras]", $('#idMarcaNum6BaseHoras').val());
            formData.append("objComparativo[5][tiempoDeEntrega]", $('#idMarcaNum6Tiempo').val());
            formData.append("objComparativo[5][ubicacion]", $('#idMarcaNum6Ubicacion').val());
            formData.append("objComparativo[5][horas]", $('#idMarcaNum6Horas').val());
            formData.append("objComparativo[5][seguro]", $('#idMarcaNum6Seguro').val());
            formData.append("objComparativo[5][garantia]", $('#idMarcaNum6Garantia').val());
            formData.append("objComparativo[5][serviciosPreventivos]", $('#idMarcaNum6Servicios').val());
            formData.append("objComparativo[5][capacitacion]", $('#idMarcaNum6Capacitacion').val());
            formData.append("objComparativo[5][depositoEnGarantia]", $('#idMarcaNum6Deposito').val());
            formData.append("objComparativo[5][lugarDeEntrega]", $('#idMarcaNum6Lugar').val());
            formData.append("objComparativo[5][flete]", $('#idMarcaNum6Flete').val());
            formData.append("objComparativo[5][condicionesDePagoEntrega]", $('#idMarcaNum6Condiciones').val());
            formData.append("objComparativo[5][caracteristicasDelEquipo1]", $('#Caracteristica1').val());
            formData.append("objComparativo[5][caracteristicasDelEquipo2]", $('#Caracteristica2').val());
            formData.append("objComparativo[5][caracteristicasDelEquipo3]", $('#Caracteristica3').val());
            formData.append("objComparativo[5][caracteristicasDelEquipo4]", $('#Caracteristica4').val());
            formData.append("objComparativo[5][caracteristicasDelEquipo5]", $('#Caracteristica5').val());
            formData.append("objComparativo[5][caracteristicasDelEquipo6]", $('#Caracteristica6').val());
            formData.append("objComparativo[5][caracteristicasDelEquipo7]", $('#Caracteristica7').val());


            let lstCaracteristicas = crearlstCaracteristicas(6);

            let conlst = 0;
            lstCaracteristicas.forEach(x => {
                formData.append("objComparativo[5][lstCaracteristicas][" + conlst + "][idRow]", x.idRow);
                formData.append("objComparativo[5][lstCaracteristicas][" + conlst + "][idComparativoDetalle]", x.idComparativoDetalle);
                formData.append("objComparativo[5][lstCaracteristicas][" + conlst + "][Descripcion]", x.Descripcion);
                conlst++;
            });
            if (document.getElementById("inputAgregarImagen6") != null) {
                var file1 = document.getElementById("inputAgregarImagen6").files[0];
                if (file1 != undefined) {
                    formData.append("file", file1);
                }
            }
        }

        if ($('#idMarcaNum7Marca').val() != "" &&
            $('#idMarcaNum7proveedor').val() != "" &&
            $('#idMarcaNum7precio').val() != "" &&
            $('#idMarcaNum7Trade').val() != "" &&
            $('#idMarcaNum7Valores').val() != "" &&
            $('#idMarcaNum7Precio').val() != "" &&
            $('#idMarcaNum7PrecioRoc').val() != "" &&
            $('#idMarcaNum7BaseHoras').val() != "" &&
            $('#idMarcaNum7Tiempo').val() != "" &&
            $('#idMarcaNum7Ubicacion').val() != "" &&
            $('#idMarcaNum7Horas').val() != "" &&
            $('#idMarcaNum7Seguro').val() != "" &&
            $('#idMarcaNum7Garantia').val() != "" &&
            $('#idMarcaNum7Servicios').val() != "" &&
            $('#idMarcaNum7Capacitacion').val() != "" &&
            $('#idMarcaNum7Deposito').val() != "" &&
            $('#idMarcaNum7Lugar').val() != "" &&
            $('#idMarcaNum7Flete').val() != "" &&
            $('#idMarcaNum7Condiciones').val() != "") {
            formData.append("objComparativo[6][idRow]", 7);
            formData.append("objComparativo[6][idDet]", idDet7);
            formData.append("objComparativo[6][idComparativo]", $('#idComparativo').val());
            formData.append("objComparativo[6][marcaModelo]", $('#idMarcaNum7Marca').val());
            formData.append("objComparativo[6][proveedor]", $('#idMarcaNum7proveedor').val());
            formData.append("objComparativo[6][precioDeVenta]", $('#idMarcaNum7precio').val());
            formData.append("objComparativo[6][tradeIn]", $('#idMarcaNum7Trade').val());
            formData.append("objComparativo[6][valoresDeRecompra]", $('#idMarcaNum7Valores').val());
            formData.append("objComparativo[6][precioDeRentaPura]", $('#idMarcaNum7Precio').val());
            formData.append("objComparativo[6][precioDeRentaEnRoc]", $('#idMarcaNum7PrecioRoc').val());
            formData.append("objComparativo[6][baseHoras]", $('#idMarcaNum7BaseHoras').val());
            formData.append("objComparativo[6][tiempoDeEntrega]", $('#idMarcaNum7Tiempo').val());
            formData.append("objComparativo[6][ubicacion]", $('#idMarcaNum7Ubicacion').val());
            formData.append("objComparativo[6][horas]", $('#idMarcaNum7Horas').val());
            formData.append("objComparativo[6][seguro]", $('#idMarcaNum7Seguro').val());
            formData.append("objComparativo[6][garantia]", $('#idMarcaNum7Garantia').val());
            formData.append("objComparativo[6][serviciosPreventivos]", $('#idMarcaNum7Servicios').val());
            formData.append("objComparativo[6][capacitacion]", $('#idMarcaNum7Capacitacion').val());
            formData.append("objComparativo[6][depositoEnGarantia]", $('#idMarcaNum7Deposito').val());
            formData.append("objComparativo[6][lugarDeEntrega]", $('#idMarcaNum7Lugar').val());
            formData.append("objComparativo[6][flete]", $('#idMarcaNum7Flete').val());
            formData.append("objComparativo[6][condicionesDePagoEntrega]", $('#idMarcaNum7Condiciones').val());
            formData.append("objComparativo[6][caracteristicasDelEquipo1]", $('#Caracteristica1').val());
            formData.append("objComparativo[6][caracteristicasDelEquipo2]", $('#Caracteristica2').val());
            formData.append("objComparativo[6][caracteristicasDelEquipo3]", $('#Caracteristica3').val());
            formData.append("objComparativo[6][caracteristicasDelEquipo4]", $('#Caracteristica4').val());
            formData.append("objComparativo[6][caracteristicasDelEquipo5]", $('#Caracteristica5').val());
            formData.append("objComparativo[6][caracteristicasDelEquipo6]", $('#Caracteristica6').val());
            formData.append("objComparativo[6][caracteristicasDelEquipo7]", $('#Caracteristica7').val());

            let lstCaracteristicas = crearlstCaracteristicas(7);

            let conlst = 0;
            lstCaracteristicas.forEach(x => {
                formData.append("objComparativo[6][lstCaracteristicas][" + conlst + "][idRow]", x.idRow);
                formData.append("objComparativo[6][lstCaracteristicas][" + conlst + "][idComparativoDetalle]", x.idComparativoDetalle);
                formData.append("objComparativo[6][lstCaracteristicas][" + conlst + "][Descripcion]", x.Descripcion);
                conlst++;
            });
            if (document.getElementById("inputAgregarImagen7") != null) {
                var file1 = document.getElementById("inputAgregarImagen7").files[0];
                if (file1 != undefined) {
                    formData.append("file", file1);
                }
            }
        }

        return formData;
    }
    const crearlstCaracteristicas = function (Tipo) {
        let lstReturn = [];

        switch (Tipo) {
            case 1:
                for (let i = 0; i < 7; i++) {
                    let item = {};
                    if (item != {}) {
                        if (i == 0) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 1) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 2) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 3) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 4) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 5) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 6) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }

                    }
                }
                break;
            case 2:
                for (let i = 0; i < 7; i++) {
                    let item = {};
                    if (item != {}) {
                        if (i == 0) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 1) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 2) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 3) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 4) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 5) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 6) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }

                    }
                }
                break;
            case 3:
                for (let i = 0; i < 7; i++) {
                    let item = {};
                    if (item != {}) {
                        if (i == 0) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 1) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 2) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 3) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 4) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 5) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 6) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }

                    }
                }
                break;
            case 4:
                for (let i = 0; i < 7; i++) {
                    let item = {};
                    if (item != {}) {
                        if (i == 0) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 1) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 2) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 3) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 4) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 5) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 6) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }

                    }
                }
                break;
            case 5:
                for (let i = 0; i < 7; i++) {
                    let item = {};
                    if (item != {}) {
                        if (i == 0) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 1) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 2) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 3) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 4) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 5) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 6) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }

                    }
                }
                break;
            case 6:
                for (let i = 0; i < 7; i++) {
                    let item = {};
                    if (item != {}) {
                        if (i == 0) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 1) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 2) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 3) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 4) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 5) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 6) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }

                    }
                }
                break;
            case 7:
                for (let i = 0; i < 7; i++) {
                    let item = {};
                    if (item != {}) {
                        if (i == 0) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 1) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 2) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 3) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 4) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 5) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }
                        if (i == 6) {
                            item = objCaracteristicas(Tipo, i);
                            lstReturn.push(item);
                        }

                    }
                }
                break;
            default:
                break;
        }
        return lstReturn;
    }
    const objCaracteristicas = function (Tipo, idRow) {
        let objReturn = {};

        switch (Tipo) {
            case 1:
                if (idRow == 0) {
                    objReturn = {
                        id: idCaracteristica1,
                        idComparativoDetalle: idDet1,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum1Caracteristicas11').val()
                    };
                }
                if (idRow == 1) {
                    objReturn = {
                        id: idCaracteristica2,
                        idComparativoDetalle: idDet1,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum1Caracteristicas12').val()
                    };
                }
                if (idRow == 2) {
                    objReturn = {
                        id: idCaracteristica3,
                        idComparativoDetalle: idDet1,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum1Caracteristicas13').val()
                    };
                }
                if (idRow == 3) {
                    objReturn = {
                        id: idCaracteristica4,
                        idComparativoDetalle: idDet1,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum1Caracteristicas14').val()
                    };
                }
                if (idRow == 4) {
                    objReturn = {
                        id: idCaracteristica5,
                        idComparativoDetalle: idDet1,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum1Caracteristicas15').val()
                    };
                }
                if (idRow == 5) {
                    objReturn = {
                        id: idCaracteristica6,
                        idComparativoDetalle: idDet1,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum1Caracteristicas16').val()
                    };
                }
                if (idRow == 6) {
                    objReturn = {
                        id: idCaracteristica7,
                        idComparativoDetalle: idDet1,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum1Caracteristicas17').val()
                    };
                }
                break;
            case 2:
                if (idRow == 0) {
                    objReturn = {
                        id: idCaracteristica21,
                        idComparativoDetalle: idDet2,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum2Caracteristicas21').val()
                    };
                }
                if (idRow == 1) {
                    objReturn = {
                        id: idCaracteristica22,
                        idComparativoDetalle: idDet2,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum2Caracteristicas22').val()
                    };
                }
                if (idRow == 2) {
                    objReturn = {
                        id: idCaracteristica23,
                        idComparativoDetalle: idDet2,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum2Caracteristicas23').val()
                    };
                }
                if (idRow == 3) {
                    objReturn = {
                        id: idCaracteristica24,
                        idComparativoDetalle: idDet2,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum2Caracteristicas24').val()
                    };
                }
                if (idRow == 4) {
                    objReturn = {
                        id: idCaracteristica25,
                        idComparativoDetalle: idDet2,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum2Caracteristicas25').val()
                    };
                }
                if (idRow == 5) {
                    objReturn = {
                        id: idCaracteristica26,
                        idComparativoDetalle: idDet2,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum2Caracteristicas26').val()
                    };
                }
                if (idRow == 6) {
                    objReturn = {
                        id: idCaracteristica27,
                        idComparativoDetalle: idDet2,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum2Caracteristicas27').val()
                    };
                }
                break;
            case 3:
                if (idRow == 0) {
                    objReturn = {
                        id: idCaracteristica31,
                        idComparativoDetalle: idDet3,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum3Caracteristicas31').val()
                    };
                }
                if (idRow == 1) {
                    objReturn = {
                        id: idCaracteristica32,
                        idComparativoDetalle: idDet3,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum3Caracteristicas32').val()
                    };
                }
                if (idRow == 2) {
                    objReturn = {
                        id: idCaracteristica33,
                        idComparativoDetalle: idDet3,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum3Caracteristicas33').val()
                    };
                }
                if (idRow == 3) {
                    objReturn = {
                        id: idCaracteristica34,
                        idComparativoDetalle: idDet3,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum3Caracteristicas34').val()
                    };
                }
                if (idRow == 4) {
                    objReturn = {
                        id: idCaracteristica35,
                        idComparativoDetalle: idDet3,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum3Caracteristicas35').val()
                    };
                }
                if (idRow == 5) {
                    objReturn = {
                        id: idCaracteristica36,
                        idComparativoDetalle: idDet3,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum3Caracteristicas36').val()
                    };
                }
                if (idRow == 6) {
                    objReturn = {
                        id: idCaracteristica37,
                        idComparativoDetalle: idDet3,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum3Caracteristicas37').val()
                    };
                }
                break;
            case 4:
                if (idRow == 0) {
                    objReturn = {
                        id: idCaracteristica41,
                        idComparativoDetalle: idDet4,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum4Caracteristicas41').val()
                    };
                }
                if (idRow == 1) {
                    objReturn = {
                        id: idCaracteristica42,
                        idComparativoDetalle: idDet4,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum4Caracteristicas42').val()
                    };
                }
                if (idRow == 2) {
                    objReturn = {
                        id: idCaracteristica43,
                        idComparativoDetalle: idDet4,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum4Caracteristicas43').val()
                    };
                }
                if (idRow == 3) {
                    objReturn = {
                        id: idCaracteristica44,
                        idComparativoDetalle: idDet4,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum4Caracteristicas44').val()
                    };
                }
                if (idRow == 4) {
                    objReturn = {
                        id: idCaracteristica45,
                        idComparativoDetalle: idDet4,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum4Caracteristicas45').val()
                    };
                }
                if (idRow == 5) {
                    objReturn = {
                        id: idCaracteristica46,
                        idComparativoDetalle: idDet4,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum4Caracteristicas46').val()
                    };
                }
                if (idRow == 6) {
                    objReturn = {
                        id: idCaracteristica47,
                        idComparativoDetalle: idDet4,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum4Caracteristicas47').val()
                    };
                }
                break;
            case 5:
                if (idRow == 0) {
                    objReturn = {
                        id: idCaracteristica51,
                        idComparativoDetalle: idDet5,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum5Caracteristicas51').val()
                    };
                }
                if (idRow == 1) {
                    objReturn = {
                        id: idCaracteristica52,
                        idComparativoDetalle: idDet5,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum5Caracteristicas52').val()
                    };
                }
                if (idRow == 2) {
                    objReturn = {
                        id: idCaracteristica53,
                        idComparativoDetalle: idDet5,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum5Caracteristicas53').val()
                    };
                }
                if (idRow == 3) {
                    objReturn = {
                        id: idCaracteristica54,
                        idComparativoDetalle: idDet5,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum5Caracteristicas54').val()
                    };
                }
                if (idRow == 4) {
                    objReturn = {
                        id: idCaracteristica55,
                        idComparativoDetalle: idDet1,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum5Caracteristicas55').val()
                    };
                }
                if (idRow == 5) {
                    objReturn = {
                        id: idCaracteristica56,
                        idComparativoDetalle: idDet5,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum5Caracteristicas56').val()
                    };
                }
                if (idRow == 6) {
                    objReturn = {
                        id: idCaracteristica57,
                        idComparativoDetalle: idDet5,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum5Caracteristicas57').val()
                    };
                }
                break;
            case 6:
                if (idRow == 0) {
                    objReturn = {
                        id: idCaracteristica61,
                        idComparativoDetalle: idDet6,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum6Caracteristicas61').val()
                    };
                }
                if (idRow == 1) {
                    objReturn = {
                        id: idCaracteristica62,
                        idComparativoDetalle: idDet6,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum6Caracteristicas62').val()
                    };
                }
                if (idRow == 2) {
                    objReturn = {
                        id: idCaracteristica63,
                        idComparativoDetalle: idDet6,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum6Caracteristicas63').val()
                    };
                }
                if (idRow == 3) {
                    objReturn = {
                        id: idCaracteristica64,
                        idComparativoDetalle: idDet6,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum6Caracteristicas64').val()
                    };
                }
                if (idRow == 4) {
                    objReturn = {
                        id: idCaracteristica65,
                        idComparativoDetalle: idDet6,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum6Caracteristicas65').val()
                    };
                }
                if (idRow == 5) {
                    objReturn = {
                        id: idCaracteristica66,
                        idComparativoDetalle: idDet6,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum6Caracteristicas66').val()
                    };
                }
                if (idRow == 6) {
                    objReturn = {
                        id: idCaracteristica67,
                        idComparativoDetalle: idDet6,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum6Caracteristicas67').val()
                    };
                }
                break;
            case 7:
                if (idRow == 0) {
                    objReturn = {
                        id: idCaracteristica71,
                        idComparativoDetalle: idDet7,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum7Caracteristicas71').val()
                    };
                }
                if (idRow == 1) {
                    objReturn = {
                        id: idCaracteristica72,
                        idComparativoDetalle: idDet7,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum7Caracteristicas72').val()
                    };
                }
                if (idRow == 2) {
                    objReturn = {
                        id: idCaracteristica73,
                        idComparativoDetalle: idDet7,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum7Caracteristicas73').val()
                    };
                }
                if (idRow == 3) {
                    objReturn = {
                        id: idCaracteristica74,
                        idComparativoDetalle: idDet7,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum7Caracteristicas74').val()
                    };
                }
                if (idRow == 4) {
                    objReturn = {
                        id: idCaracteristica75,
                        idComparativoDetalle: idDet7,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum7Caracteristicas75').val()
                    };
                }
                if (idRow == 5) {
                    objReturn = {
                        id: idCaracteristica76,
                        idComparativoDetalle: idDet7,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum7Caracteristicas76').val()
                    };
                }
                if (idRow == 6) {
                    objReturn = {
                        id: idCaracteristica77,
                        idComparativoDetalle: idDet7,
                        idRow: idRow + 1,
                        Descripcion: $('#idMarcaNum7Caracteristicas77').val()
                    };
                }
                break;
            default:
                break;
        }

        return objReturn;
    }
    const Parametros = function () {
        let objeto = [
            {
                id: selAutSolicita1.attr('data-id'),
                idAsignacion: idAsignacionp,
                autorizanteID: selAutSolicita1.data("id"),
                autorizanteNombre: selAutSolicita1.val(),
                autorizanteStatus: false,
                autorizanteFinal: false,
                orden: 1
            },
            {
                id: selAutSolicita2.attr('data-id'),
                idAsignacion: idAsignacionp,
                autorizanteID: selAutSolicita2.data("id"),
                autorizanteNombre: selAutSolicita2.val(),
                autorizanteStatus: false,
                autorizanteFinal: false,
                orden: 2
            },
            {
                id: selAutSolicita3.attr('data-id'),
                idAsignacion: idAsignacionp,
                autorizanteID: selAutSolicita3.data("id"),
                autorizanteNombre: selAutSolicita3.val(),
                autorizanteStatus: false,
                autorizanteFinal: false,
                orden: 3
            },
            {
                id: selAutSolicita4.attr('data-id'),
                idAsignacion: idAsignacionp,
                autorizanteID: selAutSolicita4.data("id"),
                autorizanteNombre: selAutSolicita4.val(),
                autorizanteStatus: false,
                autorizanteFinal: false,
                orden: 4
            },
            {
                id: selAutSolicita5.attr('data-id'),
                idAsignacion: idAsignacionp,
                autorizanteID: selAutSolicita5.data("id"),
                autorizanteNombre: selAutSolicita5.val(),
                autorizanteStatus: false,
                autorizanteFinal: true,
                orden: 6
            }
        ];

        return objeto;
    }

    //#region TABLA 
    var Iniciar = function () {
        Asignacion = function () {
            (function init() {
                init_TblM_CargarTabla();
                initdt_ComparativoFinanciero();
                init_AutorizanteAdquisicion();
                init_AutorizanteFinanciero();
                bootGRenta();
            })();
        }
    }
    async function bootGRenta() {
        axios.post('/CatMaquina/CargarSolicitudes', { estado: $("#cboEstado").val(), obra: $('#inputObra').val(), fechaInicio: inputFechaInicio.val(), fechaFin: inputFechaFin.val() })
            .then(response => {
                let { success, datos, message } = response.data;

                if (success) {
                    AddRows(tblAutorizacion, response.data.tblEquiporenta)
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));

        // try {
        //     dtAutorizacion.clear().draw();
        //     response = await ejectFetchJson(originURL('/CatMaquina/CargarSolicitudes?estado=') + $("#cboEstado").val() + '&obra=' + $('#inputObra').val(), {});

        //     if (response.tblEquiporenta != null) {
        //         dtAutorizacion.rows.add(response.tblEquiporenta).draw();
        //     } else {
        //         AlertaGeneral("Aviso", "NO HAY DATOS QUE MOSTRAR.");
        //     }
        // } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
    }
    function AddRows(tbl, lst) {
        dt = tbl.DataTable();
        dt.clear().draw();
        dt.rows.add(lst).draw(false);
    }
    const fcnButons = function () {
        btnBuscar.click(function () {
            bootGRenta();
        });
        btnReporteImprimir.click(function () {
            fncCargarTablaIncidentesPRINT(renta);
        })
    }
    const fncBotonesmodal = function () {
        btnAutorizarFinanciera.click(function () {
            if (ObjParametrosFinanciera.idRow != '' && ObjParametrosFinanciera.idAsignacion != '') {
                AutorizarFinanciera();
            } else {
                AlertaGeneral("Alerta", "Debe seleccionar una columna para autorizar.")
            }
        })
    }
    const init_TblM_CargarTabla = function () {
        dtAutorizacion = tblAutorizacion.DataTable({
            destroy: true,
            language: dtDicEsp,
            columns: [
                {
                    title: 'Solicitud', data: 'noSolicitud', createdCell: (td, data, rowData, row, col) => {
                        if (!rowData.registroCuadro) {
                            if (rowData.estado != 3) {
                                $(td).html(`${data}`);
                                $(td).css('background-color', '#ffff00')
                            } else {
                                $(td).html(`${data}`);
                                $(td).css('background-color', '#0D8E2C')
                            }
                        } else {
                            if (rowData.idAsignacion == 0) {
                                $(td).html(`<button class="btn btn-xs btn-default botonAsignarSolicitud"><i class="fa fa-arrow-right"></i></button>`);
                            }
                        }
                    },
                },
                { title: 'Tipo Solicitud', data: 'TipoSolicitud' },
                { title: 'Descripción', data: 'GrupoEquipo' },
                { title: 'Modelo', data: 'Modelo' },
                { title: 'Fecha En Obra', data: 'FechaPromesa' },
                { title: 'Comentario', data: 'Comentario' },
                { title: 'Obra', data: 'obra' },
                { title: 'Fecha Elaboración Cuadro', data: 'fechaElaboracionCuadroString' },
                { title: 'Fecha Última Autorizacion Cuadro', data: 'fechaUltimaAutorizacionCuadroString' },
                {
                    title: 'Autorizar Cuadro comparativo Financiero', data: 'TipoSolicitud', createdCell: (td, data, rowData, row, col) => {
                        if (data == "COMPRA") {
                            if (rowData.lstFinanciero != 0) {
                                if (rowData.botonFin == 0) {
                                    $(td).html(`<button type='button' class='btn btn-xs btn-primary CuadroComparativoFinanciero' data-idAsignacion='${rowData.idAsignacion}' data-CentroCostos='${rowData.CentroCostos} '><span class='glyphicon glyphicon-check'></span>  </button>`);
                                } else {
                                    $(td).html(`<button type='button' class='btn btn-xs btn-success CuadroComparativoFinanciero' data-idAsignacion='${rowData.idAsignacion}' data-CentroCostos='${rowData.CentroCostos} '><span class='glyphicon glyphicon-check'></span>  </button>`);
                                }
                            } else {
                                $(td).html('');
                            }

                        } else {
                            $(td).html('');

                        }
                    }
                },
                {
                    title: 'Editar Cuadro', render: function (data, type, row, meta) {
                        if (row.cuadroEditable) {
                            return `<button class="btn btn-xs btn-warning botonEditarCuadro"><i class="fa fa-edit"></i></button>`;
                        } else {
                            return ``;
                        }
                    }
                },
                {
                    title: 'Autorizar Cuadro Comparativo', data: 'Comentario', createdCell: (td, data, rowData, row, col) => {
                        if (rowData.lstComparativo != 0) {
                            if (rowData.botonAdq == 0) {
                                $(td).html(`<button type='button' class='btn btn-xs btn-primary Autorizacion' data-idAsignacion='${rowData.idAsignacion}' data-CentroCostos='${rowData.CentroCostos} '><span class='glyphicon glyphicon-check'></span>  </button>`);
                            } else {
                                $(td).html(`<button type='button' class='btn btn-xs btn-success Autorizacion' data-idAsignacion='${rowData.idAsignacion}' data-CentroCostos='${rowData.CentroCostos} '><span class='glyphicon glyphicon-check'></span>  </button>`);
                            }
                        } else {
                            $(td).html('');
                        }
                    }
                },
                {
                    title: 'Reporte Solicitud', render: function (data, type, row, meta) {
                        return row.idSolicitud > 0 ? `
                            <button type="button" class="btn btn-xs btn-primary reporteSolicitud" data-idSolicitud="${row.idSolicitud}" data-CentroCostos="${row.CentroCostos}">
                                <span class="glyphicon glyphicon-print"></span>
                            </button>` : ``;
                    }
                }
            ],
            createdRow: function (row, rowData) {
                if ($('#cboEstado').val() == 0) {
                    if (rowData.estatusCuadro != 3) { //Cuadro no autorizado
                        $(row).addClass('renglonCuadroPendiente');
                    } else { //Cuadro autorizado
                        $(row).addClass('renglonCuadroAutorizado');
                    }
                }
            },
            initComplete: function () {
                tblAutorizacion.on('click', '.botonAsignarSolicitud', function () {
                    let rowData = tblAutorizacion.DataTable().row($(this).closest('tr')).data();

                    _idCuadro = rowData.idCuadro;

                    inputAsignarSolicitud.val('');
                    modalAsignarSolicitud.modal('show');
                });

                tblAutorizacion.on("click", '.reporteSolicitud', function (e) {
                    let rowData = tblAutorizacion.DataTable().row($(this).closest('tr')).data();

                    $.blockUI({ message: 'Procesando...' });
                    $.ajax({
                        url: '/SolicitudEquipo/GetReporte2',
                        type: "POST",
                        datatype: "json",
                        data: { obj: rowData.idSolicitud },
                        success: function (response) {
                            var path = "/Reportes/Vista.aspx?idReporte=12&pCC=" + rowData.CentroCostos;
                            ireport.attr("src", path);
                            document.getElementById('report').onload = function () {
                                $.unblockUI();
                                openCRModal();
                            };
                        },
                        error: function () {
                            $.unblockUI();
                        }
                    });
                });

                tblAutorizacion.on('click', '.botonEditarCuadro', function () {
                    let rowData = tblAutorizacion.DataTable().row($(this).closest('tr')).data();

                    _idCuadro = rowData.idCuadro;

                    btnNuevo.data().id = rowData.idCuadro;

                    mostrarModalCuadroEditar();
                });
            },
            drawCallback: function (settings) {
                $(".Autorizacion").on("click", function (e) {
                    let rowData = dtAutorizacion.row($(this).closest('tr')).data();

                    idAsignacion = $(this).attr('data-idAsignacion');
                    _idCuadro = rowData.idCuadro;
                    dlgFormAdquisicion.dialog("open");

                    $('#dlgFormAdquisicion').css('min-height', '850px');
                    $('#dlgFormAdquisicion').css('height', '100%');

                    CargandoColorColumna(idAsignacion, 0);
                    fncCargarTablaIncidentes(rowData.idCuadro);
                    CargarTablaAutorizanteAdquisicion(rowData.idCuadro);

                    btnAutorizar.data('idCuadro', rowData.idCuadro);

                    if ($(this).attr('data-Compra') == "COMPRA") {
                        renta = true
                    } else {
                        renta = false;
                    }
                });
                $(".CuadroComparativoFinanciero").on("click", function (e) {
                    idAsignacion = $(this).attr('data-idAsignacion');
                    dlgFormFinanciero.dialog("open");
                    $('#dlgFormFinanciero').css('min-height', '850px');
                    $('#dlgFormFinanciero').css('height', '100%');
                    CargarTablaAutorizanteFinanciero();
                    CargandoColorColumna(idAsignacion, 1);
                    getTablaComparativoFinanciero();
                });

                $(".ui-widget-header").css("color", "#fff");
                $(".ui-widget-header").css("background", "linear-gradient(40deg, #45cafc, #303f9f");
                $(".ui-widget-header").css("border", "none");

                $.unblockUI();
            },
            columnDefs: [
                { className: "dt-center", "targets": "_all" }
            ]
        });
    }

    const MostrarOcultar = function () {
        let mostraocultar = false;
        if ($('#btnidMarcaNum1Marca').prop('checked') == true) {
            return mostraocultar = true;
        }
        if ($('#btnidMarcaNum2Marca').prop('checked') == true) {
            return mostraocultar = true;
        }
        if ($('#btnidMarcaNum3Marca').prop('checked') == true) {
            return mostraocultar = true;
        }
        if ($('#btnidMarcaNum4Marca').prop('checked') == true) {
            return mostraocultar = true;
        }
        if ($('#btnidMarcaNum5Marca').prop('checked') == true) {
            return mostraocultar = true;
        }
        if ($('#btnidMarcaNum6Marca').prop('checked') == true) {
            return mostraocultar = true;
        }
        if ($('#btnidMarcaNum7Marca').prop('checked') == true) {
            return mostraocultar = true;
        }

        return mostraocultar;
    }
    const MostrarOcultarFinanciero = function () {
        let mostraocultar = false;
        if ($('#btnAgregarInput1').prop('checked') == true) {
            return mostraocultar = true;
        } if ($('#btnAgregarInput2').prop('checked') == true) {
            return mostraocultar = true;
        } if ($('#btnAgregarInput3').prop('checked') == true) {
            return mostraocultar = true;
        } if ($('#btnAgregarInput4').prop('checked') == true) {
            return mostraocultar = true;
        } if ($('#btnAgregarInput5').prop('checked') == true) {
            return mostraocultar = true;
        } if ($('#btnAgregarInput6').prop('checked') == true) {
            return mostraocultar = true;
        } if ($('#btnAgregarInput7').prop('checked') == true) {
            return mostraocultar = true;
        } if ($('#btnAgregarInput8').prop('checked') == true) {
            return mostraocultar = true;
        } if ($('#btnAgregarInput9').prop('checked') == true) {
            return mostraocultar = true;
        } if ($('#btnAgregarInput10').prop('checked') == true) {
            return mostraocultar = true;
        } if ($('#btnAgregarInput11').prop('checked') == true) {
            return mostraocultar = true;
        } if ($('#btnAgregarInput12').prop('checked') == true) {
            return mostraocultar = true;
        }
        return mostraocultar;
    }

    const fncBtnAutorizar = function () {

        btnAutorizar.click(function () {
            if (ObjParametros.idComparativoDetalle != '' && ObjParametros.idAsignacion != '') {
                Autorizar();
            } else {
                AlertaGeneral("Alerta", "Debe seleccionar una columna para autorizar.")
            }
        });

    }
    const IniciarModal = function () {
        $("#dlgFormAdquisicion").removeClass('hide');
        dlgFormAdquisicion = $("#dlgFormAdquisicion").dialog({
            draggable: false,
            modal: true,
            resizable: true,
            width: "100%",
            height: "100%",
            autoOpen: false,
            position: 'absolute'
        });
        $("#dlgFormFinanciero").removeClass('hide');
        dlgFormFinanciero = $("#dlgFormFinanciero").dialog({
            draggable: false,
            modal: true,
            resizable: true,
            width: "100%",
            height: "100%",
            autoOpen: false,
            position: 'absolute'
        });

        $("#mdlImagen").removeClass('hide');
        mdlImagen = $("#mdlImagen").dialog({
            draggable: false,
            modal: true,
            resizable: true,
            width: "100%",
            height: "100%",
            autoOpen: false,
            position: 'relative'
        });

    }
    let establecerAdquisicion = function (lstAdquisicion) {

        const columnas = [
            {
                data: 'header'
                , render: (data, type, row) => {
                    if (data == 'Caracteristica1' || data == 'Caracteristica2' || data == 'Caracteristica3' || data == 'Caracteristica4' || data == 'Caracteristica5' || data == 'Caracteristica6' || data == 'Caracteristica7') {
                        let html = '';
                        html += '<span id="' + data + '" ></span>';
                        return html;
                    } else if (data == 'Caracteristicas del equipo') {
                        let html = '';
                        html += '<p id=' + 'img' + '>' + 'Evidencia' + '</p>';
                        return html;

                    } else if (data == 'check') {

                        let html = '';
                        html += '';
                        return html;

                    } else {

                        let html = '';
                        html += data;
                        return html;

                    }
                }
            },
            {
                data: 'txtIdnumero1', render: (data, type, row) => {
                    if (data == "btnidMarcaNum1Marca") {
                        let html = '';
                        html += '<h4>Opcion 1</h4><input type="checkbox" id="' + data + '"/>';
                        return html;
                    } else if (data == "idMarcaNum1Caracteristicas") {
                        let html = '';
                        // html += '<img id="idMarcaNum1Caracteristicas" style="30%"/>';
                        html += '<span class="btn btn-primary" id="idMarcaNum1Caracteristicas"><i class="fa fa-download"><i></span>';
                        return html;
                    } else {

                        let html = '';
                        html += '<span id="' + data + '" ></span>';
                        return html;
                    }
                }
            },
            {
                data: 'txtIdnumero2', render: (data, type, row) => {

                    if (data == "btnidMarcaNum2Marca") {
                        let html = '';
                        html += '<h4>Opcion 2</h4><input type="checkbox" id="' + data + '"/>';
                        return html;
                    } else if (data == "idMarcaNum2Caracteristicas") {
                        let html = '';
                        // html += '<img id="idMarcaNum2Caracteristicas" style="30%"/>';
                        html += '<span class="btn btn-primary" id="idMarcaNum2Caracteristicas"><i class="fa fa-download"><i></span>';
                        return html;
                    } else {
                        let html = '';
                        html += '<span id="' + data + '" ></span>';
                        return html;
                    }
                }
            },
            {
                data: 'txtIdnumero3', render: (data, type, row) => {
                    if (data == "btnidMarcaNum3Marca") {
                        let html = '';
                        html += '<h4>Opcion 3</h4><input type="checkbox" id="' + data + '"/>';
                        return html;
                    } else if (data == "idMarcaNum3Caracteristicas") {
                        let html = '';
                        // html += '<img id="idMarcaNum3Caracteristicas" style="30%"/>';
                        html += '<span class="btn btn-primary" id="idMarcaNum3Caracteristicas"><i class="fa fa-download"><i></span>';
                        return html;
                    } else {
                        let html = '';
                        html += '<span id="' + data + '" ></span>';
                        return html;
                    }
                }
            },
            {
                data: 'txtIdnumero4'
                , render: (data, type, row) => {
                    if (data == "btnidMarcaNum4Marca") {
                        let html = '';
                        html += '<h4>Opcion 4</h4><input type="checkbox" id="' + data + '"/>';
                        return html;
                    } else if (data == "idMarcaNum4Caracteristicas") {
                        let html = '';
                        // html += '<img id="idMarcaNum4Caracteristicas" style="30%"/>';
                        html += '<span class="btn btn-primary" id="idMarcaNum4Caracteristicas"><i class="fa fa-download"><i></span>';
                        return html;
                    } else {
                        let html = '';
                        html += '<span id="' + data + '" ></span>';
                        return html;
                    }
                }
            },
            {
                data: 'txtIdnumero5'
                , render: (data, type, row) => {
                    if (data == "btnidMarcaNum5Marca") {
                        let html = '';
                        html += '<h4>Opcion 5</h4><input type="checkbox" id="' + data + '"/>';
                        return html;
                    } else if (data == "idMarcaNum5Caracteristicas") {
                        let html = '';
                        // html += '<img id="idMarcaNum5Caracteristicas" style="30%"/>';
                        html += '<span class="btn btn-primary" id="idMarcaNum5Caracteristicas"><i class="fa fa-download"><i></span>';
                        return html;
                    } else {
                        let html = '';
                        html += '<span id="' + data + '" > </span>';
                        return html;
                    }
                }
            },
            {
                data: 'txtIdnumero6'
                , render: (data, type, row) => {
                    if (data == "btnidMarcaNum6Marca") {
                        let html = '';
                        html += '<h4>Opcion 6</h4><input type="checkbox" id="' + data + '"/>';
                        return html;
                    } else if (data == "idMarcaNum6Caracteristicas") {
                        let html = '';
                        // html += '<img id="idMarcaNum6Caracteristicas" style="30%"/>';
                        html += '<span class="btn btn-primary" id="idMarcaNum6Caracteristicas"><i class="fa fa-download"><i></span>';
                        return html;
                    } else {
                        let html = '';
                        html += '<span id="' + data + '" > </span>';
                        return html;
                    }
                }
            },
            {
                data: 'txtIdnumero7'
                , render: (data, type, row) => {
                    if (data == "btnidMarcaNum7Marca") {
                        let html = '';
                        html += '<h4>Opcion 7</h4><input type="checkbox" id="' + data + '"/>';
                        return html;
                    } else if (data == "idMarcaNum7Caracteristicas") {
                        let html = '';
                        // html += '<img id="idMarcaNum7Caracteristicas" style="30%"/>';
                        html += '<span class="btn btn-primary" id="idMarcaNum7Caracteristicas"><i class="fa fa-download"><i></span>';
                        return html;
                    } else {
                        let html = '';
                        html += '<span id="' + data + '" > </span>';
                        return html;
                    }
                }
            },
        ];

        //  lstAdquisicion.forEach(x => 
        //      columnas.push({ data: x.header , title: x.Item2 })


        //     );

        initDataTblM_AdquisicionyRenta(lstAdquisicion, columnas);
    }
    var initDataTblM_AdquisicionyRenta = function (data, columns) {
        if (dtComaparativo != null) {
            dtComaparativo.clear().destroy();
            tblComparativo.empty();
            dtComaparativo.draw();

        }

        dtComaparativo = tblComparativo.DataTable({
            destroy: true,
            language: dtDicEsp,
            data,
            paging: false,
            ordering: false,
            searching: false,
            bInfo: false,
            order: [[3, "asc"], [1, "asc"]],
            // fixedHeader: true,
            columns,
            scrollX: true,
            scrollY: false,
            scrollCollapse: true,
            columnDefs: [
                { className: "dt-center", "targets": "_all" }
            ]
        });

        // Al agregar columnas estáticas, agregarlas de esta forma para evitar el error: "fixedColumns already initialised on this table"
        // new $.fn.dataTable.FixedColumns(dtComaparativo, {
        //     leftColumns: 1
        // });
    }
    var fncCargarTablaIncidentes = function (idCuadro) {
        if (idCuadro > 0) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/CatMaquina/getTablaComparativoAutorizar",
                data: { objFiltro: { idAsignacion: idAsignacion, idCuadro: _idCuadro } },
                success: function (response) {
                    if (response.success) {
                        //#region getTablaComparativoAdquisicionDetalle
                        $.ajax({
                            datatype: "json",
                            type: "POST",
                            url: "/CatMaquina/getTablaComparativoAdquisicionDetallePorCuadro",
                            data: { idCuadro },
                            success: function (response) {
                                if (response.success) {
                                    formatearLista(response.items);
                                } else {
                                    Alert2Error(response.message);
                                }
                            },
                            error: function () {
                                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                            },
                            complete: function () {
                                $.unblockUI();
                            }
                        });
                        //#endregion

                        establecerAdquisicion(response.items);
                    } else {
                        Alert2Error(response.message);
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        } else {
            let objFiltro = fncGetFiltros();
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/CatMaquina/getTablaComparativoAutorizar",
                data: objFiltro,
                success: function (response) {
                    if (response.success) {

                        getTablaComparativoAdquisicionDetalle();
                        establecerAdquisicion(response.items);
                    } else {
                        Alert2Error(response.message);
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
    }
    var getTablaComparativoAdquisicionDetalle = function () {
        let objFiltro = fncGetFiltros();
        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/CatMaquina/getTablaComparativoAdquisicionDetalle",
            data: objFiltro,
            success: function (response) {
                if (response.success) {
                    formatearLista(response.items);


                } else {
                    Alert2Error(response.message);
                }
            },
            error: function () {
                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            },
            complete: function () {
                $.unblockUI();
            }
        });
    }
    const fncGetFiltros = function () {
        let item = {};
        item = {
            idAsignacion: idAsignacion,
            idCuadro: _idCuadro
        }
        return item;
    }
    const Acomodar = function () {
        // $('#idMarcaNum1Marca').css('width', '210px');
        // $('#idMarcaNum2Marca').css('width', '210px');
        // $('#idMarcaNum3Marca').css('width', '210px');
        // $('#idMarcaNum4Marca').css('width', '210px');
        // $('#idMarcaNum5Marca').css('width', '210px');
        // $('#idMarcaNum6Marca').css('width', '210px');
        // $('#idMarcaNum7Marca').css('width', '210px');
        $('.dataTables_scrollHead').css('display', 'none');
        // $('#tblM_AutorizarAsginacionNoEconomigo_paginate').css('display', 'none');
        // $('#tblM_AutorizarAsginacionNoEconomigo_length').css('display', 'none');
        // $('#tblM_AutorizarAsginacionNoEconomigo_filter').css('display', 'none');
    }
    const formatearLista = function (lstDatos) {
        let color = '#D8DCD8';
        lstDatos.forEach(x => {



            if (x.idRow == 1) {
                idDet1 = 0;
                idDet1 = x.idDet;
                if (x.idDet == objColor.id) {
                    $('#btnidMarcaNum1Marca').parent().css('background-color', color);
                    $('#idMarcaNum1Marca').parent().css('background-color', color);
                    $('#idMarcaNum1proveedor').parent().css('background-color', color);
                    $('#idMarcaNum1precio').parent().css('background-color', color);
                    $('#idMarcaNum1Trade').parent().css('background-color', color);
                    $('#idMarcaNum1Valores').parent().css('background-color', color);
                    $('#idMarcaNum1Precio').parent().css('background-color', color);
                    $('#idMarcaNum1PrecioRoc').parent().css('background-color', color);
                    $('#idMarcaNum1BaseHoras').parent().css('background-color', color);
                    $('#idMarcaNum1Tiempo').parent().css('background-color', color);
                    $('#idMarcaNum1Ubicacion').parent().css('background-color', color);
                    $('#idMarcaNum1Horas').parent().css('background-color', color);
                    $('#idMarcaNum1Seguro').parent().css('background-color', color);
                    $('#idMarcaNum1Garantia').parent().css('background-color', color);
                    $('#idMarcaNum1Servicios').parent().css('background-color', color);
                    $('#idMarcaNum1Capacitacion').parent().css('background-color', color);
                    $('#idMarcaNum1Deposito').parent().css('background-color', color);
                    $('#idMarcaNum1Lugar').parent().css('background-color', color);
                    $('#idMarcaNum1Flete').parent().css('background-color', color);
                    $('#idMarcaNum1Condiciones').parent().css('background-color', color);
                }
                $('#btnidMarcaNum1Marca').attr('data-id', x.idDet);
                $('#btnidMarcaNum1Marca').prop('checked', x.check)
                $('#idMarcaNum1Marca').text(x.marcaModelo);
                $('#idMarcaNum1proveedor').text(x.proveedor)
                $('#idMarcaNum1precio').text(x.precioDeVenta)
                $('#idMarcaNum1Trade').text(x.tradeIn)
                $('#idMarcaNum1Valores').text(x.valoresDeRecompra)
                $('#idMarcaNum1Precio').text(x.precioDeRentaPura)
                $('#idMarcaNum1PrecioRoc').text(x.precioDeRentaEnRoc)
                $('#idMarcaNum1BaseHoras').text(x.baseHoras)
                $('#idMarcaNum1Tiempo').text(x.tiempoDeEntrega)
                $('#idMarcaNum1Ubicacion').text(x.ubicacion)
                $('#idMarcaNum1Horas').text(x.horas)
                $('#idMarcaNum1Seguro').text(x.seguro)
                $('#idMarcaNum1Garantia').text(x.garantia)
                $('#idMarcaNum1Servicios').text(x.serviciosPreventivos)
                $('#idMarcaNum1Capacitacion').text(x.capacitacion)
                $('#idMarcaNum1Deposito').text(x.depositoEnGarantia)
                $('#idMarcaNum1Lugar').text(x.lugarDeEntrega)
                $('#idMarcaNum1Flete').text(x.flete)
                $('#idMarcaNum1Condiciones').text(x.condicionesDePagoEntrega)
                $('#Caracteristica1').text(x.caracteristicasDelEquipo)
                $('#rptprintTipoMoneda').val(x.tipoMoneda)
                // $('#idMarcaNum1Caracteristicas').text(x.rutaArchivo);

                x.lstCaracteristicas.forEach(y => {

                    if (y.idRow == 1) {
                        idCaracteristica1 = 0;
                        idCaracteristica1 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas11').parent().css('background-color', color);
                        }
                        $('#idMarcaNum1Caracteristicas11').text(y.Descripcion)
                    }
                    if (y.idRow == 2) {
                        idCaracteristica2 = 0;
                        idCaracteristica2 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas12').parent().css('background-color', color);
                        }
                        $('#idMarcaNum1Caracteristicas12').text(y.Descripcion)
                    }
                    if (y.idRow == 3) {
                        idCaracteristica3 = 0;
                        idCaracteristica3 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas13').parent().css('background-color', color);
                        }
                        $('#idMarcaNum1Caracteristicas13').text(y.Descripcion)
                    }
                    if (y.idRow == 4) {
                        idCaracteristica4 = 0;
                        idCaracteristica4 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas14').parent().css('background-color', color);
                        }
                        $('#idMarcaNum1Caracteristicas14').text(y.Descripcion)
                    }
                    if (y.idRow == 5) {
                        idCaracteristica5 = 0;
                        idCaracteristica5 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas15').parent().css('background-color', color);
                        }
                        $('#idMarcaNum1Caracteristicas15').text(y.Descripcion)
                    }
                    if (y.idRow == 6) {
                        idCaracteristica6 = 0;
                        idCaracteristica6 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas16').parent().css('background-color', color);
                        }
                        $('#idMarcaNum1Caracteristicas16').text(y.Descripcion)
                    }
                    if (y.idRow == 7) {
                        idCaracteristica7 = 0;
                        idCaracteristica7 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas17').parent().css('background-color', color);
                        }
                        $('#idMarcaNum1Caracteristicas17').text(y.Descripcion)
                    }
                });
            }

            if (x.idRow == 2) {
                idDet2 = 0;
                idDet2 = x.idDet;
                if (x.idDet == objColor.id) {
                    $('#btnidMarcaNum2Marca').parent().css('background-color', color);
                    $('#idMarcaNum2Marca').parent().css('background-color', color);
                    $('#idMarcaNum2proveedor').parent().css('background-color', color);
                    $('#idMarcaNum2precio').parent().css('background-color', color);
                    $('#idMarcaNum2Trade').parent().css('background-color', color);
                    $('#idMarcaNum2Valores').parent().css('background-color', color);
                    $('#idMarcaNum2Precio').parent().css('background-color', color);
                    $('#idMarcaNum2PrecioRoc').parent().css('background-color', color);
                    $('#idMarcaNum2BaseHoras').parent().css('background-color', color);
                    $('#idMarcaNum2Tiempo').parent().css('background-color', color);
                    $('#idMarcaNum2Ubicacion').parent().css('background-color', color);
                    $('#idMarcaNum2Horas').parent().css('background-color', color);
                    $('#idMarcaNum2Seguro').parent().css('background-color', color);
                    $('#idMarcaNum2Garantia').parent().css('background-color', color);
                    $('#idMarcaNum2Servicios').parent().css('background-color', color);
                    $('#idMarcaNum2Capacitacion').parent().css('background-color', color);
                    $('#idMarcaNum2Deposito').parent().css('background-color', color);
                    $('#idMarcaNum2Lugar').parent().css('background-color', color);
                    $('#idMarcaNum2Flete').parent().css('background-color', color);
                    $('#idMarcaNum2Condiciones').parent().css('background-color', color);
                }
                $('#btnidMarcaNum2Marca').attr('data-id', x.idDet);
                $('#btnidMarcaNum2Marca').prop('checked', x.check)
                $('#idMarcaNum2Marca').text(x.marcaModelo)
                $('#idMarcaNum2proveedor').text(x.proveedor)
                $('#idMarcaNum2precio').text(x.precioDeVenta)
                $('#idMarcaNum2Trade').text(x.tradeIn)
                $('#idMarcaNum2Valores').text(x.valoresDeRecompra)
                $('#idMarcaNum2Precio').text(x.precioDeRentaPura)
                $('#idMarcaNum2PrecioRoc').text(x.precioDeRentaEnRoc)
                $('#idMarcaNum2BaseHoras').text(x.baseHoras)
                $('#idMarcaNum2Tiempo').text(x.tiempoDeEntrega)
                $('#idMarcaNum2Ubicacion').text(x.ubicacion)
                $('#idMarcaNum2Horas').text(x.horas)
                $('#idMarcaNum2Seguro').text(x.seguro)
                $('#idMarcaNum2Garantia').text(x.garantia)
                $('#idMarcaNum2Servicios').text(x.serviciosPreventivos)
                $('#idMarcaNum2Capacitacion').text(x.capacitacion)
                $('#idMarcaNum2Deposito').text(x.depositoEnGarantia)
                $('#idMarcaNum2Lugar').text(x.lugarDeEntrega)
                $('#idMarcaNum2Flete').text(x.flete)
                $('#idMarcaNum2Condiciones').text(x.condicionesDePagoEntrega)
                $('#Caracteristica2').text(x.caracteristicasDelEquipo)
                // $('#idMarcaNum2Caracteristicas').text(x.rutaArchivo);
                x.lstCaracteristicas.forEach(y => {

                    if (y.idRow == 1) {
                        idCaracteristica21 = 0;
                        idCaracteristica21 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas21').parent().css('background-color', color);
                        }
                        $('#idMarcaNum2Caracteristicas21').text(y.Descripcion)
                    }
                    if (y.idRow == 2) {
                        idCaracteristica22 = 0;
                        idCaracteristica22 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas22').parent().css('background-color', color);
                        }
                        $('#idMarcaNum2Caracteristicas22').text(y.Descripcion)
                    }
                    if (y.idRow == 3) {
                        idCaracteristica23 = 0;
                        idCaracteristica23 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas23').parent().css('background-color', color);
                        }
                        $('#idMarcaNum2Caracteristicas23').text(y.Descripcion)
                    }
                    if (y.idRow == 4) {
                        idCaracteristica24 = 0;
                        idCaracteristica24 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas24').parent().css('background-color', color);
                        }
                        $('#idMarcaNum2Caracteristicas24').text(y.Descripcion)
                    }
                    if (y.idRow == 5) {
                        idCaracteristica25 = 0;
                        idCaracteristica25 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas25').parent().css('background-color', color);
                        }
                        $('#idMarcaNum2Caracteristicas25').text(y.Descripcion)
                    }
                    if (y.idRow == 6) {
                        idCaracteristica26 = 0;
                        idCaracteristica26 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas26').parent().css('background-color', color);
                        }
                        $('#idMarcaNum2Caracteristicas26').text(y.Descripcion)
                    }
                    if (y.idRow == 7) {
                        idCaracteristica27 = 0;
                        idCaracteristica27 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas27').parent().css('background-color', color);
                        }
                        $('#idMarcaNum2Caracteristicas27').text(y.Descripcion)
                    }
                });

            }
            if (x.idRow == 3) {
                idDet3 = 0;
                idDet3 = x.idDet;
                if (x.idDet == objColor.id) {
                    $('#btnidMarcaNum3Marca').parent().css('background-color', color);
                    $('#idMarcaNum3Marca').parent().css('background-color', color);
                    $('#idMarcaNum3proveedor').parent().css('background-color', color);
                    $('#idMarcaNum3precio').parent().css('background-color', color);
                    $('#idMarcaNum3Trade').parent().css('background-color', color);
                    $('#idMarcaNum3Valores').parent().css('background-color', color);
                    $('#idMarcaNum3Precio').parent().css('background-color', color);
                    $('#idMarcaNum3PrecioRoc').parent().css('background-color', color);
                    $('#idMarcaNum3BaseHoras').parent().css('background-color', color);
                    $('#idMarcaNum3Tiempo').parent().css('background-color', color);
                    $('#idMarcaNum3Ubicacion').parent().css('background-color', color);
                    $('#idMarcaNum3Horas').parent().css('background-color', color);
                    $('#idMarcaNum3Seguro').parent().css('background-color', color);
                    $('#idMarcaNum3Garantia').parent().css('background-color', color);
                    $('#idMarcaNum3Servicios').parent().css('background-color', color);
                    $('#idMarcaNum3Capacitacion').parent().css('background-color', color);
                    $('#idMarcaNum3Deposito').parent().css('background-color', color);
                    $('#idMarcaNum3Lugar').parent().css('background-color', color);
                    $('#idMarcaNum3Flete').parent().css('background-color', color);
                    $('#idMarcaNum3Condiciones').parent().css('background-color', color);
                }
                $('#btnidMarcaNum3Marca').attr('data-id', x.idDet);
                $('#btnidMarcaNum3Marca').prop('checked', x.check)
                $('#idMarcaNum3Marca').text(x.marcaModelo)
                $('#idMarcaNum3proveedor').text(x.proveedor)
                $('#idMarcaNum3precio').text(x.precioDeVenta)
                $('#idMarcaNum3Trade').text(x.tradeIn)
                $('#idMarcaNum3Valores').text(x.valoresDeRecompra)
                $('#idMarcaNum3Precio').text(x.precioDeRentaPura)
                $('#idMarcaNum3PrecioRoc').text(x.precioDeRentaEnRoc)
                $('#idMarcaNum3BaseHoras').text(x.baseHoras)
                $('#idMarcaNum3Tiempo').text(x.tiempoDeEntrega)
                $('#idMarcaNum3Ubicacion').text(x.ubicacion)
                $('#idMarcaNum3Horas').text(x.horas)
                $('#idMarcaNum3Seguro').text(x.seguro)
                $('#idMarcaNum3Garantia').text(x.garantia)
                $('#idMarcaNum3Servicios').text(x.serviciosPreventivos)
                $('#idMarcaNum3Capacitacion').text(x.capacitacion)
                $('#idMarcaNum3Deposito').text(x.depositoEnGarantia)
                $('#idMarcaNum3Lugar').text(x.lugarDeEntrega)
                $('#idMarcaNum3Flete').text(x.flete)
                $('#idMarcaNum3Condiciones').text(x.condicionesDePagoEntrega)
                $('#Caracteristica3').text(x.caracteristicasDelEquipo)
                // $('#idMarcaNum3Caracteristicas').text(x.rutaArchivo);
                x.lstCaracteristicas.forEach(y => {

                    if (y.idRow == 1) {
                        idCaracteristica31 = 0;
                        idCaracteristica31 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas31').parent().css('background-color', color);
                        }
                        $('#idMarcaNum3Caracteristicas31').text(y.Descripcion)
                    }
                    if (y.idRow == 2) {
                        idCaracteristica32 = 0;
                        idCaracteristica32 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas32').parent().css('background-color', color);
                        }
                        $('#idMarcaNum3Caracteristicas32').text(y.Descripcion)
                    }
                    if (y.idRow == 3) {
                        idCaracteristica33 = 0;
                        idCaracteristica33 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas33').parent().css('background-color', color);
                        }
                        $('#idMarcaNum3Caracteristicas33').text(y.Descripcion)
                    }
                    if (y.idRow == 4) {
                        idCaracteristica34 = 0;
                        idCaracteristica34 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas34').parent().css('background-color', color);
                        }
                        $('#idMarcaNum3Caracteristicas34').text(y.Descripcion)
                    }
                    if (y.idRow == 5) {
                        idCaracteristica35 = 0;
                        idCaracteristica35 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas35').parent().css('background-color', color);
                        }
                        $('#idMarcaNum3Caracteristicas35').text(y.Descripcion)
                    }
                    if (y.idRow == 6) {
                        idCaracteristica36 = 0;
                        idCaracteristica36 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas36').parent().css('background-color', color);
                        }
                        $('#idMarcaNum3Caracteristicas36').text(y.Descripcion)
                    }
                    if (y.idRow == 7) {
                        idCaracteristica37 = 0;
                        idCaracteristica37 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas37').parent().css('background-color', color);
                        }
                        $('#idMarcaNum3Caracteristicas37').text(y.Descripcion)
                    }
                });
            }
            if (x.idRow == 4) {


                idDet4 = 0;
                idDet4 = x.idDet;
                if (x.idDet == objColor.id) {
                    $('#btnidMarcaNum4Marca').parent().css('background-color', color);
                    $('#idMarcaNum4Marca').parent().css('background-color', color);
                    $('#idMarcaNum4proveedor').parent().css('background-color', color);
                    $('#idMarcaNum4precio').parent().css('background-color', color);
                    $('#idMarcaNum4Trade').parent().css('background-color', color);
                    $('#idMarcaNum4Valores').parent().css('background-color', color);
                    $('#idMarcaNum4Precio').parent().css('background-color', color);
                    $('#idMarcaNum4PrecioRoc').parent().css('background-color', color);
                    $('#idMarcaNum4BaseHoras').parent().css('background-color', color);
                    $('#idMarcaNum4Tiempo').parent().css('background-color', color);
                    $('#idMarcaNum4Ubicacion').parent().css('background-color', color);
                    $('#idMarcaNum4Horas').parent().css('background-color', color);
                    $('#idMarcaNum4Seguro').parent().css('background-color', color);
                    $('#idMarcaNum4Garantia').parent().css('background-color', color);
                    $('#idMarcaNum4Servicios').parent().css('background-color', color);
                    $('#idMarcaNum4Capacitacion').parent().css('background-color', color);
                    $('#idMarcaNum4Deposito').parent().css('background-color', color);
                    $('#idMarcaNum4Lugar').parent().css('background-color', color);
                    $('#idMarcaNum4Flete').parent().css('background-color', color);
                    $('#idMarcaNum4Condiciones').parent().css('background-color', color);
                }
                $('#btnidMarcaNum4Marca').attr('data-id', x.idDet);
                $('#btnidMarcaNum4Marca').prop('checked', x.check)
                $('#idMarcaNum4Marca').text(x.marcaModelo)
                $('#idMarcaNum4proveedor').text(x.proveedor)
                $('#idMarcaNum4precio').text(x.precioDeVenta)
                $('#idMarcaNum4Trade').text(x.tradeIn)
                $('#idMarcaNum4Valores').text(x.valoresDeRecompra)
                $('#idMarcaNum4Precio').text(x.precioDeRentaPura)
                $('#idMarcaNum4PrecioRoc').text(x.precioDeRentaEnRoc)
                $('#idMarcaNum4BaseHoras').text(x.baseHoras)
                $('#idMarcaNum4Tiempo').text(x.tiempoDeEntrega)
                $('#idMarcaNum4Ubicacion').text(x.ubicacion)
                $('#idMarcaNum4Horas').text(x.horas)
                $('#idMarcaNum4Seguro').text(x.seguro)
                $('#idMarcaNum4Garantia').text(x.garantia)
                $('#idMarcaNum4Servicios').text(x.serviciosPreventivos)
                $('#idMarcaNum4Capacitacion').text(x.capacitacion)
                $('#idMarcaNum4Deposito').text(x.depositoEnGarantia)
                $('#idMarcaNum4Lugar').text(x.lugarDeEntrega)
                $('#idMarcaNum4Flete').text(x.flete)
                $('#idMarcaNum4Condiciones').text(x.condicionesDePagoEntrega)
                $('#Caracteristica4').text(x.caracteristicasDelEquipo)
                // $('#idMarcaNum4Caracteristicas').text(x.rutaArchivo);
                x.lstCaracteristicas.forEach(y => {

                    if (y.idRow == 1) {
                        idCaracteristica41 = 0;
                        idCaracteristica41 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas41').parent().css('background-color', color);
                        }
                        $('#idMarcaNum4Caracteristicas41').text(y.Descripcion)
                    }
                    if (y.idRow == 2) {
                        idCaracteristica42 = 0;
                        idCaracteristica42 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas42').parent().css('background-color', color);
                        }
                        $('#idMarcaNum4Caracteristicas42').text(y.Descripcion)
                    }
                    if (y.idRow == 3) {
                        idCaracteristica43 = 0;
                        idCaracteristica43 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas43').parent().css('background-color', color);
                        }
                        $('#idMarcaNum4Caracteristicas43').text(y.Descripcion)
                    }
                    if (y.idRow == 4) {
                        idCaracteristica44 = 0;
                        idCaracteristica44 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas44').parent().css('background-color', color);
                        }
                        $('#idMarcaNum4Caracteristicas44').text(y.Descripcion)
                    }
                    if (y.idRow == 5) {
                        idCaracteristica45 = 0;
                        idCaracteristica45 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas45').parent().css('background-color', color);
                        }
                        $('#idMarcaNum4Caracteristicas45').text(y.Descripcion)
                    }
                    if (y.idRow == 6) {
                        idCaracteristica46 = 0;
                        idCaracteristica46 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas46').parent().css('background-color', color);
                        }
                        $('#idMarcaNum4Caracteristicas46').text(y.Descripcion)
                    }
                    if (y.idRow == 7) {
                        idCaracteristica47 = 0;
                        idCaracteristica47 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas47').parent().css('background-color', color);
                        }
                        $('#idMarcaNum4Caracteristicas47').text(y.Descripcion)
                    }
                });
            }
            if (x.idRow == 5) {


                idDet5 = 0;
                idDet5 = x.idDet;
                if (x.idDet == objColor.id) {
                    $('#btnidMarcaNum5Marca').parent().css('background-color', color);
                    $('#idMarcaNum5Marca').parent().css('background-color', color);
                    $('#idMarcaNum5proveedor').parent().css('background-color', color);
                    $('#idMarcaNum5precio').parent().css('background-color', color);
                    $('#idMarcaNum5Trade').parent().css('background-color', color);
                    $('#idMarcaNum5Valores').parent().css('background-color', color);
                    $('#idMarcaNum5Precio').parent().css('background-color', color);
                    $('#idMarcaNum5PrecioRoc').parent().css('background-color', color);
                    $('#idMarcaNum5BaseHoras').parent().css('background-color', color);
                    $('#idMarcaNum5Tiempo').parent().css('background-color', color);
                    $('#idMarcaNum5Ubicacion').parent().css('background-color', color);
                    $('#idMarcaNum5Horas').parent().css('background-color', color);
                    $('#idMarcaNum5Seguro').parent().css('background-color', color);
                    $('#idMarcaNum5Garantia').parent().css('background-color', color);
                    $('#idMarcaNum5Servicios').parent().css('background-color', color);
                    $('#idMarcaNum5Capacitacion').parent().css('background-color', color);
                    $('#idMarcaNum5Deposito').parent().css('background-color', color);
                    $('#idMarcaNum5Lugar').parent().css('background-color', color);
                    $('#idMarcaNum5Flete').parent().css('background-color', color);
                    $('#idMarcaNum5Condiciones').parent().css('background-color', color);
                }
                $('#btnidMarcaNum5Marca').attr('data-id', x.idDet);
                $('#btnidMarcaNum5Marca').prop('checked', x.check)
                $('#idMarcaNum5Marca').text(x.marcaModelo)
                $('#idMarcaNum5proveedor').text(x.proveedor)
                $('#idMarcaNum5precio').text(x.precioDeVenta)
                $('#idMarcaNum5Trade').text(x.tradeIn)
                $('#idMarcaNum5Valores').text(x.valoresDeRecompra)
                $('#idMarcaNum5Precio').text(x.precioDeRentaPura)
                $('#idMarcaNum5PrecioRoc').text(x.precioDeRentaEnRoc)
                $('#idMarcaNum5BaseHoras').text(x.baseHoras)
                $('#idMarcaNum5Tiempo').text(x.tiempoDeEntrega)
                $('#idMarcaNum5Ubicacion').text(x.ubicacion)
                $('#idMarcaNum5Horas').text(x.horas)
                $('#idMarcaNum5Seguro').text(x.seguro)
                $('#idMarcaNum5Garantia').text(x.garantia)
                $('#idMarcaNum5Servicios').text(x.serviciosPreventivos)
                $('#idMarcaNum5Capacitacion').text(x.capacitacion)
                $('#idMarcaNum5Deposito').text(x.depositoEnGarantia)
                $('#idMarcaNum5Lugar').text(x.lugarDeEntrega)
                $('#idMarcaNum5Flete').text(x.flete)
                $('#idMarcaNum5Condiciones').text(x.condicionesDePagoEntrega)
                $('#Caracteristica5').text(x.caracteristicasDelEquipo)
                // $('#idMarcaNum5Caracteristicas').text(x.rutaArchivo);
                x.lstCaracteristicas.forEach(y => {

                    if (y.idRow == 1) {
                        idCaracteristica51 = 0;
                        idCaracteristica51 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas51').parent().css('background-color', color);
                        }
                        $('#idMarcaNum5Caracteristicas51').text(y.Descripcion)
                    }
                    if (y.idRow == 2) {
                        idCaracteristica52 = 0;
                        idCaracteristica52 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas52').parent().css('background-color', color);
                        }
                        $('#idMarcaNum5Caracteristicas52').text(y.Descripcion)
                    }
                    if (y.idRow == 3) {
                        idCaracteristica53 = 0;
                        idCaracteristica53 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas53').parent().css('background-color', color);
                        }
                        $('#idMarcaNum5Caracteristicas53').text(y.Descripcion)
                    }
                    if (y.idRow == 4) {
                        idCaracteristica54 = 0;
                        idCaracteristica54 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas54').parent().css('background-color', color);
                        }
                        $('#idMarcaNum5Caracteristicas54').text(y.Descripcion)
                    }
                    if (y.idRow == 5) {
                        idCaracteristica55 = 0;
                        idCaracteristica55 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas55').parent().css('background-color', color);
                        }
                        $('#idMarcaNum5Caracteristicas55').text(y.Descripcion)
                    }
                    if (y.idRow == 6) {
                        idCaracteristica56 = 0;
                        idCaracteristica56 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas56').parent().css('background-color', color);
                        }
                        $('#idMarcaNum5Caracteristicas56').text(y.Descripcion)
                    }
                    if (y.idRow == 7) {
                        idCaracteristica57 = 0;
                        idCaracteristica57 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas57').parent().css('background-color', color);
                        }
                        $('#idMarcaNum5Caracteristicas57').text(y.Descripcion)
                    }
                });

            }

            if (x.idRow == 6) {


                idDet6 = 0;
                idDet6 = x.idDet;
                if (x.idDet == objColor.id) {
                    $('#btnidMarcaNum6Marca').parent().css('background-color', color);
                    $('#idMarcaNum6Marca').parent().css('background-color', color);
                    $('#idMarcaNum6proveedor').parent().css('background-color', color);
                    $('#idMarcaNum6precio').parent().css('background-color', color);
                    $('#idMarcaNum6Trade').parent().css('background-color', color);
                    $('#idMarcaNum6Valores').parent().css('background-color', color);
                    $('#idMarcaNum6Precio').parent().css('background-color', color);
                    $('#idMarcaNum6PrecioRoc').parent().css('background-color', color);
                    $('#idMarcaNum6BaseHoras').parent().css('background-color', color);
                    $('#idMarcaNum6Tiempo').parent().css('background-color', color);
                    $('#idMarcaNum6Ubicacion').parent().css('background-color', color);
                    $('#idMarcaNum6Horas').parent().css('background-color', color);
                    $('#idMarcaNum6Seguro').parent().css('background-color', color);
                    $('#idMarcaNum6Garantia').parent().css('background-color', color);
                    $('#idMarcaNum6Servicios').parent().css('background-color', color);
                    $('#idMarcaNum6Capacitacion').parent().css('background-color', color);
                    $('#idMarcaNum6Deposito').parent().css('background-color', color);
                    $('#idMarcaNum6Lugar').parent().css('background-color', color);
                    $('#idMarcaNum6Flete').parent().css('background-color', color);
                    $('#idMarcaNum6Condiciones').parent().css('background-color', color);
                }
                $('#btnidMarcaNum6Marca').attr('data-id', x.idDet);
                $('#btnidMarcaNum6Marca').prop('checked', x.check)
                $('#idMarcaNum6Marca').text(x.marcaModelo)
                $('#idMarcaNum6proveedor').text(x.proveedor)
                $('#idMarcaNum6precio').text(x.precioDeVenta)
                $('#idMarcaNum6Trade').text(x.tradeIn)
                $('#idMarcaNum6Valores').text(x.valoresDeRecompra)
                $('#idMarcaNum6Precio').text(x.precioDeRentaPura)
                $('#idMarcaNum6PrecioRoc').text(x.precioDeRentaEnRoc)
                $('#idMarcaNum6BaseHoras').text(x.baseHoras)
                $('#idMarcaNum6Tiempo').text(x.tiempoDeEntrega)
                $('#idMarcaNum6Ubicacion').text(x.ubicacion)
                $('#idMarcaNum6Horas').text(x.horas)
                $('#idMarcaNum6Seguro').text(x.seguro)
                $('#idMarcaNum6Garantia').text(x.garantia)
                $('#idMarcaNum6Servicios').text(x.serviciosPreventivos)
                $('#idMarcaNum6Capacitacion').text(x.capacitacion)
                $('#idMarcaNum6Deposito').text(x.depositoEnGarantia)
                $('#idMarcaNum6Lugar').text(x.lugarDeEntrega)
                $('#idMarcaNum6Flete').text(x.flete)
                $('#idMarcaNum6Condiciones').text(x.condicionesDePagoEntrega)
                $('#Caracteristica6').text(x.caracteristicasDelEquipo)
                // $('#idMarcaNum6Caracteristicas').text(x.rutaArchivo);
                x.lstCaracteristicas.forEach(y => {

                    if (y.idRow == 1) {
                        idCaracteristica61 = 0;
                        idCaracteristica61 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas61').parent().css('background-color', color);
                        }
                        $('#idMarcaNum6Caracteristicas61').text(y.Descripcion)
                    }
                    if (y.idRow == 2) {
                        idCaracteristica62 = 0;
                        idCaracteristica62 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas62').parent().css('background-color', color);
                        }
                        $('#idMarcaNum6Caracteristicas62').text(y.Descripcion)
                    }
                    if (y.idRow == 3) {
                        idCaracteristica63 = 0;
                        idCaracteristica63 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas63').parent().css('background-color', color);
                        }
                        $('#idMarcaNum6Caracteristicas63').text(y.Descripcion)
                    }
                    if (y.idRow == 4) {
                        idCaracteristica64 = 0;
                        idCaracteristica64 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas64').parent().css('background-color', color);
                        }
                        $('#idMarcaNum6Caracteristicas64').text(y.Descripcion)
                    }
                    if (y.idRow == 5) {
                        idCaracteristica65 = 0;
                        idCaracteristica65 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas65').parent().css('background-color', color);
                        }
                        $('#idMarcaNum6Caracteristicas65').text(y.Descripcion)
                    }
                    if (y.idRow == 6) {
                        idCaracteristica66 = 0;
                        idCaracteristica66 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas66').parent().css('background-color', color);
                        }
                        $('#idMarcaNum6Caracteristicas66').text(y.Descripcion)
                    }
                    if (y.idRow == 7) {
                        idCaracteristica67 = 0;
                        idCaracteristica67 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas67').parent().css('background-color', color);
                        }
                        $('#idMarcaNum6Caracteristicas67').text(y.Descripcion)
                    }
                });
            }
            if (x.idRow == 7) {

                idDet7 = 0;
                idDet7 = x.idDet;
                if (x.idDet == objColor.id) {
                    $('#btnidMarcaNum7Marca').parent().css('background-color', color);
                    $('#idMarcaNum7Marca').parent().css('background-color', color);
                    $('#idMarcaNum7proveedor').parent().css('background-color', color);
                    $('#idMarcaNum7precio').parent().css('background-color', color);
                    $('#idMarcaNum7Trade').parent().css('background-color', color);
                    $('#idMarcaNum7Valores').parent().css('background-color', color);
                    $('#idMarcaNum7Precio').parent().css('background-color', color);
                    $('#idMarcaNum7PrecioRoc').parent().css('background-color', color);
                    $('#idMarcaNum7BaseHoras').parent().css('background-color', color);
                    $('#idMarcaNum7Tiempo').parent().css('background-color', color);
                    $('#idMarcaNum7Ubicacion').parent().css('background-color', color);
                    $('#idMarcaNum7Horas').parent().css('background-color', color);
                    $('#idMarcaNum7Seguro').parent().css('background-color', color);
                    $('#idMarcaNum7Garantia').parent().css('background-color', color);
                    $('#idMarcaNum7Servicios').parent().css('background-color', color);
                    $('#idMarcaNum7Capacitacion').parent().css('background-color', color);
                    $('#idMarcaNum7Deposito').parent().css('background-color', color);
                    $('#idMarcaNum7Lugar').parent().css('background-color', color);
                    $('#idMarcaNum7Flete').parent().css('background-color', color);
                    $('#idMarcaNum7Condiciones').parent().css('background-color', color);
                }
                $('#btnidMarcaNum7Marca').attr('data-id', x.idDet);
                $('#btnidMarcaNum7Marca').prop('checked', x.check)
                $('#idMarcaNum7Marca').text(x.marcaModelo)
                $('#idMarcaNum7proveedor').text(x.proveedor)
                $('#idMarcaNum7precio').text(x.precioDeVenta)
                $('#idMarcaNum7Trade').text(x.tradeIn)
                $('#idMarcaNum7Valores').text(x.valoresDeRecompra)
                $('#idMarcaNum7Precio').text(x.precioDeRentaPura)
                $('#idMarcaNum7PrecioRoc').text(x.precioDeRentaEnRoc)
                $('#idMarcaNum7BaseHoras').text(x.baseHoras)
                $('#idMarcaNum7Tiempo').text(x.tiempoDeEntrega)
                $('#idMarcaNum7Ubicacion').text(x.ubicacion)
                $('#idMarcaNum7Horas').text(x.horas)
                $('#idMarcaNum7Seguro').text(x.seguro)
                $('#idMarcaNum7Garantia').text(x.garantia)
                $('#idMarcaNum7Servicios').text(x.serviciosPreventivos)
                $('#idMarcaNum7Capacitacion').text(x.capacitacion)
                $('#idMarcaNum7Deposito').text(x.depositoEnGarantia)
                $('#idMarcaNum7Lugar').text(x.lugarDeEntrega)
                $('#idMarcaNum7Flete').text(x.flete)
                $('#idMarcaNum7Condiciones').text(x.condicionesDePagoEntrega)
                $('#Caracteristica7').text(x.caracteristicasDelEquipo)
                // $('#idMarcaNum7Caracteristicas').text(x.rutaArchivo);
                x.lstCaracteristicas.forEach(y => {

                    if (y.idRow == 1) {
                        idCaracteristica71 = 0;
                        idCaracteristica71 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas71').parent().css('background-color', color);
                        }
                        $('#idMarcaNum7Caracteristicas71').text(y.Descripcion)
                    }
                    if (y.idRow == 2) {
                        idCaracteristica72 = 0;
                        idCaracteristica72 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas72').parent().css('background-color', color);
                        }
                        $('#idMarcaNum7Caracteristicas72').text(y.Descripcion)
                    }
                    if (y.idRow == 3) {
                        idCaracteristica73 = 0;
                        idCaracteristica73 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas73').parent().css('background-color', color);
                        }
                        $('#idMarcaNum7Caracteristicas73').text(y.Descripcion)
                    }
                    if (y.idRow == 4) {
                        idCaracteristica74 = 0;
                        idCaracteristica74 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas74').parent().css('background-color', color);
                        }
                        $('#idMarcaNum7Caracteristicas74').text(y.Descripcion)
                    }
                    if (y.idRow == 5) {
                        idCaracteristica75 = 0;
                        idCaracteristica75 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas75').parent().css('background-color', color);
                        }
                        $('#idMarcaNum7Caracteristicas75').text(y.Descripcion)
                    }
                    if (y.idRow == 6) {
                        idCaracteristica76 = 0;
                        idCaracteristica76 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas76').parent().css('background-color', color);
                        }
                        $('#idMarcaNum7Caracteristicas76').text(y.Descripcion)
                    }
                    if (y.idRow == 7) {
                        idCaracteristica77 = 0;
                        idCaracteristica77 = y.id;
                        if (x.idDet == objColor.id) {
                            $('#idMarcaNum1Caracteristicas77').parent().css('background-color', color);
                        }
                        $('#idMarcaNum7Caracteristicas77').text(y.Descripcion)
                    }
                });
            }



        });
        // $('#idMarcaNum1Caracteristicas').attr('style','width:30%');
        // $('#idMarcaNum2Caracteristicas').attr('style','width:30%');
        // $('#idMarcaNum3Caracteristicas').attr('style','width:30%');
        // $('#idMarcaNum4Caracteristicas').attr('style','width:30%');
        // $('#idMarcaNum5Caracteristicas').attr('style','width:30%');
        // $('#idMarcaNum6Caracteristicas').attr('style','width:30%');
        // $('#idMarcaNum7Caracteristicas').attr('style','width:30%');
        inputImagenClick();
        let numeroDeListas = lstDatos.length;
        for (let i = 0; i < 8; i++) {
            if (i <= numeroDeListas) {
            } else {
                var table = $('#tblM_ComparativoAdquisicionyRenta').DataTable();
                table.columns([i]).visible(false);
            }
        }
        // $('.dt-center').css('min-width', '210px');
        $('#tblM_ComparativoAdquisicionyRenta').find('td').css('padding', 0)
        Acomodar();
        CambiarCheck();

        let tr = $('#tblM_ComparativoAdquisicionyRenta').find('tr')
        for (let index = 0; index < tr.length; index++) {
            let td = $(tr[index]).find('td')
            $(td[0]).css('font-weight', 'bold')
        }


        if (MostrarOcultar() == true) {
            btnAutorizar.css('visibility', 'hidden')
        } else {
            btnAutorizar.css('visibility', 'visible')
        }
    }
    function descargarimagen(examen_id) {

        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/CatMaquina/DescargarArchivo?examen_id=" + examen_id,
            success: function (response) {
                if (response.success) {
                    AlertaGeneral('Alerta', 'No tiene ninguna imagen o archivo de evidencia');
                } else {
                    location.href = `DescargarArchivo?examen_id=${examen_id}`;
                }
            },
            error: function () {
                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            },
            complete: function () {
                $.unblockUI();
            }
        });

    }
    const inputImagenClick = function () {

        $('#idMarcaNum1Caracteristicas').click(function () {
            // mdlImagen.dialog("open");
            // $('#mdlImagen').css('min-height', '850px');
            // $('#mdlImagen').css('height', '60%');
            // $('#MostrarImagen').attr('src',$('#idMarcaNum1Caracteristicas').attr('src'));
            // $('#MostrarImagen').attr('style','width:80%');
            var examen_id = $('#btnidMarcaNum1Marca').attr('data-id');
            descargarimagen(examen_id)

        });
        $('#idMarcaNum2Caracteristicas').click(function () {
            // mdlImagen.dialog("open");
            // $('#mdlImagen').css('min-height', '850px');
            // $('#mdlImagen').css('height', '60%');
            // $('#MostrarImagen').attr('src',$('#idMarcaNum2Caracteristicas').attr('src'));
            var examen_id = $('#btnidMarcaNum2Marca').attr('data-id');
            descargarimagen(examen_id);
        });
        $('#idMarcaNum3Caracteristicas').click(function () {
            // mdlImagen.dialog("open");
            // $('#mdlImagen').css('min-height', '850px');
            // $('#mdlImagen').css('height', '60%');
            // $('#MostrarImagen').attr('src',$('#idMarcaNum3Caracteristicas').attr('src'));
            // $('#MostrarImagen').attr('style','width:80%');
            var examen_id = $('#btnidMarcaNum3Marca').attr('data-id');
            descargarimagen(examen_id);
        });
        $('#idMarcaNum4Caracteristicas').click(function () {
            // mdlImagen.dialog("open");
            // $('#mdlImagen').css('min-height', '850px');
            // $('#mdlImagen').css('height', '60%');
            // $('#MostrarImagen').attr('src',$('#idMarcaNum4Caracteristicas').attr('src'));
            // $('#MostrarImagen').attr('style','width:80%');
            var examen_id = $('#btnidMarcaNum4Marca').attr('data-id');
            descargarimagen(examen_id);
        });
        $('#idMarcaNum5Caracteristicas').click(function () {
            // mdlImagen.dialog("open");
            // $('#mdlImagen').css('min-height', '850px');
            // $('#mdlImagen').css('height', '60%');
            // $('#MostrarImagen').attr('src',$('#idMarcaNum5Caracteristicas').attr('src'));
            // $('#MostrarImagen').attr('style','width:80%');
            var examen_id = $('#btnidMarcaNum5Marca').attr('data-id');
            descargarimagen(examen_id);
        });
        $('#idMarcaNum6Caracteristicas').click(function () {
            // mdlImagen.dialog("open");
            // $('#mdlImagen').css('min-height', '850px');
            // $('#mdlImagen').css('height', '60%');
            // $('#MostrarImagen').attr('src',$('#idMarcaNum6Caracteristicas').attr('src'));
            // $('#MostrarImagen').attr('style','width:80%');
            var examen_id = $('#btnidMarcaNum6Marca').attr('data-id');
            descargarimagen(examen_id);
        });
        $('#idMarcaNum7Caracteristicas').click(function () {
            // mdlImagen.dialog("open");
            // $('#mdlImagen').css('min-height', '850px');
            // $('#mdlImagen').css('height', '60%');
            // $('#MostrarImagen').attr('src',$('#idMarcaNum7Caracteristicas').attr('src'));
            // $('#MostrarImagen').attr('style','width:80%');
            var examen_id = $('#btnidMarcaNum7Marca').attr('data-id');
            descargarimagen(examen_id);
        });




    }
    const CambiarCheck = function () {
        $('#btnidMarcaNum1Marca').click(function (e) {
            if ($('#btnidMarcaNum1Marca').prop('checked') == true) {
                $('#btnidMarcaNum2Marca').prop('checked', false);
                $('#btnidMarcaNum3Marca').prop('checked', false);
                $('#btnidMarcaNum4Marca').prop('checked', false);
                $('#btnidMarcaNum5Marca').prop('checked', false);
                $('#btnidMarcaNum6Marca').prop('checked', false);
                $('#btnidMarcaNum7Marca').prop('checked', false);
                LlenarObjetoParametros(idDet1, idAsignacion);
            }
        });
        $('#btnidMarcaNum2Marca').click(function (e) {
            if ($('#btnidMarcaNum2Marca').prop('checked') == true) {
                $('#btnidMarcaNum1Marca').prop('checked', false);
                $('#btnidMarcaNum3Marca').prop('checked', false);
                $('#btnidMarcaNum4Marca').prop('checked', false);
                $('#btnidMarcaNum5Marca').prop('checked', false);
                $('#btnidMarcaNum6Marca').prop('checked', false);
                $('#btnidMarcaNum7Marca').prop('checked', false);
                LlenarObjetoParametros(idDet2, idAsignacion);
            }
        });

        $('#btnidMarcaNum3Marca').click(function (e) {
            if ($('#btnidMarcaNum3Marca').prop('checked') == true) {
                $('#btnidMarcaNum2Marca').prop('checked', false);
                $('#btnidMarcaNum1Marca').prop('checked', false);
                $('#btnidMarcaNum4Marca').prop('checked', false);
                $('#btnidMarcaNum5Marca').prop('checked', false);
                $('#btnidMarcaNum6Marca').prop('checked', false);
                $('#btnidMarcaNum7Marca').prop('checked', false);
                LlenarObjetoParametros(idDet3, idAsignacion);
            }
        });
        $('#btnidMarcaNum4Marca').click(function (e) {
            if ($('#btnidMarcaNum4Marca').prop('checked') == true) {
                $('#btnidMarcaNum2Marca').prop('checked', false);
                $('#btnidMarcaNum3Marca').prop('checked', false);
                $('#btnidMarcaNum1Marca').prop('checked', false);
                $('#btnidMarcaNum5Marca').prop('checked', false);
                $('#btnidMarcaNum6Marca').prop('checked', false);
                $('#btnidMarcaNum7Marca').prop('checked', false);
                LlenarObjetoParametros(idDet4, idAsignacion);
            }
        });
        $('#btnidMarcaNum5Marca').click(function (e) {
            if ($('#btnidMarcaNum5Marca').prop('checked') == true) {
                $('#btnidMarcaNum2Marca').prop('checked', false);
                $('#btnidMarcaNum3Marca').prop('checked', false);
                $('#btnidMarcaNum4Marca').prop('checked', false);
                $('#btnidMarcaNum1Marca').prop('checked', false);
                $('#btnidMarcaNum6Marca').prop('checked', false);
                $('#btnidMarcaNum7Marca').prop('checked', false);
                LlenarObjetoParametros(idDet5, idAsignacion);
            }
        });
        $('#btnidMarcaNum6Marca').click(function (e) {
            if ($('#btnidMarcaNum6Marca').prop('checked') == true) {
                $('#btnidMarcaNum2Marca').prop('checked', false);
                $('#btnidMarcaNum3Marca').prop('checked', false);
                $('#btnidMarcaNum4Marca').prop('checked', false);
                $('#btnidMarcaNum5Marca').prop('checked', false);
                $('#btnidMarcaNum1Marca').prop('checked', false);
                $('#btnidMarcaNum7Marca').prop('checked', false);
                LlenarObjetoParametros(idDet6, idAsignacion);
            }
        });
        $('#btnidMarcaNum7Marca').click(function (e) {
            if ($('#btnidMarcaNum7Marca').prop('checked') == true) {
                $('#btnidMarcaNum2Marca').prop('checked', false);
                $('#btnidMarcaNum3Marca').prop('checked', false);
                $('#btnidMarcaNum4Marca').prop('checked', false);
                $('#btnidMarcaNum5Marca').prop('checked', false);
                $('#btnidMarcaNum6Marca').prop('checked', false);
                $('#btnidMarcaNum1Marca').prop('checked', false);
                LlenarObjetoParametros(idDet7, idAsignacion);
            }
        });
    }
    const LlenarObjetoParametros = function (idComparativoDetalle, idAsignacion) {
        ObjParametros = {};
        ObjParametros = {
            idComparativoDetalle: idComparativoDetalle,
            idAsignacion: idAsignacion
        }
    }
    var Autorizar = function () {
        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/CatMaquina/AutorizandoComparativo?idComparativoDetalle=" + ObjParametros.idComparativoDetalle + "&idAsignacion=" + ObjParametros.idAsignacion + "&idCuadro=" + btnAutorizar.data().idCuadro,
            success: function (response) {
                if (response.success) {
                    if (response.items.estatusExito == 2) {
                        dtComaparativo.clear();
                        dtComaparativo.draw();
                        ObjParametros = {
                            idComparativoDetalle: '',
                            idAsignacion: ''
                        }
                        dlgFormAdquisicion.dialog("close");
                        bootGRenta();
                    } else {
                        AlertaGeneral("Alerta", response.items.msjExito);
                    }
                } else {
                    Alert2Error(response.message);
                }
            },
            error: function () {
                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            },
            complete: function () {
                $.unblockUI();
            }
        });
    }
    var AutorizarFinanciera = function () {

        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/CatMaquina/AutorizandoComparativoFinanciera?idRow=" + ObjParametrosFinanciera.idRow + "&idAsignacion=" + ObjParametrosFinanciera.idAsignacion + "",
            success: function (response) {
                if (response.success) {
                    if (response.items.estatusExito == 2) {
                        dtFinanciera.clear();
                        dtFinanciera.draw();
                        ObjParametrosFinanciera = {
                            idRow: '',
                            idAsignacion: ''
                        }
                        dlgFormFinanciero.dialog("close");
                        bootGRenta();
                    } else {
                        AlertaGeneral("Alerta", response.items.msjExito);
                    }

                } else {
                    Alert2Error(response.message);
                }
            },
            error: function () {
                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            },
            complete: function () {
                $.unblockUI();
            }
        });
    }
    const CargandoColorColumna = function (id, Tipo) {
        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/CatMaquina/indicadorColumnaMaximoVoto?idAsignacion=" + id + "&Tipo=" + Tipo,
            success: function (response) {
                if (response.success) {
                    objColor = {
                        id: response.items.id,
                        color: response.items.votoMayor
                    }
                } else {
                    Alert2Error(response.message);
                }
            },
            error: function () {
                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            },
            complete: function () {
                $.unblockUI();
            }
        });
    }
    var initdt_ComparativoFinanciero = function () {
        dtFinanciera = tblFinanciera.DataTable({
            destroy: true,
            ordering: false,
            language: dtDicEsp,
            paging: false,
            scrollX: false,
            scrollY: true,
            width: "50%",
            columns: [
                {
                    data: 'header', title: 'header'
                    , render: (data, type, row) => {
                        let html = "";
                        if (data == "botones") {
                            html = '';
                        } else if (data == "Selector") {
                            html = 'Plazos';
                        } else {
                            html = data;
                        }
                        return html;
                    }
                },
                {
                    data: 'txtIdnumero1', title: 'txtIdnumero1'
                    , render: (data, type, row) => {

                        let html = "";
                        if (data == "btnAgregarInput1") {
                            let html = '';
                            html += '<h4>Opcion 1</h4> <input type="checkbox" id="' + data + '"/>';
                            return html;
                        } else {
                            html = '<div id=' + data + '></div>';
                        }
                        return html;
                    }
                },
                {
                    data: 'txtIdnumero2', title: 'txtIdnumero2'
                    , render: (data, type, row) => {
                        if (data == "btnAgregarInput2") {
                            let html = '';
                            html += '<h4>Opcion 2</h4> <input type="checkbox" id="' + data + '"/>';
                            return html;
                        } else {
                            let html = "";
                            html = '<div id=' + data + '></div>';
                            return html;
                        }
                    }
                },
                {
                    data: 'txtIdnumero3', title: 'txtIdnumero3'
                    , render: (data, type, row) => {
                        if (data == "btnAgregarInput3") {
                            let html = '';
                            html += '<h4>Opcion 3</h4> <input type="checkbox" id="' + data + '"/>';
                            return html;
                        } else {
                            let html = "";

                            html = '<div id=' + data + '></div>';
                            return html;
                        }
                    }
                },
                {
                    data: 'txtIdnumero4', title: 'txtIdnumero3'
                    , render: (data, type, row) => {
                        if (data == "btnAgregarInput4") {
                            let html = '';
                            html += '<h4>Opcion 4</h4> <input type="checkbox" id="' + data + '"/>';
                            return html;
                        } else {
                            let html = "";

                            html = '<div id=' + data + '></div>';
                            return html;
                        }
                    }
                },
                {
                    data: 'txtIdnumero5', title: 'txtIdnumero3'
                    , render: (data, type, row) => {
                        if (data == "btnAgregarInput5") {
                            let html = '';
                            html += '<h4>Opcion 5</h4> <input type="checkbox" id="' + data + '"/>';
                            return html;
                        } else {
                            let html = "";

                            html = '<div id=' + data + '></div>';
                            return html;
                        }
                    }
                },
                {
                    data: 'txtIdnumero6', title: 'txtIdnumero3'
                    , render: (data, type, row) => {
                        if (data == "btnAgregarInput6") {
                            let html = '';
                            html += '<h4>Opcion 6</h4> <input type="checkbox" id="' + data + '"/>';
                            return html;
                        } else {
                            let html = "";

                            html = '<div id=' + data + '></div>';
                            return html;
                        }
                    }
                },
                {
                    data: 'txtIdnumero7', title: 'txtIdnumero3'
                    , render: (data, type, row) => {
                        if (data == "btnAgregarInput7") {
                            let html = '';
                            html += '<h4>Opcion 7</h4> <input type="checkbox" id="' + data + '"/>';
                            return html;
                        } else {
                            let html = "";

                            html = '<div id=' + data + '></div>';
                            return html;
                        }
                    }
                },
                {
                    data: 'txtIdnumero8', title: 'txtIdnumero3'
                    , render: (data, type, row) => {
                        if (data == "btnAgregarInput8") {
                            let html = '';
                            html += '<h4>Opcion 8</h4> <input type="checkbox" id="' + data + '"/>';
                            return html;
                        } else {
                            let html = "";

                            html = '<div id=' + data + '></div>';
                            return html;
                        }
                    }
                },
                {
                    data: 'txtIdnumero9', title: 'txtIdnumero3'
                    , render: (data, type, row) => {
                        if (data == "btnAgregarInput9") {
                            let html = '';
                            html += '<h4>Opcion 9</h4> <input type="checkbox" id="' + data + '"/>';
                            return html;
                        } else {
                            let html = "";

                            html = '<div id=' + data + '></div>';
                            return html;
                        }
                    }
                },
                {
                    data: 'txtIdnumero10', title: 'txtIdnumero3'
                    , render: (data, type, row) => {
                        if (data == "btnAgregarInput10") {
                            let html = '';
                            html += '<h4>Opcion 10</h4> <input type="checkbox" id="' + data + '"/>';
                            return html;
                        } else {
                            let html = "";

                            html = '<div id=' + data + '></div>';
                            return html;
                        }
                    }
                },
                {
                    data: 'txtIdnumero11', title: 'txtIdnumero3'
                    , render: (data, type, row) => {
                        if (data == "btnAgregarInput11") {
                            let html = '';
                            html += '<h4>Opcion 11</h4> <input type="checkbox" id="' + data + '"/>';
                            return html;
                        } else {
                            let html = "";

                            html = '<div id=' + data + '></div>';
                            return html;
                        }
                    }
                },
                {
                    data: 'txtIdnumero12', title: 'txtIdnumero3'
                    , render: (data, type, row) => {
                        if (data == "btnAgregarInput12") {
                            let html = '';
                            html += '<h4>Opcion 12</h4> <input type="checkbox" id="' + data + '"/>';
                            return html;
                        } else {
                            let html = "";

                            html = '<div id=' + data + '></div>';
                            return html;
                        }
                    }
                }
            ],
            drawCallback: function (settings) {
                $('.btnAgregarPlazos').on("click", function (e) {

                    fncAgregarPlazos();
                });
            },
            columnDefs: [
                { className: "dt-center", "targets": "_all" }
            ]

        });
        $('#tblM_ComparativoFinanciera_length').css('visibility', 'hidden');
        $('#tblM_ComparativoFinanciera_filter').css('visibility', 'hidden');
        $('.dataTables_scrollHead').css('visibility', 'hidden');


    }
    const getTablaComparativoFinanciero = function () {
        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/CatMaquina/getTablaComparativoFinancieroAutorizar",
            success: function (response) {
                if (response.success) {
                    dtFinanciera.clear();
                    dtFinanciera.rows.add(response.items);
                    dtFinanciera.draw();
                    getTablaComparativoFinancieroDetalle();
                } else {
                    Alert2Error(response.message);
                }
            },
            error: function () {
                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            },
            complete: function () {
                $.unblockUI();
            }
        });
    }
    const getTablaComparativoFinancieroDetalle = function () {
        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/CatMaquina/getTablaComparativoFinancieroDetalle?idFinanciero=" + idAsignacion + "",
            success: function (response) {
                if (response.success) {
                    dibujarInputs(response.items);
                } else {
                    Alert2Error(response.message);
                }
            },
            error: function () {
                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            },
            complete: function () {
                $.unblockUI();
            }
        });
    }
    const dibujarInputs = function (lstDatosFinanciero) {
        let FinancieroMayor = 0;
        // $('#tblM_ComparativoFinanciero').find('tr').css('width','');
        $('#tblM_ComparativoFinanciera').find('thead').find('tr').find('th').css('width', '');

        let todos = [];
        for (let i = 0; i <= 12; i++) {
            todos.push(i);
        }

        var table2 = $('#tblM_ComparativoFinanciera').DataTable();
        table2.columns(todos).visible(true);
        if (lstDatosFinanciero.length != 0) {




            for (let i = 0; i <= lstDatosFinanciero.length; i++) {


                $('#banco' + i).append('<span type="text" /></span>');
                $('#plazo' + i).append('<span type="text" /></span>');
                $('#precioDelEquipo' + i).append('<span type="text" /></span>')
                $('#tiempoRestanteProyecto' + i).append('<span type="text" /></span>')
                $('#iva' + i).append('<span type="text" /></span>')
                $('#total' + i).append('<span type="text" /></span>')
                $('#montoFinanciar' + i).append('<span type="text" /></span>')
                $('#tipoOperacion' + i).append('<span type="text" /></span>')
                $('#opcionCompra' + i).append('<span type="text" /></span>')
                $('#valorResidual' + i).append('<span type="text" /></span>')
                $('#depositoEfectivo' + i).append('<span type="text" /></span>')
                $('#moneda' + i).append('<span type="text" /></span>')
                $('#plazoMeses' + i).append('<span type="text" /></span>')
                $('#tasaDeInteres' + i).append('<span type="text" /></span>')
                $('#gastosFijos' + i).append('<span type="text" /></span>')
                $('#comision' + i).append('<span type="text" /></span>')
                $('#montoComision' + i).append('<span type="text" /></span>')
                $('#rentasEnGarantia' + i).append('<span type="text" /></span>')
                $('#crecimientoPagos' + i).append('<span type="text" /></span>')
                $('#pagoInicial' + i).append('<span type="text" /></span>')
                $('#pagoTotalIntereses' + i).append('<span type="text" /></span>')
                $('#tasaEfectiva' + i).append('<span type="text" /></span>')
                $('#mensualidad' + i).append('<span type="text" /></span>')
                $('#mensualidadSinprecioDelEquipo' + i).append('<span type="text" /></span>')
                $('#pagoTotal' + i).append('<span type="text" /></span>')


            }
            let contador = 0;
            for (let index = 0; index < lstDatosFinanciero.length; index++) {
                contador++;
                if (objColor.id == lstDatosFinanciero[index].id) {
                    let color = '#D8DCD8';

                    $('#banco' + contador).parent().css('background-color', color)
                    $('#plazo' + contador).parent().css('background-color', color)
                    $('#precioDelEquipo' + contador).parent().css('background-color', color)
                    $('#tiempoRestanteProyecto' + contador).parent().css('background-color', color)
                    $('#iva' + contador).parent().css('background-color', color)
                    $('#total' + contador).parent().css('background-color', color)
                    $('#montoFinanciar' + contador).parent().css('background-color', color)
                    $('#tipoOperacion' + contador).parent().css('background-color', color)
                    $('#opcionCompra' + contador).parent().css('background-color', color)
                    $('#valorResidual' + contador).parent().css('background-color', color)
                    $('#depositoEfectivo' + contador).parent().css('background-color', color)
                    $('#moneda' + contador).parent().css('background-color', color)
                    $('#plazoMeses' + contador).parent().css('background-color', color)
                    $('#tasaDeInteres' + contador).parent().css('background-color', color)
                    $('#gastosFijos' + contador).parent().css('background-color', color)
                    $('#comision' + contador).parent().css('background-color', color)
                    $('#montoComision' + contador).parent().css('background-color', color)
                    $('#rentasEnGarantia' + contador).parent().css('background-color', color)
                    $('#crecimientoPagos' + contador).parent().css('background-color', color)
                    $('#pagoInicial' + contador).parent().css('background-color', color)
                    $('#pagoTotalIntereses' + contador).parent().css('background-color', color)
                    $('#tasaEfectiva' + contador).parent().css('background-color', color)
                    $('#mensualidad' + contador).parent().css('background-color', color)
                    $('#mensualidadSinIVA' + contador).parent().css('background-color', color)
                    $('#pagoTotal' + contador).parent().css('background-color', color)

                }

                $('#btnAgregarInput' + contador).prop('checked', lstDatosFinanciero[index].check);
                $('#btnAgregarInput' + contador).attr("data-id", lstDatosFinanciero[index].id);
                $('#banco' + contador).attr("data-id", lstDatosFinanciero[index].id);

                $('#banco' + contador).find('span').text(lstDatosFinanciero[index].banco);
                $('#plazo' + contador).find('span').text(lstDatosFinanciero[index].plazo);
                $('#precioDelEquipo' + contador).find('span').text(lstDatosFinanciero[index].precioDelEquipo);
                $('#tiempoRestanteProyecto' + contador).find('span').text(lstDatosFinanciero[index].tiempoRestanteProyecto);
                $('#iva' + contador).find('span').text(lstDatosFinanciero[index].iva);
                $('#total' + contador).find('span').text(lstDatosFinanciero[index].total);
                $('#montoFinanciar' + contador).find('span').text(lstDatosFinanciero[index].montoFinanciar);
                $('#tipoOperacion' + contador).find('span').text(lstDatosFinanciero[index].tipoOperacion);
                $('#opcionCompra' + contador).find('span').text(lstDatosFinanciero[index].opcionCompra);
                $('#valorResidual' + contador).find('span').text(lstDatosFinanciero[index].valorResidual);
                $('#depositoEfectivo' + contador).find('span').text(lstDatosFinanciero[index].depositoEfectivo);
                $('#moneda' + contador).find('span').text(lstDatosFinanciero[index].moneda);
                $('#plazoMeses' + contador).find('span').text(lstDatosFinanciero[index].plazoMeses);
                $('#tasaDeInteres' + contador).find('span').text(lstDatosFinanciero[index].tasaDeInteres);
                $('#gastosFijos' + contador).find('span').text(lstDatosFinanciero[index].gastosFijos);
                $('#comision' + contador).find('span').text(lstDatosFinanciero[index].comision);
                $('#montoComision' + contador).find('span').text(lstDatosFinanciero[index].montoComision);
                $('#rentasEnGarantia' + contador).find('span').text(lstDatosFinanciero[index].rentasEnGarantia);
                $('#crecimientoPagos' + contador).find('span').text(lstDatosFinanciero[index].crecimientoPagos);
                $('#pagoInicial' + contador).find('span').text(lstDatosFinanciero[index].pagoInicial);
                $('#pagoTotalIntereses' + contador).find('span').text(lstDatosFinanciero[index].pagoTotalIntereses);
                $('#tasaEfectiva' + contador).find('span').text(lstDatosFinanciero[index].tasaEfectiva);
                $('#mensualidad' + contador).find('span').text(lstDatosFinanciero[index].mensualidad);
                $('#mensualidadSinIVA' + contador).find('span').text(lstDatosFinanciero[index].mensualidadSinIVA);
                $('#pagoTotal' + contador).find('span').text(lstDatosFinanciero[index].pagoTotal);
            }
            $('#tblM_ComparativoFinanciera').find('input').prop('disabled', true);
            $('#btnAgregarInput1').prop('disabled', false);
            $('#btnAgregarInput2').prop('disabled', false);
            $('#btnAgregarInput3').prop('disabled', false);
            $('#btnAgregarInput4').prop('disabled', false);
            $('#btnAgregarInput5').prop('disabled', false);
            $('#btnAgregarInput6').prop('disabled', false);
            $('#btnAgregarInput7').prop('disabled', false);
            $('#btnAgregarInput8').prop('disabled', false);
            $('#btnAgregarInput9').prop('disabled', false);
            $('#btnAgregarInput10').prop('disabled', false);
            $('#btnAgregarInput11').prop('disabled', false);
            $('#btnAgregarInput12').prop('disabled', false);
            // let numeroDeRows=0;
            // let soyUnPazo=false;
            // if (lstDatosFinanciero.length==4) {
            //     for (let i = 0; i <= lstDatosFinanciero.length; i++) {
            //         if (lstDatosFinanciero[i].idRow==1) {
            //             numeroDeRows++;
            //             if (numeroDeRows==4) {
            //                 soyUnPazo=true;
            //                 break;
            //             }else{
            //                 soyUnPazo=false;
            //                 break;
            //             }
            //         }
            //     }
            // }
            let tr = $('#tblM_ComparativoFinanciera').find('tr')
            for (let index = 0; index < tr.length; index++) {
                let td = $(tr[index]).find('td')
                if (index == 2) {
                    for (let b = 0; b < td.length; b++) {
                        $(td[b]).css('font-weight', 'bold')
                    }
                }
                $(td[0]).css('font-weight', 'bold')
            }
        }
        AcomodarFinanciero(lstDatosFinanciero.length);
        let contadorcolor = 0;
        for (let i = 0; i < lstDatosFinanciero.length; i++) {
            contadorcolor++;
            if (lstDatosFinanciero[i].idRow == 1) {
                $('#Color' + contadorcolor).parent().css('background-color', '#096A9B')
            } else if (lstDatosFinanciero[i].idRow == 2) {
                $('#Color' + contadorcolor).parent().css('background-color', '#099B4E')
            } else if (lstDatosFinanciero[i].idRow == 3) {
                $('#Color' + contadorcolor).parent().css('background-color', '#C48E08')
            }
        }
    }
    const AcomodarFinanciero = function (numeroDeListasFinanciero) {
        let todos = [];
        let lstnum = [];
        for (let i = 0; i <= 12; i++) {
            todos.push(i);
            if (i > numeroDeListasFinanciero) {
                lstnum.push(i);
            }
        }


        var table = $('#tblM_ComparativoFinanciera').DataTable();
        table.columns(lstnum).visible(false);
        CambiarCheckFinanciera();
        if (MostrarOcultarFinanciero() == true) {
            btnAutorizarFinanciera.css('visibility', 'hidden')
        } else {
            btnAutorizarFinanciera.css('visibility', 'visible')
        }

        $('#tblM_ComparativoFinanciera').find('td').css('padding', 0)
    }
    const CambiarCheckFinanciera = function () {
        $('#btnAgregarInput1').click(function (e) {
            if ($('#btnAgregarInput1').prop('checked') == true) {
                $('#btnAgregarInput2').prop('checked', false);
                $('#btnAgregarInput3').prop('checked', false);
                $('#btnAgregarInput4').prop('checked', false);
                $('#btnAgregarInput5').prop('checked', false);
                $('#btnAgregarInput6').prop('checked', false);
                $('#btnAgregarInput7').prop('checked', false);
                $('#btnAgregarInput8').prop('checked', false);
                $('#btnAgregarInput9').prop('checked', false);
                $('#btnAgregarInput10').prop('checked', false);
                $('#btnAgregarInput11').prop('checked', false);
                $('#btnAgregarInput12').prop('checked', false);
                LlenarObjetoParametrosFinanciera($('#btnAgregarInput1').attr('data-id'), idAsignacion);
            }
        });
        $('#btnAgregarInput2').click(function (e) {
            if ($('#btnAgregarInput2').prop('checked') == true) {
                $('#btnAgregarInput1').prop('checked', false);
                $('#btnAgregarInput3').prop('checked', false);
                $('#btnAgregarInput4').prop('checked', false);
                $('#btnAgregarInput5').prop('checked', false);
                $('#btnAgregarInput6').prop('checked', false);
                $('#btnAgregarInput7').prop('checked', false);
                $('#btnAgregarInput8').prop('checked', false);
                $('#btnAgregarInput9').prop('checked', false);
                $('#btnAgregarInput10').prop('checked', false);
                $('#btnAgregarInput11').prop('checked', false);
                $('#btnAgregarInput12').prop('checked', false);
                LlenarObjetoParametrosFinanciera($('#btnAgregarInput2').attr('data-id'), idAsignacion);
            }
        });
        $('#btnAgregarInput3').click(function (e) {
            if ($('#btnAgregarInput3').prop('checked') == true) {
                $('#btnAgregarInput2').prop('checked', false);
                $('#btnAgregarInput1').prop('checked', false);
                $('#btnAgregarInput4').prop('checked', false);
                $('#btnAgregarInput5').prop('checked', false);
                $('#btnAgregarInput6').prop('checked', false);
                $('#btnAgregarInput7').prop('checked', false);
                $('#btnAgregarInput8').prop('checked', false);
                $('#btnAgregarInput9').prop('checked', false);
                $('#btnAgregarInput10').prop('checked', false);
                $('#btnAgregarInput11').prop('checked', false);
                $('#btnAgregarInput12').prop('checked', false);
                LlenarObjetoParametrosFinanciera($('#btnAgregarInput3').attr('data-id'), idAsignacion);
            }
        });
        $('#btnAgregarInput4').click(function (e) {
            if ($('#btnAgregarInput4').prop('checked') == true) {
                $('#btnAgregarInput2').prop('checked', false);
                $('#btnAgregarInput3').prop('checked', false);
                $('#btnAgregarInput1').prop('checked', false);
                $('#btnAgregarInput5').prop('checked', false);
                $('#btnAgregarInput6').prop('checked', false);
                $('#btnAgregarInput7').prop('checked', false);
                $('#btnAgregarInput8').prop('checked', false);
                $('#btnAgregarInput9').prop('checked', false);
                $('#btnAgregarInput10').prop('checked', false);
                $('#btnAgregarInput11').prop('checked', false);
                $('#btnAgregarInput12').prop('checked', false);
                LlenarObjetoParametrosFinanciera($('#btnAgregarInput4').attr('data-id'), idAsignacion);
            }
        });
        $('#btnAgregarInput5').click(function (e) {
            if ($('#btnAgregarInput5').prop('checked') == true) {
                $('#btnAgregarInput2').prop('checked', false);
                $('#btnAgregarInput3').prop('checked', false);
                $('#btnAgregarInput4').prop('checked', false);
                $('#btnAgregarInput1').prop('checked', false);
                $('#btnAgregarInput6').prop('checked', false);
                $('#btnAgregarInput7').prop('checked', false);
                $('#btnAgregarInput8').prop('checked', false);
                $('#btnAgregarInput9').prop('checked', false);
                $('#btnAgregarInput10').prop('checked', false);
                $('#btnAgregarInput11').prop('checked', false);
                $('#btnAgregarInput12').prop('checked', false);
                LlenarObjetoParametrosFinanciera($('#btnAgregarInput5').attr('data-id'), idAsignacion);
            }
        });
        $('#btnAgregarInput6').click(function (e) {
            if ($('#btnAgregarInput6').prop('checked') == true) {
                $('#btnAgregarInput2').prop('checked', false);
                $('#btnAgregarInput3').prop('checked', false);
                $('#btnAgregarInput4').prop('checked', false);
                $('#btnAgregarInput5').prop('checked', false);
                $('#btnAgregarInput1').prop('checked', false);
                $('#btnAgregarInput7').prop('checked', false);
                $('#btnAgregarInput8').prop('checked', false);
                $('#btnAgregarInput9').prop('checked', false);
                $('#btnAgregarInput10').prop('checked', false);
                $('#btnAgregarInput11').prop('checked', false);
                $('#btnAgregarInput12').prop('checked', false);
                LlenarObjetoParametrosFinanciera($('#btnAgregarInput6').attr('data-id'), idAsignacion);
            }
        });
        $('#btnAgregarInput7').click(function (e) {
            if ($('#btnAgregarInput7').prop('checked') == true) {
                $('#btnAgregarInput2').prop('checked', false);
                $('#btnAgregarInput3').prop('checked', false);
                $('#btnAgregarInput4').prop('checked', false);
                $('#btnAgregarInput5').prop('checked', false);
                $('#btnAgregarInput6').prop('checked', false);
                $('#btnAgregarInput1').prop('checked', false);
                $('#btnAgregarInput8').prop('checked', false);
                $('#btnAgregarInput9').prop('checked', false);
                $('#btnAgregarInput10').prop('checked', false);
                $('#btnAgregarInput11').prop('checked', false);
                $('#btnAgregarInput12').prop('checked', false);
                LlenarObjetoParametrosFinanciera($('#btnAgregarInput7').attr('data-id'), idAsignacion);
            }
        });
        $('#btnAgregarInput8').click(function (e) {
            if ($('#btnAgregarInput8').prop('checked') == true) {
                $('#btnAgregarInput2').prop('checked', false);
                $('#btnAgregarInput3').prop('checked', false);
                $('#btnAgregarInput4').prop('checked', false);
                $('#btnAgregarInput5').prop('checked', false);
                $('#btnAgregarInput6').prop('checked', false);
                $('#btnAgregarInput7').prop('checked', false);
                $('#btnAgregarInput1').prop('checked', false);
                $('#btnAgregarInput9').prop('checked', false);
                $('#btnAgregarInput10').prop('checked', false);
                $('#btnAgregarInput11').prop('checked', false);
                $('#btnAgregarInput12').prop('checked', false);
                LlenarObjetoParametrosFinanciera($('#btnAgregarInput8').attr('data-id'), idAsignacion);
            }
        });
        $('#btnAgregarInput9').click(function (e) {
            if ($('#btnAgregarInput9').prop('checked') == true) {
                $('#btnAgregarInput2').prop('checked', false);
                $('#btnAgregarInput3').prop('checked', false);
                $('#btnAgregarInput4').prop('checked', false);
                $('#btnAgregarInput5').prop('checked', false);
                $('#btnAgregarInput6').prop('checked', false);
                $('#btnAgregarInput7').prop('checked', false);
                $('#btnAgregarInput8').prop('checked', false);
                $('#btnAgregarInput1').prop('checked', false);
                $('#btnAgregarInput10').prop('checked', false);
                $('#btnAgregarInput11').prop('checked', false);
                $('#btnAgregarInput12').prop('checked', false);
                LlenarObjetoParametrosFinanciera($('#btnAgregarInput9').attr('data-id'), idAsignacion);
            }
        });
        $('#btnAgregarInput10').click(function (e) {
            if ($('#btnAgregarInput10').prop('checked') == true) {
                $('#btnAgregarInput2').prop('checked', false);
                $('#btnAgregarInput3').prop('checked', false);
                $('#btnAgregarInput4').prop('checked', false);
                $('#btnAgregarInput5').prop('checked', false);
                $('#btnAgregarInput6').prop('checked', false);
                $('#btnAgregarInput7').prop('checked', false);
                $('#btnAgregarInput8').prop('checked', false);
                $('#btnAgregarInput9').prop('checked', false);
                $('#btnAgregarInput1').prop('checked', false);
                $('#btnAgregarInput11').prop('checked', false);
                $('#btnAgregarInput12').prop('checked', false);
                LlenarObjetoParametrosFinanciera($('#btnAgregarInput10').attr('data-id'), idAsignacion);
            }
        });
        $('#btnAgregarInput11').click(function (e) {
            if ($('#btnAgregarInput11').prop('checked') == true) {
                $('#btnAgregarInput2').prop('checked', false);
                $('#btnAgregarInput3').prop('checked', false);
                $('#btnAgregarInput4').prop('checked', false);
                $('#btnAgregarInput5').prop('checked', false);
                $('#btnAgregarInput6').prop('checked', false);
                $('#btnAgregarInput7').prop('checked', false);
                $('#btnAgregarInput8').prop('checked', false);
                $('#btnAgregarInput9').prop('checked', false);
                $('#btnAgregarInput10').prop('checked', false);
                $('#btnAgregarInput1').prop('checked', false);
                $('#btnAgregarInput12').prop('checked', false);
                LlenarObjetoParametrosFinanciera($('#btnAgregarInput11').attr('data-id'), idAsignacion);
            }
        });
        $('#btnAgregarInput12').click(function (e) {
            if ($('#btnAgregarInput12').prop('checked') == true) {
                $('#btnAgregarInput2').prop('checked', false);
                $('#btnAgregarInput3').prop('checked', false);
                $('#btnAgregarInput4').prop('checked', false);
                $('#btnAgregarInput5').prop('checked', false);
                $('#btnAgregarInput6').prop('checked', false);
                $('#btnAgregarInput7').prop('checked', false);
                $('#btnAgregarInput8').prop('checked', false);
                $('#btnAgregarInput9').prop('checked', false);
                $('#btnAgregarInput10').prop('checked', false);
                $('#btnAgregarInput11').prop('checked', false);
                $('#btnAgregarInput1').prop('checked', false);
                LlenarObjetoParametrosFinanciera($('#btnAgregarInput12').attr('data-id'), idAsignacion);
            }
        });


    }
    const LlenarObjetoParametrosFinanciera = function (idRow, idAsignacion) {
        ObjParametrosFinanciera = {};
        ObjParametrosFinanciera = {
            idRow: idRow,
            idAsignacion: idAsignacion
        }
    }
    const CargarTablaAutorizanteAdquisicion = function (idCuadro) {
        if (idCuadro > 0) {
            axios.post('/CatMaquina/getAutorizanteAdquisicionPorCuadro', { idCuadro: idCuadro })
                .catch(o_O => AlertaGeneral(o_O.message))
                .then(response => {
                    let { success, lstTablaComparativa } = response.data;
                    if (success) {
                        dtAutorizanteAdquisicion.clear();
                        dtAutorizanteAdquisicion.rows.add(response.data.items);
                        dtAutorizanteAdquisicion.draw();
                    }
                });
        } else {
            axios.post('/CatMaquina/getAutorizanteAdquisicion', { idAsignacion: idAsignacion })
                .catch(o_O => AlertaGeneral(o_O.message))
                .then(response => {
                    let { success, lstTablaComparativa } = response.data;
                    if (success) {
                        dtAutorizanteAdquisicion.clear();
                        dtAutorizanteAdquisicion.rows.add(response.data.items);
                        dtAutorizanteAdquisicion.draw();
                    }
                });
        }
    }
    const init_AutorizanteAdquisicion = function () {
        dtAutorizanteAdquisicion = tblAutorizanteAdquisicion.DataTable({
            destroy: true,
            ordering: false,
            language: dtDicEsp,
            paging: false,
            searching: false,
            bInfo: false,
            // scrollX: false,
            // scrollY: true,
            // searching: false,
            columns: [
                { title: 'Nombre', data: 'autorizanteNombre', width: '20%' },
                { title: 'Puesto', data: 'autorizantePuesto', width: '20%' },
                {
                    title: 'Fecha Autorizacion', data: 'autorizanteFecha', width: '20%'
                    , render: (data, type, row) => {
                        if (data != null) {
                            let html = "";
                            html = moment(data).format('MMMM Do YYYY, h:mm:ss a');
                            return html;
                        } else {
                            let html = "";
                            html = ""
                            return html;
                        }
                    }

                },
                {
                    title: 'Voto', data: 'voto', width: '20%'
                    , render: (data, type, row) => {
                        if (data == "Opcion 0") {
                            let html = "";
                            html = "NO HA VOTADO"
                            return html;
                        } else {
                            let html = "";
                            html = data
                            return html;
                        }
                    }

                },
                {
                    title: 'Estatus', data: 'firma', width: '20%', render: (data, type, row) => {
                        if (data != "") {
                            let html = "";
                            html = "AUTORIZADO"
                            return html;
                        } else {
                            let html = "";
                            html = "PENDIENTE"
                            return html;
                        }
                    }
                },
                { title: 'Firma', data: 'firma', width: '20%' },

            ],
        });
    }
    const CargarTablaAutorizanteFinanciero = function () {
        axios.post('/CatMaquina/getAutorizanteFinanciero', { idAsignacion: idAsignacion })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, lstTablaComparativa } = response.data;
                if (success) {
                    dtAutorizantesFinanciera.clear();
                    dtAutorizantesFinanciera.rows.add(response.data.items);
                    dtAutorizantesFinanciera.draw();
                }
            });

    }
    const init_AutorizanteFinanciero = function () {
        dtAutorizantesFinanciera = tblAutorizantesFinanciera.DataTable({
            destroy: true,
            ordering: false,
            language: dtDicEsp,
            // paging: false,  
            // scrollX: false,
            // scrollY: true,
            // searching: false,
            columns: [
                { title: 'Nombre', data: 'autorizanteNombre', width: '20%' },
                { title: 'Puesto', data: 'autorizantePuesto', width: '20%' },
                {
                    title: 'Fecha autorización', data: 'autorizanteFecha', width: '20%'
                    , render: (data, type, row) => {
                        if (data != null) {
                            let html = "";
                            // html = moment(data).format('MMMM Do YYYY, h:mm:ss a'); //Do es grados
                            html = moment(data).format('MMMM DD YYYY, h:mm:ss a');
                            // html = moment(data).format('dd/MM/yyyy, h:mm:ss a');
                            // html = moment(data, 'MM/DD/YYYY').format("DD/MM/YYYY");
                            // html = "11111"
                            return html;
                        } else {
                            let html = "";
                            html = ""
                            return html;
                        }
                    }
                },
                {
                    title: 'Voto', data: 'voto', width: '20%'
                    , render: (data, type, row) => {
                        if (data == "Opcion 0") {
                            let html = "";
                            html = "NO HA VOTADO"
                            return html;
                        } else {
                            let html = "";
                            html = data
                            return html;
                        }
                    }
                },
                {
                    title: 'Estatus', data: 'firma', width: '20%', render: (data, type, row) => {
                        if (data != "") {
                            let html = "";
                            html = "AUTORIZADO"
                            return html;
                        } else {
                            let html = "";
                            html = "PENDIENTE"
                            return html;
                        }
                    }
                },
                { title: 'Firma', data: 'firma', width: '20%' },

            ],
            columnDefs: [
                { className: "dt-center", "targets": "_all" }
            ]
        });

        $(".dataTables_scrollBody").css("width", "50% !important;");
        // $(".dataTables_scrollBody").css("align", "center");
    }


    var fncCargarTablaIncidentesPRINT = function (renta) {
        let objFiltro = fncGetFiltros();
        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/CatMaquina/getTablaComparativoAutorizar",
            data: objFiltro,
            success: function (response) {
                if (response.success) {

                    getTablaComparativoAdquisicionDetallePRINT();
                    establecerAdquisicionPRINT(response.items, renta);
                } else {
                    Alert2Error(response.message);
                }
            },
            error: function () {
                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            },
            complete: function () {
                $.unblockUI();
            }
        });
    }
    var getTablaComparativoAdquisicionDetallePRINT = function () {
        let objFiltro = fncGetFiltros();
        $.ajax({
            datatype: "json",
            type: "POST",
            url: objFiltro.idCuadro > 0 ? "/CatMaquina/getTablaComparativoAdquisicionDetallePorCuadro" : "/CatMaquina/getTablaComparativoAdquisicionDetalle",
            data: objFiltro,
            success: function (response) {
                if (response.success) {
                    formatearListaPRINT(response.items);
                } else {
                    Alert2Error(response.message);
                }
            },
            error: function () {
                AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
            },
            complete: function () {
                $.unblockUI();
            }
        });
    }
    let establecerAdquisicionPRINT = function (lstAdquisicion, renta) {
        let html = '';
        // check-square


        html += "<div class='row' id='contenidoheader'>";
        html += "<div class='col-lg-3' >";
        html += "</div>";

        html += "<div class='col-lg-2' >";
        html += "compra <input id='checkComprarpt' type='checkbox'/>";
        html += "</div>";

        html += "<div class='col-lg-2' >";
        html += "Renta <input id='checkRentarpt' type='checkbox'/>";
        html += "</div>";

        html += "<div class='col-lg-2' >";
        html += "Roc <input id='checkRocrpt' type='checkbox'/>";
        html += "</div>";

        html += "<div class='col-lg-3' >";
        html += "</div><br>";
        html += "</div>";
        html += "<div class='row' id='contenidoFecha'>";
        html += "<div class='col-lg-8' >";
        html += "</div>";
        html += "<div class='col-lg-4' >";
        html += "<p id='fechacomparativo'></p>";
        html += "</div>";
        html += "</div>";

        html += '<table style="font-size:10px;" >'
        lstAdquisicion.forEach(x => {
            if (x.header != "check") {
                if (x.header == 'Caracteristica1' || x.header == 'Caracteristica2' || x.header == 'Caracteristica3' || x.header == 'Caracteristica4' || x.header == 'Caracteristica5' || x.header == 'Caracteristica6' || x.header == 'Caracteristica7') {
                    // html += '<tr> <th><div id=' + x.header + '></div></th> <td><div id="' + x.txtIdnumero1 + '"></div></td><td><div id="' + x.txtIdnumero2 + '"></div></td><td><div id="' + x.txtIdnumero3 + '"></div></td><td><div id="' + x.txtIdnumero4 + '"></div></td><td><div id="' + x.txtIdnumero5 + '"></div></td><td><div id="' + x.txtIdnumero6 + '"></div></td><td><div id="' + x.txtIdnumero7 + '"></div></td> </tr>';
                } else {
                    html += '<tr> <th>' + x.header + '</th> <td><div id="' + x.txtIdnumero1 + '"></div></td><td><div id="' + x.txtIdnumero2 + '"></div></td><td><div id="' + x.txtIdnumero3 + '"></div></td><td><div id="' + x.txtIdnumero4 + '"></div></td><td><div id="' + x.txtIdnumero5 + '"></div></td><td><div id="' + x.txtIdnumero6 + '"></div></td><td><div id="' + x.txtIdnumero7 + '"></div></td> </tr>';
                }
            } else {

                html += '<tr> <th><div id=' + x.header + '></div></th> <td><div id="' + x.txtIdnumero1 + '"></div></td><td><div id="' + x.txtIdnumero2 + '"></div></td><td><div id="' + x.txtIdnumero3 + '"></div></td><td><div id="' + x.txtIdnumero4 + '"></div></td><td><div id="' + x.txtIdnumero5 + '"></div></td><td><div id="' + x.txtIdnumero6 + '"></div></td><td><div id="' + x.txtIdnumero7 + '"></div></td> </tr>';

            }

        });
        html += '</table>'
        html += '  <div class="col-lg-5">'
        html += '<label class="text-color" for="cboCC">Tipo Moneda : </label>'
        html += '<input id="rptprintTipoMoneda" class="form-group" />'
        html += '</div>'
        contenidoPRINTadquisicion.append(html);
    }
    const formatearListaPRINT = function (lstDatos) {
        let contador = 0;
        $('#fechacomparativo').text('Fecha elaboracion: ' + moment(lstDatos[0].fechaDeElaboracion).format('YYYY/MM/DD'));
        $('#txtObrarpt').text('Obra: ' + lstDatos[0].obra);
        $('#txtNombreDelEquiporpt').text('Nombre del Equipo: ' + lstDatos[0].nombreDelEquipo);
        $('#checkComprarpt').prop('checked', lstDatos[0].compra);
        $('#checkRentarpt').prop('checked', lstDatos[0].renta);
        $('#checkRocrpt').prop('checked', lstDatos[0].roc);
        $('#rptprintTipoMoneda').val(lstDatos[0].tipoMoneda);
        lstDatos.forEach(x => {
            contador++;
            $('#Caracteristica' + contador).text(x.caracteristicasDelEquipo);
            $('#idMarcaNum' + contador + 'Marca').text(x.marcaModelo);
            $('#idMarcaNum' + contador + 'proveedor').text(x.proveedor);
            $('#idMarcaNum' + contador + 'precio').text(x.precioDeVenta);
            $('#idMarcaNum' + contador + 'Trade').text(x.tradeIn);
            $('#idMarcaNum' + contador + 'Valores').text(x.valoresDeRecompra);
            $('#idMarcaNum' + contador + 'Precio').text(x.precioDeRentaPura);
            $('#idMarcaNum' + contador + 'PrecioRoc').text(x.precioDeRentaEnRoc);
            $('#idMarcaNum' + contador + 'BaseHoras').text(x.baseHoras);
            $('#idMarcaNum' + contador + 'Tiempo').text(x.tiempoDeEntrega);
            $('#idMarcaNum' + contador + 'Ubicacion').text(x.ubicacion);
            $('#idMarcaNum' + contador + 'Horas').text(x.horas);
            $('#idMarcaNum' + contador + 'Seguro').text(x.seguro);
            $('#idMarcaNum' + contador + 'Garantia').text(x.garantia);
            $('#idMarcaNum' + contador + 'Servicios').text(x.serviciosPreventivos);
            $('#idMarcaNum' + contador + 'Capacitacion').text(x.capacitacion);
            $('#idMarcaNum' + contador + 'Deposito').text(x.depositoEnGarantia);
            $('#idMarcaNum' + contador + 'Lugar').text(x.lugarDeEntrega);
            $('#idMarcaNum' + contador + 'Flete').text(x.flete);
            $('#idMarcaNum' + contador + 'Condiciones').text(x.condicionesDePagoEntrega);


            x.lstCaracteristicas.forEach(y => {
                if (x.idRow == 1) {
                    if (y.idRow == 1) { $('#idMarcaNum1Caracteristicas11').text(y.Descripcion); }
                    if (y.idRow == 2) { $('#idMarcaNum1Caracteristicas12').text(y.Descripcion); }
                    if (y.idRow == 3) { $('#idMarcaNum1Caracteristicas13').text(y.Descripcion); }
                    if (y.idRow == 4) { $('#idMarcaNum1Caracteristicas14').text(y.Descripcion); }
                    if (y.idRow == 5) { $('#idMarcaNum1Caracteristicas15').text(y.Descripcion); }
                    if (y.idRow == 6) { $('#idMarcaNum1Caracteristicas16').text(y.Descripcion); }
                    if (y.idRow == 7) { $('#idMarcaNum1Caracteristicas17').text(y.Descripcion); }

                }
                if (x.idRow == 2) {
                    if (y.idRow == 1) { $('#idMarcaNum2Caracteristicas21').text(y.Descripcion); }
                    if (y.idRow == 2) { $('#idMarcaNum2Caracteristicas22').text(y.Descripcion); }
                    if (y.idRow == 3) { $('#idMarcaNum2Caracteristicas23').text(y.Descripcion); }
                    if (y.idRow == 4) { $('#idMarcaNum2Caracteristicas24').text(y.Descripcion); }
                    if (y.idRow == 5) { $('#idMarcaNum2Caracteristicas25').text(y.Descripcion); }
                    if (y.idRow == 6) { $('#idMarcaNum2Caracteristicas26').text(y.Descripcion); }
                    if (y.idRow == 7) { $('#idMarcaNum2Caracteristicas27').text(y.Descripcion); }

                }
                if (x.idRow == 3) {
                    if (y.idRow == 1) { $('#idMarcaNum3Caracteristicas31').text(y.Descripcion); }
                    if (y.idRow == 2) { $('#idMarcaNum3Caracteristicas32').text(y.Descripcion); }
                    if (y.idRow == 3) { $('#idMarcaNum3Caracteristicas33').text(y.Descripcion); }
                    if (y.idRow == 4) { $('#idMarcaNum3Caracteristicas34').text(y.Descripcion); }
                    if (y.idRow == 5) { $('#idMarcaNum3Caracteristicas35').text(y.Descripcion); }
                    if (y.idRow == 6) { $('#idMarcaNum3Caracteristicas36').text(y.Descripcion); }
                    if (y.idRow == 7) { $('#idMarcaNum3Caracteristicas37').text(y.Descripcion); }
                }
                if (x.idRow == 4) {
                    if (y.idRow == 1) { $('#idMarcaNum4Caracteristicas41').text(y.Descripcion); }
                    if (y.idRow == 2) { $('#idMarcaNum4Caracteristicas42').text(y.Descripcion); }
                    if (y.idRow == 3) { $('#idMarcaNum4Caracteristicas43').text(y.Descripcion); }
                    if (y.idRow == 4) { $('#idMarcaNum4Caracteristicas44').text(y.Descripcion); }
                    if (y.idRow == 5) { $('#idMarcaNum4Caracteristicas45').text(y.Descripcion); }
                    if (y.idRow == 6) { $('#idMarcaNum4Caracteristicas46').text(y.Descripcion); }
                    if (y.idRow == 7) { $('#idMarcaNum4Caracteristicas47').text(y.Descripcion); }
                }
                if (x.idRow == 5) {
                    if (y.idRow == 1) { $('#idMarcaNum5Caracteristicas51').text(y.Descripcion); }
                    if (y.idRow == 2) { $('#idMarcaNum5Caracteristicas52').text(y.Descripcion); }
                    if (y.idRow == 3) { $('#idMarcaNum5Caracteristicas53').text(y.Descripcion); }
                    if (y.idRow == 4) { $('#idMarcaNum5Caracteristicas54').text(y.Descripcion); }
                    if (y.idRow == 5) { $('#idMarcaNum5Caracteristicas55').text(y.Descripcion); }
                    if (y.idRow == 6) { $('#idMarcaNum5Caracteristicas56').text(y.Descripcion); }
                    if (y.idRow == 7) { $('#idMarcaNum5Caracteristicas57').text(y.Descripcion); }
                }
                if (x.idRow == 6) {
                    if (y.idRow == 1) { $('#idMarcaNum6Caracteristicas61').text(y.Descripcion); }
                    if (y.idRow == 2) { $('#idMarcaNum6Caracteristicas62').text(y.Descripcion); }
                    if (y.idRow == 3) { $('#idMarcaNum6Caracteristicas63').text(y.Descripcion); }
                    if (y.idRow == 4) { $('#idMarcaNum6Caracteristicas64').text(y.Descripcion); }
                    if (y.idRow == 5) { $('#idMarcaNum6Caracteristicas65').text(y.Descripcion); }
                    if (y.idRow == 6) { $('#idMarcaNum6Caracteristicas66').text(y.Descripcion); }
                    if (y.idRow == 7) { $('#idMarcaNum6Caracteristicas67').text(y.Descripcion); }
                }
                if (x.idRow == 7) {
                    if (y.idRow == 1) { $('#idMarcaNum7Caracteristicas71').text(y.Descripcion); }
                    if (y.idRow == 2) { $('#idMarcaNum7Caracteristicas72').text(y.Descripcion); }
                    if (y.idRow == 3) { $('#idMarcaNum7Caracteristicas73').text(y.Descripcion); }
                    if (y.idRow == 4) { $('#idMarcaNum7Caracteristicas74').text(y.Descripcion); }
                    if (y.idRow == 5) { $('#idMarcaNum7Caracteristicas75').text(y.Descripcion); }
                    if (y.idRow == 6) { $('#idMarcaNum7Caracteristicas76').text(y.Descripcion); }
                    if (y.idRow == 7) { $('#idMarcaNum7Caracteristicas77').text(y.Descripcion); }
                }
            });

        });
        $('#contenidoPRINTadquisicion').find('th').css('border', '0.2px solid');
        $('#contenidoPRINTadquisicion').find('td').css('border', '0.2px solid');
        $('#contenidoPRINTadquisicion').find('td').css('text-align', 'center');
        $('#contenidoPRINTadquisicion').find('th').css('text-align', 'center');
        $('#contenidoPRINTadquisicion').find('th').css('width', '160px');
        if (lstDatos.length <= 7) {
            let px = 110;
            if (lstDatos.length < 4) {
                px = 250;
            } else if (lstDatos.length == 2) {
                px = 350;
            }
            $('#contenidoPRINTadquisicion').find('td').css('width', '' + px + 'px');
        }


        let tr = $('#contenidoPRINTadquisicion').find('table').find('tr');
        let cantidadRow = lstDatos.length;
        let cantidadColum = lstDatos[0].lstCaracteristicas.length + 23;
        for (let index = 0; index < tr.length; index++) {
            let td = $(tr[index]).find('td')
            $(td[0]).css('font-weight', 'bold')
            for (let n = 0; n < td.length; n++) {
                if (n >= cantidadRow) {
                    $(td[n]).css('display', 'none');
                }
            }
            if (index >= cantidadColum) {
                $(tr[index]).css('display', 'none');
            }
        }



        ImprimirReporteHTML(false);
    }
    const ImprimirReporteHTML = function (Financiera) {
        if (Financiera == false) {
            var doc = new html2pdf();
            // var doc = new jsPDF('l','mm','A4');
            var divContenido = $('#contenidoPRINTadquisicion').get(0)
            var opt = {
                margin: [0.5, 0.5, 0.42, 0.5],
                filename: 'CuadroComparativoAdquisicion.pdf',
                image: { type: 'jpeg', quality: 1 },
                html2canvas: { scale: 1 },
                jsPDF: { unit: 'in', format: 'letter', orientation: 'P' }
            };

            // New Promise-based usage:
            doc.set(opt).from(divContenido).save().then(r => {
                $('#contenidoPRINTadquisicion').find('table').remove();
                $('#contenidoheader').remove();
                $('#contenidoFecha').remove();
                $('#contenidoAdquisicionRenta').remove();
                $('#rptprintTipoMoneda').remove()

            });
        } else {
            var doc = new html2pdf();
            // var doc = new jsPDF('l','mm','A4');
            var divContenido = $('#contenidoImpresion').get(0)
            var opt = {
                margin: [0.5, 0.5, 0.42, 0.5],
                filename: 'CuadroComparativoFinanciero.pdf',
                image: { type: 'jpeg', quality: 1 },
                html2canvas: { scale: 1 },
                jsPDF: { unit: 'in', format: 'letter', orientation: 'P' }
            };

            // New Promise-based usage:
            doc.set(opt).from(divContenido).save().then(r => {
                $('#contenidoPRINTFinanciero').find('table').remove();
                $('#divOpinionGeneral').remove();

            });
        }
    }













    $(document).ready(() => {
        AsignacionComparativo.Asignacion = new Asignacion();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...', baseZ: 2000 }); })
        .ajaxStop(() => { $.unblockUI(); });

    //#endregion
    return {
        Inicializar: Inicializar
    }
};