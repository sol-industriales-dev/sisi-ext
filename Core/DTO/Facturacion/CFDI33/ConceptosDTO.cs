using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Facturacion.CFDI33
{
    public class ConceptosDTO
    {
        /// <summary>
        /// Clave Producto Servicio (Catálogo: c_ClaveProdServ)
        /// </summary>
        public string ClaveProdServ { get; set; }
        /// <summary>
        /// Clave o código del producto (NoIdentificacion
        /// </summary>
        public string NoIdentificacion { get; set; }
        /// <summary>
        /// Cantidad
        /// </summary>
        public int Cantidad { get; set; }
        /// <summary>
        /// Clave Unidad de medida (Catálogo: c_ClaveUnidad
        /// </summary>
        public string ClaveUnidad { get; set; }
        /// <summary>
        /// Unidad de medida
        /// </summary>
        public string UnidadMedida { get; set; }
        /// <summary>
        /// Descripción del producto
        /// </summary>
        public string Descripcion { get; set; }
        /// <summary>
        /// Importe unitario
        /// </summary>
        public decimal ImporteUnitario { get; set; }
        /// <summary>
        /// Importe Total
        /// </summary>
        public decimal ImporteTotal { get; set; }
        /// <summary>
        /// Descuento
        /// </summary>
        public decimal Descuento { get; set; }
    }
}
