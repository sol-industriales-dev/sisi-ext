using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Subcontratistas
{
    public class CalendarioEvaluacionDTO
    {

        public string centroCosto { get; set; }
        public int idSubContratista { get; set; }
        public string SubContratista { get; set; }
        public int idAsignacion { get; set; }
        public int idContrato { get; set; }
        public DateTime fecha { get; set; }
        public bool registroActivo { get; set; }

    }
}
