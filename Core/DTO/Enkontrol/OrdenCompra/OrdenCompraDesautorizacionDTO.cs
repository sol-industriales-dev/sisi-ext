using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class OrdenCompraDesautorizacionDTO
    {
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public int numero { get; set; }
        public int proveedor { get; set; }
        public string proveedorNom { get; set; }
        public decimal total { get; set; }
        public decimal tipo_cambio { get; set; }
        public int moneda { get; set; }
        public DateTime fecha { get; set; }
        public int num_requisicion { get; set; }
    }
}
