using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadDNRecorridoHallazgoLider
    {
        public int id { get; set; }
        public int lider { get; set; }
        public int hallazgoID { get; set; }
        public bool estatus { get; set; }
    }
}
