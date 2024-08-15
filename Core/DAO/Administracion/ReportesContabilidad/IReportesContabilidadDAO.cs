using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.ReportesContabilidad
{
    public interface IReportesContabilidadDAO
    {
        Dictionary<string, object> cargarAuxiliarEnkontrol(DateTime fechaInicio, DateTime fechaFin, string ctaInicio, string ctaFin, List<string> areaCuenta);
        Dictionary<string, object> getListaAC();
        Dictionary<string, object> GetCuentas(string term);
    }
}