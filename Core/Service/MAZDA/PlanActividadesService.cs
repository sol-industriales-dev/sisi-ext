using Core.DAO.MAZDA;
using Core.DTO.MAZDA;
using Core.Entity.MAZDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Principal.Generales;
using System.IO;

namespace Core.Service.MAZDA
{
    public class PlanActividadesService : IPlanActividadesDAO
    {
        private IPlanActividadesDAO m_PlanActividadesDAO;
        public IPlanActividadesDAO PlanActividadesDAO
        {
            get { return m_PlanActividadesDAO; }
            set { m_PlanActividadesDAO = value; }
        }
        public PlanActividadesService(IPlanActividadesDAO PlanActividadesDAO)
        {
            this.PlanActividadesDAO = PlanActividadesDAO;
        }

        #region CUADRILLAS
        public List<CuadrillaDTO> getCuadrillas(int cuadrillaID, string personal)
        {
            return PlanActividadesDAO.getCuadrillas(cuadrillaID, personal);
        }
        public void GuardarCuadrilla(string desc, List<UsuarioMAZDADTO> personal)
        {
            PlanActividadesDAO.GuardarCuadrilla(desc, personal);
        }
        public CuadrillaDTO getCuadrilla(int id)
        {
            return PlanActividadesDAO.getCuadrilla(id);
        }
        public void EditarCuadrilla(int id, string desc, List<UsuarioMAZDADTO> personal)
        {
            PlanActividadesDAO.EditarCuadrilla(id, desc, personal);
        }
        public void RemoveCuadrilla(int id)
        {
            PlanActividadesDAO.RemoveCuadrilla(id);
        }



        #endregion

        #region ACTIVIDADES
        public List<ActividadDTO> getActividades(int cuadrillaID, int periodo, string area, string actividad)
        {
            return PlanActividadesDAO.getActividades(cuadrillaID, periodo, area, actividad);
        }
        public void GuardarActividad(string desc, string descripcion, int cuadrillaID, string area, int periodo)
        {
            PlanActividadesDAO.GuardarActividad(desc, descripcion, cuadrillaID, area, periodo);
        }
        public ActividadDTO getActividad(int id)
        {
            return PlanActividadesDAO.getActividad(id);
        }
        public void RemoveActividad(int id)
        {
            PlanActividadesDAO.RemoveActividad(id);
        }
        public void EditarActividad(int id, string desc, string descripcion, int cuadrillaID, string area, int periodo)
        {
            PlanActividadesDAO.EditarActividad(id, desc, descripcion, cuadrillaID, area, periodo);
        }
        public List<ActividadDTO> getActividadesAC()
        {
            return PlanActividadesDAO.getActividadesAC();
        }

        #endregion

        #region AREAS
        public List<AreaDTO> getAreas(int cuadrillaID, string area)
        {
            return PlanActividadesDAO.getAreas(cuadrillaID, area);
        }
        public bool GuardarArea(tblMAZ_Area area, List<tblMAZ_Area_Referencia> referencias)
        {
            return PlanActividadesDAO.GuardarArea(area, referencias);
        }
        public AreaDTO getArea(int id)
        {
            return PlanActividadesDAO.getArea(id);
        }
        public void RemoveArea(int id)
        {
            PlanActividadesDAO.RemoveArea(id);
        }
        public void EditarArea(int id, string desc, int cuadrillaID)
        {
            PlanActividadesDAO.EditarArea(id, desc, cuadrillaID);
        }
        public int GetUltimoArchivoArea()
        {
            return PlanActividadesDAO.GetUltimoArchivoArea();
        }
        public void QuitarReferenciaArea(int areaID)
        {
            PlanActividadesDAO.QuitarReferenciaArea(areaID);
        }
        public bool GuardarReferenciaArea(int areaID, List<tblMAZ_Area_Referencia> referencias)
        {
            return PlanActividadesDAO.GuardarReferenciaArea(areaID, referencias);
        }
        #endregion

        #region REVISIONES
        public bool GuardarRevision(tblMAZ_Revision_AC rev, List<tblMAZ_Revision_AC_Ayudantes> ayu, List<tblMAZ_Revision_AC_Detalle> det, List<tblMAZ_Revision_AC_Evidencia> evi)
        {
            return PlanActividadesDAO.GuardarRevision(rev, ayu, det, evi);
        }
        public bool GuardarRevisionCuadrilla(tblMAZ_Revision_Cuadrilla rev, List<tblMAZ_Revision_Cuadrilla_Ayudantes> ayu, List<tblMAZ_Revision_Cuadrilla_Detalle> det, List<tblMAZ_Revision_Cuadrilla_Evidencia> evi)
        {
            return PlanActividadesDAO.GuardarRevisionCuadrilla(rev, ayu, det, evi);
        }
        public int GetUltimaRevision(int tipo)
        {
            return PlanActividadesDAO.GetUltimaRevision(tipo);
        }
        public RevisionACDTO getRevisionAC(int revisionID)
        {
            return PlanActividadesDAO.getRevisionAC(revisionID);
        }
        public RevisionCuaDTO getRevisionCua(int cuadrillaID, int revisionID)
        {
            return PlanActividadesDAO.getRevisionCua(cuadrillaID, revisionID);
        }
        public int GetUltimoArchivo(int tipo)
        {
            return PlanActividadesDAO.GetUltimoArchivo(tipo);
        }
        #endregion

        #region USUARIOS
        public List<UsuarioMAZDADTO> getUsuarios(string usuario, int cuadrillaID)
        {
            return PlanActividadesDAO.getUsuarios(usuario, cuadrillaID);
        }
        public void GuardarUsuario(string nombre, string apellidoPaterno, string apellidoMaterno, string correo, string usuario, int cuadrillaID)
        {
            PlanActividadesDAO.GuardarUsuario(nombre, apellidoPaterno, apellidoMaterno, correo, usuario, cuadrillaID);
        }
        public void EditarUsuario(int id, string nombre, string apellidoPaterno, string apellidoMaterno, string correo, string usuario, int cuadrillaID)
        {
            PlanActividadesDAO.EditarUsuario(id, nombre, apellidoPaterno, apellidoMaterno, correo, usuario, cuadrillaID);
        }
        public UsuarioMAZDADTO getUsuario(int id)
        {
            return PlanActividadesDAO.getUsuario(id);
        }
        public void RemoveUsuario(int id)
        {
            PlanActividadesDAO.RemoveUsuario(id);
        }
        #endregion

        #region PLAN MESTRO
        public List<CuadrillaDTO> getPlanMaestro()
        {
            return PlanActividadesDAO.getPlanMaestro();
        }
        public List<PlanMaestroDTO> getPlanMaestroOrdenado(List<int> arrCuadrillas, List<int> arrPeriodos, List<string> arrAreas, List<string> arrActividades, List<int> arrMeses)
        {
            return PlanActividadesDAO.getPlanMaestroOrdenado(arrCuadrillas, arrPeriodos, arrAreas, arrActividades, arrMeses);
        }
        public PlanMesDTO getPlanMes(int cuadrillaID, int periodo, int mes)
        {
            return PlanActividadesDAO.getPlanMes(cuadrillaID, periodo, mes);
        }
        public List<PlanMesDTO> getPlanMesEquipo(int cuadrillaID, int periodo, List<int> equipoID)
        {
            return PlanActividadesDAO.getPlanMesEquipo(cuadrillaID, periodo, equipoID);
        }
        public List<PlanMesDTO> getPlanMesGeneral(int mes)
        {
            return PlanActividadesDAO.getPlanMesGeneral(mes);
        }
        public void GuardarPlanMes(PlanMesDTO plan)
        {
            PlanActividadesDAO.GuardarPlanMes(plan);
        }
        public List<ComboDTO> getAllDays(int year)
        {
            return PlanActividadesDAO.getAllDays(year);
        }
        public MemoryStream GenerarPlanExcel(List<EquipoDTO> equipos, List<ComboDTO> dias, List<PlanMesDTO> planMesDetalle, int periodoID, List<ReporteDiarioDTO> revision)
        {
            return PlanActividadesDAO.GenerarPlanExcel(equipos, dias, planMesDetalle, periodoID, revision);
        }

        public List<ReporteDiarioDTO> getRevisionActividadEquipo(List<EquipoDTO> equipos, int cuadrillaID, int areaID)
        {
            return PlanActividadesDAO.getRevisionActividadEquipo(equipos, cuadrillaID, areaID);
        }

        #endregion

        #region EQUIPOS
        public List<EquipoDTO> getEquipos(string descripcion, string subArea, int periodo)
        {
            return PlanActividadesDAO.getEquipos(descripcion, subArea, periodo);
        }
        public List<EquipoDTO> getEquiposCatalogo(List<int> arrCuadrillas, List<int> arrAreas, List<int> arrSubAreas)
        {
            return PlanActividadesDAO.getEquiposCatalogo(arrCuadrillas, arrAreas, arrSubAreas);
        }
        public EquipoDTO getEquipo(int id)
        {
            return PlanActividadesDAO.getEquipo(id);
        }
        public bool GuardarEquipo(tblMAZ_Equipo_AC equi, List<tblMAZ_Equipo_Referencia> referencias)
        {
            return PlanActividadesDAO.GuardarEquipo(equi, referencias);
        }
        public void EditarEquipo(int id, string descripcion, string caracteristicas, string modelo, string tonelaje, int subAreaID, string subArea, int cantidad, bool estatus)
        {
            PlanActividadesDAO.EditarEquipo(id, descripcion, caracteristicas, modelo, tonelaje, subAreaID, subArea, cantidad, estatus);
        }
        public void RemoveEquipo(int id)
        {
            PlanActividadesDAO.RemoveEquipo(id);
        }
        public List<EquipoAreaDTO> getEquiposAreas(int cuadrillaID)
        {
            return PlanActividadesDAO.getEquiposAreas(cuadrillaID);
        }
        public EquipoDTO getEquipoAC(int equipoID)
        {
            return PlanActividadesDAO.getEquipoAC(equipoID);
        }
        public List<tblMAZ_Usuario_Cuadrilla> GetEmpleadoList()
        {
            return PlanActividadesDAO.GetEmpleadoList();
        }
        public int GetUltimoArchivoEquipo()
        {
            return PlanActividadesDAO.GetUltimoArchivoEquipo();
        }
        public void QuitarReferenciaEquipo(int equipoID)
        {
            PlanActividadesDAO.QuitarReferenciaEquipo(equipoID);
        }
        public bool GuardarReferenciaEquipo(int equipoID, List<tblMAZ_Equipo_Referencia> referencias)
        {
            return PlanActividadesDAO.GuardarReferenciaEquipo(equipoID, referencias);
        }
        public List<string> getReferencias(List<int> equiposID)
        {
            return PlanActividadesDAO.getReferencias(equiposID);
        }
        #endregion

        #region EVIDENCIA / REPORTE
        public List<string> getEvidenciasReporte(int revDetID)
        {
            return PlanActividadesDAO.getEvidenciasReporte(revDetID);
        }
        public List<ReporteDiarioDTO> getReporteDiario(string fecha)
        {
            return PlanActividadesDAO.getReporteDiario(fecha);
        }
        public List<string> getEvidenciasAC(int revisionID)
        {
            return PlanActividadesDAO.getEvidenciasAC(revisionID);
        }
        public List<string> getEvidenciasCua(int revisionID)
        {
            return PlanActividadesDAO.getEvidenciasCua(revisionID);
        }
        public bool GuardarInfoRevDet(tblMAZ_Reporte_Actividades info, List<tblMAZ_Reporte_Actividades_Equipo> eq, List<tblMAZ_Reporte_Actividades_Evidencia> evi, bool flagPasarEvidencias)
        {
            return PlanActividadesDAO.GuardarInfoRevDet(info, eq, evi, flagPasarEvidencias);
        }
        public int GetUltimoArchivoReporteAct()
        {
            return PlanActividadesDAO.GetUltimoArchivoReporteAct();
        }
        public int GetUltimoReporteAct()
        {
            return PlanActividadesDAO.GetUltimoReporteAct();
        }
        #endregion

        #region SUBAREA
        public List<subAreaDTO> getSubAreas(int areaID, string subArea)
        {
            return PlanActividadesDAO.getSubAreas(areaID, subArea);
        }
        public List<subAreaDTO> getSubAreasCatalogo(List<int> arrCuadrillas, List<int> arrAreas)
        {
            return PlanActividadesDAO.getSubAreasCatalogo(arrCuadrillas, arrAreas);
        }
        public subAreaDTO getSubArea(int id)
        {
            return PlanActividadesDAO.getSubArea(id);
        }
        public bool GuardarSubArea(tblMAZ_SubArea subArea, List<tblMAZ_Subarea_Referencia> referencias)
        {
            return PlanActividadesDAO.GuardarSubArea(subArea, referencias);
        }
        public void EditarSubArea(int id, string descripcion, int areaID, bool estatus)
        {
            PlanActividadesDAO.EditarSubArea(id, descripcion, areaID, estatus);
        }
        public void RemoveSubArea(int id)
        {
            PlanActividadesDAO.RemoveSubArea(id);
        }
        public int GetUltimoArchivoSubArea()
        {
            return PlanActividadesDAO.GetUltimoArchivoSubArea();
        }
        public void QuitarReferenciaSubarea(int subareaID)
        {
            PlanActividadesDAO.QuitarReferenciaSubarea(subareaID);
        }
        public bool GuardarReferenciaSubArea(int subareaID, List<tblMAZ_Subarea_Referencia> referencias)
        {
            return PlanActividadesDAO.GuardarReferenciaSubArea(subareaID, referencias);
        }
        #endregion

        #region METODOS UTILERIA
        public byte[] ResizeImageToByteArray(byte[] image, int width, int height)
        {
            return PlanActividadesDAO.ResizeImageToByteArray(image, width, height);
        }

        #endregion
    }
}