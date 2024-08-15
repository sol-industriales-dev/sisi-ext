using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class AgendaInstructorDTO
    {
        public int claveEmpleado { get; set; }
        public List<DiaAgendaDTO> agenda { get; set; }
    }
}
