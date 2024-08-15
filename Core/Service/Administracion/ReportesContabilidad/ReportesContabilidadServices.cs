using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Core.DAO.ReportesContabilidad;
using Core.DTO.Principal.Generales;

namespace Core.Service.ReportesContabilidad
{
    public class ReportesContabilidadServices : IReportesContabilidadDAO
    {
        #region Atributos
        private IReportesContabilidadDAO ReportesContabilidad;
        #endregion

        #region Propiedades
        public IReportesContabilidadDAO ReportesContabilidadDAO
        {
            get { return ReportesContabilidad; }
            set { ReportesContabilidad = value; }
        }
        #endregion

        #region Constructores
        public ReportesContabilidadServices(IReportesContabilidadDAO ReportesContabilidadDAO)
        {
            this.ReportesContabilidadDAO = ReportesContabilidadDAO;
        }
        #endregion

        public Dictionary<string, object> cargarAuxiliarEnkontrol(DateTime fechaInicio, DateTime fechaFin, string ctaInicio, string ctaFin, List<string> areaCuenta) 
        {
            return ReportesContabilidad.cargarAuxiliarEnkontrol(fechaInicio, fechaFin, ctaInicio, ctaFin, areaCuenta);
        }
        public Dictionary<string, object> getListaAC()
        {
            return ReportesContabilidad.getListaAC();
        }

        public Dictionary<string, object> GetCuentas(string term)
        {
            return ReportesContabilidad.GetCuentas(term);
        }
    }
}
