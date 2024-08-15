using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Facturacion.CFDI33
{
    public class ImpuestosDTO
    {
        /// <summary>
        /// Total de los impuestos retenidos
        /// </summary>
        public decimal TotalRetenido { get; set; }
        /// <summary>
        /// Total de los impuestos trasladados
        /// </summary>
        public decimal TotalTrasladado { get; set; }        
    }
}
