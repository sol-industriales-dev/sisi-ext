using Core.DAO.Maquinaria.Captura;
using Core.Service.Maquinaria.Capturas;
using Data.DAO.Maquinaria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Maquinaria.Captura
{
    public class KPIFactoryServices
    {
        public IKPIDAO getKPIFactoryService()
        {
            return new KPIService(new KPIDAO());
        }
    }
}
