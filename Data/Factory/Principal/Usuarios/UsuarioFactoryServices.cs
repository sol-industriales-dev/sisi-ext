using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Service.Principal.Usuarios;
using Core.DAO.Principal.Usuarios;
using Data.DAO.Principal.Usuarios;

namespace Data.Factory.Principal.Usuarios
{
    public class UsuarioFactoryServices
    {
        public IUsuarioDAO getUsuarioService()
        {
            return new UsuarioService(new UsuarioDAO());
        }
    }
}
