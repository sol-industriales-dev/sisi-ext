using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadCursosCC
    {
        public int id { get; set; }
        public int curso_id { get; set; }
        public virtual tblS_CapacitacionSeguridadCursos Curso { get; set; }
        public string cc { get; set; }
        public int empresa { get; set; }
        public int division { get; set; }
        public bool estatus { get; set; }
    }
}
