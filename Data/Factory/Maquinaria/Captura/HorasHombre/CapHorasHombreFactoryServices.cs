using Core.DAO.Maquinaria.Captura.HorasHombre;
using Core.Service.Maquinaria.Capturas.HorasHombre;
using Data.DAO.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Maquinaria.Captura.HorasHombre
{
    public class CapHorasHombreFactoryServices
    {
        public ICapHorasHombreDAO getCapHorasHombreFactoryServices()
        {
            return new CapHorasHombreService(new CapHorasHombreDAO());
        }
    }
}
