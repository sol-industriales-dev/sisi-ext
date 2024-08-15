using Core.Enum.Administracion.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionCursosMando
    {
        public int id { get; set; }
        public int curso_id { get; set; }
        public virtual tblS_CapacitacionCursos Curso { get; set; }
        public int mando { get; set; }
        
        public int division { get; set; }
        public bool estatus { get; set; }

        [NotMapped]
        public MandoEnum filMando { get; set; }
    }
}
