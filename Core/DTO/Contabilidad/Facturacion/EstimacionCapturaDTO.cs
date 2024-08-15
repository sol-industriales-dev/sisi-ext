using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Facturacion
{
    public class EstimacionCapturaDTO
    {
        public int numcte { get; set; }
        public string cc { get; set; }
        public string factura { get; set; }
        public DateTime fecha { get; set; }
        /// <summary>
        /// fecha de vencimiento
        /// </summary>
        public DateTime fechavenc { get; set; }
        /// <summary>
        /// descripcion
        /// </summary>
        public string linea { get; set; }
        public decimal tipocambio { get; set; }
        public decimal estimacion { get; set; }
        public decimal anticipo { get; set; }
        public decimal vencido { get; set; }
        public decimal pronostico { get; set; }
        public decimal cobrado { get; set; }
    }
}
