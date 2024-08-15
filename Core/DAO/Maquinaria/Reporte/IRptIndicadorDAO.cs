using Core.Entity.Maquinaria.Reporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Reporte
{
    public interface IRptIndicadorDAO
    {
        void SaveReporte(tblM_RptIndicador obj);
        tblM_RptIndicador getReporte(int tipo, DateTime fechaInicio, DateTime fechaFin, string cc);
    }
}
