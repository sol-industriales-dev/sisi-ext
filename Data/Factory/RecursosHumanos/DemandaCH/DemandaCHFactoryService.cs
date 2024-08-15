using Core.DAO.RecursosHumanos.Demandas;
using Core.Service.RecursosHumanos.Demandas;
using Data.DAO.RecursosHumanos.Demandas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.RecursosHumanos.Demandas
{
    public class DemandaCHFactoryService
    {
        public IDemandaCHDAO GetDemandaDAO()
        {
            return new DemandaCHService(new DemandaCHDAO());
        }
    }
}