using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas.SubContratista
{
    public class subContratistaDashboardDTO
    {
        public string centroCostos { get; set; }
        public string centroCostosName { get; set; }
        public string noContrato { get; set; }
        public string evaluador { get; set; }
        public string nombreProyecto { get; set; }
        public int numContratista { get; set; }
        public string comentarios { get; set; }
        public int idRespuestaEncuesta { get; set; }
        public string acciones { get; set; }

    }
}
