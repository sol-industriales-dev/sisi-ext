using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Facturas
{
    public class tblF_EK_Facturas_Det
    {
        public int id { get; set; }
        public string folioPrefactura { get; set; }
        public int factura { get; set; }
        public int partida { get; set; }
        public int insumo { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal importe { get; set; }
        public string unidad { get; set; }
        public decimal cant_factura { get; set; }
        public decimal precio_factura { get; set; }
        public int pedido { get; set; }
        public int ped_part { get; set; }
        public decimal porcent_descto { get; set; }
        public decimal cant_entregada { get; set; }
        public decimal cant_kg_factura { get; set; }
        public decimal cant_kg { get; set; }
        public int cia_sucursal { get; set; }
        public int numero_nc { get; set; }
        public int fac_part { get; set; }
        public int remision { get; set; }
        public string linea { get; set; }
        public string linea_nc { get; set; }
        public decimal porcen_iva_partida { get; set; }
        public decimal iva { get; set; }
        public string cc { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; } 
    }
}
