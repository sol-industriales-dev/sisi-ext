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
    public class SolicitudEquipoReemplazoServices : ISolicitudEquipoReemplazo
    {
        #region Atributos
        private ISolicitudEquipoReemplazo m_interfazDAO;
        #endregion

        #region Propiedades
        private ISolicitudEquipoReemplazo interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion

        #region Constructores
        public SolicitudEquipoReemplazoServices(ISolicitudEquipoReemplazo InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion
        public void Guardar(tblM_SolicitudReemplazoEquipo obj)
        {
            interfazDAO.Guardar(obj);

        }
        public List<tblM_SolicitudReemplazoEquipo> ListaSolicitudesEquipoByCC(string CC)
        {
            return interfazDAO.ListaSolicitudesEquipoByCC(CC);
        }

        public string GetFolio(string obj)
        {
            return interfazDAO.GetFolio(obj);
        }
        public  tblM_SolicitudReemplazoEquipo GetSolicitudReemplazobyID(int idSolicitud)
        {
            return interfazDAO.GetSolicitudReemplazobyID(idSolicitud);
        }

    }
}
