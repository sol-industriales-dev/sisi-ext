using Core.DAO.Maquinaria.Inventario.ControlCalidad;
using Core.Entity.Maquinaria.Inventario.ControlCalidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Inventario.ControlCalidad
{
    public class GrupoPreguntasService : IGruposPreguntasCalidadDAO
    {
        #region Atributos
        private IGruposPreguntasCalidadDAO m_GruposPreguntasCalidadDAO;
        #endregion
        #region Propiedades
        public IGruposPreguntasCalidadDAO GruposPreguntasCalidadDAO
        {
            get { return m_GruposPreguntasCalidadDAO; }
            set { m_GruposPreguntasCalidadDAO = value; }
        }
        #endregion
        #region Constructores
        public GrupoPreguntasService(IGruposPreguntasCalidadDAO gruposPreguntasCalidadDAO)
        {
            this.GruposPreguntasCalidadDAO = gruposPreguntasCalidadDAO;
        }
        #endregion

        public List<tblM_CatGrupoPreguntasCalidad> getListGrupoPreguntas()
        {
            return GruposPreguntasCalidadDAO.getListGrupoPreguntas();
        }
    }
}
