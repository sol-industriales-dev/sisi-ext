using Core.DAO.Maquinaria.Reporte;
using Core.Entity.Maquinaria.Reporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Reporte
{
    public class RptIndicadorServices : IRptIndicadorDAO
    {
        #region Atributos
        private IRptIndicadorDAO m_rptIndicadorDAO;
        #endregion
        #region Propiedades
        public IRptIndicadorDAO RptIndicadorDAO
        {
            get { return m_rptIndicadorDAO; }
            set { m_rptIndicadorDAO = value; }
        }
        #endregion
        #region Constructores
        public RptIndicadorServices(IRptIndicadorDAO rptIndicador)
        {
            this.RptIndicadorDAO = rptIndicador;
        }
        #endregion
        public void SaveReporte(tblM_RptIndicador obj)
        {
            RptIndicadorDAO.SaveReporte(obj);
        }
        public tblM_RptIndicador getReporte(int tipo, DateTime fechaInicio, DateTime fechaFin, string cc)
        {
            return RptIndicadorDAO.getReporte(tipo, fechaInicio, fechaFin, cc);
        }
    }
}
