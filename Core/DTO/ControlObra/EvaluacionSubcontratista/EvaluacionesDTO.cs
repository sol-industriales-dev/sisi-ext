using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra.EvaluacionSubcontratista
{
    public class EvaluacionesDTO
    {
        public string cc { get; set; }
        public string nombreEvaluacion { get; set; }
        public bool esActivo { get; set; }
        public int id { get; set; }
        public int statusAutorizacion { get; set; }
        public int evaluacionAnteriorid { get; set; }
        public bool statusVobo { get; set; }
        public DateTime fechaInicial { get; set; }
        public DateTime fechaFinal { get; set; }
        public List<EvaluacionConAsignacionDTO> evaluacionesConAsignacion { get; set; }
    }
}
