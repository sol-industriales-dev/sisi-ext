using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta.Validacion
{
    public class FacturaXmlDTO
    {
        public Int64 Folio { get; set; }
        public string Serie { get; set; }
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TipoCambio { get; set; }
        public decimal Descuento { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Version { get; set; }
        public decimal Iva { get; set; }
        public decimal TotalImpuestosTrasladados { get; set; }
        public string MetodoPago { get; set; }
        public string FormaPago { get; set; }
        public string UsoCFDI { get; set; }
        public string UUID { get; set; }
        public DateTime FechaTimbrado { get; set; }
        public string RFCEmisor { get; set; }
        public string nombreEmisor { get; set; }
        public string RFCRecepcion { get; set; }
        public string nombreReceptor { get; set; }
        public string TipoComprobante { get; set; }
        public string Moneda { get; set; }
        public string CondicionesDePago { get; set; }
        public string Certificado { get; set; }
        public string Descripcion { get; set; }
    }
}
