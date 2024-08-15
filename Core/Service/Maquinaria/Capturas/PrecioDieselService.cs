using Core.DAO.Maquinaria.Captura;
using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Capturas
{
    public class PrecioDieselService : IPrecioDieselDAO
    {
        #region Atributos
        private IPrecioDieselDAO m_PrecioDieselDAO;
        #endregion
        #region Propiedades
        public IPrecioDieselDAO PrecioDieselDAO
        {
            get { return m_PrecioDieselDAO; }
            set { m_PrecioDieselDAO = value; }
        }
        #endregion
        #region Constructores
        #endregion
        public PrecioDieselService(IPrecioDieselDAO precioDieselDAO)
        {
            this.PrecioDieselDAO = precioDieselDAO;
        }

        public tblM_CapPrecioDiesel GetPrecioDiesel()
        {
            return PrecioDieselDAO.GetPrecioDiesel();
        }
        public void Guardar(tblM_CapPrecioDiesel obj)
        {
            PrecioDieselDAO.Guardar(obj);
        }
    }
}
