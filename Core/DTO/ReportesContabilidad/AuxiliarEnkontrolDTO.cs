using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ReportesContabilidad
{
    public class AuxiliarEnkontrolDTO
    {
        public int year { get; set; }
        public int mes { get; set; }
        public int poliza { get; set; }
        public string tp { get; set; }
        public int linea { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string referencia { get; set; }
        public string concepto { get; set; }
        public string cc { get; set; }
        public decimal cargo { get; set; }
        public decimal abono { get; set; }
        public int itm { get; set; }
        public string areaCuenta { get; set; }
        public DateTime fechapol { get; set; }
        public string fechapolStr { get; set; }
    }
}
