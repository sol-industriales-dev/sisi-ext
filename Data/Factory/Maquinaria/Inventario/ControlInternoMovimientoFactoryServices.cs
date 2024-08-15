using Core.DAO.Maquinaria.Inventario;
using Core.Service.Maquinaria.Inventario;
using Data.DAO.Maquinaria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Maquinaria.Inventario
{
    public class ControlInternoMovimientoFactoryServices
    {

        public IControlInternoMovimientosDAO getControlInternoMovimientoFactoryServices()
        {
            return new ControlInternoMovimientosServices(new ControlInternoMovimientosDAO());
        }
    }
}
