using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Proyecciones
{
    public interface ISaldosInicialesDAO
    {
        tblPro_SaldosIniciales GetJsonData(int mes, int anio, int estatus);
      //  tblPro_SaldosIniciales ObtenerRegistroById();
        void Guardar(tblPro_SaldosIniciales obj);

        int getUltimoMesCapturado();
    }
}
