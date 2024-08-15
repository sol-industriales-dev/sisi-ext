using Core.DTO.ControlObra.MatrizDeRiesgo;
using Core.DTO.Principal.Generales;
using Core.Entity.ControlObra.MatrizDeRiesgo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.ControlObra
{
    public interface IMatrizDeRiesgoDAO
    {
        Dictionary<string, object> obtenerMatrizesDeRiesgo(string variable);
        MatrizPrinDTO obtenerMatrizesDeRiesgoxID(int idMatrizDeRiesgo, List<int> lstFiltro);
        Dictionary<string, object> GuardarEditarMatriz(MatrizDTO parametros, bool editar);
        List<ComboDTO> obtenerContratos();
        List<ComboDTO> TraermeTodosLosCC();
        List<ComboDTO> QuienElaboro(int idUsuario);
        List<tblCO_MR_CategoriaDeRiesgo> lstMrCategorias();
        tblCO_MR_CategoriaDeRiesgo AgregarEditarCategoria(tblCO_MR_CategoriaDeRiesgo parametros);
        tblCO_MR_CategoriaDeRiesgo EliminarCategoria(tblCO_MR_CategoriaDeRiesgo parametros);

        List<ComboDTO> cbolstMrCategorias();
        List<ComboDTO> cboTiposDeRespuestas(int idTipo);
        List<ComboDTO> cboResponsables();

        List<TipoRespuestaDTO> lstMrTiposDeRespuestas();
        tblCO_MR_TipoDeRespuestas AgregarEditarTiposDeRespuestas(tblCO_MR_TipoDeRespuestas parametros);
        tblCO_MR_TipoDeRespuestas EliminarTiposDeRespuestas(tblCO_MR_TipoDeRespuestas parametros);


    }
}
