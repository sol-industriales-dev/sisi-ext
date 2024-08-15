using Core.DAO.RecursosHumanos.Captura;
using Core.Service.RecursosHumanos.Captura;
using Data.DAO.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.RecursosHumanos.Captura
{
    public class FormatoCambioFactoryService
    {
        public IFormatoCambio getFormatoCambioService()
        {
            return new FormatoCambioService(new FormatoCambioDAO());
        }
    }
}
