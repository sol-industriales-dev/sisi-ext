using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina.PolizaNomina
{
    public class tblPolizaDTO
    {
        public int linea { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public int digito { get; set; }
        public string descripcion { get; set; }
        public string cc { get; set; }
        public string referencia { get; set; }
        public string concepto { get; set; }
        public int tm { get; set; }
        public decimal cargo { get; set; }
        public decimal abono { get; set; }
    }
}
