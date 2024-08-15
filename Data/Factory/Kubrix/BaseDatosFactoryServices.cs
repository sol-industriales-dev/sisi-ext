using Core.DAO.Kubrix;
using Core.Service.Kubrix;
using Data.DAO.Kubrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Kubrix
{
    public class BaseDatosFactoryServices
    {
        public IBaseDatosDAO getBDServices()
        {
            return new BaseDatosServices(new BaseDatosDAO());
        }
    }
}
