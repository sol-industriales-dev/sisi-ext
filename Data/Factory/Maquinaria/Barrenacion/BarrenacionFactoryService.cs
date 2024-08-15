using Core.DAO.Maquinaria.Barrenacion;
using Core.Service.Maquinaria.Barrenacion;
using Data.DAO.Maquinaria.Barrenacion;

namespace Data.Factory.Maquinaria.Barrenacion
{
    public class BarrenacionFactoryService
    {

        public IBarrenacionDAO GetBarrenacionService()
        {
            return new BarrenacionService(new BarrenacionDAO());
        }

    }
}
