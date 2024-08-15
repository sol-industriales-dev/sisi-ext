(function () {

    $.namespace('maquinaria.overhaul.remocion');
 
    remocion = function () {
        id = $.urlParam('id');
        estadoReporte = -1;
        let tipoUsuarioR = 6;
        mensajes = {
            NOMBRE: 'Componentes',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        txtFecha = $("#txtFecha"),
        txtNoEconomico = $("#txtNoEconomico"),
        txtModelo = $("#txtModelo"),
        txtHoras = $("#txtHoras"),
        txtSerieMaquina = $("#txtSerieMaquina"),
        txtDescripcionComponente = $("#txtDescripcionComponente"),
        txtNumParteComponente = $("#txtNumParteComponente"),
        txtNoComponenteRemovido = $("#txtSerieComponenteRemovido"),
        txtHorasComponenteRemovido = $("#txtHorasComponenteRemovido"),
        cboNoComponenteInstalado = $("#cboSerieComponenteInstalado"),
        cboGarantia = $("#cboGarantia"),
        txtPersonal = $("#txtPersonal"),
        btnAgregarEmpleado = $("#btnAgregarEmpleado"),
        gridPersonal = $("#gridPersonal"),
        btnVerReporte = $("#btnVerReporte"),
        btncargarImgRemovido = $("#btncargarImgRemovido"),
        inCargarImgRemovido = $("#inCargarImgRemovido"),
        btncargarImgInstalado = $("#btncargarImgInstalado"),
        inCargarImgInstalado = $("#inCargarImgInstalado"),
        imgRemovido = $("#imgRemovido"),
        imgInstalado = $("#imgInstalado"),
        cboEmpresaResponsable = $("#cboEmpresaResponsable"),
        cboMotivo = $("#cboMotivo"),
        txtacomentario = $("#txtacomentario"),
        txtCC = $("#txtCC"),
        cboDestino = $("#cboDestino"),
        lblcargarImgRemovido = $("#lblcargarImgRemovido"),
        lblcargarImgInstalado = $("#lblcargarImgInstalado"),
        //txtfechaAlta = $("#txtfechaAlta"),
        btnEnviarReporte = $("#btnEnviarReporte"),
        //txtUltimaReparacion = $("#txtUltimaReparacion"),
        cboEmpresaInstalacion = $("#cboEmpresaInstalacion"),
        txtFechaInstalacionComponente = $("#txtFechaInstalacionComponente"),
        txtFechaInstalacion = $("#txtFechaInstalacion");
        var folio = 0;
        btnAbrirModalImagen = $('#btnAbrirModalImagen');
        imgRemovidoExpand = $('#imgRemovidoExpand');
        imgInstalado = $('#imgInstalado');
        
        function init() {
            PermisosBotonesC();
            cboNoComponenteInstalado.change(cargarFechaInstalacion);
            iniciarGrid();
            cargarDatos();            

            //txtPersonal.change(habilitarBtnAgregarEmp);
            btnAgregarEmpleado.click(cargarEmpleado);
            btnVerReporte.click(ValidarReporte);
            btnEnviarReporte.click(ValidarReporte);
            inCargarImgRemovido.change(function () {
                leerURL(this, imgRemovido);
            });
            inCargarImgInstalado.change(function () {
                leerURL(this, imgInstalado);
            });
            cboMotivo.change(cargarDestino);
            btncargarImgRemovido.click(function () {
                inCargarImgRemovido.click();
            });
            btncargarImgInstalado.click(function () {
                inCargarImgInstalado.click();
            });
          
            imgRemovido.click(function () {
                $("#tituloModalImagen").text(txtNoComponenteRemovido.val());
                var thisSrc = $('#imgRemovido').attr('src');
                var ckEdImg = '<p><img src="' + thisSrc + '" /></p>';                
                imgRemovidoExpand.attr("src", thisSrc);
            });
            imgInstalado.click(function () {
                if (cboNoComponenteInstalado.val() != null) $("#tituloModalImagen").text($("#cboSerieComponenteInstalado option:selected").attr("data-prefijo"));
                else { $("#tituloModalImagen").text("SIN ESPECIFICAR"); }
                
                var thisSrc = $('#imgInstalado').attr('src');
                var ckEdImg = '<p><img src="'+thisSrc+'" /></p>';  
                imgRemovidoExpand.attr("src", thisSrc);
            });

            btnAbrirModalImagen.click(function () {
                $("#tituloModalImagen").text(txtNoComponenteRemovido.val());
                var thisSrc = $('#imgRemovido').attr('src');
                var ckEdImg = '<p><img src="'+thisSrc+'" /></p>';  
                imgRemovidoExpand.attr("src", thisSrc);
            });
              

            //cboGarantia.change(habilitarEmpresaResponsable);
            cboEmpresaResponsable.change(habilitarPersonal);

            txtPersonal.getAutocomplete(funSelEmpleado, null, '/Overhaul/getEmpleadosRemocion');
            
        }

        /******Funciones para select de auto completar********/

        function funSelEmpleado(event, ui) {
            txtPersonal.val(ui.item.value);
            txtPersonal.attr("data-index", ui.item.id);
            habilitarBtnAgregarEmp();
        }


        function PermisosBotonesC() {
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: "/Overhaul/PermisosBotonesAdminComp",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                success: function (response) {
                    $.unblockUI();
                    tipoUsuarioR = response.tipoUsuario;
                    if (response.tipoUsuario == 0 || response.tipoUsuario == 1 || response.tipoUsuario == 3) {
                        btnEnviarReporte.prop("disabled", false);
                    }
                    else {
                        btnEnviarReporte.prop("disabled", true);
                    }
                    if (response.tipoUsuario == 7) {
                        btnEnviarReporte.prop("disabled", false);
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }


        function habilitarEmpresaResponsable() {
            cboEmpresaResponsable.val("");
            cboEmpresaResponsable.change();
            if (cboGarantia.val() == "1") {
                cboEmpresaResponsable.prop("disabled", false);
            }
            else {
                cboEmpresaResponsable.prop("disabled", true);
                if (cboGarantia.val() == "0") {
                    cboEmpresaResponsable.val("0");
                    cboEmpresaResponsable.change();
                }
            }
        }

        function habilitarPersonal()
        {
            if (cboEmpresaResponsable.val() == "0") {
                txtPersonal.parent().parent().parent().parent().parent().parent().css("display", "block");
            }
            else {
                txtPersonal.parent().parent().parent().parent().parent().parent().css("display", "none");
            }
        }


        function cargarDestino()
        {
            if (cboMotivo.val() == "" || cboMotivo.val() == null) {
                cboDestino.clearCombo();
            }
            else
            {
                cboDestino.fillCombo('fillCboLocaciones', { idModelo: txtModelo.attr('data-info'), tipoLocacion: cboMotivo.val() });
            }
        }

        function leerURL(input, imagen) {
            if (input.files && input.files[0]) {
                ImageTools.resize(
                    input.files[0],
                    { width: 500, height: 500 },
                    function (blob, didItResize) {
                        toDataUrl(window.URL.createObjectURL(blob), function (myBase64) {
                            imagen.attr('src', myBase64);
                        });
                    }
                );
            }
        }

        function toDataUrl(url, callback) {
            var xhr = new XMLHttpRequest();
            xhr.onload = function () {
                var reader = new FileReader();
                reader.onloadend = function () {
                    callback(reader.result);
                }
                reader.readAsDataURL(xhr.response);
            };
            xhr.open('GET', url);
            xhr.responseType = 'blob';
            xhr.send();
        }

        function ValidarReporte()
        {
            if (tipoUsuarioR == 0 || tipoUsuarioR == 1 || tipoUsuarioR == 3 || tipoUsuarioR == 7) {
                var mensajeEstatus = ""
                if (estadoReporte < 5) {
                    var estado = 1;
                    //
                    if (txtFecha.val() == null || txtFecha.val() == "") { estado = 0; }
                    if (cboMotivo.val() == null || cboMotivo.val() == "") { estado = 0; }
                    if (cboEmpresaResponsable.val() == null || cboEmpresaResponsable.val() == "") { estado = 0; }
                    if (cboGarantia.val() == null || cboGarantia.val() == "") { estado = 0; }
                    if (cboDestino.val() == null || cboDestino.val() == "") { estado = 0; }
                    if (cboEmpresaResponsable.val() == "0" && gridPersonal.bootgrid("getTotalRowCount") < 1) { estado = 0; }
                    if (imgRemovido.attr("src") == null || imgRemovido.attr("src") == "") { estado = 0; }

                    if (estado == 1) {
                        if (estadoReporte < 2) {
                            mensajeEstatus = "PENDIENTE VoBo";
                        }
                        else {
                            estado = 4;
                            if (cboNoComponenteInstalado.val() == null || cboNoComponenteInstalado.val() == "") { estado = 3; }
                            if (txtFechaInstalacionComponente.val() == null || txtFechaInstalacionComponente.val() == "") { estado = 3; }
                            if (cboEmpresaInstalacion.val() == null || cboEmpresaInstalacion.val() == "") { estado = 3; }
                            if (imgInstalado.attr("src") == null || imgInstalado.attr("src") == "") { estado = 3; }
                            mensajeEstatus = "PENDIENTE AUTORIZACION";
                        }
                    }
                    else {
                        mensajeEstatus = "INCOMPLETO";
                    }
                }
                //if (estadoReporte >= 3 && estadoReporte < 5) {
                //    estado = 4;
                //    if (cboNoComponenteInstalado.val() == null || cboNoComponenteInstalado.val() == "") { estado = 3; }
                //    if (txtFechaInstalacionComponente.val() == null || txtFechaInstalacionComponente.val() == "") { estado = 3; }
                //    if (cboEmpresaInstalacion.val() == null || cboEmpresaInstalacion.val() == "") { estado = 3; }
                //    if (imgInstalado.attr("src") == null || imgInstalado.attr("src") == "") { estado = 3; }
                //    mensajeEstatus = "PENDIENTE AUTORIZACION";
                //}
                guardarReporte(estado); /*abrirReporte();*/
                AlertaGeneral("Alerta", "Se guardo el reporte con estatus de " + mensajeEstatus);
                window.location.href = "/Overhaul/AdministracionComponentes?tab=1";
            }
        }
        
        //function abrirReporte()
        //{
        //    var personal = [];
        //    $('#gridPersonal tbody tr').each(function () {
        //        personal.push($(this).find('td:eq(1)').text());
        //    });

        //    $.blockUI({ message: mensajes.PROCESANDO });
        //    $.ajax({
        //        url: '/Overhaul/enviarReporte',
        //        type: 'POST',
        //        dataType: 'json',
        //        contentType: 'application/json',
        //        data: JSON.stringify({ noEconomico: txtNoEconomico.val().trim(), imgRemovido: imgRemovido.attr("src"), imgInstalado: imgInstalado.attr("src"), personal: personal, comentario: txtacomentario.val() }),
        //        success: function (response) {
        //            //$.unblockUI();
        //            var html = "/Reportes/Vista.aspx?idReporte=150";
        //            html += "&fecha=" + txtFecha.val().trim();
        //            html += "&noEconomico=" + txtNoEconomico.val().trim();
        //            html += "&modelo=" + txtModelo.val().trim();
        //            html += "&horasmaquina=" + txtHoras.val().trim();
        //            html += "&seriemaquina=" + txtSerieMaquina.val().trim();
        //            html += "&descripcion=" + txtDescripcionComponente.val().trim();
        //            html += "&numparte=" + txtNumParteComponente.val().trim();
        //            html += "&nocomponenteremovido=" + txtNoComponenteRemovido.val().trim();
        //            html += "&horasComponenteRemovido=" + txtHorasComponenteRemovido.val().trim();
        //            html += "&nocomponenteinstalado=" + $("#cboSerieComponenteInstalado option:selected").data("prefijo").trim(); // $("#txtNoComponenteRemovido option:selected").attr("data-prefijo");
        //            html += "&garantia=" + $("#cboGarantia option:selected").text();
        //            html += "&empresaresponsable=" + cboEmpresaResponsable.val().trim();
        //            html += "&motivo=" + $("#cboMotivo option:selected").text().trim();
        //            html += "&realiza=" + getUsuario();
        //            ireport.attr("src", html);
        //            document.getElementById('report').onload = function () {                        
        //                openCRModal();
                        
        //            };
        //            $.unblockUI();
        //        },
        //        error: function (response) {
        //            $.unblockUI();
        //            AlertaGeneral("Alerta", response.message);
                    
        //        }
        //    });
        //}

        function datosReporte(estado)
        {
            var personal = "";
            $('#gridPersonal tbody tr').each(function () {
                personal += $(this).find('td:eq(1)').text() + ",";
            });
            return {
                id: folio,
                fechaRemocion: parseDate (txtFecha.val().trim()),
                componenteRemovidoID: txtDescripcionComponente.attr("data-id"),
                componenteInstaladoID: cboNoComponenteInstalado.val() == "" ? "-1" : cboNoComponenteInstalado.val(),
                maquinaID: txtNoEconomico.attr("data-id"),
                areaCuenta: txtCC.attr("data_id"),
                motivoRemocionID: cboMotivo.val(),
                destinoID: cboDestino.val(),
                comentario: txtacomentario.val(),
                garantia: cboGarantia.val() == "1" ? true : (cboGarantia.val() == "0" ? false : null),//txtDescripcionComponente.attr("data-garantia") == null ? false : txtDescripcionComponente.attr("data-garantia"),
                empresaResponsable: cboEmpresaResponsable.val(),
                personal: personal == "," ? "" : personal,
                imgComponenteRemovido: imgRemovido.attr("src"),
                imgComponenteInstalado: imgInstalado.attr("src"),
                empresaInstala: cboEmpresaInstalacion.val() == "" ? "-1" : cboEmpresaInstalacion.val(),
                fechaInstalacionCRemovido: parseDate (txtFechaInstalacion.val().trim()),
                fechaInstalacionCInstalado: parseDate (txtFechaInstalacionComponente.val().trim()),
                //fechaUltimaReparacion: txtUltimaReparacion.val() == "N/A" ? "" : txtUltimaReparacion.val(),
                estatus: estado,
                horasComponente: txtHorasComponenteRemovido.val(),
                horasMaquina: txtHoras.val()
            }
        }

        function parseDate(input) {
            var parts = input.split('/');
            // new Date(year, month [, day [, hours[, minutes[, seconds[, ms]]]]])
            return new Date(parts[2], parts[1] - 1, parts[0]); // Note: months are 0-based
        }

        function guardarReporte(estado)
        {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/guardarReporte',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ reporte: datosReporte(estado) }),
                success: function (response) {

                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);

                }
            });
        }


        function cargarDatos()
        {            
            cboEmpresaResponsable.fillCombo('/Overhaul/fillCboLocacion', { tipoLocacion: 2 });
            $('#cboEmpresaResponsable option:first').after($('<option />', { "value": '0', text: 'CONSTRUPLAN' }));
            cboEmpresaInstalacion.fillCombo('/Overhaul/fillCboLocacion', { tipoLocacion: 2 });
            $('#cboEmpresaInstalacion option:first').after($('<option />', { "value": '0', text: 'CONSTRUPLAN' }));
            cboDestino.append("<option value=''>--Seleccione--</option>");

            txtFecha.datepicker().datepicker("setDate", new Date());
            txtFechaInstalacionComponente.datepicker();
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/cargarDatosRemocionComponente',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ idComponente: id }),
                success: function (response) {
                    $.unblockUI();
                    txtDescripcionComponente.val(response.remocion.descripcionComponente);
                    txtNoEconomico.val(response.remocion.noEconomico);
                    txtModelo.val(response.remocion.modelo);
                    txtModelo.attr('data-info', response.remocion.idModelo);
                    txtHoras.val(response.remocion.horas);
                    txtHorasComponenteRemovido.val(response.remocion.horasComponenteRemovido);
                    txtNumParteComponente.val(response.remocion.numParteComponente);
                    txtNoComponenteRemovido.val(response.remocion.serieComponenteRemovido);
                    txtSerieMaquina.val(response.remocion.serieMaquina);
                    txtCC.val(response.remocion.nombreCC);
                    txtFechaInstalacion.val(response.remocion.fechaInstalacionRemovido);
                    txtFechaInstalacion.attr("data_fechaNum", response.remocion.fechaInstalacionRemovidoRaw);
                    //txtUltimaReparacion.val(response.remocion.fechaUltimaReparacion)
                    cboNoComponenteInstalado.fillCombo('/Overhaul/cargarCboComponenteInstalado', { idModelo: response.remocion.modeloID, idSubconjunto: response.remocion.subconjuntoID });
                    //txtPersonal.fillCombo('/Overhaul/cargartxtPersonal', { cc: response.remocion.cc });
                    txtFecha.val(response.remocion.fecha);
                    txtFecha.attr("data_fechaNum", response.remocion.fechaNum);
                    txtDescripcionComponente.attr("data-id", response.remocion.componenteRemovidoID);
                    txtNoEconomico.attr("data-id", response.remocion.maquinaID);
                    txtCC.attr("data_id", response.remocion.cc);
                    txtDescripcionComponente.attr("data-garantia", response.remocion.garantia);
                    response.remocion.motivoID != "-1" ? cboMotivo.val(response.remocion.motivoID) : cboMotivo.val("");
                    cargarDestino();
                    response.remocion.destinoID != "-1" ? cboDestino.val(response.remocion.destinoID) : cboDestino.val("");
                    txtacomentario.val(response.remocion.comentario);
                    response.remocion.componenteInstaladoID != "-1" ? cboNoComponenteInstalado.val(response.remocion.componenteInstaladoID) : cboNoComponenteInstalado.val("");
                    cboGarantia.val(response.remocion.garantia != null ? (response.remocion.garantia == true ? "1" : "0") : "");
                    cboGarantia.change();
                    response.remocion.empresaResponsable == -1 ? cboEmpresaResponsable.val("") : cboEmpresaResponsable.val(response.remocion.empresaResponsable);
                    response.remocion.empresaInstala == -1 ? cboEmpresaInstalacion.val("") : cboEmpresaInstalacion.val(response.remocion.empresaInstala);
                    cboEmpresaResponsable.change();
                    imgInstalado.attr("src", response.remocion.imgInstalado);
                    imgRemovido.attr("src", response.remocion.imgRemovido);
                    //imgRemovido.attr("src", response.remocion.imgRemovido);
                    if (response.remocion.fechaInstalacionInstaladoRaw != null) {
                        txtFechaInstalacionComponente.datepicker("setDate", response.remocion.fechaInstalacionInstalado);
                        cargarFechaInstalacion();
                    }
                    if (response.remocion.personal != null && response.remocion.personal != "") {
                        var arrPersonal = response.remocion.personal.split(',');
                        for (var i = 0; i < arrPersonal.length - 1; i++) {
                            var JSONINFO = [{ "usuarioID": i, "usuario": arrPersonal[i] }];
                            gridPersonal.bootgrid("append", JSONINFO);
                        }
                    }
                    if (response.remocion.estatus > 1)
                    {
                        cboGarantia.prop("disabled", true);
                        txtPersonal.prop("disabled", true);
                        txtFecha.prop("disabled", true);
                        cboEmpresaResponsable.prop("disabled", true);
                        btncargarImgRemovido.prop("disabled", true);
                        txtacomentario.prop("disabled", true);
                        cboMotivo.prop("disabled", true);
                        cboDestino.prop("disabled", true);
                        
                        if (response.remocion.estatus > 4)
                        {                            
                            txtFechaInstalacionComponente.prop("disabled", true);
                            cboEmpresaInstalacion.prop("disabled", true);
                            btncargarImgInstalado.prop("disabled", true);
                            cboNoComponenteInstalado.prop("disabled", true);
                        }
                    }
                    folio = response.remocion.folioReporte != null ? response.remocion.folioReporte : 0;
                    estadoReporte = response.remocion.estatus;

                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function habilitarBtnAgregarEmp() {
            if (txtPersonal.val() != "") {
                btnAgregarEmpleado.prop("disabled", false);
            }
            else {
                btnAgregarEmpleado.prop("disabled", true);
            }
        }

        function cargarEmpleado() {
            var JSONINFO = [{ "usuarioID": parseInt(txtPersonal.attr("data-index")), "usuario": txtPersonal.val() }];
            gridPersonal.bootgrid("append", JSONINFO);
        }

        function iniciarGrid()
        {            
            gridPersonal.bootgrid({
                rowCount: -1,
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "quitar": function (column, row) {
                        return "<button type='button' class='btn btn-danger quitar'  data-index='" + row.usuarioID + "'" + "data-usuario='" + row.usuario + "'>" +
                        "<span class='glyphicon glyphicon-remove'></span>  </button>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                gridPersonal.find(".quitar").on('click', function (e) {
                    var rowID = parseInt($(this).parent().parent().attr('data-row-id'));
                    gridPersonal.bootgrid("remove", [rowID]);
                })
            });;
            gridPersonal.bootgrid("clear");
        }

        function cargarFechaInstalacion()
        {
            if (cboNoComponenteInstalado.val() != "") {
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    url: '/Overhaul/cargarFechaInstalacion',
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json',
                    data: JSON.stringify({ idComponente: cboNoComponenteInstalado.val() }),
                    success: function (response) {
                        $.unblockUI();
                        txtFechaInstalacionComponente.datepicker('option', 'minDate', moment.utc(response.fecha)._d);
                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", response.message);

                    }
                });
            }
            else { txtFechaInstalacionComponente.datepicker('option', 'minDate', new Date()); }
        }

        init();
    };

    $(document).ready(function () {
        maquinaria.overhaul.remocion = new remocion();
    });
})();


