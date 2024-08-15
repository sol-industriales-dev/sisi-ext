(() => {
    $.namespace('SaludOcupacional.HistorialClinico');

    //#region CONST SECCIONES
    //#region CONST SECCION DATOS PERSONAS
    const txt_dtsPer_Folio = $('#txt_dtsPer_Folio');
    const txt_dtsPer_ImagenPersona = $('#txt_dtsPer_ImagenPersona');
    const cbo_dtsPer_EmpresaID = $('#cbo_dtsPer_EmpresaID');
    const cbo_dtsPer_CCID = $('#cbo_dtsPer_CCID');
    const txt_dtsPer_Paciente = $('#txt_dtsPer_Paciente');
    const txt_dtsPer_FechaHora = $('#txt_dtsPer_FechaHora');
    const txt_dtsPer_FechaNac = $('#txt_dtsPer_FechaNac');
    const btnGetEdadPaciente = $('#btnGetEdadPaciente');
    const txt_dtsPer_Edad = $('#txt_dtsPer_Edad');
    const cbo_dtsPer_Sexo = $('#cbo_dtsPer_Sexo');
    const cbo_dtsPer_EstadoCivil = $('#cbo_dtsPer_EstadoCivil');
    const cbo_dtsPer_TipoSanguineo = $('#cbo_dtsPer_TipoSanguineo');
    const txt_dtsPer_CURP = $('#txt_dtsPer_CURP');
    const txt_dtsPer_Domicilio = $('#txt_dtsPer_Domicilio');
    const txt_dtsPer_Ciudad = $('#txt_dtsPer_Ciudad');
    const cbo_dtsPer_Escolaridad = $('#cbo_dtsPer_Escolaridad');
    const txt_dtsPer_LugarNacimiento = $('#txt_dtsPer_LugarNacimiento');
    const txt_dtsPer_Telefono = $('#txt_dtsPer_Telefono');
    //#endregion
    //#region CONST SECCION MOTIVO DE LA EVALUACION
    const chk_motEva_esIngreso = $('#chk_motEva_esIngreso');
    const chk_motEva_esRetiro = $('#chk_motEva_esRetiro');
    const chk_motEva_esEvaOpcional = $('#chk_motEva_esEvaOpcional');
    const chk_motEva_esPostIncapacidad = $('#chk_motEva_esPostIncapacidad');
    const chk_motEva_esReubicacion = $('#chk_motEva_esReubicacion');
    //#endregion
    //#region CONST ANTECEDENTES LABORALES
    const txt_antLab_Puesto = $('#txt_antLab_Puesto');
    const txt_antLab_Empresa = $('#txt_antLab_Empresa');
    const txt_antLab_Desde = $('#txt_antLab_Desde');
    const txt_antLab_Hasta = $('#txt_antLab_Hasta');
    const txt_antLab_Turno = $('#txt_antLab_Turno');
    const chk_antLab_esDePie = $('#chk_antLab_esDePie');
    const chk_antLab_esInclinado = $('#chk_antLab_esInclinado');
    const chk_antLab_esSentado = $('#chk_antLab_esSentado');
    const chk_antLab_esArrodillado = $('#chk_antLab_esArrodillado');
    const chk_antLab_esCaminando = $('#chk_antLab_esCaminando');
    const chk_antLab_esOtra = $('#chk_antLab_esOtra');
    const txt_antLab_Cual = $('#txt_antLab_Cual');
    //#endregion
    //#region ACCIDENTES Y ENFERMEDADES DE TRABAJO
    const txt_accET_Empresa = $('#txt_accET_Empresa');
    const txt_accET_Anio = $('#txt_accET_Anio');
    const txt_accET_LesionAreaAnatomica = $('#txt_accET_LesionAreaAnatomica');
    const txt_accET_Secuelas = $('#txt_accET_Secuelas');
    const txt_accET_Cuales = $('#txt_accET_Cuales');
    const txt_accET_ExamNoAceptables = $('#txt_accET_ExamNoAceptables');
    const txt_accET_Causas = $('#txt_accET_Causas');
    const txt_accET_AbandonoTrabajo = $('#txt_accET_AbandonoTrabajo');
    const txt_accET_IncapacidadFrecuente = $('#txt_accET_IncapacidadFrecuente');
    const txt_accET_Prolongadas = $('#txt_accET_Prolongadas');
    //#endregion
    //#region USO DE ELEMENTOS DE PROTECCIÓN PERSONAL
    const chk_usoElePP_esActual = $('#chk_usoElePP_esActual');
    const chk_usoElePP_esCasco = $('#chk_usoElePP_esCasco');
    const chk_usoElePP_esTapaboca = $('#chk_usoElePP_esTapaboca');
    const chk_usoElePP_esGafas = $('#chk_usoElePP_esGafas');
    const chk_usoElePP_esRespirador = $('#chk_usoElePP_esRespirador');
    const chk_usoElePP_esBotas = $('#chk_usoElePP_esBotas');
    const chk_usoElePP_esAuditivos = $('#chk_usoElePP_esAuditivos');
    const chk_usoElePP_esOverol = $('#chk_usoElePP_esOverol');
    const chk_usoElePP_esGuantes = $('#chk_usoElePP_esGuantes');
    const txt_usoElePP_OtroCual = $('#txt_usoElePP_OtroCual');
    const txt_usoElePP_DeberiaRecibir = $('#txt_usoElePP_DeberiaRecibir');
    const txt_usoElePP_ConsideraAdecuado = $('#txt_usoElePP_ConsideraAdecuado');
    //#endregion
    //#region ANTECEDENTES FAMILIARES
    const txt_antFam_esTuberculosis = $("#txt_antFam_esTuberculosis");
    const txt_antFam_TuberculosisParentesco = $("#txt_antFam_TuberculosisParentesco");
    const txt_antFam_esHTA = $("#txt_antFam_esHTA");
    const txt_antFam_HTAParentesco = $("#txt_antFam_HTAParentesco");
    const txt_antFam_esDiabetes = $("#txt_antFam_esDiabetes");
    const txt_antFam_DiabetesParentesco = $("#txt_antFam_DiabetesParentesco");
    const txt_antFam_esACV = $("#txt_antFam_esACV");
    const txt_antFam_ACVParentesco = $("#txt_antFam_ACVParentesco");
    const txt_antFam_esInfarto = $("#txt_antFam_esInfarto");
    const txt_antFam_InfartoParentesco = $("#txt_antFam_InfartoParentesco");
    const txt_antFam_esAsma = $("#txt_antFam_esAsma");
    const txt_antFam_AsmaParentesco = $("#txt_antFam_AsmaParentesco");
    const txt_antFam_esAlergias = $("#txt_antFam_esAlergias");
    const txt_antFam_AlergiasParentesco = $("#txt_antFam_AlergiasParentesco");
    const txt_antFam_esMental = $("#txt_antFam_esMental");
    const txt_antFam_MentalParentesco = $("#txt_antFam_MentalParentesco");
    const txt_antFam_esCancer = $("#txt_antFam_esCancer");
    const txt_antFam_CancerParentesco = $("#txt_antFam_CancerParentesco");
    const txt_antFam_Observaciones = $("#txt_antFam_Observaciones");
    //#endregion
    //#region ANTECEDENTES PERSONALES NO PATOLÓGICOS
    const cbo_antPerNoPat_Tabaquismo = $("#cbo_antPerNoPat_Tabaquismo");
    const txt_antPerNoPat_CigarroDia = $("#txt_antPerNoPat_CigarroDia");
    const txt_antPerNoPat_CigarroAnios = $("#txt_antPerNoPat_CigarroAnios");
    const cbo_antPerNoPat_Alcoholismo = $("#cbo_antPerNoPat_Alcoholismo");
    const txt_antPerNoPat_AlcoholismoAnios = $("#txt_antPerNoPat_AlcoholismoAnios");
    const chk_antPerNoPat_esDrogradiccion = $("#chk_antPerNoPat_esDrogradiccion");
    const chk_antPerNoPat_esMarihuana = $("#chk_antPerNoPat_esMarihuana");
    const chk_antPerNoPat_esCocaina = $("#chk_antPerNoPat_esCocaina");
    const chk_antPerNoPat_esAnfetaminas = $("#chk_antPerNoPat_esAnfetaminas");
    const txt_antPerNoPat_Otros = $("#txt_antPerNoPat_Otros");
    const txt_antPerNoPat_Inmunizaciones = $("#txt_antPerNoPat_Inmunizaciones");
    const txt_antPerNoPat_Tetanicos = $("#txt_antPerNoPat_Tetanicos");
    const txt_antPerNoPat_FechaAntitenica = $("#txt_antPerNoPat_FechaAntitenica");
    const txt_antPerNoPat_Hepatitis = $("#txt_antPerNoPat_Hepatitis");
    const txt_antPerNoPat_Influenza = $("#txt_antPerNoPat_Influenza");
    const txt_antPerNoPat_FechaInfluenza = $("#txt_antPerNoPat_FechaInfluenza");
    const txt_antPerNoPat_Infancia = $("#txt_antPerNoPat_Infancia");
    const txt_antPerNoPat_DescInfancia = $("#txt_antPerNoPat_DescInfancia");
    const txt_antPerNoPat_Alimentacion = $("#txt_antPerNoPat_Alimentacion");
    const txt_antPerNoPat_Higiene = $("#txt_antPerNoPat_Higiene");
    const txt_antPerNoPat_MedicacionActual = $("#txt_antPerNoPat_MedicacionActual");
    //#endregion
    //#region ANTECEDENTES PERSONALES PATOLÓGICOS
    const chk_antPerPat_esNeoplasicos = $("#chk_antPerPat_esNeoplasicos");
    const chk_antPerPat_esNeumopatias = $("#chk_antPerPat_esNeumopatias");
    const chk_antPerPat_esAsma = $("#chk_antPerPat_esAsma");
    const chk_antPerPat_esFimico = $("#chk_antPerPat_esFimico");
    const chk_antPerPat_esNeumoconiosis = $("#chk_antPerPat_esNeumoconiosis");
    const chk_antPerPat_esCardiopatias = $("#chk_antPerPat_esCardiopatias");
    const chk_antPerPat_esReumaticos = $("#chk_antPerPat_esReumaticos");
    const chk_antPerPat_esAlergias = $("#chk_antPerPat_esAlergias");
    const chk_antPerPat_esHipertension = $("#chk_antPerPat_esHipertension");
    const chk_antPerPat_esHepatitis = $("#chk_antPerPat_esHepatitis");
    const chk_antPerPat_esTifoidea = $("#chk_antPerPat_esTifoidea");
    const chk_antPerPat_esHernias = $("#chk_antPerPat_esHernias");
    const chk_antPerPat_esLumbalgias = $("#chk_antPerPat_esLumbalgias");
    const chk_antPerPat_esDiabetes = $("#chk_antPerPat_esDiabetes");
    const chk_antPerPat_esEpilepsias = $("#chk_antPerPat_esEpilepsias");
    const chk_antPerPat_esVenereas = $("#chk_antPerPat_esVenereas");
    const chk_antPerPat_esCirugias = $("#chk_antPerPat_esCirugias");
    const chk_antPerPat_esFracturas = $("#chk_antPerPat_esFracturas");
    const txt_antPerPat_ObservacionesPat = $("#txt_antPerPat_ObservacionesPat");
    //#endregion
    //#region INTERROGATORIO POR APARATOS Y SISTEMAS
    const chk_intApaSis_esRespiratorio = $("#chk_intApaSis_esRespiratorio");
    const chk_intApaSis_esDigestivo = $("#chk_intApaSis_esDigestivo");
    const chk_intApaSis_esCardiovascular = $("#chk_intApaSis_esCardiovascular");
    const chk_intApaSis_esNervioso = $("#chk_intApaSis_esNervioso");
    const chk_intApaSis_esUrinario = $("#chk_intApaSis_esUrinario");
    const chk_intApaSis_esEndocrino = $("#chk_intApaSis_esEndocrino");
    const chk_intApaSis_esPsiquiatrico = $("#chk_intApaSis_esPsiquiatrico");
    const chk_intApaSis_esEsqueletico = $("#chk_intApaSis_esEsqueletico");
    const chk_intApaSis_esAudicion = $("#chk_intApaSis_esAudicion");
    const chk_intApaSis_esVision = $("#chk_intApaSis_esVision");
    const chk_intApaSis_esOlfato = $("#chk_intApaSis_esOlfato");
    const chk_intApaSis_esTacto = $("#chk_intApaSis_esTacto");
    const txt_intApaSis_ObservacionesPat = $("#txt_intApaSis_ObservacionesPat");
    //#endregion
    //#region PADECIMIENTOS ACTUALES
    const txt_padAct_PadActuales = $('#txt_padAct_PadActuales');
    //#endregion
    //#region EXPLORACIÓN FÍSICA-SIGNOS VITALES
    const txt_expFSV_TArterial = $("#txt_expFSV_TArterial");
    const txt_expFSV_Pulso = $("#txt_expFSV_Pulso");
    const txt_expFSV_Temp = $("#txt_expFSV_Temp");
    const txt_expFSV_FCardiaca = $("#txt_expFSV_FCardiaca");
    const txt_expFSV_FResp = $("#txt_expFSV_FResp");
    const txt_expFSV_Peso = $("#txt_expFSV_Peso");
    const txt_expFSV_Talla = $("#txt_expFSV_Talla");
    const txt_expFSV_IMC = $("#txt_expFSV_IMC");
    //#endregion
    //#region EXPLORACIÓN FÍSICA-CABEZA
    const cbo_expFC_Craneo = $("#cbo_expFC_Craneo");
    const cbo_expFC_Parpados = $("#cbo_expFC_Parpados");
    const cbo_expFC_Conjutiva = $("#cbo_expFC_Conjutiva");
    const cbo_expFC_Reflejos = $("#cbo_expFC_Reflejos");
    const cbo_expFC_FosasNasales = $("#cbo_expFC_FosasNasales");
    const cbo_expFC_Boca = $("#cbo_expFC_Boca");
    const cbo_expFC_Amigdalas = $("#cbo_expFC_Amigdalas");
    const cbo_expFC_Dentadura = $("#cbo_expFC_Dentadura");
    const cbo_expFC_Encias = $("#cbo_expFC_Encias");
    const cbo_expFC_Cuello = $("#cbo_expFC_Cuello");
    const cbo_expFC_Tiroides = $("#cbo_expFC_Tiroides");
    const cbo_expFC_Ganglios = $("#cbo_expFC_Ganglios");
    const cbo_expFC_Oidos = $("#cbo_expFC_Oidos");
    const cbo_expFC_Otros = $("#cbo_expFC_Otros");
    const txt_expFC_Observaciones = $("#txt_expFC_Observaciones");
    //#endregion
    //#region EXPLORACIÓN FÍSICA-AGUDEZA VISUAL
    const txt_expFAV_VisCerAmbosOjos = $("#txt_expFAV_VisCerAmbosOjos");
    const txt_expFAV_VisCerOjoIzq = $("#txt_expFAV_VisCerOjoIzq");
    const txt_expFAV_VisCerOjoDer = $("#txt_expFAV_VisCerOjoDer");
    const txt_expFAV_VisLejAmbosOjos = $("#txt_expFAV_VisLejAmbosOjos");
    const txt_expFAV_VisLejOjoIzq = $("#txt_expFAV_VisLejOjoIzq");
    const txt_expFAV_VisLejOjoDer = $("#txt_expFAV_VisLejOjoDer");
    const txt_expFAV_CorregidaAmbosOjos = $("#txt_expFAV_CorregidaAmbosOjos");
    const txt_expFAV_CorregidaOjoIzq = $("#txt_expFAV_CorregidaOjoIzq");
    const txt_expFAV_CorregidaOjoDer = $("#txt_expFAV_CorregidaOjoDer");
    const txt_expFAV_CampimetriaOI = $("#txt_expFAV_CampimetriaOI");
    const txt_expFAV_CampimetriaOD = $("#txt_expFAV_CampimetriaOD");
    const txt_expFAV_PterigionOI = $("#txt_expFAV_PterigionOI");
    const txt_expFAV_PterigionOD = $("#txt_expFAV_PterigionOD");
    const txt_expFAV_FondoOjo = $("#txt_expFAV_FondoOjo");
    const txt_expFAV_Daltonismo = $("#txt_expFAV_Daltonismo");
    const txt_expFAV_Observaciones = $("#txt_expFAV_Observaciones");
    //#endregion
    //#region EXPLORACIÓN FÍSICA-TORAX, ABDOMEN, TRONCO Y EXTREMIDADES
    const chk_expFTATE_esCamposPulmonares = $("#chk_expFTATE_esCamposPulmonares");
    const chk_expFTATE_esPuntosDolorosos = $("#chk_expFTATE_esPuntosDolorosos");
    const chk_expFTATE_esGenitales = $("#chk_expFTATE_esGenitales");
    const chk_expFTATE_esRuidosCardiacos = $("#chk_expFTATE_esRuidosCardiacos");
    const chk_expFTATE_esHallusValgus = $("#chk_expFTATE_esHallusValgus");
    const chk_expFTATE_esHerniasUmbili = $("#chk_expFTATE_esHerniasUmbili");
    const chk_expFTATE_esAreaRenal = $("#chk_expFTATE_esAreaRenal");
    const chk_expFTATE_esVaricocele = $("#chk_expFTATE_esVaricocele");
    const chk_expFTATE_esGrandulasMamarias = $("#chk_expFTATE_esGrandulasMamarias");
    const chk_expFTATE_esColumnaVertebral = $("#chk_expFTATE_esColumnaVertebral");
    const chk_expFTATE_esPiePlano = $("#chk_expFTATE_esPiePlano");
    const chk_expFTATE_esVarices = $("#chk_expFTATE_esVarices");
    const chk_expFTATE_esMiembrosSup = $("#chk_expFTATE_esMiembrosSup");
    const chk_expFTATE_esParedAbdominal = $("#chk_expFTATE_esParedAbdominal");
    const chk_expFTATE_esAnillosInguinales = $("#chk_expFTATE_esAnillosInguinales");
    const chk_expFTATE_esMiembrosInf = $("#chk_expFTATE_esMiembrosInf");
    const chk_expFTATE_esTatuajes = $("#chk_expFTATE_esTatuajes");
    const chk_expFTATE_esVisceromegalias = $("#chk_expFTATE_esVisceromegalias");
    const chk_expFTATE_esMarcha = $("#chk_expFTATE_esMarcha");
    const chk_expFTATE_esHerniasInguinales = $("#chk_expFTATE_esHerniasInguinales");
    const chk_expFTATE_esHombrosDolorosos = $("#chk_expFTATE_esHombrosDolorosos");
    const chk_expFTATE_esQuistes = $("#chk_expFTATE_esQuistes");
    const txt_expFTATE_Observaciones = $("#txt_expFTATE_Observaciones");
    const chk_expFTATE_MS_HombroDer_esFlexion = $("#chk_expFTATE_MS_HombroDer_esFlexion");
    const chk_expFTATE_MS_HombroIzq_esFlexion = $("#chk_expFTATE_MS_HombroIzq_esFlexion");
    const chk_expFTATE_MS_CodoDer_esFlexion = $("#chk_expFTATE_MS_CodoDer_esFlexion");
    const chk_expFTATE_MS_CodoIzq_esFlexion = $("#chk_expFTATE_MS_CodoIzq_esFlexion");
    const chk_expFTATE_MS_MunecaDer_esFlexion = $("#chk_expFTATE_MS_MunecaDer_esFlexion");
    const chk_expFTATE_MS_MunecaIzq_esFlexion = $("#chk_expFTATE_MS_MunecaIzq_esFlexion");
    const chk_expFTATE_MS_DedosDer_esFlexion = $("#chk_expFTATE_MS_DedosDer_esFlexion");
    const chk_expFTATE_MS_DedosIzq_esFlexion = $("#chk_expFTATE_MS_DedosIzq_esFlexion");
    const chk_expFTATE_MS_HombroDer_esExtension = $("#chk_expFTATE_MS_HombroDer_esExtension");
    const chk_expFTATE_MS_HombroIzq_esExtension = $("#chk_expFTATE_MS_HombroIzq_esExtension");
    const chk_expFTATE_MS_CodoDer_esExtension = $("#chk_expFTATE_MS_CodoDer_esExtension");
    const chk_expFTATE_MS_CodoIzq_esExtension = $("#chk_expFTATE_MS_CodoIzq_esExtension");
    const chk_expFTATE_MS_MunecaDer_esExtension = $("#chk_expFTATE_MS_MunecaDer_esExtension");
    const chk_expFTATE_MS_MunecaIzq_esExtension = $("#chk_expFTATE_MS_MunecaIzq_esExtension");
    const chk_expFTATE_MS_DedosDer_esExtension = $("#chk_expFTATE_MS_DedosDer_esExtension");
    const chk_expFTATE_MS_DedosIzq_esExtension = $("#chk_expFTATE_MS_DedosIzq_esExtension");
    const chk_expFTATE_MS_HombroDer_esAbduccion = $("#chk_expFTATE_MS_HombroDer_esAbduccion");
    const chk_expFTATE_MS_HombroIzq_esAbduccion = $("#chk_expFTATE_MS_HombroIzq_esAbduccion");
    const chk_expFTATE_MS_CodoDer_esAbduccion = $("#chk_expFTATE_MS_CodoDer_esAbduccion");
    const chk_expFTATE_MS_CodoIzq_esAbduccion = $("#chk_expFTATE_MS_CodoIzq_esAbduccion");
    const chk_expFTATE_MS_MunecaDer_esAbduccion = $("#chk_expFTATE_MS_MunecaDer_esAbduccion");
    const chk_expFTATE_MS_MunecaIzq_esAbduccion = $("#chk_expFTATE_MS_MunecaIzq_esAbduccion");
    const chk_expFTATE_MS_DedosDer_esAbduccion = $("#chk_expFTATE_MS_DedosDer_esAbduccion");
    const chk_expFTATE_MS_DedosIzq_esAbduccion = $("#chk_expFTATE_MS_DedosIzq_esAbduccion");
    const chk_expFTATE_MS_HombroDer_esAduccion = $("#chk_expFTATE_MS_HombroDer_esAduccion");
    const chk_expFTATE_MS_HombroIzq_esAduccion = $("#chk_expFTATE_MS_HombroIzq_esAduccion");
    const chk_expFTATE_MS_MunecaDer_esAduccion = $("#chk_expFTATE_MS_MunecaDer_esAduccion");
    const chk_expFTATE_MS_MunecaIzq_esAduccion = $("#chk_expFTATE_MS_MunecaIzq_esAduccion");
    const chk_expFTATE_MS_DedosDer_esAduccion = $("#chk_expFTATE_MS_DedosDer_esAduccion");
    const chk_expFTATE_MS_DedosIzq_esAduccion = $("#chk_expFTATE_MS_DedosIzq_esAduccion");
    const chk_expFTATE_MS_HombroDer_esRotInterna = $("#chk_expFTATE_MS_HombroDer_esRotInterna");
    const chk_expFTATE_MS_HombroIzq_esRotInterna = $("#chk_expFTATE_MS_HombroIzq_esRotInterna");
    const chk_expFTATE_MS_MunecaDer_esRotInterna = $("#chk_expFTATE_MS_MunecaDer_esRotInterna");
    const chk_expFTATE_MS_MunecaIzq_esRotInterna = $("#chk_expFTATE_MS_MunecaIzq_esRotInterna");
    const chk_expFTATE_MS_DedosDer_esRotInterna = $("#chk_expFTATE_MS_DedosDer_esRotInterna");
    const chk_expFTATE_MS_DedosIzq_esRotInterna = $("#chk_expFTATE_MS_DedosIzq_esRotInterna");
    const chk_expFTATE_MS_HombroDer_esRotExterna = $("#chk_expFTATE_MS_HombroDer_esRotExterna");
    const chk_expFTATE_MS_HombroIzq_esRotExterna = $("#chk_expFTATE_MS_HombroIzq_esRotExterna");
    const chk_expFTATE_MS_MunecaDer_esRotExterna = $("#chk_expFTATE_MS_MunecaDer_esRotExterna");
    const chk_expFTATE_MS_MunecaIzq_RotExterna = $("#chk_expFTATE_MS_MunecaIzq_RotExterna");
    const chk_expFTATE_MS_DedosDer_RotExterna = $("#chk_expFTATE_MS_DedosDer_RotExterna");
    const chk_expFTATE_MS_DedosIzq_RotExterna = $("#chk_expFTATE_MS_DedosIzq_RotExterna");
    const chk_expFTATE_MS_CodoDer_esPronacion = $("#chk_expFTATE_MS_CodoDer_esPronacion");
    const chk_expFTATE_MS_CodoIzq_esPronacion = $("#chk_expFTATE_MS_CodoIzq_esPronacion");
    const chk_expFTATE_MS_MunecaDer_esPronacion = $("#chk_expFTATE_MS_MunecaDer_esPronacion");
    const chk_expFTATE_MS_MunecaIzq_esPronacion = $("#chk_expFTATE_MS_MunecaIzq_esPronacion");
    const chk_expFTATE_MS_CodoDer_esSupinacion = $("#chk_expFTATE_MS_CodoDer_esSupinacion");
    const chk_expFTATE_MS_CodoIzq_esSupinacion = $("#chk_expFTATE_MS_CodoIzq_esSupinacion");
    const chk_expFTATE_MS_MunecaDer_esSupinacion = $("#chk_expFTATE_MS_MunecaDer_esSupinacion");
    const chk_expFTATE_MS_MunecaIzq_esSupinacion = $("#chk_expFTATE_MS_MunecaIzq_esSupinacion");
    const chk_expFTATE_MS_MunecaDer_esDesvUlnar = $("#chk_expFTATE_MS_MunecaDer_esDesvUlnar");
    const chk_expFTATE_MS_MunecaIzq_esDesvUlnar = $("#chk_expFTATE_MS_MunecaIzq_esDesvUlnar");
    const chk_expFTATE_MS_MunecaDer_esDesvRadial = $("#chk_expFTATE_MS_MunecaDer_esDesvRadial");
    const chk_expFTATE_MS_MunecaIzq_esDesvRadial = $("#chk_expFTATE_MS_MunecaIzq_esDesvRadial");
    const chk_expFTATE_MS_MunecaDer_esOponencia = $("#chk_expFTATE_MS_MunecaDer_esOponencia");
    const chk_expFTATE_MS_MunecaIzq_esOponencia = $("#chk_expFTATE_MS_MunecaIzq_esOponencia");
    const chk_expFTATE_MS_DedosDer_esOponencia = $("#chk_expFTATE_MS_DedosDer_esOponencia");
    const chk_expFTATE_MS_DedosIzq_esOponencia = $("#chk_expFTATE_MS_DedosIzq_esOponencia");
    const chk_expFTATE_MI_CaderaDer_esFlexion = $("#chk_expFTATE_MI_CaderaDer_esFlexion");
    const chk_expFTATE_MI_CaderaIzq_esFlexion = $("#chk_expFTATE_MI_CaderaIzq_esFlexion");
    const chk_expFTATE_MI_RodillasDer_esFlexion = $("#chk_expFTATE_MI_RodillasDer_esFlexion");
    const chk_expFTATE_MI_RodillasIzq_esFlexion = $("#chk_expFTATE_MI_RodillasIzq_esFlexion");
    const chk_expFTATE_MI_CllPieDer_esFlexion = $("#chk_expFTATE_MI_CllPieDer_esFlexion");
    const chk_expFTATE_MI_CllPieIzq_esFlexion = $("#chk_expFTATE_MI_CllPieIzq_esFlexion");
    const chk_expFTATE_MI_DedosDer_esFlexion = $("#chk_expFTATE_MI_DedosDer_esFlexion");
    const chk_expFTATE_MI_DedosIzq_esFlexion = $("#chk_expFTATE_MI_DedosIzq_esFlexion");
    const chk_expFTATE_MI_CaderaDer_esExtension = $("#chk_expFTATE_MI_CaderaDer_esExtension");
    const chk_expFTATE_MI_CaderaIzq_esExtension = $("#chk_expFTATE_MI_CaderaIzq_esExtension");
    const chk_expFTATE_MI_RodillasDer_esExtension = $("#chk_expFTATE_MI_RodillasDer_esExtension");
    const chk_expFTATE_MI_RodillasIzq_esExtension = $("#chk_expFTATE_MI_RodillasIzq_esExtension");
    const chk_expFTATE_MI_CllPieDer_esExtension = $("#chk_expFTATE_MI_CllPieDer_esExtension");
    const chk_expFTATE_MI_CllPieIzq_esExtension = $("#chk_expFTATE_MI_CllPieIzq_esExtension");
    const chk_expFTATE_MI_DedosDer_esExtension = $("#chk_expFTATE_MI_DedosDer_esExtension");
    const chk_expFTATE_MI_DedosIzq_esExtension = $("#chk_expFTATE_MI_DedosIzq_esExtension");
    const chk_expFTATE_MI_CaderaDer_esAbduccion = $("#chk_expFTATE_MI_CaderaDer_esAbduccion");
    const chk_expFTATE_MI_CaderaIzq_esAbduccion = $("#chk_expFTATE_MI_CaderaIzq_esAbduccion");
    const chk_expFTATE_MI_CllPieDer_esAbduccion = $("#chk_expFTATE_MI_CllPieDer_esAbduccion");
    const chk_expFTATE_MI_CllPieIzq_esAbduccion = $("#chk_expFTATE_MI_CllPieIzq_esAbduccion");
    const chk_expFTATE_MI_DedosDer_esAbduccion = $("#chk_expFTATE_MI_DedosDer_esAbduccion");
    const chk_expFTATE_MI_DedosIzq_esAbduccion = $("#chk_expFTATE_MI_DedosIzq_esAbduccion");
    const chk_expFTATE_MI_CaderaDer_esAduccion = $("#chk_expFTATE_MI_CaderaDer_esAduccion");
    const chk_expFTATE_MI_CaderaIzq_esAduccion = $("#chk_expFTATE_MI_CaderaIzq_esAduccion");
    const chk_expFTATE_MI_CllPieDer_esAduccion = $("#chk_expFTATE_MI_CllPieDer_esAduccion");
    const chk_expFTATE_MI_CllPieIzq_esAduccion = $("#chk_expFTATE_MI_CllPieIzq_esAduccion");
    const chk_expFTATE_MI_DedosDer_esAduccion = $("#chk_expFTATE_MI_DedosDer_esAduccion");
    const chk_expFTATE_MI_DedosIzq_esAduccion = $("#chk_expFTATE_MI_DedosIzq_esAduccion");
    const chk_expFTATE_MI_CaderaDer_esRotInterna = $("#chk_expFTATE_MI_CaderaDer_esRotInterna");
    const chk_expFTATE_MI_CaderaIzq_esRotInterna = $("#chk_expFTATE_MI_CaderaIzq_esRotInterna");
    const chk_expFTATE_MI_RodillasDer_esRotInterna = $("#chk_expFTATE_MI_RodillasDer_esRotInterna");
    const chk_expFTATE_MI_RodillasIzq_esRotInterna = $("#chk_expFTATE_MI_RodillasIzq_esRotInterna");
    const chk_expFTATE_MI_CllPieDer_esRotInterna = $("#chk_expFTATE_MI_CllPieDer_esRotInterna");
    const chk_expFTATE_MI_CllPieIzq_esRotInterna = $("#chk_expFTATE_MI_CllPieIzq_esRotInterna");
    const chk_expFTATE_MI_CaderaDer_esRotExterna = $("#chk_expFTATE_MI_CaderaDer_esRotExterna");
    const chk_expFTATE_MI_CaderaIzq_esRotExterna = $("#chk_expFTATE_MI_CaderaIzq_esRotExterna");
    const chk_expFTATE_MI_RodillasDer_esRotExterna = $("#chk_expFTATE_MI_RodillasDer_esRotExterna");
    const chk_expFTATE_MI_RodillasIzq_esRotExterna = $("#chk_expFTATE_MI_RodillasIzq_esRotExterna");
    const chk_expFTATE_MS_MunecaIzq_esRotExterna = $("#chk_expFTATE_MS_MunecaIzq_esRotExterna");
    const chk_expFTATE_MS_DedosDer_esRotExterna = $("#chk_expFTATE_MS_DedosDer_esRotExterna");
    const chk_expFTATE_MS_DedosIzq_esRotExterna = $("#chk_expFTATE_MS_DedosIzq_esRotExterna");
    const chk_expFTATE_MI_CllPieDer_esRotExterna = $("#chk_expFTATE_MI_CllPieDer_esRotExterna");
    const chk_expFTATE_MI_CllPieIzq_esRotExterna = $("#chk_expFTATE_MI_CllPieIzq_esRotExterna");
    const chk_expFTATE_MI_CllPieDer_esInversion = $("#chk_expFTATE_MI_CllPieDer_esInversion");
    const chk_expFTATE_MI_CllPieIzq_esInversion = $("#chk_expFTATE_MI_CllPieIzq_esInversion");
    const chk_expFTATE_MI_CllPieDer_esEversion = $("#chk_expFTATE_MI_CllPieDer_esEversion");
    const chk_expFTATE_MI_CllPieIzq_esEversion = $("#chk_expFTATE_MI_CllPieIzq_esEversion");
    const txt_expFTATE_MS_MI_Observaciones = $("#txt_expFTATE_MS_MI_Observaciones");
    //#endregion
    //#region EXPLORACIÓN FÍSICA-DATOS GENERALES
    const txt_expFDT_PielMucosas = $("#txt_expFDT_PielMucosas");
    const txt_expFDT_EstadoPsiquiatrico = $("#txt_expFDT_EstadoPsiquiatrico");
    const txt_expFDT_ExamenNeurologico = $("#txt_expFDT_ExamenNeurologico");
    const txt_expFDT_FobiasActuales = $("#txt_expFDT_FobiasActuales");
    const txt_expFDT_Higiene = $("#txt_expFDT_Higiene");
    const txt_expFDT_ConstitucionFisica = $("#txt_expFDT_ConstitucionFisica");
    const txt_expFDT_Otros = $("#txt_expFDT_Otros");
    const txt_expFDT_Observaciones = $("#txt_expFDT_Observaciones");
    //#endregion
    //#region ESTUDIOS DE GABINETE
    const cbo_estGab_TipoSanguineoID = $("#cbo_estGab_TipoSanguineoID");
    const cbo_estGab_Antidoping = $("#cbo_estGab_Antidoping");
    const txt_estGab_Laboratorios = $("#txt_estGab_Laboratorios");
    const txt_estGab_ObservacionesGrupoRH = $("#txt_estGab_ObservacionesGrupoRH");
    const txt_estGab_ExamGenOrina = $("#txt_estGab_ExamGenOrina");
    const txt_estGab_ExamGenOrinaObservaciones = $("#txt_estGab_ExamGenOrinaObservaciones");
    const txt_estGab_Radiografias = $("#txt_estGab_Radiografias");
    const txt_estGab_RadiografiasObservaciones = $("#txt_estGab_RadiografiasObservaciones");
    const txt_estGab_Audiometria = $("#txt_estGab_Audiometria");
    const txt_estGab_HBC = $("#txt_estGab_HBC");
    const txt_estGab_AudiometriaObservaciones = $("#txt_estGab_AudiometriaObservaciones");
    const txt_estGab_Espirometria = $("#txt_estGab_Espirometria");
    const txt_estGab_EspirometriaObservaciones = $("#txt_estGab_EspirometriaObservaciones");
    const txt_estGab_Electrocardiograma = $("#txt_estGab_Electrocardiograma");
    const txt_estGab_ElectrocardiogramaObservaciones = $("#txt_estGab_ElectrocardiogramaObservaciones");
    const txt_estGab_FechaPrimeraDosis = $("#txt_estGab_FechaPrimeraDosis");
    const txt_estGab_FechaSegundaDosis = $("#txt_estGab_FechaSegundaDosis");
    const cbo_estGab_MarcaDosisID = $("#cbo_estGab_MarcaDosisID");
    const txt_estGab_VacunacionObservaciones = $("#txt_estGab_VacunacionObservaciones");
    const txt_estGab_LstProblemas = $("#txt_estGab_LstProblemas");
    const txt_estGab_Recomendaciones = $("#txt_estGab_Recomendaciones");
    //#endregion
    //#region ESPIROMETRÍA
    const txt_esp_CargarEspirometria = $("#txt_esp_CargarEspirometria");
    const txt_esp_Espirometria = $("#txt_esp_Espirometria");
    const txt_esp_EspirometriaObservaciones = $("#txt_esp_EspirometriaObservaciones");
    //#endregion
    //#region AUDIOMETRÍA
    const txt_CargarAudiometria = $('#txt_CargarAudiometria');
    const txt_aud_HipoacusiaOD = $("#txt_aud_HipoacusiaOD");
    const txt_aud_HipoacusiaOI = $("#txt_aud_HipoacusiaOI");
    const txt_aud_HBC = $("#txt_aud_HBC");
    const txt_aud_CargarAudiometria = $("#txt_aud_CargarAudiometria");
    const cbo_aud_Diagnostico = $("#cbo_aud_Diagnostico");
    const txt_aud_KH1 = $("#txt_aud_KH1");
    const txt_aud_KH1_OI = $("#txt_aud_KH1_OI");
    const txt_aud_KH1_OD = $("#txt_aud_KH1_OD");
    const txt_aud_KH2 = $("#txt_aud_KH2");
    const txt_aud_KH2_OI = $("#txt_aud_KH2_OI");
    const txt_aud_KH2_OD = $("#txt_aud_KH2_OD");
    const txt_aud_KH3 = $("#txt_aud_KH3");
    const txt_aud_KH3_OI = $("#txt_aud_KH3_OI");
    const txt_aud_KH3_OD = $("#txt_aud_KH3_OD");
    const txt_aud_KH4 = $("#txt_aud_KH4");
    const txt_aud_KH4_OI = $("#txt_aud_KH4_OI");
    const txt_aud_KH4_OD = $("#txt_aud_KH4_OD");
    const txt_aud_KH5 = $("#txt_aud_KH5");
    const txt_aud_KH5_OI = $("#txt_aud_KH5_OI");
    const txt_aud_KH5_OD = $("#txt_aud_KH5_OD");
    const txt_aud_KH6 = $("#txt_aud_KH6");
    const txt_aud_KH6_OI = $("#txt_aud_KH6_OI");
    const txt_aud_KH6_OD = $("#txt_aud_KH6_OD");
    const txt_aud_KH7 = $("#txt_aud_KH7");
    const txt_aud_KH7_OI = $("#txt_aud_KH7_OI");
    const txt_aud_KH7_OD = $("#txt_aud_KH7_OD");
    const txt_aud_NotasAudiometria = $("#txt_aud_NotasAudiometria");
    //#endregion
    //#region ELECTROCARDIOGRAMA 12 DERIVACIONES
    const txt_eleDer_CargarElectrocardiograma = $("#txt_eleDer_CargarElectrocardiograma");
    const txt_eleDer_Interpretacion = $("#txt_eleDer_Interpretacion");
    //#endregion
    //#region RADIOGRAFIAS - TORAX Y COLUMNA LUMBAR DOS POSICIONES
    const txt_CargarRadiografia_radTCLP_Conclusiones = $("#txt_CargarRadiografia_radTCLP_Conclusiones");
    const txt_radTCLP_Conclusiones = $("#txt_radTCLP_Conclusiones");
    //#endregion
    //#region LABORATORIO
    const txt_CargarDocLaboratorio = $('#txt_CargarDocLaboratorio');
    //#endregion
    //#region CERTIFICADO MEDICO
    const txt_certMed_CertificadoMedico = $("#txt_certMed_CertificadoMedico");
    const cbo_certMed_AptitudID = $("#cbo_certMed_AptitudID");
    const txt_certMed_Fecha = $("#txt_certMed_Fecha");
    const txt_certMed_NombrePaciente = $("#txt_certMed_NombrePaciente");
    //#endregion
    //#region RECOMENDACIÓN
    const txt_recom_Recomendaciones = $('#txt_recom_Recomendaciones');
    //#endregion
    //#region DOCUMENTOS ADJUNTOS
    const txt_CargarDocumentosAdjuntos = $('#txt_CargarDocumentosAdjuntos');
    //#endregion
    //#region RADIOGRAFIAS
    const txt_CargarRadiografias = $('#txt_CargarRadiografias');
    //#endregion
    //#endregion

    //#region CONST OBSERVACIONES MEDICO INTERNO CP
    const mdlObservacionesCP = $('#mdlObservacionesCP');
    const cbo_cp_aptitudIDCP = $('#cbo_cp_aptitudIDCP');
    const txt_cp_observacionMedicoCP = $('#txt_cp_observacionMedicoCP');
    const btnCEObservacionMedicoCP = $('#btnCEObservacionMedicoCP');
    const txt_cp_archivosCP = $('#txt_cp_archivosCP');
    const btnDescargarDocumentoMedicoInterno = $('#btnDescargarDocumentoMedicoInterno');
    //#endregion

    //#region CONST CARGAR DOCUMENTO FIRMADO POR MEDICO EXTERNO
    const mdlShowMdlHCFirmado = $('#mdlShowMdlHCFirmado');
    const txt_hc_firmado_doctorExterno = $('#txt_hc_firmado_doctorExterno');
    const btnCargarDocumentoFirmadoMedicoExterno = $('#btnCargarDocumentoFirmadoMedicoExterno');
    const btnDescargarHCFirmado = $('#btnDescargarHCFirmado');
    //#endregion

    //#region CONST FECHA ACTUAL
    const fechaActual = new Date();
    const fechaInicioAnio = new Date(new Date().getFullYear(), 0, 1);
    const yyyy = fechaActual.getFullYear();
    const mm = fechaActual.getMonth() + 1;
    const dd = fechaActual.getDate();
    const fechaActualInputDate = `${yyyy}/${mm}/${dd}`;
    //#endregion

    //#region CONST DIV PRINCIPAL
    const btnGuardarParcial = $('#btnGuardarParcial');
    const btnShowCEHistorialClinico = $('#btnShowCEHistorialClinico');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const tblS_SO_HistorialesClinicos = $('#tblS_SO_HistorialesClinicos');
    const divtblS_SO_HistorialesClinicos = $('#divtblS_SO_HistorialesClinicos');
    let dtHistorialClinico;
    //#endregion

    //#region CONST DIV CREAR/EDITAR HISTORIAL CLINICO
    const divCEHistorialClinico = $('#divCEHistorialClinico');
    const btnCECancelar = $('#btnCECancelar');
    //#endregion

    //#region CONST BOTONES FOOTER
    const btnCEHistorialClinico = $('#btnCEHistorialClinico');
    const btnCECancelarHistorialClinico = $('#btnCECancelarHistorialClinico');
    //#endregion
    const idEmpresa = $('#idEmpresa');

    HistorialClinico = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT DATATABLE
            initTblHistorialClinico();
            //#endregion

            //#region EVENTOS HISTORIALES CLINICOS
            fncShowCtrlsPrincipales();

            btnCEHistorialClinico.on("click", function () {
                fncLimpiarFormulario();
                btnCECancelar.trigger("click");
            });

            btnShowCEHistorialClinico.on("click", function () {
                fncLimpiarFormulario();
                fncGetUltimoFolioHistorialClinico();
                fncHideCtrlsPrincipales();
            });

            btnCECancelar.on("click", function () {
                fncShowCtrlsPrincipales();
                fncLimpiarFormulario();
                fncGetHistorialesClinicos();
            });

            btnCECancelarHistorialClinico.on("click", function () {
                fncLimpiarFormulario();
                btnCECancelar.trigger("click");
            });

            btnFiltroBuscar.on("click", function () {
                fncGetHistorialesClinicos();
            });

            btnGuardarParcial.on("click", function () {
                fncCEHistorialClinico(true);
            });

            fncFillCbos();

            //#region DATOS PERSONALES
            txt_dtsPer_FechaHora.val(moment(fechaActualInputDate).format("YYYY-MM-DD"));
            txt_dtsPer_FechaNac.val(moment(fechaActualInputDate).format("YYYY-MM-DD"));

            btnGetEdadPaciente.on("click", function () {
                fncGetEdadPaciente();
            });

            txt_dtsPer_Paciente.on("keyup", function () {
                txt_certMed_NombrePaciente.val($(this).val());
            })
            //#endregion

            //#region ANTECEDENTES PERSONALES NO PATOLÓGICOS
            txt_antPerNoPat_FechaAntitenica.val(moment(fechaActualInputDate).format("YYYY-MM-DD"));
            txt_antPerNoPat_FechaInfluenza.val(moment(fechaActualInputDate).format("YYYY-MM-DD"));
            //#endregion

            //#region CERTIFICADO MEDICO
            txt_certMed_Fecha.val(moment(fechaActualInputDate).format("YYYY-MM-DD"));
            //#endregion

            //#region EXPLORACIÓN FÍSICA-SIGNOS VITALES
            txt_expFSV_Peso.on("keyup", function () {
                fncGetIMC($(this).val(), txt_expFSV_Talla.val());
            });

            txt_expFSV_Talla.on("keyup", function () {
                fncGetIMC(txt_expFSV_Peso.val(), $(this).val());
            });
            //#endregion

            fncGetHistorialesClinicos();
            //#endregion

            //#region EVENTOS OBSERVACIONES MEDICO INTERIOR
            btnCEObservacionMedicoCP.on("click", function () {
                fncEditarObservacionMedicoInternoCP($(this).attr("data-id"));
            });

            btnDescargarDocumentoMedicoInterno.on("click", function () {
                fncGetRutaArchivo(btnCEObservacionMedicoCP.attr("data-id"), 2);
            });
            //#endregion

            //#region EVENTOS CARGAR DOCUMENTO FIRMADO MEDICO EXTERNO
            btnCargarDocumentoFirmadoMedicoExterno.on("click", function () {
                fncCargarDocumentoFirmadoMedicoExterno($(this).attr("data-id"));
            });

            btnDescargarHCFirmado.on("click", function () {
                fncGetRutaArchivo(btnCargarDocumentoFirmadoMedicoExterno.attr("data-id"), 3);
            });
            //#endregion
        }

        //#region CRUD HISTORIAL CLINICO
        function fncGetIMC(peso, altura) {
            let imc = 0;
            if (peso > 0 && altura > 0) {
                imc = (parseFloat(peso) / (parseFloat(altura) * 2));
            }
            txt_expFSV_IMC.val(parseFloat(imc).toFixed(2));
        }

        function fncGetHistorialesClinicos() {
            axios.post("GetHistorialesClinicos").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtHistorialClinico.clear();
                    dtHistorialClinico.rows.add(response.data.lstHC);
                    dtHistorialClinico.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTblHistorialClinico() {
            dtHistorialClinico = tblS_SO_HistorialesClinicos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'dtsPer_Paciente', title: 'Paciente' },
                    { data: 'dtsPer_CURP', title: (idEmpresa.val() == 6 ? 'DNI' : 'CURP') },
                    { data: 'cp_StrEsVoBo', title: 'Estatus' },
                    {
                        title: 'HC firmado',
                        render: function (data, type, row, meta) {
                            return `<button class="btn btn-primary showMdlHCFirmado" title="Historial clinico firmado."><i class="fas fa-book-open"></i></button>`;
                        },
                    },
                    {
                        title: 'Observaciones',
                        render: function (data, type, row, meta) {
                            return `<button class="btn btn-primary showMdlObservacionCP" title="Observación."><i class="fas fa-book-open"></i></button>`;
                        },
                    },
                    {
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class="btn btn-warning editarHistorialClinico" title="Editar historial clinico."><i class="fas fa-pencil-alt"></i></button>&nbsp;`;
                            let btnEliminar = `<button class="btn btn-danger eliminarHistorialClinico" title="Eliminar historial clinico."><i class="fas fa-trash"></i></button>&nbsp;`;
                            let btnExportar = `<button class="btn btn-primary exportarHistorialClinico" title="Exportar historial clinico."><i class="fas fa-download"></i></button>`;

                            return btnEditar + btnEliminar + btnExportar;
                        },
                    },
                    { data: 'id', visible: false },
                ],
                initComplete: function (settings, json) {
                    tblS_SO_HistorialesClinicos.on('click', '.editarHistorialClinico', function () {
                        let rowData = dtHistorialClinico.row($(this).closest('tr')).data();
                        btnCEHistorialClinico.attr("data-id", rowData.id);

                        fncLimpiarFormulario();
                        fncHideCtrlsPrincipales();

                        //#region SE CONSULTA LOS DATOS COMPLETOS DEL HISTORIAL CLINICO SELECCIONADO
                        let obj = new Object();
                        obj = {
                            idHistorialClinico: rowData.id
                        }
                        axios.post("GetDatosActualizarHistorialClinico", obj).then(response => {
                            let { success, items, message } = response.data;
                            if (success) {
                                //#region DATOS PERSONALES
                                txt_dtsPer_Folio.val(response.data.objHC.folio);
                                txt_dtsPer_ImagenPersona.val(response.data.objHC.dtsPer_ImagenPersona);
                                cbo_dtsPer_EmpresaID.val(response.data.objHC.dtsPer_EmpresaID);
                                cbo_dtsPer_EmpresaID.trigger("change");
                                cbo_dtsPer_CCID.val(response.data.objHC.dtsPer_CCID);
                                cbo_dtsPer_CCID.trigger("change");
                                txt_dtsPer_Paciente.val(response.data.objHC.dtsPer_Paciente);
                                txt_dtsPer_FechaHora.val(moment(response.data.objHC.dtsPer_FechaHora).format("YYYY-MM-DD"));
                                txt_dtsPer_FechaNac.val(moment(response.data.objHC.dtsPer_FechaNac).format("YYYY-MM-DD"));
                                btnGetEdadPaciente.trigger("click");
                                txt_dtsPer_Edad.val(response.data.objHC.dtsPer_Edad);
                                cbo_dtsPer_Sexo.val(response.data.objHC.dtsPer_Sexo);
                                cbo_dtsPer_Sexo.trigger("change");
                                cbo_dtsPer_EstadoCivil.val(response.data.objHC.dtsPer_EstadoCivilID);
                                cbo_dtsPer_EstadoCivil.trigger("change");
                                cbo_dtsPer_TipoSanguineo.val(response.data.objHC.dtsPer_TipoSangreID);
                                cbo_dtsPer_TipoSanguineo.trigger("change");
                                txt_dtsPer_CURP.val(response.data.objHC.dtsPer_CURP);
                                txt_dtsPer_Domicilio.val(response.data.objHC.dtsPer_Domicilio);
                                txt_dtsPer_Ciudad.val(response.data.objHC.dtsPer_Ciudad);
                                cbo_dtsPer_Escolaridad.val(response.data.objHC.dtsPer_EscolaridadID);
                                cbo_dtsPer_Escolaridad.trigger("change");
                                txt_dtsPer_LugarNacimiento.val(response.data.objHC.dtsPer_LugarNac);
                                txt_dtsPer_Telefono.val(response.data.objHC.dtsPer_Telefono);
                                //#endregion
                                //#region MOTIVO DE LA EVALUACION
                                chk_motEva_esIngreso.prop("checked", response.data.objHC.motEva_esIngreso);
                                chk_motEva_esRetiro.prop("checked", response.data.objHC.motEva_esRetiro);
                                chk_motEva_esEvaOpcional.prop("checked", response.data.objHC.motEva_esEvaOpcional);
                                chk_motEva_esPostIncapacidad.prop("checked", response.data.objHC.motEva_esPostIncapacidad);
                                chk_motEva_esReubicacion.prop("checked", response.data.objHC.motEva_esReubicacion);
                                //#endregion
                                //#region ANTECEDENTES LABORALES
                                txt_antLab_Puesto.val(response.data.objHC.antLab_Puesto);
                                txt_antLab_Empresa.val(response.data.objHC.antLab_Empresa);
                                txt_antLab_Desde.val(response.data.objHC.antLab_Desde);
                                txt_antLab_Hasta.val(response.data.objHC.antLab_Hasta);
                                txt_antLab_Turno.val(response.data.objHC.antLab_Turno);
                                chk_antLab_esDePie.prop("checked", response.data.objHC.antLab_esDePie);
                                chk_antLab_esInclinado.prop("checked", response.data.objHC.antLab_esInclinado);
                                chk_antLab_esSentado.prop("checked", response.data.objHC.antLab_esSentado);
                                chk_antLab_esArrodillado.prop("checked", response.data.objHC.antLab_esArrodillado);
                                chk_antLab_esCaminando.prop("checked", response.data.objHC.antLab_esCaminando);
                                chk_antLab_esOtra.prop("checked", response.data.objHC.antLab_esOtra);
                                txt_antLab_Cual.val(response.data.objHC.antLab_Cual);
                                //#endregion
                                //#region ACCIDENTES Y ENFERMEDADES DE TRABAJO
                                txt_accET_Empresa.val(response.data.objHC.accET_Empresa);
                                txt_accET_Anio.val(response.data.objHC.accET_Anio);
                                txt_accET_LesionAreaAnatomica.val(response.data.objHC.accET_LesionAreaAnatomica);
                                txt_accET_Secuelas.val(response.data.objHC.accET_Secuelas);
                                txt_accET_Cuales.val(response.data.objHC.accET_Cuales);
                                txt_accET_ExamNoAceptables.val(response.data.objHC.accET_ExamNoAceptables);
                                txt_accET_Causas.val(response.data.objHC.accET_Causas);
                                txt_accET_AbandonoTrabajo.val(response.data.objHC.accET_AbandonoTrabajo);
                                txt_accET_IncapacidadFrecuente.val(response.data.objHC.accET_IncapacidadFrecuente);
                                txt_accET_Prolongadas.val(response.data.objHC.accET_Prolongadas);
                                //#endregion
                                //#region USO DE ELEMENTOS DE PROTECCIÓN PERSONAL
                                chk_usoElePP_esActual.prop("checked", response.data.objHC.usoElePP_esActual);
                                chk_usoElePP_esCasco.prop("checked", response.data.objHC.usoElePP_esCasco);
                                chk_usoElePP_esTapaboca.prop("checked", response.data.objHC.usoElePP_esTapaboca);
                                chk_usoElePP_esGafas.prop("checked", response.data.objHC.usoElePP_esGafas);
                                chk_usoElePP_esRespirador.prop("checked", response.data.objHC.usoElePP_esRespirador);
                                chk_usoElePP_esBotas.prop("checked", response.data.objHC.usoElePP_esBotas);
                                chk_usoElePP_esAuditivos.prop("checked", response.data.objHC.usoElePP_esAuditivos);
                                chk_usoElePP_esOverol.prop("checked", response.data.objHC.usoElePP_esOverol);
                                chk_usoElePP_esGuantes.prop("checked", response.data.objHC.usoElePP_esGuantes);
                                txt_usoElePP_OtroCual.val(response.data.objHC.usoElePP_OtroCual);
                                txt_usoElePP_DeberiaRecibir.val(response.data.objHC.usoElePP_DeberiaRecibir);
                                txt_usoElePP_ConsideraAdecuado.val(response.data.objHC.usoElePP_ConsideraAdecuado);
                                //#endregion
                                //#region ANTECEDENTES FAMILIARES
                                txt_antFam_esTuberculosis.val(response.data.objHC.antFam_esTuberculosis);
                                txt_antFam_TuberculosisParentesco.val(response.data.objHC.antFam_TuberculosisParentesco);
                                txt_antFam_esHTA.val(response.data.objHC.antFam_esHTA);
                                txt_antFam_HTAParentesco.val(response.data.objHC.antFam_HTAParentesco);
                                txt_antFam_esDiabetes.val(response.data.objHC.antFam_esDiabetes);
                                txt_antFam_DiabetesParentesco.val(response.data.objHC.antFam_DiabetesParentesco);
                                txt_antFam_esACV.val(response.data.objHC.antFam_esACV);
                                txt_antFam_ACVParentesco.val(response.data.objHC.antFam_ACVParentesco);
                                txt_antFam_esInfarto.val(response.data.objHC.antFam_esInfarto);
                                txt_antFam_InfartoParentesco.val(response.data.objHC.antFam_InfartoParentesco);
                                txt_antFam_esAsma.val(response.data.objHC.antFam_esAsma);
                                txt_antFam_AsmaParentesco.val(response.data.objHC.antFam_AsmaParentesco);
                                txt_antFam_esAlergias.val(response.data.objHC.antFam_esAlergias);
                                txt_antFam_AlergiasParentesco.val(response.data.objHC.antFam_AlergiasParentesco);
                                txt_antFam_esMental.val(response.data.objHC.antFam_esMental);
                                txt_antFam_MentalParentesco.val(response.data.objHC.antFam_MentalParentesco);
                                txt_antFam_esCancer.val(response.data.objHC.antFam_esCancer);
                                txt_antFam_CancerParentesco.val(response.data.objHC.antFam_CancerParentesco);
                                txt_antFam_Observaciones.val(response.data.objHC.antFam_Observaciones);
                                //#endregion
                                //#region ANTECEDENTES PERSONALES NO PATOLÓGICOS
                                cbo_antPerNoPat_Tabaquismo.val(response.data.objHC.antPerNoPat_Tabaquismo);
                                cbo_antPerNoPat_Tabaquismo.trigger("change");
                                txt_antPerNoPat_CigarroDia.val(response.data.objHC.antPerNoPat_CigarroDia);
                                txt_antPerNoPat_CigarroAnios.val(response.data.objHC.antPerNoPat_CigarroAnios);
                                cbo_antPerNoPat_Alcoholismo.val(response.data.objHC.antPerNoPat_Alcoholismo);
                                cbo_antPerNoPat_Alcoholismo.trigger("change");
                                txt_antPerNoPat_AlcoholismoAnios.val(response.data.objHC.antPerNoPat_AlcoholismoAnios);
                                chk_antPerNoPat_esDrogradiccion.prop("checked", response.data.objHC.antPerNoPat_esDrogadiccion);
                                chk_antPerNoPat_esMarihuana.prop("checked", response.data.objHC.antPerNoPat_esMarihuana);
                                chk_antPerNoPat_esCocaina.prop("checked", response.data.objHC.antPerNoPat_esCocaina);
                                chk_antPerNoPat_esAnfetaminas.prop("checked", response.data.objHC.antPerNoPat_esAnfetaminas);
                                txt_antPerNoPat_Otros.val(response.data.objHC.antPerNoPat_Otros);
                                txt_antPerNoPat_Inmunizaciones.val(response.data.objHC.antPerNoPat_Inmunizaciones);
                                txt_antPerNoPat_Tetanicos.val(response.data.objHC.antPerNoPat_Tetanicos);
                                txt_antPerNoPat_FechaAntitenica.val(moment(response.data.objHC.antPerNoPat_FechaAntitetanica).format("YYYY-MM-DD"));
                                txt_antPerNoPat_Hepatitis.val(response.data.objHC.antPerNoPat_Hepatitis);
                                txt_antPerNoPat_Influenza.val(response.data.objHC.antPerNoPat_Influenza);
                                txt_antPerNoPat_FechaInfluenza.val(moment(response.data.objHC.antPerNoPat_FechaInfluenza).format("YYYY-MM-DD"));
                                txt_antPerNoPat_Infancia.val(response.data.objHC.antPerNoPat_Infancia);
                                txt_antPerNoPat_DescInfancia.val(response.data.objHC.antPerNoPat_DescInfancia);
                                txt_antPerNoPat_Alimentacion.val(response.data.objHC.antPerNoPat_Alimentacion);
                                txt_antPerNoPat_Higiene.val(response.data.objHC.antPerNoPat_Higiene);
                                txt_antPerNoPat_MedicacionActual.val(response.data.objHC.antPerNoPat_MedicacionActual);
                                //#endregion
                                //#region ANTECEDENTES PERSONALES PATOLÓGICOS
                                chk_antPerPat_esNeoplasicos.prop("checked", response.data.objHC.antPerPat_esNeoplasicos);
                                chk_antPerPat_esNeumopatias.prop("checked", response.data.objHC.antPerPat_esNeumopatias);
                                chk_antPerPat_esAsma.prop("checked", response.data.objHC.antPerPat_esAsma);
                                chk_antPerPat_esFimico.prop("checked", response.data.objHC.antPerPat_esFimico);
                                chk_antPerPat_esNeumoconiosis.prop("checked", response.data.objHC.antPerPat_esNeumoconiosis);
                                chk_antPerPat_esCardiopatias.prop("checked", response.data.objHC.antPerPat_esCardiopatias);
                                chk_antPerPat_esReumaticos.prop("checked", response.data.objHC.antPerPat_esReumaticos);
                                chk_antPerPat_esAlergias.prop("checked", response.data.objHC.antPerPat_esAlergias);
                                chk_antPerPat_esHipertension.prop("checked", response.data.objHC.antPerPat_esHipertension);
                                chk_antPerPat_esHepatitis.prop("checked", response.data.objHC.antPerPat_esHepatitis);
                                chk_antPerPat_esTifoidea.prop("checked", response.data.objHC.antPerPat_esTifoidea);
                                chk_antPerPat_esHernias.prop("checked", response.data.objHC.antPerPat_esHernias);
                                chk_antPerPat_esLumbalgias.prop("checked", response.data.objHC.antPerPat_esLumbalgias);
                                chk_antPerPat_esDiabetes.prop("checked", response.data.objHC.antPerPat_esDiabetes);
                                chk_antPerPat_esEpilepsias.prop("checked", response.data.objHC.antPerPat_esEpilepsias);
                                chk_antPerPat_esVenereas.prop("checked", response.data.objHC.antPerPat_esVenereas);
                                chk_antPerPat_esCirugias.prop("checked", response.data.objHC.antPerPat_esCirugias);
                                chk_antPerPat_esFracturas.prop("checked", response.data.objHC.antPerPat_esFracturas);
                                txt_antPerPat_ObservacionesPat.val(response.data.objHC.antPerPat_ObservacionesPat);
                                //#endregion
                                //#region INTERROGATORIO POR APARATOS Y SISTEMAS
                                chk_intApaSis_esRespiratorio.prop("checked", response.data.objHC.intApaSis_esRespiratorio);
                                chk_intApaSis_esDigestivo.prop("checked", response.data.objHC.intApaSis_esDigestivo);
                                chk_intApaSis_esCardiovascular.prop("checked", response.data.objHC.intApaSis_esCardiovascular);
                                chk_intApaSis_esNervioso.prop("checked", response.data.objHC.intApaSis_esNervioso);
                                chk_intApaSis_esUrinario.prop("checked", response.data.objHC.intApaSis_esUrinario);
                                chk_intApaSis_esEndocrino.prop("checked", response.data.objHC.intApaSis_esEndocrino);
                                chk_intApaSis_esPsiquiatrico.prop("checked", response.data.objHC.intApaSis_esPsiquiatrico);
                                chk_intApaSis_esEsqueletico.prop("checked", response.data.objHC.intApaSis_esEsqueletico);
                                chk_intApaSis_esAudicion.prop("checked", response.data.objHC.intApaSis_esAudicion);
                                chk_intApaSis_esVision.prop("checked", response.data.objHC.intApaSis_esVision);
                                chk_intApaSis_esOlfato.prop("checked", response.data.objHC.intApaSis_esOlfato);
                                chk_intApaSis_esTacto.prop("checked", response.data.objHC.intApaSis_esTacto);
                                txt_intApaSis_ObservacionesPat.val(response.data.objHC.intApaSis_ObservacionesPat);
                                //#endregion
                                //#region PADECIMIENTOS ACTUALES
                                txt_padAct_PadActuales.val(response.data.objHC.padAct_PadActuales);
                                //#endregion
                                //#region EXPLORACIÓN FÍSICA-SIGNOS VITALES
                                txt_expFSV_TArterial.val(response.data.objHC.expFSV_TArterial);
                                txt_expFSV_Pulso.val(response.data.objHC.expFSV_Pulso);
                                txt_expFSV_Temp.val(response.data.objHC.expFSV_Temp);
                                txt_expFSV_FCardiaca.val(response.data.objHC.expFSV_FCardiaca);
                                txt_expFSV_FResp.val(response.data.objHC.expFSV_FResp);
                                txt_expFSV_Peso.val(response.data.objHC.expFSV_Peso);
                                txt_expFSV_Talla.val(response.data.objHC.expFSV_Talla);
                                txt_expFSV_IMC.val(response.data.objHC.expFSV_IMC);
                                //#endregion
                                //#region EXPLORACIÓN FÍSICA-CABEZA
                                $("#cbo_expFC_Craneo").val(response.data.objHC.expFC_Craneo);
                                $("#cbo_expFC_Craneo").trigger("change");
                                $("#cbo_expFC_Parpados").val(response.data.objHC.expFC_Parpados);
                                $("#cbo_expFC_Parpados").trigger("change");
                                $("#cbo_expFC_Conjutiva").val(response.data.objHC.expFC_Conjutiva);
                                $("#cbo_expFC_Conjutiva").trigger("change");
                                $("#cbo_expFC_Reflejos").val(response.data.objHC.expFC_Reflejos);
                                $("#cbo_expFC_Reflejos").trigger("change");
                                $("#cbo_expFC_FosasNasales").val(response.data.objHC.expFC_FosasNasales);
                                $("#cbo_expFC_FosasNasales").trigger("change");
                                $("#cbo_expFC_Boca").val(response.data.objHC.expFC_Boca);
                                $("#cbo_expFC_Boca").trigger("change");
                                $("#cbo_expFC_Amigdalas").val(response.data.objHC.expFC_Amigdalas);
                                $("#cbo_expFC_Amigdalas").trigger("change");
                                $("#cbo_expFC_Dentadura").val(response.data.objHC.expFC_Dentadura);
                                $("#cbo_expFC_Dentadura").trigger("change");
                                $("#cbo_expFC_Encias").val(response.data.objHC.expFC_Encias);
                                $("#cbo_expFC_Encias").trigger("change");
                                $("#cbo_expFC_Cuello").val(response.data.objHC.expFC_Cuello);
                                $("#cbo_expFC_Cuello").trigger("change");
                                $("#cbo_expFC_Tiroides").val(response.data.objHC.expFC_Tiroides);
                                $("#cbo_expFC_Tiroides").trigger("change");
                                $("#cbo_expFC_Ganglios").val(response.data.objHC.expFC_Ganglios);
                                $("#cbo_expFC_Ganglios").trigger("change");
                                $("#cbo_expFC_Oidos").val(response.data.objHC.expFC_Oidos);
                                $("#cbo_expFC_Oidos").trigger("change");
                                $("#cbo_expFC_Otros").val(response.data.objHC.expFC_Otros);
                                $("#cbo_expFC_Otros").trigger("change");
                                $("#txt_expFC_Observaciones").val(response.data.objHC.expFC_Observaciones);
                                //#endregion
                                //#region EXPLORACIÓN FÍSICA-AGUDEZA VISUAL
                                txt_expFAV_VisCerAmbosOjos.val(response.data.objHC.expFAV_VisCerAmbosOjos);
                                txt_expFAV_VisCerOjoIzq.val(response.data.objHC.expFAV_VisCerOjoIzq);
                                txt_expFAV_VisCerOjoDer.val(response.data.objHC.expFAV_VisCerOjoDer);
                                txt_expFAV_VisLejAmbosOjos.val(response.data.objHC.expFAV_VisLejAmbosOjos);
                                txt_expFAV_VisLejOjoIzq.val(response.data.objHC.expFAV_VisLejOjoIzq);
                                txt_expFAV_VisLejOjoDer.val(response.data.objHC.expFAV_VisLejOjoDer);
                                txt_expFAV_CorregidaAmbosOjos.val(response.data.objHC.expFAV_CorregidaAmbosOjos);
                                txt_expFAV_CorregidaOjoIzq.val(response.data.objHC.expFAV_CorregidaOjoIzq);
                                txt_expFAV_CorregidaOjoDer.val(response.data.objHC.expFAV_CorregidaOjoDer);
                                txt_expFAV_CampimetriaOI.val(response.data.objHC.expFAV_CampimetriaOI);
                                txt_expFAV_CampimetriaOD.val(response.data.objHC.expFAV_CampimetriaOD);
                                txt_expFAV_PterigionOI.val(response.data.objHC.expFAV_PterigionOI);
                                txt_expFAV_PterigionOD.val(response.data.objHC.expFAV_PterigionOD);
                                txt_expFAV_FondoOjo.val(response.data.objHC.expFAV_FondoOjo);
                                txt_expFAV_Daltonismo.val(response.data.objHC.expFAV_Daltonismo);
                                txt_expFAV_Observaciones.val(response.data.objHC.expFAV_Observaciones);
                                //#endregion
                                //#region EXPLORACIÓN FÍSICA-TORAX, ABDOMEN, TRONCO Y EXTREMIDADES
                                chk_expFTATE_esCamposPulmonares.prop("checked", response.data.objHC.expFTATE_esCamposPulmonares);
                                chk_expFTATE_esCamposPulmonares.trigger("change");
                                chk_expFTATE_esPuntosDolorosos.prop("checked", response.data.objHC.expFTATE_esPuntosDolorosos);
                                chk_expFTATE_esPuntosDolorosos.trigger("change");
                                chk_expFTATE_esGenitales.prop("checked", response.data.objHC.expFTATE_esGenitales);
                                chk_expFTATE_esGenitales.trigger("change");
                                chk_expFTATE_esRuidosCardiacos.prop("checked", response.data.objHC.expFTATE_esRuidosCardiacos);
                                chk_expFTATE_esRuidosCardiacos.trigger("change");
                                chk_expFTATE_esHallusValgus.prop("checked", response.data.objHC.expFTATE_esHallusValgus);
                                chk_expFTATE_esHallusValgus.trigger("change");
                                chk_expFTATE_esHerniasUmbili.prop("checked", response.data.objHC.expFTATE_esHerniasUmbili);
                                chk_expFTATE_esHerniasUmbili.trigger("change");
                                chk_expFTATE_esAreaRenal.prop("checked", response.data.objHC.expFTATE_esAreaRenal);
                                chk_expFTATE_esAreaRenal.trigger("change");
                                chk_expFTATE_esVaricocele.prop("checked", response.data.objHC.expFTATE_esVaricocele);
                                chk_expFTATE_esVaricocele.trigger("change");
                                chk_expFTATE_esGrandulasMamarias.prop("checked", response.data.objHC.expFTATE_esGrandulasMamarias);
                                chk_expFTATE_esGrandulasMamarias.trigger("change");
                                chk_expFTATE_esColumnaVertebral.prop("checked", response.data.objHC.expFTATE_esColumnaVertebral);
                                chk_expFTATE_esColumnaVertebral.trigger("change");
                                chk_expFTATE_esPiePlano.prop("checked", response.data.objHC.expFTATE_esPiePlano);
                                chk_expFTATE_esPiePlano.trigger("change");
                                chk_expFTATE_esVarices.prop("checked", response.data.objHC.expFTATE_esVarices);
                                chk_expFTATE_esVarices.trigger("change");
                                chk_expFTATE_esMiembrosSup.prop("checked", response.data.objHC.expFTATE_esMiembrosSup);
                                chk_expFTATE_esMiembrosSup.trigger("change");
                                chk_expFTATE_esParedAbdominal.prop("checked", response.data.objHC.expFTATE_esParedAbdominal);
                                chk_expFTATE_esParedAbdominal.trigger("change");
                                chk_expFTATE_esAnillosInguinales.prop("checked", response.data.objHC.expFTATE_esAnillosInguinales);
                                chk_expFTATE_esAnillosInguinales.trigger("change");
                                chk_expFTATE_esMiembrosInf.prop("checked", response.data.objHC.expFTATE_esMiembrosInf);
                                chk_expFTATE_esMiembrosInf.trigger("change");
                                chk_expFTATE_esTatuajes.prop("checked", response.data.objHC.expFTATE_esTatuajes);
                                chk_expFTATE_esTatuajes.trigger("change");
                                chk_expFTATE_esVisceromegalias.prop("checked", response.data.objHC.expFTATE_esVisceromegalias);
                                chk_expFTATE_esVisceromegalias.trigger("change");
                                chk_expFTATE_esMarcha.prop("checked", response.data.objHC.expFTATE_esMarcha);
                                chk_expFTATE_esMarcha.trigger("change");
                                chk_expFTATE_esHerniasInguinales.prop("checked", response.data.objHC.expFTATE_esHerniasInguinales);
                                chk_expFTATE_esHerniasInguinales.trigger("change");
                                chk_expFTATE_esHombrosDolorosos.prop("checked", response.data.objHC.expFTATE_esHombrosDolorosos);
                                chk_expFTATE_esHombrosDolorosos.trigger("change");
                                chk_expFTATE_esQuistes.prop("checked", response.data.objHC.expFTATE_esQuistes);
                                chk_expFTATE_esQuistes.trigger("change");
                                txt_expFTATE_Observaciones.val(response.data.objHC.expFTATE_Observaciones);
                                chk_expFTATE_MS_HombroDer_esFlexion.prop("checked", response.data.objHC.expFTATE_MS_HombroDer_esFlexion);
                                chk_expFTATE_MS_HombroDer_esFlexion.trigger("change");
                                chk_expFTATE_MS_HombroIzq_esFlexion.prop("checked", response.data.objHC.expFTATE_MS_HombroIzq_esFlexion);
                                chk_expFTATE_MS_HombroIzq_esFlexion.trigger("change");
                                chk_expFTATE_MS_CodoDer_esFlexion.prop("checked", response.data.objHC.expFTATE_MS_CodoDer_esFlexion);
                                chk_expFTATE_MS_CodoDer_esFlexion.trigger("change");
                                chk_expFTATE_MS_CodoIzq_esFlexion.prop("checked", response.data.objHC.expFTATE_MS_CodoIzq_esFlexion);
                                chk_expFTATE_MS_CodoIzq_esFlexion.trigger("change");
                                chk_expFTATE_MS_MunecaDer_esFlexion.prop("checked", response.data.objHC.expFTATE_MS_MunecaDer_esFlexion);
                                chk_expFTATE_MS_MunecaDer_esFlexion.trigger("change");
                                chk_expFTATE_MS_MunecaIzq_esFlexion.prop("checked", response.data.objHC.expFTATE_MS_MunecaIzq_esFlexion);
                                chk_expFTATE_MS_MunecaIzq_esFlexion.trigger("change");
                                chk_expFTATE_MS_DedosDer_esFlexion.prop("checked", response.data.objHC.expFTATE_MS_DedosDer_esFlexion);
                                chk_expFTATE_MS_DedosDer_esFlexion.trigger("change");
                                chk_expFTATE_MS_DedosIzq_esFlexion.prop("checked", response.data.objHC.expFTATE_MS_DedosIzq_esFlexion);
                                chk_expFTATE_MS_DedosIzq_esFlexion.trigger("change");
                                chk_expFTATE_MS_HombroDer_esExtension.prop("checked", response.data.objHC.expFTATE_MS_HombroDer_esExtension);
                                chk_expFTATE_MS_HombroDer_esExtension.trigger("change");
                                chk_expFTATE_MS_HombroIzq_esExtension.prop("checked", response.data.objHC.expFTATE_MS_HombroIzq_esExtension);
                                chk_expFTATE_MS_HombroIzq_esExtension.trigger("change");
                                chk_expFTATE_MS_CodoDer_esExtension.prop("checked", response.data.objHC.expFTATE_MS_CodoDer_esExtension);
                                chk_expFTATE_MS_CodoDer_esExtension.trigger("change");
                                chk_expFTATE_MS_CodoIzq_esExtension.prop("checked", response.data.objHC.expFTATE_MS_CodoIzq_esExtension);
                                chk_expFTATE_MS_CodoIzq_esExtension.trigger("change");
                                chk_expFTATE_MS_MunecaDer_esExtension.prop("checked", response.data.objHC.expFTATE_MS_MunecaDer_esExtension);
                                chk_expFTATE_MS_MunecaDer_esExtension.trigger("change");
                                chk_expFTATE_MS_MunecaIzq_esExtension.prop("checked", response.data.objHC.expFTATE_MS_MunecaIzq_esExtension);
                                chk_expFTATE_MS_MunecaIzq_esExtension.trigger("change");
                                chk_expFTATE_MS_DedosDer_esExtension.prop("checked", response.data.objHC.expFTATE_MS_DedosDer_esExtension);
                                chk_expFTATE_MS_DedosDer_esExtension.trigger("change");
                                chk_expFTATE_MS_DedosIzq_esExtension.prop("checked", response.data.objHC.expFTATE_MS_DedosIzq_esExtension);
                                chk_expFTATE_MS_DedosIzq_esExtension.trigger("change");
                                chk_expFTATE_MS_HombroDer_esAbduccion.prop("checked", response.data.objHC.expFTATE_MS_HombroDer_esAbduccion);
                                chk_expFTATE_MS_HombroDer_esAbduccion.trigger("change");
                                chk_expFTATE_MS_HombroIzq_esAbduccion.prop("checked", response.data.objHC.expFTATE_MS_HombroIzq_esAbduccion);
                                chk_expFTATE_MS_HombroIzq_esAbduccion.trigger("change");
                                chk_expFTATE_MS_CodoDer_esAbduccion.prop("checked", response.data.objHC.expFTATE_MS_CodoDer_esAbduccion);
                                chk_expFTATE_MS_CodoDer_esAbduccion.trigger("change");
                                chk_expFTATE_MS_CodoIzq_esAbduccion.prop("checked", response.data.objHC.expFTATE_MS_CodoIzq_esAbduccion);
                                chk_expFTATE_MS_CodoIzq_esAbduccion.trigger("change");
                                chk_expFTATE_MS_MunecaDer_esAbduccion.prop("checked", response.data.objHC.expFTATE_MS_MunecaDer_esAbduccion);
                                chk_expFTATE_MS_MunecaDer_esAbduccion.trigger("change");
                                chk_expFTATE_MS_MunecaIzq_esAbduccion.prop("checked", response.data.objHC.expFTATE_MS_MunecaIzq_esAbduccion);
                                chk_expFTATE_MS_MunecaIzq_esAbduccion.trigger("change");
                                chk_expFTATE_MS_DedosDer_esAbduccion.prop("checked", response.data.objHC.expFTATE_MS_DedosDer_esAbduccion);
                                chk_expFTATE_MS_DedosDer_esAbduccion.trigger("change");
                                chk_expFTATE_MS_DedosIzq_esAbduccion.prop("checked", response.data.objHC.expFTATE_MS_DedosIzq_esAbduccion);
                                chk_expFTATE_MS_DedosIzq_esAbduccion.trigger("change");
                                chk_expFTATE_MS_HombroDer_esAduccion.prop("checked", response.data.objHC.expFTATE_MS_HombroDer_esAduccion);
                                chk_expFTATE_MS_HombroDer_esAduccion.trigger("change");
                                chk_expFTATE_MS_HombroIzq_esAduccion.prop("checked", response.data.objHC.expFTATE_MS_HombroIzq_esAduccion);
                                chk_expFTATE_MS_HombroIzq_esAduccion.trigger("change");
                                chk_expFTATE_MS_MunecaDer_esAduccion.prop("checked", response.data.objHC.expFTATE_MS_MunecaDer_esAduccion);
                                chk_expFTATE_MS_MunecaDer_esAduccion.trigger("change");
                                chk_expFTATE_MS_MunecaIzq_esAduccion.prop("checked", response.data.objHC.expFTATE_MS_MunecaIzq_esAduccion);
                                chk_expFTATE_MS_MunecaIzq_esAduccion.trigger("change");
                                chk_expFTATE_MS_DedosDer_esAduccion.prop("checked", response.data.objHC.expFTATE_MS_DedosDer_esAduccion);
                                chk_expFTATE_MS_DedosDer_esAduccion.trigger("change");
                                chk_expFTATE_MS_DedosIzq_esAduccion.prop("checked", response.data.objHC.expFTATE_MS_DedosIzq_esAduccion);
                                chk_expFTATE_MS_DedosIzq_esAduccion.trigger("change");
                                chk_expFTATE_MS_HombroDer_esRotInterna.prop("checked", response.data.objHC.expFTATE_MS_HombroDer_esRotInterna);
                                chk_expFTATE_MS_HombroDer_esRotInterna.trigger("change");
                                chk_expFTATE_MS_HombroIzq_esRotInterna.prop("checked", response.data.objHC.expFTATE_MS_HombroIzq_esRotInterna);
                                chk_expFTATE_MS_HombroIzq_esRotInterna.trigger("change");
                                chk_expFTATE_MS_MunecaDer_esRotInterna.prop("checked", response.data.objHC.expFTATE_MS_MunecaDer_esRotInterna);
                                chk_expFTATE_MS_MunecaDer_esRotInterna.trigger("change");
                                chk_expFTATE_MS_MunecaIzq_esRotInterna.prop("checked", response.data.objHC.expFTATE_MS_MunecaIzq_esRotInterna);
                                chk_expFTATE_MS_MunecaIzq_esRotInterna.trigger("change");
                                chk_expFTATE_MS_DedosDer_esRotInterna.prop("checked", response.data.objHC.expFTATE_MS_DedosDer_esRotInterna);
                                chk_expFTATE_MS_DedosDer_esRotInterna.trigger("change");
                                chk_expFTATE_MS_DedosIzq_esRotInterna.prop("checked", response.data.objHC.expFTATE_MS_DedosIzq_esRotInterna);
                                chk_expFTATE_MS_DedosIzq_esRotInterna.trigger("change");
                                chk_expFTATE_MS_HombroDer_esRotExterna.prop("checked", response.data.objHC.expFTATE_MS_HombroDer_esRotExterna);
                                chk_expFTATE_MS_HombroDer_esRotExterna.trigger("change");
                                chk_expFTATE_MS_HombroIzq_esRotExterna.prop("checked", response.data.objHC.expFTATE_MS_HombroIzq_esRotExterna);
                                chk_expFTATE_MS_HombroIzq_esRotExterna.trigger("change");
                                chk_expFTATE_MS_MunecaDer_esRotExterna.prop("checked", response.data.objHC.expFTATE_MS_MunecaDer_esRotExterna);
                                chk_expFTATE_MS_MunecaDer_esRotExterna.trigger("change");
                                chk_expFTATE_MS_MunecaIzq_esRotExterna.prop("checked", response.data.objHC.expFTATE_MS_MunecaIzq_esRotExterna);
                                chk_expFTATE_MS_MunecaIzq_esRotExterna.trigger("change");
                                chk_expFTATE_MS_DedosDer_esRotExterna.prop("checked", response.data.objHC.expFTATE_MS_DedosDer_esRotExterna);
                                chk_expFTATE_MS_DedosDer_esRotExterna.trigger("change");
                                chk_expFTATE_MS_DedosIzq_esRotExterna.prop("checked", response.data.objHC.expFTATE_MS_DedosIzq_esRotExterna);
                                chk_expFTATE_MS_DedosIzq_esRotExterna.trigger("change");
                                chk_expFTATE_MS_CodoDer_esPronacion.prop("checked", response.data.objHC.expFTATE_MS_CodoDer_esPronacion);
                                chk_expFTATE_MS_CodoDer_esPronacion.trigger("change");
                                chk_expFTATE_MS_CodoIzq_esPronacion.prop("checked", response.data.objHC.expFTATE_MS_CodoIzq_esPronacion);
                                chk_expFTATE_MS_CodoIzq_esPronacion.trigger("change");
                                chk_expFTATE_MS_MunecaDer_esPronacion.prop("checked", response.data.objHC.expFTATE_MS_MunecaDer_esPronacion);
                                chk_expFTATE_MS_MunecaDer_esPronacion.trigger("change");
                                chk_expFTATE_MS_MunecaIzq_esPronacion.prop("checked", response.data.objHC.expFTATE_MS_MunecaIzq_esPronacion);
                                chk_expFTATE_MS_MunecaIzq_esPronacion.trigger("change");
                                chk_expFTATE_MS_CodoDer_esSupinacion.prop("checked", response.data.objHC.expFTATE_MS_CodoDer_esSupinacion);
                                chk_expFTATE_MS_CodoDer_esSupinacion.trigger("change");
                                chk_expFTATE_MS_CodoIzq_esSupinacion.prop("checked", response.data.objHC.expFTATE_MS_CodoIzq_esSupinacion);
                                chk_expFTATE_MS_CodoIzq_esSupinacion.trigger("change");
                                chk_expFTATE_MS_MunecaDer_esSupinacion.prop("checked", response.data.objHC.expFTATE_MS_MunecaDer_esSupinacion);
                                chk_expFTATE_MS_MunecaDer_esSupinacion.trigger("change");
                                chk_expFTATE_MS_MunecaIzq_esSupinacion.prop("checked", response.data.objHC.expFTATE_MS_MunecaIzq_esSupinacion);
                                chk_expFTATE_MS_MunecaIzq_esSupinacion.trigger("change");
                                chk_expFTATE_MS_MunecaDer_esDesvUlnar.prop("checked", response.data.objHC.expFTATE_MS_MunecaDer_esDesvUlnar);
                                chk_expFTATE_MS_MunecaDer_esDesvUlnar.trigger("change");
                                chk_expFTATE_MS_MunecaIzq_esDesvUlnar.prop("checked", response.data.objHC.expFTATE_MS_MunecaIzq_esDesvUlnar);
                                chk_expFTATE_MS_MunecaIzq_esDesvUlnar.trigger("change");
                                chk_expFTATE_MS_MunecaDer_esDesvRadial.prop("checked", response.data.objHC.expFTATE_MS_MunecaDer_esDesvRadial);
                                chk_expFTATE_MS_MunecaDer_esDesvRadial.trigger("change");
                                chk_expFTATE_MS_MunecaIzq_esDesvRadial.prop("checked", response.data.objHC.expFTATE_MS_MunecaIzq_esDesvRadial);
                                chk_expFTATE_MS_MunecaIzq_esDesvRadial.trigger("change");
                                chk_expFTATE_MS_MunecaDer_esOponencia.prop("checked", response.data.objHC.expFTATE_MS_MunecaDer_esOponencia);
                                chk_expFTATE_MS_MunecaDer_esOponencia.trigger("change");
                                chk_expFTATE_MS_MunecaIzq_esOponencia.prop("checked", response.data.objHC.expFTATE_MS_MunecaIzq_esOponencia);
                                chk_expFTATE_MS_MunecaIzq_esOponencia.trigger("change");
                                chk_expFTATE_MS_DedosDer_esOponencia.prop("checked", response.data.objHC.expFTATE_MS_DedosDer_esOponencia);
                                chk_expFTATE_MS_DedosDer_esOponencia.trigger("change");
                                chk_expFTATE_MS_DedosIzq_esOponencia.prop("checked", response.data.objHC.expFTATE_MS_DedosIzq_esOponencia);
                                chk_expFTATE_MS_DedosIzq_esOponencia.trigger("change");
                                chk_expFTATE_MI_CaderaDer_esFlexion.prop("checked", response.data.objHC.expFTATE_MI_CaderaDer_esFlexion);
                                chk_expFTATE_MI_CaderaDer_esFlexion.trigger("change");
                                chk_expFTATE_MI_CaderaIzq_esFlexion.prop("checked", response.data.objHC.expFTATE_MI_CaderaIzq_esFlexion);
                                chk_expFTATE_MI_CaderaIzq_esFlexion.trigger("change");
                                chk_expFTATE_MI_RodillasDer_esFlexion.prop("checked", response.data.objHC.expFTATE_MI_RodillasDer_esFlexion);
                                chk_expFTATE_MI_RodillasDer_esFlexion.trigger("change");
                                chk_expFTATE_MI_RodillasIzq_esFlexion.prop("checked", response.data.objHC.expFTATE_MI_RodillasIzq_esFlexion);
                                chk_expFTATE_MI_RodillasIzq_esFlexion.trigger("change");
                                chk_expFTATE_MI_CllPieDer_esFlexion.prop("checked", response.data.objHC.expFTATE_MI_CllPieDer_esFlexion);
                                chk_expFTATE_MI_CllPieDer_esFlexion.trigger("change");
                                chk_expFTATE_MI_CllPieIzq_esFlexion.prop("checked", response.data.objHC.expFTATE_MI_CllPieIzq_esFlexion);
                                chk_expFTATE_MI_CllPieIzq_esFlexion.trigger("change");
                                chk_expFTATE_MI_DedosDer_esFlexion.prop("checked", response.data.objHC.expFTATE_MI_DedosDer_esFlexion);
                                chk_expFTATE_MI_DedosDer_esFlexion.trigger("change");
                                chk_expFTATE_MI_DedosIzq_esFlexion.prop("checked", response.data.objHC.expFTATE_MI_DedosIzq_esFlexion);
                                chk_expFTATE_MI_DedosIzq_esFlexion.trigger("change");
                                chk_expFTATE_MI_CaderaDer_esExtension.prop("checked", response.data.objHC.expFTATE_MI_CaderaDer_esExtension);
                                chk_expFTATE_MI_CaderaDer_esExtension.trigger("change");
                                chk_expFTATE_MI_CaderaIzq_esExtension.prop("checked", response.data.objHC.expFTATE_MI_CaderaIzq_esExtension);
                                chk_expFTATE_MI_CaderaIzq_esExtension.trigger("change");
                                chk_expFTATE_MI_RodillasDer_esExtension.prop("checked", response.data.objHC.expFTATE_MI_RodillasDer_esExtension);
                                chk_expFTATE_MI_RodillasDer_esExtension.trigger("change");
                                chk_expFTATE_MI_RodillasIzq_esExtension.prop("checked", response.data.objHC.expFTATE_MI_RodillasIzq_esExtension);
                                chk_expFTATE_MI_RodillasIzq_esExtension.trigger("change");
                                chk_expFTATE_MI_CllPieDer_esExtension.prop("checked", response.data.objHC.expFTATE_MI_CllPieDer_esExtension);
                                chk_expFTATE_MI_CllPieDer_esExtension.trigger("change");
                                chk_expFTATE_MI_CllPieIzq_esExtension.prop("checked", response.data.objHC.expFTATE_MI_CllPieIzq_esExtension);
                                chk_expFTATE_MI_CllPieIzq_esExtension.trigger("change");
                                chk_expFTATE_MI_DedosDer_esExtension.prop("checked", response.data.objHC.expFTATE_MI_DedosDer_esExtension);
                                chk_expFTATE_MI_DedosDer_esExtension.trigger("change");
                                chk_expFTATE_MI_DedosIzq_esExtension.prop("checked", response.data.objHC.expFTATE_MI_DedosIzq_esExtension);
                                chk_expFTATE_MI_DedosIzq_esExtension.trigger("change");
                                chk_expFTATE_MI_CaderaDer_esAbduccion.prop("checked", response.data.objHC.expFTATE_MI_CaderaDer_esAbduccion);
                                chk_expFTATE_MI_CaderaDer_esAbduccion.trigger("change");
                                chk_expFTATE_MI_CaderaIzq_esAbduccion.prop("checked", response.data.objHC.expFTATE_MI_CaderaIzq_esAbduccion);
                                chk_expFTATE_MI_CaderaIzq_esAbduccion.trigger("change");
                                chk_expFTATE_MI_CllPieDer_esAbduccion.prop("checked", response.data.objHC.expFTATE_MI_CllPieDer_esAbduccion);
                                chk_expFTATE_MI_CllPieDer_esAbduccion.trigger("change");
                                chk_expFTATE_MI_CllPieIzq_esAbduccion.prop("checked", response.data.objHC.expFTATE_MI_CllPieIzq_esAbduccion);
                                chk_expFTATE_MI_CllPieIzq_esAbduccion.trigger("change");
                                chk_expFTATE_MI_DedosDer_esAbduccion.prop("checked", response.data.objHC.expFTATE_MI_DedosDer_esAbduccion);
                                chk_expFTATE_MI_DedosDer_esAbduccion.trigger("change");
                                chk_expFTATE_MI_DedosIzq_esAbduccion.prop("checked", response.data.objHC.expFTATE_MI_DedosIzq_esAbduccion);
                                chk_expFTATE_MI_DedosIzq_esAbduccion.trigger("change");
                                chk_expFTATE_MI_CaderaDer_esAduccion.prop("checked", response.data.objHC.expFTATE_MI_CaderaDer_esAduccion);
                                chk_expFTATE_MI_CaderaDer_esAduccion.trigger("change");
                                chk_expFTATE_MI_CaderaIzq_esAduccion.prop("checked", response.data.objHC.expFTATE_MI_CaderaIzq_esAduccion);
                                chk_expFTATE_MI_CaderaIzq_esAduccion.trigger("change");
                                chk_expFTATE_MI_CllPieDer_esAduccion.prop("checked", response.data.objHC.expFTATE_MI_CllPieDer_esAduccion);
                                chk_expFTATE_MI_CllPieDer_esAduccion.trigger("change");
                                chk_expFTATE_MI_CllPieIzq_esAduccion.prop("checked", response.data.objHC.expFTATE_MI_CllPieIzq_esAduccion);
                                chk_expFTATE_MI_CllPieIzq_esAduccion.trigger("change");
                                chk_expFTATE_MI_DedosDer_esAduccion.prop("checked", response.data.objHC.expFTATE_MI_DedosDer_esAduccion);
                                chk_expFTATE_MI_DedosDer_esAduccion.trigger("change");
                                chk_expFTATE_MI_DedosIzq_esAduccion.prop("checked", response.data.objHC.expFTATE_MI_DedosIzq_esAduccion);
                                chk_expFTATE_MI_DedosIzq_esAduccion.trigger("change");
                                chk_expFTATE_MI_CaderaDer_esRotInterna.prop("checked", response.data.objHC.expFTATE_MI_CaderaDer_esRotInterna);
                                chk_expFTATE_MI_CaderaDer_esRotInterna.trigger("change");
                                chk_expFTATE_MI_CaderaIzq_esRotInterna.prop("checked", response.data.objHC.expFTATE_MI_CaderaIzq_esRotInterna);
                                chk_expFTATE_MI_CaderaIzq_esRotInterna.trigger("change");
                                chk_expFTATE_MI_RodillasDer_esRotInterna.prop("checked", response.data.objHC.expFTATE_MI_RodillasDer_esRotInterna);
                                chk_expFTATE_MI_RodillasDer_esRotInterna.trigger("change");
                                chk_expFTATE_MI_RodillasIzq_esRotInterna.prop("checked", response.data.objHC.expFTATE_MI_RodillasIzq_esRotInterna);
                                chk_expFTATE_MI_RodillasIzq_esRotInterna.trigger("change");
                                chk_expFTATE_MI_CllPieDer_esRotInterna.prop("checked", response.data.objHC.expFTATE_MI_CllPieDer_esRotInterna);
                                chk_expFTATE_MI_CllPieDer_esRotInterna.trigger("change");
                                chk_expFTATE_MI_CllPieIzq_esRotInterna.prop("checked", response.data.objHC.expFTATE_MI_CllPieIzq_esRotInterna);
                                chk_expFTATE_MI_CllPieIzq_esRotInterna.trigger("change");
                                chk_expFTATE_MI_CaderaDer_esRotExterna.prop("checked", response.data.objHC.expFTATE_MI_CaderaDer_esRotExterna);
                                chk_expFTATE_MI_CaderaDer_esRotExterna.trigger("change");
                                chk_expFTATE_MI_CaderaIzq_esRotExterna.prop("checked", response.data.objHC.expFTATE_MI_CaderaIzq_esRotExterna);
                                chk_expFTATE_MI_CaderaIzq_esRotExterna.trigger("change");
                                chk_expFTATE_MI_RodillasDer_esRotExterna.prop("checked", response.data.objHC.expFTATE_MI_RodillasDer_esRotExterna);
                                chk_expFTATE_MI_RodillasDer_esRotExterna.trigger("change");
                                chk_expFTATE_MI_RodillasIzq_esRotExterna.prop("checked", response.data.objHC.expFTATE_MI_RodillasIzq_esRotExterna);
                                chk_expFTATE_MI_RodillasIzq_esRotExterna.trigger("change");
                                chk_expFTATE_MI_CllPieDer_esRotExterna.prop("checked", response.data.objHC.expFTATE_MI_CllPieDer_esRotExterna);
                                chk_expFTATE_MI_CllPieDer_esRotExterna.trigger("change");
                                chk_expFTATE_MI_CllPieIzq_esRotExterna.prop("checked", response.data.objHC.expFTATE_MI_CllPieIzq_esRotExterna);
                                chk_expFTATE_MI_CllPieIzq_esRotExterna.trigger("change");
                                chk_expFTATE_MI_CllPieDer_esInversion.prop("checked", response.data.objHC.expFTATE_MI_CllPieDer_esInversion);
                                chk_expFTATE_MI_CllPieDer_esInversion.trigger("change");
                                chk_expFTATE_MI_CllPieIzq_esInversion.prop("checked", response.data.objHC.expFTATE_MI_CllPieIzq_esInversion);
                                chk_expFTATE_MI_CllPieIzq_esInversion.trigger("change");
                                chk_expFTATE_MI_CllPieDer_esEversion.prop("checked", response.data.objHC.expFTATE_MI_CllPieDer_esEversion);
                                chk_expFTATE_MI_CllPieDer_esEversion.trigger("change");
                                chk_expFTATE_MI_CllPieIzq_esEversion.prop("checked", response.data.objHC.expFTATE_MI_CllPieIzq_esEversion);
                                chk_expFTATE_MI_CllPieIzq_esEversion.trigger("change");
                                txt_expFTATE_MS_MI_Observaciones.val(response.data.objHC.expFTATE_MS_MI_Observaciones);
                                //#endregion
                                //#region EXPLORACIÓN FÍSICA-DATOS GENERALES
                                txt_expFDT_PielMucosas.val(response.data.objHC.expFDT_PielMucosas);
                                txt_expFDT_EstadoPsiquiatrico.val(response.data.objHC.expFDT_EstadoPsiquiatrico);
                                txt_expFDT_ExamenNeurologico.val(response.data.objHC.expFDT_ExamenNeurologico);
                                txt_expFDT_FobiasActuales.val(response.data.objHC.expFDT_FobiasActuales);
                                txt_expFDT_Higiene.val(response.data.objHC.expFDT_Higiene);
                                txt_expFDT_ConstitucionFisica.val(response.data.objHC.expFDT_ConstitucionFisica);
                                txt_expFDT_Otros.val(response.data.objHC.expFDT_Otros);
                                txt_expFDT_Observaciones.val(response.data.objHC.expFDT_Observaciones);
                                //#endregion
                                //#region ESTUDIOS DE GABINETE
                                cbo_estGab_TipoSanguineoID.val(response.data.objHC.estGab_TipoSanguineoID);
                                cbo_estGab_TipoSanguineoID.trigger("change");
                                cbo_estGab_Antidoping.val(response.data.objHC.estGab_Antidoping);
                                cbo_estGab_Antidoping.trigger("change");
                                txt_estGab_Laboratorios.val(response.data.objHC.estGab_Laboratorios);
                                txt_estGab_ObservacionesGrupoRH.val(response.data.objHC.estGab_ObservacionesGrupoRH);
                                txt_estGab_ExamGenOrina.val(response.data.objHC.estGab_ExamGenOrina);
                                txt_estGab_ExamGenOrinaObservaciones.val(response.data.objHC.estGab_ExamGenOrinaObservaciones);
                                txt_estGab_Radiografias.val(response.data.objHC.estGab_Radiografias);
                                txt_estGab_RadiografiasObservaciones.val(response.data.objHC.estGab_RadiografiasObservaciones);
                                txt_estGab_Audiometria.val(response.data.objHC.estGab_Audiometria);
                                txt_estGab_HBC.val(response.data.objHC.estGab_HBC);
                                txt_estGab_AudiometriaObservaciones.val(response.data.objHC.estGab_AudiometriaObservaciones);
                                txt_estGab_Espirometria.val(response.data.objHC.estGab_Espirometria);
                                txt_estGab_EspirometriaObservaciones.val(response.data.objHC.estGab_EspirometriaObservaciones);
                                txt_estGab_Electrocardiograma.val(response.data.objHC.estGab_Electrocardiograma);
                                txt_estGab_ElectrocardiogramaObservaciones.val(response.data.objHC.estGab_ElectrocardiogramaObservaciones);
                                txt_estGab_FechaPrimeraDosis.val(response.data.objHC.estGab_FechaPrimeraDosis);
                                txt_estGab_FechaSegundaDosis.val(response.data.objHC.estGab_FechaSegundaDosis);
                                cbo_estGab_MarcaDosisID.val(response.data.objHC.estGab_MarcaDosisID);
                                cbo_estGab_MarcaDosisID.trigger("change");
                                txt_estGab_VacunacionObservaciones.val(response.data.objHC.estGab_VacunacionObservaciones);
                                txt_estGab_LstProblemas.val(response.data.objHC.estGab_LstProblemas);
                                txt_estGab_Recomendaciones.val(response.data.objHC.estGab_Recomendaciones);
                                //#endregion
                                //#region ESPIROMETRÍA
                                txt_esp_Espirometria.val(response.data.objHC.esp_Espirometria);
                                txt_esp_EspirometriaObservaciones.val(response.data.objHC.esp_EspirometriaObservaciones);
                                //#endregion
                                //#region AUDIOMETRÍA
                                txt_aud_HipoacusiaOD.val(response.data.objHC.aud_HipoacusiaOD);
                                txt_aud_HipoacusiaOI.val(response.data.objHC.aud_HipoacusiaOI);
                                txt_aud_HBC.val(response.data.objHC.aud_HBC);
                                txt_aud_CargarAudiometria.val(response.data.objHC.aud_CargarAudiometria);
                                cbo_aud_Diagnostico.val(response.data.objHC.aud_Diagnostico);
                                cbo_aud_Diagnostico.trigger("change");
                                txt_aud_KH1.val(response.data.objHC.aud_KH1);
                                txt_aud_KH1_OI.val(response.data.objHC.aud_KH1_OI);
                                txt_aud_KH1_OD.val(response.data.objHC.aud_KH1_OD);
                                txt_aud_KH2.val(response.data.objHC.aud_KH2);
                                txt_aud_KH2_OI.val(response.data.objHC.aud_KH2_OI);
                                txt_aud_KH2_OD.val(response.data.objHC.aud_KH2_OD);
                                txt_aud_KH3.val(response.data.objHC.aud_KH3);
                                txt_aud_KH3_OI.val(response.data.objHC.aud_KH3_OI);
                                txt_aud_KH3_OD.val(response.data.objHC.aud_KH3_OD);
                                txt_aud_KH4.val(response.data.objHC.aud_KH4);
                                txt_aud_KH4_OI.val(response.data.objHC.aud_KH4_OI);
                                txt_aud_KH4_OD.val(response.data.objHC.aud_KH4_OD);
                                txt_aud_KH5.val(response.data.objHC.aud_KH5);
                                txt_aud_KH5_OI.val(response.data.objHC.aud_KH5_OI);
                                txt_aud_KH5_OD.val(response.data.objHC.aud_KH5_OD);
                                txt_aud_KH6.val(response.data.objHC.aud_KH6);
                                txt_aud_KH6_OI.val(response.data.objHC.aud_KH6_OI);
                                txt_aud_KH6_OD.val(response.data.objHC.aud_KH6_OD);
                                txt_aud_KH7.val(response.data.objHC.aud_KH7);
                                txt_aud_KH7_OI.val(response.data.objHC.aud_KH7_OI);
                                txt_aud_KH7_OD.val(response.data.objHC.aud_KH7_OD);
                                txt_aud_NotasAudiometria.val(response.data.objHC.aud_NotasAudiometria);
                                //#endregion
                                //#region ELECTROCARDIOGRAMA 12 DERIVACIONES
                                txt_eleDer_Interpretacion.val(response.data.objHC.eleDer_Interpretacion);
                                //#endregion
                                //#region RADIOGRAFIAS - TORAX Y COLUMNA LUMBAR DOS POSICIONES
                                txt_radTCLP_Conclusiones.val(response.data.objHC.radTCLP_Conclusiones);
                                //#endregion
                                //#region CERTIFICADO MEDICO
                                txt_certMed_CertificadoMedico.val(response.data.objHC.certMed_CertificadoMedico);
                                cbo_certMed_AptitudID.val(response.data.objHC.certMed_AptitudID);
                                cbo_certMed_AptitudID.trigger("change");
                                txt_certMed_Fecha.val(moment(response.data.objHC.certMed_Fecha).format("YYYY-MM-DD"));
                                txt_certMed_NombrePaciente.val(response.data.objHC.certMed_NombrePaciente);
                                //#endregion
                                //#region RECOMENDACIÓN
                                txt_recom_Recomendaciones.val(response.data.objHC.recom_Recomendaciones);
                                //#endregion

                                btnGuardarParcial.attr("data-id", rowData.id);
                                btnCEHistorialClinico.attr("data-id", rowData.id);
                            }
                        }).catch(error => Alert2Error(error.message));

                        //#endregion
                    });
                    tblS_SO_HistorialesClinicos.on('click', '.eliminarHistorialClinico', function () {
                        let rowData = dtHistorialClinico.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarHistorialClinico(rowData.id));
                    });
                    tblS_SO_HistorialesClinicos.on('click', '.exportarHistorialClinico', function () {
                        let rowData = dtHistorialClinico.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('HISTORIAL CLINICO', '¿Desea imprimir el historial clinico?', 'Confirmar', 'Cancelar', () => fncGetReporte(rowData.id));
                    });
                    tblS_SO_HistorialesClinicos.on('click', '.showMdlHCFirmado', function () {
                        let rowData = dtHistorialClinico.row($(this).closest('tr')).data();
                        btnCargarDocumentoFirmadoMedicoExterno.attr("data-id", rowData.id);
                        btnDescargarHCFirmado.css("display", "none");
                        fncVerificarExisteDocumento(rowData.id, 3);
                        mdlShowMdlHCFirmado.modal("show");
                    });
                    tblS_SO_HistorialesClinicos.on('click', '.showMdlObservacionCP', function () {
                        let rowData = dtHistorialClinico.row($(this).closest('tr')).data();
                        btnCEObservacionMedicoCP.attr("data-id", rowData.id);
                        fncGetObservacionMedicoInternoCP(rowData.id);
                        btnDescargarDocumentoMedicoInterno.css("display", "none");
                        fncVerificarExisteDocumento(rowData.id, 2);
                        mdlObservacionesCP.modal("show");
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { "width": "10%", "targets": 3 },
                    { "width": "10%", "targets": 4 },
                    { "width": "15%", "targets": 5 },
                ],
            });
        }

        function fncCEHistorialClinico(esGuardadoParcial) {
            let obj = fncCEObjHistorialClinico();
            axios.post("CEHistorialClinico", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito(response.data.message);
                    // fncGetHistorialesClinicos();
                    if (!esGuardadoParcial) {
                        btnCECancelar.trigger("click");
                    }
                    if (response.data.dataID > 0) {
                        btnCEHistorialClinico.attr("data-id", response.data.dataID);
                        btnGuardarParcial.attr("data-id", response.data.dataID);
                    }
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCEObjHistorialClinico() {
            let strMensajeError = "";

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return obj;
            } else {
                let obj = new Object();
                obj = {
                    id: btnCEHistorialClinico.attr("data-id"),
                    //#region DATOS PERSONALES
                    dtsPer_EmpresaID: cbo_dtsPer_EmpresaID.val() > 0 ? cbo_dtsPer_EmpresaID.val() : 0,
                    dtsPer_CCID: cbo_dtsPer_CCID.val() > 0 ? cbo_dtsPer_CCID.val() : 0,
                    dtsPer_Paciente: txt_dtsPer_Paciente.val() != "" ? txt_dtsPer_Paciente.val() : "",
                    dtsPer_FechaHora: txt_dtsPer_FechaHora.val() != "" ? txt_dtsPer_FechaHora.val() : "",
                    dtsPer_FechaNac: txt_dtsPer_FechaNac.val() != "" ? txt_dtsPer_FechaNac.val() : "",
                    dtsPer_Sexo: cbo_dtsPer_Sexo.val() != "" ? cbo_dtsPer_Sexo.val() : "",
                    dtsPer_EstadoCivilID: cbo_dtsPer_EstadoCivil.val() > 0 ? cbo_dtsPer_EstadoCivil.val() : 0,
                    dtsPer_TipoSangreID: cbo_dtsPer_TipoSanguineo.val() > 0 ? cbo_dtsPer_TipoSanguineo.val() : 0,
                    dtsPer_CURP: txt_dtsPer_CURP.val() != "" ? txt_dtsPer_CURP.val() : "",
                    dtsPer_Domicilio: txt_dtsPer_Domicilio.val() != "" ? txt_dtsPer_Domicilio.val() : "",
                    dtsPer_Ciudad: txt_dtsPer_Ciudad.val() != "" ? txt_dtsPer_Ciudad.val() : "",
                    dtsPer_EscolaridadID: cbo_dtsPer_Escolaridad.val() > 0 ? cbo_dtsPer_Escolaridad.val() : 0,
                    dtsPer_LugarNac: txt_dtsPer_LugarNacimiento.val() != "" ? txt_dtsPer_LugarNacimiento.val() : "",
                    dtsPer_Telefono: txt_dtsPer_Telefono.val() != "" ? txt_dtsPer_Telefono.val() : "",
                    //#endregion
                    //#region MOTIVO DE LA EVALUACION
                    motEva_esIngreso: chk_motEva_esIngreso.prop("checked"),
                    motEva_esRetiro: chk_motEva_esRetiro.prop("checked"),
                    motEva_esEvaOpcional: chk_motEva_esEvaOpcional.prop("checked"),
                    motEva_esPostIncapacidad: chk_motEva_esPostIncapacidad.prop("checked"),
                    motEva_esReubicacion: chk_motEva_esReubicacion.prop("checked"),
                    //#endregion
                    //#region ANTECEDENTES LABORALES
                    antLab_Puesto: txt_antLab_Puesto.val() != "" ? txt_antLab_Puesto.val() : "",
                    antLab_Empresa: txt_antLab_Empresa.val() != "" ? txt_antLab_Empresa.val() : "",
                    antLab_Desde: txt_antLab_Desde.val() != "" ? txt_antLab_Desde.val() : "",
                    antLab_Hasta: txt_antLab_Hasta.val() != "" ? txt_antLab_Hasta.val() : "",
                    antLab_Turno: txt_antLab_Turno.val() != "" ? txt_antLab_Turno.val() : "",
                    antLab_esDePie: chk_antLab_esDePie.prop("checked"),
                    antLab_esInclinado: chk_antLab_esInclinado.prop("checked"),
                    antLab_esSentado: chk_antLab_esSentado.prop("checked"),
                    antLab_esArrodillado: chk_antLab_esArrodillado.prop("checked"),
                    antLab_esCaminando: chk_antLab_esCaminando.prop("checked"),
                    antLab_esOtra: chk_antLab_esOtra.prop("checked"),
                    antLab_Cual: txt_antLab_Cual.val() != "" ? txt_antLab_Cual.val() : "",
                    //#endregion
                    //#region ACCIDENTES Y ENFERMEDADES DE TRABAJO
                    accET_Empresa: txt_accET_Empresa.val() != "" ? txt_accET_Empresa.val() : "",
                    accET_Anio: txt_accET_Anio.val() != "" ? txt_accET_Anio.val() : "",
                    accET_LesionAreaAnatomica: txt_accET_LesionAreaAnatomica.val() != "" ? txt_accET_LesionAreaAnatomica.val() : "",
                    accET_Secuelas: txt_accET_Secuelas.val() != "" ? txt_accET_Secuelas.val() : "",
                    accET_Cuales: txt_accET_Cuales.val() != "" ? txt_accET_Cuales.val() : "",
                    accET_ExamNoAceptables: txt_accET_ExamNoAceptables.val() != "" ? txt_accET_ExamNoAceptables.val() : "",
                    accET_Causas: txt_accET_Causas.val() != "" ? txt_accET_Causas.val() : "",
                    accET_AbandonoTrabajo: txt_accET_AbandonoTrabajo.val() != "" ? txt_accET_AbandonoTrabajo.val() : "",
                    accET_IncapacidadFrecuente: txt_accET_IncapacidadFrecuente.val() != "" ? txt_accET_IncapacidadFrecuente.val() : "",
                    accET_Prolongadas: txt_accET_Prolongadas.val() != "" ? txt_accET_Prolongadas.val() : "",
                    //#endregion
                    //#region USO DE ELEMENTOS DE PROTECCIÓN PERSONAL
                    usoElePP_esActual: chk_usoElePP_esActual.prop("checked"),
                    usoElePP_esCasco: chk_usoElePP_esCasco.prop("checked"),
                    usoElePP_esTapaboca: chk_usoElePP_esTapaboca.prop("checked"),
                    usoElePP_esGafas: chk_usoElePP_esGafas.prop("checked"),
                    usoElePP_esRespirador: chk_usoElePP_esRespirador.prop("checked"),
                    usoElePP_esBotas: chk_usoElePP_esBotas.prop("checked"),
                    usoElePP_esAuditivos: chk_usoElePP_esAuditivos.prop("checked"),
                    usoElePP_esOverol: chk_usoElePP_esOverol.prop("checked"),
                    usoElePP_esGuantes: chk_usoElePP_esGuantes.prop("checked"),
                    usoElePP_OtroCual: txt_usoElePP_OtroCual.val() != "" ? txt_usoElePP_OtroCual.val() : "",
                    usoElePP_DeberiaRecibir: txt_usoElePP_DeberiaRecibir.val() != "" ? txt_usoElePP_DeberiaRecibir.val() : "",
                    usoElePP_ConsideraAdecuado: txt_usoElePP_ConsideraAdecuado.val() != "" ? txt_usoElePP_ConsideraAdecuado.val() : "",
                    //#endregion
                    //#region ANTECEDENTES FAMILIARES
                    antFam_esTuberculosis: txt_antFam_esTuberculosis.val() != "" ? txt_antFam_esTuberculosis.val() : "",
                    antFam_TuberculosisParentesco: txt_antFam_TuberculosisParentesco.val() != "" ? txt_antFam_TuberculosisParentesco.val() : "",
                    antFam_esHTA: txt_antFam_esHTA.val() != "" ? txt_antFam_esHTA.val() : "",
                    antFam_HTAParentesco: txt_antFam_HTAParentesco.val() != "" ? txt_antFam_HTAParentesco.val() : "",
                    antFam_esDiabetes: txt_antFam_esDiabetes.val() != "" ? txt_antFam_esDiabetes.val() : "",
                    antFam_DiabetesParentesco: txt_antFam_DiabetesParentesco.val() != "" ? txt_antFam_DiabetesParentesco.val() : "",
                    antFam_esACV: txt_antFam_esACV.val() != "" ? txt_antFam_esACV.val() : "",
                    antFam_ACVParentesco: txt_antFam_ACVParentesco.val() != "" ? txt_antFam_ACVParentesco.val() : "",
                    antFam_esInfarto: txt_antFam_esInfarto.val() != "" ? txt_antFam_esInfarto.val() : "",
                    antFam_InfartoParentesco: txt_antFam_InfartoParentesco.val() != "" ? txt_antFam_InfartoParentesco.val() : "",
                    antFam_esAsma: txt_antFam_esAsma.val() != "" ? txt_antFam_esAsma.val() : "",
                    antFam_AsmaParentesco: txt_antFam_AsmaParentesco.val() != "" ? txt_antFam_AsmaParentesco.val() : "",
                    antFam_esAlergias: txt_antFam_esAlergias.val() != "" ? txt_antFam_esAlergias.val() : "",
                    antFam_AlergiasParentesco: txt_antFam_AlergiasParentesco.val() != "" ? txt_antFam_AlergiasParentesco.val() : "",
                    antFam_esMental: txt_antFam_esMental.val() != "" ? txt_antFam_esMental.val() : "",
                    antFam_MentalParentesco: txt_antFam_MentalParentesco.val() != "" ? txt_antFam_MentalParentesco.val() : "",
                    antFam_esCancer: txt_antFam_esCancer.val() != "" ? txt_antFam_esCancer.val() : "",
                    antFam_CancerParentesco: txt_antFam_CancerParentesco.val() != "" ? txt_antFam_CancerParentesco.val() : "",
                    antFam_Observaciones: txt_antFam_Observaciones.val() != "" ? txt_antFam_Observaciones.val() : "",
                    //#endregion
                    //#region ANTECEDENTES PERSONALES NO PATOLÓGICOS
                    antPerNoPat_Tabaquismo: cbo_antPerNoPat_Tabaquismo.val() != "" ? cbo_antPerNoPat_Tabaquismo.val() : "",
                    antPerNoPat_CigarroDia: txt_antPerNoPat_CigarroDia.val() != "" ? txt_antPerNoPat_CigarroDia.val() : "",
                    antPerNoPat_CigarroAnios: txt_antPerNoPat_CigarroAnios.val() != "" ? txt_antPerNoPat_CigarroAnios.val() : "",
                    antPerNoPat_Alcoholismo: cbo_antPerNoPat_Alcoholismo.val() != "" ? cbo_antPerNoPat_Alcoholismo.val() : "",
                    antPerNoPat_AlcoholismoAnios: txt_antPerNoPat_AlcoholismoAnios.val() != "" ? txt_antPerNoPat_AlcoholismoAnios.val() : "",
                    antPerNoPat_esDrogadiccion: chk_antPerNoPat_esDrogradiccion.prop("checked"),
                    antPerNoPat_esMarihuana: chk_antPerNoPat_esMarihuana.prop("checked"),
                    antPerNoPat_esCocaina: chk_antPerNoPat_esCocaina.prop("checked"),
                    antPerNoPat_esAnfetaminas: chk_antPerNoPat_esAnfetaminas.prop("checked"),
                    antPerNoPat_Otros: txt_antPerNoPat_Otros.val() != "" ? txt_antPerNoPat_Otros.val() : "",
                    antPerNoPat_Inmunizaciones: txt_antPerNoPat_Inmunizaciones.val() != "" ? txt_antPerNoPat_Inmunizaciones.val() : "",
                    antPerNoPat_Tetanicos: txt_antPerNoPat_Tetanicos.val() != "" ? txt_antPerNoPat_Tetanicos.val() : "",
                    antPerNoPat_FechaAntitetanica: txt_antPerNoPat_FechaAntitenica.val() != "" ? txt_antPerNoPat_FechaAntitenica.val() : "",
                    antPerNoPat_Hepatitis: txt_antPerNoPat_Hepatitis.val() != "" ? txt_antPerNoPat_Hepatitis.val() : "",
                    antPerNoPat_Influenza: txt_antPerNoPat_Influenza.val() != "" ? txt_antPerNoPat_Influenza.val() : "",
                    antPerNoPat_FechaInfluenza: txt_antPerNoPat_FechaInfluenza.val() != "" ? txt_antPerNoPat_FechaInfluenza.val() : "",
                    antPerNoPat_Infancia: txt_antPerNoPat_Infancia.val() != "" ? txt_antPerNoPat_Infancia.val() : "",
                    antPerNoPat_DescInfancia: txt_antPerNoPat_DescInfancia.val() != "" ? txt_antPerNoPat_DescInfancia.val() : "",
                    antPerNoPat_Alimentacion: txt_antPerNoPat_Alimentacion.val() != "" ? txt_antPerNoPat_Alimentacion.val() : "",
                    antPerNoPat_Higiene: txt_antPerNoPat_Higiene.val() != "" ? txt_antPerNoPat_Higiene.val() : "",
                    antPerNoPat_MedicacionActual: txt_antPerNoPat_MedicacionActual.val() != "" ? txt_antPerNoPat_MedicacionActual.val() : "",
                    //#endregion
                    //#region ANTECEDENTES PERSONALES PATOLÓGICOS
                    antPerPat_esNeoplasicos: chk_antPerPat_esNeoplasicos.prop("checked"),
                    antPerPat_esNeumopatias: chk_antPerPat_esNeumopatias.prop("checked"),
                    antPerPat_esAsma: chk_antPerPat_esAsma.prop("checked"),
                    antPerPat_esFimico: chk_antPerPat_esFimico.prop("checked"),
                    antPerPat_esNeumoconiosis: chk_antPerPat_esNeumoconiosis.prop("checked"),
                    antPerPat_esCardiopatias: chk_antPerPat_esCardiopatias.prop("checked"),
                    antPerPat_esReumaticos: chk_antPerPat_esReumaticos.prop("checked"),
                    antPerPat_esAlergias: chk_antPerPat_esAlergias.prop("checked"),
                    antPerPat_esHipertension: chk_antPerPat_esHipertension.prop("checked"),
                    antPerPat_esHepatitis: chk_antPerPat_esHepatitis.prop("checked"),
                    antPerPat_esTifoidea: chk_antPerPat_esTifoidea.prop("checked"),
                    antPerPat_esHernias: chk_antPerPat_esHernias.prop("checked"),
                    antPerPat_esLumbalgias: chk_antPerPat_esLumbalgias.prop("checked"),
                    antPerPat_esDiabetes: chk_antPerPat_esDiabetes.prop("checked"),
                    antPerPat_esEpilepsias: chk_antPerPat_esEpilepsias.prop("checked"),
                    antPerPat_esVenereas: chk_antPerPat_esVenereas.prop("checked"),
                    antPerPat_esCirugias: chk_antPerPat_esCirugias.prop("checked"),
                    antPerPat_esFracturas: chk_antPerPat_esFracturas.prop("checked"),
                    antPerPat_ObservacionesPat: txt_antPerPat_ObservacionesPat.val() != "" ? txt_antPerPat_ObservacionesPat.val() : "",
                    //#endregion
                    //#region INTERROGATORIO POR APARATOS Y SISTEMAS
                    intApaSis_esRespiratorio: chk_intApaSis_esRespiratorio.prop("checked"),
                    intApaSis_esDigestivo: chk_intApaSis_esDigestivo.prop("checked"),
                    intApaSis_esCardiovascular: chk_intApaSis_esCardiovascular.prop("checked"),
                    intApaSis_esNervioso: chk_intApaSis_esNervioso.prop("checked"),
                    intApaSis_esUrinario: chk_intApaSis_esUrinario.prop("checked"),
                    intApaSis_esEndocrino: chk_intApaSis_esEndocrino.prop("checked"),
                    intApaSis_esPsiquiatrico: chk_intApaSis_esPsiquiatrico.prop("checked"),
                    intApaSis_esEsqueletico: chk_intApaSis_esEsqueletico.prop("checked"),
                    intApaSis_esAudicion: chk_intApaSis_esAudicion.prop("checked"),
                    intApaSis_esVision: chk_intApaSis_esVision.prop("checked"),
                    intApaSis_esOlfato: chk_intApaSis_esOlfato.prop("checked"),
                    intApaSis_esTacto: chk_intApaSis_esTacto.prop("checked"),
                    intApaSis_ObservacionesPat: txt_intApaSis_ObservacionesPat.val() != "" ? txt_intApaSis_ObservacionesPat.val() : "",
                    //#endregion
                    //#region PADECIMIENTOS ACTUALES
                    padAct_PadActuales: txt_padAct_PadActuales.val() != "" ? txt_padAct_PadActuales.val() : "",
                    //#endregion
                    //#region EXPLORACIÓN FÍSICA-SIGNOS VITALES
                    expFSV_TArterial: txt_expFSV_TArterial.val() != "" ? txt_expFSV_TArterial.val() : "",
                    expFSV_Pulso: txt_expFSV_Pulso.val() != "" ? txt_expFSV_Pulso.val() : "",
                    expFSV_Temp: txt_expFSV_Temp.val() != "" ? txt_expFSV_Temp.val() : "",
                    expFSV_FCardiaca: txt_expFSV_FCardiaca.val() != "" ? txt_expFSV_FCardiaca.val() : "",
                    expFSV_FResp: txt_expFSV_FResp.val() != "" ? txt_expFSV_FResp.val() : "",
                    expFSV_Peso: txt_expFSV_Peso.val() != "" ? txt_expFSV_Peso.val() : "",
                    expFSV_Talla: txt_expFSV_Talla.val() != "" ? txt_expFSV_Talla.val() : "",
                    expFSV_IMC: txt_expFSV_IMC.val() != "" ? txt_expFSV_IMC.val() : "",
                    //#endregion
                    //#region EXPLORACIÓN FÍSICA-CABEZA
                    expFC_Craneo: $("#cbo_expFC_Craneo").val() != "" ? $("#cbo_expFC_Craneo").val() : "",
                    expFC_Parpados: $("#cbo_expFC_Parpados").val() != "" ? $("#cbo_expFC_Parpados").val() : "",
                    expFC_Conjutiva: $("#cbo_expFC_Conjutiva").val() != "" ? $("#cbo_expFC_Conjutiva").val() : "",
                    expFC_Reflejos: $("#cbo_expFC_Reflejos").val() != "" ? $("#cbo_expFC_Reflejos").val() : "",
                    expFC_FosasNasales: $("#cbo_expFC_FosasNasales").val() != "" ? $("#cbo_expFC_FosasNasales").val() : "",
                    expFC_Boca: $("#cbo_expFC_Boca").val() != "" ? $("#cbo_expFC_Boca").val() : "",
                    expFC_Amigdalas: $("#cbo_expFC_Amigdalas").val() != "" ? $("#cbo_expFC_Amigdalas").val() : "",
                    expFC_Dentadura: $("#cbo_expFC_Dentadura").val() != "" ? $("#cbo_expFC_Dentadura").val() : "",
                    expFC_Encias: $("#cbo_expFC_Encias").val() != "" ? $("#cbo_expFC_Encias").val() : "",
                    expFC_Cuello: $("#cbo_expFC_Cuello").val() != "" ? $("#cbo_expFC_Cuello").val() : "",
                    expFC_Tiroides: $("#cbo_expFC_Tiroides").val() != "" ? $("#cbo_expFC_Tiroides").val() : "",
                    expFC_Ganglios: $("#cbo_expFC_Ganglios").val() != "" ? $("#cbo_expFC_Ganglios").val() : "",
                    expFC_Oidos: $("#cbo_expFC_Oidos").val() != "" ? $("#cbo_expFC_Oidos").val() : "",
                    expFC_Otros: $("#cbo_expFC_Otros").val() != "" ? $("#cbo_expFC_Otros").val() : "",
                    expFC_Observaciones: $("#txt_expFC_Observaciones").val() != "" ? $("#txt_expFC_Observaciones").val() : "",
                    //#endregion
                    //#region EXPLORACIÓN FÍSICA-AGUDEZA VISUAL
                    expFAV_VisCerAmbosOjos: txt_expFAV_VisCerAmbosOjos.val() != "" ? txt_expFAV_VisCerAmbosOjos.val() : "",
                    expFAV_VisCerOjoIzq: txt_expFAV_VisCerOjoIzq.val() != "" ? txt_expFAV_VisCerOjoIzq.val() : "",
                    expFAV_VisCerOjoDer: txt_expFAV_VisCerOjoDer.val() != "" ? txt_expFAV_VisCerOjoDer.val() : "",
                    expFAV_VisLejAmbosOjos: txt_expFAV_VisLejAmbosOjos.val() != "" ? txt_expFAV_VisLejAmbosOjos.val() : "",
                    expFAV_VisLejOjoIzq: txt_expFAV_VisLejOjoIzq.val() != "" ? txt_expFAV_VisLejOjoIzq.val() : "",
                    expFAV_VisLejOjoDer: txt_expFAV_VisLejOjoDer.val() != "" ? txt_expFAV_VisLejOjoDer.val() : "",
                    expFAV_CorregidaAmbosOjos: txt_expFAV_CorregidaAmbosOjos.val() != "" ? txt_expFAV_CorregidaAmbosOjos.val() : "",
                    expFAV_CorregidaOjoIzq: txt_expFAV_CorregidaOjoIzq.val() != "" ? txt_expFAV_CorregidaOjoIzq.val() : "",
                    expFAV_CorregidaOjoDer: txt_expFAV_CorregidaOjoDer.val() != "" ? txt_expFAV_CorregidaOjoDer.val() : "",
                    expFAV_CampimetriaOI: txt_expFAV_CampimetriaOI.val() != "" ? txt_expFAV_CampimetriaOI.val() : "",
                    expFAV_CampimetriaOD: txt_expFAV_CampimetriaOD.val() != "" ? txt_expFAV_CampimetriaOD.val() : "",
                    expFAV_PterigionOI: txt_expFAV_PterigionOI.val() != "" ? txt_expFAV_PterigionOI.val() : "",
                    expFAV_PterigionOD: txt_expFAV_PterigionOD.val() != "" ? txt_expFAV_PterigionOD.val() : "",
                    expFAV_FondoOjo: txt_expFAV_FondoOjo.val() != "" ? txt_expFAV_FondoOjo.val() : "",
                    expFAV_Daltonismo: txt_expFAV_Daltonismo.val() != "" ? txt_expFAV_Daltonismo.val() : "",
                    expFAV_Observaciones: txt_expFAV_Observaciones.val() != "" ? txt_expFAV_Observaciones.val() : "",
                    //#endregion
                    //#region EXPLORACIÓN FÍSICA-TORAX, ABDOMEN, TRONCO Y EXTREMIDADES
                    expFTATE_esCamposPulmonares: chk_expFTATE_esCamposPulmonares.prop("checked"),
                    expFTATE_esPuntosDolorosos: chk_expFTATE_esPuntosDolorosos.prop("checked"),
                    expFTATE_esGenitales: chk_expFTATE_esGenitales.prop("checked"),
                    expFTATE_esRuidosCardiacos: chk_expFTATE_esRuidosCardiacos.prop("checked"),
                    expFTATE_esHallusValgus: chk_expFTATE_esHallusValgus.prop("checked"),
                    expFTATE_esHerniasUmbili: chk_expFTATE_esHerniasUmbili.prop("checked"),
                    expFTATE_esAreaRenal: chk_expFTATE_esAreaRenal.prop("checked"),
                    expFTATE_esVaricocele: chk_expFTATE_esVaricocele.prop("checked"),
                    expFTATE_esGrandulasMamarias: chk_expFTATE_esGrandulasMamarias.prop("checked"),
                    expFTATE_esColumnaVertebral: chk_expFTATE_esColumnaVertebral.prop("checked"),
                    expFTATE_esPiePlano: chk_expFTATE_esPiePlano.prop("checked"),
                    expFTATE_esVarices: chk_expFTATE_esVarices.prop("checked"),
                    expFTATE_esMiembrosSup: chk_expFTATE_esMiembrosSup.prop("checked"),
                    expFTATE_esParedAbdominal: chk_expFTATE_esParedAbdominal.prop("checked"),
                    expFTATE_esAnillosInguinales: chk_expFTATE_esAnillosInguinales.prop("checked"),
                    expFTATE_esMiembrosInf: chk_expFTATE_esMiembrosInf.prop("checked"),
                    expFTATE_esTatuajes: chk_expFTATE_esTatuajes.prop("checked"),
                    expFTATE_esVisceromegalias: chk_expFTATE_esVisceromegalias.prop("checked"),
                    expFTATE_esMarcha: chk_expFTATE_esMarcha.prop("checked"),
                    expFTATE_esHerniasInguinales: chk_expFTATE_esHerniasInguinales.prop("checked"),
                    expFTATE_esHombrosDolorosos: chk_expFTATE_esHombrosDolorosos.prop("checked"),
                    expFTATE_esQuistes: chk_expFTATE_esQuistes.prop("checked"),
                    expFTATE_Observaciones: txt_expFTATE_Observaciones.val() != "" ? txt_expFTATE_Observaciones.val() : "",
                    expFTATE_MS_HombroDer_esFlexion: chk_expFTATE_MS_HombroDer_esFlexion.prop("checked"),
                    expFTATE_MS_HombroIzq_esFlexion: chk_expFTATE_MS_HombroIzq_esFlexion.prop("checked"),
                    expFTATE_MS_CodoDer_esFlexion: chk_expFTATE_MS_CodoDer_esFlexion.prop("checked"),
                    expFTATE_MS_CodoIzq_esFlexion: chk_expFTATE_MS_CodoIzq_esFlexion.prop("checked"),
                    expFTATE_MS_MunecaDer_esFlexion: chk_expFTATE_MS_MunecaDer_esFlexion.prop("checked"),
                    expFTATE_MS_MunecaIzq_esFlexion: chk_expFTATE_MS_MunecaIzq_esFlexion.prop("checked"),
                    expFTATE_MS_DedosDer_esFlexion: chk_expFTATE_MS_DedosDer_esFlexion.prop("checked"),
                    expFTATE_MS_DedosIzq_esFlexion: chk_expFTATE_MS_DedosIzq_esFlexion.prop("checked"),
                    expFTATE_MS_HombroDer_esExtension: chk_expFTATE_MS_HombroDer_esExtension.prop("checked"),
                    expFTATE_MS_HombroIzq_esExtension: chk_expFTATE_MS_HombroIzq_esExtension.prop("checked"),
                    expFTATE_MS_CodoDer_esExtension: chk_expFTATE_MS_CodoDer_esExtension.prop("checked"),
                    expFTATE_MS_CodoIzq_esExtension: chk_expFTATE_MS_CodoIzq_esExtension.prop("checked"),
                    expFTATE_MS_MunecaDer_esExtension: chk_expFTATE_MS_MunecaDer_esExtension.prop("checked"),
                    expFTATE_MS_MunecaIzq_esExtension: chk_expFTATE_MS_MunecaIzq_esExtension.prop("checked"),
                    expFTATE_MS_DedosDer_esExtension: chk_expFTATE_MS_DedosDer_esExtension.prop("checked"),
                    expFTATE_MS_DedosIzq_esExtension: chk_expFTATE_MS_DedosIzq_esExtension.prop("checked"),
                    expFTATE_MS_HombroDer_esAbduccion: chk_expFTATE_MS_HombroDer_esAbduccion.prop("checked"),
                    expFTATE_MS_HombroIzq_esAbduccion: chk_expFTATE_MS_HombroIzq_esAbduccion.prop("checked"),
                    expFTATE_MS_CodoDer_esAbduccion: chk_expFTATE_MS_CodoDer_esAbduccion.prop("checked"),
                    expFTATE_MS_CodoIzq_esAbduccion: chk_expFTATE_MS_CodoIzq_esAbduccion.prop("checked"),
                    expFTATE_MS_MunecaDer_esAbduccion: chk_expFTATE_MS_MunecaDer_esAbduccion.prop("checked"),
                    expFTATE_MS_MunecaIzq_esAbduccion: chk_expFTATE_MS_MunecaIzq_esAbduccion.prop("checked"),
                    expFTATE_MS_DedosDer_esAbduccion: chk_expFTATE_MS_DedosDer_esAbduccion.prop("checked"),
                    expFTATE_MS_DedosIzq_esAbduccion: chk_expFTATE_MS_DedosIzq_esAbduccion.prop("checked"),
                    expFTATE_MS_HombroDer_esAduccion: chk_expFTATE_MS_HombroDer_esAduccion.prop("checked"),
                    expFTATE_MS_HombroIzq_esAduccion: chk_expFTATE_MS_HombroIzq_esAduccion.prop("checked"),
                    expFTATE_MS_MunecaDer_esAduccion: chk_expFTATE_MS_MunecaDer_esAduccion.prop("checked"),
                    expFTATE_MS_MunecaIzq_esAduccion: chk_expFTATE_MS_MunecaIzq_esAduccion.prop("checked"),
                    expFTATE_MS_DedosDer_esAduccion: chk_expFTATE_MS_DedosDer_esAduccion.prop("checked"),
                    expFTATE_MS_DedosIzq_esAduccion: chk_expFTATE_MS_DedosIzq_esAduccion.prop("checked"),
                    expFTATE_MS_HombroDer_esRotInterna: chk_expFTATE_MS_HombroDer_esRotInterna.prop("checked"),
                    expFTATE_MS_HombroIzq_esRotInterna: chk_expFTATE_MS_HombroIzq_esRotInterna.prop("checked"),
                    expFTATE_MS_MunecaDer_esRotInterna: chk_expFTATE_MS_MunecaDer_esRotInterna.prop("checked"),
                    expFTATE_MS_MunecaIzq_esRotInterna: chk_expFTATE_MS_MunecaIzq_esRotInterna.prop("checked"),
                    expFTATE_MS_DedosDer_esRotInterna: chk_expFTATE_MS_DedosDer_esRotInterna.prop("checked"),
                    expFTATE_MS_DedosIzq_esRotInterna: chk_expFTATE_MS_DedosIzq_esRotInterna.prop("checked"),
                    expFTATE_MS_HombroDer_esRotExterna: chk_expFTATE_MS_HombroDer_esRotExterna.prop("checked"),
                    expFTATE_MS_HombroIzq_esRotExterna: chk_expFTATE_MS_HombroIzq_esRotExterna.prop("checked"),
                    expFTATE_MS_MunecaDer_esRotExterna: chk_expFTATE_MS_MunecaDer_esRotExterna.prop("checked"),
                    expFTATE_MS_MunecaIzq_esRotExterna: chk_expFTATE_MS_MunecaIzq_esRotExterna.prop("checked"),
                    expFTATE_MS_DedosDer_esRotExterna: chk_expFTATE_MS_DedosDer_esRotExterna.prop("checked"),
                    expFTATE_MS_DedosIzq_esRotExterna: chk_expFTATE_MS_DedosIzq_esRotExterna.prop("checked"),
                    expFTATE_MS_CodoDer_esPronacion: chk_expFTATE_MS_CodoDer_esPronacion.prop("checked"),
                    expFTATE_MS_CodoIzq_esPronacion: chk_expFTATE_MS_CodoIzq_esPronacion.prop("checked"),
                    expFTATE_MS_MunecaDer_esPronacion: chk_expFTATE_MS_MunecaDer_esPronacion.prop("checked"),
                    expFTATE_MS_MunecaIzq_esPronacion: chk_expFTATE_MS_MunecaIzq_esPronacion.prop("checked"),
                    expFTATE_MS_CodoDer_esSupinacion: chk_expFTATE_MS_CodoDer_esSupinacion.prop("checked"),
                    expFTATE_MS_CodoIzq_esSupinacion: chk_expFTATE_MS_CodoIzq_esSupinacion.prop("checked"),
                    expFTATE_MS_MunecaDer_esSupinacion: chk_expFTATE_MS_MunecaDer_esSupinacion.prop("checked"),
                    expFTATE_MS_MunecaIzq_esSupinacion: chk_expFTATE_MS_MunecaIzq_esSupinacion.prop("checked"),
                    expFTATE_MS_MunecaDer_esDesvUlnar: chk_expFTATE_MS_MunecaDer_esDesvUlnar.prop("checked"),
                    expFTATE_MS_MunecaIzq_esDesvUlnar: chk_expFTATE_MS_MunecaIzq_esDesvUlnar.prop("checked"),
                    expFTATE_MS_MunecaDer_esDesvRadial: chk_expFTATE_MS_MunecaDer_esDesvRadial.prop("checked"),
                    expFTATE_MS_MunecaIzq_esDesvRadial: chk_expFTATE_MS_MunecaIzq_esDesvRadial.prop("checked"),
                    expFTATE_MS_MunecaDer_esOponencia: chk_expFTATE_MS_MunecaDer_esOponencia.prop("checked"),
                    expFTATE_MS_MunecaIzq_esOponencia: chk_expFTATE_MS_MunecaIzq_esOponencia.prop("checked"),
                    expFTATE_MS_DedosDer_esOponencia: chk_expFTATE_MS_DedosDer_esOponencia.prop("checked"),
                    expFTATE_MS_DedosIzq_esOponencia: chk_expFTATE_MS_DedosIzq_esOponencia.prop("checked"),
                    expFTATE_MI_CaderaDer_esFlexion: chk_expFTATE_MI_CaderaDer_esFlexion.prop("checked"),
                    expFTATE_MI_CaderaIzq_esFlexion: chk_expFTATE_MI_CaderaIzq_esFlexion.prop("checked"),
                    expFTATE_MI_RodillasDer_esFlexion: chk_expFTATE_MI_RodillasDer_esFlexion.prop("checked"),
                    expFTATE_MI_RodillasIzq_esFlexion: chk_expFTATE_MI_RodillasIzq_esFlexion.prop("checked"),
                    expFTATE_MI_CllPieDer_esFlexion: chk_expFTATE_MI_CllPieDer_esFlexion.prop("checked"),
                    expFTATE_MI_CllPieIzq_esFlexion: chk_expFTATE_MI_CllPieIzq_esFlexion.prop("checked"),
                    expFTATE_MI_DedosDer_esFlexion: chk_expFTATE_MI_DedosDer_esFlexion.prop("checked"),
                    expFTATE_MI_DedosIzq_esFlexion: chk_expFTATE_MI_DedosIzq_esFlexion.prop("checked"),
                    expFTATE_MI_CaderaDer_esExtension: chk_expFTATE_MI_CaderaDer_esExtension.prop("checked"),
                    expFTATE_MI_CaderaIzq_esExtension: chk_expFTATE_MI_CaderaIzq_esExtension.prop("checked"),
                    expFTATE_MI_RodillasDer_esExtension: chk_expFTATE_MI_RodillasDer_esExtension.prop("checked"),
                    expFTATE_MI_RodillasIzq_esExtension: chk_expFTATE_MI_RodillasIzq_esExtension.prop("checked"),
                    expFTATE_MI_CllPieDer_esExtension: chk_expFTATE_MI_CllPieDer_esExtension.prop("checked"),
                    expFTATE_MI_CllPieIzq_esExtension: chk_expFTATE_MI_CllPieIzq_esExtension.prop("checked"),
                    expFTATE_MI_DedosDer_esExtension: chk_expFTATE_MI_DedosDer_esExtension.prop("checked"),
                    expFTATE_MI_DedosIzq_esExtension: chk_expFTATE_MI_DedosIzq_esExtension.prop("checked"),
                    expFTATE_MI_CaderaDer_esAbduccion: chk_expFTATE_MI_CaderaDer_esAbduccion.prop("checked"),
                    expFTATE_MI_CaderaIzq_esAbduccion: chk_expFTATE_MI_CaderaIzq_esAbduccion.prop("checked"),
                    expFTATE_MI_CllPieDer_esAbduccion: chk_expFTATE_MI_CllPieDer_esAbduccion.prop("checked"),
                    expFTATE_MI_CllPieIzq_esAbduccion: chk_expFTATE_MI_CllPieIzq_esAbduccion.prop("checked"),
                    expFTATE_MI_DedosDer_esAbduccion: chk_expFTATE_MI_DedosDer_esAbduccion.prop("checked"),
                    expFTATE_MI_DedosIzq_esAbduccion: chk_expFTATE_MI_DedosIzq_esAbduccion.prop("checked"),
                    expFTATE_MI_CaderaDer_esAduccion: chk_expFTATE_MI_CaderaDer_esAduccion.prop("checked"),
                    expFTATE_MI_CaderaIzq_esAduccion: chk_expFTATE_MI_CaderaIzq_esAduccion.prop("checked"),
                    expFTATE_MI_CllPieDer_esAduccion: chk_expFTATE_MI_CllPieDer_esAduccion.prop("checked"),
                    expFTATE_MI_CllPieIzq_esAduccion: chk_expFTATE_MI_CllPieIzq_esAduccion.prop("checked"),
                    expFTATE_MI_DedosDer_esAduccion: chk_expFTATE_MI_DedosDer_esAduccion.prop("checked"),
                    expFTATE_MI_DedosIzq_esAduccion: chk_expFTATE_MI_DedosIzq_esAduccion.prop("checked"),
                    expFTATE_MI_CaderaDer_esRotInterna: chk_expFTATE_MI_CaderaDer_esRotInterna.prop("checked"),
                    expFTATE_MI_CaderaIzq_esRotInterna: chk_expFTATE_MI_CaderaIzq_esRotInterna.prop("checked"),
                    expFTATE_MI_RodillasDer_esRotInterna: chk_expFTATE_MI_RodillasDer_esRotInterna.prop("checked"),
                    expFTATE_MI_RodillasIzq_esRotInterna: chk_expFTATE_MI_RodillasIzq_esRotInterna.prop("checked"),
                    expFTATE_MI_CllPieDer_esRotInterna: chk_expFTATE_MI_CllPieDer_esRotInterna.prop("checked"),
                    expFTATE_MI_CllPieIzq_esRotInterna: chk_expFTATE_MI_CllPieIzq_esRotInterna.prop("checked"),
                    expFTATE_MI_CaderaDer_esRotExterna: chk_expFTATE_MI_CaderaDer_esRotExterna.prop("checked"),
                    expFTATE_MI_CaderaIzq_esRotExterna: chk_expFTATE_MI_CaderaIzq_esRotExterna.prop("checked"),
                    expFTATE_MI_RodillasDer_esRotExterna: chk_expFTATE_MI_RodillasDer_esRotExterna.prop("checked"),
                    expFTATE_MI_RodillasIzq_esRotExterna: chk_expFTATE_MI_RodillasIzq_esRotExterna.prop("checked"),
                    expFTATE_MI_CllPieDer_esRotExterna: chk_expFTATE_MI_CllPieDer_esRotExterna.prop("checked"),
                    expFTATE_MI_CllPieIzq_esRotExterna: chk_expFTATE_MI_CllPieIzq_esRotExterna.prop("checked"),
                    expFTATE_MI_CllPieDer_esInversion: chk_expFTATE_MI_CllPieDer_esInversion.prop("checked"),
                    expFTATE_MI_CllPieIzq_esInversion: chk_expFTATE_MI_CllPieIzq_esInversion.prop("checked"),
                    expFTATE_MI_CllPieDer_esEversion: chk_expFTATE_MI_CllPieDer_esEversion.prop("checked"),
                    expFTATE_MI_CllPieIzq_esEversion: chk_expFTATE_MI_CllPieIzq_esEversion.prop("checked"),
                    expFTATE_MS_MI_Observaciones: txt_expFTATE_MS_MI_Observaciones.val() != "" ? txt_expFTATE_MS_MI_Observaciones.val() : "",
                    //#endregion
                    //#region EXPLORACIÓN FÍSICA-DATOS GENERALES
                    expFDT_PielMucosas: txt_expFDT_PielMucosas.val() != "" ? txt_expFDT_PielMucosas.val() : "",
                    expFDT_EstadoPsiquiatrico: txt_expFDT_EstadoPsiquiatrico.val() != "" ? txt_expFDT_EstadoPsiquiatrico.val() : "",
                    expFDT_ExamenNeurologico: txt_expFDT_ExamenNeurologico.val() != "" ? txt_expFDT_ExamenNeurologico.val() : "",
                    expFDT_FobiasActuales: txt_expFDT_FobiasActuales.val() != "" ? txt_expFDT_FobiasActuales.val() : "",
                    expFDT_Higiene: txt_expFDT_Higiene.val() != "" ? txt_expFDT_Higiene.val() : "",
                    expFDT_ConstitucionFisica: txt_expFDT_ConstitucionFisica.val() != "" ? txt_expFDT_ConstitucionFisica.val() : "",
                    expFDT_Otros: txt_expFDT_Otros.val() != "" ? txt_expFDT_Otros.val() : "",
                    expFDT_Observaciones: txt_expFDT_Observaciones.val() != "" ? txt_expFDT_Observaciones.val() : "",
                    //#endregion
                    //#region ESTUDIOS DE GABINETE
                    estGab_TipoSanguineoID: cbo_estGab_TipoSanguineoID.val() > 0 ? cbo_estGab_TipoSanguineoID.val() : 0,
                    estGab_Antidoping: cbo_estGab_Antidoping.val() != "" ? cbo_estGab_Antidoping.val() : "",
                    estGab_Laboratorios: txt_estGab_Laboratorios.val() != "" ? txt_estGab_Laboratorios.val() : "",
                    estGab_ObservacionesGrupoRH: txt_estGab_ObservacionesGrupoRH.val() != "" ? txt_estGab_ObservacionesGrupoRH.val() : "",
                    estGab_ExamGenOrina: txt_estGab_ExamGenOrina.val() != "" ? txt_estGab_ExamGenOrina.val() : "",
                    estGab_ExamGenOrinaObservaciones: txt_estGab_ExamGenOrinaObservaciones.val() != "" ? txt_estGab_ExamGenOrinaObservaciones.val() : "",
                    estGab_Radiografias: txt_estGab_Radiografias.val() != "" ? txt_estGab_Radiografias.val() : "",
                    estGab_RadiografiasObservaciones: txt_estGab_RadiografiasObservaciones.val() != "" ? txt_estGab_RadiografiasObservaciones.val() : "",
                    estGab_Audiometria: txt_estGab_Audiometria.val() != "" ? txt_estGab_Audiometria.val() : "",
                    estGab_HBC: txt_estGab_HBC.val() != "" ? txt_estGab_HBC.val() : "",
                    estGab_AudiometriaObservaciones: txt_estGab_AudiometriaObservaciones.val() != "" ? txt_estGab_AudiometriaObservaciones.val() : "",
                    estGab_Espirometria: txt_estGab_Espirometria.val() != "" ? txt_estGab_Espirometria.val() : "",
                    estGab_EspirometriaObservaciones: txt_estGab_EspirometriaObservaciones.val() != "" ? txt_estGab_EspirometriaObservaciones.val() : "",
                    estGab_Electrocardiograma: txt_estGab_Electrocardiograma.val() != "" ? txt_estGab_Electrocardiograma.val() : "",
                    estGab_ElectrocardiogramaObservaciones: txt_estGab_ElectrocardiogramaObservaciones.val() != "" ? txt_estGab_ElectrocardiogramaObservaciones.val() : "",
                    estGab_FechaPrimeraDosis: txt_estGab_FechaPrimeraDosis.val() != "" ? txt_estGab_FechaPrimeraDosis.val() : "",
                    estGab_FechaSegundaDosis: txt_estGab_FechaSegundaDosis.val() != "" ? txt_estGab_FechaSegundaDosis.val() : "",
                    estGab_MarcaDosisID: cbo_estGab_MarcaDosisID.val() > 0 ? cbo_estGab_MarcaDosisID.val() : 0,
                    estGab_VacunacionObservaciones: txt_estGab_VacunacionObservaciones.val() != "" ? txt_estGab_VacunacionObservaciones.val() : "",
                    estGab_LstProblemas: txt_estGab_LstProblemas.val() != "" ? txt_estGab_LstProblemas.val() : "",
                    estGab_Recomendaciones: txt_estGab_Recomendaciones.val() != "" ? txt_estGab_Recomendaciones.val() : "",
                    //#endregion
                    //#region ESPIROMETRÍA
                    esp_Espirometria: txt_esp_Espirometria.val() != "" ? txt_esp_Espirometria.val() : "",
                    esp_EspirometriaObservaciones: txt_esp_EspirometriaObservaciones.val() != "" ? txt_esp_EspirometriaObservaciones.val() : "",
                    //#endregion
                    //#region AUDIOMETRÍA
                    aud_HipoacusiaOD: txt_aud_HipoacusiaOD.val() != "" ? txt_aud_HipoacusiaOD.val() : "",
                    aud_HipoacusiaOI: txt_aud_HipoacusiaOI.val() != "" ? txt_aud_HipoacusiaOI.val() : "",
                    aud_HBC: txt_aud_HBC.val() != "" ? txt_aud_HBC.val() : "",
                    aud_CargarAudiometria: txt_aud_CargarAudiometria.val() != "" ? txt_aud_CargarAudiometria.val() : "",
                    aud_Diagnostico: cbo_aud_Diagnostico.val() != "" ? cbo_aud_Diagnostico.val() : "",
                    aud_KH1: txt_aud_KH1.val() != "" ? txt_aud_KH1.val() : "",
                    aud_KH1_OI: txt_aud_KH1_OI.val() != "" ? txt_aud_KH1_OI.val() : "",
                    aud_KH1_OD: txt_aud_KH1_OD.val() != "" ? txt_aud_KH1_OD.val() : "",
                    aud_KH2: txt_aud_KH2.val() != "" ? txt_aud_KH2.val() : "",
                    aud_KH2_OI: txt_aud_KH2_OI.val() != "" ? txt_aud_KH2_OI.val() : "",
                    aud_KH2_OD: txt_aud_KH2_OD.val() != "" ? txt_aud_KH2_OD.val() : "",
                    aud_KH3: txt_aud_KH3.val() != "" ? txt_aud_KH3.val() : "",
                    aud_KH3_OI: txt_aud_KH3_OI.val() != "" ? txt_aud_KH3_OI.val() : "",
                    aud_KH3_OD: txt_aud_KH3_OD.val() != "" ? txt_aud_KH3_OD.val() : "",
                    aud_KH4: txt_aud_KH4.val() != "" ? txt_aud_KH4.val() : "",
                    aud_KH4_OI: txt_aud_KH4_OI.val() != "" ? txt_aud_KH4_OI.val() : "",
                    aud_KH4_OD: txt_aud_KH4_OD.val() != "" ? txt_aud_KH4_OD.val() : "",
                    aud_KH5: txt_aud_KH5.val() != "" ? txt_aud_KH5.val() : "",
                    aud_KH5_OI: txt_aud_KH5_OI.val() != "" ? txt_aud_KH5_OI.val() : "",
                    aud_KH5_OD: txt_aud_KH5_OD.val() != "" ? txt_aud_KH5_OD.val() : "",
                    aud_KH6: txt_aud_KH6.val() != "" ? txt_aud_KH6.val() : "",
                    aud_KH6_OI: txt_aud_KH6_OI.val() != "" ? txt_aud_KH6_OI.val() : "",
                    aud_KH6_OD: txt_aud_KH6_OD.val() != "" ? txt_aud_KH6_OD.val() : "",
                    aud_KH7: txt_aud_KH7.val() != "" ? txt_aud_KH7.val() : "",
                    aud_KH7_OI: txt_aud_KH7_OI.val() != "" ? txt_aud_KH7_OI.val() : "",
                    aud_KH7_OD: txt_aud_KH7_OD.val() != "" ? txt_aud_KH7_OD.val() : "",
                    aud_NotasAudiometria: txt_aud_NotasAudiometria.val() != "" ? txt_aud_NotasAudiometria.val() : "",
                    //#endregion
                    //#region ELECTROCARDIOGRAMA 12 DERIVACIONES
                    eleDer_Interpretacion: txt_eleDer_Interpretacion.val() != "" ? txt_eleDer_Interpretacion.val() : "",
                    //#endregion
                    //#region RADIOGRAFIAS - TORAX Y COLUMNA LUMBAR DOS POSICIONES
                    radTCLP_Conclusiones: txt_radTCLP_Conclusiones.val() != "" ? txt_radTCLP_Conclusiones.val() : "",
                    //#endregion
                    //#region CERTIFICADO MEDICO
                    certMed_CertificadoMedico: txt_certMed_CertificadoMedico.val() != "" ? txt_certMed_CertificadoMedico.val() : "",
                    certMed_AptitudID: cbo_certMed_AptitudID.val() > 0 ? cbo_certMed_AptitudID.val() : 0,
                    certMed_Fecha: txt_certMed_Fecha.val() != "" ? txt_certMed_Fecha.val() : "",
                    certMed_NombrePaciente: txt_certMed_NombrePaciente.val() != "" ? txt_certMed_NombrePaciente.val() : "",
                    //#endregion
                    //#region RECOMENDACIÓN
                    recom_Recomendaciones: txt_recom_Recomendaciones.val() != "" ? txt_recom_Recomendaciones.val() : "",
                    //#endregion
                }

                const data = new FormData();
                data.append('objHC', JSON.stringify(obj));

                //#region DATOS PERSONALES
                for (let i = 0; i < $(`#txt_dtsPer_ImagenPersona`)[0].files.length; i++) {
                    data.append('lstArchivosDatosPersonales', $(`#txt_dtsPer_ImagenPersona`)[0].files.length > 0 ? $(`#txt_dtsPer_ImagenPersona`)[0].files[i] : null);
                }
                //#endregion
                //#region ESPIROMETRÍA
                for (let i = 0; i < $(`#txt_esp_CargarEspirometria`)[0].files.length; i++) {
                    data.append('lstArchivosEspirometria', $(`#txt_esp_CargarEspirometria`)[0].files.length > 0 ? $(`#txt_esp_CargarEspirometria`)[0].files[i] : null);
                }
                //#endregion
                //#region AUDIOMETRIA
                for (let i = 0; i < $(`#txt_CargarAudiometria`)[0].files.length; i++) {
                    data.append('lstArchivosAudiometria', $(`#txt_CargarAudiometria`)[0].files.length > 0 ? $(`#txt_CargarAudiometria`)[0].files[i] : null);
                }
                //#endregion
                //#region ELECTROCARDIOGRAMA 12 DERIVACIONES
                for (let i = 0; i < $(`#txt_eleDer_CargarElectrocardiograma`)[0].files.length; i++) {
                    data.append('lstArchivosElectrocardiograma', $(`#txt_eleDer_CargarElectrocardiograma`)[0].files.length > 0 ? $(`#txt_eleDer_CargarElectrocardiograma`)[0].files[i] : null);
                }
                //#endregion
                //#region RADIOGRAFIAS - TORAX Y COLUMNA LUMBAR DOS POSICIONES
                for (let i = 0; i < $(`#txt_CargarRadiografia_radTCLP_Conclusiones`)[0].files.length; i++) {
                    data.append('lstArchivosRadiografias', $(`#txt_CargarRadiografia_radTCLP_Conclusiones`)[0].files.length > 0 ? $(`#txt_CargarRadiografia_radTCLP_Conclusiones`)[0].files[i] : null);
                }
                //#endregion
                //#region LABORATORIO
                for (let i = 0; i < $(`#txt_CargarDocLaboratorio`)[0].files.length; i++) {
                    data.append('lstArchivosLaboratorio', $(`#txt_CargarDocLaboratorio`)[0].files.length > 0 ? $(`#txt_CargarDocLaboratorio`)[0].files[i] : null);
                }
                //#endregion
                //#region DOCUMENTOS ADJUNTOS
                for (let i = 0; i < $(`#txt_CargarDocumentosAdjuntos`)[0].files.length; i++) {
                    data.append('lstArchivosDocumentosAdjuntos', $(`#txt_CargarDocumentosAdjuntos`)[0].files.length > 0 ? $(`#txt_CargarDocumentosAdjuntos`)[0].files[i] : null);
                }
                //#endregion
                return data;
            }
        }

        function fncShowCtrlsPrincipales() {
            divtblS_SO_HistorialesClinicos.css("display", "inline");
            btnFiltroBuscar.css("display", "inline");
            btnShowCEHistorialClinico.css("display", "inline");
            btnCECancelar.css("display", "none");
            divCEHistorialClinico.css("display", "none");
            btnGuardarParcial.css("display", "none");
        }

        function fncHideCtrlsPrincipales() {
            divtblS_SO_HistorialesClinicos.css("display", "none");
            btnFiltroBuscar.css("display", "none");
            btnShowCEHistorialClinico.css("display", "none");
            btnCECancelar.css("display", "inline");
            divCEHistorialClinico.css("display", "inline");
            btnGuardarParcial.css("display", "inline");
        }

        function fncFillCbos() {
            cbo_dtsPer_EmpresaID.fillCombo("FillCboEmpresas", {}, false);
            cbo_dtsPer_EmpresaID.select2({ width: "100%" });

            cbo_dtsPer_CCID.fillCombo("FillCboCC", {}, false);
            cbo_dtsPer_CCID.select2({ width: "100%" });

            cbo_dtsPer_EstadoCivil.fillCombo("FillCboEstadoCivil", {}, false);
            cbo_dtsPer_EstadoCivil.select2({ width: "100%" });

            cbo_dtsPer_TipoSanguineo.fillCombo("FillCboTipoSanguineo", {}, false);
            cbo_dtsPer_TipoSanguineo.select2({ width: "100%" });

            cbo_dtsPer_Escolaridad.fillCombo("FillCboEscolaridades", {}, false);
            cbo_dtsPer_Escolaridad.select2({ width: "100%" });

            cbo_dtsPer_Sexo.select2({ width: "100%" });

            cbo_estGab_TipoSanguineoID.fillCombo("FillCboTipoSanguineo", {}, false);
            cbo_estGab_TipoSanguineoID.select2({ width: "100%" });

            $("#cbo_estGab_MarcaDosisID").fillCombo("FillCboMarcasCovid19", {}, false);
            $("#cbo_estGab_MarcaDosisID").select2({ width: "100%" });
        }

        function fncLimpiarFormulario() {
            $('input[type="text"]').val("");

            btnCEHistorialClinico.attr("data-id", 0);
            btnCEObservacionMedicoCP.attr("data-id", 0);
            btnCargarDocumentoFirmadoMedicoExterno.attr("data-id", 0);

            //#region DATOS PERSONALES
            txt_dtsPer_ImagenPersona.val("");
            txt_dtsPer_FechaHora.val(moment(fechaActualInputDate).format("YYYY-MM-DD"));
            txt_dtsPer_FechaNac.val(moment(fechaActualInputDate).format("YYYY-MM-DD"));
            cbo_dtsPer_EmpresaID[0].selectedIndex = 0;
            cbo_dtsPer_EmpresaID.trigger("change");
            cbo_dtsPer_CCID[0].selectedIndex = 0;
            cbo_dtsPer_CCID.trigger("change");
            cbo_dtsPer_EstadoCivil[0].selectedIndex = 0;
            cbo_dtsPer_EstadoCivil.trigger("change");
            cbo_dtsPer_TipoSanguineo[0].selectedIndex = 0;
            cbo_dtsPer_TipoSanguineo.trigger("change");
            cbo_dtsPer_Escolaridad[0].selectedIndex = 0;
            cbo_dtsPer_Escolaridad.trigger("change");
            cbo_dtsPer_Sexo[0].selectedIndex = 0;
            cbo_dtsPer_Sexo.trigger("change");
            //#endregion
            //#region MOTIVO DE LA EVALUACION
            chk_motEva_esIngreso.prop("checked", false);
            chk_motEva_esRetiro.prop("checked", false);
            chk_motEva_esEvaOpcional.prop("checked", false);
            chk_motEva_esPostIncapacidad.prop("checked", false);
            chk_motEva_esReubicacion.prop("checked", false);
            //#endregion
            //#region ACCIDENTES Y ENFERMEDADES DE TRABAJO
            txt_accET_AbandonoTrabajo.val("No");
            txt_accET_IncapacidadFrecuente.val("No");
            txt_accET_Prolongadas.val("No");
            //#endregion
            //#region USO DE ELEMENTOS DE PROTECCIÓN PERSONAL
            chk_usoElePP_esActual.prop("checked", true);
            chk_usoElePP_esCasco.prop("checked", true);
            chk_usoElePP_esTapaboca.prop("checked", true);
            chk_usoElePP_esGafas.prop("checked", true);
            chk_usoElePP_esRespirador.prop("checked", true);
            chk_usoElePP_esBotas.prop("checked", true);
            chk_usoElePP_esAuditivos.prop("checked", true);
            chk_usoElePP_esOverol.prop("checked", true);
            chk_usoElePP_esGuantes.prop("checked", true);
            //#endregion
            //#region ANTECEDENTES FAMILIARES
            txt_antFam_esTuberculosis.val("No");
            txt_antFam_esHTA.val("No");
            txt_antFam_esDiabetes.val("No");
            txt_antFam_esACV.val("No");
            txt_antFam_esInfarto.val("No");
            txt_antFam_esAsma.val("No");
            txt_antFam_esAlergias.val("No");
            txt_antFam_esMental.val("No");
            txt_antFam_esCancer.val("No");
            //#endregion
            //#region ANTECEDENTES PERSONALES PATOLÓGICOS
            txt_antPerPat_ObservacionesPat.val("");
            //#endregion
            //#region INTERROGATORIO POR APARATOS Y SISTEMAS
            chk_intApaSis_esRespiratorio.prop("checked", false);
            chk_intApaSis_esDigestivo.prop("checked", false);
            chk_intApaSis_esCardiovascular.prop("checked", false);
            chk_intApaSis_esNervioso.prop("checked", false);
            chk_intApaSis_esUrinario.prop("checked", false);
            chk_intApaSis_esEndocrino.prop("checked", false);
            chk_intApaSis_esPsiquiatrico.prop("checked", false);
            chk_intApaSis_esEsqueletico.prop("checked", false);
            chk_intApaSis_esAudicion.prop("checked", false);
            chk_intApaSis_esVision.prop("checked", false);
            chk_intApaSis_esOlfato.prop("checked", false);
            chk_intApaSis_esTacto.prop("checked", false);
            txt_intApaSis_ObservacionesPat.val("");
            //#endregion
            //#region PADECIMIENTOS ACTUALES
            txt_padAct_PadActuales.val("");
            //#endregion
            //#region EXPLORACIÓN FÍSICA-CABEZA
            cbo_expFC_Craneo[0] = selectedIndex = 0;
            cbo_expFC_Craneo.trigger("change");
            cbo_expFC_Parpados[0] = selectedIndex = 0;
            cbo_expFC_Parpados.trigger("change");
            cbo_expFC_Conjutiva[0] = selectedIndex = 0;
            cbo_expFC_Conjutiva.trigger("change");
            cbo_expFC_Reflejos[0] = selectedIndex = 0;
            cbo_expFC_Reflejos.trigger("change");
            cbo_expFC_FosasNasales[0] = selectedIndex = 0;
            cbo_expFC_FosasNasales.trigger("change");
            cbo_expFC_Boca[0] = selectedIndex = 0;
            cbo_expFC_Boca.trigger("change");
            cbo_expFC_Amigdalas[0] = selectedIndex = 0;
            cbo_expFC_Amigdalas.trigger("change");
            cbo_expFC_Dentadura[0] = selectedIndex = 0;
            cbo_expFC_Dentadura.trigger("change");
            cbo_expFC_Encias[0] = selectedIndex = 0;
            cbo_expFC_Encias.trigger("change");
            cbo_expFC_Cuello[0] = selectedIndex = 0;
            cbo_expFC_Cuello.trigger("change");
            cbo_expFC_Tiroides[0] = selectedIndex = 0;
            cbo_expFC_Tiroides.trigger("change");
            cbo_expFC_Ganglios[0] = selectedIndex = 0;
            cbo_expFC_Ganglios.trigger("change");
            cbo_expFC_Oidos[0] = selectedIndex = 0;
            cbo_expFC_Oidos.trigger("change");
            cbo_expFC_Otros[0] = selectedIndex = 0;
            cbo_expFC_Otros.trigger("change");
            txt_expFC_Observaciones.val("");
            //#endregion
            //#region EXPLORACIÓN FÍSICA-AGUDEZA VISUAL
            txt_expFAV_Observaciones.val("");
            //#endregion
            //#region EXPLORACIÓN FÍSICA-TORAX, ABDOMEN, TRONCO Y EXTREMIDADES
            chk_expFTATE_esCamposPulmonares.prop("checked", false);
            chk_expFTATE_esCamposPulmonares.trigger("change");
            chk_expFTATE_esPuntosDolorosos.prop("checked", false);
            chk_expFTATE_esPuntosDolorosos.trigger("change");
            chk_expFTATE_esGenitales.prop("checked", false);
            chk_expFTATE_esGenitales.trigger("change");
            chk_expFTATE_esRuidosCardiacos.prop("checked", false);
            chk_expFTATE_esRuidosCardiacos.trigger("change");
            chk_expFTATE_esHallusValgus.prop("checked", false);
            chk_expFTATE_esHallusValgus.trigger("change");
            chk_expFTATE_esHerniasUmbili.prop("checked", false);
            chk_expFTATE_esHerniasUmbili.trigger("change");
            chk_expFTATE_esAreaRenal.prop("checked", false);
            chk_expFTATE_esAreaRenal.trigger("change");
            chk_expFTATE_esVaricocele.prop("checked", false);
            chk_expFTATE_esVaricocele.trigger("change");
            chk_expFTATE_esGrandulasMamarias.prop("checked", false);
            chk_expFTATE_esGrandulasMamarias.trigger("change");
            chk_expFTATE_esColumnaVertebral.prop("checked", false);
            chk_expFTATE_esColumnaVertebral.trigger("change");
            chk_expFTATE_esPiePlano.prop("checked", false);
            chk_expFTATE_esPiePlano.trigger("change");
            chk_expFTATE_esVarices.prop("checked", false);
            chk_expFTATE_esVarices.trigger("change");
            chk_expFTATE_esMiembrosSup.prop("checked", false);
            chk_expFTATE_esMiembrosSup.trigger("change");
            chk_expFTATE_esParedAbdominal.prop("checked", false);
            chk_expFTATE_esParedAbdominal.trigger("change");
            chk_expFTATE_esAnillosInguinales.prop("checked", false);
            chk_expFTATE_esAnillosInguinales.trigger("change");
            chk_expFTATE_esMiembrosInf.prop("checked", false);
            chk_expFTATE_esMiembrosInf.trigger("change");
            chk_expFTATE_esTatuajes.prop("checked", false);
            chk_expFTATE_esTatuajes.trigger("change");
            chk_expFTATE_esVisceromegalias.prop("checked", false);
            chk_expFTATE_esVisceromegalias.trigger("change");
            chk_expFTATE_esMarcha.prop("checked", false);
            chk_expFTATE_esMarcha.trigger("change");
            chk_expFTATE_esHerniasInguinales.prop("checked", false);
            chk_expFTATE_esHerniasInguinales.trigger("change");
            chk_expFTATE_esHombrosDolorosos.prop("checked", false);
            chk_expFTATE_esHombrosDolorosos.trigger("change");
            chk_expFTATE_esQuistes.prop("checked", false);
            chk_expFTATE_esQuistes.trigger("change");
            txt_expFTATE_Observaciones.val("");
            chk_expFTATE_MS_HombroDer_esFlexion.prop("checked", false);
            chk_expFTATE_MS_HombroDer_esFlexion.trigger("change");
            chk_expFTATE_MS_HombroIzq_esFlexion.prop("checked", false);
            chk_expFTATE_MS_HombroIzq_esFlexion.trigger("change");
            chk_expFTATE_MS_CodoDer_esFlexion.prop("checked", false);
            chk_expFTATE_MS_CodoDer_esFlexion.trigger("change");
            chk_expFTATE_MS_CodoIzq_esFlexion.prop("checked", false);
            chk_expFTATE_MS_CodoIzq_esFlexion.trigger("change");
            chk_expFTATE_MS_MunecaDer_esFlexion.prop("checked", false);
            chk_expFTATE_MS_MunecaDer_esFlexion.trigger("change");
            chk_expFTATE_MS_MunecaIzq_esFlexion.prop("checked", false);
            chk_expFTATE_MS_MunecaIzq_esFlexion.trigger("change");
            chk_expFTATE_MS_DedosDer_esFlexion.prop("checked", false);
            chk_expFTATE_MS_DedosDer_esFlexion.trigger("change");
            chk_expFTATE_MS_DedosIzq_esFlexion.prop("checked", false);
            chk_expFTATE_MS_DedosIzq_esFlexion.trigger("change");
            chk_expFTATE_MS_HombroDer_esExtension.prop("checked", false);
            chk_expFTATE_MS_HombroDer_esExtension.trigger("change");
            chk_expFTATE_MS_HombroIzq_esExtension.prop("checked", false);
            chk_expFTATE_MS_HombroIzq_esExtension.trigger("change");
            chk_expFTATE_MS_CodoDer_esExtension.prop("checked", false);
            chk_expFTATE_MS_CodoDer_esExtension.trigger("change");
            chk_expFTATE_MS_CodoIzq_esExtension.prop("checked", false);
            chk_expFTATE_MS_CodoIzq_esExtension.trigger("change");
            chk_expFTATE_MS_MunecaDer_esExtension.prop("checked", false);
            chk_expFTATE_MS_MunecaDer_esExtension.trigger("change");
            chk_expFTATE_MS_MunecaIzq_esExtension.prop("checked", false);
            chk_expFTATE_MS_MunecaIzq_esExtension.trigger("change");
            chk_expFTATE_MS_DedosDer_esExtension.prop("checked", false);
            chk_expFTATE_MS_DedosDer_esExtension.trigger("change");
            chk_expFTATE_MS_DedosIzq_esExtension.prop("checked", false);
            chk_expFTATE_MS_DedosIzq_esExtension.trigger("change");
            chk_expFTATE_MS_HombroDer_esAbduccion.prop("checked", false);
            chk_expFTATE_MS_HombroDer_esAbduccion.trigger("change");
            chk_expFTATE_MS_HombroIzq_esAbduccion.prop("checked", false);
            chk_expFTATE_MS_HombroIzq_esAbduccion.trigger("change");
            chk_expFTATE_MS_CodoDer_esAbduccion.prop("checked", false);
            chk_expFTATE_MS_CodoDer_esAbduccion.trigger("change");
            chk_expFTATE_MS_CodoIzq_esAbduccion.prop("checked", false);
            chk_expFTATE_MS_CodoIzq_esAbduccion.trigger("change");
            chk_expFTATE_MS_MunecaDer_esAbduccion.prop("checked", false);
            chk_expFTATE_MS_MunecaDer_esAbduccion.trigger("change");
            chk_expFTATE_MS_MunecaIzq_esAbduccion.prop("checked", false);
            chk_expFTATE_MS_MunecaIzq_esAbduccion.trigger("change");
            chk_expFTATE_MS_DedosDer_esAbduccion.prop("checked", false);
            chk_expFTATE_MS_DedosDer_esAbduccion.trigger("change");
            chk_expFTATE_MS_DedosIzq_esAbduccion.prop("checked", false);
            chk_expFTATE_MS_DedosIzq_esAbduccion.trigger("change");
            chk_expFTATE_MS_HombroDer_esAduccion.prop("checked", false);
            chk_expFTATE_MS_HombroDer_esAduccion.trigger("change");
            chk_expFTATE_MS_HombroIzq_esAduccion.prop("checked", false);
            chk_expFTATE_MS_HombroIzq_esAduccion.trigger("change");
            chk_expFTATE_MS_MunecaDer_esAduccion.prop("checked", false);
            chk_expFTATE_MS_MunecaDer_esAduccion.trigger("change");
            chk_expFTATE_MS_MunecaIzq_esAduccion.prop("checked", false);
            chk_expFTATE_MS_MunecaIzq_esAduccion.trigger("change");
            chk_expFTATE_MS_DedosDer_esAduccion.prop("checked", false);
            chk_expFTATE_MS_DedosDer_esAduccion.trigger("change");
            chk_expFTATE_MS_DedosIzq_esAduccion.prop("checked", false);
            chk_expFTATE_MS_DedosIzq_esAduccion.trigger("change");
            chk_expFTATE_MS_HombroDer_esRotInterna.prop("checked", false);
            chk_expFTATE_MS_HombroDer_esRotInterna.trigger("change");
            chk_expFTATE_MS_HombroIzq_esRotInterna.prop("checked", false);
            chk_expFTATE_MS_HombroIzq_esRotInterna.trigger("change");
            chk_expFTATE_MS_MunecaDer_esRotInterna.prop("checked", false);
            chk_expFTATE_MS_MunecaDer_esRotInterna.trigger("change");
            chk_expFTATE_MS_MunecaIzq_esRotInterna.prop("checked", false);
            chk_expFTATE_MS_MunecaIzq_esRotInterna.trigger("change");
            chk_expFTATE_MS_DedosDer_esRotInterna.prop("checked", false);
            chk_expFTATE_MS_DedosDer_esRotInterna.trigger("change");
            chk_expFTATE_MS_DedosIzq_esRotInterna.prop("checked", false);
            chk_expFTATE_MS_DedosIzq_esRotInterna.trigger("change");
            chk_expFTATE_MS_HombroDer_esRotExterna.prop("checked", false);
            chk_expFTATE_MS_HombroDer_esRotExterna.trigger("change");
            chk_expFTATE_MS_HombroIzq_esRotExterna.prop("checked", false);
            chk_expFTATE_MS_HombroIzq_esRotExterna.trigger("change");
            chk_expFTATE_MS_MunecaDer_esRotExterna.prop("checked", false);
            chk_expFTATE_MS_MunecaDer_esRotExterna.trigger("change");
            chk_expFTATE_MS_MunecaIzq_RotExterna.prop("checked", false);
            chk_expFTATE_MS_MunecaIzq_RotExterna.trigger("change");
            chk_expFTATE_MS_DedosDer_RotExterna.prop("checked", false);
            chk_expFTATE_MS_DedosDer_RotExterna.trigger("change");
            chk_expFTATE_MS_DedosIzq_RotExterna.prop("checked", false);
            chk_expFTATE_MS_DedosIzq_RotExterna.trigger("change");
            chk_expFTATE_MS_CodoDer_esPronacion.prop("checked", false);
            chk_expFTATE_MS_CodoDer_esPronacion.trigger("change");
            chk_expFTATE_MS_CodoIzq_esPronacion.prop("checked", false);
            chk_expFTATE_MS_CodoIzq_esPronacion.trigger("change");
            chk_expFTATE_MS_MunecaDer_esPronacion.prop("checked", false);
            chk_expFTATE_MS_MunecaDer_esPronacion.trigger("change");
            chk_expFTATE_MS_MunecaIzq_esPronacion.prop("checked", false);
            chk_expFTATE_MS_MunecaIzq_esPronacion.trigger("change");
            chk_expFTATE_MS_CodoDer_esSupinacion.prop("checked", false);
            chk_expFTATE_MS_CodoDer_esSupinacion.trigger("change");
            chk_expFTATE_MS_CodoIzq_esSupinacion.prop("checked", false);
            chk_expFTATE_MS_CodoIzq_esSupinacion.trigger("change");
            chk_expFTATE_MS_MunecaDer_esSupinacion.prop("checked", false);
            chk_expFTATE_MS_MunecaDer_esSupinacion.trigger("change");
            chk_expFTATE_MS_MunecaIzq_esSupinacion.prop("checked", false);
            chk_expFTATE_MS_MunecaIzq_esSupinacion.trigger("change");
            chk_expFTATE_MS_MunecaDer_esDesvUlnar.prop("checked", false);
            chk_expFTATE_MS_MunecaDer_esDesvUlnar.trigger("change");
            chk_expFTATE_MS_MunecaIzq_esDesvUlnar.prop("checked", false);
            chk_expFTATE_MS_MunecaIzq_esDesvUlnar.trigger("change");
            chk_expFTATE_MS_MunecaDer_esDesvRadial.prop("checked", false);
            chk_expFTATE_MS_MunecaDer_esDesvRadial.trigger("change");
            chk_expFTATE_MS_MunecaIzq_esDesvRadial.prop("checked", false);
            chk_expFTATE_MS_MunecaIzq_esDesvRadial.trigger("change");
            chk_expFTATE_MS_MunecaDer_esOponencia.prop("checked", false);
            chk_expFTATE_MS_MunecaDer_esOponencia.trigger("change");
            chk_expFTATE_MS_MunecaIzq_esOponencia.prop("checked", false);
            chk_expFTATE_MS_MunecaIzq_esOponencia.trigger("change");
            chk_expFTATE_MS_DedosDer_esOponencia.prop("checked", false);
            chk_expFTATE_MS_DedosDer_esOponencia.trigger("change");
            chk_expFTATE_MS_DedosIzq_esOponencia.prop("checked", false);
            chk_expFTATE_MS_DedosIzq_esOponencia.trigger("change");
            chk_expFTATE_MI_CaderaDer_esFlexion.prop("checked", false);
            chk_expFTATE_MI_CaderaDer_esFlexion.trigger("change");
            chk_expFTATE_MI_CaderaIzq_esFlexion.prop("checked", false);
            chk_expFTATE_MI_CaderaIzq_esFlexion.trigger("change");
            chk_expFTATE_MI_RodillasDer_esFlexion.prop("checked", false);
            chk_expFTATE_MI_RodillasDer_esFlexion.trigger("change");
            chk_expFTATE_MI_RodillasIzq_esFlexion.prop("checked", false);
            chk_expFTATE_MI_RodillasIzq_esFlexion.trigger("change");
            chk_expFTATE_MI_CllPieDer_esFlexion.prop("checked", false);
            chk_expFTATE_MI_CllPieDer_esFlexion.trigger("change");
            chk_expFTATE_MI_CllPieIzq_esFlexion.prop("checked", false);
            chk_expFTATE_MI_CllPieIzq_esFlexion.trigger("change");
            chk_expFTATE_MI_DedosDer_esFlexion.prop("checked", false);
            chk_expFTATE_MI_DedosDer_esFlexion.trigger("change");
            chk_expFTATE_MI_DedosIzq_esFlexion.prop("checked", false);
            chk_expFTATE_MI_DedosIzq_esFlexion.trigger("change");
            chk_expFTATE_MI_CaderaDer_esExtension.prop("checked", false);
            chk_expFTATE_MI_CaderaDer_esExtension.trigger("change");
            chk_expFTATE_MI_CaderaIzq_esExtension.prop("checked", false);
            chk_expFTATE_MI_CaderaIzq_esExtension.trigger("change");
            chk_expFTATE_MI_RodillasDer_esExtension.prop("checked", false);
            chk_expFTATE_MI_RodillasDer_esExtension.trigger("change");
            chk_expFTATE_MI_RodillasIzq_esExtension.prop("checked", false);
            chk_expFTATE_MI_RodillasIzq_esExtension.trigger("change");
            chk_expFTATE_MI_CllPieDer_esExtension.prop("checked", false);
            chk_expFTATE_MI_CllPieDer_esExtension.trigger("change");
            chk_expFTATE_MI_CllPieIzq_esExtension.prop("checked", false);
            chk_expFTATE_MI_CllPieIzq_esExtension.trigger("change");
            chk_expFTATE_MI_DedosDer_esExtension.prop("checked", false);
            chk_expFTATE_MI_DedosDer_esExtension.trigger("change");
            chk_expFTATE_MI_DedosIzq_esExtension.prop("checked", false);
            chk_expFTATE_MI_DedosIzq_esExtension.trigger("change");
            chk_expFTATE_MI_CaderaDer_esAbduccion.prop("checked", false);
            chk_expFTATE_MI_CaderaDer_esAbduccion.trigger("change");
            chk_expFTATE_MI_CaderaIzq_esAbduccion.prop("checked", false);
            chk_expFTATE_MI_CaderaIzq_esAbduccion.trigger("change");
            chk_expFTATE_MI_CllPieDer_esAbduccion.prop("checked", false);
            chk_expFTATE_MI_CllPieDer_esAbduccion.trigger("change");
            chk_expFTATE_MI_CllPieIzq_esAbduccion.prop("checked", false);
            chk_expFTATE_MI_CllPieIzq_esAbduccion.trigger("change");
            chk_expFTATE_MI_DedosDer_esAbduccion.prop("checked", false);
            chk_expFTATE_MI_DedosDer_esAbduccion.trigger("change");
            chk_expFTATE_MI_DedosIzq_esAbduccion.prop("checked", false);
            chk_expFTATE_MI_DedosIzq_esAbduccion.trigger("change");
            chk_expFTATE_MI_CaderaDer_esAduccion.prop("checked", false);
            chk_expFTATE_MI_CaderaDer_esAduccion.trigger("change");
            chk_expFTATE_MI_CaderaIzq_esAduccion.prop("checked", false);
            chk_expFTATE_MI_CaderaIzq_esAduccion.trigger("change");
            chk_expFTATE_MI_CllPieDer_esAduccion.prop("checked", false);
            chk_expFTATE_MI_CllPieDer_esAduccion.trigger("change");
            chk_expFTATE_MI_CllPieIzq_esAduccion.prop("checked", false);
            chk_expFTATE_MI_CllPieIzq_esAduccion.trigger("change");
            chk_expFTATE_MI_DedosDer_esAduccion.prop("checked", false);
            chk_expFTATE_MI_DedosDer_esAduccion.trigger("change");
            chk_expFTATE_MI_DedosIzq_esAduccion.prop("checked", false);
            chk_expFTATE_MI_DedosIzq_esAduccion.trigger("change");
            chk_expFTATE_MI_CaderaDer_esRotInterna.prop("checked", false);
            chk_expFTATE_MI_CaderaDer_esRotInterna.trigger("change");
            chk_expFTATE_MI_CaderaIzq_esRotInterna.prop("checked", false);
            chk_expFTATE_MI_CaderaIzq_esRotInterna.trigger("change");
            chk_expFTATE_MI_RodillasDer_esRotInterna.prop("checked", false);
            chk_expFTATE_MI_RodillasDer_esRotInterna.trigger("change");
            chk_expFTATE_MI_RodillasIzq_esRotInterna.prop("checked", false);
            chk_expFTATE_MI_RodillasIzq_esRotInterna.trigger("change");
            chk_expFTATE_MI_CllPieDer_esRotInterna.prop("checked", false);
            chk_expFTATE_MI_CllPieDer_esRotInterna.trigger("change");
            chk_expFTATE_MI_CllPieIzq_esRotInterna.prop("checked", false);
            chk_expFTATE_MI_CllPieIzq_esRotInterna.trigger("change");
            chk_expFTATE_MI_CaderaDer_esRotExterna.prop("checked", false);
            chk_expFTATE_MI_CaderaDer_esRotExterna.trigger("change");
            chk_expFTATE_MI_CaderaIzq_esRotExterna.prop("checked", false);
            chk_expFTATE_MI_CaderaIzq_esRotExterna.trigger("change");
            chk_expFTATE_MI_RodillasDer_esRotExterna.prop("checked", false);
            chk_expFTATE_MI_RodillasDer_esRotExterna.trigger("change");
            chk_expFTATE_MI_RodillasIzq_esRotExterna.prop("checked", false);
            chk_expFTATE_MI_RodillasIzq_esRotExterna.trigger("change");
            chk_expFTATE_MI_CllPieDer_esRotExterna.prop("checked", false);
            chk_expFTATE_MI_CllPieDer_esRotExterna.trigger("change");
            chk_expFTATE_MI_CllPieIzq_esRotExterna.prop("checked", false);
            chk_expFTATE_MI_CllPieIzq_esRotExterna.trigger("change");
            chk_expFTATE_MI_CllPieDer_esInversion.prop("checked", false);
            chk_expFTATE_MI_CllPieDer_esInversion.trigger("change");
            chk_expFTATE_MI_CllPieIzq_esInversion.prop("checked", false);
            chk_expFTATE_MI_CllPieIzq_esInversion.trigger("change");
            chk_expFTATE_MI_CllPieDer_esEversion.prop("checked", false);
            chk_expFTATE_MI_CllPieDer_esEversion.trigger("change");
            chk_expFTATE_MI_CllPieIzq_esEversion.prop("checked", false);
            chk_expFTATE_MI_CllPieIzq_esEversion.trigger("change");
            txt_expFTATE_MS_MI_Observaciones.val("");
            //#endregion
            //#region EXPLORACIÓN FÍSICA-DATOS GENERALES
            txt_expFDT_Observaciones.val("");
            //#endregion
            //#region ESTUDIOS DE GABINETE
            cbo_estGab_TipoSanguineoID[0].selectedIndex = 0;
            cbo_estGab_TipoSanguineoID.trigger("change");
            cbo_estGab_Antidoping[0].selectedIndex = 0;
            cbo_estGab_Antidoping.trigger("change");
            cbo_estGab_MarcaDosisID[0].selectedIndex = 0;
            cbo_estGab_MarcaDosisID.trigger("change");
            txt_estGab_Recomendaciones.val("");
            //#endregion
            //#region ESPIROMETRÍA
            txt_esp_CargarEspirometria.val("");
            //#endregion
            //#region AUDIOMETRÍA
            txt_CargarAudiometria.val("");
            cbo_aud_Diagnostico[0].selectedIndex = 0;
            cbo_aud_Diagnostico.trigger("change");
            //#endregion
            //#region ELECTROCARDIOGRAMA 12 DERIVACIONES
            txt_eleDer_CargarElectrocardiograma.val("");
            txt_eleDer_Interpretacion.val("");
            //#endregion
            //#region RADIOGRAFIAS - TORAX Y COLUMNA LUMBAR DOS POSICIONES
            txt_radTCLP_Conclusiones.val("");
            txt_CargarRadiografia_radTCLP_Conclusiones.val("");
            //#endregion
            //#region LABORATORIO
            txt_CargarDocLaboratorio.val("");
            //#endregion
            //#region CERTIFICADO MEDICO
            txt_certMed_CertificadoMedico.val("");
            cbo_certMed_AptitudID[0].selectedIndex = 0;
            cbo_certMed_AptitudID.trigger("change");
            //#endregion
            //#region RECOMENDACIÓN
            txt_recom_Recomendaciones.val("");
            //#endregion
        }

        function fncGetUltimoFolioHistorialClinico() {
            axios.post("GetUltimoFolioHistorialClinico").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    txt_dtsPer_Folio.val(response.data.folio);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetEdadPaciente() {
            let obj = new Object();
            obj = {
                fechaNac: txt_dtsPer_FechaNac.val()
            }
            axios.post("GetEdadPaciente", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    txt_dtsPer_Edad.val(response.data.edadPaciente)
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncEliminarHistorialClinico(idHistorialClinico) {
            if (idHistorialClinico <= 0) {
                Alert2Warning("Ocurrió un error al eliminar el historial clinico seleccionado.");
            } else if (idHistorialClinico > 0) {
                let obj = new Object();
                obj = {
                    idHistorialClinico: idHistorialClinico
                }
                axios.post("EliminarHistorialClinico", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(response.data.message);
                        fncGetHistorialesClinicos();
                        btnCECancelar.trigger("change");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGetReporte(id) {
            var path = `/Reportes/Vista.aspx?idReporte=244&id=${id}`;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }
        //#endregion

        //#region CRUD OBSERVACION MEDICO INTERNO
        function fncGetObservacionMedicoInternoCP(idHC) {
            if (idHC > 0) {
                let obj = new Object();
                obj = {
                    idHC: idHC
                }
                axios.post("GetObservacionMedicoInternoCP", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        cbo_cp_aptitudIDCP.val(response.data.objHC.cp_aptitudIDCP);
                        cbo_cp_aptitudIDCP.trigger("change");
                        txt_cp_observacionMedicoCP.val(response.data.objHC.cp_observacionMedicoCP);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncEditarObservacionMedicoInternoCP(idHC) {
            if (idHC > 0) {
                let obj = new Object();
                obj = {
                    id: idHC,
                    cp_observacionMedicoCP: txt_cp_observacionMedicoCP.val(),
                    cp_aptitudIDCP: cbo_cp_aptitudIDCP.val()
                }
                const data = new FormData();
                data.append('obj', JSON.stringify(obj));
                for (let i = 0; i < $(`#txt_cp_archivosCP`)[0].files.length; i++) {
                    data.append('lstArchivos', $(`#txt_cp_archivosCP`)[0].files.length > 0 ? $(`#txt_cp_archivosCP`)[0].files[i] : null);
                }
                axios.post("EditarObservacionMedicoInternoCP", data).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        mdlObservacionesCP.modal("hide");
                        btnFiltroBuscar.trigger("click");
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }
        //#endregion

        //#region CRUD CARGAR DOCUMENTO FIRMADO MEDICO EXTERNO
        function fncCargarDocumentoFirmadoMedicoExterno(idHC) {
            if (idHC > 0) {
                let obj = new Object();
                obj = {
                    id: idHC
                }
                const data = new FormData();
                data.append('obj', JSON.stringify(obj));
                for (let i = 0; i < $(`#txt_hc_firmado_doctorExterno`)[0].files.length; i++) {
                    data.append('lstArchivos', $(`#txt_hc_firmado_doctorExterno`)[0].files.length > 0 ? $(`#txt_hc_firmado_doctorExterno`)[0].files[i] : null);
                }

                axios.post("CargarDocumentoFirmadoMedicoExterno", data).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        mdlShowMdlHCFirmado.modal("hide");
                        btnFiltroBuscar.trigger("click");
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGetRutaArchivo(idHC, tipoArchivo) {
            location.href = `/Administrativo/SaludOcupacional/DescargarArchivo?idHC=${idHC}&&tipoArchivo=${tipoArchivo}`;
        }

        function fncVerificarExisteDocumento(idHC, tipoArchivo) {
            let obj = new Object();
            obj = {
                idHC: idHC,
                tipoArchivo: tipoArchivo
            }
            axios.post("VerificarExisteDocumento", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (response.data.cantDocumentos > 0) {
                        switch (tipoArchivo) {
                            case 2:
                                btnDescargarDocumentoMedicoInterno.css("display", "inline");
                                break;
                            case 3:
                                btnDescargarHCFirmado.css("display", "inline");
                                break;
                            default:
                                break;
                        }
                    }
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }

    $(document).ready(() => {
        SaludOcupacional.HistorialClinico = new HistorialClinico();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();