using Core.DAO.Contabilidad.Cuenta;
using Core.Service.Contabilidad.Cuenta;
using Data.DAO.Enkontrol.General.Cuenta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Contabilidad.Cuenta
{
    public class CuentaFactoryService
    {
        public ICuentaDAO GetCuentaEkService()
        {
            return new CuentaService(new CuentaEkDAO());
        }
    }
}
