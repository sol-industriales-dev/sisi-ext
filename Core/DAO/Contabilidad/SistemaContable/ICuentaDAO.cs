using Core.DTO.Contabilidad.SistemaContable.Cuentas;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.Cuentas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.SistemaContable
{
    public interface ICuentaDAO
    {
        #region Asignación de ctas
        bool saveRelCuentas(List<tblC_Cta_RelCuentas> lst);
        bool DeleteCuenta(tblC_Cta_RelCuentas cuenta);
        List<tblC_Cta_RelCuentas> getRelCuentas(BusqAsignacionCuenta busq);
        List<CatCtaEmpresa> getCatCta();
        #endregion
    }
}
