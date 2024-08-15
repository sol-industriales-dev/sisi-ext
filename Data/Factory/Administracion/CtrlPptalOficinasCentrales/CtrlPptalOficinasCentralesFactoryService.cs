using Core.DAO.Administracion.CtrlPresupuestalOficinasCentrales;
using Core.Service.Administracion.CtrlPresupuestalOficinasCentrales;
using Data.DAO.Administracion.CtrlPresupuestalOficinasCentrales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Factory.Administracion.CtrlPresupuestalOficinasCentrales
{
    public class CtrlPresupuestalOficinasCentralesFactoryService
    {
        public ICtrlPresupuestalOficinasCentralesDAO getCtrlPresupuestalOficinasCentrales()
        {
            return new CtrlPresupuestalOficinasCentralesService(new CtrlPresupuestalOficinasCentralesDAO());
        }
    }
}
