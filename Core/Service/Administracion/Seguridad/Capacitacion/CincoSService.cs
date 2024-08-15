using Core.DAO.Administracion.Seguridad.Capacitacion;
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

namespace Core.Service.Administracion.Seguridad.Capacitacion
{
    public class CincoSService : ICincoSDAO
    {
        private ICincoSDAO _cincoSDAO;

        private ICincoSDAO CincoSDAO
        {
            get { return _cincoSDAO; }
            set { _cincoSDAO = value; }
        }

        public CincoSService(ICincoSDAO cincoS)
        {
            this.CincoSDAO = cincoS;
        }

        #region CHECKLIST
        public Dictionary<string, object> GetCCs(ConsultaCCsEnum consulta, int? checkListId)
        {
            return this.CincoSDAO.GetCCs(consulta, checkListId);
        }

        public Dictionary<string, object> GetCheckLists(List<string> ccs)
        {
            return this.CincoSDAO.GetCheckLists(ccs);
        }

        public Dictionary<string, object> GetCheckList(int checkListId)
        {
            return this.CincoSDAO.GetCheckList(checkListId);
        }

        public Dictionary<string, object> GuardarCheckList(CheckListGuardarDTO checkList)
        {
            return this.CincoSDAO.GuardarCheckList(checkList);
        }

        public Dictionary<string, object> EliminarCheckList(int checkListId)
        {
            return this.CincoSDAO.EliminarCheckList(checkListId);
        }

        public Dictionary<string, object> EditarCheckList(CheckListGuardarDTO checkList)
        {
            return this.CincoSDAO.EditarCheckList(checkList);
        }

        public Dictionary<string, object> GetAreas()
        {
            return this.CincoSDAO.GetAreas();
        }

        public Dictionary<string, object> GetLideres()
        {
            return this.CincoSDAO.GetLideres();
        }

        public Dictionary<string, object> GetSubAreas()
        {
            return this.CincoSDAO.GetSubAreas();
        }

        public Dictionary<string, object> GetCalendarioCheckList(int checkListId)
        {
            return this.CincoSDAO.GetCalendarioCheckList(checkListId);
        }

        public Dictionary<string, object> GuardarCalendarioCheckList(CalendarioCheckListDTO calendario)
        {
            return this.CincoSDAO.GuardarCalendarioCheckList(calendario);
        }
        #endregion

        #region CALENDARIO
        public Dictionary<string, object> GetCalendarios(List<string> ccsFiltro, int añoFiltro)
        {
            return this.CincoSDAO.GetCalendarios(ccsFiltro, añoFiltro);
        }
        #endregion

        #region AUDITORIAS 5'S
        public Dictionary<string, object> GetAuditorias(AuditoriaDTO objParamDTO)
        {
            return this.CincoSDAO.GetAuditorias(objParamDTO);
        }

        public Dictionary<string, object> CrearEditarAuditoria(AuditoriaDTO objParamDTO, List<HttpPostedFileBase> lstDetecciones, List<HttpPostedFileBase> lstMedidas, List<int> lstIndice_Detecciones, List<int> lstIndice_Medidas)
        {
            return this.CincoSDAO.CrearEditarAuditoria(objParamDTO, lstDetecciones, lstMedidas, lstIndice_Detecciones, lstIndice_Medidas);
        }

        public Dictionary<string, object> GetInspeccionesRelCheckList(AuditoriaDTO objParamDTO)
        {
            return this.CincoSDAO.GetInspeccionesRelCheckList(objParamDTO);
        }

        public Dictionary<string, object> GetInspeccionesRelCheckListReporte(AuditoriaDTO objParamDTO)
        {
            return this.CincoSDAO.GetInspeccionesRelCheckListReporte(objParamDTO);
        }

        public Dictionary<string, object> FillCboAuditores(AuditoriaDTO objParamDTO)
        {
            return this.CincoSDAO.FillCboAuditores(objParamDTO);
        }

        public Dictionary<string, object> GetDatosActualizarAuditoria(AuditoriaDTO objParamDTO)
        {
            return this.CincoSDAO.GetDatosActualizarAuditoria(objParamDTO);
        }

        public Dictionary<string, object> NotificarAuditoria(int idAuditoria)
        {
            return this.CincoSDAO.NotificarAuditoria(idAuditoria);
        }
        #endregion

        #region SEGUIMIENTOS
        public Dictionary<string, object> GetSeguimientos(AuditoriaDTO objParamDTO)
        {
            return this.CincoSDAO.GetSeguimientos(objParamDTO);
        }

        public Dictionary<string, object> RegistrarArchivoSeguimiento(AuditoriaDTO objParamDTO, HttpPostedFileBase objArchivoSeguimiento)
        {
            return this.CincoSDAO.RegistrarArchivoSeguimiento(objParamDTO, objArchivoSeguimiento);
        }

        public Dictionary<string, object> AutorizarRechazarArchivoSeguimiento(AuditoriaDetDTO objParamDTO)
        {
            return this.CincoSDAO.AutorizarRechazarArchivoSeguimiento(objParamDTO);
        }

        public Tuple<Stream, string> DescargarArchivo(AuditoriaDetDTO objParamDTO)
        {
            return this.CincoSDAO.DescargarArchivo(objParamDTO);
        }

        public Dictionary<string, object> VisualizarArchivo(AuditoriaDetDTO objParamDTO)
        {
            return this.CincoSDAO.VisualizarArchivo(objParamDTO);
        }

        public Dictionary<string, object> GuardarComentarioLider(int idAuditDet, string comentario)
        {
            return this.CincoSDAO.GuardarComentarioLider(idAuditDet, comentario);
        }
        #endregion

        #region GENERALES
        public Dictionary<string, object> FillCboCheckList()
        {
            return this.CincoSDAO.FillCboCheckList();
        }

        public Dictionary<string, object> FillCboProyectos(AuditoriaDTO objParamDTO)
        {
            return this.CincoSDAO.FillCboProyectos(objParamDTO);
        }
        #endregion

        #region FACULTAMIENTOS
        public List<AutoCompleteDTO> GetUsuario(string term)
        {
            return this.CincoSDAO.GetUsuario(term);
        }

        public Dictionary<string, object> GetInfoUsuario(int idUsuario)
        {
            return this.CincoSDAO.GetInfoUsuario(idUsuario);
        }

        public Dictionary<string, object> GetAuditores(List<string> ccs)
        {
            return this.CincoSDAO.GetAuditores(ccs);
        }

        public Dictionary<string, object> GetFacultamientos(List<string> ccs, int? privilegioId)
        {
            return this.CincoSDAO.GetFacultamientos(ccs, privilegioId);
        }

        public Dictionary<string, object> GetAuditor(int idAuditor)
        {
            return this.CincoSDAO.GetAuditor(idAuditor);
        }

        public Dictionary<string, object> EliminarAuditor(int idAuditor)
        {
            return this.CincoSDAO.EliminarAuditor(idAuditor);
        }

        public Dictionary<string, object> EditarAuditor(AuditorInfoDTO info)
        {
            return this.CincoSDAO.EditarAuditor(info);
        }

        public Dictionary<string, object> GuardarAuditor(GuardarUsuarioDTO usuario)
        {
            return this.CincoSDAO.GuardarAuditor(usuario);
        }

        public Dictionary<string, object> GetAuditorPrivilegio(int idAuditor)
        {
            return this.CincoSDAO.GetAuditorPrivilegio(idAuditor);
        }

        public Dictionary<string, object> GuardarAuditorPrivilegio(AuditorPrivilegioDTO privilegio)
        {
            return this.CincoSDAO.GuardarAuditorPrivilegio(privilegio);
        }

        public Dictionary<string, object> EliminarAuditorPrivilegio(int idAuditor)
        {
            return this.CincoSDAO.EliminarAuditorPrivilegio(idAuditor);
        }

        public Dictionary<string, object> GetTablaLideres(List<string> ccs)
        {
            return this.CincoSDAO.GetTablaLideres(ccs);
        }

        public Dictionary<string, object> GetLider(int idLider)
        {
            return this.CincoSDAO.GetLider(idLider);
        }

        public Dictionary<string, object> GuardarLider(LiderInfoDTO info)
        {
            return this.CincoSDAO.GuardarLider(info);
        }

        public Dictionary<string, object> EliminarLider(int idLider)
        {
            return this.CincoSDAO.EliminarLider(idLider);
        }

        public Dictionary<string, object> GetAreaOperativaLider()
        {
            return this.CincoSDAO.GetAreaOperativaLider();
        }

        public Dictionary<string, object> GetPrivilegios()
        {
            return this.CincoSDAO.GetPrivilegios();
        }

        public Dictionary<string, object> GetTablaSubAreas()
        {
            return this.CincoSDAO.GetTablaSubAreas();
        }

        public Dictionary<string, object> GetSubArea(int id)
        {
            return this.CincoSDAO.GetSubArea(id);
        }

        public Dictionary<string, object> EditarSubArea(int id, string nombre)
        {
            return this.CincoSDAO.EditarSubArea(id, nombre);
        }

        public Dictionary<string, object> EliminarSubArea(int id)
        {
            return this.CincoSDAO.EliminarSubArea(id);
        }

        public Dictionary<string, object> GuardarSubArea(string nombre)
        {
            return this.CincoSDAO.GuardarSubArea(nombre);
        }

        public bool AccesoPermitido(PrivilegioEnum privilegio)
        {
            return this.CincoSDAO.AccesoPermitido(privilegio);
        }
        #endregion

        #region SEGUIMIENTOS
        #region SeguimientoPlanAccion
        public Dictionary<string, object> llenarTablaPlanAccion(AuditoriaDTO objParamDTO, DateTime fechaInicio, DateTime fechaFinal)
        {
            return this.CincoSDAO.llenarTablaPlanAccion(objParamDTO, fechaInicio, fechaFinal);
		}
		#endregion
        #endregion
		
        #region REPORTES
        public Dictionary<string, object> GetEstadisticasTendencias(List<string> CCs, List<int> areas, DateTime fechaInicio, DateTime fechaFin)
        {
            return this.CincoSDAO.GetEstadisticasTendencias(CCs, areas, fechaInicio, fechaFin);
        }
        public Dictionary<string, object> GetReporteEjecutivo(List<string> CCs, List<int> areas, DateTime fechaInicio, DateTime fechaFin)
        {
            return this.CincoSDAO.GetReporteEjecutivo(CCs, areas, fechaInicio, fechaFin);
        }
        #endregion
    }
}
