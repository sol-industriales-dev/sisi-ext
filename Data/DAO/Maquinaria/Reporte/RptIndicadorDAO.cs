using Core.DAO.Maquinaria.Reporte;
using Core.Entity.Maquinaria.Reporte;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Reporte
{
    public class RptIndicadorDAO : GenericDAO<tblM_RptIndicador>, IRptIndicadorDAO
    {
        public void SaveReporte(tblM_RptIndicador obj)
        {
            try
            {
                if (obj.id == 0)
                {
                    SaveEntity(obj, (int)BitacoraEnum.RptIndicador);
                }
                else
                {
                    Update(obj, obj.id ,(int)BitacoraEnum.RptIndicador);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public tblM_RptIndicador getReporte(int tipo, DateTime fechaInicio, DateTime fechaFin, string cc)
        {
            var obj = _context.tblM_RptIndicador.Where(x => x.Tipo == tipo && x.FechaInicio >= fechaInicio && x.FechaFin <= fechaFin && x.CC == cc)
                .FirstOrDefault();
            if (obj == null)
            {
                return new tblM_RptIndicador();
            }
            else
            {
                return obj;    
            }
        }
    }
}
