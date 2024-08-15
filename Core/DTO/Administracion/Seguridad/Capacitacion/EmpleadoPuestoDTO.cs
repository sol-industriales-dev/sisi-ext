using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
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
        public string nss { get; set; }
        public string curp { get; set; }
        public string cedula_cuidadania { get; set; }
        public string num_dni { get; set; }
    }
}
