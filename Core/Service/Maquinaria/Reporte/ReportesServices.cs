using Core.DAO.Maquinaria.Reporte;
using Core.DTO.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Reporte
{
    public class ReportesServices : IReportesInternosDAO
    {
        #region Atributos
        private IReportesInternosDAO m_reportesInternosDAO;
        #endregion
        #region Propiedades
        public IReportesInternosDAO RepGastosDAO
        {
            get { return m_reportesInternosDAO; }
            set { m_reportesInternosDAO = value; }
        }
        #endregion
        #region Constructores
        public ReportesServices(IReportesInternosDAO reportesInternosDAO)
        {
            this.RepGastosDAO = reportesInternosDAO;
        }
        #endregion

        public ICollection<pruebaDto> getPrueba()
        {
            return this.getPrueba();
        }
    }
}
