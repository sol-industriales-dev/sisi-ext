using Core.DAO.Maquinaria.Mantenimiento;
using Core.Service.Maquinaria.Mantenimiento;
using Data.DAO.Maquinaria.Mantenimiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Maquinaria.Mantenimiento
{
   public class GrupoComponenteModeloFactoryServices
    {
       public IGrupoComponenteModeloDAO GrupoComponenteModeloService()
        {
            return new GrupoComponenteModeloService(new GrupoComponenteModeloDAO());
        }
    }
}
