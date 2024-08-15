using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class CapacitacionDTO
    {
        public int id { get; set; }
        public int cursoID { get; set; }
        public DateTime fechaCapacitacion { get; set; }
        public bool validacion { get; set; }
        public bool DC3 { get; set; }
        public int clave_empleado { get; set; }
    }
}
