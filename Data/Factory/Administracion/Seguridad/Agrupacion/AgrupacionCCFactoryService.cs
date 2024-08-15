using Core.DAO.Administracion.Seguridad.AgrupacionCC;
using Core.Service.Administracion.Seguridad.AgrupacionCC;
using Data.DAO.Administracion.Seguridad.AgrupacionCC;
using Core.DTO.Principal.Generales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Administracion.Seguridad.Agrupacion
{
    public class AgrupacionCCFactoryService
    {
        public IAgrupacionCCDAO getAgrupacionCCFactoryService()
        {
            return new AgrupacionCCService(new AgrupacionCCDAO());
        }
    }
}
