using Core.DAO.Administracion.SalaJuntas;
using Core.Service.Administracion.SalaJuntas;
using Data.DAO.Administracion.SalaJuntas;

namespace Data.Factory.Administracion.SalaJuntas
{
    public class SalaJuntasFactoryService
    {
        public ISalaJuntasDAO GetSalaJuntasService()
        {
            return new SalaJuntasService(new SalaJuntasDAO());
        }
    }
}
