using Core.DAO.Maquinaria.Captura;
using Core.DTO.Captura;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Capturas
{
    public class CapturaDesfaseService : ICapturaDesfaseDAO
    {
        #region Atributos
        private ICapturaDesfaseDAO m_CapturaDesfaseDAO;
        #endregion
        #region Propiedades
        public ICapturaDesfaseDAO CapturaDesfaseDAO
        {
            get { return m_CapturaDesfaseDAO; }
            set { m_CapturaDesfaseDAO = value; }
        }
        #endregion
        #region Constructores
        public CapturaDesfaseService(ICapturaDesfaseDAO capturaDesfaseDAO)
        {
            this.CapturaDesfaseDAO = capturaDesfaseDAO;
        }
        #endregion

        public IList<economicoDTO> getEconomicos(int cc)
        {
            return CapturaDesfaseDAO.getEconomicos(cc);
        }
        public void Guardar(tblM_CapDesfase obj)
        {
            CapturaDesfaseDAO.Guardar(obj);
        }

        public tblM_CapDesfase getDesfase(string economico)
        {
            return CapturaDesfaseDAO.getDesfase(economico);
        }


    }
}
