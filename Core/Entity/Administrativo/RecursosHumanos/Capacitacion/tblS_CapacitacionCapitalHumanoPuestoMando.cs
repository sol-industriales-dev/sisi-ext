using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Capacitacion
{
    public class tblS_CapacitacionCapitalHumanoPuestoMando
    {
        public int id { get; set; }
        public int puesto { get; set; }
        public string descripcion { get; set; }
        public int mando { get; set; }
        public bool estatus { get; set; }
    }
}
