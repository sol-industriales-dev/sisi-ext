using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad
{
    public class CadenaProductivaDTO
    {
        public string factura { get; set; }
        public string concepto { get; set; }
        public decimal monto { get; set; }
        public decimal iva { get; set; }
        public string tipoCambio { get; set; }
        
    }
}
