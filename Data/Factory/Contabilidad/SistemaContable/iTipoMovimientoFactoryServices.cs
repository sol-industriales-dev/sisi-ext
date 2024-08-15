using Core.DAO.Contabilidad.SistemaContable;
using Core.Service.Contabilidad.SistemaContable;
using Data.DAO.Contabilidad.SistemaContable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Contabilidad.SistemaContable
{
    public class iTipoMovimientoFactoryServices
    {
        public IiTipoMovimientoDAO getiTmServices()
        {
            return new iTipoMovimientoServices(new iTipoMovimientoDAO());
        }
    }
}
