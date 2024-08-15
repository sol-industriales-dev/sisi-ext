using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Tablas.Proveedor
{
    public class sp_proveedoresDTO
    {
        public int numpro { get; set; }
        public string nomCorto { get; set; }
        public string nombre { get; set; }
        public int moneda { get; set; }

        public string descripcionMoneda { get; set; }
    }
}
