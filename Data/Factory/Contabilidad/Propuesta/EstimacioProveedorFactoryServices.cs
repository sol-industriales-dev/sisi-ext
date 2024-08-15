using Core.DAO.Contabilidad.Propuesta;
using Core.Service.Contabilidad.Propuesta;
using Data.DAO.Contabilidad.Propuesta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Contabilidad.Propuesta
{
    public class EstimacioProveedorFactoryServices
    {
        public IEstimacioProveedorDAO getEstProvService()
        {
            return new EstimacioProveedorServices(new EstimacioProveedorDAO());
        }
    }
}
