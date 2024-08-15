using Core.DTO;
using Core.DTO.Administracion.Seguridad.Capacitacion.CincoS;
using Core.Entity.Administrativo.Seguridad.Capacitacion.CincoS;
using Core.Enum.Administracion.Seguridad.Capacitacion.CincoS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.Administracion.Seguridad.Capacitacion
{
    public interface ICincoSDAO
    {
        #region CHECKLIST
        Dictionary<string, object> GetCCs(ConsultaCCsEnum consulta, int? checkListId);
        Dictionary<string, object> GetCheckLists(List<string> ccs);
        Dictionary<string, object> GetCheckList(int checkListId);
        Dictionary<string, object> GuardarCheckList(CheckListGuardarDTO checkList);
        Dictionary<string, object> EliminarCheckList(int checkListId);
        Dictionary<string, object> EditarCheckList(CheckListGuardarDTO checkList);
        Dictionary<string, object> GetAreas();
        Dictionary<string, object> GetLideres();
        Dictionary<string, object> GetSubAreas();
        Dictionary<string, object> GetCalendarioCheckList(int checkListId);
        Dictionary<string, object> GuardarCalendarioCheckList(CalendarioCheckListDTO calendario);
        #endregion

        #region CALENDARIO
        Dictionary<string, object> GetCalendarios(List<string> ccsFiltro, int añoFiltro);

        #endregion

        #region AUDITORIAS 5'S
        Dictionary<string, object> GetAuditorias(AuditoriaDTO objParamDTO);

        Dictionary<string, object> CrearEditarAuditoria(AuditoriaDTO objParamDTO, List<HttpPostedFileBase> lstDetecciones, List<HttpPostedFileBase> lstMedidas, List<int> lstIndice_Detecciones, List<int> lstIndice_Medidas);

        Dictionary<string, object> GetInspeccionesRelCheckList(AuditoriaDTO objParamDTO);

        Dictionary<string, object> GetInspeccionesRelCheckListReporte(AuditoriaDTO objParamDTO);

        Dictionary<string, object> FillCboAuditores(AuditoriaDTO objParamDTO);

        Dictionary<string, object> GetDatosActualizarAuditoria(AuditoriaDTO objParamDTO);

        Dictionary<string, object> NotificarAuditoria(int idAuditoria);
        #endregion

        #region SEGUIMIENTOS
        Dictionary<string, object> GetSeguimientos(AuditoriaDTO objParamDTO);

        Dictionary<string, object> RegistrarArchivoSeguimiento(AuditoriaDTO objParamDTO, HttpPostedFileBase objArchivoSeguimiento);

        Dictionary<string, object> AutorizarRechazarArchivoSeguimiento(AuditoriaDetDTO objParamDTO);

        Tuple<Stream, string> DescargarArchivo(AuditoriaDetDTO objParamDTO);

        Dictionary<string, object> VisualizarArchivo(AuditoriaDetDTO objParamDTO);

        Dictionary<string, object> GuardarComentarioLider(int idAuditDet, string comentario);

        #endregion

        #region GENERALES
        Dictionary<string, object> FillCboCheckList();

        Dictionary<string, object> FillCboProyectos(AuditoriaDTO objParamDTO);
        #endregion

        #region FACULTAMIENTOS
        List<AutoCompleteDTO> GetUsuario(string term);
        Dictionary<string, object> GetInfoUsuario(int idUsuario);
        Dictionary<string, object> GetAuditores(List<string> ccs);
        Dictionary<string, object> GetFacultamientos(List<string> ccs, int? privilegioId);
        Dictionary<string, object> GetAuditor(int idAuditor);
        Dictionary<string, object> EliminarAuditor(int idAuditor);
        Dictionary<string, object> EditarAuditor(AuditorInfoDTO info);
        Dictionary<string, object> GuardarAuditor(GuardarUsuarioDTO usuario);
        Dictionary<string, object> GetAuditorPrivilegio(int idAuditor);
        Dictionary<string, object> GuardarAuditorPrivilegio(AuditorPrivilegioDTO privilegio);
        Dictionary<string, object> EliminarAuditorPrivilegio(int idAuditor);
        Dictionary<string, object> GetTablaLideres(List<string> ccs);
        Dictionary<string, object> GetLider(int idLider);
        Dictionary<string, object> GuardarLider(LiderInfoDTO info);
        Dictionary<string, object> EliminarLider(int idLider);
        Dictionary<string, object> GetAreaOperativaLider();
        Dictionary<string, object> GetPrivilegios();
        Dictionary<string, object> GetTablaSubAreas();
        Dictionary<string, object> GetSubArea(int id);
        Dictionary<string, object> EditarSubArea(int id, string nombre);
        Dictionary<string, object> EliminarSubArea(int id);
        Dictionary<string, object> GuardarSubArea(string nombre);
        bool AccesoPermitido(PrivilegioEnum privilegio);
        #endregion

        #region SEGUIMIENTOS
        #region SeguimientoPlanAccion
        Dictionary<string, object> llenarTablaPlanAccion(AuditoriaDTO objParamDTO, DateTime fechaInicio, DateTime fechaFinal);
		#endregion
        #endregion      
	  
        #region REPORTES
        Dictionary<string, object> GetEstadisticasTendencias(List<string> CCs, List<int> areas, DateTime fechaInicio, DateTime fechaFin);
        Dictionary<string, object> GetReporteEjecutivo(List<string> CCs, List<int> areas, DateTime fechaInicio, DateTime fechaFin);
        #endregion
    }
}