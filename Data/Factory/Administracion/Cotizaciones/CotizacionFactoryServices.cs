using Core.DAO.Administracion.Cotizaciones;
using Core.Service.Administracion.Cotizaciones;
using Data.DAO.Administracion.Cotizaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Administracion.Cotizaciones
{
    public class CotizacionFactoryServices
    {
        public ICotizacionDAO getCotizacionService()
        {
            return new CotizacionService(new CotizacionDAO());
        }
    }
}
