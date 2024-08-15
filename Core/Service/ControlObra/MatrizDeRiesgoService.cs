using Core.DAO.ControlObra;
using Core.DTO.ControlObra.MatrizDeRiesgo;
using Core.DTO.Principal.Generales;
using Core.Entity.ControlObra.MatrizDeRiesgo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.ControlObra
{
    public class MatrizDeRiesgoService : IMatrizDeRiesgoDAO
    {
        #region CONSTRUCTOR
        private IMatrizDeRiesgoDAO m_MatrizDeRiesgoDAO;
        public IMatrizDeRiesgoDAO MatrizDeRiesgoDAO
        {
            get { return m_MatrizDeRiesgoDAO; }
            set { m_MatrizDeRiesgoDAO = value; }
        }
        public MatrizDeRiesgoService(IMatrizDeRiesgoDAO MatrizDeRiesgoDAO)
        {
            this.MatrizDeRiesgoDAO = MatrizDeRiesgoDAO;
        }
        #endregion

        public Dictionary<string, object> obtenerMatrizesDeRiesgo(string variable)
        {
            return MatrizDeRiesgoDAO.obtenerMatrizesDeRiesgo(variable);
        }
        public MatrizPrinDTO obtenerMatrizesDeRiesgoxID(int idMatrizDeRiesgo, List<int> lstFiltro)
        {
            return MatrizDeRiesgoDAO.obtenerMatrizesDeRiesgoxID(idMatrizDeRiesgo, lstFiltro);
        }
        public Dictionary<string, object> GuardarEditarMatriz(MatrizDTO parametros, bool editar)
        {
            return MatrizDeRiesgoDAO.GuardarEditarMatriz(parametros, editar);
        }
        public List<ComboDTO> obtenerContratos()
        {
            return MatrizDeRiesgoDAO.obtenerContratos();
        }
        public List<ComboDTO> TraermeTodosLosCC()
        {
            return MatrizDeRiesgoDAO.TraermeTodosLosCC();
        }
        public List<ComboDTO> QuienElaboro(int idUsuario)
        {
            return MatrizDeRiesgoDAO.QuienElaboro(idUsuario);
        }
        public List<tblCO_MR_CategoriaDeRiesgo> lstMrCategorias()
        {
            return MatrizDeRiesgoDAO.lstMrCategorias();
        }
        public tblCO_MR_CategoriaDeRiesgo AgregarEditarCategoria(tblCO_MR_CategoriaDeRiesgo parametros)
        {
            return MatrizDeRiesgoDAO.AgregarEditarCategoria(parametros);
        }
        public tblCO_MR_CategoriaDeRiesgo EliminarCategoria(tblCO_MR_CategoriaDeRiesgo parametros)
        {
            return MatrizDeRiesgoDAO.EliminarCategoria(parametros);
        }
        public List<ComboDTO> cbolstMrCategorias()
        {
            return MatrizDeRiesgoDAO.cbolstMrCategorias();
        }
        public List<ComboDTO> cboTiposDeRespuestas(int idTipo)
        {
            return MatrizDeRiesgoDAO.cboTiposDeRespuestas(idTipo);
        }
        public List<ComboDTO> cboResponsables()
        {
            return MatrizDeRiesgoDAO.cboResponsables();
        }
        public List<TipoRespuestaDTO> lstMrTiposDeRespuestas()
        {
            return MatrizDeRiesgoDAO.lstMrTiposDeRespuestas();
        }
        public tblCO_MR_TipoDeRespuestas AgregarEditarTiposDeRespuestas(tblCO_MR_TipoDeRespuestas parametros)
        {
            return MatrizDeRiesgoDAO.AgregarEditarTiposDeRespuestas(parametros);
        }
        public tblCO_MR_TipoDeRespuestas EliminarTiposDeRespuestas(tblCO_MR_TipoDeRespuestas parametros)
        {
            return MatrizDeRiesgoDAO.EliminarTiposDeRespuestas(parametros);
        }

    }
}
