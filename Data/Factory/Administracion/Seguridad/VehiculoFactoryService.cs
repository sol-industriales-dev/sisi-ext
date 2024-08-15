using Core.DAO.Administracion.Seguridad;
using Core.Service.Administracion.Seguridad;
using Data.DAO.Administracion.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Administracion.Seguridad
{
    public class VehiculoFactoryService
    {
        public IVehiculoDAO getVehiculoService()
        {
            return new VehiculoServices(new VehiculoDAO());
        }
    }
}
