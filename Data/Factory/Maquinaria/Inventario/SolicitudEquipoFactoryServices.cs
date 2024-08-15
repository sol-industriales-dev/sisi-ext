using Core.DAO.Maquinaria.Inventario;
using Core.Service.Maquinaria.Inventario;
using Data.DAO.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Maquinaria.Inventario
{
    public class SolicitudEquipoFactoryServices
    {
        public ISolicitudEquipoDAO getSolicitudEquipoServices()
        {
           return new SolicitudEquipoServices(new SolicitudEquipoDAO());
        }
    }
}
