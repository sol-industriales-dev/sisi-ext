using Core.DTO.Administracion.Seguridad.Indicadores;
using Core.DTO.Administracion.Seguridad;
using Core.Entity.Administrativo.Seguridad.Indicadores;
using Core.Enum.Administracion.Seguridad.Indicadores.ReporteGlobal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Core.Enum.Administracion.Seguridad.Indicadores;
using Core.DTO.Principal.Generales;


namespace Core.DAO.Administracion.Seguridad
{
    public interface ISeguridadIncidentesDAO
    {
        #region CAPTURA INFORME PRELIMINAR
        Dictionary<string, object> GetDatosGeneralesIncidentes(int agrupacionID, int empresa, DateTime fechaInicio, DateTime fechaFin);
        Dictionary<string, object> getInformesPreliminares(List<int> listaDivisiones, List<int> listaLineasNegocio, int idAgrupacion, int idEmpresa, DateTime fechaInicio, DateTime fechaFin, int tipoAccidente, int supervisor, int departamento, int estatus);
        Dictionary<string, object> getInformePreliminarByID(int id);
        Dictionary<string, object> getUsuariosCCSigoPlan(int idEmpresa, int idAgrupacion);
        Dictionary<string, object> getFolio(string cc);
        Dictionary<string, object> getEvaluacionesRiesgo();
        Dictionary<string, object> guardarInforme(InformeDTO captura);
        Dictionary<string, object> updateInforme(tblS_IncidentesInformePreliminar informe);
        bool enviarCorreoPreliminar(InformeDTO captura, FormatoRIADTO informacionReporte, List<Byte[]> archivoInformePreliminar);
        bool enviarCorreoIncidente(IncidenteDTO captura, FormatoRIADTO informacionReporte, List<Byte[]> archivoIncidente);
        Dictionary<string, object> enviarCorreo(int informe_id, List<int> usuarios);
        Dictionary<string, object> getTipoProcedimientosVioladosList();
        /// <summary>
        /// Obtiene el listado de evidencias pertenecientes a un informe.
        /// </summary>
        /// <param name="informeID"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerEvidenciasInforme(int informeID);
        Dictionary<string, object> ObtenerEvidenciasRIA(int informeID);

        /// <summary>
        /// Guarda un conjunto de archivos (evidencias) relacionados a un informe preliminar.
        /// </summary>
        /// <param name="evidencias"></param>
        /// <param name="informeID"></param>
        /// <returns></returns>
        Dictionary<string, object> GuardarEvidencias(List<HttpPostedFileBase> evidencias, int informeID);

        /// <summary>
        /// Descarga una evidencia de un informe.
        /// </summary>
        /// <param name="evidenciaID"></param>
        /// <returns></returns>
        Tuple<Stream, string> DescargarEvidenciaInforme(int evidenciaID);
        Tuple<Stream, string> DescargarEvidenciaRIA(int evidenciaID);

        /// <summary>
        /// Elimina una evidencia ligada a un informe preliminar.
        /// </summary>
        /// <param name="evidenciaID"></param>
        /// <returns></returns>
        Dictionary<string, object> EliminarEvidencia(int evidenciaID);

        /// <summary>
        /// Retorna los datos necesarios para mostrar en el visor de documentos la evidencia indicada.
        /// </summary>
        /// <param name="evidenciaID"></param>
        /// <returns></returns>
        Dictionary<string, object> CargarDatosEvidencia(int evidenciaID);
        Dictionary<string, object> CargarDatosEvidenciaRIA(int evidenciaID);

        /// <summary>
        /// Sube un archivo escaneado (informe preliminar) al servidor.
        /// </summary>
        /// <param name="archivo"></param>
        /// <param name="informeID"></param>
        /// <returns></returns>
        Dictionary<string, object> SubirReporteIncidente(HttpPostedFileBase archivo, int informeID, bool esRIA);

        /// <summary>
        /// Descarga un archivo escaneado de un reporte (preliminar o RIA).
        /// </summary>
        /// <param name="informeID"></param>
        /// <param name="esRIA"></param>
        /// <returns></returns>
        Tuple<Stream, string> DescargarReporte(int informeID, bool esRIA);

        Dictionary<string, object> LlenarComboSupervisorIncidente();
        Dictionary<string, object> LlenarComboDepartamentoIncidente();
        Dictionary<string, object> EliminarIncidente(int id);
        #endregion

        #region CAPTURA INCIDENTE
        Dictionary<string, object> getTiposAccidentesList();
        Dictionary<string, object> GetSubclasificacionesAccidente();
        Dictionary<string, object> getDepartamentosList();
        Dictionary<string, object> getSupervisoresList();
        Dictionary<string, object> getSupervisoresIncidentesList();
        Dictionary<string, object> getTiposLesionList();
        Dictionary<string, object> getPartesCuerposList();
        Dictionary<string, object> getTiposContactoList();
        Dictionary<string, object> getAgentesImplicados();
        Dictionary<string, object> getExperienciaEmpleados();
        Dictionary<string, object> getAntiguedadEmpleados();
        Dictionary<string, object> getTurnosEmpleado();
        Dictionary<string, object> getProtocolosTrabajoList();
        Dictionary<string, object> getTecnicasInvestigacion();
        Dictionary<string, object> getEmpleadosCCList(string cc);
        Dictionary<string, object> getEmpleadosContratistasList(int claveContratista);
        Dictionary<string, object> obtenerCentrosCostos();
        Dictionary<string, object> ObtenerCentrosCostosUsuario();
        Dictionary<string, object> getSubcontratistas();
        Dictionary<string, object> getInfoEmpleado(int claveEmpleado, bool esContratista, int idEmpresaContratista);
        Dictionary<string, object> getInfoEmpleadoContratista(int empleado_id);
        Dictionary<string, object> getUsersEnkontrol(string user);
        Dictionary<string, object> getUsersEnkontrolByClave(string clave);
        Dictionary<string, object> getPrioridadesActividad();
        Dictionary<string, object> guardarEmpleadoSubcontratista(tblS_IncidentesEmpleadosContratistas empleado);
        Dictionary<string, object> guardarIncidente(IncidenteDTO captura);

        /// <summary>
        /// Obtiene todos los datos relevantes sobre un incidente en base al ID del informe.
        /// </summary>
        /// <param name="informeID"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerIncidentePorInformeID(int informeID);
        Dictionary<string, object> ObtenerInformeParaReporte(int informeID);
        /// <summary>
        /// Obtiene un listado de usuarios de SIGOPLAN en base a una palabra de búsqueda.
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        object GetUsuariosAutocomplete(string term);
        #endregion

        #region CAPTURA INFORMACION COLABORADORES
        Dictionary<string, object> getInformacionColaboradores(int idAgrupacion, DateTime fechaInicio, DateTime fechaFin, int idEmpresa);
        Dictionary<string, object> getInformacionColaboradoresByID(int id);
        //raguilarr 24/12/19
        Dictionary<string, object> getInformacionColaboradoresByIDDetalle(int id);
        Dictionary<string, object> getFechasUltimoCorte(int idEmpresa, int idAgrupacion);
        //Dictionary<string, object> guardarRegistroInformacion(tblS_IncidentesInformacionColaboradores registroInformacion);
        Dictionary<string, object> GuardarRegistroInformacion(
            tblS_IncidentesInformacionColaboradores registroInformacion,
            List<tblS_IncidentesInformacionColaboradoresDetalle> lstDetalle,
            List<tblS_IncidentesInformacionColaboradoresClasificacion> listaClasificacion
        );
        Dictionary<string, object> UpdateRegistroInformacion(
            tblS_IncidentesInformacionColaboradores registroInformacion,
            List<tblS_IncidentesInformacionColaboradoresDetalle> lstDetalle,
            List<tblS_IncidentesInformacionColaboradoresClasificacion> listaClasificacion
        );
        Dictionary<string, object> EliminarHHT(int id);
        #endregion

        #region DASHBOARD
        Dictionary<string, object> getIncidentesRegistrables(busqDashboardDTO busq);
        Dictionary<string, object> getIncidentesReportables(busqDashboardDTO busq);
        Dictionary<string, object> getHorasHombreLostDay(busqDashboardDTO busq);
        Dictionary<string, object> getPotencialSeveridad(busqDashboardDTO busq);
        Dictionary<string, object> getIncidentesPorMes(busqDashboardDTO busq);
        Dictionary<string, object> getIncidentesRegistrablesXmes(busqDashboardDTO busq, TipoCargaGraficaEnum tipoCarga);
        Dictionary<string, object> getDanoInstalacionEquipo(busqDashboardDTO busq);
        Dictionary<string, object> getIncidentesDepartamento(busqDashboardDTO busq);
        Dictionary<string, object> getTasaIncidencias(busqDashboardDTO busq, TipoCargaGraficaEnum tipoCarga);
        Dictionary<string, object> getTIFR(busqDashboardDTO busq, TipoCargaGraficaEnum tipoCarga);
        Dictionary<string, object> getTPDFR(busqDashboardDTO busq, TipoCargaGraficaEnum tipoCarga);
        Dictionary<string, object> getIncidenciasPresentadas(busqDashboardDTO busq);
        Dictionary<string, object> getIncidenciasPresentadasTipo(string tipo, busqDashboardDTO busq);
        Dictionary<string, object> getAccidentabilidad(busqDashboardDTO busq);
        Dictionary<string, object> getAccidentabilidadTop(busqDashboardDTO busq);
        Dictionary<string, object> getCausasIncidencias(busqDashboardDTO busq);
        Dictionary<string, object> ObtenerMetasGrafica();
        Dictionary<string, object> AgregarMetaGrafica(tblS_IncidentesMetasGrafica meta);
        Dictionary<string, object> EliminarMetaGrafica(int id);
        Dictionary<string, object> GetDatosLesionesPersonal(busqDashboardDTO busq);
        Dictionary<string, object> GetDatosDañosMateriales(busqDashboardDTO busq);
        #endregion

        #region
        /// <summary>
        /// Obtiene todos los datos necesarios para generar un reporte global dependiendo del tipo de reporte especificado.
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <returns></returns>
        ReporteGlobalDTO ObtenerDatosReporteGlobal(TipoReporteGlobalEnum tipoReporte);

        Dictionary<string, object> EnviarCorreoReporteGlobal(List<Byte[]> pdf);
        #endregion

        #region CALCULOS DE HORAS TRABAJADAS - HORAS HOMBRE
        Dictionary<string, object> GetDatos(CalculosHorasHombreDTO objSelected);

        Dictionary<string, object> ObtenerComboCCAmbasEmpresas(bool incContratista, int? division);

        Dictionary<string, object> ObtenerComboCCAmbasEmpresas_SoloGrupos(bool incContratista, int? division);

        Dictionary<string, object> ObtenerComboCCAmbasEmpresasDivisionesLineas(bool incContratista, List<int> listaDivisiones, List<int> listaLineasNegocio);
        #endregion

        #region CONTRATISTAS
        bool ValidarAccesoContratista();
        #endregion

        #region CATÁLOGO AGRUPACIÓN CONTRATISTAS
        List<IncidentesAgrupacionesContratistasDTO> GetAgrupacionesContratistas(IncidentesAgrupacionesContratistasDTO objFiltro);

        bool existeNomAgrupacion(IncidentesAgrupacionesContratistasDTO objAgrupacion);

        bool CrearAgrupacion(IncidentesAgrupacionesContratistasDTO objAgrupacion);

        bool ActualizarAgrupacion(IncidentesAgrupacionesContratistasDTO objAgrupacion);

        bool EliminarAgrupacion(int idAgrupacion);

        List<Core.DTO.Principal.Generales.ComboDTO> FillCboAgrupaciones();

        List<IncidentesAgrupacionesContratistasDTO> GetContratistas(int idAgrupacion);

        bool existeContratistaEnAgrupacion(IncidentesAgrupacionesContratistasDTO objAgrupacion);

        bool CrearContratistaEnAgrupacion(IncidentesAgrupacionesContratistasDTO objAgrupacion);

        List<Core.DTO.Principal.Generales.ComboDTO> FillCboContratistas();

        bool EliminarContratistaEnAgrupacion(int idAgrupacionDet);
        #endregion

        #region CATÁLOGO RELACIÓN CONTRATISTA - EMPRESA
        List<IncidentesRelEmpresaContratistasDTO> GetEmpresaRelContratistas(IncidentesRelEmpresaContratistasDTO objFiltro);

        bool CrearRelEmpresaContratista(IncidentesRelEmpresaContratistasDTO objRel);

        bool ActualizarRelEmpresaContratista(IncidentesRelEmpresaContratistasDTO objRel);

        bool EliminarRelEmpresaContratista(int idRel);

        List<Core.DTO.Principal.Generales.ComboDTO> FillCboContratistasSP();

        bool DisponibleRelEmpresaContratista(IncidentesRelEmpresaContratistasDTO objRel);
        #endregion
    }
}
