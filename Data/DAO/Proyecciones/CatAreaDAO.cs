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
    public class CatAreaDAO : GenericDAO<tblPro_CatAreas>, IcatAreasDAO
    {

        public void Guardar(tblPro_CatAreas obj)
        {
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.ALTADEPARTAMENTOS);
            else
                Update(obj, obj.id, (int)BitacoraEnum.ALTADEPARTAMENTOS);
        }

        public List<tblPro_CatAreas> FillCboArea()
        {
            return _context.tblPro_CatAreas.ToList();
        }

        public string getAreaByID(int obj)
        {
            var res = _context.tblPro_CatAreas.FirstOrDefault(x => x.id.Equals(obj)).descripcion;

            return res;

        }
    }
}
