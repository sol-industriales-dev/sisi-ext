using Data.EntityFramework.Generic;
using Core.Entity.Maquinaria.Catalogo;
using Core.DAO.Maquinaria.Catalogos;
using Core.DTO.Maquinaria.Catalogos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Principal.Bitacoras;

namespace Data.DAO.Maquinaria.Catalogos
{
    public class TipoMaquinariaDAO : GenericDAO<tblM_CatTipoMaquinaria>, ITipoMaquinaDAO
    {
        public List<tblM_CatTipoMaquinaria> FillGridTipoMaquinaria(tblM_CatTipoMaquinaria obj)
        {
            var result = (from t in _context.tblM_CatTipoMaquinaria
                         where (string.IsNullOrEmpty(obj.descripcion) == true ? t.descripcion == t.descripcion : t.descripcion.Contains(obj.descripcion)) && t.estatus == obj.estatus
                         select t).ToList();
            return result;
        }
        public void Guardar(tblM_CatTipoMaquinaria obj)
        {
            if (!Exists(obj))
            {
                if (obj.id == 0)
                    SaveEntity(obj, (int)BitacoraEnum.GRUPOMAQUINARIA);
                else
                    Update(obj, obj.id, (int)BitacoraEnum.GRUPOMAQUINARIA);
            }
            else
            {
                throw new Exception("Ya existe un tipo de maquinaria con esa descripción seleccionado");
            }
        }

        public bool Exists(tblM_CatTipoMaquinaria obj)
        {
            return _context.tblM_CatTipoMaquinaria.Where(x => x.descripcion == obj.descripcion &&
                                        x.id != obj.id).ToList().Count > 0 ? true : false;
        }
    }
}
