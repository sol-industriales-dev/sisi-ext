using Core.DTO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.Maquinaria.Catalogos
{
    public interface IModeloEquipoDAO
    {

        tblM_CatModeloEquipo getModeloByID(int id);
        void Guardar(tblM_CatModeloEquipo obj);
        List<ModeloEquipoDTO> FillGridModeloEquipo(tblM_CatModeloEquipo obj, string grupoDesc);
        
        List<tblM_CatMarcaEquipo> FillCboMarcaEquipo(bool estatus);
        List<tblM_CatModeloEquipo> GetLstModeloActivos();
        List<tblM_CatGrupoMaquinaria> fillGrupoMaquinaria(bool estatus);

        List<tblM_CatModeloEquipo> FillCboModelo(int idGrupo);
        void SubirArchivos(tblM_CatModeloEquipo obj, HttpPostedFileBase file);
        void SaveArchivo(HttpPostedFileBase archivo, string ruta);
        tblM_CatModeloEquipo LoadArchivos(int id);
        void GuardarSubconjuntos(List<int> listaSubConjuntos, List<string> listaNumParte, int idModelo);

    }
}
