using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte
{
    public class ReporteCargoMensualNominaCCDTO
    {
        public string proyecto { get; set; }
        public string mes { get; set; }
        public string año { get; set; }
        public string horasHombreTotales { get; set; }
        public string costoSocialTotal { get; set; }
        public List<ReporteMaquinaNominaMensualCCDTO> listaMaquinas { get; set; }
    }
}
