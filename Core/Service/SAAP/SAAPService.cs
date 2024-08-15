using Core.DAO.SAAP;
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

namespace Core.Service.SAAP
{
    public class SAAPService : ISAAPDAO
    {
        public ISAAPDAO saapDAO;
        public ISAAPDAO SAAPDAO
        {
            get { return saapDAO; }
            set { saapDAO = value; }
        }
        public SAAPService(ISAAPDAO saapDAO)
        {
            this.SAAPDAO = saapDAO;
        }

        public Dictionary<string, object> GetAgrupacionCombo()
        {
            return saapDAO.GetAgrupacionCombo();
        }

        public Dictionary<string, object> GetActividades()
        {
            return saapDAO.GetActividades();
        }

        public Dictionary<string, object> GuardarNuevaActividad(tblSAAP_Actividad actividad)
        {
            return saapDAO.GuardarNuevaActividad(actividad);
        }

        public Dictionary<string, object> EditarActividad(tblSAAP_Actividad actividad)
        {
            return saapDAO.EditarActividad(actividad);
        }

        public Dictionary<string, object> EliminarActividad(tblSAAP_Actividad actividad)
        {
            return saapDAO.EliminarActividad(actividad);
        }

        public Dictionary<string, object> GetRelacionesEmpleadoAreaAgrupacion()
        {
            return saapDAO.GetRelacionesEmpleadoAreaAgrupacion();
        }

        public Dictionary<string, object> GuardarNuevaRelacion(tblSAAP_RelacionEmpleadoAreaAgrupacion relacion)
        {
            return saapDAO.GuardarNuevaRelacion(relacion);
        }

        public Dictionary<string, object> EditarRelacion(tblSAAP_RelacionEmpleadoAreaAgrupacion relacion)
        {
            return saapDAO.EditarRelacion(relacion);
        }

        public Dictionary<string, object> EliminarRelacion(tblSAAP_RelacionEmpleadoAreaAgrupacion relacion)
        {
            return saapDAO.EliminarRelacion(relacion);
        }

        public Dictionary<string, object> GetUsuarioPorClave(int claveEmpleado)
        {
            return saapDAO.GetUsuarioPorClave(claveEmpleado);
        }

        public Dictionary<string, object> GetAreaCombo()
        {
            return saapDAO.GetAreaCombo();
        }

        public Dictionary<string, object> GetClasificacionCombo(List<int> areas)
        {
            return saapDAO.GetClasificacionCombo(areas);
        }

        public Dictionary<string, object> GetActividadesAplicables(List<int> areas, List<ClasificacionActividadEnum> clasificaciones)
        {
            return saapDAO.GetActividadesAplicables(areas, clasificaciones);
        }

        public Dictionary<string, object> GetAsignacionActividades(int agrupacion_id)
        {
            return saapDAO.GetAsignacionActividades(agrupacion_id);
        }

        public Dictionary<string, object> GuardarAsignacionActividades(List<int> agrupaciones, DateTime fechaInicioEvaluacion, List<int> actividades)
        {
            return saapDAO.GuardarAsignacionActividades(agrupaciones, fechaInicioEvaluacion, actividades);
        }

        public Dictionary<string, object> EliminarAsignacionesActividades(List<int> listaAsignaciones_id)
        {
            return saapDAO.EliminarAsignacionesActividades(listaAsignaciones_id);
        }

        public Dictionary<string, object> GetAsignacionActividadesCaptura(int agrupacion_id, int area, EstatusEvidenciaEnum estatus)
        {
            return saapDAO.GetAsignacionActividadesCaptura(agrupacion_id, area, estatus);
        }

        public Dictionary<string, object> GuardarEvidencia(tblSAAP_Evidencia captura, List<HttpPostedFileBase> evidencias)
        {
            return saapDAO.GuardarEvidencia(captura, evidencias);
        }

        public Dictionary<string, object> CargarDatosArchivoEvidencia(int evidencia_id)
        {
            return saapDAO.CargarDatosArchivoEvidencia(evidencia_id);
        }

        public Tuple<Stream, string> DescargarArchivoEvidencia(int evidencia_id)
        {
            return saapDAO.DescargarArchivoEvidencia(evidencia_id);
        }

        public Dictionary<string, object> GetActividadesEvaluacion(int agrupacion_id, int estatus, int filtroArea)
        {
            return saapDAO.GetActividadesEvaluacion(agrupacion_id, estatus, filtroArea);
        }

        public Dictionary<string, object> GetEvidenciasActividad(int agrupacion_id, int area, int actividad_id)
        {
            return saapDAO.GetEvidenciasActividad(agrupacion_id, area, actividad_id);
        }

        public Dictionary<string, object> GuardarEvaluaciones(List<tblSAAP_Evidencia> evaluaciones)
        {
            return saapDAO.GuardarEvaluaciones(evaluaciones);
        }

        public Dictionary<string, object> CargarDashboard(List<int> listaAgrupaciones, int filtroArea)
        {
            return saapDAO.CargarDashboard(listaAgrupaciones, filtroArea);
        }
    }
}
