using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Facturacion
{
    public class MovcltesDTO
    {
        public int linea { get; set; }
        public string factura { get; set; }
        public string folioDigital { get; set; }
        public DateTime fecha { get; set; }
        public int numcte { get; set; }
        public decimal total { get; set; }
        public int moneda { get; set; }
        public decimal tipocambio { get; set; }
        public string cc { get; set; }
        public int tm { get; set; }
        public int poliza { get; set; }
        public string tp { get; set; }
        public string tipoMovimiento { get; set; }
        public string cliente { get; set; }
    }
}
