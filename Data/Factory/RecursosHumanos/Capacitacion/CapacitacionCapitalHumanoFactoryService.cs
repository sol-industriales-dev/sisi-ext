using Core.DAO.RecursosHumanos.Capacitacion;
using Core.Service.RecursosHumanos.Capacitacion;
using Data.DAO.RecursosHumanos.Capacitacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.RecursosHumanos.Capacitacion
{
    public class CapacitacionCapitalHumanoFactoryService
    {
        public ICapacitacionCapitalHumanoDAO GetCapacitacionCapitalHumanoService()
        {
            return new CapacitacionCapitalHumanoService(new CapacitacionCapitalHumanoDAO());
        }
    }
}
