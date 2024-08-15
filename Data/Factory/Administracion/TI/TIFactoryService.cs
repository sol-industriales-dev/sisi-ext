using Core.DAO.Administracion.TI;
using Core.Service.Administracion.TI;
using Data.DAO.Administracion.TI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Administracion.TI
{
    public class TIFactoryService
    {
        public ITIDAO GetTI()
        {
            return new TIService(new TIDAO());
        }
    }
}
