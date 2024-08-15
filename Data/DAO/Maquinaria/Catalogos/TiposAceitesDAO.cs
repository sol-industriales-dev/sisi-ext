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
    public class TiposAceitesDAO : GenericDAO<tblM_CatTiposAceites>, ITiposAceitesDAO
    {
        public void Guardar(tblM_CatTiposAceites obj)
        {
            if (!Exists(obj))
            {
                if (obj.id == 0)
                    SaveEntity(obj, (int)BitacoraEnum.TIPOACEITES);
                else
                    Update(obj, obj.id, (int)BitacoraEnum.TIPOACEITES);
            }
            else
            {
                throw new Exception("Ya existe una Aseguradora con esa descripción seleccionada");
            }
        }

        public bool Exists(tblM_CatTiposAceites obj)
        {
            return _context.tblM_CatTiposAceites.Where(x => x.descripcion == obj.descripcion &&
                                        x.id != obj.id).ToList().Count > 0 ? true : false;
        }

        public List<tblM_CatTiposAceites> GetListaAceites(string descripcion, bool estatus)
        {
            return _context.tblM_CatTiposAceites.Where(a => (string.IsNullOrEmpty(descripcion) ? a.id == a.id : a.descripcion.Contains(descripcion)) && a.estatus == estatus).ToList();
        }

    }
}
