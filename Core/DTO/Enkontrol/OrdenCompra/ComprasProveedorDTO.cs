using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class ComprasProveedorDTO
    {
        public string cc { get; set; }
        public int numero { get; set; }
        public DateTime fecha { get; set; }
        public string fechaString { get; set; }
        public int proveedor { get; set; }
        public string proveedorDesc { get; set; }
        public string detalle { get; set; }
        public decimal sub_total { get; set; }
        public decimal total { get; set; }
        public string moneda { get; set; }
        public string subTotalPesos { get; set; }
        public string subTotalDolares { get; set; }
        public string totalPesos { get; set; }
        public string totalDolares { get; set; }
        public string compradorDesc { get; set; }
        public string tipoCompraDesc { get; set; }
    }
}
