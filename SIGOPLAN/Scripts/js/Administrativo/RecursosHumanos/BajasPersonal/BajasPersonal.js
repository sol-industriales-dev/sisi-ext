(() => {
    $.namespace('CH.Bajas');

    //#region CONST FILTROS
    const selectCC = $('#selectCC');
    const btnFiltroNuevo = $('#btnFiltroNuevo');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const tblRH_Baja_Registro = $('#tblRH_Baja_Registro');
    const cboFiltroPrioridad = $('#cboFiltroPrioridad');
    const cboFiltroEstatus = $('#cboFiltroEstatus');
    const fechaFiltroInicio = $('#fechaFiltroInicio');
    const fechaFiltroFin = $('#fechaFiltroFin');
    const inputClaveEmpleado = $('#inputClaveEmpleado');
    const inputNombreEmpleado = $('#inputNombreEmpleado');
    const fechaFiltroContaInicio = $('#fechaFiltroContaInicio');
    const fechaFiltroContaFin = $('#fechaFiltroContaFin');
    const btnDiasBajas = $('#btnDiasBajas');
    const cboFiltroAnticipada = $('#cboFiltroAnticipada');
    const btnFiltroNotificar = $('#btnFiltroNotificar');
    let dtBajas;
    //#endregion

    //#region DIAS BAJAS
    const mdlDiasBajas = $('#mdlDiasBajas');
    const btnActualizarDiasBajas = $('#btnActualizarDiasBajas');
    const txtDiasAnterioresPermitidos = $('#txtDiasAnterioresPermitidos');
    const txtDiasPosterioresPermitidos = $('#txtDiasPosterioresPermitidos');
    //#endregion

    //#region CONST BAJA DE PERSONAL
    const lblTituloCrearEditarBaja = $('#lblTituloCrearEditarBaja');
    const txt_registro_numeroEmpleado = $("#txt_registro_numeroEmpleado");
    const txt_registro_numeroEmpleado_baja = $("#txt_registro_numeroEmpleado_baja");
    const btnBuscarPersonal = $('#btnBuscarPersonal');
    const txt_registro_nombre = $("#txt_registro_nombre");
    const txt_registro_nombre_baja = $("#txt_registro_nombre_baja");
    const txt_registro_cc = $("#txt_registro_cc");
    const txt_registro_puesto = $("#txt_registro_puesto");
    const txt_registro_fechaIngreso = $('#txt_registro_fechaIngreso');
    const txt_registro_curp = $("#txt_registro_curp");
    const txt_registro_rfc = $("#txt_registro_rfc");
    const txt_registro_nss = $("#txt_registro_nss");
    const txt_registro_habilidadesEquipo = $("#txt_registro_habilidadesEquipo");
    const txt_registro_telPersonal = $("#txt_registro_telPersonal");
    const cbo_registro_tieneWha = $("#cbo_registro_tieneWha");
    const txt_registro_telCasa = $("#txt_registro_telCasa");
    const txt_registro_contactoFamilia = $("#txt_registro_contactoFamilia");
    const cbo_registro_idEstado = $("#cbo_registro_idEstado");
    const cbo_registro_idCiudad = $("#cbo_registro_idCiudad");
    const cbo_registro_idMunicipio = $("#cbo_registro_idMunicipio");
    const txt_registro_direccion = $("#txt_registro_direccion");
    const txt_registro_facebook = $("#txt_registro_facebook");
    const txt_registro_instagram = $("#txt_registro_instagram");
    const txt_registro_correo = $("#txt_registro_correo");
    const txt_registro_fechaBaja = $("#txt_registro_fechaBaja");
    const cbo_registro_motivoBajaSistema = $("#cbo_registro_motivoBajaSistema");
    const txt_registro_motivoSeparacionEmpresa = $("#txt_registro_motivoSeparacionEmpresa");
    const cbo_registro_regresariaALaEmpresa = $("#cbo_registro_regresariaALaEmpresa");
    const txt_registro_porqueRegresariaALaEmpresa = $("#txt_registro_porqueRegresariaALaEmpresa");
    const txt_registro_dispuestoCambioDeProyecto = $("#txt_registro_dispuestoCambioDeProyecto");
    const txt_registro_experienciaCP = $("#txt_registro_experienciaCP");
    const cbo_registro_contratable = $("#cbo_registro_contratable");
    const cbo_registro_prioridad = $("#cbo_registro_prioridad");
    const btnCEBajaPersonal = $('#btnCEBajaPersonal');
    const lblTituloBtnCEBajaPersonal = $('#lblTituloBtnCEBajaPersonal');
    const mdlBaja = $('#mdlBaja');
    const mdlActos = $('#mdlActos');
    const tblActos = $('#tblActos');
    const lblCantActos = $('#lblCantActos');
    let dtActos;
    const mdlCapacitaciones = $('#mdlCapacitaciones');
    const tblCapacitaciones = $('#tblCapacitaciones');
    const lblCantCapacitaciones = $('#lblCantCapacitaciones');
    let dtCapacitaciones;
    const btnCantActos = $('#btnCantActos');
    const btnCantCapacitaciones = $('#btnCantCapacitaciones');
    let dtHistorial
    const mdlHistorial = $('#mdlHistorial');
    const tblHistorial = $('#tblHistorial');
    const div_registro_comentariosRecontratable = $('#div_registro_comentariosRecontratable');
    const txt_registro_comentariosRecontratable = $('#txt_registro_comentariosRecontratable');

    const mdlDetalleaut = $('#mdlDetalleaut');

    const txt_aut_fecha = $('#txt_aut_fecha');
    const txt_aut_Usuario = $('#txt_aut_Usuario');
    const txt_aut_comentario = $('#txt_aut_comentario');

    const divPreguntaEstancia = $('#divPreguntaEstancia');
    const txt_registro_observaciones = $('#txt_registro_observaciones');
    const txt_registro_nombre_autoriza = $('#txt_registro_nombre_autoriza');
    const btnCEBajaCardToggle = $('#btnCEBajaCardToggle');
    const chk_registro_esAnticipada = $('#chk_registro_esAnticipada');
    const cbo_registro_idDepartamento = $('#cbo_registro_idDepartamento');
    const txt_registro_dni = $('#txt_registro_dni');
    const divRegresariaEmpresa = $('#divRegresariaEmpresa');
    const txt_registro_cedula = $('#txt_registro_cedula');
    //#endregion

    //#region CONST ENTREVISTA
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
    //#endregion

    //#region COMPARTIR ENTREVISTA
    const mdlCompartir = $('#mdlCompartir');
    const txtCompartirCorreo = $('#txtCompartirCorreo');
    const txtCompartirLink = $('#txtCompartirLink');
    const btnCompartirEnviar = $('#btnCompartirEnviar');
    const txtCompartirCopiar = $('#txtCompartirCopiar');
    const txtCompartirSubtitulo = $('#txtCompartirSubtitulo');
    //#endregion

    //#region MDL VER FINIQUITO
    const mdlFiniquito = $('#mdlFiniquito');
    const btnImprimirFiniquito = $('#btnImprimirFiniquito');
    const btnImprimirFiniquitoRecalc = $('#btnImprimirFiniquitoRecalc');
    const btnImprimirFiniquitoCierre = $('#btnImprimirFiniquitoCierre');
    const txtVerMonto = $('#txtVerMonto');
    const txtVerMontoRecalc = $('#txtVerMontoRecalc');
    const txtVerMontoCierre = $('#txtVerMontoCierre');
    const btnVerFiniquito = $("#btnVerFiniquito");
    const btnVerFiniquitoRecalc = $("#btnVerFiniquitoRecalc");
    const btnVerFiniquitoCierre = $("#btnVerFiniquitoCierre");
    const btnVerFiniquitoIMS = $("#btnVerFiniquitoIMS");
    //#endregion

    //#region CE Finiquito
    const mdlCapturarFiniquito = $('#mdlCapturarFiniquito');
    const inCapturarFiniquito = $('#inCapturarFiniquito');
    const btnGuardarFiniquito = $('#btnGuardarFiniquito');
    const cboCEFiniquitoTipo = $('#cboCEFiniquitoTipo');
    const txtCEFiniquitoMonto = $('#txtCEFiniquitoMonto');
    const txtCEPathFile = $('#txtCEPathFile');
    const lblCapturarFiniquito = $('#lblCapturarFiniquito');

    //FUUUUUUUUUUUUUUUUSION
    const btnCapturarFiniquito = $('#btnCapturarFiniquito');
    const spanCapturarFiniquito = $('#spanCapturarFiniquito');
    const txtVerNombreFiniquito = $('#txtVerNombreFiniquito');
    const btnCapturarRecalculo = $('#btnCapturarRecalculo');
    const spanCapturarRecalculo = $('#spanCapturarRecalculo');
    const txtVerNombreRecalculo = $('#txtVerNombreRecalculo');
    const btnCapturarCierre = $('#btnCapturarCierre');
    const spanCapturarCierre = $('#spanCapturarCierre');
    const txtVerNombreCierre = $('#txtVerNombreCierre');
    const btnCapturarIMS = $('#btnCapturarIMS');
    const spanCapturarIMS = $('#spanCapturarIMS');
    const txtVerMontoIMS = $('#txtVerMontoIMS');
    const txtVerNombreIMS = $('#txtVerNombreIMS');
    const btnImprimirFiniquitoIMS = $('#btnImprimirFiniquitoIMS');
    const txtVencidoIMS = $('#txtVencidoIMS');
    //RIP

    const mdlFiniquitoImss = $('#mdlFiniquitoImss');
    //#endregion

    //#region CONST EMPLEADOS

    //#region CONST LISTADO EMPLEADOS
    const tblRH_REC_Empleados = $('#tblRH_REC_Empleados');
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
    const chkFiltroEsPendiente = $('#chkFiltroEsPendiente');
    //#endregion

    //#endregion

    //#region CONST GENERALES Y CONTACTO
    const cboCEGenContactoEstadoCivil = $('#cboCEGenContactoEstadoCivil');
    const txtCEGenContactoFechaPlanta = $('#txtCEGenContactoFechaPlanta');
    const txtCEGenContactoEstudios = $('#txtCEGenContactoEstudios');
    const txtCEGenContactoAbreviatura = $('#txtCEGenContactoAbreviatura');
    const txtCEGenContactoCredElector = $('#txtCEGenContactoCredElector');
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
    const cboCECompaniaTipoFormula = $('#cboCECompaniaTipoFormula');
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

    //#region SALARIO TABULADOR
    const mdlCrearEditarNominaSalario = $('#mdlCrearEditarNominaSalario');
    const txtCENominaSalarioSalarioBase = $('#txtCENominaSalarioSalarioBase');
    const txtCENominaSalarioComplemento = $('#txtCENominaSalarioComplemento');
    const txtCENominaSalarioBonoZona = $('#txtCENominaSalarioBonoZona');
    const txtCENominaSalarioTotal = $('#txtCENominaSalarioTotal');
    const btnCENominaSalario = $('#btnCENominaSalario');
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
    const txtCEUniformeUniformeDama = $('#txtCEUniformeUniformeDama');
    const chkCEUniformeUniformeDama = $('#chkCEUniformeUniformeDama');
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

    //#region 
    const mdlComentario = $('#mdlComentario');
    const txtMdlComentario = $('#txtMdlComentario');
    //#endregion

    const mdlContratosFirmados = $('#mdlContratosFirmados');
    const tblContratosFirmados = $('#tblContratosFirmados');
    const btnCrearContratoFirmado = $('#btnCrearContratoFirmado');
    const mdlGuardarContratoFirmado = $('#mdlGuardarContratoFirmado');
    const btnGuardarContratoFirmado = $('#btnGuardarContratoFirmado');
    const inputArchivoExcel = $('#inputArchivoExcel');
    const txtCEContratoFirmadoDescripcion = $('#txtCEContratoFirmadoDescripcion');
    let dtContratosFirmados;

    const botonVerRequisiciones = $('#botonVerRequisiciones');
    const modalRequisiciones = $('#modalRequisiciones');
    const tablaRequisiciones = $('#tablaRequisiciones');

    let dtRequisiciones;
    let lstEstatus = [
        { val: 'A', text: 'ALTA' },
        { val: 'P', text: 'PENDIENTE' },
        { val: 'B', text: 'BAJA' },
    ]

    let _clave_empleado = 0;
    //#endregion

    //#region CONST DOCUMENTOS
    const tblDocumentos = $('#tblDocumentos');
    const tabDocumentos = $('#tabDocumentos');
    let dtDocumentos = $('#dtDocumentos');
    //#endregion

    //#region PERMISOS
    const inputPermisoUsuarioTeorico = $('#inputPermisoUsuarioTeorico');
    let esUsuarioTeorico = inputPermisoUsuarioTeorico.val() == 1 ? true : false;

    //NOTIFICAR
    const inputPermisoUsuarioNotificar = $('#inputPermisoUsuarioNotificar');
    let esUsuarioNotificar = inputPermisoUsuarioNotificar.val() == 1 ? true : false;

    //LIBERAR NOMINAS
    const inputPermisoUsuarioLiberarNominas = $('#inputPermisoUsuarioLiberarNominas');
    let esUsuarioLiberarNominas = inputPermisoUsuarioLiberarNominas.val() == 1 ? true : false;
    //#endregion

    let _bajaAutorizada = 0;
    let _bajaEntrevista_id = 0;

    let fechaMaxBaja = null;
    let fechaMinBaja = null;


    let empresa = 0;
    const empresaActual = $('#empresaActual');
    _empresaActual = empresaActual.val();

    Bajas = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT TABLAS
            initTblBajasPersonal();
            initTblActos();
            initTblCapacitaciones();
            initTblHistorial();

            //#region TABLAS VER CAPTURA BAJAS (RECLUTAMIENTOS)
            initTblFamiliares();
            initTblNominas();
            initTblUniformes();
            initTblContratos();
            initTblExamenMedico();
            initTblMaquinaria();
            initTblContratosFirmados();
            initTablaRequisiciones();
            initTblDocumentos();
            //#endregion
            //#endregion

            //#region CONTROL EMPRESAS

            if (_empresaActual == "6" || _empresaActual == "3") {
                dtBajas.column(10).visible(false); //
                dtBajas.column(11).visible(false); //
            }

            //#endregion

            txt_registro_fechaBaja.val(moment().format('YYYY-MM-DD'));

            //#region EVENTOS BAJAS PERSONAL
            // fncGetBajasPersonal();
            $(".select2").select2();
            $(".select2").select2({ width: "100%" });

            btnFiltroNuevo.on("click", function () {
                lblTituloCrearEditarBaja.text("REGISTRO DE BAJA");
                lblTituloBtnCEBajaPersonal.html("<i class='fa fa-save'></i>&nbsp;Guardar");
                btnCEBajaPersonal.attr("data-id", 0);
                div_registro_comentariosRecontratable.hide();
                fncLimpiarFormulario();
                fncBorderDefault();

                fieldRegistro.find('input, select, textarea').each(function (idx, element) {
                    // if ($(element).attr('id') != 'txt_registro_fechaBaja') {
                    //     $(element).prop('disabled', false);
                    // }
                    $(element).prop('disabled', false);

                });

                fieldEntrevista.find('input, select, textarea').each(function (idx, element) {
                    $(element).prop('disabled', false);
                });

                // chk_registro_esAnticipada.bootstrapToggle('off');

                // txt_registro_fechaBaja.prop('disabled', true);

                mdlBaja.modal("show");
                fieldEntrevista.show();
                fieldRegistro.show();
            });

            btnCantActos.on("click", function () {
                fncGetActosCapacitaciones(txt_registro_numeroEmpleado.val(), "ACTO");
            });

            btnCantCapacitaciones.on("click", function () {
                fncGetActosCapacitaciones(txt_registro_numeroEmpleado.val(), "CAPACITACIONES");
            });

            btnFiltroNuevo.on("click", function () {
                btnCEBajaPersonal.show();
                txt_registro_fechaBaja.val(moment().format('YYYY-MM-DD'));

            });

            btnCEBajaPersonal.on("click", function () {
                if (cbo_registro_motivoBajaSistema.val() == 1 && txt_entrevista_gerenteClave.val() == "") {
                    // Alert2AccionConfirmar('Baja sin entrevista. (Recordatorio)', '¿Desea capturar la baja sin entrevista de salida?', 'Confirmar', 'Cancelar', () => fncCEBajaPersonal());
                    swal({
                        title: "Baja sin entrevista. (Recordatorio)",
                        text: "¿Desea capturar la baja sin entrevista de salida?",
                        icon: "warning",
                        buttons: true,
                        dangerMode: true,
                    }).then((confirmar) => {
                        if (confirmar) {
                            fncCEBajaPersonal();
                        } else {
                            // btnCEBajaCardToggle.scrollIntoView();
                            document.getElementById("btnCEBajaCardToggle").scrollIntoView();
                        }
                    });
                } else {
                    fncCEBajaPersonal();

                }

            });

            btnFiltroBuscar.on("click", function () {
                fncGetBajasPersonal();
            });

            btnDiasBajas.on('click', function () {
                txtDiasAnterioresPermitidos.val(0);
                txtDiasPosterioresPermitidos.val(0);

                $.get('/BajasPersonal/GetDiasDisponibles').then(response => {
                    if (response.success) {
                        txtDiasAnterioresPermitidos.val(response.items.anteriores);
                        txtDiasPosterioresPermitidos.val(response.items.posteriores);

                        mdlDiasBajas.modal('show');
                    } else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal('Alerta!', 'Hubo un problema al comuncarse al servidor.', 'warning');
                });
            });

            btnActualizarDiasBajas.on('click', function () {
                $.post('/BajasPersonal/SetDiasDisponibles',
                    {
                        anteriores: +txtDiasAnterioresPermitidos.val(),
                        posteriores: +txtDiasPosterioresPermitidos.val()
                    }).then(response => {
                        if (response.success) {
                            txt_registro_fechaBaja.attr('min', response.items.anterior);
                            txt_registro_fechaBaja.attr('max', response.items.posterior);

                            fechaMaxBaja = response.items.posterior;
                            fechaMinBaja = response.items.anterior;

                            swal('Confirmación!', 'Se actualizaron los días', 'success');
                        } else {
                            swal('Alerta!', response.message, 'warning');
                        }
                    }, error => {
                        swal('Alerta!', 'Hubo un problema al comuncarse al servidor.', 'warning');
                    });
            });

            txt_registro_numeroEmpleado.on("change", function () {
                fncGetDatosPersonal(false);
            });

            cbo_registro_contratable.change(fncMostrarEntrevista);

            cbo_registro_idDepartamento.on("change", function () {
                cbo_registro_idEstado.fillCombo("/Administrativo/Reclutamientos/FillCboEstadosPERU", { claveDepartamento: $(this).val() }, false);

            });

            cbo_registro_idEstado.on("change", function () {
                if ($(this).val() > 0) {
                    cbo_registro_idCiudad.fillCombo("/BajasPersonal/FillCboMunicipiosEK", { idEstado: $(this).val() }, false);
                    cbo_registro_idMunicipio.fillCombo("/BajasPersonal/FillCboMunicipiosEK", { idEstado: $(this).val() }, false);
                }
            });

            cbo_registro_idDepartamento.fillCombo("/Administrativo/Reclutamientos/FillComboGeoDepartamentos", null, false);
            cbo_registro_idEstado.fillCombo("/BajasPersonal/FillCboEstadosEK", {}, false);
            cbo_registro_motivoBajaSistema.fillCombo('/Administrativo/ReportesRH/FillComboConceptosBaja', { est: true }, false, "--Seleccione--");

            btnVerFiniquito.on("click", function () {
                fncVisualizarDocumento(btnImprimirFiniquito.attr("data-id"), 0);
            });

            btnVerFiniquitoRecalc.on("click", function () {
                fncVisualizarDocumento(btnImprimirFiniquito.attr("data-id"), 1);
            });

            btnVerFiniquitoCierre.on("click", function () {
                fncVisualizarDocumento(btnImprimirFiniquito.attr("data-id"), 2);
            });

            btnVerFiniquitoIMS.on("click", function () {
                fncVisualizarDocumento(btnImprimirFiniquito.attr("data-id"), 3);
            });
            //#endregion

            //#region EVENTOS ENTREVISTA
            cbo_entrevista_p1.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 1 }, false, "--Seleccione--");
            cbo_entrevista_p2.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 2 }, false, "--Seleccione--");
            cbo_entrevista_p3_1.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 3 }, false, "--Seleccione--");
            cbo_entrevista_p3_2.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 3 }, false, "--Seleccione--");
            cbo_entrevista_p3_3.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 3 }, false, "--Seleccione--");
            cbo_entrevista_p3_4.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 3 }, false, "--Seleccione--");
            cbo_entrevista_p3_5.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 3 }, false, "--Seleccione--");
            cbo_entrevista_p3_6.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 3 }, false, "--Seleccione--");
            cbo_entrevista_p3_7.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 3 }, false, "--Seleccione--");
            cbo_entrevista_p3_8.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 3 }, false, "--Seleccione--");
            cbo_entrevista_p3_9.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 3 }, false, "--Seleccione--");
            cbo_entrevista_p3_10.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 3 }, false, "--Seleccione--");
            cbo_entrevista_p4.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 4 }, false, "--Seleccione--");
            cbo_entrevista_p5.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 5 }, false, "--Seleccione--");
            cbo_entrevista_p8.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 8 }, false, "--Seleccione--");
            cbo_entrevista_p9.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 9 }, false, "--Seleccione--");
            cbo_entrevista_p10.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 10 }, false, "--Seleccione--");
            cbo_entrevista_p11.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 11 }, false, "--Seleccione--");
            cbo_entrevista_p11_1.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 11 }, false, "--Seleccione--");
            cbo_entrevista_p12.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 12 }, false, "--Seleccione--");
            cbo_entrevista_p13.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 13 }, false, "--Seleccione--");
            cbo_entrevista_p14.fillCombo('/Administrativo/BajasPersonal/FillCboPreguntas', { idPregunta: 14 }, false, "--Seleccione--");
            cbo_entrevista_estadoCivilID.fillCombo('/Administrativo/BajasPersonal/FillCboEstadosCiviles', null, false, "--Seleccione--");
            cbo_entrevista_escolaridadID.fillCombo('/Administrativo/BajasPersonal/FillCboEscolaridades', null, false, "--Seleccione--");
            //#endregion

            selectCC.fillCombo('/Administrativo/BajasPersonal/GetCCs', { est: true }, false, 'Todos');
            convertToMultiselect('#selectCC');

            txt_entrevista_gerenteClave.on("change", function () {
                // fncGetDatosPersonal(true);

            });
            txt_entrevista_gerenteNombre.getAutocomplete(funGetGerente, null, '/Administrativo/FormatoCambio/getCatEmpleadosReclutamientos');
            txt_registro_nombre.getAutocomplete(funGetEmpleado, null, '/Administrativo/FormatoCambio/getCatEmpleadosReclutamientos');

            //Se cambia por fill combo
            // txt_registro_nombre_baja.getAutocomplete(funGetEmpleado_baja, null, '/Administrativo/BajasPersonalEntrevista/getEmpleadosGeneral');


            txtCompartirCopiar.on("click", function () {
                /* Select the text field */
                txtCompartirLink.select();
                /* Copy the text inside the text field */
                navigator.clipboard.writeText(txtCompartirLink.val());

                /* Alert the copied text */
                Swal.fire({
                    icon: 'success',
                    title: 'Link copiado',
                    text: "",
                    showConfirmButton: false,
                    timer: 1000
                });
            });

            btnCompartirEnviar.on("click", function () {
                Alert2AccionConfirmar('!¡', `¿Desea mandar correo a ${txtCompartirCorreo.val()}?`, 'Confirmar', 'Cancelar', () =>
                    EnviarCorreo(txtCompartirCorreo.val(), txtCompartirLink.val(), btnCompartirEnviar.attr('data-user'))
                );
            });

            btnGuardarFiniquito.on("click", function () {
                // if (cboCEFiniquitoTipo.val() == "0") {
                //     fncGuardarArchivoFiniquito();
                // } else {
                //     let asd = btnGuardarFiniquito.attr("data-finiquito");
                //     if (btnGuardarFiniquito.attr("data-finiquito") != undefined) {
                //         fncGuardarArchivoFiniquito();
                //     } else {
                //         Alert2Warning("No existe un finiquito capturado.");
                //     }
                // }

                fncGuardarArchivoFiniquito();

            });

            btnCapturarFiniquito.on("click", function () {
                txtCEPathFile.val("");
                txtCEFiniquitoMonto.val(0);
                lblCapturarFiniquito.text(_empresaActual == 6 || _empresaActual == "3" ? "Liquidacion" : "Finiquito");
                cboCEFiniquitoTipo.val(0);
                cboCEFiniquitoTipo.trigger("change");
                mdlCapturarFiniquito.modal("show");
            });

            btnCapturarRecalculo.on("click", function () {
                txtCEPathFile.val("");
                txtCEFiniquitoMonto.val(0);
                lblCapturarFiniquito.text("Recalculo");
                cboCEFiniquitoTipo.val(1);
                cboCEFiniquitoTipo.trigger("change");
                mdlCapturarFiniquito.modal("show");
            });

            btnCapturarCierre.on("click", function () {
                txtCEPathFile.val("");
                txtCEFiniquitoMonto.val(0);
                lblCapturarFiniquito.text("Cierre");
                cboCEFiniquitoTipo.val(2);
                cboCEFiniquitoTipo.trigger("change");
                mdlCapturarFiniquito.modal("show");
            });

            btnCapturarIMS.on("click", function () {
                txtCEPathFile.val("");
                txtCEFiniquitoMonto.val(0);
                lblCapturarFiniquito.text(_empresaActual == 6 ? "SUNAT" : "IMS");
                cboCEFiniquitoTipo.val(3);
                cboCEFiniquitoTipo.trigger("change");
                mdlCapturarFiniquito.modal("show");
            });

            btnImprimirFiniquito.on("click", function () {
                location.href = `/Administrativo/BajasPersonal/DescargarArchivoFiniquito?baja_id=` + btnImprimirFiniquito.attr("data-id") + "&tipoFiniquito=0";
            });

            btnImprimirFiniquitoRecalc.on("click", function () {
                location.href = `/Administrativo/BajasPersonal/DescargarArchivoFiniquito?baja_id=` + btnImprimirFiniquito.attr("data-id") + "&tipoFiniquito=1";
            });

            btnImprimirFiniquitoCierre.on("click", function () {
                location.href = `/Administrativo/BajasPersonal/DescargarArchivoFiniquito?baja_id=` + btnImprimirFiniquito.attr("data-id") + "&tipoFiniquito=2";
            });

            btnImprimirFiniquitoIMS.on("click", function () {
                location.href = `/Administrativo/BajasPersonal/DescargarArchivoFiniquito?baja_id=` + btnImprimirFiniquito.attr("data-id") + "&tipoFiniquito=3";

            });

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

            $(document).on('change', ':file', function () {
                var input = $(this),
                    numFiles = input.get(0).files ? input.get(0).files.length : 1,
                    label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
                input.trigger('fileselect', [numFiles, label]);
            });

            $(document).ready(function () {
                $(':file').on('fileselect', function (event, numFiles, label) {

                    var input = $(this).parents('.input-group').find(':text'),
                        log = numFiles > 1 ? numFiles + ' files selected' : label;

                    if (input.length) {
                        input.val(log);
                    } else {
                        if (log) alert(log);
                    }
                });
            });
            // inCapturarFiniquito.on('fileselect', function(event, numFiles, label) {

            //     txtCEPathFile.val(label);

            // });

            // txt_registro_nombre_autoriza.on("change", function () {
            //     txt_registro_numeroEmpleado_baja.val($(this).val());
            // });

            //#region FILL COMBO (REC)

            cboCEDatosEmpleadoPaisNac.fillCombo("/Administrativo/Reclutamientos/FillCboPaises", {}, false);
            cboCEDatosEmpleadoEstadoNac.fillCombo("/Administrativo/Reclutamientos/FillCboEstados", { _clavePais: 0 }, false);
            cboCEDatosEmpleadoLugarNac.fillCombo("/Administrativo/Reclutamientos/FillCboMunicipios", { _clavePais: 0, _claveEstado: 0 }, false);

            cboCEDatosEmpleadoPaisNac.on("change", function () {
                if ($(this).val() != "") {
                    cboCEDatosEmpleadoEstadoNac.fillCombo("/Administrativo/Reclutamientos/FillCboEstados", { _clavePais: parseFloat($(this).val()) }, false);
                }
            });

            cboCEDatosEmpleadoEstadoNac.on("change", function () {
                if ($(this).val() != "") {
                    cboCEDatosEmpleadoLugarNac.fillCombo("/Administrativo/Reclutamientos/FillCboMunicipios", { _clavePais: cboCEDatosEmpleadoPaisNac.val(), _claveEstado: $(this).val() }, false);
                }
            });

            // cboCEBeneficiarioEstado.on("change", function () {
            //     if ($(this).val() != "") {
            //         cboCEBeneficiarioCiudad.fillCombo("/Administrativo/Reclutamientos/FillCboMunicipios", { _clavePais: cboCEDatosEmpleadoPaisNac.val(), _claveEstado: $(this).val() }, false);
            //     }
            // });

            cboCEDatosEmpleadoPaisNac.select2({ width: "100%" });
            cboCEDatosEmpleadoEstadoNac.select2({ width: "100%" });
            cboCEDatosEmpleadoLugarNac.select2({ width: "100%" });
            cboCEDatosEmpleadoSexo.select2({ width: "100%" });

            cboCandidatosAprobados.fillCombo("/Administrativo/Reclutamientos/FillCboCandidatosAprobados", {}, false);
            cboCandidatosAprobados.select2({ width: "50%" });

            cboCEGenContactoEstadoCivil.append($("<option />").val("--Seleccione--").text("--Seleccione--"));
            cboCEGenContactoEstadoCivil.append($("<option />").val("Casado").text("Casado"));
            cboCEGenContactoEstadoCivil.append($("<option />").val("Divorciado").text("Divorciado"));
            cboCEGenContactoEstadoCivil.append($("<option />").val("Soltero").text("Soltero"));
            cboCEGenContactoEstadoCivil.append($("<option />").val("Union Libre").text("Unión libre"));
            cboCEGenContactoEstadoCivil.append($("<option />").val("Viudo").text("Viudo"));
            cboCEGenContactoEstadoCivil.select2({ width: "100%" });

            cboCEGenContactoEstado.select2({ width: "100%" });
            cboCEGenContactoEstado.fillCombo("/Administrativo/Reclutamientos/FillCboEstados", { _clavePais: 0 }, false);
            cboCEGenContactoCiudad.select2({ width: "100%" });
            cboCEGenContactoCiudad.fillCombo("/Administrativo/Reclutamientos/FillCboMunicipios", { _clavePais: 0, _claveEstado: 0 }, false);

            cboCEGenContactoEstado.on("change", function () {
                if ($(this).val() != "") {
                    cboCEGenContactoCiudad.select2({ width: "100%" });
                    cboCEGenContactoCiudad.fillCombo("/Administrativo/Reclutamientos/FillCboMunicipios", { _clavePais: cboCEDatosEmpleadoPaisNac.val(), _claveEstado: $(this).val() }, false);
                }
            });

            cboCEDatosEmpleadoPaisNac.on("change", function () {
                if ($(this).val() != "") {
                    cboCEGenContactoEstado.select2({ width: "100%" });
                    cboCEGenContactoEstado.fillCombo("/Administrativo/Reclutamientos/FillCboEstados", { _clavePais: parseFloat($(this).val()) }, false);
                }
            });

            cboCEBeneficiarioParentesco.select2({ width: "100%" });
            cboCEBeneficiarioParentesco.fillCombo("/Administrativo/Reclutamientos/FillCboParentesco", {}, false);

            cboCEBeneficiarioCiudad.fillCombo("/Administrativo/Reclutamientos/FillCboMunicipios", { _clavePais: 0, _claveEstado: 0 }, false);
            cboCEBeneficiarioCiudad.select2({ width: "100%" });

            cboCECompaniaTipoFormula.select2({ width: "100%" });
            cboCECompaniaTipoFormula.fillCombo("/Administrativo/Reclutamientos/FillCboTipoFormulaIMSS", {}, false);

            cboCEGenContactoTipoSangre.select2({ width: "100%" });
            cboCEGenContactoTipoSangre.fillCombo("/Administrativo/Reclutamientos/FillCboTipoSangre", {}, false);

            cboCEGenContactoTipoCasa.select2({ width: "100%" });
            cboCEGenContactoTipoCasa.fillCombo("/Administrativo/Reclutamientos/FillCboTipoCasa", {}, false);

            cboCETabuladorTipoNomina.select2({ width: "100%" });

            cboCETabuladorBanco.fillCombo("/Administrativo/Reclutamientos/FillCboBancos", {}, false);
            cboCETabuladorBanco.select2({ width: "100%" });

            txtCECompaniaCCDescripcion.on('change', function () {
                let cc = txtCECompaniaCCDescripcion.data('cc');

                selectRegistroPatronal.fillCombo('/Administrativo/BajasPersonal/FillComboRegistroPatronal', { cc }, false, null);
            });

            selectRegistroPatronal.fillCombo('/Administrativo/BajasPersonal/FillComboRegistroPatronal', { cc: '' }, false, null);
            selectTipoContrato.fillCombo('/Administrativo/BajasPersonal/FillComboDuracionContrato', null, false, null);
            //#endregion

            cbo_registro_contratable.on("change", function () {
                if ($(this).val() == "0") {
                    div_registro_comentariosRecontratable.show();
                } else {
                    div_registro_comentariosRecontratable.hide();

                }
            });

            chk_registro_esAnticipada.on("change", function () {

                if (txt_registro_fechaBaja.attr('max') != null) {
                    fechaMaxBaja = txt_registro_fechaBaja.attr('max');

                }

                if (fechaMinBaja == null) {
                    fechaMinBaja = txt_registro_fechaBaja.attr('min');
                }

                if ($(this).prop("checked")) {
                    txt_registro_fechaBaja.attr('max', null);
                    txt_registro_fechaBaja.attr('min', moment().add(1, "w").format("YYYY-MM-DD"));
                    txt_registro_fechaBaja.val(moment().add(1, "w").format("YYYY-MM-DD"));


                } else {
                    txt_registro_fechaBaja.attr('max', fechaMaxBaja);
                    txt_registro_fechaBaja.attr('min', fechaMinBaja);
                    txt_registro_fechaBaja.val(moment().format("YYYY-MM-DD"));


                }
            });
            // chk_registro_esAnticipada.prop("checked")

            cbo_registro_regresariaALaEmpresa.on("change", function () {
                if ($(this).val() == 1) {
                    divRegresariaEmpresa.show();
                } else {
                    divRegresariaEmpresa.hide();

                }
            });

            if (esUsuarioNotificar) {
                btnFiltroNotificar.show();
            } else {
                btnFiltroNotificar.hide();

            }

            btnFiltroNotificar.on("click", function () {
                let arrRowsChecked = [];
                let rowsChecked = tblRH_Baja_Registro.DataTable().column(0).checkboxes.selected();
                $.each(rowsChecked, function (index, claveEmpleado) {
                    arrRowsChecked.push(claveEmpleado)
                });

                Alert2AccionConfirmar('¡Cuidado!', "Desea notificar las bajas seleccionadas?", 'Confirmar', 'Cancelar', () => {
                    if (arrRowsChecked.length > 0) {
                        axios.post("/Administrativo/BajasPersonal/NotificarBajas", { lstClavesEmps: arrRowsChecked }).then(response => {
                            let { success, items, message } = response.data;
                            if (success) {
                                Alert2Exito("Bajas notificadas con exito");
                                fncGetBajasPersonal();
                            }
                        }).catch(error => Alert2Error(error.message));
                    } else {
                        Alert2Warning("Favor de seleccionar bajas a notificar");
                    }
                });

            });

        }

        function fncVisualizarDocumento(idBaja, tipoFiniquito) {
            let obj = new Object();
            obj.idBaja = idBaja;
            obj.tipoFiniquito = tipoFiniquito;
            axios.post("/Administrativo/BajasPersonal/VisualizarDocumento", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    $('#myModal').data().ruta = null;
                    $('#myModal').modal('show');
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function funGetGerente(event, ui) {
            txt_entrevista_gerenteClave.val(ui.item.id);
            txt_entrevista_gerenteNombre.val(ui.item.value);

        }
        function funGetEmpleado(event, ui) {
            txt_registro_numeroEmpleado.val(ui.item.id);
            txt_registro_nombre.val(ui.item.value);
            txt_registro_numeroEmpleado.change();
        }
        function funGetEmpleado_baja(event, ui) {
            txt_registro_numeroEmpleado_baja.val(ui.item.id);
            txt_registro_nombre_baja.val(ui.item.value);
            txt_registro_numeroEmpleado_baja.change();
        }
        //#region CRUD BAJA
        function initTblBajasPersonal() {
            dtBajas = tblRH_Baja_Registro.DataTable({
                language: dtDicEsp,
                destroy: false,
                "sScrollX": "100%",
                "sScrollXInner": "100%",
                "bScrollCollapse": true,
                scrollY: '65vh',
                scrollCollapse: true,
                "bLengthChange": false,
                "searching": true,
                "bFilter": true,
                "bInfo": true,
                "bAutoWidth": false,
                order: [13, 'desc'],
                dom: 'Bfrtip',
                buttons: parametrosImpresion("Reporte de Bajas", "<center><h3>Bajas de Personal</h3></center>"),
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        exportOptions: {
                            columns: _empresaActual == 6 || _empresaActual == 3 ? [0, 1, 2, 3, 4, 5, 6, 7, 8, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21] : [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21],
                            // columns: [1, 2, 3, 4, 5, 6, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61]
                        }

                    }
                ],
                columns: [
                    { data: 'numeroEmpleado' },
                    {
                        data: 'fechaCreacion', title: 'FECHA CAPTURA', render: function (data, type, row, meta) {
                            if (moment(data).year() > 1) {

                                return moment(data).format("DD/MM/YYYY");
                            } else {
                                return moment(row.fechaBaja).format("DD/MM/YYYY");

                            }
                        }
                    },
                    { data: 'numeroEmpleado', title: '#' },
                    { data: 'nombre', title: 'NOMBRE' },
                    { data: 'nombrePuesto', title: 'PUESTO' },
                    {
                        title: 'CC', render: function (data, type, row, meta) {
                            return '[' + row.cc + '] ' + row.descripcionCC;
                        }
                    },
                    {
                        data: 'usuarioCreacion_Nombre', title: 'CAPTURO', visible: !esUsuarioTeorico,
                        render: function (data, type, row) {
                            return (data ?? "enKontrol").toUpperCase();
                        }
                    },
                    { data: 'telPersonal', title: 'CELULAR', visible: !esUsuarioTeorico, },
                    { data: 'motivoBajaDeSistema', title: 'MOTIVO BAJA' },
                    {
                        data: 'curp', title: _empresaActual == 6 ? 'DNI' : _empresaActual == 3 ? 'CEDULA CIUD.' : 'CURP', visible: false, visible: !esUsuarioTeorico,
                        render: function (data, type, row) {
                            if (_empresaActual == 6) {
                                return row.dni;

                            } else {
                                if (_empresaActual == 3) {
                                    return row.cedula_ciudadania;

                                } else {
                                    return data;

                                }
                            }
                        }
                    },
                    { data: 'rfc', title: 'RFC', visible: false, visible: !esUsuarioTeorico, },
                    { data: 'nss', title: 'NSS', visible: false, visible: !esUsuarioTeorico, },
                    {
                        data: 'fechaIngreso', title: 'FECHA ALTA',
                        render: function (data, type, row) {
                            if (type === 'display') {
                                if (data) {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                                else {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                            }
                            else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'fechaBaja', title: 'FECHA BAJA',
                        render: function (data, type, row) {
                            if (type === 'display') {
                                if (data) {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                                else {
                                    return moment(data).format("DD/MM/YYYY");
                                }
                            }
                            else {
                                return data;
                            }
                        }
                    },
                    {
                        data: "esContratable", title: "CONTRATABLE", visible: true,
                        render: function (data, type, row) {
                            if (data) {
                                return "SI";
                            } else {
                                return "NO";
                            }
                        }
                    },
                    { data: 'est_baja', title: 'BAJA', visible: false, },
                    { data: 'est_inventario', title: 'ALMACEN', visible: false },
                    { data: 'est_compras', title: 'TALLER', visible: false },
                    { data: 'est_contabilidad', title: 'CONTABILIDAD', visible: false },
                    {
                        data: 'rutaCierre', title: 'FINIQUITO', visible: false,
                        render: function (data, type, row) {
                            if (data == null) {
                                return `P`;
                            } else {
                                return `A`;
                            }
                        }
                    },
                    {
                        data: 'rutaIMS', title: _empresaActual == 6 ? "BAJA SUNAT" : 'BAJAS IMSS', visible: false,
                        render: function (data, type, row) {
                            if (data == null) {
                                return `P`;
                            } else {
                                return `A`;
                            }
                        }
                    },
                    {
                        data: 'est_baja', title: 'BAJA CAPTURADA', width: '10px',
                        render: function (data, type, row) {
                            if (data == 'A') {
                                return `<button class="btn btn-xs btn-success botonRedondo" data-id='${row.id}' data-tipo='1'><i class="fa fa-check"></i></button>`;
                            } else {
                                return `<button class="btn btn-xs btn-warning botonRedondo" disabled><i class="fa fa-clock"></i></button>`;
                            }
                        }
                    },
                    {
                        data: 'est_inventario', title: 'LIBERACION ALMACEN', width: '10px',
                        render: function (data, type, row) {
                            if (data == 'A') {
                                return `<button class="btn btn-xs btn-success botonRedondo detalleAut" data-id='${row.id}' data-tipo='2'><i class="fa fa-check"></i></button>`;
                            } else {
                                return `<button class="btn btn-xs btn-warning botonRedondo" disabled><i class="fa fa-clock"></i></button>`;
                            }
                        }
                    },
                    {
                        data: 'est_compras', title: 'LIBERACION TALLER', width: '10px',
                        render: function (data, type, row) {
                            if (data == 'A') {
                                return `<button class="btn btn-xs btn-success botonRedondo detalleAut" data-id='${row.id}' data-tipo='4'><i class="fa fa-check"></i></button>`;
                            } else {
                                return `<button class="btn btn-xs btn-warning botonRedondo" disabled><i class="fa fa-clock"></i></button>`;
                            }
                        }
                    },
                    {
                        data: 'est_contabilidad', title: 'LIBERACION CONTABILIDAD', width: '10px',
                        render: function (data, type, row) {
                            if (data == 'A') {
                                return `<button class="btn btn-xs btn-success botonRedondo detalleAut" data-id='${row.id}' data-tipo='3'><i class="fa fa-check"></i></button>`;
                            } else {
                                return `<button class="btn btn-xs btn-warning botonRedondo" disabled><i class="fa fa-clock"></i></button>`;
                            }
                        }
                    },
                    {
                        data: 'est_nominas', title: 'CHEQUE/ TRANSEFERENCIA FINIQUITO (NOMINAS)', width: '10px',
                        render: function (data, type, row) {
                            if (data == 'A') {
                                return `<button class="btn btn-xs btn-success botonRedondo detalleAut" data-id='${row.id}' data-tipo='5'><i class="fa fa-check"></i></button>`;
                            } else {
                                return `<button class="btn btn-xs btn-warning botonRedondo" disabled><i class="fa fa-clock"></i></button>`;
                            }
                        }
                    },
                    {
                        data: 'esPendienteNoti', title: 'ENVIO LAYOUT BAJA', width: '10px',
                        render: function (data, type, row) {
                            if (!data) {
                                return `<button class="btn btn-xs btn-success botonRedondo "><i class="fa fa-check"></i></button>`;
                            } else {
                                return `<button class="btn btn-xs btn-warning botonRedondo" disabled><i class="fa fa-clock"></i></button>`;
                            }
                        }
                    },
                    {
                        title: _empresaActual == 6 || _empresaActual == 3 ? "LIQUIDACION" : 'EVIDENCIA FINIQUITO', width: '10px',

                        //#region V1
                        render: function (data, type, row) {
                            if (row.rutaCierre != null && row.rutaFiniquito != null) {
                                return `<button class="btn btn-xs btn-success botonRedondo verFiniquito" data-id='${row.id}' data-tipo='1'><i class="fa fa-check"></i></button>`;
                            } else {
                                if (row.rutaCierre == null && row.rutaFiniquito == null) {
                                    return `<button class="btn btn-xs btn-warning botonRedondo verFiniquito" data-id='${row.id}' data-tipo='1'><i class="fa fa-clock"></i></button>`;

                                } else {
                                    return `<button style="background-color: #fd7e14;" class="btn btn-xs botonRedondo verFiniquito" data-id='${row.id}' data-tipo='1'><i  style="background-color: #fd7e14; color: white;" class="fa fa-clock"></i></button>`;

                                }
                            }
                        }
                        //#endregion
                        // render: function (data, type, row) {
                        //     if (row.rutaFiniquito != null) {
                        //         return `<button class="btn btn-xs btn-success botonRedondo verFiniquito" data-id='${row.id}' data-tipo='1'><i class="fa fa-check"></i></button>`;
                        //     } else {
                        //         return `<button class="btn btn-xs btn-warning botonRedondo verFiniquito" data-id='${row.id}' data-tipo='1'><i class="fa fa-clock"></i></button>`;
                        //         // if (row.rutaFiniquito == null) {

                        //         // } else {
                        //         //     return `<button style="background-color: #fd7e14;" class="btn btn-xs botonRedondo verFiniquito" data-id='${row.id}' data-tipo='1'><i  style="background-color: #fd7e14; color: white;" class="fa fa-clock"></i></button>`;

                        //         // }
                        //     }
                        // }
                    },
                    {
                        title: _empresaActual == 6 ? "BAJA SUNAT" : 'CONFIRMACION BAJA IMSS', width: '10px',
                        render: function (data, type, row) {
                            if (row.rutaIMS != null) {
                                return `<button class="btn btn-xs btn-success botonRedondo verFiniquitoImss" data-id='${row.id}' data-tipo='1'><i class="fa fa-check"></i></button>`;
                            } else {
                                return `<button class="btn btn-xs btn-warning botonRedondo verFiniquitoImss" data-id='${row.id}' data-tipo='1'><i class="fa fa-clock"></i></button>`;
                                // if (row.rutaIMS == null) {

                                // } else {
                                //     return `<button style="background-color: #fd7e14;" class="btn btn-xs botonRedondo verFiniquito" data-id='${row.id}' data-tipo='1'><i  style="background-color: #fd7e14; color: white;" class="fa fa-clock"></i></button>`;

                                // }
                            }
                        }
                    },
                    {
                        data: 'cantidadActos', title: 'CANT. ACTOS',
                        render: function (data, type, row) {
                            return `<button class="btn  btn-xs btn-primary btn-sm getActos">Actos: ${data}</button>`;
                        }
                    },
                    {
                        data: 'cantidadCursos', title: 'CANT. CAPACITACIONES',
                        render: function (data, type, row) {
                            return `<button class="btn  btn-xs btn-primary btn-sm getCapacitaciones">Caps: ${data}</button>`;
                        }
                    },
                    {
                        data: 'cantidadHistorico', title: 'HISTORICO CC',
                        render: function (data, type, row) {
                            return `<button class="btn  btn-xs btn-primary btn-sm getHistorial">CCs: ${data}</button>`;
                        }
                    },
                    {
                        render: (data, type, row, meta) => {
                            return `<button title="Imprimir aviso de baja" class="btn btn-primary btn-xs imprimirDoc"><i class="fas fa-print"></i></button>`;
                        }
                    },
                    {
                        render: function (data, type, row) {
                            //#region V1
                            // return `
                            //     ${!(row.rutaCierre != null) ? !(row.est_baja == "P" || row.est_inventario == "P" || row.est_contabilidad == "P" || row.est_compras == "P") ? `<button title="Capturar finiquito" class="btn btn-sm btn-${row.numDiasFiniquito < 10 ? "warning" : row.numDiasFiniquito < 30 ? "warning buttonOrange" : "danger"} capturarFiniquito btn-xs"><i class="far fa-file"></i></button>` : `` : ``}
                            //     <button title="Ver entrevista" class="btn btn-sm btn-${row.gerente_clave > 0 ? 'success' : 'primary'} verEntrevista btn-xs" ${row.estado_civil_clave > 0 ? '' : 'disabled'}><i class="far fa-comments"></i></button>
                            //     <button title="Mandar entrevista" class="btn btn-sm btn-primary mandarEntrevista btn-xs" ${!row.estado_civil_clave > 0 ? '' : 'disabled'}><i class="fas fa-link"></i></button>
                            //     ${row.autorizada != 0 && row.entrevista_id > 0 ?
                            //         `<button title="Ver datos de baja" class="btn btn-sm btn-default verBajaPersonal btn-xs"><i class="far fa-eye"></i></button>` :
                            //         `<button title="Actualizar datos de baja" class="btn btn-sm btn-warning actualizarBajaPersonal btn-xs"><i class="far fa-edit"></i></button>`}
                            //     <button title="Ver captura de Baja" class="btn btn-sm btn-primary verBajaCaptura btn-xs"><b>E</b></button>
                            //     ${row.comentariosCancelacion != null && row.esCancelar && row.est_baja == "P" ? `<button title="Ver comentario de cancelado" class="btn btn-sm btn-info verComentarioCancelado btn-xs"><i class="far fa-comments"></i></button>` : ``}
                            //     ${row.esCancelar && row.est_baja == "A" && !(row.est_inventario == "A" || row.est_contabilidad == "A" || row.est_compras == "A") ? `<button title="Cancelar Baja" class="btn btn-sm btn-danger cancelarBaja btn-xs"><i class="fas fa-user-times"></i></button>` : ``}
                            //     ${(row.est_baja == "P" && row.est_inventario == "P" && row.est_contabilidad == "P" && row.est_compras == "P") ? `<button title="Eliminar Baja" class="btn btn-sm btn-danger eliminarBajaPersonal btn-xs"><i class="far fa-trash-alt"></i></button> ` : ""}
                            // `;
                            //#endregion

                            if (!esUsuarioTeorico) {
                                return `
                                    ${!(row.rutaCierre != null) ? !(row.est_baja == "P" || row.est_inventario == "P" || row.est_contabilidad == "P" || row.est_compras == "P") ? `<button title="Capturar finiquito" class="btn btn-sm btn-${row.numDiasFiniquito < 10 ? "warning" : row.numDiasFiniquito < 30 ? "warning buttonOrange" : "danger"} capturarFiniquito btn-xs"><i class="far fa-file"></i></button>` : `` : ``}
                                    <button title="Ver entrevista" class="btn btn-sm btn-${row.gerente_clave > 0 ? 'success' : 'primary'} verEntrevista btn-xs" ${row.estado_civil_clave > 0 ? '' : 'disabled'}><i class="far fa-comments"></i></button>
                                    <button title="Mandar entrevista" class="btn btn-sm btn-primary mandarEntrevista btn-xs" ${!row.estado_civil_clave > 0 ? '' : 'disabled'}><i class="fas fa-link"></i></button>
                                    ${row.autorizada != 0 && row.entrevista_id > 0 ?
                                        `<button title="Ver datos de baja" class="btn btn-sm btn-default verBajaPersonal btn-xs"><i class="far fa-eye"></i></button>` :
                                        `<button title="Actualizar datos de baja" class="btn btn-sm btn-warning actualizarBajaPersonal btn-xs"><i class="far fa-edit"></i></button>`}
                                    <button title="Ver captura de Baja" class="btn btn-sm btn-primary verBajaCaptura btn-xs"><b>E</b></button>
                                    ${row.comentariosCancelacion != null && row.esCancelar && row.est_baja == "P" ? `<button title="Ver comentario de cancelado" class="btn btn-sm btn-info verComentarioCancelado btn-xs"><i class="far fa-comments"></i></button>` : ``}
                                    ${(row.esDiana || row.esCancelar) && row.est_baja == "A" ? `<button title="Cancelar Baja" class="btn btn-sm btn-danger cancelarBaja btn-xs"><i class="fas fa-user-times"></i></button>` : ``}
                                    ${row.esDiana ? `<button title="Eliminar Baja" class="btn btn-sm btn-danger eliminarBajaPersonal btn-xs"><i class="far fa-trash-alt"></i></button> ` : ""}
                                    ${(esUsuarioLiberarNominas && row.est_nominas != "A" && row.est_inventario == "A" && row.est_contabilidad == "A" && row.est_compras == "A") ? `<button title="Liberar Nomina" class="btn btn-sm btn-info liberarNomina btn-xs"><i class="fas fa-user-check"></i></button> ` : ""}
                                `;
                                // let btnEliminar = `< button title = "Eliminar Baja" class= "btn btn-sm btn-danger eliminarBajaPersonal btn-xs" > <i class="far fa-trash-alt"></i></button > `;
                            } else {
                                return ``;
                            }
                        }
                    }
                ],
                columnDefs: [
                    // { "width": "12%", "targets": 10 },
                    // { width: '15%', targets: 23 }
                    {
                        targets: 0,
                        checkboxes: {
                            selectRow: true
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblRH_Baja_Registro.on('click', '.actualizarBajaPersonal', function () {
                        lblTituloCrearEditarBaja.text("ACTUALIZAR BAJA");

                        let rowData = dtBajas.row($(this).closest('tr')).data();
                        fncBorderDefault();
                        fncGetDatosActualizarBajaPersonal(rowData.id);
                        lblCantActos.html(rowData.cantidadActos);
                        lblCantCapacitaciones.html(rowData.cantidadCursos);
                        fieldRegistro.show();
                        fieldEntrevista.show();
                        btnCEBajaPersonal.show();

                        _bajaAutorizada = rowData.autorizada;
                        _bajaEntrevista_id = rowData.entrevista_id;

                        fieldRegistro.find('input, select, textarea').each(function (idx, element) {
                            $(element).prop('disabled', !rowData.autorizada == 0);
                        });

                        fieldRegistro.find('input, select, textarea').each(function (idx, element) {
                            $(element).prop('disabled', rowData.est_baja == 'A');
                        });

                        fieldEntrevista.find('input, select, textarea').each(function (idx, element) {
                            $(element).prop('disabled', rowData.entrevista_id > 0);
                            // $()
                        });

                        txt_registro_numeroEmpleado_baja.attr("disabled", true);
                        txt_registro_nombre_baja.attr("disabled", true);

                        if (rowData.esDiana) {
                            txt_registro_fechaBaja.prop("disabled", false);
                            txt_registro_nombre_autoriza.prop("disabled", false)
                        }

                        cbo_registro_motivoBajaSistema.prop('disabled', false);
                        txt_registro_motivoSeparacionEmpresa.prop('disabled', false);

                        mdlBaja.modal("show");
                    });
                    tblRH_Baja_Registro.on('click', '.verBajaPersonal', function () {
                        lblTituloCrearEditarBaja.text("VER BAJA");

                        let rowData = dtBajas.row($(this).closest('tr')).data();

                        fncGetDatosActualizarBajaPersonal(rowData.id);
                        lblCantActos.html(rowData.cantidadActos);
                        lblCantCapacitaciones.html(rowData.cantidadCursos);
                        fieldRegistro.show();
                        fieldEntrevista.show();
                        btnCEBajaPersonal.hide();

                        _bajaAutorizada = rowData.autorizada;
                        _bajaEntrevista_id = rowData.entrevista_id;


                        fieldRegistro.find('input, select, textarea').each(function (idx, element) {
                            $(element).prop('disabled', !rowData.autorizada == 0);
                        });

                        fieldEntrevista.find('input, select, textarea').each(function (idx, element) {
                            $(element).prop('disabled', rowData.entrevista_id > 0);
                        });



                        mdlBaja.modal("show");
                    });
                    tblRH_Baja_Registro.on('click', '.eliminarBajaPersonal', function () {
                        let rowData = dtBajas.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarBajaPersonal(rowData.id));
                    });
                    tblRH_Baja_Registro.on('click', '.getActos', function () {
                        let rowData = dtBajas.row($(this).closest('tr')).data();
                        fncGetActosCapacitaciones(rowData.numeroEmpleado, "ACTO");
                    });
                    tblRH_Baja_Registro.on('click', '.getCapacitaciones', function () {
                        let rowData = dtBajas.row($(this).closest('tr')).data();
                        fncGetActosCapacitaciones(rowData.numeroEmpleado, "CAPACITACIONES");
                    });
                    tblRH_Baja_Registro.on('click', '.getHistorial', function () {
                        let rowData = dtBajas.row($(this).closest('tr')).data();
                        fncGetHistorialCC(rowData.numeroEmpleado);
                    });
                    tblRH_Baja_Registro.on('click', '.detalleAut', function () {
                        let rowData = dtBajas.row($(this).closest('tr')).data();
                        let tipo = $(this).data("tipo");

                        if (!esUsuarioTeorico) {
                            switch (tipo) {
                                case 1:
                                    if (moment(rowData.est_baja_fecha).year() > 0) {
                                        txt_aut_fecha.val(moment(rowData.est_baja_fecha).format("DD/MM/YYYY"));
                                    }
                                    txt_aut_Usuario.val(rowData.est_baja_nombre);
                                    txt_aut_comentario.val(rowData.est_baja_comentario == "" || rowData.est_baja_comentario == null ? "LIBERADO" : rowData.est_baja_comentario);
                                    break;

                                case 2:
                                    if (moment(rowData.est_inventario_fecha).year() > 0) {
                                        txt_aut_fecha.val(moment(rowData.est_inventario_fecha).format("DD/MM/YYYY"));
                                    }
                                    txt_aut_Usuario.val(rowData.est_inventario_nombre);
                                    txt_aut_comentario.val(rowData.est_inventario_comentario == "" || rowData.est_inventario_comentario == null ? "LIBERADO" : rowData.est_inventario_comentario);
                                    break;
                                case 3:
                                    if (moment(rowData.est_contabilidad_fecha).year() > 0) {
                                        txt_aut_fecha.val(moment(rowData.est_contabilidad_fecha).format("DD/MM/YYYY"));
                                    }
                                    txt_aut_Usuario.val(rowData.est_contabilidad_nombre);
                                    txt_aut_comentario.val(rowData.est_contabilidad_comentario == "" || rowData.est_contabilidad_comentario == null ? "LIBERADO" : rowData.est_contabilidad_comentario);
                                    break;
                                case 4:
                                    if (moment(rowData.est_compras_fecha).year() > 0) {
                                        txt_aut_fecha.val(moment(rowData.est_compras_fecha).format("DD/MM/YYYY"));
                                    }
                                    txt_aut_Usuario.val(rowData.est_compras_nombre);
                                    txt_aut_comentario.val(rowData.est_compras_comentario == "" || rowData.est_compras_comentario == null ? "LIBERADO" : rowData.est_compras_comentario);
                                    break;
                                case 5:
                                    if (moment(rowData.est_nominas_fecha).year() > 0) {
                                        txt_aut_fecha.val(moment(rowData.est_nominas_fecha).format("DD/MM/YYYY"));
                                    }
                                    txt_aut_Usuario.val(rowData.est_nominas_nombre);
                                    txt_aut_comentario.val(rowData.est_nominas_comentario == "" || rowData.est_nominas_comentario == null ? "LIBERADO" : rowData.est_nominas_comentario);
                                    break;

                                default:
                                    break;
                            }

                            mdlDetalleaut.modal("show");
                        }



                        // fnDetalleAut(rowData);
                    });
                    tblRH_Baja_Registro.on('click', '.verEntrevista', function () {
                        let rowData = dtBajas.row($(this).closest('tr')).data();
                        fncGetDatosActualizarBajaPersonal(rowData.id);
                        lblCantActos.html(rowData.cantidadActos);
                        lblCantCapacitaciones.html(rowData.cantidadCursos);
                        mdlBaja.modal("show");
                        btnCEBajaPersonal.hide();
                        fieldRegistro.hide();
                        fieldEntrevista.show();

                        fieldEntrevista.find('input, select, textarea').each(function (idx, element) {
                            $(element).prop('disabled', true);
                        });

                        lblTituloCrearEditarBaja.text("ENTREVISTA");
                    });
                    tblRH_Baja_Registro.on('click', '.mandarEntrevista', function () {
                        let rowData = dtBajas.row($(this).closest('tr')).data();
                        mdlCompartir.modal("show");
                        txtCompartirCorreo.val(rowData.correo);
                        btnCompartirEnviar.attr('data-email', rowData.correo);
                        btnCompartirEnviar.attr('data-user', rowData.id);
                        txtCompartirLink.val(`http://sigoplan.construplan.com.mx/Administrativo/BajasPersonalEntrevista/Entrevista?id=${rowData.id}&empresa=${_empresaActual}`);
                        txtCompartirSubtitulo.text(`Numero Empleado: ${rowData.numeroEmpleado}. Nombre: ${rowData.nombre}. Fecha de Baja: ${moment(rowData.fechaBaja).format("DD/MM/YYYY")}.`);
                    });
                    tblRH_Baja_Registro.on('click', '.capturarFiniquito', function () {
                        let rowData = dtBajas.row($(this).closest('tr')).data();
                        // mdlCapturarFiniquito.modal("show");
                        // btnGuardarFiniquito.attr("data-id", rowData.id);
                        // btnGuardarFiniquito.attr("data-finiquito", rowData.rutaFiniquito);
                        // txtCEFiniquitoMonto.val(0);
                        // inCapturarFiniquito.val("");
                        // txtCEPathFile.val("")
                        // cboCEFiniquitoTipo.val("0")
                        // cboCEFiniquitoTipo.change();
                    });
                    tblRH_Baja_Registro.on('click', '.verFiniquito', function () {
                        let rowData = dtBajas.row($(this).closest('tr')).data();
                        mdlFiniquito.modal("show");
                        btnGuardarFiniquito.attr("data-id", rowData.id);
                        btnImprimirFiniquito.attr("data-id", rowData.id);
                        txtVerMonto.val(rowData.montoInicial);
                        txtVerMontoRecalc.val(rowData.montoRecalc);
                        txtVerMontoCierre.val(rowData.montoCierre);
                        txtVerMontoIMS.val(rowData.montoIMS);

                        if (!rowData.rutaFiniquito) {
                            btnImprimirFiniquito.attr("disabled", true);
                            spanCapturarFiniquito.text("Capturar");
                            txtVerNombreFiniquito.val("");
                            btnVerFiniquito.attr("disabled", true);
                        } else {
                            btnImprimirFiniquito.attr("disabled", false);
                            spanCapturarFiniquito.text("Actualizar");
                            txtVerNombreFiniquito.val(rowData.archivoFiniquito);
                            btnVerFiniquito.attr("disabled", false);
                        }

                        if (!rowData.rutaRecalc) {
                            btnImprimirFiniquitoRecalc.attr("disabled", true);
                            spanCapturarRecalculo.text("Capturar");
                            txtVerNombreRecalculo.val("");
                            btnVerFiniquitoRecalc.attr("disabled", true);
                        } else {
                            btnImprimirFiniquitoRecalc.attr("disabled", false);
                            spanCapturarRecalculo.text("Actualizar");
                            txtVerNombreRecalculo.val(rowData.archivoRecalc);
                            btnVerFiniquitoRecalc.attr("disabled", false);
                        }

                        if (!rowData.rutaCierre) {
                            btnImprimirFiniquitoCierre.attr("disabled", true);
                            spanCapturarCierre.text("Capturar");
                            txtVerNombreCierre.val("");
                            btnVerFiniquitoCierre.attr("disabled", true);
                        } else {
                            btnImprimirFiniquitoCierre.attr("disabled", false);
                            spanCapturarCierre.text("Actualizar");
                            txtVerNombreCierre.val(rowData.archivoCierre);
                            btnVerFiniquitoCierre.attr("disabled", false);
                        }

                        if (!rowData.rutaIMS) {
                            btnImprimirFiniquitoIMS.attr("disabled", true);
                            spanCapturarIMS.text("Capturar");
                            txtVerNombreIMS.val("");
                            btnVerFiniquitoIMS.attr("disabled", true);
                        } else {
                            btnImprimirFiniquitoIMS.attr("disabled", false);
                            spanCapturarIMS.text("Actualizar");
                            txtVerNombreIMS.val(rowData.archivoIMS);
                            btnVerFiniquitoIMS.attr("disabled", false);
                        }
                        // btnImprimirFiniquito.attr("href",`/Administrativo/BajasPersonal/DescargarArchivoFiniquito?baja_id=`+rowData.id);
                    });
                    tblRH_Baja_Registro.on('click', '.verFiniquitoImss', function () {
                        let rowData = dtBajas.row($(this).closest('tr')).data();
                        mdlFiniquitoImss.modal("show");
                        btnGuardarFiniquito.attr("data-id", rowData.id);
                        btnImprimirFiniquito.attr("data-id", rowData.id);

                        txtVerMontoIMS.val(rowData.montoIMS);

                        if (rowData.rutaIMS == null) {
                            btnImprimirFiniquitoIMS.attr("disabled", true);
                            spanCapturarIMS.text("Capturar");
                            txtVerNombreIMS.val("");
                            btnVerFiniquitoIMS.attr("disabled", true);
                            txtVencidoIMS.html(`${rowData.esVencidoIMS ? '<p style="color: red;">Tiempo excedido en carga de archivo.</p>' : ''}`);

                        } else {
                            btnImprimirFiniquitoIMS.attr("disabled", false);
                            spanCapturarIMS.text("Actualizar");
                            txtVerNombreIMS.val(rowData.archivoIMS);
                            btnVerFiniquitoIMS.attr("disabled", false);
                            txtVencidoIMS.html(`Fecha de captura: ${moment(rowData.fechaCapturaIMS).format("DD/MM/YYYY")} ${rowData.esVencidoIMS ? '<p style="color: red;">Tiempo excedido en carga de archivo.</p>' : ''}`);

                        }
                        // btnImprimirFiniquito.attr("href",`/Administrativo/BajasPersonal/DescargarArchivoFiniquito?baja_id=`+rowData.id);
                    });
                    tblRH_Baja_Registro.on('click', '.verBajaCaptura', function () {
                        let rowData = dtBajas.row($(this).closest('tr')).data();
                        cboCandidatosAprobados.attr("disabled", true);
                        fncGetDatosActualizarEmpleado(rowData.numeroEmpleado, false);
                    });
                    tblRH_Baja_Registro.on('click', '.cancelarBaja', function () {
                        let rowData = dtBajas.row($(this).closest('tr')).data();

                        Swal.fire({
                            position: "center",
                            icon: "warning",
                            title: "Cancelar Baja",
                            input: 'textarea',
                            width: '50%',
                            showCancelButton: true,
                            html: "<h3>¿Desea cancelar la baja seleccionada?<br>Indicar el motivo:</h3>",
                            confirmButtonText: "Aceptar",
                            confirmButtonColor: "#5cb85c",
                            cancelButtonText: "Cancelar",
                            cancelButtonColor: "#5c636a",
                            showCloseButton: true
                        }).then((result) => {
                            if (result.isConfirmed) {
                                fncCancelarAutorizacionBaja(rowData.id, $('.swal2-textarea').val());
                            }
                        });
                    });
                    tblRH_Baja_Registro.on('click', '.verComentarioCancelado', function () {
                        let rowData = dtBajas.row($(this).closest('tr')).data();
                        txtMdlComentario.text(rowData.comentariosCancelacion);
                        mdlComentario.modal("show");
                    });
                    tblRH_Baja_Registro.on("click", ".imprimirDoc", function () {
                        let rowData = dtBajas.row($(this).closest('tr')).data();
                        fncImprimirDocumentos(rowData.id);
                    });
                    tblRH_Baja_Registro.on("click", ".liberarNomina", function () {
                        let rowData = dtBajas.row($(this).closest('tr')).data();

                        Swal.fire({
                            position: "center",
                            icon: "warning",
                            title: "Liberar Baja",
                            input: 'textarea',
                            width: '50%',
                            showCancelButton: true,
                            html: "<h3>¿Desea liberar la baja seleccionada por el area de Nominas?<br>Comentario:</h3>",
                            confirmButtonText: "Aceptar",
                            confirmButtonColor: "#5cb85c",
                            cancelButtonText: "Cancelar",
                            cancelButtonColor: "#5c636a",
                            showCloseButton: true
                        }).then((result) => {
                            if (result.isConfirmed) {
                                fncLiberarNomina(rowData.id, $('.swal2-textarea').val());
                            }
                        });

                    });
                },
                select: {
                    style: 'multi',
                    selector: 'td:first-child'
                }
            });
        }

        function fnDetalleAut(datos) {
            axios.post("/Administrativo/BajasPersonal/GetDetalleAut", { id: datos.id, tipo: datos.tipo }).then(response => {
                let { success, fecha, usuario, comentario } = response.data;
                if (success) {
                    txt_aut_fecha.val(fecha);
                    txt_aut_Usuario.val(usuario);
                    txt_aut_comentario.val(comentario);
                    mdlDetalleaut.modal("show");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetBajasPersonal() {
            let listaCC = getValoresMultiples('#selectCC');
            let contratable = +cboFiltroEstatus.val();
            let prioridad = +cboFiltroPrioridad.val();
            let fechaInicio = fechaFiltroInicio.val();
            let fechaFin = fechaFiltroFin.val();
            let clave_empleado = +inputClaveEmpleado.val();
            let nombre_empleado = inputNombreEmpleado.val();

            if (fechaInicio == "" && fechaFin != "") {
                Alert2Warning("Ingrese una fecha de inicio");
                return "";
            }

            if (fechaFin == "" && fechaInicio != "") {
                Alert2Warning("Ingrese una fecha de fin");
                return "";
            }

            if (fechaFiltroContaInicio.val() == "" && fechaFiltroContaFin.val() != "") {
                Alert2Warning("Ingrese una fecha de inicio de liberacion");
                return "";
            }

            if (fechaFiltroContaFin.val() == "" && fechaFiltroContaInicio.val() != "") {
                Alert2Warning("Ingrese una fecha de fin de liberacion");
                return "";
            }

            if (selectCC.val().length == 0 && (!inputClaveEmpleado.val() && !inputNombreEmpleado.val())) {
                Alert2Warning("Favor de seleccionar un CC o una clave de empleado o nombre de empleado");
                return "";
            }

            axios.post("/Administrativo/BajasPersonal/GetBajasPersonal", {
                listaCC,
                contratable,
                prioridad,
                fechaInicio,
                fechaFin,
                clave_empleado,
                nombre_empleado,
                fechaContaInicio: fechaFiltroContaInicio.val(),
                fechaContaFin: fechaFiltroContaFin.val(),
                anticipada: cboFiltroAnticipada.val(),
            }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtBajas.clear();
                    dtBajas.rows.add(response.data.lstBajas);
                    dtBajas.draw();
                    //#endregion

                    empresa = response.data.empresa;
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetDatosActualizarBajaPersonal(idBaja) {
            let obj = new Object();
            obj = {
                idBaja: idBaja
            }
            axios.post("/Administrativo/BajasPersonal/GetDatosActualizarBajaPersonal", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {

                    txt_registro_nombre_autoriza.fillCombo("GetAutorizantes", {
                        cc: response.data.objBaja.cc,
                        clave_empleado: response.data.objBaja.clave_autoriza,
                        nombre_empleado: response.data.objBaja.nombre_autoriza
                    }, false, null);

                    //#region SE LLENAN LOS CAMPOS EN BASE A LA INFORMACIÓN ENCONTRADA
                    txt_registro_numeroEmpleado.val(response.data.objBaja.numeroEmpleado);
                    // txt_registro_numeroEmpleado.trigger("change");
                    txt_registro_nombre.val(response.data.objBaja.nombre);
                    txt_registro_cc.val(response.data.objBaja.cc);
                    txt_registro_fechaIngreso.val(moment(response.data.objBaja.fechaIngreso).format("DD/MM/YYYY"));
                    txt_registro_puesto.val(response.data.objBaja.nombrePuesto);
                    txt_registro_habilidadesEquipo.val(response.data.objBaja.habilidadesConEquipo);
                    txt_registro_telPersonal.val(response.data.objBaja.telPersonal);
                    txt_registro_curp.val(response.data.objBaja.curp);
                    txt_registro_rfc.val(response.data.objBaja.rfc);
                    txt_registro_nss.val(response.data.objBaja.nss);
                    txt_registro_observaciones.val(response.data.objBaja.comentarios);
                    txt_registro_comentariosRecontratable.val(response.data.objBaja.comentariosRecontratacion);
                    if (response.data.objBaja.tieneWha) {
                        cbo_registro_tieneWha.val("1");
                        cbo_registro_tieneWha.trigger("change");
                    } else if (!response.data.objBaja.tieneWha) {
                        cbo_registro_tieneWha.val("0")
                        cbo_registro_tieneWha.trigger("change");
                    }

                    txt_registro_telCasa.val(response.data.objBaja.telCasa);
                    txt_registro_contactoFamilia.val(response.data.objBaja.contactoFamilia);

                    if (_empresaActual == 6) {
                        txt_registro_dni.val(response.data.objBaja.dni);
                        cbo_registro_idDepartamento.val(response.data.objBaja.idDepartamento);
                        cbo_registro_idDepartamento.trigger("change");
                    }

                    if (_empresaActual == 3) {
                        txt_registro_cedula.val(response.data.objBaja.cedula_ciudadania);
                    }

                    cbo_registro_idEstado.val(response.data.objBaja.idEstado);
                    cbo_registro_idEstado.trigger("change");
                    if (cbo_registro_idEstado.val() > 0) {
                        cbo_registro_idCiudad.val(response.data.objBaja.idCiudad);
                        cbo_registro_idCiudad.trigger("change");
                        cbo_registro_idMunicipio.val(response.data.objBaja.idMunicipio);
                        cbo_registro_idMunicipio.trigger("change");
                    }

                    txt_registro_direccion.val(response.data.objBaja.direccion);
                    txt_registro_facebook.val(response.data.objBaja.facebook);
                    txt_registro_instagram.val(response.data.objBaja.instagram);
                    txt_registro_correo.val(response.data.objBaja.correo);
                    cbo_registro_motivoBajaSistema.val(response.data.objBaja.motivoBajaDeSistema);
                    cbo_registro_motivoBajaSistema.trigger("change");
                    txt_registro_motivoSeparacionEmpresa.val(response.data.objBaja.motivoSeparacionDeEmpresa);

                    if (response.data.objBaja.regresariaALaEmpresa) {
                        cbo_registro_regresariaALaEmpresa.val("1");
                        cbo_registro_regresariaALaEmpresa.trigger("change");
                    } else if (!response.data.objBaja.regresariaALaEmpresa) {
                        cbo_registro_regresariaALaEmpresa.val("0");
                        cbo_registro_regresariaALaEmpresa.trigger("change");
                    }

                    txt_registro_porqueRegresariaALaEmpresa.val(response.data.objBaja.porqueRegresariaALaEmpresa);

                    txt_registro_dispuestoCambioDeProyecto.val(response.data.objBaja.dispuestoCambioDeProyecto);
                    txt_registro_experienciaCP.val(response.data.objBaja.experienciaEnCP);

                    if (response.data.objBaja.esContratable) {
                        cbo_registro_contratable.val("1");
                        cbo_registro_contratable.trigger("change");
                    } else if (!response.data.objBaja.esContratable) {
                        cbo_registro_contratable.val("0");
                        cbo_registro_contratable.trigger("change");
                    }

                    //txt_registro_nombre_autoriza
                    // txt_registro_numeroEmpleado_baja.val(response.data.objBaja.clave_autoriza);
                    txt_registro_nombre_baja.val(response.data.objBaja.nombre_autoriza);
                    txt_registro_nombre_autoriza.val(response.data.objBaja.clave_autoriza);
                    txt_registro_nombre_autoriza.trigger("change");

                    // txt_registro_nombre_baja.trigger("change");

                    cbo_registro_prioridad.val(response.data.objBaja.prioridad);
                    cbo_registro_prioridad.trigger("change");
                    btnCEBajaPersonal.attr("data-id", response.data.objBaja.id);
                    lblTituloBtnCEBajaPersonal.html("<i class='fa fa-save'></i>&nbsp;Actualizar");

                    // chk_registro_esAnticipada.prop("disabled", false);
                    if (response.data.objBaja.esAnticipada) {
                        // chk_registro_esAnticipada.bootstrapToggle('on');

                    } else {
                        // chk_registro_esAnticipada.bootstrapToggle('off');

                    }

                    txt_registro_fechaBaja.val(moment(response.data.objBaja.fechaBaja).format("YYYY-MM-DD"));

                    // chk_registro_esAnticipada.prop("disabled", true);    
                    //#endregion

                    //#region SE CARGA ENTREVISTA EN BASE A LOS DATOS ENCONTRADOS
                    if (response.data.objEntrevista != null) {
                        txt_entrevista_gerenteClave.val(response.data.objEntrevista.gerente_clave)
                        if (response.data.objEntrevista.gerente_clave > 0) {
                            txt_entrevista_gerenteNombre.val(response.data.objEntrevista.nombreGerente);
                            txt_entrevista_gerenteClave.trigger("change");
                        }
                        txt_entrevista_fechaNacimiento.val(moment(response.data.objEntrevista.fecha_nacimiento).format("YYYY-MM-DD"));
                        cbo_entrevista_estadoCivilID.val(response.data.objEntrevista.estado_civil_clave);
                        cbo_entrevista_estadoCivilID.trigger("change");
                        cbo_entrevista_escolaridadID.val(response.data.objEntrevista.escolaridad_clave);
                        cbo_entrevista_escolaridadID.trigger("change");
                        cbo_entrevista_p1.val(response.data.objEntrevista.p1_clave);
                        cbo_entrevista_p1.trigger("change");
                        cbo_entrevista_p2.val(response.data.objEntrevista.p2_clave);
                        cbo_entrevista_p2.trigger("change");
                        cbo_entrevista_p3_1.val(response.data.objEntrevista.p3_1_clave);
                        cbo_entrevista_p3_1.trigger("change");
                        cbo_entrevista_p3_2.val(response.data.objEntrevista.p3_2_clave);
                        cbo_entrevista_p3_2.trigger("change");
                        cbo_entrevista_p3_3.val(response.data.objEntrevista.p3_3_clave);
                        cbo_entrevista_p3_3.trigger("change");
                        cbo_entrevista_p3_4.val(response.data.objEntrevista.p3_4_clave);
                        cbo_entrevista_p3_4.trigger("change");
                        cbo_entrevista_p3_5.val(response.data.objEntrevista.p3_5_clave);
                        cbo_entrevista_p3_5.trigger("change");
                        cbo_entrevista_p3_6.val(response.data.objEntrevista.p3_6_clave);
                        cbo_entrevista_p3_6.trigger("change");
                        cbo_entrevista_p3_7.val(response.data.objEntrevista.p3_7_clave);
                        cbo_entrevista_p3_7.trigger("change");
                        cbo_entrevista_p3_8.val(response.data.objEntrevista.p3_8_clave);
                        cbo_entrevista_p3_8.trigger("change");
                        cbo_entrevista_p3_9.val(response.data.objEntrevista.p3_9_clave);
                        cbo_entrevista_p3_9.trigger("change");
                        cbo_entrevista_p3_10.val(response.data.objEntrevista.p3_10_clave);
                        cbo_entrevista_p3_10.trigger("change");
                        cbo_entrevista_p4.val(response.data.objEntrevista.p4_clave);
                        cbo_entrevista_p4.trigger("change");
                        cbo_entrevista_p5.val(response.data.objEntrevista.p5_clave);
                        cbo_entrevista_p5.trigger("change");
                        txt_entrevista_p6.val(response.data.objEntrevista.p6_concepto);
                        txt_entrevista_p7.val(response.data.objEntrevista.p7_concepto);
                        cbo_entrevista_p8.val(response.data.objEntrevista.p8_clave);
                        cbo_entrevista_p8.trigger("change");
                        txt_entrevista_p8_porque.val(response.data.objEntrevista.p8_porque);
                        cbo_entrevista_p9.val(response.data.objEntrevista.p9_clave);
                        cbo_entrevista_p9.trigger("change");
                        txt_entrevista_p9_porque.val(response.data.objEntrevista.p9_porque);
                        cbo_entrevista_p10.val(response.data.objEntrevista.p10_clave);
                        cbo_entrevista_p10.trigger("change");
                        txt_entrevista_p10_porque.val(response.data.objEntrevista.p10_porque);
                        cbo_entrevista_p11.val(response.data.objEntrevista.p11_1_clave);
                        cbo_entrevista_p11.trigger("change");
                        cbo_entrevista_p11_1.val(response.data.objEntrevista.p11_2_clave);
                        cbo_entrevista_p11_1.trigger("change");
                        cbo_entrevista_p12.val(response.data.objEntrevista.p12_clave);
                        cbo_entrevista_p12.trigger("change");
                        txt_entrevista_p12_porque.val(response.data.objEntrevista.p12_porque);
                        cbo_entrevista_p13.val(response.data.objEntrevista.p13_clave);
                        cbo_entrevista_p13.trigger("change");
                        cbo_entrevista_p14.val(response.data.objEntrevista.p14_clave);
                        cbo_entrevista_p14.trigger("change");
                        txt_entrevista_fechaAproximada.val(moment(response.data.objEntrevista.p14_fecha).format("YYYY-MM-DD"));
                        txt_entrevista_comoFue.val(response.data.objEntrevista.p14_porque);
                    } else {
                        fieldEntrevista.find('input, select, textarea').each(function (idx, element) {
                            // $(element).text('');
                            $(element).val('');
                            $(element).trigger("change");
                        });
                    }
                    //#endregion
                } else {
                    // fncLimpiarFormulario();
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCEBajaPersonal() {
            let obj = fncGetObjCEBajaPersonal();
            if (obj != "") {
                axios.post("/Administrativo/BajasPersonal/CrearEditarBajaPersonal", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        inputClaveEmpleado.val(txt_registro_numeroEmpleado.val());
                        fncGetBajasPersonal();
                        mdlBaja.modal("hide");
                        fncLimpiarFormulario();
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(response.data.message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGetObjCEBajaPersonal() {
            fncBorderDefault();
            let strMensajeError = "";
            if (txt_registro_numeroEmpleado.val() == "") { txt_registro_numeroEmpleado.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txt_registro_nombre.val() == "") { txt_registro_nombre.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txt_registro_fechaBaja.val() == "") { txt_registro_fechaBaja.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_registro_motivoBajaSistema.val() == "--Seleccione--") { $("#select2-cbo_registro_motivoBajaSistema-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txt_registro_telPersonal.val() == "") { txt_registro_telPersonal.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            // if (txt_registro_telCasa.val() == "") { txt_registro_telCasa.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_registro_idEstado.val() == "") { $("#select2-cbo_registro_idEstado-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_registro_idCiudad.val() == "" || cbo_registro_idCiudad.val() == null) { $("#select2-cbo_registro_idCiudad-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if ((_empresaActual == 1 || _empresaActual == 2) && (cbo_registro_idMunicipio.val() == "" || cbo_registro_idMunicipio.val() == null)) { $("#select2-cbo_registro_idMunicipio-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txt_registro_direccion.val() == "") { txt_registro_direccion.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txt_registro_fechaBaja.val() == "") { txt_registro_fechaBaja.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cbo_registro_contratable.val() == "") { $("#select2-cbo_registro_contratable-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            // if (txt_registro_nombre_autoriza.val() == "") { txt_registro_nombre_autoriza.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }

            if (_empresaActual == 6 && cbo_registro_idDepartamento.val() == "") { $("#select2-cbo_registro_idDepartamento-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }

            if (txt_registro_fechaBaja.val() != "" && (txt_registro_fechaBaja.val() > txt_registro_fechaBaja.attr("max") || txt_registro_fechaBaja.val() < txt_registro_fechaBaja.attr("min"))) {
                txt_registro_fechaBaja.val(moment().format("YYYY-MM-DD"));
                strMensajeError = "Favor de ingresar una fecha entre el rango permitido";
            }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    //#region BAJA DE PERSONAL
                    id: btnCEBajaPersonal.attr("data-id"),
                    numeroEmpleado: txt_registro_numeroEmpleado.val(),
                    nombre: txt_registro_nombre.val(),
                    cc: txt_registro_cc.val(),
                    habilidadesConEquipo: txt_registro_habilidadesEquipo.val(),
                    telPersonal: txt_registro_telPersonal.val(),
                    strTieneWha: cbo_registro_tieneWha.val(),
                    telCasa: txt_registro_telCasa.val(),
                    contactoFamilia: txt_registro_contactoFamilia.val(),
                    idDepartamento: cbo_registro_idDepartamento.val(),
                    idEstado: cbo_registro_idEstado.val(),
                    idCiudad: cbo_registro_idCiudad.val(),
                    idMunicipio: cbo_registro_idMunicipio.val(),
                    direccion: txt_registro_direccion.val(),
                    facebook: txt_registro_facebook.val(),
                    instagram: txt_registro_instagram.val(),
                    correo: txt_registro_correo.val(),
                    fechaBaja: txt_registro_fechaBaja.val(),
                    motivoBajaDeSistema: cbo_registro_motivoBajaSistema.val(),
                    motivoSeparacionDeEmpresa: txt_registro_motivoSeparacionEmpresa.val(),
                    strRegresariaALaEmpresa: cbo_registro_regresariaALaEmpresa.val(),
                    porqueRegresariaALaEmpresa: txt_registro_porqueRegresariaALaEmpresa.val(),
                    dispuestoCambioDeProyecto: txt_registro_dispuestoCambioDeProyecto.val(),
                    experienciaEnCP: txt_registro_experienciaCP.val(),
                    strContratable: cbo_registro_contratable.val(),
                    prioridad: cbo_registro_prioridad.val(),
                    curp: txt_registro_curp.val(),
                    rfc: txt_registro_rfc.val(),
                    nss: txt_registro_nss.val(),
                    dni: txt_registro_dni.val(),
                    cedula_ciudadania: txt_registro_cedula.val(),
                    comentarios: txt_registro_observaciones.val(),
                    comentariosRecontratacion: txt_registro_comentariosRecontratable.val(),
                    // clave_autoriza: txt_registro_nombre_autoriza.val(),
                    // nombre_autoriza: $('select[id="txt_registro_nombre_autoriza"] option:selected').text(),
                    clave_autoriza: 0,
                    nombre_autoriza: "",
                    esAnticipada: false,
                    //#endregion
                    //#region ENTREVISTA
                    fecha_ingreso: txt_registro_fechaIngreso.val(),
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
                    p14_porque: txt_entrevista_comoFue.val()
                    //#endregion
                }
                return obj;
            }
        }

        function fncEliminarBajaPersonal(idBaja) {
            let obj = new Object();
            obj = {
                idBaja: idBaja
            }
            axios.post("/Administrativo/BajasPersonal/EliminarBajaPersonal", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetBajasPersonal();
                    Alert2Exito(response.data.message);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetDatosPersonal(esGerente) {
            let obj = new Object();
            obj = {
                claveEmpleado: !esGerente ? txt_registro_numeroEmpleado.val() : txt_entrevista_gerenteClave.val(),
                nombre: txt_registro_nombre.val()
            }
            axios.post("/Administrativo/BajasPersonal/GetDatosPersona", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (!esGerente) {
                        txt_registro_nombre.val(response.data.objDatosPersona.nombreCompleto);
                        txt_registro_cc.val(response.data.objDatosPersona.cc);
                        txt_registro_puesto.val(response.data.objDatosPersona.nombrePuesto);
                        txt_registro_fechaIngreso.val(moment(response.data.objDatosPersona.fechaIngreso).format("DD/MM/YYYY"));
                        txt_registro_curp.val(response.data.objDatosPersona.curp);
                        txt_registro_rfc.val(response.data.objDatosPersona.rfc);
                        txt_registro_nss.val(response.data.objDatosPersona.nss);
                        lblCantActos.html(response.data.objDatosPersona.cantidadActos);
                        lblCantCapacitaciones.html(response.data.objDatosPersona.cantidadCursos);

                        if (response.data.objDatosPersona.altaContratable != null) {
                            cbo_registro_contratable.val(response.data.objDatosPersona.altaContratable == "S" ? 1 : 0);
                            cbo_registro_contratable.trigger("change");
                        }

                        if (_empresaActual == 6) {
                            txt_registro_dni.val(response.data.objDatosPersona.dni);
                            cbo_registro_idDepartamento.val(response.data.objDatosPersona.idDepartamento);
                            cbo_registro_idDepartamento.trigger("change");
                        }

                        if (_empresaActual == 3) {
                            txt_registro_cedula.val(response.data.objDatosPersona.cedula_ciudadania);
                        }

                        cbo_registro_idEstado.val(response.data.objDatosPersona.idEstado);
                        cbo_registro_idEstado.change();

                        //Por mientras se utiliza un delay de 5 segundos para darle tiempo a la petición de terminar de llenar los combos. Cambiar esto después.
                        setTimeout(() => {
                            cbo_registro_idCiudad.val(response.data.objDatosPersona.idCiudad);
                            cbo_registro_idCiudad.change();
                            cbo_registro_idMunicipio.val(response.data.objDatosPersona.idCiudad);
                            cbo_registro_idMunicipio.change();
                        }, 5000);

                        txt_registro_nombre_autoriza.fillCombo("GetAutorizantes", { cc: response.data.objDatosPersona.numCC, clave_empleado: null, nombre_empleado: null }, false, null);
                    } else {
                        txt_entrevista_gerenteNombre.val(response.data.objDatosPersona.nombreCompleto);
                    }

                    txt_registro_nombre_baja.fillCombo("GetFacultamientosAutorizante", { cc: response.data.objDatosPersona.numCC }, false, null);

                } else {
                    Alert2Warning(message);
                }
            }).catch(error => Alert2Error("Clave de usuario no encontrada"));
        }

        function fncMostrarEntrevista() {
            // var _this = $(this);
            // if (_this.val() == 1) {
            //     fieldEntrevista.show();
            // }
            // else {
            //     fieldEntrevista.hide();
            // }
        }

        function initTblActos() {
            dtActos = tblActos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    {
                        data: 'fechaCreacion', title: 'Fecha creacion',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    { data: 'descripcion', title: 'Descripción' },
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function initTblCapacitaciones() {
            dtCapacitaciones = tblCapacitaciones.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'curso', title: 'Curso' },
                    { data: 'fecha', title: 'Fecha del curso' },
                    { data: 'calificacion', title: 'Calificación' },
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

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
                    { data: 'cc_origen', title: 'CC Origen' },
                    { data: 'cc_nuevo', title: 'CC Nuevo' },
                    { data: 'puesto_origen', title: 'Puesto Origen' },
                    { data: 'puesto_nuevo', title: 'Puesto Nuevo' },
                    { data: 'jefe_origen', title: 'Jefe Origen' },
                    { data: 'jefe_nuevo', title: 'Jefe Nuevo' },
                    { data: 'patron_origen', title: 'Patron Origen' },
                    { data: 'patron_nuevo', title: 'Patron Nuevo' },
                    { data: 'fechaInicio', title: 'Fecha Cambio' },

                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetActosCapacitaciones(claveEmpleado, actoCapacitaciones) {
            let obj = new Object();
            obj = {
                claveEmpleado: claveEmpleado
            }
            axios.post("/Administrativo/BajasPersonal/GetEmpleadoCursosActos", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    if (actoCapacitaciones == "ACTO") {
                        dtActos.clear();
                        dtActos.rows.add(response.data.data.actos);
                        dtActos.draw();
                        mdlActos.modal("show");
                    } else {
                        dtCapacitaciones.clear();
                        dtCapacitaciones.rows.add(response.data.data.capacitaciones);
                        dtCapacitaciones.draw();
                        mdlCapacitaciones.modal("show");
                    }
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetHistorialCC(claveEmpleado) {
            axios.post("/Administrativo/BajasPersonal/GetHistorialCC", { cvEmpleado: claveEmpleado }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    console.log(items)
                    dtHistorial.clear();
                    dtHistorial.rows.add(items);
                    dtHistorial.draw();
                    mdlHistorial.modal("show");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGuardarArchivoFiniquito() {
            const data = new FormData();


            data.append('idBaja', btnGuardarFiniquito.attr("data-id"));
            data.append('archivo', inCapturarFiniquito[0].files[0]);
            data.append('tipoFiniquito', cboCEFiniquitoTipo.val());
            data.append('monto', txtCEFiniquitoMonto.val() != "" ? txtCEFiniquitoMonto.val() : 0);

            if (inCapturarFiniquito[0].files.length > 0) {
                axios.post("/Administrativo/BajasPersonal/GuardarArchivoFiniquito", data, { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito("Se guardo el finiquito con exito");
                        fncGetBajasPersonal();
                        // mdlFiniquito.hide();
                        // mdlFiniquito.show();
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }
        //#endregion

        //#region FUNCIONES GENERALES
        function fncLimpiarFormulario() {
            $('input[type="text"]').val("");

            txt_registro_observaciones.val("");

            //txt_registro_nombre_autoriza
            txt_registro_numeroEmpleado_baja.val("");
            txt_registro_nombre_baja.val("");
            // txt_registro_nombre_baja.trigger("change");

            btnCEBajaPersonal.attr("data-id", 0);
            txt_registro_habilidadesEquipo.val("");

            cbo_registro_tieneWha[0].selectedIndex = 0;
            cbo_registro_tieneWha.trigger("change");

            cbo_registro_idDepartamento[0].selectedIndex = 0;
            cbo_registro_idDepartamento.trigger("change");

            cbo_registro_idEstado[0].selectedIndex = 0;
            cbo_registro_idEstado.trigger("change");

            cbo_registro_idCiudad[0].selectedIndex = 0;
            cbo_registro_idCiudad.trigger("change");

            cbo_registro_idMunicipio[0].selectedIndex = 0;
            cbo_registro_idMunicipio.trigger("change");

            cbo_registro_motivoBajaSistema[0].selectedIndex = 0;
            cbo_registro_motivoBajaSistema.trigger("change");

            txt_registro_motivoSeparacionEmpresa.val("");

            cbo_registro_regresariaALaEmpresa[0].selectedIndex = 0;
            cbo_registro_regresariaALaEmpresa.trigger("change");

            txt_registro_porqueRegresariaALaEmpresa.val("");
            txt_registro_dispuestoCambioDeProyecto.val("");
            txt_registro_experienciaCP.val("");

            cbo_registro_contratable[0].selectedIndex = 0;
            cbo_registro_contratable.trigger("change");

            cbo_registro_prioridad[0].selectedIndex = 0;
            cbo_registro_prioridad.trigger("change");

            lblCantActos.html("0");
            lblCantCapacitaciones.html("0");

            fieldEntrevista.find('input, select, textarea').each(function (idx, element) {
                // $(element).text('');
                $(element).val('');
                $(element).trigger("change");
            });
        }

        function fncBorderDefault() {
            txt_registro_numeroEmpleado.css("border", "1px solid #CCC");
            txt_registro_nombre.css("border", "1px solid #CCC");
            txt_registro_fechaBaja.css("border", "1px solid #CCC");
            $("#select2-cbo_registro_motivoBajaSistema-container").css("border", "1px solid #CCC");
            // $("#select2-cboTecnicoResponsable-container").css("border", "1px solid #CCC");
        }

        function EnviarCorreo(correo, liga, idRegistro) {
            axios.post("/Administrativo/BajasPersonal/EnviarCorreo", { email: correo, link: liga, id: idRegistro }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Correo Enviado");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncImprimirDocumentos(idBaja) {
            // console.log('imprimir')
            var path = `/Reportes/Vista.aspx?idReporte=256&idBaja=${idBaja}`;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }
        //#endregion

        //#region ALTA/BAJA EMPLEADO (VER BAJA DEL EMPLEADO EN EL MODULO DE RECLUTAMIENTOS)

        //#region INIT TBLS CAPTURAS
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
                            let fechaNacimiento = moment(data).format("DD/MM/YYYY");
                            if (fechaNacimiento != '01/01/2000') {
                                return fechaNacimiento;
                            } else {
                                return "";
                            }
                        }
                    },
                    { data: 'estado_civil', title: 'ESTADO CIVIL' },
                    { data: 'genero', title: 'GENERO' },
                    { data: 'grado_de_estudios', title: 'ESTUDIOS' },
                    {
                        render: function (data, type, row) {
                            let btnActualizar = `<button class="btn btn-primary actualizarFamiliar btn-xs"><i class="far fa-search"></i></button>&nbsp;`;
                            return btnActualizar;
                        }
                    },
                    { data: 'parentesco', visible: false },
                    { data: 'clave_empleado', visible: false },
                    { data: 'nombre', visible: false },
                    { data: 'apellido_paterno', visible: false },
                    { data: 'apellido_materno', visible: false },
                    { data: 'estudia', visible: false },
                    { data: 'vive', visible: false },
                    { data: 'beneficiario', visible: false },
                    { data: 'trabaja', visible: false },
                    { data: 'comentarios', visible: false },
                    { data: 'id', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_Familiares.on('click', '.actualizarFamiliar', function () {
                        let rowData = dtFamiliares.row($(this).closest('tr')).data();

                        cboCEFamiliarParentesco.val(rowData.parentesco);
                        cboCEFamiliarParentesco.trigger("change");

                        cboCEFamiliarGenero.val(rowData.genero);
                        cboCEFamiliarGenero.trigger("change");

                        txtCEFamiliarFechaNacimiento.val(moment(rowData.fecha_de_nacimiento).format("YYYY-MM-DD"));

                        chkCEFamiliarVive.prop("checked", rowData.vive);
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

                        mdlCrearEditarFamiliar.modal("show");
                        lblTitleCEFamiliares.text("Familiares");

                    });



                    tblRH_REC_Familiares.on('click', '.eliminarFamiliar', function () {
                        let rowData = dtFamiliares.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarFamiliar(rowData.id, rowData.clave_empleado));
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

        function initTblNominas() {
            dtTabuladores = tblRH_REC_Tabuladores.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: false,
                bFilter: false,
                info: false,
                "order": [[0, "desc"]],
                columns: [
                    { data: 'fecha_cambio', title: 'FECHA REAL', visible: false },
                    { data: 'fechaRealNomina', title: 'FECHA REAL' },
                    { data: 'salario_base', title: 'SALARIO BASE' },
                    { data: 'complemento', title: 'COMPLEMENTO' },
                    { data: 'bono_de_zona', title: 'BONO ZONA' },
                    { data: 'suma', title: 'TOTAL' }
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_Tabuladores.on('click', '.classBtn', function () {
                        let rowData = dtTabuladores.row($(this).closest('tr')).data();
                    });

                    tblRH_REC_Tabuladores.on('click', '.classBtn', function () {
                        let rowData = dtTabuladores.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

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
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    { data: "calzado", title: 'CALZADO' },
                    { data: "camisa", title: 'CAMISA' },
                    { data: "pantalon", title: 'PANTALÓN' },
                    { data: "overol", title: 'OVEROL' },
                    { data: "uniforme_dama", title: 'UNIFORME DAMA' },
                    {
                        render: function (data, type, row) {
                            let btnActualizar = `<button class="btn btn-warning actualizarUniforme btn-xs"><i class="far fa-edit"></i></button>&nbsp;`;
                            return btnActualizar;
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
                    { data: 'entrego_uniforme_dama', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_Uniformes.on('click', '.actualizarUniforme', function () {
                        // fncLimpiarMdlCEUniforme();
                        let row = dtUniformes.row($(this).closest('tr')).data();
                        txtCEUniformeFechaEntrega.val(moment(row.fechaEntrega).format("YYYY-MM-DD"));
                        txtCEUniformeNoCalzado.val(row.calzado);
                        txtCEUniformeCamisa.val(row.camisa);
                        txtCEUniformePantalon.val(row.pantalon);
                        txtCEUniformeOverol.val(row.overol);
                        txtCEUniformeUniformeDama.val(row.uniforme_dama);
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

                        chkCEUniformeUniformeDama.prop("checked", row.entrego_uniforme_dama == "S" ? true : false);
                        chkCEUniformeUniformeDama.trigger("change");

                        lblTitleCEUniforme.html("Uniformes");
                        mdlCrearEditarUniforme.modal("show");
                    });

                    tblRH_REC_Uniformes.on('click', '.eliminarUniforme', function () {
                        let rowData = dtUniformes.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarUniforme(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

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
                    { data: 'descripcion', title: 'Observación' },
                    {
                        render: function (data, type, row) {
                            return `<button class="btn btn-danger eliminarArchivo btn-xs"><i class="far fa-trash-alt"></i></button>`;
                        }
                    },
                    { data: 'id', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_ArchivosExamenMedico.on('click', '.eliminarArchivo', function () {
                        let rowData = dtExamenMedico.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarExamenMedico(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

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

        function initTblContratos() {
            dtContratos = tblContratos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'id_contrato_empleado', title: 'CONTRATO' },
                    { data: 'fechaString', title: 'FECHA MODIFICACIÓN' },
                    { data: 'fecha_aplicacionString', title: 'FECHA APLICACIÓN' }
                ],
                initComplete: function (settings, json) {
                    // tblContratos.on('click','.classBtn', function () {
                    //     let rowData = dtContratos.row($(this).closest('tr')).data();
                    // });
                    // tblContratos.on('click','.classBtn', function () {
                    //     let rowData = dtContratos.row($(this).closest('tr')).data();
                    //     //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    // });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

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
                    {
                        title: 'Seleccionar', render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-warning botonSeleccionarRequisicion" disabled><i class="fa fa-arrow-right"></i></button>`;
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
                    { width: '12%', targets: [0, 1, 4] },
                    { className: 'dt-center', targets: '_all' }
                ]
            });
        }
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
                            if (row.estado) {
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

            axios.post("/Administrativo/BajasPersonal/GetDocs", { clave_empleado, id_candidato }).then(response => {
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

        function fncGetDatosActualizarEmpleado(claveEmpleado, esReingresoEmpleado) {
            let obj = new Object();
            obj = {
                claveEmpleado: claveEmpleado,
                esReingresoEmpleado: esReingresoEmpleado
            }
            axios.post("/Administrativo/BajasPersonal/GetDatosActualizarEmpleado", obj).then(response => {
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
                    } else {
                        lblCEDatosEmpleadoEstatus.html("PENDIENTE");
                        lblCEDatosEmpleadoEstatus.css("color", "blue");
                    }

                    lblCEDatosEmpleadoClaveEmpleado.html(response.data.lstDatos[0].clave_empleado);
                    lblCEDatosEmpleadoClaveEmpleado.attr("data-clvEmpleado", response.data.lstDatos[0].clave_empleado);
                    txtCEDatosEmpleadoNombre.val(response.data.lstDatos[0].nombre);
                    txtCEDatosEmpleadoApePaterno.val(response.data.lstDatos[0].ape_paterno);
                    txtCEDatosEmpleadoApeMaterno.val(response.data.lstDatos[0].ape_materno);
                    txtCEDatosEmpleadoFechaNacimiento.val(moment(response.data.lstDatos[0].fecha_nac).format("YYYY-MM-DD"));
                    cboCEDatosEmpleadoPaisNac.val(response.data.lstDatos[0].clave_pais_nac);
                    cboCEDatosEmpleadoPaisNac.trigger("change");
                    cboCEDatosEmpleadoEstadoNac.val(response.data.lstDatos[0].clave_estado_nac);
                    cboCEDatosEmpleadoEstadoNac.trigger("change");
                    cboCEDatosEmpleadoLugarNac.val(response.data.lstDatos[0].clave_ciudad_nac);
                    cboCEDatosEmpleadoLugarNac.trigger("change");
                    txtCEDatosEmpleadoLocalidadNac.val(response.data.lstDatos[0].localidad_nacimiento);
                    txtCEDatosEmpleadoFechaIngreso.val(moment(response.data.lstDatos[0].fecha_alta).format("YYYY-MM-DD"));
                    cboCEDatosEmpleadoSexo.val(response.data.lstDatos[0].sexo);
                    cboCEDatosEmpleadoSexo.trigger("change");
                    txtCEDatosEmpleadoRFC.val(response.data.lstDatos[0].rfc);
                    txtCEDatosEmpleadoCURP.val(response.data.lstDatos[0].curp);
                    cboCandidatosAprobados[0].selectedIndex = 0;
                    cboCandidatosAprobados.trigger("change");
                    lblTitleCrearEditarEmpleadoEK.html("Actualizar empleado");
                    //#endregion

                    //#region GENERALES Y CONTACTO
                    cboCEGenContactoEstadoCivil.val(response.data.lstGenerales[0].estado_civil);
                    cboCEGenContactoEstadoCivil.trigger("change");
                    txtCEGenContactoEstudios.val(response.data.lstGenerales[0].ocupacion);
                    txtCEGenContactoCalle.val(response.data.lstGenerales[0].domicilio);
                    txtCEGenContactoColonia.val(response.data.lstGenerales[0].colonia);
                    txtCEGenContactoCP.val(response.data.lstGenerales[0].codigo_postal);
                    txtCEGenContactoEmail.val(response.data.lstGenerales[0].email);
                    txtCEGenContactoAlergias.val(response.data.lstGenerales[0].alergias);
                    txtCEGenContactoFechaPlanta.val(moment(response.data.lstGenerales[0].fecha_planta).format("DD/MM/YYYY"));
                    txtCEGenContactoAbreviatura.val(response.data.lstGenerales[0].ocupacion_abrev);
                    txtCEGenContactoNumExterior.val(response.data.lstGenerales[0].numero_exterior);
                    cboCEGenContactoEstado.val(response.data.lstGenerales[0].estado_dom);
                    cboCEGenContactoEstado.trigger("change");
                    txtCEGenContactoTelCasa.val(response.data.lstGenerales[0].tel_casa);
                    cboCEGenContactoTipoCasa.val(response.data.lstGenerales[0].tipo_casa);
                    cboCEGenContactoTipoCasa.trigger("change");
                    txtCEGenContactoCredElector.val(response.data.lstGenerales[0].num_cred_elector);
                    txtCEGenContactoNumInterior.val(response.data.lstGenerales[0].numero_interior);
                    cboCEGenContactoCiudad.val(response.data.lstGenerales[0].ciudad_dom);
                    cboCEGenContactoCiudad.trigger("change");
                    txtCEGenContactoTelCelular.val(response.data.lstGenerales[0].tel_cel);
                    cboCEGenContactoTipoSangre.val(response.data.lstGenerales[0].tipo_sangre);
                    cboCEGenContactoTipoSangre.trigger("change");
                    //#endregion

                    //#region BENEFICIARIO
                    // console.log(response.data.lstGenerales[0].estado_ben);
                    cboCEBeneficiarioCiudad.fillCombo("/Administrativo/Reclutamientos/FillCboMunicipios", { _clavePais: 1, _claveEstado: response.data.lstGenerales[0].estado_ben }, false);
                    cboCEBeneficiarioCiudad.select2({ width: "100%" });

                    cboCEBeneficiarioEstado.select2({ width: "100%" });
                    cboCEBeneficiarioEstado.fillCombo("/Administrativo/Reclutamientos/FillCboEstados", { _clavePais: 1 }, false);

                    cboCEBeneficiarioParentesco.val(response.data.lstGenerales[0].parentesco_ben);
                    cboCEBeneficiarioParentesco.trigger("change");
                    txtCEBeneficiarioApePaterno.val(response.data.lstGenerales[0].paterno_ben);
                    cboCEBeneficiarioEstado.val(response.data.lstGenerales[0].estado_ben);
                    cboCEBeneficiarioEstado.trigger("change");
                    txtCEBeneficiarioDomicilio.val(response.data.lstGenerales[0].domicilio_ben);
                    txtCEBeneficiarioFechaNacimiento.val(moment(response.data.lstGenerales[0].fecha_nac_ben).format("DD/MM/YYYY"));
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
                    //#endregion

                    //#region COMPAÑIA
                    fncCECompaniaFillDepartamentos(txtCECompaniaCCDescripcion.attr("title"), true, response.data.lstDatos[0].clave_depto);

                    if (!esReingresoEmpleado) {
                        txtCECompaniaRequisicion.val(response.data.lstDatos[0].requisicion);
                        txtCECompaniaCCDescripcion.attr("title", response.data.lstDatos[0].cc_contable)
                        txtCECompaniaCCDescripcion.data('cc', response.data.lstDatos[0].cc);
                        txtCECompaniaCCDescripcion.val(response.data.lstDatos[0].nombreCC);
                        txtCECompaniaCCDescripcion.change();
                        txtCECompaniaPuestoDescripcion.data('puesto', response.data.lstDatos[0].puesto);
                        txtCECompaniaPuestoDescripcion.val(response.data.lstDatos[0].descripcion);
                        txtCECompaniaJefeInmediatoDescripcion.data('jefe_inmediato', response.data.lstDatos[0].jefe_inmediato);

                        let esSindicato = response.data.lstDatos[0].sindicato;
                        if (esSindicato != "N") {
                            chkCECompaniaSindicato.prop("checked", true);
                        } else {
                            chkCECompaniaSindicato.prop("checked", false);
                        }
                        chkCECompaniaSindicato.trigger("change");

                        txtCECompaniaUsuarioResg.val(response.data.lstDatos[0].usuario_compras);
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

                        fncGetIDUsuarioEK();
                    }

                    selectRegistroPatronal.val(response.data.lstDatos[0].id_regpat);
                    txtCECompaniaActividades.val(response.data.lstDatos[0].descripcion_puesto);
                    selectTipoContrato.val(response.data.lstDatos[0].duracion_contrato);

                    txtCECompaniaDepto.trigger("change");
                    txtCECompaniaNSS.val(response.data.lstDatos[0].nss);
                    cboCECompaniaTipoFormula.val(response.data.lstDatos[0].tipo_formula_imss);
                    cboCECompaniaTipoFormula.trigger("change");
                    txtCECompaniaJefeInmediatoDescripcion.val(response.data.lstDatos[0].nombreJefeInmediato);
                    txtCECompaniaDeptoDescripcion.val(response.data.lstDatos[0].desc_depto);
                    txtCECompaniaContrato.val(moment(response.data.lstDatos[0].fecha_contrato).format("YYYY-MM-DD"));
                    lblCECompaniaAltaEnElSistema.html(moment(response.data.lstDatos[0].fecha_alta).format("DD/MM/YYYY"));
                    lblCECompaniaAntiguedad.html(response.data.lstDatos[0].antiguedad);

                    txtCEUltimaModificacionIDEK.val(response.data.lstDatos[0].idUsuarioEK);
                    txtCEUltimaModificacionNombreUsuario.val(response.data.lstDatos[0].usuarioModificacion);
                    //#endregion

                    // fncHabilitarDeshabilitarCtrlsMdl(false);
                    // fncBorderDefault();

                    btnCEEmpleado.attr("data-esActualizar", 1);
                    btnCEEmpleado.attr("data-esReingresoEmpleado", 1);
                    btnCEEmpleado.html("Actualizar");
                    fncGetFamiliares();
                    fncGetContratos();
                    fncGetUniformes();
                    fncGetArchivoExamenMedico();
                    fncGetTabuladores();
                    fncGetDocs(claveEmpleado, null);
                    mdlCrearEditarEmpleadoEK.modal("show");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetFamiliares() {
            let clave_empleado = lblCEDatosEmpleadoClaveEmpleado.attr("data-clvEmpleado");
            let obj = new Object();
            obj = {
                clave_empleado: parseFloat(clave_empleado)
            }
            axios.post("/Administrativo/BajasPersonal/GetFamiliares", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtFamiliares.clear();
                    dtFamiliares.rows.add(response.data.lstFamiliaresEK);
                    dtFamiliares.draw();
                    //#endregion
                } else {
                    dtFamiliares.draw();

                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetContratos() {
            let clave_empleado = lblCEDatosEmpleadoClaveEmpleado.attr("data-clvEmpleado");
            let obj = new Object();
            obj = {
                clave_empleado: parseFloat(clave_empleado)
            }
            axios.post("/Administrativo/BajasPersonal/GetContratos", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtContratos.clear();
                    dtContratos.rows.add(response.data.data);
                    dtContratos.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetUniformes() {
            let obj = new Object();
            obj = {
                claveEmpleado: lblCEDatosEmpleadoClaveEmpleado.attr("data-clvEmpleado")
            }
            axios.post("/Administrativo/BajasPersonal/GetUniformes", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    if (response.data.lstUniformes != null) {
                        dtUniformes.clear();
                        dtUniformes.rows.add([response.data.lstUniformes]);
                        dtUniformes.draw();
                    } else {
                        dtUniformes.clear();
                        dtUniformes.draw();
                    }
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetArchivoExamenMedico() {
            let obj = new Object();
            obj = {
                claveEmpleado: lblCEDatosEmpleadoClaveEmpleado.attr("data-clvEmpleado")
            }
            axios.post("/Administrativo/BajasPersonal/GetArchivoExamenMedico", obj).then(response => {
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

        function fncGetTabuladores() {
            let obj = new Object();
            obj = {
                clave_empleado: lblCEDatosEmpleadoClaveEmpleado.attr("data-clvEmpleado")
            }
            axios.post("/Administrativo/BajasPersonal/GetTabuladores", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtTabuladores.clear();
                    dtTabuladores.rows.add(response.data.lstNomina);
                    dtTabuladores.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCECompaniaFillDepartamentos(cc, esActualizar, clave_depto) {
            let obj = new Object();
            obj = {
                cc: cc > 0 ? cc : ""
            }
            axios.post("/Administrativo/BajasPersonal/FillDepartamentos", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    txtCECompaniaDepto.fillCombo("FillDepartamentos", { cc: cc }, false);
                    txtCECompaniaDepto.select2();
                    txtCECompaniaDepto.select2({ width: "100%" });

                    if (esActualizar) {
                        txtCECompaniaDepto.val(clave_depto);
                        txtCECompaniaDepto.trigger("change");
                    }
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region CANCELACION DE BAJA

        function fncCancelarAutorizacionBaja(idRow, comment) {
            let obj = {
                baja_id: idRow,
                comentariosCancelacion: comment
            }
            axios.post("/Administrativo/BajasPersonal/CancelarAutorizacionBajas", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetBajasPersonal();
                }
            }).catch(error => Alert2Error(error.message));
        }

        //#endregion

        //#region LIBERACION DE BAJA
        function fncLiberarNomina(idBaja, comentario) {
            let obj = {
                idBaja,
                comentario
            };

            axios.post("/Administrativo/BajasPersonal/SetNominaLiberada", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    Alert2Exito("Baja liberada con exito");
                    fncGetBajasPersonal();
                } else {
                    Alert2Warning("Ocurrio algo mal con la liberacion, favor de contactarse con el departamento de TI");

                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.Bajas = new Bajas();
    })

        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();