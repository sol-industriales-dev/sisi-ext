using Core.DAO.SeguimientoAcuerdos;
using Core.Service.SeguimientoAcuerdos;
using Data.DAO.SeguimientoAcuerdos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.SeguimientoAcuerdos
{
    public class SeguimientoAcuerdosFactoryServices
    {
        public ISeguimientoAcuerdosDAO getSeguimientoAcuerdosService()
        {
            return new SeguimientoAcuerdosService(new SeguimientoAcuerdosDAO());
        }
    }
}
