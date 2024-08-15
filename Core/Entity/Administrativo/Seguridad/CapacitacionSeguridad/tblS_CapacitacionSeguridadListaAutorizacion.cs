using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadListaAutorizacion
    {
        public int id { get; set; }
        public string claveLista { get; set; }
        public int cursoID { get; set; }
        public string revision { get; set; }
        public int jefeDepartamento { get; set; }
        public int gerenteProyecto { get; set; }
        public int coordinadorCSH { get; set; }
        public int secretarioCSH { get; set; }
        public int seguridad { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int division { get; set; }
        public bool estatus { get; set; }
    }
}
