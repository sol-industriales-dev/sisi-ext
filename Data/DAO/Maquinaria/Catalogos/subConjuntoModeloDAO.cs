using Core.DAO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Overhaul;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Catalogos
{
    public class subConjuntoModeloDAO : GenericDAO<tblM_SubConjuntoModelo>, IsubConjuntoModeloDAO
    {
        public void Guardar(tblM_SubConjuntoModelo obj)
        {
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.COMPONENTESXMODELO);
            else
                Update(obj, obj.id, (int)BitacoraEnum.COMPONENTESXMODELO);
        }

        public List<tblM_SubConjuntoModelo> getDataSubConjuntoModelo(int idModelo)
        {
            var data = _context.tblM_SubConjuntoModelo.Where(x => x.modeloID == idModelo).ToList();

            return data;
        }

        public List<tblM_CatConjunto> FillCboConjunto()
        {
            var data = _context.tblM_CatConjunto.ToList();
            return data;
        }

        public List<tblM_CatSubConjunto> FillCboSubConjunto(int idConjunto) 
        {
            var data = _context.tblM_CatSubConjunto.Where(x => x.conjuntoID == idConjunto && x.estatus == true).ToList();
            return data;
        }

        public List<tblM_CatModeloEquipotblM_CatSubConjunto> FillGridSubConjunto(int idModelo) 
        {
            var data = _context.tblM_CatModeloEquipotblM_CatSubConjunto.Where(x => x.modeloID == idModelo).ToList();
            return data;
        } 
    }
}
