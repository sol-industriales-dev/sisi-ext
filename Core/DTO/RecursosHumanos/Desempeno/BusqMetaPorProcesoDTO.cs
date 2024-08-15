using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Desempeno
{
    public class BusqMetaPorProcesoDTO
    {
        public int idProceso { get; set; }
        public int idUsuario { get; set; }
        public int idEvaluacion { get; set; }
        public bool esVobo { get; set; }
        public bool esCalificar { get; set; }
        public bool esJefe { get; set; }
        public bool esRevisados { get; set; }
    }
}
