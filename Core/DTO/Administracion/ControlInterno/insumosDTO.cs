using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.ControlInterno
{
    public class insumosDTO
    {
        
        public int numInsumoArrendadora { get; set; }
        public int numInsumoConstruplan { get; set; }
        public string almacen { get; set; }
        public string almacenNombre { get; set; }
        public string desInsumoConstruplan { get; set; }
        public string desInsumoArrendadora { get; set; }
        public decimal cantidadInsumos { get; set; }
        public int locacion { get; set; }
        
    }
}
