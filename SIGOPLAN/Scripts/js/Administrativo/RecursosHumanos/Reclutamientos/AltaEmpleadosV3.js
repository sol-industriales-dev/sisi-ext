(() => {
    $.fn.modal.Constructor.prototype.enforceFocus = function () { };
    $.namespace('CH.AltaEmpleados');

    const inputPermisoEditarFechaCambio = $('#inputPermisoEditarFechaCambio');
    const inputPermisoOcultarTabulador = $('#inputPermisoOcultarTabulador');
    const inputPermisoMostarBotones = $('#inputPermisoMostarBotones');
    const inputPermisoVerSalarios = $('#inputPermisoVerSalarios');

    let esPermisoMostrarBotones = inputPermisoMostarBotones.val() == 1 ? true : false;

    const txtEmpresa = $('#txtEmpresa');
    const empleadosAdmn = $('#empleadosAdmn');

    //#region CONST FILTROS PRINCIPALES
    const btnCrearEmpleado = $('#btnCrearEmpleado');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnFiltroExportar = $('#btnFiltroExportar');
    const btnAutorizacionMultiple = $('#btnAutorizacionMultiple');
    const btnDiasIngresos = $('#btnDiasIngresos');
    const cboFiltroCC = $('#cboFiltroCC');
    const motivoSueldo = $('#motivoSueldo');
    const btnEnviarCorreos = $('#btnEnviarCorreos');
    //#endregion

    //#region DIAS INGRESO
    const mdlDiasIngreso = $('#mdlDiasIngreso');
    const btnActualizarDiasIngresos = $('#btnActualizarDiasIngresos');
    const txtDiasAnterioresPermitidos = $('#txtDiasAnterioresPermitidos');
    const txtDiasPosterioresPermitidos = $('#txtDiasPosterioresPermitidos');
    //#endregion

    //#region CONST EMPLEADOS

    //#region CONST LISTADO EMPLEADOS
    let tblRH_REC_Empleados = $('#tblRH_REC_Empleados');
    const divTblEmpleados = $('#divTblEmpleados');
    //#endregion

    //#region CONST CREAR/EDITAR EMPLEADOS
    const cboCandidatosAprobados = $('#cboCandidatosAprobados');
    const mdlCrearEditarEmpleadoEK = $('#mdlCrearEditarEmpleadoEK');
    const btnCEEmpleado = $('#btnCEEmpleado');
    const lblTitleCrearEditarEmpleadoEK = $('#lblTitleCrearEditarEmpleadoEK');
    const lblCEDatosEmpleadoClaveEmpleado = $('#lblCEDatosEmpleadoClaveEmpleado');
    const lblCEDatosEmpleadoEstatus = $('#lblCEDatosEmpleadoEstatus');
    const txtCEDatosEmpleadoNombre = $('#txtCEDatosEmpleadoNombre');
    const txtCEDatosEmpleadoApePaterno = $('#txtCEDatosEmpleadoApePaterno');
    const txtCEDatosEmpleadoApeMaterno = $('#txtCEDatosEmpleadoApeMaterno');
    const txtCEDatosEmpleadoFechaNacimiento = $('#txtCEDatosEmpleadoFechaNacimiento');
    const txtCEDatosEmpleadoFechaIngreso = $('#txtCEDatosEmpleadoFechaIngreso');
    const cboCEDatosEmpleadoPaisNac = $('#cboCEDatosEmpleadoPaisNac');
    const cboCEDatosEmpleadoSexo = $('#cboCEDatosEmpleadoSexo');
    const cboCEDatosEmpleadoEstadoNac = $('#cboCEDatosEmpleadoEstadoNac');
    const txtCEDatosEmpleadoRFC = $('#txtCEDatosEmpleadoRFC');
    const cboCEDatosEmpleadoLugarNac = $('#cboCEDatosEmpleadoLugarNac');
    const txtCEDatosEmpleadoCURP = $('#txtCEDatosEmpleadoCURP');
    const txtCEDatosEmpleadoLocalidadNac = $('#txtCEDatosEmpleadoLocalidadNac');
    const txtCEDatosEmpleadoCUSPP = $('#txtCEDatosEmpleadoCUSPP'); //PERU
    const cboCEDatosEmpleadoDepartamentoNac = $('#cboCEDatosEmpleadoDepartamentoNac');
    const divCeDatosEmpleadosDepartamentoNac = $('#divCeDatosEmpleadosDepartamentoNac'); //PERU
    const chkFiltroEsPendiente = $('#chkFiltroEsPendiente');
    const divCandidatosAprobados = $('#divCandidatosAprobados');
    const nomina_tab = $('#nomina-tab');
    const home_tab = $('#home-tab');
    const txtCEDatosEmpleadoCPCIF = $('#txtCEDatosEmpleadoCPCIF'); //MEXICO
    //#endregion

    //#endregion

    //#region CONST GENERALES Y CONTACTO
    const cboCEGenContactoEstadoCivil = $('#cboCEGenContactoEstadoCivil');
    const txtCEGenContactoFechaPlanta = $('#txtCEGenContactoFechaPlanta');
    const txtCEGenContactoEstudios = $('#txtCEGenContactoEstudios');
    const txtCEGenContactoAbreviatura = $('#txtCEGenContactoAbreviatura');
    const txtCEGenContactoCredElector = $('#txtCEGenContactoCredElector');
    const txtCEGenContactoDNI = $('#txtCEGenContactoDNI');
    const txtCEGenContactoCalle = $('#txtCEGenContactoCalle');
    const txtCEGenContactoNumExterior = $('#txtCEGenContactoNumExterior');
    const txtCEGenContactoNumInterior = $('#txtCEGenContactoNumInterior');
    const txtCEGenContactoColonia = $('#txtCEGenContactoColonia');
    const cboCEGenContactoEstado = $('#cboCEGenContactoEstado');
    const cboCEGenContactoCiudad = $('#cboCEGenContactoCiudad');
    const txtCEGenContactoCP = $('#txtCEGenContactoCP');
    const txtCEGenContactoTelCasa = $('#txtCEGenContactoTelCasa');
    const txtCEGenContactoTelCelular = $('#txtCEGenContactoTelCelular');
    const txtCEGenContactoEmail = $('#txtCEGenContactoEmail');
    const cboCEGenContactoTipoCasa = $('#cboCEGenContactoTipoCasa');
    const cboCEGenContactoTipoSangre = $('#cboCEGenContactoTipoSangre');
    const txtCEGenContactoAlergias = $('#txtCEGenContactoAlergias');
    const chkCEGenContactoAlergias = $('#chkCEGenContactoAlergias');
    const divCEGenContactoAlergias = $('#divCEGenContactoAlergias');
    const cboCEGenContactoEmpleadoPais = $('#cboCEGenContactoEmpleadoPais');
    const txtCEGenContactoCedulaCuidadania = $('#txtCEGenContactoCedulaCuidadania');
    const cboCEGenContactoDepartamentoNac = $('#cboCEGenContactoDepartamentoNac');
    const divCeGenContactoDepartamentoNac = $('#divCeGenContactoDepartamentoNac');
    //#endregion

    //#region CONST BENEFICIARIOS

    //#region CONST FORMULARIO CREAR/EDITAR BENEFICIARIO
    const cboCEBeneficiarioParentesco = $('#cboCEBeneficiarioParentesco');
    const txtCEBeneficiarioFechaNacimiento = $('#txtCEBeneficiarioFechaNacimiento');
    const txtCEBeneficiarioCP = $('#txtCEBeneficiarioCP');
    const txtCEBeneficiarioApePaterno = $('#txtCEBeneficiarioApePaterno');
    const txtCEBeneficiarioApeMaterno = $('#txtCEBeneficiarioApeMaterno');
    const txtCEBeneficiarioNombre = $('#txtCEBeneficiarioNombre');
    const cboCEBeneficiarioEstado = $('#cboCEBeneficiarioEstado');
    const cboCEBeneficiarioCiudad = $('#cboCEBeneficiarioCiudad');
    const txtCEBeneficiarioColonia = $('#txtCEBeneficiarioColonia');
    const txtCEBeneficiarioDomicilio = $('#txtCEBeneficiarioDomicilio');
    const txtCEBeneficiarioNumExt = $('#txtCEBeneficiarioNumExt');
    const txtCEBeneficiarioNumInt = $('#txtCEBeneficiarioNumInt');
    const txtCEBeneficiarioCel = $('#txtCEBeneficiarioCel');
    const cboCEBeneficiarioEmpleadoPais = $('#cboCEBeneficiarioEmpleadoPais');
    const txtCEBeneficiarioCURP = $('#txtCEBeneficiarioCURP');
    const cboCEBeneficiarioDepartamentoNac = $('#cboCEBeneficiarioDepartamentoNac');
    const cboCEBeneficiarioDepartamentoNacIndex = $('#cboCEBeneficiarioDepartamentoNacIndex');
    //#endregion

    //#region CONST FORMULARIO CREAR/EDITAR CASO DE ACCIDENTE
    const txtCECasoAccidenteNombre = $('#txtCECasoAccidenteNombre');
    const txtCECasoAccidenteTelefono = $('#txtCECasoAccidenteTelefono');
    const txtCECasoAccidenteDomicilio = $('#txtCECasoAccidenteDomicilio');
    const btnMdlCopyBeneficiario = $('#btnMdlCopyBeneficiario');
    //#endregion

    //#endregion

    //#region CONST COMPAÑIA
    //#region CONST FORMULARIO CREAR/EDITAR COMPAÑIA
    const txtCECompaniaRequisicion = $('#txtCECompaniaRequisicion');
    const txtCECompaniaActividades = $('#txtCECompaniaActividades');
    const chkCECompaniaSindicato = $('#chkCECompaniaSindicato');
    const txtCECompaniaAutoriza = $('#txtCECompaniaAutoriza');
    const txtCECompaniaUsuarioResg = $('#txtCECompaniaUsuarioResg');
    const txtCECompaniaDepto = $('#txtCECompaniaDepto');
    const txtCECompaniaNSS = $('#txtCECompaniaNSS');
    // const cboCECompaniaTipoFormula = $('#cboCECompaniaTipoFormula');
    const txtCECompaniaContrato = $('#txtCECompaniaContrato');
    const lblCECompaniaAltaEnElSistema = $('#lblCECompaniaAltaEnElSistema');
    const lblCECompaniaAntiguedad = $('#lblCECompaniaAntiguedad');
    const selectRegistroPatronal = $('#selectRegistroPatronal');
    const txtCECompaniaCCDescripcion = $('#txtCECompaniaCCDescripcion');
    const txtCECompaniaPuestoDescripcion = $('#txtCECompaniaPuestoDescripcion');
    const selectTipoContrato = $('#selectTipoContrato');
    const txtCECompaniaJefeInmediatoDescripcion = $('#txtCECompaniaJefeInmediatoDescripcion');
    const txtCECompaniaAutorizaDescripcion = $('#txtCECompaniaAutorizaDescripcion');
    const txtCECompaniaUsuarioResgDescripcion = $('#txtCECompaniaUsuarioResgDescripcion');
    const txtCECompaniaDeptoDescripcion = $('#txtCECompaniaDeptoDescripcion');
    const txtCECompaniaPlantilla = $('#txtCECompaniaPlantilla');
    const txtCEUltimaModificacionIDEK = $('#txtCEUltimaModificacionIDEK');
    const txtCEUltimaModificacionNombreUsuario = $('#txtCEUltimaModificacionNombreUsuario');
    const cboCECompaniaPuestoCategoria = $('#cboCECompaniaPuestoCategoria');
    const divCECompaniaPuestoCategoria = $('#divCECompaniaPuestoCategoria');
    const dateCECompaniaContratoFin = $('#dateCECompaniaContratoFin');
    //#endregion
    //#endregion

    //#region CONST FAMILIARES

    //#region CONST LISTADO EMPLEADOS
    const tblRH_REC_Familiares = $('#tblRH_REC_Familiares');
    let dtFamiliares
    //#endregion

    //#region CONST CREAR/EDITAR EMPLEADOS REL FAMILIARES
    const btnMdlFamiliar = $('#btnMdlFamiliar');
    const lblTitleCEFamiliares = $('#lblTitleCEFamiliares');
    const mdlCrearEditarFamiliar = $('#mdlCrearEditarFamiliar');
    const cboCEFamiliarParentesco = $('#cboCEFamiliarParentesco');
    const cboCEFamiliarGenero = $('#cboCEFamiliarGenero');
    const txtCEFamiliarFechaNacimiento = $('#txtCEFamiliarFechaNacimiento');
    const chkCEFamiliarVive = $('#chkCEFamiliarVive');
    const txtCEFamiliarNombre = $('#txtCEFamiliarNombre');
    const txtCEFamiliarApePaterno = $('#txtCEFamiliarApePaterno');
    const txtCEFamiliarApeMaterno = $('#txtCEFamiliarApeMaterno');
    const cboCEFamiliarEstadoCivil = $('#cboCEFamiliarEstadoCivil');
    const txtCEFamiliarGradoEstudios = $('#txtCEFamiliarGradoEstudios');
    const chkCEFamiliarBeneficiario = $('#chkCEFamiliarBeneficiario');
    const chkCEFamiliarTrabaja = $('#chkCEFamiliarTrabaja');
    const chkCEFamiliarEstudia = $('#chkCEFamiliarEstudia');
    const txtCEFamiliarComentarios = $('#txtCEFamiliarComentarios');
    const btnCEFamiliar = $('#btnCEFamiliar');
    const lblBtnCEFamiliar = $('#lblBtnCEFamiliar');
    //PERU
    const txtCEFamiliarDNI = $('#txtCEFamiliarDNI');
    const cboCEAsignacionEscolar = $('#cboCEAsignacionEscolar');
    const cboCETipoEmpleado = $('#cboCETipoEmpleado');
    const btnCargarSustentoHijo = $('#btnCargarSustentoHijo');
    //COLOMBIA
    const txtCEFamiliarCedula = $('#txtCEFamiliarCedula');
    //#endregion

    //#endregion

    //#region CONST NOMINAS
    const tblRH_REC_Tabuladores = $('#tblRH_REC_Tabuladores');
    let dtTabuladores;
    const btnShowMdlNomina = $('#btnShowMdlNomina');
    const mdlCrearEditarTabuladores = $('#mdlCrearEditarTabuladores');
    const cboCETabuladorTipoNomina = $('#cboCETabuladorTipoNomina');
    const chkCETabuladorLibre = $('#chkCETabuladorLibre');
    const txtCETabuladorNumTabulador = $('#txtCETabuladorNumTabulador');
    const cboCETabuladorBanco = $('#cboCETabuladorBanco');
    const txtCETabuladorTarjeta = $('#txtCETabuladorTarjeta');
    const txtCETabuladorCtaAhorro = $('#txtCETabuladorCtaAhorro');
    const chkCETabuladorTarjetaNomina = $('#chkCETabuladorTarjetaNomina');
    const btnCETabulador = $('#btnCETabulador');

    const cboTipoNomina = $('#cboTipoNomina');
    const chkTabuladorLibre = $('#chkTabuladorLibre');
    const txtNumeroTabulador = $('#txtNumeroTabulador');
    const cboBancoNomina = $('#cboBancoNomina');
    const chkTarjetaNomina = $('#chkTarjetaNomina');
    const txtTarjetaNomina = $('#txtTarjetaNomina');
    const txtClaveInterbancaria = $('#txtClaveInterbancaria');
    const mdlTabuladorLibre = $('#mdlTabuladorLibre');
    const txtTabuladorLibreFecha = $('#txtTabuladorLibreFecha');
    const txtTabuladorLibreSalarioBase = $('#txtTabuladorLibreSalarioBase');
    const txtTabuladorLibreComplemento = $('#txtTabuladorLibreComplemento');
    const txtTabuladorLibreBono = $('#txtTabuladorLibreBono');
    const txtTabuladorLibreTotal = $('#txtTabuladorLibreTotal');
    const btnAgregarTabuladorLibre = $('#btnAgregarTabuladorLibre');

    //#region SALARIO TABULADOR
    const mdlCrearEditarNominaSalario = $('#mdlCrearEditarNominaSalario');
    const txtCENominaSalarioSalarioBase = $('#txtCENominaSalarioSalarioBase');
    const txtCENominaSalarioComplemento = $('#txtCENominaSalarioComplemento');
    const txtCENominaSalarioBonoZona = $('#txtCENominaSalarioBonoZona');
    const txtCENominaSalarioTotal = $('#txtCENominaSalarioTotal');
    const btnCENominaSalario = $('#btnCENominaSalario');

    const divInfomacionBacaria = $('#divInfomacionBacaria');
    const divTablaTabuladores = $('#divTablaTabuladores');
    //#endregion
    //#endregion

    //#region CONST IMPRIMIR CONTRATOS
    const mdlImprimirContratos = $('#mdlImprimirContratos');
    const btnImprimirContratos = $('#btnImprimirContratos');
    const chkTipoDocumento1 = $('#chkTipoDocumento1');
    const chkTipoDocumento2 = $('#chkTipoDocumento2');
    const chkTipoDocumento3 = $('#chkTipoDocumento3');
    const chkTipoDocumento4 = $('#chkTipoDocumento4');
    const chkTipoDocumento5 = $('#chkTipoDocumento5');
    const chkTipoDocumento6 = $('#chkTipoDocumento6');
    const chkTipoDocumento7 = $('#chkTipoDocumento7');
    const chkTipoDocumento8 = $('#chkTipoDocumento8');
    const chkTipoDocumento9 = $('#chkTipoDocumento9');
    const chkTipoDocumento10 = $('#chkTipoDocumento10');
    const chkTipoDocumento11 = $('#chkTipoDocumento11');

    const btnImprimirCredencial = $('#btnImprimirCredencial');
    //#endregion

    //#region CONST UNIFORMES
    const tblRH_REC_Uniformes = $('#tblRH_REC_Uniformes');
    let dtUniformes;
    const btnMdlUniforme = $('#btnMdlUniforme');
    const mdlCrearEditarUniforme = $('#mdlCrearEditarUniforme');
    const lblTitleCEUniforme = $('#lblTitleCEUniforme');
    const lblBtnCEUniforme = $('#lblBtnCEUniforme');

    //#region CTRLS MODAL CE UNIFORME
    const txtCEUniformeFechaEntrega = $('#txtCEUniformeFechaEntrega');
    const txtCEUniformeNoCalzado = $('#txtCEUniformeNoCalzado');
    const chkCEUniformeCalzado = $('#chkCEUniformeCalzado');
    const txtCEUniformeCamisa = $('#txtCEUniformeCamisa');
    const chkCEUniformeCamisa = $('#chkCEUniformeCamisa');
    const txtCEUniformePantalon = $('#txtCEUniformePantalon');
    const chkCEUniformePantalon = $('#chkCEUniformePantalon');
    const txtCEUniformeOverol = $('#txtCEUniformeOverol');
    const chkCEUniformeOverol = $('#chkCEUniformeOverol');
    // const txtCEUniformeUniformeDama = $('#txtCEUniformeUniformeDama');
    // const chkCEUniformeUniformeDama = $('#chkCEUniformeUniformeDama');
    const txtCEUniformeOtros = $('#txtCEUniformeOtros');
    const txtCEUniformeComentarios = $('#txtCEUniformeComentarios');
    const btnCEUniforme = $('#btnCEUniforme');
    //#endregion
    //#endregion

    //#region CONST EXAMEN MEDICO
    const tblRH_REC_ArchivosExamenMedico = $('#tblRH_REC_ArchivosExamenMedico');
    let dtExamenMedico;
    const btnMdlExamenMedico = $('#btnMdlExamenMedico');
    const txtCEExamenMedicoArchivo = $('#txtCEExamenMedicoArchivo');
    const txtCEExamenMedicoObservacion = $('#txtCEExamenMedicoObservacion');
    const btnCEExamenMedico = $('#btnCEExamenMedico');
    const mdlCrearEditarArchivoExamenMedico = $('#mdlCrearEditarArchivoExamenMedico');
    //#endregion

    //#region CONST MAQUINARIA
    const mdlCrearEditarArchivoMaquinaria = $('#mdlCrearEditarArchivoMaquinaria');
    const btnMdlMaquinaria = $('#btnMdlMaquinaria');
    const tblRH_REC_ArchivosMaquinaria = $('#tblRH_REC_ArchivosMaquinaria');
    const txtCEMaquinariaArchivo = $('#txtCEMaquinariaArchivo');
    const txtCEMaquinariaObservacion = $('#txtCEMaquinariaObservacion');
    const btnCEMaquinaria = $('#btnCEMaquinaria');
    //#endregion

    //#region CONST TAB CONTRATOS
    const tblContratos = $('#tblContratos');
    let dtContratos;
    //#endregion

    //#region CONST CARGA CONTRATOS FIRMADOS
    const mdlContratosFirmados = $('#mdlContratosFirmados');
    const tblContratosFirmados = $('#tblContratosFirmados');
    const btnCrearContratoFirmado = $('#btnCrearContratoFirmado');
    const mdlGuardarContratoFirmado = $('#mdlGuardarContratoFirmado');
    const btnGuardarContratoFirmado = $('#btnGuardarContratoFirmado');
    const inputArchivoExcel = $('#inputArchivoExcel');
    const txtCEContratoFirmadoDescripcion = $('#txtCEContratoFirmadoDescripcion');
    let dtContratosFirmados;
    //#endregion

    //#region CONST MDL COMENTARIO
    const mdlComentario = $('#mdlComentario');
    const txtMdlComentario = $('#txtMdlComentario');
    //#endregion

    //#region CONST MDL ADD CONTRATO
    const bntAddContrato = $('#bntAddContrato');
    const mdlAddContrato = $('#mdlAddContrato');
    const txtAddContratoFechaMod = $('#txtAddContratoFechaMod');
    const txtAddContratoFechaApli = $('#txtAddContratoFechaApli');
    const cboAddContratoTipoContrato = $('#cboAddContratoTipoContrato');
    const btnAddContratoGuardar = $('#btnAddContratoGuardar');
    const txtAddContratoFechaInicio = $('#txtAddContratoFechaInicio');
    const txtAddContratoFechaFin = $('#txtAddContratoFechaFin');
    //#endregion

    //#region CONST DOCUMENTOS
    const tblDocumentos = $('#tblDocumentos');
    const tabDocumentos = $('#tabDocumentos');
    let dtDocumentos = $('#dtDocumentos');
    //#endregion

    //#region CONST FOTO EMPLEADO
    const btnMdlFotoEmpleado = $('#btnMdlFotoEmpleado');
    const mdlVerFotoEmpleado = $('#mdlVerFotoEmpleado');
    const btnCEFotoEmpleado = $('#btnCEFotoEmpleado');
    const btnCargarFotoEmpleado = $('#btnCargarFotoEmpleado');
    const fileFotoEmpleado = $('#fileFotoEmpleado');
    const lblNombreFotoEmpleado = $('#lblNombreFotoEmpleado');
    const imgFotoEmpleado = $('#imgFotoEmpleado');
    const txtImageFotoEmpleadoBase64 = $('#txtImageFotoEmpleadoBase64');
    const imgFotoEmpleadoHead = $('#imgFotoEmpleadoHead');
    //#endregion

    //#region CONST HISOTRIAL CC
    let dtHistorial
    const mdlHistorial = $('#mdlHistorial');
    const tblHistorial = $('#tblHistorial');
    //#endregion

    //#region CONST HEADER

    const txtHeadEstatusA = $('#txtHeadEstatusA');
    const txtHeadCantA = $('#txtHeadCantA');
    const txtHeadEstatusB = $('#txtHeadEstatusB');
    const txtHeadCantB = $('#txtHeadCantB');
    const txtHeadEstatusP = $('#txtHeadEstatusP');
    const txtHeadCantP = $('#txtHeadCantP');
    const txtHeadEstatusC = $('#txtHeadEstatusC');
    const txtHeadCantC = $('#txtHeadCantC');

    //#endregion

    const botonVerRequisiciones = $('#botonVerRequisiciones');
    const modalRequisiciones = $('#modalRequisiciones');
    const tablaRequisiciones = $('#tablaRequisiciones');

    //#region DATOS EMPLEADO PERU
    const checkAfiliadoEps = $('#checkAfiliadoEps');
    const cboSituacionEps = $('#cboSituacionEps');
    const cboRucEps = $('#cboRucEps');
    const cboAfps = $('#cboAfps');
    const cboMensualDiario = $('#cboMensualDiario');
    const txtBasico = $('#txtBasico');
    const txtAsigFamiliar = $('#txtAsigFamiliar');
    const cboTipoTrabajador = $('#cboTipoTrabajador');
    const cboSituacion = $('#cboSituacion');
    const cboRegimenLaboral = $('#cboRegimenLaboral');
    const cboBancoCTS = $('#cboBancoCTS');
    //#endregion

    //#region CONST SUSTENTO HIJOS
    const mdlSustentosHijos = $("#mdlSustentosHijos");
    const txtCargarSustento = $("#txtCargarSustento");
    const tblSustentos = $("#tblSustentos");
    const btnCESustento = $("#btnCESustento");
    const initTblSustentos = $('#initTblSustentos');
    let dtSustentos;
    //#endregion

    let dtRequisiciones;
    let lstEstatus = [
        { val: 'A', text: 'ALTA' },
        { val: 'P', text: 'PENDIENTE' },
        { val: 'B', text: 'BAJA' },
    ]

    const _ESTATUS = {
        AUTORIZADA: 'A',
        PENDIENTE: 'P',
        CANCELADA: 'C'
    }

    let _clave_empleado = 0;

    let isReingresar = false;

    //TIPO DE VISTA EMPLEADOS (FILTRO)

    AltaEmpleados = function () {
        (function init() {
            if (txtEmpresa.val() == 6) {
                llenarFiltrosDatosEmpleadoPeru();
            }

            $.fn.dataTable.moment('DD/MM/YYYY');
            fncListeners();
            checarPermisoTabuladorLibre();
        })();

        //#region DATOS EMPLEADO PERU
        function llenarFiltrosDatosEmpleadoPeru() {
            cboSituacionEps.fillComboBox('GetSituacion', { afiliado: checkAfiliadoEps.prop('checked') }, '-- Seleccionar --', () => {
                cboSituacionEps.select2({ width: '100%', dropdownParent: $(mdlCrearEditarEmpleadoEK) });
            });

            cboRucEps.fillComboBox('GetRucEps', null, '-- Seleccionar -- ', () => {
                cboRucEps.select2({ width: '100%', dropdownParent: $(mdlCrearEditarEmpleadoEK) });
            });

            cboAfps.fillComboBox('GetAfps', null, '-- Seleccionar --', () => {
                cboAfps.select2({ width: '100%', dropdownParent: $(mdlCrearEditarEmpleadoEK) });
            });

            cboTipoTrabajador.fillComboBox('FillComboTiposTrabajadores', null, '-- Seleccionar --', () => {
                cboTipoTrabajador.select2({ width: '100%', dropdownParent: $(mdlCrearEditarEmpleadoEK) });
            });

            cboRegimenLaboral.fillComboBox('FillComboRegimenLaboralPeru', null, '-- Seleccionar --', () => {
                cboRegimenLaboral.select2({ width: '100%', dropdownParent: $(mdlCrearEditarEmpleadoEK) });
            });

            cboBancoCTS.fillComboBox('FillComboBancosPeru', null, 'NONE', () => {
                cboBancoCTS.select2({ width: '100%', dropdownParent: $(mdlCrearEditarEmpleadoEK) });
            });


        }
        //#endregion

        //#region FUNCIONES DE LOS PARAMETROS DEL URL
        function obtenerUrlPArams() {
            const variables = getUrlParams(window.location.href);
            var clean_uri = location.protocol + "//" + location.host + location.pathname;

            if (variables.cargarModal == 1) {
                mdlCrearEditarEmpleadoEK.modal("show");
                cboCandidatosAprobados.attr("disabled", true);
                divCandidatosAprobados.css("display", "initial");
                cboCandidatosAprobados.val(variables.candidatoID);
                cboCandidatosAprobados.change();
            }


            window.history.replaceState({}, document.title, clean_uri);

        }

        function getUrlParams(url) {
            let params = {};
            let parser = document.createElement('a');
            parser.href = url;
            let query = parser.search.substring(1);
            let vars = query.split('&');

            for (let i = 0; i < vars.length; i++) {
                let pair = vars[i].split('=');
                params[pair[0]] = decodeURIComponent(pair[1]);
            }

            return params;
        }
        //#endregion

        function obtenerCURPRFCSinCandidato() {
            if (cboCandidatosAprobados.val() == '') {
                if (txtCEDatosEmpleadoApePaterno.val() &&
                    txtCEDatosEmpleadoApeMaterno.val() &&
                    txtCEDatosEmpleadoNombre.val() &&
                    cboCEDatosEmpleadoPaisNac.val() &&
                    cboCEDatosEmpleadoEstadoNac.val() &&
                    txtCEDatosEmpleadoFechaNacimiento.val() &&
                    cboCEDatosEmpleadoSexo.val()) {
                    axios.post('GenerarCURP', {
                        nombres: txtCEDatosEmpleadoNombre.val(),
                        paterno: txtCEDatosEmpleadoApePaterno.val(),
                        materno: txtCEDatosEmpleadoApeMaterno.val(),
                        sexo: cboCEDatosEmpleadoSexo.val() == 'M' ? 'Hombre' : 'Mujer',
                        fechaNacimiento: txtCEDatosEmpleadoFechaNacimiento.val(),
                        estado: cboCEDatosEmpleadoEstadoNac.val()
                    }).then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            txtCEDatosEmpleadoCURP.val(response.data.curp)
                            txtCEDatosEmpleadoCURP.trigger("change");
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));

                    axios.post('GetRFC', {
                        objCandidato: {
                            nombre: txtCEDatosEmpleadoNombre.val(),
                            apePaterno: txtCEDatosEmpleadoApePaterno.val(),
                            apeMaterno: txtCEDatosEmpleadoApeMaterno.val(),
                            fechaNacimiento: txtCEDatosEmpleadoFechaNacimiento.val()
                        }
                    }).then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            // txtCEDatosEmpleadoRFC.val(response.data.rfc)
                            // txtCEDatosEmpleadoRFC.trigger("change");
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
                }
            }
        }

        function fncListeners() {
            //#region  SET COMBO POR VISTA
            if (empleadosAdmn.val() == 1) {
                $('#chkFiltroEsPendiente option[value="P"]').css("display", "none");
                chkFiltroEsPendiente.val("A");
                chkFiltroEsPendiente.trigger("change");

            } else {
                $('#chkFiltroEsPendiente option[value="C"]').css("display", "none");
                $('#chkFiltroEsPendiente option[value="A"]').css("display", "none");
                $('#chkFiltroEsPendiente option[value="B"]').css("display", "none");
                chkFiltroEsPendiente.val("P");
                chkFiltroEsPendiente.trigger("change");

            }
            //#endregion

            //#region INIT TABLAS
            initTblEmpleados();
            initTblFamiliares();
            initTblNominas();
            initTblUniformes();
            initTblContratos();
            initTblExamenMedico();
            initTblMaquinaria();
            initTblContratosFirmados();
            initTablaRequisiciones();
            initTblDocumentos();
            initTblHistorial();
            initTblSustentos();
            //#endregion
            fncGetHeaderEmpleados();

            txtCEDatosEmpleadoFechaIngreso.datepicker({ dateFormat: 'dd/mm/yy' })
                .datepicker('option', 'showAnim', 'slide')
                .datepicker('setDate', new Date())
                .datepicker("option", "maxDate", new Date(moment(txtCEDatosEmpleadoFechaIngreso.attr('max'))))
                .datepicker("option", "minDate", new Date(moment(txtCEDatosEmpleadoFechaIngreso.attr('min'))));

            txtCEDatosEmpleadoFechaNacimiento.datepicker({ dateFormat: 'dd/mm/yy' })

            // console.log(txtCEDatosEmpleadoFechaIngreso.attr('min'));
            // console.log(txtCEDatosEmpleadoFechaIngreso.attr('max'));
            txtTabuladorLibreFecha.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker('option', 'showAnim', 'slide').datepicker('setDate', new Date());

            motivoSueldo.fillComboBox('FillMotivoSueldo', null, '-- Seleccionar --', null);

            mdlCrearEditarEmpleadoEK.on('hidden.bs.modal', function () {
                fncLimpiarMdlCEEmpleado();
                mdlImprimirContratos.attr("data-claveEmpleado", null);
                lblCEDatosEmpleadoClaveEmpleado.data('clvempleado', 0);
                btnCEFamiliar.data("id", null);
                btnCEEmpleado.data("id-ek", null);
            });

            txtCEDatosEmpleadoApePaterno.on('change', function (event, noModificarRFCCURP) {
                if (!noModificarRFCCURP) {
                    obtenerCURPRFCSinCandidato();
                }
            });

            txtCEDatosEmpleadoApeMaterno.on('change', function (event, noModificarRFCCURP) {
                if (!noModificarRFCCURP) {
                    obtenerCURPRFCSinCandidato();
                }
            });

            txtCEDatosEmpleadoNombre.on('change', function (event, noModificarRFCCURP) {
                if (!noModificarRFCCURP) {
                    obtenerCURPRFCSinCandidato();
                }
            });

            // txtCEDatosEmpleadoLocalidadNac.on('change', function (event, noModificarRFCCURP) {
            //     if (!noModificarRFCCURP) {
            //         obtenerCURPRFCSinCandidato();
            //     }
            // });

            btnEnviarCorreos.on("click", function () {
                //#region SE OBTIENE LA CLAVE DEL EMPLEADO SELECCIONADO
                let strMensaje = "";
                let arrRowsChecked = [];
                let rowsChecked = $("#tblRH_REC_Empleados").DataTable().column(0).checkboxes.selected();
                $.each(rowsChecked, function (index, claveEmpleado) {
                    arrRowsChecked.push(claveEmpleado);
                    strMensaje == "" ? "¿Desea notificar a los empleados seleccionados?" : "¿Desea notificar al empleado seleccionado?";
                });
                //#endregion

                Alert2AccionConfirmar('¡Cuidado!', strMensaje, 'Confirmar', 'Cancelar', () => fncEnviarCorreos(arrRowsChecked));
            });

            //#region FILL COMBO ESTATUS FILTRO
            // chkFiltroEsPendiente.fillCombo("FillEstatusFiltro", { esAdmn }, false, null);
            // convertToMultiselect('#chkFiltroEsPendiente');
            //#region CODIGO PARA SELECCIONAR TODO EL COMBO
            // chkFiltroEsPendiente.multiselect('selectAll', false);
            // chkFiltroEsPendiente.multiselect('refresh');
            // chkFiltroEsPendiente.multiselect('deselect', 'TODOS');
            // chkFiltroEsPendiente.multiselect('refresh');
            //#endregion
            // $("#chkFiltroEsPendiente").multiselect('select', 'A');
            // $("#chkFiltroEsPendiente").multiselect('refresh');
            //#endregion

            $(`.comboChange`).select2();
            $(`.comboChange`).select2({ width: '100%' });
            // $(`#chkFiltroEsPendiente`).select2().change();
            // fncGetEmpleadosEK();
            // chkFiltroEsPendiente.change(function () {
            //     if ($(this).val() != "") {
            //         fncGetEmpleadosEK();
            //         var table = $('#tblRH_REC_Empleados').DataTable();
            //         if (chkFiltroEsPendiente.val() == 'B' || chkFiltroEsPendiente.val() == 'P') {
            //             table.column(7).visible(false);
            //         } else {
            //             table.column(7).visible(true);
            //         }
            //     }
            // });

            cboFiltroCC.on("change", function () {
                if ($(this).val() != "" && chkFiltroEsPendiente.val() > 0) {
                    fncGetEmpleadosEK();
                }
            });

            //#region FUNCIONES GENERALES
            fncEliminarAutoComplete();

            cboCEGenContactoEstadoCivil.on("change click", function () {
                fncFocus("cboCEGenContactoEstadoCivil");
            });

            cboCandidatosAprobados.on("change click", function () {
                if ($(this).val() != "") {
                    fncHabilitarDeshabilitarCtrlsMdl(false);
                    $("#select2-cboCandidatosAprobados-container").css('border', '1px solid #CCC');
                    txtCEDatosEmpleadoNombre.css('border', '1px solid #CCC');
                    txtCEDatosEmpleadoApePaterno.css('border', '1px solid #CCC');

                    let option = $(this).find(`option[value="${$(this).val()}"]`);
                    let prefijo = option.attr("data-prefijo");
                    let claveCandidato;
                    var lstNombreCompleto = prefijo.split("|");
                    let strNombre = "", strApePaterno = "", strApeMaterno = "";
                    for (let i = 0; i < lstNombreCompleto.length; i++) {
                        switch (i) {
                            case 0:
                                strApePaterno = lstNombreCompleto[i];
                                break;
                            case 1:
                                strApeMaterno = lstNombreCompleto[i];
                                break;
                            case 2:
                                strNombre = lstNombreCompleto[i];
                                break;
                            case 3:
                                claveCandidato = lstNombreCompleto[i];
                                break;
                            default:
                                break;
                        }
                    }
                    txtCEDatosEmpleadoNombre.val(strNombre);
                    txtCEDatosEmpleadoApePaterno.val(strApePaterno);
                    txtCEDatosEmpleadoApeMaterno.val(strApeMaterno);

                    if (claveCandidato > 0) {
                        fncGetDatosActualizarEmpleado(Number(claveCandidato), true);
                    } else {

                        fncGetDatosCandidatoAprobado($(this).val());
                    }
                    btnCEEmpleado.css("display", "initial"); //OCULTAR BOTON DE INGRESO

                } else {
                    fncHabilitarDeshabilitarCtrlsMdl(true);
                    fncBorderDefault();
                }
            });
            cboCEGenContactoTipoSangre.on("change", function () {
                if ($(this).val() != "") {
                    $("#select2-cboCEGenContactoTipoSangre-container").css('border', '1px solid #CCC');
                }
                else {
                    $("#select2-cboCEGenContactoTipoSangre-container").css('border', '2px solid red');
                }
                fncFocus("cboCEGenContactoTipoSangre");
            });

            cboTipoNomina.fillComboBox('CargarTiposNomina', null, null, null);
            cboBancoNomina.fillComboBox('CargarBancos', null, '-- Seleccionar --', null);
            cboBancoNomina.on("change", function () {
                fncFocus("cboBancoNomina");
            });

            cboCEGenContactoTipoCasa.on("change", function () {
                fncFocus("cboCEGenContactoTipoCasa");
            });
            //#endregion

            cboTipoNomina.on("change", function () {
                cboTipoNomina.css('border', '1px solid #CCC');
                fncFocus("cboTipoNomina");
            });

            //#region EVENTOS FILTROS
            btnAutorizacionMultiple.on('click', function () {
                let arrRowsSelected = [];
                let rowsChecked = $("#tblRH_REC_Empleados").DataTable().column(0).checkboxes.selected();
                $.each(rowsChecked, function (index, claveEmpleado) {
                    arrRowsSelected.push(claveEmpleado);
                });

                if (arrRowsSelected.length > 0) {
                    swal({
                        title: "Autorizar empleados",
                        text: "Se autorizaran los empleados seleccionados, ¿desea continuar?",
                        icon: "warning",
                        buttons: true,
                        dangerMode: true,
                    }).then((autorizar) => {
                        if (autorizar) {
                            autorizacionMultiple(arrRowsSelected);
                        }
                    });
                } else {
                    swal('Alerta!', 'Debe seleccionar al menos un empleado', 'warning');
                }
            });

            btnDiasIngresos.on('click', function () {
                txtDiasAnterioresPermitidos.val(0);
                txtDiasPosterioresPermitidos.val(0);

                $.get('/Reclutamientos/GetDiasDisponibles').then(response => {
                    if (response.success) {
                        txtDiasAnterioresPermitidos.val(response.items.anteriores);
                        txtDiasPosterioresPermitidos.val(response.items.posteriores);

                        mdlDiasIngreso.modal('show');
                    } else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal('Alerta!', 'Hubo un problema al comuncarse al servidor.', 'warning');
                });
            });

            btnActualizarDiasIngresos.on('click', function () {
                $.post('/Reclutamientos/SetDiasDisponibles',
                    {
                        anteriores: +txtDiasAnterioresPermitidos.val(),
                        posteriores: +txtDiasPosterioresPermitidos.val()
                    }).then(response => {
                        if (response.success) {
                            // txtCEDatosEmpleadoFechaIngreso.attr('min', response.items.anterior);
                            // txtCEDatosEmpleadoFechaIngreso.attr('max', response.items.posterior);

                            swal('Confirmación!', 'Se actualizaron los días', 'success');
                        } else {
                            swal('Alerta!', response.message, 'warning');
                        }
                    }, error => {
                        swal('Alerta!', 'Hubo un problema al comuncarse al servidor.', 'warning');
                    });
            });

            btnCrearEmpleado.on("click", function () {
                cboCandidatosAprobados.attr("disabled", false);
                divCandidatosAprobados.css("display", "initial");
                cboCandidatosAprobados.fillCombo("/Reclutamientos/FillCboCandidatosAprobados", {}, false);
                cboCandidatosAprobados.select2({ width: "75%" });
                lblTitleCrearEditarEmpleadoEK.html("Crear empleado");
                btnCEEmpleado.attr("data-id", 0);
                chkCEGenContactoAlergias.prop("checked", false);
                chkCEGenContactoAlergias.trigger("change");
                divCEGenContactoAlergias.hide();
                bntAddContrato.prop('disabled', true);
                selectTipoContrato.prop('disabled', false);
                dateCECompaniaContratoFin.prop('disabled', false);
                // cboTipoNomina.attr("disabled", false);

                fncLimpiarMdlCEEmpleado();
                fncBorderDefault();
                cboCEGenContactoEstadoCivil.val("--Seleccione--");
                cboCEGenContactoEstadoCivil.trigger("change");
                lblCEDatosEmpleadoClaveEmpleado.html("");
                lblCEDatosEmpleadoClaveEmpleado.data('clvempleado', 0);
                lblCEDatosEmpleadoEstatus.html("PENDIENTE");
                lblCEDatosEmpleadoEstatus.css("color", "blue");
                btnCEEmpleado.html("Guardar");
                mdlCrearEditarEmpleadoEK.modal("show");
                // .clear().draw();
                isReingresar = false;
                dtFamiliares.clear().draw();
                dtTabuladores.clear().draw();
                dtUniformes.clear().draw();
                dtExamenMedico.clear().draw();
                dtMaquinaria.clear().draw();
                dtContratosFirmados.clear().draw();
                dtContratos.clear().draw();
                dtDocumentos.clear().draw();
                fncCECompaniaFillDepartamentos(0, false, 0);
                fncGetIDUsuarioEK();
                btnCEEmpleado.attr("data-id", 0);
                btnCEEmpleado.attr("data-esActualizar", 0);
                txtAddContratoFechaMod.val(moment().format("YYYY-MM-DD"));
                txtAddContratoFechaApli.val(moment().format("YYYY-MM-DD"));
                btnCEFotoEmpleado.css("display", "none");

                if (txtEmpresa.val() == "6") {

                    dtFamiliares.column(6).visible(true); //COLUMNA DNI (PERU)
                    dtFamiliares.column(7).visible(false); //COLUMNA Cedula (COL)

                } else if (txtEmpresa.val() == "3") {

                    dtFamiliares.column(6).visible(false); //COLUMNA DNI (PERU)
                    dtFamiliares.column(7).visible(true); //COLUMNA Cedula (COL)
                }
                else {

                    dtFamiliares.column(6).visible(false); //COLUMNA DNI (PERU)
                    dtFamiliares.column(7).visible(false); //COLUMNA Cedula (COL)
                }

                //#region DATOS PERU

                checkAfiliadoEps.prop("checked", false); //afiliado_eps:
                checkAfiliadoEps.trigger("change");
                //#endregion

                btnCEEmpleado.css("display", "none"); //OCULTAR BOTON DE INGRESO
                botonVerRequisiciones.attr("disabled", false);
                txtCECompaniaRequisicion.attr("disabled", false);
                chkTabuladorLibre.attr("disabled", false);
            });

            btnFiltroExportar.on("click", function () {
                fncExportarPendientes();
            });
            //#endregion

            //#region EVENTOS DATOS EMPLEADO
            // fncGetEmpleadosEK();
            btnFiltroBuscar.on("click", function () {
                if (chkFiltroEsPendiente.val() == "P") {
                    btnAutorizacionMultiple.prop('disabled', false);
                } else {
                    btnAutorizacionMultiple.prop('disabled', true);
                }

                // tblRH_REC_Empleados.destroy();
                // tblRH_REC_Empleados.empty();

                //Delete the datable object first
                dtEmpleados.destroy(true);
                //Remove all the DOM elements
                divTblEmpleados.html('<table id="tblRH_REC_Empleados" class="table table-hover table-sm table-bordered compact" style="width:100%;"></table>');

                initTblEmpleados();
                fncGetEmpleadosEK();
                if (chkFiltroEsPendiente.val() != "") {
                    // fncGetEmpleadosEK();
                    let flag = (chkFiltroEsPendiente.val() == "B");
                    $('#tblRH_REC_Empleados').DataTable().column(10).visible(flag);
                }
            });

            btnCEEmpleado.on("click", function () {
                // $(this).attr("data-id", 0);
                // $(this).attr("data-esActualizar", 0);
                fncCrearEditarEmpleado();
            });

            cboFiltroCC.fillCombo("/Reclutamientos/FillCboCC", {}, false, 'Todos');
            // cboFiltroCC.select2({ width: "100%" });
            convertToMultiselect('#cboFiltroCC');

            cboCEDatosEmpleadoPaisNac.fillCombo("/Reclutamientos/FillCboPaises", {}, false);
            cboCEDatosEmpleadoEstadoNac.fillCombo("/Reclutamientos/FillCboEstados", { _clavePais: 0 }, false);
            cboCEDatosEmpleadoLugarNac.fillCombo("/Reclutamientos/FillCboMunicipios", { _clavePais: 0, _claveEstado: 0 }, false);
            cboCEDatosEmpleadoDepartamentoNac.fillCombo("/Reclutamientos/FillComboGeoDepartamentos", {}, false);
            cboCEGenContactoDepartamentoNac.fillCombo("/Reclutamientos/FillComboGeoDepartamentos", {}, false);
            cboCEBeneficiarioDepartamentoNac.fillCombo("/Reclutamientos/FillComboGeoDepartamentos", {}, false);

            cboCEDatosEmpleadoPaisNac.on("change", function (e, noModificarRFCCURP) {
                if ($(this).val() != "") {
                    cboCEDatosEmpleadoEstadoNac.fillCombo("/Reclutamientos/FillCboEstados", { _clavePais: parseFloat($(this).val()) }, false);
                    // cboCEBeneficiarioEstado.fillCombo("/Reclutamientos/FillCboEstados", { _clavePais: parseFloat($(this).val()) }, false);
                    // cboCEGenContactoEstado.fillCombo("/Reclutamientos/FillCboEstados", { _clavePais: parseFloat($(this).val()) }, false);

                    if ($(this).val() == 7 && txtEmpresa.val() == 6) {
                        divCeDatosEmpleadosDepartamentoNac.show();
                    } else {
                        divCeDatosEmpleadosDepartamentoNac.hide();

                    }

                    if (!noModificarRFCCURP) {
                        obtenerCURPRFCSinCandidato();
                    }
                }
                fncFocus("cboCEDatosEmpleadoPaisNac");
            });

            cboCEDatosEmpleadoDepartamentoNac.on("change", function () {
                if ($(this).val() != "" && txtEmpresa.val() == 6) {
                    cboCEDatosEmpleadoEstadoNac.fillCombo("/Reclutamientos/FillCboEstadosPERU", { claveDepartamento: parseFloat($(this).val()) }, false);

                }
            });

            cboCEGenContactoDepartamentoNac.on("change", function (e, noFillEstados) {
                if ($(this).val() != "" && txtEmpresa.val() == 6) {
                    if (!noFillEstados) {
                        cboCEGenContactoEstado.fillCombo("/Reclutamientos/FillCboEstadosPERU", { claveDepartamento: parseFloat($(this).val()) }, false);
                    }
                }
            });

            cboCEBeneficiarioDepartamentoNac.on("change", function (e, noFillEstados) {
                if ($(this).val() != "" && txtEmpresa.val() == 6) {
                    if (!noFillEstados) {
                        cboCEBeneficiarioEstado.fillCombo("/Reclutamientos/FillCboEstadosPERU", { claveDepartamento: parseFloat($(this).val()) }, false);
                    }
                }
            });


            cboCEDatosEmpleadoEstadoNac.on("change", function (event, noModificarRFCCURP) {
                if ($(this).val() != "") {
                    cboCEDatosEmpleadoLugarNac.fillCombo("/Reclutamientos/FillCboMunicipios", { _clavePais: cboCEDatosEmpleadoPaisNac.val(), _claveEstado: $(this).val() }, false);
                    if (!noModificarRFCCURP) {
                        obtenerCURPRFCSinCandidato();
                    }
                }
                fncFocus("cboCEDatosEmpleadoEstadoNac");
            });

            cboCEBeneficiarioEstado.on("change", function () {

                if ($(this).val() != "") {
                    cboCEBeneficiarioCiudad.fillCombo("/Reclutamientos/FillCboMunicipios", { _clavePais: cboCEBeneficiarioEmpleadoPais.val(), _claveEstado: $(this).val() }, false);
                } else {
                    $("#select2-cboCEBeneficiarioEstado-container").css('border', '1px solid #CCC');
                }
            });

            cboCEBeneficiarioCiudad.on("change", function () {

                if ($(this).val() == "") {
                    $("#select2-cboCEBeneficiarioEstado-container").css('border', '1px solid #CCC');
                }
            });

            cboCEDatosEmpleadoPaisNac.select2({ width: "100%" });
            cboCEDatosEmpleadoEstadoNac.select2({ width: "100%" });
            cboCEDatosEmpleadoLugarNac.select2({ width: "100%" });
            cboCEDatosEmpleadoSexo.select2({ width: "100%" });

            cboCandidatosAprobados.fillCombo("/Reclutamientos/FillCboCandidatosAprobados", {}, false);
            cboCandidatosAprobados.select2({ width: "50%" });

            txtCEDatosEmpleadoNombre.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
            });
            txtCEDatosEmpleadoApePaterno.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
            });
            cboCEDatosEmpleadoPaisNac.on("change", function () {
                $("#select2-cboCEDatosEmpleadoPaisNac-container").css('border', '1px solid #CCC');
            });
            cboCEDatosEmpleadoEstadoNac.on("change", function () {
                $("#select2-cboCEDatosEmpleadoEstadoNac-container").css('border', '1px solid #CCC');
            });
            cboCEDatosEmpleadoLugarNac.on("change", function (event, noModificarRFCCURP) {
                $("#select2-cboCEDatosEmpleadoLugarNac-container").css('border', '1px solid #CCC');
                if (!noModificarRFCCURP) {
                    obtenerCURPRFCSinCandidato();
                }
                fncFocus("cboCEDatosEmpleadoLugarNac");
            });
            txtCEDatosEmpleadoFechaNacimiento.on("change", function (event, noModificarRFCCURP) {
                if ($(this).val() == "") {
                    $(this).css('border', '2px solid red');
                } else {
                    $(this).css('border', '1px solid #CCC');
                    if (!noModificarRFCCURP) {
                        obtenerCURPRFCSinCandidato();
                    }
                }
            });
            txtCEDatosEmpleadoFechaIngreso.on("change", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
                // if ($(this).val() > moment().format("YYYY-MM-DD")) {
                //     $(this).val("");
                //     Alert2Warning("Ingrese una Fecha de Ingreso valida.");
                //     if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }

                // }
            });
            txtCEDatosEmpleadoLocalidadNac.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
            });
            txtCEDatosEmpleadoRFC.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
                if ($(this).val().length < 13) { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
            });

            txtCEDatosEmpleadoCPCIF.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
            });

            txtCEDatosEmpleadoCURP.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
            });
            txtCEGenContactoEmail.on("keyup", function () {
                if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }
            });
            cboCEDatosEmpleadoSexo.on("change", function (event, noModificarRFCCURP) {
                $("#select2-cboCEDatosEmpleadoSexo-container").css('border', '1px solid #CCC');
                if (!noModificarRFCCURP) {
                    obtenerCURPRFCSinCandidato();
                }
                fncFocus("cboCEDatosEmpleadoSexo");
            });

            btnImprimirContratos.on("click", function () {
                fncImprimirDocumentos();
            });

            btnImprimirCredencial.on("click", function () {
                fncImprimirCredencial();
            });

            txtCEDatosEmpleadoRFC.on("change", function () {

                if (lblCEDatosEmpleadoClaveEmpleado.text() == "") {
                    fncCheckEmpleado();
                }
            });

            txtCEDatosEmpleadoCURP.on("change", function () {

                if (lblCEDatosEmpleadoClaveEmpleado.text() == "") {
                    fncCheckEmpleado();
                }
            });

            txtCECompaniaNSS.on("change", function () {
                if (lblCEDatosEmpleadoClaveEmpleado.text() == "") {
                    fncCheckEmpleado();
                }
            });
            //#endregion

            //#region EVENTOS GENERALES Y CONTACTO
            cboCEGenContactoEstadoCivil.append($("<option />").val("--Seleccione--").text("--Seleccione--"));
            cboCEGenContactoEstadoCivil.append($("<option />").val("Casado").text("Casado"));
            cboCEGenContactoEstadoCivil.append($("<option />").val("Divorciado").text("Divorciado"));
            cboCEGenContactoEstadoCivil.append($("<option />").val("Soltero").text("Soltero"));
            if (txtEmpresa.val() == 6) {
                cboCEGenContactoEstadoCivil.append($("<option />").val("Union Libre").text("Conviviente"));
            } else {
                cboCEGenContactoEstadoCivil.append($("<option />").val("Union Libre").text("Unión libre"));
            }
            cboCEGenContactoEstadoCivil.append($("<option />").val("Viudo").text("Viudo"));
            cboCEGenContactoEstadoCivil.select2({ width: "100%" });

            cboCEGenContactoEstado.select2({ width: "100%" });
            cboCEGenContactoEstado.fillCombo("/Reclutamientos/FillCboEstados", { _clavePais: 0 }, false);
            cboCEGenContactoCiudad.select2({ width: "100%" });
            cboCEGenContactoCiudad.fillCombo("/Reclutamientos/FillCboMunicipios", { _clavePais: 0, _claveEstado: 0 }, false);

            // cboCEDatosEmpleadoPaisNac.on("change", function () {
            //     if ($(this).val() != "") {
            //         cboCEGenContactoEstado.select2({ width: "100%" });
            //         cboCEGenContactoEstado.fillCombo("/Reclutamientos/FillCboEstados", { _clavePais: parseFloat($(this).val()) }, false);
            //     }
            // });
            cboCEGenContactoEmpleadoPais.fillCombo("/Reclutamientos/FillCboPaises", {}, false);
            cboCEGenContactoEmpleadoPais.select2({ width: "100%" });

            cboCEBeneficiarioEmpleadoPais.fillCombo("/Reclutamientos/FillCboPaises", {}, false);
            cboCEBeneficiarioEmpleadoPais.select2({ width: "100%" });

            cboCEGenContactoEmpleadoPais.on("change", function () {
                if ($(this).val() != "") {
                    cboCEGenContactoEstado.select2({ width: "100%" });
                    cboCEGenContactoEstado.fillCombo("/Reclutamientos/FillCboEstados", { _clavePais: parseFloat($(this).val()) }, false);
                }
                fncFocus("cboCEGenContactoEmpleadoPais");
            });

            cboCEGenContactoEstado.on("change", function () {
                if ($(this).val() != "") {
                    cboCEGenContactoCiudad.select2({ width: "100%" });
                    cboCEGenContactoCiudad.fillCombo("/Reclutamientos/FillCboMunicipios", { _clavePais: cboCEGenContactoEmpleadoPais.val(), _claveEstado: $(this).val() }, false);
                }
                fncFocus("cboCEGenContactoEstado");
            });

            txtCEGenContactoCP.on("change", function () {
                txtCEBeneficiarioCP.val(txtCEGenContactoCP.val() ?? "");
            });

            txtCEGenContactoColonia.on("change", function () {
                txtCEBeneficiarioColonia.val(txtCEGenContactoColonia.val() ?? "");
            });

            txtCEGenContactoCalle.on("change", function () {
                txtCEBeneficiarioDomicilio.val(txtCEGenContactoCalle.val() ?? "");
            });

            txtCEGenContactoNumExterior.on("change", function () {
                txtCEBeneficiarioNumExt.val(txtCEGenContactoNumExterior.val() ?? "");
            });

            txtCEGenContactoNumInterior.on("change", function () {
                txtCEBeneficiarioNumInt.val(txtCEGenContactoNumInterior.val() ?? "");
            });

            cboCEGenContactoTipoSangre.select2({ width: "100%" });
            cboCEGenContactoTipoSangre.fillCombo("/Reclutamientos/FillCboTipoSangre", {}, false);

            cboCEGenContactoTipoCasa.select2({ width: "100%" });
            cboCEGenContactoTipoCasa.fillCombo("/Reclutamientos/FillCboTipoCasa", {}, false);

            txtCEGenContactoCalle.on("keyup", function () {
                if ($(this).val() != "") { $(this).css('border', '1px solid #CCC'); } else { $(this).css('border', '2px solid red'); }
            });
            txtCEGenContactoNumExterior.on("keyup", function () {
                if ($(this).val() != "") { $(this).css('border', '1px solid #CCC'); } else { $(this).css('border', '2px solid red'); }
            });
            // txtCEGenContactoNumInterior.on("keyup", function () {
            //     if ($(this).val() != "") { $(this).css('border', '1px solid #CCC'); } else { $(this).css('border', '2px solid red'); }
            // });
            txtCEGenContactoColonia.on("keyup", function () {
                if ($(this).val() != "") { $(this).css('border', '1px solid #CCC'); } else { $(this).css('border', '2px solid red'); }
            });
            cboCEGenContactoEstado.on("change", function () {
                if ($(this).val() != "") {
                    $("#select2-cboCEGenContactoEstado-container").css('border', '1px solid #CCC');
                } else {
                    $("#select2-cboCEGenContactoEstado-container").css('border', '2px solid red');
                }
            });
            cboCEGenContactoCiudad.on("change", function () {
                if ($(this).val() != "") {
                    $("#select2-cboCEGenContactoCiudad-container").css('border', '1px solid #CCC');
                } else {
                    $("#select2-cboCEGenContactoCiudad-container").css('border', '2px solid red');
                }
                fncFocus("cboCEGenContactoCiudad");
            });
            txtCEGenContactoCP.on("keyup", function () {
                if ($(this).val() != "") { $(this).css('border', '1px solid #CCC'); } else { $(this).css('border', '2px solid red'); }
            });
            txtCEGenContactoTelCasa.on("keyup, keydown", function (event) {
                if ($(this).val() != "") { $(this).css('border', '1px solid #CCC'); } else { $(this).css('border', '2px solid red'); }
                aceptaSoloNumero0D($(this), event);
            });

            chkCEGenContactoAlergias.on("change", function () {
                if ($(this).prop("checked")) {
                    divCEGenContactoAlergias.show();
                } else {
                    divCEGenContactoAlergias.hide();
                }
            });

            //#endregion

            //#region EVENTOS FAMILIARES
            btnMdlFamiliar.on("click", function () {
                lblTitleCEFamiliares.html("Crear familiar");
                lblBtnCEFamiliar.html("Guardar");
                btnCEFamiliar.attr("data-esActualizar", 0);
                btnCEFamiliar.html("Guardar");
                btnCEFamiliar.data("id", 0);
                btnCEFamiliar.data('esNuevo', 1);
                fncLimpiarMdlCEFamiliares();
                mdlCrearEditarFamiliar.modal("show");
            });

            btnCEFamiliar.on("click", function () {
                fncCrearEditarFamiliar();
            });

            cboCEFamiliarParentesco.select2({ width: "100%" });
            cboCEFamiliarParentesco.fillCombo("/Reclutamientos/FillCboParentesco", {}, false);

            cboCEFamiliarParentesco.on("change", function () {
                // cboCEFamiliarGenero
                switch ($(this).val()) {
                    case "1":
                        cboCEFamiliarGenero.val("M");
                        cboCEFamiliarGenero.trigger("change");
                        break;

                    case "2":
                        cboCEFamiliarGenero.val("F");
                        cboCEFamiliarGenero.trigger("change");
                        break;
                    case "3":

                        break;
                    case "4":

                        break;
                    case "5":

                        break;
                    case "11":

                        break;

                    default:
                        break;
                }
            })

            cboCEFamiliarGenero.select2({ width: "100%" });
            cboCEFamiliarEstadoCivil.select2({ width: "100%" });
            //#endregion

            //#region EVENTOS BENEFICIARIOS
            cboCEBeneficiarioEmpleadoPais.on("change", function () {
                if ($(this).val() != "") {
                    cboCEBeneficiarioEstado.fillCombo("/Reclutamientos/FillCboEstados", { _clavePais: parseFloat($(this).val()) }, false);
                    fncFocus("cboCEBeneficiarioEmpleadoPais");
                }
            });

            cboCEBeneficiarioParentesco.select2({ width: "100%" });
            cboCEBeneficiarioParentesco.fillCombo("/Reclutamientos/FillCboParentesco", {}, false);
            cboCEBeneficiarioParentesco.on("change click", function () {
                fncFocus("txtCEBeneficiarioFechaNacimiento");
            });

            cboCEBeneficiarioEstado.fillCombo("/Reclutamientos/FillCboEstados", { _clavePais: 0 }, false);
            cboCEBeneficiarioEstado.select2({ width: "100%" });
            cboCEBeneficiarioEstado.on("change", function () {
                fncFocus("cboCEBeneficiarioEstado");
            });

            cboCEBeneficiarioCiudad.fillCombo("/Reclutamientos/FillCboMunicipios", { _clavePais: 0, _claveEstado: 0 }, false);
            cboCEBeneficiarioCiudad.select2({ width: "100%" });
            cboCEBeneficiarioCiudad.on("change", function () {
                fncFocus("cboCEBeneficiarioCiudad");
            })

            btnMdlCopyBeneficiario.on("click", function () {
                txtCECasoAccidenteNombre.val((txtCEBeneficiarioNombre.val() ?? "") + " " + (txtCEBeneficiarioApePaterno.val() ?? "") + " " + (txtCEBeneficiarioApeMaterno.val() ?? ""));
                txtCECasoAccidenteDomicilio.val((txtCEBeneficiarioDomicilio.val() ?? "") + `${txtCEBeneficiarioNumExt.val() != "" ? " Num. Ext/Int. " : ""}` + (txtCEBeneficiarioNumExt.val() ?? "") + `${txtCEBeneficiarioNumInt.val() != "" ? "/" : ""}` + (txtCEBeneficiarioNumInt.val() ?? " ") + `${txtCEBeneficiarioColonia.val() != "" ? "  Colonia. " : ""}` + (txtCEBeneficiarioColonia.val() ?? ""));
            });
            //#endregion

            //#region EVENTOS COMPAÑIA
            // cboCECompaniaTipoFormula.select2({ width: "100%" });
            // cboCECompaniaTipoFormula.fillCombo("/Reclutamientos/FillCboTipoFormulaIMSS", {}, false);

            txtCECompaniaRequisicion.on("keyup", function () {
                if ($(this).val() != "") { $(this).css('border', '1px solid #CCC'); } else { $(this).css('border', '2px solid red'); }
            });
            txtCECompaniaRequisicion.on("change", function () {
                cargarInformacionRequisicion();
                if ($(this).val() != "") { $(this).css('border', '1px solid #CCC'); } else { $(this).css('border', '2px solid red'); }
            });
            botonVerRequisiciones.on('click', function () {
                cargarRequisiciones();
            });
            txtCECompaniaAutoriza.on("keyup", function () {
                if ($(this).val() != "") { $(this).css('border', '1px solid #CCC'); } else { $(this).css('border', '2px solid red'); }
            });
            txtCECompaniaUsuarioResg.on("keyup", function () {
                if ($(this).val() != "") { $(this).css('border', '1px solid #CCC'); } else { $(this).css('border', '2px solid red'); }
            });
            txtCECompaniaNSS.on("keyup", function () {
                if ($(this).val() != "") { $(this).css('border', '1px solid #CCC'); } else { $(this).css('border', '2px solid red'); }
            });
            // cboCECompaniaTipoFormula.on("change", function () {
            //     if ($(this).val() != "") {
            //         $("#select2-cboCECompaniaTipoFormula-container").css('border', '1px solid #CCC');
            //     } else {
            //         $("#select2-cboCECompaniaTipoFormula-container").css('border', '2px solid red');
            //     }
            // });
            txtCECompaniaAutoriza.on("change", function () {
                let autoriza = +txtCECompaniaAutoriza.val();

                if (autoriza > 0 && !isNaN(autoriza)) {
                    cargarAutoriza(autoriza);
                } else {
                    txtCECompaniaAutorizaDescripcion.val('');
                }
            });
            txtCECompaniaUsuarioResg.on("change", function () {
                let usuarioResg = +txtCECompaniaUsuarioResg.val();

                if (usuarioResg > 0 && !isNaN(usuarioResg)) {
                    cargarUsuarioResg(usuarioResg);
                } else {
                    txtCECompaniaUsuarioResgDescripcion.val('');
                }
            });
            txtCECompaniaDepto.on("change", function () {
                if ($(this).val() != "") { $(this).css('border', '1px solid #CCC'); } else { $(this).css('border', '2px solid red'); }

                let depto = +txtCECompaniaDepto.val();
                if (depto > 0 && !isNaN(depto)) {
                    cargarDepto(depto);
                } else {
                    txtCECompaniaDeptoDescripcion.val('');
                }

                fncFocus("txtCECompaniaDepto");
            });
            txtCECompaniaContrato.on("change", function () {
                if ($(this).val() > moment().format("YYYY-MM-DD")) {
                    $(this).val("");
                    Alert2Warning("Ingrese una Fecha de Ingreso valida.");
                    if ($(this).val() == "") { $(this).css('border', '2px solid red'); } else { $(this).css('border', '1px solid #CCC'); }

                } else {
                    txtCECompaniaContrato.css('border', '1px solid #CCC');

                }
            });
            txtCECompaniaPlantilla.on("keyup", function () {
                if ($(this).val() != "") { $(this).css('border', '1px solid #CCC'); } else { $(this).css('border', '2px solid red'); }
            });
            selectRegistroPatronal.on("change", function () {
                if ($(this).val() != "") { $(this).css('border', '1px solid #CCC'); } else { $(this).css('border', '2px solid red'); }
                fncFocus("selectRegistroPatronal");
            });
            selectTipoContrato.on("change", function () {
                fncFocus("selectTipoContrato");
            });
            //#endregion

            //#region EVENTOS NOMINA
            cboCETabuladorTipoNomina.select2({ width: "100%" });

            cboCETabuladorBanco.fillCombo("/Reclutamientos/FillCboBancos", {}, false);
            cboCETabuladorBanco.select2({ width: "100%" });

            btnShowMdlNomina.on("click", function () {
                mdlCrearEditarTabuladores.modal("show");
            });

            chkCETabuladorLibre.on("change", function () {
                let tabulador = txtCETabuladorNumTabulador.val();

                if (tabulador != '' && !isNaN(tabulador)) {
                    if ($(this).prop("checked")) {
                        mdlCrearEditarNominaSalario.modal("show");
                    }
                } else {
                    if ($(this).prop("checked")) {
                        $('#divTabulador .toggle-group').click();
                        Alert2Warning('Debe especificar el tabulador.');
                    }
                }
            });

            btnCETabulador.on("click", function () {
                fncCrearTabulador();
            });

            btnCENominaSalario.on("click", function () {
                fncCrearTabuladorPuesto();
            });

            txtCENominaSalarioSalarioBase.on("keyup", function () {
                fncCalcularTotalSalario();
            });

            txtCENominaSalarioComplemento.on("keyup", function () {
                fncCalcularTotalSalario();
            });

            txtCENominaSalarioBonoZona.on("keyup", function () {
                fncCalcularTotalSalario();
            });

            chkTarjetaNomina.on('change', function () {
                if ($(this).prop('checked')) {
                    cboBancoNomina.prop('disabled', false);
                    txtTarjetaNomina.prop('disabled', false);
                    txtClaveInterbancaria.prop('disabled', false);
                } else {
                    cboBancoNomina.prop('disabled', true);
                    txtTarjetaNomina.prop('disabled', true);
                    txtClaveInterbancaria.prop('disabled', true);

                    cboBancoNomina.val('');
                    txtTarjetaNomina.val('');
                    txtClaveInterbancaria.val('');
                }
            });

            chkTabuladorLibre.on('change', function () {
                if ($(this).prop('checked')) {
                    txtNumeroTabulador.val(1);

                    let fechaTabLibre = new Date();
                    let diaTabLibre = `${(fechaTabLibre.getDate())}`.padStart(2, '0');
                    let mesTabLibre = `${(fechaTabLibre.getMonth() + 1)}`.padStart(2, '0');
                    let yearTabLibre = fechaTabLibre.getFullYear();

                    // txtTabuladorLibreFecha.val(new Date());
                    txtTabuladorLibreFecha.val(`${diaTabLibre}/${mesTabLibre}/${yearTabLibre}`);
                    // txtTabuladorLibreFecha.val(moment(txtCEDatosEmpleadoFechaIngreso.val()).format('DD/MM/YYYY'));
                    txtTabuladorLibreSalarioBase.val(0);
                    txtTabuladorLibreComplemento.val(0);
                    txtTabuladorLibreBono.val(0);
                    txtTabuladorLibreTotal.val(0);

                    mdlTabuladorLibre.modal('show');
                } else {

                }
            });

            txtTabuladorLibreSalarioBase.on('change', function () {
                let total = 0;
                let salarioBase = $(this).val();
                let complemento = txtTabuladorLibreComplemento.val();
                let bono = txtTabuladorLibreBono.val();
                total = +salarioBase + +complemento + +bono;
                txtTabuladorLibreTotal.val(total.toFixed(2));
            });

            txtTabuladorLibreComplemento.on('change', function () {
                let total = 0;
                let salarioBase = txtTabuladorLibreSalarioBase.val();
                let complemento = $(this).val();
                let bono = txtTabuladorLibreBono.val();
                total = +salarioBase + +complemento + +bono;
                txtTabuladorLibreTotal.val(total.toFixed(2));
            });

            txtTabuladorLibreBono.on('change', function () {
                let total = 0;
                let salarioBase = txtTabuladorLibreSalarioBase.val();
                let complemento = txtTabuladorLibreComplemento.val();
                let bono = $(this).val();
                total = +salarioBase + +complemento + +bono;
                txtTabuladorLibreTotal.val(total.toFixed(2));
            });

            btnAgregarTabuladorLibre.on('click', function () {
                tblRH_REC_Tabuladores.DataTable().rows().every(function (rowIdx, tableLoop, rowLoop) {
                    this.data().esNuevoTabulador = false;
                });

                let listaTabulador = new Array();
                // let objTabulador = {
                //     fecha_cambio: txtTabuladorLibreFecha.val(),
                //     fechaAplicaCambio: txtTabuladorLibreFecha.val(),
                //     fechaRealNomina: txtTabuladorLibreFecha.val(),
                //     salario_base: txtTabuladorLibreSalarioBase.val(),
                //     complemento: txtTabuladorLibreComplemento.val(),
                //     bono_de_zona: txtTabuladorLibreBono.val(),
                //     suma: txtTabuladorLibreTotal.val(),
                //     esNuevoTabulador: true,
                //     esTabuladorLibre: true
                // }

                let motivo_cambio = 0;
                if (btnCEEmpleado.attr("data-esReingresoEmpleado") == 'true') {
                    motivo_cambio = 3;
                }
                if (lblCEDatosEmpleadoClaveEmpleado.text() == '') {
                    motivo_cambio = 1
                }
                //lblCEDatosEmpleadoClaveEmpleado == ''

                let objTabulador = {
                    motivoCambio: motivo_cambio,
                    fecha_cambio: txtTabuladorLibreFecha.val(),
                    fechaAplicaCambio: txtTabuladorLibreFecha.val(),
                    fechaRealNomina: txtTabuladorLibreFecha.val(),
                    salario_base: txtTabuladorLibreSalarioBase.val(),
                    complemento: txtTabuladorLibreComplemento.val(),
                    bono_de_zona: txtTabuladorLibreBono.val(),
                    suma: txtTabuladorLibreTotal.val(),
                    esNuevoTabulador: true,
                    esTabuladorLibre: true
                }


                listaTabulador.push(objTabulador);
                addRowsSinLimpiar(tblRH_REC_Tabuladores, listaTabulador);
                mdlTabuladorLibre.modal('hide');
            });
            //#endregion

            //#region EVENTOS UNIFORMES
            btnMdlUniforme.on("click", function () {
                lblTitleCEUniforme.html("Guardar uniforme");
                btnCEUniforme.html("Guardar");
                btnCEUniforme.attr("data-id", 0);
                fncLimpiarMdlCEUniforme();
                mdlCrearEditarUniforme.modal("show");
            });

            btnCEUniforme.on("click", function () {
                fncCEUniforme();
            });
            //#endregion

            //#region EVENTOS EXAMEN MEDICO
            btnMdlExamenMedico.on("click", function () {
                txtCEExamenMedicoArchivo.val("");
                txtCEExamenMedicoObservacion.val("");
                mdlCrearEditarArchivoExamenMedico.modal("show");
            });

            btnCEExamenMedico.on("click", function () {
                fncCrearArchivoExamenMedico();
            });
            //#endregion

            //#region EVENTOS MAQUINARIA
            btnMdlMaquinaria.on("click", function () {
                txtCEMaquinariaArchivo.val("");
                txtCEMaquinariaObservacion.val("");
                mdlCrearEditarArchivoMaquinaria.modal("show");
            });

            btnCEMaquinaria.on("click", function () {
                fncCrearArchivoMaquinaria();
            });
            //#endregion

            //#region EVENTOS CONTRATOS FIRMADOS
            btnCrearContratoFirmado.on("click", function () {

            });
            //#endregion

            //#region PERU EVENTS

            checkAfiliadoEps.on("change", function (event, noCargar) {
                if (!noCargar) {
                    cboSituacionEps.fillComboBox('GetSituacion', { afiliado: checkAfiliadoEps.prop('checked') }, '-- Seleccionar --', () => {
                        cboSituacionEps.select2({ width: '100%', dropdownParent: $(mdlCrearEditarEmpleadoEK) });
                    });
                }
            });


            //#endregion

            //TO_DO: PRECARGAR CANDIDATO PARA DAR DE ALTA COMO EMPLEADO, QUE SE MANDO DESDE EL SEGUIMIENTO DEL CANDIDATO
            chkTipoDocumento8.prop("checked", false);
            chkTipoDocumento9.prop("checked", false);
            obtenerUrlPArams();

            txtCECompaniaCCDescripcion.on('change', function () {
                let cc = txtCECompaniaCCDescripcion.data('cc');
                selectRegistroPatronal.fillCombo('FillComboRegistroPatronal', { cc }, false, null);
            });

            selectRegistroPatronal.fillCombo('FillComboRegistroPatronal', { cc: '' }, false, null);
            selectTipoContrato.fillCombo('FillComboDuracionContrato', null, false, null);

            selectRegistroPatronal.select2({ width: "100%" });
            selectTipoContrato.select2({ width: "100%" });

            bntAddContrato.on("click", function () {
                //txtAddContratoFechaMod.val("");
                //txtAddContratoFechaApli.val("");

                // txtAddContratoFechaApli.val(moment().format("YYYY-MM-DD"));
                // txtAddContratoFechaMod.val(moment().format("YYYY-MM-DD"));

                // cboAddContratoTipoContrato[0].selectedIndex = 0;
                // cboAddContratoTipoContrato.trigger("change");

                mdlAddContrato.modal("show");
            });

            btnAddContratoGuardar.on("click", function () {
                if (cboAddContratoTipoContrato.val() == "") {
                    Alert2Warning("Elija una duracion de contrato");
                    return "";
                } else {
                    //fncAddContratos();

                    let lstContratos = new Array();
                    let objContrato = {
                        id_contrato_empleado: 0,
                        clave_empleado: 0,
                        clave_duracion: cboAddContratoTipoContrato.val(),
                        fecha: txtAddContratoFechaMod.val(),
                        fecha_aplicacion: txtAddContratoFechaApli.val(),
                        fechaString: moment(txtAddContratoFechaMod.val()).format("DD/MM/YYYY"),
                        fecha_aplicacionString: moment(txtAddContratoFechaApli.val()).format("DD/MM/YYYY"),
                        fecha_fin: moment(txtAddContratoFechaFin.val()),
                        desc_duracion: $('select[id="cboAddContratoTipoContrato"] option:selected').text(),
                        esNuevoContrato: true
                    }
                    lstContratos.push(objContrato);
                    addRowsSinLimpiar(tblContratos, lstContratos);
                    //mdlTabuladorLibre.modal('hide');

                }
            });

            cboAddContratoTipoContrato.fillCombo('FillComboDuracionContrato', null, false, null);

            // cboAddContratoTipoContrato.on("change", function () {
            //     if (cboAddContratoTipoContrato.val() != "") {
            //         let fechaFin = null;
            //         // let fechaFin = moment(txtAddContratoFechaMod.val());
            //         // console.log(moment(txtAddContratoFechaMod.val()));

            //         switch (Number($(this).val())) {
            //             case 2:
            //                 fechaFin = moment(txtAddContratoFechaApli.val()).add(3, 'M');

            //                 break;
            //             case 3:
            //                 fechaFin = null;

            //                 break;
            //             case 4:
            //                 fechaFin = null;

            //                 break;
            //             case 5:
            //                 fechaFin = moment(txtAddContratoFechaApli).add(28, 'd')//AddDays(28);

            //                 break;
            //             case 7:
            //                 fechaFin = moment(txtAddContratoFechaApli).add(6, 'M')//AddMonths(6);

            //                 break;
            //             case 8:
            //                 fechaFin = moment(txtAddContratoFechaApli).add(13, 'd')//AddDays(13);

            //                 break;
            //             case 9:
            //                 fechaFin = moment(txtAddContratoFechaApli).add(1, 'M')//AddMonths(1);

            //                 break;
            //             case 10:
            //                 fechaFin = moment(txtAddContratoFechaApli).add(15, 'd')//AddDays(15);

            //                 break;
            //             case 11:
            //                 fechaFin = moment(txtAddContratoFechaApli).add(16, 'd')//AddDays(16);

            //                 break;
            //             case 12:
            //                 fechaFin = moment(txtAddContratoFechaApli).add(9, 'd')//AddDays(9);

            //                 break;
            //             case 13:
            //                 fechaFin = moment(txtAddContratoFechaApli).add(25, 'd')//AddDays(25);

            //                 break;
            //             case 14:
            //                 fechaFin = moment(txtAddContratoFechaApli).add(4, 'd')//AddDays(4);

            //                 break;
            //             case 16:
            //                 fechaFin = moment(txtAddContratoFechaApli).add(5, 'd')//AddDays(5);

            //                 break;
            //             case 18:
            //                 fechaFin = moment(txtAddContratoFechaApli).add(2, 'M')//AddMonths(2);

            //                 break;
            //             case 19:
            //                 fechaFin = moment(txtAddContratoFechaApli).add(3, 'y')//AddYears(3);

            //                 break;

            //             default:
            //                 break;
            //         }
            //         if (fechaFin != null) {
            //             txtAddContratoFechaMod.val(fechaFin.format("YYYY-MM-DD"));
            //         } else {
            //             txtAddContratoFechaMod.val("");

            //         }
            //     }

            // });

            //#region EVENTOS FOTO EMPLEADO
            btnMdlFotoEmpleado.on("click", function () {
                fncGetFotoEmpleado();
                mdlVerFotoEmpleado.modal("show");

            });

            btnCargarFotoEmpleado.on("click", function () {
                fileFotoEmpleado.trigger("click");
            });

            fileFotoEmpleado.on("change", function () {
                lblNombreFotoEmpleado.html(document.getElementById('fileFotoEmpleado').files[0].name);
                lblNombreFotoEmpleado.trigger("change");
            });

            lblNombreFotoEmpleado.on("change", function (e) {
                let file = document.querySelector('input[id=fileFotoEmpleado]')['files'][0];
                let reader = new FileReader();
                reader.onload = function () {
                    base64String = reader.result.replace("data:", "").replace(/^.+,/, "");
                    document.getElementById("txtImageFotoEmpleadoBase64").value = reader.result;
                    document.getElementById("imgFotoEmpleado").src = document.getElementById("txtImageFotoEmpleadoBase64").value;
                    document.getElementById("imgFotoEmpleadoHead").src = document.getElementById("txtImageFotoEmpleadoBase64").value;
                }
                if (!file) {
                    // CODE ...
                } else {
                    reader.readAsDataURL(file);
                }
            });

            btnCEFotoEmpleado.on("click", function () {
                fncGuardarFotoEmpleado();

            });
            //#endregion

            btnEnviarCorreos.css("display", "none");

            cboCETipoEmpleado.fillCombo('FillCboTipoEmpleados', null, false, null);

            //#region SUSTENTOS HIJOS PERU
            btnCESustento.click(function () {
                fncGuardarSustentoHijo();
            });

            btnCargarSustentoHijo.click(function () {
                fncGetSustentos();
                mdlSustentosHijos.modal("show");
            });
            //#endregion
        }

        function fncEnviarCorreos(lstClaveEmpleado) {
            if (lstClaveEmpleado != "") {
                let obj = new Object();
                obj.lstClaveEmpleado = lstClaveEmpleado;
                axios.post('EnviarCorreos', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Es necesario seleccionar al menos un empleado para notificar.");
            }
        }

        function revisarArreglosIdenticos(arr1, arr2) {

            // Check if the arrays are the same length
            if (arr1.length !== arr2.length) return false;

            // Check if all items exist and are in the same order
            for (var i = 0; i < arr1.length; i++) {
                if (arr1[i] !== arr2[i]) return false;
            }

            // Otherwise, return true
            return true;
        }

        function addRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function addRowsSinLimpiar(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.rows.add(datos).draw();
        }

        function cargarAutoriza(autoriza) {
            axios.post('CargarAutoriza', { autoriza })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        txtCECompaniaAutorizaDescripcion.val(response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarUsuarioResg(usuarioResg) {
            axios.post('CargarUsuarioResg', { usuarioResg })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        txtCECompaniaUsuarioResgDescripcion.val(response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarDepto(depto) {
            axios.post('CargarDepto', { depto }).then(response => {
                let { success, datos, message } = response.data;
                if (success) {
                    txtCECompaniaDeptoDescripcion.val(response.data.data);
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function checarPermisoTabuladorLibre() {
            axios.post('ChecarPermisoTabuladorLibre')
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        $('#divTabulador').css('display', response.data.tienePermiso ? 'block' : 'none');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarRequisiciones() {
            axios.post('CargarRequisiciones')
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        dtRequisiciones.clear();
                        dtRequisiciones.rows.add(response.data.data);
                        dtRequisiciones.draw();
                        modalRequisiciones.modal('show');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarTabulador(cc, puesto, puestoTipoNom, idTabuladorDet) {
            $.get('CargarTabulador',
                {
                    cc,
                    puesto,
                    idTabuladorDet
                }).then(response => {
                    if (response.success) {
                        if (response.items == null) {
                            tblRH_REC_Tabuladores.DataTable().clear().draw();
                            AlertaGeneral('Alerta', 'No se encontro tabulador');
                            if (txtCECompaniaAutoriza.val() == null || txtCECompaniaAutoriza.val() == "") {
                                if (puestoTipoNom == 4) {
                                    txtCECompaniaAutoriza.val("113");
                                    txtCECompaniaAutoriza.trigger("change");
                                }

                                if (puestoTipoNom == 1) {
                                    txtCECompaniaAutoriza.val("71");
                                    txtCECompaniaAutoriza.trigger("change");
                                }
                            }
                            cboTipoNomina.val(puestoTipoNom);

                        } else {
                            tblRH_REC_Tabuladores.DataTable().rows().every(function (rowIdx, tableLoop, rowLoop) {
                                this.data().esNuevoTabulador = false;
                            });

                            let tabuladorPuesto = new Array();

                            let motivo_cambio = 0;
                            if (btnCEEmpleado.attr("data-esReingresoEmpleado") == 'true') {
                                motivo_cambio = 3;
                            }
                            if (lblCEDatosEmpleadoClaveEmpleado.text() == '') {
                                motivo_cambio = 1
                            }
                            let objTabulador = {
                                motivoCambio: motivo_cambio,
                                fecha_cambio: response.items.fecha_cambio,
                                fechaAplicaCambio: moment(response.items.fechaAplicaCambio).format('DD/MM/YYYY'),
                                fechaRealNomina: moment(response.items.fecha_real).format('DD/MM/YYYY'),
                                salario_base: response.items.salario_base,
                                complemento: response.items.complemento,
                                bono_de_zona: response.items.bono_de_zona,
                                suma: response.items.total,
                                esNuevoTabulador: true
                            }
                            tabuladorPuesto.push(objTabulador);
                            addRows(tblRH_REC_Tabuladores, tabuladorPuesto);
                            txtNumeroTabulador.val(response.items.tabuladorId);
                            cboTipoNomina.val(response.items.tipo_nomina);
                            cboTipoNomina.trigger("change");

                            if (txtCECompaniaAutoriza.val() == null || txtCECompaniaAutoriza.val() == "") {
                                if (response.items.tipo_nomina == 4) {
                                    txtCECompaniaAutoriza.val("113");
                                    txtCECompaniaAutoriza.trigger("change");
                                }

                                if (response.items.tipo_nomina == 1) {
                                    txtCECompaniaAutoriza.val("71");
                                    txtCECompaniaAutoriza.trigger("change");
                                }
                            }
                        }
                    } else {
                        // AlertaGeneral(`Alerta`, message);
                        AlertaGeneral('Alerta', 'No se encontro tabulador');

                    }
                }, error => {
                    AlertaGeneral(`Alerta`, error.message)
                });
        }

        //#region FUNCIONES GENERALES
        function fncEliminarAutoComplete() {
            //#region DATOS EMPLEADO
            txtCEDatosEmpleadoNombre.attr("autocomplete", "off");
            txtCEDatosEmpleadoApePaterno.attr("autocomplete", "off");
            txtCEDatosEmpleadoApeMaterno.attr("autocomplete", "off");
            txtCEDatosEmpleadoLocalidadNac.attr("autocomplete", "off");
            txtCEDatosEmpleadoRFC.attr("autocomplete", "off");
            txtCEDatosEmpleadoCPCIF.attr("autocomplete", "off");
            txtCEDatosEmpleadoCURP.attr("autocomplete", "off");
            //#endregion

            //#region GENERALES Y CONTACTO
            txtCEGenContactoEstudios.attr("autocomplete", "off");
            txtCEGenContactoAbreviatura.attr("autocomplete", "off");
            txtCEGenContactoCredElector.attr("autocomplete", "off");
            txtCEGenContactoDNI.attr("autocomplete", "off");
            txtCEGenContactoCalle.attr("autocomplete", "off");
            txtCEGenContactoNumExterior.attr("autocomplete", "off");
            txtCEGenContactoNumInterior.attr("autocomplete", "off");
            txtCEGenContactoColonia.attr("autocomplete", "off");
            txtCEGenContactoCP.attr("autocomplete", "off");
            txtCEGenContactoTelCasa.attr("autocomplete", "off");
            txtCEGenContactoTelCelular.attr("autocomplete", "off");
            txtCEGenContactoEmail.attr("autocomplete", "off");
            txtCEGenContactoAlergias.attr("autocomplete", "off");
            //#endregion

            //#region FAMILIARES
            txtCEFamiliarNombre.attr("autocomplete", "off");
            txtCEFamiliarApePaterno.attr("autocomplete", "off");
            txtCEFamiliarApeMaterno.attr("autocomplete", "off");
            txtCEFamiliarGradoEstudios.attr("autocomplete", "off");
            //#endregion

            //#region BENEFICIARIO
            txtCEBeneficiarioCP.attr("autocomplete", "off");
            txtCEBeneficiarioApePaterno.attr("autocomplete", "off");
            txtCEBeneficiarioApeMaterno.attr("autocomplete", "off");
            txtCEBeneficiarioNombre.attr("autocomplete", "off");
            txtCEBeneficiarioColonia.attr("autocomplete", "off");
            txtCEBeneficiarioDomicilio.attr("autocomplete", "off");
            txtCEBeneficiarioNumExt.attr("autocomplete", "off");
            txtCEBeneficiarioNumInt.attr("autocomplete", "off");
            txtCECasoAccidenteNombre.attr("autocomplete", "off");
            txtCECasoAccidenteTelefono.attr("autocomplete", "off");
            txtCECasoAccidenteDomicilio.attr("autocomplete", "off");
            //#endregion

            //#region COMPAÑIA
            txtCECompaniaRequisicion.attr("autocomplete", "off");
            txtCECompaniaAutoriza.attr("autocomplete", "off");
            txtCECompaniaUsuarioResg.attr("autocomplete", "off");
            txtCECompaniaDepto.attr("autocomplete", "off");
            txtCECompaniaNSS.attr("autocomplete", "off");
            txtCECompaniaContrato.attr("autocomplete", "off");
            //#endregion

            //#region NOMINA
            txtCETabuladorNumTabulador.attr("autocomplete", "off");
            txtCETabuladorTarjeta.attr("autocomplete", "off");
            txtCETabuladorCtaAhorro.attr("autocomplete", "off");
            txtCENominaSalarioSalarioBase.attr("autocomplete", "off");
            txtCENominaSalarioComplemento.attr("autocomplete", "off");
            txtCENominaSalarioBonoZona.attr("autocomplete", "off");
            txtCENominaSalarioTotal.attr("autocomplete", "off");
            //#endregion

            //#region UNIFORME
            txtCEUniformeFechaEntrega.attr("autocomplete", "off");
            txtCEUniformeNoCalzado.attr("autocomplete", "off");
            txtCEUniformeCamisa.attr("autocomplete", "off");
            txtCEUniformePantalon.attr("autocomplete", "off");
            txtCEUniformeOverol.attr("autocomplete", "off");
            // txtCEUniformeUniformeDama.attr("autocomplete", "off");
            txtCEUniformeOtros.attr("autocomplete", "off");

            txtCEUniformeNoCalzado.on("change", function () {
                fncFocus("txtCEUniformeNoCalzado");
            });

            txtCEUniformeCamisa.on("change", function () {
                fncFocus("txtCEUniformeCamisa");
            });

            txtCEUniformePantalon.on("change", function () {
                fncFocus("txtCEUniformePantalon");
            });

            txtCEUniformeOverol.on("change", function () {
                fncFocus("txtCEUniformeOverol");
            });
            //#endregion
        }

        function fncHabilitarDeshabilitarCtrlsMdl(disabled) {
            // //#region DATOS EMPLEADO
            // txtCEDatosEmpleadoNombre.attr("disabled", disabled);
            // txtCEDatosEmpleadoApePaterno.attr("disabled", disabled);
            // txtCEDatosEmpleadoApeMaterno.attr("disabled", disabled);
            // cboCEDatosEmpleadoPaisNac.attr("disabled", disabled);
            // cboCEDatosEmpleadoEstadoNac.attr("disabled", disabled);
            // cboCEDatosEmpleadoLugarNac.attr("disabled", disabled);
            // txtCEDatosEmpleadoFechaNacimiento.attr("disabled", disabled);
            // txtCEDatosEmpleadoFechaIngreso.attr("disabled", disabled);
            // txtCEDatosEmpleadoLocalidadNac.attr("disabled", disabled);
            // txtCEDatosEmpleadoRFC.attr("disabled", disabled);
            // txtCEDatosEmpleadoCURP.attr("disabled", disabled);
            // cboCEDatosEmpleadoSexo.attr("disabled", disabled);
            // //#endregion

            // //#region GENERALES Y CONTACTO
            // cboCEGenContactoEstadoCivil.attr("disabled", disabled);
            // txtCEGenContactoFechaPlanta.attr("disabled", disabled);
            // txtCEGenContactoEstudios.attr("disabled", disabled);
            // txtCEGenContactoAbreviatura.attr("disabled", disabled);
            // txtCEGenContactoCredElector.attr("disabled", disabled);
            // txtCEGenContactoDNI.attr("disabled", disabled);
            // txtCEGenContactoCalle.attr("disabled", disabled);
            // txtCEGenContactoNumExterior.attr("disabled", disabled);
            // txtCEGenContactoNumInterior.attr("disabled", disabled);
            // txtCEGenContactoColonia.attr("disabled", disabled);
            // cboCEGenContactoEstado.attr("disabled", disabled);
            // cboCEGenContactoCiudad.attr("disabled", disabled);
            // txtCEGenContactoCP.attr("disabled", disabled);
            // txtCEGenContactoTelCasa.attr("disabled", disabled);
            // txtCEGenContactoTelCelular.attr("disabled", disabled);
            // txtCEGenContactoEmail.attr("disabled", disabled);
            // cboCEGenContactoTipoCasa.attr("disabled", disabled);
            // cboCEGenContactoTipoSangre.attr("disabled", disabled);
            // txtCEGenContactoAlergias.attr("disabled", disabled);
            // //#endregion
        }

        function fncFocus(obj) {
            if (obj != "") {
                setTimeout(() => $(`#${obj}`).focus(), 50);
            }
        }

        function fncGetHeaderEmpleados() {
            axios.post("GetHeaderEmpleados").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    // txtHeadCantA.text(items[0].cant);
                    // txtHeadCantP.text(items[1].cant);
                    // txtHeadCantC.text(items[2].cant);
                    // txtHeadCantB.text(items[3].cant);

                    items.forEach(e => {
                        let estatus = e.estatus_empleado;
                        switch (estatus) {
                            case "A":
                                txtHeadCantA.text(maskNumero_NoMonedaNoDecimal(e.cant));
                                break;
                            case "B":
                                txtHeadCantB.text(maskNumero_NoMonedaNoDecimal(e.cant));
                                break;
                            case "C":
                                txtHeadCantC.text(maskNumero_NoMonedaNoDecimal(e.cant));
                                break;
                            case "P":
                                txtHeadCantP.text(maskNumero_NoMonedaNoDecimal(e.cant));
                                break;
                        }
                    });
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        function cargarInformacionRequisicion() {
            let requisicion_id = txtCECompaniaRequisicion.val();

            if (isNaN(requisicion_id)) {
                Alert2Warning('Debe capturar un ID de requisición válido.');
                return;
            }

            if (requisicion_id == '') {
                return;
            }

            axios.post('GetInformacionRequisicion', { requisicion_id })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        txtCECompaniaPlantilla.val(response.data.data.id_plantilla);
                        if (txtCECompaniaPlantilla.val() != "") {
                            txtCECompaniaPlantilla.trigger("keyup");
                        }

                        txtCECompaniaCCDescripcion.data('cc', response.data.data.cc);
                        txtCECompaniaCCDescripcion.attr("title", response.data.data.cc);
                        fncCECompaniaFillDepartamentos(response.data.data.cc, false, 0);
                        txtCECompaniaDeptoDescripcion.val("");
                        txtCECompaniaCCDescripcion.val(response.data.data.ccDesc);
                        txtCECompaniaCCDescripcion.change();
                        txtCECompaniaPuestoDescripcion.data('puesto', response.data.data.puesto);
                        txtCECompaniaPuestoDescripcion.val("[" + response.data.data.puesto + "] " + response.data.data.puestoDesc);
                        txtCECompaniaJefeInmediatoDescripcion.data('jefe_inmediato', response.data.data.jefe_inmediato);
                        txtCECompaniaJefeInmediatoDescripcion.val(response.data.data.jefe_inmediatoDesc);

                        if (response.data.data.puesto_sindicalizado) {
                            if (!chkCECompaniaSindicato.prop('checked')) {
                                chkCECompaniaSindicato.next().click();
                            }
                        } else {
                            if (chkCECompaniaSindicato.prop('checked')) {
                                chkCECompaniaSindicato.next().click();
                            }
                        }

                        cboCECompaniaPuestoCategoria.fillComboBox('GetCategoriaPuesto', { tabuladorDetID: response.data.data.idTabuladorDet }, '-- Seleccionar --', () => {
                            cboCECompaniaPuestoCategoria.select2({ width: "100%" });
                            cboCECompaniaPuestoCategoria.find('option:selected').remove();
                        });

                        cargarTabulador(response.data.data.cc, response.data.data.puesto, response.data.data.puestoTipoNom, response.data.data.idTabuladorDet);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        //#region CRUD DATOS EMPLEADOS
        function initTblEmpleados() {
            dtEmpleados = $("#tblRH_REC_Empleados").DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'clave_empleado' },
                    { data: 'cc_contableDesc', title: 'CC' },
                    {
                        data: 'id_regpat', title: 'REGISTRO PATRONAL',
                        render: function (data, type, row) {
                            return "[" + data + "] " + row.desc_reg_pat;
                        }
                    },
                    { data: 'clave_empleado', title: 'CLAVE' },
                    {
                        title: 'NOMBRE',
                        render: function (data, type, row) {
                            let apePaterno = row.ape_paterno;
                            let apeMaterno = row.ape_materno;
                            let nombre = row.nombre;

                            if (apeMaterno == null) {
                                return apePaterno + " " + nombre;
                            } else {
                                return apePaterno + " " + apeMaterno + " " + nombre;
                            }
                        }
                    },
                    {
                        data: 'descripcion', title: 'PUESTO',
                        render: function (data, type, row) {
                            return "[" + row.puesto + "] " + data;
                        }
                    },
                    {
                        data: 'categoriaDescripcion', title: 'CATEGORIA',
                        render: function (data, type, row) {
                            return data ?? "";
                        }
                    },
                    {
                        title: 'ESTATUS', visible: false,
                        render: function (data, type, row) {
                            return `EnKontrol`;
                        }
                    },
                    {
                        //data: 'fecha_primer_contrato', title: 'FECHA PRIMER INGRESO',
                        data: 'fecha_altaStr', title: 'FECHA PRIMER INGRESO',
                        render: function (data, type, row) {
                            //return moment(data).format("DD/MM/YYYY");
                            return data;
                        }
                    },
                    {
                        //data: 'fecha_antiguedad', title: 'FECHA ALTA',
                        data: 'fecha_antiguedadStr', title: 'FECHA INGRESO ACTUAL',
                        render: function (data, type, row) {
                            //return moment(data).format("DD/MM/YYYY");
                            return data;
                        }
                    },
                    {
                        data: 'fechaBaja', title: 'FECHA BAJA',
                        render: function (data, type, row) {
                            return data != null ? moment(data).format("DD/MM/YYYY") : "";
                        }
                    },
                    {
                        data: 'estatus_empleado', title: 'ESTATUS',
                        render: function (data, type, row) {
                            let estatusEmpleado = "";
                            switch (data) {
                                case "A":
                                    estatusEmpleado = "ACTIVO";
                                    break;
                                case "B":
                                    estatusEmpleado = "BAJA";
                                    break;
                                case "P":
                                    estatusEmpleado = "PENDIENTE";
                                    break;
                                case "C":
                                    estatusEmpleado = "CANCELADA";
                                    break;
                                default:
                                    break;
                            }
                            return estatusEmpleado;
                        }
                    },
                    {
                        title: 'IMPRIMIR',
                        render: function (data, type, row) {
                            return (row.estatus_empleado == 'A' || row.estatus_empleado == 'P') ? `<button class="btn btn-primary btn-xs imprimirContratos"><i class="fas fa-print"></i></button>` : `<button class="btn btn-primary btn-xs imprimirContratos" disabled><i class="fas fa-print"></i></button>`;
                        }
                    },
                    {
                        data: 'cantidadCCHistorial', title: '',
                        render: function (data, type, row) {
                            return (row.estatus_empleado == 'A' || row.estatus_empleado == 'P') ? `<button class="btn  btn-xs btn-primary btn-sm getHistorial">CCs: ${data}</button>` : `<button class="btn  btn-xs btn-primary btn-sm getHistorial" disabled>CCs: ${data}</button>`;
                        }

                    },
                    {
                        data: 'fecha_nac', title: 'fecha_nac', visible: false,
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    { data: 'clave_pais_nac', visible: false },
                    { data: 'paisNac', visible: false },
                    { data: 'clave_estado_nac', visible: false },
                    { data: 'estadoNac', visible: false },
                    { data: 'clave_ciudad_nac', visible: false },
                    { data: 'lugarNac', visible: false },
                    { data: 'localidad_nacimiento', visible: false },
                    {
                        data: 'fecha_alta', visible: false,
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    { data: 'sexo', visible: false },
                    { data: 'rfc', visible: false },
                    { data: 'curp', visible: false },
                    {
                        render: function (data, type, row) {
                            let btnReingreso = `<button title='Reinigresar empleado.' class="btn btn-success reingresoEmpleado btn-xs"><i class="fa fa-door-open"></i></button>&nbsp;`;
                            let btnCambiarContratable = `<button title='No contratable.' class="btn btn-danger cambiarContratable btn-xs"><i class="fa fa-door-open"></i></button>&nbsp;`;
                            let btnActualizar = `<button title='Actualizar datos de empleado.' class="btn btn-warning actualizarEmpleado btn-xs"><i class="far fa-edit"></i></button>&nbsp;`;
                            let btnEliminar = `<button title='Eliminar registro' class="btn btn-danger eliminarEmpleado btn-xs"><i class="far fa-trash-alt"></i></button>`;
                            let btnAutorizar = '<button class="btn btn-success btnAutorizar btn-xs" title="Autorizar"><i class="fas fa-user-check"></i></button>&nbsp;';
                            let btnCancelar = '<button class="btn btn-danger btnCancelar btn-xs" title="Cancelar"><i class="fas fa-user-times"></i></button>&nbsp;';
                            let btnComentario = '<button title="Ver comentario" class="btn btn-sm btn-primary verComentario btn-xs" ><i class="far fa-comments"></i></button>&nbsp;'
                            let btnRazonBaja = '<button class="btn btn-default btnRazonBaja btn-xs" title="Ver Razón de Baja"><i class="fas fa-eye"></i></button>&nbsp;';

                            if (row.esAutorizante && row.estatus_empleado == _ESTATUS.PENDIENTE) {
                                return btnAutorizar + btnCancelar + btnActualizar;
                            }

                            // let lstEstatus = getValoresMultiples('#chkFiltroEsPendiente').filter(function (x) { return x })

                            if (row.estatus_empleado == 'B') {


                                if (row.contratable == 'S') {

                                    if (esPermisoMostrarBotones) {
                                        btnCambiarContratable = `<button title='Contratable.' class="btn btn-danger cambiarContratable btn-xs"><i class="fas fa-user-lock"></i></button>&nbsp;`;

                                    } else {
                                        btnCambiarContratable = ``;

                                    }

                                    if (row.est_contabilidad == 'A') {
                                        return btnCambiarContratable + btnReingreso + btnRazonBaja;

                                    } else {
                                        return btnCambiarContratable + btnRazonBaja;
                                    }
                                } else {
                                    return btnCambiarContratable + btnRazonBaja;
                                }
                            } else if (row.estatus_empleado == 'C') {
                                if (row.contratable == 'S') {
                                    // if (row.est_contabilidad == 'A') {
                                    //     return btnComentario + btnReingreso + btnRazonBaja;
                                    // } else {
                                    //     return btnComentario + btnRazonBaja;
                                    // }
                                    if (esPermisoMostrarBotones) {
                                        btnCambiarContratable = `<button title='Contratable.' class="btn btn-danger cambiarContratable btn-xs"><i class="fas fa-user-lock"></i></button>&nbsp;`;

                                    } else {
                                        btnCambiarContratable = ``;

                                    }

                                    return btnComentario + btnCambiarContratable + btnReingreso + btnRazonBaja;
                                } else {
                                    return btnComentario + btnCambiarContratable + btnRazonBaja;
                                }
                            }
                            else {
                                return btnActualizar;
                            }
                        }
                    },
                    { data: 'nombre', title: 'Nombre', visible: false },
                    { data: 'ape_paterno', title: 'Apellido paterno', visible: false },
                    { data: 'ape_materno', title: 'Apellido materno', visible: false }
                ],
                initComplete: function (settings, json) {
                    $("#tblRH_REC_Empleados").on('click', '.actualizarEmpleado', function () {
                        cboCandidatosAprobados.attr("disabled", true);
                        divCandidatosAprobados.css("display", "none");

                        let rowData = $("#tblRH_REC_Empleados").DataTable().row($(this).closest('tr')).data();
                        fncGetDatosActualizarEmpleado(rowData.clave_empleado, false);
                        btnCEFotoEmpleado.css("display", "inline");
                        btnCEEmpleado.css("display", "initial");
                    });

                    $("#tblRH_REC_Empleados").on('click', '.eliminarEmpleado', function () {
                        let rowData = dtEmpleados.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarEmpleado());
                    });

                    $("#tblRH_REC_Empleados").on("click", ".imprimirContratos", function () {
                        let rowData = dtEmpleados.row($(this).closest('tr')).data();
                        mdlImprimirContratos.attr("data-claveEmpleado", rowData.clave_empleado);
                        mdlImprimirContratos.modal("show");
                    });

                    $("#tblRH_REC_Empleados").on("click", ".contratosFirmados", function () {
                        let rowData = dtEmpleados.row($(this).closest('tr')).data();
                        fncGetContratosFirmados(rowData.clave_empleado);
                        mdlContratosFirmados.modal("show");
                    });

                    $("#tblRH_REC_Empleados").on('click', '.reingresoEmpleado', function () {
                        //#region SE ABRE MODAL EDITAR PERO SIN UNA REQUISICIÓN ASIGNADA, Y CON EL ESTATUS DEL EMPLEADO P (PENDIENTE).
                        let rowData = dtEmpleados.row($(this).closest('tr')).data();
                        cboCandidatosAprobados.attr("disabled", true);
                        divCandidatosAprobados.css("display", "initial");

                        if (rowData.idCandidato != null) {
                            if ($(`#cboCandidatosAprobados option[value='${rowData.idCandidato}']`).length > 0) {
                                fncGetDatosActualizarEmpleado(rowData.clave_empleado, true);
                                btnCEEmpleado.css("display", "initial"); //OCULTAR BOTON DE INGRESO

                            } else {
                                Alert2Warning("Este empleado no cuenta con un canidato apto para ser dado de alta.");
                            }
                        } else {
                            Alert2Warning("Este empleado no cuenta con un candidato.");
                        }

                        //#endregion
                    });

                    $("#tblRH_REC_Empleados").on('click', '.cambiarContratable', function () {
                        let rowData = dtEmpleados.row($(this).closest('tr')).data();
                        let apePaterno = rowData.ape_paterno;
                        let apeMaterno = rowData.ape_materno;
                        let nombre = rowData.nombre;
                        let nombre_completo = apePaterno + " " + apeMaterno + " " + nombre;
                        let motivoDeBaja = "";

                        if (rowData.contratable == "S") {
                            _clave_empleado = rowData.clave_empleado;

                            Alert2AccionConfirmar('¡Cuidado!',
                                `¿Desea cambiar el estado de Contratable a No contrable en el empleado ${rowData.clave_empleado} - ${nombre_completo}? ${motivoDeBaja} <br/><br/>
                                <div style="text-align: left;">
                                    <label style="font-size: 14px;">Estatus:</label>
                                    <select id="inputFolioRequisicion" class="form-control">
                                        <option value='N'>No Contratable</option>
                                    </selec>
                                </div>`, 'Confirmar', 'Cancelar', () => fncCambiarContratable(rowData.clave_empleado, $("#inputFolioRequisicion").val()));

                        } else {
                            if (rowData.motivoBaja != "" && rowData.motivoBaja != null) {
                                motivoDeBaja = "<br>Motivo de Baja: " + rowData.motivoBaja
                            }

                            _clave_empleado = rowData.clave_empleado;

                            Alert2AccionConfirmar('¡Cuidado!',
                                `¿Desea cambiar el estado de No contratable a contratable en el empleado ${rowData.clave_empleado} - ${nombre_completo}? ${motivoDeBaja} <br/><br/>
                            <div style="text-align: left;">
                                <label style="font-size: 14px;">Estatus:</label>
                                <select id="inputFolioRequisicion" class="form-control">
                                    <option value='S'>contratable</option>
                                </selec>
                            </div>`, 'Confirmar', 'Cancelar', () => fncCambiarContratable(rowData.clave_empleado, $("#inputFolioRequisicion").val()));

                        }

                    });

                    $("#tblRH_REC_Empleados").on('click', '.btnAutorizar', function () {
                        let rowData = dtEmpleados.row($(this).closest('tr')).data();

                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea autorizar la alta seleccionada?', 'Confirmar', 'Cancelar', () => fncCambiarEstatusEmpleado(rowData.clave_empleado, "A"));

                    });

                    $("#tblRH_REC_Empleados").on('click', '.btnCancelar', function () {
                        let rowData = dtEmpleados.row($(this).closest('tr')).data();

                        Swal.fire({
                            position: "center",
                            icon: "warning",
                            title: "Rechazar alta",
                            input: 'textarea',
                            width: '50%',
                            showCancelButton: true,
                            html: "<h3>¿Desea rechazar la alta seleccionada?<br>Comentario:</h3>",
                            confirmButtonText: "Rechazar",
                            confirmButtonColor: "#d9534f",
                            cancelButtonText: "Cancelar",
                            cancelButtonColor: "#5c636a",
                            showCloseButton: true
                        }).then((result) => {
                            if (result.isConfirmed) {
                                fncCrearCometario(rowData.clave_empleado, $('.swal2-textarea').val());
                            }
                        });
                    });

                    $("#tblRH_REC_Empleados").on('click', '.verComentario', function () {
                        let rowData = dtEmpleados.row($(this).closest('tr')).data();

                        fncGetComentario(rowData.clave_empleado);
                        mdlComentario.modal("show");

                    });

                    $("#tblRH_REC_Empleados").on('click', '.getHistorial', function () {
                        let rowData = dtEmpleados.row($(this).closest('tr')).data();

                        fncGetHistorialCC(rowData.clave_empleado);
                        mdlHistorial.modal("show");

                    });

                    $("#tblRH_REC_Empleados").on('click', '.btnRazonBaja', function () {
                        let rowData = dtEmpleados.row($(this).closest('tr')).data();

                        Alert2GeneralPermanente('Razón de Baja:', rowData.motivoBaja);
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { "width": "2%", "targets": 0 },
                    { "width": "10%", "targets": 2 },
                    { "width": "4%", "targets": 3 },
                    { "width": "8%", "targets": 6 },
                    { "width": "8%", "targets": 8 },
                    { "width": "4%", "targets": 9 },
                    { "width": "8%", "targets": 10 },
                    { "width": "4%", "targets": 11 },
                    { "width": "4%", "targets": 12 },
                    {
                        targets: 0,
                        checkboxes: {
                            selectRow: true
                        }
                    }
                ],
                select: {
                    style: 'multi',
                    selector: 'td:first-child'
                }
            });
        }

        function fncCambiarContratable(claveEmpleado, esContratable) {
            axios.post('CambiarContratable', { claveEmpleado: claveEmpleado, esContratable }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetEmpleadosEK();
                    Alert2Exito(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncReingresoEmpleadoEmpleado() {
            AlertaGeneral("Confirmación", "Se realizacion los movimientos correspondientes para el reingreso");
        }

        function fncGetDatosActualizarEmpleado(claveEmpleado, esReingresoEmpleado) {
            let obj = new Object();
            obj = {
                claveEmpleado: claveEmpleado,
                esReingresoEmpleado: esReingresoEmpleado
            }

            if (inputPermisoEditarFechaCambio.val() != '1' && !esReingresoEmpleado) {
                txtCEDatosEmpleadoFechaIngreso.prop('disabled', true);
            } else {
                txtCEDatosEmpleadoFechaIngreso.prop('disabled', false);
            }

            axios.post("GetDatosActualizarEmpleado", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region DATOS EMPLEADO
                    if (!esReingresoEmpleado) {
                        switch (response.data.lstDatos[0].estatus_empleado) {
                            case "A":
                                lblCEDatosEmpleadoEstatus.html("ACTIVO");
                                lblCEDatosEmpleadoEstatus.css("color", "blue");
                                break;
                            case "B":
                                lblCEDatosEmpleadoEstatus.html("BAJA");
                                lblCEDatosEmpleadoEstatus.css("color", "red");
                                break;
                            case "C":
                                lblCEDatosEmpleadoEstatus.html("C");
                                lblCEDatosEmpleadoEstatus.css("color", "red");
                                break;
                            case "P":
                                lblCEDatosEmpleadoEstatus.html("PENDIENTE");
                                lblCEDatosEmpleadoEstatus.css("color", "blue");
                                break;
                            default:
                                lblCEDatosEmpleadoEstatus.html("");
                                lblCEDatosEmpleadoEstatus.css("color", "blue");
                                break;
                        }

                        txtCEDatosEmpleadoFechaIngreso.val(moment(response.data.lstDatos[0].fecha_antiguedad).format("DD/MM/YYYY"));

                        cboCandidatosAprobados[0].selectedIndex = 0;
                        cboCandidatosAprobados.trigger("change");

                    } else {
                        lblCEDatosEmpleadoEstatus.html("PENDIENTE");
                        lblCEDatosEmpleadoEstatus.css("color", "blue");
                        txtCEDatosEmpleadoFechaIngreso.prop('disabled', false);
                        txtCEDatosEmpleadoFechaIngreso.val(moment().format("DD/MM/YYYY"));
                    }

                    txtCEDatosEmpleadoRFC.val(response.data.lstDatos[0].rfc);
                    txtCEDatosEmpleadoCURP.val(response.data.lstDatos[0].curp);
                    txtCEDatosEmpleadoCPCIF.val(response.data.lstDatos[0].CPCIF); //Constancia de identificacion fiscal
                    txtCEDatosEmpleadoCUSPP.val(response.data.lstDatos[0].cuspp);
                    lblCEDatosEmpleadoClaveEmpleado.html(response.data.lstDatos[0].clave_empleado);
                    lblCEDatosEmpleadoClaveEmpleado.data('clvempleado', response.data.lstDatos[0].clave_empleado);
                    txtCEDatosEmpleadoNombre.val(response.data.lstDatos[0].nombre);
                    txtCEDatosEmpleadoApePaterno.val(response.data.lstDatos[0].ape_paterno);
                    txtCEDatosEmpleadoApeMaterno.val(response.data.lstDatos[0].ape_materno);
                    txtCEDatosEmpleadoFechaNacimiento.val(moment(response.data.lstDatos[0].fecha_nacString).format('DD/MM/YYYY'));
                    cboCEDatosEmpleadoPaisNac.val(response.data.lstDatos[0].clave_pais_nac);
                    cboCEDatosEmpleadoPaisNac.trigger("change", ['noModificarRFCCURP']);
                    cboCEDatosEmpleadoDepartamentoNac.val(response.data.lstDatos[0].clave_departamento_nac_PERU);
                    cboCEDatosEmpleadoDepartamentoNac.change();
                    cboCEDatosEmpleadoEstadoNac.val(response.data.lstDatos[0].clave_estado_nac);
                    cboCEDatosEmpleadoEstadoNac.trigger("change", ['noModificarRFCCURP']);
                    cboCEDatosEmpleadoLugarNac.val(response.data.lstDatos[0].clave_ciudad_nac);
                    cboCEDatosEmpleadoLugarNac.trigger("change", ['noModificarRFCCURP']);
                    txtCEDatosEmpleadoLocalidadNac.val(response.data.lstDatos[0].localidad_nacimiento);
                    cboCEDatosEmpleadoSexo.val(response.data.lstDatos[0].sexo);
                    cboCEDatosEmpleadoSexo.trigger("change", ['noModificarRFCCURP']);


                    lblTitleCrearEditarEmpleadoEK.html("Actualizar empleado");
                    if (response.data.lstDatos[0].banco) {
                        chkTarjetaNomina.prop('checked', true);
                        chkTarjetaNomina.trigger('change');
                    }
                    cboBancoNomina.val(response.data.lstDatos[0].banco);
                    txtTarjetaNomina.val(response.data.lstDatos[0].num_cta_pago);
                    txtClaveInterbancaria.val(response.data.lstDatos[0].num_cta_fondo_aho);
                    cboTipoNomina.val(response.data.lstDatos[0].tipo_nomina);
                    txtNumeroTabulador.val(response.data.lstDatos[0].tabulador);
                    //#endregion

                    //#region GENERALES Y CONTACTO
                    cboCEGenContactoEmpleadoPais.val(response.data.lstGenerales[0].pais_dom > 0 ? response.data.lstGenerales[0].pais_dom : response.data.lstDatos[0].clave_pais_nac);
                    cboCEGenContactoEmpleadoPais.trigger("change");

                    cboCEGenContactoEstadoCivil.val(response.data.lstGenerales[0].estado_civil);
                    cboCEGenContactoEstadoCivil.trigger("change");
                    txtCEGenContactoEstudios.val(response.data.lstGenerales[0].ocupacion);
                    txtCEGenContactoCalle.val(response.data.lstGenerales[0].domicilio);
                    txtCEGenContactoColonia.val(response.data.lstGenerales[0].colonia);
                    txtCEGenContactoCP.val(response.data.lstGenerales[0].codigo_postal);
                    txtCEGenContactoEmail.val(response.data.lstGenerales[0].email);
                    txtCEGenContactoAlergias.val(response.data.lstGenerales[0].alergias);
                    if (response.data.lstGenerales[0].alergias) {
                        chkCEGenContactoAlergias.prop('checked', true);

                    } else {
                        chkCEGenContactoAlergias.prop('checked', false);
                    }
                    chkCEGenContactoAlergias.trigger("change");
                    txtCEGenContactoFechaPlanta.val(moment(response.data.lstGenerales[0].fecha_planta).format("DD/MM/YYYY"));
                    txtCEGenContactoAbreviatura.val(response.data.lstGenerales[0].ocupacion_abrev);
                    txtCEGenContactoNumExterior.val(response.data.lstGenerales[0].numero_exterior);
                    cboCEGenContactoDepartamentoNac.val(response.data.lstGenerales[0].PERU_departamento_dom);
                    cboCEGenContactoDepartamentoNac.trigger("change", ['noFillEstados']);
                    cboCEGenContactoEstado.val(response.data.lstGenerales[0].estado_dom);
                    cboCEGenContactoEstado.trigger("change");
                    txtCEGenContactoTelCasa.val(response.data.lstGenerales[0].tel_casa);
                    cboCEGenContactoTipoCasa.val(response.data.lstGenerales[0].tipo_casa);
                    cboCEGenContactoTipoCasa.trigger("change");
                    txtCEGenContactoCredElector.val(response.data.lstGenerales[0].num_cred_elector);
                    txtCEGenContactoDNI.val(response.data.lstGenerales[0].num_dni);
                    txtCEGenContactoCedulaCuidadania.val(response.data.lstGenerales[0].cedula_cuidadania);
                    txtCEGenContactoNumInterior.val(response.data.lstGenerales[0].numero_interior);
                    cboCEGenContactoCiudad.val(response.data.lstGenerales[0].ciudad_dom);
                    cboCEGenContactoCiudad.trigger("change");
                    txtCEGenContactoTelCelular.val(response.data.lstGenerales[0].tel_cel);
                    cboCEGenContactoTipoSangre.val(response.data.lstGenerales[0].tipo_sangre);
                    cboCEGenContactoTipoSangre.trigger("change");

                    //#endregion

                    //#region BENEFICIARIO
                    cboCEBeneficiarioEmpleadoPais.val(response.data.lstGenerales[0].pais_ben > 0 ? response.data.lstGenerales[0].pais_ben : response.data.lstDatos[0].clave_pais_nac);
                    cboCEBeneficiarioEmpleadoPais.trigger("change");

                    cboCEBeneficiarioCiudad.fillCombo("/Reclutamientos/FillCboMunicipios", { _clavePais: cboCEBeneficiarioEmpleadoPais.val(), _claveEstado: response.data.lstGenerales[0].estado_ben }, false);
                    cboCEBeneficiarioCiudad.select2({ width: "100%" });

                    cboCEBeneficiarioParentesco.val(response.data.lstGenerales[0].parentesco_ben);
                    cboCEBeneficiarioParentesco.trigger("change");
                    txtCEBeneficiarioApePaterno.val(response.data.lstGenerales[0].paterno_ben);
                    cboCEBeneficiarioDepartamentoNac.val(response.data.lstGenerales[0].PERU_departamento_ben);
                    cboCEBeneficiarioDepartamentoNac.trigger("change", ['noFillEstados']);
                    cboCEBeneficiarioEstado.val(response.data.lstGenerales[0].estado_ben);
                    cboCEBeneficiarioEstado.trigger("change");
                    txtCEBeneficiarioDomicilio.val(response.data.lstGenerales[0].domicilio_ben);
                    txtCEBeneficiarioFechaNacimiento.val(moment(response.data.lstGenerales[0].fecha_nac_ben).format("YYYY-MM-DD"));
                    txtCEBeneficiarioApeMaterno.val(response.data.lstGenerales[0].materno_ben);
                    cboCEBeneficiarioCiudad.val(response.data.lstGenerales[0].ciudad_ben);
                    cboCEBeneficiarioCiudad.trigger("change");
                    txtCEBeneficiarioNumExt.val(response.data.lstGenerales[0].num_ext_ben);
                    txtCEBeneficiarioCP.val(response.data.lstGenerales[0].codigo_postal_ben);
                    txtCEBeneficiarioNombre.val(response.data.lstGenerales[0].nombre_ben);
                    txtCEBeneficiarioColonia.val(response.data.lstGenerales[0].colonia_ben);
                    txtCEBeneficiarioNumInt.val(response.data.lstGenerales[0].num_int_ben);
                    txtCECasoAccidenteNombre.val(response.data.lstGenerales[0].en_accidente_nombre);
                    txtCECasoAccidenteDomicilio.val(response.data.lstGenerales[0].en_accidente_direccion);
                    txtCECasoAccidenteTelefono.val(response.data.lstGenerales[0].en_accidente_telefono);
                    txtCEBeneficiarioCel.val(response.data.lstGenerales[0].tel_cel);
                    txtCEBeneficiarioCURP.val(response.data.lstGenerales[0].ben_num_dni);
                    //#endregion

                    //#region COMPAÑIA
                    txtCECompaniaActividades.val(response.data.lstDatos[0].actividades);
                    if (!esReingresoEmpleado) {
                        txtCECompaniaRequisicion.val(response.data.lstDatos[0].requisicion);
                        // txtCECompaniaRequisicion.trigger("change");
                        txtCECompaniaPlantilla.val(response.data.plantilla);
                        txtCECompaniaCCDescripcion.attr("title", response.data.lstDatos[0].cc_contable)
                        txtCECompaniaCCDescripcion.data('cc', response.data.lstDatos[0].cc_contable);
                        txtCECompaniaCCDescripcion.val(response.data.lstDatos[0].nombreCC);
                        txtCECompaniaCCDescripcion.change();
                        txtCECompaniaPuestoDescripcion.data('puesto', response.data.lstDatos[0].puesto);
                        txtCECompaniaPuestoDescripcion.val("[" + response.data.lstDatos[0].puesto + "] " + response.data.lstDatos[0].descripcion);
                        txtCECompaniaJefeInmediatoDescripcion.data('jefe_inmediato', response.data.lstDatos[0].jefe_inmediato);

                        let esSindicato = response.data.lstDatos[0].sindicato;
                        if (esSindicato != "N") {
                            chkCECompaniaSindicato.prop("checked", true);
                        } else {
                            chkCECompaniaSindicato.prop("checked", false);
                        }
                        chkCECompaniaSindicato.trigger("change");

                        let usuario_compras = +response.data.lstDatos[0].usuario_compras > 0 ? response.data.lstDatos[0].usuario_compras : "N/A";
                        txtCECompaniaUsuarioResg.val(usuario_compras);
                        txtCECompaniaUsuarioResgDescripcion.val(response.data.lstDatos[0].nombreUsuarioReg);

                        txtCECompaniaAutoriza.val(response.data.lstDatos[0].autoriza);
                        txtCECompaniaAutorizaDescripcion.val(response.data.lstDatos[0].nombreAutoriza);
                    } else {
                        txtCECompaniaRequisicion.val("");
                        txtCECompaniaCCDescripcion.attr("title", "");
                        txtCECompaniaCCDescripcion.data('cc', "");
                        txtCECompaniaCCDescripcion.val("");
                        txtCECompaniaCCDescripcion.change();
                        txtCECompaniaPuestoDescripcion.data('puesto', "");
                        txtCECompaniaPuestoDescripcion.val("");
                        txtCECompaniaJefeInmediatoDescripcion.data('jefe_inmediato', "");

                        chkCECompaniaSindicato.prop("checked", false);

                        txtCECompaniaUsuarioResg.val("");
                        txtCECompaniaUsuarioResgDescripcion.val("");
                        txtCECompaniaAutoriza.val("");
                        txtCECompaniaAutorizaDescripcion.val("");
                        // cboCECompaniaPuestoCategoria.val("");
                        // cboCECompaniaPuestoCategoria.trigger("change");
                        fncGetIDUsuarioEK();
                    }

                    fncCECompaniaFillDepartamentos(txtCECompaniaCCDescripcion.attr("title"), true, response.data.lstDatos[0].clave_depto);

                    // cboCECompaniaPuestoCategoria.val(response.data.lstDatos[0].idCategoria);
                    // cboCECompaniaPuestoCategoria.trigger("change");

                    selectRegistroPatronal.val(response.data.lstDatos[0].id_regpat);
                    selectTipoContrato.prop('disabled', true);
                    selectTipoContrato.val(response.data.lstDatos[0].duracion_contrato);
                    selectTipoContrato.trigger('change');

                    if (response.data.lstDatos[0].fecha_fin != null) {
                        dateCECompaniaContratoFin.prop('disabled', true);
                    }

                    dateCECompaniaContratoFin.val(moment(response.data.lstDatos[0].fecha_fin).format("YYYY-MM-DD"));

                    // txtCECompaniaDepto.trigger("change");
                    txtCECompaniaNSS.val(response.data.lstDatos[0].nss);
                    // cboCECompaniaTipoFormula.val(response.data.lstDatos[0].tipo_formula_imss);
                    // cboCECompaniaTipoFormula.trigger("change");
                    txtCECompaniaJefeInmediatoDescripcion.val(response.data.lstDatos[0].nombreJefeInmediato);
                    // txtCECompaniaDeptoDescripcion.val(response.data.lstDatos[0].desc_depto);
                    txtCECompaniaContrato.val(moment(response.data.lstDatos[0].fecha_contrato).format("YYYY-MM-DD"));
                    lblCECompaniaAltaEnElSistema.html(moment(response.data.lstDatos[0].fecha_alta).format("DD/MM/YYYY"));
                    lblCECompaniaAntiguedad.html(response.data.lstDatos[0].antiguedad);

                    let idUsuarioEK = +response.data.lstDatos[0].idUsuarioEK > 0 ? response.data.lstDatos[0].idUsuarioEK : "N/A";
                    txtCEUltimaModificacionIDEK.val(idUsuarioEK);
                    txtCEUltimaModificacionNombreUsuario.val(response.data.lstDatos[0].usuarioModificacion);
                    //#endregion

                    //#region DATOS PERU
                    if (txtEmpresa.val() == 6 && response.data.objDatosPeru != null) {
                        checkAfiliadoEps.prop("checked", (response.data.objDatosPeru.afiliado_eps == "S" ? true : false)); //afiliado_eps:
                        checkAfiliadoEps.trigger("change", ['noCargar']);

                        cboAfps.val(response.data.objDatosPeru.codafp); //codafp:
                        cboAfps.trigger("change");

                        $("#cboMensualDiario").val(response.data.objDatosPeru.ctbasico); //ctbasico:
                        cboMensualDiario.trigger("change");

                        $("#txtBasico").val(response.data.objDatosPeru.basico); //basico:
                        $("#txtAsigFamiliar").val(response.data.objDatosPeru.asigfam); //asigfam:
                        // fondopens:
                        cboTipoTrabajador.fillComboBox('FillComboTiposTrabajadores', null, '-- Seleccionar --', () => {
                            $("#cboTipoTrabajador").val(response.data.objDatosPeru.tipotrab); //tipotrab:
                        });

                        $("#chkEsSaludVida").prop("checked", response.data.objDatosPeru.essaludvida == 1 ? true : false); //essaludvida:
                        $("#chkCEPeruNOPDT").prop("checked", response.data.objDatosPeru.nopdt == 1 ? true : false); //nopdt:
                        $("#chkCEPeruOpcion01").prop("checked", response.data.objDatosPeru.opcion01 == 1 ? true : false); //opcion01:
                        $("#chkCEPeruOpcion02").prop("checked", response.data.objDatosPeru.opcion02 == 1 ? true : false); //opcion02:
                        $("#txtCEPeruOpcionA").val(response.data.objDatosPeru.opciona); //opciona:
                        $("#txtCEPeruOpcionB").val(response.data.objDatosPeru.opcionb); //opcionb:
                        $("#chkCEPeruNoCalculo").prop("checked", response.data.objDatosPeru.nocalculo == 1 ? true : false); //nocalculo:
                        $("#chkCEPeruAfectoQuinta").prop("checked", response.data.objDatosPeru.afectoquinta == 1 ? true : false); //afectoquinta:
                        $("#txtCEPeruCentroRiesgo").val(response.data.objDatosPeru.codsctr); //codsctr:
                        $("#cboSctrPension").val(response.data.objDatosPeru.sctr_pension); //sctr_pension:
                        $("#cboSctrPension").trigger("change");

                        $("#cboSctrSalud").val(response.data.objDatosPeru.sctr_salud); //sctr_salud:
                        $("#cboSctrSalud").trigger("change");

                        $("#cboCEPeruComisionMixta").val(response.data.objDatosPeru.es_comision_mixta); //es_comision_mixta:
                        $("#cboCEPeruComisionMixta").trigger("change");

                        $("#chkRegimenAlternativo").prop("checked", (response.data.objDatosPeru.trabajador_regimen == "S" ? true : false));
                        $("#chkJornadaMaxima").prop("checked", (response.data.objDatosPeru.trabajador_jornada == "S" ? true : false));
                        $("#chkHorarioNocturno").prop("checked", (response.data.objDatosPeru.trabajador_nocturno == "S" ? true : false));
                        $("#chkOtrosIngresos5ta").prop("checked", (response.data.objDatosPeru.otros_ingresos == "S" ? true : false));
                        $("#chkSindicalizado").prop("checked", (response.data.objDatosPeru.sindicalizado == "S" ? true : false));
                        $("#chkDiscapacitado").prop("checked", (response.data.objDatosPeru.discapacitado == "S" ? true : false));
                        $("#chkDomiciliado").prop("checked", (response.data.objDatosPeru.domiciliado == "S" ? true : false));
                        $("#chkAfiliacionAseguraPension").prop("checked", (response.data.objDatosPeru.aseg_pension));
                        $("#chkRentasExoneradas").prop("checked", (response.data.objDatosPeru.rentas_exoneradas));
                        $("#cboBancoCTS").val(response.data.objDatosPeru.bancocts);
                        $("#cboBancoCTS").trigger("change");

                        $("#txtCTS").val(response.data.objDatosPeru.ctacts);
                        $("#cboRegimenLaboral").val(response.data.objDatosPeru.regimen_laboral);
                        $("#cboRegimenLaboral").trigger("change");

                        cboSituacionEps.fillComboBox('GetSituacion', { afiliado: checkAfiliadoEps.prop('checked') }, '-- Seleccionar --', () => {
                            $("#cboSituacionEps").val(response.data.objDatosPeru.situacion); //situacion:
                            cboSituacionEps.trigger("change");
                            $("#cboRucEps").val(response.data.objDatosPeru.ruceps); //ruceps:
                            cboRucEps.trigger("change");
                            $("#cboRucEps").trigger("change");
                            $("#txtCEPeruNoAfiliados").val(response.data.objDatosPeru.factor_familiar_eps); //factor_familiar_eps:
                        });
                    }
                    //#endregion

                    if (response.data.esDiana) {
                        // txtCEDatosEmpleadoFechaIngreso.attr("disabled", false);
                        botonVerRequisiciones.attr("disabled", false);
                        txtCECompaniaRequisicion.attr("disabled", false);
                        chkTabuladorLibre.attr("disabled", false);
                        // cboTipoNomina.attr("disabled", false);

                    } else {
                        // cboTipoNomina.attr("disabled", true);

                        if (esReingresoEmpleado) {
                            botonVerRequisiciones.attr("disabled", false);
                            txtCECompaniaRequisicion.attr("disabled", false);
                            chkTabuladorLibre.attr("disabled", false);

                        } else {
                            botonVerRequisiciones.attr("disabled", true);
                            txtCECompaniaRequisicion.attr("disabled", true);
                            chkTabuladorLibre.attr("disabled", true);
                        }
                    }
                    // txtTabuladorLibreBono.attr("disabled", true);
                    // cboTipoNomina.attr("disabled", true);

                    if (response.data.lstDatos[0].lstCCOcultar.includes(response.data.lstDatos[0].cc_contable) && inputPermisoOcultarTabulador.val() == 1) {
                        nomina_tab.attr("href", "#tab");
                        home_tab.click();
                    } else {
                        if (!response.data.esSalarios) {
                            nomina_tab.attr("href", "#tab");
                            home_tab.click();
                        } else {
                            nomina_tab.attr("href", "#tabNomina");
                            home_tab.click();
                        }
                    }

                    // fncHabilitarDeshabilitarCtrlsMdl(false);
                    fncBorderDefault();
                    btnCEEmpleado.attr("data-esActualizar", 1);
                    btnCEEmpleado.attr("data-esReingresoEmpleado", esReingresoEmpleado);
                    btnCEEmpleado.html("Actualizar");
                    fncGetFamiliares();
                    fncGetContratos();
                    fncGetUniformes();
                    fncGetArchivoExamenMedico();
                    fncGetTabuladores(response.data.lstDatos[0].idCategoria);
                    fncGetDocs(claveEmpleado, null);
                    fncGetFotoEmpleado();

                    var column = dtTabuladores.column(1);

                    // if (esReingresoEmpleado) {
                    //     column.visible(false);

                    // } else {
                    //     column.visible(true);

                    // }

                    mdlCrearEditarEmpleadoEK.modal("show");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => { Alert2Error(error.message); });
        }

        function fncEliminarEmpleado() {

        }

        function fncReingresarEmpleado() {
            let requisicion_id = +$('#inputFolioRequisicion').val();

            if (requisicion_id != '' && !isNaN(requisicion_id)) {
                axios.post('ReingresarEmpleado', { clave_empleado: _clave_empleado, requisicion_id })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            Alert2Exito('Se ha guardado la información.');
                            btnFiltroBuscar.click();
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            } else {
                Alert2Warning('Debe capturar un ID de requisición válido.');
                return;
            }
        }

        function fncGetEmpleadosEK() {
            let objFiltro = fncGetObjFiltro();
            axios.post("GetEmpleadosEK", objFiltro).then(response => {
                let { success, items, message } = response.data;
                if (success) {

                    //#region FILL DATATABLE
                    dtEmpleados.clear();
                    dtEmpleados.rows.add(response.data.lstEmpleadosEK);
                    dtEmpleados.draw();
                    var table = $('#tblRH_REC_Empleados').DataTable();
                    let flag = (chkFiltroEsPendiente.val() == "B");
                    $('#tblRH_REC_Empleados').DataTable().column(10).visible(flag);

                    if (response.data.lstEmpleadosEK.length > 0) {

                        if (response.data.lstEmpleadosEK[0].estatus_empleado == 'P') {
                            btnEnviarCorreos.css("display", "inline");
                        } else {
                            btnEnviarCorreos.css("display", "none");
                        }

                        if (response.data.lstEmpleadosEK[0].esDiana) {
                            txtTabuladorLibreBono.attr("disabled", false);
                            // cboTipoNomina.attr("disabled", false);
                        } else {
                            txtTabuladorLibreBono.attr("disabled", true);
                            // cboTipoNomina.attr("disabled", true);

                        }
                    }

                    // cboTipoNomina.attr("disabled", false);


                    // if (chkFiltroEsPendiente.val() == 'B' || chkFiltroEsPendiente.val() == 'P') {
                    //     table.column(7).visible(false);
                    // } else {
                    //     table.column(7).visible(true);
                    // }
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetObjFiltro() {
            let obj = new Object();
            obj = {
                // cc: cboFiltroCC.val(),
                cc: getValoresMultiples('#cboFiltroCC'),
                lstEstatusEmpleado: chkFiltroEsPendiente.val()
            }
            return obj;
        }

        function fncCrearEditarEmpleado() {
            fncBorderDefault();
            // let lstFams = dtFamiliares.rows().data().toArray();
            let lstFams = fncGetObjFamilia();

            if (lstFams.length == 0 && lblCEDatosEmpleadoClaveEmpleado.data('clvempleado') == 0) {
                Alert2Warning("Es necesario ingresar familiares nuevos al empledo");
                return "";
            }

            let objUniformes = dtUniformes.rows().data().toArray();

            for (let i = 0; i < lstFams.length; i++) {
                lstFams[i].fecha_de_nacimiento = moment(lstFams[i].fecha_de_nacimiento);
            }

            let objEmpleadoDTO = fncGetObjCEEmpleado();
            let objGenContactoDTO = fncGetObjCEGenContactoDTO();
            let objBeneficiariosDTO = fncGetObjCEBeneficiarioDTO();
            let objContEmergenciasDTO = fncGetObjCECasoAccidente();
            let objCompaniaDTO = fncGetObjCECompania();
            let objTabulador = fncGetObjTabulador();
            let objContrato = fncGetObjContrato();
            let objDatosPeru = txtEmpresa.val() == 6 ? fncGetObjDatosPeru() : null;

            if (objEmpleadoDTO != "" && objGenContactoDTO != "" && objCompaniaDTO != "" && objContEmergenciasDTO != "" && objTabulador != "" && objBeneficiariosDTO != "" && objDatosPeru != "") {
                axios.post("CrearEditarInformacionEmpleado", {
                    objEmpleadoDTO: objEmpleadoDTO,
                    objGenContactoDTO: objGenContactoDTO,
                    objBeneficiariosDTO: objBeneficiariosDTO,
                    objContEmergenciasDTO: objContEmergenciasDTO,
                    objCompaniaDTO: objCompaniaDTO,
                    lstFamiliares: lstFams,
                    objUniforme: objUniformes[0],
                    objTabulador: objTabulador,
                    objContrato: objContrato,
                    objDatosPeru: objDatosPeru
                }).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        if (imgFotoEmpleadoHead.attr('src') != undefined && imgFotoEmpleadoHead.attr('src') != "") {
                            if (!objEmpleadoDTO.esReingresoEmpleado && !objEmpleadoDTO.esActualizar) {
                                fncGuardarFotoEmpleado(response.data.claveEmpleado);
                            }
                        }

                        fncLimpiarMdlCEEmpleado();
                        fncBorderDefault();
                        mdlCrearEditarEmpleadoEK.modal("hide");
                        fncGetEmpleadosEK();

                        btnCEEmpleado.attr("data-id", 0);
                        btnCEEmpleado.attr("data-esActualizar", 0);
                        Alert2Exito(message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGetObjCEEmpleado() {
            let strMensajeError = "";

            //if (cboCandidatosAprobados.val() == "") { $("#select2-cboCandidatosAprobados-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCEDatosEmpleadoNombre.val() == "") { txtCEDatosEmpleadoNombre.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtCEDatosEmpleadoApePaterno.val() == "") { txtCEDatosEmpleadoApePaterno.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }

            if (cboCEDatosEmpleadoPaisNac.val() == "" || cboCEDatosEmpleadoPaisNac.val() == null) { $("#select2-cboCEDatosEmpleadoPaisNac-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (cboCEDatosEmpleadoEstadoNac.val() == "" || cboCEDatosEmpleadoEstadoNac.val() == null) { $("#select2-cboCEDatosEmpleadoEstadoNac-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (cboCEDatosEmpleadoLugarNac.val() == "" || cboCEDatosEmpleadoLugarNac.val() == null) { $("#select2-cboCEDatosEmpleadoLugarNac-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }

            if (txtCEDatosEmpleadoFechaNacimiento.val() == "") { txtCEDatosEmpleadoFechaNacimiento.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (txtCEDatosEmpleadoFechaIngreso.val() == "") { txtCEDatosEmpleadoFechaIngreso.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (txtCEDatosEmpleadoLocalidadNac.val() == "") { txtCEDatosEmpleadoLocalidadNac.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }

            if (txtEmpresa.val() != 6 && txtEmpresa.val() != 3) {
                if (txtCEDatosEmpleadoRFC.val() == "") { txtCEDatosEmpleadoRFC.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; } else if (txtCEDatosEmpleadoRFC.val().length != 13) { txtCEDatosEmpleadoRFC.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
                if (txtCEDatosEmpleadoRFC.val().length != 13) { txtCEDatosEmpleadoRFC.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; } else if (txtCEDatosEmpleadoRFC.val().length != 13) { txtCEDatosEmpleadoRFC.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }

                if (txtCEDatosEmpleadoCPCIF.val() == "") { txtCEDatosEmpleadoCPCIF.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }

                if (txtCEDatosEmpleadoCURP.val() == "") { txtCEDatosEmpleadoCURP.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
                if (txtCEDatosEmpleadoCURP.val().length != 18) { txtCEDatosEmpleadoCURP.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }

            }

            if (cboCEDatosEmpleadoSexo.val() == "") { $("#select2-cboCEDatosEmpleadoSexo-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }

            if (txtEmpresa.val() == 6 && txtCEDatosEmpleadoCUSPP.val() == "") { txtCEDatosEmpleadoCUSPP.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (txtEmpresa.val() == 6 && cboCEDatosEmpleadoDepartamentoNac.val() == "") { $("#select2-cboCEDatosEmpleadoDepartamentoNac-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    id: btnCEEmpleado.attr("data-id"),
                    clave_empleado: +btnCEEmpleado.attr("data-esActualizar") == 1 ? lblCEDatosEmpleadoClaveEmpleado.data('clvempleado') : 0,
                    nombre: txtCEDatosEmpleadoNombre.val(),
                    ape_paterno: txtCEDatosEmpleadoApePaterno.val(),
                    ape_materno: txtCEDatosEmpleadoApeMaterno.val(),
                    fecha_nac_string: txtCEDatosEmpleadoFechaNacimiento.val(),
                    clave_pais_nac: cboCEDatosEmpleadoPaisNac.val(),
                    clave_departamento_nac_PERU: cboCEDatosEmpleadoDepartamentoNac.val(),
                    clave_estado_nac: cboCEDatosEmpleadoEstadoNac.val(),
                    clave_ciudad_nac: cboCEDatosEmpleadoLugarNac.val(),
                    localidad_nacimiento: txtCEDatosEmpleadoLocalidadNac.val(),
                    fecha_alta: txtCEDatosEmpleadoFechaIngreso.val(),
                    str_fecha_alta: txtCEDatosEmpleadoFechaIngreso.val(),
                    sexo: cboCEDatosEmpleadoSexo.val(),
                    rfc: txtCEDatosEmpleadoRFC.val(),
                    CPCIF: txtCEDatosEmpleadoCPCIF.val(),
                    curp: txtCEDatosEmpleadoCURP.val(),
                    cuspp: txtCEDatosEmpleadoCUSPP.val(),
                    idCandidato: cboCandidatosAprobados.val(),
                    esActualizar: +btnCEEmpleado.attr("data-esActualizar") == 1 ? true : false,
                    esReingresoEmpleado: btnCEEmpleado.attr("data-esReingresoEmpleado") == 'true' ? true : false,
                    bancoNomina: chkTarjetaNomina.prop('checked') ? cboBancoNomina.val() : null,
                    num_cta_pago: chkTarjetaNomina.prop('checked') ? txtTarjetaNomina.val() : null,
                    num_cta_fondo_aho: chkTarjetaNomina.prop('checked') ? txtClaveInterbancaria.val() : null,
                    tabulador: txtNumeroTabulador.val(),
                    tipo_nomina: cboTipoNomina.val(),
                    desc_puesto: txtCECompaniaActividades.val()
                }
                return obj;
            }
        }

        function fncGetObjCEGenContactoDTO() {
            let strMensajeError = "";

            if (txtCEGenContactoCalle.val() == "") { txtCEGenContactoCalle.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (txtCEGenContactoNumExterior.val() == "") { txtCEGenContactoNumExterior.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (txtCEGenContactoColonia.val() == "") { txtCEGenContactoColonia.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (cboCEGenContactoEstado.val() == "") { $("#select2-cboCEGenContactoEstado-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (cboCEGenContactoCiudad.val() == "") { $("#select2-cboCEGenContactoCiudad-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (cboCEGenContactoEmpleadoPais.val() == "") { $("#select2-cboCEGenContactoEmpleadoPais-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }

            if (txtEmpresa.val() == 6) {
                if (cboCEGenContactoDepartamentoNac.val() == "") { $("#select2-cboCEGenContactoDepartamentoNac-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
                if (txtCEGenContactoTelCasa.val() == "" || txtCEGenContactoTelCasa.val().length != 9) { txtCEGenContactoTelCasa.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            } else if (txtEmpresa.val() == 3) {
                if (txtCEGenContactoTelCasa.val() == "" || txtCEGenContactoTelCasa.val().length != 10) { txtCEGenContactoTelCasa.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            } else {
                if (txtCEGenContactoCP.val() == "") { txtCEGenContactoCP.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
                if (txtCEGenContactoTelCasa.val() == "" || txtCEGenContactoTelCasa.val().length != 10) { txtCEGenContactoTelCasa.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            }

            if (cboCEGenContactoTipoSangre.val() == "" || cboCEGenContactoTipoSangre.val() == null) { $("#select2-cboCEGenContactoTipoSangre-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (txtCEGenContactoEmail.val() == "") { txtCEGenContactoEmail.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    estado_civil: cboCEGenContactoEstadoCivil.val(),
                    fecha_planta: txtCEGenContactoFechaPlanta.val(),
                    ocupacion: txtCEGenContactoEstudios.val(),
                    ocupacion_abrev: txtCEGenContactoAbreviatura.val(),
                    num_cred_elector: txtCEGenContactoCredElector.val(),
                    num_dni: txtCEGenContactoDNI.val(), //PERU
                    cedula_cuidadania: txtCEGenContactoCedulaCuidadania.val(), //COL
                    domicilio: txtCEGenContactoCalle.val(),
                    numero_exterior: txtCEGenContactoNumExterior.val(),
                    numero_interior: txtCEGenContactoNumInterior.val(),
                    colonia: txtCEGenContactoColonia.val(),
                    estado_dom: cboCEGenContactoEstado.val(),
                    ciudad_dom: cboCEGenContactoCiudad.val(),
                    codigo_postal: txtCEGenContactoCP.val(),
                    tel_casa: txtCEGenContactoTelCasa.val(),
                    tel_cel: txtCEGenContactoTelCelular.val(),
                    email: txtCEGenContactoEmail.val(),
                    tipo_casa: cboCEGenContactoTipoCasa.val(),
                    tipo_sangre: cboCEGenContactoTipoSangre.val(),
                    alergias: txtCEGenContactoAlergias.val(),
                    pais_dom: cboCEGenContactoEmpleadoPais.val(),
                    PERU_departamento_dom: cboCEGenContactoDepartamentoNac.val(),
                }
                return obj;
            }
        }

        function fncGetObjCEBeneficiarioDTO() {
            let strMensajeError = "";

            let obj = new Object();

            if (cboCEBeneficiarioEstado.val() == "") { $("#select2-cboCEBeneficiarioEstado-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (cboCEBeneficiarioCiudad.val() == "") { $("#select2-cboCEBeneficiarioCiudad-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (cboCEBeneficiarioEmpleadoPais.val() == "") { $("#select2-cboCEBeneficiarioEmpleadoPais-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }

            if (txtEmpresa.val() == 6) {
                if (cboCEBeneficiarioDepartamentoNac.val() == "") { $("#select2-cboCEBeneficiarioDepartamentoNac-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            }

            obj = {
                parentesco_ben: cboCEBeneficiarioParentesco.val(),
                fecha_nac_ben: txtCEBeneficiarioFechaNacimiento.val(),
                paterno_ben: txtCEBeneficiarioApePaterno.val(),
                materno_ben: txtCEBeneficiarioApeMaterno.val(),
                nombre_ben: txtCEBeneficiarioNombre.val(),
                estado_ben: cboCEBeneficiarioEstado.val(),
                ciudad_ben: cboCEBeneficiarioCiudad.val(),
                colonia_ben: txtCEBeneficiarioColonia.val(),
                codigo_postal_ben: txtCEBeneficiarioCP.val(),
                domicilio_ben: txtCEBeneficiarioDomicilio.val(),
                num_ext_ben: txtCEBeneficiarioNumExt.val(),
                num_int_ben: txtCEBeneficiarioNumInt.val(),
                cel: txtCEBeneficiarioCel.val(),
                pais_ben: cboCEBeneficiarioEmpleadoPais.val(),
                ben_num_dni: txtCEBeneficiarioCURP.val(),
                PERU_departamento_ben: cboCEBeneficiarioDepartamentoNac.val(),
            }
            return obj;
        }

        function fncGetObjCECasoAccidente() {
            if (txtEmpresa.val() == 6) {
                if (txtCECasoAccidenteNombre.val() != "") {
                    if (txtCECasoAccidenteTelefono.val() == "" || txtCECasoAccidenteTelefono.val().length != 9) {
                        Alert2Warning("Favor de ingresar la información de contacto de emergencia, el # debe de ser de 9 dígitos");
                        return "";
                    }
                }
            } else {
                if (txtCECasoAccidenteNombre.val() != "") {
                    if (txtCECasoAccidenteTelefono.val() == "" || txtCECasoAccidenteTelefono.val().length != 10) {
                        Alert2Warning("Favor de ingresar la información de contacto de emergencia, el # debe de ser de 10 dígitos");
                        return "";
                    }
                }
            }

            let obj = new Object();
            obj = {
                en_accidente_nombre: txtCECasoAccidenteNombre.val(),
                en_accidente_telefono: txtCECasoAccidenteTelefono.val(),
                en_accidente_direccion: txtCECasoAccidenteDomicilio.val()
            }
            return obj;
        }

        function fncGetObjFamilia() {
            let rowData = tblRH_REC_Familiares.DataTable().rows().data().toArray();


            let familia = new Array();

            for (let index = 0; index < rowData.length; index++) {
                if (rowData[index].esNuevo == 1) {
                    familia.push(rowData[index]);
                }
            }

            return familia;
        }

        function fncGetObjTabulador() {
            let rowData = tblRH_REC_Tabuladores.DataTable().rows().data().toArray();

            if (cboTipoNomina.val() == null) {
                //if (txtCECompaniaDepto.val() == "") { $("#select2-txtCECompaniaDepto-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }

                $("#cboTipoNomina").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios";
                return "";

            }

            for (let index = 0; index <= rowData.length; index++) {
                if (index == rowData.length) {
                    if (rowData[index - 1] != undefined) {
                        if (rowData[index - 1].esNuevoTabulador) {
                            let objTabulador = {
                                tabulador: +txtNumeroTabulador.val(),
                                puesto: +txtCECompaniaPuestoDescripcion.data('puesto'),
                                salario_base: +rowData[index - 1].salario_base,
                                complemento: +rowData[index - 1].complemento,
                                bono_de_zona: +rowData[index - 1].bono_de_zona,
                                year: moment(rowData[index - 1].fechaRealNomina).format('YYYY'),
                                motivoCambio: +rowData[index - 1].motivoCambio,
                                fechaAplicaCambio: moment(rowData[index - 1].fechaAplicaCambio, 'DD/MM/YYYY')
                            }

                            return objTabulador;
                        }
                    }

                }
            }

            return null;
        }

        function fncGetObjContrato() {
            let rowData = tblContratos.DataTable().rows().data().toArray();


            for (let index = 0; index <= rowData.length; index++) {
                if (index == rowData.length) {
                    if (rowData[index - 1] != undefined) {
                        if (rowData[index - 1].esNuevoContrato) {
                            let objTabulador = {
                                id_contrato_empleado: 0,
                                clave_empleado: 0,
                                clave_duracion: rowData[index - 1].clave_duracion,
                                fecha: rowData[index - 1].fechaString,
                                fecha_aplicacion: rowData[index - 1].fecha_aplicacion,
                                fechaString: rowData[index - 1].fechaString,
                                fecha_aplicacionString: rowData[index - 1].fecha_aplicacionString,
                                fecha_fin: rowData[index - 1].fecha_fin,

                                desc_duracion: rowData[index - 1].desc_duracion,
                                esNuevoContrato: rowData[index - 1].esNuevoContrato
                            }

                            return objTabulador;
                        }
                    }

                }
            }

            return null;
        }

        function fncGetObjDatosPeru() {
            let obj = new Object();
            let strMensajeError = "";

            // if ($("#cboMensualDiario").val() == "" || $("#cboMensualDiario").val() == "-- Seleccionar --") { cboMensualDiario.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; };
            // if ($("#txtBasico").val() == "" || $("#txtBasico").val() == "-- Seleccionar --") { txtBasico.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; };
            // if ($("#txtAsigFamiliar").val() == "" || $("#txtAsigFamiliar").val() == "-- Seleccionar --") { txtAsigFamiliar.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; };
            // if ($("#cboTipoTrabajador").val() == "" || $("#cboTipoTrabajador").val() == "-- Seleccionar --") { cboTipoTrabajador.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; };
            // if ($("#cboSituacionEps").val() == "" || $("#cboSituacionEps").val() == "-- Seleccionar --") { cboSituacionEps.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; };
            // if ($("#cboRucEps").val() == "" || $("#cboRucEps").val() == "-- Seleccionar --") { $("#cboRucEps").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; };
            // if ($("#txtCEPeruOpcionA").val() == "" || $("#txtCEPeruOpcionA").val() == "-- Seleccionar --") { $("#txtCEPeruOpcionA").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; };
            // if ($("#txtCEPeruOpcionB").val() == "" || $("#txtCEPeruOpcionB").val() == "-- Seleccionar --") { $("#txtCEPeruOpcionB").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; };
            // if ($("#txtCEPeruCentroRiesgo").val() == "" || $("#txtCEPeruCentroRiesgo").val() == "-- Seleccionar --") { $("#txtCEPeruCentroRiesgo").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; };
            // if ($("#cboSctrPension").val() == "" || $("#cboSctrPension").val() == "-- Seleccionar --") { $("#cboSctrPension").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; };
            // if ($("#cboSctrSalud").val() == "" || $("#cboSctrSalud").val() == "-- Seleccionar --") { $("#cboSctrSalud").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; };
            // if ($("#cboCEPeruComisionMixta").val() == "" || $("#cboCEPeruComisionMixta").val() == "-- Seleccionar --") { $("#cboCEPeruComisionMixta").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; };
            // if ($("#txtCEPeruNoAfiliados").val() == "" || $("#txtCEPeruNoAfiliados").val() == "-- Seleccionar --") { $("#txtCEPeruNoAfiliados").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; };
            // if ($("#cboBancoCTS").val() == "" || $("#cboBancoCTS").val() == "-- Seleccionar --") { $("#cboBancoCTS").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; };
            // if ($("#txtCTS").val() == "" || $("#txtCTS").val() == "-- Seleccionar --") { $("#txtCTS").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; };
            // if ($("#cboRegimenLaboral").val() == "" || $("#cboRegimenLaboral").val() == "-- Seleccionar --") { $("#cboRegimenLaboral").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; };

            if (strMensajeError == "") {
                obj = {
                    codafp: cboAfps.val(),
                    aporobli: 0,
                    topeseguro: 0,
                    comisionra: 0,
                    ctbasico: $("#cboMensualDiario").val() ?? 0,
                    basico: $("#txtBasico").val() ?? 0,
                    asigfam: $("#txtAsigFamiliar").val() ?? 0,
                    fondopens: 0,
                    tipotrab: $("#cboTipoTrabajador").val(),
                    situacion: $("#cboSituacionEps").val(),
                    essaludvida: $("#chkEsSaludVida").prop("checked"),
                    ruceps: $("#cboRucEps").val(),
                    nopdt: $("#chkCEPeruNOPDT").prop("checked"),
                    opcion01: $("#chkCEPeruOpcion01").prop("checked"),
                    opcion02: $("#chkCEPeruOpcion02").prop("checked"),
                    opciona: $("#txtCEPeruOpcionA").val(),
                    opcionb: $("#txtCEPeruOpcionB").val(),
                    nocalculo: $("#chkCEPeruNoCalculo").prop("checked"),
                    afectoquinta: $("#chkCEPeruAfectoQuinta").prop("checked"),
                    afiliado_eps: (checkAfiliadoEps.prop("checked") ? "S" : "N"),
                    codsctr: $("#txtCEPeruCentroRiesgo").val(),
                    sctr_pension: $("#cboSctrPension").val() ?? 0,
                    sctr_salud: $("#cboSctrSalud").val(),
                    es_comision_mixta: $("#cboCEPeruComisionMixta").val(),
                    factor_familiar_eps: $("#txtCEPeruNoAfiliados").val(),
                    trabajador_regimen: $("#chkRegimenAlternativo").prop("checked") ? "S" : "N",
                    trabajador_jornada: $("#chkJornadaMaxima").prop("checked") ? "S" : "N",
                    trabajador_nocturno: $("#chkHorarioNocturno").prop("checked") ? "S" : "N",
                    otros_ingresos: $("#chkOtrosIngresos5ta").prop("checked") ? "S" : "N",
                    sindicalizado: $("#chkSindicalizado").prop("checked") ? "S" : "N",
                    discapacitado: $("#chkDiscapacitado").prop("checked") ? "S" : "N",
                    domiciliado: $("#chkDomiciliado").prop("checked") ? "S" : "N",
                    aseg_pension: $("#chkAfiliacionAseguraPension").prop("checked"),
                    rentas_exoneradas: $("#chkRentasExoneradas").prop("checked"),
                    bancocts: $("#cboBancoCTS").val(),
                    ctacts: $("#txtCTS").val(),
                    regimen_laboral: $("#cboRegimenLaboral").val(),
                }
            } else {
                Alert2Warning(strMensajeError);
                return "";
            }

            return obj;
        }

        function fncGetObjCECompania() {
            let strMensajeError = "";

            if (txtCECompaniaRequisicion.val() == "") { txtCECompaniaRequisicion.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (selectRegistroPatronal.val() == "") { $("#select2-selectRegistroPatronal-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (txtCECompaniaAutoriza.val() == "") { txtCECompaniaAutoriza.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (txtCECompaniaUsuarioResg.val() == "") { txtCECompaniaUsuarioResg.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (txtCECompaniaDepto.val() == "") { $("#select2-txtCECompaniaDepto-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }

            if (txtEmpresa.val() == 6) {
                if (dateCECompaniaContratoFin.val() == "") { dateCECompaniaContratoFin.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
                if (cboCETipoEmpleado.val() == "" && cboRegimenLaboral.val() == 8 /* CONSTRUCCIÓN CIVIL */) {
                    cboCETipoEmpleado.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios";
                }
            } else if (txtEmpresa.val() == 3) {
            }
            else {
                if (txtCECompaniaNSS.val() == "") { txtCECompaniaNSS.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
                if (txtCECompaniaNSS.val().length != 11) { txtCECompaniaNSS.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            }
            // if (cboCECompaniaTipoFormula.val() == "") { $("#select2-cboCECompaniaTipoFormula-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }
            if (txtCECompaniaPlantilla.val() == "") { txtCECompaniaPlantilla.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios"; }


            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    requisicion: txtCECompaniaRequisicion.val(),
                    id_regpat: selectRegistroPatronal.val(),
                    cc_contable: txtCECompaniaCCDescripcion.data('cc'),
                    puesto: txtCECompaniaPuestoDescripcion.data('puesto'),
                    idTabuladorDet: cboCECompaniaPuestoCategoria.val(),
                    duracion_contrato: selectTipoContrato.val(),
                    fecha_fin: moment(dateCECompaniaContratoFin.val()).format("DD/MM/YYYY"),
                    jefe_inmediato: txtCECompaniaJefeInmediatoDescripcion.data('jefe_inmediato'),
                    autoriza: txtCECompaniaAutoriza.val(),
                    usuario_compras: txtCECompaniaUsuarioResg.val(),
                    chkSindicato: chkCECompaniaSindicato.prop("checked"),
                    clave_depto: txtCECompaniaDepto.val(),
                    nss: txtCECompaniaNSS.val(),
                    // tipo_formula_imss: cboCECompaniaTipoFormula.val(),
                    fecha_contrato: txtCECompaniaContrato.val(),
                    actividades: txtCECompaniaActividades.val(),
                    tipoEmpleado: cboCETipoEmpleado.val()
                }
                return obj;
            }
        }

        function fncLimpiarMdlCEEmpleado() {
            $("input[type='text']").val("");
            $("input[type='number']").val("");
            $("input[type='date']").val("");
            $("textarea").val("");

            txtCEDatosEmpleadoFechaIngreso.val(moment().format("DD/MM/YYYY"));
            // txtCEDatosEmpleadoFechaIngreso.val(moment().format("YYYY-MM-DD"));

            txtCEDatosEmpleadoFechaIngreso.prop('disabled', false);

            cboCandidatosAprobados[0].selectedIndex = 0;
            cboCandidatosAprobados.trigger("change");

            cboCEDatosEmpleadoPaisNac[0].selectedIndex = 0;
            cboCEDatosEmpleadoPaisNac.trigger("change");

            cboCEDatosEmpleadoDepartamentoNac[0].selectedIndex = 0;
            cboCEDatosEmpleadoDepartamentoNac.change();

            cboCEDatosEmpleadoEstadoNac[0].selectedIndex = 0;
            cboCEDatosEmpleadoEstadoNac.trigger("change");

            cboCEDatosEmpleadoLugarNac[0].selectedIndex = 0;
            cboCEDatosEmpleadoLugarNac.trigger("change");

            cboCEDatosEmpleadoSexo[0].selectedIndex = 0;
            cboCEDatosEmpleadoSexo.trigger("change");

            cboCEGenContactoEmpleadoPais[0].selectedIndex = 0;
            cboCEGenContactoEmpleadoPais.trigger("change");

            cboCEGenContactoEstado[0].selectedIndex = 0;
            cboCEGenContactoEstado.trigger("change");

            cboCEGenContactoCiudad[0].selectedIndex = 0;
            cboCEGenContactoCiudad.trigger("change");

            cboCEGenContactoTipoCasa[0].selectedIndex = 0;
            cboCEGenContactoTipoCasa.trigger("change");

            cboCEGenContactoTipoSangre[0].selectedIndex = 0;
            cboCEGenContactoTipoSangre.trigger("change");

            cboCEBeneficiarioParentesco[0].selectedIndex = 0;
            cboCEBeneficiarioParentesco.trigger("change");

            cboCEBeneficiarioEmpleadoPais[0].selectedIndex = 0;
            cboCEBeneficiarioEmpleadoPais.trigger("change");

            cboCEBeneficiarioEstado[0].selectedIndex = 0;
            cboCEBeneficiarioEstado.trigger("change");

            cboCEBeneficiarioCiudad[0].selectedIndex = 0;
            cboCEBeneficiarioCiudad.trigger("change");

            cboCECompaniaPuestoCategoria.val("");
            cboCECompaniaPuestoCategoria.trigger("change");

            // cboCECompaniaTipoFormula[0].selectedIndex = 0;
            // cboCECompaniaTipoFormula.trigger("change");

            lblCECompaniaAltaEnElSistema.text("");
            lblCECompaniaAntiguedad.text("");

            // INFORMACION BANCARIA
            chkTarjetaNomina.prop('checked', false);
            cboBancoNomina.prop('disabled', true);
            txtTarjetaNomina.prop('disabled', true);
            txtClaveInterbancaria.prop('disabled', true);
            cboBancoNomina.val('');
            txtTarjetaNomina.val('');
            txtClaveInterbancaria.val('');

            // INFORMACION TABULADOR
            cboTipoNomina.val('');
            cboTipoNomina.trigger('change');

            //IMAGEN EMPLEADO
            imgFotoEmpleado.attr("src", null);
            imgFotoEmpleadoHead.attr("src", null);

            //#region PERU

            cboAfps.val(""); //codafp:
            cboAfps.trigger("change");

            $("#cboMensualDiario").val(""); //ctbasico:
            cboMensualDiario.trigger("change");

            $("#cboTipoTrabajador").val(""); //tipotrab:
            $("#cboTipoTrabajador").trigger("change");

            $("#chkEsSaludVida").prop("checked", false); //essaludvida:
            $("#chkCEPeruNOPDT").prop("checked", false); //nopdt:
            $("#chkCEPeruOpcion01").prop("checked", false); //opcion01:
            $("#chkCEPeruOpcion02").prop("checked", false); //opcion02:

            $("#chkCEPeruNoCalculo").prop("checked", false); //nocalculo:
            $("#chkCEPeruAfectoQuinta").prop("checked", false); //afectoquinta:
            $("#cboSctrPension").val(""); //sctr_pension:
            $("#cboSctrPension").trigger("change");

            $("#cboSctrSalud").val(""); //sctr_salud:
            $("#cboSctrSalud").trigger("change");

            $("#cboCEPeruComisionMixta").val(""); //es_comision_mixta:
            $("#cboCEPeruComisionMixta").trigger("change");

            $("#chkRegimenAlternativo").prop("checked", false);
            $("#chkJornadaMaxima").prop("checked", false);
            $("#chkHorarioNocturno").prop("checked", false);
            $("#chkOtrosIngresos5ta").prop("checked", false);
            $("#chkSindicalizado").prop("checked", false);
            $("#chkDiscapacitado").prop("checked", false);
            $("#chkDomiciliado").prop("checked", false);
            $("#chkAfiliacionAseguraPension").prop("checked", false);
            $("#chkRentasExoneradas").prop("checked", false);
            $("#cboBancoCTS").val("");
            $("#cboBancoCTS").trigger("change");

            $("#cboRegimenLaboral").val("");
            $("#cboRegimenLaboral").trigger("change");
            $("#cboRucEps").val(""); //ruceps:
            $("#cboRucEps").trigger("change");
            //#endregion
        }

        function fncBorderDefault() {
            //#region DATOS EMPLEADO
            $("#select2-cboCandidatosAprobados-container").css('border', '1px solid #CCC');
            txtCEDatosEmpleadoNombre.css('border', '1px solid #CCC');
            txtCEDatosEmpleadoApePaterno.css('border', '1px solid #CCC');
            $("#select2-cboCEDatosEmpleadoPaisNac-container").css('border', '1px solid #CCC');
            $("#select2-cboCEDatosEmpleadoEstadoNac-container").css('border', '1px solid #CCC');
            $("#select2-cboCEDatosEmpleadoLugarNac-container").css('border', '1px solid #CCC');
            $("#select2-cboCEDatosEmpleadoDepartamentoNac-container").css('border', '1px solid #CCC');
            txtCEDatosEmpleadoFechaNacimiento.css('border', '1px solid #CCC');
            txtCEDatosEmpleadoFechaIngreso.css('border', '1px solid #CCC');
            txtCEDatosEmpleadoLocalidadNac.css('border', '1px solid #CCC');
            txtCEDatosEmpleadoRFC.css('border', '1px solid #CCC');
            txtCEDatosEmpleadoCPCIF.css("border", "1px solid #CCC")
            txtCEDatosEmpleadoCURP.css('border', '1px solid #CCC');
            txtCEDatosEmpleadoCUSPP.css('border', '1px solid #CCC');
            txtCEGenContactoTelCasa.css('border', '1px solid #CCC');
            $("#select2-cboCEDatosEmpleadoSexo-container").css('border', '1px solid #CCC');
            $("#select2-cboCEGenContactoTipoSangre-container").css('border', '1px solid #CCC');

            //#endregion

            //#region GENERALES Y CONTACTO
            txtCEGenContactoCalle.css('border', '1px solid #CCC');
            txtCEGenContactoNumExterior.css('border', '1px solid #CCC');
            txtCEGenContactoNumInterior.css('border', '1px solid #CCC');
            txtCEGenContactoColonia.css('border', '1px solid #CCC');
            txtCEGenContactoCP.css('border', '1px solid #CCC');
            txtCEGenContactoTelCasa.css('border', '1px solid #CCC');
            $("#select2-cboCEGenContactoEstado-container").css('border', '1px solid #CCC');
            $("#select2-cboCEGenContactoCiudad-container").css('border', '1px solid #CCC');
            $("#select2-cboCEGenContactoEmpleadoPais-container").css('border', '1px solid #CCC');
            $("#select2-cboCEGenContactoDepartamentoNac-container").css('border', '1px solid #CCC');

            //#endregion

            //#region COMPAÑIA
            txtCECompaniaRequisicion.css('border', '1px solid #CCC');
            txtCECompaniaAutoriza.css('border', '1px solid #CCC');
            txtCECompaniaUsuarioResg.css('border', '1px solid #CCC');
            txtCECompaniaDepto.css('border', '1px solid #CCC');
            txtCECompaniaNSS.css('border', '1px solid #CCC');
            // $("#select2-cboCECompaniaTipoFormula-container").css('border', '1px solid #CCC');
            txtCECompaniaPlantilla.css('border', '1px solid #CCC');
            dateCECompaniaContratoFin.css('border', '1px solid #CCC');

            //#endregion

            //#region BENEFICIARIO
            $("#select2-cboCEBeneficiarioEstado-container").css('border', '1px solid #CCC');
            $("#select2-cboCEBeneficiarioCiudad-container").css('border', '1px solid #CCC');
            $("#select2-cboCEBeneficiarioEmpleadoPais-container").css('border', '1px solid #CCC');
            $("#select2-cboCEBeneficiarioDepartamentoNac-container").css('border', '1px solid #CCC');
            //#endregion

            //#region DATOS PERU
            $("#cboMensualDiario").css('border', '1px solid #CCC');
            $("#txtBasico").css('border', '1px solid #CCC');
            $("#txtAsigFamiliar").css('border', '1px solid #CCC');
            $("#cboTipoTrabajador").css('border', '1px solid #CCC');
            $("#cboSituacionEps").css('border', '1px solid #CCC');
            $("#cboRucEps").css('border', '1px solid #CCC');
            $("#txtCEPeruOpcionA").css('border', '1px solid #CCC');
            $("#txtCEPeruOpcionB").css('border', '1px solid #CCC');
            $("#txtCEPeruCentroRiesgo").css('border', '1px solid #CCC');
            $("#cboSctrPension").css('border', '1px solid #CCC');
            $("#cboSctrSalud").css('border', '1px solid #CCC');
            $("#cboCEPeruComisionMixta").css('border', '1px solid #CCC');
            $("#txtCEPeruNoAfiliados").css('border', '1px solid #CCC');
            $("#cboBancoCTS").css('border', '1px solid #CCC');
            $("#txtCTS").css('border', '1px solid #CCC');
            $("#cboRegimenLaboral").css('border', '1px solid #CCC');
            //#endregion
        }

        function autorizacionMultiple(claveEmpleados) {
            $.post('AutorizacionMultiple', { claveEmpleados: claveEmpleados }).then(response => {
                if (response.success) {
                    Alert2Exito("Se han autorizado los empleados.");
                    fncGetEmpleadosEK();
                } else {
                    Alert2Error(response.message);
                }
            }, error => {
                swal('Alerta!', 'Ocurrió un problema al enviar la solicitud al servidor', 'warning');
            });
        }

        function fncExportarPendientes() {
            //#region SE OBTIENE LA CLAVE DEL EMPLEADO SELECCIONADO
            let arrRowsChecked = [];
            let rowsChecked = $("#tblRH_REC_Empleados").DataTable().column(0).checkboxes.selected();
            $.each(rowsChecked, function (index, claveEmpleado) {
                arrRowsChecked.push(claveEmpleado)
            });
            //#endregion

            //#region 
            let obj = new Object();
            obj = {
                _lstClaveEmpleados: arrRowsChecked
            }
            axios.post("ExportarExcel", obj).then(response => {
                let { success, items, message } = response.data;
                // console.log(response.data);
                if (success) {
                    Alert2Exito("SUCCESS.");
                    window.location = 'GetArchivoLayoutAltas';
                    AlertaGeneral("Confirmación", "¡Archivo Generado Correctamente!");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
            //#endregion
        }

        function fncGetDatosCandidatoAprobado(idCandidatoAprobado) {
            let obj = new Object();
            obj = {
                idCandidatoAprobado: idCandidatoAprobado
            }
            axios.post("GetDatosCandidatoAprobado", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    // console.log(response.data.objCandidato);
                    cboCEDatosEmpleadoPaisNac.val(response.data.objCandidato.pais);
                    cboCEDatosEmpleadoPaisNac.trigger("change");
                    cboCEDatosEmpleadoDepartamentoNac.val(response.data.objCandidato.PERU_departamento);
                    cboCEDatosEmpleadoDepartamentoNac.change();
                    cboCEDatosEmpleadoEstadoNac.val(response.data.objCandidato.estado);
                    cboCEDatosEmpleadoEstadoNac.trigger("change");
                    cboCEDatosEmpleadoLugarNac.val(response.data.objCandidato.municipio);
                    cboCEDatosEmpleadoLugarNac.trigger("change");
                    txtCEDatosEmpleadoLocalidadNac.val(cboCEDatosEmpleadoLugarNac.find('option:selected').text());
                    txtCEDatosEmpleadoFechaNacimiento.val(moment(response.data.objCandidato.fechaNacimiento).format("DD/MM/YYYY"));
                    // txtCEDatosEmpleadoRFC.val(response.data.objCandidato.RFC);
                    // txtCEDatosEmpleadoRFC.trigger("keyup");
                    // txtCEDatosEmpleadoRFC.trigger("change");

                    txtCEGenContactoTelCasa.val(response.data.objCandidato.telefono);
                    txtCEGenContactoTelCelular.val(response.data.objCandidato.celular);
                    cboCEDatosEmpleadoSexo.val(response.data.objCandidato.sexo ?? "");
                    cboCEDatosEmpleadoSexo.trigger("change");
                    txtCEDatosEmpleadoCURP.val(response.data.objCandidato.CURP);
                    txtCEDatosEmpleadoCURP.trigger("change");

                    cboCEGenContactoEstadoCivil.val(response.data.objCandidato.estadoCivil ?? "--Seleccione--");
                    cboCEGenContactoEstadoCivil.trigger("change");
                    // cboCEGenContactoEstado.val(response.data.objCandidato.estado);
                    // cboCEGenContactoEstado.trigger('change');
                    // cboCEGenContactoCiudad.val(response.data.objCandidato.municipio);
                    txtCEGenContactoEmail.val(response.data.objCandidato.correo);
                    txtCEGenContactoEstudios.val(response.data.objCandidato.escolaridad);
                    txtCECompaniaDepto.val(response.data.objCandidato.departamentoDesc);
                    txtCECompaniaDepto.trigger("change");
                    txtCEDatosEmpleadoCUSPP.val(response.data.objCandidato.cuspp);

                    //GENERALES CANDIDATO 
                    cboCEGenContactoEmpleadoPais.val(response.data.objCandidato.pais);
                    cboCEGenContactoEmpleadoPais.trigger("change");
                    cboCEGenContactoDepartamentoNac.val(response.data.objCandidato.PERU_departamento);
                    cboCEGenContactoDepartamentoNac.change();
                    cboCEGenContactoEstado.val(response.data.objCandidato.estado);
                    cboCEGenContactoEstado.trigger("change");
                    cboCEGenContactoCiudad.val(response.data.objCandidato.municipio);
                    cboCEGenContactoCiudad.trigger("change");

                    //#region AUTOLLENAR CAMPOS BENEFICIARIO.
                    cboCEBeneficiarioEmpleadoPais.val(response.data.objCandidato.pais);
                    cboCEBeneficiarioEmpleadoPais.trigger("change");
                    cboCEBeneficiarioDepartamentoNac.val(response.data.objCandidato.PERU_departamento);
                    cboCEBeneficiarioDepartamentoNac.change();
                    cboCEBeneficiarioEstado.val(response.data.objCandidato.estado);
                    cboCEBeneficiarioEstado.trigger("change");
                    cboCEBeneficiarioCiudad.val(response.data.objCandidato.municipio);
                    cboCEBeneficiarioCiudad.trigger("change");

                    fncCECompaniaFillDepartamentos(response.data.objCandidato.ccSolicitud, false, null);
                    fncGetIDUsuarioEK();

                    addRows(tblRH_REC_ArchivosExamenMedico, response.data.objCandidato.documentosMedicos);

                    fncGetDocs(null, idCandidatoAprobado);


                    txtCEDatosEmpleadoFechaIngreso.attr('min', moment().subtract(7, 'd').format("YYYY-MM-DD"));
                    txtCEDatosEmpleadoFechaIngreso.attr('max', moment().add(7, 'd').format("YYYY-MM-DD"));
                    // txtCEDatosEmpleadoFechaIngreso.val(moment().format("YYYY-MM-DD"));
                    txtCEDatosEmpleadoFechaIngreso.val(moment().format("DD/MM/YYYY"));


                    if (response.data.objCandidato.esDiana) {
                        txtCEDatosEmpleadoFechaIngreso.attr("disabled", false);
                        txtTabuladorLibreBono.attr("disabled", false);

                    } else {
                        txtTabuladorLibreBono.attr("disabled", true);

                    }

                    // cboTipoNomina.attr("disabled", false);
                    btnCEFotoEmpleado.css("display", "none");
                    //#endregion

                    fncFocus("txtCEDatosEmpleadoApePaterno");
                } else {
                    Alert2Error(message);
                }
                lblTitleCrearEditarEmpleadoEK.html("Crear empleado");
                if (txtEmpresa.val() == "6") {

                    dtFamiliares.column(6).visible(true); //COLUMNA DNI (PERU)
                    dtFamiliares.column(7).visible(false); //COLUMNA Cedula (COL)

                } else if (txtEmpresa.val() == "3") {

                    dtFamiliares.column(6).visible(false); //COLUMNA DNI (PERU)
                    dtFamiliares.column(7).visible(true); //COLUMNA Cedula (COL)
                }
                else {

                    dtFamiliares.column(6).visible(false); //COLUMNA DNI (PERU)
                    dtFamiliares.column(7).visible(false); //COLUMNA Cedula (COL)
                }

            }).catch(error => Alert2Error(error.message));
        }

        function fncCambiarEstatusEmpleado(cveEmpleado, estatus) {
            axios.post("CambiarEstatusEmpleado", { claveEmpleado: cveEmpleado, status: estatus }).then(response => {
                let { success, items, message } = response.data;
                if (success) { //🚗🚓🚕🛺🚙🚌🚐🚎🚑🚒🚚🚛 nascar hillo
                    Alert2Exito("Se ha registrado con éxito el empleado.");
                    fncGetEmpleadosEK();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCheckEmpleado() {
            let obj = {
                curp: txtCEDatosEmpleadoCURP.val(),
                rfc: txtCEDatosEmpleadoRFC.val(),
                nss: txtCECompaniaNSS.val()
            }

            axios.post("CheckEmpleado", obj).then(response => {
                let { success, items, message } = response.data;
                if (!success) {
                    if (mdlCrearEditarEmpleadoEK.hasClass('in')) {
                        // mdlCrearEditarEmpleadoEK.modal("hide");
                        Alert2Warning(message);
                    }

                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region CRUD FAMILIARES
        function initTblFamiliares() {
            dtFamiliares = tblRH_REC_Familiares.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'strParentesco', title: "PARENTESCO" },
                    {
                        title: "NOMBRE",
                        render: function (data, type, row) {
                            let nombre = row.nombre;
                            let apePaterno = row.apellido_paterno;
                            let apeMaterno = row.apellido_materno;
                            return nombre + " " + apePaterno + " " + apeMaterno;
                        }
                    },
                    {
                        data: 'fecha_de_nacimiento', title: 'FECHA NACIMIENTO',
                        render: function (data, type, row) {
                            if (data != null && data != '' && data != undefined) {
                                return moment(data).format('DD/MM/YYYY');
                            } else {
                                return null;
                            }
                        }
                    },
                    {
                        data: 'estado_civil', title: 'ESTADO CIVIL',
                        render: function (data, type, row) {
                            if (txtEmpresa.val() == 6 && data == "Union Libre") {
                                return "Conviviente";

                            } else {
                                return data;
                            }
                        }
                    },
                    { data: 'genero', title: 'GENERO' },
                    { data: 'grado_de_estudios', title: 'ESTUDIOS' },
                    {
                        data: 'num_dni', title: 'DNI',
                        render: function (data, type, row) {
                            return data ?? "";
                        }
                    },
                    {
                        data: 'cedula_cuidadania', title: 'CÉDULA DE CUIDADANÍA',
                        render: function (data, type, row) {
                            return data ?? "";
                        }
                    },
                    {
                        render: function (data, type, row) {
                            let btnActualizar = `<button class="btn btn-warning actualizarFamiliar btn-xs"><i class="far fa-edit"></i></button>&nbsp;`;
                            let btnEliminar = `<button class="btn btn-danger eliminarFamiliar btn-xs"><i class="far fa-trash-alt"></i></button>`;
                            return btnActualizar + btnEliminar;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_Familiares.on('click', '.actualizarFamiliar', function () {
                        let rowData = dtFamiliares.row($(this).closest('tr')).data();

                        cboCEFamiliarParentesco.val(rowData.parentesco);
                        cboCEFamiliarParentesco.trigger("change");

                        cboCEFamiliarGenero.val(rowData.genero);
                        cboCEFamiliarGenero.trigger("change");

                        txtCEFamiliarFechaNacimiento.val(moment(rowData.fecha_de_nacimiento).format("YYYY-MM-DD"));

                        chkCEFamiliarVive.prop("checked", rowData.vive == 'S');
                        chkCEFamiliarVive.trigger("change");

                        txtCEFamiliarNombre.val(rowData.nombre);
                        txtCEFamiliarApePaterno.val(rowData.apellido_paterno);
                        txtCEFamiliarApeMaterno.val(rowData.apellido_materno);

                        let endStr = "";

                        if (rowData.estado_civil != null) {
                            let words = rowData.estado_civil.split(" ");
                            let lastInx = words.length - 1;

                            for (let i = 0; i < words.length; i++) {
                                const element = words[i];
                                let capFL = words[i][0];
                                let restStr = words[i].toLowerCase().substring(1, words[i].length);

                                endStr += capFL + restStr;

                                if (i != lastInx) {
                                    endStr += " ";
                                }
                            }
                        }


                        cboCEFamiliarEstadoCivil.val(endStr);
                        cboCEFamiliarEstadoCivil.trigger("change");

                        txtCEFamiliarGradoEstudios.val(rowData.grado_de_estudios);

                        chkCEFamiliarBeneficiario.prop("checked", rowData.beneficiario == "S" ? true : false);
                        chkCEFamiliarBeneficiario.trigger("change");

                        chkCEFamiliarTrabaja.prop("checked", rowData.trabaja == "S" ? true : false);
                        chkCEFamiliarTrabaja.trigger("change");

                        chkCEFamiliarEstudia.prop("checked", rowData.estudia == "S" ? true : false);
                        chkCEFamiliarEstudia.trigger("change");

                        txtCEFamiliarComentarios.val(rowData.comentarios);

                        txtCEFamiliarDNI.val(rowData.num_dni);

                        btnCEFamiliar.text("Actualizar");
                        btnCEFamiliar.data("id", rowData.id);
                        btnCEEmpleado.data("id-ek", rowData.idEKFam);
                        btnCEFamiliar.attr("data-esActualizar", 1);
                        if (rowData.esNuevo == 1) {
                            btnCEFamiliar.data('esNuevo', 2);
                            btnCEFamiliar.data('row', tblRH_REC_Familiares.DataTable().row($(this).closest('tr')).index());
                        } else {
                            btnCEFamiliar.data('esNuevo', 0);
                            btnCEFamiliar.data('row', tblRH_REC_Familiares.DataTable().row($(this).closest('tr')).index());
                        }
                        mdlCrearEditarFamiliar.modal("show");
                    });

                    tblRH_REC_Familiares.on('click', '.eliminarFamiliar', function () {
                        let rowData = dtFamiliares.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarFamiliar(rowData.id, rowData.clave_empleado, rowData.esNuevo, tblRH_REC_Familiares.DataTable().row($(this).closest('tr')).index()));
                    });
                },
                drawCallback: function (row, data, index) {

                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { "width": "20%", "targets": 1 }
                ],
            });
        }

        function fncGetFamiliares() {
            let clave_empleado = lblCEDatosEmpleadoClaveEmpleado.data('clvempleado');
            let obj = new Object();
            obj = {
                clave_empleado: parseFloat(clave_empleado)
            }
            axios.post("GetFamiliares", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtFamiliares.clear();
                    dtFamiliares.rows.add(response.data.lstFamiliaresEK);
                    dtFamiliares.draw();

                    if (txtEmpresa.val() == "6") {

                        dtFamiliares.column(6).visible(true); //COLUMNA DNI (PERU)
                        dtFamiliares.column(7).visible(false); //COLUMNA Cedula (COL)

                    } else if (txtEmpresa.val() == "3") {

                        dtFamiliares.column(6).visible(false); //COLUMNA DNI (PERU)
                        dtFamiliares.column(7).visible(true); //COLUMNA Cedula (COL)
                    }
                    else {

                        dtFamiliares.column(6).visible(false); //COLUMNA DNI (PERU)
                        dtFamiliares.column(7).visible(false); //COLUMNA Cedula (COL)
                    }
                    //#endregion
                } else {
                    dtFamiliares.draw();

                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetContratos() {
            bntAddContrato.prop('disabled', false);
            let clave_empleado = lblCEDatosEmpleadoClaveEmpleado.data('clvempleado');
            let obj = new Object();
            obj = {
                clave_empleado: parseFloat(clave_empleado)
            }
            axios.post("GetContratos", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    if (response.data.data.length > 0) {

                        let fechaFin = null;
                        let last = response.data.data[0];
                        let lstdate = moment(last.fecha_aplicacion);
                        //#region SWITCH CALCULAR FECHAS

                        switch (last.clave_duracion) {
                            case 2:
                                fechaFin = lstdate.add(3, 'M');

                                break;
                            case 3:
                                fechaFin = null;

                                break;
                            case 4:
                                fechaFin = null;

                                break;
                            case 5:
                                fechaFin = lstdate.add(28, 'd')//AddDays(28);

                                break;
                            case 7:
                                fechaFin = lstdate.add(6, 'M')//AddMonths(6);

                                break;
                            case 8:
                                fechaFin = lstdate.add(13, 'd')//AddDays(13);

                                break;
                            case 9:
                                fechaFin = lstdate.add(1, 'M')//AddMonths(1);

                                break;
                            case 10:
                                fechaFin = lstdate.add(15, 'd')//AddDays(15);

                                break;
                            case 11:
                                fechaFin = lstdate.add(16, 'd')//AddDays(16);

                                break;
                            case 12:
                                fechaFin = lstdate.add(9, 'd')//AddDays(9);

                                break;
                            case 13:
                                fechaFin = lstdate.add(25, 'd')//AddDays(25);

                                break;
                            case 14:
                                fechaFin = lstdate.add(4, 'd')//AddDays(4);

                                break;
                            case 16:
                                fechaFin = lstdate.add(5, 'd')//AddDays(5);

                                break;
                            case 18:
                                fechaFin = lstdate.add(2, 'M')//AddMonths(2);

                                break;
                            case 19:
                                fechaFin = lstdate.add(3, 'y')//AddYears(3);
                                break;
                            case 20:
                                fechaFin = lstdate.add(12, 'M');
                                break;

                            default:
                                break;
                        }

                        if (fechaFin != null) {
                            // txtAddContratoFechaApli.val(lstdate);
                            txtAddContratoFechaMod.val(moment().format("YYYY-MM-DD"));

                            txtAddContratoFechaInicio.val(moment(last.fecha_aplicacion).format("YYYY-MM-DD"));
                            txtAddContratoFechaFin.val(moment(fechaFin).format("YYYY-MM-DD"));

                            txtAddContratoFechaApli.val(fechaFin.add(1, 'd').format("YYYY-MM-DD"));

                            txtAddContratoFechaInicio.attr("display", "initial");
                            txtAddContratoFechaFin.attr("display", "initial");


                        } else {
                            if (last.clave_duracion == 3 || last.clave_duracion == 4) {
                                txtAddContratoFechaFin.attr("display", "none");
                            }

                            txtAddContratoFechaMod.val(moment().format("YYYY-MM-DD"));
                            txtAddContratoFechaApli.val(moment().format("YYYY-MM-DD"));

                        }
                        //#endregion

                    } else {
                        txtAddContratoFechaMod.val(moment().format("YYYY-MM-DD"));
                        txtAddContratoFechaApli.val(moment().format("YYYY-MM-DD"));

                        txtAddContratoFechaInicio.attr("display", "none");
                        txtAddContratoFechaFin.attr("display", "none");
                    }

                    dtContratos.clear();
                    dtContratos.rows.add(response.data.data);
                    dtContratos.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearEditarFamiliar() {
            let objFamiliarDTO = fncGetObjCEFamiliar();

            let lstFam = new Array();
            lstFam.push(objFamiliarDTO);

            if (objFamiliarDTO.esBeneficiario) {
                cboCEBeneficiarioParentesco.val(objFamiliarDTO.parentesco);
                cboCEBeneficiarioParentesco.select2().change();
                cboCEBeneficiarioParentesco.select2({ width: "100%" });
                txtCEBeneficiarioFechaNacimiento.val(objFamiliarDTO.fecha_de_nacimiento);
                txtCEBeneficiarioNombre.val(objFamiliarDTO.nombre);
                txtCEBeneficiarioApePaterno.val(objFamiliarDTO.apellido_paterno);
                txtCEBeneficiarioApeMaterno.val(objFamiliarDTO.apellido_materno);
                txtCEBeneficiarioCURP.val(objFamiliarDTO.num_dni);
            }

            switch (objFamiliarDTO.esNuevo) {
                case 0:
                    {
                        $.post('CrearEditarInformacionFamiliar',
                            {
                                objFamiliarDTO
                            }).then(response => {
                                if (response.success) {
                                    let row = btnCEFamiliar.data('row');
                                    let rowData = tblRH_REC_Familiares.DataTable().row(row).data();

                                    rowData.nombre = objFamiliarDTO.nombre;
                                    rowData.apellido_paterno = objFamiliarDTO.apellido_paterno;
                                    rowData.apellido_materno = objFamiliarDTO.apellido_materno;
                                    rowData.fecha_de_nacimiento = objFamiliarDTO.fecha_de_nacimiento;
                                    rowData.parentesco = objFamiliarDTO.parentesco;
                                    rowData.strParentesco = objFamiliarDTO.strParentesco;
                                    rowData.grado_de_estudios = objFamiliarDTO.grado_de_estudios;
                                    rowData.estado_civil = objFamiliarDTO.estado_civil;
                                    rowData.estudia = objFamiliarDTO.estudia;
                                    rowData.esEstudia = objFamiliarDTO.esEstudia;
                                    rowData.genero = objFamiliarDTO.genero;
                                    rowData.vive = objFamiliarDTO.vive;
                                    rowData.esVive = objFamiliarDTO.esVive;
                                    rowData.beneficiario = objFamiliarDTO.beneficiario;
                                    rowData.esBeneficiario = objFamiliarDTO.esBeneficiario;
                                    rowData.trabaja = objFamiliarDTO.trabaja;
                                    rowData.esTrabaja = objFamiliarDTO.esTrabaja;
                                    rowData.comentarios = objFamiliarDTO.comentarios;
                                    rowData.esNuevo = 0;

                                    tblRH_REC_Familiares.find('tbody').find('tr:eq(' + row + ')').find('td:eq(0)').text(rowData.strParentesco);
                                    tblRH_REC_Familiares.find('tbody').find('tr:eq(' + row + ')').find('td:eq(1)').text(rowData.apellido_paterno + ' ' + rowData.apellido_materno + ' ' + rowData.nombre);
                                    tblRH_REC_Familiares.find('tbody').find('tr:eq(' + row + ')').find('td:eq(2)').text(moment(rowData.fecha_de_nacimiento).format('DD/MM/YYYY'));
                                    tblRH_REC_Familiares.find('tbody').find('tr:eq(' + row + ')').find('td:eq(3)').text(rowData.estado_civil);
                                    tblRH_REC_Familiares.find('tbody').find('tr:eq(' + row + ')').find('td:eq(4)').text(rowData.genero);
                                    tblRH_REC_Familiares.find('tbody').find('tr:eq(' + row + ')').find('td:eq(5)').text(rowData.grado_de_estudios);

                                    fncGetFamiliares();
                                    mdlCrearEditarFamiliar.modal('hide');
                                    Alert2Exito("Registro editado con exito");
                                } else {
                                    Alert2Error(response.message);
                                }
                            }, error => {
                                Alert2Error(error.message);
                            });
                    }
                    break;
                case 1:
                    {
                        addRowsSinLimpiar(tblRH_REC_Familiares, lstFam);
                        mdlCrearEditarFamiliar.modal("hide");
                    }
                    break;
                case 2:
                    {
                        let row = btnCEFamiliar.data('row');
                        let rowData = tblRH_REC_Familiares.DataTable().row(row).data();

                        rowData.nombre = objFamiliarDTO.nombre;
                        rowData.apellido_paterno = objFamiliarDTO.apellido_paterno;
                        rowData.apellido_materno = objFamiliarDTO.apellido_materno;
                        rowData.fecha_de_nacimiento = objFamiliarDTO.fecha_de_nacimiento;
                        rowData.parentesco = objFamiliarDTO.parentesco;
                        rowData.strParentesco = objFamiliarDTO.strParentesco;
                        rowData.grado_de_estudios = objFamiliarDTO.grado_de_estudios;
                        rowData.estado_civil = objFamiliarDTO.estado_civil;
                        rowData.estudia = objFamiliarDTO.estudia;
                        rowData.esEstudia = objFamiliarDTO.esEstudia;
                        rowData.genero = objFamiliarDTO.genero;
                        rowData.vive = objFamiliarDTO.vive;
                        rowData.esVive = objFamiliarDTO.esVive;
                        rowData.beneficiario = objFamiliarDTO.beneficiario;
                        rowData.esBeneficiario = objFamiliarDTO.esBeneficiario;
                        rowData.trabaja = objFamiliarDTO.trabaja;
                        rowData.esTrabaja = objFamiliarDTO.esTrabaja;
                        rowData.comentarios = objFamiliarDTO.comentarios;
                        rowData.esNuevo = 1;

                        tblRH_REC_Familiares.find('tbody').find('tr:eq(' + row + ')').find('td:eq(0)').text(rowData.strParentesco);
                        tblRH_REC_Familiares.find('tbody').find('tr:eq(' + row + ')').find('td:eq(1)').text(rowData.apellido_paterno + ' ' + rowData.apellido_materno + ' ' + rowData.nombre);
                        tblRH_REC_Familiares.find('tbody').find('tr:eq(' + row + ')').find('td:eq(2)').text(moment(rowData.fecha_de_nacimiento).format('DD/MM/YYYY'));
                        tblRH_REC_Familiares.find('tbody').find('tr:eq(' + row + ')').find('td:eq(3)').text(rowData.estado_civil);
                        tblRH_REC_Familiares.find('tbody').find('tr:eq(' + row + ')').find('td:eq(4)').text(rowData.genero);
                        tblRH_REC_Familiares.find('tbody').find('tr:eq(' + row + ')').find('td:eq(5)').text(rowData.grado_de_estudios);

                        if (txtEmpresa.val() == 6) {
                            rowData.num_dni = objFamiliarDTO.num_dni;
                            tblRH_REC_Familiares.find('tbody').find('tr:eq(' + row + ')').find('td:eq(6)').text(rowData.num_dni);

                        }

                        if (txtEmpresa.val() == 3) {
                            rowData.cedula_cuidadania = objFamiliarDTO.cedula_cuidadania;
                            tblRH_REC_Familiares.find('tbody').find('tr:eq(' + row + ')').find('td:eq(6)').text(rowData.cedula_cuidadania);
                        }

                        mdlCrearEditarFamiliar.modal('hide');
                    }
                    break;
            }



            // if (objFamiliarDTO != "") {
            // axios.post("CrearEditarInformacionFamiliar", objFamiliarDTO).then(response => {
            //     let { success, items, message } = response.data;
            //     if (success) {
            //         if (objFamiliarDTO.esActualizar == 1) {
            //             dtFamiliares.rows().every(function () {
            //                 var d = this.data();
            //                 if (objFamiliarDTO.idEKFam == 0) {
            //                     if (d.id == objFamiliarDTO.id) {
            //                         d.nombre = objFamiliarDTO.nombre;
            //                         d.apellido_paterno = objFamiliarDTO.apellido_paterno;
            //                         d.apellido_materno = objFamiliarDTO.apellido_materno;
            //                         d.fecha_de_nacimiento = objFamiliarDTO.fecha_de_nacimiento;
            //                         d.strParentesco = objFamiliarDTO.strParentesco;
            //                         d.parentesco = objFamiliarDTO.parentesco;
            //                         d.grado_de_estudios = objFamiliarDTO.grado_de_estudios;
            //                         d.estado_civil = objFamiliarDTO.estado_civil;
            //                         d.estudia = !objFamiliarDTO.esEstudia ? "N" : "S";
            //                         d.genero = objFamiliarDTO.genero;
            //                         d.vive = !objFamiliarDTO.esVive ? "N" : "S";
            //                         d.beneficiario = !objFamiliarDTO.esBeneficiario ? "N" : "S";
            //                         d.trabaja = !objFamiliarDTO.esTrabaja ? "N" : "S";
            //                         d.comentarios = objFamiliarDTO.comentarios;
            //                         this.invalidate(); // invalidate the data DataTables has cached for this row
            //                     }
            //                 } else {
            //                     if (d.id == objFamiliarDTO.id || d.idEKFam == objFamiliarDTO.idEKFam) {
            //                         d.nombre = objFamiliarDTO.nombre;
            //                         d.apellido_paterno = objFamiliarDTO.apellido_paterno;
            //                         d.apellido_materno = objFamiliarDTO.apellido_materno;
            //                         d.fecha_de_nacimiento = objFamiliarDTO.fecha_de_nacimiento;
            //                         d.strParentesco = objFamiliarDTO.strParentesco;
            //                         d.parentesco = objFamiliarDTO.parentesco;
            //                         d.grado_de_estudios = objFamiliarDTO.grado_de_estudios;
            //                         d.estado_civil = objFamiliarDTO.estado_civil;
            //                         d.estudia = !objFamiliarDTO.esEstudia ? "N" : "S";
            //                         d.genero = objFamiliarDTO.genero;
            //                         d.vive = !objFamiliarDTO.esVive ? "N" : "S";
            //                         d.beneficiario = !objFamiliarDTO.esBeneficiario ? "N" : "S";
            //                         d.trabaja = !objFamiliarDTO.esTrabaja ? "N" : "S";
            //                         d.comentarios = objFamiliarDTO.comentarios;
            //                         this.invalidate(); // invalidate the data DataTables has cached for this row
            //                     }
            //                 }

            //             });
            //         }

            //         // Draw once all updates are done
            //         if (objFamiliarDTO.esActualizar == 0) {
            //             dtFamiliares.rows.add([items]);

            //         }

            //         dtFamiliares.draw();

            //         if (objFamiliarDTO.esBeneficiario) { // CHECAR SI ESTA LLENADO EN EK PARA NO ACTULIZAR LOS DATOS YA LLENADOS
            //             cboCEBeneficiarioParentesco.val(objFamiliarDTO.parentesco);
            //             cboCEBeneficiarioParentesco.trigger("change");

            //             txtCEBeneficiarioFechaNacimiento.val(moment(objFamiliarDTO.fecha_de_nacimiento ?? "").format("YYYY-MM-DD"));

            //             txtCEBeneficiarioApePaterno.val(objFamiliarDTO.apellido_paterno ?? "");

            //             txtCEBeneficiarioApeMaterno.val(objFamiliarDTO.apellido_materno ?? "");

            //             txtCEBeneficiarioNombre.val(objFamiliarDTO.nombre ?? "");
            //         }

            //         Alert2Exito("Se ha registrado con éxito al familiar.");
            //         mdlCrearEditarFamiliar.modal("hide");
            //         //fncGetFamiliares(parseFloat(lblCEDatosEmpleadoClaveEmpleado.attr("data-clvEmpleado")));
            //     } else {
            //         Alert2Error(message);
            //     }
            // }).catch(error => Alert2Error(error.message));
            // }
        }

        function fncGetObjCEFamiliar() {
            let obj = new Object();
            let lstParentescos = [];
            lstParentescos[1] = "Padre";
            lstParentescos[2] = "Madre";
            lstParentescos[3] = "Conyuge";
            lstParentescos[4] = "Hijo";
            lstParentescos[5] = "Hermano";
            lstParentescos[11] = "Otro";

            obj = {
                id: btnCEFamiliar.data("id"),
                idEKFam: btnCEEmpleado.data("id-ek") ?? 0,
                clave_empleado: lblCEDatosEmpleadoClaveEmpleado.data('clvempleado'),
                nombre: txtCEFamiliarNombre.val(),
                apellido_paterno: txtCEFamiliarApePaterno.val(),
                apellido_materno: txtCEFamiliarApeMaterno.val(),
                fecha_de_nacimiento: txtCEFamiliarFechaNacimiento.val(),
                parentesco: cboCEFamiliarParentesco.val(),
                strParentesco: lstParentescos[Number(cboCEFamiliarParentesco.val())],
                grado_de_estudios: txtCEFamiliarGradoEstudios.val(),
                estado_civil: cboCEFamiliarEstadoCivil.val(),
                estudia: chkCEFamiliarEstudia.prop("checked") ? 'S' : 'N',
                esEstudia: chkCEFamiliarEstudia.prop("checked"),
                genero: cboCEFamiliarGenero.val(),
                vive: chkCEFamiliarVive.prop("checked") ? 'S' : 'N',
                esVive: chkCEFamiliarVive.prop("checked"),
                beneficiario: chkCEFamiliarBeneficiario.prop("checked") ? 'S' : 'N',
                esBeneficiario: chkCEFamiliarBeneficiario.prop("checked"),
                trabaja: chkCEFamiliarTrabaja.prop("checked") ? 'S' : 'N',
                esTrabaja: chkCEFamiliarTrabaja.prop("checked"),
                comentarios: txtCEFamiliarComentarios.val(),
                esActualizar: btnCEFamiliar.attr("data-esActualizar"),
                esNuevo: btnCEFamiliar.data('esNuevo'),
                num_dni: txtCEFamiliarDNI.val(),
                cedula_cuidadania: txtCEFamiliarCedula.val(),
                asignacionEscolar: cboCEAsignacionEscolar.val()
            }

            return obj;
        }

        function fncLimpiarMdlCEFamiliares() {
            cboCEFamiliarParentesco[0].selectedIndex = 0;
            cboCEFamiliarParentesco.trigger("change");

            txtCEFamiliarFechaNacimiento.val("");

            cboCEFamiliarGenero[0].selectedIndex = 0;
            cboCEFamiliarGenero.trigger("change");

            chkCEFamiliarVive.prop("checked", true);
            chkCEFamiliarVive.trigger("change");

            chkCEFamiliarBeneficiario.prop("checked", false);
            chkCEFamiliarBeneficiario.trigger("change");

            chkCEFamiliarTrabaja.prop("checked", false);
            chkCEFamiliarTrabaja.trigger("change");

            chkCEFamiliarEstudia.prop("checked", false);
            chkCEFamiliarEstudia.trigger("change");

            cboCEFamiliarEstadoCivil[0].selectedIndex = 0;
            cboCEFamiliarEstadoCivil.trigger("change");

            cboCEFamiliarEstadoCivil[0].selectIndex = 0;
            cboCEFamiliarEstadoCivil.trigger("change");

            txtCEFamiliarComentarios.val("");
            $("#mdlCrearEditarFamiliar input[type='text']").val("");
        }

        function fncEliminarFamiliar(idFamiliar, cveEmpleado, esNuevo, row) {
            if (esNuevo == 1) {
                tblRH_REC_Familiares.DataTable().row(row).remove().draw();
            } else {
                let idEKFam = tblRH_REC_Familiares.DataTable().row(row).data();

                $.post('EliminarFamiliar',
                    {
                        idFamiliar: idEKFam.idEKFam,
                        clave_empleado: cveEmpleado
                    }).then(response => {
                        if (response.success) {
                            tblRH_REC_Familiares.DataTable().row(row).remove().draw();
                        } else {
                            Alert2Error(response.message);
                        }
                    }, error => {
                        Alert2Error(error.message);
                    });
                // let obj = new Object();
                // obj = {
                //     idFamiliar: idFamiliar,
                //     clave_empleado: cveEmpleado

                // }
                // axios.post("EliminarFamiliar", obj).then(response => {
                //     let { success, items, message } = response.data;
                //     if (success) {
                //         Alert2Exito("Se ha eliminado con éxito el registro.");

                //         dtFamiliares.rows().every(function () {
                //             var d = this.data();
                //             var row = this.row();

                //             if (d.id == idFamiliar) {

                //                 row.remove();
                //             }

                //         });

                //         // Draw once all updates are done
                //         dtFamiliares.draw();
                //         // fncGetFamiliares();
                //     } else {
                //         Alert2Error(message);
                //     }
                // }).catch(error => Alert2Error(error.message));
            }
        }
        //#endregion

        //#region CRUD NOMINA
        function initTblNominas() {
            dtTabuladores = tblRH_REC_Tabuladores.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                "order": [[0, "desc"]],
                createdRow: function (row, rowData) {
                    if (rowData.fechaAplicaCambio != null) {
                        $(row).find('.inputFechaCambio').datepicker({ dateFormat: 'dd/mm/yy' }).datepicker('option', 'showAnim', 'slide').datepicker('setDate', rowData.fechaAplicaCambio);
                    } else {
                        $(row).find('.inputFechaCambio').datepicker({ dateFormat: 'dd/mm/yy' }).datepicker('option', 'showAnim', 'slide');
                    }
                },
                columns: [
                    {
                        data: 'motivoCambio', title: 'MOTIVO', render: function (data, type, row) {
                            let selectMotivoSueldo = `<select class="form-control motivoSueldo" ${row.esNuevoTabulador ? '' : 'disabled'}>`;

                            motivoSueldo.find('option').each(function (value, index, array) {
                                if (row.motivoCambio == value) {
                                    selectMotivoSueldo += `<option value="${value}" selected>${$(this).text()}</option>`;
                                } else {
                                    selectMotivoSueldo += `<option value="${value}">${$(this).text()}</option>`;
                                }
                            });

                            selectMotivoSueldo += `</select>`;

                            return selectMotivoSueldo;
                        }
                    },
                    {
                        data: 'fecha_cambio', title: 'REGISTRO', visible: false, render: function (data, type, row) {
                            return data;
                        }
                    },
                    {
                        data: 'fechaAplicaCambio', title: 'FECHA APLICA CAMBIO', render: function (data, type, row) {
                            return `<input class="form-control text-center inputFechaCambio" ${inputPermisoEditarFechaCambio.val() == '1' || (row.esTabuladorLibre) ? '' : 'disabled'}>`;
                        }
                    },
                    {
                        data: 'fechaRealNomina', title: 'FECHA CAMBIO', render: function (data, type, row) {
                            // return `<input class="form-control text-center inputFechaCambio" ${inputPermisoEditarFechaCambio.val() == '1' ? '' : 'disabled'}>`;
                            return data;
                        }
                    },
                    {
                        data: 'salario_base', title: 'SALARIO BASE', render: function (data, type, row) {
                            if (txtEmpresa.val() == "6") {
                                return maskNumero2DCompras_PERU(data)
                            } else {
                                return maskNumero(data);
                            }
                        }
                    },
                    {
                        data: 'complemento', title: 'COMPLEMENTO', render: function (data, type, row) {
                            if (txtEmpresa.val() == "6") {
                                return maskNumero2DCompras_PERU(data)
                            } else {
                                return maskNumero(data);
                            }
                        }
                    },
                    {
                        data: 'bono_de_zona', title: 'BONO EN SISTEMA', render: function (data, type, row) {
                            if (txtEmpresa.val() == "6") {
                                return maskNumero2DCompras_PERU(data)
                            } else {
                                return maskNumero(data);
                            }
                        }
                    },
                    {
                        data: 'suma', title: 'TOTAL', render: function (data, type, row) {
                            if (txtEmpresa.val() == "6") {
                                return maskNumero2DCompras_PERU(data)
                            } else {
                                return maskNumero(data);
                            }
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_Tabuladores.on('click', '.classBtn', function () {
                        let rowData = dtTabuladores.row($(this).closest('tr')).data();
                    });

                    tblRH_REC_Tabuladores.on('click', '.classBtn', function () {
                        let rowData = dtTabuladores.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });

                    tblRH_REC_Tabuladores.on('change', '.motivoSueldo', function () {
                        let rowData = dtTabuladores.row($(this).closest('tr')).data();
                        rowData.motivoCambio = $(this).val();
                        if (rowData.esTabuladorLibre) {

                        } else {
                            if ($(this).val() == 4) {
                                $(this.closest('tr')).find('.inputFechaCambio').prop('disabled', false);
                            } else {
                                $(this.closest('tr')).find('.inputFechaCambio').prop('disabled', true);
                            }
                        }
                    });

                    tblRH_REC_Tabuladores.on('change', '.inputFechaCambio', function () {
                        let row = $(this).closest('tr');
                        let rowData = dtTabuladores.row(row).data();

                        // rowData.fechaRealNomina = $(row).find('.inputFechaCambio').val();
                        rowData.fechaAplicaCambio = $(row).find('.inputFechaCambio').val();

                        if (!rowData.esNuevoTabulador) {
                            Alert2AccionConfirmar('ATENCIÓN', '¿Desea cambiar la fecha de cambio del registro?', 'Confirmar', 'Cancelar', () => {
                                axios.post('CambiarFechaCambioTabulador', { id: rowData.id, fecha_cambio: $(row).find('.inputFechaCambio').val(), claveEmpleado: rowData.claveEmpleado })
                                    .then(response => {
                                        let { success, datos, message } = response.data;

                                        if (success) {
                                            Alert2Exito('Se ha guardado la información.');
                                        } else {
                                            AlertaGeneral(`Alerta`, message);
                                        }
                                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
                            });
                        }
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetTabuladores(idCategoriaDet) {
            let obj = new Object();
            obj = {
                clave_empleado: lblCEDatosEmpleadoClaveEmpleado.data('clvempleado')
            }
            axios.post("GetTabuladores", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (inputPermisoVerSalarios.val() == 1) {
                        if (response.data.esOcultarSalarioSoloGerencia != null) {
                            // const divInfomacionBacaria = $('#divInfomacionBacaria');
                            // const divTablaTabuladores = $('#divTablaTabuladores');
                            if (response.data.esOcultarSalarioSoloGerencia) {
                                nomina_tab.attr("href", "#tab");
                                home_tab.click();
                            } else {
                                nomina_tab.attr("href", "#tabNomina");
                                home_tab.click();
                            }
                        }
                    }

                    //#region FILL DATATABLE
                    dtTabuladores.clear();
                    dtTabuladores.rows.add(response.data.lstNomina);
                    dtTabuladores.draw();
                    // let FK_Tabulador = response.data.lstNomina[0].fk_tabulador
                    // if (FK_Tabulador > 0) {
                    //     txtNumeroTabulador.val(FK_Tabulador);
                    // }
                    //#endregion

                    cboCECompaniaPuestoCategoria.fillComboBox('GetCategoriaPuesto', { tabuladorDetID: idCategoriaDet }, '-- Seleccionar --', () => {
                        cboCECompaniaPuestoCategoria.select2({ width: "100%" });
                        cboCECompaniaPuestoCategoria.find('option:selected').remove();
                    });
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearTabulador() {
            let obj = new Object();
            obj = {
                nomina: cboCETabuladorTipoNomina.val(),
                libre: chkCETabuladorLibre.prop("checked"),
                claveEmpleado: lblCEDatosEmpleadoClaveEmpleado.data('clvempleado')
            }
            axios.post("CrearTabulador", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se ha registrado con éxito.");
                    mdlCrearEditarTabuladores.modal("hide");
                    cboCETabuladorTipoNomina[0].selectedIndex = 0;
                    cboCETabuladorTipoNomina.trigger("change");
                    txtCETabuladorNumTabulador.val("");
                    chkCETabuladorLibre.prop("checked", false);
                    chkCETabuladorLibre.trigger("change");
                    cboCETabuladorBanco[0].selectedIndex = 0;
                    cboCETabuladorBanco.trigger("change");
                    txtCETabuladorTarjeta.val("");
                    txtCETabuladorCtaAhorro.val("");
                    chkCETabuladorTarjetaNomina.prop("checked", false);
                    chkCETabuladorTarjetaNomina.trigger("change");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCalcularTotalSalario() {
            let salarioBase = 0;
            let complemento = 0;
            let bonoZona = 0;
            let total = 0;

            if (txtCENominaSalarioSalarioBase.val() > 0) { salarioBase = txtCENominaSalarioSalarioBase.val(); }
            if (txtCENominaSalarioComplemento.val() > 0) { complemento = txtCENominaSalarioComplemento.val(); };
            if (txtCENominaSalarioBonoZona.val() > 0) { bonoZona = txtCENominaSalarioBonoZona.val(); };

            total = parseFloat(salarioBase) + parseFloat(complemento) + parseFloat(bonoZona);
            txtCENominaSalarioTotal.val(parseFloat(total).toFixed(2));
        }
        //#region NOMINA SALARIO
        function fncCrearTabuladorPuesto() {
            let obj = new Object();
            obj = {
                tabulador: txtCETabuladorNumTabulador.val(),
                salario_base: txtCENominaSalarioSalarioBase.val(),
                complemento: txtCENominaSalarioComplemento.val(),
                bono_de_zona: txtCENominaSalarioBonoZona.val(),
                claveEmpleado: lblCEDatosEmpleadoClaveEmpleado.data('clvempleado')
            }
            axios.post("CrearTabuladorPuesto", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se ha registrado con éxito el salario.")
                    mdlCrearEditarNominaSalario.modal("hide");
                    txtCENominaSalarioSalarioBase.val("");
                    txtCENominaSalarioComplemento.val("");
                    txtCENominaSalarioBonoZona.val("");
                    txtCENominaSalarioTotal.val("");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
        //#endregion

        //#region IMPRIMIR DOCUMENTOS
        function fncImprimirDocumentos() {
            // console.log('imprimir')
            var path = `/Reportes/Vista.aspx?idReporte=228&tiDoc1=${chkTipoDocumento1.prop("checked") ? 1 : 0}
                                                          &tiDoc2=${chkTipoDocumento2.prop("checked") ? 1 : 0}
                                                          &tiDoc4=${chkTipoDocumento4.prop("checked") ? 1 : 0}
                                                          &tiDoc5=${chkTipoDocumento5.prop("checked") ? 1 : 0}
                                                          &tiDoc6=${chkTipoDocumento6.prop("checked") ? 1 : 0}
                                                          &tiDoc8=${chkTipoDocumento8.prop("checked") ? 1 : 0}
                                                          &tiDoc9=${chkTipoDocumento9.prop("checked") ? 1 : 0}
                                                          &tiDoc10=${chkTipoDocumento10.prop("checked") ? 1 : 0}
                                                          &tiDoc11=${chkTipoDocumento11.prop("checked") ? 1 : 0}
                                                          


                                                          &claveEmpleado=${mdlImprimirContratos.attr("data-claveEmpleado")}`;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function fncImprimirCredencial() {
            var path = `/Reportes/Vista.aspx?idReporte=254&claveEmpleado=${mdlImprimirContratos.attr("data-claveEmpleado")}`;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }
        //#endregion

        //#region CRUD UNIFORMES
        function initTblUniformes() {
            dtUniformes = tblRH_REC_Uniformes.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    {
                        data: "fechaEntrega", title: 'FECHA ENTREGA',
                        visible: false,
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    { data: "calzado", title: 'CALZADO' },
                    { data: "camisa", title: 'CAMISA' },
                    { data: "pantalon", title: 'PANTALÓN' },
                    { data: "overol", title: 'OVEROL' },
                    {
                        render: function (data, type, row) {
                            let btnActualizar = `<button class="btn btn-warning actualizarUniforme btn-xs"><i class="far fa-edit"></i></button>&nbsp;`;
                            let btnEliminar = `<button class="btn btn-danger eliminarUniforme btn-xs"><i class="far fa-trash-alt"></i></button>`;
                            return btnActualizar + btnEliminar;
                        }
                    },
                    { data: 'id', visible: false },
                    { data: 'clave_empleado', visible: false },
                    { data: 'otros', visible: false },
                    { data: 'comentarios', visible: false },
                    { data: 'entrego_calzado', visible: false },
                    { data: 'entrego_camisa', visible: false },
                    { data: 'entrego_pantalon', visible: false },
                    { data: 'entrego_overol', visible: false },
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_Uniformes.on('click', '.actualizarUniforme', function () {
                        fncLimpiarMdlCEUniforme();
                        let row = dtUniformes.row($(this).closest('tr')).data();
                        txtCEUniformeFechaEntrega.val(moment(row.fechaEntrega).format("YYYY-MM-DD"));
                        txtCEUniformeNoCalzado.val(row.calzado);
                        txtCEUniformeNoCalzado.trigger("change");
                        txtCEUniformeCamisa.val(row.camisa);
                        txtCEUniformeCamisa.trigger("change");
                        txtCEUniformePantalon.val(row.pantalon);
                        txtCEUniformePantalon.trigger("change");
                        txtCEUniformeOverol.val(row.overol);
                        txtCEUniformeOverol.trigger("change");

                        //txtCEUniformeUniformeDama.val(row.uniforme_dama);
                        txtCEUniformeOtros.val(row.otros);
                        txtCEUniformeComentarios.val(row.comentarios);

                        chkCEUniformeCalzado.prop("checked", row.entrego_calzado == "S" ? true : false);
                        chkCEUniformeCalzado.trigger("change");

                        chkCEUniformeCamisa.prop("checked", row.entrego_camisa == "S" ? true : false);
                        chkCEUniformeCamisa.trigger("change");

                        chkCEUniformePantalon.prop("checked", row.entrego_pantalon == "S" ? true : false);
                        chkCEUniformePantalon.trigger("change");

                        chkCEUniformeOverol.prop("checked", row.entrego_overol == "S" ? true : false);
                        chkCEUniformeOverol.trigger("change");

                        // chkCEUniformeUniformeDama.prop("checked", row.entrego_uniforme_dama == "S" ? true : false);
                        // chkCEUniformeUniformeDama.trigger("change");

                        btnCEUniforme.attr("data-id", row.id)
                        btnCEUniforme.html("Actualizar");
                        lblTitleCEUniforme.html("Actualizar uniforme");
                        mdlCrearEditarUniforme.modal("show");
                    });

                    tblRH_REC_Uniformes.on('click', '.eliminarUniforme', function () {
                        let rowData = dtUniformes.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarUniforme(rowData.clave_empleado));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetUniformes() {
            let obj = new Object();
            obj = {
                claveEmpleado: lblCEDatosEmpleadoClaveEmpleado.data('clvempleado')
            }
            axios.post("GetUniformes", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (response.data.lstUniformes != null) {
                        dtUniformes.clear();
                        dtUniformes.rows.add([response.data.lstUniformes]);
                        dtUniformes.draw();
                    } else {
                        dtUniformes.clear();
                        dtUniformes.draw();
                    }
                    //#region FILL DATATABLE
                    // dtUniformes.clear();
                    // dtUniformes.rows.add(response.data.lstUniformes);
                    // dtUniformes.draw();
                    //#endregion
                } else {

                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCEUniforme() {
            let obj = fncCEObjUniforme();

            axios.post("CrearEditarUniforme", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (obj.esActualizar) {
                        // fncGetUniformes();
                        dtUniformes.rows().every(function () {
                            var d = this.data();
                            if (d.id == obj.id) {
                                d.fechaEntrega = obj.fechaEntrega;
                                d.calzado = obj.calzado;
                                d.camisa = obj.camisa;
                                d.pantalon = obj.pantalon;
                                d.overol = obj.overol;
                                // d.uniforme_dama = obj.uniforme_dama;
                                d.otros = obj.otros;
                                d.comentarios = obj.comentarios;
                                d.entrego_calzado = obj.entrego_calzado;
                                d.entrego_camisa = obj.entrego_camisa;
                                d.entrego_pantalon = obj.entrego_pantalon;
                                d.entrego_overol = obj.entrego_overol;
                                // d.entrego_uniforme_dama = obj.entrego_uniforme_dama;
                                d.esActualizar = obj.esActualizar;
                                this.invalidate(); // invalidate the data DataTables has cached for this row
                            }
                            dtUniformes.draw();

                        });

                    } else {
                        dtUniformes.clear();
                        dtUniformes.rows.add([items]);
                        dtUniformes.draw();
                    }
                    Alert2Exito("Se ha registrado con éxito el uniforme.");
                    mdlCrearEditarUniforme.modal("hide");

                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCEObjUniforme() {
            let obj = new Object();
            obj = {
                id: btnCEUniforme.attr("data-id"),
                fechaEntrega: txtCEUniformeFechaEntrega.val(),
                clave_empleado: lblCEDatosEmpleadoClaveEmpleado.text() == "" ? 0 : lblCEDatosEmpleadoClaveEmpleado.data('clvempleado'),
                calzado: txtCEUniformeNoCalzado.val(),
                camisa: txtCEUniformeCamisa.val(),
                pantalon: txtCEUniformePantalon.val(),
                overol: txtCEUniformeOverol.val(),
                //uniforme_dama: txtCEUniformeUniformeDama.val(),
                otros: txtCEUniformeOtros.val(),
                comentarios: txtCEUniformeComentarios.val(),
                entrego_calzado: chkCEUniformeCalzado.prop("checked") == true ? "S" : "N",
                entrego_camisa: chkCEUniformeCamisa.prop("checked") == true ? "S" : "N",
                entrego_pantalon: chkCEUniformePantalon.prop("checked") == true ? "S" : "N",
                entrego_overol: chkCEUniformeOverol.prop("checked") == true ? "S" : "N",
                //entrego_uniforme_dama: chkCEUniformeUniformeDama.prop("checked") == true ? "S" : "N",
                esActualizar: Number(btnCEUniforme.attr("data-id")) > 0 ? true : false
            }
            return obj;
        }

        function fncEliminarUniforme(idUniforme) {
            let obj = new Object();
            obj = {
                idUniforme: idUniforme
            }
            axios.post("EliminarUniforme", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetUniformes();
                    Alert2Exito("Se ha eliminado con éxito el registro.");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncLimpiarMdlCEUniforme() {
            $("#mdlCrearEditarUniforme input[type='text']").val("");
            txtCEUniformeComentarios.val("");

            chkCEUniformeCalzado.prop("checked", false);
            chkCEUniformeCalzado.trigger("change");

            chkCEUniformeCamisa.prop("checked", false);
            chkCEUniformeCamisa.trigger("change");

            chkCEUniformePantalon.prop("checked", false);
            chkCEUniformePantalon.trigger("change");

            chkCEUniformeOverol.prop("checked", false);
            chkCEUniformeOverol.trigger("change");

            txtCEUniformeNoCalzado.val("");
            txtCEUniformeNoCalzado.trigger("change");

            txtCEUniformeCamisa.val("");
            txtCEUniformeCamisa.trigger("change");

            txtCEUniformePantalon.val("");
            txtCEUniformePantalon.trigger("change");

            txtCEUniformeOverol.val("");
            txtCEUniformeOverol.trigger("change");

            // chkCEUniformeUniformeDama.prop("checked", false);
            // chkCEUniformeUniformeDama.trigger("change");

            btnCEUniforme.attr("data-id", 0);
        }
        //#endregion

        //#region CRUD EXAMEN MEDICO
        function initTblExamenMedico() {
            dtExamenMedico = tblRH_REC_ArchivosExamenMedico.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreArchivo', title: 'Archivo' },
                    // { data: 'descripcion', title: 'Observación' },
                    {
                        title: 'Opciones',
                        render: function (data, type, row) {
                            // return `<button class="btn btn-danger eliminarArchivo btn-xs"><i class="far fa-trash-alt"></i></button>`;
                            return `<button class="btn btn-gander descargarArchivo btn-xs" title="Descargar" data-id="${row.idArchivo}"><i class="fas fa-file-download"></i></button>`;
                        }
                    },
                    // { data: 'id', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_ArchivosExamenMedico.on('click', '.descargarArchivo', function () {
                        fncDescargarDocumentoMedico($(this).data('id'));
                    });
                    // tblRH_REC_ArchivosExamenMedico.on('click', '.eliminarArchivo', function () {
                    //     let rowData = dtExamenMedico.row($(this).closest('tr')).data();
                    //     Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarExamenMedico(rowData.id));
                    // });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncDescargarDocumentoMedico(id) {
            document.location.href = `/Administrativo/Reclutamientos/DescargarArchivoEmpleado?id=${id}`;
        }

        function fncGetArchivoExamenMedico() {
            let obj = new Object();
            obj = {
                claveEmpleado: lblCEDatosEmpleadoClaveEmpleado.data('clvempleado')
            }
            axios.post("GetArchivoExamenMedico", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtExamenMedico.clear();
                    dtExamenMedico.rows.add(response.data.lstArchivos);
                    dtExamenMedico.draw();
                    //#endregion

                    if (response.data.lstArchivos.length > 0) {
                        btnMdlExamenMedico.attr("disabled", true);
                    } else {
                        btnMdlExamenMedico.attr("disabled", false);
                    }
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncEliminarExamenMedico(idExamenMedico) {
            let obj = new Object();
            obj = {
                idExamenMedico: idExamenMedico
            }
            axios.post("EliminarExamenMedico", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetArchivoExamenMedico();
                    Alert2Exito("Se ha eliminado con éxito el registro.");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearArchivoExamenMedico() {
            let objArchivoDTO = fncObjArchivoExamenMedico();
            axios.post("CrearArchivoExamenMedico", objArchivoDTO, { headers: { 'Content-Type': 'multipart/form-data' } })
                .then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        // GET ARCHIVO
                        txtCEExamenMedicoArchivo.val("");
                        fncGetArchivoExamenMedico();
                        mdlCrearEditarArchivoExamenMedico.modal("hide");
                        Alert2Exito("Éxito al registrar el archivo.");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
        }

        function fncObjArchivoExamenMedico() {
            const archivo = txtCEExamenMedicoArchivo.get(0).files[0];
            let objArchivoDTO = new Object();
            objArchivoDTO = {
                descripcion: txtCEExamenMedicoObservacion.val(),
                claveEmpleado: lblCEDatosEmpleadoClaveEmpleado.data('clvempleado')
            }
            let formData = new FormData();
            formData.set('objFile', archivo);
            formData.set('objArchivoDTO', JSON.stringify(objArchivoDTO));

            return formData;
        }
        //#endregion

        //#region CRUD MAQUINARIA
        function initTblMaquinaria() {
            dtMaquinaria = tblRH_REC_ArchivosMaquinaria.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreArchivo', title: 'Archivo' },
                    { data: 'descripcion', title: 'Observación' },
                    {
                        render: function (data, type, row) {
                            return `<button class="btn btn-danger eliminarArchivo btn-xs"><i class="far fa-trash-alt"></i></button>`;
                        }
                    },
                    { data: 'id', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_ArchivosMaquinaria.on('click', '.eliminarArchivo', function () {
                        let rowData = dtMaquinaria.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarMaquinaria(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetArchivoMaquinaria() {
            let obj = new Object();
            obj = {
                claveEmpleado: lblCEDatosEmpleadoClaveEmpleado.data('clvempleado')
            }
            axios.post("GetArchivoMaquinaria", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtMaquinaria.clear();
                    dtMaquinaria.rows.add(response.data.lstArchivos);
                    dtMaquinaria.draw();
                    //#endregion

                    if (response.data.lstArchivos.length > 0) {
                        btnMdlMaquinaria.attr("disabled", true);
                    } else {
                        btnMdlMaquinaria.attr("disabled", false);
                    }
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncEliminarMaquinaria(idMaquinaria) {
            let obj = new Object();
            obj = {
                idMaquinaria: idMaquinaria
            }
            axios.post("EliminarMaquinaria", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetArchivoMaquinaria();
                    Alert2Exito("Se ha eliminado con éxito el registro.");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearArchivoMaquinaria() {
            let objArchivoDTO = fncObjArchivoMaquinaria();
            axios.post("CrearArchivoMaquinaria", objArchivoDTO, { headers: { 'Content-Type': 'multipart/form-data' } })
                .then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        // GET ARCHIVO
                        txtCEMaquinariaArchivo.val("");
                        fncGetArchivoMaquinaria();
                        mdlCrearEditarArchivoMaquinaria.modal("hide");
                        Alert2Exito("Éxito al registrar el archivo.");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
        }

        function fncObjArchivoMaquinaria() {
            const archivo = txtCEMaquinariaArchivo.get(0).files[0];
            let objArchivoDTO = new Object();
            objArchivoDTO = {
                descripcion: txtCEMaquinariaObservacion.val(),
                claveEmpleado: lblCEDatosEmpleadoClaveEmpleado.data('clvempleado')
            }
            let formData = new FormData();
            formData.set('objFile', archivo);
            formData.set('objArchivoDTO', JSON.stringify(objArchivoDTO));

            return formData;
        }
        //#endregion

        //#region TAB CONTRATOS
        function initTblContratos() {
            dtContratos = tblContratos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: false,
                order: [[1, 'desc']],
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    // { data: 'id_contrato_empleado', title: 'CONTRATO' },
                    { data: 'desc_duracion', title: 'TIPO CONTRATO' },
                    { data: 'fecha_aplicacionString', title: 'FECHA APLICACIÓN' },
                    { data: 'fechaString', title: 'FECHA MODIFICACIÓN' },
                    {
                        data: 'fecha_aplicacion', title: 'FECHA INICIO',
                        render: (data, type, row, meta) => {
                            if (data != null) {
                                return moment(data).format("DD/MM/YYYY");
                            } else {
                                return moment().format("DD/MM/YYYY");

                            }
                        }
                    },
                    {
                        data: 'fecha_fin', title: 'FECHA FIN',
                        render: (data, type, row, meta) => {
                            // if (data != null) {
                            //     return moment(data).format("DD/MM/YYYY");
                            // } else {
                            //     return moment().format("DD/MM/YYYY");

                            // }

                            if (txtEmpresa.val() == 6) {
                                if (data != null) {
                                    return moment(data).format("DD/MM/YYYY");
                                } else {
                                    return "";
                                }

                            } else {
                                let fechaFin;
                                let fechaInicio = moment(row.fecha_aplicacion) ?? moment().format("DD/MM/YYYY");

                                switch (Number(row.clave_duracion)) {
                                    case 2:
                                        fechaFin = fechaInicio.add(3, 'M');

                                        break;
                                    case 3:
                                        fechaFin = null;

                                        break;
                                    case 4:
                                        fechaFin = null;

                                        break;
                                    case 5:
                                        fechaFin = fechaInicio.add(28, 'd')//AddDays(28);

                                        break;
                                    case 7:
                                        fechaFin = fechaInicio.add(6, 'M')//AddMonths(6);

                                        break;
                                    case 8:
                                        fechaFin = fechaInicio.add(13, 'd')//AddDays(13);

                                        break;
                                    case 9:
                                        fechaFin = fechaInicio.add(1, 'M')//AddMonths(1);

                                        break;
                                    case 10:
                                        fechaFin = fechaInicio.add(15, 'd')//AddDays(15);

                                        break;
                                    case 11:
                                        fechaFin = fechaInicio.add(16, 'd')//AddDays(16);

                                        break;
                                    case 12:
                                        fechaFin = fechaInicio.add(9, 'd')//AddDays(9);

                                        break;
                                    case 13:
                                        fechaFin = fechaInicio.add(25, 'd')//AddDays(25);

                                        break;
                                    case 14:
                                        fechaFin = fechaInicio.add(4, 'd')//AddDays(4);

                                        break;
                                    case 16:
                                        fechaFin = fechaInicio.add(5, 'd')//AddDays(5);

                                        break;
                                    case 18:
                                        fechaFin = fechaInicio.add(2, 'M')//AddMonths(2);

                                        break;
                                    case 19:
                                        fechaFin = fechaInicio.add(3, 'y')//AddYears(3);
                                        break;
                                    case 20:
                                        fechaFin = fechaInicio.add(12, 'M');
                                        break;
                                    default:
                                        break;
                                }

                                if (fechaFin != null) {
                                    fechaFin = fechaFin.format("DD/MM/YYYY")
                                }
                                return fechaFin ?? "";
                            }
                        }
                    },
                    {
                        data: null, title: 'OPCIONES', render: function (data, type, row) {
                            let btnEliminarContrato = '<button class="btn btn-danger btn-xs btnEliminarContrato"><i class="far fa-trash-alt"></i></button>';
                            return btnEliminarContrato;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    // tblContratos.on('click','.classBtn', function () {
                    //     let rowData = dtContratos.row($(this).closest('tr')).data();
                    // });
                    // tblContratos.on('click','.classBtn', function () {
                    //     let rowData = dtContratos.row($(this).closest('tr')).data();
                    //     //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    // });
                    tblContratos.on('click', '.btnEliminarContrato', function () {
                        let rowData = tblContratos.DataTable().row($(this).closest('tr')).data();
                        let row = $(this).closest('tr');
                        Alert2AccionConfirmar('Alerta!', '¿Desea eliminar el contrato seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarContrato(rowData.id_contrato_empleado, row));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncEliminarContrato(id_contrato_empleado, row) {
            if (id_contrato_empleado == 0) {
                tblContratos.DataTable().row(row).remove().draw();
            } else {
                $.post('EliminarContrato',
                    {
                        id_contrato_empleado: id_contrato_empleado
                    }).then(response => {
                        if (response.success) {
                            tblContratos.DataTable().row(row).remove().draw();
                        } else {
                            Alert2Error(response.message);
                        }
                    }, error => {
                        Alert2Error(error.message);
                    });
            }
        }

        function fncAddContratos() {
            let clave_empleado = lblCEDatosEmpleadoClaveEmpleado.data('clvempleado');
            let tipoDuracion = cboAddContratoTipoContrato.val();
            axios.post("AddContratos", { clave_empleado, tipoDuracion }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    //fncGetContratosFirmados();
                    fncGetContratos();
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region CRUD CARGA CONTRATOS FIRMADOS
        function initTblContratosFirmados() {
            dtContratosFirmados = tblContratosFirmados.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreArchivo', title: 'Archivo' },
                    { data: 'descripcion', title: 'Descripción' },
                    {
                        render: function (data, type, row) {
                            return `<button class="btn btn-danger eliminarContratoFirmado btn-xs"><i class="far fa-trash-alt"></i></button>`;
                        }
                    },
                    { data: 'id', visible: false },
                    { data: 'claveEmpleado', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblContratosFirmados.on('click', '.eliminarContratoFirmado', function () {
                        let rowData = dtContratosFirmados.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarContratoFirmado(rowData.id, rowData.claveEmpleado));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetContratosFirmados(claveEmpleado) {
            let obj = new Object();
            obj = {
                claveEmpleado: claveEmpleado
            }
            axios.post("GetContratosFirmados", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtContratosFirmados.clear();
                    dtContratosFirmados.rows.add(response.data.lstArchivos);
                    dtContratosFirmados.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncEliminarContratoFirmado(idArchivo, claveEmpleado) {
            let obj = new Object();
            obj = {
                idArchivo: idArchivo,
                claveEmpleado: claveEmpleado
            }
            axios.post("EliminarContratoFirmado", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se ha eliminado con éxito el contrato.");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGuardarContratoFirmado() {
            var request = new XMLHttpRequest();

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            request.open("POST", "/Administrativo/RecursosHumanos/Reclutamientos/");
            request.send(formDataCargaMasiva());

            request.onload = function (response) {
                $.unblockUI();

                if (request.status == 200) {
                    let respuesta = JSON.parse(request.response);
                    $('#inputArchivoExcel').val('');
                    if (respuesta.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                    } else {
                        AlertaGeneral(`Alerta`, `${respuesta.message}`);
                    }
                } else {
                    AlertaGeneral(`Alerta`, `Error al guardar la información.`);
                }
            };
        }

        function formDataCargaMasiva() {
            let formData = new FormData();
            $.each(document.getElementById("inputArchivoExcel").files, function (i, file) {
                formData.append("files[]", file);
            });
            return formData;
        }
        //#endregion

        //#region COMPAÑIAS
        function initTablaRequisiciones() {
            dtRequisiciones = tablaRequisiciones.DataTable({
                retrieve: true,
                language: dtDicEsp,
                bInfo: false,
                ordering: false,
                columns: [
                    { data: 'id', title: 'REQUISICIÓN ID' },
                    { data: 'ccDesc', title: 'CC' },
                    { data: 'id_plantilla', title: 'PLANTILLA' },
                    { data: 'puestoDesc', title: 'PUESTO' },
                    { data: 'descCategoria', title: 'CATEGORIA' },
                    { data: 'solicitados', title: 'SOLICITADOS', },
                    { data: 'faltantes', title: 'FALTANTES' },
                    {
                        title: 'Seleccionar', render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-warning botonSeleccionarRequisicion"><i class="fa fa-arrow-right"></i></button>`;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tablaRequisiciones.on('click', '.botonSeleccionarRequisicion', function () {
                        let rowData = dtRequisiciones.row($(this).closest('tr')).data();
                        txtCECompaniaRequisicion.val(rowData.id);
                        txtCECompaniaRequisicion.change();
                        txtCECompaniaCCDescripcion.attr("title", rowData.cc);
                        fncCECompaniaFillDepartamentos(rowData.cc, false, 0);
                        txtCECompaniaDeptoDescripcion.val("");
                        modalRequisiciones.modal('hide');
                    });
                },
                columnDefs: [
                    { width: '12%', targets: [0, 1, 7] },
                    { className: 'dt-center', targets: '_all' }
                ]
            });
        }

        var _cc;
        var _esActualizar;
        var _clave_depto;
        function fncCECompaniaFillDepartamentos(cc, esActualizar, clave_depto) {
            _cc = cc;
            _esActualizar = esActualizar;
            _clave_depto = clave_depto;
            txtCECompaniaDepto.fillCombo("FillDepartamentos", { cc: cc == 0 ? null : cc }, false, null, () => {
                txtCECompaniaDepto.select2();
                txtCECompaniaDepto.select2({ width: "100%" });

                let obj = new Object();
                obj = {
                    cc: _cc > 0 ? _cc : ""
                }

                if (_esActualizar) {
                    $('#txtCECompaniaDepto').val(_clave_depto);
                    $('#txtCECompaniaDepto').trigger("change");
                }
                // axios.post("FillDepartamentos", obj).then(response => {
                //     let { success, items, message } = response.data;
                //     if (success) {
                //         //#region FILL DATATABLE
                //         if (esActualizar) {
                //             $('#txtCECompaniaDepto').val(clave_depto);
                //             $('#txtCECompaniaDepto').trigger("change");
                //         }
                //         //#endregion
                //     } else {
                //         Alert2Error(message);
                //     }
                // }).catch(error => Alert2Error(error.message));
            });
        }

        function fncGetIDUsuarioEK() {
            axios.post("GetIDUsuarioEK").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (response.data.idEK > 0) {
                        txtCECompaniaUsuarioResg.val(response.data.idEK);
                        txtCECompaniaUsuarioResg.trigger("change");
                    }
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region COMENTARIOS
        function fncGetComentario(cveEmpleado) {
            axios.post("GetComentario", { claveEmpleado: cveEmpleado }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    txtMdlComentario.text(items ?? "");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearCometario(cveEmpleado, comment) {
            axios.post("CrearComentario", { claveEmpleado: cveEmpleado, comentario: comment }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncCambiarEstatusEmpleado(cveEmpleado, "C");
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region DOCUMENTOS
        function initTblDocumentos() {
            dtDocumentos = tblDocumentos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'archiveDesc', title: 'Archivo' },
                    {
                        title: '',
                        render: function (data, type, row) {
                            if (row.estado && row.rutaArchivo != null) {
                                return `<button class="btn btn-xs btn-success botonRedondo detalleAut" data-id='${row.id}' data-tipo='1' disabled><i class="fa fa-check"></i></button>`;
                            } else {
                                return `<button class="btn btn-xs btn-warning botonRedondo" disabled><i class="fa fa-clock"></i></button>`;
                            }
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblDocumentos.on('click', '.classBtn', function () {
                        let rowData = dtDocumentos.row($(this).closest('tr')).data();
                    });
                    tblDocumentos.on('click', '.classBtn', function () {
                        let rowData = dtDocumentos.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetDocs(clave_empleado, id_candidato) {

            axios.post("GetDocs", { clave_empleado, id_candidato }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    dtDocumentos.clear();
                    dtDocumentos.rows.add(items);
                    dtDocumentos.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region FOTO DEL EMPLEADO
        function fncGuardarFotoEmpleado(claveEmpleado) {
            let data = fncGetFotoEmpleadoParaGuardar();
            let obj = new Object();
            if (claveEmpleado) {
                obj.claveEmpleado = claveEmpleado;

            } else {
                obj.claveEmpleado = lblCEDatosEmpleadoClaveEmpleado.data('clvempleado');
            }
            axios.post('GuardarFotoEmpleado', data, { params: objDTO = obj }, { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                let { success, datos, message } = response.data;
                if (success) {
                    Alert2Exito(message);
                    fncGetFotoEmpleado();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetFotoEmpleadoParaGuardar() {
            let data = new FormData();
            $.each(document.getElementById("fileFotoEmpleado").files, function (i, file) {
                data.append("objFotoEmpleado", file);
            });
            return data;
        }

        function fncGetFotoEmpleado() {
            let obj = new Object();
            obj.claveEmpleado = lblCEDatosEmpleadoClaveEmpleado.data('clvempleado');
            axios.post("GetFotoEmpleado", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region SE VERIFICA SI YA CUENTA CON UNA FOTO EL EMPLEADO
                    fncLimpiarMdlFotoEmpleado();
                    if (response.data.objFotoEmpleado.rutaArchivo != "" && response.data.objFotoEmpleado.rutaArchivo != null) {
                        let image = response.data.objFotoEmpleado.imageToBase64;
                        base64String = image.replace("data:", "").replace(/^.+,/, "");
                        document.getElementById("txtImageFotoEmpleadoBase64").value = "data:image/jpeg;base64," + base64String;
                        document.getElementById("imgFotoEmpleado").src = document.getElementById("txtImageFotoEmpleadoBase64").value;
                        document.getElementById("imgFotoEmpleadoHead").src = document.getElementById("txtImageFotoEmpleadoBase64").value;
                        btnCEFotoEmpleado.html(`<i class="fas fa-save"></i>&nbsp;Actualizar`);
                    } else {
                        btnCEFotoEmpleado.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
                    }
                    // mdlVerFotoEmpleado.modal("show");
                    //#endregion
                } else {
                    // Alert2Error(message);
                    imgFotoEmpleado.attr("src", null);
                    imgFotoEmpleadoHead.attr("src", null);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncLimpiarMdlFotoEmpleado() {
            lblNombreFotoEmpleado.html("");
            fileFotoEmpleado.val("");
            imgFotoEmpleado.attr("src", "");
            imgFotoEmpleadoHead.attr("src", null);
        }
        //#endregion

        //#region HISTORIAL CC
        function initTblHistorial() {
            dtHistorial = tblHistorial.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'cc', title: 'CC Nuevo' },
                    { data: 'cc_anterior', title: 'CC Anterior' },
                    {
                        data: 'fecha_cambio', title: 'Fecha de Cambio',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }

                    },
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetHistorialCC(claveEmpleado) {
            axios.post("GetHistorialCC", { clave_empleado: claveEmpleado }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    // console.log(items)
                    dtHistorial.clear();
                    dtHistorial.rows.add(items);
                    dtHistorial.draw();
                    mdlHistorial.modal("show");
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region SUSTENTO HIJOS PERU
        function initTblSustentos() {
            dtSustentos = tblSustentos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreArchivo', title: 'Sustento' },
                    {
                        render: function (data, type, row, meta) {
                            return `<button class='btn btn-xs btn-primary descargarArchivo' title='Descargar archivo.'><i class="fas fa-file-download"></i></button>`;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblSustentos.on('click', '.descargarArchivo', function () {
                        let rowData = dtSustentos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('Descargar archivo', '¿Desea descargar el sustento?', 'Confirmar', 'Cancelar', () => fncDescagarSustento(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', 'targets': '_all' },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
            });
        }

        function fncDescagarSustento(id) {
            document.location.href = `/Administrativo/Reclutamientos/DescagarSustento?id=${id}`;
        }

        function fncGetSustentos() {
            let obj = {};
            obj.claveEmpleado = lblCEDatosEmpleadoClaveEmpleado.html();
            obj.FK_EmplFamilia = btnCEFamiliar.data("id");
            axios.post('GetSustentos', obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtSustentos.clear();
                    dtSustentos.rows.add(items);
                    dtSustentos.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGuardarSustentoHijo() {
            let data = new FormData();
            $.each(document.getElementById("txtCargarSustento").files, function (i, file) {
                data.append("lstSustentos", file);
            });
            data.append('claveEmpleado', lblCEDatosEmpleadoClaveEmpleado.html());
            data.append('FK_EmplFamilia', btnCEFamiliar.data("id"));
            axios.post('GuardarSustentoHijo', data, { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetSustentos();
                    Alert2Exito(message);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.AltaEmpleados = new AltaEmpleados();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();   