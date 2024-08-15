
(function () {

    $.namespace('maquinaria.captura.conciliacion.autorizaCaratula');

    AutorizaCaratula = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Autorizacion de Caratulas',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        cboCC = $("#cboCC"),
            cboEstatus = $("#cboEstatus"),
            tblCaratulas = $("#tblCaratulas"),
            divppal = $("#divppal"),
            divVista = $("#divVista"),
            BntRegresar = $(".BntRegresar"),
            lblEnvia = $("#lblEnvia"),
            btnEnvia = $("#btnEnvia"),
            lblVobo2 = $("#lblVobo2"),
            btnVobo2 = $("#btnVobo2"),
            lblVobo1 = $("#lblVobo1"),
            btnVobo1 = $("#btnVobo1"),
            lblDireccion = $("#lblDireccion"),
            btnDireccion = $("#btnDireccion");
        ireport1 = $("#report1");
        ireport = $("#report");

        const modalRechazo = $("#modalRechazo");
        const btnRechazoSave = $("#btnRechazoSave");

        function init() {
            cboCC.fillCombo('/Conciliacion/getCboCC', null, false, null);
            cboCC.change(loadTabla);
            cboEstatus.change(loadTabla);
            loadTabla();

            BntRegresar.click(Regresar);
            btnRechazoSave.click(rechazoSolicitud);
        }

        function Regresar() {
            var url = window.location.href;

            var urlLength = url.split('?').length;
            if (urlLength > 1) {
                window.location.href = url.split('?')[0];
            }
            else {
                divppal.removeClass('hide');
                divVista.addClass('hide');
            }
        }


        $(document).on('click', "#btnAutorizacion", function () {
            var id = $("#btnAutorizacion").attr('data-idAutorizacion');
            var puesto = $("#btnAutorizacion").attr('data-PuestoAutorizador');
            saveOrUpdate(id, puesto, 1, "Se Autorizo Correctamente", "");
        });

        $(document).on('click', "#btnRechazo", () => modalRechazo.modal("show"));

        function rechazoSolicitud() {
            var id = $("#btnAutorizacion").attr('data-idAutorizacion');
            var puesto = $("#btnAutorizacion").attr('data-PuestoAutorizador');
            const comentario = $("#txtAreaNota").val();
            if (comentario === "" || comentario.trim().length < 10) {
                AlertaGeneral("Aviso", "Debe agregar un comentario mayor a 10 caracteres antes de poder rechazar la solicitud.");
                return;
            }
            modalRechazo.modal('hide');
            saveOrUpdate(id, puesto, 2, "Se rechazó correctamente.", comentario);
        }

        function saveOrUpdate(obj, Autoriza, tipo, cadena, comentario) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/conciliacion/SaveOrUpdateAutorizacion',
                type: 'POST',
                dataType: 'json',
                data: { obj, Autoriza, tipo, comentario },
                success: function (response) {
                    if (response.success == true) {

                        reload(obj);
                        ConfirmacionGeneral("Confirmación", cadena, "bg-green");

                        $.unblockUI();
                    }
                    else {
                        ConfirmacionGeneral("Alerta", response.message, "bg-red");
                        $.unblockUI();
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function loadTabla() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/conciliacion/loadTlbAutorizacionesCaratula',
                type: "POST",
                datatype: "json",
                data: {
                    cc: cboCC.val() == "" ? 0 : cboCC.val(),
                    estatus: cboEstatus.val()
                },
                success: function (response) {

                    $.unblockUI();
                    setTblData(response.dataSet)

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function setTblData(dataSet) {
            tblCaratulasGrid = $("#tblCaratulas").DataTable({
                info: false,
                paging: false,
                searching: false,
                language: dtDicEsp,
                destroy: true,
                scrollY: "300px",
                scrollX: true,
                data: dataSet,
                columns: [
                    { data: 'obraID', sortable: false, },
                    { data: 'estadoCaratula', sortable: false, },
                    {
                        data: 'id', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (rowData.autorizando) {
                                $(td).append('<button type="button" class="btn btn-default btn-block btn-sm" onclick="verRpt(' + cellData + ')" >Validar</button>');
                            }
                        }
                    },
                    {
                        data: 'comentario', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            if (rowData.comentario && rowData.comentario.length > 0) {
                                $(td).append(`<button type="button" class="btn btn-default btn-block btn-sm" onclick="mostrarComentario('${cellData}')">Comentario</button>`);
                            }
                        }
                    },
                    {
                        data: 'ids', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                           
                            $(td).append('<button type="button" class="btn btn-default btn-block btn-sm" onclick="verRptView(' + cellData + ')" >Reporte</button>');
                            
                        }
                    },

                ],

            });
            tblCaratulasGrid.draw();
        }

        function reload(autorizacionID) {

            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/conciliacion/GetInfoAutorizacion',
                type: "POST",
                datatype: "json",
                data: {
                    id: autorizacionID
                },
                success: function (response) {

                    var data = response.data;

                    var path = "/Reportes/Vista.aspx?idReporte=" + 94 + "&pCaratulaID=" + data.caratulaID + "&pIDAutoriza=" + data.id;
                    ireport1.attr("src", path);
                    document.getElementById('report1').onload = function () {
                        $.unblockUI();
                    };

                    lblEnvia.text(data.usuarioElaboraNombre);
                    lblVobo2.text(data.usuarioVobo2Nombre);
                    lblVobo1.text(data.usuarioVobo1Nombre);
                    lblDireccion.text(data.usuarioAutorizaNombre);

                    divVista.removeClass('hide');
                    divppal.addClass('hide');
                   

                },
                error: function () {
                    $.unblockUI();
                }
            });

        }

        init();

    };

    $(document).ready(function () {

        maquinaria.captura.conciliacion.autorizaCaratula = new AutorizaCaratula();
    });
})();

function mostrarComentario(comentario) {
    AlertaGeneral("Razón de rechazo: ", comentario);
}

function verRptView(autorizacionID) {

    $.blockUI({ message: mensajes.PROCESANDO });
    $.ajax({
        url: '/conciliacion/GetInfoAutorizacion',
        type: "POST",
        datatype: "json",
        data: {
            id: autorizacionID
        },
        success: function (response) {

            var data = response.data;
            var path = "/Reportes/Vista.aspx?idReporte=" + 94 + "&pCaratulaID=" + data.caratulaID + "&pIDAutoriza=" + data.id;
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

function verRpt(autorizacionID) {
    $("#liAut").click();
    $.blockUI({ message: mensajes.PROCESANDO });
    $.ajax({
        url: '/conciliacion/GetInfoAutorizacion',
        type: "POST",
        datatype: "json",
        data: {
            id: autorizacionID
        },
        success: function (response) {
            var data = response.data;
            lblEnvia.text(data.usuarioElaboraNombre);
            lblVobo2.text(data.usuarioVobo2Nombre);
            lblVobo1.text(data.usuarioVobo1Nombre);
            lblDireccion.text(data.usuarioAutorizaNombre);

            divVista.removeClass('hide');
            divppal.addClass('hide');
            usuarioFirma = response.usuarioFirma;

            setEstatusBnts($("#btnEnvia"), data.firmaElabora);
            setEstatusBnts($("#btnVobo2"), data.firmaVobo2);
            setEstatusBnts($("#btnVobo1"), data.firmaVobo1);
            setEstatusBnts($("#btnDireccion"), data.firmaAutoriza);

            if (usuarioFirma) {
                setElemntoFirma = response.ElementoFirma;
                setFirmas($("#" + setElemntoFirma), data.id, data.usuarioFirma);

                if (setElemntoFirma != "btnEnvia") {
                    setEstatusBnts($("#btnEnvia"), data.firmaElabora);
                }

                if (setElemntoFirma != "btnVobo2") {
                    setEstatusBnts($("#btnVobo2"), data.firmaVobo2);
                }

                if (setElemntoFirma != "btnVobo1") {
                    setEstatusBnts($("#btnVobo1"), data.firmaVobo1);
                }

                if (setElemntoFirma != "btnDireccion") {
                    setEstatusBnts($("#btnDireccion"), data.firmaAutoriza);
                }
            }
            else {
                setEstatusBnts($("#btnEnvia"), data.firmaElabora);
                setEstatusBnts($("#btnVobo2"), data.firmaVobo2);
                setEstatusBnts($("#btnVobo1"), data.firmaVobo1);
                setEstatusBnts($("#btnDireccion"), data.firmaAutoriza);

            }

            
            $.ajax({
                url: '/conciliacion/GetCaratulaComparacion',
                type: "POST",
                datatype: "json",
                data: {
                    caratulaActualID: data.caratulaActualID,
                    caratulaNuevaID: data.caratulaID
                },
                success: function (response) {

                    
                    $("#divCaratulaActual").html(response.actual);
                    $("#divCaratulaActualizada").html(response.nueva);
                    var path = "/Reportes/Vista.aspx?idReporte=" + 94 + "&pCaratulaID=" + data.caratulaID + "&pIDAutoriza=" + data.id;
                    ireport1.attr("src", path);
                    document.getElementById('report1').onload = function () {
                        if(data.caratulaActualID>0){
                            $("#liComp").show();
                            $("#linkComp").click();
                        }
                        else{
                            $("#liComp").hide();
                        }
                        $.unblockUI();
                    };



                },
                error: function () {
                    $.unblockUI();
                }
            });
            

        },
        error: function () {
            $.unblockUI();
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