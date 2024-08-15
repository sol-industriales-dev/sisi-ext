using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Proyecciones
{
    public interface ICxCDAO
    {
        tblPro_CxC GetJsonData(int escenario, int meses, int anio);
        int getUltimoMesCapturado();
        void Guardar(tblPro_CxC obj);
    }
}
