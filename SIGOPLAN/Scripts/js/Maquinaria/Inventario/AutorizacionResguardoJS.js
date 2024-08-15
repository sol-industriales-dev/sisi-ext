(function () {

    $.namespace('maquinaria.inventario.ResguardoEquipo.AutorizacionResguardo');

    AutorizacionResguardo = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Autorizacion de Solicitudes Reemplazo',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        txtCC = $("#txtCC");
        BntRegresar = $("#BntRegresar");
        lblValidoCurso = $("#lblValidoCurso"),
            lblElaboro = $("#lblElaboro"),
            ireport = $("#report");
        ireport2 = $("#report2");
        btnElaboro = $("#btnElaboro"),
            btnValidoCurso = $("#btnValidoCurso"),
            divFirmar = $("#divFirmar"),
            divSolicitudesPendientes = $("#divSolicitudesPendientes"),
            tblAsignacionResguardoVehiculo = $("#tblAsignacionResguardoVehiculo"),
            cboTipoFiltro = $("#cboTipoFiltro");
        btnGuardarSubidaArchivoManejo = $("#btnGuardarSubidaArchivoManejo");//raguilar subir manejo docto 10/04/18 04:16pm
        const inputFechaVigencia = $('#inputFechaVigencia');

        var idAutorizacion = ("#idAutorizacion");
        function init() {
            inputFechaVigencia.datepicker({ dateFormat: 'dd/mm/yy', showAnim: 'slide' });

            BntRegresar.click(regresar);
            txtCC.fillCombo('/CatObra/cboCentroCostosUsuarios', null, true);
            loadTabla();
            txtCC.change(loadTabla);
            cboTipoFiltro.change(loadTabla);
            btnGuardarSubidaArchivoManejo.click(SubirDocManejo);
            $("#btnRechazoSave").click(SubirDocManejo);
            //$("#tabTitle2").click(Iframe2Carga);
            $("#tabTitle2").click(Iframe1Carga);
            $("#tabTitle1").click(Iframe1Carga);
            $("#btnRegresar").css("display", "none");
            $("#btnRegresar").on("click", function () {
                location.reload();
            });
        }
        function Iframe1Carga() {
            $.blockUI({ message: mensajes.PROCESANDO });
            var id = $(this).attr('data-id');
            BntRegresar.click();
            $(".verSolicitud[data-id=" + id + "]").click();
        }

        function SubirDocManejo() {
            var idresguardoManejo = btnGuardarSubidaArchivoManejo.attr("data-idResguardo");
            var formData = new FormData();
            var file1 = document.getElementById("fResguardoManejo").files[0];

            formData.append("fResguardoManejo", file1);
            formData.append("idresguardoManejo", JSON.stringify(idresguardoManejo));
            formData.append("Comentario", JSON.stringify($("#txtAreaNota").val()));
            formData.append("fechaVigencia", inputFechaVigencia.val() != '' ? JSON.stringify(inputFechaVigencia.val()) : '');

            if ((file1 != undefined && $("#txtAreaNota").val() == "") || (file1 == undefined && $("#txtAreaNota").val() != "")) {
                formData.append("obj", JSON.stringify(idAutorizacion));
                if ($("#txtAreaNota").val() != "") {
                    formData.append("tipo", JSON.stringify(3));
                } else {
                    formData.append("tipo", JSON.stringify(1));
                }

                //#region COMENTADO
                //$.blockUI({ message: 'Cargando archivo... ¡Espere un momento!' });
                //$.ajax({
                //    type: "POST",
                //    url: '/ResguardoEquipo/SubirArchivoResguardo',
                //    data: formData,
                //    dataType: 'json',
                //    contentType: false,
                //    processData: false,
                //    success: function (response) {
                //        idResguardo = response.idResguardo;
                //        LimpiarCampos();
                //        ConfirmacionGeneralAccion('Confirmacion', 'Se Actualizaron los archivos Correctamente.');

                //        $.unblockUI();
                //    },
                //    error: function (error) {
                //        $.unblockUI();
                //    }
                //});
                //#endregion

                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    url: '/ResguardoEquipo/SaveOrUpdate',
                    type: 'POST',
                    dataType: 'json',
                    data: formData,
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        if (response.success == true) {
                            $("#modalCartaManejo").modal("hide");
                            //.verSolicitud").on
                            LoadAutorizadores(idresguardoManejo)
                            //LoadReporte(idresguardoManejo);
                            // ConfirmacionGeneral("Confirmación", "Se Autorizo Correctamente", "bg-green");
                            Alert2Exito(response.message);
                            setTimeout(location.reload(), 10000);
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
        }
        function regresar() {
            divSolicitudesPendientes.removeClass('hide');
            divFirmar.addClass('hide');
            //raguilar recargar  nuevamente la grilla con os cambios ragular 12/04/18
            loadTabla();
        }

        function loadTabla() {
            bootG('/ResguardoEquipo/GetListaAutorizacionesPendientes');
        }

        function bootG(url) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: url,
                type: "POST",
                datatype: "json",
                data: { tipoDocumento: cboTipoFiltro.val(), cc: txtCC.val() },
                success: function (response) {
                    $.unblockUI();
                    var data = response.listAutorizaciones;

                    tblAsignacionResguardoVehiculo.bootgrid("clear");
                    if (data != undefined) {
                        tblAsignacionResguardoVehiculo.bootgrid("append", data);
                    }

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function iniciarGrid() {
            tblAsignacionResguardoVehiculo.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "VerSolicitud": function (column, row) {
                        return `<button type='button' class='btn btn-primary verSolicitud' data-id='${row.id}' title="Ver solicitud."><span class='glyphicon glyphicon-eye-open'></span></button>&nbsp;`;
                        // let btnRechazarSolicitud = `<button type='button' class='btn btn-danger rechazarSolicitud' data-id='${row.id}' title="Rechazar solictud."><i class="fas fa-ban"></i></button>`;
                        // return btnVerSolicitud + btnRechazarSolicitud;
                    },

                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblAsignacionResguardoVehiculo.find(".verSolicitud").on("click", function (e) {
                    divSolicitudesPendientes.addClass('hide');
                    divFirmar.removeClass('hide');
                    LoadAutorizadores($(this).attr('data-id'));
                    LoadReporte($(this).attr('data-id'));
                    //raguilar asignando id resguardo para boton documento 10/04/18 04:27pm
                    $("#tabTitle2").attr('data-id', $(this).attr('data-id'));
                    $("#tabTitle1").attr('data-id', $(this).attr('data-id'));
                    btnGuardarSubidaArchivoManejo.attr("data-idResguardo", $(this).attr('data-id'));
                    $("#btnRegresar").css("display", "inline");
                });

                tblAsignacionResguardoVehiculo.find(".rechazarSolicitud").on("click", function (e) {
                    $("#modalRechazo").modal("show");
                });
            });
        }

        function LoadAutorizadores(idAutorizadores) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/ResguardoEquipo/GetDataAutorizacion',
                data: { obj: Number(idAutorizadores) },
                success: function (response) {

                    var AutorizadorElabora = response.AutorizadorElabora;
                    var AutorizadorSeguridad = response.AutorizadorSeguridad;

                    if (!AutorizadorSeguridad.firma && AutorizadorSeguridad.firmaCadena == "") {
                        setFirmas(btnValidoCurso, idAutorizadores);
                    }
                    else {
                        // let firmaAutorizado = AutorizadorSeguridad.firmaCadena.split("A");
                        // let firmaRechazado = AutorizadorSeguridad.firmaCadena.split("R");
                        let firma = AutorizadorSeguridad.firmaCadena.substr(-1);

                        // let tipoFirma = "";
                        // firmaAutorizado.forEach(element => {
                        //     tipoFirma = element;
                        // });

                        // firmaRechazado.forEach(element => {
                        //     tipoFirma = element
                        // });

                        if (AutorizadorSeguridad.firma && AutorizadorSeguridad.firmaCadena != "" && firma == "A") {
                            setEstatusBnts(btnValidoCurso, 1)
                        }
                        else if (firma == "R") {
                            setEstatusBnts(btnValidoCurso, 3)
                        }

                    }
                    SetNombreAutorizadores(AutorizadorElabora.nombreUsuario, AutorizadorSeguridad.nombreUsuario);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function SetNombreAutorizadores(Elabora, Seguridad) {
            lblElaboro.text(Elabora),
                lblValidoCurso.text(Seguridad);
        }

        function LoadReporte(idResguardo) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/ResguardoEquipo/GetReporte',
                type: "POST",
                datatype: "json",
                data: { obj: idResguardo },
                success: function (response) {

                    var path = "/Reportes/Vista.aspx?idReporte=42&size=1";
                    ireport.attr("src", path);
                    document.getElementById('report').onload = function () {
                    }
                    var path2 = "/Reportes/Vista.aspx?idReporte=44&size=1&idReguardo=" + idResguardo;
                    ireport2.attr("src", path2);
                    document.getElementById('report2').onload = function () {
                    }
                    $.unblockUI();


                },
                error: function () {
                    $.unblockUI();
                }
            });
        }


        function setEstatusBnts(elemento, tipo) {
            if (tipo == 1) {

                $("#divAccionesAutorizacion").remove();
                elemento.next().removeClass('panel-footer-Pendiente');
                elemento.next().addClass('panel-footer-Autoriza').html("Autorizado");
                elemento.removeClass('btn btn-block');
                elemento.attr('data-Autorizado', true);
                elemento.removeClass('bg-primary');
            }
            if (tipo == 3) {
                elemento.next().removeClass('panel-footer-Pendiente');
                elemento.next().addClass('panel-footer-Rechazo').html("Rechazado");
                elemento.removeClass('noPadding');
                elemento.attr('data-Autorizado', false);
            }

        }

        $(document).on('click', "#btnAutorizacion", function () {
            //raguilar 10/04/18 03:43pm carga modal para subir un archivo
            $("#modalCartaManejo").modal("show");
            idAutorizacion = $(this).attr('data-idAutorizacion');
        });

        $(document).on("click", "#btnRechazo", function () {
            $("#modalRechazo").modal("show");
            idAutorizacion = $(this).attr('data-idAutorizacion');
            console.log(idAutorizacion);
        });


        function setFirmas(elemento, id) {
            elemento.children().remove();
            var btnsControl = `<div class='row'> 
                                    <div class='col-lg-12 col-xs-12' id='divAccionesAutorizacion'> 
                                        <div class='col-xs-offset-4 col-xs-4'>
                                            <button class='form-control btn btn-block colorAutoriza' id='btnAutorizacion' data-idAutorizacion='${id}'>Autorizar</button>
                                            <button class='form-control btn btn-block colorRechaza rechazo' id='btnRechazo' data-idAutorizacion='${id}'>Rechazar</button>
                                        </div>
                                    </div>
                                </div>`;
            elemento.append(btnsControl);
        }
        iniciarGrid();
        init();
    };
    $(document).ready(function () {
        maquinaria.inventario.ResguardoEquipo.AutorizacionResguardo = new AutorizacionResguardo();
    });
})();

