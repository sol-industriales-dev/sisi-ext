using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Facturas
{
    public class tblF_EK_Pedidos_Det
    {
        public int id { get; set; }
        public string folioPrefactura { get; set; }
        public int pedido { get; set; }
        public int partida { get; set; }
        public int insumo { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal importe { get; set; }
        public string unidad { get; set; }
        public string estatus { get; set; }
        public decimal porcent_descto { get; set; }
        public decimal cant_pedido { get; set; }
        public decimal precio_pedido { get; set; }
        public decimal cant_facturada { get; set; }
        public decimal factor_uni { get; set; }
        public DateTime fec_entrega { get; set; }
        public decimal cant_cancelada { get; set; }
        public string muestra { get; set; }
        public int usuario { get; set; }
        public DateTime fecha_hora { get; set; }
        public decimal cant_kg { get; set; }
        public decimal cant_kg_cancelada { get; set; }
        public decimal cant_kg_facturada { get; set; }
        public decimal cant_kg_pedido { get; set; }
        public decimal cant_x_surtir { get; set; }
        public decimal cant_en_produc { get; set; }
        public decimal cant_x_embarc { get; set; }
        public decimal cant_produccion { get; set; }
        public decimal cant_entregada { get; set; }
        public decimal cant_remision { get; set; }
        public decimal porcen_iva_partida { get; set; }
        public string linea { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; } 
    }
}
