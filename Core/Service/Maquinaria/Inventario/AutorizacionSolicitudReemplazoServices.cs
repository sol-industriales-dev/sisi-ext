using Core.DAO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Inventario
{
    public class AutorizacionSolicitudReemplazoServices : IAutorizacionSolicitudReemplazoDAO
    {
        #region Atributos
        private IAutorizacionSolicitudReemplazoDAO m_interfazDAO;
        #endregion

        #region Propiedades
        private IAutorizacionSolicitudReemplazoDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion

        #region Constructores
        public AutorizacionSolicitudReemplazoServices(IAutorizacionSolicitudReemplazoDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion
        public void Guardar(tblM_AutorizacionSolicitudReemplazo obj)
        {
            interfazDAO.Guardar(obj);

        }

        public tblM_AutorizacionSolicitudReemplazo getAutorizadores(int id)
        {
            return interfazDAO.getAutorizadores(id);
        }

        public List<tblM_SolicitudReemplazoEquipo> getListPendientes(int idUsuario, int tipo, string folio)
        {
            return interfazDAO.getListPendientes(idUsuario, tipo, folio);
        }


        public tblM_AutorizacionSolicitudReemplazo GetAutorizacionReemplazoByID(int id)
        {
            return interfazDAO.GetAutorizacionReemplazoByID(id);
        }
        public tblM_AutorizacionSolicitudReemplazo GetAutorizacionReemplazoByIDReemplazo(int id)
        {
            return interfazDAO.GetAutorizacionReemplazoByIDReemplazo(id);
        }

        public string obtenerComentarioSolicitudReemplazo(int solicitudID)
        {
            return interfazDAO.obtenerComentarioSolicitudReemplazo(solicitudID);
        }
    }
}
