using Core.DAO.Administracion.Seguridad.SaludOcupacional;
using Core.Service.Administracion.Seguridad.SaludOcupacional;
using Data.DAO.Administracion.Seguridad.SaludOcupacional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Administracion.Seguridad.SaludOcupacional
{
    public class SaludOcupacionalFactoryService
    {
        public ISaludOcupacionalDAO getSaludOcupacionalService()
        {
            return new SaludOcupacionalService(new SaludOcupacionalDAO());
        }
    }
}
