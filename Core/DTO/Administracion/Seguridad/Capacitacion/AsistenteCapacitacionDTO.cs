using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class AsistenteCapacitacionDTO
    {
        public int numeroAsistente { get; set; }
        public string claveEmpleado { get; set; }
        public string nombreEmpleado { get; set; }
        public string puesto { get; set; }
        public string cc { get; set; }
        public string departamento { get; set; }
        public string razonSocial { get; set; }
        public string rfc { get; set; }
    }
}
