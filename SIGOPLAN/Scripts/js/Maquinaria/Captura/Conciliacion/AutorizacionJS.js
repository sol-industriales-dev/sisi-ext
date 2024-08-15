(function () {
    $.namespace('maquinaria.captura.conciliacion');
    AutorizacionConciliacion = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Autorizacion de Conciliacion Horometros',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        btnBuscar = $("#btnBuscar");
        btnReenviar = $("#btnReenviar");
        txtConciliacionID = $("#txtConciliacionID");
        colPermiso = [
            {
                data: 'btnRpt', sortable: false,
                createdCell: function (td, cellData, rowData, row, col) {
                    $(td).html(padLeft(cellData, 10));
                }
            },
            { data: 'areaCuenta', sortable: false, },
            { data: 'descripcion', sortable: false, },
            { data: 'periodo', sortable: false, },
            {
                data: 'estatus', sortable: false,
                createdCell: function (td, cellData, rowData, row, col) {
                    $(td).html(cellData == 0 ? "Pendiente" : cellData == 1 ? "Aceptado" : "Rechazado");
                }
            },
            {
                data: 'btnRpt', sortable: false,
                createdCell: function (td, cellData, rowData, row, col) {
                    if (rowData.isUsuarioAutorisable) {
                        $(td).html(`<button type="button" class="btn btn-default btn-block btn-sm" onclick="askRespuesta('${cellData}')" ${rowData.estatus == 1 ? "disabled" : ""}>Validar</button>`);
                    }
                    else { $(td).html(''); }

                }
            },
            {
                data: 'btnValidar', sortable: false,
                createdCell: function (td, cellData, rowData, row, col) {
                    $(td).html(`<button type="button" class="btn btn-default btn-block btn-sm" onclick="verValidacion('${cellData}')" >Reporte</button>`);
                }
            },
            {
                data: 'comentario', sortable: false,
                createdCell: function (td, cellData, rowData, row, col) {
                    if (rowData.comentario && rowData.comentario.length > 0) {
                        $(td).html(`<button type="button" class="btn btn-default btn-block btn-sm" onclick="mostrarComentario('${rowData.comentario}')"}>Comentario</button>`);
                    }
                    else { $(td).html(''); }
                }
            }];
        colSinPermiso = [
            {
                data: 'btnRpt', sortable: false,
                createdCell: function (td, cellData, rowData, row, col) {
                    $(td).html(padLeft(cellData, 10));
                }
            },
            { data: 'areaCuenta', sortable: false, },
            { data: 'descripcion', sortable: false, },
            { data: 'periodo', sortable: false, },
            {
                data: 'estatus', sortable: false,
                createdCell: function (td, cellData, rowData, row, col) {
                    $(td).html(cellData == 0 ? "Pendiente" : cellData == 1 ? "Aceptado" : "Rechazado");
                }
            },
            {
                data: 'btnValidar', sortable: false,
                createdCell: function (td, cellData, rowData, row, col) {
                    $(td).html(`<button type="button" class="btn btn-default btn-block btn-sm" onclick="verValidacion('${cellData}')" >Reporte</button>`);
                }
            },
            {
                data: 'comentario', sortable: false,
                createdCell: function (td, cellData, rowData, row, col) {
                    if (rowData.comentario && rowData.comentario.length > 0) {
                        $(td).html(`<button type="button" class="btn btn-default btn-block btn-sm" onclick="mostrarComentario('${rowData.comentario}')"}>Comentario</button>`);
                    }
                    else { $(td).html(''); }
                }
            }];

        divPendientes = $("#divPendientes");
        divAutorizaciones = $("#divAutorizaciones");
        ireport = $("#report");
        ireport2 = $("#report2");
        lblElaboro = $("#lblElaboro");
        lblGerente = $("#lblGerente");
        lblDirector = $("#lblDirector");
        vistaReporte = $("#vistaReporte");
        ppal = $("#ppal");
        cboCentroCostos = $("#cboCentroCostos");
        cboPeriodos = $("#cboPeriodos");
        cboStatus = $("#cboStatus");
        tblConciliaciones = $("#tblConciliaciones");
        mdlAuth = $("#mdlAuth");
        lblFolio = $("#lblFolio");
        btnAceptar = $("#btnAceptar");
        BntRegresar = $("#BntRegresar");
        radioBtn = $('.radioBtn a');
        urlCboCC = '/Conciliacion/getCboCC';
        const modalRechazo = $("#modalRechazo");
        const btnRechazoSave = $("#btnRechazoSave");
        const btnRechazar = $("#btnRechazar");

        function init() {
            cboCentroCostos.fillCombo(urlCboCC, null, false, null);
            cboPeriodos.fillCombo('/Conciliacion/FillCboQuincenasVariables', {ccID :cboCentroCostos.val() }, false, null);
            cboCentroCostos.change(fnFechas);
            //cboPeriodos.change(fnLoadtblData);
            cboStatus.change(fnLoadtblData);
            radioBtn.click(aClick);
            btnAceptar.click(setValidacion);
            btnRechazar.click(setValidacion);
            // fnLoadtblDataInicio();
            fnLoadtblData();
            btnBuscar.click(fnLoadtblData);
            BntRegresar.click(Regresar);
            btnRechazoSave.click(rechazoSolicitud);
            btnReenviar.click(fnReenviar);
        }
        function fnReenviar()
        {
            if(txtConciliacionID.val()!=''){
                var conID  = txtConciliacionID.val();
                envioCorreos(conID);

            }
            else{
                AlertaGeneral("Alerta","Debe indicar un id de conciliacion");
            }
            
        }
        function fnFechas()
        {
            cboPeriodos.fillCombo('/Conciliacion/FillCboQuincenasVariables', {ccID :cboCentroCostos.val() }, false, null);
        }
        function GetFecha(index) {
            var FechaRaw = $("#cboPeriodos :selected").text();
            FechaRaw = FechaRaw.replace(' ','');
            var arrayFechas = FechaRaw.split("-");

            return arrayFechas[index];
        }
        function aClick() {
            let sel = $(this).data('title'),
                tog = $(this).data('toggle'),
                url = sel ? '/Conciliacion/FillCboQuincenasVariables' : '/CatInventario/FillCboSemanas';
            $(`a[data-toggle="${tog}"]`).not(`[data-title="${sel}"]`).removeClass('active').addClass('notActive');
            $(`a[data-toggle="${tog}"][data-title="${sel}"]`).removeClass('notActive').addClass('active');
            cboPeriodos.fillCombo(url, {ccID :cboCentroCostos.val() }, false, null);
        }
        function getRadioValue(tog) {
            return $(`a.active[data-toggle="${tog}"]`).data('title');
        }
        function Regresar() {
            var url = window.location.href;

            var urlLength = url.split('?').length;
            if (urlLength > 1) {
                window.location.href = url.split('?')[0];
            }
            else {
                divPendientes.removeClass('hidden');
                divAutorizaciones.addClass('hidden');
            }
        }



        function fnLoadtblData() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/conciliacion/loadtlbAutorizaciones',
                type: "POST",
                datatype: "json",
                data: {
                    centroCostosID: cboCentroCostos.val() == "" ? 0 : cboCentroCostos.val(),
                    fechaID: cboPeriodos.val() == "" ? 0 : cboPeriodos.val(),
                    fechaInicio: GetFecha(0),
                    fechaFin: GetFecha(1),
                    estatus: cboStatus.val(),
                    esQuincena: getRadioValue("radQuincena")
                },
                success: function (response) {
                    $.unblockUI();
                    fnSetInfoTbl(response.items, true);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        $(document).on('click', "#btnRechazo", function () {
            modalRechazo.modal('show');
        });

        function rechazoSolicitud() {
            const conciliacionID = $("#btnRechazo").attr('data-idAutorizacion');
            const comentario = $("#txtAreaNota").val();
            if (comentario === "" || comentario.trim().length < 10) {
                AlertaGeneral("Aviso", "Debe agregar un comentario mayor a 10 caracteres antes de poder rechazar la solicitud.");
                return;
            }
            modalRechazo.modal('hide');
            sendValidacion(conciliacionID, 2, comentario);
        }

        function fnLoadtblDataInicio() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/conciliacion/loadtlbAutorizacionesPendientes',
                type: "POST",
                datatype: "json",
                data: { centroCostosID: cboCentroCostos.val() == "" ? 0 : cboCentroCostos.val(), },
                success: function (response) {
                    $.unblockUI();
                    fnSetInfoTbl(response.items, true);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function fnSetInfoTbl(dataSet, permiso) {
            tblConciliacionesGrid = tblConciliaciones.DataTable({
                info: true,
                paging: false,
                searching: true,
                destroy: true,
                scrollX: "100%",
                sScrollXInner: "100%",
                bScrollCollapse: true,
                scrollY: '65vh',
                scrollCollapse: true,
                bLengthChange: false,
                Filter: true,
                AutoWidth: false,

                language: dtDicEsp,
                data: dataSet,
                columns: permiso ? colPermiso : colSinPermiso
            });
        }
        init();
    };
    $(document).ready(function () {
        maquinaria.captura.AutorizacionConciliacion = new AutorizacionConciliacion();
    });
})();
function askRespuesta(conciliacionID) {
    $.blockUI({ message: mensajes.PROCESANDO });

    lblFolio.data().id = conciliacionID;
    lblFolio.text(padLeft(conciliacionID, 10));
    // mdlAuth.modal("show");

    $("#divPendientes").addClass('hidden');
    $("#divAutorizaciones").removeClass('hidden');

    var idReporte = 87;

    var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&pConciliacionID=" + conciliacionID + "&size=1&inMemory=1&pTipoVista=2";
    ireport2.attr("src", path);
    document.getElementById('report2').onload = function () {
        $.unblockUI();

        loadAutorizadoresConciliacion(conciliacionID);
    };
}

$(document).on('click', "#btnAutorizacion", function () {
    var conciliacionID = $("#btnAutorizacion").attr('data-idAutorizacion');

    sendValidacion(conciliacionID, 1, "");
});

function loadAutorizadoresConciliacion(conciliacionID) {
   // $.blockUI({ message: mensajes.PROCESANDO });
    $.ajax({
        url: '/conciliacion/loadAutorizadoresConciliacion',
        type: 'POST',
        dataType: 'json',
        data: { conciliacionID: conciliacionID },
        success: function (response) {

            lblElaboro.text(response.NombreAdmin);
            lblGerente.text(response.NombreGerente);
            lblDirector.text(response.NombreDirector);
            var objAutorizacion = response.objAutorizacion;
            setEstatusBnts($("#btnElaboro"), objAutorizacion.pendienteAdmin);
            setEstatusBnts($("#btnGerenteObra"), objAutorizacion.pendienteGerente);
            setEstatusBnts($("#btnDirectorDivision"), objAutorizacion.pendienteDirector);


            if (response.autorizando) {
                switch (objAutorizacion.autorizando) {
                    case 1:
                        setFirmas($("#btnElaboro"), objAutorizacion.conciliacionID, 1);
                        break;
                    case 2:
                        setFirmas($("#btnGerenteObra"), objAutorizacion.conciliacionID, 2);
                        break;

                    case 3:
                        setFirmas($("#btnDirectorDivision"), objAutorizacion.conciliacionID, 3);
                        break;
                    default:
                }
            }
            else {
                setEstatusBnts($("#btnElaboro"), objAutorizacion.pendienteAdmin);
                setEstatusBnts($("#btnGerenteObra"), objAutorizacion.pendienteGerente);
                setEstatusBnts($("#btnDirectorDivision"), objAutorizacion.pendienteDirector);

            }


            $.unblockUI();
        },
        error: function (response) {
            $.unblockUI();
            AlertaGeneral("Alerta", response.message);
        }
    });
}

function setFirmas(elemento, idAutorizacion, puesto) {

    elemento.children().remove();
    var btnsControl = "<div class='row'> <div class='col-lg-12 col-xs-12' id='divAccionesAutorizacion'> <div class='col-xs-6'><button class='form-control btn btn-block colorAutoriza' id='btnAutorizacion'>Autorizar</button></div>" +
        "<div class='col-xs-6'><button class='form-control btn btn-block colorRechaza rechazo' id='btnRechazo'>Rechazar</button></div></div></div>"
    elemento.append(btnsControl);

    $("#btnAutorizacion").attr('data-idAutorizacion', idAutorizacion);
    $("#btnAutorizacion").attr('data-PuestoAutorizador', puesto);

    $("#btnRechazo").attr('data-idAutorizacion', idAutorizacion);
    $("#btnRechazo").attr('data-PuestoAutorizador', puesto);

}

function setEstatusBnts(elemento, tipo) {

    if (tipo == 1) {
        elemento.children().remove();
        elemento.next().removeClass('panel-footer-Rechazo');
        elemento.next().removeClass('panel-footer-Pendiente');
        elemento.next().addClass('panel-footer-Autoriza').html("Autorizado");
        elemento.removeClass('btn btn-block');
        elemento.attr('data-Autorizado', true);
        elemento.removeClass('bg-primary');
        elemento.removeClass('noPadding');
    } else if (tipo == 2) {
        elemento.children().remove();
        elemento.next().removeClass('panel-footer-Autoriza');
        elemento.next().removeClass('panel-footer-Pendiente');
        elemento.next().addClass('panel-footer-Rechazo').html("Rechazado");
        elemento.removeClass('noPadding');
        elemento.attr('data-Autorizado', false);
    } else
        if (tipo == 0) {
            elemento.next().addClass('panel-footer-Pendiente').html("Pendiente");
            elemento.next().removeClass('panel-footer-Rechazo');
            elemento.next().removeClass('panel-footer-Autoriza');
        }

}

function padLeft(positiveInteger, totalDigits) {
    var padding = "00000000000000";
    var rounding = 1.000000000001;
    var currentDigits = positiveInteger > 0 ? 1 + Math.floor(rounding * (Math.log(positiveInteger) / Math.LN10)) : 1;
    return (padding + positiveInteger).substr(padding.length - (totalDigits - currentDigits));
}
function sendValidacion(conciliacionID, respuesta, comentario) {
    
    $.blockUI({ message: mensajes.PROCESANDO });
    $.ajax({
        url: '/conciliacion/sendValidacion',
        type: "POST",
        datatype: "json",
        data: { conciliacionID: conciliacionID, respuesta: respuesta, comentario },
        success: function (response) {
            if (response.success) {
                if (response.enviarCorreo) {


                    var idReporte = 87;

                    var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&pConciliacionID=" + conciliacionID + "&size=1&inMemory=1&pTipoVista=2";
                    ireport.attr("src", path);
                    document.getElementById('report').onload = function () {
                        $.unblockUI();
                        SendCorreoAjuntos(conciliacionID);

                    };
                }
                else {
                    loadAutorizadoresConciliacion(conciliacionID);
                    AlertaGeneral("Aviso", "Validación realizada.");
                }

            }
            else {
                AlertaGeneral("Aviso", "No fue posible validar la conciliación.");
            }
            $.unblockUI();
        },
        error: function () {
            $.unblockUI();
        }
    });
}



function setValidacion() {

    var conciliacionID = lblFolio.data().id;
    var respuesta = this.value;

    $.blockUI({ message: mensajes.PROCESANDO });
    var post = sendValidacion(lblFolio.data().id, this.value, "");
    post.done(function (response) {
        if (response.success) {
            //  fnLoadtblData();
            AlertaGeneral("Aviso", "Validación realizada.");

            if (response.enviarCorreo) {
                var Autorizadores = response.Autorizadores;

                var idReporte = 87;

                var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&pConciliacionID=" + Autorizadores + "&size=1&inMemory=1&pTipoVista=2";
                ireport.attr("src", path);
                document.getElementById('report').onload = function () {
                    $.unblockUI();
                    SendCorreoAjuntos(Autorizadores);
                };
            }

        }
        else {
            AlertaGeneral("Aviso", "No fue posible validar la conciliación.");
        }
    });
    post.always(function () {
        $.unblockUI();
        mdlAuth.modal("hide");
    })
}

function envioCorreos(conciliacionID) {


    var idReporte = 87;

    var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&pConciliacionID=" + conciliacionID + "&size=1&inMemory=1&pTipoVista=2";
    ireport.attr("src", path);
    document.getElementById('report').onload = function () {
        $.unblockUI();
        ReenviarCorreoAjuntos(conciliacionID);

    };
}
function SendCorreoAjuntos(conciliacionID) {
    $.blockUI({ message: mensajes.PROCESANDO });
    $.ajax({
        url: '/conciliacion/sendCorreoAjunto',
        type: "POST",
        datatype: "json",
        data: { conciliacionID: conciliacionID },
        success: function (response) {
            $.unblockUI();
            loadAutorizadoresConciliacion(conciliacionID);
            AlertaGeneral("Aviso", "Validación realizada.");

        },
        error: function () {
            $.unblockUI();
        }
    });
}
function ReenviarCorreoAjuntos(conciliacionID) {
    $.blockUI({ message: mensajes.PROCESANDO });
    $.ajax({
        url: '/conciliacion/fnReenviarCorreo',
        type: "POST",
        datatype: "json",
        data: { conciliacionID: conciliacionID },
        success: function (response) {
            $.unblockUI();
            loadAutorizadoresConciliacion(conciliacionID);
            AlertaGeneral("Aviso", "Validación realizada.");

        },
        error: function () {
            $.unblockUI();
        }
    });
}

function mostrarComentario(comentario) {
    AlertaGeneral("Razón de rechazo:", comentario);
}

function verValidacion(validaID) {
    $.blockUI({ message: mensajes.PROCESANDO });
    $.ajax({
        url: '/conciliacion/loadAutorizacion',
        type: "POST",
        datatype: "json",
        data: { validaID: validaID },
        success: function (response) {
            var Autorizadores = response.autorizaciones;
            var idReporte = 87;
            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&pConciliacionID=" + Autorizadores.conciliacionID + "&size=1";
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
}


