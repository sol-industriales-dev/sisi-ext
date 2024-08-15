using Core.DAO.Maquinaria.Reporte;
using Core.Service.Maquinaria.Reporte;
using Data.DAO.Maquinaria.Reporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Maquinaria.Reporte
{
    public class RptIndicadorFactoryServices
    {
        public IRptIndicadorDAO getRptIndicadorService()
        {
            return new RptIndicadorServices(new RptIndicadorDAO());
        }
    }
}
