using Core.DAO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Inventario
{
    public class SolicitudEquipoDetServices : ISolicitudEquipoDetDAO
    {
        #region Atributos
        private ISolicitudEquipoDetDAO m_SolicitudEquipoDetDAO;
        #endregion
        #region Propiedades
        public ISolicitudEquipoDetDAO SolicitudEquipoDetDAO
        {
            get { return m_SolicitudEquipoDetDAO; }
            set { m_SolicitudEquipoDetDAO = value; }
        }
        #endregion
        #region Constructores
        public SolicitudEquipoDetServices(ISolicitudEquipoDetDAO solicitudEquipoDetDAO)
        {
            this.SolicitudEquipoDetDAO = solicitudEquipoDetDAO;
        }
        #endregion

        public void Guardar(List<tblM_SolicitudEquipoDet> obj)
        {
            SolicitudEquipoDetDAO.Guardar(obj);
        }

        public void delete(int obj)
        {
            SolicitudEquipoDetDAO.delete(obj);
        }
        public List<tblM_SolicitudEquipoDet> listaDetalleSolicitud(int obj)
        {
            return SolicitudEquipoDetDAO.listaDetalleSolicitud(obj);

        }
        public tblM_SolicitudEquipoDet DetSolicitud(int obj)
        {
            return SolicitudEquipoDetDAO.DetSolicitud(obj);

        }
        public void Guardar(tblM_SolicitudEquipoDet obj)
        {
            SolicitudEquipoDetDAO.Guardar(obj);
        }

        public List<tblM_SolicitudEquipo> ListaSolicitudes(string cc)
        {
            return SolicitudEquipoDetDAO.ListaSolicitudes(cc);
        }

        public tblM_SolicitudEquipo getSolicitudbyID(int obj)
        {
            return SolicitudEquipoDetDAO.getSolicitudbyID(obj);
        }
    }
}
