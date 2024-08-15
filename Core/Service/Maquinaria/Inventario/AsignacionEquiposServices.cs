using Core.DAO.Maquinaria.Inventario;
using Core.DTO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Catalogos;
using Core.DTO.Maquinaria.Inventario.Comparativos;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.Enum.Principal.Bitacoras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Inventario
{
    public class AsignacionEquiposServices : IAsignacionEquiposDAO
    {

        #region Atributos
        private IAsignacionEquiposDAO m_AsignacionEquiposDAO;
        #endregion
        #region Propiedades
        public IAsignacionEquiposDAO AsignacionEquiposDAO
        {
            get { return m_AsignacionEquiposDAO; }
            set { m_AsignacionEquiposDAO = value; }
        }
        #endregion
        #region Constructores
        public AsignacionEquiposServices(IAsignacionEquiposDAO asignacionEquiposDAO)
        {
            this.AsignacionEquiposDAO = asignacionEquiposDAO;
        }
        #endregion

        public void SaveOrUpdate(List<tblM_AsignacionEquipos> obj)
        {
            AsignacionEquiposDAO.SaveOrUpdate(obj);
        }

        public void SaveOrUpdate(tblM_AsignacionEquipos obj)
        {
            AsignacionEquiposDAO.SaveOrUpdate(obj);
        }

        public tblM_AsignacionEquipos GetAsiganacionBySolicitud(int obj)
        {
            return AsignacionEquiposDAO.GetAsiganacionBySolicitud(obj);
        }

        public List<tblM_CatMaquina> getMaquinasAsignadas(List<int> idEconomicos)
        {
            return AsignacionEquiposDAO.getMaquinasAsignadas(idEconomicos);
        }
        public List<int> getMaquinariaAsiganadasD(DateTime FechaInico)
        {
            return AsignacionEquiposDAO.getMaquinariaAsiganadasD(FechaInico);
        }

        public List<tblM_AsignacionEquipos> getAsignacionesByEconomicos(int economicoID)
        {
            return AsignacionEquiposDAO.getAsignacionesByEconomicos(economicoID);
        }


        public List<tblM_AsignacionEquipos> getAsignacionActivas(List<string> ccs)
        {
            return AsignacionEquiposDAO.getAsignacionActivas(ccs);
        }

        public tblM_AsignacionEquipos GetAsiganacionById(int obj)
        {
            return AsignacionEquiposDAO.GetAsiganacionById(obj);
        }

        public List<tblM_AsignacionEquipos> getAsignacionesCompras()
        {
            return AsignacionEquiposDAO.getAsignacionesCompras();
        }
        public tblM_AsignacionEquipos getAsignacionesCompras(int id)
        {
            return AsignacionEquiposDAO.getAsignacionesCompras(id);
        }
        public tblM_AsignacionEquipos getEconomicoAsignado(int idEconomico)
        {
            return AsignacionEquiposDAO.getEconomicoAsignado(idEconomico);
        }


        public bool GetAsginadosRecibidos(int obj)
        {
            return AsignacionEquiposDAO.GetAsginadosRecibidos(obj);
        }
        public List<tblM_AsignacionEquipos> getAsignacionesByCC(string obj)
        {
            return AsignacionEquiposDAO.getAsignacionesByCC(obj);
        }

        public List<tblM_AsignacionEquipos> getAsignacionesByID(int id)
        {
            return AsignacionEquiposDAO.getAsignacionesByID(id);
        }

        public List<tblM_AsignacionEquipos> getAsignacionesByIDs(int id)
        {
            return AsignacionEquiposDAO.getAsignacionesByIDs(id);
        }

        public void delete(tblM_AsignacionEquipos Entidad)
        {
            AsignacionEquiposDAO.delete(Entidad);
        }
        public List<tblM_AsignacionEquipos> GetListaAsignaciones()
        {
            return AsignacionEquiposDAO.GetListaAsignaciones();
        }
        public List<tblM_AsignacionEquipos> getAsignacionesbySDet(int id)
        {
            return AsignacionEquiposDAO.getAsignacionesbySDet(id);
        }

        public List<tblM_AsignacionEquipos> GetAsignacionControles(int Filtro, string CentroCostos)
        {
            return AsignacionEquiposDAO.GetAsignacionControles(Filtro, CentroCostos);
        }

        public List<HistoricoMaquinariaDTO> GetHistorialMaquina(string EconomicoID)
        {
            return AsignacionEquiposDAO.GetHistorialMaquina(EconomicoID);
        }

        public List<HistoricoMaquinariaDTO> getHistorialEconomicos(string EconomicoID)
        {
            return AsignacionEquiposDAO.getHistorialEconomicos(EconomicoID);
        }

        public void logErrores(int sistema, int modulo, string controlador, string action, Exception exception, AccionEnum tipo, long registroId, object objeto)
        {
            AsignacionEquiposDAO.logErrores(sistema, modulo, controlador, action, exception, tipo, registroId, objeto);
        }
        public List<CuadroAutorizacionDTO> CargarSolicitudes(int estado, string obra, DateTime fechaInicio, DateTime fechaFin)
        {
            return AsignacionEquiposDAO.CargarSolicitudes(estado, obra, fechaInicio, fechaFin);
        }

    }


}
