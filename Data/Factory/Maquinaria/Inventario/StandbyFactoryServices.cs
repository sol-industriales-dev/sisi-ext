using Core.DAO.Maquinaria.Inventario;
using Core.Service.Maquinaria.Inventario;
using Data.DAO.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Maquinaria.Inventario
{
    public class StandbyFactoryServices
    {
        public IStandByDAO getStandbyFactoryServices()
        {
            return new StandbyServices(new StandbyDAO());
        }
    }
}
