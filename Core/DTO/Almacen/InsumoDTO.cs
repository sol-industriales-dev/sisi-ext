using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Almacen
{
    public class InsumoDTO
    {
        public string insumoNumero { get; set; }
        public string insumoDescripcion { get; set; }
        public decimal insumoCantidad { get; set; }
        public string almacenNumero { get; set; }
        public string almacenNombre { get; set; }
    }
}
