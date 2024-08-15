using Core.DAO.Contabilidad.Reportes;
using Core.Service.Contabilidad.Reportes;
using Data.EntityFramework.Mapping.Administrativo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Contabilidad.Reportes
{
    public class CatCCBaseFactoryServices
    {
        public ICatCCBaseDAO getBaseServices()
        {
            return new CatCCBaseServices(new CatCCBaseDAO());
        }
    }
}
