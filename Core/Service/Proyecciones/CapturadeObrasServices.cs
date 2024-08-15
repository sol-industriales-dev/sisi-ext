using Core.DAO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Proyecciones
{
    public class CapturadeObrasServices : ICapturadeObrasDAO
    {

        #region Atributos
        private ICapturadeObrasDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private ICapturadeObrasDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public CapturadeObrasServices(ICapturadeObrasDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores

        public tblPro_CapturadeObras GetJsonData(int escenario, int meses, int anio)
        {
            return interfazDAO.GetJsonData(escenario, meses, anio);
        }
        public void GuardarActualizarCapturadeObras(tblPro_CapturadeObras obj)
        {
            interfazDAO.GuardarActualizarCapturadeObras(obj);
        }
        public Dictionary<string, object> getinfoCapturaObras(int Escenario, decimal divisor, int mes, int anio)
        {
            return interfazDAO.getinfoCapturaObras(Escenario, divisor, mes, anio);
        }
        public List<tblPro_Obras> dataEscenarios(List<tblPro_Obras> listas, int escenario)
        {
            return interfazDAO.dataEscenarios(listas, escenario);
        }

        public int getUltimoMesCapturado()
        {
            return interfazDAO.getUltimoMesCapturado();
        }

        public List<tblPro_CapturadeObras> FillCboObra()
        {
            return interfazDAO.FillCboObra();
        }
        public tblPro_CapturadeObras GetJsonDataID(int idData)
        {
            return interfazDAO.GetJsonDataID(idData);
        }
    }
}
