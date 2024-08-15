using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta.Nomina
{
    public class NominaPolizaDTO
    {
        public int year { get; set; }
        public int mes { get; set; }
        public string tp { get; set; }
        public int poliza { get; set; }
        public int linea { get; set; }
        public int sscta { get; set; }
        public int tm { get; set; }
        public string cc { get; set; }
        public string concepto { get; set; }
        public decimal monto { get; set; }
        public decimal? retencion { get; set; }
        public string referencia { get; set; }
        public DateTime fechapol { get; set; }
    }
}
