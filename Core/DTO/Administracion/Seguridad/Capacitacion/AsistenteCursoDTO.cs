using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class AsistenteCursoDTO
    {
        public int id { get; set; }
        public bool aprobado { get; set; }
        public decimal calificacion { get; set; }
    }
}
