using Core.DAO.Maquinaria.SOS;
using Core.Service.Maquinaria.SOS;
using Data.DAO.Maquinaria.SOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Maquinaria.SOS
{
    public class MinadoFactoryServices
    {
        public IMinadoDAO getMinadoService()
        {
            return new MinadoServices(new MinadoDAO());
        }
    }
}
