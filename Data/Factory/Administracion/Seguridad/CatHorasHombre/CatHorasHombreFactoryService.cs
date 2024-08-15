using Core.DAO.Administracion.Seguridad;
using Core.Service.Administracion.Seguridad.CatHorasHombre;
using Data.DAO.Administracion.Seguridad.CatHorasHombre;


namespace Data.Factory.Administracion.Seguridad.CatHorasHombre
{
    public class CatHorasHombreFactoryService
    {
        public ICatHorasHombreDAO getCatHorasHombreService()
        {
            return new CatHorasHombreService(new CatHorasHombreDAO());
        }
    }
}