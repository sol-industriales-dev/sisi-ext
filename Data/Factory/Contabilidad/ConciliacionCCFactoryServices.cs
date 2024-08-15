using Core.DAO.Contabilidad;
using Core.Service.Contabilidad;
using Data.DAO.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Contabilidad
{
    public class ConciliacionCCFactoryServices
    {

        public IConciliacionCCDAO GetConciliacionCCService()
        {
            return new ConciliacionCCService(new ConciliacionCCDAO());
        }


    }
}
