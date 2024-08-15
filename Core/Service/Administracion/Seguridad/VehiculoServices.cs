using Core.DAO.Administracion.Seguridad;
using Core.Entity.Administrativo.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Administracion.Seguridad
{
    public class VehiculoServices : IVehiculoDAO
    {
        #region Atributos
        private IVehiculoDAO s_vehiculo;
        #endregion
        #region Propiedades
        public IVehiculoDAO VehiculoDAO {
            get { return s_vehiculo; }
            set { s_vehiculo = value; }
        }
        #endregion
        #region Constructores
        public VehiculoServices(IVehiculoDAO vehiculoDAO)
        {
            this.VehiculoDAO = vehiculoDAO;
        }
        #endregion
        public tblS_Vehiculo getVehiculo(string cc, string eco, DateTime fecha)
        {
            return VehiculoDAO.getVehiculo(cc, eco, fecha);
        }
        public List<tblS_Observaciones> getLstObs(int idVehiculo)
        {
            return VehiculoDAO.getLstObs(idVehiculo);
        }
        public List<tblS_CatPartes> getLstPartes()
        {
            return VehiculoDAO.getLstPartes();
        }
        public List<object> fillCboEconomico(string cc)
        {
            return VehiculoDAO.fillCboEconomico(cc);
        }
    }
}
