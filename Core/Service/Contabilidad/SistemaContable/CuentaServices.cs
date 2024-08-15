using Core.DAO.Contabilidad.SistemaContable;
using Core.DTO.Contabilidad.SistemaContable.Cuentas;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.Cuentas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.SistemaContable
{
    public class CuentaServices : ICuentaDAO
    {
        #region Atributos
        private ICuentaDAO sc_CtaDAO;
        #endregion
        #region Propiedades
        public ICuentaDAO scCtaDAO
        {
            get { return sc_CtaDAO; }
            set { sc_CtaDAO = value; }
        }
        #endregion
        #region Contructor
        public CuentaServices(ICuentaDAO scCta)
        {
            scCtaDAO = scCta;
        }
        #endregion
        #region Asignación de cuentas
        public bool saveRelCuentas(List<tblC_Cta_RelCuentas> lst)
        {
            return sc_CtaDAO.saveRelCuentas(lst);
        }
        public bool DeleteCuenta(tblC_Cta_RelCuentas cuenta)
        {
            return sc_CtaDAO.DeleteCuenta(cuenta);
        }
        public List<tblC_Cta_RelCuentas> getRelCuentas(BusqAsignacionCuenta busq)
        {
            return scCtaDAO.getRelCuentas(busq);
        }
        public List<CatCtaEmpresa> getCatCta()
        {
            return scCtaDAO.getCatCta();
        }
        #endregion
    }
}
