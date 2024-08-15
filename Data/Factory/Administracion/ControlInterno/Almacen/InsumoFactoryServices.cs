using Core.DAO.Administracion.ControlInterno.Almacen;
using Core.Service.Administracion.ControlInterno.Almacen;
using Data.DAO.Administracion.ControlInterno.Reporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Administracion.ControlInterno.Almacen
{
    public class InsumoFactoryServices
    {
        public IinsumosDAO getRepTraspasoServices()
        {
            return new InsumoService(new InsumoDAO());
        }
    }
}
