using Core.DAO.RecursosHumanos.Evaluacion360;
using Core.Service.RecursosHumanos.Evaluacion360;
using Data.DAO.RecursosHumanos.Evaluacion360;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.RecursosHumanos.Evaluacion360
{
    public class Evaluacion360FactoryServices
    {
        public IEvaluacion360DAO getEvaluacion360()
        {
            return new Evaluacion360Service(new Evaluacion360DAO());
        }
    }
}
