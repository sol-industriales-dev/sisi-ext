using Core.DAO.Administracion.Facultamiento;
using Core.Service.Administracion.Facultamiento;
using Data.DAO.Administracion.Facultamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Administracion.Facultamiento
{
    public class FacultamientoFactoryServices
    {
        public IFacultamientoDAO getFacutamientoService()
        {
            return new FacultamientoServices(new FacultamientoDAO());
        }
    }
}
