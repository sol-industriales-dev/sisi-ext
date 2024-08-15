using Core.DAO.Administracion.ControlInterno;
using Core.Service.Administracion.ControlInterno;
using Data.DAO.Administracion.ControlInterno;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Administracion.ControlInterno
{
    public class ObraFactoryServices
    {
        public IObraDAO getObraServices()
        {
            return new ObraService(new ObraDAO());
        }
    }
}
