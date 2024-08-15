using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Poliza
{
    public class PolizasDTO
    {
        public int year { get; set; }
        public int mes { get; set; }
        public int poliza { get; set; }
        public string tp { get; set; }
        public DateTime fechapol { get; set; }
        public decimal cargos { get; set; }
        public decimal abonos { get; set; }
        public decimal cargosUs { get; set; }
        public decimal abonosUs { get; set; }
        public string generada { get; set; }
        public string status { get; set; }
        public string status_lock { get; set; }
        public string concepto { get; set; }
    }
}
