using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas
    {
        public int id { get; set; }
        public string propuesta { get; set; }
        public string rutaEvidencia { get; set; }
        public int evaluador { get; set; }
        public bool solventada { get; set; }
        public int cicloRegistroID { get; set; }
        public bool estatus { get; set; }
    }
}
