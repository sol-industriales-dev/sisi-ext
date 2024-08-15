using Core.DAO.Maquinaria.Catalogos;
using System;
using System.Collections.Generic;
using Core.DTO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria
{
    public class GrupoMaquinariaService :  IGrupoMaquinariaDAO
    {
        #region Atributos
        private IGrupoMaquinariaDAO m_grupoMaquinariaDAO;
        #endregion
        #region Propiedades
        public IGrupoMaquinariaDAO GrupoMaquinariaDAO
        {
            get { return m_grupoMaquinariaDAO; }
            set { m_grupoMaquinariaDAO = value; }
        }
        #endregion
        #region Constructores
        public GrupoMaquinariaService(IGrupoMaquinariaDAO grupoMaquinariaDAO)
        {
            this.GrupoMaquinariaDAO = grupoMaquinariaDAO;
        }
        #endregion

        public List<tblM_CatTipoMaquinaria> FillCboTipoMaquinaria(bool estatus)
        {
            return GrupoMaquinariaDAO.FillCboTipoMaquinaria(estatus);
        }
        public List<tblM_CatGrupoMaquinaria> FillGridGrupoMaquinaria(tblM_CatGrupoMaquinaria obj)
        {
            return GrupoMaquinariaDAO.FillGridGrupoMaquinaria(obj);
        }
        public void Guardar(tblM_CatGrupoMaquinaria obj)
        {
            GrupoMaquinariaDAO.Guardar(obj);
        }
        public List<tblM_CatGrupoMaquinaria> FillCboGrupoMaquinaria(int idTipo)
        {
            return GrupoMaquinariaDAO.FillCboGrupoMaquinaria(idTipo);
        }
        public tblM_CatGrupoMaquinaria getDataGrupo(int idGrupo)
        {
            return GrupoMaquinariaDAO.getDataGrupo(idGrupo);
        }

    }
}
