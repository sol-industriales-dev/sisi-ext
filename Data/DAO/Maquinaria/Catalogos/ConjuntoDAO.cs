using Core.DAO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Catalogos
{
    public class ConjuntoDAO : GenericDAO<tblM_CatConjunto>, IConjuntoDAO
    {
        public void Guardar(tblM_CatConjunto obj)
        {
            if (!Exists(obj))
            {
                if (obj.id == 0)
                    SaveEntity(obj, (int)BitacoraEnum.CONJUNTO);
                else
                    Update(obj, obj.id, (int)BitacoraEnum.CONJUNTO);
            }
            else
            {
                throw new Exception("Ya existe un conjunto con esa descripción seleccionada");
            }
        }
        public bool Exists(tblM_CatConjunto obj)
        {
            return _context.tblM_CatConjunto.Where(x => x.descripcion == obj.descripcion &&
                                        x.id != obj.id).ToList().Count > 0 ? true : false;
        }

        public List<tblM_CatConjunto> FillGridConjunto(tblM_CatConjunto obj)
        {
            var result = (from c in _context.tblM_CatConjunto
                          where (string.IsNullOrEmpty(obj.descripcion) == true ? c.descripcion == c.descripcion : c.descripcion.Contains(obj.descripcion))
                          && c.estatus == obj.estatus
                          select c).ToList();
            return result;
        }
    }
}
