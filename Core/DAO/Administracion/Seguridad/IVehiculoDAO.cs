using Core.Entity.Administrativo.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Administracion.Seguridad
{
    public interface IVehiculoDAO
    {
        tblS_Vehiculo getVehiculo(string cc, string eco, DateTime fecha);
        List<tblS_Observaciones> getLstObs(int idVehiculo);
        List<tblS_CatPartes> getLstPartes();
        List<object> fillCboEconomico(string cc);
    }
}
