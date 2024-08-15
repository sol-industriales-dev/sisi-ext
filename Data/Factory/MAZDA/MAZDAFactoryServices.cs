using Data.DAO.MAZDA;
using Core.Service.MAZDA;
using Data.DAO.MAZDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DAO.MAZDA;

namespace Data.Factory.MAZDA
{
    public class MAZDAFactoryServices
    {
        public IPlanActividadesDAO getPlanActividadesService()
        {
            return new PlanActividadesService(new PlanActividadesDAO());
        }
    }
}
