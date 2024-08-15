using Core.Enum.Administracion.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionDNCiclosRequeridos
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int empresa { get; set; }
        public DivisionCapacitacionEnum division { get; set; }
        public int cantidad { get; set; }
        public bool registroActivo { get; set; }
    }
}
