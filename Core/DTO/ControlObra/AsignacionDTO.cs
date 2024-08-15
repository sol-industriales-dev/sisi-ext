using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class AsignacionDTO
    {
        public int id { get; set; }
        public int contrato_id { get; set; }
        public DateTime fechaInicial { get; set; }
        public string fechaInicialString { get; set; }
        public DateTime fechaFinal { get; set; }
        public string fechaFinalString { get; set; }
        public List<EvaluacionAsignacionDTO> evaluaciones { get; set; }
    }
}
