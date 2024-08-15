using Core.DAO.Administracion.Seguridad.CatDepartamentos;
using Core.Service.Administracion.Seguridad;
using Data.DAO.Administracion.Seguridad.CatDepartamentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Administracion.Seguridad
{
    public class CatDepartamentosFactoryService
    {
        public ICatDepartamentosDAO getCatDepartamentosService()
        {
            return new CatDepartamentosService(new CatDepartamentosDAO());
        }
    }
}
