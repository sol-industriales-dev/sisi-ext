using Core.DAO.Maquinaria.Inventario;
using Core.DTO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario.ControlCalidad;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Principal.Bitacoras;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.Maquinaria.Inventario
{
    public class ControlRecepcionyEnvioServices : IControlRecepcionyEnvioDAO
    {
        #region Atributos
        private IControlRecepcionyEnvioDAO m_ControlRecepcionyEnvioDAO;
        #endregion
        #region Propiedades
        public IControlRecepcionyEnvioDAO ControlRecepcionyEnvioDAO
        {
            get { return m_ControlRecepcionyEnvioDAO; }
            set { m_ControlRecepcionyEnvioDAO = value; }
        }
        #endregion
        #region Constructores
        public ControlRecepcionyEnvioServices(IControlRecepcionyEnvioDAO controlRecepcionyEnvioDAO)
        {
            ControlRecepcionyEnvioDAO = controlRecepcionyEnvioDAO;
        }
        #endregion

        public List<tblM_AsignacionEquipos> getListEquiposPendientes(int obj)
        {
            return ControlRecepcionyEnvioDAO.getListEquiposPendientes(obj);
        }

        public tblM_CatMaquina GetInfoMaquinaria(int idEconomico)
        {
            return ControlRecepcionyEnvioDAO.GetInfoMaquinaria(idEconomico);
        }

        public void SaveOrUpdate(tblM_ControlEnvioMaquinaria obj, HttpPostedFileBase file)
        {
            ControlRecepcionyEnvioDAO.SaveOrUpdate(obj, file);
        }

        public tblM_AsignacionEquipos getInfoAsignacion(int obj)
        {
            return ControlRecepcionyEnvioDAO.getInfoAsignacion(obj);
        }

        public tblM_ControlEnvioMaquinaria getReporteEnvio(int idControl)
        {
            return ControlRecepcionyEnvioDAO.getReporteEnvio(idControl);
        }
        public tblM_ControlEnvioMaquinaria getReporteEnvioTipo(int idControl, int tipo)
        {
            return ControlRecepcionyEnvioDAO.getReporteEnvioTipo(idControl, tipo);
        }
        public tblM_ControlEnvioMaquinaria getReporteRecepcion(int idEconomico, int idSolicitud, int tipo)
        {
            return ControlRecepcionyEnvioDAO.getReporteRecepcion(idEconomico, idSolicitud, tipo);
        }

        public tblM_ControlEnvioMaquinaria getInfoControl(int idAsignacion, int TipoControl, int idSolicitud)
        {
            return ControlRecepcionyEnvioDAO.getInfoControl(idAsignacion, TipoControl, idSolicitud);
        }
        public List<LiberacionDTO> getMaquinariaAsignada(int cc)
        {
            return ControlRecepcionyEnvioDAO.getMaquinariaAsignada(cc);
        }

        public List<LiberacionDTO> getMaquinariaAsignadaPendienteAutorizar(int cc)
        {
            return ControlRecepcionyEnvioDAO.getMaquinariaAsignadaPendienteAutorizar(cc);
        }
        public List<tblM_AsignacionEquipos> GetListaControles(int obj, List<tblP_CC_Usuario> listObj, int tipoFiltro)
        {
            return ControlRecepcionyEnvioDAO.GetListaControles(obj, listObj, tipoFiltro);
        }

        public List<tblM_AsignacionEquipos> GetPendientesEnvio(List<tblP_CC_Usuario> listObj)
        {
            return ControlRecepcionyEnvioDAO.GetPendientesEnvio(listObj);
        }
        public List<tblM_AsignacionEquipos> GetListaControlesCalidad(int obj, List<tblP_CC_Usuario> listObj, int tipoFiltro, DateTime? fechaInicio, DateTime? fechaFin, string cc, int? numEconomico)
        {
            return ControlRecepcionyEnvioDAO.GetListaControlesCalidad(obj, listObj, tipoFiltro, fechaInicio, fechaFin, cc, numEconomico);
        }

        public List<tblM_AsignacionEquipos> GetListaControlesPendientesRecepcion()
        {
            return ControlRecepcionyEnvioDAO.GetListaControlesPendientesRecepcion();
        }

        public string getCorreoGerente(string centroCostos)
        {
            return ControlRecepcionyEnvioDAO.getCorreoGerente(centroCostos);
        }

        public string getAC(string ac_cc)
        {
            return ControlRecepcionyEnvioDAO.getAC(ac_cc);
        }
        public void QuitarComponentes(int maquinaid)
        {
             ControlRecepcionyEnvioDAO.QuitarComponentes(maquinaid);
        }

        public List<string> getCorreosAdministradoresMaquinaria(tblM_CatControlCalidad objCalidad)
        {
            return ControlRecepcionyEnvioDAO.getCorreosAdministradoresMaquinaria(objCalidad);
        }

        public Tuple<Stream, string> descargarArchivos(int idAsignacion, int solicitudID)
        {
            return ControlRecepcionyEnvioDAO.descargarArchivos(idAsignacion, solicitudID);
        }

        public bool asignacionContieneArchivos(int idAsignacion)
        {
            return ControlRecepcionyEnvioDAO.asignacionContieneArchivos(idAsignacion);
        }

        #region FILL COMBOS
        public Dictionary<string, object> GetCCs()
        {
            return ControlRecepcionyEnvioDAO.GetCCs();
        }

        public Dictionary<string, object> GetEconomicos()
        {
            return ControlRecepcionyEnvioDAO.GetEconomicos();
        }

        #endregion
    }
}
