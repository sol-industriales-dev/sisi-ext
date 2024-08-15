using Core.DTO.Enkontrol.Tablas.RH.Empleado;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos.Enkontrol;
using Core.DTO.RecursosHumanos.Reclutamientos;
using Core.DTO.RecursosHumanos.Starsoft;
using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using Core.Entity.Administrativo.RecursosHumanos.Reclutamientos;
using Core.Entity.RecursosHumanos.Catalogo;
using Core.Entity.RecursosHumanos.Reclutamientos;
using Core.Enum.RecursosHumanos.Reclutamientos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.RecursosHumanos.Reclutamientos
{
    public interface IReclutamientosDAO
    {
        #region SOLICITUDES
        List<SolicitudesDTO> GetSolicitudes(SolicitudesDTO objFiltro);

        bool CrearSolicitud(tblRH_REC_Solicitudes objSolicitud);

        bool ActualizarSolicitud(tblRH_REC_Solicitudes objSolicitud);

        bool EliminarSolicitud(int idSolicitud);

        bool ExisteSolicitudRelCandidato(int idSolicitud);

        List<Core.Entity.RecursosHumanos.Catalogo.tblRH_CatPuestos> FillCboPuestosSolicitudes(string cc);

        List<string> GetCategoriasRelPuesto(string _cc, string _strPuesto);
        #endregion

        #region GESTIÓN DE SOLICITUDES
        List<GestionSolicitudesDTO> GetGestionSolicitudes(GestionSolicitudesDTO objFiltro);

        bool RechazarSolicitud(GestionSolicitudesDTO objGestionSolicitud);
        #endregion

        #region GESTIÓN DE CANDIDATOS
        List<CandidatosDTO> GetCandidatos(CandidatosDTO objCandidatos);

        Dictionary<string, object> CrearCandidato(GestionCandidatosDTO objCandidato, HttpPostedFileBase objFile);

        Tuple<bool, string> ActualizarCandidato(GestionCandidatosDTO objCandidato, HttpPostedFileBase objFile);

        bool EliminarCandidato(int idCandidato);

        List<ComboDTO> FillFiltroCboPuestosDisponibles();

        EntrevistaInicialDTO GetEntrevistaInicial(EntrevistaInicialDTO objEntrevistaInicialDTO);

        Dictionary<string, object> CrearEditarEntrevistaInicial(EntrevistaInicialDTO objEntrevistaInicialDTO);

        Tuple<Stream, string> DescargarArchivoCV(int candidatoID);

        Dictionary<string, object> GetUsuarioEntrevistaActual();

        List<tblRH_CatEmpleados> getAutoCompleteEmpleadosBaja(string term);

        List<tblRH_CatEmpleados> getAutoCompleteCandidatos(string term);

        #endregion

        #region FASES
        List<FasesDTO> GetFases();

        bool CrearFase(tblRH_REC_Fases objFase);

        bool ActualizarFase(tblRH_REC_Fases objFase);

        bool EliminarFase(int idFase);
        #endregion

        #region FASES RELACIÓN ACTIVIDADES
        Dictionary<string, object> GetActividades(SegCandidatosDTO objSegCandidatosDTO);

        bool CrearActividad(tblRH_REC_Actividades objActividad);

        bool ActualizarActividad(tblRH_REC_Actividades objActividad);

        bool EliminarActividad(int idActividad);

        bool AsignarEncargadoFase(ActividadesDTO objActividadDTO);
        #endregion

        #region PUESTOS RELACIONADOS A FASES
        List<PuestosRelFasesDTO> GetPuestosRelFase(int idFase);

        bool CrearPuestoRelFase(tblRH_REC_PuestosRelFases objPuestoRelFase);

        bool EliminarPuestoRelFase(int idPuestoRelFase);

        bool ExistePuestoEnFase(int idFase, int idPuesto);
        #endregion

        #region SEGUIMIENTO DE CANDIDATOS
        List<SegCandidatosDTO> GetSegCandidatos(SegCandidatosDTO objFiltro);

        List<SegCandidatosDTO> GetSegDetCandidatos(SegCandidatosDTO objFiltro);

        bool CrearEditarSegCandidatos(SegCandidatosDTO objSegCandidatoDTO);

        bool CrearEditarComentarioActividad(SegCandidatosDTO objSegCandidatoDTO);

        string GetObservacionActividad(SegCandidatosDTO objSegCandidatoDTO);

        bool CrearArchivoActividad(SegCandidatosDTO objSegCandidatoDTO, List<HttpPostedFileBase> lstFiles);

        List<ArchivosDTO> GetArchivosActividadesRelFase(SegCandidatosDTO objSegCandidatoDTO);

        string GetArchivoEvidencia(int id_file);

        bool EliminarArchivoActividad(int idArchivo);

        bool CrearEditarCalificacionActividad(SegCandidatosDTO objSegCandidatoDTO);

        Dictionary<string, object> GetFasesAutorizadas(int idPuesto);

        Dictionary<string, object> NotificarActividad(int idCandidato, int idActividad, int estatus, int? idNotificante);

        List<tblRH_CatEmpleados> getCatUsuariosGeneral(string term);

        bool SetNotiEstatusActividad(int idCandidato, int idFase, int idActividad);

        #endregion

        #region EMPLEADOS EK
        List<EmpleadosDTO> GetEmpleadosEK(List<string> cc, List<string> lstEstatusEmpleado);

        Dictionary<string, object> CambiarContratable(string claveEmpleado, string esContratable);

        Dictionary<string, object> CrearEditarInformacionEmpleado(EmpleadosDTO objEmpleadoDTO, GeneralesContactoDTO objGenContactoDTO, BeneficiariosDTO objBeneficiariosDTO, ContEmergenciasDTO objContEmergenciasDTO, CompaniaDTO objCompaniaDTO, List<FamiliaresDTO> lstFamiliares, UniformesDTO objUniforme, NuevoTabuladorDTO objTabulador, ContratoDTO objContrato, InfoEmpleadoPeruDTO objDatosPeru);

        bool EliminarEmpleado(int claveEmpleado);

        List<FamiliaresDTO> GetFamiliares(int clave_empleado);

        List<ContratoDTO> GetContratos(int clave_empleado);

        Dictionary<string, object> CrearEditarInformacionFamiliar(FamiliaresDTO objFamiliarDTO);

        bool EliminarFamiliar(int idFamiliar, int clave_empleado);

        Dictionary<string, object> EliminarContrato(int id_contrato_empleado);

        List<ComboDTO> FillCboCandidatosAprobados();

        List<ComboDTO> FillCboParentesco();

        List<ComboDTO> FillCboTipoSangre();

        List<ComboDTO> FillCboTipoCasa();

        List<dynamic> GetDatosEmpleadoDocumentos(int claveEmpleado);

        UniformesDTO GetUniformes(int claveEmpleado);

        Dictionary<string,object> CrearEditarUniforme(UniformesDTO objUniforme);

        bool EliminarUniforme(int idUniforme);

        List<ArchivosDTO> GetArchivoExamenMedico(int claveEmpleado);

        bool CrearArchivoExamenMedico(ArchivosDTO objArchivoDTO, HttpPostedFileBase objFile);

        bool EliminarExamenMedico(int idExamenMedico);

        List<ArchivosDTO> GetArchivoMaquinaria(int claveEmpleado);

        bool CrearArchivoMaquinaria(ArchivosDTO objArchivoDTO, HttpPostedFileBase objFile);

        bool EliminarMaquinaria(int idMaquinaria);

        List<TabuladoresDTO> GetTabuladores(TabuladoresDTO objTabDTO);

        Dictionary<string, object> CrearTabuladorPuesto(TabuladoresPuestoDTO objTabuladorPuestoDTO);

        Dictionary<string, object> CrearTabulador(TabuladoresDTO objTabuladorDTO);

        Dictionary<string, object> CambiarFechaCambioTabulador(int id, DateTime fecha_cambio, int claveEmpleado);

        List<ComboDTO> FillCboBancos();

        Dictionary<string, object> GetReporteSegCandidatos(ReporteSegCandidatosDTO objFiltroDTO);

        Dictionary<string, object> GetDatosActualizarEmpleado(int claveEmpleado, bool esReingresoEmpleado);

        bool EliminarContratoFirmado(int idArchivo, int claveEmpleado);

        List<ArchivosDTO> GetContratosFirmados(int claveEmpleado);

        Dictionary<string, object> GuardarExcelActoCondicionCargaMasiva(HttpPostedFileBase _archivoExcel);

        #region METODOS PARA DESCARGAR EXCEL, SOLAMENTE EMPLEADOS CON ESTATUS PENDIENTE
        List<LayoutAltasRHDTO> GetEmpleadosLayoutAlta(List<string> _lstClaveEmpleados);
        #endregion

        Dictionary<string, object> GetDatosCandidatoAprobado(int idCandidatoAprobado);

        Dictionary<string, object> ReingresarEmpleado(int clave_empleado, int requisicion_id);

        Dictionary<string, object> GetInformacionRequisicion(int requisicion_id);

        Dictionary<string, object> ChecarPermisoTabuladorLibre();

        Dictionary<string, object> GetIDUsuarioEK();

        Dictionary<string, object> CambiarEstatusEmpleado(int claveEmpleado, string status);
        Dictionary<string, object> AddContratos(int idEmpleado, int tipoDuracionContrato);
        Dictionary<string, object> GetDocs(int? clave_empleado, int? id_candidato);

        #region FOTO DEL EMPLEADO
        Dictionary<string, object> GuardarFotoEmpleado(List<HttpPostedFileBase> objFotoEmpleado, ArchivosDTO objDTO);

        Dictionary<string, object> GetFotoEmpleado(ArchivosDTO objDTO);
        #endregion

        Dictionary<string, object> GetHistorialCC(int clave_empleado);

        Dictionary<string, object> AutorizacionMultiple(List<int> claveEmpleados);

        Dictionary<string, object> GetHeaderEmpleados();

        Dictionary<string, object> CheckEmpleado(string curp, string rfc, string nss);
        Dictionary<string, object> EnviarCorreos(List<int> lstClaveEmpleado);

        Dictionary<string, object> FillCboTipoEmpleados();

        Dictionary<string, object> GuardarSustentoHijo(List<HttpPostedFileBase> lstSustentos, int claveEmpleado, int FK_EmplFamilia);

        Dictionary<string, object> GetSustentos(int claveEmpleado, int FK_EmplFamilia);

        Tuple<Stream, string> DescagarSustento(int id);
        #endregion

        #region GESTION DE ARCHIVOS DEL CANDIDATO/EMPLEADO
        List<ArchivosDTO> GetArchivosCandidato(int idArchivo);

        bool EliminarArchivoCandidato(int idArchivo);
        #endregion

        #region PLATAFORMAS
        List<PlataformasDTO> GetPlataformas();

        bool CrearEditarPlataforma(PlataformasDTO objCEDTO);

        bool EliminarPlataforma(int idPlataforma);
        #endregion

        #region CATALOGO CORREOS
        List<tblRH_REC_CatCorreos> GetCorreos(CatCorreosDTO objFiltroDTO);

        bool CrearEditarCorreo(CatCorreosDTO objCEDTO);

        bool EliminarCorreo(int idCorreo);
        #endregion

        #region FILL COMBOS
        List<ComboDTO> FillCboCC();

        Dictionary<string, object> FillComboCCUnique();

        List<ComboDTO> FillCboPuestos();

        List<ComboDTO> FillCboPaises();

        List<ComboDTO> FillCboEstados(int _clavePais);

        List<ComboDTO> FillCboMunicipios(int _clavePais, int _claveEstado);

        List<ComboDTO> FillCboMotivos();

        List<ComboDTO> FillCboEscolaridades();

        List<ComboDTO> FillFiltroCboCC();

        List<ComboDTO> FillFiltroCboPuestos();

        List<ComboDTO> FillFiltroCboPuestosGestion();

        List<ComboDTO> FillCboTipoFormulaIMSS();

        List<ComboDTO> FillCboDepartamentos(string cc);

        List<ComboDTO> FillCboUsuarios();

        List<ComboDTO> FillCboPlataformas();

        List<ComboDTO> FillComboRegistroPatronal(string cc);

        List<ComboDTO> FillComboDuracionContrato();

        Dictionary<string, object> FillEstatusFiltro();

        Dictionary<string, object> FillDepartamentos(string cc);

        Dictionary<string, object> CargarTiposNomina();

        Dictionary<string, object> CargarBancos();
        Dictionary<string, object> FillComboEDArchivos();
        Dictionary<string, object> FillCboCCRegistrosPatronales(int? clave_reg_pat);
        Dictionary<string, object> FillComboRelFases();

        Dictionary<string, object> FillMotivoSueldo();
        Dictionary<string, object> FillComboGeoDepartamentos();
        Dictionary<string, object> FillCboEstadosPERU(int claveDepartamento);

        #endregion

        #region EXPEDIENTE DIGITAL
        Dictionary<string, object> CargarExpedientesDigitales(string estatus_emp, string cc, List<int> estado);
        Dictionary<string, object> GetArchivosCombo();
        Dictionary<string, object> CargarInformacionEmpleado(int claveEmpleado);
        Dictionary<string, object> GuardarNuevoExpediente(int claveEmpleado, List<int> listaArchivosAplicables);
        Dictionary<string, object> EditarExpediente(int claveEmpleado, List<int> listaArchivosAplicables);
        Dictionary<string, object> GuardarArchivoExpediente(int expediente_id, int archivo_id, HttpPostedFileBase archivo);
        tblRH_REC_ED_RelacionExpedienteArchivo GetArchivoExpediente(int archivoCargado_id, int tipo_archivo);
        Dictionary<string, object> EliminarArchivoExpediente(int expediente_id, int archivo_id);

     
        Dictionary<string, object> GetArchivos();
        Dictionary<string, object> CrearEditarArchivos(ExpedienteArchivosDTO objArchivo);
        Dictionary<string, object> EliminarArchivo(int idArchivo);
        Dictionary<string, object> VerHistorialExpediente(int expediente_id, int archivo_id);

        Dictionary<string, object> DescargarAvanceExcel(string estatus_emp, string cc);
        #endregion

        #region ALTA EMPLEADO (REGION TEMPORAL)
        Dictionary<string, object> CargarRequisiciones();
        Dictionary<string, object> CargarAutoriza(int autoriza);
        Dictionary<string, object> CargarUsuarioResg(int usuarioResg);
        Dictionary<string, object> CargarDepto(int depto);
        Dictionary<string, object> CargarTabulador(string cc, int puesto, int? idTabuladorDet);
        Tuple<Stream, string> DescargarArchivoEmpleado(int id);
        string GenerarCURP(string nombres, string paterno, string materno, SexoEnum sexo, DateTime fechaNacimiento, EstadoEnum estado);
        string GetRFC(GestionCandidatosDTO objCandidato);
        #endregion

        #region REQUISICION
        Dictionary<string, object> GetCCs();
        Dictionary<string, object> GetRequisiciones(List<string> ccs, string estatus);
        Dictionary<string, object> GetIdRequisicionDisponible();
        Dictionary<string, object> GetPlantilla(string cc, int? puesto);
        Dictionary<string, object> GetTipoContrato();
        Dictionary<string, object> GetRazonSolicitud();
        Dictionary<string, object> GetJefeInmediato(string cc);
        Dictionary<string, object> GetAutoriza(string cc);
        List<AutocompleteDTO> GetAutocompleteJefe(string term);
        Dictionary<string, object> GetSolicita();
        List<AutocompleteDTO> GetAutocompleteSolicita(string term);
        List<AutocompleteDTO> GetAutocompleteAutoriza(string term, string cc);
        Dictionary<string, object> GuardarRequisicion(sn_requisicion_personal requisicion);
        Dictionary<string, object> GetInformacionRequisicionConsulta(int requisicion_id, string cc);
        Dictionary<string, object> AutorizarRechazarRequisicion(RequisicionRHDTO objDTO);
        Dictionary<string, object> GetFechaVigencia7DiasNaturales();
        Dictionary<string, object> EliminarRequisicion(int requisicion_id);
        Dictionary<string, object> GetCategoriasPuesto(int idPuesto, string cc);
        Dictionary<string, object> GetTabuladorCategoria(int categoriaId);
        Dictionary<string, object> GetCategoriaPuesto(int tabuladorDetID);
        #endregion

        #region COMENTARIOS
        Dictionary<string, object> GetComentario(int claveEmpleado);
        Dictionary<string, object> CrearComentario(int claveEmpleado, string comentario);
        #endregion

        #region FNC GRALES
        Dictionary<string, object> GetContratoReporte(int clave_empleado);
        Dictionary<string, object> GetCategoriasByLineaNegocio(int idLineaNegocio, int idPuesto);
        Dictionary<string, object> GetLineaNegocioByCC(string cc);

        #endregion

        #region REL REGISTRO PATRONALES
        Dictionary<string, object> GetRelRegPatronales(tblRH_EK_Registros_Patronales objFiltro);
        Dictionary<string, object> GetRelRegPatCC(int clave_reg_pat);
        Dictionary<string, object> AddCCRegPatronal(int clave_reg_pat, string cc);
        Dictionary<string, object> DeleteCCRegPatronal(int clave_reg_pat, string cc);
        Dictionary<string, object> CrearEditarRegistroPatronal(tblRH_EK_Registros_Patronales objCERegPat, HttpPostedFileBase archivoAdjunto);
        Dictionary<string, object> DeleteRegistroPatronal(int idRegPat);
        int GetUltimoIdRegPat();
        Dictionary<string, object> GetRelRegPatronalesReporte();
        Tuple<Stream, string> DescargarArchivoRegPat(int id);

        #endregion

        #region RELACION FASES
        Dictionary<string, object> GetFasesUsuarios(int idFase);
        Dictionary<string, object> AddFasesUsuarios(int idUsuario, int idFase);
        Dictionary<string, object> DeleteFasesUsuarios(int idUsuario, int idFase);

        #endregion

        #region CAT CONTRATOS
        Dictionary<string, object> GetDuracionContratos();
        Dictionary<string, object> AddDuracionContrato(string duracion_desc, int? dias, int? meses, int? años, bool indef);
        Dictionary<string, object> DeleteDuracionContrato(int id_duracion);
        
        #endregion

        #region CAT PUESTOS
        Dictionary<string, object> GetPuestos();
        Dictionary<string, object> GuardarNuevoPuesto(tblRH_EK_Puestos puesto);
        Dictionary<string, object> EditarPuesto(tblRH_EK_Puestos puesto);
        Dictionary<string, object> EliminarPuesto(tblRH_EK_Puestos puesto);
        Dictionary<string, object> CargarArchivoDescriptor(HttpPostedFileBase file, int puesto);
        tblRH_EK_Puesto_ArchivoDescriptor GetFileDownloadDescriptor(int id);
        #endregion

        #region PERMISOS
        bool UsuarioDeConsulta();
        bool UsuarioDeConsultaTeorico(); //MARISELA EXAMEN TEORICO
        bool UsuarioPermisoAccionEditarFechaCambio();
        bool UsuarioPermisoAccionOcultarSalarioByCC();
        bool UsuarioPermisoMostrarBotones();
        bool UsuarioVerSalarios();
        bool UsuarioOcultarSalariosGernecia(int clv_emp);
        #endregion

        #region CAT CATEGORIAS
        Dictionary<string, object> FillComboPuestos(string cC);
        Dictionary<string, object> GetPuestosCategoriasRelPuesto(string _cc, string _strPuesto, int idPuesto);
        Dictionary<string, object> EditarPlantilla(List<InputCategoriasDTO> lstCambios, string cc, int puesto, string descPuesto);
        Dictionary<string, object> GetAllPuestos();
        Dictionary<string, object> AddPlantillaPuesto(string _cc, string _strPuesto, int idPuesto, string _strNuevoPuesto, int nuevoPuesto);

        Dictionary<string, object> GetPuestoDetalle(PuestoDetalleDTO objDTO);
        #endregion

        #region CAT TABULADORES
        Dictionary<string, object> GetPuestosTabuladores(string cc);

        Dictionary<string, object> CrearEditarTabuladorPuesto(string cc, CatTabuladoresDTO objTabulador);

        Dictionary<string, object> GetReporteTabuladores(string cc);


        #endregion

        #region DIAS PARA INGRESOS
        bool GetPuedeEditarDiasDisponibles();
        Dictionary<string, object> GetDiasDisponibles();
        Dictionary<string, object> GetDiasDisponiblesParaLimitarFecha();
        Dictionary<string, object> SetDiasDisponibles(int anteriores, int posteriores);
        #endregion

        #region EXAMEN MÉDICO
        void GuardarExamenMedico(int idCandidato, tblRH_REC_ExamenMedico examenMedico, List<tblRH_REC_ExamenMedico_Antecedentes> listaAntecedentes, Byte[] archivoReporteExamenMedico);
        #endregion

        bool PermisoTabuladorAltaEmpleado();
        void factorizar();

        #region Datos Perú
        List<ComboDTO> FillComboAFPPeru();
        #endregion

        #region DATOS EMPLEADO PERU
        Dictionary<string, object> GetSituacion(bool afiliado);
        Dictionary<string, object> GetRucEps();
        Dictionary<string, object> GetAfps();
        Dictionary<string, object> FillComboTiposTrabajadores();
        Dictionary<string,object> FillComboBancosPeru();
        Dictionary<string, object> FillComboRegimenLaboralPeru();

        #endregion

        tblRH_EK_Estados GetEstadoByID(int id);
    }
}