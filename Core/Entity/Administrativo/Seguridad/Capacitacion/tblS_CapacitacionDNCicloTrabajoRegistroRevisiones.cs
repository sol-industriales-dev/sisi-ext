using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionDNCicloTrabajoRegistroRevisiones
    {
        public int id { get; set; }
        public int criterioID { get; set; }
        public bool acredito { get; set; }
        public int cicloRegistroID { get; set; }
        public bool estatus { get; set; }
    }
}
