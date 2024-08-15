using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Facturacion
{
    public class PartidaDTO
    {
        public int Partida { get; set; }
        public int Insumo { get; set; }
        public string Descripcion { get; set; }
        public string Unidad { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Descuento { get; set; }
        public decimal DescuentoDinero { get; set; }
        public decimal Importe { get; set; }
        public string IvaRetenido { get; set; }
        public int? partida_factura { get; set; }
        public int? ped_part { get; set; }
        public int? cant_facturada { get; set; }
        public string linea { get; set; }

    }
}
