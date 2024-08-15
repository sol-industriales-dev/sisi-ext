using Core.DAO.ControlObra;
using Core.Service.ControlObra;
using Data.DAO.ControlObra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.ControlObra
{
    public class MatrizDeRiesgoFactoryService
    {
        public IMatrizDeRiesgoDAO getMatrizDeRiesgoService()
        {
            return new MatrizDeRiesgoService(new MatrizDeRiesgoDAO());
        }
    }
}
