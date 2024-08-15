using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Poliza
{
    public class CtaIvaDTO
    {
        public string sistema { get; set; }
        public int cta_iva { get; set; }
        public int scta_iva { get; set; }
        public int sscta_iva { get; set; }
        public int cta_iva_anterior { get; set; }
        public int scta_iva_anterior { get; set; }
        public int sscta_iva_anterior { get; set; }
        public int cta_iva_actual { get; set; }
        public int scta_iva_actual { get; set; }
        public int sscta_iva_actual { get; set; }
        public decimal porcentaje { get; set; }
    }
}
