using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Proyecciones
{
    public interface IObraDAO
    {
        List<tblPro_Obras> getObras(int tipo);

        void GuardarRegistros(tblPro_Obras obj);

        void GuardarActualizarRegistroMensual(List<tblPro_Obras> obj);
    }
}
