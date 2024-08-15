using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class EmpleadoPendienteLiberacionDTO
    {
        public int idBaja { get; set; }
        public int clave_empleado { get; set; }
        public string nombreEmpleado { get; set; }
        public string rfc { get; set; }
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public string puestoDesc { get; set; }
        public string comentario { get; set; }
    }
}
