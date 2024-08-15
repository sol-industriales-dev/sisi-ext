using Core.DAO.Maquinaria.Overhaul;
using Core.Service.Maquinaria.Overhaul;
using Data.DAO.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Maquinaria.Overhaul
{
    public class PresupuestoOverhaulFactoryServices
    {
        public IPresupuestoOverhaulDAO getPresupuestoOverhaulFactoryServices()
        {
            return new PresupuestoOverhaulServices(new PresupuestoOverhaulDAO());
        }
    }
}