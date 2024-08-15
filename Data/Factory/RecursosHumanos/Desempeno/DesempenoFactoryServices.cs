using Core.DAO.RecursosHumanos.Desempeno;
using Core.Service.RecursosHumanos.Desempeno;
using Data.DAO.RecursosHumanos.Desempeno;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.RecursosHumanos.Desempeno
{
    public class DesempenoFactoryServices
    {
        public IDesempenoDAO getDesempenoService()
        {
            return new DesempenoService(new DesempenoDAO());
        }
    }
}
