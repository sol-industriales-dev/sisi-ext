using Core.DAO.Enkontrol.Almacen;
using Core.Service.Enkontrol.Almacen;
using Data.DAO.Enkontrol.Almacen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Enkontrol.Almacen
{
    public class ValuacionFactoryServices
    {
        public IValuacionDAO getVSerice()
        {
            return new ValuacionService(new ValuacionDAO());
        }
    }
}
