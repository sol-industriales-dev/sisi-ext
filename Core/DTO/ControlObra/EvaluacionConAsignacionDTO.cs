using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class EvaluacionConAsignacionDTO
    {
        public int id { get; set; }
        public bool evaluacionPendiente { get; set; }
        public List<bool> evaluacionesPendiente { get; set; }
    }
}
