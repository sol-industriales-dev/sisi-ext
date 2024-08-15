using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Facturacion
{
    public class pedidoDTO
    {
        public int pedido { get; set; }
        public int numcte { get; set; }
        public int sucursal { get; set; }
        public DateTime fecha { get; set; }
        public string requisicion { get; set; }
        public int vendedor { get; set; }
        public int cond_pago { get; set; }
        public string moneda { get; set; }
        public decimal tipo_cambio { get; set; }
        public decimal sub_total { get; set; }
        public decimal iva { get; set; }
        public decimal total { get; set; }
        public decimal porcent_iva { get; set; }
        public string tipo { get; set; }
        public decimal porcent_descto { get; set; }
        public decimal descuento { get; set; }
        public string estatus { get; set; }
        public string condicion_entrega { get; set; }
        public string tipo_flete { get; set; }
        public decimal lista_precios { get; set; }
        public string status_autorizado { get; set; }
        public int zona { get; set; }
        public string obs { get; set; }
        public string otros_cond_pago { get; set; }
        public string usuario { get; set; }
        public DateTime fecha_hora { get; set; }
        public string tipo_pedido { get; set; }
        public string cc { get; set; }
        public int tm { get; set; }
        public int elaboro { get; set; }
        public string cia_sucursal { get; set; }
        public string tipo_credito { get; set; }
        public decimal retencion { get; set; }
        public decimal aplica_total_antes_iva { get; set; }
        public decimal total_dec { get; set; }
    }
}
