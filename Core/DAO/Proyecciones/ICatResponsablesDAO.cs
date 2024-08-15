using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Proyecciones
{
    public interface ICatResponsablesDAO
    {
        tblPro_CatResponsables GetDataById(int id);
        void Guardar(tblPro_CatResponsables obj);
        List<tblPro_CatResponsables> fillCboResponsables();
    }
}
