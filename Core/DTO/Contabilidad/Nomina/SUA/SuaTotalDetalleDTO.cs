using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina.SUA
{
    public class SuaTotalDetalleDTO
    {
        public int suaID { get; set; }
        public string cc { get; set; }
        public string ac { get; set; }
        public string estado { get; set; }
        public string registroPatronal { get; set; }
        public string descripcionCC { get; set; }
        public decimal imssPatronal { get; set; }
        public decimal imssObrero { get; set; }
        public decimal rcvPatronal { get; set; }
        public decimal rcvObrero { get; set; }
        public decimal infonavit { get; set; }
        public decimal amortizacion { get; set; }
        public decimal subtotal { get; set; }
        public decimal isn { get; set; }
        public decimal total { get; set; }
    }
}
