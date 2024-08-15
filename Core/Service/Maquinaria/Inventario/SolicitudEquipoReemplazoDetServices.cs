using Core.DAO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.Maquinaria.Inventario
{
    public class SolicitudEquipoReemplazoDetServices : ISolicitudEquipoReemplazoDetDAO
    {
        #region Atributos
        private ISolicitudEquipoReemplazoDetDAO m_interfazDAO;
        #endregion

        #region Propiedades
        private ISolicitudEquipoReemplazoDetDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion

        #region Constructores
        public SolicitudEquipoReemplazoDetServices(ISolicitudEquipoReemplazoDetDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion
        public void Guardar(List<tblM_SolicitudReemplazoDet> obj)
        {
            interfazDAO.Guardar(obj);

        }

        public void GuardarDetalleArchivos(tblM_SolicitudReemplazoDet obj, HttpPostedFileBase file)
        {
            interfazDAO.GuardarDetalleArchivos(obj, file);
        }

        public List<tblM_SolicitudReemplazoDet> GetSolicitudReemplazoDetByIdSolicitud(int id)
        {
            return interfazDAO.GetSolicitudReemplazoDetByIdSolicitud(id);
        }

        public List<tblM_SolicitudReemplazoEquipo> GetSolicitudesPendientes(int tipo)
        {
            return interfazDAO.GetSolicitudesPendientes(tipo);
        }

        public tblM_SolicitudReemplazoDet GetSolicitudReemplazoDetById(int id)
        {
            return interfazDAO.GetSolicitudReemplazoDetById(id);
        }

        public void GuardarSingle(tblM_SolicitudReemplazoDet obj)
        {
            interfazDAO.GuardarSingle(obj);
        }
    }
}
