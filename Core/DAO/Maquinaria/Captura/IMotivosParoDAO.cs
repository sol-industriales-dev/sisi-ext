using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Captura
{
    public interface IMotivosParoDAO
    {
        List<tblM_CatCriteriosCausaParo> cboMotivosParo();
        tblM_CatCriteriosCausaParo getMotivosParo(int id);
    }
}
