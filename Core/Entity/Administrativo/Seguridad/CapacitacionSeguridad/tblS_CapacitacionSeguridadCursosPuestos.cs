using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadCursosPuestos
    {
        public int id { get; set; }
        public int puesto_id { get; set; }
        public bool estatus { get; set; }
        public int curso_id { get; set; }
        public virtual tblS_CapacitacionSeguridadCursos curso { get; set; }
        public int division { get; set; }
    }
}
