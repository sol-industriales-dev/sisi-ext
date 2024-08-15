using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Catalogos
{
    public interface ITiposAceitesDAO
    {
        void Guardar(tblM_CatTiposAceites obj);

        List<tblM_CatTiposAceites> GetListaAceites(string descripcion, bool estatus);


    }
}
