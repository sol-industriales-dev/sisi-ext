using Core.DAO.Maquinaria.Captura;
using Core.Service.Maquinaria.Capturas;
using Data.DAO.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Maquinaria.Captura
{
    public class RN_MaquinariaFactoryServices
    {
        public IRN_MaquinariaDAO getRN_MaquinariaServices()
        {
            return new RN_MaquinariaServices(new RN_MaquinariaDAO());
        }
    }
}