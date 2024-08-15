using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina
{
    public class SolicitudChequeReporteDetalleDTO
    {
        public string cantidadEmpleados { get; set; }
        public string cc { get; set; }
        public string reqGlobal { get; set; }
        public string obra { get; set; }
        public string subtotal { get; set; }
    }
}
