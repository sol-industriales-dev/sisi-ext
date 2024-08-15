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
    public class ActivoFijoServices : IActivoFijoDAO
    {
        #region Atributos
        private IActivoFijoDAO m_interfazDAO;
        #endregion

        #region Propiedades
        private IActivoFijoDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion

        #region Constructores
        public ActivoFijoServices(IActivoFijoDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion

        public tblPro_ActivoFijo GetJsonData(FiltrosGeneralDTO objFiltro)
        {
            return interfazDAO.GetJsonData(objFiltro);
        }
        public void GuardarActualizarActivoFijo(tblPro_ActivoFijo obj)
        {
            interfazDAO.GuardarActualizarActivoFijo(obj);
        }
        public int getUltimoMesCapturado(int Mes)
        {
            return interfazDAO.getUltimoMesCapturado(Mes);
        }
    }
}
