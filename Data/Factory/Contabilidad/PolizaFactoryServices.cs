using Core.DAO.Contabilidad.Poliza;
using Core.Service.Contabilidad.Poliza;
using Data.DAO.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Contabilidad
{
    public class PolizaFactoryServices
    {
        public IPolizaDAO getPolizaService() {
            return new PolizaService(new PolizaDAO());
        }
    }
}
