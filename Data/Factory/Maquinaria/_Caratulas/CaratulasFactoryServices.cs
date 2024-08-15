
using Core.DAO.Maquinaria.Caratulas;
using Core.Service.Maquinaria;
using Core.Service.Maquinaria.Caratulas;
using Data.DAO.Maquinaria;
using Data.DAO.Maquinaria.Caratulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Maquinaria.Caratulas
{
    public class CaratulasFactoryServices
    {
        public ICaratulasDAO GetCaratula()
        {
            return new CaratulasService(new CaratulasDAO());
        }
    }
}
