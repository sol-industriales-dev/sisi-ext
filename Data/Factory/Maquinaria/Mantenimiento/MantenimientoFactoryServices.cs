using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DAO.Maquinaria.Mantenimiento;//interfaz
using Core.Service.Maquinaria.Mantenimiento;//service
using Data.DAO.Maquinaria.Mantenimiento;//clase datos
namespace Data.Factory.Maquinaria.Mantenimiento
{
    public class MantenimientoFactoryServices
    {
        public IMantenimientoDAO getMantenimientoService()
        {
            return new MantenimientoService(new MantenimientoDAO());
        }
    }
}