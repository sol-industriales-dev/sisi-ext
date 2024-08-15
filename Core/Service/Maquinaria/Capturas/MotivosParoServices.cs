using Core.DAO.Maquinaria.Captura;
using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Capturas
{
    public class MotivosParoServices :IMotivosParoDAO
    {
                #region Atributos
        private IMotivosParoDAO m_interfazDAO;
        #endregion

        #region Propiedades
        private IMotivosParoDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion

        #region Constructores
        public MotivosParoServices(IMotivosParoDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion

        public List<tblM_CatCriteriosCausaParo> cboMotivosParo()
        {
            return interfazDAO.cboMotivosParo();
        }
        public tblM_CatCriteriosCausaParo getMotivosParo(int id)
        {
            return interfazDAO.getMotivosParo(id);
        }
    }
}
