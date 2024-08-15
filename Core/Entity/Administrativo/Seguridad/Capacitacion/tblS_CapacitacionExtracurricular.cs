using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionExtracurricular
    {
        public int id { get; set; }
        public int claveEmpleado { get; set; }
        public string nombre { get; set; }
        public decimal duracion { get; set; }
        public DateTime fecha { get; set; }
        public DateTime? fechaExpiracion { get; set; }
        public string rutaEvidencia { get; set; }
        public bool esActivo { get; set; }
        public int division { get; set; }
    }
}
