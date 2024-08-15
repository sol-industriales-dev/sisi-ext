using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Cheque
{
    public class CapturaCheque
    {
        public string tipoPoliza { get; set; }
        public string estatus { get; set; }
        public decimal cargo { get; set; }
        public string poliza { get; set; }
        public string Generada { get; set; }
        public decimal abonos { get; set; }
        public string cta { get; set; }
        public string interfase { get; set; }
        public string TM { get; set; }
        public string Concepto { get; set; }

    }
}
