using Core.DAO.RecursosHumanos.Mural;
using Core.Service.RecursosHumanos.Mural;
using Data.DAO.RecursosHumanos.Mural;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.RecursosHumanos.Mural
{
    public class MuralFactoryServices
    {
        public IMuralDAO GetMuralService()
        {
            return new MuralService(new MuralDAO());
        }
    }
}
