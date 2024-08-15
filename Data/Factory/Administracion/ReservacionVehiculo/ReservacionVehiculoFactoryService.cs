using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DAO.Administracion.ReservacionVehiculo;
using Data.DAO.Administracion.ReservacionVehiculo;
using Core.Service.Administracion.ReservacionVehiculo;

namespace Data.Factory.Administracion.ReservacionVehiculo
{
    public class ReservacionVehiculoFactoryService
    {
        public IReservacionVehiculoDAO getControlObraService()
        {
            return new ReservacionVehiculoService(new ReservacionVehiculoDAO());
        }
    }
}
