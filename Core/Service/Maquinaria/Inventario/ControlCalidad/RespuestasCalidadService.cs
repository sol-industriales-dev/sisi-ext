using Core.DAO.Maquinaria.Inventario.ControlCalidad;
using Core.Entity.Maquinaria.Inventario.ControlCalidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Inventario.ControlCalidad
{
    public class RespuestasCalidadService : IRespuestasCalidadDAO
    {
        #region Atributos
        private IRespuestasCalidadDAO m_RespuestasCalidadDAO;
        #endregion
        #region Propiedades
        public IRespuestasCalidadDAO RespuestasCalidadDAO
        {
            get { return m_RespuestasCalidadDAO; }
            set { m_RespuestasCalidadDAO = value; }
        }
        #endregion
        #region Constructores
        public RespuestasCalidadService(IRespuestasCalidadDAO respuestasCalidadDAO)
        {
            this.RespuestasCalidadDAO = respuestasCalidadDAO;
        }
        #endregion

        public void saveRespuestasCalidad(List<tblM_RelPreguntaControlCalidad> lstObj)
        {
            RespuestasCalidadDAO.saveRespuestasCalidad(lstObj);
        }


        public List<tblM_RelPreguntaControlCalidad> getListRespuestasCalidad(int idCalidad)
        {
            return RespuestasCalidadDAO.getListRespuestasCalidad(idCalidad);
        }
    }
}
