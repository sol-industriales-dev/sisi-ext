using Core.DAO.Maquinaria.Inventario;
using Core.Service.Maquinaria.Inventario;
using Data.DAO.Maquinaria.Catalogos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Maquinaria.Catalogos
{
    public class HistInventarioFactoryServices
    {
        public IHistInventarioDAO getHistInventarioFactoryServices()
        {
            return new HistInventarioServices(new HistInventarioDAO());
        }
    }
}
