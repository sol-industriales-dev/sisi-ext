using Core.DAO.Facturacion.Prefacturacion;
using Core.Service.Facturacion.Prefacturacion;
using Data.DAO.Facturacion.Prefacturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Facturacion.Prefacturacion
{
    public class PrefacturacionFactoryServices
    {
        public IPrefacturacionDAO getPrefacturacionServices()
        {
            return new PrefacturacionService(new PrefacturacionDAO());
        }
    }
}
