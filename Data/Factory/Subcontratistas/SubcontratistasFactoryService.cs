using Core.DAO.Subcontratistas;
using Core.Service.Subcontratistas;
using Data.DAO.Subcontratistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Subcontratistas
{
    public class SubcontratistasFactoryService
    {
        public ISubcontratistasDAO getSubService()
        {
            return new SubcontratistasService(new SubcontratistasDAO());
        }
    }
}
