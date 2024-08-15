using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class EmpleadoRelacionCursosDTO
    {
        public int claveEmpleado { get; set; }
        public string nombre { get; set; }
        public string puesto { get; set; }
        public List<Tuple<string, string>> cursos { get; set; }
    }
}
