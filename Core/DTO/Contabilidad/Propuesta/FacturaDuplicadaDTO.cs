using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta
{
    public class FacturaDuplicadaDTO
    {
        public int numpro { get; set; }
        public int factura { get; set; }
        public string serie { get; set; }
        public string xx { get; set; }
        public string concepto { get; set; }
        public decimal monto { get; set; }
        public int year { get; set; }
        public int mes { get; set; }
        public int poliza { get; set; }
        public string tp { get; set; }
        public int linea { get; set; }
        public decimal tipoCambio { get; set; }
        public string cc { get; set; }
        public int referenciaoc { get; set; }
        public int tm { get; set; }
    }
}
