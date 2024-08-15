using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina.ReporteRangoCC
{
    public class polizaOcsiReporteDTO
    {
        public List<polizaOcsiDTO> tabla { get; set; }
        public string nombreEmpresa { get; set; }
        public string periodo { get; set; }
        public decimal totalCargos { get; set; }
        public decimal totalAbonos { get; set; }
        public string tipoBanco { get; set; }
    }
}
