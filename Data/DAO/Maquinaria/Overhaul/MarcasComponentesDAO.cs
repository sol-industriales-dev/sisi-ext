using Core.DAO.Maquinaria.Overhaul;
using Core.Entity.Maquinaria.Overhaul;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;
using Core.DTO.Maquinaria.Overhaul;
using Core.DTO.Principal.Generales;

namespace Data.DAO.Maquinaria.Overhaul
{
    public class MarcasComponentesDAO : GenericDAO<tblM_CatMarcasComponentes>, IMarcasComponentesDAO
    {

        public void Guardar(tblM_CatMarcasComponentes obj)
        {
            if (!Exists(obj))
            {
                if (obj.id == 0)
                    SaveEntity(obj, 120);
                else
                    Update(obj, obj.id, 120);
            }
            else
            {
                throw new Exception("Ya existe un conjunto con esa descripción seleccionada");
            }
        }

        private bool Exists(tblM_CatMarcasComponentes obj)
        {
            return _context.tblM_CatMarcasComponentes.Where(x => x.descripcion == obj.descripcion && x.estatus == obj.estatus).ToList().Count > 0 ? true : false;
        }

        public List<tblM_CatMarcasComponentes> getLocaciones(bool estatus, string descripcion)
        {
            if (descripcion == "") { return _context.tblM_CatMarcasComponentes.Where(x => x.estatus == estatus).ToList(); }
            else { return _context.tblM_CatMarcasComponentes.Where(x => x.estatus == estatus && x.descripcion.Contains(descripcion)).ToList(); }
        }

        public void eliminar(int idMarca)
        {
            if (ExistsByID(idMarca))
            {
                var aux = _context.tblM_CatMarcasComponentes.FirstOrDefault(x => x.id == idMarca);
                _context.tblM_CatMarcasComponentes.Remove(aux);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("No se encuenta el registro que desea eliminar");
            }
        }

        private bool ExistsByID(int idMarca)
        {
            return _context.tblM_CatMarcasComponentes.Where(x => x.id == idMarca).ToList().Count > 0 ? true : false;
        }

    }
}