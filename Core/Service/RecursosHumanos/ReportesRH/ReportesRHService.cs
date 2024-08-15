using Core.DAO.RecursosHumanos.ReportesRH;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos;
using Core.DTO.RecursosHumanos.Constancias;
using Core.Entity.RecursosHumanos.Catalogo;
using Core.Entity.RecursosHumanos.Reclutamientos;
using Core.Entity.RecursosHumanos.Reportes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.RecursosHumanos.ReportesRH
{
    public class ReportesRHService : IReportesRHDAO
    {
        public IReportesRHDAO ReportesInterfaz { get; set; }
        #region Atributos
        private IReportesRHDAO m_reportesRHDAO;
        #endregion

        #region Propiedades
        public IReportesRHDAO ReportesRHDAO
        {
            get { return m_reportesRHDAO; }
            set { m_reportesRHDAO = value; }
        }
        #endregion

        #region Constructores
        public ReportesRHService(IReportesRHDAO reportesDAO)
        {
            this.ReportesRHDAO = reportesDAO;
        }
        #endregion
        public List<ComboDTO> getListaCC()
        {
            return this.ReportesRHDAO.getListaCC();
        }
        public List<ComboDTO> getListaCCRH()
        {
            return this.ReportesRHDAO.getListaCCRH();
        }
        public List<ComboDTO> getListaCCRHBajas()
        {
            return this.ReportesRHDAO.getListaCCRHBajas();
        }
        public List<ComboDTO> getListaConceptosBaja()
        {
            return this.ReportesRHDAO.getListaConceptosBaja();
        }
        public List<ComboDTO> getListaEmpleadosByCC(List<string> cc)
        {
            return this.ReportesRHDAO.getListaEmpleadosByCC(cc);
        }
        public List<RepAltasDTO> getListaAltas(List<string> cc, DateTime fechaInicio, DateTime fechaFin)
        {
            return this.ReportesRHDAO.getListaAltas(cc, fechaInicio, fechaFin);
        }
        public List<RepBajasDTO> getLayoutBajas(List<string> cc, List<string> concepto, DateTime fechaInicio, DateTime fechaFin, bool tipo, List<int> estatus, DateTime? fechaContaInicio, DateTime? fechaContaFin)
        {
            return this.ReportesRHDAO.getLayoutBajas(cc, concepto, fechaInicio, fechaFin, tipo, estatus, fechaContaInicio, fechaContaFin);
        }
        public List<RepBajasDTO> getListaBajas(List<string> cc, List<string> concepto, DateTime fechaInicio, DateTime fechaFin, bool tipo, List<int> estatus, DateTime? fechaContaInicio, DateTime? fechaContaFin, int? tipoBajas)
        {
            return this.ReportesRHDAO.getListaBajas(cc, concepto, fechaInicio, fechaFin, tipo, estatus, fechaContaInicio, fechaContaFin, tipoBajas);
        }
        public List<RepCambiosDTO> getListaCambios(List<string> cc, List<string> concepto, List<string> empleado, DateTime fechaInicio, DateTime fechaFin)
        {
            return this.ReportesRHDAO.getListaCambios(cc, concepto, empleado, fechaInicio, fechaFin);
        }
        public List<RepModificacionesDTO> getListaModificaciones(List<string> cc, List<string> concepto, DateTime fechaInicio, DateTime fechaFin)
        {
            return this.ReportesRHDAO.getListaModificaciones(cc, concepto, fechaInicio, fechaFin);
        }

        public List<RepActivosDTO> getListaActivos(List<string> cc)
        {
            return this.ReportesRHDAO.getListaActivos(cc);
        }

        public Dictionary<string, object> GetRptActivos(List<string> cc)
        {
            return this.ReportesRHDAO.GetRptActivos(cc);
        }

        public List<LayoutAltasRHDTO> getListaEmpleados(List<string> cc, DateTime fechaInicio, DateTime fechaFin)
        {
            return this.ReportesRHDAO.getListaEmpleados(cc, fechaInicio, fechaFin);
        }
        public List<LayoutIncidenciasRHDTO> getListaEmpleadosIncidencias(List<string> cc, DateTime fechaInicio, DateTime fechaFin)
        {
            return this.ReportesRHDAO.getListaEmpleadosIncidencias(cc, fechaInicio, fechaFin);
        }

        public bool setUsuariosBaja(tblRH_LayautBajaEmpleados obj)
        {
            return this.ReportesRHDAO.setUsuariosBaja(obj);
        }

        #region REPORTE GENERAL
        public Dictionary<string, object> GetRptGeneral(List<string> cc)
        {
            return this.ReportesRHDAO.GetRptGeneral(cc);
        }
        #endregion

        #region FILL COMBO
        public List<ComboDTO> CatIncidencia()
        {
            return this.ReportesRHDAO.CatIncidencia();
        }
        #endregion

        #region RPT STAFFING GUIDE
        public Dictionary<string, object> GetPuestosCategoriasRelPuesto(string _cc, string _strPuesto)
        {
            return this.ReportesRHDAO.GetPuestosCategoriasRelPuesto(_cc, _strPuesto);
        }
        public MemoryStream crearReporte(string cc)
        {
            return this.ReportesRHDAO.crearReporte(cc);
        }

        #endregion

        #region DASHBOARD
        public Dictionary<string, object> GetDashboard(List<string> ccs, DateTime fechaInicio, DateTime fechaFin)
        {
            return this.ReportesRHDAO.GetDashboard(ccs, fechaInicio, fechaFin);
        }

        #endregion

        #region Constancias
        public Dictionary<string, object> GetEmpleadosPrestamos(List<string> cc)
        {
            return this.ReportesRHDAO.GetEmpleadosPrestamos(cc);
        }
        public Dictionary<string, object> GetConsultaCC(List<string> cc, string estatus)
        {
            return this.ReportesRHDAO.GetConsultaCC(cc, estatus);
        }

        public repPrestamosDTO GetInfoPrestamos(int clave_empleado)
        {
            return this.ReportesRHDAO.GetInfoPrestamos(clave_empleado);
        }

        public Dictionary<string, object> GetConsultaPrestamos(int clave_empleado)
        {
            return this.ReportesRHDAO.GetConsultaPrestamos(clave_empleado);
        }
        public Dictionary<string, object> GetPrestamos(int clave_empleado)
        {
            return this.ReportesRHDAO.GetPrestamos(clave_empleado);
        }

        public Dictionary<string, object> GetPrestamosFiltro(FiltroPrestamosDTO objFiltro)
        {
            return this.ReportesRHDAO.GetPrestamosFiltro(objFiltro);
        }
        public Dictionary<string, object> GetPrestamosFiltroGestion(List<string> cc3, string estatus, string tipoPrestamo)
        {
            return this.ReportesRHDAO.GetPrestamosFiltroGestion(cc3, estatus, tipoPrestamo);
        }

        public Dictionary<string, object> GetSolicitudPrestamos(int clave_empleado)
        {
            return this.ReportesRHDAO.GetSolicitudPrestamos(clave_empleado);
        }
        public Dictionary<string, object> GetSolicitudFonacot(int clave_empleado)
        {
            return this.ReportesRHDAO.GetSolicitudFonacot(clave_empleado);
        }

        public Dictionary<string, object> GuardarEditarPrestamos(tblRH_EK_Prestamos data)
        {
            return this.ReportesRHDAO.GuardarEditarPrestamos(data);
        }
        public Dictionary<string, object> GetConfiguracionPrestamos()
        {
            return this.ReportesRHDAO.GetConfiguracionPrestamos();
        }

        public Dictionary<string, object> GuardarConfiguracionPrestamo(tblRH_Prestamos_ConfiguracionPrestamo data)
        {
            return this.ReportesRHDAO.GuardarConfiguracionPrestamo(data);
        }

        public Dictionary<string, object> EliminarPrestamo(int prestamo_id)
        {
            return this.ReportesRHDAO.EliminarPrestamo(prestamo_id);
        }
        public Dictionary<string, object> EliminarConfiguracion(int id)
        {
            return this.ReportesRHDAO.EliminarConfiguracion(id);
        }

        //public Dictionary<string, object> AutorizarRechazarPrestamo(string estatus)
        //{
        //    return this.ReportesRHDAO.AutorizarRechazarPrestamo(estatus);
        //}
        public Dictionary<string, object> ActivarDesactivarPeriodo(int id, string estatus)
        {
            return this.ReportesRHDAO.ActivarDesactivarPeriodo(id, estatus);
        }
        public Dictionary<string, object> GetFechasPeriodos()
        {
            return this.ReportesRHDAO.GetFechasPeriodos();
        }




        public repLaboralDTO GetInfoLaboral(int clave_empleado)
        {
            return this.ReportesRHDAO.GetInfoLaboral(clave_empleado);
        }
        public repLiberacionDTO GetInfoLiberacion(int clave_empleado)
        {
            return this.ReportesRHDAO.GetInfoLiberacion(clave_empleado);
        }
        public repFonacotDTO GetInfoFonacot(int clave_empleado)
        {
            return this.ReportesRHDAO.GetInfoFonacot(clave_empleado);
        }
        public repPagareDTO GetInfoPagare(int clave_empleado)
        {
            return this.ReportesRHDAO.GetInfoPagare(clave_empleado);
        }
        public repGuarderiaDTO GetInfoGuarderia(int clave_empleado)
        {
            return this.ReportesRHDAO.GetInfoGuarderia(clave_empleado);
        }
        public repLactanciaDTO GetInfoLactancia(int clave_empleado)
        {
            return this.ReportesRHDAO.GetInfoLactancia(clave_empleado);
        }
        public Dictionary<string, object> GetSolicitudLactancia(int clave_empleado)
        {
            return this.ReportesRHDAO.GetSolicitudLactancia(clave_empleado);
        }
        public Dictionary<string, object> GetSolicitudGuarderia(int clave_empleado)
        {
            return this.ReportesRHDAO.GetSolicitudGuarderia(clave_empleado);
        }
        public Dictionary<string, object> GetHijos(int clave_empleado)
        {
            return this.ReportesRHDAO.GetHijos(clave_empleado);
        }
        public Dictionary<string, object> GetSolicitudLaboral(int clave_empleado)
        {
            return this.ReportesRHDAO.GetSolicitudLaboral(clave_empleado);
        }
        public Dictionary<string, object> GetResponsableCC(int clave_empleado)
        {
            return this.ReportesRHDAO.GetResponsableCC(clave_empleado);
        }
        public Dictionary<string, object> GetDirectorGeneral(int clave_empleado)
        {
            return this.ReportesRHDAO.GetDirectorGeneral(clave_empleado);
        }

        public Dictionary<string, object> FillComboAutorizantesPrestamos(int clave_empleado, string tipoPrestamo)
        {
            return this.ReportesRHDAO.FillComboAutorizantesPrestamos(clave_empleado, tipoPrestamo);
        }
        public Dictionary<string, object> FillCboCapitalHumano()
        {
            return this.ReportesRHDAO.FillCboCapitalHumano();
        }
        #endregion

        #region EXPEDICIONES
        public Dictionary<string, object> GetExpediciones(string cc, int? tipoReporte, int? claveEmpleado, string nombreEmpleado)
        {
            return this.ReportesRHDAO.GetExpediciones(cc, tipoReporte, claveEmpleado, nombreEmpleado);
        }
        public Dictionary<string, object> GuardarExpediciones(tblRH_REC_Expediciones objExpedicion)
        {
            return this.ReportesRHDAO.GuardarExpediciones(objExpedicion);
        }
        public Dictionary<string, object> GuardarArchivoExpedicion(int idExp, Byte[] archivo)
        {
            return this.ReportesRHDAO.GuardarArchivoExpedicion(idExp, archivo);
        }

        public byte[] GetFirmaFormatos()
        {
            return this.ReportesRHDAO.GetFirmaFormatos();
        }

        public tblRH_REC_Expediciones_Archivos GetArchivoExpedicion(int idExpArchivo)
        {
            return this.ReportesRHDAO.GetArchivoExpedicion(idExpArchivo);
        }
        #endregion

        #region MODULO DE PRESTAMOS
        #region CAPTURA DE PRESTAMOS

        #endregion

        #region CONSULTA DE PRESTAMOS
        public Dictionary<string, object> NotificarPrestamo(int FK_Prestamo)
        {
            return this.ReportesRHDAO.NotificarPrestamo(FK_Prestamo);
        }
        #endregion

        #region CARGA DE ARCHIVOS
        public Dictionary<string, object> GuardarArchivoAdjunto(List<HttpPostedFileBase> lstArchivos, int FK_Prestamo)
        {
            return this.ReportesRHDAO.GuardarArchivoAdjunto(lstArchivos, FK_Prestamo);
        }
        public Dictionary<string, object> GuardarArchivoAdjuntoEnCaptura(List<HttpPostedFileBase> lstArchivos)
        {
            return this.ReportesRHDAO.GuardarArchivoAdjuntoEnCaptura(lstArchivos);
        }
        
        public Dictionary<string, object> GetArchivosAdjuntos(int FK_Prestamo)
        {
            return this.ReportesRHDAO.GetArchivosAdjuntos(FK_Prestamo);
        }

        public Dictionary<string, object> VisualizarArchivoAdjunto(int idArchivo)
        {
            return this.ReportesRHDAO.VisualizarArchivoAdjunto(idArchivo);
        }

        public Dictionary<string, object> EliminarArchivoAdjunto(int idArchivo)
        {
            return this.ReportesRHDAO.EliminarArchivoAdjunto(idArchivo);
        }
        #endregion

        #region GESTIÓN DE PRESTAMOS (AUTORIZACIONES)
        //public Dictionary<string, object> AutorizarRechazarPrestamo(AutorizarRechazarPrestamoDTO objFiltroDTO)
        //{
        //    return this.ReportesRHDAO.AutorizarRechazarPrestamo(objFiltroDTO);
        //}

        public Dictionary<string, object> GetListadoAutorizantes(int idPrestamo)
        {
            return this.ReportesRHDAO.GetListadoAutorizantes(idPrestamo);
        }

        public Dictionary<string, object> AutorizarRechazarPrestamo(repPrestamosDTO objFiltroDTO)
        {
            return this.ReportesRHDAO.AutorizarRechazarPrestamo(objFiltroDTO);
        }
        #endregion

        #region DASHBOARD PRESTAMOS
        public Dictionary<string, object> GetDashboardPrestamos(FiltroPrestamosDTO objFiltroDTO)
        {
            return this.ReportesRHDAO.GetDashboardPrestamos(objFiltroDTO);
        }
        #endregion
        #endregion

        public List<RepActivosDTO> GetEmpleadosActivos_CC(string cc)
        {
            return this.ReportesRHDAO.GetEmpleadosActivos_CC(cc);
        }
    }
}
