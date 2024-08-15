using Core.DAO.Contabilidad.Cheque;
using Core.Service.Contabilidad.Cheque;
using Data.DAO.Contabilidad.Cheque;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Administracion.Cheque
{
    public class CapChequeFactoryServices
    {
        public ICapChequeDAO getChequeServices()
        {
            return new CapChequeServices(new CapChequeDAO());
        }
    }
}
