using Core.DAO.Contabilidad.Presupuesto;
using Core.Service.Contabilidad.Presupuesto;
using Data.DAO.Contabilidad.Presupuesto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Contabilidad.Presupuesto
{
    public class ControlPresupuestoFactoryService
    {
        public IControlPresupuestalDAO GetControlPresupuestalService()
        {
            return new ControlPresupuestalService(new ControlPresupuestalDAO());
        }
    }
}
