using Core.DAO.Administracion.ControlInterno;
using Core.DTO.Administracion.ControlInterno.Obra;
using Core.Entity.Administrativo.ControlInterno.Obra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Administracion.ControlInterno
{
    public class ObraService : IObraDAO
    {
        #region Atributos
        private IObraDAO m_interfazDAO;
        #endregion
        #region Propiedades
        private IObraDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion
        #region Constructores
        public ObraService(IObraDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion
        #region Gestion
        /// <summary>
        /// Obras de Sigoplan
        /// </summary>
        public List<tblM_O_CatCCAC> getAllObra()
        {
            return m_interfazDAO.getAllObra();
        }
        public List<tblM_O_CatCCAC> getLstObra(BusqObraGestionDTO busq)
        {
            return m_interfazDAO.getLstObra(busq);
        }
        #endregion
        #region _formObra
        public bool GuardarObra(List<tblM_O_CatCCAC> lst)
        {
            return m_interfazDAO.GuardarObra(lst);
        }
        public tblM_O_CatCCAC getFormDesdeClave(string clave)
        {
            return m_interfazDAO.getFormDesdeClave(clave);
        }
        public List<tblM_O_CatCCAC> getAutocompleteClave(string term)
        {
            return m_interfazDAO.getAutocompleteClave(term);
        }
        #endregion
        #region Combobox

        #endregion
    }
}
