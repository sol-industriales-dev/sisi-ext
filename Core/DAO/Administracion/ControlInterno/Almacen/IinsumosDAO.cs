using Core.DTO.Administracion;
using Core.DTO.Administracion.ControlInterno;
using Core.DTO.Principal.Generales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Administracion.ControlInterno.Almacen
{
    public interface IinsumosDAO
    {
        List<insumosDTO> getListaInsumo(int insumoID, int anio);
        List<detInsumoDTO> getInsumo(string term, int TipoInsumo, int GrupoInsumo, int sistema);
        List<detInsumoDTO> getListaInsumos(string term, int TipoInsumo, int GrupoInsumo, int sistema);
        List<ComboDTO> fillTipoInsumos(int sistema);

        List<ComboDTO> fillGrupoInsumos(int tipo,int sistema);

        List<insumosDTO> getInsumo(int insumo, int anio, int tipo);
        List<insumosDTO> getInsumoMultiple(List<int> insumo, int anio, int tipo,string almacen);

        ComboDTO getInsumoTipoGrupoByID(int idInsumo);
    }
}
