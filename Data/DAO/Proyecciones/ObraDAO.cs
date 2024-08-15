using Core.DAO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Proyecciones
{
    public class ObraDAO : GenericDAO<tblPro_Obras>, IObraDAO
    {
        public List<tblPro_Obras> getObras(int tipo)
        {
            return _context.tblPro_Obras.Where(x => x.Tipo.Equals(tipo)).ToList();
        }
        public void GuardarRegistros(tblPro_Obras obj)
        {
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.COMPONENTE);
            else
                Update(obj, obj.id, (int)BitacoraEnum.COMPONENTE);
        }

        public void GuardarActualizarRegistroMensual(List<tblPro_Obras> obj)
        {
            if (true)
            {
                saveEntitys(obj, (int)BitacoraEnum.HOROMETROSCAPTURA);
            }
            else
            {
                throw new Exception("");
            }

        }


    }
}
