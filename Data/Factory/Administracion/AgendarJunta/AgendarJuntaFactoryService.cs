using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DAO.Administracion.AgendarJunta;
using Core.Entity.Administrativo.AgendarJunta;
using Core.Service.Administracion.AgendarJunta;
using Data.DAO.Administracion.AgendarJunta;

namespace Data.Factory.Administracion.AgendarJunta
{
    public class AgendarJuntaFactoryService
    {
        public IAgendarJuntaDAO getJuntaService()
        {
            return new AgendarJuntaService(new AgendarJuntaDAO());
        }
    }
}
