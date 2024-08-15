using Core.DAO.CuentasPorCobrar;
using Core.Service.CuentasPorCobrar;
using Data.DAO.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.CuentasPorCobrar
{
    public class CuentasPorCobrarFactoryService
    {
        public ICuentasPorCobrarDAO getCuentasPorCobrarService()
        {
            return new CuentasPorCobrarService(new CuentasPorCobrarDAO());
        }
    }
}
