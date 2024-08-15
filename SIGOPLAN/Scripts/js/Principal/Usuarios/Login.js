var _isLocal = true;
var _Local = 'http://localhost:3676';
var _Remoto = 'http://66.175.239.161';
(function () {
    $.namespace('sigoplan.principal.login');
    localStorage.removeItem('MenuSelected');
    Login = function () {
        txtUsuario = $("#txtUsuario");
        txtPassword = $("#txtPassword");
        btnEntrar = $("#btnEntrar");
        formLogin = $("#formLogin");
        txtAll = $(".input");
        divMenuAcceso = $("#divMenuAcceso");
        modalMenuPpal = $("#modalMenuPpal");

        const modalEliminar = $("#modalEliminar");
        const mensajeErrorLogin = $('#mensajeErrorLogin');

        function init() {
            bestRouting(true);
            txtAll.focusin(animateFocusin).focusout(animateFocusout).keydown(loginEnter);
            btnEntrar.click(beforeLogIn);
            modalMenuPpal.on("click", "a", fnIniciarNuevo);
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
                data: { systemID: _systemID },
                success: function (result) {
                    if (result.esVirtual) {
                        var objExt = result.objExt;
                        var a = (_isLocal ? _Local : _Remoto) + result.auxURL + '&sistemaID=' + result.sistemaID;
                        window.location.href = a;
                    }
                    else {
                        if (result.externo == true) {
                            var objExt = result.objExt;
                            var a = (_isLocal ? _Local : _Remoto) + ':8080/Login/InitExtDto';
                            var b = (_isLocal ? _Local : _Remoto) + ':8080/Home/Principal';
                            $.ajax({
                                url: a,
                                data: {
                                    ObjUsuarioExt: objExt, remoto: _Remoto, empresa: 1
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
                        else {
                            window.location.href = _url;
                        }
                    }

                }
            });
        }

        function fnIniciarNuevo(e) {
            e.preventDefault();
            var _this = $(this);
            var _systemID = _this.data("sistemaid");
            var _url = _this.data("url");
            $.ajax({
                url: "/SISTEMA/Sistema/SendReedireccion",
                type: "POST",
                dataType: "json",
                data: { empresaID: _systemID },
                success: function (result) {
                    var objExt = result.objExt;
                    var _objRedireect = result.objURLSistema;
                    if (result.externo == true) {
                        var a = (_isLocal ? _Local : _Remoto) + ':8080/SISTEMA/Sistema/GetReedireccion';
                        window.location.href = window.location.href = b;
                    }
                    else {
                        window.location.href = _objRedireect;

                    }
                }
            });
        }

        function loginEnter(e) {
            var key = e.which;
            if (key == 13) {
                beforeLogIn();
            }
        }

        function LogIn(obj) {
            bestRouting(false);
            $.ajax({
                url: '/Usuario/getLogin',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                success: function (response) {
                    if (response.success === true) {
                        setResoluciones(screen.width, screen.height);

                        if (response.externoSeguridad) {
                            var _systemID = 10;
                            var _url = ':8082';
                            //var _url = ':7905';

                            $.ajax({
                                url: "/Base/setCurrentSystem",
                                type: "POST",
                                dataType: "json",
                                data: { systemID: _systemID },
                                success: function (result) {
                                    if (result.externo == true) {
                                        var objExt = result.objExt;
                                        var a = "http://" + window.location.hostname + _url + '/Login/InitExtDto';
                                        var b = "http://" + window.location.hostname + _url + '/Home/Principal';
                                        $.ajax({
                                            url: a,
                                            data: {
                                                ObjUsuarioExt: objExt, remoto: _Remoto, empresa: 1
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
                                    else {
                                        window.location.href = _url;
                                    }
                                }
                            });
                        } else {
                            if (response.sistemas.length == 1 && !response.VistaProcedimientos) {

                                if (response.VistaGestorCorporativo) {
                                    window.location.href = '/GestorCorporativo/GestorCorporativo';
                                }

                                $.ajax({
                                    url: "/Base/setCurrentSystem",
                                    type: "POST",
                                    dataType: "json",
                                    data: { systemID: response.sistemas[0].id },
                                    success: function () {
                                        window.location.href = '' + response.sistemas[0].url;
                                    }
                                });
                            } else {

                                if (response.VistaCalendario) {
                                    window.location.href = '/Administrativo/ReservacionVehiculo/Calendario';
                                }
                                else {
                                    var html = '';

                                    var offi = response.sistemas.length > 3 ? 0 : response.sistemas.length > 2 ? 1 : 2;
                                    $.each(response.sistemas, function (i, e) {
                                        var Ioffi = ((offi == 1 && i == 0) ? 0 : offi);
                                        if (e.activo == true) {
                                            html += '<div class="col-md-offset-' + Ioffi + ' col-md-3">';
                                            html += ' <a href="#" data-url="' + e.url + '" data-SistemaID="' + e.id + '" class="btn" role="button"><span><img class="tamano-icono " src="' + e.icono + '"></span> <br />' + e.nombre + '</a>';
                                            html += '</div>';
                                        }
                                        else {
                                            html += '<div class="col-md-offset-' + Ioffi + ' col-md-3">';
                                            html += ' <span class="btndisabled disabled"><span><img class="tamano-icono " src="' + e.icono + '"></span> <br />' + e.nombre + '</span>';
                                            html += '</div>';
                                        }

                                        i--;
                                    });

                                    if (response.setReedireccion) {
                                        var html = '';
                                        var contador = 0;
                                        $.each(response.sistemas, function (i, e) {
                                            contador++;
                                            var Ioffi = ((offi == 1 && i == 0) ? 0 : offi);
                                            if (e.activo == true) {
                                                 //html += '<a href="#" data-url="' + e.url + '" data-SistemaID="' + e.id + '" class="btn btn-primary" >' + e.nombre + '</a>';
                                                 html += '<div class="col-md-offset-' + Ioffi + ' col-md-3">';
                                                //html += '<div class="col-sm-4" style="margin-bottom: 35px;">'
                                                html += ' <a href="#" data-url="' + e.url + '" data-SistemaID="' + e.id + '" class="btn" role="button"><span><img class="tamano-icono " src="' + e.icono + '"></span> <br />' + e.nombre + '</a>';
                                                //html += ' <a href="#" data-url="' + e.url + '" data-SistemaID="' + e.id + '" class="btn" role="button"><span><img class="tamano-icono " src="' + e.icono + '"></span></a>';

                                                html += '</div>';
                                            }
                                            else {
                                                html += '<div class="col-md-offset-' + Ioffi + ' col-md-3">';
                                                html += ' <span class="btndisabled disabled"><span><img class="tamano-icono " src="' + e.icono + '"></span> <br />' + e.nombre + '</span>';
                                                html += '</div>';
                                            }
                                        });

                                        divMenuAcceso.html('');
                                        divMenuAcceso.html(html);
                                        modalMenuPpal.modal('show');

                                    }
                                    else {
                                        var empresaActual = response.empresaActual;
                                        $.ajax({
                                            url: "/SISTEMA/Sistema/SendReedireccion",
                                            type: "POST",
                                            dataType: "json",
                                            data: { empresaID: empresaActual.id },
                                            success: function (result) {
                                                var objExt = result.objExt;
                                                var _objRedireect = result.objURLSistema;
                                                if (result.externo == true) {
                                                    var a = (_isLocal ? _Local : _Remoto) + ':8080/SISTEMA/Sistema/GetReedireccion';
                                                    window.location.href = window.location.href = b;;
                                                }
                                                else {
                                                    window.location.href = _objRedireect;

                                                }
                                            }
                                        });
                                    }
                                }
                            }
                        }
                    }
                    else {
                        modalEliminar.modal('show');
                        mensajeErrorLogin.html(`${response.message ? response.message : 'Datos de acceso inválidos.'}`);
                    }
                },
                error: function (response) {
                    AlertaLogin("Error", response.message);
                }
            });
        }
        function beforeLogIn() {

            if (valid()) {
                LogIn(getPlainObject());
            }
        }
        function valid() {
            var state = true;
            if (!txtUsuario.valid()) { state = false; }
            if (!txtPassword.valid()) { state = false; }
            return state;
        }
        function getPlainObject() {
            return {
                nombreUsuario: txtUsuario.val().trim(),
                contrasena: txtPassword.val().trim()
            }
        }
        function animateFocusin() {
            $(this).find("span").animate({ "opacity": "0" }, 200);
        }
        function animateFocusout() {
            $(this).find("span").animate({ "opacity": "1" }, 300);
        }
        init();
    };
    $(document).ready(function () {

        sigoplan.principal.Login = new Login();
    });
})();