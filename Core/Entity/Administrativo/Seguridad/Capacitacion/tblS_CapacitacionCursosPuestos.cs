using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionCursosPuestos
    {
        public int id { get; set; }
        public int puesto_id { get; set; }
        public bool estatus { get; set; }
        public int curso_id { get; set; }
        public virtual tblS_CapacitacionCursos curso { get; set; }
        public int division { get; set; }
    }
}
