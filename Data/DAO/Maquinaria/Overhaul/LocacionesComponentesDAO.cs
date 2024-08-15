using Core.DAO.Maquinaria.Overhaul;
using Core.Entity.Maquinaria.Overhaul;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Multiempresa;
using Newtonsoft.Json;

namespace Data.DAO.Maquinaria.Overhaul
{
    public class LocacionesComponentesDAO : GenericDAO<tblM_CatLocacionesComponentes>, ILocacionesComponentesDAO
    {
        public List<tblM_CatLocacionesComponentes> getLocaciones(bool estatus, string descripcion) 
        {
            return _context.tblM_CatLocacionesComponentes.Where(x => x.estatus == estatus && (descripcion == "" ? true : x.descripcion.Contains(descripcion)) && x.tipoLocacion != 3).OrderBy(x => x.tipoLocacion).ToList(); 
            
        }

        public void Guardar(tblM_CatLocacionesComponentes obj)
        {
            //if (!Exists(obj))
            //{
            if (obj.id == 0)
                SaveEntity(obj, 121);
            else
                Update(obj, obj.id, 121);
            //}
            //else
            //{
            //    throw new Exception("Ya existe un conjunto con esa descripción seleccionada");
            //}
        }

        public void eliminarLocacion(int idLocacion)
        {
            if (ExistsByID(idLocacion))
            {
                var aux = _context.tblM_CatLocacionesComponentes.FirstOrDefault(x => x.id == idLocacion);
                _context.tblM_CatLocacionesComponentes.Remove(aux);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("No se encuenta el registro que desea eliminar");
            }
        }

        //private bool Exists(tblM_CatLocacionesComponentes obj)
        //{
        //    return _context.tblM_CatLocacionesComponentes.Where(x => x.tipoLocacion == obj.tipoLocacion && x.descripcion == obj.descripcion).ToList().Count > 0 ? true : false;
        //}

        private bool ExistsByID(int idLocacion)
        {
            return _context.tblM_CatLocacionesComponentes.Where(x => x.id == idLocacion).ToList().Count > 0 ? true : false;
        }

        public List<tblP_CC> getCentrosCostos() 
        {
            return _context.tblP_CC.Where(x => x.estatus == true).ToList();
        }

        public List<string> GetCorreosLocacionesOverhaul(List<int> idLocaciones) 
        {
            var locaciones = _context.tblM_CatLocacionesComponentes.Where(x => idLocaciones.Contains(x.id));
            var correos = new List<string>();
            if (locaciones.Count() > 0) {
                foreach (var item in locaciones) 
                {
                    if (item.JsonCorreos != null)
                        correos.AddRange(JsonConvert.DeserializeObject<List<string>>(item.JsonCorreos)); 
                }                
            }
            return correos;
        }
    }
}