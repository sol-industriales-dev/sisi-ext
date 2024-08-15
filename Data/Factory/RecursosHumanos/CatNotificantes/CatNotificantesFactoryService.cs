using Core.DAO.RecursosHumanos.CatNotificantes;
using Core.Service.RecursosHumanos.CatNotificantes;
using Data.DAO.RecursosHumanos.CatNotificantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.RecursosHumanos.CatNotificantes
{
    public class CatNotificantesFactoryService
    {
        public ICatNotificantesDAO GetCatNotificantesService()
        {
            return new CatNotificantesService(new CatNotificantesDAO());
        }
    }
}
