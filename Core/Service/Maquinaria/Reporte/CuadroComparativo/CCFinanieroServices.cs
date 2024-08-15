using Core.DAO.Maquinaria.Reporte.CuadroComparativo;
using Core.Entity.Maquinaria.Reporte.CuadroComparativo.Financiero;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Reporte.CuadroComparativo
{
    public class CCFinanieroServices : ICCFinanieroDAO
    {
        #region Atributos
        private ICCFinanieroDAO m_FinancieroDAO;
        #endregion
        #region Propiedades
        public ICCFinanieroDAO FinancieroDAO
        {
            get { return m_FinancieroDAO; }
            set { m_FinancieroDAO = value; }
        }
        #endregion
        #region Constructores
        public CCFinanieroServices(ICCFinanieroDAO FinancieroDAO)
        {
            this.FinancieroDAO = FinancieroDAO;
        }
        #endregion
        #region AsignacionNoEconomico
        public List<tblM_CCF_EncFinanciero> getCuadrosFinancieros(List<int> lstIdAsignacion)
        {
            return FinancieroDAO.getCuadrosFinancieros(lstIdAsignacion);
        }
        #endregion
    }
}
