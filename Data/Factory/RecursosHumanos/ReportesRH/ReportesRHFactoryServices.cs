using Core.DAO.RecursosHumanos.ReportesRH;
using Core.Service.RecursosHumanos.ReportesRH;
using Data.DAO.RecursosHumanos.ReportesRH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.RecursosHumanos.ReportesRH
{
    public class ReportesRHFactoryServices
    {
        public IReportesRHDAO getReportesRHService()
        {
            return new ReportesRHService(new ReportesRHDAO());
        }
    }
}
