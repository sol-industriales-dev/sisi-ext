using Core.DTO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario.ControlCalidad;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.Maquinaria.Inventario
{
    public interface IControlRecepcionyEnvioDAO
    {
        List<tblM_AsignacionEquipos> getListEquiposPendientes(int obj);
        tblM_AsignacionEquipos getInfoAsignacion(int obj);
        tblM_CatMaquina GetInfoMaquinaria(int idEconomico);
        void SaveOrUpdate(tblM_ControlEnvioMaquinaria obj, HttpPostedFileBase file);
        tblM_ControlEnvioMaquinaria getReporteEnvio(int idControl);
        tblM_ControlEnvioMaquinaria getReporteEnvioTipo(int obj, int tipo);
        tblM_ControlEnvioMaquinaria getReporteRecepcion(int idEconomico, int idSolicitud, int tipo);
        tblM_ControlEnvioMaquinaria getInfoControl(int idAsignacion, int TipoControl, int idSolicitud);
        List<LiberacionDTO> getMaquinariaAsignada(int cc);
        List<LiberacionDTO> getMaquinariaAsignadaPendienteAutorizar(int cc);
        List<tblM_AsignacionEquipos> GetListaControles(int obj, List<tblP_CC_Usuario> listObj, int tipoFiltro);
        List<tblM_AsignacionEquipos> GetPendientesEnvio(List<tblP_CC_Usuario> listObj);
        List<tblM_AsignacionEquipos> GetListaControlesCalidad(int obj, List<tblP_CC_Usuario> listObj, int tipoFiltro, DateTime? fechaInicio, DateTime? fechaFin, string cc, int? numEconomico);
        List<tblM_AsignacionEquipos> GetListaControlesPendientesRecepcion();
        string getCorreoGerente(string centroCostos);
        string getAC(string ac_cc);
        void QuitarComponentes(int maquinaid);
        List<string> getCorreosAdministradoresMaquinaria(tblM_CatControlCalidad objCalidad);
        Tuple<Stream, string> descargarArchivos(int idAsignacion, int solicitudID);
        bool asignacionContieneArchivos(int idAsignacion);

        #region FILL COMBO
        Dictionary<string, object> GetCCs();
        Dictionary<string, object> GetEconomicos();
        #endregion
    }


}

