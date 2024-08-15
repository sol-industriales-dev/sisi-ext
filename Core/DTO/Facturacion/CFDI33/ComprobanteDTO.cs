using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Facturacion.CFDI33
{
    public class ComprobanteDTO
    {
        public ComprobanteDTO()
        {
            Version = "3.3";
            Fecha = DateTime.Now;
        }
        /// <summary>
        /// Versión del estandar CFDI
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// Serie
        /// </summary>
        public string Serie { get; set; }
        /// <summary>
        /// Folio
        /// </summary>
        public string Folio { get; set; }
        /// <summary>
        /// Fecha y Hora
        /// </summary>
        public DateTime Fecha { get; set; }
        /// <summary>
        /// Forma de Pago (Catálogo: c_FormaPago)
        /// </summary>
        public string FormaPago { get; set; }
        /// <summary>
        /// Condiciones de pago
        /// </summary>
        public string CondPago { get; set; }
        /// <summary>
        /// Subtotal
        /// </summary>
        public decimal Subtotal { get; set; }
        /// <summary>
        /// Descuento
        /// </summary>
        public decimal Descuento { get; set; }
        /// <summary>
        /// Moneda (Catálogo: c_Moneda)
        /// </summary>
        public string Moneda { get; set; }
        /// <summary>
        /// Tipo de Cambio
        /// </summary>
        public decimal TipoCambio { get; set; }
        /// <summary>
        /// Total
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// Tipo de Comprobante (Catálogo: c_TipoDeComprobante)
        /// </summary>
        public string TipoComprobante { get; set; }
        /// <summary>
        /// Método de Pago (Catálogo: c_MetodoPago)
        /// </summary>
        public decimal MetodoPago { get; set; }
        /// <summary>
        /// Lugar de expedición (Catálogo: c_CodigoPostal)
        /// </summary>
        public string LugarExpedicion { get; set; }
        /// <summary>
        /// Confirmación
        /// </summary>
        public string Confirmacion { get; set; }
    }
}
