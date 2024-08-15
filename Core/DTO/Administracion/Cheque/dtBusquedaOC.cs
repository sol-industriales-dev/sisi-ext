using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Cheque
{
    public class dtBusquedaOC
    {
        public int id { get; set; }
        public int numero { get; set; }
        public bool anticipo { get; set; }
        public decimal totalAnticipo { get; set; }
        public decimal total { get; set; }
        public string cc { get; set; }
        public string proveedor { get; set; }
        public int numpro { get; set; }

    }
}
