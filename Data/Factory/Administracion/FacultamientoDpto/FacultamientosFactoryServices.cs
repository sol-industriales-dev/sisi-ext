using Core.DAO.Administracion.FacultamientosDpto;
using Data.DAO.Administracion.FacultamientosDpto;
using Core.Service.Administracion.FacultamientosDpto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Administracion.FacultamientsDpto
{
    public class FacultamientosFactoryServices
    {
        public IFacultamientosDAO getFacultamientosService()
        {
            return new FacultamientosServices(new FacultamientosDAO());
        }
    }
}
