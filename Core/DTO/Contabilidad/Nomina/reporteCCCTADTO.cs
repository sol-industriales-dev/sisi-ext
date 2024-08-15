using Core.DTO.Contabilidad.Nomina.ReporteCentroCuenta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina
{
    public class reporteCCCTADTO
    {

        public string cta { get; set; }
        public string concepto { get; set; }
        public List<ReporteCentroCuentaDTO> lstCentros { get; set; }
        public string total { get; set; }

    }
}
