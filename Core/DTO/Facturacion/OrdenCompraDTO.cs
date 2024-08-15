using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Facturacion
{
    public class OrdenCompraDTO
    {
        public string numero { get; set; }
        public string partida { get; set; }
        public int insumo { get; set; }
        public DateTime fecha_entrega { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal importe { get; set; }
        public string descripcion { get; set; }
    }
}
