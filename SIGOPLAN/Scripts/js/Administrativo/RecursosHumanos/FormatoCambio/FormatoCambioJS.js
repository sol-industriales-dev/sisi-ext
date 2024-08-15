(function () {
    $.namespace('recursoshumanos.formatocambio.FormatoCambio');

    FormatoCambio = function () {

        mensajes = {
            PROCESANDO: 'Enviando Correo...'
        };
        /*********************************Declaracion de Variables***************************************/

        /******* Variables Locales *****************/

        var objEmpleadoCambios = {
            id: 0,
            Clave_Empleado: 0,
            Nombre: "",
            Ape_Paterno: "",
            Ape_Materno: "",
            Fecha_Alta: "",
            PuestoID: 0,
            Puesto: "",
            TipoNominaID: 0,
            TipoNomina: "",
            CcID: "",
            CC: "",
            RegistroPatronalID: 0,
            RegistroPatronal: "",
            Clave_Jefe_Inmediato: 0,
            Nombre_Jefe_Inmediato: "",
            Salario_Base: 0.0,
            Complemento: 0.0,
            InicioNomina: "",
            FechaInicioCambio: "",
            Justificacion: "",
            Folio: "",
            CamposCambiados: ""
        };

        var argAutorizaciones = [];
        var objAutorizacion = {
            Clave_Aprobador: 0,
            Nombre_Aprobador: "",
            PuestoAprobador: "",
            Autorizando: false,
            Orden: 0,
            tipoAutoriza: 0
        }
        groupAmbosAut = $(".groupAmbosAut");

        /******* Variables divInfoUsuario **********/
        txtBonoNominaChange = $("#txtBonoNominaChange"),
            lblBonoMensualChange = $("#lblBonoMensualChange"),
            lblIgBonoNomina = $("#lblIgBonoNomina"),
            lblIgBonoMensual = $("#lblIgBonoMensual"),

            ireport = $("#report");
        FormatoId = $("#FormatoId");
        txtIgNumeroEmpleado = $("#txtIgNumeroEmpleado");

        lblIgNombre = $("#lblIgNombre");
        lblIgIngreso = $("#lblIgIngreso");
        lblIgPuesto = $("#lblIgPuesto");
        lblIgNomina = $("#lblIgNomina");
        lblIgCentro = $("#lblIgCentro");
        lblIgJefe = $("#lblIgJefe");
        lblIgRegistro = $("#lblIgRegistro");
        lblIgDepartamento = $('#lblIgDepartamento');
        lblIgSueldoNomina = $("#lblIgSueldoNomina");
        lblIgSueldoMensual = $("#lblIgSueldoMensual");
        lblIgComplementoNomina = $("#lblIgComplementoNomina");
        lblIgComplementoMensual = $("#lblIgComplementoMensual");
        lblIgTotalNomina = $("#lblIgTotalNomina");
        lblIgTotalMensual = $("#lblIgTotalMensual");
        lblTblIgNomina = $("#lblTblIgNomina");


        /******** Variables divInfoCambio ***********/
        selPuestoChang = $("#selPuestoChang");
        selTipoNominaChang = $("#selTipoNominaChang");
        selCentroCostoChang = $("#selCentroCostoChang");
        selDepartamento = $('#selDepartamento');
        selJefeInmediatoChang = $("#selJefeInmediatoChang");
        selRegistroPatronalChang = $("#selRegistroPatronalChang");

        lblTblNominaChang = $("#lblTblNominaChang");
        txtSueldoNominaChange = $("#txtSueldoNominaChange");
        txtComplementoNominaChange = $("#txtComplementoNominaChange");
        lblSueldoMensualChange = $("#lblSueldoMensualChange");
        lblComplementoMensualChange = $("#lblComplementoMensualChange");
        lblTotalNominaChange = $("#lblTotalNominaChange");
        lblTotalMesChange = $("#lblTotalMesChange");

        lblInicioNomina = $("#lblInicioNomina");
        txtNumeroInicio = $("#txtNumeroInicio");
        lblAPartirDe = $("#lblAPartirDe");
        txtFechaInicio = $("#txtFechaInicio");
        txtJustificacion = $("#txtJustificacion");


        /******** Variables Autorizacion ***********/
        btnAutorizaCck1 = $("#btnAutorizaCck1");
        btnAutorizaCck2 = $("#btnAutorizaCck2");
        btnAutorizaCck3 = $("#btnAutorizaCck3");


        selAutSolicitaEnvia = $("#selAutSolicitaEnvia");
        selAutSolicita = $("#selAutSolicita");
        selAutVoBo = $("#selAutVoBo");
        selAutVoBo2 = $("#selAutVoBo2");
        selAutoriza1 = $("#selAutoriza1");
        selAutoriza12 = $("#selAutoriza12");
        selAutoriza2 = $("#selAutoriza2");
        selAutoriza22 = $("#selAutoriza22");
        selAutoriza3 = $("#selAutoriza3");
        selAutoriza32 = $("#selAutoriza32");
        slAnio = $("#slAnio");

        /******** Variables Botones ***********/

        btnSolicitarAp = $("#btnSolicitarAp");
        const getPeriodoActual = new URL(window.location.origin + '/Administrativo/FormatoCambio/getPeriodoActual');
        const getPeriodo = new URL(window.location.origin + '/Administrativo/FormatoCambio/getPeriodo');

        const lblIgLineaNegocio = $('#lblIgLineaNegocio');
        const lblIgCategoria = $('#lblIgCategoria');
        const selLineaNegocio = $('#selLineaNegocio');
        const selCategoria = $('#selCategoria');

        /******************************Fin Declaracion de Variables******************************************/

        //#region VARS RANGO TABULADORES
        let lowerBase = 0;
        let lowerComplemento = 0;
        let upperBase = 0;
        let upperComplemento = 0;

        //#endregion

        //DIVS TABLAS
        const divTablaSalariosGenerales = $('#divTablaSalariosGenerales');
        const divTablaSalariosDatos = $('#divTablaSalariosDatos');

        //EMPRESA ACTUAL
        const empresaActual = $('#empresaActual');
        _empresaActual = empresaActual.val();

        function ini() {

            btnAutorizaCck1.prop('checked', true);
            btnAutorizaCck2.prop('checked', true);
            btnAutorizaCck3.prop('checked', true);
            var hoy = new Date();
            var dd = hoy.getDate();
            var mm = hoy.getMonth() + 1;
            var yyyy = hoy.getFullYear();

            txtFechaInicio.datepicker().datepicker("setDate", dd + "/" + mm + "/" + yyyy);

            txtSueldoNominaChange.change(selectTNomina);
            txtComplementoNominaChange.change(selectTNomina);
            txtBonoNominaChange.change(selectTNomina);

            txtIgNumeroEmpleado.change(selectEmpleado);

            selTipoNominaChang.change(selectTNomina);
            selCentroCostoChang.on('change', function () {
                funSelCC(0, true);
            });
            selDepartamento.change(funSelDepartamento);
            fillCombos();

            setAutoComplete();

            btnSolicitarAp.click(SolicitarAprobacion);


            if (FormatoId.val() > 0) {
                CargarInfoEdit(FormatoId.val());
            }


            selAutoriza1.change();
            slAnio.change(setPeriodo);
            txtNumeroInicio.change(setPeriodo);
            selTipoNominaChang.change(setPeriodoActual);
            $(groupAmbosAut).attr("disabled", true);//deshabiltaa check's

            selLineaNegocio.on("change", function () {
                if (selLineaNegocio.val() != "") {
                    selCategoria.empty();
                    selCategoria.fillComboBox('/Administrativo/Reclutamientos/GetCategoriasByLineaNegocio', { idLineaNegocio: $(this).val(), idPuesto: objEmpleadoCambios.PuestoID }, '-- Seleccionar --', () => {
                    });

                } else {
                    selCategoria.empty();
                }
            });

            selCategoria.on("change", function () {
                if ($(this).val() != null && objEmpleadoCambios.Clave_Empleado > 0) {
                    fnGetTabuladorByEmpleado();
                }
            });

        }

        function GetFecha() {

            if (selTipoNominaChang.val() == "1") {
                if (Number(txtNumeroInicio.val()) != 1 && Number(txtNumeroInicio.val()) <= 52) {
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var simple = new Date(y, 0, 1 + (w - 1) * 7);
                    var semana = new Date(simple.getYear() + 1900, simple.getMonth(), simple.getDate() - 6);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));

                }
                else if (Number(txtNumeroInicio.val()) == 1) {

                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear() - 1;
                    var w = Number(53);
                    var simple = new Date(y, 0, 1 + (w - 1) * 7);
                    var semana = new Date(simple.getYear() + 1900, simple.getMonth(), simple.getDate() - 2);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));

                }
            }
            else {
                if (txtNumeroInicio.val() == "1") {
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var semana = new Date(y, 0, 1);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                }
                else if (txtNumeroInicio.val() == "2") {

                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var semana = new Date(y, 0, 1 + 15);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                } else {

                    quincenas(Number(txtNumeroInicio.val()));
                }
            }
        }
        async function setPeriodoActual() {
            try {
                var _this = $(this);
                if (_this.val() != null && _this.val() != undefined && _this.val() != "") {
                    response = await ejectFetchJson(getPeriodoActual, {
                        tipoNomina: selTipoNominaChang.val()
                    });
                    if (response.success) {
                        txtNumeroInicio.val(response.periodo.periodo);
                        txtFechaInicio.val(response.periodo.inicio);
                    } else {
                        AlertaGeneral("Aviso", "No existe el periodo");
                    }
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message); }
        }
        async function setPeriodo() {
            try {
                var _this = $(this);
                if (_this.val() != null && _this.val() != undefined && _this.val() != "") {
                    response = await ejectFetchJson(getPeriodo, {
                        anio: slAnio.val()
                        , periodo: txtNumeroInicio.val()
                        , tipoNomina: selTipoNominaChang.val()
                    });
                    if (response.success) {
                        txtFechaInicio.val(response.periodo.inicio);
                    } else {
                        AlertaGeneral("Aviso", "No existe el periodo");
                    }
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message); }
        }
        ini();

        function quincenas(valor) {
            switch (valor) {
                case 1:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 0, 1);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 2:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 0, 1 + 15);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 3:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 1, 1);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 4:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 1, 1 + 15);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 5:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 2, 1);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 6:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 2, 1 + 15);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 7:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 3, 1);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 8:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 3, 1 + 15);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 9:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 4, 1);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 10:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 4, 1 + 15);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                case 11:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 5, 1);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 12:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 5, 1 + 15);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 13:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 6, 1);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 14:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 6, 1 + 15);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 15:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 7, 1);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 16:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 7, 1 + 15);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 17:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 8, 1);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 18:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 8, 1 + 15);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 19:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 9, 1);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 20:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 9, 1 + 15);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 21:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 10, 1);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 22:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 10, 1 + 15);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 23:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 11, 1);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                case 24:
                    var GetDateCurrent = new Date();
                    var y = GetDateCurrent.getFullYear();
                    var w = Number(txtNumeroInicio.val());
                    var semana = new Date(y, 11, 1 + 15);
                    txtFechaInicio.datepicker().datepicker("setDate", new Date(semana));
                    break;
                default:

            }


        }

        function Redirect() {
            window.location = "/Administrativo/FormatoCambio/";
        }

        function CargarInfoEdit(id) {
            selectEmpleadoEditar(id);
        }

        function SolicitarAprobacion() {

            objEmpleadoCambios.Salario_Base = txtSueldoNominaChange.val();
            objEmpleadoCambios.Complemento = txtComplementoNominaChange.val();
            objEmpleadoCambios.TipoNomina = $("#selTipoNominaChang option:selected").html();
            objEmpleadoCambios.TipoNominaID = selTipoNominaChang.val();
            objEmpleadoCambios.Bono = txtBonoNominaChange.val();
            objEmpleadoCambios.idCategoria = selCategoria.val();
            objEmpleadoCambios.descCategoria = $("#selCategoria option:selected").text();
            objEmpleadoCambios.idLineaNegocios = selLineaNegocio.val();
            objEmpleadoCambios.descLineaNegocios = $("#selLineaNegocio option:selected").text();
            objEmpleadoCambios.RegistroPatronal = $("#selRegistroPatronalChang option:selected").text();
            objEmpleadoCambios.RegistroPatronalID = selRegistroPatronalChang.val();

            if (selLineaNegocio.val() == "--Seleccione--" || selCategoria.val() == "--Seleccionar--" || selLineaNegocio.val() == "") {
                // objEmpleadoCambios.idLineaNegocios = 0;
                // objEmpleadoCambios.descLineaNegocios = "S/N";
                AlertaGeneral("Alerta", "Ubo un problema con la linea de negocio del puesto")
                selLineaNegocio.focus();
                return null;
            }

            if (selCategoria.val() == "--Seleccione--" || selCategoria.val() == "--Seleccionar--" || selCategoria.val() == "") {
                // objEmpleadoCambios.idCategoria = 0;
                // objEmpleadoCambios.descCategoria = "S/N";
                AlertaGeneral("Alerta", "Debe seleccionar a una categoria a modificar")
                selCategoria.focus();
                return null;
            }

            if (selRegistroPatronalChang.val() == "--Seleccione--" || selRegistroPatronalChang.val() == "--Seleccionar--" || selRegistroPatronalChang.val() == "") {
                // objEmpleadoCambios.idCategoria = 0;
                // objEmpleadoCambios.descCategoria = "S/N";
                AlertaGeneral("Alerta", "Debe seleccionar a un Registro patronal")
                selRegistroPatronalChang.focus();
                return null;
            }

            objEmpleadoCambios.InicioNomina = txtNumeroInicio.val();
            objEmpleadoCambios.FechaInicioCambio = txtFechaInicio.val();
            objEmpleadoCambios.Justificacion = txtJustificacion.val();

            objEmpleadoCambios.Folio = $("#hFolio").text();

            if (txtIgNumeroEmpleado.val() === "") {
                AlertaGeneral("Alerta", "Debe seleccionar a un empleado a modificar")
                txtIgNumeroEmpleado.focus();
            }
            else if (txtNumeroInicio.val() === "") {
                txtNumeroInicio.focus();
                AlertaGeneral("Alerta", "Debe inicar semana/quincena de inicio")
            }
            else if (selAutSolicitaEnvia.val() === "" || selAutSolicita.val() === "" || selAutVoBo.val() === "") {
                if (selAutSolicitaEnvia.val() === "") {
                    selAutSolicitaEnvia.focus();
                }
                else if (selAutSolicita.val() === "") {
                    selAutSolicita.focus();
                }
                else {
                    selAutVoBo.focus();
                }
                AlertaGeneral("Alerta", "Debe seleccionar un Gerente Envia, Gerente Recibe y VoBo para poder guardar")
            }
            else {
                btnSolicitarAp.prop("disabled", true);
                objEmpleadoCambios.CamposCambiados = " ";
                objEmpleadoCambios.CamposCambiados += objEmpleadoCambios.PuestoID != objEmpleadoORG.PuestoID ? "Puesto, " : "";
                objEmpleadoCambios.CamposCambiados += objEmpleadoCambios.CcID != objEmpleadoORG.CcID ? "CC, " : "";
                objEmpleadoCambios.CamposCambiados += objEmpleadoCambios.TipoNominaID != objEmpleadoORG.TipoNominaID ? "Tipo Nomina, " : "";
                objEmpleadoCambios.CamposCambiados += objEmpleadoCambios.Clave_Jefe_Inmediato != objEmpleadoORG.Clave_Jefe_Inmediato ? "Jefe Inmediato, " : "";
                objEmpleadoCambios.CamposCambiados += objEmpleadoCambios.RegistroPatronalID != objEmpleadoORG.RegistroPatronalID ? "Registro Patronal, " : "";
                objEmpleadoCambios.CamposCambiados += objEmpleadoCambios.Salario_Base != objEmpleadoORG.Salario_Base ? "Sueldo, " : "";
                objEmpleadoCambios.CamposCambiados += objEmpleadoCambios.Bono != objEmpleadoORG.Bono ? "Bono, " : "";
                objEmpleadoCambios.CamposCambiados += objEmpleadoCambios.idCategoria != "" && objEmpleadoCambios.idCategoria != objEmpleadoORG.idCategoria ? "Categoria, " : "";
                objEmpleadoCambios.CamposCambiados += objEmpleadoCambios.idLineaNegocios != "" && objEmpleadoCambios.idLineaNegocios != objEmpleadoORG.idLineaNegocios ? "Linea de Negocios, " : "";
                objEmpleadoCambios.CamposCambiados += objEmpleadoCambios.ClaveDepartamento == objEmpleadoORG.ClaveDepartamento && objEmpleadoCambios.CcID == objEmpleadoORG.CcID ? "" : "Departamento";

                objEmpleadoCambios.ClaveDepartamentoAnterior = objEmpleadoORG.ClaveDepartamento;
                objEmpleadoCambios.DepartamentoAnterior = objEmpleadoORG.Departamento;
                objEmpleadoCambios.descCategoriaAnterior = objEmpleadoORG.descCategoria;
                objEmpleadoCambios.descLineaNegociosAnterior = objEmpleadoORG.descLineaNegocios;
                objEmpleadoCambios.idCategoriaAnterior = objEmpleadoORG.idCategoria;
                objEmpleadoCambios.idLineaNegociosAnterior = objEmpleadoORG.idLineaNegocios;

                revisarAmbos();
                revisarExisteAutorizadores();

                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: '/Administrativo/FormatoCambio/SolicitarAprobacion',
                    data: { objEmpleadoCambio: objEmpleadoCambios, lstAutorizacion: argAutorizaciones },
                    success: function (response) {

                        if (response.success) {
                            var idFormatoCambios = response.idFormatoCambios;
                            var usuarioEnvia = response.usuarioEnvia;
                            var idReporte = "11";
                            var folioFormatoCambio = response.objFormatoCambioDTO.objFormatoCambio.folio;
                            var numeroEmpleado = response.objFormatoCambioDTO.objFormatoCambio.Clave_Empleado;
                            objEditar = response.objFormatoCambioDTO.objFormatoCambio;
                            var objAprovacion = response.objFormatoCambioDTO.objFormatoCambio.id;
                            var folio = response.objFormatoCambioDTO.objFormatoCambio.folio;
                            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&fId=" + idFormatoCambios + "&inMemory=1";

                            ireport.attr("src", path);
                            var tipo = FormatoId.val() == 0 ? "nuevo" : "cambio";
                            document.getElementById('report').onload = function () {
                                $.ajax({
                                    datatype: "json",
                                    type: "POST",
                                    url: '/Administrativo/FormatoCambio/enviarCorreos',
                                    data: { usuariorecibe: usuarioEnvia, formatoID: idFormatoCambios, tipo: tipo, orden: 0 },
                                    success: function (response) {
                                        $.unblockUI();
                                        if (response.success) {
                                            $("#hFolio").text(folioFormatoCambio);
                                            tipoMensaje = "";
                                            btnSolicitarAp.prop("disabled", false);
                                            if (FormatoId.val() > 0) {
                                                tipoMensaje = "Se ha modificado el folio ";
                                            } else {
                                                tipoMensaje = "Se ha guardado el folio ";
                                            }
                                            ConfirmacionGeneralFC("Confirmación", tipoMensaje + folio, "bg-green");
                                        }
                                    },
                                    error: function () {
                                        btnSolicitarAp.prop("disabled", false);
                                        $.unblockUI();
                                    }
                                });
                            };
                        } else {
                            btnSolicitarAp.prop("disabled", false);
                            AlertaGeneral(`Aviso`, `Ocurrió un error.`);
                        }

                    },
                    error: function (o_O) {
                        btnSolicitarAp.prop("disabled", false);
                        $.unblockUI();
                    }
                });
            }

        }

        function revisarAmbos() {

            for (var i = 0; i < argAutorizaciones.length; i++) {
                if (argAutorizaciones[i].Orden == 2 || argAutorizaciones[i].Orden == 3) {
                    if (!btnAutorizaCck1.is(':checked')) {
                        argAutorizaciones[i].Orden = 2
                        argAutorizaciones[i].tipoAutoriza = true;
                    }
                }
                if (argAutorizaciones[i].Orden == 4 || argAutorizaciones[i].Orden == 5) {
                    if (!btnAutorizaCck2.is(':checked')) {
                        argAutorizaciones[i].Orden = 4
                        argAutorizaciones[i].tipoAutoriza = true;
                    }
                }
                if (argAutorizaciones[i].Orden == 6 || argAutorizaciones[i].Orden == 7) {
                    if (!btnAutorizaCck3.is(':checked')) {
                        argAutorizaciones[i].Orden = 6
                        argAutorizaciones[i].tipoAutoriza = true;
                    }
                }
            }

        }


        function ConfirmacionGeneralFC(titulo, mensaje, color) {
            if (!$("#dialogalertaGeneral").is(':visible')) {
                var html = '<div id="dialogalertaGeneral" class="modal fade modalAlerta" role="dialog">' +
                    '<div class="modal-dialog">' +
                    '<div class="modal-content">' +
                    '<div class="modal-header text-center modal-md">' +
                    '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">' +
                    '&times;</button>' +
                    '<h4  class="modal-title">' + titulo + '</h4>' +
                    '</div>' +
                    '<div class="modal-body">' +
                    '<div class="container">' +
                    '<div class="row">' +
                    '<div class="col-lg-12">' +
                    '<h3> <span class="glyphicon glyphicon-ok-circle ' + color + '" aria-hidden="true" style="font-size:40px;"></span> <label style="position: fixed; margin-left:35px">' + mensaje + '</label></h3>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '<div class="modal-footer">' +
                    '<a id="btndialogalertaGeneral" href="/Administrativo/FormatoCambio/Index" class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-ok"></span> Aceptar</a>' +
                    '</div>' +
                    '</div>' +
                    '</div></div>';

                var _this = $(html);
                _this.modal("show");
            }

        }

        function selectTNomina() {

            var sueldo = txtSueldoNominaChange.val();
            var complemento = txtComplementoNominaChange.val();
            var Bono = txtBonoNominaChange.val();


            if (selTipoNominaChang.val() == 1) {

                lblTblNominaChang.text($("#selTipoNominaChang option:selected").html());

                var sueldoMensual = (sueldo / 7) * 30.4;
                var complementoMensual = (complemento / 7) * 30.4;
                var BonoMensual = (Bono / 7) * 30.4;

                if (_empresaActual == 6) {
                    lblSueldoMensualChange.text('S/ ' + (sueldoMensual.toFixed(2)));
                    lblComplementoMensualChange.text('S/ ' + (complementoMensual.toFixed(2)));
                    lblBonoMensualChange.text('S/ ' + BonoMensual.toFixed(2));

                } else {
                    lblSueldoMensualChange.text('$ ' + (sueldoMensual.toFixed(2)));
                    lblComplementoMensualChange.text('$ ' + (complementoMensual.toFixed(2)));
                    lblBonoMensualChange.text('$ ' + BonoMensual.toFixed(2));

                }

                //RANGOS
                let totalNominal = Number((parseFloat(sueldo) + parseFloat(complemento) + parseFloat(Bono)).toFixed(2));
                let inputBase = Number(sueldo);
                let inputComplemento = Number(complemento);

                console.log(lowerBase);
                console.log(lowerComplemento);
                console.log(upperBase);
                console.log(upperComplemento);

                if (lowerBase > 0 || lowerComplemento > 0 || upperBase > 0 || upperComplemento > 0) {

                    //BASE
                    if (lowerBase <= inputBase && upperBase >= inputBase) {
                        btnSolicitarAp.prop('disabled', false);
                    } else {
                        btnSolicitarAp.prop('disabled', true);
                        Alert2Warning("El sueldo BASE que ingreso es mayor o menor al rango permitido (" + lowerBase + " - " + upperBase + ").");
                    }

                    //COMPLEMENTO
                    if (lowerComplemento <= inputComplemento && upperComplemento >= inputComplemento) {
                        btnSolicitarAp.prop('disabled', false);
                    } else {
                        btnSolicitarAp.prop('disabled', true);
                        Alert2Warning("El sueldo COMPLEMENTO que ingreso es mayor o menor al rango permitido (" + lowerComplemento + " - " + upperComplemento + ").");
                    }
                }

                if (_empresaActual == 6) {
                    lblTotalNominaChange.text('S/ ' + totalNominal);
                    lblTotalMesChange.text('S/ ' + ((parseFloat(sueldoMensual) + parseFloat(complementoMensual) + parseFloat(BonoMensual)).toFixed(2)));
                } else {
                    lblTotalNominaChange.text('$ ' + totalNominal);
                    lblTotalMesChange.text('$ ' + ((parseFloat(sueldoMensual) + parseFloat(complementoMensual) + parseFloat(BonoMensual)).toFixed(2)));
                }


                lblInicioNomina.text("Inicia Semana:");
                lblAPartirDe.text("A partir del miércoles:");


            }
            else if (selTipoNominaChang.val() == 4) {

                lblTblNominaChang.text($("#selTipoNominaChang option:selected").html());

                var sueldoMensual = sueldo * 2;
                var complementoMensual = complemento * 2;
                var BonoMensual = Bono * 2;

                if (_empresaActual == 6) {
                    lblSueldoMensualChange.text('S/ ' + (sueldoMensual.toFixed(2)));
                    lblComplementoMensualChange.text('S/ ' + (complementoMensual.toFixed(2)));
                    lblBonoMensualChange.text('S/ ' + BonoMensual.toFixed(2));
                } else {
                    lblSueldoMensualChange.text('$ ' + (sueldoMensual.toFixed(2)));
                    lblComplementoMensualChange.text('$ ' + (complementoMensual.toFixed(2)));
                    lblBonoMensualChange.text('$ ' + BonoMensual.toFixed(2));
                }


                //RANGOS
                let totalNominal = Number((parseFloat(sueldo) + parseFloat(complemento) + parseFloat(Bono)).toFixed(2));
                let inputBase = Number(sueldo);
                let inputComplemento = Number(complemento);

                console.log(lowerBase);
                console.log(lowerComplemento);
                console.log(upperBase);
                console.log(upperComplemento);

                if (lowerBase > 0 || lowerComplemento > 0 || upperBase > 0 || upperComplemento > 0) {

                    //BASE
                    if (lowerBase <= inputBase && upperBase >= inputBase) {
                        btnSolicitarAp.prop('disabled', false);
                    } else {
                        btnSolicitarAp.prop('disabled', true);
                        Alert2Warning("El sueldo BASE que ingreso es mayor o menor al rango permitido (" + lowerBase + " - " + upperBase + ").");
                    }

                    //COMPLEMENTO
                    if (lowerComplemento <= inputComplemento && upperComplemento >= inputComplemento) {
                        btnSolicitarAp.prop('disabled', false);
                    } else {
                        btnSolicitarAp.prop('disabled', true);
                        Alert2Warning("El sueldo COMPLEMENTO que ingreso es mayor o menor al rango permitido (" + lowerComplemento + " - " + upperComplemento + ").");
                    }
                }

                if (_empresaActual == 6) {
                    lblTotalNominaChange.text('S/ ' + (totalNominal));
                    lblTotalMesChange.text('S/ ' + ((parseFloat(sueldoMensual) + parseFloat(complementoMensual) + +parseFloat(BonoMensual)).toFixed(2)));
                } else {
                    lblTotalNominaChange.text('$ ' + (totalNominal));
                    lblTotalMesChange.text('$ ' + ((parseFloat(sueldoMensual) + parseFloat(complementoMensual) + +parseFloat(BonoMensual)).toFixed(2)));
                }


                lblInicioNomina.text("Inicia Quincena:");
                lblAPartirDe.text("A partir de:");
            }
        }

        //Extrae la informacion del empleado y llena los campos requeridos
        function selectEmpleadoEditar(id) {
            var objEditar;
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/Administrativo/FormatoCambio/getEmpleadoEditar',
                data: { id: id },
                async: false,
                success: function (response) {
                    if (response.success) {

                        objEmpleadoORG = response.items[0];

                        $("#hFolio").text(response.items[0].Folio);
                        txtIgNumeroEmpleado.val(response.items[0].Clave_Empleado);
                        objEditar = response.items[0];
                        txtIgNumeroEmpleado.change();
                        selectAprobaciones(response.items[0].id);
                        viewDatosEmpleadoEditar(objEditar);
                        selectAprobaciones(response.items[0].id);
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });



        }
        function selectAprobaciones(id) {

            argAutorizaciones = [];
            selAutSolicitaEnvia.val("");
            selAutSolicita.val("");
            selAutVoBo.val("");
            selAutVoBo2.val("");
            selAutoriza1.val("");
            selAutoriza12.val("");
            selAutoriza2.val("");
            selAutoriza22.val("");
            selAutoriza3.val("");
            selAutoriza32.val("");

            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: '/Administrativo/FormatoCambio/GetAutorizacion',
                data: { id: id },
                async: false,
                success: function (response) {

                    var i = 0;
                    for (i; i <= response.items.length - 1; i++) {
                        if (response.items[0].Estatus == true) {
                            selAutSolicitaEnvia.attr("disabled", true);
                            selAutSolicita.attr("disabled", true);
                            selAutVoBo.attr("disabled", true);
                            selAutVoBo2.attr("disabled", true);
                            selAutoriza1.attr("disabled", true);
                            selAutoriza12.attr("disabled", true);
                            selAutoriza2.attr("disabled", true);
                            selAutoriza22.attr("disabled", true);
                            selAutoriza3.attr("disabled", true);
                            selAutoriza32.attr("disabled", true);
                        }


                        switch (response.items[i].Responsable) {
                            case 'Gerente Actual':
                                selAutSolicitaEnvia.val(response.items[i].Nombre_Aprobador);
                                objAutorizacion = {
                                    id: response.items[i].id,
                                    Clave_Aprobador: response.items[i].Clave_Aprobador,
                                    Nombre_Aprobador: response.items[i].Nombre_Aprobador,
                                    PuestoAprobador: response.items[i].PuestoAprobador,
                                    Responsable: response.items[i].Responsable,
                                    Autorizando: response.items[i].Autorizando,
                                    Orden: response.items[i].Orden

                                }
                                argAutorizaciones.push(objAutorizacion);
                                break;

                            case 'Gerente Recibe':
                                selAutSolicita.val(response.items[i].Nombre_Aprobador);
                                objAutorizacion = {
                                    id: response.items[i].id,
                                    Clave_Aprobador: response.items[i].Clave_Aprobador,
                                    Nombre_Aprobador: response.items[i].Nombre_Aprobador,
                                    PuestoAprobador: response.items[i].PuestoAprobador,
                                    Responsable: response.items[i].Responsable,
                                    Autorizando: response.items[i].Autorizando,
                                    Orden: response.items[i].Orden

                                }
                                argAutorizaciones.push(objAutorizacion);
                                break;

                            case 'VoBo':
                                if (selAutVoBo.val().length == 0) {
                                    selAutVoBo.val(response.items[i].Nombre_Aprobador);
                                }
                                else { selAutVoBo2.val(response.items[i].Nombre_Aprobador); }
                                objAutorizacion = {
                                    id: response.items[i].id,
                                    Clave_Aprobador: response.items[i].Clave_Aprobador,
                                    Nombre_Aprobador: response.items[i].Nombre_Aprobador,
                                    PuestoAprobador: response.items[i].PuestoAprobador,
                                    Responsable: response.items[i].Responsable,
                                    Autorizando: response.items[i].Autorizando,
                                    Orden: response.items[i].Orden
                                }
                                argAutorizaciones.push(objAutorizacion);
                                break;
                            case 'Autorización 1':
                                if (selAutoriza1.val().length == 0) {
                                    selAutoriza1.val(response.items[i].Nombre_Aprobador);
                                }
                                else { selAutoriza12.val(response.items[i].Nombre_Aprobador); }
                                objAutorizacion = {
                                    id: response.items[i].id,
                                    Clave_Aprobador: response.items[i].Clave_Aprobador,
                                    Nombre_Aprobador: response.items[i].Nombre_Aprobador,
                                    PuestoAprobador: response.items[i].PuestoAprobador,
                                    Responsable: response.items[i].Responsable,
                                    Autorizando: response.items[i].Autorizando,
                                    Orden: response.items[i].Orden
                                }
                                argAutorizaciones.push(objAutorizacion);
                                break;
                            case 'Autorización 2':
                                if (selAutoriza2.val().length == 0) {
                                    selAutoriza2.val(response.items[i].Nombre_Aprobador);
                                }
                                else { selAutoriza22.val(response.items[i].Nombre_Aprobador); }

                                objAutorizacion = {
                                    id: response.items[i].id,
                                    Clave_Aprobador: response.items[i].Clave_Aprobador,
                                    Nombre_Aprobador: response.items[i].Nombre_Aprobador,
                                    PuestoAprobador: response.items[i].PuestoAprobador,
                                    Responsable: response.items[i].Responsable,
                                    Autorizando: response.items[i].Autorizando,
                                    Orden: response.items[i].Orden
                                }
                                argAutorizaciones.push(objAutorizacion);
                                break;
                            case 'Autorización 3':
                                if (selAutoriza3.val().length == 0) {
                                    selAutoriza3.val(response.items[i].Nombre_Aprobador);
                                }
                                else { selAutoriza32.val(response.items[i].Nombre_Aprobador); }
                                objAutorizacion = {
                                    id: response.items[i].id,
                                    Clave_Aprobador: response.items[i].Clave_Aprobador,
                                    Nombre_Aprobador: response.items[i].Nombre_Aprobador,
                                    PuestoAprobador: response.items[i].PuestoAprobador,
                                    Responsable: response.items[i].Responsable,
                                    Autorizando: response.items[i].Autorizando,
                                    Orden: response.items[i].Orden
                                }
                                argAutorizaciones.push(objAutorizacion);
                                break;

                        }
                    }
                },
                error: function (response) {
                    //$.unblockUI();
                    // alert(response.message);
                }
            });
        }

        function selectEmpleado() {
            var editar = (FormatoId.val() > 0 ? true : false);
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/Administrativo/FormatoCambio/getEmpleadoExclusion',
                data: { empleadoCVE: txtIgNumeroEmpleado.val() },
                async: false,
                success: function (response) {
                    if (response.success) {
                        $.ajax({
                            datatype: "json",
                            type: "POST",
                            url: '/Administrativo/FormatoCambio/getEmpleado',
                            data: { id: txtIgNumeroEmpleado.val(), editar: editar },
                            async: false,
                            success: function (response) {
                                if (response.success) {
                                    var Pendientes = response.Pendientes;
                                    if (!editar) {
                                        if (Pendientes) {
                                            btnSolicitarAp.hide();
                                            AlertaGeneral('Alerta', '¡Este empleado ya tiene un formato de cambio pendiente, por tal motivo no se puede generar otro hasta finalizar el cambio pendiente!')
                                        }
                                        else {
                                            btnSolicitarAp.show();
                                        }

                                    }
                                    else {
                                        btnSolicitarAp.show();
                                    }

                                    if (response.items.CC == null) {
                                        Alert2Warning("Ocurrio algo mal con el empleado ingresado, favor de contactarse con el departamento de sistemas");
                                        return "";
                                    } else {
                                        viewDatosEmpleados(response.items)
                                        objEmpleadoORG = response.items;
                                    }
                                }
                                else {
                                    AlertaGeneral('Alerta', 'No se encontró el número de usuario')
                                }
                            },
                            error: function () {
                                $.unblockUI();
                            }
                        });
                    }
                    else {
                        AlertaGeneral('Alerta', 'No tiene permiso para consultar a este empleado')
                    }
                },
                error: function () {
                    AlertaGeneral('Alerta', 'No se encontró el número de usuario')
                    $.unblockUI();
                }
            });


        }

        var formatNumber = {
            separador: ",", // separador para los miles
            sepDecimal: '.', // separador para los decimales
            formatear: function (num) {
                num += '';
                var splitStr = num.split('.');
                var splitLeft = splitStr[0];
                var splitRight = splitStr.length > 1 ? this.sepDecimal + splitStr[1] : '';
                var regx = /(\d+)(\d{3})/;
                while (regx.test(splitLeft)) {
                    splitLeft = splitLeft.replace(regx, '$1' + this.separador + '$2');
                }
                return this.simbol + splitLeft + splitRight;
            },
            new: function (num, simbol) {
                this.simbol = simbol || '';
                return this.formatear(num);
            }
        }

        function viewDatosEmpleados(objEmpleado) {

            objEmpleadoCambios = {
                Clave_Empleado: objEmpleado.Clave_Empleado,
                Nombre: objEmpleado.Nombre,
                Ape_Paterno: objEmpleado.Ape_Paterno,
                Ape_Materno: objEmpleado.Ape_Materno,
                Fecha_Alta: objEmpleado.Fecha_Alta,
                PuestoID: objEmpleado.PuestoID,
                Puesto: objEmpleado.Puesto,
                TipoNominaID: objEmpleado.TipoNominaID,
                TipoNomina: objEmpleado.TipoNomina,
                CcID: objEmpleado.CcID,
                CC: objEmpleado.CC,
                RegistroPatronalID: objEmpleado.RegistroPatronalID,
                RegistroPatronal: objEmpleado.RegistroPatronal,
                Clave_Jefe_Inmediato: objEmpleado.Clave_Jefe_Inmediato,
                Nombre_Jefe_Inmediato: objEmpleado.Nombre_Jefe_Inmediato,
                Salario_Base: objEmpleado.Salario_Base,
                Complemento: objEmpleado.Complemento,
                Bono: objEmpleado.Bono,
                idCategoria: objEmpleado.idCategoria,
                descCategoria: objEmpleado.descCategoria,
                idLineaNegocios: objEmpleado.idLineaNegocios,
                descLineaNegocios: objEmpleado.descLineaNegocios,
                Departamento: objEmpleado.Departamento,
                ClaveDepartamento: objEmpleado.ClaveDepartamento,
                DepartamentoAnterior: objEmpleado.DepartamentoAnterior,
                ClaveDepartamentoAnterior: objEmpleado.ClaveDepartamentoAnterior,
                idCategoriaAnterior: objEmpleado.idCategoriaAnterior,
                descCategoriaAnterior: objEmpleado.descCategoriaAnterior,
                idLineaNegociosAnterior: objEmpleado.idLineaNegociosAnterior,
                descLineaNegociosAnterior: objEmpleado.descLineaNegociosAnterior,
            };

            //RANGOS

            if (objEmpleado.idTabuladorDet != null && objEmpleado.esRango) {
                upperBase = objEmpleado.Salario_Base;
                upperComplemento = objEmpleado.Complemento;
                lowerBase = Number(objEmpleado.lowerBase) > 0 ? (Number(objEmpleado.lowerBase) + 1) : 0;
                lowerComplemento = Number(objEmpleado.lowerComplemento) > 0 ? (Number(objEmpleado.lowerComplemento) + 1) : 0;

            } else {
                upperBase = 0;
                upperComplemento = 0;
                lowerBase = 0;
                lowerComplemento = 0;

            }

            var ingresoEmpleado = new Date(objEmpleado.Fecha_Alta.match(/\d+/)[0] * 1);

            lblIgNombre.val(objEmpleado.Nombre + " " + objEmpleado.Ape_Paterno + " " + objEmpleado.Ape_Materno);


            lblIgIngreso.text(ingresoEmpleado.toLocaleDateString());
            lblIgPuesto.text(objEmpleado.Puesto);
            lblIgNomina.text(objEmpleado.TipoNomina);
            lblIgCentro.text(objEmpleado.CC);
            lblIgJefe.text(objEmpleado.Nombre_Jefe_Inmediato);
            lblIgRegistro.text(objEmpleado.RegistroPatronal);
            lblIgDepartamento.text(objEmpleado.Departamento);
            lblTblIgNomina.text(objEmpleado.TipoNomina);
            lblIgLineaNegocio.text(objEmpleado.descLineaNegocios);
            lblIgCategoria.text(objEmpleado.descCategoria);
            // selLineaNegocio.trigger("change");
            // selCategoria.trigger("change");

            // Calculo de sueldo tabla informativa
            if (_empresaActual == 6) {
                lblIgSueldoNomina.text('S/ ' + objEmpleado.Salario_Base);
                lblIgComplementoNomina.text('S/ ' + (objEmpleado.Complemento));
                lblIgBonoNomina.text('S/ ' + objEmpleado.Bono);
            } else {
                lblIgSueldoNomina.text('$ ' + objEmpleado.Salario_Base);
                lblIgComplementoNomina.text('$ ' + (objEmpleado.Complemento));
                lblIgBonoNomina.text('$ ' + objEmpleado.Bono);
            }


            if (objEmpleado.TipoNominaID == 1) {
                var bonoMensual = (objEmpleado.Bono / 7) * 30.4;
                var sueldoMensual = (objEmpleado.Salario_Base / 7) * 30.4;
                var complementoMensual = (objEmpleado.Complemento / 7) * 30.4;

                if (_empresaActual == 6) {
                    lblIgBonoMensual.text('S/ ' + bonoMensual.toFixed(2));
                    lblIgSueldoMensual.text('S/ ' + (sueldoMensual.toFixed(2)));
                    lblIgComplementoMensual.text('S/ ' + ((complementoMensual.toFixed(2))));
                    lblIgTotalNomina.text('S/ ' + ((objEmpleado.Salario_Base + objEmpleado.Complemento + objEmpleado.Bono).toFixed(2)));
                    lblIgTotalMensual.text('S/ ' + ((sueldoMensual + complementoMensual + bonoMensual).toFixed(2)));
                } else {
                    lblIgBonoMensual.text('$ ' + bonoMensual.toFixed(2));
                    lblIgSueldoMensual.text('$ ' + (sueldoMensual.toFixed(2)));
                    lblIgComplementoMensual.text('$ ' + ((complementoMensual.toFixed(2))));
                    lblIgTotalNomina.text('$ ' + ((objEmpleado.Salario_Base + objEmpleado.Complemento + objEmpleado.Bono).toFixed(2)));
                    lblIgTotalMensual.text('$ ' + ((sueldoMensual + complementoMensual + bonoMensual).toFixed(2)));
                }


            }
            else if (objEmpleado.TipoNominaID == 4) {

                var sueldoMensual = objEmpleado.Salario_Base * 2;
                var complementoMensual = objEmpleado.Complemento * 2;
                var bonoMensual = objEmpleado.Bono * 2;

                if (_empresaActual == 6) {
                    lblIgSueldoMensual.text('S/ ' + (sueldoMensual.toFixed(2)));
                    lblIgComplementoMensual.text('S/ ' + (complementoMensual.toFixed(2)));
                    lblIgBonoMensual.text('S/ ' + bonoMensual.toFixed(2));
                    lblIgTotalNomina.text('S/ ' + ((objEmpleado.Salario_Base + objEmpleado.Complemento + objEmpleado.Bono).toFixed(2)));
                    lblIgTotalMensual.text('S/ ' + ((sueldoMensual + complementoMensual + bonoMensual).toFixed(2)));
                } else {
                    lblIgSueldoMensual.text('$ ' + (sueldoMensual.toFixed(2)));
                    lblIgComplementoMensual.text('$ ' + (complementoMensual.toFixed(2)));
                    lblIgBonoMensual.text('$ ' + bonoMensual.toFixed(2));
                    lblIgTotalNomina.text('$ ' + ((objEmpleado.Salario_Base + objEmpleado.Complemento + objEmpleado.Bono).toFixed(2)));
                    lblIgTotalMensual.text('$ ' + ((sueldoMensual + complementoMensual + bonoMensual).toFixed(2)));
                }
            }

            //LLena campos a editar
            selPuestoChang.val(objEmpleado.Puesto);
            selTipoNominaChang.val(objEmpleado.TipoNominaID);
            selCentroCostoChang.val(objEmpleado.CcID);
            funSelCC(objEmpleado.ClaveDepartamento, false);

            selCategoria.empty();

            selLineaNegocio.fillCombo('/Administrativo/Reclutamientos/GetLineaNegocioByCC', { cc: objEmpleado.CcID }, null, null, () => {
                selLineaNegocio.find('option:selected').remove();

                if (objEmpleado.idLineaNegocios != null && objEmpleado.idLineaNegocios > 0) {
                    selCategoria.fillComboBox('/Administrativo/Reclutamientos/GetCategoriasByLineaNegocio', { idLineaNegocio: objEmpleado.idLineaNegocios, idPuesto: objEmpleado.PuestoID }, '-- Seleccionar --', () => {
                        selCategoria.val(objEmpleado.idCategoria);
                    });
                } else {
                    if (selLineaNegocio.val() != null) {
                        selCategoria.fillComboBox('/Administrativo/Reclutamientos/GetCategoriasByLineaNegocio', { idLineaNegocio: selLineaNegocio.val(), idPuesto: objEmpleado.PuestoID }, '-- Seleccionar --', () => {
                            selCategoria.val(objEmpleado.idCategoria);
                        });
                    }
                }
            });

            selJefeInmediatoChang.val(objEmpleado.Nombre_Jefe_Inmediato);
            selRegistroPatronalChang.val(objEmpleado.RegistroPatronalID);
            lblTblNominaChang.text(objEmpleado.TipoNomina);

            txtSueldoNominaChange.val(objEmpleado.Salario_Base);
            txtComplementoNominaChange.val(objEmpleado.Complemento);
            txtBonoNominaChange.val(objEmpleado.Bono);

            if (objEmpleado.TipoNominaID == 1) {

                var sueldoMensual = (objEmpleado.Salario_Base / 7) * 30.4;
                var complementoMensual = (objEmpleado.Complemento / 7) * 30.4;
                var BonoMensual = (objEmpleado.Bono / 7) * 30.4;

                if (_empresaActual == 6) {
                    lblBonoMensualChange.text('S/ ' + BonoMensual.toFixed(2));
                    lblSueldoMensualChange.text('S/ ' + (sueldoMensual.toFixed(2)));
                    lblComplementoMensualChange.text('S/ ' + (complementoMensual.toFixed(2)));
                    lblTotalNominaChange.text('S/ ' + ((objEmpleado.Salario_Base + objEmpleado.Complemento + objEmpleado.Bono).toFixed(2)));
                    lblTotalMesChange.text('S/ ' + ((sueldoMensual + complementoMensual + BonoMensual).toFixed(2)));
                } else {
                    lblBonoMensualChange.text('$ ' + BonoMensual.toFixed(2));
                    lblSueldoMensualChange.text('$ ' + (sueldoMensual.toFixed(2)));
                    lblComplementoMensualChange.text('$ ' + (complementoMensual.toFixed(2)));
                    lblTotalNominaChange.text('$ ' + ((objEmpleado.Salario_Base + objEmpleado.Complemento + objEmpleado.Bono).toFixed(2)));
                    lblTotalMesChange.text('$ ' + ((sueldoMensual + complementoMensual + BonoMensual).toFixed(2)));
                }

                lblInicioNomina.text("Inicia Semana:");
                lblAPartirDe.text("A partir del miércoles:");
            }
            else if (objEmpleado.TipoNominaID == 4) {

                var sueldoMensual = objEmpleado.Salario_Base * 2;
                var complementoMensual = objEmpleado.Complemento * 2;
                var BonoMensual = objEmpleado.Bono * 2;

                if (_empresaActual == 6) {
                    lblSueldoMensualChange.text('S/ ' + (sueldoMensual.toFixed(2)));
                    lblComplementoMensualChange.text('S/ ' + (complementoMensual.toFixed(2)));
                    lblTotalNominaChange.text('S/ ' + ((objEmpleado.Salario_Base + objEmpleado.Complemento + BonoMensual).toFixed(2)));
                    lblBonoMensualChange.text('S/ ' + BonoMensual);
                    lblTotalMesChange.text('S/ ' + ((sueldoMensual + complementoMensual + BonoMensual).toFixed(2)));
                } else {
                    lblSueldoMensualChange.text('$ ' + (sueldoMensual.toFixed(2)));
                    lblComplementoMensualChange.text('$ ' + (complementoMensual.toFixed(2)));
                    lblTotalNominaChange.text('$ ' + ((objEmpleado.Salario_Base + objEmpleado.Complemento + BonoMensual).toFixed(2)));
                    lblBonoMensualChange.text('$ ' + BonoMensual);
                    lblTotalMesChange.text('$ ' + ((sueldoMensual + complementoMensual + BonoMensual).toFixed(2)));
                }


                lblInicioNomina.text("Inicia Quincena:");
                lblAPartirDe.text("A partir de:");
            }
            selectTNomina();
        }

        function viewDatosEmpleadoEditar(objFormato) {

            objEmpleadoCambios = {
                id: objFormato.id,
                Clave_Empleado: objFormato.Clave_Empleado,
                Nombre: objFormato.Nombre,
                Ape_Paterno: objFormato.Ape_Paterno,
                Ape_Materno: objFormato.Ape_Materno,
                Fecha_Alta: objFormato.Fecha_Alta,
                PuestoID: objFormato.PuestoID,
                Puesto: objFormato.Puesto,
                TipoNominaID: objFormato.TipoNominaID,
                TipoNomina: objFormato.TipoNomina,
                CcID: objFormato.CcID,
                CC: objFormato.CC,
                RegistroPatronalID: objFormato.RegistroPatronalID,
                RegistroPatronal: objFormato.RegistroPatronal,
                Clave_Jefe_Inmediato: objFormato.Clave_Jefe_Inmediato,
                Nombre_Jefe_Inmediato: objFormato.Nombre_Jefe_Inmediato,
                Salario_Base: objFormato.Salario_Base,
                Complemento: objFormato.Complemento,
                Bono: objFormato.Bono,
                InicioNomina: objFormato.InicioNomina,
                FechaInicioCambio: objFormato.FechaInicioCambio,
                Justificacion: objFormato.Justificacion,
                Departamento: objFormato.Departamento,
                idCategoria: objFormato.idCategoria,
                descCategoria: objFormato.descCategoria,
                idLineaNegocios: objFormato.idLineaNegocios,
                descLineaNegocios: objFormato.descLineaNegocios,
                ClaveDepartamento: objFormato.ClaveDepartamento,
                DepartamentoAnterior: objFormato.DepartamentoAnterior,
                ClaveDepartamentoAnterior: objFormato.ClaveDepartamentoAnterior,
                idCategoriaAnterior: objFormato.idCategoriaAnterior,
                descCategoriaAnterior: objFormato.descCategoriaAnterior,
                idLineaNegociosAnterior: objFormato.idLineaNegociosAnterior,
                descLineaNegociosAnterior: objFormato.descLineaNegociosAnterior,
            };

            var editadoEmpleado = new Date(objFormato.FechaInicioCambio.match(/\d+/)[0] * 1);

            selPuestoChang.val(objFormato.Puesto);
            selTipoNominaChang.val(objFormato.TipoNominaID);
            selCentroCostoChang.val(objFormato.CcID);
            selJefeInmediatoChang.val(objFormato.Nombre_Jefe_Inmediato);
            selRegistroPatronalChang.val(objFormato.RegistroPatronalID);
            lblTblNominaChang.text(objFormato.TipoNomina);
            lblIgLineaNegocio.text(objFormato.descLineaNegocios);
            lblIgCategoria.text(objFormato.descCategoria);

            txtSueldoNominaChange.val((objFormato.Salario_Base));
            txtComplementoNominaChange.val((objFormato.Complemento));
            txtBonoNominaChange.val((objFormato.Bono));

            if (objFormato.TipoNominaID == 1) {

                var sueldoMensual = (objFormato.Salario_Base / 7) * 30.4;
                var complementoMensual = (objFormato.Complemento / 7) * 30.4;

                if (_empresaActual == 6) {
                    lblSueldoMensualChange.text('S/ ' + (sueldoMensual.toFixed(2)));
                    lblComplementoMensualChange.text('S/ ' + (complementoMensual.toFixed(2)));
                    lblTotalNominaChange.text('S/ ' + ((objFormato.Salario_Base + objFormato.Complemento).toFixed(2)));
                    lblTotalMesChange.text('S/ ' + ((sueldoMensual + complementoMensual).toFixed(2)));
                } else {
                    lblSueldoMensualChange.text('$ ' + (sueldoMensual.toFixed(2)));
                    lblComplementoMensualChange.text('$ ' + (complementoMensual.toFixed(2)));
                    lblTotalNominaChange.text('$ ' + ((objFormato.Salario_Base + objFormato.Complemento).toFixed(2)));
                    lblTotalMesChange.text('$ ' + ((sueldoMensual + complementoMensual).toFixed(2)));
                }


                lblInicioNomina.text("Inicia Semana:");
                lblAPartirDe.text("A partir del miércoles:");

            }
            else if (objFormato.TipoNominaID == 4) {

                var sueldoMensual = objFormato.Salario_Base * 2;
                var complementoMensual = objFormato.Complemento * 2;

                if (_empresaActual == 6) {
                    lblSueldoMensualChange.text('S/ ' + (sueldoMensual.toFixed(2)));
                    lblComplementoMensualChange.text('S/ ' + (complementoMensual.toFixed(2)));
                    lblTotalNominaChange.text('S/ ' + ((objFormato.Salario_Base + objFormato.Complemento).toFixed(2)));
                    lblTotalMesChange.text('S/ ' + ((sueldoMensual + complementoMensual).toFixed(2)));
                } else {
                    lblSueldoMensualChange.text('$ ' + (sueldoMensual.toFixed(2)));
                    lblComplementoMensualChange.text('$ ' + (complementoMensual.toFixed(2)));
                    lblTotalNominaChange.text('$ ' + ((objFormato.Salario_Base + objFormato.Complemento).toFixed(2)));
                    lblTotalMesChange.text('$ ' + ((sueldoMensual + complementoMensual).toFixed(2)));
                }

                lblInicioNomina.text("Inicia Quincena:");
                lblAPartirDe.text("A partir de:");
            }

            txtNumeroInicio.val(objFormato.InicioNomina);
            txtFechaInicio.val(editadoEmpleado.toLocaleDateString());
            txtJustificacion.val(objFormato.Justificacion);
            txtBonoNominaChange.change();

            selLineaNegocio.fillCombo('/Administrativo/Reclutamientos/GetLineaNegocioByCC', { cc: objFormato.CcID }, null, null, () => {
                selLineaNegocio.find('option:selected').remove();

                if (objFormato.idLineaNegocios != null && objFormato.idLineaNegocios > 0) {
                    selCategoria.fillComboBox('/Administrativo/Reclutamientos/GetCategoriasByLineaNegocio', { idLineaNegocio: objFormato.idLineaNegocios, idPuesto: objFormato.PuestoID }, '-- Seleccionar --', () => {
                        selCategoria.val(objFormato.idCategoria);
                    });
                } else {
                    selCategoria.fillComboBox('/Administrativo/Reclutamientos/GetCategoriasByLineaNegocio', { idLineaNegocio: selLineaNegocio.val(), idPuesto: objFormato.PuestoID }, '-- Seleccionar --', () => {
                        selCategoria.val(objFormato.idCategoria);
                    });
                }
            });
        }

        function fillCombos() {
            selTipoNominaChang.fillCombo('/Administrativo/FormatoCambio/getTipoNomina', null);
            selCentroCostoChang.fillCombo('/Administrativo/FormatoCambio/getCboCC', null);
        }


        function setAutoComplete() {
            selPuestoChang.getAutocompleteValid(funSelPuesto, funSelPuestoNull, { id: 1 }, '/Administrativo/FormatoCambio/getPuestos');
            selJefeInmediatoChang.getAutocompleteValid(funSelJefeIn, funSelJefeInNull, null, '/Administrativo/Finiquito/getEmpleados');
            // selRegistroPatronalChang.getAutocompleteValid(funSelRegPatronal, funSelRegPatronalNull, null, '/Administrativo/FormatoCambio/getRegPatronal');
            selRegistroPatronalChang.fillCombo("/Administrativo/Reclutamientos/FillComboRegistroPatronal", { cc: selCentroCostoChang.val() }, false, null);
            selAutSolicitaEnvia.getAutocomplete(funAutEnvia, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutSolicita.getAutocomplete(funAutSolicita, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutVoBo.getAutocomplete(funAutVoBo, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutVoBo2.getAutocomplete(funAutVoBo2, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutoriza1.getAutocomplete(funAutoriza1, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutoriza12.getAutocomplete(funAutoriza12, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutoriza2.getAutocomplete(funAutoriza2, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutoriza22.getAutocomplete(funAutoriza22, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutoriza3.getAutocomplete(funAutoriza3, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutoriza32.getAutocomplete(funAutoriza32, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');

            lblIgNombre.getAutocomplete(setIdEmpleado, null, '/Administrativo/FormatoCambio/getEmpleados');
        }

        function setIdEmpleado(event, ui) {
            txtIgNumeroEmpleado.val(ui.item.id);
            txtIgNumeroEmpleado.trigger('change');
        }

        function funSelPuesto(event, ui) {
            selPuestoChang.text(ui.item.value);
            objEmpleadoCambios.Puesto = ui.item.value;
            objEmpleadoCambios.PuestoID = ui.item.id;

            selLineaNegocio.fillCombo('/Administrativo/Reclutamientos/GetLineaNegocioByCC', { cc: objEmpleadoCambios.CcID }, null, null, () => {
                selLineaNegocio.find('option:selected').remove();
                selLineaNegocio.trigger("change");
            });
        }
        function funSelPuestoNull(event, ui) {
            if (ui.item === null) {
                selPuestoChang.val('');
                selPuestoChang.text('');
                objEmpleadoCambios.Puesto = '';
                objEmpleadoCambios.PuestoID = 0;
                AlertaGeneral("Alerta", "Debe seleccionar un puesto de la lista. Si no aparece en la lista de autocompletado favor de solicitar al personal de T.I.");
            }
        }

        function funSelCC(clave_depto, actualizarLineaNegocio) {
            let CcID = selCentroCostoChang.val()
                , cc = selCentroCostoChang.find(`option[value="${CcID}"]`).text().split('-')[1].trim();
            if (cc != lblIgCentro.html() && lblIgBonoMensual.getVal(2) != 0) {
                txtBonoNominaChange.val(0);
                txtBonoNominaChange.change();
                AlertaGeneral("Alerta", "El empleado tiene un bono, el cual se resetea al cambiar de CC, si desea que siga teniendo este bono favor de capturarlo nuevamente");
            }
            if (CcID.length < 3) {
                CC = '';
                CcID = 0;
                AlertaGeneral("Alerta", "Debe seleccionar un Centro de Costo de la lista. Si no aparece en la lista de autocompletado favor de solicitar al personal de T.I.");
            }
            objEmpleadoCambios.CC = cc;
            objEmpleadoCambios.CcID = CcID;

            selDepartamento.fillCombo('/Administrativo/FormatoCambio/getDepartamentosCC', { cc: CcID }, null, null, () => {
                $('#selDepartamento').val(clave_depto);
            });

            if (clave_depto > 0) {
                fnGetRegistroPatCC(selCentroCostoChang.val());
            } else {
                fnGetResponsableCC(selCentroCostoChang.val());
                fnGetRegistroPatCC(selCentroCostoChang.val());
            }

            if (actualizarLineaNegocio) {
                selLineaNegocio.fillCombo('/Administrativo/Reclutamientos/GetLineaNegocioByCC', { cc: CcID }, null, null, () => {
                    selLineaNegocio.find('option:selected').remove();
                    selLineaNegocio.trigger("change");
                });
            }
        }

        function funSelDepartamento() {
            objEmpleadoCambios.Departamento = selDepartamento.find('option:selected').text();
            objEmpleadoCambios.ClaveDepartamento = selDepartamento.val();
        }

        function funSelJefeIn(event, ui) {
            selJefeInmediatoChang.text(ui.item.value);
            objEmpleadoCambios.Nombre_Jefe_Inmediato = ui.item.value;
            objEmpleadoCambios.Clave_Jefe_Inmediato = ui.item.id;
        }

        function funSelJefeInNull(event, ui) {
            if (ui.item === null) {
                selJefeInmediatoChang.val('');
                selJefeInmediatoChang.text('');
                objEmpleadoCambios.Nombre_Jefe_Inmediato = '';
                objEmpleadoCambios.Clave_Jefe_Inmediato = 0;
                AlertaGeneral("Alerta", "Debe seleccionar un usuario de la lista. Si no aparece en la lista de autocompletado favor de solicitar al personal de T.I.");
            }
        }

        function funSelRegPatronal(event, ui) {
            selRegistroPatronalChang.text(ui.item.value);
            objEmpleadoCambios.RegistroPatronal = ui.item.value;
            objEmpleadoCambios.RegistroPatronalID = ui.item.id;
        }
        function funSelRegPatronalNull(event, ui) {
            if (ui.item === null) {
                selRegistroPatronalChang.val('');
                selRegistroPatronalChang.text('');
                objEmpleadoCambios.RegistroPatronal = '';
                objEmpleadoCambios.RegistroPatronalID = 0;
                AlertaGeneral("Alerta", "Debe seleccionar un registro patronal de la lista. Si no aparece en la lista de autocompletado favor de solicitar al personal de T.I.");
            }
        }
        function revisarExisteAutorizadores() {
            if (selAutSolicitaEnvia.val() == "") {
                argAutorizaciones = $.grep(argAutorizaciones,
                    function (o, i) { return o.PuestoAprobador == "Gerente Actual"; },
                    true);
            }
            if (selAutSolicita.val() == "") {
                argAutorizaciones = $.grep(argAutorizaciones,
                    function (o, i) { return o.PuestoAprobador == "Gerente Recibe"; },
                    true);
            }
            if (selAutVoBo.val() == "") {
                argAutorizaciones = $.grep(argAutorizaciones,
                    function (o, i) { return o.PuestoAprobador == "Capital Humano 2"; },
                    true);
            }
            if (selAutVoBo2.val() == "") {
                argAutorizaciones = $.grep(argAutorizaciones,
                    function (o, i) { return o.PuestoAprobador == "Capital Humano 2"; },
                    true);
            }
            if (selAutoriza1.val() == "") {
                argAutorizaciones = $.grep(argAutorizaciones,
                    function (o, i) { return o.PuestoAprobador == "Gerente/SubDirector/Director de Área 1"; },
                    true);
            }
            if (selAutoriza12.val() == "") {
                argAutorizaciones = $.grep(argAutorizaciones,
                    function (o, i) { return o.PuestoAprobador == "Gerente/SubDirector/Director de Área 2"; },
                    true);
            }
            if (selAutoriza2.val() == "") {
                argAutorizaciones = $.grep(argAutorizaciones,
                    function (o, i) { return o.PuestoAprobador == "Director de Línea de Negocios 1"; },
                    true);
            }
            if (selAutoriza22.val() == "") {
                argAutorizaciones = $.grep(argAutorizaciones,
                    function (o, i) { return o.PuestoAprobador == "Director de Línea de Negocios 2"; },
                    true);
            }
            if (selAutoriza3.val() == "") {
                argAutorizaciones = $.grep(argAutorizaciones,
                    function (o, i) { return o.PuestoAprobador == "Alta Dirección 1"; },
                    true);
            }
            if (selAutoriza32.val() == "") {
                argAutorizaciones = $.grep(argAutorizaciones,
                    function (o, i) { return o.PuestoAprobador == "Alta Dirección 2"; },
                    true);
            }
        }

        function funAutEnvia(event, ui) {
            selAutSolicitaEnvia.text(ui.item.value);


            var a = $.grep(argAutorizaciones,
                function (o, i) { return o.PuestoAprobador != "Responsable del Centro de Costos Actual"; },
                true);
            if (a.length > 0) {

                for (var i = 0; i < argAutorizaciones.length; i++) {
                    if (argAutorizaciones[i].PuestoAprobador == "Responsable del Centro de Costos Actual") {
                        argAutorizaciones[i].Clave_Aprobador = ui.item.id;
                        argAutorizaciones[i].Nombre_Aprobador = ui.item.value;
                    }
                }
            }
            else {
                objAutorizacion = {
                    Clave_Aprobador: ui.item.id,
                    Nombre_Aprobador: ui.item.value,
                    PuestoAprobador: "Responsable del Centro de Costos Actual",
                    Responsable: "Gerente Actual",
                    Orden: 1,
                    tipoAutoriza: false
                };
                argAutorizaciones.push(objAutorizacion);
            }

        }
        function funAutSolicita(event, ui) {
            selAutSolicita.text(ui.item.value);


            var a = $.grep(argAutorizaciones,
                function (o, i) { return o.PuestoAprobador != "Responsable del Centro de Costos Recibe"; },
                true);
            if (a.length > 0) {

                for (var i = 0; i < argAutorizaciones.length; i++) {
                    if (argAutorizaciones[i].PuestoAprobador == "Responsable del Centro de Costos Recibe") {
                        argAutorizaciones[i].Clave_Aprobador = ui.item.id;
                        argAutorizaciones[i].Nombre_Aprobador = ui.item.value;
                    }
                }
            }
            else {
                objAutorizacion = {
                    Clave_Aprobador: ui.item.id,
                    Nombre_Aprobador: ui.item.value,
                    PuestoAprobador: "Responsable del Centro de Costos Recibe",
                    Responsable: "Gerente Recibe",
                    Orden: 2,
                    tipoAutoriza: false
                };
                argAutorizaciones.push(objAutorizacion);
            }

        }
        function funAutVoBo(event, ui) {
            selAutVoBo.text(ui.item.value);


            var a = $.grep(argAutorizaciones,
                function (o, i) { return o.PuestoAprobador != "Capital Humano 1"; },
                true);
            if (a.length > 0) {

                for (var i = 0; i < argAutorizaciones.length; i++) {
                    if (argAutorizaciones[i].PuestoAprobador == "Capital Humano 1") {
                        argAutorizaciones[i].Clave_Aprobador = ui.item.id;
                        argAutorizaciones[i].Nombre_Aprobador = ui.item.value;
                    }
                }
            }
            else {
                objAutorizacion = {
                    Clave_Aprobador: ui.item.id,
                    Nombre_Aprobador: ui.item.value,
                    PuestoAprobador: "Capital Humano 1",
                    Responsable: "VoBo",
                    Orden: 3,
                    tipoAutoriza: false
                };
                argAutorizaciones.push(objAutorizacion);
            }

        }
        function funAutVoBo2(event, ui) {
            selAutVoBo2.text(ui.item.value);


            var a = $.grep(argAutorizaciones,
                function (o, i) { return o.PuestoAprobador != "Capital Humano 2"; },
                true);
            if (a.length > 0) {

                for (var i = 0; i < argAutorizaciones.length; i++) {
                    if (argAutorizaciones[i].PuestoAprobador == "Capital Humano 2") {
                        argAutorizaciones[i].Clave_Aprobador = ui.item.id;
                        argAutorizaciones[i].Nombre_Aprobador = ui.item.value;
                    }
                }
            }
            else {
                objAutorizacion = {
                    Clave_Aprobador: ui.item.id,
                    Nombre_Aprobador: ui.item.value,
                    PuestoAprobador: "Capital Humano 2",
                    Responsable: "VoBo",
                    Orden: 4,
                    tipoAutoriza: false
                };
                argAutorizaciones.push(objAutorizacion);
            }

        }

        function funAutoriza1(event, ui) {
            selAutoriza1.text(ui.item.value);


            var a = $.grep(argAutorizaciones,
                function (o, i) { return o.PuestoAprobador != "Gerente/SubDirector/Director de Área 1"; },
                true);
            if (a.length > 0) {

                for (var i = 0; i < argAutorizaciones.length; i++) {
                    if (argAutorizaciones[i].PuestoAprobador == "Gerente/SubDirector/Director de Área 1") {
                        argAutorizaciones[i].Clave_Aprobador = ui.item.id;
                        argAutorizaciones[i].Nombre_Aprobador = ui.item.value;
                    }
                }
            }
            else {
                objAutorizacion = {
                    Clave_Aprobador: ui.item.id,
                    Nombre_Aprobador: ui.item.value,
                    PuestoAprobador: "Gerente/SubDirector/Director de Área 1",
                    Responsable: "Autorización 1",
                    Orden: 5,
                    tipoAutoriza: false
                };
                argAutorizaciones.push(objAutorizacion);
            }

        }
        function funAutoriza12(event, ui) {
            selAutoriza12.text(ui.item.value);

            var a = $.grep(argAutorizaciones,
                function (o, i) { return o.PuestoAprobador != "Gerente/SubDirector/Director de Área 2"; },
                true);
            if (a.length > 0) {

                for (var i = 0; i < argAutorizaciones.length; i++) {
                    if (argAutorizaciones[i].PuestoAprobador == "Gerente/SubDirector/Director de Área 2") {
                        argAutorizaciones[i].Clave_Aprobador = ui.item.id;
                        argAutorizaciones[i].Nombre_Aprobador = ui.item.value;
                    }
                }
            }
            else {
                objAutorizacion = {
                    Clave_Aprobador: ui.item.id,
                    Nombre_Aprobador: ui.item.value,
                    PuestoAprobador: "Gerente/SubDirector/Director de Área 2",
                    Responsable: "Autorización 1",
                    Orden: 6,
                    tipoAutoriza: false
                };
                argAutorizaciones.push(objAutorizacion);
            }
        }
        function funAutoriza2(event, ui) {
            selAutoriza2.text(ui.item.value);

            var a = $.grep(argAutorizaciones,
                function (o, i) { return o.PuestoAprobador != "Director de Línea de Negocios 1"; },
                true);
            if (a.length > 0) {

                for (var i = 0; i < argAutorizaciones.length; i++) {
                    if (argAutorizaciones[i].PuestoAprobador == "Director de Línea de Negocios 1") {
                        argAutorizaciones[i].Clave_Aprobador = ui.item.id;
                        argAutorizaciones[i].Nombre_Aprobador = ui.item.value;
                    }
                }
            }
            else {
                objAutorizacion = {
                    Clave_Aprobador: ui.item.id,
                    Nombre_Aprobador: ui.item.value,
                    PuestoAprobador: "Director de Línea de Negocios 1",
                    Responsable: "Autorización 2",
                    Orden: 7,
                    tipoAutoriza: false
                };
                argAutorizaciones.push(objAutorizacion);
            }

        }
        function funAutoriza22(event, ui) {
            selAutoriza22.text(ui.item.value);

            var a = $.grep(argAutorizaciones,
                function (o, i) { return o.PuestoAprobador != "Director de Línea de Negocios 2"; },
                true);
            if (a.length > 0) {

                for (var i = 0; i < argAutorizaciones.length; i++) {
                    if (argAutorizaciones[i].PuestoAprobador == "Director de Línea de Negocios 2") {
                        argAutorizaciones[i].Clave_Aprobador = ui.item.id;
                        argAutorizaciones[i].Nombre_Aprobador = ui.item.value;
                    }
                }
            }
            else {
                objAutorizacion = {
                    Clave_Aprobador: ui.item.id,
                    Nombre_Aprobador: ui.item.value,
                    PuestoAprobador: "Director de Línea de Negocios 2",
                    Responsable: "Autorización 2",
                    Orden: 8,
                    tipoAutoriza: false
                };
                argAutorizaciones.push(objAutorizacion);
            }
        }
        function funAutoriza3(event, ui) {
            selAutoriza3.text(ui.item.value);
            var a = $.grep(argAutorizaciones,
                function (o, i) { return o.PuestoAprobador != "Alta Dirección 1"; },
                true);
            if (a.length > 0) {

                for (var i = 0; i < argAutorizaciones.length; i++) {
                    if (argAutorizaciones[i].PuestoAprobador == "Alta Dirección 1") {
                        argAutorizaciones[i].Clave_Aprobador = ui.item.id;
                        argAutorizaciones[i].Nombre_Aprobador = ui.item.value;
                    }
                }
            }
            else {
                objAutorizacion = {
                    Clave_Aprobador: ui.item.id,
                    Nombre_Aprobador: ui.item.value,
                    PuestoAprobador: "Alta Dirección 1",
                    Responsable: "Autorización 3",
                    Orden: 9,
                    tipoAutoriza: false
                };
                argAutorizaciones.push(objAutorizacion);
            }

        }
        function funAutoriza32(event, ui) {
            selAutoriza32.text(ui.item.value);
            var a = $.grep(argAutorizaciones,
                function (o, i) { return o.PuestoAprobador != "Alta Dirección 2"; },
                true);
            if (a.length > 0) {

                for (var i = 0; i < argAutorizaciones.length; i++) {
                    if (argAutorizaciones[i].PuestoAprobador == "Alta Dirección 2") {
                        argAutorizaciones[i].Clave_Aprobador = ui.item.id;
                        argAutorizaciones[i].Nombre_Aprobador = ui.item.value;
                    }
                }
            }
            else {
                objAutorizacion = {
                    Clave_Aprobador: ui.item.id,
                    Nombre_Aprobador: ui.item.value,
                    PuestoAprobador: "Alta Dirección 2",
                    Responsable: "Autorización 3",
                    Orden: 10,
                    tipoAutoriza: false
                };
                argAutorizaciones.push(objAutorizacion);
            }
        }
        function arrayObjectIndexOf(myArray, searchTerm, property) {
            for (var i = 0, len = myArray.length; i < len; i++) {
                if (myArray[i][property] === searchTerm) return i;
            }
            return -1;
        }

        function fnGetResponsableCC(cc) {
            axios.post("GetResponsableCC", { cc }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (items.length > 0) {
                        selJefeInmediatoChang.val(items[0].Text);

                    } else {
                        selJefeInmediatoChang.val("");

                    }
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fnGetRegistroPatCC(cc) {
            axios.post("GetRegistroPatCC", { cc }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (items.length > 0) {
                        // selRegistroPatronalChang.val(items[0].nombre_corto);
                        selRegistroPatronalChang.fillCombo("/Administrativo/Reclutamientos/FillComboRegistroPatronal", { cc: selCentroCostoChang.val() }, false, null, () => {
                            selRegistroPatronalChang.val(items[0].clave_reg_pat);
                            selRegistroPatronalChang.trigger("change");
                        });



                    } else {
                        selRegistroPatronalChang.val("");
                        selRegistroPatronalChang.trigger("change");

                    }
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fnGetTabuladorByEmpleado() {
            axios.post("GetTabuladorByEmpleado", { puesto: objEmpleadoCambios.PuestoID, lineaNegocios: selLineaNegocio.val(), categoria: selCategoria.val() }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    if (items != null) {
                        objEmpleadoCambios.Salario_Base = items.sueldoBase;
                        objEmpleadoCambios.Complemento = items.complemento;

                        txtSueldoNominaChange.val(items.sueldoBase);
                        txtComplementoNominaChange.val(items.complemento);

                        //RANGO TABULADORES

                        if (response.data.esRango) {
                            if (response.data.lowerTabulador.id > 0) {
                                lowerBase = Number(response.data.lowerTabulador.sueldoBase) > 0 ? (Number(response.data.lowerTabulador.sueldoBase) + 1) : 0;
                                lowerComplemento = Number(response.data.lowerTabulador.complemento) > 0 ? (Number(response.data.lowerTabulador.complemento) + 1) : 0;
                                upperBase = items.sueldoBase;
                                upperComplemento = items.complemento;

                            } else {
                                lowerBase = 0;
                                lowerComplemento = 0;
                                upperBase = items.sueldoBase;
                                upperComplemento = items.complemento;

                            }
                        } else {
                            lowerBase = 0;
                            lowerComplemento = 0;
                            upperBase = 0;
                            upperComplemento = 0;
                        }

                        console.log(lowerBase);
                        console.log(lowerComplemento);
                        console.log(upperBase);
                        console.log(upperComplemento);

                        selectTNomina();

                    } else {
                        lowerBase = 0;
                        lowerComplemento = 0;
                        upperBase = 0;
                        upperComplemento = 0;

                    }
                } else {
                    lowerBase = 0;
                    lowerComplemento = 0;
                    upperBase = 0;
                    upperComplemento = 0;

                }
            }).catch(error => Alert2Error(error.message));
        }
    }

    $(document).ready(function () {
        recursoshumanos.formatocambio.FormatoCambio = new FormatoCambio();
    });
})();