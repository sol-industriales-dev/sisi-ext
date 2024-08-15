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
    public class SubConjuntoDAO : GenericDAO<tblM_CatSubConjunto>, ISubConjuntoDAO
    {
        public void Guardar(tblM_CatSubConjunto obj)
        {
            //if (!Exists(obj))
            //{
            if (obj.posicionID != null && obj.posicionID != "0") obj.hasPosicion = true;    
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.SUBCONJUNTO);
            else
                Update(obj, obj.id, (int)BitacoraEnum.SUBCONJUNTO);
            //}
            //else
            //{
            //    throw new Exception("Ya existe un SubConjunto con esa descripción seleccionada");
            //}
        }
        public bool Exists(tblM_CatSubConjunto obj)
        {

            if (obj.id != 0)
            {
                return _context.tblM_CatSubConjunto.Where(x => x.descripcion == obj.descripcion && x.id != obj.id).ToList().Count > 0 ? true : false;
            }
            else
            {
                return false;
            }
        }
        public List<tblM_CatSubConjunto> FillGridSubConjunto(tblM_CatSubConjunto obj)
        {

            var result = (from sc in _context.tblM_CatSubConjunto
                          where (string.IsNullOrEmpty(obj.descripcion) == true ? sc.descripcion == sc.descripcion : sc.descripcion.Contains(obj.descripcion)) &&
                                (obj.conjuntoID == 0 ? sc.conjuntoID == sc.conjuntoID : sc.conjuntoID == obj.conjuntoID) &&
                                sc.estatus == obj.estatus && sc.conjunto.estatus == true

                          select sc).ToList();
            return result;
        }
        public List<tblM_CatConjunto> FillCboConjuntos(bool estatus)
        {
            return _context.tblM_CatConjunto.Where(x => x.estatus == estatus).ToList();
        }


    }
}
