using Core.DAO.Encuestas;
using Core.Service.Encuestas;
using Data.DAO.Encuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Encuestas
{
    public class EncuestasProveedoresFactoryServices
    {
        public IEncuestasProveedorDAO getEncuestasProveedoresFactoryServices()
        {
            return new EncuestasProveedorServices(new EncuestasProveedoresDAO());
        }
    }
}
