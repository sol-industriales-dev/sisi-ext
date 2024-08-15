using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Facturacion
{
    public class CiaSucursalDTO
    {
        public string calle { get; set; }
        public string ciudad { get; set; }
        public string codigo_postal { get; set; }
        public string nombre { get; set; }
        public string no_exterior { get; set; }
        public string no_interior { get; set; }
        public string colonia { get; set; }
    }
}
