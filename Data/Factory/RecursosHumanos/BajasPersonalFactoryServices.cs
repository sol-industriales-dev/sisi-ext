using Core.DAO.RecursosHumanos.Bajas;
using Core.Service.RecursosHumanos.Reclutamientos;
using Data.DAO.RecursosHumanos.BajasPersonal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.RecursosHumanos
{
    public class BajasPersonalFactoryServices
    {
        public IBajasPersonalDAO GetBajasPersonalService()
        {
            return new BajasPersonalService(new BajasPersonalDAO());
        }
    }
}
