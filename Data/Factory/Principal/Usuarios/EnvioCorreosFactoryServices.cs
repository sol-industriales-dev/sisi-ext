using Core.DAO.Principal.Usuarios;
using Core.Service.Principal.Usuarios;
using Data.DAO.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Principal.Usuarios
{
    public class EnvioCorreosFactoryServices
    {
        public IenvioCorreosDAO getEnvioCorreosFactoryServices()
        {
            return new EnvioCorreosService(new envioCorreosDAO());
        }
    }
}
