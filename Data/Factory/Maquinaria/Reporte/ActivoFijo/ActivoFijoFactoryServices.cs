using Core.DAO.Maquinaria.Reporte.ActivoFijo;
using Core.Service.Maquinaria.Reporte.ActivoFijo;
using Data.DAO.Maquinaria.Reporte.ActivoFijo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoFactoryServices
    {
        public IActivoFijoDAO getActivoFijoServices()
        {
            return new ActivoFijoServices(new ActivoFijoDAO());
        }
    }
}