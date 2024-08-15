using Core.DAO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Proyecciones
{
    public class CxCServices : ICxCDAO
    {
        #region Atributos
        private ICxCDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private ICxCDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public CxCServices(ICxCDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores

        public tblPro_CxC GetJsonData(int escenario, int meses, int anio)
        {
            return interfazDAO.GetJsonData(escenario, meses, anio);
        }
        public void Guardar(tblPro_CxC obj)
        {
            interfazDAO.Guardar(obj);
        }
        public int getUltimoMesCapturado()
        {
           return interfazDAO.getUltimoMesCapturado();
        }
    }
}
