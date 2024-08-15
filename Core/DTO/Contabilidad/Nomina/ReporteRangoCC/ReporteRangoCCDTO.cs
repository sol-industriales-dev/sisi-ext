using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina.ReporteRangoCC
{
    public class ReporteRangoCCDTO
    {
        public int mes { get; set; }
        public string nombreDeMes { get; set; }
        public int año { get; set; }
        public decimal monto { get; set; }
        public string cc { get; set; }
        public string nombreCC { get; set; }
        public string area { get; set; }
        public string cuenta { get; set; }
    }
}
