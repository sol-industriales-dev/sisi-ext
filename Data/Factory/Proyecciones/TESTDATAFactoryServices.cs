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
    public class TESTDATAFactoryServices
    {
        public ITESTDATADAO getDATAtestServices()
        {
            return new TESTDATAServices(new TESTDATADAO());
        }
    }
}
