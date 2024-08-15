using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class EventoCalendarioDTO
    {
        public int consecutivoEvento { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public DateTime fechaStart { get; set; }
        public DateTime fechaEnd { get; set; }
        public string classNames { get; set; }
        public string backgroundColor { get; set; }
        public ClasificacionCursoEnum clasificacion { get; set; }
    }
}
