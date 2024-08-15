using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionCursosCC
    {
        public int id { get; set; }
        public int curso_id { get; set; }
        public virtual tblS_CapacitacionCursos Curso { get; set; }
        public string cc { get; set; }
        public int empresa { get; set; }
        public int division { get; set; }
        public bool estatus { get; set; }
    }
}
