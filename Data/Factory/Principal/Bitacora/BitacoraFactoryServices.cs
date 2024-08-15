using Core.DAO.Principal.Bitacoras;
using Core.Service.Principal.Bitacora;
using Data.DAO.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Principal.Bitacora
{
    public class BitacoraFactoryServices
    {
        public IBitacoraDAO getBitacoraService()
        {
            return new BitacoraService(new BitacoraDAO());
        }
    }
}
