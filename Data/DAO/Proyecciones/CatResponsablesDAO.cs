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
    public class CatResponsablesDAO : GenericDAO<tblPro_CatResponsables>, ICatResponsablesDAO
    {

        public tblPro_CatResponsables GetDataById(int id)
        {
            return _context.tblPro_CatResponsables.FirstOrDefault();
        }


        public void Guardar(tblPro_CatResponsables obj)
        {
            if (!Exists(obj))
            {

                if (obj.id == 0)
                    SaveEntity(obj, (int)BitacoraEnum.CATRESPONSABLES);
                else
                    Update(obj, obj.id, (int)BitacoraEnum.CATRESPONSABLES);
            }
            else
            {
                if (obj.id == 0)
                    throw new Exception("No puede haber dos colores similares");
                else
                    Update(obj, obj.id, (int)BitacoraEnum.CATRESPONSABLES);
            }
        }

        public bool Exists(tblPro_CatResponsables obj)
        {
            if (obj.responsableID != null)
            {
                return _context.tblPro_CatResponsables.Where(x => x.Color == obj.Color &&
                                         x.Descripcion == obj.Descripcion).ToList().Count > 0 ? true : false;
            }
            return false;

        }

        public  List<tblPro_CatResponsables> fillCboResponsables()
        {
            return _context.tblPro_CatResponsables.ToList();
        }
    }
}
