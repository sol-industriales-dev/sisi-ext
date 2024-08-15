using Core.DAO.Maquinaria.Inventario;
using Core.Service.Maquinaria.Inventario;
using Data.DAO.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Proyecciones
{
    public class AutorizaStandbyFactoryServices
    {
        public IAutorizaStandbyDAO GetAutorizaStandby()
        {
            return new AutorizaStandbyServices(new AutorizaStandByDAO());
        }

    }
}
