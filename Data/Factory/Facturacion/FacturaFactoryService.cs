using Core.DAO.Facturacion;
using Core.Service.Facturacion;
using Data.DAO.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Facturacion
{
    public class FacturaFactoryService
    {
        public IFacturaciónDAO getFacturaService()
        {
            return new FacturaciónService(new FacturaciónDAO());
        }
    }
}
