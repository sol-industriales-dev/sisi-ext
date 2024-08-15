using Core.DAO.RecursosHumanos.Empleado;
using Core.Service.Enkontrol.General.RH.Empleado;
using Data.DAO.Enkontrol.RH.Empleado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.RecursosHumanos.Empleado
{
    public class EmpleadoFactoryService
    {
        public IEmpleadoDAO GetEmpleadoService()
        {
            return new EmpleadoService(new EmpleadoDAO());
        }
    }
}
