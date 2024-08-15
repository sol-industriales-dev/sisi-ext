using Core.DTO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Catalogos
{
    public interface IMarcaEquipoDAO
    {
        void Guardar(tblM_CatMarcaEquipo obj, tblM_CatGrupoMaquinaria entididad);
        List<MarcaDTO> FillGridMarcaEquipo(tblM_CatMarcaEquipo obj);
        List<tblM_CatGrupoMaquinaria> FillCboGrupoMaquinaria(bool estatus);
        tblM_CatGrupoMaquinaria getEntidadGrupo(int p);
        List<tblM_CatMarcaEquipo> GetLstMarcaActivas();
        List<tblM_CatGrupoMaquinaria> GetGruposByMarca(int marcaID);
    }
}
