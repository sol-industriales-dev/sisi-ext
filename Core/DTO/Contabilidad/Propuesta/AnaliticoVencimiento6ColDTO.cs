using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta
{
    public class AnaliticoVencimiento6ColDTO
    {
        public int numpro { get; set; }
        public int factura { get; set; }
        public string cc { get; set; }
        public int tm { get; set; }
        public string moneda { get; set; }
        public DateTime fechaFactura { get; set; }
        public DateTime fechaVence { get; set; }
        public decimal porVencer { get; set; }
        public decimal dias7 { get; set; }
        public decimal dias14 { get; set; }
        public decimal dias30 { get; set; }
        public decimal dias45 { get; set; }
        public decimal dias60 { get; set; }
        public decimal dias61 { get; set; }
    }
}
