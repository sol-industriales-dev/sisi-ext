using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class ListaAsistentesReporteDTO
    {
        public string claveEmpleado { get; set; }
        public string nombreEmpleado { get; set; }
        public string puestoDesc { get; set; }
        public string fechaVencimientoString { get; set; }
    }
}
