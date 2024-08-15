using Core.DTO.Administracion.Seguridad.ActoCondicion;
using Core.Enum.Administracion.Seguridad.ActoCondicion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Core.DAO.Administracion.Seguridad.ActoCondicion
{
    public interface IActoCondicionDAO
    {
        #region Actos y Condiciones
        /// <summary>
        /// Obtiene todas las condiciones y actos activos en base a los filtros seleccionados.
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        Dictionary<string, object> CargarActosCondiciones(FiltroActoCondicionDTO filtro);

        /// <summary>
        /// Obtiene un listado con todos los centros de costos activos.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> ObtenerCentrosCostos();

        /// <summary>
        /// Obtiene un listado con todos los distintos supervisores de actos y condiciones.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> ObtenerSupervisores();

        /// <summary>
        /// Obtiene un listado con los distintos departamentos de actos y condiciones.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> ObtenerDepartamentos();

        /// <summary>
        /// Obtiene un listado de las subclasficaciones de departamentos en base al departamento principal seleccionado
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> FillCboSubclasificacionesDepartamentos(int idDepartamento);
        Dictionary<string, object> FillCboSubclasificaciones();


        /// <summary>
        /// Obtiene un listado con las diferentes acciones correctivas existentes.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> ObtenerAcciones();

        /// <summary>
        /// Obtiene dos listados, las clasificaciones de actos y de condiciones.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> ObtenerClasificaciones();

        /// <summary>
        /// Guarda o actualiza un acto.
        /// </summary>
        /// <param name="acto"></param>
        /// <returns></returns>
        Dictionary<string, object> GuardarActo(ActoDTO acto);

        /// <summary>
        /// Guarda o actualiza una condición.
        /// </summary>
        /// <param name="condicion"></param>
        /// <returns></returns>
        Dictionary<string, object> GuardarCondicion(CondicionDTO condicion);

        /// <summary>
        /// Obtiene la información referente a un acto o una condición según sea el tipo de riesgo indicado y su folio.
        /// </summary>
        /// <param name="tipoRiesgo"></param>
        /// <param name="folio"></param>
        /// <param name="cc"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerActoCondicion(TipoRiesgo tipoRiesgo, int id);

        /// <summary>
        /// Marca como inactivo a algún acto o condición.
        /// </summary>
        /// <param name="tipoRiesgo"></param>
        /// <param name="folio"></param>
        /// <param name="cc"></param>
        /// <returns></returns>
        Dictionary<string, object> EliminarActoCondicion(TipoRiesgo tipoRiesgo, int id);

        /// <summary>
        /// Descarga el tipo de archivo indicado.
        /// </summary>
        /// <param name="sucesoID"></param>
        /// <param name="tipoRiesgo"></param>
        /// <param name="tipoArchivo"></param>
        Tuple<Stream, string> DescargarArchivo(int sucesoID, TipoRiesgo tipoRiesgo, TipoArchivo tipoArchivo);

        /// <summary>
        /// Carga un archivo en memoria para ser visualizado posteriormente por el visor de documentos.
        /// </summary>
        /// <param name="sucesoID"></param>
        /// <param name="tipoRiesgo"></param>
        /// <param name="tipoArchivo"></param>
        Dictionary<string, object> CargarDatosArchivo(int sucesoID, TipoRiesgo tipoRiesgo, TipoArchivo tipoArchivo);

        /// <summary>
        /// Obtiene información sobre el tipo de infracción, cuantas faltas tiene el empleado, accion a realizar, etc.
        /// </summary>
        /// <param name="numeroInfraccion"></param>
        /// <param name="claveEmpleado"></param>
        /// <param name="fechaSuceso"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerInformacionInfraccion(int numeroInfraccion, int claveEmpleado, DateTime fechaSuceso);

        Dictionary<string, object> ObtenerInformacionInfraccionContratista(int numeroInfraccion, int claveEmpleado, DateTime fechaSuceso);

        /// <summary>
        /// Obtiene todas las acciones o reacciones para llenar los checkbox en el crud de acto/condicion
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> ObtenerAccionReaccion(int tipo);

        /// <summary>
        /// Obtiene todos las clasificaciones de prioridades
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> ObtenerPrioridades();

        /// <summary>
        /// Obtiene el reporte de acto y condiciones
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        reporteActoCondicionDTO ObtenerReporteActoCondicion(int id, int tipo);

        /// <summary>
        /// Se guarda la firma del acto o condición.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Dictionary<string, object> GuardarFirma(GuardarFirmaDTO data);

        /// <summary>
        /// Cambia el estatus de la alerta a visto.
        /// </summary>
        /// <param name="idActoCondicion"></param>
        /// <returns></returns>
        Dictionary<string, object> VistoAlerta(int id);

        /// <summary>
        /// Genera el acta administrativa y la descarga
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Dictionary<string, object> DescargarActa(int id);

        /// <summary>
        /// Cargar acta administrativa
        /// </summary>
        /// <param name="acta"></param>
        /// <returns></returns>
        Dictionary<string, object> CargarActa(HttpPostedFileBase acta, int id);

        /// <summary>
        /// Cargar archivo zip incluye imagenes y archivo en excel con información sobre actos y condiciones las cuales se registraran en el sistema
        /// </summary>
        /// <param name="archivoComprimido"></param>
        /// <returns></returns>
        Dictionary<string, object> CargarComprimido(HttpPostedFileBase archivoComprimido);

        /// <summary>
        /// Descargar formato de excel, el cual se usa para cargar masiva de actos y condiciones
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> DescargarFormato();

        /// <summary>
        /// Regresa información del empleado para los inputs autocomplete
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        List<CapturaActoCondicion_AutocompleteEmpleadoDTO> GetInfoEmpleado(string term);

        List<CapturaActoCondicion_AutocompleteEmpleadoDTO> GetInfoEmpleadoInternoContratista(string term, bool esContratista, int idEmpresaContratista);

        /// <summary>
        /// Genera un reporte de actos y condiciones en excel y lo descarga
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        Dictionary<string, object> DescargarReporteExcel(FiltroActoCondicionDTO filtro);

        /// <summary>
        /// Obtiene todas las clasificaciones generales y las muestra en un comboBox
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> ObtenerClasificacionesGenerales();
        #endregion

        #region Dashboard

        /// <summary>
        /// Carga la cantidad de actos seguros, actos inseguros y condiciones presentadas por mes en el año actual.
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        Dictionary<string, object> CargarDatosDashboard(FiltroDashboardDTO filtro);

        /// <summary>
        /// Carga el porcentaje de cumplimiento de actos y condiciones.
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        Dictionary<string, object> obtenerGraficaTotalDep(FiltroDashboardDTO filtro);
        #endregion

        #region Historial

        /// <summary>
        /// Carga todos los actos activos hechos por el empleado.
        /// </summary>
        /// <param name="claveEmpleado"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerHistorialEmpleado(int claveEmpleado);

        /// <summary>
        /// Descarga el archivo evidencia de un acto indicado.
        /// </summary>
        /// <param name="actoID"></param>
        /// <returns></returns>
        Tuple<Stream, string> DescargarActo(int actoID);

        /// <summary>
        /// Carga todos los actos y condiciones en base a una serie de filtros establecidos.
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerMatrices(FiltroDashboardDTO filtro);

        /// <summary>
        /// Genera un Excel en base a los actos almacenados en sesión.
        /// </summary>
        /// <returns></returns>
        Tuple<MemoryStream, string> DescargarExcelMatrizActos();

        /// <summary>
        /// Genera un Excel en base a las condiciones almacenadas en sesión.
        /// </summary>
        /// <returns></returns>
        Tuple<MemoryStream, string> DescargarExcelMatrizCondiciones();

        #endregion

        Dictionary<string, object> ObtenerComboCCAmbasEmpresas(bool incContratista, int? division);
    }
}
