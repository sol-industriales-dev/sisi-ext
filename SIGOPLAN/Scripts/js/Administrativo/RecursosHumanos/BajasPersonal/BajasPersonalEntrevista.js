(() => {
    $.namespace('CH.BajasPErsonalEntrevista');

    //#region CONST ENTREVISTA
    const txtSubtitulo = $('#txtSubtitulo');

    const fieldRegistro = $('#fieldRegistro');
    const fieldEntrevista = $("#fieldEntrevista");
    const txt_entrevista_gerenteClave = $("#txt_entrevista_gerenteClave");
    const txt_entrevista_gerenteNombre = $("#txt_entrevista_gerenteNombre");
    const txt_entrevista_fechaNacimiento = $("#txt_entrevista_fechaNacimiento");
    const cbo_entrevista_estadoCivilID = $("#cbo_entrevista_estadoCivilID");
    const cbo_entrevista_escolaridadID = $("#cbo_entrevista_escolaridadID");
    const cbo_entrevista_p1 = $("#cbo_entrevista_p1");
    const cbo_entrevista_p2 = $("#cbo_entrevista_p2");
    const cbo_entrevista_p3_1 = $("#cbo_entrevista_p3_1");
    const cbo_entrevista_p3_2 = $("#cbo_entrevista_p3_2");
    const cbo_entrevista_p3_3 = $("#cbo_entrevista_p3_3");
    const cbo_entrevista_p3_4 = $("#cbo_entrevista_p3_4");
    const cbo_entrevista_p3_5 = $("#cbo_entrevista_p3_5");
    const cbo_entrevista_p3_6 = $("#cbo_entrevista_p3_6");
    const cbo_entrevista_p3_7 = $("#cbo_entrevista_p3_7");
    const cbo_entrevista_p3_8 = $("#cbo_entrevista_p3_8");
    const cbo_entrevista_p3_9 = $("#cbo_entrevista_p3_9");
    const cbo_entrevista_p3_10 = $("#cbo_entrevista_p3_10");
    const cbo_entrevista_p4 = $("#cbo_entrevista_p4");
    const cbo_entrevista_p5 = $("#cbo_entrevista_p5");
    const txt_entrevista_p6 = $("#txt_entrevista_p6");
    const txt_entrevista_p7 = $("#txt_entrevista_p7");
    const cbo_entrevista_p8 = $("#cbo_entrevista_p8");
    const txt_entrevista_p8_porque = $("#txt_entrevista_p8_porque");
    const cbo_entrevista_p9 = $("#cbo_entrevista_p9");
    const txt_entrevista_p9_porque = $("#txt_entrevista_p9_porque");
    const cbo_entrevista_p10 = $("#cbo_entrevista_p10");
    const txt_entrevista_p10_porque = $("#txt_entrevista_p10_porque");
    const cbo_entrevista_p11 = $("#cbo_entrevista_p11");
    const cbo_entrevista_p11_1 = $("#cbo_entrevista_p11_1");
    const cbo_entrevista_p12 = $("#cbo_entrevista_p12");
    const txt_entrevista_p12_porque = $("#txt_entrevista_p12_porque");
    const cbo_entrevista_p13 = $("#cbo_entrevista_p13");
    const cbo_entrevista_p14 = $("#cbo_entrevista_p14");
    const txt_entrevista_fechaAproximada = $("#txt_entrevista_fechaAproximada");
    const txt_entrevista_comoFue = $("#txt_entrevista_comoFue");
    const txt_registro_fechaIngreso = $('#txt_registro_fechaIngreso');
    const divPreguntaEstancia = $('#divPreguntaEstancia');

    const btnGuardarEntrevista = $('#btnGuardarEntrevista');

    const params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
    //#endregion

    BajasPErsonalEntrevista = function () {
        (function init() {
            fncGetCapturada(params.id, params.empresa);
            fncGetBaja(params.id, params.empresa);
            fncListeners();
        })();

        function fncListeners() {

            btnGuardarEntrevista.on("click", function () {
                fncCrearEditarEntrevista();
            });

            $(".select2").select2();
            $(".select2").select2({ width: "100%" });

            //#region EVENTOS ENTREVISTA
            cbo_entrevista_p1.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 1 }, false, "--Seleccione--");
            cbo_entrevista_p2.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 2 }, false, "--Seleccione--");
            cbo_entrevista_p3_1.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 3 }, false, "--Seleccione--");
            cbo_entrevista_p3_2.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 3 }, false, "--Seleccione--");
            cbo_entrevista_p3_3.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 3 }, false, "--Seleccione--");
            cbo_entrevista_p3_4.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 3 }, false, "--Seleccione--");
            cbo_entrevista_p3_5.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 3 }, false, "--Seleccione--");
            cbo_entrevista_p3_6.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 3 }, false, "--Seleccione--");
            cbo_entrevista_p3_7.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 3 }, false, "--Seleccione--");
            cbo_entrevista_p3_8.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 3 }, false, "--Seleccione--");
            cbo_entrevista_p3_9.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 3 }, false, "--Seleccione--");
            cbo_entrevista_p3_10.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 3 }, false, "--Seleccione--");
            cbo_entrevista_p4.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 4 }, false, "--Seleccione--");
            cbo_entrevista_p5.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 5 }, false, "--Seleccione--");
            cbo_entrevista_p8.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 8 }, false, "--Seleccione--");
            cbo_entrevista_p9.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 9 }, false, "--Seleccione--");
            cbo_entrevista_p10.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 10 }, false, "--Seleccione--");
            cbo_entrevista_p11.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 11 }, false, "--Seleccione--");
            cbo_entrevista_p11_1.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 11 }, false, "--Seleccione--");
            cbo_entrevista_p12.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 12 }, false, "--Seleccione--");
            cbo_entrevista_p13.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 13 }, false, "--Seleccione--");
            cbo_entrevista_p14.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboPreguntas', { idPregunta: 14 }, false, "--Seleccione--");
            cbo_entrevista_estadoCivilID.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboEstadosCiviles', null, false, "--Seleccione--");
            cbo_entrevista_escolaridadID.fillCombo('/Administrativo/BajasPersonalEntrevista/FillCboEscolaridades', null, false, "--Seleccione--");
            //#endregion

            txt_entrevista_gerenteClave.on("change", function () {
                fncGetDatosPersonal(true);
            });

            txt_entrevista_gerenteNombre.getAutocomplete(funGetGerente, { term: "", empresa: params.empresa }, '/Administrativo/BajasPersonalEntrevista/getEmpleadosGeneral');

            cbo_entrevista_p14.on("change", function () {
                console.log(cbo_entrevista_p14.val())
                if (cbo_entrevista_p14.val() == "65") {
                    divPreguntaEstancia.css("display", "initial");
                } else if (cbo_entrevista_p14.val() == "66") {
                    divPreguntaEstancia.css("display", "none");
                } else {
                    divPreguntaEstancia.css("display", "none");
                }
            });
        }

        //#region BACK END
        function funGetGerente(event, ui) {
            txt_entrevista_gerenteClave.val(ui.item.id);
            txt_entrevista_gerenteNombre.val(ui.item.value);

        }

        //SE ASIGNA LA EMPRESA A CONSULTAR EN BajasPersonalEntrevistaDAO
        function fncGetCapturada(idReg, empresa) {
            axios.post("GetCapturada", { idRegistro: idReg, empresa }).then(response => {
                let { success, items, message } = response.data;
                if (!success) {
                    btnGuardarEntrevista.prop("disabled", true);
                    Alert2Warning("Esta entrevista ya fue capturada");
                    setInterval(function () {
                        window.location.href = 'http://sigoplan.construplan.com.mx';
                    }, 1000);

                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetBaja(idRegistro, empresa) {
            axios.post("GetBaja", { id: idRegistro, empresa }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    console.log(items);
                    console.log(response.data);
                    txtSubtitulo.text(`Numero Empleado: ${items.numeroEmpleado}. Nombre: ${items.nombre}. Fecha de Baja: ${moment(items.fechaBaja).format("DD/MM/YYYY")}.`);
                    txt_registro_fechaIngreso.val(moment(response.data.fecha_ingreso).format("DD/MM/YYYY"));

                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearEditarEntrevista() {
            let obj = fncGetObjCEBajaPersonal();

            if (obj != "") {
                axios.post("CrearEditarEntrevista", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Warning(message);
                        setInterval(function () {
                            window.location.href = 'http://sigoplan.construplan.com.mx';
                        }, 1000);
                    } else {
                        Alert2Warning(message);
                        setInterval(function () {
                            window.location.href = 'http://sigoplan.construplan.com.mx';
                        }, 1000);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGetDatosPersonal(esGerente) {
            let obj = new Object();
            obj = {
                claveEmpleado: txt_entrevista_gerenteClave.val(),
                nombre: txt_entrevista_gerenteNombre.val()
            }
            axios.post("GetDatosPersona", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (!esGerente) {
                    } else {
                        txt_entrevista_gerenteNombre.val(response.data.objDatosPersona.nombreCompleto);
                        //Alert2Warning(message);
                        btnGuardarEntrevista.attr("disabled");
                    }
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region FUNCS GENERALES

        function fncGetObjCEBajaPersonal() {
            let strMensajeError = "";

            if (txt_entrevista_gerenteClave.val() == "") { txt_entrevista_gerenteClave.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txt_entrevista_gerenteNombre.val() == "") { txt_entrevista_gerenteNombre.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txt_entrevista_fechaNacimiento.val() == "") { txt_entrevista_fechaNacimiento.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_estadoCivilID.val() == "--Seleccione--") { $("#select2-cbo_entrevista_estadoCivilID-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_escolaridadID.val() == "--Seleccione--") { $("#select2-cbo_entrevista_escolaridadID-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_p1.val() == "--Seleccione--") { $("#select2-cbo_entrevista_p1-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_p2.val() == "--Seleccione--") { $("#select2-cbo_entrevista_p2-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_p3_1.val() == "--Seleccione--") { $("#select2-cbo_entrevista_p3_1-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_p3_2.val() == "--Seleccione--") { $("#select2-cbo_entrevista_p3_2-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_p3_3.val() == "--Seleccione--") { $("#select2-cbo_entrevista_p3_3-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_p3_4.val() == "--Seleccione--") { $("#select2-cbo_entrevista_p3_4-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_p3_5.val() == "--Seleccione--") { $("#select2-cbo_entrevista_p3_5-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_p3_6.val() == "--Seleccione--") { $("#select2-cbo_entrevista_p3_6-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_p3_7.val() == "--Seleccione--") { $("#select2-cbo_entrevista_p3_7-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_p3_8.val() == "--Seleccione--") { $("#select2-cbo_entrevista_p3_8-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_p3_9.val() == "--Seleccione--") { $("#select2-cbo_entrevista_p3_9-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_p3_10.val() == "--Seleccione--") { $("#select2-cbo_entrevista_p3_10-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_p4.val() == "--Seleccione--") { $("#select2-cbo_entrevista_p4-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_p5.val() == "--Seleccione--") { $("#select2-cbo_entrevista_p5-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txt_entrevista_p6.val() == "") { txt_entrevista_p6.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txt_entrevista_p7.val() == "") { txt_entrevista_p7.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_p8.val() == "--Seleccione--") { $("#select2-cbo_entrevista_p8-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txt_entrevista_p8_porque.val() == "") { txt_entrevista_p8_porque.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_p9.val() == "--Seleccione--") { $("#select2-cbo_entrevista_p9-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txt_entrevista_p9_porque.val() == "") { txt_entrevista_p9_porque.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_p10.val() == "--Seleccione--") { $("#select2-cbo_entrevista_p10-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txt_entrevista_p10_porque.val() == "") { txt_entrevista_p10_porque.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_p11.val() == "--Seleccione--") { $("#select2-cbo_entrevista_p11-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_p11_1.val() == "--Seleccione--") { $("#select2-cbo_entrevista_p11_1-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_p12.val() == "--Seleccione--") { $("#select2-cbo_entrevista_p12-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txt_entrevista_p12_porque.val() == "") { txt_entrevista_p12_porque.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_entrevista_p13.val() == "--Seleccione--") { $("#select2-cbo_entrevista_p13-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                let dateIngreso = txt_registro_fechaIngreso.val().split('/');
                obj = {
                    id: params.id,
                    //#region ENTREVISTA
                    fechaIngreso: `${dateIngreso[2]}-${dateIngreso[1]}-${dateIngreso[0]}`,
                    gerente_clave: txt_entrevista_gerenteClave.val(),
                    nombreGerente: txt_entrevista_gerenteNombre.val(),
                    fecha_nacimiento: txt_entrevista_fechaNacimiento.val(),
                    estado_civil_clave: cbo_entrevista_estadoCivilID.val(),
                    escolaridad_clave: cbo_entrevista_escolaridadID.val(),
                    p1_clave: cbo_entrevista_p1.val(),
                    p2_clave: cbo_entrevista_p2.val(),
                    p3_1_clave: cbo_entrevista_p3_1.val(),
                    p3_2_clave: cbo_entrevista_p3_2.val(),
                    p3_3_clave: cbo_entrevista_p3_3.val(),
                    p3_4_clave: cbo_entrevista_p3_4.val(),
                    p3_5_clave: cbo_entrevista_p3_5.val(),
                    p3_6_clave: cbo_entrevista_p3_6.val(),
                    p3_7_clave: cbo_entrevista_p3_7.val(),
                    p3_8_clave: cbo_entrevista_p3_8.val(),
                    p3_9_clave: cbo_entrevista_p3_9.val(),
                    p3_10_clave: cbo_entrevista_p3_10.val(),
                    p4_clave: cbo_entrevista_p4.val(),
                    p5_clave: cbo_entrevista_p5.val(),
                    p6_concepto: txt_entrevista_p6.val(),
                    p7_concepto: txt_entrevista_p7.val(),
                    p8_clave: cbo_entrevista_p8.val(),
                    p8_porque: txt_entrevista_p8_porque.val(),
                    p9_clave: cbo_entrevista_p9.val(),
                    p9_porque: txt_entrevista_p9_porque.val(),
                    p10_clave: cbo_entrevista_p10.val(),
                    p10_porque: txt_entrevista_p10_porque.val(),
                    p11_1_clave: cbo_entrevista_p11.val(),
                    p11_2_clave: cbo_entrevista_p11_1.val(),
                    p12_clave: cbo_entrevista_p12.val(),
                    p12_porque: txt_entrevista_p12_porque.val(),
                    p13_clave: cbo_entrevista_p13.val(),
                    p14_clave: cbo_entrevista_p14.val(),
                    p14_fecha: txt_entrevista_fechaAproximada.val(),
                    p14_porque: txt_entrevista_comoFue.val(),
                    empresa: params.empresa
                    //#endregion
                }
                return obj;
            }


        }

        //#endregion
    }

    $(document).ready(() => {
        CH.BajasPErsonalEntrevista = new BajasPErsonalEntrevista();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();