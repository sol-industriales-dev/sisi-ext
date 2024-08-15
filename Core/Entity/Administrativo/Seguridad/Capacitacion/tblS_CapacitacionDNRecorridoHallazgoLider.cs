using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionDNRecorridoHallazgoLider
    {
        public int id { get; set; }
        public int lider { get; set; }
        public int hallazgoID { get; set; }
        public bool estatus { get; set; }
    }
}
