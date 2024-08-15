using Core.DAO.Facturacion.Enkontrol;
using Core.Service.Facturacion.Enkontrol;
using Data.DAO.Facturacion.Enkontrol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Facturacion.Enkontrol
{
    public class FacturasSPFactoryService
    {
         
        public IFacturasSPDAO getFacturasSPFactoryService()
        {
            return new FacturasSPService(new FacturasSPDAO());
        }

        public IFacturasSPDAO getFacturasEKFactoryService()
        {
            return new FacturasSPService(new FacturasEKDAO());
        }

    }
}
