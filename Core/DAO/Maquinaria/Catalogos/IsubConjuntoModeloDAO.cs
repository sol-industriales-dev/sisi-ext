using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Catalogos
{
    public interface IsubConjuntoModeloDAO
    {
        void Guardar(tblM_SubConjuntoModelo obj);

        List<tblM_SubConjuntoModelo> getDataSubConjuntoModelo(int idModelo);
        List<tblM_CatConjunto> FillCboConjunto();
        List<tblM_CatSubConjunto> FillCboSubConjunto(int idConjunto);
        List<tblM_CatModeloEquipotblM_CatSubConjunto> FillGridSubConjunto(int idModelo);
    }
}
