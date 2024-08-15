using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DAO.Contabilidad.Nomina;
using Core.Service.Contabilidad.Nomina;
using Data.DAO.Contabilidad.Nomina;

namespace Data.Factory.Contabilidad.Nomina
{
    public class NominaFactoryService
    {
        public INominaDAO getNominaService()
        {
            return new NominaService(new NominaDAO());
        }
    }
}
