using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridad_PCAdministracionInstructores
    {
        public int id { get; set; }
        public string cveEmpleado { get; set; }
        public int grupo { get; set; }
        public DateTime? fechaInicio { get; set; }
        public int division { get; set; }
        public int empresa { get; set; }
        public bool instructor { get; set; }
        public int tematica { get; set; }
        public bool esActivo { get; set; }
    }
}
