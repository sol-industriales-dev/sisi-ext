using Core.DAO.RecursosHumanos.Reclutamientos;
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

namespace Core.Service.RecursosHumanos.Reclutamientos
{
    public class ReclutamientosService : IReclutamientosDAO
    {
        #region INIT
        public IReclutamientosDAO r_reclutamientos { get; set; }
        public IReclutamientosDAO Reclutamientos
        {
            get { return r_reclutamientos; }
            set { r_reclutamientos = value; }
        }
        public ReclutamientosService(IReclutamientosDAO Reclutamientos)
        {
            this.Reclutamientos = Reclutamientos;
        }
        #endregion

        #region SOLICITUDES
        public List<SolicitudesDTO> GetSolicitudes(SolicitudesDTO objFiltro)
        {
            return Reclutamientos.GetSolicitudes(objFiltro);
        }

        public bool CrearSolicitud(tblRH_REC_Solicitudes objSolicitud)
        {
            return Reclutamientos.CrearSolicitud(objSolicitud);
        }

        public bool ActualizarSolicitud(tblRH_REC_Solicitudes objSolicitud)
        {
            return Reclutamientos.ActualizarSolicitud(objSolicitud);
        }

        public bool EliminarSolicitud(int idSolicitud)
        {
            return Reclutamientos.EliminarSolicitud(idSolicitud);
        }

        public bool ExisteSolicitudRelCandidato(int idSolicitud)
        {
            return Reclutamientos.ExisteSolicitudRelCandidato(idSolicitud);
        }

        public List<Core.Entity.RecursosHumanos.Catalogo.tblRH_CatPuestos> FillCboPuestosSolicitudes(string cc)
        {
            return Reclutamientos.FillCboPuestosSolicitudes(cc);
        }

        public List<string> GetCategoriasRelPuesto(string _cc, string _puesto)
        {
            return Reclutamientos.GetCategoriasRelPuesto(_cc, _puesto);
        }
        #endregion

        #region GESTIÓN DE SOLICITUDES
        public List<GestionSolicitudesDTO> GetGestionSolicitudes(GestionSolicitudesDTO objFiltro)
        {
            return Reclutamientos.GetGestionSolicitudes(objFiltro);
        }

        public bool RechazarSolicitud(GestionSolicitudesDTO objGestionSolicitud)
        {
            return Reclutamientos.RechazarSolicitud(objGestionSolicitud);
        }
        #endregion

        #region GESTIÓN DE CANDIDATOS
        public List<CandidatosDTO> GetCandidatos(CandidatosDTO objCandidatos)
        {
            return Reclutamientos.GetCandidatos(objCandidatos);
        }

        public Dictionary<string, object> CrearCandidato(GestionCandidatosDTO objCandidato, HttpPostedFileBase objFile)
        {
            return Reclutamientos.CrearCandidato(objCandidato, objFile);
        }

        public Tuple<bool, string> ActualizarCandidato(GestionCandidatosDTO objCandidato, HttpPostedFileBase objFile)
        {
            return Reclutamientos.ActualizarCandidato(objCandidato, objFile);
        }

        public bool EliminarCandidato(int idCandidato)
        {
            return Reclutamientos.EliminarCandidato(idCandidato);
        }

        public List<ComboDTO> FillFiltroCboPuestosDisponibles()
        {
            return Reclutamientos.FillFiltroCboPuestosDisponibles();
        }

        public EntrevistaInicialDTO GetEntrevistaInicial(EntrevistaInicialDTO objEntrevistaInicialDTO)
        {
            return Reclutamientos.GetEntrevistaInicial(objEntrevistaInicialDTO);
        }

        public Dictionary<string, object> CrearEditarEntrevistaInicial(EntrevistaInicialDTO objEntrevistaInicialDTO)
        {
            return Reclutamientos.CrearEditarEntrevistaInicial(objEntrevistaInicialDTO);
        }

        public Tuple<Stream, string> DescargarArchivoCV(int candidatoID)
        {
            return Reclutamientos.DescargarArchivoCV(candidatoID);
        }

        public Dictionary<string, object> GetUsuarioEntrevistaActual()
        {
            return Reclutamientos.GetUsuarioEntrevistaActual();
        }

        public List<tblRH_CatEmpleados> getAutoCompleteEmpleadosBaja(string term)
        {
            return Reclutamientos.getAutoCompleteEmpleadosBaja(term);
        }

        public List<tblRH_CatEmpleados> getAutoCompleteCandidatos(string term)
        {
            return Reclutamientos.getAutoCompleteCandidatos(term);
        }
        #endregion

        #region FASES
        public List<FasesDTO> GetFases()
        {
            return Reclutamientos.GetFases();
        }

        public bool CrearFase(tblRH_REC_Fases objFase)
        {
            return Reclutamientos.CrearFase(objFase);
        }

        public bool ActualizarFase(tblRH_REC_Fases objFase)
        {
            return Reclutamientos.ActualizarFase(objFase);
        }

        public bool EliminarFase(int idFase)
        {
            return Reclutamientos.EliminarFase(idFase);
        }
        #endregion

        #region FASES RELACIÓN ACTIVIDADES
        public Dictionary<string, object> GetActividades(SegCandidatosDTO objSegCandidatosDTO)
        {
            return Reclutamientos.GetActividades(objSegCandidatosDTO);
        }

        public bool CrearActividad(tblRH_REC_Actividades objActividad)
        {
            return Reclutamientos.CrearActividad(objActividad);
        }

        public bool ActualizarActividad(tblRH_REC_Actividades objActividad)
        {
            return Reclutamientos.ActualizarActividad(objActividad);
        }

        public bool EliminarActividad(int idActividad)
        {
            return Reclutamientos.EliminarActividad(idActividad);
        }

        public bool AsignarEncargadoFase(ActividadesDTO objActividadDTO)
        {
            return Reclutamientos.AsignarEncargadoFase(objActividadDTO);
        }
        #endregion

        #region PUESTOS RELACIONADOS A FASES
        public List<PuestosRelFasesDTO> GetPuestosRelFase(int idFase)
        {
            return Reclutamientos.GetPuestosRelFase(idFase);
        }

        public bool CrearPuestoRelFase(tblRH_REC_PuestosRelFases objPuestoRelFase)
        {
            return Reclutamientos.CrearPuestoRelFase(objPuestoRelFase);
        }

        public bool EliminarPuestoRelFase(int idPuestoRelFase)
        {
            return Reclutamientos.EliminarPuestoRelFase(idPuestoRelFase);
        }

        public bool ExistePuestoEnFase(int idFase, int idPuesto)
        {
            return Reclutamientos.ExistePuestoEnFase(idFase, idPuesto);
        }
        #endregion

        #region SEGUIMIENTO DE CANDIDATOS
        public List<SegCandidatosDTO> GetSegCandidatos(SegCandidatosDTO objFiltro)
        {
            return Reclutamientos.GetSegCandidatos(objFiltro);
        }

        public List<SegCandidatosDTO> GetSegDetCandidatos(SegCandidatosDTO objFiltro)
        {
            return Reclutamientos.GetSegDetCandidatos(objFiltro);
        }

        public bool CrearEditarSegCandidatos(SegCandidatosDTO objSegCandidatoDTO)
        {
            return Reclutamientos.CrearEditarSegCandidatos(objSegCandidatoDTO);
        }

        public bool CrearEditarComentarioActividad(SegCandidatosDTO objSegCandidatoDTO)
        {
            return Reclutamientos.CrearEditarComentarioActividad(objSegCandidatoDTO);
        }

        public string GetObservacionActividad(SegCandidatosDTO objSegCandidatoDTO)
        {
            return Reclutamientos.GetObservacionActividad(objSegCandidatoDTO);
        }

        public bool CrearArchivoActividad(SegCandidatosDTO objSegCandidatoDTO, List<HttpPostedFileBase> lstFiles)
        {
            return Reclutamientos.CrearArchivoActividad(objSegCandidatoDTO, lstFiles);
        }

        public List<ArchivosDTO> GetArchivosActividadesRelFase(SegCandidatosDTO objSegCandidatoDTO)
        {
            return Reclutamientos.GetArchivosActividadesRelFase(objSegCandidatoDTO);
        }
        public string GetArchivoEvidencia(int id_file)
        {
            return Reclutamientos.GetArchivoEvidencia(id_file);
        }

        public bool EliminarArchivoActividad(int idArchivo)
        {
            return Reclutamientos.EliminarArchivoActividad(idArchivo);
        }

        public bool CrearEditarCalificacionActividad(SegCandidatosDTO objSegCandidatoDTO)
        {
            return Reclutamientos.CrearEditarCalificacionActividad(objSegCandidatoDTO);
        }

        public Dictionary<string, object> GetFasesAutorizadas(int idPuesto)
        {
            return Reclutamientos.GetFasesAutorizadas(idPuesto);
        }

        public Dictionary<string, object> NotificarActividad(int idCandidato, int idActividad, int estatus, int? idNotificante)
        {
            return Reclutamientos.NotificarActividad(idCandidato, idActividad, estatus, idNotificante);
        }

        public List<tblRH_CatEmpleados> getCatUsuariosGeneral(string term)
        {
            return Reclutamientos.getCatUsuariosGeneral(term);
        }

        public bool SetNotiEstatusActividad(int idCandidato, int idFase, int idActividad)
        {
            return Reclutamientos.SetNotiEstatusActividad(idCandidato, idFase, idActividad);
        }
        #endregion

        #region EMPLEADOS EK
        public List<EmpleadosDTO> GetEmpleadosEK(List<string> cc, List<string> lstEstatusEmpleado)
        {
            return Reclutamientos.GetEmpleadosEK(cc, lstEstatusEmpleado);
        }

        public Dictionary<string, object> CambiarContratable(string claveEmpleado, string esContratable)
        {
            return Reclutamientos.CambiarContratable(claveEmpleado, esContratable);
        }

        public Dictionary<string, object> CrearEditarInformacionEmpleado(EmpleadosDTO objEmpleadoDTO, GeneralesContactoDTO objGenContactoDTO, BeneficiariosDTO objBeneficiariosDTO, ContEmergenciasDTO objContEmergenciasDTO, CompaniaDTO objCompaniaDTO, List<FamiliaresDTO> lstFamiliares, UniformesDTO objUniforme, NuevoTabuladorDTO objTabulador, ContratoDTO objContrato, InfoEmpleadoPeruDTO objDatosPeru)
        {
            return Reclutamientos.CrearEditarInformacionEmpleado(objEmpleadoDTO, objGenContactoDTO, objBeneficiariosDTO, objContEmergenciasDTO, objCompaniaDTO, lstFamiliares, objUniforme, objTabulador, objContrato, objDatosPeru);
        }

        public bool EliminarEmpleado(int claveEmpleado)
        {
            return Reclutamientos.EliminarEmpleado(claveEmpleado);
        }

        public List<FamiliaresDTO> GetFamiliares(int clave_empleado)
        {
            return Reclutamientos.GetFamiliares(clave_empleado);
        }

        public List<ContratoDTO> GetContratos(int clave_empleado)
        {
            return Reclutamientos.GetContratos(clave_empleado);
        }

        public Dictionary<string, object> CrearEditarInformacionFamiliar(FamiliaresDTO objFamiliarDTO)
        {
            return Reclutamientos.CrearEditarInformacionFamiliar(objFamiliarDTO);
        }

        public bool EliminarFamiliar(int idFamiliar, int clave_empleado)
        {
            return Reclutamientos.EliminarFamiliar(idFamiliar, clave_empleado);
        }

        public Dictionary<string, object> EliminarContrato(int id_contrato_empleado)
        {
            return Reclutamientos.EliminarContrato(id_contrato_empleado);
        }

        public List<ComboDTO> FillCboCandidatosAprobados()
        {
            return Reclutamientos.FillCboCandidatosAprobados();
        }

        public List<ComboDTO> FillCboParentesco()
        {
            return Reclutamientos.FillCboParentesco();
        }

        public List<ComboDTO> FillCboTipoSangre()
        {
            return Reclutamientos.FillCboTipoSangre();
        }

        public List<ComboDTO> FillCboTipoCasa()
        {
            return Reclutamientos.FillCboTipoCasa();
        }

        public List<dynamic> GetDatosEmpleadoDocumentos(int claveEmpleado)
        {
            return Reclutamientos.GetDatosEmpleadoDocumentos(claveEmpleado);
        }

        public UniformesDTO GetUniformes(int claveEmpleado)
        {
            return Reclutamientos.GetUniformes(claveEmpleado);
        }

        public Dictionary<string, object> CrearEditarUniforme(UniformesDTO objUniforme)
        {
            return Reclutamientos.CrearEditarUniforme(objUniforme);
        }

        public bool EliminarUniforme(int idUniforme)
        {
            return Reclutamientos.EliminarUniforme(idUniforme);
        }

        public List<ArchivosDTO> GetArchivoExamenMedico(int claveEmpleado)
        {
            return Reclutamientos.GetArchivoExamenMedico(claveEmpleado);
        }

        public bool CrearArchivoExamenMedico(ArchivosDTO objArchivoDTO, HttpPostedFileBase objFile)
        {
            return Reclutamientos.CrearArchivoExamenMedico(objArchivoDTO, objFile);
        }

        public bool EliminarExamenMedico(int idExamenMedico)
        {
            return Reclutamientos.EliminarExamenMedico(idExamenMedico);
        }

        public List<ArchivosDTO> GetArchivoMaquinaria(int claveEmpleado)
        {
            return Reclutamientos.GetArchivoMaquinaria(claveEmpleado);
        }

        public bool CrearArchivoMaquinaria(ArchivosDTO objArchivoDTO, HttpPostedFileBase objFile)
        {
            return Reclutamientos.CrearArchivoMaquinaria(objArchivoDTO, objFile);
        }

        public bool EliminarMaquinaria(int idExamenMedico)
        {
            return Reclutamientos.EliminarMaquinaria(idExamenMedico);
        }

        public List<TabuladoresDTO> GetTabuladores(TabuladoresDTO objTabDTO)
        {
            return Reclutamientos.GetTabuladores(objTabDTO);
        }

        public Dictionary<string, object> CrearTabuladorPuesto(TabuladoresPuestoDTO objTabuladorPuestoDTO)
        {
            return Reclutamientos.CrearTabuladorPuesto(objTabuladorPuestoDTO);
        }

        public Dictionary<string, object> CrearTabulador(TabuladoresDTO objTabuladorDTO)
        {
            return Reclutamientos.CrearTabulador(objTabuladorDTO);
        }

        public Dictionary<string, object> CambiarFechaCambioTabulador(int id, DateTime fecha_cambio, int claveEmpleado)
        {
            return Reclutamientos.CambiarFechaCambioTabulador(id, fecha_cambio, claveEmpleado);
        }

        public List<ComboDTO> FillCboBancos()
        {
            return Reclutamientos.FillCboBancos();
        }

        public Dictionary<string, object> GetReporteSegCandidatos(ReporteSegCandidatosDTO objFiltroDTO)
        {
            return Reclutamientos.GetReporteSegCandidatos(objFiltroDTO);
        }

        public Dictionary<string, object> GetDatosActualizarEmpleado(int claveEmpleado, bool esReingresoEmpleado)
        {
            return Reclutamientos.GetDatosActualizarEmpleado(claveEmpleado, esReingresoEmpleado);
        }

        public bool EliminarContratoFirmado(int idArchivo, int claveEmpleado)
        {
            return Reclutamientos.EliminarContratoFirmado(idArchivo, claveEmpleado);
        }

        public List<ArchivosDTO> GetContratosFirmados(int claveEmpleado)
        {
            return Reclutamientos.GetContratosFirmados(claveEmpleado);
        }

        public Dictionary<string, object> GuardarExcelActoCondicionCargaMasiva(HttpPostedFileBase _archivoExcel)
        {
            return Reclutamientos.GuardarExcelActoCondicionCargaMasiva(_archivoExcel);
        }

        #region METODOS PARA DESCARGAR EXCEL, SOLAMENTE EMPLEADOS CON ESTATUS PENDIENTE
        public List<LayoutAltasRHDTO> GetEmpleadosLayoutAlta(List<string> _lstClaveEmpleados)
        {
            return Reclutamientos.GetEmpleadosLayoutAlta(_lstClaveEmpleados);
        }
        #endregion

        public Dictionary<string, object> GetDatosCandidatoAprobado(int idCandidatoAprobado)
        {
            return r_reclutamientos.GetDatosCandidatoAprobado(idCandidatoAprobado);
        }

        public Dictionary<string, object> ReingresarEmpleado(int clave_empleado, int requisicion_id)
        {
            return Reclutamientos.ReingresarEmpleado(clave_empleado, requisicion_id);
        }

        public Dictionary<string, object> GetInformacionRequisicion(int requisicion_id)
        {
            return Reclutamientos.GetInformacionRequisicion(requisicion_id);
        }

        public Dictionary<string, object> ChecarPermisoTabuladorLibre()
        {
            return Reclutamientos.ChecarPermisoTabuladorLibre();
        }

        public Dictionary<string, object> GetIDUsuarioEK()
        {
            return Reclutamientos.GetIDUsuarioEK();
        }

        public Dictionary<string, object> CambiarEstatusEmpleado(int claveEmpleado, string status)
        {
            return Reclutamientos.CambiarEstatusEmpleado(claveEmpleado, status);
        }

        public Dictionary<string, object> AddContratos(int idEmpleado, int tipoDuracionContrato)
        {
            return Reclutamientos.AddContratos(idEmpleado, tipoDuracionContrato);
        }

        public Dictionary<string, object> GetDocs(int? clave_empleado, int? id_candidato)
        {
            return Reclutamientos.GetDocs(clave_empleado, id_candidato);
        }

        #region FOTO DEL EMPLEADO
        public Dictionary<string, object> GuardarFotoEmpleado(List<HttpPostedFileBase> objFotoEmpleado, ArchivosDTO objDTO)
        {
            return Reclutamientos.GuardarFotoEmpleado(objFotoEmpleado, objDTO);
        }

        public Dictionary<string, object> GetFotoEmpleado(ArchivosDTO objDTO)
        {
            return Reclutamientos.GetFotoEmpleado(objDTO);
        }
        #endregion

        public Dictionary<string, object> GetHistorialCC(int clave_empleado)
        {
            return r_reclutamientos.GetHistorialCC(clave_empleado);
        }

        public Dictionary<string, object> AutorizacionMultiple(List<int> claveEmpleados)
        {
            return r_reclutamientos.AutorizacionMultiple(claveEmpleados);
        }

        public Dictionary<string, object> GetHeaderEmpleados()
        {
            return Reclutamientos.GetHeaderEmpleados();
        }

        public Dictionary<string, object> CheckEmpleado(string curp, string rfc, string nss)
        {
            return Reclutamientos.CheckEmpleado(curp, rfc, nss);
        }

        public Dictionary<string, object> EnviarCorreos(List<int> lstClaveEmpleado)
        {
            return Reclutamientos.EnviarCorreos(lstClaveEmpleado);
        }

        public Dictionary<string, object> FillCboTipoEmpleados()
        {
            return Reclutamientos.FillCboTipoEmpleados();
        }

        public Dictionary<string, object> GuardarSustentoHijo(List<HttpPostedFileBase> lstSustentos, int claveEmpleado, int FK_EmplFamilia)
        {
            return Reclutamientos.GuardarSustentoHijo(lstSustentos, claveEmpleado, FK_EmplFamilia);
        }

        public Dictionary<string, object> GetSustentos(int claveEmpleado, int FK_EmplFamilia)
        {
            return Reclutamientos.GetSustentos(claveEmpleado, FK_EmplFamilia);
        }

        public Tuple<Stream, string> DescagarSustento(int id)
        {
            return Reclutamientos.DescagarSustento(id);
        }
        #endregion

        #region GESTION DE ARCHIVOS DEL CANDIDATO/EMPLEADO
        public List<ArchivosDTO> GetArchivosCandidato(int idCandidato)
        {
            return Reclutamientos.GetArchivosCandidato(idCandidato);
        }

        public bool EliminarArchivoCandidato(int idArchivo)
        {
            return Reclutamientos.EliminarArchivoCandidato(idArchivo);
        }
        #endregion

        #region PLATAFORMAS
        public List<PlataformasDTO> GetPlataformas()
        {
            return Reclutamientos.GetPlataformas();
        }

        public bool CrearEditarPlataforma(PlataformasDTO objCEDTO)
        {
            return Reclutamientos.CrearEditarPlataforma(objCEDTO);
        }

        public bool EliminarPlataforma(int idPlataforma)
        {
            return Reclutamientos.EliminarPlataforma(idPlataforma);
        }
        #endregion

        #region CATALOGO CORREOS
        public List<tblRH_REC_CatCorreos> GetCorreos(CatCorreosDTO objFiltroDTO)
        {
            return Reclutamientos.GetCorreos(objFiltroDTO);
        }

        public bool CrearEditarCorreo(CatCorreosDTO objCEDTO)
        {
            return Reclutamientos.CrearEditarCorreo(objCEDTO);
        }

        public bool EliminarCorreo(int idCorreo)
        {
            return Reclutamientos.EliminarCorreo(idCorreo);
        }
        #endregion

        #region FILL COMBOS
        public List<ComboDTO> FillCboCC()
        {
            return Reclutamientos.FillCboCC();
        }

        public Dictionary<string, object> FillComboCCUnique()
        {
            return Reclutamientos.FillComboCCUnique();
        }

        public List<ComboDTO> FillCboPuestos()
        {
            return Reclutamientos.FillCboPuestos();
        }

        public List<ComboDTO> FillCboPaises()
        {
            return Reclutamientos.FillCboPaises();
        }

        public List<ComboDTO> FillCboEstados(int _clavePais)
        {
            return Reclutamientos.FillCboEstados(_clavePais);
        }

        public List<ComboDTO> FillCboMunicipios(int _clavePais, int _claveEstado)
        {
            return Reclutamientos.FillCboMunicipios(_clavePais, _claveEstado);
        }

        public List<ComboDTO> FillCboMotivos()
        {
            return Reclutamientos.FillCboMotivos();
        }

        public List<ComboDTO> FillCboEscolaridades()
        {
            return Reclutamientos.FillCboEscolaridades();
        }

        public List<ComboDTO> FillFiltroCboCC()
        {
            return Reclutamientos.FillFiltroCboCC();
        }

        public List<ComboDTO> FillFiltroCboPuestos()
        {
            return Reclutamientos.FillFiltroCboPuestos();
        }

        public List<ComboDTO> FillFiltroCboPuestosGestion()
        {
            return Reclutamientos.FillFiltroCboPuestosGestion();
        }

        public List<ComboDTO> FillCboTipoFormulaIMSS()
        {
            return Reclutamientos.FillCboTipoFormulaIMSS();
        }

        public List<ComboDTO> FillCboDepartamentos(string cc)
        {
            return Reclutamientos.FillCboDepartamentos(cc);
        }

        public List<ComboDTO> FillCboUsuarios()
        {
            return Reclutamientos.FillCboUsuarios();
        }

        public List<ComboDTO> FillCboPlataformas()
        {
            return Reclutamientos.FillCboPlataformas();
        }

        public List<ComboDTO> FillComboRegistroPatronal(string cc)
        {
            return Reclutamientos.FillComboRegistroPatronal(cc);
        }

        public List<ComboDTO> FillComboDuracionContrato()
        {
            return Reclutamientos.FillComboDuracionContrato();
        }

        public Dictionary<string, object> FillEstatusFiltro()
        {
            return Reclutamientos.FillEstatusFiltro();
        }

        public Dictionary<string, object> FillDepartamentos(string cc)
        {
            return Reclutamientos.FillDepartamentos(cc);
        }

        public Dictionary<string, object> CargarTiposNomina()
        {
            return Reclutamientos.CargarTiposNomina();
        }

        public Dictionary<string, object> CargarBancos()
        {
            return Reclutamientos.CargarBancos();
        }

        public Dictionary<string, object> FillComboEDArchivos()
        {
            return Reclutamientos.FillComboEDArchivos();
        }

        public Dictionary<string, object> FillCboCCRegistrosPatronales(int? clave_reg_pat)
        {
            return Reclutamientos.FillCboCCRegistrosPatronales(clave_reg_pat);
        }

        public Dictionary<string, object> FillComboRelFases()
        {
            return Reclutamientos.FillComboRelFases();
        }

        public Dictionary<string, object> FillMotivoSueldo()
        {
            return Reclutamientos.FillMotivoSueldo();
        }

        public Dictionary<string, object> FillComboGeoDepartamentos()
        {
            return Reclutamientos.FillComboGeoDepartamentos();
        }

        public Dictionary<string, object> FillCboEstadosPERU(int claveDepartamento)
        {
            return Reclutamientos.FillCboEstadosPERU(claveDepartamento);
        }
        #endregion

        #region EXPEDIENTE DIGITAL
        public Dictionary<string, object> CargarExpedientesDigitales(string estatus_emp, string cc, List<int> estado)
        {
            return Reclutamientos.CargarExpedientesDigitales(estatus_emp, cc, estado);
        }

        public Dictionary<string, object> GetArchivosCombo()
        {
            return Reclutamientos.GetArchivosCombo();
        }

        public Dictionary<string, object> CargarInformacionEmpleado(int claveEmpleado)
        {
            return Reclutamientos.CargarInformacionEmpleado(claveEmpleado);
        }

        public Dictionary<string, object> GuardarNuevoExpediente(int claveEmpleado, List<int> listaArchivosAplicables)
        {
            return Reclutamientos.GuardarNuevoExpediente(claveEmpleado, listaArchivosAplicables);
        }

        public Dictionary<string, object> EditarExpediente(int claveEmpleado, List<int> listaArchivosAplicables)
        {
            return Reclutamientos.EditarExpediente(claveEmpleado, listaArchivosAplicables);
        }

        public Dictionary<string, object> GuardarArchivoExpediente(int expediente_id, int archivo_id, HttpPostedFileBase archivo)
        {
            return Reclutamientos.GuardarArchivoExpediente(expediente_id, archivo_id, archivo);
        }

        public tblRH_REC_ED_RelacionExpedienteArchivo GetArchivoExpediente(int archivoCargado_id, int tipo_archivo)
        {
            return Reclutamientos.GetArchivoExpediente(archivoCargado_id, tipo_archivo);
        }

        public Dictionary<string, object> GetArchivos()
        {
            return Reclutamientos.GetArchivos();
        }

        public Dictionary<string, object> CrearEditarArchivos(ExpedienteArchivosDTO objArchivo)
        {
            return Reclutamientos.CrearEditarArchivos(objArchivo);
        }

        public Dictionary<string, object> EliminarArchivo(int idArchivo)
        {
            return Reclutamientos.EliminarArchivo(idArchivo);
        }

        public Dictionary<string, object> EliminarArchivoExpediente(int expediente_id, int archivo_id)
        {
            return Reclutamientos.EliminarArchivoExpediente(expediente_id, archivo_id);
        }

        public Dictionary<string, object> VerHistorialExpediente(int expediente_id, int archivo_id)
        {
            return Reclutamientos.VerHistorialExpediente(expediente_id, archivo_id);
        }

        public Dictionary<string, object> DescargarAvanceExcel(string estatus_emp, string cc)
        {
            return Reclutamientos.DescargarAvanceExcel(estatus_emp, cc);
        }
        #endregion

        #region ALTA EMPLEADO (REGION TEMPORAL)
        public Dictionary<string, object> CargarRequisiciones()
        {
            return Reclutamientos.CargarRequisiciones();
        }

        public Dictionary<string, object> CargarAutoriza(int autoriza)
        {
            return Reclutamientos.CargarAutoriza(autoriza);
        }

        public Dictionary<string, object> CargarUsuarioResg(int usuarioResg)
        {
            return Reclutamientos.CargarUsuarioResg(usuarioResg);
        }

        public Dictionary<string, object> CargarDepto(int depto)
        {
            return Reclutamientos.CargarDepto(depto);
        }

        public Dictionary<string, object> CargarTabulador(string cc, int puesto, int? idTabuladorDet)
        {
            return Reclutamientos.CargarTabulador(cc, puesto, idTabuladorDet);
        }

        public Tuple<Stream, string> DescargarArchivoEmpleado(int id)
        {
            return Reclutamientos.DescargarArchivoEmpleado(id);
        }

        public string GenerarCURP(string nombres, string paterno, string materno, SexoEnum sexo, DateTime fechaNacimiento, EstadoEnum estado)
        {
            return Reclutamientos.GenerarCURP(nombres, paterno, materno, sexo, fechaNacimiento, estado);
        }
        public string GetRFC(GestionCandidatosDTO objCandidato)
        {
            return Reclutamientos.GetRFC(objCandidato);
        }
        #endregion

        #region REQUISICION
        public Dictionary<string, object> GetCCs()
        {
            return Reclutamientos.GetCCs();
        }

        public Dictionary<string, object> GetRequisiciones(List<string> ccs, string estatus)
        {
            return Reclutamientos.GetRequisiciones(ccs, estatus);
        }

        public Dictionary<string, object> GetIdRequisicionDisponible()
        {
            return Reclutamientos.GetIdRequisicionDisponible();
        }

        public Dictionary<string, object> GetPlantilla(string cc, int? puesto)
        {
            return Reclutamientos.GetPlantilla(cc, puesto);
        }

        public Dictionary<string, object> GetTipoContrato()
        {
            return Reclutamientos.GetTipoContrato();
        }

        public Dictionary<string, object> GetRazonSolicitud()
        {
            return Reclutamientos.GetRazonSolicitud();
        }

        public Dictionary<string, object> GetJefeInmediato(string cc)
        {
            return Reclutamientos.GetJefeInmediato(cc);
        }

        public Dictionary<string, object> GetAutoriza(string cc)
        {
            return Reclutamientos.GetAutoriza(cc);
        }

        public List<AutocompleteDTO> GetAutocompleteJefe(string term)
        {
            return Reclutamientos.GetAutocompleteJefe(term);
        }

        public Dictionary<string, object> GetSolicita()
        {
            return Reclutamientos.GetSolicita();
        }

        public List<AutocompleteDTO> GetAutocompleteSolicita(string term)
        {
            return Reclutamientos.GetAutocompleteSolicita(term);
        }

        public List<AutocompleteDTO> GetAutocompleteAutoriza(string term, string cc)
        {
            return Reclutamientos.GetAutocompleteAutoriza(term, cc);
        }

        public Dictionary<string, object> GuardarRequisicion(sn_requisicion_personal requisicion)
        {
            return Reclutamientos.GuardarRequisicion(requisicion);
        }

        public Dictionary<string, object> GetInformacionRequisicionConsulta(int requisicion_id, string cc)
        {
            return Reclutamientos.GetInformacionRequisicionConsulta(requisicion_id, cc);
        }

        public Dictionary<string, object> AutorizarRechazarRequisicion(RequisicionRHDTO objDTO)
        {
            return Reclutamientos.AutorizarRechazarRequisicion(objDTO);
        }

        public Dictionary<string, object> GetFechaVigencia7DiasNaturales()
        {
            return Reclutamientos.GetFechaVigencia7DiasNaturales();
        }

        public Dictionary<string, object> EliminarRequisicion(int requisicion_id)
        {
            return Reclutamientos.EliminarRequisicion(requisicion_id);
        }

        public Dictionary<string, object> GetCategoriasPuesto(int idPuesto, string cc)
        {
            return Reclutamientos.GetCategoriasPuesto(idPuesto, cc);
        }

        public Dictionary<string, object> GetTabuladorCategoria(int categoriaID)
        {
            return Reclutamientos.GetTabuladorCategoria(categoriaID);
        }

        public Dictionary<string, object> GetCategoriaPuesto(int tabuladorDetID)
        {
            return Reclutamientos.GetCategoriaPuesto(tabuladorDetID);
        }
        #endregion

        #region COMENTARIOS

        public Dictionary<string, object> GetComentario(int claveEmpleado)
        {
            return Reclutamientos.GetComentario(claveEmpleado);
        }
        public Dictionary<string, object> CrearComentario(int claveEmpleado, string comentario)
        {
            return Reclutamientos.CrearComentario(claveEmpleado, comentario);
        }
        #endregion

        #region FNC GRALES
        public Dictionary<string, object> GetContratoReporte(int clave_empleado)
        {
            return r_reclutamientos.GetContratoReporte(clave_empleado);
        }

        public Dictionary<string, object> GetCategoriasByLineaNegocio(int idLineaNegocio, int idPuesto)
        {
            return r_reclutamientos.GetCategoriasByLineaNegocio(idLineaNegocio, idPuesto);
        }

        public Dictionary<string, object> GetLineaNegocioByCC(string cc)
        {
            return r_reclutamientos.GetLineaNegocioByCC(cc);
        }
        #endregion

        #region REL REGISTROS PATRONALES
        public Dictionary<string, object> GetRelRegPatronales(tblRH_EK_Registros_Patronales objFiltro)
        {
            return Reclutamientos.GetRelRegPatronales(objFiltro);
        }
        public Dictionary<string, object> GetRelRegPatCC(int clave_reg_pat)
        {
            return Reclutamientos.GetRelRegPatCC(clave_reg_pat);
        }
        public Dictionary<string, object> AddCCRegPatronal(int clave_reg_pat, string cc)
        {
            return Reclutamientos.AddCCRegPatronal(clave_reg_pat, cc);
        }
        public Dictionary<string, object> DeleteCCRegPatronal(int clave_reg_pat, string cc)
        {
            return Reclutamientos.DeleteCCRegPatronal(clave_reg_pat, cc);
        }

        public Dictionary<string, object> CrearEditarRegistroPatronal(tblRH_EK_Registros_Patronales objCERegPat, HttpPostedFileBase archivoAdjunto)
        {
            return Reclutamientos.CrearEditarRegistroPatronal(objCERegPat, archivoAdjunto);
        }

        public Dictionary<string, object> DeleteRegistroPatronal(int idRegPat)
        {
            return Reclutamientos.DeleteRegistroPatronal(idRegPat);
        }

        public int GetUltimoIdRegPat()
        {
            return Reclutamientos.GetUltimoIdRegPat();
        }

        public Dictionary<string, object> GetRelRegPatronalesReporte()
        {
            return Reclutamientos.GetRelRegPatronalesReporte();
        }

        public Tuple<Stream, string> DescargarArchivoRegPat(int id)
        {
            return Reclutamientos.DescargarArchivoRegPat(id);
        }
        #endregion

        #region RELACION FASES
        public Dictionary<string, object> GetFasesUsuarios(int idFase)
        {
            return Reclutamientos.GetFasesUsuarios(idFase);
        }
        public Dictionary<string, object> AddFasesUsuarios(int idUsuario, int idFase)
        {
            return Reclutamientos.AddFasesUsuarios(idUsuario, idFase);
        }
        public Dictionary<string, object> DeleteFasesUsuarios(int idUsuario, int idFase)
        {
            return Reclutamientos.DeleteFasesUsuarios(idUsuario, idFase);
        }
        #endregion

        #region CAT CONTRATOS
        public Dictionary<string, object> GetDuracionContratos()
        {
            return Reclutamientos.GetDuracionContratos();
        }

        public Dictionary<string, object> AddDuracionContrato(string duracion_desc, int? dias, int? meses, int? años, bool indef)
        {
            return Reclutamientos.AddDuracionContrato(duracion_desc, dias, meses, años, indef);
        }

        public Dictionary<string, object> DeleteDuracionContrato(int id_duracion)
        {
            return Reclutamientos.DeleteDuracionContrato(id_duracion);
        }

        #endregion

        #region CAT PUESTOS
        public Dictionary<string, object> GetPuestos()
        {
            return Reclutamientos.GetPuestos();
        }
        public Dictionary<string, object> GuardarNuevoPuesto(tblRH_EK_Puestos puesto)
        {
            return Reclutamientos.GuardarNuevoPuesto(puesto);
        }
        public Dictionary<string, object> EditarPuesto(tblRH_EK_Puestos puesto)
        {
            return Reclutamientos.EditarPuesto(puesto);
        }
        public Dictionary<string, object> EliminarPuesto(tblRH_EK_Puestos puesto)
        {
            return Reclutamientos.EliminarPuesto(puesto);
        }

        public Dictionary<string, object> CargarArchivoDescriptor(HttpPostedFileBase file, int puesto)
        {
            return Reclutamientos.CargarArchivoDescriptor(file, puesto);
        }

        public tblRH_EK_Puesto_ArchivoDescriptor GetFileDownloadDescriptor(int id)
        {
            return Reclutamientos.GetFileDownloadDescriptor(id);
        }
        #endregion

        #region PERMISOS
        public bool UsuarioDeConsulta()
        {
            return Reclutamientos.UsuarioDeConsulta();
        }
        public bool UsuarioDeConsultaTeorico() //MARISELA EXAMEN TEORICO
        {
            return Reclutamientos.UsuarioDeConsultaTeorico();
        } 
        public bool UsuarioPermisoAccionEditarFechaCambio()
        {
            return Reclutamientos.UsuarioPermisoAccionEditarFechaCambio();
        }

        public bool UsuarioPermisoAccionOcultarSalarioByCC()
        {
            return Reclutamientos.UsuarioPermisoAccionOcultarSalarioByCC();
        }

        public bool UsuarioPermisoMostrarBotones()
        {
            return Reclutamientos.UsuarioPermisoMostrarBotones();
        }

        public bool UsuarioVerSalarios()
        {
            return Reclutamientos.UsuarioVerSalarios();
        }

        public bool UsuarioOcultarSalariosGernecia(int clv_emp)
        {
            return Reclutamientos.UsuarioOcultarSalariosGernecia(clv_emp);
        }
        #endregion

        #region CAT CATEGORIAS
        public Dictionary<string, object> FillComboPuestos(string cC)
        {
            return Reclutamientos.FillComboPuestos(cC);
        }
        public Dictionary<string, object> GetPuestosCategoriasRelPuesto(string _cc, string _strPuesto, int idPuesto)
        {
            return Reclutamientos.GetPuestosCategoriasRelPuesto(_cc, _strPuesto, idPuesto);
        }
        public Dictionary<string, object> EditarPlantilla(List<InputCategoriasDTO> lstCambios, string cc, int puesto, string descPuesto)
        {
            return r_reclutamientos.EditarPlantilla(lstCambios, cc, puesto, descPuesto);
        }
        public Dictionary<string, object> GetAllPuestos()
        {
            return Reclutamientos.GetAllPuestos();
        }
        public Dictionary<string, object> AddPlantillaPuesto(string _cc, string _strPuesto, int idPuesto, string _strNuevoPuesto, int nuevoPuesto)
        {
            return Reclutamientos.AddPlantillaPuesto(_cc, _strPuesto, idPuesto, _strNuevoPuesto, nuevoPuesto);
        }

        public Dictionary<string, object> GetPuestoDetalle(PuestoDetalleDTO objDTO)
        {
            return Reclutamientos.GetPuestoDetalle(objDTO);
        }
        #endregion

        #region CAT TABULADORES
        public Dictionary<string, object> GetPuestosTabuladores(string cc)
        {
            return Reclutamientos.GetPuestosTabuladores(cc);
        }
        
        public Dictionary<string, object> CrearEditarTabuladorPuesto(string cc, CatTabuladoresDTO objTabulador)
        {
            return Reclutamientos.CrearEditarTabuladorPuesto(cc, objTabulador);
        }

        public Dictionary<string, object> GetReporteTabuladores(string cc)
        {
            return Reclutamientos.GetReporteTabuladores(cc);
        }



        #endregion

        #region DIAS PARA INGRESOS
        public bool GetPuedeEditarDiasDisponibles()
        {
            return Reclutamientos.GetPuedeEditarDiasDisponibles();
        }

        public Dictionary<string, object> GetDiasDisponibles()
        {
            return Reclutamientos.GetDiasDisponibles();
        }

        public Dictionary<string, object> GetDiasDisponiblesParaLimitarFecha()
        {
            return Reclutamientos.GetDiasDisponiblesParaLimitarFecha();
        }

        public Dictionary<string, object> SetDiasDisponibles(int anteriores, int posteriores)
        {
            return Reclutamientos.SetDiasDisponibles(anteriores, posteriores);
        }
        #endregion

        #region EXAMEN MÉDICO
        public void GuardarExamenMedico(int idCandidato, tblRH_REC_ExamenMedico examenMedico, List<tblRH_REC_ExamenMedico_Antecedentes> listaAntecedentes, Byte[] archivoReporteExamenMedico)
        {
            Reclutamientos.GuardarExamenMedico(idCandidato, examenMedico, listaAntecedentes, archivoReporteExamenMedico);
        }
        #endregion

        public bool PermisoTabuladorAltaEmpleado()
        {
            return Reclutamientos.PermisoTabuladorAltaEmpleado();
        }

        public void factorizar()
        {
            Reclutamientos.factorizar();
        }

        #region Datos Perú
        public List<ComboDTO> FillComboAFPPeru() 
        {
            return Reclutamientos.FillComboAFPPeru();
        }
        #endregion

        #region DATOS EMPLEADO PERU
        public Dictionary<string, object> GetSituacion(bool afiliado)
        {
            return Reclutamientos.GetSituacion(afiliado);
        }

        public Dictionary<string, object> GetRucEps()
        {
            return Reclutamientos.GetRucEps();
        }

        public Dictionary<string, object> GetAfps()
        {
            return Reclutamientos.GetAfps();
        }

        public Dictionary<string, object> FillComboTiposTrabajadores()
        {
            return Reclutamientos.FillComboTiposTrabajadores();
        }

        public Dictionary<string,object> FillComboBancosPeru()
        {
            return Reclutamientos.FillComboBancosPeru();
        }

        public Dictionary<string, object> FillComboRegimenLaboralPeru()
        {
            return Reclutamientos.FillComboRegimenLaboralPeru();
        }
        #endregion

        public tblRH_EK_Estados GetEstadoByID(int id)
        {
            return Reclutamientos.GetEstadoByID(id);
        }
    }
}