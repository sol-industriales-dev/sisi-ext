using Core.DAO.Administracion.Seguridad;
using Core.DTO.Administracion.Seguridad.Indicadores;
using Core.DTO.Administracion.Seguridad;
using Core.Entity.Administrativo.Seguridad.Indicadores;
using Core.Enum.Administracion.Seguridad.Indicadores.ReporteGlobal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using Core.Enum.Administracion.Seguridad.Indicadores;
using Core.DTO.Principal.Generales;

namespace Core.Service.Administracion.Seguridad
{
    public class SeguridadIncidentesServices : ISeguridadIncidentesDAO
    {

        private ISeguridadIncidentesDAO m_SeguridadIncidentesDAO;
        public ISeguridadIncidentesDAO SeguridadIncidentesDAO
        {
            get { return m_SeguridadIncidentesDAO; }
            set { m_SeguridadIncidentesDAO = value; }
        }
        public SeguridadIncidentesServices(ISeguridadIncidentesDAO SeguridadIncidentesDAO)
        {
            this.SeguridadIncidentesDAO = SeguridadIncidentesDAO;
        }

        #region CAPTURA INFORME PRELIMINAR

        public Dictionary<string, object> GetDatosGeneralesIncidentes(int agrupacionID, int empresa, DateTime fechaInicio, DateTime fechaFin)
        {
            return SeguridadIncidentesDAO.GetDatosGeneralesIncidentes(agrupacionID, empresa, fechaInicio, fechaFin);
        }
        public Dictionary<string, object> getInformesPreliminares(List<int> listaDivisiones, List<int> listaLineasNegocio, int idAgrupacion, int idEmpresa, DateTime fechaInicio, DateTime fechaFin, int tipoAccidente, int supervisor, int departamentoID, int estatus)
        {
            return SeguridadIncidentesDAO.getInformesPreliminares(listaDivisiones, listaLineasNegocio, idAgrupacion, idEmpresa, fechaInicio, fechaFin, tipoAccidente, supervisor, departamentoID, estatus);
        }
        public Dictionary<string, object> getInformePreliminarByID(int id)
        {
            return SeguridadIncidentesDAO.getInformePreliminarByID(id);
        }
        public Dictionary<string, object> getUsuariosCCSigoPlan(int idEmpresa, int idAgrupacion)
        {
            return SeguridadIncidentesDAO.getUsuariosCCSigoPlan(idEmpresa, idAgrupacion);
        }
        public Dictionary<string, object> getFolio(string cc)
        {
            return SeguridadIncidentesDAO.getFolio(cc);
        }
        public Dictionary<string, object> getEvaluacionesRiesgo()
        {
            return SeguridadIncidentesDAO.getEvaluacionesRiesgo();
        }
        public Dictionary<string, object> guardarInforme(InformeDTO captura)
        {
            return SeguridadIncidentesDAO.guardarInforme(captura);
        }
        public Dictionary<string, object> updateInforme(tblS_IncidentesInformePreliminar informe)
        {
            return SeguridadIncidentesDAO.updateInforme(informe);
        }
        public bool enviarCorreoPreliminar(InformeDTO captura, FormatoRIADTO informacionReporte, List<Byte[]> archivoInformePreliminar)
        {
            return SeguridadIncidentesDAO.enviarCorreoPreliminar(captura, informacionReporte, archivoInformePreliminar);
        }
        public bool enviarCorreoIncidente(IncidenteDTO captura, FormatoRIADTO informacionReporte, List<Byte[]> archivoIncidente)
        {
            return SeguridadIncidentesDAO.enviarCorreoIncidente(captura, informacionReporte, archivoIncidente);
        }
        public Dictionary<string, object> enviarCorreo(int informe_id, List<int> usuarios)
        {
            return SeguridadIncidentesDAO.enviarCorreo(informe_id, usuarios);
        }

        public Dictionary<string, object> ObtenerEvidenciasInforme(int informeID)
        {
            return SeguridadIncidentesDAO.ObtenerEvidenciasInforme(informeID);
        }

        public Dictionary<string, object> ObtenerEvidenciasRIA(int informeID)
        {
            return SeguridadIncidentesDAO.ObtenerEvidenciasRIA(informeID);
        }

        public Dictionary<string, object> GuardarEvidencias(List<HttpPostedFileBase> evidencias, int informeID)
        {
            return SeguridadIncidentesDAO.GuardarEvidencias(evidencias, informeID);
        }

        public Tuple<Stream, string> DescargarEvidenciaInforme(int evidenciaID)
        {
            return SeguridadIncidentesDAO.DescargarEvidenciaInforme(evidenciaID);
        }

        public Tuple<Stream, string> DescargarEvidenciaRIA(int evidenciaID)
        {
            return SeguridadIncidentesDAO.DescargarEvidenciaRIA(evidenciaID);
        }

        public Dictionary<string, object> EliminarEvidencia(int evidenciaID)
        {
            return SeguridadIncidentesDAO.EliminarEvidencia(evidenciaID);
        }

        public Dictionary<string, object> CargarDatosEvidencia(int evidenciaID)
        {
            return SeguridadIncidentesDAO.CargarDatosEvidencia(evidenciaID);
        }

        public Dictionary<string, object> CargarDatosEvidenciaRIA(int evidenciaID)
        {
            return SeguridadIncidentesDAO.CargarDatosEvidenciaRIA(evidenciaID);
        }

        public Dictionary<string, object> SubirReporteIncidente(HttpPostedFileBase archivo, int informeID, bool esRIA)
        {
            return SeguridadIncidentesDAO.SubirReporteIncidente(archivo, informeID, esRIA);
        }

        public Tuple<Stream, string> DescargarReporte(int informeID, bool esRIA)
        {
            return SeguridadIncidentesDAO.DescargarReporte(informeID, esRIA);
        }
        public Dictionary<string, object> getTipoProcedimientosVioladosList()
        {
            return SeguridadIncidentesDAO.getTipoProcedimientosVioladosList();
        }
        public Dictionary<string, object> LlenarComboSupervisorIncidente()
        {
            return SeguridadIncidentesDAO.LlenarComboSupervisorIncidente();
        }
        public Dictionary<string, object> LlenarComboDepartamentoIncidente()
        {
            return SeguridadIncidentesDAO.LlenarComboDepartamentoIncidente();
        }

        public Dictionary<string, object> EliminarIncidente(int id)
        {
            return SeguridadIncidentesDAO.EliminarIncidente(id);
        }
        #endregion

        #region CAPTURA ACCIDENTE
        public Dictionary<string, object> getTiposAccidentesList()
        {
            return SeguridadIncidentesDAO.getTiposAccidentesList();
        }
        public Dictionary<string, object> GetSubclasificacionesAccidente()
        {
            return SeguridadIncidentesDAO.GetSubclasificacionesAccidente();
        }
        public Dictionary<string, object> getDepartamentosList()
        {
            return SeguridadIncidentesDAO.getDepartamentosList();
        }
        public Dictionary<string, object> getSupervisoresList()
        {
            return SeguridadIncidentesDAO.getSupervisoresList();
        }
        public Dictionary<string, object> getSupervisoresIncidentesList()
        {
            return SeguridadIncidentesDAO.getSupervisoresIncidentesList();
        }
        public Dictionary<string, object> getTiposLesionList()
        {
            return SeguridadIncidentesDAO.getTiposLesionList();
        }
        public Dictionary<string, object> getPartesCuerposList()
        {
            return SeguridadIncidentesDAO.getPartesCuerposList();
        }
        public Dictionary<string, object> getTiposContactoList()
        {
            return SeguridadIncidentesDAO.getTiposContactoList();
        }
        public Dictionary<string, object> getAgentesImplicados()
        {
            return SeguridadIncidentesDAO.getAgentesImplicados();
        }
        public Dictionary<string, object> getExperienciaEmpleados()
        {
            return SeguridadIncidentesDAO.getExperienciaEmpleados();
        }
        public Dictionary<string, object> getAntiguedadEmpleados()
        {
            return SeguridadIncidentesDAO.getAntiguedadEmpleados();
        }
        public Dictionary<string, object> getTurnosEmpleado()
        {
            return SeguridadIncidentesDAO.getTurnosEmpleado();
        }
        public Dictionary<string, object> getProtocolosTrabajoList()
        {
            return SeguridadIncidentesDAO.getProtocolosTrabajoList();
        }
        public Dictionary<string, object> getTecnicasInvestigacion()
        {
            return SeguridadIncidentesDAO.getTecnicasInvestigacion();
        }
        public Dictionary<string, object> getEmpleadosCCList(string cc)
        {
            return SeguridadIncidentesDAO.getEmpleadosCCList(cc);
        }
        public Dictionary<string, object> getEmpleadosContratistasList(int claveContratista)
        {
            return SeguridadIncidentesDAO.getEmpleadosContratistasList(claveContratista);
        }
        public Dictionary<string, object> obtenerCentrosCostos()
        {
            return SeguridadIncidentesDAO.obtenerCentrosCostos();
        }
        public Dictionary<string, object> ObtenerCentrosCostosUsuario()
        {
            return SeguridadIncidentesDAO.ObtenerCentrosCostosUsuario();
        }
        public Dictionary<string, object> getSubcontratistas()
        {
            return SeguridadIncidentesDAO.getSubcontratistas();
        }
        public Dictionary<string, object> getInfoEmpleado(int claveEmpleado, bool esContratista, int idEmpresaContratista)
        {
            return SeguridadIncidentesDAO.getInfoEmpleado(claveEmpleado, esContratista, idEmpresaContratista);
        }
        public Dictionary<string, object> getInfoEmpleadoContratista(int empleado_id)
        {
            return SeguridadIncidentesDAO.getInfoEmpleadoContratista(empleado_id);
        }
        public Dictionary<string, object> getUsersEnkontrol(string user)
        {
            return SeguridadIncidentesDAO.getUsersEnkontrol(user);
        }
        public Dictionary<string, object> getUsersEnkontrolByClave(string clave)
        {
            return SeguridadIncidentesDAO.getUsersEnkontrolByClave(clave);
        }
        public Dictionary<string, object> getPrioridadesActividad()
        {
            return SeguridadIncidentesDAO.getPrioridadesActividad();
        }
        public Dictionary<string, object> guardarEmpleadoSubcontratista(tblS_IncidentesEmpleadosContratistas empleado)
        {
            return SeguridadIncidentesDAO.guardarEmpleadoSubcontratista(empleado);
        }
        public Dictionary<string, object> guardarIncidente(IncidenteDTO captura)
        {
            return SeguridadIncidentesDAO.guardarIncidente(captura);
        }

        public Dictionary<string, object> ObtenerIncidentePorInformeID(int informeID)
        {
            return SeguridadIncidentesDAO.ObtenerIncidentePorInformeID(informeID);
        }

        public Dictionary<string, object> ObtenerInformeParaReporte(int informeID)
        {
            return SeguridadIncidentesDAO.ObtenerInformeParaReporte(informeID);
        }

        public object GetUsuariosAutocomplete(string term)
        {
            return SeguridadIncidentesDAO.GetUsuariosAutocomplete(term);
        }
        #endregion

        #region CAPTURA INFORMACION COLABORADORES
        public Dictionary<string, object> getInformacionColaboradores(int idAgrupacion, DateTime fechaInicio, DateTime fechaFin, int idEmpresa)
        {
            return SeguridadIncidentesDAO.getInformacionColaboradores(idAgrupacion, fechaInicio, fechaFin, idEmpresa);
        }
        public Dictionary<string, object> getInformacionColaboradoresByID(int id)
        {
            return SeguridadIncidentesDAO.getInformacionColaboradoresByID(id);
        }
        //raguilar  24/12/19
        public Dictionary<string, object> getInformacionColaboradoresByIDDetalle(int id)
        {
            return SeguridadIncidentesDAO.getInformacionColaboradoresByIDDetalle(id);
        }
        public Dictionary<string, object> getFechasUltimoCorte(int idEmpresa, int idAgrupacion)
        {
            return SeguridadIncidentesDAO.getFechasUltimoCorte(idEmpresa, idAgrupacion);
        }
        public Dictionary<string, object> GuardarRegistroInformacion(
            tblS_IncidentesInformacionColaboradores registroInformacion,
            List<tblS_IncidentesInformacionColaboradoresDetalle> lstDetalle,
            List<tblS_IncidentesInformacionColaboradoresClasificacion> listaClasificacion
        )
        {
            return SeguridadIncidentesDAO.GuardarRegistroInformacion(registroInformacion, lstDetalle, listaClasificacion);
        }
        public Dictionary<string, object> UpdateRegistroInformacion(
            tblS_IncidentesInformacionColaboradores registroInformacion,
            List<tblS_IncidentesInformacionColaboradoresDetalle> lstDetalle,
            List<tblS_IncidentesInformacionColaboradoresClasificacion> listaClasificacion
        )
        {
            return SeguridadIncidentesDAO.UpdateRegistroInformacion(registroInformacion, lstDetalle, listaClasificacion);
        }

        public Dictionary<string, object> EliminarHHT(int id)
        {
            return SeguridadIncidentesDAO.EliminarHHT(id);
        }
        #endregion

        #region DASHBOARD
        public Dictionary<string, object> getIncidentesRegistrables(busqDashboardDTO busq)
        {
            return SeguridadIncidentesDAO.getIncidentesRegistrables(busq);
        }
        public Dictionary<string, object> getIncidentesReportables(busqDashboardDTO busq)
        {
            return SeguridadIncidentesDAO.getIncidentesReportables(busq);
        }
        public Dictionary<string, object> getHorasHombreLostDay(busqDashboardDTO busq)
        {
            return SeguridadIncidentesDAO.getHorasHombreLostDay(busq);
        }
        public Dictionary<string, object> getPotencialSeveridad(busqDashboardDTO busq)
        {
            return SeguridadIncidentesDAO.getPotencialSeveridad(busq);
        }
        public Dictionary<string, object> getIncidentesPorMes(busqDashboardDTO busq)
        {
            return SeguridadIncidentesDAO.getIncidentesPorMes(busq);
        }
        public Dictionary<string, object> getIncidentesRegistrablesXmes(busqDashboardDTO busq, TipoCargaGraficaEnum tipoCarga)
        {
            return SeguridadIncidentesDAO.getIncidentesRegistrablesXmes(busq,tipoCarga);
        }
        public Dictionary<string, object> getDanoInstalacionEquipo(busqDashboardDTO busq)
        {
            return SeguridadIncidentesDAO.getDanoInstalacionEquipo(busq);
        }
        public Dictionary<string, object> getIncidentesDepartamento(busqDashboardDTO busq)
        {
            return SeguridadIncidentesDAO.getIncidentesDepartamento(busq);
        }
        public Dictionary<string, object> getTasaIncidencias(busqDashboardDTO busq, TipoCargaGraficaEnum tipoCarga)
        {
            return SeguridadIncidentesDAO.getTasaIncidencias(busq, tipoCarga);
        }
        public Dictionary<string, object> getTIFR(busqDashboardDTO busq, TipoCargaGraficaEnum tipoCarga)
        {
            return SeguridadIncidentesDAO.getTIFR(busq, tipoCarga);
        }
        public Dictionary<string, object> getTPDFR(busqDashboardDTO busq, TipoCargaGraficaEnum tipoCarga)
        {
            return SeguridadIncidentesDAO.getTPDFR(busq, tipoCarga);
        }
        public Dictionary<string, object> getIncidenciasPresentadas(busqDashboardDTO busq)
        {
            return SeguridadIncidentesDAO.getIncidenciasPresentadas(busq);
        }
        public Dictionary<string, object> getIncidenciasPresentadasTipo(string tipo, busqDashboardDTO busq)
        {
            return SeguridadIncidentesDAO.getIncidenciasPresentadasTipo(tipo, busq);
        }
        public Dictionary<string, object> getAccidentabilidad(busqDashboardDTO busq)
        {
            return SeguridadIncidentesDAO.getAccidentabilidad(busq);
        }
        public Dictionary<string, object> getAccidentabilidadTop(busqDashboardDTO busq)
        {
            return SeguridadIncidentesDAO.getAccidentabilidadTop(busq);
        }
        public Dictionary<string, object> getCausasIncidencias(busqDashboardDTO busq)
        {
            return SeguridadIncidentesDAO.getCausasIncidencias(busq);
        }
        public Dictionary<string, object> ObtenerMetasGrafica()
        {
            return SeguridadIncidentesDAO.ObtenerMetasGrafica();
        }

        public Dictionary<string, object> AgregarMetaGrafica(tblS_IncidentesMetasGrafica meta)
        {
            return SeguridadIncidentesDAO.AgregarMetaGrafica(meta);
        }

        public Dictionary<string, object> EliminarMetaGrafica(int id)
        {
            return SeguridadIncidentesDAO.EliminarMetaGrafica(id);
        }

        public Dictionary<string, object> GetDatosLesionesPersonal(busqDashboardDTO busq)
        {
            return SeguridadIncidentesDAO.GetDatosLesionesPersonal(busq);
        }

        public Dictionary<string, object> GetDatosDañosMateriales(busqDashboardDTO busq)
        {
            return SeguridadIncidentesDAO.GetDatosDañosMateriales(busq);
        }
        #endregion

        #region Reporte Global
        public ReporteGlobalDTO ObtenerDatosReporteGlobal(TipoReporteGlobalEnum tipoReporte)
        {
            return SeguridadIncidentesDAO.ObtenerDatosReporteGlobal(tipoReporte);
        }

        public Dictionary<string, object> EnviarCorreoReporteGlobal(List<byte[]> pdf)
        {
            return SeguridadIncidentesDAO.EnviarCorreoReporteGlobal(pdf);
        }
        #endregion

        #region CALCULO DE HORAS TRABAJADAS - HORAS HOMBRE
        public Dictionary<string, object> GetDatos(CalculosHorasHombreDTO objSelected)
        {
            return SeguridadIncidentesDAO.GetDatos(objSelected);
        }

        public Dictionary<string, object> ObtenerComboCCAmbasEmpresas(bool incContratista, int? division)
        {
            return SeguridadIncidentesDAO.ObtenerComboCCAmbasEmpresas(incContratista, division);
        }
        public Dictionary<string, object> ObtenerComboCCAmbasEmpresas_SoloGrupos(bool incContratista, int? division)
        {
            return SeguridadIncidentesDAO.ObtenerComboCCAmbasEmpresas_SoloGrupos(incContratista, division);
        }

        public Dictionary<string, object> ObtenerComboCCAmbasEmpresasDivisionesLineas(bool incContratista, List<int> listaDivisiones, List<int> listaLineasNegocio)
        {
            return SeguridadIncidentesDAO.ObtenerComboCCAmbasEmpresasDivisionesLineas(incContratista, listaDivisiones, listaLineasNegocio);
        }
        #endregion

        #region CONTRATISTAS
        public bool ValidarAccesoContratista()
        {
            return SeguridadIncidentesDAO.ValidarAccesoContratista();
        }
        #endregion

        #region CATÁLOGO AGRUPACIÓN CONTRATISTAS
        public List<IncidentesAgrupacionesContratistasDTO> GetAgrupacionesContratistas(IncidentesAgrupacionesContratistasDTO objFiltro)
        {
            return SeguridadIncidentesDAO.GetAgrupacionesContratistas(objFiltro);
        }

        public bool existeNomAgrupacion(IncidentesAgrupacionesContratistasDTO objAgrupacion)
        {
            return SeguridadIncidentesDAO.existeNomAgrupacion(objAgrupacion);
        }

        public bool CrearAgrupacion(IncidentesAgrupacionesContratistasDTO objAgrupacion)
        {
            return SeguridadIncidentesDAO.CrearAgrupacion(objAgrupacion);
        }

        public bool ActualizarAgrupacion(IncidentesAgrupacionesContratistasDTO objAgrupacion)
        {
            return SeguridadIncidentesDAO.ActualizarAgrupacion(objAgrupacion);
        }

        public bool EliminarAgrupacion(int idAgrupacion)
        {
            return SeguridadIncidentesDAO.EliminarAgrupacion(idAgrupacion);
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillCboAgrupaciones()
        {
            return SeguridadIncidentesDAO.FillCboAgrupaciones();
        }

        public List<IncidentesAgrupacionesContratistasDTO> GetContratistas(int idAgrupacion)
        {
            return SeguridadIncidentesDAO.GetContratistas(idAgrupacion);
        }

        public bool existeContratistaEnAgrupacion(IncidentesAgrupacionesContratistasDTO objAgrupacion)
        {
            return SeguridadIncidentesDAO.existeContratistaEnAgrupacion(objAgrupacion);
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillCboContratistas()
        {
            return SeguridadIncidentesDAO.FillCboContratistas();
        } 

        public bool CrearContratistaEnAgrupacion(IncidentesAgrupacionesContratistasDTO objAgrupacion)
        {
            return SeguridadIncidentesDAO.CrearContratistaEnAgrupacion(objAgrupacion);
        }

        public bool EliminarContratistaEnAgrupacion(int idAgrupacionDet)
        {
            return SeguridadIncidentesDAO.EliminarContratistaEnAgrupacion(idAgrupacionDet);
        }
        #endregion

        #region CATÁLOGO RELACIÓN CONTRATISTA - EMPRESA
        public List<IncidentesRelEmpresaContratistasDTO> GetEmpresaRelContratistas(IncidentesRelEmpresaContratistasDTO objFiltro)
        {
            return SeguridadIncidentesDAO.GetEmpresaRelContratistas(objFiltro);
        }

        public bool CrearRelEmpresaContratista(IncidentesRelEmpresaContratistasDTO objRel)
        {
            return SeguridadIncidentesDAO.CrearRelEmpresaContratista(objRel);
        }

        public bool ActualizarRelEmpresaContratista(IncidentesRelEmpresaContratistasDTO objRel)
        {
            return SeguridadIncidentesDAO.ActualizarRelEmpresaContratista(objRel);
        }

        public bool EliminarRelEmpresaContratista(int idRel)
        {
            return SeguridadIncidentesDAO.EliminarRelEmpresaContratista(idRel);
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillCboContratistasSP()
        {
            return SeguridadIncidentesDAO.FillCboContratistasSP();
        }

        public bool DisponibleRelEmpresaContratista(IncidentesRelEmpresaContratistasDTO objRel)
        {
            return SeguridadIncidentesDAO.DisponibleRelEmpresaContratista(objRel);
        }
        #endregion
    }
}
