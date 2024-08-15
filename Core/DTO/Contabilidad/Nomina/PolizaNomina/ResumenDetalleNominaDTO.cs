using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina.PolizaNomina
{
    public class ResumenDetalleNominaDTO
    {
        public int nominaId { get; set; }
        public string cta { get; set; }
        public string concepto { get; set; }
        public decimal monto { get; set; }
        public int cuentaId { get; set; }
    }
}
