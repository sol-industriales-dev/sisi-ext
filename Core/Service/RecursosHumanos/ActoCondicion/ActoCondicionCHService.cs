using Core.DAO.RecursosHumanos.ActoCondicion;
using Core.DTO.Administracion.Seguridad.Indicadores;
using Core.DTO.RecursosHumanos.ActoCondicion;
using Core.DTO.RecursosHumanos.ActoCondicion.Graficas;
using Core.Enum.RecursosHumanos.ActoCondicion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.RecursosHumanos.ActoCondicion
{
    public class ActoCondicionCHService : IActoCondicionCHDAO
    {
        #region CONSTRUCTOR
        public IActoCondicionCHDAO actoCondicionDAO { get; set; }

        public ActoCondicionCHService(IActoCondicionCHDAO actoCondicionDAO)
        {
            this.actoCondicionDAO = actoCondicionDAO;
        }
        #endregion

        #region ACTOS Y CONDICIONES
        public Dictionary<string, object> CargarActosCondiciones(FiltroActoCondicionDTO filtro)
        {
            return actoCondicionDAO.CargarActosCondiciones(filtro);
        }

        public Dictionary<string, object> ObtenerCentrosCostos()
        {
            return actoCondicionDAO.ObtenerCentrosCostos();
        }

        public Dictionary<string, object> ObtenerSupervisores()
        {
            return actoCondicionDAO.ObtenerSupervisores();

        }

        public Dictionary<string, object> ObtenerDepartamentos()
        {
            return actoCondicionDAO.ObtenerDepartamentos();
        }

        public Dictionary<string, object> FillCboSubclasificacionesDepartamentos(int idDepartamento)
        {
            return actoCondicionDAO.FillCboSubclasificacionesDepartamentos(idDepartamento);
        }

        public Dictionary<string, object> FillCboSubclasificaciones()
        {
            return actoCondicionDAO.FillCboSubclasificaciones();
        }

        public Dictionary<string, object> ObtenerAcciones()
        {
            return actoCondicionDAO.ObtenerAcciones();
        }

        public Dictionary<string, object> ObtenerClasificaciones()
        {
            return actoCondicionDAO.ObtenerClasificaciones();
        }

        public Dictionary<string, object> GuardarActo(ActoDTO acto)
        {
            return actoCondicionDAO.GuardarActo(acto);
        }

        public Dictionary<string, object> GuardarCondicion(CondicionDTO condicion)
        {
            return actoCondicionDAO.GuardarCondicion(condicion);
        }

        public Dictionary<string, object> ObtenerActoCondicion(TipoRiesgoCH tipoRiesgo, int id)
        {
            return actoCondicionDAO.ObtenerActoCondicion(tipoRiesgo, id);
        }

        public Dictionary<string, object> EliminarActoCondicion(TipoRiesgoCH tipoRiesgo, int id)
        {
            return actoCondicionDAO.EliminarActoCondicion(tipoRiesgo, id);
        }

        public Tuple<Stream, string> DescargarArchivo(int sucesoID, TipoRiesgoCH tipoRiesgo, TipoArchivo tipoArchivo)
        {
            return actoCondicionDAO.DescargarArchivo(sucesoID, tipoRiesgo, tipoArchivo);
        }

        public Dictionary<string, object> CargarDatosArchivo(int sucesoID, TipoRiesgoCH tipoRiesgo, TipoArchivo tipoArchivo)
        {
            return actoCondicionDAO.CargarDatosArchivo(sucesoID, tipoRiesgo, tipoArchivo);
        }

        public Dictionary<string, object> ObtenerInformacionInfraccion(int numeroInfraccion, int claveEmpleado, DateTime fechaSuceso)
        {
            return actoCondicionDAO.ObtenerInformacionInfraccion(numeroInfraccion, claveEmpleado, fechaSuceso);
        }

        public Dictionary<string, object> ObtenerInformacionInfraccionContratista(int numeroInfraccion, int claveEmpleado, DateTime fechaSuceso)
        {
            return actoCondicionDAO.ObtenerInformacionInfraccionContratista(numeroInfraccion, claveEmpleado, fechaSuceso);
        }

        public Dictionary<string, object> ObtenerAccionReaccion(int tipo)
        {
            return actoCondicionDAO.ObtenerAccionReaccion(tipo);
        }

        public Dictionary<string, object> ObtenerPrioridades()
        {
            return actoCondicionDAO.ObtenerPrioridades();
        }

        public reporteActoCondicionDTO ObtenerReporteActoCondicion(int id, int tipo)
        {
            return actoCondicionDAO.ObtenerReporteActoCondicion(id, tipo);
        }

        public Dictionary<string, object> GuardarFirma(GuardarFirmaDTO data)
        {
            return actoCondicionDAO.GuardarFirma(data);
        }

        public Dictionary<string, object> VistoAlerta(int id)
        {
            return actoCondicionDAO.VistoAlerta(id);
        }

        //public Dictionary<string, object> DescargarActa(int id)
        //{
        //    return actoCondicionDAO.DescargarActa(id);
        //}

        public Dictionary<string, object> CargarActa(HttpPostedFileBase acta, int id)
        {
            return actoCondicionDAO.CargarActa(acta, id);
        }

        public Dictionary<string, object> CargarComprimido(HttpPostedFileBase archivoComprimido)
        {
            return actoCondicionDAO.CargarComprimido(archivoComprimido);
        }

        public Dictionary<string, object> DescargarFormato()
        {
            return actoCondicionDAO.DescargarFormato();
        }

        public List<CapturaActoCondicion_AutocompleteEmpleadoDTO> GetInfoEmpleado(string term)
        {
            return actoCondicionDAO.GetInfoEmpleado(term);
        }

        public List<CapturaActoCondicion_AutocompleteEmpleadoDTO> GetInfoEmpleadoInternoContratista(string term, bool esContratista, int idEmpresaContratista)
        {
            return actoCondicionDAO.GetInfoEmpleadoInternoContratista(term, esContratista, idEmpresaContratista);
        }

        public Dictionary<string, object> DescargarReporteExcel(FiltroActoCondicionDTO filtro)
        {
            return actoCondicionDAO.DescargarReporteExcel(filtro);
        }

        public Dictionary<string, object> ObtenerClasificacionesGenerales()
        {
            return actoCondicionDAO.ObtenerClasificacionesGenerales();
        }

        public Dictionary<string, object> FillCboProcedimientos(int FK_Clasificacion)
        {
            return actoCondicionDAO.FillCboProcedimientos(FK_Clasificacion);
        }

        public Dictionary<string, object> GuardarArchivoAdjunto(List<HttpPostedFileBase> lstArchivos, int idActo)
        {
            return actoCondicionDAO.GuardarArchivoAdjunto(lstArchivos, idActo);
        }

        public Dictionary<string, object> GetArchivosAdjuntos(int idActo)
        {
            return actoCondicionDAO.GetArchivosAdjuntos(idActo);
        }

        public Dictionary<string, object> VisualizarArchivoAdjunto(int idArchivo)
        {
            return actoCondicionDAO.VisualizarArchivoAdjunto(idArchivo);
        }

        public Dictionary<string, object> EliminarArchivoAdjunto(int idArchivo)
        {
            return actoCondicionDAO.EliminarArchivoAdjunto(idArchivo);
        }

        public Dictionary<string, object> GetSiguienteInfraccion(int claveEmpleado, int procedimientoID)
        {
            return actoCondicionDAO.GetSiguienteInfraccion(claveEmpleado, procedimientoID);
        }

        public Dictionary<string, object> GetPermisos()
        {
            return actoCondicionDAO.GetPermisos();
        }
        #endregion

        #region DASHBOARD
        public Dictionary<string, object> CargarDatosDashboard(FiltroDashboardDTO filtro)
        {
            return actoCondicionDAO.CargarDatosDashboard(filtro);
        }

        public Dictionary<string, object> obtenerGraficaTotalDep(FiltroDashboardDTO filtro)
        {
            return actoCondicionDAO.obtenerGraficaTotalDep(filtro);
        }

        public Dictionary<string, object> GetDashboard(int anio)
        {
            return actoCondicionDAO.GetDashboard(anio);
        }
        #endregion

        #region HISTORIAL

        public Dictionary<string, object> ObtenerHistorialEmpleado(int claveEmpleado)
        {
            return actoCondicionDAO.ObtenerHistorialEmpleado(claveEmpleado);
        }

        public Tuple<Stream, string> DescargarActo(int actoID)
        {
            return actoCondicionDAO.DescargarActo(actoID);
        }

        public Dictionary<string, object> ObtenerMatrices(FiltroDashboardDTO filtro)
        {
            return actoCondicionDAO.ObtenerMatrices(filtro);
        }

        public Tuple<MemoryStream, string> DescargarExcelMatrizActos()
        {
            return actoCondicionDAO.DescargarExcelMatrizActos();
        }

        public Tuple<MemoryStream, string> DescargarExcelMatrizCondiciones()
        {
            return actoCondicionDAO.DescargarExcelMatrizCondiciones();
        }

        public Dictionary<string, object> GetListadoArchivosAdjuntos(int FK_Acto)
        {
            return actoCondicionDAO.GetListadoArchivosAdjuntos(FK_Acto);
        }
        #endregion

        #region MATRIZ
        public Dictionary<string, object> GetMatriz()
        {
            return actoCondicionDAO.GetMatriz();
        }
        #endregion

        #region REPORTES
        public Dictionary<string, object> GenerarReporte(ReporteActoCondicionCH objParametros)
        {
            return actoCondicionDAO.GenerarReporte(objParametros);
        }

        public Dictionary<string, object> DescargarActa(int idActo, string ciudad, string articulos)
        {
            return actoCondicionDAO.DescargarActa(idActo, ciudad, articulos);
        }
        #endregion

        #region GENERALES
        public Dictionary<string, object> GetContenidoActaAdministrativa(int idActo)
        {
            return actoCondicionDAO.GetContenidoActaAdministrativa(idActo);
        }

        public Dictionary<string, object> GetPuestoEmpleado(int idActo)
        {
            return actoCondicionDAO.GetPuestoEmpleado(idActo);
        }

        public Dictionary<string, object> ObtenerComboCCAmbasEmpresas(bool incContratista, int? division)
        {
            return actoCondicionDAO.ObtenerComboCCAmbasEmpresas(incContratista, division);
        }

        public Dictionary<string, object> FillCboCC()
        {
            return actoCondicionDAO.FillCboCC();
        }

        public Dictionary<string, object> FillCboDepartamentos(string cc)
        {
            return actoCondicionDAO.FillCboDepartamentos(cc);
        }

        public Dictionary<string, object> FillCboTipoReportes()
        {
            return actoCondicionDAO.FillCboTipoReportes();
        }

        public Dictionary<string, object> InfEmpleado(int claveEmpleado)
        {
            return actoCondicionDAO.InfEmpleado(claveEmpleado);
        }
        #endregion
    }
}