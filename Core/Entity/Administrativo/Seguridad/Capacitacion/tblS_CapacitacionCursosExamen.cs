using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionCursosExamen
    {
        public int id { get; set; }
        public string nombreExamen { get; set; }
        public string pathExamen { get; set; }
        public bool isActivo { get; set; }
        public int tipoExamen { get; set; }

        public int curso_id  { get; set; }
        public virtual tblS_CapacitacionCursos Cursos { get; set; }
        public int division { get; set; }
    }
}
