using Core.DAO.Comercializacion.CRM;
using Core.Service.Administracion.Comercializacion.CRM;
using Data.DAO.Administracion.Comercializacion.CRM;

namespace Data.Factory.Administracion.Comercilizacion.CRM
{
    public class CRMFactoryService
    {
        public ICRMDAO GetCRMService()
        {
            return new CRMService(new CRMDAO());
        }
    }
}