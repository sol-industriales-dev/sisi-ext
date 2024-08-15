using Core.DAO.Maquinaria.Captura;
using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Capturas
{
    public class ParosService : IParos
    {
        #region Atributos
        private IParos m_ParosDAO;
        #endregion
        #region Propiedades
        public IParos ParosDAO
        {
            get { return m_ParosDAO; }
            set { m_ParosDAO = value; }
        }
        #endregion
        #region Constructores
        #endregion
        public ParosService(IParos parosDAO)
        {
            this.ParosDAO = parosDAO;
        }

        public List<tblM_Paros> getParosMaquina(int obj)
        {
            return m_ParosDAO.getParosMaquina(obj);
        }
    }
}
