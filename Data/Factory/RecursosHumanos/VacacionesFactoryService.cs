using Core.DAO.RecursosHumanos.Vacaciones;
using Core.Service.RecursosHumanos.Vacaciones;
using Data.DAO.RecursosHumanos.Vacaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.RecursosHumanos
{
    public class VacacionesFactoryService
    {

        public IVacacionesDAO GetVacacionesService()
        {
            return new VacacionesService(new VacacionesDAO());
        }

    }
}
