using Core.DTO.Administracion.Seguridad;
using Core.DTO.Administracion.Seguridad.Requerimientos;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Seguridad.Requerimientos;
using Core.Enum.Administracion.Seguridad.Requerimientos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.Administracion.Seguridad.Requerimientos
{
    public interface IRequerimientosDAO
    {
        #region Catálogos
        Dictionary<string, object> getRequerimientos();
        Dictionary<string, object> getPuntos();
        Dictionary<string, object> getPuntosRequerimiento(int requerimientoID);
        Dictionary<string, object> guardarNuevoRequerimiento(tblS_Req_Requerimiento requerimiento, List<PuntoDTO> puntos);
        Dictionary<string, object> editarRequerimiento(tblS_Req_Requerimiento requerimiento, List<PuntoDTO> punto);
        Dictionary<string, object> eliminarRequerimiento(tblS_Req_Requerimiento requerimiento);
        Dictionary<string, object> getActividades();
        Dictionary<string, object> guardarNuevaActividad(tblS_Req_Actividad actividad);
        Dictionary<string, object> editarActividad(tblS_Req_Actividad actividad);
        Dictionary<string, object> eliminarActividad(tblS_Req_Actividad actividad);
        Dictionary<string, object> getCondicionantes();
        Dictionary<string, object> guardarNuevaCondicionante(tblS_Req_Condicionante condicionante);
        Dictionary<string, object> editarCondicionante(tblS_Req_Condicionante condicionante);
        Dictionary<string, object> eliminarCondicionante(tblS_Req_Condicionante condicionante);
        Dictionary<string, object> getSecciones();
        Dictionary<string, object> guardarNuevaSeccion(tblS_Req_Seccion seccion);
        Dictionary<string, object> editarSeccion(tblS_Req_Seccion seccion);
        Dictionary<string, object> eliminarSeccion(tblS_Req_Seccion seccion);
        Dictionary<string, object> getRelacionCentroCostoDivision();
        Dictionary<string, object> getDivisiones();
        Dictionary<string, object> GetLineaNegocioCombo(int division);
        Dictionary<string, object> guardarRelacionCentroCostoDivision(int idEmpresa,int idAgrupacion, int division, int lineaNegocio_id);
        Dictionary<string, object> getRelacionesEmpleadoAreaCC();
        Dictionary<string, object> guardarNuevaRelacion(tblS_Req_EmpleadoAreaCC relacion, bool esContratista);
        Dictionary<string, object> editarRelacion(tblS_Req_EmpleadoAreaCC relacion, bool esContratista);
        Dictionary<string, object> eliminarRelacion(tblS_Req_EmpleadoAreaCC relacion);
        #endregion
        Dictionary<string, object> getAsignacion(int idEmpresa, int idAgrupacion);
        Dictionary<string, object> guardarAsignacion(AsignacionDTO asignacion);
        Dictionary<string, object> eliminarAsignacionPunto(int asignacionID);
        Dictionary<string, object> getAsignacionCaptura(FiltrosAsignacionCapturaDTO filtros);
        Dictionary<string, object> getEvidencias(string cc, int requerimientoID);
        Dictionary<string, object> guardarEvidencia(List<EvidenciaDTO> captura, List<HttpPostedFileBase> evidencias);
        Dictionary<string, object> cargarDatosArchivoEvidencia(int evidenciaID);
        Tuple<Stream, string> descargarArchivoEvidencia(int evidenciaID);
        Dictionary<string, object> getEvidenciasEvaluacion(int clasificacion, int idEmpresa, int idAgrupacion, int requerimientoID, DateTime fechaInicio, DateTime fechaFin, int estatus);
        Dictionary<string, object> guardarEvaluacion(List<EvidenciaDTO> evaluacion);
        bool cargarExcelRequerimientosMasivo(HttpPostedFileBase archivo);
        Dictionary<string, object> cargarDashboard(List<int> listaDivisiones, List<int> listaLineasNegocio, List<MultiSegDTO> arrGrupos, List<int> listaRequerimientos, DateTime fechaInicio, DateTime fechaFin);
        Dictionary<string, object> cargarDashboardClasificacion(List<int> listaDivisiones, List<int> listaLineasNegocio, List<MultiSegDTO> arrGrupos, List<ClasificacionEnum> listaClasificaciones, DateTime fechaInicio, DateTime fechaFin);
        Dictionary<string, object> getAsignacionCapturaAuditoria(int idEmpresa, int idAgrupacion, ClasificacionEnum clasificacion);
        Dictionary<string, object> guardarEvidenciaAuditoria(List<EvidenciaDTO> captura, List<HttpPostedFileBase> evidencias);
        Dictionary<string, object> guardarEvidenciaCargaMasiva(HttpPostedFileBase evidencias, DateTime fechaPuntos);
        List<ComboDTO> getRequerimientosAsignacionCombo(List<int> clasificaciones);
        List<ComboDTO> getActividadesAsignacionCombo(List<int> requerimientos);
        List<ComboDTO> getCondicionantesAsignacionCombo(List<int> requerimientos, List<int> actividades);
        List<ComboDTO> getSeccionesAsignacionCombo(List<int> requerimientos, List<int> actividades, List<int> condicionantes);
        List<ComboDTO> FillComboDivision();
        List<ComboDTO> FillComboRequerimientosDashboard(int division, List<string> listaCC);
        Dictionary<string, object> getResponsables();
        List<ComboDTO> FillComboCcPorDivision(int division);
        Dictionary<string, object> GetAreaCombo();
    }
}
