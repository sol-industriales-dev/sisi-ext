using Core.DAO.Enkontrol.Almacen;
using Core.Service.Enkontrol.Resguardo;
using Data.DAO.Enkontrol.Almacen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Enkontrol.Resguardo
{
    public class ResguardoFactoryService
    {
        public IResguardoDAO getResguardoService()
        {
            return new ResguardoService(new ResguardoDAO());
        }
    }
}
