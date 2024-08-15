using Core.DAO.Administracion.Seguridad.ActoCondicion;
using Core.Service.Administracion.Seguridad.ActoCondicion;
using Data.DAO.Administracion.Seguridad.ActoCondicion;

namespace Data.Factory.Administracion.Seguridad.ActoCondicion
{
    public class ActoCondicionFactoryService
    {
        public IActoCondicionDAO GetActoCondicionService()
        {
            return new ActoCondicionService(new ActoCondicionDAO());
        }
    }
}
