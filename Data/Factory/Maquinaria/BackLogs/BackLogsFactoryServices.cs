using Core.DAO.Maquinaria.BackLogs;
using Core.Service.Maquinaria.BackLogs;
using Data.DAO.Maquinaria.Backlogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Maquinaria.BackLogs
{
    public class BackLogsFactoryServices
    {
        public IBackLogsDAO GetBackLogs()
        {
            return new BackLogsService(new BackLogsDAO());
        }
    }
}
