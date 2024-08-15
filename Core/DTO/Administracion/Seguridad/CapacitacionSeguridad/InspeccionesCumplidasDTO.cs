using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class InspeccionesCumplidasDTO
    {
        public int totalCiclos { get; set; }
        public int mes { get; set; }
        public string mesDesc { get; set; }
        public int criticos { get; set; }
        public string criticosDesc { get; set; }
        public int medios { get; set; }
        public string mediosDesc { get; set; }
        public int bajos { get; set; }
        public string bajosDesc { get; set; }
    }
}
