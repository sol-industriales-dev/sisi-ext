using Core.DAO.Maquinaria.Inventario.ControlCalidad;
using Core.Entity.Maquinaria.Inventario.ControlCalidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Inventario.ControlCalidad
{
    public class PreguntasCalidadServices : IPreguntasCalidadDAO
    {
        #region Atributos
        private IPreguntasCalidadDAO m_PreguntasCalidadDAO;
        #endregion
        #region Propiedades
        public IPreguntasCalidadDAO PreguntasCalidadDAO
        {
            get { return m_PreguntasCalidadDAO; }
            set { m_PreguntasCalidadDAO = value; }
        }
        #endregion
        #region Constructores
        public PreguntasCalidadServices(IPreguntasCalidadDAO preguntasCalidadDAO)
        {
            this.PreguntasCalidadDAO = preguntasCalidadDAO;
        }
        #endregion

        public List<tblM_CatPreguntasCalidad> getListPreguntasCalidad()
        {
            return PreguntasCalidadDAO.getListPreguntasCalidad();
        }
    }
}
