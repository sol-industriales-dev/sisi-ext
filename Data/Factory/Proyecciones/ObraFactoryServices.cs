using Core.DAO.Proyecciones;
using Core.Service.Proyecciones;
using Data.DAO.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Proyecciones
{
    public class ObraFactoryServices
    {
        public IObraDAO getObraServices()
        {
            return new ObraServices(new ObraDAO());
        }
    }
}
