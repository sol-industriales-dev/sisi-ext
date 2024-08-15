using Core.DAO.RecursosHumanos.BajasPersonal;
using Core.Service.RecursosHumanos.Reclutamientos;
using Data.DAO.RecursosHumanos.BajasPersonal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.RecursosHumanos.BajasPersonal
{
    public class BajasPersonalEntrevistaFactoryService
    {

        public IBajasPersonalEntrevistaDAO GetBajasPersonalEntrevistaService()
        {
            return new BajasPersonalEntrevistaService(new BajasPersonalEntrevistaDAO());
        }

    }
}
