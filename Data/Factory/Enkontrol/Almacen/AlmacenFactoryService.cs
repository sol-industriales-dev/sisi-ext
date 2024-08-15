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
    public class AlmacenFactoryService
    {
        public IAlmacenDAO getAlmService()
        {
            return new AlmacenService(new AlmacenDAO());
        }
    }
}
