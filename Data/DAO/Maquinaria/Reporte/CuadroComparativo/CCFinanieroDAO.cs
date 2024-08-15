using Core.DAO.Maquinaria.Reporte.CuadroComparativo;
using Core.Entity.Maquinaria.Reporte.CuadroComparativo.Financiero;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Reporte.CuadroComparativo
{
    public class CCFinanieroDAO : GenericDAO<tblM_CCF_EncFinanciero>, ICCFinanieroDAO
    {
        #region AsignacionNoEconomico
        public List<tblM_CCF_EncFinanciero> getCuadrosFinancieros(List<int> lstIdAsignacion)
        {
            try
            {
                lstIdAsignacion = lstIdAsignacion == null ? new List<int>() : lstIdAsignacion;
                return (from equipo in _context.tblM_CCF_EncFinanciero
                        where equipo.esActivo && lstIdAsignacion.Contains(equipo.IdAsignacion)
                        select equipo).ToList();
            }
            catch(Exception)
            {
                return new List<tblM_CCF_EncFinanciero>();
            }
        }
        #endregion
    }
}
