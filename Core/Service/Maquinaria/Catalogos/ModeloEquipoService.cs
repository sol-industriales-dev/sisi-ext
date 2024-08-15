using Core.DAO.Maquinaria.Catalogos;
using Core.DTO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.Maquinaria.Catalogos
{
    public class ModeloEquipoService : IModeloEquipoDAO
    {
        #region Atributos
        private IModeloEquipoDAO m_modeloEquipoDAO;
        #endregion
        #region Propiedades
        public IModeloEquipoDAO ModeloEquipoDAO
        {
            get { return m_modeloEquipoDAO; }
            set { m_modeloEquipoDAO = value; }
        }
        #endregion
        #region Constructores
        public ModeloEquipoService(IModeloEquipoDAO modeloEquipoDAO)
        {
            this.ModeloEquipoDAO = modeloEquipoDAO;
        }
        #endregion
        public void Guardar(tblM_CatModeloEquipo obj)
        {
            ModeloEquipoDAO.Guardar(obj);
        }
        public List<ModeloEquipoDTO> FillGridModeloEquipo(tblM_CatModeloEquipo modeloEquipo, string grupoDesc)
        {
            return ModeloEquipoDAO.FillGridModeloEquipo(modeloEquipo, grupoDesc);
        }

        public  tblM_CatModeloEquipo getModeloByID(int id)
        {
            return ModeloEquipoDAO.getModeloByID(id);

        }
        public List<tblM_CatMarcaEquipo> FillCboMarcaEquipo(bool estatus)
        {
            return ModeloEquipoDAO.FillCboMarcaEquipo(estatus);
        }
        public List<tblM_CatModeloEquipo> GetLstModeloActivos()
        {
            return ModeloEquipoDAO.GetLstModeloActivos();
        }
        public List<tblM_CatGrupoMaquinaria> fillGrupoMaquinaria(bool estatus)
        {
            return ModeloEquipoDAO.fillGrupoMaquinaria(estatus);
        }

        public List<tblM_CatModeloEquipo> FillCboModelo(int idGrupo)
        {
            return ModeloEquipoDAO.FillCboModelo(idGrupo);
        }

        public void SubirArchivos(tblM_CatModeloEquipo obj, HttpPostedFileBase file)
        {
            ModeloEquipoDAO.SubirArchivos(obj, file);
        }
        public void SaveArchivo(HttpPostedFileBase archivo, string ruta)
        {
            ModeloEquipoDAO.SaveArchivo(archivo, ruta);
        }

        public tblM_CatModeloEquipo LoadArchivos(int id)
        {
            return ModeloEquipoDAO.LoadArchivos(id);
        }
        public void GuardarSubconjuntos(List<int> listaSubConjuntos, List<string> listaNumParte, int idModelo)
        {
            ModeloEquipoDAO.GuardarSubconjuntos(listaSubConjuntos, listaNumParte, idModelo);
        }

    }
}
