using Core.DAO.Administracion.Contratistas;
using Core.Service.Administracion.Contratistas;
using Data.DAO.Administracion.Contratistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Administracion.Contratistas
{
    public class EmpleadosFactoryService
    {
        public IEmpleadosDAO getEmpleadosService()
        {
            return new EmpleadosService(new EmpleadosDAO());
        }
    }
}
