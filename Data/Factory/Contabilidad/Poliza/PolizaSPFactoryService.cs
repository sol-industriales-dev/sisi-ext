using Core.DAO.Contabilidad.Poliza;
using Core.Service.Contabilidad.Poliza;
using Data.DAO.Contabilidad.Poliza;
using Data.DAO.Enkontrol.General.Poliza;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Contabilidad.Poliza
{
    public class PolizaSPFactoryService
    {
        public IPolizaSPDAO GetPolizaEkService()
        {
            return new PolizaSPService(new PolizaEkDAO());
        }

        public IPolizaSPDAO GetPolizaSPService()
        {
            return new PolizaSPService(new PolizaSPDAO());
        }
    }
}
