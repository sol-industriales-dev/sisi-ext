using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionDNRecorrido
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int area { get; set; }
        public int empresa { get; set; }
        public DateTime fecha { get; set; }
        public int realizador { get; set; }
        public bool cerrado { get; set; }
        public int division { get; set; }
        public bool estatus { get; set; }
    }
}
