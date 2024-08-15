using Core.DAO.Maquinaria.Captura;
using Core.Entity.Maquinaria.Captura;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Captura
{
    public class MotivosParoDAO : GenericDAO<tblM_CatCriteriosCausaParo>, IMotivosParoDAO
    {
        public List<tblM_CatCriteriosCausaParo> cboMotivosParo()
        {
            return _context.tblM_CatCriteriosCausaParo.ToList();
        }


        public tblM_CatCriteriosCausaParo getMotivosParo(int id)
        {
            return _context.tblM_CatCriteriosCausaParo.Where(x=>x.id == id).FirstOrDefault();
        }


    }
}
