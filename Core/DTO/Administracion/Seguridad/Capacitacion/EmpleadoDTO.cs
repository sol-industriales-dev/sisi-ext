using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class EmpleadoDTO
    {
        public int clave_empleado { get; set; }
        public string nombre { get; set; }
        public string clave_depto { get; set; }
        public string cc_contable { get; set; }
        public string estatus_empleado { get; set; }
        public int empresa { get; set; }
    }
}
