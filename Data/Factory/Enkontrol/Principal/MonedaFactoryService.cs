using Core.DAO.Enkontrol.Principal;
using Core.Service.Enkontrol.Principal;
using Data.DAO.Enkontrol.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Enkontrol.Principal
{
    public class MonedaFactoryService
    {
        public IMonedaDAO getMonedaService()
        {
            return new MonedaService(new MonedaDAO());
        }
    }
}
