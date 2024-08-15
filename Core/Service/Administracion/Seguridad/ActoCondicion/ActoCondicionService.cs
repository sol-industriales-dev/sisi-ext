using Core.DAO.Administracion.Seguridad.ActoCondicion;
using Core.DTO.Administracion.Seguridad.ActoCondicion;
using Core.Enum.Administracion.Seguridad.ActoCondicion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Core.Service.Administracion.Seguridad.ActoCondicion
{
    public class ActoCondicionService : IActoCondicionDAO
    {
        public IActoCondicionDAO actoCondicionDAO { get; set; }
        
        public ActoCondicionService(IActoCondicionDAO actoCondicionDAO)
        {
            this.actoCondicionDAO = actoCondicionDAO;
        }

        #region Actos y Condiciones

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
        public Dictionary<string, object> ObtenerActoCondicion(TipoRiesgo tipoRiesgo, int id)
        {
            return actoCondicionDAO.ObtenerActoCondicion(tipoRiesgo, id);
        }
        public Dictionary<string, object> EliminarActoCondicion(TipoRiesgo tipoRiesgo, int id)
        {
            return actoCondicionDAO.EliminarActoCondicion(tipoRiesgo, id);
        }

        public Tuple<Stream, string> DescargarArchivo(int sucesoID, TipoRiesgo tipoRiesgo, TipoArchivo tipoArchivo)
        {
            return actoCondicionDAO.DescargarArchivo(sucesoID, tipoRiesgo, tipoArchivo);
        }

        public Dictionary<string, object> CargarDatosArchivo(int sucesoID, TipoRiesgo tipoRiesgo, TipoArchivo tipoArchivo)
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

        public Dictionary<string, object> DescargarActa(int id)
        {
            return actoCondicionDAO.DescargarActa(id);
        }

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
        #endregion

        #region Dashboard
        public Dictionary<string, object> CargarDatosDashboard(FiltroDashboardDTO filtro)
        {
            return actoCondicionDAO.CargarDatosDashboard(filtro);
        }

        public Dictionary<string, object> obtenerGraficaTotalDep(FiltroDashboardDTO filtro)
        {
            return actoCondicionDAO.obtenerGraficaTotalDep(filtro);
        }
        #endregion

        #region Historial

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

        #endregion

        public Dictionary<string, object> ObtenerComboCCAmbasEmpresas(bool incContratista, int? division)
        {
            return actoCondicionDAO.ObtenerComboCCAmbasEmpresas(incContratista, division);
        }
    }
}
