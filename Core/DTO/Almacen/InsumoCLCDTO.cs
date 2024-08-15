using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Almacen
{
    public class InsumoCLCDTO
    {
        public int id { get; set; }
        public int insumo { get; set; }
        public string insumoDesc { get; set; }
        public int proveedor { get; set; }
        public string proveedorDesc { get; set; }
        public string articulo { get; set; }
        public string unidad { get; set; }
        public decimal precio { get; set; }
    }
}
