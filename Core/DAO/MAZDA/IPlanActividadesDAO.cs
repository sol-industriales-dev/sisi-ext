using Core.DTO.MAZDA;
using Core.Entity.MAZDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Principal.Generales;
using System.IO;

namespace Core.DAO.MAZDA
{
    public interface IPlanActividadesDAO
    {
        #region CUADRILLAS
        List<CuadrillaDTO> getCuadrillas(int cuadrillaID, string personal);
        void GuardarCuadrilla(string desc, List<UsuarioMAZDADTO> personal);
        CuadrillaDTO getCuadrilla(int id);
        void EditarCuadrilla(int id, string desc, List<UsuarioMAZDADTO> personal);
        void RemoveCuadrilla(int id);

        #endregion

        #region AREAS
        List<AreaDTO> getAreas(int cuadrillaID, string area);
        bool GuardarArea(tblMAZ_Area area, List<tblMAZ_Area_Referencia> referencias);
        AreaDTO getArea(int id);
        void RemoveArea(int id);
        void EditarArea(int id, string desc, int cuadrillaID);
        int GetUltimoArchivoArea();
        void QuitarReferenciaArea(int areaID);
        bool GuardarReferenciaArea(int areaID, List<tblMAZ_Area_Referencia> referencias);
        #endregion

        #region ACTIVIDADES
        List<ActividadDTO> getActividades(int cuadrillaID, int periodo, string area, string actividad);
        void GuardarActividad(string desc, string descripcion, int cuadrillaID, string area, int periodo);
        ActividadDTO getActividad(int id);
        void RemoveActividad(int id);
        void EditarActividad(int id, string desc, string descripcion, int cuadrillaID, string area, int periodo);
        List<ActividadDTO> getActividadesAC();

        #endregion

        #region USUARIOS
        List<UsuarioMAZDADTO> getUsuarios(string usuario, int cuadrillaID);
        void RemoveUsuario(int id);
        void GuardarUsuario(string nombre, string apellidoPaterno, string apellidoMaterno, string correo, string usuario, int cuadrillaID);
        void EditarUsuario(int id, string nombre, string apellidoPaterno, string apellidoMaterno, string correo, string usuario, int cuadrillaID);
        UsuarioMAZDADTO getUsuario(int id);
        List<tblMAZ_Usuario_Cuadrilla> GetEmpleadoList();
        #endregion

        #region REVISION
        bool GuardarRevision(tblMAZ_Revision_AC rev, List<tblMAZ_Revision_AC_Ayudantes> ayu, List<tblMAZ_Revision_AC_Detalle> det, List<tblMAZ_Revision_AC_Evidencia> evi);
        bool GuardarRevisionCuadrilla(tblMAZ_Revision_Cuadrilla rev, List<tblMAZ_Revision_Cuadrilla_Ayudantes> ayu, List<tblMAZ_Revision_Cuadrilla_Detalle> det, List<tblMAZ_Revision_Cuadrilla_Evidencia> evi);
        int GetUltimaRevision(int tipo);
        #endregion

        #region EQUIPOS
        List<EquipoDTO> getEquipos(string descripcion, string subArea, int periodo);
        List<EquipoDTO> getEquiposCatalogo(List<int> arrCuadrillas, List<int> arrAreas, List<int> arrSubAreas);
        EquipoDTO getEquipo(int id);
        bool GuardarEquipo(tblMAZ_Equipo_AC equi, List<tblMAZ_Equipo_Referencia> referencias);
        void EditarEquipo(int id, string descripcion, string caracteristicas, string modelo, string tonelaje, int subAreaID, string subArea, int cantidad, bool estatus);
        void RemoveEquipo(int id);
        List<EquipoAreaDTO> getEquiposAreas(int cuadrillaID);
        EquipoDTO getEquipoAC(int equipoID);
        int GetUltimoArchivo(int tipo);
        int GetUltimoArchivoEquipo();
        void QuitarReferenciaEquipo(int equipoID);
        bool GuardarReferenciaEquipo(int equipoID, List<tblMAZ_Equipo_Referencia> referencias);
        List<string> getReferencias(List<int> equiposID);
        #endregion

        #region PLAN MAESTRO
        List<CuadrillaDTO> getPlanMaestro();
        List<PlanMaestroDTO> getPlanMaestroOrdenado(List<int> arrCuadrillas, List<int> arrPeriodos, List<string> arrAreas, List<string> arrActividades, List<int> arrMeses);
        PlanMesDTO getPlanMes(int cuadrillaID, int periodo, int mes);
        List<PlanMesDTO> getPlanMesEquipo(int cuadrillaID, int periodo, List<int> equipoID);
        List<PlanMesDTO> getPlanMesGeneral(int mes);
        void GuardarPlanMes(PlanMesDTO plan);
        List<ComboDTO> getAllDays(int year);
        MemoryStream GenerarPlanExcel(List<EquipoDTO> equipos, List<ComboDTO> dias, List<PlanMesDTO> planMesDetalle, int periodoID, List<ReporteDiarioDTO> revision);
        List<ReporteDiarioDTO> getRevisionActividadEquipo(List<EquipoDTO> equipos, int cuadrillaID, int areaID);
        #endregion

        #region REPORTE /EVIDENCIA
        List<ReporteDiarioDTO> getReporteDiario(string fecha);
        List<string> getEvidenciasAC(int revisionID);
        List<string> getEvidenciasCua(int revisionID);
        RevisionACDTO getRevisionAC(int revisionID);
        RevisionCuaDTO getRevisionCua(int cuadrillaID, int revisionID);
        bool GuardarInfoRevDet(tblMAZ_Reporte_Actividades info, List<tblMAZ_Reporte_Actividades_Equipo> eq, List<tblMAZ_Reporte_Actividades_Evidencia> evi, bool flagPasarEvidencias);
        int GetUltimoArchivoReporteAct();
        int GetUltimoReporteAct();
        List<string> getEvidenciasReporte(int revDetID);
        #endregion

        #region SUBAREAS
        List<subAreaDTO> getSubAreas(int areaID, string subArea);
        List<subAreaDTO> getSubAreasCatalogo(List<int> arrCuadrillas, List<int> arrAreas);
        subAreaDTO getSubArea(int id);
        bool GuardarSubArea(tblMAZ_SubArea subArea, List<tblMAZ_Subarea_Referencia> referencias);
        void EditarSubArea(int id, string descripcion, int areaID, bool estatus);
        void RemoveSubArea(int id);
        int GetUltimoArchivoSubArea();
        void QuitarReferenciaSubarea(int subareaID);
        bool GuardarReferenciaSubArea(int subareaID, List<tblMAZ_Subarea_Referencia> referencias);
        #endregion

        #region UTILIDADES
        byte[] ResizeImageToByteArray(byte[] image, int width, int height);
        #endregion

    }
}
