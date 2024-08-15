using Core.Entity.SeguimientoCompromisos;
using Core.Enum.SeguimientoCompromisos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.Administracion.Seguridad.SeguimientoCompromisos
{
    public interface ISeguimientoCompromisosDAO
    {
        Dictionary<string, object> GetAgrupacionCombo();
        Dictionary<string, object> GetActividades();
        Dictionary<string, object> GuardarNuevaActividad(tblSC_Actividad actividad);
        Dictionary<string, object> EditarActividad(tblSC_Actividad actividad);
        Dictionary<string, object> EliminarActividad(tblSC_Actividad actividad);
        Dictionary<string, object> GetRelacionesEmpleadoAreaAgrupacion();
        Dictionary<string, object> GuardarNuevaRelacion(tblSC_RelacionEmpleadoAreaAgrupacion relacion);
        Dictionary<string, object> EditarRelacion(tblSC_RelacionEmpleadoAreaAgrupacion relacion);
        Dictionary<string, object> EliminarRelacion(tblSC_RelacionEmpleadoAreaAgrupacion relacion);
        Dictionary<string, object> GetUsuarioPorClave(int claveEmpleado);
        Dictionary<string, object> GetAreaCombo();
        Dictionary<string, object> GetClasificacionCombo(List<int> areas);
        Dictionary<string, object> GetActividadesAplicables(List<int> areas, List<ClasificacionActividadSCEnum> clasificaciones);
        Dictionary<string, object> GetAsignacionActividades(int agrupacion_id);
        Dictionary<string, object> GuardarAsignacionActividades(List<int> agrupaciones, DateTime fechaInicioEvaluacion, List<int> actividades);
        Dictionary<string, object> EliminarAsignacionActividad(int asignacion_id);
        Dictionary<string, object> GetAsignacionActividadesCaptura(int agrupacion_id);
        Dictionary<string, object> GuardarEvidencia(tblSC_Evidencia captura, List<HttpPostedFileBase> evidencias);
        Dictionary<string, object> CargarDatosArchivoEvidencia(int evidencia_id);
        Tuple<Stream, string> DescargarArchivoEvidencia(int evidencia_id);
        Dictionary<string, object> GetActividadesEvaluacion(int agrupacion_id, int estatus);
        Dictionary<string, object> GetEvidenciasActividad(int agrupacion_id, int area, int actividad_id);
        Dictionary<string, object> GuardarEvaluaciones(List<tblSC_Evidencia> evaluaciones);
        Dictionary<string, object> CargarDashboard(List<int> listaAgrupaciones);
    }
}
