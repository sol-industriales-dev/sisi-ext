using Core.Entity.Maquinaria.Reporte.CuadroComparativo.Financiero;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Reporte.CuadroComparativo
{
    public interface ICCFinanieroDAO
    {
        #region AsignacionNoEconomico
        List<tblM_CCF_EncFinanciero> getCuadrosFinancieros(List<int> lstIdAsignacion);
        #endregion
    }
}
