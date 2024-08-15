using Core.DAO.Maquinaria.Reporte.CuadroComparativo;
using Core.Service.Maquinaria.Reporte.CuadroComparativo;
using Data.DAO.Maquinaria.Reporte.CuadroComparativo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Maquinaria.Reporte.CuadroComparativo
{
    public class CCFinancieroFactoryServices
    {
        public ICCFinanieroDAO getFinancieroServices()
        {
            return new CCFinanieroServices(new CCFinanieroDAO());
        }
    }
}
