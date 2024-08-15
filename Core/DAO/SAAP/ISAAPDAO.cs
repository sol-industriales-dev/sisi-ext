using Core.DTO.Principal.Generales;
using Core.Entity.SAAP;
using Core.Enum.SAAP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.SAAP
{
    public interface ISAAPDAO
    {
        Dictionary<string, object> GetAgrupacionCombo();
        Dictionary<string, object> GetActividades();
        Dictionary<string, object> GuardarNuevaActividad(tblSAAP_Actividad actividad);
        Dictionary<string, object> EditarActividad(tblSAAP_Actividad actividad);
        Dictionary<string, object> EliminarActividad(tblSAAP_Actividad actividad);
        Dictionary<string, object> GetRelacionesEmpleadoAreaAgrupacion();
        Dictionary<string, object> GuardarNuevaRelacion(tblSAAP_RelacionEmpleadoAreaAgrupacion relacion);
        Dictionary<string, object> EditarRelacion(tblSAAP_RelacionEmpleadoAreaAgrupacion relacion);
        Dictionary<string, object> EliminarRelacion(tblSAAP_RelacionEmpleadoAreaAgrupacion relacion);
        Dictionary<string, object> GetUsuarioPorClave(int claveEmpleado);
        Dictionary<string, object> GetAreaCombo();
        Dictionary<string, object> GetClasificacionCombo(List<int> areas);
        Dictionary<string, object> GetActividadesAplicables(List<int> areas, List<ClasificacionActividadEnum> clasificaciones);
        Dictionary<string, object> GetAsignacionActividades(int agrupacion_id);
        Dictionary<string, object> GuardarAsignacionActividades(List<int> agrupaciones, DateTime fechaInicioEvaluacion, List<int> actividades);
        Dictionary<string, object> EliminarAsignacionesActividades(List<int> listaAsignaciones_id);
        Dictionary<string, object> GetAsignacionActividadesCaptura(int agrupacion_id, int area, EstatusEvidenciaEnum estatus);
        Dictionary<string, object> GuardarEvidencia(tblSAAP_Evidencia captura, List<HttpPostedFileBase> evidencias);
        Dictionary<string, object> CargarDatosArchivoEvidencia(int evidencia_id);
        Tuple<Stream, string> DescargarArchivoEvidencia(int evidencia_id);
        Dictionary<string, object> GetActividadesEvaluacion(int agrupacion_id, int estatus, int filtroArea);
        Dictionary<string, object> GetEvidenciasActividad(int agrupacion_id, int area, int actividad_id);
        Dictionary<string, object> GuardarEvaluaciones(List<tblSAAP_Evidencia> evaluaciones);
        Dictionary<string, object> CargarDashboard(List<int> listaAgrupaciones, int filtroArea);
    }
}
