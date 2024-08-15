using Core.DAO.Contabilidad.Poliza;
using Core.Service.Contabilidad.Poliza;
using Data.DAO.Contabilidad.Poliza;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Contabilidad.Poliza
{
    public class ConversionPolizaFactoryServices
    {
        public IConversionPolizaDAO getPolizaService()
        {
            return new ConversionPolizaService(new ConversionPolizaDAO());
        }
    }
}
