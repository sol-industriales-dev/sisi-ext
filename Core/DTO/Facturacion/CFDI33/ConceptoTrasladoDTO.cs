using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Facturacion.CFDI33
{
    public class ConceptoTrasladoDTO
    {
        /// <summary>
        /// Base para el cálculo del impuesto
        /// </summary>
        public decimal ImpuestoBase { get; set; }
        /// <summary>
        /// Clave impuesto trasladado (Catálogo: c_Impuesto)
        /// </summary>
        public string ClaveImpuesto { get; set; }
        /// <summary>
        /// Clave tipo de factor (Catálogo: c_TipoFactor)
        /// </summary>
        public string ClaveTipoFactor { get; set; }
        /// <summary>
        /// Tasa o cuota del impuesto
        /// </summary>
        public decimal Cuota { get; set; }
        /// <summary>
        /// Importe del impuesto
        /// </summary>
        public decimal ImporteImpuesto { get; set; }
    }
}
