using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class ControlAsistenciaDTO
    {
        public int id { get; set; }
        public string ccCurso { get; set; }
        public string fechaCapacitacion { get; set; }
        public string duracion { get; set; }
        public string nombreCurso { get; set; }
        public string nombreInstructor { get; set; }
        public string lugar { get; set; }
        public string horario { get; set; }
        public string objetivos { get; set; }
        public string temasPrincipales { get; set; }
        public bool esExterno { get; set; }
        public string empresaExterna { get; set; }
        public List<AsistenteCapacitacionDTO> asistentes { get; set; }
    }
}
