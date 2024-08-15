using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Reportes
{
    public class pruebaDto
    {
        public decimal noEconomico { get; set; }
        public decimal precio { get; set; }
        public int tipo { get; set; }
        public string descripcion { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }
    }
}
