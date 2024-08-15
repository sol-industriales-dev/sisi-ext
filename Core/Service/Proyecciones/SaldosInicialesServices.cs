using Core.DAO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Proyecciones
{
    public class SaldosInicialesServices: ISaldosInicialesDAO
    {
                
        #region Atributos
        private ISaldosInicialesDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private ISaldosInicialesDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public SaldosInicialesServices(ISaldosInicialesDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores

        public tblPro_SaldosIniciales GetJsonData(int mes, int anio, int estatus)
        {
            return interfazDAO.GetJsonData(mes, anio, estatus);
        }
        public void Guardar(tblPro_SaldosIniciales obj)
        {
            interfazDAO.Guardar(obj);
        }

        public int getUltimoMesCapturado()
        {
            return interfazDAO.getUltimoMesCapturado();
        }
    }
}
