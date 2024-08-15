(() => {
    $.namespace('shared._authPanel');
    _authPanel = function () {
        var formatosID = 0;
        const report = $('#report');
        const mdlPanelAuth = $('#mdlPanelAuth');
        const divAuthPanel = $('#divAuthPanel');
        const lblAuthMessage = $('#lblAuthMessage');
        const divAutorizantes = $('#divAutorizantes');
        let init = () => {
            setIframeResolution();
        }
        EnTurno = null;
        objPanelAuth = {
            idPanelReporte: 0,
            urlLstAuth: ``,
            urlAuth: ``,
            urlRech: ``,
            callbackAuth: null,
            callbackRech: null
        }
        function setIframeResolution() {
            let height = screen.height;
            if (height > 769) {
                report.css("height", "66.3em");
            } else {
                report.css("height", "43em");
            }
        }

        setPanelAutorizantes = (objLstAuth) => {
            lblAuthMessage.text("");
            report.contents().find("body").html("<center><h3 style='color:white;font-weight:bold;'><br/><br/>Cargando información...</h3></center>");
            report.contents().find('body').css('backgroundColor', 'rgb(142, 142, 142)');
            axios.post(objPanelAuth.urlLstAuth, objLstAuth)
                .then(response => {
                    let { success } = response.data;
                    if (success) {
                        createPanelAuth(response.data);
                        formatosID = objLstAuth.id;
                        setPanelReporte(objLstAuth.id);
                        mdlPanelAuth.modal(`show`);
                    }
                }).catch(o_O => console.log(o_O.message));
        }
        setPanelReporte = (formatoID) => {
            report.attr("src", `/Reportes/Vista.aspx?idReporte=${objPanelAuth.idPanelReporte}&fId=${formatoID}`);
            document.getElementById('report').onload = () => {
            }
        };
        setAuth = () => {
            axios.post(objPanelAuth.urlAuth, EnTurno)
                .then(response => {
                    if (response.data.success) {
                        report.attr("src", `/Reportes/Vista.aspx?idReporte=${objPanelAuth.idPanelReporte}&fId=${formatosID}&inMemory=1`);
                        document.getElementById('report').onload = () => {
                            if (objPanelAuth.callbackAuth) {
                                objPanelAuth.callbackAuth(response.data);
                            }
                            mdlPanelAuth.modal(`hide`);
                            AlertaGeneral("Aviso", "Autorización firmada con éxito");
                        }
                    } else {
                        AlertaGeneral("Aviso", response.data.message);
                    }
                }).catch(o_O => console.log(o_O.message));
        };
        setRech = () => {
            AlertaAceptarRechazar("Aviso", `<p>¿Cúal es el motivo de rechazo?</p><textarea rows="4" cols="70" class="form-control comentarioRechazo"></textarea>`, objPanelAuth.urlRech, null)
                .then(btn => {
                    EnTurno.comentario = $(".comentarioRechazo").val();
                    axios.post(objPanelAuth.urlRech, EnTurno).then(response => {
                        if (response.data.success) {
                            report.attr("src", `/Reportes/Vista.aspx?idReporte=${objPanelAuth.idPanelReporte}&fId=${formatosID}&inMemory=1`);
                            document.getElementById('report').onload = () => {
                                if (objPanelAuth.callbackRech) {
                                    objPanelAuth.callbackRech(response.data);
                                }
                                mdlPanelAuth.modal(`hide`);
                                AlertaGeneral("Aviso", "Autorización rechazada con éxito");
                            }
                        } else {
                            AlertaGeneral("Aviso", response.data.message);
                        }

                    }).catch(o_O => console.log(o_O.message));
                })
        };
        createPanelAuth = ({ autorizantes, message }) => {
            EnTurno = null;
            divAutorizantes.html(``);
            autorizantes.forEach(auth => {
                let panel = $("<div>");
                let encabezado = $("<div>");
                let cuerpo = $(`<div>`);
                let pie = $(`<div>`);
                let color = ColorDesdeEstado(auth);
                panel.data(auth);
                panel.addClass("panel panel-default text-center");
                encabezado.addClass(`panel-heading ${color}`);
                cuerpo.addClass(`panel-body`);
                pie.addClass(`panel-footer ${auth.clase} ${color}`);
                encabezado.text(auth.nombre);
                cuerpo.text(auth.descripcion);
                pie.text(auth.clase);
                if (auth.authEstado === 3) {
                    EnTurno = auth;
                    let btnAuth = $(`<button>`);
                    let btnRech = $(`<button>`);
                    btnAuth.addClass(`btn btn-success btn-xs pull-right btnAuth`);
                    btnRech.addClass(`btn btn-danger btn-xs pull-left btnRech`);
                    btnAuth.html(`<i class="fa fa-check"></i>`);
                    btnRech.html(`<i class="fas fa-times"></i>`);
                    pie.text(`Autorice`);
                    btnAuth.click(setAuth);
                    btnRech.click(setRech);
                    pie.append(btnAuth);
                    pie.append(btnRech);
                }
                panel.append(encabezado);
                panel.append(cuerpo);
                panel.append(pie);
                divAutorizantes.append(panel);
            });
            if (message !== null) {
                lblAuthMessage.text(message);
            }
        };
        function ColorDesdeEstado({ authEstado }) {
            switch (authEstado) {
                case 0:
                    return "Espera";
                case 1:
                    return "Autorizado";
                case 2:
                    return "Rechazado";
                case 3:
                    return "AutorizanteEnTurno";
                default:
                    return "";
            }
        }
        init();
    }
    $(document).ready(() => {
        shared._authPanel = new _authPanel();
    }).ajaxStart(() => {
        $.blockUI({
            message: 'Procesando...',
            baseZ: 2000
        });
    }).ajaxStop(() => { $.unblockUI(); });;
})();