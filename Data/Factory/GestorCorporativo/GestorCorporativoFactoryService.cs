using Core.DAO.GestorCorporativo;
using Core.Service.GestorCorporativo;
using Data.DAO.GestorCorporativo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.GestorCorporativo
{
    public class GestorCorporativoFactoryService
    {
        public IGestorCorporativoDAO getDirectorioService()
        {
            return new GestorCorporativoService(new GestorCorporativoDAO());
        }
    }
}
