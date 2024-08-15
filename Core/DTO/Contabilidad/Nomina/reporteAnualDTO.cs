using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina
{
    public class reporteAnualDTO
    {
        public int mes { get; set; }
        public string nombreDeMes { get; set; }
        public int año { get; set; }
        public decimal monto { get; set; }
        public string cta { get; set; }
        public string cta1 { get; set; }
        public string cta2 { get; set; }
        public string cta3 { get; set; }
        public string concepto { get; set; }
    }
}
