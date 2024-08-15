using Core.DAO.Principal.Alertas;
using Core.Service.Principal.Alertas;
using Data.DAO.Principal.Alertas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Principal.Alertas
{
    public class AlertaFactoryServices
    {
        public IAlertasDAO getAlertaService()
        {
            return new AlertaService(new AlertasDAO());
        }
    }
}
