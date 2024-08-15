using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.EstadoFinanciero
{
    public class EFGraficaDetallesDTO
    {
        public string concepto { get; set; }
        public decimal monto { get; set; }
        public int anio { get; set; }
        public int mes { get; set; }
    }
}
