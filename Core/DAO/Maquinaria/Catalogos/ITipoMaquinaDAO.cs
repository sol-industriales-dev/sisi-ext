using Core.DTO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Catalogos
{
    public interface ITipoMaquinaDAO
    {
        void Guardar(tblM_CatTipoMaquinaria obj);
        List<tblM_CatTipoMaquinaria> FillGridTipoMaquinaria(tblM_CatTipoMaquinaria tipoMaquinaria);

    }
}
