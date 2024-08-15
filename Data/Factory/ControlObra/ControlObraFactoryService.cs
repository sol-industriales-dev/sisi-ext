using Core.DAO.ControlObra;
using Data.DAO.ControlObra;
using Core.Service.ControlObra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.ControlObra
{
    public class ControlObraFactoryService
    {
        public IControlObraDAO getControlObraService()
        {
            return new ControlObraService(new ControlObraDAO());
        }
    }
}
