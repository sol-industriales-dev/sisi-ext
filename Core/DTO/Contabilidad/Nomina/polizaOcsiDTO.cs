using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina
{
    public class polizaOcsiDTO
    {
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string cc { get; set; }
        public string ccDescripcion { get; set; }
        public string concepto { get; set; }
        public decimal cargo { get; set; }
        public decimal abono { get; set; }
        public string observaciones { get; set; }
        public string areaCuenta { get; set; }
    }
}
