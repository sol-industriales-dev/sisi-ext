using Core.DTO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Catalogos
{
    public interface IGrupoMaquinariaDAO
    {
        List<tblM_CatTipoMaquinaria> FillCboTipoMaquinaria(bool estatus);
        List<tblM_CatGrupoMaquinaria> FillGridGrupoMaquinaria(tblM_CatGrupoMaquinaria grupoMaquinaria);
        void Guardar(tblM_CatGrupoMaquinaria obj);
        List<tblM_CatGrupoMaquinaria> FillCboGrupoMaquinaria(int idTipo);

        tblM_CatGrupoMaquinaria getDataGrupo(int idGrupo);
    }
}
