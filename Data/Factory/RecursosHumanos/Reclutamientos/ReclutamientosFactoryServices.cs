using Core.DAO.RecursosHumanos.Reclutamientos;
using Core.Service.RecursosHumanos.Reclutamientos;
using Data.DAO.RecursosHumanos.Reclutamientos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.RecursosHumanos
{
    public class ReclutamientosFactoryServices
    {
        public IReclutamientosDAO getReclutamientosService()
        {
            return new ReclutamientosService(new ReclutamientosDAO());
        }
    }
}
