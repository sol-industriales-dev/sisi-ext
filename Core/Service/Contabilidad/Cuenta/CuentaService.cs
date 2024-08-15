using Core.DAO.Contabilidad.Cuenta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.Cuenta
{
    public class CuentaService : ICuentaDAO
    {
        private ICuentaDAO _cuentaDAO;

        private ICuentaDAO CuentaDAO
        {
            get { return _cuentaDAO; }
            set { _cuentaDAO = value; }
        }

        public CuentaService(ICuentaDAO cuenta)
        {
            this.CuentaDAO = cuenta;
        }

        public object BuscarCuenta(string term)
        {
            return this.CuentaDAO.BuscarCuenta(term);
        }

        public object GetCuenta(int cta, int scta, int sscta)
        {
            return this.CuentaDAO.GetCuenta(cta, scta, sscta);
        }

        public object GetCuenta(int cta, int scta, string descripcion)
        {
            return this.CuentaDAO.GetCuenta(cta, scta, descripcion);
        }

        public object GetCuentas(List<int> ctas)
        {
            return this.CuentaDAO.GetCuentas(ctas);
        }
    }
}
