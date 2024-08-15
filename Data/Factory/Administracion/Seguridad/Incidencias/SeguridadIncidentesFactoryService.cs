using Core.DAO.Administracion.Seguridad;
using Core.Service.Administracion.Seguridad;
using Data.DAO.Administracion.Seguridad.Incidencias;

namespace Data.Factory.Administracion.Seguridad.Incidencias
{
    public class SeguridadIncidentesFactoryService
    {
        public ISeguridadIncidentesDAO getSeguridadIncidenteService()
        {
            return new SeguridadIncidentesServices(new SeguridadIncidentesDAO());
        }
    }
}
