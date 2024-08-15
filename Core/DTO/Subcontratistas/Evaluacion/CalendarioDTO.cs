using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Subcontratistas.Evaluacion
{
    public class CalendarioDTO
    {
        public int id { get; set; }
        public string nombreEvaluacion { get; set; }
        public DateTime fechaInicial { get; set; }
        public DateTime fechaFinal { get; set; }
        public DateTime fecha7Dias { get; set; }
        public DateTime fechaInicialEjecutable { get; set; }
        public DateTime fechaFinalEjecutable { get; set; }
        public DateTime? fechaEvaluacion { get; set; }
        public bool esEvaluacionActual { get; set; }
        public string color { get; set; }
    }
}
