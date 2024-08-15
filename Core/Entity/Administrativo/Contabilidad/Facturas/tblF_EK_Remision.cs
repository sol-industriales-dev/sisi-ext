using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Facturas
{
    public class tblF_EK_Remision
    {
        public int id { get; set; }
        public string folioPrefactura { get; set; }
        public int sucursal { get; set; }
        public int remision { get; set; }
        public DateTime fecha { get; set; }
        public string rfc { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string ciudad { get; set; }
        public string cp { get; set; }
        public int telefono { get; set; }
        public string transporte { get; set; }
        public int talon { get; set; }
        public string consignado { get; set; }
        public string observaciones { get; set; }
        public int moneda { get; set; }
        public decimal tipo_cambio { get; set; }
        public decimal sub_total { get; set; }
        public decimal iva { get; set; }
        public decimal total { get; set; }
        public decimal porcent_iva { get; set; }
        public decimal porcent_descto { get; set; }
        public int elaboro { get; set; }
        public string tipo_flete { get; set; }
        public decimal descuento { get; set; }
        public int factura { get; set; }
        public string estatus { get; set; }
        public string entregado { get; set; }
        public int pedido { get; set; }
        public decimal retencion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; } 
    }
}
