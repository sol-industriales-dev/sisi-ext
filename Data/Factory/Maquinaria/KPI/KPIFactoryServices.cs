

using Core.DAO.Maquinaria.KPI;
using Core.Service.Maquinaria.KPI;
using Data.DAO.Maquinaria.KPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Maquinaria.KPI
{
    public class KPIFactoryServices
    {
        public IKPIDAO KPIFactoruServices()
        {
            return new KPIService(new KPIDAO());
        }
    }
}
