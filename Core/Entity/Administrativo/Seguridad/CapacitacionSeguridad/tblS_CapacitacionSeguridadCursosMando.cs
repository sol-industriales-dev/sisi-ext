using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadCursosMando
    {
        public int id { get; set; }
        public int curso_id { get; set; }
        public virtual tblS_CapacitacionSeguridadCursos Curso { get; set; }
        public int mando { get; set; }
        public int division { get; set; }
        public bool estatus { get; set; }
    }
}
