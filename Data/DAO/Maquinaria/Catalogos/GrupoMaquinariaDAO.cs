using Core.DAO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using Data.EntityFramework.Generic;
using Core.DTO.Maquinaria.Catalogos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Principal.Bitacoras;

namespace Data.DAO.Maquinaria.Catalogos
{
    public class GrupoMaquinariaDAO : GenericDAO<tblM_CatGrupoMaquinaria>, IGrupoMaquinariaDAO
    {
        public List<tblM_CatTipoMaquinaria> FillCboTipoMaquinaria(bool estatus)
        {
            return _context.tblM_CatTipoMaquinaria.Where(x => x.estatus == estatus).OrderBy(x => x.descripcion).ToList(); ///item;
        }
        public List<tblM_CatGrupoMaquinaria> FillGridGrupoMaquinaria(tblM_CatGrupoMaquinaria obj)
        {
            var result = (from g in _context.tblM_CatGrupoMaquinaria
                          where (obj.tipoEquipoID == 0 ? g.tipoEquipoID == g.tipoEquipoID : obj.tipoEquipoID == g.tipoEquipoID) &&
                                (string.IsNullOrEmpty(obj.descripcion) == true ? g.descripcion == g.descripcion : g.descripcion.Contains(obj.descripcion)) &&
                                g.estatus == obj.estatus && g.tipoEquipo.estatus == true
                          select g).ToList();
            return result;
        }
        public void Guardar(tblM_CatGrupoMaquinaria obj)
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
                throw new Exception("Ya existe un grupo de maquinaria con esa descripción seleccionada");
            }
        }
        public bool Exists(tblM_CatGrupoMaquinaria obj)
        {
            return _context.tblM_CatGrupoMaquinaria.Where(x => x.descripcion == obj.descripcion &&
                                        x.id != obj.id).ToList().Count > 0 ? true : false;
        }

        public List<tblM_CatGrupoMaquinaria> FillCboGrupoMaquinaria(int idTipo)
        {
            return _context.tblM_CatGrupoMaquinaria.Where(x => x.estatus == true && (idTipo == 0 ? true : x.tipoEquipoID == idTipo)).OrderBy(x => x.descripcion).ToList();
        }

        public tblM_CatGrupoMaquinaria getDataGrupo(int idGrupo)
        {
            return _context.tblM_CatGrupoMaquinaria.FirstOrDefault(x => x.id == idGrupo);
        }


    }
}
