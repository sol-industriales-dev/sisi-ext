using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class causasIncidenciasDTO
    {

        public decimal alturas { get; set; }
        public decimal corteSoldadura { get; set; }
        public decimal espaciosConfinados { get; set; }
        public decimal excavaciones { get; set; }
        public decimal controlEnergias { get; set; }
        public decimal manejoDefensivo { get; set; }
        public decimal manipulacionCargas { get; set; }
        public decimal estabilizacionTaludez { get; set; }
        public decimal sustanciasQuimicas { get; set; }
        public decimal voladura { get; set; }
        public decimal nd { get; set; }
    }
}
