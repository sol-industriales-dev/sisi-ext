using Core.DAO.Administracion.Seguridad.SeguimientoCompromisos;
using Core.Service.Administracion.Seguridad.SeguimientoCompromisos;
using Data.DAO.Administracion.Seguridad.SeguimientoCompromisos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Administracion.Seguridad.SeguimientoCompromisos
{
    public class SeguimientoCompromisosFactoryService
    {
        public ISeguimientoCompromisosDAO getSeguimientoCompromisosService()
        {
            return new SeguimientoCompromisosService(new SeguimientoCompromisosDAO());
        }
    }
}
