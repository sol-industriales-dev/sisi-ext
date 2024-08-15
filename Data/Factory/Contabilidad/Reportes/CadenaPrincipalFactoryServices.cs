using Core.DAO.Contabilidad.Reportes;
using Core.Service.Contabilidad.Reportes;
using Data.DAO.Contabilidad.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Contabilidad.Reportes
{
    public class CadenaPrincipalFactoryServices
    {
        public ICadenaPrincipalDAO getCadenaPrincipalService()
        {
            return new CadenaPrincipalServices(new CadenaPrincipalDAO());
        }
    }
}
