using Core.DAO.Maquinaria;
using Core.Service.Maquinaria.Capturas;
using Data.DAO.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Maquinaria.Captura
{
    public class ConciliacionFactoryServices
    {
        public IConciliacionDAO getConciliacionServices()
        {
            return new ConciliacionService(new ConciliacionDAO());
        }
    }
}
