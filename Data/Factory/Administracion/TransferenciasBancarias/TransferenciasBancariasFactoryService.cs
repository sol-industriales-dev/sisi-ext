using Core.DAO.Administracion.TransferenciasBancarias;
using Core.Service.Administracion.TransferenciasBancarias;
using Data.DAO.Administracion.TransferenciasBancarias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Administracion.TransferenciasBancarias
{
    public class TransferenciasBancariasFactoryService
    {
        public ITransferenciasBancariasDAO getTransferenciasBancariasService()
        {
            return new TransferenciasBancariasService(new TransferenciasBancariasDAO());
        }
    }
}
