using Core.DAO.Administracion.Seguridad.SeguimientoCompromisos;
using Core.Entity.SeguimientoCompromisos;
using Core.Enum.SeguimientoCompromisos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.Administracion.Seguridad.SeguimientoCompromisos
{
    public class SeguimientoCompromisosService : ISeguimientoCompromisosDAO
    {
        public ISeguimientoCompromisosDAO seguimientoCompromisosDAO;
        public ISeguimientoCompromisosDAO SeguimientoCompromisosDAO
        {
            get { return seguimientoCompromisosDAO; }
            set { seguimientoCompromisosDAO = value; }
        }
        public SeguimientoCompromisosService(ISeguimientoCompromisosDAO seguimientoCompromisosDAO)
        {
            this.SeguimientoCompromisosDAO = seguimientoCompromisosDAO;
        }

        public Dictionary<string, object> GetAgrupacionCombo()
        {
            return seguimientoCompromisosDAO.GetAgrupacionCombo();
        }

        public Dictionary<string, object> GetActividades()
        {
            return seguimientoCompromisosDAO.GetActividades();
        }

        public Dictionary<string, object> GuardarNuevaActividad(tblSC_Actividad actividad)
        {
            return seguimientoCompromisosDAO.GuardarNuevaActividad(actividad);
        }

        public Dictionary<string, object> EditarActividad(tblSC_Actividad actividad)
        {
            return seguimientoCompromisosDAO.EditarActividad(actividad);
        }

        public Dictionary<string, object> EliminarActividad(tblSC_Actividad actividad)
        {
            return seguimientoCompromisosDAO.EliminarActividad(actividad);
        }

        public Dictionary<string, object> GetRelacionesEmpleadoAreaAgrupacion()
        {
            return seguimientoCompromisosDAO.GetRelacionesEmpleadoAreaAgrupacion();
        }

        public Dictionary<string, object> GuardarNuevaRelacion(tblSC_RelacionEmpleadoAreaAgrupacion relacion)
        {
            return seguimientoCompromisosDAO.GuardarNuevaRelacion(relacion);
        }

        public Dictionary<string, object> EditarRelacion(tblSC_RelacionEmpleadoAreaAgrupacion relacion)
        {
            return seguimientoCompromisosDAO.EditarRelacion(relacion);
        }

        public Dictionary<string, object> EliminarRelacion(tblSC_RelacionEmpleadoAreaAgrupacion relacion)
        {
            return seguimientoCompromisosDAO.EliminarRelacion(relacion);
        }

        public Dictionary<string, object> GetUsuarioPorClave(int claveEmpleado)
        {
            return seguimientoCompromisosDAO.GetUsuarioPorClave(claveEmpleado);
        }

        public Dictionary<string, object> GetAreaCombo()
        {
            return seguimientoCompromisosDAO.GetAreaCombo();
        }

        public Dictionary<string, object> GetClasificacionCombo(List<int> areas)
        {
            return seguimientoCompromisosDAO.GetClasificacionCombo(areas);
        }

        public Dictionary<string, object> GetActividadesAplicables(List<int> areas, List<ClasificacionActividadSCEnum> clasificaciones)
        {
            return seguimientoCompromisosDAO.GetActividadesAplicables(areas, clasificaciones);
        }

        public Dictionary<string, object> GetAsignacionActividades(int agrupacion_id)
        {
            return seguimientoCompromisosDAO.GetAsignacionActividades(agrupacion_id);
        }

        public Dictionary<string, object> GuardarAsignacionActividades(List<int> agrupaciones, DateTime fechaInicioEvaluacion, List<int> actividades)
        {
            return seguimientoCompromisosDAO.GuardarAsignacionActividades(agrupaciones, fechaInicioEvaluacion, actividades);
        }

        public Dictionary<string, object> EliminarAsignacionActividad(int asignacion_id)
        {
            return seguimientoCompromisosDAO.EliminarAsignacionActividad(asignacion_id);
        }

        public Dictionary<string, object> GetAsignacionActividadesCaptura(int agrupacion_id)
        {
            return seguimientoCompromisosDAO.GetAsignacionActividadesCaptura(agrupacion_id);
        }

        public Dictionary<string, object> GuardarEvidencia(tblSC_Evidencia captura, List<HttpPostedFileBase> evidencias)
        {
            return seguimientoCompromisosDAO.GuardarEvidencia(captura, evidencias);
        }

        public Dictionary<string, object> CargarDatosArchivoEvidencia(int evidencia_id)
        {
            return seguimientoCompromisosDAO.CargarDatosArchivoEvidencia(evidencia_id);
        }

        public Tuple<Stream, string> DescargarArchivoEvidencia(int evidencia_id)
        {
            return seguimientoCompromisosDAO.DescargarArchivoEvidencia(evidencia_id);
        }

        public Dictionary<string, object> GetActividadesEvaluacion(int agrupacion_id, int estatus)
        {
            return seguimientoCompromisosDAO.GetActividadesEvaluacion(agrupacion_id, estatus);
        }

        public Dictionary<string, object> GetEvidenciasActividad(int agrupacion_id, int area, int actividad_id)
        {
            return seguimientoCompromisosDAO.GetEvidenciasActividad(agrupacion_id, area, actividad_id);
        }

        public Dictionary<string, object> GuardarEvaluaciones(List<tblSC_Evidencia> evaluaciones)
        {
            return seguimientoCompromisosDAO.GuardarEvaluaciones(evaluaciones);
        }

        public Dictionary<string, object> CargarDashboard(List<int> listaAgrupaciones)
        {
            return seguimientoCompromisosDAO.CargarDashboard(listaAgrupaciones);
        }
    }
}
