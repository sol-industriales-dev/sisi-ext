using Core.DAO.ControlObra.AdministradorProyectos;
using Core.Service.ControlObra.AdministradorProyectos;
using Data.DAO.ControlObra.AdministradorProyectos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.ControlObra.AdministradorProyectos
{
    public class CGPFactoryService
    {
        public ICGPDAO getCGPService()
        {
            return new CGPService(new CGPDAO());
        }
    }
}
