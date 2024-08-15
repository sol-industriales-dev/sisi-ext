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
    public class CadenaProductivaFactoryServices
    {
        public ICadenaProductivaDAO getCadenaProductivaService()
        {
            return new CadenaProductivaServices(new CadenaProductivaDAO());
        }
    }
}
