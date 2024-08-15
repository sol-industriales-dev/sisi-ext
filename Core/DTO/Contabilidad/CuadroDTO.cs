using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad
{
    public class CuadroDTO
    {
        public string banco { get; set; }
        public decimal importe { get; set; }
        public decimal subtotal { get; set; }
        public decimal total { get; set; }
        public string factoraje { get; set; }
        public decimal sumPagado { get; set; }
        public decimal sumNoPagado { get; set; }
    }
}
