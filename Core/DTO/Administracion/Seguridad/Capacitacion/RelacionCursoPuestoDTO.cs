using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class RelacionCursoPuestoDTO
    {
        public int curso_id { get; set; }
        public int puesto_id { get; set; }
        public string claveCurso { get; set; }
        public string nombreCurso { get; set; }
        public string puesto { get; set; }
        public bool aplicaAutorizacion { get; set; }
    }
}
