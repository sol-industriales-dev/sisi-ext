using Core.DAO.Administracion.ControlInterno.Reporte;
using Core.Service.Administracion.ControlInterno.Reporte;
using Data.DAO.Administracion.ControlInterno.Reporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Administracion.ControlInterno.Reporte
{
    public class RepTraspasoFactoryServices
    {
        public IRepTrapasoDAO getRepTraspasoServices()
        {
            return new RepTraspasoService(new RepTrapasoDAO());
        }
    }
}
