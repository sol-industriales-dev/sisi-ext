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

namespace Core.DAO.RecursosHumanos.ReportesRH
{
    public interface IReportesRHDAO
    {
        List<ComboDTO> getListaCC();
        List<ComboDTO> getListaCCRH();
        List<ComboDTO> getListaCCRHBajas();
        List<ComboDTO> getListaConceptosBaja();
        List<ComboDTO> getListaEmpleadosByCC(List<string> cc);
        List<RepAltasDTO> getListaAltas(List<string> cc, DateTime fechaInicio, DateTime fechaFin);
        List<RepActivosDTO> getListaActivos(List<string> cc);
        Dictionary<string, object> GetRptActivos(List<string> cc);
        List<RepBajasDTO> getLayoutBajas(List<string> cc, List<string> concepto, DateTime fechaInicio, DateTime fechaFin, bool tipo, List<int> estatus, DateTime? fechaContaInicio, DateTime? fechaContaFin);

        List<RepBajasDTO> getListaBajas(List<string> cc, List<string> concepto, DateTime fechaInicio, DateTime fechaFin, bool tipo, List<int> estatus, DateTime? fechaContaInicio, DateTime? fechaContaFin, int? tipoBajas);
        List<RepCambiosDTO> getListaCambios(List<string> cc, List<string> concepto, List<string> empleado, DateTime fechaInicio, DateTime fechaFin);
        List<RepModificacionesDTO> getListaModificaciones(List<string> cc, List<string> concepto, DateTime fechaInicio, DateTime fechaFin);
        List<LayoutAltasRHDTO> getListaEmpleados(List<string> cc, DateTime fechaInicio, DateTime fechaFin);
        List<LayoutIncidenciasRHDTO> getListaEmpleadosIncidencias(List<string> cc, DateTime fechaInicio, DateTime fechaFin);

        bool setUsuariosBaja(tblRH_LayautBajaEmpleados obj);

        #region REPORTE GENERAL
        Dictionary<string, object> GetRptGeneral(List<string> cc);

        #endregion

        #region FILL COMBO
        List<ComboDTO> CatIncidencia();
        #endregion

        #region RPT STAFFING GUIDE
        Dictionary<string, object> GetPuestosCategoriasRelPuesto(string _cc, string _strPuesto);
        MemoryStream crearReporte(string cc);

        #endregion

        #region DASHBOARD
        Dictionary<string, object> GetDashboard(List<string> ccs, DateTime fechaInicio, DateTime fechaFin);

        #endregion

        #region Constancias

        Dictionary<string, object> GetEmpleadosPrestamos(List<string> cc);
        Dictionary<string, object> GetConsultaCC(List<string> cc, string estatus);
        repPrestamosDTO GetInfoPrestamos(int clave_empleado);
        Dictionary<string, object> GetPrestamos(int clave_empleado);
        Dictionary<string, object> GetConsultaPrestamos(int clave_empleado);

        Dictionary<string, object> GetPrestamosFiltro(FiltroPrestamosDTO objFiltro);
        Dictionary<string, object> GetPrestamosFiltroGestion(List<string> cc3, string estatus, string tipoPrestamo);

        Dictionary<string, object> GetSolicitudLactancia(int clave_empleado);
        Dictionary<string, object> GetSolicitudPrestamos(int clave_empleado);
        Dictionary<string, object> GetSolicitudFonacot(int clave_empleado);
        Dictionary<string, object> GetSolicitudGuarderia(int clave_empleado);
        Dictionary<string, object> GetHijos(int clave_empleado);

        Dictionary<string, object> GetSolicitudLaboral(int clave_empleado);


        Dictionary<string, object> GuardarEditarPrestamos(tblRH_EK_Prestamos data);
        Dictionary<string, object> GetConfiguracionPrestamos();
        Dictionary<string, object> GuardarConfiguracionPrestamo(tblRH_Prestamos_ConfiguracionPrestamo data);
        Dictionary<string, object> EliminarPrestamo(int prestamo_id);
        Dictionary<string, object> EliminarConfiguracion(int id);

        //Dictionary<string, object> AutorizarRechazarPrestamo(string estatus);
        Dictionary<string, object> ActivarDesactivarPeriodo(int id, string estatus);
        Dictionary<string, object> GetFechasPeriodos();





        repLaboralDTO GetInfoLaboral(int clave_empleado);
        repLiberacionDTO GetInfoLiberacion(int clave_empleado);
        repFonacotDTO GetInfoFonacot(int clave_empleado);
        repPagareDTO GetInfoPagare(int clave_empleado);
        repGuarderiaDTO GetInfoGuarderia(int clave_empleado);
        repLactanciaDTO GetInfoLactancia(int clave_empleado);
        Dictionary<string, object> GetResponsableCC(int clave_empleado);
        Dictionary<string, object> GetDirectorGeneral(int clave_empleado);
        Dictionary<string, object> FillComboAutorizantesPrestamos(int clave_empleado, string tipoPrestamo);
        Dictionary<string, object> FillCboCapitalHumano();
        #endregion

        #region EXPEDICIONES
        Dictionary<string, object> GetExpediciones(string cc, int? tipoReporte, int? claveEmpleado, string nombreEmpleado);
        Dictionary<string, object> GuardarExpediciones(tblRH_REC_Expediciones objExpedicion);
        Dictionary<string, object> GuardarArchivoExpedicion(int idExp, Byte[] archivo);
        byte[] GetFirmaFormatos();
        tblRH_REC_Expediciones_Archivos GetArchivoExpedicion(int idExpArchivo);

        #endregion

        #region MODULO DE PRESTAMOS
        #region CAPTURA DE PRESTAMOS

        #endregion  

        #region CONSULTA DE PRESTAMOS
        Dictionary<string, object> NotificarPrestamo(int FK_Prestamo);
        #endregion

        #region CARGA DE ARCHIVOS
        Dictionary<string, object> GuardarArchivoAdjunto(List<HttpPostedFileBase> lstArchivos, int FK_Prestamo);

        Dictionary<string, object> GuardarArchivoAdjuntoEnCaptura(List<HttpPostedFileBase> lstArchivos);

        Dictionary<string, object> GetArchivosAdjuntos(int FK_Prestamo);

        Dictionary<string, object> VisualizarArchivoAdjunto(int idArchivo);

        Dictionary<string, object> EliminarArchivoAdjunto(int idArchivo);
        #endregion

        #region GESTIÓN DE PRESTAMOS (AUTORIZACIONES)
        //Dictionary<string, object> AutorizarRechazarPrestamo(AutorizarRechazarPrestamoDTO objFiltroDTO);

        Dictionary<string, object> GetListadoAutorizantes(int idPrestamo);

        Dictionary<string, object> AutorizarRechazarPrestamo(repPrestamosDTO objFiltroDTO);
        #endregion

        #region DASHBOARD PRESTAMOS
        Dictionary<string, object> GetDashboardPrestamos(FiltroPrestamosDTO objFiltroDTO);
        #endregion
        #endregion

        List<RepActivosDTO> GetEmpleadosActivos_CC(string cc);
    }
}
