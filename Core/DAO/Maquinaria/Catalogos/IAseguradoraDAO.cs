using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Catalogos
{
    public interface IAseguradoraDAO
    {
        void Guardar(tblM_CatAseguradora obj);

        List<tblM_CatAseguradora> FillGridAseguradora(tblM_CatAseguradora obj);

    }
}
