using Core.DAO.RecursosHumanos.Tabuladores;
using Core.Service.RecursosHumanos.Tabuladores;
using Data.DAO.RecursosHumanos.Tabuladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.RecursosHumanos.Tabuladores
{
    public class TabuladoresFactoryService
    {
        public ITabuladoresDAO GetTabuladoresService()
        {
            return new TabuladoresService(new TabuladoresDAO());
        }
    }
}
