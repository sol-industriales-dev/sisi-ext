using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Proyecciones
{
    public interface IcatAreasDAO
    {
        void Guardar(tblPro_CatAreas obj);
        List<tblPro_CatAreas> FillCboArea();
        string getAreaByID(int obj);
    }
}
