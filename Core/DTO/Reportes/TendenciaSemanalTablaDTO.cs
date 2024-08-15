using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Reportes
{
    public class TendenciaSemanalTablaDTO
    {
        public string concepto { get; set; }
        public decimal monto { get; set; }
        public decimal montoMensual { get; set; }
    }
}
