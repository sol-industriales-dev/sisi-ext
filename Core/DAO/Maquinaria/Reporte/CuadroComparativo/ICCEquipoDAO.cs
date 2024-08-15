using Core.Entity.Maquinaria.Reporte.CuadroComparativo.Equipo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Reporte.CuadroComparativo
{
    public interface ICCEquipoDAO
    {
        #region AsignacionNoEconomico
        List<tblM_CCE_EncEquipo> GetCuadroEquipo(List<int> lstIdAsignacion);
        #endregion
        #region _formCCEquipo
        List<tblM_CCE_CatConcepto> LstCatalogoActivo();
        tblM_CCE_EncEquipo GetCuadroEquipo(int idAsignacion);
        #endregion
    }
}
