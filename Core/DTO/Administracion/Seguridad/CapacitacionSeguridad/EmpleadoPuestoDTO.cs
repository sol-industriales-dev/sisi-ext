using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class EmpleadoPuestoDTO
    {
        public string claveEmpleado { get; set; }
        public string nombreEmpleado { get; set; }
        public string fechaAlta { get; set; }
        public string puestoEmpleado { get; set; }
        public int puestoID { get; set; }
        public string ccID { get; set; }
        public string cc { get; set; }
        public int empresa { get; set; }
    }
}
