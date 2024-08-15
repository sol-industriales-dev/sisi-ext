using Core.DAO.RecursosHumanos.Desempeno;
using Core.DTO.Principal.Generales;
using Core.Entity.RecursosHumanos.Desempeno;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;
using Core.DTO;
using System.Web;
using Core.DTO.RecursosHumanos.Desempeno;
using System.IO;

namespace Core.Service.RecursosHumanos.Desempeno
{
    public class DesempenoService : IDesempenoDAO
    {
        #region Init
        public IDesempenoDAO d_desempeno { get; set; }
        public IDesempenoDAO Desempeno
        {
            get { return d_desempeno; }
            set { d_desempeno = value; }
        }
        public DesempenoService(IDesempenoDAO Desempeno)
        {
            this.Desempeno = Desempeno;
        }
        #endregion        
        #region Dashboard
        public Respuesta UsuarioSesion()
        {
            return d_desempeno.UsuarioSesion();
        }
        #endregion
        #region Meta
        public bool guardarMeta(tblRH_ED_DetMetas meta)
        {
            return d_desempeno.guardarMeta(meta);
        }
        public bool guardarMeta(List<tblRH_ED_DetMetas> lst)
        {
            return d_desempeno.guardarMeta(lst);
        }
        public bool eliminarMeta(int id)
        {
            return d_desempeno.eliminarMeta(id);
        }
        public bool darVoboMeta(tblRH_ED_DetMetas meta)
        {
            return d_desempeno.darVoboMeta(meta);
        }
        public Respuesta VoBoMetas(int idProceso, int idEmpleado, int idEvaluacion)
        {
            return d_desempeno.VoBoMetas(idProceso, idEmpleado, idEvaluacion);
        }
        public List<tblRH_ED_DetMetas> getLstMetaPorProceso(int idProceso, int usuarioID)
        {
            return d_desempeno.getLstMetaPorProceso(idProceso, usuarioID);
        }
        public List<tblRH_ED_DetMetas> getLstMetaPorProceso(int idProceso, List<int> lstIdUsuario)
        {
            return d_desempeno.getLstMetaPorProceso(idProceso, lstIdUsuario);
        }
        public tblRH_ED_DetMetas getMeta(int idMeta)
        {
            return d_desempeno.getMeta(idMeta);
        }

        public Respuesta DiasSeguimiento(int idProceso)
        {
            return d_desempeno.DiasSeguimiento(idProceso);
        }

        public bool Notificado(int idEmpleado, int idEvaluacion, int? idJefe)
        {
            return d_desempeno.Notificado(idEmpleado, idEvaluacion, idJefe);
        }

        public Respuesta Notificar(int idEmpleado, int idEvaluacion, int idProceso, int? idJefe)
        {
            return d_desempeno.Notificar(idEmpleado, idEvaluacion, idProceso, idJefe);
        }
        public Respuesta NotificarMetas(int idEmpleado, int idProceso)
        {
            return d_desempeno.NotificarMetas(idEmpleado, idProceso);
        }
        #endregion
        #region Observaciones
        public List<tblRH_ED_DetObservacion> getLstObservacionesPorUsuario(int idUsuario)
        {
            return d_desempeno.getLstObservacionesPorUsuario(idUsuario);
        }
        public List<tblRH_ED_DetObservacion> getLstObservacionesPorProcesosYJefe(int idProceso, int idJefe)
        {
            return d_desempeno.getLstObservacionesPorProcesosYJefe(idProceso, idJefe);
        }
        public tblRH_ED_DetObservacion getObservacion(int idMeta, int idEvaluacion, int idUsuarioCalificar)
        {
            return d_desempeno.getObservacion(idMeta, idEvaluacion, idUsuarioCalificar);
        }
        public List<ComboDTO> CargarPuestosEmpleados(List<string> empleadosIDs)
        {
            return d_desempeno.CargarPuestosEmpleados(empleadosIDs);
        }
        public bool eliminarEvidencia(int id)
        {
            return d_desempeno.eliminarEvidencia(id);
        }
        public string metaEvidenciaGuardar(tblRH_ED_DetObservacion eva, List<HttpPostedFileBase> lstArchivo)
        {
            return d_desempeno.metaEvidenciaGuardar(eva, lstArchivo);
        }

        public Tuple<Stream, string> DescargarEvidenciasMeta(int idObservacion)
        {
            return d_desempeno.DescargarEvidenciasMeta(idObservacion);
        }

        public Respuesta GetEvaluaciones()
        {
            return d_desempeno.GetEvaluaciones();
        }

        public Respuesta GetEvaluacionesPorProceso(int idProceso)
        {
            return d_desempeno.GetEvaluacionesPorProceso(idProceso);
        }

        public Respuesta CUDEvaluacion(EvaluacionDTO objEvaluacion)
        {
            return d_desempeno.CUDEvaluacion(objEvaluacion);
        }

        public Respuesta NotificacionEvaluacion(tblRH_ED_DetObservacion info)
        {
            return d_desempeno.NotificacionEvaluacion(info);
        }
        public tblRH_ED_CatEvaluacion GetInfoEvaluacion(int idEvaluacion)
        {
            return d_desempeno.GetInfoEvaluacion(idEvaluacion);
        }
        #endregion
        #region Semaforo
        public List<tblRH_ED_CatSemaforo> getLstSemaforo()
        {
            return d_desempeno.getLstSemaforo();
        }
        #endregion
        #region Empleados
        public tblRH_ED_Empleado getEmpleado(int idEmpleado)
        {
            return d_desempeno.getEmpleado(idEmpleado);
        }
        public List<tblRH_ED_Empleado> getSubordinado(int idUsuario)
        {
            return d_desempeno.getSubordinado(idUsuario);
        }
        public List<tblRH_ED_Empleado> CargarTblEmpleados(bool estatus)
        {
            return d_desempeno.CargarTblEmpleados(estatus);
        }

        public List<ReporteDesempenoDTO> CargarTblPersonalEvaluado(int proceso, int periodo)
        {
            return d_desempeno.CargarTblPersonalEvaluado(proceso, periodo);
        }
        public List<tblP_Usuario> getEmpleados(string term)
        {
            return d_desempeno.getEmpleados(term);
        }
        public List<tblRH_ED_Empleado> getEmpleadosDesempeno(string term)
        {
            return d_desempeno.getEmpleadosDesempeno(term);
        }
        public Respuesta GuardarEmpleado(int empleadoID, int? jefeID, int tipo)
        {
            return d_desempeno.GuardarEmpleado(empleadoID, jefeID, tipo);
        }
        public Respuesta VerComo(int idUsuario)
        {
            return d_desempeno.VerComo(idUsuario);
        }
        public Respuesta ModificarProcesoEmpleado(List<int> idProceso, int idEmpleado)
        {
            return d_desempeno.ModificarProcesoEmpleado(idProceso, idEmpleado);
        }
        public Respuesta EliminarEmpleado(int idEmpleado)
        {
            return d_desempeno.EliminarEmpleado(idEmpleado);
        }
        #endregion
        #region combobox
        public List<ComboDTO> getCboProceso(int idEmpleado)
        {
            return d_desempeno.getCboProceso(idEmpleado);
        }
        public List<ComboDTO> getCboEvaluacionPorProceso(int idProceso)
        {
            return d_desempeno.getCboEvaluacionPorProceso(idProceso);
        }
        public List<ComboDTO> getCboMetaPorProceso(int idProceso, int idEmpleado)
        {
            return d_desempeno.getCboMetaPorProceso(idProceso, idEmpleado);
        }
        public List<ComboDTO> getCboEstrategias()
        {
            return d_desempeno.getCboEstrategias();
        }
        public List<ComboDTO> getCboTodosLosProcesos()
        {
            return d_desempeno.getCboTodosLosProcesos();
        }
        public Dictionary<string, object> getEvaluacionVigenteID(int idProceso)
        {
            return d_desempeno.getEvaluacionVigenteID(idProceso);
        }
        #endregion
        #region Calendario
        public Respuesta GetEvaluaciones(int? idUsuarioVercomo)
        {
            return d_desempeno.GetEvaluaciones(idUsuarioVercomo);
        }
        #endregion
        #region Procesos
        public Respuesta CRUDProceso(CRUDProcesoDTO objProceso)
        {
            return d_desempeno.CRUDProceso(objProceso);
        }

        public Respuesta ObtenerTodosLosProcesos()
        {
            return d_desempeno.ObtenerTodosLosProcesos();
        }

        public tblRH_ED_CatProceso GetProceso(int idProceso)
        {
            return d_desempeno.GetProceso(idProceso);
        }
        #endregion
        #region Empleado Meta
        public List<tblRH_ED_Empleado> getLstEmpleadoJefe(int idJefe)
        {
            return d_desempeno.getLstEmpleadoJefe(idJefe);
        }
        #endregion
    }
}
