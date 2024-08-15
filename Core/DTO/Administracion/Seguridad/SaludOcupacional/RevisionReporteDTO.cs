using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.SaludOcupacional
{
    public class RevisionReporteDTO
    {
        public string consecutivo { get; set; }
        public string diagnostico { get; set; }
        public string tratamiento { get; set; }
        public string comentarios { get; set; }
        public string fecha { get; set; }
    }
}
