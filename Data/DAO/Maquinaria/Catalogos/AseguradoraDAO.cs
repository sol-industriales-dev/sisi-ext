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
    public class AseguradoraDAO : GenericDAO<tblM_CatAseguradora>, IAseguradoraDAO
    {
        public void Guardar(tblM_CatAseguradora obj)
        {
            if (!Exists(obj))
            {
                if (obj.id == 0)
                    SaveEntity(obj, (int)BitacoraEnum.ASEGURADORA);
                else
                    Update(obj, obj.id, (int)BitacoraEnum.ASEGURADORA);
            }
            else
            {
                throw new Exception("Ya existe una Aseguradora con esa descripción seleccionada");
            }
        }
        public bool Exists(tblM_CatAseguradora obj)
        {
            return _context.tblM_CatAseguradora.Where(x => x.descripcion == obj.descripcion &&
                                        x.id != obj.id).ToList().Count > 0 ? true : false;
        }

        public List<tblM_CatAseguradora> FillGridAseguradora(tblM_CatAseguradora obj)
        {
            var result = (from a in _context.tblM_CatAseguradora
                           where (string.IsNullOrEmpty(obj.descripcion) == true ? a.descripcion == a.descripcion : a.descripcion.Contains(obj.descripcion)) &&
                                 a.estatus == obj.estatus
                           select a).ToList();
            return result;
        }

    }
}
