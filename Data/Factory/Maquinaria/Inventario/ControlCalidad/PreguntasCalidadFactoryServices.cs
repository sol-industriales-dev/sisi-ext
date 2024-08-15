using Core.DAO.Maquinaria.Inventario.ControlCalidad;
using Core.Service.Maquinaria.Inventario.ControlCalidad;
using Data.DAO.Maquinaria.Inventario.ControlCalidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Maquinaria.Inventario.ControlCalidad
{
    public class PreguntasCalidadFactoryServices
    {
        public IPreguntasCalidadDAO getPreguntasFactoryServices()
        {
            return new PreguntasCalidadServices(new PreguntasCalidadDAO());
        }
    }


}
