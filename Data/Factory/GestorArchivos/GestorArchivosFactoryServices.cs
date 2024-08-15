using Data.DAO.GestorArchivos;
using Core.Service.GestorArchivos;
using Core.DAO.GestorArchivos;

namespace Data.Factory.GestorArchivos
{
    public class GestorArchivosFactoryServices
    {
        public IGestorArchivosDAO GetGestorCorporativoService()
        {
            return new GestorArchivosService(new GestorArchivosDAO());
        }
    }
}
