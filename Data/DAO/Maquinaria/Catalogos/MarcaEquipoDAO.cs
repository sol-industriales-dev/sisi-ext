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
using System.Data.Entity;

namespace Data.DAO.Maquinaria.Catalogos
{
    public class MarcaEquipoDAO : GenericDAO<tblM_CatMarcaEquipo>, IMarcaEquipoDAO
    {
        public List<MarcaDTO> FillGridMarcaEquipo(tblM_CatMarcaEquipo obj)
        {
            var grupoMaquina = _context.tblM_CatMarcaEquipotblM_CatGrupoMaquinaria.Where(x => obj.id != 0 ? x.tblM_CatGrupoMaquinaria_id == obj.id : true).ToList();
            var idMarca = grupoMaquina.Select(x => x.tblM_CatMarcaEquipo_id).ToList();
            var idGrupo = grupoMaquina.Select(x => x.tblM_CatGrupoMaquinaria_id).ToList();

            var grupos = _context.tblM_CatGrupoMaquinaria.Where(x => obj.id != 0 ? x.id == obj.id : true).ToList();
            var marcas = _context.tblM_CatMarcaEquipo.Where(x => !string.IsNullOrEmpty(obj.descripcion) ? x.descripcion.Contains(obj.descripcion) : idMarca.Contains(x.id) && x.estatus == obj.estatus).ToList();

            var result = new List<MarcaDTO>();
            foreach (var item in marcas)
            {
                var gpoId = grupoMaquina.FirstOrDefault(x => x.tblM_CatMarcaEquipo_id == item.id);
                if (gpoId != null)
                {
                    var gpo = grupos.FirstOrDefault(x => x.id == gpoId.tblM_CatGrupoMaquinaria_id);
                    if (gpo != null)
                    {
                        var mar = new MarcaDTO();
                        mar.id = item.id;
                        mar.descripcion = item.descripcion;
                        mar.estatus = item.estatus;
                        mar.grupoEquipoID = gpo.id;
                        mar.grupo = gpo.descripcion;
                        result.Add(mar);
                    }
                }
            }


            //var relacionMarca = _context.tblM_CatMarcaEquipotblM_CatGrupoMaquinaria.Where(x => x.tblM_CatMarcaEquipo_id == obj.id).Select(x => x.tblM_CatGrupoMaquinaria_id).ToList();
            //var grupos = _context.tblM_CatGrupoMaquinaria.Where(x => relacionMarca.Contains(x.id)).ToList();
            //var catGrupo = _context.tblM_CatMarcaEquipotblM_CatGrupoMaquinaria.ToList();

            //var result1 = _context.tblM_CatMarcaEquipo.Where(x =>
            //    (string.IsNullOrEmpty(obj.descripcion) == true ? x.descripcion == x.descripcion : x.descripcion.Contains(obj.descripcion)) &&
            //    x.estatus == obj.estatus).ToList();

            //var result = new List<MarcaDTO>();
            //foreach (var item in result1)
            //{
            //    var idGrupo = catGrupo.FirstOrDefault(x => x.tblM_CatMarcaEquipo_id == item.id);
            //    if (idGrupo != null)
            //    {
            //        var grupo = grupos.FirstOrDefault(x => x.id == idGrupo.tblM_CatGrupoMaquinaria_id);
            //        if (grupo != null)
            //        {
            //            var mar = new MarcaDTO();
            //            mar.id = item.id;
            //            mar.descripcion = item.descripcion;
            //            mar.estatus = item.estatus;
            //            mar.grupoEquipoID = grupo.id;
            //            mar.grupo = grupo.descripcion;
            //            result.Add(mar);
            //        }
            //    }

            //}

            //var result = (from m in _context.tblM_CatMarcaEquipo
            //              from g in grupos
            //              where (string.IsNullOrEmpty(obj.descripcion) == true ? m.descripcion == m.descripcion : m.descripcion.Contains(obj.descripcion)) &&
            //                     obj.estatus == m.estatus //&& (m.grupo.FirstOrDefault(x => x.id == g.id).id > 0)
            //              select new MarcaDTO { id = m.id, descripcion = m.descripcion, estatus = m.estatus, grupoEquipoID = g.id, grupo = g.descripcion }).ToList();

            return result;
        }

        public List<tblM_CatGrupoMaquinaria> FillCboGrupoMaquinaria(bool estatus)
        {
            return _context.tblM_CatGrupoMaquinaria.Where(x => x.estatus == estatus).ToList();
        }

        public void Guardar(tblM_CatMarcaEquipo obj, tblM_CatGrupoMaquinaria entidad)
        {

            if (!Exists(obj))
            {
                if (obj.id == 0)
                {
                    //var temp = _context.tblM_CatGrupoMaquinaria.FirstOrDefault(x => x.id == entidad.id);
                    //var relacionMarcas = _context.tblM_CatMarcaEquipotblM_CatGrupoMaquinaria.Where(x => x.tblM_CatGrupoMaquinaria_id == temp.id).Select(x => x.tblM_CatMarcaEquipo_id).ToList();
                    //var marcas = _context.tblM_CatMarcaEquipo.Where(x => relacionMarcas.Contains(x.id)).ToList();
                    //temp.marca.AddRange(marcas);
                    tblM_CatMarcaEquipotblM_CatGrupoMaquinaria auxRelacion = new tblM_CatMarcaEquipotblM_CatGrupoMaquinaria {
                        id = 0,
                        isActivo = true,
                        tblM_CatGrupoMaquinaria_id = entidad.id,
                        tblM_CatMarcaEquipo_id = obj.id
                    };
                    _context.tblM_CatMarcaEquipotblM_CatGrupoMaquinaria.Add(auxRelacion);
                    //_context.tblM_CatGrupoMaquinaria.Attach(obj.grupo);
                    _objectSet.AddObject(obj);
                    SaveChanges();
                    //SaveEntity(obj, (int)BitacoraEnum.MARCA);
                }
                else
                    Update(obj, obj.id, (int)BitacoraEnum.MARCA);
            }
            else
            {
                throw new Exception("Ya existe una marca con esa descripción");
            }
        }

        public bool Exists(tblM_CatMarcaEquipo obj)
        {
            return _context.tblM_CatTipoMaquinaria.Where(x => x.descripcion == obj.descripcion &&
                                        x.id != obj.id).ToList().Count > 0 ? true : false;
        }

        public tblM_CatGrupoMaquinaria getEntidadGrupo(int idGrupo)
        {
            return _context.tblM_CatGrupoMaquinaria.FirstOrDefault(x => x.id == idGrupo);
        }
        public List<tblM_CatMarcaEquipo> GetLstMarcaActivas()
        {
            return (from marca in _context.tblM_CatMarcaEquipo.AsQueryable()
                    where marca.estatus
                    select marca).ToList();
        }
        public List<tblM_CatGrupoMaquinaria> GetGruposByMarca(int marcaID)
        {           
            try
            {
                List<tblM_CatGrupoMaquinaria> grupos = new List<tblM_CatGrupoMaquinaria>();
                var relacionMarca = _context.tblM_CatMarcaEquipotblM_CatGrupoMaquinaria.Where(x => x.tblM_CatMarcaEquipo_id == marcaID).Select(x => x.tblM_CatGrupoMaquinaria_id).ToList();
                grupos = _context.tblM_CatGrupoMaquinaria.Where(x => relacionMarca.Contains(x.id)).ToList();
                return grupos;
            }
            catch (Exception e) {
                return new List<tblM_CatGrupoMaquinaria>();
            }
        }
    }
}
