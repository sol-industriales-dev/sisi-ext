using Core.DAO.RecursosHumanos.ActoCondicion;
using Core.Service.RecursosHumanos.ActoCondicion;
using Data.DAO.RecursosHumanos.ActoCondicion;

namespace Data.Factory.RecursosHumanos
{
    public class ActoCondicionCHFactoryService
    {
        public IActoCondicionCHDAO GetActoCondicionCHService()
        {
            return new ActoCondicionCHService(new ActoCondicionCHDAO());
        }
    }
}
