using Core.DAO.Maquinaria.Caratula;
using Core.Service.Maquinaria;
using Data.DAO.Maquinaria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Maquinaria.Caratula
{
    public class CaratulaFactoryServices
    {
        public ICaratulaDAO GetCaratula()
        {
            return new CaratulaServices(new CaratulaDAO());
        }
    }
}
