using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Catalogos
{
    public interface ISubConjuntoDAO
    {
        void Guardar(tblM_CatSubConjunto obj);
        List<tblM_CatSubConjunto> FillGridSubConjunto(tblM_CatSubConjunto obj);
        List<tblM_CatConjunto> FillCboConjuntos(bool estatus);
    }
}
