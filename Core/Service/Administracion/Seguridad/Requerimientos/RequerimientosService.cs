using Core.DAO.Administracion.Seguridad.Requerimientos;
using Core.DTO.Administracion.Seguridad;
using Core.DTO.Administracion.Seguridad.Requerimientos;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Seguridad.Requerimientos;
using Core.Enum.Administracion.Seguridad.Requerimientos;
using Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.Administracion.Seguridad.Requerimientos
{
    public class RequerimientosService : IRequerimientosDAO
    {
        public IRequerimientosDAO requerimientosDAO { get; set; }

        public RequerimientosService(IRequerimientosDAO requerimientosDAO)
        {
            this.requerimientosDAO = requerimientosDAO;
        }

        #region Catálogos
        public Dictionary<string, object> getRequerimientos()
        {
            return requerimientosDAO.getRequerimientos();
        }

        public Dictionary<string, object> getPuntos()
        {
            return requerimientosDAO.getPuntos();
        }

        public Dictionary<string, object> getPuntosRequerimiento(int requerimientoID)
        {
            return requerimientosDAO.getPuntosRequerimiento(requerimientoID);
        }

        public Dictionary<string, object> guardarNuevoRequerimiento(tblS_Req_Requerimiento requerimiento, List<PuntoDTO> puntos)
        {
            return requerimientosDAO.guardarNuevoRequerimiento(requerimiento, puntos);
        }

        public Dictionary<string, object> editarRequerimiento(tblS_Req_Requerimiento requerimiento, List<PuntoDTO> puntos)
        {
            return requerimientosDAO.editarRequerimiento(requerimiento, puntos);
        }

        public Dictionary<string, object> eliminarRequerimiento(tblS_Req_Requerimiento requerimiento)
        {
            return requerimientosDAO.eliminarRequerimiento(requerimiento);
        }

        public Dictionary<string, object> getActividades()
        {
            return requerimientosDAO.getActividades();
        }

        public Dictionary<string, object> guardarNuevaActividad(tblS_Req_Actividad actividad)
        {
            return requerimientosDAO.guardarNuevaActividad(actividad);
        }

        public Dictionary<string, object> editarActividad(tblS_Req_Actividad actividad)
        {
            return requerimientosDAO.editarActividad(actividad);
        }

        public Dictionary<string, object> eliminarActividad(tblS_Req_Actividad actividad)
        {
            return requerimientosDAO.eliminarActividad(actividad);
        }

        public Dictionary<string, object> getCondicionantes()
        {
            return requerimientosDAO.getCondicionantes();
        }

        public Dictionary<string, object> guardarNuevaCondicionante(tblS_Req_Condicionante condicionante)
        {
            return requerimientosDAO.guardarNuevaCondicionante(condicionante);
        }

        public Dictionary<string, object> editarCondicionante(tblS_Req_Condicionante condicionante)
        {
            return requerimientosDAO.editarCondicionante(condicionante);
        }

        public Dictionary<string, object> eliminarCondicionante(tblS_Req_Condicionante condicionante)
        {
            return requerimientosDAO.eliminarCondicionante(condicionante);
        }

        public Dictionary<string, object> getSecciones()
        {
            return requerimientosDAO.getSecciones();
        }

        public Dictionary<string, object> guardarNuevaSeccion(tblS_Req_Seccion seccion)
        {
            return requerimientosDAO.guardarNuevaSeccion(seccion);
        }

        public Dictionary<string, object> editarSeccion(tblS_Req_Seccion seccion)
        {
            return requerimientosDAO.editarSeccion(seccion);
        }

        public Dictionary<string, object> eliminarSeccion(tblS_Req_Seccion seccion)
        {
            return requerimientosDAO.eliminarSeccion(seccion);
        }

        public Dictionary<string, object> getRelacionCentroCostoDivision()
        {
            return requerimientosDAO.getRelacionCentroCostoDivision();
        }

        public Dictionary<string, object> getDivisiones()
        {
            return requerimientosDAO.getDivisiones();
        }

        public Dictionary<string, object> GetLineaNegocioCombo(int division)
        {
            return requerimientosDAO.GetLineaNegocioCombo(division);
        }

        public Dictionary<string, object> guardarRelacionCentroCostoDivision(int idEmpresa, int idAgrupacion, int division, int lineaNegocio_id)
        {
            return requerimientosDAO.guardarRelacionCentroCostoDivision(idEmpresa,idAgrupacion, division, lineaNegocio_id);
        }

        public Dictionary<string, object> getRelacionesEmpleadoAreaCC()
        {
            return requerimientosDAO.getRelacionesEmpleadoAreaCC();
        }

        public Dictionary<string, object> guardarNuevaRelacion(tblS_Req_EmpleadoAreaCC relacion, bool esContratista)
        {
            return requerimientosDAO.guardarNuevaRelacion(relacion, esContratista);
        }

        public Dictionary<string, object> editarRelacion(tblS_Req_EmpleadoAreaCC relacion, bool esContratista)
        {
            return requerimientosDAO.editarRelacion(relacion, esContratista);
        }

        public Dictionary<string, object> eliminarRelacion(tblS_Req_EmpleadoAreaCC relacion)
        {
            return requerimientosDAO.eliminarRelacion(relacion);
        }
        #endregion

        public Dictionary<string, object> getAsignacion(int idEmpresa, int idAgrupacion)
        {
            return requerimientosDAO.getAsignacion(idEmpresa, idAgrupacion);
        }

        public Dictionary<string, object> guardarAsignacion(AsignacionDTO asignacion)
        {
            return requerimientosDAO.guardarAsignacion(asignacion);
        }

        public Dictionary<string, object> eliminarAsignacionPunto(int asignacionID)
        {
            return requerimientosDAO.eliminarAsignacionPunto(asignacionID);
        }

        public Dictionary<string, object> getAsignacionCaptura(FiltrosAsignacionCapturaDTO filtros)
        {
            return requerimientosDAO.getAsignacionCaptura(filtros);
        }

        public Dictionary<string, object> getEvidencias(string cc, int requerimientoID)
        {
            return requerimientosDAO.getEvidencias(cc, requerimientoID);
        }

        public Dictionary<string, object> guardarEvidencia(List<EvidenciaDTO> captura, List<HttpPostedFileBase> evidencias)
        {
            return requerimientosDAO.guardarEvidencia(captura, evidencias);
        }

        public Dictionary<string, object> cargarDatosArchivoEvidencia(int evidenciaID)
        {
            return requerimientosDAO.cargarDatosArchivoEvidencia(evidenciaID);
        }

        public Tuple<Stream, string> descargarArchivoEvidencia(int evidenciaID)
        {
            return requerimientosDAO.descargarArchivoEvidencia(evidenciaID);
        }

        public Dictionary<string, object> getEvidenciasEvaluacion(int clasificacion, int idEmpresa, int idAgrupacion, int requerimientoID, DateTime fechaInicio, DateTime fechaFin, int estatus)
        {
            return requerimientosDAO.getEvidenciasEvaluacion(clasificacion, idEmpresa, idAgrupacion, requerimientoID, fechaInicio, fechaFin, estatus);
        }

        public Dictionary<string, object> guardarEvaluacion(List<EvidenciaDTO> evaluacion)
        {
            return requerimientosDAO.guardarEvaluacion(evaluacion);
        }

        public bool cargarExcelRequerimientosMasivo(HttpPostedFileBase archivo)
        {
            return requerimientosDAO.cargarExcelRequerimientosMasivo(archivo);
        }

        public Dictionary<string, object> cargarDashboard(List<int> listaDivisiones, List<int> listaLineasNegocio, List<MultiSegDTO> arrGrupos, List<int> listaRequerimientos, DateTime fechaInicio, DateTime fechaFin)
        {
            return requerimientosDAO.cargarDashboard(listaDivisiones, listaLineasNegocio, arrGrupos, listaRequerimientos, fechaInicio, fechaFin);
        }

        public Dictionary<string, object> cargarDashboardClasificacion(List<int> listaDivisiones, List<int> listaLineasNegocio, List<MultiSegDTO> arrGrupos, List<ClasificacionEnum> listaClasificaciones, DateTime fechaInicio, DateTime fechaFin)
        {
            return requerimientosDAO.cargarDashboardClasificacion(listaDivisiones, listaLineasNegocio, arrGrupos, listaClasificaciones, fechaInicio, fechaFin);
        }

        public Dictionary<string, object> getAsignacionCapturaAuditoria(int idEmpresa, int idAgrupacion, ClasificacionEnum clasificacion)
        {
            return requerimientosDAO.getAsignacionCapturaAuditoria(idEmpresa, idAgrupacion, clasificacion);
        }

        public Dictionary<string, object> guardarEvidenciaAuditoria(List<EvidenciaDTO> captura, List<HttpPostedFileBase> evidencias)
        {
            return requerimientosDAO.guardarEvidenciaAuditoria(captura, evidencias);
        }

        public Dictionary<string, object> guardarEvidenciaCargaMasiva(HttpPostedFileBase evidencias, DateTime fechaPuntos)
        {
            return requerimientosDAO.guardarEvidenciaCargaMasiva(evidencias, fechaPuntos);
        }

        public List<ComboDTO> getRequerimientosAsignacionCombo(List<int> clasificaciones)
        {
            return requerimientosDAO.getRequerimientosAsignacionCombo(clasificaciones);
        }

        public List<ComboDTO> getActividadesAsignacionCombo(List<int> requerimientos)
        {
            return requerimientosDAO.getActividadesAsignacionCombo(requerimientos);
        }

        public List<ComboDTO> getCondicionantesAsignacionCombo(List<int> requerimientos, List<int> actividades)
        {
            return requerimientosDAO.getCondicionantesAsignacionCombo(requerimientos, actividades);
        }

        public List<ComboDTO> getSeccionesAsignacionCombo(List<int> requerimientos, List<int> actividades, List<int> condicionantes)
        {
            return requerimientosDAO.getSeccionesAsignacionCombo(requerimientos, actividades, condicionantes);
        }

        public List<ComboDTO> FillComboDivision()
        {
            return requerimientosDAO.FillComboDivision();
        }

        public List<ComboDTO> FillComboRequerimientosDashboard(int division, List<string> listaCC)
        {
            return requerimientosDAO.FillComboRequerimientosDashboard(division, listaCC);
        }

        public Dictionary<string, object> getResponsables()
        {
            return requerimientosDAO.getResponsables();
        }

        public List<ComboDTO> FillComboCcPorDivision(int division)
        {
            return requerimientosDAO.FillComboCcPorDivision(division);
        }

        public Dictionary<string, object> GetAreaCombo()
        {
            return requerimientosDAO.GetAreaCombo();
        }
    }
}
