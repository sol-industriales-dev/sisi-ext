using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.TransferenciasBancarias
{
    public class MovimientoPolizaDTO
    {
        public int poliza { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string referencia { get; set; }
        public int numpro { get; set; }
        public decimal monto { get; set; }
    }
}
