
var promoverActividadObjLO = {
    promoverID: 0,
    accion: 0
};
var promoverActividadListObjLO = new Array();
var gSessionContinuar = true;
var gSessionInterval;
var gSessionMainInterval;
var _isLocal = false;
var _Local = 'http://localhost:7905';
var _Remoto = window.location.host.includes("sigoplan.construplan") ? 'http://sigoplan.construplan.com.mx' : 'http://10.1.0.126';

mensajeAlerta = "";

$(document).ready(function () {

    setResoluciones(screen.width, screen.height);
    $(".promoverActividadesCantidadLO").removeClass("esteEsDashBoard");

    $('#divAlertaMantenimiento').hide();
    if (localStorage.getItem("alertaMantenimiento") == null) {
        localStorage.setItem("alertaMantenimiento", 0);
    }

    $('#botonAlertaMantenimiento').click(mostrarMensajeAlertaMantenimiento);

    //getAllActividadesAPromoverLO();
    $("#btnCerrarSesion").click(function () {
        $.ajax({
            url: '/Usuario/CerrarSesion',
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json',
            success: function (response) {
                if (response.success === true) {
                    var data = response.items;
                    location.href = "/";
                } else {
                    //MostrarMensaje("Alerta", "No se encontro ningun registro");
                }
            },
            error: function (response) {
                alert(response.message);
            }
        });
    });
    $('.dropdown-submenu').on('click', function (event) {
        // The event won't be propagated up to the document NODE and 
        // therefore delegated events won't be fired
        event.stopPropagation();
    });
    $('.horizontalMenu').find('a').click(function (event) {
        event.preventDefault();
        var tipo = $(this).attr("data-Tipo");
        if (tipo == "1") {
            var id = $(this).attr("data-MenuID");

        } else if (tipo == "2" && $(this).attr("href") != '#') {
            var url = $(this).attr("href");
            var id = $(this).attr("data-MenuID");
            location.href = url;
        } else if (tipo == "3") {
            var id = $(this).attr("data-MenuID");
            var url = $(this).attr("href");
            sistemaExterno(url);
        }
    });
    $("#promoverActividadesLO").on("click", ".notificacionPromover", function () {
        var _this = $(this);
        var promoverID = _this.data("promoverid");
        promoverActividadObjLO.promoverID = promoverID;
        var obj = Enumerable.From(promoverActividadListObjLO).Where(function (x) {
            return x.id == promoverID
        }).Select(function (x) {
            return x
        }).FirstOrDefault();

        $("#txtMinutaLO").val(obj.minuta);
        $("#txtActividadLO").val(obj.actividad);
        $("#txtPromoverLo").val(obj.columna);
        $("#txtEnviadoPor").val(obj.responsable);
        $("#txtDescripcionLO").val(obj.descripcion);
        $("#txtObservacionLO").val(obj.observacion);
        $("#divDialogPromoverLO").modal("show");
    });
    $("#btnPromoverLO").click(function () {
        promoverActividadLO(2);
    });
    $("#btnRechazarLO").click(function () {
        promoverActividadLO(3);
    });
    $("#btnModelSistemas").click(function () {
        modalSistemas();
    });

    $("#btnModelConfig").click(function () {
        window.location = "/Administrador/Usuarios/PerfilUsuario";
    });

    $('.Notificaciones a[data-tipoalerta="4"]').click(function () {
        let alerta_id = $(this).attr('data-id');

        axios.post('/Base/ColocarVistoAlerta', { alerta_id })
            .then(response => {
                let { success, datos, message } = response.data;

                if (success) {

                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
    });

    $("#btnLogOut").click(AbandonarSession);
    $('#divModalMenuSistemas').on("click", "a", fnIniciar);

    if (_permitirAlertaPublicacion == 'true') {
        heartbeat();
        comprobarSession(1000);
        verificarAlarma();
    }
    $('li.dropdown-submenu').hover(
        function () {
            if ($(this).hasClass('setFocus')) {
                $(this).removeClass('setFocus');
            } else {
                $(this).addClass('setFocus');
            }
        }
    );


    $(".desplegaOpcionUsuario").click(function () {
        var _this = $(".opcionesUsuario");
        if (_this.is(":visible")) {
            $(".opcionesUsuario").hide();
        }
        else {
            $(".opcionesUsuario").show();
        }
    });

    $(document).mouseup(function (e) {

        var container = $(".opcionesUsuario");

        if (!container.is(e.target) && container.has(e.target).length === 0) {

            container.hide();

        }
    });
    $(".opcionesUsuario").click(function () {
        $(this).hide();
    });
    setDicionario();

});

function sistemaExterno(_url) {
    $.ajax({
        url: "/Base/getUsuarioExt",
        type: "POST",
        dataType: "json",
        success: function (result) {
            var objExt = result.objExt;
            var a = "http://" + window.location.hostname + _url + '/Login/InitExtDto';
            var b = "http://" + window.location.hostname + _url + '/Home/Principal';
            $.ajax({
                url: a,
                data: {
                    ObjUsuarioExt: objExt, remoto: _Remoto, empresa: _GEmpresaID
                },
                datatype: 'jsonp',
                type: 'POST',
                async: false,
                xhrFields: {
                    withCredentials: true,
                },
                success: function (data) {
                    window.location.href = b;
                }
            });
            window.location.href = b;
        }
    });
}
function setDicionario() {
    getEmpresa().done(function (res) {
        getVistaActual().done(function (res2) {
            let vistasExcepcion = [4143, 4142, 2071, 2069, 4214, 7278, 7279, 7285, 7344, 2095, 7220, 7340, 7341, 7342, 7343, 7399, 7400, 7411, 7413, 7223];
            getVistasExcepcionCC().done(function (res3) {
                if (res == 2 && !res3.includes(res2)) {
                    $(`.RenderBody 
                        span:not(
                        .btn, 
                        .multiselect-native-select, 
                        .multiselect-selected-text, 
                        .select2, .select2-container, .selection, .select2-selection, .select2-selection__rendered
                    ), label:not(.checkbox), th`).each(function (idx, e) {
                        if ($.inArray("CENTRO COSTOS", e.innerText.toUpperCase().split(" ")) > -1)
                            e.innerText = "Area Cuenta";
                        if ($.inArray("CENTRO COSTO", e.innerText.toUpperCase().split(" ")) > -1)
                            e.innerText = "Area Cuenta";
                        if ($.inArray("CENTRO DE COSTO", e.innerText.toUpperCase().split(" ")) > -1)
                            e.innerText = "Area Cuenta";
                        if ($.inArray("CENTRO DE COSTOS", e.innerText.toUpperCase().split(" ")) > -1)
                            e.innerText = "Area Cuenta";
                        if ($.inArray("CC", e.innerText.toUpperCase().split(" ")) > -1)
                            e.innerText = "AC";
                        if ($.inArray("ECONOMICO", e.innerText.toUpperCase().split(" ")) > -1)
                            e.innerText = "CC";
                        if ($.inArray("NO ECONOMICO", e.innerText.toUpperCase().split(" ")) > -1)
                            e.innerText = "Centro Costos";
                        if ($.inArray("NO ECONÓMICO", e.innerText.toUpperCase().split(" ")) > -1)
                            e.innerText = "Centro Costos";
                    });
                }
            });
        });
    });
}

function getEmpresa() {
    return $.post("/Base/getEmpresa");
}

function getVistaActual() {
    return $.post("/Base/getVistaActual");
}
function getVistasExcepcionCC() {
    return $.post("/Base/getVistasExcepcionPalabraCC");
}
function modalSistemas() {
    $.ajax({
        url: '/Usuario/getSistemas',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: {},
        success: function (response) {
            if (response.success === true) {
                if (response.sistemas.length == 1) {
                    window.location.href = '' + response.sistemas[0].url;
                } else {
                    var html = '';
                    $.each(response.sistemas, function (i, e) {
                        if (e.activo == true) {
                            html += '<div class="col-xs-6 col-sm-4 col-md-2 col-lg-2 SubSystemsP">';
                            html += ' <a href="#" data-url="' + e.url + '" data-SistemaID="' + e.id + '" class="btn sizeBtn" role="button"><span><img class="tamano-icono " src="' + e.icono + '"></span> <br />' + e.nombre + '</a>';
                            html += '</div>';
                        } else {
                            html += '<div class="col-xs-6 col-sm-4 col-md-2 col-lg-2 SubSystemsP">';
                            html += ' <span class="btnMenudisabled disabled"><span><img class="tamano-icono " src="' + e.icono + '"></span> <br />' + e.nombre + '</span>';
                            html += '</div>';
                        }
                    });
                    $("#divMenuAcceso").html(html);
                    $('#divModalMenuSistemas').modal('show');
                }
            } else { }
        },
        error: function (response) {
            AlertaGeneral("Error", response.message);
        }
    });
}

function fnIniciar(e) {
    e.preventDefault();
    var _this = $(this);
    var _systemID = _this.data("sistemaid");
    var _url = _this.data("url");
    $.ajax({
        url: "/Base/setCurrentSystem",
        type: "POST",
        dataType: "json",
        data: {
            systemID: _systemID
        },
        success: function (result) {
            if (result.esVirtual) {
                var objExt = result.objExt;
                var a = "http://" + window.location.hostname + _url + '&sistemaID=' + result.sistemaID;

                // if (result.necesitaIngresarDatosEnKontrol) {
                //     let puerto = _url.split('/')[0];
                //     window.location.href = "http://" + window.location.hostname + puerto + result.formularioURL;
                //     // window.location.href = 'http://localhost:3676/Encuestas/EncuestasProveedor/dashboard';
                // }

                window.location.href = a;
            }
            else {
                if (result.externo == true) {
                    var a = "http://" + window.location.hostname + _url + '/Login/InitExtDto';
                    var b = "http://" + window.location.hostname + _url /*+ '/Home/Index/?id=1'*/;
                    var objExt = result.objExt;
                    $.ajax({
                        url: a,
                        data: {
                            ObjUsuarioExt: objExt,
                            remoto: _Remoto,
                            empresa: _GEmpresaID
                        },
                        datatype: 'jsonp',
                        type: 'POST',
                        async: false,
                        xhrFields: {
                            withCredentials: true,
                        },
                        success: function (data) {
                            window.location.href = b;
                        }
                    });
                    window.location.href = b;
                } else {
                    if (result.necesitaIngresarDatosEnKontrol) {
                        window.location.href = result.formularioURL;
                    } else {
                        window.location.href = _url;
                    }
                }
            }
        }
    });
}

function getAllActividadesAPromoverLO() {
    $.ajax({
        datatype: "json",
        type: "POST",
        url: "/SeguimientoAcuerdos/getAllActividadesAPromover",
        data: {},
        success: function (response) {
            if ($(".promoverActividadesCantidadLO").hasClass("esteEsDashBoard")) {
                $("#slOrganigrama").change();
            }
            promoverActividadListObjLO = response.obj;
            var cantidad = promoverActividadListObjLO.length;
            var totalAlertas = Number($(".totalAlertas").html()) + cantidad;
            $(".totalAlertas").html(totalAlertas);
            $(".promoverActividadesCantidadLO").html(cantidad);

            var html = '';
            if (promoverActividadListObjLO.length > 0) {
                $.each(promoverActividadListObjLO, function (i, e) {
                    html += '<li class="notificacionPromover" data-promoverid="' + e.id + '">';
                    html += '   <a href="#">';
                    html += '       <h4>' + e.actividad + '<small class="pull-right">' + e.columna + '%</small></h4>';
                    html += '       <div class="progress xs">';
                    html += '           <div class="progress-bar progress-bar-aqua" style="width: ' + e.columna + '%" role="progressbar" aria-valuenow="' + e.columna + '" aria-valuemin="0" aria-valuemax="100">';
                    html += '               <span class="sr-only">' + e.columna + '%</span>';
                    html += '           </div>';
                    html += '       </div>';
                    html += '   </a>';
                    html += '</li>';
                });
            } else {
                html += '<li class="menuOption"><a href="#">¡Sin alertas!</a></li>';
            }
            $("#promoverActividadesLO").html(html);
        },
        error: function () { }
    });
}

function validarCampoLayout(_this) {
    var r = false;
    if (_this.val() == '' || _this.val() == '0') {
        if (!_this.hasClass("errorClass")) {
            _this.addClass("errorClass")
        }
        r = false;
    } else {
        if (_this.hasClass("errorClass")) {
            _this.removeClass("errorClass")
        }
        r = true;
    }
    return r;
}

function promoverActividadLO(accion) {
    promoverActividadObjLO.accion = accion;
    if (accion == 2) {
        $.ajax({
            datatype: "json",
            type: "POST",
            url: "/SeguimientoAcuerdos/promocionAvanceActividad",
            data: {
                promoverID: promoverActividadObjLO.promoverID,
                accion: promoverActividadObjLO.accion,
                observacion: ""
            },
            success: function (response) {
                getAllActividadesAPromoverLO();
                AlertaGeneral("Confirmación", "Actividad actualizada correctamente");
            },
            error: function () { }
        });
    } else {
        if (validarCampoLayout($("#txtObservacionLORechazo"))) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/SeguimientoAcuerdos/promocionAvanceActividad",
                data: {
                    promoverID: promoverActividadObjLO.promoverID,
                    accion: promoverActividadObjLO.accion,
                    observacion: $("#txtObservacionLORechazo").val()
                },
                success: function (response) {
                    getAllActividadesAPromoverLO();
                    $("#divDialogPromoverLO .close").click();
                    AlertaGeneral("Confirmación", "Actividad actualizada correctamente");
                },
                error: function () { }
            });
        }
    }
}

//Sesion
function comprobarSession(timeout) {
    gSessionMainInterval = setTimeout(function () {
        if (gSessionContinuar) {
            $.ajax({
                type: 'POST',
                url: '/Usuario/ComprobarSession/',
                contentType: 'application/json',
                timeout: 3000,
                cache: false,
                async: false,
                global: false,
                success: function (result) {

                    mostrarDetallesSession(result.object);

                    var new_timeout_minutes = result.object.Restante;

                    if (new_timeout_minutes <= 3) {
                        gSessionContinuar = false;
                        mostrarMensajeExpiracion();
                    } else {
                        var new_timeout_miliseconds = new_timeout_minutes * 60 * 1000;
                        comprobarSession(1000 /*new_timeout_miliseconds - 60000*/);
                    }

                },
                complete: function () { }
            });
        }
    }, timeout);
}

function mostrarDetallesSession(obj) {
    $("#lblDetallesSession").html("UpdateSession: " + obj.UpdateSession + "\n" +
        "ServerTime: " + obj.ServerTime + "\n" +
        "TimeInactive: " + obj.Minutos + "\n" +
        "TimeOut: " + obj.Timeout + "\n" +
        "TimeRemaining: " + obj.Restante);
}

function mostrarMensajeExpiracion() {
    clearInterval(gSessionMainInterval);
    gSessionContinuar = false;
    localStorage.setItem('EstadoSession', 'Espera');
    $("#dialogSesionTimeout").modal("show");
    //$("#btnContinuarSesion").button('reset');
    //$("#btnCerrarSesion").button('reset');
    iniciarCuentaRegresiva($("#dialogSesionMessage"));
}

function iniciarCuentaRegresiva(Mensaje) {

    var lbl = $("#mostrarDialogo");
    var dialogSesionTimeout = $("#dialogSesionTimeout");
    var btnCerrarDialogoSession = $("#btnCerrarDialogoSession");
    var btnContinuarSesion = $("#btnContinuarSesion");
    var btnCerrarSesion = $("#btnCerrarSesion");
    var estadoSession = localStorage.getItem('EstadoSession');

    var time = 180;
    gSessionInterval = setInterval(function () {
        if (time > 0) {
            var hours = Math.floor(time / 3600);
            var minutes = Math.floor((time % 3600) / 60);
            var seconds = time % 60;

            //Anteponiendo un 0 a los minutos si son menos de 10 
            minutes = minutes < 10 ? '0' + minutes : minutes;

            //Anteponiendo un 0 a los segundos si son menos de 10 
            seconds = seconds < 10 ? '0' + seconds : seconds;

            var result = hours + ":" + minutes + ":" + seconds; // 2:41:30
            time--;
            Mensaje.html("Se cerrara sesión en " + result);
            estadoSession = localStorage.getItem('EstadoSession');


            if (estadoSession == 'continuar') {
                clearInterval(gSessionInterval);
                $("#dialogSesionTimeout").modal("hide");
                //comprobarSession(60000);
                gSessionContinuar = true;
            } else if (estadoSession == 'abandonar') {
                gSessionContinuar = false;
                AbandonarSession();
            }
        } else {
            AbandonarSession();
        }
    }, 1000);


    btnCerrarDialogoSession.click(function () {
        localStorage.setItem('EstadoSession', 'continuar');
    });

    btnContinuarSesion.click(function () {
        btnContinuarSesion.button('loading');
        localStorage.setItem('EstadoSession', 'continuar');
        clearInterval(gSessionInterval);
        ContinuarSession();
    });

    btnCerrarSesion.click(function () {
        btnCerrarSesion.button('loading');
        localStorage.setItem('EstadoSession', 'abandonar');
        clearInterval(gSessionInterval);
        AbandonarSession();
    });
}

function ValidarSession() {
    $.ajax({
        type: 'POST',
        url: "/Usuario/ComprobarSession/",
        contentType: 'application/json',
        timeout: 3000,
        cache: false,
        async: false,
        success: function (result) {

            //mostrarDetallesSession(result.object);
            var new_timeout_minutes = result.object.Restante;

            if (new_timeout_minutes <= 0) {
                AbandonarSession();
            }

            $("#dialogSesionTimeout").modal("hide");
            localStorage.setItem('EstadoSession', 'abandonar');

        },
        complete: function () { }
    });
}

function ContinuarSession() {
    $.ajax({
        type: 'POST',
        url: "/Usuario/ContinuarSession/",
        contentType: 'application/json',
        timeout: 1000,
        cache: false,
        async: false,
        success: function (result) {
            gSessionContinuar = true;
            //var new_timeout_minutes = result.object.Restante;

            //if (new_timeout_minutes <= 1) {
            //    mostrarMensajeExpiracion();
            //}
            //else {
            //    var new_timeout_miliseconds = new_timeout_minutes * 60 * 1000;
            //    comprobarSession(new_timeout_miliseconds - 60000);
            //}
            comprobarSession(1000);
        },
        complete: function () { }
    });
}

function AbandonarSession() {
    $.ajax({
        type: 'POST',
        url: '/Usuario/AbandonarSession/',
        contentType: 'application/json',
        timeout: 1000,
        cache: false,
        async: false,
        success: function (result) { },
        complete: function () {
            var url = new URL(_SessionMainURL);
            if (_SessionMainURL != 'http://localhost:3676') {
                url.port = '';
            }
            window.location.href = url;
        }
    });
}


//Alerta Mantenimiento
function verificarAlarma() {
    setInterval(verificarAlertaMantenimiento, 1000);
}

function verificarAlertaMantenimiento() {
    $.ajax({
        type: 'POST',
        url: "/Usuario/VerificarAlarmaMantenimiento/",
        global: false
    }).done(response => {
        if (response.success) {
            if (response.activo) {

                const alertaNoVista = localStorage.getItem('alertaMantenimiento') === "0";

                mensajeAlerta = response.mensajeAlerta;

                if (alertaNoVista) {
                    mostrarMensajeAlertaMantenimiento();
                    localStorage.setItem("alertaMantenimiento", 1);
                }

                if (response.vencido) {
                    localStorage.clear();
                    window.location.href = response.redirectURL;
                } else {
                    $('#divAlertaMantenimiento').show();
                    $('#spanMensajeMantenimiento').html(`<i class="fas fa-exclamation-triangle"></i>${response.tiempoRestante}`);
                }
            }
        }
    });
}

function mostrarMensajeAlertaMantenimiento() {
    AlertaGeneral(`AVISO`, `${mensajeAlerta}`);
}

function _OcultarHeader_Footer_SIGOPLAN() {
    $("#_headersigoplan").hide();
    $("#_bodysigoplan").css({ "position": "absolute", "top": 0, "paddingRight": "0px", "paddingLeft": "0px", "marginRight": "auto", "marginLeft": "auto", "marginTop": "0px", "marginBottom": "0px" });
    $("#_footersigoplan").hide();
}
function _MostrarHeader_Footer_SIGOPLAN() {
    $("#_headersigoplan").show();
    $("#_bodysigoplan").css({ "position": "relative", "top": "15px", "paddingRight": "0px", "paddingLeft": "0px", "marginRight": "auto", "marginLeft": "auto", "marginTop": "15px", "marginBottom": "15px" });
    $("#_footersigoplan").show();
}