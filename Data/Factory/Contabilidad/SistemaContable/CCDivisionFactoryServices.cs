using Core.DAO.Contabilidad.Reportes;
using Core.Service.Contabilidad.Reportes;
using Data.DAO.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Contabilidad
{
    public class CCDivisionFactoryServices
    {
        public ICCDivisionDAO getCcDivisionService()
        {
            return new CCDivisionServices(new CCDivisionDAO());
        }
    }
}
