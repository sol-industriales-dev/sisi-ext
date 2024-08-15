using Core.DAO.Administracion.Seguridad;
using Core.Service.Administracion.Seguridad;
using Data.DAO.Administracion.Seguridad.CapacitacionSeguridad;

namespace Data.Factory.Administracion.Seguridad.CapacitacionSeguridad
{
    public class CapacitacionSeguridadFactoryService
    {
        public ICapacitacionSeguridadDAO GetCapacitacionSeguridadService()
        {
            return new CapacitacionSeguridadService(new CapacitacionSeguridadDAO());
        }
    }
}
