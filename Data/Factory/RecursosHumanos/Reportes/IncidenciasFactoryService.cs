using Core.DAO.RecursosHumanos.Reportes;
using Core.Service.RecursosHumanos.Reportes;
using Data.DAO.RecursosHumanos.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.RecursosHumanos.Reportes
{
    public class IncidenciasFactoryService
    {
        public IIncidencias getIncidenciasService()
        {
            return new IncidenciasReporteService(new IncidenciasDAO());
        }
    }
}
