using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Facturacion
{
    public class FacturaDet
    {
        public int partida { get; set; }
        public int insumo { get; set; }
        public int cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal importe { get; set; }
        public string unidad { get; set; }
        public decimal porcent_descto { get; set; }
        public decimal porc_ret { get; set; }
        public decimal calc_iva_factura { get; set; }
        public string descripcion { get; set; }
    }
}
