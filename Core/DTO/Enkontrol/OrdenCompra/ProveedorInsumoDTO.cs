using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class ProveedorInsumoDTO
    {
        public int insumo { get; set; }
        public string insumoDesc { get; set; }
        public int proveedor { get; set; }
        public string proveedorDesc { get; set; }
        public string direccion { get; set; }
        public string telefono1 { get; set; }
        public string email { get; set; }
        public string estado { get; set; }
        public string categoria { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }
        public string ccUltimaCompra { get; set; }
        public int numeroUltimaCompra { get; set; }
        public DateTime? fechaUltimaCompra { get; set; }
        public string fechaUltimaCompraString { get; set; }
        public decimal ultimoPrecio { get; set; }
        public string proyecto { get; set; }
        public string proyectoDesc { get; set; }
    }
}
