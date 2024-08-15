using Core.DAO.RecursosHumanos.Captura;
using Core.Service.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DAO.RecursosHumanos.Captura;
namespace Data.Factory.RecursosHumanos.Captura
{
    public class AutorizacionAditivaDeductivaFactoryService
    {
        public IAutorizacionAditivaDeductivaDAO getAutAditivaDeductivaService()
        {
            return new AutorizacionAditivaDeductivaService (new AutorizacionAditivaDeductivaDAO());
        }
    }

}

