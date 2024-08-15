using Core.DAO.Administracion.Seguridad;
using Core.Service.Administracion.Seguridad;
using Data.DAO.Administracion.Seguridad.Capacitacion;

namespace Data.Factory.Administracion.Seguridad.Capacitacion
{
    public class CapacitacionFactoryService
    {
        public ICapacitacionDAO GetCapacitacionService()
        {
            return new CapacitacionService(new CapacitacionDAO());
        }
    }
}
