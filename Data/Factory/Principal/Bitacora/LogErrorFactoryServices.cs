using Core.DAO.Principal.Bitacoras;
using Core.Service.Principal.Bitacora;
using Data.DAO.Principal;

namespace Data.Factory.Principal.Bitacora
{
    public class LogErrorFactoryServices
    {
        public ILogErrorDAO getLogErrorService()
        {
            return new LogErrorService(new LogErrorDAO());
        }
    }
}
