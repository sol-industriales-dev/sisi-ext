using Core.DTO.Principal.Generales;
using Core.Entity.RecursosHumanos.Desempeno;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;
using System.Web;
using Core.DTO;
using Core.DTO.RecursosHumanos.Desempeno;
using System.IO;

namespace Core.DAO.RecursosHumanos.Desempeno
{
    public interface IDesempenoDAO
    {
        #region Dashboard
        Respuesta UsuarioSesion();
        #endregion
        #region Meta
        bool guardarMeta(tblRH_ED_DetMetas meta);
        bool guardarMeta(List<tblRH_ED_DetMetas> lst);
        bool eliminarMeta(int id);
        bool darVoboMeta(tblRH_ED_DetMetas meta);
        Respuesta VoBoMetas(int idProceso, int idEmpleado, int idEvaluacion);
        List<tblRH_ED_DetMetas> getLstMetaPorProceso(int idProceso, int usuarioID);
        List<tblRH_ED_DetMetas> getLstMetaPorProceso(int idProceso, List<int> lstIdUsuario);
        tblRH_ED_DetMetas getMeta(int idMeta);

        Respuesta DiasSeguimiento(int idProceso);

        bool Notificado(int idEmpleado, int idEvaluacion, int? idJefe);
        Respuesta Notificar(int idEmpleado, int idEvaluacion, int idProceso, int? idJefe);
        Respuesta NotificarMetas(int idEmpleado, int idProceso);
        #endregion
        #region Semaforo
        List<tblRH_ED_CatSemaforo> getLstSemaforo();
        #endregion
        #region Empleados
        tblRH_ED_Empleado getEmpleado(int idEmpleado);
        List<tblRH_ED_Empleado> getSubordinado(int idUsuario);
        List<tblRH_ED_Empleado> CargarTblEmpleados(bool estatus);
        List<ReporteDesempenoDTO> CargarTblPersonalEvaluado(int proceso, int periodo);
        
        List<tblP_Usuario> getEmpleados(string term);
        List<tblRH_ED_Empleado> getEmpleadosDesempeno(string term);
        Respuesta GuardarEmpleado(int empleadoID, int? jefeID, int tipo);
        Respuesta VerComo(int idUsuario);
        Respuesta ModificarProcesoEmpleado(List<int> idProceso, int idEmpleado);
        Respuesta EliminarEmpleado(int idEmpleado);
        #endregion
        #region Observaciones
        List<tblRH_ED_DetObservacion> getLstObservacionesPorUsuario(int idUsuario);
        List<tblRH_ED_DetObservacion> getLstObservacionesPorProcesosYJefe(int idProceso, int idJefe);
        tblRH_ED_DetObservacion getObservacion(int idMeta, int idEvaluacion, int idUsuarioCalificar);
        string metaEvidenciaGuardar(tblRH_ED_DetObservacion eva, List<HttpPostedFileBase> lstArchivo);
        Tuple<Stream, string> DescargarEvidenciasMeta(int idObservacion);

        List<ComboDTO> CargarPuestosEmpleados(List<string> empleadosIDs);
        bool eliminarEvidencia(int id);

        Respuesta GetEvaluaciones();

        Respuesta GetEvaluacionesPorProceso(int idProceso);

        Respuesta CUDEvaluacion(EvaluacionDTO objEvaluacion);

        Respuesta NotificacionEvaluacion(tblRH_ED_DetObservacion info);
        tblRH_ED_CatEvaluacion GetInfoEvaluacion(int idEvaluacion);
        #endregion
        #region combobox
        List<ComboDTO> getCboProceso(int idEmplado);
        List<ComboDTO> getCboEvaluacionPorProceso(int idProceso);
        List<ComboDTO> getCboMetaPorProceso(int idProceso, int idEmpleado);
        List<ComboDTO> getCboEstrategias();
        List<ComboDTO> getCboTodosLosProcesos();
        Dictionary<string, object> getEvaluacionVigenteID(int idProceso);

        #endregion        
        #region Calendario
        Respuesta GetEvaluaciones(int? idUsuarioVerComo);
        #endregion
        #region Procesos
        Respuesta CRUDProceso(CRUDProcesoDTO objProceso);
        Respuesta ObtenerTodosLosProcesos();
        tblRH_ED_CatProceso GetProceso(int idProceso);
        #endregion
        #region Empleados Meta
        List<tblRH_ED_Empleado> getLstEmpleadoJefe(int idJefe);
        #endregion
    }
}
