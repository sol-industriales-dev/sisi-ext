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
    public class AdministracionServices : IAdministracionDAO
    {
        #region Atributos
        private IAdministracionDAO m_interfazDAO;
        #endregion

        #region Propiedades
        private IAdministracionDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion

        #region Constructores
        public AdministracionServices(IAdministracionDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion

        public tblPro_Administracion GetJsonData(FiltrosGeneralDTO filtro)
        {
            return interfazDAO.GetJsonData(filtro);
        }
        public void GuardarActualizarAdministracion(FiltrosGeneralDTO objFiltro, AdministracionDTO obj)
        {
            interfazDAO.GuardarActualizarAdministracion(objFiltro,obj);
        }
    }
}
