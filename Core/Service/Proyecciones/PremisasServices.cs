using Core.DAO.Proyecciones;
using Core.DTO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Proyecciones
{
    public class PremisasServices : IPremisasDAO
    {
        #region Atributos
        private IPremisasDAO m_interfazDAO;
        #endregion
        #region Propiedades
        private IPremisasDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion
        #region Constructores
        public PremisasServices(IPremisasDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion

        public tblPro_Premisas GetJsonData(FiltrosGeneralDTO objFiltro)
        {
            return interfazDAO.GetJsonData(objFiltro);
        }
        public void GuardarActualizarPremisas(tblPro_Premisas obj)
        {
            interfazDAO.GuardarActualizarPremisas(obj);
        }
    }
}
