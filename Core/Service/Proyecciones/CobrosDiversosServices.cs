using Core.DAO.Proyecciones;
using Core.DTO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Proyecciones
{
    public class CobrosDiversosServices : ICobrosDiversosDAO
    {
        #region Atributos
        private ICobrosDiversosDAO m_interfazDAO;
        #endregion

        #region Propiedades
        private ICobrosDiversosDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion

        #region Constructores
        public CobrosDiversosServices(ICobrosDiversosDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion

        public tblPro_CobrosDiversos GetJsonData(FiltrosGeneralDTO filtro)
        {
            return interfazDAO.GetJsonData(filtro);
        }
        public void GuardarActualizarCobrosDiversos(FiltrosGeneralDTO objFiltro, CobrosDivDTO obj)
        {
            interfazDAO.GuardarActualizarCobrosDiversos(objFiltro,obj);
        }
    }
}
