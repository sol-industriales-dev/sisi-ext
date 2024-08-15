using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Reportes
{
    public class tenecoDTO
    {
        public string economico { get; set; }
        public string descripcion { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string concepto { get; set; }
        public int year { get; set; }
        public int mes { get; set; }
        public decimal monto { get; set; }
    }
}
