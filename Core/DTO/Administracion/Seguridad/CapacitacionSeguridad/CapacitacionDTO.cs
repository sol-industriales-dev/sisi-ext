using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class CapacitacionDTO
    {
        public int id { get; set; }
        public int cursoID { get; set; }
        public DateTime fechaCapacitacion { get; set; }
        public bool validacion { get; set; }
    }
}
