using Core.DAO.Administracion.Seguridad.Requerimientos;
using Core.Service.Administracion.Seguridad.Requerimientos;
using Data.DAO.Administracion.Seguridad.Requerimientos;

namespace Data.Factory.Administracion.Seguridad.Requerimientos
{
    public class RequerimientosFactoryService
    {
        public IRequerimientosDAO GetRequerimientosService()
        {
            return new RequerimientosService(new RequerimientosDAO());
        }
    }
}
